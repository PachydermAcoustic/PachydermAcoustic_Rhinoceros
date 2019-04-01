//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2019, Arthur van der Harten 
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
using Pachyderm_Acoustic.Utilities;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        /// <summary>
        /// The OnRayShooter does not return the surface that was hit, so this class partitions the NURBS model and speedily hunts for the surface that the intersection point is on.
        /// </summary>
        public class VoxelGrid_RC
        {
            //This is required as a handicap for the ONRayShooter, which does not yet return the surface that the ray hit...
            private double X_Incr;
            private double Y_Incr;
            private double Z_Incr;
            private int VoxelCT;
            private List<int>[, ,] VoxelInventory;
            private BoundingBox OverallBBox = new BoundingBox();
            private List<Point3d>[, ,] Voxel;
            double Radius2;

            public VoxelGrid_RC(Environment.RhCommon_Scene ModelSurfaces, List<Point3d> Pts, int VG_Domain)
            {
                VoxelCT = VG_Domain;
                VoxelInventory = new List<int>[VoxelCT, VoxelCT, VoxelCT];
                Voxel = new List<Point3d>[VoxelCT, VoxelCT, VoxelCT];

                //for (int q = 0; q < ModelSurfaces.Objects.Count; q++)
                //{
                //    Surfaces[q] = ModelSurfaces.Objects[q].Object();
                //}

                BoundingBox TightOBox = new BoundingBox();
                Point3d BoxMin = new Point3d();
                Point3d BoxMax = new Point3d();

                foreach (Point3d Pt in Pts)
                {
                    TightOBox.Union(Pt);
                }

                foreach (Brep B in ModelSurfaces.Breps())
                {
                    TightOBox.Union(B.GetBoundingBox(true));
                }               
                
                OverallBBox = new BoundingBox(new Point3d(TightOBox.Min.X - 1, TightOBox.Min.Y - 1, TightOBox.Min.Z - 1), new Point3d(TightOBox.Max.X + 1, TightOBox.Max.Y + 1, TightOBox.Max.Z + 1));

                this.X_Incr = (OverallBBox.Max.X - OverallBBox.Min.X) / VoxelCT;
                this.Y_Incr = (OverallBBox.Max.Y - OverallBBox.Min.Y) / VoxelCT;
                this.Z_Incr = (OverallBBox.Max.Z - OverallBBox.Min.Z) / VoxelCT;

                double X_2 = X_Incr/2;
                double Y_2 = Y_Incr/2;
                double Z_2 = Z_Incr/2;

                Radius2 = X_2*X_2 + Y_2*Y_2 + Z_2*Z_2;

                //For((int XBox = 0; XBox < VoxelCT; XBox++)
                Parallel.For(0, VoxelCT, XBox =>
                {
                    Rhino.RhinoApp.SetCommandPrompt(string.Format("Voxelizing: {0}%", Math.Round((double)XBox / VoxelCT - 1, 2) * 100));
                    for (int YBox = 0; YBox < VoxelCT; YBox++)
                    {
                        for (int ZBox = 0; ZBox < VoxelCT; ZBox++)
                        {
                            BoxMin = new Point3d((OverallBBox.Min.X + this.X_Incr * XBox) - X_Incr / 10, (OverallBBox.Min.Y + this.Y_Incr * YBox) - Y_Incr / 10, (OverallBBox.Min.Z + this.Z_Incr * ZBox) - Z_Incr / 10);
                            BoxMax = new Point3d((OverallBBox.Min.X + this.X_Incr * (XBox + 1)) + X_Incr / 10, (OverallBBox.Min.Y + this.Y_Incr * (YBox + 1)) + Y_Incr / 10, (OverallBBox.Min.Z + this.Z_Incr * (ZBox + 1)) + X_Incr / 10);
                            this.Voxel[XBox, YBox, ZBox] = new List<Point3d>();
                            this.Voxel[XBox, YBox, ZBox].Add(BoxMin);
                            this.Voxel[XBox, YBox, ZBox].Add(BoxMax);
                            this.VoxelInventory[XBox, YBox, ZBox] = new List<int>();
                            for (int Index = 0; Index < ModelSurfaces.Count(); Index++)
                            {
                                BoundingBox TestBox = new BoundingBox();
                                TestBox = new BoundingBox(this.Voxel[XBox, YBox, ZBox][0], this.Voxel[XBox, YBox, ZBox][1]);
                                if (BoxIntersection(ModelSurfaces, TestBox, Index))
                                {
                                    this.VoxelInventory[XBox, YBox, ZBox].Add(Index);
                                }
                            }
                        }
                    }
                });
            }

            /// <summary>
            /// Intesects all NURBS Surfaces with Voxels.
            /// </summary>
            /// <param name="Model">the scene.</param>
            /// <param name="Box">the voxel</param>
            /// <param name="Index">the index of the surface to test.</param>
            /// <returns></returns>
            private bool BoxIntersection(RhCommon_Scene Model, BoundingBox Box, int Index)
            {
                Point3d CP = Model.Brep(Index).ClosestPoint(Box.Center);
                Vector3d V = CP - Box.Center;
                if (V.X*V.X + V.Y*V.Y + V.Z*V.Z < Radius2)
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Quickly finds which voxel the intersection point lies in.
            /// </summary>
            /// <param name="Point"></param>
            /// <param name="XVoxel"></param>
            /// <param name="YVoxel"></param>
            /// <param name="ZVoxel"></param>
            public void PointIsInVoxel(Point3d Point, ref int XVoxel, ref int YVoxel, ref int ZVoxel)
            {
                XVoxel = (int)Math.Floor((Point.X - OverallBounds().Min.X) / X_Incr);
                YVoxel = (int)Math.Floor((Point.Y - OverallBounds().Min.Y) / Y_Incr);
                ZVoxel = (int)Math.Floor((Point.Z - OverallBounds().Min.Z) / Z_Incr);
            }

            /// <summary>
            /// Gets the contents of a voxel.
            /// </summary>
            /// <param name="X">the x voxel index</param>
            /// <param name="Y">the y voxel index</param>
            /// <param name="Z">the z voxel index</param>
            /// <returns></returns>
            public List<int> VoxelList(int X, int Y, int Z)
            {
                return VoxelInventory[X, Y, Z];
            }

            /// <summary>
            /// The bounding box of the entire model.
            /// </summary>
            /// <returns></returns>
            public BoundingBox OverallBounds()
            {
                return OverallBBox;
            }
        }
    }
}