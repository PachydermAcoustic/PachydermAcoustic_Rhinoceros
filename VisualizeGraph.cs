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
using System.Linq;
using Eto.Forms;
using ScottPlot.DataSources;
using ScottPlot.Plottables;

namespace Pachyderm_Acoustic
{
    /// <summary>
    /// Debug tool which shows a 1 dimensional function/array of values graphically.
    /// </summary>
    public class VisualizationBox : Eto.Forms.FloatingForm, Pach_Graphics.IReporting
    {
        private ProgressBar Progress;
        private ScottPlot.Eto.EtoPlot Graph;

        double[] arrayx, arrayy, arrayx2, arrayy2, arrayx3, arrayy3, arrayx4, arrayy4;
        string xdom, ydom;
        int Prog_Percent;

        Scatter sd1, sd2, sd3, sd4;
        //Eto.Forms.Form _parent;
        System.Threading.Thread UI;

        public VisualizationBox(Eto.Forms.Form parent)
        {
            //_parent = parent;
            UI = System.Threading.Thread.CurrentThread;
            InitializeComponent();

            Graph.Plot.XLabel(xdom, 12);//new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.Plot.YLabel(ydom, 12);//new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            ScottPlot.DataSources.ScatterSourceDoubleArray sd1 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx, arrayy);
            this.sd1 = Graph.Plot.Add.Scatter(sd1, ScottPlot.Colors.Red);
            this.sd1.MarkerStyle.Shape = ScottPlot.MarkerShape.None;
            ScottPlot.DataSources.ScatterSourceDoubleArray sd2 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx2, arrayy2);
            this.sd2 = Graph.Plot.Add.Scatter(sd2, ScottPlot.Colors.Orange);
            this.sd2.MarkerStyle.Shape = ScottPlot.MarkerShape.None;
            ScottPlot.DataSources.ScatterSourceDoubleArray sd3 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx3, arrayy3);
            this.sd3 = Graph.Plot.Add.Scatter(sd3, ScottPlot.Colors.Green);
            this.sd3.MarkerStyle.Shape = ScottPlot.MarkerShape.None;
            ScottPlot.DataSources.ScatterSourceDoubleArray sd4 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx4, arrayy4);
            this.sd4 = Graph.Plot.Add.Scatter(sd4, ScottPlot.Colors.Blue);
            this.sd4.MarkerStyle.Shape = ScottPlot.MarkerShape.None;

            this.Title = "Displaying Data";
        }

        public VisualizationBox(float minx, float maxx, float miny, float maxy)
        {
            InitializeComponent();
            this.Title = "Displaying Data";
            Graph.Plot.Axes.SetLimits(minx, maxx, miny, maxy);
        }

        /// <summary>
        /// Graphs and displays the contents of an array.
        /// </summary>
        /// <param name="array">The array to display.</param>
        /// <param name="WaitDuration">The amount of time the information is to be left on the screen.</param>
        public void Populate(double[] arrayx, double[]arrayy, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            if (this.Visible == false) return;
            Graph.Plot.Clear();
            Graph.Plot.XLabel(xdom, 12);//new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.Plot.YLabel(ydom, 12);//new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);

            ScottPlot.DataSources.ScatterSourceDoubleArray sd = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx, arrayy);
            Graph.Plot.Add.Scatter(sd, ScottPlot.Colors.Red);
            Graph.Plot.Axes.AutoScale();
            Progress.Value = Prog_Percent;

            if (WaitDuration != 0) System.Threading.Thread.Sleep(WaitDuration);
            this.Progress.Value = Prog_Percent;
        }

        public void Populate(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            if (this.Visible == false) return;
            Graph.Plot.Clear();
            Graph.Plot.XLabel(xdom, 12);//new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.Plot.YLabel(ydom, 12);//new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            ScottPlot.DataSources.ScatterSourceDoubleArray sd1 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx, arrayy);
            this.sd1 = Graph.Plot.Add.Scatter(sd1, ScottPlot.Colors.Red);
            ScottPlot.DataSources.ScatterSourceDoubleArray sd2 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx2, arrayy2);
            this.sd2 = Graph.Plot.Add.Scatter(sd2, ScottPlot.Colors.Orange);
            Graph.Plot.Axes.AutoScale();

            Progress.Value = Prog_Percent;
            System.Threading.Thread.Sleep(WaitDuration);
        }

        public void Populate()
        {
            Graph.Update(new Eto.Drawing.Rectangle(Graph.Size));
        }

        public void Populate(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, double[] arrayx3, double[] arrayy3, double[] arrayx4, double[] arrayy4, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            if (this.Visible == false) return;
            //Graph.Plot.Clear();
            Graph.Plot.XLabel(xdom, 12);//new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.Plot.YLabel(ydom, 12);//new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            ScottPlot.DataSources.ScatterSourceDoubleArray sd1 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx, arrayy);
            this.sd1 = Graph.Plot.Add.Scatter(sd1, ScottPlot.Colors.Red);
            ScottPlot.DataSources.ScatterSourceDoubleArray sd2 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx2, arrayy2);
            this.sd2 = Graph.Plot.Add.Scatter(sd2, ScottPlot.Colors.Orange);
            ScottPlot.DataSources.ScatterSourceDoubleArray sd3 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx3, arrayy3);
            this.sd3 = Graph.Plot.Add.Scatter(sd3, ScottPlot.Colors.Green);
            ScottPlot.DataSources.ScatterSourceDoubleArray sd4 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx4, arrayy4);
            this.sd4 = Graph.Plot.Add.Scatter(sd4, ScottPlot.Colors.Blue);
            Graph.Plot.Axes.AutoScale();
            //Graph.Plot.Render
            Graph.Refresh();

            Progress.Value = Prog_Percent;

            this.Invalidate();
            //this.Show();
            //System.Threading.Thread.Sleep(WaitDuration);
        }

        //public delegate void data_pass(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, double[] arrayx3, double[] arrayy3, double[] arrayx4, double[] arrayy4, string xdom, string ydom, int WaitDuration, int Prog_Percent);
        //public event data_pass On_Fill;

        public void Fill(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, double[] arrayx3, double[] arrayy3, double[] arrayx4, double[] arrayy4, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            Graph.Plot.XLabel(xdom, 12);//new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.Plot.YLabel(ydom, 12);//new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);

            //if (this.arrayx != null && arrayx.Length == this.arrayx.Length)
            //{
            //    for (int i = 0; i < arrayx.Length; i++)
            //    {
            //        this.arrayx[i] = arrayx[i];
            //        this.arrayy[i] = arrayy[i];
            //        this.arrayx2[i] = arrayx2[i];
            //        this.arrayy2[i] = arrayy2[i];
            //        this.arrayx3[i] = arrayx3[i];
            //        this.arrayy3[i] = arrayy3[i];
            //        this.arrayx4[i] = arrayx4[i];
            //        this.arrayy4[i] = arrayy4[i];
            //        this.xdom = xdom;
            //        this.ydom = ydom;
            //        this.Prog_Percent = Prog_Percent;
            //    }
            //}
            //else 
            //{
            Graph.Plot.Clear();

                this.arrayx = arrayx;
                this.arrayy = arrayy;
                this.arrayx2 = arrayx2;
                this.arrayy2 = arrayy2;
                this.arrayx3 = arrayx3;
                this.arrayy3 = arrayy3;
                this.arrayx4 = arrayx4;
                this.arrayy4 = arrayy4;

                ScottPlot.DataSources.ScatterSourceDoubleArray sd1 = new ScottPlot.DataSources.ScatterSourceDoubleArray(this.arrayx, this.arrayy);
                this.sd1 = Graph.Plot.Add.Scatter(sd1, ScottPlot.Colors.Red);
                if (arrayx2 != null)
                {
                    ScottPlot.DataSources.ScatterSourceDoubleArray sd2 = new ScottPlot.DataSources.ScatterSourceDoubleArray(this.arrayx2, this.arrayy2);
                    this.sd2 = Graph.Plot.Add.Scatter(sd2, ScottPlot.Colors.Orange);
                }
                if (arrayx3 != null)
                {
                    ScottPlot.DataSources.ScatterSourceDoubleArray sd3 = new ScottPlot.DataSources.ScatterSourceDoubleArray(this.arrayx3, this.arrayy3);
                    this.sd3 = Graph.Plot.Add.Scatter(sd3, ScottPlot.Colors.Green);
                }
                if (arrayx4 != null)
                {
                    ScottPlot.DataSources.ScatterSourceDoubleArray sd4 = new ScottPlot.DataSources.ScatterSourceDoubleArray(this.arrayx4, this.arrayy4);
                    this.sd4 = Graph.Plot.Add.Scatter(sd4, ScottPlot.Colors.Blue);
                }

            //Graph.Plot.Render();
            Graph.Refresh();
        }

        public void Populate(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, double[] arrayx3, double[] arrayy3, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            Graph.Refresh();
            //if (this.Visible == false) return;
            //Graph.Plot.Clear();
            //Graph.Plot.XAxis.Label.Text = xdom;//new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            //Graph.Plot.YAxis.Label.Text = ydom;//new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            //ScottPlot.DataSources.ScatterSourceDoubleArray sd1 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx.ToList(), arrayy.ToList());
            //this.sd1 = Graph.Plot.Add.Scatter(sd1, ScottPlot.Colors.Red);
            //ScottPlot.DataSources.ScatterSourceDoubleArray sd2 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx2.ToList(), arrayy2.ToList());
            //this.sd2 = Graph.Plot.Add.Scatter(sd2, ScottPlot.Colors.Green);
            //ScottPlot.DataSources.ScatterSourceDoubleArray sd3 = new ScottPlot.DataSources.ScatterSourceDoubleArray(arrayx3.ToList(), arrayy3.ToList());
            //this.sd3 = Graph.Plot.Add.Scatter(sd1, ScottPlot.Colors.Blue);
            //Graph.Plot.AutoScale();

            //Progress.Value = Prog_Percent;

            //this.Invalidate();
            //System.Threading.Thread.Sleep(WaitDuration);
        }

        private void InitializeComponent()
        {
            this.Size = new Eto.Drawing.Size(300, 350);
            this.Graph = new ScottPlot.Eto.EtoPlot();
            Graph.Size = new Eto.Drawing.Size(300, 250);
            this.Progress = new ProgressBar();
            this.Progress.Size = new Eto.Drawing.Size(Size.Width, 100);

            DynamicLayout all = new DynamicLayout();
            all.AddRow(Graph);
            all.AddRow(Progress);
            this.Content = all;
        }
    }
}
