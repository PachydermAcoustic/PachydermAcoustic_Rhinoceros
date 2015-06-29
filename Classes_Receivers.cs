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
using System;
using System.Collections.Generic;
using Hare.Geometry;
using System.Linq;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        /// <summary>
        /// A bank for storing all receiver objects.
        /// </summary>
        [Serializable]
        public class Receiver_Bank
        {
            public Spherical_Receiver[] Rec_List;
            public Type Rec_Type;
            public int SampleCT;
            public int SampleRate;
            public int RayCount;
            public double CutOffTime;
            public Hare.Geometry.Point Min;
            public Hare.Geometry.Point Max;
            public int[] Oct_choice;
            public double delay_ms;

            /// <summary>
            /// Specifies the kinds of receivers that can be placed in this receiver bank.
            /// </summary>
            public enum Type
            {
                Stationary,
                Variable
            }

            public Receiver_Bank()
            {
            }

            /// <summary>
            /// Private receiver bank constructer for file read-in.
            /// </summary>
            /// <param name="Rec_Count">The number of receivers specified by the user</param>
            /// <param name="SampleRate_in">the simulation histogram sampling frequency</param>
            /// <param name="CSound">the speed of sound in m/s</param>
            /// <param name="CO_Time_in">The Cut Off Time of the simulation in ms.</param>
            /// <param name="Type">The type of receiver to be used</param>
            private Receiver_Bank(int Rec_Count, int SampleRate_in, double CO_Time_in, double delayinms, double[] rho_c, int[] Octaves, Receiver_Bank.Type Type)
            {
                delay_ms = delayinms;
                SampleRate = SampleRate_in;
                SampleCT = (int)Math.Ceiling(CO_Time_in * SampleRate_in / 1000);
                this.CutOffTime = CO_Time_in;
                Rec_List = new Spherical_Receiver[Rec_Count];
                Rec_Type = Type;
                for (int i = 0; i < Rec_Count; i++)
                {
                    Rec_List[i] = new Spherical_Receiver(SampleRate_in, CO_Time_in, rho_c[i]);
                }
            }

            /// <summary>
            /// Receiver bank constructor.
            /// </summary>
            /// <param name="Pt">array of receiver origin points</param>
            /// <param name="SrcPT">sound source point</param>
            /// <param name="Room">the acoustical scene</param>
            /// <param name="RCT">the number or rays emanating from the source</param>
            /// <param name="CSound">the speed of sound in m/s</param>
            /// <param name="SampleRate_in">the simulation histogram sampling frequency</param>
            /// <param name="COTime_in">the Cut Off Time in ms.</param>
            /// <param name="Type">the type of receivers contained in this receiver bank</param>
            public Receiver_Bank(IEnumerable<Point3d> Pt, Point3d SrcPT, Scene Sc, int SampleRate_in, double COTime_in, double delayinms, Type Type)
            {
                delay_ms = delayinms;
                SampleRate = SampleRate_in;
                SampleCT = (int)Math.Floor(COTime_in * SampleRate_in / 1000);
                this.CutOffTime = COTime_in;
                Rec_Type = Type;
                Point3d[] arrPts = Pt.ToArray<Point3d>();
                Rec_List = new Spherical_Receiver[arrPts.Length];
                Min = new Hare.Geometry.Point(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
                Max = new Hare.Geometry.Point(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

                for (int i = 0; i < arrPts.Length; i++)
                {
                    if (Type == Type.Stationary) Rec_List[i] = new Spherical_Receiver(new Point3d(arrPts[i]), new Point3d(SrcPT), Sc.Attenuation(Utilities.PachTools.RPttoHPt(arrPts[i])), Sc.Sound_speed(Utilities.PachTools.RPttoHPt(arrPts[i])), Sc.Rho(Utilities.PachTools.RPttoHPt(arrPts[i])), SampleRate_in, COTime_in);
                    if (Type == Type.Variable) Rec_List[i] = new Expanding_Receiver(new Point3d(arrPts[i]), new Point3d(SrcPT), RayCount, Sc.Attenuation(Utilities.PachTools.RPttoHPt(arrPts[i])), Sc.Sound_speed(Utilities.PachTools.RPttoHPt(arrPts[i])), Sc.Rho(Utilities.PachTools.RPttoHPt(arrPts[i])), SampleRate_in, COTime_in);

                    if (arrPts[i].X > Max.x) Max.x = arrPts[i].X;
                    if (arrPts[i].Y > Max.y) Max.y = arrPts[i].Y;
                    if (arrPts[i].Z > Max.z) Max.z = arrPts[i].Z;
                    if (arrPts[i].X < Min.x) Min.x = arrPts[i].X;
                    if (arrPts[i].Y < Min.y) Min.y = arrPts[i].Y;
                    if (arrPts[i].Z < Min.z) Min.z = arrPts[i].Z;
                }
            }

            public double CO_Time
            {
                get
                {
                    return CutOffTime;
                }
                set
                {
                    CutOffTime = value;
                    if (Rec_List == null) return;
                    foreach (Spherical_Receiver S in Rec_List) S.CO_Time = value;
                }
            }
            
            /// <summary>
            /// this method checks all receivers in the bank for intersections with a broadband ray.
            /// </summary>
            /// <param name="Length">the distance traveled by the ray up to the reflection prior to potential receiver intersection</param>
            /// <param name="R">the broadband ray</param>
            /// <param name="EndPt">the point of intersection with the room after the potential receiver intersection</param>
            public virtual void CheckBroadbandRay(BroadRay R, Hare.Geometry.Point EndPt)
            {
                foreach (Spherical_Receiver S in Rec_List) {S.CheckBroadbandRay(R,EndPt);};
            }

            /// <summary>
            /// this method checks all receivers in the bank for intersections with a single octave ray.
            /// </summary>
            /// <param name="Length">the distance traveled by the ray up to the reflection prior to potential receiver intersection</param>
            /// <param name="R">the single octave band ray</param>
            /// <param name="EndPt">the point of intersection with the room after the potential receiver intersection</param>
            public virtual void CheckRay(OctaveRay R, Hare.Geometry.Point EndPt)
            {
                foreach (Spherical_Receiver S in Rec_List) {S.CheckRay(R,EndPt);};
            }

            /// <summary>
            /// Checks all receivers for intersections with a ray. This method does not record energy.
            /// </summary>
            /// <param name="R">the single octave band ray</param>
            /// <param name="EndPt">the point of intersection with the room after the potential receiver intersection</param>
            /// <param name="Length">the distance traveled by the ray up to the reflection prior to potential receiver intersection</param>
            /// <returns> an array of boolean values, indexed by receiver number</returns>
            public virtual bool[] SimpleCheck(BroadRay R, Hare.Geometry.Point EndPt, double Length)
            {
                bool[] B = new bool[Rec_List.Length];
                for (int i = 0; i < Rec_List.Length; i++) { B[i] = Rec_List[i].SimpleCheck(R, EndPt);};
                return B;
            }

            /// <summary>
            /// this method saves a completed receiver bank to memory.
            /// </summary>
            /// <param name="BW">the binary writer used to copy data to disk.</param>
            public void Write_Data(ref System.IO.BinaryWriter BW)
            {
                //1. Write an indicator to signify that there is ray traced data: BOOL
                BW.Write("Ray-Traced_Data");

                //2. Write the type of receivers used
                BW.Write((int)Rec_Type);

                //3. Write the sample rate:int
                BW.Write(SampleRate);

                //4. Write the number of samples:int
                BW.Write(SampleCT);

                //5. Write the Speed of Sound:double (deprecated as of v.1.6)
                //BW.Write(343.0d);

                //6. Write the cut off time:double
                BW.Write(CutOffTime);

                for (int q = 0; q < Rec_List.Length; q++)
                {
                    //7a. Write the Speed of Sound for this receiver: double
                    BW.Write(Rec_List[q].Sound_Speed);

                    double[] Hist0 = Rec_List[q].GetEnergyHistogram(0);
                    double[] Hist1 = Rec_List[q].GetEnergyHistogram(1);
                    double[] Hist2 = Rec_List[q].GetEnergyHistogram(2);
                    double[] Hist3 = Rec_List[q].GetEnergyHistogram(3);
                    double[] Hist4 = Rec_List[q].GetEnergyHistogram(4);
                    double[] Hist5 = Rec_List[q].GetEnergyHistogram(5);
                    double[] Hist6 = Rec_List[q].GetEnergyHistogram(6);
                    double[] Hist7 = Rec_List[q].GetEnergyHistogram(7);

                    for (int j = 0; j < SampleCT; j++)
                    {
                        //7b. Write the echogram sample and directional sample:(double + 3double) * 8
                        BW.Write(Hist0[j]);                        
                        BW.Write(Rec_List[q].Directions_Pos(0, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(0, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(0, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(0, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(0, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(0, j, 2));

                        BW.Write(Hist1[j]);
                        BW.Write(Rec_List[q].Directions_Pos(1, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(1, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(1, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(1, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(1, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(1, j, 2));

                        BW.Write(Hist2[j]);
                        BW.Write(Rec_List[q].Directions_Pos(2, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(2, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(2, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(2, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(2, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(2, j, 2));

                        BW.Write(Hist3[j]);
                        BW.Write(Rec_List[q].Directions_Pos(3, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(3, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(3, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(3, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(3, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(3, j, 2));

                        BW.Write(Hist4[j]);
                        BW.Write(Rec_List[q].Directions_Pos(4, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(4, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(4, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(4, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(4, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(4, j, 2));

                        BW.Write(Hist5[j]);
                        BW.Write(Rec_List[q].Directions_Pos(5, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(5, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(5, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(5, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(5, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(5, j, 2));

                        BW.Write(Hist6[j]);
                        BW.Write(Rec_List[q].Directions_Pos(6, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(6, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(6, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(6, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(6, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(6, j, 2));

                        BW.Write(Hist7[j]);
                        BW.Write(Rec_List[q].Directions_Pos(7, j, 0));
                        BW.Write(Rec_List[q].Directions_Pos(7, j, 1));
                        BW.Write(Rec_List[q].Directions_Pos(7, j, 2));
                        BW.Write(Rec_List[q].Directions_Neg(7, j, 0));
                        BW.Write(Rec_List[q].Directions_Neg(7, j, 1));
                        BW.Write(Rec_List[q].Directions_Neg(7, j, 2));
                    }
                }
            }

            /// <summary>
            /// this method creates a complete Receiver bank from saved data.
            /// </summary>
            /// <param name="BR">the binary reader from which the data will come.</param>
            /// <param name="Rec_CT">The number of receivers</param>
            /// <param name="SampleRate">the histogram sampling frequency</param>
            /// <returns>a complete receiver bank</returns>
            public static Receiver_Bank Read_Data(ref System.IO.BinaryReader BR, int Rec_CT, IEnumerable<Hare.Geometry.Point> RecPts, double[] rho_c, double delayms, ref int SampleRate, string version)
            {
                //2. Write the type of receivers used
                Type Rec_typ = (Type)Enum.ToObject(typeof(Type), BR.ReadUInt32());

                //3. Write the sample rate:int
                SampleRate = BR.ReadInt32();

                //4. Write the number of samples:int
                int SampleCT = BR.ReadInt32();

                //5. Write the Speed of Sound:double (deprecated as of v1.6...)
                //double C_Sound = BR.ReadDouble();

                //6. Write the cut off time:double
                double CutoffTime = BR.ReadDouble();

                Receiver_Bank Rec = new Receiver_Bank(Rec_CT, SampleRate, CutoffTime, delayms, rho_c, new int[]{0,1,2,3,4,5,6,7},Rec_typ);

                double v = double.Parse(version.Substring(0, 3));

                for (int q = 0; q < Rec_CT; q++)
                {
                    Rec.Rec_List[q].H_Origin = RecPts.ElementAt<Hare.Geometry.Point>(q);
                    Rec.Rec_List[q].Recs.Energy[0] = new double[SampleCT];
                    Rec.Rec_List[q].Recs.Energy[1] = new double[SampleCT];
                    Rec.Rec_List[q].Recs.Energy[2] = new double[SampleCT];
                    Rec.Rec_List[q].Recs.Energy[3] = new double[SampleCT];
                    Rec.Rec_List[q].Recs.Energy[4] = new double[SampleCT];
                    Rec.Rec_List[q].Recs.Energy[5] = new double[SampleCT];
                    Rec.Rec_List[q].Recs.Energy[6] = new double[SampleCT];
                    Rec.Rec_List[q].Recs.Energy[7] = new double[SampleCT];

                    //if (v == 1.7)
                    //{
                    //    for (int i = 0; i < 16; i++) BR.ReadSingle();
                    //}
                    ////////Initiates the directional Histogram////////
                    //Rec.Rec_List[q].Recs = new Spherical_Receiver.Directional_Histogram(SampleRate, SampleCT);
                    //7a. Reads in the speed of sound.
                    Rec.Rec_List[q].Sound_Speed = BR.ReadDouble();

                    for (int j = 0; j < SampleCT; j++)
                    {
                        //7b. Write the echogram sample and directional sample:(double + 3double) * 8
                        for (int oct = 0; oct < 8; oct++)
                        {
                            Rec.Rec_List[q].Recs.Energy[oct][j] = BR.ReadDouble();
                            if (v == 1.7)
                            {
                                BR.ReadSingle(); BR.ReadSingle();
                                Rec.Rec_List[q].Recs.Add_direction(oct, j, new Vector(BR.ReadSingle(), BR.ReadSingle(), BR.ReadSingle()), new Vector(BR.ReadSingle(), BR.ReadSingle(), BR.ReadSingle()));
                            }
                            else
                            {
                                Rec.Rec_List[q].Recs.Add_direction(oct, j, new Vector(BR.ReadDouble(), BR.ReadDouble(), BR.ReadDouble()), new Vector(BR.ReadDouble(), BR.ReadDouble(), BR.ReadDouble()));
                            }
                        }
                    }
                }

                Rec.Create_Pressure();
                return Rec;
            }

            /// <summary>
            /// returns the raytraced energy response of a single receiver at an indexed octave band.
            /// </summary>
            /// <param name="Octave">octave band index</param>
            /// <param name="Rec_Index">the index of the receiver to be indexed</param>
            /// <returns>the energy response</returns>
            public virtual double[] GetEnergyHistogram(int Octave, int Rec_Index)
            {
                if (Octave < 8)
                {
                    return (double[])Rec_List[Rec_Index].GetEnergyHistogram(Octave).Clone();
                }
                else
                {
                    double[] Hist = Rec_List[Rec_Index].GetEnergyHistogram(0);
                    double[] SUM = new double[Hist.Length];
                    for (int o = 1; o < 8; o++)
                    {
                        Hist = Rec_List[Rec_Index].GetEnergyHistogram(o);
                        for (int i = 0; i < Hist.Length; i++)
                        {
                            SUM[i] += Hist[i];
                        }
                    }
                    return SUM;
                }
            }

            public virtual bool HasPressure()
            {
                return Rec_List[0].Recs.P != null;
            }

            public virtual void GetPressure(int Rec_Index, out double[] P)
            {
                if (Rec_List[Rec_Index].Recs.P == null) P = new double[SampleCT];
                else
                {
                    P = (double[])Rec_List[Rec_Index].Recs.GetPressure().Clone();
                }
            }

            /// <summary>
            /// get a histogram of vectors indicating the integrated direction of sound at any given time.
            /// </summary>
            /// <param name="Octave">the indexed octave band</param>
            /// <param name="t">time index</param>
            /// <param name="Rec_Index">the index of the receiver</param>
            /// <returns>the integrated direction of sound at the chosen frequency and time</returns>
            public virtual Vector Directions_Pos(int Octave, int t, int Rec_Index)
            {
                Vector Dir = new Vector(Rec_List[Rec_Index].Directions_Pos(Octave, t,0), Rec_List[Rec_Index].Directions_Pos(Octave, t, 1), Rec_List[Rec_Index].Directions_Pos(Octave, t, 2));
                return Dir;
            }

            /// <summary>
            /// get a histogram of vectors indicating the integrated direction of sound at any given time.
            /// </summary>
            /// <param name="Octave">the indexed octave band</param>
            /// <param name="t">time index</param>
            /// <param name="Rec_Index">the index of the receiver</param>
            /// <returns>the integrated direction of sound at the chosen frequency and time</returns>
            public virtual Vector Directions_Neg(int Octave, int t, int Rec_Index)
            {
                Vector Dir = new Vector(Rec_List[Rec_Index].Directions_Neg(Octave, t, 0), Rec_List[Rec_Index].Directions_Neg(Octave, t, 1), Rec_List[Rec_Index].Directions_Neg(Octave, t, 2));
                return Dir;
            }

            public virtual Vector Directions_Pos(int Octave, int t, int Rec_Index, double alt, double azi, bool degrees)
            {
                Vector V = new Vector(Rec_List[Rec_Index].Directions_Pos(Octave, t).x, Rec_List[Rec_Index].Directions_Pos(Octave, t).y, Rec_List[Rec_Index].Directions_Pos(Octave, t).z);
                return Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V, azi, 0, degrees), 0, alt, degrees);
            }

            public virtual Vector Directions_Neg(int Octave, int t, int Rec_Index, double alt, double azi, bool degrees)
            {
                Vector V = new Vector(Rec_List[Rec_Index].Directions_Neg(Octave, t).x, Rec_List[Rec_Index].Directions_Neg(Octave, t).y, Rec_List[Rec_Index].Directions_Neg(Octave, t).z);
                return Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V, azi, 0, degrees), 0, alt, degrees);
            }

            public virtual Vector Directions_Pos(int Octave, int t, int Rec_Index, Vector V)
            {
                double l = Math.Sqrt(V.z * V.z + V.x * V.x);
                double azi = Math.Asin(V.y / l);
                double alt = Math.Atan2(V.x,  V.z);
                return Directions_Pos(Octave, t, Rec_Index, alt, azi, false);
            }

            public virtual Vector Directions_Neg(int Octave, int t, int Rec_Index, Vector V)
            {
                double l = Math.Sqrt(V.z * V.z + V.x * V.x);
                double azi = Math.Asin(V.y / l);
                double alt = Math.Atan2(V.x, V.z);
                return Directions_Neg(Octave, t, Rec_Index, alt, azi, false);
            }

            /// <summary>
            /// combines all threadlocal results
            /// </summary>
            /// <param name="Rec">a receiver bank clone to combine with this receiver bank instance</param>
            public virtual void Combine_Clones(Receiver_Bank Rec)
            {
                //C_Sound = SoundSpeed;
                for (int R = 0; R < Rec.Count; R++)
                {
                    for (int oct = 0; oct < 8; oct++)
                    {
                        for (int T = 0; T < Rec.Duration(); T++)
                        {
                            Rec_List[R].Combine_Sample(T, Rec.Rec_List[R].Energy(T, oct), Rec.Rec_List[R].Directions_Pos(oct,T), Rec.Rec_List[R].Directions_Neg(oct,T), oct);
                        }
                    }
                }
            }

            /// <summary>
            /// Gets the origin of a receiver in the bank
            /// </summary>
            /// <param name="ID">the index or 'ID' of a receiver in the bank</param>
            /// <returns>the origin point in Rhino format</returns>
            public Point3d Origin(int ID)
            {
                return Utilities.PachTools.HPttoRPt(Rec_List[ID].H_Origin);
            }

            /// <summary>
            /// Gets the origin of a receiver in the bank
            /// </summary>
            /// <param name="ID">the index or 'ID' of a receiver in the bank</param>
            /// <returns>the origin point in Hare format</returns>
            public Hare.Geometry.Point H_Origin(int ID)
            {
                return Rec_List[ID].H_Origin;
            }

            public Point3d[] Origins()
            {
                Point3d[] P = new Point3d[Rec_List.Length];
                for (int i = 0; i < Rec_List.Length; i++)
                {
                    P[i] = Utilities.PachTools.HPttoRPt(Rec_List[i].H_Origin);
                }
                return P;
            }

            /// <summary>
            /// obtains the number of samples in histograms owned by this instance.
            /// </summary>
            /// <returns>Number of histogram samples</returns>
            public int Duration()
            {
                return SampleCT;
            }

            /// <summary>
            /// Gets the number of receivers in this bank.
            /// </summary>
            public virtual int Count 
            {
                get 
                {
                    return Rec_List.Length;
                }
            }

            /// <summary>
            /// Combines two Map Receivers with data in differing octave bands.
            /// </summary>
            /// <param name="A"></param>
            /// <param name="B"></param>
            /// <returns></returns>
            public static Receiver_Bank operator *(Receiver_Bank A, Receiver_Bank B)
            {
                Rhino.Geometry.Point3d[] a_pts = A.Origins();
                Rhino.Geometry.Point3d[] b_pts = B.Origins();
                if (A.Rec_Type != B.Rec_Type || a_pts.Length != b_pts.Length)
                {
                    System.Windows.Forms.MessageBox.Show("Data is for two different calculations. Simulations not Combined.");
                    return null;
                }

                for (int i = 0; i < a_pts.Length; i++)
                {
                    if (a_pts[i].GetHashCode() != b_pts[i].GetHashCode())
                    {
                        System.Windows.Forms.MessageBox.Show("Data is for two different calculations. Simulations not Combined.");
                        return null;
                    }
                }

                foreach (int a in A.Oct_choice)
                {
                    foreach (int b in B.Oct_choice)
                    {
                        if (a == b)
                        {
                            System.Windows.Forms.MessageBox.Show("Data Conflicts. Simulations not Combined.");
                            return null;
                        }
                    }
                }

                foreach (int oct in B.Oct_choice)
                {
                    for (int rec = 0; rec < a_pts.Length; rec++)
                    {
                        for (int t = 0; t <= A.Rec_List[rec].Recs.SampleCT; t++) A.Rec_List[rec].Combine_Sample(t, B.Rec_List[rec].Energy(t,oct), B.Rec_List[rec].Directions_Pos(oct, t), B.Rec_List[rec].Directions_Neg(oct, t), oct);
                    }
                }

                return A;
            }

            public virtual void Create_Pressure()
            {
                for (int rec = 0; rec < Rec_List.Length; rec++)
                {
                    Rhino.RhinoApp.SetCommandPrompt(String.Format("Generating Pressure for Receiver {0} of {1}.", rec, Rec_List.Length));
                    Rec_List[rec].Create_Pressure();
                }
            }

            public double[][] Pressure_3Axis(int rec_id)
            {
                return Rec_List[rec_id].Pressure3Axis();
            }
        }

        /// <summary>
        /// 1m. stationary receiver
        /// </summary>
        [Serializable]
        public class Spherical_Receiver
        {
            public int[] Ray_ID;
            public Hare.Geometry.Point H_Origin;
            protected double C_Sound;
            public double Rho_C;
            protected double Inv_C_Sound;
            protected double[] Atten = new double[8];
            protected internal Histogram Recs;
            protected double D_Length;
            public int SampleRate;
            public double CO_Time;
            protected internal double Radius;
            protected internal double Radius2;
            protected internal double SizeMod;

            public Spherical_Receiver()
            { }

            /// <summary>
            /// Use this constructor when reading in a file...
            /// </summary>
            /// <param name="SampleRate_in">The sampling frequency at which detections will be recorded. Enter 0 for raw detection recording. Watch out for memory overflow...</param>
            /// <param name="CSound_in"> the speed of sound</param>
            /// <param name="CO_Time_in">the Cut Off Time in ms.</param>
            public Spherical_Receiver(int SampleRate_in, double CO_Time_in, double rho_c)
            {
                Rho_C = rho_c;
                CO_Time = CO_Time_in;
                SampleRate = SampleRate_in;
                Recs = new Directional_Histogram(SampleRate, CO_Time);
            }
            
            /// <summary>
            /// Constructor which takes Rhino point input.
            /// </summary>
            /// <param name="Center"></param>
            /// <param name="SrcCenter"></param>
            /// <param name="room"></param>
            /// <param name="RCT"></param>
            /// <param name="C_Sound_in"></param>
            /// <param name="SampleRate_in"></param>
            /// <param name="COTime_in"></param>
            public Spherical_Receiver(Point3d Center, Point3d SrcCenter, double[] Attenuation, double C_Sound_in, double rho, int SampleRate_in, double COTime_in)
            {
                D_Length = Center.DistanceTo(SrcCenter);
                Radius = 1;
                Radius2 = Radius * Radius;
                SampleRate = SampleRate_in;
                CO_Time = COTime_in;
                H_Origin = new Hare.Geometry.Point(Center.X, Center.Y, Center.Z);
                Sound_Speed = C_Sound_in;
                Rho_C = C_Sound_in * rho;
                Atten = new double[8];
                for (int o = 0; o < 8; o++) Atten[o] = Attenuation[o] * 2;
                SizeMod = 1 / Math.PI;
                Recs = new Directional_Histogram(SampleRate, CO_Time);
            }

            /// <summary>
            /// Constructor which takes Hare Point input.
            /// </summary>
            /// <param name="Center"></param>
            /// <param name="SrcCenter"></param>
            /// <param name="room"></param>
            /// <param name="RCT"></param>
            /// <param name="C_Sound_in"></param>
            /// <param name="SampleRate_in"></param>
            /// <param name="COTime_in"></param>
            public Spherical_Receiver(Hare.Geometry.Point Center, Hare.Geometry.Point SrcCenter, Scene room, double C_Sound_in, int SampleRate_in, double COTime_in)
            {
                D_Length = Math.Sqrt(Center.x * SrcCenter.x + Center.y * SrcCenter.y * Center.z * SrcCenter.z);
                Radius = 1;
                Radius2 = Radius * Radius;
                CO_Time = COTime_in;
                SampleRate = SampleRate_in;
                H_Origin = Center;
                //Origin = new Point3d(Center.x, Center.y, Center.z);
                Sound_Speed = C_Sound_in;
                Atten = room.Attenuation(Center);
                SizeMod = 1 / Math.PI;
                Recs = new Directional_Histogram(SampleRate, CO_Time);
            }

            /// <summary>
            /// This method checks a receiver for a ray using a Broadband Ray (these typically occur before rays are split in the Raytracing simulation.
            /// </summary>
            /// <param name="Length">The length of the ray at the reflection point before potential intersection</param>
            /// <param name="R">the ray.</param>
            /// <param name="EndPt">The point at which the ray intersects the model after potential receiver intersection.</param>
            public virtual void CheckBroadbandRay(BroadRay R, Hare.Geometry.Point EndPt)
            {
                Vector m = R.origin - H_Origin;
                double b = Hare_math.Dot(m, R.direction);
                double c = Hare_math.Dot(m, m)  - Radius2;
                if (c > 0 && b > 0) return;
                double discr = b * b - c;
                if (discr < 0) return;
                double t1 = -b - Math.Sqrt(discr);
                double t2 = -b + Math.Sqrt(discr);
                double tsphere = (t2 - t1) * 0.5;
                double t = (t1 + t2) * .5;
                if (t > 0 && t * t < SqDistance(EndPt, R.origin))
                {
                    double RayTime = t*Inv_C_Sound + R.t_sum;
                    Vector Dir = R.direction * -1;
                    Dir.Normalize();
                    Recs.Add(RayTime, R.Energy[0] * Math.Pow(10,-.1 * Atten[0] * t) * SizeMod * tsphere, Dir, R.phase[0], Rho_C, 0);
                    Recs.Add(RayTime, R.Energy[1] * Math.Pow(10,-.1 * Atten[1] * t) * SizeMod * tsphere, Dir, R.phase[1], Rho_C, 1);
                    Recs.Add(RayTime, R.Energy[2] * Math.Pow(10,-.1 * Atten[2] * t) * SizeMod * tsphere, Dir, R.phase[2], Rho_C, 2);
                    Recs.Add(RayTime, R.Energy[3] * Math.Pow(10,-.1 * Atten[3] * t) * SizeMod * tsphere, Dir, R.phase[3], Rho_C, 3);
                    Recs.Add(RayTime, R.Energy[4] * Math.Pow(10,-.1 * Atten[4] * t) * SizeMod * tsphere, Dir, R.phase[4], Rho_C, 4);
                    Recs.Add(RayTime, R.Energy[5] * Math.Pow(10,-.1 * Atten[5] * t) * SizeMod * tsphere, Dir, R.phase[5], Rho_C, 5);
                    Recs.Add(RayTime, R.Energy[6] * Math.Pow(10,-.1 * Atten[6] * t) * SizeMod * tsphere, Dir, R.phase[6], Rho_C, 6);
                    Recs.Add(RayTime, R.Energy[7] * Math.Pow(10,-.1 * Atten[7] * t) * SizeMod * tsphere, Dir, R.phase[7], Rho_C, 7);
                }
            }

            /// <summary>
            /// Checks receiver for an intersection with a ray 
            /// </summary>
            /// <param name="Length">The length of the ray at the reflection point before potential intersection</param>
            /// <param name="R">the ray.</param>
            /// <param name="EndPt">The point at which the ray intersects the model after potential receiver intersection.</param>
            public virtual void CheckRay(OctaveRay R, Hare.Geometry.Point EndPt)
            {
                Vector m = R.origin - H_Origin;
                double b = Hare_math.Dot(m, R.direction);
                double c = Hare_math.Dot(m, m) - Radius2;
                if (c > 0 && b > 0) return;
                double discr = b * b - c;
                if (discr < 0) return;
                double t1 = -b - Math.Sqrt(discr);
                double t2 = -b + Math.Sqrt(discr);
                double tsphere = (t2 - t1) * 0.5;
                double t = (t1 + t2) *.5;
                if (t > 0 && t * t < SqDistance(EndPt, R.origin))
                {
                    Vector Dir = R.direction * -1;
                    Dir.Normalize();
                    double Raydist = t*Inv_C_Sound + R.t_sum;
                    Recs.Add(Raydist, R.Intensity * Math.Pow(10,-.1 * Atten[R.Octave] * t) * SizeMod * tsphere, Dir, R.phase, Rho_C, R.Octave);
                }
            }

            /// <summary>
            /// This method checks a receiver for an intersection with a ray.
            /// </summary>
            /// <param name="R">the ray.</param>
            /// <param name="EndPt">The point at which the ray intersects the model after potential receiver intersection.</param>
            /// <param name="Length">The length of the ray at the reflection point before potential intersection</param>
            /// <returns>true if intersects, false if not.</returns>
            public virtual bool SimpleCheck(BroadRay R, Hare.Geometry.Point EndPt)
            {
                Vector m = R.origin - H_Origin;
                double b = Hare_math.Dot(m, R.direction);
                double c = Hare_math.Dot(m, m) - Radius2;
                if (c > 0 && b > 0) return false;
                double discr = b * b - c;
                if (discr < 0) return false;
                double t1 = -b - Math.Sqrt(discr);
                double t2 = -b + Math.Sqrt(discr);
                double t = (t1 + t2) * .5;
                if (t > 0 && t * t < SqDistance(EndPt, R.origin)) return true;
                return false;
            }

            /// <summary>
            /// A quick way of getting a relative distance between two points. The equivalent of the pythagorean theorem without the last square root.
            /// </summary>
            /// <param name="StartPt">Point 1</param>
            /// <param name="EndPt">Point 2</param>
            /// <returns>the square of the distance</returns>
            protected double SqDistance(Hare.Geometry.Point StartPt, Hare.Geometry.Point EndPt)
            {
                Hare.Geometry.Point DistPt = EndPt - StartPt;
                return DistPt.x * DistPt.x + DistPt.y * DistPt.y + DistPt.z * DistPt.z;
            }

            /// <summary>
            /// returns the energy response of the simulation.
            /// </summary>
            /// <param name="Octave">an octave band index</param>
            /// <returns></returns>
            public double[] GetEnergyHistogram(int Octave)
            {
                return Recs.GetEnergyHistogram(Octave);
            }

            /// <summary>
            /// returns the direction of sound at a given time.
            /// </summary>
            /// <param name="Octave">an octave band index</param>
            /// <param name="t">a point in time</param>
            /// <returns></returns>
            public virtual Vector Directions_Pos(int Octave, int t)
            {
                return Recs.Directions_Pos(Octave, t);
            }

            /// <summary>
            /// returns the direction of sound at a given time.
            /// </summary>
            /// <param name="Octave">an octave band index</param>
            /// <param name="t">a point in time</param>
            /// <returns></returns>
            public virtual Vector Directions_Neg(int Octave, int t)
            {
                return Recs.Directions_Neg(Octave, t);
            }

            ///// <summary>
            ///// returns the direction of sound at a given time.
            ///// </summary>
            ///// <param name="Octave">an octave band index</param>
            ///// <param name="t">a point in time</param>
            ///// <param name="c">0 for x, 1 for y, 2 for z</param>
            /// <returns></returns>
            public virtual double Directions_Pos(int Octave, int t, int c)
            {
                return Recs.Directions_Pos(Octave, t, c);
            }

            ///// <summary>
            ///// returns the direction of sound at a given time.
            ///// </summary>
            ///// <param name="Octave">an octave band index</param>
            ///// <param name="t">a point in time</param>
            ///// <param name="c">0 for x, 1 for y, 2 for z</param>
            /// <returns></returns>
            public virtual double Directions_Neg(int Octave, int t, int c)
            {
                return Recs.Directions_Neg(Octave, t, c);
            }

            /// <summary>
            /// An energy value at a given time and frequency.
            /// </summary>
            /// <param name="t">the index of the sample.</param>
            /// <param name="oct">the index of the octave band</param>
            /// <returns></returns>
            public virtual double Energy(int t, int oct)
            {
                if (oct == 8) return Recs.Energy[0][t] + Recs.Energy[1][t] + Recs.Energy[2][t] + Recs.Energy[3][t] + Recs.Energy[4][t] + Recs.Energy[5][t] + Recs.Energy[6][t] + Recs.Energy[7][t];
                return Recs.Energy[oct][t];
            }

            /// <summary>
            /// Add an energy entry to this receiver's histograms.
            /// </summary>
            /// <param name="Distance">the distance traveled from the source to this receiver detection</param>
            /// <param name="Energy_in">the energy to be recorded</param>
            /// <param name="direction">The incoming direction of the ray</param>
            /// <param name="Octave">the octave band index</param>
            public void Add(double Distance, double Energy_in, Vector direction, double phase, int Octave)
            {
                Recs.Add(Distance, Energy_in, direction, phase, Rho_C, Octave);
            }

            /// <summary>
            /// Add an energy entry to this receiver's histograms using the actual sample point.
            /// </summary>
            /// <param name="Sample"></param>
            /// <param name="Energy_in"></param>
            /// <param name="direction"></param>
            /// <param name="Octave"></param>
            public void Add(int Sample, double Energy_in, Vector direction, double phase, int Octave)
            {
                Recs.Add(Sample, Energy_in, direction, phase, Rho_C, Octave);
            }

            public void Combine_Sample(int sample, double Energy_in, Vector direction_pos, Vector direction_neg, int Octave)// float p_real, float p_imag,
            {
                Recs.Combine_Sample(sample, Energy_in, direction_pos, direction_neg, Octave);// p_real, p_imag,
            }

            public double Sound_Speed
            {
                get 
                {
                    return C_Sound;
                }
                set 
                {
                    C_Sound = value;
                    Inv_C_Sound = 1 / value;
                }
            }

            public virtual void Create_Pressure()
            {
                Recs.Create_Pressure(Rho_C);
            }

            public double[][] Pressure3Axis()
            {
                if (Recs is Directional_Histogram)
                {
                    return (Recs as Directional_Histogram).Pdir;
                }
                else return null;
            }

            /// <summary>
            /// Class used ot store the energy histogram of the receiver.
            /// </summary>
            [Serializable]
            protected internal class Histogram
            {
                protected internal double[][] Energy;
                protected internal double[] P;
                protected internal double CO_Time;
                protected internal int SampleRate;
                protected internal int SampleCT;
 
                public Histogram(int SampleRate_in, int SampleCT)
                {
                    SampleRate = SampleRate_in;
                    this.SampleCT = SampleCT;
                    CO_Time = (double)SampleCT / SampleRate;
                    Energy = new double[8][];
                    //P = new double[SampleCT + 8192];

                    for (int o = 0; o < 8; o++)
                    {
                        Energy[o] = new double[SampleCT];
                    }
                }

                public Histogram(int SampleRate_in, double COTime_in)
                {
                    SampleRate = SampleRate_in;
                    CO_Time = COTime_in / 1000;
                    SampleCT = (int)(CO_Time * SampleRate);
                    Energy = new double[8][];
                    //P = new double[8][];

                    for (int o = 0; o < 8; o++)
                    {
                        Energy[o] = new double[SampleCT];
                    }
                }

                public virtual string Type()
                {
                    return "Type;Map_Data_NoDir";
                }

                /// <summary>
                /// Adds energy detections to this histogram.
                /// </summary>
                /// <param name="Sample">the time point of arrival</param>
                /// <param name="Energy_in">the energy to be recorded</param>
                /// <param name="direction">the incoming direction of the ray</param>
                /// <param name="Octave">the index of the octave band.</param>
                public virtual void Add(double time, double Energy_in, Vector direction, double phase, double Rho_C, int Octave)
                {
                    int sample = (int)(time * SampleRate);
                    if (sample >= Energy[Octave].Length) return;
                    if (sample < 0) return;
                    float real, imag;
                    Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[Octave] * time + phase), out real, out imag);
                    float pmag = (float)Math.Sqrt(Energy_in * Rho_C);
                    Energy[Octave][sample] += Energy_in;
                }

                public virtual void Combine_Sample(int Sample, double Energy_in, Vector direction_pos, Vector direction_neg, int Octave)
                {
                    if (Sample >= Energy[Octave].Length) return;
                    Energy[Octave][Sample] += Energy_in;
                }

                public virtual void Create_Pressure(double Rho_C)
                {
                    if (Energy[0].Length == 1)
                    {
                        return;
                    }
                    P = Audio.Pach_SP.ETCToPTC(this.Energy, this.CO_Time, this.SampleRate, 44100, Rho_C);
                }

                /// <summary>
                /// Returns the energy response of the simulation.
                /// </summary>
                /// <param name="Octave">the octave band index</param>
                /// <returns></returns>
                public virtual double[] GetEnergyHistogram(int Octave)
                {
                    if (Octave < 8)
                    {
                        return Energy[Octave];
                    }
                    else 
                    {
                        double[] Sum_Energy = new double[(Energy[0].Length)];
                        for (int Oct = 0; Oct < 8; Oct++)
                        {
                            for (int idx = 0; idx < Energy[Oct].Length; idx++)
                            {
                                Sum_Energy[idx] += Energy[Oct][idx];
                            }
                        }
                        return Sum_Energy;
                    }
                }

                public virtual double[] GetPressure()
                {
                    return P;
                }

                public virtual double[][] GetPressure3Axis(int rec_id)
                {
                    return null;
                }

                public virtual void Divide(double divisor)
                {
                    for (int i = 0; i < Energy.Length; i++)
                    {
                        for (int j = 0; j < Energy[i].Length; j++)
                        {
                            Energy[i][j] /= divisor;
                        }
                    }
                }

                /// <summary>
                /// Direction addiion empty for this operation.
                /// </summary>
                /// <param name="octave"></param>
                /// <param name="sample"></param>
                /// <param name="V"></param>
                public virtual void Add_direction(int octave, int sample, Vector VPos, Vector VNeg)
                {
                    return;
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <returns></returns>
                public virtual Vector Directions_Pos(int oct, int t)
                {
                    return new Vector(0,0,-1);
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <returns></returns>
                public virtual Vector Directions_Neg(int oct, int t)
                {
                    return new Vector(0, 0, -1);
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <param name="c">0 for x, 1 for y, 2 for z</param>
                /// <returns></returns>
                public virtual double Directions_Pos(int oct, int t, int c)
                {
                    return 0;
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <param name="c">0 for x, 1 for y, 2 for z</param>
                /// <returns></returns>
                public virtual double Directions_Neg(int oct, int t, int c)
                {
                    return 0;
                }
            }

            /// <summary>
            /// Class used ot store the energy histogram of the receiver.
            /// </summary>
            [Serializable]
            protected internal class Histogram_1Pt:Histogram
            {
                public Histogram_1Pt(int SampleRate_in, int SampleCT)
                :base(1, 1)
                {
                }

                public Histogram_1Pt(int SampleRate_in, double COTime_in)
                :base(1, 1)
                {
                }

                public virtual string Receiver_Type()
                {
                    return "Type;Map_Data_NoDir";
                }

                /// <summary>
                /// Adds energy detections to this histogram.
                /// </summary>
                /// <param name="Sample">the time point of arrival</param>
                /// <param name="Energy_in">the energy to be recorded</param>
                /// <param name="direction">the incoming direction of the ray</param>
                /// <param name="Octave">the index of the octave band.</param>
                public override void Add(double time, double Energy_in, Vector direction, double phase, double Rho_C, int Octave)
                {
                    Energy[Octave][0] += Energy_in;
                    //float real, imag;
                    //Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[Octave] * time + phase), out real, out imag);
                    ////float pmag = (float)Math.Sqrt(Energy_in * Rho_C);
                    ///Not sure how to address this pressure at one point... Maybe we need to keep the full histogram till the end, and then add together h^2.
                    ///Maybe this is an intensity only technique...
                    //P_Real[Octave][0] += pmag * real;
                    //P_Imag[Octave][0] += pmag * imag;
                }
            }

            /// <summary>
            /// This class stores all energy detections in a compact, reduced sampling rate series of histograms.
            /// </summary>
            [Serializable]
            protected internal class Directional_Histogram:Histogram
            {
                public double[][][] Dir_Rec_Pos;
                public double[][][] Dir_Rec_Neg;
                public double[][] Pdir;
                public Directional_Histogram(int SampleRate_in, int SampleCT)
                :base(SampleRate_in, SampleCT)
                {
                    SampleRate = SampleRate_in;
                    this.SampleCT = SampleCT;
                    Energy = new double[8][];

                    for (int o = 0; o < 8; o++)
                    {
                        Energy[o] = new double[SampleCT];
                    }
                    Dir_Rec_Pos = new double[3][][];
                    Dir_Rec_Neg = new double[3][][];
                    for (int i = 0; i < 3; i++)
                    {
                        Dir_Rec_Pos[i] = new double[8][];
                        Dir_Rec_Neg[i] = new double[8][];
                        for (int oct = 0; oct < 8; oct++)
                        {
                            Dir_Rec_Pos[i][oct] = new double[SampleCT];
                            Dir_Rec_Neg[i][oct] = new double[SampleCT];
                        }
                    }
                }

                /// <summary>
                /// gets the histogram of aggregated vector directions.
                /// </summary>
                /// <param name="SampleRate_in"></param>
                /// <param name="C_Sound_in"></param>
                /// <param name="COTime_in"></param>
                public Directional_Histogram(int SampleRate_in, double COTime_in)
                :base(SampleRate_in, COTime_in)
                {
                    SampleRate = SampleRate_in;
                    CO_Time = COTime_in / 1000;
                    SampleCT = (int)(CO_Time * SampleRate);
                    Energy = new double[8][];
                    for (int o = 0; o < 8; o++)
                    {
                        Energy[o] = new double[(int)(SampleRate * CO_Time)];
                    }
                    Dir_Rec_Pos = new double[3][][];
                    Dir_Rec_Neg = new double[3][][];
                    for (int i = 0; i < 3; i++)
                    {
                        Dir_Rec_Pos[i] = new double[8][];
                        Dir_Rec_Neg[i] = new double[8][];
                        for (int oct = 0; oct < 8; oct++)
                        {
                            Dir_Rec_Pos[i][oct] = new double[SampleCT];
                            Dir_Rec_Neg[i][oct] = new double[SampleCT];
                        }
                    }
                }

                public override string Type()
                {
                    return "Type;Map_Data";
                }

                public override void Combine_Sample(int Sample, double Energy_in, Vector direction_pos, Vector direction_neg, int Octave)// float p_real, float p_imag,
                {
                    if (Sample >= Energy[Octave].Length) return;
                    if (Sample < 0) Sample = 0;
                    Energy[Octave][Sample] += Energy_in;
                    Dir_Rec_Pos[0][Octave][Sample] += (direction_pos.x);
                    Dir_Rec_Pos[1][Octave][Sample] += (direction_pos.y);
                    Dir_Rec_Pos[2][Octave][Sample] += (direction_pos.z);
                    Dir_Rec_Pos[0][Octave][Sample] += (direction_neg.x);
                    Dir_Rec_Pos[1][Octave][Sample] += (direction_neg.y);
                    Dir_Rec_Pos[2][Octave][Sample] += (direction_neg.z);
                }

                /// <summary>
                /// Returns the energy response of the simulation.
                /// </summary>
                /// <param name="Octave">the octave band index</param>
                /// <returns></returns>
                public override double[] GetEnergyHistogram(int Octave)
                {
                    if (Octave < 8)
                    {
                        return Energy[Octave];
                    }
                    else 
                    {
                        double[] Sum_Energy = new double[(Energy[0].Length)];
                        for (int Oct = 0; Oct < 8; Oct++)
                        {
                            for (int idx = 0; idx < Energy[Oct].Length; idx++)
                            {
                                Sum_Energy[idx] += Energy[Oct][idx];
                            }
                        }
                        return Sum_Energy;
                    }
                }

                public override void Add(double time, double Energy_in, Vector direction, double phase, double Rho_C, int Octave)
                {
                    int sample = (int)(time * SampleRate);
                    if (sample >= Energy[Octave].Length) return;
                    Energy[Octave][sample] += Energy_in;
                    if (direction.x > 0) Dir_Rec_Pos[0][Octave][sample] += (float)(direction.x * Energy_in);
                    else Dir_Rec_Neg[0][Octave][sample] += (float)(direction.x * Energy_in);
                    if (direction.y > 0) Dir_Rec_Pos[1][Octave][sample] += (float)(direction.y * Energy_in);
                    else Dir_Rec_Neg[1][Octave][sample] += (float)(direction.y * Energy_in);
                    if (direction.z > 0) Dir_Rec_Pos[2][Octave][sample] += (float)(direction.z * Energy_in);
                    else Dir_Rec_Pos[2][Octave][sample] += (float)(direction.z * Energy_in);
                    float real, imag;
                    Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[Octave] * time + phase), out real, out imag);
                    float pmag = (float)Math.Sqrt(Energy_in * Rho_C);
                }

                /// <summary>
                /// Adds the direction of the arrival.
                /// </summary>
                /// <param name="octave">the octave of the arriving ray.</param>
                /// <param name="sample">the time point of arrival</param>
                /// <param name="V">the direction of the arrival.</param>
                public override void Add_direction(int octave, int sample, Vector VPos, Vector VNeg)
                {
                    this.Dir_Rec_Pos[0][octave][sample] = (float)VPos.x;
                    this.Dir_Rec_Pos[1][octave][sample] = (float)VPos.y;
                    this.Dir_Rec_Pos[2][octave][sample] = (float)VPos.z;
                    this.Dir_Rec_Neg[0][octave][sample] = (float)VNeg.x;
                    this.Dir_Rec_Neg[1][octave][sample] = (float)VNeg.y;
                    this.Dir_Rec_Neg[2][octave][sample] = (float)VNeg.z;
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <returns></returns>
                public override Vector Directions_Pos(int oct, int t)
                {
                    if (oct < 8) return new Vector(Dir_Rec_Pos[0][oct][t], Dir_Rec_Pos[1][oct][t], Dir_Rec_Pos[2][oct][t]);
                    
                    Vector D = new Vector(Dir_Rec_Pos[0][0][t], Dir_Rec_Pos[1][0][t], Dir_Rec_Pos[2][0][t]);
                    D.x += Dir_Rec_Pos[0][1][t] + Dir_Rec_Pos[0][2][t] + Dir_Rec_Pos[0][3][t] + Dir_Rec_Pos[0][4][t] + Dir_Rec_Pos[0][5][t] + Dir_Rec_Pos[0][6][t] + Dir_Rec_Pos[0][7][t];
                    D.y += Dir_Rec_Pos[1][1][t] + Dir_Rec_Pos[1][2][t] + Dir_Rec_Pos[1][3][t] + Dir_Rec_Pos[1][4][t] + Dir_Rec_Pos[1][5][t] + Dir_Rec_Pos[1][6][t] + Dir_Rec_Pos[1][7][t];
                    D.x += Dir_Rec_Pos[2][1][t] + Dir_Rec_Pos[2][2][t] + Dir_Rec_Pos[2][3][t] + Dir_Rec_Pos[2][4][t] + Dir_Rec_Pos[2][5][t] + Dir_Rec_Pos[2][6][t] + Dir_Rec_Pos[2][7][t];
                    return D;
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <returns></returns>
                public override Vector Directions_Neg(int oct, int t)
                {
                    if (oct < 8) return new Vector(Dir_Rec_Neg[0][oct][t], Dir_Rec_Neg[1][oct][t], Dir_Rec_Neg[2][oct][t]);

                    Vector D = new Vector(Dir_Rec_Neg[0][0][t], Dir_Rec_Neg[1][0][t], Dir_Rec_Neg[2][0][t]);
                    D.x += Dir_Rec_Neg[0][1][t] + Dir_Rec_Neg[0][2][t] + Dir_Rec_Neg[0][3][t] + Dir_Rec_Neg[0][4][t] + Dir_Rec_Neg[0][5][t] + Dir_Rec_Neg[0][6][t] + Dir_Rec_Neg[0][7][t];
                    D.y += Dir_Rec_Neg[1][1][t] + Dir_Rec_Neg[1][2][t] + Dir_Rec_Neg[1][3][t] + Dir_Rec_Neg[1][4][t] + Dir_Rec_Neg[1][5][t] + Dir_Rec_Neg[1][6][t] + Dir_Rec_Neg[1][7][t];
                    D.x += Dir_Rec_Neg[2][1][t] + Dir_Rec_Neg[2][2][t] + Dir_Rec_Neg[2][3][t] + Dir_Rec_Neg[2][4][t] + Dir_Rec_Neg[2][5][t] + Dir_Rec_Neg[2][6][t] + Dir_Rec_Neg[2][7][t];
                    return D;
                }

                public override void Divide(double divisor)
                {
                    for (int i = 0; i < Energy.Length; i++)
                    {
                        for (int j = 0; j < Energy[i].Length; j++)
                        {
                            this.Energy[i][j] /= divisor;
                            for (int k = 0; k < 3; k++) this.Dir_Rec_Pos[k][i][j] /= (float)divisor;
                            for (int k = 0; k < 3; k++) this.Dir_Rec_Neg[k][i][j] /= (float)divisor;
                        }
                    }
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <param name="c">0 for x, 1 for y, 2 for z</param>
                /// <returns></returns>
                public override double Directions_Pos(int oct, int t, int c)
                {
                    return Dir_Rec_Pos[c][oct][t];
                }

                public override void Create_Pressure(double Rho_C)
                {
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. Creating Omnidirectional..");
                    P = Audio.Pach_SP.ETCToPTC(this.Energy, this.CO_Time, this.SampleRate, 44100, Rho_C);
                    Pdir = new double[6][];
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. Creating Positive X...");
                    Pdir[0] = Audio.Pach_SP.ETCToPTC(this.Dir_Rec_Pos[0], this.CO_Time, this.SampleRate, 44100, Rho_C);
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. Creating Negative X...");
                    Pdir[1] = Audio.Pach_SP.ETCToPTC(this.Dir_Rec_Neg[0], this.CO_Time, this.SampleRate, 44100, Rho_C);
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. Creating Positive Y...");
                    Pdir[2] = Audio.Pach_SP.ETCToPTC(this.Dir_Rec_Pos[1], this.CO_Time, this.SampleRate, 44100, Rho_C);
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. Creating Negative Y...");
                    Pdir[3] = Audio.Pach_SP.ETCToPTC(this.Dir_Rec_Neg[1], this.CO_Time, this.SampleRate, 44100, Rho_C);
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. Creating Positive Z...");
                    Pdir[4] = Audio.Pach_SP.ETCToPTC(this.Dir_Rec_Pos[2], this.CO_Time, this.SampleRate, 44100, Rho_C);
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. Creating Negative Z...");
                    Pdir[5] = Audio.Pach_SP.ETCToPTC(this.Dir_Rec_Neg[2], this.CO_Time, this.SampleRate, 44100, Rho_C);
                    Rhino.RhinoApp.SetCommandPrompt("Generating Pressure for Cardinal directions. All Pressure Channels Complete");
                }

                /// <summary>
                /// the direction of the sound at a given instant in the energy response.
                /// </summary>
                /// <param name="oct">octave band index</param>
                /// <param name="t">the sample in time of the energy response to reference</param>
                /// <param name="c">0 for x, 1 for y, 2 for z</param>
                /// <returns></returns>
                public override double Directions_Neg(int oct, int t, int c)
                {
                    return Dir_Rec_Neg[c][oct][t];
                }

                public override double[][] GetPressure3Axis(int rec_id)
                {
                    return Pdir;
                }
            }
        }

        [Serializable]
        public class Expanding_Receiver : Spherical_Receiver
        {
            int RayCount;

            public Expanding_Receiver(Point3d Point, Point3d SrcPoint, int RCT, double[] Attenuation, double Speed_of_Sound, double rho, int SampleRate, double COTime)
                : base(Point, SrcPoint, Attenuation, Speed_of_Sound, rho, SampleRate, COTime)
            {
                RayCount = RCT;
            }

            //public Expanding_Receiver(Spherical_Receiver Rec)
            //    : base(Rec, )
            //{
            //}

            public Expanding_Receiver(Hare.Geometry.Point Point, Hare.Geometry.Point SrcPoint, Scene room, int RCT, double Speed_of_Sound, int SampleRate_in, double COTime_in)
                :base(Point, SrcPoint, room, Speed_of_Sound,SampleRate_in,COTime_in)
            {
                RayCount = RCT;
            }

            /// <summary>
            /// Checks receiver for an intersection with a ray 
            /// </summary>
            /// <param name="Length">The length of the ray at the reflection point before potential intersection</param>
            /// <param name="R">the ray.</param>
            /// <param name="EndPt">The point at which the ray intersects the model after potential receiver intersection.</param>
            public override void CheckRay(OctaveRay R, Hare.Geometry.Point EndPt)
            {
                double Radius = (R.t_sum * C_Sound) * Math.Sqrt(6.28 / (RayCount));
                double Radius2 = Radius * Radius;
                Vector m = R.origin - H_Origin;
                double RayLength = (EndPt - R.origin).Length();
                Vector d = (EndPt - R.origin) / RayLength;
                double b = Hare_math.Dot(m, d);
                double c = Hare_math.Dot(m, m) - (Radius2);
                if (c > 0 && b > 0) return;
                double discr = b * b - c;
                if (discr < 0) return;
                double t1 = -b - Math.Sqrt(discr);
                double t2 = -b + Math.Sqrt(discr);
                double t = (t1 + t2) * .5;

                if (t > 0 && t * t < SqDistance(EndPt, R.origin))
                {
                    double Raydist = t * Inv_C_Sound + R.t_sum;
                    double Ei = R.Intensity * Math.Pow(10,-.1 * Atten[R.Octave] * t) / (Math.PI * Radius2);
                    Recs.Add(Raydist, Ei, R.direction * -1, R.phase,Rho_C, R.Octave);
                }
                return;
            }

            /// <summary>
            /// This method checks a receiver for a ray using a Broadband Ray (these typically occur before rays are split in the Raytracing simulation.
            /// </summary>
            /// <param name="Length">The length of the ray at the reflection point before potential intersection</param>
            /// <param name="R">the ray.</param>
            /// <param name="EndPt">The point at which the ray intersects the model after potential receiver intersection.</param>
            public override void CheckBroadbandRay(BroadRay R, Hare.Geometry.Point EndPt)
            {
                double Radius = (R.t_sum * C_Sound) * Math.Sqrt(6.28 / (RayCount));
                double Radius2 = Radius * Radius;
                Vector m = R.origin - H_Origin;
                double RayLength = (EndPt - R.origin).Length();
                Vector d = (EndPt - R.origin) / RayLength;
                double b = Hare_math.Dot(m, d);
                double c = Hare_math.Dot(m, m) - (Radius2);
                if (c > 0 && b > 0) return;
                double discr = b * b - c;
                if (discr < 0) return;
                double t1 = -b - Math.Sqrt(discr);
                double t2 = -b + Math.Sqrt(discr);
                double t = (t1 + t2) * .5;
                if (t > 0 && t * t < SqDistance(EndPt, R.origin))
                {
                    double t_ACC = Math.Abs(t1 - t2);
                    double RayTime = t * Inv_C_Sound + R.t_sum;
                    Vector Dir = R.direction * -1;
                    double Area = Math.PI * Radius2;

                    Recs.Add(RayTime, (R.Energy[0] * Math.Pow(10,-.1 * Atten[0] * t) / Area), Dir, R.phase[0], Rho_C, 0);
                    Recs.Add(RayTime, (R.Energy[1] * Math.Pow(10,-.1 * Atten[1] * t) / Area), Dir, R.phase[1], Rho_C, 1);
                    Recs.Add(RayTime, (R.Energy[2] * Math.Pow(10,-.1 * Atten[2] * t) / Area), Dir, R.phase[2], Rho_C, 2);
                    Recs.Add(RayTime, (R.Energy[3] * Math.Pow(10,-.1 * Atten[3] * t) / Area), Dir, R.phase[3], Rho_C, 3);
                    Recs.Add(RayTime, (R.Energy[4] * Math.Pow(10,-.1 * Atten[4] * t) / Area), Dir, R.phase[4], Rho_C, 4);
                    Recs.Add(RayTime, (R.Energy[5] * Math.Pow(10,-.1 * Atten[5] * t) / Area), Dir, R.phase[5], Rho_C, 5);
                    Recs.Add(RayTime, (R.Energy[6] * Math.Pow(10,-.1 * Atten[6] * t) / Area), Dir, R.phase[6], Rho_C, 6);
                    Recs.Add(RayTime, (R.Energy[7] * Math.Pow(10,-.1 * Atten[7] * t) / Area), Dir, R.phase[7], Rho_C, 7);   
                }
                return;
            }

            /// <summary>
            /// This method checks a receiver for an intersection with a ray.
            /// </summary>
            /// <param name="R">the ray.</param>
            /// <param name="EndPt">The point at which the ray intersects the model after potential receiver intersection.</param>
            /// <param name="Length">The length of the ray at the reflection point before potential intersection</param>
            /// <returns>true if intersects, false if not.</returns>
            public override bool SimpleCheck(BroadRay R, Hare.Geometry.Point EndPt)
            {
                double Radius = R.t_sum * Math.Sqrt(6.28 / RayCount);
                Vector m = R.origin - H_Origin;
                double b = Hare_math.Dot(m, R.direction);
                double c = Hare_math.Dot(m, m) - (Radius * Radius);
                if (c > 0 && b > 0) return false;
                double discr = b * b - c;
                if (discr < 0) return false;
                double t = (-b - Math.Sqrt(discr)) + (-b + Math.Sqrt(discr)) / 2;
                if (t * t < SqDistance(EndPt, R.origin)) return true;
                return false;
            }
        }
    }
}