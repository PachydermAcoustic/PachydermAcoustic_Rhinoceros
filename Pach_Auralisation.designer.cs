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
            this.SourceList = new System.Windows.Forms.CheckedListBox();
            this.Source_Aim = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.Alt_Choice = new System.Windows.Forms.NumericUpDown();
            this.Azi_Choice = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.Export_Filter = new System.Windows.Forms.Button();
            this.Graph_Octave = new System.Windows.Forms.ComboBox();
            this.RenderBtn = new System.Windows.Forms.Button();
            this.Analysis_View = new ZedGraph.ZedGraphControl();
            this.label16 = new System.Windows.Forms.Label();
            this.DryChannel = new System.Windows.Forms.NumericUpDown();
            this.Signal_Status = new System.Windows.Forms.TextBox();
            this.OpenSignal = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.Receiver_Choice = new System.Windows.Forms.ComboBox();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.Data_Source = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Data_From = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.Supplement_Numerical = new System.Windows.Forms.CheckBox();
            this.Hybrid_Select = new System.Windows.Forms.RadioButton();
            this.Mapping_Select = new System.Windows.Forms.RadioButton();
            this.Move_Down = new System.Windows.Forms.Button();
            this.Remove_Channel = new System.Windows.Forms.Button();
            this.Save_Channels = new System.Windows.Forms.Button();
            this.Move_Up = new System.Windows.Forms.Button();
            this.Add_Channel = new System.Windows.Forms.Button();
            this.Channel_View = new System.Windows.Forms.ListBox();
            this.DistributionType = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Render_Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DryChannel)).BeginInit();
            this.Tabs.SuspendLayout();
            this.Data_Source.SuspendLayout();
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
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(214, 22);
            this.ToolStripMenuItem1.Text = "Omni-Directional Source...";
            // 
            // FromMeshSphereToolStripMenuItem
            // 
            this.FromMeshSphereToolStripMenuItem.Name = "FromMeshSphereToolStripMenuItem";
            this.FromMeshSphereToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.FromMeshSphereToolStripMenuItem.Text = "From MeshSphere";
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
            this.DirectionalSourceToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
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
            this.SelectASphereToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.SelectASphereToolStripMenuItem.Text = "Select...";
            // 
            // FromSphereObjectToolStripMenuItem1
            // 
            this.FromSphereObjectToolStripMenuItem1.Name = "FromSphereObjectToolStripMenuItem1";
            this.FromSphereObjectToolStripMenuItem1.Size = new System.Drawing.Size(179, 22);
            this.FromSphereObjectToolStripMenuItem1.Text = "From Sphere Object";
            // 
            // FromPointInputToolStripMenuItem2
            // 
            this.FromPointInputToolStripMenuItem2.Name = "FromPointInputToolStripMenuItem2";
            this.FromPointInputToolStripMenuItem2.Size = new System.Drawing.Size(179, 22);
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
            this.Render_Settings.Controls.Add(this.SourceList);
            this.Render_Settings.Controls.Add(this.Source_Aim);
            this.Render_Settings.Controls.Add(this.label27);
            this.Render_Settings.Controls.Add(this.label26);
            this.Render_Settings.Controls.Add(this.Alt_Choice);
            this.Render_Settings.Controls.Add(this.Azi_Choice);
            this.Render_Settings.Controls.Add(this.label25);
            this.Render_Settings.Controls.Add(this.tableLayoutPanel3);
            this.Render_Settings.Controls.Add(this.label20);
            this.Render_Settings.Controls.Add(this.Receiver_Choice);
            this.Render_Settings.Location = new System.Drawing.Point(4, 22);
            this.Render_Settings.Name = "Render_Settings";
            this.Render_Settings.Padding = new System.Windows.Forms.Padding(3);
            this.Render_Settings.Size = new System.Drawing.Size(392, 534);
            this.Render_Settings.TabIndex = 2;
            this.Render_Settings.Text = "Render Settings";
            this.Render_Settings.UseVisualStyleBackColor = true;
            this.Render_Settings.UseWaitCursor = true;
            // 
            // SourceList
            // 
            this.SourceList.CheckOnClick = true;
            this.SourceList.FormattingEnabled = true;
            this.SourceList.Location = new System.Drawing.Point(6, 6);
            this.SourceList.MinimumSize = new System.Drawing.Size(4, 64);
            this.SourceList.Name = "SourceList";
            this.SourceList.ScrollAlwaysVisible = true;
            this.SourceList.Size = new System.Drawing.Size(239, 94);
            this.SourceList.TabIndex = 52;
            this.SourceList.UseWaitCursor = true;
            // 
            // Source_Aim
            // 
            this.Source_Aim.FormattingEnabled = true;
            this.Source_Aim.Location = new System.Drawing.Point(325, 33);
            this.Source_Aim.Name = "Source_Aim";
            this.Source_Aim.Size = new System.Drawing.Size(61, 21);
            this.Source_Aim.TabIndex = 51;
            this.Source_Aim.Text = "None";
            this.Source_Aim.UseWaitCursor = true;
            this.Source_Aim.SelectedIndexChanged += new System.EventHandler(this.Source_Aim_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Enabled = false;
            this.label27.Location = new System.Drawing.Point(251, 62);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(42, 13);
            this.label27.TabIndex = 50;
            this.label27.Text = "Altitude";
            this.label27.UseWaitCursor = true;
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Enabled = false;
            this.label26.Location = new System.Drawing.Point(251, 88);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(44, 13);
            this.label26.TabIndex = 49;
            this.label26.Text = "Azimuth";
            this.label26.UseWaitCursor = true;
            // 
            // Alt_Choice
            // 
            this.Alt_Choice.DecimalPlaces = 2;
            this.Alt_Choice.Location = new System.Drawing.Point(299, 60);
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
            this.Alt_Choice.Size = new System.Drawing.Size(87, 20);
            this.Alt_Choice.TabIndex = 48;
            this.Alt_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Alt_Choice.UseWaitCursor = true;
            this.Alt_Choice.ValueChanged += new System.EventHandler(this.Alt_Choice_ValueChanged);
            // 
            // Azi_Choice
            // 
            this.Azi_Choice.DecimalPlaces = 2;
            this.Azi_Choice.Location = new System.Drawing.Point(299, 86);
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
            this.Azi_Choice.Size = new System.Drawing.Size(87, 20);
            this.Azi_Choice.TabIndex = 47;
            this.Azi_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Azi_Choice.UseWaitCursor = true;
            this.Azi_Choice.ValueChanged += new System.EventHandler(this.Azi_Choice_ValueChanged);
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Enabled = false;
            this.label25.Location = new System.Drawing.Point(251, 36);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(73, 13);
            this.label25.TabIndex = 46;
            this.label25.Text = "Aim at Source";
            this.label25.UseWaitCursor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.1845F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.8155F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.tableLayoutPanel3.Controls.Add(this.Export_Filter, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.Graph_Octave, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.RenderBtn, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.Analysis_View, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label16, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.DryChannel, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.Signal_Status, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.OpenSignal, 0, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 112);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(429, 212);
            this.tableLayoutPanel3.TabIndex = 43;
            this.tableLayoutPanel3.UseWaitCursor = true;
            // 
            // Export_Filter
            // 
            this.Export_Filter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Export_Filter, 4);
            this.Export_Filter.Location = new System.Drawing.Point(3, 189);
            this.Export_Filter.Name = "Export_Filter";
            this.Export_Filter.Size = new System.Drawing.Size(423, 20);
            this.Export_Filter.TabIndex = 54;
            this.Export_Filter.Text = "Export IR as Wave File";
            this.Export_Filter.UseVisualStyleBackColor = true;
            this.Export_Filter.UseWaitCursor = true;
            this.Export_Filter.Click += new System.EventHandler(this.ExportFilter);
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
            this.Graph_Octave.Location = new System.Drawing.Point(3, 3);
            this.Graph_Octave.Name = "Graph_Octave";
            this.Graph_Octave.Size = new System.Drawing.Size(160, 21);
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
            this.RenderBtn.Location = new System.Drawing.Point(3, 163);
            this.RenderBtn.Name = "RenderBtn";
            this.RenderBtn.Size = new System.Drawing.Size(423, 20);
            this.RenderBtn.TabIndex = 49;
            this.RenderBtn.Text = "Render Auralization";
            this.RenderBtn.UseVisualStyleBackColor = true;
            this.RenderBtn.UseWaitCursor = true;
            this.RenderBtn.Click += new System.EventHandler(this.RenderBtn_Click);
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
            this.Analysis_View.Location = new System.Drawing.Point(3, 28);
            this.Analysis_View.Name = "Analysis_View";
            this.Analysis_View.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.Analysis_View.ScrollGrace = 0D;
            this.Analysis_View.ScrollMaxX = 0D;
            this.Analysis_View.ScrollMaxY = 120D;
            this.Analysis_View.ScrollMaxY2 = 0D;
            this.Analysis_View.ScrollMinX = 0D;
            this.Analysis_View.ScrollMinY = 0D;
            this.Analysis_View.ScrollMinY2 = 0D;
            this.Analysis_View.Size = new System.Drawing.Size(423, 71);
            this.Analysis_View.TabIndex = 42;
            this.Analysis_View.UseWaitCursor = true;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(169, 102);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(97, 32);
            this.label16.TabIndex = 46;
            this.label16.Text = "Selected Channel";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label16.UseWaitCursor = true;
            // 
            // DryChannel
            // 
            this.DryChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DryChannel.AutoSize = true;
            this.DryChannel.Location = new System.Drawing.Point(272, 105);
            this.DryChannel.Name = "DryChannel";
            this.DryChannel.Size = new System.Drawing.Size(154, 20);
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
            this.Signal_Status.Location = new System.Drawing.Point(3, 137);
            this.Signal_Status.Name = "Signal_Status";
            this.Signal_Status.ReadOnly = true;
            this.Signal_Status.Size = new System.Drawing.Size(423, 20);
            this.Signal_Status.TabIndex = 48;
            this.Signal_Status.UseWaitCursor = true;
            // 
            // OpenSignal
            // 
            this.OpenSignal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.OpenSignal, 2);
            this.OpenSignal.Location = new System.Drawing.Point(3, 105);
            this.OpenSignal.Name = "OpenSignal";
            this.OpenSignal.Size = new System.Drawing.Size(160, 26);
            this.OpenSignal.TabIndex = 47;
            this.OpenSignal.Text = "Open...";
            this.OpenSignal.UseVisualStyleBackColor = true;
            this.OpenSignal.UseWaitCursor = true;
            this.OpenSignal.Click += new System.EventHandler(this.OpenSignal_Click);
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Enabled = false;
            this.label20.Location = new System.Drawing.Point(251, 9);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 13);
            this.label20.TabIndex = 40;
            this.label20.Text = "Receiver";
            this.label20.UseWaitCursor = true;
            // 
            // Receiver_Choice
            // 
            this.Receiver_Choice.FormattingEnabled = true;
            this.Receiver_Choice.Items.AddRange(new object[] {
            "0"});
            this.Receiver_Choice.Location = new System.Drawing.Point(325, 6);
            this.Receiver_Choice.Name = "Receiver_Choice";
            this.Receiver_Choice.Size = new System.Drawing.Size(61, 21);
            this.Receiver_Choice.TabIndex = 39;
            this.Receiver_Choice.Text = "0";
            this.Receiver_Choice.UseWaitCursor = true;
            this.Receiver_Choice.SelectedIndexChanged += new System.EventHandler(this.Receiver_Choice_SelectedIndexChanged);
            // 
            // Tabs
            // 
            this.Tabs.AccessibleDescription = "";
            this.Tabs.AccessibleName = "";
            this.Tabs.Controls.Add(this.Data_Source);
            this.Tabs.Controls.Add(this.Render_Settings);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.MinimumSize = new System.Drawing.Size(400, 400);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(400, 560);
            this.Tabs.TabIndex = 5;
            this.Tabs.SelectedIndexChanged += new System.EventHandler(this.Tab_Selecting);
            // 
            // Data_Source
            // 
            this.Data_Source.Controls.Add(this.label2);
            this.Data_Source.Controls.Add(this.label1);
            this.Data_Source.Controls.Add(this.Data_From);
            this.Data_Source.Controls.Add(this.Move_Down);
            this.Data_Source.Controls.Add(this.Remove_Channel);
            this.Data_Source.Controls.Add(this.Save_Channels);
            this.Data_Source.Controls.Add(this.Move_Up);
            this.Data_Source.Controls.Add(this.Add_Channel);
            this.Data_Source.Controls.Add(this.Channel_View);
            this.Data_Source.Controls.Add(this.DistributionType);
            this.Data_Source.Location = new System.Drawing.Point(4, 22);
            this.Data_Source.Name = "Data_Source";
            this.Data_Source.Padding = new System.Windows.Forms.Padding(3);
            this.Data_Source.Size = new System.Drawing.Size(392, 534);
            this.Data_Source.TabIndex = 3;
            this.Data_Source.Text = "Data Source";
            this.Data_Source.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Channels:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Type of Auralisation:";
            // 
            // Data_From
            // 
            this.Data_From.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Data_From.Controls.Add(this.label5);
            this.Data_From.Controls.Add(this.label4);
            this.Data_From.Controls.Add(this.numericUpDown1);
            this.Data_From.Controls.Add(this.Supplement_Numerical);
            this.Data_From.Controls.Add(this.Hybrid_Select);
            this.Data_From.Controls.Add(this.Mapping_Select);
            this.Data_From.Location = new System.Drawing.Point(6, 6);
            this.Data_From.Name = "Data_From";
            this.Data_From.Size = new System.Drawing.Size(380, 77);
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
            this.label5.Location = new System.Drawing.Point(266, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
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
            this.label4.Location = new System.Drawing.Point(155, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 65;
            this.label4.Text = "For :";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Enabled = false;
            this.numericUpDown1.Location = new System.Drawing.Point(189, 42);
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
            this.numericUpDown1.Size = new System.Drawing.Size(71, 20);
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
            this.Supplement_Numerical.Location = new System.Drawing.Point(143, 19);
            this.Supplement_Numerical.Name = "Supplement_Numerical";
            this.Supplement_Numerical.Size = new System.Drawing.Size(231, 17);
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
            this.Hybrid_Select.Location = new System.Drawing.Point(6, 19);
            this.Hybrid_Select.Name = "Hybrid_Select";
            this.Hybrid_Select.Size = new System.Drawing.Size(55, 17);
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
            this.Mapping_Select.Location = new System.Drawing.Point(6, 42);
            this.Mapping_Select.Name = "Mapping_Select";
            this.Mapping_Select.Size = new System.Drawing.Size(66, 17);
            this.Mapping_Select.TabIndex = 55;
            this.Mapping_Select.TabStop = true;
            this.Mapping_Select.Text = "Mapping";
            this.Mapping_Select.UseVisualStyleBackColor = true;
            this.Mapping_Select.CheckedChanged += new System.EventHandler(this.Tab_Selecting);
            // 
            // Move_Down
            // 
            this.Move_Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Move_Down.Enabled = false;
            this.Move_Down.Location = new System.Drawing.Point(195, 243);
            this.Move_Down.Name = "Move_Down";
            this.Move_Down.Size = new System.Drawing.Size(191, 23);
            this.Move_Down.TabIndex = 59;
            this.Move_Down.Text = "Move Down";
            this.Move_Down.UseVisualStyleBackColor = true;
            this.Move_Down.Visible = false;
            this.Move_Down.Click += new System.EventHandler(this.Move_Down_Click);
            // 
            // Remove_Channel
            // 
            this.Remove_Channel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Remove_Channel.Enabled = false;
            this.Remove_Channel.Location = new System.Drawing.Point(195, 272);
            this.Remove_Channel.Name = "Remove_Channel";
            this.Remove_Channel.Size = new System.Drawing.Size(191, 23);
            this.Remove_Channel.TabIndex = 59;
            this.Remove_Channel.Text = "Remove";
            this.Remove_Channel.UseVisualStyleBackColor = true;
            this.Remove_Channel.Visible = false;
            this.Remove_Channel.Click += new System.EventHandler(this.Remove_Channel_Click);
            // 
            // Save_Channels
            // 
            this.Save_Channels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Save_Channels.Enabled = false;
            this.Save_Channels.Location = new System.Drawing.Point(6, 301);
            this.Save_Channels.Name = "Save_Channels";
            this.Save_Channels.Size = new System.Drawing.Size(380, 23);
            this.Save_Channels.TabIndex = 58;
            this.Save_Channels.Text = "Save Configuration";
            this.Save_Channels.UseVisualStyleBackColor = true;
            this.Save_Channels.Visible = false;
            this.Save_Channels.Click += new System.EventHandler(this.Save_Channels_Click);
            // 
            // Move_Up
            // 
            this.Move_Up.Enabled = false;
            this.Move_Up.Location = new System.Drawing.Point(6, 243);
            this.Move_Up.Name = "Move_Up";
            this.Move_Up.Size = new System.Drawing.Size(183, 23);
            this.Move_Up.TabIndex = 57;
            this.Move_Up.Text = "Move Up";
            this.Move_Up.UseVisualStyleBackColor = true;
            this.Move_Up.Visible = false;
            this.Move_Up.Click += new System.EventHandler(this.Move_Up_Click);
            // 
            // Add_Channel
            // 
            this.Add_Channel.Enabled = false;
            this.Add_Channel.Location = new System.Drawing.Point(6, 272);
            this.Add_Channel.Name = "Add_Channel";
            this.Add_Channel.Size = new System.Drawing.Size(183, 23);
            this.Add_Channel.TabIndex = 57;
            this.Add_Channel.Text = "Add";
            this.Add_Channel.UseVisualStyleBackColor = true;
            this.Add_Channel.Visible = false;
            this.Add_Channel.Click += new System.EventHandler(this.Add_Channel_Click);
            // 
            // Channel_View
            // 
            this.Channel_View.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Channel_View.FormattingEnabled = true;
            this.Channel_View.Items.AddRange(new object[] {
            "0:Left",
            "1:Right"});
            this.Channel_View.Location = new System.Drawing.Point(6, 142);
            this.Channel_View.Name = "Channel_View";
            this.Channel_View.Size = new System.Drawing.Size(380, 95);
            this.Channel_View.TabIndex = 56;
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
            this.DistributionType.Location = new System.Drawing.Point(6, 102);
            this.DistributionType.Name = "DistributionType";
            this.DistributionType.Size = new System.Drawing.Size(180, 21);
            this.DistributionType.TabIndex = 54;
            this.DistributionType.Text = "Stereo";
            this.DistributionType.UseWaitCursor = true;
            this.DistributionType.SelectedIndexChanged += new System.EventHandler(this.DistributionType_SelectedIndexChanged);
            // 
            // Pach_Auralisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.Tabs);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Pach_Auralisation";
            this.Size = new System.Drawing.Size(400, 560);
            this.Render_Settings.ResumeLayout(false);
            this.Render_Settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DryChannel)).EndInit();
            this.Tabs.ResumeLayout(false);
            this.Data_Source.ResumeLayout(false);
            this.Data_Source.PerformLayout();
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
        }
    }
}