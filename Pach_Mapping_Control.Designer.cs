﻿//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
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
        partial class Pach_Mapping_Control : System.Windows.Forms.UserControl
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
            this.Discretize = new System.Windows.Forms.CheckBox();
            this.FromPointInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DirectionalSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromMeshSphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectASphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromSphereObjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.EdgeFreq = new System.Windows.Forms.CheckBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.Atten_Method = new System.Windows.Forms.ComboBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.Air_Pressure = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.Rel_Humidity = new System.Windows.Forms.NumericUpDown();
            this.AirTemp = new System.Windows.Forms.Label();
            this.Air_Temp = new System.Windows.Forms.NumericUpDown();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Sum_Time = new System.Windows.Forms.CheckBox();
            this.DirectionalToggle = new System.Windows.Forms.CheckBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Increment = new System.Windows.Forms.NumericUpDown();
            this.COTime = new System.Windows.Forms.Label();
            this.CO_TIME = new System.Windows.Forms.NumericUpDown();
            this.Label2 = new System.Windows.Forms.Label();
            this.RT_Count = new System.Windows.Forms.NumericUpDown();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.Rec_Centroid = new System.Windows.Forms.RadioButton();
            this.Rec_Vertex = new System.Windows.Forms.RadioButton();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.Disp_Audience = new System.Windows.Forms.RadioButton();
            this.Disp_Other = new System.Windows.Forms.RadioButton();
            this.Offset_Mesh = new System.Windows.Forms.CheckBox();
            this.Select_Map = new System.Windows.Forms.Button();
            this.Calculate = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SourceList = new System.Windows.Forms.CheckedListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.ZeroAtSource = new System.Windows.Forms.RadioButton();
            this.ZeroAtDirect = new System.Windows.Forms.RadioButton();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.Coherent = new System.Windows.Forms.RadioButton();
            this.Incoherent = new System.Windows.Forms.RadioButton();
            this.Octave = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.End_Time_Control = new System.Windows.Forms.NumericUpDown();
            this.Start_Time_Control = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.Parameter_Selection = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.Color_Selection = new System.Windows.Forms.ComboBox();
            this.Param1_4 = new System.Windows.Forms.Label();
            this.Param1_2 = new System.Windows.Forms.Label();
            this.Param_Min = new System.Windows.Forms.NumericUpDown();
            this.Param_Max = new System.Windows.Forms.NumericUpDown();
            this.Param3_4 = new System.Windows.Forms.Label();
            this.Param_Scale = new System.Windows.Forms.PictureBox();
            this.Plot_Values = new System.Windows.Forms.Button();
            this.Calculate_Map = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Folder_Status = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.OpenFolder = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.Max_Time_out = new System.Windows.Forms.Label();
            this.Min_Time_out = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.Start_Over = new System.Windows.Forms.Button();
            this.Back_Step = new System.Windows.Forms.Button();
            this.Forw_Step = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Tick_Select = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.Integration_select = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.Step_Select = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.T_End_select = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.T_Start_Select = new System.Windows.Forms.NumericUpDown();
            this.Flip_Toggle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Increment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).BeginInit();
            this.GroupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.End_Time_Control)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Start_Time_Control)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tick_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Integration_select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Step_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_End_select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_Start_Select)).BeginInit();
            this.SuspendLayout();
            // 
            // Discretize
            // 
            this.Discretize.AutoSize = true;
            this.Discretize.Location = new System.Drawing.Point(7, 261);
            this.Discretize.Name = "Discretize";
            this.Discretize.Size = new System.Drawing.Size(104, 17);
            this.Discretize.TabIndex = 83;
            this.Discretize.Text = "Discretize Colors";
            this.Discretize.UseVisualStyleBackColor = true;
            this.Discretize.CheckedChanged += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
            // 
            // FromPointInputToolStripMenuItem
            // 
            this.FromPointInputToolStripMenuItem.Name = "FromPointInputToolStripMenuItem";
            this.FromPointInputToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
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
            this.FromMeshSphereToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
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
            this.FromPointInputToolStripMenuItem2.Size = new System.Drawing.Size(179, 22);
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
            this.FromSphereObjectToolStripMenuItem1.Size = new System.Drawing.Size(179, 22);
            this.FromSphereObjectToolStripMenuItem1.Text = "From Sphere Object";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(193, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "label4";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Location = new System.Drawing.Point(234, 345);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 396);
            this.pictureBox2.TabIndex = 36;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Location = new System.Drawing.Point(6, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 129);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Environmental Factors";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Method:";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Evans & Bazley (Indoor Attenuation)",
            "ISO 9613-1 (Outdoor Attenuation)"});
            this.comboBox1.Location = new System.Drawing.Point(70, 96);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(201, 21);
            this.comboBox1.TabIndex = 14;
            this.comboBox1.Text = "Evans & Bazley (Indoor Attenuation)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Static Air Pressure (hPa)";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.numericUpDown1.Location = new System.Drawing.Point(207, 70);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown1.TabIndex = 13;
            this.numericUpDown1.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Relative Humidity(%)";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown2.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.numericUpDown2.Location = new System.Drawing.Point(239, 44);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(32, 20);
            this.numericUpDown2.TabIndex = 12;
            this.numericUpDown2.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Air Temperature (C)";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown3.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.numericUpDown3.Location = new System.Drawing.Point(239, 18);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(32, 20);
            this.numericUpDown3.TabIndex = 11;
            this.numericUpDown3.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 118);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Simulation Settings";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 189);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Map Increment (cm.)";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown4.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.numericUpDown4.Location = new System.Drawing.Point(251, 187);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(32, 20);
            this.numericUpDown4.TabIndex = 24;
            this.numericUpDown4.UseWaitCursor = true;
            this.numericUpDown4.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.comboBox2);
            this.groupBox3.Controls.Add(this.comboBox3);
            this.groupBox3.Location = new System.Drawing.Point(6, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 110);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Geometry";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 75);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(268, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Select Mapping Surfaces";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Source:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(2, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(38, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Room:";
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "From Point Input",
            "From Source Directivity Data"});
            this.comboBox2.Location = new System.Drawing.Point(61, 43);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(213, 21);
            this.comboBox2.TabIndex = 27;
            this.comboBox2.Text = "Select Source...";
            // 
            // comboBox3
            // 
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Use Entire Model",
            "Select Surfaces"});
            this.comboBox3.Location = new System.Drawing.Point(61, 16);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(213, 21);
            this.comboBox3.TabIndex = 26;
            this.comboBox3.Text = "Use Entire Model";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(21, 164);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 13);
            this.label17.TabIndex = 17;
            this.label17.Text = "Cut Off Time (ms)";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown5.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.numericUpDown5.Location = new System.Drawing.Point(216, 162);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(67, 20);
            this.numericUpDown5.TabIndex = 16;
            this.numericUpDown5.UseWaitCursor = true;
            this.numericUpDown5.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(21, 138);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "Number of Rays";
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown6.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.numericUpDown6.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown6.Location = new System.Drawing.Point(216, 136);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            150000,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(67, 20);
            this.numericUpDown6.TabIndex = 7;
            this.numericUpDown6.UseWaitCursor = true;
            this.numericUpDown6.Value = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(5, 746);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(281, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Animate";
            this.button2.UseVisualStyleBackColor = true;
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
            // openDataToolStripMenuItem
            // 
            this.openDataToolStripMenuItem.Name = "openDataToolStripMenuItem";
            this.openDataToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openDataToolStripMenuItem.Text = "Open Data...";
            this.openDataToolStripMenuItem.Click += new System.EventHandler(this.OpenDataToolStripMenuItem_Click);
            // 
            // saveDataToolStripMenuItem
            // 
            this.saveDataToolStripMenuItem.Name = "saveDataToolStripMenuItem";
            this.saveDataToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveDataToolStripMenuItem.Text = "Save Data...";
            this.saveDataToolStripMenuItem.Click += new System.EventHandler(this.SaveDataToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(400, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(400, 671);
            this.tabControl1.TabIndex = 81;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.GroupBox4);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.GroupBox2);
            this.tabPage1.Controls.Add(this.Calculate);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(392, 645);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Calculation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // GroupBox4
            // 
            this.GroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox4.Controls.Add(this.EdgeFreq);
            this.GroupBox4.Controls.Add(this.Label21);
            this.GroupBox4.Controls.Add(this.Atten_Method);
            this.GroupBox4.Controls.Add(this.Label19);
            this.GroupBox4.Controls.Add(this.Air_Pressure);
            this.GroupBox4.Controls.Add(this.label7);
            this.GroupBox4.Controls.Add(this.Rel_Humidity);
            this.GroupBox4.Controls.Add(this.AirTemp);
            this.GroupBox4.Controls.Add(this.Air_Temp);
            this.GroupBox4.Location = new System.Drawing.Point(5, 261);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(380, 133);
            this.GroupBox4.TabIndex = 84;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Environmental Factors";
            // 
            // EdgeFreq
            // 
            this.EdgeFreq.AutoSize = true;
            this.EdgeFreq.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EdgeFreq.Location = new System.Drawing.Point(70, 113);
            this.EdgeFreq.Name = "EdgeFreq";
            this.EdgeFreq.Size = new System.Drawing.Size(155, 17);
            this.EdgeFreq.TabIndex = 36;
            this.EdgeFreq.Text = "Edge Frequency Correction";
            this.EdgeFreq.UseVisualStyleBackColor = true;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(11, 91);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(46, 13);
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
            this.Atten_Method.Location = new System.Drawing.Point(70, 88);
            this.Atten_Method.Name = "Atten_Method";
            this.Atten_Method.Size = new System.Drawing.Size(301, 21);
            this.Atten_Method.TabIndex = 14;
            this.Atten_Method.Text = "ISO 9613-1 (Outdoor Attenuation)";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(8, 66);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(121, 13);
            this.Label19.TabIndex = 32;
            this.Label19.Text = "Static Air Pressure (hPa)";
            // 
            // Air_Pressure
            // 
            this.Air_Pressure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Pressure.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Pressure.Location = new System.Drawing.Point(307, 64);
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
            this.Air_Pressure.Size = new System.Drawing.Size(64, 20);
            this.Air_Pressure.TabIndex = 13;
            this.Air_Pressure.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Relative Humidity(%)";
            // 
            // Rel_Humidity
            // 
            this.Rel_Humidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Rel_Humidity.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Rel_Humidity.Location = new System.Drawing.Point(328, 40);
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
            this.Rel_Humidity.Size = new System.Drawing.Size(43, 20);
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
            this.AirTemp.Location = new System.Drawing.Point(8, 18);
            this.AirTemp.Name = "AirTemp";
            this.AirTemp.Size = new System.Drawing.Size(98, 13);
            this.AirTemp.TabIndex = 28;
            this.AirTemp.Text = "Air Temperature (C)";
            // 
            // Air_Temp
            // 
            this.Air_Temp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Temp.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Temp.Location = new System.Drawing.Point(328, 16);
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
            this.Air_Temp.Size = new System.Drawing.Size(43, 20);
            this.Air_Temp.TabIndex = 11;
            this.Air_Temp.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.Sum_Time);
            this.groupBox6.Controls.Add(this.DirectionalToggle);
            this.groupBox6.Controls.Add(this.Label1);
            this.groupBox6.Controls.Add(this.Increment);
            this.groupBox6.Controls.Add(this.COTime);
            this.groupBox6.Controls.Add(this.CO_TIME);
            this.groupBox6.Controls.Add(this.Label2);
            this.groupBox6.Controls.Add(this.RT_Count);
            this.groupBox6.Location = new System.Drawing.Point(6, 145);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(380, 110);
            this.groupBox6.TabIndex = 83;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Simulation Settings";
            // 
            // Sum_Time
            // 
            this.Sum_Time.AutoSize = true;
            this.Sum_Time.Location = new System.Drawing.Point(37, 87);
            this.Sum_Time.Name = "Sum_Time";
            this.Sum_Time.Size = new System.Drawing.Size(132, 17);
            this.Sum_Time.TabIndex = 69;
            this.Sum_Time.Text = "SPL Only (sum of time)";
            this.Sum_Time.UseVisualStyleBackColor = true;
            this.Sum_Time.CheckedChanged += new System.EventHandler(this.Sum_Time_CheckedChanged);
            // 
            // DirectionalToggle
            // 
            this.DirectionalToggle.AutoSize = true;
            this.DirectionalToggle.Location = new System.Drawing.Point(174, 87);
            this.DirectionalToggle.Name = "DirectionalToggle";
            this.DirectionalToggle.Size = new System.Drawing.Size(117, 17);
            this.DirectionalToggle.TabIndex = 68;
            this.DirectionalToggle.Text = "Track Directionality";
            this.DirectionalToggle.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(9, 66);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(104, 13);
            this.Label1.TabIndex = 67;
            this.Label1.Text = "Map Increment (cm.)";
            // 
            // Increment
            // 
            this.Increment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Increment.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Increment.Location = new System.Drawing.Point(174, 64);
            this.Increment.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.Increment.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.Increment.Name = "Increment";
            this.Increment.Size = new System.Drawing.Size(198, 20);
            this.Increment.TabIndex = 66;
            this.Increment.UseWaitCursor = true;
            this.Increment.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // COTime
            // 
            this.COTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.COTime.AutoSize = true;
            this.COTime.Location = new System.Drawing.Point(9, 41);
            this.COTime.Name = "COTime";
            this.COTime.Size = new System.Drawing.Size(88, 13);
            this.COTime.TabIndex = 65;
            this.COTime.Text = "Cut Off Time (ms)";
            // 
            // CO_TIME
            // 
            this.CO_TIME.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CO_TIME.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.CO_TIME.Location = new System.Drawing.Point(174, 39);
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
            this.CO_TIME.Size = new System.Drawing.Size(198, 20);
            this.CO_TIME.TabIndex = 64;
            this.CO_TIME.UseWaitCursor = true;
            this.CO_TIME.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // Label2
            // 
            this.Label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(9, 15);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(83, 13);
            this.Label2.TabIndex = 63;
            this.Label2.Text = "Number of Rays";
            // 
            // RT_Count
            // 
            this.RT_Count.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RT_Count.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.RT_Count.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RT_Count.Location = new System.Drawing.Point(174, 13);
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
            this.RT_Count.Size = new System.Drawing.Size(198, 20);
            this.RT_Count.TabIndex = 62;
            this.RT_Count.UseWaitCursor = true;
            this.RT_Count.Value = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            // 
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.tableLayoutPanel1);
            this.GroupBox2.Controls.Add(this.Offset_Mesh);
            this.GroupBox2.Controls.Add(this.Select_Map);
            this.GroupBox2.Location = new System.Drawing.Point(6, 6);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(380, 136);
            this.GroupBox2.TabIndex = 82;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Geometry";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox11, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox10, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 64);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(380, 72);
            this.tableLayoutPanel1.TabIndex = 85;
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox11.Controls.Add(this.Rec_Centroid);
            this.groupBox11.Controls.Add(this.Rec_Vertex);
            this.groupBox11.Location = new System.Drawing.Point(193, 3);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(184, 66);
            this.groupBox11.TabIndex = 35;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Receiver Orientation";
            // 
            // Rec_Centroid
            // 
            this.Rec_Centroid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Rec_Centroid.AutoSize = true;
            this.Rec_Centroid.Checked = true;
            this.Rec_Centroid.Location = new System.Drawing.Point(8, 19);
            this.Rec_Centroid.Name = "Rec_Centroid";
            this.Rec_Centroid.Size = new System.Drawing.Size(172, 17);
            this.Rec_Centroid.TabIndex = 32;
            this.Rec_Centroid.TabStop = true;
            this.Rec_Centroid.Text = "Face Centroid (no interpolation)";
            this.Rec_Centroid.UseVisualStyleBackColor = true;
            // 
            // Rec_Vertex
            // 
            this.Rec_Vertex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Rec_Vertex.AutoSize = true;
            this.Rec_Vertex.Location = new System.Drawing.Point(8, 41);
            this.Rec_Vertex.Name = "Rec_Vertex";
            this.Rec_Vertex.Size = new System.Drawing.Size(141, 17);
            this.Rec_Vertex.TabIndex = 33;
            this.Rec_Vertex.Text = "Vertex (false color interp)";
            this.Rec_Vertex.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.Controls.Add(this.Disp_Audience);
            this.groupBox10.Controls.Add(this.Disp_Other);
            this.groupBox10.Location = new System.Drawing.Point(3, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(184, 66);
            this.groupBox10.TabIndex = 34;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Receiver Displacement";
            // 
            // Disp_Audience
            // 
            this.Disp_Audience.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Disp_Audience.AutoSize = true;
            this.Disp_Audience.Checked = true;
            this.Disp_Audience.Location = new System.Drawing.Point(6, 18);
            this.Disp_Audience.Name = "Disp_Audience";
            this.Disp_Audience.Size = new System.Drawing.Size(155, 17);
            this.Disp_Audience.TabIndex = 32;
            this.Disp_Audience.TabStop = true;
            this.Disp_Audience.Text = "Audience (+z displacement)";
            this.Disp_Audience.UseVisualStyleBackColor = true;
            // 
            // Disp_Other
            // 
            this.Disp_Other.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Disp_Other.AutoSize = true;
            this.Disp_Other.Location = new System.Drawing.Point(6, 40);
            this.Disp_Other.Name = "Disp_Other";
            this.Disp_Other.Size = new System.Drawing.Size(158, 17);
            this.Disp_Other.TabIndex = 33;
            this.Disp_Other.Text = "Other (Normal displacement)";
            this.Disp_Other.UseVisualStyleBackColor = true;
            // 
            // Offset_Mesh
            // 
            this.Offset_Mesh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Offset_Mesh.AutoSize = true;
            this.Offset_Mesh.Checked = true;
            this.Offset_Mesh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Offset_Mesh.Location = new System.Drawing.Point(223, 19);
            this.Offset_Mesh.Name = "Offset_Mesh";
            this.Offset_Mesh.Size = new System.Drawing.Size(147, 17);
            this.Offset_Mesh.TabIndex = 36;
            this.Offset_Mesh.Text = "Mesh Offset by Increment";
            this.Offset_Mesh.UseVisualStyleBackColor = true;
            // 
            // Select_Map
            // 
            this.Select_Map.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Select_Map.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Select_Map.Location = new System.Drawing.Point(6, 17);
            this.Select_Map.Name = "Select_Map";
            this.Select_Map.Size = new System.Drawing.Size(211, 41);
            this.Select_Map.TabIndex = 31;
            this.Select_Map.Text = "Select Mapping Surfaces";
            this.Select_Map.UseVisualStyleBackColor = true;
            this.Select_Map.Click += new System.EventHandler(this.Select_Map_Click);
            // 
            // Calculate
            // 
            this.Calculate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Calculate.Location = new System.Drawing.Point(6, 399);
            this.Calculate.Margin = new System.Windows.Forms.Padding(2);
            this.Calculate.Name = "Calculate";
            this.Calculate.Size = new System.Drawing.Size(380, 30);
            this.Calculate.TabIndex = 81;
            this.Calculate.Text = "Run Calculation";
            this.Calculate.UseVisualStyleBackColor = true;
            this.Calculate.Click += new System.EventHandler(this.Calculate_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.SourceList);
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Controls.Add(this.Plot_Values);
            this.tabPage2.Controls.Add(this.Calculate_Map);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(392, 645);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Output";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SourceList
            // 
            this.SourceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceList.FormattingEnabled = true;
            this.SourceList.Location = new System.Drawing.Point(6, 6);
            this.SourceList.Name = "SourceList";
            this.SourceList.Size = new System.Drawing.Size(377, 274);
            this.SourceList.TabIndex = 83;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(6, 286);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox9);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox8);
            this.splitContainer1.Panel1.Controls.Add(this.Octave);
            this.splitContainer1.Panel1.Controls.Add(this.label22);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox5);
            this.splitContainer1.Panel1.Controls.Add(this.label24);
            this.splitContainer1.Panel1.Controls.Add(this.Parameter_Selection);
            this.splitContainer1.Panel1.Controls.Add(this.label23);
            this.splitContainer1.Panel1.Controls.Add(this.Color_Selection);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Param1_4);
            this.splitContainer1.Panel2.Controls.Add(this.Param1_2);
            this.splitContainer1.Panel2.Controls.Add(this.Discretize);
            this.splitContainer1.Panel2.Controls.Add(this.Param_Min);
            this.splitContainer1.Panel2.Controls.Add(this.Param_Max);
            this.splitContainer1.Panel2.Controls.Add(this.Param3_4);
            this.splitContainer1.Panel2.Controls.Add(this.Param_Scale);
            this.splitContainer1.Size = new System.Drawing.Size(380, 281);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 82;
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.ZeroAtSource);
            this.groupBox9.Controls.Add(this.ZeroAtDirect);
            this.groupBox9.Location = new System.Drawing.Point(89, 213);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(135, 65);
            this.groupBox9.TabIndex = 93;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Zero Time At:";
            // 
            // ZeroAtSource
            // 
            this.ZeroAtSource.AutoSize = true;
            this.ZeroAtSource.Location = new System.Drawing.Point(6, 42);
            this.ZeroAtSource.Name = "ZeroAtSource";
            this.ZeroAtSource.Size = new System.Drawing.Size(107, 17);
            this.ZeroAtSource.TabIndex = 90;
            this.ZeroAtSource.Text = "Source Actuation";
            this.ZeroAtSource.UseVisualStyleBackColor = true;
            // 
            // ZeroAtDirect
            // 
            this.ZeroAtDirect.AutoSize = true;
            this.ZeroAtDirect.Checked = true;
            this.ZeroAtDirect.Location = new System.Drawing.Point(6, 19);
            this.ZeroAtDirect.Name = "ZeroAtDirect";
            this.ZeroAtDirect.Size = new System.Drawing.Size(87, 17);
            this.ZeroAtDirect.TabIndex = 91;
            this.ZeroAtDirect.TabStop = true;
            this.ZeroAtDirect.Text = "Direct Sound";
            this.ZeroAtDirect.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.Coherent);
            this.groupBox8.Controls.Add(this.Incoherent);
            this.groupBox8.Location = new System.Drawing.Point(3, 213);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(80, 65);
            this.groupBox8.TabIndex = 92;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "IR Summing";
            // 
            // Coherent
            // 
            this.Coherent.AutoSize = true;
            this.Coherent.Location = new System.Drawing.Point(6, 42);
            this.Coherent.Name = "Coherent";
            this.Coherent.Size = new System.Drawing.Size(66, 17);
            this.Coherent.TabIndex = 90;
            this.Coherent.Text = "Pressure";
            this.Coherent.UseVisualStyleBackColor = true;
            this.Coherent.CheckedChanged += new System.EventHandler(this.Coherent_CheckedChanged);
            // 
            // Incoherent
            // 
            this.Incoherent.AutoSize = true;
            this.Incoherent.Checked = true;
            this.Incoherent.Location = new System.Drawing.Point(6, 19);
            this.Incoherent.Name = "Incoherent";
            this.Incoherent.Size = new System.Drawing.Size(64, 17);
            this.Incoherent.TabIndex = 91;
            this.Incoherent.TabStop = true;
            this.Incoherent.Text = "Intensity";
            this.Incoherent.UseVisualStyleBackColor = true;
            // 
            // Octave
            // 
            this.Octave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.Octave.Location = new System.Drawing.Point(5, 115);
            this.Octave.Name = "Octave";
            this.Octave.Size = new System.Drawing.Size(219, 21);
            this.Octave.TabIndex = 89;
            this.Octave.Text = "Summation: All Octaves";
            this.Octave.SelectionChangeCommitted += new System.EventHandler(this.Parameter_Selection_SelectedIndexChanged);
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
            this.groupBox5.Controls.Add(this.End_Time_Control);
            this.groupBox5.Controls.Add(this.Start_Time_Control);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Location = new System.Drawing.Point(3, 142);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(221, 66);
            this.groupBox5.TabIndex = 87;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Time Interval";
            // 
            // End_Time_Control
            // 
            this.End_Time_Control.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.End_Time_Control.Location = new System.Drawing.Point(85, 38);
            this.End_Time_Control.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.End_Time_Control.Name = "End_Time_Control";
            this.End_Time_Control.Size = new System.Drawing.Size(130, 20);
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
            this.Start_Time_Control.Location = new System.Drawing.Point(85, 12);
            this.Start_Time_Control.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Start_Time_Control.Name = "Start_Time_Control";
            this.Start_Time_Control.Size = new System.Drawing.Size(130, 20);
            this.Start_Time_Control.TabIndex = 39;
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
            // Parameter_Selection
            // 
            this.Parameter_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Parameter_Selection.ForeColor = System.Drawing.Color.Black;
            this.Parameter_Selection.FormattingEnabled = true;
            this.Parameter_Selection.Items.AddRange(new object[] {
            "Sound Pressure Level",
            "Sound Pressure Level (A-weighted)",
            "Directionality",
            "Reverberation Time (T-15)",
            "Reverberation Time (T-30)",
            "Speech Transmission Index - 2003",
            "Speech Transmission Index - Male",
            "Speech Transmission Index - Female",
            "Early Decay Time (EDT)",
            "Clarity (C-80)",
            "Definition (D-50)",
            "Strength/Loudness (G)",
            "Percent who perceive echoes (EK)"});
            this.Parameter_Selection.Location = new System.Drawing.Point(5, 69);
            this.Parameter_Selection.Name = "Parameter_Selection";
            this.Parameter_Selection.Size = new System.Drawing.Size(219, 21);
            this.Parameter_Selection.TabIndex = 85;
            this.Parameter_Selection.Text = "Sound Pressure Level";
            this.Parameter_Selection.TextChanged += new System.EventHandler(this.Parameter_Selection_SelectedIndexChanged);
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
            // Color_Selection
            // 
            this.Color_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Color_Selection.FormattingEnabled = true;
            this.Color_Selection.Items.AddRange(new object[] {
            "R-O-Y-G-B-I-V",
            "R-O-Y",
            "Y-G-B",
            "R-M-B",
            "W-B"});
            this.Color_Selection.Location = new System.Drawing.Point(5, 23);
            this.Color_Selection.Name = "Color_Selection";
            this.Color_Selection.Size = new System.Drawing.Size(219, 21);
            this.Color_Selection.TabIndex = 83;
            this.Color_Selection.Text = "R-O-Y-G-B-I-V";
            this.Color_Selection.TextChanged += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
            // 
            // Param1_4
            // 
            this.Param1_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Param1_4.AutoSize = true;
            this.Param1_4.Location = new System.Drawing.Point(8, 179);
            this.Param1_4.Name = "Param1_4";
            this.Param1_4.Size = new System.Drawing.Size(28, 13);
            this.Param1_4.TabIndex = 88;
            this.Param1_4.Text = "22.5";
            // 
            // Param1_2
            // 
            this.Param1_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Param1_2.AutoSize = true;
            this.Param1_2.Location = new System.Drawing.Point(8, 122);
            this.Param1_2.Name = "Param1_2";
            this.Param1_2.Size = new System.Drawing.Size(19, 13);
            this.Param1_2.TabIndex = 87;
            this.Param1_2.Text = "45";
            // 
            // Param_Min
            // 
            this.Param_Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Param_Min.DecimalPlaces = 1;
            this.Param_Min.Location = new System.Drawing.Point(7, 235);
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
            this.Param_Min.TabIndex = 86;
            this.Param_Min.ValueChanged += new System.EventHandler(this.Param_Leave);
            // 
            // Param_Max
            // 
            this.Param_Max.DecimalPlaces = 1;
            this.Param_Max.Location = new System.Drawing.Point(7, 3);
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
            this.Param_Max.TabIndex = 85;
            this.Param_Max.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.Param_Max.ValueChanged += new System.EventHandler(this.Param_Leave);
            // 
            // Param3_4
            // 
            this.Param3_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Param3_4.AutoSize = true;
            this.Param3_4.Location = new System.Drawing.Point(8, 64);
            this.Param3_4.Name = "Param3_4";
            this.Param3_4.Size = new System.Drawing.Size(28, 13);
            this.Param3_4.TabIndex = 84;
            this.Param3_4.Text = "67.5";
            // 
            // Param_Scale
            // 
            this.Param_Scale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Scale.Location = new System.Drawing.Point(62, 3);
            this.Param_Scale.Name = "Param_Scale";
            this.Param_Scale.Size = new System.Drawing.Size(72, 252);
            this.Param_Scale.TabIndex = 83;
            this.Param_Scale.TabStop = false;
            // 
            // Plot_Values
            // 
            this.Plot_Values.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Plot_Values.Location = new System.Drawing.Point(6, 608);
            this.Plot_Values.Margin = new System.Windows.Forms.Padding(2);
            this.Plot_Values.Name = "Plot_Values";
            this.Plot_Values.Size = new System.Drawing.Size(380, 32);
            this.Plot_Values.TabIndex = 78;
            this.Plot_Values.Text = "Plot Numerical Values";
            this.Plot_Values.UseVisualStyleBackColor = true;
            this.Plot_Values.Click += new System.EventHandler(this.Plot_Values_Click);
            // 
            // Calculate_Map
            // 
            this.Calculate_Map.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Calculate_Map.Location = new System.Drawing.Point(6, 572);
            this.Calculate_Map.Margin = new System.Windows.Forms.Padding(2);
            this.Calculate_Map.Name = "Calculate_Map";
            this.Calculate_Map.Size = new System.Drawing.Size(380, 32);
            this.Calculate_Map.TabIndex = 77;
            this.Calculate_Map.Text = "Create Map";
            this.Calculate_Map.UseVisualStyleBackColor = true;
            this.Calculate_Map.Click += new System.EventHandler(this.Calculate_Map_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.Folder_Status);
            this.tabPage3.Controls.Add(this.label28);
            this.tabPage3.Controls.Add(this.OpenFolder);
            this.tabPage3.Controls.Add(this.label30);
            this.tabPage3.Controls.Add(this.Max_Time_out);
            this.tabPage3.Controls.Add(this.Min_Time_out);
            this.tabPage3.Controls.Add(this.label27);
            this.tabPage3.Controls.Add(this.Start_Over);
            this.tabPage3.Controls.Add(this.Back_Step);
            this.tabPage3.Controls.Add(this.Forw_Step);
            this.tabPage3.Controls.Add(this.groupBox7);
            this.tabPage3.Controls.Add(this.Flip_Toggle);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(392, 645);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "T-Flip";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Folder_Status
            // 
            this.Folder_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Folder_Status.Location = new System.Drawing.Point(6, 213);
            this.Folder_Status.Name = "Folder_Status";
            this.Folder_Status.ReadOnly = true;
            this.Folder_Status.Size = new System.Drawing.Size(380, 20);
            this.Folder_Status.TabIndex = 87;
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(7, 187);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(104, 13);
            this.label28.TabIndex = 88;
            this.label28.Text = "Select Output Folder";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OpenFolder
            // 
            this.OpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenFolder.Location = new System.Drawing.Point(117, 180);
            this.OpenFolder.Name = "OpenFolder";
            this.OpenFolder.Size = new System.Drawing.Size(269, 27);
            this.OpenFolder.TabIndex = 86;
            this.OpenFolder.Text = "Open...";
            this.OpenFolder.UseVisualStyleBackColor = true;
            this.OpenFolder.Click += new System.EventHandler(this.OpenFolder_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(157, 164);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(59, 13);
            this.label30.TabIndex = 8;
            this.label30.Text = "Max Time: ";
            // 
            // Max_Time_out
            // 
            this.Max_Time_out.AutoSize = true;
            this.Max_Time_out.Location = new System.Drawing.Point(257, 162);
            this.Max_Time_out.Name = "Max_Time_out";
            this.Max_Time_out.Size = new System.Drawing.Size(19, 13);
            this.Max_Time_out.TabIndex = 7;
            this.Max_Time_out.Text = "00";
            // 
            // Min_Time_out
            // 
            this.Min_Time_out.AutoSize = true;
            this.Min_Time_out.Location = new System.Drawing.Point(76, 164);
            this.Min_Time_out.Name = "Min_Time_out";
            this.Min_Time_out.Size = new System.Drawing.Size(19, 13);
            this.Min_Time_out.TabIndex = 6;
            this.Min_Time_out.Text = "00";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(7, 164);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(56, 13);
            this.label27.TabIndex = 5;
            this.label27.Text = "Min Time: ";
            // 
            // Start_Over
            // 
            this.Start_Over.Location = new System.Drawing.Point(6, 239);
            this.Start_Over.Name = "Start_Over";
            this.Start_Over.Size = new System.Drawing.Size(67, 23);
            this.Start_Over.TabIndex = 4;
            this.Start_Over.Text = "|<<";
            this.Start_Over.UseVisualStyleBackColor = true;
            this.Start_Over.Click += new System.EventHandler(this.Start_Over_Click);
            // 
            // Back_Step
            // 
            this.Back_Step.Location = new System.Drawing.Point(79, 239);
            this.Back_Step.Name = "Back_Step";
            this.Back_Step.Size = new System.Drawing.Size(75, 23);
            this.Back_Step.TabIndex = 3;
            this.Back_Step.Text = "<<";
            this.Back_Step.UseVisualStyleBackColor = true;
            this.Back_Step.Click += new System.EventHandler(this.Back_Step_Click);
            // 
            // Forw_Step
            // 
            this.Forw_Step.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Forw_Step.Location = new System.Drawing.Point(315, 239);
            this.Forw_Step.Name = "Forw_Step";
            this.Forw_Step.Size = new System.Drawing.Size(71, 23);
            this.Forw_Step.TabIndex = 2;
            this.Forw_Step.Text = ">>";
            this.Forw_Step.UseVisualStyleBackColor = true;
            this.Forw_Step.Click += new System.EventHandler(this.Forw_Step_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Tick_Select);
            this.groupBox7.Controls.Add(this.label26);
            this.groupBox7.Controls.Add(this.Integration_select);
            this.groupBox7.Controls.Add(this.label25);
            this.groupBox7.Controls.Add(this.Step_Select);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this.T_End_select);
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.T_Start_Select);
            this.groupBox7.Location = new System.Drawing.Point(6, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(380, 153);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Time Parameters";
            // 
            // Tick_Select
            // 
            this.Tick_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tick_Select.Location = new System.Drawing.Point(254, 124);
            this.Tick_Select.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Tick_Select.Name = "Tick_Select";
            this.Tick_Select.Size = new System.Drawing.Size(120, 20);
            this.Tick_Select.TabIndex = 9;
            this.Tick_Select.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(15, 126);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(28, 13);
            this.label26.TabIndex = 8;
            this.label26.Text = "Tick";
            // 
            // Integration_select
            // 
            this.Integration_select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Integration_select.Location = new System.Drawing.Point(254, 98);
            this.Integration_select.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Integration_select.Name = "Integration_select";
            this.Integration_select.Size = new System.Drawing.Size(120, 20);
            this.Integration_select.TabIndex = 7;
            this.Integration_select.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(15, 100);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(96, 13);
            this.label25.TabIndex = 6;
            this.label25.Text = "Integration Domain";
            // 
            // Step_Select
            // 
            this.Step_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Step_Select.Location = new System.Drawing.Point(254, 72);
            this.Step_Select.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Step_Select.Name = "Step_Select";
            this.Step_Select.Size = new System.Drawing.Size(120, 20);
            this.Step_Select.TabIndex = 5;
            this.Step_Select.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(15, 74);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 13);
            this.label16.TabIndex = 4;
            this.label16.Text = "Step Increment";
            // 
            // T_End_select
            // 
            this.T_End_select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.T_End_select.Location = new System.Drawing.Point(254, 45);
            this.T_End_select.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.T_End_select.Name = "T_End_select";
            this.T_End_select.Size = new System.Drawing.Size(120, 20);
            this.T_End_select.TabIndex = 3;
            this.T_End_select.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 47);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(113, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "End Time (Post Direct)";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Start Time (Post Direct)";
            // 
            // T_Start_Select
            // 
            this.T_Start_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.T_Start_Select.Location = new System.Drawing.Point(254, 19);
            this.T_Start_Select.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.T_Start_Select.Name = "T_Start_Select";
            this.T_Start_Select.Size = new System.Drawing.Size(120, 20);
            this.T_Start_Select.TabIndex = 0;
            // 
            // Flip_Toggle
            // 
            this.Flip_Toggle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Flip_Toggle.Location = new System.Drawing.Point(160, 239);
            this.Flip_Toggle.Name = "Flip_Toggle";
            this.Flip_Toggle.Size = new System.Drawing.Size(149, 23);
            this.Flip_Toggle.TabIndex = 1;
            this.Flip_Toggle.Text = "Flip";
            this.Flip_Toggle.UseVisualStyleBackColor = true;
            this.Flip_Toggle.Click += new System.EventHandler(this.Flip_Toggle_Click);
            // 
            // Pach_Mapping_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Pach_Mapping_Control";
            this.Size = new System.Drawing.Size(400, 701);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Increment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).EndInit();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.End_Time_Control)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Start_Time_Control)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tick_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Integration_select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Step_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_End_select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.T_Start_Select)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem DirectionalSourceToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromMeshSphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem2;
            internal System.Windows.Forms.ToolStripMenuItem SelectASphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromSphereObjectToolStripMenuItem1;
            private System.Windows.Forms.Label label4;
            private System.Windows.Forms.PictureBox pictureBox2;
            internal System.Windows.Forms.GroupBox groupBox1;
            internal System.Windows.Forms.Label label5;
            internal System.Windows.Forms.ComboBox comboBox1;
            internal System.Windows.Forms.Label label8;
            internal System.Windows.Forms.NumericUpDown numericUpDown1;
            internal System.Windows.Forms.Label label9;
            internal System.Windows.Forms.NumericUpDown numericUpDown2;
            internal System.Windows.Forms.Label label10;
            internal System.Windows.Forms.NumericUpDown numericUpDown3;
            internal System.Windows.Forms.Label label11;
            internal System.Windows.Forms.Label label12;
            internal System.Windows.Forms.NumericUpDown numericUpDown4;
            internal System.Windows.Forms.GroupBox groupBox3;
            private System.Windows.Forms.Button button1;
            internal System.Windows.Forms.Label label13;
            internal System.Windows.Forms.Label label14;
            internal System.Windows.Forms.ComboBox comboBox2;
            internal System.Windows.Forms.ComboBox comboBox3;
            internal System.Windows.Forms.Label label17;
            internal System.Windows.Forms.NumericUpDown numericUpDown5;
            internal System.Windows.Forms.Label label18;
            internal System.Windows.Forms.NumericUpDown numericUpDown6;
            internal System.Windows.Forms.Button button2;
            private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem openDataToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveDataToolStripMenuItem;
            private System.Windows.Forms.MenuStrip menuStrip1;
            private System.Windows.Forms.TabControl tabControl1;
            private System.Windows.Forms.TabPage tabPage1;
            internal System.Windows.Forms.GroupBox GroupBox4;
            private System.Windows.Forms.CheckBox EdgeFreq;
            internal System.Windows.Forms.Label Label21;
            internal System.Windows.Forms.ComboBox Atten_Method;
            internal System.Windows.Forms.Label Label19;
            internal System.Windows.Forms.NumericUpDown Air_Pressure;
            internal System.Windows.Forms.Label label7;
            internal System.Windows.Forms.NumericUpDown Rel_Humidity;
            internal System.Windows.Forms.Label AirTemp;
            internal System.Windows.Forms.NumericUpDown Air_Temp;
            private System.Windows.Forms.GroupBox groupBox6;
            internal System.Windows.Forms.Label Label1;
            internal System.Windows.Forms.NumericUpDown Increment;
            internal System.Windows.Forms.Label COTime;
            internal System.Windows.Forms.NumericUpDown CO_TIME;
            internal System.Windows.Forms.Label Label2;
            internal System.Windows.Forms.NumericUpDown RT_Count;
            internal System.Windows.Forms.GroupBox GroupBox2;
            private System.Windows.Forms.Button Select_Map;
            internal System.Windows.Forms.Button Calculate;
            private System.Windows.Forms.TabPage tabPage2;
            private System.Windows.Forms.SplitContainer splitContainer1;
            internal System.Windows.Forms.ComboBox Octave;
            private System.Windows.Forms.Label label22;
            internal System.Windows.Forms.GroupBox groupBox5;
            private System.Windows.Forms.NumericUpDown End_Time_Control;
            private System.Windows.Forms.NumericUpDown Start_Time_Control;
            internal System.Windows.Forms.Label label3;
            internal System.Windows.Forms.Label label20;
            private System.Windows.Forms.Label label24;
            internal System.Windows.Forms.ComboBox Parameter_Selection;
            private System.Windows.Forms.Label label23;
            internal System.Windows.Forms.ComboBox Color_Selection;
            private System.Windows.Forms.Label Param1_4;
            private System.Windows.Forms.Label Param1_2;
            private System.Windows.Forms.NumericUpDown Param_Min;
            private System.Windows.Forms.NumericUpDown Param_Max;
            private System.Windows.Forms.Label Param3_4;
            private System.Windows.Forms.PictureBox Param_Scale;
            internal System.Windows.Forms.Button Calculate_Map;
            internal System.Windows.Forms.Button Plot_Values;
            private System.Windows.Forms.CheckBox DirectionalToggle;
            private System.Windows.Forms.CheckBox Discretize;
            private System.Windows.Forms.CheckedListBox SourceList;
            private System.Windows.Forms.TabPage tabPage3;
            private System.Windows.Forms.RadioButton Disp_Other;
            private System.Windows.Forms.RadioButton Disp_Audience;
            private System.Windows.Forms.GroupBox groupBox7;
            private System.Windows.Forms.NumericUpDown Integration_select;
            private System.Windows.Forms.Label label25;
            private System.Windows.Forms.NumericUpDown Step_Select;
            private System.Windows.Forms.Label label16;
            private System.Windows.Forms.NumericUpDown T_End_select;
            private System.Windows.Forms.Label label15;
            private System.Windows.Forms.Label label6;
            private System.Windows.Forms.NumericUpDown T_Start_Select;
            private System.Windows.Forms.Button Flip_Toggle;
            private System.Windows.Forms.Button Start_Over;
            private System.Windows.Forms.Button Back_Step;
            private System.Windows.Forms.Button Forw_Step;
            private System.Windows.Forms.NumericUpDown Tick_Select;
            private System.Windows.Forms.Label label26;
            private System.Windows.Forms.Label label30;
            private System.Windows.Forms.Label Max_Time_out;
            private System.Windows.Forms.Label Min_Time_out;
            private System.Windows.Forms.Label label27;
            private System.Windows.Forms.RadioButton Incoherent;
            private System.Windows.Forms.RadioButton Coherent;
            private System.Windows.Forms.GroupBox groupBox8;
            private System.Windows.Forms.GroupBox groupBox9;
            private System.Windows.Forms.RadioButton ZeroAtSource;
            private System.Windows.Forms.RadioButton ZeroAtDirect;
            private System.Windows.Forms.GroupBox groupBox11;
            private System.Windows.Forms.RadioButton Rec_Centroid;
            private System.Windows.Forms.RadioButton Rec_Vertex;
            private System.Windows.Forms.GroupBox groupBox10;
            private System.Windows.Forms.CheckBox Offset_Mesh;
            private System.Windows.Forms.CheckBox Sum_Time;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            internal System.Windows.Forms.TextBox Folder_Status;
            internal System.Windows.Forms.Label label28;
            internal System.Windows.Forms.Button OpenFolder;
        }
    }
}