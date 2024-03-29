﻿//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
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
        partial class Pach_TD_Numeric_Control
        {
            ////UserControl overrides dispose to clean up the component list. 
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
            this.components = new System.ComponentModel.Container();
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.Export_Signal = new System.Windows.Forms.Button();
            this.CO_TIME = new System.Windows.Forms.NumericUpDown();
            this.CalculateSim = new System.Windows.Forms.Button();
            this.Frequency_View = new ZedGraph.ZedGraphControl();
            this.label5 = new System.Windows.Forms.Label();
            this.TransientView = new ZedGraph.ZedGraphControl();
            this.Eigen_Extent = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.EigenFrequencies = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Receiver_Choice = new System.Windows.Forms.ComboBox();
            this.EigenPML = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Scat_Dir_75 = new System.Windows.Forms.CheckBox();
            this.Scat_Dir_60 = new System.Windows.Forms.CheckBox();
            this.Scat_Dir_45 = new System.Windows.Forms.CheckBox();
            this.Scat_Dir_30 = new System.Windows.Forms.CheckBox();
            this.Scat_Dir_15 = new System.Windows.Forms.CheckBox();
            this.Scat_Dir_00 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.ScatteringGraph = new ZedGraph.ZedGraphControl();
            this.label12 = new System.Windows.Forms.Label();
            this.Scatter_Scale = new System.Windows.Forms.PictureBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Scat_Color = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.Analysis_Technique = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Sample_Depth = new System.Windows.Forms.NumericUpDown();
            this.ScatteringRadius = new System.Windows.Forms.NumericUpDown();
            this.ScatMax = new System.Windows.Forms.NumericUpDown();
            this.ScatMin = new System.Windows.Forms.NumericUpDown();
            this.Scat_Mid = new System.Windows.Forms.Label();
            this.Freq_Trackbar1 = new System.Windows.Forms.TrackBar();
            this.Freq_Trackbar2 = new System.Windows.Forms.TrackBar();
            this.Freq_Feedback = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.Scat_Param_Select = new System.Windows.Forms.ComboBox();
            this.CalculateScattering = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EdgeFreq = new System.Windows.Forms.CheckBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.Atten_Method = new System.Windows.Forms.ComboBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.Air_Pressure = new System.Windows.Forms.NumericUpDown();
            this.Label3 = new System.Windows.Forms.Label();
            this.Rel_Humidity = new System.Windows.Forms.NumericUpDown();
            this.AirTemp = new System.Windows.Forms.Label();
            this.Air_Temp = new System.Windows.Forms.NumericUpDown();
            this.Freq_Max = new System.Windows.Forms.NumericUpDown();
            this.Frequency_Selection = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SourceSelect = new System.Windows.Forms.ComboBox();
            this.Preview = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Param3_4 = new System.Windows.Forms.Label();
            this.Param1_2 = new System.Windows.Forms.Label();
            this.Param_Max = new System.Windows.Forms.NumericUpDown();
            this.Param_Scale = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Time_Preview = new System.Windows.Forms.Label();
            this.Param1_4 = new System.Windows.Forms.Label();
            this.Param_Min = new System.Windows.Forms.NumericUpDown();
            this.Loop = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.AxisSelect = new System.Windows.Forms.ComboBox();
            this.Map_Planes = new System.Windows.Forms.ListBox();
            this.AddPlane = new System.Windows.Forms.Button();
            this.SetFolder = new System.Windows.Forms.Button();
            this.Folder_Status = new System.Windows.Forms.TextBox();
            this.GroundPlane = new System.Windows.Forms.CheckBox();
            this.Magnitude = new System.Windows.Forms.CheckBox();
            this.Color_Selection = new System.Windows.Forms.ComboBox();
            this.Pos_Select = new System.Windows.Forms.NumericUpDown();
            this.DeletePlane = new System.Windows.Forms.Button();
            this.VisualPML = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Scatter_Scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sample_Depth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatteringRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Trackbar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Trackbar2)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Frequency_Selection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pos_Select)).BeginInit();
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
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1106, 1505);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(8, 45);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPage1.Size = new System.Drawing.Size(1090, 1452);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Eigen Values";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.02724F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.97275F));
            this.tableLayoutPanel3.Controls.Add(this.label14, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.Export_Signal, 3, 5);
            this.tableLayoutPanel3.Controls.Add(this.CO_TIME, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.CalculateSim, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.Frequency_View, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.TransientView, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.Eigen_Extent, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label20, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.EigenFrequencies, 2, 8);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.Receiver_Choice, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.EigenPML, 3, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 9;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1074, 1436);
            this.tableLayoutPanel3.TabIndex = 44;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Red;
            this.label14.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label14.Location = new System.Drawing.Point(897, 0);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(171, 31);
            this.label14.TabIndex = 84;
            this.label14.Text = "Experimental";
            // 
            // Export_Signal
            // 
            this.Export_Signal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Export_Signal.Location = new System.Drawing.Point(854, 316);
            this.Export_Signal.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.Export_Signal.Name = "Export_Signal";
            this.Export_Signal.Size = new System.Drawing.Size(214, 52);
            this.Export_Signal.TabIndex = 45;
            this.Export_Signal.Text = "Export...";
            this.Export_Signal.UseVisualStyleBackColor = true;
            this.Export_Signal.Click += new System.EventHandler(this.Export_Signal_Click);
            // 
            // CO_TIME
            // 
            this.CO_TIME.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CO_TIME.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.CO_TIME.Location = new System.Drawing.Point(662, 86);
            this.CO_TIME.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.CO_TIME.Size = new System.Drawing.Size(178, 38);
            this.CO_TIME.TabIndex = 83;
            this.CO_TIME.UseWaitCursor = true;
            this.CO_TIME.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // CalculateSim
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.CalculateSim, 4);
            this.CalculateSim.Location = new System.Drawing.Point(8, 164);
            this.CalculateSim.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.CalculateSim.Name = "CalculateSim";
            this.CalculateSim.Size = new System.Drawing.Size(1054, 54);
            this.CalculateSim.TabIndex = 0;
            this.CalculateSim.Text = "Calculate";
            this.CalculateSim.UseVisualStyleBackColor = true;
            this.CalculateSim.Click += new System.EventHandler(this.CalculateSim_Click);
            // 
            // Frequency_View
            // 
            this.Frequency_View.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Frequency_View.AutoSize = true;
            this.Frequency_View.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.Frequency_View, 4);
            this.Frequency_View.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.Frequency_View.Location = new System.Drawing.Point(16, 825);
            this.Frequency_View.Margin = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.Frequency_View.Name = "Frequency_View";
            this.Frequency_View.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.Frequency_View.ScrollGrace = 0D;
            this.Frequency_View.ScrollMaxX = 0D;
            this.Frequency_View.ScrollMaxY = 0D;
            this.Frequency_View.ScrollMaxY2 = 0D;
            this.Frequency_View.ScrollMinX = 0D;
            this.Frequency_View.ScrollMinY = 0D;
            this.Frequency_View.ScrollMinY2 = 0D;
            this.Frequency_View.Size = new System.Drawing.Size(1042, 411);
            this.Frequency_View.TabIndex = 46;
            this.Frequency_View.UseExtendedPrintDialog = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label5, 2);
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(8, 78);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(304, 31);
            this.label5.TabIndex = 48;
            this.label5.Text = "Cutoff Time (ms):";
            // 
            // TransientView
            // 
            this.TransientView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TransientView.AutoSize = true;
            this.TransientView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.TransientView, 4);
            this.TransientView.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.TransientView.Location = new System.Drawing.Point(16, 386);
            this.TransientView.Margin = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.TransientView.Name = "TransientView";
            this.TransientView.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.TransientView.ScrollGrace = 0D;
            this.TransientView.ScrollMaxX = 0D;
            this.TransientView.ScrollMaxY = 0D;
            this.TransientView.ScrollMaxY2 = 0D;
            this.TransientView.ScrollMinX = 0D;
            this.TransientView.ScrollMinY = 0D;
            this.TransientView.ScrollMinY2 = 0D;
            this.TransientView.Size = new System.Drawing.Size(1042, 411);
            this.TransientView.TabIndex = 42;
            this.TransientView.UseExtendedPrintDialog = true;
            // 
            // Eigen_Extent
            // 
            this.Eigen_Extent.FormattingEnabled = true;
            this.Eigen_Extent.Items.AddRange(new object[] {
            "63 Hz. Octave Band",
            "125 Hz. Octave Band",
            "250 Hz. Octave Band",
            "500 Hz. Octave Band",
            "1000 Hz. Octave Band",
            "2000 Hz. Octave Band",
            "4000 Hz. Octave Band",
            "8000 Hz. Octave Band"});
            this.Eigen_Extent.Location = new System.Drawing.Point(328, 8);
            this.Eigen_Extent.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Eigen_Extent.Name = "Eigen_Extent";
            this.Eigen_Extent.Size = new System.Drawing.Size(508, 39);
            this.Eigen_Extent.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label2, 2);
            this.label2.Location = new System.Drawing.Point(8, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "Calculate up to:";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Enabled = false;
            this.label20.Location = new System.Drawing.Point(8, 234);
            this.label20.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(144, 31);
            this.label20.TabIndex = 47;
            this.label20.Text = "Receiver";
            // 
            // EigenFrequencies
            // 
            this.EigenFrequencies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.EigenFrequencies, 2);
            this.EigenFrequencies.FormattingEnabled = true;
            this.EigenFrequencies.ItemHeight = 31;
            this.EigenFrequencies.Location = new System.Drawing.Point(326, 1254);
            this.EigenFrequencies.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.EigenFrequencies.Name = "EigenFrequencies";
            this.EigenFrequencies.Size = new System.Drawing.Size(742, 159);
            this.EigenFrequencies.TabIndex = 47;
            this.EigenFrequencies.SelectedIndexChanged += new System.EventHandler(this.EigenFrequencies_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label6, 2);
            this.label6.Location = new System.Drawing.Point(6, 1250);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(308, 186);
            this.label6.TabIndex = 48;
            this.label6.Text = "Eigen-Frequencies";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Receiver_Choice
            // 
            this.Receiver_Choice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Receiver_Choice.FormattingEnabled = true;
            this.Receiver_Choice.Location = new System.Drawing.Point(328, 242);
            this.Receiver_Choice.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Receiver_Choice.Name = "Receiver_Choice";
            this.Receiver_Choice.Size = new System.Drawing.Size(512, 39);
            this.Receiver_Choice.TabIndex = 46;
            this.Receiver_Choice.Text = "No Results Calculated...";
            this.Receiver_Choice.SelectedIndexChanged += new System.EventHandler(this.Receiver_Choice_SelectedIndexChanged);
            // 
            // EigenPML
            // 
            this.EigenPML.AutoSize = true;
            this.EigenPML.Checked = true;
            this.EigenPML.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EigenPML.Location = new System.Drawing.Point(854, 84);
            this.EigenPML.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.EigenPML.Name = "EigenPML";
            this.EigenPML.Size = new System.Drawing.Size(101, 35);
            this.EigenPML.TabIndex = 85;
            this.EigenPML.Text = "PML";
            this.EigenPML.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel2);
            this.tabPage3.Location = new System.Drawing.Point(8, 45);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPage3.Size = new System.Drawing.Size(1090, 1452);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Scattering Analysis";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Enter += new System.EventHandler(this.ScatteringLab_Focus);
            this.tabPage3.Leave += new System.EventHandler(this.ScatteringLab_FocusLost);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoScroll = true;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 232F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.button1, 3, 7);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ScatteringGraph, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.Scatter_Scale, 3, 9);
            this.tableLayoutPanel2.Controls.Add(this.comboBox2, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Scat_Color, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label26, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.Analysis_Technique, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.Sample_Depth, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.ScatteringRadius, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.ScatMax, 2, 9);
            this.tableLayoutPanel2.Controls.Add(this.ScatMin, 2, 15);
            this.tableLayoutPanel2.Controls.Add(this.Scat_Mid, 2, 12);
            this.tableLayoutPanel2.Controls.Add(this.Freq_Trackbar1, 0, 14);
            this.tableLayoutPanel2.Controls.Add(this.Freq_Trackbar2, 0, 15);
            this.tableLayoutPanel2.Controls.Add(this.Freq_Feedback, 0, 13);
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.Scat_Param_Select, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.CalculateScattering, 0, 7);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 16;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1074, 1436);
            this.tableLayoutPanel2.TabIndex = 48;
            // 
            // groupBox5
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.groupBox5, 2);
            this.groupBox5.Controls.Add(this.Scat_Dir_75);
            this.groupBox5.Controls.Add(this.Scat_Dir_60);
            this.groupBox5.Controls.Add(this.Scat_Dir_45);
            this.groupBox5.Controls.Add(this.Scat_Dir_30);
            this.groupBox5.Controls.Add(this.Scat_Dir_15);
            this.groupBox5.Controls.Add(this.Scat_Dir_00);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(618, 6);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tableLayoutPanel2.SetRowSpan(this.groupBox5, 5);
            this.groupBox5.Size = new System.Drawing.Size(450, 328);
            this.groupBox5.TabIndex = 58;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Direction";
            // 
            // Scat_Dir_75
            // 
            this.Scat_Dir_75.AutoSize = true;
            this.Scat_Dir_75.Location = new System.Drawing.Point(232, 147);
            this.Scat_Dir_75.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Scat_Dir_75.Name = "Scat_Dir_75";
            this.Scat_Dir_75.Size = new System.Drawing.Size(186, 35);
            this.Scat_Dir_75.TabIndex = 5;
            this.Scat_Dir_75.Text = "75 Degrees";
            this.Scat_Dir_75.UseVisualStyleBackColor = true;
            // 
            // Scat_Dir_60
            // 
            this.Scat_Dir_60.AutoSize = true;
            this.Scat_Dir_60.Location = new System.Drawing.Point(232, 95);
            this.Scat_Dir_60.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Scat_Dir_60.Name = "Scat_Dir_60";
            this.Scat_Dir_60.Size = new System.Drawing.Size(186, 35);
            this.Scat_Dir_60.TabIndex = 4;
            this.Scat_Dir_60.Text = "60 Degrees";
            this.Scat_Dir_60.UseVisualStyleBackColor = true;
            // 
            // Scat_Dir_45
            // 
            this.Scat_Dir_45.AutoSize = true;
            this.Scat_Dir_45.Location = new System.Drawing.Point(12, 200);
            this.Scat_Dir_45.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Scat_Dir_45.Name = "Scat_Dir_45";
            this.Scat_Dir_45.Size = new System.Drawing.Size(186, 35);
            this.Scat_Dir_45.TabIndex = 3;
            this.Scat_Dir_45.Text = "45 Degrees";
            this.Scat_Dir_45.UseVisualStyleBackColor = true;
            // 
            // Scat_Dir_30
            // 
            this.Scat_Dir_30.AutoSize = true;
            this.Scat_Dir_30.Location = new System.Drawing.Point(12, 147);
            this.Scat_Dir_30.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Scat_Dir_30.Name = "Scat_Dir_30";
            this.Scat_Dir_30.Size = new System.Drawing.Size(186, 35);
            this.Scat_Dir_30.TabIndex = 2;
            this.Scat_Dir_30.Text = "30 Degrees";
            this.Scat_Dir_30.UseVisualStyleBackColor = true;
            // 
            // Scat_Dir_15
            // 
            this.Scat_Dir_15.AutoSize = true;
            this.Scat_Dir_15.Location = new System.Drawing.Point(12, 95);
            this.Scat_Dir_15.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Scat_Dir_15.Name = "Scat_Dir_15";
            this.Scat_Dir_15.Size = new System.Drawing.Size(186, 35);
            this.Scat_Dir_15.TabIndex = 1;
            this.Scat_Dir_15.Text = "15 Degrees";
            this.Scat_Dir_15.UseVisualStyleBackColor = true;
            // 
            // Scat_Dir_00
            // 
            this.Scat_Dir_00.AutoSize = true;
            this.Scat_Dir_00.Checked = true;
            this.Scat_Dir_00.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Scat_Dir_00.Location = new System.Drawing.Point(14, 43);
            this.Scat_Dir_00.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Scat_Dir_00.Name = "Scat_Dir_00";
            this.Scat_Dir_00.Size = new System.Drawing.Size(257, 35);
            this.Scat_Dir_00.TabIndex = 0;
            this.Scat_Dir_00.Text = "Normal Incidence";
            this.Scat_Dir_00.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(848, 360);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(220, 75);
            this.button1.TabIndex = 45;
            this.button1.Text = "Export...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Red;
            this.label13.Dock = System.Windows.Forms.DockStyle.Top;
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label13.Location = new System.Drawing.Point(6, 0);
            this.label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(294, 31);
            this.label13.TabIndex = 57;
            this.label13.Text = "Experimental";
            // 
            // ScatteringGraph
            // 
            this.ScatteringGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScatteringGraph.AutoSize = true;
            this.ScatteringGraph.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.SetColumnSpan(this.ScatteringGraph, 4);
            this.ScatteringGraph.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.ScatteringGraph.Location = new System.Drawing.Point(16, 453);
            this.ScatteringGraph.Margin = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.ScatteringGraph.Name = "ScatteringGraph";
            this.ScatteringGraph.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.ScatteringGraph.ScrollGrace = 0D;
            this.ScatteringGraph.ScrollMaxX = 0D;
            this.ScatteringGraph.ScrollMaxY = 0D;
            this.ScatteringGraph.ScrollMaxY2 = 0D;
            this.ScatteringGraph.ScrollMinX = 0D;
            this.ScatteringGraph.ScrollMinY = 0D;
            this.ScatteringGraph.ScrollMinY2 = 0D;
            this.ScatteringGraph.Size = new System.Drawing.Size(1042, 633);
            this.ScatteringGraph.TabIndex = 42;
            this.ScatteringGraph.UseExtendedPrintDialog = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 136);
            this.label12.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(203, 31);
            this.label12.TabIndex = 56;
            this.label12.Text = "Calculate up to:";
            // 
            // Scatter_Scale
            // 
            this.Scatter_Scale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scatter_Scale.Location = new System.Drawing.Point(850, 1108);
            this.Scatter_Scale.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Scatter_Scale.Name = "Scatter_Scale";
            this.tableLayoutPanel2.SetRowSpan(this.Scatter_Scale, 7);
            this.Scatter_Scale.Size = new System.Drawing.Size(216, 320);
            this.Scatter_Scale.TabIndex = 84;
            this.Scatter_Scale.TabStop = false;
            // 
            // comboBox2
            // 
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "63 Hz. Octave Band",
            "125 Hz. Octave Band",
            "250 Hz. Octave Band",
            "500 Hz. Octave Band",
            "1000 Hz. Octave Band",
            "2000 Hz. Octave Band",
            "4000 Hz. Octave Band",
            "8000 Hz. Octave Band"});
            this.comboBox2.Location = new System.Drawing.Point(314, 144);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(290, 39);
            this.comboBox2.TabIndex = 55;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 68);
            this.label9.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(192, 31);
            this.label9.TabIndex = 47;
            this.label9.Text = "Analysis Type:";
            // 
            // Scat_Color
            // 
            this.Scat_Color.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.Scat_Color, 2);
            this.Scat_Color.FormattingEnabled = true;
            this.Scat_Color.Items.AddRange(new object[] {
            "R-O-Y-G-B-I-V",
            "R-O-Y",
            "Y-G-B",
            "R-M-B",
            "W-B"});
            this.Scat_Color.Location = new System.Drawing.Point(8, 1156);
            this.Scat_Color.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Scat_Color.Name = "Scat_Color";
            this.Scat_Color.Size = new System.Drawing.Size(596, 39);
            this.Scat_Color.TabIndex = 90;
            this.Scat_Color.Text = "R-O-Y-G-B-I-V";
            this.Scat_Color.SelectedIndexChanged += new System.EventHandler(this.Scat_Color_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 272);
            this.label11.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(222, 62);
            this.label11.TabIndex = 54;
            this.label11.Text = "Depth of Sample (meters)";
            // 
            // label26
            // 
            this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(8, 1108);
            this.label26.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(198, 31);
            this.label26.TabIndex = 91;
            this.label26.Text = "Color Selection";
            // 
            // Analysis_Technique
            // 
            this.Analysis_Technique.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Analysis_Technique.FormattingEnabled = true;
            this.Analysis_Technique.Items.AddRange(new object[] {
            "Correlation Scattering Coefficient"});
            this.Analysis_Technique.Location = new System.Drawing.Point(314, 76);
            this.Analysis_Technique.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Analysis_Technique.Name = "Analysis_Technique";
            this.Analysis_Technique.Size = new System.Drawing.Size(290, 39);
            this.Analysis_Technique.TabIndex = 46;
            this.Analysis_Technique.SelectedIndexChanged += new System.EventHandler(this.LabGuideParametersChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 204);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(278, 62);
            this.label7.TabIndex = 52;
            this.label7.Text = "Measurement Radius (meters)";
            // 
            // Sample_Depth
            // 
            this.Sample_Depth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sample_Depth.DecimalPlaces = 1;
            this.Sample_Depth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Sample_Depth.Location = new System.Drawing.Point(450, 280);
            this.Sample_Depth.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Sample_Depth.Name = "Sample_Depth";
            this.Sample_Depth.Size = new System.Drawing.Size(154, 38);
            this.Sample_Depth.TabIndex = 53;
            this.Sample_Depth.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.Sample_Depth.ValueChanged += new System.EventHandler(this.LabGuideParametersChanged);
            // 
            // ScatteringRadius
            // 
            this.ScatteringRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScatteringRadius.DecimalPlaces = 1;
            this.ScatteringRadius.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScatteringRadius.Location = new System.Drawing.Point(450, 212);
            this.ScatteringRadius.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ScatteringRadius.Name = "ScatteringRadius";
            this.ScatteringRadius.Size = new System.Drawing.Size(154, 38);
            this.ScatteringRadius.TabIndex = 51;
            this.ScatteringRadius.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ScatteringRadius.ValueChanged += new System.EventHandler(this.LabGuideParametersChanged);
            // 
            // ScatMax
            // 
            this.ScatMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScatMax.DecimalPlaces = 1;
            this.ScatMax.Location = new System.Drawing.Point(698, 1108);
            this.ScatMax.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ScatMax.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.ScatMax.Name = "ScatMax";
            this.ScatMax.Size = new System.Drawing.Size(136, 38);
            this.ScatMax.TabIndex = 98;
            this.ScatMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ScatMax.ValueChanged += new System.EventHandler(this.Scat_ValueChanged);
            // 
            // ScatMin
            // 
            this.ScatMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScatMin.DecimalPlaces = 1;
            this.ScatMin.Location = new System.Drawing.Point(698, 1396);
            this.ScatMin.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ScatMin.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.ScatMin.Name = "ScatMin";
            this.ScatMin.Size = new System.Drawing.Size(136, 38);
            this.ScatMin.TabIndex = 99;
            this.ScatMin.ValueChanged += new System.EventHandler(this.Scat_ValueChanged);
            // 
            // Scat_Mid
            // 
            this.Scat_Mid.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Scat_Mid.AutoSize = true;
            this.Scat_Mid.Location = new System.Drawing.Point(769, 1252);
            this.Scat_Mid.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Scat_Mid.Name = "Scat_Mid";
            this.Scat_Mid.Size = new System.Drawing.Size(67, 31);
            this.Scat_Mid.TabIndex = 86;
            this.Scat_Mid.Text = "0.50";
            // 
            // Freq_Trackbar1
            // 
            this.Freq_Trackbar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.Freq_Trackbar1, 2);
            this.Freq_Trackbar1.Location = new System.Drawing.Point(6, 1346);
            this.Freq_Trackbar1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Freq_Trackbar1.Maximum = 4095;
            this.Freq_Trackbar1.Name = "Freq_Trackbar1";
            this.Freq_Trackbar1.Size = new System.Drawing.Size(600, 36);
            this.Freq_Trackbar1.TabIndex = 100;
            this.Freq_Trackbar1.Value = 1;
            this.Freq_Trackbar1.Scroll += new System.EventHandler(this.Freq_Trackbar_Scroll);
            // 
            // Freq_Trackbar2
            // 
            this.Freq_Trackbar2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.Freq_Trackbar2, 2);
            this.Freq_Trackbar2.Location = new System.Drawing.Point(6, 1394);
            this.Freq_Trackbar2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Freq_Trackbar2.Maximum = 4095;
            this.Freq_Trackbar2.Name = "Freq_Trackbar2";
            this.Freq_Trackbar2.Size = new System.Drawing.Size(600, 36);
            this.Freq_Trackbar2.TabIndex = 101;
            this.Freq_Trackbar2.Value = 4095;
            this.Freq_Trackbar2.Scroll += new System.EventHandler(this.Freq_Trackbar_Scroll);
            // 
            // Freq_Feedback
            // 
            this.Freq_Feedback.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Freq_Feedback.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.Freq_Feedback, 2);
            this.Freq_Feedback.Location = new System.Drawing.Point(8, 1300);
            this.Freq_Feedback.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Freq_Feedback.Name = "Freq_Feedback";
            this.Freq_Feedback.Size = new System.Drawing.Size(438, 31);
            this.Freq_Feedback.TabIndex = 92;
            this.Freq_Feedback.Text = "Frequency Selection: 0 to 8000 Hz.";
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 1204);
            this.label18.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(259, 31);
            this.label18.TabIndex = 91;
            this.label18.Text = "Parameter Selection";
            // 
            // Scat_Param_Select
            // 
            this.Scat_Param_Select.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.Scat_Param_Select, 2);
            this.Scat_Param_Select.FormattingEnabled = true;
            this.Scat_Param_Select.Items.AddRange(new object[] {
            "Total Reflected Power",
            "Correlation Coefficient"});
            this.Scat_Param_Select.Location = new System.Drawing.Point(8, 1252);
            this.Scat_Param_Select.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Scat_Param_Select.Name = "Scat_Param_Select";
            this.Scat_Param_Select.Size = new System.Drawing.Size(596, 39);
            this.Scat_Param_Select.TabIndex = 90;
            this.Scat_Param_Select.Text = "Total Reflected Power";
            this.Scat_Param_Select.SelectedIndexChanged += new System.EventHandler(this.Scat_Color_SelectedIndexChanged);
            // 
            // CalculateScattering
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.CalculateScattering, 3);
            this.CalculateScattering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CalculateScattering.Location = new System.Drawing.Point(8, 364);
            this.CalculateScattering.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.CalculateScattering.Name = "CalculateScattering";
            this.CalculateScattering.Size = new System.Drawing.Size(826, 67);
            this.CalculateScattering.TabIndex = 49;
            this.CalculateScattering.Text = "Calculate";
            this.CalculateScattering.UseVisualStyleBackColor = true;
            this.CalculateScattering.Click += new System.EventHandler(this.CalculateScattering_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(8, 45);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPage2.Size = new System.Drawing.Size(1090, 1452);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Visualization";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.Freq_Max, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.Frequency_Selection, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label17, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.SourceSelect, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Preview, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Param3_4, 3, 8);
            this.tableLayoutPanel1.Controls.Add(this.Param1_2, 3, 9);
            this.tableLayoutPanel1.Controls.Add(this.Param_Max, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.Param_Scale, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.Param1_4, 3, 10);
            this.tableLayoutPanel1.Controls.Add(this.Param_Min, 3, 11);
            this.tableLayoutPanel1.Controls.Add(this.Loop, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.button2, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.label23, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.AxisSelect, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.Map_Planes, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.AddPlane, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.SetFolder, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.Folder_Status, 3, 13);
            this.tableLayoutPanel1.Controls.Add(this.GroundPlane, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.Magnitude, 4, 12);
            this.tableLayoutPanel1.Controls.Add(this.Color_Selection, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.Pos_Select, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.DeletePlane, 2, 12);
            this.tableLayoutPanel1.Controls.Add(this.VisualPML, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 16;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 388F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1074, 1436);
            this.tableLayoutPanel1.TabIndex = 100;
            // 
            // groupBox3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox3, 5);
            this.groupBox3.Controls.Add(this.EdgeFreq);
            this.groupBox3.Controls.Add(this.Label21);
            this.groupBox3.Controls.Add(this.Atten_Method);
            this.groupBox3.Controls.Add(this.Label19);
            this.groupBox3.Controls.Add(this.Air_Pressure);
            this.groupBox3.Controls.Add(this.Label3);
            this.groupBox3.Controls.Add(this.Rel_Humidity);
            this.groupBox3.Controls.Add(this.AirTemp);
            this.groupBox3.Controls.Add(this.Air_Temp);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(8, 1017);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.groupBox3.Size = new System.Drawing.Size(1058, 372);
            this.groupBox3.TabIndex = 101;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Environmental Factors";
            // 
            // EdgeFreq
            // 
            this.EdgeFreq.AutoSize = true;
            this.EdgeFreq.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EdgeFreq.Location = new System.Drawing.Point(186, 293);
            this.EdgeFreq.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.EdgeFreq.Name = "EdgeFreq";
            this.EdgeFreq.Size = new System.Drawing.Size(378, 35);
            this.EdgeFreq.TabIndex = 35;
            this.EdgeFreq.Text = "Edge Frequency Correction";
            this.EdgeFreq.UseVisualStyleBackColor = true;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(22, 236);
            this.Label21.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(112, 31);
            this.Label21.TabIndex = 34;
            this.Label21.Text = "Method:";
            // 
            // Atten_Method
            // 
            this.Atten_Method.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Atten_Method.FormattingEnabled = true;
            this.Atten_Method.Items.AddRange(new object[] {
            "ISO 9613-1 (Outdoor Attenuation)",
            "Evans & Bazley (Indoor Attenuation)",
            "Known values (Vorlander)"});
            this.Atten_Method.Location = new System.Drawing.Point(186, 229);
            this.Atten_Method.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Atten_Method.Name = "Atten_Method";
            this.Atten_Method.Size = new System.Drawing.Size(840, 39);
            this.Atten_Method.TabIndex = 14;
            this.Atten_Method.Text = "ISO 9613-1 (Outdoor Attenuation)";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(22, 172);
            this.Label19.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(312, 31);
            this.Label19.TabIndex = 32;
            this.Label19.Text = "Static Air Pressure (hPa)";
            // 
            // Air_Pressure
            // 
            this.Air_Pressure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Pressure.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Pressure.Location = new System.Drawing.Point(862, 167);
            this.Air_Pressure.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Air_Pressure.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.Air_Pressure.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.Air_Pressure.Name = "Air_Pressure";
            this.Air_Pressure.Size = new System.Drawing.Size(170, 38);
            this.Air_Pressure.TabIndex = 13;
            this.Air_Pressure.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(22, 110);
            this.Label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(268, 31);
            this.Label3.TabIndex = 30;
            this.Label3.Text = "Relative Humidity(%)";
            // 
            // Rel_Humidity
            // 
            this.Rel_Humidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Rel_Humidity.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Rel_Humidity.Location = new System.Drawing.Point(948, 105);
            this.Rel_Humidity.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Rel_Humidity.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.Rel_Humidity.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.Rel_Humidity.Name = "Rel_Humidity";
            this.Rel_Humidity.Size = new System.Drawing.Size(86, 38);
            this.Rel_Humidity.TabIndex = 12;
            this.Rel_Humidity.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // AirTemp
            // 
            this.AirTemp.AutoSize = true;
            this.AirTemp.Location = new System.Drawing.Point(22, 48);
            this.AirTemp.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.AirTemp.Name = "AirTemp";
            this.AirTemp.Size = new System.Drawing.Size(254, 31);
            this.AirTemp.TabIndex = 28;
            this.AirTemp.Text = "Air Temperature (C)";
            // 
            // Air_Temp
            // 
            this.Air_Temp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Temp.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Temp.Location = new System.Drawing.Point(948, 43);
            this.Air_Temp.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.Air_Temp.Size = new System.Drawing.Size(86, 38);
            this.Air_Temp.TabIndex = 11;
            this.Air_Temp.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // Freq_Max
            // 
            this.Freq_Max.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Freq_Max.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Freq_Max.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Freq_Max.Location = new System.Drawing.Point(888, 144);
            this.Freq_Max.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Freq_Max.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.Freq_Max.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.Freq_Max.Name = "Freq_Max";
            this.Freq_Max.Size = new System.Drawing.Size(178, 38);
            this.Freq_Max.TabIndex = 78;
            this.Freq_Max.UseWaitCursor = true;
            this.Freq_Max.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // Frequency_Selection
            // 
            this.Frequency_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Frequency_Selection.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Frequency_Selection.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.Frequency_Selection.Location = new System.Drawing.Point(888, 76);
            this.Frequency_Selection.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Frequency_Selection.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Frequency_Selection.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Frequency_Selection.Name = "Frequency_Selection";
            this.Frequency_Selection.Size = new System.Drawing.Size(178, 38);
            this.Frequency_Selection.TabIndex = 84;
            this.Frequency_Selection.UseWaitCursor = true;
            this.Frequency_Selection.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Red;
            this.label17.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label17.Location = new System.Drawing.Point(897, 0);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(171, 31);
            this.label17.TabIndex = 58;
            this.label17.Text = "Experimental";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Location = new System.Drawing.Point(8, 136);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 31);
            this.label1.TabIndex = 85;
            this.label1.Text = "Frequency Max";
            // 
            // SourceSelect
            // 
            this.SourceSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceSelect.AutoCompleteCustomSource.AddRange(new string[] {
            "Sine Wave",
            "Dirac Pulse"});
            this.tableLayoutPanel1.SetColumnSpan(this.SourceSelect, 2);
            this.SourceSelect.FormattingEnabled = true;
            this.SourceSelect.Items.AddRange(new object[] {
            "Sine Wave",
            "Dirac Pulse",
            "Sine Pulse",
            "Pseudo Random Noise",
            "Noise Spectrum"});
            this.SourceSelect.Location = new System.Drawing.Point(436, 8);
            this.SourceSelect.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.SourceSelect.Name = "SourceSelect";
            this.SourceSelect.Size = new System.Drawing.Size(412, 39);
            this.SourceSelect.TabIndex = 27;
            this.SourceSelect.Text = "Sine Wave";
            // 
            // Preview
            // 
            this.Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Preview, 5);
            this.Preview.Location = new System.Drawing.Point(6, 316);
            this.Preview.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(1062, 66);
            this.Preview.TabIndex = 87;
            this.Preview.Text = "Calculate & Run";
            this.Preview.UseVisualStyleBackColor = true;
            this.Preview.Click += new System.EventHandler(this.Calculate_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(190, 31);
            this.label8.TabIndex = 30;
            this.label8.Text = "Source Signal:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label10, 3);
            this.label10.Location = new System.Drawing.Point(8, 68);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(262, 31);
            this.label10.TabIndex = 79;
            this.label10.Text = "Frequency Selection";
            // 
            // Param3_4
            // 
            this.Param3_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param3_4.AutoSize = true;
            this.Param3_4.Location = new System.Drawing.Point(804, 481);
            this.Param3_4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Param3_4.Name = "Param3_4";
            this.Param3_4.Size = new System.Drawing.Size(44, 112);
            this.Param3_4.TabIndex = 95;
            this.Param3_4.Text = "75";
            this.Param3_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param1_2
            // 
            this.Param1_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param1_2.AutoSize = true;
            this.Param1_2.Location = new System.Drawing.Point(804, 593);
            this.Param1_2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Param1_2.Name = "Param1_2";
            this.Param1_2.Size = new System.Drawing.Size(44, 112);
            this.Param1_2.TabIndex = 98;
            this.Param1_2.Text = "50";
            this.Param1_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param_Max
            // 
            this.Param_Max.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Max.DecimalPlaces = 1;
            this.Param_Max.Location = new System.Drawing.Point(712, 417);
            this.Param_Max.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Param_Max.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.Param_Max.Name = "Param_Max";
            this.Param_Max.Size = new System.Drawing.Size(136, 38);
            this.Param_Max.TabIndex = 96;
            this.Param_Max.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.Param_Max.ValueChanged += new System.EventHandler(this.Param_Max_ValueChanged);
            this.Param_Max.Click += new System.EventHandler(this.Param_Max_ValueChanged);
            // 
            // Param_Scale
            // 
            this.Param_Scale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Scale.Location = new System.Drawing.Point(864, 417);
            this.Param_Scale.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Param_Scale.Name = "Param_Scale";
            this.tableLayoutPanel1.SetRowSpan(this.Param_Scale, 5);
            this.Param_Scale.Size = new System.Drawing.Size(202, 464);
            this.Param_Scale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Param_Scale.TabIndex = 94;
            this.Param_Scale.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox4, 3);
            this.groupBox4.Controls.Add(this.Time_Preview);
            this.groupBox4.Location = new System.Drawing.Point(8, 394);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox4, 2);
            this.groupBox4.Size = new System.Drawing.Size(626, 79);
            this.groupBox4.TabIndex = 104;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current Time (ms)";
            // 
            // Time_Preview
            // 
            this.Time_Preview.AutoSize = true;
            this.Time_Preview.Location = new System.Drawing.Point(66, 39);
            this.Time_Preview.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Time_Preview.Name = "Time_Preview";
            this.Time_Preview.Size = new System.Drawing.Size(186, 31);
            this.Time_Preview.TabIndex = 106;
            this.Time_Preview.Text = "Time_Preview";
            // 
            // Param1_4
            // 
            this.Param1_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param1_4.AutoSize = true;
            this.Param1_4.Location = new System.Drawing.Point(804, 705);
            this.Param1_4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Param1_4.Name = "Param1_4";
            this.Param1_4.Size = new System.Drawing.Size(44, 112);
            this.Param1_4.TabIndex = 99;
            this.Param1_4.Text = "25";
            this.Param1_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param_Min
            // 
            this.Param_Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Min.DecimalPlaces = 1;
            this.Param_Min.Location = new System.Drawing.Point(712, 843);
            this.Param_Min.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Param_Min.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.Param_Min.Name = "Param_Min";
            this.Param_Min.Size = new System.Drawing.Size(136, 38);
            this.Param_Min.TabIndex = 97;
            this.Param_Min.ValueChanged += new System.EventHandler(this.Param_Max_ValueChanged);
            this.Param_Min.Click += new System.EventHandler(this.Param_Max_ValueChanged);
            // 
            // Loop
            // 
            this.Loop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Loop, 3);
            this.Loop.Location = new System.Drawing.Point(8, 258);
            this.Loop.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Loop.Name = "Loop";
            this.Loop.Size = new System.Drawing.Size(626, 46);
            this.Loop.TabIndex = 35;
            this.Loop.Text = "Loop";
            this.Loop.UseVisualStyleBackColor = true;
            this.Loop.Click += new System.EventHandler(this.Loop_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.button2, 2);
            this.button2.Location = new System.Drawing.Point(650, 258);
            this.button2.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(416, 46);
            this.button2.TabIndex = 34;
            this.button2.Text = ">>";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Forw_Click);
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(8, 481);
            this.label23.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(198, 112);
            this.label23.TabIndex = 107;
            this.label23.Text = "Color Selection";
            // 
            // AxisSelect
            // 
            this.AxisSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AxisSelect.AutoCompleteCustomSource.AddRange(new string[] {
            "X",
            "Y",
            "Z"});
            this.AxisSelect.FormattingEnabled = true;
            this.AxisSelect.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.AxisSelect.Location = new System.Drawing.Point(8, 825);
            this.AxisSelect.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.AxisSelect.Name = "AxisSelect";
            this.AxisSelect.Size = new System.Drawing.Size(198, 39);
            this.AxisSelect.TabIndex = 117;
            this.AxisSelect.Text = "Z";
            this.AxisSelect.SelectedIndexChanged += new System.EventHandler(this.AxisSelect_SelectedIndexChanged);
            this.AxisSelect.Click += new System.EventHandler(this.AxisSelect_SelectedIndexChanged);
            // 
            // Map_Planes
            // 
            this.Map_Planes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Map_Planes, 3);
            this.Map_Planes.FormattingEnabled = true;
            this.Map_Planes.ItemHeight = 31;
            this.Map_Planes.Location = new System.Drawing.Point(8, 601);
            this.Map_Planes.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Map_Planes.Name = "Map_Planes";
            this.tableLayoutPanel1.SetRowSpan(this.Map_Planes, 2);
            this.Map_Planes.Size = new System.Drawing.Size(626, 190);
            this.Map_Planes.TabIndex = 118;
            // 
            // AddPlane
            // 
            this.AddPlane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.AddPlane, 2);
            this.AddPlane.Location = new System.Drawing.Point(8, 897);
            this.AddPlane.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.AddPlane.Name = "AddPlane";
            this.AddPlane.Size = new System.Drawing.Size(412, 44);
            this.AddPlane.TabIndex = 116;
            this.AddPlane.Text = "Add Plane";
            this.AddPlane.UseVisualStyleBackColor = true;
            this.AddPlane.Click += new System.EventHandler(this.AddPlane_Click);
            // 
            // SetFolder
            // 
            this.SetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.SetFolder, 3);
            this.SetFolder.Location = new System.Drawing.Point(6, 953);
            this.SetFolder.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.SetFolder.Name = "SetFolder";
            this.SetFolder.Size = new System.Drawing.Size(630, 52);
            this.SetFolder.TabIndex = 121;
            this.SetFolder.Text = "Select Output Folder";
            this.SetFolder.UseVisualStyleBackColor = true;
            this.SetFolder.Click += new System.EventHandler(this.SetFolder_Click);
            // 
            // Folder_Status
            // 
            this.Folder_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Folder_Status, 2);
            this.Folder_Status.Location = new System.Drawing.Point(650, 957);
            this.Folder_Status.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Folder_Status.Name = "Folder_Status";
            this.Folder_Status.ReadOnly = true;
            this.Folder_Status.Size = new System.Drawing.Size(416, 38);
            this.Folder_Status.TabIndex = 120;
            // 
            // GroundPlane
            // 
            this.GroundPlane.AutoSize = true;
            this.GroundPlane.Checked = true;
            this.GroundPlane.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.GroundPlane, 2);
            this.GroundPlane.Location = new System.Drawing.Point(648, 210);
            this.GroundPlane.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.GroundPlane.Name = "GroundPlane";
            this.GroundPlane.Size = new System.Drawing.Size(340, 34);
            this.GroundPlane.TabIndex = 122;
            this.GroundPlane.Text = "Reflective Ground Plane";
            this.GroundPlane.UseVisualStyleBackColor = true;
            // 
            // Magnitude
            // 
            this.Magnitude.AutoSize = true;
            this.Magnitude.Location = new System.Drawing.Point(864, 897);
            this.Magnitude.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Magnitude.Name = "Magnitude";
            this.Magnitude.Size = new System.Drawing.Size(172, 35);
            this.Magnitude.TabIndex = 119;
            this.Magnitude.Text = "Magnitude";
            this.Magnitude.UseVisualStyleBackColor = true;
            // 
            // Color_Selection
            // 
            this.Color_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Color_Selection.FormattingEnabled = true;
            this.Color_Selection.Items.AddRange(new object[] {
            "R-O-Y-G-B-I-V",
            "R-O-Y",
            "Y-G-B",
            "W-B",
            "R-M-B"});
            this.Color_Selection.Location = new System.Drawing.Point(222, 489);
            this.Color_Selection.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Color_Selection.Name = "Color_Selection";
            this.Color_Selection.Size = new System.Drawing.Size(198, 39);
            this.Color_Selection.TabIndex = 100;
            this.Color_Selection.Text = "R-O-Y-G-B-I-V";
            this.Color_Selection.SelectedIndexChanged += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
            this.Color_Selection.Click += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
            // 
            // Pos_Select
            // 
            this.Pos_Select.Location = new System.Drawing.Point(222, 825);
            this.Pos_Select.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Pos_Select.Name = "Pos_Select";
            this.Pos_Select.Size = new System.Drawing.Size(198, 38);
            this.Pos_Select.TabIndex = 109;
            this.Pos_Select.ValueChanged += new System.EventHandler(this.Pos_Select_ValueChanged);
            this.Pos_Select.Click += new System.EventHandler(this.Pos_Select_ValueChanged);
            // 
            // DeletePlane
            // 
            this.DeletePlane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.DeletePlane, 2);
            this.DeletePlane.Location = new System.Drawing.Point(436, 897);
            this.DeletePlane.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.DeletePlane.Name = "DeletePlane";
            this.DeletePlane.Size = new System.Drawing.Size(412, 44);
            this.DeletePlane.TabIndex = 123;
            this.DeletePlane.Text = "Delete Plane";
            this.DeletePlane.UseVisualStyleBackColor = true;
            this.DeletePlane.Click += new System.EventHandler(this.DeletePlane_Click);
            // 
            // VisualPML
            // 
            this.VisualPML.AutoSize = true;
            this.VisualPML.Checked = true;
            this.VisualPML.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VisualPML.Location = new System.Drawing.Point(434, 210);
            this.VisualPML.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.VisualPML.Name = "VisualPML";
            this.VisualPML.Size = new System.Drawing.Size(101, 34);
            this.VisualPML.TabIndex = 124;
            this.VisualPML.Text = "PML";
            this.VisualPML.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VisualPML.UseVisualStyleBackColor = true;
            // 
            // Pach_TD_Numeric_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Name = "Pach_TD_Numeric_Control";
            this.Size = new System.Drawing.Size(1122, 1519);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Scatter_Scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sample_Depth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatteringRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Trackbar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Trackbar2)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Frequency_Selection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pos_Select)).EndInit();
            this.ResumeLayout(false);

            }
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem DirectionalSourceToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromMeshSphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem2;
            internal System.Windows.Forms.ToolStripMenuItem SelectASphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromSphereObjectToolStripMenuItem1;
            internal System.Windows.Forms.Button Forw;
            internal System.Windows.Forms.Button Rev;
            internal System.Windows.Forms.Label label4;
            internal System.Windows.Forms.ComboBox comboBox1;
            internal System.Windows.Forms.Label Label16;
            internal System.Windows.Forms.Label Label15;
            internal System.Windows.Forms.ComboBox SourceSelection;
            internal System.Windows.Forms.ComboBox RoomSelection;
            private System.Windows.Forms.TabControl tabControl1;
            private System.Windows.Forms.TabPage tabPage1;
            private System.Windows.Forms.TabPage tabPage2;
            internal System.Windows.Forms.GroupBox groupBox3;
            private System.Windows.Forms.CheckBox EdgeFreq;
            internal System.Windows.Forms.Label Label21;
            internal System.Windows.Forms.ComboBox Atten_Method;
            internal System.Windows.Forms.Label Label19;
            internal System.Windows.Forms.NumericUpDown Air_Pressure;
            internal System.Windows.Forms.Label Label3;
            internal System.Windows.Forms.NumericUpDown Rel_Humidity;
            internal System.Windows.Forms.Label AirTemp;
            internal System.Windows.Forms.NumericUpDown Air_Temp;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            internal System.Windows.Forms.Button Preview;
            private System.Windows.Forms.Label Param3_4;
            private System.Windows.Forms.Label Param1_2;
            private System.Windows.Forms.NumericUpDown Param_Max;
            private System.Windows.Forms.PictureBox Param_Scale;
            private System.Windows.Forms.GroupBox groupBox4;
            private System.Windows.Forms.Label Time_Preview;
            private System.Windows.Forms.Label Param1_4;
            private System.Windows.Forms.NumericUpDown Param_Min;
            internal System.Windows.Forms.Button Loop;
            internal System.Windows.Forms.Button button2;
            internal System.Windows.Forms.ComboBox Color_Selection;
            private System.Windows.Forms.Label label23;
            private System.Windows.Forms.NumericUpDown Pos_Select;
            internal System.Windows.Forms.ComboBox AxisSelect;
            private System.Windows.Forms.ListBox Map_Planes;
            private System.Windows.Forms.Button AddPlane;
            private System.Windows.Forms.CheckBox Magnitude;
            internal System.Windows.Forms.Label label1;
            internal System.Windows.Forms.NumericUpDown Frequency_Selection;
            internal System.Windows.Forms.Label label10;
            internal System.Windows.Forms.NumericUpDown Freq_Max;
            internal System.Windows.Forms.Label label8;
            internal System.Windows.Forms.ComboBox SourceSelect;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.ComboBox Eigen_Extent;
            private System.Windows.Forms.Button CalculateSim;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
            internal System.Windows.Forms.Label label20;
            internal System.Windows.Forms.ComboBox Receiver_Choice;
            internal System.Windows.Forms.NumericUpDown CO_TIME;
            internal System.Windows.Forms.Label label5;
            private System.Windows.Forms.Button Export_Signal;
            private ZedGraph.ZedGraphControl Frequency_View;
            private ZedGraph.ZedGraphControl TransientView;
            private System.Windows.Forms.ListBox EigenFrequencies;
            private System.Windows.Forms.Label label6;
            internal System.Windows.Forms.TextBox Folder_Status;
            private System.Windows.Forms.Button SetFolder;
            private System.Windows.Forms.TabPage tabPage3;
            private System.Windows.Forms.Button CalculateScattering;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
            private System.Windows.Forms.Button button1;
            private ZedGraph.ZedGraphControl ScatteringGraph;
            private System.Windows.Forms.Label label9;
            private System.Windows.Forms.ComboBox Analysis_Technique;
            private System.Windows.Forms.Label label7;
            private System.Windows.Forms.NumericUpDown ScatteringRadius;
            private System.Windows.Forms.Label label11;
            private System.Windows.Forms.NumericUpDown Sample_Depth;
            private System.Windows.Forms.Label label12;
            private System.Windows.Forms.ComboBox comboBox2;
            private System.Windows.Forms.Label label14;
            private System.Windows.Forms.Label label13;
            private System.Windows.Forms.Label label17;
            private System.Windows.Forms.PictureBox Scatter_Scale;
            private System.Windows.Forms.Label Scat_Mid;
            internal System.Windows.Forms.ComboBox Scat_Color;
            private System.Windows.Forms.Label Freq_Feedback;
            private System.Windows.Forms.Label label26;
            private System.Windows.Forms.NumericUpDown ScatMax;
            private System.Windows.Forms.NumericUpDown ScatMin;
            private System.Windows.Forms.TrackBar Freq_Trackbar1;
            private System.Windows.Forms.TrackBar Freq_Trackbar2;
            private System.Windows.Forms.Label label18;
            internal System.Windows.Forms.ComboBox Scat_Param_Select;
            private System.Windows.Forms.GroupBox groupBox5;
            private System.Windows.Forms.CheckBox Scat_Dir_75;
            private System.Windows.Forms.CheckBox Scat_Dir_60;
            private System.Windows.Forms.CheckBox Scat_Dir_45;
            private System.Windows.Forms.CheckBox Scat_Dir_30;
            private System.Windows.Forms.CheckBox Scat_Dir_15;
            private System.Windows.Forms.CheckBox Scat_Dir_00;
            private System.Windows.Forms.CheckBox GroundPlane;
            private System.Windows.Forms.Button DeletePlane;
            private System.Windows.Forms.CheckBox EigenPML;
            private System.Windows.Forms.CheckBox VisualPML;
        }
    }
}