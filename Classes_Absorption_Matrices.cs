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

using MathNet.Numerics.LinearAlgebra.Complex;
using System.Numerics;

namespace Pachyderm_Acoustic
{
    namespace AbsorptionModels
    {
        public static class Explicit_TMM
        {
            public static SparseMatrix PorousMatrix(bool Rigid, double d, Complex k, Complex sin_theta, double freq, double porosity, double tortuosity, double YoungsModulus, double PoissonRatio, double Viscous_Characteristic_Length, double flow_resistivity, double FrameDensity, double Thermal_Permeability_0, double AmbientMeanPressure)
            {
                double w = Utilities.Numerics.PiX2 * freq;
                double v = Biot_Porous_Absorbers.v();
                double FrameShear = AbsorptionModels.Biot_Porous_Absorbers.Shear_Modulus(YoungsModulus, PoissonRatio);
                double kb = 2 * FrameShear * (PoissonRatio + 1) / (3 * (1 - 2 * PoissonRatio));
                double BulkMod_Frame = AbsorptionModels.Biot_Porous_Absorbers.BulkMod_Solid(YoungsModulus, PoissonRatio);
                Complex Kf = Biot_Porous_Absorbers.BulkMod_Fluid(w, AmbientMeanPressure, porosity, Thermal_Permeability_0);//AmbientMeanPressure / (1 - (gamma - 1) / (gamma * alpha));
                Complex LameL = YoungsModulus * PoissonRatio / ((1 + PoissonRatio) * (1 - 2 * PoissonRatio));
                Complex LameMu = YoungsModulus / (2 * (1 + PoissonRatio));
                Complex delta21 = w * w * FrameDensity;
                Complex delta22 = w * w * FrameDensity;
                Complex delta23 = delta21 / LameMu;
                delta21 /= (LameL + 2 * LameMu);
                delta22 /= (LameL + 2 * LameMu);

                //Taken from Lauriks, et. al., 1990.
                double rho12 = Biot_Porous_Absorbers.rho12(porosity, tortuosity);
                double rhoa = Biot_Porous_Absorbers.rhoA(rho12);
                double Viscous_Permeability = Biot_Porous_Absorbers.Viscous_Permeability(flow_resistivity);
                Complex Gw = Biot_Porous_Absorbers.G_w(tortuosity, porosity, Viscous_Permeability, Viscous_Characteristic_Length, freq, v);

                //Complex rho12eff = Biot_Porous_Absorbers.rho12eff(rhoa, porosity, flow_resistivity, Gw, freq);
                Complex rho22eff = Biot_Porous_Absorbers.rho22eff(rhoa, porosity, flow_resistivity, Gw, freq);
                Complex rho11eff = Biot_Porous_Absorbers.rho11eff(FrameDensity, rhoa, porosity, flow_resistivity, Gw, freq);

                Complex P, Q, R;

                if (!Rigid)
                {
                    //Universal (Limp) Frame Case:
                    P = ((1 - porosity) * (1 - kb / BulkMod_Frame) * BulkMod_Frame + porosity * BulkMod_Frame * kb / Kf) / (1 - porosity - kb / BulkMod_Frame + porosity * BulkMod_Frame / Kf);
                    Q = (1 - porosity - kb / BulkMod_Frame) * porosity * BulkMod_Frame / (1 - porosity - kb / BulkMod_Frame + porosity * BulkMod_Frame / Kf);
                    R = porosity * porosity * BulkMod_Frame / (1 - porosity - kb / BulkMod_Frame + porosity * BulkMod_Frame / Kf);
                }
                else
                {
                    //Rigid Frame Case:
                    P = 4 * FrameShear / 3 + kb + (porosity * porosity) * Kf / porosity;
                    R = porosity * Kf;
                    Q = Kf * (1 - porosity);
                }

                Complex kt = k * sin_theta;
                Complex k13 = Complex.Sqrt(delta21 - kt * kt);
                Complex k23 = Complex.Sqrt(delta22 - kt * kt);
                Complex k33 = Complex.Sqrt(delta23 - kt * kt);
                Complex Mu1 = Q * delta21 - w * w * rho11eff / (w * w * rho22eff - R * delta21);
                Complex Mu2 = Q * delta22 - w * w * rho11eff / (w * w * rho22eff - R * delta22);
                Complex Mu3 = FrameShear * delta23 - w * w * rho11eff / (w * w * rho22eff);

                SparseMatrix GH = GammaH_P(kt, w, d, FrameShear, P, Q, R, k13, k23, k33, Mu1, Mu2, Mu3);
                SparseMatrix G0T = Gamma0T_P(kt, w, FrameShear, P, Q, R, k13, k23, k33, Mu1, Mu2, Mu3);

                return GH * G0T;
            }

