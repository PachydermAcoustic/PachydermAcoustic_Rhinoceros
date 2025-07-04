//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2025, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit 
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

using Rhino.PlugIns;
using Rhino.Geometry;
using System.Collections.Generic;
using System;
using Rhino.UI;
using Rhino.Runtime;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary>
        /// Every Rhino.NET Plug-In must have one and only one MRhinoPlugIn derived C:\Users\User\Desktop\Pachyderm_Acoustic_2.0\Pachyderm_Acoustic\Pachyderm_AcousticPlugIn.cs
        /// class. DO NOT create an instance of this class. It is the responsibility 
        /// of Rhino.NET to create an instance of this class and register it with Rhino. 
        ///</summary>
        public class PachydermAc_PlugIn : Rhino.PlugIns.PlugIn
        {
            public static Guid Instance_ID = Guid.NewGuid();
            public Pach_Properties_Panel Pach_Props;

            public PachydermAc_PlugIn()
            {
                HostUtils.SendDebugToCommandLine = true;
                new SourceConduit();
                new ReceiverConduit();
                Pach_Props = Pach_Properties_Panel.Instance;
                Audio.Pach_SP.Initialize_FFTW();
                //System.AppDomain.CurrentDomain.AssemblyResolve += GetAssemblies;
                Instance = this;
            }

            ///<summary>Gets the only instance of the PachydermAcoustic plug-in.</summary>
            public static PachydermAc_PlugIn Instance
            {
                get;
                private set;
            }
                
            protected override LoadReturnCode OnLoad(ref string errorMessage)            
            {
                //the following should always be the case, but just to be paranoid 
                //Pach_Splash splash = new Pach_Splash();

                ////splash.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
                //splash.Closed += (sender, e) =>
                //{
                //    if (splash.pmode == 0)
                //    {
                //        splash.pmode++;
                //        splash.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
                //    }
                //    else
                //        return;
                //};

                //Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_Splash), "PachyDerm Splash", Properties.Resources.PIcon1);
                //Rhino.UI.Panels.OpenPanel(new Guid("E022769F-AAF6-45BA-9EB6-CF0391E0B239"));


                //Register the UserControl "Panels"
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.PachTDNumericControl), "PachyDerm TimeDomain Models", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.PachHybridControl), "Pachyderm Hybrid Models", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_Mapping_Control), "Pachyderm Mapping Method", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_MapCustom), "Pachyderm Custom Maps", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.PachVisualControl), "Pachyderm Particle Animation", Properties.Resources.PIcon1);
                //Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_Auralisation), "Pachyderm Auralisation", Properties.Resources.PIcon1);
                //Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Convergence_Progress), "Convergence Progress", Properties.Resources.PIcon1);

                Rhino.RhinoApp.Initialized += (sender, e) =>
                {
                    PachSplash splash = new PachSplash(0);
                    PachSplash splash2 = new PachSplash(1);
                    splash2.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
                    //System.Threading.Thread.Sleep(3000);
                    //splash.Close();
                    splash.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
                    //await System.Threading.Tasks.Task.Delay(5000);
                    //System.Threading.Thread.Sleep(3000);
                };

                return LoadReturnCode.Success;
            }

            public UI.Pach_Props_Page m_Doc_Page = new UI.Pach_Props_Page();
            public UI.Pach_Materials_Page m_Obj_Page = new UI.Pach_Materials_Page();
            public UI.Pach_SourceControl_Page m_Source_Page = new UI.Pach_SourceControl_Page();

            [Obsolete]
            protected override void ObjectPropertiesPages(List<Rhino.UI.ObjectPropertiesPage> pages)
            {
                pages.Add(m_Obj_Page);
                pages.Add(m_Source_Page);
            }

            protected override void OptionsDialogPages(List<Rhino.UI.OptionsDialogPage> pages)
            {
                pages.Add(m_Doc_Page);
            }

            public int TaskPriority => m_Doc_Page.Get_TaskPriority();

            public int Geometry_Spec => m_Doc_Page.Get_Geometry_Spec();

            public int SPSpec => m_Doc_Page.Get_SP_Spec();

            public int OctDepth => m_Doc_Page.Get_Oct_Depth();

            public static int VGDomain => Pach_Props_Page.VGDomain;

            public static string MLPath => Pach_Props_Page.MatLibPath;

            public static bool SaveResults => Pach_Props_Page.SaveResults();

            private string specialCode = "";

            public bool SourceRef(out List<Rhino.DocObjects.RhinoObject> Points)
            {
                UI.Pach_Source_Object S_command = Pach_Source_Object.Instance;
                Points = new List<Rhino.DocObjects.RhinoObject>();

                for (int i = 0; i < SourceConduit.Instance.UUID.Count; i++)
                {
                    System.Guid S_ID = SourceConduit.Instance.UUID[0];
                    if (S_ID != System.Guid.Empty || S_ID != System.Guid.NewGuid())
                    {
                        Points.Add(Rhino.RhinoDoc.ActiveDoc.Objects.Find(S_ID));        
                    }
                }
                if (Points.Count > 0) return true;
                
                Points = null;
                return false;
            }

            public bool SourceOrigin(out Hare.Geometry.Point[] Points)
            {
                //Pach_Source_Object c = Pach_Source_Object.Instance;
                Points = new Hare.Geometry.Point[SourceConduit.Instance.UUID.Count];
                for (int i = 0; i < SourceConduit.Instance.UUID.Count; i++)
                {
                    System.Guid S_ID = SourceConduit.Instance.UUID[i];
                    if (S_ID == System.Guid.Empty || S_ID == System.Guid.NewGuid()) break;
                    Points[i] = Utilities.RCPachTools.RPttoHPt(Rhino.RhinoDoc.ActiveDoc.Objects.Find(S_ID).Geometry.GetBoundingBox(true).Min);
                }

                if (Points.Length > 0) return true;
                return false;
            }
             
            public string GetPluginPath => System.Reflection.Assembly.GetExecutingAssembly().Location;

            public string SpecialCode { get => specialCode; set => specialCode = value; }

            public bool Source(out Environment.Source[] Srcs)
            {
                System.Guid[] S_ID = SourceConduit.Instance.UUID.ToArray();
                List<Environment.Source> S = new List<Environment.Source>();
                Dictionary<String,List<Environment.Source>> s_dict = new Dictionary<String, List<Environment.Source>>();

                for (int id = 0; id < S_ID.Length; id++)
                {
                    if (S_ID[id] == System.Guid.Empty || S_ID[id] == System.Guid.NewGuid()) break;
                    Rhino.DocObjects.RhinoObject Origin = Rhino.RhinoDoc.ActiveDoc.Objects.Find(S_ID[id]);

                    if (Origin.ObjectType == Rhino.DocObjects.ObjectType.Point)
                    {
                        string S_Type = Origin.Geometry.GetUserString("SourceType");
                        string SWL = Origin.Geometry.GetUserString("SWL");
                        string D = Origin.Geometry.GetUserString("Delay");
                        double delay; if (D != "" && D != null) delay = double.Parse(D) / 1000; else delay = 0;
                        string cluster = Origin.Geometry.GetUserString("Cluster");

                        List<Environment.Source> c_list = new List<Environment.Source>();

                        if (cluster != null && cluster != "")
                        {
                            if (!s_dict.TryGetValue(cluster, out List<Environment.Source> sources))
                            {
                                c_list = new List<Environment.Source>();
                                s_dict.Add(cluster, c_list);
                            }
                            else c_list = sources;
                        }
                        else 
                        {
                            cluster = null;
                        }

                        string Ph = Origin.Geometry.GetUserString("Phase");
                        //double[] phase = new double[8];
                        //if (Ph != "")
                        //{
                        //    string[] phstr = Ph.Split(";"[0]);
                        //    for (int o = 0; o < 8; o++) phase[o] = double.Parse(phstr[o]);
                        //}

                        double[] SWL_Values = Utilities.PachTools.DecodeSourcePower(SWL);
                        Pachyderm_Acoustic.Environment.Source s = null;

                        switch (S_Type)
                        {
                            case "":
                            case "0":
                                s = new Environment.GeodesicSource(SWL_Values, Utilities.RCPachTools.RPttoHPt(Origin.Geometry.GetBoundingBox(true).Min), id, false);
                                break;
                            case "1":
                                s = new Environment.RandomSource(SWL_Values, Utilities.RCPachTools.RPttoHPt(Origin.Geometry.GetBoundingBox(true).Min), id, false);
                                break;
                            case "2":
                            case "3":
                                string Bands = Origin.Geometry.GetUserString("Bands");
                                string[] B;
                                B = new string[2] { "0", "7" };
                                SourceConduit SC = SourceConduit.Instance;
                                s = new Environment.DirectionalSource(SC.m_Balloons[id], SWL_Values, Utilities.RCPachTools.RPttoHPt(Origin.Geometry.GetBoundingBox(true).Min), new int[] { int.Parse(B[0]), int.Parse(B[1]) }, id, false);
                                break;
                        }

                        if (cluster == null) { S.Add(s); }
                        else { c_list.Add(s); }
                    }
                    else if (Origin.ObjectType == Rhino.DocObjects.ObjectType.Brep)
                    {
                        //string SWL = Origin.Geometry.GetUserString("SWL");

                        //string Ph = Origin.Geometry.GetUserString("Phase");
                        //double[] phase = new double[8];
                        //if (Ph != "")
                        //{
                        //    string[] phstr = Ph.Split(";"[0]);
                        //    for (int o = 0; o < 8; o++) phase[o] = double.Parse(phstr[o]);
                        //}
                        //double el_m;
                        //double area = Brep.GetArea();
                        //Hare.Geometry.Topology t = new Hare.Geometry.Topology();
                        //Hare.Geometry.Point Samples;
                        ////TODO: develope discretization of surface
                        ////new Brep[] { (Origin.Geometry as Brep) };

                        //S[id] = new Environment.SurfaceSource(Samples, t, SWL, area, el_m, id, Environment.Source.Phase_Regime.Random); 
                    }
                    else if (Origin.ObjectType == Rhino.DocObjects.ObjectType.Curve)
                    {
                        string SWL = Origin.Geometry.GetUserString("SWL");

                        Rhino.Geometry.Point3d[] pts = (Origin.Geometry as Curve).DivideEquidistant(1d / 4d);
                        if (pts == null || pts.Length == 0) pts = new Point3d[1] { (Origin.Geometry as Curve).PointAtNormalizedLength(0.5) };
                        Hare.Geometry.Point[] Samples = new Hare.Geometry.Point[pts.Length];

                        for (int i = 0; i < pts.Length; i++)
                        {
                            Samples[i] = Utilities.RCPachTools.RPttoHPt(pts[i]);
                        }
                        S[id] = new Environment.LineSource(Samples, (Origin.Geometry as Curve).GetLength(), SWL, 4, id, false);
                    }
                }
                //If there are any clusters, add them to the list
                foreach (KeyValuePair<string, List<Environment.Source>> kvp in s_dict)
                {
                    List<Environment.Source> c_list = kvp.Value;
                    Environment.SourceCluster cs = new Environment.SourceCluster(c_list,S.Count);
                    S.Add(cs);
                }
                if (S.Count > 0)
                {
                    Srcs = S.ToArray();
                    return true;
                }
                Srcs = new Environment.Source[0];
                return false;
            }

            public bool Receiver(out List<Hare.Geometry.Point> Point)
            {
                UI.Pach_Receiver_Object c = Pach_Receiver_Object.Instance;
                Point = new List<Hare.Geometry.Point>();

                for (int i = 0; i < ReceiverConduit.Instance.UUID.Count; i++)
                {
                    System.Guid R_ID = ReceiverConduit.Instance.UUID[i];
                    if (R_ID == System.Guid.Empty || R_ID == System.Guid.NewGuid()) break;
                    Point.Add(Utilities.RCPachTools.RPttoHPt(Rhino.RhinoDoc.ActiveDoc.Objects.Find(R_ID).Geometry.GetBoundingBox(true).Min));
                }

                if (Point.Count > 0) return true;
                return false;
            }

            protected override bool ShouldCallWriteDocument(Rhino.FileIO.FileWriteOptions options)
            {
                //only return true if you REALLY want to save something to the document
                //that is about to be written to disk
                if (options.WriteSelectedObjectsOnly) return false;
                if (options.FileVersion < 4) return false;

                return true;
            }

            //If any ON_BinaryArchive::Write*() functions return false than you should 
            //immediately return false otherwise return true if all data was written 
            //successfully. Returning false will cause Rhino to stop writing this document. 
            protected override void WriteDocument(Rhino.RhinoDoc doc, Rhino.FileIO.BinaryArchiveWriter archive, Rhino.FileIO.FileWriteOptions options)
            {
                //This function is called because ShouldCallWriteDocument returned True. 
                //Write your plug-in data to the document 

                string date_string = System.DateTime.Now.ToShortDateString();
                string time_string = System.DateTime.Now.ToShortTimeString();

                //It is a good idea to always start with a version number 
                //so you can modify your document read/write code in the future 
                archive.Write3dmChunkVersion(1, 0);
                archive.WriteString(date_string);
                archive.WriteString(time_string);

                UI.Pach_Source_Object S_command = Pach_Source_Object.Instance;
                UI.Pach_Receiver_Object R_command = Pach_Receiver_Object.Instance;

                foreach (System.Guid ID in SourceConduit.Instance.UUID)
                {
                    System.Guid N_ID = new System.Guid(ID.ToString());
                    archive.WriteGuid(N_ID);
                    archive.WriteString("Source");
                }

                foreach (System.Guid ID in ReceiverConduit.Instance.UUID)
                {
                    System.Guid N_ID = new System.Guid(ID.ToString());
                    archive.WriteGuid(N_ID);
                    archive.WriteString("Receiver");
                }
            }

            //Called whenever a Rhino document is being loaded and plug-in user data was 
            //encountered written by a plug-in with this plug-in's GUID. 
            // 
            //If any ON_BinaryArchive::Read*() functions return false than you should 
            //immediately return false otherwise return true when all data was read. 
            protected override void ReadDocument(Rhino.RhinoDoc doc, Rhino.FileIO.BinaryArchiveReader archive, Rhino.FileIO.FileReadOptions options)
            {
                //Always read data in the EXACT same order you wrote it 
                int major, minor;
                archive.Read3dmChunkVersion(out major, out minor);

                //If you've changed your reading/writing code over time, 
                //you can use the version number of what you read 
                //to figure out what can be read from the archive 
                if ((major > 1 | minor > 0)) return;

                //the data you read/write will probably be member variables of your plug-in class, 
                //but for simplicity this sample is just using locally defined strings 
                string date_string = archive.ReadString();
                string time_string = archive.ReadString();

                //Get the commands for "Insert_Source" and "Insert_Receiver". This is where the Source and Receiver conduits are stored.
                UI.Pach_Source_Object S_command = Pach_Source_Object.Instance;
                UI.Pach_Receiver_Object R_command = Pach_Receiver_Object.Instance;

                System.Guid objectId = default(System.Guid);
                string Type = null;
                do
                {
                    try
                    {
                        objectId = archive.ReadGuid();
                        Type = archive.ReadString();
                        if (Type == "Source")
                        {
                            Rhino.DocObjects.RhinoObject Source = doc.Objects.Find(objectId);
                            if (Source != null)
                            {
                                SourceConduit.Instance.SetSource(Source);
                                doc.Views.Redraw();
                            }
                        }
                        else if (Type == "Receiver")
                        {
                            Rhino.DocObjects.RhinoObject Receiver = doc.Objects.Find(objectId);
                            if (Receiver != null)
                            {
                                ReceiverConduit.Instance.SetReceiver(Receiver);
                                doc.Views.Redraw();
                            }
                        }
                        Type = null;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                while (true);
            }

            /// <summary>
            /// Returns the Sound Power Level of the specified source.
            /// </summary>
            /// <returns></returns>
            public double[] GetSourceSWL(int index)
            {
                List<Rhino.DocObjects.RhinoObject> P;
                if (!this.SourceRef(out P)) return new double[8]{120,120,120,120,120,120,120,120};
                string Code = P[index].Geometry.GetUserString("SWL");
                if (Code == null) return new double[8]{120,120,120,120,120,120,120,120}; 
                return Utilities.PachTools.DecodeSourcePower(Code);
            }
        }
    }
}