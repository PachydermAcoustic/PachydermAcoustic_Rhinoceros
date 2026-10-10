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

using Eto.Drawing;
using Eto.Forms;
using MathNet.Numerics.Optimization;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using Rhino.Geometry;
using Rhino.UI;
using Rhino.UI.Controls.ThumbnailUI;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static HDF.PInvoke.H5Z;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("8559be06-21d7-4535-803e-95a9dd3a2898")]
        public class PachHybridControl : Panel, IPanel
        {
            public PachHybridControl()
            {
                this.MenuStrip = new SegmentedButton();
                this.fileToolStripMenuItem = new MenuSegmentedItem();
                this.openDataToolStripMenuItem = new ButtonMenuItem();
                this.saveDataToolStripMenuItem = new ButtonMenuItem();
                this.saveParameterResultsToolStripMenuItem = new ButtonMenuItem();
                this.savePressureResultsToolStripMenuItem = new ButtonMenuItem();
                this.savePTBFormatToolStripMenuItem = new ButtonMenuItem();
                this.savePressurePTBFormatToolStripMenuItem = new ButtonMenuItem();
                this.saveEDCToolStripMenuItem = new ButtonMenuItem();

                DynamicLayout File = new DynamicLayout();
                File.AddRow(MenuStrip, null);
                this.MenuStrip.Items.Add(this.fileToolStripMenuItem);
                this.MenuStrip.Items.Add(new UI.HelpMenu());
                fileToolStripMenuItem.Menu = new ContextMenu();
                this.fileToolStripMenuItem.Menu.Items.Add(openDataToolStripMenuItem);
                this.fileToolStripMenuItem.Menu.Items.Add(this.saveDataToolStripMenuItem);
                this.fileToolStripMenuItem.Menu.Items.Add(this.saveParameterResultsToolStripMenuItem);
                this.fileToolStripMenuItem.Menu.Items.Add(this.savePressureResultsToolStripMenuItem);
                this.fileToolStripMenuItem.Menu.Items.Add(this.savePTBFormatToolStripMenuItem);
                this.fileToolStripMenuItem.Menu.Items.Add(this.savePressurePTBFormatToolStripMenuItem);
                this.fileToolStripMenuItem.Menu.Items.Add(this.saveEDCToolStripMenuItem);
                this.fileToolStripMenuItem.Text = "File";

                // 
                // openDataToolStripMenuItem
                // 
                this.openDataToolStripMenuItem.Text = "Open Data...";
                this.openDataToolStripMenuItem.Click += this.OpenDataToolStripMenuItem_Click;
                // 
                // saveDataToolStripMenuItem
                // 
                this.saveDataToolStripMenuItem.Text = "Save Data";
                this.saveDataToolStripMenuItem.Click += this.SaveDataToolStripMenuItem_Click;
                // 
                // saveParameterResultsToolStripMenuItem
                // 
                this.saveParameterResultsToolStripMenuItem.Text = "Save Intensity Results";
                this.saveParameterResultsToolStripMenuItem.Click += this.SaveIntensityResultsToolStripMenuItem_Click;
                // 
                // savePressureResultsToolStripMenuItem
                // 
                this.savePressureResultsToolStripMenuItem.Text = "Save Pressure Results";
                this.savePressureResultsToolStripMenuItem.Click += this.savePressureResultsToolStripMenuItem_Click_1;
                // 
                // savePTBFormatToolStripMenuItem
                // 
                this.savePTBFormatToolStripMenuItem.Text = "Save Intensity PTB Format";
                this.savePTBFormatToolStripMenuItem.Click += this.saveIntensityPTBFormatToolStripMenuItem_Click;
                // 
                // savePressurePTBFormatToolStripMenuItem
                // 
                this.savePressurePTBFormatToolStripMenuItem.Text = "Save Pressure PTB Format";
                this.savePressurePTBFormatToolStripMenuItem.Click += this.savePressurePTBFormatToolStripMenuItem_Click_1;
                // 
                // saveEDCToolStripMenuItem
                // 
                this.saveEDCToolStripMenuItem.Text = "Save Decay Curve";
                this.saveEDCToolStripMenuItem.Click += this.saveEDCToolStripMenuItem_Click;

                DynamicLayout HybridLO = new DynamicLayout();
                HybridLO.AddRow(File);
                

                this.Tabs = new TabControl();
                this.Tabs.SelectedIndexChanged += this.Tab_Selecting;
                this.TabImpulse = new TabPage();
                this.TabImpulse.Text = "Impulse";
                DynamicLayout Impulse_LO = new DynamicLayout();
                Impulse_LO.DefaultSpacing = new Size(8, 8);
                DynamicLayout RL = new DynamicLayout();
                RL.DefaultSpacing = new Size(8, 8);
                Label LabelRec = new Label();
                LabelRec.Text = "Receiver:";

                this.ReceiverSelection = new ComboBox();
                this.ReceiverSelection.Items.Add("1 m. Stationary Receiver");
                this.ReceiverSelection.Items.Add("Expanding Receiver (Expanding)");
                this.ReceiverSelection.SelectedIndex = 0;
                RL.AddRow(LabelRec, ReceiverSelection);
                SimulationOctave = new RadioButton { Text = "Octave", Checked = true };
                SimulationThirdOctave = new RadioButton(SimulationOctave) { Text = "Third octave" };
                StackLayout bandSelection = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 18,
                    Padding = new Padding(4, 10, 4, 12),
                    Items = { SimulationOctave, SimulationThirdOctave }
                };
                RL.AddRow(new GroupBox { Text = "Simulation frequency bands", Content = bandSelection });
                Impulse_LO.AddRow(RL);

                DynamicLayout Ctrls = new DynamicLayout();
                Ctrls.DefaultSpacing = new Size(8, 8);
                this.ISBox = new CheckBox();
                this.ISBox.Checked = true;
                this.ISBox.Text = "Image Source Solution";
                this.ISBox.CheckedChanged += this.CalcType_CheckedChanged;
                Label LabelISOrder = new Label();
                LabelISOrder.Text = "Reflection Order";
                this.Image_Order = new NumericStepper();
                this.Image_Order.MaxValue = 12;
                this.Image_Order.MinValue = 1;
                this.Image_Order.Value = 1;
                Ctrls.AddRow(ISBox);
                Ctrls.AddRow(LabelISOrder, null, null, Image_Order);
                this.BTM_ED = new CheckBox();
                this.BTM_ED.Text = "BTM Edge Diffraction";
                this.labelExp = new Label();
                this.labelExp.Text = "Experimental";
                Ctrls.AddRow(BTM_ED, null, null, labelExp);
                this.Specular_Trace = new CheckBox();
                this.Specular_Trace.Enabled = false;
                this.Specular_Trace.Text = "Image Source Tracing";
                this.Specular_Trace.CheckedChanged += this.CalcType_CheckedChanged;
                this.Spec_RayCount = new NumericStepper();
                this.Spec_RayCount.Increment = 1000;
                this.Spec_RayCount.MaxValue = 10000000;
                this.Spec_RayCount.MinValue = 1000;
                this.Spec_RayCount.Value = 50000;
                Ctrls.AddRow(Specular_Trace, null, null, Spec_RayCount);

                this.RTBox = new CheckBox();
                this.RTBox.Checked = true;
                this.RTBox.Text = "Raytracing Solution";
                this.RTBox.CheckedChanged += this.CalcType_CheckedChanged;
                Ctrls.AddRow(RTBox);
                GroupBox labelConv = new GroupBox();
                labelConv.Text = "Convergence:";
                this.Spec_Rays = new RadioButton();
                this.Spec_Rays.Text = "Specify Ray Count (no convergence)";
                this.Spec_Rays.MouseUp += this.Convergence_CheckedChanged;
                this.Minimum_Convergence = new RadioButton();
                this.Minimum_Convergence.Checked = true;
                this.Minimum_Convergence.Text = "Minimum Convergence";
                this.Minimum_Convergence.MouseUp += this.Convergence_CheckedChanged;
                this.DetailedConvergence = new RadioButton();
                this.DetailedConvergence.Text = "Detailed Convergence";
                this.DetailedConvergence.MouseUp += this.Convergence_CheckedChanged;
                DynamicLayout conv = new DynamicLayout();
                conv.AddRow(Minimum_Convergence);
                conv.AddRow(DetailedConvergence);
                conv.AddRow(Spec_Rays);
                labelConv.Content = conv;
                Impulse_LO.AddRow(Ctrls);
                Impulse_LO.AddRow(labelConv);
                DynamicLayout Ctrls2 = new DynamicLayout();
                Label LabelRayNo = new Label();
                this.RT_Count = new NumericStepper();
                LabelRayNo.Text = "Number of Rays";
                this.RT_Count.Enabled = false;
                this.RT_Count.Increment = 1000;
                this.RT_Count.MaxValue = 10000000;
                this.RT_Count.MinValue = 1;
                this.RT_Count.Value = 10000;
                Ctrls2.AddRow(LabelRayNo, null, null, RT_Count);

                Label COTime = new Label();
                COTime.Text = "Cut Off Time (ms)";
                this.CO_TIME = new NumericStepper();
                this.CO_TIME.MaxValue = 15000;
                this.CO_TIME.MinValue = 1;
                this.CO_TIME.Value = 1000;
                Ctrls2.AddRow(COTime, null, null, CO_TIME);
                Impulse_LO.AddRow(Ctrls2);
                this.Calculate = new Button();
                this.Calculate.Text = "Calculate Solution";
                this.Calculate.Click += this.Calculate_Click;
                Impulse_LO.AddRow(Calculate);
                this.MediumProps = new Medium_Properties_Group();
                Impulse_LO.AddSpace();
                Impulse_LO.AddRow(MediumProps);
                TabImpulse.Content = Impulse_LO;
                this.Tabs.Pages.Add(this.TabImpulse);

                this.TabMaterials = new TabPage();
                this.TabMaterials.Text = "Materials";
                this.TabMaterials.MouseEnter += this.Materials_MouseEnter;
                DynamicLayout Mat_LO = new DynamicLayout();
                Mat_LO.DefaultSpacing = new Size(8, 8);
                DynamicLayout LL = new DynamicLayout();
                LL.DefaultSpacing = new Size(8, 8);
                this.LayerDisplay = new DropDown();
                Label LayerLbl = new Label();
                LayerLbl.Text = "For Layer:";
                this.LayerDisplay.SelectedValueChanged += this.Retrieve_Layer_Acoustics;
                LL.AddRow(LayerLbl, LayerDisplay);
                Mat_LO.AddRow(LL);

                this.tabCoef = new TabControl();
                Mat_LO.AddRow(tabCoef);

                this.Absorption = new TabPage();
                this.Absorption.Text = "Absorption";
                tabCoef.Pages.Add(Absorption);

                DynamicLayout ABSLib = new DynamicLayout();
                ABSLib.DefaultSpacing = new Size(8, 8);
                DynamicLayout RM = new DynamicLayout();
                RM.DefaultSpacing = new Size(8, 8);
                Scrollable matscroll = new Scrollable();
                this.Material_Lib = new ListBox();
                matscroll.Content = Material_Lib;
                Label Mat_Lbl = new Label();
                Mat_Lbl.Text = "Material Library:";
                this.Material_Lib.SelectedIndexChanged += this.Material_Lib_SelectedIndexChanged;
                RM.AddRow(Mat_Lbl);
                matscroll.Width = 200;
                matscroll.Height = 100;
                RM.AddRow(matscroll);
                this.SaveAbsBox = new GroupBox();
                this.Delete_Material = new Button();
                this.Save_Material = new Button();
                this.Material_Name = new MaskedTextBox();
                Label labelAbs = new Label();
                labelAbs.Text = "Absorption Coefficients (% energy absorbed)";
                this.SaveAbsBox.Text = "Save Material Absorption";
                DynamicLayout SM = new DynamicLayout();
                SM.DefaultSpacing = new Size(8, 8);
                SM.AddRow(Material_Name);
                this.Save_Material.Text = "Save Material";
                this.Save_Material.Click += this.SaveAbs_Click;
                SM.AddRow(Save_Material);
                this.Delete_Material.Text = "Delete Materials";
                this.Delete_Material.Click += this.Delete_Material_Click;
                SM.AddRow(Delete_Material);
                SaveAbsBox.Content = SM;
                ABSLib.AddRow(RM, SaveAbsBox);
                DynamicLayout AL = new DynamicLayout();
                AL.DefaultSpacing = new Size(8, 8);
                AL.AddRow(ABSLib);

                this.Abs_Designer = new Button();
                this.Abs_Designer.Text = "Call Absorption Designer";
                this.Abs_Designer.Click += this.Abs_Designer_Click;
                AL.AddRow(Abs_Designer);
                AL.AddRow(labelAbs);

                ABSSlider = new FreqSlider(FreqSlider.bands.Octave, allowBandSelection: true);
                ABSSlider.MouseLeave += this.Acoustics_Coef_Update;
                ABSSlider.BandChanged += this.Acoustics_Coef_Update;

                Scrollable Absscroll = new Scrollable();
                Absscroll.Content = ABSSlider;
                AL.AddRow(Absscroll);
                this.SmartMat_Display = new ScottPlot.Eto.EtoPlot();

                polarPlot = this.SmartMat_Display.Plot.Add.PolarAxis(1);
                polarPlot.Circles.ForEach(x => x.LineColor = ScottPlot.Colors.Grey);
                polarPlot.Spokes.ForEach(x => x.LineColor = ScottPlot.Colors.Grey);
                polarPlot.Rotation = ScottPlot.Angle.FromDegrees(90);
                //(SmartMat_Display.Plot.PlottableList.Last() as ScottPlot.Plottables.Scatter).MarkerStyle = ScottPlot.MarkerStyle.None;

                this.SmartMat_Display.Size = new Size(0, 300);
                this.SmartMat_Display.TabIndex = 45;
                this.SmartMat_Display.Plot.Title("Absorption By Angle");
                this.SmartMat_Display.Visible = false;
                AL.AddRow(SmartMat_Display);
                AL.AddSpace();
                Absorption.Content = AL;

                this.Scattering = new TabPage();
                this.Scattering.Text = "Scattering";
                this.tabCoef.Pages.Add(Scattering);

                DynamicLayout SL = new DynamicLayout();
                SL.DefaultSpacing = new Size(8, 8);
                this.GlassScatter = new Button();
                this.GlassScatter.Text = "Flat (Glass)";
                this.GlassScatter.Click += this.GlassScatter_Click;
                this.PlasterScatter = new Button();
                this.PlasterScatter.Text = "Flat (plaster/gypsum)";
                this.PlasterScatter.Click += this.PlasterScatter_Click;
                SL.AddRow(GlassScatter);
                SL.AddRow(PlasterScatter);
                this.labelVar = new Label();
                this.labelVar.Text = "Variegation (characteristic dimension)";
                SL.AddRow(labelVar);

                DynamicLayout VC = new DynamicLayout();
                VC.DefaultSpacing = new Size(8, 8);
                this.user_quart_lambda = new Slider();
                this.user_quart_lambda.MaxValue = 1000;
                this.user_quart_lambda.TickFrequency = 10;
                this.user_quart_lambda.Value = 25;
                this.user_quart_lambda.ValueChanged += this.Variegaton_Scroll;
                this.user_quart_lambda.Width = 300;
                VC.SizeChanged += delegate { this.user_quart_lambda.Width = this.Width - 75; };
                this.quart_lambda = new Label();
                this.quart_lambda.Text = "25 mm.";
                VC.AddRow(user_quart_lambda, null, quart_lambda);
                SL.AddRow(VC);

                Label labelScat = new Label();
                labelScat.Text = "Scattering Coefficients (% non-specular reflected energy)";
                SCATSlider = new FreqSlider(FreqSlider.bands.Octave);
                SCATSlider.MouseLeave += Acoustics_Coef_Update;
                SL.AddRow(labelScat);
                SL.AddRow(SCATSlider);
                SL.AddSpace();
                Scattering.Content = SL;

                this.Transparency = new TabPage();
                this.Transparency.Text = "Transparency";
                this.tabCoef.Pages.Add(this.Transparency);
                this.tabTC = new TabPage();
                this.tabTL = new TabPage();

                this.tabTransControls = new TabControl();
                this.tabTransControls.Pages.Add(this.tabTC);
                this.tabTransControls.Pages.Add(this.tabTL);
                this.tabTransControls.SelectedIndex = 0;
                this.tabTC.Text = "Transmission Coefficient";
                this.tabTL.Text = "Transmission Loss";
                Transparency.Content = tabTransControls;

                Label labelTC = new Label();
                labelTC.Text = "Transmission Coefficients (%  non-absorbed transmitted energy)";

                DynamicLayout TCL = new DynamicLayout();
                TCL.DefaultSpacing = new Size(8, 8);
                TCL.AddRow(labelTC);
                TRANSSlider = new FreqSlider(FreqSlider.bands.Octave);
                TRANSSlider.Enabled = false;
                TRANSSlider.MouseLeave += Acoustics_Coef_Update;
                TCL.AddRow(TRANSSlider);
                this.Trans_Check = new CheckBox();
                this.Trans_Check.Text = "Semi-Transparent Material";
                this.Trans_Check.CheckedChanged += this.Trans_Check_CheckedChanged;
                TCL.AddRow(Trans_Check);
                TCL.AddSpace();
                tabTC.Content = TCL;

                DynamicLayout TLLO = new DynamicLayout();
                TLLO.DefaultSpacing = new Size(8, 8);
                DynamicLayout TLtop = new DynamicLayout();
                TLtop.DefaultSpacing = new Size(8, 8);
                DynamicLayout TLL = new DynamicLayout();
                TLL.DefaultSpacing = new Size(8, 8);
                this.labelTransLib = new Label();
                this.labelTransLib.Text = "Transmission Library:";
                this.Isolation_Lib = new ListBox();
                this.Isolation_Lib.SelectedIndexChanged += this.Isolation_Lib_SelectedIndexChanged;
                TLL.AddRow(labelTransLib);
                TLL.AddRow(Isolation_Lib);

                this.SaveTLBox = new GroupBox();
                this.SaveTLBox.Text = "Save Assembly Transmission Loss";
                this.labelEXP2 = new Label();
                this.labelEXP2.Text = "Experimental";
                DynamicLayout STL = new DynamicLayout();
                STL.DefaultSpacing = new Size(8, 8);
                this.IsolationAssemblies = new MaskedTextBox();
                STL.AddRow(IsolationAssemblies);
                this.SaveAssembly = new Button();
                this.SaveAssembly.Text = "Save Assembly";
                this.SaveAssembly.Click += this.SaveIso_Click;
                STL.AddRow(SaveAssembly);
                this.DeleteAssembly = new Button();
                this.DeleteAssembly.Text = "Delete Assembly";
                this.DeleteAssembly.Click += this.Delete_Isolation_Click;
                STL.AddRow(DeleteAssembly);
                SaveTLBox.Content = STL;
                TLtop.AddRow(TLL, STL);
                TLLO.AddRow(TLtop);
                Label labelTL = new Label();
                labelTL.Text = "Transmission Loss (decibels lost)";
                TLLO.AddRow(labelTL);
                TLSlider = new FreqSlider(FreqSlider.bands.Octave, true);
                TLSlider.Enabled = false;
                TLSlider.MouseLeave += Acoustics_Coef_Update;
                TLLO.AddRow(TLSlider);
                this.TL_Check = new CheckBox();
                this.TL_Check.Text = "Transmissive Assembly";
                this.TL_Check.CheckedChanged += this.Trans_Check_CheckedChanged;
                TLLO.AddRow(TL_Check);
                TLLO.AddSpace();
                tabTL.Content = TLLO;

                this.Tabs.Pages.Add(this.TabMaterials);

                TabMaterials.Content = Mat_LO;

                // 
                // TabPage3
                // 
                this.TabAnalysis = new TabPage();
                this.TabAnalysis.Text = "Analysis";

                DynamicLayout ANALO = new DynamicLayout();
                ANALO.DefaultSpacing = new Size(8, 8);
                DynamicLayout AnaTop = new DynamicLayout();
                AnaTop.DefaultSpacing = new Size(8, 8);
                DynamicLayout PB = new DynamicLayout();
                PB.DefaultSpacing = new Size(8, 8);

                ParameterBox = new GroupBox();
                this.ParameterBox.Text = "Parametric Analysis";
                this.ISOCOMP = new Label();
                this.ISOCOMP.Text = "ISO-Compliant:";

                this.Parameter_Choice = new ComboBox();
                this.Parameter_Choice.Items.Add("Early Decay Time");
                this.Parameter_Choice.Items.Add("T-10");
                this.Parameter_Choice.Items.Add("T-15");
                this.Parameter_Choice.Items.Add("T-20");
                this.Parameter_Choice.Items.Add("T-30");
                this.Parameter_Choice.Items.Add("Center Time (TS)");
                this.Parameter_Choice.Items.Add("Clarity (C-50)");
                this.Parameter_Choice.Items.Add("Clarity (C-80)");
                this.Parameter_Choice.Items.Add("Definition (D-50)");
                this.Parameter_Choice.Items.Add("Strength/Loudness (G)");
                this.Parameter_Choice.Items.Add("Sound Pressure Level (SPL)");
                this.Parameter_Choice.Items.Add("Initial Time Delay Gap (ITDG)");
                this.Parameter_Choice.Items.Add("Speech Transmission Index (Explicit)");
                this.Parameter_Choice.Items.Add("Modulation Transfer Index (MTI - root STI)");
                this.Parameter_Choice.Items.Add("Lateral Fraction (LF)");
                this.Parameter_Choice.Items.Add("Lateral Efficiency (LE)");
                this.Parameter_Choice.Items.Add("Interaural Cross-Correlation (Early)");
                this.Parameter_Choice.Items.Add("Interaural Cross-Correlation (Late)");
                this.Parameter_Choice.Items.Add("Echo Criterion (Music, 10%)");
                this.Parameter_Choice.Items.Add("Echo Criterion (Music, 50%)");
                this.Parameter_Choice.Items.Add("Echo Criterion (Speech, 10%)");
                this.Parameter_Choice.Items.Add("Echo Criterion (Speech, 50%)");
                this.Parameter_Choice.SelectedValue = "Select Parameter...";
                this.Parameter_Choice.SelectedValueChanged += this.Parameter_Choice_SelectedIndexChanged;

                ParameterResults = new ParameterResultsView();
                ParameterResults.BandChanged += (sender, args) => Update_Parameters();
                var parameterHeader = new TableLayout { Spacing = new Size(8, 0), Rows = { new TableRow(new TableCell(Parameter_Choice, true), ISOCOMP) } };
                PB.AddRow(parameterHeader);
                PB.Add(ParameterResults, xscale: true);

                ParameterBox.Content = PB;

                DynamicLayout SrcL = new DynamicLayout();
                SrcL.DefaultSpacing = new Size(2, 2);
                Label labelSrc = new Label();
                labelSrc.Text = "Source";
                this.SourceList = new SourceListBox();
                this.SourceList.Update += this.SourceList_MouseUp;
                SrcL.AddRow(labelSrc);
                SrcL.AddRow(SourceList);
                DynamicLayout Rec = new DynamicLayout();
                Rec.DefaultSpacing = new Size(2, 2);
                Label labelRec = new Label();
                labelRec.Text = "Receiver";
                this.Receiver_Choice = new ComboBox();
                this.Receiver_Choice.Text = "No Results Calculated...";
                this.Receiver_Choice.SelectedIndexChanged += this.Receiver_Choice_SelectedIndexChanged;
                this.Receiver_Choice.Height = 20;
                Rec.AddRow(labelRec, Receiver_Choice);

                this.AimatSrc = new Label();
                this.AimatSrc.Enabled = false;
                this.AimatSrc.Text = "Aim at Source";
                this.Source_Aim = new ComboBox();
                this.Source_Aim.Text = "None";
                this.Source_Aim.SelectedIndexChanged += this.Source_Aim_SelectedIndexChanged;
                this.Source_Aim.Height = 20;

                Label labelAlt = new Label();
                labelAlt.Text = "Altitude";
                this.Alt_Choice = new NumericStepper();
                this.Alt_Choice.DecimalPlaces = 2;
                this.Alt_Choice.MaxValue = 90;
                this.Alt_Choice.MinValue = -90;
                this.Alt_Choice.ValueChanged += this.Alt_Choice_ValueChanged;
                this.Alt_Choice.Height = 18;

                Label labelAzi = new Label();
                labelAzi.Text = "Azimuth";
                this.Azi_Choice = new NumericStepper();
                this.Azi_Choice.DecimalPlaces = 2;
                this.Azi_Choice.MaxValue = 360;
                this.Azi_Choice.MinValue = 1;
                this.Azi_Choice.ValueChanged += this.Azi_Choice_ValueChanged;
                this.Azi_Choice.Height = 18;

                SrcL.AddSpace();
                Rec.AddRow(AimatSrc, Source_Aim);
                Rec.AddRow(labelAlt, Alt_Choice);
                Rec.AddRow(labelAzi, Azi_Choice);
                SrcL.AddRow(Rec);
                AnaTop.AddRow(SrcL, ParameterBox);

                DynamicLayout Anamid = new DynamicLayout();
                Anamid.DefaultSpacing = new Size(8, 8);

                Label LabelISPaths = new Label();
                LabelISPaths.Text = "Deterministic Paths";
                this.PathCount = new Label();
                this.PathCount.Text = "Pending...";
                Anamid.AddRow(LabelISPaths, PathCount, labelAzi, Azi_Choice);

                ANALO.AddRow(AnaTop);

                analysis_tabs = new TabControl();
                TabPage Simreview = new TabPage();
                Simreview.Text = "Simulation";
                TabPage Auralization = new TabPage();
                Auralization.Text = "Auralization";
                ANALO.AddRow(analysis_tabs);
                analysis_tabs.Pages.Add(Simreview);
                analysis_tabs.Pages.Add(Auralization);
                DynamicLayout SimRev = new DynamicLayout();
                SimRev.DefaultSpacing = new Size(8,8);
                analysis_tabs.SelectedIndexChanged += Toggle_Aur_Conduits;
                analysis_tabs.SelectedIndexChanged += Update_Graph;


                this.IS_Path_Box = new PathListBox();
                this.IS_Path_Box.Update += this.IS_Path_Box_MouseUp;
                SimRev.AddRow(IS_Path_Box);

                DynamicLayout AnaGctrl = new DynamicLayout();
                AnaGctrl.DefaultSpacing = new Size(8, 8);
                this.Graph_Type = new ComboBox();
                this.Graph_Type.Items.Add("Energy Time Curve");
                this.Graph_Type.Items.Add("Pressure Time Curve");
                this.Graph_Type.Items.Add("Lateral ETC");
                this.Graph_Type.Items.Add("Lateral PTC");
                this.Graph_Type.Items.Add("Vertical ETC");
                this.Graph_Type.Items.Add("Vertical PTC");
                this.Graph_Type.Items.Add("Fore-Aft ETC");
                this.Graph_Type.Items.Add("Fore-Aft PTC");
                this.Graph_Type.SelectedIndex = 0;
                this.Graph_Type.TextChanged += this.Update_Graph;
                this.Graph_Octave = new ComboBox();
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
                this.Graph_Octave.TextChanged += this.Update_Graph;
                AnaGctrl.AddRow(Graph_Type, Graph_Octave);
                this.LockUserScale = new CheckBox();
                this.LockUserScale.Text = "Lock User Scale";
                this.LockUserScale.CheckedChanged += this.Update_Graph;
                this.Normalize_Graph = new CheckBox();
                this.Normalize_Graph.Checked = true;
                this.Normalize_Graph.Text = "Normalize To Direct";
                this.Normalize_Graph.CheckedChanged += this.Normalize_Graph_CheckedChanged;
                Button Add_reference = new Button();
                Add_reference.Text = "Add In-Situ Reference";
                Add_reference.Click += this.Add_reference_Click;
                AnaGctrl.AddRow(LockUserScale, Normalize_Graph, Add_reference);
                SimRev.AddRow(AnaGctrl);
                this.Analysis_View = new ScottPlot.Eto.EtoPlot();
                this.Analysis_View.Size = new Size(-1, 250);
                SimRev.AddRow(Analysis_View);
                this.IR_Parts_Analysis = new GroupBox();
                DynamicLayout Parts = new DynamicLayout();
                Specular_Part = new CheckBox();
                Specular_Part.CheckedChanged += Update_Graph;
                Specular_Part.Text = "Show Specular Reflections";
                Diffuse_Part = new CheckBox();
                Diffuse_Part.CheckedChanged += Update_Graph;
                Diffuse_Part.Text = "Show Diffuse Reflections";
                Specular_postdiffuse_Part = new CheckBox();
                Specular_postdiffuse_Part.CheckedChanged += Update_Graph;
                Specular_postdiffuse_Part.Text = "Show Specular Reflections After Diffusion";
                Parts.AddRow(Specular_Part);
                Parts.AddRow(Diffuse_Part);
                Parts.AddRow(Specular_postdiffuse_Part);
                IR_Parts_Analysis.Text = "IR Reflection Analysis (omni only)";
                IR_Parts_Analysis.Content = Parts;
                SimRev.AddRow(IR_Parts_Analysis);
                IR_Parts_Analysis.Visible = false;
                Receiver_Rose = new GroupBox();
                Show_Rec_Rose = new CheckBox();
                Show_Rec_Rose.CheckedChanged += Update_Rose;
                Show_Rec_Rose.Text = "Show Receiver Rose";
                Show_Rec_Rose.Checked = false;
                Receiver_Rose.Text = "Receiver Rose Time Window";
                RR_tstart = new NumericStepper();
                RR_tstart.DecimalPlaces = 0;
                RR_tstart.MaxValue = 1000;
                RR_tstart.ValueChanged += Update_Rose;
                RR_Width = new NumericStepper();
                RR_Width.DecimalPlaces = 0;
                RR_Width.MaxValue = 1000;
                RR_Width.Value = 50;
                RR_Width.ValueChanged += Update_Rose;
                DynamicLayout RR = new DynamicLayout();
                Label RRstart = new Label();
                Label RRend = new Label();
                RRstart.Text = "Window Start (ms)";
                RRend.Text = "Window breadth (ms)";
                RR.AddRow(Show_Rec_Rose, RRstart, RR_tstart);
                RR.AddRow(null, RRend, RR_Width);
                Receiver_Rose.Content = RR;
                SimRev.AddRow(Receiver_Rose);
                Simreview.Content = SimRev;

                this.Normalization_Choice = new NumericStepper();
                this.Graph_Aur_Octave = new DropDown();
                this.RenderBtn = new Button();
                this.Auralization_View = new ScottPlot.Eto.EtoPlot();
                this.DryChannel = new NumericStepper();
                this.Signal_Status = new TextBox();
                this.OpenSignal = new Button();
                this.Export_Filter = new Button();
                this.Sample_Freq_Selection = new DropDown();
                //this.PlayAuralization = new CheckBox();

                this.Save_Channels = new Button();
                this.Remove_Channel = new Button();
                this.Move_Down = new Button();
                this.DistributionType = new DropDown();
                this.Move_Up = new Button();
                this.Channel_View = new ListBox();
                this.Add_Channel = new Button();

                DynamicLayout Aur_Layout = new DynamicLayout();
                Aur_Layout.DefaultSpacing = new Size(8, 8);
                DynamicLayout TOA_DL = new DynamicLayout();
                TOA_DL.DefaultSpacing = new Size(8, 8);
                Label dist_label = new Label();
                dist_label.Text = "Type of Auralisation:";
                this.DistributionType.Items.Add("Monaural");
                this.DistributionType.Items.Add("Stereo");
                this.DistributionType.Items.Add("Binaural (select file...)");
                this.DistributionType.Items.Add("Surround Array via VBAP...");
                this.DistributionType.Items.Add("A-Format (type I-A)");
                this.DistributionType.Items.Add("A-Format (type II-A)");
                this.DistributionType.Items.Add("First Order Ambisonics (ACN+SN3D)");
                this.DistributionType.Items.Add("First Order Ambisonics (FuMa+SN3D)");
                this.DistributionType.Items.Add("Second Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Second Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.Items.Add("Third Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Third Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.Items.Add("Fourth Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Fourth Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.Items.Add("Fifth Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Fifth Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.Items.Add("Sixth Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Sixth Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.Items.Add("Seventh Order Ambisonics(ACN+SN3D)");
                this.DistributionType.Items.Add("Seventh Order Ambisonics(FuMa+SN3D)");
                this.DistributionType.SelectedIndex = 1;
                this.DistributionType.SelectedValueChanged += this.DistributionType_SelectedIndexChanged;

                ////////////////
                TOA_DL.AddRow(dist_label, DistributionType);
                Aur_Layout.AddRow(TOA_DL);
                Label ChLbl = new Label();
                ChLbl.Text = "Channels:";
                Aur_Layout.AddRow(ChLbl);

                this.Channel_View.Items.Add(channel.Left(0));
                this.Channel_View.Items.Add(channel.Right(1));
                this.Channel_View.Height = 50;
                Aur_Layout.AddRow(Channel_View);

                DynamicLayout buttons = new DynamicLayout();
                buttons.DefaultSpacing = new Size(8, 8);

                this.Move_Up.Text = "Move Up";
                this.Move_Up.Visible = false;
                this.Move_Up.Click += this.Move_Up_Click;

                this.Move_Down.Enabled = false;
                this.Move_Down.Text = "Move Down";
                this.Move_Down.Visible = false;
                this.Move_Down.Click += this.Move_Down_Click;

                this.Add_Channel.Enabled = false;
                this.Add_Channel.Text = "Add";
                this.Add_Channel.Visible = false;
                this.Add_Channel.Click += this.Add_Channel_Click;

                this.Remove_Channel.Enabled = false;
                this.Remove_Channel.Text = "Remove";
                this.Remove_Channel.Visible = false;
                this.Remove_Channel.Click += this.Remove_Channel_Click;

                buttons.AddRow(Add_Channel, Move_Up, Move_Down, Remove_Channel);
                Aur_Layout.AddRow(buttons);

                this.Save_Channels.Enabled = false;
                this.Save_Channels.Text = "Save Configuration";
                this.Save_Channels.Visible = false;
                this.Save_Channels.Click += this.Save_Channels_Click;
                Aur_Layout.AddRow(Save_Channels);

                /////////////
                Label GainLbl = new Label();
                GainLbl.Text = "Max Output (dB):";
                this.Normalization_Choice.DecimalPlaces = 2;
                this.Normalization_Choice.MaxValue = 200;
                this.Normalization_Choice.MinValue = 0;
                DynamicLayout GraphCtrls = new DynamicLayout();
                GraphCtrls.DefaultSpacing = new Size(4, 4);
                DynamicLayout TL = new DynamicLayout();
                TL.DefaultSpacing = new Size(8, 8);
                SourceList.Size = new Size(-1, 45);
                TL.AddRow(SourceList);

                this.Graph_Aur_Octave.Items.Add("62.5 Hz.");
                this.Graph_Aur_Octave.Items.Add("125 Hz.");
                this.Graph_Aur_Octave.Items.Add("250 Hz.");
                this.Graph_Aur_Octave.Items.Add("500 Hz.");
                this.Graph_Aur_Octave.Items.Add("1 kHz.");
                this.Graph_Aur_Octave.Items.Add("2 kHz.");
                this.Graph_Aur_Octave.Items.Add("4 kHz.");
                this.Graph_Aur_Octave.Items.Add("8 kHz.");
                this.Graph_Aur_Octave.Items.Add("Summation: All Octaves");
                this.Graph_Aur_Octave.SelectedIndex = 8;
                this.Graph_Aur_Octave.SelectedIndexChanged += this.Graph_Octave_SelectedIndexChanged;
                this.Graph_Aur_Octave.Size = new Size(-1, 25);

                GraphCtrls.AddRow(GainLbl, Normalization_Choice, Graph_Aur_Octave);
                Aur_Layout.AddRow(GraphCtrls);
                Aur_Layout.AddRow(Auralization_View);
                Auralization_View.Size = new Size(-1, 200);

                DynamicLayout OC = new DynamicLayout();
                OC.DefaultSpacing = new Size(4, 4);
                this.OpenSignal.Text = "Open...";
                this.OpenSignal.Width = 100;
                this.OpenSignal.Click += this.OpenSignal_Click;
                Label CH_Lbl = new Label();
                CH_Lbl.Text = "Selected Channel";
                CH_Lbl.Width = 75;
                DryChannel.Width = 75;
                DryChannel.Height = 20;
                Signal_Status.Height = 20;
                OpenSignal.Height = 20;
                OC.AddRow(OpenSignal, Signal_Status, CH_Lbl, DryChannel);
                Aur_Layout.AddRow(OC);
                this.Signal_Status.ReadOnly = true;

                DynamicLayout FSLO = new DynamicLayout();

                Label FS_lbl = new Label();
                FS_lbl.Text = "Sample Frequency:";

                this.Sample_Freq_Selection.Items.Add("22050 Hz.");
                this.Sample_Freq_Selection.Items.Add("44100 Hz.");
                this.Sample_Freq_Selection.Items.Add("48000 Hz.");
                this.Sample_Freq_Selection.Items.Add("96000 Hz.");
                this.Sample_Freq_Selection.Items.Add("192000 Hz.");
                this.Sample_Freq_Selection.Items.Add("384000 Hz.");
                this.Sample_Freq_Selection.Width = 100;
                FSLO.AddRow(FS_lbl, Sample_Freq_Selection);

                this.Export_Filter.Text = "Export IR as Wave File";
                this.Export_Filter.Click += this.ExportFilter;
                DynamicLayout FE = new DynamicLayout();
                FE.DefaultSpacing = new Size(8, 8);
                FE.AddRow(FSLO, Export_Filter);

                this.RenderBtn.Enabled = false;
                this.RenderBtn.Text = "Render Auralization";
                this.RenderBtn.Click += this.RenderBtn_Click;

                //this.PlayAuralization.Text = "Play Rendering";
                //this.PlayAuralization.Width = 100;
                //this.PlayAuralization.CheckedChanged += this.PlayAuralization_CheckedChanged;
                //DynamicLayout RP = new DynamicLayout();
                //RP.DefaultSpacing = new Size(8, 8);
                //FE.AddRow(PlayAuralization, RenderBtn);
                FE.AddRow(null ,RenderBtn);
                //Aur_Layout.Add(RP);
                Aur_Layout.AddRow(FE);
                //DynamicScrollable Aur_scroll = new DynamicScrollable();
                //Aur_scroll.Content = Aur_Layout;
                Auralization.Content = Aur_Layout;

                A = new AuralisationConduit();

                DistributionType.SelectedIndex = 1;
                DistributionType_SelectedIndexChanged(this, new EventArgs());
                Sample_Freq_Selection.SelectedIndex = 1;
                ///////////

                this.TabAnalysis.Content = ANALO;
                this.Tabs.Pages.Add(this.TabAnalysis);

                HybridLO.AddRow(Tabs);
                HybridLO.AddRow();
                Scrollable OverallScroll = new Scrollable();
                OverallScroll.Content = HybridLO;
                this.Content = OverallScroll;

                Load_Library();
                Instance = this;
                Rhino.RhinoDoc.LayerTableEvent += Load_Library_Event;
                Linear_Phase = Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System;
            }

            double[][] Ref_Schroeder = null;
            double[] ReferencePressure;
            private void Add_reference_Click(object sender, EventArgs e)
            {
                Eto.Forms.OpenFileDialog ofd = new Eto.Forms.OpenFileDialog();
                ofd.Filters.Add(new FileFilter("Wave Files", ".wav"));
                ofd.CurrentFilterIndex = 0;
                if (ofd.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == DialogResult.Ok)
                {
                    int fs;
                    double[][] ir = Audio.Pach_SP.Wave.ReadtoDouble(ofd.FileName, true, out fs);
                    if (ir[0].Length / fs > 45) return;
                    if (fs != 44100) ir[0] = Audio.Pach_SP.Resample(ir[0], fs, 44100, 0, true);

                    ReferencePressure = ir[0];
                    RebuildReference();
                    Update_Graph(null, null);
                }
            }

            private void RebuildReference()
            {
                if (ReferencePressure == null) return;
                bool third = Direct_Data != null && Direct_Data.Length > 0 && Direct_Data[0] != null && Direct_Data[0].ThirdOctave;
                int bandCount = third ? 24 : 8;
                    Ref_Schroeder = new double[bandCount + 1][];
                    for (int oct = 0; oct < bandCount; oct++)
                    {
                        double[] BP = third ? Audio.Pach_SP.FIR_Third_Bandpass(ReferencePressure, oct, 44100, 0) : Audio.Pach_SP.FIR_Bandpass(ReferencePressure, oct, 44100, 0);
                        for (int i = 0; i < BP.Length; i++) BP[i] = BP[i] * BP[i];
                        Ref_Schroeder[oct] = AcousticalMath.Schroeder_Integral(BP, (int)(44100 * CutoffTime / 1000));
                        for (int i = 0; i < Ref_Schroeder[oct].Length; i++) Ref_Schroeder[oct][i] = 10 * Math.Log10(Ref_Schroeder[oct][i] + 1E-10);
                    }
                    double[] BP_ = new double[ReferencePressure.Length];
                    for (int i = 0; i < BP_.Length; i++) BP_[i] = ReferencePressure[i] * ReferencePressure[i];
                    Ref_Schroeder[bandCount] = AcousticalMath.Schroeder_Integral(BP_, (int)(44100 * CutoffTime / 1000));
                    for (int i = 0; i < Ref_Schroeder[bandCount].Length; i++) Ref_Schroeder[bandCount][i] = 10 * Math.Log10(Ref_Schroeder[bandCount][i] + 1E-10);
            }

            public void Toggle_Aur_Conduits(object sender, System.EventArgs e)
            {
                if (AuralisationConduit.Instance != null)
                {
                    if (analysis_tabs.SelectedIndex == 1) AuralisationConduit.Instance.Enabled = true;
                    else AuralisationConduit.Instance.Enabled= false;
                }
            }

            private void Load_Library_Event(object sender, Rhino.DocObjects.Tables.LayerTableEventArgs e)
            {
                Load_Library();
            }

            private void Load_Library()
            {
                if (LayerDisplay.SelectedIndex < 0) LayerDisplay.SelectedIndex = 0;
                LayerPaths.Clear();

                object Selection = LayerDisplay.SelectedValue;
                LayerDisplay.Items.Clear();
                for (int q = 0; q < Rhino.RhinoDoc.ActiveDoc.Layers.Count; q++)
                {
                    if (!Rhino.RhinoDoc.ActiveDoc.Layers[q].IsDeleted) LayerPaths.Add(new layer(Rhino.RhinoDoc.ActiveDoc.Layers[q].FullPath, Rhino.RhinoDoc.ActiveDoc.Layers[q].Id));
                }
                foreach (layer l in LayerPaths) LayerDisplay.Items.Add(l.path.ToString());
                if (Selection == null) LayerDisplay.SelectedIndex = 0;
                else LayerDisplay.SelectedValue = Selection;

                Material_Lib.Items.Clear();
                string[] abs = Materials.Names_Abs().ToArray();
                foreach(string a in abs) Material_Lib.Items.Add(a);
                Isolation_Lib.Items.Clear();
                string[] TL = Materials.Names_TL().ToArray();
                foreach (string t in TL) Isolation_Lib.Items.Add(t);
            }

            ///<summary>Gets the only instance of the PachydermAcoustic plug-in.</summary>
            public static PachHybridControl Instance
            {
                get;
                private set;
            }

            #region Tab 1: Impulse Response... 
            //PachyDerm needs these common objects... 
            private Source[] Source = null;
            private Receiver_Bank[] Receiver = null;
            private Hare.Geometry.Point[] Recs;
            public int SampleRate = 44100;
            public double CutoffTime;
            List<System.Guid> ShownPaths = new List<Guid>();
            bool Linear_Phase = false;
            //bool Pressure_Ready;

            //To Begin Calculation... 
            PachydermAc_PlugIn plugin = PachydermAc_PlugIn.Instance;
            Direct_Sound[] Direct_Data = null;
            ImageSourceData[] IS_Data = null;
            //List<string> SrcTypeList = new List<string>();

            private async void Calculate_Click(object sender, System.EventArgs e)
            {
                try
                {
                SampleRate = 44100;
                IR_Parts_Analysis.Visible = false;
                string SavePath = null;
                CutoffTime = (double)this.CO_TIME.Value;
                RR_tstart.MaxValue = CO_TIME.Value;

                if (PachydermAc_PlugIn.SaveResults)
                {
                    Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                    sf.Filters.Add(new FileFilter("Pachyderm Ray Data file (.pac1)", ".pac1"));
                    sf.CurrentFilterIndex = 0;
                    if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                    {
                        SavePath = sf.FileName;
                    }
                }
                for (int i = 0; i < SourceList.Count; i++) 
                {
                    Source_Aim.Items.Add(i.ToString());
                    SourceList.Set_Src_Checked(i, false); 
                }

                SourceList.Clear();
                Source_Aim.Items.Clear();
                Receiver_Choice.SelectedIndex = 0;

                List<Hare.Geometry.Point> R;
                plugin.Receiver(out R);
                Recs = new Hare.Geometry.Point[R.Count];
                for (int q = 0; q < R.Count; q++)
                {
                    Recs[q] = R[q];
                }

                IS_Data = null;
                Direct_Data = null;
                List<Hare.Geometry.Point> RPT;

                if (!(plugin.Receiver(out RPT) && plugin.Source(out Source, thirdOctave: SimulationThirdOctave.Checked)))
                {
                    Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                    return;
                }

                List<Hare.Geometry.Point> SPT = new List<Hare.Geometry.Point>();
                foreach (Source S in Source)
                {
                    S.AppendPts(ref SPT);
                }
                IS_Data = new ImageSourceData[Source.Length];
                Direct_Data = new Direct_Sound[Source.Length];

                Calculate.Enabled = false;
                IS_Path_Box.Clear();

                List<Hare.Geometry.Point> P = new List<Hare.Geometry.Point>();
                P.AddRange(RPT);
                P.AddRange(SPT);

                Polygon_Scene PScene = RCPachTools.Get_Poly_Scene(MediumProps.RelHumidity, this.BTM_ED.Checked.Value, MediumProps.Temp_Celsius, MediumProps.StaticPressure_hPa, MediumProps.Atten_Method.SelectedIndex, MediumProps.Edge_Frequency);
                if (PScene == null || !PScene.Complete)
                {
                    CancelCalc();
                    return;
                }
                PScene.partition(P);

                ///Needed as long as there are issues with compound reflections...
                for (int i = 0; i < PScene.ObjectCount; i++)
                {
                    if (!PScene.IsPlanar(i))
                    {
                        if (ISBox.Checked.Value)
                        {
                            if ((int)Image_Order.Value > 1 || Specular_Trace.Checked == true)
                            {
                                Eto.Forms.MessageBox.Show("You have started a simulation with higher order image source in a model with curves. This version of Pachyderm uses an experimental method for deterministic curved reflections. At this time, it is not possible to perform the operation on more than one reflection. Even when it is possible, it will be prohibitively processor intensive. This simulation will be canceled, but you can proceed with an image source order of 1, or by turning image source off altogether. As always, scrutinize the results carefully, and email ORASE (info@orase.org) with any questions or concerns.", "Temporary Alert");
                                CancelCalc();
                                return;
                            }
                            else
                            {

                                DialogResult DR = MessageBox.Show("You have started a simulation with image source enabled in a model with curves. This version of Pachyderm uses an experimental method for deterministic curved reflections. Proceed with caution. If you would not like to participate in testing of this experimental feature, you can run your simulation with Image Source disabled for a more reliable result. Would you like to continue to run this simulation?", "Temporary Alert", MessageBoxButtons.YesNo);
                                if (DR == DialogResult.Yes)
                                {
                                    Eto.Forms.MessageBox.Show("As always, scrutinize the results carefully, and email ORASE (info@orase.org) with any questions or concerns.", "Temporary Alert");
                                    break;
                                }
                                else
                                {
                                    CancelCalc();
                                    return;
                                }
                            }
                        }
                    }
                }

                if (this.BTM_ED.Checked.Value)
                {
                    if ((int)Image_Order.Value > 1)
                    {
                        Eto.Forms.MessageBox.Show("You have started a simulation with higher order image source, and enabled edge diffraction. This version of Pachyderm does not have the capability of finding second order or higher edge reflections. Even when it is possible, it will be prohibitively expensive in terms of data and processor time. This simulation will be canceled, but you can proceed with an image source order of 1, or by turning image source off altogether. As always, scrutinize the results carefully, and email ORASE (info@orase.org) with any questions or concerns.", "Temporary Alert");
                        CancelCalc();
                        return;
                    }
                }

                double[] s_total = new double[8];
                double[] a_total = new double[8];
                double total_area = 0;
                bool reviewedSCT = false;
                bool reviewedABS = false;
                ///User competence checks...
                for (int i = 0; i < PScene.AbsorptionValue.Count; i++)
                {
                    double area = PScene.SurfaceArea(i);
                    total_area += area;

                    for (int oct = 0; oct < 8; oct++)
                    {
                        double a = PScene.AbsorptionValue[i].Coefficient_A_Broad(oct);
                        if (!reviewedABS && a == 0)
                        {
                            reviewedABS = true;
                            Eto.Forms.MessageBox.Show("Your model has absorption coefficients of zero. Not only is this unrealistic, it will prevent your simulation from finishing in a timely manner. Please set more realistic absorption coefficients.", "Absorption Coefficient Alert");
                        }
                        a_total[oct] += area * a;
                    }
                    for (int oct = 0; oct < 8; oct++)
                    {
                        double s = PScene.ScatteringValue[i].Coefficient(oct);
                        s_total[oct] += area * s;
                        if (!reviewedSCT)
                        {
                            DialogResult DR;
                            if (s == 0)
                            {
                                reviewedSCT = true;
                                DR = Eto.Forms.MessageBox.Show("Your model has scattering coefficients of zero. There are cases where a simulation of this kind can be useful, but the user is warned that all surfaces scatter at least a little bit. Very low scattering only occurs with very flat, smooth, uniformly heavy surfaces, such as steel or polished, painted concrete. We recommend that you use the guides on the scattering pane of the materials control. Would you like to proceed with these coefficients?", "Zero Scattering Alert", MessageBoxButtons.YesNo);
                            }
                            else if (s < 0.1)
                            {
                                reviewedSCT = true;
                                DR = Eto.Forms.MessageBox.Show("Your model has very low scattering coefficients. The user is advised that rooms where these coefficients are valid are very unusual. Very low scattering only occurs with very flat, smooth, uniformly heavy surfaces, such as steel or polished, painted concrete, as might be found in a reverberation chamber. We recommend that you use the guides on the scattering pane of the materials control. Would you like to proceed with these coefficients?", "Low Scattering Alert", MessageBoxButtons.YesNo);
                            }
                            else continue;

                            if (DR != DialogResult.Yes)
                            {
                                CancelCalc();
                                return;
                            }
                        }
                    }
                }

                for(int oct = 0; oct < 8; oct++) 
                {
                    s_total[oct] /= total_area;
                    a_total[oct] /= total_area;
                    if (s_total[oct] < 0.15)
                    {
                        DialogResult DR;
                        if (!reviewedSCT)
                        {
                            reviewedSCT = true;
                            DR = MessageBox.Show("The total scattering of this model is very low. Are you sure?", "Low Total Scattering", MessageBoxButtons.YesNo);
                            if (DR != DialogResult.Yes)
                            {
                                CancelCalc();
                                return;
                            }
                        }
                    }
                    if (a_total[oct] < 0.1)
                    {
                        if (!reviewedABS)
                        {
                            reviewedABS = true;
                            DialogResult DR = MessageBox.Show("The total absorption of this model is very low. There are cases where this is appropriate, but please be aware that this simulation may take a very long time to run. Are you sure?", "Low Total Absorption", MessageBoxButtons.YesNo);
                            if (DR != DialogResult.Yes)
                            {
                                CancelCalc();
                                return;
                            }
                        }
                    }
                }

                ////////////////////////////////////////////////
                //in order to check coefficients for all polygons
                //for (int i = 0; i < PScene.Count(); i++)
                //{
                //    Hare.Geometry.Point p = PScene.polygon_centroid(i);
                    
                //    double coef = PScene.AbsorptionValue[i].Coefficient_A_Broad(4);
                //    Rhino.RhinoDoc.ActiveDoc.Objects.AddTextDot(coef.ToString(), new Point3d(p.x, p.y, p.z));
                //}
                ////////////////////////////////////////////////
                ///////////////
                if (BTM_ED.Checked.Value) PScene.Register_Edges(SPT, RPT);
                ///////////////

                Scene Flex_Scene;
                if (PachydermAc_PlugIn.Instance.Geometry_Spec == 0)
                {
                    RhCommon_Scene NScene = RCPachTools.GetNURBSScene(MediumProps.RelHumidity, MediumProps.Temp_Celsius, MediumProps.StaticPressure_hPa, MediumProps.Atten_Method.SelectedIndex, MediumProps.Edge_Frequency);
                    if (!NScene.Complete)
                    {
                        CancelCalc();
                        return;
                    }
                    NScene.partition(P, Pach_Properties.Instance.Spatial_Depth, Pach_Properties.Instance.Max_Polys_Per_Node);
                    Flex_Scene = NScene;
                }
                else
                {
                    Flex_Scene = PScene;
                }

                Receiver_Bank.Type T;

                switch ((string)ReceiverSelection.SelectedValue.ToString())
                {
                    case "1 m. Stationary Receiver":
                        T = Receiver_Bank.Type.Stationary;
                        break;
                    case "Expanding Receiver (Expanding)":
                        T = Receiver_Bank.Type.Variable;
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Receiver Object Error");
                        Calculate.Enabled = true;
                        Source = null;
                        return;
                }

                Receiver = new Receiver_Bank[Source.Length];

                for (int s = 0; s < Source.Length; s++)
                {
                    Receiver_Bank Rec = new Receiver_Bank(RPT.ToArray(), Source[s], PScene, SampleRate, CutoffTime, T, SimulationThirdOctave.Checked);
                    Direct_Sound DS = new Direct_Sound(Source[s], Rec, PScene, Enumerable.Range(0, Rec.Third_Oct ? 24 : 8).ToArray());
                    TaskAwaiter<Simulation_Type> TA = RCPachTools.RunSimulation(DS).GetAwaiter();
                    while (!TA.IsCompleted) await Task.Delay(1500);
                    DS = TA.GetResult() as Direct_Sound;

                    if (DS != null)
                    {
                        Direct_Data[s] = DS;
                        Direct_Data[s].Create_Filter();
                    }
                    else
                    {
                        CancelCalc();
                        return;
                    } 

                    if (ISBox.Checked.Value)
                    {
                        ImageSourceData IS = new ImageSourceData(Source[s], Rec, Direct_Data[s], PScene, new int[] { 0, Rec.Third_Oct ? 23 : 7 }, (int)Image_Order.Value, BTM_ED.Checked.Value, s);
                        TaskAwaiter<Simulation_Type> TISA = RCPachTools.RunSimulation(IS).GetAwaiter();
                        while (!TISA.IsCompleted) await Task.Delay(1500);
                        IS = TISA.GetResult() as ImageSourceData;

                        if (IS != null)
                        {
                            ProgressBox VB = new ProgressBox("Creating IR filters for deterinistic reflections...");
                            VB.Show();
                            IS_Data[s] = IS;
                            Populate_Sources();
                            if (!await CreateImagePressure(IS_Data[s], Source[s].SWL(), VB))
                            {
                                CancelCalc();
                                return;
                            }
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                    }

                    if (ISBox.Checked.Value && Specular_Trace.Checked.Value)
                    {
                        IS_Trace IST = new IS_Trace(Source[s], Rec, PScene, ((double)(CO_TIME.Value / 1000) * PScene.Sound_speed(0)), (int)Spec_RayCount.Value, (int)Image_Order.Value, PScene.Sound_speed(0), SampleRate);
                        TaskAwaiter<Simulation_Type> TISTA = RCPachTools.RunSimulation(IST).GetAwaiter();

                        while (!TISTA.IsCompleted) await Task.Delay(1500);
                        IST = TISTA.GetResult() as IS_Trace;

                        Populate_Sources();

                        if (IST != null)
                        {
                            ProgressBox VB = new ProgressBox("Creating IR filters for Deterministic Reflections...");
                            VB.Show(Rhino.RhinoDoc.ActiveDoc);
                            IS_Data[s].Lookup_Sequences(IST.IS_Sequences());
                            if (!await CreateImagePressure(IS_Data[s], Source[s].SWL(), VB))
                            {
                                CancelCalc();
                                return;
                            }
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                    }
                    if (RTBox.Checked.Value)
                    {
                        CancellationTokenSource CTS = new CancellationTokenSource();
                        CancellationToken CT = CTS.Token;
                        ConvergenceProgress CP = new ConvergenceProgress(CTS, 44100);
                        SplitRayTracer RT = new SplitRayTracer(Source[s], Rec, Flex_Scene, ((double)(CO_TIME.Value / 1000) * PScene.Sound_speed(0)), new int[2] { 0, Rec.Third_Oct ? 23 : 7 }, Specular_Trace.Checked.Value ? int.MaxValue : ISBox.Checked.Value ? (int)Image_Order.Value : 0, Minimum_Convergence.Checked ? -1 : DetailedConvergence.Checked ? 0 : (int)RT_Count.Value, CP);
                        if (!Spec_Rays.Checked) CP.Show();

                        EventHandler usercancel = (source, args) =>
                                                {
                                                    Rhino.RhinoApp.SetCommandPrompt("Calculation Cancelled by User... Results may be inconclusive...");
                                                    CTS.Cancel();
                                                };
                        Rhino.RhinoApp.EscapeKeyPressed += usercancel;

                        TaskAwaiter<Simulation_Type> TRTA = RCPachTools.RunSimulation(RT).GetAwaiter();
                        while (!TRTA.IsCompleted) 
                            await Task.Delay(3000);
                        RT = TRTA.GetResult() as SplitRayTracer;
                        Rhino.RhinoApp.EscapeKeyPressed -= usercancel;

                        if (RT != null)
                        {
                            Receiver[s] = RT.GetReceiver;
                            Populate_Sources();
                            ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                            VB.Show(Rhino.RhinoDoc.ActiveDoc);
                            await Task.Run(() => { Receiver[s].Create_Filter(VB); });
                            VB.Close();
                            Rhino.RhinoApp.WriteLine(string.Format("{0} Rays ({1} sub-rays) cast in {2} hours, {3} minutes, {4} seconds.", RT._currentRay.Sum(), RT._rayTotal.Sum(), RT._ts.Hours, RT._ts.Minutes, RT._ts.Seconds));
                            Rhino.RhinoApp.WriteLine("Percentage of energy lost: {0}%", RT.PercentLost);
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                        IR_Parts_Analysis.Visible = true;
                    }
                    Source[s].Lighten();
                }

                if (Source != null)
                {
                    if (SavePath != null) Utilities.FileIO.Write_Pac1(SavePath, Direct_Data, IS_Data, Receiver);
                    OpenAnalysis();
                    cleanup();
                }

                Rhino.RhinoApp.CommandPrompt = "Command:";

                ///Set a recommended minimum auralization ceiling...
                List<int> srcs = new List<int>();
                double max = 0;
                for(int i = 0; i < Source.Length; i++) srcs.Add(i);
                for (int r = 0; r < Recs.Length; r++)
                {
                    double[] Filter = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, srcs, false, true);
                    for(int i = 0; i < Filter.Length; i++) if (Filter[i] > max) max = Math.Abs(Filter[i]);
                }
                double mspl = 20 * Math.Log10(max * (float)(Math.Pow(10, 120 / 20) / Math.Pow(10, (15 / 20)))) - 144;

                Normalization_Choice.Value = Math.Max(0, -mspl);
                }
                catch (Exception error)
                {
                    Calculate.Enabled = true;
                    ReportSimulationError(error, "Simulation failed");
                }
            }

            private async Task<bool> CreateImagePressure(ImageSourceData images, double[] spectrum, ProgressBox progress)
            {
                try
                {
                    await Task.Run(() => images.Create_Filter(spectrum, 16384, progress));
                    return true;
                }
                catch (Exception error)
                {
                    ReportSimulationError(error, "Image-source pressure calculation failed");
                    return false;
                }
                finally
                {
                    progress.Close();
                }
            }

            private void ReportSimulationError(Exception error, string stage)
            {
                string details = stage + "\n" + error.ToString();
                string context = "";
                foreach (System.Collections.DictionaryEntry entry in error.Data)
                    context += "\n" + entry.Key + ": " + entry.Value;
                details += context;
                Rhino.RhinoApp.WriteLine(details);
                string message = stage + ": " + error.Message + context;
                try
                {
                    string directory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Pachyderm", "Diagnostics");
                    Directory.CreateDirectory(directory);
                    string filename = Path.Combine(directory, "simulation-error-" + DateTime.Now.ToString("yyyyMMdd-HHmmss-fff") + ".txt");
                    File.WriteAllText(filename, details);
                    message += "\n\nFull error saved to:\n" + filename;
                    Rhino.RhinoApp.WriteLine("Full error saved to: " + filename);
                }
                catch (Exception logError)
                {
                    Rhino.RhinoApp.WriteLine("Could not save the error log: " + logError.Message);
                    message += "\nThe full error is in Rhino's command history.";
                }
                Eto.Forms.MessageBox.Show(message, "Pachyderm");
            }

            private void CancelCalc()
            {
                if (Receiver == null || Receiver.Length == 0)
                {
                    Source = null;
                    Receiver = null;
                    Direct_Data = null;
                    IS_Data = null;
                }
                else
                {
                    int i;
                    for (i = 0; i < Receiver.Length; i++) if (Receiver[i] == null) break;
                    if (i == 0)
                    {
                        Source = null;
                        Receiver = null;
                        Direct_Data = null;
                        IS_Data = null;
                    }
                    else
                    {
                        Array.Resize(ref Source, i);
                        Array.Resize(ref Direct_Data, i);
                        Array.Resize(ref IS_Data, i);
                        Populate_Sources();
                    }
                }
                this.Calculate.Enabled = true;
            }

            private void Populate_Sources()
            {

                if (Source != null)
                {
                    SourceList.Clear();
                    SourceList.Populate(Direct_Data, IS_Data, Receiver);
                    foreach (Source S in Source)
                    {
                        Source_Aim.Items.Add(String.Format("S{0}-", S.Source_ID()) + S.Type());
                    }
                    SourceList.Set_Src_Checked(0, true);
                    Source_Aim.SelectedIndex = 0;
                }
                OpenAnalysis();
                Update_Graph(null, null);
            }

            private void cleanup()
            {
                //Cleanup Code 
                Calculate.Enabled = true;
                Rhino.RhinoApp.WriteLine("Calculation has been completed. Have a nice day.");
            }

            private bool updatingAnalysis;

            private void OpenAnalysis()
            {
                bool wasUpdating = updatingAnalysis;
                updatingAnalysis = true;
                try
                {
                    bool third = Direct_Data != null && Direct_Data.Length > 0 && Direct_Data[0] != null && Direct_Data[0].ThirdOctave;
                    double[] centers = third ? AcousticalMath.ThirdOctaveCenters() : AcousticalMath.OctaveCenters();
                    if (ReferencePressure != null && (Ref_Schroeder == null || Ref_Schroeder.Length != centers.Length + 1)) RebuildReference();
                    foreach (var selector in new DropDown[] { Graph_Octave, Graph_Aur_Octave })
                    {
                        if (selector.Items.Count == centers.Length + 1) continue;
                        selector.Items.Clear();
                        foreach (double center in centers) selector.Items.Add(center.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Hz.");
                        selector.Items.Add("Summation: All Bands");
                        selector.SelectedIndex = centers.Length;
                    }

                    IS_Path_Box.Clear();

                    if (IS_Data != null)
                    {
                        if (IS_Data != null && IS_Data[0] != null && Receiver_Choice.SelectedIndex > -1)
                        {
                            List<int> srcs = SourceList.SelectedSources();
                            if (srcs.Count == 0) return;
                            IS_Path_Box.Populate(IS_Data, srcs, Receiver_Choice.SelectedIndex);
                            PathCount.Text = string.Format("{0} Deterministic Reflections", IS_Path_Box.Count);
                        }
                    }
                }
                finally
                {
                    updatingAnalysis = wasUpdating;
                }
            }
            #endregion

            #region Tab 2: Materials Tab 

            private List<layer> LayerPaths = new List<layer>();
            private Acoustics_Library Materials = new Acoustics_Library();

            private void Materials_MouseEnter(object sender, System.EventArgs e)
            {
                if (Tabs.SelectedPage.Text == "Materials")
                {
                    string Selection = LayerDisplay.SelectedValue.ToString();
                    LayerPaths.Clear();
                    for (int q = 0; q < Rhino.RhinoDoc.ActiveDoc.Layers.Count; q++)
                    {
                        if (Rhino.RhinoDoc.ActiveDoc.Layers[q].IsDeleted) continue;
                        LayerPaths.Add(new layer(Rhino.RhinoDoc.ActiveDoc.Layers[q].FullPath, Rhino.RhinoDoc.ActiveDoc.Layers[q].Id));
                    }
                    LayerDisplay.Items.Clear();
                    for (int i = 0; i < LayerPaths.Count; i++) if (LayerPaths[i]!= null) LayerDisplay.Items.Add(LayerPaths[i].path);
                    LayerDisplay.SelectedValue = Selection;
                }
            }

            public class layer
            {
                public string path;
                public Guid layer_id;

                public layer(string n, Guid id)
                {
                    path = n;
                    layer_id = id;
                }

                public override string ToString()
                {
                    return path;
                }
            }

            private void Acoustics_Coef_Update(object sender, System.EventArgs e)
            {
                Commit_Layer_Acoustics();
            }

            private void SaveAbs_Click(object sender, EventArgs e)
            {
                foreach (Material MAT in Materials.Abs_List)
                {
                    if (!string.Equals(MAT.Name, Material_Name.Text, StringComparison.OrdinalIgnoreCase)) continue;

                    int si = Material_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_Abs(si);
                    Material_Lib.Items.RemoveAt(si);
                    Materials.Save_Abs_Library();

                    Load_Library();
                    break;
                }
    
                double[] Abs = ABSSlider.MaterialValues;

                Materials.Add_Unique_Abs(Material_Name.Text, Abs);
                Material_Lib.Items.Add(Material_Name.Text);
                Materials.Save_Abs_Library();

                Load_Library();
            }

            private void SaveIso_Click(object sender, EventArgs e)
            {
                foreach (Material MAT in Materials.TL_List)
                {
                    if (!string.Equals(MAT.Name, IsolationAssemblies.Text, StringComparison.OrdinalIgnoreCase)) continue;

                    int si = Isolation_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_TL(si);
                    Isolation_Lib.Items.RemoveAt(si);
                    Materials.Save_TL_Library();

                    Load_Library();
                    break;
                }

                double[] TL = TLSlider.Value;

                Materials.Add_Unique_TL(IsolationAssemblies.Text, TL);
                Isolation_Lib.Items.Add(IsolationAssemblies.Text);
                Materials.Save_TL_Library();

                Load_Library();
            }

            private void Delete_Material_Click(object sender, EventArgs e)
            {
                if (Delete_Material.Text == "Delete Materials")
                {
                    Delete_Material.Text = "Cancel Deletion Mode";
                    Save_Material.Enabled = false;
                }
                else
                {
                    Delete_Material.Text = "Delete Materials";
                    Save_Material.Enabled = true;
                }
            }

            private void Delete_Isolation_Click(object sender, EventArgs e)
            {
                if (DeleteAssembly.Text == "Delete Assembly")
                {
                    DeleteAssembly.Text = "Cancel Deletion Mode";
                    SaveAssembly.Enabled = false;
                }
                else
                {
                    DeleteAssembly.Text = "Delete Assembly";
                    SaveAssembly.Enabled = true;
                }
            }

            private void Isolation_Lib_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (DeleteAssembly.Text == "Delete Assembly")
                {
                    if (Isolation_Lib.SelectedValue == null) return;
                    string Selection = Isolation_Lib.SelectedValue.ToString();
                    Material_Name.Text = Isolation_Lib.SelectedValue.ToString();
                    Material Mat = Materials.TL_byKey(Selection);

                    TLSlider.Value = Mat.Values;
                    Commit_Layer_Acoustics();

                    Material_Mode(true);
                }
                else
                {
                    int si = Isolation_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_TL(si);
                    Isolation_Lib.Items.RemoveAt(si);
                    Materials.Save_TL_Library();

                    Load_Library();
                }
            }

            private void Material_Lib_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Delete_Material.Text == "Delete Materials")
                {
                    if (Material_Lib.SelectedValue == null) return;
                    string Selection = Material_Lib.SelectedValue.ToString();
                    Material_Name.Text = Material_Lib.SelectedValue.ToString();
                    Material Mat = Materials.Abs_byKey(Selection);
                    double[] mat = new double[Mat.Values.Length];
                    //for (int i = 0; i < Mat.Values.Length; i++) mat[i] = Mat.Values[i];

                    ABSSlider.Value = Mat.Values;
                    Commit_Layer_Acoustics();

                    Material_Mode(true);
                }
                else
                {
                    int si = Material_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_Abs(si);
                    Material_Lib.Items.RemoveAt(si);
                    Materials.Save_Abs_Library();

                    Load_Library();
                }
            }

            private void Commit_SmartMaterial(Pach_Absorption_Designer AD)
            {
                int layer_index = LayerDisplay.SelectedIndex;
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                if (AD.Is_Finite)
                {
                    layer.SetUserString("ABSType", "Buildup_Finite");
                }
                else
                {
                    layer.SetUserString("ABSType", "Buildup");
                }
                string Buildup = "";
                List<AbsorptionModels.ABS_Layer> Layers = AD.Material_Layers();
                foreach (AbsorptionModels.ABS_Layer Layer in Layers) Buildup += Layer.LayerCode();
                layer.SetUserString("BuildUp", Buildup);

                SmartMat_Display.Plot.Clear();

                Material_Mode(false);

                Retrieve_Layer_Acoustics(null, null);
            }

            private void Commit_Layer_Acoustics()
            {
                if (LayerDisplay.SelectedIndex < 0) LayerDisplay.SelectedIndex = 0;
                if ( LayerDisplay.SelectedValue.ToString().Length == 0) return;
                int layer_index = LayerDisplay.SelectedIndex;
                double[] Absd = ABSSlider.MaterialValues;
                double[] Sctd = SCATSlider.Value;
                int[] Abs = new int[Absd.Length];
                int[] Sct = new int[Sctd.Length];
                int[] Trn = null;
                for(int i = 0; i < Absd.Length; i++) Abs[i] = (int)(Absd[i]*10);
                for(int i = 0; i < Sctd.Length; i++) Sct[i] = (int)(Sctd[i]);

                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers.FindId(this.LayerPaths[layer_index].layer_id);
                double[] TL = null;
                if (Trans_Check.Checked.Value)
                {
                    Trn = new int[8];
                    double[] Trnd = TRANSSlider.Value;
                    for (int i = 0; i < Trnd.Length; i++) Trn[i] = (int)(Trnd[i]);
                    layer.SetUserString("Transmission", "");
                }
                else if (TL_Check.Checked.Value)
                {
                    TL = new double[8];
                    double[] TLd = TLSlider.Value;
                    for (int i = 0; i < TLd.Length; i++) TL[i] = (int)(TLd[i]);
                    layer.SetUserString("Transmission", PachTools.EncodeTransmissionLoss(TL));
                }

                layer.SetUserString("Acoustics", RCPachTools.EncodeAcoustics(Abs, Sct, Trn));
                //Rhino.RhinoDoc.ActiveDoc.Layers.Modify(layer, layer_index, false);
            }

            public void Set_Phase_Regime(bool Linear_Phase)
            {
                if (Direct_Data == null) return;
                if (this.Linear_Phase == Linear_Phase) return;
                this.Linear_Phase = Linear_Phase;
                if ((Linear_Phase == true && Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System) || (Linear_Phase == false && Audio.Pach_SP.Filter is Audio.Pach_SP.Minimum_Phase_System))
                {
                    for (int i = 0; i < Direct_Data.Length; i++) Direct_Data[i].Create_Filter();
                    ProgressBox VB = new ProgressBox("Creating IR Filters for Deterministic Reflections...");
                    VB.Show(Rhino.RhinoDoc.ActiveDoc);
                    for (int i = 0; i < IS_Data.Length; i++) if (IS_Data[i] != null) IS_Data[i].Create_Filter(Direct_Data[i].SWL, 16384, VB);
                    VB.change_title("Creating Impulse Responses...");
                    for (int i = 0; i < Receiver.Length; i++) Receiver[i].Create_Filter(VB);
                    VB.Close();
                    this.Linear_Phase = Linear_Phase;
                }
            }

            private void Trans_Check_CheckedChanged(object sender, EventArgs e)
            {

                if (sender == TL_Check)
                {
                    if (TL_Check.Checked.Value) Trans_Check.Checked = false;
                    else
                    {
                        int layer_index = LayerDisplay.SelectedIndex;// (LayerDisplay.SelectedItem as layer).id;
                        Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                        layer.SetUserString("Transmission", "");
                        TLSlider.Value = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    }
                }
                else
                {
                    if (Trans_Check.Checked.Value)
                    {
                        TL_Check.Checked = false;
                        //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                        int layer_index = LayerDisplay.SelectedIndex;//(LayerDisplay.SelectedItem as layer).id;
                        Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                        layer.SetUserString("Transmission", "");
                        TLSlider.Value = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    }
                }

                TRANSSlider.Enabled = Trans_Check.Checked.Value;

                TLSlider.Enabled = TL_Check.Checked.Value;

                Commit_Layer_Acoustics();
            }

            private void Retrieve_Layer_Acoustics(object sender, EventArgs e)
            {
                int layer_index = LayerDisplay.SelectedIndex;// (LayerDisplay.SelectedItem as layer).id;
                if (layer_index < 0) return;
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers.FindId(LayerPaths[layer_index].layer_id);
                string AC = layer.GetUserString("Acoustics");
                string TL = layer.GetUserString("Transmission");

                string M = layer.GetUserString("ABSType");
                if (M == "Buildup")
                {
                    Material_Mode(false);
                    string[] BU = layer.GetUserString("Buildup").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                    for (int i = 0; i < BU.Length; i++) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(BU[i]));

                    Environment.Smart_Material sm = new Smart_Material(false, Layers, 44100, 1.2, 343);
                    double[] AnglesDeg = new double[sm.Angles.Length];
                    ScottPlot.Color[] colors = new ScottPlot.Color[8] { ScottPlot.Colors.DarkRed, ScottPlot.Colors.Red, ScottPlot.Colors.Orange, ScottPlot.Colors.Yellow, ScottPlot.Colors.Green, ScottPlot.Colors.Blue, ScottPlot.Colors.BlueViolet, ScottPlot.Colors.Violet};

                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;
                    polarPlot = this.SmartMat_Display.Plot.Add.PolarAxis(1);
                    polarPlot.Circles.ForEach(x => x.LineColor = ScottPlot.Colors.Grey);
                    polarPlot.Spokes.ForEach(x => x.LineColor = ScottPlot.Colors.Grey);
                    polarPlot.Rotation = ScottPlot.Angle.FromDegrees(90);

                    for (int oct = 0; oct < 8; oct++)
                    {
                        List<ScottPlot.Coordinates> polar = new List<ScottPlot.Coordinates>();
                        for (int i = 0; i < AnglesDeg.Length; i++)
                        {
                            polar.Add(polarPlot.GetCoordinates(sm.Ang_Coef_Oct[oct][i], AnglesDeg[i]));
                        }
                        SmartMat_Display.Plot.Add.Scatter(polar, colors[oct]);
                        (SmartMat_Display.Plot.PlottableList.Last() as ScottPlot.Plottables.Scatter).MarkerStyle = ScottPlot.MarkerStyle.None;
                    }

                    SmartMat_Display.Refresh();
                    SmartMat_Display.Plot.Axes.AutoScale();
                }
                else if (M == "Buildup_Finite")
                {
                    Material_Mode(false);
                    string[] BU = layer.GetUserString("Buildup").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                    for (int i = 0; i < BU.Length; i++) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(BU[i]));

                    Environment.Smart_Material sm = new Smart_Material(false, Layers, 44100, 1.2, 343);
                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[0], ScottPlot.Colors.Red);
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[1], ScottPlot.Colors.Orange);
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[2], ScottPlot.Colors.Yellow);
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[3], ScottPlot.Colors.Green);
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[4], ScottPlot.Colors.Blue);
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[5], ScottPlot.Colors.BlueViolet);
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[6], ScottPlot.Colors.Violet);
                    SmartMat_Display.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[7], ScottPlot.Colors.Plum);
                }
                else
                {
                    Material_Mode(true);
                }

                if (!string.IsNullOrEmpty(AC))
                {
                    double[] Abs = new double[8];
                    double[] Sct = new double[8];
                    double[] Trn = new double[1];
                    RCPachTools.DecodeAcoustics(AC, ref Abs, ref Sct, ref Trn);
                    double[] AbsI = new double[Abs.Length];
                    double[] SctI = new double[Sct.Length];
                    for(int i = 0; i < AbsI.Length; i++) AbsI[i] = Abs[i] * 100;
                    for (int i = 0; i < SctI.Length; i++) SctI[i] = Sct[i] * 100;

                    ABSSlider.Value = AbsI;
                    SCATSlider.Value = SctI;
                    if (TL != "" && TL != null)
                    {
                        double[] T_Loss = PachTools.DecodeTransmissionLoss(TL);
                        TLSlider.Value = T_Loss;
                        TL_Check.Checked = true;
                    }
                    else if (Trn != null && Trn.Length == 8 && Trn.Sum() > 0)
                    {
                        double[] TrnI = new double[Trn.Length];
                        for (int i = 0; i < TrnI.Length; i++) TrnI[i] = Trn[i] * 100;
                        TRANSSlider.Value = TrnI;
                        Trans_Check.Checked = true;
                    }
                    else
                    {
                        Trans_Check.Checked = false; TL_Check.Checked = false;
                    }
                }
                else
                {
                    ABSSlider.Value = new double[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
                    SCATSlider.Value = new double[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
                    TRANSSlider.Value = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    TLSlider.Value = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                }
            }

            private void Abs_Designer_Click(object sender, EventArgs e)
            {
                Pach_Absorption_Designer AD = new Pach_Absorption_Designer();
                AD.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, this);
                int layer_index = LayerDisplay.SelectedIndex;
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];

                switch (AD.Result)
                {
                    case Pach_Absorption_Designer.AbsorptionModelResult.Random_Incidence:
                        //Assign Random Incidence Absorption Coefficients...
                        Material_Mode(true);
                        double[] abs = new double[8];
                        for (int i = 0; i < 8; i++) abs[i] = AD.RI_Absorption[i] * 100;
                        ABSSlider.Value = abs;

                        //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                        layer.SetUserString("ABSType", "Coefficients");
                        layer.SetUserString("Carbon", AD.Total_Carbon.ToString());
                        Commit_Layer_Acoustics();
                        Material_Mode(true);
                        return;
                    case Pach_Absorption_Designer.AbsorptionModelResult.Smart_Material:
                        //Store and Assign Smart Material.
                        Material_Mode(false);
                        Commit_SmartMaterial(AD);
                        layer.SetUserString("Carbon", AD.Total_Carbon.ToString());
                        //Commit_Layer_Acoustics();
                        return;
                    default:
                        return; //Do nothing
                }
            }

            private void Material_Mode(bool RI)
            {

                if (RI)
                {
                    ABSSlider.Visible = true;
                    SmartMat_Display.Visible = false;
                }
                else
                {
                    ABSSlider.Visible = false;
                    SmartMat_Display.Visible = true;
                }
            }

            #endregion

            #region Tab 3: Data Analysis 

            private void Tab_Selecting(object sender, EventArgs e)
            {
                if (Tabs.SelectedPage.Text == "Analysis")
                {

                    if (Direct_Data != null && Direct_Data[0] != null)
                    {
                        int r = Receiver_Choice.SelectedIndex;
                        Receiver_Choice.Items.Clear();

                        for (int i = 0; i < Direct_Data[0].Rec_Origin.Count(); i++)
                        {
                            Receiver_Choice.Items.Add(i.ToString());
                        }
                        Receiver_Choice.SelectedIndex = Math.Max(0, r);
                    }

                    if (IS_Data != null && IS_Data.Length > 0)
                    {
                        IS_Path_Box_MouseUp(sender, null);
                    }

                    if (Direct_Data != null && Direct_Data.Length > 0 && Direct_Data[0] != null) Update_Graph(null, new System.EventArgs());
                }
                else if (Tabs.SelectedPage.Text == "Processing")
                {
                    Update_Graph(null, new System.EventArgs());
                }
            }

            private void Parameter_Choice_SelectedIndexChanged(object sender, System.EventArgs e)
            {
                Update_Parameters();
            }

            private double[] Lateral_Pressure_Energy(double[] lateralPressure, int octave, double rhoC)
            {
                double[] energy = Audio.Pach_SP.FIR_Bandpass(lateralPressure, octave, SampleRate, 0);
                for (int i = 0; i < energy.Length; i++) energy[i] = energy[i] * energy[i] / rhoC;
                return energy;
            }

            private bool updatingParameters;
            private ParameterResultsView ParameterResults;

            private void Update_Parameters()
            {
                if (ParameterResults == null || updatingParameters) return;
                updatingParameters = true;
                try
                {
                    if (Parameter_Choice.SelectedIndex < 0) Parameter_Choice.SelectedIndex = 0;
                    string parameter = Parameter_Choice.SelectedValue.ToString();
                    int receiver = Receiver_Choice.SelectedIndex;
                    List<int> sources = SourceList.SelectedSources();
                    if (receiver < 0 || sources == null || sources.Count == 0 || Direct_Data == null || sources.Any(id => id < 0 || id >= Direct_Data.Length || Direct_Data[id] == null || receiver >= Direct_Data[id].Io.Length))
                    {
                        ParameterResults.SetResults(parameter, "Select a receiver and source.", new ParameterResult[0]);
                        return;
                    }
                    bool speechMetric = parameter == "Speech Transmission Index (Explicit)" || parameter == "Modulation Transfer Index (MTI - root STI)";
                    bool thirdData = sources.All(id => Direct_Data[id].ThirdOctave);
                    ParameterResults.ConfigureBands(thirdData, speechMetric);
                    bool third = ParameterResults.ThirdOctave;
                    double[] centers = third ? AcousticalMath.ThirdOctaveCenters() : AcousticalMath.OctaveCenters();
                    string selection = "Receiver " + (receiver + 1) + " � Sources " + string.Join(", ", sources.Select(id => id + 1)) + " � " + (third ? "Third octave" : "Octave");
                    ISOCOMP.Text = third ? "Third-octave analysis" : "ISO Compliant: " + (sources.Count == 1 && (SourceList.SourceTypes[0] == "Pseudo-Random" || SourceList.SourceTypes[0] == "Geodesic") ? "Yes" : "No");
                    bool pressure = Graph_Type.Text == "Pressure Time Curve" || Graph_Type.Text.EndsWith(" PTC", StringComparison.Ordinal);
                    double directTime = sources.Min(id => Direct_Data[id].Min_Time(receiver));
                    double lateralStart = sources.Min(id => Direct_Data[id].Min_Time(receiver) + Direct_Data[id].Delay_ms / 1000.0);
                    double[][] energy = new double[centers.Length][];
                    if (pressure)
                    {
                        double[] broadband = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, receiver, sources, false, true);
                        for (int band = 0; band < centers.Length; band++)
                        {
                            energy[band] = third ? Audio.Pach_SP.FIR_Third_Bandpass(broadband, band, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(broadband, band, SampleRate, 0);
                            double rhoC = Direct_Data[sources[0]].Rho_C[receiver];
                            for (int t = 0; t < energy[band].Length; t++) energy[band][t] *= energy[band][t] / rhoC;
                        }
                    }
                    else for (int band = 0; band < centers.Length; band++)
                        energy[band] = third ? IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false) : IR_Construction.ETCurveOctave(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false);

                    var results = new List<ParameterResult>();
                    if (speechMetric)
                    {
                        string noiseText = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                        if (string.IsNullOrEmpty(noiseText))
                        {
                            this.Enabled = false;
                            try { Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false); }
                            finally { this.Enabled = true; }
                            noiseText = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                        }
                        if (string.IsNullOrEmpty(noiseText)) { ParameterResults.SetResults(parameter, "Background noise is required.", results); return; }
                        double[] noise = noiseText.Split(',').Select(double.Parse).ToArray();
                        if (noise.Length != 8) throw new InvalidOperationException("Background noise must contain eight octave bands.");
                        if (parameter == "Speech Transmission Index (Explicit)")
                        {
                            double[] values = AcousticalMath.Speech_Transmission_Index(energy, 1.2 * 343, noise, SampleRate);
                            string[] labels = {"General (2003)", "Male", "Female"};
                            for (int i = 0; i < labels.Length; i++) results.Add(new ParameterResult(labels[i], null, values[i]));
                        }
                        else
                        {
                            // MTI uses its existing energy-based octave formulation.
                            double[][] octaveEnergy = Enumerable.Range(0,8).Select(band => IR_Construction.ETCurveOctave(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false)).ToArray();
                            double[] values = AcousticalMath.Modulation_Transfer_Index(octaveEnergy, 1.2 * 343, noise, SampleRate);
                            for (int i = 0; i < 7; i++) results.Add(new ParameterResult(centers[i+1] + " Hz", centers[i+1], values[i]));
                        }
                        ParameterResults.SetResults(parameter, selection, results);
                        return;
                    }
                    bool lateral = parameter == "Lateral Fraction (LF)" || parameter == "Lateral Efficiency (LE)";
                    double[] lateralPressure = lateral && pressure ? IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, receiver, sources, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[1] : null;
                    bool echo = parameter.StartsWith("Echo Criterion", StringComparison.Ordinal);
                    double[] echoPressure = echo ? IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, receiver, sources, false, true, null) : null;
                    bool iacc = parameter.StartsWith("Interaural", StringComparison.Ordinal);
                    double[][][] binaural = iacc ? GenBandBinauralIRs(SampleRate, third) : null;
                    for (int band = 0; band < centers.Length; band++)
                    {
                        double value;
                        string unit = ParameterUnit(parameter);
                        if (lateral)
                        {
                            bool fraction = parameter == "Lateral Fraction (LF)";
                            double[] direction;
                            if (pressure)
                            {
                                direction = third ? Audio.Pach_SP.FIR_Third_Bandpass(lateralPressure, band, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(lateralPressure, band, SampleRate, 0);
                                double rhoC = Direct_Data[sources[0]].Rho_C[receiver];
                                for (int t = 0; t < direction.Length; t++) direction[t] *= direction[t] / rhoC;
                            }
                            else direction = third ? (fraction ? IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true) : IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true))[1] : IR_Construction.ETCurveOctave_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false, fraction ? (double)Alt_Choice.Value : -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true, fraction)[1];
                            value = fraction ? AcousticalMath.Lateral_Fraction(energy[band], direction, SampleRate, lateralStart, pressure) : AcousticalMath.Lateral_Efficiency(energy[band], direction, SampleRate, lateralStart, pressure);
                        }
                        else if (echo)
                        {
                            double[] filtered = third ? Audio.Pach_SP.FIR_Third_Bandpass(echoPressure, band, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(echoPressure, band, SampleRate, 0);
                            bool ten, fifty; double[] gradient, percent;
                            // Retain legacy octave echo evaluation; evaluate speech explicitly for new third-band results.
                            AcousticalMath.EchoCriterion(filtered, SampleRate, directTime, third && parameter.Contains("Speech"), out gradient, out percent, out ten, out fifty);
                            value = (parameter.Contains("10%") ? ten : fifty) ? 1 : 0;
                        }
                        else if (iacc)
                        {
                            if (binaural == null) value = double.NaN;
                            else
                            {
                                double duration = binaural[band][0].Length / (double)SampleRate;
                                bool early = parameter.EndsWith("(Early)", StringComparison.Ordinal);
                                value = duration <= .08 ? double.NaN : AcousticalMath.InterauralCrossCorrelation(binaural[band][0], binaural[band][1], SampleRate, early ? directTime : .08, early ? .08 : duration);
                            }
                        }
                        else if (parameter == "Initial Time Delay Gap (ITDG)")
                        {
                            double[] arrivals = pressure ? (third ? IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false) : IR_Construction.ETCurveOctave(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, band, receiver, sources, false)) : energy[band];
                            value = AcousticalMath.InitialTimeDelayGap(arrivals, SampleRate);
                        }
                        else
                        {
                            double sourcePower = third ? Direct_Data[sources[0]].SWL[band] : Direct_Data[sources[0]].OctaveSWL[band];
                            value = parameter == "Strength/Loudness (G)" && sources.Count != 1 ? double.NaN : EvaluateBandParameter(parameter, energy[band], SampleRate, directTime, sourcePower, pressure);
                            // Preserve the existing first-octave T-10 calculation outside this display change.
                            if (!third && band == 0 && parameter == "T-10") value = AcousticalMath.T_X(AcousticalMath.Schroeder_Integral(energy[band]), 15, SampleRate);
                        }
                        results.Add(new ParameterResult(centers[band].ToString("0.##") + " Hz", centers[band], value, unit, echo));
                    }
                    ParameterResults.SetResults(parameter, selection, results);
                }
                finally { updatingParameters = false; }
            }

            internal sealed class ParameterResult
            {
                public string Band { get; private set; }
                public double? Frequency { get; private set; }
                public double Value { get; private set; }
                public string Unit { get; private set; }
                public bool Boolean { get; private set; }
                public string ValueText { get { return double.IsNaN(Value) || double.IsInfinity(Value) ? "NA" : Boolean ? (Value != 0 ? "Yes" : "No") : Value.ToString("0.##", CultureInfo.CurrentCulture); } }
                public ParameterResult(string band, double? frequency, double value, string unit = "", bool boolean = false)
                { Band = band; Frequency = frequency; Value = value; Unit = unit; Boolean = boolean; }
            }

            internal sealed class ParameterResultsView : Panel
            {
                readonly RadioButton octave = new RadioButton { Text = "Octave", Checked = true };
                readonly RadioButton thirds;
                readonly GridView table;
                readonly Button copy = new Button { Text = "Copy data", Enabled = false };
                readonly Button openTable = new Button { Text = "Open table�", Enabled = false };
                readonly Button openGraph = new Button { Text = "Open graph�", Enabled = false };
                readonly Label status = new Label();
                readonly ObservableCollection<ParameterResult> rows = new ObservableCollection<ParameterResult>();
                string parameter = "Parameters", context = "", unit = "";
                bool settingBands, preferredThird;
                bool? hasThirdData;
                Form tableWindow, graphWindow;
                GridView expandedTable;
                Label tableContext, graphContext;
                ScottPlot.Eto.EtoPlot graph;
                public event EventHandler BandChanged;
                public bool ThirdOctave { get { return thirds.Checked; } }

                public ParameterResultsView()
                {
                    thirds = new RadioButton(octave) { Text = "Third octave", Enabled = false };
                    octave.CheckedChanged += ChangeBand;
                    thirds.CheckedChanged += ChangeBand;
                    table = CreateTable(); table.Height = 110;
                    table.DataStore = rows;
                    copy.Click += (s, e) => CopyData();
                    openTable.Click += (s, e) => OpenTable();
                    openGraph.Click += (s, e) => OpenGraph();
                    var layout = new DynamicLayout { DefaultSpacing = new Size(6, 6) };
                    layout.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 10, Items = { octave, thirds } });
                    layout.Add(table, xscale: true);
                    layout.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { copy, openTable, openGraph } });
                    layout.AddRow(status);
                    Content = layout;
                }

                GridView CreateTable()
                {
                    var grid = new GridView { AllowMultipleSelection = false };
                    grid.Columns.Add(new GridColumn { HeaderText = "Frequency / result", Editable = false, Width = 120, DataCell = new TextBoxCell { Binding = Binding.Property<ParameterResult, string>(r => r.Band) } });
                    grid.Columns.Add(new GridColumn { HeaderText = "Value", Editable = false, Width = 85, DataCell = new TextBoxCell { Binding = Binding.Property<ParameterResult, string>(r => r.ValueText) } });
                    grid.Columns.Add(new GridColumn { HeaderText = "Unit", Editable = false, Width = 45, DataCell = new TextBoxCell { Binding = Binding.Property<ParameterResult, string>(r => r.Unit) } });
                    return grid;
                }

                void ChangeBand(object sender, EventArgs e)
                {
                    if (settingBands || !(octave.Checked && sender == octave || thirds.Checked && sender == thirds)) return;
                    preferredThird = thirds.Checked;
                    BandChanged?.Invoke(this, EventArgs.Empty);
                }

                public void ConfigureBands(bool available, bool octaveOnly)
                {
                    settingBands = true;
                    try
                    {
                        if (hasThirdData != available) preferredThird = available;
                        hasThirdData = available;
                        thirds.Enabled = available && !octaveOnly;
                        thirds.Checked = available && !octaveOnly && preferredThird;
                        octave.Checked = !thirds.Checked;
                        status.Text = octaveOnly ? "This metric uses octave bands." : "";
                    }
                    finally { settingBands = false; }
                }

                public void SetResults(string title, string selection, IEnumerable<ParameterResult> results)
                {
                    ParameterResult[] incoming = results.ToArray();
                    parameter = title; context = selection;
                    rows.Clear();
                    foreach (ParameterResult result in incoming) rows.Add(result);
                    unit = rows.Select(r => r.Unit).FirstOrDefault(x => x.Length > 0) ?? "";
                    // Refresh the embedded grid itself, without relying on a new window to trigger painting.
                    table.ReloadData(Enumerable.Range(0, rows.Count));
                    table.Invalidate();
                    copy.Enabled = openTable.Enabled = openGraph.Enabled = rows.Count > 0;
                    if (expandedTable != null) { expandedTable.ReloadData(Enumerable.Range(0, rows.Count)); expandedTable.Invalidate(); tableContext.Text = context; tableWindow.Title = parameter + " � table"; }
                    if (graph != null) { graphContext.Text = context; graphWindow.Title = parameter + " � graph"; RefreshGraph(); }
                }

                internal string CopyText()
                {
                    return parameter + "\t" + context + "\nFrequency / result\tValue\tUnit\n" + string.Join("\n", rows.Select(r => r.Band + "\t" + (double.IsNaN(r.Value) || double.IsInfinity(r.Value) ? "NA" : r.Boolean ? r.ValueText : r.Value.ToString("G17", CultureInfo.InvariantCulture)) + "\t" + r.Unit));
                }
                void CopyData() { Clipboard.Instance.Text = CopyText(); }

                internal void OpenTable()
                {
                    if (tableWindow != null) { tableWindow.BringToFront(); return; }
                    expandedTable = CreateTable(); expandedTable.DataStore = rows;
                    tableContext = new Label { Text = context, Wrap = WrapMode.Word };
                    var copyButton = new Button { Text = "Copy data" }; copyButton.Click += (s, e) => CopyData();
                    var layout = new DynamicLayout { Padding = 12, DefaultSpacing = new Size(8, 8) };
                    layout.AddRow(tableContext); layout.Add(expandedTable, xscale: true, yscale: true); layout.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Items = { copyButton } });
                    tableWindow = new Form { Title = parameter + " � table", ClientSize = new Size(440, 740), MinimumSize = new Size(340, 260), Resizable = true, Content = layout };
                    tableWindow.Closed += (s, e) => { tableWindow = null; expandedTable = null; tableContext = null; };
                    tableWindow.Show();
                }

                internal void OpenGraph()
                {
                    if (graphWindow != null) { graphWindow.BringToFront(); return; }
                    graph = new ScottPlot.Eto.EtoPlot(); graphContext = new Label { Text = context, Wrap = WrapMode.Word };
                    var save = new Button { Text = "Save image�" }; save.Click += (s, e) =>
                    {
                        var dialog = new Eto.Forms.SaveFileDialog { Title = "Save parameter graph", FileName = "parameter-graph.png" };
                        dialog.Filters.Add(new FileFilter("PNG image", ".png"));
                        if (dialog.ShowDialog(graphWindow) == DialogResult.Ok)
                            graph.Plot.SavePng(dialog.FileName, Math.Max(640, graph.Width), Math.Max(400, graph.Height));
                    };
                    var layout = new DynamicLayout { Padding = 12, DefaultSpacing = new Size(8, 8) };
                    layout.AddRow(graphContext); layout.Add(graph, xscale: true, yscale: true); layout.AddRow(new StackLayout { Orientation = Orientation.Horizontal, Items = { save } });
                    graphWindow = new Form { Title = parameter + " � graph", ClientSize = new Size(800, 520), MinimumSize = new Size(480, 320), Resizable = true, Content = layout };
                    graphWindow.Closed += (s, e) => { graphWindow = null; graph = null; graphContext = null; };
                    RefreshGraph(); graphWindow.Show();
                }

                void RefreshGraph()
                {
                    graph.Plot.Clear();
                    bool spectral = rows.Count > 0 && rows.All(r => r.Frequency.HasValue);
                    double[] x = rows.Select((r, i) => spectral ? Math.Log10(r.Frequency.Value) : (double)i).ToArray();
                    double[] y = rows.Select(r => double.IsInfinity(r.Value) ? double.NaN : r.Value).ToArray();
                    if (y.Any(v => !double.IsNaN(v))) graph.Plot.Add.Scatter(x, y);
                    int[] ticks = Enumerable.Range(0, rows.Count).Where(i => !spectral || rows.Count <= 8 || i == 0 || i == rows.Count - 1 || i % 3 == 1).ToArray();
                    graph.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks.Select(i => x[i]).ToArray(), ticks.Select(i => spectral ? rows[i].Frequency.Value.ToString("0.##", CultureInfo.InvariantCulture) : rows[i].Band).ToArray());
                    graph.Plot.XLabel(spectral ? "Frequency (Hz, logarithmic scale)" : "Result");
                    graph.Plot.YLabel(parameter + (unit.Length > 0 ? " (" + unit + ")" : ""));
                    graph.Plot.Title(parameter);
                    if (rows.Count > 0 && rows.All(r => r.Boolean)) graph.Plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual(new[] { 0.0, 1.0 }, new[] { "No", "Yes" });
                    else graph.Plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericAutomatic();
                    graph.Plot.Axes.AutoScale(); graph.Refresh();
                }

                public void CloseWindows() { tableWindow?.Close(); graphWindow?.Close(); }
            }

            internal static string ParameterUnit(string parameter)
            {
                if (parameter == "Early Decay Time" || parameter.StartsWith("T-", StringComparison.Ordinal)) return "s";
                if (parameter == "Center Time (TS)" || parameter == "Initial Time Delay Gap (ITDG)") return "ms";
                if (parameter == "Definition (D-50)") return "%";
                if (parameter.StartsWith("Clarity", StringComparison.Ordinal) || parameter == "Strength/Loudness (G)" || parameter == "Sound Pressure Level (SPL)") return "dB";
                return "";
            }

            internal static double EvaluateBandParameter(string parameter, double[] energy, int rate, double directTime, double sourcePower, bool pressure)
            {
                if (energy == null || energy.Length == 0 || !energy.Any(value => value > 0)) return double.NaN;
                double window = parameter == "Clarity (C-80)" ? .08 : parameter == "Clarity (C-50)" || parameter == "Definition (D-50)" ? .05 : 0;
                if (window > 0 && (directTime + window) * rate + (pressure ? 8192 : 0) >= energy.Length) return double.NaN;
                switch (parameter)
                {
                    case "Early Decay Time": return AcousticalMath.EarlyDecayTime(AcousticalMath.Schroeder_Integral(energy), rate);
                    case "T-10": case "T-15": case "T-20": case "T-30": return AcousticalMath.T_X(AcousticalMath.Schroeder_Integral(energy), int.Parse(parameter.Substring(2)), rate);
                    case "Center Time (TS)": return AcousticalMath.Center_Time(energy, rate, directTime, pressure) * 1000;
                    case "Clarity (C-50)": return AcousticalMath.Clarity(energy, rate, .05, directTime, pressure);
                    case "Clarity (C-80)": return AcousticalMath.Clarity(energy, rate, .08, directTime, pressure);
                    case "Definition (D-50)": return AcousticalMath.Definition(energy, rate, .05, directTime, pressure);
                    case "Strength/Loudness (G)": return AcousticalMath.Strength(energy, sourcePower, pressure);
                    case "Sound Pressure Level (SPL)": return AcousticalMath.SPL_Intensity(energy.Sum());
                    default: return double.NaN;
                }
            }

            ReceiverSphereConduit SC;

            public void Update_Rose(object sender, EventArgs e)
            {
                int receiverId = Receiver_Choice.SelectedIndex;
                int octave = Graph_Octave.SelectedIndex;
                List<int> srcIDs = SelectedSources();
                if (Show_Rec_Rose.Checked != true || receiverId < 0 || octave < 0
                    || srcIDs == null || srcIDs.Count == 0 || RR_Width.Value <= 0)
                {
                    Hide_Receiver_Rose();
                    return;
                }

                // Keep all three arrays aligned to the same selected source IDs.
                var direct = new List<Direct_Sound>();
                var images = new List<ImageSourceData>();
                var rays = new List<Receiver_Bank>();
                Hare.Geometry.Point center = null;
                foreach (int src in srcIDs)
                {
                    if (src < 0) continue;
                    Direct_Sound ds = Direct_Data != null && src < Direct_Data.Length ? Direct_Data[src] : null;
                    ImageSourceData image = IS_Data != null && src < IS_Data.Length ? IS_Data[src] : null;
                    Receiver_Bank bank = Receiver != null && src < Receiver.Length ? Receiver[src] : null;
                    direct.Add(ds);
                    images.Add(image);
                    rays.Add(bank);
                    if (center == null && ds != null && receiverId < ds.Rec_Origin.Count())
                        center = ds.Rec_Origin.ElementAt(receiverId);
                    if (center == null && bank != null && bank.Rec_List != null
                        && receiverId < bank.Rec_List.Length && bank.Rec_List[receiverId] != null)
                        center = bank.Rec_List[receiverId].Origin;
                }
                if (center == null && images.Any(image => image != null)
                    && Recs != null && receiverId < Recs.Length)
                    center = Recs[receiverId];
                if (center == null || direct.Count == 0)
                {
                    Hide_Receiver_Rose();
                    return;
                }

                Sphere_Plot plot = new Sphere_Plot(center);
                double[] levels = plot.SPL_From_IR(receiverId, octave,
                    (int)(Math.Max(0, RR_tstart.Value) * 44.1),
                    (int)((Math.Max(0, RR_tstart.Value) + RR_Width.Value) * 44.1),
                    direct.ToArray(), images.ToArray(), rays.ToArray()).ToArray();
                if (!levels.Any(level => !double.IsInfinity(level) && !double.IsNaN(level)))
                {
                    Hide_Receiver_Rose();
                    return;
                }

                if (SC == null) SC = new ReceiverSphereConduit();
                SC.Data_in(plot.Output(levels), center);
                SC.Enabled = true;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Hide_Receiver_Rose()
            {
                if (SC == null || !SC.Enabled) return;
                SC.Enabled = false;
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }


            private void IS_Path_Box_MouseUp(object sender, EventArgs e)
            {
                List<int> Srcs = SourceList.SelectedSources();
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                foreach (Guid path in ShownPaths)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Delete(path, true);
                }
                ShownPaths.Clear();
                foreach (int s in Srcs)
                {
                    //foreach (string pathid in IS_Path_Box.CheckedItems)
                    List<Deterministic_Reflection> Paths = IS_Path_Box.SelectedPaths();
                    foreach (Deterministic_Reflection Path in Paths)
                    {
                        foreach (Hare.Geometry.Point[] P in Path.Path)
                        {
                            List<Point3d> pts = new List<Point3d>();
                            foreach (Hare.Geometry.Point p in P) pts.Add(Utilities.RCPachTools.HarePointToModel(p));
                            ShownPaths.Add(Rhino.RhinoDoc.ActiveDoc.Objects.AddPolyline(new Polyline(pts)));
                        }
                    }
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                Update_Graph(null, null);
            }

            private void Update_Graph(object sender, EventArgs e)
            {
                if (updatingAnalysis) return;
                // The compact parameter table also updates when another graph tab is selected.
                Update_Parameters();
                if (analysis_tabs.SelectedIndex == 0)
                {
                    Analysis_View.Plot.Clear();

                    int REC_ID = 0;
                    try
                    {
                        if (Receiver_Choice.SelectedIndex < 0) return;
                        REC_ID = Receiver_Choice.SelectedIndex;

                        int OCT_ID = Graph_Octave.SelectedIndex;
                        if (OCT_ID < 0) return;
                        Analysis_View.Plot.Title("Logarithmic Energy Time Curve");
                        Analysis_View.Plot.XLabel("Time (seconds)", 10);
                        Analysis_View.Plot.YLabel("Sound Pressure Level (dB)", 10);

                        if (Ref_Schroeder != null && OCT_ID >= 0 && OCT_ID < Ref_Schroeder.Length)
                        {
                            int length = (int)(CutoffTime * 44100 / 1000);
                            double[] Ref = new double[length];
                            Array.Copy(Ref_Schroeder[OCT_ID], 0, Ref, 0, Math.Min(length, Ref_Schroeder[OCT_ID].Length));
                            Analysis_View.Plot.Add.Signal(Ref, 1.0/44100.0, ScottPlot.Colors.Gray);
                        }

                        List<int> SrcIDs = SourceList.SelectedSources();

                        double[] Filter;
                        double[] Schroeder;
                        double[] Filter2;
                        int zero_sample = 0;
                        switch (Graph_Type.Text)
                        {
                            case "Energy Time Curve":
                                Filter = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false);
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            case "Pressure Time Curve":
                                zero_sample = 16384 / 2;
                                Filter2 = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, true);
                                if (OCT_ID < (Direct_Data[0].ThirdOctave ? 24 : 8))
                                {
                                    Filter2 = (Direct_Data[0].ThirdOctave ? Audio.Pach_SP.FIR_Third_Bandpass(Filter2, OCT_ID, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0));
                                }
                                Filter = new double[Filter2.Length];
                                for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            case "Lateral ETC":
                                Filter = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1];
                                for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            case "Lateral PTC":
                                zero_sample = 16384 / 2;
                                Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[1];
                                if (OCT_ID < (Direct_Data[0].ThirdOctave ? 24 : 8))
                                {
                                    Filter2 = (Direct_Data[0].ThirdOctave ? Audio.Pach_SP.FIR_Third_Bandpass(Filter2, OCT_ID, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0));
                                }
                                Filter = new double[Filter2.Length];
                                for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            case "Vertical ETC":
                                Filter = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[2];
                                for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            case "Vertical PTC":
                                zero_sample = 16384 / 2;
                                Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[2];
                                if (OCT_ID < (Direct_Data[0].ThirdOctave ? 24 : 8))
                                {
                                    Filter2 = (Direct_Data[0].ThirdOctave ? Audio.Pach_SP.FIR_Third_Bandpass(Filter2, OCT_ID, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0));
                                }
                                Filter = new double[Filter2.Length];
                                for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            case "Fore-Aft ETC":
                                Filter = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[0];
                                for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            case "Fore-Aft PTC":
                                zero_sample = 16384 / 2;
                                Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[0];
                                if (OCT_ID < (Direct_Data[0].ThirdOctave ? 24 : 8))
                                {
                                    Filter2 = (Direct_Data[0].ThirdOctave ? Audio.Pach_SP.FIR_Third_Bandpass(Filter2, OCT_ID, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0));
                                }
                                Filter = new double[Filter2.Length];
                                for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                                Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        //Get the maximum value of the Direct Sound
                        double DirectMagnitude = 0;
                        foreach (int i in SrcIDs)
                        {
                            double[] E = Direct_Data[i].EnergyValue(OCT_ID, REC_ID);
                            for (int j = 0; j < E.Length; j++)
                            {
                                double D = AcousticalMath.SPL_Intensity(E[j]);
                                if (D > DirectMagnitude) DirectMagnitude = D;
                            }
                        }

                        Filter = AcousticalMath.SPL_Intensity_Signal(Filter);
                        Schroeder = AcousticalMath.SPL_Intensity_Signal(Schroeder);

                        List<double> S_time = new List<double>();
                        List<double> S_Power = new List<double>();

                        List<Deterministic_Reflection> Paths = IS_Path_Box.SelectedPaths();

                        if (Paths.Count > 0)
                        {
                            SortedList<double, double> Selected = new SortedList<double, double>();

                            foreach (Deterministic_Reflection i in Paths)
                            {
                                double[] refl = i.Energy(OCT_ID, SampleRate);
                                for (int j = 0; j < refl.Length; j++)
                                {
                                    double t = i.TravelTime + (double)j / (double)SampleRate;
                                    try
                                    {
                                        Selected.Add(t, refl[j]);
                                    }
                                    catch
                                    {
                                        Selected[t] += refl[j];
                                    }
                                }
                            }

                            for (int i = 0; i < Selected.Count; i++)
                            {
                                S_time.Add(Selected.Keys[i]);
                                S_Power.Add(AcousticalMath.SPL_Intensity(Selected.Values[i]));
                            }
                        }

                        if (Normalize_Graph.Checked.Value)
                        {
                            Filter = Utilities.AcousticalMath.Normalize_Function(Filter, S_Power);
                            Schroeder = Utilities.AcousticalMath.Normalize_Function(Schroeder);
                        }

                        double[] time = new double[Filter.Length];
                        for (int i = 0; i < Filter.Length; i++)
                        {
                            time[i] = (double)(i - zero_sample) / SampleRate;
                        }

                        Analysis_View.Plot.Add.Signal(Schroeder, 1.0 / 44100.0, ScottPlot.Colors.Red);
                        Analysis_View.Plot.Add.Signal(Filter, 1.0 / 44100.0, ScottPlot.Colors.Blue);

                        double[][] analysis_signal = new double[3][];
                        analysis_signal[0] = new double[time.Length];
                        analysis_signal[1] = new double[time.Length];
                        analysis_signal[2] = new double[time.Length];

                        if (Specular_Part.Checked == true || Diffuse_Part.Checked == true || Specular_postdiffuse_Part.Checked == true)
                        {
                            if (Receiver[0].Rec_List[0].Reflection_Analysis_Data != null)
                            {
                                foreach (int i in SrcIDs)
                                {
                                    for (int j = 0; j < Math.Min(time.Length, Receiver[i].Rec_List[REC_ID].Reflection_Analysis_Data[0][0].Length); j++)
                                    {
                                        for (int t = 0; t < 3; t++)
                                        {
                                            int count = Receiver[i].Third_Oct ? 24 : 8;
                                            int first = OCT_ID < count ? OCT_ID : 0, last = OCT_ID < count ? OCT_ID + 1 : count;
                                            for (int band = first; band < last; band++) analysis_signal[t][j] += Receiver[i].Rec_List[REC_ID].Reflection_Analysis_Data[t][band][j];
                                        }
                                    }
                                }

                                double min = Filter.Min();

                                for (int t = 0; t < 3; t++)
                                {
                                    analysis_signal[t] = AcousticalMath.SPL_Intensity_Signal(analysis_signal[t]);
                                    for (int i = 0; i < analysis_signal[t].Length; i++) analysis_signal[t][i] += min;
                                }
                                if (Specular_postdiffuse_Part.Checked == true) Analysis_View.Plot.Add.Signal(analysis_signal[2], 1.0 / 44100.0, ScottPlot.Colors.MediumPurple);
                                if (Diffuse_Part.Checked == true) Analysis_View.Plot.Add.Signal(analysis_signal[1], 1.0 / 44100.0, ScottPlot.Colors.Crimson);
                                if (Specular_Part.Checked == true) Analysis_View.Plot.Add.Signal(analysis_signal[0], 1.0 / 44100.0, ScottPlot.Colors.DarkBlue);
                            }
                        }
                        if (Paths.Count > 0)
                        {
                            ScottPlot.Plottables.Scatter IScurve = Analysis_View.Plot.Add.Scatter(S_time, S_Power, ScottPlot.Colors.Red);
                            IScurve.LineStyle.IsVisible = false;
                        }

                        if (!LockUserScale.Checked.Value)
                        {
                            Analysis_View.Plot.Axes.SetLimitsX(time[0], time[time.Length - 1]);

                            if (Normalize_Graph.Checked.Value)
                            {
                                Analysis_View.Plot.Axes.SetLimitsY(-100, 0);
                            }
                            else
                            {
                                Analysis_View.Plot.Axes.SetLimitsY(0, DirectMagnitude + 15);
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

                        Hare.Geometry.Vector V = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(float)Alt_Choice.Value, true), -(float)Azi_Choice.Value, 0, true);

                        if (Receiver_Choice.SelectedIndex >= 0) ReceiverConduit.Instance.set_direction(Utilities.RCPachTools.HarePointToModel(Recs[Receiver_Choice.SelectedIndex]), new Vector3d(V.dx, V.dy, V.dz));
                        Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                    }
                    catch (Exception x)
                    {
                        Rhino.RhinoApp.WriteLine("Analysis graph update failed:\n" + x.ToString());
                        Eto.Forms.MessageBox.Show(x.Message);
                        return;
                    }

                    Analysis_View.Invalidate();
                    Update_Rose(null, null);
                }
                else
                {
                    Auralization_View.Plot.Clear();

                    int REC_ID = 0;
                    try
                    {
                        REC_ID = Receiver_Choice.SelectedIndex;

                        int OCT_ID = Graph_Aur_Octave.SelectedIndex;
                        if (OCT_ID < 0) return;
                        Auralization_View.Plot.Title("Logarithmic Energy Time Curve");
                        Auralization_View.Plot.XLabel("Time (seconds)", 10);
                        Auralization_View.Plot.YLabel("Sound Pressure Level (dB)", 10);

                        List<int> SrcIDs = SourceList.SelectedSources();

                        Response = RenderFilter(44100);

                        List<ScottPlot.Color> C;

                        if (Response.Length == 2) // Left and right channels are easily differentiable
                        {
                            C = new List<ScottPlot.Color>
                            {
                                ScottPlot.Colors.Blue,
                                ScottPlot.Colors.Orange
                            };
                        }
                        else
                        {
                            C = new List<ScottPlot.Color>
                            {
                                ScottPlot.Colors.Red, ScottPlot.Colors.OrangeRed, ScottPlot.Colors.Orange,
                                ScottPlot.Colors.DarkGoldenRod, ScottPlot.Colors.Olive, ScottPlot.Colors.Green,
                                ScottPlot.Colors.Aquamarine, ScottPlot.Colors.Azure, ScottPlot.Colors.Blue,
                                ScottPlot.Colors.Indigo, ScottPlot.Colors.Violet
                            };
                        }

                        //Get the maximum value of the Direct Sound
                        double DirectMagnitude = 0;
                        foreach (int i in SrcIDs)
                        {
                            double[] E = Direct_Data[i].EnergyValue(OCT_ID, REC_ID);
                            for (int j = 0; j < E.Length; j++)
                            {
                                double D = AcousticalMath.SPL_Intensity(E[j]) - 150;
                                if (D > DirectMagnitude) DirectMagnitude = D;
                            }
                        }

                        double[] time = new double[Response[0].Length];
                        for (int i = 0; i < Response[0].Length; i++)
                        {
                            time[i] = (double)i / SampleRate - 2048f / SampleRate;
                        }

                        for (int i = 0; i < Response.Length; i++)
                        {
                            if (OCT_ID < (Direct_Data[0].ThirdOctave ? 24 : 8))
                            {
                                double[] filter = (Direct_Data[0].ThirdOctave ? Audio.Pach_SP.FIR_Third_Bandpass(Response[i], OCT_ID, SampleRate, 0) : Audio.Pach_SP.FIR_Bandpass(Response[i], OCT_ID, SampleRate, 0));
                                Array.Resize(ref filter, filter.Length - 12288);
                                //String.Format("Channel {0}", i)
                                Auralization_View.Plot.Add.Scatter(time, AcousticalMath.SPL_Pressure_Signal(filter), C[i % 10]);
                                (Auralization_View.Plot.PlottableList[i] as ScottPlot.Plottables.Scatter).MarkerStyle = ScottPlot.MarkerStyle.None;
                            }
                            else
                            {
                                Auralization_View.Plot.Add.Scatter(time, AcousticalMath.SPL_Pressure_Signal(Response[i]), C[i % 10]);
                                (Auralization_View.Plot.PlottableList[i] as ScottPlot.Plottables.Scatter).MarkerStyle = ScottPlot.MarkerStyle.None;
                            }
                        }
                        Auralization_View.Plot.Axes.SetLimits(time[0], time[time.Length - 1], DirectMagnitude - 100, DirectMagnitude);

                        //if (analysis_tabs.SelectedIndex == 1) Auralization_View.Plot.SetAxisLimits(AL);
                        //if (!LockUserScale.Checked.Value)
                        //{
                        //    Analysis_View.Plot.XAxis.Max = time[time.Length - 1];
                        //    Analysis_View.Plot.XAxis.Min = time[0];

                        //    if (Normalize_Graph.Checked.Value)
                        //    {
                        //        Analysis_View.Plot.YAxis.Max = 0;
                        //        Analysis_View.Plot.YAxis.Min = -100;
                        //    }
                        //    else
                        //    {
                        //        Analysis_View.Plot.YAxis.Max = DirectMagnitude + 15;
                        //        Analysis_View.Plot.YAxis.Min = 0;
                        //    }
                        //}
                        //else
                        //{
                        //    double max = Analysis_View.Plot.YAxis.Max;
                        //    double min = Analysis_View.Plot.YAxis.Min;

                        //    if (Normalize_Graph.Checked.Value)
                        //    {
                        //        Analysis_View.Plot.YAxis.Max = max;
                        //        Analysis_View.Plot.YAxis.Min = min;
                        //    }
                        //    else
                        //    {
                        //        Analysis_View.Plot.YAxis.Max = max;
                        //        Analysis_View.Plot.YAxis.Min = min;
                        //    }
                        //}

                        Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                    }
                    catch { return; }
                    Auralization_View.Invalidate();
                    Draw_Feedback();
                }
            }

            private void Normalize_Graph_CheckedChanged(object sender, EventArgs e)
            {
                LockUserScale.Checked = false;
                Update_Graph(null, new System.EventArgs());
            }

            private void SourceList_MouseUp(object sender, MouseEventArgs e)
            {
                if (e.Buttons == MouseButtons.Alternate) return;
                Update_Parameters();
                Update_Graph(null, System.EventArgs.Empty);
                OpenAnalysis();
            }

            private void SourceList_MouseUp(object sender, EventArgs e)
            {
                //if (e.Buttons == MouseButtons.Alternate) return;
                if (SourceList.SelectedSources() == null || SourceList.SelectedSources().Count < 1) return;
                Source_Aim.SelectedIndex = SourceList.SelectedSources()[0];
                OpenAnalysis();
                Update_Parameters();
                Update_Graph(null, System.EventArgs.Empty);
            }

            private void Source_Aim_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Receiver_Choice.SelectedIndex < 0 || Source_Aim.SelectedIndex < 0) return;
                double azi, alt;

                PachTools.World_Angles(Direct_Data[Source_Aim.SelectedIndex].Src.Origin, Recs[Receiver_Choice.SelectedIndex], true, out alt, out azi);

                Alt_Choice.Value = alt;
                Azi_Choice.Value = azi;

                Update_Graph(sender, e);
            }

            public void GetSims(ref Hare.Geometry.Point[] Src, ref Hare.Geometry.Point[] Rec, ref Direct_Sound[] D, ref ImageSourceData[] IS, ref Receiver_Bank[] RT)
            {
                if (Source != null)
                {
                    Src = new Hare.Geometry.Point[Source.Length];
                    for (int i = 0; i < Source.Length; i++) Src[i] = Source[i].Origin;
                    Rec = Recs;
                }
                else return;
                if (Direct_Data != null) D = Direct_Data;
                if (IS_Data != null) IS = IS_Data;
                if (Receiver != null) RT = Receiver;
            }

            public bool AuralisationReady()
            {
                if (Receiver != null) return true;
                return false;
            }

            private void CalcType_CheckedChanged(object sender, EventArgs e)
            {
                if (ISBox.Checked == false)
                {
                    Specular_Trace.Enabled = false;
                    Spec_RayCount.Enabled = false;
                    return;
                }
                if (RTBox.Checked == true)
                {
                    Specular_Trace.Enabled = false;
                    Spec_RayCount.Enabled = false;
                }
                else
                {
                    Specular_Trace.Enabled = true;
                    Spec_RayCount.Enabled = true;
                }
                if (Specular_Trace.Checked == true)
                {
                    RTBox.Enabled = false;
                    RT_Count.Enabled = false;
                }
                else
                {
                    RTBox.Enabled = true;
                    RT_Count.Enabled = true;
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
            #endregion

            #region ToolstripMenuItems
            private void SaveDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Write_File();
            }

            private void OpenDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                IR_Parts_Analysis.Visible = false;

                Eto.Forms.OpenFileDialog OF = new Eto.Forms.OpenFileDialog();
                OF.Filters.Add(new FileDialogFilter("Pachyderm Ray Data file (.pac1)", ".pac1"));
                OF.Filters.Add(new FileDialogFilter("All", ".*"));
                OF.CurrentFilterIndex = 0;
                if (OF.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                { 
                    Read_File(OF.FileName);
                    Populate_Sources();
                    
                    Update_Graph(null, System.EventArgs.Empty);
                }
            }

            private void Receiver_Choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                OpenAnalysis();
                Source_Aim_SelectedIndexChanged(null, EventArgs.Empty);
                if (Receiver_Choice.SelectedIndex >= 0 && Receiver_Choice.Items.Count >= 0)
                {
                    Update_Parameters();
                    Update_Graph(null, System.EventArgs.Empty);
                }
                
                List<int> srcs = SelectedSources();
                double dtime = double.PositiveInfinity;

                for (int i = 0; i < srcs.Count; i++)
                {
                    if (Direct_Data[srcs[i]].Delay_ms < dtime) dtime = (int)Direct_Data[srcs[i]].Delay_ms;
                }

                RR_tstart.Value = Math.Floor(dtime);
            }

            private void SaveIntensityResultsToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_Results_Intensity();
            }

            private void savePressureResultsToolStripMenuItem_Click_1(object sender, EventArgs e)
            {
                Plot_Results_Pressure();
            }

            private void saveIntensityPTBFormatToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_PTB_Results_Intensity();
            }

            private void savePressurePTBFormatToolStripMenuItem_Click_1(object sender, EventArgs e)
            {
                Plot_PTB_Results_Pressure();
            }

            private void SavePressureResultsToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_Results_Pressure();
            }

            private void savePressurePTBFormatToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_PTB_Results_Pressure();
            }

            private void saveEDCToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.SelectedValue.ToString() != "Sabine RT" && Parameter_Choice.SelectedValue.ToString() != "Eyring RT") { return; }
                double[][][][] Schroeder = new double[Direct_Data.Length][][][];
                //string[] paramtype = new string[] { "T30/s", "EDT/s", "D/%", "C/dB", "TS/ms", "G/dB", "LF%", "LFC%", "IACC" };//LF/% LFC/% IACC
                string ReceiverLine = "Receiver{0};";
                //double[,,,] ParamValues = new double[SourceList.Items.Count, Recs.Length, 8, paramtype.Length];

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.Filters.Add(new FileFilter("Text File (*.txt)", ".txt"));
                sf.CurrentFilterIndex = 0;

                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    System.IO.StreamWriter SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    try
                    {
                        SW.WriteLine("Pachyderm Acoustic Simulation Results");
                        SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                        SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);

                        for (int s = 0; s < Direct_Data.Length; s++)
                        {
                            Schroeder[s] = new double[Recs.Length][][];
                            for (int r = 0; r < Recs.Length; r++)
                            {
                                Schroeder[s][r] = new double[8][];
                                ReceiverLine = "S" + s.ToString() + "/" + "R" + r.ToString() + ";";
                                SW.WriteLine(ReceiverLine + "63;125;250;500;1k;2k;4k;8k");

                                double RhoC = Direct_Data[0].Rho_C[Receiver_Choice.SelectedIndex];
                                ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                                VB.Show(Rhino.RhinoDoc.ActiveDoc);
                                double[] ETC_BB = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new List<int>() { s }, false, true, VB);
                                VB.Close();
                                for (int oct = 0; oct < 8; oct++)
                                {
                                    ///Need a new PT_Curve method that will apply the correct power to each filter appropriately. This one forces only one power level to be used...)
                                    double[] ETC = Audio.Pach_SP.FIR_Bandpass(ETC_BB, oct, SampleRate, 0);
                                    for (int i = 0; i < ETC.Length; i++) ETC[i] *= ETC[i] / RhoC;
                                    //double[] ETC = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, (int)(CO_TIME.Value / 1000), SampleRate, oct, r, s, false);
                                    Schroeder[s][r][oct] = AcousticalMath.Log10Data(AcousticalMath.Schroeder_Integral(ETC),-100);
                                }

                                string line = ";";

                                for (int i = 0; i < Schroeder[s][r][0].Length; i++) 
                                {
                                    for(int oct = 0; oct < 8; oct++) 
                                    {
                                        line += Math.Round(Schroeder[s][r][oct][i],3).ToString() + ";";
                                    }
                                    SW.WriteLine(line);
                                    line = ";";
                                }

                                SW.WriteLine("");
                            }
                        }
                        SW.Close();
                    }
                    catch (System.Exception)
                    {
                        SW.Close();
                        Rhino.RhinoApp.WriteLine("File is open, and cannot be written over.");
                        return;
                    }
                }
            }

            #endregion

            private void Convergence_CheckedChanged(object sender, EventArgs e)
            {
                if (sender == Spec_Rays)
                {
                    Minimum_Convergence.Checked = false;
                    DetailedConvergence.Checked = false;
                    RT_Count.Enabled = true;
                }
                else if (sender == Minimum_Convergence)
                {
                    Spec_Rays.Checked = false;
                    DetailedConvergence.Checked = false;
                    RT_Count.Enabled = false;
                }
                else if (sender == DetailedConvergence)
                {
                    Spec_Rays.Checked = false;
                    Minimum_Convergence.Checked = false;
                    RT_Count.Enabled = false;
                }
            }

            private void Variegaton_Scroll(object sender, EventArgs e)
            {
                quart_lambda.Text = user_quart_lambda.Value.ToString() + " mm.";
                double f = 1000d * 343d / (user_quart_lambda.Value * 4);
                double[] SCT = new double[8];

                if (f > 88) SCT[0] = 20;
                else if (f < 44) SCT[0] = 90;
                else SCT[0] = 20 + 70d * (1 - (f - 44d) / 44d);

                if (f > 196) SCT[1] = 20;
                else if (f < 88) SCT[1] = 90;
                else SCT[1] = 20 + 70d * (1 - (f - 88d) / 88d);

                if (f > 392) SCT[2] = 20;
                else if (f < 196) SCT[2] = 90;
                else SCT[2] = 20 + 70d * (1 - (f - 196d) / 196d);

                if (f > 784) SCT[3] = 20;
                else if (f < 392) SCT[3] = 90;
                else SCT[3] = 20 + 70f * (1 - (f - 392f) / 392f);

                if (f > 1568) SCT[4] = 20;
                else if (f < 784) SCT[4] = 90;
                else SCT[4] = 20 + 70f * (1 - (f - 784f) / 784f);

                if (f > 3136) SCT[5] = 20;
                else if (f < 1568) SCT[5] = 90;
                else SCT[5] = 20 + 70f * (1 - (f - 1568f) / 1568f);

                if (f > 6272) SCT[6] = 20;
                else if (f < 3136) SCT[6] = 90;
                else SCT[6] = 20 + 70f * (1 - (f - 3136f) / 3136f);

                if (f > 12544) SCT[7] = 20;
                else if (f < 6272) SCT[7] = 90;
                else SCT[7] = 20 + 70f * (1 - (f - 6272f) / 6272f);

                SCATSlider.Value = SCT;
                Commit_Layer_Acoustics();
            }

            private void PlasterScatter_Click(object sender, EventArgs e)
            {
                SCATSlider.Value = new double[8] { 25, 25, 25, 25, 25, 25, 25, 25 };
                Commit_Layer_Acoustics();
            }

            private void GlassScatter_Click(object sender, EventArgs e)
            {
                SCATSlider.Value = new double[8] { 15, 15, 15, 15, 15, 15, 15, 15 };
                Commit_Layer_Acoustics();
            }

            private void Draw_Feedback()
            {
                if (Direct_Data != null)
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
                                PTS.Add(Utilities.RCPachTools.HarePointToModel(hpt));
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
                        pts.Add(Recs[Receiver_Choice.SelectedIndex] + (TempDir) * .343 * Math.Max(5, ((channel)Channel_View.Items[i]).delay));
                        Dirs.Add(new Vector3d(-TempDir.dx, -TempDir.dy, -TempDir.dz));
                    }
                    AuralisationConduit.Instance.add_Speakers(pts, Dirs);
                    AuralisationConduit.Instance.set_direction(Utilities.RCPachTools.HarePointToModel(Recs[Receiver_Choice.SelectedIndex]), Utilities.RCPachTools.HPttoRPt(Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(double)Alt_Choice.Value, true), -(double)Azi_Choice.Value, 0, true)));
                    Update_Rose(null, null);
                }
                if (Rhino.RhinoDoc.ActiveDoc.IsAvailable) Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.Redraw();
            }

            public double[][] RenderFilter(int Sample_Frequency)
            {
                double[][] Temp;
                double[][] Response;

                switch (DistributionType.SelectedValue.ToString())
                {
                    case "Monaural":
                        Response = new double[1][];
                        Response[0] = (IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true));
                        break;
                    case "Binaural (select file...)":
                        Response = new double[2][];
                        if (hrtf == null) break;
                        Response = IR_Construction.Aurfilter_HRTF(Direct_Data, IS_Data, Receiver, hrtf, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), _sysCompSettingsPrimitives, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, false);
                        break;
                    case "First Order Ambisonics (ACN+SN3D)":
                        Response = new double[4][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        break;
                    case "First Order Ambisonics (FuMa+SN3D)":
                        Response = new double[4][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        break;
                    case "Second Order Ambisonics(ACN+SN3D)":
                        Response = new double[9][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                    case "Fourth Order Ambisonics(ACN+SN3D)":
                        Response = new double[25][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        break;
                    case "Fourth Order Ambisonics(FuMa+SN3D)":
                        Response = new double[25][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        break;
                    case "Fifth Order Ambisonics(ACN+SN3D)":
                        Response = new double[36][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        Temp = IR_Construction.AurFilter_Ambisonics5(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[25] = Temp[0];
                        Response[26] = Temp[1];
                        Response[27] = Temp[2];
                        Response[28] = Temp[3];
                        Response[29] = Temp[4];
                        Response[30] = Temp[5];
                        Response[31] = Temp[6];
                        Response[32] = Temp[7];
                        Response[33] = Temp[8];
                        Response[34] = Temp[9];
                        Response[35] = Temp[10];
                        break;
                    case "Fifth Order Ambisonics(FuMa+SN3D)":
                        Response = new double[36][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        Temp = IR_Construction.AurFilter_Ambisonics5(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[25] = Temp[0];
                        Response[26] = Temp[1];
                        Response[27] = Temp[2];
                        Response[28] = Temp[3];
                        Response[29] = Temp[4];
                        Response[30] = Temp[5];
                        Response[31] = Temp[6];
                        Response[32] = Temp[7];
                        Response[33] = Temp[8];
                        Response[34] = Temp[9];
                        Response[35] = Temp[10];
                        break;
                    case "Sixth Order Ambisonics(ACN+SN3D)":
                        Response = new double[49][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        Temp = IR_Construction.AurFilter_Ambisonics5(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[25] = Temp[0];
                        Response[26] = Temp[1];
                        Response[27] = Temp[2];
                        Response[28] = Temp[3];
                        Response[29] = Temp[4];
                        Response[30] = Temp[5];
                        Response[31] = Temp[6];
                        Response[32] = Temp[7];
                        Response[33] = Temp[8];
                        Response[34] = Temp[9];
                        Response[35] = Temp[10];
                        Temp = IR_Construction.AurFilter_Ambisonics6(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[36] = Temp[0];
                        Response[37] = Temp[1];
                        Response[38] = Temp[2];
                        Response[39] = Temp[3];
                        Response[40] = Temp[4];
                        Response[41] = Temp[5];
                        Response[42] = Temp[6];
                        Response[43] = Temp[7];
                        Response[44] = Temp[8];
                        Response[45] = Temp[9];
                        Response[46] = Temp[10];
                        Response[47] = Temp[11];
                        Response[48] = Temp[12];
                        break;
                    case "Sixth Order Ambisonics(FuMa+SN3D)":
                        Response = new double[49][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        Temp = IR_Construction.AurFilter_Ambisonics5(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[25] = Temp[0];
                        Response[26] = Temp[1];
                        Response[27] = Temp[2];
                        Response[28] = Temp[3];
                        Response[29] = Temp[4];
                        Response[30] = Temp[5];
                        Response[31] = Temp[6];
                        Response[32] = Temp[7];
                        Response[33] = Temp[8];
                        Response[34] = Temp[9];
                        Response[35] = Temp[10];
                        Temp = IR_Construction.AurFilter_Ambisonics6(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[36] = Temp[0];
                        Response[37] = Temp[1];
                        Response[38] = Temp[2];
                        Response[39] = Temp[3];
                        Response[40] = Temp[4];
                        Response[41] = Temp[5];
                        Response[42] = Temp[6];
                        Response[43] = Temp[7];
                        Response[44] = Temp[8];
                        Response[45] = Temp[9];
                        Response[46] = Temp[10];
                        Response[47] = Temp[11];
                        Response[48] = Temp[12];
                        break;
                    case "Seventh Order Ambisonics(ACN+SN3D)":
                        Response = new double[64][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        Temp = IR_Construction.AurFilter_Ambisonics5(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[25] = Temp[0];
                        Response[26] = Temp[1];
                        Response[27] = Temp[2];
                        Response[28] = Temp[3];
                        Response[29] = Temp[4];
                        Response[30] = Temp[5];
                        Response[31] = Temp[6];
                        Response[32] = Temp[7];
                        Response[33] = Temp[8];
                        Response[34] = Temp[9];
                        Response[35] = Temp[10];
                        Temp = IR_Construction.AurFilter_Ambisonics6(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[36] = Temp[0];
                        Response[37] = Temp[1];
                        Response[38] = Temp[2];
                        Response[39] = Temp[3];
                        Response[40] = Temp[4];
                        Response[41] = Temp[5];
                        Response[42] = Temp[6];
                        Response[43] = Temp[7];
                        Response[44] = Temp[8];
                        Response[45] = Temp[9];
                        Response[46] = Temp[10];
                        Response[47] = Temp[11];
                        Response[48] = Temp[12];
                        Temp = IR_Construction.AurFilter_Ambisonics7(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[49] = Temp[0];
                        Response[50] = Temp[1];
                        Response[51] = Temp[2];
                        Response[52] = Temp[3];
                        Response[53] = Temp[4];
                        Response[54] = Temp[5];
                        Response[55] = Temp[6];
                        Response[56] = Temp[7];
                        Response[57] = Temp[8];
                        Response[58] = Temp[9];
                        Response[59] = Temp[10];
                        Response[60] = Temp[11];
                        Response[61] = Temp[12];
                        Response[62] = Temp[13];
                        Response[63] = Temp[14];
                        break;
                    case "Seventh Order Ambisonics(FuMa+SN3D)":
                        Response = new double[64][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
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
                        Temp = IR_Construction.AurFilter_Ambisonics4(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[16] = Temp[0];
                        Response[17] = Temp[1];
                        Response[18] = Temp[2];
                        Response[19] = Temp[3];
                        Response[20] = Temp[4];
                        Response[21] = Temp[5];
                        Response[22] = Temp[6];
                        Response[23] = Temp[7];
                        Response[24] = Temp[8];
                        Temp = IR_Construction.AurFilter_Ambisonics5(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[25] = Temp[0];
                        Response[26] = Temp[1];
                        Response[27] = Temp[2];
                        Response[28] = Temp[3];
                        Response[29] = Temp[4];
                        Response[30] = Temp[5];
                        Response[31] = Temp[6];
                        Response[32] = Temp[7];
                        Response[33] = Temp[8];
                        Response[34] = Temp[9];
                        Response[35] = Temp[10];
                        Temp = IR_Construction.AurFilter_Ambisonics6(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[36] = Temp[0];
                        Response[37] = Temp[1];
                        Response[38] = Temp[2];
                        Response[39] = Temp[3];
                        Response[40] = Temp[4];
                        Response[41] = Temp[5];
                        Response[42] = Temp[6];
                        Response[43] = Temp[7];
                        Response[44] = Temp[8];
                        Response[45] = Temp[9];
                        Response[46] = Temp[10];
                        Response[47] = Temp[11];
                        Response[48] = Temp[12];
                        Temp = IR_Construction.AurFilter_Ambisonics7(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[49] = Temp[0];
                        Response[50] = Temp[1];
                        Response[51] = Temp[2];
                        Response[52] = Temp[3];
                        Response[53] = Temp[4];
                        Response[54] = Temp[5];
                        Response[55] = Temp[6];
                        Response[56] = Temp[7];
                        Response[57] = Temp[8];
                        Response[58] = Temp[9];
                        Response[59] = Temp[10];
                        Response[60] = Temp[11];
                        Response[61] = Temp[12];
                        Response[62] = Temp[13];
                        Response[63] = Temp[14];
                        break;
                    default:
                        Response = new double[Channel_View.Items.Count][];
                        for (int i = 0; i < Channel_View.Items.Count; i++)
                        {
                            double alt = (double)Alt_Choice.Value + 180 * Math.Asin((Channel_View.Items[i] as channel).V.dz) / Math.PI;
                            double azi = (double)Azi_Choice.Value + 180 * Math.Atan2((Channel_View.Items[i] as channel).V.dy, (Channel_View.Items[i] as channel).V.dx) / Math.PI;
                            if (alt > 90) alt -= 180;
                            if (alt < -90) alt += 180;
                            if (azi > 360) azi -= 360;
                            if (azi < 0) azi += 360;
                            Response[i] = IR_Construction.AurFilter_Directional(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Graph_Octave.SelectedIndex, Receiver_Choice.SelectedIndex, SelectedSources(), false, alt, azi, true, true);
                        }
                        break;
                }

                return Response;
            }

            private static readonly object _hrtfLock = new object();
            private static Pachyderm_Acoustic.Audio.HRTF _cachedHrtf = null;

            public static Pachyderm_Acoustic.Audio.HRTF GetOrLoadHrtf(PachHybridControl control)
            {
                if (_cachedHrtf != null)
                {
                    return _cachedHrtf;
                }

                lock (_hrtfLock)
                {
                    if (_cachedHrtf != null)
                        return _cachedHrtf;

                    // Citation notice
                    Rhino.RhinoApp.WriteLine(
                        "Default HRTF (KEMAR subject 80) will be used.\n\n" +
                        "We sincerely thank the Advanced Acoustic Information Systems Laboratory, RIEC, Tohoku University, Japan,\n" +
                        "for providing this dataset.\n\n" +
                        "Citation:\n" +
                        "K. Watanabe, Y. Iwaya, Y. Suzuki, S. Takane, and S. Sato,\n" +
                        "\"Dataset of head-related transfer functions measured with a circular loudspeaker array,\"\n" +
                        "Acoust. Sci. & Tech. 35(3), 159-165 (2014). Copyright 2013 by Advanced Acoustic Information Systems Laboratory,\n" +
                        "RIEC, Tohoku University, Japan. All rights reserved.\n\n");

                    string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string kemarPath = Path.Combine(dir, "RIEC_hrir_subject_080.sofa");

                    if (!File.Exists(kemarPath))
                    {
                        Console.WriteLine($"Error: Required HRTF file not found at: {kemarPath}");
                        throw new FileNotFoundException($"HRTF file not found at: {kemarPath}");
                    }

                    _cachedHrtf = new Pachyderm_Acoustic.Audio.HRTF(kemarPath);
                    Console.WriteLine($"Using HRTF file at: {kemarPath}");

                    return _cachedHrtf;
                }
            }

            public double[][][] GenAllOctBandBinauralIRs(int sampleFrequency)
            {
                return GenBandBinauralIRs(sampleFrequency, false);
            }

            private double[][][] GenBandBinauralIRs(int sampleFrequency, bool third)
            {
                var hrtf = GetOrLoadHrtf(this);

                double[][] Broadband;
                try
                {
                    Broadband = IR_Construction.Aurfilter_HRTF(
                        Direct_Data,
                        IS_Data,
                        Receiver,
                        hrtf,
                        CutoffTime,
                        sampleFrequency,
                        Receiver_Choice.SelectedIndex,
                        SelectedSources(),
                        _sysCompSettingsPrimitives,
                        false,
                        (double)Alt_Choice.Value,
                        (double)Azi_Choice.Value,
                        true,
                        true,
                        true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Aurfilter_HRTF failed: {ex.Message}");
                    throw;
                }

                if (Broadband == null || Broadband.Length < 2 || Broadband[0] == null || Broadband[1] == null)
                {
                    Console.WriteLine("Error: Invalid broadband data returned from Aurfilter_HRTF.");
                    return null;
                }

                double[][][] BinauralIRsPerBand = new double[third ? 24 : 8][][];
                for (int band = 0; band < BinauralIRsPerBand.Length; band++)
                {
                    BinauralIRsPerBand[band] = new double[2][];
                    BinauralIRsPerBand[band][0] = third ? Audio.Pach_SP.FIR_Third_Bandpass(Broadband[0], band, sampleFrequency, 0) : Audio.Pach_SP.FIR_Bandpass(Broadband[0], band, sampleFrequency, 0);
                    BinauralIRsPerBand[band][1] = third ? Audio.Pach_SP.FIR_Third_Bandpass(Broadband[1], band, sampleFrequency, 0) : Audio.Pach_SP.FIR_Bandpass(Broadband[1], band, sampleFrequency, 0);
                }

                Console.WriteLine("All octave-band binaural IRs generated successfully.");
                return BinauralIRsPerBand;
            }

            private Eto.Forms.OpenFileDialog GetWave = new Eto.Forms.OpenFileDialog();
            Pachyderm_Acoustic.Audio.HRTF hrtf;
            public Pachyderm_Acoustic.Audio.HRTF ReadSofaHRTF()
            {
                GetWave.Filters.Clear();
                GetWave.Filters.Add("HRTF - SOFA convention (*.sofa) |*.sofa");
                GetWave.CurrentFilterIndex = 0;

                var result = GetWave.ShowDialog(this);
                if (result != DialogResult.Ok)
                {
                    Rhino.RhinoApp.WriteLine("HRTF selection cancelled by user.");
                    return null;
                }

                var hrtf = new Pachyderm_Acoustic.Audio.HRTF(GetWave.FileName);
                if (!hrtf.ValidationPassed)
                {
                    Rhino.RhinoApp.WriteLine($"{hrtf.ValidationMessage}");
                    return null;
                }

                Rhino.RhinoApp.WriteLine($"Using HRTF file at: {GetWave.FileName}");
                Channel_View.Items.Clear();
                Channel_View.Items.Add(new channel(0, new Hare.Geometry.Vector(0, 0, 0), channel.channel_type.hrtf, 0));

                return hrtf;
            }

            private Pachyderm_Acoustic.Audio.SystemResponseCompensation.SystemCompensationSettings _sysCompSettingsPrimitives;

            private static void AssignEQSettings(RhinoSystemCompSettings settings, int choice)
            {
                bool enableSmoothing;

                switch (choice)
                {
                    case 1: // Measurement EQ
                        settings.SelectedEQ = RhinoSystemCompSettings.EQType.Measurement;

                        // Load transfer function
                        var openDialog = new Eto.Forms.OpenFileDialog
                        {
                            Title = "Select transfer function for measurement equalisation (mono)"
                        };
                        openDialog.Filters.Add("Wave Audio (*.wav)|*.wav");

                        if (openDialog.ShowDialog(null) != Eto.Forms.DialogResult.Ok)
                            throw new OperationCanceledException("User cancelled dialog.");

                        int tfFs;
                        double[][] tfReference = Pachyderm_Acoustic.Audio.Pach_SP.Wave.ReadtoDouble(openDialog.FileName, true, out tfFs);

                        if (tfReference.Length != 1)
                            Rhino.RhinoApp.WriteLine("Warning: .wav contains multiple channels. Using channel 0 as mono reference."); // Gardner, 1997 recommends mono - 'equalize the data set with respect to a reference measurement obtained using one of the ear microphones positioned at the center of the head with no head present'.

                        settings.TFReference = tfReference[0];
                        settings.IsCalibrated = ReadBool("Is the transfer function magnitude calibrated? (y/n): ");

                        enableSmoothing = ReadBool("Enable smoothing? (y/n): "); // It should be noted that this refers to smoothing of the EQ curve, not the input data.
                        settings.SmoothingOct = enableSmoothing
                            ? ReadClampedDouble("Enter smoothing octave fraction (e.g., 0.33): ", 0.05, 2.0)
                            : null;
                        break;

                    case 2: // Free-field EQ
                        settings.SelectedEQ = RhinoSystemCompSettings.EQType.FreeField;
                        settings.FreeFieldIncidence = ReadIntInRange("Specify the azimuth of the free-field reference (e.g. 30�):", 0, 360);
                        settings.MaxBoostDb = ReadClampedDouble("Max boost (dB) [press Enter to skip]: ", 0, 24);
                        settings.LowFreqHz = ReadClampedDouble("Low-frequency cutoff (Hz) [press Enter to skip]: ", 20, 20000);
                        enableSmoothing = ReadBool("Enable smoothing? (y/n): ");
                        settings.SmoothingOct = enableSmoothing
                            ? ReadClampedDouble("Enter smoothing octave fraction (e.g., 0.33): ", 0.05, 2.0)
                            : null;
                        break;

                    case 3: // Diffuse-field EQ
                        settings.SelectedEQ = RhinoSystemCompSettings.EQType.DiffuseField;
                        settings.MaxBoostDb = ReadClampedDouble("Max boost (dB) [press Enter to skip]: ", 0, 24);
                        settings.LowFreqHz = ReadClampedDouble("Low-frequency cutoff (Hz) [press Enter to skip]: ", 20, 20000);
                        settings.MinPhase = ReadBool("Use minimum-phase filter? (y/n): ");
                        enableSmoothing = ReadBool("Enable smoothing? (y/n): ");
                        settings.SmoothingOct = enableSmoothing
                            ? ReadClampedDouble("Enter smoothing octave fraction (e.g., 0.33): ", 0.05, 2.0)
                            : null;
                        break;

                    case 0: // No EQ
                    default:
                        settings.SelectedEQ = RhinoSystemCompSettings.EQType.None;
                        Rhino.RhinoApp.WriteLine("No equalisation applied.");
                        break;
                }
            }

            private static int ReadIntInRange(string prompt, int min, int max)
            {
                while (true)
                {
                    int result = 0;
                    var rc = Rhino.Input.RhinoGet.GetInteger(prompt, false, ref result);

                    if (rc == Rhino.Commands.Result.Success && result >= min && result <= max)
                        return result;

                    if (rc == Rhino.Commands.Result.Cancel)
                        throw new OperationCanceledException();

                    Rhino.RhinoApp.WriteLine($"Invalid choice. Enter an integer between {min} and {max}.");
                }
            }

            private static double? ReadClampedDouble(string prompt, double min, double max, double? defaultValue = null)
            {
                double number = defaultValue ?? 0.0;

                var rc = Rhino.Input.RhinoGet.GetNumber(prompt, true, ref number, min, max);
                if (rc == Rhino.Commands.Result.Success)
                {
                    return Clamp(number, min, max);
                }
                if (rc == Rhino.Commands.Result.Nothing)
                {
                    return defaultValue;
                }
                if (rc == Rhino.Commands.Result.Cancel)
                {
                    return null;
                }

                return null;
            }

            public static double Clamp(double value, double min, double max)
            {
                if (value < min) return min;
                if (value > max) return max;
                return value;
            }

            public static bool ReadBool(string prompt)
            {
                var go = new Rhino.Input.Custom.GetOption();
                go.SetCommandPrompt(prompt);

                int optYes = go.AddOption("Yes");
                int optNo = go.AddOption("No");

                while (true)
                {
                    Rhino.Input.GetResult rc = go.Get();

                    if (rc == Rhino.Input.GetResult.Option)
                    {
                        if (go.Option().Index == optYes)
                            return true;
                        if (go.Option().Index == optNo)
                            return false;
                    }
                    else
                    {
                        Rhino.RhinoApp.WriteLine("Invalid input. Please choose Yes or No.");
                    }
                }
            }

            public void ReadArray()
            {
                GetWave.Filters.Clear();
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
                            Channel_View.Items.Add(new channel(int.Parse(s[0]), new Hare.Geometry.Vector(double.Parse(s[1]), double.Parse(s[2]), double.Parse(s[3])), (channel.channel_type)int.Parse(s[4]), double.Parse(s[5])));
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

            private void OpenSignal_Click(object sender, System.EventArgs e)
            {
                GetWave.Filters.Clear();
                GetWave.Filters.Add(" Wave Audio (*.wav) |*.wav");
                if (GetWave.ShowDialog(this) == DialogResult.Ok)
                {
                    Signal_Status.Text = GetWave.FileName;
                    RenderBtn.Enabled = false;
                    try
                    {
                        int[][] signal = Audio.Pach_SP.Wave.ReadtoInt(Signal_Status.Text, false, out SampleRate, true);
                        if (SampleRate <= 0 || signal.Length == 0 || signal.Any(channel => channel == null || channel.Length == 0)) throw new System.IO.InvalidDataException("The wave file contains no usable audio samples.");
                        DryChannel.MinValue = 1;
                        DryChannel.MaxValue = signal.Length;
                        RenderBtn.Enabled = true;
                    }
                    catch (Exception x)
                    {
                        SampleRate = 0;
                        Eto.Forms.MessageBox.Show(x.Message);
                    }
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
                try
                {
                    SignalETC = Audio.Pach_SP.Wave.ReadtoDouble(Signal_Status.Text, true, out Sample_Freq)[0];
                }
                catch (Exception x)
                {
                    Eto.Forms.DialogResult DR = Eto.Forms.MessageBox.Show(x.Message, Eto.Forms.MessageBoxButtons.YesNo);
                    if (DR == Eto.Forms.DialogResult.No)
                    {
                        SignalETC = new double[1] { 0 };
                        Sample_Freq = 44100;
                    }
                    else
                    {
                        SignalETC = Audio.Pach_SP.Wave.ReadtoDouble(Signal_Status.Text, true, out Sample_Freq, true)[0];
                    }
                }
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
                    NewSignal[i] = Pachyderm_Acoustic.Audio.Pach_SP.FFT_Convolution(SignalBuffer, Render_Response[i], 0);
                    for (int j = 0; j < NewSignal[i].Length; j++) NewSignal[i][j] *= (float)(Math.Pow(10, 94 / 20) / Math.Pow(10, ((double)Normalization_Choice.Value) / 20));
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

                    if (!Audio.Pach_SP.Wave.Write(NewSignal, SamplesPerSec, SaveWave.FileName)) MessageBox.Show("Your output file has clipped. Adjust the Gain (Max dB) of the auralization.");

                    //if (PlayAuralization.Checked.Value)
                    //{
                    //    Player = new System.Media.SoundPlayer(SaveWave.FileName);
                    //    Player.Play();
                    //}
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
                    SaveWave.Filters.Add("Extended Wave Audio (*.wavex) |*.wavex");
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

                float mod = (float)(Math.Pow(10, 120 / 20) / Math.Pow(10, ((double)Normalization_Choice.Value + 15) / 20));
                for (int c = 0; c < Render_Response.Length; c++) RR[c] = new float[maxlength];

                for (int j = 0; j < Render_Response[0].Length; j++)
                {
                    for (int c = 0; c < Render_Response.Length; c++) RR[c][j] = (j > Render_Response[c].Length - 1) ? 0 : (float)Render_Response[c][j] * mod;
                }

                if (SaveWave.ShowDialog(this) == DialogResult.Ok)
                {
                    Audio.Pach_SP.Wave.Write(RR, SamplesPerSec, SaveWave.FileName, 24);
                }
            }

            /// <summary>
            /// Defunct, until .Net Core includes better support for audio...
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void PlayAuralization_CheckedChanged(object sender, EventArgs e)
            {
                //if (PlayAuralization.Checked.Value) Player.Play();
                //else Player.Stop();
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
                        HrtfCompensationOptions seed = new HrtfCompensationOptions();

                        if (_sysCompSettingsPrimitives != null)
                        {
                            seed.SelectedEQ = (RhinoSystemCompSettings.EQType)_sysCompSettingsPrimitives.SelectedEQ;
                            seed.SmoothingOct = _sysCompSettingsPrimitives.SmoothingOct;
                            seed.MaxBoostDb = _sysCompSettingsPrimitives.MaxBoostDb;
                            seed.LowFreqHz = _sysCompSettingsPrimitives.LowFreqHz;
                            seed.MinPhase = _sysCompSettingsPrimitives.MinPhase;
                            seed.IsCalibrated = _sysCompSettingsPrimitives.IsCalibrated;
                            seed.FreeFieldIncidence = _sysCompSettingsPrimitives.FreeFieldIncidence;
                        }

                        string initialPath = null;
                        var dlg = new Pachyderm_Acoustic.UI.PachHrtfDialog(initialPath, seed);
                        bool ok = dlg.ShowModal(Rhino.UI.RhinoEtoApp.MainWindow);
                        if (!ok || dlg.ResultData == null) return;

                        hrtf = dlg.ResultData.Hrtf;
                        _sysCompSettingsPrimitives = dlg.ResultData.CompensationPrimitives;

                        Channel_View.Items.Clear();
                        Channel_View.Items.Add(new channel(0, new Hare.Geometry.Vector(0, 0, 0), channel.channel_type.hrtf, 0));
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
                    case "Surround Array via VBAP...":
                        enable_CEdit();
                        ReadArray();
                        break;
                }
                Draw_Feedback();
                Update_Graph(sender, e);
            }

            private void Graph_Octave_SelectedIndexChanged(object sender, EventArgs e)
            {
                var dropdown = sender as DropDown;
                if (dropdown == null || dropdown.SelectedIndex < 0) return;

                if (Response == null)
                {
                    Rhino.RhinoApp.WriteLine("No impulse response found to update graph...");
                    return;
                }
                Update_Graph(sender, e);
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

            private void Move_Up_Click(object sender, EventArgs e)
            {
                int s = Channel_View.SelectedIndex;
                channel t = (channel)Channel_View.Items[s];
                Channel_View.Items.Insert(s - 1, (IListItem)t);
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
                Rhino.Input.RhinoGet.GetNumber("Enter the azimuth of the Speaker in degrees (X = front/back, Y = Left/Right)...", false, ref azi, -360, 360);
                Rhino.Input.RhinoGet.GetNumber("Enter the altitude of the Speaker in degrees (Z = Up/Down)...", false, ref alt, -90, 90);
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
                        string Entry = c._index.ToString() + ':' + c.V.dx.ToString() + ':' + c.V.dy.ToString() + ':' + c.V.dz.ToString() + ":" + c.Type.GetHashCode() + ":" + c.delay.ToString();
                        SW.WriteLine(Entry);
                    }
                    SW.Close();
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
            }




        #region IO Methods
        private void Write_File()
            {
                if (Direct_Data == null && IS_Data == null && IS_Data == null && this.Receiver != null)
                {
                    Eto.Forms.MessageBox.Show("There is no simulated data to save.");
                    return;
                }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.Filters.Add( new FileFilter("Pachyderm Ray Data file (*.pac1)", ".pac1"));
                sf.CurrentFilterIndex = 0;

                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok) FileIO.Write_Pac1(sf.FileName, Direct_Data, IS_Data, Receiver);

                //if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                //{
                //    System.IO.BinaryWriter sw = new System.IO.BinaryWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                //    //1. Date & Time
                //    sw.Write(System.DateTime.Now.ToString());
                //    //2. Plugin Version... if less than 1.1, assume only 1 source.
                //    sw.Write(plugin.Version);
                //    //3. Cut off Time (seconds) and SampleRate
                //    sw.Write((double)CO_TIME.Value);
                //    sw.Write(SampleRate);
                //    //4.0 Source Count(int)
                //    Hare.Geometry.Point[] SRC;
                //    plugin.SourceOrigin(out SRC);
                //    sw.Write(SRC.Length);
                //    for (int i = 0; i < SRC.Length; i++)
                //    {
                //        //4.1 Source Location x (double)    
                //        sw.Write(SRC[i].x);
                //        //4.2 Source Location y (double)
                //        sw.Write(SRC[i].y);
                //        //4.3 Source Location z (double)
                //        sw.Write(SRC[i].z);
                //    }
                //    //5. No of Receivers
                //    sw.Write(Recs.Length);

                //    //6. Write the coordinates of each receiver point
                //    for (int q = 0; q < Recs.Length; q++)
                //    {
                //        sw.Write(Recs[q].x);
                //        sw.Write(Recs[q].y);
                //        sw.Write(Recs[q].z);
                //        sw.Write(Direct_Data[0].Rho_C[q]);
                //    }

                //    for (int s = 0; s < SRC.Length; s++)
                //    {
                //        if (Direct_Data != null)
                //        {
                //            //7. Write Direct Sound Data
                //            Direct_Data[s].Write_Data(ref sw);
                //        }

                //        if (IS_Data[0] != null)
                //        {
                //            //8. Write Image Source Sound Data
                //            IS_Data[s].Write_Data(ref sw);
                //        }

                //        if (Receiver.Length > s && Receiver[s] != null)
                //        {
                //            //9. Write Ray Traced Sound Data
                //            Receiver[s].Write_Data(ref sw);
                //        }
                //    }
                //    sw.Write("End");
                //    sw.Close();
                //}
            }

            private async void Read_File(string path)
            {
                ProgressBox VB = new ProgressBox("Calculating Pressure/Filter Responses");
                VB.Show();
                if (FileIO.Read_Pac1(path, ref Direct_Data, ref IS_Data, ref Receiver, VB))
                {
                    SourceList.Clear();
                    LockUserScale.Checked = false;
                    Update_Graph(null, new System.EventArgs());
                    Receiver_Choice.SelectedIndex = 0;
                    OpenAnalysis();
                    cleanup();

                    Source = new Source[Direct_Data.Length];
                    Recs = new Hare.Geometry.Point[Receiver[0].Count];
                    CutoffTime = Direct_Data[0].Cutoff_Time;

                    for (int i = 0; i < Recs.Length; i++) Recs[i] = Receiver[0].Rec_List[i].Origin;
                    SourceList.Populate(Direct_Data, IS_Data, Receiver);

                    for (int DDCT = 0; DDCT < Direct_Data.Length; DDCT++)
                    {
                        Source[DDCT] = Direct_Data[DDCT].Src;
                    }

                    int r = Receiver_Choice.SelectedIndex;
                    Receiver_Choice.Items.Clear();

                    for (int i = 0; i < Direct_Data[0].Rec_Origin.Count(); i++)
                    {
                        Receiver_Choice.Items.Add(i.ToString());
                    }
                    Receiver_Choice.SelectedIndex = Math.Max(0, r);


                    if (IS_Data != null && IS_Data[0] != null && Receiver_Choice.SelectedIndex > -1)
                    {
                        this.IS_Path_Box.Populate(IS_Data, SourceList.SelectedSources(), 0);
                    }

                    SampleRate = (int)Direct_Data[0].SampleRate;
                    //Task t = new Task(() => { foreach (Receiver_Bank R in Receiver) R.Create_Filter(VB); });
                    //t.Start();
                    
                    //while (! t.IsCompleted) await Task.Delay(1000);

                    //return true;
                }
                else
                {
                    Eto.Forms.MessageBox.Show("File Read Failed...", "Results file was corrupt or incomplete. We apologize for this inconvenience. Please report this to the software author. It will be much appreciated.");
                    //return false;
                }
                //VB.Close();

                ///Set a recommended minimum auralization ceiling...
                List<int> s = new List<int>();
                double max = 0;
                for (int i = 0; i < Source.Length; i++) s.Add(i);
                for (int r = 0; r < Recs.Length; r++)
                {
                    double[] Filter = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, s, false, true);
                    for (int i = 0; i < Filter.Length; i++) if (Filter[i] > max) max = Math.Abs(Filter[i]);
                }

                Normalization_Choice.Value = Math.Max(0, 20 * Math.Log10(max * (float)(Math.Pow(10, 120 / 20) / Math.Pow(10, (15 / 20)))) - 144);
            }

            public void Plot_Results_Intensity()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.SelectedValue.ToString() != "Sabine RT" && Parameter_Choice.SelectedValue.ToString() != "Eyring RT") { return; }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.Filters.Add(new FileFilter("Text File (*.txt)", ".txt"));
                sf.CurrentFilterIndex = 0;

                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    System.IO.StreamWriter SW;

                fileread:
                    try
                    {
                        SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    }
                    catch
                    {
                        Eto.Forms.DialogResult dr = Eto.Forms.MessageBox.Show("File is in use. Close any programs using the file, and try again. (click OK to try again)", "File In Use", Eto.Forms.MessageBoxButtons.OKCancel);
                        if (dr == Eto.Forms.DialogResult.Cancel) return;
                        goto fileread;
                    }

                    double[] Schroeder;
                    double[,,] EDT = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] T10 = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] T15 = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] T20 = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] T30 = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] G = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] C80 = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] C50 = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] D50 = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] TS = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] LF = new double[this.SourceList.Count, Recs.Length, 8];
                    double[,,] LE = new double[this.SourceList.Count, Recs.Length, 8];

                    for (int s = 0; s < SourceList.Count; s++)
                    {
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            for (int oct = 0; oct < 8; oct++)
                            {
                                double[] ETC = IR_Construction.ETCurveOctave(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, s, false);
                                Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                                EDT[s, r, oct] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                                T10[s, r, oct] = AcousticalMath.T_X(Schroeder, 10, SampleRate);
                                T15[s, r, oct] = AcousticalMath.T_X(Schroeder, 15, SampleRate);
                                T20[s, r, oct] = AcousticalMath.T_X(Schroeder, 20, SampleRate);
                                T30[s, r, oct] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                                G[s, r, oct] = AcousticalMath.Strength(ETC, Direct_Data[s].OctaveSWL[oct], false);
                                C50[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                C80[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                                D50[s, r, oct] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                TS[s, r, oct] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                                double[] L_ETC = IR_Construction.ETCurveOctave_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int>() { s }, false, (double)this.Alt_Choice.Value, (double)this.Azi_Choice.Value, true)[1];
                                LF[s, r, oct] = AcousticalMath.Lateral_Fraction(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 100;
                                LE[s, r, oct] = AcousticalMath.Lateral_Efficiency(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 100;
                            }
                        }
                    }

                    SW.WriteLine("Pachyderm Acoustic Simulation Results");
                    SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                    SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);
                    for (int s = 0; s < SourceList.Count; s++)
                    {
                        SW.WriteLine("Source {0};", s);
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            SW.WriteLine("Receiver {0};63 Hz.;125 Hz.;250 Hz.;500 Hz.;1000 Hz.; 2000 Hz.;4000 Hz.; 8000 Hz.", r);
                            SW.WriteLine("Early Decay Time (EDT);{0};{1};{2};{3};{4};{5};{6};{7}", EDT[s, r, 0], EDT[s, r, 1], EDT[s, r, 2], EDT[s, r, 3], EDT[s, r, 4], EDT[s, r, 5], EDT[s, r, 6], EDT[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-10);{0};{1};{2};{3};{4};{5};{6};{7}", T10[s, r, 0], T10[s, r, 1], T10[s, r, 2], T10[s, r, 3], T10[s, r, 4], T10[s, r, 5], T10[s, r, 6], T10[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-15);{0};{1};{2};{3};{4};{5};{6};{7}", T15[s, r, 0], T15[s, r, 1], T15[s, r, 2], T15[s, r, 3], T15[s, r, 4], T15[s, r, 5], T15[s, r, 6], T15[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-20);{0};{1};{2};{3};{4};{5};{6};{7}", T20[s, r, 0], T20[s, r, 1], T20[s, r, 2], T20[s, r, 3], T20[s, r, 4], T20[s, r, 5], T20[s, r, 6], T20[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-30);{0};{1};{2};{3};{4};{5};{6};{7}", T30[s, r, 0], T30[s, r, 1], T30[s, r, 2], T30[s, r, 3], T30[s, r, 4], T30[s, r, 5], T30[s, r, 6], T30[s, r, 7]);
                            SW.WriteLine("Clarity - 50 ms (C-50);{0};{1};{2};{3};{4};{5};{6};{7}", C50[s, r, 0], C50[s, r, 1], C50[s, r, 2], C50[s, r, 3], C50[s, r, 4], C50[s, r, 5], C50[s, r, 6], C50[s, r, 7]);
                            SW.WriteLine("Definition/Deutlichkeit (D);{0};{1};{2};{3};{4};{5};{6};{7}", D50[s, r, 0], D50[s, r, 1], D50[s, r, 2], D50[s, r, 3], D50[s, r, 4], D50[s, r, 5], D50[s, r, 6], D50[s, r, 7]);
                            SW.WriteLine("Clarity - 80 ms (C-80);{0};{1};{2};{3};{4};{5};{6};{7}", C80[s, r, 0], C80[s, r, 1], C80[s, r, 2], C80[s, r, 3], C80[s, r, 4], C80[s, r, 5], C80[s, r, 6], C80[s, r, 7]);
                            SW.WriteLine("Center Time (TS);{0};{1};{2};{3};{4};{5};{6};{7}", TS[s, r, 0], TS[s, r, 1], TS[s, r, 2], TS[s, r, 3], TS[s, r, 4], TS[s, r, 5], TS[s, r, 6], TS[s, r, 7]);
                            SW.WriteLine("Strength(G);{0};{1};{2};{3};{4};{5};{6};{7}", G[s, r, 0], G[s, r, 1], G[s, r, 2], G[s, r, 3], G[s, r, 4], G[s, r, 5], G[s, r, 6], G[s, r, 7]);
                            SW.WriteLine("Lateral Fraction(LF);{0};{1};{2};{3};{4};{5};{6};{7}", LF[s, r, 0], LF[s, r, 1], LF[s, r, 2], LF[s, r, 3], LF[s, r, 4], LF[s, r, 5], LF[s, r, 6], LF[s, r, 7]);
                            SW.WriteLine("Lateral Efficiency(LE);{0};{1};{2};{3};{4};{5};{6};{7}", LE[s, r, 0], LE[s, r, 1], LE[s, r, 2], LE[s, r, 3], LE[s, r, 4], LE[s, r, 5], LE[s, r, 6], LE[s, r, 7]);
                        }
                    }
                    SW.Close();
                }
            }

            public void Plot_Results_Pressure()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.SelectedValue.ToString() != "Sabine RT" && Parameter_Choice.SelectedValue.ToString() != "Eyring RT") { return; }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.Filters.Add(new FileFilter("Text File (*.txt)", ".txt"));
                sf.CurrentFilterIndex = 0;

                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    System.IO.StreamWriter SW;

                fileread:
                    try
                    {
                        SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    }
                    catch
                    {
                        Eto.Forms.DialogResult dr = Eto.Forms.MessageBox.Show("File is in use. Close any programs using the file, and try again. (Click ok to retry)", "File In Use", Eto.Forms.MessageBoxButtons.OKCancel);
                        if (dr == Eto.Forms.DialogResult.Cancel) return;
                        goto fileread;
                    }

                    double[] Schroeder;
                    #pragma warning disable CA1814
                    double[,,] EDT = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] T10 = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] T15 = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] T20 = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] T30 = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] G = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] C80 = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] C50 = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] D50 = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] TS = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] LF = new double[SourceList.Count, Recs.Length, 8];
                    double[,,] LE = new double[SourceList.Count, Recs.Length, 8];
                    #pragma warning restore CA1814

                    for (int s = 0; s < SourceList.Count; s++)
                    {
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                            VB.Show(Rhino.RhinoDoc.ActiveDoc);
                            double[] PTC = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new System.Collections.Generic.List<int>(1) { s }, false, true, VB);
                            VB.Close();
                            double[] lateralPTC = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new List<int> { s }, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[1];
                            for (int oct = 0; oct < 8; oct++)
                            {
                                double[] ETC = Pachyderm_Acoustic.Audio.Pach_SP.FIR_Bandpass(PTC, oct, SampleRate, 0);
                                for (int i = 0; i < ETC.Length; i++) { ETC[i] = ETC[i] * ETC[i] / Direct_Data[s].Rho_C[r]; }; Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                                EDT[s, r, oct] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                                T10[s, r, oct] = AcousticalMath.T_X(Schroeder, 10, SampleRate);
                                T15[s, r, oct] = AcousticalMath.T_X(Schroeder, 15, SampleRate);
                                T20[s, r, oct] = AcousticalMath.T_X(Schroeder, 20, SampleRate);
                                T30[s, r, oct] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                                G[s, r, oct] = AcousticalMath.Strength(ETC, Direct_Data[s].OctaveSWL[oct], true);
                                C50[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), true);
                                C80[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), true);
                                D50[s, r, oct] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), true);
                                TS[s, r, oct] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r), true) * 1000;
                                double[] L_ETC = Lateral_Pressure_Energy(lateralPTC, oct, Direct_Data[s].Rho_C[r]);
                                LF[s, r, oct] = AcousticalMath.Lateral_Fraction(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r) + Direct_Data[s].Delay_ms / 1000.0, true) * 100;
                                LE[s, r, oct] = AcousticalMath.Lateral_Efficiency(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r) + Direct_Data[s].Delay_ms / 1000.0, true) * 100;
                            }
                        }
                    }

                    SW.WriteLine("Pachyderm Acoustic Simulation Results");
                    SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                    SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);
                    for (int s = 0; s < SourceList.Count; s++)
                    {
                        SW.WriteLine("Source {0};", s);
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            SW.WriteLine("Receiver {0};63 Hz.;125 Hz.;250 Hz.;500 Hz.;1000 Hz.; 2000 Hz.;4000 Hz.; 8000 Hz.", r);
                            SW.WriteLine("Early Decay Time (EDT);{0};{1};{2};{3};{4};{5};{6};{7}", EDT[s, r, 0], EDT[s, r, 1], EDT[s, r, 2], EDT[s, r, 3], EDT[s, r, 4], EDT[s, r, 5], EDT[s, r, 6], EDT[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-10);{0};{1};{2};{3};{4};{5};{6};{7}", T10[s, r, 0], T10[s, r, 1], T10[s, r, 2], T10[s, r, 3], T10[s, r, 4], T10[s, r, 5], T10[s, r, 6], T10[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-15);{0};{1};{2};{3};{4};{5};{6};{7}", T15[s, r, 0], T15[s, r, 1], T15[s, r, 2], T15[s, r, 3], T15[s, r, 4], T15[s, r, 5], T15[s, r, 6], T15[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-20);{0};{1};{2};{3};{4};{5};{6};{7}", T20[s, r, 0], T20[s, r, 1], T20[s, r, 2], T20[s, r, 3], T20[s, r, 4], T20[s, r, 5], T20[s, r, 6], T20[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-30);{0};{1};{2};{3};{4};{5};{6};{7}", T30[s, r, 0], T30[s, r, 1], T30[s, r, 2], T30[s, r, 3], T30[s, r, 4], T30[s, r, 5], T30[s, r, 6], T30[s, r, 7]);
                            SW.WriteLine("Clarity - 50 ms (C-50);{0};{1};{2};{3};{4};{5};{6};{7}", C50[s, r, 0], C50[s, r, 1], C50[s, r, 2], C50[s, r, 3], C50[s, r, 4], C50[s, r, 5], C50[s, r, 6], C50[s, r, 7]);
                            SW.WriteLine("Definition/Deutlichkeit (D);{0};{1};{2};{3};{4};{5};{6};{7}", D50[s, r, 0], D50[s, r, 1], D50[s, r, 2], D50[s, r, 3], D50[s, r, 4], D50[s, r, 5], D50[s, r, 6], D50[s, r, 7]);
                            SW.WriteLine("Clarity - 80 ms (C-80);{0};{1};{2};{3};{4};{5};{6};{7}", C80[s, r, 0], C80[s, r, 1], C80[s, r, 2], C80[s, r, 3], C80[s, r, 4], C80[s, r, 5], C80[s, r, 6], C80[s, r, 7]);
                            SW.WriteLine("Center Time (TS);{0};{1};{2};{3};{4};{5};{6};{7}", TS[s, r, 0], TS[s, r, 1], TS[s, r, 2], TS[s, r, 3], TS[s, r, 4], TS[s, r, 5], TS[s, r, 6], TS[s, r, 7]);
                            SW.WriteLine("Strength(G);{0};{1};{2};{3};{4};{5};{6};{7}", G[s, r, 0], G[s, r, 1], G[s, r, 2], G[s, r, 3], G[s, r, 4], G[s, r, 5], G[s, r, 6], G[s, r, 7]);
                            SW.WriteLine("Lateral Fraction(LF);{0};{1};{2};{3};{4};{5};{6};{7}", LF[s, r, 0], LF[s, r, 1], LF[s, r, 2], LF[s, r, 3], LF[s, r, 4], LF[s, r, 5], LF[s, r, 6], LF[s, r, 7]);
                            SW.WriteLine("Lateral Efficiency(LE);{0};{1};{2};{3};{4};{5};{6};{7}", LE[s, r, 0], LE[s, r, 1], LE[s, r, 2], LE[s, r, 3], LE[s, r, 4], LE[s, r, 5], LE[s, r, 6], LE[s, r, 7]);
                        }
                    }
                    SW.Close();
                }
            }

            public void Plot_PTB_Results_Intensity()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.SelectedValue.ToString() != "Sabine RT" && Parameter_Choice.SelectedValue.ToString() != "Eyring RT") { return; }
                double[] Schroeder;
                string[] paramtype = new string[] { "T30/s", "EDT/s", "D/%", "C/dB", "TS/ms", "G/dB", "LF%", "LFC%", "IACC" };//LF/% LFC/% IACC
                string ReceiverLine = "Receiver{0};";
                double[,,,] ParamValues = new double[SourceList.Count, Recs.Length, 8, paramtype.Length];

                for (int s = 0; s < Direct_Data.Length; s++)
                {
                    for (int r = 0; r < Recs.Length; r++)
                    {
                        ReceiverLine += r.ToString() + ";";
                        for (int oct = 0; oct < 8; oct++)
                        {
                            double[] ETC = IR_Construction.ETCurveOctave(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, s, false);
                            Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                            ParamValues[s, r, oct, 0] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                            ParamValues[s, r, oct, 1] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                            ParamValues[s, r, oct, 2] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 3] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 4] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                            ParamValues[s, r, oct, 5] = AcousticalMath.Strength(ETC, Direct_Data[s].OctaveSWL[oct], false);
                            double azi, alt;
                            PachTools.World_Angles(Direct_Data[s].Src.Origin, Recs[r], true, out alt, out azi);
                            double[][] Lateral_ETC = IR_Construction.ETCurveOctave_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int> { s }, false, alt, azi, true);
                            ParamValues[s, r, oct, 6] = AcousticalMath.Lateral_Fraction(ETC, Lateral_ETC, SampleRate, Direct_Data[s].Min_Time(r), false);
                        }
                    }
                }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.Filters.Add(new FileFilter("Text File (*.txt)", ".txt"));
                sf.CurrentFilterIndex = 0;

                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    try
                    {
                        System.IO.StreamWriter SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                        SW.WriteLine("Pachyderm Acoustic Simulation Results");
                        SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                        SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);

                        for (int oct = 0; oct < 8; oct++)
                        {
                            SW.WriteLine(string.Format(ReceiverLine, oct));
                            for (int param = 0; param < paramtype.Length; param++)
                            {
                                SW.Write(paramtype[param] + ";");
                                for (int i = 0; i < Direct_Data.Length; i++)
                                {
                                    for (int q = 0; q < Recs.Length; q++)
                                    {
                                        SW.Write(ParamValues[i, q, oct, param].ToString() + ";");
                                    }
                                }
                                SW.WriteLine("");
                            }
                        }
                        SW.Close();
                    }
                    catch (System.Exception)
                    {
                        Rhino.RhinoApp.WriteLine("File is open, and cannot be written over.");
                        return;
                    }
                }
            }

            public void Plot_PTB_Results_Pressure()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.SelectedValue.ToString() != "Sabine RT" && Parameter_Choice.SelectedValue.ToString() != "Eyring RT") { return; }
                double[] Schroeder;
                string[] paramtype = new string[] { "T30/s", "EDT/s", "D/%", "C/dB", "TS/ms", "G/dB", "LF%", "LFC%", "IACC" };//LF/% LFC/% IACC
                string ReceiverLine = "Receiver{0};";
                double[,,,] ParamValues = new double[SourceList.Count, Recs.Length, 8, paramtype.Length];

                for (int s = 0; s < Direct_Data.Length; s++)
                {
                    for (int r = 0; r < Recs.Length; r++)
                    {
                        ReceiverLine += r.ToString() + ";";
                        ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                        VB.Show(Rhino.RhinoDoc.ActiveDoc);
                        double[] PTC = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new System.Collections.Generic.List<int>(1) { s }, false, true, VB);
                        VB.Close();
                        for (int oct = 0; oct < 8; oct++)
                        {
                            double[] ETC = Pachyderm_Acoustic.Audio.Pach_SP.FIR_Bandpass(PTC, oct, SampleRate, 0);
                            for (int i = 0; i < ETC.Length; i++) { ETC[i] = AcousticalMath.Intensity_Pressure(ETC[i]); };
                            Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                            ParamValues[s, r, oct, 0] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                            ParamValues[s, r, oct, 1] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                            ParamValues[s, r, oct, 2] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), true);
                            ParamValues[s, r, oct, 3] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), true);
                            ParamValues[s, r, oct, 4] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r), true) * 1000;
                            ParamValues[s, r, oct, 5] = AcousticalMath.Strength(ETC, Direct_Data[s].OctaveSWL[oct], true);
                            double azi, alt;
                            PachTools.World_Angles(Direct_Data[s].Src.Origin, Recs[r], true, out alt, out azi);
                            double[][] Lateral_ETC = IR_Construction.ETCurveOctave_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int> { s }, false, alt, azi, true);
                            ParamValues[s, r, oct, 6] = AcousticalMath.Lateral_Fraction(ETC, Lateral_ETC, SampleRate, Direct_Data[s].Min_Time(r), false);
                        }
                    }
                }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.Filters.Add(new FileFilter("Text file for use with Excel(*.txt)", ".txt"));
                sf.CurrentFilterIndex = 0;

                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    try
                    {
                        System.IO.StreamWriter SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                        SW.WriteLine("Pachyderm Acoustic Simulation Results");
                        SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                        SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);

                        for (int oct = 0; oct < 8; oct++)
                        {
                            SW.WriteLine(string.Format(ReceiverLine, oct));
                            for (int param = 0; param < paramtype.Length; param++)
                            {
                                SW.Write(paramtype[param] + ";");
                                for (int i = 0; i < Direct_Data.Length; i++)
                                {
                                    for (int q = 0; q < Recs.Length; q++)
                                    {
                                        SW.Write(ParamValues[i, q, oct, param].ToString() + ";");
                                    }
                                }
                                SW.WriteLine("");
                            }
                        }
                        SW.Close();
                    }
                    catch (System.Exception)
                    {
                        Rhino.RhinoApp.WriteLine("File is open, and cannot be written over.");
                        return;
                    }
                }
            }
            #endregion IO Methods

            private class channel : IListItem
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
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0), channel_type.Left, 0);
                }

                public static channel Right(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(2), -1 / Math.Sqrt(2), 0), channel_type.Right, 0);
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
                            return string.Format("{0}: Dir-({1},{2},{3}), Delay {4} ms.", _index, Math.Round(V.dx, 2), Math.Round(V.dy, 2), Math.Round(V.dz, 2), Math.Round(delay));
                        case channel_type.hrtf:
                            return "0 & 1: Binaural HRTF - 2 channels";
                        default:
                            return "Whoops... Doesn't conform";
                    }
                }
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
                ParameterResults?.CloseWindows();
                // Called when the document or panel container is closed/destroyed
            }
            #endregion IPanel methods

            internal PolarAxis polarPlot;
            internal Button Calculate;
            internal CheckBox RTBox;
            internal CheckBox ISBox;
            internal TabControl Tabs;
            internal TabPage TabImpulse;
            internal NumericStepper Image_Order;
            internal NumericStepper RT_Count;
            internal NumericStepper CO_TIME;
            internal NumericStepper Spec_RayCount;
            internal TabPage TabAnalysis;
            internal TabPage TabMaterials;
            internal ListBox Material_Lib;
            internal DropDown ReceiverSelection;
            internal GroupBox ParameterBox;
            internal DropDown LayerDisplay;
            internal Label PathCount;
            internal DropDown Parameter_Choice;
            internal Medium_Properties_Group MediumProps;
            internal Label Label3;
            internal Label Label19;
            internal CheckBox Specular_Trace;
            internal ComboBox Receiver_Choice;
            internal Slider ScatFlat;
            private GroupBox SaveAbsBox;
            private Button Save_Material;
            private MaskedTextBox Material_Name;
            private CheckBox EdgeFreq;
            internal SourceListBox SourceList;
            private Label ISOCOMP;
            internal CheckBox BTM_ED;
            private NumericStepper Alt_Choice;
            private NumericStepper Azi_Choice;
            internal PathListBox IS_Path_Box;
            internal ComboBox Graph_Octave;
            private ScottPlot.Eto.EtoPlot Analysis_View;
            private CheckBox Normalize_Graph;
            private CheckBox LockUserScale;
            internal ComboBox Graph_Type;
            internal ComboBox Source_Aim;
            internal Label AimatSrc;
            private Button Abs_Designer;
            private ScottPlot.Eto.EtoPlot SmartMat_Display;
            private TabControl tabCoef;
            private TabPage Absorption;
            private TabPage Scattering;
            private TabPage Transparency;
            internal Label labelFlatten;
            internal Slider Trans_Flat;


            private SegmentedButton MenuStrip;
            private MenuSegmentedItem fileToolStripMenuItem;
            private ButtonMenuItem AboutToolStripMenuItem;
            private ButtonMenuItem LearnToolStripMenuItem;
            private ButtonMenuItem saveDataToolStripMenuItem;
            private ButtonMenuItem openDataToolStripMenuItem;
            private ButtonMenuItem saveParameterResultsToolStripMenuItem;
            private ButtonMenuItem savePTBFormatToolStripMenuItem;
            private ButtonMenuItem saveEDCToolStripMenuItem;
            private ButtonMenuItem savePressureResultsToolStripMenuItem;
            private ButtonMenuItem savePressurePTBFormatToolStripMenuItem;
            private ButtonMenuItem DonateToolStripMenuItem;

            TabControl analysis_tabs;

            private Button Delete_Material;
            private Label labelExp;
            private RadioButton Spec_Rays;
            private RadioButton DetailedConvergence;
            private RadioButton SimulationOctave;
            private RadioButton SimulationThirdOctave;
            private RadioButton Minimum_Convergence;
            private Label labelVar;
            private Label quart_lambda;
            internal Slider user_quart_lambda;
            private Button PlasterScatter;
            private Button GlassScatter;
            private FreqSlider ABSSlider;
            private FreqSlider SCATSlider;
            private FreqSlider TRANSSlider;
            private FreqSlider TLSlider;
            internal Label labelConv;
            private TabControl tabTransControls;
            private TabPage tabTC;
            private CheckBox Trans_Check;
            private TabPage tabTL;
            internal Label labelTransLib;
            private GroupBox SaveTLBox;
            private Button DeleteAssembly;
            private Button SaveAssembly;
            private MaskedTextBox IsolationAssemblies;
            private CheckBox TL_Check;
            internal ListBox Isolation_Lib;
            private Label labelEXP2;
            private GroupBox IR_Parts_Analysis;
            private CheckBox Specular_Part;
            private CheckBox Diffuse_Part;
            private CheckBox Specular_postdiffuse_Part;

            //Auralization controls
            private ListBox Channel_View;
            private Button Remove_Channel;
            private Button Save_Channels;
            private Button Add_Channel;
            private Button Move_Down;
            private Button Move_Up;
            private DropDown Graph_Aur_Octave;
            private ScottPlot.Eto.EtoPlot Auralization_View;
            internal NumericStepper DryChannel;
            internal TextBox Signal_Status;
            internal Button OpenSignal;
            internal Button Export_Filter;
            private DropDown Sample_Freq_Selection;
            internal NumericStepper Normalization_Choice;
            //private CheckBox PlayAuralization;
            internal Button RenderBtn;
            internal DropDown DistributionType;

            internal GroupBox Receiver_Rose;
            internal CheckBox Show_Rec_Rose;
            internal NumericStepper RR_tstart;
            internal NumericStepper RR_Width;

            AuralisationConduit A;

            int[] SrcRendered;
            int RecRendered;
            double[][] Response;
            int SFreq_Rendered;


        }

        public class RhinoSystemCompSettings
        {
            public enum EQType { None = 0, Measurement = 1, FreeField = 2, DiffuseField = 3 }

            public EQType SelectedEQ { get; set; }
            public double? SmoothingOct { get; set; }
            public double? MaxBoostDb { get; set; }
            public double? LowFreqHz { get; set; }
            public bool MinPhase { get; set; }
            public bool IsCalibrated { get; set; }
            public double[] TFReference { get; set; }
            public int FreeFieldIncidence { get; set; }
        }
    }
}