            public static SparseMatrix GammaH_P(Complex kt, double w, double h, double ShearModulus, Complex P, Complex Q, Complex R, Complex k13, Complex k23, Complex k33, Complex Mu1, Complex Mu2, Complex Mu3)
            {
                SparseMatrix M = new SparseMatrix(6, 6);
                h *= -1;

                //double w = Utilities.Numerics.PiX2 * freq;
                //Complex LameL = FrameElasticity * PoissonRatio / ((1 + PoissonRatio) * (1 - 2 * PoissonRatio));
                //Complex LameMu = FrameElasticity / (2 * (1 + PoissonRatio));
                //Complex delta21 = w * w * FrameDensity;
                //Complex delta22 = w * w * FrameDensity;
                //Complex delta23 = delta21 / LameMu;
                // delta21 /= (LameL + 2 * LameMu);
                //delta22 /= (LameL + 2 * LameMu);
                ////Complex k1 = k * Complex.Cos(theta);
                ////Complex k3 = k * Complex.Sin(theta);
                //Complex k13 = Complex.Sqrt(delta21 - kt * kt);
                //Complex k23 = Complex.Sqrt(delta22 - kt * kt);
                //Complex k33 = Complex.Sqrt(delta23 - kt * kt);
                ////Complex Damping1 = LameMu * (k13 * k13 - k0 * k0);
                ////Complex Damping2 = 2 * LameMu * k0;

                //double rho12 = Biot_Porous_Absorbers.rho12(porosity, tortuosity);
                //double rhoa = Biot_Porous_Absorbers.rhoA(rho12);
                //double Viscous_Permeability = Biot_Porous_Absorbers.Viscous_Permeability(flow_resistivity);
                //double v = Biot_Porous_Absorbers.v();
                //Complex Gw = Biot_Porous_Absorbers.G_w(tortuosity, porosity, Viscous_Permeability, characteristic_length, freq, v);
                //Complex rho12eff = Biot_Porous_Absorbers.rho12eff(rhoa, porosity, flow_resistivity, Gw, freq);
                //Complex rho22eff = Biot_Porous_Absorbers.rho22eff(rhoa, porosity, flow_resistivity, Gw, freq);
                //Complex rho11eff = Biot_Porous_Absorbers.rho11eff(FrameDensity, rhoa, porosity, flow_resistivity, Gw, freq);

                //Complex Kf = AmbientMeanPressure / (1 - (gamma - 1) / (gamma * alpha));
                //Complex P = 4 * FrameShear / 3 + kb + (phi_1 * phi_1) * Kf / porosity;
                //Complex R = porosity * Kf;
                //Complex Q = Kf * (1 - porosity);
                //Complex Mu1 = Q * delta21 - w * w * rho11eff / (w * w *rho22eff - R *delta21);
                //Complex Mu2 = Q * delta22 - w * w * rho11eff / (w * w * rho22eff - R * delta22);
                //Complex Mu3 = FrameShear * delta23 - w * w * rho11eff / (w * w * rho22eff);

                Complex D1 = (P + Q * Mu1) * (kt * kt + k13 * k13) - 2 * ShearModulus * kt * kt;
                Complex D2 = (P + Q * Mu2) * (kt * kt + k23 * k23) - 2 * ShearModulus * kt * kt;
                Complex E1 = (R * Mu1 + Q) * (kt * kt + k13 * k13);
                Complex E2 = (R * Mu2 + Q) * (kt * kt + k23 * k23);

                //Complex Mu1 =                         

                //Taken from Lauriks, et. al 1990.
                M[0, 0] = w * kt * Complex.Cos(k13 * h);
                M[1, 0] = -Complex.ImaginaryOne * w * k13 * Complex.Sin(k13 * h);
                M[2, 0] = -Complex.ImaginaryOne * w * k13 * Mu1 * Complex.Sin(k13 * h);
                M[3, 0] = -D1 * Complex.Cos(k13 * h);
                M[4, 0] = 2 * Complex.ImaginaryOne * ShearModulus * kt * k13 * Complex.Sin(k13 * h);
                M[5, 0] = -E1 * Complex.Cos(k13 * h);
                M[0, 1] = -Complex.ImaginaryOne * w * kt * Complex.Sin(k13 * h);
                M[1, 1] = w * k13 * Complex.Cos(k13 * h);
                M[2, 1] = w * k13 * Mu1 * Complex.Cos(k13 * h);
                M[3, 1] = Complex.ImaginaryOne * D1 * Complex.Sin(k13 * h);
                M[4, 1] = -2 * ShearModulus * kt * k13 * Complex.Cos(k13 * h);
                M[5, 1] = Complex.ImaginaryOne * E1 * Complex.Sin(k13 * h);

                M[0, 2] = w * kt * Complex.Cos(k23 * h);
                M[1, 2] = -Complex.ImaginaryOne * k23 * Complex.Sin(k23 * h);
                M[2, 2] = -Complex.ImaginaryOne * k23 * Mu2 * Complex.Sin(k23 * h);
                M[3, 2] = -D2 * Complex.Cos(k23 * h);
                M[4, 2] = 2 * Complex.ImaginaryOne * ShearModulus * kt * k23 * Complex.Sin(k23 * h);
                M[5, 2] = -E2 * Complex.Cos(k23 * h);
                M[0, 3] = -Complex.ImaginaryOne * w * kt * Complex.Sin(k23 * h);
                M[1, 3] = w * k23 * Complex.Cos(k23 * h);
                M[2, 3] = w * k23 * Mu2 * Complex.Cos(k23 * h);
                M[3, 3] = Complex.ImaginaryOne * D2 * Complex.Sin(k23 * h);
                M[4, 3] = -2 * ShearModulus * kt * k23 * Complex.Cos(k23 * h);
                M[5, 3] = Complex.ImaginaryOne * E2 * Complex.Sin(k23 * h);

                M[0, 4] = Complex.ImaginaryOne * w * k33 * Complex.Sin(k33 * h);
                M[1, 4] = w * kt * Complex.Cos(k33 * h);
                M[2, 4] = w * kt * Mu3 * Complex.Cos(k33 * h);
                M[3, 4] = 2 * Complex.ImaginaryOne * ShearModulus * k33 * kt * Complex.Sin(k33 * h);
                M[4, 4] = ShearModulus * (k33 * k33 - kt * kt) * Complex.Cos(k33 * h);
                M[5, 4] = 0;
                M[0, 5] = -w * k33 * Complex.Cos(k33 * h);
                M[1, 5] = -Complex.ImaginaryOne * w * kt * Complex.Sin(k33 * h);
                M[2, 5] = -Complex.ImaginaryOne * w * kt * Mu3 * Complex.Sin(k33 * h);
                M[3, 5] = -2 * ShearModulus * k33 * kt * Complex.Cos(k33 * h);
                M[4, 5] = -Complex.ImaginaryOne * ShearModulus * (k33 * k33 - kt * kt) * Complex.Sin(k33 * h);
                M[5, 5] = 0;

                return M;
            }

