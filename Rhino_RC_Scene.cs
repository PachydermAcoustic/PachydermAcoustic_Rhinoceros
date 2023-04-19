//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2023, Arthur van der Harten 
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
using Rhino.Geometry;
using Hare.Geometry;

namespace Pachyderm_Acoustic
{
    namespace Environment
    {
        public class RhCommon_PolygonScene : Polygon_Scene
        {
            public RhCommon_PolygonScene(List<Rhino.DocObjects.RhinoObject> ObjectList, bool register_edges, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool AcousticFailsafes)
                : this(ObjectList, null, null, register_edges, Temp, hr, Pa, Air_Choice, EdgeCorrection, AcousticFailsafes)
            {
            }

            public RhCommon_PolygonScene(List<Rhino.DocObjects.RhinoObject> ObjectList, List<Rhino.Geometry.Brep> Additional_Geometry, int[] Layer_IDs, bool register_edges, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool AcousticFailsafes)
                : base(Temp, hr, Pa, Air_Choice, EdgeCorrection, AcousticFailsafes)
            {
                List<Hare.Geometry.Point[][]> model = new List<Hare.Geometry.Point[][]>();
                List<double[][]> Kurvatures = new List<double[][]>();
                List<Vector[][]> Frame_Axes = new List<Vector[][]>();

                List<GeometryBase> BList = new List<GeometryBase>();

                List<int> Brep_ids = new List<int>();

                List<Brep> BrepList = new List<Brep>();
                List<Material> ABS_Construct = new List<Material>();
                List<Scattering> SCT_Construct = new List<Scattering>();
                List<double[]> TRN_Construct = new List<double[]>();
                List<bool> isCurved_Construct = new List<bool>();

                List<Material> Mat_Layer = new List<Material>();
                List<Scattering> Scat_Layer = new List<Scattering>();
                List<Material> Mat_Obj = new List<Material>();
                List<Scattering> Scat_Obj = new List<Scattering>();
                List<double[]> Trans_Layer = new List<double[]>();
                List<double[]> Trans_Obj = new List<double[]>();
                //Organize the geometry into Breps
                //Get materials for each layer:

                if (Additional_Geometry != null && Additional_Geometry.Count != Layer_IDs.Length) throw new Exception("The quantity of additional geometry and layer ids must be the same.");

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
                        Pachyderm_Acoustic.Utilities.PachTools.DecodeAcoustics(Layer.GetUserString("Acoustics"), ref Abs, ref Scat, ref Trans);
                        ///Other properties are still coefficient based...
                        Scat_Layer.Add(new Environment.Lambert_Scattering(Scat, SplitRatio));
                        Trans_Layer.Add(Trans);
                    }
                    else if (abstype == "Buildup_Finite")
                    {
                        //Finite_Layers.Add(true);
                        string BU = Layer.GetUserString("BuildUp");
                        string[] BU_split = BU.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        List<AbsorptionModels.ABS_Layer> Buildup = new List<AbsorptionModels.ABS_Layer>();
                        foreach (string swatch in BU_split) Buildup.Add(AbsorptionModels.ABS_Layer.LayerFromCode(swatch));
                        Environment.Smart_Material sm = new Environment.Smart_Material(false, Buildup, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2);
                        Mat_Layer.Add(sm);
                        double[] Abs = new double[8], Scat = new double[8], Trans = new double[8];
                        Pachyderm_Acoustic.Utilities.PachTools.DecodeAcoustics(Layer.GetUserString("Acoustics"), ref Abs, ref Scat, ref Trans);
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
                        Pachyderm_Acoustic.Utilities.PachTools.DecodeAcoustics(spec, ref Abs, ref Scat, ref Trans);
                        //New code for transmission loss incorporation...
                        string trans = Layer.GetUserString("Transmission");
                        Trans = (trans == "" || trans == null ) ? ( Trans[3] + Trans[4] + Trans[5] == 0 ? new double[] { 0, 0, 0, 0, 0, 0, 0, 0 } : Trans = Trans) : Utilities.PachTools.DecodeTransmissionLoss(trans);
                        for (int oct = 0; oct < 8; oct++)
                        {
                            double ret = Math.Pow(10, -Trans[oct] / 10);
                            Trans[oct] = 1 - ret;
                            //Abs[oct] *= ret;
                        }

                        Mat_Layer.Add(new Environment.Basic_Material(Abs));
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
                        if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
                        {
                            double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
                            Pachyderm_Acoustic.Utilities.PachTools.DecodeAcoustics(ObjectList[q].Geometry.GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
                            Mat_Obj.Add(new Basic_Material(ABS));
                            Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
                            Trans_Obj.Add(TRANS);
                        }
                        else
                        {
                            Mat_Obj.Add(Mat_Layer[ObjectList[q].Attributes.LayerIndex]);
                            Scat_Obj.Add(Scat_Layer[ObjectList[q].Attributes.LayerIndex]);
                            Trans_Obj.Add(Trans_Layer[ObjectList[q].Attributes.LayerIndex]);
                        }
                    }
                    else if (ObjectList[q].ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        Rhino.Geometry.Brep BObj = ((Rhino.DocObjects.ExtrusionObject)ObjectList[q]).ExtrusionGeometry.ToBrep();
                        for (int i = 0; i < BObj.Faces.Count; i++)
                        {
                            if (ObjectList[q].Geometry.GetUserString("Acoustics_User") == "yes")
                            {
                                double[] ABS = new double[8], SCAT = new double[8], TRANS = new double[8];
                                Pachyderm_Acoustic.Utilities.PachTools.DecodeAcoustics(ObjectList[q].Geometry.GetUserString("Acoustics"), ref ABS, ref SCAT, ref TRANS);
                                Mat_Obj.Add(new Basic_Material(ABS));
                                Scat_Obj.Add(new Lambert_Scattering(SCAT, SplitRatio));
                                Trans_Obj.Add(TRANS);
                            }
                            else
                            {
                                Mat_Obj.Add(Mat_Layer[ObjectList[q].Attributes.LayerIndex]);
                                Scat_Obj.Add(Scat_Layer[ObjectList[q].Attributes.LayerIndex]);
                                Trans_Obj.Add(Trans_Layer[ObjectList[q].Attributes.LayerIndex]);
                            }

                            B.Add(BObj.Faces[i].ToBrep());
                        }
                    }
                    else
                    {
                        continue;
                    }
                    BList.AddRange(B);
                }

