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

using System.Collections.Generic;
using System;
using Rhino.Geometry;
using Pachyderm_Acoustic.Environment;
using System.Linq;
using Hare;

namespace Pachyderm_Acoustic
{
    namespace Utilities
    {
        public static partial class Numerics
        {
            public static double rt2 = Math.Sqrt(2);
            public static double PiX2 = 2 * Math.PI;
            public static double Pi_180 = Math.PI / 180;
            public static double[] angularFrequency = new double[] { 62.5 * PiX2, 125 * PiX2, 250 * PiX2, 500 * PiX2, 1000 * PiX2, 2000 * PiX2, 4000 * PiX2, 8000 * PiX2 };

            public enum ComplexComponent
            {
                Real,
                Imaginary,
                Magnitude
            }

            public static void ExpComplex(float re, float im, out float re_out, out float im_out)
            {
                float exp = (float)Math.Exp(re);
                re_out = exp * (float)Math.Cos(im);
                im_out = exp * (float)Math.Sin(im);
            }

            public static void ExpComplex(double re, double im, out double re_out, out double im_out)
            {
                float exp = (float)Math.Exp(re);
                re_out = exp * (float)Math.Cos(im);
                im_out = exp * (float)Math.Sin(im);
            }

            public static float Abs(float re, float im)
            {
                return (float)Math.Sqrt((re * re) + (im * im));
            }

            public static double Abs(double re, double im)
            {
                return Math.Sqrt((re * re) + (im * im));
            }

            public static System.Numerics.Complex jBessel(int order, System.Numerics.Complex X)
            {
                //if (order < 1) throw new NotFiniteNumberException();
                //Asymptotic Solution
                //Get Angle
                double Arg = X.Phase;
                //double Arg = Math.Atan2(X.Imaginary, X.Real);                 
                int asy_sign = (Arg >= 0) ? -1 : 1;

                return (1 / System.Numerics.Complex.Sqrt(2 * Math.PI * X)) * System.Numerics.Complex.Exp(asy_sign * System.Numerics.Complex.ImaginaryOne * (X - order * Math.PI / 2 - Math.PI / 4));

                //System.Numerics.Complex J;
                //int ntmfact;
                //if (order == 0)
                //{ 
                //    J = 1; 
                //    ntmfact = 1; 
                //}
                //else if (order == 1)
                //{ 
                //    J = X; 
                //    ntmfact = 2; 
                //}
                //else
                //{
                //    ntmfact = 1;
                //    for (int i = 1; i <= order; i++) ntmfact *= i;
                //    System.Numerics.Complex.Pow(X, order);
                //}
                //System.Numerics.Complex series_sum = - X * X *.25;

                //int mfact = 1;
                //int t1 = 1;

                //for (int m = 1; m < 1000; m++)
                //{
                //    t1 *= -1;
                //    mfact *= m;
                //    ntmfact *= (m + order);

                //    series_sum += t1 * System.Numerics.Complex.Pow(X, 2 * m) / (Math.Pow(2, 2 * m + order) * mfact * ntmfact);
                //}

                //return series_sum * System.Numerics.Complex.Pow(X, order);

                //System.Numerics.Complex z, zproduct, zanswer, zarg;
                //double k;
                //int i;
                //if (order > 2 || order < 0) throw new Exception("Order of Bessel function of first kind must be 0, 1 or 2");
                //z = System.Numerics.Complex.One;
                //zproduct = System.Numerics.Complex.One;
                //zanswer = System.Numerics.Complex.One;
                //zarg = -0.25 * (X * X);

                //for (i = 0; i < 1000; i++)
                //{
                //    k = (i + 1) * (i + 1 + order);
                //    z = (1 / (k)) * (z * zarg);
                //    if (System.Numerics.Complex.Abs(z) < 1e-20) break;
                //    zanswer = zanswer + z;
                //}

                //for (i = 0; i < order; i++) zproduct = zproduct * 0.5 * X;
                //zanswer = zanswer * zproduct;
                //return zanswer;

                //if (order > 2 || order < 0) throw new Exception("Order of greater than 2, or less than zero not allowed");

                //System.Numerics.Complex z = 1, z_final = 1, z_int = -0.25 * (X * X);;

                //for(int i = 0; i < 1000; i++)
                //{
                //    z *= z_int / ((i + 1)*(i + 1 + order));
                //    if ((z.Real*z.Real + z.Imaginary * z.Imaginary) < 1e-22) break;
                //    z_final += z;
                //}

                //for(int i = 0; i < order; i++) z_final *= 0.5 * X;

                //return z_final;
            }
        }

        ///<summary>
        /// Simple Mathematical tools to estimate certain acoustical parameters...
        ///</summary>
        public class AcousticalMath
        {
                /// <summary>
                /// Calculate Sound Pressure Level (in dB) from intensity/energy.
                /// </summary>
                /// <param name="Intensity"></param>
                /// <returns></returns>
                public static double SPL_Intensity(double Intensity)
                {
                    double SPL = 10 * Math.Log10(Intensity / 1E-12);
                    if (SPL < 0) return 0;
                    return SPL;
                }

                public static double SPL_Pressure(double Pressure)
                {
                    return 20 * Math.Log10(Math.Abs(Pressure) / 2E-5);
                }

                /// <summary>
                /// Calculate Sound Pressure Level (in dB) from intensity/energy for entire signal.
                /// </summary>
                /// <param name="Intensity"></param>
                /// <returns></returns>
                public static double[] SPL_Intensity_Signal(double[] Intensity)
                {
                    for (int i = 0; i < Intensity.Length; i++)
                    {
                        Intensity[i] = SPL_Intensity(Intensity[i]);
                    }
                    return Intensity;
                }

                public static double[] SPL_Pressure_Signal(double[] P)
                {
                    double[] SPL = new double[P.Length];

                    for (int i = 0; i < P.Length; i++)
                    {
                        SPL[i] = SPL_Pressure(P[i]);
                    }
                    return SPL;
                }
                
                /// <summary>
                /// Calculates SPL-time curve from simulation output.
                /// </summary>
                /// <param name="Direct"></param>
                /// <param name="ISData"></param>
                /// <param name="RTData"></param>
                /// <param name="CO_Time"></param>
                /// <param name="samplerate"></param>
                /// <param name="Octave">the chosen octave band.</param>
                /// <param name="Rec_ID">the id of the selected receiver.</param>
                /// <returns></returns>
                public static double[] SPLTCurve_Intensity(Direct_Sound[] Direct, ImageSourceData[] ISData, Environment.Receiver_Bank[] RTData, double CO_Time, int samplerate, int Octave, int Rec_ID, int SrcID, bool Start_at_Zero)
                {
                    double[] Energy = ETCurve(Direct, ISData, RTData, CO_Time, samplerate, Octave, Rec_ID, SrcID, Start_at_Zero);
                    double[] SPL = new double[Energy.Length];

                    for (int i = 0; i < Energy.Length; i++)
                    {
                        SPL[i] = SPL_Intensity(Energy[i]);
                    }
                    return SPL;
                }

                public static double[] SPLTCurve_Pressure(Direct_Sound[] Direct, ImageSourceData[] ISData, Environment.Receiver_Bank[] RTData, double CO_Time, int samplerate, int Octave, int Rec_ID, int SrcID, bool Start_at_Zero)
                {
                    double[] P;//, Imag;
                    //double[] Pressure = PTCurve(Direct, ISData, RTData, CO_Time, samplerate, Octave, Rec_ID, SrcID, Start_at_Zero, Numerics.ComplexComponent.Magnitude);
                    PTCurve(Direct, ISData, RTData, CO_Time, samplerate, Rec_ID, SrcID, Start_at_Zero, out P);//, out Imag);
                    float[] Pressure = new float[P.Length];
                    //for (int i = 0; i < Pressure.Length; i++) Pressure[i] = Numerics.Abs(Real[i], Imag[i]);

                    double[] SPL = new double[Pressure.Length];

                    for (int i = 0; i < Pressure.Length; i++)
                    {
                        SPL[i] = SPL_Pressure(Pressure[i]);
                    }
                    return SPL;
                }

                public static double[] SPLTCurve_Intensity(Direct_Sound Direct, ImageSourceData ISData, Environment.Receiver_Bank RTData, double CO_Time, int samplerate, int Octave, int Rec_ID, bool StartAtZero)
                {
                    Direct_Sound[] ArrDirect = new Direct_Sound[1];
                    ArrDirect[0] = Direct;
                    ImageSourceData[] ArrIS = new ImageSourceData[1];
                    ArrIS[0] = ISData;
                    Environment.Receiver_Bank[] ArrRT = new Environment.Receiver_Bank[1];
                    ArrRT[0] = RTData;
                    return SPLTCurve_Intensity(ArrDirect, ArrIS, ArrRT, CO_Time, samplerate, Octave, Rec_ID, 0, StartAtZero);
                }

                public static double[] SPLTCurve_Pressure(Direct_Sound Direct, ImageSourceData ISData, Environment.Receiver_Bank RTData, double CO_Time, int samplerate, int Octave, int Rec_ID, bool StartAtZero)
                {
                    Direct_Sound[] ArrDirect = new Direct_Sound[1];
                    ArrDirect[0] = Direct;
                    ImageSourceData[] ArrIS = new ImageSourceData[1];
                    ArrIS[0] = ISData;
                    Environment.Receiver_Bank[] ArrRT = new Environment.Receiver_Bank[1];
                    ArrRT[0] = RTData;
                    return SPLTCurve_Pressure(ArrDirect, ArrIS, ArrRT, CO_Time, samplerate, Octave, Rec_ID, 0, StartAtZero);
                }

                public static double[] ETCurve(Direct_Sound[] Direct, ImageSourceData[] ISData, Environment.Receiver_Bank[] RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, List<int> SrcIDs, bool StartAtZero)
                {
                    //TODO: Make provisions for specifying source delays...
                    //double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency)];

                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[] IR = ETCurve(Direct, ISData, RTData, CO_Time, Sampling_Frequency, Octave, Rec_ID, s, StartAtZero);
                        //Array.Resize(ref IR, IR.Length + (int)Math.Ceiling(maxdelay));
                        //if (IR.Length > Histogram.Length) Array.Resize(ref Histogram, IR.Length);
                        for (int i = 0; i < IR.Length; i++)
                        {
                            Histogram[i + (int)Math.Ceiling(Direct[s].Delay_ms / 1000 * Sampling_Frequency)] += IR[i];
                        }
                    }
                    return Histogram;
                }

                public static double[] PTCurve(Direct_Sound[] Direct, ImageSourceData[] ISData, Environment.Receiver_Bank[] RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, List<int> SrcIDs, bool StartAtZero)
                {
                    //TODO: Make provisions for specifying source delays...
                    //double[] Ptotal = new double[(int)(CO_Time * Sampling_Frequency)];
                    //double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency)];

                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[] P;
                        PTCurve(Direct, ISData, RTData, CO_Time, Sampling_Frequency, Rec_ID, s, StartAtZero, out P);
                        //Array.Resize(ref P, P.Length + (int)Math.Ceiling(maxdelay));
                        //if (P.Length > Ptotal.Length) Array.Resize(ref Ptotal, P.Length);
                        for (int i = 0; i < P.Length; i++)
                        {
                            Histogram[i + (int)Math.Ceiling(Direct[s].Delay_ms /1000 * Sampling_Frequency)] += P[i];
                        }
                    }
                    return Histogram;
                }

                public static double[] ETCurve(Direct_Sound Direct, ImageSourceData ISData, Environment.Receiver_Bank RTData, double CO_Time, int samplerate, int Octave, int Rec_ID, bool StartAtZero)
                {
                    Direct_Sound[] ArrDirect = new Direct_Sound[1];
                    ArrDirect[0] = Direct;
                    ImageSourceData[] ArrIS = new ImageSourceData[1];
                    ArrIS[0] = ISData;
                    Environment.Receiver_Bank[] ArrRT = new Environment.Receiver_Bank[1];
                    ArrRT[0] = RTData;
                    return ETCurve(ArrDirect, ArrIS, ArrRT, CO_Time, samplerate, Octave, Rec_ID, 0, StartAtZero);
                }

                public static double[] PTCurve(Direct_Sound Direct, ImageSourceData ISData, Environment.Receiver_Bank RTData, double CO_Time, int samplerate, int Octave, int Rec_ID, bool StartAtZero, Numerics.ComplexComponent Output_Type)
                {
                    Direct_Sound[] ArrDirect = new Direct_Sound[1];
                    ArrDirect[0] = Direct;
                    ImageSourceData[] ArrIS = new ImageSourceData[1];
                    ArrIS[0] = ISData;
                    Environment.Receiver_Bank[] ArrRT = new Environment.Receiver_Bank[1];
                    ArrRT[0] = RTData;

                    double[] P;
                    PTCurve(ArrDirect, ArrIS, ArrRT, CO_Time, samplerate, Rec_ID, 0, StartAtZero, out P);
                    
                    double[] Pressure = new double[P.Length];
                    for (int i = 0; i < Pressure.Length; i++) Pressure[i] = P[i];
                    return Pressure;
                }

