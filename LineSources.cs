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
using System.Linq;
using Hare.Geometry;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        class LineSource: Source
        {
            /// <summary>
            /// Original curves saved to the source object.
            /// </summary>
            public Curve Curve;
            /// <summary>
            /// Sample points on curves.
            /// </summary>
            public Point3d[] Samples;
            /// <summary>
            /// Sound Power of each sample on the curve. (10^Lp/10 * L / no_of_samples)
            /// </summary>m
            public double[] Power;
            /// <summary>
            /// User designated sound power of each line source.
            /// </summary>
            double[] Level;
            /// <summary>
            /// number of samples per meter.
            /// </summary>
            int samplespermeter = 16;
            /// <summary>
            /// Private member controlling the directional characteristics of the line source.
            /// </summary>
            public Directionality D;

            public LineSource(Curve SrcPath, String Code, int el_m, int SrcID, Phase_Regime ph)
                :base(new double[8]{60, 49, 41, 35, 31, 28, 26, 24}, new Point3d(0,0,0), ph, SrcID)
            {
                string type = SrcPath.GetUserString("SourceType");
                string v = SrcPath.GetUserString("Velocity");
                double velocity = double.Parse(v);
                double delta = double.Parse(SrcPath.GetUserString("delta"));

                if (type == "Aircraft (ANCON derived)")
                {
                    D = new ANCON(delta, velocity);
                }
                else D = new Simple();

                samplespermeter = el_m;
                Curve = SrcPath;

                //Divide curve up in ~equal length segments.
                Samples = Curve.DivideEquidistant(1.0 / (double)samplespermeter);

                Level = Utilities.PachTools.DecodeSourcePower(Code);
                Power = new double[8];
            
                double PowerMod = Curve.GetLength() / (double)Samples.Length;
                for (int oct = 0; oct < 8; oct++) Power[oct] = 1E-12 * Math.Pow(10, .1 * Level[oct]) * PowerMod;
            }

            public override string Type()
            {
                return "Line Source";
            }

            public override void AppendPts(ref List<Point3d> SPT)
            {                
                for (int j = 0; j < Samples.Length; j++)
                {
                    SPT.Add(Samples[j]);
                }
            }

            public override BroadRay Directions(int index, int thread, ref Random random, int[] Octaves)
            {
                BroadRay B = Directions(index, thread, ref random);
                B.Octaves = Octaves;
                return B;
            }

            public override BroadRay Directions(int index, int thread, ref Random random)
            {
                return D.Directions(index, thread, ref random, ref Curve, ref Power, ref delay, ref ph, ref S_ID );
            }

            public double[] DirPower(int threadid, int random, Vector Direction, Point3d pt)
            {
                double t;
                Curve.ClosestPoint(pt, out t);
                return D.DirPower(threadid, random, Direction, t, ref Curve, ref Power);
            }

            public double[] DirPower(int threadid, int random, Vector Direction, double position)
            {
                return D.DirPower(threadid, random, Direction, position, ref Curve, ref Power);
            }

            public abstract class Directionality
            {
                public abstract BroadRay Directions(int index, int thread, ref Random random, ref Curve Curves, ref double[] DomainPower, ref double delay, ref Phase_Regime ph, ref int S_ID);
                public abstract double[] DirPower(int threadid, int random, Vector Direction, double position, ref Curve Curves, ref double[] DomainPower);
            }

            public class Simple: Directionality
            {
                public override BroadRay Directions(int index, int thread, ref Random random, ref Curve Curves, ref double[] DomainPower, ref double Delay, ref Phase_Regime ph, ref int S_ID)
                {
                    double pos = random.NextDouble();
                    int i;

                    Interval t = Curves.Domain;
                    double x = random.NextDouble() * (t[1] - t[0]) + t[0];
                    Point3d P = Curves.PointAt(x);

                    double Theta = random.NextDouble() * 2 * System.Math.PI;
                    double Phi = random.NextDouble() * 2 * System.Math.PI;
                    Vector Direction = new Hare.Geometry.Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Sin(Theta) * Math.Sin(Phi), Math.Cos(Theta));
                    double[] phase = new double[8];
                    if (ph == Phase_Regime.Random) for (int o = 0; o < 8; o++) phase[o] = random.Next() * 2 * Math.PI;
                    else for (int o = 0; o < 8; o++) phase[o] = 0 - Delay * Utilities.Numerics.angularFrequency[o];

                    return new BroadRay(Utilities.PachTools.RPttoHPt(P), Direction, random.Next(), thread, DomainPower, phase, Delay, S_ID);
                }

                public override double[] DirPower(int threadid, int random, Vector Direction, double position, ref Curve Curves, ref double[] DomainPower)
                {
                    return DomainPower;
                }
            }

            public class ANCON : Directionality
            {
                double delta; //slant angle in radians
                double reciprocal_velocity; //m/s
                double dLinf = 10 * Math.Pow(10, 0.8 / 10);
                    
                public ANCON(double delta_in, double velocity)
                {
                    delta = delta_in * Math.PI / 180; //degrees to radians
                    reciprocal_velocity = 1 / velocity;
                }

                public override double[] DirPower(int threadid, int random, Vector Direction, double position, ref Curve Curves, ref double[] DomainPower)
                {
                    X_Event X = new X_Event();
                    double[] RayPower = new double[8];

                    Interval t = Curves.Domain;
                    Point3d P = Curves.PointAt((position/t.Length) + t.Min);
                    Vector3d f = Curves.TangentAt(position);
                    Vector fore = new Hare.Geometry.Vector(f.X, f.Y, f.Z);

                    double cosphi = Hare.Geometry.Hare_math.Dot(Direction, fore);
                    double sinphi = Math.Sqrt(1 - cosphi * cosphi);
                    double tanphi = sinphi / cosphi;
                    double F_r = sinphi * sinphi * Math.Pow((tanphi * tanphi + 1) / (tanphi * tanphi + (1 + tanphi * Math.Tan(delta))), 1.5);

                    for (int oct = 0; oct < 8; oct++)
                    {
                        if (DomainPower[oct] == 0)
                        {
                            RayPower[oct] = 0;
                        }
                        else
                        {
                            RayPower[oct] = DomainPower[oct] * reciprocal_velocity * dLinf * F_r;
                        }
                    }
                    return RayPower;
                }

                //public List<Point3d> Pt = new List<Point3d>(10000);

                public override BroadRay Directions(int index, int thread, ref Random random, ref Curve Curves, ref double[] DomainPower, ref double Delay, ref Phase_Regime ph, ref int S_ID)
                {
                    double pos = random.NextDouble();
                    int i;

                    Interval t = Curves.Domain;
                    double x = random.NextDouble() * (t[1] - t[0]) + t[0];
                    Point3d P = Curves.PointAt(x);
                    Vector3d f = Curves.TangentAt(x);
                    Vector fore = new Hare.Geometry.Vector(f.X, f.Y, f.Z);

                    double Theta = random.NextDouble() * 2 * System.Math.PI;
                    double Phi = random.NextDouble() * 2 * System.Math.PI;
                    Vector Direction = new Hare.Geometry.Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Sin(Theta) * Math.Sin(Phi), Math.Cos(Theta));

                    double cosphi = Hare.Geometry.Hare_math.Dot(Direction, fore);
                    double sinphi = Math.Sqrt(1 - cosphi * cosphi);
                    double tanphi =  sinphi/cosphi;
                    double F_r = sinphi * sinphi * Math.Pow((tanphi * tanphi + 1) / (tanphi * tanphi + (1 + tanphi * Math.Tan(delta))), 1.5);

                    double[] power = new double[8];
                    for (int oct = 0; oct < 8; oct++) power[oct] = DomainPower[oct] * reciprocal_velocity * dLinf * F_r;
                    double[] phase = new double[8];
                    if (ph == Phase_Regime.Random) for (int o = 0; o < 8; o++) phase[o] = random.Next() * 2 * Math.PI;
                    else for (int o = 0; o < 8; o++) phase[o] = 0 - Delay * Utilities.Numerics.angularFrequency[o];

                    return new BroadRay(Utilities.PachTools.RPttoHPt(P), Direction, random.Next(), thread, DomainPower, phase, Delay, S_ID);
                }
            }
        }
    }
}