                try
                {
                    if (Additional_Geometry != null)
                    {
                        for (int i = 0; i < Additional_Geometry.Count; i++)
                        {
                            for (int j = 0; j < Additional_Geometry[i].Faces.Count; j++)
                            {
                                BList.Add(Additional_Geometry[i].Faces[j].ToBrep());
                                Mat_Obj.Add(Mat_Layer[Layer_IDs[i]]);
                                Scat_Obj.Add(Scat_Layer[Layer_IDs[i]]);
                                Trans_Obj.Add(Trans_Layer[Layer_IDs[i]]);
                            }
                        }
                    }
                }
                catch
                {
                    throw new Exception("Issue with Layer Material Assignments. Make sure that your layer index is correct.");
                }

                int id = -1;

                MeshingParameters mp = new MeshingParameters();
                List<Brep> blist = new List<Brep>();
                mp.JaggedSeams = false;
                mp.MaximumEdgeLength = 343d / 1000d;
                mp.MinimumEdgeLength = 343d / 1000d;
                mp.SimplePlanes = true;
                mp.RefineGrid = true;

                while (mp.MaximumEdgeLength < 20)
                {
                    Mesh Test_mesh = new Mesh();
                    for (int q = 0; q < BList.Count; q++)
                    {
                        Mesh[] m = Mesh.CreateFromBrep(BList[q] as Brep, mp);
                        for (int i = 0; i < m.Length; i++) Test_mesh.Append(m[i]);
                    }
                    if (Test_mesh.Faces.Count < 80000) break;
                    mp.MaximumEdgeLength *= 1.5;
                    mp.MinimumEdgeLength *= 1.5;
                }

