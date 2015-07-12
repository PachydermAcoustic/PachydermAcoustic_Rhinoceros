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
using System.Text;
using Pachyderm_Acoustic.Environment;
using Hare.Geometry;
using System.Numerics;

namespace Pachyderm_Acoustic
{
    namespace Numeric
    {
        namespace TimeDomain
        {
            public partial class Acoustic_Compact_FDTD : Simulation_Type
            {
                Polygon_Scene Rm;
                public double tmax;       //seconds
                private double rho0 = 1.21;    //density of air kg/m^3
                double dt;
                public double dx;
                public readonly int xDim, yDim, zDim;
                Acoustic_Compact_FDTD.P_Node[][][] PFrame;            //pressure scalar field initialisation
                Vector[] Dir = new Vector[13];
                Point[] Orig = new Point[13];
                double fmax;
                Hare.Geometry.AABB Bounds;
                public Signal_Driver_Compact SD;
                public Microphone_Compact Mic;
                public Rhino.Geometry.Mesh m_templateX, m_templateY, m_templateZ;
                public int n;
                double time_ms;

                public Acoustic_Compact_FDTD(Polygon_Scene Rm_in, ref Signal_Driver_Compact S_in, ref Microphone_Compact M_in, double fmax_in, double tmax_in)
                {
                    Rm = Rm_in;
                    SD = S_in;
                    Mic = M_in;
                    fmax = fmax_in;
                    tmax = tmax_in;

                    Rm.partition(20);

                    dx = Rm.Sound_speed(0) / fmax * .1;          

                    Bounds = new AABB(Rm.Min() - new Point(.05 * dx, .05 * dx, .05 * dx), Rm.Max() + new Point(.05 * dx, .05 * dx, .05 * dx));
                    double x_length = Bounds.X_Length();
                    double y_length = Bounds.Y_Length();
                    double z_length = Bounds.Z_Length();

                    //estimated distance between nodes
                    xDim = (int)Math.Ceiling(x_length / dx);                                //set number of nodes in x direction
                    dx = x_length / xDim;                                                       //refined distance between nodes
                    yDim = (int)Math.Ceiling(y_length / dx);                                //set number of nodes in y direction
                    double dy = y_length / yDim;
                    zDim = (int)Math.Ceiling(z_length / dx);                              //set number of nodes in y direction
                    double dz = z_length / zDim;

                    dt = dx / (Math.Sqrt(1.0/3.0) * (Rm.Sound_speed(0)));                           //set time step small enough to satisfy courrant condition
                    dt = dx / (Rm.Sound_speed(0));                           //set time step small enough to satisfy courrant condition
                    //dt = dx / (Math.Sqrt(.75) * (Rm.Sound_speed));                           //set time step small enough to satisfy courrant condition
                    double rt2 = Math.Sqrt(2);
                    double rt3 = Math.Sqrt(3);
                    dxrt2 = dx * rt2;
                    dxrt3 = dx * rt3;

                    Dir = new Vector[13]{
                        new Vector(-1,double.Epsilon,double.Epsilon),
                        new Vector(double.Epsilon,-1,double.Epsilon),
                        new Vector(double.Epsilon,double.Epsilon,-1),
                        
                        new Vector(-1/rt2,-1/rt2,double.Epsilon),
                        new Vector(1/rt2, -1/rt2,double.Epsilon),
                        
                        new Vector(-1/rt2, double.Epsilon, 1/rt2),
                        new Vector(double.Epsilon, -1/rt2, 1/rt2),
                        new Vector(1/rt2, double.Epsilon, 1/rt2),
                        new Vector(double.Epsilon, 1/rt2, 1/rt2),

                        new Vector(-1/rt3,-1/rt3,1/rt3),
                        new Vector(1/rt3,-1/rt3,1/rt3),
                        new Vector(1/rt3,1/rt3,1/rt3),
                        new Vector(-1/rt3,1/rt3,1/rt3)
                    };

                    foreach (Vector V in Dir) V.Normalize();

                    PFrame = new P_Node[xDim][][];// yDim, zDim];                               //pressure scalar field initialisation

                    //System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                    for (int x = 0; x < xDim; x++)
                    {
                        PFrame[x] = new P_Node[yDim][];
                        Random Rnd = new Random(x);
                        for (int y = 0; y < yDim; y++)
                        {
                            PFrame[x][y] = new P_Node[zDim];
                            for (int z = 0; z < zDim; z++)
                            {
                                List<double[]> abs;
                                List<Bound_Node.Boundary> BDir;
                                Point Loc = new Point(Bounds.Min_PT.x + (((double)x + 0.5) * dx), Bounds.Min_PT.y + (((double)y + 0.5) * dy), Bounds.Min_PT.z + (((double)z + 0.5) * dz));
                                if (!Intersect_26Pt(Loc, out BDir, out abs, ref Rnd))
                                {
                                    PFrame[x][y][z] = new P_Node(Loc);//, rho0, dt, dx, Rm.Sound_speed, new int[] { x, y, z });
                                }
                                else
                                {
                                    //Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(Utilities.PachTools.HPttoRPt(Loc));
                                    PFrame[x][y][z] = 
                                        new Bound_Node(Loc, rho0, dt, dx, Rm.Sound_speed(0), new int[] { x, y, z }, abs, BDir);
                                    //PFrame[x, y, z] = new Bound_Node(Loc, rho0, dt, dx, Rm.Sound_speed, new int[] { x, y, z }, abs, BDir);
                                }
                            }
                        }
                    }//);
                    //return;

                    bool failed = false;
                    //Make Mesh Templates:
                    Build_Mesh_Sections();
                    for (int x = 0; x < xDim; x++)
                    //System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                    {
                        for (int y = 0; y < yDim; y++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                try
                                {
                                    PFrame[x][y][z].Link_Nodes(ref PFrame, x, y, z);
                                }
                                catch
                                {
                                    //Display faulty voxels in a display conduit here...
                                    UI.CellConduit.Instance.Add(PFrame[x][y][z], x, y, z, dx, Utilities.PachTools.HPttoRPt(Bounds.Min_PT));
                                    failed = true;
                                }
                            }
                        }
                    }//);

