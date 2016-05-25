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
            this.CO_TIME = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.Receiver_Choice = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.LockUserScale = new System.Windows.Forms.CheckBox();
            this.Normalize_Graph = new System.Windows.Forms.CheckBox();
            this.Export_Signal = new System.Windows.Forms.Button();
            this.Frequency_View = new ZedGraph.ZedGraphControl();
            this.TransientView = new ZedGraph.ZedGraphControl();
            this.EigenFrequencies = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Selected_Extent = new System.Windows.Forms.ComboBox();
            this.CalculateSim = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Preview = new System.Windows.Forms.Button();
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
            this.Color_Selection = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.Pos_Select = new System.Windows.Forms.NumericUpDown();
            this.AxisSelect = new System.Windows.Forms.ComboBox();
            this.Map_Planes = new System.Windows.Forms.ListBox();
            this.AddPlane = new System.Windows.Forms.Button();
            this.Magnitude = new System.Windows.Forms.CheckBox();
            this.SetFolder = new System.Windows.Forms.Button();
            this.Folder_Status = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Frequency_Selection = new System.Windows.Forms.NumericUpDown();
            this.COTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Freq_Max = new System.Windows.Forms.NumericUpDown();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SourceSelect = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pos_Select)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Frequency_Selection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Max)).BeginInit();
            this.GroupBox2.SuspendLayout();
            this.SuspendLayout();
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
            this.comboBox1.Size = new System.Drawing.Size(213, 21);
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
            this.SourceSelection.Size = new System.Drawing.Size(213, 21);
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
            this.RoomSelection.Size = new System.Drawing.Size(213, 21);
            this.RoomSelection.TabIndex = 26;
            this.RoomSelection.Text = "Use Entire Model";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(415, 631);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CO_TIME);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label20);
            this.tabPage1.Controls.Add(this.Receiver_Choice);
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.Selected_Extent);
            this.tabPage1.Controls.Add(this.CalculateSim);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(407, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Simulation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CO_TIME
            // 
            this.CO_TIME.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CO_TIME.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.CO_TIME.Location = new System.Drawing.Point(98, 30);
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
            this.CO_TIME.Size = new System.Drawing.Size(67, 20);
            this.CO_TIME.TabIndex = 83;
            this.CO_TIME.UseWaitCursor = true;
            this.CO_TIME.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(3, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "Cutoff Time (ms):";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Enabled = false;
            this.label20.Location = new System.Drawing.Point(3, 82);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 13);
            this.label20.TabIndex = 47;
            this.label20.Text = "Receiver";
            // 
            // Receiver_Choice
            // 
            this.Receiver_Choice.FormattingEnabled = true;
            this.Receiver_Choice.Location = new System.Drawing.Point(59, 81);
            this.Receiver_Choice.Name = "Receiver_Choice";
            this.Receiver_Choice.Size = new System.Drawing.Size(201, 21);
            this.Receiver_Choice.TabIndex = 46;
            this.Receiver_Choice.Text = "No Results Calculated...";
            this.Receiver_Choice.SelectedIndexChanged += new System.EventHandler(this.Receiver_Choice_SelectedIndexChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.LockUserScale, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.Normalize_Graph, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.Export_Signal, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.Frequency_View, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.TransientView, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.EigenFrequencies, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 115);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(395, 430);
            this.tableLayoutPanel3.TabIndex = 44;
            // 
            // LockUserScale
            // 
            this.LockUserScale.AutoSize = true;
            this.LockUserScale.Location = new System.Drawing.Point(3, 3);
            this.LockUserScale.Name = "LockUserScale";
            this.LockUserScale.Size = new System.Drawing.Size(105, 17);
            this.LockUserScale.TabIndex = 44;
            this.LockUserScale.Text = "Lock User Scale";
            this.LockUserScale.UseVisualStyleBackColor = true;
            // 
            // Normalize_Graph
            // 
            this.Normalize_Graph.AutoSize = true;
            this.Normalize_Graph.Checked = true;
            this.Normalize_Graph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Normalize_Graph.Location = new System.Drawing.Point(134, 3);
            this.Normalize_Graph.Name = "Normalize_Graph";
            this.Normalize_Graph.Size = new System.Drawing.Size(119, 17);
            this.Normalize_Graph.TabIndex = 43;
            this.Normalize_Graph.Text = "Normalize To Direct";
            this.Normalize_Graph.UseVisualStyleBackColor = true;
            // 
            // Export_Signal
            // 
            this.Export_Signal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Export_Signal.Location = new System.Drawing.Point(264, 2);
            this.Export_Signal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Export_Signal.Name = "Export_Signal";
            this.Export_Signal.Size = new System.Drawing.Size(129, 21);
            this.Export_Signal.TabIndex = 45;
            this.Export_Signal.Text = "Export...";
            this.Export_Signal.UseVisualStyleBackColor = true;
            this.Export_Signal.Click += new System.EventHandler(this.Export_Signal_Click);
            // 
            // Frequency_View
            // 
            this.Frequency_View.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Frequency_View.AutoSize = true;
            this.Frequency_View.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.Frequency_View, 3);
            this.Frequency_View.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.Frequency_View.Location = new System.Drawing.Point(6, 194);
            this.Frequency_View.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Frequency_View.Name = "Frequency_View";
            this.Frequency_View.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.Frequency_View.ScrollGrace = 0D;
            this.Frequency_View.ScrollMaxX = 0D;
            this.Frequency_View.ScrollMaxY = 0D;
            this.Frequency_View.ScrollMaxY2 = 0D;
            this.Frequency_View.ScrollMinX = 0D;
            this.Frequency_View.ScrollMinY = 0D;
            this.Frequency_View.ScrollMinY2 = 0D;
            this.Frequency_View.Size = new System.Drawing.Size(383, 151);
            this.Frequency_View.TabIndex = 46;
            this.Frequency_View.UseExtendedPrintDialog = true;
            // 
            // TransientView
            // 
            this.TransientView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TransientView.AutoSize = true;
            this.TransientView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.TransientView, 3);
            this.TransientView.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.TransientView.Location = new System.Drawing.Point(6, 31);
            this.TransientView.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TransientView.Name = "TransientView";
            this.TransientView.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.TransientView.ScrollGrace = 0D;
            this.TransientView.ScrollMaxX = 0D;
            this.TransientView.ScrollMaxY = 0D;
            this.TransientView.ScrollMaxY2 = 0D;
            this.TransientView.ScrollMinX = 0D;
            this.TransientView.ScrollMinY = 0D;
            this.TransientView.ScrollMinY2 = 0D;
            this.TransientView.Size = new System.Drawing.Size(383, 151);
            this.TransientView.TabIndex = 42;
            this.TransientView.UseExtendedPrintDialog = true;
            // 
            // EigenFrequencies
            // 
            this.EigenFrequencies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.EigenFrequencies, 2);
            this.EigenFrequencies.FormattingEnabled = true;
            this.EigenFrequencies.Location = new System.Drawing.Point(133, 353);
            this.EigenFrequencies.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.EigenFrequencies.Name = "EigenFrequencies";
            this.EigenFrequencies.Size = new System.Drawing.Size(260, 69);
            this.EigenFrequencies.TabIndex = 47;
            this.EigenFrequencies.SelectedIndexChanged += new System.EventHandler(this.EigenFrequencies_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 351);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 79);
            this.label6.TabIndex = 48;
            this.label6.Text = "Eigen-Frequencies";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Calculate up to:";
            // 
            // Selected_Extent
            // 
            this.Selected_Extent.FormattingEnabled = true;
            this.Selected_Extent.Items.AddRange(new object[] {
            "63 Hz. Octave Band",
            "125 Hz. Octave Band",
            "250 Hz. Octave Band",
            "500 Hz. Octave Band",
            "1000 Hz. Octave Band",
            "2000 Hz. Octave Band",
            "4000 Hz. Octave Band",
            "8000 Hz. Octave Band"});
            this.Selected_Extent.Location = new System.Drawing.Point(98, 7);
            this.Selected_Extent.Name = "Selected_Extent";
            this.Selected_Extent.Size = new System.Drawing.Size(303, 21);
            this.Selected_Extent.TabIndex = 1;
            // 
            // CalculateSim
            // 
            this.CalculateSim.Location = new System.Drawing.Point(3, 53);
            this.CalculateSim.Name = "CalculateSim";
            this.CalculateSim.Size = new System.Drawing.Size(395, 23);
            this.CalculateSim.TabIndex = 0;
            this.CalculateSim.Text = "Calculate";
            this.CalculateSim.UseVisualStyleBackColor = true;
            this.CalculateSim.Click += new System.EventHandler(this.CalculateSim_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.GroupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(407, 605);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Visualization";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.EdgeFreq);
            this.groupBox3.Controls.Add(this.Label21);
            this.groupBox3.Controls.Add(this.Atten_Method);
            this.groupBox3.Controls.Add(this.Label19);
            this.groupBox3.Controls.Add(this.Air_Pressure);
            this.groupBox3.Controls.Add(this.Label3);
            this.groupBox3.Controls.Add(this.Rel_Humidity);
            this.groupBox3.Controls.Add(this.AirTemp);
            this.groupBox3.Controls.Add(this.Air_Temp);
            this.groupBox3.Location = new System.Drawing.Point(6, 458);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(395, 141);
            this.groupBox3.TabIndex = 101;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Environmental Factors";
            // 
            // EdgeFreq
            // 
            this.EdgeFreq.AutoSize = true;
            this.EdgeFreq.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EdgeFreq.Location = new System.Drawing.Point(70, 123);
            this.EdgeFreq.Name = "EdgeFreq";
            this.EdgeFreq.Size = new System.Drawing.Size(155, 17);
            this.EdgeFreq.TabIndex = 35;
            this.EdgeFreq.Text = "Edge Frequency Correction";
            this.EdgeFreq.UseVisualStyleBackColor = true;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(8, 99);
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
            this.Atten_Method.Location = new System.Drawing.Point(70, 96);
            this.Atten_Method.Name = "Atten_Method";
            this.Atten_Method.Size = new System.Drawing.Size(316, 21);
            this.Atten_Method.TabIndex = 14;
            this.Atten_Method.Text = "ISO 9613-1 (Outdoor Attenuation)";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(8, 72);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(121, 13);
            this.Label19.TabIndex = 32;
            this.Label19.Text = "Static Air Pressure (hPa)";
            // 
            // Air_Pressure
            // 
            this.Air_Pressure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Pressure.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Pressure.Location = new System.Drawing.Point(322, 70);
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
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(8, 46);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(103, 13);
            this.Label3.TabIndex = 30;
            this.Label3.Text = "Relative Humidity(%)";
            // 
            // Rel_Humidity
            // 
            this.Rel_Humidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Rel_Humidity.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Rel_Humidity.Location = new System.Drawing.Point(354, 44);
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
            this.Rel_Humidity.Size = new System.Drawing.Size(32, 20);
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
            this.AirTemp.Location = new System.Drawing.Point(8, 20);
            this.AirTemp.Name = "AirTemp";
            this.AirTemp.Size = new System.Drawing.Size(98, 13);
            this.AirTemp.TabIndex = 28;
            this.AirTemp.Text = "Air Temperature (C)";
            // 
            // Air_Temp
            // 
            this.Air_Temp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Temp.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Temp.Location = new System.Drawing.Point(354, 18);
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
            this.Air_Temp.Size = new System.Drawing.Size(32, 20);
            this.Air_Temp.TabIndex = 11;
            this.Air_Temp.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
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
            this.tableLayoutPanel1.Controls.Add(this.Preview, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Param3_4, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.Param1_2, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.Param_Max, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.Param_Scale, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Param1_4, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.Param_Min, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.Loop, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Color_Selection, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label23, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.Pos_Select, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.AxisSelect, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.Map_Planes, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.AddPlane, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.Magnitude, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.SetFolder, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.Folder_Status, 2, 9);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 159);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(394, 293);
            this.tableLayoutPanel1.TabIndex = 100;
            // 
            // Preview
            // 
            this.Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Preview, 4);
            this.Preview.Location = new System.Drawing.Point(2, 32);
            this.Preview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(390, 26);
            this.Preview.TabIndex = 87;
            this.Preview.Text = "Calculate & Run";
            this.Preview.UseVisualStyleBackColor = true;
            this.Preview.Click += new System.EventHandler(this.Calculate_Click);
            // 
            // Param3_4
            // 
            this.Param3_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param3_4.AutoSize = true;
            this.Param3_4.Location = new System.Drawing.Point(272, 100);
            this.Param3_4.Name = "Param3_4";
            this.Param3_4.Size = new System.Drawing.Size(19, 37);
            this.Param3_4.TabIndex = 95;
            this.Param3_4.Text = "75";
            this.Param3_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param1_2
            // 
            this.Param1_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param1_2.AutoSize = true;
            this.Param1_2.Location = new System.Drawing.Point(272, 137);
            this.Param1_2.Name = "Param1_2";
            this.Param1_2.Size = new System.Drawing.Size(19, 37);
            this.Param1_2.TabIndex = 98;
            this.Param1_2.Text = "50";
            this.Param1_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param_Max
            // 
            this.Param_Max.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Max.DecimalPlaces = 1;
            this.Param_Max.Location = new System.Drawing.Point(240, 73);
            this.Param_Max.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.Param_Max.Name = "Param_Max";
            this.Param_Max.Size = new System.Drawing.Size(51, 20);
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
            this.Param_Scale.Location = new System.Drawing.Point(297, 73);
            this.Param_Scale.Name = "Param_Scale";
            this.tableLayoutPanel1.SetRowSpan(this.Param_Scale, 5);
            this.Param_Scale.Size = new System.Drawing.Size(94, 165);
            this.Param_Scale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Param_Scale.TabIndex = 94;
            this.Param_Scale.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox4, 2);
            this.groupBox4.Controls.Add(this.Time_Preview);
            this.groupBox4.Location = new System.Drawing.Point(3, 63);
            this.groupBox4.Name = "groupBox4";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox4, 2);
            this.groupBox4.Size = new System.Drawing.Size(190, 34);
            this.groupBox4.TabIndex = 104;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current Time (ms)";
            // 
            // Time_Preview
            // 
            this.Time_Preview.AutoSize = true;
            this.Time_Preview.Location = new System.Drawing.Point(25, 16);
            this.Time_Preview.Name = "Time_Preview";
            this.Time_Preview.Size = new System.Drawing.Size(74, 13);
            this.Time_Preview.TabIndex = 106;
            this.Time_Preview.Text = "Time_Preview";
            // 
            // Param1_4
            // 
            this.Param1_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Param1_4.AutoSize = true;
            this.Param1_4.Location = new System.Drawing.Point(272, 174);
            this.Param1_4.Name = "Param1_4";
            this.Param1_4.Size = new System.Drawing.Size(19, 37);
            this.Param1_4.TabIndex = 99;
            this.Param1_4.Text = "25";
            this.Param1_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Param_Min
            // 
            this.Param_Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Param_Min.DecimalPlaces = 1;
            this.Param_Min.Location = new System.Drawing.Point(240, 218);
            this.Param_Min.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.Param_Min.Name = "Param_Min";
            this.Param_Min.Size = new System.Drawing.Size(51, 20);
            this.Param_Min.TabIndex = 97;
            this.Param_Min.ValueChanged += new System.EventHandler(this.Param_Max_ValueChanged);
            this.Param_Min.Click += new System.EventHandler(this.Param_Max_ValueChanged);
            // 
            // Loop
            // 
            this.Loop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Loop, 2);
            this.Loop.Location = new System.Drawing.Point(3, 3);
            this.Loop.Name = "Loop";
            this.Loop.Size = new System.Drawing.Size(190, 24);
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
            this.button2.Location = new System.Drawing.Point(199, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(192, 24);
            this.button2.TabIndex = 34;
            this.button2.Text = ">>";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Forw_Click);
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
            this.Color_Selection.Location = new System.Drawing.Point(101, 103);
            this.Color_Selection.Name = "Color_Selection";
            this.Color_Selection.Size = new System.Drawing.Size(92, 21);
            this.Color_Selection.TabIndex = 100;
            this.Color_Selection.Text = "R-O-Y-G-B-I-V";
            this.Color_Selection.SelectedIndexChanged += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
            this.Color_Selection.Click += new System.EventHandler(this.Color_Selection_SelectedIndexChanged);
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(3, 100);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(78, 37);
            this.label23.TabIndex = 107;
            this.label23.Text = "Color Selection";
            // 
            // Pos_Select
            // 
            this.Pos_Select.Location = new System.Drawing.Point(101, 214);
            this.Pos_Select.Name = "Pos_Select";
            this.Pos_Select.Size = new System.Drawing.Size(92, 20);
            this.Pos_Select.TabIndex = 109;
            this.Pos_Select.ValueChanged += new System.EventHandler(this.Pos_Select_ValueChanged);
            this.Pos_Select.Click += new System.EventHandler(this.Pos_Select_ValueChanged);
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
            this.AxisSelect.Location = new System.Drawing.Point(3, 214);
            this.AxisSelect.Name = "AxisSelect";
            this.AxisSelect.Size = new System.Drawing.Size(92, 21);
            this.AxisSelect.TabIndex = 117;
            this.AxisSelect.Text = "Z";
            this.AxisSelect.SelectedIndexChanged += new System.EventHandler(this.AxisSelect_SelectedIndexChanged);
            this.AxisSelect.Click += new System.EventHandler(this.AxisSelect_SelectedIndexChanged);
            // 
            // Map_Planes
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.Map_Planes, 2);
            this.Map_Planes.FormattingEnabled = true;
            this.Map_Planes.Location = new System.Drawing.Point(3, 140);
            this.Map_Planes.Name = "Map_Planes";
            this.tableLayoutPanel1.SetRowSpan(this.Map_Planes, 2);
            this.Map_Planes.Size = new System.Drawing.Size(190, 56);
            this.Map_Planes.TabIndex = 118;
            // 
            // AddPlane
            // 
            this.AddPlane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.AddPlane, 2);
            this.AddPlane.Location = new System.Drawing.Point(3, 244);
            this.AddPlane.Name = "AddPlane";
            this.AddPlane.Size = new System.Drawing.Size(190, 19);
            this.AddPlane.TabIndex = 116;
            this.AddPlane.Text = "Add Plane";
            this.AddPlane.UseVisualStyleBackColor = true;
            this.AddPlane.Click += new System.EventHandler(this.AddPlane_Click);
            // 
            // Magnitude
            // 
            this.Magnitude.AutoSize = true;
            this.Magnitude.Location = new System.Drawing.Point(199, 244);
            this.Magnitude.Name = "Magnitude";
            this.Magnitude.Size = new System.Drawing.Size(76, 17);
            this.Magnitude.TabIndex = 119;
            this.Magnitude.Text = "Magnitude";
            this.Magnitude.UseVisualStyleBackColor = true;
            // 
            // SetFolder
            // 
            this.SetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.SetFolder, 2);
            this.SetFolder.Location = new System.Drawing.Point(2, 268);
            this.SetFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SetFolder.Name = "SetFolder";
            this.SetFolder.Size = new System.Drawing.Size(192, 23);
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
            this.Folder_Status.Location = new System.Drawing.Point(199, 269);
            this.Folder_Status.Name = "Folder_Status";
            this.Folder_Status.ReadOnly = true;
            this.Folder_Status.Size = new System.Drawing.Size(192, 20);
            this.Folder_Status.TabIndex = 120;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Frequency_Selection);
            this.groupBox1.Controls.Add(this.COTime);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.Freq_Max);
            this.groupBox1.Location = new System.Drawing.Point(6, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 96);
            this.groupBox1.TabIndex = 99;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulation Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 85;
            this.label1.Text = "Frequency Max";
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
            this.Frequency_Selection.Location = new System.Drawing.Point(322, 16);
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
            this.Frequency_Selection.Size = new System.Drawing.Size(67, 20);
            this.Frequency_Selection.TabIndex = 84;
            this.Frequency_Selection.UseWaitCursor = true;
            this.Frequency_Selection.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // COTime
            // 
            this.COTime.AutoSize = true;
            this.COTime.Location = new System.Drawing.Point(6, 70);
            this.COTime.Name = "COTime";
            this.COTime.Size = new System.Drawing.Size(88, 13);
            this.COTime.TabIndex = 83;
            this.COTime.Text = "Cut Off Time (ms)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 13);
            this.label10.TabIndex = 79;
            this.label10.Text = "Frequency Selection";
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
            this.Freq_Max.Location = new System.Drawing.Point(322, 42);
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
            this.Freq_Max.Size = new System.Drawing.Size(67, 20);
            this.Freq_Max.TabIndex = 78;
            this.Freq_Max.UseWaitCursor = true;
            this.Freq_Max.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.label8);
            this.GroupBox2.Controls.Add(this.SourceSelect);
            this.GroupBox2.Location = new System.Drawing.Point(6, 6);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(395, 43);
            this.GroupBox2.TabIndex = 98;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Geometry";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Source Signal:";
            // 
            // SourceSelect
            // 
            this.SourceSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceSelect.AutoCompleteCustomSource.AddRange(new string[] {
            "Sine Wave",
            "Dirac Pulse"});
            this.SourceSelect.FormattingEnabled = true;
            this.SourceSelect.Items.AddRange(new object[] {
            "Sine Wave",
            "Dirac Pulse",
            "Sine Pulse",
            "Pseudo Random Noise"});
            this.SourceSelect.Location = new System.Drawing.Point(79, 13);
            this.SourceSelect.Name = "SourceSelect";
            this.SourceSelect.Size = new System.Drawing.Size(310, 21);
            this.SourceSelect.TabIndex = 27;
            this.SourceSelect.Text = "Sine Wave";
            // 
            // Pach_TD_Numeric_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "Pach_TD_Numeric_Control";
            this.Size = new System.Drawing.Size(421, 637);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Scale)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pos_Select)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Frequency_Selection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Freq_Max)).EndInit();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
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
            private System.Windows.Forms.GroupBox groupBox1;
            internal System.Windows.Forms.Label label1;
            internal System.Windows.Forms.NumericUpDown Frequency_Selection;
            internal System.Windows.Forms.Label COTime;
            internal System.Windows.Forms.Label label10;
            internal System.Windows.Forms.NumericUpDown Freq_Max;
            internal System.Windows.Forms.GroupBox GroupBox2;
            internal System.Windows.Forms.Label label8;
            internal System.Windows.Forms.ComboBox SourceSelect;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.ComboBox Selected_Extent;
            private System.Windows.Forms.Button CalculateSim;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
            private System.Windows.Forms.CheckBox LockUserScale;
            private System.Windows.Forms.CheckBox Normalize_Graph;
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
        }
    }
}