            public static SparseMatrix Gamma0T_P(Complex kt, double w, double ShearModulus, Complex P, Complex Q, Complex R, Complex k13, Complex k23, Complex k33, Complex Mu1, Complex Mu2, Complex Mu3)
            {
                SparseMatrix M = new SparseMatrix(6, 6);

                Complex A1 = -(P - 2 * ShearModulus + Mu1 * Q) * kt * kt - 2 * ShearModulus * k13 * k13;
                Complex A2 = -(P - 2 * ShearModulus + Mu2 * Q) * kt * kt - 2 * ShearModulus * k23 * k23;
                Complex B1 = -(Q + R * Mu1) * kt * kt;
                Complex B2 = -(Q + R * Mu2) * kt * kt;
                Complex X = B1 * A2 - B2 * A1;
                Complex Y = X - 2 * ShearModulus * w * w * kt * kt * (B1 - B2);
                Complex Mu21 = Mu2 - Mu1;
                Complex Mu31 = Mu3 - Mu1;

                //  gamma = kt?

                M[0, 0] = -B2 * 2 * ShearModulus * w * kt / (Complex.ImaginaryOne * Y);
                //M[0,1] = 
                //M[0,2] = 
                M[0, 3] = -B2 * (1 / X + 2 * ShearModulus * w * w * kt * kt * (B1 - B2)) / (X * Y);
                //M[0,4] = 
                M[0, 5] = 1 + B2 * ((A1 / X) - (1 - A1 * (B1 - B2) / X) * (2 * ShearModulus * w * w * kt * kt) / Y) / B1;
                //M[1,0] = 
                M[1, 1] = -((1 + Mu1 / Mu21) - 2 * w * w * kt * kt * (1 - Mu31 / Mu21) / (kt * kt)) / (Complex.ImaginaryOne * k13);
                M[1, 2] = 1 / (k13 * Mu21);
                //M[1,3] = 
                M[1, 4] = -w * kt * (1 - (Mu31 / Mu21)) / (k13 * ShearModulus * kt * kt);
                //M[1,5] = 
                M[2, 0] = (B1 * 2 * ShearModulus * w * kt) / (Complex.ImaginaryOne * Y);
                //M[2,1] = 
                //M[2,2] = 
                M[2, 3] = B1 * (1 / X + (B1 - B2) * (2 * ShearModulus * w * w * kt * kt) / (X * Y));
                //M[2,4] = 
                M[2, 5] = -(A1 / X - 2 * ShearModulus * w * w * kt * kt * (1 - A1 * (B1 - B2) / X) / Y);
                //M[3,0] = 
                M[3, 1] = (Mu1 / Mu21 + 2 * (w * w * kt * kt * Mu31) / (kt * kt * Mu21)) / (Complex.ImaginaryOne * k23); ///Shown in paper as simply alpha... alpha2 = k23 in Atalla's work... 
                M[3, 2] = -1 / (Complex.ImaginaryOne * k23 * Mu21);
                //M[3,3] = 
                M[3, 4] = -(w * w * kt * kt * Mu31) / (k23 * ShearModulus * kt * kt * Mu21);
                //M[3,5] =                                                 M[0,0] = 
                //M[0,4] =
                M[4, 1] = -(2 * w * w * kt * kt) / (Complex.ImaginaryOne * kt * kt);
                //M[4,2] = 
                //M[4,3] = 
                M[4, 4] = 1 / (ShearModulus * kt * kt);
                //M[4,5] =
                M[5, 0] = X / (Complex.ImaginaryOne * k33 * Y);
                //M[5,1] = 
                //M[5,2] = 
                M[5, 3] = w * kt * (B1 - B2) / (k33 * Y);
                //M[5,4] = 
                M[5, 5] = w * kt * (1 - A1 * (B1 - B2) / X) / (k33 * Y * B1);

                return M;
            }

