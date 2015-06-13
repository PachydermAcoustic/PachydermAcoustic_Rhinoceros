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

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        partial class Pach_MapCustom
        {
            /// <summary> 
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary> 
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Component Designer generated code

            /// <summary> 
            /// Required method for Designer support - do not modify 
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.label22 = new System.Windows.Forms.Label();
                this.groupBox5 = new System.Windows.Forms.GroupBox();
                this.label3 = new System.Windows.Forms.Label();
                this.label20 = new System.Windows.Forms.Label();
                this.label24 = new System.Windows.Forms.Label();
                this.label23 = new System.Windows.Forms.Label();
                this.Discretize = new System.Windows.Forms.CheckBox();
                this.groupBox1 = new System.Windows.Forms.GroupBox();
                this.End_Time_Control = new System.Windows.Forms.NumericUpDown();
                this.Start_Time_Control = new System.Windows.Forms.NumericUpDown();
                this.label2 = new System.Windows.Forms.Label();
                this.label4 = new System.Windows.Forms.Label();
                this.label5 = new System.Windows.Forms.Label();
                this.label6 = new System.Windows.Forms.Label();
                this.Param1_4 = new System.Windows.Forms.Label();
                this.Param1_2 = new System.Windows.Forms.Label();
                this.Param_Min = new System.Windows.Forms.NumericUpDown();
                this.Param_Max = new System.Windows.Forms.NumericUpDown();
                this.Param3_4 = new System.Windows.Forms.Label();
                this.Param_Scale = new System.Windows.Forms.PictureBox();
                this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.openDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.saveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.Plot_Values = new System.Windows.Forms.Button();
                this.Calculate_Map = new System.Windows.Forms.Button();
                this.Color_Selection = new System.Windows.Forms.ComboBox();
                this.MeshList = new System.Windows.Forms.ListBox();
                this.groupBox5.SuspendLayout();
                this.groupBox1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.End_Time_Control)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.Start_Time_Control)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).BeginInit();
                this.menuStrip1.SuspendLayout();
                this.SuspendLayout();
                // 
                // label22
                // 
                this.label22.AutoSize = true;
                this.label22.Location = new System.Drawing.Point(4, 96);
                this.label22.Name = "label22";
                this.label22.Size = new System.Drawing.Size(117, 13);
                this.label22.TabIndex = 88;
                this.label22.Text = "Octave Band Selection";
                // 
                // groupBox5
                // 
                this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.groupBox5.Controls.Add(this.label3);
                this.groupBox5.Controls.Add(this.label20);
                this.groupBox5.Location = new System.Drawing.Point(3, 142);
                this.groupBox5.Name = "groupBox5";
                this.groupBox5.Size = new System.Drawing.Size(221, 66);
                this.groupBox5.TabIndex = 87;
                this.groupBox5.TabStop = false;
                this.groupBox5.Text = "Time Interval";
                // 
                // label3
                // 
                this.label3.AutoSize = true;
                this.label3.Location = new System.Drawing.Point(2, 43);
                this.label3.Name = "label3";
                this.label3.Size = new System.Drawing.Size(74, 13);
                this.label3.TabIndex = 30;
                this.label3.Text = "End Time (ms)";
                // 
                // label20
                // 
                this.label20.AutoSize = true;
                this.label20.Location = new System.Drawing.Point(2, 19);
                this.label20.Name = "label20";
                this.label20.Size = new System.Drawing.Size(77, 13);
                this.label20.TabIndex = 29;
                this.label20.Text = "Start Time (ms)";
                // 
                // label24
                // 
                this.label24.AutoSize = true;
                this.label24.Location = new System.Drawing.Point(3, 49);
                this.label24.Name = "label24";
                this.label24.Size = new System.Drawing.Size(102, 13);
                this.label24.TabIndex = 86;
                this.label24.Text = "Parameter Selection";
                // 
                // label23
                // 
                this.label23.AutoSize = true;
                this.label23.Location = new System.Drawing.Point(3, 5);
                this.label23.Name = "label23";
                this.label23.Size = new System.Drawing.Size(78, 13);
                this.label23.TabIndex = 84;
                this.label23.Text = "Color Selection";
                // 
                // Discretize
                // 
                this.Discretize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.Discretize.AutoSize = true;
                this.Discretize.Location = new System.Drawing.Point(26, 233);
                this.Discretize.Name = "Discretize";
                this.Discretize.Size = new System.Drawing.Size(104, 17);
                this.Discretize.TabIndex = 91;
                this.Discretize.Text = "Discretize Colors";
                this.Discretize.UseVisualStyleBackColor = true;
                this.Discretize.CheckedChanged += new System.EventHandler(this.Discretize_CheckedChanged);
                // 
                // groupBox1
                // 
                this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.groupBox1.Controls.Add(this.End_Time_Control);
                this.groupBox1.Controls.Add(this.Start_Time_Control);
                this.groupBox1.Controls.Add(this.label2);
                this.groupBox1.Controls.Add(this.label4);
                this.groupBox1.Enabled = false;
                this.groupBox1.Location = new System.Drawing.Point(10, 166);
                this.groupBox1.Name = "groupBox1";
                this.groupBox1.Size = new System.Drawing.Size(246, 66);
                this.groupBox1.TabIndex = 95;
                this.groupBox1.TabStop = false;
                this.groupBox1.Text = "Time Interval";
                // 
                // End_Time_Control
                // 
                this.End_Time_Control.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.End_Time_Control.Location = new System.Drawing.Point(101, 38);
                this.End_Time_Control.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
                this.End_Time_Control.Name = "End_Time_Control";
                this.End_Time_Control.Size = new System.Drawing.Size(139, 20);
                this.End_Time_Control.TabIndex = 40;
                this.End_Time_Control.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
                // 
                // Start_Time_Control
                // 
                this.Start_Time_Control.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
                this.Start_Time_Control.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Start_Time_Control.Location = new System.Drawing.Point(101, 12);
                this.Start_Time_Control.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
                this.Start_Time_Control.Name = "Start_Time_Control";
                this.Start_Time_Control.Size = new System.Drawing.Size(139, 20);
                this.Start_Time_Control.TabIndex = 39;
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(2, 43);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(74, 13);
                this.label2.TabIndex = 30;
                this.label2.Text = "End Time (ms)";
                // 
                // label4
                // 
                this.label4.AutoSize = true;
                this.label4.Location = new System.Drawing.Point(2, 19);
                this.label4.Name = "label4";
                this.label4.Size = new System.Drawing.Size(77, 13);
                this.label4.TabIndex = 29;
                this.label4.Text = "Start Time (ms)";
                // 
                // label5
                // 
                this.label5.AutoSize = true;
                this.label5.Location = new System.Drawing.Point(10, 71);
                this.label5.Name = "label5";
                this.label5.Size = new System.Drawing.Size(51, 13);
                this.label5.TabIndex = 94;
                this.label5.Text = "Selection";
                // 
                // label6
                // 
                this.label6.AutoSize = true;
                this.label6.Location = new System.Drawing.Point(10, 27);
                this.label6.Name = "label6";
                this.label6.Size = new System.Drawing.Size(78, 13);
                this.label6.TabIndex = 92;
                this.label6.Text = "Color Selection";
                // 
                // Param1_4
                // 
                this.Param1_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Param1_4.AutoSize = true;
                this.Param1_4.Location = new System.Drawing.Point(263, 187);
                this.Param1_4.Name = "Param1_4";
                this.Param1_4.Size = new System.Drawing.Size(28, 13);
                this.Param1_4.TabIndex = 103;
                this.Param1_4.Text = "22.5";
                // 
                // Param1_2
                // 
                this.Param1_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Param1_2.AutoSize = true;
                this.Param1_2.Location = new System.Drawing.Point(263, 142);
                this.Param1_2.Name = "Param1_2";
                this.Param1_2.Size = new System.Drawing.Size(19, 13);
                this.Param1_2.TabIndex = 102;
                this.Param1_2.Text = "45";
                // 
                // Param_Min
                // 
                this.Param_Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.Param_Min.DecimalPlaces = 1;
                this.Param_Min.Location = new System.Drawing.Point(262, 230);
                this.Param_Min.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
                this.Param_Min.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
                this.Param_Min.Name = "Param_Min";
                this.Param_Min.Size = new System.Drawing.Size(51, 20);
                this.Param_Min.TabIndex = 101;
                // 
                // Param_Max
                // 
                this.Param_Max.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                this.Param_Max.DecimalPlaces = 1;
                this.Param_Max.Location = new System.Drawing.Point(262, 45);
                this.Param_Max.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
                this.Param_Max.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
                this.Param_Max.Name = "Param_Max";
                this.Param_Max.Size = new System.Drawing.Size(51, 20);
                this.Param_Max.TabIndex = 100;
                this.Param_Max.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
                // 
                // Param3_4
                // 
                this.Param3_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Param3_4.AutoSize = true;
                this.Param3_4.Location = new System.Drawing.Point(263, 95);
                this.Param3_4.Name = "Param3_4";
                this.Param3_4.Size = new System.Drawing.Size(28, 13);
                this.Param3_4.TabIndex = 99;
                this.Param3_4.Text = "67.5";
                // 
                // Param_Scale
                // 
                this.Param_Scale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Param_Scale.Location = new System.Drawing.Point(319, 46);
                this.Param_Scale.Name = "Param_Scale";
                this.Param_Scale.Size = new System.Drawing.Size(82, 204);
                this.Param_Scale.TabIndex = 98;
                this.Param_Scale.TabStop = false;
                // 
                // menuStrip1
                // 
                this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
                this.menuStrip1.Location = new System.Drawing.Point(0, 0);
                this.menuStrip1.Name = "menuStrip1";
                this.menuStrip1.Size = new System.Drawing.Size(405, 24);
                this.menuStrip1.TabIndex = 104;
                this.menuStrip1.Text = "menuStrip1";
                // 
                // fileToolStripMenuItem
                // 
                this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDataToolStripMenuItem,
            this.saveDataToolStripMenuItem});
                this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
                this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
                this.fileToolStripMenuItem.Text = "File";
                // 
                // saveDataToolStripMenuItem
                // 
                this.saveDataToolStripMenuItem.Name = "saveDataToolStripMenuItem";
                this.saveDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.saveDataToolStripMenuItem.Text = "Save Data...";
                this.saveDataToolStripMenuItem.Click += new System.EventHandler(this.SaveDataToolStripMenuItem_Click);
                // 
                // Plot_Values
                // 
                this.Plot_Values.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Plot_Values.Location = new System.Drawing.Point(12, 291);
                this.Plot_Values.Margin = new System.Windows.Forms.Padding(2);
                this.Plot_Values.Name = "Plot_Values";
                this.Plot_Values.Size = new System.Drawing.Size(389, 32);
                this.Plot_Values.TabIndex = 106;
                this.Plot_Values.Text = "Plot Numerical Values";
                this.Plot_Values.UseVisualStyleBackColor = true;
                this.Plot_Values.Click += new System.EventHandler(this.Plot_Values_Click);
                // 
                // Calculate_Map
                // 
                this.Calculate_Map.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Calculate_Map.Location = new System.Drawing.Point(12, 255);
                this.Calculate_Map.Margin = new System.Windows.Forms.Padding(2);
                this.Calculate_Map.Name = "Calculate_Map";
                this.Calculate_Map.Size = new System.Drawing.Size(389, 32);
                this.Calculate_Map.TabIndex = 105;
                this.Calculate_Map.Text = "Create Map";
                this.Calculate_Map.UseVisualStyleBackColor = true;
                this.Calculate_Map.Click += new System.EventHandler(this.Create_Map_Click);
                // 
                // Color_Selection
                // 
                this.Color_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.Color_Selection.FormattingEnabled = true;
                this.Color_Selection.Items.AddRange(new object[] {
            "R-O-Y-G-B-I-V",
            "R-O-Y",
            "Y-G",
            "R-M-B",
            "W-B",
            "MEEP"});
                this.Color_Selection.Location = new System.Drawing.Point(12, 45);
                this.Color_Selection.Name = "Color_Selection";
                this.Color_Selection.Size = new System.Drawing.Size(244, 21);
                this.Color_Selection.TabIndex = 90;
                this.Color_Selection.Text = "R-O-Y-G-B-I-V";
                this.Color_Selection.SelectedIndexChanged += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
                // 
                // MeshList
                // 
                this.MeshList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.MeshList.FormattingEnabled = true;
                this.MeshList.Location = new System.Drawing.Point(12, 88);
                this.MeshList.Name = "MeshList";
                this.MeshList.Size = new System.Drawing.Size(244, 69);
                this.MeshList.TabIndex = 107;
                // 
                // Pach_MapCustom
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.MeshList);
                this.Controls.Add(this.Plot_Values);
                this.Controls.Add(this.Calculate_Map);
                this.Controls.Add(this.menuStrip1);
                this.Controls.Add(this.Param1_4);
                this.Controls.Add(this.Param1_2);
                this.Controls.Add(this.Param_Min);
                this.Controls.Add(this.Param_Max);
                this.Controls.Add(this.Param3_4);
                this.Controls.Add(this.Param_Scale);
                this.Controls.Add(this.Discretize);
                this.Controls.Add(this.groupBox1);
                this.Controls.Add(this.label5);
                this.Controls.Add(this.label6);
                this.Controls.Add(this.Color_Selection);
                this.MaximumSize = new System.Drawing.Size(800, 329);
                this.MinimumSize = new System.Drawing.Size(405, 329);
                this.Name = "Pach_MapCustom";
                this.Size = new System.Drawing.Size(405, 329);
                this.groupBox5.ResumeLayout(false);
                this.groupBox5.PerformLayout();
                this.groupBox1.ResumeLayout(false);
                this.groupBox1.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.End_Time_Control)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.Start_Time_Control)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).EndInit();
                this.menuStrip1.ResumeLayout(false);
                this.menuStrip1.PerformLayout();
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.Label label22;
            internal System.Windows.Forms.GroupBox groupBox5;
            internal System.Windows.Forms.Label label3;
            internal System.Windows.Forms.Label label20;
            private System.Windows.Forms.Label label24;
            private System.Windows.Forms.Label label23;
            private System.Windows.Forms.CheckBox Discretize;
            internal System.Windows.Forms.GroupBox groupBox1;
            private System.Windows.Forms.NumericUpDown End_Time_Control;
            private System.Windows.Forms.NumericUpDown Start_Time_Control;
            internal System.Windows.Forms.Label label2;
            internal System.Windows.Forms.Label label4;
            private System.Windows.Forms.Label label5;
            private System.Windows.Forms.Label label6;
            private System.Windows.Forms.Label Param1_4;
            private System.Windows.Forms.Label Param1_2;
            private System.Windows.Forms.NumericUpDown Param_Min;
            private System.Windows.Forms.NumericUpDown Param_Max;
            private System.Windows.Forms.Label Param3_4;
            private System.Windows.Forms.PictureBox Param_Scale;
            private System.Windows.Forms.MenuStrip menuStrip1;
            private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem openDataToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveDataToolStripMenuItem;
            internal System.Windows.Forms.Button Plot_Values;
            internal System.Windows.Forms.Button Calculate_Map;
            internal System.Windows.Forms.ComboBox Color_Selection;
            private System.Windows.Forms.ListBox MeshList;
        }
    }
}