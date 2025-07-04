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
//'GNU General Public License for more    details. 
//' 
//'You should have received a copy of the GNU General Public 
//'License along with Pachyderm-Acoustic; if not, write to the Free Software 
//'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA. 

using Rhino.Geometry;
using System;
using Eto.Drawing;
using Eto.Forms;
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using Rhino.Display;
using System.Linq;
using Rhino.UI;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("55E14BEE-72F4-4d8c-B751-9BED20A7C5BC")]
        public class Pach_Mapping_Control : Panel, IPanel
        {
            private SegmentedButton FileMenu;
            private MenuSegmentedItem fileToolStripMenuItem;
            private ButtonMenuItem saveDataToolStripMenuItem;
            private ButtonMenuItem openDataToolStripMenuItem;

            private TabControl tabsPrime;
            private TabPage tabCalculation;
            private TabPage TabAnalysis;
            private TabControl TabOutputModes;

            private Label RecDispLbl;
            private Label RecOrientLbl;
            private Label GeoLbl;
            private Label ConvLbl;
            private CheckBox Screen_Attenuation;
            private RadioButton Rec_Centroid;
            private RadioButton Rec_Vertex;
            private CheckBox Offset_Mesh;
            private CheckBox Sum_Time;
            private RadioButton Disp_Other;
            private RadioButton Disp_Audience;
            private CheckBox DirectionalToggle;
            private RadioButton Spec_Rays;
            private RadioButton DetailedConvergence;
            private RadioButton Minimum_Convergence;
            private Label MapIncr;
            private NumericStepper Increment;
            private Label COTime;
            private NumericStepper CO_TIME;
            private Label RayNo;
            private NumericStepper RT_Count;
            private Button Select_Map;
            private Button Calculate;
            private Medium_Properties_Group Env_Control;

            private TabPage tabOutput;
            private SourceListBox SourceList;
            private Button Plot_Values;
            private GroupBox groupBox_Time;
            private GroupBox IntervalControl;
            private Label MTLbl;
            private Label Max_Time_out;
            private Label Min_Time_out;
            private Label MinTLbl;
            private GroupBox groupBox_ZeroTime;
            private RadioButton ZeroAtSource;
            private RadioButton ZeroAtDirect;
            private GroupBox groupBox_IRSum;
            private RadioButton Coherent;
            private RadioButton Incoherent;
            private CheckBox Discretize;
            private DropDown Parameter_Selection;
            private Color_Output_Control color_control;
            private NumericStepper End_Time_Control;
            private NumericStepper Start_Time_Control;
            private Label ETlbl;
            private Label STlbl;
            private Button Calculate_Map;

            private TabPage tabIRs;
            private DropDown Octave;
            private Label OctLbl;
            private ScottPlot.Eto.EtoPlot Analysis_View;
            private CheckBox Normalize_Graph;
            private CheckBox LockUserScale;
            private DropDown Graph_Octave;//
            private DropDown Graph_Type;
            private NumericStepper Receiver_Selection;
            private Label RecLbl;

            private TabPage tabTflip;
            private NumericStepper Integration_select;
            private Label IntDmnLbl;
            private NumericStepper Step_Select;
            private NumericStepper T_End_select;
            private Label ETLbl;
            private Label STLbl;
            private NumericStepper T_Start_Select;
            private Label StpLbl;
            private Button Flip_Toggle;
            private Button Start_Over;
            private Button Back_Step;
            private Button Forw_Step;
            private NumericStepper Tick_Select;
            private Label TickLbl;
            private TextBox Folder_Status;
            private Label OutLbl;
            private Button OpenFolder;

            public Pach_Mapping_Control()
            {

                this.tabsPrime = new TabControl();
                this.tabCalculation = new TabPage();
                this.Select_Map = new Button();
                this.Env_Control = new Medium_Properties_Group();
                this.RayNo = new Label();
                this.COTime = new Label();
                this.MapIncr = new Label();
                this.RT_Count = new NumericStepper();
                this.CO_TIME = new NumericStepper();
                this.Increment = new NumericStepper();
                this.RecDispLbl = new Label();
                this.RecOrientLbl = new Label();
                this.GeoLbl = new Label();
                this.Offset_Mesh = new CheckBox();
                this.ConvLbl = new Label();
                this.DirectionalToggle = new CheckBox();
                this.Sum_Time = new CheckBox();

                this.Spec_Rays = new RadioButton();
                this.Minimum_Convergence = new RadioButton();
                this.DetailedConvergence = new RadioButton();

                this.Disp_Audience = new RadioButton();
                this.Disp_Other = new RadioButton();

                this.Rec_Centroid = new RadioButton();
                this.Rec_Vertex = new RadioButton();
                this.Calculate = new Button();
                this.Screen_Attenuation = new CheckBox();
                this.tabOutput = new TabPage();

                this.Discretize = new CheckBox();
                this.IntervalControl = new GroupBox();
                this.End_Time_Control = new NumericStepper();
                this.Start_Time_Control = new NumericStepper();
                this.ETlbl = new Label();
                this.STlbl = new Label();

                this.groupBox_ZeroTime = new GroupBox();
                this.ZeroAtSource = new RadioButton();
                this.ZeroAtDirect = new RadioButton();
                this.Calculate_Map = new Button();
                this.Plot_Values = new Button();
                this.tabIRs = new TabPage();
                this.Analysis_View = new ScottPlot.Eto.EtoPlot();
                this.Normalize_Graph = new CheckBox();
                this.LockUserScale = new CheckBox();
                this.Graph_Type = new DropDown();
                //this.Auralisation = new Button();
                this.Receiver_Selection = new NumericStepper();
                this.RecLbl = new Label();
                this.tabTflip = new TabPage();
                this.Folder_Status = new TextBox();
                this.OutLbl = new Label();
                this.OpenFolder = new Button();
                this.MTLbl = new Label();
                this.Max_Time_out = new Label();
                this.Min_Time_out = new Label();
                this.MinTLbl = new Label();
                this.Start_Over = new Button();
                this.Back_Step = new Button();
                this.Forw_Step = new Button();
                this.groupBox_Time = new GroupBox();
                this.Tick_Select = new NumericStepper();
                this.TickLbl = new Label();
                this.Integration_select = new NumericStepper();
                this.IntDmnLbl = new Label();
                this.Step_Select = new NumericStepper();
                this.StpLbl = new Label();
                this.T_End_select = new NumericStepper();
                this.ETLbl = new Label();
                this.STLbl = new Label();
                this.T_Start_Select = new NumericStepper();
                this.Flip_Toggle = new Button();

                DynamicLayout PrimaryLayout = new DynamicLayout();

                this.Parameter_Selection = new DropDown();
                this.OctLbl = new Label();
                this.Octave = new DropDown();
                this.groupBox_IRSum = new GroupBox();
                this.Coherent = new RadioButton();
                this.Incoherent = new RadioButton();
                this.Incoherent.Checked = true;
                this.SourceList = new SourceListBox();

                FileMenu = new SegmentedButton();
                this.fileToolStripMenuItem = new MenuSegmentedItem();
                this.fileToolStripMenuItem.Text = "File";
                this.openDataToolStripMenuItem = new ButtonMenuItem();
                this.openDataToolStripMenuItem.Text = "Open Data...";
                this.openDataToolStripMenuItem.Click += this.OpenDataToolStripMenuItem_Click;
                this.saveDataToolStripMenuItem = new ButtonMenuItem();
                this.saveDataToolStripMenuItem.Text = "Save Data...";
                this.saveDataToolStripMenuItem.Click += this.SaveDataToolStripMenuItem_Click;
                this.fileToolStripMenuItem.Menu = new ContextMenu();
                this.fileToolStripMenuItem.Menu.Items.Add(this.openDataToolStripMenuItem);
                this.fileToolStripMenuItem.Menu.Items.Add(this.saveDataToolStripMenuItem);
                FileMenu.Items.Add(fileToolStripMenuItem);
                FileMenu.Items.Add(new HelpMenu());
                DynamicLayout File = new DynamicLayout();
                File.AddRow(FileMenu, null);
                PrimaryLayout.AddRow(File);
                PrimaryLayout.Padding = new Padding(8, 8, 8, 8);

                PrimaryLayout.DefaultSpacing = new Size(8, 8);
                this.Select_Map.Click += this.Select_Map_Click;

                //Tab page 1 - Calculation
                this.tabCalculation.TabIndex = 0;
                this.tabCalculation.Text = "Calculation";

                DynamicLayout Calc_Layout = new DynamicLayout();
                Calc_Layout.Spacing = new Size(8, 8);
                DynamicLayout mapping = new DynamicLayout();
                mapping.Spacing = new Size(8, 8);
                Calc_Layout.AddRow(mapping);
                this.GeoLbl.Text = "Geometry";
                this.Offset_Mesh.Text = "Mesh Offset by Increment";
                this.Offset_Mesh.Checked = true;
                this.Offset_Mesh.Width = 100;
                Select_Map.Text = "Select Mapping Surface...";
                Select_Map.Width = 350;
                mapping.AddRow(Select_Map, Offset_Mesh);

                GroupBox Rec_Disp = new GroupBox();
                Rec_Disp.Text = "Receiver Displacement";
                StackLayout RD = new StackLayout();
                RD.Spacing = 8;
                this.Disp_Audience.Text = "Audience (+z displacement)";
                this.Disp_Audience.Checked = true;
                this.Disp_Audience.MouseUp += Rec_Disp_CheckedChanged;
                this.Disp_Other.Text = "Other (Normal displacement)";
                this.Disp_Other.MouseUp += Rec_Disp_CheckedChanged;
                RD.Items.Add(Disp_Audience);
                RD.Items.Add(Disp_Other);
                Rec_Disp.Content = RD;

                GroupBox Rec_Orient = new GroupBox();
                Rec_Orient.Text = "Receiver Orientation";
                StackLayout RO = new StackLayout();
                RO.Spacing = 8;
                this.Rec_Centroid.Text = "Face Centroid (no interpolation)";
                this.Rec_Centroid.Checked = true;
                this.Rec_Centroid.MouseUp += Rec_OR_CheckedChanged;
                this.Rec_Vertex.Text = "Vertex (false color interp)";
                this.Rec_Vertex.MouseUp += Rec_OR_CheckedChanged;
                RO.Items.Add(Rec_Centroid);
                RO.Items.Add(Rec_Vertex);
                Rec_Orient.Content = RO;
                DynamicLayout RS = new DynamicLayout();
                RS.Spacing = new Size(8, 8);
                RS.AddRow(Rec_Disp, Rec_Orient);
                Calc_Layout.AddRow(RS);

                GroupBox Conv = new GroupBox();
                Conv.Text = "Convergence:";
                DynamicLayout CL = new DynamicLayout();
                CL.Spacing = new Size(8, 8);
                this.Spec_Rays.Text = "Specify Ray Count";
                this.Minimum_Convergence.Text = "Minimum Convergence";
                this.DetailedConvergence.Text = "Detailed Convergence";
                this.Spec_Rays.MouseUp += this.Convergence_CheckedChanged;
                this.Minimum_Convergence.MouseUp += this.Convergence_CheckedChanged;
                this.DetailedConvergence.MouseUp += this.Convergence_CheckedChanged;
                this.Minimum_Convergence.Checked = true;
                CL.AddRow(Spec_Rays, Minimum_Convergence, DetailedConvergence);
                Calc_Layout.AddRow(CL);

                this.RayNo.Text = "Number of Rays";
                this.COTime.Text = "Cut Off Time (ms)";
                this.MapIncr.Text = "Map Increment (cm.)";
                this.RT_Count.Increment = 1000;
                this.RT_Count.MaxValue = 10000000;
                this.RT_Count.MinValue = 100;
                this.RT_Count.Value = 15000;
                this.CO_TIME.MaxValue = 8000;
                this.CO_TIME.MinValue = 1;
                this.CO_TIME.Value = 300;
                this.Increment.MaxValue = 10000;
                this.Increment.MinValue = 10;
                this.Increment.Value = 50;

                DynamicLayout RCT = new DynamicLayout();
                RCT.Spacing = new Size(8, 8);
                RCT.AddRow(RayNo, null, RT_Count);
                RCT.AddRow(COTime, null, CO_TIME);
                RCT.AddRow(MapIncr, null, Increment);
                Calc_Layout.AddRow(RCT);

                this.Screen_Attenuation.Text = "Screen Calculation (Use for Environmental Noise Simulations)";
                DynamicLayout SARow = new DynamicLayout();
                SARow.AddRow(null, Screen_Attenuation, null);
                Calc_Layout.AddRow(SARow);

                DynamicLayout Rset = new DynamicLayout();
                this.Sum_Time.CheckedChanged += this.Sum_Time_CheckedChanged;
                this.Sum_Time.Text = "SPL Only (sum of time)";
                this.DirectionalToggle.Text = "Track Directionality";
                Rset.AddRow(null, Sum_Time, null, DirectionalToggle, null);
                Calc_Layout.AddRow(Rset);

                this.Calculate.Text = "Run Calculation";
                Calc_Layout.AddRow(Calculate);
                Calc_Layout.AddRow(Env_Control);

                tabCalculation.Content = Calc_Layout;
                this.tabCalculation.Padding = new Padding(8, 8, 8, 8);
                this.tabsPrime.Pages.Add(this.tabCalculation);

                //Tab page 2 = Analysis
                TabAnalysis = new TabPage();
                TabAnalysis.Text = "Analysis";
                DynamicLayout AnalysisLayout = new DynamicLayout();
                Label srclbl = new Label();
                srclbl.Text = "Select Sources...";
                srclbl.TextAlignment = TextAlignment.Left;
                this.SourceList.Height = 80;
                this.SourceList.Update += Update_Graph;
                AnalysisLayout.AddRow(srclbl);
                AnalysisLayout.AddRow(SourceList);
                TabOutputModes = new TabControl();
                AnalysisLayout.AddRow(TabOutputModes);
                TabAnalysis.Content = AnalysisLayout;
                tabsPrime.Pages.Add(TabAnalysis);

                //Tab page 2 - Maps

                DynamicLayout OL = new DynamicLayout();
                OL.Spacing = new Size(8, 8);

                this.Parameter_Selection.Items.Add("Sound Pressure Level");
                this.Parameter_Selection.Items.Add("Sound Pressure Level (A-weighted)");
                this.Parameter_Selection.Items.Add("Directionality");
                this.Parameter_Selection.Items.Add("Reverberation Time (T-15)");
                this.Parameter_Selection.Items.Add("Reverberation Time (T-30)");
                this.Parameter_Selection.Items.Add("Speech Transmission Index - 2003");
                this.Parameter_Selection.Items.Add("Speech Transmission Index - Male");
                this.Parameter_Selection.Items.Add("Speech Transmission Index - Female");
                this.Parameter_Selection.Items.Add("Early Decay Time (EDT)");
                this.Parameter_Selection.Items.Add("Clarity (C-80)");
                this.Parameter_Selection.Items.Add("Definition (D-50)");
                this.Parameter_Selection.Items.Add("Strength/Loudness (G)");
                this.Parameter_Selection.Items.Add("Percent who perceive echoes (EK)");
                this.Parameter_Selection.SelectedIndex = 0;
                this.Parameter_Selection.SelectedIndexChanged += this.Parameter_Selection_SelectedIndexChanged;

                color_control = new Color_Output_Control(true, "Parameter Selection", Parameter_Selection);
                color_control.Update += Parameter_Selection_SelectedIndexChanged;
                color_control.On_Output_Changed += Parameter_Selection_SelectedIndexChanged;
                OL.AddRow(color_control);

                DynamicLayout Output_Times = new DynamicLayout();
                Output_Times.Spacing = new Size(8, 8);

                DynamicLayout DL = new DynamicLayout();
                DL.Spacing = new Size(8, 8);
                this.IntervalControl.Text = "Time Interval";
                this.IntervalControl.Width = 200;
                this.ETlbl.Text = "End Time (ms)";
                this.STlbl.Text = "Start Time (ms)"; 
                DL.AddRow(STlbl, null, Start_Time_Control);
                this.Start_Time_Control.MaxValue = 10000;
                this.End_Time_Control.MaxValue = 10000;
                this.End_Time_Control.Value = 50;
                DL.AddRow(ETlbl, null, End_Time_Control);
                this.IntervalControl.Content = DL;

                StackLayout SZL = new StackLayout();
                SZL.Spacing = 8;
                SZL.Items.Add(ZeroAtSource);
                SZL.Items.Add(ZeroAtDirect);
                groupBox_ZeroTime.Content = SZL;
                this.groupBox_ZeroTime.Text = "Zero Time At:";
                this.groupBox_IRSum.Width = 200;
                this.ZeroAtSource.Text = "Source Actuation";
                this.ZeroAtDirect.Text = "Direct Sound";
                ZeroAtDirect.Checked = true;
                this.ZeroAtSource.MouseUp += ZeroAtChanged;
                this.ZeroAtDirect.MouseUp += ZeroAtChanged;

                Output_Times.AddRow(IntervalControl, groupBox_ZeroTime);
                OL.AddRow(Output_Times);

                StackLayout Buttons = new StackLayout();
                Buttons.Spacing = 8;
                this.Calculate_Map.Text = "Create Map";
                this.Calculate_Map.Width = 200;
                this.Plot_Values.Text = "Plot Numerical Values";
                this.Plot_Values.Width = 200;
                this.Calculate_Map.Click += this.Calculate_Map_Click;
                this.Plot_Values.Click += this.Plot_Values_Click;
                Buttons.Items.Add(Calculate_Map);
                Buttons.Items.Add(Plot_Values);

                StackLayout SL = new StackLayout();
                SL.Spacing = 8;
                groupBox_IRSum.Text = "IR Summing";
                this.Coherent.Text = "Pressure";
                this.Incoherent.Text = "Intensity";
                this.Coherent.MouseUp += this.Coherent_CheckedChanged;
                this.Incoherent.MouseUp += this.Coherent_CheckedChanged;
                SL.Items.Add(Coherent);
                SL.Items.Add(Incoherent);
                groupBox_IRSum.Content = SL;

                DynamicLayout OBR = new DynamicLayout();
                OBR.Spacing = new Size(8, 8);
                OBR.AddRow(groupBox_IRSum, null, Buttons);
                OL.Add(OBR);
                tabOutput.Text = "Maps";
                tabOutput.Content = OL;
                this.tabOutput.Padding = new Padding(8, 8, 8, 8);
                this.TabOutputModes.Pages.Add(this.tabOutput);

                //Tab page 3 - Impulses

                this.tabIRs.Text = "Impulses";
                DynamicLayout IRL = new DynamicLayout();
                IRL.Spacing = new Size(8, 8);
                DynamicLayout IR_ctrls = new DynamicLayout();
                IR_ctrls.Spacing = new Size(8, 8);
                this.RecLbl.Text = "Receiver Index:";
                this.Graph_Type.Items.Add("Energy Time Curve");
                this.Graph_Type.Items.Add("Pressure Time Curve");
                this.Graph_Type.Items.Add("Lateral ETC");
                this.Graph_Type.Items.Add("Lateral PTC");
                this.Graph_Type.Items.Add("Vertical ETC");
                this.Graph_Type.Items.Add("Vertical PTC");
                this.Graph_Type.Items.Add("Fore-Aft ETC");
                this.Graph_Type.Items.Add("Fore-Aft PTC");
                this.Graph_Type.SelectedIndex = 0;
                this.Graph_Type.SelectedIndexChanged += Update_Graph;
                this.Octave.Items.Add("62.5 Hz.");
                this.Octave.Items.Add("125 Hz.");
                this.Octave.Items.Add("250 Hz.");
                this.Octave.Items.Add("500 Hz.");
                this.Octave.Items.Add("1 kHz.");
                this.Octave.Items.Add("2 kHz.");
                this.Octave.Items.Add("4 kHz.");
                this.Octave.Items.Add("8 kHz.");
                this.Octave.Items.Add("Summation: All Octaves");
                this.Octave.SelectedIndex = 8;
                this.Octave.SelectedIndexChanged += Update_Graph;

                this.Normalize_Graph.Text = "Normalize To Direct";
                this.Normalize_Graph.CheckedChanged += Update_Graph;
                this.LockUserScale.Text = "Lock User Scale";
                this.LockUserScale.CheckedChanged += Update_Graph;

                this.Normalize_Graph.Checked = true;
                Receiver_Selection.MinValue = 0;
                Receiver_Selection.ValueChanged += ReceiverSelection_ValueChanged;

                IR_ctrls.AddRow(RecLbl, Receiver_Selection);
                IR_ctrls.AddRow(Graph_Type, Graph_Octave);
                IR_ctrls.AddRow(LockUserScale, Normalize_Graph);
                IRL.AddRow(IR_ctrls);

                GroupBox display = new GroupBox();
                display.Text = "Impulse Response";
                display.Content = Analysis_View;
                Analysis_View.Plot.Title("Logarithmic Energy Time Curve", 12);
                Analysis_View.Size = new Size(-1, 300);
                IRL.Add(display);

                //this.Auralisation.Text = "Go To Auralizations";
                //IRL.AddRow(Auralisation);

                tabIRs.Content = IRL;
                this.tabIRs.Padding = new Padding(8, 8, 8, 8);
                this.TabOutputModes.Pages.Add(this.tabIRs);

                //Tab page 4 - T-flip
                this.tabTflip.Text = "T-Flip";
                DynamicLayout TFL = new DynamicLayout();
                TFL.Spacing = new Size(8, 8);

                this.Tick_Select.MaxValue = 100000;
                this.Tick_Select.Value = 500;
                this.Integration_select.MaxValue = 10000;
                this.Integration_select.Value = 15;
                this.Step_Select.MaxValue = 10000;
                this.Step_Select.Value = 5;
                this.T_End_select.MaxValue = 10000;
                this.T_End_select.Value = 300;
                this.T_Start_Select.MaxValue = 10000;

                this.groupBox_Time.Text = "Time Parameters";
                DynamicLayout TPL = new DynamicLayout();
                TPL.Spacing = new Size(8, 8);
                this.STLbl.Text = "Start Time (Post Direct)";
                TPL.AddRow(STLbl, null, T_Start_Select);
                this.ETLbl.Text = "End Time (Post Direct)";
                TPL.AddRow(ETLbl, null, T_End_select);
                this.StpLbl.Text = "Step Increment";
                TPL.AddRow(StpLbl, null, Step_Select);
                this.IntDmnLbl.Text = "Integration Domain";
                TPL.AddRow(IntDmnLbl, null, Integration_select);
                this.TickLbl.Text = "Tick";
                TPL.AddRow(TickLbl, null, Tick_Select);

                TFL.AddRow(TPL);

                DynamicLayout Time_Display = new DynamicLayout();
                Time_Display.Spacing = new Size(8, 8);
                this.MTLbl.Text = "Max Time: ";
                this.Max_Time_out.Text = "00";
                this.Min_Time_out.Text = "00";
                this.MinTLbl.Text = "Min Time: ";
                Time_Display.AddRow(MinTLbl, Min_Time_out, MTLbl, Max_Time_out);
                TFL.AddRow(Time_Display);

                this.OpenFolder.Click += this.OpenFolder_Click;
                this.Flip_Toggle.Click += Flip_Toggle_Click;
                this.Start_Over.Click += this.Start_Over_Click;
                this.Back_Step.Click += this.Back_Step_Click;
                this.Forw_Step.Click += this.Forw_Step_Click;
                this.Calculate.Click += this.Calculate_Click;

                this.Start_Over.Text = "|<<";
                Start_Over.Width = 100;
                Start_Over.Height = 50;
                this.Back_Step.Text = "<<";
                Back_Step.Width = 100;
                Back_Step.Height = 50;
                this.Flip_Toggle.Text = "Flip";
                Flip_Toggle.Height = 50;
                this.Forw_Step.Text = ">>";
                Forw_Step.Height = 50;
                Forw_Step.Width = 100;
                DynamicLayout Ctrls = new DynamicLayout();
                Ctrls.Spacing = new Size(8, 8);
                Ctrls.AddRow(Start_Over, Back_Step, Flip_Toggle, Forw_Step);
                TFL.AddRow(null);
                TFL.AddRow(Ctrls);

                this.OutLbl.Text = "Select Output Folder";
                this.OpenFolder.Text = "Open...";
                DynamicLayout OFL = new DynamicLayout();
                OFL.Spacing = new Size(8, 8);
                OFL.AddRow(OutLbl, OpenFolder);
                TFL.AddRow(OFL);
                this.Folder_Status.ReadOnly = true;
                TFL.AddRow(Folder_Status);

                tabTflip.Content = TFL;
                this.tabTflip.Padding = new Padding(8, 8, 8, 8);
                this.TabOutputModes.Pages.Add(this.tabTflip);
                this.tabsPrime.SelectedIndex = 0;

                PrimaryLayout.AddRow(tabsPrime);

                this.Content = PrimaryLayout;

                Instance = this;
                Linear_Phase = Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System;
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

            private async void Calculate_Click(object sender, System.EventArgs e)
            {
                PachydermAc_PlugIn p = PachydermAc_PlugIn.Instance;
                string SavePath = null;

                if (PachydermAc_PlugIn.SaveResults)
                {
                    Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                    sf.CurrentFilter = ".pachm";
                    sf.Filters.Add("Pachyderm Mapping Data File (*.pachm)|*.pachm|");
                    sf.Filters.Add("All Files|");
                    if (sf.ShowDialog(this) == DialogResult.Ok)
                    {
                        SavePath = sf.FileName;
                    }
                }

                SourceList.Clear();
                Map = null;
                System.Threading.Thread.Sleep(500);

                if (!p.Source(out Source) || Rec_Srfs == null)
                {
                    Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                    return;
                }

                Hare.Geometry.Point[] SPT;
                p.SourceOrigin(out SPT);
                Calculate.Enabled = false;
                List<Hare.Geometry.Point> P = new List<Hare.Geometry.Point>();
                P.AddRange(SPT);

                Polygon_Scene PScene = Utilities.RCPachTools.Get_Poly_Scene(Env_Control.RelHumidity, false, Env_Control.Temp_Celsius, Env_Control.StaticPressure_kPa, Env_Control.Atten_Method.SelectedIndex, Env_Control.Edge_Frequency);
                if (!PScene.Complete)
                {
                    Calculate.Enabled = true;
                    return;
                }
                WC = new WaveConduit(color_control.Scale, new double[] { Current_SPLMin, Current_SPLMax });

                PScene.partition(P);
                Scene Flex_Scene;
                if (PachydermAc_PlugIn.Instance.Geometry_Spec == 0)
                {
                    RhCommon_Scene NScene = Utilities.RCPachTools.GetNURBSScene(Env_Control.RelHumidity, Env_Control.Temp_Celsius, Env_Control.StaticPressure_hPa, Env_Control.Atten_Method.SelectedIndex, Env_Control.Edge_Frequency);
                    if (!NScene.Complete) return;
                    NScene.partition(P, Pach_Properties.Instance.Spatial_Depth, Pach_Properties.Instance.Max_Polys_Per_Node);
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
                    Mesh Map_Mesh = Utilities.RCPachTools.Create_Map_Mesh(Rec_Srfs, (double)Increment.Value * 0.01);
                    Map[i] = new PachMapReceiver(Utilities.RCPachTools.RhinotoHareMesh(Map_Mesh), Source[i], 1000, (double)Increment.Value * 0.01, Flex_Scene, (double)CO_TIME.Value, Sum_Time.Checked.Value, Disp_Audience.Checked, DirectionalToggle.Checked.Value, Rec_Vertex.Checked, Offset_Mesh.Checked.Value);
                }

                for (int s_id = 0; s_id < Source.Length; s_id++)
                {
                    if (Source[s_id] is LineSource)
                    {
                        //cull samples to suit map density...
                        int skip = (int)Math.Floor(Increment.Value * 2 / 100);
                        if (skip > 1)
                        {
                            Hare.Geometry.Point[] smpl = (Source[s_id] as LineSource).Samples;
                            List<Hare.Geometry.Point> newsmpl = new List<Hare.Geometry.Point>();
                            for (int i = 0; i < smpl.Length; i += skip) newsmpl.Add(smpl[i]);
                            if (newsmpl.Count < 2) newsmpl.Add(smpl.Last());
                            (Source[s_id] as LineSource).Samples = newsmpl.ToArray();
                        }
                    }

                    Direct_Sound DS = new Direct_Sound(Source[s_id], Map[s_id], PScene, new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, this.Screen_Attenuation.Checked.Value, this.Sum_Time.Checked.Value);
                    TaskAwaiter<Simulation_Type> TA = RCPachTools.RunSimulation(DS).GetAwaiter();
                    while (!TA.IsCompleted) await Task.Delay(1500);
                    DS = TA.GetResult() as Direct_Sound;

                    Direct_Sound Direct_Data;
                    if (DS != null)
                    {
                        Direct_Data = DS;
                    }
                    else
                    {
                        Array.Resize(ref Source, s_id);
                        Array.Resize(ref Map, s_id);
                        Calculate.Enabled = true;
                        return;
                    }

                    ConvergenceProgress CP = new ConvergenceProgress(new System.Threading.CancellationTokenSource());
                    if (!Spec_Rays.Checked) CP.Show(); //Rhino.UI.Panels.OpenPanel(new Guid("79B97A26-CEBC-4FA8-8275-9D961ADF1772"));//new System.Threading.Thread(() => {  }).Start();

                    SplitRayTracer RT = new SplitRayTracer(Source[s_id], Map[s_id], Flex_Scene, CutOffLength(), new int[2] { 0, 7 }, 0, Minimum_Convergence.Checked ? -1 : DetailedConvergence.Checked ? 0 : (int)RT_Count.Value, CP);

                    if (!Spec_Rays.Checked) foreach (SplitRayTracer.Convergence_Check c in RT.Convergence_Report) if (c != null) c.On_Convergence_Check += CP.Fill;
                    TaskAwaiter<Simulation_Type> RTA = RCPachTools.RunSimulation(RT).GetAwaiter();
                    while (!RTA.IsCompleted) await Task.Delay(1500);
                    RT = RTA.GetResult() as SplitRayTracer;

                    Rhino.RhinoApp.WriteLine(string.Format("{0} Rays ({1} sub-rays) cast in {2} hours, {3} minutes, {4} seconds.", RT._currentRay.Sum(), RT._rayTotal.Sum(), RT._ts.Hours, RT._ts.Minutes, RT._ts.Seconds));
                    Rhino.RhinoApp.WriteLine("Percentage of energy lost: {0}%", RT.PercentLost);

                    if (RT != null)
                    {
                        Map[s_id] = (PachMapReceiver)RT.GetReceiver;
                    }
                    else
                    {
                        Array.Resize(ref Source, s_id);
                        Array.Resize(ref Map, s_id);
                        Calculate.Enabled = true;
                        break;
                    }
                    Map[s_id].AddDirect(Direct_Data, Source[s_id]);
                    Source[s_id].Lighten();
                }

                if (Source != null)
                {
                    SourceList.Populate(null, null, Map);

                    if (SavePath != null) Utilities.FileIO.Write_pachm(SavePath, Map);
                }

                if (Map != null)
                {
                    Create_Map(false);
                }
                else
                {
                    Receiver_Selection.MaxValue = Map[0].Count;
                }
                Rhino.RhinoApp.WriteLine("Calculation has been completed. Have a nice day!");

                ///////
                //for(int i = 0;i < 10000; i++) Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(((Source[0] as LineSource).D as LineSource.ANCON).Pt[i]);
                ///////
                color_control.Invalidate();
                //Commit_Param_Bounds();
                Calculate.Enabled = true;
            }

            private double CutOffLength()
            {
                return ((double)CO_TIME.Value / 1000) * AcousticalMath.SoundSpeed((double)Env_Control.Air_Temp.Value); ;
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
            //Pach_Graphics.colorscale c_scale;

            public void Create_Map(bool PlotNumbers)
            {
                List<int> srclist = SourceList.SelectedSources();

                if (srclist.Count == 0) return;
                Mesh Mesh_Map;
                switch (Parameter_Selection.SelectedKey as string)
                {
                    case "Sound Pressure Level":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, color_control.Scale);
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Utilities.RC_PachTools.Hare_to_RhinoMesh(Map[0].MapMesh(),true));
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Sound Pressure Level (A-weighted)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLAMin, Current_SPLAMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Directionality":
                        if (Map != null)
                        {
                            PachMapReceiver.Get_Directional_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, color_control.Octave.SelectedIndex, SourceList.SelectedSources());
                        }
                        break;
                    case "Early Decay Time (EDT)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_EDT_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_EDTMin, Current_EDTMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        }
                        break;
                    case "Reverberation Time (T-15)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_RT_Map(Map, 15, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_RTMin, Current_RTMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        }
                        break;
                    case "Reverberation Time (T-30)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_RT_Map(Map, 30, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_RTMin, Current_RTMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        }
                        break;
                    case "Speech Transmission Index - 2003":
                        if (Map != null)
                        {
                            double[] Noise = new double[8];
                            string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                            if (n != null && n.Length > 0)
                            {
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }
                            else
                            {
                                this.Enabled = false;
                                Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                                n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                                this.Enabled = true;
                            }

                            double[] Values = PachMapReceiver.Get_STI_Map(Map, Noise, SourceList.SelectedSources(), 0, Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_STI1Min, Current_STI1Max }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        } 
                        break;
                    case "Speech Transmission Index - Male":
                        if (Map != null)
                        {
                            double[] Noise = new double[8];
                            string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                            if (n != null && n.Length > 0)
                            {
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }
                            else
                            {
                                this.Enabled = false;
                                Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                                n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                                this.Enabled = true;
                            }

                            double[] Values = PachMapReceiver.Get_STI_Map(Map, Noise, SourceList.SelectedSources(), 1, Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_STI2Min, Current_STI2Max }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        }
                        break;
                    case "Speech Transmission Index - Female":
                        if (Map != null)
                        {
                            double[] Noise = new double[8];
                            string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                            if (n != null && n.Length > 0)
                            {
                                string[] ns = n.Split(","[0]);
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            }
                            else
                            {
                                this.Enabled = false;
                                Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                                n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                                string[] ns = n.Split(","[0]);
                                Noise = new double[8];
                                for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                                this.Enabled = true;
                            }

                            double[] Values = PachMapReceiver.Get_STI_Map(Map, Noise, SourceList.SelectedSources(), 2, Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] {Current_STI3Min, Current_STI3Max }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        }
                        break;
                    case "Clarity (C-80)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Clarity_Map(Map, 80, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_CMin, Current_CMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        }
                        break;
                    case "Definition (D-50)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Definition_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_DMin, Current_DMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Strength/Loudness (G)":
                        if (Map != null)
                        {
                            if (SourceList.SelectedSources().Count > 1)
                            {
                                MessageBox.Show("G values", "Important: G values are only valid for single sources. Select a single source, and plot the G-values again.");
                                return;
                            }
                            int SrcID = SourceList.SelectedSources()[0];
                            int oct = color_control.Octave.SelectedIndex;
                            double SWL = 0;
                            if (oct < 7) SWL = Map[SrcID].SWL[oct];
                            else
                            {
                                if (Map[SrcID].SWL == null) Map[SrcID].SWL = new double[8] { 120, 120, 120, 120, 120, 120, 120, 120 };
                                for (int i = 0; i < 8; i++) SWL += Math.Pow(10, Map[SrcID].SWL[i] / 10);
                                SWL = 10 * Math.Log10(SWL);
                            }
                            double[] Values = PachMapReceiver.Get_G_Map(Map, oct, SWL, SrcID, Coherent.Checked);//, G_Ref_Energy[PachTools.OctaveStr2Int(Octave.Text)]
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_GMin, Current_GMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 1);
                        }
                        break;
                    case "Percent who perceive echoes (EK)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_EchoCritPercent_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources());
                            if (Values == null) return;
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_EMin, Current_EMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Parameter selection invalid...");
                        break;
                }
            }

            private void Commit_Param_Bounds()
            {
                switch (Parameter_Selection.SelectedKey)
                {
                    case "Sound Pressure Level":
                        Current_SPLMin = (double)this.color_control.Min;
                        Current_SPLMax = (double)this.color_control.Max;
                        break;
                    case "Sound Pressure Level (A-weighted)":
                        Current_SPLAMin = (double)this.color_control.Min;
                        Current_SPLAMax = (double)this.color_control.Max;
                        break;
                    case "Early Decay Time (T-15)":
                        Current_EDTMin = (double)this.color_control.Min;
                        Current_EDTMax = (double)this.color_control.Max;
                        break;
                    case "Reverberation Time (T-15)":
                        Current_RTMin = (double)this.color_control.Min;
                        Current_RTMax = (double)this.color_control.Max;
                        break;
                    case "Reverberation Time (T-30)":
                        Current_RTMin = (double)this.color_control.Min;
                        Current_RTMax = (double)this.color_control.Max;
                        break;
                    case "Speech Transmission Index - 2003":
                        Current_STI1Min = (double)this.color_control.Min;
                        Current_STI1Max = (double)this.color_control.Max;
                        break;
                    case "Speech Transmission Index - Male":
                        Current_STI2Min = (double)this.color_control.Min;
                        Current_STI2Max = (double)this.color_control.Max;
                        break;
                    case "Speech Transmission Index - Female":
                        Current_STI3Min = (double)this.color_control.Min;
                        Current_STI3Max = (double)this.color_control.Max;
                        break;
                    case "Clarity (C-80)":
                        Current_CMin = (double)this.color_control.Min;
                        Current_CMax = (double)this.color_control.Max;
                        break;
                    case "Definition (D-50)":
                        Current_DMin = (double)this.color_control.Min;
                        Current_DMax = (double)this.color_control.Max;
                        break;
                    case "Strength/Loudness (G)":
                        Current_GMin = (double)this.color_control.Min;
                        Current_GMax = (double)this.color_control.Max;
                        break;
                    case "Percent who perceive echoes (EK)":
                        Current_EMin = (double)this.color_control.Min;
                        Current_EMax = (double)this.color_control.Max;
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Parameter selection invalid...");
                        break;
                }

                if (WC != null) WC.SetColorScale(color_control.Scale, new double[2] { color_control.Min, color_control.Max });
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
                Eto.Forms.SaveFileDialog of = new Eto.Forms.SaveFileDialog();
                of.Filters.Add("Pachyderm Mapping Data File (*.pachm)|*.pachm");
                of.Filters.Add("All Files|");
                of.CurrentFilterIndex = 0;
                if (of.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    if (Map != null)
                    {
                        Utilities.FileIO.Write_pachm(of.FileName, Map);
                    }
                }
            }

            private void OpenDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Eto.Forms.OpenFileDialog of = new Eto.Forms.OpenFileDialog();
                of.Filters.Add("Pachyderm Mapping Data File (*.pachm)|*.pachm");
                of.Filters.Add("All Files|");
                of.CurrentFilterIndex = 0;

                if (of.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    PachMapReceiver[] RT_IN = new PachMapReceiver[0];
                    if (!Utilities.FileIO.Read_pachm(of.FileName,ref RT_IN)) return;
                    //foreach (PachMapReceiver p in RT_IN) p.Correction(80000);

                    Map = RT_IN;
                    SourceList.Clear();
                    //for (int i = 0; i < Map.Length; i++)
                    //{
                    SourceList.Populate(null, null, Map);
                    //SourceList.Items.Add(String.Format("S{0}-", i) + Map[i].SrcType);
                    //}

                    WC = new WaveConduit(color_control.Scale, new double[] { Current_SPLMin, Current_SPLMax });
                }
            }

            private void Parameter_Selection_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (!(sender is NumericStepper))
                {
                    switch (Parameter_Selection.SelectedKey)
                    {
                        case "Sound Pressure Level":
                            color_control.setscale(Current_SPLMin, Current_SPLMax);
                            break;
                        case "Sound Pressure Level (A-weighted)":
                            color_control.setscale(Current_SPLAMin, Current_SPLAMax);
                            break;
                        case "Early Decay Time (T-15)":
                            color_control.setscale(Current_EDTMin, Current_EDTMax);
                            break;
                        case "Reverberation Time (T-15)":
                            color_control.setscale(Current_RTMin, Current_RTMax);
                            break;
                        case "Reverberation Time (T-30)":
                            color_control.setscale(Current_RTMin, Current_RTMax);
                            break;
                        case "Speech Transmission Index - 2003":
                            color_control.setscale(Current_STI1Min, Current_STI1Max);
                            break;
                        case "Speech Transmission Index - Male":
                            color_control.setscale(Current_STI2Min, Current_STI2Max);
                            break;
                        case "Speech Transmission Index - Female":
                            color_control.setscale(Current_STI3Min, Current_STI3Max);
                            break;
                        case "Clarity (C-80)":
                            color_control.setscale(Current_CMin, Current_CMax);
                            break;
                        case "Definition (D-50)":
                            color_control.setscale(Current_DMin, Current_DMax);
                            break;
                        case "Strength/Loudness (G)":
                            color_control.setscale(Current_GMin, Current_GMax);
                            break;
                        case "Percent who perceive echoes (EK)":
                            color_control.setscale(Current_EMin, Current_EMax);
                            break;
                        default:
                            Rhino.RhinoApp.WriteLine("Whoops... Parameter selection invalid...");
                            break;
                    }
                }

                Commit_Param_Bounds();
            }

            int SampleRate = 1000;

            private void Update_Graph(object sender, EventArgs e)
            {
                Analysis_View.Plot.Clear();

                int REC_ID = (int)Receiver_Selection.Value;
                try
                {
                    if (Map != null)
                    {
                        int SampleRate = Map[0].SampleRate;
                    }

                    int OCT_ID = color_control.Freq_Band_ID;
                    Analysis_View.Plot.Title("Logarithmic Energy Time Curve", 12);
                    Analysis_View.Plot.XLabel("Time (seconds)", 12);
                    Analysis_View.Plot.YLabel("Sound Pressure Level (dB)",12);
 
                    if (Map == null) return;

                    List<int> SrcIDs = SourceList.SelectedSources();

                    double[] Filter;
                    double[] Schroeder;
                    int zero_sample = 0;
                    switch (Graph_Type.SelectedIndex)
                    {
                        case 0:
                            Filter = IR_Construction.ETCurve(null, null, Map, (double)CO_TIME.Value, SampleRate, Octave.SelectedIndex, REC_ID, SrcIDs, false);
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        //case "Pressure Time Curve":
                        //    zero_sample = 4096 / 2;
                        //    Filter2 = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false);
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
                        //    Filter2 = IR_Construction.Auralization_Filter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1];
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
                        //    Filter2 = IR_Construction.Auralization_Filter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[2];
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
                        //    Filter2 = IR_Construction.Auralization_Filter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[0];
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
                    
                    if (Normalize_Graph.Checked.Value)
                    {
                        Filter = Utilities.AcousticalMath.Normalize_Function(Filter);
                        Schroeder = Utilities.AcousticalMath.Normalize_Function(Schroeder);
                    }

                    double[] time = new double[Filter.Length];
                    for (int i = 0; i < Filter.Length; i++)
                    {
                        time[i] = (double)(i - zero_sample) / SampleRate;
                    }

                    Analysis_View.Plot.Add.Signal(Schroeder, 1.0/1000.0, ScottPlot.Color.FromSKColor(SkiaSharp.SKColors.Red));
                    Analysis_View.Plot.Add.Signal(Filter, 1.0/1000.0, ScottPlot.Color.FromSKColor(SkiaSharp.SKColors.Blue));

                    if (!LockUserScale.Checked.Value)
                    {
                        Analysis_View.Plot.Axes.SetLimitsX(time[0], time[time.Length - 1]);

                        if (Normalize_Graph.Checked.Value)
                        {
                            Analysis_View.Plot.Axes.SetLimitsY(-100,0);
                        }
                        else
                        {
                            Analysis_View.Plot.Axes.SetLimitsY(0, DirectMagnitude+15);
                        }
                    }
                    else
                    {
                        double max = Analysis_View.Plot.Axes.Left.Max;
                        double min = Analysis_View.Plot.Axes.Left.Min;

                        if (Normalize_Graph.Checked.Value)
                        {
                            Analysis_View.Plot.Axes.SetLimitsY(min, max);
                        }
                        else
                        {
                            Analysis_View.Plot.Axes.SetLimitsY(min, max);
                        }
                    }

                    Analysis_View.Refresh();

                    //Hare.Geometry.Vector V = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(float)Alt_Choice.Value, true), -(float)Azi_Choice.Value, 0, true);

                    //if (Receiver_Choic.SelectedIndex > 0) ReceiverConduit.Instance.set_direction(Utilities.RC_PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), new Vector3d(V.x, V.y, V.z));
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

                }
                catch (Exception x)
                {
                    //System.Windows.Forms.MessageBox.Show(x.Message);
                    return;
                }

                //Analysis_View.AxesChanged();
                Analysis_View.Refresh();
                //Update_Parameters();
            }

            System.Threading.Thread T;
            //private delegate void ForCall();
            //ForCall FC;
            //private delegate void T_Call();
            //T_Call TC;

            private void Flip_Toggle_Click(object sender, EventArgs e)
            {
                if (Flip_Toggle.Text == "Flip")
                {
                    stop = false;
                    this.Flip_Toggle.Text = "Pause";
                    System.Threading.ParameterizedThreadStart St = new System.Threading.ParameterizedThreadStart(delegate { Flip_Forward(); });
                    T = new System.Threading.Thread(St);
                    T.Start();
                }
                else 
                {
                    Flip_Toggle.Text = "Flip";
                    stop = true;
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

                double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { t_hi, t_lo }, Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked);
                Eto.Drawing.Color[] c = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, color_control.Scale);
                Mesh Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, c);
                if (WC == null) return;
                WC.Populate(Mesh_Map);
                
                //////////////////////////////
                if (Folder_Status.Text.Length > 0)
                {
                    string number;
                    if (t < 100)
                    {
                        if (t < 10) number = "00" + t.ToString();
                        else number = "0" + t.ToString();
                    }
                    else number = t.ToString();

                    //this.Invoke((MethodInvoker)delegate { Rhino.RhinoApp.RunScript("-ViewCaptureToFile " + Folder_Status.Text + "\\"[0] + "frame" + number + ".jpg Width=1280 Height=720 DrawGrid=No Enter", true); });
                }
                //////////////////////////////
                t++;

                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            bool stop = false;

            private async void Flip_Forward()
            {
                stop = false;   
                do
                {
                    await System.Threading.Tasks.Task.Run(() => Step_Forward());
                    if (stop) break;
                    //System.Threading.Thread.Sleep(100);
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
                double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { t_hi, t_lo }, Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked);
                Eto.Drawing.Color[] c = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, color_control.Scale);
                Mesh Map_Mesh = Utilities.RCPachTools.PlotMesh(Map, c);
                WC.Populate(Map_Mesh);
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Back_Step_Click(object sender, EventArgs e)
            {
                Step_Backward();
            }

            private void Forw_Step_Click(object sender, EventArgs e)
            {
                Step_Forward();
            }

            private void Start_Over_Click(object sender, EventArgs e)
            {
                t = 0;
                double t_lo = t * (double)Step_Select.Value;
                double t_hi = t * (double)Step_Select.Value + (double)Increment.Value;
                Min_Time_out.Text = t_lo.ToString();
                Max_Time_out.Text = t_hi.ToString();
                Step_Forward();
            }

            public bool Simulations_Ready()
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
                if((sender as CheckBox).Checked.Value) { DirectionalToggle.Enabled = false; }
                else { DirectionalToggle.Enabled = true; }
            }

            bool Linear_Phase = false;

            public void Set_Phase_Regime(bool Linear_Phase)
            {
                if (Map == null || Map.Length == 0 || Map[0] == null) return;
                if (Linear_Phase == this.Linear_Phase) return;
                if ((this.Linear_Phase == true && !(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System)) || (this.Linear_Phase == false && !(Audio.Pach_SP.Filter is Audio.Pach_SP.Minimum_Phase_System)))
                {
                    for (int i = 0; i < Map.Length; i++) Map[i].reset_filter();
                    Rhino.RhinoApp.Write("Mapping pressure reset.");
                    this.Linear_Phase = Linear_Phase;
                }
            }

            private void Coherent_CheckedChanged(object sender, EventArgs e)
            {
                if (sender == Coherent)
                {
                    Incoherent.Checked = false;
                    if (Map != null && !Map[0].HasFilter())
                    {
                        DialogResult DR = MessageBox.Show("Pachyderm will now create the pressure impulse responses for your previously calculated intensity responses. This can take awhile, though, depending on how many receivers you have, and how long a cutoff time you chose. Are you sure you would like to do this?", "Pressure Impulse Response Creation", MessageBoxButtons.YesNo);
                        if (DR == DialogResult.Yes)
                        {
                            Linear_Phase = Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System;
                            ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                            VB.Show(Rhino.RhinoDoc.ActiveDoc);
                            for (int i = 0; i < this.Map.Length; i++) this.Map[i].Create_Filter(VB); //this.Map[i].Create_Pressure(Map[i].SWL);
                            VB.Close();
                        }
                    }
                }
                else if (sender == Incoherent)
                {
                    Coherent.Checked = false;
                }
            }

            private Eto.Forms.SelectFolderDialog FileLocation = new SelectFolderDialog();
            private void OpenFolder_Click(object sender, System.EventArgs e)
            {
                if (FileLocation.ShowDialog(this) == DialogResult.Ok)
                {
                    Folder_Status.Text = FileLocation.Directory;
                }
            }


            public class Mapindicator : Rhino.Display.DisplayConduit
            {
                Rhino.Geometry.Point3d PT;

                public void setPoint(Rhino.Geometry.Point3d pt)
                {
                    PT = pt;
                }

                protected override void DrawForeground(DrawEventArgs e)
                {
                    //if (PT == null) return;
                    Rhino.Geometry.Point2d screen_pt = e.Display.Viewport.WorldToClient(PT);                    
                    e.Display.Draw2dRectangle(new System.Drawing.Rectangle((int)screen_pt.X, (int)screen_pt.Y, 5, 5), System.Drawing.Color.Green, 2, System.Drawing.Color.Yellow);
                    return;
                }
            }

            Mapindicator ReceiverPointer = new Mapindicator();

            private void ReceiverSelection_ValueChanged(object sender, EventArgs e)
            {
                if (Map == null || Map.Length < 1)
                {
                    Receiver_Selection.Value = 0;
                    Receiver_Selection.MaxValue = 0;
                    ReceiverPointer.Enabled = false;
                    return;
                }

                Receiver_Selection.MaxValue = Map[0].Rec_List.Length;
                
                List<int> SrcID = SourceList.SelectedSources();

                if (SrcID.Count < 1) return;

                ReceiverPointer.Enabled = true;
                ReceiverPointer.setPoint(Utilities.RCPachTools.HPttoRPt(Map[(int)(SrcID[0])].Origin((int)Receiver_Selection.Value)));
                Update_Graph(sender, e);
            }

            private void Rec_OR_CheckedChanged(object sender, EventArgs e)
            {
                if(sender == this.Rec_Vertex)
                {
                    Rec_Centroid.Checked = false;
                }
                else
                {
                    Rec_Vertex.Checked = false;
                }
            }

            private void Rec_Disp_CheckedChanged(object sender, EventArgs e)
            {
                if (sender == this.Disp_Audience)
                { 
                    this.Disp_Other.Checked = false;
                }
                else
                {
                    this.Disp_Audience.Checked = false;
                }
            }


            private void Convergence_CheckedChanged(object sender, EventArgs e)
            {
                if (sender == Spec_Rays)
                {
                    Minimum_Convergence.Checked = false;
                    DetailedConvergence.Checked = false;
                    RT_Count.Enabled = true;
                    return;
                }
                else if(sender == Minimum_Convergence)
                {
                    Spec_Rays.Checked = false;
                    DetailedConvergence.Checked= false;
                }
                else if(sender == DetailedConvergence)
                {
                    Minimum_Convergence.Checked = false;
                    Spec_Rays.Checked = false;
                }
                
                RT_Count.Enabled = false;
            }

            private void ZeroAtChanged(object sender, EventArgs e)
            {
                if (sender == ZeroAtDirect) { ZeroAtSource.Checked = false; }
                else { ZeroAtDirect.Checked = false;}
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                FileLocation.Dispose();
                FileMenu.Dispose();
                fileToolStripMenuItem.Dispose();
                saveDataToolStripMenuItem.Dispose();
                openDataToolStripMenuItem.Dispose();

                tabsPrime.Dispose();
                //tabCalculation.Dispose();
                //TabAnalysis.Dispose();
                TabOutputModes.Dispose();

                RecDispLbl.Dispose();
                RecOrientLbl.Dispose();
                GeoLbl.Dispose();
                ConvLbl.Dispose();
                Screen_Attenuation.Dispose();
                Rec_Centroid.Dispose();
                Rec_Vertex.Dispose();
                Offset_Mesh.Dispose();
                Sum_Time.Dispose();
                Disp_Other.Dispose();
                Disp_Audience.Dispose();
                DirectionalToggle.Dispose();
                Spec_Rays.Dispose();
                DetailedConvergence.Dispose();
                Minimum_Convergence.Dispose();
                MapIncr.Dispose();
                Increment.Dispose();
                COTime.Dispose();
                CO_TIME.Dispose();
                RayNo.Dispose();
                RT_Count.Dispose();
                Select_Map.Dispose();
                Calculate.Dispose();
                Env_Control.Dispose();

                //tabOutput.Dispose();
                SourceList.Dispose();
                Plot_Values.Dispose();
                groupBox_Time.Dispose();
                IntervalControl.Dispose();
                MTLbl.Dispose();
                Max_Time_out.Dispose();
                Min_Time_out.Dispose();
                MinTLbl.Dispose();
                groupBox_ZeroTime.Dispose();
                ZeroAtSource.Dispose();
                ZeroAtDirect.Dispose();
                groupBox_IRSum.Dispose();
                Coherent.Dispose();
                Incoherent.Dispose();
                Discretize.Dispose();
                Parameter_Selection.Dispose();
                color_control.Dispose();
                End_Time_Control.Dispose();
                Start_Time_Control.Dispose();
                ETlbl.Dispose();
                STlbl.Dispose();
                Calculate_Map.Dispose();

                //tabIRs.Dispose();
                Octave.Dispose();
                OctLbl.Dispose();
                Analysis_View.Dispose();
                Normalize_Graph.Dispose();
                LockUserScale.Dispose();
                Graph_Octave.Dispose();
                Graph_Type.Dispose();
                Receiver_Selection.Dispose();
                RecLbl.Dispose();
                //Auralisation.Dispose();

                //tabTflip.Dispose();
                Integration_select.Dispose();
                IntDmnLbl.Dispose();
                Step_Select.Dispose();
                T_End_select.Dispose();
                ETLbl.Dispose();
                STLbl.Dispose();
                T_Start_Select.Dispose();
                StpLbl.Dispose();
                Flip_Toggle.Dispose();
                Start_Over.Dispose();
                Back_Step.Dispose();
                Forw_Step.Dispose();
                Tick_Select.Dispose();
                TickLbl.Dispose();
                Folder_Status.Dispose();
                OutLbl.Dispose();
                OpenFolder.Dispose();
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