            public static SparseMatrix FluidMatrix(double h, Complex kz, Complex K, Complex Zc)
            {
                SparseMatrix M = new SparseMatrix(2, 2);
                Complex kzh = kz * h;
                Complex sinkzh = Complex.Sin(kzh);
                Complex coskzh = Complex.Cos(kzh);
                //Complex pr2 = w * K * Zc / w;
                Complex pr2 =  K * Zc;

                M[0, 0] = 1d * coskzh;
                M[0, 1] = (Complex.ImaginaryOne * pr2 / kz) * sinkzh;
                M[1, 0] = (Complex.ImaginaryOne * kz / pr2) * sinkzh;
                M[1, 1] = 1d * coskzh;

                return M;
            }

            public static SparseMatrix Solid_Matrix(Complex kt, double h, double freq, double density, double Youngs_Modulus, double Poisson_Ratio)
            {
                h *= -1;

                double Shear_Modulus = Youngs_Modulus / (2 * (1 + Poisson_Ratio));

                double w = Utilities.Numerics.PiX2 * freq;

                Complex kcis2 = w * w * density / Shear_Modulus;
                Complex kcomp2 = kcis2 * ((1 - 2 * Poisson_Ratio) / (2 - 2 * Poisson_Ratio));

                Complex kphi3 = Complex.Sqrt(kcomp2 - kt * kt);
                Complex kpsi3 = Complex.Sqrt(kcis2 - kt * kt);

                Complex cosk13h = Complex.Cos(kphi3 * h);
                Complex sink13h = Complex.Sin(kphi3 * h);
                Complex cosk33h = Complex.Cos(kpsi3 * h);
                Complex sink33h = Complex.Sin(kpsi3 * h);
                Complex KK = kphi3 * kphi3 + (Poisson_Ratio / (1-2*Poisson_Ratio))*(kt*kt + kphi3*kphi3);

                SparseMatrix MH = new SparseMatrix(4, 4);
                MH[0, 0] = -Complex.ImaginaryOne * kt * cosk13h;
                MH[0, 1] = - kt * sink13h;
                MH[0, 2] = -kpsi3 * sink33h;
                MH[0, 3] = -Complex.ImaginaryOne * kpsi3 * cosk33h;
                MH[1, 0] = -kphi3 * sink13h;
                MH[1, 1] = -Complex.ImaginaryOne * kphi3 * cosk13h;
                MH[1, 2] = Complex.ImaginaryOne * kt * cosk33h;
                MH[1, 3] = kt * sink33h;
                MH[2, 0] = -2 * Shear_Modulus * cosk13h * KK / (Complex.ImaginaryOne * w);
                MH[2, 1] = 2 * Shear_Modulus * sink13h * KK / (Complex.ImaginaryOne * w);
                MH[2, 2] = -(2 * Shear_Modulus * kt * kpsi3 * sink33h / w);
                MH[2, 3] = -(Complex.ImaginaryOne * 2 * Shear_Modulus * kt * kpsi3 * cosk33h / w);
                MH[3, 0] = 2 * Shear_Modulus * kt * kphi3 * sink13h / w;
                MH[3, 1] = Complex.ImaginaryOne * 2 * Shear_Modulus * kt * kphi3 * cosk13h / w;
                MH[3, 2] = -Complex.ImaginaryOne * Shear_Modulus * cosk33h * (kt * kt - kpsi3 * kpsi3) / w;
                MH[3, 3] = -Shear_Modulus * sink33h * (kt * kt - kpsi3 * kpsi3) / w;

                SparseMatrix M0 = new SparseMatrix(4, 4);
                M0[0, 0] = -Complex.ImaginaryOne * kt;
                M0[0, 3] = -Complex.ImaginaryOne * kpsi3;
                M0[1, 1] = -Complex.ImaginaryOne * kphi3;
                M0[1, 2] = Complex.ImaginaryOne * kt;
                M0[2, 0] = -2 * Shear_Modulus * KK / (Complex.ImaginaryOne * w);
                M0[2, 3] = -(Complex.ImaginaryOne * 2 * Shear_Modulus * kt * kpsi3 / w);
                M0[3, 1] = Complex.ImaginaryOne * 2 * Shear_Modulus * kt * kphi3 / w;
                M0[3, 2] = -Complex.ImaginaryOne * Shear_Modulus * (kt * kt - kpsi3 * kpsi3) / w;
                
                return MH * M0.Inverse() as SparseMatrix;
            }

