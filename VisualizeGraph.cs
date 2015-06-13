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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Pachyderm_Acoustic.UI;
using ZedGraph;

namespace Pachyderm_Acoustic
{
    /// <summary>
    /// Debug tool which shows a 1 dimensional function/array of values graphically.
    /// </summary>
    partial class VisualizationBox : Form
    {
        private ProgressBar Progress;
        private ZedGraphControl Graph;
        private IContainer components;

        public VisualizationBox()
        {
            InitializeComponent();
            this.Text = String.Format("Displaying Data");
        }

        public VisualizationBox(float minx, float maxx, float miny, float maxy)
        {
            InitializeComponent();
            this.Text = String.Format("Displaying Data");
            Graph.GraphPane.XAxis.Scale.Min = minx;
            Graph.GraphPane.XAxis.Scale.Max = maxx;
            Graph.GraphPane.YAxis.Scale.Min = miny;
            Graph.GraphPane.YAxis.Scale.Max = maxy;            
        }

        /// <summary>
        /// Graphs and displays the contents of an array.
        /// </summary>
        /// <param name="array">The array to display.</param>
        /// <param name="WaitDuration">The amount of time the information is to be left on the screen.</param>
        public void Populate(double[] arrayx, double[]arrayy, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            if (this.Visible == false) return;
            Graph.GraphPane.CurveList.Clear();
            Graph.GraphPane.XAxis.Title = new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.YAxis.Title = new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.AddCurve("Data", arrayx, arrayy, System.Drawing.Color.Red);
            //Graph.AxisChange();
            //Graph.Invalidate();
            //this.InvokePaint(this, null);
            Progress.Value = Prog_Percent;
            Refresh();
            if (WaitDuration != 0) System.Threading.Thread.Sleep(WaitDuration);
            this.Progress.Value = Prog_Percent;
        }

        public void Populate(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            if (this.Visible == false) return;
            Graph.GraphPane.CurveList.Clear();
            Graph.GraphPane.XAxis.Title = new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.YAxis.Title = new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.AddCurve("Data", arrayx, arrayy, System.Drawing.Color.Red, SymbolType.None);
            Graph.GraphPane.AddCurve("Data", arrayx2, arrayy2, System.Drawing.Color.Blue, SymbolType.None);
            //Graph.AxisChange();
            //Graph.Invalidate();
            //this.InvokePaint(this, null);
            Progress.Value = Prog_Percent;
            Refresh();
            System.Threading.Thread.Sleep(WaitDuration);
        }
        public void Populate(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, double[] arrayx3, double[] arrayy3, double[] arrayx4, string xdom, string ydom, double[] arrayy4, int WaitDuration, int Prog_Percent)
        {
            if (this.Visible == false) return;
            Graph.GraphPane.CurveList.Clear();
            Graph.GraphPane.XAxis.Title = new AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.YAxis.Title = new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.AddCurve("Data", arrayx, arrayy, System.Drawing.Color.Red, SymbolType.None);
            Graph.GraphPane.AddCurve("Data", arrayx2, arrayy2, System.Drawing.Color.Blue, SymbolType.None);
            Graph.GraphPane.AddCurve("Data", arrayx3, arrayy3, System.Drawing.Color.Red, SymbolType.None);
            Graph.GraphPane.AddCurve("Data", arrayx4, arrayy4, System.Drawing.Color.Blue, SymbolType.None);
            //Graph.AxisChange();
            //Graph.Invalidate();
            //this.InvokePaint(this, null);
            Progress.Value = Prog_Percent;
            Refresh();
            System.Threading.Thread.Sleep(WaitDuration);
        }
        public void Populate(double[] arrayx, double[] arrayy, double[] arrayx2, double[] arrayy2, double[] arrayx3, double[] arrayy3, string xdom, string ydom, int WaitDuration, int Prog_Percent)
        {
            if (this.Visible == false) return;
            Graph.GraphPane.CurveList.Clear();
            Graph.GraphPane.XAxis.Title = new  AxisLabel(xdom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.YAxis.Title = new AxisLabel(ydom, "Arial", 10, System.Drawing.Color.Black, false, false, false);
            Graph.GraphPane.AddCurve("Data", arrayx, arrayy, System.Drawing.Color.Red, SymbolType.None);
            Graph.GraphPane.AddCurve("Data", arrayx2, arrayy2, System.Drawing.Color.Blue, SymbolType.None);
            Graph.GraphPane.AddCurve("Data", arrayx3, arrayy3, System.Drawing.Color.Blue, SymbolType.None);
            //Graph.AxisChange();
            //Graph.Invalidate();
            //this.InvokePaint(this, null);
            Progress.Value = Prog_Percent;
            Refresh();
            System.Threading.Thread.Sleep(WaitDuration);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Graph = new ZedGraph.ZedGraphControl();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Graph
            // 
            this.Graph.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.Graph.Location = new System.Drawing.Point(12, 12);
            this.Graph.Name = "Graph";
            this.Graph.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.Graph.ScrollGrace = 0D;
            this.Graph.ScrollMaxX = 0D;
            this.Graph.ScrollMaxY = 0D;
            this.Graph.ScrollMaxY2 = 0D;
            this.Graph.ScrollMinX = 0D;
            this.Graph.ScrollMinY = 0D;
            this.Graph.ScrollMinY2 = 0D;
            this.Graph.Size = new System.Drawing.Size(411, 259);
            this.Graph.TabIndex = 16;
            // 
            // Progress
            // 
            // 
            // progressBar1
            // 
            this.Progress.Location = new System.Drawing.Point(13, 278);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(410, 23);
            this.Progress.TabIndex = 17;
            // 
            // VisualizationBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 308);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.Graph);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisualizationBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutBox1";
            this.ResumeLayout(false);

        }
    }
}
