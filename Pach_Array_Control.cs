using Eto.Drawing;
using Eto.Forms;
using MathNet.Numerics.LinearAlgebra;
using Pachyderm_Acoustic.Utilities;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using ScottPlot.Triangulation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rhino.UI;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public class Pach_ArrayControl : Form
        {
            private readonly List<RhinoObject> Elements;

            private DynamicLayout ElementLayout;
            private Scrollable ElementScroll;
            private ArrayPowerControl ArrayPower;

            private bool LoadingGroup = false;
            private DropDown ArrayControlMode;
            private GroupBox ArrayOrientation;
            private NumericStepper ArrayAlt, ArrayAzi, ArrayAxial;
            private bool ColumnMode { get { return ArrayControlMode != null && ArrayControlMode.SelectedIndex == 1; } }

            private CheckBox ShowPattern;
            private DropDown PatternOctave;
            private RadioButton ThirdBands;
            private bool ThirdOctave { get { return ThirdBands != null && ThirdBands.Checked == true; } }
            private double[] BandFrequencies { get { return ThirdOctave ? AcousticalMath.ThirdOctaveCenters() : AcousticalMath.OctaveCenters(); } }
            private string BandKey { get { return ThirdOctave ? "ThirdOctave" : "Octave"; } }
            private NumericStepper PatternReferenceDistance;
            private CheckBox ShowArrayGraph;
            private CheckBox ShowArraySlice;
            private DropDown DiagnosticPlaneSelect;
            private NumericStepper DiagnosticAngle;
            private Label DiagnosticLevel;
            private DropDown PatternCenterMode;

            private SpeakerPatternConduit PatternConduit;

            public Pach_ArrayControl(List<RhinoObject> elements)
            {
                if (elements == null || elements.Count == 0)
                    throw new ArgumentException("Select at least one array element.", nameof(elements));
                Elements = elements;

                Title = "Pachyderm Speaker Array Control";
                ClientSize = new Size(900, 680);
                MinimumSize = new Size(780, 580);

                PatternConduit = SpeakerPatternConduit.Instance;
                PatternConduit.Mode = SpeakerPatternConduit.Display_Mode.Boundary_Contours;
                PatternConduit.Contour_Levels = new double[] { -1, -2, -3, -4, -5, -6, -12, -18 };
                PatternConduit.Contour_Mesh_Max_Edge = 2.0;
                PatternConduit.Enabled = false;

                DynamicLayout layout = new DynamicLayout();
                layout.Padding = 8;
                layout.DefaultSpacing = new Size(6, 6);

                string label = "Steerable Array " + Elements[0].Geometry.GetUserString("ArrayGroupLabel");

                ArrayControlMode = new DropDown { Width = 140 };
                ArrayControlMode.Items.Add("Line array");
                ArrayControlMode.Items.Add("Column array");
                string controlMode = Elements[0].Geometry.GetUserString("ArrayControlMode") ?? Elements[0].Geometry.GetUserString("ArrayDeviceType");
                ArrayControlMode.SelectedIndex = controlMode == "Column" ? 1 : 0;
                layout.AddSeparateRow(new Label { Text = label, Font = new Eto.Drawing.Font(SystemFont.Bold, 12) },
                    null, new Label { Text = "Array control" }, ArrayControlMode);

                ArrayAlt = new NumericStepper { MinValue = -90, MaxValue = 90, DecimalPlaces = 1, Increment = 1, Width = 80 };
                ArrayAzi = new NumericStepper { MinValue = -360, MaxValue = 360, DecimalPlaces = 1, Increment = 1, Width = 80 };
                ArrayAxial = new NumericStepper { MinValue = -360, MaxValue = 360, DecimalPlaces = 1, Increment = 1, Width = 80 };
                DynamicLayout orientation = new DynamicLayout { Padding = 8, DefaultSpacing = new Size(6, 6) };
                DynamicLayout orientationAngles = new DynamicLayout { DefaultSpacing = new Size(6, 6) };
                orientationAngles.AddRow(new Label { Text = "Altitude" }, ArrayAlt, new Label { Text = "Azimuth" }, ArrayAzi,
                    new Label { Text = "Axial rotation" }, ArrayAxial, null);
                orientation.AddRow(orientationAngles);
                orientation.AddRow(new Label { Text = "Rotates all elements together, preserving spacing and splay.", Wrap = WrapMode.Word });
                ArrayOrientation = new GroupBox { Text = "Global array orientation", Content = orientation, ToolTip = "Angles use the first element as the orientation reference. Changing them rotates every element together. Column mode locks individual aiming." };
                layout.AddRow(ArrayOrientation);
                ArrayAlt.ValueChanged += OnArrayOrientationChanged;
                ArrayAzi.ValueChanged += OnArrayOrientationChanged;
                ArrayAxial.ValueChanged += OnArrayOrientationChanged;
                ArrayControlMode.SelectedIndexChanged += (s, e) =>
                {
                    if (LoadingGroup) return;
                    RhinoDoc doc = Elements[0].Document ?? RhinoDoc.ActiveDoc;
                    if (doc == null) return;
                    uint undo = doc.BeginUndoRecord("Change array control mode");
                    try
                    {
                        for (int i = 0; i < Elements.Count; i++)
                        {
                            RhinoObject current = doc.Objects.FindId(Elements[i].Id);
                            if (current == null || current.Geometry == null) continue;
                            current.Geometry.SetUserString("ArrayControlMode", ColumnMode ? "Column" : "Line");
                            current.CommitChanges();
                            Elements[i] = doc.Objects.FindId(current.Id) ?? current;
                        }
                    }
                    finally { if (undo != 0) doc.EndUndoRecord(undo); }
                    RebuildElementEditors();
                    ArrayPower?.SetBandResolution(ThirdOctave, true);
                    UpdatePatternConduit();
                    doc.Views.Redraw();
                };

                //Add Pattern Controls
                GroupBox box = new GroupBox { Text = "Array preview and steering" };

                DynamicLayout l = new DynamicLayout();
                l.Padding = 8;
                l.DefaultSpacing = new Size(4, 6);

                ShowPattern = new CheckBox { Text = "Show array preview and wall contours" };
                ShowPattern.Checked = true;
                ShowPattern.CheckedChanged += (s, e) => UpdatePatternConduit();

                PatternOctave = new DropDown { Width = 140 };
                bool thirdOctave = Elements.Exists(obj => obj != null && obj.Geometry.GetUserString("ArrayBandResolution") == "ThirdOctave");
                RadioButton octaveBands = new RadioButton { Text = "Octave", Checked = !thirdOctave };
                ThirdBands = new RadioButton(octaveBands) { Text = "Third octave", Checked = thirdOctave };
                foreach (double f in BandFrequencies) PatternOctave.Items.Add(f.ToString("0.##") + " Hz");
                PatternOctave.SelectedIndex = ThirdOctave ? 13 : 4;
                ThirdBands.CheckedChanged += (s, e) =>
                {
                    PatternOctave.Items.Clear();
                    foreach (double f in BandFrequencies) PatternOctave.Items.Add(f.ToString("0.##") + " Hz");
                    PatternOctave.SelectedIndex = ThirdOctave ? 13 : 4;
                    foreach (RhinoObject element in Elements)
                    {
                        RhinoObject obj = RhinoDoc.ActiveDoc.Objects.FindId(element.Id);
                        if (obj == null) continue;
                        obj.Geometry.SetUserString("ArrayBandResolution", BandKey);
                        obj.CommitChanges();
                    }
                    RebuildElementEditors();
                    ArrayPower?.SetBandResolution(ThirdOctave);
                    UpdatePatternConduit();
                };
                PatternOctave.SelectedIndexChanged += (s, e) => UpdatePatternConduit();

                PatternReferenceDistance = new NumericStepper();
                PatternReferenceDistance.MinValue = 1;
                PatternReferenceDistance.MaxValue = 200;
                PatternReferenceDistance.DecimalPlaces = 2;
                PatternReferenceDistance.Width = 80;
                PatternReferenceDistance.Value = ArraySimulationSettings.ReferenceDistance(Elements[0]);
                PatternReferenceDistance.ValueChanged += (s, e) =>
                {
                    foreach (RhinoObject element in Elements)
                    {
                        if (element == null || element.Geometry == null) continue;
                        element.Geometry.SetUserString(ArraySimulationSettings.DistanceKey, PatternReferenceDistance.Value.ToString(CultureInfo.InvariantCulture));
                        element.CommitChanges();
                    }
                    UpdatePatternConduit();
                };

                PatternCenterMode = new DropDown { Width = 230 };
                PatternCenterMode.Items.Add("Geometric Average");
                PatternCenterMode.Items.Add("Driver Apparent Center");
                PatternCenterMode.SelectedIndex = ArraySimulationSettings.UseAimingCenter(Elements[0]) ? 1 : 0;

                PatternCenterMode.SelectedIndexChanged += (s, e) =>
                {
                    string mode = PatternCenterMode.SelectedIndex == 1 ? "Aiming" : "Geometric";

                    foreach (RhinoObject element in Elements)
                    {
                        if (element == null || element.Geometry == null) continue;

                        element.Geometry.SetUserString(
                            ArraySimulationSettings.CenterModeKey,
                            mode);

                        element.CommitChanges();
                    }

                    UpdatePatternConduit();
                };

                DynamicLayout simulationLayout = new DynamicLayout { Padding = 8, DefaultSpacing = new Size(6, 6) };
                simulationLayout.AddRow(new Label { Text = "Simulation" }, new Label { Text = "Hybrid array" }, null);
                simulationLayout.AddRow(new Label { Text = "Array center" }, PatternCenterMode, null);
                simulationLayout.AddRow(new Label { Text = "Reference distance" }, PatternReferenceDistance, null);
                GroupBox simulationBox = new GroupBox { Text = "Simulation settings", Content = simulationLayout,
                    ToolTip = "Direct sound and image sources use coherent element paths. Ray tracing uses the array balloon at the selected center and reference distance." };

                Button aimAtPoints = new Button();
                aimAtPoints.Text = "Aim at Points";
                aimAtPoints.Click += (s, e) => AimPhaseAtPoints(false);

                Button optimizeAtPoints = new Button();
                optimizeAtPoints.Text = "Optimize";
                optimizeAtPoints.Click += (s, e) => AimPhaseAtPoints(true);

                // Keep settings and preview rows in separate grids so their
                // column widths do not push unrelated controls outside the form.
                DynamicLayout bands = new DynamicLayout { Padding = 8, DefaultSpacing = new Size(6, 6) };
                bands.AddSeparateRow(octaveBands, ThirdBands, null);
                bands.AddSeparateRow(new Label { Text = "Preview frequency" }, PatternOctave, null);
                bands.Add(null, yscale: true);
                DynamicLayout settingsLayout = new DynamicLayout { DefaultSpacing = new Size(6, 6) };
                settingsLayout.AddRow(simulationBox, new GroupBox { Text = "Frequency bands", Content = bands });
                layout.AddRow(settingsLayout);

                ShowArrayGraph = new CheckBox { Text = "3D graph", Checked = PatternConduit.Show_Array_Graph };
                ShowArraySlice = new CheckBox { Text = "Polar slice", Checked = PatternConduit.Show_Array_Diagnostic_Slice };
                DiagnosticPlaneSelect = new DropDown { Width = 70 };
                DiagnosticPlaneSelect.Items.Add("XY");
                DiagnosticPlaneSelect.Items.Add("XZ");
                DiagnosticPlaneSelect.Items.Add("YZ");
                DiagnosticPlaneSelect.SelectedIndex = (int)PatternConduit.Array_Diagnostic_Plane;
                DiagnosticAngle = new NumericStepper { MinValue = -180, MaxValue = 180, DecimalPlaces = 2, Increment = 0.25, Width = 80, Value = PatternConduit.Array_Diagnostic_Angle };
                DiagnosticLevel = new Label { Text = "-- dB", Width = 90 };
                DiagnosticAngle.ToolTip = "XY: +X toward +Y; XZ: +X toward +Z; YZ: +Y toward +Z. Level is relative to the sampled balloon maximum.";
                ShowArrayGraph.CheckedChanged += (s, e) => UpdatePatternConduit();
                ShowArraySlice.CheckedChanged += (s, e) => UpdatePatternConduit();
                DiagnosticPlaneSelect.SelectedIndexChanged += (s, e) => UpdatePatternConduit();
                DiagnosticAngle.ValueChanged += (s, e) =>
                {
                    PatternConduit.Array_Diagnostic_Angle = DiagnosticAngle.Value;
                    double db = PatternConduit.ArrayDiagnosticLevel(DiagnosticAngle.Value);
                    DiagnosticLevel.Text = double.IsNaN(db) ? "-- dB" : db.ToString("0.00") + " dB";
                    Rhino.RhinoDoc.ActiveDoc?.Views.Redraw();
                };
                l.AddSeparateRow(ShowPattern, ShowArrayGraph, ShowArraySlice, null);
                l.AddSeparateRow(new Label { Text = "Slice plane" }, DiagnosticPlaneSelect,
                    new Label { Text = "Angle (°)" }, DiagnosticAngle, new Label { Text = "Level" }, DiagnosticLevel, null);
                l.AddSeparateRow(new Label { Text = "Automatic steering" },
                    aimAtPoints, optimizeAtPoints, null);

                box.Content = l;
                layout.AddRow(box);

                AddElementControls(layout);

                Button close = new Button { Text = "Close" };
                close.Click += (s, e) => Close();

                layout.AddSeparateRow(null, close);

                Content = layout;

                Closed += (s, e) =>
                {
                    if (PatternConduit != null)
                    {
                        PatternConduit.Clear();
                        PatternConduit.Enabled = false;
                    }
                };

                UpdatePatternConduit();
            }

            private List<Hare.Geometry.Point> GetTargetPoints()
            {
                List<Hare.Geometry.Point> targets = new List<Hare.Geometry.Point>();

                while (true)
                {
                    Rhino.Geometry.Point3d pt;
                    Rhino.Commands.Result rc = Rhino.Input.RhinoGet.GetPoint(targets.Count == 0 ? "Select target point" : "Select another target point. Press Enter when done.", true, out pt);

                    if (rc == Rhino.Commands.Result.Success)
                    {
                        targets.Add(Utilities.RCPachTools.ModelPointToHare(pt));
                        continue;
                    }

                    break;
                }

                return targets;
            }

            private void AddElementControls(DynamicLayout layout)
            {
                ElementScroll = new Scrollable();
                ElementScroll.Width = 500;

                RebuildElementEditors();
                ArrayPower = new ArrayPowerControl(Elements, ThirdOctave);
                ArrayPower.Changed += (sender, args) => UpdatePatternConduit();
                Scrollable powerScroll = new Scrollable { Content = ArrayPower, Width = 320 };
                DynamicLayout body = new DynamicLayout { DefaultSpacing = new Size(6, 6) };
                body.AddRow(new GroupBox { Text = "Overall band levels", Content = powerScroll }, ElementScroll);
                layout.Add(body, xscale: true, yscale: true);
            }

            private void AimPhaseAtPoints(bool fineTune)
            {
                if (Elements == null || Elements.Count == 0) return;

                RhinoDoc doc = RhinoDoc.ActiveDoc;

                if (doc == null) return;

                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i] == null) continue;

                    RhinoObject current =
                        doc.Objects.FindId(Elements[i].Id);

                    if (current != null)
                    {
                        Elements[i] = current;
                    }
                }

                List<Hare.Geometry.Point> targets = GetTargetPoints();
                if (targets.Count == 0) return;

                double c = 343.0;
                double[] seed_ms = new double[Elements.Count];

                for (int t = 0; t < targets.Count; t++)
                {
                    double[] r = new double[Elements.Count];
                    double r_ref = double.NegativeInfinity;

                    for (int i = 0; i < Elements.Count; i++)
                    {
                        Hare.Geometry.Point src = Utilities.RCPachTools.SourcePoint(Elements[i]); 
                        r[i] = (src - targets[t]).Length();
                        if (r[i] > r_ref) r_ref = r[i];
                    }

                    for (int i = 0; i < Elements.Count; i++)
                    {
                        seed_ms[i] += (r_ref - r[i]) / c * 1000.0;
                    }
                }

                for (int i = 0; i < seed_ms.Length; i++)
                {
                    seed_ms[i] /= targets.Count;
                }

                NormalizeDelays(seed_ms);

                double[][] delay_by_element = new double[Elements.Count][];
                double[][] phase_by_element = new double[Elements.Count][];
                double[][] gain_by_element = new double[Elements.Count][];

                for (int i = 0; i < Elements.Count; i++)
                {
                    delay_by_element[i] = new double[BandFrequencies.Length];
                    phase_by_element[i] = new double[BandFrequencies.Length];
                    gain_by_element[i] = new double[BandFrequencies.Length];
                }

                for (int oct = 0; oct < BandFrequencies.Length; oct++)
                {
                    double[] delay_ms = new double[seed_ms.Length];
                    Array.Copy(seed_ms, delay_ms, seed_ms.Length);

                    double[] gain_db;

                    if (fineTune)
                    {
                        delay_ms = OptimizeDelaysAndGainsForCleanLobe(targets, delay_ms, oct, out gain_db);
                    }
                    else
                    {
                        gain_db = new double[delay_ms.Length];
                    }

                    NormalizeDelays(delay_ms);
                    NormalizeGains(gain_db);

                    double f = BandFrequencies[oct];

                    for (int i = 0; i < Elements.Count; i++)
                    {
                        delay_by_element[i][oct] = delay_ms[i];
                        phase_by_element[i][oct] = delay_ms[i] / 1000.0 * f * 360.0;
                        gain_by_element[i][oct] = gain_db[i];
                    }
                }

                for (int i = 0; i < Elements.Count; i++)
                {
                    RhinoObject obj = Elements[i];

                    if (obj == null || obj.Geometry == null) continue;

                    obj.Geometry.SetUserString("ArrayPhase" + BandKey + "Deg", PachTools.EncodeSourcePower(phase_by_element[i]));
                    obj.Geometry.SetUserString("ArrayDelay" + BandKey + "Ms", PachTools.EncodeSourcePower(delay_by_element[i]));
                    obj.Geometry.SetUserString("ArrayGain" + BandKey + "Db", PachTools.EncodeSourcePower(gain_by_element[i]));
                    obj.CommitChanges();

                    EnsureSourceInConduit(obj);
                }

                RebuildElementEditors();
                UpdatePatternConduit();

                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private double[] OptimizeDelaysAndGainsForCleanLobe(List<Hare.Geometry.Point> targets, double[] seedDelayMs, int octave, out double[] gainDb)
            {
                double[] bestDelay = new double[seedDelayMs.Length];
                Array.Copy(seedDelayMs, bestDelay, seedDelayMs.Length);

                gainDb = new double[seedDelayMs.Length];

                NormalizeDelays(bestDelay);
                NormalizeGains(gainDb);

                double f = BandFrequencies[octave];

                double delayStepMs = 45.0 / 360.0 / f * 1000.0; // 45 degrees
                double gainStepDb = 1.0;

                double bestScore = ArrayPatternScore(targets, bestDelay, gainDb, octave);

                for (int pass = 0; pass < 10; pass++)
                {
                    bool improved = false;

                    // Delay pass.
                    for (int i = 0; i < bestDelay.Length; i++)
                    {
                        double original = bestDelay[i];

                        bestDelay[i] = original + delayStepMs;
                        NormalizeDelays(bestDelay);

                        double plusScore = ArrayPatternScore(targets, bestDelay, gainDb, octave);

                        if (plusScore > bestScore)
                        {
                            bestScore = plusScore;
                            improved = true;
                            continue;
                        }

                        bestDelay[i] = original - delayStepMs;
                        NormalizeDelays(bestDelay);

                        double minusScore = ArrayPatternScore(targets, bestDelay, gainDb, octave);

                        if (minusScore > bestScore)
                        {
                            bestScore = minusScore;
                            improved = true;
                            continue;
                        }

                        bestDelay[i] = original;
                        NormalizeDelays(bestDelay);
                    }

                    // Gain-shading pass.
                    for (int i = 0; i < gainDb.Length; i++)
                    {
                        double original = gainDb[i];

                        gainDb[i] = original + gainStepDb;
                        NormalizeGains(gainDb);

                        double plusScore = ArrayPatternScore(targets, bestDelay, gainDb, octave);

                        if (plusScore > bestScore)
                        {
                            bestScore = plusScore;
                            improved = true;
                            continue;
                        }

                        gainDb[i] = original - gainStepDb;
                        NormalizeGains(gainDb);

                        double minusScore = ArrayPatternScore(targets, bestDelay, gainDb, octave);

                        if (minusScore > bestScore)
                        {
                            bestScore = minusScore;
                            improved = true;
                            continue;
                        }

                        gainDb[i] = original;
                        NormalizeGains(gainDb);
                    }

                    if (!improved)
                    {
                        delayStepMs *= 0.5;
                        gainStepDb *= 0.5;
                    }

                    if (delayStepMs < 0.001 && gainStepDb < 0.05) break;
                }

                NormalizeDelays(bestDelay);
                NormalizeGains(gainDb);

                return bestDelay;
            }

            private static void NormalizeGains(double[] gain_db)
            {
                if (gain_db == null || gain_db.Length == 0) return;

                double max = double.NegativeInfinity;

                for (int i = 0; i < gain_db.Length; i++)
                {
                    if (gain_db[i] > max) max = gain_db[i];
                }

                if (double.IsNegativeInfinity(max)) return;

                for (int i = 0; i < gain_db.Length; i++)
                {
                    gain_db[i] -= max;
                    if (gain_db[i] > 0) gain_db[i] = 0;
                    if (gain_db[i] < -18) gain_db[i] = -18;
                }
            }

            private static void NormalizeDelays(double[] delay)
            {
                if (delay == null || delay.Length == 0) return;

                double min = double.PositiveInfinity;

                for (int i = 0; i < delay.Length; i++)
                {
                    if (delay[i] < min) min = delay[i];
                }

                if (double.IsInfinity(min)) return;

                for (int i = 0; i < delay.Length; i++)
                {
                    delay[i] -= min;
                }
            }

            private double ArrayPatternScore(List<Hare.Geometry.Point> targets, double[] delay_ms, double[] gain_db, int octave)
            {
                List<double> targetDb = new List<double>();

                for (int i = 0; i < targets.Count; i++)
                {
                    double mag = ArrayMagnitudeAtPoint(targets[i], delay_ms, gain_db, octave);
                    targetDb.Add(20.0 * Math.Log10(Math.Max(1E-12, mag)));
                }

                double meanTarget = 0;

                for (int i = 0; i < targetDb.Count; i++)
                {
                    meanTarget += targetDb[i];
                }

                meanTarget /= Math.Max(1, targetDb.Count);

                double minTarget = double.PositiveInfinity;
                double maxTarget = double.NegativeInfinity;

                for (int i = 0; i < targetDb.Count; i++)
                {
                    if (targetDb[i] < minTarget) minTarget = targetDb[i];
                    if (targetDb[i] > maxTarget) maxTarget = targetDb[i];
                }

                double targetSpread = maxTarget - minTarget;

                List<double> sidePowers = SampleSideLobes(targets, delay_ms, gain_db, octave);

                double maxSideDb = -120;
                double topSideDb = -120;

                if (sidePowers.Count > 0)
                {
                    sidePowers.Sort();
                    sidePowers.Reverse();

                    maxSideDb = 10.0 * Math.Log10(Math.Max(1E-12, sidePowers[0]));

                    int topCount = Math.Max(1, sidePowers.Count / 10);
                    double topSide = 0;

                    for (int i = 0; i < topCount; i++)
                    {
                        topSide += sidePowers[i];
                    }

                    topSide /= topCount;
                    topSideDb = 10.0 * Math.Log10(Math.Max(1E-12, topSide));
                }

                double delaySmooth = 0;

                for (int i = 1; i < delay_ms.Length - 1; i++)
                {
                    double d2 = delay_ms[i - 1] - 2.0 * delay_ms[i] + delay_ms[i + 1];
                    delaySmooth += d2 * d2;
                }

                double gainUse = 0;
                double gainSmooth = 0;

                for (int i = 0; i < gain_db.Length; i++)
                {
                    gainUse += gain_db[i] * gain_db[i];
                }

                for (int i = 1; i < gain_db.Length - 1; i++)
                {
                    double d2 = gain_db[i - 1] - 2.0 * gain_db[i] + gain_db[i + 1];
                    gainSmooth += d2 * d2;
                }

                return meanTarget + 0.35 * minTarget - 0.75 * maxSideDb - 0.15 * topSideDb
                    - 0.25 * targetSpread - 0.02 * delaySmooth - 0.001 * gainUse - 0.005 * gainSmooth;
            }
            private double ArrayMagnitudeAtPoint(Hare.Geometry.Point target, double[] delay_ms, double[] gain_db, int octave)
            {
                if (Elements == null || Elements.Count == 0) return 0;

                int oct = Math.Max(0, Math.Min(BandFrequencies.Length - 1, octave));

                double omega = BandFrequencies[oct] * Utilities.Numerics.PiX2;
                double k = omega / 343.0;

                System.Numerics.Complex sum = System.Numerics.Complex.Zero;

                for (int i = 0; i < Elements.Count; i++)
                {
                    RhinoObject obj = Elements[i];

                    if (obj == null || obj.Geometry == null) continue;
                    if (delay_ms == null || i >= delay_ms.Length) continue;
                    if (gain_db == null || i >= gain_db.Length) continue;

                    Hare.Geometry.Point src = RCPachTools.ModelPointToHare(obj.Geometry.GetBoundingBox(true).Min);

                    double r = (target - src).Length();

                    if (r <= Rhino.RhinoMath.ZeroTolerance) continue;

                    double tau = delay_ms[i] / 1000.0;
                    double amp = Math.Pow(10.0, gain_db[i] / 20.0);

                    double phase = -k * r - omega * tau;

                    sum += System.Numerics.Complex.FromPolarCoordinates(amp, phase);
                }

                return sum.Magnitude;
            }

            private double ArrayMagnitudeAtPoint(Rhino.Geometry.Point3d target, double[] delay_ms, int octave)
            {
                double omega = BandFrequencies[octave] * Utilities.Numerics.PiX2;
                double k = omega / 343.0;

                System.Numerics.Complex sum = System.Numerics.Complex.Zero;

                for (int i = 0; i < Elements.Count; i++)
                {
                    Rhino.Geometry.Point3d src =
                        Elements[i].Geometry.GetBoundingBox(true).Min;

                    double r = Utilities.RCPachTools.ModelToMeters(src.DistanceTo(target));
                    double tau = delay_ms[i] / 1000.0;

                    double phase = -k * r - omega * tau;

                    sum += System.Numerics.Complex.FromPolarCoordinates(1.0, phase);
                }

                return sum.Magnitude;
            }

            private static void EnsureSourceInConduit(Rhino.DocObjects.RhinoObject obj)
            {
                if (obj == null) return;

                bool found = false;

                foreach (System.Guid id in SourceConduit.Instance.UUID)
                {
                    if (id == obj.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    SourceConduit.Instance.SetSource(obj);
                }
            }

            private List<double> SampleSideLobes(List<Hare.Geometry.Point> targets, double[] delay_ms, double[] gain_db, int octave)
            {
                List<double> powers = new List<double>();

                List<Hare.Geometry.Point> origins = new List<Hare.Geometry.Point>();
                List<Hare.Geometry.Vector> directions = new List<Hare.Geometry.Vector>();

                for (int i = 0; i < Elements.Count; i++)
                {
                    RhinoObject obj = Elements[i];

                    if (obj == null || obj.Geometry == null) continue;

                    Point3d pt;

                    if (obj.Geometry is Rhino.Geometry.Point point)
                    {
                        pt = point.Location;
                    }
                    else
                    {
                        pt = obj.Geometry.GetBoundingBox(true).Center;
                    }

                    origins.Add(Utilities.RCPachTools.ModelPointToHare(pt));
                    directions.Add(Utilities.RCPachTools.SourceAimingVector(obj));
                }

                Hare.Geometry.Point center = Utilities.Geometry.ArrayCenter(origins, directions, ArraySimulationSettings.UseAimingCenter(Elements[0]));
                Hare.Geometry.Topology sphere = Utilities.Geometry.GeoSphere(2).Model[0];
                Rhino.Geometry.Mesh mesh = Utilities.RCPachTools.HaretoRhinoMesh(sphere, true);

                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    Hare.Geometry.Vector dir = new Hare.Geometry.Vector(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
                    dir.Normalize();
                    if (double.IsNaN(dir.dx) || double.IsInfinity(dir.dx)) continue;
                    double cosLimit = Math.Cos(12.0 * Math.PI / 180.0);
                    bool limit_exceeded = false;

                    for (int j = 0; j < targets.Count; j++)
                    {
                        Hare.Geometry.Vector tdir = targets[j] - center;
                        tdir.Normalize();
                        if (double.IsNaN(tdir.dx) || double.IsInfinity(tdir.dx)) continue;
                        if (Hare.Geometry.Hare_math.Dot(dir, tdir) >= cosLimit) limit_exceeded = true;
                    }

                    if (limit_exceeded) continue;

                    Hare.Geometry.Point sample = center + dir * PatternReferenceDistance.Value;

                    double mag = ArrayMagnitudeAtPoint(sample, delay_ms, gain_db, octave);

                    powers.Add(mag * mag);
                }

                return powers;
            }

            private void OnArrayOrientationChanged(object sender, EventArgs e)
            {
                if (LoadingGroup || Elements.Count == 0) return;
                // Let the numeric control finish committing typed input before
                // rotating the array and reloading its orientation values.
                if (sender != null)
                {
                    Application.Instance.AsyncInvoke(() => OnArrayOrientationChanged(null, e));
                    return;
                }
                RhinoDoc doc = Elements[0].Document ?? RhinoDoc.ActiveDoc;
                if (doc == null) return;
                List<RhinoObject> current = new List<RhinoObject>();
                List<Guid> currentIds = new List<Guid>();
                foreach (RhinoObject element in Elements)
                {
                    RhinoObject obj = doc.Objects.FindId(element.Id);
                    if (obj == null || obj.Geometry == null)
                    {
                        RhinoApp.WriteLine("The array contains a missing element. Reopen the array control.");
                        return;
                    }
                    current.Add(obj);
                    currentIds.Add(obj.Id);
                }

                Point3d center = Point3d.Origin;
                foreach (RhinoObject obj in current)
                {
                    Point3d point = obj.Geometry.GetBoundingBox(true).Center;
                    center += new Vector3d(point.X, point.Y, point.Z) / current.Count;
                }
                double[] previous = PachTools.DecodeTriple(current[0].Geometry.GetUserString("Aiming"));
                if (Math.Abs(previous[0] - ArrayAlt.Value) < 1e-8 &&
                    Math.Abs(previous[1] - ArrayAzi.Value) < 1e-8 &&
                    Math.Abs(previous[2] - ArrayAxial.Value) < 1e-8) return;
                // Reuse the cabinet transform so group rotation follows the same axial/altitude/azimuth convention as the balloons.
                Environment.Cabinet_Geometry axes = new Environment.Cabinet_Geometry();
                axes.Vertices.Add(new Hare.Geometry.Point(1, 0, 0));
                axes.Vertices.Add(new Hare.Geometry.Point(0, 1, 0));
                axes.Vertices.Add(new Hare.Geometry.Point(0, 0, 1));
                Environment.Cabinet_Geometry oldFrame = axes.Transform(new Hare.Geometry.Point(0, 0, 0), previous[0], previous[1], previous[2], 1);
                Environment.Cabinet_Geometry newFrame = axes.Transform(new Hare.Geometry.Point(0, 0, 0), ArrayAlt.Value, ArrayAzi.Value, ArrayAxial.Value, 1);
                Plane from = new Plane(center, new Vector3d(oldFrame.Vertices[0].x, oldFrame.Vertices[0].y, oldFrame.Vertices[0].z),
                    new Vector3d(oldFrame.Vertices[1].x, oldFrame.Vertices[1].y, oldFrame.Vertices[1].z));
                Plane to = new Plane(center, new Vector3d(newFrame.Vertices[0].x, newFrame.Vertices[0].y, newFrame.Vertices[0].z),
                    new Vector3d(newFrame.Vertices[1].x, newFrame.Vertices[1].y, newFrame.Vertices[1].z));
                Transform rotation = Transform.PlaneToPlane(from, to);
                List<GeometryBase> originals = new List<GeometryBase>();
                List<GeometryBase> rotated = new List<GeometryBase>();
                foreach (RhinoObject obj in current)
                {
                    double[] aim = PachTools.DecodeTriple(obj.Geometry.GetUserString("Aiming"));
                    Environment.Cabinet_Geometry frame = axes.Transform(new Hare.Geometry.Point(0, 0, 0), aim[0], aim[1], aim[2], 1);
                    Vector3d forward = new Vector3d(frame.Vertices[1].x, frame.Vertices[1].y, frame.Vertices[1].z);
                    Vector3d up = new Vector3d(frame.Vertices[2].x, frame.Vertices[2].y, frame.Vertices[2].z);
                    forward.Transform(rotation);
                    up.Transform(rotation);
                    originals.Add(obj.Geometry.Duplicate());
                    GeometryBase geometry = obj.Geometry.Duplicate();
                    if (!geometry.Transform(rotation))
                    {
                        geometry.Dispose();
                        foreach (GeometryBase original in originals) original.Dispose();
                        foreach (GeometryBase transformed in rotated) transformed.Dispose();
                        RhinoApp.WriteLine("Unable to rotate an array element.");
                        RebuildElementEditors();
                        return;
                    }
                    geometry.SetUserString("Aiming", ArraySourceConstruction.AimingString(forward, up));
                    geometry.SetUserString("ArrayControlMode", ColumnMode ? "Column" : "Line");
                    rotated.Add(geometry);
                }

                uint undo = doc.BeginUndoRecord("Rotate array");
                int replaced = 0;
                bool previewEnabled = PatternConduit.Enabled;
                LoadingGroup = true;
                PatternConduit.Enabled = false;
                try
                {
                    for (int i = 0; i < current.Count; i++)
                    {
                        string aiming = rotated[i].GetUserString("Aiming");
                        string controlMode = rotated[i].GetUserString("ArrayControlMode");
                        if (!doc.Objects.Replace(currentIds[i], rotated[i], false)) throw new InvalidOperationException("Unable to replace an array element.");
                        replaced++;
                        RhinoObject obj = doc.Objects.FindId(currentIds[i]);
                        if (obj == null || obj.Geometry == null) throw new InvalidOperationException("Unable to find a rotated array element.");
                        obj.Geometry.SetUserString("Aiming", aiming);
                        obj.Geometry.SetUserString("ArrayControlMode", controlMode);
                        obj.CommitChanges();
                        obj = doc.Objects.FindId(currentIds[i]);
                        if (obj == null || obj.Geometry == null || obj.Geometry.GetUserString("Aiming") != aiming)
                            throw new InvalidOperationException("Rhino did not save the array orientation.");
                    }
                }
                catch (Exception error)
                {
                    for (int i = 0; i < replaced; i++)
                    {
                        string aiming = originals[i].GetUserString("Aiming");
                        string controlMode = originals[i].GetUserString("ArrayControlMode");
                        doc.Objects.Replace(currentIds[i], originals[i], false);
                        RhinoObject obj = doc.Objects.FindId(currentIds[i]);
                        if (obj == null || obj.Geometry == null) continue;
                        obj.Geometry.SetUserString("Aiming", aiming);
                        obj.Geometry.SetUserString("ArrayControlMode", controlMode);
                        obj.CommitChanges();
                    }
                    RhinoApp.WriteLine(error.Message);
                }
                finally
                {
                    if (undo != 0) doc.EndUndoRecord(undo);
                    for (int i = 0; i < current.Count; i++)
                    {
                        RhinoObject obj = doc.Objects.FindId(currentIds[i]);
                        if (obj != null)
                        {
                            Elements[i] = obj;
                            OnElementChanged(obj);
                        }
                    }
                    LoadingGroup = false;
                    PatternConduit.Enabled = previewEnabled;
                    RebuildElementEditors();
                    UpdatePatternConduit();
                    doc.Views.Redraw();
                    foreach (GeometryBase geometry in originals) geometry.Dispose();
                    foreach (GeometryBase geometry in rotated) geometry.Dispose();
                }
            }

            private void RebuildElementEditors()
            {
                bool wasLoading = LoadingGroup;
                LoadingGroup = true;
                try
                {
                    RhinoDoc doc = Elements[0].Document ?? RhinoDoc.ActiveDoc;
                    if (doc != null)
                        for (int i = 0; i < Elements.Count; i++) Elements[i] = doc.Objects.FindId(Elements[i].Id) ?? Elements[i];
                    double[] aim = PachTools.DecodeTriple(Elements[0].Geometry.GetUserString("Aiming"));
                    ArrayAlt.Value = aim[0];
                    ArrayAzi.Value = aim[1];
                    ArrayAxial.Value = aim[2];
                }
                finally { LoadingGroup = wasLoading; }
                ElementLayout = new DynamicLayout();
                ElementLayout.Padding = 2;
                ElementLayout.DefaultSpacing = new Eto.Drawing.Size(4, 4);

                List<Control> row = new List<Control>();

                for (int i = 0; i < Elements.Count; i++)
                {
                    row.Add(new ArrayElementEditor(Elements[i], OnElementChanged, ThirdOctave));
                }

                ElementLayout.AddRow(row.ToArray());

                ElementScroll.Content = ElementLayout;
            }

            private void OnElementChanged(RhinoObject obj)
            {
                if (obj == null) return;
                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i] != null && Elements[i].Id == obj.Id)
                    {
                        Elements[i] = obj;
                        break;
                    }
                }

                EnsureSourceInConduit(obj);
                double[] aim = PachTools.DecodeTriple(obj.Geometry.GetUserString("Aiming"));
                SourceConduit.Instance.Update_Aiming(obj.Id, (float)aim[0], (float)aim[1], (float)aim[2]);
                if (LoadingGroup) return;
                if (Elements.Count > 0 && Elements[0].Id == obj.Id)
                {
                    LoadingGroup = true;
                    try
                    {
                        ArrayAlt.Value = aim[0];
                        ArrayAzi.Value = aim[1];
                        ArrayAxial.Value = aim[2];
                    }
                    finally { LoadingGroup = false; }
                }
                UpdatePatternConduit();
                RhinoDoc doc = obj.Document ?? RhinoDoc.ActiveDoc;
                if (doc != null) doc.Views.Redraw();
            }

            private void UpdatePatternConduit()
            {
                if (PatternConduit == null) return;

                if (ShowPattern == null ||
                    !ShowPattern.Checked.HasValue ||
                    !ShowPattern.Checked.Value)
                {
                    PatternConduit.Clear();
                    if (DiagnosticLevel != null) DiagnosticLevel.Text = "-- dB";
                    return;
                }

                PatternConduit.Show_Array_Graph = ShowArrayGraph.Checked == true;
                PatternConduit.Show_Array_Diagnostic_Slice = ShowArraySlice.Checked == true;
                PatternConduit.Array_Diagnostic_Plane = (SpeakerPatternConduit.DiagnosticPlane)DiagnosticPlaneSelect.SelectedIndex;
                PatternConduit.Array_Diagnostic_Angle = DiagnosticAngle.Value;
                PatternConduit.ThirdOctave = ThirdOctave;
                PatternConduit.Octave = PatternOctave.SelectedIndex;
                PatternConduit.SetArrayElements(Elements, PatternReferenceDistance.Value);
                double db = PatternConduit.ArrayDiagnosticLevel(DiagnosticAngle.Value);
                DiagnosticLevel.Text = double.IsNaN(db) ? "-- dB" : db.ToString("0.00") + " dB";
            }
        }

        public class ArrayElementEditor : GroupBox
        {
            private readonly System.Guid ObjectId;
            private readonly Action<Rhino.DocObjects.RhinoObject> Changed;

            private NumericStepper Alt;
            private NumericStepper Azi;
            private NumericStepper Axial;
            private NumericStepper[] OctPhaseDelay;
            private readonly bool ThirdOctave;
            private readonly double[] Frequencies;
            private string BandKey { get { return ThirdOctave ? "ThirdOctave" : "Octave"; } }
            private NumericStepper[] OctGainDb;

            private bool Loading = false;

            public ArrayElementEditor(Rhino.DocObjects.RhinoObject obj, Action<Rhino.DocObjects.RhinoObject> changed) : this(obj, changed, false) { }

            public ArrayElementEditor(Rhino.DocObjects.RhinoObject obj, Action<Rhino.DocObjects.RhinoObject> changed, bool thirdOctave)
            {
                ThirdOctave = thirdOctave;
                Frequencies = thirdOctave ? AcousticalMath.ThirdOctaveCenters() : AcousticalMath.OctaveCenters();
                OctPhaseDelay = new NumericStepper[Frequencies.Length];
                OctGainDb = new NumericStepper[Frequencies.Length];
                ObjectId = obj.Id;
                Changed = changed;

                if (obj == null || obj.Geometry == null) Text = "Array Element";
                else
                {
                    string label = obj.Geometry.GetUserString("SourceLabel");
                    if (!string.IsNullOrWhiteSpace(label)) Text = label;
                    else
                    {
                        string group = obj.Geometry.GetUserString("ArrayGroupLabel");
                        string suffix = obj.Geometry.GetUserString("ArrayElementSuffix");

                        if (!string.IsNullOrWhiteSpace(group) && !string.IsNullOrWhiteSpace(suffix))
                        {
                            Text = group + suffix;
                        }
                        else
                        {
                            Text = obj.Id.ToString();
                        }
                    }
                }
                DynamicLayout layout = new DynamicLayout();
                layout.Padding = 4;
                layout.DefaultSpacing = new Eto.Drawing.Size(2, 2);

                Alt = NewAngleStepper(-90, 90);
                Azi = NewAngleStepper(-360, 360);
                Axial = NewAngleStepper(-360, 360);
                string controlMode = obj.Geometry.GetUserString("ArrayControlMode") ?? obj.Geometry.GetUserString("ArrayDeviceType");
                Alt.Enabled = Azi.Enabled = Axial.Enabled = controlMode != "Column";

                Alt.ValueChanged += ValueChanged;
                Azi.ValueChanged += ValueChanged;
                Axial.ValueChanged += ValueChanged;

                for (int i = 0; i < Frequencies.Length; i++)
                {
                    OctPhaseDelay[i] = new NumericStepper();
                    OctPhaseDelay[i].DecimalPlaces = ThirdOctave ? 2 : 0;
                    OctPhaseDelay[i].MinValue = -36000;
                    OctPhaseDelay[i].MaxValue = 36000;
                    OctPhaseDelay[i].Increment = 5;
                    OctPhaseDelay[i].Width = 58;
                    OctPhaseDelay[i].ValueChanged += ValueChanged;

                    OctGainDb[i] = NewGainStepper();
                    OctGainDb[i].ValueChanged += ValueChanged;
                }

                layout.AddRow(new Label { Text = "Alt", Width = 28 }, Alt);
                layout.AddRow(new Label { Text = "Azi", Width = 28 }, Azi);
                layout.AddRow(new Label { Text = "Ax", Width = 28 }, Axial);

                layout.AddRow(new Label { Text = "Hz", Width = 28 }, new Label { Text = "Phase°" }, new Label { Text = "Gain" });
                for (int i = 0; i < Frequencies.Length; i++) layout.AddRow(new Label { Text = Frequencies[i].ToString("0.##"), Width = 42 }, OctPhaseDelay[i], OctGainDb[i]);

                Button zeroDelays = new Button();
                zeroDelays.Text = "Zero Phase";
                zeroDelays.Width = 58;
                zeroDelays.Click += (s, e) =>
                {
                    Loading = true;
                    for (int i = 0; i < Frequencies.Length; i++)
                    {
                        OctPhaseDelay[i].Value = 0;
                    }
                    Loading = false;
                    Commit();
                };

                Button zeroGain = new Button();
                zeroGain.Text = "Zero Gain";
                zeroGain.Width = 76;
                zeroGain.Click += (s, e) =>
                {
                    Loading = true;
                    for (int i = 0; i < Frequencies.Length; i++)
                    {
                        OctGainDb[i].Value = 0;
                    }
                    Loading = false;
                    Commit();
                };

                layout.AddRow(null, zeroDelays, zeroGain);

                Content = layout;

                //Load From Object;
                Loading = true;

                double[] aim = PachTools.DecodeTriple(obj.Geometry.GetUserString("Aiming"));

                Alt.Value = aim[0];
                Azi.Value = aim[1];
                Axial.Value = aim[2];

                double[] delay = ArraySimulationSettings.BandValues(obj, "Delay", ThirdOctave);
                double[] gain = ArraySimulationSettings.BandValues(obj, "Gain", ThirdOctave);
                for (int i = 0; i < Frequencies.Length; i++)
                {
                    OctPhaseDelay[i].Value = delay[i] * Frequencies[i] * 360 / 1000;
                    OctGainDb[i].Value = gain[i];
                }

                Loading = false;
            }

            private NumericStepper NewAngleStepper(double min, double max)
            {
                NumericStepper s = new NumericStepper();
                s.DecimalPlaces = 1;
                s.MinValue = min;
                s.MaxValue = max;
                s.Increment = 1;
                s.Width = 58;
                return s;
            }

            private NumericStepper NewGainStepper()
            {
                NumericStepper s = new NumericStepper();
                s.DecimalPlaces = 1;
                s.MinValue = -30;
                s.MaxValue = 6;
                s.Increment = 0.5;
                s.Width = 50;
                return s;
            }

            private void ValueChanged(object sender, EventArgs e)
            {
                if (Loading) return;
                Commit();
            }

            private void Commit()
            {
                Rhino.DocObjects.RhinoObject obj =
                    Rhino.RhinoDoc.ActiveDoc?.Objects.FindId(ObjectId);

                if (obj == null || obj.Geometry == null) return;

                string controlMode = obj.Geometry.GetUserString("ArrayControlMode") ?? obj.Geometry.GetUserString("ArrayDeviceType");
                if (controlMode != "Column") obj.Geometry.SetUserString(
                    "Aiming",
                    Alt.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) + ";" +
                    Azi.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) + ";" +
                    Axial.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));

                double[] phase = new double[Frequencies.Length];
                double[] delay = new double[Frequencies.Length];
                double[] gain = new double[Frequencies.Length];

                for (int i = 0; i < Frequencies.Length; i++)
                {
                    phase[i] = OctPhaseDelay[i].Value;
                    delay[i] = phase[i] / 360.0 / Frequencies[i] * 1000.0;
                    gain[i] = OctGainDb[i].Value;
                }

                obj.Geometry.SetUserString("ArrayPhase" + BandKey + "Deg", PachTools.EncodeSourcePower(phase));
                obj.Geometry.SetUserString("ArrayDelay" + BandKey + "Ms", PachTools.EncodeSourcePower(delay));
                obj.Geometry.SetUserString("ArrayGain" + BandKey + "Db", PachTools.EncodeSourcePower(gain));

                if (!obj.CommitChanges()) return;

                // Re-fetch after committing so everybody downstream gets
                // the current document object rather than this edit wrapper.
                obj = Rhino.RhinoDoc.ActiveDoc.Objects.FindId(ObjectId);

                if (obj == null) return;

                bool found = false;

                foreach (System.Guid id in SourceConduit.Instance.UUID)
                {
                    if (id == obj.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    SourceConduit.Instance.SetSource(obj);
                }

                Changed?.Invoke(obj);
            }
        }
    }
}