            public static SparseMatrix Solid_Matrix(Complex ksolid, Complex k0, Complex kt, double h, double freq, double density, Complex LameMu, Complex LameL)
            {
                h *= -1;
                double w = Utilities.Numerics.PiX2 * freq;

                double delta21 = w * w * density;
                //Complex k1 = ksolid;
                double delta23 = delta21 / LameMu.Real;
                delta21 /= (LameL.Real + 2 * LameMu.Real);
                //double k1 = System.Math.Sqrt(delta21);
                //Complex k1 = ksolid * kt / k0;
                Complex k1 = kt;
                //Complex k3 = Complex.Sqrt( - k1 * k1);
                Complex k13 = Complex.Sqrt(delta21 - k1 * k1);//kt
                Complex k33 = Complex.Sqrt(delta23 - k1 * k1);//kt
                Complex D1 = LameL * (ksolid * ksolid + k13 * k13) + 2 * LameMu * k13 * k13;
                //Complex D1 = LameMu * (k13 * k13 - k0 * k0); //ksolid could also be k_air, or k of the previous layer (not clear).
                Complex D2 = 2 * LameMu * ksolid;

                Complex cosk13h = Complex.Cos(k13 * h);
                Complex sink13h = Complex.Sin(k13 * h);
                Complex cosk33h = Complex.Cos(k33 * h);
                Complex sink33h = Complex.Sin(k33 * h);
                Complex wk1 = w * k1;
                Complex wk13 = w * k13;
                Complex wk33 = w * k33;
                Complex D1k13 = D1 * k13;
                Complex D1k33 = D1 * k33;
                Complex D2k13 = D2 * k13;
                Complex D2k33 = D2 * k33;

                SparseMatrix MH = new SparseMatrix(4, 4);
                MH[0, 0] = wk1 * cosk13h;
                MH[0, 1] = -Complex.ImaginaryOne * wk1 * sink13h;
                MH[0, 2] = Complex.ImaginaryOne * wk33 * sink33h;
                MH[0, 3] = -wk33 * cosk33h;
                MH[1, 0] = -Complex.ImaginaryOne * wk13 * sink13h;
                MH[1, 1] = wk13 * cosk13h;
                MH[1, 2] = wk1 * cosk33h;
                MH[1, 3] = -Complex.ImaginaryOne * wk1 * sink33h;
                MH[2, 0] = -D1 * cosk13h;
                MH[2, 1] = Complex.ImaginaryOne * D1 * sink13h;
                MH[2, 2] = Complex.ImaginaryOne * D2k33 * sink33h;
                MH[2, 3] = -D2k33 * cosk33h;
                MH[3, 0] = Complex.ImaginaryOne * D2k13 * sink13h;
                MH[3, 1] = -D2k13 * cosk13h;
                MH[3, 2] = D1 * cosk33h;
                MH[3, 3] = -Complex.ImaginaryOne * D1 * sink33h;

                SparseMatrix M0 = new SparseMatrix(4, 4);
                M0[0, 0] = 2 * k1 / (w * delta23);
                M0[0, 2] = -1 / (LameMu * delta23);
                M0[1, 1] = (k33 * k33 - k1 * k1) / (w * k13 * delta23);
                M0[1, 3] = -k1 / (LameMu * k13 * delta23);
                M0[2, 1] = k1 / (w * delta23);
                M0[2, 3] = 1 / (LameMu * delta23);
                M0[3, 0] = (k33 * k33 - k1 * k1) / (w * k33 * delta23);
                M0[3, 2] = -k1 / (LameMu * k33 * delta23);

                return MH * M0;
            }

