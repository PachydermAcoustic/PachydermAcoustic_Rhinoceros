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

using Rhino.Geometry;
using Rhino.Display;
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Visualization;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        /// <summary>
        /// A conduit used to display spatial data.
        /// </summary>
        public class WaveConduit : DisplayConduit
        {
            List<double> SPLList;
            //List<Point3d> particles;
            ParticleRays[] PR;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 2, 2);
            Rhino.Geometry.PointCloud PC = new PointCloud();
            bool Mesh_Vis = false;
            bool Section_Vis = false;
            List<Mesh> DisplayMesh;

            public WaveConduit(Pach_Graphics.colorscale C_in, double[] V_Bounds_in)
            {
                C = C_in;
                V_Bounds = V_Bounds_in;    
            }

            public WaveConduit(ParticleRays[] PR_in, Pach_Graphics.colorscale C_in, double[] V_Bounds_in)
            {
                C = C_in;
                V_Bounds = V_Bounds_in;
                PR = PR_in;
            }

            /// <summary>
            /// Adds particle points to this class for display based on the distance traveled from the source.
            /// </summary>
            /// <param name="Dist"></param>
            public void Populate(double Dist)
            {
                Mesh_Vis = false;
                Section_Vis = false;
                PC = new PointCloud();
                SPLList = new List<double>();
                this.Enabled = false;
                for (int s = 0; s < PR.Length; s++)
                {
                    for (int q = 0; q < PR[s].Count(); q++)
                    {
                        double SPL;
                        Point3d N, PT;
                        if (!PR[s].RayPt(q, Dist, 4, out SPL, out N, out PT)) continue;
                        SPLList.Add(SPL);
                        PC.Add(new Point3d(PT.X, PT.Y, PT.Z), P_Color(SPL));
                    }
                }
                this.Enabled = true;
            }

            /// <summary>
            /// Adds particle points to this class for display based on the distance traveled from the source.
            /// </summary>
            /// <param name="Dist"></param>
            public void Populate(Mesh M)
            {
                Mesh_Vis = true;
                Section_Vis = true;
                DisplayMesh = new List<Mesh>();
                DisplayMesh.Add(M);
                this.Enabled = true;
            }

            /// <summary>
            /// Adds particle points to this class for display based on the distance traveled from the source.
            /// </summary>
            /// <param name="Dist"></param>
            public void Populate(double Dist, Mesh M)
            {
                Mesh_Vis = true;
                Section_Vis = false;
                SPLList = new List<double>();
                this.Enabled = false;
                DisplayMesh = new List<Mesh>();//[PR.Length];
                for (int s = 0; s < PR.Length; s++)
                {
                    Mesh DM = M.DuplicateMesh();
                    DM.VertexColors.Clear();
                    System.Drawing.Color[] C = new System.Drawing.Color[DM.Vertices.Count];
                    for (int q = 0; q < PR[s].Count(); q++)
                    {
                        double SPL;
                        Point3d N, PT;
                        if (!PR[s].RayPt(q, Dist, 4, out SPL, out N, out PT)) continue;
                        SPLList.Add(SPL);
                        DM.Vertices[q] = new Point3f((float)PT.X, (float)PT.Y, (float)PT.Z);
                        C[q] = P_Color(SPL);
                    }
                    DM.VertexColors.AppendColors(C);
                    DM.Normals.ComputeNormals();
                    DM.FaceNormals.ComputeFaceNormals();
                    DisplayMesh.Add(DM);
                }
                this.Enabled = true;
            }

            public void Populate(List<Point3d> P)
            {
                Mesh_Vis = false;
                Section_Vis = false;
                PC = new PointCloud(P);
                this.Enabled = true;
            }

            public void Populate(List<List<Point3d>> P, List<List<System.Numerics.Complex>> pressure)
            {
                Mesh_Vis = false;
                Section_Vis = false;
                if (pressure.Count > 0)
                {
                    PC = new PointCloud();
                    for (int j = 0; j < P.Count; j++)
                    {
                        for (int i = 0; i < P[j].Count; i++)
                        {
                            PC.Add(P[j][i], P_Color(20 * System.Math.Log(pressure[j][i].Magnitude / 2E-5)));//0.0000000004
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < P.Count; j++)
                    {
                        PC = new PointCloud(P[j]);
                    }
                }
                this.Enabled = true;
            }

            public void Populate(int[] X, int[] Y, int[] Z, double dx, List<List<System.Numerics.Complex>> pressure, Mesh[] M, bool Magnitude)
            {
                DisplayMesh = new List<Mesh>();//[M.Length];    
                Mesh_Vis = true;
                Section_Vis = true;

                int ct = -1;

                for (int x = 0; x < X.Length; x++)
                {
                    ct++;
                    DisplayMesh.Add(M[0].DuplicateMesh());
                    DisplayMesh[ct].VertexColors.Clear();
                    DisplayMesh[ct].Translate(X[x] * dx, 0,0);
                }

                for (int y = 0; y < Y.Length; y++)
                {
                    ct++;
                    DisplayMesh.Add(M[1].DuplicateMesh());
                    DisplayMesh[ct].VertexColors.Clear();
                    DisplayMesh[ct].Translate(0, Y[y] * dx,0);
                }

                for (int z = 0; z < Z.Length; z++)
                {
                    ct++;
                    DisplayMesh.Add(M[2].DuplicateMesh());
                    DisplayMesh[ct].VertexColors.Clear();
                    DisplayMesh[ct].Translate(0, 0, Z[z] * dx);
                }

                if (ct != pressure.Count-1) throw new System.Exception("Input of unmatched pairs - Display of Mesh Plans.");

                //List<int> nullfaces;

                for (int j = 0; j < pressure.Count; j++)
                {
                    //nullfaces = new List<int>();
                    for (int i = 0; i < DisplayMesh[j].Faces.Count; i++)
                    {
                        double V = 10 * System.Math.Log(System.Math.Pow((Magnitude ? pressure[j][i].Magnitude : pressure[j][i].Real), 2) / 0.0000000004);

                        //if (V < V_Bounds[0]) { nullfaces.Add(i); continue; }

                        if (pressure[j].Count > 0)
                        {
                            System.Drawing.Color C = P_Color(V);
                            int i4 = i * 4;
                            DisplayMesh[j].VertexColors.SetColor(i4,C);
                            DisplayMesh[j].VertexColors.SetColor(i4+1,C);
                            DisplayMesh[j].VertexColors.SetColor(i4+2,C);
                            DisplayMesh[j].VertexColors.SetColor(i4+3,C);
                        }
                    }
                    //DisplayMesh[j].Faces.DeleteFaces(nullfaces);
                }

                this.Enabled = true;
            }

            public void Populate(List<List<Point3d>> P, List<List<double>> pressure)
            {
                Mesh_Vis = false;
                Section_Vis = false;
                if (pressure.Count > 0)
                {
                    PC = new PointCloud();
                    for (int j = 0; j < P.Count; j++)
                    {
                        for (int i = 0; i < P[j].Count; i++)
                        {
                            PC.Add(P[j][i], P_Color(Utilities.AcousticalMath.SPL_Intensity(pressure[j][i] * pressure[j][i])));
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < P.Count; j++)
                    {
                        PC = new PointCloud(P[j]);
                    }
                }
            }

            protected override void  PostDrawObjects(DrawEventArgs e)        
            {
                try
                {
                    lock (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView)
                    {
                        if (Mesh_Vis)
                        {
                            for (int i = 0; i < DisplayMesh.Count; i++)
                            {
                                if (e.Viewport.DisplayMode.SupportsShading)
                                {
                                    e.Display.DrawMeshFalseColors(DisplayMesh[i]);
                                    if (e.Viewport.DisplayMode.WireframePipelineRequired)
                                    {
                                        e.Display.DrawMeshWires(DisplayMesh[i], System.Drawing.Color.Black);
                                    }
                                }
                                else
                                {
                                    if (Section_Vis)
                                    {
                                        e.Display.DrawMeshFalseColors(DisplayMesh[i]);
                                    }
                                    else
                                    {
                                        e.Display.DrawMeshWires(DisplayMesh[i], System.Drawing.Color.Green);
                                    }
                                }
                            }
                        }
                        else
                        {
                            e.Display.DrawPointCloud(PC, 2);
                        }
                    }
                }
                catch { return; }
            }

            /// <summary>
            /// assigns particle color based on remaining energy
            /// </summary>
            /// <param name="Value">strength of particle</param>
            /// <returns></returns>
            private System.Drawing.Color P_Color(double Value)
            {
                try
                {
                    return C.GetValue(Value, V_Bounds[0], V_Bounds[1]);
                }
                catch 
                {
                    return System.Drawing.Color.Black;
                }
            }

            System.Drawing.Color[] C_Bounds;
            double[] V_Bounds;
            Pach_Graphics.colorscale C;

            /// <summary>
            /// Allows user to change the colors of the particles.
            /// </summary>
            /// <param name="Colors"></param>
            /// <param name="Values"></param>
            public void SetColorScale(Pach_Graphics.colorscale C_in, double[] Values)
            {
                V_Bounds = Values;
                C = C_in;
            }

            public void SetColorScale(System.Drawing.Color[] Colors, double[] Values)
            {
                C_Bounds = Colors;
                V_Bounds = Values;
            }
        }  
    }
}