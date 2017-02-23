//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2015, Arthur van der Harten 
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

using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        /// <summary>
        /// Handles Feedback for Auralisation.
        /// </summary>
        public class AuralisationConduit : Rhino.Display.DisplayConduit
        {
            public AuralisationConduit()
            {
                Instance = this;
            }

            ///<summary>The only instance of this conduit.</summary>
            public static AuralisationConduit Instance
            {
                get;
                private set;
            }

            private void ClearConduit(object sender, EventArgs e)
            {
                this.Enabled = false;
            }

            protected override void DrawForeground(Rhino.Display.DrawEventArgs e)
            {
                if (Recs != null) e.Display.DrawPointCloud(Srcs, 5, System.Drawing.Color.Green);
                if (Srcs != null) e.Display.DrawPointCloud(Srcs, 5, System.Drawing.Color.Red);
                if (Dir != null) e.Display.DrawLineArrow(Dir, System.Drawing.Color.Red, 3, .1);
                if (Reflections != null) foreach (Rhino.Geometry.Polyline L in Reflections) e.Display.DrawDottedPolyline(L.AsEnumerable<Point3d>(), System.Drawing.Color.GreenYellow, false);
                if (Speakers != null)
                    foreach (Rhino.Geometry.Line Sp in Speakers)
                    {
                        //TODO: Draw Speaker Cabinets, for clarity.
                        Rhino.Geometry.Box BB = new Box(new Rhino.Geometry.Plane(Sp.From, Sp.UnitTangent), new BoundingBox(-.125, -.15, -.18, .125, .15, .18));
                        e.Display.DrawBox(BB, System.Drawing.Color.Blue);
                        e.Display.DrawLineArrow(Sp, System.Drawing.Color.Blue, 1, .05);
                    }
            }

            PointCloud Recs;
            PointCloud Srcs;
            List<Polyline> Reflections;
            List<Line> Speakers;
            Line Dir;

            public void add_Receivers(IEnumerable<Hare.Geometry.Point> pts)
            {
                this.Enabled = true;
                List<Point3d> PTS = new List<Point3d>();
                foreach (Hare.Geometry.Point p in pts) PTS.Add(Utilities.RC_PachTools.HPttoRPt(p));
                Recs = new PointCloud(PTS);
            }

            public void add_Sources(IEnumerable<Hare.Geometry.Point> pts)
            {
                List<Rhino.Geometry.Point3d> PTS = new List<Point3d>();
                foreach(Hare.Geometry.Point p in pts) PTS.Add(Utilities.RC_PachTools.HPttoRPt(p));
                add_Sources(PTS);
            }

            public void add_Sources(IEnumerable<Rhino.Geometry.Point3d> pts)
            {
                this.Enabled = true;
                Srcs = new PointCloud(pts);
            }

            public void add_Reflections(IEnumerable<Rhino.Geometry.Polyline> refs)
            {
                this.Enabled = true;
                Reflections = refs.ToList<Rhino.Geometry.Polyline>();
            }

            public void add_Speakers(IEnumerable<Hare.Geometry.Point> pts, IEnumerable<Rhino.Geometry.Vector3d> vec)
            {
                this.Enabled = true;
                Speakers = new List<Line>();
                for (int i = 0; i < pts.Count<Hare.Geometry.Point>(); i++)
                {
                    Hare.Geometry.Vector V = new Hare.Geometry.Vector(vec.ElementAt<Vector3d>(i).X, vec.ElementAt<Vector3d>(i).Y, vec.ElementAt<Vector3d>(i).Z);
                    Speakers.Add(new Line(Utilities.RC_PachTools.HPttoRPt(pts.ElementAt(i)), Utilities.RC_PachTools.HPttoRPt(pts.ElementAt(i) + V)));
                }
            }

            public void set_direction(Rhino.Geometry.Point3d rec, Rhino.Geometry.Point3d dir)
            {
                this.Enabled = true;
                Rhino.Geometry.Vector3d V = new Vector3d(dir.X, dir.Y, dir.Z);
                V.Unitize();
                Dir = new Rhino.Geometry.Line(rec, rec + V);
            }

            public void set_direction(Rhino.Geometry.Point3d rec, Rhino.Geometry.Vector3d V)
            {
                this.Enabled = true;
                V.Unitize();
                Dir = new Rhino.Geometry.Line(rec, rec + V);
            }
        }
    }
}