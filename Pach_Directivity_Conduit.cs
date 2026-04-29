//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)
//'
//'This file is part of Pachyderm-Acoustic.
//'
//'Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit
//'Pachyderm-Acoustic is free software; you can redistribute it and/or modify
//'it under the terms of the GNU General Public License as published
//'by the Free Software Foundation; either version 3 of the License, or
//'(at your option) any later version.
//'Pachyderm-Acoustic is distributed in the hope that it will be useful,
//'but WITHOUT ANY WARRANTY; without even the implied warranty of
//'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//'GNU General Public License for more details.
//'
//'You should have received a copy of the GNU General Public
//'License along with Pachyderm-Acoustic; if not, write to the Free Software
//'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

using Hare.Geometry;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using SDColor = System.Drawing.Color;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        /// <summary>
        /// Displays relative loudspeaker directivity contours on model geometry.
        /// This is an aiming aid only: no distance loss, no air attenuation, no received SPL calculation.
        /// </summary>
        public class SpeakerPatternConduit : DisplayConduit
        {
            public enum Display_Mode
            {
                Boundary_Contours,

                // Kept only so older SourceControl code still compiles.
                // This conduit no longer draws its own balloon.
                Sphere,
                Sphere_And_Boundary_Contours
            }

            private struct FaceIndex
            {
                public int A;
                public int B;
                public int C;
                public int D;
                public bool IsQuad;

                public FaceIndex(MeshFace f)
                {
                    A = f.A;
                    B = f.B;
                    C = f.C;
                    D = f.D;
                    IsQuad = f.IsQuad;
                }
            }

            private struct ContourLabel
            {
                public Point3d Location;
                public string Text;
                public SDColor DotColor;

                public ContourLabel(Point3d location, string text, SDColor dotColor)
                {
                    Location = location;
                    Text = text;
                    DotColor = dotColor;
                }
            }

            private struct AimTransform
            {
                public double Alt;
                public double Azi;
                public double Axi;

                public static AimTransform FromSource(RhinoObject source)
                {
                    AimTransform aim = new AimTransform();

                    if (source == null || source.Geometry == null)
                    {
                        return aim;
                    }

                    string aiming = source.Geometry.GetUserString("Aiming");
                    if (string.IsNullOrEmpty(aiming))
                    {
                        return aim;
                    }

                    string[] A = aiming.Split(';');
                    if (A.Length < 3)
                    {
                        return aim;
                    }

                    aim.Alt = SafeParse(A[0]) * Math.PI / 180.0;
                    aim.Azi = SafeParse(A[1]) * Math.PI / 180.0;
                    aim.Axi = SafeParse(A[2]) * Math.PI / 180.0;

                    return aim;
                }

                public Hare.Geometry.Vector WorldToLocal(Vector3d world)
                {
                    if (!world.Unitize())
                    {
                        return new Hare.Geometry.Vector(0, 1, 0);
                    }

                    double x = world.X;
                    double y = world.Y;
                    double z = world.Z;

                    // Inverse azimuth rotation in x/y.
                    double x0 = x * Math.Cos(-Azi) - y * Math.Sin(-Azi);
                    double y0 = x * Math.Sin(-Azi) + y * Math.Cos(-Azi);
                    x = x0;
                    y = y0;

                    // Inverse altitude rotation in y/z.
                    double y1 = y * Math.Cos(-Alt) - z * Math.Sin(-Alt);
                    double z1 = y * Math.Sin(-Alt) + z * Math.Cos(-Alt);
                    y = y1;
                    z = z1;

                    // Inverse axial rotation in x/z.
                    double x2 = x * Math.Cos(-Axi) - z * Math.Sin(-Axi);
                    double z2 = x * Math.Sin(-Axi) + z * Math.Cos(-Axi);
                    x = x2;
                    z = z2;

                    Hare.Geometry.Vector local = new Hare.Geometry.Vector(x, y, z);
                    local.Normalize();

                    return local;
                }
            }

            private sealed class DirectivityLookup
            {
                private readonly double[,] Table;
                private readonly int UMax;
                private readonly int VMax;

                public readonly double Maximum;

                private DirectivityLookup(double[,] table, int umax, int vmax, double maximum)
                {
                    Table = table;
                    UMax = umax;
                    VMax = vmax;
                    Maximum = maximum;
                }

                public static DirectivityLookup FromSource(RhinoObject source, int octave)
                {
                    if (source == null || source.Geometry == null)
                    {
                        return null;
                    }

                    string source_type = source.Geometry.GetUserString("SourceType");
                    if (string.IsNullOrEmpty(source_type) || source_type == "0" || source_type == "1")
                    {
                        return Omni();
                    }

                    string code = BalloonCode(source, Math.Max(0, Math.Min(7, octave)));
                    if (string.IsNullOrEmpty(code))
                    {
                        return Omni();
                    }

                    if (!TryParseBalloon(code, out double[,] table, out int umax, out int vmax))
                    {
                        return Omni();
                    }

                    double max = double.NegativeInfinity;

                    for (int v = 0; v < vmax; v++)
                    {
                        for (int u = 0; u < umax; u++)
                        {
                            double val = table[u, v];

                            if (double.IsNaN(val) || double.IsInfinity(val)) continue;

                            max = Math.Max(max, val);
                        }
                    }

                    if (double.IsNegativeInfinity(max))
                    {
                        max = 0;
                    }

                    return new DirectivityLookup(table, umax, vmax, max);
                }

                private static DirectivityLookup Omni()
                {
                    return new DirectivityLookup(new double[,] { { 0 } }, 1, 1, 0);
                }

                public double Evaluate(Hare.Geometry.Vector local_dir)
                {
                    if (UMax == 1 && VMax == 1)
                    {
                        return 0;
                    }

                    local_dir.Normalize();

                    // Balloon convention from Classes_Balloons:
                    // Theta = u * PI / (umax - 1)
                    // Phi = 2 * v * PI / vmax + PI / 2
                    // Vector = (sin(theta) cos(phi), cos(theta), sin(theta) sin(phi))
                    double theta = Math.Acos(Math.Max(-1, Math.Min(1, local_dir.dy)));
                    double phi = Math.Atan2(local_dir.dz, local_dir.dx) - Math.PI / 2.0;

                    while (phi < 0) phi += 2.0 * Math.PI;
                    while (phi >= 2.0 * Math.PI) phi -= 2.0 * Math.PI;

                    double u = theta / Math.PI * (UMax - 1);
                    double v = phi / (2.0 * Math.PI) * VMax;

                    int u0 = (int)Math.Floor(u);
                    int u1 = Math.Min(UMax - 1, u0 + 1);
                    int v0 = ((int)Math.Floor(v)) % VMax;
                    int v1 = (v0 + 1) % VMax;

                    double fu = u - u0;
                    double fv = v - Math.Floor(v);

                    double a = Table[u0, v0] * (1 - fu) + Table[u1, v0] * fu;
                    double b = Table[u0, v1] * (1 - fu) + Table[u1, v1] * fu;

                    return a * (1 - fv) + b * fv;
                }

                private static string BalloonCode(RhinoObject source, int octave)
                {
                    switch (octave)
                    {
                        case 0: return source.Geometry.GetUserString("Balloon63");
                        case 1: return source.Geometry.GetUserString("Balloon125");
                        case 2: return source.Geometry.GetUserString("Balloon250");
                        case 3: return source.Geometry.GetUserString("Balloon500");
                        case 4: return source.Geometry.GetUserString("Balloon1000");
                        case 5: return source.Geometry.GetUserString("Balloon2000");
                        case 6: return source.Geometry.GetUserString("Balloon4000");
                        case 7: return source.Geometry.GetUserString("Balloon8000");
                        default: return source.Geometry.GetUserString("Balloon1000");
                    }
                }

                private static bool TryParseBalloon(string code, out double[,] table, out int umax, out int vmax)
                {
                    table = null;
                    umax = 0;
                    vmax = 0;

                    string[] rows = code.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (rows.Length == 0) return false;

                    string[] first = rows[0].Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    bool row_based = first.Length == 19 || first.Length == 37;

                    if (row_based)
                    {
                        if (first.Length == 19)
                        {
                            umax = 19;
                            vmax = 36;
                        }
                        else
                        {
                            umax = 37;
                            vmax = 72;
                        }

                        table = new double[umax, vmax];

                        for (int v = 0; v < vmax; v++)
                        {
                            int row = SymmetricRowIndex(v, rows.Length, vmax);
                            string[] cols = rows[row].Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int u = 0; u < umax; u++)
                            {
                                table[u, v] = u < cols.Length ? SafeParse(cols[u]) : 0;
                            }
                        }
                    }
                    else
                    {
                        List<double> values = new List<double>();

                        for (int r = 0; r < rows.Length; r++)
                        {
                            string[] cells = rows[r].Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int c = 0; c < cells.Length; c++)
                            {
                                values.Add(SafeParse(cells[c]));
                            }
                        }

                        if (values.Count >= 37 * 72)
                        {
                            umax = 37;
                            vmax = 72;
                        }
                        else if (values.Count >= 19 * 36)
                        {
                            umax = 19;
                            vmax = 36;
                        }
                        else
                        {
                            return false;
                        }

                        table = new double[umax, vmax];

                        int idx = 0;

                        for (int v = 0; v < vmax; v++)
                        {
                            for (int u = 0; u < umax; u++)
                            {
                                table[u, v] = idx < values.Count ? values[idx++] : 0;
                            }
                        }
                    }

                    ConvertStoredBalloonToDirectivity(table, umax, vmax);

                    return true;
                }

                private static void ConvertStoredBalloonToDirectivity(double[,] table, int umax, int vmax)
                {
                    double min = double.PositiveInfinity;
                    double max = double.NegativeInfinity;

                    for (int v = 0; v < vmax; v++)
                    {
                        for (int u = 0; u < umax; u++)
                        {
                            double val = table[u, v];

                            if (double.IsNaN(val) || double.IsInfinity(val)) continue;

                            min = Math.Min(min, val);
                            max = Math.Max(max, val);
                        }
                    }

                    // Generic Pachyderm balloon files are normally positive attenuation:
                    // magnitude = SPL - stored_value.
                    // CLF-style relative directivity may already be negative/relative.
                    bool values_are_attenuation = min >= 0 && max <= 70;

                    if (!values_are_attenuation) return;

                    for (int v = 0; v < vmax; v++)
                    {
                        for (int u = 0; u < umax; u++)
                        {
                            table[u, v] = -table[u, v];
                        }
                    }
                }

                private static int SymmetricRowIndex(int v, int row_count, int vmax)
                {
                    if (row_count >= vmax)
                    {
                        return v;
                    }

                    if (row_count == vmax / 2 + 1)
                    {
                        return v <= vmax / 2 ? v : vmax - v;
                    }

                    if (row_count == vmax / 4 + 1)
                    {
                        int half = vmax / 2;
                        int quarter = vmax / 4;

                        int vv = v % half;
                        if (vv > quarter) vv = half - vv;

                        return Math.Max(0, Math.Min(row_count - 1, vv));
                    }

                    return v % row_count;
                }
            }

            private RhinoObject Source_Object;
            private List<Line>[] Boundary_Contour_Lines;
            private SDColor[] Boundary_Contour_Colors;
            private readonly List<ContourLabel> Boundary_Contour_Labels = new List<ContourLabel>();

            private Rhino.Geometry.Mesh Cached_Scene;
            private string Cached_Scene_Signature;
            private double Cached_Mesh_Max_Edge = double.NaN;

            public int Octave = 4;

            /// <summary>
            /// Mesh density used for meshing Breps before contouring.
            /// Larger values are faster but produce rougher contours.
            /// </summary>
            public double Contour_Mesh_Max_Edge = 2.0;

            public double[] Contour_Levels = new double[] {-1, -2, -3, -4, -5, -6, -12, -18 };
            public int Contour_Line_Thickness = 3;

            /// <summary>
            /// If true, contours are culled when another model surface lies between the source and the contour segment.
            /// This keeps the pattern from appearing on surfaces behind the first impacted boundary.
            /// </summary>
            public bool First_Surface_Only = true;

            /// <summary>
            /// Tolerance, in Rhino model units, for matching a contour segment to the first mesh-ray hit.
            /// Increase this slightly if contours disappear on coarse meshes.
            /// </summary>
            public double First_Surface_Tolerance = 0.05;

            public bool Show_Contour_Labels = true;
            public int Labels_Per_Level = 1;
            public double Min_Label_Separation = 2.0;

            public bool Use_Cached_Geometry = true;
            public Display_Mode Mode = Display_Mode.Boundary_Contours;

            public static SpeakerPatternConduit Instance
            {
                get;
                private set;
            }

            public SpeakerPatternConduit()
            {
                Instance = this;
                EnsureContourStorage();
            }

            public void SetSource(RhinoObject Obj)
            {
                Source_Object = Obj;
                Rebuild();
            }

            public void InvalidateScene()
            {
                Cached_Scene = null;
                Cached_Scene_Signature = null;
                Cached_Mesh_Max_Edge = double.NaN;
            }

            public void Clear()
            {
                Source_Object = null;
                ClearContours();
                Boundary_Contour_Labels.Clear();
                Enabled = false;
                RhinoDoc.ActiveDoc?.Views.Redraw();
            }

            public void Rebuild()
            {
                EnsureContourStorage();
                ClearContours();
                Boundary_Contour_Labels.Clear();

                if (Mode == Display_Mode.Sphere)
                {
                    Enabled = false;
                    RhinoDoc.ActiveDoc?.Views.Redraw();
                    return;
                }

                if (Source_Object == null) return;
                if (Source_Object.Geometry == null) return;

                DirectivityLookup lookup = DirectivityLookup.FromSource(Source_Object, Octave);
                if (lookup == null) return;

                Point3d source_pt = SourcePoint(Source_Object);
                AimTransform aim = AimTransform.FromSource(Source_Object);

                BuildBoundaryContours(source_pt, aim, lookup);

                Enabled = true;
                RhinoDoc.ActiveDoc?.Views.Redraw();
            }

            protected override void PostDrawObjects(DrawEventArgs e)
            {
                if (Boundary_Contour_Lines == null) return;

                for (int i = 0; i < Boundary_Contour_Lines.Length; i++)
                {
                    if (Boundary_Contour_Lines[i] == null) continue;
                    if (Boundary_Contour_Lines[i].Count == 0) continue;

                    e.Display.DrawLines(
                        Boundary_Contour_Lines[i],
                        Boundary_Contour_Colors[i],
                        Contour_Line_Thickness
                    );
                }

                if (Show_Contour_Labels && Boundary_Contour_Labels.Count > 0)
                {
                    for (int i = 0; i < Boundary_Contour_Labels.Count; i++)
                    {
                        ContourLabel label = Boundary_Contour_Labels[i];
                        e.Display.DrawDot(label.Location, label.Text, label.DotColor, SDColor.White);
                    }
                }
            }

            private void BuildBoundaryContours(Point3d source, AimTransform aim, DirectivityLookup lookup)
            {
                Rhino.Geometry.Mesh scene = ProjectionSceneMesh();

                if (scene == null || scene.Vertices.Count == 0 || scene.Faces.Count == 0)
                {
                    return;
                }

                Point3d[] points = new Point3d[scene.Vertices.Count];

                for (int i = 0; i < scene.Vertices.Count; i++)
                {
                    points[i] = MeshVertexPoint(scene, i);
                }

                FaceIndex[] faces = new FaceIndex[scene.Faces.Count];

                for (int i = 0; i < scene.Faces.Count; i++)
                {
                    faces[i] = new FaceIndex(scene.Faces[i]);
                }

                double[] rel = new double[points.Length];

                Parallel.For(0, points.Length, i =>
                {
                    Vector3d dir = points[i] - source;

                    if (!dir.Unitize())
                    {
                        rel[i] = double.NaN;
                        return;
                    }

                    Hare.Geometry.Vector local = aim.WorldToLocal(dir);
                    double raw = lookup.Evaluate(local);

                    if (double.IsNaN(raw) || double.IsInfinity(raw))
                    {
                        rel[i] = double.NaN;
                    }
                    else
                    {
                        rel[i] = raw - lookup.Maximum;
                    }
                });

                int contour_count = Contour_Levels.Length;
                object merge_lock = new object();

                Parallel.For(
                    0,
                    faces.Length,
                    () => NewLocalContourStore(contour_count),
                    (f, state, local_lines) =>
                    {
                        FaceIndex face = faces[f];

                        if (face.IsQuad)
                        {
                            Point3d A = points[face.A];
                            Point3d B = points[face.B];
                            Point3d C = points[face.C];
                            Point3d D = points[face.D];

                            double vA = rel[face.A];
                            double vB = rel[face.B];
                            double vC = rel[face.C];
                            double vD = rel[face.D];

                            AddTriangleContours(local_lines, A, vA, B, vB, C, vC);
                            AddTriangleContours(local_lines, A, vA, C, vC, D, vD);
                        }
                        else
                        {
                            AddTriangleContours(
                                local_lines,
                                points[face.A], rel[face.A],
                                points[face.B], rel[face.B],
                                points[face.C], rel[face.C]
                            );
                        }

                        return local_lines;
                    },
                    local_lines =>
                    {
                        lock (merge_lock)
                        {
                            for (int i = 0; i < contour_count; i++)
                            {
                                Boundary_Contour_Lines[i].AddRange(local_lines[i]);
                            }
                        }
                    }
                );

                if (First_Surface_Only)
                {
                    CullContoursToFirstSurface(scene, source);
                }

                if (Show_Contour_Labels)
                {
                    BuildContourLabels();
                }
            }

            private void CullContoursToFirstSurface(Rhino.Geometry.Mesh scene, Point3d source)
            {
                if (scene == null || Boundary_Contour_Lines == null) return;

                for (int i = 0; i < Boundary_Contour_Lines.Length; i++)
                {
                    List<Line> src = Boundary_Contour_Lines[i];
                    if (src == null || src.Count == 0) continue;

                    List<Line> kept = new List<Line>(src.Count);

                    for (int j = 0; j < src.Count; j++)
                    {
                        if (LineIsOnFirstSurface(scene, source, src[j]))
                        {
                            kept.Add(src[j]);
                        }
                    }

                    Boundary_Contour_Lines[i] = kept;
                }
            }

            private bool LineIsOnFirstSurface(Rhino.Geometry.Mesh scene, Point3d source, Line line)
            {
                if (!line.IsValid) return false;

                Point3d mid = line.PointAt(0.5);
                Vector3d dir = mid - source;
                double target_dist = dir.Length;

                if (target_dist <= Rhino.RhinoMath.ZeroTolerance) return false;
                if (!dir.Unitize()) return false;

                Ray3d ray = new Ray3d(source, dir);
                double hit_dist = Rhino.Geometry.Intersect.Intersection.MeshRay(scene, ray);

                if (hit_dist < 0 || double.IsNaN(hit_dist) || double.IsInfinity(hit_dist)) return false;

                double tol = Math.Max(First_Surface_Tolerance, Contour_Mesh_Max_Edge * 0.02);

                // If something is appreciably closer than the contour segment midpoint, this contour is behind
                // another surface and should not be shown.
                return hit_dist >= target_dist - tol && hit_dist <= target_dist + tol;
            }

            private void BuildContourLabels()
            {
                Boundary_Contour_Labels.Clear();

                if (Boundary_Contour_Lines == null || Contour_Levels == null) return;
                if (Labels_Per_Level <= 0) return;

                double min_sep2 = Min_Label_Separation * Min_Label_Separation;

                for (int i = 0; i < Boundary_Contour_Lines.Length && i < Contour_Levels.Length; i++)
                {
                    List<Line> lines = Boundary_Contour_Lines[i];
                    if (lines == null || lines.Count == 0) continue;

                    List<Line> candidates = new List<Line>(lines);
                    candidates.Sort((a, b) => b.Length.CompareTo(a.Length));

                    int labels_added = 0;

                    for (int j = 0; j < candidates.Count; j++)
                    {
                        Line line = candidates[j];
                        if (!line.IsValid || line.Length < Rhino.RhinoMath.ZeroTolerance) continue;

                        Point3d p = line.PointAt(0.5);
                        if (IsTooCloseToExistingLabel(p, min_sep2)) continue;

                        Boundary_Contour_Labels.Add(new ContourLabel(
                            p,
                            LevelLabel(Contour_Levels[i]),
                            Boundary_Contour_Colors[i]
                        ));

                        labels_added++;
                        if (labels_added >= Labels_Per_Level) break;
                    }
                }
            }

            private bool IsTooCloseToExistingLabel(Point3d p, double min_sep2)
            {
                for (int i = 0; i < Boundary_Contour_Labels.Count; i++)
                {
                    if (Boundary_Contour_Labels[i].Location.DistanceToSquared(p) < min_sep2)
                    {
                        return true;
                    }
                }

                return false;
            }

            private static string LevelLabel(double level)
            {
                return level.ToString("0.#", CultureInfo.InvariantCulture) + " dB";
            }

            private static List<Line>[] NewLocalContourStore(int count)
            {
                List<Line>[] lines = new List<Line>[count];

                for (int i = 0; i < count; i++)
                {
                    lines[i] = new List<Line>();
                }

                return lines;
            }

            private void AddTriangleContours(
                List<Line>[] line_store,
                Point3d A, double vA,
                Point3d B, double vB,
                Point3d C, double vC)
            {
                for (int i = 0; i < Contour_Levels.Length && i < line_store.Length; i++)
                {
                    double level = Contour_Levels[i];

                    List<Point3d> pts = new List<Point3d>();

                    AddContourEdgePoint(A, vA, B, vB, level, pts);
                    AddContourEdgePoint(B, vB, C, vC, level, pts);
                    AddContourEdgePoint(C, vC, A, vA, level, pts);

                    if (pts.Count == 2)
                    {
                        line_store[i].Add(new Line(pts[0], pts[1]));
                    }
                    else if (pts.Count > 2)
                    {
                        double max_dist = 0;
                        int a = 0;
                        int b = 1;

                        for (int m = 0; m < pts.Count; m++)
                        {
                            for (int n = m + 1; n < pts.Count; n++)
                            {
                                double d = pts[m].DistanceToSquared(pts[n]);

                                if (d > max_dist)
                                {
                                    max_dist = d;
                                    a = m;
                                    b = n;
                                }
                            }
                        }

                        line_store[i].Add(new Line(pts[a], pts[b]));
                    }
                }
            }

            private static void AddContourEdgePoint(
                Point3d A, double vA,
                Point3d B, double vB,
                double level,
                List<Point3d> pts)
            {
                if (double.IsNaN(vA) || double.IsNaN(vB)) return;
                if (double.IsInfinity(vA) || double.IsInfinity(vB)) return;

                double dA = vA - level;
                double dB = vB - level;

                if (Math.Abs(dA) < 1e-9 && Math.Abs(dB) < 1e-9)
                {
                    return;
                }

                if (Math.Abs(dA) < 1e-9)
                {
                    AddUniquePoint(pts, A);
                    return;
                }

                if (Math.Abs(dB) < 1e-9)
                {
                    AddUniquePoint(pts, B);
                    return;
                }

                if ((dA > 0 && dB > 0) || (dA < 0 && dB < 0))
                {
                    return;
                }

                double t = (level - vA) / (vB - vA);

                if (t < 0 || t > 1) return;

                Point3d P = A + t * (B - A);
                AddUniquePoint(pts, P);
            }

            private static void AddUniquePoint(List<Point3d> pts, Point3d P)
            {
                for (int i = 0; i < pts.Count; i++)
                {
                    if (pts[i].DistanceToSquared(P) < 1e-10)
                    {
                        return;
                    }
                }

                pts.Add(P);
            }

            private Rhino.Geometry.Mesh ProjectionSceneMesh()
            {
                RhinoDoc doc = RhinoDoc.ActiveDoc;
                if (doc == null) return null;

                string signature = SceneSignature(doc);

                if (Use_Cached_Geometry &&
                    Cached_Scene != null &&
                    Cached_Scene_Signature == signature &&
                    Math.Abs(Cached_Mesh_Max_Edge - Contour_Mesh_Max_Edge) < 1e-9)
                {
                    return Cached_Scene;
                }

                Rhino.Geometry.Mesh scene = new Rhino.Geometry.Mesh();

                MeshingParameters mp = new MeshingParameters();
                mp.MaximumEdgeLength = Contour_Mesh_Max_Edge;
                mp.SimplePlanes = false;
                mp.JaggedSeams = false;

                foreach (RhinoObject obj in doc.Objects.GetObjectList(ObjectType.AnyObject))
                {
                    if (obj == null) continue;
                    if (obj.Geometry == null) continue;
                    if (obj.IsHidden) continue;
                    if (Source_Object != null && obj.Id == Source_Object.Id) continue;

                    string name = obj.Attributes.Name;
                    if (name == "Acoustical Source") continue;
                    if (name == "Acoustical Receiver") continue;

                    Rhino.Geometry.Mesh[] meshes = null;

                    if (obj.Geometry is Rhino.Geometry.Mesh m)
                    {
                        meshes = new Rhino.Geometry.Mesh[] { m };
                    }
                    else if (obj.Geometry is Brep b)
                    {
                        meshes = Rhino.Geometry.Mesh.CreateFromBrep(b, mp);
                    }
                    else if (obj.Geometry is Surface s)
                    {
                        Brep sb = s.ToBrep();
                        if (sb != null) meshes = Rhino.Geometry.Mesh.CreateFromBrep(sb, mp);
                    }
                    else if (obj.Geometry is Extrusion x)
                    {
                        Brep xb = x.ToBrep();
                        if (xb != null) meshes = Rhino.Geometry.Mesh.CreateFromBrep(xb, mp);
                    }

                    if (meshes == null) continue;

                    foreach (Rhino.Geometry.Mesh mesh in meshes)
                    {
                        if (mesh == null) continue;
                        if (mesh.Vertices.Count == 0) continue;
                        if (mesh.Faces.Count == 0) continue;

                        scene.Append(mesh);
                    }
                }

                if (scene.Vertices.Count == 0 || scene.Faces.Count == 0)
                {
                    return null;
                }

                scene.Normals.ComputeNormals();
                scene.Compact();

                Cached_Scene = scene;
                Cached_Scene_Signature = signature;
                Cached_Mesh_Max_Edge = Contour_Mesh_Max_Edge;

                return scene;
            }

            private static string SceneSignature(RhinoDoc doc)
            {
                StringBuilder sb = new StringBuilder();

                foreach (RhinoObject obj in doc.Objects.GetObjectList(ObjectType.AnyObject))
                {
                    if (obj == null) continue;
                    if (obj.Geometry == null) continue;
                    if (obj.IsHidden) continue;

                    string name = obj.Attributes.Name;
                    if (name == "Acoustical Source") continue;
                    if (name == "Acoustical Receiver") continue;

                    BoundingBox bb = obj.Geometry.GetBoundingBox(true);

                    sb.Append(obj.Id);
                    sb.Append('|');
                    sb.Append(obj.ObjectType);
                    sb.Append('|');
                    sb.Append(bb.Min.X.ToString("R", CultureInfo.InvariantCulture));
                    sb.Append(',');
                    sb.Append(bb.Min.Y.ToString("R", CultureInfo.InvariantCulture));
                    sb.Append(',');
                    sb.Append(bb.Min.Z.ToString("R", CultureInfo.InvariantCulture));
                    sb.Append('|');
                    sb.Append(bb.Max.X.ToString("R", CultureInfo.InvariantCulture));
                    sb.Append(',');
                    sb.Append(bb.Max.Y.ToString("R", CultureInfo.InvariantCulture));
                    sb.Append(',');
                    sb.Append(bb.Max.Z.ToString("R", CultureInfo.InvariantCulture));
                    sb.Append(';');
                }

                return sb.ToString();
            }

            private static Point3d SourcePoint(RhinoObject source)
            {
                if (source.Geometry is Rhino.Geometry.Point pt)
                {
                    return pt.Location;
                }

                BoundingBox bb = source.Geometry.GetBoundingBox(true);
                return bb.Center;
            }

            private static Point3d MeshVertexPoint(Rhino.Geometry.Mesh mesh, int index)
            {
                Point3f p = mesh.Vertices[index];
                return new Point3d(p.X, p.Y, p.Z);
            }

            private static double SafeParse(string value)
            {
                if (string.IsNullOrWhiteSpace(value)) return 0;

                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double d))
                {
                    return d;
                }

                if (double.TryParse(value, out d))
                {
                    return d;
                }

                return 0;
            }

            private void EnsureContourStorage()
            {
                int count = Contour_Levels == null ? 0 : Contour_Levels.Length;

                if (count == 0)
                {
                    Contour_Levels = new double[] {-1,-2,-3,-4,-5, -6, -12, -18 };
                    count = Contour_Levels.Length;
                }

                if (Boundary_Contour_Lines != null && Boundary_Contour_Lines.Length == count)
                {
                    return;
                }

                Boundary_Contour_Lines = new List<Line>[count];
                Boundary_Contour_Colors = new SDColor[count];

                for (int i = 0; i < count; i++)
                {
                    Boundary_Contour_Lines[i] = new List<Line>();
                }

                if (count > 0) Boundary_Contour_Colors[0] = SDColor.Red;
                if (count > 1) Boundary_Contour_Colors[1] = SDColor.OrangeRed;
                if (count > 2) Boundary_Contour_Colors[2] = SDColor.Orange;
                if (count > 3) Boundary_Contour_Colors[3] = SDColor.Yellow;
                if (count > 4) Boundary_Contour_Colors[4] = SDColor.GreenYellow;
                if (count > 5) Boundary_Contour_Colors[5] = SDColor.Green;

                if (count > 6) Boundary_Contour_Colors[6] = SDColor.DodgerBlue;
                if (count > 7) Boundary_Contour_Colors[7] = SDColor.Blue;

                for (int i = 8; i < count; i++)
                {
                    Boundary_Contour_Colors[i] = SDColor.Black;
                }
            }

            private void ClearContours()
            {
                if (Boundary_Contour_Lines == null) return;

                for (int i = 0; i < Boundary_Contour_Lines.Length; i++)
                {
                    Boundary_Contour_Lines[i]?.Clear();
                }
            }
        }
    }
}