            public static SparseMatrix InterfacePP(double porosity_of_1, double porosity_of_2)
            //    : base(6, 6)
            {
                SparseMatrix M = new SparseMatrix(6, 6);
                double phi2_1 = porosity_of_2 / porosity_of_1;
                double phi1_2 = porosity_of_1 / porosity_of_2;
                M[0, 0] = 1;
                M[1, 1] = 1;
                M[2, 1] = 1 - phi2_1;
                M[2, 2] = phi2_1;
                M[3, 3] = 1;
                M[3, 5] = 1 - phi1_2;
                M[4, 4] = 1;
                M[5, 5] = phi1_2;
                return M;
            }

            public static SparseMatrix InterfaceSF_Solid()
            //: base(3, 4)
            {
                SparseMatrix M = new SparseMatrix(3, 4);
                M[0, 1] = 1;
                M[1, 2] = 1;
                M[2, 3] = 1;
                return M;
            }

            public static SparseMatrix InterfaceSF_Fluid()
            //: base(3, 2)
            {
                SparseMatrix M = new SparseMatrix(3, 2);
                M[0, 1] = -1;
                M[1, 0] = 1;
                return M;
            }

            public static SparseMatrix Interfacepf_Porous(double porosity)
            //: base(4, 6)
            {
                SparseMatrix M = new SparseMatrix(4, 6);
                M[0, 1] = 1 - porosity;
                M[0, 2] = porosity;
                M[1, 3] = 1;
                M[2, 4] = 1;
                M[3, 5] = 1;
                return M;
            }

