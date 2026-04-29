//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2025, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit 
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

using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        ///</summary> 
        [System.Runtime.InteropServices.Guid("49d3422c-aab5-4eb3-ac15-3796be13482c")]
        public class Pach_Receiver_Object : Command
        {
            public Pach_Receiver_Object()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_Receiver_Object Instance
            {
                get;
                private set;
            }

            public override string EnglishName
            {
                get { return "Insert_Receiver"; }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
            {
                ReceiverConduit m_Receiver_conduit = ReceiverConduit.Instance;
                Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
                ConfigurationSectionCollection S = config.Sections;


                Rhino.Geometry.Point3d location;
                if (Rhino.Input.RhinoGet.GetPoint("Select Receiver Position", false, out location) != Result.Success) return Result.Cancel;

                Rhino.DocObjects.RhinoObject rhObj = doc.Objects.Find(doc.Objects.AddPoint(location));
                rhObj.Attributes.Name = "Acoustical Receiver";
                rhObj.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(new double[] { 120, 120, 120, 120, 120, 120, 120, 120 }));

                Rhino.RhinoDoc.ActiveDoc.Objects.ModifyAttributes(rhObj, rhObj.Attributes, true);

                m_Receiver_conduit.SetReceiver(rhObj);
                doc.Views.Redraw();

                return Result.Success;
            }
        }

        /// <summary>
        /// Handles the receiver objects, and displays an icon instead of the point object it is based on.
        /// </summary>
        public class ReceiverConduit : Rhino.Display.DisplayConduit
        {
            private bool m_bHandlerAdded = false;
            private List<System.Guid> m_id_list = new List<System.Guid>();
            private readonly MicrophoneGlyph Mic = new MicrophoneGlyph(0.1);
            private Rhino.Geometry.Line Dir;
            public ReceiverConduit()
            : base()
            {
                //System.Drawing.Bitmap RSbmp = Properties.Resources.Receiver_Selected;
                //System.Drawing.Bitmap RUbmp = Properties.Resources.Receiver;
                //RSbmp.MakeTransparent(System.Drawing.Color.Black);
                //RUbmp.MakeTransparent(System.Drawing.Color.Black);
                //RS = new DisplayBitmap(RSbmp);
                //RU = new DisplayBitmap(RUbmp);

                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static ReceiverConduit Instance
            {
                get;
                private set;
            }

            /// <summary>
            /// Sets up watchers, which trigger events in the handling of the receiver objects.
            /// </summary>
            /// <param name="bAdd"></param>
            private void SetupEventHandlers(bool bAdd)
            {
                if (bAdd)
                {
                    if ((!m_bHandlerAdded))
                    {
                        Rhino.RhinoDoc.AddRhinoObject += OnCopyObject;
                        Rhino.RhinoDoc.DeleteRhinoObject += OnDeleteObject;
                        Rhino.RhinoDoc.ReplaceRhinoObject += OnReplaceObject;
                        Rhino.RhinoDoc.BeginOpenDocument += ClearConduit;
                        Rhino.RhinoDoc.NewDocument += ClearConduit;
                    }
                }
                else
                {
                    Rhino.RhinoDoc.AddRhinoObject -= OnCopyObject;
                    Rhino.RhinoDoc.DeleteRhinoObject -= OnDeleteObject;
                    Rhino.RhinoDoc.ReplaceRhinoObject -= OnReplaceObject;
                    Rhino.RhinoDoc.BeginOpenDocument -= ClearConduit;
                    Rhino.RhinoDoc.NewDocument -= ClearConduit;
                }

                m_bHandlerAdded = bAdd;
            }

            private bool m_bReplaceCalled = false;

            private void ClearConduit(object sender, EventArgs e)
            {
                m_id_list.Clear();
                this.Enabled = false;
                // we don't want to watch for events any more 
                SetupEventHandlers(false);
            }

            private void OnReplaceObject(object sender, Rhino.DocObjects.RhinoReplaceObjectEventArgs e)
            {
                m_bReplaceCalled = true;
            }

            /// <summary>
            /// What to do if an object is copied...
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="rhObject"></param>
            private void OnCopyObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
            {
                if (e.TheObject.Attributes.Name == "Acoustical Receiver") SetReceiver(e.TheObject);
            }

            /// <summary>
            /// What to do if an object is deleted...
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="rhObject"></param>
            private void OnDeleteObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
            {
                // skip this if a replace was called beforehand 
                if ((m_bReplaceCalled))
                {
                    m_bReplaceCalled = false;
                    return;
                }

                foreach (System.Guid m_id in m_id_list)
                {
                    if ((m_id != System.Guid.Empty && e.TheObject != null))
                    {
                        if ((e.TheObject.Attributes.ObjectId == m_id))
                        {
                            m_id_list.Remove(m_id);
                            if (m_id_list.Count < 1)
                            {
                                this.Enabled = false;
                                // we don't want to watch for events any more 
                                SetupEventHandlers(false);
                            }
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// Adds a receiver to the conduit.
            /// </summary>
            /// <param name="rhino_object"></param>
            public void SetReceiver(Rhino.DocObjects.RhinoObject rhino_object)
            {
                if ((rhino_object == null))
                {
                    if (m_id_list.Count == 0)
                    {
                        SetupEventHandlers(false);
                        this.Enabled = false;
                    }
                }
                else
                {
                    SetupEventHandlers(true);
                    this.Enabled = true;
                    if (!m_id_list.Contains(rhino_object.Attributes.ObjectId)) m_id_list.Add(rhino_object.Attributes.ObjectId);
                }
            }

            protected override void DrawForeground(DrawEventArgs e)
            {
                int index = 0;

                if (Dir != null)
                    e.Display.DrawLineArrow(Dir, System.Drawing.Color.Red, 3, .1);

                foreach (Guid G in m_id_list)
                {
                    Rhino.DocObjects.RhinoObject rhobj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(G);
                    if (rhobj == null)
                    {
                        m_id_list.Remove(G);
                        continue;
                    }
                    // Draw our own representation of the object 
                    //Rhino.Geometry.Point2d screen_pt = e.Display.Viewport.WorldToClient(rhobj.Geometry.GetBoundingBox(true).Min);
                    //if ((rhobj.IsSelected(false) != 0))
                    //{
                    //    e.Display.DrawSprite(RS, rhobj.Geometry.GetBoundingBox(true).Min, 0.5f, true);// screen_pt, 32.0f);
                    //    e.Display.Draw2dText(index.ToString(), Color.Yellow, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                    //}
                    //else
                    //{
                    //    e.Display.DrawSprite(RU, rhobj.Geometry.GetBoundingBox(true).Min, 0.5f, true);// screen_pt, 32.0f);
                    //    e.Display.Draw2dText(index.ToString(), Color.Black, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                    //}

                    Point3d pt = rhobj.Geometry.GetBoundingBox(true).Min;
                    Point2d screen_pt = e.Display.Viewport.WorldToClient(pt);
                    bool selected = rhobj.IsSelected(false) != 0;

                    Color text = selected ? Color.Yellow : Color.Black;
                    Color body = selected ? Color.Yellow : Color.Black;
                    Color cable = selected ? Color.Goldenrod : Color.DimGray;
                    Color collar = Color.Black;
                    Color waves = selected ? Color.OrangeRed : Color.Red;

                    foreach (Line l in Mic.GetBodyLines(pt)) e.Display.DrawLine(l, body, 2);
                    foreach (Circle c in Mic.GetBodyRings(pt)) e.Display.DrawCircle(c, body, 2);
                    foreach (Circle c in Mic.GetCollarRings(pt)) e.Display.DrawCircle(c, collar, 3);
                    foreach (Line l in Mic.GetCable(pt)) e.Display.DrawLine(l, cable, 2);
                    foreach (Line l in Mic.GetIncomingWaveStar(pt)) e.Display.DrawLine(l, waves, 2);

                    e.Display.Draw2dText(index.ToString(), text, new Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 18, "Arial");
                    index++;
                }
            }

            public void set_direction(Rhino.Geometry.Point3d rec, Rhino.Geometry.Point3d dir)
            {
                Rhino.Geometry.Vector3d V = rec - dir;
                V.Unitize();
                Dir = new Rhino.Geometry.Line(rec, rec + dir);
            }

            public void set_direction(Rhino.Geometry.Point3d rec, Rhino.Geometry.Vector3d V)
            {
                V.Unitize();
                Dir = new Rhino.Geometry.Line(rec, rec + V);
            }

            /// <summary>
            /// The ids of the receiver point objects. Rhino objects all have GUIDs.
            /// </summary>
            public List<System.Guid> UUID
            {
                get
                {
                    return m_id_list;
                }
            }

            private class MicrophoneGlyph
            {
                private readonly double r;

                public MicrophoneGlyph(double radius)
                {
                    r = radius;
                }

                public List<Line> GetBodyLines(Point3d p)
                {
                    List<Line> lines = new List<Line>();

                    // p is the acoustic receiver point, near the omni capsule.
                    // The microphone hangs downward from this point.
                    double topTubeR = r * 0.055;
                    double topTubeTop = r * 0.42;
                    double topTubeBottom = -r * 0.42;

                    double taperTopR = r * 0.075;
                    double taperBottomR = r * 0.22;
                    double taperTop = topTubeBottom;
                    double taperBottom = -r * 1.12;

                    double bodyR = r * 0.22;
                    double bodyTop = taperBottom;
                    double bodyBottom = -r * 2.75;

                    AddVerticalCylinderLines(lines, p, topTubeR, topTubeBottom, topTubeTop, 6);
                    AddFrustumLines(lines, p, taperBottomR, taperTopR, taperBottom, taperTop, 6);
                    AddVerticalCylinderLines(lines, p, bodyR, bodyBottom, bodyTop, 6);

                    // Subtle lengthwise highlight/groove lines on the main body.
                    for (int i = 0; i < 3; i++)
                    {
                        double a = (2.0 * Math.PI * i / 3.0) + Math.PI / 6.0;
                        Vector3d v = new Vector3d(Math.Cos(a), Math.Sin(a), 0) * bodyR * 0.72;

                        lines.Add(new Line(
                            p + v + new Vector3d(0, 0, bodyTop - r * 0.10),
                            p + v + new Vector3d(0, 0, bodyBottom + r * 0.12)
                        ));
                    }

                    return lines;
                }

                public List<Circle> GetBodyRings(Point3d p)
                {
                    List<Circle> rings = new List<Circle>();

                    double topTubeR = r * 0.055;
                    double topTubeTop = r * 0.42;
                    double topTubeBottom = -r * 0.42;

                    double taperTopR = r * 0.075;
                    double taperBottomR = r * 0.22;
                    double taperBottom = -r * 1.12;

                    double bodyR = r * 0.22;
                    double bodyBottom = -r * 2.75;

                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, topTubeTop), Vector3d.ZAxis), topTubeR));
                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, topTubeBottom), Vector3d.ZAxis), topTubeR));

                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, topTubeBottom), Vector3d.ZAxis), taperTopR));
                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, taperBottom), Vector3d.ZAxis), taperBottomR));

                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, taperBottom), Vector3d.ZAxis), bodyR));
                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, bodyBottom), Vector3d.ZAxis), bodyR));

                    // Bottom cap detail.
                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, bodyBottom + r * 0.12), Vector3d.ZAxis), bodyR * 0.96));

                    return rings;
                }

                public List<Circle> GetCollarRings(Point3d p)
                {
                    List<Circle> rings = new List<Circle>();

                    double bodyR = r * 0.225;
                    double collarZ = -r * 1.18;

                    // Several close rings read as the black name/collar band in wire display.
                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, collarZ - r * 0.06), Vector3d.ZAxis), bodyR));
                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, collarZ), Vector3d.ZAxis), bodyR));
                    rings.Add(new Circle(new Plane(p + new Vector3d(0, 0, collarZ + r * 0.06), Vector3d.ZAxis), bodyR));

                    return rings;
                }

                public List<Line> GetCable(Point3d p)
                {
                    List<Line> lines = new List<Line>();

                    double bodyBottom = -r * 2.75;

                    Point3d micBottom = p + new Vector3d(0, 0, bodyBottom);

                    // Place a small coiled cable bundle below and slightly to the side of the mic.
                    // The coil lies mostly in the XY plane, with a small vertical pitch so the
                    // turns are not exactly coincident.
                    Point3d coilCenter = p + new Vector3d(r * 0.95, -r * 0.10, bodyBottom - r * 0.70);

                    double radiusX0 = r * 0.28;
                    double radiusY0 = r * 0.16;
                    double radiusX1 = r * 0.92;
                    double radiusY1 = r * 0.48;

                    double turns = 3.35;
                    int segments = 150;

                    // Start the spiral near the upper-left side of the coil, close to the mic.
                    double startAngle = Math.PI * 0.92;

                    Point3d coilStart = CoilPoint(
                        coilCenter,
                        radiusX0, radiusY0,
                        radiusX1, radiusY1,
                        startAngle,
                        0.0,
                        turns
                    );

                    // Cable from mic body into the coiled bundle.
                    AddCubic(
                        lines,
                        micBottom,
                        p + new Vector3d(r * 0.10, -r * 0.03, bodyBottom - r * 0.20),
                        p + new Vector3d(r * 0.36, -r * 0.10, bodyBottom - r * 0.46),
                        coilStart,
                        18
                    );

                    // Continuous flattened spiral: this is the actual coiled cable.
                    Point3d last = coilStart;

                    for (int i = 1; i <= segments; i++)
                    {
                        double t = (double)i / segments;
                        double a = startAngle - 2.0 * Math.PI * turns * t;

                        Point3d next = CoilPoint(
                            coilCenter,
                            radiusX0, radiusY0,
                            radiusX1, radiusY1,
                            a,
                            t,
                            turns
                        );

                        lines.Add(new Line(last, next));
                        last = next;
                    }

                    // Free cable tail leaving the outer turn, like the reference image.
                    Point3d tailStart = last;
                    Point3d tailEnd = p + new Vector3d(r * 1.95, r * 0.24, bodyBottom - r * 0.88);

                    AddCubic(
                        lines,
                        tailStart,
                        p + new Vector3d(r * 1.34, -r * 0.04, bodyBottom - r * 0.92),
                        p + new Vector3d(r * 1.62, r * 0.22, bodyBottom - r * 0.78),
                        tailEnd,
                        24
                    );

                    return lines;
                }

                private static Point3d CoilPoint(Point3d center, double rx0, double ry0, double rx1, double ry1, double angle, double t, double turns)
                {
                    // Radius increases with t, creating an Archimedean-style elliptical spiral.
                    double rx = rx0 + (rx1 - rx0) * t;
                    double ry = ry0 + (ry1 - ry0) * t;

                    // Very slight vertical drop through the coil, just enough to avoid
                    // every pass landing exactly on the same apparent plane.
                    double zPitch = -0.025 * t;

                    return center + new Vector3d(
                        rx * Math.Cos(angle),
                        ry * Math.Sin(angle),
                        zPitch
                    );
                }

                public List<Line> GetIncomingWaveStar(Point3d p)
                {
                    List<Line> lines = new List<Line>();

                    // Sharper six-point red incident-wave glyph.
                    // The tips are cusps. The connecting arcs scoop inward,
                    // so the outside edge reads as concave between points.
                    int lobes = 6;
                    int steps = 10;

                    double tipR = r * 0.62;
                    double valleyR = r * 0.25;
                    double z = r * 0.02;

                    for (int i = 0; i < lobes; i++)
                    {
                        double tipA = 2.0 * Math.PI * i / lobes;
                        double nextTipA = 2.0 * Math.PI * (i + 1) / lobes;
                        double valleyA = tipA + Math.PI / lobes;

                        Point3d tip0 = PolarPoint(p, tipR, tipA, z);
                        Point3d valley = PolarPoint(p, valleyR, valleyA, z);
                        Point3d tip1 = PolarPoint(p, tipR, nextTipA, z);

                        // Controls pulled strongly inward for concave-outside sides.
                        Point3d c0 = PolarPoint(p, valleyR * 0.92, tipA + Math.PI / lobes * 0.30, z);
                        Point3d c1 = PolarPoint(p, valleyR * 0.75, valleyA - Math.PI / lobes * 0.18, z);

                        Point3d c2 = PolarPoint(p, valleyR * 0.75, valleyA + Math.PI / lobes * 0.18, z);
                        Point3d c3 = PolarPoint(p, valleyR * 0.92, nextTipA - Math.PI / lobes * 0.30, z);

                        AddCubic(lines, tip0, c0, c1, valley, steps);
                        AddCubic(lines, valley, c2, c3, tip1, steps);
                    }

                    // Short incident ticks aimed at the capsule.
                    for (int i = 0; i < lobes; i++)
                    {
                        double a = 2.0 * Math.PI * i / lobes;

                        Point3d outer = PolarPoint(p, tipR + r * 0.16, a, z);
                        Point3d inner = PolarPoint(p, tipR + r * 0.02, a, z);

                        lines.Add(new Line(outer, inner));
                    }

                    return lines;
                }

                private static Point3d LoopPoint(Point3d c, double radius, double angle)
                {
                    return c + new Vector3d(
                        radius * Math.Cos(angle),
                        radius * 0.12 * Math.Sin(2.0 * angle),
                        radius * Math.Sin(angle)
                    );
                }

                private static Point3d PolarPoint(Point3d p, double radius, double angle, double z)
                {
                    return p + new Vector3d(
                        radius * Math.Cos(angle),
                        radius * Math.Sin(angle),
                        z
                    );
                }

                private static void AddVerticalCylinderLines(List<Line> lines, Point3d p, double radius, double z0, double z1, int count)
                {
                    for (int i = 0; i < count; i++)
                    {
                        double a = 2.0 * Math.PI * i / count;
                        Vector3d v = new Vector3d(Math.Cos(a), Math.Sin(a), 0) * radius;

                        lines.Add(new Line(
                            p + v + new Vector3d(0, 0, z0),
                            p + v + new Vector3d(0, 0, z1)
                        ));
                    }
                }

                private static void AddFrustumLines(List<Line> lines, Point3d p, double r0, double r1, double z0, double z1, int count)
                {
                    for (int i = 0; i < count; i++)
                    {
                        double a = 2.0 * Math.PI * i / count;
                        Vector3d v0 = new Vector3d(Math.Cos(a), Math.Sin(a), 0) * r0;
                        Vector3d v1 = new Vector3d(Math.Cos(a), Math.Sin(a), 0) * r1;

                        lines.Add(new Line(
                            p + v0 + new Vector3d(0, 0, z0),
                            p + v1 + new Vector3d(0, 0, z1)
                        ));
                    }
                }

                private static void AddCubic(List<Line> lines, Point3d p0, Point3d p1, Point3d p2, Point3d p3, int steps)
                {
                    Point3d last = p0;

                    for (int i = 1; i <= steps; i++)
                    {
                        double t = (double)i / steps;
                        Point3d next = CubicPoint(p0, p1, p2, p3, t);

                        lines.Add(new Line(last, next));
                        last = next;
                    }
                }

                private static Point3d CubicPoint(Point3d p0, Point3d p1, Point3d p2, Point3d p3, double t)
                {
                    Point3d a = Lerp(p0, p1, t);
                    Point3d b = Lerp(p1, p2, t);
                    Point3d c = Lerp(p2, p3, t);

                    Point3d d = Lerp(a, b, t);
                    Point3d e = Lerp(b, c, t);

                    return Lerp(d, e, t);
                }

                private static Point3d Lerp(Point3d a, Point3d b, double t)
                {
                    return a + (b - a) * t;
                }
            }
        }
    }
}