                for (int q = 0; q < BList.Count; q++)
                {
                    Brep BL = BList[q] is BrepFace ? (BList[q] as BrepFace).DuplicateFace(false) : BList[q] as Brep;
                    for (int r = 0; r < BL.Faces.Count; r++)
                    {
                        ABS_Construct.Add(Mat_Obj[q]);
                        SCT_Construct.Add(Scat_Obj[q]);
                        TRN_Construct.Add(Trans_Obj[q]);
                        if (!BL.Faces[r].IsPlanar())
                        {
                            //Filter for a geometrically significant amount of curvature...
                            //This is necessary in order to avoid numerical errors...
                            double K0 = Math.Abs(BL.Faces[r].CurvatureAt(0.1, 0.1).Kappa(0));
                            double K1 = Math.Abs(BL.Faces[r].CurvatureAt(0.1, 0.1).Kappa(1));
                            if (Math.Max(K0, K1) > 1E-3)
                            { isCurved_Construct.Add(true); }
                            else { isCurved_Construct.Add(false); }
                        }
                        else { isCurved_Construct.Add(false); }
                        BrepList.Add(BL.Faces[r].DuplicateFace(false));
                        Brep B = BL.Faces[r].DuplicateFace(false);
                        double[] Transmission = new double[8];

                        if ((Mat_Obj[q] == null) || (Scat_Obj[q] == null) || (Trans_Obj[q] == null))
                        {
                            if (!Custom_Method)
                            {
                                Status = System.Windows.Forms.MessageBox.Show("A material is not specified correctly. Please assign absorption and scattering to all layers in the model.", "Materials Error", System.Windows.Forms.MessageBoxButtons.OK);
                                Complete = false;
                                return;
                            }
                        }

                        Mesh[] meshes;

                        //TODO: insert a check for degenerate polygons, and exclude prior to passing to construct method.
                        do
                        {
                            meshes = Rhino.Geometry.Mesh.CreateFromBrep(B, mp);
                            if (meshes == null) //throw new Exception("Problem with meshes");
                            {
                                //Mesh min length is likely too great. Try again
                                mp.MinimumEdgeLength /= 2;
                                continue;
                            }
                            break;
                        } while (true);


                        for (int t = 0; t < meshes.Length; t++)
                        {
                            id++;
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
                            List<Hare.Geometry.Point[]> temp_polys = new List<Hare.Geometry.Point[]>();
                            List<double[]> temp_K = new List<double[]>();
                            List<Vector[]> temp_Frames = new List<Vector[]>();

                            for (int u = 0; u < meshes[t].Faces.Count; u++)
                            {   
                                Hare.Geometry.Point[] P;
                                Hare.Geometry.Point Centroid;

                                if (meshes[t].Faces[u].IsQuad)
                                {
                                    P = new Hare.Geometry.Point[4];
                                    Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
                                    P[0] = new Hare.Geometry.Point(Math.Round(FP.X,10), Math.Round(FP.Y,10), Math.Round(FP.Z,10));
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
                                    P[1] = new Hare.Geometry.Point(Math.Round(FP.X,10), Math.Round(FP.Y,10), Math.Round(FP.Z,10));
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
                                    P[2] = new Hare.Geometry.Point(Math.Round(FP.X,10), Math.Round(FP.Y,10), Math.Round(FP.Z,10));
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][3]];
                                    P[3] = new Hare.Geometry.Point(Math.Round(FP.X,10), Math.Round(FP.Y,10), Math.Round(FP.Z,10));
                                    if (isDegenerate(ref P)) continue;

                                    Centroid = (P.Length == 4) ? (P[0] + P[1] + P[2] + P[3]) / 4 : (P[0] + P[1] + P[2]) / 3;
                                }
                                else
                                {
                                    P = new Hare.Geometry.Point[3];
                                    Point3f FP = meshes[t].Vertices[meshes[t].Faces[u][0]];
                                    P[0] = new Hare.Geometry.Point(Math.Round(FP.X,10), Math.Round(FP.Y,10), Math.Round(FP.Z,10));
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][1]];
                                    P[1] = new Hare.Geometry.Point(Math.Round(FP.X,10), Math.Round(FP.Y,10), Math.Round(FP.Z,10));
                                    FP = meshes[t].Vertices[meshes[t].Faces[u][2]];
                                    P[2] = new Hare.Geometry.Point(Math.Round(FP.X,10), Math.Round(FP.Y,10), Math.Round(FP.Z,10));
                                    Centroid = (P[0] + P[1] + P[2]) / 3;

                                    if (isDegenerate(ref P)) continue;
                                }

                                B.Surfaces[0].ClosestPoint(Utilities.RC_PachTools.HPttoRPt(Centroid), out double _u, out double _v);
                                SurfaceCurvature SC = B.Surfaces[0].CurvatureAt(_u, _v);
                                //Kurvatures[Kurvatures.Count - 1][u] = new double[2] { SC.Kappa(0), SC.Kappa(1) };
                                //Frame_Axes[Kurvatures.Count - 1][u] = new Vector[2] { Utilities.RC_PachTools.RPttoHPt(SC.Direction(0)), Utilities.RC_PachTools.RPttoHPt(SC.Direction(1)) };
                                temp_K.Add(new double[2] { SC.Kappa(0), SC.Kappa(1) });
                                temp_Frames.Add(new Vector[2] { Utilities.RC_PachTools.RPttoHPt(SC.Direction(0)), Utilities.RC_PachTools.RPttoHPt(SC.Direction(1)) });

