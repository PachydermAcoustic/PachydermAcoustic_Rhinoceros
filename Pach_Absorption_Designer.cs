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
using System.Runtime.CompilerServices;
using HDF.PInvoke;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Solvers;
using MathNet.Numerics.Optimization;
using MathNet.Numerics;
using MathNet.Numerics.Optimization.ObjectiveFunctions;
using MathNet.Numerics.Differentiation;
using System.Dynamic;
using MathNet.Numerics.Statistics;
using System.Threading;
using System.Threading.Tasks;
using Rhino.FileIO;

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
                //this.Averaging.SelectedIndex = 0;
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
                    Porosity_Percent.Value = 99.3;
                }
                else if (Sigma.Value < 30000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (45 kg/m^3)";
                    Porosity_Percent.Value = 98.4;
                }
                else if (Sigma.Value < 65000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (90 kg/m^3)";
                    Porosity_Percent.Value = 96.8;
                }
                else if (Sigma.Value < 100000)
                {
                    Resistivity_Feedback.Text = "i.e. Mineral Wool (145 kg/m^3)";
                    Porosity_Percent.Value = 95.2;
                }
                else
                {
                    Resistivity_Feedback.Text = "???";
                    Porosity_Percent.Value = 95.2;
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

                    if (Inf_Sample.Checked) sm = new Environment.Smart_Material(Air_Term.Checked, Layers_, 16000, 1.2, 343);
                    else sm = new Environment.Smart_Material(Air_Term.Checked, Layers_, 16000, 1.2, 343, Zr, 0.1);

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
                        if (sm.RI_Averages != null) Alpha_Normal.Plot.Add.Scatter(logfreq, sm.RI_Averages, ScottPlot.Colors.Maroon);
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

                    if (Inf_Sample.Checked) sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343);
                    else sm = new Environment.Smart_Material(Air_Term.Checked, Layers, 16000, 1.2, 343, Zr, 0.1);

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

                Alpha_Normal.Plot.XAxis.TickGenerator = new ScottPlot.TickGenerators.NumericManual(new Tick[8]
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
                //Estimate_Recursive_IIR();
            }

            //public void Estimate_IIR()
            //{
            //    ////Complex[] IR = Audio.Pach_SP.IFFT_General(Audio.Pach_SP.Mirror_Spectrum(sm.Reflection_Coefficient[18]), 0);
            //    ////for (int i = 0; i < IR.Length; i++) IR[i] = IR[i].Real;
            //    ////Complex[] R = Audio.Pach_SP.IIR_Design.AutoCorrelation_Coef(IR, (int)IIR_Order.Value);

            //    ////Complex[] a, b;
            //    ////Audio.Pach_SP.IIR_Design.Yule_Walker(R, out a, out b);

            //    //double[] a, b;
            //    //Complex[] RefSpectrum = sm.Reflection_Coefficient[18];
            //    //Audio.Pach_SP.IIR_Design.OptimizeIIR(RefSpectrum, 16000, (int)IIR_Order.Value, out a, out b);

            //    Complex[] reflectionCoefficient = sm.Reflection_Spectrum(16000, 4096, new Hare.Geometry.Vector(0, 0, 1), new Hare.Geometry.Vector(0, 0, 1), 0);

            //    double max = 0;
            //    foreach (Complex comp in reflectionCoefficient) max = Math.Max(max, comp.Magnitude);
            //    for (int i = 0; i < reflectionCoefficient.Length; i++) reflectionCoefficient[i] /= max;

            //    // Obtain the impulse response (Inverse FFT)
            //    double[] impulseResponse = Audio.Pach_SP.IFFT_Real4096(reflectionCoefficient, 0);
            //    //for (int i = 0; i < impulseResponse.Length; i++) impulseResponse[i] = Math.Min(impulseResponse[i],0.999999999999999);

            //        // Desired order of the IIR filter
            //        int p = 6;  // Order of the numerator
            //        int q = 5;  // Order of the denominator

            //    //    // Form the Toeplitz matrix T and vector b
            //    //    double[,] T = new double[q, q];
            //    //    for (int i = 0; i < q; i++)
            //    //    {
            //    //        for (int j = 0; j < q; j++)
            //    //        {
            //    //            T[i, j] = impulseResponse[i + j];
            //    //        }
            //    //    }

            //    //    double[] bVec = new double[q];
            //    //    for (int i = 0; i < q; i++)
            //    //    {
            //    //        bVec[i] = -impulseResponse[p + i]; //+1
            //    //    }

            //    //    // Solve the linear system T * a = b
            //    //    double[] aCoefficients = SolveLinearSystem(T, bVec);

            //    //    // Form the denominator polynomial a(z)
            //    //    double[] a = new double[q + 1];
            //    //    a[0] = 1.0;
            //    //    for (int i = 1; i <= q; i++)
            //    //    {
            //    //        a[i] = aCoefficients[i - 1];
            //    //    }

            //    //    // Form the numerator polynomial b(z)
            //    //    double[] c = new double[impulseResponse.Length];
            //    //    for (int i = 0; i < impulseResponse.Length; i++)
            //    //    {
            //    //        c[i] = 0.0;
            //    //        for (int j = 0; j <= i && j <= q; j++)
            //    //        {
            //    //            c[i] += impulseResponse[i - j] * a[j];
            //    //        }
            //    //    }
            //    //    double[] b = new double[p + 1];
            //    //    Array.Copy(c, b, p + 1);

            //    // Step 1: Fit a rational function to find poles and zeros in s-domain
            //    //(Complex[] poles, Complex[] zeros) = FitRationalFunction(reflectionCoefficient);

            //    ////Step 1a: normalize...
            //    //double maxpz = 1;
            //    //foreach (Complex c in poles) maxpz = Math.Max(maxpz, c.Magnitude);
            //    //foreach (Complex c in zeros) maxpz = Math.Max(maxpz, c.Magnitude);
            //    //for (int i = 0; i < poles.Length; i++) poles[i] /= maxpz;
            //    //for (int i = 0; i < zeros.Length; i++) zeros[i] /= maxpz;

            //    //// Step 2: Apply bilinear transform to poles and zeros
            //    //Complex[] zPoles = BilinearTransform(poles, 16000);
            //    //Complex[] zZeros = BilinearTransform(zeros, 16000);

            //    //// Step 3: Formulate the IIR filter coefficients from poles and zeros
            //    //(double[] b, double[] a) = FormulateIIRFilter(zPoles, zZeros);

            //    (double[]a, double[] b) = Fit(impulseResponse, (int)IIR_Order.Value, (int)IIR_Order.Value);

            //    ///////////////////////////////

            //    Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);
            //    //audio.pach_sp.iir_design.spectrumfromiir(a, b, 16000, 4096);

            //    double corr = Audio.Pach_SP.IIR_Design.CrossCorrelation_Coef(reflectionCoefficient, IIR_spec).Real;

            //    Rhino.RhinoApp.WriteLine(string.Format("Correlation: {0}", corr));

            //    //Normalize
            //    //double ms = 0;
            //    //foreach (Complex s in IIR_spec) ms = ((ms > s.Magnitude) ? ms : s.Magnitude);
            //    //double mb = 0;
            //    //foreach (Complex s in RefSpectrum) mb = ((mb > s.Magnitude) ? mb : s.Magnitude);

            //    //for (int i = 0; i < IIR_spec.Length; i++) IIR_spec[i] *= mb / ms;

            //    MathNet.Numerics.
            //    Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);


            //    double[] alpha = AbsorptionModels.Operations.Absorption_Coef(IIR_spec);

            //    //Alpha_Normal.Series[2].Points.Clear();
            //    //for (int i = 0; i < alpha.Length; i++) Alpha_Normal.Plot.Add.Scatter((double)(i + 1) * 16000 / alpha.Length, alpha[i]);
            //    double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();
            //    for (int i = 0; i < alpha.Length; i++) alpha[i] = 1 - alpha[i];
            //    Alpha_Normal.Plot.Add.Scatter(logfreq, alpha);
            //}

            private async void Estimate_Recursive_IIR()
            {
                double[] frequencies = new double[950];
                Array.Copy(sm.frequency, frequencies, 950);
                int fs = 44100;
                double pgbest = 0, zgbest = 0, pfbest = 0, zfbest = 0;
                double err_best = 0.2;
                Complex[] desired_Spectrum = new Complex[(int)(4096 * 10000 / 44100)];
                Array.Copy(sm.Reflection_Spectrum(fs, 4096, new Hare.Geometry.Vector(0, 0, 1), new Hare.Geometry.Vector(0, 0, 1), 0), desired_Spectrum, 743);
                double[] desired_alpha = AbsorptionModels.Operations.Absorption_Coef(desired_Spectrum);

                //Iterate over spectrum:
                Complex[] Zeros = new Complex[0];
                Complex[] Poles = new Complex[0];

                ///////////////////////////////
                ///Execute the search...
                Task<filterspec> t = IIR_Branch(Poles, Zeros, err_best, fs, frequencies, desired_alpha);
                //t.Start();
                await t;
                filterspec result = t.Result as filterspec;
                if (result == null) return;
                ///////////////////////////////

                (double[] b, double[] a) = FormulateIIRFilter(result.Poles, result.Zeros);

                Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), frequencies, 44100);//AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);
                //Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.PZ_FreqResponse(Poles, Zeros, 1, frequencies);//AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);

                double m = IIR_spec.MaximumMagnitudePhase().Magnitude;
                if (m > 1)
                {
                    for (int j = 0; j < b.Length; j++) b[j] /= m;
                    //recalculate
                }
                else
                {
                    double mod = m / desired_Spectrum.MaximumMagnitudePhase().Magnitude;
                    for (int j = 0; j < b.Length; j++) b[j] /= mod;
                }
                IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), frequencies, 44100);//AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);

                double[] alpha = AbsorptionModels.Operations.Absorption_Coef(IIR_spec);
                double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();
                Alpha_Normal.Plot.Add.Scatter(logfreq, alpha);
            }

            private class filterspec
            {
                public double Error;
                public Complex[] Poles;
                public Complex[] Zeros;
            }

            private async Task<filterspec> IIR_Branch(Complex[] Poles, Complex[] Zeros, double error_Current, double fs, double[] frequencies, double[] desired_alpha)
            {
                Random rnd = new Random((int)DateTime.Now.Ticks);
                if (Poles.Length >= 8) return new filterspec() { Poles = Poles, Zeros = Zeros, Error = error_Current };
                double target = 0.05;
                SortedList<double, filterspec> next_attempts = new SortedList<double, filterspec>();
                filterspec finish = null;
                double pgbest = -1, pfbest = -1, zgbest = -1, zfbest = -1;

                List<Task> srch = new List<Task>();
                //Parallel.For(0, 10, pftest => 
                for (double pftest = 0; pftest < 15; pftest += 1.5)
                {
                    //srch.Add(Task.Run(() =>
                    //{
                        double ptheta = 2 * Math.PI * Math.Pow(12, pftest * 2 / 6) / fs;
                        for (double zftest = 0; zftest < 15; zftest += 1.5)
                        {
                            double ztheta = 8000 - 2 * Math.PI * Math.Pow(12, zftest * 2 / 3) / fs;
                            for (double pgtest = .9; pgtest > 0; pgtest -= 0.1)
                            {
                                for (double zgtest = .9; zgtest > 0; zgtest -= 0.1)
                                {
                                    Complex[] Prosp_Poles = new Complex[2];
                                    Complex[] Prosp_Zeros = new Complex[2];
                                    Prosp_Poles[0] = new Complex(pgtest * Math.Cos(ptheta), pgtest * Math.Sin(ptheta));
                                    Prosp_Poles[1] = new Complex(pgtest * Math.Cos(ptheta), -pgtest * Math.Sin(ptheta));
                                    Prosp_Zeros[0] = new Complex(zgtest * Math.Cos(ztheta), zgtest * Math.Sin(ztheta));
                                    Prosp_Zeros[1] = new Complex(zgtest * Math.Cos(ztheta), -zgtest * Math.Sin(ztheta));
                                    Complex[] Test_Poles = new Complex[Poles.Length + 2];
                                    if (Poles.Length > 0) Array.Copy(Poles.ToArray(), Test_Poles, Poles.Length);
                                    Array.Copy(Prosp_Poles, 0, Test_Poles, Poles.Length, 2);
                                    Complex[] Test_Zeros = new Complex[Poles.Length + 2];
                                    if (Zeros.Length > 0) Array.Copy(Zeros.ToArray(), Test_Zeros, Zeros.Length);
                                    Array.Copy(Prosp_Zeros, 0, Test_Zeros, Zeros.Length, 2);
                                    double error = test_spectrum(frequencies, Test_Poles.ToList(), Test_Zeros.ToList(), desired_alpha);

                                    if (error < error_Current)
                                    {
                                        if (error < target)
                                        {
                                            //target = error;
                                            finish = new filterspec() { Poles = Test_Poles, Zeros = Test_Zeros, Error = error };
                                        }
                                        else next_attempts.Add(error + rnd.NextDouble() / 1000, new filterspec() { Poles = Test_Poles, Zeros = Test_Zeros, Error = error });
                                    }
                                }
                            }
                        }
                    }//));
                //}
                //Task.WaitAll(srch.ToArray());

                List<System.Threading.Tasks.Task<filterspec>> t = new List<Task<filterspec>>();
                if (finish == null)
                {
                    // Iterate over up to 10 of the best next_attempts list items and create a task for each item   
                    int limit = Math.Min(1, next_attempts.Count);
                    for(int i = 0; i < limit; i++)
                    {
                        filterspec attempt = next_attempts.ElementAt(i).Value;
                        t.Add(Task.Run(() => IIR_Branch(attempt.Poles, attempt.Zeros, attempt.Error, fs, frequencies, desired_alpha)));
                    }

                    // Await all the tasks and get the results
                    filterspec[] results = await Task.WhenAll(t.ToArray());
                }
                for (int i = 0; i < t.Count; i++)
                {
                    filterspec current = (t[i] as Task<filterspec>).Result as filterspec;
                    if (current == null) continue;
                    double err = current.Error;
                    if (err < target)
                    {
                        target = err;
                        finish = current;
                    }
                }
                return finish;
            }
        
            //private void Estimate_Iterative_IIR()
            //{
            //    double[] frequencies = new double[950];
            //    Array.Copy(sm.frequency, frequencies, 950);
            //    int fs = 44100;
            //    int numPoles = 2;
            //    int numZeros = 2;
            //    Random r = new Random((int)DateTime.Now.Ticks);
            //    double pgbest = 0, zgbest = 0, pfbest = 0, zfbest = 0;
            //    double err_best = double.PositiveInfinity;

            //    Complex[] desired_Spectrum = new Complex[(int)(4096 * 10000/44100)];
            //    Array.Copy(sm.Reflection_Spectrum(fs, 4096, new Hare.Geometry.Vector(0, 0, 1), new Hare.Geometry.Vector(0, 0, 1), 0), desired_Spectrum, 743);
            //    double[] desired_alpha = AbsorptionModels.Operations.Absorption_Coef(desired_Spectrum);

            //    //Iterate over spectrum:

            //    int i = desired_Spectrum.Length - 2;// sm.frequency.Length/6;

            //    List<Complex> Zeros = new List<Complex>();
            //    List<Complex> Poles = new List<Complex>();

            //    Complex[] Prosp_Poles = new Complex[2];
            //    Complex[] Prosp_Zeros = new Complex[2];

            //    //int order = 9;
            //    //int ord = 0;

            //    while ( err_best > 55)
            //    {
            //        ///Find Local Maximum or minimum
            //        //if (Math.Sign(desired_alpha[i - 1] - desired_alpha[i]) != Math.Sign(desired_alpha[i] - desired_alpha[i + 1]))
            //        //{
            //        pgbest = -1; pfbest = -1; zgbest = -1; zfbest = -1;
            //            for (double pftest = 0; pftest < 15; pftest+=1.5)
            //            {
            //                double ptheta = 2 * Math.PI * Math.Pow(12, pftest * 2 / 6) / fs;
            //                for (double zftest = 0; zftest < 15; zftest+=1.5)
            //                {
            //                    double ztheta = 8000 - 2 * Math.PI * Math.Pow(12, zftest * 2 / 3) / fs;
            //                    for (double pgtest = .9; pgtest > 0; pgtest -= 0.2)
            //                    {
            //                        for (double zgtest = .9; zgtest > 0; zgtest -= 0.2)
            //                        {
            //                            Prosp_Poles[0] = new Complex(pgtest * Math.Cos(ptheta), pgtest * Math.Sin(ptheta));
            //                            Prosp_Poles[1] = new Complex(pgtest * Math.Cos(ptheta), -pgtest * Math.Sin(ptheta));
            //                            Prosp_Zeros[0] = new Complex(zgtest * Math.Cos(ztheta), zgtest * Math.Sin(ztheta));
            //                            Prosp_Zeros[1] = new Complex(zgtest * Math.Cos(ztheta), -zgtest * Math.Sin(ztheta));
            //                            Complex[] Test_Poles = new Complex[Poles.Count + 2];
            //                            if (Poles.Count > 0) Array.Copy(Poles.ToArray(), Test_Poles, Poles.Count);
            //                            Array.Copy(Prosp_Poles, 0, Test_Poles, Poles.Count, 2);
            //                            Complex[] Test_Zeros = new Complex[Poles.Count + 2];
            //                            if (Zeros.Count > 0) Array.Copy(Zeros.ToArray(), Test_Zeros, Zeros.Count);
            //                            Array.Copy(Prosp_Zeros, 0, Test_Zeros, Zeros.Count, 2);
                                        
            //                            double error = test_spectrum(frequencies, Test_Poles.ToList(), Test_Zeros.ToList(), desired_alpha);
            //                            if (error < err_best)
            //                            {
            //                                err_best = error;
            //                                pgbest = pgtest;
            //                                zgbest = zgtest;
            //                                pfbest = ptheta;
            //                                zfbest = ztheta;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        if (pgbest == -1) break;

            //        Poles.Add(new Complex(pgbest * Math.Cos(pfbest), pgbest * Math.Sin(pfbest)));
            //            Poles.Add(new Complex(pgbest * Math.Cos(pfbest), -pgbest * Math.Sin(pfbest)));
            //            Zeros.Add(new Complex(zgbest * Math.Cos(zfbest), zgbest * Math.Sin(zfbest)));
            //            Zeros.Add(new Complex(zgbest * Math.Cos(zfbest), -zgbest * Math.Sin(zfbest)));
            //        //}
            //    }

            //    (double[] b, double[] a) = FormulateIIRFilter(Poles.ToArray(), Zeros.ToArray());
            //    Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), frequencies, 44100);//AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);
            //    //Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.PZ_FreqResponse(Poles, Zeros, 1, frequencies);//AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);

            //    double m = IIR_spec.MaximumMagnitudePhase().Magnitude;
            //    if (m > 1)
            //    {
            //        for (int j = 0; j < b.Length; j++) b[j] /= m;
            //        //recalculate
            //        IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), frequencies, 44100);//AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);
            //    }
            //    else
            //    {
            //        double mod = m / desired_Spectrum.MaximumMagnitudePhase().Magnitude;
            //        for (int j = 0; j < b.Length; j++) b[j] /= mod;
            //        IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), frequencies, 44100);//AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);
            //    }

            //    double[] alpha = AbsorptionModels.Operations.Absorption_Coef(IIR_spec);
            //    double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();
            //    Alpha_Normal.Plot.Add.Scatter(logfreq, alpha);
            //}

            private double test_spectrum(double[] frequencies, List<Complex> poles, List<Complex> zeros, double[] desired_alpha)
            { 
                double[] bs, As;
                (bs, As) = FormulateIIRFilter(poles.ToArray(), zeros.ToArray());

                Complex[] response = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(bs), new List<double>(As), frequencies, 44100);
                double m = response.MaximumMagnitudePhase().Magnitude; 
                if (m > 1)
                {
                    for (int j = 0; j < bs.Length; j++) bs[j] /= m;
                }
                else
                {
                    double mod = m / Math.Sqrt(Math.Abs(desired_alpha.Minimum() - 1));
                    for (int j = 0; j < bs.Length; j++) bs[j] /= mod;
                }
                response = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(bs), new List<double>(As), frequencies, 44100);

                //Complex[] response = Audio.Pach_SP.IIR_Design.PZ_FreqResponse(poles, zeros, 1, frequencies);
                double[] alpha_r = AbsorptionModels.Operations.Absorption_Coef(response);

                // Calculate the correlation between the desired and actual spectrum
                double error = 0;
                for (int i = 0; i < desired_alpha.Length; i++)
                {
                    error = Math.Max(Math.Abs((alpha_r[i] - desired_alpha[i])), error);
                }

                return error;
            }
            //public void Estimate_IIR()
            //{
            //    // Initial guess for the zeros and poles (random or based on initial analysis)
            //    //Complex[] initialZeros = Enumerable.Repeat(new Complex(0.5, 0.5), numZeros).ToArray();
            //    //Complex[] initialPoles = Enumerable.Repeat(new Complex(0.5, -0.5), numPoles).ToArray();

            //    //var initialGuess = initialZeros.Concat(initialPoles).SelectMany(c => new[] { c.Real, c.Imaginary }).ToArray();

            //    double[] frequencies = new double[950];
            //    Array.Copy(sm.frequency, frequencies, 950);
            //    int fs = 16000;
            //    int numPoles = 2;
            //    int numZeros = 2;
            
            //    // Initial guess for the zeros and poles (random or based on initial analysis)
            //    Random r = new Random((int)DateTime.Now.Ticks);

            //    double[] initialtheta = new double[numPoles/2];
            //    double[] initialmag = new double[numPoles/2 + numZeros];
            //    for (int i = 0; i < numPoles/2 + numZeros; i++)
            //    {
            //        if (i < numPoles/2) initialtheta[i] = frequencies[(1 + i * frequencies.Length) / (1 + numPoles/2)]/fs;
            //        initialmag[i] = i < numPoles/2 ? r.NextDouble() : - Math.Abs(r.NextDouble());
            //    }
            //    // Convert initial guesses to an array of doubles (real and imaginary parts)
            //    double[] initialGuessArray = new double[numZeros + numPoles];
            //    Array.Copy(initialtheta, initialGuessArray, initialtheta.Length);
            //    Array.Copy(initialmag, 0, initialGuessArray, initialtheta.Length, initialmag.Length);

            //    // Convert to Vector<double>
            //    MathNet.Numerics.LinearAlgebra.Vector<double> initialGuess = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray(initialGuessArray);

            //    Complex[] desired_Spectrum = new Complex[950];
            //    Array.Copy(sm.Reflection_Spectrum(fs, 4096, new Hare.Geometry.Vector(0, 0, 1), new Hare.Geometry.Vector(0, 0, 1), 0), desired_Spectrum, 950);
            //    double[] desired_alpha = AbsorptionModels.Operations.Absorption_Coef(desired_Spectrum);

            //    // Define the objective function to minimize the error
            //    Func<MathNet.Numerics.LinearAlgebra.Vector<double>, double> objectiveFunction = coefficients =>
            //    {
            //        double[] theta = new double[numPoles/2], mag = new double[numZeros + numPoles/2];
            //        Complex[] poles = new Complex[numPoles], zeros = new Complex[numZeros];
            //        for (int i = 0; i < numZeros + numPoles/2; i++)
            //        {
            //            mag[i] = (coefficients[i + numPoles/2] % 2) - 1;
            //            if (i < numPoles / 2)
            //            {
            //                theta[i] = Math.PI * coefficients[i];
            //                poles[2 * i] = new Complex(mag[i] * Math.Cos(theta[i]), mag[i] * Math.Sin(theta[i])) * .95;
            //                poles[2 * i + 1] = new Complex(mag[i] * Math.Cos(-theta[i]), mag[i] * Math.Sin(-theta[i])) * .95;
            //            }
            //            else zeros[i - numPoles / 2] = new Complex(mag[i] , 0);//new Complex(mag[i] * Math.Cos(theta[i]), mag[i] * Math.Sin(theta[i]));
            //        }

            //        double[] bs, As;
            //        (bs, As) = FormulateIIRFilter(poles, zeros);

            //        Complex[] response = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(bs), new List<double>(As), frequencies, 16000);

            //        //Complex[]  response = Audio.Pach_SP.IIR_Design.PZ_FreqResponse(poles.ToList(), zeros.ToList(), 1, frequencies);
            //        //Complex corr = Audio.Pach_SP.IIR_Design.CrossCorrelation_Coef(response, desired_Spectrum);

            //        double[] alpha_r = AbsorptionModels.Operations.Absorption_Coef(response);

            //        // Calculate the correlation between the desired and actual spectrum
            //        double error = 0;
            //        for(int i = 0; i < frequencies.Length; i++)
            //        {
            //            //Complex z = Complex.Exp(new Complex(0, -2 * Math.PI * f / fs));
            //            //Complex H_z = EvaluateFilter(zeros, poles, z);
            //            //error = Math.Max(error, Math.Abs(Math.Log(response[i].Magnitude) - Math.Log(desired_Spectrum[i].Magnitude)));
            //            //error = Math.Max(error, Math.Abs((response[i].Magnitude - desired_Spectrum[i].Magnitude)));
            //            //double e = Math.Abs((alpha_r[i] - desired_alpha[i]));
            //            //if (e > 1) return double.PositiveInfinity;
            //            error += Math.Abs((alpha_r[i] - desired_alpha[i]));
            //        }

            //        return error;//1 - corr.Magnitude;
            //    };

                
            //    // Perform the optimization using BFGS algorithm
            //    var solver = new MathNet.Numerics.Optimization.NelderMeadSimplex(1E-10, 1000000);
            //    var result = solver.FindMinimum(MathNet.Numerics.Optimization.ObjectiveFunction.Value(objectiveFunction), initialGuess);

            //    // Extract the optimized zeros and poles
            //    Complex[] optimizedPoles = new Complex[numZeros], optimizedZeros = new Complex[numZeros];
            //    double[] thetaO = new double[numZeros * 2], magO = new double[numZeros * 2];
            //    for (int i = 0; i < numZeros + numPoles / 2; i++)
            //    {
            //        thetaO[i] = Math.PI * result.MinimizingPoint[i];
            //        //magO[i] = result.MinimizingPoint[i + numZeros + numPoles / 2] % 2 - 1;
            //        //if (i < numPoles / 2)
            //        //{
            //        //    optimizedPoles[2 * i] = new Complex(magO[i] * Math.Cos(thetaO[i]), magO[i] * Math.Sin(thetaO[i]));
            //        //    optimizedPoles[2 * i + 1] = new Complex(magO[i] * Math.Cos(-thetaO[i]), magO[i] * Math.Sin(-thetaO[i]));
            //        //}
            //        //else optimizedZeros[i - numPoles / 2] = new Complex(magO[i] * Math.Cos(thetaO[i]), magO[i] * Math.Sin(thetaO[i]));

            //        magO[i] = (result.MinimizingPoint[i + numPoles / 2] % 2) - 1;
            //        if (i < numPoles / 2)
            //        {
            //            thetaO[i] = 2 * Math.PI * result.MinimizingPoint[i];
            //            optimizedPoles[2 * i] = new Complex(magO[i] * Math.Cos(thetaO[i]), magO[i] * Math.Sin(thetaO[i]));
            //            optimizedPoles[2 * i + 1] = new Complex(magO[i] * Math.Cos(-thetaO[i]), magO[i] * Math.Sin(-thetaO[i]));
            //        }
            //        else optimizedZeros[i - numPoles / 2] = new Complex(magO[i], 0);//new Complex(mag[i] * Math.Cos(theta[i]), mag[i] * Math.Sin(theta[i]));

            //    }
            //    //for (int i = 0; i < numZeros * 2; i++)
            //    //{
            //    //    thetaO[i] = 2 * Math.PI * result.MinimizingPoint[i];
            //    //    magO[i] = result.MinimizingPoint[i + numPoles * 2] % 1;
            //    //    if (i < numZeros) optimizedPoles[i] = new Complex( magO[i] *Math.Cos(thetaO[i]), magO[i] * Math.Sin(thetaO[i]));
            //    //    else optimizedZeros[i - numZeros] = new Complex(magO[i] * Math.Cos(thetaO[i]), magO[i] * Math.Sin(thetaO[i]));
            //    //}

            //    (double[] b, double[] a) = FormulateIIRFilter(optimizedPoles, optimizedZeros);

            //    Complex[] IIR_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), sm.frequency, 16000);

            //    double m = IIR_spec.MaximumMagnitudePhase().Magnitude; if (m > 1) for (int i = 0; i < b.Length; i++) b[i] /= m;

            //    double[] alpha = AbsorptionModels.Operations.Absorption_Coef(IIR_spec);
            //    double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();
            //    Alpha_Normal.Plot.Add.Scatter(logfreq, alpha);
            //}

            private static Complex EvaluateFilter(Complex[] zeros, Complex[] poles, Complex z)
            {
                Complex numerator = zeros.Select(zero => z - zero).Aggregate((product, term) => product * term);
                Complex denominator = poles.Select(pole => z - pole).Aggregate((product, term) => product * term);
                return numerator / denominator;
            }
            public static (double[], double[]) Fit(double[] impulseResponse, int p, int q)
            {
                int N = impulseResponse.Length;
                //normalized IR:
                double max = 0;
                foreach (double i in impulseResponse) max = Math.Max(max, Math.Abs(i));
                for (int i = 0; i < impulseResponse.Length; i++) impulseResponse[i] /= max;

                // Step 1: Create the Toeplitz matrix T
                var T = Matrix<double>.Build.Dense(q, q);
                for (int i = 0; i < q; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        // Ensure that we do not exceed the bounds of the impulseResponse array
                        if (i + j < N)
                        {
                            T[i, j] = impulseResponse[i + j];
                        }
                        else
                        {
                            T[i, j] = 0; // Fill with zeros if out of bounds
                        }
                    }
                }

                // Step 2: Create vector b (right-hand side)
                var bVec = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(q);
                for (int i = 0; i < q; i++)
                {
                    if (p + i < N)
                    {
                        bVec[i] = -impulseResponse[p + i];
                    }
                    else
                    {
                        bVec[i] = 0; // Fill with zeros if out of bounds
                    }
                }

                // Step 3: Solve for the denominator coefficients (a)
                var aVec = T.Solve(bVec);

                // Step 4: Compute the numerator coefficients (b)
                var b = new double[p + 1];
                var a = new double[q + 1];
                a[0] = 1.0; // a0 is typically normalized to 1

                // Copy the solved aVec to the a array, skipping a0
                for (int i = 1; i <= q; i++)
                {
                    a[i] = aVec[i - 1];
                }

                // Compute the numerator coefficients using the recursive relationship
                for (int i = 0; i <= p; i++)
                {
                    b[i] = impulseResponse[i];
                    for (int j = 1; j <= Math.Min(i, q); j++)
                    {
                        b[i] += a[j] * impulseResponse[i - j];
                    }
                }

                return (b, a);
            }
        
            // Step 1: Fit a rational function to determine poles and zeros
            public static (Complex[] poles, Complex[] zeros) FitRationalFunction(Complex[] H_f)
            {
                int maxIterations = 500;
                int order = 6; // Desired order of the numerator and denominator
                int N = H_f.Length;

                // Initialize poles with evenly spaced points on the unit circle (for z-domain)
                Complex[] poles = new Complex[order];
                for (int i = 0; i < order; i++)
                {
                    double theta = 2.0 * Math.PI * i / order;
                    poles[i] = new Complex(Math.Cos(theta), Math.Sin(theta));
                }

                Complex[] a, b = new Complex[0];

                // Iterative fitting process
                for (int iter = 0; iter < maxIterations; iter++)
                {
                    // Solve the linear system to find the coefficients for the current poles
                    (a, b) = SolveForCoefficients(H_f, poles);

                    // Update poles based on current solution
                    for (int i = 0; i < order; i++)
                    {
                        poles[i] = UpdatePole(H_f, poles[i], a, b);
                    }
                }

                // Calculate zeros from the resulting coefficients
                Complex[] zeros = CalculateZeros(b);

                return (poles, zeros);
            }

            // Step 2: Apply bilinear transform to poles and zeros
            public static Complex[] BilinearTransform(Complex[] sDomain, double fs)
            {
                double T = 1.0 / fs;
                Complex[] zDomain = new Complex[sDomain.Length];
                for (int i = 0; i < sDomain.Length; i++)
                {
                    zDomain[i] = (2.0 + T * sDomain[i]) / (2.0 - T * sDomain[i]);
                }
                return zDomain;
            }

            // Step 3: Formulate IIR filter from poles and zeros
            //public static (double[], double[]) FormulateIIRFilter(Complex[] zPoles, Complex[] zZeros)
            //{




            //    int order = zPoles.Length;
            //    double[] a = new double[order + 1];
            //    double[] b = new double[order + 1];

            //    a[0] = 1.0;
            //    b[0] = 1.0;

            //    for (int i = 1; i <= order; i++)
            //    {
            //        a[i] = 0;
            //        b[i] = 0;
            //    }

            //    // Multiply factors of (z - pole) for the denominator and (z - zero) for the numerator
            //    for (int i = 0; i < order; i++)
            //    {
            //        for (int j = order; j > 0; j--)
            //        {
            //            if (i < zPoles.Length) a[j] = a[j] - zPoles[i].Real * a[j - 1];
            //            if (i < zZeros.Length) 
            //                b[j] = b[j] - zZeros[i].Real * b[j - 1];
            //        }
            //    }

            //    return (b, a);
            //}

            //    public (double[] a, double[] b) FormulateIIRFilter(Complex[] sDomainPoles, Complex[] sDomainZeros, double fs)
            //    {
            //        // Apply Bilinear Transform
            //        Complex[] zPoles = sDomainPoles;// BilinearTransform(sDomainPoles, fs);
            //        Complex[] zZeros = sDomainZeros;// BilinearTransform(sDomainZeros, fs);

            //        // Ensure stability by keeping poles within the unit circle
            //        //zPoles = EnsureStability(zPoles);

            //        // Convert poles and zeros to polynomial coefficients
            //        double[] a = PolynomialFromRoots(zPoles);
            //        double[] b = PolynomialFromRoots(zZeros);

            //        // Normalize the coefficients
            //        double gain = b[0] / a[0];
            //        for (int i = 0; i < b.Length; i++)
            //        {
            //            b[i] /= gain;
            //        }

            //        return (a, b);
            //    }

            //    public static double[] PolynomialFromRoots(Complex[] roots)
            //    {
            //        int order = roots.Length;
            //        double[] coefficients = new double[order + 1];
            //        coefficients[0] = 1.0;

            //        for (int i = 0; i < order; i++)
            //        {
            //            for (int j = i; j >= 0; j--)
            //            {
            //                coefficients[j + 1] -= roots[i].Real * coefficients[j];
            //            }
            //        }

            //    double leadingCoefficient = 1.0;
            //    for (int i = 0; i < order; i++)
            //    {
            //        leadingCoefficient *= -roots[i].Real;
            //    }

            //    for (int i = 0; i <= order; i++)
            //    {
            //        coefficients[i] *= leadingCoefficient;
            //    }

            //    return coefficients;
            //}

            public (double[], double[]) FormulateIIRFilter(Complex[] poles, Complex[] zeros)
            {
                // Stabilize poles and zeros
                poles = poles.Select(p => p.Magnitude > 1 ? 1.0 / Complex.Conjugate(p) : p).ToArray();
                zeros = zeros.Select(z => z.Magnitude > 1 ? 1.0 / Complex.Conjugate(z) : z).ToArray();

                int order = Math.Max(zeros.Length, poles.Length);

                // Coefficient arrays
                double[] b = new double[order + 1];
                double[] a = new double[order + 1];

                // Initialize coefficients for the denominator (a) with 1
                a[0] = 1.0;
                for (int i = 1; i <= order; i++)
                {
                    a[i] = 0.0;
                }

                // Calculate the denominator coefficients (a) from poles
                foreach (var pole in poles)
                {
                    for (int i = order; i > 0; i--)
                    {
                        a[i] = a[i] - pole.Real * a[i - 1] + pole.Imaginary * a[i - 1];
                    }
                    a[0] = a[0] - pole.Real;
                }

                // Initialize coefficients for the numerator (b) with 1
                b[0] = 1.0;
                for (int i = 1; i <= order; i++)
                {
                    b[i] = 0.0;
                }

                // Calculate the numerator coefficients (b) from zeros
                foreach (var zero in zeros)
                {
                    for (int i = order; i > 0; i--)
                    {
                        b[i] = b[i] - zero.Real * b[i - 1] + zero.Imaginary * b[i - 1];
                    }
                    b[0] = b[0] - zero.Real;
                }

                //// Normalize the coefficients so that a[0] = 1
                //double normFactor = a[0];
                //for (int i = 0; i <= order; i++)
                //{
                //    a[i] /= normFactor;
                //    b[i] /= normFactor;
                //}
                return (b, a);
            }

            public static (Complex[] a, Complex[] b) SolveForCoefficients(Complex[] H_f, Complex[] poles)
            {
                int N = H_f.Length;
                int order = poles.Length;

                // Form the linear system matrix and vector
                var A = MathNet.Numerics.LinearAlgebra.Matrix<Complex>.Build.Dense(N, order + 1);
                var bVec = MathNet.Numerics.LinearAlgebra.Vector<Complex>.Build.Dense(N);

                for (int i = 0; i < N; i++)
                {
                    bVec[i] = H_f[i];
                    A[i, 0] = 1.0;
                    for (int j = 1; j <= order; j++)
                    {
                        A[i, j] = Complex.One / (Complex.One - poles[j - 1] * H_f[i]);
                    }
                }

                // Solve the least squares problem: A * x = bVec
                var qr = A.QR();
                var x = qr.Solve(bVec);

                // Extract the numerator (b) and denominator (a) coefficients from the solution
                var a = new Complex[order + 1];
                var b = new Complex[order + 1];

                for (int i = 0; i <= order; i++)
                {
                    a[i] = x[i]; // First half of x gives a
                    b[i] = x[i]; // Second half of x gives b (this depends on how you structure the problem)
                }
                return (a, b);
            }

            public static Complex UpdatePole(Complex[] H_f, Complex pole, Complex[] a, Complex[] b, double learningRate = 0.1)
            {
                int N = H_f.Length;

                // Compute the current error
                double initialError = ComputeError(H_f, pole, a, b);

                // Small step to estimate the gradient (finite difference method)
                double delta = 1e-6;

                // Perturb the pole slightly in the real direction
                Complex polePerturbedReal = new Complex(pole.Real + delta, pole.Imaginary);
                double errorReal = ComputeError(H_f, polePerturbedReal, a, b);

                // Perturb the pole slightly in the imaginary direction
                Complex polePerturbedImaginary = new Complex(pole.Real, pole.Imaginary + delta);
                double errorImaginary = ComputeError(H_f, polePerturbedImaginary, a, b);

                // Compute the gradient of the error with respect to the pole
                double gradientReal = (errorReal - initialError) / delta;
                double gradientImaginary = (errorImaginary - initialError) / delta;

                // Update the pole position using the gradient
                double newReal = pole.Real - learningRate * gradientReal;
                double newImaginary = pole.Imaginary - learningRate * gradientImaginary;
                Complex updatedPole = new Complex(newReal, newImaginary);

                return updatedPole;
            }

            private static double ComputeError(Complex[] H_f, Complex pole, Complex[] a, Complex[] b)
            {
                int N = H_f.Length;
                double error = 0.0;

                for (int i = 0; i < N; i++)
                {
                    // Compute the modeled response at each frequency point using the current pole
                    Complex modeledResponse = 0.0;
                    for (int j = 0; j < a.Length; j++)
                    {
                        modeledResponse += b[j] / (Complex.One - pole * H_f[i]);
                    }

                    // Compute the squared error between the actual and modeled response
                    Complex diff = H_f[i] - modeledResponse;
                    error += diff.Magnitude * diff.Magnitude;
                }

                return error;
            }

            public static Complex[] CalculateZeros(Complex[] b)
            {
                int n = b.Length - 1; // Degree of the polynomial
                Complex[] roots = new Complex[n];

                // Step 1: Initialize roots with initial guesses (evenly distributed on the unit circle)
                for (int i = 0; i < n; i++)
                {
                    double angle = 2 * Math.PI * i / n;
                    roots[i] = new Complex(Math.Cos(angle), Math.Sin(angle));
                }

                // Step 2: Iteratively refine the roots
                double tolerance = 1e-9; // Convergence tolerance
                int maxIterations = 1000;

                for (int iter = 0; iter < maxIterations; iter++)
                {
                    bool converged = true;
                    for (int i = 0; i < n; i++)
                    {
                        Complex numerator = EvaluatePolynomial(b, roots[i]);
                        Complex denominator = new Complex(1.0, 0.0);
                        for (int j = 0; j < n; j++)
                        {
                            if (i != j) denominator *= (roots[i] - roots[j]);
                        }
                        Complex delta = numerator / denominator;
                        roots[i] -= delta;
                        if (delta.Magnitude > tolerance) converged = false;
                    }

                    if (converged) break;
                }

                return roots;
            }
            private static Complex EvaluatePolynomial(Complex[] b, Complex z)
            {
                Complex result = new Complex(0.0, 0.0);
                for (int i = 0; i < b.Length; i++)
                {
                    result += b[i] * Complex.Pow(z, b.Length - 1 - i);
                }
                return result;
            }

            // Function to solve a linear system of equations using Gaussian elimination
            public static double[] SolveLinearSystem(double[,] T, double[] b)
                {
                    int n = b.Length;
                    double[] x = new double[n];
                    double[,] A = new double[n, n + 1];

                    // Form the augmented matrix
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            A[i, j] = T[i, j];
                        }
                        A[i, n] = b[i];
                    }

                    // Perform Gaussian elimination
                    for (int i = 0; i < n; i++)
                    {
                        for (int k = i + 1; k < n; k++)
                        {
                            double factor = A[k, i] / A[i, i];
                            for (int j = i; j < n + 1; j++)
                            {
                                A[k, j] -= factor * A[i, j];
                            }
                        }
                    }

                    // Back-substitution
                    for (int i = n - 1; i >= 0; i--)
                    {
                        x[i] = A[i, n];
                        for (int j = i + 1; j < n; j++)
                        {
                            x[i] -= A[i, j] * x[j];
                        }
                        x[i] /= A[i, i];
                    }

                    return x;
                }


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

            private async void Calc_Zr_Click(object sender, EventArgs e)
            {
                Calc_Zr.Enabled = false;
                if (T != null && T.ThreadState == System.Threading.ThreadState.Running) return;
                VisualizationBox B = new VisualizationBox(-90, 90, -15, 15);
                B.Show();
                double xd = (double)XDim.Value;
                double yd = (double)YDim.Value;
                T = new System.Threading.Thread(() => { Zr = new Environment.Smart_Material.Finite_Field_Impedance(xd, yd, 10000, 343, 1.2, B); });
                T.Start();
                do
                {
                    await System.Threading.Tasks.Task.Delay(50);
                    B.Populate();
                } while (T.ThreadState != System.Threading.ThreadState.Stopped);
                Calc_Zr.Enabled = true;
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