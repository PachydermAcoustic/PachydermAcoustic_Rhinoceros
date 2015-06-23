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

using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;
using Pachyderm_Acoustic.Utilities;
using System.Collections.Generic;
using System.Numerics;
using FFTWSharp;
using System.Linq;

namespace Pachyderm_Acoustic
{
    namespace Audio
    {
        public class Pach_SP
        {
            static System.Threading.Semaphore S = new System.Threading.Semaphore(1, 1);
            //For all standard 2048 sample FFTs...
            static fftw_complexarray[] FFT_ArrayIn2048;
            static fftw_complexarray[] FFT_ArrayOut2048;
            static fftw_plan[] FFT_Plan2048;
            static fftw_complexarray[] IFFT_ArrayIn2048;
            static fftw_complexarray[] IFFT_ArrayOut2048;
            static fftw_plan[] IFFT_Plan2048;

            //For all others...
            static fftw_complexarray[] FFT_ArrayIn;
            static fftw_complexarray[] FFT_ArrayOut;
            static fftw_plan[] FFT_Plan;
            static fftw_complexarray[] IFFT_ArrayIn;
            static fftw_complexarray[] IFFT_ArrayOut;
            static fftw_plan[] IFFT_Plan;

            public static void Initialize_FFTW()
            {
                int proc = System.Environment.ProcessorCount;
                FFT_ArrayIn2048 = new fftw_complexarray[proc];
                FFT_ArrayOut2048 = new fftw_complexarray[proc];
                FFT_Plan2048 = new fftw_plan[proc];
                IFFT_ArrayIn2048 = new fftw_complexarray[proc];
                IFFT_ArrayOut2048 = new fftw_complexarray[proc];
                IFFT_Plan2048 = new fftw_plan[proc];

                FFT_ArrayIn = new fftw_complexarray[proc];
                FFT_ArrayOut = new fftw_complexarray[proc];
                FFT_Plan = new fftw_plan[proc];
                IFFT_ArrayIn = new fftw_complexarray[proc];
                IFFT_ArrayOut = new fftw_complexarray[proc];
                IFFT_Plan = new fftw_plan[proc];
                
                for(int i = 0; i < proc; i++)
                {
                    FFT_ArrayIn2048[i] = new fftw_complexarray(2048);
                    FFT_ArrayOut2048[i] = new fftw_complexarray(2048);
                    FFT_Plan2048[i] = fftw_plan.dft_1d(2048, FFT_ArrayIn2048[i], FFT_ArrayOut2048[i], fftw_direction.Forward, fftw_flags.Exhaustive);
                    IFFT_ArrayIn2048[i] = new fftw_complexarray(2048);
                    IFFT_ArrayOut2048[i] = new fftw_complexarray(2048);
                    IFFT_Plan2048[i] = fftw_plan.dft_1d(2048, IFFT_ArrayIn2048[i], IFFT_ArrayOut2048[i], fftw_direction.Forward, fftw_flags.Exhaustive);                    
                }
            }

            public static void Raised_Cosine_Window(ref double[] h)
            {
                double sum_before = 0;
                double sum_after = 0;

                int div = 4;

                int idl = (div/2 - 1) * h.Length / div;
                int idu = (div/2 + 1) * h.Length / div;

                for(int i = 0; i < idl; i++)
                {
                    h[i] = 0;
                }

                for(int i = idu; i < h.Length; i++)
                {
                    h[i] = 0;
                }

                for (int i = idl; i < idu; i++)
                {
                    sum_before += h[i] * h[i];
                    double weight = (-Math.Cos(((double)(i - idl) / (2 * h.Length / div)) * 2 * Math.PI) + 1) * .5;
                    h[i] *= weight;
                    sum_after += h[i] * h[i];
                }

                double factor = Math.Sqrt(sum_before / sum_after);
                for (int i = 0; i < h.Length; i++) h[i] *= factor;
            }

            public static void Raised_HCosine_Window(ref double[] h)
            {
                double sum_before = 0;
                double sum_after = 0;

                for(int i = 0; i < h.Length; i++)
                {
                    sum_before += h[i] * h[i];
                    double weight = (Math.Cos((1 - Math.Pow(1 - ((double)i / h.Length), 8)) * Math.PI) + 1) * .5;
                    h[i] *= weight;
                    sum_after += h[i] * h[i];
                }

                double factor = Math.Sqrt(sum_before/sum_after);
                for (int i = 0; i < h.Length; i++) h[i] *= factor;
            }

            public static double[] FIR_Bandpass(double[] h, int octave_index, int Sample_Freq, int thread)
            {
                int length = h.Length;

                if (length != 2048)
                {
                    Array.Resize(ref h, (int)Math.Pow(2, Math.Ceiling(Math.Log(h.Length, 2)) + 1));
                }

                double ctr =  62.5 * Math.Pow(2, octave_index);
                double freq_l = ctr / Utilities.Numerics.rt2;
                double freq_u = ctr * Utilities.Numerics.rt2;
                int idl = (int)Math.Round((h.Length * freq_l) / 44100);
                int idu = (int)Math.Round((h.Length * freq_u) / 44100);
                //////////////////////////////////////////
                //Design Butterworth filters with relevant passbands...
                Complex[] magspec = new Complex[h.Length / 2];
                for (int i = 1; i <= magspec.Length; i++)
                {
                    if (i < idl)
                    {
                        magspec[i - 1] = 1 / Math.Sqrt(1 + (Math.Pow((double)idl / (double)i, 6)));
                    }
                    else if (i > idu)
                    {
                        magspec[i - 1] = 1 / Math.Sqrt(1 + (Math.Pow((double)i / (double)idu, 12)));
                    }
                    else
                    {
                        magspec[i - 1] = 1;
                    }
                }

                ///////////////Create Linear Phase bandpass///////////// Too much phase distortion...

                ///////////Use Zero Phase Bandpass//////////////////////
                System.Numerics.Complex[] filter = Mirror_Spectrum(magspec);

                ///////////Use Minimum Phase Bandpass///////////////////
                //System.Numerics.Complex[] filter = Minimum_Phase_TF(magspec, 44100, thread);

                //Convolve signal with Bandpass Filter.
                Complex[] freq_h = FFT_General(h, thread);

                for(int i = 0; i < freq_h.Length; i++)
                {
                    freq_h[i] *= filter[i];
                }

                double[] h_oct = IFFT_Real_General(freq_h, thread);
                Scale(ref h_oct);

                Array.Resize(ref h_oct, length);

                return h_oct;
            }

            //public static System.Numerics.Complex[] FFT(Complex[] Signal, int threadid)
            //{
            //    //FFTW.Net Setup//
            //    FFT_ArrayIn2048[threadid].SetData(Signal);
            //    //FFT_ArrayOut2048[threadid].SetZeroData();
            //    FFT_Plan2048[threadid].Execute();

            //    System.Numerics.Complex[] Out = FFT_ArrayOut2048[threadid].GetData_Complex();
            //    return Out;
            //}