                                bool Trans = false;
                                for (int t_oct = 0; t_oct < 8; t_oct++)
                                {
                                    if (Trans_Obj[q][t_oct] > 0)
                                    {
                                        Trans = true;
                                        break;
                                    }
                                }
                                Transmissive.Add(Trans);
                                temp_polys.Add(P);
                            }
                            model.Add(temp_polys.ToArray());
                            Kurvatures.Add(temp_K.ToArray());
                            Frame_Axes.Add(temp_Frames.ToArray());
                        }
                    }
                }

                Construct(model.ToArray(), ABS_Construct, SCT_Construct, TRN_Construct, isCurved_Construct.ToArray(), Kurvatures.ToArray(), Frame_Axes.ToArray());

                if (register_edges)
                {
                    //Collect Edge Curves
                    Edges = new List<MathNet.Numerics.Interpolation.CubicSpline[]>();
                    Edge_Tangents = new List<MathNet.Numerics.Interpolation.CubicSpline[]>();
                    Edge_isSoft = new List<bool>();
                    EdgeLength = new List<double>();
                    List<Brep> Naked_Breps = new List<Brep>();
                    List<Curve> Naked_Edges = new List<Curve>();
                    List<double> Lengths = new List<double>();
                    List<double[]> EdgeDomains = new List<double[]>();
                    List<int> Brep_IDS = new List<int>();

                    for (int p = 0; p < BrepList.Count; p++)
                    {
                        foreach (BrepEdge be in BrepList[p].Edges)
                        {
                            int[] EdgeMembers = be.AdjacentFaces();
                            switch (be.Valence)
                            {
                                case EdgeAdjacency.Interior:
                                    ///This probably doesn't do any work anymore... Kept for future reference (and because it is harmless to do so...).
                                    if (be.IsLinear())
                                    {
                                        if (be.IsSmoothManifoldEdge()) continue; //ignore this condition...
                                        if (BrepList[p].Faces[EdgeMembers[0]].IsPlanar() && BrepList[p].Faces[EdgeMembers[1]].IsPlanar())
                                        {
                                            //Curve is straight, and surfaces are planar (Monolithic Edge)
                                            //Determine if surfaces are coplanar.//////////
                                            Brep[] BR = new Brep[2] { BrepList[p].Faces[EdgeMembers[0]].DuplicateFace(false), BrepList[p].Faces[EdgeMembers[1]].DuplicateFace(false) };
                                            create_curved_edge_entry(be, BR, new Material[2] { AbsorptionData[p], AbsorptionData[p] }, p );
                                        }
                                        else
                                        {
                                            ///Edge Curved Condition
                                            Brep[] BR = new Brep[2] { BrepList[p].Faces[EdgeMembers[0]].DuplicateFace(false), BrepList[p].Faces[EdgeMembers[1]].DuplicateFace(false) };
                                            create_curved_edge_entry(be, BR, new Material[2] { AbsorptionData[p], AbsorptionData[p] }, p);
                                        }
                                    }
                                    ////Now would do harm. Throw exception instead...
                                    //throw new Exception("Tried to get edges from breps that have not been split up...");
                                    break;
                                case EdgeAdjacency.Naked:
                                    //Sorted edges allow us to assume a relationship between curves...
                                    double l = be.GetLength();
                                    bool register_anyway = true;
                                    for (int i = 0; i < Naked_Edges.Count; i++)
                                    {
                                        if (l < Lengths[i])
                                        {
                                            Naked_Breps.Insert(i, BrepList[p].Faces[EdgeMembers[0]].DuplicateFace(false));
                                            Naked_Edges.Insert(i, be);
                                            EdgeDomains.Insert(i, new double[] { be.Domain[0], be.Domain[1] });
                                            Lengths.Insert(i, l);
                                            Brep_IDS.Insert(i, p);
                                            register_anyway = false;
                                            break;
                                        }
                                    }
                                    if (register_anyway)
                                    {
                                        Naked_Breps.Add(BrepList[p].Faces[EdgeMembers[0]].DuplicateFace(false));
                                        Naked_Edges.Add(be);
                                        EdgeDomains.Add(new double[] { be.Domain[0], be.Domain[1] });
                                        Lengths.Add(l);
                                        Brep_IDS.Add(p);
                                    }
                                    break;
                                case EdgeAdjacency.None:
                                    //Ignore this edge...
                                    //What kind of edge, is neither Naked, nor Interior, nor Non-Manifold??
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

                        if(Naked_Edges[0] == null)
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
                                    if (Naked_Edges[shortid].IsLinear()) create_curved_edge_entry(Naked_Edges[shortid], BR, new Material[2] { AbsorptionData[B_IDS[0]], AbsorptionData[B_IDS[1]]}, Brep_IDS[0], Brep_IDS[shortid]);                                                                                                                                                                                                    //else Edge_Nodes.Add(new Edge_Curved(ref S, ref R, this.Env_Prop, BR, B_IDS, Naked_Edges[0]));

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

                            //Does start or end point fall on longer curve...
                            double t1;
                            double dir;
                            double domstart;
                            Point3d Start;

                            if (Naked_Edges[0].ClosestPoint(Naked_Edges[shortid].PointAtStart, out t1, tolerance) && Naked_Edges[0].TangentAt(t1).IsParallelTo(Naked_Edges[shortid].TangentAtStart) > 0)
                            {
                                //Does start id on short curve intersect?
                                //if (Edges[shortid].TangentAt(t1).IsParallelTo(Edges[0].TangentAtStart) == 0) continue;
                                dir = .1;
                                Start = Naked_Edges[shortid].PointAtStart;
                                domstart = EdgeDomains[shortid][0];
                            }
                            else if (Naked_Edges[0].ClosestPoint(Naked_Edges[shortid].PointAtEnd, out t1, tolerance) && Naked_Edges[0].TangentAt(t1).IsParallelTo(Naked_Edges[shortid].TangentAtEnd) > 0)
                            {
                                //Does end point on short curve intersect?
                                //if (Edges[shortid].TangentAt(t1).IsParallelTo(Edges[0].TangentAtEnd) == 0) continue;
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

                                if (Math.Abs(Vector3d.Multiply(Naked_Edges[shortid].TangentAt(tsS), Naked_Edges[0].TangentAt(tlS))) < 0.98)
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

                                if (tl2 == 0) break;

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

                                if (newEdge == null || newEdge.GetLength() < 0.01)  continue;
                                create_curved_edge_entry(newEdge, BR, new Material[2] { AbsorptionData[B_IDS[0]], AbsorptionData[B_IDS[1]] }, Brep_IDS[0], Brep_IDS[shortid]);
                                //if (newEdge.IsLinear()) Edge_Nodes.Add(new Edge_Straight(ref S, ref R, this.Env_Prop, BR, B_IDS, newEdge));
                                //else Edge_Nodes.Add(new Edge_Curved(ref S, ref R, this.Env_Prop, BR, B_IDS, newEdge));

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
                                    double l = ToAdd[k].GetLength();
                                    for (int i = 0; i < Naked_Edges.Count; i++)
                                    {
                                        if (l > Lengths[i])
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
                            bool coincident = false;
                            //Check for intersecting geometry. (T-section)
                            List<Curve> starting = new List<Curve>();
                            starting.Add(Naked_Edges[0]);
                            for (int i = 0; i < BrepList.Count; i++)
                            {
                                if (Brep_IDS[0] == i) continue;
                                if (starting.Count == 0)
                                {
                                    coincident = true;
                                    break;
                                }
                                Curve[] crv;
                                Point3d[] pts;
                                List<Curve> passed = new List<Curve>();
                                foreach (Curve totest in starting)
                                {
                                    Rhino.Geometry.Intersect.Intersection.CurveBrep(totest, BrepList[i], 0.01, out crv, out pts);
                                    if (crv != null)
                                    {
                                        foreach (Curve c in crv)
                                        {
                                            if (Lengths[0] > c.GetLength() - 0.02)
                                            {
                                                coincident = true;
                                                break;
                                            }
                                            else
                                            {
                                                passed.AddRange(Rhino.Geometry.Curve.CreateBooleanDifference(totest, c));
                                            }
                                        }
                                    }
                                }
                                if (coincident)
                                {
                                    starting.Clear();
                                    break;
                                }
                                if(passed.Count > 0) starting = passed;
                            }

                            //Create thin plate...
                            foreach (Curve c in starting)
                            {
                                Brep Converse = Naked_Breps[0].Duplicate() as Brep;
                                Converse.Surfaces[0].Reverse(0, true);
                                create_curved_edge_entry(c, new Brep[] { Naked_Breps[0], Converse }, new Material[2] { AbsorptionData[Brep_IDS[0]], AbsorptionData[Brep_IDS[0]] }, Brep_IDS[0]);
                            }

                            //Remove this edge from Edges
                            Naked_Edges.RemoveAt(0);
                            Naked_Breps.RemoveAt(0);
                            EdgeDomains.RemoveAt(0);
                            Brep_IDS.RemoveAt(0);
                            Lengths.RemoveAt(0);
                        }
                    }
                }
            }

            public bool isDegenerate(ref Hare.Geometry.Point[] m)
            {
                //returns 0 for false, 1 for true, and 2 for misleading... as in a quad is really a tri.
                if (m.Length == 4)
                {
                    Vector AB = m[1] - m[0];
                    Vector AC = m[2] - m[0];
                    if (Hare_math.Cross(AB, AC).Length() < 1E-6)
                    {
                        Vector DB = m[1] - m[3];
                        Vector DC = m[2] - m[3];
                        if (Hare_math.Cross(DB, DC).Length() < 1E-6) return true;
                        m = new Hare.Geometry.Point[3] { m[1], m[2], m[3] };
                    }
                    else
                    {
                        Vector DB = m[1] - m[3];
                        Vector DC = m[2] - m[3];
                        if (Hare_math.Cross(DB, DC).Length() < 1E-6)
                        {
                            m = new Hare.Geometry.Point[3] { m[0], m[1], m[2] };
                        }
                    }
                }
                else
                {
                    Vector AB = m[1] - m[0];
                    Vector AC = m[2] - m[0];
                    if (Hare_math.Cross(AB, AC).Length() < 1E-6)
                    {
                        return true;
                    }
                }
                return false;
            }

            private void create_curved_edge_entry(Curve i, Brep[] B, Material[] absorption_characteristics, int Srf1, int Srf2 = -1)
            {
                //int fs = 176400;
                //double length = Env_Prop.Sound_Speed(Utilities.RC_PachTools.RPttoHPt(i.PointAtStart)) / (4 * fs);
                double length = 0.01;

                List<double> d = new List<double>();
                List<double> x = new List<double>();
                List<double> y = new List<double>();
                List<double> z = new List<double>();
                List<double> t1x = new List<double>();
                List<double> t1y = new List<double>();
                List<double> t1z = new List<double>();
                List<double> t2x = new List<double>();
                List<double> t2y = new List<double>();
                List<double> t2z = new List<double>();

                while (true)
                {
                    double t;
                    if (!i.LengthParameter(length, out t))
                    {
                        EdgeLength.Add(length - 0.025);
                        break;
                    }

                    Plane P;
                    i.PerpendicularFrameAt(t, out P);
                    Curve[] Csects1, Csects2;
                    Point3d[] Psects;

                    //double Delta_Z = Env_Prop.Sound_Speed(Utilities.RC_PachTools.RPttoHPt(P.Origin)) / (4 * fs);//TODO: Adjust depending on distance from source to receiver... (nearest, farthest?)
                    //Sources.Add(new EdgeSource(Edge.Rigid, Utilities.Pach_Tools.RPttoHPt(P.Origin), Delta_Z, HTangents));
                    //length += Delta_Z;
                    
                    //double DeltaZ = Env_Props.Sound_Speed(Point3d.Origin) / (4 * fs);
                    Rhino.Geometry.Intersect.Intersection.BrepPlane(B[0], P, 0.001, out Csects1, out Psects);
                    Rhino.Geometry.Intersect.Intersection.BrepPlane(B[1], P, 0.001, out Csects2, out Psects);
                    if (Csects1.Length == 0 || Csects2.Length == 0) continue;

                    d.Add(length);
                    x.Add(P.Origin.X);
                    y.Add(P.Origin.Y);
                    z.Add(P.Origin.Z);
                    length += 0.025;

                    ///Control Start Point of curve
                    if ((Csects1[0].PointAtStart.X * P.Origin.X + Csects1[0].PointAtStart.Y * P.Origin.Y + Csects1[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects1[0].Reverse();
                    if ((Csects2[0].PointAtStart.X * P.Origin.X + Csects2[0].PointAtStart.Y * P.Origin.Y + Csects2[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects2[0].Reverse();
                    ///Get Tangent Vector
                    Vector3d T1 = (Csects1[0].PointAtNormalizedLength(0.05) - P.Origin);
                    T1.Unitize();
                    t1x.Add(T1.X);
                    t1y.Add(T1.Y);
                    t1z.Add(T1.Z);
                    Vector3d T2 = (Csects2[0].PointAtNormalizedLength(0.05) - P.Origin);
                    T2.Unitize();
                    t2x.Add(T2.X);
                    t2y.Add(T2.Y);
                    t2z.Add(T2.Z);
                    //Hare.Geometry.Vector[] HTangents = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Tangents[0].X, Tangents[0].Y, Tangents[0].Z), new Hare.Geometry.Vector(Tangents[1].X, Ta0gents[1].Y, 0T00000000000Z) };
                    ///Get Normal
                    double up, vp;
                    ComponentIndex CI;
                    Point3d outPt;
                    Vector3d[] Normals = new Vector3d[2];
                    //Pomid);
                    B[0].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[0]);
                    B[1].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[1]);
                    //Hare.Geometry.Vector[] HNormals = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Normals[0].X, Normals[0].Y, Normals[0].Z), new Hare.Geometry.Vector(Normals[1].X, Normals[1].Y, Normals[1].Z) };
                }

                if (d.Count < 5) return;

                Edge_Srfs.Add(new int[2] { Srf1, Srf2 });

                Edges.Add(new MathNet.Numerics.Interpolation.CubicSpline[] {
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), x.ToArray()),
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), y.ToArray()),
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), z.ToArray()) });

                Edge_Tangents.Add(new MathNet.Numerics.Interpolation.CubicSpline[] {
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), t1x.ToArray()),
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), t1y.ToArray()),
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), t1z.ToArray()),
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), t2x.ToArray()),
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), t2y.ToArray()),
                MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(d.ToArray(), t2z.ToArray()) });
                Edge_isSoft.Add(absorption_characteristics[0].Coefficient_A_Broad(5) + absorption_characteristics[1].Coefficient_A_Broad(5) > 0.4);

                //Plane P;
                //Curve[] Csects1;
                //Curve[] Csects2;
                //Point3d[] Psects;

                ////for (;;)
                ////{
                //double t = r.NextDouble() * (Be.Domain.Max - Be.Domain.Min) + Be.Domain.Min;

                //Be.FrameAt(t, out P);

                //P.Origin();

                //Brep[0].Surfaces[0].P

                ////Rhino.Geometry.Intersect.Intersection.BrepPlane(Brep[0], P, 0.1, out Csects1, out Psects);
                ////Rhino.Geometry.Intersect.Intersection.BrepPlane(Brep[1], P, 0.1, out Csects2, out Psects);

                ////if (Csects1 != null && Csects2 != null && Csects1.Length > 0 && Csects2.Length > 0) break;

                ////Rhino.RhinoDoc.ActiveDoc.Objects.Add(Csects1[0]);
                ////Rhino.RhinoDoc.ActiveDoc.Objects.Add(Csects2[0]);
                ////}

                //Vector3d[] Tangents = new Vector3d[2];
                /////Control Start Point of curve
                //if ((Csects1[0].PointAtStart.X * P.Origin.X + Csects1[0].PointAtStart.Y * P.Origin.Y + Csects1[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects1[0].Reverse();
                //if ((Csects2[0].PointAtStart.X * P.Origin.X + Csects2[0].PointAtStart.Y * P.Origin.Y + Csects2[0].PointAtStart.Z * P.Origin.Z) < 0.00001) Csects2[0].Reverse();
                /////Get Tangent Vector
                //Tangents[0] = (Csects1[0].PointAtNormalizedLength(0.05) - P.Origin);
                //Tangents[0].Unitize();
                //Tangents[1] = (Csects2[0].PointAtNormalizedLength(0.05) - P.Origin);
                //Tangents[1].Unitize();
                //Hare.Geometry.Vector[] HTangents = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Tangents[0].X, Tangents[0].Y, Tangents[0].Z), new Hare.Geometry.Vector(Tangents[1].X, Tangents[1].Y, Tangents[1].Z) };
                /////Get Normal
                //double up, vp;
                //ComponentIndex CI;
                //Point3d outPt;
                //Vector3d[] Normals = new Vector3d[2];
                //Brep[0].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[0]);
                //Brep[1].ClosestPoint(P.Origin, out outPt, out CI, out up, out vp, 0.01, out Normals[1]);

                //Hare.Geometry.Vector[] HNormals = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(Normals[0].X, Normals[0].Y, Normals[0].Z), new Hare.Geometry.Vector(Normals[1].X, Normals[1].Y, Normals[1].Z) };
            }
        }

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

            public RhCommon_Scene(List<Rhino.DocObjects.RhinoObject> ObjRef, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool IsAcoustic, List<Rhino.Geometry.GeometryBase> Additional_Geometry = null, List<int> Additional_Layers = null)
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
                        double[] Transparency = new double[8];
                        double[] Transmission = new double[8];
                        Mode = BObj.GetUserString("Acoustics_User");
                        double[] Scat = new double[8];

                        if (Mode == "yes")
                        {
                            AcousticsData = BObj.GetUserString("Acoustics");
                            if (AcousticsData != "")
                            {
                                Utilities.PachTools.DecodeAcoustics(AcousticsData, ref Absorption, ref Scat, ref Transparency);
                                AbsorptionData.Add(new Basic_Material(Absorption));
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
                                //TODO - reconcile transmission with buildup materials... could be specified in smart material.
                                List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                                string[] Buildup = layer.GetUserString("Buildup").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string l in Buildup) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(l));
                                AbsorptionData.Add(new Smart_Material(false ,Layers, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2));
                            }
                            if (!string.IsNullOrEmpty(AcousticsData))
                            {
                                //New code for transmission loss incorporation...
                                Utilities.PachTools.DecodeAcoustics(AcousticsData, ref Absorption, ref Scat, ref Transparency);
                                string trans = layer.GetUserString("Transmission");
                                Transmission = trans == "" ? new double[] {0,0,0,0,0,0,0,0} : Utilities.PachTools.DecodeTransmissionLoss(trans);
                                for (int oct = 0; oct < 8; oct++)
                                {
                                    double ret = Math.Pow(10, -Transmission[oct] / 10);
                                    Transmission[oct] = 1 - ret;
                                    //Absorption[oct] *= ret; 
                                }
                                AbsorptionData.Add(new Basic_Material(Absorption));
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

                        Area = new double[BrepList.Count];
                        for (int i = 0; i < BrepList.Count; i++) Area[i] = BrepList[i].GetArea();

                        ScatteringData.Add(new Lambert_Scattering(Scat, SplitRatio));
                        TransmissionData.Add(Transparency);
                        bool Trans = false;
                        for (int t_oct = 0; t_oct < 8; t_oct++)
                        {
                            if (Transparency[t_oct] > 0)
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

                if (Additional_Geometry == null) return;
                if (Additional_Layers == null) return;
                if (Additional_Geometry.Count != Additional_Layers.Count) return;

                for (int q = 0; q < Additional_Geometry.Count; q++)
                {
                    Rhino.Geometry.Brep BObj;
                    if (Additional_Geometry[q].ObjectType == Rhino.DocObjects.ObjectType.Brep)
                    {
                        BObj = (Rhino.Geometry.Brep)Additional_Geometry[q];
                    }
                    else
                    {
                        BObj = ((Rhino.Geometry.Extrusion)Additional_Geometry[q]).ToBrep();
                    }

                    for (int j = 0; j < BObj.Faces.Count; j++)
                    {
                        Brep B_Temp = BObj.DuplicateSubBrep(new List<int>() { j });
                        BrepList.Add(B_Temp);
                        string AcousticsData = null;
                        double[] Absorption = new double[8];
                        double[] Transparency = new double[8];
                        double[] Transmission = new double[8];
                        double[] Scat = new double[8];

                        Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[Additional_Layers[q]];
                        string Method = layer.GetUserString("ABSType");
                        AcousticsData = layer.GetUserString("Acoustics");
                        if (Method == "Buildup")
                        {
                            List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                            string[] Buildup = layer.GetUserString("Buildup").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string l in Buildup) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(l));
                            AbsorptionData.Add(new Smart_Material(false, Layers, 44100, Env_Prop.Rho(0), Env_Prop.Sound_Speed(0), 2));
                        }
                        if (!string.IsNullOrEmpty(AcousticsData))
                        {
                            Utilities.PachTools.DecodeAcoustics(AcousticsData, ref Absorption, ref Scat, ref Transparency);
                            AbsorptionData.Add(new Basic_Material(Absorption));
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
                    
                        Area = new double[BrepList.Count];
                        for (int i = 0; i < BrepList.Count; i++) Area[i] = BrepList[i].GetArea();

                        for (int oct = 0; oct < 8; oct++)
                        {
                            Transmission[oct] = Transparency[oct];
                        }

                        ScatteringData.Add(new Lambert_Scattering(Scat, SplitRatio));
                        TransmissionData.Add(Transmission);
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
            }

//            public RhCommon_Scene(List<Rhino.Geometry.Brep> ObjRef, double Temp, double hr, double Pa, int Air_Choice, bool EdgeCorrection, bool IsAcoustic)
//                : base(Temp, hr, Pa, Air_Choice, EdgeCorrection, IsAcoustic)
//            {
//                Vector3d NormalHolder = new Vector3d();
//                Plane PlaneHolder = new Plane();
//                Transform XHolder = new Transform();
//                Random RND = new Random();

//                for (int q = 0; q < ObjRef.Count; q++)
//                {
//                    //ObjectList.Add(new Rhino.DocObjects.ObjRef(ObjRef[q]));

//                    //Rhino.Geometry.Brep BObj;
//                    //if (ObjRef[q].ObjectType == Rhino.DocObjects.ObjectType.Brep)
//                    //{
//                    //    BObj = ((Rhino.DocObjects.BrepObject)ObjRef[q]).BrepGeometry;
//                    //}
//                    //else
//                    //{
//                    //    BObj = ((Rhino.DocObjects.ExtrusionObject)ObjRef[q]).ExtrusionGeometry.ToBrep();
//                    //}

//                    for (int j = 0; j < ObjRef[q].Faces.Count; j++)
//                    {
//                        Brep B_Temp = ObjRef[q].DuplicateSubBrep(new List<int>() { j });
//                        BrepList.Add(B_Temp);
//                        string Mode = null;
//                        double[] Absorption = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
//                        double[,] Scattering = new double[8, 3] {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}};
////                        double[] Reflection = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
//                        double[] Transparency = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
//                        double[] Transmission = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
//                        Mode = ObjRef[q].GetUserString("Acoustics_User");
//                        double[] Scat = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};
//                        double[] phase = new double[8]{0, 0, 0, 0, 0, 0, 0, 0};

//                        //ReflectionData.Add(Reflection);
//                        AbsorptionData.Add(new Basic_Material(Absorption, phase));
//                        ScatteringData.Add(new Lambert_Scattering(Scat, SplitRatio));
//                        TransmissionData.Add(Transmission);
//                        //PhaseData.Add(phase);

//                        Transmissive.Add(false);
//                        PlaneBoolean.Add(ObjRef[q].Faces[j].IsPlanar());

//                        if (PlaneBoolean[PlaneBoolean.Count - 1])
//                        {
//                            Vector3d Normal = new Vector3d();
//                            Point3d Origin = new Point3d();
//                            //Transform MirrorSingle = new Transform();
//                            //Plane PlaneSingle = new Plane();
//                            Origin = ObjRef[q].Faces[j].PointAt(0, 0);
//                            Normal = ObjRef[q].Faces[j].NormalAt(RND.NextDouble(), RND.NextDouble());
//                            Mirror.Add(Transform.Mirror(Origin, Normal));
//                            Plane.Add(new Plane(Origin, Normal));
//                            PlanarNormal.Add(Normal);
//                        }
//                        else
//                        {
//                            PlanarNormal.Add(NormalHolder);
//                            Plane.Add(PlaneHolder);
//                            Mirror.Add(XHolder);
//                        }
//                    }
//                }
//                //SurfaceArray = SurfaceList.ToArray();
//                Valid = true;
//            }

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
                S_Origin = Utilities.RC_PachTools.HPttoRPt(R.origin);
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

            public override bool shoot(Ray R, int topo, out X_Event Xpt)
            {
                double u, v, t;
                int id;
                Hare.Geometry.Point Pt;
                bool success = shoot(R, out u, out v, out id, out Pt, out t);
                if (success)
                {
                    Xpt = new X_Event(Pt, u, v, t, id);
                }
                else
                {
                    Xpt = new X_Event();
                }
                return success;
            }

            public override bool shoot(Ray R, int topo, out X_Event Xpt, int srf_origin1, int srf_origin2 = -1)
            {
                double u, v, t;
                int id;
                Hare.Geometry.Point Pt;
                bool success = shoot(R, out u, out v, out id, out Pt, out t);
                if (success)
                {
                    Xpt = new X_Event(Pt, u, v, t, id);
                }
                else
                {
                    Xpt = new X_Event();
                }
                return success;
            }

            public bool shoot(Point3d Start, Vector3d Dir, int Random, out double u, out double v, out int Srf_ID, out List<Point3d> X_PT, out List<double> t, out List<int> Code, int srf_origin1 = -1, int srf_origin2 = -1)
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
                        if (index == srf_origin1 || index == srf_origin2) continue;
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

            public void partition(Point3d[] P, int SP_PARAM)
            {
                Partitioned = true;
                partition(new List<Point3d>(P), SP_PARAM);
            }

            public override void partition(Hare.Geometry.Point[] P, int SP_PARAM)
            {
                Partitioned = true;
                partition(new List<Hare.Geometry.Point>(P), SP_PARAM);
            }

            public void partition(List<Point3d> P, int SP_PARAM)
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

            public Point3d ClosestPt(Point3d P, ref double Dist)
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
                return Utilities.RC_PachTools.RPttoHPt(Voxels.OverallBounds().Max);
            }

            public override Hare.Geometry.Point Min()
            {
                if (this.Voxels == null) return null;
                return Utilities.RC_PachTools.RPttoHPt(Voxels.OverallBounds().Min);
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
