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
using MathNet.Numerics;
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
using System.Linq;
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
            private List<Guid> Array_Object_IDs = null;
            private bool Use_Array_Mode = false;
            private double Array_Reference_Distance = 10.0;
            private Rhino.Geometry.Mesh Array_Balloon_Mesh = null;
            private bool Show_Array_Balloon = true;
            private double Array_Balloon_Radius = 2.0;
            public int Array_Balloon_Min_Rays = 800;

            public enum Display_Mode
            {
                Boundary_Contours,
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

            public void SetArrayElements(List<RhinoObject> objects, double referenceDistance)
            {
                Array_Object_IDs = new List<Guid>();

                if (objects != null)
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        if (objects[i] != null)
                        {
                            Array_Object_IDs.Add(objects[i].Id);
                        }
                    }
                }

                Use_Array_Mode = Array_Object_IDs.Count > 0;
                Array_Reference_Distance = Math.Max(0.1, referenceDistance);

                Source_Object = null;

                Rebuild();
            }

            private static void GetAiming(RhinoObject source, out double alt, out double azi, out double axi)
            {
                alt = 0;
                azi = 0;
                axi = 0;

                if (source == null || source.Geometry == null) return;

                string aiming = source.Geometry.GetUserString("Aiming");
                if (string.IsNullOrWhiteSpace(aiming)) return;

                string[] A = aiming.Split(';');

                if (A.Length > 0) alt = SafeParse(A[0]);
                if (A.Length > 1) azi = SafeParse(A[1]);
                if (A.Length > 2) axi = SafeParse(A[2]);
            }

            private static Hare.Geometry.Vector WorldToLocal(Vector3d world, double alt, double azi, double axi)
            {
                if (!world.Unitize())
                {
                    return new Hare.Geometry.Vector(0, 1, 0);
                }

                Hare.Geometry.Vector local = new Hare.Geometry.Vector(world.X, world.Y, world.Z);

                // Inverse of the normal Pachyderm aiming rotation.
                local = Utilities.PachTools.Rotate_Vector(local, -azi, 0, true);
                local = Utilities.PachTools.Rotate_Vector(local, 0, -alt, true);

                if (Math.Abs(axi) > 1E-9)
                {
                    double a = -axi * Math.PI / 180.0;
                    double x = local.dx;
                    double z = local.dz;

                    local.dx = x * Math.Cos(a) - z * Math.Sin(a);
                    local.dz = x * Math.Sin(a) + z * Math.Cos(a);
                }

                local.Normalize();
                return local;
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

                    string code;

                    switch (octave)
                    {
                        case 0: code = source.Geometry.GetUserString("Balloon63"); break;
                        case 1: code = source.Geometry.GetUserString("Balloon125"); break;
                        case 2: code = source.Geometry.GetUserString("Balloon250"); break;
                        case 3: code = source.Geometry.GetUserString("Balloon500"); break;
                        case 4: code = source.Geometry.GetUserString("Balloon1000"); break;
                        case 5: code = source.Geometry.GetUserString("Balloon2000"); break;
                        case 6: code = source.Geometry.GetUserString("Balloon4000"); break;
                        case 7: code = source.Geometry.GetUserString("Balloon8000"); break;
                        default: code = source.Geometry.GetUserString("Balloon500"); break;
                    }

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
                            int row;

                            if (rows.Length >= vmax) row = v;
                            else if (rows.Length == vmax / 2 + 1) row = v <= vmax / 2 ? v : vmax - v;
                            else if (rows.Length == vmax / 4 + 1)
                            {
                                int half = vmax / 2;
                                int quarter = vmax / 4;

                                int vv = v % half;
                                if (vv > quarter) vv = half - vv;

                                row = Math.Max(0, Math.Min(rows.Length - 1, vv));
                            }
                            else row = v % rows.Length;

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

                    //Convert Stored Balloon To Directivity
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

                    bool values_are_attenuation = min >= 0 && max <= 70;

                    if (values_are_attenuation)
                    {
                        for (int v = 0; v < vmax; v++)
                        {
                            for (int u = 0; u < umax; u++)
                            {
                                table[u, v] = -table[u, v];
                            }
                        }
                    }
                    return true;
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

            private static SpeakerPatternConduit instance = null;

            public static SpeakerPatternConduit Instance
            {
                get
                {
                    if (instance == null) instance = new SpeakerPatternConduit();
                    return instance;
                }
            }

            private SpeakerPatternConduit()
            {
                EnsureContourStorage();
            }

            public void SetSource(RhinoObject Obj)
            {
                Array_Object_IDs = null;
                Use_Array_Mode = false;
                Array_Balloon_Mesh = null;

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
                Array_Object_IDs = null;
                Use_Array_Mode = false;
                Array_Balloon_Mesh = null;

                if (Boundary_Contour_Lines != null)
                {
                    for (int i = 0; i < Boundary_Contour_Lines.Length; i++)
                    {
                        Boundary_Contour_Lines[i]?.Clear();
                    }
                }

                Boundary_Contour_Labels.Clear();
                Enabled = false;
                RhinoDoc.ActiveDoc?.Views.Redraw();
            }

            public void Rebuild()
            {
                EnsureContourStorage();
                if (Boundary_Contour_Lines != null) for (int i = 0; i < Boundary_Contour_Lines.Length; i++) Boundary_Contour_Lines[i]?.Clear();

                Boundary_Contour_Labels.Clear();
                Array_Balloon_Mesh = null;

                if (Mode == Display_Mode.Sphere)
                {
                    Enabled = false;
                    RhinoDoc.ActiveDoc?.Views.Redraw();
                    return;
                }

                if (Use_Array_Mode)
                {
                    List<RhinoObject> array_objects = new List<RhinoObject>();

                    if (Array_Object_IDs == null) return;
                    if (RhinoDoc.ActiveDoc == null) return;

                    for (int i = 0; i < Array_Object_IDs.Count; i++)
                    {
                        RhinoObject obj = RhinoDoc.ActiveDoc.Objects.FindId(Array_Object_IDs[i]);

                        if (obj != null && obj.Geometry != null)
                        {
                            array_objects.Add(obj);
                        }
                    }

                    array_objects.Sort((a, b) =>
                    {
                        int ai = 0;
                        int bi = 0;

                        int.TryParse(a.Geometry.GetUserString("ArrayElementIndex"), out ai);
                        int.TryParse(b.Geometry.GetUserString("ArrayElementIndex"), out bi);

                        return ai.CompareTo(bi);
                    });

                    BuildBoundaryContoursArray(array_objects, Octave);
                }
                else
                {
                    if (Source_Object == null) return;
                    if (Source_Object.Geometry == null) return;

                    DirectivityLookup lookup = DirectivityLookup.FromSource(Source_Object, Octave);
                    if (lookup == null) return;

                    Point3d source_pt = SourcePoint(Source_Object);
                    
                    double alt, azi, axi;
                    GetAiming(Source_Object, out alt, out azi, out axi);

                    BuildBoundaryContours(source_pt, alt, azi, axi, lookup);
                }

                Enabled = true;
                RhinoDoc.ActiveDoc?.Views.Redraw();
            }

            private void BuildBoundaryContoursArray(List<RhinoObject> sources, int octave)
            {
                Rhino.Geometry.Mesh scene = ProjectionSceneMesh();

                if (scene == null || scene.Vertices.Count == 0 || scene.Faces.Count == 0)
                {
                    return;
                }

                int oct = Math.Max(0, Math.Min(7, octave));

                double frequency;
                switch (oct)
                {
                    case 0: frequency = 62.5; break;
                    case 1: frequency = 125; break;
                    case 2: frequency = 250; break;
                    case 3: frequency = 500; break;
                    case 4: frequency = 1000; break;
                    case 5: frequency = 2000; break;
                    case 6: frequency = 4000; break;
                    case 7: frequency = 8000; break;
                    default: frequency = 1000; break;
                }

                double omega = Utilities.Numerics.angularFrequency_Octave[oct];
                double k = omega / 343.0;

                List<Point3d> elementOrigins = new List<Point3d>();
                List<double> elementAlt = new List<double>();
                List<double> elementAzi = new List<double>();
                List<double> elementAxi = new List<double>(); List<DirectivityLookup> elementLookups = new List<DirectivityLookup>();
                List<double> elementGainsDb = new List<double>();
                List<double> elementDelaysMs = new List<double>();

                for (int i = 0; i < sources.Count; i++)
                {
                    RhinoObject src = sources[i];

                    if (src == null || src.Geometry == null) continue;

                    DirectivityLookup lookup = DirectivityLookup.FromSource(src, oct);
                    if (lookup == null) continue;

                    Point3d origin = SourcePoint(src);
                    double alt, azi, axi;
                    GetAiming(src, out alt, out azi, out axi);

                    double gainDb = 0.0;
                    string swl = src.Geometry.GetUserString("SWL");

                    if (!string.IsNullOrWhiteSpace(swl))
                    {
                        try
                        {
                            double[] values = Utilities.PachTools.DecodeSourcePower(swl);

                            if (values != null && values.Length > 0)
                            {
                                int gainOct = Math.Max(0, Math.Min(oct, values.Length - 1));
                                gainDb = values[gainOct];
                            }
                        }
                        catch
                        {
                            gainDb = 0.0;
                        }
                    }

                    double delayMs = 0.0;
                    string delayText = src.Geometry.GetUserString("Delay");

                    if (!double.TryParse(
                        delayText,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out delayMs))
                    {
                        double.TryParse(delayText, out delayMs);
                    }

                    string octaveDelayText = src.Geometry.GetUserString("ArrayDelayOctaveMs");

                    if (!string.IsNullOrWhiteSpace(octaveDelayText))
                    {
                        string[] parts = octaveDelayText.Split(';');

                        if (oct >= 0 && oct < parts.Length)
                        {
                            double bandDelay = 0.0;
                            if (!double.TryParse(parts[oct], NumberStyles.Float, CultureInfo.InvariantCulture, out bandDelay))
                            {
                                double.TryParse(parts[oct], out bandDelay);
                            }
                            delayMs += bandDelay;
                        }
                    }

                    string arrayGainText = src.Geometry.GetUserString("ArrayGainOctaveDb");

                    if (!string.IsNullOrWhiteSpace(arrayGainText))
                    {
                        double[] arrayGain = Utilities.PachTools.DecodeEight(arrayGainText);

                        if (arrayGain != null && arrayGain.Length > 0)
                        {
                            int gainOct = Math.Max(0, Math.Min(oct, arrayGain.Length - 1));
                            gainDb += arrayGain[gainOct];
                        }
                    }

                    elementOrigins.Add(origin);
                    elementAlt.Add(alt);
                    elementAzi.Add(azi);
                    elementAxi.Add(axi);
                    elementLookups.Add(lookup);
                    elementGainsDb.Add(gainDb);
                    elementDelaysMs.Add(delayMs);
                }

                if (elementOrigins.Count == 0) return;

                double cx = 0;
                double cy = 0;
                double cz = 0;

                for (int i = 0; i < elementOrigins.Count; i++)
                {
                    cx += elementOrigins[i].X;
                    cy += elementOrigins[i].Y;
                    cz += elementOrigins[i].Z;
                }

                Point3d arrayCenter = new Point3d(cx / elementOrigins.Count, cy / elementOrigins.Count,cz / elementOrigins.Count);

                double maxGainDb = double.NegativeInfinity;

                for (int i = 0; i < elementGainsDb.Count; i++)
                {
                    if (elementGainsDb[i] > maxGainDb)
                    {
                        maxGainDb = elementGainsDb[i];
                    }
                }

                if (double.IsNegativeInfinity(maxGainDb))
                {
                    maxGainDb = 0.0;
                }

                double c = 343.0;

                Array_Balloon_Mesh = null;

                if (Show_Array_Balloon)
                {
                    Hare.Geometry.Topology sphere = Utilities.Geometry.GeoSphere(4).Model[0];

                    Array_Balloon_Mesh = Utilities.RCPachTools.HaretoRhinoMesh(sphere, true);

                    double[] rawDb = new double[Array_Balloon_Mesh.Vertices.Count];
                    
                    for (int i = 0; i < Array_Balloon_Mesh.Vertices.Count; i++)
                    {
                        Vector3d patternDirection = new Vector3d(Array_Balloon_Mesh.Vertices[i].X, Array_Balloon_Mesh.Vertices[i].Y, Array_Balloon_Mesh.Vertices[i].Z);

                        if (!patternDirection.Unitize()) patternDirection = new Vector3d(0, 1, 0);
                        Point3d virtualTarget = arrayCenter + patternDirection * Array_Reference_Distance;
                        System.Numerics.Complex sum = System.Numerics.Complex.Zero;

                        for (int e = 0; e < elementOrigins.Count; e++)
                        {
                            Vector3d worldDir = virtualTarget - elementOrigins[e];
                            double r = worldDir.Length;

                            if (r <= Rhino.RhinoMath.ZeroTolerance) continue;
                            worldDir.Unitize();

                            Hare.Geometry.Vector local = WorldToLocal(worldDir, elementAlt[e], elementAzi[e], elementAxi[e]);

                            double directivityDb = elementLookups[e].Evaluate(local) - elementLookups[e].Maximum;
                            double gain = Math.Pow(10.0, (elementGainsDb[e] - maxGainDb + directivityDb) / 20.0);
                            double tau = elementDelaysMs[e] / 1000.0;
                            double phase = -k * r - omega * tau;
                            sum += System.Numerics.Complex.FromPolarCoordinates(gain, phase);
                        }

                        double mag = sum.Magnitude;
                        rawDb[i] = mag <= 1E-12 ? -120.0 : 20.0 * Math.Log10(mag);
                    }

                    double balloonMax = rawDb.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).DefaultIfEmpty(0.0).Max();

                    Pach_Graphics.HSV_colorscale c_scale = new Pach_Graphics.HSV_colorscale(1, 1, 0, 4.0 / 3.0, 1, 0, 1, 0, false, 24);

                    for (int i = 0; i < Array_Balloon_Mesh.Vertices.Count; i++)
                    {
                        double relDb = rawDb[i] - balloonMax;
                        double displayDb = Math.Max(-30.0, relDb);

                        Vector3d dir = new Vector3d(Array_Balloon_Mesh.Vertices[i].X, Array_Balloon_Mesh.Vertices[i].Y, Array_Balloon_Mesh.Vertices[i].Z);

                        if (!dir.Unitize()) dir = new Vector3d(0, 1, 0);
                        double radius = Array_Balloon_Radius * Math.Pow(10.0, displayDb / 20.0);

                        Array_Balloon_Mesh.Vertices.SetVertex(i, arrayCenter.X + radius * dir.X, arrayCenter.Y + radius * dir.Y, arrayCenter.Z + radius * dir.Z);
                        Eto.Drawing.Color color = c_scale.GetValue(relDb, -30.0, 0.0);
                        Array_Balloon_Mesh.VertexColors.SetColor(i, color.Rb, color.Gb, color.Bb);
                    }

                    Array_Balloon_Mesh.Normals.ComputeNormals();
                    Array_Balloon_Mesh.Compact();
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

                double[] raw = new double[points.Length];

                Parallel.For(0, points.Length, i =>
                {
                    Vector3d patternDirection = points[i] - arrayCenter;

                    if (!patternDirection.Unitize())
                    {
                        raw[i] = double.NaN;
                        return;
                    }
                    Point3d virtualTarget = arrayCenter + patternDirection * Array_Reference_Distance;

                    System.Numerics.Complex sum = System.Numerics.Complex.Zero;

                    for (int e = 0; e < elementOrigins.Count; e++)
                    {
                        Vector3d worldDir = virtualTarget - elementOrigins[e];
                        double r = worldDir.Length;

                        if (r <= Rhino.RhinoMath.ZeroTolerance) continue;
                        worldDir.Unitize();

                        Hare.Geometry.Vector local = WorldToLocal(worldDir, elementAlt[e], elementAzi[e], elementAxi[e]); double directivityDb = elementLookups[e].Evaluate(local) - elementLookups[e].Maximum;

                        double gain = Math.Pow(10.0, (elementGainsDb[e] - maxGainDb + directivityDb) / 20.0);
                        double tau = elementDelaysMs[e] / 1000.0;

                        // Do not apply 1/r amplitude loss here; this is a relative directivity display.
                        double phase = -k * r - omega * tau;
                        sum += System.Numerics.Complex.FromPolarCoordinates(gain, phase);
                    }

                    double mag = sum.Magnitude;

                    if (mag <= 1E-12)
                    {
                        raw[i] = -120.0;
                    }
                    else
                    {
                        raw[i] = 20.0 * Math.Log10(mag);
                    }
                });

                double max = double.NegativeInfinity;

                for (int i = 0; i < raw.Length; i++)
                {
                    if (double.IsNaN(raw[i]) || double.IsInfinity(raw[i])) continue;
                    if (raw[i] > max) max = raw[i];
                }

                if (double.IsNegativeInfinity(max)) return;

                double[] rel = new double[points.Length];

                for (int i = 0; i < raw.Length; i++)
                {
                    rel[i] = double.IsNaN(raw[i]) || double.IsInfinity(raw[i])
                        ? double.NaN
                        : raw[i] - max;
                }

                int contour_count = Contour_Levels.Length;
                object merge_lock = new object();

                Parallel.For(0, faces.Length, () => NewLocalContourStore(contour_count), (f, state, local_lines) =>
                    {
                        FaceIndex face = faces[f];

                        if (face.IsQuad)
                        {
                            Point3d A = points[face.A];
                            Point3d B = points[face.B];
                            Point3d C = points[face.C];
                            Point3d D = points[face.D];

                            AddTriangleContours(local_lines, A, rel[face.A], B, rel[face.B], C, rel[face.C]);
                            AddTriangleContours(local_lines, A, rel[face.A], C, rel[face.C], D, rel[face.D]);
                        }
                        else
                        {
                            AddTriangleContours(
                                local_lines,
                                points[face.A], rel[face.A],
                                points[face.B], rel[face.B],
                                points[face.C], rel[face.C]);
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
                    });

                if (First_Surface_Only)
                {
                    CullContoursToFirstSurface(scene, arrayCenter);
                }

                if (Show_Contour_Labels)
                {
                    BuildContourLabels();
                }
            }

            protected override void PostDrawObjects(DrawEventArgs e)
            {
                if (Array_Balloon_Mesh != null && Array_Balloon_Mesh.Vertices.Count > 0 && Array_Balloon_Mesh.Faces.Count > 0)
                {
                    e.Display.DrawMeshFalseColors(Array_Balloon_Mesh);
                    e.Display.DrawMeshWires(Array_Balloon_Mesh, SDColor.FromArgb(80, SDColor.Black));
                }

                if (Boundary_Contour_Lines == null) return;

                for (int i = 0; i < Boundary_Contour_Lines.Length; i++)
                {
                    if (Boundary_Contour_Lines[i] == null) continue;
                    if (Boundary_Contour_Lines[i].Count == 0) continue;
                    e.Display.DrawLines(Boundary_Contour_Lines[i], Boundary_Contour_Colors[i], Contour_Line_Thickness);
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

            private void BuildBoundaryContours(Point3d source, double alt, double azi, double axi, DirectivityLookup lookup)
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

                    Hare.Geometry.Vector local = WorldToLocal(dir, alt, azi, axi);
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
                        if (!src[j].IsValid) continue;

                        Point3d mid = src[j].PointAt(0.5);
                        Vector3d dir = mid - source;
                        double target_dist = dir.Length;

                        if (target_dist <= Rhino.RhinoMath.ZeroTolerance) continue;
                        if (!dir.Unitize()) continue;

                        Ray3d ray = new Ray3d(source, dir);
                        double hit_dist = Rhino.Geometry.Intersect.Intersection.MeshRay(scene, ray);

                        if (hit_dist < 0 || double.IsNaN(hit_dist) || double.IsInfinity(hit_dist)) continue;

                        double tol = Math.Max(First_Surface_Tolerance, Contour_Mesh_Max_Edge * 0.02);

                        // If something is appreciably closer than the contour segment midpoint, this contour is behind
                        // another surface and should not be shown.
                        if (hit_dist >= target_dist - tol && hit_dist <= target_dist + tol)
                        {
                            kept.Add(src[j]);
                        }
                    }

                    Boundary_Contour_Lines[i] = kept;
                }
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

                        bool tooclose = false;

                        for (int k = 0; k < Boundary_Contour_Labels.Count; k++)
                        {
                            if (Boundary_Contour_Labels[k].Location.DistanceToSquared(p) < min_sep2)
                            {
                                tooclose = true;
                                break;
                            }
                        }

                        if (!tooclose)
                        {
                            Boundary_Contour_Labels.Add(new ContourLabel(p, Contour_Levels[i].ToString("0.#", CultureInfo.InvariantCulture) + " dB", Boundary_Contour_Colors[i]));
                            labels_added++;
                            if (labels_added >= Labels_Per_Level) break;
                        }
                    }
                }
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

                StringBuilder strbldr = new StringBuilder();

                foreach (RhinoObject obj in doc.Objects.GetObjectList(ObjectType.AnyObject))
                {
                    if (obj == null) continue;
                    if (obj.Geometry == null) continue;
                    if (obj.IsHidden) continue;

                    string name = obj.Attributes.Name;
                    if (name == "Acoustical Source") continue;
                    if (name == "Acoustical Receiver") continue;

                    BoundingBox bb = obj.Geometry.GetBoundingBox(true);

                    strbldr.Append(obj.Id);
                    strbldr.Append('|');
                    strbldr.Append(obj.ObjectType);
                    strbldr.Append('|');
                    strbldr.Append(bb.Min.X.ToString("R", CultureInfo.InvariantCulture));
                    strbldr.Append(',');
                    strbldr.Append(bb.Min.Y.ToString("R", CultureInfo.InvariantCulture));
                    strbldr.Append(',');
                    strbldr.Append(bb.Min.Z.ToString("R", CultureInfo.InvariantCulture));
                    strbldr.Append('|');
                    strbldr.Append(bb.Max.X.ToString("R", CultureInfo.InvariantCulture));
                    strbldr.Append(',');
                    strbldr.Append(bb.Max.Y.ToString("R", CultureInfo.InvariantCulture));
                    strbldr.Append(',');
                    strbldr.Append(bb.Max.Z.ToString("R", CultureInfo.InvariantCulture));
                    strbldr.Append(';');
                }

                string signature = strbldr.ToString();

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
                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double d)) return d;
                if (double.TryParse(value, out d)) return d;

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
        }
    }
}