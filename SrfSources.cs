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
using Rhino.Geometry;
using Hare.Geometry;
using System.Linq;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        class SurfaceSource: Source
        {
            /// <summary>
            /// Normalized (0 to 1) proportion of domains of surfaces. 
            /// A selection of random number 0 to 1 should give you a 
            /// random point on all curves, hopefully evenly distributed.
            /// </summary>
            double[] Domains;
            double[][] SubDomains;
            /// <summary>
            /// Original surfaces saved to the source object.
            /// </summary>
            public List<Brep> Srfs;
            /// <summary>
            /// Sample points on curves.
            /// </summary>
            public Point3d[][] Samples;
            /// <summary>
            /// Sound Power of each sample on the curve. (10^Lp/10 * L / no_of_samples)
            /// </summary>m
            double[][] DomainPower;
            /// <summary>
            /// User designated sound power of each line source.
            /// </summary>
            public double[][] DomainLevel;
            /// <summary>
            /// Sum length of all segments
            /// </summary>
            double Total_A;
            double[] Sub_A;
            /// <summary>
            /// number of samples per meter.
            /// </summary>
            int samplespermeter = 16;
            /// <summary>
            /// Topology of the meshed surface.
            /// </summary>
            Hare.Geometry.Topology[] T;

            public SurfaceSource(IEnumerable<Brep> Surfaces, List<String> CodeList, int el_m, int SrcID, Phase_Regime ph)
                :base(new double[8]{0, 0, 0, 0, 0, 0, 0, 0}, new Point3d(0,0,0), ph, SrcID)
            {
                samplespermeter = el_m;
                
                Srfs = Surfaces.ToList<Brep>();
                Samples = new Point3d[Srfs.Count][];
                
                MeshingParameters mp = new MeshingParameters();
                mp.MaximumEdgeLength = 1.0 / (double)samplespermeter;
                mp.MinimumEdgeLength = 1.0 / (double)samplespermeter;

                Sub_A = new double[Srfs.Count];
                SubDomains = new double[Srfs.Count][];
                T = new Topology[Srfs.Count];

                //for(int i = 0; i < Curves.Count; i++)
                System.Threading.Tasks.Parallel.For(0, Srfs.Count, ips =>
                {
                    int i = (int)ips;
                    
                    
                    //Divide each curve up in ~equal length segments.
                    Mesh[] m = Mesh.CreateFromBrep(Srfs[i],mp);
                    BoundingBox Box = m[0].GetBoundingBox(true);
                    for (int j = 1; j < m.Length; j++) Box.Union(m[j].GetBoundingBox(true));
                    T[i] = new Topology(Utilities.PachTools.RPttoHPt(Box.Min), Utilities.PachTools.RPttoHPt(Box.Max));
                    
                    List<Point3d> pts = new List<Point3d>();
                    SubDomains[i] = new double[m[i].Faces.Count + 1];
                    for (int j = 0; j < m[i].Faces.Count; j++)
                    {
                        double u,v;
                        ComponentIndex ci;
                        Point3d no;
                        Vector3d V;
                        Point3d A = m[i].Vertices[m[i].Faces[j].A];
                        Srfs[i].ClosestPoint(A, out no, out ci, out u, out v, 1, out V);
                        A += V;
                        Point3d B = m[i].Vertices[m[i].Faces[j].B];
                        Srfs[i].ClosestPoint(B, out no, out ci, out u, out v, 1, out V);
                        B += V;
                        Point3d C = m[i].Vertices[m[i].Faces[j].C];
                        Srfs[i].ClosestPoint(C, out no, out ci, out u, out v, 1, out V);
                        C += V;
                        Point3d D = m[i].Vertices[m[i].Faces[j].D];
                        Srfs[i].ClosestPoint(D, out no, out ci, out u, out v, 1, out V);
                        D += V;
                        
                        if (m[i].Faces[j].IsQuad)
                        {
                            Hare.Geometry.Point[] poly = new Hare.Geometry.Point[4];
                            poly[0] = new Hare.Geometry.Point(A.X, A.Y, A.Z);
                            poly[1] = new Hare.Geometry.Point(B.X, B.Y, B.Z);
                            poly[2] = new Hare.Geometry.Point(C.X, C.Y, C.Z);
                            poly[3] = new Hare.Geometry.Point(D.X, D.Y, D.Z);
                            T[i].Add_Polygon(poly);
                        }
                        else
                        {
                            Hare.Geometry.Point[] poly = new Hare.Geometry.Point[3];
                            poly[0] = new Hare.Geometry.Point(A.X, A.Y, A.Z);
                            poly[1] = new Hare.Geometry.Point(B.X, B.Y, B.Z);
                            poly[2] = new Hare.Geometry.Point(C.X, C.Y, C.Z);
                            T[i].Add_Polygon(poly);
                        }

                        pts.Add(m[i].Faces[j].IsQuad ? new Point3d(A.X + B.X + C.X + D.X, A.Y + B.Y + C.Y + D.Y, A.Z + B.Z + C.Z + D.Z) / 4 : new Point3d(A.X + B.X + C.X, A.Y + B.Y + C.Y, A.Z + B.Z + C.Z) / 3);

                        SubDomains[i][j + 1] = Sub_A[i] += T[i].Polygon_Area(j);
                    }
                    Samples[i] = pts.ToArray();
                });

                Domains = new double[Srfs.Count+1];
                DomainLevel = new double[Srfs.Count][];
                DomainPower = new double[Srfs.Count][];
                Total_A = 0;
                
                for (int i = 0; i < Srfs.Count; i++)
                {
                    for (int j = 0; j < SubDomains[i].Length; j++) SubDomains[i][j] /= Sub_A[i];
                    double A = Srfs[i].GetArea();
                    Domains[i + 1] = Total_A += A;
                    DomainLevel[i] = Utilities.PachTools.DecodeSourcePower(CodeList[i]);
                    DomainPower[i] = new double[8];
                    double PowerMod = A;
                    for (int oct = 0; oct < 8; oct++) DomainPower[i][oct] = 1E-12 * Math.Pow(10, .1 * DomainLevel[i][oct]) / PowerMod;
                }

                for (int i = 0; i < Domains.Length; i++)
                {
                    Domains[i] /= Total_A;
                }
            }

            public override string Type()
            {
                return "Special - Surface Source";
            }

            //double[] TimePt;
            //bool[] Validity;
            //double[][,] energy;
            //Scene Room;
            //List<Hare.Geometry.Point> R;
            //public override bool Direct_Special(ref double[][,] energy_in, ref float[][,] P_real, ref float[][,] P_imag, ref float[][, ,] Dir_Rec_Pos, ref float[][, ,] Dir_Rec_Neg, ref bool[] Validity_in, ref double[] TimePt_in, ref double[] G_Ref, ref List<Hare.Geometry.Point> R_in, ref Scene Room_in, ref Random rnd_in, int SampleRate)
            //{
            //    //Homogeneous media only...
            //    Special_Status.Instance.special = true;
                
            //    TimePt = TimePt_in;
            //    Validity = Validity_in;
            //    energy = energy_in;
            //    Room = Room_in;

            //    //SampleFreq = SampleRate;
            //    R = new List<Hare.Geometry.Point>();
            //    foreach (Hare.Geometry.Point p in R_in) R.Add(p);
                
            //    //double incr_3 = Room.Sound_speed / SampleFreq;
            //    //List<Hare.Geometry.Point> SrcPts = new List<Hare.Geometry.Point>();
            //    //List<Point3d> A3P = new List<Point3d>();
            //    //List<int> SrcDomain = new List<int>();

            //    int[][][] tau = new int[R.Count][][]; //Rec;Curve;Sample
            //    double [][][] dist = new double[R.Count][][]; //Rec;Curve;Sample
            //    int[] RecT = new int[R.Count];
            //    energy = new double[R.Count][,];
            //    P_real = new float[R.Count][,];
            //    P_imag = new float[R.Count][,];
            //    Validity = new Boolean[R.Count];

            //    double C_Sound = Room_in.Sound_speed(0);

            //    //int MT = MaxT;

            //    List<int> rnd = new List<int>();
            //    for (int i = 0; i < R.Count; i++)
            //    {
            //        rnd.Add(rnd_in.Next());
            //    }

            //    System.Threading.Semaphore S = new System.Threading.Semaphore(1,1);

            //    //for (int k = 0; k < R.Count; k++)
            //    System.Threading.Tasks.Parallel.For(0, R.Count, k =>
            //    {
            //        Random RndGen = new Random(rnd[k]);
            //        tau[k] = new int[Srfs.Count][];
            //        dist[k] = new double[Srfs.Count][];
            //        for (int i = 0; i < Srfs.Count; i++)
            //        {
            //            tau[k][i] = new int[Samples[i].Length];
            //            dist[k][i] = new double[Samples[i].Length];
            //            TimePt[k] = double.MaxValue;

            //            for (int j = 0; j < Samples[i].Length; j++)
            //            {
            //                Point3d p = Samples[i][j];
            //                Vector d = R[k] - Utilities.PachTools.RPttoHPt(Samples[i][j]);
            //                dist[k][i][j] = d.Length();
            //                double tdbl = dist[k][i][j] / C_Sound;
            //                tau[k][i][j] = (int)Math.Ceiling(tdbl * SampleRate);
            //                if (RecT[k] < tau[k][i][j]) RecT[k] = tau[k][i][j];

            //                if (TimePt[k] > tdbl) TimePt[k] = tdbl;

            //                d.Normalize();

            //                Ray D = new Ray(Utilities.PachTools.RPttoHPt(p), d, 0, RndGen.Next());
            //                double x1 = 0, x2 = 0;
            //                List<double> t_in;
            //                List<int> code;
            //                int x3 = 0;
            //                List<Hare.Geometry.Point> x4;
            //                if (Room.shoot(D, out x1, out x2, out x3, out x4, out t_in, out code))
            //                {
            //                    if (t_in[0] >= dist[k][i][j]) Validity[k] = true; ;
            //                }
            //                else
            //                {
            //                    Validity[k] = true;
            //                }
            //            }
            //        }

            //        S.WaitOne();
            //        //foreach (int T in RecT) if (MT < T) MT = T;
            //        Special_Status.Instance.progress += 1.0f / R.Count;
            //        S.Release();
            //    });

            //    //MaxT = MT;

            //    for (int rec_id = 0; rec_id < R.Count; rec_id++)
            //    {
            //        energy[rec_id] = new double[RecT[rec_id] + 1, 8];
            //        P_real[rec_id] = new float[RecT[rec_id] + 1, 8];
            //        P_imag[rec_id] = new float[RecT[rec_id] + 1, 8];
            //        //Parallel.For(0, SrcPts.Count, i =>
            //        for (int i = 0; i < Srfs.Count; i++)
            //        {
            //            for (int j = 0; j < tau[rec_id][i].Length; j++)
            //            {
            //                double[] Io = new double[8];

            //                //Io[0] = DomainPower[i][0] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[0] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
            //                //Io[1] = DomainPower[i][1] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[1] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
            //                //Io[2] = DomainPower[i][2] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[2] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
            //                //Io[3] = DomainPower[i][3] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[3] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
            //                //Io[4] = DomainPower[i][4] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[4] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
            //                //Io[5] = DomainPower[i][5] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[5] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
            //                //Io[6] = DomainPower[i][6] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[6] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
            //                //Io[7] = DomainPower[i][7] * Math.Exp(-0.2302 * Room_in.Attenuation(0)[7] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);

            //                Io[0] = Math.Pow(10, DomainLevel[i][0] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[0] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;
            //                Io[1] = Math.Pow(10, DomainLevel[i][1] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[1] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;
            //                Io[2] = Math.Pow(10, DomainLevel[i][2] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[2] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;
            //                Io[3] = Math.Pow(10, DomainLevel[i][3] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[3] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;
            //                Io[4] = Math.Pow(10, DomainLevel[i][4] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[4] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;
            //                Io[5] = Math.Pow(10, DomainLevel[i][5] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[5] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;
            //                Io[6] = Math.Pow(10, DomainLevel[i][6] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[6] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;
            //                Io[7] = Math.Pow(10, DomainLevel[i][7] / 10) * 1E-12 * Math.Exp(-0.2302 * Room_in.Attenuation(0)[7] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Samples[i].Length;

            //                energy[rec_id][tau[rec_id][i][j], 0] += Io[0];
            //                energy[rec_id][tau[rec_id][i][j], 1] += Io[1];
            //                energy[rec_id][tau[rec_id][i][j], 2] += Io[2];
            //                energy[rec_id][tau[rec_id][i][j], 3] += Io[3];
            //                energy[rec_id][tau[rec_id][i][j], 4] += Io[4];
            //                energy[rec_id][tau[rec_id][i][j], 5] += Io[5];
            //                energy[rec_id][tau[rec_id][i][j], 6] += Io[6];
            //                energy[rec_id][tau[rec_id][i][j], 7] += Io[7];

            //                for (int oct = 0; oct < 8; oct++)
            //                {
            //                    double real, imag;
            //                    Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[oct] * dist[rec_id][i][j]/ C_Sound + Utilities.Numerics.PiX2 * rnd_in.NextDouble()), out real, out imag);
            //                    P_real[rec_id][tau[rec_id][i][j], oct] += (float)(Math.Sqrt(Io[oct] * Room.Rho_C(0)) * real);
            //                    P_imag[rec_id][tau[rec_id][i][j], oct] += (float)(Math.Sqrt(Io[oct] * Room.Rho_C(0)) * imag);
            //                }
            //            }
            //        }//);

            //        energy_in = energy;
            //        TimePt_in = TimePt;
            //        Validity_in = Validity;
            //    }

            //    for (int oct = 0; oct < 8; oct++) G_Ref[oct] = 1;
            //    Special_Status.Instance.Reset(); 
            //    return true;
            //}

            public override void AppendPts(ref List<Point3d> SPT)
            {                
                for (int i = 0; i < Srfs.Count; i++)
                {
                    for (int j = 0; j < Samples[i].Length; j++)
                    {
                        SPT.Add(Samples[i][j]);
                    }
                }
            }

            public override BroadRay Directions(int index, int thread, ref Random random, int[] Octaves)
            {
                BroadRay Ray = Directions(index, thread, ref random);
                Ray.Octaves = Octaves;
                return Ray;
            }

            public override BroadRay Directions(int index, int thread, ref Random random)
            {
                double pos = random.NextDouble();
                double subpos = random.NextDouble();
                int i,j;
                for (i = 0; i < Srfs.Count; i++) if (pos > Domains[i] && pos < Domains[i + 1]) break;
                for (j = 0; j < SubDomains[i].Length; j++) if (subpos > SubDomains[i][j] && subpos < SubDomains[i][j+1]) break;

                Point3d P = Utilities.PachTools.HPttoRPt(T[i].Polys[j].GetRandomPoint(random.NextDouble(), random.NextDouble(), 0));
                double Theta = random.NextDouble() * 2 * System.Math.PI;
                double Phi = random.NextDouble() * 2 * System.Math.PI;
                Hare.Geometry.Vector Direction = new Hare.Geometry.Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Sin(Theta) * Math.Sin(Phi), Math.Cos(Theta));
                double[] phase = new double[8];
                if (ph == Phase_Regime.Random) for(int o = 0; o < 8; o++) phase[o] = random.Next() * 2 * Math.PI;
                else for(int o = 0; o < 8; o++) phase[o] = 0 - Delay * Utilities.Numerics.angularFrequency[o];

                return new BroadRay(Utilities.PachTools.RPttoHPt(P), Direction, random.Next(), thread, DomainPower[i], phase, delay, S_ID);
            }
        }
    }
}