                /// <summary>
                /// Calculates Energy-time curve from simulation output.
                /// </summary>
                /// <param name="Direct"></param>
                /// <param name="ISData"></param>
                /// <param name="RTData"></param>
                /// <param name="CO_Time"></param>
                /// <param name="Sampling_Frequency"></param>
                /// <param name="Octave">the chosen octave band.</param>
                /// <param name="Rec_ID">the id of the selected receiver.</param>
                /// <returns></returns>
                public static double[] ETCurve(Direct_Sound[] Direct, ImageSourceData[] ISData, Environment.Receiver_Bank[] RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, int Src_ID, bool Start_at_Zero)
                {
                    double[] Histogram = null;
                    if (RTData[Src_ID] != null)
                    {
                        Histogram = RTData[Src_ID].GetEnergyHistogram(Octave, Rec_ID);
                    }
                    else
                    {
                        Histogram = new double[(int)(CO_Time * Sampling_Frequency)];
                    }

                    if (Direct[Src_ID] != null && Direct[Src_ID].IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct[Src_ID].Time(Rec_ID) * Sampling_Frequency);
                        for (int i = 0; i < Direct[Src_ID].Io[Rec_ID].GetLength(0); i++)
                        {
                            double DirectValue = 0;
                            switch (Octave)
                            {
                                case 8:
                                    DirectValue = Direct[Src_ID].EnergySum(Rec_ID,i);
                                    break;
                                default:
                                    DirectValue = Direct[Src_ID].EnergyValue(Octave, Rec_ID)[i];
                                    break;
                            }
                            Histogram[D_Start + i] += DirectValue;
                        }
                    }