            public static SparseMatrix Interfacepf_Fluid(double porosity)
            //: base(4, 2)
            {
                SparseMatrix M = new SparseMatrix(4, 2);
                M[0, 1] = -1;
                M[1, 0] = 1 - porosity;
                M[3, 0] = porosity;
                return M;
            }

            public static SparseMatrix Interfacesp_Solid()
            //: base(5, 4)
            {
                SparseMatrix M = new SparseMatrix(5, 4);
                M[0, 0] = 1;
                M[1, 1] = 1;
                M[2, 1] = 1;
                M[3, 2] = 1;
                M[4, 3] = 1;
                return M;
            }

            public static SparseMatrix Interfacesp_Porous()
            {
                SparseMatrix M = new SparseMatrix(5, 6);
                M[0, 0] = 1;
                M[1, 1] = 1;
                M[2, 2] = 1;
                M[3, 3] = 1;
                M[3, 5] = 1;
                M[4, 4] = 1;
                return M;
            }

            public static SparseMatrix Interfacepi_Porous()
            //: base(4, 6)
            {
                SparseMatrix M = new SparseMatrix(4, 6);
                M[0, 1] = 1;
                M[1, 2] = 1;
                M[2, 3] = 1;
                M[2, 5] = 1;
                M[3, 4] = 1;
                return M;
            }

            public static SparseMatrix Interfacepi_Thinplate()
            {
                SparseMatrix M = new SparseMatrix(4, 2);
                M[0, 1] = -1;
                M[1, 1] = -1;
                M[2, 0] = 1;
                return M;
            }

            public static SparseMatrix RigidTerminationP()
            {
                SparseMatrix M = new SparseMatrix(3, 6);
                M[0, 0] = 1;
                M[1, 1] = 1;
                M[2, 2] = 1;
                return M;
            }

            public static SparseMatrix RigidTerminationF()
            {
                SparseMatrix M = new SparseMatrix(1, 2);
                M[0, 1] = 1;
                return M;
            }

            public static SparseMatrix RigidTerminationS()
            {
                SparseMatrix M = new SparseMatrix(2, 4);
                M[0, 0] = 1;
                M[1, 1] = 1;
                return M;
            }
        }
    }
}