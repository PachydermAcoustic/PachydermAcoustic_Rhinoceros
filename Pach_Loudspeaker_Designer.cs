// Pachyderm Acoustic - GPL version 3 or later.
// Copyright (c) 2026, Open Research in Acoustical Science and Education, Inc.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Complex = System.Numerics.Complex;
using System.Text;
using System.Xml.Linq;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;
using Pachyderm_Acoustic.Source_Constructions;
using HPoint = Hare.Geometry.Point;

namespace Pachyderm_Acoustic.UI
{
    [System.Runtime.InteropServices.Guid("E5324390-FA01-4419-AB70-12BE9F9478C2")]
    public sealed class PachDesignLoudspeakerCommand : Command
    {
        public override string EnglishName => "Design_Loudspeaker";
        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;
        private sealed class Driver
        {
            internal string Kind;
            internal int Front, Rear = -1;
            internal double[] Gain, Phase, Delay;
            internal Complex Excitation(int band, bool thirds, double frequency)
            {
                double Value(double[] values) => values[values.Length == 1 ? 0 : values.Length == 8 && thirds ? band / 3 : band];
                return Complex.FromPolarCoordinates(Math.Pow(10, Value(Gain) / 20),
                    Value(Phase) * Math.PI / 180 - 2 * Math.PI * frequency * Value(Delay) / 1000);
            }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("Experimental exterior radiation model. Model a closed cabinet or thin baffle solid, including separate planar diaphragm faces. Dipoles require front and rear diaphragm faces.");
            using (var selection = new GetObject())
            {
                selection.SetCommandPrompt("Select closed loudspeaker cabinet polysurface (including diaphragm faces)");
                selection.GeometryFilter = ObjectType.Brep;
                selection.SubObjectSelect = false;
                if (selection.Get() != GetResult.Object) return selection.CommandResult();
                RhinoObject cabinetObject = selection.Object(0).Object();
                Brep cabinet = selection.Object(0).Brep();
                if (cabinet == null || !cabinet.IsValid || !cabinet.IsSolid)
                { RhinoApp.WriteLine("The acoustic boundary must be a valid closed Brep solid. Close diaphragm openings with their radiating faces."); return Result.Failure; }

                var drivers = new List<Driver>();
                while (true)
                {
                    using (var options = new GetOption())
                    {
                        options.SetCommandPrompt("Add a modeled driver, or generate the loudspeaker balloon");
                        int circular = options.AddOption("CircularPiston");
                        int dipole = options.AddOption("CircularDipole");
                        int ribbon = options.AddOption("Ribbon");
                        int generate = options.AddOption("Generate");
                        if (options.Get() != GetResult.Option) return options.CommandResult();
                        int chosen = options.OptionIndex();
                        if (chosen == generate)
                        {
                            if (drivers.Count == 0) { RhinoApp.WriteLine("Add at least one driver."); continue; }
                            break;
                        }
                        var driver = new Driver { Kind = chosen == circular ? "CircularPiston" : chosen == dipole ? "CircularDipole" : "Ribbon" };
                        bool paired = chosen == dipole;
                        if (chosen == ribbon)
                        {
                            using (var radiation = new GetOption())
                            {
                                radiation.SetCommandPrompt("Ribbon radiation");
                                int baffled = radiation.AddOption("Baffled");
                                int open = radiation.AddOption("Dipole");
                                if (radiation.Get() != GetResult.Option) return radiation.CommandResult();
                                paired = radiation.OptionIndex() == open;
                                if (paired) driver.Kind = "RibbonDipole";
                            }
                        }
                        Result result = PickFace(doc, cabinetObject.Id, cabinet, drivers, -1, "Select front diaphragm face (Ctrl+Shift to select a face)", out driver.Front);
                        if (result != Result.Success) return result;
                        if (paired)
                        {
                            result = PickFace(doc, cabinetObject.Id, cabinet, drivers, driver.Front, "Select opposite rear diaphragm face", out driver.Rear);
                            if (result != Result.Success) return result;
                            Plane front, rear;
                            cabinet.Faces[driver.Front].TryGetPlane(out front);
                            cabinet.Faces[driver.Rear].TryGetPlane(out rear);
                            if (cabinet.Faces[driver.Front].OrientationIsReversed) front.Flip();
                            if (cabinet.Faces[driver.Rear].OrientationIsReversed) rear.Flip();
                            if (front.Normal * rear.Normal > -0.99)
                            { RhinoApp.WriteLine("A dipole pair needs opposite outward diaphragm normals."); return Result.Failure; }
                        }
                        try { ValidateDiaphragm(cabinet, driver, doc.ModelAbsoluteTolerance); }
                        catch (ArgumentException ex) { RhinoApp.WriteLine(ex.Message); return Result.Failure; }
                        result = Spectrum("Relative diaphragm velocity gain dB (one, 8 octave, or 24 third-octave values)", out driver.Gain);
                        if (result != Result.Success) return result;
                        result = Spectrum("Driver phase degrees (one, 8, or 24 values)", out driver.Phase);
                        if (result != Result.Success) return result;
                        result = Spectrum("Driver delay milliseconds (one, 8, or 24 values)", out driver.Delay);
                        if (result != Result.Success) return result;
                        drivers.Add(driver);
                        RhinoApp.WriteLine("Added {0}; {1} driver(s).", driver.Kind, drivers.Count);
                    }
                }
                bool thirds = drivers.Any(d => d.Gain.Length == 24 || d.Phase.Length == 24 || d.Delay.Length == 24);
                if (!thirds)
                {
                    using (var bands = new GetOption())
                    {
                        bands.SetCommandPrompt("Balloon frequency bands");
                        int octaves = bands.AddOption("Octave");
                        int third = bands.AddOption("ThirdOctave");
                        if (bands.Get() != GetResult.Option) return bands.CommandResult();
                        thirds = bands.OptionIndex() == third;
                    }
                }
                double[] frequencies = thirds ? Utilities.AcousticalMath.ThirdOctaveCenters() : Utilities.AcousticalMath.OctaveCenters();
                double highest = 500;
                Result rc = RhinoGet.GetNumber("Highest simulated frequency Hz (higher bands inactive)", false, ref highest, frequencies[0], frequencies[frequencies.Length - 1]);
                if (rc != Result.Success) return rc;
                highest = frequencies.Last(f => f <= highest);
                double meshEdge = 0;
                rc = RhinoGet.GetNumber("Optional maximum mesh edge in meters (0 = six samples per wavelength in each band)", false, ref meshEdge, 0, Loudspeaker_Boundary_Radiation.SoundSpeed / (6 * frequencies[0]));
                if (rc != Result.Success) return rc;
                double power = 90;
                rc = RhinoGet.GetNumber("Total loudspeaker sound power dB per active band (directivity normalized to this power)", false, ref power, -100, 200);
                if (rc != Result.Success) return rc;
                Point3d origin;
                rc = RhinoGet.GetPoint("Select loudspeaker acoustic reference point", false, out origin);
                if (rc != Result.Success) return rc;
                Vector3d forward, up;
                rc = PickDirection(origin, "Select speaker forward direction", out forward);
                if (rc != Result.Success) return rc;
                rc = PickDirection(origin, "Select speaker up direction", out up);
                if (rc != Result.Success) return rc;
                up -= forward * (up * forward);
                if (!up.Unitize()) { RhinoApp.WriteLine("Forward and up must be independent directions."); return Result.Failure; }
                Vector3d right = Vector3d.CrossProduct(forward, up);
                var frame = new Plane(origin, right, forward);
                double scale = Utilities.RCPachTools.GetModelToMetersScale(doc);
                Transform local = Transform.Scale(Point3d.Origin, scale) * Transform.PlaneToPlane(frame, Plane.WorldXY);
                using var cancellation = new CancellationTokenSource();
                EventHandler escape = (sender, args) => cancellation.Cancel();
                ProgressBox progress = null;
                bool progressClosed = false;
                bool closingProgress = false;
                RhinoApp.EscapeKeyPressed += escape;
                try
                {
                    const string initialMessage = "Preparing loudspeaker balloon...";
                    progress = new ProgressBox(initialMessage);
                    progress.change_title(initialMessage);
                    progress.Closed += (sender, args) =>
                    {
                        progressClosed = true;
                        if (!closingProgress) cancellation.Cancel();
                    };
                    progress.Show(doc);

                    using (var displayMesh = new Mesh())
                    {
                        string[] balloons = new string[frequencies.Length];
                        double[] levels = new double[frequencies.Length];
                        double worstResidual = 0;
                        int bandCount = Array.FindLastIndex(frequencies, f => f <= highest) + 1;
                        for (int band = 0; band < frequencies.Length; band++)
                        {
                            RhinoApp.Wait();
                            cancellation.Token.ThrowIfCancellationRequested();
                            if (band >= bandCount)
                            {
                                balloons[band] = OmniCode();
                                levels[band] = -300;
                                continue;
                            }

                            double frequency = frequencies[band];
                            double bandEdge = Loudspeaker_Boundary_Radiation.SoundSpeed / (6 * frequency);
                            if (meshEdge > 0) bandEdge = Math.Min(bandEdge, meshEdge);
                            string label = $"Band {band + 1}/{bandCount}, {frequency:g} Hz";
                            progress.change_title($"{label} | Meshing cabinet");
                            progress.Report(100 * band / bandCount);
                            RhinoApp.WriteLine("{0}: meshing at {1:g4} m maximum panel edge.", label, bandEdge);
                            using (Mesh mesh = BoundaryMesh(cabinet, bandEdge, scale, cancellation.Token))
                            {
                                int[] diaphragm = MapDiaphragms(mesh, cabinet, drivers, doc.ModelAbsoluteTolerance);
                                int drivenPanels = diaphragm.Count(patch => patch != 0);
                                RhinoApp.WriteLine($"{label}: {mesh.Vertices.Count:N0} nodes, {mesh.Faces.Count:N0} panels, {drivenPanels:N0} driven panels.");
                                if (!ConfirmMemory(mesh.Faces.Count)) return Result.Cancel;
                                mesh.Transform(local);
                                if (band == bandCount - 1) displayMesh.Append(mesh);
                                var panels = BoundaryPanels(mesh);
                                var solver = new Loudspeaker_Boundary_Radiation(panels);
                                Complex[] excitation = new Complex[panels.Count];
                                for (int i = 0; i < excitation.Length; i++)
                                {
                                    int patch = diaphragm[i];
                                    if (patch != 0)
                                        excitation[i] = Math.Sign(patch) * drivers[Math.Abs(patch) - 1].Excitation(band, thirds, frequency);
                                }

                                var result = SolveBand(solver, frequency, excitation, (message, bandFraction) =>
                                {
                                    progress.change_title($"{label} | {message}");
                                    progress.Report((int)(100 * (band + bandFraction) / bandCount));
                                }, cancellation.Token);
                                progress.Report(100 * (band + 1) / bandCount);
                                worstResidual = Math.Max(worstResidual, result.Residual);
                                balloons[band] = result.Code;
                                levels[band] = power;
                                double spread = result.Code.Split(new[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Max(value => double.Parse(value, Invariant));
                                RhinoApp.WriteLine("{0}: {1:F1} dB sampled attenuation range (60 dB limit), {2:P1} trace residual.",
                                    label, spread, result.Residual);
                            }
                        }
                        cancellation.Token.ThrowIfCancellationRequested();
                        progress.change_title("Saving loudspeaker balloon...");
                        var geometry = new Rhino.Geometry.Point(origin);
                        geometry.SetUserString("SourceType", "3");
                        geometry.SetUserString("Model", "Rhino Loudspeaker - Experimental Neumann Boundary");
                        geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(levels));
                        geometry.SetUserString("Phase", "0;0;0;0;0;0;0;0");
                        geometry.SetUserString("Delay", "0");
                        geometry.SetUserString("Aiming", ArraySourceConstruction.AimingString(forward, up));
                        string[] octave = Utilities.AcousticalMath.ConvertBandEnergy(balloons, false, true, levels);
                        double[] octaveCenters = Utilities.AcousticalMath.OctaveCenters();
                        for (int i = 0; i < octave.Length; i++) geometry.SetUserString("Balloon" + Math.Ceiling(octaveCenters[i]).ToString(Invariant), octave[i]);
                        if (thirds)
                            for (int i = 0; i < frequencies.Length; i++) geometry.SetUserString("BalloonThird" + frequencies[i].ToString(Invariant), balloons[i]);
                        StoreCabinet(geometry, displayMesh);
                        geometry.SetUserString("LoudspeakerDesign", Definition(cabinetObject.Id, drivers, thirds, highest, meshEdge, power).ToString());
                        geometry.SetUserString("LoudspeakerBoundaryResidual", worstResidual.ToString("R", Invariant));
                        Guid id = doc.Objects.Add(geometry, new ObjectAttributes { Name = "Acoustical Source" });
                        if (id == Guid.Empty) return Result.Failure;
                        try { ArraySourceConstruction.AddToSourceConduit(doc.Objects.FindId(id)); }
                        catch { doc.Objects.Delete(id, true); throw; }
                        doc.Objects.UnselectAll(); doc.Objects.Select(id); doc.Views.Redraw();
                        RhinoApp.WriteLine("Created loudspeaker balloon through {0} Hz. Preview shows {0} Hz. Worst independent boundary residual: {1:P1}. This experimental model needs convergence and measurement validation for production use.", highest, worstResidual);
                        return Result.Success;
                    }
                }
                catch (OperationCanceledException)
                {
                    RhinoApp.WriteLine("Loudspeaker generation cancelled.");
                    return Result.Cancel;
                }
                catch (OutOfMemoryException)
                {
                    RhinoApp.WriteLine("Could not allocate the dense boundary solve. Free memory or lower the highest frequency.");
                    return Result.Failure;
                }
                catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is AggregateException || ex is OverflowException)
                { RhinoApp.WriteLine("Loudspeaker generation failed: {0}", ex.GetBaseException().Message); return Result.Failure; }
                finally
                {
                    RhinoApp.EscapeKeyPressed -= escape;
                    closingProgress = true;
                    if (!progressClosed) progress?.Close();
                }
            }
        }

