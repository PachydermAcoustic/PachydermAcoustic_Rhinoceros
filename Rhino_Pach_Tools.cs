using System;
using System.Collections.Generic;
using System.Linq;
using Pachyderm_Acoustic.Environment;
using Rhino.Geometry;

namespace Pachyderm_Acoustic
{
    namespace Utilities
    {
        public class RC_PachTools: PachTools
        {
            /// <summary>
            /// Shorthand tool used to run a simulation.
            /// </summary>
            /// <param name="Sim">the simulation to run...</param>
            /// <returns>the completed simulation...</returns>
            public static Simulation_Type Run_Simulation(Simulation_Type Sim)
            {
                UI.PachydermAc_PlugIn plugin = UI.PachydermAc_PlugIn.Instance;
                UI.Pach_RunSim_Command command = UI.Pach_RunSim_Command.Instance;
                if (command == null) { return null; }
                command.Reset();
                command.Sim = Sim;
                Rhino.RhinoApp.RunScript("Run_Simulation", false);
                return command.Sim;
            }

            /// <summary>
            /// Tool used for debug of voxel grids and bounding boxes.
            /// </summary>
            /// <param name="MinPt"></param>
            /// <param name="MaxPt"></param>
            public static void AddBox(Point3d MinPt, Point3d MaxPt)
            {
                Rhino.RhinoDoc.ActiveDoc.Objects.AddBrep((new BoundingBox(MinPt, MaxPt)).ToBrep());
            }

            /// <summary>
            /// Shorthand tool to obtain Rhino_Scene object.
            /// </summary>
            /// <param nam``e="Rel_Humidity">in percent</param>
            /// <param name="AirTempC">in degrees C.</param>
            /// <param name="AirPressurePa">in Pascals</param>
            /// <param name="AirAttenMethod"></param>
            /// <param name="EdgeFreq">Use edge frequency correction?</param>
            /// <returns></returns>
            public static Environment.RhCommon_Scene Get_NURBS_Scene(double Rel_Humidity, double AirTempC, double AirPressurePa, int AirAttenMethod, bool EdgeFreq)
            {
                Rhino.DocObjects.ObjectEnumeratorSettings settings = new Rhino.DocObjects.ObjectEnumeratorSettings();
                settings.DeletedObjects = false;
                settings.HiddenObjects = false;
                settings.LockedObjects = true;
                settings.NormalObjects = true;
                settings.VisibleFilter = true;
                settings.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep & Rhino.DocObjects.ObjectType.Surface & Rhino.DocObjects.ObjectType.Extrusion;
                List<Rhino.DocObjects.RhinoObject> RC_List = new List<Rhino.DocObjects.RhinoObject>();
                foreach (Rhino.DocObjects.RhinoObject RHobj in Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(settings))
                {
                    if (RHobj.ObjectType == Rhino.DocObjects.ObjectType.Brep || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Surface || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        RC_List.Add(RHobj);
                    }
                }
                if (RC_List.Count != 0)
                {
                    return new Environment.RhCommon_Scene(RC_List, AirTempC, Rel_Humidity, AirPressurePa, AirAttenMethod, EdgeFreq, false);
                }
                return null;
            }

            /// <summary>
            /// Shorthand tool to obtain Polygon_Scene object.
            /// </summary>
            /// <param name="Rel_Humidity">in percent</param>
            /// <param name="AirTempC">in degrees C.</param>
            /// <param name="AirPressurePa">in Pascals</param>
            /// <param name="AirAttenMethod"></param>
            /// <param name="EdgeFreq">Use edge frequency correction?</param>
            /// <returns></returns>
            public static Environment.Polygon_Scene Get_Poly_Scene(double Rel_Humidity, bool edges, double AirTempC, double AirPressurePa, int AirAttenMethod, bool EdgeFreq)
            {
                Rhino.DocObjects.ObjectEnumeratorSettings settings = new Rhino.DocObjects.ObjectEnumeratorSettings();
                settings.DeletedObjects = false;
                settings.HiddenObjects = false;
                settings.LockedObjects = true;
                settings.NormalObjects = true;
                settings.VisibleFilter = true;
                settings.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep & Rhino.DocObjects.ObjectType.Surface & Rhino.DocObjects.ObjectType.Extrusion;
                List<Rhino.DocObjects.RhinoObject> RC_List = new List<Rhino.DocObjects.RhinoObject>();
                foreach (Rhino.DocObjects.RhinoObject RHobj in Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(settings))
                {
                    if (RHobj.ObjectType == Rhino.DocObjects.ObjectType.Brep || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Surface || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        RC_List.Add(RHobj);
                    }
                }
                if (RC_List.Count != 0)
                {
                    return new Environment.RhCommon_PolygonScene(RC_List, edges, AirTempC, Rel_Humidity, AirPressurePa, AirAttenMethod, EdgeFreq, false);
                }
                return null;
            }

