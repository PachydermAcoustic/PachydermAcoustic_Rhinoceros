//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2018, Arthur van der Harten 
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
            internal System.Windows.Forms.TableLayoutPanel MainLayoutPanel;

            //Required by the Windows Form Designer
            private System.ComponentModel.IContainer components = null;
            //NOTE: The following procedure is required by the Windows Form Designer 
            //It can be modified using the Windows Form Designer. 
            //Do not modify it using the code editor. 
            [System.Diagnostics.DebuggerStepThrough()]
            private void InitializeComponent()
            {
                this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.title_box = new System.Windows.Forms.Label();
                this.Version_box = new System.Windows.Forms.Label();
                this.Copyright = new System.Windows.Forms.Label();
                this.Attribution = new System.Windows.Forms.Label();
                this.MainLayoutPanel.SuspendLayout();
                this.SuspendLayout();
                // 
                // MainLayoutPanel
                // 
                this.MainLayoutPanel.BackgroundImage = global::Pachyderm_Acoustic.Properties.Resources.logo1Splash;
                this.MainLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                this.MainLayoutPanel.ColumnCount = 2;
                this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 301F));
                this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 195F));
                this.MainLayoutPanel.Controls.Add(this.title_box, 1, 0);
                this.MainLayoutPanel.Controls.Add(this.Version_box, 1, 1);
                this.MainLayoutPanel.Controls.Add(this.Copyright, 1, 2);
                this.MainLayoutPanel.Controls.Add(this.Attribution, 1, 3);
                this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
                this.MainLayoutPanel.Name = "MainLayoutPanel";
                this.MainLayoutPanel.RowCount = 4;
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 179F));
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 218F));
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
                this.MainLayoutPanel.Size = new System.Drawing.Size(496, 302);
                this.MainLayoutPanel.TabIndex = 0;
                // 
                // title_box
                // 
                this.title_box.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.title_box.BackColor = System.Drawing.Color.Transparent;
                this.title_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.title_box.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                this.title_box.Location = new System.Drawing.Point(304, 9);
                this.title_box.Name = "title_box";
                this.title_box.Size = new System.Drawing.Size(189, 161);
                this.title_box.TabIndex = 2;
                this.title_box.Text = "Pachyderm-Acoustic\r\nGeometrical Acoustics for Rhinoceros";
                this.title_box.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                // 
                // Version_box
                // 
                this.Version_box.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.Version_box.BackColor = System.Drawing.Color.Transparent;
                this.Version_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Version_box.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                this.Version_box.Location = new System.Drawing.Point(304, 181);
                this.Version_box.Name = "Version_box";
                this.Version_box.Size = new System.Drawing.Size(189, 17);
                this.Version_box.TabIndex = 1;
                this.Version_box.Text = "version:";
                this.Version_box.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
                // 
                // Copyright
                // 
                this.Copyright.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.Copyright.BackColor = System.Drawing.Color.Transparent;
                this.Copyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Copyright.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                this.Copyright.Location = new System.Drawing.Point(304, 205);
                this.Copyright.Name = "Copyright";
                this.Copyright.Size = new System.Drawing.Size(189, 15);
                this.Copyright.TabIndex = 3;
                this.Copyright.Text = "copyright Arthur van der Harten";
                this.Copyright.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
                // 
                // Attribution
                // 
                this.Attribution.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.Attribution.BackColor = System.Drawing.Color.Transparent;
                this.Attribution.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Attribution.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                this.Attribution.Location = new System.Drawing.Point(304, 240);
                this.Attribution.Name = "Attribution";
                this.Attribution.Size = new System.Drawing.Size(189, 45);
                this.Attribution.TabIndex = 4;
                this.Attribution.Text = "Developed under the guidance of Paul Calamia at Rensselaer Polytechnic Institute";
                this.Attribution.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
                // 
                // Pach_Splash
                // 
                this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(496, 300);
                this.ControlBox = false;
                this.Controls.Add(this.MainLayoutPanel);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "Pach_Splash";
                this.ShowInTaskbar = false;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.MainLayoutPanel.ResumeLayout(false);
                this.ResumeLayout(false);

            }
            internal System.Windows.Forms.Label Version_box;
            internal System.Windows.Forms.Label title_box;
            internal System.Windows.Forms.Label Copyright;
            internal System.Windows.Forms.Label Attribution;
        }
    }
}