        private static (string Code, double Residual) SolveBand(Loudspeaker_Boundary_Radiation solver,
            double frequency, Complex[] excitation, Action<string, double> showProgress,
            CancellationToken cancellation)
        {
            const int stagesPerBand = 4;
            string stage = "Starting boundary solve";
            int stageIndex = -1, completed = 0, total = 0;
            var progressLock = new object();
            void Report(string nextStage, int count, int countTotal)
            {
                lock (progressLock)
                {
                    if (stage != nextStage)
                    {
                        stageIndex++;
                        stage = nextStage;
                        completed = 0;
                    }
                    completed = Math.Max(completed, count);
                    total = countTotal;
                }
            }

            var work = Task.Run(() =>
            {
                var field = solver.Solve(frequency, excitation, cancellation, Report);
                return (field.Balloon(cancellation, Report), field.BoundaryResidual);
            }, cancellation);
            var elapsed = Stopwatch.StartNew();
            while (!work.IsCompleted)
            {
                string message;
                double bandFraction;
                lock (progressLock)
                {
                    message = total > 0 ? $"{stage} {completed:N0}/{total:N0}" : stage;
                    // Factorization has no row count. Hold the bar at the start of that stage.
                    double stageFraction = total > 0 ? (double)completed / total : 0;
                    bandFraction = Math.Max(0, Math.Min(1, (stageIndex + stageFraction) / stagesPerBand));
                }
                showProgress($"{message} | {elapsed.Elapsed:c}", bandFraction);
                RhinoApp.Wait();
                Thread.Sleep(50);
            }
            return work.GetAwaiter().GetResult();
        }

