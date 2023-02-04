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

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        partial class Pach_Visual_Control : System.Windows.Forms.UserControl
        {

            //UserControl overrides dispose to clean up the component list. 
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
            this.FromPointInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DirectionalSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromMeshSphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectASphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromSphereObjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Forw = new System.Windows.Forms.Button();
            this.Rev = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.SourceSelection = new System.Windows.Forms.ComboBox();
            this.RoomSelection = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Time_Preview = new System.Windows.Forms.Label();
            this.Octave = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.Color_Selection = new System.Windows.Forms.ComboBox();
            this.Param1_4 = new System.Windows.Forms.Label();
            this.Param1_2 = new System.Windows.Forms.Label();
            this.Param_Min = new System.Windows.Forms.NumericUpDown();
            this.Param_Max = new System.Windows.Forms.NumericUpDown();
            this.Param3_4 = new System.Windows.Forms.Label();
            this.Param_Scale = new System.Windows.Forms.PictureBox();
            this.Preview = new System.Windows.Forms.Button();
            this.Loop = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.SourceSelect = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Frame_Rate = new System.Windows.Forms.NumericUpDown();
            this.Label1 = new System.Windows.Forms.Label();
            this.Seconds = new System.Windows.Forms.NumericUpDown();
            this.COTime = new System.Windows.Forms.Label();
            this.CO_TIME = new System.Windows.Forms.NumericUpDown();
            this.AirTemp = new System.Windows.Forms.Label();
            this.Air_Temp = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.RT_Count = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ParticleChoice = new System.Windows.Forms.ComboBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.Folder_Status = new System.Windows.Forms.TextBox();
            this.OpenFolder = new System.Windows.Forms.Button();
            this.Calculate = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.ForwButton = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).BeginInit();
            this.GroupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Frame_Rate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Seconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FromPointInputToolStripMenuItem
            // 
            this.FromPointInputToolStripMenuItem.Name = "FromPointInputToolStripMenuItem";
            this.FromPointInputToolStripMenuItem.Size = new System.Drawing.Size(343, 44);
            this.FromPointInputToolStripMenuItem.Text = "From Point Input";
            // 
            // DirectionalSourceToolStripMenuItem
            // 
            this.DirectionalSourceToolStripMenuItem.Name = "DirectionalSourceToolStripMenuItem";
            this.DirectionalSourceToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.DirectionalSourceToolStripMenuItem.Text = "Directional Source...";
            // 
            // FromMeshSphereToolStripMenuItem
            // 
            this.FromMeshSphereToolStripMenuItem.Name = "FromMeshSphereToolStripMenuItem";
            this.FromMeshSphereToolStripMenuItem.Size = new System.Drawing.Size(343, 44);
            this.FromMeshSphereToolStripMenuItem.Text = "From MeshSphere";
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FromMeshSphereToolStripMenuItem,
            this.FromPointInputToolStripMenuItem});
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.ToolStripMenuItem1.Text = "Omni-Directional Source...";
            // 
            // FromPointInputToolStripMenuItem2
            // 
            this.FromPointInputToolStripMenuItem2.Name = "FromPointInputToolStripMenuItem2";
            this.FromPointInputToolStripMenuItem2.Size = new System.Drawing.Size(361, 44);
            this.FromPointInputToolStripMenuItem2.Text = "From Point Input";
            // 
            // SelectASphereToolStripMenuItem
            // 
            this.SelectASphereToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FromSphereObjectToolStripMenuItem1,
            this.FromPointInputToolStripMenuItem2});
            this.SelectASphereToolStripMenuItem.Name = "SelectASphereToolStripMenuItem";
            this.SelectASphereToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.SelectASphereToolStripMenuItem.Text = "Select...";
            // 
            // FromSphereObjectToolStripMenuItem1
            // 
            this.FromSphereObjectToolStripMenuItem1.Name = "FromSphereObjectToolStripMenuItem1";
            this.FromSphereObjectToolStripMenuItem1.Size = new System.Drawing.Size(361, 44);
            this.FromSphereObjectToolStripMenuItem1.Text = "From Sphere Object";
            // 
            // Forw
            // 
            this.Forw.Location = new System.Drawing.Point(209, 19);
            this.Forw.Name = "Forw";
            this.Forw.Size = new System.Drawing.Size(65, 42);
            this.Forw.TabIndex = 34;
            this.Forw.Text = ">>";
            this.Forw.UseVisualStyleBackColor = true;
            // 
            // Rev
            // 
            this.Rev.Location = new System.Drawing.Point(7, 19);
            this.Rev.Name = "Rev";
            this.Rev.Size = new System.Drawing.Size(65, 42);
            this.Rev.TabIndex = 33;
            this.Rev.Text = "<<";
            this.Rev.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Particle:";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Tetrahedron",
            "Pyramid",
            "Cube",
            "Sphere"});
            this.comboBox1.Location = new System.Drawing.Point(61, 70);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(213, 39);
            this.comboBox1.TabIndex = 32;
            this.comboBox1.Text = "Tetrahedron";
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(2, 46);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(44, 13);
            this.Label16.TabIndex = 30;
            this.Label16.Text = "Source:";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(2, 19);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(38, 13);
            this.Label15.TabIndex = 29;
            this.Label15.Text = "Room:";
            // 
            // SourceSelection
            // 
            this.SourceSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceSelection.FormattingEnabled = true;
            this.SourceSelection.Items.AddRange(new object[] {
            "From Point Input",
            "From Source Directivity Data"});
            this.SourceSelection.Location = new System.Drawing.Point(61, 43);
            this.SourceSelection.Name = "SourceSelection";
            this.SourceSelection.Size = new System.Drawing.Size(213, 39);
            this.SourceSelection.TabIndex = 27;
            this.SourceSelection.Text = "Select Source...";
            // 
            // RoomSelection
            // 
            this.RoomSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RoomSelection.FormattingEnabled = true;
            this.RoomSelection.Items.AddRange(new object[] {
            "Use Entire Model",
            "Select Surfaces"});
            this.RoomSelection.Location = new System.Drawing.Point(61, 16);
            this.RoomSelection.Name = "RoomSelection";
            this.RoomSelection.Size = new System.Drawing.Size(213, 39);
            this.RoomSelection.TabIndex = 26;
            this.RoomSelection.Text = "Use Entire Model";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox4, 2);
            this.groupBox4.Controls.Add(this.Time_Preview);
            this.groupBox4.Location = new System.Drawing.Point(8, 151);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox4, 2);
            this.groupBox4.Size = new System.Drawing.Size(388, 81);
            this.groupBox4.TabIndex = 104;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current Time (ms)";
            // 
            // Time_Preview
            // 
            this.Time_Preview.AutoSize = true;
            this.Time_Preview.Location = new System.Drawing.Point(67, 38);
            this.Time_Preview.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Time_Preview.Name = "Time_Preview";
            this.Time_Preview.Size = new System.Drawing.Size(186, 31);
            this.Time_Preview.TabIndex = 106;
            this.Time_Preview.Text = "Time_Preview";
            // 
            // Octave
            // 
            this.Octave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Octave, 2);
            this.Octave.FormattingEnabled = true;
            this.Octave.Items.AddRange(new object[] {
            "Summation: All Octaves",
            "62.5 Hz.",
            "125 Hz.",
            "250 Hz.",
            "500 Hz.",
            "1 kHz.",
            "2 kHz.",
            "4 kHz.",
            "8 kHz.",
            "Unified Filter"});
            this.Octave.Location = new System.Drawing.Point(8, 463);
            this.Octave.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Octave.Name = "Octave";
            this.Octave.Size = new System.Drawing.Size(388, 39);
            this.Octave.TabIndex = 103;
            this.Octave.Text = "Summation: All Octaves";
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label22, 2);
            this.label22.Location = new System.Drawing.Point(8, 384);
            this.label22.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(290, 72);
            this.label22.TabIndex = 102;
            this.label22.Text = "Octave Band Selection";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label23, 2);
            this.label23.Location = new System.Drawing.Point(8, 240);
            this.label23.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(198, 72);
            this.label23.TabIndex = 101;
            this.label23.Text = "Color Selection";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Color_Selection
            // 
            this.Color_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Color_Selection, 2);
            this.Color_Selection.FormattingEnabled = true;
            this.Color_Selection.Items.AddRange(new object[] {
            "R-O-Y-G-B-I-V",
            "R-O-Y",
            "Y-G-B",
            "W-B",
            "R-B"});
            this.Color_Selection.Location = new System.Drawing.Point(8, 319);
            this.Color_Selection.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Color_Selection.Name = "Color_Selection";
            this.Color_Selection.Size = new System.Drawing.Size(388, 39);
            this.Color_Selection.TabIndex = 100;
            this.Color_Selection.Text = "R-O-Y-G-B-I-V";
            this.Color_Selection.SelectedIndexChanged += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
            // 
            // Param1_4
            // 
            this.Param1_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param1_4.AutoSize = true;
            this.Param1_4.Location = new System.Drawing.Point(554, 384);
            this.Param1_4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Param1_4.Name = "Param1_4";
            this.Param1_4.Size = new System.Drawing.Size(44, 72);
            this.Param1_4.TabIndex = 99;
            this.Param1_4.Text = "00";
            this.Param1_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param1_2
            // 
            this.Param1_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param1_2.AutoSize = true;
            this.Param1_2.Location = new System.Drawing.Point(554, 312);
            this.Param1_2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Param1_2.Name = "Param1_2";
            this.Param1_2.Size = new System.Drawing.Size(44, 72);
            this.Param1_2.TabIndex = 98;
            this.Param1_2.Text = "00";
            this.Param1_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param_Min
            // 
            this.Param_Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Min.DecimalPlaces = 1;
            this.Param_Min.Location = new System.Drawing.Point(462, 483);
            this.Param_Min.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Param_Min.Name = "Param_Min";
            this.Param_Min.Size = new System.Drawing.Size(136, 38);
            this.Param_Min.TabIndex = 97;
            this.Param_Min.ValueChanged += new System.EventHandler(this.Param_Max_ValueChanged);
            // 
            // Param_Max
            // 
            this.Param_Max.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Max.DecimalPlaces = 1;
            this.Param_Max.Location = new System.Drawing.Point(462, 175);
            this.Param_Max.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Param_Max.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.Param_Max.Name = "Param_Max";
            this.Param_Max.Size = new System.Drawing.Size(136, 38);
            this.Param_Max.TabIndex = 96;
            this.Param_Max.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.Param_Max.ValueChanged += new System.EventHandler(this.Param_Max_ValueChanged);
            // 
            // Param3_4
            // 
            this.Param3_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param3_4.AutoSize = true;
            this.Param3_4.Location = new System.Drawing.Point(554, 240);
            this.Param3_4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Param3_4.Name = "Param3_4";
            this.Param3_4.Size = new System.Drawing.Size(44, 72);
            this.Param3_4.TabIndex = 95;
            this.Param3_4.Text = "00";
            this.Param3_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param_Scale
            // 
            this.Param_Scale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Scale.Location = new System.Drawing.Point(614, 175);
            this.Param_Scale.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Param_Scale.Name = "Param_Scale";
            this.tableLayoutPanel1.SetRowSpan(this.Param_Scale, 5);
            this.Param_Scale.Size = new System.Drawing.Size(189, 346);
            this.Param_Scale.TabIndex = 94;
            this.Param_Scale.TabStop = false;
            // 
            // Preview
            // 
            this.Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Preview, 4);
            this.Preview.Location = new System.Drawing.Point(5, 77);
            this.Preview.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(801, 62);
            this.Preview.TabIndex = 87;
            this.Preview.Text = "Preview";
            this.Preview.UseVisualStyleBackColor = true;
            this.Preview.Click += new System.EventHandler(this.Preview_Click);
            // 
            // Loop
            // 
            this.Loop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Loop, 2);
            this.Loop.Enabled = false;
            this.Loop.Location = new System.Drawing.Point(210, 7);
            this.Loop.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Loop.Name = "Loop";
            this.Loop.Size = new System.Drawing.Size(388, 58);
            this.Loop.TabIndex = 35;
            this.Loop.Text = "Loop";
            this.Loop.UseVisualStyleBackColor = true;
            this.Loop.Click += new System.EventHandler(this.Loop_Click);
            // 
            // BackButton
            // 
            this.BackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackButton.Enabled = false;
            this.BackButton.Location = new System.Drawing.Point(8, 7);
            this.BackButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(186, 58);
            this.BackButton.TabIndex = 33;
            this.BackButton.Text = "<<";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.Rev_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.SourceSelect);
            this.GroupBox2.Location = new System.Drawing.Point(21, 7);
            this.GroupBox2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.GroupBox2.Size = new System.Drawing.Size(811, 103);
            this.GroupBox2.TabIndex = 71;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Visualisation Type";
            // 
            // SourceSelect
            // 
            this.SourceSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceSelect.FormattingEnabled = true;
            this.SourceSelect.Items.AddRange(new object[] {
            "Smart Particle Wave",
            "Particle Wave",
            "Mesh Wave"});
            this.SourceSelect.Location = new System.Drawing.Point(24, 31);
            this.SourceSelect.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.SourceSelect.Name = "SourceSelect";
            this.SourceSelect.Size = new System.Drawing.Size(764, 39);
            this.SourceSelect.TabIndex = 27;
            this.SourceSelect.Text = "Smart Particle Wave";
            this.SourceSelect.SelectedIndexChanged += new System.EventHandler(this.SourceSelect_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Label3);
            this.groupBox1.Controls.Add(this.Frame_Rate);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Controls.Add(this.Seconds);
            this.groupBox1.Controls.Add(this.COTime);
            this.groupBox1.Controls.Add(this.CO_TIME);
            this.groupBox1.Controls.Add(this.AirTemp);
            this.groupBox1.Controls.Add(this.Air_Temp);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.RT_Count);
            this.groupBox1.Location = new System.Drawing.Point(21, 117);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Size = new System.Drawing.Size(811, 351);
            this.groupBox1.TabIndex = 94;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulation Settings";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(16, 291);
            this.Label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(157, 31);
            this.Label3.TabIndex = 103;
            this.Label3.Text = "Frame Rate";
            // 
            // Frame_Rate
            // 
            this.Frame_Rate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Frame_Rate.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Frame_Rate.Location = new System.Drawing.Point(656, 286);
            this.Frame_Rate.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Frame_Rate.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.Frame_Rate.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.Frame_Rate.Name = "Frame_Rate";
            this.Frame_Rate.Size = new System.Drawing.Size(139, 38);
            this.Frame_Rate.TabIndex = 102;
            this.Frame_Rate.UseWaitCursor = true;
            this.Frame_Rate.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(16, 229);
            this.Label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(244, 31);
            this.Label1.TabIndex = 101;
            this.Label1.Text = "Duration (seconds)";
            // 
            // Seconds
            // 
            this.Seconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Seconds.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Seconds.Location = new System.Drawing.Point(656, 224);
            this.Seconds.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Seconds.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.Seconds.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.Seconds.Name = "Seconds";
            this.Seconds.Size = new System.Drawing.Size(139, 38);
            this.Seconds.TabIndex = 100;
            this.Seconds.UseWaitCursor = true;
            this.Seconds.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // COTime
            // 
            this.COTime.AutoSize = true;
            this.COTime.Location = new System.Drawing.Point(16, 105);
            this.COTime.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.COTime.Name = "COTime";
            this.COTime.Size = new System.Drawing.Size(229, 31);
            this.COTime.TabIndex = 83;
            this.COTime.Text = "Cut Off Time (ms)";
            // 
            // CO_TIME
            // 
            this.CO_TIME.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CO_TIME.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.CO_TIME.Location = new System.Drawing.Point(616, 100);
            this.CO_TIME.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CO_TIME.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.CO_TIME.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CO_TIME.Name = "CO_TIME";
            this.CO_TIME.Size = new System.Drawing.Size(179, 38);
            this.CO_TIME.TabIndex = 82;
            this.CO_TIME.UseWaitCursor = true;
            this.CO_TIME.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // AirTemp
            // 
            this.AirTemp.AutoSize = true;
            this.AirTemp.Location = new System.Drawing.Point(16, 167);
            this.AirTemp.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.AirTemp.Name = "AirTemp";
            this.AirTemp.Size = new System.Drawing.Size(254, 31);
            this.AirTemp.TabIndex = 81;
            this.AirTemp.Text = "Air Temperature (C)";
            // 
            // Air_Temp
            // 
            this.Air_Temp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Temp.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Temp.Location = new System.Drawing.Point(656, 162);
            this.Air_Temp.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Air_Temp.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.Air_Temp.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.Air_Temp.Name = "Air_Temp";
            this.Air_Temp.Size = new System.Drawing.Size(139, 38);
            this.Air_Temp.TabIndex = 80;
            this.Air_Temp.UseWaitCursor = true;
            this.Air_Temp.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 43);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(210, 31);
            this.label10.TabIndex = 79;
            this.label10.Text = "Number of Rays";
            // 
            // RT_Count
            // 
            this.RT_Count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RT_Count.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.RT_Count.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RT_Count.Location = new System.Drawing.Point(616, 38);
            this.RT_Count.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.RT_Count.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.RT_Count.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.RT_Count.Name = "RT_Count";
            this.RT_Count.Size = new System.Drawing.Size(179, 38);
            this.RT_Count.TabIndex = 78;
            this.RT_Count.UseWaitCursor = true;
            this.RT_Count.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 599);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 60);
            this.label2.TabIndex = 87;
            this.label2.Text = "Particle:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ParticleChoice
            // 
            this.ParticleChoice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.ParticleChoice, 3);
            this.ParticleChoice.FormattingEnabled = true;
            this.ParticleChoice.Items.AddRange(new object[] {
            "Tetrahedron",
            "Icosahedron",
            "Cube",
            "Geodesic Sphere"});
            this.ParticleChoice.Location = new System.Drawing.Point(210, 606);
            this.ParticleChoice.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ParticleChoice.Name = "ParticleChoice";
            this.ParticleChoice.Size = new System.Drawing.Size(593, 39);
            this.ParticleChoice.TabIndex = 86;
            this.ParticleChoice.Text = "Tetrahedron";
            // 
            // Label14
            // 
            this.Label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label14.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.Label14, 2);
            this.Label14.Location = new System.Drawing.Point(8, 659);
            this.Label14.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(263, 60);
            this.Label14.TabIndex = 85;
            this.Label14.Text = "Select Output Folder";
            this.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Folder_Status
            // 
            this.Folder_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Folder_Status, 4);
            this.Folder_Status.Location = new System.Drawing.Point(8, 745);
            this.Folder_Status.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Folder_Status.Name = "Folder_Status";
            this.Folder_Status.ReadOnly = true;
            this.Folder_Status.Size = new System.Drawing.Size(795, 38);
            this.Folder_Status.TabIndex = 84;
            // 
            // OpenFolder
            // 
            this.OpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.OpenFolder, 2);
            this.OpenFolder.Location = new System.Drawing.Point(412, 666);
            this.OpenFolder.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.OpenFolder.Name = "OpenFolder";
            this.OpenFolder.Size = new System.Drawing.Size(391, 46);
            this.OpenFolder.TabIndex = 83;
            this.OpenFolder.Text = "Open...";
            this.OpenFolder.UseVisualStyleBackColor = true;
            this.OpenFolder.Click += new System.EventHandler(this.OpenFolder_Click);
            // 
            // Calculate
            // 
            this.Calculate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Calculate, 4);
            this.Calculate.Location = new System.Drawing.Point(5, 805);
            this.Calculate.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Calculate.Name = "Calculate";
            this.Calculate.Size = new System.Drawing.Size(801, 108);
            this.Calculate.TabIndex = 82;
            this.Calculate.Text = "Animate";
            this.Calculate.UseVisualStyleBackColor = true;
            this.Calculate.Click += new System.EventHandler(this.Calculate_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.Calculate, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.Folder_Status, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.Label14, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.ParticleChoice, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.OpenFolder, 2, 11);
            this.tableLayoutPanel1.Controls.Add(this.BackButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Preview, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ForwButton, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.Param3_4, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.Param1_2, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.Param1_4, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.Param_Min, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.Param_Max, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.Param_Scale, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label23, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.Color_Selection, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label22, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.Octave, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.Loop, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(21, 481);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(811, 918);
            this.tableLayoutPanel1.TabIndex = 96;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.Location = new System.Drawing.Point(8, 552);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 47);
            this.label5.TabIndex = 96;
            this.label5.Text = "Render Settings";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ForwButton
            // 
            this.ForwButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForwButton.Enabled = false;
            this.ForwButton.Location = new System.Drawing.Point(614, 7);
            this.ForwButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ForwButton.Name = "ForwButton";
            this.ForwButton.Size = new System.Drawing.Size(189, 58);
            this.ForwButton.TabIndex = 34;
            this.ForwButton.Text = ">>";
            this.ForwButton.UseVisualStyleBackColor = true;
            this.ForwButton.Click += new System.EventHandler(this.Forw_Click);
            // 
            // Pach_Visual_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GroupBox2);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Pach_Visual_Control";
            this.Size = new System.Drawing.Size(840, 1407);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).EndInit();
            this.GroupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Frame_Rate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Seconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

            }
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem DirectionalSourceToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromMeshSphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem2;
            internal System.Windows.Forms.ToolStripMenuItem SelectASphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromSphereObjectToolStripMenuItem1;
            internal System.Windows.Forms.Button Loop;
            internal System.Windows.Forms.Button Forw;
            internal System.Windows.Forms.Button Rev;
            internal System.Windows.Forms.Label label4;
            internal System.Windows.Forms.ComboBox comboBox1;
            internal System.Windows.Forms.Label Label16;
            internal System.Windows.Forms.Label Label15;
            internal System.Windows.Forms.ComboBox SourceSelection;
            internal System.Windows.Forms.ComboBox RoomSelection;
            internal System.Windows.Forms.Button BackButton;
            internal System.Windows.Forms.GroupBox GroupBox2;
            internal System.Windows.Forms.ComboBox SourceSelect;
            private System.Windows.Forms.GroupBox groupBox1;
            internal System.Windows.Forms.Label Label3;
            internal System.Windows.Forms.NumericUpDown Frame_Rate;
            internal System.Windows.Forms.Label Label1;
            internal System.Windows.Forms.NumericUpDown Seconds;
            internal System.Windows.Forms.Label COTime;
            internal System.Windows.Forms.NumericUpDown CO_TIME;
            internal System.Windows.Forms.Label AirTemp;
            internal System.Windows.Forms.NumericUpDown Air_Temp;
            internal System.Windows.Forms.Label label10;
            internal System.Windows.Forms.NumericUpDown RT_Count;
            internal System.Windows.Forms.Label Label14;
            internal System.Windows.Forms.TextBox Folder_Status;
            internal System.Windows.Forms.Button OpenFolder;
            internal System.Windows.Forms.Button Calculate;
            internal System.Windows.Forms.ComboBox Octave;
            private System.Windows.Forms.Label label22;
            private System.Windows.Forms.Label label23;
            internal System.Windows.Forms.ComboBox Color_Selection;
            private System.Windows.Forms.Label Param1_4;
            private System.Windows.Forms.Label Param1_2;
            private System.Windows.Forms.NumericUpDown Param_Min;
            private System.Windows.Forms.NumericUpDown Param_Max;
            private System.Windows.Forms.Label Param3_4;
            private System.Windows.Forms.PictureBox Param_Scale;
            internal System.Windows.Forms.Button Preview;
            internal System.Windows.Forms.Label label2;
            internal System.Windows.Forms.ComboBox ParticleChoice;
            private System.Windows.Forms.GroupBox groupBox4;
            private System.Windows.Forms.Label Time_Preview;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            internal System.Windows.Forms.Button ForwButton;
            internal System.Windows.Forms.Label label5;
        }
    }
}