                    if (failed) return;

                    SD.Connect_Grid(PFrame, Bounds, dx, tmax, dt);
                    Mic.Connect_Grid(PFrame, Bounds, dx, tmax, dt);
                }

                public double Increment()
                {
                    time_ms = n * dt * 1000;
                    List<Rhino.Geometry.Point3d> Pts = new List<Rhino.Geometry.Point3d>();
                    List<double> Pressure = new List<double>();

                    System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                    {
                        for (int x = 0; x < xDim; x++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                PFrame[x][y][z].UpdateIWB();
                            }
                        }
                    });

                    System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                    {
                        for (int x = 0; x < xDim; x++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                PFrame[x][y][z].UpdateT();
                            }
                        }
                    });

                    SD.Drive(n);
                    Mic.Record(n);
                    n += 1;

                    return time_ms;
                }

                private void Build_Mesh_Sections()
                {
                    int ct = -1;
                    m_templateX = new Rhino.Geometry.Mesh();

                    for (int y = 0; y < yDim; y++)
                    {
                        for (int z = 0; z < zDim; z++)
                        {
                            ct++;
                            m_templateX.Vertices.Add(Utilities.PachTools.HPttoRPt(PFrame[0][y][z].Pt));
                            m_templateX.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[0][y][z].Pt) - new Rhino.Geometry.Point3d(0, 0, -dx)));
                            m_templateX.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[0][y][z].Pt) - new Rhino.Geometry.Point3d(0, -dx, -dx)));
                            m_templateX.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[0][y][z].Pt) - new Rhino.Geometry.Point3d(0, -dx, 0)));
                            //m_templateX.VertexColors.Add(System.Drawing.Color.Black);
                            int ct4 = ct * 4;
                            m_templateX.Faces.AddFace(ct4, ct4 + 1, ct4 + 2, ct4 + 3);
                        }
                    }

                    ct = -1;
                    m_templateY = new Rhino.Geometry.Mesh();

                    for (int x = 0; x < xDim; x++)
                    {
                        for (int z = 0; z < zDim; z++)
                        {
                            ct++;
                            m_templateY.Vertices.Add(Utilities.PachTools.HPttoRPt(PFrame[x][0][z].Pt));
                            m_templateY.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[x][0][z].Pt) - new Rhino.Geometry.Point3d(0, 0, -dx)));
                            m_templateY.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[x][0][z].Pt) - new Rhino.Geometry.Point3d(-dx, 0, -dx)));
                            m_templateY.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[x][0][z].Pt) - new Rhino.Geometry.Point3d(-dx, 0, 0)));
                            //m_templateX.VertexColors.Add(System.Drawing.Color.Black);
                            int ct4 = ct * 4;
                            m_templateY.Faces.AddFace(ct4, ct4 + 1, ct4 + 2, ct4 + 3);
                        }
                    }

                    ct = -1;
                    m_templateZ = new Rhino.Geometry.Mesh();

                    for (int x = 0; x < xDim; x++)
                    {
                        for (int y = 0; y < yDim; y++)
                        {
                            ct++;
                            m_templateZ.Vertices.Add(Utilities.PachTools.HPttoRPt(PFrame[x][y][0].Pt));
                            m_templateZ.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[x][y][0].Pt) - new Rhino.Geometry.Point3d(0, -dx, 0)));
                            m_templateZ.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[x][y][0].Pt) - new Rhino.Geometry.Point3d(-dx, -dx, 0)));
                            m_templateZ.Vertices.Add((Rhino.Geometry.Point3d)(Utilities.PachTools.HPttoRPt(PFrame[x][y][0].Pt) - new Rhino.Geometry.Point3d(-dx, 0, 0)));
                            //m_templateX.VertexColors.Add(System.Drawing.Color.Black);
                            int ct4 = ct * 4;
                            m_templateZ.Faces.AddFace(ct4, ct4 + 1, ct4 + 2, ct4 + 3);
                        }
                    }

                    m_templateX.Normals.ComputeNormals();
                    m_templateX.FaceNormals.ComputeFaceNormals();
                    m_templateY.Normals.ComputeNormals();
                    m_templateY.FaceNormals.ComputeFaceNormals();
                    m_templateZ.Normals.ComputeNormals();
                    m_templateZ.FaceNormals.ComputeFaceNormals();

                    //Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(m_templateX);
                    //Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(m_templateY);
                    //Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(m_templateZ);
                }

                //private void Build_Mesh_Sections()
                //{
                //    int ct = 0;
                //    m_templateX = new Rhino.Geometry.Mesh();
                //    for (int z = 0; z < zDim; z++)
                //    {
                //        m_templateX.Vertices.Add(PFrame[0, 0, z].Pt.x, PFrame[0, 0, z].Pt.y, PFrame[0, 0, z].Pt.z);
                //        ct++;
                //    }

                //    for (int y = 1; y < yDim; y++)
                //    {
                //        m_templateX.Vertices.Add(PFrame[0, y, 0].Pt.x, PFrame[0, y, 0].Pt.y, PFrame[0, y, 0].Pt.z);
                //        ct++;
                //        for (int z = 0; z < zDim; z++)
                //        {
                //            ct++;
                //            m_templateX.Vertices.Add(PFrame[0, y, z].Pt.x, PFrame[0, y, z].Pt.y, PFrame[0, y, z].Pt.z);
                //            m_templateX.Faces.AddFace(ct, ct - yDim, ct - yDim - 1, ct - 1);
                //        }
                //    }

                //    ct = 0;
                //    m_templateY = new Rhino.Geometry.Mesh();
                //    for (int x = 1; x < xDim; x++)
                //    {
                //        m_templateY.Vertices.Add(PFrame[x, 0, 0].Pt.x, PFrame[x, 0, 0].Pt.y, PFrame[x, 0, 0].Pt.z);
                //        ct++;
                //        for (int z = 0; z < zDim; z++)
                //        {
                //            ct++;
                //            m_templateY.Vertices.Add(PFrame[x, 0, z].Pt.x, PFrame[x, 0, z].Pt.y, PFrame[x, 0, z].Pt.z);
                //            m_templateY.Faces.AddFace(ct, ct - yDim, ct - yDim - 1, ct - 1);
                //        }
                //    }

                //    ct = 0;
                //    m_templateZ = new Rhino.Geometry.Mesh();
                //    for (int x = 1; x < xDim; x++)
                //    {
                //        m_templateZ.Vertices.Add(PFrame[x, 0, 0].Pt.x, PFrame[x, 0, 0].Pt.y, PFrame[x, 0, 0].Pt.z);
                //        ct++;
                //        for (int y = 0; y < yDim; y++)
                //        {
                //            ct++;
                //            m_templateZ.Vertices.Add(PFrame[x, y, 0].Pt.x, PFrame[x, y, 0].Pt.y, PFrame[x, y, 0].Pt.z);
                //            m_templateZ.Faces.AddFace(ct, ct - yDim, ct - yDim - 1, ct - 1);
                //        }
                //    }
                //}

                /////////////////
                //Display Methods : Get Points and Pressure for display output
                /////////////////
                public void Pressure_Points(ref List<List<Rhino.Geometry.Point3d>> Pts, ref List<List<System.Numerics.Complex>> Pressure, int[] X, int[] Y, int[] Z, double Low_P, bool Volume, bool Vectored, bool Colored, bool Magnitude)
                {
                    Pts = new List<List<Rhino.Geometry.Point3d>>();
                    Pressure = new List<List<System.Numerics.Complex>>();
                    if (Volume)
                    {
                        for (int x = 0; x < xDim; x++)
                        {
                            List<System.Numerics.Complex> P = new List<System.Numerics.Complex>();
                            List<Rhino.Geometry.Point3d> PtList = new List<Rhino.Geometry.Point3d>();
                            //System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                            for (int y = 0; y < yDim; y++)
                            {
                                for (int z = 0; z < zDim; z++)
                                {
                                    if (PFrame[x][y][z].P.Magnitude < Low_P) continue;
                                    if (Colored) P.Add(PFrame[x][y][z].P);
                                    if (Vectored)
                                    {
                                        PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z) + PFrame[x][y][z].VelocityDirection() * 3 * dx * (Magnitude ? PFrame[x][y][z].P.Magnitude: PFrame[x][y][z].P.Real) ));
                                    }
                                    else
                                    {
                                        PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z)));
                                    }
                                }
                            }//);
                            Pressure.Add(P);
                            Pts.Add(PtList);
                        }
                        return;
                    }
                    if (X != null || X.Length > 0)
                    {
                        foreach (int x in X)
                        {
                            List<System.Numerics.Complex> P = new List<System.Numerics.Complex>();
                            List<Rhino.Geometry.Point3d> PtList = new List<Rhino.Geometry.Point3d>();
                            //System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                            for (int y = 0; y < yDim; y++)
                            {
                                for (int z = 0; z < zDim; z++)
                                {
                                    lock (Pts)
                                    {
                                        if (Colored) P.Add(PFrame[x][y][z].P);
                                        if (Vectored)
                                        {
                                            PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z) + PFrame[x][y][z].VelocityDirection() * PFrame[x][y][z].P.Magnitude * 3 * dx));
                                        }
                                        else
                                        {
                                            PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z)));
                                        }
                                    }
                                }
                            }//);
                            Pressure.Add(P);
                            Pts.Add(PtList);
                        }
                    }
                    if (Y != null || Y.Length > 0)
                    {
                        foreach (int y in Y)
                        {
                            List<System.Numerics.Complex> P = new List<System.Numerics.Complex>();
                            List<Rhino.Geometry.Point3d> PtList = new List<Rhino.Geometry.Point3d>();
                            //System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                            for (int x = 0; x < xDim; x++)
                            {
                                for (int z = 0; z < zDim; z++)
                                {
                                    lock (Pts)
                                    {
                                        if (Colored) P.Add(PFrame[x][y][z].P);
                                        if (Vectored)
                                        {
                                            PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z) + PFrame[x][y][z].VelocityDirection() * PFrame[x][y][z].P.Magnitude * 3 * dx));
                                        }
                                        else
                                        {
                                            PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z)));
                                        }
                                    }
                                }
                            }//);
                            Pressure.Add(P);
                            Pts.Add(PtList);
                        }
                    }
                    if (Z != null || Z.Length > 0)
                    {
                        foreach (int z in Z)
                        {
                            List<System.Numerics.Complex> P = new List<System.Numerics.Complex>();
                            List<Rhino.Geometry.Point3d> PtList = new List<Rhino.Geometry.Point3d>();
                            //System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                            for (int x = 0; x < xDim; x++)
                            {
                                for (int y = 0; y < yDim; y++)
                                {
                                    lock (Pts)
                                    {
                                        if (Colored) P.Add(PFrame[x][y][z].P);
                                        if (Vectored)
                                        {
                                            PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z) + PFrame[x][y][z].VelocityDirection() * PFrame[x][y][z].P.Magnitude * 3 * dx));
                                        }
                                        else
                                        {
                                            PtList.Add((new Rhino.Geometry.Point3d(PFrame[x][y][z].Pt.x, PFrame[x][y][z].Pt.y, PFrame[x][y][z].Pt.z)));
                                        }
                                    }
                                }
                            }//);
                            Pressure.Add(P);
                            Pts.Add(PtList);
                        }
                    }
                }

                double dxrt2;
                double dxrt3;

                public bool Intersect_26Pt(Point Center, out List<Bound_Node.Boundary> Bout, out List<double[]> alpha, ref Random Rnd)
                {
                    Bout = new List<Bound_Node.Boundary>();
                    alpha = new List<double[]>();

                    Center += new Point((Rnd.NextDouble() - .5)* 1E-6, (Rnd.NextDouble() - .5) * 1E-6, (Rnd.NextDouble() - .5) * 1E-6);

                    X_Event XPt = new X_Event();
                    //new Vector(-1,0,0),
                    if (Rm.shoot(new Ray(Center, Dir[0], 0, Rnd.Next()), 0, out XPt) && XPt.t < dx)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[0] * dx)));
                            Bout.Add(Bound_Node.Boundary.AXNeg);
                        //TODO: Intelligently Assign Absorption
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[0] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dx)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[0] * dx)));
                            Bout.Add(Bound_Node.Boundary.AXPos);
                            //TODO: Intelligently Assign Absorption
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(0,-1,0),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[1], 0, Rnd.Next()), 0, out XPt) && XPt.t < dx)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[1] * dx)));
                            Bout.Add(Bound_Node.Boundary.AYNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[1] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dx)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[1] * dx)));
                            Bout.Add(Bound_Node.Boundary.AYPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(0,0,-1),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[2], 0, Rnd.Next()), 0, out XPt) && XPt.t < dx)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[2] * dx)));
                            Bout.Add(Bound_Node.Boundary.AZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[2] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dx)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[2] * dx)));
                            Bout.Add(Bound_Node.Boundary.AZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(-1/rt2,-1/rt2,0),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[3], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[3] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXNegYNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[3] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[3] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXPosYPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(1/rt2, -1/rt2,0),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[4], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[4] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXPosYNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[4] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[4] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXNegYPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(-1/rt2, 0, 1/rt2),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[5], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[5] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXNegZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[5] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[5] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXPosZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(0, -1/rt2, 1/rt2),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[6], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[6] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDYNegZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[6] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[6] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDYPosZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(1/rt2, 0, 1/rt2),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[7], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[7] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXPosZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[7] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[7] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDXNegZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(0, 1/rt2, 1/rt2),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[8], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[8] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDYPosZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[8] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt2)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[8] * dx)));
                            Bout.Add(Bound_Node.Boundary.SDYNegZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(-1/rt3,-1/rt3,1/rt3),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[9], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[9] * dx)));
                            Bout.Add(Bound_Node.Boundary.DXNegYNegZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[9] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            Bout.Add(Bound_Node.Boundary.DXPosYPosZNeg);
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[9] * dx)));
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(1/rt3,-1/rt3,1/rt3),
                    XPt = new X_Event();
                    if (Rm.shoot(new Ray(Center, Dir[10], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[10] * dx)));
                            Bout.Add(Bound_Node.Boundary.DXPosYNegZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[10] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[10] * dx)));
                            Bout.Add(Bound_Node.Boundary.DXNegYPosZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    //new Vector(1/rt3,1/rt3,1/rt3),
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center + new Point(0,1E-6,-1E-6), Dir[11], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[11] * dx)));
                            Bout.Add(Bound_Node.Boundary.DXPosYPosZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[11] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[11] * dx)));
                            Bout.Add(Bound_Node.Boundary.DXNegYNegZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }    
                    //new Vector(-1/rt3,1/rt3,1/rt3)
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[12], 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center + Dir[12] * dx)));
                            Bout.Add(Bound_Node.Boundary.DXNegYPosZPos);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }
                    XPt = new X_Event(); 
                    if (Rm.shoot(new Ray(Center, Dir[12] * -1, 0, Rnd.Next()), 0, out XPt) && XPt.t < dxrt3)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(new Rhino.Geometry.Line(Utilities.PachTools.HPttoRPt(Center), Utilities.PachTools.HPttoRPt(Center - Dir[12] * dx)));
                            Bout.Add(Bound_Node.Boundary.DXPosYNegZNeg);
                            alpha.Add(Rm.AbsorptionValue[XPt.Poly_id].Coefficient_A_Broad());
                        }

                    return Bout.Count > 0;
                }

                public override string ProgressMsg()
                {
                    throw new NotImplementedException();
                }

                public override System.Threading.ThreadState ThreadState()
                {
                    throw new NotImplementedException();
                }

                public override void Abort_Calculation()
                {
                    throw new NotImplementedException();
                }

                public override void Combine_ThreadLocal_Results()
                {
                    throw new NotImplementedException();
                }

                public override void Begin()
                {
                    throw new NotImplementedException();
                }

                public override string Sim_Type()
                {
                    return "Finite Difference Time Domain";
                }

                public System.Numerics.Complex P(int x, int y, int z)
                {
                    return this.PFrame[x][y][z].P;
                }

                public class P_Node
                {
                    //public static double Courrant = double.NegativeInfinity;
                    //public static double Courrant2 = double.NegativeInfinity;
                    //public static double t_courrant = double.NegativeInfinity;
                    public Point Pt;
                    public System.Numerics.Complex Pnf, Pn, Pn_1;
                    protected System.Numerics.Complex X, Y, Z;
                    protected P_Node Xpos_Link;
                    protected P_Node Ypos_Link;
                    protected P_Node Zpos_Link;
                    protected P_Node Xneg_Link;
                    protected P_Node Yneg_Link;
                    protected P_Node Zneg_Link;

                    protected P_Node[] Links2;
                    protected P_Node[] Links3;

                    public P_Node(Point loc)//, double rho0, double dt, double dx, double c, int[] id_in)
                    {
                        //id = id_in;
                        Pt = loc;
                        //if (Courrant == double.NegativeInfinity)
                        //{
                        //    Courrant = dt * c / dx;
                        //    Courrant2 = Courrant * Courrant;
                        //    t_courrant = 2 * (1 - 3 * Courrant2);
                        //}
                    }

                    public virtual void Link_Nodes(ref P_Node[][][] Frame, int x, int y, int z)
                    {
                        int xdim = Frame.Length-1;
                        int ydim = Frame[0].Length-1;
                        int zdim = Frame[0][0].Length-1;

                        if (x < xdim)
                        {
                            Xpos_Link = Frame[x + 1][y][z];
                        }
                        else Xpos_Link = new Null_Node();
                        //FrameX[x + 1, y, z].id.AddRange(id);

                        if (y < ydim)
                        {
                            Ypos_Link = Frame[x][y + 1][z];
                        }
                        else Ypos_Link = new Null_Node();
                        //FrameY[x, y + 1, z].id.AddRange(id);
                        if (z < zdim)
                        {
                            Zpos_Link = Frame[x][y][z + 1];
                        }
                        else Zpos_Link = new Null_Node();
                        //FrameZ[x, y, z + 1].id.AddRange(id);
                        if (x > 0)
                        {
                            Xneg_Link = Frame[x - 1][y][z];
                        }
                        else Xneg_Link = new Null_Node();
                        //FrameX[x, y, z].id.AddRange(id);
                        if (y > 0)
                        {
                            Yneg_Link = Frame[x][y - 1][z];
                        }
                        else Yneg_Link = new Null_Node();
                        //FrameY[x, y, z].id.AddRange(id);
                        if (z > 0)
                        {
                            Zneg_Link = Frame[x][y][z - 1];
                        }
                        else Zneg_Link = new Null_Node();
                        //FrameZ[x, y, z].id.AddRange(id);

                        Links2 = new P_Node[12];
                        Links3 = new P_Node[8];

                        if (x < xdim && y < ydim) Links2[0] = Frame[x + 1][y + 1][z]; else Links2[0] = new Null_Node();
                        if (x > 0 && y > 0) Links2[1] = Frame[x - 1][y - 1][z]; else Links2[1] = new Null_Node();
                        if (x < xdim && y > 0) Links2[2] = Frame[x + 1][y - 1][z]; else Links2[2] = new Null_Node();
                        if (x > 0 && y < ydim) Links2[3] = Frame[x - 1][y + 1][z]; else Links2[3] = new Null_Node();
                        if (x < xdim && z < zdim) Links2[4] = Frame[x + 1][y][z + 1]; else Links2[4] = new Null_Node();
                        if (x > 0 && z > 0) Links2[5] = Frame[x - 1][y][z - 1]; else Links2[5] = new Null_Node();
                        if (x > 0 && z < zdim) Links2[6] = Frame[x - 1][y][z + 1]; else Links2[6] = new Null_Node();
                        if (x < xdim && z > 0) Links2[7] = Frame[x + 1][y][z - 1]; else Links2[7] = new Null_Node();
                        if (y < ydim && z < zdim) Links2[8] = Frame[x][y + 1][z + 1]; else Links2[8] = new Null_Node();
                        if (y > 0 && z > 0) Links2[9] = Frame[x][y - 1][z - 1]; else Links2[9] = new Null_Node();
                        if (y > 0 && z < zdim) Links2[10] = Frame[x][y - 1][z + 1]; else Links2[10] = new Null_Node();
                        if (y < ydim && z > 0) Links2[11] = Frame[x][y + 1][z - 1]; else Links2[11] = new Null_Node();
                        
                        if (x < xdim && y < ydim && z < zdim) Links3[0] = Frame[x + 1][y + 1][z + 1]; else Links3[0] = new Null_Node();
                        if (x > 0 && y > 0 && z > 0) Links3[1] = Frame[x - 1][y - 1][z - 1]; else Links3[1] = new Null_Node();
                        if (x > 0 && y > 0 && z < zdim) Links3[2] = Frame[x - 1][y - 1][z + 1]; else Links3[2] = new Null_Node();
                        if (x < xdim && y < ydim && z > 0) Links3[3] = Frame[x + 1][y + 1][z - 1]; else Links3[3] = new Null_Node();
                        if (x > 0 && y < ydim && z < zdim) Links3[4] = Frame[x - 1][y + 1][z + 1]; else Links3[4] = new Null_Node();
                        if (x < xdim && y > 0 && z > 0) Links3[5] = Frame[x + 1][y - 1][z - 1]; else Links3[5] = new Null_Node();
                        if (x > 0 && y < ydim && z > 0) Links3[6] = Frame[x - 1][y + 1][z - 1]; else Links3[6] = new Null_Node();
                        if (x < xdim && y > 0 && z < zdim) Links3[7] = Frame[x + 1][y - 1][z + 1]; else Links3[7] = new Null_Node();
                    }

                    //public virtual void UpdateSLF()
                    //{
                    //    X = Xpos_Link.P + Xneg_Link.P;
                    //    Y = Ypos_Link.P + Yneg_Link.P;
                    //    Z = Zpos_Link.P + Zneg_Link.P;
                    //    Pnf = Courrant2 * (X + Y + Z) - Pn_1;
                    //}

                    //public virtual void UpdateOCTA()
                    //{
                    //    Complex p3 = 0;
                    //    foreach (P_Node node in Links3) p3 += node.P;
                    //    Pnf += p3 / 4 - Pn_1; ;
                    //}

                    //public virtual void UpdateCCP()
                    //{
                    //    Complex p2 = 0;
                    //    foreach (P_Node node in Links2) p2 += node.P;
                    //    Pnf += p2 / 4 - Pn - Pn_1;
                    //}

                    public virtual void UpdateIWB()
                    {
                        X = Xpos_Link.P + Xneg_Link.P;
                        Y = Ypos_Link.P + Yneg_Link.P;
                        Z = Zpos_Link.P + Zneg_Link.P;
                        ////IWB
                        //Pnf = 0.25 * (X + Y + Z) - 1.5 * Pn - Pn_1;
                        //Complex p2 = 0;
                        //foreach (P_Node node in Links2) p2 += node.P;
                        //Pnf += p2 * 0.125;
                        //Complex p3 = 0;
                        //foreach (P_Node node in Links3) p3 += node.P;
                        //Pnf += p3 * 0.0625;
                        ////IISO2
                        Pnf = (15.0/48.0) * (X + Y + Z) - (9.0/8.0) * Pn - Pn_1;
                        System.Numerics.Complex p2 = 0;
                        foreach (P_Node node in Links2) p2 += node.P;
                        Pnf += p2 * (3.0/32.0);
                        System.Numerics.Complex p3 = 0;
                        foreach (P_Node node in Links3) p3 += node.P;
                        Pnf += p3 * (1.0/64.0);
                        ////IISO
                        //Pnf = (.25) * (X + Y + Z) - Pn - Pn_1;
                        //Complex p2 = 0;
                        //foreach (P_Node node in Links2) p2 += node.P;
                        //Pnf += p2 * (.125);
                        ////CCP
                        //Pnf = -Pn - Pn_1;//-Pn - Pn_1;
                        //Complex p2 = 0;
                        //foreach (P_Node node in Links2) p2 += node.P;
                        //Pnf += p2 * (.25);
                        ////OCTA
                        //Pnf = -Pn_1;
                        //Complex p3 = 0;
                        //foreach (P_Node node in Links3) p3 += node.P;
                        //Pnf += p3 * (.25);
                        ////SLF
                        //Pnf = (1/3) * (X + Y + Z) - (9.0/8.0) * Pn - Pn_1;
                    }

                    public virtual void UpdateT()
                    {
                        Pn_1 = Pn;
                        Pn = Pnf;
                    }

                    public Rhino.Geometry.Vector3d VelocityDirection()
                    {
                        Rhino.Geometry.Vector3d V = new Rhino.Geometry.Vector3d(X.Magnitude, Y.Magnitude, Z.Magnitude);
                        V.Unitize();
                        return V;
                    }

                    public System.Numerics.Complex P
                    {
                        get
                        {
                            return Pn;
                        }
                    }
                }

                public class Null_Node : P_Node
                {
                    public Null_Node()//Point loc, double rho0, double dt, double dx, double c, int[] id_in)
                        : base(new Point())//, rho0, dt, dx, c, id_in)
                    {
                        Instance = this;
                    }

                    public static Null_Node Instance
                    {
                        get;
                        private set;
                    }

                    public override void UpdateIWB()
                    {
                        Pnf = 0;
                        Pn = 0;
                        Pn_1 = 0;
                    }

                    public override void UpdateT()
                    {
                    }
                }
            }

            public class Signal_Driver_Compact
            {
                Acoustic_Compact_FDTD.P_Node[] SrcNode;
                Complex[] signal;
                double f;
                Rhino.Geometry.Point3d[] Loc;
                Signal_Type S;
                double w;

                public enum Signal_Type
                {
                    Dirac_Pulse,
                    Sine_Tone,
                    Gaussian_Pulse,
                    Sine_Pulse,
                    SteadyState_Noise,
                    SS_Noise_Pulse
                }

                public Signal_Driver_Compact(Signal_Type S_in, double freq, double w_in, Rhino.Geometry.Point3d[] Loc_in)
                {
                    S = S_in;
                    f = freq;
                    Loc = Loc_in;
                    w = w_in;
                }

                public void Connect_Grid(Acoustic_Compact_FDTD.P_Node[][][] Frame, AABB Bounds, double dx, double tmax, double dt)
                {
                    signal = new Complex[(int)Math.Ceiling(tmax / dt)];
                    double f2pi = f * 2 * Math.PI;

                    Random R = new Random();

                    switch (S)
                    {
                        case Signal_Type.Dirac_Pulse:
                            signal[1] = 1;
                            break;
                        case Signal_Type.Sine_Tone:
                            for (int n = 0; n < tmax / dt; n++) 
                            {
                                double ph = f2pi * n * dt;
                                signal[n] = new Complex(Math.Sin(ph), Math.Cos(ph));
                            }
                            break;
                        case Signal_Type.Gaussian_Pulse:
                            Complex sum = 0;
                            for (int n = 0; n < tmax / dt; n++) 
                            {
                                signal[n] = Math.Exp(-.5 * Math.Pow((double)n / w, 2));
                                sum += signal[n];
                            }
                            for(int n = 0; n < tmax/dt; n++)
                            {
                                signal[n] /= sum;
                            }
                            break;
                        case Signal_Type.Sine_Pulse:
                            for (int n = 0; n < tmax / dt; n++) signal[n] = Math.Exp(-.5 * Math.Pow((double)n / w, 2) * Math.Cos(f2pi * n * dt));
                            break;
                        case Signal_Type.SteadyState_Noise:
                            for (int n = 0; n < tmax / dt; n++) signal[n] = R.NextDouble();
                            break;
                        case Signal_Type.SS_Noise_Pulse:
                            for (int n = 0; n < tmax / dt; n++) signal[n] = Math.Exp(-.5 * Math.Pow((double)n / w, 2) * R.NextDouble());
                            break;
                    }

                    SrcNode = new Acoustic_Compact_FDTD.P_Node[Loc.Length];

                    for (int i = 0; i < Loc.Length; i++)
                    {
                        int X = (int)Math.Floor((Loc[i].X - Bounds.Min_PT.x) / dx);
                        int Y = (int)Math.Floor((Loc[i].Y - Bounds.Min_PT.y) / dx);
                        int Z = (int)Math.Floor((Loc[i].Z - Bounds.Min_PT.z) / dx);
                        SrcNode[i] = Frame[X][Y][Z];
                    }
                }

                public void Drive(int t)
                {
                    foreach (Acoustic_Compact_FDTD.P_Node n in SrcNode)
                    {
                        Complex prop = signal[t];
                        n.Pn = prop;
                    }
                }
            }

            public class Microphone_Compact
            {
                System.Numerics.Complex[][] recording;
                Acoustic_Compact_FDTD.P_Node[] RecNode;
                Rhino.Geometry.Point3d[] Loc;

                public Microphone_Compact(Rhino.Geometry.Point3d[] Loc_in)
                {
                    Loc = Loc_in;
                    Random R = new Random();
                    recording = new System.Numerics.Complex[Loc.Length][];
                    RecNode = new Acoustic_Compact_FDTD.P_Node[Loc.Length];
                }

                public void Connect_Grid(Acoustic_Compact_FDTD.P_Node[][][] Frame, AABB Bounds, double dx, double tmax, double dt)
                {
                    for (int i = 0; i < Loc.Length; i++)
                    {
                        recording[i] = new System.Numerics.Complex[(int)Math.Ceiling(tmax / dt)];
                        int X = (int)Math.Floor((Loc[i].X - Bounds.Min_PT.x) / dx);
                        int Y = (int)Math.Floor((Loc[i].Y - Bounds.Min_PT.y) / dx);
                        int Z = (int)Math.Floor((Loc[i].Z - Bounds.Min_PT.z) / dx);
                        RecNode[i] = Frame[X][Y][Z];
                    }
                }

                public void Record(int n)
                {
                    for (int i = 0; i < recording.Length; i++)
                    {
                        recording[i][n] = RecNode[i].P;
                    }
                }
            }
        }
    }
}