        private static bool ConfirmMemory(int panelCount)
        {
            const double bytesPerGib = 1024.0 * 1024 * 1024;
            double estimate = Loudspeaker_Boundary_Radiation.EstimateDenseWorkingBytes(panelCount) / bytesPerGib;
            RhinoApp.WriteLine("Estimated dense solver workspace: {0:F2} GiB.", estimate);
            if (System.Environment.OSVersion.Platform != PlatformID.Win32NT) return true;

            var memory = new MemoryStatus { Length = (uint)Marshal.SizeOf(typeof(MemoryStatus)) };
            if (!GlobalMemoryStatusEx(ref memory)) return true;
            double available = Math.Min(memory.AvailablePhysical, memory.AvailableCommit) / bytesPerGib;
            RhinoApp.WriteLine("Available physical memory or commit headroom, whichever is lower: {0:F2} GiB.", available);
            if (estimate < available * 0.8) return true;

            using (var confirm = new GetOption())
            {
                confirm.SetCommandPrompt("Estimated solver memory approaches or exceeds available memory. Choose Proceed to try");
                int proceed = confirm.AddOption("Proceed");
                confirm.AddOption("Cancel");
                return confirm.Get() == GetResult.Option && confirm.OptionIndex() == proceed;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MemoryStatus
        {
            internal uint Length, Load;
            internal ulong TotalPhysical, AvailablePhysical, TotalCommit, AvailableCommit;
            internal ulong TotalVirtual, AvailableVirtual, AvailableExtendedVirtual;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MemoryStatus memory);

        private sealed class Aperture
        {
            internal Point3d Center;
            internal double Width, Height;
        }
        private static Aperture ApertureShape(BrepFace face, bool circular, double tolerance)
        {
            if (face.Loops.Count != 1) throw new ArgumentException("A diaphragm must have one outer boundary and no holes.");
            using (Curve boundary = face.OuterLoop.To3dCurve())
            {
                if (circular)
                {
                    Circle circle;
                    if (boundary == null || !boundary.TryGetCircle(out circle, tolerance)) throw new ArgumentException("Select a circular planar face for a circular piston or dipole.");
                    return new Aperture { Center = circle.Center, Width = 2 * circle.Radius, Height = 2 * circle.Radius };
                }
                Polyline rectangle;
                if (boundary == null || !boundary.TryGetPolyline(out rectangle) || rectangle.Count != 5 || !rectangle.IsClosed)
                    throw new ArgumentException("Select a rectangular planar diaphragm face for a ribbon.");
                double[] length = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    Vector3d a = rectangle[(i + 1) % 4] - rectangle[i], b = rectangle[(i + 2) % 4] - rectangle[(i + 1) % 4];
                    length[i] = a.Length;
                    if (!a.Unitize() || !b.Unitize() || Math.Abs(a * b) > 1e-6) throw new ArgumentException("Ribbon diaphragm boundaries must have four right angles.");
                }
                if (Math.Abs(length[0] - length[2]) > tolerance || Math.Abs(length[1] - length[3]) > tolerance) throw new ArgumentException("Ribbon diaphragm edges must form a rectangle.");
                return new Aperture { Center = (rectangle[0] + rectangle[2]) / 2, Width = Math.Min(length[0], length[1]), Height = Math.Max(length[0], length[1]) };
            }
        }
        private static void ValidateDiaphragm(Brep cabinet, Driver driver, double tolerance)
        {
            bool circular = driver.Kind.StartsWith("Circular", StringComparison.Ordinal);
            Aperture front = ApertureShape(cabinet.Faces[driver.Front], circular, tolerance);
            if (driver.Rear < 0) return;
            Aperture rear = ApertureShape(cabinet.Faces[driver.Rear], circular, tolerance);
            Plane plane; cabinet.Faces[driver.Front].TryGetPlane(out plane);
            Vector3d displacement = rear.Center - front.Center;
            Vector3d lateral = displacement - plane.Normal * (displacement * plane.Normal);
            if (Math.Abs(front.Width - rear.Width) > tolerance || Math.Abs(front.Height - rear.Height) > tolerance || lateral.Length > tolerance)
                throw new ArgumentException("A dipole needs matching, aligned front and rear diaphragm faces.");
        }
        private static Result PickDirection(Point3d origin, string prompt, out Vector3d direction)
        {
            direction = Vector3d.Unset;
            using (var pick = new GetPoint())
            {
                pick.SetCommandPrompt(prompt); pick.SetBasePoint(origin, true); pick.DrawLineFromPoint(origin, true);
                if (pick.Get() != GetResult.Point) return pick.CommandResult();
                direction = pick.Point() - origin;
                return direction.Unitize() ? Result.Success : Result.Failure;
            }
        }
        private static Result Spectrum(string prompt, out double[] values)
        {
            values = null;
            string text = "0";
            Result rc = RhinoGet.GetString(prompt, true, ref text);
            if (rc != Result.Success) return rc;
            string[] parts = text.Split(new[] { ';', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 1 && parts.Length != 8 && parts.Length != 24)
            { RhinoApp.WriteLine("Enter one, 8, or 24 values in ascending band order."); return Result.Failure; }
            values = new double[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                if (!double.TryParse(parts[i], NumberStyles.Float, Invariant, out values[i]) || double.IsNaN(values[i]) || double.IsInfinity(values[i]) || Math.Abs(values[i]) > 10000)
                { RhinoApp.WriteLine("Each value must be finite and within +/-10000."); return Result.Failure; }
            return Result.Success;
        }
        private static Result PickFace(RhinoDoc doc, Guid cabinetId, Brep cabinet, List<Driver> drivers, int excluded, string prompt, out int index)
        {
            index = -1;
            doc.Objects.UnselectAll();
            using (var pick = new GetObject())
            {
                pick.SetCommandPrompt(prompt); pick.GeometryFilter = ObjectType.Surface; pick.SubObjectSelect = true;
                pick.EnablePreSelect(false, true);
                if (pick.Get() != GetResult.Object) return pick.CommandResult();
                ObjRef reference = pick.Object(0);
                BrepFace face = reference.Face();
                if (reference.ObjectId != cabinetId || face == null)
                { RhinoApp.WriteLine("Select a face of the chosen cabinet."); return Result.Failure; }
                index = face.FaceIndex;
                Plane plane;
                if (index == excluded || drivers.Any(d => d.Front == face.FaceIndex || d.Rear == face.FaceIndex) || !cabinet.Faces[index].TryGetPlane(out plane, doc.ModelAbsoluteTolerance))
                { RhinoApp.WriteLine("Each diaphragm needs its own unused planar cabinet face."); return Result.Failure; }
                return Result.Success;
            }
        }
        private static Mesh BoundaryMesh(Brep cabinet, double edgeMeters, double scale,
            CancellationToken cancellation)
        {
            const int maxAttempts = 4;
            double requestedEdge = edgeMeters * 0.65;
            double longestEdge = 0;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                RhinoApp.Wait();
                cancellation.ThrowIfCancellationRequested();
                Mesh mesh = CreateBoundaryMesh(cabinet, requestedEdge / scale);
                bool accepted = false;
                try
                {
                    longestEdge = LongestTriangleEdge(mesh) * scale;
                    // Keep a margin for the solver's check after transforming to local meters.
                    accepted = longestEdge <= edgeMeters;
                    if (accepted)
                    {
                        RhinoApp.WriteLine("Mesh attempt {0}: longest triangle edge {1:G5} m (limit {2:G5} m).",
                            attempt, longestEdge, edgeMeters);
                        return mesh;
                    }
                }
                finally
                {
                    if (!accepted) mesh.Dispose();
                }

                if (attempt < maxAttempts)
                {
                    double reduction = Math.Max(0.5, Math.Min(0.85, 0.95 * edgeMeters / longestEdge));
                    requestedEdge *= reduction;
                    RhinoApp.WriteLine($"Mesh attempt {attempt}/{maxAttempts}: longest triangle edge {longestEdge:G5} m exceeds the {edgeMeters:G5} m limit. Retrying with a {requestedEdge:G5} m mesher setting.");
                }
            }

            throw new InvalidOperationException($"Rhino meshing exceeded the edge limit after {maxAttempts} attempts. The last mesh had a {longestEdge:G5} m edge against a {edgeMeters:G5} m limit.");
        }

        private static double LongestTriangleEdge(Mesh mesh)
        {
            double longest = 0;
            foreach (MeshFace face in mesh.Faces)
            {
                Point3d a = mesh.Vertices.Point3dAt(face.A);
                Point3d b = mesh.Vertices.Point3dAt(face.B);
                Point3d c = mesh.Vertices.Point3dAt(face.C);
                longest = Math.Max(longest, Math.Max(a.DistanceTo(b), Math.Max(b.DistanceTo(c), c.DistanceTo(a))));
            }
            return longest;
        }

        private static Mesh CreateBoundaryMesh(Brep cabinet, double requestedEdge)
        {
            var parameters = new MeshingParameters { MaximumEdgeLength = requestedEdge, MinimumEdgeLength = 0, RefineGrid = true, JaggedSeams = false, SimplePlanes = false, GridMinCount = 16 };
            Mesh[] parts = Mesh.CreateFromBrep(cabinet, parameters);
            if (parts == null || parts.Length == 0) throw new ArgumentException("Cabinet could not be meshed.");
            var mesh = new Mesh();
            try
            {
                foreach (Mesh part in parts) mesh.Append(part);
                mesh.Faces.ConvertQuadsToTriangles();
                mesh.Vertices.CombineIdentical(true, true); mesh.Weld(Math.PI); mesh.UnifyNormals();
                if (!mesh.IsValid || !mesh.IsClosed) throw new ArgumentException("Cabinet mesh is invalid or not watertight.");
                if (mesh.SolidOrientation() < 0) mesh.Flip(true, true, true);
                if (mesh.SolidOrientation() != 1) throw new ArgumentException("Cabinet mesh must have one consistent outward solid orientation.");
                return mesh;
            }
            catch { mesh.Dispose(); throw; }
            finally { foreach (Mesh part in parts) part?.Dispose(); }
        }
        private static int[] MapDiaphragms(Mesh mesh, Brep cabinet, List<Driver> drivers, double tolerance)
        {
            int[] patches = new int[mesh.Faces.Count];
            var counts = new Dictionary<int, int>();
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Point3d center = mesh.Faces.GetFaceCenter(i);
                for (int d = 0; d < drivers.Count; d++)
                    foreach (int face in new[] { drivers[d].Front, drivers[d].Rear })
                    {
                        if (face < 0) continue;
                        double u, v;
                        BrepFace surface = cabinet.Faces[face];
                        if (!surface.ClosestPoint(center, out u, out v) || surface.PointAt(u, v).DistanceTo(center) > tolerance || surface.IsPointOnFace(u, v) == PointFaceRelation.Exterior) continue;
                        if (patches[i] != 0) throw new ArgumentException("Overlapping diaphragm faces.");
                        int patch = face == drivers[d].Front ? d + 1 : -(d + 1);
                        patches[i] = patch; counts[patch] = counts.TryGetValue(patch, out int count) ? count + 1 : 1;
                    }
            }
            for (int d = 0; d < drivers.Count; d++)
                foreach (int patch in drivers[d].Rear < 0 ? new[] { d + 1 } : new[] { d + 1, -(d + 1) })
                    if (!counts.TryGetValue(patch, out int count) || count < 6)
                        throw new ArgumentException("Each diaphragm needs at least six mesh triangles. Increase meshing resolution or enlarge the diaphragm.");
            return patches;
        }
        private static List<Loudspeaker_Boundary_Radiation.Panel> BoundaryPanels(Mesh mesh)
        {
            var result = new List<Loudspeaker_Boundary_Radiation.Panel>();
            HPoint H(Point3d p) => new HPoint(p.X, p.Y, p.Z);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                MeshFace face = mesh.Faces[i];
                Point3d a = mesh.Vertices.Point3dAt(face.A), b = mesh.Vertices.Point3dAt(face.B), c = mesh.Vertices.Point3dAt(face.C);
                result.Add(new Loudspeaker_Boundary_Radiation.Panel(H(a), H(b), H(c)));
            }
            return result;
        }
        private static void StoreCabinet(GeometryBase source, Mesh mesh)
        {
            var points = new StringBuilder("P|"); var faces = new StringBuilder("F|");
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Point3d p = mesh.Vertices.Point3dAt(i);
                points.Append(i).Append(':').Append(p.X.ToString("R", Invariant)).Append(',').Append(p.Y.ToString("R", Invariant)).Append(',').Append(p.Z.ToString("R", Invariant)).Append(';');
            }
            for (int i = 0; i < mesh.Faces.Count; i++)
            { MeshFace f = mesh.Faces[i]; faces.Append(i).Append(':').Append(f.A).Append(',').Append(f.B).Append(',').Append(f.C).Append(';'); }
            source.SetUserString("CLF_CabinetPoints", points.ToString()); source.SetUserString("CLF_CabinetFaces", faces.ToString()); source.SetUserString("CLF_CabinetLines", "L|");
        }
        private static XElement Definition(Guid cabinet, List<Driver> drivers, bool thirds, double highest, double meshEdge, double power)
        {
            string Values(double[] values) => string.Join(";", values.Select(v => v.ToString("R", Invariant)));
            return new XElement("LoudspeakerDesign", new XAttribute("version", 1), new XAttribute("cabinet", cabinet),
                new XAttribute("bands", thirds ? 24 : 8), new XAttribute("highestHz", highest.ToString("R", Invariant)), new XAttribute("meshEdgeMeters", meshEdge.ToString("R", Invariant)), new XAttribute("powerDb", power.ToString("R", Invariant)),
                drivers.Select(d => new XElement("Driver", new XAttribute("kind", d.Kind), new XAttribute("frontFace", d.Front), new XAttribute("rearFace", d.Rear),
                    new XElement("VelocityGainDb", Values(d.Gain)), new XElement("PhaseDeg", Values(d.Phase)), new XElement("DelayMs", Values(d.Delay)))));
        }
        private static string OmniCode() => string.Concat(Enumerable.Repeat(string.Join(" ", Enumerable.Repeat("0", 37)) + ";", 72));
    }
}



