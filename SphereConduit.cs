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

using Rhino.Geometry;
using Rhino.Display;
using System.Collections.Generic;
using Pachyderm_Acoustic.Numeric.TimeDomain;
using Hare.Geometry;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using System;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        /// <summary>
        /// A conduit used to display spatial data.
        /// </summary>
        public class HemisphereConduit : DisplayConduit
        {
            Rhino.Geometry.Mesh Sphere;
            public Pach_Graphics.Colorscale C;
            public Hemisphere_Plot plot;
            Hare.Geometry.Point center;

            public HemisphereConduit(Hemisphere_Plot plot_in, Hare.Geometry.Point center_in, Pach_Graphics.Colorscale C_in, double[] V_Bounds_in)
            {
                plot = plot_in;
                center = center_in;
                C = C_in;
                Instance = this;
            }

            public static HemisphereConduit Instance
            {
                get;
                private set;
            }

            protected override void PostDrawObjects(DrawEventArgs e)
            {
                if (Sphere != null)
                {
                    e.Display.DrawMeshFalseColors(Sphere);                    
                }
            }

            /// <summary>
            /// Allows user to change the colors of the particles.
            /// </summary>
            /// <param name="Colors"></param>
            /// <param name="Values"></param>
            public void SetColorScale(Pach_Graphics.Colorscale C_in)
            {
                C = C_in;
            }

            public void Data_in(double[] magnitude, double[] bounds, double display_diameter)
            {
                Mesh m = Utilities.RCPachTools.HaretoRhinoMesh(plot.Output(magnitude, bounds[0], bounds[1], display_diameter), true);
                for (int i = 0; i < magnitude.Length; i++)
                {
                    Eto.Drawing.Color c = C.GetValue(magnitude[i], bounds[0], bounds[1]);
                    m.VertexColors.Add(c.Rb, c.Gb, c.Bb);
                }
                m.UnifyNormals();
                Sphere = m;
                this.Enabled = true;
            }
        }

        public class ReceiverSphereConduit : DisplayConduit
        {
            Rhino.Geometry.Mesh Sphere;
            public Sphere_Plot plot;
            double[][][] ETCs;
            Point3d center;
            TextEntity TE00, TE30, TE60, TE90, DB00, DB10, DB20, DB30;
            Point3d[] L00 = new Point3d[3], L30 = new Point3d[3], L60 = new Point3d[3], L90 = new Point3d[3];

            public ReceiverSphereConduit()
            {
                plot = new Sphere_Plot(new Hare.Geometry.Point(0,0,0));
                TE00 = new TextEntity();
                TE00.TextHeight = .015;
                TE00.Text = "0";
                TE30 = new TextEntity();
                TE30.TextHeight = .015;
                TE30.Text = "30";
                TE60 = new TextEntity();
                TE60.TextHeight = .015;
                TE60.Text = "60";
                TE90 = new TextEntity();
                TE90.TextHeight = .015;
                TE90.Text = "90";

                L00[0] = new Point3d(.3, 0, 0);
                L00[1] = new Point3d(.3, 0, 0);
                L00[2] = new Point3d(0, .3, 0);
                L30[0] = new Point3d(.3 * Math.Cos(Math.PI * .166666667), .3 * Math.Sin(Math.PI * .166666667), 0);
                L30[1] = new Point3d(.3 * Math.Cos(Math.PI * .166666667), 0, .3 * Math.Sin(Math.PI * .166666667));
                L30[2] = new Point3d(0, .3 * Math.Cos(Math.PI * .166666667), .3 * Math.Sin(Math.PI * .166666667));
                L60[0] = new Point3d(.3 * Math.Cos(Math.PI * .33333333), .3 * Math.Sin(Math.PI * .33333333), 0);
                L60[1] = new Point3d(.3 * Math.Cos(Math.PI * .33333333), 0, .3 * Math.Sin(Math.PI * .33333333));
                L60[2] = new Point3d(0, .3 * Math.Cos(Math.PI * .33333333), .3 * Math.Sin(Math.PI * .33333333));
                L90[0] = new Point3d(0, 0.3, 0);
                L90[1] = new Point3d(0, 0, 0.3);
                L90[2] = new Point3d(0, 0, 0.3);

                DB00 = new TextEntity();
                DB10 = new TextEntity();
                DB20 = new TextEntity();
                DB30 = new TextEntity();
                DB00.Text = "0 dB";
                DB10.Text = "-10 dB";
                DB20.Text = "-20 dB";
                DB30.Text = "-30 dB";
                DB00.TextHeight = 0.020;
                DB10.TextHeight = 0.020;
                DB20.TextHeight = 0.020;
                DB30.TextHeight = 0.020;
                Instance = this;
            }

            public static ReceiverSphereConduit Instance
            {
                get;
                private set;
            }

            protected override void PostDrawObjects(DrawEventArgs e)
            {
                if (Sphere != null)
                {
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldXY, center, .1), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldZX, center, .1), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldYZ, center, .1), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldXY, center, .2), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldZX, center, .2), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldYZ, center, .2), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldXY, center, .3), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldZX, center, .3), System.Drawing.Color.Black);
                    e.Display.DrawCircle(new Circle(Rhino.Geometry.Plane.WorldYZ, center, .3), System.Drawing.Color.Black);

                    List<Line> lines = new List<Line>();
                    lines.Add(new Line(center, center + L00[0]));
                    lines.Add(new Line(center, center + L30[0]));
                    lines.Add(new Line(center, center + L60[0]));
                    lines.Add(new Line(center, center + L90[0]));
                    lines.Add(new Line(center, center + L00[1]));
                    lines.Add(new Line(center, center + L30[1]));
                    lines.Add(new Line(center, center + L60[1]));
                    lines.Add(new Line(center, center + L90[1]));
                    lines.Add(new Line(center, center + L00[2]));
                    lines.Add(new Line(center, center + L30[2]));
                    lines.Add(new Line(center, center + L60[2]));
                    lines.Add(new Line(center, center + L90[2]));
                    //lines.Add(new Line(center, center + new Point3d(0, 1, 0)));
                    //lines.Add(new Line(center, center + new Point3d(0, .3 * Math.Cos(Math.PI * .33333), .3 * Math.Sin(Math.PI * .33333))));
                    //lines.Add(new Line(center, center + new Point3d(0, .3 * Math.Cos(Math.PI * .66666), .3 * Math.Sin(Math.PI * .66666))));
                    //lines.Add(new Line(center, center + new Point3d(0, 0, 1)));
                    //lines.Add(new Line(center, center + new Point3d(.3 * Math.Cos(Math.PI * .33333), 0, .3 * Math.Sin(Math.PI * .33333))));
                    //lines.Add(new Line(center, center + new Point3d(.3 * Math.Cos(Math.PI * .66666), 0, .3 * Math.Sin(Math.PI * .66666))));

                    e.Display.DrawLines(lines, System.Drawing.Color.Black);
                    Point3d XYadjust = new Point3d(0, 0.015, 0);
                    Rhino.Geometry.Plane p00 = Rhino.Geometry.Plane.WorldXY;
                    p00.Origin = center + L00[0] + XYadjust;
                    TE00.Plane = p00;
                    e.Display.DrawText(TE00, System.Drawing.Color.Black);
                    p00.Origin = center + L30[0] + XYadjust;
                    TE30.Plane = p00;
                    e.Display.DrawText(TE30, System.Drawing.Color.Black);
                    p00.Origin = center + L60[0] + XYadjust;
                    TE60.Plane = p00;
                    e.Display.DrawText(TE60, System.Drawing.Color.Black);
                    p00.Origin = center + L90[0] + XYadjust;
                    TE90.Plane = p00;
                    e.Display.DrawText(TE90, System.Drawing.Color.Black);

                    p00 = Rhino.Geometry.Plane.WorldZX;
                    p00.Origin = center + L00[1];
                    TE00.Plane = p00;
                    TE00.Rotate(Math.PI / 2, new Vector3d(0, 1, 0), p00.Origin);
                    e.Display.DrawText(TE00, System.Drawing.Color.Black);
                    p00.Origin = center + L30[1];
                    TE30.Plane = p00;
                    TE30.Rotate(Math.PI / 2, new Vector3d(0, 1, 0), p00.Origin);
                    e.Display.DrawText(TE30, System.Drawing.Color.Black);
                    p00.Origin = center + L60[1];
                    TE60.Plane = p00;
                    TE60.Rotate(Math.PI / 2, new Vector3d(0, 1, 0), p00.Origin);
                    e.Display.DrawText(TE60, System.Drawing.Color.Black);
                    p00.Origin = center + L90[1];
                    TE90.Plane = p00;
                    TE90.Rotate(Math.PI / 2, new Vector3d(0, 1, 0), p00.Origin);
                    e.Display.DrawText(TE90, System.Drawing.Color.Black);

                    Point3d YZadjust = new Point3d(0, 0, 0.015);
                    p00 = Rhino.Geometry.Plane.WorldYZ;
                    p00.Origin = center + L00[2] + YZadjust;
                    TE00.Plane = p00;
                    e.Display.DrawText(TE00, System.Drawing.Color.Black);
                    p00.Origin = center + L30[2] + YZadjust;
                    TE30.Plane = p00;
                    e.Display.DrawText(TE30, System.Drawing.Color.Black);
                    p00.Origin = center + L60[2] + YZadjust;
                    TE60.Plane = p00;
                    e.Display.DrawText(TE60, System.Drawing.Color.Black);
                    p00.Origin = center + L90[2] + YZadjust;
                    TE90.Plane = p00;
                    e.Display.DrawText(TE90, System.Drawing.Color.Black);
                    
                    Rhino.Geometry.Plane DBP = Rhino.Geometry.Plane.WorldXY;
                    DBP.Rotate(Math.PI / 2, new Vector3d(0, 0, 1));
                    DBP.Origin = center;
                    DB30.Plane = DBP;
                    DBP.OriginX -= 0.1;
                    DB20.Plane = DBP;
                    DBP.OriginX -= 0.1;
                    DB10.Plane = DBP;
                    DBP.OriginX -= 0.1;
                    DB00.Plane = DBP;

                    e.Display.DrawText(DB00, System.Drawing.Color.Blue);
                    e.Display.DrawText(DB10, System.Drawing.Color.Blue);
                    e.Display.DrawText(DB20, System.Drawing.Color.Blue);
                    e.Display.DrawText(DB30, System.Drawing.Color.Blue);

                    e.Display.DrawMeshWires(Sphere, System.Drawing.Color.Red);
                }
            }

            /// <summary>
            /// Allows user to change the colors of the particles.
            /// </summary>
            /// <param name="Colors"></param>
            /// <param name="Values"></param>

            //public void Data_in(double[] magnitude, double[] bounds, double display_diameter)
            //{
            //    Mesh m = Utilities.RCPachTools.HaretoRhinoMesh(plot.Output(magnitude, bounds[0], bounds[1], display_diameter), true);
            //    m.UnifyNormals();
            //    Sphere = m;
            //    this.Enabled = true;
            //}

            public void Data_in(Topology sphere, Hare.Geometry.Point Center)
            {
                center = RCPachTools.HPttoRPt(Center);
                Mesh m = Utilities.RCPachTools.HaretoRhinoMesh(sphere, true);
                m.UnifyNormals();
                Sphere = m;
                this.Enabled = true;
            }
        }
    }
}