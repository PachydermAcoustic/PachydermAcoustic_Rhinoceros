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
using Hare.Geometry;
using Rhino.Geometry;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        /// <summary>
        /// Scene base class. Do not use.
        /// </summary>C:\Users\User\Desktop\Pachyderm_Acoustic_2.0\Pachyderm_Acoustic\Classes_Scenes.cs
        [Serializable]
        public abstract class Scene
        {
            protected List<Material> AbsorptionData = new List<Material>();
            protected List<Scattering> ScatteringData = new List<Scattering>();
            protected List<double[]> TransmissionData = new List<double[]>();
            protected List<bool> Transmissive = new List<bool>();
            protected double TempC_S;
            protected double Pa_S;
            protected double hr_S;
            protected bool EdgeFC;
            protected int AC_S;
            protected double[] Area;
            protected Medium_Properties Env_Prop;
            protected double SplitRatio = 0.25;
            public System.Windows.Forms.DialogResult Status = System.Windows.Forms.DialogResult.OK;
            public Random R_Seed;
            public bool Valid;
            public bool Custom_Method;
            public bool Complete = true;
            public bool Partitioned = false;
            public bool Standard_Normals = false;
            protected List<BrepEdge> EdgeList = new List<BrepEdge>();
            public List<Edge> Edge_Nodes = new List<Edge>();
            public bool IsHomogeneous = true;
            public bool hasnulllayers = false;
            #region Inheritables

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Temp">temperature in Celsius</param>
            /// <param name="hr">relative humidity in percent</param>
            /// <param name="Pa">Pressure in Pascals</param>
            /// <param name="Air_Choice"></param>
            /// <param name="EdgeCorrection"></param>
            /// <param name="IsAcoustic"></param>
            public Scene(double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool IsAcoustic)
            {
                Custom_Method = IsAcoustic;
                Valid = false;
                TempC_S = Temp;
                Pa_S = Pa;          //TODO: Check that the pressure is in the correct units... hpa, kpa, pa.
                hr_S = hr;
                AC_S = Air_Choice;
                EdgeFC = EdgeCorrection;
                R_Seed = new Random();

                double TK = Temp + 273.15;
                //convert to Kelvins
                //Psat = Pr * 10 ^ (-6.8346 * (To1 / T) ^ 1.261 + 4.6151)
                double Psat = 101.325 * Math.Pow(10, (-6.8346 * Math.Pow((273.16 / TK), 1.261) + 4.6151));
                //6.1121 * Math.Exp((18.678 - T / 234.5) * TC / (257.14 + T))
                double h = hr * (Psat / Pa);

                //C_Sound = Utilities.AcousticalMath.SoundSpeed(Temp);

                Env_Prop = new Uniform_Medium(Air_Choice, Pa, TK, hr, EdgeCorrection);

                // Saturation water vapor pressure:
                // psv = exp(aa(1,1)*T^2 + aa(2,1)*T + aa(3,1) + aa(4,1)/T); % Formula Giacomo
                double psv = 101325 * Math.Pow(10, (4.6151 - 6.8346 * Math.Pow(((273.15 + 0.01) / (Temp + 273.15)), 1.261)));     // Formula ISO 9613-1:1993

                // Enhancement factor:
                double fpst = 1.00062 + 3.14e-8 * Atmospheric_Pressure + 5.6e-7 * Temp * Temp;

                // Mole fraction of water vapor in air:
                double xw = Relative_Humidity * psv * fpst / (100 * Atmospheric_Pressure);

                // Compressibility factor:
                double Z = 1 - Atmospheric_Pressure / (Temp + 273.15) * (1.58123e-6 + -2.9331e-8 * +1.1043e-10 * Temp * Temp + (5.707e-6 + -2.051e-8) * xw + (1.9898e-4 + -2.376e-6) * xw * xw) + Math.Pow((Atmospheric_Pressure / (Temp + 273.15)), 2) * (1.83e-11 + -0.765e-8 * xw * xw);

                //// Density of air:
                //rho = 3.48349 * 1e-3 * Atmospheric_Pressure / (Z * (Temp + 273.15)) * (1 - 0.3780 * xw);

                //Get_Edges();
            }

            public virtual void Standardize_Normals()
            {
                Standard_Normals = true;
                if (!Partitioned) throw new Exception("Normals can not be standardized until the model has been partitioned.");
            }

            public int EdgeCount
            {
                get
                {
                    return this.Edge_Nodes.Count;
                }
            }

            /// <summary>
            /// cast a ray within the model.
            /// </summary>
            /// <param name="Start">the origin of the ray</param>
            /// <param name="Dir">the direction vector of the ray</param>
            /// <param name="Random">a random number used to differentiate the ray from others that were cast before</param>
            /// <param name="u">u coordinate for NURBS tracing</param>
            /// <param name="v">v coordinate for NURBS tracing</param>
            /// <param name="Srf_ID">the surface of intersection</param>
            /// <param name="X_PT">the point of intersection</param>
            /// <param name="t">the distance traveled by the ray</param>
            /// <returns>true if successful, false if no hit</returns>
            public abstract bool shoot(Point3d Start, Vector3d Dir, int Random, out double u, out double v, out int Srf_ID, out List<Point3d> X_PT, out List<double> t, out List<int> code);
            public abstract bool shoot(Hare.Geometry.Ray R, out double u, out double v, out int Poly_ID, out Hare.Geometry.Point X_PT, out double t);
            /// <summary>
            /// cast a ray within the model.
            /// </summary>
            /// <param name="R">the ray to cast</param>
            /// <param name="u">u coordinate for NURBS tracing</param>
            /// <param name="v">v coordinate for NURBS tracing<9tgk,m iy'//param>
            /// <param name="Poly_ID">the surface of intersection</param>
            /// <param name="X_PT">the point of intersection</param>
            /// <param name="t">the distance traveled by the ray</param>
            /// <returns>true if successful, false if no hit</returns>
            public abstract bool shoot(Ray R, out double u, out double v, out int Poly_ID, out List<Hare.Geometry.Point> X_PT, out List<double> t, out List<int> code);
            /// <summary>
            /// The local normal of a surface.
            /// </summary>
            /// <param name="i">surface index</param>
            /// <param name="u">u coordinate for NURBS tracing</param>
            /// <param name="v">v coordinate for NURBS tracing</param>
            /// <returns></returns>
            public abstract Hare.Geometry.Vector Normal(int i, double u, double v);
            /// <summary>
            /// The local normal of a surface.
            /// </summary>
            /// <param name="i">surface index</param>
            /// <returns></returns>
            public abstract Hare.Geometry.Vector Normal(int i);
            ///// <summary>
            ///// The local normal of a surface.
            ///// </summary>
            ///// <param name="u">u coordinate for NURBS tracing</param>
            ///// <param name="v">v coordinate for NURBS tracing</param>
            ///// <param name="i">surface index</param>
            ///// <returns></returns>
            //public abstract Vector3d Normal(double u, double v, int i);
            /// <summary>
            /// Returns the surface area of a surface in the model.
            /// </summary>
            /// <param name="x">the index of the surface.</param>
            /// <returns></returns>
            public abstract double SurfaceArea(int x);
            /// <summary>
            /// Returns the number of surface objects in the model.
            /// </summary>
            /// <returns></returns>
            public abstract int Count();
            /// <summary>
            /// Optimizes the model using the chosen spatial partition system.
            /// </summary>
            public abstract void partition();
            /// <summary>
            /// Optimizes the model using the chosen spatial partition system.
            /// </summary>
            /// <param name="SP_Param">a parameter describing some aspect of the partition</param>
            public abstract void partition(int SP_Param);
            /// <summary>
            /// Optimizes the model using the chosen spatial partition system.
            /// </summary>
            /// <param name="P">points to add to the spatial partition system.</param>
            /// <param name="SP_PARAM">a parameter describing some aspect of the partition</param>
            public abstract void partition(List<Point3d> P, int SP_PARAM);
            /// <summary>
            ///a parameter describing some aspect of the partition
            /// </summary>
            /// <param name="P">points to add to the spatial partition system.</param>
            /// <param name="SP_PARAM">a parameter describing some aspect of the partition</param>
            public abstract void partition(List<Hare.Geometry.Point> P, int SP_PARAM);
            /// <summary>
            ///a parameter describing some aspect of the partition
            /// </summary>
            /// <param name="P">points to add to the spatial partition system.</param>
            /// <param name="SP_PARAM">a parameter describing some aspect of the partition</param>
            public abstract void partition(Point3d[] P, int SP_PARAM);
            /// <summary>
            ///a parameter describing some aspect of the partition
            /// </summary>
            /// <param name="P">points to add to the spatial partition system.</param>
            /// <param name="SP_PARAM">a parameter describing some aspect of the partition</param>
            public abstract void partition(Hare.Geometry.Point[] P, int SP_PARAM);
            /// <summary>
            /// Checks whether the specified surface is planar.
            /// </summary>
            /// <param name="i">the index of the surface.</param>
            /// <returns>true if planar, false if not.</returns>
            public abstract bool IsPlanar(int i);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="P"></param>
            /// <param name="D"></param>
            /// <returns></returns>
            public abstract Hare.Geometry.Point ClosestPt(Hare.Geometry.Point P, ref double D);
            /// <summary>
            /// Returns the closest point on the model to a point.
            /// </summary>
            /// <param name="P">the point</param>
            /// <param name="D">the distance between the point and the closest point</param>
            /// <returns>the closest point</returns>
            public abstract Point3d ClosestPt(Point3d P, ref double D);
            /// <summary>
            /// returns all points in the model.
            /// </summary>
            /// <param name="PTS"></param>
            /// <returns></returns>
            public abstract bool PointsInScene(List<Hare.Geometry.Point> PTS);
            public abstract string Scene_Type();
            public abstract void EdgeFrame_Tangents(Hare.Geometry.Point Origin, Hare.Geometry.Vector Normal, int[] SrfIDs, ref List<double> dist2, List<Vector> Dir, List<int> IDs);
            public abstract void Register_Edges(IEnumerable<Hare.Geometry.Point> S, IEnumerable<Hare.Geometry.Point> R);
            
            public void Register_Edges(IEnumerable<Point3d> S, IEnumerable<Point3d> R)
            {
                List<Hare.Geometry.Point> HS = new List<Hare.Geometry.Point>();
                List<Hare.Geometry.Point> HR = new List<Hare.Geometry.Point>();

                foreach (Point3d SPT in S) HS.Add(Utilities.PachTools.RPttoHPt(SPT));
                foreach (Point3d RPT in R) HR.Add(Utilities.PachTools.RPttoHPt(RPT));

                Register_Edges(HS, HR);
            }

            public void Register_Edges(IEnumerable<Source> S, Receiver_Bank R)
            {
                List<Hare.Geometry.Point> HS = new List<Hare.Geometry.Point>();
                List<Hare.Geometry.Point> HR = new List<Hare.Geometry.Point>();

                foreach (Source SPT in S) HS.Add(Utilities.PachTools.RPttoHPt(SPT.Origin()));
                foreach (Point3d RPT in R.Origins()) HR.Add(Utilities.PachTools.RPttoHPt(RPT));

                Register_Edges(HS, HR);
            }
            #endregion

            public abstract double Sound_speed(int arg);
            public abstract double Sound_speed(Hare.Geometry.Point pt);
            
            public bool Edge_Frequency
            {
                get
                {
                    return EdgeFC;
                }
            }

            public double Temperature
            {
                get
                {
                    return TempC_S;
                }
            }

            public double Relative_Humidity
            {
                get
                {
                    return hr_S;
                }
            }

            public double Atmospheric_Pressure
            {
                get
                {
                    return Pa_S;
                }
            }

            public int Attenuation_Method
            {
                get
                {
                    return AC_S;
                }
            }

            public void AttenuationFilter(int no_of_elements, int sampleFrequency, double dist, ref double[] Freq, ref double[] Atten, Hare.Geometry.Point pt)
            {
                Env_Prop.AttenuationFilter(no_of_elements, sampleFrequency, dist, ref Freq, ref Atten, pt);
            }

            public double AttenuationPureTone(Hare.Geometry.Point pt, double frequency)
            {
                return this.Env_Prop.AttenuationPureTone(pt, frequency);
            }

            public double[] Attenuation(int arg)
            {
                return this.Env_Prop.Attenuation_Coef(arg);
            }

            public double[] Attenuation(Hare.Geometry.Point pt)
            {
                return this.Env_Prop.Attenuation_Coef(pt);
            }

            public double Rho(int arg)
            {
                return this.Env_Prop.Rho(arg);
            }

            public double Rho(Hare.Geometry.Point pt)
            {
                return this.Env_Prop.Rho(pt);
            }

            public double Rho_C(int arg)
            {
                return this.Env_Prop.Rho_C(arg);
            }

            public double Rho_C(Hare.Geometry.Point pt)
            {
                return this.Env_Prop.Rho_C(pt);
            }

            public Material Surface_Material(int id)
            {
                return AbsorptionData[id];
            }

            public abstract void Absorb(ref BroadRay Ray, out double cos_theta, double u, double v);
            public abstract void Absorb(ref OctaveRay Ray, out double cos_theta, double u, double v);
            public abstract void Scatter_Early(ref BroadRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, double cos_theta, double u, double v);
            public abstract void Scatter_Late(ref OctaveRay Ray, ref Queue<OctaveRay> Rays, ref Random rand, double cos_theta, double u, double v);
            public abstract void Scatter_Simple(ref OctaveRay Ray, ref Random rand, double cos_theta, double u, double v);
            
            public List<Material> AbsorptionValue
            {
                get
                {
                    return AbsorptionData;
                }
            }

            public List<Scattering> ScatteringValue
            {
                get
                {
                    return ScatteringData;
                }
            }

            public List<double[]> TransmissionValue
            {
                get
                {
                    return TransmissionData;
                }
            }

            public List<bool> IsTransmissive
            {
                get
                {
                    return Transmissive;
                }
            }

            public abstract Hare.Geometry.Point Min();
            public abstract Hare.Geometry.Point Max();
        }

        [Serializable]
        public class Polygon_Scene : Scene
        {
            private Hare.Geometry.Topology[] Topo;
            private Hare.Geometry.Spatial_Partition SP;
            //Data kept to identify polygons as being part of larger planar entities...//
            private List<Vector3d[]> PDir = new List<Vector3d[]>();
            private List<double[]> Kurvatures = new List<double[]>();
            /// <summary>
            /// List of polygons with plane ids they are attributed.
            /// </summary>
            //private List<int> Plane_ID = new List<int>();
            /// <summary>
            /// List of planes with list of polygons attributed to each plane.
            /// </summary>
            //private List<List<int>> Plane_Members = new List<List<int>>();
            public double[] Plane_Area;
            private List<int> Brep_ids;
            private double[][] PolyPlaneFract;
            List<Brep> BrepList = new List<Brep>();
            ////////////////////////////////////////////////////////////////////
            public void Plot()
            {
                Utilities.PachTools.Plot_Hare_Topology(Topo[0]);
            }

            public Polygon_Scene(List<Rhino.DocObjects.RhinoObject> Objects, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool IsAcoustic)
                : base(Temp, hr, Pa, Air_Choice, EdgeCorrection, IsAcoustic)
            {
                Construct(Objects);
            }

            public Polygon_Scene(List<Rhino.DocObjects.RhinoObject> Objects, List<GeometryBase> Additional_Objs, List<int> AddObjs_Layer, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool IsAcoustic)
                : base(Temp, hr, Pa, Air_Choice, EdgeCorrection, IsAcoustic)
            {
                if (Additional_Objs.Count != AddObjs_Layer.Count) throw new InvalidOperationException("Number of Additional Objects must match number of additional object layer ids.");
                List<GeometryBase> objs = new List<GeometryBase>();
                List<int> LayerIds = new List<int>();

                foreach (Rhino.DocObjects.RhinoObject Ob in Objects)
                {
                    objs.Add(Ob.Geometry);
                    LayerIds.Add(Ob.Attributes.LayerIndex);
                }

                objs.AddRange(Additional_Objs);
                LayerIds.AddRange(AddObjs_Layer);

                Construct(objs, LayerIds);
            }

            private void Construct(List<GeometryBase> Breps, List<int> LayerIds)
            {
                BoundingBox Box = Breps[0].GetBoundingBox(true);
                for (int i = 1; i < Breps.Count; i++) Box.Union(Breps[i].GetBoundingBox(true));

                List<Brep> BList = new List<Brep>();
                Brep_ids = new List<int>();

                List<Material> Mat_Layer = new List<Material>();
                List<Scattering> Scat_Layer = new List<Scattering>();
                List<Material> Mat_Obj = new List<Material>();
                List<Scattering> Scat_Obj = new List<Scattering>();
                List<double[]> Trans_Layer = new List<double[]>();
                List<double[]> Trans_Obj = new List<double[]>();
                //Organize the geometry into Breps
                //Get materials for each layer:
                for (int l = 0; l < Rhino.RhinoDoc.ActiveDoc.Layers.Count; l++)
                {
                    Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[l];
                    string abstype = Layer.GetUserString("ABSType");
                    if (abstype == "Buildup")
                    {
                        string BU = Layer.GetUserString("BuildUp");
                        string[] BU_split = BU.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        List<AbsorptionModels.ABS_Layer> Buildup = new List<AbsorptionModels.ABS_Layer>();
                        foreach (string swatch in BU_split) Buildup.Add(AbsorptionModels.ABS_Layer.LayerFromCode(swatch));
                        Mat_Layer.Add(new Environment.Smart_Material(false, Buildup, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2));

                        double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
                        Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(Layer.GetUserString("Acoustics"), ref Abs, ref Scat, ref Trans);
                        ///Other properties are still coefficient based...
                        Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
                        Trans_Layer.Add(Trans);
                    }
                    else
                    {
                        string spec = Layer.GetUserString("Acoustics");

                        if (spec == "")
                        {
                            ///Layer is not used. As long as there is no geometry for pachyderm on this layer without object set properties, this is ok.
                            Mat_Layer.Add(null);
                            Scat_Layer.Add(null);
                            Trans_Layer.Add(null);
                            continue;
                        }

                        double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
                        Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(spec, ref Abs, ref Scat, ref Trans);
                        Mat_Layer.Add(new Environment.Basic_Material(Abs, new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 }));
                        Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
                        Trans_Layer.Add(Trans);
                    }
                }

                for (int q = 0; q <= Breps.Count - 1; q++)
                {
                    List<Brep> B = new List<Brep>();
                    if (Breps[q].ObjectType == Rhino.DocObjects.ObjectType.Brep || Breps[q].ObjectType == Rhino.DocObjects.ObjectType.Surface)
                    {
                        //string m = ObjectList[q].Geometry.GetUserString("Acoustics_User");

                        if (Breps[q].ObjectType == Rhino.DocObjects.ObjectType.Surface) B.Add((Breps[q] as Surface).ToBrep());
                        else B.Add(((Rhino.Geometry.Brep)Breps[q]).DuplicateBrep());

                        if (Breps[q].GetUserString("Acoustics_User") == "yes")
                        {
                            double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
                            Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(Breps[q].GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
                            Mat_Obj.Add(new Basic_Material(ABS, new double[] { 0, 0, 0, 0, 0, 0, 0, 0 }));
                            Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
                            Trans_Obj.Add(TRANS);
                        }
                        else
                        {
                            //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                            //AcousticsData.Add(Layer.GetUserString("Acoustics"));
                            if (Mat_Layer[LayerIds[q]] == null) hasnulllayers = true;

                            Mat_Obj.Add(Mat_Layer[LayerIds[q]]);
                            Scat_Obj.Add(Scat_Layer[LayerIds[q]]);
                            Trans_Obj.Add(Trans_Layer[LayerIds[q]]);
                        }
                    }
                    else if (Breps[q].ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        Rhino.Geometry.Brep BObj = ((Rhino.Geometry.Extrusion)Breps[q]).ToBrep();
                        for (int i = 0; i < BObj.Faces.Count; i++)
                        {
                            if (Breps[q].GetUserString("Acoustics_User") == "yes")
                            {
                                //AcousticsData.Add(ObjectList[q].Geometry.GetUserString("Acoustics"));
                                double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
                                Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(Breps[q].GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
                                Mat_Obj.Add(new Basic_Material(ABS, new double[] { 0, 0, 0, 0, 0, 0, 0, 0 }));
                                Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
                                Trans_Obj.Add(TRANS);
                            }
                            else
                            {
                                //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                                //AcousticsData.Add(Layer.GetUserString("Acoustics"));
                                //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                                //AcousticsData.Add(Layer.GetUserString("Acoustics"));
                                if (Mat_Layer[LayerIds[q]] == null) hasnulllayers = true;

                                Mat_Obj.Add(Mat_Layer[LayerIds[q]]);
                                Scat_Obj.Add(Scat_Layer[LayerIds[q]]);
                                Trans_Obj.Add(Trans_Layer[LayerIds[q]]);
                            }

                            //B.Add(BObj.Faces[0].ToBrep());
                            //for (int i = 1; i < BObj.Faces.Count; i++)
                            //{
                            //    if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
                            //    {
                            //        AcousticsData.Add(ObjectList[q].Geometry.GetUserString("Acoustics"));
                            //    }
                            //    else
                            //    {
                            //        Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                            //        AcousticsData.Add(Layer.GetUserString("Acoustics"));
                            //    }
                            B.Add(BObj.Faces[i].ToBrep());
                        }
                    }
                    else
                    {
                        continue;
                    }
                    BList.AddRange(B);
                }
                ////////////////////////////////////////
                Topo = new Hare.Geometry.Topology[1];
                Topo[0] = new Hare.Geometry.Topology(Utilities.PachTools.RPttoHPt(Box.Min), Utilities.PachTools.RPttoHPt(Box.Max));
                ////////////////////////////////////////
                for (int q = 0; q < BList.Count; q++)
                {
                    BrepList.Add(BList[q]);

                    //Material Abs = null;
                    //Scattering Scat = null;

                    //double[] Transparency = new double[8];
                    double[] Transmission = new double[8];

                    //double[] Scat = new double[8];
                    //if (!string.IsNullOrEmpty(AcousticsData[q]))
                    //if (Mat_Obj[q] != null)
                    //{
                    //    //double[] Absorption = new double[8];
                    //    //double[] phase = new double[8];
                    //    //double[] Scattering = new double[8];
                    //    ////double[,] Scattering = new double[8, 3];
                    //    //double[] Reflection = new double[8];
                    //    //UI.PachydermAc_PlugIn.DecodeAcoustics(AcousticsData[q], ref Absorption, ref Scattering, ref Transparency);
                    //    Abs = Mat_Obj[q];
                    //    Scat = Scat_Obj[q];
                    //    Transmission = Trans_Obj[q];
                    //}
                    //else
                    if ((Mat_Obj[q] == null) || (Scat_Obj[q] == null) || (Trans_Obj[q] == null))
                    {
                        if (!Custom_Method)
                        {
                            Status = System.Windows.Forms.MessageBox.Show("A material is not specified correctly. Please assign absorption and scattering to all layers in the model.", "Materials Error", System.Windows.Forms.MessageBoxButtons.OK);
                            Complete = false;
                            return;
                        }
                        ///Materials do not need to be specified, as it will not be used for an acoustical simulation... (hopefully...)
                    }

                    //for (int i = 0; i < 8; i++)
                    //{
                    //    Reflection[i] = (1 - Absorption[i]);
                    //    Transmission[i] = Transparency[i];
                    //    Scattering[i, 1] = Scat[i];
                    //    double Mod = ((Scattering[i, 1] < (1 - Scattering[i, 1])) ? (Scattering[i, 1] * SplitRatio / 2) : ((1 - Scattering[i, 1]) * SplitRatio / 2));
                    //    Scattering[i, 0] = Scattering[i, 1] - Mod;
                    //    Scattering[i, 2] = Scattering[i, 1] + Mod;
                    //    phase[i] = 0;
                    //}

                    Mesh[] meshes;
                    MeshingParameters mp = new MeshingParameters();
                    mp.MinimumEdgeLength = 0.1;
                    mp.MaximumEdgeLength = 1;
                    //mp.SimplePlanes = true;
                    meshes = Rhino.Geometry.Mesh.CreateFromBrep(BList[q], MeshingParameters.Smooth);
                    if (meshes == null) throw new Exception("Problem with meshes");

                    for (int t = 0; t < meshes.Length; t++)
                    {
                        if (meshes[t].Faces.Count < 1)
                        {
                            Status = System.Windows.Forms.MessageBox.Show("A surface in the model does not generate a rendermesh. This surface will not be represented in the simulation. It is recommended that you cancel this simulation and repair the affected surface. It can be located in shaded view by finding the surface which generates boundary and isoparm lines, but does not generate a fill. It can sometimes be repaired by running the command 'ShrinkTrimmedSurface'. If this does not work, it will have to be replaced by some means which would generate a proper surface.", "Surface without Rendermesh", System.Windows.Forms.MessageBoxButtons.OKCancel);
                            if (Status == System.Windows.Forms.DialogResult.Cancel)
                            {
                                Complete = false;
                                return;
                            }
                            continue;
                        }

                        for (int u = 0; u < meshes[t].Faces.Count; u++)
                        {
                            Hare.Geometry.Point[] P;
                            if (meshes[t].Faces[u].IsQuad)
                            {
                                P = new Hare.Geometry.Point[4];
                                Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
                                P[0] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
                                P[1] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
                                P[2] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                FP = meshes[t].Vertices[meshes[t].Faces[u][3]];
                                P[3] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                            }
                            else
                            {
                                P = new Hare.Geometry.Point[3];
                                Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
                                P[0] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
                                P[1] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
                                P[2] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                            }

                            //ReflectionData.Add(Reflection);
                            AbsorptionData.Add(Mat_Obj[q]);
                            ScatteringData.Add(Scat_Obj[q]);
                            TransmissionData.Add(Trans_Obj[q]);
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
                            if (BrepList[q].Faces[t].IsPlanar())
                            {
                                Topo[0].Add_Polygon(P);
                                Brep_ids.Add(q);
                            }
                            else
                            {
                                Topo[0].Add_Polygon(new Hare.Geometry.Point[3] { P[0], P[1], P[2] });
                                Brep_ids.Add(q);
                                if (P.Length > 3)
                                {//ReflectionData.Add(Reflection);
                                    AbsorptionData.Add(Mat_Obj[q]);
                                    ScatteringData.Add(Scat_Obj[q]);
                                    TransmissionData.Add(Trans_Obj[q]);
                                    Transmissive.Add(Trans);
                                    //PhaseData.Add(phase);
                                    Topo[0].Add_Polygon(new Hare.Geometry.Point[3] { P[0], P[2], P[3] });
                                    Brep_ids.Add(q);
                                }
                            }
                        }
                    }
                }

                //Set up a system to find random points on planes.//
                Plane_Area = new double[Topo[0].Plane_Members.Length];
                PolyPlaneFract = new double[Topo[0].Plane_Members.Length][];

                for (int q = 0; q < Topo[0].Plane_Members.Length; q++)
                {
                    foreach (int t in Topo[0].Plane_Members[q])
                    {
                        Plane_Area[q] += Topo[0].Polygon_Area(t);
                    }
                }

                //////////////////////////
                for (int i = 0; i < Topo[0].planeList.Count; i++)
                    for (int j = 0; j < Topo[0].Plane_Members[i].Count; j++)
                    {
                        Point3d pt = Utilities.PachTools.HPttoRPt(Topo[0].Polygon_Centroid(Topo[0].Plane_Members[i][j]));
                        string n = Topo[0].Polys[Topo[0].Plane_Members[i][j]].Plane_ID.ToString();
                    }
                //////////////////////////

                for (int q = 0; q < Topo[0].Plane_Members.Length; q++)
                {
                    PolyPlaneFract[q] = new double[Topo[0].Plane_Members[q].Count];
                    PolyPlaneFract[q][0] = Topo[0].Polygon_Area(Topo[0].Plane_Members[q][0]) / Plane_Area[q];
                    for (int t = 1; t < Topo[0].Plane_Members[q].Count; t++)
                    {
                        PolyPlaneFract[q][t] += PolyPlaneFract[q][t - 1] + Topo[0].Polygon_Area(Topo[0].Plane_Members[q][t]) / Plane_Area[q];
                    }
                }
                Valid = true;
                Rhino.RhinoDoc.ActiveDoc.Objects.Add(Utilities.PachTools.Hare_to_RhinoMesh(Topo[0]));
            }

            private void Construct(List<Rhino.DocObjects.RhinoObject> ObjectList)
            {
                BoundingBox Box = ObjectList[0].Geometry.GetBoundingBox(true);
                for (int i = 1; i < ObjectList.Count; i++) Box.Union(ObjectList[i].Geometry.GetBoundingBox(true));

                List<GeometryBase> BList = new List<GeometryBase>();

                Brep_ids = new List<int>();

                List<Material> Mat_Layer = new List<Material>();
                List<Scattering> Scat_Layer = new List<Scattering>();
                List<Material> Mat_Obj = new List<Material>();
                List<Scattering> Scat_Obj = new List<Scattering>();
                List<double[]> Trans_Layer = new List<double[]>();
                List<double[]> Trans_Obj = new List<double[]>();
                List<bool> Finite_Layers = new List<bool>();
                List<bool> Finite_Obj = new List<bool>();
                //Organize the geometry into Breps
                //Get materials for each layer:
                for (int l = 0; l < Rhino.RhinoDoc.ActiveDoc.Layers.Count; l++)
                {
                    Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[l];
                    string abstype = Layer.GetUserString("ABSType");
                    if (abstype == "Buildup")
                    {
                        Finite_Layers.Add(false);
                        string BU = Layer.GetUserString("BuildUp");
                        string[] BU_split = BU.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        List<AbsorptionModels.ABS_Layer> Buildup = new List<AbsorptionModels.ABS_Layer>();
                        foreach (string swatch in BU_split) Buildup.Add(AbsorptionModels.ABS_Layer.LayerFromCode(swatch));
                        Mat_Layer.Add(new Environment.Smart_Material(false, Buildup, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2));

                        double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
                        Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(Layer.GetUserString("Acoustics"), ref Abs, ref Scat, ref Trans);
                        ///Other properties are still coefficient based...
                        Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
                        Trans_Layer.Add(Trans);
                    }
                    else if (abstype == "Buildup_Finite")
                    {
                        Finite_Layers.Add(true);
                        string BU = Layer.GetUserString("BuildUp");
                        string[] BU_split = BU.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        List<AbsorptionModels.ABS_Layer> Buildup = new List<AbsorptionModels.ABS_Layer>();
                        foreach (string swatch in BU_split) Buildup.Add(AbsorptionModels.ABS_Layer.LayerFromCode(swatch));
                        Environment.Smart_Material sm = new Environment.Smart_Material(false, Buildup, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2);
                        Mat_Layer.Add(sm);
                        double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
                        Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(Layer.GetUserString("Acoustics"), ref Abs, ref Scat, ref Trans);
                        ///Other properties are still coefficient based...
                        Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
                        Trans_Layer.Add(Trans);
                    }
                    else
                    {
                        Finite_Layers.Add(false);
                        string spec = Layer.GetUserString("Acoustics");

                        if (spec == "")
                        {
                            ///Layer is not used. As long as there is no geometry for pachyderm on this layer without object set properties, this is ok.
                            Mat_Layer.Add(null);
                            Scat_Layer.Add(null);
                            Trans_Layer.Add(null);
                            continue;
                        }

                        double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
                        Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(spec, ref Abs, ref Scat, ref Trans);
                        Mat_Layer.Add(new Environment.Basic_Material(Abs, new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 }));
                        Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
                        Trans_Layer.Add(Trans);
                    }
                }

                for (int q = 0; q <= ObjectList.Count - 1; q++)
                {
                    List<Brep> B = new List<Brep>();
                    if (ObjectList[q].ObjectType == Rhino.DocObjects.ObjectType.Brep)
                    {
                        Rhino.DocObjects.BrepObject BObj = ((Rhino.DocObjects.BrepObject)ObjectList[q]);
                        B.Add(BObj.BrepGeometry.DuplicateBrep());
                        //string m = ObjectList[q].Geometry.GetUserString("Acoustics_User");
                        if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
                        {
                            double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
                            Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(ObjectList[q].Geometry.GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
                            Mat_Obj.Add(new Basic_Material(ABS, new double[] { 0, 0, 0, 0, 0, 0, 0, 0 }));
                            Finite_Obj.Add(false);
                            Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
                            Trans_Obj.Add(TRANS);
                        }
                        else
                        {
                            //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                            //AcousticsData.Add(Layer.GetUserString("Acoustics"));
                            Mat_Obj.Add(Mat_Layer[ObjectList[q].Attributes.LayerIndex]);
                            Scat_Obj.Add(Scat_Layer[ObjectList[q].Attributes.LayerIndex]);
                            Trans_Obj.Add(Trans_Layer[ObjectList[q].Attributes.LayerIndex]);
                            Finite_Obj.Add(Finite_Layers[ObjectList[q].Attributes.LayerIndex]);
                        }
                    }
                    else if (ObjectList[q].ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        Rhino.Geometry.Brep BObj = ((Rhino.DocObjects.ExtrusionObject)ObjectList[q]).ExtrusionGeometry.ToBrep();
                        for (int i = 0; i < BObj.Faces.Count; i++)
                        {
                            if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
                            {
                                //AcousticsData.Add(ObjectList[q].Geometry.GetUserString("Acoustics"));
                                double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
                                Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(ObjectList[q].Geometry.GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
                                Mat_Obj.Add(new Basic_Material(ABS, new double[] { 0, 0, 0, 0, 0, 0, 0, 0 }));
                                Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
                                Trans_Obj.Add(TRANS);
                                Finite_Obj.Add(false);
                            }
                            else
                            {
                                //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                                //AcousticsData.Add(Layer.GetUserString("Acoustics"));
                                //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                                //AcousticsData.Add(Layer.GetUserString("Acoustics"));
                                Mat_Obj.Add(Mat_Layer[ObjectList[q].Attributes.LayerIndex]);
                                Scat_Obj.Add(Scat_Layer[ObjectList[q].Attributes.LayerIndex]);
                                Trans_Obj.Add(Trans_Layer[ObjectList[q].Attributes.LayerIndex]);
                                Finite_Obj.Add(Finite_Layers[ObjectList[q].Attributes.LayerIndex]);
                            }

                            //B.Add(BObj.Faces[0].ToBrep());
                            //for (int i = 1; i < BObj.Faces.Count; i++)
                            //{
                            //    if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
                            //    {
                            //        AcousticsData.Add(ObjectList[q].Geometry.GetUserString("Acoustics"));
                            //    }
                            //    else
                            //    {
                            //        Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
                            //        AcousticsData.Add(Layer.GetUserString("Acoustics"));
                            //    }
                            B.Add(BObj.Faces[i].ToBrep());
                        }
                    }
                    else
                    {
                        continue;
                    }
                    BList.AddRange(B);
                }

                ////////////////////////////////////////
                Topo = new Hare.Geometry.Topology[1];
                Topo[0] = new Topology(Utilities.PachTools.RPttoHPt(Box.Min), Utilities.PachTools.RPttoHPt(Box.Max));
                ////////////////////////////////////////
                for (int q = 0; q < BList.Count; q++)
                {
                    for (int r = 0; r < ((Brep)BList[q]).Faces.Count; r++)
                    {
                        BrepList.Add(((Brep)BList[q]).Faces[r].DuplicateFace(false));

                        //Material Abs = null ;
                        //Scattering Scat = null;

                        //double[] Transparency = new double[8];
                        double[] Transmission = new double[8];

                        //double[] Scat = new double[8];
                        //if (!string.IsNullOrEmpty(AcousticsData[q]))
                        //if (Mat_Obj[q] != null)
                        //{
                        //    //double[] Absorption = new double[8];
                        //    //double[] phase = new double[8];
                        //    //double[] Scattering = new double[8];
                        //    ////double[,] Scattering = new double[8, 3];
                        //    //double[] Reflection = new double[8];
                        //    //UI.PachydermAc_PlugIn.DecodeAcoustics(AcousticsData[q], ref Absorption, ref Scattering, ref Transparency);
                        //    Abs = Mat_Obj[q];
                        //    Scat = Scat_Obj[q];
                        //    Transmission = Trans_Obj[q];
                        //}
                        //else
                        if ((Mat_Obj[q] == null) || (Scat_Obj[q] == null) || (Trans_Obj[q] == null))
                        {
                            if (!Custom_Method)
                            {
                                Status = System.Windows.Forms.MessageBox.Show("A material is not specified correctly. Please assign absorption and scattering to all layers in the model.", "Materials Error", System.Windows.Forms.MessageBoxButtons.OK);
                                Complete = false;
                                return;
                            }
                            ///Materials do not need to be specified, as it will not be used for an acoustical simulation... (hopefully...)
                        }

                        //for (int i = 0; i < 8; i++)
                        //{
                        //    Reflection[i] = (1 - Absorption[i]);
                        //    Transmission[i] = Transparency[i];
                        //    Scattering[i, 1] = Scat[i];
                        //    double Mod = ((Scattering[i, 1] < (1 - Scattering[i, 1])) ? (Scattering[i, 1] * SplitRatio / 2) : ((1 - Scattering[i, 1]) * SplitRatio / 2));
                        //    Scattering[i, 0] = Scattering[i, 1] - Mod;
                        //    Scattering[i, 2] = Scattering[i, 1] + Mod;
                        //    phase[i] = 0;
                        //}

                        Mesh[] meshes;
                        MeshingParameters mp = new MeshingParameters();
                        if (Finite_Obj[q])
                        {
                            mp.MinimumEdgeLength = 0.1;
                            mp.SimplePlanes = false;
                        }
                        else
                        {
                            mp.MinimumEdgeLength = 0.1;
                            mp.SimplePlanes = true;
                        }

                        meshes = Rhino.Geometry.Mesh.CreateFromBrep((Brep)BrepList[BrepList.Count - 1], mp);
                        if (meshes == null) throw new Exception("Problem with meshes");

                        for (int t = 0; t < meshes.Length; t++)
                        {
                            if (meshes[t].Faces.Count < 1)
                            {
                                Status = System.Windows.Forms.MessageBox.Show("A surface in the model does not generate a rendermesh. This surface will not be represented in the simulation. It is recommended that you cancel this simulation and repair the affected surface. It can be located in shaded view by finding the surface which generates boundary and isoparm lines, but does not generate a fill. It can sometimes be repaired by running the command 'ShrinkTrimmedSurface'. If this does not work, it will have to be replaced by some means which would generate a proper surface.", "Surface without Rendermesh", System.Windows.Forms.MessageBoxButtons.OKCancel);
                                if (Status == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    Complete = false;
                                    return;
                                }
                                continue;
                            }

                            for (int u = 0; u < meshes[t].Faces.Count; u++)
                            {
                                Hare.Geometry.Point[] P;
                                if (meshes[t].Faces[u].IsQuad)
                                {
                                    P = new Hare.Geometry.Point[4];
                                    Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
                                    P[0] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
                                    P[1] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
                                    P[2] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][3]];
                                    P[3] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                }
                                else
                                {
                                    P = new Hare.Geometry.Point[3];
                                    Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
                                    P[0] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
                                    P[1] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
                                    P[2] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
                                }

                                if (Finite_Obj[q])
                                {
                                    if (!(Mat_Obj[q] is Smart_Material)) throw new Exception("Finite Material must have a Smart_Material...");
                                    Smart_Material mat = Mat_Obj[q] as Smart_Material;
                                    AbsorptionData.Add(new Finite_Material(mat, BrepList[q], meshes[t], u, Env_Prop));
                                }
                                else AbsorptionData.Add(Mat_Obj[q]);
                                ScatteringData.Add(Scat_Obj[q]);
                                TransmissionData.Add(Trans_Obj[q]);

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
                                if (BrepList[BrepList.Count - 1].Faces[t].IsPlanar())
                                {
                                    Topo[0].Add_Polygon(P);
                                    Brep_ids.Add(BrepList.Count - 1);
                                }
                                else
                                {
                                    Topo[0].Add_Polygon(new Hare.Geometry.Point[3] { P[0], P[1], P[2] });
                                    Brep_ids.Add(BrepList.Count - 1);
                                    if (P.Length > 3)
                                    {
                                        //break this quad into two polygons in order to avoid warping...
                                        if (Finite_Obj[q])
                                        {
                                            if (!(Mat_Obj[q] is Smart_Material)) throw new Exception("Finite Material must have a Smart_Material...");
                                            Smart_Material mat = Mat_Obj[q] as Smart_Material;
                                            AbsorptionData.Add(new Finite_Material(mat, BrepList[q], meshes[t], u, Env_Prop));
                                        }
                                        else AbsorptionData.Add(Mat_Obj[q]);
                                        ScatteringData.Add(Scat_Obj[q]);
                                        TransmissionData.Add(Trans_Obj[q]);
                                        Transmissive.Add(Trans);
                                        Topo[0].Add_Polygon(new Hare.Geometry.Point[3] { P[0], P[2], P[3] });
                                        Brep_ids.Add(BrepList.Count - 1);
                                    }
                                }
                            }
                        }
                    }
                }

                //Set up a system to find random points on planes.//
                Plane_Area = new double[Topo[0].Plane_Members.Length];
                PolyPlaneFract = new double[Topo[0].Plane_Members.Length][];

                for (int q = 0; q < Topo[0].Plane_Members.Length; q++)
                {
                    foreach (int t in Topo[0].Plane_Members[q])
                    {
                        Plane_Area[q] += Topo[0].Polygon_Area(t);
                    }
                }

                //////////////////////////
                for (int i = 0; i < Topo[0].planeList.Count; i++)
                    for (int j = 0; j < Topo[0].Plane_Members[i].Count; j++)
                    {
                        Point3d pt = Utilities.PachTools.HPttoRPt(Topo[0].Polygon_Centroid(Topo[0].Plane_Members[i][j]));
                        string n = Topo[0].Polys[Topo[0].Plane_Members[i][j]].Plane_ID.ToString();
                    }
                //////////////////////////

                for (int q = 0; q < Topo[0].Plane_Members.Length; q++)
                {
                    PolyPlaneFract[q] = new double[Topo[0].Plane_Members[q].Count];
                    PolyPlaneFract[q][0] = Topo[0].Polygon_Area(Topo[0].Plane_Members[q][0]) / Plane_Area[q];
                    for (int t = 1; t < Topo[0].Plane_Members[q].Count; t++)
                    {
                        PolyPlaneFract[q][t] += PolyPlaneFract[q][t - 1] + Topo[0].Polygon_Area(Topo[0].Plane_Members[q][t]) / Plane_Area[q];
                    }
                }
                Valid = true;

                //Utilities.PachTools.Plot_Hare_Topology(Topo[0]);
            }

            //private void Construct(List<Rhino.DocObjects.RhinoObject> ObjectList)
            //{
            //    BoundingBox Box = ObjectList[0].Geometry.GetBoundingBox(true);
            //    for (int i = 1; i < ObjectList.Count; i++) Box.Union(ObjectList[i].Geometry.GetBoundingBox(true));

            //    //List<Hare.Geometry.Point[]> FaceList = new List<Hare.Geometry.Point[]>();
            //    //int p = -1;
            //    List<GeometryBase> BList = new List<GeometryBase>();
            //    //List<string> AcousticsData = new List<string>();
                
            //    Brep_ids = new List<int>();

            //    List<Material> Mat_Layer = new List<Material>();
            //    List<Scattering> Scat_Layer = new List<Scattering>();
            //    List<Material> Mat_Obj = new List<Material>();
            //    List<Scattering> Scat_Obj = new List<Scattering>();
            //    List<double[]> Trans_Layer = new List<double[]>();
            //    List<double[]>Trans_Obj = new List<double[]>();
            //    //Organize the geometry into Breps
            //    //Get materials for each layer:
            //    for (int l = 0; l < Rhino.RhinoDoc.ActiveDoc.Layers.Count; l++)
            //    {
            //        Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[l];
            //        string abstype = Layer.GetUserString("ABSType");
            //        if (abstype == "Buildup")
            //        {
            //            string BU = Layer.GetUserString("BuildUp");
            //            string[] BU_split = BU.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //            List<AbsorptionModels.ABS_Layer> Buildup = new List<AbsorptionModels.ABS_Layer>();
            //            foreach (string swatch in BU_split) Buildup.Add(AbsorptionModels.ABS_Layer.LayerFromCode(swatch));
            //            Mat_Layer.Add(new Environment.Smart_Material(Buildup, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2));

            //            double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
            //            Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(Layer.GetUserString("Acoustics"), ref Abs, ref Scat, ref Trans);
            //            ///Other properties are still coefficient based...
            //            Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
            //            Trans_Layer.Add(Trans);
            //        }
            //        else
            //        {
            //            string spec = Layer.GetUserString("Acoustics");

            //            if(spec == "")
            //            {
            //                ///Layer is not used. As long as there is no geometry for pachyderm on this layer without object set properties, this is ok.
            //                Mat_Layer.Add(null);
            //                Scat_Layer.Add(null);
            //                Trans_Layer.Add(null);
            //                continue;
            //            }

            //            double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
            //            Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(spec, ref Abs, ref Scat, ref Trans);
            //            Mat_Layer.Add(new Environment.Basic_Material(Abs, new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 }));
            //            Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
            //            Trans_Layer.Add(Trans);
            //        }
            //    }

            //    for (int q = 0; q <= ObjectList.Count - 1; q++)
            //    {
            //        List<Brep> B = new List<Brep>();
            //        if (ObjectList[q].ObjectType == Rhino.DocObjects.ObjectType.Brep)
            //        {
            //            Rhino.DocObjects.BrepObject BObj = ((Rhino.DocObjects.BrepObject)ObjectList[q]);
            //            B.Add(BObj.BrepGeometry.DuplicateBrep());
            //            //string m = ObjectList[q].Geometry.GetUserString("Acoustics_User");
            //            if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
            //            {
            //                double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
            //                Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(ObjectList[q].Geometry.GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
            //                Mat_Obj.Add(new Basic_Material(ABS, new double[]{0,0,0,0,0,0,0,0}));
            //                Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
            //                Trans_Obj.Add(TRANS);
            //            }
            //            else
            //            {
            //                //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
            //                //AcousticsData.Add(Layer.GetUserString("Acoustics"));
            //                Mat_Obj.Add(Mat_Layer[ObjectList[q].Attributes.LayerIndex]);
            //                Scat_Obj.Add(Scat_Layer[ObjectList[q].Attributes.LayerIndex]);
            //                Trans_Obj.Add(Trans_Layer[ObjectList[q].Attributes.LayerIndex]);
            //            }
            //        }
            //        else if (ObjectList[q].ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
            //        {
            //            Rhino.Geometry.Brep BObj = ((Rhino.DocObjects.ExtrusionObject)ObjectList[q]).ExtrusionGeometry.ToBrep();
            //            for (int i = 0; i < BObj.Faces.Count; i++)
            //            {
            //                if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
            //                {
            //                    //AcousticsData.Add(ObjectList[q].Geometry.GetUserString("Acoustics"));
            //                    double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
            //                    Pachyderm_Acoustic.UI.PachydermAc_PlugIn.DecodeAcoustics(ObjectList[q].Geometry.GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
            //                    Mat_Obj.Add(new Basic_Material(ABS, new double[] { 0, 0, 0, 0, 0, 0, 0, 0 }));
            //                    Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
            //                    Trans_Obj.Add(TRANS);
            //                }
            //                else
            //                {
            //                    //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
            //                    //AcousticsData.Add(Layer.GetUserString("Acoustics"));
            //                    //Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
            //                    //AcousticsData.Add(Layer.GetUserString("Acoustics"));
            //                    Mat_Obj.Add(Mat_Layer[ObjectList[q].Attributes.LayerIndex]);
            //                    Scat_Obj.Add(Scat_Layer[ObjectList[q].Attributes.LayerIndex]);
            //                    Trans_Obj.Add(Trans_Layer[ObjectList[q].Attributes.LayerIndex]);
            //                }

            //                //B.Add(BObj.Faces[0].ToBrep());
            //                //for (int i = 1; i < BObj.Faces.Count; i++)
            //                //{
            //                //    if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
            //                //    {
            //                //        AcousticsData.Add(ObjectList[q].Geometry.GetUserString("Acoustics"));
            //                //    }
            //                //    else
            //                //    {
            //                //        Rhino.DocObjects.Layer Layer = Rhino.RhinoDoc.ActiveDoc.Layers[ObjectList[q].Attributes.LayerIndex];
            //                //        AcousticsData.Add(Layer.GetUserString("Acoustics"));
            //                //    }
            //                B.Add(BObj.Faces[i].ToBrep());
            //            }
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //        BList.AddRange(B);
            //    }

            //    ////////////////////////////////////////
            //    Topo = new Hare.Geometry.Topology[1];
            //    Topo[0] = new Topology(Utilities.PachTools.RPttoHPt(Box.Min), Utilities.PachTools.RPttoHPt(Box.Max));
            //    ////////////////////////////////////////
            //    for (int q = 0; q < BList.Count; q++)
            //    {
            //        BrepList.Add((Brep)BList[q]);

            //        //Material Abs = null ;
            //        //Scattering Scat = null;

            //        //double[] Transparency = new double[8];
            //        double[] Transmission = new double[8];
                    
            //        //double[] Scat = new double[8];
            //        //if (!string.IsNullOrEmpty(AcousticsData[q]))
            //        //if (Mat_Obj[q] != null)
            //        //{
            //        //    //double[] Absorption = new double[8];
            //        //    //double[] phase = new double[8];
            //        //    //double[] Scattering = new double[8];
            //        //    ////double[,] Scattering = new double[8, 3];
            //        //    //double[] Reflection = new double[8];
            //        //    //UI.PachydermAc_PlugIn.DecodeAcoustics(AcousticsData[q], ref Absorption, ref Scattering, ref Transparency);
            //        //    Abs = Mat_Obj[q];
            //        //    Scat = Scat_Obj[q];
            //        //    Transmission = Trans_Obj[q];
            //        //}
            //        //else
            //        if ((Mat_Obj[q] == null) || (Scat_Obj[q] == null) || (Trans_Obj[q] == null))
            //        {
            //            if (!Custom_Method)
            //            {
            //                Status = System.Windows.Forms.MessageBox.Show("A material is not specified correctly. Please assign absorption and scattering to all layers in the model.", "Materials Error", System.Windows.Forms.MessageBoxButtons.OK);
            //                Complete = false;
            //                return;
            //            }
            //            ///Materails do not need to be specified, as it will not be used for an acoustical simulation... (hopefully...)
            //        }

            //        //for (int i = 0; i < 8; i++)
            //        //{
            //        //    Reflection[i] = (1 - Absorption[i]);
            //        //    Transmission[i] = Transparency[i];
            //        //    Scattering[i, 1] = Scat[i];
            //        //    double Mod = ((Scattering[i, 1] < (1 - Scattering[i, 1])) ? (Scattering[i, 1] * SplitRatio / 2) : ((1 - Scattering[i, 1]) * SplitRatio / 2));
            //        //    Scattering[i, 0] = Scattering[i, 1] - Mod;
            //        //    Scattering[i, 2] = Scattering[i, 1] + Mod;
            //        //    phase[i] = 0;
            //        //}
                    
            //        Mesh[] meshes;
            //        MeshingParameters mp = new MeshingParameters();
            //        mp.MinimumEdgeLength = 0.1;
            //        mp.SimplePlanes = true;
            //        meshes = Rhino.Geometry.Mesh.CreateFromBrep((Brep)BList[q],mp);
            //        if (meshes == null) throw new Exception("Problem with meshes");

            //        for (int t = 0; t < meshes.Length; t++)
            //        {
            //            if (meshes[t].Faces.Count < 1)
            //            {
            //                Status = System.Windows.Forms.MessageBox.Show("A surface in the model does not generate a rendermesh. This surface will not be represented in the simulation. It is recommended that you cancel this simulation and repair the affected surface. It can be located in shaded view by finding the surface which generates boundary and isoparm lines, but does not generate a fill. It can sometimes be repaired by running the command 'ShrinkTrimmedSurface'. If this does not work, it will have to be replaced by some means which would generate a proper surface.", "Surface without Rendermesh", System.Windows.Forms.MessageBoxButtons.OKCancel);
            //                if (Status == System.Windows.Forms.DialogResult.Cancel)
            //                {
            //                    Complete = false;
            //                    return;
            //                }
            //                continue;
            //            }
                     
            //            for (int u = 0; u < meshes[t].Faces.Count; u++)
            //            {
            //                Hare.Geometry.Point[] P;
            //                if (meshes[t].Faces[u].IsQuad)
            //                {
            //                    P = new Hare.Geometry.Point[4];
            //                    Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
            //                    P[0] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
            //                    FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
            //                    P[1] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
            //                    FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
            //                    P[2] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
            //                    FP = meshes[t].Vertices[meshes[t].Faces[u][3]];
            //                    P[3] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
            //                }
            //                else
            //                {
            //                    P = new Hare.Geometry.Point[3];
            //                    Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
            //                    P[0] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
            //                    FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
            //                    P[1] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
            //                    FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
            //                    P[2] = new Hare.Geometry.Point(FP.X, FP.Y, FP.Z);
            //                }

            //                //ReflectionData.Add(Reflection);
            //                AbsorptionData.Add(Mat_Obj[q]);
            //                ScatteringData.Add(Scat_Obj[q]);
            //                TransmissionData.Add(Trans_Obj[q]);
            //                //PhaseData.Add(phase);

            //                bool Trans = false;
            //                for (int t_oct = 0; t_oct < 8; t_oct++)
            //                {
            //                    if (Transmission[t_oct] > 0)
            //                    {
            //                        Trans = true;
            //                        break;
            //                    }
            //                }
            //                Transmissive.Add(Trans);
            //                if (BrepList[q].Faces[t].IsPlanar())
            //                {
            //                    Topo[0].Add_Polygon(P);
            //                    Brep_ids.Add(q);
            //                }
            //                else
            //                {
            //                    Topo[0].Add_Polygon(new Hare.Geometry.Point[3] { P[0], P[1], P[2] });
            //                    Brep_ids.Add(q);
            //                    //ReflectionData.Add(Reflection);
            //                    AbsorptionData.Add(Mat_Obj[q]);
            //                    ScatteringData.Add(Scat_Obj[q]);
            //                    TransmissionData.Add(Trans_Obj[q]);
            //                    Transmissive.Add(Trans);
            //                    //PhaseData.Add(phase);
            //                    if (P.Length > 3) Topo[0].Add_Polygon(new Hare.Geometry.Point[3] { P[0], P[2], P[3] });
            //                }
            //            }
            //        }
            //    }
                
            //    //Set up a system to find random points on planes.//
            //    Plane_Area = new double[Topo[0].Plane_Members.Length];
            //    PolyPlaneFract = new double[Topo[0].Plane_Members.Length][];

            //    for (int q = 0; q < Topo[0].Plane_Members.Length; q++)
            //    {
            //        foreach (int t in Topo[0].Plane_Members[q])
            //        {
            //            Plane_Area[q] += Topo[0].Polygon_Area(t);
            //        }
            //    }

            //    //////////////////////////
            //    for (int i = 0; i < Topo[0].planeList.Count; i++)
            //        for (int j = 0; j < Topo[0].Plane_Members[i].Count; j++)
            //        {
            //            Point3d pt= Utilities.PachTools.HPttoRPt(Topo[0].Polygon_Centroid(Topo[0].Plane_Members[i][j]));
            //            string n = Topo[0].Polys[Topo[0].Plane_Members[i][j]].Plane_ID.ToString();
            //        }
            //    //////////////////////////

            //    for (int q = 0; q < Topo[0].Plane_Members.Length; q++)
            //    {
            //        PolyPlaneFract[q] = new double[Topo[0].Plane_Members[q].Count];
            //        PolyPlaneFract[q][0] = Topo[0].Polygon_Area(Topo[0].Plane_Members[q][0]) / Plane_Area[q];
            //        for (int t = 1; t < Topo[0].Plane_Members[q].Count; t++)
            //        {
            //            PolyPlaneFract[q][t] += PolyPlaneFract[q][t - 1] + Topo[0].Polygon_Area(Topo[0].Plane_Members[q][t]) / Plane_Area[q];
            //        }
            //    }
            //    Valid = true;

            //    Utilities.PachTools.Plot_Hare_Topology(Topo[0]);
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

            private bool coplanarmesh(Rhino.Geometry.Mesh M)
            {
                Vector3d meanN = new Vector3d();
                if (M.FaceNormals.Count == 0) M.FaceNormals.ComputeFaceNormals();
                meanN = M.FaceNormals[0];
                meanN.Unitize();
                double total = 0;
                foreach (Vector3d n in M.FaceNormals) total += Math.Sqrt(meanN.X * n.X + meanN.Y * n.Y + meanN.Z * n.Z);
                return total == 0;
            }

            public override void Register_Edges(IEnumerable<Hare.Geometry.Point> S, IEnumerable<Hare.Geometry.Point> R)
            {
                //Collect Edge Curves
                List<Curve> Naked_Edges = new List<Curve>();
                List<Brep> Naked_Breps = new List<Brep>();
                List<double> Lengths = new List<double>();
                List<double[]> EdgeDomains = new List<double[]>();
                List<int> Brep_IDS = new List<int>();
                //Brep.JoinBreps()

                for (int q = 0; q < BrepList.Count; q++)
                {
                    foreach (BrepEdge be in BrepList[q].Edges)
                    {                                    
                        int[] EdgeMembers = be.AdjacentFaces();
                        switch (be.Valence)
                        {
                            case EdgeAdjacency.Interior:
                                ///This probably doesn't do any work anymore... Kept for future reference (and because it is harmless to do so...).
                                if (be.IsLinear())
                                {
                                    if (be.IsSmoothManifoldEdge()) continue; //ignore this condition...
                                    if (BrepList[q].Faces[EdgeMembers[0]].IsPlanar() && BrepList[q].Faces[EdgeMembers[1]].IsPlanar())
                                    {
                                        //Curve is straight, and surfaces are planar (Monolithic Edge)
                                        //Determine if surfaces are coplanar.//////////
                                        Brep[] BR = new Brep[2] { BrepList[q].Faces[EdgeMembers[0]].DuplicateFace(false), BrepList[q].Faces[EdgeMembers[1]].DuplicateFace(false) };
                                        //Rhino.RhinoDoc.ActiveDoc.Objects.Add(BR[0]);
                                        //Rhino.RhinoDoc.ActiveDoc.Objects.Add(BR[1]);
                                        Edge_Nodes.Add(new Edge_Straight(ref S, ref R, this.Env_Prop, BR, new int[2]{q, q}, be));///TODO: Confirm SoundSpeed Approach suitability...
                                    }
                                    else
                                    {
                                        ///Edge Curved Condition
                                        Brep[] BR = new Brep[2] { BrepList[q].Faces[EdgeMembers[0]].DuplicateFace(false), BrepList[q].Faces[EdgeMembers[1]].DuplicateFace(false) };
                                        Edge_Nodes.Add(new Edge_Curved(ref S, ref R, this.Env_Prop, BR, new int[2]{q, q}, be));///TODO: Confirm SoundSpeed Approach suitability...                                        
                                    }
                                }
                                break;
                            case EdgeAdjacency.Naked:
                                //Sorted edges allow us to assume a relationship between curves...
                                double l = be.GetLength();
                                bool register_anyway = true;
                                for (int i = 0; i < Naked_Edges.Count; i++)
                                {
                                    if (l < Lengths[i]) 
                                    {
                                        Naked_Breps.Insert(0, BrepList[q].Faces[EdgeMembers[0]].DuplicateFace(false));
                                        Naked_Edges.Insert(0, be);
                                        EdgeDomains.Insert(0, new double[] { be.Domain[0], be.Domain[1] });
                                        Lengths.Insert(0, l);
                                        Brep_IDS.Insert(0, q);
                                        register_anyway = false;
                                        ////////////////
                                        //Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(be);
                                        ////////////////
                                        break;
                                    }
                                }
                                if (register_anyway)
                                { 
                                    Naked_Breps.Add(BrepList[q].Faces[EdgeMembers[0]].DuplicateFace(false));
                                    Naked_Edges.Add(be);
                                    EdgeDomains.Add(new double[] { be.Domain[0], be.Domain[1] });
                                    Lengths.Add(l);
                                    Brep_IDS.Add(q);
                                    ////////////////
                                    //Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(be);
                                    ////////////////
                                }
                                break;
                            case EdgeAdjacency.None:
                                //Ignore this edge...
                                //What kind of edge, is neither Naked, nor Interior, nor NonManifold??
                                continue;
                            case EdgeAdjacency.NonManifold:
                                //Ignore tnis edge, but alert the user...
                                System.Windows.Forms.MessageBox.Show("Non-Manifold Edges. Could you please send a copy of this model to Pach.Acoustic.Sim@gmail.com? Also - if you know how you built this, please don't do it again...");
                                break;
                        }
                    }
                }

                ///In descending order by length - first edge will always be the longer one.
                Naked_Breps.Reverse();
                Naked_Edges.Reverse();
                EdgeDomains.Reverse();
                Brep_IDS.Reverse();
                Lengths.Reverse();

                //Handle Naked Edges... (this is now probably the only work being done by this function... this is where the magic happens...)
                double tolerance = 0.001;
                List<Curve> Orphaned_Edge = new List<Curve>();
                List<Brep> Orphaned_Brep = new List<Brep>();
                while (Naked_Edges.Count > 0)
                {
                    bool orphaned = true; //Orphaned until proven otherwise...

                    while (Naked_Edges[0] == null)
                    {
                        Naked_Edges.RemoveAt(0);
                        Naked_Breps.RemoveAt(0);
                        EdgeDomains.RemoveAt(0);
                        Lengths.RemoveAt(0);
                        Brep_IDS.RemoveAt(0);
                        continue;
                    }

                    List<Curve> ToAdd = new List<Curve>();
                    List<Brep> ToAddMembers = new List<Brep>();
                    List<int> ToAddIDS = new List<int>();

                    for (int shortid = 1; shortid < Naked_Edges.Count; shortid++)
                    {
                        Brep[] BR = new Brep[2] { Naked_Breps[shortid], Naked_Breps[0] };
                        int[] B_IDS = new int[2] { Brep_IDS[shortid], Brep_IDS[0] };

                        //Check for Match Case: (simplest edge coincidence)
                        if (Naked_Edges[0].GetLength() - Naked_Edges[shortid].GetLength() < 0.01)
                        {
                            //Curves are effectively the same length... this might be very easy.
                            if (((Naked_Edges[0].PointAtStart - Naked_Edges[shortid].PointAtStart).SquareLength < 0.0001) && ((Naked_Edges[0].PointAtEnd - Naked_Edges[shortid].PointAtEnd).SquareLength < 0.0001) ||
                            ((Naked_Edges[0].PointAtStart - Naked_Edges[shortid].PointAtEnd).SquareLength < 0.0001) && ((Naked_Edges[0].PointAtEnd - Naked_Edges[shortid].PointAtStart).SquareLength < 0.0001))
                            {
                                //Curves are effectively coincident...
                                if (Naked_Edges[shortid].IsLinear()) Edge_Nodes.Add(new Edge_Straight(ref S, ref R, this.Env_Prop, BR, B_IDS, Naked_Edges[shortid]));
                                else Edge_Nodes.Add(new Edge_Curved(ref S, ref R, this.Env_Prop, BR, B_IDS, Naked_Edges[0]));

                                Naked_Edges.RemoveAt(shortid);
                                Naked_Breps.RemoveAt(shortid);
                                EdgeDomains.RemoveAt(shortid);
                                Lengths.RemoveAt(shortid);
                                Brep_IDS.RemoveAt(shortid);
                                Naked_Edges.RemoveAt(0);
                                Naked_Breps.RemoveAt(0);
                                EdgeDomains.RemoveAt(0);
                                Lengths.RemoveAt(0);
                                Brep_IDS.RemoveAt(0);
                                orphaned = false;
                                break;
                            }
                        }

                        //if (Naked_Edges[shortid] == null) continue;
                        //Does start or end point fall on longer curve...
                        double t1;
                        double dir;
                        double domstart;
                        Point3d Start;

                        if (Naked_Edges[0].ClosestPoint(Naked_Edges[shortid].PointAtStart, out t1, tolerance) && Naked_Edges[0].TangentAt(t1).IsParallelTo(Naked_Edges[shortid].TangentAtStart) > 0)
                        {
                            //Does start id on short curve intersect?
                            //if (Naked_Edges[shortid].TangentAt(t1).IsParallelTo(Naked_Edges[0].TangentAtStart) == 0) continue;
                            dir = .1;
                            Start = Naked_Edges[shortid].PointAtStart;
                            domstart = EdgeDomains[shortid][0];
                        }
                        else if (Naked_Edges[0].ClosestPoint(Naked_Edges[shortid].PointAtEnd, out t1, tolerance) && Naked_Edges[0].TangentAt(t1).IsParallelTo(Naked_Edges[shortid].TangentAtEnd) > 0)
                        {
                            //Does end point on short curve intersect?
                            //if (Naked_Edges[shortid].TangentAt(t1).IsParallelTo(Naked_Edges[0].TangentAtEnd) == 0) continue;
                            Start = Naked_Edges[shortid].PointAtEnd;
                            dir = -.1;
                            domstart = EdgeDomains[shortid][1];
                        }
                        else
                        {
                            //Does any point on curves intersect?
                            int g;
                            Point3d p1, p2;
                            if (!Naked_Edges[0].ClosestPoints(new Curve[] { Naked_Edges[shortid] }, out p1, out p2, out g, 0.01)) continue;
                            dir = .1;
                            Start = Naked_Edges[shortid].PointAtStart;
                            t1 = Naked_Edges[shortid].Domain.T0;
                            domstart = EdgeDomains[shortid][0];
                        }

                        //Iterate across curves, Dog-Man style.
                        double tsS = t1, tsE = t1;
                        
                        while (true)
                        {
                            ////if curves are not merged
                            //////Go to the next point on the short curve and keep looking, till the short curve is exhausted. 
                            tsS = tsE;
                            double tlS;

                            Point3d pt = Naked_Edges[shortid].PointAt(tsS);
                            if (!Naked_Edges[0].ClosestPoint(pt, out tlS, 0.02))
                            {
                                //They don't match up.
                                if (Naked_Edges[0].Domain.IncludesParameter(tsS))
                                {
                                    //We are still on the long curve: Keep checking in case it may match up later...
                                    tsE += dir;
                                    continue;
                                }
                                break;
                            }

                            double tlE = tlS;

                            if (Math.Abs(Rhino.Geometry.Vector3d.Multiply(Naked_Edges[shortid].TangentAt(tsS), Naked_Edges[0].TangentAt(tlS))) < 0.98)
                            {
                                // edges may meet here, but the curves are not even remotely parallel... they might be some kind of corner junction. Go to the next curve.
                                tsE += dir;
                                continue;
                            }

                            ////If curves are merged, iterate through points until curves do not merge, or entire section of one or other is exhausted.
                            
                            tsE += dir;
                            pt = Naked_Edges[shortid].PointAt(tsE);

                            double tl2, ts2 = t1;
                            while (Naked_Edges[0].ClosestPoint(pt, out tl2, 0.01))
                            {
                                double d = Naked_Edges[0].PointAt(tl2).DistanceTo(pt);
                                if (d > 0.01) break;
                                //Curves are merged - increment check and try again.
                                tlE = tl2;
                                tsE = ts2;
                                ts2 += dir;
                                pt = Naked_Edges[shortid].PointAt(tsE);
                            }

                            if (tsS > tsE)
                            {
                                double t = tsE;
                                tsE = tsS;
                                tsS = t;
                            }

                            if (tlS > tlE)
                            {
                                double t = tlE;
                                tlE = tlS;
                                tlS = t;
                            }

                            Curve newEdge = Naked_Edges[shortid].Trim(new Interval(tsS, tsE));

                            if (newEdge == null || newEdge.GetLength() < 0.01) continue;

                            if (newEdge.IsLinear()) Edge_Nodes.Add(new Edge_Straight(ref S, ref R, this.Env_Prop, BR, B_IDS, newEdge));
                            else Edge_Nodes.Add(new Edge_Curved(ref S, ref R, this.Env_Prop, BR, B_IDS, newEdge));

                                newEdge = Naked_Edges[shortid].Trim(new Interval(tsE, Naked_Edges[shortid].Domain.T1));
                                if (newEdge != null)
                                {
                                    ToAdd.Add(newEdge);
                                    ToAddMembers.Add(Naked_Breps[shortid]);
                                    ToAddIDS.Add(Brep_IDS[shortid]);
                                }
                                newEdge = Naked_Edges[shortid].Trim(new Interval(Naked_Edges[shortid].Domain.T0, tsS));
                                if (newEdge != null)
                                {
                                    ToAdd.Add(newEdge);
                                    ToAddMembers.Add(Naked_Breps[shortid]);
                                    ToAddIDS.Add(Brep_IDS[shortid]);
                                }

                            newEdge = Naked_Edges[0].Trim(new Interval(tlE, Naked_Edges[0].Domain.T1));
                            if (newEdge != null)
                            {
                                ToAdd.Add(newEdge);
                                ToAddMembers.Add(Naked_Breps[0]);
                                ToAddIDS.Add(Brep_IDS[0]);
                            }
                            newEdge = Naked_Edges[0].Trim(new Interval(Naked_Edges[0].Domain.T0, tlS));
                            if (newEdge != null)
                            {
                                ToAdd.Add(newEdge);
                                ToAddMembers.Add(Naked_Breps[0]);
                                ToAddIDS.Add(Brep_IDS[0]);
                            }

                            ///////If curves diverge... trim merged extent from one, and record as edge. Cut this portion out of existing curves, and re-enter into sorted curve list.
                            orphaned = false;
                            Naked_Edges.RemoveAt(shortid);
                            Naked_Breps.RemoveAt(shortid);
                            EdgeDomains.RemoveAt(shortid);
                            Brep_IDS.RemoveAt(shortid);
                            Lengths.RemoveAt(shortid);
                            Naked_Edges.RemoveAt(0);
                            Naked_Breps.RemoveAt(0);
                            EdgeDomains.RemoveAt(0);
                            Brep_IDS.RemoveAt(0);
                            Lengths.RemoveAt(0);

                            for (int k = 0; k < ToAdd.Count; k++)
                            {
                                bool added = false;
                                ///////////
                                Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(ToAdd[k]);
                                ///////////
                                double l = ToAdd[k].GetLength();
                                for (int i = 0; i < Naked_Edges.Count; i++)
                                {
                                    if (l < Lengths[i])
                                    {
                                        Naked_Breps.Insert(i, ToAddMembers[k]);
                                        Naked_Edges.Insert(i, ToAdd[k]);
                                        EdgeDomains.Insert(i, new double[] { ToAdd[k].Domain.T0, ToAdd[k].Domain.T1 });
                                        Lengths.Insert(i, l);
                                        Brep_IDS.Insert(i, ToAddIDS[k]);
                                        added = true;
                                        break;
                                    }
                                }
                                if (added) continue;
                                Naked_Breps.Add(ToAddMembers[k]);
                                Naked_Edges.Add(ToAdd[k]);
                                EdgeDomains.Add(new double[] { ToAdd[k].Domain[0], ToAdd[k].Domain[1] });
                                Lengths.Add(l);
                                Brep_IDS.Add(ToAddIDS[k]);
                            }
                            break;
                        }
                        if (ToAdd.Count > 0) break;
                    }
                    if (orphaned)
                    {
                        //Check for intersecting geometry. (T-section)
                        for (int i = 0; i < this.BrepList.Count; i++ )
                        {
                            Curve[] crv;
                            Point3d[] pts;
                            Rhino.Geometry.Intersect.Intersection.CurveBrep(Naked_Edges[0], BrepList[i], 0.01, out crv, out pts);
                        }

                        Rhino.RhinoApp.WriteLine("Orphaned Edge... Assuming thin plate in free air. (Warning: T-Intersects not yet implemented.)");

                        //Create thin plate...
                        Brep Converse = Naked_Breps[0].Duplicate() as Brep;
                        Converse.Surfaces[0].Reverse(0, true);
                        if (Naked_Edges[0].IsLinear()) Edge_Nodes.Add(new Edge_Straight(ref S, ref R, this.Env_Prop, new Brep[] { Naked_Breps[0], Naked_Breps[0] }, new int[2] { Brep_IDS[0], Brep_IDS[0] }, Naked_Edges[0]));
                        else Edge_Nodes.Add(new Edge_Curved(ref S, ref R, this.Env_Prop, new Brep[] { Naked_Breps[0], Converse }, new int[2] { Brep_IDS[0], Brep_IDS[0] }, Naked_Edges[0]));
                        //Remove this edge from Naked_Edges
                        Naked_Edges.RemoveAt(0);
                        Naked_Breps.RemoveAt(0);
                        EdgeDomains.RemoveAt(0);
                        Brep_IDS.RemoveAt(0);
                        Lengths.RemoveAt(0);
                    }
                }
            }
 
            /// <summary>
            /// used by the image source method to mirror sources over a face.
            /// </summary>
            /// <param name="Point">the point to mirror</param>
            /// <param name="Top_Id">the id of the topology to reference (for multi-resolution)</param>
            /// <param name="q">the index of the surface to use</param>
            /// <returns>the mirrored point</returns>
            public Hare.Geometry.Point Image(Hare.Geometry.Point Point, int Top_Id, int q)
            {
                //Mirror the point along the plane of the polygon...
                double Dist = Topo[0].DistToPlane(Point, q);
                Hare.Geometry.Point PX = Point - 2 * Topo[Top_Id].Normal(q) * Dist; //Point + 2 * (P - Point);

                //Rhino.RhinoDoc.ActiveDoc.Objects.Add(new Rhino.Geometry.LineCurve(Utilities.PachTools.HPttoRPt(PX), Utilities.PachTools.HPttoRPt(Topo[0].Polygon_Centroid(q))));

                return PX;//Point + 2 * (P - Point);
            }

            /// <summary>
            /// Generates a random point on a polygon in the model
            /// </summary>
            /// <param name="Plane_ID">the plane index</param>
            /// <param name="Polygon">a number from 0 to 1, which aids selection of a polygon on the plane.</param>
            /// <param name="Rnd1">random number 1</param>
            /// <param name="Rnd2">random number 2</param>
            /// <param name="Rnd3">random number 3</param>
            /// <returns>the random point</returns>
            public Hare.Geometry.Point RandomPoint(int Plane_ID, double Polygon, double Rnd1, double Rnd2, double Rnd3)
            {
                int i = 0;
                for (i = 0; i < PolyPlaneFract[Plane_ID].Length; i++)
                {
                    if (PolyPlaneFract[Plane_ID][i] > Polygon) break;
                }
                return Topo[0].Polys[Topo[0].Plane_Members[Plane_ID][i]].GetRandomPoint(Rnd1, Rnd2, Rnd3);
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
                List<Hare.Geometry.Point> PTS = new List<Hare.Geometry.Point>();
                foreach (Point3d PT in P)
                {
                    PTS.Add(new Hare.Geometry.Point(PT.X, PT.Y, PT.Z));
                }
                partition(PTS, SP_PARAM);
            }

            public override void partition(List<Hare.Geometry.Point> P, int SP_PARAM)
            {
                Partitioned = true;
                for (int i = 0; i < Topo.Length; i++)
                {
                    Topo[i].Finish_Topology(P);
                }

                UI.PachydermAc_PlugIn plugin = UI.PachydermAc_PlugIn.Instance;
                if (plugin.SP_Spec() == 0)
                {
                    SP = new Hare.Geometry.Voxel_Grid(Topo, SP_PARAM, 3);
                }
                else if (plugin.SP_Spec() == 1)
                {
                    //TODO: implement an Octree...
                    throw new NotImplementedException();
                }
            }

            public void partition(List<Hare.Geometry.Point> P)
            {
                Partitioned = true;
                partition(P, UI.PachydermAc_PlugIn.Instance.VG_Domain());
            }

            public void partition(List<Point3d> P)
            {
                Partitioned = true;
                List<Hare.Geometry.Point> PTS = new List<Hare.Geometry.Point>();
                foreach (Point3d PT in P)
                {
                    PTS.Add(new Hare.Geometry.Point(PT.X, PT.Y, PT.Z));
                }
                partition(PTS, UI.PachydermAc_PlugIn.Instance.VG_Domain());
            }

            public override bool shoot(Point3d Start, Vector3d Dir, int Random, out double u, out double v, out int Poly_ID, out List<Point3d> X_PT, out List<double> t, out List<int> code)
            {
                List<Hare.Geometry.Point> X;
                if (shoot(new Hare.Geometry.Ray(new Hare.Geometry.Point(Start.X, Start.Y, Start.Z), new Hare.Geometry.Vector(Dir.X, Dir.Y, Dir.Z), 0, Random), out u, out v, out Poly_ID, out X, out t, out code))
                {
                    X_PT = new List<Point3d>();
                    foreach (Hare.Geometry.Point P in X) X_PT.Add(new Point3d(P.x, P.y, P.z));
                    return true;
                }
                X_PT = new List<Point3d> { default(Point3d) };
                return false;
            }
            
            public override bool shoot(Hare.Geometry.Ray R, out double u, out double v, out int Poly_ID, out Hare.Geometry.Point X_PT, out double t)
            {
                Hare.Geometry.X_Event X = new Hare.Geometry.X_Event();
                if (SP.Shoot(R, 0, out X))
                {
                    Poly_ID = X.Poly_id;
                    X_PT = X.X_Point;
                    t = X.t;
                    u = 0;
                    v = 0;
                    return true;
                }
                Poly_ID = 0;
                X_PT = new Hare.Geometry.Point();
                //Rhino.RhinoDoc.ActiveDoc.Objects.Add(new Rhino.Geometry.LineCurve(Utilities.PachTools.HPttoRPt(R.origin), Utilities.PachTools.HPttoRPt(R.origin + R.direction)));
                t = 0;
                u = 0;
                v = 0;
                return false;
            }

            public override bool shoot(Hare.Geometry.Ray R, out double u, out double v, out int Poly_ID, out List<Hare.Geometry.Point> X_PT, out List<double> t, out List<int> code)
            {
                ///////////////////////
                //double L2 = (R.direction.x * R.direction.x + R.direction.y * R.direction.y + R.direction.z *R.direction.z);
                //if (L2 > 1.05 || L2 < 0.95)
                //{
                //    Rhino.RhinoApp.Write("Vectors have lost normalization...");
                //}
                ///////////////////////
                //R.direction.Normalize();
                ///////////////////////
                Hare.Geometry.X_Event X = new Hare.Geometry.X_Event();
                if (SP.Shoot(R, 0, out X))
                {
                    Poly_ID = X.Poly_id;
                    X_Event_NH XNH = X as X_Event_NH;
                    if (XNH != null)
                    {
                        X_PT = XNH.P_Points;
                        t = XNH.t_trav;
                        code = XNH.SPCode;
                    }
                    else
                    {
                        X_PT = new List<Hare.Geometry.Point>{X.X_Point};
                        t = new List<double>{X.t};
                        code = new List<int>{0};
                    }

                    u = 0;
                    v = 0;
                    return true;
                }
                Poly_ID = 0;
                X_PT = new List<Hare.Geometry.Point>();
                //Rhino.RhinoDoc.ActiveDoc.Objects.Add(new Rhino.Geometry.LineCurve(Utilities.PachTools.HPttoRPt(R.origin), Utilities.PachTools.HPttoRPt(R.origin + R.direction)));
                t = new List<double>();
                u = 0;
                v = 0;
                code = new List<int>() { 0 };
                return false;
            }

            public bool shoot(Hare.Geometry.Ray R, int top_id, out Hare.Geometry.X_Event X)
            {
                if (SP.Shoot(R, top_id, out X)) return true;
                return false;
            }

            public override Hare.Geometry.Point ClosestPt(Hare.Geometry.Point P, ref double Dist)
            {
                double Max = double.MaxValue;
                Hare.Geometry.Point RP = new Hare.Geometry.Point();
                Hare.Geometry.Point TP;
                for (int i = 0; i < Topo[0].Polygon_Count; i++)
                {
                    TP = Topo[0].Closest_Point(P, i);
                    Dist = TP.x * TP.x + TP.y * TP.y + TP.z * TP.z;
                    if (Dist < Max)
                    {
                        RP = TP;
                        Max = Dist;
                    }
                }
                Dist = Math.Sqrt(Max);
                return RP;
            }

            public override Point3d ClosestPt(Point3d P, ref double Dist)
            {
                Hare.Geometry.Point PH = new Hare.Geometry.Point(P.X, P.Y, P.Z);
                PH = ClosestPt(PH, ref Dist);
                return new Point3d(PH.x, PH.y, PH.z);
            }

            /// <summary>
            /// Identifies the scene type.
            /// </summary>
            /// <returns></returns>
            public override string Scene_Type()
            {
                return "Polygon_Scene";
            }

            public override bool PointsInScene(List<Hare.Geometry.Point> PTS)
            {
                throw new NotImplementedException();
            }

            public override double Sound_speed(Hare.Geometry.Point pt)
            {
                return Env_Prop.Sound_Speed(pt);
                //if (IsHomogeneous) return Env_Prop.Sound_Speed(pt);
                //else return (SP as Environment.VoxelGrid_PolyRefractive).Sound_Speed(pt);
            }

            public override double Sound_speed(int arg)
            {
                return Env_Prop.Sound_Speed(arg);
                //if (IsHomogeneous) return Env_Prop.Sound_Speed(X,Y,Z);
                //else return (SP as Environment.VoxelGrid_PolyRefractive).Sound_Speed(X, Y, Z);
            }

            public override Hare.Geometry.Vector Normal(int i, double u, double v)
            {
                return Topo[0].Normal(i);
            }

            public override Hare.Geometry.Vector Normal(int i)
            {
                return Topo[0].Normal(i);
            }

            public override int Count()
            {
                return Topo[0].Polygon_Count;
            }

            public override bool IsPlanar(int q)
            {
                return true;
            }

            public override double SurfaceArea(int x)
            {
                return Topo[0].Polygon_Area(x);
            }

            public override void EdgeFrame_Tangents(Hare.Geometry.Point Origin, Vector Normal, int[] PlaneIDs, ref List<double> dist2, List<Vector> Dir, List<int> EdgeIDs)
            {
                double d = Hare_math.Dot(Normal, Origin);
                //for (int i = 0; i < PlaneCount; i++)
                foreach(int i in EdgeIDs)
                {
                    Hare.Geometry.Point[] Pts = new Hare.Geometry.Point[2];
                    double d_min = double.MaxValue;
                    double d_max = double.MinValue;
                    foreach (int j in Topo[0].Plane_Members[i])
                    {
                        //Do the polygon/plane intersection for each member 'j' of i.
                        Hare.Geometry.Point[] vtx = Topo[0].Polygon_Vertices(j);
                        Hare.Geometry.Point[] temp = new Hare.Geometry.Point[1];
                        uint tmpcount = 0;

                        for (int k = 0, h = 1; k < vtx.Length; k++, h++)
                        {
                            Vector ab = vtx[h % vtx.Length] - vtx[k];
                            double t = (d - Hare_math.Dot(Normal, vtx[k])) / Hare_math.Dot(Normal, ab);

                            // If t in [0..1] compute and return intersection point
                            if (t >= 0.0f && t <= 1.0f)
                            {
                                temp[tmpcount] = vtx[k] + t * ab;
                                tmpcount++;
                            }
                            if (h == 0 && tmpcount == 0) break;
                            if (tmpcount > 1) break;
                        }
                        foreach (Hare.Geometry.Point p in temp)
                        {
                            Hare.Geometry.Point v = Origin - p;
                            double tmp = v.x * v.x + v.y * v.y + v.z * v.z;
                            if (tmp > d_max)
                            {
                                Pts[1] = p;
                                d_max = tmp;
                            }
                            if (tmp < d_min)
                            {
                                Pts[0] = p;
                                d_min = tmp;
                            }
                        }
                    }
                    dist2.Add(d_min);
                    EdgeIDs.Add(i);
                    Vector direction = (Pts[1] - Pts[0]);
                    direction.Normalize();
                    Dir.Add(direction);
                }
            }

            /// <summary>
            /// returns the number of planes. (not polygons)
            /// </summary>
            public int PlaneCount
            {
                get
                {
                    return Topo[0].Plane_Members.Length;
                }
            }

            /// <summary>
            /// gets the list of plane ids by polygon index.
            /// </summary>
            public int PlaneID(int i)
            {
                   return Topo[0].Polys[i].Plane_ID;
            }

            public int BrepID(int i)
            {
                return Brep_ids[i];
            }

            public bool Box_Intersect(AABB box, out double abs, out Vector V)//, out int[] PolyIds, out double[] Abs, out double[] Trans, out double[] Scat)
            {
                List<int> isect;
                abs = 0;
                V = new Vector();
                SP.Box_Intersect(box, out isect);
                if (isect.Count == 0) return false;

                //if (isect.Count > 1)
                //{
                //    Rhino.RhinoApp.WriteLine(isect.Count.ToString());
                //}

                foreach(int i in isect)
                {
                    V += this.Normal(i);
                    abs += (1 - this.AbsorptionData[i].Reflection_Narrow(0)).Magnitude;
                    ///TODO:Find some intelligent way of applying absorption.
                }

                //if (double.IsNaN(V.y))
                //{
                //    Rhino.RhinoApp.WriteLine("Doh!");
                //}

                V.Normalize();
                abs /= isect.Count;

                return true;
            }

            /// <summary>
            /// gets the list of polygons on a each plane.
            /// </summary>
            public List<int>[] PlaneMembers
            {
                get
                {
                    return Topo[0].Plane_Members;
                }
            }

            public override Hare.Geometry.Point Max()
            {
                return this.Topo[0].Max;
            }

            public override Hare.Geometry.Point Min()
            {
                return this.Topo[0].Min;
            }
        }
    }
}