            public static void Plot_Hare_Topology(Hare.Geometry.Topology T)
            {
                Mesh m_RhinoMesh = Hare_to_RhinoMesh(T, true);
                m_RhinoMesh.FaceNormals.ComputeFaceNormals();
                m_RhinoMesh.Normals.ComputeNormals();
                Rhino.RhinoDoc.ActiveDoc.Objects.Add(m_RhinoMesh);
            }

            public static Hare.Geometry.Topology Rhino_to_HareMesh(Mesh M)
            {
                Hare.Geometry.Point[][] polys = new Hare.Geometry.Point[M.Faces.Count][];

                for(int i = 0; i < M.Faces.Count; i++)
                {
                    Hare.Geometry.Point[] pts;
                    if (M.Faces[i].IsTriangle)
                    {
                        pts = new Hare.Geometry.Point[3];
                        pts[0] = RPttoHPt(M.Vertices[M.Faces[i].A]);
                        pts[1] = RPttoHPt(M.Vertices[M.Faces[i].B]);
                        pts[2] = RPttoHPt(M.Vertices[M.Faces[i].C]);
                    }else
                    {
                        pts = new Hare.Geometry.Point[4];
                        pts[0] = RPttoHPt(M.Vertices[M.Faces[i].A]);
                        pts[1] = RPttoHPt(M.Vertices[M.Faces[i].B]);
                        pts[2] = RPttoHPt(M.Vertices[M.Faces[i].C]);
                        pts[3] = RPttoHPt(M.Vertices[M.Faces[i].D]);
                    }
                    polys[i] = pts;
                }

                Hare.Geometry.Topology HM = new Hare.Geometry.Topology(polys);
                HM.Finish_Topology(new List<Hare.Geometry.Point>());
                return HM;
            }

            public static Mesh Hare_to_RhinoMesh(Hare.Geometry.Topology T, bool welded)
            {
                Mesh m_RhinoMesh = new Mesh();
                int ct = 0;

                if (welded)
                {
                    for (int j = 0; j < T.Vertex_Count; j++)
                    {
                        m_RhinoMesh.Vertices.Add(Utilities.RC_PachTools.HPttoRPt(T[j]));
                    }
                    for (int j = 0; j < T.Polygon_Count; j++)
                    {
                        if (T.Polys[j].Points.Count() == 3)
                        {
                            m_RhinoMesh.Faces.AddFace(T.Polys[j].Points[0].index, T.Polys[j].Points[1].index, T.Polys[j].Points[2].index);
                        }
                        else if (T.Polys[j].Points.Count() == 4)
                        {
                            m_RhinoMesh.Faces.AddFace(T.Polys[j].Points[0].index, T.Polys[j].Points[1].index, T.Polys[j].Points[2].index, T.Polys[j].Points[3].index);
                        }
                        else
                        {
                            throw new Exception("Faces of more than 4 vertices not supported...");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < T.Polygon_Count; i++)
                    {
                        Hare.Geometry.Point[] Pt = T.Polygon_Vertices(i);
                        int[] F = new int[T.Polys[i].VertextCT];
                        for (int j = 0; j < T.Polys[i].VertextCT; j++)
                        {
                            m_RhinoMesh.Vertices.Add(new Point3d(Pt[j].x, Pt[j].y, Pt[j].z));
                            F[j] = ct;
                            ct++;
                        }
                        if (F.Length == 3)
                        {
                            m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0]);
                        }
                        else
                        {
                            m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0], F[3]);
                        }
                    }
                }
                m_RhinoMesh.Normals.ComputeNormals();

