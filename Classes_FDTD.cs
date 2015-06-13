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

namespace Pachyderm_Acoustic
{
    namespace Numeric
    {
        namespace TimeDomain
        {
            public class Acoustic_FDTD: Simulation_Type
            {
                Polygon_Scene Rm;
                public double tmax;       //seconds
                private double rho0 = 1.21;    //density of air kg/m^3  
                double dt;
                double dx;
                public readonly int xDim, yDim, zDim;
                P_Node[, ,] PFrame;            //pressure scalar field initialisation
                U_Node[, ,] UFrameX;
                U_Node[, ,] UFrameY;
                U_Node[, ,] UFrameZ;

                double fmax = 300;                    
                Hare.Geometry.AABB Bounds;
                public Signal_Driver SD;
                public Microphone Mic;

                public int n;
                double time_ms;

                public Acoustic_FDTD(Polygon_Scene Rm_in, ref Signal_Driver S_in, ref Microphone M_in, double fmax_in, double tmax_in)
                {
                    Rm = Rm_in;
                    SD = S_in;
                    Mic = M_in;
                    fmax = fmax_in;
                    tmax = tmax_in;

                    Rm.partition(20);
                    Bounds = new AABB(Rm.Min(), Rm.Max());

                    double x_length = Bounds.X_Length();
                    double y_length = Bounds.Y_Length();
                    double z_length = Bounds.Z_Length();

                    dx = Rm.Sound_speed(0) / fmax * .1;                                     //estimated distance between nodes
                    xDim = (int)Math.Ceiling(x_length / dx);                                //set number of nodes in x direction
                    dx = x_length / xDim;                                                       //refined distance between nodes
                    yDim = (int)Math.Ceiling(y_length / dx);                                //set number of nodes in y direction
                    zDim = (int)Math.Ceiling(z_length / dx);                              //set number of nodes in y direction

                    dt = dx / (Math.Sqrt(3) * Rm.Sound_speed(0));                           //set time step small enough to satisfy courrant condition
                    
                    PFrame = new P_Node[xDim, yDim, zDim];                               //pressure scalar field initialisation
                    UFrameX = new U_Node[xDim + 1, yDim, zDim];
                    UFrameY = new U_Node[xDim, yDim + 1, zDim];
                    UFrameZ = new U_Node[xDim, yDim, zDim + 1];

                    //System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                    for (int x = 0; x < xDim; x++)
                    {
                        for (int y = 0; y < yDim; y++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                PFrame[x, y, z] = new P_Node(new Point(Bounds.Min_PT.x + (((double)x + 0.5) * dx), Bounds.Min_PT.y + (((double)y + 0.5) * dx), Bounds.Min_PT.z + (((double)z + 0.5) * dx)), rho0, dt, dx, Rm.Sound_speed(0), new int[] { x, y });
                            }
                        }
                    }//);

