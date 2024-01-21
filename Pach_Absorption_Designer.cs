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

using System;
using System.Collections.Generic;
using System.Numerics;
using Pachyderm_Acoustic.AbsorptionModels;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Rhino.UI;
using System.IO;
using Eto.Forms;
using ScottPlot.DataSources;
using Rhino.UI.Controls;
using System.Security.Cryptography;
using System.Linq;
using ScottPlot;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public partial class Pach_Absorption_Designer : Eto.Forms.Dialog
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
                int id = LayerList.SelectedIndex;
                if (Sigma.Value < 7000)
                {
                    Resistivity_Feedback.Text = "i.e. Polyfill";
                    Porosity_Percent.Value = 99.7;
                }
                else if (Sigma.Value < 15000)
                {
                    Resistivity_Feedback.Text = "i.e. Melamine Foam";
                    //11 [kg/cu m] / 1570 [kg/cu m]
                    Porosity_Percent.Value =  99.3;
                }
                else if (Sigma.Value < 30000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (45 kg/m^3)";
                    Porosity_Percent.Value =  98.4;
                }
                else if (Sigma.Value < 65000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (90 kg/m^3)";
                    Porosity_Percent.Value =  96.8;
                }
                else if (Sigma.Value < 100000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (145 kg/m^3)";
                    Porosity_Percent.Value =  95.2;
                }
                else
                {
                    Resistivity_Feedback.Text = "???";
                    Porosity_Percent.Value =  95.2;
                }

                if (LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (Layers[LayerList.SelectedIndex] as ABS_Layer);
                L.Flow_Resist = (double)Sigma.Value;
                L.porosity = (double)Porosity_Percent.Value / 100;

                Layers[LayerList.SelectedIndex] = L;
                LayerList.Items.Insert(LayerList.SelectedIndex, (ListItem)L.ToString());
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);

                LayerList.SelectedIndex = id;

                this.Invalidate();
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
                this.lblBP.Visible = E;
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
                switch ((sender as DropDown).SelectedIndex)
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
                List<ABS_Layer> Layers_ = new List<ABS_Layer>();
                foreach (ABS_Layer AL in Layers) Layers_.Add(AL);
                return Layers_;                
            }

            private void Update_Graphs()
            {
                if (LayerList.Items.Count < 1) return;
                
                if (Chart_Contents.SelectedIndex == 0)
                {
                    Alpha_Normal.Plot.Clear();
                    Polar_Absorption.Plot.Clear();
                    Impedance_Graph.Plot.Clear();

                    Alpha_Normal.Plot.YAxis.Label.Text = "Alpha (0 to 1)";

                    if (Fin_Sample.Checked && Zr == null) return;

                    List<ABS_Layer> Layers_ = Material_Layers();

                    if (Layers_.Count == 0) return;

                    if (Inf_Sample.Checked) sm = new Environment.Smart_Material(Air_Term.Checked, Layers_, 16000, 1.2, 343, Averaging.SelectedIndex);
                    else sm = new Environment.Smart_Material(Air_Term.Checked, Layers_, 16000, 1.2, 343, Zr, 0.1, Averaging.SelectedIndex, Zf_Incorp_Method.SelectedIndex);

                    double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();

                    //Polar Graph
                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;

                    RI_Absorption = new double[8];
                    Polar_Absorption.Plot.YAxis.Min = 0;

                    Polar_Absorption.Plot.YAxis.Max = 1;

                    ScottPlot.Color[] colors = new Color[8] { ScottPlot.Colors.Red, ScottPlot.Colors.Orange, ScottPlot.Colors.Yellow, ScottPlot.Colors.Green, ScottPlot.Colors.Blue, ScottPlot.Colors.BlueViolet, ScottPlot.Colors.Violet, ScottPlot.Colors.Plum };

                    for (int oct = 0; oct < 8; oct++)
                    {
                        //Polar_Absorption.Series[oct].Points.DataBindXY(AnglesDeg, sm.Ang_Coef_Oct[oct]);
                        Polar_Absorption.Plot.Add.Scatter(AnglesDeg, sm.Ang_Coef_Oct[oct], colors[oct]);
                        (Polar_Absorption.Plot.PlottableList[oct] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;

                        for (int a = 0; a < sm.Ang_Coef_Oct[oct].Length; a++)
                        {
                            if (Polar_Absorption.Plot.YAxis.Max < sm.Ang_Coef_Oct[oct][a]) Polar_Absorption.Plot.YAxis.Max = Math.Ceiling(sm.Ang_Coef_Oct[oct][a] * 10) / 10;
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

                    Impedance_Graph.Plot.Add.Scatter(logfreq, real, ScottPlot.Colors.Blue);
                    Impedance_Graph.Plot.Add.Scatter(logfreq, imag, ScottPlot.Colors.Red);
                    (Impedance_Graph.Plot.PlottableList[0] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;
                    (Impedance_Graph.Plot.PlottableList[1] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;

                    Impedance_Graph.Plot.YAxis.Max = 4000;
                    Impedance_Graph.Plot.YAxis.Min = -6000;

                    //Alpha Normal graph
                    RI_Absorption = sm.RI_Coef;

                    Alpha_Normal.Plot.YAxis.Max = 1;
                    if (Fin_Sample.Checked) { foreach (double a in RI_Absorption) if (Alpha_Normal.Plot.YAxis.Max < a) Alpha_Normal.Plot.YAxis.Max = Math.Ceiling(a * 10) / 10; }
                    foreach (double a in sm.NI_Coef) if (Alpha_Normal.Plot.YAxis.Max < a) Alpha_Normal.Plot.YAxis.Max = Math.Ceiling(a * 10) / 10;

                    if (Direction_choice.SelectedIndex == 0)
                    {
                        //for (int i = 0; i < sm.frequency.Length; i++) Alpha_Normal.Series[0].Points.AddXY(sm.frequency[i], sm.NI_Coef[i]);
                        Alpha_Normal.Plot.Add.Scatter(logfreq, sm.NI_Coef, ScottPlot.Colors.Blue);//
                        (Alpha_Normal.Plot.PlottableList[0] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;

                    }
                    else
                    {
                        double[][] acoef = AbsorptionModels.Operations.Absorption_Coef(sm.Reflection_Coefficient); //(sm.Z, sm.frequency);
                                                                                                                   //for (int i = 0; i < sm.frequency.Length; i++) Alpha_Normal.Series[0].Points.AddXY(sm.frequency[i], acoef[Direction_choice.SelectedIndex + 17][i]); //sm.NI_Coef[i]);
                        Alpha_Normal.Plot.Add.Scatter(logfreq, acoef[Direction_choice.SelectedIndex + 17], ScottPlot.Colors.Blue);
                        (Alpha_Normal.Plot.PlottableList[0] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;
                    }

                    //double[] xs = new double[8] { 63, 125, 250, 500, 1000, 2000, 4000, 8000 };
                    double[] xs = new double[8] { 3, 4, 5, 6, 7, 8, 9, 10 };
                    double[] ys = new double[8];
                    for (int oct = 0; oct < 8; oct++) ys[oct] = RI_Absorption[oct];
                    Alpha_Normal.Plot.Add.Scatter(xs, ys, ScottPlot.Colors.Red);
                }
                else
                {
                    Alpha_Normal.Plot.Clear();
                    Polar_Absorption.Plot.Clear();
                    Impedance_Graph.Plot.Clear();

                    if (Rigid_Term.Checked) return;

                    Alpha_Normal.Plot.YAxis.Label.Text = "Transmission Loss (dB)";

                    if (Fin_Sample.Checked && Zr == null) return;

                    List<ABS_Layer> Layers = Material_Layers();

                    if (Layers.Count == 0) return;

                    if (Inf_Sample.Checked) sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343, Averaging.SelectedIndex);
                    else sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343, Zr, 0.1, Averaging.SelectedIndex, Zf_Incorp_Method.SelectedIndex);

                    double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();

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

                    Impedance_Graph.Plot.Add.Scatter(logfreq, real, ScottPlot.Colors.Blue);
                    Impedance_Graph.Plot.Add.Scatter(logfreq, imag, ScottPlot.Colors.Red);
                    (Impedance_Graph.Plot.PlottableList[0] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;
                    (Impedance_Graph.Plot.PlottableList[1] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;

                    Impedance_Graph.Plot.YAxis.Max = 40000;
                    Impedance_Graph.Plot.YAxis.Min = -60000;

                    Impedance_Graph.Plot.XAxis.TickGenerator.Ticks = new Tick[12] 
                    {
                            new Tick(1, "16 Hz."), 
                            new Tick(2, "31.25 Hz."),
                            new Tick(3, "62.5 Hz."),
                            new Tick(4, "125 Hz."),
                            new Tick(5, "250 Hz."),
                            new Tick(6, "500 Hz."),
                            new Tick(7, "1000 Hz."),
                            new Tick(8, "2000 Hz."),
                            new Tick(9, "4000 Hz."),
                            new Tick(10, "8000 Hz."),
                            new Tick(11, "16000 Hz."),
                            new Tick(12, "32000 Hz.")
                    };

                    //T graph...
                    double[] TL = new double[sm.frequency.Length];
                    double[] aTL = new double[sm.frequency.Length];

                    double max = double.NegativeInfinity;

                    for (int i = 0; i < sm.frequency.Length; i++)
                    {
                        //Complex tau = 0;
                        //for (int a = 0; a < sm.Angles.Length; a++)
                        //{
                        //    tau += sm.Trans_Coefficient[a][i] * Math.Cos(a * sm.Angles.Length / 180) * Math.Sin(a * sm.Angles.Length / 180);                        //TL[i] = 10 * Math.Log10((sm.Trans_Loss[19][i].Real * sm.Trans_Loss[19][i].Real));
                        //}
                        //aTL[i] = -10 * Complex.Log10(tau).Real;
                        TL[i] = -10 * Complex.Log10(sm.Trans_Coefficient[Direction_choice.SelectedIndex + 17][i]).Real;                        //TL[i] = 10 * Math.Log10((sm.Trans_Loss[19][i].Real * sm.Trans_Loss[19][i].Real));
                        max = Math.Max(TL[i], max);
                    }

                    double maxTL = 0;
                    double minTL = double.PositiveInfinity;
                    Polar_Absorption.Plot.YAxis.Min = 0;
                    Polar_Absorption.Plot.YAxis.Max = 1;

                    ScottPlot.Color[] colors = new Color[8] { ScottPlot.Colors.Red, ScottPlot.Colors.Orange, ScottPlot.Colors.Yellow, ScottPlot.Colors.Green, ScottPlot.Colors.Blue, ScottPlot.Colors.BlueViolet, ScottPlot.Colors.Violet, ScottPlot.Colors.Plum };

                    for (int oct = 0; oct < 8; oct++)
                    {
                        double[] TL_ang = new double[sm.Ang_tau_Oct[oct].Length];
                        for (int a = 0; a < sm.Ang_tau_Oct[oct].Length; a++)
                        {
                            //if (Polar_Absorption.ChartAreas[0].AxisY.Maximum < sm.Ang_Coef_Oct[oct][a])
                            //{
                            double TLnow = sm.Ang_tau_Oct[oct][a];// - 10 * Math.Log10(sm.Ang_tau_Oct[oct][a]);
                            maxTL = Math.Max(TLnow, maxTL);
                            minTL = Math.Min(TLnow, minTL);
                            TL_ang[a] = TLnow;
                                //Polar_Absorption.ChartAreas[0].AxisY.Maximum = sm.Ang_tau_Oct[oct][a] * 10;
                            //}
                        }
                        Polar_Absorption.Plot.Add.Scatter(AnglesDeg, TL_ang, colors[oct]);
                        (Polar_Absorption.Plot.PlottableList[oct] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;
                    }
                    Polar_Absorption.Plot.YAxis.Max = 1; //Math.Min(1, maxTL);// maxTL;
                    if (Chart_Contents.SelectedIndex == 0) Polar_Absorption.Plot.Title("Absorption Coefficient by Angle of Incidence");
                    else Polar_Absorption.Plot.Title("Transmission Coefficient by Angle of Incidence");

                    for (int i = 0; i < sm.frequency.Length; i++) Alpha_Normal.Plot.Add.Scatter(logfreq[i], TL[i]);
                    double[] xs = new double[8] { 63, 125, 250, 500, 1000, 2000, 4000, 8000 };
                    double[] ys = new double[8];
                    for (int oct = 0; oct < 8; oct++)
                    {
                        double TLnow = -10 * Math.Log10(sm.TI_Coef[oct]);
                        ys[oct] = TLnow;
                    }
                    Alpha_Normal.Plot.YAxis.Max = -10 * Math.Log10(minTL);
                    if (Chart_Contents.SelectedIndex == 0) Alpha_Normal.Plot.TitlePanel.Label.Text = "Absorption Coefficient";
                    else Alpha_Normal.Plot.TitlePanel.Label.Text = "Transmission Loss (dB)";
                }

                Impedance_Graph.Plot.XAxis.TickGenerator = new ScottPlot.TickGenerators.NumericManual(new Tick[8]
                {
                            //new Tick(1, "16"),
                            //new Tick(2, "31"),
                            new Tick(3, "63"),
                            new Tick(4, "125"),
                            new Tick(5, "250"),
                            new Tick(6, "500"),
                            new Tick(7, "1k"),
                            new Tick(8, "2k"),
                            new Tick(9, "4k"),
                            new Tick(10, "8k")
                            //new Tick(11, "16k"),
                            //new Tick(12, "32k")
                });

               Alpha_Normal.Plot.XAxis.TickGenerator = new ScottPlot.TickGenerators.NumericManual( new Tick[8]
                {
                            //new Tick(1, "16 Hz."),
                            //new Tick(2, "31.25 Hz."),
                            new Tick(3, "62.5 Hz."),
                            new Tick(4, "125 Hz."),
                            new Tick(5, "250 Hz."),
                            new Tick(6, "500 Hz."),
                            new Tick(7, "1000 Hz."),
                            new Tick(8, "2000 Hz."),
                            new Tick(9, "4000 Hz."),
                            new Tick(10, "8000 Hz.")
                            //new Tick(11, "16000 Hz."),
                            //new Tick(12, "32000 Hz.")
                });
                Alpha_Normal.Plot.Legend.ManualItems.Add(new LegendItem());
                Alpha_Normal.Plot.Legend.ManualItems.Add(new LegendItem());
                Alpha_Normal.Plot.Legend.ManualItems[0].LineColor = ScottPlot.Colors.Blue;
                Alpha_Normal.Plot.Legend.ManualItems[1].LineColor = ScottPlot.Colors.Red;
                Alpha_Normal.Plot.Legend.ManualItems[1].Marker.Fill.Color = ScottPlot.Colors.Red;
                Alpha_Normal.Plot.Legend.ManualItems[0].Marker = MarkerStyle.None;
                Alpha_Normal.Plot.Legend.ManualItems[0].Label = "Angular Absorption Coefficient";
                Alpha_Normal.Plot.Legend.ManualItems[1].Label = "Random Incidence Absorption Coefficient";

                Alpha_Normal.Plot.Legend.IsVisible = true;

                Polar_Absorption.Plot.XAxis.Label.Text = "Angle of Incidence (degrees)";
                Polar_Absorption.Plot.YAxis.Label.Text = "Coefficient";
                Polar_Absorption.Plot.XAxis.Label.Font.Size = 10;
                Polar_Absorption.Plot.YAxis.Label.Font.Size = 10;
                for (int oct = 0; oct < 8; oct++)
                {
                    Polar_Absorption.Plot.Legend.ManualItems.Add(new LegendItem());
                    Polar_Absorption.Plot.Legend.ManualItems[oct].Marker.IsVisible = false;
                }
                Polar_Absorption.Plot.Legend.ManualItems[0].LineColor = ScottPlot.Colors.Red;
                Polar_Absorption.Plot.Legend.ManualItems[1].LineColor = ScottPlot.Colors.Orange;
                Polar_Absorption.Plot.Legend.ManualItems[2].LineColor = ScottPlot.Colors.Yellow;
                Polar_Absorption.Plot.Legend.ManualItems[3].LineColor = ScottPlot.Colors.Green;
                Polar_Absorption.Plot.Legend.ManualItems[4].LineColor = ScottPlot.Colors.Blue;
                Polar_Absorption.Plot.Legend.ManualItems[5].LineColor = ScottPlot.Colors.BlueViolet;
                Polar_Absorption.Plot.Legend.ManualItems[6].LineColor = ScottPlot.Colors.Violet;
                Polar_Absorption.Plot.Legend.ManualItems[7].LineColor = ScottPlot.Colors.Plum;
                Polar_Absorption.Plot.Legend.ManualItems[0].Label = "63 Hz.";
                Polar_Absorption.Plot.Legend.ManualItems[1].Label = "125 Hz.";
                Polar_Absorption.Plot.Legend.ManualItems[2].Label = "250 Hz.";
                Polar_Absorption.Plot.Legend.ManualItems[3].Label = "500 Hz.";
                Polar_Absorption.Plot.Legend.ManualItems[4].Label = "1 kHz.";
                Polar_Absorption.Plot.Legend.ManualItems[5].Label = "2 kHz.";
                Polar_Absorption.Plot.Legend.ManualItems[6].Label = "4 kHz.";
                Polar_Absorption.Plot.Legend.ManualItems[7].Label = "8 kHz.";
                Polar_Absorption.Plot.Legend.IsVisible = true;
                Polar_Absorption.Plot.Legend.OutlineStyle = LineStyle.None;
                Polar_Absorption.Plot.Legend.BackgroundFill = new FillStyle();
                Polar_Absorption.Plot.Legend.BackgroundFill.Color = ScottPlot.Color.FromARGB(0);

                //Alpha_Normal.Plot.AutoScale();
                Alpha_Normal.Plot.XAxis.Min = 2.5;
                Alpha_Normal.Plot.XAxis.Max = 10.5;
                Alpha_Normal.Plot.YAxis.Min = 0;
                Alpha_Normal.Plot.YAxis.Max = 1.2;
                Alpha_Normal.Plot.YAxis.Label.Font.Size = 10;
                Alpha_Normal.Plot.XAxis.Label.Text = "Frequency (Hz.)";
                Alpha_Normal.Plot.XAxis.Label.Font.Size = 10;
                Alpha_Normal.Invalidate();
                //Impedance_Graph.Plot.AutoScale();
                Impedance_Graph.Plot.XAxis.Min = 2.5;
                Impedance_Graph.Plot.XAxis.Max = 10.5;
                Impedance_Graph.Invalidate();
                Impedance_Graph.Plot.XAxis.Label.Text = "Frequency (Hz.)";
                Impedance_Graph.Plot.XAxis.Label.Font.Size = 10;
                Polar_Absorption.Plot.YAxis.Min = 0;
                Polar_Absorption.Plot.YAxis.Max = 1;
                Polar_Absorption.Plot.XAxis.Min = -90;
                Polar_Absorption.Plot.XAxis.Max = 90;

                Polar_Absorption.Invalidate();
                //Estimate_IIR();
            }

            //public void Estimate_IIR()
            //{
            //    //Complex[] IR = Audio.Pach_SP.IFFT_General(Audio.Pach_SP.Mirror_Spectrum(sm.Reflection_Coefficient[18]), 0);
            //    //for (int i = 0; i < IR.Length; i++) IR[i] = IR[i].Real;
            //    //Complex[] R = Audio.Pach_SP.IIR_Design.AutoCorrelation_Coef(IR, (int)IIR_Order.Value);

            //    //Complex[] a, b;
            //    //Audio.Pach_SP.IIR_Design.Yule_Walker(R, out a, out b);

            //    double[] a, b;
            //    Complex[] RefSpectrum = sm.Reflection_Coefficient[18];
            //    Audio.Pach_SP.IIR_Design.OptimizeIIR(RefSpectrum, 16000, (int)IIR_Order.Value, out a, out b);

            //    Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b),new List<double>(a), sm.frequency);
            //    //Audio.Pach_SP.IIR_Design.SpectrumFromIIR(a, b, 16000, 4096);

            //    double corr = Audio.Pach_SP.IIR_Design.CrossCorrelation_Coef(RefSpectrum, IIR_spec).Real;

            //    Rhino.RhinoApp.WriteLine(string.Format("Correlation: {0}", corr));

            //    //Normalize
            //    //double ms = 0;
            //    //foreach (Complex s in IIR_spec) ms = ((ms > s.Magnitude) ? ms : s.Magnitude);
            //    //double mb = 0;
            //    //foreach (Complex s in RefSpectrum) mb = ((mb > s.Magnitude) ? mb : s.Magnitude);

            //    //for (int i = 0; i < IIR_spec.Length; i++) IIR_spec[i] *= mb / ms;

            //    double[] alpha = AbsorptionModels.Operations.Absorption_Coef(IIR_spec);

            //    Alpha_Normal.Series[2].Points.Clear();
            //    for (int i = 0; i < alpha.Length; i++) Alpha_Normal.Series[2].Points.AddXY((double)(i + 1)* 16000 / alpha.Length, alpha[i]);
            //}

            //public Chart Polar_Plot()
            //{
            //    return Polar_Absorption;
            //}

            private void param_ValueChanged(object sender, EventArgs e)
            {
                int id = LayerList.SelectedIndex;
                if (indexchanged == true) return;
                if (LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (Layers[LayerList.SelectedIndex] as ABS_Layer);

                switch (Material_Type.SelectedIndex)
                {
                    case 0:
                        if (L.T != ABS_Layer.LayerType.AirSpace) return;
                        break;
                    case 1:
                        if (L.T != ABS_Layer.LayerType.BiotPorousAbsorber_Limp) return;
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

                Layers[LayerList.SelectedIndex] = L;
                LayerList.Items.Insert(LayerList.SelectedIndex, (ListItem)L.ToString());
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);
                LayerList.SelectedIndex = id;

                Update_Graphs();
            }

            List<ABS_Layer> Layers = new List<ABS_Layer>();

            private void Add_Click(object sender, EventArgs e)
            {
                if (Material_Type.SelectedIndex < 0) return;
                ABS_Layer abs = null;
                switch (Material_Type.SelectedIndex)
                {
                    case 0:
                        abs = new ABS_Layer(ABS_Layer.LayerType.AirSpace, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 1:
                        abs = ABS_Layer.CreateBiot(true, (double) depth.Value / 1000, (double)Solid_Density.Value, (double)YoungsModulus.Value * 1E9, (double)PoissonsRatio.Value, (double)SoundSpeed.Value, (double)Tortuosity.Value, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, (double)ViscousCharacteristicLength.Value, (double)ThermalPermeability.Value );
                        break;
                    case 2:
                        abs = new ABS_Layer(ABS_Layer.LayerType.PorousDB, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 3:
                        abs = new ABS_Layer(ABS_Layer.LayerType.PorousCA, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 4:
                        abs = new ABS_Layer(ABS_Layer.LayerType.PorousM, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 5:
                        abs = ABS_Layer.CreateSolid((double)depth.Value / 1000, (double)Solid_Density.Value, (double)YoungsModulus.Value * 1E9,  (double)PoissonsRatio.Value);
                        break;
                    case 6:
                        abs = new ABS_Layer(ABS_Layer.LayerType.Perforated_Modal, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 7:
                        abs = new ABS_Layer(ABS_Layer.LayerType.Slotted_Modal, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 8:
                        abs = new ABS_Layer(ABS_Layer.LayerType.CircularPerforations, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break; 
                    case 9:
                        abs = new ABS_Layer(ABS_Layer.LayerType.SquarePerforations, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 10:
                        abs = new ABS_Layer(ABS_Layer.LayerType.Slots, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 11:
                        abs = new ABS_Layer(ABS_Layer.LayerType.Microslit, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                    case 12:
                        abs = new ABS_Layer(ABS_Layer.LayerType.MicroPerforated, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100);
                        break;
                }

                if (abs != null)
                {
                    Layers.Add(abs);
                    LayerList.Items.Add(abs.ToString());
                    LayerList.Invalidate();
                }

                LayerList.SelectedIndex = LayerList.Items.Count - 1;

                Update_Graphs();
            }

            private void Rem_Click(object sender, EventArgs e)
            {
                if (LayerList.SelectedIndex < 0) return;
                Layers.RemoveAt(LayerList.SelectedIndex);
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);
                LayerList.Invalidate();
                Update_Graphs();
            }

            private void Up_Click(object sender, EventArgs e)
            {
                int id = LayerList.SelectedIndex;

                if (LayerList.SelectedIndex < 1) return;
                Layers.Insert(LayerList.SelectedIndex - 1, Layers[LayerList.SelectedIndex]);
                Layers.RemoveAt(LayerList.SelectedIndex + 1);
                LayerList.Items.Insert(LayerList.SelectedIndex - 1, LayerList.Items[LayerList.SelectedIndex]);
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);
                LayerList.SelectedIndex = id - 1;

                LayerList.Invalidate();
                Update_Graphs();
            }

            private void Down_Click(object sender, EventArgs e)
            {
                int id = LayerList.SelectedIndex;

                if (LayerList.SelectedIndex < 0 || LayerList.SelectedIndex == LayerList.Items.Count - 1) return;
                Layers.Insert(LayerList.SelectedIndex + 2, Layers[LayerList.SelectedIndex]);
                Layers.RemoveAt(LayerList.SelectedIndex);
                LayerList.Items.Insert(LayerList.SelectedIndex + 2, LayerList.Items[LayerList.SelectedIndex]);
                LayerList.Items.RemoveAt(LayerList.SelectedIndex);
                LayerList.Invalidate();
                LayerList.SelectedIndex = id + 1;
                Update_Graphs();
            }

            private void LayerList_SelectedIndexChanged(object sender, EventArgs e)
            {
                //Tell the interface that we have selected a material from the list, so that it doesn't update the graph, as the material settings populate.
                indexchanged = true;
                if (LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (Layers[LayerList.SelectedIndex] as ABS_Layer);

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
                depth.Value = L.depth * 1000;
                pitch.Value = L.pitch * 1000;
                diameter.Value = L.width * 1000;
                Porosity_Percent.Value = L.porosity * 100;
                Sigma.Value = L.Flow_Resist;

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

            System.Threading.Thread T;

            private void Calc_Zr_Click(object sender, EventArgs e)
            {
                if (T != null && T.ThreadState == System.Threading.ThreadState.Running) return;
                VisualizationBox B = new VisualizationBox(-90, 90, -15, 15);
                B.Show();
                T = new System.Threading.Thread(() => { Zr = new Environment.Smart_Material.Finite_Field_Impedance((double)XDim.Value, (double)YDim.Value, 10000, 343, 1.2, B); });
                T.Start();
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    //System.Threading.Thread.SpinWait(2);
                    B.Populate();
                    
                } while (T.ThreadState == System.Threading.ThreadState.Running);
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
                Fin_Sample.Checked = false;
                Update_Graphs();
                Is_Finite = !Inf_Sample.Checked;
            }

            private void Fin_Sample_CheckedChanged(object sender, EventArgs e)
            {
                Inf_Sample.Checked = false;
                Update_Graphs();
                Is_Finite = Fin_Sample.Checked;
            }

            private void Chart_Contents_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graphs();
            }

            private void Rigid_Term_CheckedChanged(object sender, EventArgs e)
            {
                if (sender == Rigid_Term)
                {
                    Air_Term.Checked = false;
                    label21.Text = "-- Rigid Substrate (Infinite Impedance) --"; 
                }
                else 
                {
                    Rigid_Term.Checked = false;
                    label21.Text = "-- Receiving Space (Semi-Infinite Air Medium) -- "; 
                }
                this.Update_Graphs();
            }

            private void Direction_choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graphs();
            }
        }
    }
}