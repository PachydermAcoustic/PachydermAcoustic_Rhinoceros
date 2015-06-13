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
using Hare.Geometry;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        public class VoxelGrid_PolyRefractive: Hare.Geometry.Voxel_Grid
        {
            public double[,,] Velocities;
            public double[] XPlane_sect;
            public double[] YPlane_sect;
            public double[] ZPlane_sect;

            public VoxelGrid_PolyRefractive(Topology[] T, int Domain)
            :base(T, Domain)
            {
                for (int x = 0; x < base.Voxel_Inv.GetUpperBound(0); x++)
                {
                    for (int y = 0; y < base.Voxel_Inv.GetUpperBound(1); y++)
                    {
                        for (int z = 0; z < base.Voxel_Inv.GetUpperBound(2); z++)
                        {
                                Velocities[x, y, z] = 1.0f;
                        }
                    }
                }

                XPlane_sect = new double[Domain - 1];
                YPlane_sect = new double[Domain - 1];
                ZPlane_sect = new double[Domain - 1];

                for (int i = 0; i < Domain-1; i++)
                {
                    XPlane_sect[i] = OBox.Min_PT.x + VoxelDims.x * i;
                    YPlane_sect[i] = OBox.Min_PT.y + VoxelDims.y * i;
                    ZPlane_sect[i] = OBox.Min_PT.z + VoxelDims.z * i;
                }
            }

            /// <summary>
            /// Fire a ray into the model.
            /// </summary>
            /// <param name="R"> The ray to be entered. Make certain the Ray has a unique Ray_ID variable. </param>
            /// <param name="top_index"> Indicates the topology the ray is to intersect. </param>
            /// <param name="Ret_Event"> The nearest resulting intersection information, if any. </param>
            /// <returns> Indicates whether or not an intersection was found. </returns>
            public override bool Shoot(Ray R, int top_index, out X_Event Ret_Event)//, On3dPoint EndPt, int Octave_in)
            {
                int X, Y, Z;
                ////////////////////////
                double VPrev = 0;
                ////////////////////////
                //Identify whether the Origin is inside the voxel grid...
                //Identify which voxel the Origin point is located in...
                X = (int)Math.Floor((R.origin.x - OBox.Min_PT.x) / VoxelDims.x);
                Y = (int)Math.Floor((R.origin.y - OBox.Min_PT.y) / VoxelDims.y);
                Z = (int)Math.Floor((R.origin.z - OBox.Min_PT.z) / VoxelDims.z);

                double tDeltaX, tDeltaY, tDeltaZ;
                double tMaxX = 0, tMaxY = 0, tMaxZ = 0;

                int stepX, stepY, stepZ, OutX, OutY, OutZ;
                double t_start = 0;

                if (X < 0 || X >= VoxelCtX || Y < 0 || Y >= VoxelCtY || Z < 0 || Z >= VoxelCtZ) //return false;
                {
                    if (!OBox.Intersect(R, ref t_start, ref R.origin))
                    {
                        Ret_Event = new X_Event();
                        return false; 
                    }
                    X = (int)Math.Floor((R.origin.x - OBox.Min_PT.x + R.direction.x) / VoxelDims.x);
                    Y = (int)Math.Floor((R.origin.y - OBox.Min_PT.y + R.direction.y) / VoxelDims.y);
                    Z = (int)Math.Floor((R.origin.z - OBox.Min_PT.z + R.direction.z) / VoxelDims.z);
                }

                if (R.direction.x < 0)
                {
                    OutX = -1;
                    stepX = -1;
                    tMaxX = (float)((Voxels[X, Y, Z].Min_PT.x - R.origin.x) / R.direction.x);
                    tDeltaX = (float)(VoxelDims.x / R.direction.x * stepX);
                }
                else
                {
                    OutX = VoxelCtX;
                    stepX = 1;
                    tMaxX = (float)((Voxels[X, Y, Z].Max_PT.x - R.origin.x) / R.direction.x);
                    tDeltaX = (float)(VoxelDims.x / R.direction.x * stepX);
                }

                if (R.direction.y < 0)
                {
                    OutY = -1;
                    stepY = -1;
                    tMaxY = (float)((Voxels[X, Y, Z].Min_PT.y - R.origin.y) / R.direction.y);
                    tDeltaY = (float)(VoxelDims.y / R.direction.y * stepY);
                }
                else
                {
                    OutY = VoxelCtY;
                    stepY = 1;
                    tMaxY = (float)((Voxels[X, Y, Z].Max_PT.y - R.origin.y) / R.direction.y);
                    tDeltaY = (float)(VoxelDims.y / R.direction.y * stepY);
                }

                if (R.direction.z < 0)
                {
                    OutZ = -1;
                    stepZ = -1;
                    tMaxZ = (float)((Voxels[X, Y, Z].Min_PT.z - R.origin.z) / R.direction.z);
                    tDeltaZ = (float)(VoxelDims.z / R.direction.z * stepZ);
                }
                else
                {
                    OutZ = VoxelCtZ;
                    stepZ = 1;
                    tMaxZ = (float)((Voxels[X, Y, Z].Max_PT.z - R.origin.z) / R.direction.z);
                    tDeltaZ = (float)(VoxelDims.z / R.direction.z * stepZ);
                }

                List<double> t_list = new List<double>();
                List<int> Codes = new List<int>();
                List<Point> Path_pts = new List<Point>();
                List<int> voxelCodes = new List<int>();

                List<Point> X_LIST = new List<Point>();
                List<double> ulist = new List<double>();
                List<double> vlist = new List<double>();
                List<double> tlist = new List<double>();
                List<int> pidlist = new List<int>();

                while (true)
                {
                    //Check all polygons in the current voxel...
                    foreach (int i in Voxel_Inv[X, Y, Z, top_index])
                    {
                        if (Poly_Ray_ID[R.ThreadID, top_index][i] != R.Ray_ID)
                        {
                            Poly_Ray_ID[R.ThreadID, top_index][i] = R.Ray_ID;
                            Point Pt; double u=0, v=0, t=0;
                            if (Model[top_index].intersect(i, R, out Pt, out u, out v, out t) && t > 0.0000000001) X_LIST.Add(Pt); ulist.Add(u); vlist.Add(v); tlist.Add(t); pidlist.Add(i);
                        }
                    }

                    for (int c = 0; c < X_LIST.Count; c++)
                    {
                        if (this.Voxels[X, Y, Z].IsPointInBox(X_LIST[c]))
                        {
                            int choice = c;
                            //Ret_Event = X_LIST[c];
                            for (int s = c; s < X_LIST.Count; s++)
                            {
                                if (tlist[s] < tlist[choice])
                                {
                                    choice = s;
                                    //Ret_Event = X_LIST[s];
                                }
                            }

                            Path_pts.Add(X_LIST[choice]);
                            t_list.Add(tlist[choice]);
                            Ret_Event = new X_Event_NH(Path_pts, ulist[choice], vlist[choice], t_list, pidlist[choice], Codes);
                            //Ret_Event.t += t_start;
                            return true;
                        }
                    }

                    /////////////////////////////////////////
                    VPrev = Velocities[X, Y, Z];
                    /////////////////////////////////////////

                    //Find the Next Voxel...                    
                    /////////////////////////////////////////////////
                    if (tMaxX < tMaxY)
                    {
                        if (tMaxX < tMaxZ)
                        {
                            X += stepX;
                            if (X < 0 || X >= VoxelCtX)
                            {
                                Ret_Event = new X_Event();
                                return false; /* outside grid */
                            }
                            tMaxX += tDeltaX;
                            if (VPrev != Velocities[X, Y, Z])
                            {
                                R.direction = XRefraction(R.direction, VPrev, Velocities[X, Y, Z]);
                                if (R.direction.x < 0) { stepX = -1; }
                                else { stepX = 1; }
                                if (R.direction.y < 0) { stepY = -1; }
                                else { stepY = 1; }
                                if (R.direction.z < 0) { stepZ = -1; }
                                else { stepZ = 1; }
                                tDeltaX = (float)(VoxelDims.x / R.direction.x * stepX);
                                tDeltaY = (float)(VoxelDims.y / R.direction.y * stepY);
                                tDeltaZ = (float)(VoxelDims.z / R.direction.z * stepZ);
                                t_list.Add(tDeltaX);
                                Codes.Add(VoxelCode(X,Y,Z));
                                R.origin += R.direction * tDeltaX;
                                Path_pts.Add(R.origin);
                                X_LIST.Clear();
                            }
                        }
                        else
                        {
                            Z += stepZ;

                            if (Z < 0 || Z >= VoxelCtZ)
                            {
                                Ret_Event = new X_Event();
                                return false; /* outside grid */
                            }
                            tMaxZ += tDeltaZ;
                            if (VPrev != Velocities[X, Y, Z])
                            {
                                R.direction = ZRefraction(R.direction, VPrev, Velocities[X, Y, Z]);
                                if (R.direction.x < 0) { stepX = -1; }
                                else { stepX = 1; }
                                if (R.direction.y < 0) { stepY = -1; }
                                else { stepY = 1; }
                                if (R.direction.z < 0) { stepZ = -1; }
                                else { stepZ = 1; }
                                tDeltaX = (float)(VoxelDims.x / R.direction.x * stepX);
                                tDeltaY = (float)(VoxelDims.y / R.direction.y * stepY);
                                tDeltaZ = (float)(VoxelDims.z / R.direction.z * stepZ);
                                t_list.Add(tDeltaZ);
                                R.origin += R.direction * tDeltaZ;
                                Path_pts.Add(R.origin);
                                X_LIST.Clear();
                            }
                        }
                    }
                    else
                    {
                        if (tMaxY < tMaxZ)
                        {
                            Y += stepY;
                            if (Y < 0 || Y >= VoxelCtY)
                            {
                                Ret_Event = new X_Event();
                                return false; /* outside grid */
                            }
                            tMaxY += tDeltaY;
                            if (VPrev != Velocities[X, Y, Z])
                            {
                                R.direction = YRefraction(R.direction, VPrev, Velocities[X, Y, Z]);
                                if (R.direction.x < 0) { stepX = -1; }
                                else { stepX = 1; }
                                if (R.direction.y < 0) { stepY = -1; }
                                else { stepY = 1; }
                                if (R.direction.z < 0) { stepZ = -1; }
                                else { stepZ = 1; }
                                tDeltaX = (float)(VoxelDims.x / R.direction.x * stepX);
                                tDeltaY = (float)(VoxelDims.y / R.direction.y * stepY);
                                tDeltaZ = (float)(VoxelDims.z / R.direction.z * stepZ);
                                t_list.Add(tDeltaY);
                                R.origin += R.direction * tDeltaY;
                                Path_pts.Add(R.origin);
                                X_LIST.Clear();
                            }
                        }
                        else
                        {
                            Z += stepZ;
                            if (Z < 0 || Z >= VoxelCtZ)
                            {
                                Ret_Event = new X_Event();
                                return false; /* outside grid */
                            }
                            tMaxZ += tDeltaZ;

                            if (VPrev != Velocities[X, Y, Z])
                            {
                                R.direction = ZRefraction(R.direction, VPrev, Velocities[X, Y, Z]);
                                if (R.direction.x < 0) { stepX = -1; }
                                else { stepX = 1; }
                                if (R.direction.y < 0) { stepY = -1; }
                                else { stepY = 1; }
                                if (R.direction.z < 0) { stepZ = -1; }
                                else { stepZ = 1; }
                                tDeltaX = (float)(VoxelDims.x / R.direction.x * stepX);
                                tDeltaY = (float)(VoxelDims.y / R.direction.y * stepY);
                                tDeltaZ = (float)(VoxelDims.z / R.direction.z * stepZ);
                                t_list.Add(tDeltaZ);
                                R.origin += R.direction * tDeltaZ;
                                Path_pts.Add(R.origin);
                                X_LIST.Clear();
                            }
                        }
                    }
                }
            }

            private Vector XRefraction(Vector V_in, double VPrev, double V)
            {
                Vector V_out = new Vector( 0, -V_in.z, V_in.y);
                double V_fract = VPrev/V;
                return V_fract * new Vector(0, V_out.z, -V_out.y) - new Vector(Math.Sqrt(1 - V_fract * V_fract * Hare_math.Dot(V_out, V_out)), 0, 0);
            }

            private Vector YRefraction(Vector V_in, double VPrev, double V)
            {
                Vector V_out = new Vector(V_in.z, 0, -V_in.x);
                double V_fract = VPrev / V;
                return V_fract * new Vector(-V_out.z, 0, V_out.x) - new Vector(0, Math.Sqrt(1 - V_fract * V_fract * Hare_math.Dot(V_out, V_out)), 0);
            }

            private Vector ZRefraction(Vector V_in, double VPrev, double V)
            {
                Vector V_out = new Vector(V_in.y, V_in.x, 0);
                double V_fract = VPrev / V;
                return V_fract * new Vector(-V_out.y, -V_out.x, 0) - new Vector(0, 0, Math.Sqrt(1 - V_fract * V_fract * Hare_math.Dot(V_out, V_out)));
            }

            private double intersect_XPlane(double x_intersect, Point p, Vector d, ref Point q)
            {
                double t = (x_intersect - p.x) / d.x;
                q = p + t * d; 
                return t;
            }

            private double intersect_YPlane(double y_intersect, Point p, Vector d, ref Point q)
            {
                double t = (y_intersect - p.y) / d.y;
                q = p + t * d;
                return t;
            }

            private double intersect_ZPlane(double z_intersect, Point p, Vector d, ref Point q)
            {
                double t = (z_intersect - p.z) / d.z;
                q = p + t * d;
                return t;
            }

            public double[,,] Grid_SoundSpeed
            {
                get
                {
                    return Velocities;
                }
                set
                {
                    Velocities = value;
                }
            }

            public double Sound_Speed(Hare.Geometry.Point pt)
            {
                int X = (int)Math.Floor((pt.x - OBox.Min_PT.x) / VoxelDims.x);
                int Y = (int)Math.Floor((pt.y - OBox.Min_PT.y) / VoxelDims.y);
                int Z = (int)Math.Floor((pt.z - OBox.Min_PT.z) / VoxelDims.z);
                return Velocities[X,Y,Z];
            }

            public double Sound_Speed(int X, int Y, int Z)
            {
                return Velocities[X,Y,Z];
            }
        }

        /// <summary>
        /// A structure containing all data about the intersection resulting from a "shoot" operation in a spatial partition class.
        /// </summary>
        public class X_Event_NH:X_Event
        {
            public readonly System.Collections.Generic.List<double> t_trav;
            public readonly System.Collections.Generic.List<int> SPCode;
            public readonly System.Collections.Generic.List<Point> P_Points;
            /// <summary>
            ///// Used to represent either an empty intersection event, or a no-intersection event...
            ///// </summary>
            public X_Event_NH()
                :base()
            {
            }

            /// <summary>
            /// Used to store information about a discovered intersection event...
            /// </summary>
            /// <param name="P"> The intersection point. </param>
            /// <param name="u_in"> U coordinate of the intersection. </param>
            /// <param name="v_in"> V coordinate of the intersection.</param>
            /// <param name="t_in"> The distance along the ray in units related to the ray's length. If raylength is 1, this is the actual travel distance of the ray in model units. </param>
            /// <param name="Poly_index"> The index of the polygon upon which the intersection point may be found. </param>
            public X_Event_NH(Point P, double u_in, double v_in, double t_in, int Poly_index)
            {
                throw new Exception("Not Used");
            }

            /// <summary>
            /// Used to store information about a discovered intersection event...
            /// </summary>
            /// <param name="P"> The intersection point. </param>
            /// <param name="u_in"> U coordinate of the intersection. </param>
            /// <param name="v_in"> V coordinate of the intersection.</param>
            /// <param name="t_in"> The distance along the ray in units related to the ray's length. If raylength is 1, this is the actual travel distance of the ray in model units. </param>
            /// <param name="Poly_index"> The index of the polygon upon which the intersection point may be found. </param>
            public X_Event_NH(System.Collections.Generic.List<Point> P, double u_in, double v_in, System.Collections.Generic.List<double> t_in, int Poly_index, System.Collections.Generic.List<int> Code)
            :base(P[P.Count], u_in, v_in, 0, Poly_index)
            {
                t_trav = t_in;
                P_Points = P;
                SPCode = Code;
            }
        }
    }
}