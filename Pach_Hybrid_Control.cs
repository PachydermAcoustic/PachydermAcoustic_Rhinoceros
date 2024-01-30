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
using System.Collections.Generic;
using System;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;
using static Rhino.UI.Internal.DwgOptions;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("8559be06-21d7-4535-803e-95a9dd3a2898")]
        public class PachHybridControl : Panel, IPanel
        {
            // This call is required by the Windows Form Designer. 
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

                ABSSlider = new FreqSlider(FreqSlider.bands.Octave);
                ABSSlider.MouseLeave += this.Acoustics_Coef_Update;

                Scrollable Absscroll = new Scrollable();
                Absscroll.Content = ABSSlider;
                AL.AddRow(Absscroll);
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
                this.user_quart_lambda.TickFrequency = 10;
                this.user_quart_lambda.Value = 25;
                this.user_quart_lambda.ValueChanged += this.Variegaton_Scroll;
                this.quart_lambda = new Label();
                this.quart_lambda.Text = "25 mm.";
                VC.AddRow(user_quart_lambda, quart_lambda);
                SL.AddRow(VC);

                Label labelScat = new Label();
                labelScat.Text = "Scattering Coefficients (% non-specular reflected energy)";
                SCATSlider = new FreqSlider(FreqSlider.bands.Octave);
                SCATSlider.MouseLeave += Acoustics_Coef_Update;
                SL.AddRow(labelScat);
                SL.AddRow(SCATSlider);
                this.SmartMat_Display = new ScottPlot.Eto.EtoPlot();
                this.SmartMat_Display.Size = new Size(732, 966);
                this.SmartMat_Display.TabIndex = 45;
                this.SmartMat_Display.Plot.Title("Absorption By Angle");
                this.SmartMat_Display.Visible = false;
                SL.AddRow(SmartMat_Display);
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

                //this.label37.Text = "Flatten All";

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

                PB.AddRow(Parameter_Choice, ISOCOMP);
                ParameterBox = new GroupBox();
                this.ParameterBox.Text = "Parametric Analysis";
                this.ISOCOMP = new Label();
                this.ISOCOMP.Text = "ISO-Compliant:";

                SRT1.Text = "62.5 hz:";
                SRT2.Text = "125 hz:";
                SRT3.Text = "250 hz:";
                SRT4.Text = "500 hz:";
                SRT5.Text = "1000 hz:";
                SRT6.Text = "2000 hz:";
                SRT7.Text = "4000 hz:";
                SRT8.Text = "8000 hz:";
                PB.AddRow(SRT1, SRT5);
                PB.AddRow(SRT2, SRT6);
                PB.AddRow(SRT3, SRT7);
                PB.AddRow(SRT4, SRT8);

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
                this.Parameter_Choice.Items.Add("Echo Criterion (Music, 10%)");
                this.Parameter_Choice.Items.Add("Echo Criterion (Music, 50%)");
                this.Parameter_Choice.Items.Add("Echo Criterion (Speech, 10%)");
                this.Parameter_Choice.Items.Add("Echo Criterion (Speech, 50%)");
                this.Parameter_Choice.SelectedValue = "Select Parameter...";
                this.Parameter_Choice.SelectedValueChanged += this.Parameter_Choice_SelectedIndexChanged;
                ParameterBox.Content = PB;

                DynamicLayout SrcL = new DynamicLayout();
                SrcL.DefaultSpacing = new Size(8, 8);
                Label labelSrc = new Label();
                labelSrc.Text = "Source";
                this.SourceList = new SourceListBox();
                this.SourceList.MouseUp += this.SourceList_MouseUp;
                SrcL.AddRow(labelSrc);
                SrcL.AddRow(SourceList);
                DynamicLayout Rec = new DynamicLayout();
                Rec.DefaultSpacing = new Size(8, 8);
                Label labelRec = new Label();
                labelRec.Text = "Receiver";
                this.Receiver_Choice = new ComboBox();
                this.Receiver_Choice.Text = "No Results Calculated...";
                this.Receiver_Choice.SelectedIndexChanged += this.Receiver_Choice_SelectedIndexChanged;
                Rec.AddRow(labelRec, Receiver_Choice);
                SrcL.AddSpace();
                SrcL.AddRow(Rec);
                AnaTop.AddRow(SrcL, ParameterBox);

                DynamicLayout Anamid = new DynamicLayout();
                Anamid.DefaultSpacing = new Size(8, 8);
                this.AimatSrc = new Label();
                this.AimatSrc.Enabled = false;
                this.AimatSrc.Text = "Aim at Source";
                this.Source_Aim = new ComboBox();
                this.Source_Aim.Text = "None";
                this.Source_Aim.SelectedIndexChanged += this.Source_Aim_SelectedIndexChanged;

                Label labelAlt = new Label();
                labelAlt.Text = "Altitude";
                this.Alt_Choice = new NumericStepper();
                this.Alt_Choice.DecimalPlaces = 2;
                this.Alt_Choice.MaxValue = 90;
                this.Alt_Choice.MinValue = -90;
                this.Alt_Choice.ValueChanged += this.Alt_Choice_ValueChanged;

                Anamid.AddRow(AimatSrc, Source_Aim, labelAlt, Alt_Choice);

                Label labelAzi = new Label();
                labelAzi.Text = "Azimuth";
                this.Azi_Choice = new NumericStepper();
                this.Azi_Choice.DecimalPlaces = 2;
                this.Azi_Choice.MaxValue = 360;
                this.Azi_Choice.MinValue = 1;
                this.Azi_Choice.ValueChanged += this.Azi_Choice_ValueChanged;

                Label LabelISPaths = new Label();
                LabelISPaths.Text = "Deterministic Paths";
                this.PathCount = new Label();
                this.PathCount.Text = "Pending...";
                Anamid.AddRow(LabelISPaths, PathCount, labelAzi, Azi_Choice);

                ANALO.AddRow(AnaTop);
                ANALO.AddRow(Anamid);

                this.IS_Path_Box = new PathListBox();
                this.IS_Path_Box.MouseUp += this.IS_Path_Box_MouseUp;
                ANALO.AddRow(IS_Path_Box);

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
                this.Graph_Type.SelectedIndexChanged += this.Update_Graph;
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
                AnaGctrl.AddRow(LockUserScale, Normalize_Graph);
                ANALO.AddRow(AnaGctrl);
                this.Analysis_View = new ScottPlot.Eto.EtoPlot();
                this.Analysis_View.Size = new Size(-1, 250);
                ANALO.AddRow(Analysis_View);
                this.Auralisation = new Button();
                this.Auralisation.Text = "Go To Auralizations";
                this.Auralisation.Click += this.Auralisation_Click;
                ANALO.AddRow(Auralisation);
                this.TabAnalysis.Content = ANALO;
                this.Tabs.Pages.Add(this.TabAnalysis);

                HybridLO.AddRow(Tabs);
                this.Content = HybridLO;

                Load_Library();
                Instance = this;
                Rhino.RhinoDoc.LayerTableEvent += Load_Library_Event;
                Linear_Phase = Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System;
            }

            private void Load_Library_Event(object sender, Rhino.DocObjects.Tables.LayerTableEventArgs e)
            {
                Load_Library();
            }

            private void Load_Library()
            {
                if (LayerDisplay.SelectedIndex < 0) LayerDisplay.SelectedIndex = 0;
                LayerNames.Clear();

                object Selection = LayerDisplay.SelectedValue;
                LayerDisplay.Items.Clear();
                for (int q = 0; q < Rhino.RhinoDoc.ActiveDoc.Layers.Count; q++)
                {
                    if (!Rhino.RhinoDoc.ActiveDoc.Layers[q].IsDeleted) LayerNames.Add(new layer(Rhino.RhinoDoc.ActiveDoc.Layers[q].Name, q));
                }
                foreach (layer l in LayerNames) LayerDisplay.Items.Add(l.name.ToString());
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
            List<string> SrcTypeList = new List<string>();

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                string SavePath = null;
                CutoffTime = (double)this.CO_TIME.Value;

                if (PachydermAc_PlugIn.SaveResults)
                {
                    Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                    sf.CurrentFilter = ".pac1";
                    sf.Filters.Add("Pachyderm Ray Data file (*.pac1)|*.pac1|" + "All Files|");
                    if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                    {
                        SavePath = sf.FileName;
                    }
                }
                for (int i = 0; i < SourceList.Count; i++) SourceList.Set_Src_Checked(i, false);

                SourceList.Clear();
                Source_Aim.Items.Clear();
                Receiver_Choice.Text = "0";

                IS_Data = null;
                Direct_Data = null;
                List<Hare.Geometry.Point> RPT;

                if (!(plugin.Receiver(out RPT) && plugin.Source(out Source)))
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
                            if ((int)Image_Order.Value > 1)
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
                ///

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
                ///

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
                    RhCommon_Scene NScene = RCPachTools.GetNURBSScene(MediumProps.RelHumidity, MediumProps.Temp_Celsius, MediumProps.StaticPressure_hPa, MediumProps.Atten_Method.SelectedIndex, EdgeFreq.Checked.Value);
                    if (!NScene.Complete)
                    {
                        CancelCalc();
                        return;
                    }
                    NScene.partition(P, PachydermAc_PlugIn.VGDomain);
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
                Pach_RunSim_Command command = Pach_RunSim_Command.Instance;

                if (command == null) { return; }

                for (int s = 0; s < Source.Length; s++)
                {
                    Receiver_Bank Rec = new Receiver_Bank(RPT.ToArray(), Source[s], PScene, SampleRate, CutoffTime, T, false);

                    command.Sim = new Direct_Sound(Source[s], Rec, PScene, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
                    Rhino.RhinoApp.RunScript("Run_Simulation", false);
                    if (command.CommandResult != Rhino.Commands.Result.Cancel)
                    {
                        Direct_Data[s] = ((Direct_Sound)command.Sim);
                        Direct_Data[s].Create_Filter();
                    }
                    else
                    {
                        CancelCalc();
                        return;
                    } 
                    command.Reset();

                    if (ISBox.Checked.Value)
                    {
                        command.Sim = new ImageSourceData(Source[s], Rec, Direct_Data[s], PScene, new int[] { 0, 7 }, (int)Image_Order.Value, BTM_ED.Checked.Value, s);

                        Rhino.RhinoApp.RunScript("Run_Simulation", false);
                        if (command.CommandResult != Rhino.Commands.Result.Cancel)
                        {
                            ProgressBox VB = new ProgressBox("Creating IR filters for deterinistic reflections...");
                            VB.ShowModalAsync();
                            IS_Data[s] = ((ImageSourceData)command.Sim);
                            IS_Data[s].Create_Filter(Source[s].SWL(), 4096, VB);
                            VB.Close();
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                        command.Reset();
                    }

                    if (ISBox.Checked.Value && Specular_Trace.Checked.Value)
                    {
                        command.Sim = new IS_Trace(Source[s], Rec, PScene, ((double)(CO_TIME.Value / 1000) * PScene.Sound_speed(0)), (int)Spec_RayCount.Value, (int)Image_Order.Value, PScene.Sound_speed(0), SampleRate);
                        Rhino.RhinoApp.RunScript("Run_Simulation", false);
                        if (command.CommandResult != Rhino.Commands.Result.Cancel)
                        {
                            ProgressBox VB = new ProgressBox("Creating IR filters for Deterministic Reflections...");
                            VB.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
                            IS_Data[s].Lookup_Sequences(((IS_Trace)(command.Sim)).IS_Sequences());
                            IS_Data[s].Create_Filter(Source[s].SWL(), 4096, VB);
                            VB.Close();
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                        command.Reset();
                    }
                    if (RTBox.Checked.Value)
                    {
                        ConvergenceProgress CP = new ConvergenceProgress();
                        command.Sim = new SplitRayTracer(Source[s], Rec, Flex_Scene, ((double)(CO_TIME.Value / 1000) * PScene.Sound_speed(0)), new int[2] { 0, 7 }, Specular_Trace.Checked.Value ? int.MaxValue : ISBox.Checked.Value ? (int)Image_Order.Value : 0, Minimum_Convergence.Checked ? -1 : DetailedConvergence.Checked ? 0 : (int)RT_Count.Value, CP);
                        if (!Spec_Rays.Checked) CP.Show();

                        Rhino.RhinoApp.RunScript("Run_Simulation", false);
                        if (command.CommandResult != Rhino.Commands.Result.Cancel)
                        {
                            SplitRayTracer RT_Data = (SplitRayTracer)command.Sim;
                            Receiver[s] = RT_Data.GetReceiver;
                            ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                            VB.ShowModalAsync(Rhino.UI.RhinoEtoApp.MainWindow);//(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
                            Receiver[s].Create_Filter(VB);
                            VB.Close();
                            Rhino.RhinoApp.WriteLine(string.Format("{0} Rays ({1} sub-rays) cast in {2} hours, {3} minutes, {4} seconds.", RT_Data._currentRay.Sum(), RT_Data._rayTotal.Sum(), RT_Data._ts.Hours, RT_Data._ts.Minutes, RT_Data._ts.Seconds));
                            Rhino.RhinoApp.WriteLine("Percentage of energy lost: {0}%", RT_Data.PercentLost);
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                        command.Reset();
                    }
                }

                if (Source != null)
                {
                    List<Hare.Geometry.Point> R;
                    plugin.Receiver(out R);
                    Recs = new Hare.Geometry.Point[R.Count];
                    for (int q = 0; q < R.Count; q++)
                    {
                        Recs[q] = R[q];
                    }
                    if (SavePath != null) Utilities.FileIO.Write_Pac1(SavePath, Direct_Data, IS_Data, Receiver);

                    OpenAnalysis();
                    cleanup();
                }

                Populate_Sources();
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
                        //SourceList.Item.Add(String.Format("S{0}-", S.Source_ID()) + S.Type());
                        Source_Aim.Items.Add(String.Format("S{0}-", S.Source_ID()) + S.Type());
                        SrcTypeList.Add(S.Type());
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

            private void OpenAnalysis()
            {
                if (IS_Data != null)
                {
                    IS_Path_Box.Clear();
                    if (IS_Data != null && IS_Data[0] != null)
                    {
                        List<int> srcs = SourceList.SelectedSources();
                        if (srcs.Count == 0) return;
                        foreach (int i in srcs)
                        {
                            //Deterministic_Reflection[] S = IS_Data[i].Paths[int.Parse(Receiver_Choice.Text)].ToArray();
                            IS_Path_Box.Populate(IS_Data, srcs, ReceiverSelection.SelectedIndex);
                            //foreach (Deterministic_Reflection s in S) IS_Path_Box.Items.Add(s);
                        }
                        PathCount.Text = string.Format("{0} Deterministic Reflections", IS_Path_Box.Count);
                        //SortPaths(null, null);
                    }
                }
            }
            #endregion

            #region Tab 2: Materials Tab 

            private List<layer> LayerNames = new List<layer>();
            private Acoustics_Library Materials = new Acoustics_Library();

            private void Materials_MouseEnter(object sender, System.EventArgs e)
            {
                if (Tabs.SelectedPage.Text == "Materials")
                {
                    string Selection = LayerDisplay.SelectedValue.ToString();
                    LayerNames.Clear();
                    for (int q = 0; q < Rhino.RhinoDoc.ActiveDoc.Layers.Count; q++)
                    {
                        if (Rhino.RhinoDoc.ActiveDoc.Layers[q].IsDeleted) continue;
                        LayerNames.Add(new layer(Rhino.RhinoDoc.ActiveDoc.Layers[q].Name, q));
                    }
                    LayerDisplay.Items.Clear();
                    for (int i = 0; i < LayerNames.Count; i++) if (LayerNames[i]!= null) LayerDisplay.Items.Add(LayerNames[i].name);
                    LayerDisplay.SelectedValue = Selection;
                }
            }

            public class layer
            {
                public string name;
                public int id;

                public layer(string n, int index)
                {
                    name = n;
                    id = index;
                }

                public override string ToString()
                {
                    return name;
                }
            }

            private void Acoustics_Coef_Update(object sender, System.EventArgs e)
            {
                //UpdateForm();
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
    
                double[] Abs = ABSSlider.Value;

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
                    for (int i = 0; i < Mat.Values.Length; i++) mat[i] = Mat.Values[i] * 100;

                    ABSSlider.Value = mat;
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
            }

            private void Commit_Layer_Acoustics()
            {
                if (LayerDisplay.SelectedValue.ToString().Length == 0) return;
                int layer_index = LayerDisplay.SelectedIndex;
                double[] Absd = ABSSlider.Value;
                double[] Sctd = SCATSlider.Value;
                int[] Abs = new int[Absd.Length];
                int[] Sct = new int[Sctd.Length];
                int[] Trn = null;
                for(int i = 0; i < Absd.Length; i++) Abs[i] = (int)(Absd[i]*10);
                for(int i = 0; i < Sctd.Length; i++) Sct[i] = (int)(Sctd[i]);

                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
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
                Rhino.RhinoDoc.ActiveDoc.Layers.Modify(layer, layer_index, false);
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
                    VB.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
                    for (int i = 0; i < IS_Data.Length; i++) if (IS_Data[i] != null) IS_Data[i].Create_Filter(Direct_Data[i].SWL, 4096, VB);
                    VB.change_title("Creating Impulse Responses...");
                    for (int i = 0; i < Receiver.Length; i++) Receiver[i].Create_Filter(VB);
                    VB.Close();
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
                //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                int layer_index = LayerDisplay.SelectedIndex;// (LayerDisplay.SelectedItem as layer).id;
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                string AC = layer.GetUserString("Acoustics");
                string TL = layer.GetUserString("Transmission");

                string M = layer.GetUserString("ABSType");
                if (M == "Buildup")
                {
                    Material_Mode(false);
                    string[] BU = layer.GetUserString("Buildup").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                    for (int i = 0; i < BU.Length; i++) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(BU[i]));

                    Environment.Smart_Material sm = new Smart_Material(false, Layers, 44100, 1.2, 343, 2);
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
                else if (M == "Buildup_Finite")
                {
                    Material_Mode(false);
                    string[] BU = layer.GetUserString("Buildup").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                    for (int i = 0; i < BU.Length; i++) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(BU[i]));

                    Environment.Smart_Material sm = new Smart_Material(false, Layers, 44100, 1.2, 343, 2);
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
                AD.ShowModal();

                switch (AD.Result)
                {
                    case Pach_Absorption_Designer.AbsorptionModelResult.Random_Incidence:
                        //Assign Random Incidence Absorption Coefficients...
                        Material_Mode(true);
                        double[] abs = new double[8];
                        for (int i = 0; i < 8; i++) abs[i] = AD.RI_Absorption[i] * 100;
                        ABSSlider.Value = abs;

                        //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                        int layer_index = LayerDisplay.SelectedIndex;
                        Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                        layer.SetUserString("ABSType", "Coefficients");

                        Commit_Layer_Acoustics();
                        Material_Mode(true);
                        return;
                    case Pach_Absorption_Designer.AbsorptionModelResult.Smart_Material:
                        //Store and Assign Smart Material.
                        Material_Mode(false);
                        Commit_SmartMaterial(AD);
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
                    Receiver_Choice.Items.Clear();

                    if (Direct_Data != null && Direct_Data[0] != null)
                    {
                        for (int i = 0; i < Direct_Data[0].Rec_Origin.Count(); i++)
                        {
                            Receiver_Choice.Items.Add(i.ToString());
                        }
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

            private void Update_Parameters()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null) { return; }
                double[] Schroeder;

                List<int> SrcIDs = SourceList.SelectedSources();

                if (SrcIDs.Count == 0) return;

                if (SrcIDs.Count > 1 || SrcIDs.Count < 1)
                {
                    ISOCOMP.Text = "ISO Compliant: No";
                    if (Parameter_Choice.SelectedValue.ToString() == "Strength/Loudness (G)")
                    {
                        SRT1.Text = "62.5 hz. : NA";
                        SRT2.Text = "125 hz. : NA";
                        SRT3.Text = "250 hz. : NA";
                        SRT4.Text = "500 hz. : NA";
                        SRT5.Text = "1000 hz. : NA";
                        SRT6.Text = "2000 hz. : NA";
                        SRT7.Text = "4000 hz. : NA";
                        SRT8.Text = "8000 hz. : NA";
                        return;
                    }
                }
                else if (SrcTypeList[SrcIDs[0]] == "Pseudo-Random" || SrcTypeList[SrcIDs[0]] != "Geodesic")
                {
                    ISOCOMP.Text = "ISO Compliant: No";
                }
                else
                {
                    ISOCOMP.Text = "ISO Compliant: Yes";
                }

                bool Echo10, Echo50;
                double[] EKG, PercEcho;
                double dtime = double.PositiveInfinity;

                foreach (int i in SourceList.SelectedSources())
                {
                    double d = Direct_Data[i].Min_Time(int.Parse(Receiver_Choice.Text));
                    dtime = Math.Min(dtime, d);
                }

                bool pressure;
                double[][] ETC = new double[8][];
                if (Graph_Type.Text == "Pressure Time Curve")
                {
                    double RhoC = Direct_Data[0].Rho_C[int.Parse(Receiver_Choice.Text)];
                    ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                    VB.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
                    double[] ETC_BB = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, VB);
                    VB.Close();
                    ///Need a new PT_Curve method that will apply the correct power to each filter appropriately. This one forces only one power level to be used...)
                    for (int oct = 0; oct < 8; oct++)
                    {
                        ETC[oct] = Audio.Pach_SP.FIR_Bandpass(ETC_BB, oct, SampleRate, 0);
                        for (int i = 0; i < ETC[oct].Length; i++) ETC[oct][i] *= ETC[oct][i] / RhoC;
                    }
                    pressure = true;
                }
                else
                {
                    for (int oct = 0; oct < 8; oct++) ETC[oct] = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, int.Parse(Receiver_Choice.Text), SrcIDs, false);
                    pressure = false;
                }

                if (Parameter_Choice.SelectedIndex < 0) Parameter_Choice.SelectedIndex = 0;

                switch (Parameter_Choice.SelectedValue.ToString())
                {
                    case "Early Decay Time":

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(EDT, 2));
                        break;

                    case "T-10":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T10 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T10, 2));
                        break;

                    case "T-15":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T15, 2));
                        break;

                    case "T-20":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T20, 2));
                        break;

                    case "T-30":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T30, 2));
                        break;

                    case "Strength/Loudness (G)":

                        double G = AcousticalMath.Strength(ETC[0], Direct_Data[SrcIDs[0]].SWL[0], false);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[1], Direct_Data[SrcIDs[0]].SWL[1], false);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[2], Direct_Data[SrcIDs[0]].SWL[2], false);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[3], Direct_Data[SrcIDs[0]].SWL[3], false);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[4], Direct_Data[SrcIDs[0]].SWL[4], false);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[5], Direct_Data[SrcIDs[0]].SWL[5], false);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[6], Direct_Data[SrcIDs[0]].SWL[6], false);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[7], Direct_Data[SrcIDs[0]].SWL[7], false);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(G, 2));
                        break;

                    case "Sound Pressure Level (SPL)":

                        double I = 0;
                        for (int i = 0; i < ETC[0].Length; i++) I += ETC[0][i];
                        double SPL = AcousticalMath.SPL_Intensity(I);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[1].Length; i++) I += ETC[1][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[2].Length; i++) I += ETC[2][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[3].Length; i++) I += ETC[3][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[4].Length; i++) I += ETC[4][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[5].Length; i++) I += ETC[5][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[6].Length; i++) I += ETC[6][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[7].Length; i++) I += ETC[7][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(SPL, 2));
                        break;

                    case "Clarity (C-50)":
                        double C50 = AcousticalMath.Clarity(ETC[0], SampleRate, 0.05, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[1], SampleRate, 0.05, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[2], SampleRate, 0.05, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[3], SampleRate, 0.05, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[4], SampleRate, 0.05, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[5], SampleRate, 0.05, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[6], SampleRate, 0.05, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[7], SampleRate, 0.05, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(C50, 2));
                        break;

                    case "Clarity (C-80)":

                        double C80 = AcousticalMath.Clarity(ETC[0], SampleRate, 0.08, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[1], SampleRate, 0.08, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[2], SampleRate, 0.08, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[3], SampleRate, 0.08, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[4], SampleRate, 0.08, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[5], SampleRate, 0.08, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[6], SampleRate, 0.08, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[7], SampleRate, 0.08, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(C80, 2));
                        break;

                    case "Definition (D-50)":
                        double D50 = AcousticalMath.Definition(ETC[0], SampleRate, 0.05, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[1], SampleRate, 0.05, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[2], SampleRate, 0.05, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[3], SampleRate, 0.05, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[4], SampleRate, 0.05, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[5], SampleRate, 0.05, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[6], SampleRate, 0.05, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[7], SampleRate, 0.05, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0} %", Math.Round(D50, 2));
                        break;
                    case "Center Time (TS)":
                        double TS = AcousticalMath.Center_Time(ETC[0], SampleRate, dtime);
                        SRT1.Text = string.Format("62.5 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[1], SampleRate, dtime);
                        SRT2.Text = string.Format("125 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[2], SampleRate, dtime);
                        SRT3.Text = string.Format("250 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[3], SampleRate, dtime);
                        SRT4.Text = string.Format("500 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[4], SampleRate, dtime);
                        SRT5.Text = string.Format("1000 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[5], SampleRate, dtime);
                        SRT6.Text = string.Format("2000 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[6], SampleRate, dtime);
                        SRT7.Text = string.Format("4000 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[7], SampleRate, dtime);
                        SRT8.Text = string.Format("8000 hz. : {0} ms.", Math.Round(TS * 1000, 2));
                        break;
                    case "Initial Time Delay Gap (ITDG)":
                        double ITDG = AcousticalMath.InitialTimeDelayGap(ETC[0], SampleRate);
                        SRT1.Text = string.Format("62.5 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[1], SampleRate);
                        SRT2.Text = string.Format("125 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[2], SampleRate);
                        SRT3.Text = string.Format("250 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[3], SampleRate);
                        SRT4.Text = string.Format("500 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[4], SampleRate);
                        SRT5.Text = string.Format("1000 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[5], SampleRate);
                        SRT6.Text = string.Format("2000 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[6], SampleRate);
                        SRT7.Text = string.Format("4000 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[7], SampleRate);
                        SRT8.Text = string.Format("8000 hz. : {0} ms", ITDG);
                        break;
                    case "Speech Transmission Index (Explicit)":
                        //Speech Intelligibility Index (Statistical)
                        double[] Noise = new double[8];
                        string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                        if (n != "" && n != null)
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

                        double[] STI = AcousticalMath.Speech_Transmission_Index(ETC, 1.2 * 343, Noise, SampleRate);
                        SRT1.Text = string.Format("General(2003) : {0}", Math.Round(STI[0], 2));
                        SRT2.Text = string.Format("Male : {0}", Math.Round(STI[1], 2));
                        SRT3.Text = string.Format("Female : {0}", Math.Round(STI[2], 2));
                        SRT4.Text = "";
                        SRT5.Text = "";
                        SRT6.Text = "";
                        SRT7.Text = "";
                        SRT8.Text = "";
                        break;
                    case "Modulation Transfer Index (MTI - root STI)":
                        //Speech Intelligibility Index (Statistical)
                        double[] p_2m = new double[8];
                        //for (int oct = 0; oct < 8; oct++) p_2m[oct] = Direct_Data[SrcIDs[0]].EnergyValue(oct, int.Parse(Receiver_Choice.Text))[0] * 413;
                        double[] NoiseM = new double[8];
                        string N = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                        if (N != "")
                        {
                            string[] ns = N.Split(","[0]);
                            for (int oct = 0; oct < 8; oct++) NoiseM[oct] = double.Parse(ns[oct]);
                        }
                        else
                        {
                            this.Enabled = false;
                            Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                            N = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                            string[] ns = N.Split(","[0]);
                            for (int oct = 0; oct < 8; oct++) NoiseM[oct] = double.Parse(ns[oct]);
                            this.Enabled = true;
                        }

                        double[][] ETCm = new double[8][];
                        for (int oct = 0; oct < 8; oct++)
                        {
                            ETCm[oct] = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, int.Parse(Receiver_Choice.Text), SrcIDs, false);
                        }
                        double[] MTI = AcousticalMath.Modulation_Transfer_Index(ETCm, 1.2 * 343, NoiseM, SampleRate);
                        SRT1.Text = "";
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(MTI[0], 2));
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(MTI[1], 2));
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(MTI[2], 2));
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(MTI[3], 2));
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(MTI[4], 2));
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(MTI[5], 2));
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(MTI[6], 2));
                        break;
                    case "Lateral Fraction (LF)":
                        double LF = AcousticalMath.Lateral_Fraction(ETC[0], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 0, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[1], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 1, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[2], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 2, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[3], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 3, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[4], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 4, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[5], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 5, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[6], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 6, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[7], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 7, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(LF, 2));
                        break;
                    case "Lateral Efficiency (LE)":
                        double LE = AcousticalMath.Lateral_Efficiency(ETC[0], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 0, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[1], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 1, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[2], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 2, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[3], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 3, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[4], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 4, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[5], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 5, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[6], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 6, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[7], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 7, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SrcIDs[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(LE, 2));
                        break;
                    case "Echo Criterion (Music, 10%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo10);
                        break;
                    case "Echo Criterion (Music, 50%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo50);
                        break;
                    case "Echo Criterion (Speech, 10%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo10);
                        break;
                    case "Echo Criterion (Speech, 50%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true, null), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo50);
                        break;
                }
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
                            foreach (Hare.Geometry.Point p in P) pts.Add(Utilities.RCPachTools.HPttoRPt(p));
                            ShownPaths.Add(Rhino.RhinoDoc.ActiveDoc.Objects.AddPolyline(new Polyline(pts)));
                        }
                    }
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                Update_Graph(null, null);
            }

            private void Update_Graph(object sender, EventArgs e)
            {
                Analysis_View.Plot.Clear();

                int REC_ID = 0;
                try
                {
                    if (Receiver_Choice.Text == "No Results Calculated...") return;
                    REC_ID = int.Parse(Receiver_Choice.Text);

                    int OCT_ID = PachTools.OctaveStr2Int(Graph_Octave.Text);
                    Analysis_View.Plot.Title("Logarithmic Energy Time Curve");
                    Analysis_View.Plot.XAxis.Label.Text = "Time (seconds)";
                    Analysis_View.Plot.YAxis.Label.Text = "Sound Pressure Level (dB)";

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
                            zero_sample = 4096 / 2;
                            ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                            VB.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
                            Filter2 = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, true, VB);
                            VB.Close();
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
                            }
                            Filter = new double[Filter2.Length];
                            for (int i = 0; i < Filter.Length; i++)   Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Lateral ETC":
                            Filter = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1];
                            for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Lateral PTC":
                            zero_sample = 4096 / 2;
                            Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[1];
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
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
                            zero_sample = 4096 / 2;
                            Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[2];
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
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
                            zero_sample = 4096 / 2;
                            Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[0];
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
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
                                Selected.Add(i.TravelTime + (double)j / (double)SampleRate, Utilities.AcousticalMath.SPL_Intensity(refl[j]));
                            }
                        }

                        for (int i = 0; i < Selected.Count; i++)
                        {
                            S_time.Add(Selected.Keys[i]);
                            S_Power.Add(Selected.Values[i]);
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

                    Analysis_View.Plot.Add.Signal(Schroeder, 1.0/44100.0, ScottPlot.Colors.Red);
                    Analysis_View.Plot.Add.Signal(Filter, 1.0/44100.0, ScottPlot.Colors.Blue);

                    if (Paths.Count > 0)
                    {
                        ScottPlot.Plottables.Scatter IScurve = Analysis_View.Plot.Add.Scatter(S_time, S_Power, ScottPlot.Colors.Red);
                        IScurve.LineStyle.IsVisible = false;
                    }

                    if (!LockUserScale.Checked.Value)
                    {
                        Analysis_View.Plot.XAxis.Max = time[time.Length - 1];
                        Analysis_View.Plot.XAxis.Min = time[0];

                        if (Normalize_Graph.Checked.Value)
                        {
                            Analysis_View.Plot.YAxis.Max = 0;
                            Analysis_View.Plot.YAxis.Min = -100;
                        }
                        else
                        {
                            Analysis_View.Plot.YAxis.Max = DirectMagnitude + 15;
                            Analysis_View.Plot.YAxis.Min = 0;
                        }
                    }
                    else
                    {
                        double max = Analysis_View.Plot.YAxis.Max;
                        double min = Analysis_View.Plot.YAxis.Min;

                        if (Normalize_Graph.Checked.Value)
                        {
                            Analysis_View.Plot.YAxis.Max = max;
                            Analysis_View.Plot.YAxis.Min = min;
                        }
                        else
                        {
                            Analysis_View.Plot.YAxis.Max = max;
                            Analysis_View.Plot.YAxis.Min = min;
                        } 
                    }

                    Hare.Geometry.Vector V = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(float)Alt_Choice.Value, true), -(float)Azi_Choice.Value, 0, true);

                    if (Receiver_Choice.SelectedIndex >= 0) ReceiverConduit.Instance.set_direction(Utilities.RCPachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), new Vector3d(V.x, V.y, V.z));
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                }
                catch (Exception x)
                {
                    Eto.Forms.MessageBox.Show(x.Message);
                    return;
                }

                Analysis_View.Invalidate();
                Update_Parameters();
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

            private void Source_Aim_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Receiver_Choice.SelectedIndex < 0 || Source_Aim.SelectedIndex < 0) return;
                double azi, alt;

                PachTools.World_Angles(Direct_Data[Source_Aim.SelectedIndex].Src.Origin(), Recs[Receiver_Choice.SelectedIndex], true, out alt, out azi);

                Alt_Choice.Value = alt;
                Azi_Choice.Value = azi;
            }

            public void GetSims(ref Hare.Geometry.Point[] Src, ref Hare.Geometry.Point[] Rec, ref Direct_Sound[] D, ref ImageSourceData[] IS, ref Receiver_Bank[] RT)
            {
                if (Source != null)
                {
                    Src = new Hare.Geometry.Point[Source.Length];
                    for (int i = 0; i < Source.Length; i++) Src[i] = Source[i].H_Origin();
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
                Eto.Forms.OpenFileDialog OF = new Eto.Forms.OpenFileDialog();
                OF.CurrentFilter = ".pac1";
                OF.Filters.Add("Pachyderm Ray Data file (*.pac1)|*.pac1|" + "All Files|");
                if (OF.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                { 
                    Read_File(OF.FileName);
                    Update_Graph(null, System.EventArgs.Empty);
                } 
            }

            private void Receiver_Choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Parameters();
                Source_Aim_SelectedIndexChanged(null, EventArgs.Empty);
                Update_Graph(null, System.EventArgs.Empty);
                OpenAnalysis();
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
                sf.CurrentFilter = ".txt";
                sf.Filters.Add("Text File (*.txt)|*.txt|" + "All Files|");

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

                                double RhoC = Direct_Data[0].Rho_C[int.Parse(Receiver_Choice.Text)];
                                ProgressBox VB = new ProgressBox("Creating Impulse Responses...");
                                VB.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
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

            private void Auralisation_Click(object sender, EventArgs e)
            {
                Rhino.RhinoApp.RunScript("PachyDerm_Auralisation", false);
            }

            //private void DelayMod_Click(object sender, EventArgs e)
            //{
            //    //Interface for time selection...
            //    double t = Direct_Data[SourceList.SelectedSources()[0]].Delay_ms;
            //    Rhino.Input.RhinoGet.GetNumber("Enter the delay to assign to selected source object(s)...", false, ref t, 0, 200);

            //    foreach (int id in SourceList.SelectedIndices)
            //    {
            //        Direct_Data[id].Delay_ms = t;
            //    }
            //    Update_Graph(null, null);
            //}

            //private void Source_Power_Mod_Click(object sender, EventArgs e)
            //{
            //    List<int> srcs = SourceList.SelectedSources();
            //    if (srcs.Count < 1) return;
            //    Pachyderm_Acoustic.SourcePowerMod mod = new SourcePowerMod(Direct_Data[srcs[0]].SWL);
            //    mod.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow);
            //    if (mod.accept)
            //    {
            //        //Pressure_Ready = false;
            //        foreach (int i in srcs)
            //        {
            //            double[] factor = Direct_Data[i].Set_Power(mod.Power);
            //            IS_Data[i].Set_Power(factor);
            //            Receiver[i].Set_Power(factor);
            //            Direct_Data[i].Create_Filter();
            //            IS_Data[i].Create_Filter(mod.Power, 4096);
            //            Receiver[i].Create_Filter();
            //        }
            //    }
            //    Update_Graph(null, null);
            //}

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

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Calculate.Dispose();
                RTBox.Dispose();
                ISBox.Dispose();
                Tabs.Dispose();
                TabImpulse.Dispose();
                Image_Order.Dispose();
                RT_Count.Dispose();
                CO_TIME.Dispose();
                Spec_RayCount.Dispose();
                TabAnalysis.Dispose();
                TabMaterials.Dispose();
                Material_Lib.Dispose();
                ReceiverSelection.Dispose();
                ParameterBox.Dispose();
                LayerDisplay.Dispose();
                PathCount.Dispose();
                Parameter_Choice.Dispose();
                MediumProps.Dispose();
                Specular_Trace.Dispose();
                Receiver_Choice.Dispose();
                ScatFlat.Dispose();
                SaveAbsBox.Dispose();
                Save_Material.Dispose();
                Material_Name.Dispose();
                EdgeFreq.Dispose();
                SourceList.Dispose();
                ISOCOMP.Dispose();
                BTM_ED.Dispose();
                Alt_Choice.Dispose();
                Azi_Choice.Dispose();
                IS_Path_Box.Dispose();
                Graph_Octave.Dispose();
                Analysis_View.Dispose();
                Normalize_Graph.Dispose();
                LockUserScale.Dispose();
                Graph_Type.Dispose();
                Source_Aim.Dispose();
                AimatSrc.Dispose();
                Abs_Designer.Dispose();
                SmartMat_Display.Dispose();
                tabCoef.Dispose();
                Absorption.Dispose();
                Scattering.Dispose();
                Transparency.Dispose();
                Trans_Flat.Dispose();

                MenuStrip.Dispose();
                fileToolStripMenuItem.Dispose();
                saveDataToolStripMenuItem.Dispose();
                openDataToolStripMenuItem.Dispose();
                saveParameterResultsToolStripMenuItem.Dispose();
                savePTBFormatToolStripMenuItem.Dispose();
                saveEDCToolStripMenuItem.Dispose();
                savePressureResultsToolStripMenuItem.Dispose();
                savePressurePTBFormatToolStripMenuItem.Dispose();

                Auralisation.Dispose();
                Delete_Material.Dispose();

                Spec_Rays.Dispose();
                DetailedConvergence.Dispose();
                Minimum_Convergence.Dispose();
                quart_lambda.Dispose();
                user_quart_lambda.Dispose();
                PlasterScatter.Dispose();
                GlassScatter.Dispose();

                tabTransControls.Dispose();
                tabTC.Dispose();
                Trans_Check.Dispose();
                tabTL.Dispose();

                SaveTLBox.Dispose();
                DeleteAssembly.Dispose();
                SaveAssembly.Dispose();
                IsolationAssemblies.Dispose();
                TL_Check.Dispose();
                Isolation_Lib.Dispose();
                labelVar.Dispose();
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
                sf.CurrentFilter = ".pac1";
                sf.Filters.Add("Pachyderm Ray Data file (*.pac1)|*.pac1|" + "All Files|");
                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    System.IO.BinaryWriter sw = new System.IO.BinaryWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    //1. Date & Time
                    sw.Write(System.DateTime.Now.ToString());
                    //2. Plugin Version... if less than 1.1, assume only 1 source.
                    sw.Write(plugin.Version);
                    //3. Cut off Time (seconds) and SampleRate
                    sw.Write((double)CO_TIME.Value);
                    sw.Write(SampleRate);
                    //4.0 Source Count(int)
                    Hare.Geometry.Point[] SRC;
                    plugin.SourceOrigin(out SRC);
                    sw.Write(SRC.Length);
                    for (int i = 0; i < SRC.Length; i++)
                    {
                        //4.1 Source Location x (double)    
                        sw.Write(SRC[i].x);
                        //4.2 Source Location y (double)
                        sw.Write(SRC[i].y);
                        //4.3 Source Location z (double)
                        sw.Write(SRC[i].z);
                    }
                    //5. No of Receivers
                    sw.Write(Recs.Length);

                    //6. Write the coordinates of each receiver point
                    for (int q = 0; q < Recs.Length; q++)
                    {
                        sw.Write(Recs[q].x);
                        sw.Write(Recs[q].y);
                        sw.Write(Recs[q].z);
                        sw.Write(Receiver[0].Rec_List[q].Rho_C);
                    }

                    for (int s = 0; s < SRC.Length; s++)
                    {
                        if (Direct_Data != null)
                        {
                            //7. Write Direct Sound Data
                            Direct_Data[s].Write_Data(ref sw);
                        }

                        if (IS_Data[0] != null)
                        {
                            //8. Write Image Source Sound Data
                            IS_Data[s].Write_Data(ref sw);
                        }

                        if (Receiver != null)
                        {
                            //9. Write Ray Traced Sound Data
                            Receiver[s].Write_Data(ref sw);
                        }
                    }
                    sw.Write("End");
                    sw.Close();
                }
            }

            private bool Read_File(string path)
            {
                if (FileIO.Read_Pac1(ref Direct_Data, ref IS_Data, ref Receiver, path))
                {
                    SourceList.Clear();
                    LockUserScale.Checked = false;
                    Update_Graph(null, new System.EventArgs());
                    LockUserScale.Checked = true;
                    Receiver_Choice.Text = "0";
                    OpenAnalysis();
                    cleanup();

                    Source = new Source[Direct_Data.Length];
                    Recs = new Hare.Geometry.Point[Receiver[0].Count];

                    for (int DDCT = 0; DDCT < Direct_Data.Length; DDCT++)
                    {
                        Source[DDCT] = Direct_Data[DDCT].Src;
                        SourceList.Populate(Direct_Data);
                        Source_Aim.Items.Add(string.Format("S{0}-", DDCT) + Direct_Data[DDCT].type);
                        SrcTypeList.Add(Direct_Data[DDCT].type);
                    }

                    CutoffTime = Direct_Data[0].Cutoff_Time;
                    SampleRate = (int)Direct_Data[0].SampleRate;

                    for (int i = 0; i < Recs.Length; i++) Recs[i] = Receiver[0].Rec_List[i].Origin;
                    return true;
                }
                else
                {
                    Eto.Forms.MessageBox.Show("File Read Failed...", "Results file was corrupt or incomplete. We apologize for this inconvenience. Please report this to the software author. It will be much appreciated.");
                    return false;
                }
            }

            public void Plot_Results_Intensity()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.SelectedValue.ToString() != "Sabine RT" && Parameter_Choice.SelectedValue.ToString() != "Eyring RT") { return; }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.CurrentFilter = ".txt";
                sf.Filters.Add("Text File (*.txt)|*.txt|" + "All Files|");

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
                                double[] ETC = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, s, false);
                                Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                                EDT[s, r, oct] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                                T10[s, r, oct] = AcousticalMath.T_X(Schroeder, 10, SampleRate);
                                T15[s, r, oct] = AcousticalMath.T_X(Schroeder, 15, SampleRate);
                                T20[s, r, oct] = AcousticalMath.T_X(Schroeder, 20, SampleRate);
                                T30[s, r, oct] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                                G[s, r, oct] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                                C50[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                C80[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                                D50[s, r, oct] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                TS[s, r, oct] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                                double[] L_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int>() { s }, false, (double)this.Alt_Choice.Value, (double)this.Azi_Choice.Value, true)[1];
                                LF[s, r, oct] = AcousticalMath.Lateral_Fraction(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                                LE[s, r, oct] = AcousticalMath.Lateral_Efficiency(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
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
                sf.CurrentFilter = ".txt";
                sf.Filters.Add("Text File (*.txt)|*.txt|" + "All Files|");

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
                            VB.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
                            double[] PTC = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new System.Collections.Generic.List<int>(1) { s }, false, true, VB);
                            VB.Close();
                            for (int oct = 0; oct < 8; oct++)
                            {
                                double[] ETC = Pachyderm_Acoustic.Audio.Pach_SP.FIR_Bandpass(PTC, oct, SampleRate, 0);
                                for (int i = 0; i < ETC.Length; i++) { ETC[i] = AcousticalMath.Intensity_Pressure(ETC[i]); }; Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                                EDT[s, r, oct] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                                T10[s, r, oct] = AcousticalMath.T_X(Schroeder, 10, SampleRate);
                                T15[s, r, oct] = AcousticalMath.T_X(Schroeder, 15, SampleRate);
                                T20[s, r, oct] = AcousticalMath.T_X(Schroeder, 20, SampleRate);
                                T30[s, r, oct] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                                G[s, r, oct] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                                C50[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                C80[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                                D50[s, r, oct] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                TS[s, r, oct] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                                double[] L_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int>() { s }, false, (double)this.Alt_Choice.Value, (double)this.Azi_Choice.Value, true)[1];
                                LF[s, r, oct] = AcousticalMath.Lateral_Fraction(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                                LE[s, r, oct] = AcousticalMath.Lateral_Efficiency(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
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
                            double[] ETC = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, s, false);
                            Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                            ParamValues[s, r, oct, 0] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                            ParamValues[s, r, oct, 1] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                            ParamValues[s, r, oct, 2] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 3] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 4] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                            ParamValues[s, r, oct, 5] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                            double azi, alt;
                            PachTools.World_Angles(Direct_Data[s].Src.Origin(), Recs[r], true, out alt, out azi);
                            double[][] Lateral_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int> { s }, false, alt, azi, true);
                            ParamValues[s, r, oct, 6] = AcousticalMath.Lateral_Fraction(ETC, Lateral_ETC, SampleRate, Direct_Data[s].Min_Time(r), false);
                        }
                    }
                }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.CurrentFilter = ".txt";
                sf.Filters.Add("Text File (*.txt)|*.txt|" + "All Files|");

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
                        VB.ShowSemiModal(Rhino.RhinoDoc.ActiveDoc, Rhino.UI.RhinoEtoApp.MainWindow);
                        double[] PTC = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new System.Collections.Generic.List<int>(1) { s }, false, true, VB);
                        VB.Close();
                        for (int oct = 0; oct < 8; oct++)
                        {
                            double[] ETC = Pachyderm_Acoustic.Audio.Pach_SP.FIR_Bandpass(PTC, oct, SampleRate, 0);
                            for (int i = 0; i < ETC.Length; i++) { ETC[i] = AcousticalMath.Intensity_Pressure(ETC[i]); };
                            Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                            ParamValues[s, r, oct, 0] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                            ParamValues[s, r, oct, 1] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                            ParamValues[s, r, oct, 2] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 3] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 4] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                            ParamValues[s, r, oct, 5] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                            double azi, alt;
                            PachTools.World_Angles(Direct_Data[s].Src.Origin(), Recs[r], true, out alt, out azi);
                            double[][] Lateral_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int> { s }, false, alt, azi, true);
                            ParamValues[s, r, oct, 6] = AcousticalMath.Lateral_Fraction(ETC, Lateral_ETC, SampleRate, Direct_Data[s].Min_Time(r), false);
                        }
                    }
                }

                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.CurrentFilter = ".txt";
                sf.Filters.Add("Text File (*.txt)|*.txt|" + "All Files|");

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

            Label SRT1 = new Label();
            Label SRT2 = new Label();
            Label SRT3 = new Label();
            Label SRT4 = new Label();
            Label SRT5 = new Label();
            Label SRT6 = new Label();
            Label SRT7 = new Label();
            Label SRT8 = new Label();

            private SegmentedButton MenuStrip;
            private MenuSegmentedItem fileToolStripMenuItem;
            private ButtonMenuItem saveDataToolStripMenuItem;
            private ButtonMenuItem openDataToolStripMenuItem;
            private ButtonMenuItem saveParameterResultsToolStripMenuItem;
            private ButtonMenuItem savePTBFormatToolStripMenuItem;
            private ButtonMenuItem saveEDCToolStripMenuItem;
            private ButtonMenuItem savePressureResultsToolStripMenuItem;
            private ButtonMenuItem savePressurePTBFormatToolStripMenuItem;

            private Button Auralisation;
            private Button Delete_Material;
            private Label labelExp;
            private RadioButton Spec_Rays;
            private RadioButton DetailedConvergence;
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
        }
    }
}