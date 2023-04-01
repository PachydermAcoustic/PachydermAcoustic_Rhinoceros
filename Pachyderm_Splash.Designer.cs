//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2019, Arthur van der Harten 
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

//[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
namespace Pachyderm_Acoustic
{
    namespace UI
    {
        partial class Pach_Splash : System.Windows.Forms.Form
        {
            //Form overrides dispose to clean up the component list. 
            [System.Diagnostics.DebuggerNonUserCode()]
            protected override void Dispose(bool disposing)
            {
                try
                {
                    if (disposing && components != null)
                    {
                        components.Dispose();
                    }
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }

            //Required by the Windows Form Designer
            private System.ComponentModel.IContainer components = null;
            //NOTE: The following procedure is required by the Windows Form Designer 
            //It can be modified using the Windows Form Designer. 
            //Do not modify it using the code editor. 
            [System.Diagnostics.DebuggerStepThrough()]
            private void InitializeComponent()
            {
            this.Attribution = new System.Windows.Forms.Label();
            this.Copyright = new System.Windows.Forms.Label();
            this.Version_box = new System.Windows.Forms.Label();
            this.title_box = new System.Windows.Forms.Label();
            this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Attribution
            // 
            this.Attribution.BackColor = System.Drawing.Color.Transparent;
            this.Attribution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Attribution.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Attribution.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Attribution.Location = new System.Drawing.Point(556, 421);
            this.Attribution.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Attribution.Name = "Attribution";
            this.Attribution.Size = new System.Drawing.Size(430, 152);
            this.Attribution.TabIndex = 4;
            this.Attribution.Text = "A community project serving foundational algorithms for practical application in " +
    "the development of the built environment and the global soundscape.";
            this.Attribution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Copyright
            // 
            this.Copyright.BackColor = System.Drawing.Color.Transparent;
            this.Copyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Copyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Copyright.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Copyright.Location = new System.Drawing.Point(556, 262);
            this.Copyright.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Copyright.Name = "Copyright";
            this.Copyright.Size = new System.Drawing.Size(430, 159);
            this.Copyright.TabIndex = 3;
            this.Copyright.Text = "copyright:\r\nOpen Research in Acoustical\r\nScience and Education, Inc.\r\n(a 501(c)3 " +
    "non-profit organization.)";
            this.Copyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Version_box
            // 
            this.Version_box.BackColor = System.Drawing.Color.Transparent;
            this.Version_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Version_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Version_box.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Version_box.Location = new System.Drawing.Point(556, 222);
            this.Version_box.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Version_box.Name = "Version_box";
            this.Version_box.Size = new System.Drawing.Size(430, 40);
            this.Version_box.TabIndex = 1;
            this.Version_box.Text = "version:";
            this.Version_box.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // title_box
            // 
            this.title_box.BackColor = System.Drawing.Color.Transparent;
            this.title_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.title_box.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.title_box.Location = new System.Drawing.Point(556, 0);
            this.title_box.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.title_box.Name = "title_box";
            this.title_box.Size = new System.Drawing.Size(430, 222);
            this.title_box.TabIndex = 2;
            this.title_box.Text = "Pachyderm\r\nAcoustic Simulation:\r\nFoundational Simulation\r\nin Acoustics";
            this.title_box.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainLayoutPanel
            // 
            this.MainLayoutPanel.BackgroundImage = global::Pachyderm_Acoustic.Properties.Resources.LogoOSplash;
            this.MainLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.MainLayoutPanel.ColumnCount = 2;
            this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 550F));
            this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 442F));
            this.MainLayoutPanel.Controls.Add(this.Version_box, 1, 1);
            this.MainLayoutPanel.Controls.Add(this.Attribution, 1, 3);
            this.MainLayoutPanel.Controls.Add(this.title_box, 1, 0);
            this.MainLayoutPanel.Controls.Add(this.Copyright, 1, 2);
            this.MainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainLayoutPanel.Margin = new System.Windows.Forms.Padding(6);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
            this.MainLayoutPanel.RowCount = 4;
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 222F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 419F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.MainLayoutPanel.Size = new System.Drawing.Size(992, 573);
            this.MainLayoutPanel.TabIndex = 0;
            // 
            // Pach_Splash
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 573);
            this.ControlBox = false;
            this.Controls.Add(this.MainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Pach_Splash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MainLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

            }

            internal System.Windows.Forms.Label Attribution;
            internal System.Windows.Forms.Label Copyright;
            internal System.Windows.Forms.Label Version_box;
            internal System.Windows.Forms.Label title_box;
            internal System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
        }
    }
}