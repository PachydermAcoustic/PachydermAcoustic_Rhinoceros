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
    public abstract class Edge
    {
        public static int[] Rigid = new int[] { 1, 1, 1, 1 };
        public static int[] Soft = new int[] { -1, 1, 1, -1 };
        public int[] ParentBreps;
        protected List<EdgeSource> Sources = new List<EdgeSource>();
        protected Curve BasisCurve;
 
        public Edge(Brep[] C, Curve i)
        {
            BasisCurve = i.DuplicateCurve();
        }

        public class EdgeSource
        {
            const double sincos45 = 0.70710678118654752440084436210485;
            double[] v;
            double[] v_4pi;
            double[] v_2pi;
            public Hare.Geometry.Point Z_mid;
            public Hare.Geometry.Vector Z_Norm;
            double Z_Range;
            double Z_Range_2;
            double Z_dot;//Z_Range squared.
            Hare.Geometry.Vector[] Normal = new Vector[2];
            Hare.Geometry.Vector[] Tangent = new Vector[2];
            int[] attr;
            
            public EdgeSource(int[] attr_in, Hare.Geometry.Point PtZ0, Hare.Geometry.Point _PtZ, Vector[] _Tangents)
            {
                attr = attr_in;
                Tangent = _Tangents;
                Z_Norm = _PtZ - PtZ0;

                Z_Range = Z_Norm.Length();
                Z_dot = Z_Range * Z_Range;//Hare_math.Dot(Z_Norm, Z_Norm);
                Z_Norm/= Z_Range;
                Z_Range_2 = Z_Range / 2;
                Z_mid = (PtZ0 + _PtZ) / 2;
                Vector Bisector = (Tangent[0] + Tangent[1])/2;
                Bisector.Normalize();
                double BisectAngle = Math.Acos(Hare_math.Dot(Tangent[0], Bisector));
                if (BisectAngle == 0) BisectAngle = 1E-12;
                v = new double[2] { Math.PI / (2 * BisectAngle), Math.PI / (Utilities.Numerics.PiX2 - 2 * BisectAngle) };
                v_4pi = new double[2] { v[0] / (4 * Math.PI), v[1] / (4 * Math.PI) };
                v_2pi = new double[2] { v[0] / (2 * Math.PI), v[1] / (2 * Math.PI) };
                Normal[0] = Hare_math.Cross(_Tangents[0], Z_Norm);
                Normal[1] = Hare_math.Cross(_Tangents[1], Z_Norm*-1);

                if(Hare_math.Dot(Normal[0], Bisector) > 0)
                {
                    Normal[0] *= -1;
                    Normal[1] *= -1;
                }
                
                ////////////////////////////
                //VisCheck//
                Rhino.Geometry.Point3d pt = new Rhino.Geometry.Point3d(Z_mid.x, Z_mid.y, Z_mid.z);
                Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(pt, pt + new Rhino.Geometry.Point3d(Normal[0].x, Normal[0].y, Normal[0].z));
                Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(pt, pt + new Rhino.Geometry.Point3d(Normal[1].x, Normal[1].y, Normal[1].z));
                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(pt, pt + new Rhino.Geometry.Point3d(Tangent[0].x, Tangent[0].y, Tangent[0].z));
                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(pt, pt + new Rhino.Geometry.Point3d(Tangent[1].x, Tangent[1].y, Tangent[1].z));
                //////Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(new Rhino.Geometry.Point3d(Z_mid.x, Z_mid.y, Z_mid.z));
            }

            public EdgeSource(int[] attr_in, Hare.Geometry.Point Z_mid_in, double Delta_Z, Vector[] _Tangents)
            {
                attr = attr_in;
                Tangent = _Tangents;
                Z_Norm = Hare.Geometry.Hare_math.Cross(_Tangents[0], _Tangents[1]);
                Z_Range = Delta_Z;
                Z_dot = Z_Range * Z_Range;//Hare_math.Dot(Z_Norm, Z_Norm);
                Z_Range_2 = Z_Range / 2;
                Z_Norm.Normalize();
                Z_mid = Z_mid_in;
                Vector Bisector = (Tangent[0] + Tangent[1]) / 2;
                Bisector.Normalize();
                double BisectAngle = Math.Acos(Hare_math.Dot(Tangent[0], Bisector));
                if (BisectAngle == 0) BisectAngle = 1E-12;
                v = new double[2] { Math.PI / (2 * BisectAngle), Math.PI / (Utilities.Numerics.PiX2 - 2 * BisectAngle) };
                v_4pi = new double[2] { v[0] / (4 * Math.PI), v[1] / (4 * Math.PI) };
                v_2pi = new double[2] { v[0] / (2 * Math.PI), v[1] / (2 * Math.PI) };
                //BisectAngle = Math.Cos(BisectAngle);
                Normal[0] = Hare_math.Cross(_Tangents[0], Z_Norm);
                Normal[1] = Hare_math.Cross(_Tangents[1], Z_Norm * -1);

                if (Hare_math.Dot(Normal[0], Bisector) > 0)
                {
                    Normal[0] *= -1;
                    Normal[1] *= -1;
                }

                ////////////////////////////
                //VisCheck//
                Rhino.Geometry.Point3d pt = new Rhino.Geometry.Point3d(Z_mid.x, Z_mid.y, Z_mid.z);
                Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(pt, pt + new Rhino.Geometry.Point3d(Normal[0].x, Normal[0].y, Normal[0].z));
                Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(pt, pt + new Rhino.Geometry.Point3d(Normal[1].x, Normal[1].y, Normal[1].z));
                //Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(new Rhino.Geometry.Point3d(Z_mid.x, Z_mid.y, Z_mid.z));
            }

            public bool Cyl_Coord(Hare.Geometry.Point S, Hare.Geometry.Point R, ref double rs, ref double thetas, ref double zs, ref double rr, ref double thetar, ref double zr, out int Obtuse_Side)//, out double[] tm, out double[] tl)
            {
                //diffx = Tangent;
                //diffy = Normal;
                //diffz = Z_Norm;
                Vector S_D = S - Z_mid;
                Vector R_D = R - Z_mid;

                Vector S_Norm = new Vector(S_D.x, S_D.y, S_D.z) / S_D.Length();// S - Z_mid;
                Vector R_Norm = new Vector(R_D.x, R_D.y, R_D.z) / R_D.Length();//R - Z_mid;

                double S0 = Hare_math.Dot(S_Norm, Tangent[0]);
                double S1 = Hare_math.Dot(S_Norm, Tangent[1]);

                uint SDIR = 0;
                if (S0 > S1)
                {
                    if (S1 > 0) SDIR = 1;
                }

                Obtuse_Side = (Hare_math.Dot(S_Norm, Normal[SDIR]) < 0 ? 1 : 0);

                zs = Hare_math.Dot(S_D, Z_Norm);//S_Coord.z;
                zr = Hare_math.Dot(R_D, Z_Norm);//R_Coord.z;

                Hare.Geometry.Point S_p = S - (Z_mid + zs * Z_Norm);
                Hare.Geometry.Point R_p = R - (Z_mid + zr * Z_Norm);

                rs = Math.Sqrt(S_p.x * S_p.x + S_p.y * S_p.y + S_p.z * S_p.z);//Math.Sqrt(S_Coord.x * S_Coord.x + S_Coord.y * S_Coord.y + S_Coord.z * S_Coord.z);
                rr = Math.Sqrt(R_p.x * R_p.x + R_p.y * R_p.y + R_p.z * R_p.z);//Math.Sqrt(R_Coord.x * R_Coord.x + R_Coord.y * R_Coord.y + R_Coord.z * R_Coord.z);
                thetas = Math.Acos(Hare_math.Dot(Tangent[SDIR], S_Norm));//Math.Atan2(S_Coord.y, S_Coord.x);

                //double sdt = Hare_math.Dot(Tangent[SDIR], S_Norm);
                double rdt = Hare_math.Dot(Normal[SDIR] * (Obtuse_Side == 1 ? 1 : -1), R_Norm);

                if (rdt > 0)
                {
                    thetar = Utilities.Numerics.PiX2 - Math.Acos(Hare_math.Dot(Tangent[SDIR], R_Norm));//Math.Atan2(R_Coord.y, R_Coord.x);
                }
                else 
                {
                    thetar = Math.Acos(Hare_math.Dot(Tangent[SDIR], R_Norm));
                }
               
                //if (thetas < 0) thetas += Utilities.Numerics.PiX2;
                //if (thetar < 0) thetar += Utilities.Numerics.PiX2;
                return true;
            }

            public double[] Phi(double thetaS, double thetaR)
            {
                return new double[] { 
                        Math.PI + thetaS + thetaR, 
                        Math.PI + thetaS - thetaR, 
                        Math.PI - thetaS + thetaR, 
                        Math.PI - thetaS - thetaR };
            }

            public double[] Beta(double thetaS, double thetaR, double Aux_N, int obtuse)
            {
                double[] phi = Phi(thetaS, thetaR);
                double[] B = new double[4];
                double v_A = v[obtuse] * Aux_N;
                double v_P = v[obtuse] * phi[0];
                B[0] = attr[0] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                v_P = v[obtuse] * phi[1];
                B[1] = attr[1] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                v_P = v[obtuse] * phi[2];
                B[2] = attr[2] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                v_P = v[obtuse] * phi[3];
                B[3] = attr[3] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                return B;
            }

            public double Beta_i(double thetaS, double thetaR, double Aux_N, int obtuse)
            {
                double[] phi = Phi(thetaS, thetaR);
                double v_A = v[obtuse] * Aux_N;
                double v_P = v[obtuse] * phi[0];
                double B = attr[0] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                v_P = v[obtuse] * phi[1];
                B += attr[1] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                v_P = v[obtuse] * phi[2];
                B += attr[2] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                v_P = v[obtuse] * phi[3];
                B += attr[3] * Math.Sin(v_P) / (Math.Cosh(v_A) - Math.Cos(v_P));
                return B;
            }

            public double Z_apex(double Z_S, double Z_R, double r_S, double r_R)
            {
                return (Z_R * r_S + Z_S * r_R) / (r_R + r_S);
            }

            public double Calc_Pressure(double B, double m_in, double l_out, bool obtuse)
            {
                return (-v_4pi[Convert.ToInt32(obtuse)] * B) / (m_in * l_out);
            }

            public double Aux_n(double d_S, double z_S, double z_R, double r_S, double r_R, double thetaS, double thetaR)
            {
                double z_rel = (z_R - z_S);
                double Z = (d_S * d_S - (r_S * r_S + r_R * r_R + z_rel * z_rel)) / (2 * r_S * r_R);
                if (Z < 1)
                    return 0;
                return Math.Log(Z + Math.Sqrt(Z * Z - 1));
            }

            public double Aux_n(double alpha_in, double gamma_out, double thetaS, double thetaR)
            {
                double Z = 1 + Math.Sin(alpha_in) * Math.Sin(gamma_out) / (Math.Cos(alpha_in) * Math.Cos(gamma_out));
                return Math.Log(Z + Math.Sqrt(Z * Z - 1));
            }

            public double R_o(double zr, double zs, double rr, double rs)
            {
                double r = rs + rr;
                double z = zr - zs;
                return Math.Sqrt(r * r + z * z);
            }

            public double Rho(double rr, double rs)
            {
                return rr / rs;
            }

            public double sinpsi(double rs, double rr, double R0)
            {
                return (rs + rr) / R0;
            }

            public double cospsi(double zs, double zr, double R0)
            {
                return (zr + zs) / R0;
            }

            public double[] B_0(double R0, double rho, double[] phi, double rs, double rr, int obtuse)
            {
                double[] B = new double[4];
                double rt1_2 = (1 + rho);
                rt1_2 *= rt1_2;
                double sinpsi = (rs + rr) / R0;
                double mod = 4 * R0 * R0 * rho * rho * rho / (v[obtuse] * v[obtuse] * rt1_2 * rt1_2 * (rt1_2 * sinpsi * sinpsi - 2 * rho));

                for (int i = 0; i < 4; i++)
                {
                    B[i] = attr[i] * Math.Sin(v[obtuse] * phi[i]) * mod;
                }

                return B;
            }

            public double B_0(double R0, double rho, double[] phi, double rr, double rs, double sinpsi, int obtuse)
            {
                double B = 0;
                double rt1_2 = (1 + rho);
                rt1_2 *= rt1_2;
                double mod = 4 * R0 * R0 * rho * rho * rho / (v[obtuse] * v[obtuse] * rt1_2 * rt1_2 * (rt1_2 * sinpsi * sinpsi - 2 * rho));

                for (int i = 0; i < 4; i++)
                {
                    B += attr[i] * Math.Sin(v[obtuse] * phi[i]);
                }

                return B * mod;
            }

            public double B_1(double R0, double rho, double[] phi, int obtuse)
            {
                double B = 0;
                double v_2 = v[obtuse] * 0.5;
                double rt1_2 = (1 + rho);
                rt1_2 *= rt1_2;
                for (int i = 0; i < 4; i++)
                {
                    double svp_2 = Math.Sin(phi[i] * v_2);
                    B += attr[i] * svp_2 * svp_2;
                }
                return B * 4 * R0 * R0 * rho * rho / (v[obtuse] * v[obtuse] * rt1_2 * rt1_2);
            }

            public double B_2(double R0, double rho, double sinpsi, double cospsi)
            {
                double rt1 = (1 + rho);
                return 2 * R0 * (1 - rho) * rho * cospsi / (rt1 * (rt1 * rt1 * sinpsi * sinpsi - 2 * rho));
            }

            public double B_3(double R0, double rho, double sinpsi)
            {
                double rt1_2 = (1 + rho);
                rt1_2 *= rt1_2;
                return 2 * R0 * R0 * rho * rho / (rt1_2 * (rt1_2 * sinpsi * sinpsi - 2 * rho));
            }

            public double B_4(double[] phi, int obtuse)
            {
                double B4 = 0;
                for (int i = 0; i < 4; i++)
                {
                    B4 += attr[i] * Math.Sin(v[obtuse] * phi[i]);
                }
                return B4 / (2 * v[obtuse] * v[obtuse]);
            }

            public double B_4(double B0, double B3)
            {
                return B0 / B3;
            }

            public double B_5(double B0, double B2)
            {
                return B0 / B2;
            }

            public double B_5(double R0, double rho, double[] phi, double cospsi, int obtuse)
            {
                double B5 = 0;
                for (int i = 0; i < 4; i++)
                {
                    B5 += attr[i] * Math.Sin(v[obtuse] * phi[i]);
                }
                double rt1 = rho + 1;
                return B5 * 2 * R0 * rho * rho / (v[obtuse] * v[obtuse] * rt1 * rt1 * rt1 * (1 - rho) * cospsi);
            }

            public double B_6(double B3, double B2)
            {
                return B3 / B2;
            }

            public double B_6(double R0, double rho, double cospsi)
            {
                return R0 * rho / ((1 - rho * rho) * cospsi);
            }

            public double F(double B2, double B3)
            {
                double q = 4 * B3 - B2 * B2;
                if (q < 0)
                {
                    double rtQ = Math.Sqrt(-q);
                    double ZR2 = 2 * Z_Range;
                    double rtQB2 = rtQ * B2;
                    return Math.Log((ZR2 + B2 - rtQB2 + rtQ) / (ZR2 + B2 + rtQB2 - rtQ), Math.E) / rtQ;
                }
                else if (q > 0)
                {
                    double rtQ = 1 / Math.Sqrt(q);
                    double ZR2 = 2 * Z_Range;
                    return 2 * rtQ * (Math.Atan((ZR2 + B2) * rtQ) - Math.Atan(B2 * rtQ));
                }

                return (4 * Z_Range) / (B2 * (2 * Z_Range + B2));
            }

            public double Apex_Solve(double Za, double zs, double zr, double rs, double rr, double thetas, double thetar, int obtuse, out double m, out double l)
            {
                double rho = Rho(rr, rs);
                int side = Convert.ToInt32(obtuse);
                m = Math.Sqrt(rs * rs + zs * zs);
                l = Math.Sqrt(rr * rr + zr * zr);
                double R0 = m + l;//R_o(zr, zs, rr, rs);
                double[] phi = Phi(thetas, thetar);
                double sinpsi = this.sinpsi(rs, rr, R0);
                double B1 = B_1(R0, rho, phi, obtuse);
                double sqrt_B1_inv = 1 / Math.Sqrt(B1);
                if (rho == 1 || zs == zr)
                {
                    //symmetrical case
                    if ((zr - zr) / R0 == sincos45)
                    {
                        double B4 = B_4(phi, obtuse);
                        return -v_2pi[side] * (B4 * sqrt_B1_inv) * Math.Atan(Z_Range * sqrt_B1_inv);
                    }
                    else
                    {
                        double B3 = B_3(R0, rho, sinpsi);
                        double sqrt_B3_inv = 1 / Math.Sqrt(B3);
                        double B0 = B_0(R0, rho, phi, rr, rs, sinpsi, obtuse);
                        return -v_2pi[side] * (B0 / (B3 - B1)) * (sqrt_B1_inv * Math.Atan(Z_Range * sqrt_B1_inv) - sqrt_B3_inv * Math.Atan(Z_Range * sqrt_B3_inv));
                    }
                }
                //asymmetrical case
                double rt1 = 1 + rho;
                double cospsi = this.cospsi(zs, zr, R0);
                if (sinpsi * sinpsi == 2 * rho / (rt1 * rt1))
                {
                    double B5 = B_5(R0, rho, phi, cospsi, obtuse);
                    double B6 = B_6(R0, rho, cospsi);
                    double B6sqr = B6 * B6;
                    double ZRB6 = Z_Range + B6;
                    return v_2pi[side] * (B5 * B5 / (B1 + B6sqr)) * (.5 * Math.Log(Math.Abs(B6sqr * (Z_dot + B1) / (B1 * (ZRB6 * ZRB6))), Math.E) - B6 * sqrt_B1_inv * Math.Atan(Z_Range * sqrt_B1_inv));
                }
                else
                {
                    double B0 = B_0(R0, rho, phi, rs, rr, sinpsi, obtuse);
                    double B2 = B_2(R0, rho, sinpsi, cospsi);
                    double B3 = B_3(R0, rho, sinpsi);
                    double B2_2 = B2 * B2;
                    double B1_B3 = B1 - B3;
                    double F = this.F(B2, B3);
                    return v_2pi[side] * (B0 * B2 / (B1 * B2_2 + B1_B3 * B1_B3)) * (0.5 * Math.Log(Math.Abs((B3 * (Z_dot + B1)) / B1 * (Z_dot + B2 * Z_Range + B3)), Math.E) + ((2 * B1_B3 - B2_2) * sqrt_B1_inv / B2) * Math.Atan(Z_Range * sqrt_B1_inv) + ((-2 * B1_B3 - B2_2) / (2 * B2)) * F);
                }
            }

            //public double Apex_Solve(Hare.Geometry.Point src, Hare.Geometry.Point rec, ref double m, ref double l)
            //{
            //    double rr = 0, rs = 0, zr = 0, zs = 0, thetar = 0, thetas = 0;
            //    int obtuse;
            //    if (!Cyl_Coord(src, rec, ref rs, ref thetas, ref zs, ref rr, ref thetar, ref zr, out obtuse, out )) return 0;
            //    m = Math.Sqrt(rs * rs + zs * zs);
            //    l = Math.Sqrt(rr * rr + zr * zr);
            //    double rho = Rho(rr, rs);
            //    double R0 = m + l;//R_o(zr, zs, rr, rs);
            //    double[] phi = Phi(thetas, thetar);
            //    double sinpsi = this.sinpsi(rs, rr, R0);
            //    double B1 = B_1(R0, rho, phi, obtuse);
            //    double sqrt_B1_inv = 1 / Math.Sqrt(B1);
            //    if (rho == 1 || zs == zr)
            //    {
            //        //symmetrical case
            //        if ((zr - zr) / R0 == sincos45)
            //        {
            //            double B4 = B_4(phi, obtuse);
            //            return -v_2pi[obtuse] * (B4 * sqrt_B1_inv) * Math.Atan(Z_Range * sqrt_B1_inv);
            //        }
            //        else
            //        {
            //            double B3 = B_3(R0, rho, sinpsi);
            //            double sqrt_B3_inv = 1 / Math.Sqrt(B3);
            //            double B0 = B_0(R0, rho, phi, rr, rs, sinpsi, obtuse);
            //            return -v_2pi[obtuse] * (B0 / (B3 - B1)) * (sqrt_B1_inv * Math.Atan(Z_Range * sqrt_B1_inv) - sqrt_B3_inv * Math.Atan(Z_Range * sqrt_B3_inv));
            //        }
            //    }
            //    //asymmetrical case
            //    double rt1 = 1 + rho;
            //    double cospsi = this.cospsi(zs, zr, R0);
            //    if (sinpsi * sinpsi == 2 * rho / (rt1 * rt1))
            //    {
            //        double B5 = B_5(R0, rho, phi, cospsi, obtuse);
            //        double B6 = B_6(R0, rho, cospsi);
            //        double B6sqr = B6 * B6;
            //        double ZRB6 = Z_Range + B6;
            //        return v_2pi[obtuse] * (B5 * B5 / (B1 + B6sqr)) * (.5 * Math.Log(Math.Abs(B6sqr * (Z_dot + B1) / (B1 * (ZRB6 * ZRB6))), Math.E) - B6 * sqrt_B1_inv * Math.Atan(Z_Range * sqrt_B1_inv));
            //    }
            //    else
            //    {
            //        double B0 = B_0(R0, rho, phi, rs, rr, sinpsi, obtuse);
            //        double B2 = B_2(R0, rho, sinpsi, cospsi);
            //        double B3 = B_3(R0, rho, sinpsi);
            //        double B2_2 = B2 * B2;
            //        double B1_B3 = B1 - B3;
            //        double F = this.F(B2, B3);
            //        return v_2pi[obtuse] * (B0 * B2 / (B1 * B2_2 + B1_B3 * B1_B3)) * (0.5 * Math.Log(Math.Abs((B3 * (Z_dot + B1)) / B1 * (Z_dot + B2 * Z_Range + B3)), Math.E) + ((2 * B1_B3 - B2_2) * sqrt_B1_inv / B2) * Math.Atan(Z_Range * sqrt_B1_inv) + ((-2 * B1_B3 - B2_2) / (2 * B2)) * F);
            //    }
            //}

            public double Flex_Solve(Hare.Geometry.Point src, Hare.Geometry.Point rec, ref double m, ref double l, ref double[] dM, ref double[] dL)
            {
                double zr = 0, zs = 0, thetar = 0, thetas = 0;
                int obtuse;
                double rr = 0, rs = 0;
                if (!Cyl_Coord(src, rec, ref rs, ref thetas, ref zs, ref rr, ref thetar, ref zr, out obtuse)) return 0;
                double Za = Z_apex(zs, zr, rs, rr);

                ////get the range of times that this sample occupies./////////////////////////////////
                //double z0 = Math.Abs(zs);
                //double zl = (z0 - Z_Range_2;
                //double zu = z0 + Z_Range_2;
                //tm = new double[2] { Math.Sqrt(rs * rs + zl * zl), Math.Sqrt(rs * rs + zu * zu) };
                //z0 = Math.Abs(zr);
                //zl = z0 - Z_Range_2;
                //zu = z0 + Z_Range_2;
                //tl = new double[2] { Math.Sqrt(rr * rr + zl * zl), Math.Sqrt(rr * rr + zu * zu) };
                //////////////////////////////////////////////////////////////////////////////////////
                
                //////////////////////////////////
                //double zm = Za - zs, zl = Za - zr;
                double zm = Math.Abs(zs);
                double zl = Math.Abs(zr);
                double zml = zm - Z_Range_2;
                double zmu = zm + Z_Range_2;
                double zll = zl - Z_Range_2;
                double zlu = zl + Z_Range_2;
                double rs_2 = rs * rs, rr_2 = rr * rr;
                dM = new double[] { Math.Sqrt(zml * zml + rs_2), Math.Sqrt(zmu * zmu + rs_2) };
                dL = new double[] { Math.Sqrt(zll * zll + rr_2), Math.Sqrt(zlu * zlu + rr_2) };
                //////////////////////////////////
                if (Math.Abs(Za) < Z_Range_2)
                {
                    double B = Apex_Solve(Za, zs, zr, rs, rr, thetas, thetar, obtuse, out m, out l);
                    dM[0] = m;
                    dL[0] = l;
                    if (double.IsNaN(B))
                    {
                        Rhino.RhinoApp.WriteLine("EdgeCalc gave NAN");
                    }
                    return B;
                }
                return Gen_Solve(Za, zs, zr, rs, rr, thetas, thetar, obtuse, ref m, ref l);
            }

            //public double Gen_Solve(Hare.Geometry.Point src, Hare.Geometry.Point rec, ref double m, ref double l)
            //{
            //    double rr = 0, rs = 0, zr = 0, zs = 0, thetar = 0, thetas = 0;
            //    int obtuse;
            //    if (!Cyl_Coord(src, rec, ref rs, ref thetas, ref zs, ref rr, ref thetar, ref zr, out obtuse)) return 0;
            //    double Za = Z_apex(zs, zr, rs, rr);
            //    double zl = Za - zs;
            //    double zm = Za - zr;
            //    m = Math.Sqrt((rs * rs) + (zl * zl));
            //    l = Math.Sqrt((rr * rr) + (zm * zm));
            //    double N = this.Aux_n(m + l, zs, zr, rs, rr, thetas, thetar);
            //    double  Beta = Beta_i(thetas, thetar, N, obtuse);
            //    return -v_4pi[obtuse] * Beta / (m * l);
            //}

            public double Gen_Solve(double Za, double zs, double zr, double rs, double rr, double thetas, double thetar, int obtuse, ref double m, ref double l)
            {
                //double zl = Za - zs;
                //double zm = Za - zr;
                m = Math.Sqrt((rs * rs) + (zs * zs));
                l = Math.Sqrt((rr * rr) + (zr * zr));
                double N = this.Aux_n(m + l, zs, zr, rs, rr, thetas, thetar);
                double Beta = Beta_i(thetas, thetar, N, obtuse);
                double B = -v_4pi[obtuse] * Beta / (m * l);

                if (double.IsNaN(B))
                {
                    Rhino.RhinoApp.WriteLine("EdgeCalc gave NAN");
                }

                return B;
            }
        }

        /// <summary>
        /// Returns the list of Apexes for this source/receiver combination.
        /// </summary>
        /// <param name="src">source point</param>
        /// <param name="rec">receiver point</param>
        /// <returns></returns>
        public List<int> Find_Apex(Hare.Geometry.Point src, Hare.Geometry.Point rec)
        {
            List<int> Apex = new List<int>();
            for (int i = 0; i < Sources.Count; i++)
            {
                Vector T_Comp = Hare.Geometry.Hare_math.Cross(Sources[i].Z_mid - src, Sources[i].Z_mid - rec);
                if ((Sources[i].Z_Norm.x * T_Comp.x + Sources[i].Z_Norm.y * T_Comp.y + Sources[i].Z_Norm.z * T_Comp.z) > .95) Apex.Add(i);
            }
            return Apex;
        }

        public List<EdgeSource> EdgeSources
        {
            get 
            {
                return Sources;
            }
        }

        public Curve Basis_Edge
        {
            get 
            {
                return BasisCurve;
            }
        }
        
        //public void Solve(Hare.Geometry.Point Src, Hare.Geometry.Point Rec, Environment.Scene Rm, out List<double> P, out List<double> t, out List<Vector> dir, out List<Hare.Geometry.Point> PT, int threadid, ref Random r)
        //{
        //    P = new List<double>();
        //    t = new List<double>();
        //    dir = new List<Vector>();
        //    PT = new List<Hare.Geometry.Point>();
        //    for(int i = 0; i < Sources.Count; i++)
        //    {
        //        double dr = 0, ds = 0;                
        //        double p = Sources[i].Flex_Solve(Src, Rec, ref ds, ref dr);
        //        Vector DirR = (Sources[i].Z_mid - Rec);
        //        Vector DirS = (Sources[i].Z_mid - Src);
                
        //        DirS /= ds;
        //        DirR /= dr;
        //        double u, v;
        //        Hare.Geometry.Point pt;
        //        int id;
        //        double d;
                
        //        //Occlusion Check...
        //        if (Rm.shoot(new Ray(Src, DirS, threadid, r.Next()), out u, out v, out id, out pt, out d)) if(d < ds) {continue;};
        //        if (Rm.shoot(new Ray(Rec, DirR, threadid, r.Next()), out u, out v, out id, out pt, out d)) if(d < ds) {continue;};
                
        //        t.Add((ds + dr) / Rm.Sound_speed(Sources[i].Z_mid));
        //        dir.Add(DirR);
        //        PT.Add(Sources[i].Z_mid);
        //        P.Add(p);
        //    }

        //    //double MaxT = 0;
        //    //for (int i = 0; i < t.Count; i++) if (MaxT < t[i]) MaxT = t[i];

        //    //for (int i = 0; i < P.Count; i++)
        //    //{
                
        //    //}
        //}
    }

    public class Edge_Straight : Edge
    {
        Hare.Geometry.Point PointA;
        Hare.Geometry.Point PointB;

        public Edge_Straight(ref IEnumerable<Hare.Geometry.Point> SPT, ref IEnumerable<Hare.Geometry.Point> RPT, Environment.Medium_Properties Att_Props, Rhino.Geometry.Brep[] Brep, int[] Brep_Index, Curve Be)
            : base(Brep, Be)
        {
            //Rhino.RhinoDoc.ActiveDoc.Objects.Add(Brep[0]);
            //Rhino.RhinoDoc.ActiveDoc.Objects.Add(Brep[1]);
            ParentBreps = Brep_Index;
            //Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(Be);

            //if (!Brep.Edges[edge_id].IsLinear()) throw new Exception("Straight_Edge object called for curved edge.");
            Rhino.Geometry.Point3d PA = Be.PointAtStart;
            Rhino.Geometry.Point3d PB = Be.PointAtEnd;
            PointA = Utilities.PachTools.RPttoHPt(PA);
            PointB = Utilities.PachTools.RPttoHPt(PB);

            //Get the open wedge angle. This needs the source location, and occlusion info...
            //Assuming tangent angles are always correctly oriented...
            Vector Z_Dir = PointA - PointB;
            double length = Z_Dir.Length();
            Z_Dir.Normalize();
            double MinAngle = double.PositiveInfinity;
            List<Hare.Geometry.Point> Dpt = new List<Hare.Geometry.Point>();

            //Find the secondary source spacing DeltaZ
            Dpt.AddRange(RPT);
            Dpt.AddRange(SPT);

            for (int j = 0; j < Dpt.Count; j++)
            {
                double angle = Math.Abs(Hare_math.Dot(PointA - Dpt[j], Z_Dir));
                if (angle < MinAngle) MinAngle = angle;
                angle = Math.Abs(Hare_math.Dot(PointB - Dpt[j], Z_Dir));
                if (angle < MinAngle) MinAngle = angle;
            }
            double fs =176400; //Hz.
            double DeltaZ = Att_Props.Sound_Speed(this.PointA) / (fs * MinAngle);//TODO: Adjust depending on distance from source to receiver... (nearest, farthest?)
            
            double El_Ct = Math.Ceiling(length / DeltaZ);
            DeltaZ = length / El_Ct;

            Plane P;
            double t = Be.Domain.Mid;
            Be.PerpendicularFrameAt(t, out P);
            Curve[] Csects1;
            Curve[] Csects2;
            Point3d[] Psects;

            Rhino.Geometry.Intersect.Intersection.BrepPlane(Brep[0], P, 0.01, out Csects1, out Psects);
            Rhino.Geometry.Intersect.Intersection.BrepPlane(Brep[1], P, 0.01, out Csects2, out Psects);

            Rhino.RhinoDoc.ActiveDoc.Objects.Add(Csects1[0]);
            Rhino.RhinoDoc.ActiveDoc.Objects.Add(Csects2[0]);

            Vector3d[] Tangents = new Vector3d[2];
            ///Control Start Point of curve
            if ((Csects1[0].PointAtStart.X * P.Origin.X + Csects1[0].PointAtStart.Y * P.Origin.Y + Csects1[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects1[0].Reverse();
            if ((Csects2[0].PointAtStart.X * P.Origin.X + Csects2[0].PointAtStart.Y * P.Origin.Y + Csects2[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects2[0].Reverse();
            ///Get Tangent Vector
            Tangents[0] = (Csects1[0].PointAtNormalizedLength(0.05) - P.Origin);
            Tangents[0].Unitize();
            Tangents[1] = (Csects2[0].PointAtNormalizedLength(0.05) - P.Origin);
            Tangents[1].Unitize();
            Hare.Geometry.Vector[] HTangents = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Tangents[0].X, Tangents[0].Y, Tangents[0].Z), new Hare.Geometry.Vector(Tangents[1].X, Tangents[1].Y, Tangents[1].Z) };
            ///Get Normal
            double up, vp;
            ComponentIndex CI;
            Point3d outPt;
            Vector3d[] Normals = new Vector3d[2];
            Brep[0].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[0]);
            Brep[1].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[1]);
            Hare.Geometry.Vector[] HNormals = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Normals[0].X, Normals[0].Y, Normals[0].Z), new Hare.Geometry.Vector(Normals[1].X, Normals[1].Y, Normals[1].Z) };
            Hare.Geometry.Point Pt1 = new Hare.Geometry.Point(PointA.x, PointA.y, PointA.z);

            //TODO - Modify DeltaZ per change in velocity.
            for (int i = 1; i < El_Ct; i++)
            {
                Hare.Geometry.Point Pt2 = PointA - i * Z_Dir * DeltaZ;
                Sources.Add(new EdgeSource(Edge.Rigid, Pt1, Pt2, HTangents));
                //HTangents[1]*= -1;
                //Sources.Add(new EdgeSource(Edge.Rigid, Pt1, Pt2, HTangents));
                Pt1 = Pt2;
            }
        }
    }

    public class Edge_Curved : Edge
    {
        public Edge_Curved(ref IEnumerable<Hare.Geometry.Point> SPT, ref IEnumerable<Hare.Geometry.Point> RPT, Environment.Medium_Properties Env_Props, Brep[] B, int[] Brep_Index, Curve i)
            : base(B, i)
        {
            //if (i.IsLinear()) throw new Exception("Curved Edge object called for straight edge.");
            //Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(i);
            ParentBreps = Brep_Index;
            
            //Get the open wedge angle. This needs the source location, and occlusion info...
            //Assuming tangent angles are always correctly oriented...
            //Vector Z_Dir = PointA - PointB;
            //double length = Z_Dir.Length();
            //Z_Dir.Normalize();
            //double MinAngle = double.PositiveInfinity;
            //Hare.Geometry.Point[] Dpt = new Hare.Geometry.Point[0];

            //Find the secondary source spacing DeltaZ
            /// 1/4 wavelength substituted for now...
            double fs = 88200; //Hz.
            //double DeltaZ = speed_of_sound / (fs * Math.Cos(MinAngle));//TODO: Adjust depending on distance from source to receiver... (nearest, farthest?
           
            /////
            //double El_Ct = Math.Ceiling(i.GetLength() / DeltaZ);
            //DeltaZ = i.GetLength() / El_Ct;
            //List<Point3d> Pts;
            double length = Env_Props.Sound_Speed(i.PointAtStart)/ (8 * fs);
            double total_length = i.GetLength();
            double t;
            
            //for (int j = 1; j < Pts.Length; j++)
            while (true)
            {
                if (!i.LengthParameter(length, out t)) break;
                Plane P;
                i.PerpendicularFrameAt(t, out P);
                Curve[] Csects1, Csects2;
                Point3d[] Psects;

                //double DeltaZ = Env_Props.Sound_Speed(Point3d.Origin) / (4 * fs);
                Rhino.Geometry.Intersect.Intersection.BrepPlane(B[0], P, 0.001, out Csects1, out Psects);
                Rhino.Geometry.Intersect.Intersection.BrepPlane(B[1], P, 0.001, out Csects2, out Psects);
                Vector3d[] Tangents = new Vector3d[2];
                ///Control Start Point of curve
                if ((Csects1[0].PointAtStart.X * P.Origin.X + Csects1[0].PointAtStart.Y * P.Origin.Y + Csects1[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects1[0].Reverse();
                if ((Csects2[0].PointAtStart.X * P.Origin.X + Csects2[0].PointAtStart.Y * P.Origin.Y + Csects2[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects2[0].Reverse();
                ///Get Tangent Vector
                Tangents[0] = (Csects1[0].PointAtNormalizedLength(0.05) - P.Origin);
                Tangents[0].Unitize();
                Tangents[1] = (Csects2[0].PointAtNormalizedLength(0.05) - P.Origin);
                Tangents[1].Unitize();
                Hare.Geometry.Vector[] HTangents = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Tangents[0].X, Tangents[0].Y, Tangents[0].Z), new Hare.Geometry.Vector(Tangents[1].X, Tangents[1].Y, Tangents[1].Z) };
                ///Get Normal
                double up, vp;
                ComponentIndex CI;
                Point3d outPt;
                Vector3d[] Normals = new Vector3d[2];
                //Point3d CPT = Utilities.PachTools.HPttoRPt(Z_mid);
                B[0].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[0]);
                B[1].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[1]);
                Hare.Geometry.Vector[] HNormals = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Normals[0].X, Normals[0].Y, Normals[0].Z), new Hare.Geometry.Vector(Normals[1].X, Normals[1].Y, Normals[1].Z) };
                //// Get Vector Tangents.
                //Hare.Geometry.Point Pt1 = new Hare.Geometry.Point(Pts[j-1].X, Pts[j-1].Y, Pts[j-1].Z);
                //Hare.Geometry.Point Pt2 = new Hare.Geometry.Point(Pts[j].X, Pts[j].Y, Pts[j].Z);
                double Delta_Z = Env_Props.Sound_Speed(P.Origin) / (4 * fs);//TODO: Adjust depending on distance from source to receiver... (nearest, farthest?)
                Sources.Add(new EdgeSource(Edge.Rigid, Utilities.PachTools.RPttoHPt(P.Origin), Delta_Z, HTangents));

                length += Delta_Z;
            }
        }
    }
}