//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2023, Arthur van der Harten 
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
using System.Diagnostics;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        partial class Pach_Absorption_Designer
        {
            private void InitializeComponent()
            {
                this.Title = "Materials Designer [EXPERIMENTAL]";
                this.Size = new Eto.Drawing.Size(1400, 800);

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
                this.Averaging = new Eto.Forms.ComboBox();
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
            this.label18 = new Eto.Forms.Label();
            this.SoundSpeed = new Eto.Forms.NumericStepper();
            this.PolarTitle = new Eto.Forms.Label();
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
                DynamicLayout GraphLeft = new DynamicLayout();
                DynamicLayout CtrlRight = new DynamicLayout();
                All.AddRow(GraphLeft, CtrlRight);

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
                Inf_Sample.MouseUp += this.Inf_Sample_CheckedChanged;
                Fin_Sample.MouseUp += this.Fin_Sample_CheckedChanged;
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
                this.Solid_Density.Value = 9740;
                this.Solid_Density.Visible = false;

                this.label11.Text = "Young\'s Modulus (GPa)";
                this.label11.Visible = false;
                this.YoungsModulus.MaxValue = 10000;
                this.YoungsModulus.Value = 20237;
                this.YoungsModulus.Visible = false;

                this.label12.Text = "Poisson\'s Ratio";
                this.label12.Visible = false;
                this.PoissonsRatio.MaxValue = 1;
                this.PoissonsRatio.Value = 44;
                this.PoissonsRatio.Visible = false;

                this.label18.Text = "Speed of Sound (m/s)";
                this.label18.Visible = false;
                this.SoundSpeed.DecimalPlaces = 2;
                this.SoundSpeed.MaxValue = 100000;
                this.SoundSpeed.Value = 343;
                this.SoundSpeed.Visible = false;
                Solids.AddRow(label15);
                Solids.AddRow(label13, Solid_Density);
                Solids.AddRow(label11, YoungsModulus);
                Solids.AddRow(label12, PoissonsRatio);
                Solids.AddRow(label18, SoundSpeed);
                ////////////////////////////////////////////////

                this.label16.Text = "Perforation Properties";
                this.label16.Visible = false;

                DynamicLayout Perf = new DynamicLayout();
                Perf.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                Perf.Padding = 8;
                this.pitch_label.Text = "Hole Pitch (mm)";
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

                this.Label1.Text = "Airflow Resistivity (Pa *s/m^3)";
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
                BasicPorous.AddRow(null, Resistivity_Feedback);
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
                this.label9.Text = "Thermal Permeabiity (m^2)";
                this.label9.Visible = false;

                this.label10.Text = "Viscous Characteristic Length (μm)";
                this.label10.Visible = false;
                this.ViscousCharacteristicLength.MaxValue = 1000000;
                this.ViscousCharacteristicLength.Value = 100;
                this.ViscousCharacteristicLength.Visible = false;

                this.label19.Text = "Tortuosity";
                this.label19.Visible = false;
                this.Tortuosity.DecimalPlaces = 2;
                this.Tortuosity.MaxValue = 1000;
                this.Tortuosity.Value = 102;
                this.Tortuosity.Visible = false;

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

                DynamicLayout CtrlX = new DynamicLayout();
                CtrlX.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                this.Averaging.Items.Add("Paris [sin(the)cos(the)]");
                this.Averaging.Items.Add("0 to 78 only");
                this.Averaging.Items.Add("0 t0 90 Flat");
                this.Averaging.SelectedIndexChanged += this.Averaging_SelectedIndexChanged;
                this.Smart_Mat.Text = "Create Smart Material";
                this.Smart_Mat.Click += this.Smart_Mat_Click;
                this.Use_RI.Text = "Random Incidence";
                this.Use_RI.Click += this.Use_RI_Click;
                Cancel.Text = "Cancel";
                Cancel.Click += this.Cancel_Click;
                CtrlX.AddSpace();
                CtrlX.AddRow(Averaging, Smart_Mat, Use_RI, Cancel);

                CtrlRight.AddRow(Ctrl12);
                CtrlRight.AddRow(Ctrl3);
                CtrlRight.AddRow(Ctrl4);
                CtrlRight.AddRow(SampleExtent);
                CtrlRight.AddRow(ParamsAll);
                CtrlRight.AddRow(CtrlX);

                this.Content = All;
            }

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
            private Eto.Forms.Label PolarTitle;
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
            private Eto.Forms.Label label18;
            private Eto.Forms.NumericStepper SoundSpeed;
            private Eto.Forms.ComboBox Averaging;
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
        }
    }
}