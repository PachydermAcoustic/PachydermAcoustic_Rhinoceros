using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pachyderm_Acoustic
{
    /// <summary>
    /// Debug tool which shows a 1 dimensional function/array of values graphically.
    /// </summary>
    partial class ProgressBox : Form
    {
        private ProgressBar Progress;

        public ProgressBox(string text)
        {
            InitializeComponent();
            this.Text = text;
        }

        /// <summary>
        /// Graphs and displays the contents of an array.
        /// </summary>
        /// <param name="array">The array to display.</param>
        /// <param name="WaitDuration">The amount of time the information is to be left on the screen.</param>
        public void Populate(int Prog_Percent)
        {
            if (this.Visible == false) return;
            Progress.Value = Prog_Percent;
            //Refresh();
            this.Progress.Value = Prog_Percent;
        }

        private void InitializeComponent()
        {
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(12, 12);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(411, 23);
            this.Progress.TabIndex = 17;
            // 
            // ProgressBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 44);
            this.Controls.Add(this.Progress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
    }

}
