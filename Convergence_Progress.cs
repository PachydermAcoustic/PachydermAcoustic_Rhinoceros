using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
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
            private ScottPlot.Eto.EtoPlot IR_View;
            private Button Conclude;
            public ConvergenceProgress()
            {
                //this.Location = new Eto.Drawing.Point(100, 200);
                DynamicLayout Layout = new Eto.Forms.DynamicLayout();
                this.Conv_View = new ScottPlot.Eto.EtoPlot();
                this.IR_View = new ScottPlot.Eto.EtoPlot();
                this.Conclude = new Eto.Forms.Button();

                Conclude.Text = "Conclude_Simulation";
                Conclude.Click += this.Conclude_Click;

                IR_View.Plot.TitlePanel.Label.Text = "Impulse Response Status";
                IR_View.Plot.TitlePanel.Label.Font.Size = 12;
                IR_View.Plot.XAxis.Label.Text = "Time (s)";
                IR_View.Plot.XAxis.Label.Font.Size = 12;
                IR_View.Plot.YAxis.Label.Text = "Ratio of Change";
                IR_View.Plot.YAxis.Label.Font.Size = 12;
                IR_View.Size = new Eto.Drawing.Size(350, 250);
                Conv_View.Plot.TitlePanel.Label.Text = "Convergence History (Last 20 records)";
                Conv_View.Plot.TitlePanel.Label.Font.Size = 12;
                Conv_View.Plot.XAxis.Label.Text = "Iteration";
                Conv_View.Plot.XAxis.Label.Font.Size = 12;
                Conv_View.Plot.YAxis.Label.Text = "Maximum Change";
                Conv_View.Plot.YAxis.Label.Font.Size = 12;
                Conv_View.Size = new Eto.Drawing.Size(350, 250);
                
                Layout.AddRow(IR_View);
                Layout.AddRow(Conv_View);
                Layout.AddRow(Conclude);
                Layout.DefaultPadding = 8;
                Layout.DefaultSpacing = new Eto.Drawing.Size(8, 8);
                this.Content = Layout;
                this.Content.Invalidate();
            }

            public async void Populate(double[] Conv1, double Conv2, double ConvInf, int ID, int count, double corr)
            {
                //new System.Threading.Thread(() =>
                //{
                //Eto.Forms.Application.Instance.Invoke(() =>
                //{
                //await Task.Run(() =>
                //{
                    if (Conv1.Length == 1) Populate(Conv1[0], Conv2, ConvInf, ID, corr);
                    else if (Conv1.Length > 1) Populate(Conv1, Conv2, ConvInf, ID, count);
                    else return;
                    this.Invalidate();
                    IR_View.Update(new Eto.Drawing.Rectangle(IR_View.Size));
                    Conv_View.Update(new Eto.Drawing.Rectangle(IR_View.Size));
                //});
                //});
                //}).Start();
            }

            //bool intialized = false;
            public bool Populate(double Conv1, double Conv2, double ConvInf, int ID, double corr = 0)
            {
                if (Conv1.IsInfiniteOrNaN() || Conv1 > 20) Conv1 = 20;
                if (Conv2.IsInfiniteOrNaN() || Conv2 > 20) Conv2 = 20;
                if (ConvInf.IsInfiniteOrNaN() || ConvInf > 20) ConvInf = 20;
                if (this.Visible == false) return false;

                if (corr != 0) IR_View.Plot.TitlePanel.Label.Text = "Impulse Response Status - Schroeder correlation = " + Math.Round(corr, 3);

                double[] t = new double[6] { 0, 0.05, 0.05, 0.08, 0.08, 3 };
                double[] conv = new double[6] { Conv1, Conv1, Conv2, Conv2, ConvInf, ConvInf };
                double convmax = conv.Max();

                if (ID == 0)
                {
                    if (history1.Count != 0  && convmax == history1.Last()) return false;

                    IR_View.Plot.Clear();
                    Conv_View.Plot.Clear();
                    //////if (!initialized)
                    //////{
                    history1.Enqueue(convmax);
                    if (history1.Count > 20) history1.Dequeue();
                    IR_View.Plot.Add.Scatter(t.ToList(), conv.ToList(), ScottPlot.Colors.Red);
                    //IR_View.Plot.YAxis.Max = Math.Max(convmax, 0.3);
                    if (history1.Count < 20) CT.Add(CT.Count - 10);
                    Conv_View.Plot.Add.Signal(history1.ToList(), 1, ScottPlot.Colors.Red);
                    ////}
                    ////else 
                    ////{
                    ////    (IR_View.Plot.PlottableList[1] as Signal).
                    ////}
                        //Conv_View.Plot.XAxis.Min = 0;
                    //Conv_View.Plot.XAxis.Max = CT.Count();
                    //Conv_View.Plot.YAxis.Max = history1.Max();
                }
                if (ID > 0)
                {
                    if (history1.Count != 0 && convmax == history2.Last()) return false;
                    history2.Enqueue(convmax);
                    if (history2.Count > 20) history2.Dequeue();
                    IR_View.Plot.Add.Scatter(t.ToList(), conv.ToList(), ScottPlot.Colors.Blue);
                    //IR_View.Plot.XAxis.Max = Math.Max(IR_View.Plot.XAxis.Max, 1);
                    //IR_View.Plot.YAxis.Max = Math.Max(IR_View.Plot.YAxis.Max, Math.Max(convmax, 0.3));
                    //Conv_View.Plot.YAxis.Max = Math.Max(Conv_View.Plot.YAxis.Max, history2.Max());
                    Conv_View.Plot.Add.Signal(history2.ToList(), 1, ScottPlot.Colors.Blue);
                }

                ScottPlot.Color Conv_Color = Colors.Gray;
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

                Conv_View.Plot.Add.Scatter(new double[2] { 0, 20 }, new double[2] { 0.2, 0.2 }, Conv_Color);
                Conv_View.Plot.Add.Scatter(new double[2] { 0, 20 }, new double[2] { 0.02, 0.02 }, Conv_Color);
                IR_View.Plot.Add.Scatter(t, new double[6] { 0.02, 0.02, 0.02, 0.02, 0.1, 0.1 }, Conv_Color);
                Conv_View.Plot.AutoScale();
                IR_View.Plot.AutoScale();
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
                    IR_View.Plot.Clear();
                    Conv_View.Plot.Clear();
                    IR_View.Plot.Add.Scatter(t, IR_p, Colors.Red);
                    IR_View.Plot.XAxis.Max = t.Last();
                    IR_View.Plot.YAxis.Max = Math.Max(IR_p.Max(), 0.3);
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
                    IR_View.Plot.Add.Scatter(t, IR_p, Colors.Blue);
                    IR_View.Plot.XAxis.Max = Math.Max(IR_View.Plot.XAxis.Max, t.Last());
                    IR_View.Plot.YAxis.Max = Math.Max(IR_View.Plot.YAxis.Max, Math.Max(IR_p.Max(), 0.3));
                    Conv_View.Plot.YAxis.Max = Math.Max(Conv_View.Plot.YAxis.Max, history2.Max());
                    Conv_View.Plot.Add.Scatter(CT.ToArray(), history2.ToArray(), Colors.Blue);
                }
                IR_View.Plot.XAxis.Label.Text = "Time (ms)";
                IR_View.Plot.YAxis.Label.Text = "Magnitude";

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

                IR_View.Plot.Add.Scatter(new double[2] { -10000, 10000 }, new double[2] { 0.1, 0.1 }, Conv_Color);
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
                Conclude.Text = "Concluding... Results may be inconclusive...";
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Conclude.Dispose();
                Conv_View.Dispose();
                IR_View.Dispose();
            }

            //#region IPanel methods
            //public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
            //{
            //    // Called when the panel tab is made visible, in Mac Rhino this will happen
            //    // for a document panel when a new document becomes active, the previous
            //    // documents panel will get hidden and the new current panel will get shown.
            //}

            //public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
            //{
            //    // Called when the panel tab is hidden, in Mac Rhino this will happen
            //    // for a document panel when a new document becomes active, the previous
            //    // documents panel will get hidden and the new current panel will get shown.
            //}

            //public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
            //{
            //    // Called when the document or panel container is closed/destroyed
            //}
            //#endregion IPanel methods
        }
    }
}