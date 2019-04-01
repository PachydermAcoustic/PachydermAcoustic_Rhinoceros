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
        partial class Pach_Auralisation : System.Windows.Forms.UserControl
        {

            //UserControl overrides dispose to clean up the component list. 
            [System.Diagnostics.DebuggerNonUserCode()]
            protected override void Dispose(bool disposing)
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
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
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromMeshSphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DirectionalSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectSourceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectASphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromSphereObjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Render_Settings = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.Azi_Choice = new System.Windows.Forms.NumericUpDown();
            this.Alt_Choice = new System.Windows.Forms.NumericUpDown();
            this.Source_Aim = new System.Windows.Forms.ComboBox();
            this.SourceList = new System.Windows.Forms.CheckedListBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.Normalization_Choice = new System.Windows.Forms.NumericUpDown();
            this.Receiver_Choice = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Graph_Octave = new System.Windows.Forms.ComboBox();
            this.RenderBtn = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.Analysis_View = new ZedGraph.ZedGraphControl();
            this.label16 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.DryChannel = new System.Windows.Forms.NumericUpDown();
            this.Signal_Status = new System.Windows.Forms.TextBox();
            this.OpenSignal = new System.Windows.Forms.Button();
            this.Export_Filter = new System.Windows.Forms.Button();
            this.Sample_Freq_Selection = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.Data_Source = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.Save_Channels = new System.Windows.Forms.Button();
            this.Remove_Channel = new System.Windows.Forms.Button();
            this.Move_Down = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Data_From = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.Supplement_Numerical = new System.Windows.Forms.CheckBox();
            this.Hybrid_Select = new System.Windows.Forms.RadioButton();
            this.Mapping_Select = new System.Windows.Forms.RadioButton();
            this.DistributionType = new System.Windows.Forms.ComboBox();
            this.Move_Up = new System.Windows.Forms.Button();
            this.Channel_View = new System.Windows.Forms.ListBox();
            this.Add_Channel = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Render_Settings.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Normalization_Choice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DryChannel)).BeginInit();
            this.Tabs.SuspendLayout();
            this.Data_Source.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.Data_From.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FromMeshSphereToolStripMenuItem,
            this.FromPointInputToolStripMenuItem});
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(257, 26);
            this.ToolStripMenuItem1.Text = "Omni-Directional Source...";
            // 
            // FromMeshSphereToolStripMenuItem
            // 
            this.FromMeshSphereToolStripMenuItem.Name = "FromMeshSphereToolStripMenuItem";
            this.FromMeshSphereToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.FromMeshSphereToolStripMenuItem.Text = "From MeshSphere";
            // 
            // FromPointInputToolStripMenuItem
            // 
            this.FromPointInputToolStripMenuItem.Name = "FromPointInputToolStripMenuItem";
            this.FromPointInputToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.FromPointInputToolStripMenuItem.Text = "From Point Input";
            // 
            // DirectionalSourceToolStripMenuItem
            // 
            this.DirectionalSourceToolStripMenuItem.Name = "DirectionalSourceToolStripMenuItem";
            this.DirectionalSourceToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.DirectionalSourceToolStripMenuItem.Text = "Directional Source...";
            // 
            // SelectSourceToolStripMenuItem1
            // 
            this.SelectSourceToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.DirectionalSourceToolStripMenuItem});
            this.SelectSourceToolStripMenuItem1.Name = "SelectSourceToolStripMenuItem1";
            this.SelectSourceToolStripMenuItem1.Size = new System.Drawing.Size(84, 20);
            this.SelectSourceToolStripMenuItem1.Text = "Select Source";
            // 
            // SelectASphereToolStripMenuItem
            // 
            this.SelectASphereToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FromSphereObjectToolStripMenuItem1,
            this.FromPointInputToolStripMenuItem2});
            this.SelectASphereToolStripMenuItem.Name = "SelectASphereToolStripMenuItem";
            this.SelectASphereToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.SelectASphereToolStripMenuItem.Text = "Select...";
            // 
            // FromSphereObjectToolStripMenuItem1
            // 
            this.FromSphereObjectToolStripMenuItem1.Name = "FromSphereObjectToolStripMenuItem1";
            this.FromSphereObjectToolStripMenuItem1.Size = new System.Drawing.Size(216, 26);
            this.FromSphereObjectToolStripMenuItem1.Text = "From Sphere Object";
            // 
            // FromPointInputToolStripMenuItem2
            // 
            this.FromPointInputToolStripMenuItem2.Name = "FromPointInputToolStripMenuItem2";
            this.FromPointInputToolStripMenuItem2.Size = new System.Drawing.Size(216, 26);
            this.FromPointInputToolStripMenuItem2.Text = "From Point Input";
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectASphereToolStripMenuItem});
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(93, 20);
            this.ToolStripMenuItem2.Text = "Select Receiver";
            // 
            // Render_Settings
            // 
            this.Render_Settings.AutoScroll = true;
            this.Render_Settings.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.Render_Settings.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.Render_Settings.Controls.Add(this.tableLayoutPanel3);
            this.Render_Settings.Location = new System.Drawing.Point(4, 25);
            this.Render_Settings.Margin = new System.Windows.Forms.Padding(4);
            this.Render_Settings.Name = "Render_Settings";
            this.Render_Settings.Padding = new System.Windows.Forms.Padding(4);
            this.Render_Settings.Size = new System.Drawing.Size(525, 660);
            this.Render_Settings.TabIndex = 2;
            this.Render_Settings.Text = "Render Settings";
            this.Render_Settings.UseVisualStyleBackColor = true;
            this.Render_Settings.UseWaitCursor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.1845F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.8155F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel3.Controls.Add(this.Azi_Choice, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.Alt_Choice, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.Source_Aim, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.SourceList, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label26, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.label27, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.Normalization_Choice, 3, 4);
            this.tableLayoutPanel3.Controls.Add(this.Receiver_Choice, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.label6, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.Graph_Octave, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.RenderBtn, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.label25, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.Analysis_View, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.label16, 2, 6);
            this.tableLayoutPanel3.Controls.Add(this.label20, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.DryChannel, 3, 6);
            this.tableLayoutPanel3.Controls.Add(this.Signal_Status, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.OpenSignal, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.Export_Filter, 2, 9);
            this.tableLayoutPanel3.Controls.Add(this.Sample_Freq_Selection, 1, 9);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 9);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 10;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(517, 652);
            this.tableLayoutPanel3.TabIndex = 43;
            this.tableLayoutPanel3.UseWaitCursor = true;
            // 
            // Azi_Choice
            // 
            this.Azi_Choice.DecimalPlaces = 2;
            this.Azi_Choice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Azi_Choice.Location = new System.Drawing.Point(402, 100);
            this.Azi_Choice.Margin = new System.Windows.Forms.Padding(4);
            this.Azi_Choice.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.Azi_Choice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.Azi_Choice.Name = "Azi_Choice";
            this.Azi_Choice.Size = new System.Drawing.Size(111, 22);
            this.Azi_Choice.TabIndex = 47;
            this.Azi_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Azi_Choice.UseWaitCursor = true;
            this.Azi_Choice.ValueChanged += new System.EventHandler(this.Azi_Choice_ValueChanged);
            // 
            // Alt_Choice
            // 
            this.Alt_Choice.DecimalPlaces = 2;
            this.Alt_Choice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Alt_Choice.Location = new System.Drawing.Point(402, 68);
            this.Alt_Choice.Margin = new System.Windows.Forms.Padding(4);
            this.Alt_Choice.Maximum = new decimal(new int[] {
            91,
            0,
            0,
            0});
            this.Alt_Choice.Minimum = new decimal(new int[] {
            91,
            0,
            0,
            -2147483648});
            this.Alt_Choice.Name = "Alt_Choice";
            this.Alt_Choice.Size = new System.Drawing.Size(111, 22);
            this.Alt_Choice.TabIndex = 48;
            this.Alt_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Alt_Choice.UseWaitCursor = true;
            this.Alt_Choice.ValueChanged += new System.EventHandler(this.Alt_Choice_ValueChanged);
            // 
            // Source_Aim
            // 
            this.Source_Aim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Source_Aim.FormattingEnabled = true;
            this.Source_Aim.Location = new System.Drawing.Point(402, 36);
            this.Source_Aim.Margin = new System.Windows.Forms.Padding(4);
            this.Source_Aim.Name = "Source_Aim";
            this.Source_Aim.Size = new System.Drawing.Size(111, 24);
            this.Source_Aim.TabIndex = 51;
            this.Source_Aim.Text = "None";
            this.Source_Aim.UseWaitCursor = true;
            this.Source_Aim.SelectedIndexChanged += new System.EventHandler(this.Source_Aim_SelectedIndexChanged);
            // 
            // SourceList
            // 
            this.SourceList.CheckOnClick = true;
            this.tableLayoutPanel3.SetColumnSpan(this.SourceList, 2);
            this.SourceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceList.FormattingEnabled = true;
            this.SourceList.Location = new System.Drawing.Point(4, 4);
            this.SourceList.Margin = new System.Windows.Forms.Padding(4);
            this.SourceList.MinimumSize = new System.Drawing.Size(4, 78);
            this.SourceList.Name = "SourceList";
            this.tableLayoutPanel3.SetRowSpan(this.SourceList, 4);
            this.SourceList.ScrollAlwaysVisible = true;
            this.SourceList.Size = new System.Drawing.Size(233, 120);
            this.SourceList.TabIndex = 52;
            this.SourceList.UseWaitCursor = true;
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Enabled = false;
            this.label26.Location = new System.Drawing.Point(245, 96);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(149, 17);
            this.label26.TabIndex = 49;
            this.label26.Text = "Azimuth";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label26.UseWaitCursor = true;
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Enabled = false;
            this.label27.Location = new System.Drawing.Point(245, 64);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(149, 17);
            this.label27.TabIndex = 50;
            this.label27.Text = "Altitude";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label27.UseWaitCursor = true;
            // 
            // Normalization_Choice
            // 
            this.Normalization_Choice.DecimalPlaces = 2;
            this.Normalization_Choice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Normalization_Choice.Location = new System.Drawing.Point(402, 132);
            this.Normalization_Choice.Margin = new System.Windows.Forms.Padding(4);
            this.Normalization_Choice.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.Normalization_Choice.Name = "Normalization_Choice";
            this.Normalization_Choice.Size = new System.Drawing.Size(111, 22);
            this.Normalization_Choice.TabIndex = 53;
            this.Normalization_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Normalization_Choice.UseWaitCursor = true;
            // 
            // Receiver_Choice
            // 
            this.Receiver_Choice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Receiver_Choice.FormattingEnabled = true;
            this.Receiver_Choice.Items.AddRange(new object[] {
            "0"});
            this.Receiver_Choice.Location = new System.Drawing.Point(402, 4);
            this.Receiver_Choice.Margin = new System.Windows.Forms.Padding(4);
            this.Receiver_Choice.Name = "Receiver_Choice";
            this.Receiver_Choice.Size = new System.Drawing.Size(111, 24);
            this.Receiver_Choice.TabIndex = 39;
            this.Receiver_Choice.Text = "0";
            this.Receiver_Choice.UseWaitCursor = true;
            this.Receiver_Choice.SelectedIndexChanged += new System.EventHandler(this.Receiver_Choice_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(245, 128);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 31);
            this.label6.TabIndex = 53;
            this.label6.Text = "Max Output (dB):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label6.UseWaitCursor = true;
            // 
            // Graph_Octave
            // 
            this.Graph_Octave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Graph_Octave, 2);
            this.Graph_Octave.FormattingEnabled = true;
            this.Graph_Octave.Items.AddRange(new object[] {
            "Summation: All Octaves",
            "62.5 Hz.",
            "125 Hz.",
            "250 Hz.",
            "500 Hz.",
            "1 kHz.",
            "2 kHz.",
            "4 kHz.",
            "8 kHz."});
            this.Graph_Octave.Location = new System.Drawing.Point(4, 132);
            this.Graph_Octave.Margin = new System.Windows.Forms.Padding(4);
            this.Graph_Octave.Name = "Graph_Octave";
            this.Graph_Octave.Size = new System.Drawing.Size(233, 24);
            this.Graph_Octave.TabIndex = 53;
            this.Graph_Octave.Text = "Summation: All Octaves";
            this.Graph_Octave.UseWaitCursor = true;
            this.Graph_Octave.SelectedIndexChanged += new System.EventHandler(this.Graph_Octave_SelectedIndexChanged);
            // 
            // RenderBtn
            // 
            this.RenderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.RenderBtn, 4);
            this.RenderBtn.Enabled = false;
            this.RenderBtn.Location = new System.Drawing.Point(4, 592);
            this.RenderBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RenderBtn.Name = "RenderBtn";
            this.RenderBtn.Size = new System.Drawing.Size(509, 24);
            this.RenderBtn.TabIndex = 49;
            this.RenderBtn.Text = "Render Auralization";
            this.RenderBtn.UseVisualStyleBackColor = true;
            this.RenderBtn.UseWaitCursor = true;
            this.RenderBtn.Click += new System.EventHandler(this.RenderBtn_Click);
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Enabled = false;
            this.label25.Location = new System.Drawing.Point(245, 32);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(149, 17);
            this.label25.TabIndex = 46;
            this.label25.Text = "Aim at Source";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label25.UseWaitCursor = true;
            // 
            // Analysis_View
            // 
            this.Analysis_View.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Analysis_View.AutoSize = true;
            this.Analysis_View.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.Analysis_View, 4);
            this.Analysis_View.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.Analysis_View.Location = new System.Drawing.Point(8, 166);
            this.Analysis_View.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Analysis_View.Name = "Analysis_View";
            this.Analysis_View.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.Analysis_View.ScrollGrace = 0D;
            this.Analysis_View.ScrollMaxX = 0D;
            this.Analysis_View.ScrollMaxY = 120D;
            this.Analysis_View.ScrollMaxY2 = 0D;
            this.Analysis_View.ScrollMinX = 0D;
            this.Analysis_View.ScrollMinY = 0D;
            this.Analysis_View.ScrollMinY2 = 0D;
            this.Analysis_View.Size = new System.Drawing.Size(501, 344);
            this.Analysis_View.TabIndex = 42;
            this.Analysis_View.UseExtendedPrintDialog = true;
            this.Analysis_View.UseWaitCursor = true;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(245, 517);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(149, 39);
            this.label16.TabIndex = 46;
            this.label16.Text = "Selected Channel";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label16.UseWaitCursor = true;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Enabled = false;
            this.label20.Location = new System.Drawing.Point(245, 0);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(149, 17);
            this.label20.TabIndex = 40;
            this.label20.Text = "Receiver";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label20.UseWaitCursor = true;
            // 
            // DryChannel
            // 
            this.DryChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DryChannel.AutoSize = true;
            this.DryChannel.Location = new System.Drawing.Point(402, 521);
            this.DryChannel.Margin = new System.Windows.Forms.Padding(4);
            this.DryChannel.Name = "DryChannel";
            this.DryChannel.Size = new System.Drawing.Size(111, 22);
            this.DryChannel.TabIndex = 45;
            this.DryChannel.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.DryChannel.UseWaitCursor = true;
            // 
            // Signal_Status
            // 
            this.Signal_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Signal_Status, 4);
            this.Signal_Status.Location = new System.Drawing.Point(4, 560);
            this.Signal_Status.Margin = new System.Windows.Forms.Padding(4);
            this.Signal_Status.Name = "Signal_Status";
            this.Signal_Status.ReadOnly = true;
            this.Signal_Status.Size = new System.Drawing.Size(509, 22);
            this.Signal_Status.TabIndex = 48;
            this.Signal_Status.UseWaitCursor = true;
            // 
            // OpenSignal
            // 
            this.OpenSignal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.OpenSignal, 2);
            this.OpenSignal.Location = new System.Drawing.Point(4, 521);
            this.OpenSignal.Margin = new System.Windows.Forms.Padding(4);
            this.OpenSignal.Name = "OpenSignal";
            this.OpenSignal.Size = new System.Drawing.Size(233, 31);
            this.OpenSignal.TabIndex = 47;
            this.OpenSignal.Text = "Open...";
            this.OpenSignal.UseVisualStyleBackColor = true;
            this.OpenSignal.UseWaitCursor = true;
            this.OpenSignal.Click += new System.EventHandler(this.OpenSignal_Click);
            // 
            // Export_Filter
            // 
            this.Export_Filter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Export_Filter, 2);
            this.Export_Filter.Location = new System.Drawing.Point(245, 624);
            this.Export_Filter.Margin = new System.Windows.Forms.Padding(4);
            this.Export_Filter.Name = "Export_Filter";
            this.Export_Filter.Size = new System.Drawing.Size(268, 24);
            this.Export_Filter.TabIndex = 54;
            this.Export_Filter.Text = "Export IR as Wave File";
            this.Export_Filter.UseVisualStyleBackColor = true;
            this.Export_Filter.UseWaitCursor = true;
            this.Export_Filter.Click += new System.EventHandler(this.ExportFilter);
            // 
            // Sample_Freq_Selection
            // 
            this.Sample_Freq_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Sample_Freq_Selection.DisplayMember = "1";
            this.Sample_Freq_Selection.FormattingEnabled = true;
            this.Sample_Freq_Selection.Items.AddRange(new object[] {
            "22050 Hz.",
            "44100 Hz.",
            "48000 Hz.",
            "96000 Hz.",
            "192000 Hz.",
            "384000 Hz."});
            this.Sample_Freq_Selection.Location = new System.Drawing.Point(85, 622);
            this.Sample_Freq_Selection.Margin = new System.Windows.Forms.Padding(2);
            this.Sample_Freq_Selection.Name = "Sample_Freq_Selection";
            this.Sample_Freq_Selection.Size = new System.Drawing.Size(154, 24);
            this.Sample_Freq_Selection.TabIndex = 55;
            this.Sample_Freq_Selection.UseWaitCursor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 620);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 32);
            this.label3.TabIndex = 56;
            this.label3.Text = "Sample Frequency:";
            this.label3.UseWaitCursor = true;
            // 
            // Tabs
            // 
            this.Tabs.AccessibleDescription = "";
            this.Tabs.AccessibleName = "";
            this.Tabs.Controls.Add(this.Data_Source);
            this.Tabs.Controls.Add(this.Render_Settings);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Margin = new System.Windows.Forms.Padding(4);
            this.Tabs.MinimumSize = new System.Drawing.Size(533, 492);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(533, 689);
            this.Tabs.TabIndex = 5;
            this.Tabs.SelectedIndexChanged += new System.EventHandler(this.Tab_Selecting);
            // 
            // Data_Source
            // 
            this.Data_Source.Controls.Add(this.tableLayoutPanel1);
            this.Data_Source.Location = new System.Drawing.Point(4, 25);
            this.Data_Source.Margin = new System.Windows.Forms.Padding(4);
            this.Data_Source.Name = "Data_Source";
            this.Data_Source.Padding = new System.Windows.Forms.Padding(4);
            this.Data_Source.Size = new System.Drawing.Size(525, 660);
            this.Data_Source.TabIndex = 3;
            this.Data_Source.Text = "Data Source";
            this.Data_Source.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Save_Channels, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.Remove_Channel, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.Move_Down, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.DistributionType, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Move_Up, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.Channel_View, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Add_Channel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.Data_From, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 652);
            this.tableLayoutPanel1.TabIndex = 62;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 100);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 17);
            this.label1.TabIndex = 61;
            this.label1.Text = "Type of Auralisation:";
            // 
            // Save_Channels
            // 
            this.Save_Channels.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tableLayoutPanel1.SetColumnSpan(this.Save_Channels, 2);
            this.Save_Channels.Enabled = false;
            this.Save_Channels.Location = new System.Drawing.Point(5, 624);
            this.Save_Channels.Margin = new System.Windows.Forms.Padding(4);
            this.Save_Channels.Name = "Save_Channels";
            this.Save_Channels.Size = new System.Drawing.Size(507, 24);
            this.Save_Channels.TabIndex = 58;
            this.Save_Channels.Text = "Save Configuration";
            this.Save_Channels.UseVisualStyleBackColor = true;
            this.Save_Channels.Visible = false;
            this.Save_Channels.Click += new System.EventHandler(this.Save_Channels_Click);
            // 
            // Remove_Channel
            // 
            this.Remove_Channel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Remove_Channel.Enabled = false;
            this.Remove_Channel.Location = new System.Drawing.Point(262, 592);
            this.Remove_Channel.Margin = new System.Windows.Forms.Padding(4);
            this.Remove_Channel.Name = "Remove_Channel";
            this.Remove_Channel.Size = new System.Drawing.Size(251, 24);
            this.Remove_Channel.TabIndex = 59;
            this.Remove_Channel.Text = "Remove";
            this.Remove_Channel.UseVisualStyleBackColor = true;
            this.Remove_Channel.Visible = false;
            this.Remove_Channel.Click += new System.EventHandler(this.Remove_Channel_Click);
            // 
            // Move_Down
            // 
            this.Move_Down.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Move_Down.Enabled = false;
            this.Move_Down.Location = new System.Drawing.Point(262, 560);
            this.Move_Down.Margin = new System.Windows.Forms.Padding(4);
            this.Move_Down.Name = "Move_Down";
            this.Move_Down.Size = new System.Drawing.Size(251, 24);
            this.Move_Down.TabIndex = 59;
            this.Move_Down.Text = "Move Down";
            this.Move_Down.UseVisualStyleBackColor = true;
            this.Move_Down.Visible = false;
            this.Move_Down.Click += new System.EventHandler(this.Move_Down_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 132);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 17);
            this.label2.TabIndex = 61;
            this.label2.Text = "Channels:";
            // 
            // Data_From
            // 
            this.Data_From.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Data_From, 2);
            this.Data_From.Controls.Add(this.label5);
            this.Data_From.Controls.Add(this.label4);
            this.Data_From.Controls.Add(this.numericUpDown1);
            this.Data_From.Controls.Add(this.Supplement_Numerical);
            this.Data_From.Controls.Add(this.Hybrid_Select);
            this.Data_From.Controls.Add(this.Mapping_Select);
            this.Data_From.Location = new System.Drawing.Point(4, 4);
            this.Data_From.Margin = new System.Windows.Forms.Padding(4);
            this.Data_From.Name = "Data_From";
            this.Data_From.Padding = new System.Windows.Forms.Padding(4);
            this.Data_From.Size = new System.Drawing.Size(509, 92);
            this.Data_From.TabIndex = 60;
            this.Data_From.TabStop = false;
            this.Data_From.Text = "Data From:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(355, 54);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 17);
            this.label5.TabIndex = 65;
            this.label5.Text = "Hz. and below.";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(207, 54);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 17);
            this.label4.TabIndex = 65;
            this.label4.Text = "For :";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Enabled = false;
            this.numericUpDown1.Location = new System.Drawing.Point(252, 52);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            22000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(97, 22);
            this.numericUpDown1.TabIndex = 63;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            // 
            // Supplement_Numerical
            // 
            this.Supplement_Numerical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Supplement_Numerical.AutoSize = true;
            this.Supplement_Numerical.Enabled = false;
            this.Supplement_Numerical.Location = new System.Drawing.Point(191, 23);
            this.Supplement_Numerical.Margin = new System.Windows.Forms.Padding(4);
            this.Supplement_Numerical.Name = "Supplement_Numerical";
            this.Supplement_Numerical.Size = new System.Drawing.Size(304, 21);
            this.Supplement_Numerical.TabIndex = 62;
            this.Supplement_Numerical.Text = "Supplement Numerical for Low Frequencies";
            this.Supplement_Numerical.UseVisualStyleBackColor = true;
            // 
            // Hybrid_Select
            // 
            this.Hybrid_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Hybrid_Select.AutoSize = true;
            this.Hybrid_Select.Location = new System.Drawing.Point(8, 23);
            this.Hybrid_Select.Margin = new System.Windows.Forms.Padding(4);
            this.Hybrid_Select.Name = "Hybrid_Select";
            this.Hybrid_Select.Size = new System.Drawing.Size(70, 21);
            this.Hybrid_Select.TabIndex = 55;
            this.Hybrid_Select.TabStop = true;
            this.Hybrid_Select.Text = "Hybrid";
            this.Hybrid_Select.UseVisualStyleBackColor = true;
            this.Hybrid_Select.CheckedChanged += new System.EventHandler(this.Tab_Selecting);
            // 
            // Mapping_Select
            // 
            this.Mapping_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Mapping_Select.AutoSize = true;
            this.Mapping_Select.Location = new System.Drawing.Point(8, 52);
            this.Mapping_Select.Margin = new System.Windows.Forms.Padding(4);
            this.Mapping_Select.Name = "Mapping_Select";
            this.Mapping_Select.Size = new System.Drawing.Size(83, 21);
            this.Mapping_Select.TabIndex = 55;
            this.Mapping_Select.TabStop = true;
            this.Mapping_Select.Text = "Mapping";
            this.Mapping_Select.UseVisualStyleBackColor = true;
            this.Mapping_Select.CheckedChanged += new System.EventHandler(this.Tab_Selecting);
            // 
            // DistributionType
            // 
            this.DistributionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DistributionType.DisplayMember = "1";
            this.DistributionType.FormattingEnabled = true;
            this.DistributionType.Items.AddRange(new object[] {
            "Monaural",
            "Stereo",
            "Binaural (select file...)",
            "A-Format (type I-A)",
            "A-Format (type II-A)",
            "First Order Ambisonics",
            "Second Order Ambisonics",
            "Third Order Ambisonics",
            "Surround Array (select file...)"});
            this.DistributionType.Location = new System.Drawing.Point(262, 104);
            this.DistributionType.Margin = new System.Windows.Forms.Padding(4);
            this.DistributionType.Name = "DistributionType";
            this.DistributionType.Size = new System.Drawing.Size(251, 24);
            this.DistributionType.TabIndex = 54;
            this.DistributionType.Text = "Stereo";
            this.DistributionType.UseWaitCursor = true;
            this.DistributionType.SelectedIndexChanged += new System.EventHandler(this.DistributionType_SelectedIndexChanged);
            // 
            // Move_Up
            // 
            this.Move_Up.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Move_Up.Enabled = false;
            this.Move_Up.Location = new System.Drawing.Point(7, 560);
            this.Move_Up.Margin = new System.Windows.Forms.Padding(4);
            this.Move_Up.Name = "Move_Up";
            this.Move_Up.Size = new System.Drawing.Size(244, 24);
            this.Move_Up.TabIndex = 57;
            this.Move_Up.Text = "Move Up";
            this.Move_Up.UseVisualStyleBackColor = true;
            this.Move_Up.Visible = false;
            this.Move_Up.Click += new System.EventHandler(this.Move_Up_Click);
            // 
            // Channel_View
            // 
            this.Channel_View.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.Channel_View, 2);
            this.Channel_View.FormattingEnabled = true;
            this.Channel_View.ItemHeight = 16;
            this.Channel_View.Items.AddRange(new object[] {
            "0:Left",
            "1:Right"});
            this.Channel_View.Location = new System.Drawing.Point(4, 156);
            this.Channel_View.Margin = new System.Windows.Forms.Padding(4);
            this.Channel_View.Name = "Channel_View";
            this.Channel_View.Size = new System.Drawing.Size(509, 388);
            this.Channel_View.TabIndex = 56;
            // 
            // Add_Channel
            // 
            this.Add_Channel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Add_Channel.Enabled = false;
            this.Add_Channel.Location = new System.Drawing.Point(7, 592);
            this.Add_Channel.Margin = new System.Windows.Forms.Padding(4);
            this.Add_Channel.Name = "Add_Channel";
            this.Add_Channel.Size = new System.Drawing.Size(244, 24);
            this.Add_Channel.TabIndex = 57;
            this.Add_Channel.Text = "Add";
            this.Add_Channel.UseVisualStyleBackColor = true;
            this.Add_Channel.Visible = false;
            this.Add_Channel.Click += new System.EventHandler(this.Add_Channel_Click);
            // 
            // Pach_Auralisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.Tabs);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Pach_Auralisation";
            this.Size = new System.Drawing.Size(533, 689);
            this.Render_Settings.ResumeLayout(false);
            this.Render_Settings.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Normalization_Choice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DryChannel)).EndInit();
            this.Tabs.ResumeLayout(false);
            this.Data_Source.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.Data_From.ResumeLayout(false);
            this.Data_From.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

            }
            internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem FromMeshSphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem DirectionalSourceToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem SelectSourceToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem SelectASphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromSphereObjectToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem2;
            internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
            internal System.Windows.Forms.TabPage Render_Settings;
            internal System.Windows.Forms.ComboBox Source_Aim;
            internal System.Windows.Forms.Label label27;
            internal System.Windows.Forms.Label label26;
            private System.Windows.Forms.NumericUpDown Alt_Choice;
            private System.Windows.Forms.NumericUpDown Azi_Choice;
            internal System.Windows.Forms.Label label25;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
            internal System.Windows.Forms.Button RenderBtn;
            private ZedGraph.ZedGraphControl Analysis_View;
            internal System.Windows.Forms.Label label16;
            private System.Windows.Forms.NumericUpDown DryChannel;
            internal System.Windows.Forms.TextBox Signal_Status;
            internal System.Windows.Forms.Button OpenSignal;
            internal System.Windows.Forms.Label label20;
            internal System.Windows.Forms.ComboBox Receiver_Choice;
            internal System.Windows.Forms.TabControl Tabs;
            private System.Windows.Forms.TabPage Data_Source;
            internal System.Windows.Forms.CheckedListBox SourceList;
            internal System.Windows.Forms.ComboBox DistributionType;
            private System.Windows.Forms.RadioButton Hybrid_Select;
            private System.Windows.Forms.RadioButton Mapping_Select;
            internal System.Windows.Forms.ComboBox Graph_Octave;
            private System.Windows.Forms.ListBox Channel_View;
            private System.Windows.Forms.GroupBox Data_From;
            private System.Windows.Forms.Button Remove_Channel;
            private System.Windows.Forms.Button Save_Channels;
            private System.Windows.Forms.Button Add_Channel;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.NumericUpDown numericUpDown1;
            private System.Windows.Forms.CheckBox Supplement_Numerical;
            private System.Windows.Forms.Label label5;
            private System.Windows.Forms.Label label4;
            private System.Windows.Forms.Button Move_Down;
            private System.Windows.Forms.Button Move_Up;
            private System.Windows.Forms.ColorDialog colorDialog1;
            internal System.Windows.Forms.Button Export_Filter;
            private System.Windows.Forms.ComboBox Sample_Freq_Selection;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.NumericUpDown Normalization_Choice;
            internal System.Windows.Forms.Label label6;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        }
    }
}