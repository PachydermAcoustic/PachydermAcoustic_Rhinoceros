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

using Rhino.PlugIns;
using Rhino.Geometry;
using System.Collections.Generic;
using System;

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
            public Pach_Properties Pach_Props;

            public PachydermAc_PlugIn()
            {
                new SourceConduit();
                new ReceiverConduit();
                new CellConduit();
                Pach_Props = Pach_Properties.Instance;
                Audio.Pach_SP.Initialize_FFTW();
                System.AppDomain.CurrentDomain.AssemblyResolve += GetAssemblies;
                Instance = this;
            }

            public System.Reflection.Assembly GetAssemblies(object source, ResolveEventArgs e)
            {
                //string PachPath;

                //PachPath = this.GetPluginPath();
                //if (PachPath == null || PachPath == "")
                //{
                //    if (Rhino.PlugIns.PlugIn.LoadPlugIn(new Guid("25895777-97d3-4058-8753-503183d4bc01")))
                //    {
                //        PachPath = Pachyderm_Acoustic.UI.PachydermAc_PlugIn.Instance.GetPluginPath();
                //    }
                //}

                //PachPath = PachPath.Remove(PachPath.Length - 22);

                //switch (e.Name)
                //{
                //    case "Hare":
                //        return System.Reflection.Assembly.LoadFile(PachPath + "Hare.dll");
                //    case "CLF_Read":
                //        return System.Reflection.Assembly.LoadFile(PachPath + "CLF_Read.dll");
                //    case "MathNet.Numerics":
                //        return System.Reflection.Assembly.LoadFile(PachPath + "MathNet.Numerics.dll");
                //    case "NAudio":
                //        return System.Reflection.Assembly.LoadFile(PachPath + "NAudio.dll");
                //    case "Pachyderm_Acoustic_Universal":
                //        return System.Reflection.Assembly.LoadFile(PachPath + "Pachyderm_Acoustic_Universal.dll");
                //    case "ZedGraph":
                //        return System.Reflection.Assembly.LoadFile(PachPath + "ZedGraph.dll");
                //}

                //if (e.Name == "OxyPlot.Wpf")
                //    return AppDomain.CurrentDomain.Load(e.Name);
                //else return null;
                return null;
            }

            public Guid InstanceID
            {
                get
                {
                    return Instance_ID;
                }
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
                Pach_Splash splash = new Pach_Splash();
                splash.Show();
                splash.Refresh();
                System.Threading.Thread.Sleep(2000);

                //TODO: find out if we can now create arrays greater than 2 GB...
                //Register the UserControl "Panels"
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_TD_Numeric_Control), "PachyDerm TimeDomain Models", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_Hybrid_Control), "Pachyderm Hybrid Models", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_Mapping_Control), "Pachyderm Mapping Method", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_MapCustom), "Pachyderm Custom Maps", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_Visual_Control), "Pachyderm Particle Animation", Properties.Resources.PIcon1);
                Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_Auralisation), "Pachyderm Auralisation", Properties.Resources.PIcon1);
                //Rhino.UI.Panels.RegisterPanel(this, typeof(UI.Pach_SpeakerBuilder), "Pachyderm Speaker Builder", Properties.Resources.PIcon1);

                splash.Close();
                splash.Dispose();

                return LoadReturnCode.Success;
            }

            public UI.Pach_Props_Page m_Doc_Page = new UI.Pach_Props_Page();
            public UI.Pach_Materials_Page m_Obj_Page = new UI.Pach_Materials_Page();
            public UI.Pach_SourceControl_Page m_Source_Page = new UI.Pach_SourceControl_Page();

            protected override void ObjectPropertiesPages(List<Rhino.UI.ObjectPropertiesPage> pages)
            {
                pages.Add(m_Obj_Page);
                pages.Add(m_Source_Page);
            }

            protected override void OptionsDialogPages(List<Rhino.UI.OptionsDialogPage> pages)
            {
                pages.Add(m_Doc_Page);
            }

            public int ProcessorSpec()
            {
                return m_Doc_Page.Get_Processor_Spec();
            }

            public int Geometry_Spec()
            {
                return m_Doc_Page.Get_Geometry_Spec();
            }

            public int SP_Spec()
            {
                return m_Doc_Page.Get_SP_Spec();
            }

            public int Oct_Depth()
            {
                return m_Doc_Page.Get_Oct_Depth();
            }

            public int VG_Domain()
            {
                return m_Doc_Page.Get_VG_Domain();
            }

            public string ML_Path()
            {
                return m_Doc_Page.Get_MatLib_Path();
            }

            public bool Save_Results()
            {
                return m_Doc_Page.Save_Results();
            }

            public string SpecialCode = "";

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
                    Points[i] = Utilities.RC_PachTools.RPttoHPt(Rhino.RhinoDoc.ActiveDoc.Objects.Find(S_ID).Geometry.GetBoundingBox(true).Min);
                }

                if (Points.Length > 0) return true;
                return false;
            }

            public string GetPluginPath()
            {
                return System.Reflection.Assembly.GetExecutingAssembly().Location;
            }

            public bool Source(out Environment.Source[] S)
            {
                S = new Environment.Source[0];
                System.Guid[] S_ID = SourceConduit.Instance.UUID.ToArray();
                S = new Environment.Source[S_ID.Length];
                for (int id = 0; id < S_ID.Length; id++)
                {
                    if (S_ID[id] == System.Guid.Empty || S_ID[id] == System.Guid.NewGuid()) break;
                    Rhino.DocObjects.RhinoObject Origin = Rhino.RhinoDoc.ActiveDoc.Objects.Find(S_ID[id]);

                    if (Origin.ObjectType == Rhino.DocObjects.ObjectType.Point)
                    {
                        string S_Type = Origin.Geometry.GetUserString("SourceType");
                        string SWL = Origin.Geometry.GetUserString("SWL");
                        string D = Origin.Geometry.GetUserString("Delay");
                        double delay; if (D != ""&& D != null) delay = double.Parse(D)/1000; else delay = 0;

                        string Ph = Origin.Geometry.GetUserString("Phase");
                        //double[] phase = new double[8];
                        //if (Ph != "")
                        //{
                        //    string[] phstr = Ph.Split(";"[0]);
                        //    for (int o = 0; o < 8; o++) phase[o] = double.Parse(phstr[o]);
                        //}

                        double[] SWL_Values = Utilities.PachTools.DecodeSourcePower(SWL);
                        switch (S_Type)
                        {
                            case "":
                            case "0":
                                S[id] = new Environment.GeodesicSource(SWL_Values, Utilities.RC_PachTools.RPttoHPt(Origin.Geometry.GetBoundingBox(true).Min), id);
                                break;
                            case "1":
                                S[id] = new Environment.RandomSource(SWL_Values, Utilities.RC_PachTools.RPttoHPt(Origin.Geometry.GetBoundingBox(true).Min), id);
                                break;
                            case "2":
                            case "3":
                                string Bands = Origin.Geometry.GetUserString("Bands");
                                string[] B;
                                if (Bands != "")
                                    B = Bands.Split(';');
                                else
                                    B = new string[2] { "0", "7" };
                                SourceConduit SC = SourceConduit.Instance;
                                S[id] = new Environment.DirectionalSource(SC.m_Balloons[id], SWL_Values, Utilities.RC_PachTools.RPttoHPt(Origin.Geometry.GetBoundingBox(true).Min), new int[] { int.Parse(B[0]), int.Parse(B[1]) }, id);
                                break;
                        }
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
                        
                        //string Ph = Origin.Geometry.GetUserString("Phase");
                        //double[] phase = new double[8];
                        //if (Ph != "")
                        //{
                        //    string[] phstr = Ph.Split(";"[0]);
                        //    for (int o = 0; o < 8; o++) phase[o] = double.Parse(phstr[o]);
                        //}

                        Rhino.Geometry.Point3d[] pts = (Origin.Geometry as Curve).DivideEquidistant(1d / 4d);
                        if (pts == null || pts.Length == 0) pts = new Point3d[1] { (Origin.Geometry as Curve).PointAtNormalizedLength(0.5) };
                        Hare.Geometry.Point[] Samples = new Hare.Geometry.Point[pts.Length];

                        for (int i = 0; i < pts.Length; i++)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(pts[i]);
                            Samples[i] = Utilities.RC_PachTools.RPttoHPt(pts[i]);
                        }
                        S[id] = new Environment.LineSource(Samples, (Origin.Geometry as Curve).GetLength(), SWL, 4, id);
                    }
                }
                if (S.Length > 0) return true;
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
                    Point.Add(Utilities.RC_PachTools.RPttoHPt(Rhino.RhinoDoc.ActiveDoc.Objects.Find(R_ID).Geometry.GetBoundingBox(true).Min));
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
                    }catch(Exception)
                    {
                        break;
                    }
                }
                while (objectId != null);
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