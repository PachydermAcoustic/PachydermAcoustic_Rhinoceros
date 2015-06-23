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
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        public abstract class Material
        {
            public abstract void Absorb(ref OctaveRay Ray, Hare.Geometry.Vector Normal);
            public abstract void Absorb(ref BroadRay Ray, Hare.Geometry.Vector Normal);
            public abstract void Absorb(ref OctaveRay Ray, out double cos_theta, Hare.Geometry.Vector Normal);
            public abstract void Absorb(ref BroadRay Ray, out double cos_theta, Hare.Geometry.Vector Normal);
            public abstract System.Numerics.Complex Reflection_Narrow(double frequency);
            public abstract System.Numerics.Complex Reflection_Narrow(double frequency, Hare.Geometry.Vector Dir, Hare.Geometry.Vector Normal);
            public abstract double Coefficient_A_Broad(int Octave);
            public abstract double[] Coefficient_A_Broad();
            public abstract System.Numerics.Complex[] Reflection_Spectrum(int sample_frequency, int length, Hare.Geometry.Vector Normal, Hare.Geometry.Vector Dir, int threadid);
        }

        public abstract class Scattering 
        {
            public abstract double Coefficient(int octave);
            public abstract double[] Coefficient();
            public abstract void Scatter_Early(ref BroadRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, Hare.Geometry.Vector Normal, double Cos_Theta);
            public abstract void Scatter_Late(ref OctaveRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, Hare.Geometry.Vector Normal, double Cos_Theta);
            public abstract void Scatter_VeryLate(ref OctaveRay Ray, ref Random rand, Hare.Geometry.Vector Normal, double Cos_Theta);
        }

        public class Basic_Material : Material
        {
            double[] Abs = new double[8];
            double[] Ref = new double[8];
            double[] PD = new double[8];
            MathNet.Numerics.Interpolation.CubicSpline Transfer_Function;

            public Basic_Material(double[] ABS, double[] Phase_Delay)
            {
                Abs = ABS;
                PD = Phase_Delay;
                for (int i = 0; i < ABS.Length; i++) Ref[i] = 1 - ABS[i];

                //Interpolate a transfer function... this will probably be clumsy at first...
                double rt2 = Math.Sqrt(2);

                List<double> f = new List<double>();
                f.Add(0);
                f.Add(31.25 * rt2);
                for (int oct = 0; oct < 9; oct++)
                {
                    f.Add(62.5 * Math.Pow(2, oct));
                    f.Add(rt2 * 62.5 * Math.Pow(2, oct));
                }
                f.Add(24000);

                List<double> pr = new List<double>();
                pr.Add(0);
                pr.Add(Math.Sqrt(1 - Abs[0]));

                for (int oct = 0; oct < 7; oct++)
                {
                    pr.Add(Math.Sqrt(1 - Abs[oct]));
                    pr.Add(Math.Sqrt((2 - Abs[oct] - Abs[oct + 1]) / 2));
                }
                if (pr.Count < f.Count) pr.Add(Math.Sqrt(1 - Abs[7]));//8k
                if (pr.Count < f.Count) pr.Add(Math.Sqrt((1 - Abs[7] + (1 - Abs[7])) / 2));//10k
                if (pr.Count < f.Count) pr.Add(Math.Sqrt(1 - Abs[7]));//12k
                if (pr.Count < f.Count) pr.Add(Math.Sqrt(1 - Abs[7]));//16k
                while (pr.Count < f.Count) pr.Add(1 - Abs[7]);

                Transfer_Function = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(f.ToArray(), pr.ToArray());
            }

            public override System.Numerics.Complex[] Reflection_Spectrum(int sample_frequency, int length, Hare.Geometry.Vector Normal, Hare.Geometry.Vector Dir, int threadid)
            {
                System.Numerics.Complex[] Ref_trns = new System.Numerics.Complex[length];

                for (int j = 0; j < length; j++)
                {
                    Ref_trns[j] = new System.Numerics.Complex(Transfer_Function.Interpolate(j * (sample_frequency / 2) / length), 0);
                }

                return Ref_trns;
            }

            public override void Absorb(ref OctaveRay Ray, Hare.Geometry.Vector Normal)
            {
                Ray.Intensity *= (Ref[Ray.Octave]);
                Ray.phase += PD[Ray.Octave];
            }

            public override void Absorb(ref OctaveRay Ray, out double cos_theta, Hare.Geometry.Vector Normal)
            {
                cos_theta = Hare.Geometry.Hare_math.Dot(Normal, Ray.direction);

                Ray.Intensity *= (Ref[Ray.Octave]);
                Ray.phase += PD[Ray.Octave];
            }

            public override void Absorb(ref BroadRay Ray, out double cos_theta, Hare.Geometry.Vector Normal)
            {
                cos_theta = Hare.Geometry.Hare_math.Dot(Normal, Ray.direction);

                Ray.Energy[0] *= (Ref[0]);
                Ray.phase[0] += PD[0];
                Ray.Energy[1] *= (Ref[1]);
                Ray.phase[1] += PD[1];
                Ray.Energy[2] *= (Ref[2]);
                Ray.phase[2] += PD[2];
                Ray.Energy[3] *= (Ref[3]);
                Ray.phase[3] += PD[3];
                Ray.Energy[4] *= (Ref[4]);
                Ray.phase[4] += PD[4];
                Ray.Energy[5] *= (Ref[5]);
                Ray.phase[5] += PD[5];
                Ray.Energy[6] *= (Ref[6]);
                Ray.phase[6] += PD[6];
                Ray.Energy[7] *= (Ref[7]);
                Ray.phase[7] += PD[7];
            }

            public override void Absorb(ref BroadRay Ray, Hare.Geometry.Vector Normal)
            {
                Ray.Energy[0] *= (Ref[0]);
                Ray.phase[0] += PD[0];
                Ray.Energy[1] *= (Ref[1]);
                Ray.phase[1] += PD[1];
                Ray.Energy[2] *= (Ref[2]);
                Ray.phase[2] += PD[2];
                Ray.Energy[3] *= (Ref[3]);
                Ray.phase[3] += PD[3];
                Ray.Energy[4] *= (Ref[4]);
                Ray.phase[4] += PD[4];
                Ray.Energy[5] *= (Ref[5]);
                Ray.phase[5] += PD[5];
                Ray.Energy[6] *= (Ref[6]);
                Ray.phase[6] += PD[6];
                Ray.Energy[7] *= (Ref[7]);
                Ray.phase[7] += PD[7];
            }

            public override double[] Coefficient_A_Broad()
            {
                return Abs;
            }

            public override double Coefficient_A_Broad(int Octave)
            {
                return Abs[Octave];
            }

            public override System.Numerics.Complex Reflection_Narrow(double frequency)
            {
                return new System.Numerics.Complex(Transfer_Function.Interpolate(frequency), 0);
            }

            public override System.Numerics.Complex Reflection_Narrow(double frequency, Hare.Geometry.Vector Dir, Hare.Geometry.Vector Normal)
            {
                return new System.Numerics.Complex(Transfer_Function.Interpolate(frequency), 0);
            }
        }

        public class Finite_Material : Material
        {
            double[][][] alpha;
            double[] Azimuth;
            double[] Altitude;

            Smart_Material Inf_Mat;

            public Finite_Material(Smart_Material Mat, Rhino.Geometry.Brep Br, Rhino.Geometry.Mesh M, int face_id, Medium_Properties med)
            {
                //Strictly for the flat X,Y case - oversimplified for now.
                Inf_Mat = Mat;
                Azimuth = new double[36];
                Altitude = new double[Mat.Angles.Length/2];
                alpha = new double[Altitude.Length][][];
                for(int i = 0; i < Altitude.Length; i++) Altitude[i] = Mat.Angles[i].Magnitude;
                for(int i = 0; i < Azimuth.Length; i++) Azimuth[i] = i * 360f / Azimuth.Length;

                    //Set up a frequency interpolated Zr for each direction individually.
                    Rhino.Geometry.Point3d pt = M.Faces.GetFaceCenter(face_id);

                    double[][][] ZrR = new double[Altitude.Length][][], ZrI = new double[Altitude.Length][][];
                    double[] fr = new double[9];
                    for (int k = 0; k < Altitude.Length; k++)
                    {
                        ZrR[k] = new double[Azimuth.Length][];
                        ZrI[k] = new double[Azimuth.Length][];
                        alpha[k] = new double[Azimuth.Length][];
                        for (int j = 0; j < Azimuth.Length; j++)
                        {
                            ZrR[k][j] = new double[9];
                            ZrI[k][j] = new double[9];
                            alpha[k][j] = new double[8];
                        }
                    }

                    for (int oct = 0; oct < 9; oct++)
                    {
                        fr[oct] = 62.5 * Math.Pow(2, oct) / Utilities.Numerics.rt2;
                        System.Numerics.Complex[][] Zr = AbsorptionModels.Operations.Finite_Radiation_Impedance_Rect_Longhand(pt.X, pt.Y, Br, fr[oct], Altitude, Azimuth, med.Sound_Speed(pt));

                        for (int k = 0; k < Zr.Length; k++)
                        {
                            for (int j = 0; j < Zr[k].Length; j++)
                            {
                                ZrR[k][j][oct] = Zr[k][j].Real;
                                ZrI[k][j][oct] = Zr[k][j].Imaginary;
                            }
                        }
                    }

                    MathNet.Numerics.Interpolation.CubicSpline[][] Zr_r = new MathNet.Numerics.Interpolation.CubicSpline[Altitude.Length][];
                    MathNet.Numerics.Interpolation.CubicSpline[][] Zr_i = new MathNet.Numerics.Interpolation.CubicSpline[Altitude.Length][];

                    for (int k = 0; k < Zr_r.Length; k++)
                    {
                        Zr_r[k] = new MathNet.Numerics.Interpolation.CubicSpline[Azimuth.Length];
                        Zr_i[k] = new MathNet.Numerics.Interpolation.CubicSpline[Azimuth.Length];
                        for (int j = 0; j < Zr_r[k].Length; j++)
                        {
                            //Interpolate over curve real and imaginary Zr here...
                            Zr_r[k][j] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(fr, ZrR[k][j]);
                            Zr_i[k][j] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(fr, ZrI[k][j]);
                        }
                    }

                    for (int k = 0; k < Zr_r.Length; k++)
                    {
                        for (int j = 0; j < Zr_r[k].Length; j++)
                        {
                            List<double> freq = new List<double>();
                            List<double> alpha_interp = new List<double>();
                            for (int l = 0; l < Mat.frequency.Length; l++)
                            {
                                if (Mat.frequency[l] > 10000) break;
                                freq.Add(Mat.frequency[l]);
                                alpha_interp.Add(AbsorptionModels.Operations.Finite_Unit_Absorption_Coefficient(Mat.Z[k][j], new System.Numerics.Complex(Zr_r[k][j].Interpolate(Mat.frequency[l]), Zr_i[k][j].Interpolate(Mat.frequency[l])), med.Rho(Utilities.PachTools.RPttoHPt(pt)), med.Sound_Speed(pt)));
                            }
                            MathNet.Numerics.Interpolation.CubicSpline a = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(freq, alpha_interp);
                            for (int oct = 0; oct < 8; oct++)
                            {
                                alpha[k][j][oct] = 1 - a.Integrate(fr[oct], fr[oct + 1]) / (fr[oct + 1] - fr[oct]);
                            }
                        }
                    }
            }

            public override void Absorb(ref BroadRay Ray, Hare.Geometry.Vector Normal)
            {
                //Simplified for sample laid on floor...
                Ray.direction.Normalize();
                int Alt = (int)Math.Floor((Math.Acos(Hare.Geometry.Hare_math.Dot(Ray.direction, new Hare.Geometry.Vector(0, 0, -1))) * Altitude.Length) / (Math.PI / 2));
                if (Alt >= Altitude.Length / 2) Alt = Altitude.Length - 1;
                int Azi = (int)Math.Round((Math.Atan2(Ray.direction.y, Ray.direction.x) * Azimuth.Length) / Math.PI);
                if (Ray.direction.y < 0 && Azi < Azimuth.Length / 2) Azi = Azimuth.Length - Math.Abs(Azi);
                for (int oct = 0; oct < 8; oct++) Ray.Energy[oct] *= alpha[Alt][Azi][oct];
            }

            public override void Absorb(ref BroadRay Ray, out double cos_theta, Hare.Geometry.Vector Normal)
            {
                Ray.direction.Normalize();
                cos_theta = Math.Acos(Hare.Geometry.Hare_math.Dot(Ray.direction, new Hare.Geometry.Vector(0, 0, -1)));
                int Alt = (int)Math.Floor((cos_theta * Altitude.Length) / (Math.PI / 2));
                if (Alt >= Altitude.Length / 2) Alt = Altitude.Length - 1;
                int Azi = (int)Math.Round((Math.Atan2(Ray.direction.y, Ray.direction.x) * Azimuth.Length) / Math.PI);
                if (Ray.direction.y < 0 && Azi < Azimuth.Length / 2) Azi = Azimuth.Length - Math.Abs(Azi);
                
                for (int oct = 0; oct < 8; oct++) Ray.Energy[oct] *= alpha[Alt][Azi][oct];                
            }

            public override void Absorb(ref OctaveRay Ray, Hare.Geometry.Vector Normal)
            {
                Ray.direction.Normalize();
                int Alt = (int)Math.Floor((Math.Acos(Hare.Geometry.Hare_math.Dot(Ray.direction, new Hare.Geometry.Vector(0, 0, -1))) * Altitude.Length) / (Math.PI / 2));
                if (Alt >= Altitude.Length / 2) Alt = Altitude.Length - 1;
                int Azi = (int)Math.Round((Math.Atan2(Ray.direction.y, Ray.direction.x) * Azimuth.Length) / Math.PI);
                if (Ray.direction.y < 0 && Azi < Azimuth.Length / 2) Azi = Azimuth.Length - Math.Abs(Azi);
                Ray.Intensity *= alpha[Alt][Azi][Ray.Octave];
            }

            public override void Absorb(ref OctaveRay Ray, out double cos_theta, Hare.Geometry.Vector Normal)
            {
                Ray.direction.Normalize();
                cos_theta = Math.Acos(Hare.Geometry.Hare_math.Dot(Ray.direction, new Hare.Geometry.Vector(0, 0, -1)));
                int Alt = (int)Math.Floor((cos_theta * Altitude.Length) / (Math.PI / 2));
                if (Alt >= Altitude.Length / 2) Alt = Altitude.Length - 1;
                int Azi = (int)Math.Round((Math.Atan2(Ray.direction.y, Ray.direction.x) * Azimuth.Length) / Math.PI / 2);
                if (Ray.direction.y < 0 && Azi < Azimuth.Length / 2) Azi = Azimuth.Length - Math.Abs(Azi);
                if (Azi == Azimuth.Length) Azi = 0;
                Ray.Intensity *= alpha[Alt][Azi][Ray.Octave];
            }

            public override double[] Coefficient_A_Broad()
            {
                return Inf_Mat.Coefficient_A_Broad();
            }

            public override double Coefficient_A_Broad(int Octave)
            {
                return Inf_Mat.Coefficient_A_Broad(Octave);
            }

            public override System.Numerics.Complex Reflection_Narrow(double frequency)
            {
                return Inf_Mat.Reflection_Narrow(frequency);
            }

            public override System.Numerics.Complex Reflection_Narrow(double frequency, Hare.Geometry.Vector Dir, Hare.Geometry.Vector Normal)
            {
                return Inf_Mat.Reflection_Narrow(frequency, Dir, Normal);
            }

            public override System.Numerics.Complex[] Reflection_Spectrum(int sample_frequency, int length, Hare.Geometry.Vector Normal, Hare.Geometry.Vector Dir, int threadid)
            {
                return Inf_Mat.Reflection_Spectrum(sample_frequency, length, Normal, Dir, threadid);
            }
        }

        public class Smart_Material : Material
        {
            List<AbsorptionModels.ABS_Layer> Buildup;
            int Fs;
            double rho;
            double c;
            public double[] frequency = null;
            public System.Numerics.Complex[] Angles = null;
            public System.Numerics.Complex[][] Z;
            public MathNet.Numerics.Interpolation.CubicSpline[] Transfer_FunctionR;
            public MathNet.Numerics.Interpolation.CubicSpline[] Transfer_FunctionI;
            public System.Numerics.Complex[][] Reflection_Coefficient;
            public double[] NI_Coef;
            public double[][] Ang_Coef_Oct;//[oct][angle]
            public double[] RI_Coef = new double[8];
            
            private double angle_incr;
            
            public Smart_Material(List<AbsorptionModels.ABS_Layer> Layers, int Samplefreq, double Air_Density, double SoundSpeed, Finite_Field_Impedance Zr, double step, int Averaging_Choice, int Zf_incorp_Choice)
            {
                Buildup = Layers;
                Fs = Samplefreq;
                rho = Air_Density;
                c = SoundSpeed;

                int min_freq = Samplefreq / 4096;
                if (Layers.Count < 1) return;

                //the current version...
                Z = AbsorptionModels.Operations.Recursive_Transfer_Matrix(false, 10000, 343, Layers, ref frequency, ref Angles);
                //the goal...
                //Z = AbsorptionModels.Operations.Transfer_Matrix_Explicit_Alpha(false, true, 44100, 343, Layers, ref frequency, ref Angles);
                
                //////////////////Radiation Impedance///////////////////////
                double[] a_real = new double[Angles.Length]; //prop;
                for (int i = 0; i < Angles.Length; i++) a_real[i] = Angles[i].Real;
                
                double[][] Angular_Absorption;

                System.Numerics.Complex [][] Zr_interp = Zr.Interpolate(frequency);

                if (Zf_incorp_Choice == 0)
                {
                    Reflection_Coefficient = Pachyderm_Acoustic.AbsorptionModels.Operations.Reflection_Coef(Z, Air_Density, SoundSpeed); //No defined way to build a complex finite reflection coefficient.
                    Angular_Absorption = Pachyderm_Acoustic.AbsorptionModels.Operations.Finite_Unit_Absorption_Coefficient(Zr_interp, Z, a_real, rho, 343);
                }
                else if (Zf_incorp_Choice == 1)
                {
                    Reflection_Coefficient = Pachyderm_Acoustic.AbsorptionModels.Operations.Reflection_Coef(Z, Zr_interp, Air_Density, SoundSpeed); //No defined way to build a complex finite reflection coefficient.
                    Angular_Absorption = Pachyderm_Acoustic.AbsorptionModels.Operations.Absorption_Coef(Reflection_Coefficient);
                }
                else throw new Exception("Field Impedance Incorporation choice not valid or not implemented...");

                Transfer_FunctionR = new MathNet.Numerics.Interpolation.CubicSpline[Angles.Length / 2];
                Transfer_FunctionI = new MathNet.Numerics.Interpolation.CubicSpline[Angles.Length / 2];
                for(    int i = 0; i < Reflection_Coefficient.Length / 2; i++)
                {
                    List<double> real = new List<double>(), imag = new List<double>();
                    for(int j = 0; j < Reflection_Coefficient[i].Length; j++)
                    {
                        real.Add(Reflection_Coefficient[i][j].Real);
                        imag.Add(Reflection_Coefficient[i][j].Imaginary);
                    }
                    Transfer_FunctionR[Angles.Length/2 - i - 1] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(frequency, real);
                    Transfer_FunctionI[Angles.Length/2 - i - 1] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(frequency, imag);
                }
                double[] RI_Averages;

                if (Averaging_Choice == 0)
                    if (Zf_incorp_Choice == 0) RI_Averages = AbsorptionModels.Operations.Random_Incidence_Paris(Angular_Absorption, Zr_interp, SoundSpeed*Air_Density);
                    else RI_Averages = AbsorptionModels.Operations.Random_Incidence_Paris_Finite(Angular_Absorption);
                else if(Averaging_Choice == 1)
                    if (Zf_incorp_Choice == 0) RI_Averages = AbsorptionModels.Operations.Random_Incidence_0_78(Angular_Absorption, Zr_interp, SoundSpeed * Air_Density);
                    else RI_Averages = AbsorptionModels.Operations.Random_Incidence_0_78(Angular_Absorption);
                else if (Averaging_Choice == 2)
                    if (Zf_incorp_Choice == 0) RI_Averages = AbsorptionModels.Operations.Random_Incidence_NoWeights(Angular_Absorption, Zr_interp, SoundSpeed * Air_Density);
                    else RI_Averages = AbsorptionModels.Operations.Random_Incidence_NoWeights(Angular_Absorption);
                else throw new Exception("Averaging choice not valid or not implemented...");

                NI_Coef = Angular_Absorption[18];
                Ang_Coef_Oct = new double[8][];

                //5 degree increments, in radians...
                angle_incr = 5 * Math.PI / 180;

                double root2 = Math.Sqrt(2);

                int f = -1;

                for (int oct = 0; oct < 8; oct++)
                {
                    double f_center = 62.5 * Math.Pow(2, oct);
                    double f_lower = (int)((Math.Floor(f_center / root2)));// - min_freq)/df);
                    double f_upper = (int)((Math.Floor(f_center * root2)));// - min_freq)/df);

                    int f_id_l = 0;//(int)Math.Floor((double)((f_lower) / 5));
                    
                    for(int i = 0; i < frequency.Length; i++)
                    {
                        if (frequency[i] < f_lower) f_id_l = i;
                        else break;
                    }

                    int f_id_u;//(int)Math.Floor((double)((f_upper) / 5));

                    for (f_id_u = f_id_l; f_id_u < frequency.Length; f_id_u++)
                    {
                        if (frequency[f_id_u] > f_upper) break;
                    }

                    int count = 0;
                    int RI_count = 0;

                    Ang_Coef_Oct[oct] = new double[Angles.Length];
                    int[] fct = new int[Angular_Absorption.Length];
                    
                    do
                    {
                        f++;
                        RI_count++;
                        if (f < f_id_l) { f++; continue; }
                        if (f >= frequency.Length) break;
                        RI_Coef[oct] += RI_Averages[f];
                        for (int a = 0; a < 19; a++)
                        {
                            if (double.IsNaN(Angular_Absorption[a][f])) continue;
                            fct[a]++;
                            count++;
                            Ang_Coef_Oct[oct][a] += Angular_Absorption[a][f];
                        }
                        for (int a = 19; a < Angles.Length; a++)
                        {
                            if (double.IsNaN(Angular_Absorption[35 - a][f])) continue;
                            fct[a]++;
                            count++;
                            Ang_Coef_Oct[oct][a] += Angular_Absorption[35 - a][f];
                        }
                    } while (frequency[f] < f_upper);

                    for (int a = 0; a < Angles.Length; a++) Ang_Coef_Oct[oct][a] /= fct[a];
                    RI_Coef[oct] /=RI_count;
                }
            }

            public Smart_Material(List<AbsorptionModels.ABS_Layer> Layers, int Samplefreq, double Air_Density, double SoundSpeed, int Averaging_Choice)
            {
                Buildup = Layers;
                Fs = Samplefreq;
                rho = Air_Density;
                c = SoundSpeed;
                
                int min_freq = Samplefreq / 4096;
                int max_freq = Samplefreq / 2;

                if (Layers.Count < 1) return;

                //the current version...
                Z = AbsorptionModels.Operations.Recursive_Transfer_Matrix(false, 44100, 343, Layers, ref frequency, ref Angles);
                //the goal...
                //Z = AbsorptionModels.Operations.Transfer_Matrix_Explicit_Alpha(false, true, 44100, 343, Layers, ref frequency, ref Angles);
                
                Reflection_Coefficient = Pachyderm_Acoustic.AbsorptionModels.Operations.Reflection_Coef(Z, Air_Density, SoundSpeed);

                Transfer_FunctionR = new MathNet.Numerics.Interpolation.CubicSpline[Angles.Length / 2];
                Transfer_FunctionI = new MathNet.Numerics.Interpolation.CubicSpline[Angles.Length / 2];
                for (int i = 0; i < Reflection_Coefficient.Length / 2; i++)
                {
                    List<double> real = new List<double>(), imag = new List<double>();
                    for (int j = 0; j < Reflection_Coefficient[i].Length; j++)
                    {
                        real.Add(Reflection_Coefficient[i][j].Real);
                        imag.Add(Reflection_Coefficient[i][j].Imaginary);
                    }
                    Transfer_FunctionR[Angles.Length/2 - i - 1] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(frequency, real);
                    Transfer_FunctionI[Angles.Length/2 - i - 1] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(frequency, imag);
                }
                
                double[][] Angular_Absorption = Pachyderm_Acoustic.AbsorptionModels.Operations.Absorption_Coef(Reflection_Coefficient);
                NI_Coef = Angular_Absorption[18];
                double[] RI_Averages;


                if (Averaging_Choice == 0) RI_Averages = AbsorptionModels.Operations.Random_Incidence_Paris(Angular_Absorption);
                else if (Averaging_Choice == 1) RI_Averages = AbsorptionModels.Operations.Random_Incidence_0_78(Angular_Absorption);
                else if (Averaging_Choice == 2) RI_Averages = AbsorptionModels.Operations.Random_Incidence_NoWeights(Angular_Absorption);
                else throw new Exception("Averaging choice not valid or not implemented...");

                Ang_Coef_Oct = new double[8][];

                //5 degree increments, in radians...
                angle_incr = 5 * Math.PI / 180;

                double root2 = Math.Sqrt(2);

                for (int oct = 0; oct < 8; oct++)
                {
                    double f_center = 62.5 * Math.Pow(2, oct);
                    int f_lower = (int)Math.Floor(f_center / root2) - min_freq;
                    int f_upper = (int)Math.Floor(f_center * root2) - min_freq;
                    int f_id_l = (int)Math.Floor((double)((f_lower) / 5));
                    int f_id_u = (int)Math.Floor((double)((f_upper) / 5));
                    int count = 0;
                    int RI_count = 0;
                    Ang_Coef_Oct[oct] = new double[Angles.Length];
                    int[] fct = new int[Angular_Absorption.Length];
                    int f = 0;

                    do
                    {
                        RI_Coef[oct] += RI_Averages[f];
                        RI_count++;
                        for (int a = 0; a < 19; a++)
                        {
                            if (double.IsNaN(Angular_Absorption[a][f])) continue;
                            fct[a]++;
                            count++;
                            Ang_Coef_Oct[oct][a] += Angular_Absorption[a][f];                            
                        }
                        for (int a = 19; a < Angles.Length; a++)
                        {
                            if (double.IsNaN(Angular_Absorption[35 - a][f])) continue;
                            fct[a]++;
                            count++;
                            Ang_Coef_Oct[oct][a] += Angular_Absorption[35 - a][f];
                        }
                        f++;
                    } while (frequency[f] < f_upper);

                    for (int a = 0; a < Angles.Length; a++) Ang_Coef_Oct[oct][a] /= fct[a];
                    RI_Coef[oct] /= RI_count;
                }
            }

            public override double[] Coefficient_A_Broad()
            {
                return RI_Coef;
            }

            public override void Absorb(ref BroadRay Ray, Hare.Geometry.Vector Normal)
            {
                double cos_theta = Hare.Geometry.Hare_math.Dot(Ray.direction, Normal);
                int index = 18 - (int)Math.Round(Math.Acos(Math.Abs(cos_theta)) / angle_incr);

                for(int oct = 0; oct < 8; oct++) Ray.Energy[oct] *= (1 - Ang_Coef_Oct[oct][index]);
            }

            public override void Absorb(ref BroadRay Ray, out double cos_theta, Hare.Geometry.Vector Normal)
            {
                cos_theta = Hare.Geometry.Hare_math.Dot(Ray.direction, Normal);
                int index = 18 - (int)Math.Round(Math.Acos(Math.Abs(cos_theta)) / angle_incr);

                for (int oct = 0; oct < 8; oct++) Ray.Energy[oct] *= (1 - Ang_Coef_Oct[oct][index]);
            }

            public override void Absorb(ref OctaveRay Ray, Hare.Geometry.Vector Normal)
            {
                double cos_theta = Hare.Geometry.Hare_math.Dot(Ray.direction, Normal);
                int index = 18 - (int)Math.Round(Math.Acos(Math.Abs(cos_theta)) / angle_incr);

                Ray.Intensity *= (1 - Ang_Coef_Oct[Ray.Octave][index]);
            }

            public override void Absorb(ref OctaveRay Ray, out double cos_theta, Hare.Geometry.Vector Normal)
            {
                cos_theta = Hare.Geometry.Hare_math.Dot(Ray.direction, Normal);
                int index = 18 - (int)Math.Round(Math.Acos(Math.Abs(cos_theta)) / angle_incr);

                Ray.Intensity *= (1 - Ang_Coef_Oct[Ray.Octave][index]);
            }

            public override double Coefficient_A_Broad(int Octave)
            {
                return RI_Coef[Octave];
            }

            public override System.Numerics.Complex Reflection_Narrow(double frequency)
            {
                System.Numerics.Complex alpha = 0;
                for(int a = 0; a < Transfer_FunctionR.Length; a++) alpha += new System.Numerics.Complex(Transfer_FunctionR[a].Interpolate(frequency), Transfer_FunctionI[a].Interpolate(frequency)); 
                alpha /= Transfer_FunctionR.Length;
                return alpha;
            }

            public override System.Numerics.Complex Reflection_Narrow(double frequency, Hare.Geometry.Vector Dir, Hare.Geometry.Vector Normal)
            {
                int a = (int)(Math.Abs(Hare.Geometry.Hare_math.Dot(Dir, Normal))*180/Math.PI / 18);
                return new System.Numerics.Complex(Transfer_FunctionR[a].Interpolate(frequency), Transfer_FunctionI[a].Interpolate(frequency));
            }

            public  class Finite_Field_Impedance
            {
                MathNet.Numerics.Interpolation.CubicSpline[] Zr_Curves_R;
                MathNet.Numerics.Interpolation.CubicSpline[] Zr_Curves_I;

                public Finite_Field_Impedance(double Xdim, double Ydim, double freq_limit, double c_sound, double air_density)
                {
                    List<double> freq = new List<double>();
                    double f = 15.625;
                    int ct = 1;
                    while (f < freq_limit)
                    {
                        ct++;
                        f = 15.625 * Math.Pow(2, (double)ct / 3f);
                        freq.Add(f);
                    }
                    double[] anglesdeg = new double[(int)(180 / 5)];
                    anglesdeg[0] = -87.5;
                    for (int i = 1; i < anglesdeg.Length; i++) anglesdeg[i] = anglesdeg[i-1] + 5;

                    System.Numerics.Complex[][] Zr = AbsorptionModels.Operations.Finite_Radiation_Impedance_Atalla_Rect(Xdim, Ydim, freq.ToArray(), anglesdeg, c_sound, air_density);

                    Zr_Curves_R = new MathNet.Numerics.Interpolation.CubicSpline[Zr[0].Length];
                    Zr_Curves_I = new MathNet.Numerics.Interpolation.CubicSpline[Zr[0].Length];
                    for (int a = 0; a < Zr_Curves_R.Length; a++)
                    {
                        double[] ZR = new double[freq.Count];
                        double[] ZI = new double[freq.Count];
                        for (int fr = 0; fr < freq.Count; fr++)
                        {
                            ZR[fr] = Zr[fr][a].Real;
                            ZI[fr] = Zr[fr][a].Imaginary;
                        }
                        Zr_Curves_R[a] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(freq, ZR);
                        Zr_Curves_I[a] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(freq, ZI);
                    }
                }
                
                public System.Numerics.Complex[][] Interpolate(double[] freq)
                {
                    System.Numerics.Complex[][] Zr = new System.Numerics.Complex[freq.Length][];

                    for (int f = 0; f < freq.Length; f++)
                    {
                        Zr[f] = new System.Numerics.Complex[Zr_Curves_R.Length];
                        for (int a = 0; a < Zr_Curves_R.Length; a++)
                        {
                            Zr[f][a] = new System.Numerics.Complex(Zr_Curves_R[a].Interpolate(freq[f]), Zr_Curves_I[a].Interpolate(freq[f]));
                        }
                    }
                    return Zr;
                }
            }

            public override System.Numerics.Complex[] Reflection_Spectrum(int sample_frequency, int length, Hare.Geometry.Vector Normal, Hare.Geometry.Vector Dir, int threadid)
            {
                int a = (int)(Math.Abs(Hare.Geometry.Hare_math.Dot(Dir, Normal))*180/Math.PI / 18);

                System.Numerics.Complex[] Ref_trns = new System.Numerics.Complex[length];

                for (int j = 0; j < length; j++)
                {
                    double freq = j * (sample_frequency / 2) / length;
                    Ref_trns[j] = new System.Numerics.Complex(Transfer_FunctionR[a].Interpolate(freq), Transfer_FunctionI[a].Interpolate(freq));
                }

                return Ref_trns;
            }
        }

        public class Lambert_Scattering : Scattering
        {
            double[,] Scattering_Coefficient;
            public Lambert_Scattering(double[] Scattering, double SplitRatio)
            {
                Scattering_Coefficient = new double[8, 3];
                for (int oct = 0; oct < 8; oct++)
                {
                    double Mod = ((Scattering[oct] < (1 - Scattering[oct])) ? (Scattering[oct] * SplitRatio / 2) : ((1 - Scattering[oct]) * SplitRatio / 2));
                    Scattering_Coefficient[oct, 1] = Scattering[oct];
                    Scattering_Coefficient[oct, 0] = Scattering_Coefficient[oct, 1] - Mod;
                    Scattering_Coefficient[oct, 2] = Scattering_Coefficient[oct, 1] + Mod;
                }
            }
            public override double[] Coefficient()
            {
                double[] Scat = new double[8];
                for (int oct = 0; oct < 8; oct++) Scat[oct] = Scattering_Coefficient[oct, 1];
                return Scat;
            }

            public override double Coefficient(int octave)
            {
                return Scattering_Coefficient[octave, 1];
            }
            
            public override void Scatter_Early(ref BroadRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, Hare.Geometry.Vector Normal, double Cos_Theta)
            {
                double roughness_chance = rand.NextDouble();
                if (Cos_Theta > 0)
                {
                    Normal *= -1;
                    Cos_Theta *= -1;
                }
                    
                foreach (int oct in Ray.Octaves)
                {
                    // 3. Apply Scattering.
                    //// a. Create new source for scattered energy (E * Scattering).
                    //// b. Modify E (E * 1 - Scattering).
                    OctaveRay R = Ray.SplitRay(oct, Scattering_Coefficient[oct, 1]);

                    Hare.Geometry.Vector diffx;
                    Hare.Geometry.Vector diffy;
                    Hare.Geometry.Vector diffz;
                    double proj;
                    //Check that the ray and the normal are both on the same side...
                    diffz = Normal;
                    diffx = new Hare.Geometry.Vector(0, 0, 1);
                    proj = Math.Abs(Hare.Geometry.Hare_math.Dot(diffz, diffx));

                    if (0.99 < proj && 1.01 > proj) diffx = new Hare.Geometry.Vector(1, 0, 0);
                    diffy = Hare.Geometry.Hare_math.Cross(diffz, diffx);
                    diffx = Hare.Geometry.Hare_math.Cross(diffy, diffz);
                    diffx.Normalize();
                    diffy.Normalize();
                    diffz.Normalize();

                    double u1;
                    double u2;
                    double x;
                    double y;
                    double z;
                    Hare.Geometry.Vector vect;
                    u1 = 2.0 * Math.PI * rand.NextDouble();
                    // random azimuth
                    double Scat_Mod = rand.NextDouble();
                    u2 = Math.Acos(Scat_Mod);
                    // random zenith (elevation)
                    x = Math.Cos(u1) * Math.Sin(u2);
                    y = Math.Sin(u1) * Math.Sin(u2);
                    z = Math.Cos(u2);

                    vect = (diffx * x) + (diffy * y) + (diffz * z);
                    vect.Normalize();

                    //Return the new direction
                    R.direction = vect;

                    if (R.t_sum == 0)
                    {
                        Rhino.RhinoApp.Write("Something's up!");
                    }

                    Rays.Enqueue(R);

                }
                Ray.direction -= Normal * Cos_Theta * 2;
            }

            public override void Scatter_VeryLate(ref OctaveRay Ray, ref Random rand, Hare.Geometry.Vector Normal, double Cos_Theta)
            {
                if (rand.NextDouble() < Scattering_Coefficient[Ray.Octave, 1])
                {
                    Hare.Geometry.Vector diffx;
                    Hare.Geometry.Vector diffy;
                    Hare.Geometry.Vector diffz;
                    double proj;
                    //Check that the ray and the normal are both on the same side...
                    if (Cos_Theta > 0) Normal *= -1;
                    diffz = Normal;
                    diffx = new Hare.Geometry.Vector(0, 0, 1);
                    proj = Math.Abs(Hare.Geometry.Hare_math.Dot(diffz, diffx));

                    if (0.99 < proj && 1.01 > proj) diffx = new Hare.Geometry.Vector(1, 0, 0);
                    diffy = Hare.Geometry.Hare_math.Cross(diffz, diffx);
                    diffx = Hare.Geometry.Hare_math.Cross(diffy, diffz);
                    diffx.Normalize();
                    diffy.Normalize();
                    diffz.Normalize();

                    double u1;
                    double u2;
                    double x;
                    double y;
                    double z;
                    Hare.Geometry.Vector vect;
                    u1 = 2.0 * Math.PI * rand.NextDouble();
                    // random azimuth
                    double Scat_Mod = rand.NextDouble();
                    u2 = Math.Acos(Scat_Mod);
                    // random zenith (elevation)
                    x = Math.Cos(u1) * Math.Sin(u2);
                    y = Math.Sin(u1) * Math.Sin(u2);
                    z = Math.Cos(u2);

                    vect = (diffx * x) + (diffy * y) + (diffz * z);
                    vect.Normalize();

                    //Return the new direction
                    Ray.direction = vect;
                }
                else
                {
                    //Specular Reflection
                    Ray.direction -= Normal * Cos_Theta * 2;
                }
            }
            
            public override void Scatter_Late(ref OctaveRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, Hare.Geometry.Vector Normal, double Cos_Theta)
            {
                double scat_sel = rand.NextDouble();
                if (scat_sel > Scattering_Coefficient[Ray.Octave, 2])
                {
                    // Specular Reflection
                    Ray.direction -= Normal * Cos_Theta * 2;
                    return;
                }
                else if (scat_sel > Scattering_Coefficient[Ray.Octave, 0])
                {
                    //Only for a certain portion of high benefit cases--
                    //// a. Create new source for scattered energy (E * Scattering).
                    //// b. Modify E (E * 1 - Scattering).
                    //Create a new ray...
                    OctaveRay tr = Ray.SplitRay(1 - Scattering_Coefficient[Ray.Octave,1]);
                    // this is the specular reflection. Save it for later.
                    tr.direction -= Normal * Cos_Theta * 2;

                    if (tr.t_sum == 0)
                    {
                        Rhino.RhinoApp.Write("Something's up!");
                    }

                    Rays.Enqueue(tr);
                }

                //If we are here, the original ray needs a scattered direction:
                Hare.Geometry.Vector diffx;
                Hare.Geometry.Vector diffy;
                Hare.Geometry.Vector diffz;
                double proj;
                //Check that the ray and the normal are both on the same side...
                if (Cos_Theta > 0) Normal *= -1;
                diffz = Normal;
                diffx = new Hare.Geometry.Vector(0, 0, 1);
                proj = Math.Abs(Hare.Geometry.Hare_math.Dot(diffz, diffx));

                if (0.99 < proj && 1.01 > proj) diffx = new Hare.Geometry.Vector(1, 0, 0);
                diffy = Hare.Geometry.Hare_math.Cross(diffz, diffx);
                diffx = Hare.Geometry.Hare_math.Cross(diffy, diffz);
                diffx.Normalize();
                diffy.Normalize();
                diffz.Normalize();

                double u1;
                double u2;
                double x;
                double y;
                double z;
                Hare.Geometry.Vector vect;
                u1 = 2.0 * Math.PI * rand.NextDouble();
                // random azimuth
                double Scat_Mod = rand.NextDouble();
                u2 = Math.Acos(Scat_Mod);
                // random zenith (elevation)
                x = Math.Cos(u1) * Math.Sin(u2);
                y = Math.Sin(u1) * Math.Sin(u2);
                z = Math.Cos(u2);

                vect = (diffx * x) + (diffy * y) + (diffz * z);
                vect.Normalize();

                //Return the new direction
                Ray.direction = vect;    
            }
        }
    }
}