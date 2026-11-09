// Pachyderm-Acoustic (GPL-3.0-or-later)
// Copyright (c) 2026 Open Research in Acoustical Science and Education, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.Geometry;
using Pachyderm_Acoustic.Environment;
using Color = System.Drawing.Color;
using HVector = Hare.Geometry.Vector;

namespace Pachyderm_Acoustic.UI
{
    [System.Runtime.InteropServices.Guid("89e7e459-31b8-4f31-9c16-1261667ff662")]
    public sealed class PachSpatialResponseCommand : Rhino.Commands.Command
    {
        public override string EnglishName => "Pachyderm_Spatial_Response";
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Hare.Geometry.Point[] sources = null, receivers = null;
            Direct_Sound[] direct = null;
            ImageSourceData[] images = null;
            Receiver_Bank[] rays = null;
            PachHybridControl.Instance?.GetSims(ref sources, ref receivers, ref direct, ref images, ref rays);
            PachMapReceiver[] maps = null;
            Pach_Mapping_Control.Instance?.GetSims(ref maps);
            bool hybrid = receivers != null && receivers.Length > 0;
            bool mapping = maps != null && maps.Any(m => m != null);
            if (!hybrid && !mapping)
            {
                RhinoApp.WriteLine("Calculate or load Hybrid or Mapping results first.");
                return Result.Nothing;
            }
            bool useMap = !hybrid;
            if (hybrid && mapping)
            {
                using (var choice = new Rhino.Input.Custom.GetOption())
                {
                    choice.SetCommandPrompt("Spatial response results");
                    choice.AddOption("Hybrid"); int mappingOption = choice.AddOption("Mapping");
                    if (choice.Get() != Rhino.Input.GetResult.Option) return choice.CommandResult();
                    useMap = choice.OptionIndex() == mappingOption;
                }
            }
            if (useMap)
            {
                rays = maps;
                receivers = maps.First(m => m != null).Origins();
                direct = null; images = null;
            }
            new SpatialResponseForm(doc, receivers, direct, images, rays).Show();
            return Result.Success;
        }
    }

    // An angular estimate: signed axis marginals do not encode the joint distribution
    // of simultaneous arrivals. Each time bin uses independent sign probabilities,
    // with up to eight octant lobes. This preserves scalar energy and opposing signs,
    // and recovers the direction of an isolated arrival, without claiming ray recovery.
    internal static class SpatialResponseEstimate
    {
        internal struct Arrival
        {
            internal double Time, Energy;
            internal Vector3d Direction;
        }
        internal static void Add(List<Arrival> output, double time, double energy, HVector positive, HVector negative)
        {
            if (!(energy > 0) || double.IsInfinity(energy) || (double.IsNaN(time) || double.IsInfinity(time))) return;
            double[] p = { Math.Abs(positive.dx), Math.Abs(positive.dy), Math.Abs(positive.dz) };
            double[] n = { Math.Abs(negative.dx), Math.Abs(negative.dy), Math.Abs(negative.dz) };
            double[] total = { p[0] + n[0], p[1] + n[1], p[2] + n[2] };
            if (total.Any(v => double.IsNaN(v) || double.IsInfinity(v)) || total.Sum() == 0) return;
            for (int octant = 0; octant < 8; octant++)
            {
                double weight = 1;
                double[] component = new double[3];
                for (int axis = 0; axis < 3; axis++)
                {
                    bool minus = (octant & (1 << axis)) != 0;
                    double value = minus ? n[axis] : p[axis];
                    weight *= total[axis] == 0 ? (minus ? 0 : 1) : value / total[axis];
                    component[axis] = minus ? -value : value;
                }
                var direction = new Vector3d(component[0], component[1], component[2]);
                if (weight > 0 && direction.Unitize()) output.Add(new Arrival { Time = time, Energy = energy * weight, Direction = direction });
            }
        }
    }

    internal sealed class SpatialResponseForm : Form
    {
        readonly RhinoDoc doc;
        readonly Hare.Geometry.Point[] positions;
        readonly Direct_Sound[] direct;
        readonly ImageSourceData[] images;
        readonly Receiver_Bank[] rays;
        readonly DropDown receiver = new DropDown(), source = new DropDown(), band = new DropDown();
        readonly NumericStepper start = Number(0, 0, 10000000, 1), width = Number(20, .01, 10000000, 1);
        readonly NumericStepper smoothing = Number(15, 2, 90, 1), opacity = Number(60, 0, 100, 5);
        readonly NumericStepper fieldOfView = Number(120, 10, 150, 5), heading = Number(90, 0, 360, 5), elevation = Number(0, -89.9, 89.9, 5);
        readonly Color_Output_Control colorControl;
        readonly CheckBox relativeToWindow = new CheckBox { Text = "Relative to window maximum (0 dB)", Checked = false };
        readonly Label colorReference = new Label { Wrap = WrapMode.Word, Width = 350 };
        readonly SpatialResponseNavigation navigation;
        uint cameraChange;
        System.Drawing.Size cameraSize;
        bool viewportDestroyed;
        readonly Slider scrub = new Slider { MinValue = 0, MaxValue = 1000, Width = 360 };
        readonly Label status = new Label { Wrap = WrapMode.Word, Width = 360 };
        readonly Label scaleStatus = new Label { Wrap = WrapMode.Word, Width = 420 };
        readonly SpatialResponseConduit overlay;
        readonly RhinoView view;
        List<SpatialResponseEstimate.Arrival> arrivals = new List<SpatialResponseEstimate.Arrival>();
        double duration = 1, reference;
        SpatialResponseEstimate.Arrival[] directReference = Array.Empty<SpatialResponseEstimate.Arrival>();
        bool estimatedDirectReference;
        bool updating, cleaned, recentering, initialWindow = true;
        static NumericStepper Number(double value, double min, double max, double step) => new NumericStepper { Value = value, MinValue = min, MaxValue = max, Increment = step, DecimalPlaces = 2, Width = 100 };

        internal SpatialResponseForm(RhinoDoc doc, Hare.Geometry.Point[] positions, Direct_Sound[] direct, ImageSourceData[] images, Receiver_Bank[] rays)
        {
            this.doc = doc; this.positions = positions; this.direct = direct; this.images = images; this.rays = rays;
            Title = "Pachyderm spatial response"; Padding = 10; Owner = Rhino.UI.RhinoEtoApp.MainWindow;
            for (int i = 0; i < positions.Length; i++) receiver.Items.Add("Receiver " + (i + 1));
            source.Items.Add("All sources (energy sum)");
            int count = Math.Max(direct?.Length ?? 0, Math.Max(images?.Length ?? 0, rays?.Length ?? 0));
            for (int i = 0; i < count; i++) source.Items.Add("Source " + (i + 1));
            band.Items.Add("Broadband");
            bool thirds = direct?.FirstOrDefault(d => d != null)?.BandCount == 24 || rays?.Any(r => r?.Third_Oct == true) == true;
            int bands = thirds ? 24 : 8;
            for (int i = 0; i < bands; i++) band.Items.Add((62.5 * Math.Pow(2, i / (thirds ? 3.0 : 1))).ToString("0.#") + " Hz");
            receiver.SelectedIndex = 0; source.SelectedIndex = 0; band.SelectedIndex = 0;
            var previous = new Button { Text = "Previous window" }; var next = new Button { Text = "Next window" };
            var reset = new Button { Text = "Reset receiver view" };
            ClientSize = new Eto.Drawing.Size(960, 700);
            MinimumSize = new Eto.Drawing.Size(650, 450);
            var results = new DynamicLayout { Padding = 10, Spacing = new Eto.Drawing.Size(8, 8) };
            results.AddRow(new Label { Text = "Receiver" }, receiver);
            results.AddRow(new Label { Text = "Source" }, source);
            results.AddRow(new Label { Text = "Frequency band" }, band);

            var time = new DynamicLayout { Padding = 10, Spacing = new Eto.Drawing.Size(8, 8) };
            time.AddRow(new Label { Text = "Start (ms)" }, start);
            time.AddRow(new Label { Text = "Window width (ms)" }, width);
            time.AddRow(scrub);
            time.AddRow(previous, next);
            time.AddRow(new Label { Text = "Time from source emission, including source delay.", Wrap = WrapMode.Word, Width = 340 });

            var camera = new DynamicLayout { Padding = 10, Spacing = new Eto.Drawing.Size(8, 8) };
            fieldOfView.ToolTip = "Full horizontal field of view. Default: 120 degrees.";
            heading.ToolTip = "World heading: 0 = +X, 90 = +Y, 180 = -X, 270 = -Y.";
            camera.AddRow(new Label { Text = "Horizontal field of view (degrees)" }, fieldOfView);
            camera.AddRow(new Label { Text = "Heading (degrees)" }, heading);
            camera.AddRow(new Label { Text = "Elevation (degrees)" }, elevation);
            camera.AddRow(reset);
            camera.AddRow(new Label { Text = "Right-drag or middle-drag to look around. Camera position stays locked to the receiver.", Wrap = WrapMode.Word, Width = 340 });

            var display = new DynamicLayout { Spacing = new Eto.Drawing.Size(6, 8) };
            display.AddRow(new Label { Text = "Angular smoothing (degrees)" }, smoothing);
            display.AddRow(new Label { Text = "Overlay opacity (%)" }, opacity);
            relativeToWindow.ToolTip = "Use the maximum of the smoothed directional field over all directions in the current time window as 0 dB. Uncheck to reference colors to direct sound (first-arrival estimate when separate direct data is unavailable).";
            display.AddRow(relativeToWindow);
            display.AddRow(colorReference);
            display.AddRow(new Label { Text = "Scale values: relative energy (dB). Empty areas use the lowest scale color.", Wrap = WrapMode.Word, Width = 350 });
            // Keep the shared palette's own controls separate from the display settings.
            // Auto-size its container to the 300-pixel gradient and native font metrics.
            colorControl = new Color_Output_Control(new DynamicLayout(), true) { Width = -1, Height = -1 };
            colorControl.SetValueRange(-200, 0); colorControl.setscale(-40, 0);
            var appearance = new DynamicLayout { Padding = 10, Spacing = new Eto.Drawing.Size(8, 8) };
            appearance.AddRow(colorControl);
            appearance.AddRow(scaleStatus);
            appearance.AddRow(display);
            appearance.AddRow(new Label { Text = "Edit the minimum and maximum next to the gradient. Scale limits remain fixed; the reference follows the selected normalization mode.", Wrap = WrapMode.Word, Width = 420 });
            var left = new StackLayout { Spacing = 10, HorizontalContentAlignment = HorizontalAlignment.Stretch };
            left.Items.Add(new GroupBox { Text = "Results", Content = results });
            left.Items.Add(new GroupBox { Text = "Time window", Content = time });
            left.Items.Add(new GroupBox { Text = "Receiver view", Content = camera });
            left.Items.Add(status);
            // Natural-size columns inside the scrollable: a narrow window must scroll,
            // rather than compressing one panel underneath its neighbour.
            var columns = new StackLayout {
                Orientation = Orientation.Horizontal, Spacing = 12,
                VerticalContentAlignment = VerticalAlignment.Top,
                Items = { left, new GroupBox { Text = "Color display", Content = appearance } }
            };
            Content = new Scrollable { Border = BorderType.None, Content = columns };
            view = doc.Views.Add("Pachyderm receiver", DefinedViewportProjection.Perspective, new System.Drawing.Rectangle(100, 100, 900, 650), true);
            overlay = new SpatialResponseConduit(view.ActiveViewportID);
            navigation = new SpatialResponseNavigation(view.ActiveViewportID, LookAround) { Enabled = true };
            fieldOfView.ValueChanged += (s, e) => ApplyView();
            heading.ValueChanged += (s, e) => ApplyView(); elevation.ValueChanged += (s, e) => ApplyView();
            receiver.SelectedIndexChanged += (s, e) => { ResetView(); Rebuild(); };
            source.SelectedIndexChanged += (s, e) => { ConfigureBands(); Rebuild(); }; band.SelectedIndexChanged += (s, e) => Rebuild();
            start.ValueChanged += (s, e) => RefreshOverlay(); width.ValueChanged += (s, e) => RefreshOverlay();
            smoothing.ValueChanged += (s, e) => RefreshOverlay();
            colorControl.Update += (s, e) => RefreshAppearance();
            opacity.ValueChanged += (s, e) => RefreshAppearance();
            relativeToWindow.CheckedChanged += (s, e) => RefreshAppearance();
            scrub.ValueChanged += (s, e) => { if (!updating) start.Value = duration * scrub.Value / 1000; };
            previous.Click += (s, e) => start.Value = Math.Max(0, start.Value - width.Value);
            next.Click += (s, e) => start.Value = Math.Min(duration, start.Value + width.Value);
            reset.Click += (s, e) => ResetView();
            Closed += (s, e) => Cleanup(); RhinoDoc.CloseDocument += DocumentClosed; RhinoView.Destroy += ViewDestroyed; RhinoView.Modified += ViewModified; RhinoApp.Idle += ViewIdle;
            ResetView(); Rebuild();
        }
        void ResetView()
        {
            recentering = true;
            try { heading.Value = 90; elevation.Value = 0; }
            finally { recentering = false; }
            ApplyView();
        }
        void LookAround(double dx, double dy)
        {
            double degreesPerPixel = fieldOfView.Value / Math.Max(1, view.ActiveViewport.Bounds.Width);
            recentering = true;
            try
            {
                heading.Value = ((heading.Value - dx * degreesPerPixel) % 360 + 360) % 360;
                elevation.Value = Math.Max(elevation.MinValue, Math.Min(elevation.MaxValue, elevation.Value - dy * degreesPerPixel));
            }
            finally { recentering = false; }
            ApplyView();
        }
        Vector3d ViewDirection()
        {
            double azimuth = heading.Value * Math.PI / 180, altitude = elevation.Value * Math.PI / 180;
            return new Vector3d(Math.Cos(altitude) * Math.Cos(azimuth), Math.Cos(altitude) * Math.Sin(azimuth), Math.Sin(altitude));
        }
        void ApplyView()
        {
            if (cleaned || recentering) return;
            recentering = true;
            try
            {
                var viewport = view.ActiveViewport;
                var point = Utilities.RCPachTools.HarePointToModel(positions[receiver.SelectedIndex]);
                using (var projection = new Rhino.DocObjects.ViewportInfo(viewport))
                {
                    // Edit a copy, then install the receiver location and lock atomically.
                    // Native orbit/pan must never translate the actual viewport camera.
                    projection.UnlockCamera();
                    projection.ChangeToPerspectiveProjection(RhinoMath.UnsetValue, true, 50);
                    var direction = ViewDirection();
                    double azimuth = heading.Value * Math.PI / 180, altitude = elevation.Value * Math.PI / 180;
                    projection.SetCameraLocation(point); projection.SetCameraDirection(direction);
                    projection.SetCameraUp(new Vector3d(-Math.Sin(altitude) * Math.Cos(azimuth), -Math.Sin(altitude) * Math.Sin(azimuth), Math.Cos(altitude)));
                    projection.TargetPoint = point + direction;
                    double near = projection.FrustumNear, far = projection.FrustumFar;
                    double halfWidth = near * Math.Tan(fieldOfView.Value * Math.PI / 360);
                    double halfHeight = halfWidth * Math.Max(1, viewport.Bounds.Height) / Math.Max(1, viewport.Bounds.Width);
                    projection.SetFrustum(-halfWidth, halfWidth, -halfHeight, halfHeight, near, far);
                    projection.IsCameraLocationLocked = true;
                    viewport.SetViewProjection(projection, true);
                }
                cameraChange = viewport.ChangeCounter; cameraSize = viewport.Bounds.Size;
                view.Redraw();
            }
            finally { recentering = false; }
        }
        void EnsureView()
        {
            if (cleaned || recentering) return;
            var viewport = view.ActiveViewport;
            if (cameraChange == viewport.ChangeCounter && cameraSize == viewport.Bounds.Size) return;
            var point = Utilities.RCPachTools.HarePointToModel(positions[receiver.SelectedIndex]);
            bool restore;
            using (var projection = new Rhino.DocObjects.ViewportInfo(viewport))
            {
                double tangent = Math.Tan(fieldOfView.Value * Math.PI / 360);
                double aspect = Math.Max(1, viewport.Bounds.Width) / (double)Math.Max(1, viewport.Bounds.Height);
                restore = !projection.IsCameraLocationLocked || !projection.IsPerspectiveProjection ||
                    viewport.CameraLocation.DistanceTo(point) > 1e-9 ||
                    (viewport.CameraDirection - ViewDirection()).SquareLength > 1e-12 ||
                    Math.Abs(projection.FrustumWidth / (2 * projection.FrustumNear) - tangent) > 1e-8 ||
                    Math.Abs(projection.FrustumHeight / (2 * projection.FrustumNear) - tangent / aspect) > 1e-8;
            }
            if (restore) ApplyView();
            else { cameraChange = viewport.ChangeCounter; cameraSize = viewport.Bounds.Size; }
        }
        static T At<T>(T[] data, int index) where T : class => data != null && index < data.Length ? data[index] : null;
        void ConfigureBands()
        {
            int index = source.SelectedIndex == 0 ? 0 : source.SelectedIndex - 1;
            var d = At(direct, index); var bank = At(rays, index); var image = At(images, index);
            int count = d?.BandCount ?? (bank?.Third_Oct == true || image?.ThirdOctave == true ? 24 : 8);
            if (band.Items.Count == count + 1) return;
            updating = true;
            try
            {
                band.Items.Clear(); band.Items.Add("Broadband");
                for (int b = 0; b < count; b++) band.Items.Add((62.5 * Math.Pow(2, b / (count == 24 ? 3.0 : 1))).ToString("0.#") + " Hz");
                band.SelectedIndex = 0;
            }
            finally { updating = false; }
        }
        void Rebuild()
        {
            if (updating || cleaned) return;
            arrivals.Clear(); reference = 0; directReference = Array.Empty<SpatialResponseEstimate.Arrival>(); estimatedDirectReference = false; overlay.Enabled = false;
            var directArrivals = new List<SpatialResponseEstimate.Arrival>();
            int rec = receiver.SelectedIndex;
            try
            {
                int count = source.Items.Count - 1;
                IEnumerable<int> selected = source.SelectedIndex == 0 ? Enumerable.Range(0, count) : new[] { source.SelectedIndex - 1 };
                foreach (int s in selected)
                {
                    var d = At(direct, s); var image = At(images, s); var bank = At(rays, s);
                    if (d == null && image == null && bank == null) continue;
                    if (d?.CoherentSpeakers == true) throw new InvalidOperationException("Coherent speaker Hybrid results require pressure-domain directional synthesis. Use directional Mapping results for this case.");
                    if (bank is PachMapReceiver map && (map.Time1Pt || !map.Directional)) throw new InvalidOperationException("Recalculate this map with Track Directionality enabled and time-resolved storage.");
                    int bands = d?.BandCount ?? (bank?.Third_Oct == true || image?.ThirdOctave == true ? 24 : 8);
                    if (bands != band.Items.Count - 1) throw new InvalidOperationException("Selected sources use different frequency resolutions; select a single source.");
                    if (band.SelectedIndex > bands) throw new InvalidOperationException("Selected band is unavailable for this source.");
                    int first = band.SelectedIndex == 0 ? 0 : band.SelectedIndex - 1, last = band.SelectedIndex == 0 ? bands : first + 1;
                    double delay = d?.Delay_ms ?? bank?.delay_ms ?? 0;
                    int sourceStart = arrivals.Count, directStart = directArrivals.Count;
                    for (int b = first; b < last; b++)
                    {
                        if (bank != null)
                        {
                            if (rec >= bank.Rec_List.Length) throw new InvalidOperationException("Receiver sets differ between sources.");
                            double origin = bank.Rec_List[rec] is PachMapReceiver.Map_Receiver mapped ? mapped.Direct_Time * 1000 : 0;
                            for (int t = 0; t < bank.SampleCT; t++)
                                SpatialResponseEstimate.Add(arrivals, delay + origin + t * 1000.0 / bank.SampleRate, bank.Rec_List[rec].Energy(t, b), bank.Directions_Pos(b, t, rec), bank.Directions_Neg(b, t, rec));
                        }
                        if (d != null && d.IsOccluded(rec))
                        {
                            var p = d.Directions_Pos(b, rec, 0, 0, false); var n = d.Directions_Neg(b, rec, 0, 0, false);
                            var energy = d.EnergyValue(b, rec);
                            for (int t = 0; t < energy.Length; t++)
                            {
                                double time = delay + d.Time(rec) * 1000 + t * 1000.0 / d.SampleRate;
                                SpatialResponseEstimate.Add(arrivals, time, energy[t], p[t], n[t]);
                                SpatialResponseEstimate.Add(directArrivals, time, energy[t], p[t], n[t]);
                            }
                        }
                        if (image != null)
                            foreach (var path in image.Paths[rec])
                            {
                                if (path.CoherentSpeaker) throw new InvalidOperationException("Use directional Mapping results for coherent speaker paths.");
                                var energy = path.Energy(b, 44100);
                                // Reflection vectors follow propagation; receiver vectors point back to arrival.
                                var direction = path is Compound_Path ? null : path.Dir_Energy(b);
                                // Match mapping's six directional compound filters at 44.1 kHz.
                                double[][] channels = path is Compound_Path compound ? compound.Fdir.Select(f =>
                                    bands == 24 ? Audio.Pach_SP.FIR_Third_Bandpass(f, b, 44100, 0) : Audio.Pach_SP.FIR_Bandpass(f, b, 44100, 0)).ToArray() : null;
                                for (int t = 0; t < energy.Length; t++)
                                {
                                    if (channels != null)
                                    {
                                        double[] q = channels.Select(ch => t < ch.Length ? ch[t] * ch[t] : 0).ToArray();
                                        SpatialResponseEstimate.Add(arrivals, delay + path.TravelTime * 1000 + t * 1000.0 / 44100, energy[t],
                                            new HVector(q[0], q[2], q[4]), new HVector(-q[1], -q[3], -q[5]));
                                        continue;
                                    }
                                    var v = direction[Math.Min(t, direction.Length - 1)] * -1;
                                    SpatialResponseEstimate.Add(arrivals, delay + path.TravelTime * 1000 + t * 1000.0 / 44100, energy[t],
                                        new HVector(Math.Max(0, v.dx), Math.Max(0, v.dy), Math.Max(0, v.dz)),
                                        new HVector(Math.Min(0, v.dx), Math.Min(0, v.dy), Math.Min(0, v.dz)));
                                }
                            }
                    }
                    // Maps retain the combined histogram, not a separate direct response.
                    // If no directional direct sound is available, use this source's first
                    // occupied time bin and identify the reference as an estimate in the UI.
                    if (directArrivals.Count == directStart && arrivals.Count > sourceStart)
                    {
                        var sourceArrivals = arrivals.Skip(sourceStart).ToArray();
                        double firstTime = sourceArrivals.Min(a => a.Time);
                        directArrivals.AddRange(sourceArrivals.Where(a => a.Time == firstTime));
                        estimatedDirectReference = true;
                    }
                }
                directReference = directArrivals.ToArray();
                duration = Math.Max(1, arrivals.Count == 0 ? 1 : arrivals.Max(a => a.Time) + 1);
                // A stable upper bound for every window and smoothing setting, never auto-scaled per frame.
                reference = arrivals.Sum(a => a.Energy);
                if (initialWindow && arrivals.Count > 0)
                {
                    initialWindow = false; updating = true;
                    start.Value = Math.Max(0, Math.Floor(arrivals.Min(a => a.Time)));
                    updating = false;
                }
                RefreshOverlay();
            }
            catch (Exception ex) { arrivals.Clear(); status.Text = ex.Message; view.Redraw(); }
        }
        void RefreshOverlay()
        {
            if (updating || cleaned) return;
            updating = true;
            try
            {
                scrub.Value = (int)Math.Min(1000, start.Value / duration * 1000);
                var window = arrivals.Where(a => a.Time >= start.Value && a.Time < start.Value + width.Value).ToArray();
                overlay.Set(window, directReference, smoothing.Value,
                    string.Format("R{0} | {1:0.##}–{2:0.##} ms", receiver.SelectedIndex + 1, start.Value, start.Value + width.Value));
                RefreshAppearance();
                status.Text = window.Length == 0 ? "No directional energy in this window." : string.Format("{0:0.###}% of stored directional response energy in this window.", reference > 0 ? window.Sum(a => a.Energy) / reference * 100 : 0);
                overlay.Enabled = true; view.Redraw();
            }
            finally { updating = false; }
        }
        void RefreshAppearance()
        {
            if (cleaned) return;
            if (colorControl.Max <= colorControl.Min)
            {
                scaleStatus.Text = "Color scale minimum must be below its maximum.";
                return;
            }
            scaleStatus.Text = "";
            bool normalizeToWindow = relativeToWindow.Checked == true;
            colorReference.Text = normalizeToWindow ? "Color reference: maximum directional value in this window." :
                directReference.Length == 0 ? "Color reference: no direct or first-arrival energy available." :
                estimatedDirectReference ? "Color reference: direct / first-arrival peak estimate (separate direct data unavailable)." : "Color reference: direct-sound peak.";
            overlay.SetAppearance(colorControl.Scale, colorControl.Min, colorControl.Max, opacity.Value, normalizeToWindow);
            view.Redraw();
        }
        void DocumentClosed(object sender, DocumentEventArgs e) { if (e.Document == doc) { viewportDestroyed = true; Close(); } }
        void ViewDestroyed(object sender, ViewEventArgs e) { if (e.View == view) { viewportDestroyed = true; Close(); } }
        void ViewModified(object sender, ViewEventArgs e) { if (e.View == view) EnsureView(); }
        void ViewIdle(object sender, EventArgs e) { EnsureView(); }
        void Cleanup()
        {
            if (cleaned) return; cleaned = true; overlay.Enabled = false; navigation.Enabled = false;
            RhinoDoc.CloseDocument -= DocumentClosed; RhinoView.Destroy -= ViewDestroyed;
            RhinoView.Modified -= ViewModified; RhinoApp.Idle -= ViewIdle;
            if (!viewportDestroyed)
            {
                using (var projection = new Rhino.DocObjects.ViewportInfo(view.ActiveViewport))
                {
                    projection.IsCameraLocationLocked = false;
                    view.ActiveViewport.SetViewProjection(projection, true);
                }
                view.Redraw();
            }
        }
    }

    // Replace Rhino's orbit/pan gestures only in this receiver viewport. Rotating
    // heading/elevation leaves the camera at its exact measurement position.
    internal sealed class SpatialResponseNavigation : Rhino.UI.MouseCallback
    {
        readonly Guid viewport;
        readonly Action<double, double> look;
        bool dragging;
        System.Drawing.Point previous;
        internal SpatialResponseNavigation(Guid viewport, Action<double, double> look) { this.viewport = viewport; this.look = look; }
        bool IsReceiverView(Rhino.UI.MouseCallbackEventArgs e) => e.View?.ActiveViewportID == viewport;
        protected override void OnMouseDown(Rhino.UI.MouseCallbackEventArgs e)
        {
            if (!IsReceiverView(e) || (e.MouseButton != Rhino.UI.MouseButton.Right && e.MouseButton != Rhino.UI.MouseButton.Middle)) return;
            dragging = true; previous = e.ViewportPoint; e.Cancel = true;
        }
        protected override void OnMouseMove(Rhino.UI.MouseCallbackEventArgs e)
        {
            if (!IsReceiverView(e)) { dragging = false; return; }
            if (!dragging) return;
            e.Cancel = true;
            var current = e.ViewportPoint; look(current.X - previous.X, current.Y - previous.Y); previous = current;
        }
        protected override void OnMouseUp(Rhino.UI.MouseCallbackEventArgs e)
        {
            bool wasDragging = dragging; dragging = false;
            if (wasDragging && IsReceiverView(e)) e.Cancel = true;
        }
    }
    internal sealed class SpatialResponseConduit : DisplayConduit
    {
        readonly Guid viewport;
        // Precompute a world-aligned angular grid once per window, then project it at redraw.
        readonly double[,] field = new double[180, 91];
        double reference, windowMaximum, minimum = -40, maximum, alpha;
        bool normalizeToWindow;
        Pach_Graphics.Colorscale scale;
        string caption;
        internal SpatialResponseConduit(Guid viewport) { this.viewport = viewport; }
        internal void SetAppearance(Pach_Graphics.Colorscale scale, double minimum, double maximum, double opacity, bool normalizeToWindow)
        {
            this.scale = scale; this.minimum = minimum; this.maximum = maximum; alpha = opacity / 100; this.normalizeToWindow = normalizeToWindow;
        }
        internal void Set(SpatialResponseEstimate.Arrival[] arrivals, SpatialResponseEstimate.Arrival[] direct, double sigmaDegrees, string caption)
        {
            this.caption = caption;
            // Both references use the identical angular grid and Gaussian smoothing,
            // so an isolated direct response reaches 0 dB at its angular peak.
            reference = BuildField(direct, sigmaDegrees, null);
            windowMaximum = BuildField(arrivals, sigmaDegrees, field);
        }
        static double BuildField(SpatialResponseEstimate.Arrival[] arrivals, double sigmaDegrees, double[,] target)
        {
            double peak = 0;
            double sigma = sigmaDegrees * Math.PI / 180;
            // Bin directions at the angular grid resolution before smoothing. Cost depends
            // on occupied angular cells rather than the number of impulse-response bins.
            var bins = new Dictionary<(int X, int Y), double>();
            foreach (var a in arrivals)
            {
                double az = Math.Atan2(a.Direction.Y, a.Direction.X); if (az < 0) az += 2 * Math.PI;
                var key = ((int)Math.Round(az * 90 / Math.PI) % 180,
                    Math.Max(0, Math.Min(90, (int)Math.Round((Math.Asin(a.Direction.Z) + Math.PI / 2) * 90 / Math.PI))));
                bins.TryGetValue(key, out double energy); bins[key] = energy + a.Energy;
            }
            var lobes = bins.Select(pair => {
                double az = pair.Key.X * Math.PI / 90, el = pair.Key.Y * Math.PI / 90 - Math.PI / 2;
                return new SpatialResponseEstimate.Arrival { Energy = pair.Value,
                    Direction = new Vector3d(Math.Cos(el) * Math.Cos(az), Math.Cos(el) * Math.Sin(az), Math.Sin(el)) };
            }).ToArray();
            for (int x = 0; x < 180; x++) for (int y = 0; y <= 90; y++)
            {
                double az = x * Math.PI / 90, el = y * Math.PI / 90 - Math.PI / 2;
                var direction = new Vector3d(Math.Cos(el) * Math.Cos(az), Math.Cos(el) * Math.Sin(az), Math.Sin(el));
                double value = 0;
                foreach (var arrival in lobes)
                {
                    double angle = Math.Acos(Math.Max(-1, Math.Min(1, direction * arrival.Direction)));
                    if (angle < 3 * sigma) value += arrival.Energy * Math.Exp(-angle * angle / (2 * sigma * sigma));
                }
                if (target != null) target[x, y] = value;
                peak = Math.Max(peak, value);
            }
            return peak;
        }
        protected override void DrawForeground(DrawEventArgs e)
        {
            if (e.Viewport.Id != viewport || scale == null) return;
            var bounds = e.Viewport.Bounds;
            // Window reference is the peak of the complete angular field, independent
            // of which direction the camera currently faces.
            double colorReference = normalizeToWindow ? windowMaximum : reference;
            const int cell = 12;
            for (int x = 0; x < bounds.Width; x += cell) for (int y = 0; y < bounds.Height; y += cell)
            {
                var direction = e.Viewport.ClientToWorld(new Point2d(x + cell / 2.0, y + cell / 2.0)).Direction;
                direction.Unitize();
                double az = Math.Atan2(direction.Y, direction.X); if (az < 0) az += 2 * Math.PI;
                int ix = (int)(az * 90 / Math.PI) % 180, iy = Math.Max(0, Math.Min(90, (int)Math.Round((Math.Asin(direction.Z) + Math.PI / 2) * 90 / Math.PI)));
                // Silence and energy below the displayed floor use the lowest color,
                // including windows with no arrivals. Never expose grey holes.
                double value = field[ix, iy];
                double level = value > 0 && colorReference > 0 ? 10 * Math.Log10(value / colorReference) : minimum;
                level = Math.Max(minimum, Math.Min(maximum, level));
                var color = Color.FromArgb((int)(alpha * 255), Color.FromArgb(scale.GetValue(level, minimum, maximum).ToArgb()));
                e.Display.Draw2dRectangle(new System.Drawing.Rectangle(x, y, cell, cell), color, 0, color);
            }
            e.Display.Draw2dRectangle(new System.Drawing.Rectangle(8, 8, Math.Max(1, Math.Min(bounds.Width - 16, 640)), 28), Color.Black, 0, Color.Black);
            e.Display.Draw2dText(caption + string.Format(" | {0:0.#} to {1:0.#} dB relative", minimum, maximum) + (normalizeToWindow ? " | window max" : " | direct / first arrival"), Color.White, new Point2d(16, 14), false, 14);
        }
    }
}
