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

using System;
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using System.Linq;
using Eto.Forms;
using Rhino.UI;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("7c62fae6-efa7-4c07-af12-cd440049c7fc")]
        public class PachTDNumericControl : Panel, IPanel
        {
            Color_Output_Control scatcolorlayout;
            private Label Freq_Feedback;
            internal Eto.Forms.Button Forw;
            private Eto.Forms.TabControl TabsPrime;
            private Eto.Forms.TabPage EigenTab;
            private Eto.Forms.TabPage ScatTab;
            private Eto.Forms.CheckBox EdgeFreq;
            internal Eto.Forms.Button Preview;
            private Eto.Forms.GroupBox TimeContainer;
            private Eto.Forms.Label Time_Preview;
            internal Eto.Forms.Button PlayContinuous;
            internal Eto.Forms.Button ForwVis;
            internal Eto.Forms.ComboBox Color_Selection;
            private Eto.Forms.NumericStepper Pos_Select;
            internal Eto.Forms.ComboBox AxisSelect;
            private Eto.Forms.ListBox Map_Planes;
            private Eto.Forms.Button AddPlane;
            internal Eto.Forms.NumericStepper Frequency_Selection;
            internal Eto.Forms.NumericStepper Freq_Max;
            internal Eto.Forms.DropDown SourceSelect;
            private Eto.Forms.DropDown Eigen_Extent;
            private Eto.Forms.Button CalculateSim;
            internal Eto.Forms.DropDown Receiver_Choice;
            internal Eto.Forms.NumericStepper CO_TIME;
            private Eto.Forms.Button Export_Signal;
            private ScottPlot.Eto.EtoPlot Frequency_View;
            private ScottPlot.Eto.EtoPlot TransientView;
            private Eto.Forms.ListBox EigenFrequencies;
            internal Eto.Forms.TextBox Folder_Status;
            private Eto.Forms.Button SetFolder;
            private Eto.Forms.TabPage VisTab;
            private Eto.Forms.Button CalculateScattering;
            private Eto.Forms.Button ScatExport;
            private ScottPlot.Eto.EtoPlot ScatteringGraph;
            private Eto.Forms.DropDown Analysis_Technique;
            private Eto.Forms.NumericStepper ScatteringRadius;
            private Eto.Forms.NumericStepper Sample_Depth;
            private Eto.Forms.DropDown ScatLimit;
            private Eto.Forms.Slider Freq_Trackbar1;
            private Eto.Forms.Slider Freq_Trackbar2;
            internal Eto.Forms.DropDown Scat_Param_Select;
            private Eto.Forms.GroupBox DirBox;
            private Eto.Forms.CheckBox Scat_Dir_75;
            private Eto.Forms.CheckBox Scat_Dir_60;
            private Eto.Forms.CheckBox Scat_Dir_45;
            private Eto.Forms.CheckBox Scat_Dir_30;
            private Eto.Forms.CheckBox Scat_Dir_15;
            private Eto.Forms.CheckBox Scat_Dir_00;
            private Eto.Forms.CheckBox GroundPlane;
            private Eto.Forms.Button DeletePlane;
            private Eto.Forms.CheckBox EigenPML;
            private Eto.Forms.CheckBox VisualPML;
            private Medium_Properties_Group Medium;
            private Color_Output_Control viscolor;

            public PachTDNumericControl()
            {
                Instance = this;

                this.Forw = new Eto.Forms.Button();
                this.TabsPrime = new Eto.Forms.TabControl();
                this.EigenTab = new Eto.Forms.TabPage();
                this.Export_Signal = new Eto.Forms.Button();
                this.CO_TIME = new Eto.Forms.NumericStepper();
                this.CalculateSim = new Eto.Forms.Button();
                this.Eigen_Extent = new Eto.Forms.ComboBox();
                this.EigenFrequencies = new Eto.Forms.ListBox();
                this.Receiver_Choice = new Eto.Forms.ComboBox();
                this.EigenPML = new Eto.Forms.CheckBox();
                this.VisTab = new Eto.Forms.TabPage();
                this.DirBox = new Eto.Forms.GroupBox();
                this.Scat_Dir_75 = new Eto.Forms.CheckBox();
                this.Scat_Dir_60 = new Eto.Forms.CheckBox();
                this.Scat_Dir_45 = new Eto.Forms.CheckBox();
                this.Scat_Dir_30 = new Eto.Forms.CheckBox();
                this.Scat_Dir_15 = new Eto.Forms.CheckBox();
                this.Scat_Dir_00 = new Eto.Forms.CheckBox();
                this.ScatExport = new Eto.Forms.Button();
                this.ScatLimit = new Eto.Forms.DropDown();
                this.Analysis_Technique = new Eto.Forms.DropDown();
                this.Sample_Depth = new Eto.Forms.NumericStepper();
                this.ScatteringRadius = new Eto.Forms.NumericStepper();
                this.Freq_Trackbar1 = new Eto.Forms.Slider();
                this.Freq_Trackbar2 = new Eto.Forms.Slider();
                this.Scat_Param_Select = new Eto.Forms.DropDown();
                this.CalculateScattering = new Eto.Forms.Button();
                this.ScatTab = new Eto.Forms.TabPage();
                this.EdgeFreq = new Eto.Forms.CheckBox();
                this.Freq_Max = new Eto.Forms.NumericStepper();
                this.Frequency_Selection = new Eto.Forms.NumericStepper();
                this.SourceSelect = new Eto.Forms.ComboBox();
                this.Preview = new Eto.Forms.Button();
                this.TimeContainer = new Eto.Forms.GroupBox();
                this.Time_Preview = new Eto.Forms.Label();
                this.PlayContinuous = new Eto.Forms.Button();
                this.ForwVis = new Eto.Forms.Button();
                this.AxisSelect = new Eto.Forms.ComboBox();
                this.Map_Planes = new Eto.Forms.ListBox();
                this.AddPlane = new Eto.Forms.Button();
                this.SetFolder = new Eto.Forms.Button();
                this.Folder_Status = new Eto.Forms.TextBox();
                this.GroundPlane = new Eto.Forms.CheckBox();
                this.Pos_Select = new Eto.Forms.NumericStepper();
                this.DeletePlane = new Eto.Forms.Button();
                this.VisualPML = new Eto.Forms.CheckBox();

                ////Eigen values interface
                this.TabsPrime.Pages.Add(this.EigenTab);
                this.EigenTab.Text = "Eigen Values";

                DynamicLayout DLE1 = new DynamicLayout();
                Label label2 = new Label();
                label2.Text = "Calculate up to:";
                this.Eigen_Extent.Items.Add("63 Hz. Octave Band");
                this.Eigen_Extent.Items.Add("125 Hz. Octave Band");
                this.Eigen_Extent.Items.Add("250 Hz. Octave Band");
                this.Eigen_Extent.Items.Add("500 Hz. Octave Band");
                this.Eigen_Extent.Items.Add("1000 Hz. Octave Band");
                this.Eigen_Extent.Items.Add("2000 Hz. Octave Band");
                this.Eigen_Extent.Items.Add("4000 Hz. Octave Band");
                this.Eigen_Extent.Items.Add("8000 Hz. Octave Band");
                Label Explbl1 = new Label();
                Explbl1.Text = "Experimental";
                Explbl1.BackgroundColor = Eto.Drawing.Colors.Red;
                Explbl1.TextColor = Eto.Drawing.Colors.White;
                DLE1.AddRow(label2, this.Eigen_Extent, Explbl1);

                DynamicLayout DLE2 = new DynamicLayout();
                Label label5 = new Label();
                label5.Text = "Cutoff Time (ms):";
                this.CO_TIME.MaxValue = 8000;
                this.CO_TIME.MinValue = 100;
                this.CO_TIME.Value = 1000;
                this.EigenPML.Checked = false;
                this.EigenPML.Text = "PML";
                DLE2.AddRow(label5, null, CO_TIME, EigenPML);

                this.CalculateSim.Text = "Calculate";
                this.CalculateSim.Click += this.CalculateSim_Click;

                DynamicLayout DLE4 = new DynamicLayout();
                Label label20 = new Label();
                label20.Text = "Receiver";
                this.Receiver_Choice.SelectedIndex = 0;
                this.Receiver_Choice.SelectedIndexChanged += this.Receiver_Choice_SelectedIndexChanged;
                this.Export_Signal.Text = "Export...";
                this.Export_Signal.Click += this.Export_Signal_Click;
                DLE4.AddRow(label20, null, Receiver_Choice, Export_Signal);

                // 
                // Frequency_View
                // 
                Frequency_View = new ScottPlot.Eto.EtoPlot();
                Frequency_View.Size = new Eto.Drawing.Size(-1, 200);
                Frequency_View.Plot.Title("Frequency Domain Response");
                Frequency_View.Plot.XAxis.Label.Text = "Frequency (Hertz)";
                Frequency_View.Plot.YAxis.Label.Text = "Sound Pressure Level (dB)";
                Frequency_View.Plot.TitlePanel.Label.Font.Size = 10;
                Frequency_View.Plot.XAxis.Label.Font.Size = 10;
                Frequency_View.Plot.YAxis.Label.Font.Size = 10;
                // 
                // TransientView
                // 
                TransientView = new ScottPlot.Eto.EtoPlot();
                TransientView.Size = new Eto.Drawing.Size(-1, 200);
                TransientView.Plot.Title("Time Domain Response");
                TransientView.Plot.XAxis.Label.Text = "Time (seconds)";
                TransientView.Plot.YAxis.Label.Text = "Sound Pressure Level (dB)";
                TransientView.Plot.TitlePanel.Label.Font.Size = 10;
                TransientView.Plot.XAxis.Label.Font.Size = 10;
                TransientView.Plot.YAxis.Label.Font.Size = 10;

                DynamicLayout DLE7 = new DynamicLayout();
                Label label6 = new Label();
                label6.Text = "Eigen-Frequencies";
                label6.TextAlignment = TextAlignment.Center;
                label6.Width = 150;
                this.EigenFrequencies.SelectedIndexChanged += this.EigenFrequencies_SelectedIndexChanged;
                DLE7.AddRow(label6, EigenFrequencies);

                DynamicLayout EigenLayout = new DynamicLayout();
                EigenLayout.Spacing = new Eto.Drawing.Size(8, 8);
                DLE1.Spacing = new Eto.Drawing.Size(8, 8);
                EigenLayout.AddRow(DLE1);
                DLE2.Spacing = new Eto.Drawing.Size(8, 8);
                EigenLayout.AddRow(DLE2);
                EigenLayout.AddRow(CalculateSim);
                DLE4.Spacing = new Eto.Drawing.Size(8, 8);
                EigenLayout.AddRow(DLE4);
                EigenLayout.AddRow(TransientView);
                EigenLayout.AddRow(Frequency_View);
                DLE7.Spacing = new Eto.Drawing.Size(8, 8);
                EigenLayout.AddRow(DLE7);

                EigenTab.Content = EigenLayout;

                ////Scattering Tab
                this.TabsPrime.Pages.Add(this.ScatTab);
                this.TabsPrime.MouseUp += this.ScatteringLab_Focus;
                this.ScatTab.Text = "Scattering Analysis";
                this.DirBox.Text = "Direction";

                this.Scat_Dir_00.Checked = true;
                this.Scat_Dir_00.Text = "Normal Incidence";
                this.Scat_Dir_15.Text = "15 Degrees";
                this.Scat_Dir_30.Text = "30 Degrees";
                this.Scat_Dir_45.Text = "45 Degrees";
                this.Scat_Dir_60.Text = "60 Degrees";
                this.Scat_Dir_75.Text = "75 Degrees";
                DirBox.Width = 125;
                StackLayout dirlyt = new StackLayout();
                dirlyt.Items.Add(Scat_Dir_00);
                dirlyt.Items.Add(Scat_Dir_15);
                dirlyt.Items.Add(Scat_Dir_30);
                dirlyt.Items.Add(Scat_Dir_45);
                dirlyt.Items.Add(Scat_Dir_60);
                dirlyt.Items.Add(Scat_Dir_75);
                dirlyt.Spacing = 5;
                DirBox.Content = dirlyt;

                DynamicLayout SCtrls = new DynamicLayout();
                SCtrls.Spacing = new Eto.Drawing.Size(8, 8);
                Label Explbl2 = new Label();
                Explbl2.Text = "Experimental";
                Explbl2.TextColor = Eto.Drawing.Colors.Red;
                //Explbl2.TextColor = Eto.Drawing.Colors.White;

                SCtrls.AddRow(Explbl2);

                Label label9 = new Label();
                label9.Text = "Analysis Type:";
                Analysis_Technique.Width = 200;
                this.Analysis_Technique.Items.Add("Correlation Scattering Coefficient");
                this.Analysis_Technique.SelectedIndexChanged += this.LabGuideParametersChanged;
                SCtrls.AddRow(label9, null, Analysis_Technique);

                Label label12 = new Label();
                label12.Text = "Calculate up to:";
                this.ScatLimit.Items.Add("63 Hz. Octave Band");
                this.ScatLimit.Items.Add("125 Hz. Octave Band");
                this.ScatLimit.Items.Add("250 Hz. Octave Band");
                this.ScatLimit.Items.Add("500 Hz. Octave Band");
                this.ScatLimit.Items.Add("1000 Hz. Octave Band");
                this.ScatLimit.Items.Add("2000 Hz. Octave Band");
                this.ScatLimit.Items.Add("4000 Hz. Octave Band");
                this.ScatLimit.Items.Add("8000 Hz. Octave Band");
                this.ScatLimit.SelectedIndex = 3;
                ScatLimit.Width = 200;
                SCtrls.AddRow(label12, null, ScatLimit);

                Label label11 = new Label();
                label11.Text = "Depth of Sample (meters)";
                Sample_Depth.Width = 200;
                this.Sample_Depth.DecimalPlaces = 1;
                this.Sample_Depth.Value = 5;
                this.Sample_Depth.ValueChanged += this.LabGuideParametersChanged;
                SCtrls.AddRow(label11, null, Sample_Depth);

                Label label7 = new Label();
                label7.Text = "Measurement Radius (meters)";
                ScatteringRadius.Width = 200;
                this.ScatteringRadius.DecimalPlaces = 1;
                this.ScatteringRadius.Value = 5;
                this.ScatteringRadius.ValueChanged += this.LabGuideParametersChanged;
                SCtrls.AddRow(label7, null, ScatteringRadius);

                DynamicLayout top = new DynamicLayout();
                top.AddRow(SCtrls, DirBox);

                DynamicLayout ScatLyt = new DynamicLayout();
                ScatLyt.AddRow(top);

                DynamicLayout calclyt = new DynamicLayout();
                this.CalculateScattering.Text = "Calculate";
                CalculateScattering.Width = 250;
                this.CalculateScattering.Click += this.CalculateScattering_Click;
                this.ScatExport.Text = "Export...";
                calclyt.Spacing = new Eto.Drawing.Size(8, 8);
                calclyt.AddRow(CalculateScattering, null, ScatExport);
                ScatLyt.AddRow(calclyt);
                ScatteringGraph = new ScottPlot.Eto.EtoPlot();
                ScatteringGraph.Size = new Eto.Drawing.Size(-1, 200);
                ScatteringGraph.Plot.Title("Scattering Performance");
                ScatteringGraph.Plot.XAxis.Label.Text = "Frequency (Hz.)";
                ScatteringGraph.Plot.YAxis.Label.Text = "Scattering Coefficient";
                ScatteringGraph.Plot.TitlePanel.Label.Font.Size = 10;
                ScatteringGraph.Plot.XAxis.Label.Font.Size = 10;
                ScatteringGraph.Plot.YAxis.Label.Font.Size = 10;

                ScatLyt.AddRow(ScatteringGraph);

                DynamicLayout FreqCtrl = new DynamicLayout();

                Label label18 = new Label();
                label18.Text = "Parameter Selection";

                this.Scat_Param_Select.Items.Add("Total Reflected Power");
                this.Scat_Param_Select.Items.Add("Correlation Coefficient");
                this.Scat_Param_Select.SelectedIndex = 0;
                this.Scat_Param_Select.SelectedIndexChanged += this.Scat_Output_Changed;

                Freq_Feedback = new Label();
                this.Freq_Feedback.Text = "Frequency Selection: 0 to 8000 Hz.";

                this.Freq_Trackbar1.Value = 1;
                this.Freq_Trackbar1.MouseUp += this.Freq_Trackbar_Scroll;

                this.Freq_Trackbar2.MaxValue = 4095;
                this.Freq_Trackbar2.MouseUp += this.Freq_Trackbar_Scroll;
                FreqCtrl.AddRow(label18);
                FreqCtrl.AddRow(Scat_Param_Select);
                FreqCtrl.AddRow(Freq_Feedback);
                FreqCtrl.AddRow(Freq_Trackbar1);
                FreqCtrl.AddRow(Freq_Trackbar2);
                FreqCtrl.Spacing = new Eto.Drawing.Size(8, 8);

                scatcolorlayout = new Color_Output_Control(FreqCtrl);
                scatcolorlayout.On_Output_Changed += Scat_Output_Changed;

                ScatLyt.AddRow(scatcolorlayout);
                ScatLyt.Spacing = new Eto.Drawing.Size(8, 8);
                ScatTab.Content = ScatLyt;

                ////Visualization Tab
                this.TabsPrime.Pages.Add(this.VisTab);

                this.VisTab.Text = "Visualization";
                DynamicLayout vislyt = new DynamicLayout();
                vislyt.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                DynamicLayout Ctrls = new DynamicLayout();
                Ctrls.Spacing = new Eto.Drawing.Size(8, 8);

                Label label8 = new Label();
                label8.Text = "Source Signal:";
                this.SourceSelect.Items.Add("Sine Wave");
                this.SourceSelect.Items.Add("Dirac Pulse");
                this.SourceSelect.Items.Add("Sine Pulse");
                this.SourceSelect.Items.Add("Pseudo Random Noise");
                this.SourceSelect.Items.Add("Noise Spectrum");
                SourceSelect.SelectedIndex = 0;
                Label label17 = new Label();
                label17.BackgroundColor = Eto.Drawing.Colors.Red;
                label17.TextColor = Eto.Drawing.Colors.White;
                label17.Text = "Experimental";
                Ctrls.AddRow(label8, SourceSelect, label17);

                Label label10 = new Label();
                label10.Text = "Frequency Selection";
                this.Frequency_Selection.MaxValue = 100000;
                this.Frequency_Selection.MinValue = 1;
                this.Frequency_Selection.Value = 250;
                Ctrls.AddRow(label10, null, Frequency_Selection);

                Label label1 = new Label();
                label1.Text = "Frequency Max";
                this.Freq_Max.MaxValue = 10000000;
                this.Freq_Max.MinValue = 100;
                this.Freq_Max.Value = 250;
                Ctrls.AddRow(label1, null, Freq_Max);
                vislyt.AddRow(Ctrls);

                this.GroundPlane.Checked = false;
                this.GroundPlane.Text = "Reflective Ground Plane";
                this.VisualPML.Checked = true;
                this.VisualPML.Text = "PML";
                this.VisualPML.Checked = false;
                DynamicLayout dtls = new DynamicLayout();
                dtls.AddRow(null, VisualPML, null, GroundPlane, null);
                vislyt.AddRow(dtls);

                DynamicLayout playlyt = new DynamicLayout();
                playlyt.Spacing = new Eto.Drawing.Size(8, 8);
                PlayContinuous.Text = "Play Continuous";
                PlayContinuous.Click += Loop_Click;
                PlayContinuous.Width = 250;
                this.ForwVis.Text = ">>";
                this.ForwVis.Click += this.Forw_Click;
                this.Preview.Text = "Calculate & Run";
                this.Preview.Click += this.Calculate_Click;
                this.TimeContainer.Text = "Current Time (ms)";
                this.Time_Preview.Text = "Time_Preview";
                TimeContainer.Content = Time_Preview;

                playlyt.AddRow(TimeContainer, PlayContinuous, ForwVis);
                vislyt.AddRow(playlyt);
                vislyt.AddRow(Preview);

                this.AxisSelect.Items.Add("X");
                this.AxisSelect.Items.Add("Y");
                this.AxisSelect.Items.Add("Z");
                this.AxisSelect.SelectedIndex = 2;
                this.AxisSelect.SelectedIndexChanged += this.AxisSelect_SelectedIndexChanged;
                this.Pos_Select.ValueChanged += this.Pos_Select_ValueChanged;
                DynamicLayout Planes = new DynamicLayout();
                Planes.AddRow(Map_Planes);
                DynamicLayout PlaneButtons = new DynamicLayout();
                PlaneButtons.Spacing = new Eto.Drawing.Size(8, 8);
                PlaneButtons.AddRow(AxisSelect, Pos_Select);
                Planes.AddRow(PlaneButtons);

                this.AddPlane.Text = "Add Plane";
                this.AddPlane.Click += this.AddPlane_Click;
                this.DeletePlane.Text = "Delete Plane";
                this.DeletePlane.Click += this.DeletePlane_Click;
                //this.Magnitude.Text = "Magnitude";
                DynamicLayout buttons = new DynamicLayout();
                buttons.AddRow(AddPlane, DeletePlane);
                Planes.AddRow(buttons);
                //Planes.AddRow(Magnitude);
                Planes.Spacing = new Eto.Drawing.Size(8, 8);
                viscolor = new Color_Output_Control(Planes);
                viscolor.Update += Param_Max_ValueChanged;

                vislyt.AddRow(viscolor);

                this.SetFolder.Text = "Select Output Folder";
                this.SetFolder.Click += this.SetFolder_Click;
                this.Folder_Status.ReadOnly = true;
                DynamicLayout OutFldr = new DynamicLayout();
                OutFldr.Spacing = new Eto.Drawing.Size(8, 8);
                OutFldr.AddRow(SetFolder, Folder_Status);
                vislyt.AddRow(OutFldr);

                Medium = new Medium_Properties_Group();
                vislyt.AddRow(Medium);
                VisTab.Content = vislyt;

                this.TabsPrime.SelectedIndex = 0;
                this.Content = TabsPrime;

                Analysis_Technique.SelectedIndex = 0;
                Eigen_Extent.SelectedIndex = 4;
                scale = viscolor.Scale;
                scatterscale = scatcolorlayout.Scale;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                FileLocation.Dispose();
                scatcolorlayout.Dispose();
                Freq_Feedback.Dispose();
                Forw.Dispose();
                TabsPrime.Dispose();
                EigenTab.Dispose();
                ScatTab.Dispose();
                EdgeFreq.Dispose();
                Preview.Dispose();
                TimeContainer.Dispose();
                Time_Preview.Dispose();
                PlayContinuous.Dispose();
                ForwVis.Dispose();
                Color_Selection.Dispose();
                Pos_Select.Dispose();
                AxisSelect.Dispose();
                Map_Planes.Dispose();
                AddPlane.Dispose();
                Frequency_Selection.Dispose();
                Freq_Max.Dispose();
                SourceSelect.Dispose();
                Eigen_Extent.Dispose();
                CalculateSim.Dispose();
                Receiver_Choice.Dispose();
                CO_TIME.Dispose();
                Export_Signal.Dispose();
                Frequency_View.Dispose();
                TransientView.Dispose();
                EigenFrequencies.Dispose();
                Folder_Status.Dispose();
                SetFolder.Dispose();
                VisTab.Dispose();
                CalculateScattering.Dispose();
                ScatExport.Dispose();
                ScatteringGraph.Dispose();
                Analysis_Technique.Dispose();
                ScatteringRadius.Dispose();
                Sample_Depth.Dispose();
                ScatLimit.Dispose();
                Freq_Trackbar1.Dispose();
                Freq_Trackbar2.Dispose();
                Scat_Param_Select.Dispose();
                DirBox.Dispose();
                Scat_Dir_75.Dispose();
                Scat_Dir_60.Dispose();
                Scat_Dir_45.Dispose();
                Scat_Dir_30.Dispose();
                Scat_Dir_15.Dispose();
                Scat_Dir_00.Dispose();
                GroundPlane.Dispose();
                DeletePlane.Dispose();
                EigenPML.Dispose();
                VisualPML.Dispose();
                Medium.Dispose();
                viscolor.Dispose();
            }

            ///<summary>Gets the only instance of the PachydermAcoustic plug-in.</summary>
            public static PachTDNumericControl Instance
            {
                get;
                private set;
            }

            #region Visualization

            private Pach_Graphics.Colorscale scale;
            private Pach_Graphics.Colorscale scatterscale;
            public delegate void Populator(double Dist);
            public Numeric.TimeDomain.Acoustic_Compact_FDTD_RC FDTD;
            WaveConduit P;
            SphereConduit SP;
            Rhino.Geometry.Mesh[][] M;
            List<List<double>> Pressure;
            CellConduit c = new CellConduit();

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                Polygon_Scene Rm = RCPachTools.Get_Poly_Scene(Medium.RelHumidity, false, Medium.Temp_Celsius,Medium.StaticPressure_hPa, Medium.Atten_Method.SelectedIndex, Medium.Edge_Frequency);
                if (Rm == null || !Rm.Complete) return;

                if (P == null) P = new WaveConduit(viscolor.scale, new double[2] { viscolor.Min, viscolor.Max });
                Hare.Geometry.Point[] Src = RCPachTools.GetSource();
                Hare.Geometry.Point[] Rec = RCPachTools.GetReceivers().ToArray();
                if (Src.Length < 1 || Rm == null) Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");

                Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Dirac_Pulse;

                switch (SourceSelect.SelectedKey)
                {
                    case "Dirac Pulse":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Dirac_Pulse;
                        break;
                    case "Sine Wave":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Tone;
                        break;
                    case "Sine Pulse":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse;
                        break;
                    case "Noise Spectrum":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Spectrum;
                        break;
                }

                Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(s_type, (double)Frequency_Selection.Value, 1, RCPachTools.GetSource(0));
                Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact(Rec);

                FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD_RC(Rm, ref SD, ref Mic, (double)Freq_Max.Value, 3000, GroundPlane.Checked.Value? Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.Terrain : Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.Freefield, null, 0, 0, 0, VisualPML.Checked.Value);
                M = new Rhino.Geometry.Mesh[3][] { FDTD.m_templateX, FDTD.m_templateY, FDTD.m_templateZ };

                P.SetColorScale(viscolor.Scale, new double[] { viscolor.Min, viscolor.Max });
                P.Enabled = true;

                if (AxisSelect.SelectedIndex == 0) Pos_Select.MaxValue = FDTD.xDim - 1;
                else if (AxisSelect.SelectedIndex == 1) Pos_Select.MaxValue = FDTD.yDim - 1;
                else if (AxisSelect.SelectedIndex == 2) Pos_Select.MaxValue = FDTD.zDim - 1;

                if (Map_Planes.Items.Count == 0)
                {
                    Pos_Select.Value = Pos_Select.MaxValue / 2;
                    AddPlane_Click(new object(), new EventArgs());
                }

                Loop_Click(new object(), new EventArgs());
            }

            private double CutOffLength()
            {
                return ((double)CO_TIME.Value / 1000) * C_Sound();
            }

            private double C_Sound()
            {
                return AcousticalMath.SoundSpeed(Medium.Temp_Celsius);
            }

            private async void LoopStart()
            {
                do
                {
                    if (Running)
                    {    
                        await Task.Run(() => Forw_proc());
                        await Task.Delay(100);
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);
            }

            private void Forw_Click(object sender, EventArgs e)
            {
                Forw_proc();
            }

            private void Show_Field()
            {
                List<int> X = new List<int>(), Y = new List<int>(), Z = new List<int>();

                foreach (CutPlane p in PlaneList)
                {
                    if (p.axis == 0) X.Add(p.pos);
                    else if (p.axis == 1) Y.Add(p.pos);
                    else if (p.axis == 2) Z.Add(p.pos);
                }

                List<List<Hare.Geometry.Point>> hpts = new List<List<Hare.Geometry.Point>>();

                Eto.Forms.Application.Instance.Invoke(() =>
                {
                    FDTD.Pressure_Points(ref hpts, ref Pressure, X.ToArray(), Y.ToArray(), Z.ToArray(), 0.00002 * Math.Pow(10, viscolor.Min / 20), false, false, true);
                    List<List<Rhino.Geometry.Point3d>> Pts = new List<List<Rhino.Geometry.Point3d>>();

                    Rhino.Geometry.Mesh C = new Rhino.Geometry.Mesh();

                    FDTD.Sem_custom_mesh.WaitOne();
                    if (FDTD.m_templateC != null)
                    {
                        C = FDTD.m_templateC;
                        int ct = Pressure.Count;
                        Pressure.Add(new List<double>());
                        for (int i = 0; i < C.Faces.Count; i++) Pressure[ct].Add(FDTD.m_referenceC[i].P);
                    }
                    FDTD.Sem_custom_mesh.Release();


                    for (int i = 0; i < hpts.Count; i++)
                    {
                        Pts.Add(new List<Rhino.Geometry.Point3d>());
                        for (int j = 0; j < hpts[i].Count; j++)
                        {
                            Pts[i].Add(RCPachTools.HPttoRPt(hpts[i][j]));
                        }
                    }

                    P.Populate(X.ToArray(), Y.ToArray(), Z.ToArray(), C, FDTD.dx, Pressure, M);

                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                });
            }

            private void Forw_proc()
            {
                double t = FDTD.Increment();

                Rhino.RhinoApp.CommandPrompt = string.Format("Running {0} Hz., {1} ms.", FDTD.SD.frequency, Math.Round(t * 1000));

                Show_Field();

                Eto.Forms.Application.Instance.Invoke(() => 
                {
                    Time_Preview.Text = Math.Round(t, 3).ToString();
                
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

                ////////////////////////
                if (Folder_Status.Text.Length > 0)
                {
                    int j = (int)Math.Round(t / FDTD.dt);
                    string number;
                    if (j < 100)
                    {
                        if (t < 10) number = "00" + j.ToString();
                        else number = "0" + j.ToString();
                    }
                    else number = j.ToString();

                    Rhino.RhinoApp.RunScript("-ViewCaptureToFile " + Folder_Status.Text + "\\"[0] + "frame" + number + ".jpg Width=1280 Height=720 DrawGrid=No Enter", true);
                }
                    /////////////////////////
                });

                OnIncrement(EventArgs.Empty);
            }

            protected virtual void OnIncrement(EventArgs e)
            {
                Incremented?.Invoke(this, e);
            }

            public event EventHandler Incremented;

            bool Running = false;

            private void Loop_Click(object sender, EventArgs e)
            {
                if (PlayContinuous.Text == "Play Continuous")
                {
                    Running = true;
                    Time_Preview.Enabled = false;
                    PlayContinuous.Text = "Pause";
                    LoopStart();
                }
                else
                {
                    Running = false;
                    Time_Preview.Enabled = true;
                    PlayContinuous.Text = "Play Continuous";
                }
            }

            private void Param_Max_ValueChanged(object sender, EventArgs e)
            {
                P.SetColorScale(viscolor.Scale, new double[2] { viscolor.Min, viscolor.Max });
            }

            private void Output_Click(object sender, EventArgs e)
            {
                Eto.Forms.SelectFolderDialog sf = new Eto.Forms.SelectFolderDialog();
                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    for (int i = 0; i < FDTD.zDim; i++)
                    {
                        System.Drawing.Bitmap BM = new System.Drawing.Bitmap(FDTD.xDim, FDTD.yDim);
                        for (int j = 0; j < FDTD.xDim; j++)
                            for (int k = 0; k < FDTD.yDim; k++)
                            {
                                int V = (int)(255 * (20 * Math.Log10(FDTD.P(j, k, i) / 0.00002) / 200));
                                V = (V > 200) ? 200 : (V < 0) ? 0 : V;
                                BM.SetPixel(j, k, System.Drawing.Color.FromArgb(255, V, V, V));
                            }
                        BM.Save(sf.Directory + "\\" + i.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }

            private class CutPlane
            {
                public int pos;
                public int axis;

                public CutPlane(int Axis, int Pos)
                {
                    pos = Pos;
                    axis = Axis;
                }

                public override string ToString()
                {
                    return string.Format("{0} Axis - Pos {1}", axis == 0 ? "X" : axis == 1 ? "Y" : "Z", pos);
                }
            }

            List<CutPlane> PlaneList = new System.Collections.Generic.List<CutPlane>();

            private void AddPlane_Click(object sender, EventArgs e)
            {
                CutPlane CP = new CutPlane(AxisSelect.SelectedIndex, (int)Pos_Select.Value);
                PlaneList.Add(CP);
                Map_Planes.Items.Add(CP.ToString());
            }

            private void DeletePlane_Click(object sender, EventArgs e)
            {
                if (PlaneList == null || PlaneList.Count == 0) return;
                PlaneList.RemoveAt(Map_Planes.SelectedIndex);
                Map_Planes.Items.RemoveAt(Map_Planes.SelectedIndex);
            }

            private void AxisSelect_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Map_Planes.SelectedIndex < 0) return;
                PlaneList[Map_Planes.SelectedIndex].axis = AxisSelect.SelectedIndex;

                if (FDTD != null)
                {
                    Pos_Select.MaxValue = (PlaneList[Map_Planes.SelectedIndex].axis == 0 ? (int)(FDTD.xDim - 1) : PlaneList[Map_Planes.SelectedIndex].axis == 1 ? (int)(FDTD.yDim - 1) : (int)(FDTD.zDim - 1));
                }
            }

            private void Pos_Select_ValueChanged(object sender, EventArgs e)
            {
                if (Map_Planes.SelectedIndex < 0) return;
                PlaneList[Map_Planes.SelectedIndex].pos = (int)Pos_Select.Value;
            }

            #endregion

            #region Simulation
            double samplefrequency;

            private void CalculateSim_Click(object sender, EventArgs e)
            {
                EigenFrequencies.Items.Clear();
                Chosenfreq = 0;
                Polygon_Scene Rm = RCPachTools.Get_Poly_Scene(Medium.RelHumidity, false, Medium.Temp_Celsius, Medium.StaticPressure_hPa, Medium.Atten_Method.SelectedIndex, Medium.Edge_Frequency);
                if (!Rm.Complete) return;

                Hare.Geometry.Point[] Src = RCPachTools.GetSource();

                List<Hare.Geometry.Point> Rec = RCPachTools.GetReceivers();
                if (Src.Length < 1 || Rm == null) Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");

                Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, 1000, 1, RCPachTools.GetSource(0));
                Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact(Rec.ToArray());
                double fs = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Eigen_Extent.SelectedIndex);
                FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD_RC(Rm, ref SD, ref Mic, fs, (double)CO_TIME.Value, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.Freefield, null, 0, 0, 0, EigenPML.Checked.Value);
                FDTD.RuntoCompletion();

                samplefrequency = FDTD.SampleFrequency;

                Mic.reset();
                
                result_signals = Mic.Recordings()[0];
                
                //System.Numerics.Complex[] source_response = SD.Frequency_Response(result_signals[0].Length);

                //double f_limit = 0.8 * fs / samplefrequency * result_signals[0].Length;
                //double f_top = 1.3 * f_limit;
                //double dpi = Utilities.Numerics.PiX2 / (f_top - f_limit);

                //for (int c = 0; c < result_signals.Length; c++)
                //{
                //    System.Numerics.Complex[] result_response = Audio.Pach_SP.FFT_pacGeneral(result_signals[c],0);
                //    Array.Resize(ref result_response, result_response.Length / 2);
                //    for (int s = 0; s < result_response.Length; s++)
                //    {
                //        System.Numerics.Complex mod = source_response[s].Magnitude;
                //        if (s > f_limit) mod /= System.Numerics.Complex.Pow(source_response[s], (.5 * Math.Tanh((s-f_limit) * dpi) + 0.5));
                //        result_response[s] /= mod.Magnitude;
                //    }
                //    result_signals[c] = Audio.Pach_SP.IFFT_Real_General(Audio.Pach_SP.Mirror_Spectrum(result_response), 0);
                //}
                //Find Eigenfrequencies
                if (EigenFrequencies.Items.Count > 0) return;
                EigenFrequencies.Items.Clear();
                Find_EigenFrequencies();

                Receiver_Choice.Items.Clear();
                Time = new double[result_signals[0].Length];
                for (int i = 0; i < Time.Length; i++) Time[i] = (double)i / samplefrequency;
                for (int i = 0; i < result_signals.Length; i++) Receiver_Choice.Items.Add(i.ToString());
                Receiver_Choice.SelectedIndex = 0;
            }

            private void Find_EigenFrequencies()
            {
                for (int c = 0; c < result_signals.Length; c++)
                {
                    System.Numerics.Complex[] fdom = Audio.Pach_SP.FFT_General(result_signals[c], 0);
                    double[] mag = new double[fdom.Length];
                    double[] real = new double[fdom.Length];
                    double[] imag = new double[fdom.Length];
                    double[] freq = new double[fdom.Length];
                    for (int i = 0; i < fdom.Length / 2; i++)
                    {
                        mag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Magnitude);
                        real[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Real);
                        imag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Imaginary);
                        freq[i] = ((double)i / fdom.Length * samplefrequency);
                    }

                    PopulateEigenFrequencies(mag, freq, "Magnitude");
                    PopulateEigenFrequencies(real, freq, "Real-Part");
                    PopulateEigenFrequencies(imag, freq, "Imaginary-Part");
                }


                IListItem[] l = new IListItem[EigenFrequencies.Items.Count];
                EigenFrequencies.Items.CopyTo(l, 0);
                l.Distinct();

                List<string> eigens = new List<string>();
                foreach (IListItem i in l)
                {
                    eigens.Add(i.ToString());
                }
                eigens.Sort();

                EigenFrequencies.Items.Clear();

                foreach(string i in eigens) EigenFrequencies.Items.Add(i);
            }

            double[] Time;
            double[][] result_signals;

            private void Update_Graph(object sender, EventArgs e)
            {
                double max = 0;
                if (Receiver_Choice.Items.Count < 1) return;
                double[] SPL = Utilities.AcousticalMath.SPL_Pressure_Signal(result_signals[Receiver_Choice.SelectedIndex]);
                for (int i = 0; i < SPL.Length; i++) max = Math.Max(max, SPL[i]);

                TransientView.Plot.Clear();
                TransientView.Plot.Add.Signal(Utilities.AcousticalMath.SPL_Pressure_Signal(result_signals[Receiver_Choice.SelectedIndex]), Time[1] - Time[0], ScottPlot.Colors.Blue);
                TransientView.Plot.AutoScale();
                TransientView.Plot.XAxis.Max = Time[Time.Length - 1];
                TransientView.Plot.XAxis.Min = Time[0];

                TransientView.Plot.YAxis.Max = max * 1.2;
                TransientView.Plot.YAxis.Min = 0;// -max;

                TransientView.Invalidate();

                System.Numerics.Complex[] fdom = Audio.Pach_SP.FFT_General(result_signals[Receiver_Choice.SelectedIndex], 0);
                double[] mag = new double[fdom.Length];
                double[] real = new double[fdom.Length];
                double[] imag = new double[fdom.Length];
                double[] freq = new double[fdom.Length];
                for (int i = 0; i < fdom.Length / 2; i++)
                {
                    mag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Magnitude);
                    real[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Real);
                    imag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Imaginary);
                    freq[i] = ((double)i / fdom.Length * samplefrequency);
                }

                for (int i = 0; i < mag.Length; i++) max = Math.Max(max, mag[i]);

                Frequency_View.Plot.Clear();

                ScottPlot.DataSources.ScatterSourceXsYs srcmag = new ScottPlot.DataSources.ScatterSourceXsYs(freq, mag);
                ScottPlot.DataSources.ScatterSourceXsYs srcreal = new ScottPlot.DataSources.ScatterSourceXsYs(freq, real);
                ScottPlot.DataSources.ScatterSourceXsYs srcimag = new ScottPlot.DataSources.ScatterSourceXsYs(freq, imag);
                ScottPlot.Plottables.Scatter Smag = new ScottPlot.Plottables.Scatter(srcmag);
                ScottPlot.Plottables.Scatter Sreal = new ScottPlot.Plottables.Scatter(srcreal);
                ScottPlot.Plottables.Scatter Simag = new ScottPlot.Plottables.Scatter(srcimag);
                Smag.MarkerStyle = new ScottPlot.MarkerStyle(ScottPlot.MarkerShape.None, 0);
                Sreal.MarkerStyle = new ScottPlot.MarkerStyle(ScottPlot.MarkerShape.None, 0);
                Simag.MarkerStyle = new ScottPlot.MarkerStyle(ScottPlot.MarkerShape.None, 0);
                Smag.Color = ScottPlot.Colors.Red;
                Sreal.Color = ScottPlot.Colors.Blue;
                Simag.Color = ScottPlot.Colors.Gray;

                Frequency_View.Plot.PlottableList.Add(Smag);
                Frequency_View.Plot.PlottableList.Add(Sreal);
                Frequency_View.Plot.PlottableList.Add(Simag);
                Frequency_View.Plot.AutoScale();



                Frequency_View.Plot.XAxis.Max = freq[freq.Length / 2 - 1] / 5;
                
                Frequency_View.Plot.XAxis.Min = 0;

                Frequency_View.Plot.YAxis.Max = max * 1.2;
                Frequency_View.Plot.YAxis.Min = 0;

                Frequency_View.Invalidate();

                Frequency_View.Plot.Add.VerticalLine(Chosenfreq, 3, ScottPlot.Colors.Black);
            }

            public void PopulateEigenFrequencies(double[] mag, double[] freq, string functiontype)
            {
                MathNet.Numerics.Interpolation.CubicSpline CS = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(freq, mag);

                for (double f = freq[0]; f < freq[freq.Length / 10]; f += 10)
                {
                    double f_ = f + 10;
                    double v = CS.Differentiate(f);
                    double vf = CS.Differentiate(f_);

                    if (v > 0 && vf < 0)
                    {
                        //Look closer - find the exact frequency.
                        for (double f1 = f; f1 < f_; f1 += 1)
                        {
                            double v1 = CS.Differentiate(f1);
                            double v1f = CS.Differentiate(f1 + 1);
                            if (v1 > 0 && v1f < 0)
                            {
                                double eigen = v1 < -v1f ? Math.Ceiling(f1) : Math.Floor(f1 + 1);
                                string s = "";
                                if (eigen < 100) s = "00";
                                else if (eigen < 1000) s = "0";
                                s = s + string.Format("{0} hz. {1}", eigen, functiontype);
                                EigenFrequencies.Items.Add(s);
                            }
                        }
                    }
                }
            }

            private void Receiver_Choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graph(sender, e);
            }

            private void Export_Signal_Click(object sender, EventArgs e)
            {
                Eto.Forms.SaveFileDialog SaveWave = new Eto.Forms.SaveFileDialog();

                SaveWave.Filters.Add(" Wave Audio (*.wav) |*.wav");

                float[][] RR = new float[result_signals.Length][];
                int maxlength = 0;
                for (int j = 0; j < result_signals.Length; j++) maxlength = Math.Max(result_signals[j].Length, maxlength);
                for (int j = 0; j < result_signals.Length; j++) result_signals[j] = new double[maxlength];

                for (int j = 0; j < result_signals[0].Length; j++)
                {
                    for (int c = 0; c < result_signals.Length; c++) RR[c][j] = (j > result_signals[c].Length - 1) ? 0 : (float)result_signals[c][j];
                }

                if (SaveWave.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == DialogResult.Ok)
                {
                    Audio.Pach_SP.Wave.Write(RR, (int)samplefrequency, SaveWave.FileName, 24);
                }
            }
            #endregion
            double Chosenfreq;

            private void EigenFrequencies_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (EigenFrequencies.Items.Count < 1) return;
                Chosenfreq = double.Parse((EigenFrequencies.Items[EigenFrequencies.SelectedIndex].ToString()).Split(" "[0])[0]);
                Update_Graph(sender, e);
                Frequency_Selection.Value = Chosenfreq;
                Freq_Max.Value = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Eigen_Extent.SelectedIndex);
                VisualPML.Checked = EigenPML.Checked;
            }

            private Eto.Forms.SelectFolderDialog FileLocation = new SelectFolderDialog();
            private void SetFolder_Click(object sender, EventArgs e)
            {
                if (FileLocation.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == DialogResult.Ok)
                {
                    Folder_Status.Text = FileLocation.Directory;
                }
            }

            double[] Scattering;
            Numeric.TimeDomain.Microphone_Compact MicS;
            //List<System.Numerics.Complex[]> FFTs;
            double d_f = 2;
            int omit;
            //System.Numerics.Complex[][] FS2;
            //System.Numerics.Complex[][] FF2;
            //System.Numerics.Complex[][] FSFF;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FFTs;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FC2;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FE2;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FSFF;

            private void CalculateScattering_Click(object sender, EventArgs e)
            {
                Chosenfreq = 0;
                double radius = (double)ScatteringRadius.Value;
                double t = 5 * (radius + (double)Sample_Depth.Value) / C_Sound() * 1000;
                LabCenter = new Rhino.Geometry.Point3d(0,0, (double)Sample_Depth.Value);

                if (FC2 != null)
                {
                    for (int i = 0; i < FC2.Length; i++)
                    {
                        if (FC2[i] != null) FC2[i].Dispose();
                        if (FE2[i] != null) FE2[i].Dispose();
                        if (FSFF[i] != null) FSFF[i].Dispose();
                        if( i < FFTs.Length) if (FFTs[i] != null) FFTs[i].Dispose();
                    }
                }
                if (Analysis_Technique.SelectedIndex == 0)
                {
                    List<double> dir = new List<double>();
                    if (Scat_Dir_00.Checked.Value) dir.Add(0);
                    if (Scat_Dir_15.Checked.Value) dir.Add(15 * Math.PI / 180);
                    if (Scat_Dir_30.Checked.Value) dir.Add(30 * Math.PI / 180);
                    if (Scat_Dir_45.Checked.Value) dir.Add(45 * Math.PI / 180);
                    if (Scat_Dir_60.Checked.Value) dir.Add(60 * Math.PI / 180);
                    if (Scat_Dir_75.Checked.Value) dir.Add(75 * Math.PI / 180);

                    Source[] Src = new Source[dir.Count];
                    for (int i = 0; i < dir.Count; i++) Src[i] = new GeodesicSource(new double[8] {120,120,120,120,120,120,120,120}, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z) + new Hare.Geometry.Vector(Math.Sin(dir[i]), 0, Math.Cos(dir[i])) * (radius) + new Hare.Geometry.Vector(0,0,(double)Sample_Depth.Value), i, false);
                    List<Hare.Geometry.Point> Rec = new List<Hare.Geometry.Point>();

                    double fs = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, ScatLimit.SelectedIndex);

                    t += 60 / fs;

                    Polygon_Scene Rm = RCPachTools.Get_Poly_Scene(Medium.RelHumidity, false, Medium.Temp_Celsius, Medium.StaticPressure_hPa, Medium.Atten_Method.SelectedIndex, Medium.Edge_Frequency);
                    Rm.partition();
                    Empty_Scene Rm_Ctrl = new Empty_Scene(Medium.Temp_Celsius, Medium.RelHumidity, Medium.StaticPressure_hPa, Medium.Atten_Method.SelectedIndex, Medium.Edge_Frequency, true, Rm.Min(), Rm.Max());
                    Rm_Ctrl.PointsInScene(new List<Hare.Geometry.Point> { Rm.Min(), Rm.Max() });
                    Rm_Ctrl.partition();

                    if (!Rm.Complete && Rm_Ctrl.Complete) return;

                    Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact();
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDS = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.ScatteringLab, Utilities.RCPachTools.RPttoHPt(LabCenter), radius * 2.4, radius * 2.4, radius * 1.2 + (double)Sample_Depth.Value);
                    long size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of first model, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");
                    FDTDS.RuntoCompletion();                    
                    Mic.reset();
                    double[][] result_control = Mic.Recordings()[0];
                    samplefrequency = FDTDS.SampleFrequency;
                    double dx = FDTDS.dx, dy = FDTDS.dy, dz = FDTDS.dz;

                    Numeric.TimeDomain.Signal_Driver_Compact SDf = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Micf = new Numeric.TimeDomain.Microphone_Compact();
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDF = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm_Ctrl, ref SDf, ref Micf, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.ScatteringLab, Utilities.RCPachTools.RPttoHPt(LabCenter), radius * 2.4, radius * 2.4, radius * 1.2 + (double)Sample_Depth.Value);
                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of second model, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");
                    FDTDF.RuntoCompletion();
                    Micf.reset();
                    result_signals = Micf.Recordings()[0];

                    omit = SD.Z[0];// + 60;

                    //TODO: Micf and Mic signals do not match in length - Let's make sure that the points really all match up, how they match up, and then let's prevent an array screw up.
                    int len = Math.Min(result_signals.Length, result_control.Length);

                    //Calculate Scattering Coefficients
                    
                    //Zero packing
                    for (int i = 0; i < len; i++)
                    {
                        Array.Resize(ref result_control[i], (int)(samplefrequency / 2));
                        Array.Resize(ref result_signals[i], (int)(samplefrequency / 2));
                    }

                    Freq_Trackbar1.MaxValue = (int)(samplefrequency / 2);
                    Freq_Trackbar2.MaxValue = (int)(samplefrequency / 2);

                    System.Numerics.Complex[][] FCtrl = new System.Numerics.Complex[len][];
                    System.Numerics.Complex[][] FExp = new System.Numerics.Complex[len][];

                    for (int i = 0; i < len; i++)
                    {
                        FCtrl[i] = Audio.Pach_SP.FFT_General(result_control[i], 0);
                        FExp[i] = Audio.Pach_SP.FFT_General(result_signals[i], 0);
                    }

                    Scattering = new double[FCtrl[0].Length];
                    //FF2 = new System.Numerics.Complex[FS[0].Length][];
                    //FS2 = new System.Numerics.Complex[FS[0].Length][];
                    //FSFF = new System.Numerics.Complex[FS[0].Length][];
                    FE2 = new System.IO.MemoryMappedFiles.MemoryMappedFile[len];
                    FC2 = new System.IO.MemoryMappedFiles.MemoryMappedFile[len];
                    FSFF = new System.IO.MemoryMappedFiles.MemoryMappedFile[len];

                    System.Numerics.Complex[] sumFC2 = new System.Numerics.Complex[(int)(samplefrequency / 2)];
                    System.Numerics.Complex[] sumFE2 = new System.Numerics.Complex[(int)(samplefrequency / 2)];
                    System.Numerics.Complex[] sumFSFF = new System.Numerics.Complex[(int)(samplefrequency / 2)];

                    for (int i = 0; i < len; i++)
                    {
                        FC2[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FC2"+i.ToString(), (int)(samplefrequency / 2) * 16);
                        FE2[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FE2" + i.ToString(), (int)(samplefrequency / 2) * 16);
                        FSFF[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FSFF"+i.ToString(), (int)(samplefrequency / 2) * 16);
                        System.IO.BinaryWriter fs2writer = new System.IO.BinaryWriter(FC2[i].CreateViewStream());
                        System.IO.BinaryWriter ff2writer = new System.IO.BinaryWriter(FE2[i].CreateViewStream());
                        System.IO.BinaryWriter fsffwriter = new System.IO.BinaryWriter(FSFF[i].CreateViewStream());
                        //FF2[i] = new System.Numerics.Complex[FS.Length];
                        //FS2[i] = new System.Numerics.Complex[FS.Length];
                        //FSFF[i] = new System.Numerics.Complex[FS.Length];

                        for (int j = 0; j < (int)(samplefrequency / 2); j++)
                        {
                            System.Numerics.Complex fc2 = System.Numerics.Complex.Pow(FCtrl[i][j].Magnitude, 2);
                            System.Numerics.Complex fe2 = System.Numerics.Complex.Pow(FExp[i][j].Magnitude, 2);
                            System.Numerics.Complex fsff = FExp[i][j] * System.Numerics.Complex.Conjugate(FCtrl[i][j]);

                            fs2writer.Write(fc2.Real);
                            fs2writer.Write(fc2.Imaginary);
                            ff2writer.Write(fe2.Real);
                            ff2writer.Write(fe2.Imaginary);
                            fsffwriter.Write(fsff.Real);
                            fsffwriter.Write(fsff.Imaginary);

                            sumFC2[j] += fc2;// * Math.Sin(i%10 * Math.PI / 18);
                            sumFE2[j] += fe2;// * Math.Sin(i % 10 * Math.PI / 18);
                            sumFSFF[j] += fsff * Math.Sin(i % 10 * Math.PI / 18);
                            //FS2[i][j] = System.Numerics.Complex.Pow(FS[j][i].Magnitude, 2);
                            //FF2[i][j] = System.Numerics.Complex.Pow(FF[j][i].Magnitude, 2);
                            //FSFF[i][j] = FS[j][i] * System.Numerics.Complex.Conjugate(FF[j][i]);
                            //sumFS2 += FS2[i][j];
                            //sumFF2 += FF2[i][j];
                            //sumFSFF += FSFF[i][j];
                        }
                        fs2writer.Close();
                        ff2writer.Close();
                        fsffwriter.Close();
                        fs2writer.Dispose();
                        ff2writer.Dispose();
                        fsffwriter.Dispose();

                        //System.Numerics.Complex sumReflected = sumFSFF / sumFE2;
                        //System.Numerics.Complex Ratio = sumFE2 / sumFC2;
                        //Scattering[i] = 1 - System.Numerics.Complex.Abs(sumReflected * sumReflected * Ratio);
                        
                    }

                    double denom = 0;
                    for(int i = 0; i < 9; i++)
                    {
                        denom += Math.Sin(i % 10 * Math.PI / 18) / 9;
                    }

                    for (int j = 0; j < (int)(samplefrequency / 2); j++)
                    {
                        Scattering[j] = 1 - Math.Pow(sumFSFF[j].Magnitude, 2) / ((sumFC2[j] * sumFE2[j]).Real);
                        Scattering[j] /= denom;
                    }

                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of coefficient calcs, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");

                    ///Add Balloon plot aparatus
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();

                    FFTs = new System.IO.MemoryMappedFiles.MemoryMappedFile[Mic.X.Length];
                    //System.Numerics.Complex.Pow(FS[j][i].Magnitude, 2);
                    //fftwriter.Write(fs2.Real);

                    for (int i = 0; i < Mic.X.Length; i++)
                    {
                        double[] Reflection = Mic.Recordings(i, omit);
                        if (Reflection.Length < 8192) Array.Resize(ref Reflection, omit + Reflection.Length);

                        System.Numerics.Complex[] fft = Audio.Pach_SP.FFT_General(Reflection, 0);
                        FFTs[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FFT" + i.ToString(), fft.Length * 16);
                        System.IO.BinaryWriter FFTwriter = new System.IO.BinaryWriter(FFTs[i].CreateViewStream());
                        d_f = samplefrequency / fft.Length;

                        for (int j = 0; j < fft.Length; j++)
                        {
                            FFTwriter.Write(fft[j].Real);
                            FFTwriter.Write(fft[j].Imaginary);
                        }
                        pts.Add(FDTDF.RDD_Location(Mic.X[i] , Mic.Y[i], Mic.Z[i]) + Utilities.RCPachTools.RPttoHPt(LabCenter));
                        FFTwriter.Close(); FFTwriter.Dispose();
                    }

                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of balloon points, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");

                    Sphere_Plot SPS = new Sphere_Plot(pts, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), Math.Sqrt(dx * dx + dy * dy + dz * dz));
                    if (SP == null) SP = new SphereConduit(SPS, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), scatterscale, new double[2] { scatcolorlayout.Min, scatcolorlayout.Max });
                    else SP.plot = SPS;

                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of balloon construction, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");

                    MicS = Mic;
                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of simulation, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");
                }
                else if (Analysis_Technique.SelectedIndex == 1)
                {
                    Polygon_Scene Rm = RCPachTools.Get_Poly_Scene(Medium.RelHumidity, false, Medium.Temp_Celsius, Medium.StaticPressure_hPa, Medium.Atten_Method.SelectedIndex, Medium.Edge_Frequency);
                    Empty_Scene Rm_Ctrl = new Empty_Scene(Medium.Temp_Celsius, Medium.RelHumidity, Medium.StaticPressure_hPa, Medium.Atten_Method.SelectedIndex, Medium.Edge_Frequency, true, Rm.Min(), Rm.Max());
                    Rm_Ctrl.PointsInScene(new List<Hare.Geometry.Point> { Rm.Min(), Rm.Max() });
                    Rm_Ctrl.partition();

                    if (!Rm.Complete && Rm_Ctrl.Complete) return;

                    Hare.Geometry.Point ArrayCenter = new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z + (double)Sample_Depth.Value);

                    Source[] Src = new Source[1] { new GeodesicSource(new double[8] { 120, 120, 120, 120, 120, 120, 120, 120 }, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z + radius + (double)Sample_Depth.Value), 0, false) };

                    double fs = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Analysis_Technique.SelectedIndex);

                    Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact();
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDS = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.TransparencyLab, Utilities.RCPachTools.RPttoHPt(LabCenter), radius * 4, radius * 4, radius * 1.2 + (double)Sample_Depth.Value);
                    FDTDS.RuntoCompletion();

                    Numeric.TimeDomain.Signal_Driver_Compact SDf = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Micf = new Numeric.TimeDomain.Microphone_Compact();
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDF = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm_Ctrl, ref SDf, ref Micf, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.ScatteringLab, Utilities.RCPachTools.RPttoHPt(LabCenter), radius * 4, radius * 4, radius * 1.2 + (double)Sample_Depth.Value);
                    FDTDF.RuntoCompletion();

                    int start = (int)Math.Round((2.25 * radius / Rm.Sound_speed(Src[0].Origin)) / FDTDS.dt);

                    samplefrequency = FDTDS.SampleFrequency;

                    Mic.reset();
                    result_signals = Mic.Recordings()[0];

                    //Calculate Scattering Coefficients
                    double[][] TimeS = Mic.Recordings()[0];
                    double[][] TimeF = Micf.Recordings()[0];

                    System.Numerics.Complex[][] FS = new System.Numerics.Complex[TimeS.Length][];
                    System.Numerics.Complex[][] FF = new System.Numerics.Complex[TimeS.Length][];

                    for (int i = 0; i < TimeS.Length; i++)
                    {
                        double[] ts = new double[TimeS[i].Length - start];
                        double[] tf = new double[TimeS[i].Length - start];
                        for (int ti = start; ti < TimeS[i].Length; ti++)
                        {
                            ts[ti - start] = TimeS[i][ti];
                            tf[ti - start] = TimeF[i][ti];
                        }
                        FS[i] = Audio.Pach_SP.FFT_General(ts, 0);
                        FF[i] = Audio.Pach_SP.FFT_General(TimeF[i], 0);
                    }

                    Scattering = new double[FS[0].Length];

                    for (int i = 0; i < FS[0].Length; i++)
                    {
                        System.Numerics.Complex sumFF2 = 0;

                        double mod = Math.Sin(i % 10 * Math.PI / 18);

                        for (int j = 0; j < FS.Length; j++)
                        {
                            sumFF2 += System.Numerics.Complex.Pow(FS[j][i], 2) / System.Numerics.Complex.Pow(FF[j][i], 2) * mod;
                        }

                        Scattering[i] = Utilities.AcousticalMath.SPL_Pressure(System.Numerics.Complex.Abs(sumFF2));
                    }
                    ///Add Balloon plot aparatus
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();
                    for (int i = 0; i < Mic.X.Length; i++)
                    {
                        Hare.Geometry.Vector pt = FDTDS.RDD_Location(Mic.X[i], Mic.Y[i], Mic.Z[i]) - Utilities.RCPachTools.RPttoHPt(LabCenter);
                        pts.Add(new Hare.Geometry.Point(pt.dx, pt.dy, pt.dz));
                    }

                    Sphere_Plot SPS = new Sphere_Plot(pts, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), 5 * Math.Sqrt(FDTDS.dx * FDTDS.dx + FDTDS.dy * FDTDS.dy + FDTDS.dz * FDTDS.dz));
                    if (SP == null) SP = new SphereConduit(SPS, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), scatterscale, new double[2] { scatcolorlayout.Min, scatcolorlayout.Max });
                    MicS = Mic;
                }
                Update_Scattering_Graph(null, null);
            }

            Rhino.Geometry.Point3d LabCenter;

            private void Set_Origin_Click(object sender, EventArgs e)
            {
                Rhino.Geometry.Point3d pt;
                Rhino.Commands.Result rr = Rhino.Input.RhinoGet.GetPoint("Select the center point of the sample that you would like to analyze...", true, out pt);
                if (rr == Rhino.Commands.Result.Success) LabCenter = pt;
            }

            private void Update_Scattering_Graph(object sender, EventArgs e)
            {
                if (Scattering == null || Scattering.Length == 0) return;
                double max = Scattering.Max();

                ScatteringGraph.Plot.Clear();
                ScatteringGraph.Plot.XAxis.Max = samplefrequency / 2;
                ScatteringGraph.Plot.XAxis.Min = 0;
 
                ScatteringGraph.Plot.YAxis.Max = 1.0;
                if (max > 1) ScatteringGraph.Plot.YAxis.Max = max;

                ScatteringGraph.Plot.YAxis.Min = 0;

                ScatteringGraph.Plot.Add.Signal(Scattering, samplefrequency / (Scattering.Length * 2), ScottPlot.Colors.Red);

                System.Numerics.Complex[] SWC = new System.Numerics.Complex[FFTs.Length];
                double[] SW = new double[FFTs.Length];

                int[] F = new int[2] { (int)Math.Floor(Math.Min(Freq_Trackbar1.Value, Freq_Trackbar2.Value) * Scattering.Length / samplefrequency), (int)Math.Ceiling(Math.Max(Freq_Trackbar1.Value, Freq_Trackbar2.Value) * Scattering.Length / samplefrequency )};

                if (Scat_Param_Select.SelectedIndex == 0)
                {
                    for (int i = 0; i < FFTs.Length; i++)
                    {
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream fftreader = FFTs[i].CreateViewStream();
                        fftreader.Position = F[0]*16;
                        for (int f = F[0]; f < F[1]; f++)
                        {
                            byte[] R = new byte[8], I = new byte[8];
                            fftreader.Read(R, 0, 8);
                            fftreader.Read(I, 0, 8);
                            System.Numerics.Complex X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            SWC[i] += X * X;// FFTs[i][f] * FFTs[i][f];
                        }
                        fftreader.Dispose();
                        SW[i] = Pachyderm_Acoustic.Utilities.AcousticalMath.SPL_Intensity(SWC[i].Magnitude / (F[1] - F[0]));
                    }
                }
                else if (Scat_Param_Select.SelectedIndex == 1)
                {
                    System.Numerics.Complex[] sumFS2 = new System.Numerics.Complex[FFTs.Length], sumFF2 = new System.Numerics.Complex[FFTs.Length], sumFSFF = new System.Numerics.Complex[FFTs.Length];
                    for (int i = 0; i < FFTs.Length; i++)
                    {
                        sumFS2[i] = System.Numerics.Complex.Zero;
                        sumFF2[i] = System.Numerics.Complex.Zero;
                        sumFSFF[i] = System.Numerics.Complex.Zero;
                    }
                    
                    for (int i = 0; i < FFTs.Length; i++)
                    {
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream fs2reader = FC2[i].CreateViewStream();
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream ff2reader = FE2[i].CreateViewStream();
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream fsffreader = FSFF[i].CreateViewStream();
                        fs2reader.Position = F[0] * 16;
                        ff2reader.Position = F[0] * 16;
                        fsffreader.Position = F[0] * 16;

                        for (int f = F[0]; f < F[1]; f++)
                        {
                            byte[] R = new byte[8], I = new byte[8];
                            fs2reader.Read(R, 0, 8);
                            fs2reader.Read(I, 0, 8);
                            System.Numerics.Complex X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            sumFS2[i] += X;
                            ff2reader.Read(R, 0, 8);
                            ff2reader.Read(I, 0, 8);
                            X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            sumFF2[i] += X;
                            fsffreader.Read(R, 0, 8);
                            fsffreader.Read(I, 0, 8);
                            X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            sumFSFF[i] += X;
                        }

                        fs2reader.Dispose();
                        ff2reader.Dispose();
                        fsffreader.Dispose();
                    }

                    for (int i = 0; i < FFTs.Length; i++)
                    {
                        System.Numerics.Complex sumReflected = sumFSFF[i] / sumFF2[i];
                        System.Numerics.Complex Ratio = sumFF2[i] / sumFS2[i];
                        SW[i] = 100 * (1 - System.Numerics.Complex.Abs(sumReflected * sumReflected * Ratio))/(F[1] - F[0]);
                    }
                }
                SP.Enabled = true;
                SP.Data_in(SW.ToArray(), new double[2] { scatcolorlayout.Min, scatcolorlayout.Max }, (double)ScatteringRadius.Value);
            }

            private void Update_LabGuides()
            {
                c.labguide = true;
                c.hemianechoic = (Analysis_Technique.SelectedIndex == 0);
                c.radius = (double)this.ScatteringRadius.Value;
                c.depth = (double)this.Sample_Depth.Value;
            }

            private void ScatteringLab_Focus(object sender, EventArgs e)
            {
                if (TabsPrime.SelectedPage == ScatTab)
                {
                    c.Enabled = true;
                    c.labguide = true;
                    Update_LabGuides();
                }
                else 
                {
                    c.Enabled = false;
                    c.labguide = false;
                }
            }

            private void LabGuideParametersChanged(object sender, EventArgs e)
            {
                Update_LabGuides();
            }

            private void Scat_Output_Changed(object sender, EventArgs e)
            {
                Update_Scattering_Graph(null, null);
            }

            private void Freq_Trackbar_Scroll(object sender, EventArgs e)
            {
                Freq_Feedback.Text = "Frequency Selection: " + Math.Round(d_f * Math.Min(Freq_Trackbar1.Value, Freq_Trackbar2.Value)) + " to " + Math.Round(d_f * Math.Max(Freq_Trackbar1.Value, Freq_Trackbar2.Value)) + " Hz.";
                Update_Scattering_Graph(null, null);
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