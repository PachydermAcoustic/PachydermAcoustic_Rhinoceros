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
using ScottPlot.Extensions;
using ScottPlot.Plottables;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        //[GuidAttribute("79B97A26-CEBC-4FA8-8275-9D961ADF1772")]
        public class ConvergenceProgress : Eto.Forms.FloatingForm, Pachyderm_Acoustic.I_Conv_Progress
        {
            private Queue<double> history1 = new Queue<double>();
            private Queue<double> history2 = new Queue<double>();
            private List<double> CT = new List<double>();

            private ScottPlot.Eto.EtoPlot Conv_View;
            private ScottPlot.Eto.EtoPlot Hist_View;
            double[] conv1, conv2;
            double[] dom1, dom2, domh;
            private Scatter Conv1, Conv2, hist1, hist2;
            private Button Conclude;
            public CancellationTokenSource canceled;

            public ConvergenceProgress(CancellationTokenSource CTS)
            {
                canceled = CTS;
                //this.Location = new Eto.Drawing.Point(100, 200);
                DynamicLayout Layout = new Eto.Forms.DynamicLayout();
                this.Conv_View = new ScottPlot.Eto.EtoPlot();
                this.Hist_View = new ScottPlot.Eto.EtoPlot();
                this.Conclude = new Eto.Forms.Button();

                Conclude.Text = "Conclude_Simulation";
                Conclude.Click += this.Conclude_Click;

                Conv_View.Plot.TitlePanel.Label.Text = "Impulse Response Status";
                Conv_View.Plot.TitlePanel.Label.Font.Size = 12;
                Conv_View.Plot.XAxis.Label.Text = "Time (s)";
                Conv_View.Plot.XAxis.Label.Font.Size = 12;
                Conv_View.Plot.YAxis.Label.Text = "Ratio of Change";
                Conv_View.Plot.YAxis.Label.Font.Size = 12;
                Conv_View.Size = new Eto.Drawing.Size(350, 250);
                dom1 = new double[6] { 0, 0.05, 0.05, 0.08, 0.08, 3 };
                conv1 = new double[6] { 0,0,0,0,0,0 };
                dom2 = new double[6] { 0, 0.05, 0.05, 0.08, 0.08, 3 };
                conv2 = new double[6] { 0, 0, 0, 0, 0, 0 };

                Conv1 = Conv_View.Plot.Add.Scatter(dom1, conv1, ScottPlot.Colors.Blue);
                Conv2 = Conv_View.Plot.Add.Scatter(dom2, conv2, ScottPlot.Colors.Red);
                
                Hist_View.Plot.TitlePanel.Label.Text = "Convergence History (Last 20 records)";
                Hist_View.Plot.TitlePanel.Label.Font.Size = 12;
                Hist_View.Plot.XAxis.Label.Text = "Iteration";
                Hist_View.Plot.XAxis.Label.Font.Size = 12;
                Hist_View.Plot.YAxis.Label.Text = "Maximum Change";
                Hist_View.Plot.YAxis.Label.Font.Size = 12;
                Hist_View.Size = new Eto.Drawing.Size(350, 250);
                domh = new double[20] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19 };
                history1 = new Queue<double>(20);
                history2 = new Queue<double>(20);
                for (int i = 0; i < 20; i++) { history1.Enqueue(0); history2.Enqueue(0); }

                hist1 = Hist_View.Plot.Add.Scatter(domh, history1.ToArray(), ScottPlot.Colors.Blue);
                hist2 = Hist_View.Plot.Add.Scatter(domh, history2.ToArray(), ScottPlot.Colors.Red);
                Layout.AddRow(Conv_View);
                Layout.AddRow(Hist_View);
                Layout.AddRow(Conclude);
                Layout.DefaultPadding = 8;
                Layout.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                this.Content = Layout;
                this.Content.Invalidate();
            }

            public async void Fill(double[] Conv1, double Conv2, double ConvInf, int ID, int count, double corr)
            {
                    if (Conv1.Length == 1) Populate(Conv1[0], Conv2, ConvInf, ID, corr);
                    else if (Conv1.Length > 1) Populate(Conv1, Conv2, ConvInf, ID, count);
                    else return;
            }

            public void Populate()
            {
                Rhino.RhinoApp.InvokeOnUiThread(new Action(() => { 
                    Hist_View.Update(new Eto.Drawing.Rectangle(Hist_View.Size)); 
                    Conv_View.Update(new Eto.Drawing.Rectangle(Conv_View.Size)); 
                }));
            }

            public bool Populate(double Conv1, double Conv2, double ConvInf, int ID, double corr = 0)
            {
                if (Conv1.IsInfiniteOrNaN() || Conv1 > 20) Conv1 = 20;
                if (Conv2.IsInfiniteOrNaN() || Conv2 > 20) Conv2 = 20;
                if (ConvInf.IsInfiniteOrNaN() || ConvInf > 20) ConvInf = 20;
                if (this.Visible == false) return false;

                if (corr != 0) Conv_View.Plot.TitlePanel.Label.Text = "Impulse Response Status - Schroeder correlation = " + Math.Round(corr, 3);

                if (ID == 0)
                {
                    conv1 = new double[6] { Conv1, Conv1, Conv2, Conv2, ConvInf, ConvInf };
                    double convmax = conv1.Max(); 
                    if (this.history1.Count != 0 && convmax == this.history1.Last()) return false;

                    Conv_View.Plot.Clear();
                    Hist_View.Plot.Clear();

                    dom1 = new double[6] { 0, 0.05, 0.05, 0.08, 0.08, 3 };
                    double[] Conv_template = new double[6] { 0.02, 0.02, 0.02, 0.02, 0.2, 0.2 };

                    Conv_View.Plot.Add.Scatter(dom1.ToList(), Conv_template.ToList(), ScottPlot.Colors.Gray);


                    this.history1.Enqueue(convmax);
                    if (this.history1.Count > 20) this.history1.Dequeue();
                    ScottPlot.DataSources.ScatterSourceXsYs history1 = new ScottPlot.DataSources.ScatterSourceXsYs(this.domh, this.history1.ToArray());
                    this.hist1 = Hist_View.Plot.Add.Scatter(domh, this.history1.ToArray(), ScottPlot.Colors.Red);

                    Conv_View.Plot.Add.Scatter(dom1.ToList(), conv1.ToList(), ScottPlot.Colors.Red);
                    if (this.history1.Count < 20) CT.Add(CT.Count - 10);
                }
                if (ID > 0)
                {
                    dom2 = new double[6] { 0, 0.05, 0.05, 0.08, 0.08, 3 };
                    conv2 = new double[6] { Conv1, Conv1, Conv2, Conv2, ConvInf, ConvInf };
                    double convmax = conv2.Max();
                    if (history1.Count != 0 && convmax == this.history2.Last()) return false;
                    
                    Hist_View.Plot.Add.Scatter(dom2.ToList(), conv2.ToList(), ScottPlot.Colors.Blue);
                    this.history2.Enqueue(convmax);
                    if (this.history2.Count > 20) this.history2.Dequeue();
                    ScottPlot.DataSources.ScatterSourceXsYs history2 = new ScottPlot.DataSources.ScatterSourceXsYs(this.domh, this.history2.ToArray());
                    this.hist2 = Hist_View.Plot.Add.Scatter(domh, this.history2.ToArray(), ScottPlot.Colors.Blue);
                }

                ScottPlot.Color Conv_Color = Colors.Gray;
                //switch (conv_count)
                //{
                //    case 1:
                //        Conv_Color = Colors.Red;
                //        break;
                //    case 2:
                //        Conv_Color = Colors.OrangeRed;
                //        break;
                //    case 3:
                //        Conv_Color = Colors.Orange;
                //        break;
                //    case 4:
                //        Conv_Color = Colors.Yellow;
                //        break;
                //    case 5:
                //        Conv_Color = Colors.YellowGreen;
                //        break;
                //    case 6:
                //        Conv_Color = Colors.Green;
                //        break;
                //    case 7:
                //        Conv_Color = Colors.ForestGreen;
                //        break;
                //    case 8:
                //        Conv_Color = Colors.DarkSeaGreen;
                //        break;
                //    case 9:
                //        Conv_Color = Colors.Blue;
                //        break;
                //    case 10:
                //        Conv_Color = Colors.DarkBlue;
                //        break;
                //    default:
                //        break;
                //}

                Hist_View.Plot.Add.Scatter(new double[2] { 0, 20 }, new double[2] { 0.2, 0.2 }, Colors.Gray);
                Hist_View.Plot.Add.Scatter(new double[2] { 0, 20 }, new double[2] { 0.02, 0.02 }, Colors.Gray);
                Conv_View.Plot.YAxis.Min = 0;
                Hist_View.Plot.YAxis.Min = 0;
                Conv_View.Plot.YAxis.Max = Math.Max(Math.Max(conv1.Max(), conv2.Max()), 0.3);
                Hist_View.Plot.YAxis.Max = Math.Max(Math.Max(history1.Max(), history2.Max()), 0.3);
                return false;
            }

            int conv_count = 0;

            public bool Populate(double[] IR_p, double Convergence, double sample_freq, int count, int ID)
            {
                if (this.Visible == false) return false;

                double[] t = new double[IR_p.Length];
                for (int i = 0; i < IR_p.Length; i++) t[i] = (i / sample_freq);
                if (ID == 0)
                {
                    conv_count = count;
                    history1.Enqueue(Convergence);
                    if (history1.Count > 20) history1.Dequeue();
                    Hist_View.Plot.Clear();
                    Conv_View.Plot.Clear();
                    Hist_View.Plot.Add.Scatter(t, IR_p, Colors.Red);
                    Hist_View.Plot.XAxis.Max = t.Last();
                    Hist_View.Plot.YAxis.Max = Math.Max(IR_p.Max(), 0.3);
                    if (history1.Count < 20) CT.Add(CT.Count - 10);
                    Conv_View.Plot.Add.Scatter(CT.ToArray(), history1.ToArray(), Colors.Red);
                    //IR_View.ZoomOutAll(IR_View.Plot);
                    Conv_View.Plot.XAxis.Min = -10;
                    Conv_View.Plot.XAxis.Max = CT.Count() - 10;
                    Conv_View.Plot.YAxis.Max = history1.Max();
                }
                if (ID == 1)
                {
                    conv_count = Math.Min(conv_count, count);
                    history2.Enqueue(Convergence);
                    if (history2.Count > 20) history2.Dequeue();
                    Hist_View.Plot.Add.Scatter(t, IR_p, Colors.Blue);
                    Hist_View.Plot.XAxis.Max = Math.Max(Hist_View.Plot.XAxis.Max, t.Last());
                    Hist_View.Plot.YAxis.Max = Math.Max(Hist_View.Plot.YAxis.Max, Math.Max(IR_p.Max(), 0.3));
                    Conv_View.Plot.YAxis.Max = Math.Max(Conv_View.Plot.YAxis.Max, history2.Max());
                    Conv_View.Plot.Add.Scatter(CT.ToArray(), history2.ToArray(), Colors.Blue);
                }
                Hist_View.Plot.XAxis.Label.Text = "Time (ms)";
                Hist_View.Plot.YAxis.Label.Text = "Magnitude";

                //if (ID == 0)
                //{
                //    if (Convergence < 100) IR_View.Plot.YAxis.Type = AxisType.Linear;
                //    else IR_View.Plot.YAxis.Type = AxisType.Log;
                //    if (history1.Max() < 100) Conv_View.Plot.YAxis.Type = AxisType.Linear;
                //    else Conv_View.Plot.YAxis.Type = AxisType.Log;
                //}
                //else
                //{
                //    if (Convergence > 100) IR_View.Plot.YAxis.Type = AxisType.Log;
                //    if (history2.Max() > 100) Conv_View.Plot.YAxis.Type = AxisType.Log;
                //}

                Color Conv_Color = Colors.Gray;
                switch (conv_count)
                {
                    case 1:
                        Conv_Color = Colors.Red;
                        break;
                    case 2:
                        Conv_Color = Colors.OrangeRed;
                        break;
                    case 3:
                        Conv_Color = Colors.Orange;
                        break;
                    case 4:
                        Conv_Color = Colors.Yellow;
                        break;
                    case 5:
                        Conv_Color = Colors.YellowGreen;
                        break;
                    case 6:
                        Conv_Color = Colors.Green;
                        break;
                    case 7:
                        Conv_Color = Colors.ForestGreen;
                        break;
                    case 8:
                        Conv_Color = Colors.DarkSeaGreen;
                        break;
                    case 9:
                        Conv_Color = Colors.Blue;
                        break;
                    case 10:
                        Conv_Color = Colors.DarkBlue;
                        break;
                    default:
                        break;
                }

                Hist_View.Plot.Add.Scatter(new double[2] { -10000, 10000 }, new double[2] { 0.1, 0.1 }, Conv_Color);
                Conv_View.Plot.Add.Scatter(new double[2] { -5, 25 }, new double[2] { 0.1, 0.1 }, Conv_Color);

                //if (conclude)
                //{
                //    conclude = false;
                //    return true;
                //}
                //else return false;
                return false;
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