                    for (int x = 0; x < xDim + 1; x++)
                    {
                        for (int y = 0; y < yDim + 1; y++)
                        {
                            for (int z = 0; z < zDim + 1; z++)
                            {
                                if (x == 0 || x == xDim)
                                {
                                    if (y < yDim && z < zDim)
                                    {
                                        double abs;
                                        Vector V;
                                        AABB box = new AABB(new Point((x - .5) * dx, y * dx, z * dx) + Bounds.Min_PT, new Point((x + .5) * dx, (y + 1) * dx, (z + 1) * dx) + Bounds.Min_PT);
                                        if (!Rm.Box_Intersect(box, out abs, out V)) abs = 1;
                                        if (V.x != 0)
                                        {
                                            UFrameX[x, y, z] = new Bound_Node(rho0, dt, dx, Rm.Sound_speed(0), Math.Abs(abs));
                                        }
                                        else
                                        {
                                            UFrameX[x, y, z] = new PML_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                        }
                                    }
                                }
                                else
                                {
                                    if (y != yDim && z != zDim)
                                    {
                                        double abs;
                                        Vector V;
                                        if (!Rm.Box_Intersect(new AABB(new Point((x - .5) * dx, y * dx, z * dx) + Bounds.Min_PT, new Point((x + .5) * dx, (y + 1) * dx, (z + 1) * dx) + Bounds.Min_PT), out abs, out V))
                                        {
                                            UFrameX[x, y, z] = new Air_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                        }
                                        else
                                        {
                                            if (V.x != 0)
                                            {
                                                UFrameX[x, y, z] = new Bound_Node(rho0, dt, dx, Rm.Sound_speed(0), Math.Abs(abs));// * V.x));
                                            }
                                            else
                                            {
                                                UFrameX[x, y, z] = new Air_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                            }
                                        }
                                    }
                                }

                                if (y == 0 || y == yDim)
                                {
                                    if (x < xDim && z < zDim)
                                    {
                                        double abs;
                                        Vector V;
                                        if (!Rm.Box_Intersect(new AABB(new Point(x * dx, (y - .5) * dx, z * dx) + Bounds.Min_PT, new Point((x + 1) * dx, (y + .5) * dx, (z + 1) * dx) + Bounds.Min_PT), out abs, out V)) abs = 1;
                                        if (V.y != 0)
                                        {
                                            UFrameY[x, y, z] = new Bound_Node(rho0, dt, dx, Rm.Sound_speed(0), Math.Abs(abs));// * V.y));
                                        }
                                        else
                                        {
                                            UFrameY[x, y, z] = new PML_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                        }
                                    }
                                }
                                else
                                {
                                    if (x != xDim && z != zDim)
                                    {
                                        double abs;
                                        Vector V;
                                        if (!Rm.Box_Intersect(new AABB(new Point(x * dx, (y - .5) * dx, z * dx) + Bounds.Min_PT, new Point((x + 1) * dx, (y + .5) * dx, (z + 1) * dx) + Bounds.Min_PT), out abs, out V))
                                        {
                                            UFrameY[x, y, z] = new Air_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                        }
                                        else
                                        {
                                            if (V.y != 0)
                                            {
                                                UFrameY[x, y, z] = new Bound_Node(rho0, dt, dx, Rm.Sound_speed(0), Math.Abs(abs));// * V.y));
                                            }
                                            else
                                            {
                                                UFrameY[x, y, z] = new Air_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                            }
                                        }
                                    }
                                }

                                if (z == 0 || z == zDim)
                                {
                                    if (x < xDim && y < yDim)
                                    {
                                        double abs;
                                        Vector V;
                                        if (!Rm.Box_Intersect(new AABB(new Point(x * dx, y * dx, (z - .5) * dx) + Bounds.Min_PT, new Point((x + 1) * dx, (y + 1) * dx, (z + .5) * dx) + Bounds.Min_PT), out abs, out V)) abs = 1;
                                        if (V.z != 0)
                                        {
                                            UFrameZ[x, y, z] = new Bound_Node(rho0, dt, dx, Rm.Sound_speed(0), Math.Abs(abs));// * V.z));
                                        }
                                        else
                                        {
                                            UFrameZ[x, y, z] = new PML_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                        }
                                    }
                                }
                                else
                                {
                                    if (x != xDim && y != yDim)
                                    {
                                        double abs;
                                        Vector V;
                                        if (!Rm.Box_Intersect(new AABB(new Point(x * dx, y * dx, (z - .5) * dx) + Bounds.Min_PT, new Point((x + 1) * dx, (y + 1) * dx, (z + .5) * dx) + Bounds.Min_PT), out abs, out V))
                                        {
                                            UFrameZ[x, y, z] = new Air_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                        }
                                        else
                                        {
                                            if (V.z != 0)
                                            {
                                                UFrameZ[x, y, z] = new Bound_Node(rho0, dt, dx, Rm.Sound_speed(0), Math.Abs(abs));// * V.z));
                                            }
                                            else
                                            {
                                                UFrameZ[x, y, z] = new Air_Node(rho0, dt, dx, Rm.Sound_speed(0));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                    {
                        for (int y = 0; y < yDim; y++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                PFrame[x, y, z].Link_Nodes(ref UFrameX, ref UFrameY, ref UFrameZ, x, y, z);
                            }
                        }
                    });

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
                            for (int z = 0; z < zDim + 1; z++)
                            {
                                UFrameZ[x, y, z].Update();
                            }
                        }
                    });

                    System.Threading.Tasks.Parallel.For(0, yDim + 1, (y) =>
                    {
                        for (int x = 0; x < xDim; x++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                UFrameY[x, y, z].Update();
                            }
                        }
                    });

                    System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                    {
                        for (int x = 0; x < xDim + 1; x++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                UFrameX[x, y, z].Update();
                            }
                        }
                    });

