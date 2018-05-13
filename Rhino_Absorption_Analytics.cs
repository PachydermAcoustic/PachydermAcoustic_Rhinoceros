//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2018, Arthur van der Harten 
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

namespace Pachyderm_Acoustic
{
    namespace AbsorptionModels
    {
        public static class Operations_RhinoSpecific
        {
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
        }
    }
}