            public static System.Numerics.Complex[] FFT(double[] Signal, int threadid)
            {
                double[] Sig_complex = new double[Signal.Length * 2];
                for (int i = 0; i < Signal.Length; i++) Sig_complex[i * 2] = Signal[i];

                //FFTW.Net Setup//
                FFT_ArrayIn[threadid].SetData(Sig_complex);
                //FFT_ArrayOut2048[threadid].SetZeroData();
                FFT_Plan[threadid].Execute();

                System.Numerics.Complex[] Out = FFT_ArrayOut2048[threadid].GetData_Complex();
                return Out;
            }

            public static double[] IFFT_Real(System.Numerics.Complex[] spectrum, int threadid)
            {
                double[] samplep = new double[spectrum.Length * 2];
                for (int i = 0; i < spectrum.Length; i++)
                {
                    samplep[2 * i] = spectrum[i].Real;
                    samplep[2 * i + 1] = spectrum[i].Imaginary;
                }

                //FFTW.Net Setup//
                IFFT_ArrayIn2048[threadid].SetData(samplep);
                IFFT_Plan2048[threadid].Execute();

                double[] Out = IFFT_ArrayOut2048[threadid].GetData_Real();

                return Out;
            }

            public static Complex[] IFFT(System.Numerics.Complex[] spectrum, int threadid)
            {
                double[] samplep = new double[spectrum.Length * 2];
                for (int i = 0; i < spectrum.Length; i++)
                {
                    samplep[2 * i] = spectrum[i].Real;
                    samplep[2 * i + 1] = spectrum[i].Imaginary;
                }

                //FFTW.Net Setup//
                IFFT_ArrayIn2048[threadid].SetData(samplep);
                IFFT_Plan2048[threadid].Execute();

                System.Numerics.Complex[] Out = IFFT_ArrayOut2048[threadid].GetData_Complex();

                return Out;
            }

            public static System.Numerics.Complex[] FFT_General(Complex[] Signal, int threadid)
            {
                //FFTW.Net Setup//
                FFT_ArrayIn[threadid] = new fftw_complexarray(Signal);
                FFT_ArrayOut[threadid]= new fftw_complexarray(Signal.Length);
                FFT_Plan[threadid] = fftw_plan.dft_1d(Signal.Length, FFT_ArrayIn[threadid], FFT_ArrayOut[threadid], fftw_direction.Forward, fftw_flags.Estimate);
                FFT_Plan[threadid].Execute();

                System.Numerics.Complex[] Out = FFT_ArrayOut[threadid].GetData_Complex();

                return Out;
            }

            public static System.Numerics.Complex[] FFT_General(double[] Signal, int threadid)
            {
                double[] Sig_complex = new double[Signal.Length * 2];
                for (int i = 0; i < Signal.Length; i++) Sig_complex[i * 2] = Signal[i];

                //FFTW.Net Setup//
                FFT_ArrayIn[threadid] = new fftw_complexarray(Sig_complex);
                FFT_ArrayOut[threadid] = new fftw_complexarray(Signal.Length);
                FFT_Plan[threadid] = fftw_plan.dft_1d(Signal.Length, FFT_ArrayIn[threadid], FFT_ArrayOut[threadid], fftw_direction.Forward, fftw_flags.Estimate);

                FFT_Plan[threadid].Execute();

                System.Numerics.Complex[] Out = FFT_ArrayOut[threadid].GetData_Complex();

                return Out;
            }

            public static double[] IFFT_Real_General(System.Numerics.Complex[] spectrum, int threadid)
            {
                double[] samplep = new double[spectrum.Length * 2];
                for (int i = 0; i < spectrum.Length; i++)
                {
                    samplep[2 * i] = spectrum[i].Real;
                    samplep[2 * i + 1] = spectrum[i].Imaginary;
                }

                //FFTW.Net Setup//
                IFFT_ArrayIn[threadid] = new fftw_complexarray(samplep);
                IFFT_ArrayOut[threadid] = new fftw_complexarray(spectrum.Length);
                IFFT_Plan[threadid] = fftw_plan.dft_1d(spectrum.Length, IFFT_ArrayIn[threadid], IFFT_ArrayOut[threadid], fftw_direction.Backward, fftw_flags.Estimate);

                IFFT_Plan[threadid].Execute();

                double[] Out = IFFT_ArrayOut[threadid].GetData_Real();

                return Out;
            }

            public static Complex[] IFFT_General(System.Numerics.Complex[] spectrum, int threadid)
            {
                double[] samplep = new double[spectrum.Length * 2];
                for (int i = 0; i < spectrum.Length; i++)
                {
                    samplep[2 * i] = spectrum[i].Real;
                    samplep[2 * i + 1] = spectrum[i].Imaginary;
                }

                //FFTW.Net Setup//
                IFFT_ArrayIn[threadid] = new fftw_complexarray(samplep);
                IFFT_ArrayOut[threadid] = new fftw_complexarray(spectrum.Length);
                IFFT_Plan[threadid] = fftw_plan.dft_1d(spectrum.Length, IFFT_ArrayIn[threadid], IFFT_ArrayOut[threadid], fftw_direction.Backward, fftw_flags.Estimate);

                IFFT_Plan[threadid].Execute();

                System.Numerics.Complex[] Out = IFFT_ArrayOut[threadid].GetData_Complex();

                return Out;
            }

            public static Complex[] Mirror_Spectrum(double[] spectrum)
            {
                //Mirror Spectrum
                Complex[] samplep = new Complex[spectrum.Length * 2];
                samplep[0] = spectrum[0];
                samplep[spectrum.Length] = spectrum[spectrum.Length - 1];
                for (int i = 1; i < spectrum.Length; i++)
                {
                    samplep[i] = spectrum[i];
                    samplep[samplep.Length - i] = spectrum[i];
                }
                return samplep;
            }

            public static Complex[] Mirror_Spectrum(Complex[] spectrum)
            {
                //Mirror Spectrum
                Complex[] samplep = new Complex[spectrum.Length * 2];
                samplep[0] = spectrum[0];
                samplep[spectrum.Length] = spectrum[spectrum.Length -1];
                for (int i = 1; i < spectrum.Length; i++)
                {
                    samplep[i] = spectrum[i];
                    samplep[samplep.Length - i] = Complex.Conjugate(spectrum[i]);
                }
                return samplep;
            }

            public static void ScaleRoot(ref Complex[] IN)
            {
                double factor = Math.Sqrt(IN.Length);
                for (int i = 0; i < IN.Length; i++) IN[i] /= factor;
            }

            public static void ScaleRoot(ref double[] IN)
            {
                double factor = Math.Sqrt(IN.Length);
                for (int i = 0; i < IN.Length; i++) IN[i] /= factor;
            }

            public static void Scale(ref Complex[] IN)
            {
                for (int i = 0; i < IN.Length; i++) IN[i] /= IN.Length;
            }

