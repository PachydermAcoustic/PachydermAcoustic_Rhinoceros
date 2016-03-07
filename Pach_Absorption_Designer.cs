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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;
using Pachyderm_Acoustic.AbsorptionModels;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public partial class Pach_Absorption_Designer : Form
        {
            public Environment.Smart_Material sm;
            public static double c_sound = 343;
            public static double density = 1.2;
            double[][] angle_alpha = new double[8][];
            bool indexchanged = false;
            Environment.Smart_Material.Finite_Field_Impedance Zr;
            public AbsorptionModelResult Result = AbsorptionModelResult.Cancel;
            public double[] RI_Absorption;
            public bool Is_Finite = false;

            public enum AbsorptionModelResult
            {
                Random_Incidence,
                Smart_Material,
                Cancel
            }

            public Pach_Absorption_Designer()
            {
                InitializeComponent();
                this.Chart_Contents.SelectedIndex = 0;
                this.Averaging.SelectedIndex = 0;
                this.Zf_Incorp_Method.SelectedIndex = 0;
            }

            private void Resistivity_ValueChanged(object sender, EventArgs e)
            {
                if (Sigma.Value < 7000)
                {
                    Resistivity_Feedback.Text = "i.e. Polyfill";
                    Porosity_Percent.Value = (decimal)99.7;
                }
                else if (Sigma.Value < 15000)
                {
                    Resistivity_Feedback.Text = "i.e. Melamine Foam";
                    //11 [kg/cu m] / 1570 [kg/cu m]
                    Porosity_Percent.Value = (decimal)99.3;
                }
                else if (Sigma.Value < 30000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (45 kg/m^3)";
                    Porosity_Percent.Value = (decimal)98.4;
                }
                else if (Sigma.Value < 65000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (90 kg/m^3)";
                    Porosity_Percent.Value = (decimal)96.8;
                }
                else if (Sigma.Value < 100000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (145 kg/m^3)";
                    Porosity_Percent.Value = (decimal)95.2;
                }
                else
                {
                    Resistivity_Feedback.Text = "???";
                    Porosity_Percent.Value = (decimal)95.2;
                }

                if (LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (LayerList.Items[LayerList.SelectedIndex] as ABS_Layer);
                L.Flow_Resist = (double)Sigma.Value;
                L.porosity = (double)Porosity_Percent.Value / 100;

                LayerList.Items[LayerList.SelectedIndex] = L;

                Update_Graphs();
            }

            public void EnableFluidPorous(bool E)
            {
                this.label17.Visible = E;
                this.Label1.Visible = E;
                this.Resistivity_Feedback.Visible = E;
                this.Porosity_Percent.Visible = E;
                this.Sigma.Visible = E;
                this.PorosityLabel.Visible = E;
            }

            public void EnableBiotPorous(bool E)
            {
                this.label17.Visible = E;
                this.Label1.Visible = E;
                this.Resistivity_Feedback.Visible = E;
                this.Porosity_Percent.Visible = E;
                this.Sigma.Visible = E;
                this.label9.Visible = E;
                this.label10.Visible = E;
                this.ThermalPermeability.Visible = E;
                this.ViscousCharacteristicLength.Visible = E;
                this.label19.Visible = E;
                this.Tortuosity.Visible = E;
                this.PorosityLabel.Visible = E;
                EnableSolid(E);
            }

            public void EnablePerforated(bool E)
            {
                this.label16.Visible = E;
                this.diam_label.Visible = E;
                this.pitch_label.Visible = E;
                this.diameter.Visible = E;
                this.pitch.Visible = E;
            }

            public void EnableSolid(bool E)
            {
                this.label15.Visible = E;
                this.label13.Visible = E;
                this.label11.Visible = E;
                this.label12.Visible = E;
                this.Solid_Density.Visible = E;
                this.YoungsModulus.Visible = E;
                this.PoissonsRatio.Visible = E;
                this.label18.Visible = E;
                this.SoundSpeed.Visible = E;
            }

            public void ClearAll()
            {
                EnableFluidPorous(false);
                EnablePerforated(false);
                EnableSolid(false);
                EnableBiotPorous(false);
            }

            private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
            {
                ClearAll();
                switch ((sender as ComboBox).SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        //Biot Parameters
                        EnableBiotPorous(true);
                        break;
                    case 2:
                    case 3:
                    case 4:
                        EnableFluidPorous(true);
                        break;
                    case 5:
                        EnableSolid(true);
                        break;
                    default:
                        EnablePerforated(true);
                        break;
                }
            }
            
            public List<ABS_Layer> Material_Layers()
            {
                List<ABS_Layer> Layers = new List<ABS_Layer>();
                foreach (ABS_Layer AL in LayerList.Items) Layers.Add(AL);
                return Layers;                
            }

            private void Update_Graphs()
            {
                if (Chart_Contents.SelectedIndex == 0)
                {
                    Alpha_Normal.ChartAreas[0].AxisY.Title = "Alpha (0 to 1)";

                    if (Fin_Sample.Checked && Zr == null) return;

                    List<ABS_Layer> Layers = Material_Layers();

                    if (Layers.Count == 0) return;

                    if (Inf_Sample.Checked) sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343, Averaging.SelectedIndex);
                    else sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343, Zr, 0.1, Averaging.SelectedIndex, Zf_Incorp_Method.SelectedIndex);

                    //Polar Graph
                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;

                    RI_Absorption = new double[8];
                    
                    Polar_Absorption.ChartAreas[0].AxisY.Minimum = 0;
                    Polar_Absorption.ChartAreas[0].AxisY.Maximum = 1;

                    for (int oct = 0; oct < 8; oct++)
                    {
                        Polar_Absorption.Series[oct].Points.DataBindXY(AnglesDeg, sm.Ang_Coef_Oct[oct]);
                        for (int a = 0; a < sm.Ang_Coef_Oct[oct].Length; a++)
                        {
                            if (Polar_Absorption.ChartAreas[0].AxisY.Maximum < sm.Ang_Coef_Oct[oct][a]) Polar_Absorption.ChartAreas[0].AxisY.Maximum = Math.Ceiling(sm.Ang_Coef_Oct[oct][a] * 10) / 10;
                        }
                    }

                    //Z Graph
                    double[] real = new double[sm.Z[18].Length];
                    double[] imag = new double[sm.Z[18].Length];

                    for (int i = 0; i < sm.Z[18].Length; i++)
                    {
                        real[i] = sm.Z[18][i].Real;
                        imag[i] = sm.Z[18][i].Imaginary;
                    }

                    Impedance_Graph.Series[0].Points.DataBindXY(sm.frequency, real);
                    Impedance_Graph.Series[1].Points.DataBindXY(sm.frequency, imag);

                    Impedance_Graph.ChartAreas[0].AxisY.Maximum = 4000;
                    Impedance_Graph.ChartAreas[0].AxisY.Minimum = -6000;

                    //Alpha Normal graph
                    Alpha_Normal.Series[0].Points.Clear();
                    Alpha_Normal.Series[1].Points.Clear();
                    RI_Absorption = sm.RI_Coef;

                    Alpha_Normal.ChartAreas[0].AxisY.Maximum = 1;
                    if (Fin_Sample.Checked) { foreach (double a in RI_Absorption) if (Alpha_Normal.ChartAreas[0].AxisY.Maximum < a) Alpha_Normal.ChartAreas[0].AxisY.Maximum = Math.Ceiling(a * 10) / 10; }
                    foreach (double a in sm.NI_Coef) if (Alpha_Normal.ChartAreas[0].AxisY.Maximum < a) Alpha_Normal.ChartAreas[0].AxisY.Maximum = Math.Ceiling(a * 10) / 10;

                    for(int i = 0; i < sm.frequency.Length; i++) Alpha_Normal.Series[0].Points.AddXY(sm.frequency[i], sm.NI_Coef[i]);
                    
                    for (int oct = 0; oct < 8; oct++) Alpha_Normal.Series[1].Points.AddXY(62.5 * Math.Pow(2, oct), RI_Absorption[oct]);
                }
                else
                {
                    if (Rigid_Term.Checked) return;

                    Alpha_Normal.ChartAreas[0].AxisY.Title = "Transmission Loss (dB)";

                    if (Fin_Sample.Checked && Zr == null) return;

                    List<ABS_Layer> Layers = Material_Layers();

                    if (Layers.Count == 0) return;

                    if (Inf_Sample.Checked) sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343, Averaging.SelectedIndex);
                    else sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343, Zr, 0.1, Averaging.SelectedIndex, Zf_Incorp_Method.SelectedIndex);

                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;


                    //Z graph...
                    RI_Absorption = new double[8];
                    
                    double[] real = new double[sm.Z[18].Length];
                    double[] imag = new double[sm.Z[18].Length];

                    for (int i = 0; i < sm.Z[18].Length; i++)
                    {
                        real[i] = sm.Z[18][i].Real;
                        imag[i] = sm.Z[18][i].Imaginary;
                    }

                    Impedance_Graph.Series[0].Points.DataBindXY(sm.frequency, real);
                    Impedance_Graph.Series[1].Points.DataBindXY(sm.frequency, imag);

                    Impedance_Graph.ChartAreas[0].AxisY.Maximum = 40000;
                    Impedance_Graph.ChartAreas[0].AxisY.Minimum = -60000;

                    //T graph...
                    Alpha_Normal.Series[0].Points.Clear();
                    Alpha_Normal.Series[1].Points.Clear();
                    
                    double[] TL = new double[sm.frequency.Length];

                    double max = double.NegativeInfinity;

                    for (int i = 0; i < sm.frequency.Length; i++)
                    {
                        TL[i] = -10 * Complex.Log10(sm.Trans_Coefficient[19][i]).Real;                        //TL[i] = 10 * Math.Log10((sm.Trans_Loss[19][i].Real * sm.Trans_Loss[19][i].Real));
                        max = Math.Max(TL[i], max);
                    }

                    Alpha_Normal.ChartAreas[0].AxisY.Maximum = max;

                    for (int i = 0; i < sm.frequency.Length; i++) Alpha_Normal.Series[0].Points.AddXY(sm.frequency[i], TL[i]);
                }

                //Estimate_IIR();
            }

            public void Estimate_IIR()
            {
                //Complex[] IR = Audio.Pach_SP.IFFT_General(Audio.Pach_SP.Mirror_Spectrum(sm.Reflection_Coefficient[18]), 0);
                //for (int i = 0; i < IR.Length; i++) IR[i] = IR[i].Real;
                //Complex[] R = Audio.Pach_SP.IIR_Design.AutoCorrelation_Coef(IR, (int)IIR_Order.Value);

                //Complex[] a, b;
                //Audio.Pach_SP.IIR_Design.Yule_Walker(R, out a, out b);

                double[] a, b;
                Complex[] RefSpectrum = sm.Reflection_Coefficient[18];
                Audio.Pach_SP.IIR_Design.OptimizeIIR(RefSpectrum, 16000, (int)IIR_Order.Value, out a, out b);

                Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b),new List<double>(a), sm.frequency);
                //Audio.Pach_SP.IIR_Design.SpectrumFromIIR(a, b, 16000, 4096);

                double corr = Audio.Pach_SP.IIR_Design.CrossCorrelation_Coef(RefSpectrum, IIR_spec).Real;

                Rhino.RhinoApp.WriteLine(string.Format("Correlation: {0}", corr));

                //Normalize
                //double ms = 0;
                //foreach (Complex s in IIR_spec) ms = ((ms > s.Magnitude) ? ms : s.Magnitude);
                //double mb = 0;
                //foreach (Complex s in RefSpectrum) mb = ((mb > s.Magnitude) ? mb : s.Magnitude);

                //for (int i = 0; i < IIR_spec.Length; i++) IIR_spec[i] *= mb / ms;

                double[] alpha = AbsorptionModels.Operations.Absorption_Coef(IIR_spec);

                Alpha_Normal.Series[2].Points.Clear();
                for (int i = 0; i < alpha.Length; i++) Alpha_Normal.Series[2].Points.AddXY((double)(i + 1)* 16000 / alpha.Length, alpha[i]);
            }

            public Chart Polar_Plot()
            {
                return Polar_Absorption;
            }

            private void param_ValueChanged(object sender, EventArgs e)
            {
                if (indexchanged == true) return;
                if (LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (LayerList.Items[LayerList.SelectedIndex] as ABS_Layer);

                switch (Material_Type.SelectedIndex)
                {
                    case 0:
                        if (L.T != ABS_Layer.LayerType.AirSpace) return;
                        break;
                    case 1:
                        if (L.T != ABS_Layer.LayerType.BiotPorousAbsorber_Rigid) return;
                        break;
                    case 2:
                        if (L.T != ABS_Layer.LayerType.PorousDB) return;
                        break;
                    case 3:
                        if (L.T != ABS_Layer.LayerType.PorousCA) return;
                        break;
                    case 4:
                        if (L.T != ABS_Layer.LayerType.PorousM) return;
                        break;
                    case 5:
                        if (L.T != ABS_Layer.LayerType.Perforated_Modal) return;
                        break;
                    case 6:
                        if (L.T != ABS_Layer.LayerType.Slotted_Modal) return;
                        break;
                    case 7:
                        if (L.T != ABS_Layer.LayerType.SquarePerforations) return;
                        break;
                    case 8:
                        if (L.T != ABS_Layer.LayerType.CircularPerforations) return;
                        break;
                    case 9:
                        if (L.T != ABS_Layer.LayerType.Slots) return;
                        break;
                    case 10:
                        if (L.T != ABS_Layer.LayerType.Microslit) return;
                        break;
                    case 11:
                        if (L.T != ABS_Layer.LayerType.MicroPerforated) return;
                        break;
                }

                L.Flow_Resist = (double)Sigma.Value;
                L.depth = (double)depth.Value / 1000;
                L.pitch = (double)pitch.Value / 1000;
                L.width = (double)diameter.Value / 1000;
                L.porosity = (double)Porosity_Percent.Value / 100;
                L.PoissonsRatio = (double)PoissonsRatio.Value;
                L.Thermal_Permeability = (double)ThermalPermeability.Value;
                L.YoungsModulus = (double)YoungsModulus.Value;
                L.Viscous_Characteristic_Length = (double)ViscousCharacteristicLength.Value;
                L.density = (double)Solid_Density.Value;
                L.tortuosity = (double)Tortuosity.Value;
                L.SpeedOfSound = (double)SoundSpeed.Value;

                LayerList.Items[LayerList.SelectedIndex] = L;

                Update_Graphs();
            }

            private void Add_Click(object sender, EventArgs e)
            {
                if (Material_Type.SelectedIndex < 0) return;
                switch (Material_Type.SelectedIndex)
                {
                    case 0:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.AirSpace, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 1:
                        LayerList.Items.Add( ABS_Layer.CreateBiot(true, (double) depth.Value / 1000, (double)Solid_Density.Value, (double)YoungsModulus.Value * 1E9, (double)PoissonsRatio.Value, (double)SoundSpeed.Value, (double)Tortuosity.Value, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, (double)ViscousCharacteristicLength.Value, (double)ThermalPermeability.Value ));
                        break;
                    case 2:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.PorousDB, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 3:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.PorousCA, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 4:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.PorousM, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 5:
                        LayerList.Items.Add(ABS_Layer.CreateSolid((double)depth.Value / 1000, (double)Solid_Density.Value, (double)YoungsModulus.Value * 1E9,  (double)PoissonsRatio.Value));
                        break;
                    case 6:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.Perforated_Modal, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 7:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.Slotted_Modal, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 8:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.CircularPerforations, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break; 
                    case 9:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.SquarePerforations, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 10:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.Slots, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 11:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.Microslit, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                    case 12:
                        LayerList.Items.Add(new ABS_Layer(ABS_Layer.LayerType.MicroPerforated, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100));
                        break;
                }

                Update_Graphs();
            }

            private void Rem_Click(object sender, EventArgs e)
            {
                if (LayerList.SelectedIndex < 0) return;
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);
                Update_Graphs();
            }

            private void Up_Click(object sender, EventArgs e)
            {
                if (LayerList.SelectedIndex < 1) return;
                LayerList.Items.Insert(LayerList.SelectedIndex - 1, LayerList.Items[LayerList.SelectedIndex]);
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);
                Update_Graphs();
            }

            private void Down_Click(object sender, EventArgs e)
            {
                if (LayerList.SelectedIndex < 0 || LayerList.SelectedIndex == LayerList.Items.Count - 1) return;
                LayerList.Items.Insert(LayerList.SelectedIndex + 2, LayerList.Items[LayerList.SelectedIndex]);
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);
                Update_Graphs();
            }

            private void LayerList_SelectedIndexChanged(object sender, EventArgs e)
            {
                //Tell the interface that we have selected a material from the list, so that it doesn't update the graph, as the material settings populate.
                indexchanged = true;
                if (LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (LayerList.Items[LayerList.SelectedIndex] as ABS_Layer);

                switch (L.T)
                {
                    case ABS_Layer.LayerType.AirSpace:
                        Material_Type.SelectedIndex = 0;
                        break;
                    case ABS_Layer.LayerType.BiotPorousAbsorber_Limp:
                        Material_Type.SelectedIndex = 1;
                        break;
                    case ABS_Layer.LayerType.PorousDB:
                        Material_Type.SelectedIndex = 2;
                        break;
                    case ABS_Layer.LayerType.PorousCA:
                        Material_Type.SelectedIndex = 3;
                        break;
                    case ABS_Layer.LayerType.PorousM:
                        Material_Type.SelectedIndex = 4;
                        break;
                    case ABS_Layer.LayerType.SolidPlate:
                        Material_Type.SelectedIndex = 5;
                        break;
                    case ABS_Layer.LayerType.CircularPerforations:
                        Material_Type.SelectedIndex = 8;
                        break;
                    case ABS_Layer.LayerType.SquarePerforations:
                        Material_Type.SelectedIndex = 9;
                        break;
                    case ABS_Layer.LayerType.Perforated_Modal:
                        Material_Type.SelectedIndex = 6;
                        break;
                    case ABS_Layer.LayerType.Slotted_Modal:
                        Material_Type.SelectedIndex = 7;
                        break;
                    case ABS_Layer.LayerType.MicroPerforated:
                        Material_Type.SelectedIndex = 12;
                        break;
                    case ABS_Layer.LayerType.Microslit:
                        Material_Type.SelectedIndex = 11;
                        break;
                    case ABS_Layer.LayerType.Slots:
                        Material_Type.SelectedIndex = 10;
                        break;
                    default:
                        return;
                }
                depth.Value = (decimal)L.depth * 1000;
                pitch.Value = (decimal)L.pitch * 1000;
                diameter.Value = (decimal)L.width * 1000;
                Porosity_Percent.Value = (decimal)L.porosity * 100;
                Sigma.Value = (decimal)L.Flow_Resist;

                indexchanged = false;
                //Done Loading material. Updating may continue.
            }

            private void Use_RI_Click(object sender, EventArgs e)
            {
                Result = AbsorptionModelResult.Random_Incidence;
                this.Close();
            }

            private void Smart_Mat_Click(object sender, EventArgs e)
            {
                Result = AbsorptionModelResult.Smart_Material;
                /// Build Smart Material here...
                this.Close();
            }

            private void Cancel_Click(object sender, EventArgs e)
            {
                Result = AbsorptionModelResult.Cancel;
                this.Close();
            }

            private void Calc_Zr_Click(object sender, EventArgs e)
            {
                Zr = new Environment.Smart_Material.Finite_Field_Impedance((double)XDim.Value, (double)YDim.Value, 10000, 343, 1.2);
            }

            private void Averaging_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graphs();
            }

            private void Zf_Incorp_Method_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graphs();
            }

            private void Inf_Sample_CheckedChanged(object sender, EventArgs e)
            {
                Update_Graphs();
                Is_Finite = !Inf_Sample.Checked;
            }

            private void Fin_Sample_CheckedChanged(object sender, EventArgs e)
            {
                Update_Graphs();
                Is_Finite = Fin_Sample.Checked;
            }

            private void Chart_Contents_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graphs();
            }

            private void Rigid_Term_CheckedChanged(object sender, EventArgs e)
            {
                if (Rigid_Term.Checked) { label21.Text = "-- Rigid Substrate (Infinite Impedance) --"; }
                else { label21.Text = "-- Receiving Sppce (Semi-Infinite Air Medium) -- "; }
            }
        }
    }
}