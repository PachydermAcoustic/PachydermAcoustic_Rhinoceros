//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit 
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

using Eto.Drawing;
using Eto.Forms;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.UI;
using Rhino.UI.ObjectProperties;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
            private CheckBox Image_Sources;
            private NumericStepper Image_Order;
            private RadioButton Rec_Centroid;
            private RadioButton Rec_Vertex;
            private CheckBox Offset_Mesh;
            private CheckBox Sum_Time;
            private RadioButton Disp_Other;
            private RadioButton Disp_Audience;
            private CheckBox DirectionalToggle;
            private RadioButton Spec_Rays;
            private RadioButton DetailedConvergence;
            private RadioButton SimulationOctave;
            private RadioButton SimulationThirdOctave;
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
            private Button Plot_Contours;
            private NumericStepper Contour_Count;
            private Label Contour_StepLbl;
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
            private Button Preview_Map;

            private TabPage tabIRs;
            private DropDown Octave;
            private Label OctLbl;
            private ScottPlot.Eto.EtoPlot Analysis_View;
            private CheckBox Normalize_Graph;
            private CheckBox LockUserScale;
            private readonly DropDown distributionParameter = new DropDown();
            private readonly DropDown distributionDirection = new DropDown();
            private readonly CheckBox distributionNormalFit = new CheckBox { Text = "Show normal fit", Checked = true };
            private readonly DynamicLayout distributionControls = new DynamicLayout();
            private readonly Label responseSummary = new Label { Wrap = WrapMode.Word };
            private int previousGraphType = -1;
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

                this.Contour_Count = new NumericStepper();
                this.Contour_StepLbl = new Label();

                this.groupBox_ZeroTime = new GroupBox(); this.ZeroAtSource = new RadioButton();
                this.ZeroAtDirect = new RadioButton();
                this.Preview_Map = new Button();
                this.Calculate_Map = new Button();
                this.Plot_Contours = new Button();
                this.Plot_Values = new Button();
                this.tabIRs = new TabPage();
                this.Analysis_View = new ScottPlot.Eto.EtoPlot();
                this.Normalize_Graph = new CheckBox();
                this.LockUserScale = new CheckBox();
                this.Graph_Type = new DropDown();
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
                SimulationOctave = new RadioButton { Text = "Octave", Checked = true };
                SimulationThirdOctave = new RadioButton(SimulationOctave) { Text = "Third octave" };
                StackLayout bandSelection = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 18,
                    Padding = new Padding(4, 10, 4, 12),
                    Items = { SimulationOctave, SimulationThirdOctave }
                };
                Calc_Layout.AddRow(new GroupBox { Text = "Simulation frequency bands", Content = bandSelection });

                Image_Sources = new CheckBox { Text = "Include image sources", Checked = false };
                Image_Order = new NumericStepper { MinValue = 1, MaxValue = 12, Value = 1, Enabled = false };
                Image_Sources.ToolTip = "Adds deterministic specular reflections. Higher orders can take much longer on dense maps.";
                Image_Sources.CheckedChanged += (sender, args) => Image_Order.Enabled = Image_Sources.Checked == true;
                //DynamicLayout imageSettings = new DynamicLayout { Spacing = new Size(8, 8), Padding = 8 };
                //imageSettings.AddRow(Image_Sources, new Label { Text = "Maximum order" }, Image_Order);

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
                RCT.AddRow(Image_Sources, null, Image_Order);
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
                this.DirectionalToggle.CheckedChanged += (sender, args) => Update_Lateral_Controls();
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
                this.SourceList.Update += (sender, args) => { if (Graph_Type.SelectedIndex != 8) Update_Graph(sender, args); };
                this.SourceList.Update += Update_Map_Preview;
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
                this.Parameter_Selection.Items.Add("Sound Pressure Level (C-weighted)");
                this.Parameter_Selection.Items.Add("Directionality");
                this.Parameter_Selection.Items.Add("Reverberation Time (T-15)");
                this.Parameter_Selection.Items.Add("Reverberation Time (T-30)");
                this.Parameter_Selection.Items.Add("Speech Transmission Index - 2003");
                this.Parameter_Selection.Items.Add("Speech Transmission Index - Male");
                this.Parameter_Selection.Items.Add("Speech Transmission Index - Female");
                this.Parameter_Selection.Items.Add("Early Decay Time (EDT)");
                this.Parameter_Selection.Items.Add("Clarity (C-50)");
                this.Parameter_Selection.Items.Add("Clarity (C-80)");
                this.Parameter_Selection.Items.Add("Definition (D-50)");
                this.Parameter_Selection.Items.Add("Strength/Loudness (G)");
                this.Parameter_Selection.Items.Add("Percent who perceive echoes (EK)");
                this.Parameter_Selection.SelectedIndex = 0;
                this.Parameter_Selection.SelectedIndexChanged += this.Parameter_Selection_SelectedIndexChanged;

                color_control = new Color_Output_Control(true, "Parameter Selection", Parameter_Selection);
                color_control.Update += Parameter_Selection_SelectedIndexChanged;
                color_control.On_Output_Changed += Parameter_Selection_SelectedIndexChanged;
                color_control.Octave.SelectedIndexChanged += Update_Map_Preview;
                OL.AddRow(color_control);

                receiverAltitude.DecimalPlaces = receiverAzimuth.DecimalPlaces = 2;
                receiverAltitude.MinValue = -90;
                receiverAltitude.MaxValue = 90;
                receiverAzimuth.MinValue = 0;
                receiverAzimuth.MaxValue = 360;
                receiverAltitude.Width = receiverAzimuth.Width = 70;
                receiverAltitude.ValueChanged += Update_Map_Preview;
                receiverAzimuth.ValueChanged += Update_Map_Preview;
                faceSourceCenter.Text = "Face average source center";
                receiverAltitude.Enabled = receiverAzimuth.Enabled = faceSourceCenter.Checked != true;
                faceSourceCenter.CheckedChanged += (sender, args) =>
                {
                    Update_Receiver_Direction();
                    receiverAltitude.Enabled = receiverAzimuth.Enabled = faceSourceCenter.Checked != true;
                    Update_Map_Preview(sender, args);
                };
                DynamicLayout receiverDirectionLayout = new DynamicLayout { Spacing = new Size(8, 8) };
                receiverDirectionLayout.AddRow(new Label { Text = "Azimuth" }, receiverAzimuth);
                receiverDirectionLayout.AddRow(new Label { Text = "Altitude" }, receiverAltitude);
                receiverDirectionLayout.AddRow(faceSourceCenter);
                receiverDirection.Text = "Receiver Direction (degrees)";
                receiverDirection.Width = 230;
                receiverDirection.Content = receiverDirectionLayout;
                receiverDirection.Visible = false;
                color_control.ControlCluster.Items.Add(receiverDirection);
                color_control.Height = -1;

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
                this.Start_Time_Control.ValueChanged += Update_Map_Preview;
                this.End_Time_Control.ValueChanged += Update_Map_Preview;
                DL.AddRow(ETlbl, null, End_Time_Control);
                this.IntervalControl.Content = DL;

                this.Contour_Count.MinValue = 2;
                this.Contour_Count.MaxValue = 100;
                this.Contour_Count.Value = 10;
                this.Contour_Count.Width = 80;
                this.Contour_Count.ValueChanged += (s, e) => UpdateContourStepLabel();
                this.Contour_StepLbl.Text = "Step: --";

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


                Output_Times.AddRow(IntervalControl, groupBox_ZeroTime, new Label { Text = "Contours" }, Contour_Count, Contour_StepLbl); OL.AddRow(Output_Times);

                StackLayout Buttons = new StackLayout();
                Buttons.Spacing = 8;
                this.Preview_Map.Text = "Show Map Mesh";
                this.Preview_Map.Width = 200;
                this.Calculate_Map.Text = "Plot Map Mesh";
                this.Calculate_Map.Width = 200;
                this.Plot_Contours.Text = "Plot Contours";
                this.Plot_Contours.Width = 200;
                this.Plot_Values.Text = "Plot Numerical Values";
                this.Plot_Values.Width = 200;
                this.Preview_Map.Click += this.Preview_Map_Click;
                this.Calculate_Map.Click += this.Calculate_Map_Click;
                this.Plot_Contours.Click += this.Plot_Contours_Click;
                this.Plot_Values.Click += this.Plot_Values_Click;
                Buttons.Items.Add(Preview_Map);
                Buttons.Items.Add(Calculate_Map);
                Buttons.Items.Add(Plot_Contours);
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
                this.Graph_Type.Items.Add("Map parameter distribution");
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

                this.Normalize_Graph.Text = "Normalize";
                this.Normalize_Graph.CheckedChanged += Update_Graph;
                this.LockUserScale.Text = "Lock User Scale";
                this.LockUserScale.CheckedChanged += Update_Graph;

                this.Normalize_Graph.Checked = true;
                Receiver_Selection.MinValue = 0;
                Receiver_Selection.ValueChanged += ReceiverSelection_ValueChanged;

                IR_ctrls.AddRow(RecLbl, Receiver_Selection);
                IR_ctrls.AddRow(Graph_Type, Octave);
                IR_ctrls.AddRow(LockUserScale, Normalize_Graph);
                IRL.AddRow(IR_ctrls);

                foreach (var item in Parameter_Selection.Items) distributionParameter.Items.Add(new ListItem { Text = item.Text, Key = item.Key });
                distributionParameter.SelectedIndex = 0;
                distributionDirection.Items.Add("Arrival azimuth");
                distributionDirection.Items.Add("Arrival altitude");
                distributionDirection.SelectedIndex = 0;
                distributionParameter.SelectedIndexChanged += Update_Graph;
                distributionDirection.SelectedIndexChanged += Update_Graph;
                distributionNormalFit.CheckedChanged += Update_Graph;
                distributionControls.Spacing = new Size(8, 8);
                distributionControls.AddRow(new Label { Text = "Map parameter" }, distributionParameter);
                distributionControls.AddRow(distributionDirection, distributionNormalFit);
                distributionControls.Visible = false;
                IRL.AddRow(distributionControls);
                IRL.AddRow(responseSummary);

                GroupBox display = new GroupBox();
                display.Text = "Response / Map Distribution";
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
            double[] deltaSEL;
            double[] velocity_mps;
            PachMapReceiver[] Map;
            WaveConduit WC;
            private bool mapPreviewUpdatePending;
            private readonly NumericStepper receiverAltitude = new NumericStepper();
            private readonly NumericStepper receiverAzimuth = new NumericStepper();
            private readonly CheckBox faceSourceCenter = new CheckBox { Checked = true };
            private readonly GroupBox receiverDirection = new GroupBox();

            public void Toggle_Conduits_On(object sender, System.EventArgs e)
            {
                if (WC != null) WC.Enabled = true;
            }
            public void Toggle_Conduits_Off(object sender, System.EventArgs e)
            {
                if (WC != null) WC.Enabled = false;
            }

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

                if (!p.Source(out Source, SimulationThirdOctave.Checked, 4 / (Increment.Value * 0.01)) || Rec_Srfs == null)
                {
                    Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                    return;
                }

                foreach (Source source in Source) source.SetBandResolution(SimulationThirdOctave.Checked);

                Hare.Geometry.Point[] SPT;
                p.SourceOrigin(out SPT);
                Calculate.Enabled = false;
                List<Hare.Geometry.Point> P = new List<Hare.Geometry.Point>();
                P.AddRange(SPT);

                Polygon_Scene PScene = Utilities.RCPachTools.Get_Poly_Scene(Env_Control.RelHumidity, false, Env_Control.Temp_Celsius, Env_Control.StaticPressure_hPa, Env_Control.Atten_Method.SelectedIndex, Env_Control.Edge_Frequency);

                if (PScene == null)
                {
                    Rhino.RhinoApp.WriteLine("No valid geometry found in the document. Please ensure at least one Brep, Surface, or Extrusion is visible.");
                    Calculate.Enabled = true;
                    return;
                }

                if (!PScene.Complete)
                {
                    Calculate.Enabled = true;
                    return;
                }

                WC = new WaveConduit(color_control.Scale, new double[] { Current_SPLMin, Current_SPLMax });
                this.UnLoad += Toggle_Conduits_Off;
                this.Load += Toggle_Conduits_On;

                PScene.partition(P);
                Scene Flex_Scene;
                if (PachydermAc_PlugIn.Instance.Geometry_Spec == 0)
                {
                    RhCommon_Scene NScene = Utilities.RCPachTools.GetNURBSScene(Env_Control.RelHumidity, Env_Control.Temp_Celsius, Env_Control.StaticPressure_hPa, Env_Control.Atten_Method.SelectedIndex, Env_Control.Edge_Frequency);
                    if (NScene == null || !NScene.Complete)
                    {
                        Calculate.Enabled = true;
                        return;
                    }
                    NScene.partition(P, Pach_Properties.Instance.Spatial_Depth, Pach_Properties.Instance.Max_Polys_Per_Node);
                    Flex_Scene = NScene;
                }
                else
                {
                    Flex_Scene = PScene;
                }

                Map = new PachMapReceiver[Source.Length];
                int imageOrder = Image_Sources.Checked == true ? (int)Image_Order.Value : 0;
                for (int i = 0; i < Source.Length; i++)
                {
                    Source[i].AppendPts(ref P);
                    Mesh Map_Mesh = Utilities.RCPachTools.Create_Map_Mesh(Rec_Srfs, Utilities.RCPachTools.MetersToModel((double)Increment.Value * 0.01));
                    Map_Mesh.Vertices.UseDoublePrecisionVertices = true;
                    Map_Mesh.Transform(Transform.Scale(Point3d.Origin, Utilities.RCPachTools.ModelToMetersScale));
                    Map[i] = new PachMapReceiver(Utilities.RCPachTools.RhinotoHareMesh(Map_Mesh), Source[i], 1000, (double)Increment.Value * 0.01, Flex_Scene, (double)CO_TIME.Value, Sum_Time.Checked.Value, Disp_Audience.Checked, DirectionalToggle.Checked.Value, Rec_Vertex.Checked, Offset_Mesh.Checked.Value);
                }

                deltaSEL = new double[Source.Length];
                velocity_mps = new double[Source.Length];

                for (int s_id = 0; s_id < Source.Length; s_id++)
                {
                    //double T = 3600.0;
                    //double SEL = AcousticalMath.Est_SEL_from_Leq(Leq_dB, N_events, T);
                    //Lmax_from_SEL(SEL, r0_m, v_mps);

                    Coherent_Source_Waveforms sourceWaveforms = null;
                    try
                    {
                        if (Source[s_id] is SourceCluster cluster && cluster.CoherentSpeakers)
                            sourceWaveforms = await Task.Run(() => new Coherent_Source_Waveforms(cluster, Map[s_id].Third_Oct));
                    }
                    catch (Exception error)
                    {
                        Rhino.RhinoApp.WriteLine("Preparing coherent source waveforms failed: {0}", error);
                        Calculate.Enabled = true;
                        return;
                    }
                    var sourceWaveformLifetime = sourceWaveforms;
                    Direct_Sound DS = new Direct_Sound(Source[s_id], Map[s_id], PScene, Enumerable.Range(0, Map[s_id].Third_Oct ? 24 : 8).ToArray(), this.Screen_Attenuation.Checked.Value, this.Sum_Time.Checked.Value) { SourceWaveforms = sourceWaveforms };
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

                    ImageSourceData images = null;
                    if (imageOrder > 0)
                    {
                        try
                        {
                            images = new ImageSourceData(Source[s_id], Map[s_id], Direct_Data, PScene, new int[] { 0, Map[s_id].Third_Oct ? 23 : 7 }, imageOrder, false, s_id) { SourceWaveforms = sourceWaveforms };
                            images = await RCPachTools.RunSimulation(images) as ImageSourceData;
                        }
                        catch (Exception error)
                        {
                            Rhino.RhinoApp.WriteLine("Mapping image-source calculation failed: {0}", error);
                            Array.Resize(ref Source, s_id);
                            Array.Resize(ref Map, s_id);
                            Calculate.Enabled = true;
                            return;
                        }
                        if (images == null)
                        {
                            Array.Resize(ref Source, s_id);
                            Array.Resize(ref Map, s_id);
                            Calculate.Enabled = true;
                            return;
                        }
                        for (int receiver = 0; receiver < Map[s_id].Count; receiver++)
                            ((PachMapReceiver.Map_Receiver)Map[s_id].Rec_List[receiver]).Direct_Time = Direct_Data.Time(receiver);
                    }

                    ConvergenceProgress CP = new ConvergenceProgress(new System.Threading.CancellationTokenSource(), 1000);
                    if (!Spec_Rays.Checked) CP.Show(); //Rhino.UI.Panels.OpenPanel(new Guid("79B97A26-CEBC-4FA8-8275-9D961ADF1772"));//new System.Threading.Thread(() => {  }).Start();

                    SplitRayTracer RT = new SplitRayTracer(Source[s_id], Map[s_id], Flex_Scene, CutOffLength(), new int[2] { 0, Map[s_id].Third_Oct ? 23 : 7 }, imageOrder, Minimum_Convergence.Checked ? -1 : DetailedConvergence.Checked ? 0 : (int)RT_Count.Value, CP);

                    if (!Spec_Rays.Checked) foreach (SplitRayTracer.Convergence_Check c in RT.Convergence_Report) if (c != null) c.On_Convergence_Check += CP.Fill;
                    TaskAwaiter<Simulation_Type> RTA = RCPachTools.RunSimulation(RT).GetAwaiter();
                    while (!RTA.IsCompleted) await Task.Delay(1500);
                    RT = RTA.GetResult() as SplitRayTracer;

                    if (RT != null)
                    {
                        Rhino.RhinoApp.WriteLine(string.Format("{0} Rays ({1} sub-rays) cast in {2} hours, {3} minutes, {4} seconds.", RT._currentRay.Sum(), RT._rayTotal.Sum(), RT._ts.Hours, RT._ts.Minutes, RT._ts.Seconds));
                        Rhino.RhinoApp.WriteLine("Percentage of energy lost: {0}%", RT.PercentLost);
                        Map[s_id] = (PachMapReceiver)RT.GetReceiver;
                    }
                    else
                    {
                        Array.Resize(ref Source, s_id);
                        Array.Resize(ref Map, s_id);
                        Calculate.Enabled = true;
                        break;
                    }
                    if (images == null) Map[s_id].AddDirect(Direct_Data, Source[s_id]);
                    else
                    {
                        ProgressBox progress = new ProgressBox("Adding direct sound and image sources to the map...");
                        progress.Show();
                        try
                        {
                            await Task.Run(() => Map[s_id].AddDirect(Direct_Data, Source[s_id], images));
                        }
                        catch (Exception error)
                        {
                            Rhino.RhinoApp.WriteLine("Mapping image-source assembly failed: {0}", error);
                            Array.Resize(ref Source, s_id);
                            Array.Resize(ref Map, s_id);
                            Calculate.Enabled = true;
                            return;
                        }
                        finally { progress.Close(); }
                    }
                    Source[s_id].Lighten();

                    // Capture SEL and Lmax deltas for FHWA traffic line sources (hourly Leq → SEL)
                    try
                    {
                        var srcGuid = Pachyderm_Acoustic.UI.SourceConduit.Instance.UUID[s_id];
                        var rhObj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(srcGuid);
                        var sType = rhObj?.Geometry?.GetUserString("SourceType") ?? string.Empty;

                        if (Source[s_id] is Pachyderm_Acoustic.Environment.LineSource &&
                        sType.StartsWith("Traffic (FHWA)", StringComparison.Ordinal))
                        {
                            int automobiles = (int)double.Parse(rhObj.Geometry.GetUserString("Automobiles"));
                            int mediumTrucks = (int)double.Parse(rhObj.Geometry.GetUserString("Medium_Trucks"));
                            int heavyTrucks = (int)double.Parse(rhObj.Geometry.GetUserString("Heavy_Trucks"));
                            int buses = (int)double.Parse(rhObj.Geometry.GetUserString("Buses"));
                            int motorcycles = (int)double.Parse(rhObj.Geometry.GetUserString("Motorcycles"));
                            int veh = automobiles + mediumTrucks + heavyTrucks + buses + motorcycles;
                            velocity_mps[s_id] = (double.Parse(rhObj.Geometry.GetUserString("Velocity")) * 1000 / 3600);

                            deltaSEL[s_id] = AcousticalMath.Est_SEL_from_Leq(0, veh, 3600);
                        }
                    }
                    catch (Exception ex)
                    {
                        Rhino.RhinoApp.WriteLine($"FHWA delta capture error (source {s_id}): {ex.Message}");
                    }

                }

                if (Map == null || Map.Length == 0)
                {
                    Receiver_Selection.MaxValue = 0;
                    Calculate.Enabled = true;
                    return;
                }

                if (Source != null)
                {
                    UpdateResultBands();
                    SourceList.Populate(null, null, Map);

                    if (SavePath != null) Utilities.FileIO.Write_pachm(SavePath, Map);
                }

                Create_Map(false);
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
                Rhino.Commands.Result rc = Rhino.Input.RhinoGet.GetMultipleObjects("Select Mapping Surfaces", false, Rhino.DocObjects.ObjectType.Brep, out refs);
                if (rc != Rhino.Commands.Result.Success || refs == null || refs.Length == 0) return;

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
            double Current_SPLCMin = 0;
            double Current_SPLCMax = 120;
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
            double Current_C50Min = -5, Current_C50Max = 15;
            double Current_LFMin = 0, Current_LFMax = 100;
            double Current_LEMin = 0, Current_LEMax = 100;
            double Current_GMin = -5;
            double Current_GMax = 15;
            double Current_RTMin = 0;
            double Current_RTMax = 3;
            double Current_EDTMax = 3;
            double Current_EDTMin = 0;
            double Current_EMax = 50;
            double Current_EMin = 0;
            //Pach_Graphics.colorscale c_scale;

            private double[] Map_Parameter_Values(string metric, int band, List<int> srcIds, out double min, out double max)
            {
                min = max = 0;
                double[] values = null;
                switch (metric)
                {
                    case "Sound Pressure Level":
                        values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, band, srcIds, Coherent.Checked, ZeroAtDirect.Checked);
                        min = Current_SPLMin; max = Current_SPLMax;
                        break;
                    case "Sound Pressure Level (A-weighted)":
                        values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, srcIds, Coherent.Checked);
                        min = Current_SPLAMin; max = Current_SPLAMax;
                        break;
                    case "Sound Pressure Level (C-weighted)":
                        values = PachMapReceiver.Get_SPLC_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, srcIds, Coherent.Checked);
                        min = Current_SPLCMin; max = Current_SPLCMax;
                        break;
                    case "Sound Exposure Level":
                        values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, band, srcIds, Coherent.Checked, ZeroAtDirect.Checked, deltaSEL);
                        min = Current_SPLMin; max = Current_SPLMax;
                        break;
                    case "Sound Exposure Level (A-weighted)":
                        values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, srcIds, Coherent.Checked, deltaSEL);
                        min = Current_SPLAMin; max = Current_SPLAMax;
                        break;
                    case "Reverberation Time (T-15)":
                        values = PachMapReceiver.Get_RT_Map(Map, 15, band, srcIds, Coherent.Checked);
                        min = Current_RTMin; max = Current_RTMax;
                        break;
                    case "Reverberation Time (T-30)":
                        values = PachMapReceiver.Get_RT_Map(Map, 30, band, srcIds, Coherent.Checked);
                        min = Current_RTMin; max = Current_RTMax;
                        break;
                    case "Early Decay Time (EDT)":
                        values = PachMapReceiver.Get_EDT_Map(Map, band, srcIds, Coherent.Checked);
                        min = Current_EDTMin; max = Current_EDTMax;
                        break;
                    case "Clarity (C-50)":
                        values = PachMapReceiver.Get_Clarity_Map(Map, 50, band, srcIds, Coherent.Checked);
                        min = Current_C50Min; max = Current_C50Max;
                        break;
                    case "Clarity (C-80)":
                        values = PachMapReceiver.Get_Clarity_Map(Map, 80, band, srcIds, Coherent.Checked);
                        min = Current_CMin; max = Current_CMax;
                        break;
                    case "Lateral Fraction (LF)":
                        values = PachMapReceiver.Get_Lateral_Map(Map, band, srcIds, Coherent.Checked, true, receiverAltitude.Value, receiverAzimuth.Value, faceSourceCenter.Checked.Value);
                        min = Current_LFMin; max = Current_LFMax;
                        break;
                    case "Lateral Efficiency (LE)":
                        values = PachMapReceiver.Get_Lateral_Map(Map, band, srcIds, Coherent.Checked, false, receiverAltitude.Value, receiverAzimuth.Value, faceSourceCenter.Checked.Value);
                        min = Current_LEMin; max = Current_LEMax;
                        break;
                    case "Definition (D-50)":
                        values = PachMapReceiver.Get_Definition_Map(Map, band, srcIds, Coherent.Checked);
                        min = Current_DMin; max = Current_DMax;
                        break;
                    case "Strength/Loudness (G)":
                        if (srcIds.Count != 1)
                        {
                            Rhino.RhinoApp.WriteLine("G requires a single source selection.");
                            return null;
                        }
                        int gSrc = srcIds[0];
                        int oct = band;
                        double SWL = 0;
                        if (oct < Map[0].SWL.Length)
                        {
                            SWL = Map[gSrc].SWL[oct];
                        }
                        else
                        {
                            if (Map[gSrc].SWL == null) Map[gSrc].SWL = new double[8] { 120, 120, 120, 120, 120, 120, 120, 120 };
                            for (int i = 0; i < Map[0].SWL.Length; i++) SWL += Math.Pow(10, Map[gSrc].SWL[i] / 10);
                            SWL = 10 * Math.Log10(SWL);
                        }
                        values = PachMapReceiver.Get_G_Map(Map, oct, SWL, gSrc, Coherent.Checked);
                        min = Current_GMin; max = Current_GMax;
                        break;
                    case "Percent who perceive echoes (EK)":
                        values = PachMapReceiver.Get_EchoCritPercent_Map(Map, band, srcIds);
                        if (values == null)
                        {
                            Rhino.RhinoApp.WriteLine("EK requires a single receiver bank.");
                            return null;
                        }
                        min = Current_EMin; max = Current_EMax;
                        break;
                    case "Speech Transmission Index - 2003":
                    case "Speech Transmission Index - Male":
                    case "Speech Transmission Index - Female":
                        string noiseText = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                        if (string.IsNullOrWhiteSpace(noiseText))
                        {
                            this.Enabled = false;
                            try { Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false); }
                            finally { this.Enabled = true; }
                            noiseText = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                        }
                        if (string.IsNullOrWhiteSpace(noiseText)) return null;
                        double[] noise = noiseText.Split(',').Select(double.Parse).ToArray();
                        int type = metric == "Speech Transmission Index - Male" ? 1 : metric == "Speech Transmission Index - Female" ? 2 : 0;
                        values = PachMapReceiver.Get_STI_Map(Map, noise, srcIds, type, Coherent.Checked);
                        min = type == 1 ? Current_STI2Min : type == 2 ? Current_STI3Min : Current_STI1Min;
                        max = type == 1 ? Current_STI2Max : type == 2 ? Current_STI3Max : Current_STI1Max;
                        break;
                    case "Directionality":
                        if (srcIds.Any(source => !Map[source].Directional)) return null;
                        Hare.Geometry.Vector[] directions = PachMapReceiver.Get_Directional_Map(Map, new double[] { Start_Time_Control.Value, End_Time_Control.Value }, band, srcIds);
                        values = new double[directions.Length];
                        for (int receiver = 0; receiver < directions.Length; receiver++)
                        {
                            if (directions[receiver].Length() < 1E-12) { values[receiver] = double.NaN; continue; }
                            double altitude, azimuth;
                            PachTools.World_Angles(new Hare.Geometry.Point(-directions[receiver].dx, -directions[receiver].dy, -directions[receiver].dz), new Hare.Geometry.Point(), true, out altitude, out azimuth);
                            values[receiver] = distributionDirection.SelectedIndex == 1 ? altitude : azimuth;
                        }
                        min = distributionDirection.SelectedIndex == 1 ? -90 : 0;
                        max = distributionDirection.SelectedIndex == 1 ? 90 : 360;
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Unsupported map parameter.");
                        return null;
                }
                return values;
            }

            public void Create_Contours()
            {
                if (Map == null)
                {
                    Rhino.RhinoApp.WriteLine("No map data available.");
                    return;
                }

                List<int> srcIds = SourceList.SelectedSources();
                if (srcIds.Count == 0)
                {
                    Rhino.RhinoApp.WriteLine("Select at least one source.");
                    return;
                }

                string metric = Parameter_Selection.SelectedKey as string;
                if (metric == "Directionality")
                {
                    Rhino.RhinoApp.WriteLine("Directionality is vector data; contours are not supported.");
                    return;
                }
                double min, max;
                double[] values = Map_Parameter_Values(metric, color_control.Octave.SelectedIndex, srcIds, out min, out max);

                if (values == null || values.Length == 0)
                {
                    Rhino.RhinoApp.WriteLine("No values to contour.");
                    return;
                }

                if (max <= min)
                {
                    Rhino.RhinoApp.WriteLine("Invalid contour range.");
                    return;
                }

                if (max <= min)
                {
                    Rhino.RhinoApp.WriteLine("Invalid contour range.");
                    return;
                }

                int nLevels = Math.Max(2, (int)Contour_Count.Value);
                double step = (max - min) / (nLevels - 1);
                double[] levels = new double[nLevels];
                for (int i = 0; i < nLevels; i++) levels[i] = min + (step * i);

                int added = 0;
                for (int li = 0; li < levels.Length; li++)
                {
                    double level = levels[li];
                    var contours = PachMapReceiver.BuildContours(Map, values, new double[] { level });
                    if (contours == null || contours.Count == 0) continue;

                    foreach (var poly in contours)
                    {
                        if (poly == null || poly.Count < 2) continue;

                        Rhino.Geometry.Polyline pline = new Rhino.Geometry.Polyline(poly.Count);
                        double cx = 0, cy = 0, cz = 0;
                        foreach (var hp in poly)
                        {
                            pline.Add(Utilities.RCPachTools.HarePointToModel(hp));
                            cx += hp.x; cy += hp.y; cz += hp.z;
                        }

                        if (pline.Count >= 2)
                        {
                            Rhino.RhinoDoc.ActiveDoc.Objects.AddPolyline(pline);
                            double inv = 1.0 / poly.Count;
                            Rhino.Geometry.Point3d center = Utilities.RCPachTools.HarePointToModel(new Hare.Geometry.Point(cx * inv, cy * inv, cz * inv));
                            Rhino.RhinoDoc.ActiveDoc.Objects.AddTextDot($"{level:0.###}", center);
                            added++;
                        }
                    }
                }

                if (added == 0)
                {
                    Rhino.RhinoApp.WriteLine("No contours generated.");
                    return;
                }

                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                Rhino.RhinoApp.WriteLine($"Added {added} contour polylines.");
            }

            public void Show_Conduit_Map(bool PlotNumbers)
            {
                if (Map == null || Map.Length == 0) return;
                if (WC == null) WC = new WaveConduit(color_control.Scale, new double[] { color_control.Min, color_control.Max });
                Commit_Param_Bounds();
                List<int> srclist = SourceList.SelectedSources();

                if (srclist.Count == 0)
                {
                    WC.Populate(new Mesh());
                    return;
                }
                Mesh Mesh_Map = new Mesh();
                switch (Parameter_Selection.SelectedKey as string)
                {
                    case "Sound Pressure Level":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Sound Pressure Level (A-weighted)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLAMin, Current_SPLAMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Sound Pressure Level (C-weighted)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPLC_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLCMin, Current_SPLCMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Sound Exposure Level":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked, deltaSEL);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Sound Exposure Level (A-weighted)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, SourceList.SelectedSources(), Coherent.Checked, deltaSEL);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLAMin, Current_SPLAMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Directionality":
                        break;
                    case "Early Decay Time (EDT)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_EDT_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_EDTMin, Current_EDTMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Reverberation Time (T-15)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_RT_Map(Map, 15, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_RTMin, Current_RTMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Reverberation Time (T-30)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_RT_Map(Map, 30, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_RTMin, Current_RTMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
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
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_STI3Min, Current_STI3Max }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Clarity (C-50)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Clarity_Map(Map, 50, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_C50Min, Current_C50Max }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Clarity (C-80)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Clarity_Map(Map, 80, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_CMin, Current_CMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Lateral Fraction (LF)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Lateral_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, true, receiverAltitude.Value, receiverAzimuth.Value, faceSourceCenter.Checked.Value);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_LFMin, Current_LFMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Lateral Efficiency (LE)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Lateral_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, false, receiverAltitude.Value, receiverAzimuth.Value, faceSourceCenter.Checked.Value);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_LEMin, Current_LEMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Definition (D-50)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Definition_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_DMin, Current_DMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
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
                            if (oct < Map[0].SWL.Length) SWL = Map[SrcID].SWL[oct];
                            else
                            {
                                if (Map[SrcID].SWL == null) Map[SrcID].SWL = new double[8] { 120, 120, 120, 120, 120, 120, 120, 120 };
                                for (int i = 0; i < Map[0].SWL.Length; i++) SWL += Math.Pow(10, Map[SrcID].SWL[i] / 10);
                                SWL = 10 * Math.Log10(SWL);
                            }
                            double[] Values = PachMapReceiver.Get_G_Map(Map, oct, SWL, SrcID, Coherent.Checked);//, G_Ref_Energy[PachTools.OctaveStr2Int(Octave.Text)]
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_GMin, Current_GMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    case "Percent who perceive echoes (EK)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_EchoCritPercent_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources());
                            if (Values == null) return;
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_EMin, Current_EMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                        }
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Parameter selection invalid...");
                        break;
                }

                WC.Populate(Mesh_Map);
            }
            public void Create_Map(bool PlotNumbers)
            {
                WC.Enabled = false;
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
                    case "Sound Pressure Level (C-weighted)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPLC_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLCMin, Current_SPLCMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Sound Exposure Level":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, ZeroAtDirect.Checked, deltaSEL);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLMin, Current_SPLMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);

                        }
                        break;
                    case "Sound Exposure Level (A-weighted)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, SourceList.SelectedSources(), Coherent.Checked, deltaSEL);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLAMin, Current_SPLAMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Max Sound Pressure Level":
                        if (Map != null)
                        {
                            double[] Values = new double[Map[0].Rec_List.Length];
                            foreach (int s_id in srclist)
                            {
                                double[] values = PachMapReceiver.Get_SPL_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, color_control.Octave.SelectedIndex, new List<int> { s_id }, Coherent.Checked,ZeroAtDirect.Checked, deltaSEL);
                                for (int i = 0; i < Values.Length; i++)
                                {
                                    Values[i] += AcousticalMath.Intensity_SPL(Utilities.AcousticalMath.Lmax_from_SEL(Values[i], (Map[s_id].Rec_List[i] as PachMapReceiver.Map_Receiver).Direct_Time / 343, velocity_mps[s_id]));
                                }

                                for (int i = 0; i < Values.Length; i++) Values[i] = AcousticalMath.SPL_Intensity(Values[i]);
                            }
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_SPLAMin, Current_SPLAMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values);
                        }
                        break;
                    case "Max Sound Pressure Level (A-weighted)":
                        if (Map != null)
                        {
                            double[] Values = new double [Map[0].Rec_List.Length];
                            foreach (int s_id in srclist)
                            {
                                double[] values = PachMapReceiver.Get_SPLA_Map(Map, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, new List<int> {s_id}, Coherent.Checked, deltaSEL);
                                for (int i = 0; i < Values.Length; i++)
                                {
                                    Values[i] += AcousticalMath.Intensity_SPL(Utilities.AcousticalMath.Lmax_from_SEL(Values[i], (Map[s_id].Rec_List[i] as PachMapReceiver.Map_Receiver).Direct_Time / 343, velocity_mps[s_id]));
                                }

                                for (int i = 0; i < Values.Length; i++) Values[i] = AcousticalMath.SPL_Intensity(Values[i]);
                            }
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
                    case "Clarity (C-50)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Clarity_Map(Map, 50, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_C50Min, Current_C50Max }, color_control.Scale);
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
                    case "Lateral Fraction (LF)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Lateral_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, true, receiverAltitude.Value, receiverAzimuth.Value, faceSourceCenter.Checked.Value);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_LFMin, Current_LFMax }, color_control.Scale);
                            Mesh_Map = Utilities.RCPachTools.PlotMesh(Map, C);
                            if (Mesh_Map != null) Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(Mesh_Map);
                            if (PlotNumbers) Utilities.RCPachTools.PlotMapValues(Map, Values, 2);
                        }
                        break;
                    case "Lateral Efficiency (LE)":
                        if (Map != null)
                        {
                            double[] Values = PachMapReceiver.Get_Lateral_Map(Map, color_control.Octave.SelectedIndex, SourceList.SelectedSources(), Coherent.Checked, false, receiverAltitude.Value, receiverAzimuth.Value, faceSourceCenter.Checked.Value);
                            Eto.Drawing.Color[] C = PachMapReceiver.SetColors(Values, new double[] { Current_LEMin, Current_LEMax }, color_control.Scale);
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
                            if (oct < Map[0].SWL.Length) SWL = Map[SrcID].SWL[oct];
                            else
                            {
                                if (Map[SrcID].SWL == null) Map[SrcID].SWL = new double[8] { 120, 120, 120, 120, 120, 120, 120, 120 };
                                for (int i = 0; i < Map[0].SWL.Length; i++) SWL += Math.Pow(10, Map[SrcID].SWL[i] / 10);
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
                    case "Sound Pressure Level (C-weighted)":
                        Current_SPLCMin = (double)this.color_control.Min;
                        Current_SPLCMax = (double)this.color_control.Max;
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
                    case "Clarity (C-50)":
                        Current_C50Min = (double)this.color_control.Min;
                        Current_C50Max = (double)this.color_control.Max;
                        break;
                    case "Clarity (C-80)":
                        Current_CMin = (double)this.color_control.Min;
                        Current_CMax = (double)this.color_control.Max;
                        break;
                    case "Lateral Fraction (LF)":
                        Current_LFMin = (double)this.color_control.Min;
                        Current_LFMax = (double)this.color_control.Max;
                        break;
                    case "Lateral Efficiency (LE)":
                        Current_LEMin = (double)this.color_control.Min;
                        Current_LEMax = (double)this.color_control.Max;
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

            private void Preview_Map_Click(object sender, EventArgs e)
            {
                if (WC != null && WC.Enabled) WC.Enabled = false;
                else Show_Conduit_Map(false);
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Update_Map_Preview(object sender, EventArgs e)
            {
                bool refreshGraph = Graph_Type.SelectedIndex == 8 || Graph_Type.SelectedIndex >= 2 &&
                    (sender == receiverAltitude || sender == receiverAzimuth || sender == faceSourceCenter);
                if (Map == null || mapPreviewUpdatePending || !refreshGraph && (WC == null || !WC.Enabled)) return;
                mapPreviewUpdatePending = true;
                Application.Instance.AsyncInvoke(() =>
                {
                    mapPreviewUpdatePending = false;
                    if (refreshGraph) Update_Graph(sender, e);
                    if (WC == null || !WC.Enabled || Map == null || color_control.Octave.SelectedIndex < 0) return;
                    Show_Conduit_Map(false);
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                });
            }

            private void Calculate_Map_Click(object sender, EventArgs e)
            {
                Create_Map(false);
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Plot_Contours_Click(object sender, EventArgs e)
            {
                Create_Contours();
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
                    UpdateResultBands();
                    SourceList.Populate(null, null, Map);
                    //SourceList.Items.Add(String.Format("S{0}-", i) + Map[i].SrcType);
                    //}

                    WC = new WaveConduit(color_control.Scale, new double[] { Current_SPLMin, Current_SPLMax });
                }
            }

            private void Parameter_Selection_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Parameter_Selection.SelectedIndex < 0) return;
                receiverDirection.Visible = Parameter_Selection.SelectedKey == "Lateral Fraction (LF)" || Parameter_Selection.SelectedKey == "Lateral Efficiency (LE)";
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
                        case "Sound Pressure Level (C-weighted)":
                            color_control.setscale(Current_SPLCMin, Current_SPLCMax);
                            break;
                        case "Early Decay Time (EDT)":
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
                        case "Clarity (C-50)":
                            color_control.setscale(Current_C50Min, Current_C50Max);
                            break;
                        case "Clarity (C-80)":
                            color_control.setscale(Current_CMin, Current_CMax);
                            break;
                        case "Lateral Fraction (LF)":
                            color_control.setscale(Current_LFMin, Current_LFMax);
                            break;
                        case "Lateral Efficiency (LE)":
                            color_control.setscale(Current_LEMin, Current_LEMax);
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
                UpdateContourStepLabel();
                Update_Map_Preview(sender, e);
            }

            private void UpdateContourStepLabel()
            {
                double min = double.PositiveInfinity, max = double.NegativeInfinity;
                switch (Parameter_Selection.SelectedKey as string)
                {
                    case "Sound Pressure Level": min = Current_SPLMin; max = Current_SPLMax; break;
                    case "Sound Pressure Level (A-weighted)": min = Current_SPLAMin; max = Current_SPLAMax; break;
                    case "Sound Pressure Level (C-weighted)": min = Current_SPLCMin; max = Current_SPLCMax; break;
                    case "Reverberation Time (T-15)": min = Current_RTMin; max = Current_RTMax; break;
                    case "Reverberation Time (T-30)": min = Current_RTMin; max = Current_RTMax; break;
                    case "Early Decay Time (EDT)": min = Current_EDTMin; max = Current_EDTMax; break;
                    case "Clarity (C-50)": min = Current_C50Min; max = Current_C50Max; break;
                    case "Clarity (C-80)": min = Current_CMin; max = Current_CMax; break;
                    case "Lateral Fraction (LF)": min = Current_LFMin; max = Current_LFMax; break;
                    case "Lateral Efficiency (LE)": min = Current_LEMin; max = Current_LEMax; break;
                    case "Definition (D-50)": min = Current_DMin; max = Current_DMax; break;
                    case "Strength/Loudness (G)": min = Current_GMin; max = Current_GMax; break;
                    case "Percent who perceive echoes (EK)": min = Current_EMin; max = Current_EMax; break;
                    case "Speech Transmission Index - 2003": min = Current_STI1Min; max = Current_STI1Max; break;
                    case "Speech Transmission Index - Male": min = Current_STI2Min; max = Current_STI2Max; break;
                    case "Speech Transmission Index - Female": min = Current_STI3Min; max = Current_STI3Max; break;
                }
                int n = Math.Max(2, (int)Contour_Count.Value);
                double step = (max > min) ? (max - min) / (n - 1) : 0;
                Contour_StepLbl.Text = max > min ? $"Step: {step:0.###}" : "Step: --";
            }

            private void Update_Graph(object sender, EventArgs e)
            {
                if (Analysis_View == null) return;
                Analysis_View.Plot.Clear();
                responseSummary.Text = "";
                bool distribution = Graph_Type.SelectedIndex == 8;
                distributionControls.Visible = distribution;
                Receiver_Selection.Enabled = RecLbl.Enabled = !distribution;
                Normalize_Graph.Enabled = !distribution;
                bool keepScale = LockUserScale.Checked == true && previousGraphType == Graph_Type.SelectedIndex;
                try
                {
                    if (Map == null || Map.Length == 0 || Map[0] == null) { responseSummary.Text = "No map data available."; return; }
                    List<int> sources = SourceList.SelectedSources();
                    if (sources.Count == 0) { responseSummary.Text = "Select at least one source."; return; }
                    int band = Octave.SelectedIndex;
                    if (band < 0) return;
                    if (distribution)
                    {
                        Plot_Map_Distribution(sources, band, keepScale);
                        return;
                    }
                    Octave.Enabled = true;
                    int receiver = (int)Receiver_Selection.Value;
                    if (receiver < 0 || receiver >= Map[0].Rec_List.Length) return;
                    if (sources.Any(source => Map[source].Time1Pt)) { responseSummary.Text = "This result stores summed SPL only; time-resolved responses are unavailable."; return; }
                    bool pressure = Graph_Type.SelectedIndex % 2 == 1;
                    int axis = Graph_Type.SelectedIndex < 2 ? -1 : Graph_Type.SelectedIndex < 4 ? 1 : Graph_Type.SelectedIndex < 6 ? 2 : 0;
                    if (axis >= 0 && sources.Any(source => !Map[source].Directional)) { responseSummary.Text = "This response requires a map calculated with directionality tracking."; return; }
                    int rate = pressure ? 44100 : Map[0].SampleRate;
                    int bands = Map[0].Third_Oct ? 24 : 8;
                    double duration = sources.Max(source => Map[source].SampleCT / (double)Map[source].SampleRate + Map[source].delay_ms / 1000.0);
                    double cutoff = sources.Max(source => Map[source].SampleCT * 1000.0 / Map[source].SampleRate);
                    double altitude = receiverAltitude.Value, azimuth = receiverAzimuth.Value;
                    if (faceSourceCenter.Checked == true && axis >= 0)
                    {
                        Hare.Geometry.Point center = new Hare.Geometry.Point(Map.Average(map => map.Src.x), Map.Average(map => map.Src.y), Map.Average(map => map.Src.z));
                        if ((center - Map[0].Rec_List[receiver].Origin).Length() > 0)
                            PachTools.World_Angles(center, Map[0].Rec_List[receiver].Origin, true, out altitude, out azimuth);
                    }
                    double[] response;
                    if (pressure)
                    {
                        foreach (int source in sources)
                            if (!Map[source].HasFilter(receiver) || axis >= 0 && Map[source].Filter_3Axis(receiver) == null)
                                Map[source].Rec_List[receiver].Create_Filter();
                        response = axis < 0 ? IR_Construction.PressureTimeCurve(null, null, Map, cutoff, rate, receiver, sources, true, true) :
                            IR_Construction.PTC_Fig8_3Axis(null, null, Map, cutoff, rate, receiver, sources, true, altitude, azimuth, true, true)[axis];
                        if (band < bands) response = Map[0].Third_Oct ? Audio.Pach_SP.FIR_Third_Bandpass(response, band, rate, 0) : Audio.Pach_SP.FIR_Bandpass(response, band, rate, 0);
                        Array.Resize(ref response, Math.Min(response.Length, (int)Math.Ceiling(duration * rate) + 8192));
                    }
                    else
                    {
                        response = axis < 0 ? IR_Construction.ETCurve(null, null, Map, cutoff, rate, band, receiver, sources, true) :
                            IR_Construction.ETCurve_1d(null, null, Map, cutoff, rate, band, receiver, sources, true, altitude, azimuth, true)[axis];
                        Array.Resize(ref response, Math.Min(response.Length, (int)Math.Ceiling(duration * rate)));
                        for (int sample = 0; sample < response.Length; sample++) response[sample] = Math.Abs(response[sample]);
                    }
                    if (response.Length == 0) return;
                    Analysis_View.Plot.Title(Graph_Type.SelectedValue + " — " + Octave.SelectedValue, 12);
                    Analysis_View.Plot.XLabel("Time (seconds)", 12);
                    double offset = pressure ? -8192.0 / rate : 0;
                    if (pressure)
                    {
                        double peak = response.Max(value => Math.Abs(value));
                        if (Normalize_Graph.Checked == true && peak > 0)
                            for (int sample = 0; sample < response.Length; sample++) response[sample] /= peak;
                        Analysis_View.Plot.YLabel(Normalize_Graph.Checked == true ? "Relative pressure" : "Pressure (Pa)", 12);
                    }
                    else
                    {
                        double[] schroeder = AcousticalMath.Schroeder_Integral(response);
                        // The integral is normalized; restore its energy before converting to absolute levels.
                        double totalEnergy = response.Sum();
                        for (int sample = 0; sample < schroeder.Length; sample++) schroeder[sample] *= totalEnergy;
                        response = AcousticalMath.SPL_Intensity_Signal(response);
                        schroeder = AcousticalMath.SPL_Intensity_Signal(schroeder);
                        if (Normalize_Graph.Checked == true)
                        {
                            response = AcousticalMath.Normalize_Function(response);
                            schroeder = AcousticalMath.Normalize_Function(schroeder);
                        }
                        Analysis_View.Plot.YLabel(Normalize_Graph.Checked == true ? "Relative level (dB)" : "Sound Pressure Level (dB)", 12);
                        var decay = Analysis_View.Plot.Add.Signal(schroeder, 1.0 / rate, ScottPlot.Colors.Red);
                        decay.LegendText = "Schroeder decay";
                    }
                    var signal = Analysis_View.Plot.Add.Signal(response, 1.0 / rate, ScottPlot.Colors.Blue);
                    signal.Data.XOffset = offset;
                    signal.LegendText = pressure ? "Pressure response" : "Energy response";
                    Analysis_View.Plot.ShowLegend();
                    if (!keepScale)
                    {
                        Analysis_View.Plot.Axes.AutoScale();
                        Analysis_View.Plot.Axes.SetLimitsX(0, Math.Max(1.0 / rate, duration));
                        if (!pressure && Normalize_Graph.Checked == true) Analysis_View.Plot.Axes.SetLimitsY(-100, 0);
                    }
                    responseSummary.Text = $"{rate:N0} samples/s; recorded duration {duration:0.###} s.";
                }
                catch (Exception error)
                {
                    Analysis_View.Plot.Clear();
                    responseSummary.Text = "Unable to display this result: " + error.Message;
                }
                finally
                {
                    previousGraphType = Graph_Type.SelectedIndex;
                    Analysis_View.Refresh();
                }
            }

            private void Plot_Map_Distribution(List<int> sources, int band, bool keepScale)
            {
                string metric = distributionParameter.SelectedKey;
                if (string.IsNullOrEmpty(metric)) return;
                bool angular = metric == "Directionality";
                bool circular = angular && distributionDirection.SelectedIndex == 0;
                bool unitInterval = metric.StartsWith("Speech Transmission");
                bool percentage = metric == "Definition (D-50)" || metric == "Lateral Fraction (LF)" || metric == "Lateral Efficiency (LE)" || metric == "Percent who perceive echoes (EK)";
                double domainMax = unitInterval ? 1 : percentage ? 100 : double.PositiveInfinity;
                distributionDirection.Visible = angular;
                distributionNormalFit.Enabled = !circular;
                Octave.Enabled = !metric.Contains("weighted") && !metric.StartsWith("Speech Transmission");
                if (sources.Any(source => Map[source].Time1Pt) && !(metric.StartsWith("Sound Pressure") || angular))
                { responseSummary.Text = "This parameter requires time-resolved map data."; return; }
                double lower, upper;
                double[] values = Map_Parameter_Values(metric, band, sources, out lower, out upper);
                if (values == null || values.Length == 0) { responseSummary.Text = "This parameter is unavailable for the selected map or source selection."; return; }
                var mesh = Map[0].MapMesh();
                double[] area = new double[values.Length];
                for (int face = 0; face < mesh.Polys.Count; face++)
                {
                    double faceArea = mesh.Polygon_Area(face);
                    if (Map[0].Rec_Vertex)
                    {
                        var polygon = mesh.Polys[face];
                        for (int vertex = 0; vertex < polygon.VertextCT; vertex++) area[polygon.Points[vertex].index] += faceArea / polygon.VertextCT;
                    }
                    else area[face] = faceArea;
                }
                double totalArea = area.Sum();
                int[] valid = Enumerable.Range(0, values.Length).Where(index => area[index] > 0 && !double.IsNaN(values[index]) && !double.IsInfinity(values[index]) &&
                    (!(unitInterval || percentage) || values[index] >= 0 && values[index] <= domainMax)).OrderBy(index => values[index]).ToArray();
                if (totalArea <= 0 || valid.Length == 0) { responseSummary.Text = "No finite parameter values with mapped surface area."; return; }
                double validArea = valid.Sum(index => area[index]);
                double coverage = 100 * validArea / totalArea;
                double mean = valid.Sum(index => values[index] * area[index]) / validArea;
                double deviation = Math.Sqrt(valid.Sum(index => area[index] * Math.Pow(values[index] - mean, 2)) / validArea);
                double cumulative = 0, median = values[valid[valid.Length - 1]];
                foreach (int index in valid)
                {
                    cumulative += area[index];
                    if (cumulative >= validArea / 2) { median = values[index]; break; }
                }
                double min = values[valid[0]], max = values[valid[valid.Length - 1]];
                int bins = max > min ? Math.Min(50, Math.Max(8, (int)Math.Ceiling(Math.Sqrt(valid.Length)))) : 1;
                double width = max > min ? (max - min) / bins : Math.Max(1, Math.Abs(min) * .01);
                double first = max > min ? min : min - width / 2;
                if (unitInterval || percentage)
                {
                    bins = Math.Min(50, Math.Max(8, (int)Math.Ceiling(Math.Sqrt(valid.Length))));
                    first = 0;
                    width = domainMax / bins;
                }
                if (circular) { bins = 36; width = 10; first = 0; }
                double[] percentages = new double[bins];
                foreach (int index in valid)
                {
                    int bin = Math.Min(bins - 1, Math.Max(0, (int)((values[index] - first) / width)));
                    percentages[bin] += 100 * area[index] / totalArea;
                }
                var bars = Enumerable.Range(0, bins).Select(bin => new ScottPlot.Bar { Position = first + (bin + .5) * width, Value = percentages[bin], Size = width * .95, FillColor = ScottPlot.Colors.LightBlue }).ToArray();
                Analysis_View.Plot.Add.Bars(bars).LegendText = "Mapped area";
                double[] centers = Enumerable.Range(0, bins).Select(bin => first + (bin + .5) * width).ToArray();
                if (bins > 1)
                {
                    var curve = Analysis_View.Plot.Add.ScatterLine(centers, percentages, ScottPlot.Colors.Blue);
                    curve.LegendText = "Area distribution";
                }
                if (!circular && distributionNormalFit.Checked == true && max > min && deviation > 0)
                {
                    double[] x = Enumerable.Range(0, 201).Select(sample => first + sample * bins * width / 200).ToArray();
                    double[] fit = x.Select(value => coverage * width / (deviation * Math.Sqrt(2 * Math.PI)) * Math.Exp(-.5 * Math.Pow((value - mean) / deviation, 2))).ToArray();
                    Analysis_View.Plot.Add.ScatterLine(x, fit, ScottPlot.Colors.Red).LegendText = "Normal fit";
                }
                string unit = angular ? "degrees" : metric.Contains("Time") || metric.StartsWith("Early Decay") ? "s" : metric.StartsWith("Clarity") || metric.StartsWith("Sound") || metric.StartsWith("Strength") ? "dB" : metric.StartsWith("Speech") ? "" : "%";
                Analysis_View.Plot.Title(metric + " — " + (angular ? distributionDirection.SelectedValue : Octave.Enabled ? Octave.SelectedValue : "All bands"), 12);
                Analysis_View.Plot.XLabel((angular ? distributionDirection.SelectedValue : metric) + (unit.Length > 0 ? " (" + unit + ")" : ""), 12);
                Analysis_View.Plot.YLabel("Mapped surface area per bin (%)", 12);
                Analysis_View.Plot.ShowLegend();
                if (!keepScale)
                {
                    Analysis_View.Plot.Axes.AutoScale();
                    if (unitInterval || percentage) Analysis_View.Plot.Axes.SetLimitsX(0, domainMax);
                    else if (circular) Analysis_View.Plot.Axes.SetLimitsX(0, 360);
                    Analysis_View.Plot.Axes.SetLimitsY(0, Math.Max(1, Math.Max(Analysis_View.Plot.Axes.Left.Max, percentages.Max() * 1.15)));
                }
                if (circular)
                {
                    double cosine = valid.Sum(index => area[index] * Math.Cos(values[index] * Math.PI / 180));
                    double sine = valid.Sum(index => area[index] * Math.Sin(values[index] * Math.PI / 180));
                    double bearing = Math.Sqrt(cosine * cosine + sine * sine) > validArea * 1E-12 ? (Math.Atan2(sine, cosine) * 180 / Math.PI + 360) % 360 : double.NaN;
                    responseSummary.Text = $"Area-weighted circular mean: {bearing:0.###} degrees. Valid mapped area: {coverage:0.##}%; unavailable: {100 - coverage:0.##}%.";
                }
                else responseSummary.Text = $"Area-weighted mean: {mean:0.###}; median: {median:0.###}; standard deviation: {deviation:0.###} {unit}\nRange: {min:0.###} to {max:0.###} {unit}. Valid mapped area: {coverage:0.##}%; unavailable: {100 - coverage:0.##}%.";
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
                Mesh Map_Mesh = Utilities.RCPachTools.PlotMesh(Map, c);
                if (WC == null) return;
                WC.Populate(Map_Mesh);
                
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

            private void UpdateResultBands()
            {
                if (Map == null || Map.Length == 0 || Map[0] == null) return;
                Receiver_Selection.MaxValue = Math.Max(0, Map[0].Rec_List.Length - 1);
                DirectionalToggle.Checked = Map.All(map => map != null && map.Directional);
                Update_Lateral_Controls();
                double[] centers = Map[0].Third_Oct ? AcousticalMath.ThirdOctaveCenters() : AcousticalMath.OctaveCenters();
                if (Octave.Items.Count != centers.Length + 1)
                {
                    Octave.Items.Clear();
                    foreach (double center in centers) Octave.Items.Add(center.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Hz.");
                    Octave.Items.Add("Summation: All Bands");
                    Octave.SelectedIndex = centers.Length;
                }
                if (color_control.Octave.Items.Count != centers.Length + 1)
                {
                    color_control.Octave.Items.Clear();
                    foreach (double center in centers) color_control.Octave.Items.Add(center.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Hz.");
                    color_control.Octave.Items.Add("Summation: All Bands");
                    color_control.Octave.SelectedIndex = centers.Length;
                }
            }

            private void Update_Receiver_Direction()
            {
                if (Map == null || Map.Length == 0 || Map.Any(map => map == null || map.Src == null) || Map[0].Rec_List == null) return;
                Hare.Geometry.Point center = new Hare.Geometry.Point(Map.Average(map => map.Src.x), Map.Average(map => map.Src.y), Map.Average(map => map.Src.z));
                Hare.Geometry.Vector direction = new Hare.Geometry.Vector();
                foreach (var receiver in Map[0].Rec_List)
                {
                    Hare.Geometry.Vector facing = center - receiver.Origin;
                    if (facing.Length() == 0) continue;
                    facing.Normalize();
                    direction += facing;
                }
                if (direction.Length() < 1E-12) return;
                double altitude, azimuth;
                PachTools.World_Angles(new Hare.Geometry.Point(direction.dx, direction.dy, direction.dz), new Hare.Geometry.Point(), true, out altitude, out azimuth);
                receiverAltitude.Value = altitude;
                receiverAzimuth.Value = azimuth;
            }

            private void Update_Lateral_Controls()
            {
                if (Parameter_Selection == null || color_control == null) return;
                bool available = DirectionalToggle.Checked == true && Sum_Time.Checked != true &&
                    (Map == null || Map.All(map => map != null && map.Directional && !map.Time1Pt && map.SampleCT > 1));
                foreach (string name in new[] { "Lateral Fraction (LF)", "Lateral Efficiency (LE)" })
                {
                    foreach (DropDown selection in new[] { Parameter_Selection, distributionParameter })
                    {
                        var item = selection.Items.FirstOrDefault(value => value.Key == name);
                        if (available && item == null) selection.Items.Add(name);
                        else if (!available && item != null)
                        {
                            if (selection.SelectedKey == name) selection.SelectedIndex = 0;
                            selection.Items.Remove(item);
                        }
                    }
                }
                receiverDirection.Visible = available && (Parameter_Selection.SelectedKey == "Lateral Fraction (LF)" || Parameter_Selection.SelectedKey == "Lateral Efficiency (LE)");
                if (available && faceSourceCenter.Checked == true) Update_Receiver_Direction();
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
                if((sender as CheckBox).Checked.Value) 
                {
                    DirectionalToggle.Enabled = false;
                    Spec_Rays.Checked = true;
                    DetailedConvergence.Checked = false;
                    Minimum_Convergence.Checked = false;
                    DetailedConvergence.Enabled = false;
                    Minimum_Convergence.Enabled = false;

                }
                else 
                {
                    DetailedConvergence.Enabled = true;
                    Minimum_Convergence.Enabled = true;
                    DirectionalToggle.Enabled = true; 
                }
                Update_Lateral_Controls();
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
                Update_Map_Preview(sender, e);
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

                Receiver_Selection.MaxValue = Math.Max(0, Map[0].Rec_List.Length - 1);
                
                List<int> SrcID = SourceList.SelectedSources();

                if (SrcID.Count < 1 || SrcID[0] < 0 || SrcID[0] >= Map.Length) return;

                ReceiverPointer.Enabled = true;
                ReceiverPointer.setPoint(Utilities.RCPachTools.HarePointToModel(Map[(int)(SrcID[0])].Origin((int)Receiver_Selection.Value)));
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
                Update_Map_Preview(sender, e);
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
                receiverDirection.Dispose();
                receiverAltitude.Dispose();
                receiverAzimuth.Dispose();
                faceSourceCenter.Dispose();
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
                distributionParameter.Dispose();
                distributionDirection.Dispose();
                distributionNormalFit.Dispose();
                distributionControls.Dispose();
                responseSummary.Dispose();
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