            public static void Scale(ref double[] IN)
            {
                for (int i = 0; i < IN.Length; i++) IN[i] /= IN.Length;
            }

            public static double[] Magnitude_Spectrum(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            {
                double[] p_i = new double[length_starttofinish / 2];
                double df = (double)(sample_frequency) / (double)length_starttofinish;

                int octave_index = 0;
                double ctr = 62.5 * Math.Pow(2, octave_index);
                double freq_l = ctr / Utilities.Numerics.rt2;
                double freq_u = ctr * Utilities.Numerics.rt2;
                int idl = (int)Math.Round((length_starttofinish / 2 * freq_l) / 44100);
                int idu = (int)Math.Round((length_starttofinish / 2 * freq_u) / 44100);
                double l = Math.Sqrt((idu - idl)) * Math.Sqrt(2);
                //double[] magspec = new double[length_starttofinish / 2];
                
                for (int i = 1; i < p_i.Length; i++)
                {
                    if(i == idu)
                    {
                        octave_index++;
                        ctr = 62.5 * Math.Pow(2, octave_index);
                        freq_l = ctr / Utilities.Numerics.rt2;
                        freq_u = ctr * Utilities.Numerics.rt2;
                        idl = (int)Math.Round((length_starttofinish / 2 * freq_l) / 44100);
                        idu = (int)Math.Round((length_starttofinish / 2 * freq_u) / 44100);
                        l = Math.Sqrt(idu - idl) * Math.Sqrt(2);
                    }

                    //double total = 0;
                    //////////////////////////////////////////
                    //Design Raised Cosine filters with relevant passbands...

                    if (octave_index < 8)
                    {
                        p_i[i] = Octave_pressure[octave_index] / l;
                    }
                    else 
                    {
                        p_i[i] = Octave_pressure[7] / l;
                    }
                    //if (i < idl && octave_index != 0)
                    //{
                    //    magspec[i - 1] = 1 / Math.Sqrt(1 + (Math.Pow((double)idl / (double)i, 6))) / l;
                    //}
                    //else if (i > idu && octave_index != 7)
                    //{
                    //    magspec[i - 1] = 1 / Math.Sqrt(1 + (Math.Pow((double)i / (double)idu, 12))) / l;
                    //}
                    //else
                    //{
                    //    magspec[i - 1] = 1 / l;
                    //}
                    //total += magspec[i - 1];
                }
                    //////////////////////////////////////////
                    //double mod = Octave_pressure[octave_index] / Math.Sqrt((idu - idl) / df);// / total;
                    //for (int i = 0; i < magspec.Length; i++)
                    //{
                    //    p_i[i] += magspec[i] * mod;
                    //}
                


                //for (int octave_index = 0; octave_index < 8; octave_index++)
                //{
                //    double ctr = 62.5 * Math.Pow(2, octave_index);
                //    double freq_l = ctr / Utilities.Numerics.rt2;
                //    double freq_u = ctr * Utilities.Numerics.rt2;
                //    int idl = (int)Math.Round((length_starttofinish / 2 * freq_l) / 44100);
                //    int idu = (int)Math.Round((length_starttofinish / 2 * freq_u) / 44100);

                //    double total = 0;
                //    //////////////////////////////////////////
                //    //Design Raised Cosine filters with relevant passbands...
                //    double[] magspec = new double[length_starttofinish / 2];
                //    int l = idu - idl;
                //    for (int i = 1; i <= magspec.Length; i++)
                //    {
                //        if (i < idl && octave_index != 0)
                //        {
                //            magspec[i - 1] = 1 / Math.Sqrt(1 + (Math.Pow((double)idl / (double)i, 6))) / l;
                //        }
                //        else if (i > idu && octave_index != 7)
                //        {
                //            magspec[i - 1] = 1 / Math.Sqrt(1 + (Math.Pow((double)i / (double)idu, 12))) / l;
                //        }
                //        else
                //        {
                //            magspec[i - 1] = 1 / l;
                //        }
                //        //total += magspec[i - 1];
                //    }
                //    //////////////////////////////////////////
                //    double mod = Octave_pressure[octave_index] / Math.Sqrt((idu - idl) / df);// / total;
                //    for (int i = 0; i < magspec.Length; i++)
                //    {
                //        p_i[i] += magspec[i] * mod;
                //    }
                //}

                //double[] p = new double[p_i.Length];
                ///Smooth

                //for (int i = 0; i < p_i.Length; i++) p_i[i] = Math.Log10(p_i[i]);

                //for (int i = 0; i < p_i.Length; i++)
                //{
                //    double total = 0;

                //    //double[] gauss = new double[(int)Math.Ceiling(Math.Log((i + 1)*df, 31.25))];
                //    double[] gauss = new double[p_i.Length / 10];
                //    int b = (int)Math.Ceiling(gauss.Length / 2d);
                //    double c = Math.Ceiling(gauss.Length / 6f);

                //    for (int x = 0; x < gauss.Length; x++)
                //    {
                //        double xb = x - b;
                //        gauss[x] = System.Math.Exp(-(xb * xb) / (2 * c * c));
                //        total += gauss[x];
                //    }

                //    total = Math.Sqrt(total);
                //    int ib = i + b;
                //    for (int x = 0; x < gauss.Length; x++)
                //    {
                //        if (ib + x >= p_i.Length) break;
                //        p[ib + x] += Math.Pow(10, gauss[x] * p_i[i] / total);
                //    }
                //}

                return p_i;
            }

            //public static double[] Magnitude_Spectrum(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            //{
            //    double rt2 = Math.Sqrt(2);
            //    int spec_length = length_starttofinish;
            //    double df = (double)(sample_frequency) / (double)spec_length;

            //    List<double> f = new List<double>();
            //    List<double> pr = new List<double>();

            //    for (int oct = 0; oct < 7; oct++) if (Octave_pressure[oct] == 0) Octave_pressure[oct] = double.Epsilon;

            //    double s = Math.Abs(62.5 * Math.Pow(2, 0) / rt2 - 62.5 * Math.Pow(2, 0) * rt2) / df;
            //    double BASE = Math.Pow(Octave_pressure.Max() / Octave_pressure.Min(), 2);

            //    f.Add(df);
            //    pr.Add((2 * Octave_pressure[0] * Octave_pressure[0] - Octave_pressure[0] * Octave_pressure[0]) / (62.5/df));
                
            //    double[] fl = new double[8], fu = new double[8], bw = new double[8];
            //    double[] samples = new double[8];
            //    for (int oct = 0; oct < 8; oct++)
            //    {
            //        double fr = 62.5 * Math.Pow(2, oct);
            //        fl[oct] = fr / rt2;
            //        fu[oct] = fr * rt2;
            //        bw[oct] = fu[oct] - fl[oct];
            //        samples[oct] = bw[oct] / df * Math.Pow(2, oct + 1);
            //    }

            //    for (int oct = 0; oct < 8; oct++)
            //    {

            //        double I_next, samples_;
            //        if (oct == 7)
            //        {
            //            samples_ = 2 * (fu[oct] - fl[oct]) / df / Math.Pow(2, oct + 1);
            //            I_next = Octave_pressure[7] * Octave_pressure[7] / (samples_);
            //        }
            //        else
            //        {
            //            samples_ = (fu[oct + 1] - fl[oct + 1]) / df / Math.Pow(2, oct + 1);
            //            I_next = Octave_pressure[oct + 1] * Octave_pressure[oct + 1] / (samples_);
            //        }
            //        double I_c = Octave_pressure[oct] * Octave_pressure[oct] / samples[oct];
            //        double I_u = I_c + (I_next - I_c) / 3;
            //        double I_l = I_c + 2 * (I_c - pr.Last()) / 3;
            //        //double P_c = Octave_pressure[oct] / samples[oct];
            //        //double P_next = (oct < 7) ? Octave_pressure[oct + 1] / (samples[oct] * 2) : Octave_pressure[7] / (samples[oct] * 2);
            //        //double P_u = (P_c + P_next) / 3;
            //        //double P_l = 2 * (pr.Last() + P_c) / 3;
            //        ////Construct a sensitive region with the right amount of total root mean square pressure...
            //        double Itot = Octave_pressure[oct] * Octave_pressure[oct]; //total power in each half of the region of interest...
            //        //double Ptot = Octave_pressure[oct] / 2; //total power in each half of the region of interest...

            //        //double I_current = Math.Pow(BASE, spl_current);
            //        //double I_next = Math.Pow(BASE, spl_next);
            //        //I_current *= I_current;
            //        //I_next *= I_next;
            //        //double I_l = Math.Pow(BASE, pr.Last());
            //        //double I_u = (I_current + I_next) / 3;

            //        //I_l *= I_l;
            //        //I_u *= I_u;
            //        //double Itot = Octave_pressure[oct] * Octave_pressure[oct];

            //        if (samples[oct] < 4)
            //        {
            //            f.Add(fl[oct] + bw[oct] / 2);
            //            f.Add(fu[oct]);
            //            pr.Add(I_c);
            //            pr.Add(I_u);
            //        }
            //        else
            //        {
            //            f.Add(fl[oct] + bw[oct] / 4);
            //            f.Add(fl[oct] + bw[oct] / 2);
            //            f.Add(fl[oct] + 3 * bw[oct] / 4);
            //            f.Add(fu[oct]);
            //            pr.Add(2 * Itot / (samples[oct]) - (I_l + I_c) / 2);
            //            pr.Add(I_c);
            //            pr.Add(2 * Itot / (samples[oct]) - (I_u + I_c) / 2);
            //            pr.Add(I_u);
            //        }
            //        //pr.Add(2 * Ptot / Math.Sqrt(samples[oct]) - (P_l + P_c) / 2);
            //        //pr.Add(P_c);
            //        //pr.Add(2 * Ptot / Math.Sqrt(samples[oct]) - (P_u + P_c) / 2);
            //        //pr.Add(P_u);
            //        //pr.Add(2 * Itot / (samples[oct]) - (I_l + I_c) / 2);
            //        //pr.Add(I_c);
            //        //pr.Add(2 * Itot / (samples[oct]) - (I_u + I_c) / 2);
            //        //pr.Add(I_u);
            //        //pr.Add(Math.Log(Math.Sqrt(Itot / (samples[oct]) - (I_l + I_c) / 2), BASE));
            //        //pr.Add(spl_current);
            //        //pr.Add(Math.Log(Math.Sqrt(Itot / (samples[oct]) - (I_u + I_c) / 2), BASE));
            //        //pr.Add(Math.Log(Math.Sqrt(I_u)));
            //    }

            //    double freq = 0;
            //    int octave = 7;
            //    while (freq < sample_frequency / 2)
            //    {
            //        octave++;
            //        freq = 62.5 * Math.Pow(2, octave);
            //        double fl_ = freq / rt2, fu_ = freq * rt2;
            //        double bw_ = fu_ - fl_;
            //        double samples_ = bw_ / df;

            //        f.Add((fl_ + fu_) / 2);
            //        //pr.Add(Octave_pressure[7] * Octave_pressure[7] / (samples_));
            //        pr.Add(Octave_pressure[7] * Octave_pressure[7] / samples_ * Math.Pow(2, octave + 1));
            //    }

            //    for(int i = 0; i < pr.Count; i++) pr[i] = Math.Log(pr[i], BASE);

            //    double[] pra = pr.ToArray();

            //    MathNet.Numerics.Interpolation.CubicSpline prm = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(f.ToArray(), pra);

            //    double[] p_i = new double[spec_length / 2];

            //    for (int j = 0; j < spec_length / 2; j++)
            //    {
            //        double fr = (j + 1) * df;
            //        p_i[j] = Math.Pow(BASE, prm.Interpolate(fr));// / (1 + Math.Exp(-(fr - 44))));// / (fr);
            //    }

            //    //    ///Smooth
            //    double[] p = new double[p_i.Length];
            //    for (int i = 0; i < p_i.Length; i++)
            //    {
            //        double total = 0;

            //        //double[] gauss = new double[(int)Math.Ceiling(Math.Log((i + 1)*df, 31.25))];
            //        double[] gauss = new double[p_i.Length / 10];
            //        int b = (int)Math.Ceiling(gauss.Length / 2d);
            //        double c = Math.Ceiling(gauss.Length / 6f);

            //        for (int x = 0; x < gauss.Length; x++)
            //        {
            //            double xb = x - b;
            //            gauss[x] = System.Math.Exp(-(xb * xb) / (2 * c * c));
            //            total += gauss[x];
            //        }

            //        total = Math.Sqrt(total);
            //        int ib = i + b;
            //        for (int x = 0; x < gauss.Length; x++)
            //        {
            //            if (ib + x >= p_i.Length) break;
            //            p[ib + x] += Math.Pow(10, gauss[x] * p_i[i] / total);
            //        }
            //    }

            //    return p_i;
            //}

            //public static double[] Magnitude_Spectrum(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            //{
            //    double rt2 = Math.Sqrt(2);
            //    int spec_length = length_starttofinish;
            //    double df = (double)(sample_frequency) / (double)spec_length;

            //    List<double> f = new List<double>();
            //    List<double> pr = new List<double>();

            //    for (int oct = 0; oct < 7; oct++) if (Octave_pressure[oct] == 0) Octave_pressure[oct] = double.Epsilon;

            //    double s = Math.Abs(62.5 * Math.Pow(2, 0) / rt2 - 62.5 * Math.Pow(2, 0) * rt2) / df;
            //    double BASE = Math.Pow(Octave_pressure.Max() / Octave_pressure.Min(), 2);

            //    f.Add(df);
            //    pr.Add(Math.Log(Octave_pressure[0] / Math.Sqrt(2 * s), BASE));
            //    f.Add(31.25 * rt2);
            //    pr.Add(Math.Log(Octave_pressure[0] / Math.Sqrt(2 * s), BASE));
            //    double[] fl = new double[8], fu = new double[8];
            //    int[] samples = new int[8];
            //    double[] fr = new double[8];
            //    for (int oct = 0; oct < 8; oct++)
            //    {
            //        fr[oct] = 62.5 * Math.Pow(2, oct);
            //        fl[oct] = fr[oct] / rt2; fu[oct] = fr[oct] * rt2;
            //        double bw = fu[oct] - fl[oct];
            //        samples[oct] = (int)Math.Ceiling(bw / df);

            //        f.Add((fr[oct] + fl[oct]) / 2);
            //        pr.Add(Math.Log(Octave_pressure[oct] / Math.Sqrt(2 * samples[oct]), BASE));
            //    }

            //    double freq = 0;
            //    int octave = 7;
            //    while (freq < sample_frequency / 2)
            //    {
            //        octave++;
            //        freq = 62.5 * Math.Pow(2, octave);
            //        double fl_ = freq / rt2, fu_ = freq * rt2;
            //        double bw_ = fu_ - fl_;
            //        double samples_ = bw_ / df;

            //        f.Add((fl_ + fu_) / 2);
            //        pr.Add(Math.Log(Octave_pressure[7] / Math.Sqrt(2 * samples_), BASE));
            //    }

            //    MathNet.Numerics.Interpolation.CubicSpline prm = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(f.ToArray(), pr.ToArray());

            //    double[] p_i = new double[spec_length / 2];


            //    for (int j = 0; j < spec_length / 2; j++)
            //    {
            //        double frequency = (j + 1) * df;
            //        p_i[j] = Math.Pow(BASE, prm.Interpolate(frequency));
            //    }

            //    ///TODO: Total prms. Find out the deficiency, and then add on a guaussian correction.
            //    double[] p = new double[p_i.Length];
            //    for (int i = 0; i < p.Length; i++) p[i] = p_i[i] * p_i[i];

            //    //for (int oct = 1; oct < 8; oct++)
            //    //{
            //    //    double total = 0;
            //    //    double[] gauss = new double[samples[oct] * 2];
            //    //    double b = samples[oct] / 2;
            //    //    double c = Math.Ceiling(samples[oct] / 6f);

            //    //    for (int x = 0; x < gauss.Length; x++)
            //    //    {
            //    //        double xb = x - b;
            //    //        gauss[x] = System.Math.Exp(-(xb * xb) / (2 * c * c));
            //    //        total += gauss[x] * gauss[x];
            //    //    }

            //    //    total = Math.Sqrt(total);

            //    //    int offset1 = (int)(fl[oct] / df);
            //    //    int offset2 = (int)(.5 * (fl[oct - 1] + fu[oct - 1]) / df);
            //    //    double octaveP = 0;
            //    //    for (int x = 0; x < gauss.Length * .5; x++) octaveP += p[x + offset1];
            //    //    octaveP = Math.Sqrt(octaveP);
            //    //    double mod = (.5 * Octave_pressure[oct] - octaveP) / total;
            //    //    for (int x = 0; x < gauss.Length; x++) p[offset2 + x] += gauss[x] * mod;
            //    //}



            //    List<double> mod = new List<double>();
            //    mod.Add(0);//Octave_pressure[0] / 2 * s);
            //    mod.Add(0);//Octave_pressure[0] / 2 * s);
            //    mod.Add(0);//Octave_pressure[0] / 2 * s);

            //    for (int oct = 1; oct < 8; oct++)
            //    {
            //        int offset = (int)(62.5 * Math.Pow(2, oct) / rt2 / df);
            //        double octaveP = 0;
            //        for (int x = 0; x < samples[oct]; x++) octaveP += p[x + offset];
            //        octaveP = Math.Sqrt(octaveP);
            //        mod.Add((Octave_pressure[oct] * Octave_pressure[oct] - octaveP) / samples[oct]);
            //    }

            //    freq = 0;
            //    octave = 7;
            //    while (freq < sample_frequency / 2)
            //    {
            //        octave++;
            //        freq = 62.5 * Math.Pow(2, octave);
            //        //double fl_ = freq / rt2, fu_ = freq * rt2;
            //        //double bw_ = fu_ - fl_;
            //        //double samples_ = bw_ / df;
            //        mod.Add(mod.Last() / 2);
            //    }
            //    prm = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(f.ToArray(), mod.ToArray());

            //    for (int j = 0; j < spec_length / 2; j++)
            //    {
            //        double frequency = (j + 1) * df;
            //        p[j] += Math.Pow(BASE, prm.Interpolate(frequency));
            //    }

            //    for (int i = 0; i < p.Length; i++) p[i] = Math.Sqrt(p[i]);




            //        ////for (int oct = 0; oct < 8; oct++)
            //        //{
            //        //    double total = 0;
            //        //    double[] gauss = new double[samples[oct]];
            //        //    double b = samples[oct] / 2;
            //        //    double c = Math.Ceiling(samples[oct] / 6f);

            //        //    for (int x = 0; x < gauss.Length; x++)
            //        //    {
            //        //        double xb = x - b;
            //        //        gauss[x] = System.Math.Exp(-(xb * xb) / (2 * c * c));
            //        //        total += gauss[x] * gauss[x];
            //        //    }

            //        //    total = Math.Sqrt(total);

            //        //    int offset = (int)Math.Floor(fl[oct] / df);
            //        //    double octaveP = 0;
            //        //    for (int x = 0; x < gauss.Length; x++) octaveP += p_i[x + offset] * p_i[x + offset];
            //        //    octaveP = Math.Sqrt(octaveP);
            //        //    double mod = (.5 * Octave_pressure[oct] - octaveP) / total;
            //        //    for (int x = 0; x < gauss.Length; x++) p_i[x + offset] += gauss[x] * mod;
            //        //}

            //        /////Smooth
            //        //double[] p = new double[p_i.Length];
            //        ////for (int i = 0; i < p_i.Length; i++) p_i[i] = Math.Log10(p_i[i]);

            //        //    for (int i = 0; i < p_i.Length; i++)
            //        //    {
            //        //        double total = 0;
            //        //        double[] gauss = new double[7];
            //        //        int b = (int)Math.Ceiling(gauss.Length / 3d);
            //        //        double c = Math.Ceiling(gauss.Length / 12d);

            //        //        for (int x = 0; x < gauss.Length; x++)
            //        //        {
            //        //            double xb = x - b;
            //        //            gauss[x] = System.Math.Exp(-(xb * xb) / (2 * c * c));
            //        //            total += gauss[x];
            //        //        }

            //        //        total = Math.Sqrt(total);
            //        //        int ib = i + b;
            //        //        for (int x = 0; x < gauss.Length; x++)
            //        //        {
            //        //            if (ib + x >= p_i.Length) break;
            //        //            //p[ib + x] += Math.Pow(10, gauss[x] * p_i[i] / total);
            //        //            p[ib + x] += .5 * p_i[i] * gauss[x] / total;
            //        //        }

            //        //    p_i = p;
            //        //}
            //        return p;
            //}

            public static double[] Magnitude_Filter(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            {
                double rt2 = Math.Sqrt(2);
                int spec_length = length_starttofinish / 2;

                List<double> f = new List<double>();
                f.Add(0);
                f.Add(31.25 * rt2);
                for (int oct = 0; oct < 9; oct++)
                {
                    f.Add(62.5 * Math.Pow(2, oct));
                    f.Add(rt2 * 62.5 * Math.Pow(2, oct));
                }
                f.Add(sample_frequency / 2);

                double[] output = new double[spec_length];
                double[] samplep = new double[spec_length];

                List<double> pr = new List<double>();
                pr.Add(Octave_pressure[0]);
                pr.Add(Octave_pressure[0]);

                for (int oct = 0; oct < 7; oct++)
                {
                    pr.Add(Octave_pressure[oct]);
                    pr.Add((Octave_pressure[oct] + Octave_pressure[oct + 1]) / 2);
                }
                if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//8k
                if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//10k
                if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//12k
                if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//16k
                while (pr.Count < f.Count) pr.Add(Octave_pressure[7]);

                MathNet.Numerics.Interpolation.CubicSpline prm = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(f.ToArray(), pr.ToArray());

                double[] p_i = new double[spec_length];

                for (int j = 0; j < spec_length; j++)
                {
                    double fr = (j + 1) * (sample_frequency / 2) / spec_length;
                    p_i[j] = prm.Interpolate(fr);// / (fr);
                }

                return p_i;
            }
            
            //public static double[] Magnitude_Filter(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            //{
            //    double rt2 = Math.Sqrt(2);
            //    int spec_length = length_starttofinish / 2;

            //    List<double> f = new List<double>();
            //    f.Add(0);
            //    f.Add(31.25 * rt2);
            //    for (int oct = 0; oct < 9; oct++)
            //    {
            //        f.Add(62.5 * Math.Pow(2, oct));
            //        f.Add(rt2 * 62.5 * Math.Pow(2, oct));
            //    }
            //    f.Add(sample_frequency / 2);

            //    double[] output = new double[spec_length];
            //    double[] samplep = new double[spec_length];

            //    List<double> pr = new List<double>();
            //    pr.Add(Octave_pressure[0]);
            //    pr.Add(Octave_pressure[0]);

            //    for (int oct = 0; oct < 7; oct++)
            //    {
            //        pr.Add(Octave_pressure[oct]);
            //        pr.Add((Octave_pressure[oct] + Octave_pressure[oct + 1]) / 2);
            //    }
            //    if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//8k
            //    if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//10k
            //    if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//12k
            //    if (pr.Count < f.Count) pr.Add(Octave_pressure[7]);//16k
            //    while (pr.Count < f.Count) pr.Add(Octave_pressure[7]);

            //    MathNet.Numerics.Interpolation.CubicSpline prm = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(f.ToArray(), pr.ToArray());

            //    double[] p_i = new double[spec_length];

            //    for (int j = 0; j < spec_length; j++)
            //    {
            //        double fr = (j + 1) * (sample_frequency / 2) / spec_length;
            //        p_i[j] = prm.Interpolate(fr);// / (fr);
            //    }

            //    return p_i;
            //}

            public static System.Numerics.Complex[] Minimum_Phase_TF(double[] M_spec, int sample_frequency, int threadid)
            {
                System.Numerics.Complex[] logspec = new System.Numerics.Complex[M_spec.Length];
                for (int i = 0; i < M_spec.Length; i++)
                {
                    logspec[i] = Math.Log(M_spec[i]);
                }

                double[] real_cepstrum = IFFT_Real_General(Mirror_Spectrum(logspec), threadid);
                Scale(ref real_cepstrum);

                double[] ym = new double[real_cepstrum.Length];
                ym[0] = real_cepstrum[0];

                for (int i = 1; i < M_spec.Length; i++)
                {
                    ym[i] = 2 * real_cepstrum[i];
                }
                ym[M_spec.Length] = real_cepstrum[M_spec.Length];
                System.Numerics.Complex[] ymspec = FFT_General(ym, threadid);

                for (int i = 0; i < ymspec.Length; i++)
                {
                    ymspec[i] = Complex.Exp(ymspec[i]);
                }

                return ymspec;
            }

            public static System.Numerics.Complex[] Minimum_Phase_Spectrum(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            {
                double[] M_spec = Magnitude_Spectrum(Octave_pressure, sample_frequency, length_starttofinish, threadid);

                System.Numerics.Complex[] logspec = new System.Numerics.Complex[M_spec.Length];
                for (int i = 0; i < M_spec.Length; i++)
                {
                    logspec[i] = Math.Log(M_spec[i]);
                }

                double[] real_cepstrum = IFFT_Real_General(Mirror_Spectrum(logspec), threadid);
                Scale(ref real_cepstrum);

                double[] ym = new double[length_starttofinish];
                ym[0] = real_cepstrum[0];

                for (int i = 1; i < length_starttofinish/2; i++)
                {
                    ym[i] = 2 * real_cepstrum[i];
                }
                ym[length_starttofinish / 2] = real_cepstrum[length_starttofinish / 2];
                System.Numerics.Complex[] ymspec = FFT_General(ym, threadid);

                for (int i = 0; i < ymspec.Length; i++)
                {
                    ymspec[i] = Complex.Exp(ymspec[i]);
                }

                return ymspec;
            }

            public static double[] Minimum_Phase_Signal(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            {
                double[] M_spec = Magnitude_Spectrum(Octave_pressure, sample_frequency, length_starttofinish, threadid);

                double sum_start = 0;
                for (int i = 0; i < M_spec.Length; i++) sum_start += M_spec[i] * M_spec[i];

                System.Numerics.Complex[] logspec = new System.Numerics.Complex[M_spec.Length];
                for (int i = 0; i < M_spec.Length; i++)
                {
                    logspec[i] = Math.Log(M_spec[i]);
                }

                double[] real_cepstrum = IFFT_Real_General(Mirror_Spectrum(logspec), threadid);
                Scale(ref real_cepstrum);

                double[] ym = new double[length_starttofinish];
                ym[0] = real_cepstrum[0];

                for (int i = 1; i < length_starttofinish / 2; i++)
                {
                    ym[i] = 2 * real_cepstrum[i];
                }
                ym[length_starttofinish / 2] = real_cepstrum[length_starttofinish / 2];
                System.Numerics.Complex[] ymspec = FFT_General(ym, threadid);

                for (int i = 0; i < ymspec.Length; i++)
                {
                    ymspec[i] = Complex.Exp(ymspec[i]);
                }

                double[] Signal = IFFT_Real_General(ymspec, threadid); //This is real valued hopefully... (if not there is a problem...)
                for (int i = 0; i < Signal.Length; i++) Signal[i] /= Math.Sqrt(Signal.Length);

                double sum_end1 = 0;
                for (int i = 0; i < ymspec.Length; i++)
                {
                    sum_end1 += Signal[i] * Signal[i];
                }

                return Signal;
            }

            public static System.Numerics.Complex[] Minimum_Phase_TF_Octaves(double[] Octave_filter, int sample_frequency, int length_starttofinish, int threadid)
            {
                double[] M_spec = Magnitude_Filter(Octave_filter, sample_frequency, length_starttofinish, threadid);

                System.Numerics.Complex[] logspec = new System.Numerics.Complex[M_spec.Length];
                for (int i = 0; i < M_spec.Length; i++)
                {
                    logspec[i] = Math.Log(M_spec[i]);
                }

                double[] real_cepstrum = IFFT_Real_General(Mirror_Spectrum(logspec), threadid);
                Scale(ref real_cepstrum);

                double[] ym = new double[length_starttofinish];
                ym[0] = real_cepstrum[0];

                for (int i = 1; i < length_starttofinish / 2; i++)
                {
                    ym[i] = 2 * real_cepstrum[i];
                }
                ym[length_starttofinish / 2] = real_cepstrum[length_starttofinish / 2];
                System.Numerics.Complex[] ymspec = FFT_General(ym, threadid);

                for (int i = 0; i < ymspec.Length; i++)
                {
                    ymspec[i] = Complex.Exp(ymspec[i]);
                }

                return ymspec;
            }

            //public static Complex[] Linear_Phase_TF(Complex[] M_spec, int sample_frequency, int threadid)
            //{
            //    ///////////////Create Linear Phase Time Domain Filter/////////////
            //    Complex[] prefilter = IFFT(Mirror_Spectrum(M_spec), threadid);
            //    Scale(ref prefilter);

            //    int hw = prefilter.Length / 2;

            //    for (int i = 0; i < hw; i++)
            //    {
            //        prefilter[hw + i] = prefilter[i];
            //        prefilter[hw - i - 1] = prefilter[i];
            //    }

            //    return FFT(prefilter, threadid);
            //}

            public static double[] Linear_Phase_Signal(double[] Octave_pressure, int sample_frequency, int length_starttofinish, int threadid)
            {
                double[] M_s = Magnitude_Spectrum(Octave_pressure, sample_frequency, length_starttofinish, threadid);
                Complex[] M_spec = new Complex[M_s.Length];
                
                for (int i = 0; i < M_s.Length; i++)
                {
                    M_spec[i] = M_s[i];
                }

                ///////////////Create Zero Phase Time Domain Filter/////////////
                double[] prefilter = IFFT_Real_General(Mirror_Spectrum(M_spec), threadid);
                double scale = Math.Sqrt(prefilter.Length);
                //Rotate filter in time domain to center.
                int hw = prefilter.Length / 2;
                double[] filter = new double[prefilter.Length];

                for (int i = 0; i < prefilter.Length; i++)
                {
                    filter[i] = prefilter[(i + hw) % prefilter.Length] / scale;
                }

                return filter;
            }

            public static double[] Minimum_Phase_Response(double[] M_spec, int sample_frequency, int threadid)
            {
                System.Numerics.Complex[] logspec = new System.Numerics.Complex[M_spec.Length];
                for (int i = 0; i < M_spec.Length; i++)
                {
                    logspec[i] = Math.Log(M_spec[i]);
                }

                double[] real_cepstrum = IFFT_Real(Mirror_Spectrum(logspec), threadid);
                Scale(ref real_cepstrum);

                double[] ym = new double[real_cepstrum.Length];
                ym[0] = real_cepstrum[0];

                for (int i = 1; i < M_spec.Length; i++)
                {
                    ym[i] = 2 * real_cepstrum[i];
                }
                ym[M_spec.Length] = real_cepstrum[M_spec.Length];
                System.Numerics.Complex[] ymspec = FFT(ym, threadid);

                for (int i = 0; i < ymspec.Length; i++)
                {
                    ymspec[i] = Complex.Exp(ymspec[i]);
                }

                double[] Signal = IFFT_Real(ymspec, threadid);
                Scale(ref Signal);

                return Signal;
            }

            public static double[] ETCToPTC(double[][] Octave_ETC, double CutOffTime, int sample_frequency_in, int sample_frequency_out, double Rho_C)
            {
                int length = (int)Math.Pow(2, 11);
                double[] IR = new double[(int)Math.Floor(sample_frequency_out * CutOffTime) + 2 * (int)length];
                double BW = (double)sample_frequency_out / (double)sample_frequency_in;

                //Convert to Pressure & Interpolate full resolution IR
                int ct = 0;
                System.Threading.Semaphore S = new System.Threading.Semaphore(0, 1);
                S.Release(1);

                int proc = UI.PachydermAc_PlugIn.Instance.ProcessorSpec();
                double[][] output = new double[proc][];
                double[][] samplep = new double[proc][];
                System.Threading.Thread[] T = new System.Threading.Thread[proc];
                int[] to = new int[proc];
                int[] from = new int[proc];

                System.Threading.CountdownEvent CDE = new System.Threading.CountdownEvent(proc);

                for (int p = 0; p < proc; p++)
                {
                    output[p] = new double[length];
                    samplep[p] = new double[length * 2];
                    to[p] = p * Octave_ETC[0].Length / proc;
                    from[p] = (p + 1) * Octave_ETC[0].Length / proc;

                    T[p] = new System.Threading.Thread((thread) =>
                    {
                         int thr = (int)thread;
                        for (int t = to[thr]; t < from[thr]; t++)
                        {
                            ct++;
                            double[] pr = new double[8];
                            for (int oct = 0; oct < 8; oct++) pr[oct] = Math.Sqrt(Octave_ETC[0][t] * Rho_C);

                            double sum = 0;
                            foreach (double d in pr) sum += d;
                            if (sum > 0)
                            {
                                output[thr] = Linear_Phase_Signal(pr, sample_frequency_out, 2048, thr);
                                Audio.Pach_SP.Raised_Cosine_Window(ref output[thr]);
                                for (int k = 0; k < length; k++)
                                {
                                    IR[(int)Math.Floor(t * BW) + k] += output[thr][k];
                                }
                            }
                        }
                        CDE.Signal();
                    });
                    T[p].Start(p);
                }

                CDE.Wait();

                //System.Threading.Mutex.WaitAll(ThreadFinishEvents.ToArray(), -1);

                //System.Threading.Thread.CurrentThread.SetApartmentState(ExistingState);

                //while (true)
                //{
                //    Rhino.RhinoApp.SetCommandPrompt(string.Format("Extrapolating Impulse : {0}% Complete", Math.Round(100 * (double)ct / (double)Octave_ETC[0].Length)));
                //    System.Threading.Thread.Sleep(1000);
                //    bool finished = true;
                //    foreach (System.Threading.Thread t in T)
                //    {
                //        if (t.ThreadState == System.Threading.ThreadState.Running)
                //        {
                //            finished = false;
                //            break;
                //        }
                //    }
                //    if (finished) break;
                //}
                return IR;
            }

            public static double[] Expand_Response(Direct_Sound[] DirectIn, ImageSourceData[] SpecularIn, Environment.Receiver_Bank[] DiffuseIn, double CutOffTime, int sample_frequency_out, int Rec_ID, List<int> SrcID)
            {
                int length = (int)Math.Pow(2, 11);
                int sample_frequency_in = DiffuseIn[0].SampleRate;
                //fftwlib.fftw. FFT = new MathNet.Numerics.IntegralTransforms.Algorithms.DiscreteFourierTransform();
                double[] IR = new double[(int)Math.Floor(sample_frequency_out * CutOffTime) + 2 * (int)length];
                double[][] Octave_ETC = new double[8][];
                double BW = (double)sample_frequency_out / (double)sample_frequency_in;

                //Convert to Pressure & Interpolate full resolution IR
                for (int oct = 0; oct < 8; oct++)
                {
                    Octave_ETC[oct] = AcousticalMath.ETCurve(DirectIn, SpecularIn, DiffuseIn, CutOffTime, sample_frequency_in, oct, Rec_ID, SrcID, false);
                }

                return ETCToPTC(Octave_ETC, CutOffTime, sample_frequency_in, sample_frequency_out, DirectIn[0].Rho_C[Rec_ID]);
            }

            /// <summary>
            /// Takes ETC, and extrapolates a pressure domain impulse response.
            /// </summary>
            /// <param name="DirectIn"></param>
            /// <param name="SpecularIn"></param>
            /// <param name="DiffuseIn"></param>
            /// <param name="CutOffTime"></param>
            /// <param name="sample_frequency_out">The desired sample frequency.</param>
            /// <param name="Rec_ID">The index of the receiver.</param>
            /// <returns></returns>
            public static double[] Expand_Dir_Response(Direct_Sound[] DirectIn, ImageSourceData[] SpecularIn, Environment.Receiver_Bank[] DiffuseIn, double CutOffTime, int sample_frequency_out, int Rec_ID, List<int> SrcID, double alt, double azi, bool degrees)
            {
                Random Rnd = new Random();
                int length = (int)Math.Pow(2, 11);
                int sample_frequency_in = DiffuseIn[0].SampleRate;
                //fftwlib.fftw. FFT = new MathNet.Numerics.IntegralTransforms.Algorithms.DiscreteFourierTransform();
                double[] IR = new double[(int)Math.Floor(sample_frequency_out * CutOffTime) + 2 * (int)length];
                double[][] Octave_ETC = new double[8][];
                double BW = (double)sample_frequency_out / (double)sample_frequency_in;

                //Convert to Pressure & Interpolate full resolution IR
                for (int oct = 0; oct < 8; oct++)
                {
                    Octave_ETC[oct] = AcousticalMath.ETCurve_Directional(DirectIn, SpecularIn, DiffuseIn, CutOffTime, sample_frequency_in, oct, Rec_ID, SrcID, false, alt, azi, degrees);
                }

                return ETCToPTC(Octave_ETC, CutOffTime, sample_frequency_in, sample_frequency_out, DirectIn[0].Rho_C[Rec_ID]);
            }

            /// <summary>
            /// Frequency domain convolution.
            /// </summary>
            /// <param name="SignalBuffer">the dry signal.</param>
            /// <param name="Filter">the pressure domain impulse response.</param>
            /// <returns>the convolved signal.</returns>
            public static float[] FFT_Convolution(double[] SignalBuffer, double[] Filter, int threadid)
            {
                if (SignalBuffer == null) return null;
                int minlength = SignalBuffer.Length > Filter.Length ? SignalBuffer.Length : Filter.Length;

                int W = (int)Math.Pow(2, Math.Ceiling(Math.Log(minlength, 2)));

                if (SignalBuffer.Length < W) Array.Resize(ref SignalBuffer, W);
                if (Filter.Length < W) Array.Resize(ref Filter, W);

                System.Numerics.Complex[] freq1 = FFT_General(SignalBuffer, threadid);
                System.Numerics.Complex[] freq2 = FFT_General(Filter, threadid);
                
                System.Numerics.Complex[] freq3 = new System.Numerics.Complex[W];

                for (int i = 0; i < freq1.Length; i++) freq3[i] = freq1[i] * freq2[i];

                double[] conv = IFFT_Real_General(freq3, threadid);

                float[] output = new float[conv.Length];
                double mod = 1d / Math.Sqrt(conv.Length);
                for (int i = 0; i < conv.Length; i++) output[i] = (float)(conv[i] * mod);// * mod);

                double maxfilt = Filter.Max();
                double maxsig = SignalBuffer.Max();
                double max = conv.Max();
                max++;
                maxsig++;
                maxfilt++;
                return output;
            }

            public static double[] Pressure_Interpolation(double[][] ETC, int SampleRate_IN, int SampleRate_Out, double Rho_C)
            {
                double[][] SPLetc = new double[8][];
                double[] Total_E = new double[8];

                double[] time = new double[ETC[0].Length];
                double dtIn = 1.0f / SampleRate_IN;
                double dtOut = 1.0f / SampleRate_Out;
                for(int i = 1; i < time.Length; i++) time[i] = (double)i / SampleRate_IN;

                for(int oct = 0; oct < 8; oct++)
                {
                    SPLetc[oct] = new double[ETC[0].Length];
                    for(int i = 0; i < time.Length; i++) 
                    {
                        Total_E[oct] += ETC[oct][i];
                        if (ETC[oct][i] > 0) SPLetc[oct][i] = Math.Log10(ETC[oct][i]);
                        else SPLetc[oct][i] = Math.Log10(1E-12);
                    }
                }

                int NewLength = (int)Math.Ceiling((double)(SampleRate_Out * ETC[0].Length) / SampleRate_IN);
                double[][] NewETC = new double[8][];
                double[] NewTE = new double[8];
                for(int oct = 0; oct < 8; oct++)
                {
                    MathNet.Numerics.Interpolation.CubicSpline CS = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(time, SPLetc[oct]);
                    NewETC[oct] = new double[NewLength];
                    for(int i = 0;i < NewLength; i++)
                    {
                        NewETC[oct][i] = Math.Pow(10, CS.Interpolate(i * dtOut));
                        NewTE[oct] += NewETC[oct][i];
                    }

                    for(int i = 0; i < NewLength; i++)
                    {
                        NewETC[oct][i] *= Total_E[oct] / NewTE[oct];
                    }
                }
                return ETCToPTC(NewETC, (double)NewLength / SampleRate_Out, SampleRate_Out, SampleRate_Out, Rho_C);
            }
        }
    }
}