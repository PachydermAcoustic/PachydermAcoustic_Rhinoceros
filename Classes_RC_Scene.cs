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
using Rhino;
using Rhino.Geometry;
using Hare.Geometry;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        [Serializable]
        public class RhCommon_Scene : Scene
        {
            private List<Brep> BrepList = new List<Brep>();
            private List<Plane> Plane = new List<Plane>();
            private List<bool> PlaneBoolean = new List<bool>();
            private List<Vector3d> PlanarNormal = new List<Vector3d>();
            private List<Transform> Mirror = new List<Transform>();
            private VoxelGrid_RC Voxels;
            int XVoxel, YVoxel, ZVoxel;
            private Point3d S_Origin;
            List<int> SurfaceIndex;
            protected List<Rhino.DocObjects.ObjRef> ObjectList;

            public RhCommon_Scene(List<Rhino.DocObjects.RhinoObject> ObjRef, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool IsAcoustic)
                : base(Temp, hr, Pa, Air_Choice, EdgeCorrection, IsAcoustic)
            {
                Vector3d NormalHolder = new Vector3d();
                Plane PlaneHolder = new Plane();
                Transform XHolder = new Transform();
                Random RND = new Random();
                ObjectList = new List<Rhino.DocObjects.ObjRef>();
                
                for (int q = 0; q < ObjRef.Count; q++)
                {
                    ObjectList.Add(new Rhino.DocObjects.ObjRef(ObjRef[q]));

                    Rhino.Geometry.Brep BObj;
                    if (ObjRef[q].ObjectType == Rhino.DocObjects.ObjectType.Brep)
                    {
                        BObj = ((Rhino.DocObjects.BrepObject)ObjRef[q]).BrepGeometry;
                    }
                    else
                    {
                        BObj = ((Rhino.DocObjects.ExtrusionObject)ObjRef[q]).ExtrusionGeometry.ToBrep();
                    }

                    for (int j = 0; j < BObj.Faces.Count; j++)
                    {
                        Brep B_Temp = BObj.DuplicateSubBrep(new List<int>() { j });
                        BrepList.Add(B_Temp);
                        string Mode = null;
                        string AcousticsData = null;
                        double[] Absorption = new double[8];
                        //double[,] Scattering = new double[8, 3];
                        //double[] Reflection = new double[8];
                        double[] phase = new double[8];
                        double[] Transparency = new double[8];
                        double[] Transmission = new double[8];
                        Mode = BObj.GetUserString("Acoustics_User");
                        double[] Scat = new double[8];
                        for (int oct = 0; oct < 8; oct++) phase[oct] = 0;

                        if (Mode == "yes")
                        {
                            AcousticsData = BObj.GetUserString("Acoustics");
                            if (AcousticsData != "")
                            {
                                UI.PachydermAc_PlugIn.DecodeAcoustics(AcousticsData, ref Absorption, ref Scat, ref Transparency);
                            }
                            else
                            {
                                if (!Custom_Method)
                                {
                                    Status = System.Windows.Forms.MessageBox.Show("A material is not specified correctly. Please assign absorption and scattering to all layers in the model.", "Materials Error", System.Windows.Forms.MessageBoxButtons.OK);
                                    Complete = false;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjRef[q].Attributes.LayerIndex];
                            string Method = layer.GetUserString("ABSType");
                            AcousticsData = layer.GetUserString("Acoustics");
                            if (Method == "Buildup")
                            {
                                List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                                string[] Buildup = layer.GetUserString("Buildup").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string l in Buildup) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(l));
                                AbsorptionData.Add(new Smart_Material(Layers, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2));
                            }
                            if (!string.IsNullOrEmpty(AcousticsData))
                            {
                                UI.PachydermAc_PlugIn.DecodeAcoustics(AcousticsData, ref Absorption, ref Scat, ref Transparency);
                                AbsorptionData.Add(new Basic_Material(Absorption, phase));
                            }
                            else
                            {
                                if (!Custom_Method)
                                {
                                    Status = System.Windows.Forms.MessageBox.Show("A material is not specified correctly. Please assign absorption and scattering to all layers in the model.", "Materials Error", System.Windows.Forms.MessageBoxButtons.OK);
                                    Complete = false;
                                    return;
                                }
                            }
                        }

                        for (int oct = 0; oct < 8; oct++)
                        {
                            //Reflection[oct] = (1 - Absorption[oct]);
                            Transmission[oct] = Transparency[oct];
                            //Scattering[oct, 1] = Scat[oct];
                            //phase[oct] = 0;
                        }

                        //ReflectionData.Add(Reflection);
                        ScatteringData.Add(new Lambert_Scattering(Scat, SplitRatio));
                        TransmissionData.Add(Transmission);
                        //PhaseData.Add(phase);
                        bool Trans = false;
                        for (int t_oct = 0; t_oct < 8; t_oct++)
                        {
                            if (Transmission[t_oct] > 0)
                            {
                                Trans = true;
                                break;
                            }
                        }
                        Transmissive.Add(Trans);
                        PlaneBoolean.Add(BObj.Faces[j].IsPlanar());

                        if (PlaneBoolean[PlaneBoolean.Count - 1])
                        {
                            Vector3d Normal = new Vector3d();
                            Point3d Origin = new Point3d();
                            //Transform MirrorSingle = new Transform();
                            //Plane PlaneSingle = new Plane();
                            Origin = BObj.Faces[j].PointAt(0, 0);
                            Normal = BObj.Faces[j].NormalAt(RND.NextDouble(), RND.NextDouble());
                            Mirror.Add(Transform.Mirror(Origin, Normal));
                            Plane.Add(new Plane(Origin, Normal));
                            PlanarNormal.Add(Normal);
                        }
                        else
                        {
                            PlanarNormal.Add(NormalHolder);
                            Plane.Add(PlaneHolder);
                            Mirror.Add(XHolder);
                        }
                    }
                }
                Valid = true;
            }

            public RhCommon_Scene(List<Rhino.Geometry.Brep> ObjRef, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool IsAcoustic)
                : base(Temp, hr, Pa, Air_Choice, EdgeCorrection, IsAcoustic)
            {
                Vector3d NormalHolder = new Vector3d();
                Plane PlaneHolder = new Plane();
                Transform XHolder = new Transform();
                Random RND = new Random();

                for (int q = 0; q < ObjRef.Count; q++)
                {
                    //ObjectList.Add(new Rhino.DocObjects.ObjRef(ObjRef[q]));

                    //Rhino.Geometry.Brep BObj;
                    //if (ObjRef[q].ObjectType == Rhino.DocObjects.ObjectType.Brep)
                    //{
                    //    BObj = ((Rhino.DocObjects.BrepObject)ObjRef[q]).BrepGeometry;
                    //}
                    //else
                    //{
                    //    BObj = ((Rhino.DocObjects.ExtrusionObject)ObjRef[q]).ExtrusionGeometry.ToBrep();
                    //}

                    for (int j = 0; j < ObjRef[q].Faces.Count; j++)
                    {
                        Brep B_Temp = ObjRef[q].DuplicateSubBrep(new List<int>() { j });
                        BrepList.Add(B_Temp);
                        string Mode = null;
                        double[] Absorption = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
                        double[,] Scattering = new double[8, 3] {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}};
//                        double[] Reflection = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
                        double[] Transparency = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
                        double[] Transmission = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
                        Mode = ObjRef[q].GetUserString("Acoustics_User");
                        double[] Scat = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
                        double[] phase = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};

                        //ReflectionData.Add(Reflection);
                        AbsorptionData.Add(new Basic_Material(Absorption, phase));
                        ScatteringData.Add(new Lambert_Scattering(Scat, SplitRatio));
                        TransmissionData.Add(Transmission);
                        //PhaseData.Add(phase);

                        Transmissive.Add(false);
                        PlaneBoolean.Add(ObjRef[q].Faces[j].IsPlanar());

                        if (PlaneBoolean[PlaneBoolean.Count - 1])
                        {
                            Vector3d Normal = new Vector3d();
                            Point3d Origin = new Point3d();
                            //Transform MirrorSingle = new Transform();
                            //Plane PlaneSingle = new Plane();
                            Origin = ObjRef[q].Faces[j].PointAt(0, 0);
                            Normal = ObjRef[q].Faces[j].NormalAt(RND.NextDouble(), RND.NextDouble());
                            Mirror.Add(Transform.Mirror(Origin, Normal));
                            Plane.Add(new Plane(Origin, Normal));
                            PlanarNormal.Add(Normal);
                        }
                        else
                        {
                            PlanarNormal.Add(NormalHolder);
                            Plane.Add(PlaneHolder);
                            Mirror.Add(XHolder);
                        }
                    }
                }
                //SurfaceArray = SurfaceList.ToArray();
                Valid = true;
            }

            //public override void Standardize_Normals()
            //{
            //    base.Standardize_Normals();

            //    Brep[] Polyhedra = Rhino.Geometry.Brep.JoinBreps(this.BrepList, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
            //    BoundingBox Box = Polyhedra[0].GetBoundingBox(true);
            //    for (int i = 1; i < Polyhedra.Length; i++)
            //    {
            //        Box.Union(Polyhedra[i].GetBoundingBox(true));
            //    }
            //    Point3d Src = Box.Center;

            //    foreach (Brep B in BrepList)
            //    {
            //        for (int j = 0; j < B.Faces.Count; j++)
            //        {
            //            AreaMassProperties A = AreaMassProperties.Compute(B.Faces[0]);
            //            Vector3d Dir = (A.Centroid - Src);
            //            Dir.Unitize();
            //            LineCurve L = new LineCurve(Src, A.Centroid + Dir * double.Epsilon);

            //            Curve[] C;
            //            Point3d[] IPts;

            //            Rhino.Geometry.Ray3d R = new Ray3d(Src, Dir);

            //            for (int i = 0; i < Polyhedra.Length; i++)
            //            {
            //                Rhino.Geometry.Intersect.Intersection.CurveBrep(L, Polyhedra[i], 0.001, out C, out IPts);
                            
            //            }
            //        }
            //    }
            //}

            public override void Absorb(ref OctaveRay Ray, out double cos_theta, double u, double v)
            {
                AbsorptionData[Ray.Surf_ID].Absorb(ref Ray, out cos_theta, Normal(Ray.Surf_ID, u, v));
            }

            public override void Absorb(ref BroadRay Ray, out double cos_theta, double u, double v)
            {
                AbsorptionData[Ray.Surf_ID].Absorb(ref Ray, out cos_theta, Normal(Ray.Surf_ID, u, v));
            }

            public override void Scatter_Early(ref BroadRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, double cos_theta, double u, double v)
            {
                ScatteringData[Ray.Surf_ID].Scatter_Early(ref Ray, ref Rays, ref rand, Normal(Ray.Surf_ID, u, v), cos_theta);
            }

            public override void Scatter_Late(ref OctaveRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, double cos_theta, double u, double v)
            {
                ScatteringData[Ray.Surf_ID].Scatter_Late(ref Ray, ref Rays, ref rand, Normal(Ray.Surf_ID, u, v), cos_theta);
            }

            public override void Scatter_Simple(ref OctaveRay Ray, ref Random rand, double cos_theta, double u, double v)
            {
                ScatteringData[Ray.Surf_ID].Scatter_VeryLate(ref Ray, ref rand, Normal(Ray.Surf_ID, u, v), cos_theta);
            }

            public override void Register_Edges(IEnumerable<Hare.Geometry.Point> S, IEnumerable<Hare.Geometry.Point> R)
            {
                throw new NotImplementedException();
            }

            public override bool shoot(Ray R, out double u, out double v, out int Srf_ID, out Hare.Geometry.Point X_PT, out double t)
            {
                S_Origin = Utilities.PachTools.HPttoRPt(R.origin);
                Srf_ID = 0;

                while (true)
                {
                    Point3d[] P = Rhino.Geometry.Intersect.Intersection.RayShoot(new Ray3d(S_Origin, new Vector3d(R.direction.x, R.direction.y, R.direction.z)), BrepList, 1);

                    if (P == null) { X_PT = default(Hare.Geometry.Point); u = 0; v = 0; t = 0; return false; }

                    Voxels.PointIsInVoxel(P[0], ref XVoxel, ref YVoxel, ref ZVoxel);
                    try
                    {
                        SurfaceIndex = Voxels.VoxelList(XVoxel, YVoxel, ZVoxel);
                    }
                    catch (Exception)
                    {
                        //Rare floating point error on some computers... abandon the ray and start the next...
                        //Explanation: This would never happen on my IBM T43P laptop, but happened 
                        //consistently millions of function calls into the calculation on my 
                        //ASUS K8N-DL based desktop computer. I believe it has something to do with some quirk of that system.
                        //This try...catch statement is here in case this ever manifests on any user's computer. 
                        //It is rare enough that this should not affect the accuracy of the calculation.
                        t = 0.0f;
                        X_PT = default(Hare.Geometry.Point);
                        u = 0;
                        v = 0;
                        return false;
                    }

                    Point3d CP;
                    Vector3d N;
                    ComponentIndex CI;
                    double MD = 0.0001;

                    foreach (int index in SurfaceIndex)
                    {
                        if (BrepList[index].ClosestPoint(P[0], out CP, out CI, out u, out v, MD, out N) && (CI.ComponentIndexType == ComponentIndexType.BrepFace))
                        {
                            if ((Math.Abs(P[0].X - CP.X) < 0.0001) && (Math.Abs(P[0].Y - CP.Y) < 0.0001) && (Math.Abs(P[0].Z - CP.Z) < 0.0001))
                            {
                                Srf_ID = index;
                                X_PT = new Hare.Geometry.Point(P[0].X, P[0].Y, P[0].Z);
                                t = (double)(S_Origin.DistanceTo(P[0]));
                                return true;
                            }
                        }
                    }
                    S_Origin = new Point3d(P[0]);
                }
            }

            public override bool shoot(Ray R, out double u, out double v, out int Srf_ID, out List<Hare.Geometry.Point> X_PT, out List<double> t, out List<int> Code)
            {
                List<Point3d> X;
                if (shoot(new Point3d(R.origin.x, R.origin.y, R.origin.z), new Vector3d(R.direction.x, R.direction.y, R.direction.z), R.Ray_ID, out u, out v, out Srf_ID, out X, out t, out Code))
                {
                    X_PT = new List<Hare.Geometry.Point> { new Hare.Geometry.Point(X[0].X, X[0].Y, X[0].Z) };
                    return true;
                }
                X_PT =  new List<Hare.Geometry.Point> { default(Hare.Geometry.Point) };
                return false;
            }

            public override bool shoot(Point3d Start, Vector3d Dir, int Random, out double u, out double v, out int Srf_ID, out List<Point3d> X_PT, out List<double> t, out List<int> Code)
            {
                S_Origin = new Point3d(Start);
                Srf_ID = 0;

                while (true)
                {
                    Point3d[] P = Rhino.Geometry.Intersect.Intersection.RayShoot(new Ray3d(S_Origin, Dir), BrepList, 1);

                    if (P == null) { X_PT = new List<Point3d> { default(Point3d) }; u = 0; v = 0; t = new List<double> { 0 }; Code = new List<int> { 0 }; return false; }

                    Voxels.PointIsInVoxel(P[0], ref XVoxel, ref YVoxel, ref ZVoxel);
                    try
                    {
                        SurfaceIndex = Voxels.VoxelList(XVoxel, YVoxel, ZVoxel);
                    }
                    catch (Exception)
                    {
                        //Rare floating point error on some computers... abandon the ray and start the next...
                        //Explanation: This would never happen on my IBM T43P laptop, but happened 
                        //consistently millions of function calls into the calculation on my 
                        //ASUS K8N-DL based desktop computer. I believe it has something to do with some quirk of that system.
                        //This try...catch statement is here in case this ever manifests on any user's computer. 
                        //It is rare enough that this should not affect the accuracy of the calculation.
                        t = new List<double> { 0.0f };
                        X_PT = new List<Point3d> { default(Point3d) };
                        u = 0;
                        v = 0;
                        Code = new List<int> { 0 };
                        return false;
                    }

                    Point3d CP;
                    Vector3d N;
                    ComponentIndex CI;
                    double MD = 0.0001;

                    foreach (int index in SurfaceIndex)
                    {
                        if (BrepList[index].ClosestPoint(P[0], out CP, out CI, out u, out v, MD, out N) && (CI.ComponentIndexType == ComponentIndexType.BrepFace))
                        {
                            if ((Math.Abs(P[0].X - CP.X) < 0.0001) && (Math.Abs(P[0].Y - CP.Y) < 0.0001) && (Math.Abs(P[0].Z - CP.Z) < 0.0001))
                            {
                                Srf_ID = index;
                                X_PT = new List<Point3d> {P[0]};
                                t = new List<double> {(double)(S_Origin.DistanceTo(X_PT[0]))};
                                Code = new List<int>() { 0 };
                                return true;
                            }
                        }
                    }
                    S_Origin = new Point3d(P[0]);
                }
            }

            public override void partition()
            {
                Partitioned = true;
                partition(new List<Point3d>());
            }

            public override void partition(int SP_Param)
            {
                Partitioned = true;
                partition(new List<Point3d>(), SP_Param);
            }

            public override void partition(Point3d[] P, int SP_PARAM)
            {
                Partitioned = true;
                partition(new List<Point3d>(P), SP_PARAM);
            }

            public override void partition(Hare.Geometry.Point[] P, int SP_PARAM)
            {
                Partitioned = true;
                partition(new List<Hare.Geometry.Point>(P), SP_PARAM);
            }

            public override void partition(List<Point3d> P, int SP_PARAM)
            {
                Partitioned = true;
                Voxels = new VoxelGrid_RC(this, P, SP_PARAM);
            }

            public override void partition(List<Hare.Geometry.Point> P, int SP_PARAM)
            {
                Partitioned = true;
                List<Point3d> PTS = new List<Point3d>();
                foreach (Hare.Geometry.Point PT in P)
                {
                    PTS.Add(new Point3d(PT.x, PT.y, PT.z));
                }
                Voxels = new VoxelGrid_RC(this, PTS, SP_PARAM);
            }

            public void partition(List<Hare.Geometry.Point> P)
            {
                Partitioned = true;
                List<Point3d> PTS = new List<Point3d>();
                foreach (Hare.Geometry.Point PT in P)
                {
                    PTS.Add(new Point3d(PT.x, PT.y, PT.z));
                }
                Voxels = new VoxelGrid_RC(this, PTS, UI.PachydermAc_PlugIn.Instance.VG_Domain());
            }

            public void partition(List<Point3d> P)
            {
                Partitioned = true;
                Voxels = new VoxelGrid_RC(this, P, UI.PachydermAc_PlugIn.Instance.VG_Domain());
            }

            public override string Scene_Type()
            {
                return "Rhino_Scene";
            }

            /// <summary>
            /// used by the image source method to mirror sources over a face.
            /// </summary>
            /// <param name="PassedPoint">the point to mirror</param>
            /// <param name="q">the index of the surface to use</param>
            /// <returns>the mirrored point</returns>
            public Point3d Image(Point3d PassedPoint, int q, ref bool Success)
            {
                if (PlaneBoolean[q])
                {
                    PassedPoint.Transform(Mirror[q]);
                    Success = true;
                    return new Point3d(PassedPoint.X, PassedPoint.Y, PassedPoint.Z);
                }
                else
                {
                    Success = false;
                    return default(Point3d);
                }
            }

            public override bool PointsInScene(List<Hare.Geometry.Point> PTS)
            {
                Point3d Max = Voxels.OverallBounds().Max;
                Point3d Min = Voxels.OverallBounds().Min;
                foreach (Hare.Geometry.Point P in PTS)
                {
                    if (P.x < Min.X || P.x > Max.X || P.y < Min.Y || P.y > Max.Y || P.z < Min.Z || P.z > Max.Z) return false;
                }
                return true;
            }

            public override bool IsPlanar(int i)
            {
                return PlaneBoolean[i];
            }

            //public override Vector3d Normal(double u, double v, int i)
            //{
            //    if (PlaneBoolean[i])
            //    {
            //        return  PlanarNormal[i];
            //    }
            //    else
            //    {
            //        return this.BrepList[i].Faces[0].NormalAt(u, v);
            //    }
            //}

            //public Vector3d Normal(double u, double v, int i)
            //{
            //    if (PlaneBoolean[i])
            //    {
            //        return PlanarNormal[i];
            //    }
            //    else
            //    {
            //        return this.BrepList[i].Faces[0].NormalAt(u, v);
            //    }
            //}

            public override Hare.Geometry.Vector Normal(int i, double u, double v)
            {
                if (PlaneBoolean[i])
                {
                    return new Hare.Geometry.Vector(PlanarNormal[i].X, PlanarNormal[i].Y, PlanarNormal[i].Z);
                }
                else
                {
                    Vector3d LocalNormal = this.BrepList[i].Faces[0].NormalAt(u, v);
                    return new Hare.Geometry.Vector(LocalNormal.X, LocalNormal.Y, LocalNormal.Z);
                }
            }

            public override Hare.Geometry.Vector Normal(int i)
            {
                throw new Exception("(U,V) coordinates required to find the normal of a NURBS surface.");
            }

            public override double SurfaceArea(int x)
            {
                return Area[x];
            }

            public override Point3d ClosestPt(Point3d P, ref double Dist)
            {
                double Max = double.MaxValue;
                Point3d RP = new Point3d();
                foreach (Brep Srf in BrepList)
                {
                    double s = 0, t = 0;
                    if (!Srf.Faces[0].ClosestPoint(P, out s, out t)) continue;
                    Point3d CPoint = Srf.Faces[0].PointAt(s, t);
                    Dist = CPoint.DistanceTo(P);
                    if (Dist < Max)
                    {
                        RP = CPoint;
                        Max = Dist;
                    }
                }
                return RP;
            }

            public override Hare.Geometry.Point ClosestPt(Hare.Geometry.Point P, ref double Dist)
            {
                Point3d PR = new Point3d();
                PR = ClosestPt(new Point3d(P.x, P.y, P.z), ref Dist);
                return new Hare.Geometry.Point(PR.X, PR.Y, PR.Z);
            }

            /// <summary>
            /// Returns the indexed Boundary Representation object.
            /// </summary>
            /// <returns></returns>
            public List<Brep> Breps()
            {
                return BrepList;
            }

            /// <summary>
            /// Returns the indexed Boundary Representation object.
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public Brep Brep(int x)
            {
                return BrepList[x];
            }

            //public override void Plane_Intersection(Point Origin, Vector Normal, int[] SrfIDs, ref List<double> dist2, List<Vector> Dir, List<int> IDs)
            //{
            //    foreach (int i in SrfIDs)
            //    {
            //        double u = 0, v = 0;
            //        SurfaceArray[i].GetClosestPoint(Utilities.PachTools.HPttoRPt(Origin), ref u, ref v);    
            //        Vector3d N = new Vector3d();
            //        SurfaceArray[i].EvNormal(u, v, ref N);
            //        Vector tan = Hare_math.Cross(Normal, Utilities.PachTools.RPttoHPt((Point3d)N));
            //        Point P = Origin + tan * 0.01;
            //        ///There has got to be a better way...
            //    }
            //}

            public override void EdgeFrame_Tangents(Hare.Geometry.Point Origin, Vector Normal, int[] PlaneIDs, ref List<double> dist2, List<Vector> Dir, List<int> IDs)
            {
                throw new NotImplementedException();//TODO:
            }

            public override int Count()
            {
                return BrepList.Count;
            }

            public override Hare.Geometry.Point Max()
            {
                if (this.Voxels == null) return null;
                return Utilities.PachTools.RPttoHPt(Voxels.OverallBounds().Max);
            }

            public override Hare.Geometry.Point Min()
            {
                if (this.Voxels == null) return null;
                return Utilities.PachTools.RPttoHPt(Voxels.OverallBounds().Min);
            }

            public override double Sound_speed(Hare.Geometry.Point pt)
            {
                return this.Env_Prop.Sound_Speed(pt);
            }

            public override double Sound_speed(int arg)
            {
                return this.Env_Prop.Sound_Speed(arg);
            }
        }
    }
}
