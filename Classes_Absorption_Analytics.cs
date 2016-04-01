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
using System.Threading.Tasks;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace Pachyderm_Acoustic
{
    namespace AbsorptionModels
    {
        [Serializable]
        public class ABS_Layer
        {
            public LayerType T;
            public double depth;
            public double pitch;
            public double width;
            public double Flow_Resist;
            public double porosity;

            public double YoungsModulus;
            public double density;//Frame density, or solid density.
            public double PoissonsRatio;
            public double tortuosity;
            public double Viscous_Characteristic_Length;
            public double Thermal_Permeability;
            public double SpeedOfSound;

            public ABS_Layer ()
            { }

            public static ABS_Layer CreateBiot(bool rigid, double depth_in, double density_in, double YoungsModulus_in, double PoissonsRatio_in, double SpeedOfSound_In, double tortuosity_in, double flow_resistivity_in, double porosity_in, double Viscous_Characteristic_Length, double Thermal_Permeability)
            {
                ABS_Layer Layer = new ABS_Layer();
                if (rigid) Layer.T = LayerType.BiotPorousAbsorber_Rigid;
                else Layer.T = LayerType.BiotPorousAbsorber_Limp;

                Layer.depth = depth_in;
                Layer.density = density_in;
                Layer.YoungsModulus = YoungsModulus_in;
                Layer.PoissonsRatio = PoissonsRatio_in;
                Layer.SpeedOfSound = SpeedOfSound_In;
                Layer.tortuosity = tortuosity_in;
                Layer.Flow_Resist = flow_resistivity_in;
                Layer.porosity = porosity_in;
                Layer.Viscous_Characteristic_Length = Viscous_Characteristic_Length;
                Layer.Thermal_Permeability = Thermal_Permeability;
                return Layer;
            }

            public static ABS_Layer CreateSolid(double depth_in, double density_in, double YoungsModulus_in, double PoissonsRatio_in)
            {
                ABS_Layer Layer = new ABS_Layer();
                Layer.T = LayerType.SolidPlate;
                Layer.depth = depth_in;
                Layer.density = density_in;
                Layer.YoungsModulus = YoungsModulus_in;
                Layer.PoissonsRatio = PoissonsRatio_in;
                return Layer;
            }

            public ABS_Layer(LayerType T_in, double depth_in, double pitch_in, double width_in, double Flow_Resistivity, double _porosity)
            {
                T = T_in;
                depth = depth_in;
                pitch = pitch_in;
                width = width_in;
                Flow_Resist = Flow_Resistivity;
                porosity = _porosity;
            }

            public static ABS_Layer Create_CA(double depth_in, double Flow_Resistance, double _porosity, double YoungsModulus, double density, double PoissonsRatio, double tortuosity, double Viscous_Characteristic_Length, double Thermal_Permeability, double SpeedofSound)
            {
                ABS_Layer Layer = new ABS_Layer();
                Layer.T = LayerType.PorousCA;
                Layer.depth = depth_in;
                Layer.Flow_Resist = Flow_Resistance;
                Layer.porosity = _porosity;
                return Layer;
            }

            public static ABS_Layer Create_DB(double depth_in, double Flow_Resistance, double _porosity, double YoungsModulus, double density, double PoissonsRatio, double tortuosity, double Viscous_Characteristic_Length, double Thermal_Permeability, double SpeedofSound)
            {
                ABS_Layer Layer = new ABS_Layer();
                Layer.T = LayerType.PorousDB;
                Layer.depth = depth_in;
                Layer.Flow_Resist = Flow_Resistance;
                Layer.porosity = _porosity;
                return Layer;
            }

            public static ABS_Layer Create_Miki(double depth_in, double Flow_Resistance, double _porosity, double YoungsModulus, double density, double PoissonsRatio, double tortuosity, double Viscous_Characteristic_Length, double Thermal_Permeability, double SpeedofSound)
            {
                ABS_Layer Layer = new ABS_Layer();
                Layer.T = LayerType.PorousM;
                Layer.depth = depth_in;
                Layer.Flow_Resist = Flow_Resistance;
                Layer.porosity = _porosity;
                return Layer;
            }

            public enum LayerType
            {
                AirSpace = 0,
                PorousDB = 1,
                PorousCA = 2,
                PorousM = 3,
                Perforated_Modal = 4,
                Slotted_Modal = 5,
                SquarePerforations = 6,
                CircularPerforations = 7,
                Slots = 8,
                Microslit = 9,
                MicroPerforated = 10,
                BiotPorousAbsorber_Rigid = 15,
                BiotPorousAbsorber_Limp = 16,
                SolidPlate = 20,
                ThinPlate = 21
            }

            public override string ToString()
            {
                switch (T)
                {
                    case LayerType.AirSpace:
                        return string.Format("{0}: T= {1}", T.ToString(), depth);
                    case LayerType.PorousDB:
                        return string.Format("{0}: T= {1}, sigma= {2}", T.ToString(), depth, Flow_Resist);
                    case LayerType.PorousCA:
                        return string.Format("{0}: T= {1}, sigma= {2}", T.ToString(), depth, Flow_Resist);
                    case LayerType.PorousM:
                        return string.Format("{0}: T= {1}, sigma= {2}", T.ToString(), depth, Flow_Resist);
                    case LayerType.BiotPorousAbsorber_Limp:
                        return string.Format("{0}: T = {1}, sigma = {2}, YM = {3}", T.ToString(), depth, Flow_Resist, YoungsModulus);
                    case LayerType.BiotPorousAbsorber_Rigid:
                        return string.Format("{0}: T = {1}, sigma = {2}, YM = {3}", T.ToString(), depth, Flow_Resist, YoungsModulus);
                    case LayerType.SolidPlate:
                        return string.Format("{0}: T = {1}, density = {2}, YM = {3}", T.ToString(), depth, density, YoungsModulus);
                    case LayerType.ThinPlate:
                        return string.Format("{0}: T = {1}, density = {2}", T.ToString(), depth, density, YoungsModulus);
                    default:
                        return string.Format("{0}: T= {1}, diam= {2}, pitch= {3}", T.ToString(), depth, width, pitch);
                }
            }

            public string LayerCode()
            {
                switch (T)
                {
                    case LayerType.AirSpace:
                        return string.Format("0:{0};", depth);
                    case LayerType.PorousDB:
                        return string.Format("1:{0}:{1};", depth, Flow_Resist);
                    case LayerType.PorousCA:
                        return string.Format("2:{0}:{1};", depth, Flow_Resist);
                    case LayerType.PorousM:
                        return string.Format("3:{0}:{1};", depth, Flow_Resist);
                    case LayerType.Perforated_Modal:
                        return string.Format("4:{0}:{1}:{2};", depth, width, pitch);
                    case LayerType.Slotted_Modal:
                        return string.Format("5:{0}:{1}:{2};", depth, width, pitch);
                    case LayerType.CircularPerforations:
                        return string.Format("6:{0}:{1}:{2};", depth, width, pitch);
                    case LayerType.SquarePerforations:
                        return string.Format("7:{0}:{1}:{2};", depth, width, pitch);
                    case LayerType.Slots:
                        return string.Format("8:{0}:{1}:{2};", depth, width, pitch);
                    case LayerType.MicroPerforated:
                        return string.Format("9:{0}:{1}:{2};", depth, width, pitch);
                    case LayerType.Microslit:
                        return string.Format("10:{0}:{1}:{2};", depth, width, pitch);
                    case LayerType.SolidPlate:
                        return string.Format("11:{0}:{1}:{2}:{3}", depth, density, YoungsModulus, PoissonsRatio);
                    case LayerType.BiotPorousAbsorber_Limp:
                        return string.Format("12:false:{0):{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}", depth, density, YoungsModulus, PoissonsRatio, SpeedOfSound, tortuosity, Flow_Resist, porosity, Viscous_Characteristic_Length, Thermal_Permeability);
                    case LayerType.BiotPorousAbsorber_Rigid:
                        return string.Format("12:true:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}", depth, density, YoungsModulus, PoissonsRatio, SpeedOfSound, tortuosity, Flow_Resist, porosity, Viscous_Characteristic_Length, Thermal_Permeability);
                    default:
                        throw new Exception("Unknown Layer Type");
                }
            }

            public static ABS_Layer LayerFromCode(string code)
            {
                string[] elements = code.Split(':');

                switch (elements[0])
                {
                    case "0":
                        return new ABS_Layer(LayerType.AirSpace, double.Parse(elements[1]), 0, 0, 0, 0);
                    case "1":
                        return new ABS_Layer(LayerType.PorousDB, double.Parse(elements[1]), 0, 0, double.Parse(elements[2]), 0);
                    case "2":
                        return new ABS_Layer(LayerType.PorousCA, double.Parse(elements[1]), 0, 0, double.Parse(elements[2]), 0);
                    case "3":
                        return new ABS_Layer(LayerType.PorousM, double.Parse(elements[1]), 0, 0, double.Parse(elements[2]), 0);
                    case "4":
                        return new ABS_Layer(LayerType.Perforated_Modal, double.Parse(elements[1]), double.Parse(elements[3]), double.Parse(elements[2]), 0, 0);
                    case "5":
                        return new ABS_Layer(LayerType.Slotted_Modal, double.Parse(elements[1]), double.Parse(elements[3]), double.Parse(elements[2]), 0, 0);
                    case "6":
                        return new ABS_Layer(LayerType.CircularPerforations, double.Parse(elements[1]), double.Parse(elements[3]), double.Parse(elements[2]), 0, 0);
                    case "7":
                        return new ABS_Layer(LayerType.CircularPerforations, double.Parse(elements[1]), double.Parse(elements[3]), double.Parse(elements[2]), 0, 0);
                    case "8":
                        return new ABS_Layer(LayerType.Slots, double.Parse(elements[1]), double.Parse(elements[3]), double.Parse(elements[2]), 0, 0);
                    case "9":
                        return new ABS_Layer(LayerType.MicroPerforated, double.Parse(elements[1]), double.Parse(elements[3]), double.Parse(elements[2]), 0, 0);
                    case "10":
                        return new ABS_Layer(LayerType.Microslit, double.Parse(elements[1]), double.Parse(elements[3]), double.Parse(elements[2]), 0, 0);
                    case "11":
                        return ABS_Layer.CreateSolid(double.Parse(elements[1]), double.Parse(elements[2]), double.Parse(elements[3]), double.Parse(elements[4]));
                    case "12":
                        return ABS_Layer.CreateBiot(bool.Parse(elements[1]), double.Parse(elements[2]), double.Parse(elements[3]), double.Parse(elements[4]), double.Parse(elements[5]), double.Parse(elements[6]), double.Parse(elements[7]), double.Parse(elements[8]), double.Parse(elements[9]), double.Parse(elements[10]), double.Parse(elements[11]));
                    default:
                        throw new Exception("Unknown Layer Type");
                }
            }

            public static ABS_Layer Airspace(double depth)
            {
                return new ABS_Layer(LayerType.AirSpace, depth, 0, 0, 0, 0);
            }

            public static ABS_Layer Porous_DelaneyBazley(double depth, double flow_resistivity)
            {
                return new ABS_Layer(LayerType.PorousDB, depth, 0, 0, flow_resistivity, 0);
            }

            public static ABS_Layer Porous_Miki(double depth, double flow_resistivity)
            {
                return new ABS_Layer(LayerType.PorousM, depth, 0, 0, flow_resistivity, 0);
            }

            public static ABS_Layer Porous_ChampouxAllard(double depth, double flow_resistivity)
            {
                return new ABS_Layer(LayerType.PorousCA, depth, 0, 0, flow_resistivity, 0);
            }
            public static ABS_Layer Perforated_Modal(double depth, double hole_pitch, double hole_width)
            {
                return new ABS_Layer(LayerType.Perforated_Modal, depth, hole_pitch, hole_width, 0, 0);
            }

            public static ABS_Layer Slotted_Modal(double depth, double slot_pitch, double slot_width)
            {
                return new ABS_Layer(LayerType.Slotted_Modal, depth, slot_pitch, slot_width, 0, 0);
            }

            public static ABS_Layer Perforated_EndCorrection(double depth, double hole_pitch, double hole_width)
            {
                return new ABS_Layer(LayerType.CircularPerforations, depth, hole_pitch, hole_width, 0, 0);
            }

            public static ABS_Layer Slotted_EndCorrection(double depth, double slot_pitch, double slot_width)
            {
                return new ABS_Layer(LayerType.Slots, depth, slot_pitch, slot_width, 0, 0);
            }

            public static ABS_Layer MicroPerf_EndCorrection(double depth, double hole_pitch, double hole_width)
            {
                return new ABS_Layer(LayerType.MicroPerforated, depth, hole_pitch, hole_width, 0, 0);
            }

            public static ABS_Layer MicroSlot_EndCorrection(double depth, double slot_pitch, double slot_width)
            {
                return new ABS_Layer(LayerType.Microslit, depth, slot_pitch, slot_width, 0, 0);
            }
        }
        public static class Operations
        {
            public static double[] Random_Incidence_Paris_Finite(double[][] absorption_Coefficient)
            {
                double dt = Math.PI / (absorption_Coefficient.Length);
                double[] numerator = new double[absorption_Coefficient[0].Length];
                //double denominator = 0;
                for (int a = 0; a < absorption_Coefficient.Length; a++)
                {
                    double theta = a * dt - (Math.PI / 2);
                    double d_mod = Math.Abs(Math.Sin(theta) * dt);//; * Math.Abs(dt);
                                                                  //denominator += d_mod;
                    for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                    {
                        numerator[f] += Math.Abs(absorption_Coefficient[a][f] * d_mod);
                    }
                }

                //for (int f = 0; f < absorption_Coefficient[0].Length; f++)
                //{
                //    numerator[f] /= denominator;
                //}

                return numerator;
            }

            public static double[] Random_Incidence_Paris(double[][] absorption_Coefficient)
            {
                double dt = Math.PI / (absorption_Coefficient.Length);
                double[] numerator = new double[absorption_Coefficient[0].Length];
                double denominator = 0;
                for (int a = 0; a < absorption_Coefficient.Length; a++)
                {
                    double theta = a * dt - (Math.PI / 2);
                    double d_mod = Math.Abs(Math.Cos(theta) * dt);
                    denominator += d_mod;
                    for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                    {
                        numerator[f] += absorption_Coefficient[a][f] * d_mod;
                    }
                }

                for (int f = 0; f < absorption_Coefficient[0].Length; f++)
                {
                    numerator[f] /= denominator;
                }

                return numerator;
            }

            public static double[] Random_Incidence_Paris(double[][] absorption_Coefficient, Complex[][] Zr, double rho_C)
            {
                double dt = Math.PI / (absorption_Coefficient.Length);
                double[] numerator = new double[absorption_Coefficient[0].Length];
                //double denominator = 0;
                for (int a = 0; a < absorption_Coefficient.Length; a++)
                {
                    double theta = a * dt - (Math.PI / 2);
                    double d_mod = Math.Abs(Math.Sin(theta) * dt);
                    //denominator += d_mod;
                    for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                    {
                        numerator[f] += absorption_Coefficient[a][f] * d_mod / (Zr[f][a].Real / rho_C);
                    }
                }

                //for (int f = 0; f < absorption_Coefficient[0].Length; f++)
                //{
                //    numerator[f] /= denominator;
                //}

                return numerator;
            }

            public static double[] Random_Incidence_0_78(double[][] absorption_Coefficient)
            {
                double dt = 180 / (absorption_Coefficient.Length - 1);
                double[] numerator = new double[absorption_Coefficient[0].Length];
                double denominator = 0;
                double mod78 = 78 % dt;
                for (int a = 0; a < absorption_Coefficient.Length; a++)
                {
                    double theta = Math.Abs(a * dt - 90);
                    if (theta > 78)
                    {
                        if (theta < 80)
                        {
                            denominator += mod78;
                            for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                            {
                                numerator[f] += absorption_Coefficient[a][f] * mod78;
                            }
                        }
                        else continue;
                    }
                    else
                    {
                        denominator += dt;
                        for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                        {
                            numerator[f] += absorption_Coefficient[a][f] * dt;
                        }
                    }
                }

                for (int f = 0; f < absorption_Coefficient[0].Length; f++)
                {
                    numerator[f] /= denominator;
                }

                return numerator;
            }

            public static double[] Random_Incidence_NoWeights(double[][] absorption_Coefficient)
            {
                //double dt = Math.PI * (absorption_Coefficient.Length - 1) / 180;
                double[] numerator = new double[absorption_Coefficient[0].Length];
                double denominator = 0;
                for (int a = 0; a < absorption_Coefficient.Length; a++)
                {
                    //double theta = a * dt;
                    denominator++;
                    for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                    {
                        numerator[f] += absorption_Coefficient[a][f];
                    }
                }

                for (int f = 0; f < absorption_Coefficient[0].Length; f++)
                {
                    numerator[f] /= denominator;
                }

                return numerator;
            }

            public static double[] Random_Incidence_0_78(double[][] absorption_Coefficient, Complex[][] Zr, double Rho_C)
            {
                double dt = 180 / (absorption_Coefficient.Length - 1);
                double[] numerator = new double[absorption_Coefficient[0].Length];
                double denominator = 0;
                double mod78 = 78 % dt;
                for (int a = 0; a < absorption_Coefficient.Length; a++)
                {
                    double theta = Math.Abs(a * dt - 90);
                    if (theta > 78)
                    {
                        if (theta < 80)
                        {
                            denominator += mod78;
                            for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                            {
                                numerator[f] += absorption_Coefficient[a][f] * mod78 / (Zr[f][a].Real / Rho_C);
                            }
                        }
                        else continue;
                    }
                    else
                    {
                        denominator += dt;
                        for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                        {
                            numerator[f] += absorption_Coefficient[a][f] * dt;
                        }
                    }
                }

                for (int f = 0; f < absorption_Coefficient[0].Length; f++)
                {
                    numerator[f] /= denominator;
                }

                return numerator;
            }

            public static double[] Random_Incidence_NoWeights(double[][] absorption_Coefficient, Complex[][] Zr, double Rho_C)
            {
                //double dt = Math.PI * (absorption_Coefficient.Length - 1) / 180;
                double[] numerator = new double[absorption_Coefficient[0].Length];
                double denominator = 0;
                for (int a = 0; a < absorption_Coefficient.Length; a++)
                {
                    //double theta = a * dt;
                    denominator++;
                    for (int f = 0; f < absorption_Coefficient[a].Length; f++)
                    {
                        numerator[f] += absorption_Coefficient[a][f] / (Zr[f][a].Real / Rho_C);
                    }
                }

                for (int f = 0; f < absorption_Coefficient[0].Length; f++)
                {
                    numerator[f] /= denominator;
                }

                return numerator;
            }

            public static double[][] Finite_Unit_Absorption_Coefficient(Complex[][] Zr, Complex[][] Z, double[] Angles, double rho, double c_sound)
            {
                double[][] Alpha = new double[Z.Length][];

                for (int j = 0; j < Angles.Length; j++)
                {
                    //double theta = (Angles[j]) * Utilities.Numerics.Pi_180;
                    Alpha[j] = new double[Z[j].Length];
                    for (int i = 0; i < Z[0].Length; i++)
                    {
                        if (double.IsNaN(Z[j][i].Real) || double.IsNaN(Z[j][i].Imaginary)) continue;

                        Complex Za = Z[j][i] / (rho * c_sound);
                        Complex Zrn = Zr[i][j] / (rho * c_sound);
                        double a_denom = (Za + Zrn).Magnitude;// *Math.Cos(theta);
                        Alpha[j][i] = 4 * Za.Real * Zrn.Real / (a_denom * a_denom);
                    }
                }

                return Alpha;
            }

            public static double Finite_Unit_Absorption_Coefficient(Complex Z, Complex Zr, double rho, double c_sound)
            {
                if (double.IsNaN(Z.Real) || double.IsNaN(Z.Imaginary)) return 0;

                Complex Za = Z / (rho * c_sound);
                Complex Zrn = Zr / (rho * c_sound);
                double a_denom = (Za + Zrn).Magnitude;// *Math.Cos(theta);
                return 4 * Za.Real * Zrn.Real / (a_denom * a_denom);
            }

            public static double Finite_Absorption_Coefficient(Complex Z, Complex Zr, double Angles, double rho, double c_sound)
            {
                if (double.IsNaN(Z.Real) || double.IsNaN(Z.Imaginary)) return 0;
                Complex Za = Z / (rho * c_sound);
                Complex Zrn = Zr / (rho * c_sound);
                double a_denom = (Za + Zrn).Magnitude;
                double theta = Angles * Utilities.Numerics.Pi_180;
                return 8 * Za.Real * Math.Sin(theta) / (a_denom * a_denom);
            }

            public static double[][] Finite_Absorption_Coefficient(Complex[][] Zr, Complex[][] Z, double[] Angles, double rho, double c_sound)
            {
                double[][] Alpha = new double[Z.Length][];

                for (int j = 0; j < Angles.Length; j++)
                {
                    double theta = Angles[j] * Utilities.Numerics.Pi_180;
                    Alpha[j] = new double[Z[j].Length];
                    for (int i = 0; i < Z[0].Length; i++)
                    {
                        if (double.IsNaN(Z[j][i].Real) || double.IsNaN(Z[j][i].Imaginary)) continue;

                        //Complex Za = Z[j][i];
                        //Complex Zrn = Zr[i][j];
                        Complex Za = Z[j][i] / (rho * c_sound);
                        Complex Zrn = Zr[i][j] / (rho * c_sound);
                        double a_denom = (Za + Zrn).Magnitude;// *Math.Cos(theta);
                        Alpha[j][i] = 8 * Za.Real * Math.Sin(theta) / (a_denom * a_denom); // * Math.Abs(Math.Sin(theta))
                                                                                           //Alpha[j][i] = 4 * Za.Real / (a_denom * a_denom);                        
                                                                                           //Alpha[j][i] = Absorption_Coef(Reflection_Coef(Za + Zrn, rho * c_sound));
                                                                                           //Alpha[j][i] = Absorption_Coef(Reflection_Coef(Z[j][i], rho * c_sound) * Reflection_Coef(Zr[i][j], rho * c_sound));
                    }
                }

                return Alpha;
            }

            public static double Decoupling_Frequency(double porosity, double Flow_resistivity, double Frame_Inherent_Density)
            {
                return Flow_resistivity * porosity * porosity / (Utilities.Numerics.PiX2 * Frame_Inherent_Density);
            }

            public static Complex[] Finite_Radiation_Impedance_Rect(Rhino.Geometry.Mesh M, double step, double freq, double angles, double C_Sound)
            {
                Complex[] Zr = new Complex[M.Vertices.Count];
                //double Area_step = Xdim * Ydim;
                double phistep = Utilities.Numerics.PiX2 / 36;

                int ct = 0;
                Complex integral_Mod = new Complex(0, 1.2 * Utilities.Numerics.PiX2 * freq / (M.Vertices.Count - 1));
                double kt = Utilities.Numerics.PiX2 * freq * Math.Cos(angles * Math.PI / 180);
                for (double phi = 0; phi < Utilities.Numerics.PiX2; phi += phistep)
                {
                    //for (double x = 0; x < Xdim; x += step) for (double y = 0; y < Ydim; y += step)
                    for (int i = 0; i < M.Vertices.Count; i++)
                    {
                        foreach (Rhino.Geometry.Point3f p in M.Vertices)
                        {
                            //for (double x0 = 0; x0 < Xdim; x0 += step) for (double y0 = 0; y0 < Ydim; y0 += step)
                            //    {
                            //if () continue;
                            double R = (M.Vertices[i] - p).Length;
                            if (R < 0.0001) continue;
                            Complex GMM = Complex.Exp(new Complex(0, -Utilities.Numerics.PiX2 * freq * R / C_Sound)) / (Utilities.Numerics.PiX2 * R);
                            Complex PiM0 = Complex.Exp(new Complex(0, -kt * (Math.Cos(phi) * M.Vertices[i].X + Math.Sin(phi) * M.Vertices[i].Y)));
                            Complex PiM = Complex.Exp(new Complex(0, kt * (Math.Cos(phi) * p.X + Math.Sin(phi) * p.Y)));
                            Zr[i] += PiM0 * GMM * PiM;
                            ct++;
                            ///Add to area integral
                            //}
                        }
                        Zr[i] *= integral_Mod;
                    }
                    ///this is now Zr for a given angle theta.
                }
                return Zr;
            }

            public static double[][] Absorption_Coefficient(Complex[][] Z, double[] freq)
            {
                double dt = Math.PI / (Z.Length - 1);
                double[][] alpha = new double[Z.Length][];

                //Complex[] Zc = Equivalent_Fluids.DB_Miki_Impedance(1.2, 343, 25000, freq);

                for (int a = 0; a < Z.Length; a++)
                {
                    double theta = a * dt - (Math.PI / 2);
                    alpha[a] = new double[Z[a].Length];
                    for (int f = 0; f < Z[0].Length; f++)
                    {
                        //Complex Za = (1 / Z[a][f]);
                        //Complex Za = Z[a][f] / Zc[f];
                        //Za /= Za.Magnitude();
                        Complex Za = Z[a][f] / Z[a][f].Magnitude;
                        //Za = 1 / Za;
                        double denom_rt2 = (Za * Math.Cos(theta) + 1).Magnitude;
                        alpha[a][f] = (4 * Za.Real) * Math.Cos(theta) / (denom_rt2 * denom_rt2);
                    }
                }
                return alpha;
            }

            public static Complex[][] Finite_Radiation_Impedance_Rect_Longhand(double x, double y, Rhino.Geometry.Brep S, double freq, double[] altitude, double[] azimuth, double C_Sound)
            {
                Complex[][] Zr = new Complex[altitude.Length][];
                double lambda = C_Sound / freq;
                double step = lambda / 30;
                double A = Rhino.Geometry.AreaMassProperties.Compute(S).Area;
                Rhino.Geometry.MeshingParameters MP = new Rhino.Geometry.MeshingParameters();
                MP.MaximumEdgeLength = step;
                Rhino.Geometry.Mesh M = Rhino.Geometry.Mesh.CreateFromBrep(S, MP)[0];

                Complex integral_Mod = Complex.ImaginaryOne * Utilities.Numerics.PiX2 * freq * 1.2 / A;
                List<double> Areas = new List<double>();
                List<Rhino.Geometry.Point3d> pts = new List<Rhino.Geometry.Point3d>();
                for (int f = 0; f < M.Faces.Count; f++)
                {
                    if (M.Faces[f].IsQuad)
                    {
                        Hare.Geometry.Point[][] verts = new Hare.Geometry.Point[1][];
                        verts[0] = new Hare.Geometry.Point[]{
                            new Hare.Geometry.Point(M.Vertices[M.Faces[f].A].X, M.Vertices[M.Faces[f].A].Y, M.Vertices[M.Faces[f].A].Z),
                            new Hare.Geometry.Point(M.Vertices[M.Faces[f].B].X, M.Vertices[M.Faces[f].B].Y, M.Vertices[M.Faces[f].B].Z),
                            new Hare.Geometry.Point(M.Vertices[M.Faces[f].C].X, M.Vertices[M.Faces[f].C].Y, M.Vertices[M.Faces[f].C].Z),
                            new Hare.Geometry.Point(M.Vertices[M.Faces[f].D].X, M.Vertices[M.Faces[f].D].Y, M.Vertices[M.Faces[f].D].Z)};
                        Areas.Add(new Hare.Geometry.Topology(verts).Polygon_Area(0));
                    }
                    else
                    {
                        Hare.Geometry.Point[][] verts = new Hare.Geometry.Point[1][];
                        verts[0] = new Hare.Geometry.Point[]{
                            new Hare.Geometry.Point(M.Vertices[M.Faces[f].A].X, M.Vertices[M.Faces[f].A].Y, M.Vertices[M.Faces[f].A].Z),
                            new Hare.Geometry.Point(M.Vertices[M.Faces[f].B].X, M.Vertices[M.Faces[f].B].Y, M.Vertices[M.Faces[f].B].Z),
                            new Hare.Geometry.Point(M.Vertices[M.Faces[f].C].X, M.Vertices[M.Faces[f].C].Y, M.Vertices[M.Faces[f].C].Z)};
                        Areas.Add(new Hare.Geometry.Topology(verts).Polygon_Area(0));
                    }

                    pts.Add(M.Faces.GetFaceCenter(f));
                }

                Parallel.For(0, altitude.Length, j =>
                {
                    double k = Utilities.Numerics.PiX2 / C_Sound;
                    double kt = k * Math.Sin(Math.Abs(altitude[j]) * Math.PI / 180);
                    Zr[j] = new Complex[azimuth.Length];
                    for (int phi = 0; phi < azimuth.Length; phi++) //Parallel.For(0, 16, phi =>
                        {
                        double cosphi = Math.Cos(azimuth[phi] * Math.PI / 180), sinphi = Math.Sin(azimuth[phi] * Math.PI / 180);
                        for (int i = 0; i < pts.Count; i++)
                        {
                            double xd = pts[i].X - x, yd = pts[i].Y - y;
                            double R = Math.Sqrt(xd * xd + yd * yd);
                            Complex GMM = Complex.Exp(-Complex.ImaginaryOne * k * freq * R) / (Utilities.Numerics.PiX2 * R);
                            Complex PiMPiM0 = Complex.Exp(Complex.ImaginaryOne * kt * freq * (cosphi * xd + sinphi * yd));
                            Zr[j][phi] += GMM * PiMPiM0 * Areas[i];
                        }
                            ///By distributive property, all multipliers moved to outer loop.
                            Zr[j][phi] *= integral_Mod;
                            //Zr[j][35 - phi] = Zr[j][phi];
                            ///this is now Zr for a given angle theta.
                        }
                });

                return Zr;
            }

            public static Complex[][] Finite_Radiation_Impedance_Rect_Longhand(double x, double y, double Xdim, double Ydim, double freq, double[] altitude, double[] azimuth, double C_Sound)
            {
                x -= Xdim / 2;
                y -= Ydim / 2;
                Complex[][] Zr = new Complex[altitude.Length][];
                double mindim = Math.Min(Xdim, Ydim);

                double lambda = C_Sound / freq;
                double step = lambda / 30;
                Complex integral_Mod = Complex.ImaginaryOne * Utilities.Numerics.PiX2 * freq * 1.2 * step * step / (Xdim * Ydim);// / Area_step);

                Parallel.For(0, altitude.Length, j =>
                {
                    double k = Utilities.Numerics.PiX2 / C_Sound;
                    double kt = k * Math.Sin(Math.Abs(altitude[j]) * Math.PI / 180);
                    Zr[j] = new Complex[azimuth.Length];
                    for (int phi = 0; phi < azimuth.Length; phi++) //Parallel.For(0, 16, phi =>
                        {
                        double cosphi = Math.Cos(azimuth[phi] * Math.PI / 180), sinphi = Math.Sin(azimuth[phi] * Math.PI / 180);
                        for (double x0 = -Xdim / 2; x0 < Xdim / 2; x0 += step)
                            for (double y0 = -Ydim / 2; y0 < Ydim / 2; y0 += step)
                            {
                                if (x == x0 && y == y0) continue;
                                double xd = x0 - x, yd = y0 - y;
                                double R = Math.Sqrt(xd * xd + yd * yd);
                                Complex GMM = Complex.Exp(-Complex.ImaginaryOne * k * freq * R) / (Utilities.Numerics.PiX2 * R);
                                Complex PiMPiM0 = Complex.Exp(Complex.ImaginaryOne * kt * freq * (cosphi * xd + sinphi * yd));
                                Zr[j][phi] += GMM * PiMPiM0;
                            }
                            ///By distributive property, all multipliers moved to outer loop.
                            Zr[j][phi] *= integral_Mod;
                            //Zr[j][35 - phi] = Zr[j][phi];
                            ///this is now Zr for a given angle theta.
                        }
                });

                return Zr;
            }

            public static Complex[][] Finite_Radiation_Impedance_Rect_Longhand(double Xdim, double Ydim, double[] freq, double[] angles, double C_Sound)
            {
                Complex[][] Zr = new Complex[freq.Length][];
                double phistep = Utilities.Numerics.PiX2 / 16;
                double mindim = Math.Min(Xdim, Ydim);

                double lambda = C_Sound / freq[freq.Length - 1];
                double step = lambda / 20;
                Complex integral_Mod = Complex.ImaginaryOne * 1.2 * Utilities.Numerics.PiX2 * step * step * 0.0625 / (Xdim * Ydim);// / Area_step);
                for (int i = 0; i < freq.Length; i++)
                {
                    Zr[i] = new Complex[angles.Length];
                    for (int j = 0; j < angles.Length; j++) Zr[i][j] = new Complex();
                }
                int ct = 0;
                //for (int j = 0; j < angles.Length/2; j++)
                Parallel.For(0, (int)Math.Ceiling((double)angles.Length / 2), j =>
                {
                    ct++;
                        //Rhino.RhinoApp.CommandPrompt = string.Format("Radiation Impedance {0}% complete", Math.Round((double)ct * 200 / (double)angles.Length-1));
                        //Progress = (double)j / (double)angles.Length;
                        double k = Utilities.Numerics.PiX2 / C_Sound;
                    double kt = k * Math.Sin(Math.Abs(angles[j]) * Math.PI / 180);
                        //Parallel.For(0, (int)(Xdim / step), x =>
                        //{
                        for (double x = step / 2; x < Xdim; x += step)
                        for (double y = step / 2; y < Ydim; y += step)
                        {
                            for (double phi = 0; phi < Utilities.Numerics.PiX2; phi += phistep) //Parallel.For(0, 16, phi =>
                                {
                                double cosphi = Math.Cos(phi), sinphi = Math.Sin(phi);

                                for (double x0 = 0; x0 < Xdim; x0 += step)
                                    for (double y0 = 0; y0 < Ydim; y0 += step)
                                    {
                                        if (x == x0 && y == y0) continue;
                                        double xd = x0 - x, yd = y0 - y;
                                        double R = Math.Sqrt(xd * xd + yd * yd);
                                        Complex[] GMM = new Complex[freq.Length];
                                        Complex[] PiMPiM0 = new Complex[freq.Length];
                                        for (int i = 0; i < freq.Length; i++)
                                        {
                                            GMM[i] = Complex.Exp(-Complex.ImaginaryOne * k * freq[i] * R) / (Utilities.Numerics.PiX2 * R);
                                            PiMPiM0[i] = Complex.Exp(Complex.ImaginaryOne * kt * freq[i] * (cosphi * xd + sinphi * yd));
                                        }
                                        for (int i = 0; i < freq.Length; i++)
                                        {
                                            Zr[i][j] += GMM[i] * PiMPiM0[i];
                                        }
                                    }
                            }
                        }
                        //}//);
                        ///multiply by phistep...
                        ///By distributive property, all multipliers moved to outer loop.
                        for (int i = 0; i < freq.Length; i++)
                    {
                        Zr[i][j] *= freq[i] * integral_Mod * step * step;
                        Zr[i][35 - j] = Zr[i][j];
                    }
                        ///this is now Zr for a given angle theta.
                    });

                //////////////////////
                VisualizationBox B = new VisualizationBox(-90, 90, -30, 10);
                double[] rad_eff = new double[angles.Length];
                double[] rad_effI = new double[angles.Length];
                double[] rad_effM = new double[angles.Length];
                B.Show();

                double[] ComparisonCurve = new double[angles.Length];

                for (int j = 0; j < angles.Length; j++)
                {
                    //angles[j] += 90;
                    ComparisonCurve[j] = 10 * Math.Log10(1 / (Math.Cos(angles[j] * Math.PI / 180)));
                }
                for (int i = 0; i < Zr.Length; i++)
                {
                    for (int j = 0; j < angles.Length; j++)
                    {
                        rad_eff[j] = 10 * Math.Log10(Zr[i][j].Real / (1.2 * C_Sound));
                        rad_effI[j] = 10 * Math.Log10(Zr[i][j].Imaginary / (1.2 * C_Sound));
                        rad_effM[j] = 10 * Math.Log10(Zr[i][j].Magnitude / (1.2 * C_Sound));
                    }
                    B.Populate(angles, rad_eff, angles, rad_effI, angles, rad_effM, angles, "Angle of Incidence (theta)", "Radiation Efficiency (dB)", ComparisonCurve, 750, 100);
                }
                //////////////////////

                return Zr;
            }

            public static Complex[][] Finite_Radiation_Impedance_Atalla_Rect(double Xdim, double Ydim, double[] freq, double[] angles, double C_Sound, double Rho0)
            {
                Complex[][] Zr = new Complex[freq.Length][];
                double phistep = Utilities.Numerics.PiX2 / 12;
                double mindim = Math.Min(Xdim, Ydim);
                List<double> cosphi = new List<double>(), sinphi = new List<double>(), sintheta = new List<double>();

                int phi_ct = 0;

                List<double> AnglePhi = new List<double>();

                for (double phi = 0; phi < Utilities.Numerics.PiX2; phi += phistep)
                {
                    cosphi.Add(Math.Cos(phi)); sinphi.Add(Math.Sin(phi)); phi_ct++;
                    AnglePhi.Add(phi);
                }

                for (int i = 0; i < angles.Length; i++)
                {
                    sintheta.Add(Math.Abs(Math.Sin(Math.PI * angles[i] / 180)));
                }

                ///For progress feedback.
                VisualizationBox B = new VisualizationBox(-90, 90, -10, 10);
                double[] ComparisonCurve = new double[angles.Length];
                for (int j = 0; j < angles.Length; j++)
                {
                    ComparisonCurve[j] = 10 * Math.Log10(1 / (Math.Cos(angles[j] * Math.PI / 180)));
                }
                B.Show();

                for (int i = 0; i < freq.Length; i++)
                {
                    Complex Kmod = Complex.ImaginaryOne * Utilities.Numerics.PiX2 * freq[i] * Xdim / (C_Sound * 2);
                    double r = Xdim / Ydim;
                    Zr[i] = new Complex[angles.Length];
                    double lambda = C_Sound / freq[i];
                    double stepx = 1 / (4 * Xdim / (lambda / 10));
                    double stepy = 1 / (4 * Ydim / (lambda / 10));
                    Complex integral_Mod = Complex.ImaginaryOne * Rho0 * Utilities.Numerics.PiX2 * freq[i] * Ydim * stepx * stepy / (phi_ct * 4 * Math.PI);
                    Parallel.For(0, angles.Length, j =>
                    {
                        Complex FnMod = Kmod * sintheta[j];
                        for (double u = -1 + stepx / 2; u < 1; u += stepx)
                            for (double u_ = -1; u_ < 1; u_ += stepy)
                            {
                                Complex[] Zphi = new Complex[cosphi.Count];
                                double[] real = new double[cosphi.Count];
                                double[] imag = new double[cosphi.Count];
                                double u1 = u + 1, u_1 = u_ + 1;
                                double rtR = Math.Sqrt((u1) * (u1) + ((u_1 * u_1) / (r * r)));
                                Complex K = Complex.Exp(-Kmod * rtR) / rtR; //Typographical Error Identified and Fixed.

                                    for (int phi = 0; phi < cosphi.Count; phi++)
                                {
                                    Complex Fn = Complex.Exp(FnMod * (u1 * cosphi[phi] + (u_1 / r) * sinphi[phi]));
                                    Zr[i][j] += K * Fn * (1 - u) * (1 - u_);
                                }
                            }
                        Zr[i][j] *= integral_Mod;
                            /////////////
                            //Let's just try this for a moment:
                            //Zr[i][j] *= Math.Cos((angles[j]) * Math.PI / 180);
                            /////////////
                        });

                    double[] rad_eff = new double[angles.Length];
                    double[] rad_effI = new double[angles.Length];
                    double[] rad_effM = new double[angles.Length];

                    for (int j = 0; j < angles.Length; j++)
                    {
                        rad_eff[j] = 10 * Math.Log10(Zr[i][j].Real / (Rho0 * C_Sound));
                        rad_effI[j] = 10 * Math.Log10(Zr[i][j].Imaginary / (Rho0 * C_Sound));
                        rad_effM[j] = 10 * Math.Log10(Zr[i][j].Magnitude / (Rho0 * C_Sound));
                    }

                    B.Populate(angles, rad_eff, angles, rad_effI, angles, rad_effM, angles, "Angle of Incidence (theta)", "Radiation Efficiency (dB)", ComparisonCurve, 0, (int)(100f * i / freq.Length));
                }

                return Zr;
            }

            //public static Complex Angular_Impedance(double angle, Complex Specific_Impedance, bool radians)
            //{
            //    if (!radians) angle *= 180 / Math.PI;
            //    return Specific_Impedance * Math.Cos(angle);
            //}

            //public static Complex[] Angular_Impedance(double[] angle, Complex Specific_Impedance, bool radians)
            //{
            //    Complex[] Za = new Complex[angle.Length];
            //    if (!radians) for (int i = 0; i < angle.Length; i++) angle[i] *= 180 / Math.PI;
            //    for (int i = 0; i < angle.Length; i++) Za[i] = Specific_Impedance * Math.Cos(angle[i]);
            //    return Za;
            //}

            //public static Complex[][] Angular_Impedance(double[] angle, Complex[] Specific_Impedance, bool radians)
            //{
            //    Complex[][] Za = new Complex[Specific_Impedance.Length][];
            //    if (!radians) for (int i = 0; i < angle.Length; i++) angle[i] *= 180 / Math.PI;
            //    for (int j = 0; j < Specific_Impedance.Length; j++)
            //    {
            //        Za[j] = new Complex[angle.Length];
            //        for (int i = 0; i < angle.Length; i++) Za[j][i] = Specific_Impedance[j] * Math.Cos(angle[i]);
            //    }
            //    return Za;
            //}

            public static Complex Reflection_Coef(Complex Specific_Impedance, double air_density, double c_sound)
            {
                double rho_c = air_density * c_sound;
                return (Specific_Impedance - rho_c) / (Specific_Impedance + rho_c);
            }

            public static Complex Reflection_Coef(Complex Specific_Impedance, double rho_c)
            {
                return (Specific_Impedance - rho_c) / (Specific_Impedance + rho_c);
            }

            public static Complex[] Reflection_Coef(Complex[] Specific_Impedance, double air_density, double c_sound)
            {
                double rho_c = air_density * c_sound;
                Complex[] R = new Complex[Specific_Impedance.Length];
                for (int i = 0; i < R.Length; i++) R[i] = (Specific_Impedance[i] - rho_c) / (Specific_Impedance[i] + rho_c);
                return R;
            }

            public static Complex[][] Reflection_Coef(Complex[][] Specific_Impedance, Complex[][] Radiation_Impedance, double air_density, double c_sound)
            {
                double rho_c = air_density * c_sound;
                Complex[][] R = new Complex[Specific_Impedance.Length][];
                for (int i = 0; i < Specific_Impedance.Length; i++)
                {
                    double theta = (double)(i * Math.PI + (2 * Math.PI / 180)) / (double)Specific_Impedance.Length;
                    R[i] = new Complex[Specific_Impedance[i].Length];
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = (Specific_Impedance[i][j]/rho_c + Radiation_Impedance[j][i]/rho_c) / (2 * Math.Sqrt(Specific_Impedance[i][j].Real/rho_c * Radiation_Impedance[j][i].Real/rho_c));
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = ((Specific_Impedance[i][j] * Math.Cos(theta) + Radiation_Impedance[j][i]) - rho_c) / ((Specific_Impedance[i][j] * Math.Cos(theta) + Radiation_Impedance[j][i]) + rho_c);
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = ((1 - Specific_Impedance[i][j] + Radiation_Impedance[j][i]) - rho_c) / ((1 - Specific_Impedance[i][j] + Radiation_Impedance[j][i]) + rho_c);
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = ((Specific_Impedance[i][j] * Radiation_Impedance[j][i] * rho_c) - rho_c) / ((Specific_Impedance[i][j] * Radiation_Impedance[j][i] * rho_c) + rho_c);
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = 2 / (1 + Radiation_Impedance[j][i]/Specific_Impedance[i][j]);
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = (2 * Specific_Impedance[i][j]) / ((Specific_Impedance[i][j] + Radiation_Impedance[j][i]));
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = 2 * (Specific_Impedance[i][j] - rho_c) / ((Specific_Impedance[i][j] + Radiation_Impedance[j][i]) + rho_c);
                    for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = ((Specific_Impedance[i][j] + Radiation_Impedance[j][i]) - rho_c) / ((Specific_Impedance[i][j] + Radiation_Impedance[j][i]) + rho_c);
                }
                return R;
            }

            public static Complex[][] Reflection_Coef(Complex[][] Specific_Impedance, double air_density, double c_sound)
            {
                double rho_c = air_density * c_sound;
                Complex[][] R = new Complex[Specific_Impedance.Length][];
                for (int i = 0; i < Specific_Impedance.Length; i++)
                {
                    R[i] = new Complex[Specific_Impedance[i].Length];
                    for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = (Specific_Impedance[i][j] - rho_c) / (Specific_Impedance[i][j] + rho_c);
                }
                return R;
            }

            public static Complex[][] Reflection_Coef(Complex[][] Specific_Impedance, Complex[][] Radiation_Impedance)
            {
                Complex[][] R = new Complex[Specific_Impedance.Length][];
                for (int i = 0; i < Specific_Impedance.Length; i++)
                {
                    R[i] = new Complex[Specific_Impedance[i].Length];
                    for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = (Specific_Impedance[i][j] / (Specific_Impedance[i][j] + Radiation_Impedance[j][i])).Magnitude;
                    //for (int j = 0; j < Specific_Impedance[i].Length; j++) R[i][j] = ((Specific_Impedance[i][j] - Radiation_Impedance[j][i]) / (Specific_Impedance[i][j] + Radiation_Impedance[j][i])).Magnitude;
                }
                return R;
            }

            public static Complex[] Reflection_Coef(Complex[] Specific_Impedance, double rho_c)
            {
                Complex[] R = new Complex[Specific_Impedance.Length];
                for (int i = 0; i < R.Length; i++) R[i] = (Specific_Impedance[i] - rho_c) / (Specific_Impedance[i] + rho_c);
                return R;
            }

            public static double Absorption_Coef(Complex Reflection_Coef)
            {
                double mag = Reflection_Coef.Magnitude;
                return 1 - mag * mag;
            }

            public static double[] Absorption_Coef(Complex[] Reflection_Coef)
            {
                double[] alpha = new double[Reflection_Coef.Length];

                for (int i = 0; i < Reflection_Coef.Length; i++)
                {
                    double mag = Reflection_Coef[i].Magnitude;
                    alpha[i] = 1 - mag * mag;
                }

                return alpha;
            }

            public static double[][] Absorption_Coef(Complex[][] Reflection_Coef)
            {
                double[][] alpha = new double[Reflection_Coef.Length][];
                for (int i = 0; i < Reflection_Coef.Length; i++)
                {
                    alpha[i] = new double[Reflection_Coef[i].Length];
                    for (int j = 0; j < Reflection_Coef[i].Length; j++)
                    {
                        double mag = Reflection_Coef[i][j].Magnitude;
                        alpha[i][j] = 1 - mag * mag;
                    }
                }
                return alpha;
            }

            public static Complex Admittance(Complex Impedance)
            {
                return 1 / Impedance;
            }

            public static Complex[] Admittance(Complex[] Impedance)
            {
                Complex[] ad = new Complex[Impedance.Length];
                for (int i = 0; i < Impedance.Length; i++) ad[i] = 1 / Impedance[i];
                return ad;
            }

            public static Complex AirSpace(double depth, double airdensity, double c_sound, int frequency)
            {
                return new Complex(0, airdensity * c_sound / Math.Tan(Utilities.Numerics.PiX2 * frequency * depth / c_sound));
            }

            public static Complex[] Air_Wavenumber(double c_sound, double[] freq)
            {
                Complex[] ka = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    ka[i] = new Complex(Utilities.Numerics.PiX2 * freq[i] / c_sound, 0);
                }

                return ka;
            }

            public static Complex[] Air_CharImpedance(double airdensity, double c_sound, double[] freq)
            {
                Complex[] Za = new Complex[freq.Length];
                double rho_c = airdensity * c_sound;

                for (int i = 0; i < freq.Length; i++)
                {
                    Za[i] = new Complex(rho_c, 0);
                }

                return Za;
            }

            public static Complex[] AirSpace(double depth, double airdensity, double c_sound, double[] freq)
            {
                double rho_c = airdensity * c_sound;
                double DPiX2_c = Utilities.Numerics.PiX2 * depth / c_sound;
                Complex[] As = new Complex[freq.Length];

                //for(int i = 0, f = f_lower; i < As.Length; i++, f++) As[i] = new Complex(0, airdensity * c_sound / Math.Tan(DPiX2_c * f));
                for (int i = 0; i < freq.Length; i++) As[i] = new Complex(0, 1 / (airdensity * c_sound * Math.Tan(DPiX2_c * freq[i])));
                return As;
            }

            //public static Complex[][] SnellsLaw(Complex[] K_start, Complex[] K_trans, Complex[][] Angle_in)
            //{
            //    if (K_start.Length != K_trans.Length) throw new Exception("Wave number arrays must be equal in length");
            //    Complex[][] Angle_Trans = new Complex[Angle_in.Length][];

            //    for (int j = 0; j < Angle_in.Length; j++)
            //    {
            //        Angle_Trans[j] = new Complex[K_start.Length];
            //        for (int i = 0; i < K_start.Length; i++)
            //        {
            //            Angle_Trans[j][i] = Complex.Asin(K_start[i] * Complex.Sin(Angle_in[j][i]) / K_trans[i]);
            //        }
            //    }

            //    return Angle_Trans;
            //}

            //public static Complex[][] SnellsLaw(Complex[] K_start, Complex[] K_trans, Complex[] Angle_in)
            //{
            //    if (K_start.Length != K_trans.Length) throw new Exception("Wave number arrays must be equal in length");
            //    Complex[][] Angle_Trans = new Complex[Angle_in.Length][];

            //    for (int j = 0; j < Angle_in.Length; j++)
            //    {
            //        Angle_Trans[j] = new Complex[K_start.Length];
            //        for (int i = 0; i < K_start.Length; i++)
            //        {
            //            Angle_Trans[j][i] = Complex.Asin(K_start[i] * K_start[i] * Complex.Sin(Angle_in[j]) / (K_trans[i]*K_trans[i]));
            //            //Angle_Trans[j][i] = Complex.Asin(K_start[i] * Complex.Sin(Angle_in[j]) / K_trans[i]);
            //        }
            //    }

            //    return Angle_Trans;
            //}

            //public static Complex[] Transfer(Complex[] Z1, Complex[] Z0, Complex[] k2, Complex[] k1, Complex psi, double depth, double[] freq, bool in_radians)
            //{
            //    if (Z1.Length != Z0.Length || Z1.Length != k1.Length) throw new Exception("Impedance and wave number arrays must be equal in length");
            //    if (!in_radians) psi *= Math.PI / 180;

            //    Complex[] Z_trans = new Complex[Z1.Length];

            //    for (int i = 0; i < freq.Length; i++)
            //    {
            //        Complex kxi = Complex.Sqrt(k1[i] * k1[i] - k2[i] * k2[i] * Complex.Sin(psi));
            //        Complex ki_kxi = k1[i] / kxi;
            //        Complex cotkd = 1 / Complex.Tan(kxi * depth);

            //        Complex Zi = Complex.ImaginaryOne * Z1[i] * ki_kxi * cotkd;
            //        Z_trans[i] = (Z1[i] * Z1[i] * ki_kxi * ki_kxi - Z0[i] * Zi) / (Z0[i] - Zi);
            //    }
            //    return Z_trans;
            //}

            public static SparseMatrix Transfer(SparseMatrix V, SparseMatrix T, SparseMatrix J, SparseMatrix I)//Complex[] Zc, Complex[][] Z, Complex[] k, Complex[][] kxi, double depth)
            {
                if (J != null) T = -J * T;
                V = V * T;
                //if (I != null) V = V * I;
                return V;
            }

            public static Complex[][] Transfer(Complex[] Zc, Complex[][] Z, Complex[] k, Complex[][] kxi, double depth)
            {
                if (Zc.Length != Z[0].Length || Zc.Length != k.Length) throw new Exception("Impedance and wave number arrays must be equal in length");
                //if (!in_radians) psi *= Math.PI / 180;

                Complex[][] Z_trans = new Complex[kxi.Length][];

                for (int j = 0; j < kxi.Length; j++)
                {
                    Z_trans[j] = new Complex[Zc.Length];
                    for (int i = 0; i < kxi[j].Length; i++)
                    {
                        Complex ki_kxi = k[i] / kxi[j][i];
                        Complex cotkd = 1 / Complex.Tan(kxi[j][i] * depth);

                        Complex Zi = Complex.ImaginaryOne * Zc[i] * ki_kxi * cotkd;
                        Z_trans[j][i] = (Zc[i] * Zc[i] * ki_kxi * ki_kxi - Z[j][i] * Zi) / (Z[j][i] - Zi);
                    }
                }
                return Z_trans;
            }

            public static Complex[][] Rigid_Backed(Complex[] Z1, Complex[] k1, Complex[][] kxi, double depth)
            {
                if (Z1.Length != k1.Length) throw new Exception("Impedance and wave number arrays must be equal in length");
                //if (!in_radians) psi *= Math.PI / 180;

                Complex[][] Z_trans = new Complex[kxi.Length][];
                for (int j = 0; j < kxi.Length; j++)
                {
                    Z_trans[j] = new Complex[Z1.Length];

                    for (int i = 0; i < kxi[j].Length; i++)
                    {
                        //Complex kxi = Complex.Sqrt(k1[i] * k1[i] - k2[i] * k2[i] * Complex.Sin(psi));
                        Z_trans[j][i] = -Complex.ImaginaryOne * Z1[i] * k1[i] / (Complex.Tan(kxi[j][i] * depth) * kxi[j][i]);
                    }
                }
                return Z_trans;
            }

            public static Complex[][] WaveNumber_NormalComponent(Complex[] K, Complex[] Angles)
            {
                Complex[][] Kx = new Complex[Angles.Length][];

                for (int i = 0; i < Angles.Length; i++)
                {
                    Kx[i] = new Complex[K.Length];
                    for (int k = 0; k < K.Length; k++)
                    {
                        Kx[i][k] = K[k] * Complex.Sqrt(1 - Complex.Sin(Angles[i]));
                    }
                }
                return Kx;
            }

            public static Complex[][] Transfer_Matrix_Explicit_Z(bool freq_log, int sample_Freq, double c_sound, List<ABS_Layer> LayerList, ref double[] frequency, ref Complex[] anglesdeg)
            {
                Complex[][] R, Tau;
                return Transfer_Matrix_Explicit(false, freq_log, sample_Freq, c_sound, LayerList, ref frequency, ref anglesdeg, out R, out Tau);
            }

            public static Complex[][] Transfer_Matrix_Explicit_Tau(bool freq_log, int sample_Freq, double c_sound, List<ABS_Layer> LayerList, ref double[] frequency, ref Complex[] anglesdeg, ref Complex[][] Trans, ref Complex[][] R)
            {
                return Transfer_Matrix_Explicit(true, freq_log, sample_Freq, c_sound, LayerList, ref frequency, ref anglesdeg, out Trans, out R);
            }

            public static Complex[][] Transfer_Matrix_Explicit(bool Transmission, bool freq_log, int sample_Freq, double c_sound, List<ABS_Layer> LayerList, ref double[] frequency, ref Complex[] anglesdeg, out Complex[][] Trans, out Complex[][] R)
            {
                if (freq_log)
                {
                    //3rd octave band frequencies...
                    List<double> freq = new List<double>();
                    double f = 15.625;

                    int ct = 1;
                    while (f < sample_Freq / 2)
                    {
                        ct++;
                        f = 15.625 * Math.Pow(2, (double)ct / 3f);
                        freq.Add(f);
                    }
                    frequency = freq.ToArray();
                }
                else
                {
                    //Linear frequency scale
                    frequency = new double[4096];
                    double step = ((double)sample_Freq) / 4096d;
                    for (int i = 0; i < frequency.Length; i++) frequency[i] = ((double)i + .5) * step;
                }

                anglesdeg = new Complex[(int)(180 / 5)];

                double[] Angle = new double[anglesdeg.Length];
                Complex[][] sintheta_inc = new Complex[36][];
                Complex[][][] Angle_Inc = new Complex[LayerList.Count][][];
                Complex[] K_Air = AbsorptionModels.Operations.Air_Wavenumber(c_sound, frequency);
                Complex[] Zc_Air = AbsorptionModels.Operations.Air_CharImpedance(1.2, c_sound, frequency);
                Complex[][] kxi = new Complex[anglesdeg.Length][];

                for (double a = 2.5, i = 0; a <= 180; a += 5, i++)
                {
                    sintheta_inc[(int)i] = new Complex[frequency.Length];
                    anglesdeg[(int)i] = 90 - a;
                    if (a <= 90) Angle[(int)i] = (90 - a) * Utilities.Numerics.Pi_180;
                    if (a > 90) Angle[(int)i] = (a - 90) * Utilities.Numerics.Pi_180;
                    sintheta_inc[(int)i][0] = Complex.Sin(Angle[(int)i]);//a <= 90 ? Angle[(int)i] : Angle[36 - (int)i]);
                    kxi[(int)i] = new Complex[frequency.Length];
                    for (int j = 1; j < sintheta_inc[(int)i].Length; j++)
                    {
                        sintheta_inc[(int)i][j] = sintheta_inc[(int)i][0];
                        kxi[(int)i][j] = sintheta_inc[(int)i][j] * K_Air[j];
                    }
                }

                int N = 1;

                int tc = 0;
                Complex[][] kzi = new Complex[anglesdeg.Length][];
                List<SparseMatrix[][]> T = new List<SparseMatrix[][]>();
                Complex[] K0 = K_Air;

                for (int i = LayerList.Count - 1; i >= 0; i--)
                {
                    ABS_Layer Layer_i = (LayerList[i] as ABS_Layer);
                    SparseMatrix[][] tn = new SparseMatrix[anglesdeg.Length][];
                    //for (int a = 0; a < anglesdeg.Length; a++)
                    double[] fr = frequency.Clone() as double[];
                    Parallel.For(0, anglesdeg.Length, a =>
                    {
                        kzi[a] = new Complex[4096];
                        tn[a] = new SparseMatrix[4096];

                        switch (Layer_i.T)
                        {
                            case ABS_Layer.LayerType.AirSpace:
                                Complex[] Zc0 = Operations.Air_CharImpedance(1.2, c_sound, fr);
                                for (int f = 0; f < 4096; f++)
                                {
                                    Complex Ksintheta = kxi[a][f] / K_Air[f];
                                    kzi[a][f] = Complex.Sqrt(K_Air[f] * K_Air[f] * (1 - Ksintheta * Ksintheta));
                                    tn[a][f] = Explicit_TMM.FluidMatrix(Layer_i.depth, kzi[a][f], K_Air[f], Zc0[f]);
                                }
                                K0 = K_Air;
                                break;
                            case ABS_Layer.LayerType.BiotPorousAbsorber_Limp:
                                Complex[] kbl = Biot_Porous_Absorbers.WaveNumber_Fluid(Layer_i.density, Layer_i.porosity, Layer_i.Thermal_Permeability, fr);
                                for (int f = 0; f < 4096; f++)
                                {
                                    tn[a][f] = Explicit_TMM.PorousMatrix(false, Layer_i.depth, kbl[f], kxi[a][f] / K_Air[f], fr[f], Layer_i.porosity, Layer_i.tortuosity, Layer_i.YoungsModulus, Layer_i.PoissonsRatio, Layer_i.Viscous_Characteristic_Length, Layer_i.Flow_Resist, Layer_i.density, 101325);
                                }
                                K0 = kbl;
                                break;
                            case ABS_Layer.LayerType.BiotPorousAbsorber_Rigid:
                                Complex[] kbr = Biot_Porous_Absorbers.WaveNumber_Fluid(Layer_i.density, Layer_i.porosity, Layer_i.Thermal_Permeability, fr);
                                for (int f = 0; f < 4096; f++)
                                {
                                    tn[a][f] = Explicit_TMM.PorousMatrix(true, Layer_i.depth, kbr[f], kxi[a][f] / K_Air[f], fr[f], Layer_i.porosity, Layer_i.tortuosity, Layer_i.YoungsModulus, Layer_i.PoissonsRatio, Layer_i.Viscous_Characteristic_Length, Layer_i.Flow_Resist, Layer_i.density, 101325);
                                }
                                K0 = kbr;
                                break;
                            case ABS_Layer.LayerType.PorousDB:
                                Complex[] Kdb = Equivalent_Fluids.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, fr);
                                Complex[] Zcdb = Equivalent_Fluids.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, fr);
                                for (int f = 0; f < 4096; f++)
                                {
                                    Complex Ksintheta = kxi[a][f] / Kdb[f];
                                    kzi[a][f] = Complex.Sqrt(Kdb[f] * Kdb[f] * (1 - Ksintheta * Ksintheta));
                                    tn[a][f] = Explicit_TMM.FluidMatrix(Layer_i.depth, kzi[a][f], Kdb[f], Zcdb[f]);
                                }
                                K0 = Kdb;
                                break;
                            case ABS_Layer.LayerType.PorousCA:
                                Complex[] Kca = Equivalent_Fluids.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, fr);
                                Complex[] Zcca = Equivalent_Fluids.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, fr);
                                for (int f = 0; f < 4096; f++)
                                {
                                    Complex Ksintheta = kxi[a][f] / Kca[f];
                                    kzi[a][f] = Complex.Sqrt(Kca[f] * Kca[f] * (1 - Ksintheta * Ksintheta));
                                    tn[a][f] = Explicit_TMM.FluidMatrix(Layer_i.depth, kzi[a][f], Kca[f], Zcca[f]);
                                }
                                K0 = Kca;
                                break;
                            case ABS_Layer.LayerType.PorousM:
                                Complex[] Km = Equivalent_Fluids.DB_Miki_WNumber(1.2, c_sound, Layer_i.Flow_Resist, fr);
                                Complex[] Zcm = Equivalent_Fluids.DB_Miki_Impedance(1.2, c_sound, Layer_i.Flow_Resist, fr);
                                for (int f = 0; f < 4096; f++)
                                {
                                    Complex Ksintheta = kxi[a][f] / Km[f];
                                    kzi[a][f] = Complex.Sqrt(Km[f] * Km[f] * (1 - Ksintheta * Ksintheta));
                                    tn[a][f] = Explicit_TMM.FluidMatrix(Layer_i.depth, kzi[a][f], Km[f], Zcm[f]);
                                }
                                K0 = Km;
                                break;
                            case ABS_Layer.LayerType.SolidPlate:
                                Complex LameL = Solids.Lame_Lambda(Layer_i.YoungsModulus, Layer_i.PoissonsRatio);
                                Complex LameMu = Solids.Lame_Mu(Layer_i.YoungsModulus, Layer_i.PoissonsRatio);
                                Complex[] Ks = Solids.WaveNumber(fr, Layer_i.density, LameL, LameMu);
                                for (int f = 0; f < 4096; f++)
                                {
                                    //Complex Ksintheta = kxi[a][f] / Ks[
                                    //kzi[a][f] = Complex.Sqrt(Ks[f] * Ks[f] * (1 - Ksintheta * Ksintheta));
                                    //tn[a][f] =  Explicit_TMM.Solid_Matrix(Ks[f], K0[f], kxi[a][f], Layer_i.depth, frequency[f], Layer_i.density, LameMu, LameL);
                                    tn[a][f] = Explicit_TMM.Solid_Matrix(Ks[f] * kxi[a][f] / K_Air[f], Layer_i.depth, fr[f], Layer_i.density, Layer_i.YoungsModulus, Layer_i.PoissonsRatio);
                                }
                                K0 = Ks;
                                break;
                            //case ABS_Layer.LayerType.Perforated_Modal:

                            //case ABS_Layer.LayerType.Slotted_Modal:

                            //case ABS_Layer.LayerType.CircularPerforations:

                            //case ABS_Layer.LayerType.SquarePerforations:

                            //case ABS_Layer.LayerType.Slots:

                            //case ABS_Layer.LayerType.MicroPerforated:

                            //case ABS_Layer.LayerType.Microslit:

                            default:
                                throw new Exception("Unknown Layer Type");
                        }
                    });

                    if (T.Count == 0)
                    {
                        T.Add(tn);
                    }
                    else if (T[tc][0][0].RowCount == tn[0][0].RowCount)
                    {
                        if (tn[0][0].RowCount == 6)
                        {
                            for (int a = 0; a < sintheta_inc.Length; a++)
                                for (int f = 0; f < 4096; f++)
                                    T[tc][a][f] *= Explicit_TMM.InterfacePP(LayerList[i].porosity, LayerList[i+1].porosity) * tn[a][f];
                        }
                        else
                        {
                            for (int a = 0; a < sintheta_inc.Length; a++)
                                for (int f = 0; f < 4096; f++)
                                    T[tc][a][f] *= tn[a][f];
                        }
                    }
                    else
                    {
                        tc++;
                        T.Add(tn);
                    }
                }

                SparseMatrix[] I = new SparseMatrix[LayerList.Count];
                SparseMatrix[] J = new SparseMatrix[LayerList.Count];
                
                for (int i = 0; i < T.Count; i++)
                {
                    if (i == 0)
                    {
                        if (T[i][0][0].RowCount == 2)
                        {
                            //Fluid Layer
                            I[i] = MathNet.Numerics.LinearAlgebra.Complex.SparseMatrix.CreateIdentity(2);
                            J[i] = MathNet.Numerics.LinearAlgebra.Complex.SparseMatrix.CreateIdentity(2);
                        }
                        else if (T[i][0][0].RowCount == 6)
                        {
                            //Biot Porous absorbers
                            I[i] = Explicit_TMM.Interfacepf_Fluid(LayerList[i].porosity);
                            J[i] = Explicit_TMM.Interfacepf_Porous(LayerList[i].porosity);
                        }
                        else
                        {
                            //Solid Layer
                            I[i] = Explicit_TMM.InterfaceSF_Fluid();
                            J[i] = Explicit_TMM.InterfaceSF_Solid();
                        }
                    }
                    else if (T[i][0][0].RowCount == 2)
                    {
                        //Fluid Layer
                        if (T[i-1][0][0].RowCount == 2)
                        {
                            //Fluid Layer
                            throw new Exception("Fluid-fluid - should multiply them together instead.");
                            //I[i] = MathNet.Numerics.LinearAlgebra.Complex.SparseMatrix.CreateIdentity(2);
                            //J[i] = MathNet.Numerics.LinearAlgebra.Complex.SparseMatrix.CreateIdentity(2);
                        }
                        else if (T[i-1][0][0].RowCount == 6)
                        {
                            //Biot Porous absorbers
                            I[i] = Explicit_TMM.Interfacepf_Porous(LayerList[LayerList.Count - i].porosity) as SparseMatrix;
                            J[i] = Explicit_TMM.Interfacepf_Fluid(LayerList[LayerList.Count - i].porosity);
                        }
                        else
                        {
                            //Solid Layer55
                            I[i] = Explicit_TMM.InterfaceSF_Solid();
                            J[i] = Explicit_TMM.InterfaceSF_Fluid();
                        }
                    }
                    else if (T[i][0][0].RowCount == 6)
                    {
                        //Biot Porous absorbers
                        if (T[i-1][0][0].RowCount == 2)
                        {
                            //Fluid Layer
                            I[i] = Explicit_TMM.Interfacepf_Fluid(LayerList[i].porosity);
                            J[i] = Explicit_TMM.Interfacepf_Porous(LayerList[i].porosity);
                        }
                        else if (T[i-1][0][0].RowCount == 6)
                        {
                            //Biot Porous absorbers
                            throw new Exception("Biot - Biot - should multiply them together instead with interface PP");
                        }
                        else
                        {
                            //Solid Layer
                            I[i] = Explicit_TMM.Interfacesp_Solid();
                            J[i] = Explicit_TMM.Interfacesp_Porous();
                        }
                    }
                    else
                    {
                        //Solid Layer
                        if (T[i-1][0][0].RowCount == 2)
                        {
                            //Fluid Layer
                            I[i] = Explicit_TMM.InterfaceSF_Fluid();
                            J[i] = Explicit_TMM.InterfaceSF_Solid();
                        }
                        else if (T[i-1][0][0].RowCount == 6)
                        {
                            //Biot Porous absorbers
                            I[i] = Explicit_TMM.Interfacesp_Porous();
                            J[i] = Explicit_TMM.Interfacesp_Solid();
                        }
                        else
                        {
                            //Solid Layer
                            throw new Exception("Solid-Solid - should multiply them together instead.");
                        }
                    }
                    N += J[i].RowCount;
                }

                Complex[][] Z = new Complex[sintheta_inc.Length][];
                Complex[][] Reflection_Coef = new Complex[sintheta_inc.Length][];
                Complex[][] Trans_Loss = new Complex[sintheta_inc.Length][];
                
                if (!Transmission)
                {
                    SparseMatrix Y;
                    switch (T.Last()[0][0].RowCount)
                    {
                        case 2:
                            N += 1;
                            Y = Explicit_TMM.RigidTerminationF();
                            break;
                        case 4:
                            N += 2;
                            Y = Explicit_TMM.RigidTerminationS();
                            break;
                        case 6:
                            N += 3;
                            Y = Explicit_TMM.RigidTerminationP();
                            break;
                        default:
                            throw new Exception("How can you have a matrix that is not 2, 4, or 6 rows?");
                    }

                    Parallel.For(0, anglesdeg.Length, a =>
                    //for (int a = 0; a < sintheta_inc.Length; a++)
                    {
                        Z[a] = new Complex[sintheta_inc[a].Length];

                        for (int f = 0; f < sintheta_inc[a].Length; f++)
                        {
                            SparseMatrix GT = new SparseMatrix(N - 1, N);
                            int r = 0, c = 0;

                            for (int i = 0; i < T.Count; i++)
                            {
                                GT.SetSubMatrix(r, c, I[i]);
                                c += I[i].ColumnCount;
                                SparseMatrix subJT = J[i] * T[i][a][f];
                                GT.SetSubMatrix(r, c, subJT);
                                r += subJT.RowCount;
                            }
                            GT.SetSubMatrix(r, c, Y);

                            Complex D1 = (GT.RemoveColumn(0) as SparseMatrix).Determinant();
                            Complex D2 = (GT.RemoveColumn(1) as SparseMatrix).Determinant();

                            Z[a][f] = -D1 / D2;
                        }
                    });
                }
                else
                {
                    SparseMatrix ILast, JLast;

                    if (T.Last()[0][0].RowCount == 2)
                    {
                        ILast = SparseMatrix.CreateIdentity(2);
                        JLast = SparseMatrix.CreateIdentity(2);
                    }
                    else if (T.Last()[0][0].RowCount == 4)
                    {
                        ILast = Explicit_TMM.InterfaceSF_Solid();
                        JLast = Explicit_TMM.InterfaceSF_Fluid();
                    }
                    else
                    {
                        ILast = Explicit_TMM.Interfacepf_Porous(LayerList.Last().porosity);
                        JLast = Explicit_TMM.Interfacepf_Fluid(LayerList.Last().porosity);
                    }

                    N += JLast.RowCount;

                    Parallel.For(0, sintheta_inc.Length, a =>
                    //for (int a = 0; a < sintheta_inc.Length; a++)
                    {
                        Z[a] = new Complex[sintheta_inc[a].Length];
                        Reflection_Coef[a] = new Complex[sintheta_inc[a].Length];
                        Trans_Loss[a] = new Complex[sintheta_inc[a].Length];
                        //Construct [D]...
                        for (int f = 0; f < sintheta_inc[a].Length; f++)
                        {
                            SparseMatrix GT = new SparseMatrix(N, N + 1);
                            int r = 0, c = 0;

                            for (int i = 0; i < T.Count; i++)
                            {
                                GT.SetSubMatrix(r, c, I[i]);
                                c += I[i].ColumnCount;
                                SparseMatrix subJT = J[i] * T[i][a][f];
                                GT.SetSubMatrix(r, c, subJT);
                                r += subJT.RowCount;
                            }

                            GT.SetSubMatrix(r, c, ILast);
                            GT.SetSubMatrix(r, c + ILast.ColumnCount, JLast);

                            Complex costheta = Complex.Sqrt(1 - sintheta_inc[a][f] * sintheta_inc[a][f]);

                            GT[N - 1, N - 1] = -1;
                            GT[N - 1, N] = Zc_Air[f] / costheta;

                            Complex D1 = (GT.RemoveColumn(0) as SparseMatrix).Determinant();
                            Complex D2 = (GT.RemoveColumn(1) as SparseMatrix).Determinant();

                            Z[a][f] = -D1 / D2;
                            Reflection_Coef[a][f] = (Z[a][f] * costheta - Zc_Air[f]) / (Z[a][f] * costheta + Zc_Air[f]);

                            ///Checking Values...///
                            if (a == 18 && f == 2000)
                            {
                                for (int i = 0; i < GT.RowCount; i++)
                                {
                                    string line = "| ";
                                    for (int j = 0; j < GT.ColumnCount; j++) line += GT[i, j].ToString() + " ";
                                    line += " |";
                                    Rhino.RhinoApp.WriteLine(line);
                                }
                            }
                            ////////////////////////

                            Complex D7 = (GT.RemoveColumn(GT.ColumnCount - 1) as SparseMatrix).Determinant();
                            Trans_Loss[a][f] = (-(1 + Reflection_Coef[a][f]) * D7 / D1) * Zc_Air[f];
                        }
                    });
                }

                R = Reflection_Coef;
                Trans = Trans_Loss;
                return Z;
            }

            public static Complex[][] Recursive_Transfer_Matrix(bool freq_log, int sample_Freq, double c_sound, List<ABS_Layer> LayerList, ref double[] frequency, ref Complex[] anglesdeg)//, ref double[] D, ref double[] T)///Add 'D' term, for distance traveled laterally before exit. One number for each direction.
            {
                if (freq_log)
                {
                    //3rd octave band frequencies...
                    List<double> freq = new List<double>();
                    double f = 15.625;

                    int ct = 1;
                    while (f < sample_Freq / 2)
                    {
                        ct++;
                        f = 15.625 * Math.Pow(2, (double)ct / 3f);
                        freq.Add(f);
                    }
                    frequency = freq.ToArray();
                }
                else
                {
                    //Linear frequency scale
                    frequency = new double[4096];
                    double step = ((double)sample_Freq) / 4096d;
                    for (int i = 0; i < frequency.Length; i++) frequency[i] = ((double)i + .5) * step;
                }

                if (anglesdeg == null)
                {
                    anglesdeg = new Complex[(int)(180 / 5)];
                    anglesdeg[0] = -87.5;
                    for (int i = 1; i < anglesdeg.Length; i++) anglesdeg[i] += 5;
                }
                //D = new double[anglesdeg.Length];
                //T = new double[anglesdeg.Length];
                double[] Angle = new double[anglesdeg.Length];
                Complex[][] sintheta_inc = new Complex[Angle.Length][];
                Complex[][][] Angle_Inc = new Complex[LayerList.Count][][];
                Complex[] K_Air = AbsorptionModels.Operations.Air_Wavenumber(c_sound, frequency);
                Complex[] Zc_Air = AbsorptionModels.Operations.Air_CharImpedance(1.2, c_sound, frequency);

                for (int a = 0, i = 0; a < 180; a += 5, i++)
                {
                    sintheta_inc[i] = new Complex[frequency.Length];
                    anglesdeg[i] = 87.5 - a;
                    if (a <= 90) Angle[i] = (87.5 - a) * Utilities.Numerics.Pi_180;
                    sintheta_inc[i][0] = Complex.Sin(a < 90 ? Angle[i] : Angle[36 - i]);
                    for (int j = 1; j < sintheta_inc[i].Length; j++) sintheta_inc[i][j] = sintheta_inc[i][0];
                }

                //Get Characteristic Impedance, Wave Number and medium angle of incidence for each layer:
                //for (int i = LayerList.Count - 1; i >= 0; i--)
                //{
                //    //Angle_Inc[i] = new Complex[37][];
                //    ABS_Layer Layer_i = (LayerList[i] as ABS_Layer);
                //}

                //Get Normal Incidence Wave Number for each layer and calculate surface impedance...
                //Complex[][] kyi = new Complex[37][];
                //Complex[][] K = new Complex[K_Air.Length][];

                return LayerEF(LayerList.Count - 1, LayerList, K_Air, sintheta_inc, K_Air, c_sound, frequency);//, ref D, ref T);
            }

            //Set up transfer function as a recursive function using explicit matrices for broader layer support...
            private static Complex[][] LayerEF(int layer_no, List<ABS_Layer> LayerList, Complex[] K0, Complex[][] sintheta_inc, Complex[] K_Air, double c_sound, double[] freq)//, ref double[] D, ref double[] T)
            {
                ABS_Layer Layer_i = LayerList[layer_no] as ABS_Layer;
                double depth = Layer_i.depth;
                double width = Layer_i.width;
                double pitch = Layer_i.pitch;
                double porosity;
                Complex[][] sintheta_trans = new Complex[sintheta_inc.Length][];
                Complex[][] Kxi = new Complex[sintheta_trans.Length][];

                switch (Layer_i.T)
                {
                    case ABS_Layer.LayerType.AirSpace:
                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kxi[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K_Air[j];
                                Kxi[i][j] = Complex.Sqrt(K_Air[j] * K_Air[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
                            }
                        }

                        Complex[] Zc_Air = Operations.Air_CharImpedance(1.2, c_sound, freq);

                        //DistTime(ref D, ref T, depth, sintheta_trans, K_Air, freq); 

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K_Air, sintheta_trans, K_Air, c_sound, freq);//, ref D, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc_Air, Z, K_Air, Kxi, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc_Air, K_Air, Kxi, depth);
                        }
                    case ABS_Layer.LayerType.PorousDB:
                        Complex[] K = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Complex[] Zc = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kxi[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
                                Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K, sintheta_trans, K_Air, c_sound, freq);//, ref D, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kxi, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kxi, depth);
                        }
                    case ABS_Layer.LayerType.PorousCA:
                        K = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Zc = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kxi[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
                                Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K, sintheta_trans, K_Air, c_sound, freq);//, ref D, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kxi, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kxi, depth);
                        }
                    case ABS_Layer.LayerType.PorousM:
                        K = AbsorptionModels.Equivalent_Fluids.DB_Miki_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Zc = AbsorptionModels.Equivalent_Fluids.DB_Miki_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kxi[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
                                Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K, sintheta_trans, K_Air, c_sound, freq);//, ref D, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kxi, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kxi, depth);
                        }
                    case ABS_Layer.LayerType.Perforated_Modal:
                    case ABS_Layer.LayerType.Slotted_Modal:
                        double R, S, s;
                        R = width / 2;
                        int nx = 0;
                        for (;;)
                        {
                            nx++;
                            if (171.5 * Math.Sqrt(nx * nx / (pitch * pitch)) > 16000) break;
                        }
                        int m_min, m_max, n_min, n_max;

                        if (Layer_i.T == ABS_Layer.LayerType.Perforated_Modal)
                        {
                            m_min = 0; m_max = nx; n_min = -nx; n_max = nx;
                            S = Math.PI * R * R;
                            s = S / (pitch * pitch);
                        }
                        else
                        {
                            m_min = 0; m_max = 0; n_min = -nx; n_max = nx;
                            S = 2 * R * pitch;
                            s = 2 * R / pitch;
                        }

                        double vpm;
                        porosity = (LayerList[layer_no - 1] as ABS_Layer).porosity;
                        double Eta = 0.48 * Math.Sqrt(S) * (1 - 1.14 * Math.Sqrt(s));

                        if (layer_no < 1)
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int i = 0; i < sintheta_inc.Length; i++)
                            {
                                Z[i] = new Complex[freq.Length];
                                for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
                            }
                            return Z;
                        }
                        else
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int j = 0; j < sintheta_inc.Length; j++) Z[j] = new Complex[freq.Length];

                            Parallel.For(m_min, m_max + 1, m =>
                            //for (int m = m_min; m <= m_max; m++)
                            {
                                for (int n = n_min; n <= n_max; n++)
                                {
                                    Complex[][] Zmn;

                                    if (m == 0 && n == 0)
                                    {
                                        Zmn = LayerEF(layer_no - 1, LayerList, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
                                            for (int j = 0; j < Zmn.Length; j++)
                                        {
                                            for (int i = 0; i < freq.Length; i++)
                                            {
                                                Z[j][i] += s / porosity * Zmn[j][i];
                                            }
                                        }
                                        continue;
                                    }

                                    if (m == 0 || n == 0) vpm = .5; else vpm = 1;
                                    double M_TERM = Utilities.Numerics.PiX2 * m / pitch;
                                    double N_TERM = Utilities.Numerics.PiX2 * n / pitch;
                                    Zmn = LayerEF_postperf(layer_no - 1, LayerList, K_Air, N_TERM, M_TERM, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);

                                        for (int j = 0; j < Zmn.Length; j++)
                                    {
                                        for (int i = 0; i < freq.Length; i++)
                                        {
                                            Z[j][i] += vpm * Zmn[j][i] * Complex.Pow(Utilities.Numerics.jBessel(1, 2 * Math.PI * R * Complex.Sqrt((m * m / (pitch * pitch)) + Complex.Pow(n / pitch - K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2))) / (m * m + Complex.Pow(n - pitch * K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2)), 2);
                                        }
                                    }
                                }
                            });

                            for (int j = 0; j < Z.Length; j++)
                            {
                                for (int i = 0; i < freq.Length; i++)
                                {
                                    Z[j][i] *= 2 / (Math.PI * porosity);
                                    Z[j][i] += new Complex(0, (Eta + depth) * 1.2 * freq[i] * Utilities.Numerics.PiX2);
                                    Z[j][i] /= (Math.PI * R * R / (pitch * pitch));
                                }
                            }
                            return Z;
                        }
                    case ABS_Layer.LayerType.CircularPerforations:
                    case ABS_Layer.LayerType.SquarePerforations:
                        double w = Layer_i.width;
                        double p = Layer_i.pitch;
                        double d = Layer_i.depth;
                        if (Layer_i.T == ABS_Layer.LayerType.CircularPerforations)
                        {
                            porosity = Math.PI * width * width * .25 / (pitch * pitch);
                        }
                        else
                        {
                            porosity = width * width / (pitch * pitch);
                        }

                        if (layer_no < 1)
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int i = 0; i < sintheta_inc.Length; i++)
                            {
                                Z[i] = new Complex[freq.Length];
                                for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
                            }
                            return Z;
                        }
                        else
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
                            Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.PerforatedPlate_Impedance(1.2, porosity, w, p, d, freq);

                            for (int j = 0; j < sintheta_inc[0].Length; j++)
                            {
                                for (int i = 0; i < sintheta_inc.Length; i++)
                                {
                                    Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
                                }
                            }
                            return Z;
                        }
                    case ABS_Layer.LayerType.Slots:
                        if (layer_no < 1)
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int i = 0; i < sintheta_inc.Length; i++)
                            {
                                Z[i] = new Complex[freq.Length];
                                for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
                            }
                            return Z;
                        }
                        else
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
                            Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.SlottedPlate_Impedance(1.2, c_sound, width, pitch, depth, freq);

                            for (int j = 0; j < sintheta_inc[0].Length; j++)
                            {
                                for (int i = 0; i < sintheta_inc.Length; i++)
                                {
                                    Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
                                }
                            }
                            return Z;
                        }
                    case ABS_Layer.LayerType.Microslit:
                        if (layer_no < 1)
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int i = 0; i < sintheta_inc.Length; i++)
                            {
                                Z[i] = new Complex[freq.Length];
                                for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
                            }
                            return Z;
                        }
                        else
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
                            Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.MicroSlots_Impedance(1.2, c_sound, width, pitch, depth, freq);

                            for (int j = 0; j < sintheta_inc[0].Length; j++)
                            {
                                for (int i = 0; i < sintheta_inc.Length; i++)
                                {
                                    Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
                                }
                            }
                            return Z;
                        }
                    case ABS_Layer.LayerType.MicroPerforated:
                        if (Layer_i.T == ABS_Layer.LayerType.CircularPerforations)
                        {
                            porosity = Math.PI * width * width * .25 / (pitch * pitch);
                        }
                        else
                        {
                            porosity = width * width / (pitch * pitch);
                        }

                        if (layer_no < 1)
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int i = 0; i < sintheta_inc.Length; i++)
                            {
                                Z[i] = new Complex[freq.Length];
                                for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
                            }
                            return Z;
                        }
                        else
                        {
                            Complex[][] Z = LayerEF(layer_no - 1, LayerList, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
                            Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.MicroPerforatedPlate_Impedance(1.2, c_sound, width, pitch, depth, freq);

                            for (int j = 0; j < sintheta_inc[0].Length; j++)
                            {
                                for (int i = 0; i < sintheta_inc.Length; i++)
                                {
                                    Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
                                }
                            }
                            return Z;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }

            ////Set up transfer function after perforated materials as a recursive function...
            private static Complex[][] LayerEF_postperf(int layer_no, List<ABS_Layer> LayerList, Complex[] K0, double N_Term, double M_Term, Complex[][] sintheta_inc, Complex[] K_Air, double c_sound, double[] freq)//, ref double[] D_Lat, ref double[] T)
            {
                ABS_Layer Layer_i = LayerList[layer_no] as ABS_Layer;
                double depth = Layer_i.depth;
                Complex[][] sintheta_trans = new Complex[sintheta_inc.Length][];
                Complex[][] Kmn = new Complex[sintheta_inc.Length][];
                //Complex[][] Kxi = new Complex[sintheta_trans.Length][];

                switch (Layer_i.T)
                {
                    case ABS_Layer.LayerType.AirSpace:

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K_Air[j];
                                Kmn[i][j] = Complex.Sqrt(K_Air[j] * K_Air[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        Complex[] Zc_Air = Operations.Air_CharImpedance(1.2, c_sound, freq);

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF_postperf(layer_no - 1, LayerList, K_Air, M_Term, N_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc_Air, Z, K_Air, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc_Air, K_Air, Kmn, depth);
                        }
                    case ABS_Layer.LayerType.PorousDB:
                        Complex[] K = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Complex[] Zc = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
                                Kmn[i][j] = Complex.Sqrt(K[j] * K[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF_postperf(layer_no - 1, LayerList, K, M_Term, N_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kmn, depth);
                        }
                    //K[i] = AbsorptionModels.Porous_Absorbers.DelaneyBazley_WNumber(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Zc[i] = AbsorptionModels.Porous_Absorbers.DelaneyBazley_Impedance(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Screens[i] = i > 0 ? Screens[i - 1] : 0;
                    case ABS_Layer.LayerType.PorousCA:
                        K = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Zc = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                //sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
                                Kmn[i][j] = Complex.Sqrt(K[j] * K[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF_postperf(layer_no - 1, LayerList, K, N_Term, M_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kmn, depth);
                        }
                    //K[i] = AbsorptionModels.Porous_Absorbers.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Zc[i] = AbsorptionModels.Porous_Absorbers.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Screens[i] = i > 0 ? Screens[i - 1] : 0;
                    case ABS_Layer.LayerType.PorousM:
                        K = AbsorptionModels.Equivalent_Fluids.DB_Miki_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Zc = AbsorptionModels.Equivalent_Fluids.DB_Miki_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                Kmn[i][j] = Complex.Sqrt(K[j] * K[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = LayerEF_postperf(layer_no - 1, LayerList, K, N_Term, M_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kmn, depth);
                        }
                    case ABS_Layer.LayerType.Perforated_Modal:
                    case ABS_Layer.LayerType.Slotted_Modal:
                        ABS_Layer AL = (LayerList[layer_no] as ABS_Layer);
                        double D, R, S, s;
                        D = AL.pitch;
                        R = AL.width / 2;
                        int nx = 0;
                        for (;;)
                        {
                            nx++;
                            if (171.5 * Math.Sqrt(nx * nx / (D * D)) > 16000) break;
                        }
                        int m_min, m_max, n_min, n_max;


                        if (AL.T == ABS_Layer.LayerType.Perforated_Modal)
                        {
                            m_min = 0; m_max = nx; n_min = -nx; n_max = nx;
                            S = Math.PI * R * R;
                            s = S / (D * D);
                        }
                        else
                        {
                            m_min = 0; m_max = 0; n_min = -nx; n_max = nx;
                            S = 2 * R * D;
                            s = 2 * R / D;
                        }

                        double vpm;
                        double porosity = (LayerList[layer_no - 1] as ABS_Layer).porosity;
                        double Eta = 0.48 * Math.Sqrt(S) * (1 - 1.14 * Math.Sqrt(s));

                        if (layer_no < 1)
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int i = 0; i < sintheta_inc.Length; i++)
                            {
                                Z[i] = new Complex[freq.Length];
                                for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
                            }
                            return Z;
                        }
                        else
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int j = 0; j < sintheta_inc.Length; j++) Z[j] = new Complex[freq.Length];

                            for (int m = m_min; m <= m_max; m++)
                            {
                                if (m == 0) vpm = .5; else vpm = 1;
                                for (int n = n_min; n <= n_max; n++)
                                {
                                    Complex[][] rootsubKmn = new Complex[sintheta_inc.Length][];
                                    double M_TERM = Utilities.Numerics.PiX2 * m / D;
                                    double N_TERM = Utilities.Numerics.PiX2 * n / D;
                                    Complex[][] Zmn = LayerEF_postperf(layer_no - 1, LayerList, K_Air, N_TERM, M_TERM, sintheta_inc, K_Air, c_sound, freq);//, ref D_Lat, ref T);

                                    for (int j = 0; j < sintheta_inc.Length; j++)
                                    {
                                        for (int i = 0; i < sintheta_inc[j].Length; i++)
                                        {
                                            Z[j][i] += vpm * Zmn[j][i] * Complex.Pow(Utilities.Numerics.jBessel(1, 2 * Math.PI * R * ((m * m / (D * D)) + Complex.Pow(n / D - K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2))) / (m * m + Complex.Pow(n - D * K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2)), 2);
                                        }
                                    }
                                }
                            }

                            for (int j = 0; j < sintheta_inc.Length; j++)
                            {
                                for (int i = 0; i < sintheta_inc[j].Length; i++)
                                {
                                    Z[j][i] *= 2 / (Math.PI * porosity);
                                    Z[j][i] += new Complex(0, (Eta + depth) * 1.2 * i * Utilities.Numerics.PiX2);
                                }
                            }
                            return Z;
                        }
                    case ABS_Layer.LayerType.Microslit:
                        return null;
                    case ABS_Layer.LayerType.MicroPerforated:
                        return null;
                    default:
                        throw new Exception("Invalid layer choice");
                }
            }

            //            SparseMatrix V;
            //if (Rigid_Backed)
            //{
            //    if ((int)LayerList[i].T <= 10)
            //    {
            //    }
            //    else if ((int)(int)LayerList[i+1].T == 15)
            //    {
            //        //Biot Porous absorbers
            //        V = Explicit_TMM.RigidTerminationP();
            //    }
            //    else
            //    {
            //        //Solid Layer
            //        V = Explicit_TMM.RigidTerminationS();
            //    }
            //}
            //else
            //{
            //    //Transmission case: Start with a fluid - air
            //    //V = (-1, (1.2*c_sound)/Math.Cos(theta));//(1, 1 / (1.2 * c_sound));
            //    throw new NotImplementedException("Working on intialization condition for Transfer Matrix for Transmssion Case");
            //}

            //Set up transfer function as a recursive function...

            //private static List<SparseMatrix[][]> Layer(int layer_no, List<ABS_Layer> LayerList, Complex[] K0, Complex[][] sintheta_inc, Complex[] K_Air, double c_sound, double[] freq)//, ref double[] D, ref double[] T)
            //{
            //    ABS_Layer Layer_i = LayerList[layer_no] as ABS_Layer;
            //    double depth = Layer_i.depth;
            //    double width = Layer_i.width;
            //    double pitch = Layer_i.pitch;
            //    //double porosity;
            //    Complex[][] sintheta_trans = new Complex[sintheta_inc.Length][];
            //    Complex[][] Kxi = new Complex[sintheta_trans.Length][];
            //    SparseMatrix[][] V = new SparseMatrix[sintheta_inc.Length][];
            //    Complex[] K;//, Zc;

            //    for (int i = 0; i < sintheta_inc.Length; i++)
            //    {
            //        V[i] = new SparseMatrix[freq.Length];
            //        Kxi[i] = new Complex[freq.Length];
            //        sintheta_trans[i] = new Complex[freq.Length];
            //    }

            //    List<SparseMatrix[][]> Tlist = new List<SparseMatrix[][]>();
            //    SparseMatrix[][] T = new SparseMatrix[sintheta_inc.Length][];
            //    for (int i = 0; i < sintheta_inc.Length; i++) T[i] = new SparseMatrix[sintheta_inc[i].Length];

            //    if (layer_no == 0) Tlist = new List<SparseMatrix[][]>();

            //    switch (Layer_i.T)
            //    {
            //        case ABS_Layer.LayerType.BiotPorousAbsorber_Limp:

            //            K = AbsorptionModels.Biot_Porous_Absorbers.WaveNumber_Fluid(1.2, Layer_i.porosity, Layer_i.Thermal_Permeability, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[0].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0) Tlist = Layer(layer_no - 1, LayerList, K, sintheta_trans, K_Air, c_sound, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                for (int j = 0; j < sintheta_inc[0].Length; j++)
            //                {
            //                    T[i][j] = Explicit_TMM.PorousMatrix(false, depth, K0[j], sintheta_trans[i][j], freq[j], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YoungsModulus, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].Viscous_Characteristic_Length, LayerList[layer_no].Flow_Resist, LayerList[layer_no].density, LayerList[layer_no].Thermal_Permeability, 101325);
            //                }
            //            }

            //            Tlist.Add(T);
            //            return Tlist;

            //        case ABS_Layer.LayerType.SolidPlate:
            //            K = AbsorptionModels.Solids.WaveNumber(freq, LayerList[layer_no].SpeedOfSound);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[0].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0) Tlist = Layer(layer_no - 1, LayerList, K, sintheta_trans, K_Air, c_sound, freq);
            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                for (int j = 0; j < sintheta_inc[0].Length; j++)
            //                {
            //                    T[i][j] = Explicit_TMM.Solid_Matrix(K[j], sintheta_trans[i][j], depth, freq[j], LayerList[layer_no].density, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].YoungsModulus); //(false, depth, K0, sintheta_trans[a][f], freq[f], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YougsModulus, LayerList[layer_no].PoissonRatio, LayerList[layer_no].Viscous_CharacteristicLength, LayerList[layer_no].Flow_Resist, LayerList[layer_no].FrameRho, LayerList[layer_no].Thermal_Permeability, 101325);
            //                }
            //            }

            //            Tlist.Add(T);
            //            return Tlist;

            //        case ABS_Layer.LayerType.AirSpace:
            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K_Air[j];
            //                    Kxi[i][j] = Complex.Sqrt(K_Air[j] * K_Air[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0) Tlist = Layer(layer_no - 1, LayerList, K_Air, sintheta_trans, K_Air, c_sound, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    T[i][j] = Explicit_TMM.FluidMatrix(depth, Kxi[i][j], freq[j], 1.2);
            //                }
            //            }

            //            Tlist.Add(T);
            //            return Tlist;

            //        case ABS_Layer.LayerType.PorousDB:
            //            K = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0) Tlist = Layer(layer_no - 1, LayerList, K_Air, sintheta_trans, K_Air, c_sound, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    T[i][j] = Explicit_TMM.FluidMatrix(depth, Kxi[i][j], freq[j], 1.2);
            //                }
            //            }

            //            Tlist.Add(T);
            //            return Tlist;

            //        case ABS_Layer.LayerType.PorousCA:
            //            K = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0) Tlist = Layer(layer_no - 1, LayerList, K_Air, sintheta_trans, K_Air, c_sound, freq);
            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    T[i][j] = Explicit_TMM.FluidMatrix(depth, Kxi[i][j], freq[j], 1.2);
            //                }
            //            }

            //            Tlist.Add(T);
            //            return Tlist;

            //        case ABS_Layer.LayerType.PorousM:
            //            K = AbsorptionModels.Equivalent_Fluids.DB_Miki_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0) Tlist = Layer(layer_no - 1, LayerList, K_Air, sintheta_trans, K_Air, c_sound, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    T[i][j] = Explicit_TMM.FluidMatrix(depth, Kxi[i][j], freq[j], 1.2);
            //                }
            //            }

            //            Tlist.Add(T);
            //            return Tlist;

            //        //case ABS_Layer.LayerType.Perforated_Modal:
            //        //case ABS_Layer.LayerType.Slotted_Modal:
            //        //Todo: work out TMM perforated panels.
            //        //double R, S, s;
            //        //R = width / 2;
            //        //int nx = 0;
            //        //for (; ; )
            //        //{
            //        //    nx++;
            //        //    if (171.5 * Math.Sqrt(nx * nx / (pitch * pitch)) > 16000) break;
            //        //}
            //        //int m_min, m_max, n_min, n_max;

            //        //if (Layer_i.T == ABS_Layer.LayerType.Perforated_Modal)
            //        //{
            //        //    m_min = 0; m_max = nx; n_min = -nx; n_max = nx;
            //        //    S = Math.PI * R * R;
            //        //    s = S / (pitch * pitch);
            //        //}
            //        //else
            //        //{
            //        //    m_min = 0; m_max = 0; n_min = -nx; n_max = nx;
            //        //    S = 2 * R * pitch;
            //        //    s = 2 * R / pitch;
            //        //}

            //        //double vpm;
            //        //porosity = (LayerList[layer_no - 1] as ABS_Layer).porosity;
            //        //double Eta = 0.48 * Math.Sqrt(S) * (1 - 1.14 * Math.Sqrt(s));

            //        //if (layer_no < 1)
            //        //{
            //        //    Complex[][] Z = new Complex[37][];
            //        //    for (int i = 0; i < 37; i++)
            //        //    {
            //        //        Z[i] = new Complex[freq.Length];
            //        //        for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //    }
            //        //    return Z;
            //        //}
            //        //else
            //        //{
            //        //    Complex[][] Z = new Complex[37][];
            //        //    for (int j = 0; j < 37; j++) Z[j] = new Complex[freq.Length];

            //        //    Parallel.For(m_min, m_max + 1, m =>
            //        //    //for (int m = m_min; m <= m_max; m++)
            //        //    {
            //        //        for (int n = n_min; n <= n_max; n++)
            //        //        {
            //        //            Complex[][] Zmn;

            //        //            if (m == 0 && n == 0)
            //        //            {
            //        //                Zmn = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //                for (int j = 0; j < Zmn.Length; j++)
            //        //                {
            //        //                    for (int i = 0; i < freq.Length; i++)
            //        //                    {
            //        //                        Z[j][i] += s / porosity * Zmn[j][i];
            //        //                    }
            //        //                }
            //        //                continue;
            //        //            }

            //        //            if (m == 0 || n == 0) vpm = .5; else vpm = 1;
            //        //            double M_TERM = Utilities.Numerics.PiX2 * m / pitch;
            //        //            double N_TERM = Utilities.Numerics.PiX2 * n / pitch;
            //        //            Zmn = Layer_postperf(layer_no - 1, LayerList, J, I_inv, K_Air, N_TERM, M_TERM, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);

            //        //            for (int j = 0; j < Zmn.Length; j++)
            //        //            {
            //        //                for (int i = 0; i < freq.Length; i++)
            //        //                {
            //        //                    Z[j][i] += vpm * Zmn[j][i] * Complex.Pow(Utilities.Numerics.jBessel(1, 2 * Math.PI * R * Complex.Sqrt((m * m / (pitch * pitch)) + Complex.Pow(n / pitch - K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2))) / (m * m + Complex.Pow(n - pitch * K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2)), 2);
            //        //                }
            //        //            }
            //        //        }
            //        //    });

            //        //    for (int j = 0; j < Z.Length; j++)
            //        //    {
            //        //        for (int i = 0; i < freq.Length; i++)
            //        //        {
            //        //            Z[j][i] *= 2 / (Math.PI * porosity);
            //        //            Z[j][i] += new Complex(0, (Eta + depth) * 1.2 * freq[i] * Utilities.Numerics.PiX2);
            //        //            Z[j][i] /= (Math.PI * R * R / (pitch * pitch));
            //        //        }
            //        //    }
            //        //    return Z;
            //        //}
            //        //case ABS_Layer.LayerType.CircularPerforations:
            //        //case ABS_Layer.LayerType.SquarePerforations:
            //        //double w = Layer_i.width;
            //        //double p = Layer_i.pitch;
            //        //double d = Layer_i.depth;
            //        //if (Layer_i.T == ABS_Layer.LayerType.CircularPerforations)
            //        //{
            //        //    porosity = Math.PI * width * width * .25 / (pitch * pitch);
            //        //}
            //        //else
            //        //{
            //        //    porosity = width * width / (pitch * pitch);
            //        //}

            //        //if (layer_no < 1)
            //        //{
            //        //    Complex[][] Z = new Complex[37][];
            //        //    for (int i = 0; i < 37; i++)
            //        //    {
            //        //        Z[i] = new Complex[freq.Length];
            //        //        for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //    }
            //        //    return Z;
            //        //}
            //        //else
            //        //{
            //        //    Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //    Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.PerforatedPlate_Impedance(1.2, porosity, w, p, d, freq);

            //        //    for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //    {
            //        //        for (int i = 0; i < sintheta_inc.Length; i++)
            //        //        {
            //        //            Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //        }
            //        //    }
            //        //    return Z;
            //        //}
            //        //case ABS_Layer.LayerType.Slots:
            //        //    if (layer_no < 1)
            //        //    {
            //        //        Complex[][] Z = new Complex[37][];
            //        //        for (int i = 0; i < 37; i++)
            //        //        {
            //        //            Z[i] = new Complex[freq.Length];
            //        //            for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //        }
            //        //        return Z;
            //        //    }
            //        //    else
            //        //    {
            //        //        Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //        Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.SlottedPlate_Impedance(1.2, c_sound, width, pitch, depth, freq);

            //        //        for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //        {
            //        //            for (int i = 0; i < sintheta_inc.Length; i++)
            //        //            {
            //        //                Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //            }
            //        //        }
            //        //        return Z;
            //        //    }
            //        //case ABS_Layer.LayerType.Microslit:
            //        //    if (layer_no < 1)
            //        //    {
            //        //        Complex[][] Z = new Complex[37][];
            //        //        for (int i = 0; i < 37; i++)
            //        //        {
            //        //            Z[i] = new Complex[freq.Length];
            //        //            for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //        }
            //        //        return Z;
            //        //    }
            //        //    else
            //        //    {
            //        //        Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //        Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.MicroSlots_Impedance(1.2, c_sound, width, pitch, depth, freq);

            //        //        for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //        {
            //        //            for (int i = 0; i < sintheta_inc.Length; i++)
            //        //            {
            //        //                Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //            }
            //        //        }
            //        //        return Z;
            //        //    }
            //        //case ABS_Layer.LayerType.MicroPerforated:
            //        //    if (Layer_i.T == ABS_Layer.LayerType.CircularPerforations)
            //        //    {
            //        //        porosity = Math.PI * width * width * .25 / (pitch * pitch);
            //        //    }
            //        //    else
            //        //    {
            //        //        porosity = width * width / (pitch * pitch);
            //        //    }

            //        //    if (layer_no < 1)
            //        //    {
            //        //        Complex[][] Z = new Complex[37][];
            //        //        for (int i = 0; i < 37; i++)
            //        //        {
            //        //            Z[i] = new Complex[freq.Length];
            //        //            for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //        }
            //        //        return Z;
            //        //    }
            //        //    else
            //        //    {
            //        //        Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //        Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.MicroPerforatedPlate_Impedance(1.2, c_sound, width, pitch, depth, freq);

            //        //        for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //        {
            //        //            for (int i = 0; i < sintheta_inc.Length; i++)
            //        //            {
            //        //                Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //            }
            //        //        }
            //        //        return Z;
            //        //    }
            //        default:
            //            throw new NotImplementedException();
            //    }
            //}

            //private static SparseMatrix[][] Layer(int layer_no, bool Rigid_Backed, List<ABS_Layer> LayerList, SparseMatrix[] J, SparseMatrix[] I, Complex[] K0, Complex[][] sintheta_inc, Complex[] K_Air, double c_sound, double[] freq, ref SparseMatrix Denom)//, ref double[] D, ref double[] T)
            //{
            //    ABS_Layer Layer_i = LayerList[layer_no] as ABS_Layer;
            //    double depth = Layer_i.depth;
            //    double width = Layer_i.width;
            //    double pitch = Layer_i.pitch;
            //    //double porosity;
            //    Complex[][] sintheta_trans = new Complex[sintheta_inc.Length][];
            //    Complex[][] Kxi = new Complex[sintheta_trans.Length][];
            //    SparseMatrix[][] V = new SparseMatrix[sintheta_inc.Length][];
            //    Complex[] K, Zc;

            //    for (int i = 0; i < sintheta_inc.Length; i++)
            //    {
            //        V[i] = new SparseMatrix[freq.Length];
            //        Kxi[i] = new Complex[freq.Length];
            //        sintheta_trans[i] = new Complex[freq.Length];
            //    }

            //    switch (Layer_i.T)
            //    {
            //        case ABS_Layer.LayerType.BiotPorousAbsorber_Limp:

            //            K = AbsorptionModels.Biot_Porous_Absorbers.WaveNumber_Fluid(1.2, Layer_i.porosity, Layer_i.Thermal_Permeability, freq);

            //            for (int j = 0; j < sintheta_inc[0].Length; j++)
            //            {
            //                for (int i = 0; i < sintheta_inc.Length; i++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0)
            //            {
            //                V = Layer(layer_no - 1, Rigid_Backed, LayerList, J, I, K, sintheta_trans, K_Air, c_sound, freq, ref Denom);//, ref D, ref T);

            //                for (int a = 0; a < sintheta_inc.Length; a++)
            //                {
            //                    for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                    {
            //                        SparseMatrix T = Explicit_TMM.PorousMatrix(false, depth, K0[f], sintheta_trans[a][f], freq[f], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YoungsModulus, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].Viscous_Characteristic_Length, LayerList[layer_no].Flow_Resist, LayerList[layer_no].density, LayerList[layer_no].Thermal_Permeability, 101325);
            //                        V[a][f] = Transfer(V[a][f], T, J[layer_no], I[layer_no]);
            //                    }
            //                }
            //                if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //            }
            //            else
            //            {
            //                if (Rigid_Backed)
            //                {
            //                    for (int a = 0; a < sintheta_inc.Length; a++)
            //                    {
            //                        for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                        {
            //                            V[a][f] = Explicit_TMM.RigidTerminationP();
            //                            SparseMatrix T = Explicit_TMM.PorousMatrix(false, depth, K0[f], sintheta_trans[a][f], freq[f], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YoungsModulus, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].Viscous_Characteristic_Length, LayerList[layer_no].Flow_Resist, LayerList[layer_no].density, LayerList[layer_no].Thermal_Permeability, 101325);
            //                            //V[a][f] = Explicit_TMM.PorousMatrix(false, depth, K0[f], sintheta_trans[a][f], freq[f], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YoungsModulus, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].Viscous_Characteristic_Length, LayerList[layer_no].Flow_Resist, LayerList[layer_no].density, LayerList[layer_no].Thermal_Permeability, 101325);
            //                            //V[a][f] = Transfer(V[a][f], T, J[layer_no], I[layer_no]);                                        
            //                            if (J[layer_no] != null) V[a][f] = J[layer_no] * V[a][f] * T;
            //                        }
            //                    }
            //                    Denom = Explicit_TMM.RigidTerminationP();
            //                    if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //                }
            //                else
            //                {
            //                    throw new NotImplementedException();
            //                }
            //            }

            //            return V;
            //        //case ABS_Layer.LayerType.BiotPorousAbsorber_Rigid:
            //        //    

            //        //    
            //        case ABS_Layer.LayerType.SolidPlate:
            //            K = AbsorptionModels.Solids.WaveNumber(freq, LayerList[layer_no].SpeedOfSound);

            //            for (int j = 0; j < sintheta_inc[0].Length; j++)
            //            {
            //                for (int i = 0; i < sintheta_inc.Length; i++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0)
            //            {
            //                V = Layer(layer_no - 1, Rigid_Backed, LayerList, J, I, K, sintheta_trans, K_Air, c_sound, freq, ref Denom);//, ref D, ref T);

            //                for (int a = 0; a < sintheta_inc.Length; a++)
            //                {
            //                    for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                    {
            //                        SparseMatrix T = Explicit_TMM.Solid_Matrix(K[f], K0[f], sintheta_trans[a][f], depth, freq[f], LayerList[layer_no].density, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].YoungsModulus); //(false, depth, K0, sintheta_trans[a][f], freq[f], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YougsModulus, LayerList[layer_no].PoissonRatio, LayerList[layer_no].Viscous_CharacteristicLength, LayerList[layer_no].Flow_Resist, LayerList[layer_no].FrameRho, LayerList[layer_no].Thermal_Permeability, 101325);
            //                        V[a][f] = Transfer(V[a][f], T, J[layer_no], I[layer_no]);
            //                    }
            //                }
            //                if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //            }
            //            else
            //            {
            //                if (Rigid_Backed)
            //                {
            //                    for (int a = 0; a < sintheta_inc.Length; a++)
            //                    {
            //                        for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                        {
            //                            V[a][f] = Explicit_TMM.RigidTerminationS();
            //                            SparseMatrix T = Explicit_TMM.Solid_Matrix(K[f], K0[f], sintheta_trans[a][f], depth, freq[f], LayerList[layer_no].density, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].YoungsModulus); //(false, depth, K0, sintheta_trans[a][f], freq[f], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YougsModulus, LayerList[layer_no].PoissonRatio, LayerList[layer_no].Viscous_CharacteristicLength, LayerList[layer_no].Flow_Resist, LayerList[layer_no].FrameRho, LayerList[layer_no].Thermal_Permeability, 101325);
            //                                                                                                                                                                                                                              //V[a][f] = Explicit_TMM.Solid_Matrix(K[f], K0[f], sintheta_trans[a][f], depth, freq[f], LayerList[layer_no].density, LayerList[layer_no].PoissonsRatio, LayerList[layer_no].YoungsModulus); //(false, depth, K0, sintheta_trans[a][f], freq[f], LayerList[layer_no].porosity, LayerList[layer_no].tortuosity, LayerList[layer_no].YougsModulus, LayerList[layer_no].PoissonRatio, LayerList[layer_no].Viscous_CharacteristicLength, LayerList[layer_no].Flow_Resist, LayerList[layer_no].FrameRho, LayerList[layer_no].Thermal_Permeability, 101325);
            //                            if (J[layer_no] != null) V[a][f] = J[layer_no] * V[a][f] * T;
            //                        }
            //                    }
            //                    Denom = Explicit_TMM.RigidTerminationS();
            //                    if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //                }
            //                else
            //                {
            //                    throw new NotImplementedException();
            //                }
            //            }
            //            return V;
            //        case ABS_Layer.LayerType.AirSpace:
            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K_Air[j];
            //                    Kxi[i][j] = Complex.Sqrt(K_Air[j] * K_Air[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            //Complex[] Zc_Air = Operations.Air_CharImpedance(1.2, c_sound, freq);

            //            if (layer_no > 0)
            //            {
            //                V = Layer(layer_no - 1, Rigid_Backed, LayerList, J, I, K_Air, sintheta_trans, K_Air, c_sound, freq, ref Denom);//, ref D, ref T);
            //                for (int a = 0; a < sintheta_inc.Length; a++)
            //                {
            //                    for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                    {
            //                        SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                        V[a][f] = Transfer(V[a][f], T, J[layer_no], I[layer_no]);
            //                    }
            //                }
            //                if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //            }
            //            else
            //            {
            //                if (Rigid_Backed)
            //                {
            //                    for (int a = 0; a < sintheta_inc.Length; a++)
            //                    {
            //                        for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                        {
            //                            V[a][f] = Explicit_TMM.RigidTerminationF();
            //                            SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            //V[a][f] = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            if (J[layer_no] != null) V[a][f] = J[layer_no] * V[a][f] * T;
            //                        }
            //                    }
            //                    Denom = Explicit_TMM.RigidTerminationF();
            //                    if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //                }
            //                else
            //                {
            //                    throw new NotImplementedException();
            //                }
            //            }

            //            return V;
            //        case ABS_Layer.LayerType.PorousDB:
            //            K = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
            //            //Zc = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0)
            //            {
            //                V = Layer(layer_no - 1, Rigid_Backed, LayerList, J, I, K, sintheta_trans, K_Air, c_sound, freq, ref Denom);//, ref D, ref T);
            //                for (int a = 0; a < sintheta_inc.Length; a++)
            //                {
            //                    for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                    {
            //                        SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                        V[a][f] = Transfer(V[a][f], T, J[layer_no], I[layer_no]);
            //                    }
            //                }
            //                if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //            }
            //            else
            //            {
            //                if (Rigid_Backed)
            //                {
            //                    for (int a = 0; a < sintheta_inc.Length; a++)
            //                    {
            //                        for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                        {
            //                            V[a][f] = Explicit_TMM.RigidTerminationF();
            //                            SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            //V[a][f] = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            if (J[layer_no] != null) V[a][f] = J[layer_no] * V[a][f] * T;
            //                        }
            //                    }
            //                    Denom = Explicit_TMM.RigidTerminationF();
            //                    if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //                }
            //                else
            //                {
            //                    throw new NotImplementedException();
            //                }
            //            }
            //            return V;
            //        case ABS_Layer.LayerType.PorousCA:
            //            K = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
            //            //Zc = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0)
            //            {
            //                V = Layer(layer_no - 1, Rigid_Backed, LayerList, J, I, K_Air, sintheta_trans, K_Air, c_sound, freq, ref Denom);//, ref D, ref T);
            //                for (int a = 0; a < sintheta_inc.Length; a++)
            //                {
            //                    for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                    {
            //                        SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                        V[a][f] = Transfer(V[a][f], T, J[layer_no], I[layer_no]);
            //                    }
            //                }
            //                if (I[layer_no] != null) Denom = I[layer_no] * Denom;

            //            }
            //            else
            //            {
            //                if (Rigid_Backed)
            //                {
            //                    for (int a = 0; a < sintheta_inc.Length; a++)
            //                    {
            //                        for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                        {
            //                            V[a][f] = Explicit_TMM.RigidTerminationF();
            //                            SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            //V[a][f] = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            if (J[layer_no] != null) V[a][f] = J[layer_no] * V[a][f] * T;
            //                        }
            //                    }
            //                    Denom = Explicit_TMM.RigidTerminationF();
            //                    if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //                }
            //                else
            //                {
            //                    throw new NotImplementedException();
            //                }
            //            }
            //            return V;
            //        case ABS_Layer.LayerType.PorousM:
            //            K = AbsorptionModels.Equivalent_Fluids.DB_Miki_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
            //            Zc = AbsorptionModels.Equivalent_Fluids.DB_Miki_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

            //            for (int i = 0; i < sintheta_inc.Length; i++)
            //            {
            //                Kxi[i] = new Complex[freq.Length];
            //                sintheta_trans[i] = new Complex[freq.Length];
            //                for (int j = 0; j < sintheta_inc[i].Length; j++)
            //                {
            //                    sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
            //                    Kxi[i][j] = Complex.Sqrt(K[j] * K[j] * (1 - sintheta_trans[i][j] * sintheta_trans[i][j]));
            //                }
            //            }

            //            if (layer_no > 0)
            //            {
            //                V = Layer(layer_no - 1, Rigid_Backed, LayerList, J, I, K_Air, sintheta_trans, K_Air, c_sound, freq, ref Denom);//, ref D, ref T);
            //                for (int a = 0; a < sintheta_inc.Length; a++)
            //                {
            //                    for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                    {
            //                        SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                        V[a][f] = Transfer(V[a][f], T, J[layer_no], I[layer_no]);
            //                    }
            //                }
            //                if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //            }
            //            else
            //            {
            //                if (Rigid_Backed)
            //                {
            //                    for (int a = 0; a < sintheta_inc.Length; a++)
            //                    {
            //                        for (int f = 0; f < sintheta_inc[a].Length; f++)
            //                        {
            //                            V[a][f] = Explicit_TMM.RigidTerminationF();
            //                            SparseMatrix T = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            //V[a][f] = Explicit_TMM.FluidMatrix(depth, Kxi[a][f], freq[f], 1.2);
            //                            if (J[layer_no] != null) V[a][f] = J[layer_no] * V[a][f] * T;
            //                        }
            //                    }
            //                    Denom = Explicit_TMM.RigidTerminationF();
            //                    if (I[layer_no] != null) Denom = I[layer_no] * Denom;
            //                }
            //                else
            //                {
            //                    throw new NotImplementedException();
            //                }
            //            }
            //            return V;
            //        //case ABS_Layer.LayerType.Perforated_Modal:
            //        //case ABS_Layer.LayerType.Slotted_Modal:
            //        //Todo: work out TMM perforated panels.
            //        //double R, S, s;
            //        //R = width / 2;
            //        //int nx = 0;
            //        //for (; ; )
            //        //{
            //        //    nx++;
            //        //    if (171.5 * Math.Sqrt(nx * nx / (pitch * pitch)) > 16000) break;
            //        //}
            //        //int m_min, m_max, n_min, n_max;

            //        //if (Layer_i.T == ABS_Layer.LayerType.Perforated_Modal)
            //        //{
            //        //    m_min = 0; m_max = nx; n_min = -nx; n_max = nx;
            //        //    S = Math.PI * R * R;
            //        //    s = S / (pitch * pitch);
            //        //}
            //        //else
            //        //{
            //        //    m_min = 0; m_max = 0; n_min = -nx; n_max = nx;
            //        //    S = 2 * R * pitch;
            //        //    s = 2 * R / pitch;
            //        //}

            //        //double vpm;
            //        //porosity = (LayerList[layer_no - 1] as ABS_Layer).porosity;
            //        //double Eta = 0.48 * Math.Sqrt(S) * (1 - 1.14 * Math.Sqrt(s));

            //        //if (layer_no < 1)
            //        //{
            //        //    Complex[][] Z = new Complex[37][];
            //        //    for (int i = 0; i < 37; i++)
            //        //    {
            //        //        Z[i] = new Complex[freq.Length];
            //        //        for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //    }
            //        //    return Z;
            //        //}
            //        //else
            //        //{
            //        //    Complex[][] Z = new Complex[37][];
            //        //    for (int j = 0; j < 37; j++) Z[j] = new Complex[freq.Length];

            //        //    Parallel.For(m_min, m_max + 1, m =>
            //        //    //for (int m = m_min; m <= m_max; m++)
            //        //    {
            //        //        for (int n = n_min; n <= n_max; n++)
            //        //        {
            //        //            Complex[][] Zmn;

            //        //            if (m == 0 && n == 0)
            //        //            {
            //        //                Zmn = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //                for (int j = 0; j < Zmn.Length; j++)
            //        //                {
            //        //                    for (int i = 0; i < freq.Length; i++)
            //        //                    {
            //        //                        Z[j][i] += s / porosity * Zmn[j][i];
            //        //                    }
            //        //                }
            //        //                continue;
            //        //            }

            //        //            if (m == 0 || n == 0) vpm = .5; else vpm = 1;
            //        //            double M_TERM = Utilities.Numerics.PiX2 * m / pitch;
            //        //            double N_TERM = Utilities.Numerics.PiX2 * n / pitch;
            //        //            Zmn = Layer_postperf(layer_no - 1, LayerList, J, I_inv, K_Air, N_TERM, M_TERM, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);

            //        //            for (int j = 0; j < Zmn.Length; j++)
            //        //            {
            //        //                for (int i = 0; i < freq.Length; i++)
            //        //                {
            //        //                    Z[j][i] += vpm * Zmn[j][i] * Complex.Pow(Utilities.Numerics.jBessel(1, 2 * Math.PI * R * Complex.Sqrt((m * m / (pitch * pitch)) + Complex.Pow(n / pitch - K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2))) / (m * m + Complex.Pow(n - pitch * K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2)), 2);
            //        //                }
            //        //            }
            //        //        }
            //        //    });

            //        //    for (int j = 0; j < Z.Length; j++)
            //        //    {
            //        //        for (int i = 0; i < freq.Length; i++)
            //        //        {
            //        //            Z[j][i] *= 2 / (Math.PI * porosity);
            //        //            Z[j][i] += new Complex(0, (Eta + depth) * 1.2 * freq[i] * Utilities.Numerics.PiX2);
            //        //            Z[j][i] /= (Math.PI * R * R / (pitch * pitch));
            //        //        }
            //        //    }
            //        //    return Z;
            //        //}
            //        //case ABS_Layer.LayerType.CircularPerforations:
            //        //case ABS_Layer.LayerType.SquarePerforations:
            //        //double w = Layer_i.width;
            //        //double p = Layer_i.pitch;
            //        //double d = Layer_i.depth;
            //        //if (Layer_i.T == ABS_Layer.LayerType.CircularPerforations)
            //        //{
            //        //    porosity = Math.PI * width * width * .25 / (pitch * pitch);
            //        //}
            //        //else
            //        //{
            //        //    porosity = width * width / (pitch * pitch);
            //        //}

            //        //if (layer_no < 1)
            //        //{
            //        //    Complex[][] Z = new Complex[37][];
            //        //    for (int i = 0; i < 37; i++)
            //        //    {
            //        //        Z[i] = new Complex[freq.Length];
            //        //        for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //    }
            //        //    return Z;
            //        //}
            //        //else
            //        //{
            //        //    Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //    Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.PerforatedPlate_Impedance(1.2, porosity, w, p, d, freq);

            //        //    for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //    {
            //        //        for (int i = 0; i < sintheta_inc.Length; i++)
            //        //        {
            //        //            Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //        }
            //        //    }
            //        //    return Z;
            //        //}
            //        //case ABS_Layer.LayerType.Slots:
            //        //    if (layer_no < 1)
            //        //    {
            //        //        Complex[][] Z = new Complex[37][];
            //        //        for (int i = 0; i < 37; i++)
            //        //        {
            //        //            Z[i] = new Complex[freq.Length];
            //        //            for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //        }
            //        //        return Z;
            //        //    }
            //        //    else
            //        //    {
            //        //        Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //        Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.SlottedPlate_Impedance(1.2, c_sound, width, pitch, depth, freq);

            //        //        for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //        {
            //        //            for (int i = 0; i < sintheta_inc.Length; i++)
            //        //            {
            //        //                Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //            }
            //        //        }
            //        //        return Z;
            //        //    }
            //        //case ABS_Layer.LayerType.Microslit:
            //        //    if (layer_no < 1)
            //        //    {
            //        //        Complex[][] Z = new Complex[37][];
            //        //        for (int i = 0; i < 37; i++)
            //        //        {
            //        //            Z[i] = new Complex[freq.Length];
            //        //            for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //        }
            //        //        return Z;
            //        //    }
            //        //    else
            //        //    {
            //        //        Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //        Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.MicroSlots_Impedance(1.2, c_sound, width, pitch, depth, freq);

            //        //        for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //        {
            //        //            for (int i = 0; i < sintheta_inc.Length; i++)
            //        //            {
            //        //                Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //            }
            //        //        }
            //        //        return Z;
            //        //    }
            //        //case ABS_Layer.LayerType.MicroPerforated:
            //        //    if (Layer_i.T == ABS_Layer.LayerType.CircularPerforations)
            //        //    {
            //        //        porosity = Math.PI * width * width * .25 / (pitch * pitch);
            //        //    }
            //        //    else
            //        //    {
            //        //        porosity = width * width / (pitch * pitch);
            //        //    }

            //        //    if (layer_no < 1)
            //        //    {
            //        //        Complex[][] Z = new Complex[37][];
            //        //        for (int i = 0; i < 37; i++)
            //        //        {
            //        //            Z[i] = new Complex[freq.Length];
            //        //            for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
            //        //        }
            //        //        return Z;
            //        //    }
            //        //    else
            //        //    {
            //        //        Complex[][] Z = Layer(layer_no - 1, LayerList, J, I_inv, K_Air, sintheta_inc, K_Air, c_sound, freq);//, ref D, ref T);
            //        //        Complex[] Zmpa = AbsorptionModels.Perforated_Membranes.MicroPerforatedPlate_Impedance(1.2, c_sound, width, pitch, depth, freq);

            //        //        for (int j = 0; j < sintheta_inc[0].Length; j++)
            //        //        {
            //        //            for (int i = 0; i < sintheta_inc.Length; i++)
            //        //            {
            //        //                Z[i][j] += Zmpa[j] * (1 - sintheta_inc[i][j] * sintheta_inc[i][j]);
            //        //            }
            //        //        }
            //        //        return Z;
            //        //    }
            //        default:
            //            throw new NotImplementedException();
            //    }
            //}

            //Set up transfer function after perforated materials as a recursive function...
            private static Complex[][] Layer_postperf(int layer_no, List<ABS_Layer> LayerList, SparseMatrix[] J, SparseMatrix[] I_inv, Complex[] K0, double N_Term, double M_Term, Complex[][] sintheta_inc, Complex[] K_Air, double c_sound, double[] freq)//, ref double[] D_Lat, ref double[] T)
            {
                ABS_Layer Layer_i = LayerList[layer_no] as ABS_Layer;
                double depth = Layer_i.depth;
                Complex[][] sintheta_trans = new Complex[sintheta_inc.Length][];
                Complex[][] Kmn = new Complex[sintheta_inc.Length][];
                //Complex[][] Kxi = new Complex[sintheta_trans.Length][];

                switch (Layer_i.T)
                {
                    case ABS_Layer.LayerType.AirSpace:
                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K_Air[j];
                                Kmn[i][j] = Complex.Sqrt(K_Air[j] * K_Air[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        Complex[] Zc_Air = Operations.Air_CharImpedance(1.2, c_sound, freq);

                        if (layer_no > 0)
                        {
                            Complex[][] Z = Layer_postperf(layer_no - 1, LayerList, J, I_inv, K_Air, M_Term, N_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc_Air, Z, K_Air, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc_Air, K_Air, Kmn, depth);
                        }
                    case ABS_Layer.LayerType.PorousDB:
                        Complex[] K = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Complex[] Zc = AbsorptionModels.Equivalent_Fluids.DelaneyBazley_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
                                Kmn[i][j] = Complex.Sqrt(K[j] * K[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = Layer_postperf(layer_no - 1, LayerList, J, I_inv, K, M_Term, N_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kmn, depth);
                        }
                    //K[i] = AbsorptionModels.Porous_Absorbers.DelaneyBazley_WNumber(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Zc[i] = AbsorptionModels.Porous_Absorbers.DelaneyBazley_Impedance(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Screens[i] = i > 0 ? Screens[i - 1] : 0;
                    case ABS_Layer.LayerType.PorousCA:
                        K = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Zc = AbsorptionModels.Equivalent_Fluids.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                //sintheta_trans[i][j] = K0[j] * sintheta_inc[i][j] / K[j];
                                Kmn[i][j] = Complex.Sqrt(K[j] * K[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = Layer_postperf(layer_no - 1, LayerList, J, I_inv, K, N_Term, M_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kmn, depth);
                        }

                    //K[i] = AbsorptionModels.Porous_Absorbers.DBAllardChampoux_WNumber(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Zc[i] = AbsorptionModels.Porous_Absorbers.DBAllardChampoux_Impedance(1.2, c_sound, Layer_i.Flow_Resist, 40, 11314);
                    //Screens[i] = i > 0 ? Screens[i - 1] : 0;
                    case ABS_Layer.LayerType.PorousM:
                        K = AbsorptionModels.Equivalent_Fluids.DB_Miki_WNumber(1.2, c_sound, Layer_i.Flow_Resist, freq);
                        Zc = AbsorptionModels.Equivalent_Fluids.DB_Miki_Impedance(1.2, c_sound, Layer_i.Flow_Resist, freq);

                        for (int i = 0; i < sintheta_inc.Length; i++)
                        {
                            Kmn[i] = new Complex[freq.Length];
                            sintheta_trans[i] = new Complex[freq.Length];
                            for (int j = 0; j < sintheta_inc[i].Length; j++)
                            {
                                Kmn[i][j] = Complex.Sqrt(K[j] * K[j] - M_Term * M_Term - Complex.Pow(N_Term - K0[j] * sintheta_inc[i][j] * sintheta_inc[i][j], 2));
                            }
                        }

                        if (layer_no > 0)
                        {
                            Complex[][] Z = Layer_postperf(layer_no - 1, LayerList, J, I_inv, K, N_Term, M_Term, sintheta_trans, K_Air, c_sound, freq);//, ref D_Lat, ref T);
                            return AbsorptionModels.Operations.Transfer(Zc, Z, K, Kmn, depth);
                        }
                        else
                        {
                            return AbsorptionModels.Operations.Rigid_Backed(Zc, K, Kmn, depth);
                        }
                    case ABS_Layer.LayerType.Perforated_Modal:
                    case ABS_Layer.LayerType.Slotted_Modal:
                        ABS_Layer AL = (LayerList[layer_no] as ABS_Layer);
                        double D, R, S, s;
                        D = AL.pitch;
                        R = AL.width / 2;
                        int nx = 0;
                        for (;;)
                        {
                            nx++;
                            if (171.5 * Math.Sqrt(nx * nx / (D * D)) > 16000) break;
                        }
                        int m_min, m_max, n_min, n_max;


                        if (AL.T == ABS_Layer.LayerType.Perforated_Modal)
                        {
                            m_min = 0; m_max = nx; n_min = -nx; n_max = nx;
                            S = Math.PI * R * R;
                            s = S / (D * D);
                        }
                        else
                        {
                            m_min = 0; m_max = 0; n_min = -nx; n_max = nx;
                            S = 2 * R * D;
                            s = 2 * R / D;
                        }

                        double vpm;
                        double porosity = (LayerList[layer_no - 1] as ABS_Layer).porosity;
                        double Eta = 0.48 * Math.Sqrt(S) * (1 - 1.14 * Math.Sqrt(s));

                        if (layer_no < 1)
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int i = 0; i < sintheta_inc.Length; i++)
                            {
                                Z[i] = new Complex[freq.Length];
                                for (int j = 0; j < freq.Length; j++) Z[i][j] = 0;
                            }
                            return Z;
                        }
                        else
                        {
                            Complex[][] Z = new Complex[sintheta_inc.Length][];
                            for (int j = 0; j < sintheta_inc.Length; j++) Z[j] = new Complex[freq.Length];

                            for (int m = m_min; m <= m_max; m++)
                            {
                                if (m == 0) vpm = .5; else vpm = 1;
                                for (int n = n_min; n <= n_max; n++)
                                {
                                    Complex[][] rootsubKmn = new Complex[sintheta_inc.Length][];
                                    double M_TERM = Utilities.Numerics.PiX2 * m / D;
                                    double N_TERM = Utilities.Numerics.PiX2 * n / D;
                                    Complex[][] Zmn = Layer_postperf(layer_no - 1, LayerList, J, I_inv, K_Air, N_TERM, M_TERM, sintheta_inc, K_Air, c_sound, freq);//, ref D_Lat, ref T);

                                    for (int j = 0; j < sintheta_inc.Length; j++)
                                    {
                                        for (int i = 0; i < sintheta_inc[j].Length; i++)
                                        {
                                            Z[j][i] += vpm * Zmn[j][i] * Complex.Pow(Utilities.Numerics.jBessel(1, 2 * Math.PI * R * ((m * m / (D * D)) + Complex.Pow(n / D - K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2))) / (m * m + Complex.Pow(n - D * K_Air[i] * sintheta_inc[j][i] / Utilities.Numerics.PiX2, 2)), 2);
                                        }
                                    }
                                }
                            }

                            for (int j = 0; j < sintheta_inc.Length; j++)
                            {
                                for (int i = 0; i < sintheta_inc[j].Length; i++)
                                {
                                    Z[j][i] *= 2 / (Math.PI * porosity);
                                    Z[j][i] += new Complex(0, (Eta + depth) * 1.2 * i * Utilities.Numerics.PiX2);
                                }
                            }
                            return Z;
                        }
                    case ABS_Layer.LayerType.Microslit:
                        return null;
                    case ABS_Layer.LayerType.MicroPerforated:
                        return null;
                    default:
                        throw new Exception("Invalid layer choice");
                }
            }
        }

        public static class Solids
        {
            public static double Lame_Lambda(double ModulusElasticity, double PoissonRatio)
            {
                return ModulusElasticity * PoissonRatio / ((1 + PoissonRatio) * (1 - 2 * PoissonRatio));
            }

            public static double Lame_Mu(double ModulusElasticity, double PoissonRatio)
            {
                return ModulusElasticity / (2 * (1 + PoissonRatio));
            }

            public static Complex[] WaveNumber(double[] freq, double BulkModulus_GPa, double density)
            {
                Complex[] K = new Complex[freq.Length];

                for (int f = 0; f < freq.Length; f++)
                {
                    K[f] = (Utilities.Numerics.PiX2 * freq[f]) / Math.Sqrt(BulkModulus_GPa / density);
                }
                return K;
            }

            public static Complex[] WaveNumber(double[] freq, double Speed_of_Sound)
            {
                Complex[] K = new Complex[freq.Length];

                for (int f = 0; f < freq.Length; f++)
                {
                    K[f] = (Utilities.Numerics.PiX2 * freq[f]) / Speed_of_Sound;
                }
                return K;
            }

            public static Complex[] WaveNumber(double[] freq, double density, Complex Lamel, Complex Lamemu)
            {
                Complex[] K = new Complex[freq.Length];

                for (int f = 0; f < freq.Length; f++)
                {
                    K[f] = (Utilities.Numerics.PiX2 * freq[f]) * (density / Complex.Sqrt(Lamel + 2* Lamemu));
                }
                return K;
            }
        }

        public static class Equivalent_Fluids
        {
            public static Complex Specific_Impedance(double depth, Complex Char_Impedance, Complex Wave_Number)
            {
                Complex GammaT = Wave_Number * Complex.ImaginaryOne * depth;

                return Char_Impedance * Complex.Tanh(GammaT);// *Complex.Cosh(GammaT) / Complex.Sinh(GammaT);
            }

            public static Complex[] Specific_Impedance(double depth, Complex[] Char_Impedance, Complex[] Wave_Number)
            {
                Complex[] Zs = new Complex[Char_Impedance.Length];
                for (int i = 0; i < Char_Impedance.Length; i++)
                {
                    Complex GammaT = Wave_Number[i] * Complex.ImaginaryOne * depth;
                    //Complex e_x = Complex.Exp(GammaT);
                    //Complex _e_X = 1 / e_x;

                    //Zs[i] = Char_Impedance[i] * ((e_x + _e_X) / (e_x - _e_X));
                    Zs[i] = Char_Impedance[i] / Complex.Tanh(GammaT);// * Complex.Cosh(GammaT) / Complex.Sinh(GammaT);
                }
                return Zs;
            }

            public static Complex[] DelaneyBazley_Impedance(double air_density, double c_sound, double airflow_resisitivity, double[] freq)
            {
                Complex[] Zc = new Complex[freq.Length];

                double rho_c = air_density * c_sound;

                for (int i = 0; i < freq.Length; i++)
                {
                    double f_sigmaX1000 = 1000 * freq[i] / airflow_resisitivity;
                    Zc[i] = new Complex(rho_c * (1 + 9.08 * Math.Pow(f_sigmaX1000, -0.75)), rho_c * (-11.9 * Math.Pow(f_sigmaX1000, -0.73)));
                }

                //Valid where f_sigma is greater than 0.01, but less than 1.//
                return Zc;
            }

            public static Complex[] DelaneyBazley_WNumber(double air_density, double c_sound, double airflow_resisitivity, double[] freq)
            {
                Complex[] K = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    double f_sigmaX1000 = 1000 * freq[i] / airflow_resisitivity;
                    K[i] = new Complex(Utilities.Numerics.PiX2 * freq[i] / c_sound * (1 + 10.08 * Math.Pow(f_sigmaX1000, -0.7)), Utilities.Numerics.PiX2 * freq[i] / c_sound * (-10.3 * Math.Pow(f_sigmaX1000, -0.59)));
                }

                //Valid where f_sigma is greater than 0.01, but less than 1.//
                return K;
            }

            public static Complex[] DBAllardChampoux_Impedance(double air_density, double c_sound, double airflow_resisitivity, double[] freq)
            {
                Complex[] Zc = new Complex[freq.Length];

                double rho_c = air_density * c_sound;

                for (int i = 0; i < freq.Length; i++)
                {
                    double rhof_sigma = air_density * freq[i] / airflow_resisitivity;
                    Zc[i] = new Complex(rho_c * (1 + 0.0571 * Math.Pow(rhof_sigma, -0.754)), rho_c * (-0.0871 * Math.Pow(rhof_sigma, -0.732)));
                }

                //Valid where f_sigma is greater than 0.01, but less than 1.//
                return Zc;
            }

            public static Complex[] DBAllardChampoux_WNumber(double air_density, double c_sound, double airflow_resisitivity, double[] freq)
            {
                Complex[] K = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    double rhof_sigma = air_density * freq[i] / airflow_resisitivity;
                    K[i] = new Complex(Utilities.Numerics.PiX2 * freq[i] / c_sound * (1 + 0.0978 * Math.Pow(rhof_sigma, -0.7)), Utilities.Numerics.PiX2 * freq[i] / c_sound * (-0.189 * Math.Pow(rhof_sigma, -0.595)));
                }

                //Valid where f_sigma is greater than 0.01, but less than 1.//
                return K;
            }

            public static Complex[] DB_Miki_Impedance(double air_density, double c_sound, double airflow_resisitivity, double[] freq)
            {
                Complex[] Zc = new Complex[freq.Length];

                double rho_c = air_density * c_sound;

                for (int i = 0; i < freq.Length; i++)
                {
                    double f_sigmaX1000 = 1000 * freq[i] / airflow_resisitivity;
                    Zc[i] = new Complex(rho_c * (1 + 5.5 * Math.Pow(f_sigmaX1000, -0.632)), rho_c * (-8.43 * Math.Pow(f_sigmaX1000, -0.632)));
                }

                //Valid where f_sigma is greater than 0.01, but less than 1.//
                return Zc;
            }

            public static Complex[] DB_Miki_WNumber(double air_density, double c_sound, double airflow_resisitivity, double[] freq)
            {
                Complex[] K = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    double f_sigmaX1000 = 1000 * freq[i] / airflow_resisitivity;
                    K[i] = new Complex(Utilities.Numerics.PiX2 * freq[i] / c_sound * (1 + 7.81 * Math.Pow(f_sigmaX1000, -0.618)), Utilities.Numerics.PiX2 * freq[i] / c_sound * (-11.41 * Math.Pow(f_sigmaX1000, -0.618)));
                }
                //Valid where f_sigma is greater than 0.01, but less than 1.//
                return K;
            }
        }

        public static class Biot_Porous_Absorbers
        {
            public static double Shear_Viscosity = 1.84E-5;///kg / m s, for standard conditions in air, Atalla, 2009
            public static double Thermal_Conductivity = 2.6E-1;// w / m k, for standard conditions in air, Atalla, 2009
            public static double Prandtl_no = 0.707;
            public static double Ratio_of_SpecificHeats_Air = 1.4;//Ratio of specific heats (Cp / Cv)

            public static Complex[] WaveNumber_Fluid(double density, double porosity, double ThermalPermeability, double[] freq)
            {
                Complex[] k = new Complex[freq.Length];
                for (int f = 0; f < freq.Length; f++)
                {
                    Complex Kf = Biot_Porous_Absorbers.BulkMod_Fluid(Utilities.Numerics.PiX2 * freq[f], 101325, porosity, ThermalPermeability);
                    k[f] = Utilities.Numerics.PiX2 * freq[f] / Complex.Sqrt(Kf / density);
                }
                return k;
            }

            public static double Tortuosity(double porosity, double flow_resistivity_material, double flow_resistivity_air)
            {
                return porosity * flow_resistivity_material / flow_resistivity_air;
            }

            /// <summary>
            /// Viscous characteristic length for homogeneous materials (like melamine foam).
            /// Identical to Thermal Characteristic Length.
            /// </summary>
            /// <param name="Tortuosity"></param>
            /// <param name="flow_resistivity"></param>
            /// <param name="porosity"></param>
            /// <returns></returns>
            public static double Viscous_Characteristic_Length(double Tortuosity, double flow_resistivity, double porosity)
            {
                return Math.Sqrt(8 * Shear_Viscosity * Tortuosity / (flow_resistivity * porosity)); // * 1/c;
            }

            /// <summary>
            /// Viscous characteristic length for heterogeneous fibrous materials.
            /// </summary>
            /// <param name="FibreLength"></param>
            /// <param name="FibreRadius"></param>
            /// <returns></returns>
            public static double Viscous_Characteristic_Length(double FibreLength, double FibreRadius)
            {
                return 1 / (Utilities.Numerics.PiX2 * FibreLength * FibreRadius);
            }

            public static double Shear_Modulus(double YoungsModulus, double PoissonRatio)
            {
                return YoungsModulus / (2 * (1 + PoissonRatio));
            }

            /// <summary>
            /// Thermal characteristic length for heterogeneous materials.
            /// </summary>
            /// <param name="Viscous_Characteristic_Length"></param>
            /// <returns></returns>
            public static double Thermal_Characteristic_Length(double Viscous_Characteristic_Length)
            {
                return 2 * Viscous_Characteristic_Length;
            }

            public static Complex Dynamic_Tortuosity_Pride_LaFarge(double w, double v, double porosity, double Tortuosity, double Static_Tortuosity, double Viscous_Permeability, double Viscous_Length)
            {
                Complex b = 2 * Viscous_Permeability * Tortuosity * Tortuosity / (porosity * Viscous_Length * Viscous_Length * (Static_Tortuosity - Tortuosity));
                Complex term1 = (2 * Tortuosity * Viscous_Permeability / (b * porosity * Viscous_Length));
                return ((v * porosity) / Complex.ImaginaryOne * w * Viscous_Permeability) * (1 - b + b * (1 + term1 * term1 * Complex.ImaginaryOne * w / v));
            }

            public static Complex Dynamic_Tortuosity_Champoux_Allard(double w, double Thermal_Length)
            {
                double v_prime = vprime();
                return 8 * v_prime / (Complex.ImaginaryOne * w * Thermal_Length * Thermal_Length) * Complex.Sqrt(1 + Thermal_Length * Thermal_Length / 16 * Complex.ImaginaryOne * w / v_prime) + 1;
            }

            public static Complex Dynamic_Tortuosity_Johnson(double w, double v, double porosity, double Tortuosity, double Viscous_Permeability, double Viscous_Length)
            {
                Complex term1 = (2 * Tortuosity * Viscous_Permeability / (porosity * Viscous_Length));
                return (v * porosity) / (Complex.ImaginaryOne * w * Viscous_Permeability) * (1 + (1 + term1 * term1 * Complex.ImaginaryOne * w / v)) + Tortuosity;
            }

            public static Complex BulkMod_Fluid(Complex characteristic_Z, double density)
            {
                return characteristic_Z * characteristic_Z / density;
            }

            public static double BulkMod_Solid(double Youngs_Modulus, double Poissons_Ratio)
            {
                return Youngs_Modulus / (3 * (1 - 2 * Poissons_Ratio));
            }

            public static Complex BulkMod_Fluid(double w, double AmbientMeanPressure, double porosity, double Viscous_Length)
            {
                double Thermal_Length = Thermal_Characteristic_Length(Viscous_Length);
                double vp = vprime();
                double q0p = porosity * Thermal_Length * Thermal_Length / 8;
                //Complex awp = vp * porosity / Complex.ImaginaryOne * Prandtl_no * w * 1.2 * Thermal_Permeability_0 / (porosity * Shear_Viscosity);
                Complex awp = 8 * vp / (Complex.ImaginaryOne * w * Thermal_Length * Thermal_Length) * Complex.Sqrt(1 + (Thermal_Length * Thermal_Length / 16) * Complex.ImaginaryOne * 2 / vp) + 1;
                //return AmbientMeanPressure * (1 - ((Ratio_of_SpecificHeats_Air - 1) / (Ratio_of_SpecificHeats_Air * Complex.ImaginaryOne * Prandtl_no * w * 1.2 * Thermal_Permeability_0 / (porosity * Shear_Viscosity));
                return AmbientMeanPressure / (1 - ((Ratio_of_SpecificHeats_Air - 1) / (Ratio_of_SpecificHeats_Air * awp)));
            }

            public static double v()
            {
                return Shear_Viscosity / 1.2;
            }

            public static double vprime()
            {
                return Shear_Viscosity / 1.2 / Prandtl_no;
            }

            //public static double vprime(double v, double Prandtl_no)
            //{
            //    return v / (Prandtl_no * Prandtl_no);
            //}

            /// <summary>
            /// Variable q0 in Allard & Atalla.
            /// </summary>
            /// <param name="Flow_Resistivity"></param>
            /// <returns></returns>
            public static double Viscous_Permeability(double Flow_Resistivity)
            {
                return Shear_Viscosity / Flow_Resistivity;
            }

            /// <summary>
            /// The Johnson G parameter
            /// </summary>
            /// <param name="tortuosity"></param>
            /// <param name="porosity"></param>
            /// <param name="Viscous_Permeability"></param>
            /// <param name="Length_Viscous"></param>
            /// <param name="freq"></param>
            /// <param name="v"></param>
            /// <returns></returns>
            public static Complex G_w(double tortuosity, double porosity, double Viscous_Permeability, double Length_Viscous, double w)
            {
                //Johnson
                Complex term = (2 * tortuosity * Viscous_Permeability / (porosity * Length_Viscous));
                return Complex.Sqrt(1 + term * term * Complex.ImaginaryOne * w / v());
            }

            /// <summary>
            /// The Champoux-Allard G Parameter
            /// </summary>
            /// <param name="tortuosity"></param>
            /// <param name="porosity"></param>
            /// <param name="Thermal_Permeability"></param>
            /// <param name="Length_Thermal"></param>
            /// <param name="freq"></param>
            /// <param name="vp"></param>
            /// <returns></returns>
            public static Complex G_w_prime(double porosity, double Thermal_Permeability, double Length_Thermal, double w)
            {
                //Champoux - Allard
                Complex term = (2 * Thermal_Permeability / (porosity * Length_Thermal));
                return Complex.Sqrt(1 + term * term * Complex.ImaginaryOne * w / vprime());
            }

            /// <summary>
            /// The Pride G parameter
            /// </summary>
            /// <param name="b"></param>
            /// <param name="tortuosity"></param>
            /// <param name="Viscous_Permeability"></param>
            /// <param name="porosity"></param>
            /// <param name="Length_Viscous"></param>
            /// <param name="freq"></param>
            /// <param name="v"></param>
            /// <returns></returns>
            public static Complex Gp_w(Complex b, double tortuosity, double Viscous_Permeability, double porosity, double Length_Viscous, double w)
            {
                //Pride
                Complex term = 2 * tortuosity * Viscous_Permeability / (b * porosity * Length_Viscous);
                return 1 - b + b * Complex.Sqrt(1 + term * term * Complex.ImaginaryOne * w / v());
            }

            /// <summary>
            /// The LaFarge G parameter
            /// </summary>
            /// <param name="b"></param>
            /// <param name="Viscous_Permeability"></param>
            /// <param name="porosity"></param>
            /// <param name="Length_Viscous"></param>
            /// <param name="freq"></param>
            /// <param name="v"></param>
            /// <returns></returns>
            public static Complex Gp_w_prime(Complex bp, double Thermal_Permeability, double porosity, double Length_Thermal, double w)
            {
                //LaFarge
                Complex term = 2 * Thermal_Permeability / (bp * porosity * Length_Thermal);
                return 1 - bp + bp * Complex.Sqrt(1 + term * term * Complex.ImaginaryOne * w / vprime());
            }

            public static double rhoA(double porosity, double tortuosity)
            {
                return porosity * 1.2 * (tortuosity - 1);
            }

            public static double rhoA(double rho12)
            {
                return -rho12;
            }

            public static Complex rho11eff(double framedensity, double rhoa, double porosity, double flow_resistivity, Complex Gw, double w)
            {
                return framedensity + rhoa - Complex.ImaginaryOne * flow_resistivity * porosity * porosity * Gw / w;
            }

            public static Complex rho12eff(double rhoa, double porosity, double flow_resistivity, Complex Gw, double w)
            {
                return -rhoa + Complex.ImaginaryOne * flow_resistivity * porosity * porosity * Gw / w;
            }

            public static Complex rho22eff(double rhoa, double porosity, double flow_resistivity, Complex Gw, double w)
            {
                return porosity * 1.2 + rhoa - Complex.ImaginaryOne * flow_resistivity * porosity * porosity * Gw / w;
            }

            //public static void rho11eff(double framedensity, double rhoa, double porosity, double flow_resistivity, Complex Gw, double freq, out Complex rho11eff, out Complex rho12eff, out Complex rho22eff)
            //{
            //    Complex jspgw_w = Complex.ImaginaryOne * porosity * porosity * Gw / (Utilities.Numerics.PiX2 * freq);
            //    rho11eff = framedensity * rhoa - jspgw_w;
            //    rho12eff = -rhoa + jspgw_w;
            //    rho22eff = porosity * 1.2 + rhoa - jspgw_w;
            //}
            /// <summary>
            /// densities related to the geometry of the frame. (Atalla, 2009)
            /// </summary>
            /// <param name="density">Actual density of the material (i.e. glass fiber => 45 kg/m^3)</param>
            /// <returns></returns>
            public static double rho11(double density, double rho12)
            {
                return density - rho12;
            }

            /// <summary>
            /// densities related to the geometry of the frame. (Atalla, 2009)
            /// </summary>
            /// <param name="porosity">ratio of volume of air to volume of frame.</param>
            /// <param name="tortuosity">Classical tortuosity term.</param>
            /// <returns></returns>
            public static double rho12(double porosity, double tortuosity)
            {
                return -porosity * 1.2 * (tortuosity - 1);
            }

            /// <summary>
            /// densities related to the geometry of the frame. (Atalla, 2009)
            /// </summary>
            /// <param name="porosity">ratio of volume of air to volume of frame.</param>
            /// <param name="rho12">See rho12 method.</param>
            /// <returns></returns>
            public static double rho22(double porosity, double rho12)
            {
                return porosity * 1.2 - rho12;
            }
        }

        public static class Perforated_Membranes
        {
            static double nu = 1.789e-5;//Coefficient of viscosity of air in kg/ms
            static double kv = 0.000019;//Kinematic Viscosity in sq.m./s

            public static Complex[][] SquareGrid_CircPerfs_Impedance(double air_density, double c_sound, double diameter, double depth, double period, int f_lower, int f_upper)
            {
                double radius = diameter * .5;
                double s_ww = Math.Sqrt(air_density * radius * radius / nu);
                double porosity = Math.PI * radius * radius / (period * period);

                Complex[][] Z = new Complex[19][];

                for (int a = 0; a < 90 / 5 + 1; a++)
                {
                    Z[a] = new Complex[f_upper - f_lower + 1];
                    for (int f = f_lower, i = 0; f <= f_upper; f++, i++)
                    {
                        double w = Utilities.Numerics.PiX2 * f;
                        double s = s_ww * Math.Sqrt(w);
                        double k0 = w / c_sound;
                        double angle = a * 5 * Utilities.Numerics.Pi_180;
                        Complex eta = 0;

                        //sum over all modes
                        for (int n = -10; n < 10; n++)
                        {
                            Complex kmn = -Utilities.Numerics.PiX2 * n / period - k0 * Math.Sin(angle) * Complex.ImaginaryOne;

                            //m = 0
                            Complex J1 = Utilities.Numerics.jBessel(1, Utilities.Numerics.PiX2 * radius * (n / period - k0 * Math.Sin(angle) / Utilities.Numerics.PiX2));
                            Complex add = J1 * J1 / (n - k0 * period * Math.Sin(angle) / Utilities.Numerics.PiX2);

                            if (n == 0) continue;
                            eta += add;
                            //Z[a][f - f_lower] += Zmn * add;
                        }

                        //apply v'm at m = 0
                        eta *= .5;
                        Z[a][f - f_lower] *= .5;

                        //all other values of m...
                        for (int m = 1; m < 10; m++)
                            for (int n = -10; n < 10; n++)
                            {
                                Complex kmn = Utilities.Numerics.PiX2 * n / period - k0 * Math.Sin(angle);
                                kmn *= kmn;
                                kmn += 4 * Math.PI * Math.PI * m * m / (period * period);
                                kmn = -Complex.Sqrt(kmn) * Complex.ImaginaryOne;

                                double t = (n / period - k0 * Math.Sin(angle) / Utilities.Numerics.PiX2);
                                Complex J1 = Utilities.Numerics.jBessel(1, Utilities.Numerics.PiX2 * radius * Math.Sqrt(m * m / (period * period) + t * t));
                                Complex denom = (n - k0 * period * Math.Sin(angle) / Utilities.Numerics.PiX2);
                                Complex add = J1 * J1 / (m * m + denom * denom);
                                eta += add;
                                //Z[a][f - f_lower] += Zmn * add;
                            }
                        //v'm = 1. No application needed.
                        //Complete all sums
                        eta *= period / (Math.PI * Math.PI);
                        Z[a][f - f_lower] *= 2 / (Math.PI * porosity);

                        Z[a][f - f_lower] += (eta + depth) * air_density * w * Complex.ImaginaryOne;
                        Z[a][f - f_lower] /= s;
                    }
                }

                return Z;
            }

            public static Complex[] PerforatedPlate_Impedance(double air_density, double porosity, double diameter, double pitch, double thickness, double[] freq)
            {
                Complex[] Zp = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    double theta = 1 - 1.47 * Math.Sqrt(porosity) + 0.47 * Math.Pow(porosity, 1.5);
                    double viscous_boundary_layer = 0.85 * diameter * theta;
                    double angular_frequency = Utilities.Numerics.PiX2 * freq[i];
                    Zp[i] = new Complex((air_density / porosity) * Math.Sqrt(8 * kv * angular_frequency) * (1 + thickness / diameter), (angular_frequency * air_density / porosity) * (Math.Sqrt(8 * kv / porosity) * (1 + thickness / diameter) + thickness + viscous_boundary_layer));
                }
                return Zp;
            }

            public static Complex[] MicroPerforatedPlate_Impedance(double air_density, double c_sound, double diameter, double pitch, double thickness, double[] freq)
            {
                Complex rtnj = Complex.Sqrt(new Complex(0, -1));
                double radius = diameter / 2;
                double porosity = Math.PI * radius * radius / (pitch * pitch);
                Complex[] Zmp = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    double w = Utilities.Numerics.PiX2 * freq[i];
                    Complex k_rtnj = Utilities.Numerics.PiX2 * freq[i] * rtnj / c_sound;
                    Complex k_prime_rtnj = radius * Math.Sqrt(air_density * w / nu);
                    Complex B0 = Utilities.Numerics.jBessel(0, k_prime_rtnj);
                    Complex B1 = Utilities.Numerics.jBessel(1, k_prime_rtnj);

                    Complex Zm = new Complex(0, w * air_density * thickness) * ((k_prime_rtnj * B0) / (1 - 2 * B1));
                    Complex end_correction = new Complex(0, 1.7 * w * air_density * radius / porosity);
                    double rad_reactance = Math.Sqrt(air_density * w * nu / porosity);
                    Zmp[i] = Zm / porosity + rad_reactance + end_correction;
                }

                return Zmp;
            }

            public static Complex[] MicroSlots_Impedance(double air_density, double c_sound, double width, double pitch, double thickness, double[] freq)
            {
                double porosity = width / pitch;
                Complex[] Zms = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    double w = Utilities.Numerics.PiX2 * freq[i];
                    Complex kp = Complex.Sqrt(new Complex(0, air_density * w / nu));
                    Complex rho_e = air_density / (1 - 2 * Complex.Tan(kp * width * .5) / (kp * width));
                    Zms[i] = (width * rho_e * thickness / porosity - 2 * air_density * w * thickness * Math.Log10(Math.Sin(Math.PI * porosity / 2))) * Complex.ImaginaryOne;
                }

                return Zms;
            }

            public static Complex[] SlottedPlate_Impedance(double air_density, double c_sound, double width, double pitch, double depth, double[] freq)
            {
                double porosity = width / pitch;
                Complex[] Zp = new Complex[freq.Length];

                for (int i = 0; i < freq.Length; i++)
                {
                    double angular_frequency = Utilities.Numerics.PiX2 * freq[i];
                    double rmem = Math.Sqrt(2 * air_density * nu * angular_frequency) / (2 * porosity);
                    double corr = (-1 / Math.PI) * Math.Log10(Math.Sin(.5 * Math.PI * porosity));       // slot absorber correction
                    Zp[i] = new Complex(rmem, angular_frequency * (air_density / porosity) * (depth + 2 * corr * width) - air_density * c_sound / Math.Tan(width / c_sound * depth));
                }
                return Zp;
            }

            public static Complex[] MicroPerforatedPlate_Impedance2(double air_density, double c_sound, double width, double pitch, double thickness, double[] freq)
            {
                double radius = width / 2;
                double r2 = radius * radius;
                double porosity = Math.PI * r2 / (pitch * pitch);
                Complex[] Zmp = new Complex[freq.Length];
                for (int i = 0; i < freq.Length; i++)
                {
                    double w = Utilities.Numerics.PiX2 * freq[i];
                    double x = radius * Math.Sqrt(w * air_density / 17.9);//acoustic Reynold's number
                    if (x < 1)
                    {
                        Zmp[i] = new Complex(8 * nu * thickness / r2, 1.3333333333 * w * air_density * thickness);
                    }
                    else if (x < 10)
                    {
                        Zmp[i] = new Complex(8 * nu * thickness / (air_density * c_sound * r2 * porosity) * (Math.Sqrt(1 + x * x / 32) + Math.Sqrt(2) * x * radius / (4 * thickness)), (w * thickness / (porosity * c_sound)) * (1 + 1 / Math.Sqrt(9 + x * x / 2) + AbsorptionModels.Perforated_Membranes.Rayleigh_EndCorrection(radius)));
                    }
                    else
                    {
                        Zmp[i] = new Complex(Math.Sqrt(2 * nu * air_density * thickness / radius), air_density * w * thickness * (1 + Math.Sqrt(2 * nu / (air_density * w)) / radius));
                    }
                }

                return Zmp;
            }
            //shown to be incorrect for inner aperture...
            public static double Rayleigh_EndCorrection(double radius)
            {
                return 16 * radius / (3 * Math.PI);
            }

            //For circular opening/cavity, square opening/cavity, circular opening/square cavity...
            public static double Ingard_EndCorrection(double radius, double l_opening, double l_cavity)
            {
                return (8 * radius / (3 * Math.PI)) * (2 - 1.25 * l_opening / l_cavity);
            }

            //For circular opening/square cavity...
            public static double Allard_EndCorrection(double radius, double l_opening, double l_cavity)
            {
                return (8 * radius / (3 * Math.PI)) * (2 - 1.14 * l_opening / l_cavity);
            }

            //For slits...
            public static double SmithsKosten_EndCorrection(double width, double pitch)
            {
                return -2 * width / Math.PI * Math.Log(Math.Sin(Math.PI * width / (2 * pitch)));
            }
        }
    }
}