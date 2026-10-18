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
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;
using System.Diagnostics.Eventing.Reader;

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
            int _radiationGeneration;
            public AbsorptionModelResult Result = AbsorptionModelResult.Cancel;
            public double[] RI_Absorption;
            public bool Is_Finite = false;
            public bool ThirdOctave { get { return Band_Resolution.SelectedIndex == 1; } }
            EHM_Data_Library EHM = new EHM_Data_Library();
            public enum AbsorptionModelResult
            {
                Random_Incidence,
                Smart_Material,
                Cancel
            }

            public Pach_Absorption_Designer()
            {
                InitializeComponent();
                Alpha_Normal.Plot.ShowLegend(ScottPlot.Edge.Top);
                Alpha_Normal.Plot.Legend.TightHorizontalWrapping = true;
                Alpha_Normal.Plot.Legend.FontSize = 9;
                this.Chart_Contents.SelectedIndex = 0;
                for (int i = 0; i < EHM.Names.Count; i++)
                {
                    Material_List.Items.Add(EHM.Names[i]);
                }
                _buildTimer.Elapsed += (sender, args) => Build_Material();
                Porosity_Percent.ValueChanged += param_ValueChanged;
                EC_Name.TextChanged += param_ValueChanged;
                EC_Coefficient.ValueChanged += param_ValueChanged;
                XDim.ValueChanged += SampleDimension_ValueChanged;
                YDim.ValueChanged += SampleDimension_ValueChanged;
                Closed += (sender, args) =>
                {
                    _designerClosed = true;
                    _iirPending = null;
                    _buildGeneration++;
                    _buildTimer.Stop();
                    _buildTimer.Dispose();
                    _buildCts?.Cancel();
                    _iirCts?.Cancel();
                };
                _designerLoading = false;
                Update_Graphs();
            }

            public void Load_Buildup(string code, bool finiteSample)
            {
                List<ABS_Layer> restored = code.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ABS_Layer.LayerFromCode).ToList();
                _designerLoading = true;
                try
                {
                    Layers.Clear();
                    LayerList.Items.Clear();
                    Layers.AddRange(restored);
                    foreach (ABS_Layer layer in Layers) LayerList.Items.Add(layer.ToString());
                    Inf_Sample.Checked = !finiteSample;
                    Fin_Sample.Checked = finiteSample;
                    Is_Finite = finiteSample;
                    if (Layers.Count > 0) LayerList.SelectedIndex = 0;
                }
                finally { _designerLoading = false; }
                Update_Graphs();
            }

            private void Material_List_SelectedIndexChanged(object sender, EventArgs e)
            {
                Fit_MaterialIndex = -1;
                Set_Est.Enabled = false;
                int id = Material_List.SelectedIndex;
                if (id < 0) return;
                else if (id < EHM.ECC_Abs.Count)
                {
                    string name = EHM.Names[id];
                    string category = EHM.Category[id];
                    string application = EHM.Application[id];
                    double density_kgm = EHM.Density_kgm[id];
                    double thickness_m = EHM.Thickness[id];
                    double flow_resistivity = EHM.Flow_Resistivity[id];
                    double fr_dev = EHM.FR_Dev[id];
                    EHM_ECC_Thickness.Text = "   Ref depth: " + EHM.Thickness[id] + " m";
                    EHM_ECC.Text = "A1-A3 ECC (by area): " + EHM.ECC_Spec[id] + " (kgCO2e/m2)";
                    EHM_ECC_ABS.Text = "A1-A3 ECC (by volume): " + Math.Round(EHM.ECC_Abs[id], 2) + " (kgCO2e/m3)";
                    EHM_Flow_Resist.Text = "Flow Resistivity (σ): " + EHM.Flow_Resistivity[id] + " pa*s/m^2";
                }
                else 
                {
                    //Substrate
                    id -= EHM.ECC_Abs.Count;
                    EHM_ECC_Thickness.Text = "   Ref depth: Multiple Layers";
                    EHM_ECC.Text = "A1-A3 ECC (by area): " + EHM.substrate_ECC[id] + " (kgCO2e/m2)";
                    EHM_ECC_ABS.Text = "";//"A1-A3 ECC (by volume): " + Math.Round(EHM.substrate_ECC[id], 2) + " (kgCO2e/m3)";
                    EHM_Flow_Resist.Text = "";//"Flow Resistivity (σ): " + EHM.Flow_Resistivity[id] + " pa*s/m^2";
                }
            }

            private void Resistivity_ValueChanged(object sender, EventArgs e)
            {
                if (indexchanged) return;
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

                if (_designerLoading || LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (Layers[LayerList.SelectedIndex] as ABS_Layer);
                L.Flow_Resist = (double)Sigma.Value;
                L.porosity = (double)Porosity_Percent.Value / 100;

                Layers[LayerList.SelectedIndex] = L;
                LayerList.Items[id] = (ListItem)L.ToString();

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

            private CancellationTokenSource _buildCts;
            private readonly UITimer _buildTimer = new UITimer { Interval = 0.25 };
            private bool _buildRunning, _buildPending, _buildReady, _designerClosed;
            private bool _designerLoading = true;
            private int _buildGeneration;
            private int _builtGeneration;
            private double[] _iirAlpha, _iirLogfreq;
            private readonly List<(double Center, double SampleFrequency, Pach_Absorption_Filter_Fitter.LayerFitResult Fit)> _iirCoefficientFits =
                new List<(double, double, Pach_Absorption_Filter_Fitter.LayerFitResult)>();
            private CancellationTokenSource _iirCts;
            private bool _iirRunning;
            private Environment.Smart_Material _iirPending;
            private int _iirPendingGeneration;

            private List<ABS_Layer> Material_Layers_Snapshot(IEnumerable<ABS_Layer> layers = null)
            {
                List<ABS_Layer> snapshot = new List<ABS_Layer>(Layers.Count);
                foreach (ABS_Layer layer in layers ?? Layers)
                {
                    ABS_Layer copy = new ABS_Layer(layer.T, layer.depth, layer.pitch, layer.width,
                        layer.Flow_Resist, layer.porosity, layer.Embodied_Carbon,
                        layer.Material_Name ?? "Unknown");
                    copy.density = layer.density;
                    copy.YoungsModulus = layer.YoungsModulus;
                    copy.PoissonsRatio = layer.PoissonsRatio;
                    copy.tortuosity = layer.tortuosity;
                    copy.Viscous_Characteristic_Length = layer.Viscous_Characteristic_Length;
                    copy.Thermal_Permeability = layer.Thermal_Permeability;
                    copy.shape = layer.shape;
                    copy.grid = layer.grid;
                    copy.SpeedOfSound = layer.SpeedOfSound;
                    snapshot.Add(copy);
                }
                return snapshot;
            }

            private void Update_Graphs(bool rebuild = true)
            {
                if (_designerLoading || _designerClosed || indexchanged) return;
                if (!rebuild)
                {
                    if (_buildReady && _builtGeneration == _buildGeneration) Render_Graphs();
                    return;
                }
                _buildGeneration++;
                _iirPending = null;
                _buildCts?.Cancel();
                _iirCts?.Cancel();
                _buildTimer.Stop();
                _buildReady = false;
                Smart_Mat.Enabled = Use_RI.Enabled = false;
                _buildPending = Layers.Count > 0 && (!Fin_Sample.Checked || (XDim.Value > 0 && YDim.Value > 0));
                if (Layers.Count == 0)
                {
                    sm = null;
                    RI_Absorption = null;
                    Alpha_Normal.Plot.Clear();
                    Polar_Absorption.Plot.Clear();
                    Impedance_Graph.Plot.Clear();
                    EmbodiedCarbon_Pie.Plot.Clear();
                    Alpha_Normal.Refresh();
                    Polar_Absorption.Refresh();
                    Impedance_Graph.Refresh();
                    EmbodiedCarbon_Pie.Refresh();
                }
                else if (sm != null)
                {
                    foreach (var graph in new[] { Alpha_Normal, Polar_Absorption, Impedance_Graph, EmbodiedCarbon_Pie })
                    {
                        // Keep one overlay while rapid edits replace the pending calculation.
                        if (graph.Plot.PlottableList.LastOrDefault() is Rectangle) continue;
                        var limits = graph.Plot.Axes.GetLimits();
                        double width = Math.Max(1, limits.Right - limits.Left);
                        double height = Math.Max(1, limits.Top - limits.Bottom);
                        Rectangle overlay = graph.Plot.Add.Rectangle(limits.Left - 1000 * width, limits.Right + 1000 * width,
                            limits.Bottom - 1000 * height, limits.Top + 1000 * height);
                        overlay.FillColor = new ScottPlot.Color(255, 255, 255, 200);
                        graph.Refresh();
                    }
                }
                Title = "Materials Designer — " + (Layers.Count == 0 ? "Add a material layer to begin." :
                    !_buildPending ? "Enter positive finite-sample dimensions." : "Updating material — faded graphs show the previous result…");
                if (_buildPending) _buildTimer.Start();
            }

            private async void Build_Material()
            {
                _buildTimer.Stop();
                if (_designerClosed || _buildRunning || !_buildPending) return;
                _buildPending = false;
                _buildRunning = true;
                int myGeneration = _buildGeneration;
                _buildCts = new CancellationTokenSource();
                CancellationTokenSource cancellation = _buildCts;
                CancellationToken token = cancellation.Token;
                try
                {
                    List<ABS_Layer> layers = Material_Layers_Snapshot();
                    bool transmission = Air_Term.Checked;
                    bool infiniteSample = Inf_Sample.Checked;
                    var zrLocal = Zr;
                    double xd = XDim.Value, yd = YDim.Value;
                    Title = "Materials Designer — Calculating material — faded graphs show the previous result…";
                    Environment.Smart_Material result = await Task.Run(() =>
                    {
                        if (token.IsCancellationRequested) return null;
                        return infiniteSample ? new Environment.Smart_Material(transmission, layers, 16000, 1.2, 343, token) :
                            new Environment.Smart_Material(transmission, layers, 16000, 1.2, 343,
                                zrLocal ?? new Environment.Smart_Material.Finite_Field_Impedance(xd, yd, 343, 1.2), 0.1, token);
                    });
                    Application.Instance.Invoke(() =>
                    {
                        if (_designerClosed || token.IsCancellationRequested || myGeneration != _buildGeneration || result == null || result.frequency == null) return;
                        sm = result;
                        RI_Absorption = ThirdOctave ? sm.RI_Coef_3rd : sm.RI_Coef;
                        _iirAlpha = _iirLogfreq = null;
                        _iirCoefficientFits.Clear();
                        _builtGeneration = myGeneration;
                        _buildReady = true;
                        Render_Graphs();
                        Smart_Mat.Enabled = Use_RI.Enabled = true;
                        _ = Fit_IIR_Async(result, myGeneration, token);
                    });
                }
                catch (Exception ex)
                {
                    Application.Instance.Invoke(() =>
                    {
                        if (!_designerClosed && myGeneration == _buildGeneration)
                            Title = "Materials Designer — " + (_buildReady ? "Material ready; filter fit failed: " : "Material update failed: ") + ex.Message;
                    });
                }
                finally
                {
                    Application.Instance.Invoke(() =>
                    {
                        _buildRunning = false;
                        if (_buildCts == cancellation) _buildCts = null;
                        cancellation.Dispose();
                        if (_designerClosed) return;
                        if (_buildPending) _buildTimer.Start();
                    });
                }
            }

            private void Render_Graphs()
            {
                if (sm == null || sm.frequency == null) return;
                double[] bandCenters = ThirdOctave ? Utilities.AcousticalMath.ThirdOctaveCenters() : Utilities.AcousticalMath.OctaveCenters();
                double[] bandRI = ThirdOctave ? sm.RI_Coef_3rd : sm.RI_Coef;
                RI_Absorption = bandRI;
                double[] bandTI = ThirdOctave ? sm.TI_Coef_3rd : sm.TI_Coef;
                double[][] bandAlpha = ThirdOctave ? sm.Ang_Coef_3rd : sm.Ang_Coef_Oct;
                double[][] bandTau = ThirdOctave ? sm.Ang_tau_3rd : sm.Ang_tau_Oct;
                double[] bandX = bandCenters.Select(y => Math.Log(y / 7.8125, 2)).ToArray();
                int bandCount = bandCenters.Length;
                Color[] thirdColors = ThirdOctave ? Enumerable.Range(0, bandCount)
                    .Select(i => new ScottPlot.Colormaps.Turbo().GetColor((double)i / (bandCount - 1))).ToArray() : null;

                foreach (var graph in new[] { Alpha_Normal, Polar_Absorption, Impedance_Graph, EmbodiedCarbon_Pie })
                {
                    if (graph.Plot.PlottableList.LastOrDefault() is Rectangle overlay)
                    {
                        graph.Plot.Remove(overlay);
                        graph.Refresh();
                    }
                }

                if (Chart_Contents.SelectedIndex == 0)
                {
                    Alpha_Normal.Plot.Clear();
                    Polar_Absorption.Plot.Clear();
                    Impedance_Graph.Plot.Clear();

                    Alpha_Normal.Plot.YLabel("Alpha (0 to 1)", 10);

                    double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();

                    //Polar Graph
                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;

                    RI_Absorption = bandRI;
                    Polar_Absorption.Plot.Axes.SetLimitsY(0, 1);

                    ScottPlot.Color[] colors = new Color[8] { ScottPlot.Colors.DarkRed, ScottPlot.Colors.Red, ScottPlot.Colors.Orange, ScottPlot.Colors.Yellow, ScottPlot.Colors.Green, ScottPlot.Colors.Blue, ScottPlot.Colors.BlueViolet, ScottPlot.Colors.Violet };
                    if (ThirdOctave) colors = thirdColors;

                    for (int oct = 0; oct < bandCount; oct++)
                    {
                        var polarPlot = Polar_Absorption.Plot.Add.PolarAxis(1);

                        polarPlot.Circles.ForEach(x => x.LineColor = Colors.Grey);
                        polarPlot.Spokes.ForEach(x => x.LineColor = Colors.Grey);
                        polarPlot.Rotation = Angle.FromDegrees(90);
                        List<Coordinates> polar = new List<Coordinates>();
                        for (int i = 0; i < AnglesDeg.Length; i++)
                        {
                            polar.Add(polarPlot.GetCoordinates(bandAlpha[oct][i], AnglesDeg[i]));
                        }

                        Polar_Absorption.Plot.Add.ScatterLine(polar, colors[oct % colors.Length]);
                        (Polar_Absorption.Plot.PlottableList.Last() as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;

                        for (int a = 0; a < bandAlpha[oct].Length; a++)
                        {
                            if (Polar_Absorption.Plot.Axes.Left.Max < bandAlpha[oct][a])
                                Polar_Absorption.Plot.Axes.Left.Max = Math.Ceiling(bandAlpha[oct][a] * 10) / 10;
                        }
                    }

                    Polar_Absorption.Plot.Title("Absorption Coefficient by Angle of Incidence", size: 14);
                    Polar_Absorption.Plot.Layout.Frameless();
                    Polar_Absorption.Plot.Axes.Margins(0, 0, 0, 0);
                    Polar_Absorption.Plot.Benchmark.IsVisible = false;
                    Polar_Absorption.Plot.Axes.SetLimits(-1.5, 1.5, -1.5, 1.5);
                    Polar_Absorption.Refresh();

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

                    Impedance_Graph.Plot.Axes.SetLimitsY(-6000, 4000);

                    List<ABS_Layer> layers = Material_Layers();
                    List<PieSlice> slices = new List<PieSlice>();

                    Dictionary<string, double> materialValues = new Dictionary<string, double>();

                    foreach (ABS_Layer l in layers)
                    {
                        string name = l.Material_Name;
                        double layer_Carbon = l.Embodied_Carbon * l.depth;
                        if (materialValues.ContainsKey(name))
                        {
                            materialValues[name] += layer_Carbon;
                        }
                        else
                        {
                            materialValues.Add(name, layer_Carbon);
                        }
                    }

                    Color[] pie_color = new Color[]
                    {
                        ScottPlot.Colors.Red, ScottPlot.Colors.Orange, ScottPlot.Colors.Yellow,
                        ScottPlot.Colors.Green, ScottPlot.Colors.Blue, ScottPlot.Colors.BlueViolet,
                        ScottPlot.Colors.Violet, ScottPlot.Colors.Plum, ScottPlot.Colors.Cyan,
                        ScottPlot.Colors.Magenta, ScottPlot.Colors.Lime, ScottPlot.Colors.Pink,
                        ScottPlot.Colors.Teal, ScottPlot.Colors.Brown, ScottPlot.Colors.Gold,
                        ScottPlot.Colors.Coral, ScottPlot.Colors.Turquoise, ScottPlot.Colors.Indigo,
                        ScottPlot.Colors.Salmon, ScottPlot.Colors.Khaki
                    };

                    foreach (var kvp in materialValues)
                    {
                        string label = kvp.Key + " :\n" + Math.Round(kvp.Value, 1).ToString() + " kgCO2e/m2";
                        double value = kvp.Value;
                        if (value == 0) continue;
                        int i = slices.Count % pie_color.Length;
                        PieSlice slice = new PieSlice(value, pie_color[i], label);
                        slice.LabelStyle.Bold = true;
                        slices.Add(slice);
                    }

                    EmbodiedCarbon_Pie.Plot.Clear();
                    EmbodiedCarbon_Pie.Plot.Axes.Frameless();
                    EmbodiedCarbon_Pie.Plot.Axes.Margins(0, 0, 0, 0);
                    var pie = EmbodiedCarbon_Pie.Plot.Add.Pie(slices.ToArray());
                    pie.SliceLabelDistance = 0.75;
                    pie.ExplodeFraction = .1;
                    EmbodiedCarbon_Pie.Plot.Axes.SetLimitsX(1, 1);
                    EmbodiedCarbon_Pie.Plot.Legend.IsVisible = true;
                    EmbodiedCarbon_Pie.Refresh();

                    //Alpha Normal graph
                    RI_Absorption = bandRI;

                    Alpha_Normal.Plot.Axes.Left.Max = 1;
                    if (Fin_Sample.Checked) { foreach (double a in RI_Absorption) if (Alpha_Normal.Plot.Axes.Left.Max < a) Alpha_Normal.Plot.Axes.Left.Max = Math.Ceiling(a * 10) / 10; }
                    foreach (double a in sm.NI_Coef) if (Alpha_Normal.Plot.Axes.Left.Max < a) Alpha_Normal.Plot.Axes.Left.Max = Math.Ceiling(a * 10) / 10;

                    if (Direction_choice.SelectedIndex == 0)
                    {
                        if (sm.RI_Averages != null) Alpha_Normal.Plot.Add.Scatter(logfreq, sm.RI_Averages, ScottPlot.Colors.Maroon);
                        Alpha_Normal.Plot.Add.Scatter(logfreq, sm.NI_Coef, ScottPlot.Colors.Blue);
                        (Alpha_Normal.Plot.PlottableList[0] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;
                        double[] logfreq_third = thirdOctaveBands.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();
                        if (Material_List.SelectedIndex >= 0 && Show_ECC_ABS.Checked == true) Alpha_Normal.Plot.Add.Scatter(logfreq_third, EHM.Abs_Coef[Material_List.SelectedIndex], ScottPlot.Colors.Green);
                    }
                    else
                    {
                        double[][] acoef = AbsorptionModels.Operations.Absorption_Coef(sm.Reflection_Coefficient);
                        Alpha_Normal.Plot.Add.Scatter(logfreq, acoef[Direction_choice.SelectedIndex + 17], ScottPlot.Colors.Blue);
                        (Alpha_Normal.Plot.PlottableList[0] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;
                    }

                    double[] xs = bandX;
                    double[] ys = new double[bandCount];
                    for (int oct = 0; oct < bandCount; oct++) ys[oct] = RI_Absorption[oct];
                    Alpha_Normal.Plot.Add.Scatter(xs, ys, ScottPlot.Colors.Red);
                }
                else
                {
                    Alpha_Normal.Plot.Clear();
                    Polar_Absorption.Plot.Clear();
                    Impedance_Graph.Plot.Clear();

                    if (Rigid_Term.Checked) return;

                    Alpha_Normal.Plot.YLabel("Transmission Loss (dB)", 10);

                    double[] logfreq = sm.frequency.ToList().Select(y => Math.Log(y / 7.8125, 2)).ToArray();

                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;

                    RI_Absorption = bandRI;

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

                    Impedance_Graph.Plot.Axes.SetLimitsY(-60000, 40000);

                    Impedance_Graph.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(new Tick[12]
                    {
                        new Tick(1, "16 Hz."), new Tick(2, "31.25 Hz."), new Tick(3, "62.5 Hz."),
                        new Tick(4, "125 Hz."), new Tick(5, "250 Hz."), new Tick(6, "500 Hz."),
                        new Tick(7, "1000 Hz."), new Tick(8, "2000 Hz."), new Tick(9, "4000 Hz."),
                        new Tick(10, "8000 Hz."), new Tick(11, "16000 Hz."), new Tick(12, "32000 Hz.")
                    });

                    double[] TL = new double[sm.frequency.Length];
                    double max = double.NegativeInfinity;

                    for (int i = 0; i < sm.frequency.Length; i++)
                    {
                        TL[i] = -10 * Complex.Log10(sm.Trans_Coefficient[Direction_choice.SelectedIndex + 17][i]).Real;
                        max = Math.Max(TL[i], max);
                    }

                    double maxTL = 0;
                    double minTL = double.PositiveInfinity;
                    Polar_Absorption.Plot.Axes.SetLimitsY(0, 1);

                    ScottPlot.Color[] colors = new Color[8] { ScottPlot.Colors.Red, ScottPlot.Colors.Orange, ScottPlot.Colors.Yellow, ScottPlot.Colors.Green, ScottPlot.Colors.Blue, ScottPlot.Colors.BlueViolet, ScottPlot.Colors.Violet, ScottPlot.Colors.Plum };
                    if (ThirdOctave) colors = thirdColors;

                    for (int oct = 0; oct < bandCount; oct++)
                    {
                        double[] TL_ang = new double[bandTau[oct].Length];
                        for (int a = 0; a < bandTau[oct].Length; a++)
                        {
                            double TLnow = bandTau[oct][a];
                            maxTL = Math.Max(TLnow, maxTL);
                            minTL = Math.Min(TLnow, minTL);
                            TL_ang[a] = TLnow;
                        }
                        Polar_Absorption.Plot.Add.Scatter(AnglesDeg, TL_ang, colors[oct % colors.Length]);
                        (Polar_Absorption.Plot.PlottableList[oct] as ScottPlot.Plottables.Scatter).MarkerStyle = MarkerStyle.None;
                    }

                    Polar_Absorption.Plot.Axes.Left.Max = 1;
                    Polar_Absorption.Plot.Title("Transmission Coefficient by Angle of Incidence");

                    Alpha_Normal.Plot.Add.ScatterLine(logfreq, TL, ScottPlot.Colors.Blue);
                    double[] xs = bandX;
                    double[] ys = new double[bandCount];
                    for (int oct = 0; oct < bandCount; oct++)
                    {
                        double TLnow = -10 * Math.Log10(bandTI[oct]);
                        ys[oct] = TLnow;
                    }
                    Alpha_Normal.Plot.Add.Scatter(xs, ys, ScottPlot.Colors.Red);
                    Alpha_Normal.Plot.Axes.SetLimitsY(0, -10 * Math.Log10(minTL));
                    Alpha_Normal.Plot.Title("Transmission Loss (dB)");
                }

                Impedance_Graph.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(new Tick[12]
                {
                    new Tick(1, "16"), new Tick(2, "31"), new Tick(3, "63"), new Tick(4, "125"),
                    new Tick(5, "250"), new Tick(6, "500"), new Tick(7, "1k"), new Tick(8, "2k"),
                    new Tick(9, "4k"), new Tick(10, "8k"), new Tick(11, "16k"), new Tick(12, "32k")
                });

                Alpha_Normal.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(new Tick[12]
                {
                    new Tick(1, "16"), new Tick(2, "31"), new Tick(3, "63"), new Tick(4, "125"),
                    new Tick(5, "250"), new Tick(6, "500"), new Tick(7, "1k"), new Tick(8, "2k"),
                    new Tick(9, "4k"), new Tick(10, "8k"), new Tick(11, "16k"), new Tick(12, "32k")
                });

                Alpha_Normal.Plot.Legend.ManualItems.Clear();
                Alpha_Normal.Plot.Legend.ManualItems.Add(new LegendItem());
                Alpha_Normal.Plot.Legend.ManualItems.Add(new LegendItem());
                Alpha_Normal.Plot.Legend.ManualItems[0].LineStyle = new LineStyle(1, ScottPlot.Colors.Blue, LinePattern.Solid);
                Alpha_Normal.Plot.Legend.ManualItems[1].LineStyle = new LineStyle(1, ScottPlot.Colors.Red, LinePattern.Solid);
                Alpha_Normal.Plot.Legend.ManualItems[1].MarkerStyle = new MarkerStyle(MarkerShape.FilledSquare, 3, ScottPlot.Colors.Red);
                Alpha_Normal.Plot.Legend.ManualItems[0].MarkerStyle = MarkerStyle.None;
                Alpha_Normal.Plot.Legend.ManualItems[0].LabelText = "Angular Absorption Coefficient";
                Alpha_Normal.Plot.Legend.ManualItems[1].LabelText = "Random Incidence Absorption Coefficient";
                Alpha_Normal.Plot.Legend.IsVisible = false; // The panel above the graph renders the legend.

                Polar_Absorption.Plot.XLabel("Angle of Incidence (degrees)", 10);
                Polar_Absorption.Plot.YLabel("Coefficient", 10);
                Polar_Absorption.Plot.Legend.ManualItems.Clear();
                ScottPlot.Color[] legendColors = new Color[] { Colors.DarkRed, Colors.Red, Colors.Orange, Colors.Yellow, Colors.Green, Colors.Blue, Colors.BlueViolet, Colors.Violet };
                if (ThirdOctave) legendColors = thirdColors;
                for (int oct = 0; oct < bandCount; oct++)
                {
                    Polar_Absorption.Plot.Legend.ManualItems.Add(new LegendItem()
                    {
                        LabelText = bandCenters[oct].ToString("0.#") + " Hz",
                        MarkerStyle = MarkerStyle.None,
                        LineStyle = new LineStyle(1, legendColors[oct % legendColors.Length], LinePattern.Solid)
                    });
                }
                Polar_Absorption.Plot.Legend.IsVisible = true;
                Polar_Absorption.Plot.Legend.Orientation = ThirdOctave ? ScottPlot.Orientation.Horizontal : ScottPlot.Orientation.Vertical;
                Polar_Absorption.Plot.Legend.FontSize = ThirdOctave ? 9 : 12;
                Polar_Absorption.Plot.Legend.TightHorizontalWrapping = ThirdOctave;
                Polar_Absorption.Plot.Legend.OutlineStyle = LineStyle.None;
                Polar_Absorption.Plot.Legend.BackgroundColor = ScottPlot.Color.FromARGB(0);

                if (Chart_Contents.SelectedIndex == 0) Alpha_Normal.Plot.Axes.SetLimits(2.5, ThirdOctave ? 10.5 + 1.0 / 3 : 10.5, 0, 1.2);
                else Alpha_Normal.Plot.Axes.SetLimitsX(2.5, 10.5);
                Alpha_Normal.Plot.XLabel("Frequency (Hz.)", 10);
                Impedance_Graph.Plot.Axes.SetLimitsX(2.5, 10.5);
                Impedance_Graph.Plot.XLabel("Frequency (Hz.)", 10);
                Alpha_Normal.Plot.Benchmark.IsVisible = false;

                Alpha_Normal.Invalidate();
                Impedance_Graph.Invalidate();
                Polar_Absorption.Invalidate();

                if (Chart_Contents.SelectedIndex == 0 && _iirAlpha != null)
                {
                    var fit = Alpha_Normal.Plot.Add.Scatter(_iirLogfreq, _iirAlpha, ScottPlot.Colors.Green);
                    fit.MarkerStyle = MarkerStyle.None;
                    Alpha_Normal.Invalidate();
                }
                if (Chart_Contents.SelectedIndex == 0 && _iirCoefficientFits.Count > 0)
                {
                    Color[] colors = new Color[]
                    {
                        new Color(75, 105, 180, 210), new Color(30, 145, 185, 210),
                        new Color(40, 155, 120, 210), new Color(125, 155, 45, 210),
                        new Color(185, 140, 30, 210), new Color(195, 100, 50, 210),
                        new Color(180, 70, 115, 210), new Color(130, 85, 180, 210)
                    };
                    foreach (var preview in _iirCoefficientFits)
                    {
                        int band = (int)Math.Round(Math.Log(preview.Center / 62.5, 2));
                        var fit = Alpha_Normal.Plot.Add.Scatter(preview.Fit.LogFrequencies, preview.Fit.FitAlpha, colors[band]);
                        fit.MarkerStyle = MarkerStyle.None;
                        fit.LineWidth = 1.5f;
                        fit.LinePattern = LinePattern.Dashed;
                        fit.LegendText = "FVM α-only: fs " + preview.SampleFrequency.ToString("0") + " Hz";
                        Alpha_Normal.Plot.PlottableList.Remove(fit);
                        Alpha_Normal.Plot.PlottableList.Insert(0, fit);
                    }
                    Alpha_Normal.Invalidate();
                }
            }
            private async Task Fit_IIR_Async(Environment.Smart_Material smLocal, int myGeneration, CancellationToken token)
            {
                if (_designerClosed || token.IsCancellationRequested || myGeneration != _buildGeneration) return;
                if (_iirRunning)
                {
                    _iirPending = smLocal;
                    _iirPendingGeneration = myGeneration;
                    Title = "Materials Designer — Material ready; optional filter fit queued…";
                    return;
                }
                _iirRunning = true;
                CancellationTokenSource fitCancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
                _iirCts = fitCancellation;
                token = fitCancellation.Token;
                Title = "Materials Designer — Material ready; fitting FVM previews…";
                try
                {
                    // Same octave presets and sample-rate relation as the FVM fitter's
                    // SetFitParameters: fmax = center*sqrt(2), fs = fmax/sqrt(2)*10.
                    double[] centers = Utilities.AcousticalMath.OctaveCenters();
                    int failures = 0;
                    for (int band = 0; band < centers.Length; band++)
                    {
                        if (_designerClosed || token.IsCancellationRequested || myGeneration != _buildGeneration) return;
                        double center = centers[band];
                        double limit = center * Math.Sqrt(2);
                        double sampleRate = limit / Math.Sqrt(2) * 10;
                        Title = "Materials Designer — Fitting FVM absorption preview " + (band + 1) + "/" + centers.Length +
                            " (fs " + sampleRate.ToString("0") + " Hz)…";
                        var preview = await Task.Run(() => Pach_Absorption_Filter_Fitter.EvaluateAlphaOnlyMaterial(smLocal, 0, limit, sampleRate, token));
                        if (_designerClosed || token.IsCancellationRequested || myGeneration != _buildGeneration) return;
                        if (preview == null || !string.IsNullOrEmpty(preview.ErrorMessage) || preview.FitAlpha == null)
                        {
                            failures++;
                            continue;
                        }
                        _iirCoefficientFits.Add((center, sampleRate, preview));
                        Render_Graphs();
                    }
                    if (_designerClosed || token.IsCancellationRequested || myGeneration != _buildGeneration) return;
                    Title = "Materials Designer — FVM previews ready; fitting Botts preview…";
                    double fs = 44100;
                    double maxfreq = 10000;
                    double[] iirAlpha = null, iirLogfreq = null;
                    await Task.Run(() =>
                    {
                        if (token.IsCancellationRequested || myGeneration != Volatile.Read(ref _buildGeneration)) return;
                        double[] frequencies;
                        (double[] a, double[] b) = smLocal.Estimate_IIR_Coefficients(fs, maxfreq, out frequencies, 0, token);
                        if (token.IsCancellationRequested || myGeneration != Volatile.Read(ref _buildGeneration)) return;

                        if (a == null || b == null || frequencies == null || frequencies.Length < 2) return;

                        Complex[] Y_spec = Audio.Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), frequencies, fs);

                        int start = 0;
                        while (start < frequencies.Length && frequencies[start] < 1.0) start++;

                        int count = frequencies.Length - start;
                        if (count < 2) return;

                        double[] alpha = new double[count];
                        double[] logfreq = new double[count];

                        for (int i = 0; i < count; i++)
                        {
                            if (token.IsCancellationRequested || myGeneration != Volatile.Read(ref _buildGeneration)) return;
                            int fi = i + start;
                            Complex Y = Y_spec[fi];

                            Complex denom = Complex.One + Y;
                            if (denom.Magnitude < 1e-12) denom = new Complex(1e-12, 0);
                            Complex R = (Complex.One - Y) / denom;

                            double r2 = R.Real * R.Real + R.Imaginary * R.Imaginary;
                            double a_i = 1.0 - r2;
                            if (a_i < 0) a_i = 0;
                            if (a_i > 1) a_i = 1;

                            alpha[i] = a_i;
                            logfreq[i] = Math.Log(frequencies[fi] / 7.8125, 2);
                        }
                        iirAlpha = alpha;
                        iirLogfreq = logfreq;
                    });
                    Application.Instance.Invoke(() =>
                    {
                        if (_designerClosed || token.IsCancellationRequested || myGeneration != _buildGeneration) return;
                        _iirAlpha = iirAlpha;
                        _iirLogfreq = iirLogfreq;
                        Render_Graphs();
                    });
                    if (!_designerClosed && !token.IsCancellationRequested && myGeneration == _buildGeneration)
                        Title = iirAlpha == null ? "Materials Designer — Material ready; Botts fit unavailable." :
                            failures == 0 ? "Materials Designer — Ready." :
                            "Materials Designer — Ready; " + failures + " FVM previews unavailable.";
                }
                catch (Exception error)
                {
                    Application.Instance.Invoke(() =>
                    {
                        if (!_designerClosed && myGeneration == _buildGeneration)
                            Title = "Materials Designer — Material ready; filter fit failed: " + error.Message;
                    });
                }
                finally
                {
                    Application.Instance.Invoke(() =>
                    {
                        _iirRunning = false;
                        if (_iirCts == fitCancellation) _iirCts = null;
                        fitCancellation.Dispose();
                        Environment.Smart_Material pending = _iirPending;
                        int pendingGeneration = _iirPendingGeneration;
                        _iirPending = null;
                        if (!_designerClosed && _buildReady && pending != null && pendingGeneration == _buildGeneration)
                            _ = Fit_IIR_Async(pending, pendingGeneration, CancellationToken.None);
                    });
                }
            }

            private void param_ValueChanged(object sender, EventArgs e)
            {
                int id = LayerList.SelectedIndex;
                if (indexchanged || _designerLoading) return;
                if (LayerList.SelectedIndex < 0) return;
                ABS_Layer L = (Layers[LayerList.SelectedIndex] as ABS_Layer);

                switch (Material_Type.SelectedIndex)
                {
                    case 0:
                        if (L.T != ABS_Layer.LayerType.AirSpace) return;
                        break;
                    case 1:
                        if (L.T != ABS_Layer.LayerType.BiotPorousAbsorber_Limp && L.T != ABS_Layer.LayerType.BiotPorousAbsorber_Rigid) return;
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
                        if (L.T != ABS_Layer.LayerType.SolidPlate) return;
                        break;
                    case 6:
                        if (L.T != ABS_Layer.LayerType.Perforated_Modal) return;
                        break;
                    case 7:
                        if (L.T != ABS_Layer.LayerType.Slotted_Modal) return;
                        break;
                    case 8:
                        if (L.T != ABS_Layer.LayerType.Perforated_Panel) return;
                        break;
                    case 9:
                        if (L.T != ABS_Layer.LayerType.Slotted_Panel) return;
                        break;
                }

                L.Flow_Resist = (double)Sigma.Value;
                L.depth = (double)depth.Value / 1000;
                L.pitch = (double)pitch.Value / 1000;
                L.width = (double)diameter.Value / 1000;
                L.porosity = (double)Porosity_Percent.Value / 100;
                L.PoissonsRatio = (double)PoissonsRatio.Value;
                L.Thermal_Permeability = (double)ThermalPermeability.Value;
                L.YoungsModulus = (double)YoungsModulus.Value*1E9;
                L.Viscous_Characteristic_Length = (double)ViscousCharacteristicLength.Value;
                L.density = (double)Solid_Density.Value;
                L.tortuosity = (double)Tortuosity.Value;
                L.Material_Name = EC_Name.Text;
                L.Embodied_Carbon = EC_Coefficient.Value;

                Layers[LayerList.SelectedIndex] = L;
                LayerList.Items[id] = (ListItem)L.ToString();
                LayerList.SelectedIndex = id;

                Update_Graphs();
            }

            List<ABS_Layer> Layers = new List<ABS_Layer>();

            private void Add_Click(object sender, EventArgs e)
            {
                if (Material_Type.SelectedIndex < 0) return;
                ABS_Layer abs = null;
                string name = null;
                double Carbon = 0;
                if (sender == Set_EHM || sender == Set_Est)
                { 
                    name = EHM.Names[Material_List.SelectedIndex];
                    Carbon = EHM.ECC_Abs[Material_List.SelectedIndex];
                }

                switch (Material_Type.SelectedIndex)
                {
                    case 0:
                        abs = new ABS_Layer(ABS_Layer.LayerType.AirSpace, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, 0, "Air");
                        break;
                    case 1:
                        if (name == null)
                        {
                            name = "Generic Porous Material";
                            Carbon = 70;
                        }
                        abs = ABS_Layer.CreateBiot(true, (double)depth.Value / 1000, (double)Solid_Density.Value, (double)YoungsModulus.Value * 1E9, (double)PoissonsRatio.Value, (double)Tortuosity.Value, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, (double)ViscousCharacteristicLength.Value, (double)ThermalPermeability.Value, Carbon, name);
                        break;
                    case 2:
                        if (name == null)
                        {
                            name = "Generic Porous Material";
                            Carbon = 70;
                        }
                        abs = new ABS_Layer(ABS_Layer.LayerType.PorousDB, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, Carbon, name);
                        break;
                    case 3:
                        if (name == null)
                        {
                            name = "Generic Porous Material";
                            Carbon = 70;
                        }
                        abs = new ABS_Layer(ABS_Layer.LayerType.PorousCA, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, Carbon, name);
                        break;
                    case 4:
                        if (name == null)
                        {
                            name = "Generic Porous Material";
                            Carbon = 70;
                        }
                        abs = new ABS_Layer(ABS_Layer.LayerType.PorousM, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, Carbon, name);
                        break;
                    case 5:
                        if (name == null)
                        {
                            name = "Generic Solid Material";
                            Carbon = 188;
                        }
                        abs = ABS_Layer.CreateSolid((double)depth.Value / 1000, (double)Solid_Density.Value, (double)YoungsModulus.Value * 1E9,  (double)PoissonsRatio.Value, Carbon, name);
                        break;
                    case 6:
                        if (name == null)
                        {
                            name = "Generic Perforated Material";
                            Carbon = 188;
                        }
                        abs = new ABS_Layer(ABS_Layer.LayerType.Perforated_Modal, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, Carbon, name);
                        break;
                    case 7:
                        if (name == null)
                        {
                            name = "Generic Perforated Material";
                            Carbon = 188;
                        }
                        abs = new ABS_Layer(ABS_Layer.LayerType.Slotted_Modal, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, Carbon, name);
                        break;
                    case 8:
                        if (name == null)
                        {
                            name = "Generic Perforated Material";
                            Carbon = 188;
                        }
                        abs = new ABS_Layer(ABS_Layer.LayerType.Perforated_Panel, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, Carbon, name);
                        break; 
                    case 9:
                        if (name == null)
                        {
                            name = "Generic Perforated Material";
                            Carbon = 188;
                        }
                        abs = new ABS_Layer(ABS_Layer.LayerType.Slotted_Panel, (double)depth.Value / 1000, (double)pitch.Value / 1000, (double)diameter.Value / 1000, (double)Sigma.Value, (double)Porosity_Percent.Value / 100, Carbon, name);
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
                indexchanged = true;
                try
                {
                    Layers.Insert(id - 1, Layers[id]);
                    Layers.RemoveAt(id + 1);
                    LayerList.Items.Insert(id - 1, LayerList.Items[id]);
                    LayerList.Items.RemoveAt(id + 1);
                }
                finally { indexchanged = false; }
                LayerList.SelectedIndex = id - 1;

                LayerList.Invalidate();
                Update_Graphs();
            }

            private void Down_Click(object sender, EventArgs e)
            {
                int id = LayerList.SelectedIndex;

                if (LayerList.SelectedIndex < 0 || LayerList.SelectedIndex == LayerList.Items.Count - 1) return;
                indexchanged = true;
                try
                {
                    Layers.Insert(id + 2, Layers[id]);
                    Layers.RemoveAt(id);
                    LayerList.Items.Insert(id + 2, LayerList.Items[id]);
                    LayerList.Items.RemoveAt(id);
                }
                finally { indexchanged = false; }
                LayerList.Invalidate();
                LayerList.SelectedIndex = id + 1;
                Update_Graphs();
            }

            private void LayerList_SelectedIndexChanged(object sender, EventArgs e)
            {
                //Tell the interface that we have selected a material from the list, so that it doesn't update the graph, as the material settings populate.
                if (indexchanged || LayerList.SelectedIndex < 0 || LayerList.SelectedIndex >= Layers.Count) return;
                indexchanged = true;
                try
                {
                    ABS_Layer L = Layers[LayerList.SelectedIndex];

                    switch (L.T)
                    {
                        case ABS_Layer.LayerType.AirSpace:
                            Material_Type.SelectedIndex = 0;
                            break;
                        case ABS_Layer.LayerType.BiotPorousAbsorber_Limp:
                        case ABS_Layer.LayerType.BiotPorousAbsorber_Rigid:
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
                        case ABS_Layer.LayerType.Perforated_Modal:
                            Material_Type.SelectedIndex = 6;
                            break;
                        case ABS_Layer.LayerType.Slotted_Modal:
                            Material_Type.SelectedIndex = 7;
                            break;
                        case ABS_Layer.LayerType.Perforated_Panel:
                            Material_Type.SelectedIndex = 8;
                            break;
                        case ABS_Layer.LayerType.Slotted_Panel:
                            Material_Type.SelectedIndex = 9;
                            break;
                        default:
                            return;
                    }
                    if (L.depth != 0) depth.Value = L.depth * 1000;
                    if (L.pitch != 0) pitch.Value = L.pitch * 1000;
                    if (L.width != 0) diameter.Value = L.width * 1000;
                    if (L.porosity != 0) Porosity_Percent.Value = L.porosity * 100;
                    if (L.Flow_Resist != 0) Sigma.Value = L.Flow_Resist;
                    if (L.density != 0) Solid_Density.Value = L.density;
                    if (L.YoungsModulus != 0) YoungsModulus.Value = L.YoungsModulus * 1E-9;
                    if (L.PoissonsRatio != 0) PoissonsRatio.Value = L.PoissonsRatio;
                    if (L.Thermal_Permeability != 0) ThermalPermeability.Value = L.Thermal_Permeability;
                    if (L.Viscous_Characteristic_Length != 0) ViscousCharacteristicLength.Value = L.Viscous_Characteristic_Length;
                    if (L.tortuosity != 0) Tortuosity.Value = L.tortuosity;
                    if (L.Material_Name != null) EC_Name.Text = L.Material_Name;
                    else {
                        if (Material_Type.SelectedIndex == 0)
                        {
                            EC_Name.Text = "Air";
                        }
                        else if (Material_Type.SelectedIndex < 5)
                        {
                            EC_Name.Text = "Generic Porous Absorber";
                            EC_Coefficient.Value = 70;
                        }
                        else if (Material_Type.SelectedIndex == 5)
                        {
                            EC_Name.Text = "Generic Solid";
                            EC_Coefficient.Value = 188;
                        }
                        else if (Material_Type.SelectedIndex < 12)
                        {
                            EC_Name.Text = "Generic Perforated Material";
                            EC_Coefficient.Value = 188;
                        }
                        else
                        {
                            EC_Name.Text = "Generic Material";
                            EC_Coefficient.Value = 188;
                        }
                    }
                    if (L.Embodied_Carbon != 0) EC_Coefficient.Value = L.Embodied_Carbon;

                }
                finally { indexchanged = false; }
            }

            public double Total_Carbon = 0;

            private void Use_RI_Click(object sender, EventArgs e)
            {
                if (!_buildReady) return;
                Result = AbsorptionModelResult.Random_Incidence;
                Total_Carbon = 0;
                for (int i = 0; i < Layers.Count; i++)
                {
                    Total_Carbon += Layers[i].Embodied_Carbon * Layers[i].depth;
                }
                this.Close();
            }

            private void Smart_Mat_Click(object sender, EventArgs e)
            {
                if (!_buildReady) return;
                Result = AbsorptionModelResult.Smart_Material;
                Total_Carbon = 0;
                for (int i = 0; i < Layers.Count; i++)
                {
                    Total_Carbon += Layers[i].Embodied_Carbon * Layers[i].depth;
                }
                this.Close();
            }

            private void Cancel_Click(object sender, EventArgs e)
            {
                Result = AbsorptionModelResult.Cancel;
                this.Close();
            }

            System.Threading.Thread T;

            private void SampleDimension_ValueChanged(object sender, EventArgs e)
            {
                _radiationGeneration++;
                Zr = null;
                Z_rad_feedback.Text = "Z-rad: Thomasson lookup";
                if (Fin_Sample.Checked) Update_Graphs();
            }

            private async void Calc_Zr_Click(object sender, EventArgs e)
            {
                if (T != null && T.IsAlive) return;
                double xd = XDim.Value;
                double yd = YDim.Value;
                int generation = _radiationGeneration;
                if (xd <= 0 || yd <= 0)
                {
                    Title = "Materials Designer — Enter positive finite-sample dimensions.";
                    return;
                }
                Calc_Zr.Enabled = false;
                VisualizationBox B = new VisualizationBox(-90, 90, -15, 15);
                B.Title = "Radiation impedance — Calculating";
                B.Show();
                Environment.Smart_Material.Finite_Field_Impedance result = null;
                Exception failure = null;
                T = new System.Threading.Thread(() =>
                {
                    try { result = new Environment.Smart_Material.Finite_Field_Impedance(xd, yd, 10000, 343, 1.2, B); }
                    catch (Exception error) { failure = error; }
                }) { IsBackground = true };
                T.Start();
                while (T.IsAlive)
                {
                    await System.Threading.Tasks.Task.Delay(50);
                    if (_designerClosed)
                    {
                        if (B.Visible) B.Close();
                        return;
                    }
                    B.Populate();
                }
                B.Populate();
                if (B.Visible) B.Title = failure == null ? "Radiation impedance — Complete" : "Radiation impedance — Failed";
                Calc_Zr.Enabled = true;
                if (failure != null)
                {
                    Title = "Materials Designer — Radiation impedance failed: " + failure.Message;
                    return;
                }
                if (generation != _radiationGeneration || xd != XDim.Value || yd != YDim.Value) return;
                Zr = result;
                Z_rad_feedback.Text = "Z-rad: calculated";
                Update_Graphs();
            }

            private void Inf_Sample_CheckedChanged(object sender, EventArgs e)
            {
                if (!Inf_Sample.Checked) return;
                Fin_Sample.Checked = false;
                Is_Finite = false;
                Update_Graphs();
            }

            private void Fin_Sample_CheckedChanged(object sender, EventArgs e)
            {
                if (!Fin_Sample.Checked) return;
                Inf_Sample.Checked = false;
                Is_Finite = true;
                Update_Graphs();
            }

            private void Chart_Contents_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graphs(false);
            }

            private void Rigid_Term_CheckedChanged(object sender, EventArgs e)
            {
                if (sender is RadioButton button && !button.Checked) return;
                if (sender == Rigid_Term)
                {
                    label21.Text = "-- Rigid Substrate (Infinite Impedance) --"; 
                }
                else 
                {
                    label21.Text = "-- Receiving Space (Semi-Infinite Air Medium) -- "; 
                }
                this.Update_Graphs();
            }

            private void Direction_choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graphs(false);
            }
        }
    }
}
