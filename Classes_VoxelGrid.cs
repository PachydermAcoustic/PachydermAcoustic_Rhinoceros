//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2012, Arthur van der Harten 
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

using RMA.Rhino;
using RMA.OpenNURBS;
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
        public class VoxelGrid
        {
            //This is required as a handicap for the ONRayShooter, which does not yet return the surface that the ray hit...
            private double X_Incr;
            private double Y_Incr;
            private double Z_Incr;
            private int VoxelCT;
            private List<int>[, ,] VoxelInventory;
            private OnBoundingBox OverallBBox = new OnBoundingBox();
            private List<On3dPoint>[, ,] Voxel;

            public VoxelGrid(RhCommon_Scene ModelSurfaces, List<IOn3dPoint> Pts, int VG_Domain)
            {
                VoxelCT = VG_Domain;
                VoxelInventory = new List<int>[VoxelCT, VoxelCT, VoxelCT];
                Voxel = new List<On3dPoint>[VoxelCT, VoxelCT, VoxelCT];
                IRhinoObject[] Surfaces = new IRhinoObject[ModelSurfaces.Count()];

                OnBoundingBox TightOBox = new OnBoundingBox();
                On3dPoint BoxMin = new On3dPoint();
                On3dPoint BoxMax = new On3dPoint();
                ArrayOn3dPoint Points = new ArrayOn3dPoint();

                for (int i = 0; i < Pts.Count; i++)
                {
                    Points.Append(Pts[i]);
                }

                RhUtil.RhinoGetTightBoundingBox(Surfaces, ref TightOBox, false, null);
                TightOBox.Set(Points, true);
                OverallBBox = new OnBoundingBox(new On3dPoint(TightOBox.Min().x - 1, TightOBox.Min().y - 1, TightOBox.Min().z - 1), new On3dPoint(TightOBox.Max().x + 1, TightOBox.Max().y + 1, TightOBox.Max().z + 1));

                this.X_Incr = (OverallBBox.Max().x - OverallBBox.Min().x) / VoxelCT;
                this.Y_Incr = (OverallBBox.Max().y - OverallBBox.Min().y) / VoxelCT;
                this.Z_Incr = (OverallBBox.Max().z - OverallBBox.Min().z) / VoxelCT;

                //For((int XBox = 0; XBox < VoxelCT; XBox++)
                Parallel.For(0, VoxelCT, XBox =>
                {
                    RhUtil.RhinoApp().SetStatusBarMessagePane(string.Format("Voxelizing: {0}%", Math.Round((double)XBox / VoxelCT-1, 2) * 100));
                    for (int YBox = 0; YBox < VoxelCT; YBox++)
                    {
                        for (int ZBox = 0; ZBox < VoxelCT; ZBox++)
                        {
                            BoxMin = new On3dPoint((OverallBBox.Min().x + this.X_Incr * XBox) - X_Incr / 10, (OverallBBox.Min().y + this.Y_Incr * YBox) - Y_Incr / 10, (OverallBBox.Min().z + this.Z_Incr * ZBox) - Z_Incr / 10);
                            BoxMax = new On3dPoint((OverallBBox.Min().x + this.X_Incr * (XBox + 1)) + X_Incr / 10, (OverallBBox.Min().y + this.Y_Incr * (YBox + 1)) + Y_Incr / 10, (OverallBBox.Min().z + this.Z_Incr * (ZBox + 1)) + X_Incr / 10);
                            this.Voxel[XBox, YBox, ZBox] = new List<On3dPoint>();
                            this.Voxel[XBox, YBox, ZBox].Add(BoxMin);
                            this.Voxel[XBox, YBox, ZBox].Add(BoxMax);
                            this.VoxelInventory[XBox, YBox, ZBox] = new List<int>();
                            for (int Index = 0; Index < ModelSurfaces.Count(); Index++)
                            {
                                OnBoundingBox TestBox = new OnBoundingBox();
                                TestBox = new OnBoundingBox(this.Voxel[XBox, YBox, ZBox][0], this.Voxel[XBox, YBox, ZBox][1]);
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
            private bool BoxIntersection(RhCommon_Scene Model, OnBoundingBox Box, int Index)
            {
                On3dPoint Center = Box.Center();
                double Radius = Center.DistanceTo(Box.Min());
                double s = 0;
                double t = 0;
                On3dPoint point = new On3dPoint();
                if (RhUtil.RhinoBrepClosestPoint(Model.Brep(Index), Center, new OnCOMPONENT_INDEX(), ref s, ref t, point, Radius))
                //if (Center.DistanceTo(point) < Radius)
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
            public void PointIsInVoxel(IOn3dPoint Point, ref int XVoxel, ref int YVoxel, ref int ZVoxel)
            {
                XVoxel = (int)Math.Floor((Point.x - OverallBounds().Min().x) / X_Incr);
                YVoxel = (int)Math.Floor((Point.y - OverallBounds().Min().y) / Y_Incr);
                ZVoxel = (int)Math.Floor((Point.z - OverallBounds().Min().z) / Z_Incr);
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
            public OnBoundingBox OverallBounds()
            {
                return OverallBBox;
            }
        }
    }
}