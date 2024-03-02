//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2024, Arthur van der Harten 
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
using System.Linq;
using System.Collections.Generic;
using System;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Audio;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using Eto.Forms;
using Eto.Drawing;
using Rhino.UI;
using System.CodeDom;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("12db68c3-c995-43c6-860a-6bd106b94a4c")]
        public class Pach_Auralisation: Panel, IPanel
        {
             internal TabPage Render_Settings;
            internal DropDown Source_Aim;
            internal NumericStepper Alt_Choice;
            internal NumericStepper Azi_Choice;
            internal Button RenderBtn;
            private ScottPlot.Eto.EtoPlot Analysis_View;
            internal NumericStepper DryChannel;
            internal TextBox Signal_Status;
            internal Button OpenSignal;
            internal DropDown Receiver_Choice;
            internal TabControl Tabs;
            private TabPage Data_Source;
            internal SourceListBox SourceList;
            internal DropDown DistributionType;
            private RadioButton Hybrid_Select;
            private RadioButton Mapping_Select;
            internal DropDown Graph_Octave;
            private ListBox Channel_View;
            private GroupBox Data_From;
            private Button Remove_Channel;
            private Button Save_Channels;
            private Button Add_Channel;
            internal NumericStepper Crossover;
            private CheckBox Supplement_Numerical;
            private Button Move_Down;
            private Button Move_Up;
            internal Button Export_Filter;
            private DropDown Sample_Freq_Selection;
            internal NumericStepper Normalization_Choice;
            private CheckBox PlayAuralization;

            AuralisationConduit A;
            bool Linear_Phase;
            int[] SrcRendered;
            int RecRendered;
            double[][] Response;
            int SFreq_Rendered;
            public Pach_Auralisation()
            {
                Label label26 = new Label();
                Label label27 = new Label();
                Label label6 = new Label();
                Label label3 = new Label();
                Label label1 = new Label();
                Label ChLbl = new Label();
                Label Hznbelow = new Label();
                Label For = new Label();
                Label label25 = new Label();
                Label label16 = new Label();
                Label label20 = new Label();
                this.Tabs = new TabControl();
                this.Data_Source = new TabPage();
                this.Render_Settings = new TabPage();
                Tabs.Pages.Add(Data_Source);
                Tabs.Pages.Add(Render_Settings);

                this.Azi_Choice = new NumericStepper();
                this.Alt_Choice = new NumericStepper();
                this.Source_Aim = new DropDown();
                this.SourceList = new SourceListBox();
                this.Normalization_Choice = new NumericStepper();
                this.Receiver_Choice = new DropDown();
                this.Graph_Octave = new DropDown();
                this.RenderBtn = new Button();
                this.Analysis_View = new ScottPlot.Eto.EtoPlot();
                this.DryChannel = new NumericStepper();
                this.Signal_Status = new TextBox();
                this.OpenSignal = new Button();
                this.Export_Filter = new Button();
                this.Sample_Freq_Selection = new DropDown();
                this.PlayAuralization = new CheckBox();

                this.Save_Channels = new Button();
                this.Remove_Channel = new Button();
                this.Move_Down = new Button();
                this.DistributionType = new DropDown();
                this.Move_Up = new Button();
                this.Channel_View = new ListBox();
                this.Add_Channel = new Button();
                this.Data_From = new GroupBox();
                this.Crossover = new NumericStepper();
                this.Supplement_Numerical = new CheckBox();
                this.Hybrid_Select = new RadioButton();
                this.Mapping_Select = new RadioButton();

                this.Tabs.SelectedIndexChanged += this.Tab_Selecting;
                this.Data_Source.Text = "Data Source";

                DynamicLayout DS_Layout = new DynamicLayout();
                DS_Layout.DefaultSpacing = new Size(8, 8);
                this.Data_From.Text = "Data From:";
                DynamicLayout Datasource = new DynamicLayout();
                Datasource.DefaultSpacing = new Size(8, 8);
                DynamicLayout DL2 = new DynamicLayout();
                DL2.DefaultSpacing = new Size(8, 8);
                DynamicLayout DL3 = new DynamicLayout();
                DL3.DefaultSpacing = new Size(8, 8);
                this.Supplement_Numerical.Enabled = false;
                this.Supplement_Numerical.Text = "Supplement Numerical for Low Frequencies";
                For.Text = "For :";
                this.Crossover.MaxValue = 22000;
                this.Crossover.MinValue = 20;
                this.Crossover.Value = 160;
                Hznbelow.Text = "Hz. and below.";
                this.Hybrid_Select.Text = "Hybrid";
                this.Hybrid_Select.CheckedChanged += this.Tab_Selecting;
                this.Mapping_Select.Text = "Mapping";
                this.Mapping_Select.CheckedChanged += this.Tab_Selecting;
                DL2.AddRow(Hybrid_Select);
                DL2.AddRow(Mapping_Select);
                DynamicLayout inter = new DynamicLayout(this);
                inter.DefaultSpacing = new Size(8, 8);
                inter.AddRow(Supplement_Numerical);
                DL3.AddRow(inter);
                DL3.AddRow(For, Crossover, Hznbelow);
                Datasource.AddRow(DL2, DL3);
                Data_From.Content = DL2;

                DS_Layout.AddRow(Data_From);

                DynamicLayout TOA_DL = new DynamicLayout();
                TOA_DL.DefaultSpacing = new Size(8, 8);
                label1.Text = "Type of Auralisation:";
                this.DistributionType.Items.Add("Monaural");
                this.DistributionType.Items.Add("Stereo");
                this.DistributionType.Items.Add("Binaural (select file...)");
                this.DistributionType.Items.Add("A-Format (type I-A)");
                this.DistributionType.Items.Add("A-Format (type II-A)");
                this.DistributionType.Items.Add("First Order Ambisonics (ACN+SN3D)");
                this.DistributionType.Items.Add("First Order Ambisonics (FuMa+SN3D)");
                this.DistributionType.Items.Add("Second Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Second Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.Items.Add("Third Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Third Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.Items.Add("Surround Array (select file...)");
                this.DistributionType.SelectedIndex = 1;
                this.DistributionType.SelectedValueChanged += this.DistributionType_SelectedIndexChanged;

                TOA_DL.AddRow(label1, DistributionType);
                DS_Layout.AddRow(TOA_DL);

                ChLbl.Text = "Channels:";
                DS_Layout.AddRow(ChLbl);

                this.Channel_View.Items.Add(channel.Left(0));
                this.Channel_View.Items.Add(channel.Right(1));
                DS_Layout.AddRow(Channel_View);

                DynamicLayout buttons = new DynamicLayout();
                buttons.DefaultSpacing = new Size(8, 8);

                this.Move_Up.Text = "Move Up";
                this.Move_Up.Visible = false;
                this.Move_Up.Click += this.Move_Up_Click;

                this.Move_Down.Enabled = false;
                this.Move_Down.Text = "Move Down";
                this.Move_Down.Visible = false;
                this.Move_Down.Click += this.Move_Down_Click;

                buttons.AddRow(Move_Up, Move_Down);

                this.Add_Channel.Enabled = false;
                this.Add_Channel.Text = "Add";
                this.Add_Channel.Visible = false;
                this.Add_Channel.Click += this.Add_Channel_Click;

                this.Remove_Channel.Enabled = false;
                this.Remove_Channel.Text = "Remove";
                this.Remove_Channel.Visible = false;
                this.Remove_Channel.Click += this.Remove_Channel_Click;

                buttons.AddRow(Add_Channel, Remove_Channel);

                DS_Layout.AddRow(buttons);

                this.Save_Channels.Enabled = false;
                this.Save_Channels.Text = "Save Configuration";
                this.Save_Channels.Visible = false;
                this.Save_Channels.Click += this.Save_Channels_Click;
                DS_Layout.AddRow(Save_Channels);

                Data_Source.Content = DS_Layout;

                this.Render_Settings.Padding = new Padding(6);
                this.Render_Settings.Text = "Render Settings";

                DynamicLayout RSLayout = new DynamicLayout();
                RSLayout.DefaultSpacing = new Size(8, 8);
                DynamicLayout Top = new DynamicLayout();
                Top.DefaultSpacing = new Size(8, 8);
                DynamicLayout GraphCtrls = new DynamicLayout();
                GraphCtrls.DefaultSpacing = new Size(8, 8);

                label20.Text = "Receiver";
                this.Receiver_Choice.Items.Add("0");
                this.Receiver_Choice.SelectedIndex = 0;
                this.Receiver_Choice.SelectedIndexChanged += this.Receiver_Choice_SelectedIndexChanged;
                GraphCtrls.AddRow(label20, Receiver_Choice);

                label25.Text = "Aim at Source";
                this.Source_Aim.SelectedIndexChanged += this.Source_Aim_SelectedIndexChanged;
                GraphCtrls.AddRow(label25, Source_Aim);

                label27.Text = "Altitude";
                this.Alt_Choice.DecimalPlaces = 2;
                this.Alt_Choice.MaxValue = 90;
                this.Alt_Choice.MinValue = -90;
                this.Alt_Choice.ValueChanged += this.Alt_Choice_ValueChanged;
                GraphCtrls.AddRow(label27, Alt_Choice);

                label26.Text = "Azimuth";
                this.Azi_Choice.DecimalPlaces = 2;
                this.Azi_Choice.MaxValue = 360;
                this.Azi_Choice.MinValue = 1;
                this.Azi_Choice.ValueChanged += this.Azi_Choice_ValueChanged;
                GraphCtrls.AddRow(label26, Azi_Choice);

                label6.Text = "Max Output (dB):";
                this.Normalization_Choice.DecimalPlaces = 2;
                this.Normalization_Choice.MaxValue = 200;
                this.Normalization_Choice.MinValue = 0;
                GraphCtrls.AddRow(label6, Normalization_Choice);

                DynamicLayout TL = new DynamicLayout();
                TL.DefaultSpacing = new Size(8, 8);
                SourceList.Size = new Size(-1, 120);
                TL.AddRow(SourceList);

                this.Graph_Octave.Items.Add("62.5 Hz.");
                this.Graph_Octave.Items.Add("125 Hz.");
                this.Graph_Octave.Items.Add("250 Hz.");
                this.Graph_Octave.Items.Add("500 Hz.");
                this.Graph_Octave.Items.Add("1 kHz.");
                this.Graph_Octave.Items.Add("2 kHz.");
                this.Graph_Octave.Items.Add("4 kHz.");
                this.Graph_Octave.Items.Add("8 kHz.");
                this.Graph_Octave.Items.Add("Summation: All Octaves");
                this.Graph_Octave.SelectedIndex = 8;
                this.Graph_Octave.SelectedIndexChanged += this.Graph_Octave_SelectedIndexChanged;
                this.Graph_Octave.Size = new Size(-1, 25);
                TL.AddRow(Graph_Octave);

                Top.AddRow(TL, GraphCtrls);
                RSLayout.AddRow(Top);

                RSLayout.AddRow(Analysis_View);
                Analysis_View.Size = new Size(-1, 300);

                DynamicLayout OC = new DynamicLayout();
                OC.DefaultSpacing = new Size(8, 8);

                this.OpenSignal.Text = "Open...";
                this.OpenSignal.Click += this.OpenSignal_Click;

                label16.Text = "Selected Channel";

                OC.AddRow(OpenSignal, label16, DryChannel);
                RSLayout.AddRow(OC);

                this.Signal_Status.ReadOnly = true;
                RSLayout.AddRow(Signal_Status);

                this.RenderBtn.Enabled = false;
                this.RenderBtn.Text = "Render Auralization";
                this.RenderBtn.Click += this.RenderBtn_Click;

                this.PlayAuralization.Text = "Play Rendering";
                this.PlayAuralization.CheckedChanged += this.PlayAuralization_CheckedChanged;
                DynamicLayout RP = new DynamicLayout();
                RP.DefaultSpacing = new Size(8, 8);
                RP.AddRow(RenderBtn, PlayAuralization);
                RSLayout.Add(RP);

                label3.Text = "Sample Frequency:";

                this.Sample_Freq_Selection.Items.Add("22050 Hz.");
                this.Sample_Freq_Selection.Items.Add("44100 Hz.");
                this.Sample_Freq_Selection.Items.Add("48000 Hz.");
                this.Sample_Freq_Selection.Items.Add("96000 Hz.");
                this.Sample_Freq_Selection.Items.Add("192000 Hz.");
                this.Sample_Freq_Selection.Items.Add("384000 Hz.");

                this.Export_Filter.Text = "Export IR as Wave File";
                this.Export_Filter.Click += this.ExportFilter;
                DynamicLayout FE = new DynamicLayout();
                FE.DefaultSpacing = new Size(8, 8);
                FE.AddRow(label3, Sample_Freq_Selection, Export_Filter);
                RSLayout.AddRow(FE);

                Render_Settings.Content = RSLayout;
                this.Content = Tabs;

                A = new AuralisationConduit();

                DistributionType.SelectedIndex = 1;
                DistributionType_SelectedIndexChanged(this, new EventArgs());
                Sample_Freq_Selection.SelectedIndex = 1;
                Instance = this;
            }

            public void Reset()
            {
                Linear_Phase = false;
                Direct_Data = null;
                Receiver = null;
                IS_Data = null;
                Maps = null;
                Srcs = null;
                Recs = null;
            }

            ///<summary>Gets the only instance of the PachydermAcoustic plug-in.</summary>
            public static Pach_Auralisation Instance
            {
                get;
                private set;
            }

            #region Tab 1
            Direct_Sound[] Direct_Data;
            ImageSourceData[] IS_Data;
            Receiver_Bank[] Receiver;
            PachMapReceiver[] Maps;
            Hare.Geometry.Point[] Srcs;
            Hare.Geometry.Point[] Recs;
            int SampleRate;
            double CutoffTime;

            private void Draw_Feedback()
            {
                if (Hybrid_Select.Checked && Direct_Data != null)
                {
                    if (PachHybridControl.Instance != null && Receiver_Choice.SelectedIndex < 0) return;
                    Hare.Geometry.Point[] rec = new Hare.Geometry.Point[Recs.Length];
                    for (int i = 0; i < Recs.Length; i++) rec[i] = Recs[i];
                    AuralisationConduit.Instance.Enabled = true;
                    AuralisationConduit.Instance.add_Receivers(rec);
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();
                    foreach (Direct_Sound D in Direct_Data) pts.Add(D.Src_Origin);
                    AuralisationConduit.Instance.add_Sources(pts);
                    List<Deterministic_Reflection> Paths = new List<Deterministic_Reflection>();
                    List<Polyline> Lines = new List<Polyline>();
                    foreach (ImageSourceData I in IS_Data) if (I != null) Paths.AddRange(I.Paths[Receiver_Choice.SelectedIndex]);
                    foreach (Deterministic_Reflection p in Paths) foreach (Hare.Geometry.Point[] P in p.Path)
                        {
                            List<Rhino.Geometry.Point3d> PTS = new List<Rhino.Geometry.Point3d>();
                            foreach (Hare.Geometry.Point hpt in P)
                            {
                                PTS.Add(Utilities.RCPachTools.HPttoRPt(hpt));
                            }
                            Lines.Add(new Polyline(PTS));
                        }
                    AuralisationConduit.Instance.add_Reflections(Lines);
                    pts.Clear();
                    List<Vector3d> Dirs = new List<Vector3d>();
                    for (int i = 0; i < this.Channel_View.Items.Count; i++)
                    {
                        Hare.Geometry.Vector TempDir = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(((channel)Channel_View.Items[i]).V, 0, -(double)Alt_Choice.Value, true), -(double)Azi_Choice.Value, 0, true);//new Hare.Geometry.Vector(Speaker_Directions[i].X, Speaker_Directions[i].Y, Speaker_Directions[i].Z)
                        TempDir.Normalize();
                        pts.Add(Recs[0] + (TempDir) * -.343 * Math.Max(5, ((channel)Channel_View.Items[i]).delay));
                        Dirs.Add(new Vector3d(TempDir.x, TempDir.y, TempDir.z));
                    }
                    AuralisationConduit.Instance.add_Speakers(pts, Dirs);
                    AuralisationConduit.Instance.set_direction(Utilities.RCPachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), Utilities.RCPachTools.HPttoRPt(Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(double)Alt_Choice.Value, true), -(double)Azi_Choice.Value, 0, true)));
                }
                else if (Mapping_Select.Checked)
                {
                    if (Pach_Mapping_Control.Instance != null && !Pach_Mapping_Control.Instance.Auralisation_Ready() && Receiver_Choice.SelectedIndex < 0) return;
                    if (Maps == null || Maps[0] == null) return;
                    AuralisationConduit.Instance.add_Receivers(Maps[0].Origins());
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();
                    foreach (PachMapReceiver m in Maps) pts.Add(m.Src);
                    AuralisationConduit.Instance.add_Sources(pts);
                }
                else
                {
                    AuralisationConduit.Instance.Enabled = false;
                }
                Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.Redraw();
            }

            public void Set_Phase_Regime(bool Linear_Phase)
            {
                if (Direct_Data == null) return;

                if ((Linear_Phase != true && Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System) || (Linear_Phase != false && Audio.Pach_SP.Filter is Audio.Pach_SP.Minimum_Phase_System) || Direct_Data[0].F == null)
                {
                    for (int i = 0; i < Direct_Data.Length; i++) Direct_Data[i].Create_Filter();
                    ProgressBox VB = new ProgressBox("Creating IR Filters for Deterministic Reflections...");
                    VB.Show(Rhino.RhinoDoc.ActiveDoc);
                    for (int i = 0; i < IS_Data.Length; i++) IS_Data[i].Create_Filter(Direct_Data[i].SWL, 4096, VB);
                    VB.change_title("Creating Impulse Responses...");
                    for (int i = 0; i < Receiver.Length; i++) Receiver[i].Create_Filter(VB);
                    VB.Close();
                    this.Linear_Phase = Linear_Phase;
                }
            }

            public double[][] RenderFilter(int Sample_Frequency)
            {
                double[][] Temp;
                double[][] Response;
                ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                VB.Show(Rhino.RhinoDoc.ActiveDoc);

                switch (DistributionType.SelectedValue as string)
                {
                    case "Monaural":
                        Response = new double[1][];
                        Response[0] = (IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true, VB));
                        break;
                    case "First Order Ambisonics (ACN+SN3D)":
                        Response = new double[4][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true, VB);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        break;
                    case "First Order Ambisonics (FuMa+SN3D)":
                        Response = new double[4][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true, VB);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        break;
                    case "Second Order Ambisonics(ACN+SN3D)":
                        Response = new double[9][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true, VB);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        break;
                    case "Second Order Ambisonics(FuMa+SN3D)":
                        Response = new double[9][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true, VB);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        break;
                    case "Third Order Ambisonics(ACN+SN3D)":
                        Response = new double[16][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true, VB);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        Temp = IR_Construction.AurFilter_Ambisonics3(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[9] = Temp[0];
                        Response[10] = Temp[1];
                        Response[11] = Temp[2];
                        Response[12] = Temp[3];
                        Response[13] = Temp[4];
                        Response[14] = Temp[5];
                        Response[15] = Temp[6];
                        break;

                    case "Third Order Ambisonics(FuMa+SN3D)":
                        Response = new double[16][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true, VB);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        Temp = IR_Construction.AurFilter_Ambisonics3(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[9] = Temp[0];
                        Response[10] = Temp[1];
                        Response[11] = Temp[2];
                        Response[12] = Temp[3];
                        Response[13] = Temp[4];
                        Response[14] = Temp[5];
                        Response[15] = Temp[6];
                        break;
                    default:
                        Response = new double[Channel_View.Items.Count][];
                        for (int i = 0; i < Channel_View.Items.Count; i++)
                        {
                            double alt = -(double)Alt_Choice.Value + 180 * Math.Asin((Channel_View.Items[i] as channel).V.z) / Math.PI;
                            double azi = (double)Azi_Choice.Value + 180 * Math.Atan2((Channel_View.Items[i] as channel).V.y, (Channel_View.Items[i] as channel).V.x) / Math.PI;
                            if (alt > 90) alt -= 180;
                            if (alt < -90) alt += 180;
                            if (azi > 360) azi -= 360;
                            if (azi < 0) azi += 360;
                            Response[i] = IR_Construction.AurFilter_Directional(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Graph_Octave.SelectedIndex, Receiver_Choice.SelectedIndex, SelectedSources(), false, alt, azi, true, true, VB);
                        }
                        break;
                }
                return Response;
            }

            private void Update_Graph(object sender, EventArgs e)
            {
                Analysis_View.Plot.Clear();
                
                int REC_ID = 0;
                try
                {
                    REC_ID = Receiver_Choice.SelectedIndex;

                    int OCT_ID = Graph_Octave.SelectedIndex;
                    Analysis_View.Plot.Title("Logarithmic Energy Time Curve");
                    Analysis_View.Plot.XAxis.Label.Text = "Time (seconds)";
                    Analysis_View.Plot.YAxis.Label.Text = "Sound Pressure Level (dB)";

                    List<int> SrcIDs = SourceList.SelectedSources();

                    List<ScottPlot.Color> C = new List<ScottPlot.Color> {ScottPlot.Colors.Red, ScottPlot.Colors.OrangeRed, ScottPlot.Colors.Orange, ScottPlot.Colors.DarkGoldenRod, ScottPlot.Colors.Olive, ScottPlot.Colors.Green, ScottPlot.Colors.Aquamarine, ScottPlot.Colors.Azure, ScottPlot.Colors.Blue, ScottPlot.Colors.Indigo, ScottPlot.Colors.Violet};

                    Response = RenderFilter(44100);

                    //Get the maximum value of the Direct Sound
                    double DirectMagnitude = 0;
                    foreach (int i in SrcIDs)
                    {
                        double[] E = Direct_Data[i].EnergyValue(OCT_ID, REC_ID);
                        for (int j = 0; j < E.Length; j++)
                        {
                            double D = AcousticalMath.SPL_Intensity(E[j])-150;
                            if (D > DirectMagnitude) DirectMagnitude = D;
                        }
                    }

                    double[] time = new double[Response[0].Length];
                    for (int i = 0; i < Response[0].Length; i++)
                    {
                        time[i] = (double)i / SampleRate - 2048f/SampleRate;
                    }

                    for (int i = 0; i < Response.Length; i++)
                    {
                        if (OCT_ID < 8)
                        {
                            double[] filter = Audio.Pach_SP.FIR_Bandpass(Response[i], OCT_ID, SampleRate, 0);
                            Array.Resize(ref filter, filter.Length - 12288);
                            //String.Format("Channel {0}", i)
                            Analysis_View.Plot.Add.Scatter(time, AcousticalMath.SPL_Pressure_Signal(filter), C[i % 10]);
                        }
                        else
                        {
                            Analysis_View.Plot.Add.Scatter(time, AcousticalMath.SPL_Pressure_Signal(Response[i]), C[i % 10]);
                        //String.Format("Channel {0}", i),
                        }
                    }
                    Analysis_View.Plot.XAxis.Max = time[time.Length - 1];
                    Analysis_View.Plot.XAxis.Min = time[0];
                    Analysis_View.Plot.YAxis.Max = DirectMagnitude;//(OCT_ID == 8)? 3E-6 * Math.Pow(10, DirectMagnitude/20) * 1.1 : 
                    Analysis_View.Plot.YAxis.Min = DirectMagnitude - 100;

                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                }
                catch { return; }
                Analysis_View.Invalidate();
                Draw_Feedback();
            }

            public void ReadArray()
            {
                GetWave.Filters.Add(" Array Description Text File (*.txt) |*.txt");
                if (GetWave.ShowDialog(this) == DialogResult.Ok)
                {
                    Channel_View.Items.Clear();
                    System.IO.StreamReader Reader;
                    Reader = new System.IO.StreamReader(GetWave.FileName);
                    do
                    {
                        try
                        {
                            string speaker = Reader.ReadLine();
                            string[] s = speaker.Split(new char[] { ':' });
                            Channel_View.Items.Add(new channel(int.Parse(s[0]), new Hare.Geometry.Vector(double.Parse(s[1]),double.Parse(s[2]),double.Parse(s[3])), (channel.channel_type)int.Parse(s[4]), double.Parse(s[5])));
                        }
                        catch (System.Exception)
                        {
                            Reader.Close();
                            break;
                        }
                    } while (!Reader.EndOfStream);
                    Reader.Close();
                }
            }
            #endregion

            #region Tab 2
            //Signal Processing 
            private Eto.Forms.OpenFileDialog GetWave = new Eto.Forms.OpenFileDialog();

            private void OpenSignal_Click(object sender, System.EventArgs e)
            {
                GetWave.Filters.Add(" Wave Audio (*.wav) |*.wav");
                if (GetWave.ShowDialog(this) == DialogResult.Ok)
                {
                    Signal_Status.Text = GetWave.FileName;
                    RenderBtn.Enabled = true;
                    int[][] signal = Audio.Pach_SP.Wave.ReadtoInt(Signal_Status.Text, false, out SampleRate);
                    DryChannel.MinValue = 1;
                    DryChannel.MaxValue = signal.Length;
                }
            }

            private List<int> SelectedSources()
            {
                List<int> indices = new List<int>();
                indices = SourceList.SelectedSources();
                return indices;
            }

            private void OpenWaveFile(out int Sample_Freq, out double[] SignalETC)
            {
                SignalETC = Audio.Pach_SP.Wave.ReadtoDouble(Signal_Status.Text, true, out Sample_Freq)[0];
            }

            private void Clear_Render()
            {
                SrcRendered = null;
                RecRendered = -1;
                Response = null;
                SFreq_Rendered = -1;
            }

            private bool IsRendered()
            {
                List<int> srcs = SourceList.SelectedSources();

                if (Receiver_Choice.SelectedIndex != RecRendered) return false;
                if ((Response == null) || (Response[0] == null)) return false;
                if (SrcRendered == null || SrcRendered.Length != srcs.Count) return false;
                for (int i = 0; i < srcs.Count; i++)
                {
                    if (SrcRendered[i] != srcs[i]) return false;
                }
                return true;
            }

            private void RenderBtn_Click(object sender, System.EventArgs e)
            {
                if (Response == null || Response.Length == 0)
                {
                    Rhino.RhinoApp.WriteLine("No impulse response found to render...");
                    return;
                }

                int SamplesPerSec;
                double[] SignalBuffer;
                OpenWaveFile(out SamplesPerSec, out SignalBuffer);

                float maxvalue = 0;
                //Normalize input signal...
                for (int j = 0; j < SignalBuffer.Length; j++) maxvalue = (float)Math.Max(maxvalue, Math.Abs(SignalBuffer[j]));
                for (int j = 0; j < SignalBuffer.Length; j++) SignalBuffer[j] /= maxvalue;
                //Convert pressure response to a 24-bit dynamic range:

                double[][] Render_Response = RenderFilter(SamplesPerSec);

                float[][] NewSignal = new float[(int)Render_Response.Length][];
                for (int i = 0; i < Render_Response.Length; i++)
                {
                    NewSignal[i] = Pach_SP.FFT_Convolution(SignalBuffer, Render_Response[i], 0);
                    for (int j = 0; j < NewSignal[i].Length; j++) NewSignal[i][j] *= (float)(Math.Pow(10, 120 / 20) / Math.Pow(10, ((double)Normalization_Choice.Value + 60)/20));
                }

                List<int> srcs = SourceList.SelectedSources();

                SrcRendered = new int[srcs.Count];
                for (int j = 0; j < srcs.Count; j++)
                {
                    SrcRendered[j] = srcs[j];
                }
                RecRendered = Receiver_Choice.SelectedIndex;
                SFreq_Rendered = SamplesPerSec;

                Eto.Forms.SaveFileDialog SaveWave = new Eto.Forms.SaveFileDialog();
                if (NewSignal.Length < 4)
                {
                    SaveWave.Filters.Add(" Wave Audio (*.wav) |*.wav");
                }
                else 
                {
                    SaveWave.Filters.Add("Extended Wave Audio (*.wavex) |*.wavex");
                }

                if (SaveWave.ShowDialog(this) == DialogResult.Ok)
                {
                    if (Response == null || Response.Length == 0)
                    {
                        Rhino.RhinoApp.WriteLine("No impulse response found to render...");
                        return;
                    }

                    Audio.Pach_SP.Wave.Write(NewSignal, SamplesPerSec, SaveWave.FileName);

                    if (PlayAuralization.Checked.Value)
                    {
                        //Player = new System.Media.SoundPlayer(SaveWave.FileName);
                        //Player.Play();
                    }
                }
            }

            ///System.Media.SoundPlayer Player =  new System.Media.SoundPlayer();

            private void ExportFilter(object sender, EventArgs e)
            {
                Eto.Forms.SaveFileDialog SaveWave = new Eto.Forms.SaveFileDialog();

                if (Response.Length < 4)
                {
                    SaveWave.Filters.Add("Wave Audio (*.wav) |*.wav");
                }
                else 
                {
                    SaveWave.Filters.Add("Wave Audio (*.wavex) |*.wavex");
                }

                int SamplesPerSec = 44100;

                switch (Sample_Freq_Selection.SelectedIndex)
                {
                    case 0:
                        SamplesPerSec = 22050;
                        break;
                    case 1:
                        SamplesPerSec = 44100;
                        break;
                    case 2:
                        SamplesPerSec = 48000;
                        break;
                    case 3:
                        SamplesPerSec = 96000;
                        break;
                    case 4:
                        SamplesPerSec = 192000;
                        break;
                    case 5:
                        SamplesPerSec = 384000;
                        break;
                }

                double[][] Render_Response = RenderFilter(SamplesPerSec);
                float[][] RR = new float[Render_Response.Length][];
                int maxlength = 0;
                for (int j = 0; j < Render_Response.Length; j++) maxlength = Math.Max(Render_Response[j].Length, maxlength);

                float mod = (float)(Math.Pow(10, 120 / 20) / Math.Pow(10, ((double)Normalization_Choice.Value + 15)/ 20));
                for (int c = 0; c < Render_Response.Length; c++) RR[c] = new float[maxlength];

                for (int j = 0; j < Render_Response[0].Length; j++)
                {
                    for (int c = 0; c < Render_Response.Length; c++)  RR[c][j] = (j > Render_Response[c].Length - 1)? 0 : (float)Render_Response[c][j] * mod;
                }

                if (SaveWave.ShowDialog(this) == DialogResult.Ok)
                {
                    Audio.Pach_SP.Wave.Write(RR, SamplesPerSec, SaveWave.FileName, 24);
                }
            }

            private void OpenDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Update_Graph(null, System.EventArgs.Empty);
            }

            private void Receiver_Choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graph(null, System.EventArgs.Empty);
                Source_Aim_SelectedIndexChanged(null, EventArgs.Empty);
            }

            #endregion
            private void SourceList_MouseUp(object sender, MouseEventArgs e)
            {
                Update_Graph(null, System.EventArgs.Empty);
            }

            private void Source_Aim_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Receiver_Choice.SelectedIndex < 0 || Source_Aim.SelectedIndex < 0) return;
                double azi, alt;

                PachTools.World_Angles(Direct_Data[Source_Aim.SelectedIndex].Src.Origin(), Recs[Receiver_Choice.SelectedIndex], true, out alt, out azi);

                Alt_Choice.Value = alt;
                Azi_Choice.Value = azi;
            }

            private void DistributionType_SelectedIndexChanged(object sender, EventArgs e)
            {
                Channel_View.Items.Clear();                
                disable_CEdit();
                switch (DistributionType.SelectedValue.ToString())
                {
                    case "Monaural":
                        Channel_View.Items.Add(channel.Monaural(0));
                        break;
                    case "Stereo":
                        Channel_View.Items.Add(channel.Left(0));
                        Channel_View.Items.Add(channel.Right(1));
                        break;
                    case "Binaural (select file...)":
                        Eto.Forms.MessageBox.Show("Not yet implemented... If you would like to see this area completed, please help. Get in touch with me to suggest an HRTF standard, and possibly help implement. --Arthur");
                        DistributionType.SelectedIndex = 1;
                        break;
                    case "A-Format (type I-A)":
                        Channel_View.Items.Add(channel.FLU(0));
                        Channel_View.Items.Add(channel.FRD(1));
                        Channel_View.Items.Add(channel.BLD(2));
                        Channel_View.Items.Add(channel.BRU(3));
                        break;
                    case "A-Format (type II-A)":
                        Channel_View.Items.Add(channel.FLD(0));
                        Channel_View.Items.Add(channel.FRU(1));
                        Channel_View.Items.Add(channel.BLU(2));
                        Channel_View.Items.Add(channel.BRD(3));
                        break;
                    case "B-Format":
                        
                        break;
                    case "Surround Array (select file...)":
                        enable_CEdit();
                        ReadArray();
                        break;
                }
                Draw_Feedback();
            }

            private void enable_CEdit()
            {
                Add_Channel.Enabled = true;
                Add_Channel.Visible = true;
                Remove_Channel.Enabled = true;
                Remove_Channel.Visible = true;
                Move_Up.Enabled = true;
                Move_Up.Visible = true;
                Move_Down.Enabled = true;
                Move_Down.Visible = true;
                Save_Channels.Enabled = true;
                Save_Channels.Visible = true;
            }

            private void disable_CEdit()
            {
                Add_Channel.Enabled = false;
                Add_Channel.Visible = false;
                Remove_Channel.Enabled = false;
                Remove_Channel.Visible = false;
                Move_Up.Enabled = false;
                Move_Up.Visible = false;
                Move_Down.Enabled = false;
                Move_Down.Visible = false;
                Save_Channels.Enabled = false;
                Save_Channels.Visible = false;
            }

            private void Tab_Selecting(object sender, EventArgs e)
            {
                if (Tabs.SelectedIndex == 1)
                {
                    Receiver_Choice.Items.Clear();
                    if (Receiver != null && Receiver.Length > 0)
                    {
                        for (int i = 0; i < Recs.Length; i++)
                        {
                            Receiver_Choice.Items.Add(i.ToString());
                        }
                        Receiver_Choice.SelectedIndex = 0;
                        Update_Graph(this, new System.EventArgs());
                    }
                }
                else if (Tabs.SelectedIndex == 0)
                {
                    SourceList.Clear();
                    Source_Aim.Items.Clear();

                    if (Hybrid_Select.Checked)
                    {
                        UI.PachHybridControl.Instance.GetSims(ref Srcs, ref Recs, ref Direct_Data, ref IS_Data, ref Receiver);
                        //Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                        if (Direct_Data != null)
                        {
                            CutoffTime = Direct_Data[0].Cutoff_Time;
                            SampleRate = (int)Direct_Data[0].SampleRate;
                        }
                        else { return; }
                        if (Direct_Data != null && Direct_Data.Length > 0)
                        {
                            for (int i = 0; i < Direct_Data.Length; i++)
                            {
                            //    SourceList.Add(String.Format("S{0}-", i) + Direct_Data[i].Src.Type());
                                Source_Aim.Items.Add(String.Format("S{0}-", i) + Direct_Data[i].Src.Type());
                            }
                            SourceList.Populate(null, null, Receiver);
                            SourceList.Set_Src_Checked(0, true);
                            Source_Aim.SelectedIndex = 0;
                        }

                        double max = 0;
                        for (int i = 0; i < Direct_Data.Length; i++)
                        {
                            ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                            VB.Show();
                            double[] Response = (IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, 44100, i, SelectedSources(), false, true, VB));
                            VB.Close();
                            double tmax = Math.Max(Response.Max(), Math.Abs(Response.Min()));
                            max = Math.Max(max, tmax);
                        }
                        max = AcousticalMath.SPL_Pressure(max);
                        Normalization_Choice.Value = max;
                    }
                    else if (Mapping_Select.Checked)
                    {
                        if (UI.Pach_Mapping_Control.Instance != null && UI.Pach_Mapping_Control.Instance.Auralisation_Ready())
                        {
                            UI.Pach_Mapping_Control.Instance.GetSims(ref Maps);
                            CutoffTime = Maps[0].CutOffTime;
                            SampleRate = Maps[0].SampleRate;
                        }
                        if (Maps != null)
                        {
                            for (int i = 0; i < Maps.Length; i++)
                            {
                                //SourceList.Items.Add(String.Format("S{0}-", i) + Maps[i].SrcType);
                                Source_Aim.Items.Add(String.Format("S{0}-", i) + Maps[i].SrcType);
                            }
                            SourceList.Populate(null, null, Receiver);
                            SourceList.Set_Src_Checked(0, true);
                            Source_Aim.SelectedIndex = 0;
                        }
                        else { return; }
                    }
                }
                Draw_Feedback();
            }

            private void Move_Up_Click(object sender, EventArgs e)
            {
                int s = Channel_View.SelectedIndex;
                channel t = (channel)Channel_View.Items[s];
                Channel_View.Items.Insert(s-1,(IListItem)t);
                Channel_View.Items.RemoveAt(s + 1);
                Update_Channel_IDS();
                Clear_Render();
            }

            private void Move_Down_Click(object sender, EventArgs e)
            {
                int s = Channel_View.SelectedIndex;
                channel t = (channel)Channel_View.Items[s];
                Channel_View.Items.Insert(s + 1, (IListItem)t);
                Channel_View.Items.RemoveAt(s - 1);
                Update_Channel_IDS();
                Clear_Render();
            }

            private void Add_Channel_Click(object sender, EventArgs e)
            {
                double azi = 0;
                double alt = 0;
                double d = 0;
                Rhino.Input.RhinoGet.GetNumber("Enter the azimuth of the Speaker in degrees (X = front/back, Y = Left/Right)...",false, ref azi,-360, 360);
                Rhino.Input.RhinoGet.GetNumber("Enter the altitude of the Speaker in degrees (Z = Up/Down)...",false, ref alt,-90, 90);
                Rhino.Input.RhinoGet.GetNumber("Enter the distance from the listener in meters...", false, ref d, 0, 100);
                azi *= Math.PI / 180;
                alt *= Math.PI / 180;
                Channel_View.Items.Add(new channel(Channel_View.Items.Count, new Hare.Geometry.Vector(-Math.Cos(azi) * Math.Cos(alt), -Math.Sin(azi) * Math.Cos(alt), -Math.Sin(alt)), channel.channel_type.Custom, d / 0.343));
                Clear_Render();
            }

            private void Remove_Channel_Click(object sender, EventArgs e)
            {
                Channel_View.Items.RemoveAt(Channel_View.SelectedIndex);
                Update_Channel_IDS();
                Clear_Render();
            }

            private void Update_Channel_IDS()
            {
                int i = 0;
                foreach (channel c in Channel_View.Items)
                {
                    c.set_index(i);
                    i++;
                }
                Clear_Render();
            }

            private void Save_Channels_Click(object sender, EventArgs e)
            {
                Eto.Forms.SaveFileDialog SaveArray = new Eto.Forms.SaveFileDialog();
                SaveArray.Filters.Add(" Array Description Text File (*.txt) |*.txt");

                if (SaveArray.ShowDialog(this) == DialogResult.Ok)
                {
                    System.IO.StreamWriter SW = new System.IO.StreamWriter(SaveArray.FileName);

                    for (int i = 0; i < Channel_View.Items.Count; i++)
                    {
                        channel c = (channel)Channel_View.Items[i];
                        string Entry = c._index.ToString() + ':' + c.V.x.ToString() + ':' + c.V.y.ToString() + ':' + c.V.z.ToString() + ":" + c.Type.GetHashCode() + ":" + c.delay.ToString();
                        SW.WriteLine(Entry);
                    }
                    SW.Close();
                }
            }

            private class channel: IListItem
            {
                public double delay;
                public Hare.Geometry.Vector V;
                public channel_type Type;
                public int _index;

                string IListItem.Text { get => this.ToString(); set => throw new Exception(); }

                string IListItem.Key => this.ToString();

                public channel(int index, Hare.Geometry.Vector Dir, channel_type c, double delay_ms)
                {
                    _index = index;
                    V = Dir;
                    Type = c;
                    delay = delay_ms;
                }

                [Flags]
                public enum channel_type
                {
                    Omnidirectional = 0x01,
                    Left = 0x02,
                    Right = 0x04,
                    hrtf = 0x08,
                    Custom = 0x10
                }

                public void set_index(int index)
                {
                    _index = index;
                }

                public static channel Left(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0), channel_type.Left, 0);
                }

                public static channel Right(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(2), -1 / Math.Sqrt(2), 0), channel_type.Right, 0);
                }

                public static channel FLU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel FRU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }
 
                public static channel FLD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel FRD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), -1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BLU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BRU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BLD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BRD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel Monaural(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(0, 0, 0), channel_type.Omnidirectional, 0);
                }

                public override string ToString()
                {
                    switch (Type)
                    {
                        case channel_type.Omnidirectional:
                            return "0: Omnidirectional";
                        case channel_type.Left:
                            return string.Format("{0}: Stereo Left", _index);
                        case channel_type.Right:
                            return string.Format("{0}: Stereo Right", _index);
                        case (channel_type.Left | channel_type.hrtf):
                            return string.Format("{0}: Left Ear", _index);
                        case (channel_type.Right | channel_type.hrtf):
                            return string.Format("{0}: Right Ear", _index);
                        case channel_type.Custom:
                            return string.Format("{0}: Dir-({1},{2},{3}), Delay {4} ms.", _index, Math.Round(V.x,2), Math.Round(V.y,2), Math.Round(V.z,2), Math.Round(delay));
                        default:
                            return "Whoops... Doesn't conform";
                    }
                }
            }

            private void Alt_Choice_ValueChanged(object sender, EventArgs e)
            {
                if (Alt_Choice.Value == 91) Alt_Choice.Value = -90;
                else if (Alt_Choice.Value == -91) Alt_Choice.Value = 90;

                Update_Graph(sender, e);
            }

            private void Azi_Choice_ValueChanged(object sender, EventArgs e)
            {
                if (Azi_Choice.Value == 360) Azi_Choice.Value = 0;
                else if (Azi_Choice.Value == -1) Azi_Choice.Value = 359;

                Update_Graph(sender, e);
            }

            private void Graph_Octave_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graph(sender, e);
            }

            private void PlayAuralization_CheckedChanged(object sender, EventArgs e)
            {
                //if (PlayAuralization.Checked.Value) Player.Play();
                //else Player.Stop();
            }

            public string Text
            {
                get
                {
                    return this.ToString();
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Render_Settings.Dispose();
                Source_Aim.Dispose();
                Alt_Choice.Dispose();
                Azi_Choice.Dispose();
                RenderBtn.Dispose();
                Analysis_View.Dispose();
                DryChannel.Dispose();
                Signal_Status.Dispose();
                OpenSignal.Dispose();
                Receiver_Choice.Dispose();
                Tabs.Dispose();
                Data_Source.Dispose();
                SourceList.Dispose();
                DistributionType.Dispose();
                Hybrid_Select.Dispose();
                Mapping_Select.Dispose();
                Graph_Octave.Dispose();
                Channel_View.Dispose();
                Data_From.Dispose();
                Remove_Channel.Dispose();
                Save_Channels.Dispose();
                Add_Channel.Dispose();
                Crossover.Dispose();
                Supplement_Numerical.Dispose();
                Move_Down.Dispose();
                Move_Up.Dispose();
                Export_Filter.Dispose();
                Sample_Freq_Selection.Dispose();
                Normalization_Choice.Dispose();
                PlayAuralization.Dispose();
                GetWave.Dispose();
            }


            #region IPanel methods
            public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
            {
                // Called when the panel tab is made visible, in Mac Rhino this will happen
                // for a document panel when a new document becomes active, the previous
                // documents panel will get hidden and the new current panel will get shown.
            }

            public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
            {
                // Called when the panel tab is hidden, in Mac Rhino this will happen
                // for a document panel when a new document becomes active, the previous
                // documents panel will get hidden and the new current panel will get shown.
            }

            public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
            {
                // Called when the document or panel container is closed/destroyed
            }
            #endregion IPanel methods
        }
    }
}