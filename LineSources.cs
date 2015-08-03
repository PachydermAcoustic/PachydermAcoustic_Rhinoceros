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

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        class LineSource: Source
        {
            /// <summary>
            /// Normalized (0 to 1) proportion of domains of curves. 
            /// A selection of random number 0 to 1 should give you a 
            /// random point on all curves, hopefully evenly distributed.
            /// </summary>
            public double[] Domains;
            /// <summary>
            /// Original curves saved to the source object.
            /// </summary>
            public List<Curve> Curves;
            /// <summary>
            /// Sample points on curves.
            /// </summary>
            public Point3d[][] Samples;
            /// <summary>
            /// Sound Power of each sample on the curve. (10^Lp/10 * L / no_of_samples)
            /// </summary>m
            public double[][] DomainPower;
            /// <summary>
            /// User designated sound power of each line source.
            /// </summary>
            double[][] DomainLevel;
            /// <summary>
            /// Sum length of all segments
            /// </summary>
            double Total_L;
            /// <summary>
            /// number of samples per meter.
            /// </summary>
            int samplespermeter = 16;
            /// <summary>
            /// Private member controlling the directional characteristics of the line source.
            /// </summary>
            Directionality D;

            public LineSource(IEnumerable<Curve> SrcLines, List<String> CodeList, int el_m, int SrcID, Phase_Regime ph)
                :base(new double[8]{60, 49, 41, 35, 31, 28, 26, 24}, new Point3d(0,0,0), ph, SrcID)
            {
                string type = SrcLines.ElementAt<Curve>(0).GetUserString("SourceType");
                double velocity = double.Parse(SrcLines.ElementAt<Curve>(0).GetUserString("Velocity"));
                double delta = double.Parse(SrcLines.ElementAt<Curve>(0).GetUserString("delta"));

                if (type == "Aircraft (ANCON derived)")
                {
                    D = new ANCON(delta, velocity);
                }
                else D = new Simple();

                samplespermeter = el_m;
                Curves = SrcLines.ToList<Curve>();
                Samples = new Point3d[Curves.Count][];
                //for(int i = 0; i < Curves.Count; i++)
                System.Threading.Tasks.Parallel.For(0, Curves.Count, i =>
                {
                    //Divide each curve up in ~equal length segments.
                    Samples[i] = Curves[i].DivideEquidistant(1.0 / (double)samplespermeter);
                });

                Domains = new double[Curves.Count+1];
                DomainLevel = new double[Curves.Count][];
                DomainPower = new double[Curves.Count][];
                Total_L = 0;
                
                for (int i = 0; i < Curves.Count; i++)
                {
                    double L = Curves[i].GetLength();
                    Domains[i + 1] = Total_L += L;
                    DomainLevel[i] = Utilities.PachTools.DecodeSourcePower(CodeList[i]);
                    DomainPower[i] = new double[8];
                    double PowerMod = L / (double)Samples[i].Length;
                    for (int oct = 0; oct < 8; oct++) DomainPower[i][oct] = 1E-12 * Math.Pow(10, .1 * DomainLevel[i][oct]) * PowerMod;
                }

                for (int i = 0; i < Domains.Length; i++)
                {
                    Domains[i] /= Total_L;
                }
            }

            public override string Type()
            {
                return "Line Source";
            }

            public override void AppendPts(ref List<Point3d> SPT)
            {                
                for (int i = 0; i < Curves.Count; i++)
                {
                    for (int j = 0; j < Samples[i].Length; j++)
                    {
                        SPT.Add(Samples[i][j]);
                    }
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
                return D.Directions(index, thread, ref random, ref Curves, ref Domains, ref DomainPower, ref delay, ref ph, ref S_ID );
            }

            private abstract class Directionality
            {
                public abstract BroadRay Directions(int index, int thread, ref Random random, ref List<Curve> Curves, ref double[] Domains, ref double[][] DomainPower, ref double delay, ref Phase_Regime ph, ref int S_ID);
            }

            private class Simple: Directionality
            {
                public override BroadRay Directions(int index, int thread, ref Random random, ref List<Curve> Curves, ref double[] Domains, ref double[][] DomainPower, ref double Delay, ref Phase_Regime ph, ref int S_ID)
                {
                    double pos = random.NextDouble();
                    int i;
                    for (i = 0; i < Curves.Count; i++) if (pos > Domains[i] && pos < Domains[i + 1]) break;

                    Interval t = Curves[i].Domain;
                    double x = random.NextDouble() * (t[1] - t[0]) + t[0];
                    Point3d P = Curves[i].PointAt(x);

                    double Theta = random.NextDouble() * 2 * System.Math.PI;
                    double Phi = random.NextDouble() * 2 * System.Math.PI;
                    Hare.Geometry.Vector Direction = new Hare.Geometry.Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Sin(Theta) * Math.Sin(Phi), Math.Cos(Theta));
                    double[] phase = new double[8];
                    if (ph == Phase_Regime.Random) for (int o = 0; o < 8; o++) phase[o] = random.Next() * 2 * Math.PI;
                    else for (int o = 0; o < 8; o++) phase[o] = 0 - Delay * Utilities.Numerics.angularFrequency[o];

                    return new BroadRay(Utilities.PachTools.RPttoHPt(P), Direction, random.Next(), thread, DomainPower[i], phase, Delay, S_ID);
                }
            }

            private class ANCON : Directionality
            {
                double delta; //slant angle in radians
                double reciprocal_velocity; //m/s
                double dLinf = 10 * Math.Pow(10, 0.8 / 10);
                    
                public ANCON(double delta_in, double velocity)
                {
                    delta = delta_in * Math.PI / 180; //degrees to radians
                    reciprocal_velocity = 1 / velocity;
                }
                     

                public override BroadRay Directions(int index, int thread, ref Random random, ref List<Curve> Curves, ref double[] Domains, ref double[][] DomainPower, ref double Delay, ref Phase_Regime ph, ref int S_ID)
                {
                    double pos = random.NextDouble();
                    int i;
                    for (i = 0; i < Curves.Count; i++) if (pos > Domains[i] && pos < Domains[i + 1]) break;

                    Interval t = Curves[i].Domain;
                    double x = random.NextDouble() * (t[1] - t[0]) + t[0];
                    Point3d P = Curves[i].PointAt(x);
                    Rhino.Geometry.Vector3d f = Curves[i].TangentAt(x);
                    Hare.Geometry.Vector fore = new Hare.Geometry.Vector(f.X, f.Y, f.Z);

                    double Theta = random.NextDouble() * 2 * System.Math.PI;
                    double Phi = random.NextDouble() * 2 * System.Math.PI;
                    Hare.Geometry.Vector Direction = new Hare.Geometry.Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Sin(Theta) * Math.Sin(Phi), Math.Cos(Theta));

                    double cosphi = Hare.Geometry.Hare_math.Dot(Direction, fore);
                    double sinphi = Math.Sqrt(1 - cosphi * cosphi);
                    double tanphi =  sinphi/cosphi;
                    double F_r = sinphi * sinphi * Math.Pow((tanphi * tanphi + 1) / (tanphi * tanphi + (1 + tanphi * Math.Tan(delta))), 1.5);

                    double[] power = new double[8];
                    for (int oct = 0; oct < 8; oct++) power[oct] = DomainPower[i][oct] * reciprocal_velocity * dLinf * F_r;
                    double[] phase = new double[8];
                    if (ph == Phase_Regime.Random) for (int o = 0; o < 8; o++) phase[o] = random.Next() * 2 * Math.PI;
                    else for (int o = 0; o < 8; o++) phase[o] = 0 - Delay * Utilities.Numerics.angularFrequency[o];

                    return new BroadRay(Utilities.PachTools.RPttoHPt(P), Direction, random.Next(), thread, DomainPower[i], phase, Delay, S_ID);
                }
            }
        }
    }
}