                    System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                    {
                        for (int x = 0; x < xDim; x++)
                        {
                            for (int z = 0; z < zDim; z++)
                            {
                                PFrame[x, y, z].Update_Pressure();
                            }
                        }
                    });
                    SD.Drive(n);
                    Mic.Record(n);
                    n += 1;

                    return time_ms;
                }
                
                /////////////////
                //Display Methods : Get Points and Pressure for display output
                /////////////////
                public void Pressure_Points(ref List<Rhino.Geometry.Point3d> Pts, ref List<double> Pressure, int[] X, int[] Y, int[] Z, double Low_P, bool Volume, bool Vectored, bool Colored)
                {
                    Pts = new List<Rhino.Geometry.Point3d>();
                    Pressure = new List<double>();
                    if (Volume)
                    {
                        for (int x = 0; x < xDim; x++)
                        {
                            //System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                            for (int y = 0; y < yDim; y++)
                            {
                                for (int z = 0; z < zDim; z++)
                                {
                                    if (PFrame[x, y, z].P < Low_P) continue;
                                    if (Colored) Pressure.Add(PFrame[x, y, z].P);
                                    if (Vectored) 
                                    {
                                        Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z) + PFrame[x, y, z].VelocityDirection() * PFrame[x, y, z].P * 3 * dx));
                                    }
                                    else
                                    {
                                        Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z)));
                                    }
                                }
                            }//);
                        }
                        return;
                    }


                    Pts = new List<Rhino.Geometry.Point3d>();
                    Pressure = new List<double>();
                    if (X != null || X.Length > 0)
                    {
                        foreach (int x in X)
                        {
                            //System.Threading.Tasks.Parallel.For(0, yDim, (y) =>
                            for (int y = 0; y < yDim; y++)
                            {
                                for (int z = 0; z < zDim; z++)
                                {
                                    lock (Pts)
                                    {
                                        if (Colored) Pressure.Add(PFrame[x, y, z].P);
                                        if (Vectored)
                                        {
                                            Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z) + PFrame[x, y, z].VelocityDirection() * PFrame[x, y, z].P * 3 * dx));
                                        }
                                        else
                                        {
                                            Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z)));
                                        }
                                    }
                                }
                            }//);
                        }
                    }
                    if (Y != null || Y.Length > 0)
                    {
                        foreach (int y in Y)
                        {
                            //System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                            for (int x = 0; x < xDim; x++)
                            {
                                for (int z = 0; z < zDim; z++)
                                {
                                    lock (Pts)
                                    {
                                        if (Colored) Pressure.Add(PFrame[x, y, z].P);
                                        if (Vectored)
                                        {
                                            Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z) + PFrame[x, y, z].VelocityDirection() * PFrame[x, y, z].P * 3 * dx));
                                        }
                                        else
                                        {
                                            Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z)));
                                        }
                                    }
                                }
                            }//);
                        }
                    }
                    if (Z != null || Z.Length > 0)
                    {
                        foreach (int z in Z)
                        {
                            //System.Threading.Tasks.Parallel.For(0, xDim, (x) =>
                            for (int x = 0; x < xDim; x++)
                            {
                                for (int y = 0; y < yDim; y++)
                                {
                                    lock (Pts)
                                    {
                                        if (Colored) Pressure.Add(PFrame[x, y, z].P);
                                        if (Vectored)
                                        {
                                            Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z) + PFrame[x, y, z].VelocityDirection() * PFrame[x, y, z].P * 3 * dx));
                                        }
                                        else
                                        {
                                            Pts.Add((new Rhino.Geometry.Point3d(PFrame[x, y, z].Pt.x, PFrame[x, y, z].Pt.y, PFrame[x, y, z].Pt.z)));
                                        }
                                    }
                                }
                            }//);
                        }
                    }
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

                public abstract class U_Node
                {
                    public double U;
                    public P_Node N1;
                    public P_Node N2;

                    public U_Node() { }
                    public abstract void Update();
                    public virtual double U1
                    {
                        get { return U; }
                    }
                    public virtual double U2
                    {
                        get { return U; }
                    }
                }

                public class Air_Node : U_Node
                {
                    static double coef = double.NegativeInfinity;

                    public Air_Node(double rho0, double dt, double dx, double C)
                    {
                        if (coef == double.NegativeInfinity)
                        {
                            coef = dt / (dx * rho0);
                        }
                    }

                    public override void Update()
                    {
                        //lock (Rhino.RhinoDoc.ActiveDoc)
                        //{
                        //    Rhino.RhinoDoc.ActiveDoc.Objects.Add(new Rhino.Geometry.LineCurve(Utilities.PachTools.HPttoRPt(N1.Pt), Utilities.PachTools.HPttoRPt(N2.Pt)));
                        //}
                        U -= coef * (N2.P - N1.P);
                        if (double.IsNaN(U) || double.IsInfinity(U))
                        {
                            Rhino.RhinoApp.WriteLine("Unstable Calculation");
                        }
                    }
                }

                public class Bound_Node : U_Node
                {
                    //Botteldooren Boundary Node.
                    double U1_Sum, U2_Sum;
                    double KT;
                    double alpha;
                    double betacoef;
                    double Uminus;

                    public Bound_Node(double rho0, double dt, double dx, double C, double a_Coef)
                    {
                        ///////////////////
                        //a_Coef = 0.9999;
                        ///////////////////
                        double Zfdtd = rho0 * dx / dt;
                        double M = 0.00000000001; //Z1 = surface mass - kg/m2
                        double Res = Math.Sqrt(1 - a_Coef);//0.020;//Math.Sqrt(a_Coef); //Zo = Resistance (real portion of impedance
                        double R = rho0 * C *(1 + Res) / (1 - Res);
                        KT = 1000000 * dt; //Z-1 = spring constant
                        alpha = (1 - R / Zfdtd + 2 * M / (Zfdtd * dt)) / (1 + R / Zfdtd / dt + 2 * M / Zfdtd / dt);
                        betacoef = (2 * dt) / (rho0 * dx) * (1 / (1 + R / Zfdtd + 2 * M / (Zfdtd * dt)));

                        if (double.IsNaN(alpha) || double.IsInfinity(alpha))
                        {
                            Rhino.RhinoApp.WriteLine("Unstable Calculation");
                        }

                        if (double.IsNaN(betacoef) || double.IsInfinity(betacoef))
                        {
                            Rhino.RhinoApp.WriteLine("Unstable Calculation");
                        }
                    }

                    public override void Update()
                    {
                        U1_Sum += Uminus;
                        Uminus = 0;
                        U2_Sum += U;
                        U = 0;

                        if (N1 != null)
                        {
                            Uminus = alpha * Uminus + betacoef * (N1.P - KT * U1_Sum);
                        }
                        if (N2 != null)
                        {
                            U = alpha * U + betacoef * (N2.P - KT * U2_Sum);
                        }
                        if (double.IsNaN(U) || double.IsInfinity(U))
                        {
                            Rhino.RhinoApp.WriteLine("Unstable Calculation");
                        }
                    }

                    public override double U1
                    {
                        get
                        {
                            return Uminus;
                        }
                    }
                }

                public class PML_Node : U_Node
                {
                    double[] P_pml = new double[10];
                    double[] U_pml = new double[10];
                    //static double[] a = new double[30] { .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1, .1};//{ 1e-1, 1e-3, 1e-5, 1e-7, 1e-9, 1e-11, 1e-13, 1e-15, 1e-17, 1e-19 };
                    static double UtoP = double.NegativeInfinity;
                    static double coef = double.NegativeInfinity;

                    public PML_Node(double rho0, double dt, double dx, double c)
                    {

                        if (UtoP == double.NegativeInfinity)
                        {
                            coef = dt / (dx * rho0);
                            //for (int i = 0; i < a.Length; i++)
                            //{
                            //    a[i] *= coef;
                            //}

                            UtoP = dt * rho0 * c * c / dx;
                        }
                    }

                    public override void Update()
                    {
                        if (N1 != null)
                        {
                            U -= 1e-26 * coef * (P_pml[0] - N1.P);
                        }
                        else if (N2 != null)
                        {
                            U -= 1e-26 * coef * (N2.P - P_pml[0]);
                        }

                        //for (int i = 0; i < U_pml.Length-1; i++)
                        //{
                        //    U_pml[i] -= (P_pml[i + 1] - P_pml[i]);
                        //}
                        //U_pml[9] -= P_pml[9];
 
                        //for (int i = 0; i < P_pml.Length-1; i++)
                        //{
                        //    P_pml[i] = UtoP * (U_pml[i+1] -  U_pml[i]);
                        //}
                        //P_pml[9] = UtoP * (U_pml[9]);
                    }

                }

                public class P_Node
                {
                    public static double UtoP = double.NegativeInfinity;
                    public Point Pt;
                    public double Pa;
                    //public int[] id;

                    U_Node Xpos_Link;
                    U_Node Ypos_Link;
                    U_Node Zpos_Link;
                    U_Node Xneg_Link;
                    U_Node Yneg_Link;
                    U_Node Zneg_Link;

                    public P_Node(Point loc, double rho0, double dt, double dx, double c, int[] id_in)
                    {
                        //id = id_in;
                        Pt = loc;
                        if (UtoP == double.NegativeInfinity)
                        {
                            UtoP = dt * rho0 * c * c / dx;
                        }
                    }

                    public void Link_Nodes(ref U_Node[, ,] FrameX, ref U_Node[, ,] FrameY, ref U_Node[, ,] FrameZ, int x, int y, int z)
                    {
                        Xpos_Link = FrameX[x + 1, y, z];
                        FrameX[x + 1, y, z].N1 = this;
                        //FrameX[x + 1, y, z].id.AddRange(id);

                        Ypos_Link = FrameY[x, y + 1, z];
                        FrameY[x, y + 1, z].N1 = this;
                        //FrameY[x, y + 1, z].id.AddRange(id);

                        Zpos_Link = FrameZ[x, y, z + 1];
                        FrameZ[x, y, z + 1].N1 = this;
                        //FrameZ[x, y, z + 1].id.AddRange(id);

                        Xneg_Link = FrameX[x, y, z];
                        FrameX[x, y, z].N2 = this;
                        //FrameX[x, y, z].id.AddRange(id);

                        Yneg_Link = FrameY[x, y, z];
                        FrameY[x, y, z].N2 = this;
                        //FrameY[x, y, z].id.AddRange(id);

                        Zneg_Link = FrameZ[x, y, z];
                        FrameZ[x, y, z].N2 = this;
                        //FrameZ[x, y, z].id.AddRange(id);
                    }

                    public void Update_Pressure()
                    {
                        Pa -= UtoP * ((Xpos_Link.U1 - Xneg_Link.U2) + (Ypos_Link.U1 - Yneg_Link.U2) + (Zpos_Link.U1 - Zneg_Link.U2));
                    }

                    public Rhino.Geometry.Vector3d VelocityDirection()
                    {
                        Rhino.Geometry.Vector3d V = new Rhino.Geometry.Vector3d(Xpos_Link.U - Xneg_Link.U, Ypos_Link.U - Yneg_Link.U, Zpos_Link.U - Zneg_Link.U);
                        V.Unitize();
                        return V;
                    }

                    public double P
                    {
                        get
                        {
                            return Pa;
                        }
                    }
                }
            }

            public class Signal_Driver
            {
                Acoustic_FDTD.P_Node[] SrcNode;
                double[] signal;
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

                public Signal_Driver(Signal_Type S_in, double freq, double w_in, Rhino.Geometry.Point3d[] Loc_in)
                {
                    S = S_in;
                    f = freq;
                    Loc = Loc_in;
                    w = w_in;
                }

                public void Connect_Grid(Acoustic_FDTD.P_Node[,,] Frame, AABB Bounds, double dx, double tmax, double dt)
                {
                    signal = new double[(int)Math.Ceiling(tmax / dt)];
                    double f2pi = f * 2 * Math.PI;

                    Random R = new Random();

                    switch (S)
                    {
                        case Signal_Type.Dirac_Pulse:
                            signal[1] = 1;
                            break;
                        case Signal_Type.Sine_Tone:
                            for (int n = 0; n < tmax / dt; n++) signal[n] = Math.Sin(f2pi * n * dt);
                            break;
                        case Signal_Type.Gaussian_Pulse:
                            for (int n = 0; n < tmax / dt; n++) signal[n] = Math.Exp(-.5 * Math.Pow((double)n / w, 2));
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

                    SrcNode = new Acoustic_FDTD.P_Node[Loc.Length];

                    for (int i = 0; i < Loc.Length; i++)
                    {
                        int X = (int)Math.Floor((Loc[i].X - Bounds.Min_PT.x) / dx);
                        int Y = (int)Math.Floor((Loc[i].Y - Bounds.Min_PT.y) / dx);
                        int Z = (int)Math.Floor((Loc[i].Z - Bounds.Min_PT.z) / dx);
                        SrcNode[i] = Frame[X, Y, Z];
                    }
                }

                public void Drive(int t)
                {
                    //int i = -1;
                    foreach (Acoustic_FDTD.P_Node n in SrcNode)
                    {
                        double prop = signal[t];
                        n.Pa = prop;
                    }
                }
            }

            public class Microphone
            {
                double[][] recording;
                Acoustic_FDTD.P_Node[] RecNode;
                Rhino.Geometry.Point3d[] Loc;
                
                public Microphone(Rhino.Geometry.Point3d[] Loc_in)
                {
                    Loc = Loc_in;
                    Random R = new Random();
                    recording = new double[Loc.Length][];
                    RecNode = new Acoustic_FDTD.P_Node[Loc.Length];
                }

                public void Connect_Grid(Acoustic_FDTD.P_Node[, ,] Frame, AABB Bounds, double dx, double tmax, double dt)
                {
                    for (int i = 0; i < Loc.Length; i++)
                    {
                        recording[i] = new double[(int)Math.Ceiling(tmax / dt)];
                        int X = (int)Math.Floor((Loc[i].X - Bounds.Min_PT.x) / dx);
                        int Y = (int)Math.Floor((Loc[i].Y - Bounds.Min_PT.y) / dx);
                        int Z = (int)Math.Floor((Loc[i].Z - Bounds.Min_PT.z) / dx);
                        RecNode[i] = Frame[X, Y, Z];
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