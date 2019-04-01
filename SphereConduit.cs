//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2019, Arthur van der Harten 
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

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        /// <summary>
        /// A conduit used to display spatial data.
        /// </summary>
        public class SphereConduit : DisplayConduit
        {
            Rhino.Geometry.Mesh Sphere;
            public Pach_Graphics.colorscale C;
            public Sphere_Plot plot;
            Hare.Geometry.Point center;

            public SphereConduit(Sphere_Plot plot_in, Hare.Geometry.Point center_in, Pach_Graphics.colorscale C_in, double[] V_Bounds_in)
            {
                plot = plot_in;
                center = center_in;
                C = C_in;
                Instance = this;
            }

            public static SphereConduit Instance
            {
                get;
                private set;
            }

            protected override void PostDrawObjects(DrawEventArgs e)
            {
                e.Display.DrawMeshFalseColors(Sphere);
            }

            /// <summary>
            /// Allows user to change the colors of the particles.
            /// </summary>
            /// <param name="Colors"></param>
            /// <param name="Values"></param>
            public void SetColorScale(Pach_Graphics.colorscale C_in)
            {
                C = C_in;
            }

            public void Data_in(double[] magnitude, double[] bounds, double display_diameter)
            {
                Mesh m = Utilities.RC_PachTools.Hare_to_RhinoMesh(plot.Output(magnitude, bounds[0], bounds[1], display_diameter), true);
                for (int i = 0; i < magnitude.Length; i++) m.VertexColors.Add(C.GetValue(magnitude[i], bounds[0], bounds[1]));
                m.UnifyNormals();
                Sphere = m;
                this.Enabled = true;
            }
        }
    }
}