                    if (ISData[Src_ID] != null)
                    {
                        switch (Octave)
                        {
                            case 8:
                                foreach (Deterministic_Reflection value in ISData[Src_ID].Paths[Rec_ID])
                                {
                                    if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram.Length - 1)
                                    {
                                        for (int oct = 0; oct < 8; oct++)
                                        {
                                            double[] e = value.Energy(oct);
                                            for (int t = 0; t < e.Length; t++) Histogram[(int)Math.Ceiling(Sampling_Frequency * value.TravelTime) + t] += e[t];
                                        }
                                    }
                                }
                                break;
                            default:
                                foreach (Deterministic_Reflection value in ISData[Src_ID].Paths[Rec_ID])
                                {
                                    if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram.Length - 1)
                                    {
                                        double[] e = value.Energy(Octave);
                                        for(int t = 0; t < e.Length; t++) Histogram[(int)Math.Ceiling(Sampling_Frequency * value.TravelTime) + t] += e[t];
                                    }
                                }
                                break;
                        }
                    }
                    return Histogram;
                }

                /// <summary>
                /// Calculates pressure impulse response from simulation output.
                /// </summary>
                /// <param name="Direct"></param>
                /// <param name="ISData"></param>
                /// <param name="RTData"></param>
                /// <param name="CO_Time"></param>
                /// <param name="Sampling_Frequency"></param>
                /// <param name="Octave">the chosen octave band.</param>
                /// <param name="Rec_ID">the id of the selected receiver.</param>
                /// <returns></returns>
                public static void PTCurve(Direct_Sound[] Direct, ImageSourceData[] ISData, Environment.Receiver_Bank[] RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, int Src_ID, bool Start_at_Zero, out double[] P)
                {
                    if (RTData[Src_ID] != null)
                    {
                        RTData[Src_ID].GetPressure(Rec_ID, out P);//8,
                    }
                    else
                    {
                        P = new double[(int)(CO_Time * Sampling_Frequency)];
                    }

                    if (Direct[Src_ID] != null && Direct[Src_ID].IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct[Src_ID].Time(Rec_ID) * Sampling_Frequency);
                            
                        for (int i = 0; i < Direct[Src_ID].P[Rec_ID].GetLength(0); i++)
                        {
                            P[D_Start + i] += Direct[Src_ID].P[Rec_ID][i];
                        }
                    }

                    if (ISData[Src_ID] != null)
                    {
                        foreach (Deterministic_Reflection value in ISData[Src_ID].Paths[Rec_ID])
                        {
                            if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < P.Length - 1)
                            {
                                int end = value.Pressure.Length < P.Length - (int)Math.Ceiling(Sampling_Frequency * value.TravelTime) ? value.Pressure.Length : P.Length - (int)Math.Ceiling(Sampling_Frequency * value.TravelTime); 
                                for (int t = 0; t < end; t++) P[(int)Math.Ceiling(Sampling_Frequency * value.TravelTime) + t] += (float)value.Pressure[t];
                            }
                        }
                    }
                }

                public static double[][] ETCurve_1d(IEnumerable<Direct_Sound> Direct, IEnumerable<ImageSourceData> ISData, IEnumerable<Environment.Receiver_Bank> RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, List<int> SrcIDs, bool StartAtZero, double alt, double azi, bool degrees)
                {
                    //TODO: Make provisions for specifying source delays...
                    double[][] Histogram = new double[3][];
                    //Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency)];

                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[][] IR = ETCurve_1d(Direct.ElementAt<Direct_Sound>(s), ISData.ElementAt<ImageSourceData>(s), RTData.ElementAt<Receiver_Bank>(s), CO_Time, Sampling_Frequency, Octave, Rec_ID, StartAtZero, alt, azi, degrees);
                        //Array.Resize(ref IR, IR.Length + (int)Math.Ceiling(maxdelay));
                        //if (IR.Length > Histogram.Length) Array.Resize(ref Histogram, IR.Length);
                        for (int d = 0; d < 3; d++)
                        {
                            for (int i = 0; i < IR[0].Length; i++)
                            {
                                Histogram[d][i + (int)Math.Ceiling(Direct.ElementAt<Direct_Sound>(s).Delay_ms / 1000 * Sampling_Frequency)] += IR[d][i];
                            }
                        }
                    }
                    return Histogram;
                }

                public static double[][] ETCurve_1d(Direct_Sound Direct, ImageSourceData ISData, Receiver_Bank RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, bool Start_at_Zero, double alt, double azi, bool degrees)
                {
                    double[][] Histogram = new double[3][];
                    Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency)];
                    Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency)];
                    Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency)];
                    if (RTData != null)
                    {
                        for (int i = 0; i < Histogram[0].Length; i++)
                        {
                            Hare.Geometry.Vector Vpos = RTData.Directions_Pos(Octave, i, Rec_ID, alt, azi, degrees);
                            Hare.Geometry.Vector Vneg = RTData.Directions_Neg(Octave, i, Rec_ID, alt, azi, degrees);

                            double E = RTData.Rec_List[Rec_ID].Energy(i, Octave);
                            Hare.Geometry.Vector VTot = new Hare.Geometry.Vector(Math.Abs(Vpos.x) - Math.Abs(Vneg.x), Math.Abs(Vpos.y) - Math.Abs(Vneg.y), Math.Abs(Vpos.z) - Math.Abs(Vneg.z));
                            VTot.Normalize();
                            VTot *= E;

                            Histogram[0][i] = VTot.x;
                            Histogram[1][i] = VTot.y;
                            Histogram[2][i] = VTot.z;
                        }
                    }

                    if (Direct != null && Direct.IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct.Time(Rec_ID) * Sampling_Frequency);

                        Hare.Geometry.Vector[] DirectValue;
                        switch (Octave)
                        {
                            case 8:
                                DirectValue = Direct.Dir_Energy_Sum(Rec_ID, alt, azi, degrees);
                                break;
                            default:
                                DirectValue = Direct.Dir_Energy(Octave, Rec_ID, alt, azi, degrees);
                                break;
                        }

                        for (int i = 0; i < DirectValue.Length; i++)
                        {
                            Histogram[0][D_Start + i] += Math.Abs(DirectValue[i].x);
                            Histogram[1][D_Start + i] += Math.Abs(DirectValue[i].y);
                            Histogram[2][D_Start + i] += Math.Abs(DirectValue[i].z);
                        }
                    }

                    if (ISData != null)
                    {
                        switch (Octave)
                        {
                            case 8:
                                foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                                {
                                    if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram[0].Length - 1)
                                    {
                                        Hare.Geometry.Vector[] E_Sum = value.Dir_EnergySum(alt, azi, degrees);
                                        for (int i = 0; i < E_Sum.Length; i++)
                                        {
                                            Histogram[0][(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i / Sampling_Frequency)] += Math.Abs(E_Sum[i].x);
                                            Histogram[1][(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i / Sampling_Frequency)] += Math.Abs(E_Sum[i].y);
                                            Histogram[2][(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i / Sampling_Frequency)] += Math.Abs(E_Sum[i].z);
                                        }
                                    }
                                }
                                break;
                            default:
                                foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                                {
                                    if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram[0].Length - 1)
                                    {
                                        Hare.Geometry.Vector[] E_Dir = value.Dir_Energy(Octave, alt, azi, degrees);
                                        for (int i = 0; i < E_Dir.Length; i++)
                                        {
                                            Histogram[0][(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i / Sampling_Frequency)] += Math.Abs(E_Dir[i].x);
                                            Histogram[1][(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i / Sampling_Frequency)] += Math.Abs(E_Dir[i].y);
                                            Histogram[2][(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i / Sampling_Frequency)] += Math.Abs(E_Dir[i].z);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    return Histogram;
                }

                public static double[] ETCurve_Directional(IEnumerable<Direct_Sound> Direct, IEnumerable<ImageSourceData> ISData, IEnumerable<Environment.Receiver_Bank> RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, List<int> SrcIDs, bool StartAtZero, double alt, double azi, bool degrees)
                {
                    //TODO: Make provisions for specifying source delays...
                    //double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency)];

                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[] IR = ETCurve_Directional(Direct.ElementAt<Direct_Sound>(s), ISData.ElementAt<ImageSourceData>(s), RTData.ElementAt<Receiver_Bank>(s), CO_Time, Sampling_Frequency, Octave, Rec_ID, StartAtZero, alt, azi, degrees);
                        //Array.Resize(ref IR, IR.Length + (int)Math.Ceiling(maxdelay));
                        //if (IR.Length > Histogram.Length) Array.Resize(ref Histogram, IR.Length);
                        for (int i = 0; i < IR.Length; i++)
                        {
                            Histogram[i + (int)Math.Ceiling(Direct.ElementAt<Direct_Sound>(s).Delay_ms / 1000 * Sampling_Frequency)] += IR[i];
                        }
                    }
                    return Histogram;
                }

                public static double[] ETCurve_Directional(Direct_Sound Direct, ImageSourceData ISData, Receiver_Bank RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, bool Start_at_Zero, double alt, double azi, bool degrees)
                {
                    double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency)];
                    if (RTData != null)
                    {
                        for (int i = 0; i < Histogram.Length; i++)
                        {
                            Hare.Geometry.Vector Vpos = RTData.Directions_Pos(Octave, i, Rec_ID, alt, azi, degrees);
                            Hare.Geometry.Vector Vneg = RTData.Directions_Neg(Octave, i, Rec_ID, alt, azi, degrees);

                            double E = RTData.Rec_List[Rec_ID].Energy(i, Octave);
                            Hare.Geometry.Vector VTot = new Hare.Geometry.Vector(Math.Abs(Vpos.x) - Math.Abs(Vneg.x), Math.Abs(Vpos.y) - Math.Abs(Vneg.y), Math.Abs(Vpos.z) - Math.Abs(Vneg.z));

                            if (Vpos.x > 0)
                            {
                                Histogram[i] += Math.Abs(Vpos.x);
                            }
                            if (Vneg.x > 0)
                            {
                                Histogram[i] += Math.Abs(Vneg.x);
                            }

                            double L = VTot.Length();
                            if (L > 0) Histogram[i] *= E / L;

                            if (AcousticalMath.SPL_Intensity(Histogram[i]) > 200)
                            {
                                Rhino.RhinoApp.Write("Super high SPLs... what's going on, man?");
                            }
                        }
                    }

                    if (Direct != null && Direct.IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct.Time(Rec_ID) * Sampling_Frequency);

                        Hare.Geometry.Vector[] DirectValue;
                        switch (Octave)
                        {
                            case 8:
                                DirectValue = Direct.Dir_Energy_Sum(Rec_ID, alt, azi, degrees);
                                break;
                            default:
                                DirectValue = Direct.Dir_Energy(Octave, Rec_ID, alt, azi, degrees);
                                break;
                        }

                        for (int i = 0; i < DirectValue.Length; i++)
                        {
                            if (DirectValue[i].x > 0) Histogram[D_Start + i] += Math.Abs(DirectValue[i].x);
                        }
                    }

                    if (ISData != null)
                    {
                        switch (Octave)
                        {
                            case 8:
                                foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                                {
                                    if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram.Length - 1)
                                    {
                                        Hare.Geometry.Vector[] E_Sum = value.Dir_EnergySum(alt, azi, degrees);
                                        for (int i = 0; i < E_Sum.Length; i++)
                                        {
                                            if (E_Sum[i].x > 0) Histogram[(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i)] += Math.Abs(E_Sum[i].x);
                                        }
                                    }
                                }
                                break;
                            default:
                                foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                                {
                                    if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram.Length - 1)
                                    {
                                        Hare.Geometry.Vector[] E_Dir = value.Dir_Energy(Octave, alt, azi, degrees);
                                        for (int i = 0; i < E_Dir.Length; i++)
                                        {
                                            if (E_Dir[i].x > 0) Histogram[(int)Math.Ceiling(Sampling_Frequency * value.TravelTime + i)] += Math.Abs(E_Dir[i].x);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    return Histogram;
                }

                public static double[][] PTCurve_Ambisonics2(IEnumerable<Direct_Sound> Direct, IEnumerable<ImageSourceData> ISData, IEnumerable<Environment.Receiver_Bank> RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, List<int> SrcIDs, bool StartAtZero, double alt, double azi, bool degrees)
                {
                    //TODO: Make provisions for specifying source delays...
                    double[][] Histogram = new double[5][];
                    //Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[3] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[4] = new double[(int)(CO_Time * Sampling_Frequency)];

                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[3] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[4] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[][] IR = PTCurve_Ambisonics2(Direct.ElementAt<Direct_Sound>(s), ISData.ElementAt<ImageSourceData>(s), RTData.ElementAt<Receiver_Bank>(s), CO_Time, Sampling_Frequency, Rec_ID, StartAtZero, alt, azi, degrees);
                        //Array.Resize(ref IR, IR.Length + (int)Math.Ceiling(maxdelay));
                        //if (IR.Length > Histogram.Length) Array.Resize(ref Histogram, IR.Length);
                        for (int d = 0; d < IR.Length; d++)
                        {
                            for (int i = 0; i < IR[0].Length; i++)
                            {
                                Histogram[d][i + (int)Math.Ceiling(Direct.ElementAt<Direct_Sound>(s).Delay_ms / 1000 * Sampling_Frequency)] += IR[d][i];
                            }
                        }
                    }
                    return Histogram;
                }

                public static double[][] PTCurve_Ambisonics3(IEnumerable<Direct_Sound> Direct, IEnumerable<ImageSourceData> ISData, IEnumerable<Environment.Receiver_Bank> RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, List<int> SrcIDs, bool StartAtZero, double alt, double azi, bool degrees)
                {
                    //TODO: Make provisions for specifying source delays...
                    double[][] Histogram = new double[7][];
                    //Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[3] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[4] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[5] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[6] = new double[(int)(CO_Time * Sampling_Frequency)];

                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[3] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[4] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[5] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[6] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[][] IR = PTCurve_Ambisonics3(Direct.ElementAt<Direct_Sound>(s), ISData.ElementAt<ImageSourceData>(s), RTData.ElementAt<Receiver_Bank>(s), CO_Time, Sampling_Frequency, Rec_ID, StartAtZero, alt, azi, degrees);
                        //Array.Resize(ref IR, IR.Length + (int)Math.Ceiling(maxdelay));
                        //if (IR.Length > Histogram.Length) Array.Resize(ref Histogram, IR.Length);
                        for (int d = 0; d < IR.Length; d++)
                        {
                            for (int i = 0; i < IR[0].Length; i++)
                            {
                                Histogram[d][i + (int)Math.Ceiling(Direct.ElementAt<Direct_Sound>(s).Delay_ms / 1000 * Sampling_Frequency)] += IR[d][i];
                            }
                        }
                    }

                    return Histogram;
                }


            /// <summary>
            /// Approximation of the 5 second order ambisonics channels. (Fig8 3Axis and omni are the first four).
            /// </summary>
            /// <param name="Direct"></param>
            /// <param name="ISData"></param>
            /// <param name="RTData"></param>
            /// <param name="CO_Time"></param>
            /// <param name="Sampling_Frequency"></param>
            /// <param name="Rec_ID"></param>
            /// <param name="Start_at_Zero"></param>
            /// <param name="xpos_alt"></param>
            /// <param name="xpos_azi"></param>
            /// <param name="degrees"></param>
            /// <returns></returns>

                public static double[][] PTCurve_Ambisonics2(Direct_Sound Direct, ImageSourceData ISData, Receiver_Bank RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, bool Start_at_Zero, double xpos_alt, double xpos_azi, bool degrees)
                {
                    double[][] Histogram = new double[5][];
                    if (RTData != null)
                    {
                        double[][] hist_temp = RTData.Pressure_3Axis(Rec_ID);
                        Histogram[0] = new double[hist_temp[0].Length];
                        Histogram[1] = new double[hist_temp[0].Length];
                        Histogram[2] = new double[hist_temp[0].Length];
                        Histogram[3] = new double[hist_temp[0].Length];
                        Histogram[4] = new double[hist_temp[0].Length];
                        for (int i = 0; i < hist_temp[0].Length; i++)
                        {
                            Hare.Geometry.Vector Vpos = PachTools.Rotate_Vector(PachTools.Rotate_Vector(new Hare.Geometry.Vector(hist_temp[0][i], hist_temp[2][i], hist_temp[4][i]), xpos_azi, 0, true), 0, xpos_alt, true);
                            Hare.Geometry.Vector Vneg = PachTools.Rotate_Vector(PachTools.Rotate_Vector(new Hare.Geometry.Vector(-hist_temp[1][i], - hist_temp[3][i], - hist_temp[5][i]), xpos_azi, 0, true), 0, xpos_alt, true);
                            double magpos = Math.Sqrt(Vpos.x * Vpos.x + Vpos.y * Vpos.y + Vpos.z * Vpos.z);
                            double magneg = Math.Sqrt(Vneg.x * Vneg.x + Vneg.y * Vneg.y + Vneg.z * Vneg.z);
                            double phipos = Math.Asin(Vpos.z / magpos);
                            double phineg = Math.Asin(Vneg.z / magneg);
                            double thetapos = Math.Asin(Vpos.y / magpos / Math.Cos(phipos));
                            double thetaneg = Math.Asin(Vneg.y / magneg / Math.Cos(phineg));
                            double rt3_2 = Math.Sqrt(3)/2;
                            
                            double sin2phpos = Math.Sin(2 * phipos);
                            double sin2phneg = Math.Sin(2 * phineg);
                            double cossqphpos = Math.Cos(phipos) * Math.Cos(phipos);
                            double cossqphneg = Math.Cos(phineg) * Math.Cos(phineg);

                            Histogram[0][i] = magpos * (3 * (Math.Sin(phipos) * Math.Sin(phipos) - 1) / 2 + magneg * 3 * Math.Sin(phineg) * Math.Sin(phineg) - 1) / 2;
                            Histogram[1][i] = rt3_2 * (Math.Cos(thetapos) * sin2phpos * magpos + Math.Cos(thetaneg) * sin2phneg * magneg);
                            Histogram[2][i] = rt3_2 * (Math.Sin(thetapos) * sin2phpos * magpos + Math.Sin(thetaneg) * sin2phneg * magneg); 
                            Histogram[3][i] = rt3_2 * (Math.Cos(2 * thetapos) * cossqphpos * magpos + Math.Cos(2 * thetaneg) * cossqphneg * magneg);
                            Histogram[4][i] = rt3_2 * (Math.Sin(2 * thetapos) * cossqphpos * magpos + Math.Sin(2 * thetaneg) * cossqphneg * magneg);
                        }
                    }
                    else
                    {
                        Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[4] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[5] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                    }

                    if (Direct != null && Direct.IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct.Time(Rec_ID) * Sampling_Frequency);

                        double[][] V = Direct.Dir_Pressure(Rec_ID, xpos_alt, xpos_azi, degrees, true);
                        for (int i = 0; i < V.Length; i++)
                        {
                            double mag = Math.Sqrt(V[i][0] * V[i][0] + V[i][1] * V[i][1] + V[i][2] * V[i][2]);
                            double phi = Math.Asin(V[i][2] / mag);
                            double theta = Math.Asin(V[i][1] / mag / Math.Cos(phi));
                            double rt3_2 = Math.Sqrt(3) / 2;

                            double sin2phi = Math.Sin(2 * phi);
                            double cossqphi = Math.Cos(phi) * Math.Cos(phi);

                            Histogram[0][i + D_Start] += mag * 3 * (Math.Sin(phi) * Math.Sin(phi) - 1) / 2;
                            Histogram[1][i + D_Start] += rt3_2 * Math.Cos(theta) * sin2phi * mag;
                            Histogram[2][i + D_Start] += rt3_2 * Math.Sin(theta) * sin2phi * mag;
                            Histogram[3][i + D_Start] += rt3_2 * Math.Cos(2 * theta) * cossqphi * mag;
                            Histogram[4][i + D_Start] += rt3_2 * Math.Sin(2 * theta) * cossqphi * mag;
                        }
                    }

                    if (ISData != null)
                    {
                        foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                        {
                            if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram[0].Length - 1)
                            {
                                int R_Start = (int)Math.Ceiling(Sampling_Frequency * value.TravelTime);

                                double[][] V = value.Dir_Pressure(Rec_ID, xpos_alt, xpos_azi, degrees, Sampling_Frequency);

                                Hare.Geometry.Vector dir = value.Path[0][value.Path[0].Length - 1] - value.Path[0][value.Path[0].Length - 2];
                                dir.Normalize();
                                for (int i = 0; i < V.Length; i++)
                                {
                                    double mag = Math.Sqrt(V[i][0] * V[i][0] + V[i][1] * V[i][1] + V[i][2] * V[i][2]);
                                    double phi = Math.Asin(V[i][2] / mag);
                                    double theta = Math.Asin(V[i][1] / mag / Math.Cos(phi));
                                    double rt3_2 = Math.Sqrt(3) / 2;

                                    double sin2phi = Math.Sin(2 * phi);
                                    double cossqphi = Math.Cos(phi) * Math.Cos(phi);

                                    Histogram[0][i + R_Start] += mag * 3 * (Math.Sin(phi) * Math.Sin(phi) - 1) / 2;
                                    Histogram[1][i + R_Start] += rt3_2 * Math.Cos(theta) * sin2phi * mag;
                                    Histogram[2][i + R_Start] += rt3_2 * Math.Sin(theta) * sin2phi * mag;
                                    Histogram[3][i + R_Start] += rt3_2 * Math.Cos(2 * theta) * cossqphi * mag;
                                    Histogram[4][i + R_Start] += rt3_2 * Math.Sin(2 * theta) * cossqphi * mag;
                                }
                            }
                        }
                    }
                    return Histogram;
                }

            public static double[][] PTCurve_Ambisonics3(Direct_Sound Direct, ImageSourceData ISData, Receiver_Bank RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, bool Start_at_Zero, double xpos_alt, double xpos_azi, bool degrees)
                {
                    double[][] Histogram = new double[7][];
                    if (RTData != null)
                    {
                        double[][] hist_temp = RTData.Pressure_3Axis(Rec_ID);
                        Histogram[0] = new double[hist_temp[0].Length];
                        Histogram[1] = new double[hist_temp[0].Length];
                        Histogram[2] = new double[hist_temp[0].Length];
                        Histogram[3] = new double[hist_temp[0].Length];
                        Histogram[4] = new double[hist_temp[0].Length];
                        Histogram[5] = new double[hist_temp[0].Length];
                        Histogram[6] = new double[hist_temp[0].Length];

                        for (int i = 0; i < hist_temp[0].Length; i++)
                        {
                            Hare.Geometry.Vector Vpos = PachTools.Rotate_Vector(PachTools.Rotate_Vector(new Hare.Geometry.Vector(hist_temp[0][i], hist_temp[2][i], hist_temp[4][i]), xpos_azi, 0, true), 0, xpos_alt, true);
                            Hare.Geometry.Vector Vneg = PachTools.Rotate_Vector(PachTools.Rotate_Vector(new Hare.Geometry.Vector(-hist_temp[1][i], - hist_temp[3][i], - hist_temp[5][i]), xpos_azi, 0, true), 0, xpos_alt, true);
                            double magpos = Math.Sqrt(Vpos.x * Vpos.x + Vpos.y * Vpos.y + Vpos.z * Vpos.z);
                            double magneg = Math.Sqrt(Vneg.x * Vneg.x + Vneg.y * Vneg.y + Vneg.z * Vneg.z);
                            double phipos = Math.Asin(Vpos.z / magpos);
                            double phineg = Math.Asin(Vneg.z / magneg);
                            double thetapos = Math.Asin(Vpos.y / magpos / Math.Cos(phipos));
                            double thetaneg = Math.Asin(Vneg.y / magneg / Math.Cos(phineg));
                            double rt3_8 = Math.Sqrt(3)/8;
                            double rt15_2 = Math.Sqrt(15)/2;
                            double rt5_8 = Math.Sqrt(5)/8;

                            double LM_compos = Math.Cos(phipos) * (5 * Math.Pow(Math.Sin(phipos),2) - 1);
                            double LM_comneg = Math.Cos(phineg) * (5 * Math.Pow(Math.Sin(phineg),2) - 1);
                            double NO_compos = Math.Sin(phipos) * Math.Pow(Math.Cos(phipos),2);
                            double NO_comneg = Math.Sin(phineg) * Math.Pow(Math.Cos(phineg),2);
                            double PQ_compos = Math.Pow(Math.Cos(phipos), 3);
                            double PQ_comneg = Math.Pow(Math.Cos(phipos), 3);

                            Histogram[0][i] = magpos * Math.Sin(phipos) * (5 * Math.Sin(phipos) * Math.Sin(phipos) - 3) / 2 + magneg * Math.Sin(phineg) * (5 * Math.Sin(phineg) * Math.Sin(phineg) - 3) / 2;
                            Histogram[1][i] = rt3_8 * ( Math.Cos(thetapos) * LM_compos + Math.Cos(thetaneg) * LM_comneg );
                            Histogram[2][i] = rt3_8 * ( Math.Sin(thetapos) * LM_compos + Math.Sin(thetaneg) * LM_comneg ); 
                            Histogram[3][i] = rt15_2 * (Math.Cos(2*thetapos) * NO_compos + Math.Cos(2*thetaneg) * NO_compos );
                            Histogram[4][i] = rt15_2 * (Math.Sin(2*thetapos) * NO_compos + Math.Sin(2*thetaneg) * NO_compos );
                            Histogram[5][i] = rt5_8 * (Math.Cos(3*thetapos) * PQ_compos + Math.Cos(3*thetaneg) * PQ_compos );
                            Histogram[6][i] = rt5_8 * (Math.Sin(3*thetapos) * PQ_compos + Math.Sin(3*thetaneg) * PQ_compos );
                        }
                    }
                    else
                    {
                        Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[3] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[4] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[5] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[6] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                    }

                    if (Direct != null && Direct.IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct.Time(Rec_ID) * Sampling_Frequency);

                        double[][] V = Direct.Dir_Pressure(Rec_ID, xpos_alt, xpos_azi, degrees, true);
                        for (int i = 0; i < V.Length; i++)
                        {
                            double mag = Math.Sqrt(V[i][0] * V[i][0] + V[i][1] * V[i][1] + V[i][2] * V[i][2]);
                            double phi = Math.Asin(V[i][2] / mag);
                            double theta = Math.Asin(V[i][1] / mag / Math.Cos(phi));
                            double rt3_8 = Math.Sqrt(3)/8;
                            double rt15_2 = Math.Sqrt(15)/2;
                            double rt5_8 = Math.Sqrt(5)/8;

                            double LM_com = Math.Cos(phi) * (5 * Math.Pow(Math.Sin(phi),2) - 1);
                            double NO_com = Math.Sin(phi) * Math.Pow(Math.Cos(phi),2);
                            double PQ_com = Math.Pow(Math.Cos(phi), 3);
                            
                            Histogram[0][i + D_Start] += mag * Math.Sin(phi) * (5 * Math.Sin(phi) * Math.Sin(phi) - 3) / 2;
                            Histogram[1][i + D_Start] += rt3_8 * Math.Cos(theta) * LM_com;
                            Histogram[2][i + D_Start] += rt3_8 * Math.Sin(theta) * LM_com;
                            Histogram[3][i + D_Start] += rt15_2 * Math.Cos(2 * theta) * NO_com;
                            Histogram[4][i + D_Start] += rt15_2 * Math.Sin(2 * theta) * NO_com;
                            Histogram[5][i + D_Start] += rt5_8 * Math.Cos(3 * theta) * PQ_com;
                            Histogram[6][i + D_Start] += rt5_8 * Math.Sin(3 * theta) * PQ_com;
                        }
                    }

                    if (ISData != null)
                    {
                        foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                        {
                            if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram[0].Length - 1)
                            {
                                int R_Start = (int)Math.Ceiling(Sampling_Frequency * value.TravelTime);

                                double[][] V = value.Dir_Pressure(Rec_ID, xpos_alt, xpos_azi, degrees, Sampling_Frequency);

                                Hare.Geometry.Vector dir = value.Path[0][value.Path[0].Length - 1] - value.Path[0][value.Path[0].Length - 2];
                                dir.Normalize();
                                for (int i = 0; i < V.Length; i++)
                                {
                                    double mag = Math.Sqrt(V[i][0] * V[i][0] + V[i][1] * V[i][1] + V[i][2] * V[i][2]);
                                    double phi = Math.Asin(V[i][2] / mag);
                                    double theta = Math.Asin(V[i][1] / mag / Math.Cos(phi));
                                    double rt3_8 = Math.Sqrt(3)/8;
                                    double rt15_2 = Math.Sqrt(15)/2;
                                    double rt5_8 = Math.Sqrt(5)/8;

                                    double LM_com = Math.Cos(phi) * (5 * Math.Pow(Math.Sin(phi),2) - 1);
                                    double NO_com = Math.Sin(phi) * Math.Pow(Math.Cos(phi),2);
                                    double PQ_com = Math.Pow(Math.Cos(phi), 3);

                                    Histogram[0][i + R_Start] += mag * Math.Sin(phi) * (5 * Math.Sin(phi) * Math.Sin(phi) - 3) / 2;
                                    Histogram[1][i + R_Start] += rt3_8 * Math.Cos(theta) * LM_com;
                                    Histogram[2][i + R_Start] += rt3_8 * Math.Sin(theta) * LM_com;
                                    Histogram[3][i + R_Start] += rt15_2 * Math.Cos(2 * theta) * NO_com;
                                    Histogram[4][i + R_Start] += rt15_2 * Math.Sin(2 * theta) * NO_com;
                                    Histogram[5][i + R_Start] += rt5_8 * Math.Cos(3 * theta) * PQ_com;
                                    Histogram[6][i + R_Start] += rt5_8 * Math.Sin(3 * theta) * PQ_com;
                                }
                            }
                        }
                    }
                    return Histogram;
                }

                public static double[][] PTCurve_Fig8_3Axis(IEnumerable<Direct_Sound> Direct, IEnumerable<ImageSourceData> ISData, IEnumerable<Environment.Receiver_Bank> RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, List<int> SrcIDs, bool StartAtZero, double alt, double azi, bool degrees)
                {
                    //TODO: Make provisions for specifying source delays...
                    double[][] Histogram = new double[3][];
                    //Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency)];
                    //Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency)];

                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];
                    Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[][] IR = PTCurve_Fig8_3Axis(Direct.ElementAt<Direct_Sound>(s), ISData.ElementAt<ImageSourceData>(s), RTData.ElementAt<Receiver_Bank>(s), CO_Time, Sampling_Frequency, Rec_ID, StartAtZero, alt, azi, degrees);
                        //Array.Resize(ref IR, IR.Length + (int)Math.Ceiling(maxdelay));
                        //if (IR.Length > Histogram.Length) Array.Resize(ref Histogram, IR.Length);
                        for (int d = 0; d < 3; d++)
                        {
                            for (int i = 0; i < IR[0].Length; i++)
                            {
                                Histogram[d][i + (int)Math.Ceiling(Direct.ElementAt<Direct_Sound>(s).Delay_ms / 1000 * Sampling_Frequency)] += IR[d][i];
                            }
                        }
                    }
                    return Histogram;
                }

                public static double[][] PTCurve_Fig8_3Axis(Direct_Sound Direct, ImageSourceData ISData, Receiver_Bank RTData, double CO_Time, int Sampling_Frequency, int Rec_ID, bool Start_at_Zero, double xpos_alt, double xpos_azi, bool degrees)
                {
                    double[][] Histogram = new double[3][];
                    if (RTData != null)
                    {
                        double[][] hist_temp = RTData.Pressure_3Axis(Rec_ID);
                        Histogram[0] = new double[hist_temp[0].Length];
                        Histogram[1] = new double[hist_temp[0].Length];
                        Histogram[2] = new double[hist_temp[0].Length];
                        for(int i = 0; i < hist_temp[0].Length; i++)
                        {
                            Hare.Geometry.Vector V = PachTools.Rotate_Vector(PachTools.Rotate_Vector(new Hare.Geometry.Vector(hist_temp[0][i] - hist_temp[1][i],hist_temp[2][i] - hist_temp[3][i],hist_temp[4][i] - hist_temp[5][i]),xpos_azi, 0, true), 0, xpos_alt, true);
                            Histogram[0][i] = V.x;
                            Histogram[1][i] = V.y;
                            Histogram[2][i] = V.z;
                        }
                    }
                    else
                    {
                        Histogram[0] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[1] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                        Histogram[2] = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                    }

                    if (Direct != null && Direct.IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct.Time(Rec_ID) * Sampling_Frequency);
                        
                        double[][] V = Direct.Dir_Pressure(Rec_ID, xpos_alt, xpos_azi, degrees, true);
                        for(int i = 0; i < V.Length; i++)
                        {
                            Histogram[0][D_Start + i] += V[i][0];
                            Histogram[1][D_Start + i] += V[i][1];
                            Histogram[2][D_Start + i] += V[i][2];
                        }
                    }

                    if (ISData != null)
                    {
                        foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                        {
                            if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram[0].Length - 1)
                            {
                                int R_Start = (int)Math.Ceiling(Sampling_Frequency * value.TravelTime);

                                double[][] V = value.Dir_Pressure(Rec_ID, xpos_alt, xpos_azi, degrees, Sampling_Frequency);
                                
                                Hare.Geometry.Vector dir = value.Path[0][value.Path[0].Length-1] - value.Path[0][value.Path[0].Length - 2];
                                dir.Normalize();
                                for (int i = 0; i < value.Pressure.Length; i++)
                                {
                                    Histogram[0][R_Start + i] += V[i][0];
                                    Histogram[1][R_Start + i] += V[i][1];
                                    Histogram[2][R_Start + i] += V[i][2];
                                }
                            }
                        }
                    }
                    return Histogram;
                }

                public static double[] PTCurve_Directional(IEnumerable<Direct_Sound> Direct, IEnumerable<ImageSourceData> ISData, IEnumerable<Environment.Receiver_Bank> RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, List<int> SrcIDs, bool StartAtZero, double alt, double azi, bool degrees)
                {
                    if (Direct == null) Direct = new Direct_Sound[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (ISData == null) ISData = new ImageSourceData[SrcIDs[SrcIDs.Count - 1] + 1];
                    if (RTData == null) RTData = new Environment.Receiver_Bank[SrcIDs[SrcIDs.Count - 1] + 1];

                    double maxdelay = 0;
                    foreach (Direct_Sound d in Direct) maxdelay = Math.Max(maxdelay, d.Delay_ms);
                    maxdelay *= Sampling_Frequency / 1000;

                    double[] Histogram = new double[(int)(CO_Time * Sampling_Frequency) + 4096 + (int)Math.Ceiling(maxdelay)];

                    foreach (int s in SrcIDs)
                    {
                        double[] IR = PTCurve_Directional(Direct.ElementAt<Direct_Sound>(s), ISData.ElementAt<ImageSourceData>(s), RTData.ElementAt<Receiver_Bank>(s), CO_Time, Sampling_Frequency, Octave, Rec_ID, StartAtZero, alt, azi, degrees);
                        //Array.Resize(ref Histogram, Histogram.Length + (int)Math.Ceiling(maxdelay));
                        //if (IR.Length > Histogram.Length) Array.Resize(ref Histogram, IR.Length);
                        for (int i = 0; i < IR.Length; i++)
                        {
                            Histogram[i + (int)Math.Ceiling(Direct.ElementAt<Direct_Sound>(s).Delay_ms / 1000 * Sampling_Frequency)] += IR[i];
                        }
                    }
                    return Histogram;
                }

                public static double[] PTCurve_Directional(Direct_Sound Direct, ImageSourceData ISData, Receiver_Bank RTData, double CO_Time, int Sampling_Frequency, int Octave, int Rec_ID, bool Start_at_Zero, double alt, double azi, bool degrees)
                {
                    double[] Histogram;
                    if (RTData != null)
                    {
                        int[] ids = new int[3];
                        ids[0] = (azi > 90 && azi < 270) ? 1 : 0;
                        ids[0] = (azi <= 180) ? 3 : 4;
                        ids[0] = (alt > 0) ? 4 : 5;
                        double[][] hist_temp = RTData.Pressure_3Axis(Rec_ID);
                        Histogram = new double[hist_temp[0].Length];
                        for (int i = 0; i < hist_temp[0].Length; i++)
                        {
                            Hare.Geometry.Vector V = PachTools.Rotate_Vector(PachTools.Rotate_Vector(new Hare.Geometry.Vector(hist_temp[ids[0]][i], hist_temp[ids[1]][i], hist_temp[ids[2]][i]), azi, 0, true), 0, alt, true);
                            Histogram[i] = V.x;
                        }
                    }
                    else
                    {
                        Histogram = new double[(int)(CO_Time * Sampling_Frequency) + 2048];
                    }

                    if (Direct != null && Direct.IsOccluded(Rec_ID))
                    {
                        int D_Start = 0;
                        if (!Start_at_Zero) D_Start = (int)Math.Ceiling(Direct.Time(Rec_ID) * Sampling_Frequency);

                        double[][] V = Direct.Dir_Pressure(Rec_ID, alt, azi, degrees, false);
                        for (int i = 0; i < V.Length; i++)
                        {
                            Histogram[D_Start + i] += V[i][0];
                        }
                    }

                    if (ISData != null)
                    {
                        foreach (Deterministic_Reflection value in ISData.Paths[Rec_ID])
                        {
                            if (Math.Ceiling(Sampling_Frequency * value.TravelTime) < Histogram.Length - 1)
                            {
                                int R_Start = (int)Math.Ceiling(Sampling_Frequency * value.TravelTime);

                                double[] V = value.Dir_Pressure(Rec_ID, alt, azi, degrees, false, Sampling_Frequency);
                                for (int i = 0; i < value.Pressure.Length; i++)
                                {
                                    Histogram[R_Start + i] += V[i];
                                }
                            }
                        }
                    }
                    return Histogram;    
                }

            ///<summary>
            /// Sabine Reverberation Time : T60 = 0.161V/A
            ///</summary>
            public static void Sabine(Environment.Scene Room, ref double[] T60)
            {
                T60 = new double[8];
                if (Room == null) return;
                double TA;
                for (int t = 0; t <= 7; t++)
                {
                    TA = 0;
                    for (int q = 0; q <= Room.Count() - 1; q++)
                    {
                        TA += (Room.SurfaceArea(q) * Room.AbsorptionValue[q].Coefficient_A_Broad(t));
                    }
                    T60[t] = (0.161 * (Room.RoomVolume() / (TA + (4 * Room.Attenuation(0)[t] * Room.RoomVolume()))));
                }
            }

            ///<summary>
            /// Sabine Reverberation Time : T60 = 0.161V/ln(1-a)S
            ///</summary>
            public static void Eyring(Environment.Scene Room, ref double[] T60)
            {
                T60 = new double[8];
                if (Room == null) return;
                double TA;
                for (int t = 0; t <= 7; t++)
                {
                    TA = 0;
                    for (int q = 0; q <= Room.Count() - 1; q++)
                    {
                        TA += (Room.SurfaceArea(q) * (-System.Math.Log(1 - Room.AbsorptionValue[q].Coefficient_A_Broad(t),System.Math.E)));
                    }
                    T60[t] = 0.161 * Room.RoomVolume() / (TA + 4 * Room.Attenuation(0)[t] * Room.RoomVolume());
                }
            }

            /// <summary>
            /// If possible, calculate the volume of the room.
            /// </summary>
            /// <param name="Breps">Surfaces that make up the model.</param>
            /// <param name="Volume"></param>
            /// <param name="SurfaceArea"></param>
            /// <returns>True if successful.</returns>
            public static bool RoomVolume(List<Brep> Breps, ref double Volume, out double[] SurfaceArea)
            {
                //Brep[] BrepList = null;
                //List<Brep> BrepIN = new List<Brep>();
                SurfaceArea = new double[Breps.Count];
                //OnMassProperties ap = new OnMassProperties();
                for (int x = 0; x < Breps.Count; x++)
                {
                    AreaMassProperties ap = AreaMassProperties.Compute(Breps[x]);
                    SurfaceArea[x] = ap.Area;
                }

                Brep[] Room = Brep.JoinBreps(Breps, 0.001);            
                Rhino.RhinoApp.WriteLine("Room is not closed. Using Bounding Volume.");
                BoundingBox Box = new BoundingBox();
                foreach (Brep srf in Breps)
                {
                     Box.Union(srf.GetBoundingBox(false));
                }

                ///////////////////////////////////////////////////////////////
                //Rhino.RhinoDoc.ActiveDoc.Objects.AddPoints(Box.GetCorners());
                ///////////////////////////////////////////////////////////////
                try
                {
                    Volume = VolumeMassProperties.Compute(Box.ToBrep()).Volume;
                }
                catch 
                {
                    Volume = 0;
                }

                return false;
            }

            /// <summary>
            /// The speed of sound in m/s.
            /// </summary>
            /// <param name="Air_Temp"></param>
            /// <returns></returns>
            public static double SoundSpeed(double Air_Temp)
            {
                return 331 + 0.6 * Air_Temp;
            }

            /// <summary>
            /// Calculates the schroeder integral from the energy time curve.
            /// </summary>
            /// <param name="etc">energy time curve.</param>
            /// <returns></returns>
            public static double[] Schroeder_Integral(double[] etc)
            {
                double sum_energy = 0;
                double[] build_up = new double[etc.Length];
                for (int index = 0; index < etc.Length; index++)
                {
                    build_up[index] = sum_energy += etc[index];
                }

                double[] schroed = new double[etc.Length];
                if (sum_energy == 0) return schroed;
                for (int index = 0; index < etc.Length; index++)
                {
                    schroed[index] = 1 - (build_up[index] / sum_energy);
                }

                return schroed;
            }

            /// <summary>
            /// Gets the logarithmic value of the signal.
            /// </summary>
            /// <param name="etc">input data</param>
            /// <param name="limit_in_db">the minimum value of the result.</param>
            /// <returns></returns>
            public static double[] Log10Data(double[] etc, double limit_in_db)
            {
                double[] log_data = new double[etc.Length];
                double limit_val = Math.Pow(10, (limit_in_db / 10));
                for (int index = 1; index <= etc.Length - 1; index++)
                {
                    if (etc[index] >= limit_val)
                    {
                        log_data[index] = 10 * Math.Log10(etc[index]);
                    }
                    else
                    {
                        log_data[index] = limit_in_db;
                    }
                }
                return log_data;
            }

            /// <summary>
            /// Find the start and end of the linear least square fit line, based on the decay interfval chosen.
            /// </summary>
            /// <param name="log_sch">logarithmic schroeder integral</param>
            /// <param name="first">the start of the linear least square fit line</param>
            /// <param name="second">the end of the linear least square fit line</param>
            /// <returns>the array of start and end values.</returns>
            public static int[] Schroeder_Limits(double[] log_sch, double first, double second)
            {
                int[] Limits = new int[2];
                for (int index = 1; index <= log_sch.Length - 1; index++)
                {
                    if (log_sch[index] < first)
                    {
                        Limits[0] = index;
                        break;
                    }
                }

                for (int index = Limits[0]; index <= log_sch.Length - 1; index++)
                {
                    if (log_sch[index] < second)
                    {
                        Limits[1] = index;
                        break;
                    }
                }
                return Limits;
            }

            /// <summary>
            /// Calculates linear least square fit curve.
            /// </summary>
            /// <param name="Magnitude">Section schroeder integral within limits established for lsf operation.</param>
            /// <param name="sample_f">sample frequency.</param>
            /// <returns></returns>
            public static double[] Least_Square_Fit(double[] Magnitude, double sample_f)
            {
                double[] Time = new double[Magnitude.Length];

                int length = Time.Length;

                double Time_Sum = 0;
                double Mag_Sum = 0;
                double TT_Sum = 0;
                double TM_Sum = 0;
                int intIndex = 0;

                for (intIndex = 0; intIndex <= length - 1; intIndex++)
                {
                    double q = intIndex / sample_f;
                    Time_Sum += (q);
                    Mag_Sum += Magnitude[intIndex];

                    TT_Sum += q * q;
                    TM_Sum += q * Magnitude[intIndex];
                }

                double Slope = ((length * TM_Sum) - (Time_Sum * Mag_Sum)) / ((length * TT_Sum) - (Time_Sum * Time_Sum));

                double Intercept = (Mag_Sum - (Slope * Time_Sum)) / length;
                double[] dblResult = { Slope, Intercept };
                return dblResult;
            }

            /// <summary>
            /// normalizes the SPL-Time curve to relative SPL.
            /// </summary>
            /// <param name="SPL_Curve">Spl-Time curve</param>
            /// <returns>relative spl-time curve.</returns>
            public static double[] Normalize_Function(double[] SPL_Curve)
            {
                double max = 0;
                foreach (double x in SPL_Curve)
                {
                    if (max < x) max = x;
                }

                for (int i = 0; i < SPL_Curve.Length; i++)
                {
                    SPL_Curve[i] -= max;
                }

                return SPL_Curve;
            }

            /// <summary>
            /// Calculate reverberation time. (T-X)
            /// </summary>
            /// <param name="Schroeder_Integral"></param>
            /// <param name="Decay_Depth">For T-10, 10, For T-20, 20, etc...</param>
            /// <param name="sample_f">The number of bins/second</param>
            /// <returns></returns>
            public static double T_X(double[] Schroeder_Integral, int Decay_Depth, int sample_f)
            {
                double[] log_sch = Log10Data(Schroeder_Integral, -60);
                int[] Limits = Schroeder_Limits(log_sch, -5, -5 - Math.Abs(Decay_Depth));

                double[] snippet = new double[Limits[1] - Limits[0] + 1];
                for (int index = Limits[0]; index <= Limits[1]; index++)
                {
                    snippet[index - Limits[0]] = log_sch[index];
                }

                double[] Coefficients = Least_Square_Fit(snippet, sample_f);
                double Value = (-60 / Coefficients[0]);

                return Value;
            }

            /// <summary>
            /// Calculates early decay time
            /// </summary>
            /// <param name="Schroeder_Integral"></param>
            /// <param name="sample_f">sample frequency.</param>
            /// <returns></returns>
            public static double EarlyDecayTime(double[] Schroeder_Integral, int sample_f)
            {
                double[] log_sch = Log10Data(Schroeder_Integral, -60);
                int[] Limits = Schroeder_Limits(log_sch, -0.0001, -10.0001);

                double[] snippet = new double[Limits[1] - Limits[0] + 1];
                for (int index = Limits[0]; index <= Limits[1]; index++)
                {
                    snippet[index - Limits[0]] = log_sch[index];
                }

                double[] Coefficients = Least_Square_Fit(snippet, sample_f);
                double edt = (-60 / Coefficients[0]);

                return edt;
            }

            /// <summary>
            /// Calculate clarity from energy time curve.
            /// </summary>
            /// <param name="etc">energy time curve.</param>
            /// <param name="sample_f"></param>
            /// <param name="seconds"></param>
            /// <param name="startTime">direct arrival time.</param>
            /// <returns></returns>
            public static double Clarity(double[] etc, double sample_f, double seconds, double startTime, bool pressure)
            {
                if (pressure) seconds += (double)1024 / sample_f;

                double Binwidth = 1 / sample_f;

                int EndIndex = (int)Math.Floor((seconds + startTime) * sample_f);
                double Sum_Early = 0;
                double Sum_Late = 0;

                for (int q = 0; q < EndIndex; q++)
                {
                    Sum_Early += etc[q];
                }

                double bin_split_pt = (seconds + startTime - (EndIndex * Binwidth)) / Binwidth;
                //Yields percentage of last bin to give to early energy calculation.
                Sum_Early += bin_split_pt * etc[EndIndex];

                for (int q = EndIndex + 1; q < etc.Length; q++)
                {
                    Sum_Late += etc[q];
                }

                Sum_Late += (1 - bin_split_pt) * etc[EndIndex];
                return 10 * Math.Log10(Sum_Early / Sum_Late);
            }

            public static double[] abs_rt = new double[7]{46, 27, 12, 6.5, 7.5, 8, 12};

            public static double[] Modulation_Transfer_Index(double[][] ETC, double rhoC, double[] Noise, int samplefreq)
            {
                for (int i = 0; i < 7; i++) if (ETC[i].Length < samplefreq*1.6) Array.Resize<double>(ref ETC[i], (int)(samplefreq * 1.6));

                double[] MTI = new double[7];
                double[] fm = new double[14] { .63, .8, 1.0, 1.25, 1.6, 2.0, 2.5, 3.15, 4.0, 5.0, 6.3, 8.0, 10.0, 12.5 };
                double I_LowerBand = 0;
                double[] etc = AcousticalMath.Add_Noise_I(ETC[0], Noise[0]);

                for (int s = 0; s < ETC[0].Length; s++)
                {
                    I_LowerBand += etc[s];
                }

                I_LowerBand *= rhoC;

                for (int oct = 1; oct < 8; oct++)
                {
                    etc = AcousticalMath.Add_Noise_I(ETC[oct], Noise[oct]);
                    double[] mk = new double[14];
                    double[] ISin = new double[14], ICos = new double[14];
                    double sumI = 0;

                    for (int s = 0; s < etc.Length; s++)
                    {
                        double t = s/(double)samplefreq;
                        double e = etc[s];
                        sumI += e;
                        for (int mct = 0; mct < 14; mct++)
                        {
                            double p = (Utilities.Numerics.PiX2 * fm[mct] * t);
                            ISin[mct] += e * Math.Sin(p);
                            ICos[mct] += e * Math.Cos(p);
                        }
                    }

                    sumI *= rhoC;

                    for (int mct = 0; mct < 14; mct++)
                    {
                        ISin[mct] *= rhoC;
                        ICos[mct] *= rhoC;
                    }

                    double Irtk = Math.Pow(10, abs_rt[oct-1]/10) * 1e-12 * rhoC;
                    double amf;
                    double LowerbandL = AcousticalMath.SPL_Intensity(I_LowerBand);
                    if (LowerbandL < 63)
                    {
                        amf = Math.Pow(10,(0.5 * LowerbandL - 65)/10);
                    }
                    else if(LowerbandL < 67)
                    {
                        amf = Math.Pow(10,(1.8 * LowerbandL - 146.9)/10);
                    }
                    else if(LowerbandL <100)
                    {
                        amf = Math.Pow(10,(0.5 * LowerbandL - 59.8)/10);
                    }
                    else
                    {
                        amf = Math.Pow(10,-10/10);
                    }
                    
                    double Iamk = I_LowerBand * amf;

                    for (int mct = 0; mct < 14; mct++)
                    {
                        mk[mct] = Math.Sqrt(ISin[mct] * ISin[mct] + ICos[mct] * ICos[mct]) / sumI;
                        mk[mct] *= sumI / (sumI + Iamk +Irtk);
                        double TI = ((10 * Math.Log10(mk[mct] / (1 - mk[mct])) + 15.0) / 30.0);
                        MTI[oct - 1] += TI < 0 ? 0 : TI > 1 ? 1 : TI;
                    }

                    I_LowerBand = sumI;
                }

                for (int i = 0; i < MTI.Length; i++) MTI[i] /= 14;
                return MTI;
            }

            public static double[] Add_Noise_I(double[] etc, double Noise)
            {
                double i = 1e-12 * Math.Pow(10, (Noise + 5) / 10);

                Random r = new Random();

                for (int t = 0; t < etc.Length; t++) 
                {
                    double n = r.NextDouble();
                    etc[t] += i * n * n / 413;
                }

                return etc;
            }

            public static double[] Speech_Transmission_Index(double[][] ETC, double rhoC, double[] Noise, int samplefreq)
            {
                double[] MTI = Modulation_Transfer_Index(ETC, rhoC, Noise, samplefreq);

                double[] alpha2003 = new double[7] { 0.13, 0.14, 0.11, 0.12, 0.19, 0.17, 0.14 };
                double[] alphaMale = new double[7] { 0.085, 0.127, 0.23, 0.233, 0.309, 0.224, 0.173 };
                double[] alphaFemale = new double[7] { 0, 0.117, 0.223, 0.216, 0.328, 0.25, 0.194 };
                double[] BetaMale = new double[6] { 0.085, 0.078, 0.065, 0.011, 0.047, 0.095 };
                double[] BetaFemale = new double[6] { 0, 0.099, 0.066, 0.062, 0.025, 0.076};

                double[] STI = new double[3];
                for (int oct = 0; oct < 7; oct++) 
                {
                    STI[0] += MTI[oct] * alpha2003[oct];
                    STI[1] += MTI[oct] * alphaMale[oct];
                    STI[2] += MTI[oct] * alphaFemale[oct];
                }
                for (int oct = 0; oct < 6; oct++)
                {
                    double R = Math.Sqrt(MTI[oct] * MTI[oct + 1]);
                    STI[1] -= BetaMale[oct] * R;
                    STI[2] -= BetaFemale[oct] * R;
                }
                //STI:
                //1 = general (2003)
                //2 = Male
                //3 = Female
                return STI;
            }

            public static double Lateral_Parameter(double[][] Dir_ETC, double[] Total_ETC, double LowerBound_s, double UpperBound_s, double sample_f, double startTime, bool pressure)
            {
                if (pressure) startTime += (double)1024 / sample_f;

                double sum_Lateral = 0, sum_Total = 0;
                int i = (int)Math.Floor(startTime * sample_f);
                while (i < sample_f * (LowerBound_s + startTime))
                {
                    sum_Total += Total_ETC[i];
                    i++;
                }
                while (i <= Math.Floor(sample_f * (UpperBound_s + startTime)))
                {
                    Hare.Geometry.Vector V = new Hare.Geometry.Vector(Dir_ETC[0][i], Dir_ETC[1][i], Dir_ETC[2][i]);
                    sum_Lateral += Math.Abs(Hare.Geometry.Hare_math.Dot(V, new Hare.Geometry.Vector(0,1,0)));
                    sum_Total += Total_ETC[i];
                    i++;
                }

                return sum_Lateral / sum_Total;
            }

            public static double Lateral_Parameter(double[] Lateral_ETC, double[] Total_ETC, double LowerBound_s, double UpperBound_s, double sample_f, double startTime, bool pressure)
            {
                double sum_Lateral = 0, sum_Total = 0;
                int i = (int)Math.Floor(startTime * sample_f);
                while(i < sample_f * (LowerBound_s + startTime))
                {
                    sum_Total += Total_ETC[i];
                    i++;
                }
                while(i <= Math.Floor(sample_f * (UpperBound_s + startTime)))
                {
                    sum_Lateral += Math.Abs(Lateral_ETC[i]);
                    sum_Total += Total_ETC[i];
                    i++;
                }

                return sum_Lateral / sum_Total;
            }

            public static double Lateral_Fraction(double[][] ETC, double sample_f, double startTime, bool pressure)
            {
                double[] Total_ETC = new double[ETC[0].Length];
                for (int i = 0; i < ETC[0].Length; i++) Total_ETC[i] = Math.Sqrt(ETC[0][i] * ETC[0][i] + ETC[1][i] * ETC[1][i] + ETC[2][i] * ETC[2][i]);

                return Lateral_Parameter(ETC[1], Total_ETC, 0.005, 0.08, sample_f, startTime, pressure);
            }

            public static double Lateral_Fraction(double[] Total_ETC, double[] Lateral_ETC, double sample_f, double startTime, bool pressure)
            {
                return Lateral_Parameter(Lateral_ETC, Total_ETC, 0.005, 0.08, sample_f, startTime, pressure);
            }

            public static double Lateral_Fraction(double[] Total_ETC, double[][] Lateral_ETC, double sample_f, double startTime, bool pressure)
            {
                return Lateral_Parameter(Lateral_ETC, Total_ETC, 0.005, 0.08, sample_f, startTime, pressure);
            }

            public static double Lateral_Efficiency(double[] Total_ETC, double[] Lateral_ETC, double sample_f, double startTime, bool pressure)
            {
                return Lateral_Parameter(Lateral_ETC, Total_ETC, 0.025, 0.08, sample_f, startTime, pressure);
            }

            /// <summary>
            /// Calculates the Echo Kriterion (Dietsch & Kraak) of the impulse response.
            /// </summary>
            /// <param name="PTC">Pressure time curve</param>
            /// <param name="sample_freq">usually 44100 Hz.</param>
            /// <param name="Direct_Time">Time of arrival of the direct sound.</param>
            /// <param name="Speech">Is it speech (true) or music (false)?</param>
            /// <param name="EKGrad">The actual EKGrad values...</param>
            /// <param name="PercentEcho">The linearly interpolated percentage of people that would perceive an echo at that location.</param>
            /// <param name="Echo10">Do 10% of people perceive an echo?</param>
            /// <param name="Echo50">Do 50% of people perceive an echo?</param>
            public static void EchoCriterion(double[] PTC, int sample_freq, double Direct_Time, bool Speech, out double[] EKGrad, out double[] PercentEcho, out bool Echo10, out bool Echo50)
            {
                EKGrad = new double[PTC.Length];
                double[] EK = new double[2];
                double dte;
                double n;

                if (Speech)
                {
                    //Speech
                    EK = new double[]{.9, 1};
                    dte = 0.009;
                    n = 2f/3f;
                }
                else
                {           
                    //Music
                    EK = new double[2]{1.5, 1.8};
                    dte = 0.014;
                    n = 1;
                }

                double[] time = new double[PTC.Length];
                double[] num = new double[PTC.Length];
                double[] denom = new double[PTC.Length];
                double[] ts = new double[PTC.Length];
                PercentEcho = new double[PTC.Length];

                num[0] = 0;
                denom[0] = 0;
                ts[0] = 0;

                for (int t = (int)(Direct_Time * sample_freq); t < PTC.Length; t++)
                {
                    time[t] = time[t-1] + 1 / (double)sample_freq;
                    double Pn = Math.Pow(Math.Abs(PTC[t]), n);
                    denom[t] = denom[t-1] + Pn;
                    num[t] = num[t-1] + Pn * time[t];                    
                    ts[t] = num[t] / denom[t];
                }
                
                double dEK = (EK[1] - EK[0]) / 40;

                for (int t = (int)(dte * sample_freq); t < PTC.Length; t++)
                {
                    EKGrad[t] = (ts[t] - ts[t - (int)(dte*sample_freq)] )/ dte;
                    //Linear interpolation of Echo percentage... (for prudence, not to exceed 50.
                    PercentEcho[t] = (EKGrad[t] - EK[0]) * dEK + 10;
                    //PercentEcho[t] = Math.Min(50, Math.Max(10, PercentEcho[t]));
                }

                double max = PercentEcho.Max();

                Echo10 = false; Echo50 = false;

                if (max > 10) Echo10 = true;
                if (max > 50) Echo50 = true;
            }

            /// <summary>
            /// Generic logarithmic energy ratio calculation
            /// </summary>
            /// <param name="etc">energy time curve.</param>
            /// <param name="sample_f"></param>
            /// <param name="num_start">numerator early bound.</param>
            /// <param name="num_end">numberator late bound.</param>
            /// <param name="denom_start">denominator early bound.</param>
            /// <param name="denom_end">denominator late bound.</param>
            /// <param name="startTime">direct arrival time.</param>
            /// <returns></returns>
            public static double Energy_Ratio(double[] etc, double sample_f, double num_start, double num_end, double denom_start, double denom_end, double startTime)
            {
                double Binwidth = 1 / sample_f;

                int EarlyStartindex;
                double ESbinsplt;
                if (num_start == 0)
                {
                    EarlyStartindex = 0;
                    ESbinsplt = 1;
                }
                else
                {
                    EarlyStartindex = (int)Math.Floor((num_start + startTime) * sample_f);
                    ESbinsplt = 1 - (num_start + startTime - (EarlyStartindex * Binwidth)) * sample_f;
                }

                int LateStartindex;
                double LSbinsplt;
                if (denom_start == 0)
                {
                    LateStartindex = 0;
                    LSbinsplt = 1;
                }
                else
                {
                    LateStartindex = (int)Math.Floor((denom_start + startTime) * sample_f);
                    LSbinsplt = 1 - (denom_start + startTime - (LateStartindex * Binwidth)) * sample_f;
                }

                int EarlyEndindex;
                double EEbinsplt;
                if (num_end == 0)
                {
                    EarlyEndindex = 0;
                    EEbinsplt = 1;
                }
                else
                {
                    EarlyEndindex = (int)Math.Floor((num_end + startTime) * sample_f);
                    EEbinsplt = (num_end + startTime - (EarlyEndindex * Binwidth)) * sample_f;
                }

                int LateEndindex;
                double LEbinsplt;
                if (denom_end == 0)
                {
                    LateEndindex = 0;
                    LEbinsplt = 0;
                }
                else
                {
                    LateEndindex = (int)Math.Floor((denom_end + startTime) * sample_f);
                    LEbinsplt = (denom_end + startTime - (LateEndindex * Binwidth)) * sample_f;
                }

                double Sum_Early = 0;
                double Sum_Late = 0;

                for (int q = EarlyStartindex + 1; q < EarlyEndindex; q++)
                {
                    Sum_Early += etc[q];
                }

                //Yields percentage of first and last bins to give to numerator calculation.
                Sum_Early += etc[EarlyStartindex] * ESbinsplt + etc[EarlyEndindex] * EEbinsplt;

                for (int q = LateStartindex + 1; q < LateEndindex; q++)
                {
                    Sum_Late += etc[q];
                }

                //Yields percentage of first and last bins to give to numerator calculation.
                Sum_Late += etc[LateStartindex] * LSbinsplt + etc[LateEndindex] * LEbinsplt;

                return 10 * Math.Log10(Sum_Early / Sum_Late);
            }

            /// <summary>
            /// Calculate definition from energy time curve.
            /// </summary>
            /// <param name="etc">energy time curve.</param>
            /// <param name="sample_f"></param>
            /// <param name="seconds"></param>
            /// <param name="StartTime"></param>
            /// <returns></returns>
            public static double Definition(double[] etc, double sample_f, double seconds, double StartTime, bool pressure)
            {
                if (pressure) StartTime += (double)1024 / sample_f;

                double Binwidth = 1 / sample_f;

                int StartIndex = 0;
                StartIndex = (int)Math.Floor(StartTime * sample_f);

                int EndIndex = (int)Math.Floor(seconds * sample_f) + StartIndex;
                double Sum_Early = 0;
                double Sum_All = 0;

                for (int q = 0; q < EndIndex; q++)
                {
                    Sum_Early += etc[q];
                }
                double bin_split_pt = (seconds + StartTime - (EndIndex * Binwidth)) / Binwidth;

                //Yields percentage of last bin to give to early energy calculation.
                Sum_Early += bin_split_pt * etc[EndIndex];

                for (int q = 0; q < etc.Length; q++)
                {
                    Sum_All += etc[q];
                }

                return Sum_Early / Sum_All * 100;
            }

            /// <summary>
            /// Calculate center time - center of gravity of impulse response.
            /// </summary>
            /// <param name="etc">energy time curve.</param>
            /// <param name="sample_f"></param>
            /// <param name="Direct_time"></param>
            /// <returns></returns>
            public static double Center_Time(double[] etc, int sample_f, double Direct_time)
            {
                double sumPT=0, sumT=0;
                double BW = 1 / (double)sample_f;
                for(int i = 0; i < etc.Length; i++)
                {
                    sumPT += etc[i] * (i*BW - Direct_time);
                    sumT += etc[i]; 
                }
                return sumPT / sumT;
            }

            /// <summary>
            /// Calculate sound strength.
            /// </summary>
            /// <param name="etc"></param>
            /// <returns></returns>
            public static double Strength(double[] etc, double SWL, bool pressure)
            {
                double Sum_All = 0;
                for (int q = 0; q < etc.Length; q++)
                {
                    Sum_All += etc[q];
                }

                if (pressure) return AcousticalMath.SPL_Pressure(Sum_All) - SWL + 31;
                else return AcousticalMath.SPL_Intensity(Sum_All) - SWL + 31;
            }

            /// <summary>
            /// Calculate Initial Time Delay Gap (ITDG). **Caution: Unbenchmarked**
            /// </summary>
            /// <param name="etc"></param>
            /// <returns>ITDG in milliseconds</returns>
            public static int InitialTimeDelayGap(double[] etc)
            {
                int t1 = 0, t2 = 0;
                double maxdiff = 0, nextdiff = 0;
                for (int t = 0; t < 100; t++)
                {
                    double diff = etc[t + 1] - etc[t];

                    if (diff > maxdiff)
                    {
                        maxdiff = diff;
                        t1 = t;
                        continue;
                    }

                    if (diff > nextdiff)
                    {
                        nextdiff = diff;
                        t2 = t;
                    }
                }

                return t2 - t1;
            }
        }

        public class PachTools
        {
            public sealed class RandomNumberGenerator : Random
            {
                private static Random _global = new Random();
                [ThreadStatic]
                private static Random _localInstance;

                public RandomNumberGenerator()
                :base()
                {

                }

                public static Random Instance
                {
                    get
                    {
                        Random inst = _localInstance;
                        if (inst == null)
                        {
                            int seed;
                            lock (_global) seed = _global.Next();
                            _localInstance = inst = new Random(seed);
                        }
                        return _localInstance;
                    }
                }
            }

            /// <summary>
            /// Searches for the minimum value in an array of numbers.
            /// </summary>
            /// <param name="ArrNumbers"></param>
            /// <returns></returns>
            public static double Minimum(double[] ArrNumbers)
            {
                double number = ArrNumbers[0];
                foreach (double value in ArrNumbers)
                {
                    if (value < number) number = value;
                }
                return number;
            }

            public static void World_Angles(Rhino.Geometry.Point3d Src, Rhino.Geometry.Point3d Rec, bool degrees, out double alt, out double azi)
            {
                Rhino.Geometry.Vector3d V = Src - Rec;
                V.Unitize();
                azi = Math.Atan2(V.Y, V.X);
                alt = Math.Asin(V.Z);

                while (azi < 0) azi += 2 * Math.PI;

                if (degrees)
                {
                    azi *= (180 / Math.PI);
                    alt *= (180 / Math.PI);
                }
            }

            public static Hare.Geometry.Vector Rotate_Vector(Hare.Geometry.Vector V, double azi, double alt, bool degrees)
            {
                double yaw, pitch;
                if (degrees)
                {
                    yaw = Math.PI * alt / 180.0;
                    pitch = Math.PI * azi / 180.0;
                }
                else 
                {
                    yaw = alt;
                    pitch = azi;
                }

                ///Implicit Sparse Rotation Matrix
                double[] r1 = new double[3] { Math.Cos(pitch) * Math.Cos(yaw) , Math.Sin(pitch) * Math.Cos(yaw) , Math.Sin(yaw) };
                Hare.Geometry.Vector fwd = new Hare.Geometry.Vector(r1[0], r1[1], r1[2]);
                Hare.Geometry.Vector up = new Hare.Geometry.Vector(0, 0, 1) - Hare.Geometry.Hare_math.Dot(new Hare.Geometry.Vector(0, 0, 1), fwd) * fwd;
                up.Normalize();
                double[] r3 = new double[3] { up.x, up.y, up.z };
                Hare.Geometry.Vector right = Hare.Geometry.Hare_math.Cross(up, fwd);
                double[] r2 = new double[3] { right.x, right.y, right.z };

                return (new Hare.Geometry.Vector(r1[0] * V.x + r1[1] * V.y + r1[2] * V.z, r2[0] * V.x + r2[1] * V.y + r2[2] * V.z, r3[0] * V.x + r3[1] * V.y + r3[2] * V.z));
            }

            /// <summary>
            /// obtains the octave band index by string input.
            /// </summary>
            /// <param name="choice"></param>
            /// <returns></returns>
            public static int OctaveStr2Int(string choice)
            {
                switch (choice)
                {
                    case "Summation: All Octaves":
                        return 8;
                    case "62.5 Hz.":
                        return 0;
                    case "125 Hz.":
                        return 1;
                    case "250 Hz.":
                        return 2;
                    case "500 Hz.":
                        return 3;
                    case "1 kHz.":
                        return 4;
                    case "2 kHz.":
                        return 5;
                    case "4 kHz.":
                        return 6;
                    case "8 kHz.":
                        return 7;
                }
                return 8;
            }

            /// <summary>
            /// Tool used for debug of voxel grids and bounding boxes.
            /// </summary>
            /// <param name="MinPt"></param>
            /// <param name="MaxPt"></param>
            public static void AddBox(Point3d MinPt, Point3d MaxPt)
            {
                Rhino.RhinoDoc.ActiveDoc.Objects.AddBrep((new BoundingBox(MinPt, MaxPt)).ToBrep());
            }

            public static System.Drawing.Color HsvColor(double h, double S, double V)
            {
                // ######################################################################
                // T. Nathan Mundhenk
                // mundhenk@usc.edu
                // C/C++ Macro HSV to RGB
                double H = h * 180 / Math.PI;
                while (H < 0) { H += 360; };
                while (H >= 360) { H -= 360; };
                double R, G, B;
                if (V <= 0)
                { R = G = B = 0; }
                else if (S <= 0)
                {
                    R = G = B = V;
                }
                else
                {
                    double hf = H / 60.0;
                    int i = (int)Math.Floor(hf);
                    double f = hf - i;
                    double pv = V * (1 - S);
                    double qv = V * (1 - S * f);
                    double tv = V * (1 - S * (1 - f));
                    switch (i)
                    {

                        // Red is the dominant color
                        case 0:
                            R = V;
                            G = tv;
                            B = pv;
                            break;

                        // Green is the dominant color

                        case 1:
                            R = qv;
                            G = V;
                            B = pv;
                            break;
                        case 2:
                            R = pv;
                            G = V;
                            B = tv;
                            break;

                        // Blue is the dominant color

                        case 3:
                            R = pv;
                            G = qv;
                            B = V;
                            break;
                        case 4:
                            R = tv;
                            G = pv;
                            B = V;
                            break;

                        // Red is the dominant color

                        case 5:
                            R = V;
                            G = pv;
                            B = qv;
                            break;

                        // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                        case 6:
                            R = V;
                            G = tv;
                            B = pv;
                            break;
                        case -1:
                            R = V;
                            G = pv;
                            B = qv;
                            break;

                        // The color is not defined, we should throw an error.

                        default:
                            //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                            R = G = B = V; // Just pretend its black/white
                            break;
                    }
                }
                int r = (int)(R * 255.0);
                int g = (int)(G * 255.0);
                int b = (int)(B * 255.0);
                return System.Drawing.Color.FromArgb(255, r<0?0:r>255?255:r, g<0?0:g>255?255:g, b<0?0:b>255?255:b);
            }

            /// <summary>
            /// Shorthand tool used to run a simulation.
            /// </summary>
            /// <param name="Sim">the simulation to run...</param>
            /// <returns>the completed simulation...</returns>
            public static Simulation_Type Run_Simulation(Simulation_Type Sim)
            {
                UI.PachydermAc_PlugIn plugin = UI.PachydermAc_PlugIn.Instance;
                UI.Pach_RunSim_Command command = UI.Pach_RunSim_Command.Instance;
                if (command == null) { return null; }
                command.Reset();
                command.Sim = Sim;
                Rhino.RhinoApp.RunScript("Run_Simulation", false);
                return command.Sim;
            }

            /// <summary>
            /// Shorthand tool to obtain Rhino_Scene object.
            /// </summary>
            /// <param name="Rel_Humidity">in percent</param>
            /// <param name="AirTempC">in degrees C.</param>
            /// <param name="AirPressurePa">in Pascals</param>
            /// <param name="AirAttenMethod"></param>
            /// <param name="EdgeFreq">Use edge frequency correction?</param>
            /// <returns></returns>
            public static Environment.RhCommon_Scene Get_NURBS_Scene(double Rel_Humidity, double AirTempC,double AirPressurePa, int AirAttenMethod, bool EdgeFreq)
            {
                Rhino.DocObjects.ObjectEnumeratorSettings settings = new Rhino.DocObjects.ObjectEnumeratorSettings();
                settings.DeletedObjects = false;
                settings.HiddenObjects = false;
                settings.LockedObjects = true;
                settings.NormalObjects = true;
                settings.VisibleFilter = true;
                settings.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep & Rhino.DocObjects.ObjectType.Surface & Rhino.DocObjects.ObjectType.Extrusion;
                List<Rhino.DocObjects.RhinoObject> RC_List = new List<Rhino.DocObjects.RhinoObject>();
                foreach (Rhino.DocObjects.RhinoObject RHobj in Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(settings))
                {
                    if (RHobj.ObjectType == Rhino.DocObjects.ObjectType.Brep || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Surface || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        RC_List.Add(RHobj);
                    }
                }
                if (RC_List.Count != 0)
                {
                    return new Environment.RhCommon_Scene(RC_List, AirTempC, Rel_Humidity, AirPressurePa, AirAttenMethod, EdgeFreq, false);
                }
                return null;
            }

            /// <summary>
            /// Shorthand tool to obtain Polygon_Scene object.
            /// </summary>
            /// <param name="Rel_Humidity">in percent</param>
            /// <param name="AirTempC">in degrees C.</param>
            /// <param name="AirPressurePa">in Pascals</param>
            /// <param name="AirAttenMethod"></param>
            /// <param name="EdgeFreq">Use edge frequency correction?</param>
            /// <returns></returns>
            public static Environment.Polygon_Scene Get_Poly_Scene(double Rel_Humidity, double AirTempC, double AirPressurePa, int AirAttenMethod, bool EdgeFreq)
            {
                Rhino.DocObjects.ObjectEnumeratorSettings settings = new Rhino.DocObjects.ObjectEnumeratorSettings();
                settings.DeletedObjects = false;
                settings.HiddenObjects = false;
                settings.LockedObjects = true;
                settings.NormalObjects = true;
                settings.VisibleFilter = true;
                settings.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep & Rhino.DocObjects.ObjectType.Surface & Rhino.DocObjects.ObjectType.Extrusion;
                List<Rhino.DocObjects.RhinoObject> RC_List = new List<Rhino.DocObjects.RhinoObject>();
                foreach (Rhino.DocObjects.RhinoObject RHobj in Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(settings))
                {
                    if (RHobj.ObjectType == Rhino.DocObjects.ObjectType.Brep || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Surface || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        RC_List.Add(RHobj);
                    }
                }
                if (RC_List.Count != 0)
                {
                    return new Environment.Polygon_Scene(RC_List, AirTempC, Rel_Humidity, AirPressurePa, AirAttenMethod, EdgeFreq, false);
                }
                return null;
            }

            public static void Plot_Hare_Topology(Hare.Geometry.Topology T)
            {
                Mesh m_RhinoMesh = new Mesh();
                int ct = 0;
                for (int i = 0; i < T.Polygon_Count; i++)
                {
                    Hare.Geometry.Point[] Pt = T.Polygon_Vertices(i);
                    int[] F = new int[T.Polys[i].VertextCT];
                    for (int j = 0; j < T.Polys[i].VertextCT; j++)
                    {
                        m_RhinoMesh.Vertices.Add(new Point3d(Pt[j].x, Pt[j].y, Pt[j].z) / 90);
                        F[j] = ct;
                        ct++;
                    }
                    if (F.Length == 3)
                    {
                        m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0]);
                    }
                    else
                    {
                        m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0], F[3]);
                    }
                }
                m_RhinoMesh.FaceNormals.ComputeFaceNormals();
                m_RhinoMesh.Normals.ComputeNormals();
                Rhino.RhinoDoc.ActiveDoc.Objects.Add(m_RhinoMesh);
            }

            public static Mesh Hare_to_RhinoMesh(Hare.Geometry.Topology T)
            {
                Mesh m_RhinoMesh = new Mesh();
                int ct = 0;
                for (int i = 0; i < T.Polygon_Count; i++)
                {
                    Hare.Geometry.Point[] Pt = T.Polygon_Vertices(i);
                    int[] F = new int[T.Polys[i].VertextCT];
                    for (int j = 0; j < T.Polys[i].VertextCT; j++)
                    {
                        m_RhinoMesh.Vertices.Add(new Point3d(Pt[j].x, Pt[j].y, Pt[j].z) / 90);
                        F[j] = ct;
                        ct++;
                    }
                    if (F.Length == 3)
                    {
                        m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0]);
                    }
                    else
                    {
                        m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0], F[3]);
                    }
                }
                m_RhinoMesh.Normals.ComputeNormals();
                m_RhinoMesh.Vertices.CombineIdentical(true, true);
                m_RhinoMesh.Compact();
                return m_RhinoMesh;
            }

            /// <summary>
            /// Shorthand tool for map mesh objects.
            /// </summary>
            /// <param name="Map_Srf">A NURBS surface to mesh.</param>
            /// <param name="Increment">the maximum dimension between vertices.</param>
            /// <returns>the map mesh object.</returns>
            public static Mesh Create_Map_Mesh(IEnumerable<Brep> Map_Srf, double Increment)
            {
                Mesh Map_Mesh = new Mesh();
                MeshingParameters mp = new MeshingParameters();
                mp.MaximumEdgeLength = Increment;
                mp.MinimumEdgeLength = Increment;
                mp.SimplePlanes = false;
                mp.JaggedSeams = false;
                Brep[] Srfs = Map_Srf.ToArray<Brep>();
                for (int i = 0; i < Map_Srf.ToArray<Brep>().Length; i++) Map_Mesh.Append(Rhino.Geometry.Mesh.CreateFromBrep(Srfs[i], mp)[0]);
                return Map_Mesh;
            }

            /// <summary>
            /// Sets materials for an object by object.
            /// </summary>
            /// <param name="ID">object GUID (UUID)</param>
            /// <param name="Abs">0 to 100</param>
            /// <param name="Scat">0 to 100</param>
            /// <param name="Trans">0 to 100</param>
            /// <returns></returns>
            public static bool Material_SetByObject(Guid ID, int[] Abs, int[] Scat, int[] Trans)
            {
                Rhino.DocObjects.RhinoObject obj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(ID);
                obj.Geometry.SetUserString("Acoustics_User", "yes");
                string MaterialCode = Pachyderm_Acoustic.UI.PachydermAc_PlugIn.EncodeAcoustics(Abs, Scat, Trans);
                bool result = obj.Geometry.SetUserString("Acoustics", MaterialCode);
                //Rhino.RhinoDoc.ActiveDoc.Objects.ModifyAttributes(obj, obj.Attributes, true);
                return result;
            }
            
            /// <summary>
            /// Sets materials for an object by layer.
            /// </summary>
            /// <param name="ID">object GUID (UUID)</param>
            /// <returns></returns>
            public static bool Material_SetObjectToLayer(Guid ID)
            {
                Rhino.DocObjects.RhinoObject obj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(ID);
                bool result = obj.Geometry.SetUserString("Acoustics_User", "no");
                Rhino.RhinoDoc.ActiveDoc.Objects.ModifyAttributes(obj, obj.Attributes, true);
                return result;
            }

            /// <summary>
            /// Sets the material association for a layer. Note that Transparency can not be set for a layer.
            /// </summary>
            /// <param name="LayerName"></param>
            /// <param name="Abs">0 to 100</param>
            /// <param name="Scat">0 to 100</param>
            /// <returns></returns>
            public static bool Material_SetLayer(string LayerName, int[] Abs, int[] Scat)
            {
                int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerName, true);
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                layer.SetUserString("Acoustics", Pachyderm_Acoustic.UI.PachydermAc_PlugIn.EncodeAcoustics(Abs, Scat, new int[1]));
                return Rhino.RhinoDoc.ActiveDoc.Layers.Modify(layer, layer_index, false);
            }

            /// <summary>
            /// Obtain source objects already in model.
            /// </summary>
            /// <returns></returns>
            public static Point3d[] GetSource()
            {
                UI.PachydermAc_PlugIn p = UI.PachydermAc_PlugIn.Instance;
                Point3d[] SPT;
                p.SourceOrigin(out SPT);
                return SPT;
            }

            /// <summary>
            /// Obtain source objects already in model.
            /// </summary>
            /// <returns></returns>
            public static Source[] GetSource(int No_of_Rays)
            {
                UI.PachydermAc_PlugIn p = UI.PachydermAc_PlugIn.Instance;
                Source[] Srcs;
                p.Source(out Srcs);
                return Srcs;
            }

            /// <summary>
            /// Codes a string for the user key "SourcePower"
            /// </summary>
            /// <param name="SWL">8 number double array of sound power levels.</param>
            public static string EncodeSourcePower(double[] SWL)
            {
                string code = "";
                for (int i = 0; i < SWL.Length; i++)
                {
                    code += SWL[i].ToString() + ";";
                }
                return code;
            }

            public static string EncodeSourcePower(float[] SWL)
            {
                string code = "";
                for (int i = 0; i < SWL.Length; i++)
                {
                    code += SWL[i].ToString() + ";";
                }
                return code;
            }

            /// <summary>
            /// Takes a SourcePower user string and extracts sound power levels.
            /// </summary>
            /// <param name="code">Rhino source power user string.</param>
            /// <returns></returns>
            public static double[] DecodeSourcePower(string code)
            {
                string[] SWLCodes = code.Split(";".ToCharArray());

                if (SWLCodes.Length < 8) return new double[] { 120, 120, 120, 120, 120, 120, 120, 120 };
                double[] SWL = new double[SWLCodes.Length];
                for (int i = 0; i < 8; i++)
                {                                         
                    SWL[i] = double.Parse(SWLCodes[i]);
                }
                return SWL;
            }

            /// <summary>
            /// Add two points together.    `
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3d PT, Point3d PT2)
            {
                return new Point3d(PT.X + PT2.X ,PT.Y + PT2.Y,PT.Z + PT2.Z);
            }

            /// <summary>
            /// Add two points together.
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3f PT, Point3d PT2)
            {
                return new Point3d(PT.X + PT2.X, PT.Y + PT2.Y, PT.Z + PT2.Z);
            }

            /// <summary>
            /// Add two points together.
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3d PT, Vector3d PT2)
            {
                return new Point3d(PT.X + PT2.X, PT.Y + PT2.Y, PT.Z + PT2.Z);
            }

            /// <summary>
            /// Add two points together.
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3f PT, Vector3d PT2)
            {
                return new Point3d(PT.X + PT2.X, PT.Y + PT2.Y, PT.Z + PT2.Z);
            }

            /// <summary>
            /// Convenient Point3d cast.
            /// </summary>
            /// <param name="PT"></param>
            /// <returns></returns>
            public static Point3d castPoint3d(Point3f PT)
            {
                return new Point3d(PT.X, PT.Y, PT.Z);
            }

            /// <summary>
            /// Obtain receiver objects already in model.
            /// </summary>
            /// <returns></returns>
            public static List<Point3d> GetReceivers()
            {
                UI.PachydermAc_PlugIn p = UI.PachydermAc_PlugIn.Instance;
                List<Point3d> RPT;
                p.Receiver(out RPT);
                return RPT;
            }

            /// <summary>
            /// Easily constructs a receiver bank from custom data. Hard codes sample resolution of simulation to 1000 samples per second.
            /// </summary>
            /// <param name="ReceiverLocations">enumerable of receiver pts.</param>
            /// <param name="SrcLocations">enumerable of source pts.</param>
            /// <param name="No_of_Rays">number of rays</param>
            /// <param name="CutOffTime">number of milliseconds in simulation</param>
            /// <param name="Typ">0 for stationary, 1 for variable.</param>
            /// <param name="Sc">Scene object contains air attenuation, and speed of sound.</param>
            /// <returns></returns>
            public static List<Receiver_Bank> GetReceivers(IEnumerable<Point3d> ReceiverLocations, IEnumerable<Point3d> SrcLocations, int No_of_Rays, int CutOffTime, int Typ, Scene Sc)
            {
                Receiver_Bank.Type RecType;
                switch (Typ)
                {
                    case 0:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                    case 1:
                        RecType = Receiver_Bank.Type.Variable;
                        break;
                    default:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                }

                List<Receiver_Bank> R = new List<Receiver_Bank>();

                for (int i = 0; i < SrcLocations.Count<Point3d>(); i++)
                {
                     R.Add(new Receiver_Bank(ReceiverLocations, SrcLocations.ElementAt<Point3d>(i), Sc, 1000, CutOffTime, RecType));
                }

                return R; 
            }

            /// <summary>
            /// Easily constructs a receiver bank from custom data.
            /// </summary>
            /// <param name="ReceiverLocations">enumerable of receiver pts.</param>
            /// <param name="SrcLocations">enumerable of source pts.</param>
            /// <param name="No_of_Rays">number of rays</param>
            /// <param name="CutOffTime">number of milliseconds in simulation</param>
            /// <param name="Typ">0 for stationary, 1 for variable.</param>
            /// <param name="Sc">Scene object contains air attenuation, and speed of sound.</param>
            /// <returns></returns>
            public static List<Receiver_Bank> GetReceivers(IEnumerable<Point3d> ReceiverLocations, IEnumerable<Point3d> SrcLocations, int No_of_Rays, int CutOffTime, int Typ, Scene Sc, int sample_rate)
            {
                Receiver_Bank.Type RecType;
                switch (Typ)
                {
                    case 0:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                    case 1:
                        RecType = Receiver_Bank.Type.Variable;
                        break;
                    default:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                }

                List<Receiver_Bank> R = new List<Receiver_Bank>();

                for (int i = 0; i < SrcLocations.Count<Point3d>(); i++)
                {
                    R.Add(new Receiver_Bank(ReceiverLocations, SrcLocations.ElementAt<Point3d>(i), Sc, sample_rate, CutOffTime, RecType));
                }

                return R;
            }

            /// <summary>
            /// Creates color map mesh from user defined parameters and a data set.
            /// </summary>
            /// <param name="mesh">the mesh to use for parameter mapping.</param>
            /// <param name="scale_enum">an integer indicating which color scale to use.</param>
            /// <param name="Values">the list of values calculated for each point.</param>
            /// <param name="LBound">the lower bound of the scale.</param>
            /// <param name="UBound">the upper bound of the scale.</param>
            /// <returns>the colored map.</returns>
            public static string CreateMap(Mesh mesh, int scale_enum, double[] Values, double LBound, double UBound)
            {
                double H_OFFSET;
                double H_BREADTH;
                double S_OFFSET;
                double S_BREADTH;
                double V_OFFSET;
                double V_BREADTH;

                if (Values.Length != mesh.Vertices.Count) return System.Guid.Empty.ToString();

                Pach_Graphics.HSV_colorscale c_scale;

                System.Drawing.Color[] Colors;
                switch (scale_enum)
                {
                    case 0:
                        H_OFFSET = 0;
                        H_BREADTH = 4.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1,1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 24);
                        break;
                    case 1:
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 0;
                        H_BREADTH = 1.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    case 2:
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = Math.PI / 3.0;
                        H_BREADTH = 1.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    case 3:
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 0;
                        H_BREADTH = -2.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    case 4:
                        H_OFFSET = 0;
                        H_BREADTH = 0;
                        S_OFFSET = 0;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = -1;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Color selection invalid... Most obnoxious color imaginable substituted!");
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 2.0 * Math.PI / 3;
                        H_BREADTH = 0;
                        S_OFFSET = 1;
                        S_BREADTH = -1;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                }

                double Scale_Breadth = UBound - LBound;

                for (int i = 0; i < Values.Length; i++)
                {
                    System.Drawing.Color color = c_scale.GetValue(Values[i], LBound, UBound);
                    mesh.VertexColors.SetColor(i, color);
                }
                
                return Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(mesh).ToString();
            }

            /// <summary>
            /// Adds dataset to the custom mapping control.
            /// </summary>
            /// <param name="Title">the alias for the dataset.</param>
            /// <param name="Values">the calculated values for each point.</param>
            /// <param name="M">the mesh to be used.</param>
            public static void AddToCMControl(string Title, double[] Values, Mesh M)
            {
                UI.PachydermAc_PlugIn Pach = UI.PachydermAc_PlugIn.Instance;
                UI.Pach_MapCustom.Add_Result(Title, Values, M);
            }

            /// <summary>
            /// Clears the custom mapping control of all stored data.
            /// </summary>
            public static void ClearCMControl()
            {
                UI.PachydermAc_PlugIn Pach = UI.PachydermAc_PlugIn.Instance;
                UI.Pach_MapCustom.Clear();
            }

            /// <summary>
            /// Casts Rhino point to Hare point
            /// </summary>
            /// <param name="Point"></param>
            /// <returns></returns>
            public static Hare.Geometry.Point RPttoHPt(Point3d Point)
            {
                return new Hare.Geometry.Point(Point.X, Point.Y, Point.Z);
            }

            /// <summary>
            /// Casts Hare point to Rhino point
            /// </summary>
            /// <param name="Point"></param>
            /// <returns></returns>
            public static Point3d HPttoRPt(Hare.Geometry.Point Point)
            {
                return new Point3d(Point.x, Point.y, Point.z);
            }

            /// <summary>
            /// Displays custom mapping control.
            /// </summary>
            public static void Show_CM_Control()
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("1c48c00e-abd8-40fd-8642-2ce7daa90ed5"));
            }

            public static class Ray_Acoustics 
            {
                /// <summary>
                /// Calculates the direction of a specular reflection.
                /// </summary>
                /// <param name="R"></param>
                /// <param name="u"></param>
                /// <param name="v"></param>
                /// <param name="Face_ID"></param>
                public static void SpecularReflection(ref Hare.Geometry.Vector R, ref Environment.Scene Room, ref double u, ref double v, ref int Face_ID)
                {
                    Hare.Geometry.Vector local_N = Room.Normal(Face_ID, u, v);
                    R -= local_N * Hare.Geometry.Hare_math.Dot(R, local_N) * 2;
                }                
            }
        }
    }
}