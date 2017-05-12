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

using Rhino.Geometry;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using Rhino.Display;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("55E14BEE-72F4-4d8c-B751-9BED20A7C5BC")]
        public partial class Pach_Mapping_Control
        {
            // This call is required by the Windows Form Designer. 
            public Pach_Mapping_Control()
            {
                InitializeComponent();
                this.End_Time_Control.Value = CO_TIME.Value;
                Create_Map(false);
                Update_Scale();
                Octave.SelectedIndex = 0;
                Instance = this;
                FC = new ForCall(Step_Forward);
                TC = new T_Call(Update_T);
            }

            ///<summary>Gets the only instance of the PachydermAcoustic plug-in.</summary>
            public static Pach_Mapping_Control Instance
            {
                get;
                private set;
            }

            Source[] Source;
            Brep[] Rec_Srfs;
            PachMapReceiver[] Map;
            WaveConduit WC;

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                PachydermAc_PlugIn plugin = PachydermAc_PlugIn.Instance;
                string SavePath = null;

                if (plugin.Save_Results())
                {
                    SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                    sf.DefaultExt = ".pachm";
                    sf.AddExtension = true;
                    sf.Filter = "Pachyderm Mapping Data File (*.pachm)|*.pachm|" + "All Files|";
                    if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SavePath = sf.FileName;
                    }
                }

                SourceList.Items.Clear();
                Map = null;
                System.Threading.Thread.Sleep(500);
                PachydermAc_PlugIn p = PachydermAc_PlugIn.Instance;
                Pach_RunSim_Command command = Pach_RunSim_Command.Instance;
                
                if (!p.Source(out Source) || Rec_Srfs == null)
                {
                    Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                    return;
                }

                Hare.Geometry.Point[] SPT;
                plugin.SourceOrigin(out SPT);
                Calculate.Enabled = false;
                List<Hare.Geometry.Point> P = new List<Hare.Geometry.Point>();
                P.AddRange(SPT);

                Polygon_Scene PScene = Utilities.RC_PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, false, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                if (!PScene.Complete)
                {
                    Calculate.Enabled = true;
                    return;
                }
                WC = new WaveConduit(c_scale, new double[] { Current_SPLMin, Current_SPLMax });

                PScene.partition(P);
                Scene Flex_Scene;
                if (PachydermAc_PlugIn.Instance.Geometry_Spec() == 0) 
                {
                    RhCommon_Scene NScene = Utilities.RC_PachTools.Get_NURBS_Scene((double)Rel_Humidity.Value, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                    if (!NScene.Complete) return;
                    NScene.partition(P, plugin.VG_Domain());
                    Flex_Scene = NScene;
                } 
                else
                {
                    Flex_Scene = PScene;    
                }

                Map = new PachMapReceiver[Source.Length];
                for (int i = 0; i < Source.Length; i++)
                {
                    Source[i].AppendPts(ref P);
                    Mesh Map_Mesh = Utilities.RC_PachTools.Create_Map_Mesh(Rec_Srfs, (double)Increment.Value * 0.01);
                    Map[i] = new PachMapReceiver(Utilities.RC_PachTools.Rhino_to_HareMesh(Map_Mesh), Source[i], 1000, (double)Increment.Value * 0.01, Flex_Scene, (int)RT_Count.Value, (double)CO_TIME.Value, Sum_Time.Checked, Disp_Audience.Checked, DirectionalToggle.Checked, Rec_Vertex.Checked, Offset_Mesh.Checked);
                }

                for (int s_id = 0; s_id < Source.Length; s_id++)
                {
                    command.Sim = new Direct_Sound(Source[s_id], Map[s_id], PScene, new int[8]{0,1,2,3,4,5,6,7});
                    Rhino.RhinoApp.RunScript("Run_Simulation", false);

                    Direct_Sound Direct_Data;
                    if (command.CommandResult != Rhino.Commands.Result.Cancel)
                    {
                        Direct_Data = ((Direct_Sound)command.Sim);
                    }
                    else
                    {
                        Array.Resize(ref Source, s_id);
                        Array.Resize(ref Map, s_id);
                        command.Reset();
                        Calculate.Enabled = true;
                        break;
                    }
                    command.Reset();

                    command.Sim = new SplitRayTracer(Source[s_id], Map[s_id], Flex_Scene, CutOffLength(), (int)RT_Count.Value, 0);

                    Rhino.RhinoApp.RunScript("Run_Simulation", false);

                    if (command.CommandResult == Rhino.Commands.Result.Success)
                    {
                        Map[s_id] = (PachMapReceiver)((SplitRayTracer)command.Sim).GetReceiver;
                    }
                    else
                    {
                        Array.Resize(ref Source, s_id);
                        Array.Resize(ref Map, s_id);
                        command.Reset();
                        Calculate.Enabled = true;
                        break;
                    }
                    command.Reset();
                    Map[s_id].AddDirect(Direct_Data, Source[s_id]);
                }

                if (Source != null)
                {
                    foreach (Source S in Source)
                    {
                        SourceList.Items.Add(String.Format("S{0}-", S.Source_ID()) + S.Type());
                    }

                    if (SavePath != null) Utilities.FileIO.Write_pachm(SavePath, Map);
                }

                if (Map != null)
                {
                    Create_Map(false);
                }
                else
                {
                    Receiver_Selection.Maximum = Map[0].Count;
                }

                Rhino.RhinoApp.WriteLine("Calculation has been completed. Have a nice day!");

                ///////
                //for(int i = 0;i < 10000; i++) Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(((Source[0] as LineSource).D as LineSource.ANCON).Pt[i]);
                ///////
                Calculate.Enabled = true;
            }

            private double CutOffLength()
            {
                return ((double)CO_TIME.Value / 1000) * AcousticalMath.SoundSpeed((double)Air_Temp.Value); ;
            }

            private void Select_Map_Click(object sender, EventArgs e)
            {
                //Get the receiver surfaces from the user 
                Rhino.DocObjects.ObjRef[] refs;
                Rhino.Input.RhinoGet.GetMultipleObjects("Select Mapping Surfaces", false, Rhino.DocObjects.ObjectType.Brep, out refs);
                List<Brep> B_Temp = new List<Brep>();
                
                foreach (Rhino.DocObjects.ObjRef o in refs)
                {
                    Rhino.DocObjects.ObjectType t = o.Geometry().ObjectType;

                    if (o.Geometry().ObjectType == Rhino.DocObjects.ObjectType.Brep || o.Geometry().ObjectType == Rhino.DocObjects.ObjectType.Extrusion || o.Geometry().ObjectType == Rhino.DocObjects.ObjectType.Surface)
                    {
                        B_Temp.Add(o.Brep());
                    }
                }

                Rec_Srfs = B_Temp.ToArray();
                
                if (Rec_Srfs.Length > 0)
                {
                    Select_Map.Text = "Select Mapping Surface: Complete";
                }
                else
                {
                    Select_Map.Text = "Select Mapping Surface";
                }
            }

            double Current_SPLMin = 0;
            double Current_SPLMax = 131;
            double Current_SPLAMin = 0;
            double Current_SPLAMax = 120;
            double Current_STI1Min = 0;
            double Current_STI1Max = 1;
            double Current_STI2Min = 0;
            double Current_STI2Max = 1;
            double Current_STI3Min = 0;
            double Current_STI3Max = 1;
            double Current_DMin = 0;
            double Current_DMax = 70;
            double Current_CMin = -5;
            double Current_CMax = 15;
            double Current_GMin = -5;
            double Current_GMax = 15;
            double Current_RTMin = 0;
            double Current_RTMax = 3;
            double Current_EDTMax = 3;
            double Current_EDTMin = 0;
            double Current_EMax = 50;
            double Current_EMin = 0;
            Pach_Graphics.colorscale c_scale;

            public void Update_Scale()
            {
                switch (Color_Selection.Text)
                {
                    case "R-O-Y-G-B-I-V":
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 0, this.Discretize.Checked, 24);
                        break;
                    case "R-O-Y":
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 1.0 / 3.0, 1, 0, 1, 0, this.Discretize.Checked, 12);
                        break;
                    case "Y-G-B":
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, Math.PI / 3.0, 2.0 / 3.0, 1, 0, 1, 0, this.Discretize.Checked, 12);
                        break;
                    case "R-M-B":
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 1, 0, 1, -1, this.Discretize.Checked, 12);
                        break;
                    case "W-B":
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 0, 0, 1, -1, this.Discretize.Checked, 12);
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Color selection invalid... Bright green substituted!");
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, Math.PI / 2.0, 0, 0, 1, 1, this.Discretize.Checked, 12);
                        break;
                }
                this.Param_Scale.Image = c_scale.PIC;
            }

            private List<int> SelectedSources()
            {
                List<int> indices = new List<int>();
                foreach (int i in SourceList.CheckedIndices) indices.Add(i);
                return indices;
            }

            public void Create_Map(bool PlotNumbers)
            {
                if (SelectedSources().Count == 0) return;
                Update_Scale();
                Mesh Mesh_Map;
                switch (Parameter_Selection.Text)
                {
                    case "Sound Pressure Level":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, PachTools.OctaveStr2Int(Octave.Text), SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Sound Pressure Level (A-weighted)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, SelectedSources(), Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLAMin, Current_SPLAMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Directionality":
                        if (Map != null)
                        {
                            PachMapReceiver.Get_Directional_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, PachTools.OctaveStr2Int(Octave.Text), SelectedSources());
                        }
                        break;
                    case "Early Decay Time (EDT)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_EDT_Map(Map, PachTools.OctaveStr2Int(Octave.Text), SelectedSources(), Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_EDTMin, Current_EDTMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Reverberation Time (T-15)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_RT_Map(Map, 15, PachTools.OctaveStr2Int(Octave.Text), SelectedSources(), Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_RTMin, Current_RTMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Reverberation Time (T-30)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_RT_Map(Map, 30, PachTools.OctaveStr2Int(Octave.Text), SelectedSources(), Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_RTMin, Current_RTMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Speech Transmission Index - 2003":
                        if (Map != null)
                        {
                            double[] Noise = new double[8];
                            string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                            if (n != "")
                            {
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }
                            else
                            {
                                Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                                n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }

                            double[] Values = PachMapReceiver.Get_STI_Map(Map, Noise, SelectedSources(), 0, Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_STI1Min, Current_STI1Max }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        } 
                        break;
                    case "Speech Transmission Index - Male":
                        if (Map != null)
                        {
                            double[] Noise = new double[8];
                            string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                            if (n != "")
                            {
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }
                            else
                            {
                                Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                                n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }

                            double[] Values = PachMapReceiver.Get_STI_Map(Map, Noise, SelectedSources(), 1, Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_STI2Min, Current_STI2Max }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Speech Transmission Index - Female":
                        if (Map != null)
                        {
                            double[] Noise = new double[8];
                            string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                            if (n != "")
                            {
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }
                            else
                            {
                                Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                                n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }

                            double[] Values = PachMapReceiver.Get_STI_Map(Map, Noise, SelectedSources(), 2, Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] {Current_STI3Min, Current_STI3Max }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Clarity (C-80)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Clarity_Map(Map, 80, PachTools.OctaveStr2Int(Octave.Text), SelectedSources(), Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_CMin, Current_CMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Definition (D-50)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Definition_Map(Map, PachTools.OctaveStr2Int(Octave.Text), SelectedSources(), Coherent.Checked);
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_DMin, Current_DMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Strength/Loudness (G)":
                        if (Map != null)
                        {
                            if (SelectedSources().Count > 1)
                            {
                                MessageBox.Show("G values", "Important: G values are only valid for single sources. Select a single source, and plot the G-values again.");
                                return;
                            }
                            int SrcID = SelectedSources()[0];
                            double[] Values = PachMapReceiver.Get_G_Map(Map, PachTools.OctaveStr2Int(Octave.Text), Source[SrcID].SWL(PachTools.OctaveStr2Int(Octave.Text)), SrcID, Coherent.Checked);//, G_Ref_Energy[PachTools.OctaveStr2Int(Octave.Text)]
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_GMin, Current_GMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Percent who perceive echoes (EK)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_EchoCritPercent_Map(Map, PachTools.OctaveStr2Int(Octave.Text), SelectedSources());
                            System.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_EMin, Current_EMax }, c_scale);
                            Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RC_PachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Parameter selection invalid...");
                        break;
                }
            }

            private void Commit_Param_Bounds()
            {
                switch (Parameter_Selection.Text)
                {
                    case "Sound Pressure Level":
                        Current_SPLMin = (double)this.Param_Min.Value;
                        Current_SPLMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_SPLMax - Current_SPLMin) * .25) + Current_SPLMin).ToString();
                        this.Param1_2.Text = (((Current_SPLMax - Current_SPLMin) * .5) + Current_SPLMin).ToString();
                        this.Param3_4.Text = (((Current_SPLMax - Current_SPLMin) * .75) + Current_SPLMin).ToString();
                        break;
                    case "Sound Pressure Level (A-weighted)":
                        Current_SPLAMin = (double)this.Param_Min.Value;
                        Current_SPLAMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_SPLAMax - Current_SPLAMin) * .25) + Current_SPLAMin).ToString();
                        this.Param1_2.Text = (((Current_SPLAMax - Current_SPLAMin) * .5) + Current_SPLAMin).ToString();
                        this.Param3_4.Text = (((Current_SPLAMax - Current_SPLAMin) * .75) + Current_SPLAMin).ToString();
                        break;
                    case "Early Decay Time (T-15)":
                        Current_EDTMin = (double)this.Param_Min.Value;
                        Current_EDTMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_EDTMax - Current_EDTMin) * .25) + Current_EDTMin).ToString();
                        this.Param1_2.Text = (((Current_EDTMax - Current_EDTMin) * .5) + Current_EDTMin).ToString();
                        this.Param3_4.Text = (((Current_EDTMax - Current_EDTMin) * .75) + Current_EDTMin).ToString();
                        break;
                    case "Reverberation Time (T-15)":
                        Current_RTMin = (double)this.Param_Min.Value;
                        Current_RTMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_RTMax - Current_RTMin) * .25) + Current_RTMin).ToString();
                        this.Param1_2.Text = (((Current_RTMax - Current_RTMin) * .5) + Current_RTMin).ToString();
                        this.Param3_4.Text = (((Current_RTMax - Current_RTMin) * .75) + Current_RTMin).ToString();
                        break;
                    case "Reverberation Time (T-30)":
                        Current_RTMin = (double)this.Param_Min.Value;
                        Current_RTMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_RTMax - Current_RTMin) * .25) + Current_RTMin).ToString();
                        this.Param1_2.Text = (((Current_RTMax - Current_RTMin) * .5) + Current_RTMin).ToString();
                        this.Param3_4.Text = (((Current_RTMax - Current_RTMin) * .75) + Current_RTMin).ToString();
                        break;
                    case "Speech Transmission Index - 2003":
                        Current_STI1Min = (double)this.Param_Min.Value;
                        Current_STI1Max = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_STI1Max - Current_STI1Min) * .25) + Current_STI1Min).ToString();
                        this.Param1_2.Text = (((Current_STI1Max - Current_STI1Min) * .5) + Current_STI1Min).ToString();
                        this.Param3_4.Text = (((Current_STI1Max - Current_STI1Min) * .75) + Current_STI1Min).ToString();
                        break;
                    case "Speech Transmission Index - Male":
                        Current_STI2Min = (double)this.Param_Min.Value;
                        Current_STI2Max = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_STI2Max - Current_STI2Min) * .25) + Current_STI2Min).ToString();
                        this.Param1_2.Text = (((Current_STI2Max - Current_STI2Min) * .5) + Current_STI2Min).ToString();
                        this.Param3_4.Text = (((Current_STI2Max - Current_STI2Min) * .75) + Current_STI2Min).ToString();
                        break;
                    case "Speech Transmission Index - Female":
                        Current_STI3Min = (double)this.Param_Min.Value;
                        Current_STI3Max = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_STI3Max - Current_STI3Min) * .25) + Current_STI3Min).ToString();
                        this.Param1_2.Text = (((Current_STI3Max - Current_STI3Min) * .5) + Current_STI3Min).ToString();
                        this.Param3_4.Text = (((Current_STI3Max - Current_STI3Min) * .75) + Current_STI3Min).ToString();
                        break;
                    case "Clarity (C-80)":
                        Current_CMin = (double)this.Param_Min.Value;
                        Current_CMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_CMax - Current_CMin) * .25) + Current_CMin).ToString();
                        this.Param1_2.Text = (((Current_CMax - Current_CMin) * .5) + Current_CMin).ToString();
                        this.Param3_4.Text = (((Current_CMax - Current_CMin) * .75) + Current_CMin).ToString();
                        break;
                    case "Definition (D-50)":
                        Current_DMin = (double)this.Param_Min.Value;
                        Current_DMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_DMax - Current_DMin) * .25) + Current_DMin).ToString();
                        this.Param1_2.Text = (((Current_DMax - Current_DMin) * .5) + Current_DMin).ToString();
                        this.Param3_4.Text = (((Current_DMax - Current_DMin) * .75) + Current_DMin).ToString();
                        break;
                    case "Strength/Loudness (G)":
                        Current_GMin = (double)this.Param_Min.Value;
                        Current_GMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_GMax - Current_GMin) * .25) + Current_GMin).ToString();
                        this.Param1_2.Text = (((Current_GMax - Current_GMin) * .5) + Current_GMin).ToString();
                        this.Param3_4.Text = (((Current_GMax - Current_GMin) * .75) + Current_GMin).ToString();
                        break;
                    case "Percent who perceive echoes (EK)":
                        Current_EMin = (double)this.Param_Min.Value;
                        Current_EMax = (double)this.Param_Max.Value;
                        this.Param1_4.Text = (((Current_EMax - Current_EMin) * .25) + Current_GMin).ToString();
                        this.Param1_2.Text = (((Current_EMax - Current_EMin) * .5) + Current_GMin).ToString();
                        this.Param3_4.Text = (((Current_EMax - Current_EMin) * .75) + Current_GMin).ToString();
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Parameter selection invalid...");
                        break;
                }

                if(WC != null) WC.SetColorScale(c_scale, new double[2]{(double)Param_Min.Value, (double)Param_Max.Value});
            }

            private void Calculate_Map_Click(object sender, EventArgs e)
            {
                Create_Map(false);
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Plot_Values_Click(object sender, EventArgs e)
            {
                Create_Map(true);
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

            }

            private void SaveDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (Map != null)
                {
                    Utilities.FileIO.Write_pachm(Map);
                }
            }

            private void OpenDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                PachMapReceiver[] RT_IN = new PachMapReceiver[0];
                if (!Utilities.FileIO.Read_pachm(ref RT_IN)) return;
                //foreach (PachMapReceiver p in RT_IN) p.Correction(80000);

                Map = RT_IN;
                SourceList.Items.Clear();
                for (int i = 0; i < Map.Length; i++)
                {
                    SourceList.Items.Add(String.Format("S{0}-", i) + Map[i].SrcType);
                }

                WC = new WaveConduit(c_scale, new double[] { Current_SPLMin, Current_SPLMax });
            }

            private void Parameter_Selection_SelectedIndexChanged(object sender, EventArgs e)
            {
                Commit_Parameter();
            }

            private void Commit_Parameter()
            {
                switch (Parameter_Selection.Text)
                {
                    case "Sound Pressure Level":
                        this.Param_Min.Value = (decimal)Current_SPLMin;
                        this.Param_Max.Value = (decimal)Current_SPLMax;
                        this.Param1_4.Text = (((Current_SPLMax - Current_SPLMin) * .25) + Current_SPLMin).ToString();
                        this.Param1_2.Text = (((Current_SPLMax - Current_SPLMin) * .5) + Current_SPLMin).ToString();
                        this.Param3_4.Text = (((Current_SPLMax - Current_SPLMin) * .75) + Current_SPLMin).ToString();
                        Start_Time_Control.Enabled = true;
                        End_Time_Control.Enabled = true;
                        break;
                    case "Sound Pressure Level (A-weighted)":
                        this.Param_Min.Value = (decimal)Current_SPLAMin;
                        this.Param_Max.Value = (decimal)Current_SPLAMax;
                        this.Param1_4.Text = (((Current_SPLAMax - Current_SPLAMin) * .25) + Current_SPLAMin).ToString();
                        this.Param1_2.Text = (((Current_SPLAMax - Current_SPLAMin) * .5) + Current_SPLAMin).ToString();
                        this.Param3_4.Text = (((Current_SPLAMax - Current_SPLAMin) * .75) + Current_SPLAMin).ToString();
                        Start_Time_Control.Enabled = true;
                        End_Time_Control.Enabled = true;
                        break;
                    case "Early Decay Time (T-15)":
                        this.Param_Min.Value = (decimal)Current_EDTMin;
                        this.Param_Max.Value = (decimal)Current_EDTMax;
                        this.Param1_4.Text = (((Current_EDTMax - Current_EDTMin) * .25) + Current_EDTMin).ToString();
                        this.Param1_2.Text = (((Current_EDTMax - Current_EDTMin) * .5) + Current_EDTMin).ToString();
                        this.Param3_4.Text = (((Current_EDTMax - Current_EDTMin) * .75) + Current_EDTMin).ToString();
                        Start_Time_Control.Enabled = false;
                        End_Time_Control.Enabled = false;
                        break;
                    case "Reverberation Time (T-15)":
                        this.Param_Min.Value = (decimal)Current_RTMin;
                        this.Param_Max.Value = (decimal)Current_RTMax;
                        this.Param1_4.Text = (((Current_RTMax - Current_RTMin) * .25) + Current_RTMin).ToString();
                        this.Param1_2.Text = (((Current_RTMax - Current_RTMin) * .5) + Current_RTMin).ToString();
                        this.Param3_4.Text = (((Current_RTMax - Current_RTMin) * .75) + Current_RTMin).ToString();
                        Start_Time_Control.Enabled = false;
                        End_Time_Control.Enabled = false;
                        break;
                    case "Reverberation Time (T-30)":
                        this.Param_Min.Value = (decimal)Current_RTMin;
                        this.Param_Max.Value = (decimal)Current_RTMax;
                        this.Param1_4.Text = (((Current_RTMax - Current_RTMin) * .25) + Current_RTMin).ToString();
                        this.Param1_2.Text = (((Current_RTMax - Current_RTMin) * .5) + Current_RTMin).ToString();
                        this.Param3_4.Text = (((Current_RTMax - Current_RTMin) * .75) + Current_RTMin).ToString();
                        Start_Time_Control.Enabled = false;
                        End_Time_Control.Enabled = false;
                        break;
                    case "Clarity (C-80)":
                        this.Param_Min.Value = (decimal)Current_CMin;
                        this.Param_Max.Value = (decimal)Current_CMax;
                        this.Param1_4.Text = (((Current_CMax - Current_CMin) * .25) + Current_CMin).ToString();
                        this.Param1_2.Text = (((Current_CMax - Current_CMin) * .5) + Current_CMin).ToString();
                        this.Param3_4.Text = (((Current_CMax - Current_CMin) * .75) + Current_CMin).ToString();
                        Start_Time_Control.Enabled = false;
                        End_Time_Control.Enabled = false;
                        break;
                    case "Definition (D-50)":
                        this.Param_Min.Value = (decimal)Current_DMin;
                        this.Param_Max.Value = (decimal)Current_DMax;
                        this.Param1_4.Text = (((Current_DMax - Current_DMin) * .25) + Current_DMin).ToString();
                        this.Param1_2.Text = (((Current_DMax - Current_DMin) * .5) + Current_DMin).ToString();
                        this.Param3_4.Text = (((Current_DMax - Current_DMin) * .75) + Current_DMin).ToString();
                        Start_Time_Control.Enabled = false;
                        End_Time_Control.Enabled = false;
                        break;
                    case "Strength/Loudness (G)":
                        this.Param_Min.Value = (decimal)Current_GMin;
                        this.Param_Max.Value = (decimal)Current_GMax;
                        this.Param1_4.Text = (((Current_GMax - Current_GMin) * .25) + Current_GMin).ToString();
                        this.Param1_2.Text = (((Current_GMax - Current_GMin) * .5) + Current_GMin).ToString();
                        this.Param3_4.Text = (((Current_GMax - Current_GMin) * .75) + Current_GMin).ToString();
                        Start_Time_Control.Enabled = false;
                        End_Time_Control.Enabled = false;
                        break;
                    case "Percent who perceive echoes (EK)":
                        this.Param_Min.Value = (decimal)Current_EMin;
                        this.Param_Max.Value = (decimal)Current_EMax;
                        this.Param1_4.Text = (((Current_EMax - Current_EMin) * .25) + Current_GMin).ToString();
                        this.Param1_2.Text = (((Current_EMax - Current_EMin) * .5) + Current_GMin).ToString();
                        this.Param3_4.Text = (((Current_EMax - Current_EMin) * .75) + Current_GMin).ToString();
                        Start_Time_Control.Enabled = false;
                        End_Time_Control.Enabled = false;
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Parameter selection invalid...");
                        break;
                }
            }

            private void Update_Graph(object sender, EventArgs e)
            {
                Analysis_View.GraphPane.CurveList.Clear();

                int REC_ID = (int)Receiver_Selection.Value;
                try
                {
                    int SampleRate = Map[0].SampleRate;
                    //if (Receiver_Choice.Text == "No Results Calculated...") return;
                    //REC_ID = int.Parse(Receiver_Choice.Text);

                    int OCT_ID = PachTools.OctaveStr2Int(Graph_Octave.Text);
                    Analysis_View.GraphPane.Title.Text = "Logarithmic Energy Time Curve";
                    Analysis_View.GraphPane.XAxis.Title.Text = "Time (seconds)";
                    Analysis_View.GraphPane.YAxis.Title.Text = "Sound Pressure Level (dB)";

                    List<int> SrcIDs = new List<int>();
                    foreach (int i in SourceList.CheckedIndices) SrcIDs.Add(i);

                    double[] Filter;
                    double[] Schroeder;
                    double[] Filter2;
                    int zero_sample = 0;
                    switch (Graph_Type.Text)
                    {
                        case "Energy Time Curve":
                            Filter = AcousticalMath.ETCurve(null, null, Map, (double)CO_TIME.Value, SampleRate, PachTools.OctaveStr2Int(Graph_Octave.Text), REC_ID, SrcIDs, false);
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        //case "Pressure Time Curve":
                        //    zero_sample = 4096 / 2;
                        //    Filter2 = AcousticalMath.PTCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false);
                        //    if (PachTools.OctaveStr2Int(Graph_Octave.Text) < 8)
                        //    {
                        //        Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, PachTools.OctaveStr2Int(Graph_Octave.Text), SampleRate, 0);
                        //    }
                        //    Filter = new double[Filter2.Length];
                        //    for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                        //    Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                        //    break;
                        //case "Lateral ETC":
                        //    Filter = AcousticalMath.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, PachTools.OctaveStr2Int(Graph_Octave.Text), REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1];
                        //    for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                        //    Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                        //    break;
                        //case "Lateral PTC":
                        //    zero_sample = 4096 / 2;
                        //    Filter2 = AcousticalMath.PTCurve_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1];
                        //    if (PachTools.OctaveStr2Int(Graph_Octave.Text) < 8)
                        //    {
                        //        Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, PachTools.OctaveStr2Int(Graph_Octave.Text), SampleRate, 0);
                        //    }
                        //    Filter = new double[Filter2.Length];
                        //    for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                        //    Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                        //    break;
                        //case "Vertical ETC":
                        //    Filter = AcousticalMath.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, PachTools.OctaveStr2Int(Graph_Octave.Text), REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[2];
                        //    for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                        //    Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                        //    break;
                        //case "Vertical PTC":
                        //    zero_sample = 4096 / 2;
                        //    Filter2 = AcousticalMath.PTCurve_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[2];
                        //    if (PachTools.OctaveStr2Int(Graph_Octave.Text) < 8)
                        //    {
                        //        Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, PachTools.OctaveStr2Int(Graph_Octave.Text), SampleRate, 0);
                        //    }
                        //    Filter = new double[Filter2.Length];
                        //    for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                        //    Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                        //    break;
                        //case "Fore-Aft ETC":
                        //    Filter = AcousticalMath.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, PachTools.OctaveStr2Int(Graph_Octave.Text), REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[0];
                        //    for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                        //    Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                        //    break;
                        //case "Fore-Aft PTC":
                        //    zero_sample = 4096 / 2;
                        //    Filter2 = AcousticalMath.PTCurve_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[0];
                        //    if (PachTools.OctaveStr2Int(Graph_Octave.Text) < 8)
                        //    {
                        //        Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, PachTools.OctaveStr2Int(Graph_Octave.Text), SampleRate, 0);
                        //    }
                        //    Filter = new double[Filter2.Length];
                        //    for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                        //    Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                        //    break;
                        default:
                            throw new NotImplementedException();
                    }
                    //Get the maximum value of the Direct Sound

                    Filter = AcousticalMath.SPL_Intensity_Signal(Filter);
                    Schroeder = AcousticalMath.SPL_Intensity_Signal(Schroeder);

                    double DirectMagnitude = 0;
                    for (int j = 0; j < Filter.Length; j++)
                    {
                        if (Filter[j] > DirectMagnitude) DirectMagnitude = Filter[j];
                    }
                    
                    if (Normalize_Graph.Checked)
                    {
                        Filter = Utilities.AcousticalMath.Normalize_Function(Filter);
                        Schroeder = Utilities.AcousticalMath.Normalize_Function(Schroeder);
                    }

                    double[] time = new double[Filter.Length];
                    for (int i = 0; i < Filter.Length; i++)
                    {
                        time[i] = (double)(i - zero_sample) / SampleRate;
                    }

                    Analysis_View.GraphPane.AddCurve("Schroeder Integral", time, Schroeder, System.Drawing.Color.Red, ZedGraph.SymbolType.None);
                    Analysis_View.GraphPane.AddCurve("Logarithmic Energy Time Curve", time, Filter, System.Drawing.Color.Blue, ZedGraph.SymbolType.None);

                    if (!LockUserScale.Checked)
                    {
                        Analysis_View.GraphPane.XAxis.Scale.Max = time[time.Length - 1];
                        Analysis_View.GraphPane.XAxis.Scale.Min = time[0];

                        if (Normalize_Graph.Checked)
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = 0;
                            Analysis_View.GraphPane.YAxis.Scale.Min = -100;
                        }
                        else
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = DirectMagnitude + 15;
                            Analysis_View.GraphPane.YAxis.Scale.Min = 0;
                        }
                    }
                    else
                    {
                        double max = Analysis_View.GraphPane.YAxis.Scale.Max;
                        double min = Analysis_View.GraphPane.YAxis.Scale.Min;

                        if (Normalize_Graph.Checked)
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = max;
                            Analysis_View.GraphPane.YAxis.Scale.Min = min;
                        }
                        else
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = max;
                            Analysis_View.GraphPane.YAxis.Scale.Min = min;
                        }
                    }

                    //Hare.Geometry.Vector V = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(float)Alt_Choice.Value, true), -(float)Azi_Choice.Value, 0, true);

                    //if (Receiver_Choic.SelectedIndex > 0) ReceiverConduit.Instance.set_direction(Utilities.RC_PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), new Vector3d(V.x, V.y, V.z));
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                }
                catch (Exception x)
                {
                    System.Windows.Forms.MessageBox.Show(x.Message);
                    return;
                }

                Analysis_View.AxisChange();
                Analysis_View.Invalidate();
                //Update_Parameters();
            }


            private void Param_MouseUp(object sender, MouseEventArgs e)
            {
                Commit_Param_Bounds();
            }

            private void Param_Leave(object sender, EventArgs e)
            {
                Commit_Param_Bounds();
            }

            private void Color_Selection_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Scale();
            }

            System.Threading.Thread T;
            private delegate void ForCall();
            ForCall FC;
            private delegate void T_Call();
            T_Call TC;

            private void Flip_Toggle_Click(object sender, EventArgs e)
            {
                Update_Scale();
                if (Flip_Toggle.Text == "Flip")
                {
                    this.Flip_Toggle.Text = "Pause";
                    FC = new ForCall(Step_Forward);
                    TC = new T_Call(Update_T);
                    System.Threading.ParameterizedThreadStart St = new System.Threading.ParameterizedThreadStart(delegate { Flip_Forward(); });
                    T = new System.Threading.Thread(St);
                    T.Start();
                }
                else 
                {
                    Flip_Toggle.Text = "Flip";
                    T.Abort();
                }
            }

            int t = 0;
            double t_lo;
            double t_hi;

            private void Update_T()
            {
                Min_Time_out.Text = t_lo.ToString();
                Max_Time_out.Text = t_hi.ToString();
            }

            int oct;

            private void Step_Forward()
            {
                t_lo = t * (double)Step_Select.Value;
                t_hi = t * (double)Step_Select.Value + (double)Integration_select.Value;
                if (t_hi > Map[0].CutOffTime)
                {
                    t_lo = t = 0;
                    t_hi = t + (double)Integration_select.Value;
                }

                this.Invoke((MethodInvoker)delegate { oct = PachTools.OctaveStr2Int(Octave.Text); });
                this.Invoke((MethodInvoker)delegate { Update_T(); });

                double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { t_hi, t_lo }, oct, SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked);
                System.Drawing.Color[] c = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, c_scale);
                Mesh Mesh_Map = Utilities.RC_PachTools.PlotMesh(Map, c);
                if (WC == null) return;
                WC.Populate(Mesh_Map);
                
                //////////////////////////////
                if (Folder_Status.Text != "")
                {
                    string number;
                    if (t < 100)
                    {
                        if (t < 10) number = "00" + t.ToString();
                        else number = "0" + t.ToString();
                    }
                    else number = t.ToString();

                    this.Invoke((MethodInvoker)delegate { Rhino.RhinoApp.RunScript("-ViewCaptureToFile " + Folder_Status.Text + "\\"[0] + "frame" + number + ".jpg Width=1280 Height=720 DrawGrid=No Enter", true); });
                }
                //////////////////////////////
                t++;

                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Flip_Forward()
            {
                do
                {
                    FC();
                    TC();
                    System.Threading.Thread.Sleep(100);
                }
                while (true);
            }

            private void Step_Backward()
            {
                t--;
                if (t < 0) t = 0;
                t_lo = t * (double)Step_Select.Value;
                t_hi = t * (double)Step_Select.Value + (double)Increment.Value;
                Min_Time_out.Text = t_lo.ToString();
                Max_Time_out.Text = t_hi.ToString();
                double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { t_hi, t_lo }, PachTools.OctaveStr2Int(Octave.Text), SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked);
                System.Drawing.Color[] c = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, c_scale);
                Mesh Map_Mesh = Utilities.RC_PachTools.PlotMesh(Map, c);
                WC.Populate(Map_Mesh);
                TC();
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Back_Step_Click(object sender, EventArgs e)
            {
                Update_Scale();
                Step_Backward();
            }

            private void Forw_Step_Click(object sender, EventArgs e)
            {
                Update_Scale();
                Step_Forward();
            }

            private void Start_Over_Click(object sender, EventArgs e)
            {
                Update_Scale();
                t = 0;
                double t_lo = t * (double)Step_Select.Value;
                double t_hi = t * (double)Step_Select.Value + (double)Increment.Value;
                Min_Time_out.Text = t_lo.ToString();
                Max_Time_out.Text = t_hi.ToString();
                Step_Forward();
            }

            public bool Auralisation_Ready()
            {
                if (Map != null) return true;
                return false;
            }

            public void GetSims(ref PachMapReceiver[] Maps)
            {   
                if (Map != null) Maps = this.Map;
            }

            private void Sum_Time_CheckedChanged(object sender, EventArgs e)
            {
                if((sender as CheckBox).Checked) { DirectionalToggle.Enabled = false; }
                else { DirectionalToggle.Enabled = true; }
            }

            bool Linear_Phase = false;

            public void Set_Phase_Regime(bool Linear_Phase)
            {
                if (Map == null || Map[0] == null) return;
                if (Linear_Phase == this.Linear_Phase) return;
                if ((this.Linear_Phase == true && !(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System)) || (this.Linear_Phase == false && !(Audio.Pach_SP.Filter is Audio.Pach_SP.Minimum_Phase_System)))
                {
                    for (int i = 0; i < Map.Length; i++) Map[i].reset_pressure();
                    Rhino.RhinoApp.Write("Mapping pressure reset.");
                    this.Linear_Phase = Linear_Phase;
                }
            }

            private void Coherent_CheckedChanged(object sender, EventArgs e)
            {
                if (Coherent.Checked && Map != null && !Map[0].HasPressure())
                {
                    DialogResult DR = MessageBox.Show("Pachyderm will now create the pressure impulse responses for your previously calculated intensity responses. This can take awhile, though, depending on how many receivers you have, and how long a cutoff time you chose. Are you sure you would like to do this?", "Pressure Impulse Response Creation", MessageBoxButtons.YesNo);
                    if (DR == DialogResult.Yes)
                    {
                        Linear_Phase = Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System;
                        for (int i = 0; i < this.Map.Length; i++) this.Map[i].Create_Pressure();
                    }
                    else 
                    {
                        Coherent.Checked = false;
                        Incoherent.Checked = true;
                    }
                }
            }

            private FolderBrowserDialog FileLocation = new FolderBrowserDialog();
            private void OpenFolder_Click(object sender, System.EventArgs e)
            {
                if (FileLocation.ShowDialog() == DialogResult.OK)
                {
                    Folder_Status.Text = FileLocation.SelectedPath;
                }
            }


            public class Map_indicator : Rhino.Display.DisplayConduit
            {
                Rhino.Geometry.Point3d PT;

                public void setPoint(Rhino.Geometry.Point3d pt)
                {
                    PT = pt;
                }

                protected override void DrawForeground(DrawEventArgs e)
                {
                    if (PT == null) return;
                    Rhino.Geometry.Point2d screen_pt = e.Display.Viewport.WorldToClient(PT);                    
                    e.Display.Draw2dRectangle(new System.Drawing.Rectangle((int)screen_pt.X, (int)screen_pt.Y, 5, 5), System.Drawing.Color.Green, 2, System.Drawing.Color.Yellow);
                    return;
                }
            }

            Map_indicator ReceiverPointer = new Map_indicator();

            private void ReceiverSelection_ValueChanged(object sender, EventArgs e)
            {
                if (Map == null || Map.Length < 1)
                {
                    Receiver_Selection.Value = 0;
                    Receiver_Selection.Maximum = 0;
                    ReceiverPointer.Enabled = false;
                    return;
                }

                Receiver_Selection.Maximum = Map[0].Rec_List.Length;

                if (SourceList.CheckedIndices.Count < 1) return;

                ReceiverPointer.Enabled = true;
                ReceiverPointer.setPoint(Utilities.RC_PachTools.HPttoRPt(Map[(int)(SourceList.CheckedIndices[0])].Origin((int)Receiver_Selection.Value)));
                Update_Graph(sender, e);
            }
        }
    }
}