using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eto.Forms;
using MathNet.Numerics;
using Rhino.UI;
using ScottPlot;
using ScottPlot.Plottables;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public class ConvergenceProgress : Eto.Forms.FloatingForm, Pachyderm_Acoustic.I_Conv_Progress
        {
            private Queue<double>[] history;
            private List<double> CT = new List<double>();

            private ScottPlot.Eto.EtoPlot Conv_View;
            private ScottPlot.Eto.EtoPlot Hist_View;
            private ScottPlot.Eto.EtoPlot[] IR_View;
            double[] Conv_TargetY, conv2;
            double[] Conv_TargetX, dom2, domh;
            private Button Conclude;
            public CancellationTokenSource canceled;
            public int samplefreq;

            public ConvergenceProgress(CancellationTokenSource CTS, int Samplefreq = 44100)
            {
                samplefreq = Samplefreq;
                canceled = CTS;
                //this.Location = new Eto.Drawing.Point(100, 200);
                DynamicLayout Layout = new Eto.Forms.DynamicLayout();
                this.Conv_View = new ScottPlot.Eto.EtoPlot();
                this.Hist_View = new ScottPlot.Eto.EtoPlot();
                this.IR_View = new ScottPlot.Eto.EtoPlot[2];
                this.IR_View[0] = new ScottPlot.Eto.EtoPlot();
                this.IR_View[1] = new ScottPlot.Eto.EtoPlot();
                this.Conclude = new Eto.Forms.Button();

                Conclude.Text = "Conclude_Simulation";
                Conclude.Click += this.Conclude_Click;

                Conv_View.Plot.Title("Impulse Response Status", 12);
                Conv_View.Plot.XLabel("Time (s)", 12);
                Conv_View.Plot.YLabel("Ratio of Change", 12);
                Conv_View.Size = new Eto.Drawing.Size(350, 250);
                IR_View[0].Plot.Title("Farthest Clear Receiver", 12);
                IR_View[0].Plot.XLabel("Time (s)", 12) ;
                IR_View[0].Plot.YLabel("Sound Pressure Level", 12);
                IR_View[0].Size = new Eto.Drawing.Size(350, 250);
                IR_View[1].Plot.Title("Farthest Obstructed Receiver", 12) ;
                IR_View[1].Plot.XLabel("Time (s)",12);
                IR_View[1].Plot.YLabel("Sound Pressure Level",12);
                IR_View[1].Size = new Eto.Drawing.Size(350, 250); 
                Conv_TargetX = new double[3] { 0.05, 0.08, .25 };
                Conv_TargetY = new double[3] { 0.02,0.02,0.1 };
                
                Hist_View.Plot.Title("Convergence History (Last 20 records)", 12);
                Hist_View.Plot.XLabel("Iteration", 12);
                Hist_View.Plot.YLabel("Maximum Change",12);
                Hist_View.Size = new Eto.Drawing.Size(350, 250);
                domh = new double[20] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19 };
                history = new Queue<double>[2];
                history[0] = new Queue<double>(20);
                history[1] = new Queue<double>(20);
                for (int i = 0; i < 20; i++) { history[0].Enqueue(0); history[1].Enqueue(0); }

                DynamicLayout tbl1 = new DynamicLayout();
                DynamicLayout tbl2 = new DynamicLayout();
                tbl1.AddRow(Conv_View, IR_View[0]);
                tbl2.AddRow(Hist_View, IR_View[1]);
                Layout.AddRow(tbl1);
                Layout.AddRow(tbl2);
                Layout.AddRow(Conclude);
                Layout.DefaultPadding = 8;
                Layout.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                this.Content = Layout;
                this.Content.Invalidate();
            }

            public void Set_Targets(double[] X, double[] Y)
            {
                if (X != null && X.Count() > 0 && Y != null && Y.Count() > 0)
                {
                    Conv_TargetX = X;
                    Conv_TargetY = Y;
                }
                Update_Targets();
            }

            public void Update_Targets()
            {
                if (Conv_TargetX != null && Conv_TargetX.Count() > 0 && Conv_TargetY != null && Conv_TargetY.Count() > 0)
                {
                    Coordinates last_pos = new Coordinates(0, Conv_TargetY[0]);
                    for (int i = 0; i < Conv_TargetY.Length; i++)
                    {
                        Coordinates CNEW = new Coordinates(Conv_TargetX[i], Conv_TargetY[i]);
                        var C = Conv_View.Plot.Add.Line(new CoordinateLine(last_pos, CNEW));
                        C.LineStyle.Color = ScottPlot.Colors.Gray;
                        C.LineStyle.Pattern = LinePattern.Dotted;
                        last_pos = CNEW;
                        if (i < Conv_TargetY.Length - 1)
                        {
                            Coordinates next_pos = new Coordinates(CNEW.X, Conv_TargetY[i + 1]);
                            C = Conv_View.Plot.Add.Line(new CoordinateLine(CNEW, next_pos));
                            C.LineStyle.Color = ScottPlot.Colors.Gray;
                            C.LineStyle.Pattern = LinePattern.Dotted;
                            last_pos = next_pos;
                        }
                    }

                    CoordinateLine Line = new CoordinateLine(0, 1, 20, 1);
                    var H = Hist_View.Plot.Add.Line(Line);
                    H.LineStyle.Color = ScottPlot.Colors.Gray;
                    H.LineStyle.Pattern = LinePattern.Dotted;

                    Conv_View.Plot.Axes.SetLimitsX(0, Conv_TargetX.Max());
                    Hist_View.Plot.Axes.SetLimitsX(0, 20);
                    Conv_View.Plot.Axes.SetLimitsY(0, Conv_TargetY.Max() * 2);

                    double max1 = history[0].Max();
                    double max2 = history[1].Max();

                    Hist_View.Plot.Axes.SetLimitsY(0, Math.Min(20, Math.Max(max1, max2 )));
                }
                Conv_View.Invalidate();
                Hist_View.Invalidate();
                IR_View[0].Invalidate();
                IR_View[1].Invalidate();
            }

            object ui_lock = new object();

            public void Update()
            {
                lock (ui_lock)
                {
                        Conv_View.Invalidate();
                        Hist_View.Invalidate();
                        IR_View[0].Invalidate();
                        IR_View[1].Invalidate();
                        Rhino.RhinoApp.InvokeOnUiThread(new Action(() =>
                        {
                            Hist_View.Update(new Eto.Drawing.Rectangle(Hist_View.Size));
                            Conv_View.Update(new Eto.Drawing.Rectangle(Conv_View.Size));
                        }));
                }
            }

            public async void Fill(string title, double[] YArray, int ID, Queue<double[]> IR = null)
            {
                //double[] Conv1, double Conv2, double ConvInf, int ID, int count, double corr,
                lock (ui_lock)
                {
                    if (ID == 0)
                    {
                        Conv_View.Plot.Clear();
                        Hist_View.Plot.Clear();
                    }

                    if (IR == null || IR.Count == 0) IR_View[ID].Enabled = false;
                    else
                    {
                        IR_View[ID].Enabled = true;
                        IR_View[ID].Plot.Clear();
                        
                        // Add null checks for each IR element before passing to Add.Signal
                        if (IR.Count > 4) 
                        {
                            double[] irData = IR.ElementAtOrDefault(3);
                            if (irData != null && irData.Length > 0)
                                IR_View[ID].Plot.Add.Signal(irData, 1.0 / samplefreq, ScottPlot.Color.FromARGB(0x33808080));
                        }
                        if (IR.Count > 3) 
                        {
                            double[] irData = IR.ElementAtOrDefault(2);
                            if (irData != null && irData.Length > 0)
                                IR_View[ID].Plot.Add.Signal(irData, 1.0 / samplefreq, ScottPlot.Color.FromARGB(0x33808080));
                        }
                        if (IR.Count > 2) 
                        {
                            double[] irData = IR.ElementAtOrDefault(4);
                            if (irData != null && irData.Length > 0)
                                IR_View[ID].Plot.Add.Signal(irData, 1.0 / samplefreq, ScottPlot.Color.FromARGB(0x33808080));
                        }
                        if (IR.Count > 1) 
                        {
                            double[] irData = IR.ElementAtOrDefault(1);
                            if (irData != null && irData.Length > 0)
                                IR_View[ID].Plot.Add.Signal(irData, 1.0 / samplefreq, ScottPlot.Color.FromARGB(0x33808080));
                        }
                        if (IR.Count > 0)
                        {
                            double[] IR_curr = IR.ElementAtOrDefault(0);
                            if (IR_curr != null && IR_curr.Length > 0)
                            {
                                IR_View[ID].Plot.Add.Signal(IR_curr, 1.0 / samplefreq, ID == 0 ? ScottPlot.Color.FromARGB(0x800000FF) : ScottPlot.Color.FromARGB(0x80FF0000)); //ScottPlot.Color.FromARGB(0x330000F) : ScottPlot.Color.FromARGB(0x33FF0000));//ScottPlot.Color.FromARGB(0x800000FF) : ScottPlot.Color.FromARGB(0x80FF0000));
                                IR_View[ID].Plot.Axes.SetLimits(0, IR_curr.Length / samplefreq, -70, 0);
                            }
                        }
                        IR_View[ID].Invalidate();
                    }

                    if (Conv_TargetX != null && Conv_TargetX.Count() > 0 && YArray != null && YArray.Count() > 0) Conv_View.Plot.Title(title);

                    double x_last = 0;

                    double ConvMax = 0;

                    double xplotmod = ID == 0 ? 0 : 1;

                    for (int i = 0; i < Conv_TargetX.Length; i++)
                    {
                        double width = (Conv_TargetX[i] - x_last) * 0.5;
                        if (!double.IsNaN(YArray[i])) ConvMax = Math.Max(ConvMax, YArray[i] / Conv_TargetY[i]);
                        Coordinates CNEW = new Coordinates(x_last + width * xplotmod, 0);
                        var R = Conv_View.Plot.Add.Rectangle(new CoordinateRect(new Coordinates(CNEW.X + width, YArray[i]), CNEW));

                        R.FillColor = ID == 0 ? ScottPlot.Color.FromARGB(0x330000FF) : ScottPlot.Color.FromARGB(0x33FF0000);
                        R.LineColor = R.FillColor;
                        x_last = Conv_TargetX[i];
                    }

                    // Add proper bounds checking and null validation for history
                    if (ID >= 0 && ID < history.Length && history[ID] != null)
                    {
                        this.history[ID].Enqueue(ConvMax);
                        if (this.history[ID].Count > 20) this.history[ID].Dequeue();
                        
                        // Ensure domh is not null and has valid data
                        if (domh != null && domh.Length > 0)
                        {
                            double[] historyArray = this.history[ID].ToArray();
                            if (historyArray != null && historyArray.Length > 0)
                            {
                                Hist_View.Plot.Add.Scatter(domh, historyArray, ID == 0 ? ScottPlot.Colors.Blue : ScottPlot.Colors.Red);
                            }
                        }
                    }
                    
                    if (this.history[ID].Count < 20) CT.Add(CT.Count - 10);

                    Update_Targets();
                }
            }

            public event EventHandler Conclusion;

            private void Conclude_Click(object sender, EventArgs e)
            {
                if (Conclusion != null) Conclusion(this, EventArgs.Empty);
                canceled.Cancel();
                Conclude.Text = "Concluding... Results may be inconclusive...";
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Conclude.Dispose();
                Conv_View.Dispose();
                Hist_View.Dispose();
            }   

            public CancellationToken CancellationToken
            { 
                get { return canceled.Token; }
            }
            public CancellationTokenSource Canceler
            {
                get { return canceled; }
            }
        }   
    }       
}    