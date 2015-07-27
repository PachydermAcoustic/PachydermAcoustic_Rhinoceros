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
        public abstract class Medium_Properties
        {
            /// <summary>
            /// Get density of medium
            /// </summary>
            /// <param name="Tk">Temperature in Kelvins</param>
            /// <param name="Pa">Atmospheric pressure in Pascals</param>
            /// <param name="hr">Relative Humidity in percent</param>
            /// <returns></returns>
            protected double Calculate_Density(double Tk, double Pa, double hr)
            {
                double Rd = 287.05;
                double T = Tk - 273.15;
                double Pv = hr * 6.1078 * Math.Pow(10, (7.5 * T)/(237.3 + T)); //Pascals
                //return ((Pd / Rd) + (Pv / Rv)) / T;
                return (Pa / (Rd * Tk)) * (1 - 0.378*Pv/Pa); //kg/cubic meter
            }

            protected double[] Calculate_Attenuation(int Air_Choice, double Pa, double Tk, double hr, bool EdgeCorrection)
            {
                double[] Freq = new double[8] { 62.5, 125, 250, 500, 1000, 2000, 4000, 8000};
                double[] Att_Coef = new double[8];

                if (Air_Choice == 0)
                {
                    //Attenuation by ISO 9613-1: Cited from Eric Desart, http://forum.studiotips.com/viewtopic.php?t=158
                    //Pt = Pi * exp(-x * as) [Pa]
                    //x = 1/(10 * log((exp(1))^2) = ca 0.1151 (value in norm, formula E. Desart)
                    //Delta Lt = 10 * log( Pi^2 / Pt^2 ) = as [dB]
                    //a = 8.686 * f ^2 * ((1.84 * 10^-11 * (Pa / Pr)^-1 * (T / To)^(1/2)) + y) [dB/m]
                    //y = (T / To)^(-5/2) * (0.01275 * exp(-2239.1 / T) * (frO + f ^2 / frO)^-1 + z)
                    //z = 0.1068 * exp(-3352 / T) * (frN + f ^ 2 / frN) ^ -1
                    //frO = (Pa / Pr) * (24 + 4.04 * 10 ^ 4 * h * ((0.02 + h) / (0.391 + h)))
                    //frN = (Pa / Pr)*(T / To)^(-1/2) * (9 + 280 * h * exp(-4.170 * ((T / To)^(-1/3)-1)))
                    //h = hr * ((Psat / Pr) / (Pa / Pr)) = hr * (Psat / Pa)
                    //Psat = Pr * 10 ^ (-6.8346 * (To1 / T) ^ 1.261 + 4.6151)

                    //Attenuation in pressure:
                    //pi = p0 * exp(-0.1151 a * s)
                    //Attenuation in intensity
                    //I = I0 * Power(10,-a*s)

                    double T = Tk - 273.15;
                    //double Psat = 101.325 * Math.Pow(10, (-6.8346 * Math.Pow((273.16 / Tk), 1.261) + 4.6151));
                    //double Psat = hr * 6.1078 * Math.Pow(10, (7.5 * T) / (237.3 + T))/1000;
                    //double h = (Psat / 101.325) / (Pa / 101.325);
                    double h = hr * Math.Pow(10, -6.8346 * Math.Pow(273.16 / Tk, 1.261) + 4.6151) / (Pa / 101.325);

                    for (int oct = 0; oct < 8; oct++)
                    {
                        double frO = (Pa / 101.325) * (24 + 4.04 * Math.Pow(10, 4) * h * ((0.02 + h) / (0.391 + h)));
                        double frN = (Pa / 101.325) * Math.Pow(Tk / 293.15, -1 / 2) * (9 + 280 * h * Math.Exp(-4.170 * (Math.Pow((Tk / 293.15), (-1 / 3)) - 1)));
                        double z = 0.1068 * Math.Exp(-3352 / Tk) * Math.Pow((frN + Freq[oct] * Freq[oct] / frN), -1);
                        double y = Math.Pow(Tk / 293.15, -5 / 2) * (0.01275 * Math.Exp(-2239.1 / Tk) * Math.Pow(frO + Freq[oct] * Freq[oct] / frO, -1) + z);
                        Att_Coef[oct] = 8.686 * Freq[oct] * Freq[oct] * ((1.84 * Math.Pow(10, -11) * Math.Pow((Pa / 101.325), -1) * Math.Pow((Tk / 293.15), (1 / 2))) + y);//m-1         
                    }
                }
                else if (Air_Choice == 1)
                {
                    //'Evans and Bazley, cited in Cremer & Muller
                    //'1 Np = = 8.7 dB
                    //'rh = relative humidity in %
                    //'m = (85 / rh)* f^2 * 0.0001 in Np/m

                    for (int q = 0; q <= 7; q++)
                    {
                        Att_Coef[q] = ((85 / hr) * (Math.Pow((Freq[q] / 1000), 2) * 0.0001)) / 8.7;
                        Att_Coef[q] -= 0.04 * (Tk - 273.15 - 20) * Att_Coef[q];// Np/m
                        Att_Coef[q] *= 0.50248756218905477; //m-1
                    }
                }
                if (Air_Choice == 2)
                {
                    if (Tk - 273.15 > 15)
                    {
                        if (hr < 50)
                        {
                            Att_Coef[0] = 0.0001;
                            Att_Coef[1] = 0.0001;
                            Att_Coef[2] = 0.0003;
                            Att_Coef[3] = 0.0006;
                            Att_Coef[4] = 0.001;
                            Att_Coef[5] = 0.0019;
                            Att_Coef[6] = 0.0058;
                            Att_Coef[7] = 0.0203;
                        }
                        else if (hr < 70)
                        {
                            Att_Coef[0] = 0.0001;
                            Att_Coef[1] = 0.0001;
                            Att_Coef[2] = 0.0003;
                            Att_Coef[3] = 0.0006;
                            Att_Coef[4] = 0.001;
                            Att_Coef[5] = 0.0017;
                            Att_Coef[6] = 0.0041;
                            Att_Coef[7] = 0.0135;
                        }
                        else if (hr > 70)
                        {
                            Att_Coef[0] = 0.0001;
                            Att_Coef[1] = 0.0001;
                            Att_Coef[2] = 0.0003;
                            Att_Coef[3] = 0.0006;
                            Att_Coef[4] = 0.0011;
                            Att_Coef[5] = 0.0017;
                            Att_Coef[6] = 0.0035;
                            Att_Coef[7] = 0.0106;
                        }
                    }
                    else
                    {
                        if (hr < 50)
                        {
                            Att_Coef[0] = 0.0001;
                            Att_Coef[1] = 0.0001;
                            Att_Coef[2] = 0.0002;
                            Att_Coef[3] = 0.0005;
                            Att_Coef[4] = 0.0011;
                            Att_Coef[5] = 0.0027;
                            Att_Coef[6] = 0.0094;
                            Att_Coef[7] = 0.029;
                        }
                        else if (hr < 70)
                        {
                            Att_Coef[0] = 0.0001;
                            Att_Coef[1] = 0.0001;
                            Att_Coef[2] = 0.0002;
                            Att_Coef[3] = 0.0005;
                            Att_Coef[4] = 0.0008;
                            Att_Coef[5] = 0.0018;
                            Att_Coef[6] = 0.0059;
                            Att_Coef[7] = 0.0211;
                        }
                        else if (hr > 70)
                        {
                            Att_Coef[0] = 0.0001;
                            Att_Coef[1] = 0.0001;
                            Att_Coef[2] = 0.0002;
                            Att_Coef[3] = 0.0005;
                            Att_Coef[4] = 0.0007;
                            Att_Coef[5] = 0.0014;
                            Att_Coef[6] = 0.0044;
                            Att_Coef[7] = 0.0158;
                        }
                    }
                    ////Edge Frequency correction: R.H.C. Wenmaekers et. al.
                    ////Air Absorption Error in Room Acoustical Modeling
                    if (EdgeCorrection == true)
                    {
                        for (int i = 0; i < Att_Coef.Length; i++)
                        {
                            //m-band = Frequency-LowerEdge * m-pure / Frequency-Center;
                            //Att_Coef[i] = (Freq[i]/Math.Sqrt(2)) * Att_Coef[i] / Freq[i];
                            Att_Coef[i] /= Math.Sqrt(2);
                        }
                    }
                }
                return Att_Coef;
            }

            public static double ISO9613_1_attencoef(double Freq, double Tk, double Pa, double hr)
            {
                double T = Tk - 273.15;
                double h = hr * Math.Pow(10, -6.8346 * Math.Pow(273.16 / Tk, 1.261) + 4.6151) / (Pa / 101.325);
                double frO = (Pa / 101.325) * (24 + 4.04 * Math.Pow(10, 4) * h * ((0.02 + h) / (0.391 + h)));
                double frN = (Pa / 101.325) * Math.Pow(Tk / 293.15, -1 / 2) * (9 + 280 * h * Math.Exp(-4.170 * (Math.Pow((Tk / 293.15), (-1 / 3)) - 1)));
                double z = 0.1068 * Math.Exp(-3352 / Tk) * Math.Pow((frN + Freq * Freq / frN), -1);
                double y = Math.Pow(Tk / 293.15, -5 / 2) * (0.01275 * Math.Exp(-2239.1 / Tk) * Math.Pow(frO + Freq * Freq / frO, -1) + z);
                return 8.686 * Freq * Freq * ((1.84 * Math.Pow(10, -11) * Math.Pow((Pa / 101.325), -1) * Math.Pow((Tk / 293.15), (1 / 2))) + y);
            }

            public static double[] ISO9613_1_attencoef(double[] Freq, double Tk, double Pa, double hr)
            {
                double T = Tk - 273.15;
                double h = hr * Math.Pow(10, -6.8346 * Math.Pow(273.16 / Tk, 1.261) + 4.6151) / (Pa / 101.325);
                double frO = (Pa / 101.325) * (24 + 4.04 * Math.Pow(10, 4) * h * ((0.02 + h) / (0.391 + h)));
                double frN = (Pa / 101.325) * Math.Pow(Tk / 293.15, -1 / 2) * (9 + 280 * h * Math.Exp(-4.170 * (Math.Pow((Tk / 293.15), (-1 / 3)) - 1)));

                double[] Atten = new double[Freq.Length];

                for (int i = 0; i < Atten.Length; i++)
                {
                    double z = 0.1068 * Math.Exp(-3352 / Tk) * Math.Pow((frN + Freq[i] * Freq[i] / frN), -1);
                    double y = Math.Pow(Tk / 293.15, -5 / 2) * (0.01275 * Math.Exp(-2239.1 / Tk) * Math.Pow(frO + Freq[i] * Freq[i] / frO, -1) + z);
                    Atten[i] = 8.686 * Freq[i] * Freq[i] * ((1.84 * Math.Pow(10, -11) * Math.Pow((Pa / 101.325), -1) * Math.Pow((Tk / 293.15), (1 / 2))) + y);
                }

                return Atten;
            }

            public abstract double[] Attenuation_Coef(int arg);
            public abstract double[] Attenuation_Coef(Hare.Geometry.Point pt);
            public abstract double Attenuation_Coef(int arg, int octave);
            public abstract double Sound_Speed(Hare.Geometry.Point pt);
            public abstract double Sound_Speed(Rhino.Geometry.Point3d pt);
            public abstract double Sound_Speed(int arg);
            public abstract double Rho(Hare.Geometry.Point pt);
            public abstract double Rho(int arg);
            public abstract double Rho_C(Hare.Geometry.Point pt);
            public abstract double Rho_C(int arg);
            public abstract void AttenuationFilter(int no_of_elements, int sample_Frequency, double distance_meters, ref double[] Freq, ref double[] Atten, Hare.Geometry.Point pt);
            public abstract double AttenuationPureTone(Hare.Geometry.Point pt, double frequency);
        }

        public class Uniform_Medium: Medium_Properties
        {
            double[] Atten_Coef;
            double C_Sound;
            double rho; //Density
            double Zmed;

            MathNet.Numerics.Interpolation.CubicSpline Spectrum;
            
            public void ISO9613_1_Spline(double Tk, double Pa, double Hr)
            {
                double[] freq = new double[1024];
                double df = 22050/1024;
                for (int i = 1; i < 1025; i++)
                {
                    freq[i-1] = df * i;
                }

                Spectrum = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(freq, ISO9613_1_attencoef(freq, Tk, Pa, Hr));
            }

            public override void AttenuationFilter(int no_of_elements, int sample_Frequency, double distance_meters, ref double[] Freq, ref double[] Atten, Hare.Geometry.Point pt)
            {
                double df = sample_Frequency / no_of_elements;

                Freq = new double[no_of_elements];
                Atten = new double[no_of_elements];

                for (int i = 0; i < no_of_elements; i++)
                {
                    Freq[i] = df * (i + 1);
                    Atten[i] = Math.Exp(-.1151 * Spectrum.Interpolate(Freq[i]) * distance_meters);
                }
            }

            public override double AttenuationPureTone(Hare.Geometry.Point pt, double frequency)
            {
                return Spectrum.Interpolate(frequency);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Air_Choice"></param>
            /// <param name="Pa">in Pascals</param>
            /// <param name="Tk">in kelvins</param>
            /// <param name="hr">in percent</param>
            /// <param name="EdgeCorrection"></param>
            public Uniform_Medium(int Air_Choice, double Pa, double Tk, double hr, bool EdgeCorrection)
            {
                rho = Calculate_Density(Tk, Pa*100, hr);
                Atten_Coef = Calculate_Attenuation(Air_Choice, Pa/10, Tk, hr, EdgeCorrection);
                for (int i = 0; i < Atten_Coef.Length; i++) Atten_Coef[i] *= 0.1151;
                C_Sound = Utilities.AcousticalMath.SoundSpeed(Tk-273.15);
                Zmed = rho * C_Sound;
                ISO9613_1_Spline(Tk, Pa, hr);
            }

            public override double[] Attenuation_Coef(int not)
            {
                return Atten_Coef;
            }

            public override double Attenuation_Coef(int arg, int octave)
            {
                return Atten_Coef[octave];
            }

            public override double[] Attenuation_Coef(Hare.Geometry.Point pt)
            {
                return Atten_Coef;
            }

            public override double Sound_Speed(Hare.Geometry.Point pt)
            {
                return C_Sound;
            }

            public override double Sound_Speed(Rhino.Geometry.Point3d pt)
            {
                return C_Sound;
            }

            public override double Sound_Speed(int arg)
            {
                return C_Sound;
            }

            public override double Rho(Hare.Geometry.Point pt)
            {
                return rho;
            }

            public override double Rho(int arg)
            {
                return rho;    
            }

            public override double Rho_C(Hare.Geometry.Point pt)
            {
                return Zmed;
            }

            public override double Rho_C(int arg)
            {
                return Zmed;
            }
        }

        public class Heterogeneous_Grid_Medium: Medium_Properties
        {
            double[] C_Sound;
            double[][] Atten_Coef;
            double[] rho;
            double[] Zmed;
            int Xdom, Ydom, Zdom, XYTot;
            double MinX, MinY, MinZ;
            double VdimX, VdimY, VdimZ;

            MathNet.Numerics.Interpolation.CubicSpline[] Spectrum;
            
            public Heterogeneous_Grid_Medium(int Air_Choice, double[, ,] Pa, double[, ,] TC, double[, ,] hr, bool EdgeCorrection, VoxelGrid_PolyRefractive V)
            {
                VdimX = V.Xdim;
                VdimY = V.Ydim;
                VdimZ = V.Zdim;
                Hare.Geometry.Point min = V.MinPt;
                MinX = min.x;
                MinY = min.y;
                MinZ = min.z;
                Xdom = Pa.GetLength(0);
                Ydom = Pa.GetLength(1);
                Zdom = Pa.GetLength(2);
                XYTot = Xdom * Ydom;
                Atten_Coef = new double[Xdom * Ydom * Zdom][];
                C_Sound = new double[Xdom * Ydom * Zdom];
                rho = new double[Xdom * Ydom * Zdom];
                Zmed = new double[Xdom * Ydom * Zdom];
                Spectrum = new MathNet.Numerics.Interpolation.CubicSpline[Xdom * Ydom * Zdom];

                for (int x = 0; x < Xdom; x++)
                {
                    for (int y = 0; y < Ydom; y++)
                    {
                        for(int z = 0; z < Zdom; z++)
                        {
                            int code = XYTot * z + Ydom * x + y;
                            Atten_Coef[code] = Calculate_Attenuation(Air_Choice, Pa[x,y,z], TC[x,y,z], hr[x,y,z], EdgeCorrection);
                            rho[code] = Calculate_Density(TC[x,y,z], Pa[x,y,z], hr[x,y,z]);//TODO - check input
                            C_Sound[code] = Utilities.AcousticalMath.SoundSpeed(TC[x,y,z] - 273.15);
                            Zmed[code] = rho[code] * C_Sound[code];

                            //TODO: Is TC termperature in Celsius, or Kelvins?
                            ISO9613_1_Spline(TC[x,y,z], Pa[x,y,z], hr[x,y,z], code);
                        }
                    }
                }
            }

            public void ISO9613_1_Spline(double Tk, double Pa, double Hr, int code)
            {
                double[] freq = new double[1024];
                double df = 22050 / 1024;
                for (int i = 1; i < 1025; i++)
                {
                    freq[i - 1] = df * i;
                }

                Spectrum[code] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(freq, ISO9613_1_attencoef(freq, Tk, Pa, Hr));
            }

            public override void AttenuationFilter(int no_of_elements, int sample_Frequency, double distance_meters, ref double[] Freq, ref double[] Atten, Hare.Geometry.Point pt)
            {
                double df = sample_Frequency / no_of_elements;
                int X = (int)Math.Floor((pt.x - MinX) / VdimX);
                int Y = (int)Math.Floor((pt.y - MinY) / VdimY);
                int Z = (int)Math.Floor((pt.z - MinZ) / VdimZ);

                for (int i = 0; i < no_of_elements; i++)
                {
                    Freq[i] = df * (i + 1);
                    Atten[i] = Math.Exp(-.1151 * this.Spectrum[XYTot * Z + Ydom * X + Y].Interpolate(Freq[i]) * distance_meters);
                }
            }
            
            public override double[] Attenuation_Coef(int voxelCode)
            {
                return Atten_Coef[voxelCode];
            }

            public override double Attenuation_Coef(int voxelCode, int octave)
            {
                return Atten_Coef[voxelCode][octave];
            }

            public override double[] Attenuation_Coef(Hare.Geometry.Point pt)
            {
                int X = (int)Math.Floor((pt.x - MinX) / VdimX);
                int Y = (int)Math.Floor((pt.y - MinY) / VdimY);
                int Z = (int)Math.Floor((pt.z - MinZ) / VdimZ);

                return Atten_Coef[XYTot * Z + Ydom * X + Y];
            }

            public override double Sound_Speed(Hare.Geometry.Point pt)
            {
                int X = (int)Math.Floor((pt.x - MinX) / VdimX);
                int Y = (int)Math.Floor((pt.y - MinY) / VdimY);
                int Z = (int)Math.Floor((pt.z - MinZ) / VdimZ);

                return C_Sound[XYTot * Z + Ydom * X + Y];
            }

            public override double AttenuationPureTone(Hare.Geometry.Point pt, double frequency)
            {
                int X = (int)Math.Floor((pt.x - MinX) / VdimX);
                int Y = (int)Math.Floor((pt.y - MinY) / VdimY);
                int Z = (int)Math.Floor((pt.z - MinZ) / VdimZ);

                return Spectrum[XYTot * Z + Ydom * X + Y].Interpolate(frequency);
            }

            public override double Sound_Speed(Rhino.Geometry.Point3d pt)
            {
                int X = (int)Math.Floor((pt.X - MinX) / VdimX);
                int Y = (int)Math.Floor((pt.Y - MinY) / VdimY);
                int Z = (int)Math.Floor((pt.Z - MinZ) / VdimZ);

                return C_Sound[XYTot * Z + Ydom * X + Y];
            }

            public override double Sound_Speed(int voxelCode)
            {
                return C_Sound[voxelCode];
            }

            public override double Rho(Hare.Geometry.Point pt)
            {
                int X = (int)Math.Floor((pt.x - MinX) / VdimX);
                int Y = (int)Math.Floor((pt.y - MinY) / VdimY);
                int Z = (int)Math.Floor((pt.z - MinZ) / VdimZ);

                return rho[XYTot * Z + Ydom * X + Y];
            }

            public override double Rho(int arg)
            {
                return rho[arg];
            }

            public override double Rho_C(Hare.Geometry.Point pt)
            {
                int X = (int)Math.Floor((pt.x - MinX) / VdimX);
                int Y = (int)Math.Floor((pt.y - MinY) / VdimY);
                int Z = (int)Math.Floor((pt.z - MinZ) / VdimZ);

                return Zmed[XYTot * Z + Ydom * X + Y];
            }

            public override double Rho_C(int arg)
            {
                return Zmed[arg];
            }
        }
    }
}