                return m_RhinoMesh;
            }

            /// <summary>
            /// Shorthand tool for map mesh objects.
            /// </summary>
            /// <param name="Map_Srf">A NURBS surface to mesh.</param>
            /// <param name="Increment">the maximum dimension between vertices.</param>
            /// <returns>the map mesh object.</returns>
            public static Mesh Create_Map_Mesh(IEnumerable<Brep> Map_Srf, double Increment)
            {
                Mesh Map_Mesh = new Mesh();
                MeshingParameters mp = new MeshingParameters();
                mp.MaximumEdgeLength = Increment;
                mp.MinimumEdgeLength = Increment;
                mp.SimplePlanes = false;
                mp.JaggedSeams = false;
                Brep[] Srfs = Map_Srf.ToArray<Brep>();
                for (int i = 0; i < Map_Srf.ToArray<Brep>().Length; i++)
                {
                    Mesh m = Rhino.Geometry.Mesh.CreateFromBrep(Srfs[i], mp)[0];
                    Point3d pt;
                    double s, t;
                    ComponentIndex ci;
                    Vector3d Snormal;
                    Srfs[i].ClosestPoint(m.Vertices[m.Vertices.Count() / 2], out pt, out ci, out s, out t, 1.0f, out Snormal);
                    Vector3d Mnormal = m.Normals[m.Vertices.Count() / 2];
                    if ((Snormal.X * Mnormal.X + Snormal.Y * Mnormal.Y + Snormal.Z * Mnormal.Z) < 0) m.Flip(true, true, true);
                    Map_Mesh.Append(m);
                }

                return Map_Mesh;
            }

            /// <summary>
            /// Sets materials for an object by object.
            /// </summary>
            /// <param name="ID">object GUID (UUID)</param>
            /// <param name="Abs">0 to 100</param>
            /// <param name="Scat">0 to 100</param>
            /// <param name="Trans">0 to 100</param>
            /// <returns></returns>
            public static bool Material_SetByObject(Guid ID, int[] Abs, int[] Scat, int[] Trans)
            {
                Rhino.DocObjects.RhinoObject obj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(ID);
                obj.Geometry.SetUserString("Acoustics_User", "yes");
                string MaterialCode = RC_PachTools.EncodeAcoustics(Abs, Scat, Trans);
                bool result = obj.Geometry.SetUserString("Acoustics", MaterialCode);
                return result;
            }

            /// <summary>
            /// Sets materials for an object by layer.
            /// </summary>
            /// <param name="ID">object GUID (UUID)</param>
            /// <returns></returns>
            public static bool Material_SetObjectToLayer(Guid ID)
            {
                Rhino.DocObjects.RhinoObject obj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(ID);
                bool result = obj.Geometry.SetUserString("Acoustics_User", "no");
                Rhino.RhinoDoc.ActiveDoc.Objects.ModifyAttributes(obj, obj.Attributes, true);
                return result;
            }

            /// <summary>
            /// Sets the material association for a layer.
            /// </summary>
            /// <param name="LayerName"></param>
            /// <param name="Abs">0 to 100</param>
            /// <param name="Scat">0 to 100</param>
            /// <returns></returns>
            public static bool Material_SetLayer(string LayerName, int[] Abs, int[] Scat, int[] Trans = null)
            {
                int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerName, true);
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                layer.SetUserString("Acoustics", RC_PachTools.EncodeAcoustics(Abs, Scat, Trans));
                return Rhino.RhinoDoc.ActiveDoc.Layers.Modify(layer, layer_index, false);
            }

            /// <summary>
            /// Sets the material association for a layer.
            /// </summary>
            /// <param name="LayerIndex"></param>
            /// <param name="Abs">0 to 100</param>
            /// <param name="Scat">0 to 100</param>
            /// <returns></returns>
            public static bool Material_SetLayer(int LayerIndex, int[] Abs, int[] Scat, int[] Trans)
            {
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[LayerIndex];
                layer.SetUserString("ABSType", "");
                layer.SetUserString("Acoustics", RC_PachTools.EncodeAcoustics(Abs, Scat, Trans));
                return Rhino.RhinoDoc.ActiveDoc.Layers.Modify(layer, LayerIndex, false);
            }

            /// <summary>
            /// Obtain source objects already in model.
            /// </summary>
            /// <returns></returns>
            public static Hare.Geometry.Point[] GetSource()
            {
                UI.PachydermAc_PlugIn p = UI.PachydermAc_PlugIn.Instance;
                Hare.Geometry.Point[] SPT;
                p.SourceOrigin(out SPT);
                return SPT;
            }

            /// <summary>
            /// Obtain source objects already in model.
            /// </summary>
            /// <returns></returns>
            public static Source[] GetSource(int No_of_Rays)
            {
                UI.PachydermAc_PlugIn p = UI.PachydermAc_PlugIn.Instance;
                Source[] Srcs;
                p.Source(out Srcs);
                return Srcs;
            }


            /// <summary>
            /// Add two points together.    `
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3d PT, Point3d PT2)
            {
                return new Point3d(PT.X + PT2.X, PT.Y + PT2.Y, PT.Z + PT2.Z);
            }

            /// <summary>
            /// Add two points together.
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3f PT, Point3d PT2)
            {
                return new Point3d(PT.X + PT2.X, PT.Y + PT2.Y, PT.Z + PT2.Z);
            }

            /// <summary>
            /// Add two points together.
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3d PT, Vector3d PT2)
            {
                return new Point3d(PT.X + PT2.X, PT.Y + PT2.Y, PT.Z + PT2.Z);
            }

            /// <summary>
            /// Add two points together.
            /// </summary>
            /// <param name="PT"></param>
            /// <param name="PT2"></param>
            /// <returns></returns>
            public static Point3d AddPTPT(Point3f PT, Vector3d PT2)
            {
                return new Point3d(PT.X + PT2.X, PT.Y + PT2.Y, PT.Z + PT2.Z);
            }

            /// <summary>
            /// Convenient Point3d cast.
            /// </summary>
            /// <param name="PT"></param>
            /// <returns></returns>
            public static Point3d castPoint3d(Point3f PT)
            {
                return new Point3d(PT.X, PT.Y, PT.Z);
            }

            /// <summary>
            /// Obtain receiver objects already in model.
            /// </summary>
            /// <returns></returns>
            public static List<Hare.Geometry.Point> GetReceivers()
            {
                UI.PachydermAc_PlugIn p = UI.PachydermAc_PlugIn.Instance;
                List<Hare.Geometry.Point> RPT;
                p.Receiver(out RPT);

                return RPT;
            }

            /// <summary>
            /// Easily constructs a receiver bank from custom data. Hard codes sample resolution of simulation to 1000 samples per second.
            /// </summary>
            /// <param name="ReceiverLocations">enumerable of receiver pts.</param>
            /// <param name="SrcLocations">enumerable of source pts.</param>
            /// <param name="No_of_Rays">number of rays</param>
            /// <param name="CutOffTime">number of milliseconds in simulation</param>
            /// <param name="Typ">0 for stationary, 1 for variable.</param>
            /// <param name="Sc">Scene object contains air attenuation, and speed of sound.</param>
            /// <returns></returns>
            public static List<Receiver_Bank> GetReceivers(IEnumerable<Hare.Geometry.Point> ReceiverLocations, IEnumerable<Source> Srcs, int No_of_Rays, int CutOffTime, int Typ, Scene Sc)
            {
                Receiver_Bank.Type RecType;
                switch (Typ)
                {
                    case 0:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                    case 1:
                        RecType = Receiver_Bank.Type.Variable;
                        break;
                    default:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                }

                List<Receiver_Bank> R = new List<Receiver_Bank>();

                for (int i = 0; i < Srcs.Count<Source>(); i++)
                {
                    R.Add(new Receiver_Bank(ReceiverLocations, (Srcs.ElementAt<Source>(i)).Origin(), Sc, 1000, CutOffTime, RecType));
                }

                return R;
            }

            /// <summary>
            /// Easily constructs a receiver bank from custom data.
            /// </summary>
            /// <param name="ReceiverLocations">enumerable of receiver pts.</param>
            /// <param name="SrcLocations">enumerable of source pts.</param>
            /// <param name="No_of_Rays">number of rays</param>
            /// <param name="CutOffTime">number of milliseconds in simulation</param>
            /// <param name="Typ">0 for stationary, 1 for variable.</param>
            /// <param name="Sc">Scene object contains air attenuation, and speed of sound.</param>
            /// <returns></returns>
            public static List<Receiver_Bank> GetReceivers(IEnumerable<Hare.Geometry.Point> ReceiverLocations, IEnumerable<Source> Srcs, int No_of_Rays, int CutOffTime, int Typ, Scene Sc, int sample_rate)
            {
                Receiver_Bank.Type RecType;
                switch (Typ)
                {
                    case 0:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                    case 1:
                        RecType = Receiver_Bank.Type.Variable;
                        break;
                    default:
                        RecType = Receiver_Bank.Type.Stationary;
                        break;
                }

                List<Receiver_Bank> R = new List<Receiver_Bank>();

                for (int i = 0; i < Srcs.Count<Source>(); i++)
                {
                    R.Add(new Receiver_Bank(ReceiverLocations, Srcs.ElementAt<Source>(i).Origin(), Sc, sample_rate, CutOffTime, RecType));
                }

                return R;
            }

            /// <summary>
            /// Creates color map mesh from user defined parameters and a data set.
            /// </summary>
            /// <param name="mesh">the mesh to use for parameter mapping.</param>
            /// <param name="scale_enum">an integer indicating which color scale to use.</param>
            /// <param name="Values">the list of values calculated for each point.</param>
            /// <param name="LBound">the lower bound of the scale.</param>
            /// <param name="UBound">the upper bound of the scale.</param>
            /// <returns>the colored map.</returns>
            public static string CreateMap(Mesh mesh, int scale_enum, double[] Values, double LBound, double UBound)
            {
                double H_OFFSET;
                double H_BREADTH;
                double S_OFFSET;
                double S_BREADTH;
                double V_OFFSET;
                double V_BREADTH;

                if (Values.Length != mesh.Vertices.Count) return System.Guid.Empty.ToString();

                Pach_Graphics.HSV_colorscale c_scale;

                System.Drawing.Color[] Colors;
                switch (scale_enum)
                {
                    case 0:
                        H_OFFSET = 0;
                        H_BREADTH = 4.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 24);
                        break;
                    case 1:
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 0;
                        H_BREADTH = 1.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    case 2:
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = Math.PI / 3.0;
                        H_BREADTH = 1.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    case 3:
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 0;
                        H_BREADTH = -2.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    case 4:
                        H_OFFSET = 0;
                        H_BREADTH = 0;
                        S_OFFSET = 0;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = -1;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Color selection invalid... Most obnoxious color imaginable substituted!");
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 2.0 * Math.PI / 3;
                        H_BREADTH = 0;
                        S_OFFSET = 1;
                        S_BREADTH = -1;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(1, 1, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, false, 12);
                        break;
                }

                double Scale_Breadth = UBound - LBound;

                for (int i = 0; i < Values.Length; i++)
                {
                    System.Drawing.Color color = c_scale.GetValue(Values[i], LBound, UBound);
                    mesh.VertexColors.SetColor(i, color);
                }

                return Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(mesh).ToString();
            }

            /// <summary>
            /// Adds dataset to the custom mapping control.
            /// </summary>
            /// <param name="Title">the alias for the dataset.</param>
            /// <param name="Values">the calculated values for each point.</param>
            /// <param name="M">the mesh to be used.</param>
            public static void AddToCMControl(string Title, double[] Values, Mesh M)
            {
                UI.PachydermAc_PlugIn Pach = UI.PachydermAc_PlugIn.Instance;
                UI.Pach_MapCustom.Add_Result(Title, Values, M);
            }

            /// <summary>
            /// Clears the custom mapping control of all stored data.
            /// </summary>
            public static void ClearCMControl()
            {
                UI.PachydermAc_PlugIn Pach = UI.PachydermAc_PlugIn.Instance;
                UI.Pach_MapCustom.Clear();
            }

            /// <summary>
            /// Casts Rhino point to Hare point
            /// </summary>
            /// <param name="Point"></param>
            /// <returns></returns>
            public static Hare.Geometry.Point RPttoHPt(Point3d Point)
            {
                return new Hare.Geometry.Point(Point.X, Point.Y, Point.Z);
            }

            public static Hare.Geometry.Vector RPttoHPt(Vector3d Point)
            {
                return new Hare.Geometry.Vector(Point.X, Point.Y, Point.Z);
            }

            /// <summary>
            /// Casts Hare point to Rhino point
            /// </summary>
            /// <param name="Point"></param>
            /// <returns></returns>
            public static Point3d HPttoRPt(Hare.Geometry.Point Point)
            {
                return new Point3d(Point.x, Point.y, Point.z);
            }

            public static Vector3d HPttoRPt(Hare.Geometry.Vector Point)
            {
                return new Vector3d(Point.x, Point.y, Point.z);
            }

            /// <summary>
            /// Displays custom mapping control.
            /// </summary>
            public static void Show_CM_Control()
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("1c48c00e-abd8-40fd-8642-2ce7daa90ed5"));
            }

            public static class Ray_Acoustics
            {
                /// <summary>
                /// Calculates the direction of a specular reflection.
                /// </summary>
                /// <param name="R"></param>
                /// <param name="u"></param>
                /// <param name="v"></param>
                /// <param name="Face_ID"></param>
                public static void SpecularReflection(ref Hare.Geometry.Vector R, ref Environment.Scene Room, ref double u, ref double v, ref int Face_ID)
                {
                    Hare.Geometry.Vector local_N = Room.Normal(Face_ID, u, v);
                    R -= local_N * Hare.Geometry.Hare_math.Dot(R, local_N) * 2;
                }
            }

            public static Mesh PlotMesh(PachMapReceiver[] Rec_List, System.Drawing.Color[] C)
            {
                Mesh MM = Hare_to_RhinoMesh(Rec_List[0].Map_Mesh, Rec_List[0].Rec_Vertex);

                if (!Rec_List[0].Rec_Vertex)
                {
                    Mesh MF = new Mesh();
                    for (int i = 0; i < MM.Faces.Count; i++)
                    {
                        if (MM.Faces[i].IsQuad)
                        {
                            MF.Vertices.Add(MM.Vertices[MM.Faces[i].A]);
                            MF.Vertices.Add(MM.Vertices[MM.Faces[i].B]);
                            MF.Vertices.Add(MM.Vertices[MM.Faces[i].C]);
                            MF.Vertices.Add(MM.Vertices[MM.Faces[i].D]);
                            int f = MF.Vertices.Count - 4;
                            MF.Faces.AddFace(f, f + 1, f + 2, f + 3);
                            MF.VertexColors.SetColor(f, C[i]);
                            MF.VertexColors.SetColor(f + 1, C[i]);
                            MF.VertexColors.SetColor(f + 2, C[i]);
                            MF.VertexColors.SetColor(f + 3, C[i]);
                        }
                        else
                        {
                            MF.Vertices.Add(MM.Vertices[MM.Faces[i].A]);
                            MF.Vertices.Add(MM.Vertices[MM.Faces[i].B]);
                            MF.Vertices.Add(MM.Vertices[MM.Faces[i].C]);
                            int f = MF.Vertices.Count - 3;
                            MF.Faces.AddFace(f, f + 1, f + 2);
                            MF.VertexColors.SetColor(f, C[i]);
                            MF.VertexColors.SetColor(f + 1, C[i]);
                            MF.VertexColors.SetColor(f + 2, C[i]);
                        }
                    }
                    return MF;
                }
                else
                {
                    MM.VertexColors.SetColors(C);
                    return MM;
                }
            }

            public static void PlotMapValues(PachMapReceiver[] Rec_List, double[] Values)
            {
                int step = (int)Math.Ceiling((double)Rec_List[0].Rec_List.Length / 100);
                for (int i = 0; i < Rec_List[0].Rec_List.Length; i += step)
                {
                    Plane P = Plane.WorldXY;
                    P.Origin = HPttoRPt(Rec_List[0].Rec_List[i].Origin);
                    Rhino.RhinoDoc.ActiveDoc.Objects.AddText(((int)Values[i]).ToString(), P, Rec_List[0].Rec_List[0].Radius, "Arial", true, false);
                }
            }
        }
    }
}