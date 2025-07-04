//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2023, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit 
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

using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pachyderm_Acoustic.AbsorptionModels;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;
using System.Linq;
using ScottPlot.TickGenerators;
using ScottPlot.Interactivity.UserActions;
using Eto.Drawing;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        partial class Pach_Absorption_Designer
        {
            private void InitializeComponent()
            { 
                this.Title = "Materials Designer [EXPERIMENTAL]";
                this.Size = new Eto.Drawing.Size(1500, 800);
                this.Resizable = true;
                this.WindowStyle = WindowStyle.Default;
                this.ShowInTaskbar = true;

                this.Alpha_Normal = new ScottPlot.Eto.EtoPlot();
                Alpha_Normal.Size = new Eto.Drawing.Size(400, 350);
                this.Polar_Absorption = new ScottPlot.Eto.EtoPlot();
                Polar_Absorption.Size = new Eto.Drawing.Size(350, 350);
                this.Impedance_Graph = new ScottPlot.Eto.EtoPlot();
                Impedance_Graph.Size = new Eto.Drawing.Size(350, 350);

                Eto.Forms.Button Cancel;
                this.LayerList = new Eto.Forms.ListBox();
                this.Use_RI = new Eto.Forms.Button();
                this.label17 = new Eto.Forms.Label();
                this.Resistivity_Feedback = new Eto.Forms.Label();
                this.PorosityLabel = new Eto.Forms.Label();
                this.Porosity_Percent = new Eto.Forms.NumericStepper();
                this.Label1 = new Eto.Forms.Label();
                this.Sigma = new Eto.Forms.NumericStepper();
                this.label10 = new Eto.Forms.Label();
                this.ViscousCharacteristicLength = new Eto.Forms.NumericStepper();
                this.ThermalPermeability = new Eto.Forms.NumericStepper();
                this.label9 = new Eto.Forms.Label();
                this.label19 = new Eto.Forms.Label();
                this.Tortuosity = new Eto.Forms.NumericStepper();
                this.Smart_Mat = new Eto.Forms.Button();
            this.label16 = new Eto.Forms.Label();
            this.YoungsModulus = new Eto.Forms.NumericStepper();
            this.Solid_Density = new Eto.Forms.NumericStepper();
            this.PoissonsRatio = new Eto.Forms.NumericStepper();
            this.depth = new Eto.Forms.NumericStepper();
            this.label6 = new Eto.Forms.Label();
            this.label11 = new Eto.Forms.Label();
            this.label12 = new Eto.Forms.Label();
            this.label13 = new Eto.Forms.Label();
            this.pitch_label = new Eto.Forms.Label();
            this.diam_label = new Eto.Forms.Label();
            this.pitch = new Eto.Forms.NumericStepper();
            this.diameter = new Eto.Forms.NumericStepper();
            this.label21 = new Eto.Forms.Label();
            this.label22 = new Eto.Forms.Label();
            this.Air_Term = new Eto.Forms.RadioButton();
            this.Rigid_Term = new Eto.Forms.RadioButton();
            this.label4 = new Eto.Forms.Label();
            this.label5 = new Eto.Forms.Label();
            this.label14 = new Eto.Forms.Label();
            this.label15 = new Eto.Forms.Label();
            this.Zf_Incorp_Method = new Eto.Forms.ComboBox();
            this.Z_rad_feedback = new Eto.Forms.Label();
            this.Calc_Zr = new Eto.Forms.Button();
            this.Inf_Sample = new Eto.Forms.RadioButton();
            this.XDim = new Eto.Forms.NumericStepper();
            this.YDim = new Eto.Forms.NumericStepper();
            this.Fin_Sample = new Eto.Forms.RadioButton();
            this.Material_Type = new Eto.Forms.ComboBox();
            this.Up = new Eto.Forms.Button();
            this.Dn = new Eto.Forms.Button();
            this.Rem = new Eto.Forms.Button();
            this.Add = new Eto.Forms.Button();
            this.Chart_Contents = new Eto.Forms.ComboBox();
            this.IIR_Order = new Eto.Forms.NumericStepper();
            this.Direction_choice = new Eto.Forms.ComboBox();
            Cancel = new Eto.Forms.Button();

                ////////////////////////////////////////////////

                DynamicLayout All = new DynamicLayout();
                Sustainability_Reference = new DynamicLayout();
                DynamicLayout GraphLeft = new DynamicLayout();
                DynamicLayout CtrlRight = new DynamicLayout();
                All.AddRow(Sustainability_Reference, GraphLeft, CtrlRight);

                GraphLeft.AddRow(Alpha_Normal);
                DynamicLayout GraphCtrls = new DynamicLayout();
                GraphLeft.AddRow(GraphCtrls);

                ////////////////////////////////////////
                Label label23 = new Label();
                Label label24 = new Label();
                Label label25 = new Label();
                label23.Text = "IIR Filter Order";
                this.IIR_Order.Value = 3;
                label25.Text = "Direction:";
                this.Direction_choice.Items.Add("Normal");
                this.Direction_choice.Items.Add("2.5 degrees");
                this.Direction_choice.Items.Add("7.5 degrees");
                this.Direction_choice.Items.Add("12.5 degrees");
                this.Direction_choice.Items.Add("17.5 degrees");
                this.Direction_choice.Items.Add("22.5 degrees");
                this.Direction_choice.Items.Add("27.5 degrees");
                this.Direction_choice.Items.Add("32.5 degrees");
                this.Direction_choice.Items.Add("37.5 degrees");
                this.Direction_choice.Items.Add("42.5 degrees");
                this.Direction_choice.Items.Add("47.5 degrees");
                this.Direction_choice.Items.Add("52.5 degrees");
                this.Direction_choice.Items.Add("57.5 degrees");
                this.Direction_choice.Items.Add("62.5 degrees");
                this.Direction_choice.Items.Add("67.5 degrees");
                this.Direction_choice.Items.Add("72.5 degrees");
                this.Direction_choice.Items.Add("77.5 degrees");
                this.Direction_choice.Items.Add("82.5 degrees");
                this.Direction_choice.Items.Add("87.5 degrees");
                this.Direction_choice.SelectedIndex = 0;
                this.Direction_choice.SelectedIndexChanged += this.Direction_choice_SelectedIndexChanged;
                label24.Text = "Chart Contents:";
                this.Chart_Contents.Items.Add("Absorption");
                this.Chart_Contents.Items.Add("Transmission");
                this.Chart_Contents.Text = "Absorption";
                this.Chart_Contents.SelectedIndexChanged += this.Chart_Contents_SelectedIndexChanged;
                GraphCtrls.AddRow(label23, IIR_Order, label25, Direction_choice, label24, Chart_Contents); ;
                GraphCtrls.DefaultSpacing = new Eto.Drawing.Size(8, 8);

                ///Ctrls for graph display...

                DynamicLayout GraphLower = new DynamicLayout();
                GraphLower.AddRow(new Label() { Text = "Polar Absorption Coefficient" }, new Label() { Text = "Impedance" });
                GraphLower.AddRow(Polar_Absorption, Impedance_Graph);
                GraphLeft.AddRow(GraphLower);

                /////////////////////////////////////////////////////Right Size - Controls

                GroupBox TerminationBox = new GroupBox();
                TerminationBox.Text = "Termination Type";
                DynamicLayout DL = new DynamicLayout();
                this.Air_Term.Text = "Air Termination (Trans)";
                this.Air_Term.MouseUp += Rigid_Term_CheckedChanged;
                this.Rigid_Term.Checked = true;
                this.Rigid_Term.Text = "Rigid Termination (ABS Only)";
                this.Rigid_Term.MouseUp += this.Rigid_Term_CheckedChanged;
                DL.AddRow(Rigid_Term, Air_Term);
                TerminationBox.Content = DL;
                Scrollable LLScroll = new Scrollable();
                this.LayerList.SelectedIndexChanged += this.LayerList_SelectedIndexChanged;
                LLScroll.Content = LayerList;
                this.label21.Text = "-- Rigid Substrate (Infinite Impedance) --";
                this.label22.Text = "-- Incident Side (Air [Z = rho * c)] --";
                this.label21.Visible = true;
                this.label22.Visible = true;
                DynamicLayout Ctrl12 = new DynamicLayout();
                Ctrl12.AddRow(TerminationBox);
                Ctrl12.AddRow(label21);
                Ctrl12.AddRow(LLScroll);
                Ctrl12.AddRow(label22);

                DynamicLayout Ctrl3 = new DynamicLayout();
                Label label8 = new Label();
                label8.Text = "Sample Type";

                this.Material_Type.Items.Add("Air Space");
                this.Material_Type.Items.Add("Porous Absorber (Biot)");
                this.Material_Type.Items.Add("Porous Absorber (Delany-Bazley)");
                this.Material_Type.Items.Add("Porous Absorber (Champoux-Allard)");
                this.Material_Type.Items.Add("Porous Absorber (Miki)");
                this.Material_Type.Items.Add("Solid Plate");
                this.Material_Type.Items.Add("Perforated Plate (Modal Solution)");
                this.Material_Type.Items.Add("Slotted Plate (Modal Solution)");
                this.Material_Type.Items.Add("Perforated Plate (Circular)");
                this.Material_Type.Items.Add("Perforated Plate (Square)");
                this.Material_Type.Items.Add("Slotted Plate");
                this.Material_Type.Items.Add("Micro-slit Plate");
                this.Material_Type.Items.Add("Micro-Perforated Plate");
                this.Material_Type.SelectedIndexChanged += this.comboBox1_SelectedIndexChanged;
                Ctrl3.AddRow(label8, Material_Type);
                Ctrl3.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                Ctrl3.Padding = 8;

                DynamicLayout Ctrl4 = new DynamicLayout();
                Ctrl4.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                Ctrl4.Padding = 8;
                this.Up.Text = "Layer Up";
                this.Up.Click += this.Up_Click;
                this.Dn.Text = "Layer Down";
                this.Dn.Click += this.Down_Click;
                this.Add.Text = "Add Layer";
                this.Add.Click += this.Add_Click;
                this.Rem.Text = "Remove Layer";
                this.Rem.Click += this.Rem_Click;
                Rem.Width = 75;
                Up.Width = 75;
                Dn.Width = 75;
                Ctrl4.AddRow(Add, Rem, Up, Dn);

                DynamicLayout ZF = new DynamicLayout();
                ZF.DefaultSpacing = new Eto.Drawing.Size(8, 8);

                Label label3 = new Label();
                label3.Text = "X Dimension (m)";

                this.XDim.DecimalPlaces = 1;
                this.XDim.Increment = 1;
                this.XDim.Value = 2;
                Label label20 = new Label();
                label20.Text = "Correction Method";
                this.Zf_Incorp_Method.Items.Add("a = 4*Za.Real*Zf.Real / |Za + Zf|^2");
                this.Zf_Incorp_Method.Items.Add("R = [(Za + Zf) - 1] / [(Za + Zf) + 1]");
                this.Zf_Incorp_Method.SelectedIndexChanged += this.Zf_Incorp_Method_SelectedIndexChanged;
                Inf_Sample.Checked = true;
                Inf_Sample.CheckedChanged += this.Inf_Sample_CheckedChanged;
                Fin_Sample.CheckedChanged += this.Fin_Sample_CheckedChanged;
                ZF.AddRow(Inf_Sample, label3, XDim, label20, Zf_Incorp_Method);

                this.Inf_Sample.Text = "Infinite Sample";
                this.Fin_Sample.Text = "Finite Sample";
                Label label7 = new Eto.Forms.Label();
                label7.Text = "Y Dimension (m)";
                this.YDim.DecimalPlaces = 1;
                this.YDim.Increment = 1;
                this.YDim.Value = 2;
                this.Z_rad_feedback.Text = "No Z-rad Stored";

                ZF.AddRow(Fin_Sample, label7, YDim, Z_rad_feedback);

                this.Calc_Zr.Text = "Calculate Radiation Impedance";
                this.Calc_Zr.Click += this.Calc_Zr_Click;

                DynamicLayout ZF2 = new DynamicLayout();
                ZF2.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                ZF2.AddRow(ZF);
                ZF2.AddRow(Calc_Zr);

                GroupBox SampleExtent = new GroupBox();
                SampleExtent.Text = "Sample Extents";
                SampleExtent.Content = ZF2;

                ///////////////////////////////////////////////

                DynamicLayout Gross = new DynamicLayout();
                Gross.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                Gross.Padding = 8;
                this.label14.Text = "Gross Material Properties";

                this.label6.Text = "Material Depth (mm)";

                this.depth.Width = 75;
                this.depth.DecimalPlaces = 3;
                this.depth.MaxValue = 10000;
                this.depth.Value = 25;
                this.depth.ValueChanged += this.param_ValueChanged;

                Gross.AddRow(label14);
                Gross.AddRow(label6, depth);

                ///////////////////////////////////////////////
                DynamicLayout Solids = new DynamicLayout();
                Solids.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                Solids.Padding = 8;
                this.label15.Text = "Solid Components Properties";
                this.label15.Visible = false;

                this.label13.Text = "Density (kg/m^3)";
                this.label13.Visible = false;
                this.Solid_Density.MaxValue = 100000;
                this.Solid_Density.Value = 740;
                this.Solid_Density.DecimalPlaces = 2;
                this.Solid_Density.ValueChanged += param_ValueChanged;
                this.Solid_Density.Visible = false;

                this.label11.Text = "Young\'s Modulus (GPa)";
                this.label11.Visible = false;
                this.YoungsModulus.MaxValue = 50000;
                this.YoungsModulus.DecimalPlaces = 3;
                this.YoungsModulus.Value = 2;
                this.YoungsModulus.ValueChanged += param_ValueChanged;
                this.YoungsModulus.Visible = false;

                this.label12.Text = "Poisson\'s Ratio";
                this.label12.Visible = false;
                this.PoissonsRatio.MaxValue = 0.5;
                this.PoissonsRatio.MinValue = 0;
                this.PoissonsRatio.DecimalPlaces = 2;
                this.PoissonsRatio.Value = 44;
                this.PoissonsRatio.ValueChanged += param_ValueChanged;
                this.PoissonsRatio.Visible = false;

                Solids.AddRow(label15);
                Solids.AddRow(label13, Solid_Density);
                Solids.AddRow(label11, YoungsModulus);
                Solids.AddRow(label12, PoissonsRatio);
                ////////////////////////////////////////////////

                this.label16.Text = "Perforation Properties";
                this.label16.Visible = false;

                DynamicLayout Perf = new DynamicLayout();
                Perf.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                Perf.Padding = 8;
                this.pitch_label.Text = "Spacing-centers (mm)";
                this.pitch_label.Visible = false;
                this.pitch.DecimalPlaces = 3;
                this.pitch.MaxValue = 1000;
                this.pitch.Value = 25;
                this.pitch.Visible = false;
                this.pitch.ValueChanged += this.param_ValueChanged;

                this.diam_label.Text = "Hole Diameter (mm)";
                this.diam_label.Visible = false;
                this.diameter.DecimalPlaces = 3;
                this.diameter.MaxValue = 1000;
                this.diameter.Value = 25;
                this.diameter.Visible = false;
                this.diameter.ValueChanged += this.param_ValueChanged;

                Perf.AddRow(label16);
                Perf.AddRow(diam_label, diameter);
                Perf.AddRow(pitch_label, pitch);

                /////////////////////////////////////////////////////

                DynamicLayout BasicPorous = new DynamicLayout();
                BasicPorous.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                BasicPorous.Padding = 8;
                this.label17.Text = "Porous Medium Properties";
                this.label17.Visible = false;

                this.Label1.Text = "Airflow Resistivity (Pa *s/m^2)";
                this.Label1.Visible = false;
                this.Sigma.MaxValue = 200000;
                this.Sigma.Value = 25000;
                this.Sigma.Visible = false;
                this.Sigma.ValueChanged += this.Resistivity_ValueChanged;

                this.Resistivity_Feedback.Text = "i.e. Mineral Wool (45 kg/ m^3)";
                this.Resistivity_Feedback.Visible = false;
                this.PorosityLabel.Text = "Porosity (%)";
                this.PorosityLabel.Visible = false;
                this.Porosity_Percent.Value = 100;
                this.Porosity_Percent.Visible = false;

                BasicPorous.AddRow(label17);
                BasicPorous.AddRow(Label1, Sigma);
                BasicPorous.AddRow(Resistivity_Feedback);
                BasicPorous.AddRow(PorosityLabel, Porosity_Percent);

                DynamicLayout AdvancedPorous = new DynamicLayout();
                AdvancedPorous.Padding = 8;
                AdvancedPorous.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                lblBP = new Label();
                lblBP.Visible = false;
                lblBP.Text = "Biot Porous:";
                AdvancedPorous.AddRow(lblBP);
                this.ThermalPermeability.MaxValue = 1;
                this.ThermalPermeability.Value = 61;
                this.ThermalPermeability.Visible = false;
                this.ThermalPermeability.ValueChanged += param_ValueChanged;
                this.label9.Text = "Thermal Permeabiity (m^2)";
                this.label9.Visible = false;

                this.label10.Text = "Viscous Char. Length (μm)";
                this.label10.Visible = false;
                this.ViscousCharacteristicLength.MaxValue = 1000000;
                this.ViscousCharacteristicLength.Value = 100;
                this.ViscousCharacteristicLength.Visible = false;
                this.ViscousCharacteristicLength.ValueChanged += param_ValueChanged;

                this.label19.Text = "Tortuosity";
                this.label19.Visible = false;
                this.Tortuosity.DecimalPlaces = 2;
                this.Tortuosity.MaxValue = 1000;
                this.Tortuosity.Value = 102;
                this.Tortuosity.Visible = false;
                this.Tortuosity.ValueChanged += param_ValueChanged;

                AdvancedPorous.AddRow(label9, ThermalPermeability);
                AdvancedPorous.AddRow(label10, ViscousCharacteristicLength);
                AdvancedPorous.AddRow(label19, Tortuosity);

                DynamicLayout ParamsAll = new DynamicLayout();
                DynamicLayout ParamsLeft = new DynamicLayout();
                DynamicLayout ParamsRight = new DynamicLayout();
                ParamsAll.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                ParamsAll.AddRow(ParamsLeft, ParamsRight);
                ParamsLeft.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                ParamsLeft.AddRow(Gross);
                ParamsLeft.AddRow(Perf);
                ParamsLeft.AddRow(BasicPorous);
                ParamsLeft.AddSpace();
                ParamsRight.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                ParamsRight.AddRow(Solids);
                ParamsRight.AddRow(AdvancedPorous);
                ParamsRight.AddSpace();

                ////////////////////////////////////////////////////////////////
                GroupBox EC_Controls = new GroupBox() { Text = "Embodied Carbon Controls" };
                DynamicLayout EC_LO = new DynamicLayout();
                Label EC_N_Lbl = new Label() { Text = "Material Name:" };
                Label EC_Coef_Lbl = new Label() { Text = "A1-A3 ECC (kgCO2e/m3):" };
                EC_Name = new TextBox() { Width = 50 };
                EC_Coefficient = new NumericStepper() { Width = 50, DecimalPlaces = 2, MinValue = 0, MaxValue = 1000000, Value = 0 };
                EC_LO.AddRow(EC_N_Lbl, EC_Name);
                EC_LO.AddRow(EC_Coef_Lbl, EC_Coefficient);
                EC_LO.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                EC_LO.Padding = 8;
                EC_Controls.Content = EC_LO;
                ////////////////////////////////////////////////////////////////

                DynamicLayout CtrlX = new DynamicLayout();
                CtrlX.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                //this.Averaging.Items.Add("Paris [sin(the)cos(the)]");
                //this.Averaging.Items.Add("0 to 78 only");
                //this.Averaging.Items.Add("0 t0 90 Flat");
                //this.Averaging.SelectedIndexChanged += this.Averaging_SelectedIndexChanged;
                this.Smart_Mat.Text = "Create Smart Material";
                this.Smart_Mat.Click += this.Smart_Mat_Click;
                this.Use_RI.Text = "Random Incidence";
                this.Use_RI.Click += this.Use_RI_Click;
                Cancel.Text = "Cancel";
                Cancel.Click += this.Cancel_Click;
                CtrlX.AddRow( Smart_Mat, Use_RI, Cancel);

                CtrlRight.AddRow(Ctrl12);
                CtrlRight.AddRow(Ctrl3);
                CtrlRight.AddRow(Ctrl4);
                CtrlRight.AddRow(SampleExtent);
                CtrlRight.AddRow(ParamsAll);
                CtrlRight.AddSpace();
                CtrlRight.AddRow(EC_Controls);
                CtrlRight.AddRow(CtrlX);

                Sustainability_Reference.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                Sustainability_Reference.Padding = 8;

                GroupBox Data = new GroupBox() { Text = "Library - EHM Databases" };
                Material_List = new ListBox() { Height = 200 };
                Material_List.SelectedIndexChanged += this.Material_List_SelectedIndexChanged;
                this.Show_ECC_ABS = new CheckBox() { Text = "Show Absorption Coefficients" };
                this.Show_ECC_ABS.CheckedChanged += Show_ECC_ABS_CheckedChanged;
                Scrollable matlist = new Scrollable() { Content = Material_List, Height = 200, Width = 250 };
                matlist.Width = 250;
                Data.Content = matlist;
                Data.Width = 250;
                GroupBox EHM_Data = new GroupBox() { Text = "Documented Performance" };
                Sustainability_Reference.AddRow(Data);
                Sustainability_Reference.AddRow(EHM_Data);
                Estimates = new GroupBox() { Text = "Estimates" };
                Sustainability_Reference.AddRow(Estimates);
                DynamicLayout EHM_DataLayout = new DynamicLayout();
                
                //Abs_125Hz = new Label() { Text = "   125 Hz:" };
                //Abs_160Hz = new Label() { Text = "   160 Hz:" };
                //Abs_200Hz = new Label() { Text = "   200 Hz:" };
                //Abs_250Hz = new Label() { Text = "   250 Hz:" };
                //Abs_315Hz = new Label() { Text = "   315 Hz:" };
                //Abs_400Hz = new Label() { Text = "   400 Hz:" };
                //Abs_500Hz = new Label() { Text = "   500 Hz:" };
                //Abs_630Hz = new Label() { Text = "   630 Hz:" };
                //Abs_800Hz = new Label() { Text = "   800 Hz:" };
                //Abs_1000Hz = new Label() { Text = "  1000 Hz:" };
                //Abs_1250Hz = new Label() { Text = "  1250 Hz:" };
                //Abs_1600Hz = new Label() { Text = "  1600 Hz:" };
                //Abs_2000Hz = new Label() { Text = "  2000 Hz:" };
                Set_EHM = new Button() { Text = "Set Direct EHM" };
                Set_EHM.Click += Set_EHM_Click;
                Get_Est = new Button { Text = "Fit Estimate" };
                Get_Est.Click += Get_Est_Click;
                Set_Est = new Button() { Text = "Set Estimate" };
                Set_Est.Click += Set_Est_Click;

                EHM_ECC = new Label() { Text = "A1-A3 ECC (kgCO2e/m2):" };
                EHM_ECC_Thickness = new Label() { Text = "   Ref depth:" };
                EHM_ECC_ABS = new Label() { Text = "A1-A3 ECC (kgCO2e/m3):" };
                EHM_Flow_Resist = new Label() { Text = "Flow Resistivity (σ) (pa*s/m^2):" };

                EHM_DataLayout.AddRow(" ");
                EHM_DataLayout.AddRow(EHM_ECC);
                EHM_DataLayout.AddRow(EHM_ECC_Thickness);
                EHM_DataLayout.AddRow(EHM_ECC_ABS);
                EHM_DataLayout.AddRow(" ");
                EHM_DataLayout.AddRow(EHM_Flow_Resist);
                EHM_DataLayout.AddRow(Show_ECC_ABS);
                EHM_DataLayout.AddRow(Set_EHM);
                EHM_DataLayout.AddRow(" ");

                //EHM_DataLayout.AddRow(new Label() { Text = "Absorption Coefficient (α) (0.0-1.0):" });
                //EHM_DataLayout.AddRow(Abs_125Hz);
                //EHM_DataLayout.AddRow(Abs_160Hz);
                //EHM_DataLayout.AddRow(Abs_200Hz);
                //EHM_DataLayout.AddRow(Abs_250Hz);
                //EHM_DataLayout.AddRow(Abs_315Hz);
                //EHM_DataLayout.AddRow(Abs_400Hz);
                //EHM_DataLayout.AddRow(Abs_500Hz);
                //EHM_DataLayout.AddRow(Abs_630Hz);
                //EHM_DataLayout.AddRow(Abs_800Hz);
                //EHM_DataLayout.AddRow(Abs_1000Hz);
                //EHM_DataLayout.AddRow(Abs_1250Hz);
                //EHM_DataLayout.AddRow(Abs_1600Hz);
                //EHM_DataLayout.AddRow(Abs_2000Hz);

                DynamicLayout EstimatesLayout = new DynamicLayout();
                Est_FlowResistivity = new Label() { Text = "Flow Resistivity (ρ) (pa*s/m^2):" };
                Est_Porosity = new Label() { Text = "Porosity (φ) (%):" };
                Est_Density = new Label() { Text = "Density (d) (kg/m^3):" };
                EstimatesLayout.AddRow(Get_Est);
                EstimatesLayout.AddRow(Est_FlowResistivity);
                EstimatesLayout.AddRow(Est_Density);
                EstimatesLayout.AddRow(Set_Est);
                Data.Content = Material_List;
                EHM_Data.Content = EHM_DataLayout;
                Estimates.Content = EstimatesLayout;

                EmbodiedCarbon_Pie = new ScottPlot.Eto.EtoPlot();
                EmbodiedCarbon_Pie.Plot.Axes.Frameless();
                EmbodiedCarbon_Pie.BackgroundColor = Colors.Gray;
                EmbodiedCarbon_Pie.Plot.Add.Pie(new double[1] { 0 });
                EmbodiedCarbon_Pie.Plot.Title("Embodied Carbon Coefficients",10);
                EmbodiedCarbon_Pie.Plot.XLabel("A1-A3 ECC (kgCO2e/m2)",10);
                EmbodiedCarbon_Pie.Width = 250;
                EmbodiedCarbon_Pie.BackgroundColor = Colors.SlateGray;
                EmbodiedCarbon_Pie.Plot.HideGrid();
                Label ECLBL = new Label();
                ECLBL.Text = "Embodied Carbon By Material";
                Sustainability_Reference.AddRow(ECLBL);
                Sustainability_Reference.AddRow(EmbodiedCarbon_Pie);

                this.Content = All;
            }

            private void Show_ECC_ABS_CheckedChanged(object sender, EventArgs e)
            {
                Update_Graphs();
            }

            private void Set_EHM_Click(object sender, EventArgs e)
            {
                if (Material_List.SelectedIndex < 0) return;
                if (Material_List.SelectedIndex < EHM.ECC_Abs.Count)
                {
                    Material_Type.SelectedIndex = 4;
                    Material_Type.SelectedKey = "Porous Absorber (Miki)";
                    Material_Type.Invalidate();
                    Sigma.Value = EHM.Flow_Resistivity[Material_List.SelectedIndex];
                    depth.Value = EHM.Thickness[Material_List.SelectedIndex] * 1000;
                    Add_Click(sender, e);
                    if (LayerList.Items.Count == 1)
                    {
                        Show_ECC_ABS.Visible = true;
                        Estimates.Visible = true;
                        Get_Est.Visible = true;
                        Set_Est.Visible = true;
                        Est_Density.Visible = true;
                        Est_FlowResistivity.Visible = true;
                        Est_Porosity.Visible = true;
                    }
                    else
                    {
                        Show_ECC_ABS.Visible = false;
                        Show_ECC_ABS.Checked = false;
                        Estimates.Visible = false;
                        Get_Est.Visible = false;
                        Set_Est.Visible = false;
                        Est_Density.Visible = false;
                        Est_FlowResistivity.Visible = false;
                        Est_Porosity.Visible = false;
                    }
                }
                else 
                {
                    ABS_Layer[] layers = EHM.Substrates[Material_List.SelectedIndex - EHM.ECC_Abs.Count];
                    Layers.Clear();
                    LayerList.Items.Clear();
                    foreach (ABS_Layer l in layers)
                    {
                        Layers.Add(l);
                        LayerList.Items.Add(l.Material_Name);
                    }
                    Rigid_Term.Checked = false;
                    Air_Term.Checked = true;
                    Show_ECC_ABS.Checked = false;
                    Update_Graphs();
                }
            }

            private void Get_Est_Click(object sender, EventArgs e)
            {
                // Check if a material is selected in the list
                if (Material_List.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a material from the list first.");
                    return;
                }

                // Step 1: Get target absorption values directly from EHM data
                int selectedIndex = Material_List.SelectedIndex;
                double[] targetAbsorption = EHM.Abs_Coef[selectedIndex];

                // Step 2: Get the thickness value directly from EHM data
                double thickness = EHM.Thickness[selectedIndex];

                // Step 3: Get suggested flow resistivity and standard deviation from EHM
                double suggestedFlowResistivity = EHM.Flow_Resistivity[selectedIndex];
                double flowResistivityStdDev = EHM.FR_Dev[selectedIndex];

                // Set search bounds using the standard deviation as a guide
                double minResistivity = Math.Max(100, suggestedFlowResistivity - 10 * flowResistivityStdDev);
                double maxResistivity = Math.Min(200000, suggestedFlowResistivity + 10 * flowResistivityStdDev);

                // Implement Bayesian inference for finding best flow resistivity
                (double bestResistivity, double airspace, double confidence) = BayesianFlowResistivityFit(
                    targetAbsorption,
                    thickness,
                    suggestedFlowResistivity,
                    flowResistivityStdDev,
                    minResistivity,
                    maxResistivity);

                // Step 5: Calculate estimated material properties
                double estimatedDensity = EstimateDensity(bestResistivity);
                double estimatedPorosity = EstimatePorosity(bestResistivity);

                Fit_Resistivity = bestResistivity;
                Fit_Airspace = airspace;

                // Step 6: Update the UI with the results
                Est_FlowResistivity.Text = $"Flow Resistivity (ρ) (pa*s/m²): {bestResistivity:F0} (confidence: {confidence:P1})";
                Est_Density.Text = $"Density (d) (kg/m³): {estimatedDensity:F1}";
                Est_Porosity.Text = $"Airspace needed (m): {airspace:F1}";
            }

            double Fit_Resistivity = 0;
            double Fit_Airspace = 0;

            private (double bestResistivity, double airspace, double confidence) BayesianFlowResistivityFit(
            double[] targetAbsorption,
            double thickness,
            double priorMean,
            double priorStdDev,
            double minResistivity,
            double maxResistivity)
            {
                // Number of samples for Bayesian inference (increase for better results)
                int numSamples = 2000;

                // Step 1: Create more principled prior distributions using importance sampling
                double[] samples = new double[numSamples];
                double[] weights = new double[numSamples];
                double[] airspaces = new double[numSamples];
                double[] errors = new double[numSamples];

                Random random = new Random();

                // Generate samples from a truncated normal distribution
                for (int i = 0; i < numSamples; i++)
                {
                    // Use a better sampling method: Box-Muller transform for normal distribution
                    double u1 = 1.0 - random.NextDouble();
                    double u2 = 1.0 - random.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

                    // Sample from normal distribution, but keep within bounds
                    double sample;
                    do
                    {
                        sample = priorMean + priorStdDev * randStdNormal;
                    } while (sample < minResistivity || sample > maxResistivity);

                    samples[i] = sample;
                }

                // Step 2: Evaluate likelihood for each sample using multiple metrics
                object lock_records = new object();
                double minError = double.MaxValue;

                // Store all likelihood information without early aggregation
                Parallel.For(0, numSamples, i =>
                {
                    double airspace = 0;
                    double error = EvaluateFlowResistivityMultiMetric(
                        samples[i],
                        thickness,
                        targetAbsorption,
                        ref airspace
                    );

                    // Store all results for comprehensive posterior analysis
                    lock (lock_records)
                    {
                        errors[i] = error;
                        airspaces[i] = airspace;
                        weights[i] = 0; // Will be calculated after all samples are evaluated

                        if (error < minError)
                            minError = error;
                    }
                });

                // Step 3: Convert errors to likelihoods with adaptive scaling
                double errorRange = errors.Max() - errors.Min();
                double scaleFactor = errorRange > 0.001 ? 50.0 / errorRange : 50.0;

                for (int i = 0; i < numSamples; i++)
                {
                    weights[i] = Math.Exp(-scaleFactor * errors[i]);
                }

                // Normalize weights
                double sumWeights = weights.Sum();
                for (int i = 0; i < numSamples; i++)
                {
                    weights[i] /= sumWeights;
                }

                // Step 4: Calculate posterior statistics using Bayesian model averaging
                double posteriorMean = 0;
                double posteriorVar = 0;
                double maxWeight = 0;
                int maxWeightIndex = 0;
                double weightedAirspace = 0;

                // Find MAP (Maximum A Posteriori) estimate and calculate weighted statistics
                for (int i = 0; i < numSamples; i++)
                {
                    posteriorMean += samples[i] * weights[i];
                    weightedAirspace += airspaces[i] * weights[i];

                    if (weights[i] > maxWeight)
                    {
                        maxWeight = weights[i];
                        maxWeightIndex = i;
                    }
                }

                // Calculate variance and effective sample size (ESS)
                for (int i = 0; i < numSamples; i++)
                {
                    posteriorVar += weights[i] * Math.Pow(samples[i] - posteriorMean, 2);
                }

                // Effective sample size (measure of sampling quality)
                double effectiveSampleSize = 1.0 / weights.Sum(w => w * w);
                double confidenceMetric = Math.Min(effectiveSampleSize / numSamples, maxWeight * numSamples);

                // Return the best estimate based on posterior analysis
                return (samples[maxWeightIndex], airspaces[maxWeightIndex], Math.Min(confidenceMetric, 1.0));
            }

            // Enhanced evaluation function with multiple error metrics
            private double EvaluateFlowResistivityMultiMetric(
                double flowResistivity,
                double thickness,
                double[] targetAbsorption,
                ref double req_airspace)
            {
                // Create a Miki porous absorber layer
                ABS_Layer layer = new ABS_Layer(ABS_Layer.LayerType.PorousM, thickness, 0, 0, flowResistivity, 0.99, 0);

                // Track multiple error metrics
                double maxError = 0;
                double mseError = 0;
                double weightedError = 0;
                double[] frequencies = new double[1];
                Complex[] Angles = new Complex[1] { 89 };
                Complex[][] Trans, R;

                double bestTotalError = double.MaxValue;
                double bestMaxError = double.MaxValue;

                // Perceptually weight frequencies (mid-range frequencies have higher importance)
                double[] frequencyWeights = new double[thirdOctaveBands.Length];
                for (int i = 0; i < thirdOctaveBands.Length; i++)
                {
                    // A-weighting inspired perceptual weights (emphasize 500-4000 Hz range)
                    double f = thirdOctaveBands[i];
                    frequencyWeights[i] = Math.Min(1.0,
                        0.2 + 0.8 * Math.Exp(-0.5 * Math.Pow((Math.Log10(f) - Math.Log10(1000)) / 1.0, 2)));
                }

                foreach (double airspace in airspace_ImpTube_Errors)
                {
                    ABS_Layer[] layers = new ABS_Layer[2] {new ABS_Layer(ABS_Layer.LayerType.AirSpace, airspace, 0, 0, 0, 0, 0), layer};

                    Complex[][] Z = Operations.Transfer_Matrix_Divisible(
                        false,
                        false,
                        44100,
                        343,
                        layers.ToList(),
                        ref frequencies,
                        ref Angles,
                        out Trans,
                        out R
                    );

                    double[] Abs_Coef = Operations.Absorption_Coef(R)[0];
                    double currentMaxError = 0;
                    double currentMseError = 0;
                    double currentWeightedError = 0;

                    // Calculate multiple error metrics across frequency bands
                    for (int bandIndex = 0; bandIndex < thirdOctaveBands.Length; bandIndex++)
                    {
                        double bandFrequency = thirdOctaveBands[bandIndex];
                        double bandAbsorption = 0;
                        int count = 0;

                        // Average absorption coefficients within each band
                        for (int freqIndex = 0; freqIndex < frequencies.Length; freqIndex++)
                        {
                            if (frequencies[freqIndex] >= bandFrequency * 0.8909)
                            {
                                bandAbsorption += Abs_Coef[freqIndex];
                                count++;
                            }
                            if (bandFrequency * 1.12246 < frequencies[freqIndex]) break;
                        }

                        if (count > 0)
                        {
                            bandAbsorption /= count;
                        }

                        // Calculate error for this band
                        double error = Math.Abs(bandAbsorption - targetAbsorption[bandIndex]);

                        // Update error metrics
                        currentMaxError = Math.Max(currentMaxError, error);
                        currentMseError += error * error;
                        currentWeightedError += error * frequencyWeights[bandIndex];
                    }

                    currentMseError = Math.Sqrt(currentMseError / thirdOctaveBands.Length);
                    currentWeightedError /= frequencyWeights.Sum();

                    // Combined error metric (weighted combination of different error metrics)
                    double combinedError =
                        0.5 * currentMaxError +
                        0.3 * currentMseError +
                        0.2 * currentWeightedError;

                    if (combinedError < bestTotalError)
                    {
                        bestTotalError = combinedError;
                        bestMaxError = currentMaxError;
                        req_airspace = airspace;
                    }
                }

                return bestTotalError;
            }

            double[] airspace_ImpTube_Errors = new double[5] {0, 0.002, 0.005, 0.01, 0.02 };
            double[] thirdOctaveBands = { 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000 };

            private double EstimateDensity(double flowResistivity)
            {
                // Empirical relationship between flow resistivity and density
                // This is an approximation based on typical materials
                if (flowResistivity < 10000)
                    return 10 + 0.002 * flowResistivity; // Low density materials
                else if (flowResistivity < 50000)
                    return 30 + 0.001 * (flowResistivity - 10000); // Medium density
                else
                    return 70 + 0.0005 * (flowResistivity - 50000); // High density
            }

            private double EstimatePorosity(double flowResistivity)
            {
                // Empirical relationship between flow resistivity and porosity
                // Higher flow resistivity generally means lower porosity
                if (flowResistivity < 10000)
                    return 99.0;
                else if (flowResistivity < 50000)
                    return 98.0 - 0.00005 * (flowResistivity - 10000);
                else
                    return 96.0 - 0.00002 * (flowResistivity - 50000);
            }

            private void Set_Est_Click(object sender, EventArgs e)
            {
                if (Fit_Airspace > 0)
                {
                    Material_Type.SelectedIndex = 0;
                    depth.Value = Fit_Airspace * 1000;
                    Add_Click(sender, e);
                }
    
                Material_Type.SelectedIndex = 4;
                Sigma.Value = Fit_Resistivity;
                depth.Value = EHM.Thickness[Material_List.SelectedIndex] * 1000;
                Add_Click(sender, e);
            }

            private DynamicLayout Sustainability_Reference;
            ListBox Material_List;
            GroupBox Estimates;
            Label Est_FlowResistivity;
            Label Est_Porosity;
            Label Est_Density;
            ScottPlot.Eto.EtoPlot EmbodiedCarbon_Pie;

            private Label EHM_ECC;
            private Label EHM_ECC_ABS;
            private Label EHM_ECC_Thickness;
            private Label EHM_Flow_Resist;
            private CheckBox Show_ECC_ABS;
            //private Label Abs_125Hz;
            //private Label Abs_160Hz;
            //private Label Abs_200Hz;
            //private Label Abs_250Hz;
            //private Label Abs_315Hz;
            //private Label Abs_400Hz;
            //private Label Abs_500Hz;
            //private Label Abs_630Hz;
            //private Label Abs_800Hz;
            //private Label Abs_1000Hz;
            //private Label Abs_1250Hz;
            //private Label Abs_1600Hz;
            //private Label Abs_2000Hz;
            private Button Set_EHM;
            private Button Get_Est;
            private Button Set_Est;

            private ScottPlot.Eto.EtoPlot Alpha_Normal;
            private ScottPlot.Eto.EtoPlot Polar_Absorption;
            private ScottPlot.Eto.EtoPlot Impedance_Graph;
            private Eto.Forms.ListBox LayerList;
            private Eto.Forms.Button Use_RI;
            private Eto.Forms.Label Resistivity_Feedback;
            private Eto.Forms.Label Label1;
            private Eto.Forms.NumericStepper Sigma;
            private Eto.Forms.NumericStepper depth;
            private Eto.Forms.NumericStepper diameter;
            private Eto.Forms.NumericStepper pitch;
            private Eto.Forms.Label diam_label;
            private Eto.Forms.Label pitch_label;
            private Eto.Forms.Label PorosityLabel;
            private Eto.Forms.NumericStepper Porosity_Percent;
            //private Eto.Forms.Label PolarTitle;
            private Eto.Forms.Button Smart_Mat;
            private Eto.Forms.Label label6;
            private Eto.Forms.Label label9;
            private Eto.Forms.Label label10;
            private Eto.Forms.Label label11;
            private Eto.Forms.Label label12;
            private Eto.Forms.Label label13;
            private Eto.Forms.Label lblBP;
            private Eto.Forms.NumericStepper YoungsModulus;
            private Eto.Forms.NumericStepper Solid_Density;
            private Eto.Forms.NumericStepper PoissonsRatio;
            private Eto.Forms.Label label17;
            private Eto.Forms.NumericStepper ViscousCharacteristicLength;
            private Eto.Forms.NumericStepper ThermalPermeability;
            private Eto.Forms.Label label16;
            private Eto.Forms.Label label19;
            private Eto.Forms.NumericStepper Tortuosity;
            private Eto.Forms.Label label21;
            private Eto.Forms.Label label22;
            private Eto.Forms.RadioButton Air_Term;
            private Eto.Forms.RadioButton Rigid_Term;
            private Eto.Forms.Label label4;
            private Eto.Forms.Label label5;
            private Eto.Forms.Label label14;
            private Eto.Forms.ComboBox Zf_Incorp_Method;
            private Eto.Forms.Label Z_rad_feedback;
            private Eto.Forms.Button Calc_Zr;
            private Eto.Forms.RadioButton Inf_Sample;
            private Eto.Forms.NumericStepper XDim;
            private Eto.Forms.NumericStepper YDim;
            private Eto.Forms.RadioButton Fin_Sample;
            private Eto.Forms.ComboBox Material_Type;
            private Eto.Forms.Button Up;
            private Eto.Forms.Button Dn;
            private Eto.Forms.Button Rem;
            private Eto.Forms.Button Add;
            private Eto.Forms.Label label15;
            private Eto.Forms.ComboBox Chart_Contents;
            private Eto.Forms.NumericStepper IIR_Order;
            private Eto.Forms.ComboBox Direction_choice;
            private Eto.Forms.TextBox EC_Name;
            private Eto.Forms.NumericStepper EC_Coefficient;
        }
    }
}