using Eto.Forms;
using Hare.Geometry;
using Pachyderm_Acoustic.Audio;
using Pachyderm_Acoustic.Environment;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public class Pach_Absorption_Filter_Fitter : FloatingForm
        {
            private readonly List<Environment.Material> _materials;
            private readonly List<LayerFitRow> _rows;

            private readonly NumericStepper _filterOrder;
            private readonly NumericStepper _maxFrequency;
            private readonly NumericStepper _selectedLayerOrder;
            private readonly Button _evaluateAll;
            private readonly Button _evaluateSelected;
            private readonly Button _accept;
            private readonly Button _close;
            private readonly Eto.Forms.Label _status;
            private readonly Eto.Forms.Label _metrics;
            private readonly TextArea _coefficients;
            private readonly GridView _grid;
            private readonly ScottPlot.Eto.EtoPlot _curvePlot;
            private readonly ScottPlot.Eto.EtoPlot _octavePlot;
            private readonly GroupBox _selectedLayerBox;

            private int _runGeneration;
            private CancellationTokenSource _runCts;
            private Thread _runThread;

            public IReadOnlyDictionary<string, LayerFitResult> ResultsByLayerName =>
                _rows.Where(r => r.Result != null)
                     .ToDictionary(r => r.LayerName, r => r.Result);

            public Pach_Absorption_Filter_Fitter(
                IList<Environment.Material> materials,
                IList<string> layerNames)
            {
                if (materials == null || layerNames == null)
                    throw new ArgumentNullException("materials/layerNames cannot be null.");
                if (materials.Count != layerNames.Count)
                    throw new ArgumentException("materials and layerNames must have equal length.");
                if (materials.Count == 0)
                    throw new ArgumentException("At least one material is required.");

                _materials = new List<Environment.Material>(materials);
                _rows = new List<LayerFitRow>();

                for (int i = 0; i < _materials.Count; i++)
                {
                    double[] broadband = _materials[i].Coefficient_A_Broad();
                    _rows.Add(new LayerFitRow
                    {
                        Index = i,
                        LayerName = string.IsNullOrWhiteSpace(layerNames[i]) ? $"Layer_{i + 1}" : layerNames[i],
                        MaterialType = _materials[i].GetType().Name,
                        Status = "Pending",
                        IsCurrent = false,
                        Success = false,
                        TargetOctaves = broadband,
                        PerLayerOrder = -1
                    });
                }

                Title = "Absorption Filter Fitter";
                ClientSize = new Eto.Drawing.Size(1500, 1000);
                Resizable = true;

                _filterOrder = new NumericStepper
                {
                    MinValue = 0,
                    MaxValue = 16,
                    Increment = 1,
                    DecimalPlaces = 0,
                    Value = 0,
                    ToolTip = "Global filter order (0 = auto-select). Applied to all layers unless overridden per-layer."
                };

                _maxFrequency = new NumericStepper
                {
                    MinValue = 63,
                    MaxValue = 20000,
                    Increment = 125,
                    DecimalPlaces = 0,
                    Value = 10000,
                    ToolTip = "Upper frequency limit for the IIR fit."
                };

                _selectedLayerOrder = new NumericStepper
                {
                    MinValue = -1,
                    MaxValue = 16,
                    Increment = 1,
                    DecimalPlaces = 0,
                    Value = -1,
                    ToolTip = "Per-layer filter order override. -1 = use global order."
                };

                _evaluateAll = new Button { Text = "Evaluate All Layers" };
                _evaluateSelected = new Button { Text = "Re-evaluate Selected", Enabled = false };
                _accept = new Button { Text = "Accept && Save to Materials", Enabled = false };
                _close = new Button { Text = "Close" };

                _status = new Eto.Forms.Label { Text = "Ready. Select a layer to inspect or evaluate individually." };
                _metrics = new Eto.Forms.Label { Text = string.Empty };

                _coefficients = new TextArea
                {
                    ReadOnly = true,
                    Wrap = false,
                    Size = new Eto.Drawing.Size(-1, 160)
                };

                _grid = BuildGrid();

                _curvePlot = new ScottPlot.Eto.EtoPlot();
                _curvePlot.Size = new Eto.Drawing.Size(-1, 320);
                _curvePlot.Plot.Title("Absorption Curve Comparison", 12);
                _curvePlot.Plot.XLabel("Frequency (Hz.)", 10);
                _curvePlot.Plot.YLabel("Absorption Coefficient", 10);

                _octavePlot = new ScottPlot.Eto.EtoPlot();
                _octavePlot.Size = new Eto.Drawing.Size(-1, 240);
                _octavePlot.Plot.Title("Octave-Band Comparison", 12);
                _octavePlot.Plot.XLabel("Frequency (Hz.)", 10);
                _octavePlot.Plot.YLabel("Absorption Coefficient", 10);

                InitPlotAxes(_curvePlot);
                InitPlotAxes(_octavePlot);

                _evaluateAll.Click += EvaluateAll_Click;
                _evaluateSelected.Click += EvaluateSelected_Click;
                _accept.Click += Accept_Click;
                _close.Click += (s, e) => Close();

                _filterOrder.ValueChanged += SettingsChanged;
                _maxFrequency.ValueChanged += SettingsChanged;
                _selectedLayerOrder.ValueChanged += SelectedLayerOrderChanged;
                _grid.SelectedRowsChanged += GridSelectionChanged;

                DynamicLayout controls = new DynamicLayout();
                controls.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                controls.Padding = 8;
                controls.AddRow(
                    new Eto.Forms.Label { Text = "Global Filter Order (0 = auto):" }, _filterOrder,
                    new Eto.Forms.Label { Text = "Max Frequency (Hz.):" }, _maxFrequency,
                    _evaluateAll, _accept, _close);
                controls.AddRow(_status);
                controls.AddRow(_metrics);

                DynamicLayout selectedControls = new DynamicLayout();
                selectedControls.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                selectedControls.Padding = 4;
                selectedControls.AddRow(
                    new Eto.Forms.Label { Text = "Layer Filter Order Override (-1 = global):" },
                    _selectedLayerOrder,
                    _evaluateSelected);

                _selectedLayerBox = new GroupBox { Text = "Selected Layer Settings", Content = selectedControls };
                _selectedLayerBox.Visible = false;

                GroupBox rowBox = new GroupBox { Text = "Layer Fit Status" };
                rowBox.Content = _grid;

                GroupBox coeffBox = new GroupBox { Text = "Selected Layer IIR Coefficients" };
                coeffBox.Content = _coefficients;

                DynamicLayout root = new DynamicLayout();
                root.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                root.Padding = 8;
                root.AddRow(controls);
                root.AddRow(rowBox);
                root.AddRow(_selectedLayerBox);
                root.AddRow(_curvePlot);
                root.AddRow(_octavePlot);
                root.AddRow(coeffBox);

                Content = root;
            }

            private static void InitPlotAxes(ScottPlot.Eto.EtoPlot plot)
            {
                Tick[] ticks = new Tick[12]
                {
                      new Tick(1,  "16"),  new Tick(2,  "31"),  new Tick(3,  "63"),  new Tick(4,  "125"),
                      new Tick(5,  "250"), new Tick(6,  "500"), new Tick(7,  "1k"),  new Tick(8,  "2k"),
                      new Tick(9,  "4k"),  new Tick(10, "8k"),  new Tick(11, "16k"), new Tick(12, "32k")
                };
                plot.Plot.Axes.Bottom.TickGenerator = new NumericManual(ticks);
                plot.Plot.Axes.SetLimits(2.5, 10.5, 0, 1.05);
            }

            private GridView BuildGrid()
            {
                GridView g = new GridView();
                g.AllowMultipleSelection = false;
                g.Height = 200;

                g.Columns.Add(new GridColumn
                {
                    HeaderText = "Layer",
                    DataCell = new TextBoxCell { Binding = Binding.Property<LayerFitRow, string>(r => r.LayerName) },
                    AutoSize = true,
                    Width = 200
                });

                g.Columns.Add(new GridColumn
                {
                    HeaderText = "Material Type",
                    DataCell = new TextBoxCell { Binding = Binding.Property<LayerFitRow, string>(r => r.MaterialType) },
                    AutoSize = true
                });

                string[] octaveLabels = { "α 63", "α 125", "α 250", "α 500", "α 1k", "α 2k", "α 4k", "α 8k" };
                for (int oct = 0; oct < 8; oct++)
                {
                    int octIdx = oct;
                    g.Columns.Add(new GridColumn
                    {
                        HeaderText = octaveLabels[oct],
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<LayerFitRow, string>(r =>
                                r.TargetOctaves != null && r.TargetOctaves.Length > octIdx
                                    ? r.TargetOctaves[octIdx].ToString("F2") : "-")
                        },
                        Width = 52
                    });
                }

                // Order: shows fitted order once evaluated, otherwise pending override or "auto"
                g.Columns.Add(new GridColumn
                {
                    HeaderText = "Order",
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding.Property<LayerFitRow, string>(r =>
                            r.Result != null ? r.Result.FittedOrder.ToString() :
                            r.PerLayerOrder >= 0 ? r.PerLayerOrder.ToString() :
                                                      "auto")
                    },
                    Width = 52
                });

                g.Columns.Add(new GridColumn
                {
                    HeaderText = "Status",
                    DataCell = new TextBoxCell { Binding = Binding.Property<LayerFitRow, string>(r => r.Status) },
                    AutoSize = true
                });

                g.Columns.Add(new GridColumn
                {
                    HeaderText = "Mean |Δα| 125–4k",
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding.Property<LayerFitRow, string>(r =>
                            r.Result == null ? "-" : r.Result.MeanAbsError.ToString("F4"))
                    },
                    Width = 120
                });

                g.Columns.Add(new GridColumn
                {
                    HeaderText = "Max |Δα|",
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding.Property<LayerFitRow, string>(r =>
                            r.Result == null ? "-" : r.Result.MaxAbsError.ToString("F4"))
                    },
                    Width = 80
                });

                g.DataStore = _rows;
                return g;
            }

            private void SettingsChanged(object sender, EventArgs e)
            {
                foreach (LayerFitRow row in _rows)
                {
                    row.IsCurrent = false;
                    row.Success = false;
                    row.Status = "Pending";
                    row.Result = null;
                    _grid.ReloadData(row.Index);
                }

                _accept.Enabled = false;
                _coefficients.Text = string.Empty;
                _metrics.Text = string.Empty;
                _status.Text = $"Settings changed (Global Order={(int)_filterOrder.Value}, MaxFreq={_maxFrequency.Value:F0} Hz). Re-evaluate layers.";
                ClearPlots();
            }

            private void SelectedLayerOrderChanged(object sender, EventArgs e)
            {
                int rowIndex = _grid.SelectedRow;
                if (rowIndex < 0 || rowIndex >= _rows.Count) return;

                _rows[rowIndex].PerLayerOrder = (int)_selectedLayerOrder.Value;
                _grid.ReloadData(rowIndex);
            }

            private void ClearPlots()
            {
                double maxFreq = (double)_maxFrequency.Value;
                double xMax = Math.Log(maxFreq / 7.8125, 2) + 0.3;

                _curvePlot.Plot.Clear();
                _curvePlot.Plot.Title("Absorption Curve Comparison", 12);
                _curvePlot.Plot.XLabel("Frequency (Hz.)", 10);
                _curvePlot.Plot.YLabel("Absorption Coefficient", 10);
                _curvePlot.Plot.Axes.SetLimits(2.5, xMax, 0, 1.05);
                _curvePlot.Refresh();

                _octavePlot.Plot.Clear();
                _octavePlot.Plot.Title("Octave-Band Comparison", 12);
                _octavePlot.Plot.XLabel("Frequency (Hz.)", 10);
                _octavePlot.Plot.YLabel("Absorption Coefficient", 10);
                _octavePlot.Plot.Axes.SetLimits(2.5, xMax, 0, 1.05);
                _octavePlot.Refresh();
            }

            private async void EvaluateAll_Click(object sender, EventArgs e)
            {
                int myGeneration = ++_runGeneration;
                _runCts?.Cancel();
                _runCts = new CancellationTokenSource();
                CancellationToken token = _runCts.Token;

                _evaluateAll.Enabled = false;
                _evaluateSelected.Enabled = false;
                _accept.Enabled = false;
                _status.Text = "Evaluating all layers...";

                int globalOrder = (int)_filterOrder.Value;
                double maxFreq = (double)_maxFrequency.Value;

                _runThread = new Thread(() =>
                {
                    for (int i = 0; i < _materials.Count; i++)
                    {
                        if (token.IsCancellationRequested) return;

                        int rowIndex = i;
                        int effectiveOrder = _rows[rowIndex].PerLayerOrder >= 0
                            ? _rows[rowIndex].PerLayerOrder
                            : globalOrder;

                        LayerFitResult result;
                        try { result = EvaluateMaterial(_materials[rowIndex], effectiveOrder, maxFreq); }
                        catch (Exception ex) { result = new LayerFitResult { ErrorMessage = ex.Message }; }

                        Application.Instance.AsyncInvoke(() =>
                        {
                            if (myGeneration != _runGeneration) return;

                            LayerFitRow row = _rows[rowIndex];
                            row.IsCurrent = true;
                            row.Result = result;
                            row.Success = result != null && string.IsNullOrEmpty(result.ErrorMessage);
                            row.Status = row.Success ? "OK" : $"Failed: {result?.ErrorMessage ?? "Unknown"}";
                            _grid.ReloadData(rowIndex);

                            UpdateApplyState();
                            _status.Text = $"Evaluated {rowIndex + 1}/{_materials.Count}: {row.LayerName}";
                        });
                    }
                });

                _runThread.IsBackground = true;
                _runThread.Start();

                while (_runThread.IsAlive)
                    await Task.Delay(50);

                if (myGeneration != _runGeneration) return;

                _evaluateAll.Enabled = true;
                _evaluateSelected.Enabled = _grid.SelectedRow >= 0;
                UpdateApplyState();

                // Refresh display for whatever row is currently selected
                int sel = _grid.SelectedRow;
                if (sel >= 0 && sel < _rows.Count && _rows[sel].Result != null)
                    RefreshSelectedDisplay(sel);

                _status.Text = _accept.Enabled
                    ? "All layers fitted. Review plots, then Accept & Save to Materials."
                    : "Some layers failed — select a failed layer and Re-evaluate with a different order.";
            }

            private async void EvaluateSelected_Click(object sender, EventArgs e)
            {
                int rowIndex = _grid.SelectedRow;
                if (rowIndex < 0 || rowIndex >= _rows.Count) return;

                int myGeneration = ++_runGeneration;
                _runCts?.Cancel();
                _runCts = new CancellationTokenSource();

                _evaluateAll.Enabled = false;
                _evaluateSelected.Enabled = false;
                _status.Text = $"Evaluating {_rows[rowIndex].LayerName}...";

                int effectiveOrder = _rows[rowIndex].PerLayerOrder >= 0
                    ? _rows[rowIndex].PerLayerOrder
                    : (int)_filterOrder.Value;
                double maxFreq = (double)_maxFrequency.Value;

                LayerFitResult result = null;
                await Task.Run(() =>
                {
                    try { result = EvaluateMaterial(_materials[rowIndex], effectiveOrder, maxFreq); }
                    catch (Exception ex) { result = new LayerFitResult { ErrorMessage = ex.Message }; }
                });

                if (myGeneration != _runGeneration) return;

                LayerFitRow row = _rows[rowIndex];
                row.IsCurrent = true;
                row.Result = result;
                row.Success = result != null && string.IsNullOrEmpty(result.ErrorMessage);
                row.Status = row.Success ? "OK" : $"Failed: {result?.ErrorMessage ?? "Unknown"}";
                _grid.ReloadData(rowIndex);

                if (row.Result != null)
                    RefreshSelectedDisplay(rowIndex);

                _evaluateAll.Enabled = true;
                _evaluateSelected.Enabled = true;
                UpdateApplyState();
                _status.Text = row.Success
                    ? $"{row.LayerName}: OK  order={row.Result.FittedOrder}  mean Δα={row.Result.MeanAbsError:F4}"
                    : $"{row.LayerName}: Failed — try a different order.";
            }

            private void RefreshSelectedDisplay(int rowIndex)
            {
                LayerFitRow row = _rows[rowIndex];
                if (row.Result == null) return;
                PopulatePlots(row.Result);
                _coefficients.Text = row.Result.CoefficientText;
                _metrics.Text = $"{row.LayerName}  |  Order: {row.Result.FittedOrder}  |  Mean |Δα| 125–4k: {row.Result.MeanAbsError:F4}   Max |Δα|: {row.Result.MaxAbsError:F4}";
            }

            private void UpdateApplyState()
            {
                _accept.Enabled = _rows.All(r => r.IsCurrent && r.Success && r.Result != null);
            }

            private void GridSelectionChanged(object sender, EventArgs e)
            {
                int rowIndex = _grid.SelectedRow;
                if (rowIndex < 0 || rowIndex >= _rows.Count)
                {
                    _selectedLayerBox.Visible = false;
                    _evaluateSelected.Enabled = false;
                    return;
                }

                LayerFitRow row = _rows[rowIndex];
                _selectedLayerOrder.Value = row.PerLayerOrder;
                _selectedLayerBox.Text = $"Selected Layer: {row.LayerName}  ({row.MaterialType})";
                _selectedLayerBox.Visible = true;
                _evaluateSelected.Enabled = true;

                if (row.Result != null)
                    RefreshSelectedDisplay(rowIndex);
                else
                {
                    ClearPlots();
                    _coefficients.Text = string.Empty;
                    _metrics.Text = $"{row.LayerName} — not yet evaluated. Click 'Re-evaluate Selected' to fit this layer.";
                }
            }

            private void PopulatePlots(LayerFitResult result)
            {
                // Autoscale X to the actual data range
                double xMin = result.LogFrequencies.First() - 0.3;
                double xMax = result.LogFrequencies.Last() + 0.3;

                // Octave plot: only show the centers that fall within the fit range
                double[] validCenters = result.OctaveCenters
                    .Where(c => c >= xMin && c <= xMax).ToArray();
                double oxMin = validCenters.Length > 0 ? validCenters.First() - 0.6 : xMin;
                double oxMax = validCenters.Length > 0 ? validCenters.Last() + 0.6 : xMax;

                _curvePlot.Plot.Clear();
                _curvePlot.Plot.Title("Absorption Curve Comparison", 12);
                _curvePlot.Plot.XLabel("Frequency (Hz.)", 10);
                _curvePlot.Plot.YLabel("Absorption Coefficient", 10);

                _curvePlot.Plot.Add.Scatter(result.LogFrequencies, result.TargetAlpha, Colors.Blue);
                (_curvePlot.Plot.PlottableList.Last() as Scatter).MarkerStyle = MarkerStyle.None;

                _curvePlot.Plot.Add.Scatter(result.LogFrequencies, result.FitAlpha, Colors.Green);
                (_curvePlot.Plot.PlottableList.Last() as Scatter).MarkerStyle = MarkerStyle.None;

                _curvePlot.Plot.Legend.ManualItems.Clear();
                _curvePlot.Plot.Legend.ManualItems.Add(new LegendItem
                {
                    LabelText = "Target Absorption",
                    LineStyle = new LineStyle(1, Colors.Blue, LinePattern.Solid),
                    MarkerStyle = MarkerStyle.None
                });
                _curvePlot.Plot.Legend.ManualItems.Add(new LegendItem
                {
                    LabelText = "IIR Fit",
                    LineStyle = new LineStyle(1, Colors.Green, LinePattern.Solid),
                    MarkerStyle = MarkerStyle.None
                });
                _curvePlot.Plot.Legend.IsVisible = true;
                _curvePlot.Plot.Axes.SetLimits(xMin, xMax, 0, 1.05);
                _curvePlot.Refresh();

                _octavePlot.Plot.Clear();
                _octavePlot.Plot.Title("Octave-Band Comparison", 12);
                _octavePlot.Plot.XLabel("Frequency (Hz.)", 10);
                _octavePlot.Plot.YLabel("Absorption Coefficient", 10);

                _octavePlot.Plot.Add.Scatter(result.OctaveCenters, result.OctaveTarget, Colors.Red);
                (_octavePlot.Plot.PlottableList.Last() as Scatter).MarkerStyle.Shape = MarkerShape.FilledCircle;
                (_octavePlot.Plot.PlottableList.Last() as Scatter).LineStyle.Width = 0;

                _octavePlot.Plot.Add.Scatter(result.OctaveCenters, result.OctaveFit, Colors.Orange);
                (_octavePlot.Plot.PlottableList.Last() as Scatter).MarkerStyle.Shape = MarkerShape.FilledSquare;
                (_octavePlot.Plot.PlottableList.Last() as Scatter).LineStyle.Width = 0;

                _octavePlot.Plot.Legend.ManualItems.Clear();
                _octavePlot.Plot.Legend.ManualItems.Add(new LegendItem
                {
                    LabelText = "Target Octave Average",
                    MarkerStyle = new MarkerStyle(MarkerShape.FilledCircle, 6, Colors.Red),
                    LineStyle = LineStyle.None
                });
                _octavePlot.Plot.Legend.ManualItems.Add(new LegendItem
                {
                    LabelText = "IIR Octave Average",
                    MarkerStyle = new MarkerStyle(MarkerShape.FilledSquare, 6, Colors.Orange),
                    LineStyle = LineStyle.None
                });
                _octavePlot.Plot.Legend.IsVisible = true;
                _octavePlot.Plot.Axes.SetLimits(oxMin, oxMax, 0, 1.05);
                _octavePlot.Refresh();
            }

            public bool Accepted { get; private set; }

            public void SetFitParameters(int filterOrder, double maxFrequency)
            {
                _filterOrder.Value = filterOrder;
                _maxFrequency.Value = maxFrequency;
            }

            public async Task<bool> EnsureFitsAsync()
            {
                EvaluateAll_Click(this, EventArgs.Empty);

                while (_runThread != null && _runThread.IsAlive)
                    await Task.Delay(50);

                if (_rows.All(r => r.IsCurrent && r.Success && r.Result != null))
                {
                    // Force coefficients into materials even in headless path
                    for (int i = 0; i < _rows.Count; i++)
                        _materials[i].ForceIIR(_rows[i].Result.A, _rows[i].Result.B, _maxFrequency.Value);

                    Accepted = true;
                    return true;
                }

                Accepted = false;
                return false;
            }

            private void Accept_Click(object sender, EventArgs e)
            {
                if (!_rows.All(r => r.IsCurrent && r.Success && r.Result != null))
                {
                    MessageBox.Show("All layers must have valid coefficients before accepting.\n\nSelect any failed layer and use 'Re-evaluate Selected' with a different order.");
                    return;
                }

                // Lock fitted coefficients into each material instance so the FDTD
                // gets exactly these values regardless of its own call parameters.
                for (int i = 0; i < _rows.Count; i++)
                    _materials[i].ForceIIR(_rows[i].Result.A, _rows[i].Result.B, _maxFrequency.Value);

                Accepted = true;
                _status.Text = "Coefficients accepted and saved to materials.";
                Close();
            }

            private static LayerFitResult EvaluateMaterial(Environment.Material material, int order, double maxFreq)
            {
                const double fs = 44100.0;
                Hare.Geometry.Vector normal = new Hare.Geometry.Vector(0, 0, 1);
                Hare.Geometry.Vector incident = new Hare.Geometry.Vector(0, 0, 1);

                double[] frequencies;
                (double[] a, double[] b) = material.Estimate_IIR_Coefficients(fs, maxFreq, out frequencies, order);

                if (a == null || b == null || frequencies == null || frequencies.Length < 2)
                    return new LayerFitResult { ErrorMessage = "IIR coefficient estimation returned no data." };

                int fittedOrder = Math.Max(a.Length, b.Length) - 1;

                double[] targetAlpha = new double[frequencies.Length];
                for (int i = 0; i < frequencies.Length; i++)
                {
                    Complex r = material.Reflection_Narrow(frequencies[i], incident, normal);
                    double mag2 = r.Real * r.Real + r.Imaginary * r.Imaginary;
                    targetAlpha[i] = Math.Max(0, Math.Min(1, 1.0 - mag2));
                }

                Complex[] fitY = Pach_SP.IIR_Design.AB_FreqResponse(new List<double>(b), new List<double>(a), frequencies, fs);
                double[] fitAlpha = new double[frequencies.Length];
                for (int i = 0; i < frequencies.Length; i++)
                {
                    Complex denom = Complex.One + fitY[i];
                    if (denom.Magnitude < 1e-12) denom = new Complex(1e-12, 0);
                    Complex fitR = (Complex.One - fitY[i]) / denom;
                    double mag2 = fitR.Real * fitR.Real + fitR.Imaginary * fitR.Imaginary;
                    fitAlpha[i] = Math.Max(0, Math.Min(1, 1.0 - mag2));
                }

                int start = 0;
                while (start < frequencies.Length && frequencies[start] < 1.0) start++;

                double[] plotFreq = frequencies.Skip(start).ToArray();
                double[] plotTarget = targetAlpha.Skip(start).ToArray();
                double[] plotFit = fitAlpha.Skip(start).ToArray();
                double[] plotLogFreq = plotFreq.Select(f => Math.Log(f / 7.8125, 2)).ToArray();

                double[] octaveCenters = new double[8];
                double[] octaveTarget = new double[8];
                double[] octaveFit = new double[8];
                double root2 = Math.Sqrt(2.0);

                for (int oct = 0; oct < 8; oct++)
                {
                    double center = 62.5 * Math.Pow(2, oct);
                    double fLo = center / root2;
                    double fHi = center * root2;
                    octaveCenters[oct] = Math.Log(center / 7.8125, 2);

                    List<double> tBand = new List<double>();
                    List<double> fBand = new List<double>();
                    for (int i = 0; i < frequencies.Length; i++)
                    {
                        if (frequencies[i] < fLo || frequencies[i] > fHi) continue;
                        tBand.Add(targetAlpha[i]);
                        fBand.Add(fitAlpha[i]);
                    }
                    octaveTarget[oct] = tBand.Count > 0 ? tBand.Average() : 0;
                    octaveFit[oct] = fBand.Count > 0 ? fBand.Average() : 0;
                }

                List<double> errBand = new List<double>();
                for (int i = 0; i < plotFreq.Length; i++)
                {
                    if (plotFreq[i] < 125 || plotFreq[i] > 4000) continue;
                    errBand.Add(Math.Abs(plotTarget[i] - plotFit[i]));
                }

                double meanAbsError = errBand.Count > 0 ? errBand.Average() : 0;
                double maxAbsError = errBand.Count > 0 ? errBand.Max() : 0;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"// Order: {fittedOrder}  (a: {a.Length} coeffs, b: {b.Length} coeffs)");
                sb.AppendLine("// a (denominator)");
                sb.AppendLine(string.Join(", ", a.Select(v => v.ToString("G8"))));
                sb.AppendLine();
                sb.AppendLine("// b (numerator)");
                sb.AppendLine(string.Join(", ", b.Select(v => v.ToString("G8"))));

                return new LayerFitResult
                {
                    FittedOrder = fittedOrder,
                    Frequencies = plotFreq,
                    LogFrequencies = plotLogFreq,
                    TargetAlpha = plotTarget,
                    FitAlpha = plotFit,
                    OctaveCenters = octaveCenters,
                    OctaveTarget = octaveTarget,
                    OctaveFit = octaveFit,
                    MeanAbsError = meanAbsError,
                    MaxAbsError = maxAbsError,
                    CoefficientText = sb.ToString(),
                    A = a,
                    B = b
                };
            }

            private sealed class LayerFitRow
            {
                public int Index;
                public string LayerName;
                public string MaterialType;
                public string Status;
                public bool IsCurrent;
                public bool Success;
                public double[] TargetOctaves;
                public int PerLayerOrder;
                public LayerFitResult Result;
            }

            public sealed class LayerFitResult
            {
                public int FittedOrder;
                public double[] Frequencies;
                public double[] LogFrequencies;
                public double[] TargetAlpha;
                public double[] FitAlpha;
                public double[] OctaveCenters;
                public double[] OctaveTarget;
                public double[] OctaveFit;
                public double MeanAbsError;
                public double MaxAbsError;
                public string CoefficientText;
                public string ErrorMessage;
                public double[] A;
                public double[] B;
            }
        }

        public class Pach_IIR_Fit_Evaluator : Pach_Absorption_Filter_Fitter
        {
            public Pach_IIR_Fit_Evaluator(Environment.Material material)
                : base(new List<Environment.Material> { material }, new List<string> { "Layer_1" })
            {
            }
        }
    }
}