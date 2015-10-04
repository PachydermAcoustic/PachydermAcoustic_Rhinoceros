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
        partial class Pach_Hybrid_Control : System.Windows.Forms.UserControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel1 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel2 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel3 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel4 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel5 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel6 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel7 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel8 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel9 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel10 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel11 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel12 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.Abs4kOut = new System.Windows.Forms.NumericUpDown();
            this.Abs8kOut = new System.Windows.Forms.NumericUpDown();
            this.Abs8k = new System.Windows.Forms.TrackBar();
            this.Abs4k = new System.Windows.Forms.TrackBar();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Abs2k = new System.Windows.Forms.TrackBar();
            this.Abs2kOut = new System.Windows.Forms.NumericUpDown();
            this.Abs1kOut = new System.Windows.Forms.NumericUpDown();
            this.Abs1k = new System.Windows.Forms.TrackBar();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Abs500 = new System.Windows.Forms.TrackBar();
            this.Abs500Out = new System.Windows.Forms.NumericUpDown();
            this.Abs250Out = new System.Windows.Forms.NumericUpDown();
            this.Abs125Out = new System.Windows.Forms.NumericUpDown();
            this.Abs250 = new System.Windows.Forms.TrackBar();
            this.Abs125 = new System.Windows.Forms.TrackBar();
            this.Abs63Out = new System.Windows.Forms.NumericUpDown();
            this.Abs63 = new System.Windows.Forms.TrackBar();
            this.label18 = new System.Windows.Forms.Label();
            this.AbsFlat = new System.Windows.Forms.TrackBar();
            this.ScatFlat = new System.Windows.Forms.TrackBar();
            this.Scat8kOut = new System.Windows.Forms.NumericUpDown();
            this.Scat8kv = new System.Windows.Forms.TrackBar();
            this.Scat4kv = new System.Windows.Forms.TrackBar();
            this.Scat4kOut = new System.Windows.Forms.NumericUpDown();
            this.Scat2kOut = new System.Windows.Forms.NumericUpDown();
            this.Scat2kv = new System.Windows.Forms.TrackBar();
            this.Scat1kv = new System.Windows.Forms.TrackBar();
            this.Scat1kOut = new System.Windows.Forms.NumericUpDown();
            this.Scat500Out = new System.Windows.Forms.NumericUpDown();
            this.Scat500v = new System.Windows.Forms.TrackBar();
            this.Scat250v = new System.Windows.Forms.TrackBar();
            this.Scat250Out = new System.Windows.Forms.NumericUpDown();
            this.Scat125Out = new System.Windows.Forms.NumericUpDown();
            this.Scat125v = new System.Windows.Forms.TrackBar();
            this.Scat63v = new System.Windows.Forms.TrackBar();
            this.Scat63Out = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.SmartMat_Display = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Calculate = new System.Windows.Forms.Button();
            this.RTBox = new System.Windows.Forms.CheckBox();
            this.ISBox = new System.Windows.Forms.CheckBox();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.BTM_ED = new System.Windows.Forms.CheckBox();
            this.Specular_Trace = new System.Windows.Forms.CheckBox();
            this.Spec_RayCount = new System.Windows.Forms.NumericUpDown();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.EdgeFreq = new System.Windows.Forms.CheckBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.Atten_Method = new System.Windows.Forms.ComboBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.Air_Pressure = new System.Windows.Forms.NumericUpDown();
            this.Label3 = new System.Windows.Forms.Label();
            this.Rel_Humidity = new System.Windows.Forms.NumericUpDown();
            this.AirTemp = new System.Windows.Forms.Label();
            this.Air_Temp = new System.Windows.Forms.NumericUpDown();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.Label17 = new System.Windows.Forms.Label();
            this.ReceiverSelection = new System.Windows.Forms.ComboBox();
            this.COTime = new System.Windows.Forms.Label();
            this.CO_TIME = new System.Windows.Forms.NumericUpDown();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.RT_Count = new System.Windows.Forms.NumericUpDown();
            this.Image_Order = new System.Windows.Forms.NumericUpDown();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Absorption = new System.Windows.Forms.TabPage();
            this.Scattering = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.Transparency = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.Trans_Flat = new System.Windows.Forms.TrackBar();
            this.Trans_8k_Out = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.Trans_8kv = new System.Windows.Forms.TrackBar();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.Trans_4k_Out = new System.Windows.Forms.NumericUpDown();
            this.Trans_4kv = new System.Windows.Forms.TrackBar();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.Trans_2k_Out = new System.Windows.Forms.NumericUpDown();
            this.label35 = new System.Windows.Forms.Label();
            this.Trans_2kv = new System.Windows.Forms.TrackBar();
            this.label36 = new System.Windows.Forms.Label();
            this.Trans_63v = new System.Windows.Forms.TrackBar();
            this.Trans_63_Out = new System.Windows.Forms.NumericUpDown();
            this.Trans_1k_Out = new System.Windows.Forms.NumericUpDown();
            this.Trans_1kv = new System.Windows.Forms.TrackBar();
            this.Trans_125_Out = new System.Windows.Forms.NumericUpDown();
            this.Trans_125v = new System.Windows.Forms.TrackBar();
            this.Trans_500_Out = new System.Windows.Forms.NumericUpDown();
            this.Trans_250v = new System.Windows.Forms.TrackBar();
            this.Trans_500v = new System.Windows.Forms.TrackBar();
            this.Trans_250_Out = new System.Windows.Forms.NumericUpDown();
            this.Trans_Check = new System.Windows.Forms.CheckBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.Abs_Designer = new System.Windows.Forms.Button();
            this.LayerLbl = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.SaveAbs = new System.Windows.Forms.Button();
            this.Material_Name = new System.Windows.Forms.MaskedTextBox();
            this.LayerDisplay = new System.Windows.Forms.ComboBox();
            this.Material_Lib = new System.Windows.Forms.ListBox();
            this.Mat_Lbl = new System.Windows.Forms.Label();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.Source_Aim = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.Alt_Choice = new System.Windows.Forms.NumericUpDown();
            this.Azi_Choice = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.SourceList = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.IS_Path_Box = new System.Windows.Forms.CheckedListBox();
            this.Analysis_View = new ZedGraph.ZedGraphControl();
            this.Normalize_Graph = new System.Windows.Forms.CheckBox();
            this.LockUserScale = new System.Windows.Forms.CheckBox();
            this.Graph_Octave = new System.Windows.Forms.ComboBox();
            this.Graph_Type = new System.Windows.Forms.ComboBox();
            this.Auralisation = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.Receiver_Choice = new System.Windows.Forms.ComboBox();
            this.PathCount = new System.Windows.Forms.Label();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SRT8 = new System.Windows.Forms.Label();
            this.Parameter_Choice = new System.Windows.Forms.ComboBox();
            this.SRT7 = new System.Windows.Forms.Label();
            this.SRT6 = new System.Windows.Forms.Label();
            this.SRT2 = new System.Windows.Forms.Label();
            this.SRT5 = new System.Windows.Forms.Label();
            this.SRT3 = new System.Windows.Forms.Label();
            this.SRT4 = new System.Windows.Forms.Label();
            this.SRT1 = new System.Windows.Forms.Label();
            this.ISOCOMP = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromMeshSphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DirectionalSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectSourceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectASphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromSphereObjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.SP_menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveParameterResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePTBFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveVRSpectraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AbsFlat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatFlat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat8kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat8kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat4kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat4kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat2kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat2kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat1kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat1kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat500Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat500v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat250v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat250Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat125Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat125v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat63v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat63Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmartMat_Display)).BeginInit();
            this.Tabs.SuspendLayout();
            this.TabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Spec_RayCount)).BeginInit();
            this.GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).BeginInit();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Order)).BeginInit();
            this.TabPage4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Absorption.SuspendLayout();
            this.Scattering.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.Transparency.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_Flat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_8k_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_8kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_4k_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_4kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_2k_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_2kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_63v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_63_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_1k_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_1kv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_125_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_125v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_500_Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_250v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_500v)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_250_Out)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.TabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SP_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel4.Controls.Add(this.label24, 0, 9);
            this.tableLayoutPanel4.Controls.Add(this.Abs4kOut, 2, 7);
            this.tableLayoutPanel4.Controls.Add(this.Abs8kOut, 2, 8);
            this.tableLayoutPanel4.Controls.Add(this.Abs8k, 1, 8);
            this.tableLayoutPanel4.Controls.Add(this.Abs4k, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.Label8, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.Label6, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.Label9, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.Abs2k, 1, 6);
            this.tableLayoutPanel4.Controls.Add(this.Abs2kOut, 2, 6);
            this.tableLayoutPanel4.Controls.Add(this.Abs1kOut, 2, 5);
            this.tableLayoutPanel4.Controls.Add(this.Abs1k, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.Label10, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.Label11, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.Label7, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.Label12, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.Label13, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.Abs500, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.Abs500Out, 2, 4);
            this.tableLayoutPanel4.Controls.Add(this.Abs250Out, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.Abs125Out, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.Abs250, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.Abs125, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.Abs63Out, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.Abs63, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.AbsFlat, 1, 9);
            this.tableLayoutPanel4.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 8);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 11;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(365, 328);
            this.tableLayoutPanel4.TabIndex = 29;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 215);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(53, 25);
            this.label24.TabIndex = 38;
            this.label24.Text = "Flatten All";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Abs4kOut
            // 
            this.Abs4kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs4kOut.AutoSize = true;
            this.Abs4kOut.Location = new System.Drawing.Point(298, 168);
            this.Abs4kOut.Name = "Abs4kOut";
            this.Abs4kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs4kOut.Size = new System.Drawing.Size(54, 20);
            this.Abs4kOut.TabIndex = 32;
            this.Abs4kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs4kOut.ValueChanged += new System.EventHandler(this.Abs4kOut_ValueChanged);
            // 
            // Abs8kOut
            // 
            this.Abs8kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs8kOut.AutoSize = true;
            this.Abs8kOut.Location = new System.Drawing.Point(298, 193);
            this.Abs8kOut.Name = "Abs8kOut";
            this.Abs8kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs8kOut.Size = new System.Drawing.Size(54, 20);
            this.Abs8kOut.TabIndex = 33;
            this.Abs8kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs8kOut.ValueChanged += new System.EventHandler(this.Abs8kOut_ValueChanged);
            // 
            // Abs8k
            // 
            this.Abs8k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs8k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs8k.LargeChange = 10;
            this.Abs8k.Location = new System.Drawing.Point(103, 193);
            this.Abs8k.Maximum = 100;
            this.Abs8k.Name = "Abs8k";
            this.Abs8k.Size = new System.Drawing.Size(189, 19);
            this.Abs8k.TabIndex = 14;
            this.Abs8k.TickFrequency = 10;
            this.Abs8k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs8k.Value = 1;
            this.Abs8k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs4k
            // 
            this.Abs4k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs4k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs4k.LargeChange = 10;
            this.Abs4k.Location = new System.Drawing.Point(103, 168);
            this.Abs4k.Maximum = 100;
            this.Abs4k.Name = "Abs4k";
            this.Abs4k.Size = new System.Drawing.Size(189, 19);
            this.Abs4k.TabIndex = 12;
            this.Abs4k.TickFrequency = 10;
            this.Abs4k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs4k.Value = 1;
            this.Abs4k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Label8
            // 
            this.Label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(3, 190);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(35, 25);
            this.Label8.TabIndex = 24;
            this.Label8.Text = "8 kHz";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label6
            // 
            this.Label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(3, 165);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(35, 25);
            this.Label6.TabIndex = 22;
            this.Label6.Text = "4 kHz";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label9
            // 
            this.Label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(3, 140);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(35, 25);
            this.Label9.TabIndex = 21;
            this.Label9.Text = "2 kHz";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Abs2k
            // 
            this.Abs2k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs2k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs2k.LargeChange = 10;
            this.Abs2k.Location = new System.Drawing.Point(103, 143);
            this.Abs2k.Maximum = 100;
            this.Abs2k.Name = "Abs2k";
            this.Abs2k.Size = new System.Drawing.Size(189, 19);
            this.Abs2k.TabIndex = 11;
            this.Abs2k.TickFrequency = 10;
            this.Abs2k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs2k.Value = 1;
            this.Abs2k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs2kOut
            // 
            this.Abs2kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs2kOut.AutoSize = true;
            this.Abs2kOut.Location = new System.Drawing.Point(298, 143);
            this.Abs2kOut.Name = "Abs2kOut";
            this.Abs2kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs2kOut.Size = new System.Drawing.Size(54, 20);
            this.Abs2kOut.TabIndex = 31;
            this.Abs2kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs2kOut.ValueChanged += new System.EventHandler(this.Abs2kOut_ValueChanged);
            // 
            // Abs1kOut
            // 
            this.Abs1kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1kOut.AutoSize = true;
            this.Abs1kOut.Location = new System.Drawing.Point(298, 118);
            this.Abs1kOut.Name = "Abs1kOut";
            this.Abs1kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs1kOut.Size = new System.Drawing.Size(54, 20);
            this.Abs1kOut.TabIndex = 30;
            this.Abs1kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs1kOut.ValueChanged += new System.EventHandler(this.Abs1kOut_ValueChanged);
            // 
            // Abs1k
            // 
            this.Abs1k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs1k.LargeChange = 10;
            this.Abs1k.Location = new System.Drawing.Point(103, 118);
            this.Abs1k.Maximum = 100;
            this.Abs1k.Name = "Abs1k";
            this.Abs1k.Size = new System.Drawing.Size(189, 19);
            this.Abs1k.TabIndex = 10;
            this.Abs1k.TickFrequency = 10;
            this.Abs1k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs1k.Value = 1;
            this.Abs1k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Label10
            // 
            this.Label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(3, 115);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(35, 25);
            this.Label10.TabIndex = 20;
            this.Label10.Text = "1 kHz";
            this.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label11
            // 
            this.Label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(3, 90);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(41, 25);
            this.Label11.TabIndex = 19;
            this.Label11.Text = "500 Hz";
            this.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label7
            // 
            this.Label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(3, 65);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(41, 25);
            this.Label7.TabIndex = 25;
            this.Label7.Text = "250 Hz";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label12
            // 
            this.Label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(3, 40);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(41, 25);
            this.Label12.TabIndex = 18;
            this.Label12.Text = "125 Hz";
            this.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label13
            // 
            this.Label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(3, 15);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(44, 25);
            this.Label13.TabIndex = 17;
            this.Label13.Text = "62.5 Hz";
            this.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Abs500
            // 
            this.Abs500.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs500.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs500.LargeChange = 10;
            this.Abs500.Location = new System.Drawing.Point(103, 93);
            this.Abs500.Maximum = 100;
            this.Abs500.Name = "Abs500";
            this.Abs500.Size = new System.Drawing.Size(189, 19);
            this.Abs500.TabIndex = 13;
            this.Abs500.TickFrequency = 10;
            this.Abs500.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs500.Value = 1;
            this.Abs500.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs500Out
            // 
            this.Abs500Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs500Out.AutoSize = true;
            this.Abs500Out.Location = new System.Drawing.Point(298, 93);
            this.Abs500Out.Name = "Abs500Out";
            this.Abs500Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs500Out.Size = new System.Drawing.Size(54, 20);
            this.Abs500Out.TabIndex = 29;
            this.Abs500Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs500Out.ValueChanged += new System.EventHandler(this.Abs500Out_ValueChanged);
            // 
            // Abs250Out
            // 
            this.Abs250Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs250Out.AutoSize = true;
            this.Abs250Out.Location = new System.Drawing.Point(298, 68);
            this.Abs250Out.Name = "Abs250Out";
            this.Abs250Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs250Out.Size = new System.Drawing.Size(54, 20);
            this.Abs250Out.TabIndex = 28;
            this.Abs250Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs250Out.ValueChanged += new System.EventHandler(this.Abs250Out_ValueChanged);
            // 
            // Abs125Out
            // 
            this.Abs125Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs125Out.AutoSize = true;
            this.Abs125Out.Location = new System.Drawing.Point(298, 43);
            this.Abs125Out.Name = "Abs125Out";
            this.Abs125Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs125Out.Size = new System.Drawing.Size(54, 20);
            this.Abs125Out.TabIndex = 27;
            this.Abs125Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs125Out.ValueChanged += new System.EventHandler(this.Abs125Out_ValueChanged);
            // 
            // Abs250
            // 
            this.Abs250.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs250.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs250.LargeChange = 10;
            this.Abs250.Location = new System.Drawing.Point(103, 68);
            this.Abs250.Maximum = 100;
            this.Abs250.Name = "Abs250";
            this.Abs250.Size = new System.Drawing.Size(189, 19);
            this.Abs250.TabIndex = 9;
            this.Abs250.TickFrequency = 10;
            this.Abs250.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs250.Value = 1;
            this.Abs250.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs125
            // 
            this.Abs125.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs125.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs125.LargeChange = 10;
            this.Abs125.Location = new System.Drawing.Point(103, 43);
            this.Abs125.Maximum = 100;
            this.Abs125.Name = "Abs125";
            this.Abs125.Size = new System.Drawing.Size(189, 19);
            this.Abs125.TabIndex = 15;
            this.Abs125.TickFrequency = 10;
            this.Abs125.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs125.Value = 1;
            this.Abs125.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs63Out
            // 
            this.Abs63Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs63Out.AutoSize = true;
            this.Abs63Out.Location = new System.Drawing.Point(298, 18);
            this.Abs63Out.Name = "Abs63Out";
            this.Abs63Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs63Out.Size = new System.Drawing.Size(54, 20);
            this.Abs63Out.TabIndex = 26;
            this.Abs63Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs63Out.ValueChanged += new System.EventHandler(this.Abs63Out_ValueChanged);
            // 
            // Abs63
            // 
            this.Abs63.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs63.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs63.LargeChange = 10;
            this.Abs63.Location = new System.Drawing.Point(103, 18);
            this.Abs63.Maximum = 100;
            this.Abs63.Name = "Abs63";
            this.Abs63.Size = new System.Drawing.Size(189, 19);
            this.Abs63.TabIndex = 16;
            this.Abs63.TickFrequency = 10;
            this.Abs63.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs63.Value = 1;
            this.Abs63.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label18, 2);
            this.label18.Location = new System.Drawing.Point(3, 2);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(214, 13);
            this.label18.TabIndex = 40;
            this.label18.Text = "Absorption Coefficients (% energy absorbed)";
            // 
            // AbsFlat
            // 
            this.AbsFlat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AbsFlat.BackColor = System.Drawing.SystemColors.HighlightText;
            this.AbsFlat.LargeChange = 10;
            this.AbsFlat.Location = new System.Drawing.Point(103, 218);
            this.AbsFlat.Maximum = 100;
            this.AbsFlat.Name = "AbsFlat";
            this.AbsFlat.Size = new System.Drawing.Size(189, 19);
            this.AbsFlat.TabIndex = 46;
            this.AbsFlat.TickFrequency = 10;
            this.AbsFlat.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.AbsFlat.ValueChanged += new System.EventHandler(this.AbsFlat_ValueChanged);
            // 
            // ScatFlat
            // 
            this.ScatFlat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScatFlat.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ScatFlat.LargeChange = 10;
            this.ScatFlat.Location = new System.Drawing.Point(103, 218);
            this.ScatFlat.Maximum = 100;
            this.ScatFlat.Name = "ScatFlat";
            this.ScatFlat.Size = new System.Drawing.Size(186, 19);
            this.ScatFlat.TabIndex = 37;
            this.ScatFlat.TickFrequency = 10;
            this.ScatFlat.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ScatFlat.Value = 1;
            this.ScatFlat.ValueChanged += new System.EventHandler(this.ScatFlat_ValueChanged);
            // 
            // Scat8kOut
            // 
            this.Scat8kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat8kOut.Location = new System.Drawing.Point(295, 193);
            this.Scat8kOut.Name = "Scat8kOut";
            this.Scat8kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat8kOut.Size = new System.Drawing.Size(54, 20);
            this.Scat8kOut.TabIndex = 45;
            this.Scat8kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat8kOut.ValueChanged += new System.EventHandler(this.Scat8kOut_ValueChanged);
            // 
            // Scat8kv
            // 
            this.Scat8kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat8kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat8kv.LargeChange = 10;
            this.Scat8kv.Location = new System.Drawing.Point(103, 193);
            this.Scat8kv.Maximum = 100;
            this.Scat8kv.Name = "Scat8kv";
            this.Scat8kv.Size = new System.Drawing.Size(186, 19);
            this.Scat8kv.TabIndex = 14;
            this.Scat8kv.TickFrequency = 10;
            this.Scat8kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat8kv.Value = 1;
            this.Scat8kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat4kv
            // 
            this.Scat4kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat4kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat4kv.LargeChange = 10;
            this.Scat4kv.Location = new System.Drawing.Point(103, 168);
            this.Scat4kv.Maximum = 100;
            this.Scat4kv.Name = "Scat4kv";
            this.Scat4kv.Size = new System.Drawing.Size(186, 19);
            this.Scat4kv.TabIndex = 12;
            this.Scat4kv.TickFrequency = 10;
            this.Scat4kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat4kv.Value = 1;
            this.Scat4kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat4kOut
            // 
            this.Scat4kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat4kOut.Location = new System.Drawing.Point(295, 168);
            this.Scat4kOut.Name = "Scat4kOut";
            this.Scat4kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat4kOut.Size = new System.Drawing.Size(54, 20);
            this.Scat4kOut.TabIndex = 44;
            this.Scat4kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat4kOut.ValueChanged += new System.EventHandler(this.Scat4kOut_ValueChanged);
            // 
            // Scat2kOut
            // 
            this.Scat2kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat2kOut.Location = new System.Drawing.Point(295, 143);
            this.Scat2kOut.Name = "Scat2kOut";
            this.Scat2kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat2kOut.Size = new System.Drawing.Size(54, 20);
            this.Scat2kOut.TabIndex = 43;
            this.Scat2kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat2kOut.ValueChanged += new System.EventHandler(this.Scat2kOut_ValueChanged);
            // 
            // Scat2kv
            // 
            this.Scat2kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat2kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat2kv.LargeChange = 10;
            this.Scat2kv.Location = new System.Drawing.Point(103, 143);
            this.Scat2kv.Maximum = 100;
            this.Scat2kv.Name = "Scat2kv";
            this.Scat2kv.Size = new System.Drawing.Size(186, 19);
            this.Scat2kv.TabIndex = 11;
            this.Scat2kv.TickFrequency = 10;
            this.Scat2kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat2kv.Value = 1;
            this.Scat2kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat1kv
            // 
            this.Scat1kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat1kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat1kv.LargeChange = 10;
            this.Scat1kv.Location = new System.Drawing.Point(103, 118);
            this.Scat1kv.Maximum = 100;
            this.Scat1kv.Name = "Scat1kv";
            this.Scat1kv.Size = new System.Drawing.Size(186, 19);
            this.Scat1kv.TabIndex = 10;
            this.Scat1kv.TickFrequency = 10;
            this.Scat1kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat1kv.Value = 1;
            this.Scat1kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat1kOut
            // 
            this.Scat1kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat1kOut.Location = new System.Drawing.Point(295, 118);
            this.Scat1kOut.Name = "Scat1kOut";
            this.Scat1kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat1kOut.Size = new System.Drawing.Size(54, 20);
            this.Scat1kOut.TabIndex = 42;
            this.Scat1kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat1kOut.ValueChanged += new System.EventHandler(this.Scat1kOut_ValueChanged);
            // 
            // Scat500Out
            // 
            this.Scat500Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat500Out.Location = new System.Drawing.Point(295, 93);
            this.Scat500Out.Name = "Scat500Out";
            this.Scat500Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat500Out.Size = new System.Drawing.Size(54, 20);
            this.Scat500Out.TabIndex = 41;
            this.Scat500Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat500Out.ValueChanged += new System.EventHandler(this.Scat500Out_ValueChanged);
            // 
            // Scat500v
            // 
            this.Scat500v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat500v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat500v.LargeChange = 10;
            this.Scat500v.Location = new System.Drawing.Point(103, 93);
            this.Scat500v.Maximum = 100;
            this.Scat500v.Name = "Scat500v";
            this.Scat500v.Size = new System.Drawing.Size(186, 19);
            this.Scat500v.TabIndex = 13;
            this.Scat500v.TickFrequency = 10;
            this.Scat500v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat500v.Value = 1;
            this.Scat500v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat250v
            // 
            this.Scat250v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat250v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat250v.LargeChange = 10;
            this.Scat250v.Location = new System.Drawing.Point(103, 68);
            this.Scat250v.Maximum = 100;
            this.Scat250v.Name = "Scat250v";
            this.Scat250v.Size = new System.Drawing.Size(186, 19);
            this.Scat250v.TabIndex = 9;
            this.Scat250v.TickFrequency = 10;
            this.Scat250v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat250v.Value = 1;
            this.Scat250v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat250Out
            // 
            this.Scat250Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat250Out.Location = new System.Drawing.Point(295, 68);
            this.Scat250Out.Name = "Scat250Out";
            this.Scat250Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat250Out.Size = new System.Drawing.Size(54, 20);
            this.Scat250Out.TabIndex = 40;
            this.Scat250Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat250Out.ValueChanged += new System.EventHandler(this.Scat250Out_ValueChanged);
            // 
            // Scat125Out
            // 
            this.Scat125Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat125Out.Location = new System.Drawing.Point(295, 43);
            this.Scat125Out.Name = "Scat125Out";
            this.Scat125Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat125Out.Size = new System.Drawing.Size(54, 20);
            this.Scat125Out.TabIndex = 39;
            this.Scat125Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat125Out.ValueChanged += new System.EventHandler(this.Scat125Out_ValueChanged);
            // 
            // Scat125v
            // 
            this.Scat125v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat125v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat125v.LargeChange = 10;
            this.Scat125v.Location = new System.Drawing.Point(103, 43);
            this.Scat125v.Maximum = 100;
            this.Scat125v.Name = "Scat125v";
            this.Scat125v.Size = new System.Drawing.Size(186, 19);
            this.Scat125v.TabIndex = 15;
            this.Scat125v.TickFrequency = 10;
            this.Scat125v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat125v.Value = 1;
            this.Scat125v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat63v
            // 
            this.Scat63v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat63v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Scat63v.LargeChange = 10;
            this.Scat63v.Location = new System.Drawing.Point(103, 18);
            this.Scat63v.Maximum = 100;
            this.Scat63v.Name = "Scat63v";
            this.Scat63v.Size = new System.Drawing.Size(186, 19);
            this.Scat63v.TabIndex = 16;
            this.Scat63v.TickFrequency = 10;
            this.Scat63v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Scat63v.Value = 1;
            this.Scat63v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Scat63Out
            // 
            this.Scat63Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat63Out.Location = new System.Drawing.Point(295, 18);
            this.Scat63Out.Name = "Scat63Out";
            this.Scat63Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat63Out.Size = new System.Drawing.Size(54, 20);
            this.Scat63Out.TabIndex = 38;
            this.Scat63Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat63Out.ValueChanged += new System.EventHandler(this.Scat63Out_ValueChanged);
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.tableLayoutPanel5.SetColumnSpan(this.label22, 3);
            this.label22.Location = new System.Drawing.Point(3, 2);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(273, 13);
            this.label22.TabIndex = 30;
            this.label22.Text = "Scattering Coefficients (% non-specular reflected energy)";
            // 
            // SmartMat_Display
            // 
            this.SmartMat_Display.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SmartMat_Display.BackColor = System.Drawing.Color.Transparent;
            customLabel1.FromPosition = 1D;
            customLabel1.Text = "0";
            customLabel1.ToPosition = 2D;
            customLabel2.FromPosition = 1D;
            customLabel2.Text = "30";
            customLabel2.ToPosition = 2D;
            customLabel3.FromPosition = 2D;
            customLabel3.Text = "60";
            customLabel3.ToPosition = 3D;
            customLabel4.FromPosition = 3D;
            customLabel4.Text = "90";
            customLabel4.ToPosition = 4D;
            customLabel10.FromPosition = 8D;
            customLabel10.Text = "-90";
            customLabel10.ToPosition = 9D;
            customLabel11.FromPosition = 9D;
            customLabel11.Text = "-60";
            customLabel11.ToPosition = 10D;
            customLabel12.FromPosition = 11D;
            customLabel12.Text = "-30";
            customLabel12.ToPosition = 12D;
            chartArea1.AxisX.CustomLabels.Add(customLabel1);
            chartArea1.AxisX.CustomLabels.Add(customLabel2);
            chartArea1.AxisX.CustomLabels.Add(customLabel3);
            chartArea1.AxisX.CustomLabels.Add(customLabel4);
            chartArea1.AxisX.CustomLabels.Add(customLabel5);
            chartArea1.AxisX.CustomLabels.Add(customLabel6);
            chartArea1.AxisX.CustomLabels.Add(customLabel7);
            chartArea1.AxisX.CustomLabels.Add(customLabel8);
            chartArea1.AxisX.CustomLabels.Add(customLabel9);
            chartArea1.AxisX.CustomLabels.Add(customLabel10);
            chartArea1.AxisX.CustomLabels.Add(customLabel11);
            chartArea1.AxisX.CustomLabels.Add(customLabel12);
            chartArea1.Name = "ChartArea1";
            this.SmartMat_Display.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Left;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Column;
            legend1.Name = "Legend1";
            legend1.Position.Auto = false;
            legend1.Position.Height = 25F;
            legend1.Position.Width = 20F;
            legend1.Position.X = 80F;
            legend1.Position.Y = 75F;
            this.SmartMat_Display.Legends.Add(legend1);
            this.SmartMat_Display.Location = new System.Drawing.Point(3, 6);
            this.SmartMat_Display.Name = "SmartMat_Display";
            this.SmartMat_Display.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.SmartMat_Display.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.Red,
        System.Drawing.Color.Orange,
        System.Drawing.Color.Gold,
        System.Drawing.Color.GreenYellow,
        System.Drawing.Color.Green,
        System.Drawing.Color.Blue,
        System.Drawing.Color.Indigo,
        System.Drawing.Color.Violet};
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series1.CustomProperties = "CircularLabelsStyle=Circular";
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.Name = "62.5 Hz.";
            series1.ShadowColor = System.Drawing.Color.DarkGray;
            series1.ShadowOffset = 3;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series2.Legend = "Legend1";
            series2.Name = "125 Hz.";
            series2.ShadowColor = System.Drawing.Color.DarkGray;
            series2.ShadowOffset = 3;
            series3.BorderWidth = 3;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series3.Legend = "Legend1";
            series3.Name = "250 Hz.";
            series3.ShadowColor = System.Drawing.Color.DarkGray;
            series3.ShadowOffset = 3;
            series4.BorderWidth = 3;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series4.Legend = "Legend1";
            series4.Name = "500 Hz.";
            series4.ShadowColor = System.Drawing.Color.DarkGray;
            series4.ShadowOffset = 3;
            series5.BorderWidth = 3;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series5.Legend = "Legend1";
            series5.Name = "1000 Hz.";
            series5.ShadowColor = System.Drawing.Color.DarkGray;
            series5.ShadowOffset = 3;
            series6.BorderWidth = 3;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series6.Legend = "Legend1";
            series6.Name = "2000 Hz.";
            series6.ShadowColor = System.Drawing.Color.DarkGray;
            series6.ShadowOffset = 3;
            series7.BorderWidth = 3;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series7.Legend = "Legend1";
            series7.Name = "4000 Hz.";
            series7.ShadowColor = System.Drawing.Color.DarkGray;
            series7.ShadowOffset = 3;
            series8.BorderWidth = 3;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series8.Legend = "Legend1";
            series8.Name = "8000 Hz.";
            series8.ShadowColor = System.Drawing.Color.DarkGray;
            series8.ShadowOffset = 3;
            this.SmartMat_Display.Series.Add(series1);
            this.SmartMat_Display.Series.Add(series2);
            this.SmartMat_Display.Series.Add(series3);
            this.SmartMat_Display.Series.Add(series4);
            this.SmartMat_Display.Series.Add(series5);
            this.SmartMat_Display.Series.Add(series6);
            this.SmartMat_Display.Series.Add(series7);
            this.SmartMat_Display.Series.Add(series8);
            this.SmartMat_Display.Size = new System.Drawing.Size(362, 488);
            this.SmartMat_Display.TabIndex = 45;
            this.SmartMat_Display.TabStop = false;
            this.SmartMat_Display.Text = "Absorption By Angle";
            // 
            // Calculate
            // 
            this.Calculate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Calculate.Location = new System.Drawing.Point(6, 386);
            this.Calculate.Margin = new System.Windows.Forms.Padding(2);
            this.Calculate.Name = "Calculate";
            this.Calculate.Size = new System.Drawing.Size(381, 23);
            this.Calculate.TabIndex = 15;
            this.Calculate.Text = "Calculate Solution";
            this.Calculate.UseVisualStyleBackColor = true;
            this.Calculate.Click += new System.EventHandler(this.Calculate_Click);
            // 
            // RTBox
            // 
            this.RTBox.AutoSize = true;
            this.RTBox.Checked = true;
            this.RTBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RTBox.Location = new System.Drawing.Point(6, 158);
            this.RTBox.Name = "RTBox";
            this.RTBox.Size = new System.Drawing.Size(118, 17);
            this.RTBox.TabIndex = 8;
            this.RTBox.Text = "Raytracing Solution";
            this.RTBox.UseVisualStyleBackColor = true;
            this.RTBox.CheckedChanged += new System.EventHandler(this.CalcType_CheckedChanged);
            // 
            // ISBox
            // 
            this.ISBox.AutoSize = true;
            this.ISBox.Checked = true;
            this.ISBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ISBox.Location = new System.Drawing.Point(6, 71);
            this.ISBox.Name = "ISBox";
            this.ISBox.Size = new System.Drawing.Size(133, 17);
            this.ISBox.TabIndex = 4;
            this.ISBox.Text = "Image Source Solution";
            this.ISBox.UseVisualStyleBackColor = true;
            this.ISBox.CheckedChanged += new System.EventHandler(this.CalcType_CheckedChanged);
            // 
            // Tabs
            // 
            this.Tabs.AccessibleDescription = "";
            this.Tabs.AccessibleName = "";
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.TabPage1);
            this.Tabs.Controls.Add(this.TabPage4);
            this.Tabs.Controls.Add(this.TabPage3);
            this.Tabs.Location = new System.Drawing.Point(3, 27);
            this.Tabs.MinimumSize = new System.Drawing.Size(400, 400);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(400, 596);
            this.Tabs.TabIndex = 5;
            this.Tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.Tab_Selecting);
            // 
            // TabPage1
            // 
            this.TabPage1.AutoScroll = true;
            this.TabPage1.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.TabPage1.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.TabPage1.Controls.Add(this.BTM_ED);
            this.TabPage1.Controls.Add(this.Specular_Trace);
            this.TabPage1.Controls.Add(this.Spec_RayCount);
            this.TabPage1.Controls.Add(this.GroupBox4);
            this.TabPage1.Controls.Add(this.GroupBox2);
            this.TabPage1.Controls.Add(this.COTime);
            this.TabPage1.Controls.Add(this.CO_TIME);
            this.TabPage1.Controls.Add(this.Label2);
            this.TabPage1.Controls.Add(this.Label1);
            this.TabPage1.Controls.Add(this.RT_Count);
            this.TabPage1.Controls.Add(this.Image_Order);
            this.TabPage1.Controls.Add(this.RTBox);
            this.TabPage1.Controls.Add(this.ISBox);
            this.TabPage1.Controls.Add(this.Calculate);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(392, 570);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Impulse";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // BTM_ED
            // 
            this.BTM_ED.AutoSize = true;
            this.BTM_ED.Location = new System.Drawing.Point(19, 112);
            this.BTM_ED.Name = "BTM_ED";
            this.BTM_ED.Size = new System.Drawing.Size(128, 17);
            this.BTM_ED.TabIndex = 34;
            this.BTM_ED.Text = "BTM Edge Diffraction";
            this.BTM_ED.UseVisualStyleBackColor = true;
            // 
            // Specular_Trace
            // 
            this.Specular_Trace.AutoSize = true;
            this.Specular_Trace.Enabled = false;
            this.Specular_Trace.Location = new System.Drawing.Point(19, 135);
            this.Specular_Trace.Name = "Specular_Trace";
            this.Specular_Trace.Size = new System.Drawing.Size(131, 17);
            this.Specular_Trace.TabIndex = 6;
            this.Specular_Trace.Text = "Image Source Tracing";
            this.Specular_Trace.UseVisualStyleBackColor = true;
            this.Specular_Trace.CheckedChanged += new System.EventHandler(this.CalcType_CheckedChanged);
            // 
            // Spec_RayCount
            // 
            this.Spec_RayCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Spec_RayCount.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Spec_RayCount.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Spec_RayCount.Location = new System.Drawing.Point(313, 135);
            this.Spec_RayCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.Spec_RayCount.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Spec_RayCount.Name = "Spec_RayCount";
            this.Spec_RayCount.Size = new System.Drawing.Size(67, 20);
            this.Spec_RayCount.TabIndex = 7;
            this.Spec_RayCount.Value = new decimal(new int[] {
            50000,
            0,
            0,
            0});
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
            this.GroupBox4.Controls.Add(this.Label3);
            this.GroupBox4.Controls.Add(this.Rel_Humidity);
            this.GroupBox4.Controls.Add(this.AirTemp);
            this.GroupBox4.Controls.Add(this.Air_Temp);
            this.GroupBox4.Location = new System.Drawing.Point(6, 233);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(380, 149);
            this.GroupBox4.TabIndex = 33;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Environmental Factors";
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
            this.Atten_Method.Size = new System.Drawing.Size(301, 21);
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
            this.Air_Pressure.Location = new System.Drawing.Point(307, 70);
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
            this.Rel_Humidity.Location = new System.Drawing.Point(339, 44);
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
            this.Air_Temp.Location = new System.Drawing.Point(339, 18);
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
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.Label17);
            this.GroupBox2.Controls.Add(this.ReceiverSelection);
            this.GroupBox2.Location = new System.Drawing.Point(6, 15);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(380, 50);
            this.GroupBox2.TabIndex = 23;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Geometry";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(2, 22);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(53, 13);
            this.Label17.TabIndex = 31;
            this.Label17.Text = "Receiver:";
            // 
            // ReceiverSelection
            // 
            this.ReceiverSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReceiverSelection.FormattingEnabled = true;
            this.ReceiverSelection.Items.AddRange(new object[] {
            "1 m. Stationary Receiver",
            "Expanding Receiver (Expanding)"});
            this.ReceiverSelection.Location = new System.Drawing.Point(61, 19);
            this.ReceiverSelection.Name = "ReceiverSelection";
            this.ReceiverSelection.Size = new System.Drawing.Size(313, 21);
            this.ReceiverSelection.TabIndex = 3;
            this.ReceiverSelection.Text = "1 m. Stationary Receiver";
            // 
            // COTime
            // 
            this.COTime.AutoSize = true;
            this.COTime.Location = new System.Drawing.Point(16, 208);
            this.COTime.Name = "COTime";
            this.COTime.Size = new System.Drawing.Size(88, 13);
            this.COTime.TabIndex = 17;
            this.COTime.Text = "Cut Off Time (ms)";
            // 
            // CO_TIME
            // 
            this.CO_TIME.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CO_TIME.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.CO_TIME.Location = new System.Drawing.Point(313, 206);
            this.CO_TIME.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.CO_TIME.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CO_TIME.Name = "CO_TIME";
            this.CO_TIME.Size = new System.Drawing.Size(64, 20);
            this.CO_TIME.TabIndex = 10;
            this.CO_TIME.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(16, 182);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(83, 13);
            this.Label2.TabIndex = 13;
            this.Label2.Text = "Number of Rays";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(16, 91);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(84, 13);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Reflection Order";
            // 
            // RT_Count
            // 
            this.RT_Count.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RT_Count.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.RT_Count.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RT_Count.Location = new System.Drawing.Point(313, 180);
            this.RT_Count.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.RT_Count.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RT_Count.Name = "RT_Count";
            this.RT_Count.Size = new System.Drawing.Size(64, 20);
            this.RT_Count.TabIndex = 9;
            this.RT_Count.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Image_Order
            // 
            this.Image_Order.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Image_Order.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Image_Order.Location = new System.Drawing.Point(335, 89);
            this.Image_Order.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.Image_Order.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Image_Order.Name = "Image_Order";
            this.Image_Order.Size = new System.Drawing.Size(45, 20);
            this.Image_Order.TabIndex = 5;
            this.Image_Order.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TabPage4
            // 
            this.TabPage4.Controls.Add(this.tabControl1);
            this.TabPage4.Controls.Add(this.Abs_Designer);
            this.TabPage4.Controls.Add(this.LayerLbl);
            this.TabPage4.Controls.Add(this.groupBox5);
            this.TabPage4.Controls.Add(this.LayerDisplay);
            this.TabPage4.Controls.Add(this.Material_Lib);
            this.TabPage4.Controls.Add(this.Mat_Lbl);
            this.TabPage4.Location = new System.Drawing.Point(4, 22);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage4.Size = new System.Drawing.Size(392, 570);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Materials";
            this.TabPage4.UseVisualStyleBackColor = true;
            this.TabPage4.MouseEnter += new System.EventHandler(this.Materials_MouseEnter);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.Absorption);
            this.tabControl1.Controls.Add(this.Scattering);
            this.tabControl1.Controls.Add(this.Transparency);
            this.tabControl1.Location = new System.Drawing.Point(4, 135);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(382, 320);
            this.tabControl1.TabIndex = 47;
            // 
            // Absorption
            // 
            this.Absorption.Controls.Add(this.tableLayoutPanel4);
            this.Absorption.Controls.Add(this.SmartMat_Display);
            this.Absorption.Location = new System.Drawing.Point(4, 22);
            this.Absorption.Name = "Absorption";
            this.Absorption.Padding = new System.Windows.Forms.Padding(3);
            this.Absorption.Size = new System.Drawing.Size(374, 294);
            this.Absorption.TabIndex = 0;
            this.Absorption.Text = "Absorption";
            this.Absorption.UseVisualStyleBackColor = true;
            // 
            // Scattering
            // 
            this.Scattering.Controls.Add(this.tableLayoutPanel5);
            this.Scattering.Location = new System.Drawing.Point(4, 22);
            this.Scattering.Name = "Scattering";
            this.Scattering.Padding = new System.Windows.Forms.Padding(3);
            this.Scattering.Size = new System.Drawing.Size(374, 294);
            this.Scattering.TabIndex = 1;
            this.Scattering.Text = "Scattering";
            this.Scattering.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.Controls.Add(this.label37, 0, 9);
            this.tableLayoutPanel5.Controls.Add(this.ScatFlat, 1, 9);
            this.tableLayoutPanel5.Controls.Add(this.label38, 0, 8);
            this.tableLayoutPanel5.Controls.Add(this.Scat8kOut, 2, 8);
            this.tableLayoutPanel5.Controls.Add(this.label39, 0, 7);
            this.tableLayoutPanel5.Controls.Add(this.Scat8kv, 1, 8);
            this.tableLayoutPanel5.Controls.Add(this.label40, 0, 6);
            this.tableLayoutPanel5.Controls.Add(this.label41, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.label42, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.Scat4kOut, 2, 7);
            this.tableLayoutPanel5.Controls.Add(this.Scat4kv, 1, 7);
            this.tableLayoutPanel5.Controls.Add(this.label43, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.label44, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.Scat2kOut, 2, 6);
            this.tableLayoutPanel5.Controls.Add(this.label45, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.Scat2kv, 1, 6);
            this.tableLayoutPanel5.Controls.Add(this.label22, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.Scat63v, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.Scat63Out, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.Scat1kOut, 2, 5);
            this.tableLayoutPanel5.Controls.Add(this.Scat1kv, 1, 5);
            this.tableLayoutPanel5.Controls.Add(this.Scat125Out, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.Scat125v, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.Scat500Out, 2, 4);
            this.tableLayoutPanel5.Controls.Add(this.Scat250v, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.Scat500v, 1, 4);
            this.tableLayoutPanel5.Controls.Add(this.Scat250Out, 2, 3);
            this.tableLayoutPanel5.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 11;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(362, 282);
            this.tableLayoutPanel5.TabIndex = 46;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(3, 215);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(53, 25);
            this.label37.TabIndex = 38;
            this.label37.Text = "Flatten All";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(3, 190);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(35, 25);
            this.label38.TabIndex = 24;
            this.label38.Text = "8 kHz";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label39
            // 
            this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(3, 165);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(35, 25);
            this.label39.TabIndex = 22;
            this.label39.Text = "4 kHz";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(3, 140);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(35, 25);
            this.label40.TabIndex = 21;
            this.label40.Text = "2 kHz";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(3, 115);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(35, 25);
            this.label41.TabIndex = 20;
            this.label41.Text = "1 kHz";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label42
            // 
            this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(3, 90);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(41, 25);
            this.label42.TabIndex = 19;
            this.label42.Text = "500 Hz";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(3, 65);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(41, 25);
            this.label43.TabIndex = 25;
            this.label43.Text = "250 Hz";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label44
            // 
            this.label44.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(3, 40);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(41, 25);
            this.label44.TabIndex = 18;
            this.label44.Text = "125 Hz";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(3, 15);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(44, 25);
            this.label45.TabIndex = 17;
            this.label45.Text = "62.5 Hz";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Transparency
            // 
            this.Transparency.Controls.Add(this.tableLayoutPanel6);
            this.Transparency.Location = new System.Drawing.Point(4, 22);
            this.Transparency.Name = "Transparency";
            this.Transparency.Size = new System.Drawing.Size(374, 294);
            this.Transparency.TabIndex = 2;
            this.Transparency.Text = "Transparency";
            this.Transparency.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel6.Controls.Add(this.Trans_Flat, 1, 9);
            this.tableLayoutPanel6.Controls.Add(this.Trans_8k_Out, 2, 8);
            this.tableLayoutPanel6.Controls.Add(this.label29, 0, 7);
            this.tableLayoutPanel6.Controls.Add(this.Trans_8kv, 1, 8);
            this.tableLayoutPanel6.Controls.Add(this.label30, 0, 6);
            this.tableLayoutPanel6.Controls.Add(this.label31, 0, 5);
            this.tableLayoutPanel6.Controls.Add(this.label32, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.Trans_4k_Out, 2, 7);
            this.tableLayoutPanel6.Controls.Add(this.Trans_4kv, 1, 7);
            this.tableLayoutPanel6.Controls.Add(this.label33, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.label34, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.Trans_2k_Out, 2, 6);
            this.tableLayoutPanel6.Controls.Add(this.label35, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.Trans_2kv, 1, 6);
            this.tableLayoutPanel6.Controls.Add(this.label36, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.Trans_63v, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.Trans_63_Out, 2, 1);
            this.tableLayoutPanel6.Controls.Add(this.Trans_1k_Out, 2, 5);
            this.tableLayoutPanel6.Controls.Add(this.Trans_1kv, 1, 5);
            this.tableLayoutPanel6.Controls.Add(this.Trans_125_Out, 2, 2);
            this.tableLayoutPanel6.Controls.Add(this.Trans_125v, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.Trans_500_Out, 2, 4);
            this.tableLayoutPanel6.Controls.Add(this.Trans_250v, 1, 3);
            this.tableLayoutPanel6.Controls.Add(this.Trans_500v, 1, 4);
            this.tableLayoutPanel6.Controls.Add(this.Trans_250_Out, 2, 3);
            this.tableLayoutPanel6.Controls.Add(this.Trans_Check, 1, 10);
            this.tableLayoutPanel6.Controls.Add(this.label28, 0, 8);
            this.tableLayoutPanel6.Controls.Add(this.label23, 0, 9);
            this.tableLayoutPanel6.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 11;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(362, 282);
            this.tableLayoutPanel6.TabIndex = 47;
            // 
            // Trans_Flat
            // 
            this.Trans_Flat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_Flat.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_Flat.Enabled = false;
            this.Trans_Flat.LargeChange = 10;
            this.Trans_Flat.Location = new System.Drawing.Point(103, 218);
            this.Trans_Flat.Maximum = 100;
            this.Trans_Flat.Name = "Trans_Flat";
            this.Trans_Flat.Size = new System.Drawing.Size(186, 19);
            this.Trans_Flat.TabIndex = 37;
            this.Trans_Flat.TickFrequency = 10;
            this.Trans_Flat.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_Flat.ValueChanged += new System.EventHandler(this.TransFlat_ValueChanged);
            // 
            // Trans_8k_Out
            // 
            this.Trans_8k_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_8k_Out.Enabled = false;
            this.Trans_8k_Out.Location = new System.Drawing.Point(295, 193);
            this.Trans_8k_Out.Name = "Trans_8k_Out";
            this.Trans_8k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_8k_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_8k_Out.TabIndex = 45;
            this.Trans_8k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(3, 165);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(35, 25);
            this.label29.TabIndex = 22;
            this.label29.Text = "4 kHz";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Trans_8kv
            // 
            this.Trans_8kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_8kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_8kv.Enabled = false;
            this.Trans_8kv.LargeChange = 10;
            this.Trans_8kv.Location = new System.Drawing.Point(103, 193);
            this.Trans_8kv.Maximum = 100;
            this.Trans_8kv.Name = "Trans_8kv";
            this.Trans_8kv.Size = new System.Drawing.Size(186, 19);
            this.Trans_8kv.TabIndex = 14;
            this.Trans_8kv.TickFrequency = 10;
            this.Trans_8kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_8kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(3, 140);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(35, 25);
            this.label30.TabIndex = 21;
            this.label30.Text = "2 kHz";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(3, 115);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(35, 25);
            this.label31.TabIndex = 20;
            this.label31.Text = "1 kHz";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(3, 90);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(41, 25);
            this.label32.TabIndex = 19;
            this.label32.Text = "500 Hz";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Trans_4k_Out
            // 
            this.Trans_4k_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_4k_Out.Enabled = false;
            this.Trans_4k_Out.Location = new System.Drawing.Point(295, 168);
            this.Trans_4k_Out.Name = "Trans_4k_Out";
            this.Trans_4k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_4k_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_4k_Out.TabIndex = 44;
            this.Trans_4k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Trans_4kv
            // 
            this.Trans_4kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_4kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_4kv.Enabled = false;
            this.Trans_4kv.LargeChange = 10;
            this.Trans_4kv.Location = new System.Drawing.Point(103, 168);
            this.Trans_4kv.Maximum = 100;
            this.Trans_4kv.Name = "Trans_4kv";
            this.Trans_4kv.Size = new System.Drawing.Size(186, 19);
            this.Trans_4kv.TabIndex = 12;
            this.Trans_4kv.TickFrequency = 10;
            this.Trans_4kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_4kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(3, 65);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(41, 25);
            this.label33.TabIndex = 25;
            this.label33.Text = "250 Hz";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(3, 40);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(41, 25);
            this.label34.TabIndex = 18;
            this.label34.Text = "125 Hz";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Trans_2k_Out
            // 
            this.Trans_2k_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_2k_Out.Enabled = false;
            this.Trans_2k_Out.Location = new System.Drawing.Point(295, 143);
            this.Trans_2k_Out.Name = "Trans_2k_Out";
            this.Trans_2k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_2k_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_2k_Out.TabIndex = 43;
            this.Trans_2k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(3, 15);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(44, 25);
            this.label35.TabIndex = 17;
            this.label35.Text = "62.5 Hz";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Trans_2kv
            // 
            this.Trans_2kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_2kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_2kv.Enabled = false;
            this.Trans_2kv.LargeChange = 10;
            this.Trans_2kv.Location = new System.Drawing.Point(103, 143);
            this.Trans_2kv.Maximum = 100;
            this.Trans_2kv.Name = "Trans_2kv";
            this.Trans_2kv.Size = new System.Drawing.Size(186, 19);
            this.Trans_2kv.TabIndex = 11;
            this.Trans_2kv.TickFrequency = 10;
            this.Trans_2kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_2kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label36.AutoSize = true;
            this.tableLayoutPanel6.SetColumnSpan(this.label36, 3);
            this.label36.Location = new System.Drawing.Point(3, 2);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(303, 13);
            this.label36.TabIndex = 30;
            this.label36.Text = "Transmission Coefficients (%  non-absorbed transmitted energy)";
            // 
            // Trans_63v
            // 
            this.Trans_63v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_63v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_63v.Enabled = false;
            this.Trans_63v.LargeChange = 10;
            this.Trans_63v.Location = new System.Drawing.Point(103, 18);
            this.Trans_63v.Maximum = 100;
            this.Trans_63v.Name = "Trans_63v";
            this.Trans_63v.Size = new System.Drawing.Size(186, 19);
            this.Trans_63v.TabIndex = 16;
            this.Trans_63v.TickFrequency = 10;
            this.Trans_63v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_63v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_63_Out
            // 
            this.Trans_63_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_63_Out.Enabled = false;
            this.Trans_63_Out.Location = new System.Drawing.Point(295, 18);
            this.Trans_63_Out.Name = "Trans_63_Out";
            this.Trans_63_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_63_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_63_Out.TabIndex = 38;
            this.Trans_63_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Trans_1k_Out
            // 
            this.Trans_1k_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_1k_Out.Enabled = false;
            this.Trans_1k_Out.Location = new System.Drawing.Point(295, 118);
            this.Trans_1k_Out.Name = "Trans_1k_Out";
            this.Trans_1k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_1k_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_1k_Out.TabIndex = 42;
            this.Trans_1k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Trans_1kv
            // 
            this.Trans_1kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_1kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_1kv.Enabled = false;
            this.Trans_1kv.LargeChange = 10;
            this.Trans_1kv.Location = new System.Drawing.Point(103, 118);
            this.Trans_1kv.Maximum = 100;
            this.Trans_1kv.Name = "Trans_1kv";
            this.Trans_1kv.Size = new System.Drawing.Size(186, 19);
            this.Trans_1kv.TabIndex = 10;
            this.Trans_1kv.TickFrequency = 10;
            this.Trans_1kv.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_1kv.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_125_Out
            // 
            this.Trans_125_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_125_Out.Enabled = false;
            this.Trans_125_Out.Location = new System.Drawing.Point(295, 43);
            this.Trans_125_Out.Name = "Trans_125_Out";
            this.Trans_125_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_125_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_125_Out.TabIndex = 39;
            this.Trans_125_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Trans_125v
            // 
            this.Trans_125v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_125v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_125v.Enabled = false;
            this.Trans_125v.LargeChange = 10;
            this.Trans_125v.Location = new System.Drawing.Point(103, 43);
            this.Trans_125v.Maximum = 100;
            this.Trans_125v.Name = "Trans_125v";
            this.Trans_125v.Size = new System.Drawing.Size(186, 19);
            this.Trans_125v.TabIndex = 15;
            this.Trans_125v.TickFrequency = 10;
            this.Trans_125v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_125v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_500_Out
            // 
            this.Trans_500_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_500_Out.Enabled = false;
            this.Trans_500_Out.Location = new System.Drawing.Point(295, 93);
            this.Trans_500_Out.Name = "Trans_500_Out";
            this.Trans_500_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_500_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_500_Out.TabIndex = 41;
            this.Trans_500_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Trans_250v
            // 
            this.Trans_250v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_250v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_250v.Enabled = false;
            this.Trans_250v.LargeChange = 10;
            this.Trans_250v.Location = new System.Drawing.Point(103, 68);
            this.Trans_250v.Maximum = 100;
            this.Trans_250v.Name = "Trans_250v";
            this.Trans_250v.Size = new System.Drawing.Size(186, 19);
            this.Trans_250v.TabIndex = 9;
            this.Trans_250v.TickFrequency = 10;
            this.Trans_250v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_250v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_500v
            // 
            this.Trans_500v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_500v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_500v.Enabled = false;
            this.Trans_500v.LargeChange = 10;
            this.Trans_500v.Location = new System.Drawing.Point(103, 93);
            this.Trans_500v.Maximum = 100;
            this.Trans_500v.Name = "Trans_500v";
            this.Trans_500v.Size = new System.Drawing.Size(186, 19);
            this.Trans_500v.TabIndex = 13;
            this.Trans_500v.TickFrequency = 10;
            this.Trans_500v.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Trans_500v.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_250_Out
            // 
            this.Trans_250_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_250_Out.Enabled = false;
            this.Trans_250_Out.Location = new System.Drawing.Point(295, 68);
            this.Trans_250_Out.Name = "Trans_250_Out";
            this.Trans_250_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_250_Out.Size = new System.Drawing.Size(54, 20);
            this.Trans_250_Out.TabIndex = 40;
            this.Trans_250_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Trans_Check
            // 
            this.Trans_Check.AutoSize = true;
            this.Trans_Check.Dock = System.Windows.Forms.DockStyle.Top;
            this.Trans_Check.Location = new System.Drawing.Point(103, 243);
            this.Trans_Check.Name = "Trans_Check";
            this.Trans_Check.Size = new System.Drawing.Size(186, 17);
            this.Trans_Check.TabIndex = 46;
            this.Trans_Check.Text = "Semi-Transparent Material";
            this.Trans_Check.UseVisualStyleBackColor = true;
            this.Trans_Check.CheckedChanged += new System.EventHandler(this.Trans_Check_CheckedChanged);
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(3, 190);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(35, 25);
            this.label28.TabIndex = 24;
            this.label28.Text = "8 kHz";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(3, 215);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(53, 25);
            this.label23.TabIndex = 38;
            this.label23.Text = "Flatten All";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Abs_Designer
            // 
            this.Abs_Designer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs_Designer.Location = new System.Drawing.Point(6, 106);
            this.Abs_Designer.Name = "Abs_Designer";
            this.Abs_Designer.Size = new System.Drawing.Size(373, 23);
            this.Abs_Designer.TabIndex = 2;
            this.Abs_Designer.Text = "Call Absorption Designer";
            this.Abs_Designer.UseVisualStyleBackColor = true;
            this.Abs_Designer.Click += new System.EventHandler(this.Abs_Designer_Click);
            // 
            // LayerLbl
            // 
            this.LayerLbl.AutoSize = true;
            this.LayerLbl.Location = new System.Drawing.Point(3, 3);
            this.LayerLbl.Name = "LayerLbl";
            this.LayerLbl.Size = new System.Drawing.Size(54, 13);
            this.LayerLbl.TabIndex = 6;
            this.LayerLbl.Text = "For Layer:";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.SaveAbs);
            this.groupBox5.Controls.Add(this.Material_Name);
            this.groupBox5.Location = new System.Drawing.Point(189, 27);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(190, 72);
            this.groupBox5.TabIndex = 28;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Save Material Absorption";
            // 
            // SaveAbs
            // 
            this.SaveAbs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveAbs.Location = new System.Drawing.Point(6, 44);
            this.SaveAbs.Name = "SaveAbs";
            this.SaveAbs.Size = new System.Drawing.Size(178, 23);
            this.SaveAbs.TabIndex = 1;
            this.SaveAbs.Text = "Save Material";
            this.SaveAbs.UseVisualStyleBackColor = true;
            this.SaveAbs.Click += new System.EventHandler(this.SaveAbs_Click);
            // 
            // Material_Name
            // 
            this.Material_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Material_Name.Location = new System.Drawing.Point(6, 18);
            this.Material_Name.Name = "Material_Name";
            this.Material_Name.Size = new System.Drawing.Size(178, 20);
            this.Material_Name.TabIndex = 0;
            // 
            // LayerDisplay
            // 
            this.LayerDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LayerDisplay.FormattingEnabled = true;
            this.LayerDisplay.Location = new System.Drawing.Point(63, 3);
            this.LayerDisplay.MaxDropDownItems = 100;
            this.LayerDisplay.Name = "LayerDisplay";
            this.LayerDisplay.Size = new System.Drawing.Size(316, 21);
            this.LayerDisplay.TabIndex = 27;
            this.LayerDisplay.SelectedValueChanged += new System.EventHandler(this.Retrieve_Layer_Acoustics);
            // 
            // Material_Lib
            // 
            this.Material_Lib.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Material_Lib.FormattingEnabled = true;
            this.Material_Lib.Location = new System.Drawing.Point(6, 43);
            this.Material_Lib.Name = "Material_Lib";
            this.Material_Lib.ScrollAlwaysVisible = true;
            this.Material_Lib.Size = new System.Drawing.Size(177, 56);
            this.Material_Lib.TabIndex = 8;
            this.Material_Lib.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Material_Lib_MouseClick);
            // 
            // Mat_Lbl
            // 
            this.Mat_Lbl.AutoSize = true;
            this.Mat_Lbl.Location = new System.Drawing.Point(6, 27);
            this.Mat_Lbl.Name = "Mat_Lbl";
            this.Mat_Lbl.Size = new System.Drawing.Size(81, 13);
            this.Mat_Lbl.TabIndex = 7;
            this.Mat_Lbl.Text = "Material Library:";
            // 
            // TabPage3
            // 
            this.TabPage3.AutoScroll = true;
            this.TabPage3.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.TabPage3.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.TabPage3.Controls.Add(this.Source_Aim);
            this.TabPage3.Controls.Add(this.label27);
            this.TabPage3.Controls.Add(this.label26);
            this.TabPage3.Controls.Add(this.Alt_Choice);
            this.TabPage3.Controls.Add(this.Azi_Choice);
            this.TabPage3.Controls.Add(this.label25);
            this.TabPage3.Controls.Add(this.label15);
            this.TabPage3.Controls.Add(this.SourceList);
            this.TabPage3.Controls.Add(this.tableLayoutPanel3);
            this.TabPage3.Controls.Add(this.label20);
            this.TabPage3.Controls.Add(this.Receiver_Choice);
            this.TabPage3.Controls.Add(this.PathCount);
            this.TabPage3.Controls.Add(this.GroupBox3);
            this.TabPage3.Controls.Add(this.Label5);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(392, 570);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Analysis";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // Source_Aim
            // 
            this.Source_Aim.FormattingEnabled = true;
            this.Source_Aim.Location = new System.Drawing.Point(85, 149);
            this.Source_Aim.Name = "Source_Aim";
            this.Source_Aim.Size = new System.Drawing.Size(53, 21);
            this.Source_Aim.TabIndex = 51;
            this.Source_Aim.Text = "None";
            this.Source_Aim.SelectedIndexChanged += new System.EventHandler(this.Source_Aim_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Enabled = false;
            this.label27.Location = new System.Drawing.Point(160, 152);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(42, 13);
            this.label27.TabIndex = 50;
            this.label27.Text = "Altitude";
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Enabled = false;
            this.label26.Location = new System.Drawing.Point(268, 152);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(44, 13);
            this.label26.TabIndex = 49;
            this.label26.Text = "Azimuth";
            // 
            // Alt_Choice
            // 
            this.Alt_Choice.DecimalPlaces = 2;
            this.Alt_Choice.Location = new System.Drawing.Point(208, 150);
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
            this.Alt_Choice.Size = new System.Drawing.Size(60, 20);
            this.Alt_Choice.TabIndex = 48;
            this.Alt_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Alt_Choice.ValueChanged += new System.EventHandler(this.Alt_Choice_ValueChanged);
            // 
            // Azi_Choice
            // 
            this.Azi_Choice.DecimalPlaces = 2;
            this.Azi_Choice.Location = new System.Drawing.Point(318, 150);
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
            this.Azi_Choice.Size = new System.Drawing.Size(60, 20);
            this.Azi_Choice.TabIndex = 47;
            this.Azi_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Azi_Choice.ValueChanged += new System.EventHandler(this.Azi_Choice_ValueChanged);
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Enabled = false;
            this.label25.Location = new System.Drawing.Point(6, 152);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(73, 13);
            this.label25.TabIndex = 46;
            this.label25.Text = "Aim at Source";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Enabled = false;
            this.label15.Location = new System.Drawing.Point(4, 6);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 45;
            this.label15.Text = "Source";
            // 
            // SourceList
            // 
            this.SourceList.CheckOnClick = true;
            this.SourceList.FormattingEnabled = true;
            this.SourceList.Location = new System.Drawing.Point(13, 22);
            this.SourceList.MinimumSize = new System.Drawing.Size(4, 64);
            this.SourceList.Name = "SourceList";
            this.SourceList.ScrollAlwaysVisible = true;
            this.SourceList.Size = new System.Drawing.Size(125, 94);
            this.SourceList.TabIndex = 44;
            this.SourceList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SourceList_MouseUp);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.72973F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.27027F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel3.Controls.Add(this.IS_Path_Box, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.Analysis_View, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.Normalize_Graph, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.LockUserScale, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.Graph_Octave, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.Graph_Type, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.Auralisation, 0, 4);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 189);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(377, 375);
            this.tableLayoutPanel3.TabIndex = 43;
            // 
            // IS_Path_Box
            // 
            this.IS_Path_Box.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IS_Path_Box.CheckOnClick = true;
            this.tableLayoutPanel3.SetColumnSpan(this.IS_Path_Box, 4);
            this.IS_Path_Box.FormattingEnabled = true;
            this.IS_Path_Box.Location = new System.Drawing.Point(3, 3);
            this.IS_Path_Box.MinimumSize = new System.Drawing.Size(4, 64);
            this.IS_Path_Box.Name = "IS_Path_Box";
            this.IS_Path_Box.ScrollAlwaysVisible = true;
            this.IS_Path_Box.Size = new System.Drawing.Size(371, 94);
            this.IS_Path_Box.TabIndex = 7;
            this.IS_Path_Box.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IS_Path_Box_MouseUp);
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
            this.Analysis_View.Location = new System.Drawing.Point(3, 153);
            this.Analysis_View.Name = "Analysis_View";
            this.Analysis_View.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.Analysis_View.ScrollGrace = 0D;
            this.Analysis_View.ScrollMaxX = 0D;
            this.Analysis_View.ScrollMaxY = 0D;
            this.Analysis_View.ScrollMaxY2 = 0D;
            this.Analysis_View.ScrollMinX = 0D;
            this.Analysis_View.ScrollMinY = 0D;
            this.Analysis_View.ScrollMinY2 = 0D;
            this.Analysis_View.Size = new System.Drawing.Size(371, 193);
            this.Analysis_View.TabIndex = 42;
            // 
            // Normalize_Graph
            // 
            this.Normalize_Graph.AutoSize = true;
            this.Normalize_Graph.Checked = true;
            this.Normalize_Graph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Normalize_Graph.Location = new System.Drawing.Point(142, 128);
            this.Normalize_Graph.Name = "Normalize_Graph";
            this.Normalize_Graph.Size = new System.Drawing.Size(114, 17);
            this.Normalize_Graph.TabIndex = 43;
            this.Normalize_Graph.Text = "Normalize To Direct";
            this.Normalize_Graph.UseVisualStyleBackColor = true;
            this.Normalize_Graph.CheckedChanged += new System.EventHandler(this.Normalize_Graph_CheckedChanged);
            // 
            // LockUserScale
            // 
            this.LockUserScale.AutoSize = true;
            this.LockUserScale.Location = new System.Drawing.Point(24, 128);
            this.LockUserScale.Name = "LockUserScale";
            this.LockUserScale.Size = new System.Drawing.Size(105, 17);
            this.LockUserScale.TabIndex = 44;
            this.LockUserScale.Text = "Lock User Scale";
            this.LockUserScale.UseVisualStyleBackColor = true;
            this.LockUserScale.CheckedChanged += new System.EventHandler(this.Update_Graph);
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
            this.Graph_Octave.Location = new System.Drawing.Point(142, 103);
            this.Graph_Octave.Name = "Graph_Octave";
            this.Graph_Octave.Size = new System.Drawing.Size(232, 21);
            this.Graph_Octave.TabIndex = 33;
            this.Graph_Octave.Text = "Summation: All Octaves";
            this.Graph_Octave.TextChanged += new System.EventHandler(this.Update_Graph);
            // 
            // Graph_Type
            // 
            this.Graph_Type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Graph_Type, 2);
            this.Graph_Type.FormattingEnabled = true;
            this.Graph_Type.Items.AddRange(new object[] {
            "Energy Time Curve",
            "Pressure Time Curve",
            "Lateral ETC",
            "Lateral PTC",
            "Vertical ETC",
            "Vertical PTC",
            "Fore-Aft ETC",
            "Fore-Aft PTC"});
            this.Graph_Type.Location = new System.Drawing.Point(3, 103);
            this.Graph_Type.Name = "Graph_Type";
            this.Graph_Type.Size = new System.Drawing.Size(133, 21);
            this.Graph_Type.TabIndex = 33;
            this.Graph_Type.Text = "Energy Time Curve";
            this.Graph_Type.TextChanged += new System.EventHandler(this.Update_Graph);
            // 
            // Auralisation
            // 
            this.Auralisation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Auralisation, 4);
            this.Auralisation.Location = new System.Drawing.Point(3, 352);
            this.Auralisation.Name = "Auralisation";
            this.Auralisation.Size = new System.Drawing.Size(371, 20);
            this.Auralisation.TabIndex = 45;
            this.Auralisation.Text = "Go To Auralizations";
            this.Auralisation.UseVisualStyleBackColor = true;
            this.Auralisation.Click += new System.EventHandler(this.Auralisation_Click);
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Enabled = false;
            this.label20.Location = new System.Drawing.Point(4, 123);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 13);
            this.label20.TabIndex = 40;
            this.label20.Text = "Receiver";
            // 
            // Receiver_Choice
            // 
            this.Receiver_Choice.FormattingEnabled = true;
            this.Receiver_Choice.Location = new System.Drawing.Point(60, 123);
            this.Receiver_Choice.Name = "Receiver_Choice";
            this.Receiver_Choice.Size = new System.Drawing.Size(78, 21);
            this.Receiver_Choice.TabIndex = 39;
            this.Receiver_Choice.Text = "No Results Calculated...";
            this.Receiver_Choice.SelectedIndexChanged += new System.EventHandler(this.Receiver_Choice_SelectedIndexChanged);
            // 
            // PathCount
            // 
            this.PathCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PathCount.AutoSize = true;
            this.PathCount.Enabled = false;
            this.PathCount.Location = new System.Drawing.Point(315, 173);
            this.PathCount.Name = "PathCount";
            this.PathCount.Size = new System.Drawing.Size(55, 13);
            this.PathCount.TabIndex = 8;
            this.PathCount.Text = "Pending...";
            this.PathCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox3.Controls.Add(this.tableLayoutPanel1);
            this.GroupBox3.Location = new System.Drawing.Point(144, 6);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(242, 138);
            this.GroupBox3.TabIndex = 6;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Parametric Analysis";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.SRT8, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.Parameter_Choice, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SRT7, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.SRT6, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.SRT2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.SRT5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.SRT3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.SRT4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.SRT1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ISOCOMP, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(242, 108);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // SRT8
            // 
            this.SRT8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT8.AutoSize = true;
            this.SRT8.Location = new System.Drawing.Point(124, 88);
            this.SRT8.Name = "SRT8";
            this.SRT8.Size = new System.Drawing.Size(115, 20);
            this.SRT8.TabIndex = 15;
            this.SRT8.Text = "8000 hz:";
            // 
            // Parameter_Choice
            // 
            this.Parameter_Choice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Parameter_Choice.FormattingEnabled = true;
            this.Parameter_Choice.Items.AddRange(new object[] {
            "Early Decay Time",
            "T-15",
            "T-30",
            "Center Time (TS)",
            "Clarity (C-50)",
            "Clarity (C-80)",
            "Definition (D-50)",
            "Strength/Loudness (G)",
            "Sound Pressure Level (SPL)",
            "Initial Time Delay Gap (ITDG)",
            "Speech Transmission Index (Explicit)",
            "Modulation Transfer Index (MTI - root STI)",
            "Lateral Fraction (LF)",
            "Lateral Efficiency (LE)",
            "Echo Criterion (Music, 10%)",
            "Echo Criterion (Music, 50%)",
            "Echo Criterion (Speech, 10%)",
            "Echo Criterion (Speech, 50%)"});
            this.Parameter_Choice.Location = new System.Drawing.Point(3, 3);
            this.Parameter_Choice.Name = "Parameter_Choice";
            this.Parameter_Choice.Size = new System.Drawing.Size(115, 21);
            this.Parameter_Choice.TabIndex = 27;
            this.Parameter_Choice.Text = "Select Parameter...";
            this.Parameter_Choice.TextChanged += new System.EventHandler(this.Parameter_Choice_SelectedIndexChanged);
            // 
            // SRT7
            // 
            this.SRT7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT7.AutoSize = true;
            this.SRT7.Location = new System.Drawing.Point(124, 68);
            this.SRT7.Name = "SRT7";
            this.SRT7.Size = new System.Drawing.Size(115, 20);
            this.SRT7.TabIndex = 14;
            this.SRT7.Text = "4000 hz:";
            // 
            // SRT6
            // 
            this.SRT6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT6.AutoSize = true;
            this.SRT6.Location = new System.Drawing.Point(124, 48);
            this.SRT6.Name = "SRT6";
            this.SRT6.Size = new System.Drawing.Size(115, 20);
            this.SRT6.TabIndex = 13;
            this.SRT6.Text = "2000 hz:";
            // 
            // SRT2
            // 
            this.SRT2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT2.AutoSize = true;
            this.SRT2.Location = new System.Drawing.Point(3, 48);
            this.SRT2.Name = "SRT2";
            this.SRT2.Size = new System.Drawing.Size(115, 20);
            this.SRT2.TabIndex = 9;
            this.SRT2.Text = "125 hz:";
            // 
            // SRT5
            // 
            this.SRT5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT5.AutoSize = true;
            this.SRT5.Location = new System.Drawing.Point(124, 28);
            this.SRT5.Name = "SRT5";
            this.SRT5.Size = new System.Drawing.Size(115, 20);
            this.SRT5.TabIndex = 12;
            this.SRT5.Text = "1000 hz:";
            // 
            // SRT3
            // 
            this.SRT3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT3.AutoSize = true;
            this.SRT3.Location = new System.Drawing.Point(3, 68);
            this.SRT3.Name = "SRT3";
            this.SRT3.Size = new System.Drawing.Size(115, 20);
            this.SRT3.TabIndex = 10;
            this.SRT3.Text = "250 hz:";
            // 
            // SRT4
            // 
            this.SRT4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT4.AutoSize = true;
            this.SRT4.Location = new System.Drawing.Point(3, 88);
            this.SRT4.Name = "SRT4";
            this.SRT4.Size = new System.Drawing.Size(115, 20);
            this.SRT4.TabIndex = 11;
            this.SRT4.Text = "500 hz:";
            // 
            // SRT1
            // 
            this.SRT1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT1.AutoSize = true;
            this.SRT1.Location = new System.Drawing.Point(3, 28);
            this.SRT1.Name = "SRT1";
            this.SRT1.Size = new System.Drawing.Size(115, 20);
            this.SRT1.TabIndex = 8;
            this.SRT1.Text = "62.5 hz:";
            // 
            // ISOCOMP
            // 
            this.ISOCOMP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ISOCOMP.AutoSize = true;
            this.ISOCOMP.Location = new System.Drawing.Point(143, 7);
            this.ISOCOMP.Name = "ISOCOMP";
            this.ISOCOMP.Size = new System.Drawing.Size(77, 13);
            this.ISOCOMP.TabIndex = 28;
            this.ISOCOMP.Text = "ISO-Compliant:";
            this.ISOCOMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label5
            // 
            this.Label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label5.AutoSize = true;
            this.Label5.Enabled = false;
            this.Label5.Location = new System.Drawing.Point(6, 173);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(103, 13);
            this.Label5.TabIndex = 1;
            this.Label5.Text = "Image Source Paths";
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
            // SP_menu
            // 
            this.SP_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.SP_menu.Location = new System.Drawing.Point(0, 0);
            this.SP_menu.Name = "SP_menu";
            this.SP_menu.Size = new System.Drawing.Size(400, 24);
            this.SP_menu.TabIndex = 14;
            this.SP_menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDataToolStripMenuItem,
            this.saveDataToolStripMenuItem,
            this.saveParameterResultsToolStripMenuItem,
            this.savePTBFormatToolStripMenuItem,
            this.saveVRSpectraToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openDataToolStripMenuItem
            // 
            this.openDataToolStripMenuItem.Name = "openDataToolStripMenuItem";
            this.openDataToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openDataToolStripMenuItem.Text = "Open Data...";
            this.openDataToolStripMenuItem.Click += new System.EventHandler(this.OpenDataToolStripMenuItem_Click);
            // 
            // saveDataToolStripMenuItem
            // 
            this.saveDataToolStripMenuItem.Name = "saveDataToolStripMenuItem";
            this.saveDataToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveDataToolStripMenuItem.Text = "Save Data";
            this.saveDataToolStripMenuItem.Click += new System.EventHandler(this.SaveDataToolStripMenuItem_Click);
            // 
            // saveParameterResultsToolStripMenuItem
            // 
            this.saveParameterResultsToolStripMenuItem.Name = "saveParameterResultsToolStripMenuItem";
            this.saveParameterResultsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveParameterResultsToolStripMenuItem.Text = "Save Results";
            this.saveParameterResultsToolStripMenuItem.Click += new System.EventHandler(this.SaveResultsToolStripMenuItem_Click);
            // 
            // savePTBFormatToolStripMenuItem
            // 
            this.savePTBFormatToolStripMenuItem.Name = "savePTBFormatToolStripMenuItem";
            this.savePTBFormatToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.savePTBFormatToolStripMenuItem.Text = "Save PTB Format";
            this.savePTBFormatToolStripMenuItem.Click += new System.EventHandler(this.savePTBFormatToolStripMenuItem_Click);
            // 
            // saveVRSpectraToolStripMenuItem
            // 
            this.saveVRSpectraToolStripMenuItem.Name = "saveVRSpectraToolStripMenuItem";
            this.saveVRSpectraToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            // 
            // Pach_Hybrid_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.SP_menu);
            this.Controls.Add(this.Tabs);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Pach_Hybrid_Control";
            this.Size = new System.Drawing.Size(400, 626);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AbsFlat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScatFlat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat8kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat8kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat4kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat4kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat2kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat2kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat1kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat1kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat500Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat500v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat250v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat250Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat125Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat125v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat63v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Scat63Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmartMat_Display)).EndInit();
            this.Tabs.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Spec_RayCount)).EndInit();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).EndInit();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Order)).EndInit();
            this.TabPage4.ResumeLayout(false);
            this.TabPage4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.Absorption.ResumeLayout(false);
            this.Scattering.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.Transparency.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_Flat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_8k_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_8kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_4k_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_4kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_2k_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_2kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_63v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_63_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_1k_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_1kv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_125_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_125v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_500_Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_250v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_500v)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Trans_250_Out)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.TabPage3.ResumeLayout(false);
            this.TabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.SP_menu.ResumeLayout(false);
            this.SP_menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }
            internal System.Windows.Forms.Button Calculate;
            internal System.Windows.Forms.CheckBox RTBox;
            internal System.Windows.Forms.CheckBox ISBox;
            internal System.Windows.Forms.TabControl Tabs;
            internal System.Windows.Forms.TabPage TabPage1;
            internal System.Windows.Forms.NumericUpDown Image_Order;
            internal System.Windows.Forms.NumericUpDown RT_Count;
            internal System.Windows.Forms.Label Label1;
            internal System.Windows.Forms.Label Label2;
            internal System.Windows.Forms.Label COTime;
            internal System.Windows.Forms.NumericUpDown CO_TIME;
            internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem FromMeshSphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem DirectionalSourceToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem SelectSourceToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem SelectASphereToolStripMenuItem;
            internal System.Windows.Forms.ToolStripMenuItem FromSphereObjectToolStripMenuItem1;
            internal System.Windows.Forms.ToolStripMenuItem FromPointInputToolStripMenuItem2;
            internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
            internal System.Windows.Forms.TabPage TabPage3;
            internal System.Windows.Forms.TabPage TabPage4;
            internal System.Windows.Forms.Label Label5;
            internal System.Windows.Forms.Label Label7;
            internal System.Windows.Forms.Label Label8;
            internal System.Windows.Forms.Label Label6;
            internal System.Windows.Forms.Label Label9;
            internal System.Windows.Forms.Label Label10;
            internal System.Windows.Forms.Label Label11;
            internal System.Windows.Forms.Label Label12;
            internal System.Windows.Forms.Label Label13;
            internal System.Windows.Forms.TrackBar Abs63;
            internal System.Windows.Forms.TrackBar Abs125;
            internal System.Windows.Forms.TrackBar Abs8k;
            internal System.Windows.Forms.TrackBar Abs500;
            internal System.Windows.Forms.TrackBar Abs4k;
            internal System.Windows.Forms.TrackBar Abs2k;
            internal System.Windows.Forms.TrackBar Abs1k;
            internal System.Windows.Forms.TrackBar Abs250;
            internal System.Windows.Forms.TrackBar Scat63v;
            internal System.Windows.Forms.TrackBar Scat125v;
            internal System.Windows.Forms.TrackBar Scat8kv;
            internal System.Windows.Forms.TrackBar Scat500v;
            internal System.Windows.Forms.TrackBar Scat4kv;
            internal System.Windows.Forms.TrackBar Scat2kv;
            internal System.Windows.Forms.TrackBar Scat1kv;
            internal System.Windows.Forms.TrackBar Scat250v;
            internal System.Windows.Forms.Label LayerLbl;
            internal System.Windows.Forms.Label Mat_Lbl;
            internal System.Windows.Forms.ListBox Material_Lib;
            internal System.Windows.Forms.GroupBox GroupBox2;
            internal System.Windows.Forms.Label Label17;
            internal System.Windows.Forms.ComboBox ReceiverSelection;
            internal System.Windows.Forms.GroupBox GroupBox3;
            internal System.Windows.Forms.Label SRT4;
            internal System.Windows.Forms.Label SRT3;
            internal System.Windows.Forms.Label SRT2;
            internal System.Windows.Forms.Label SRT1;
            internal System.Windows.Forms.Label SRT8;
            internal System.Windows.Forms.Label SRT7;
            internal System.Windows.Forms.Label SRT6;
            internal System.Windows.Forms.Label SRT5;
            internal System.Windows.Forms.ComboBox LayerDisplay;
            internal System.Windows.Forms.Label PathCount;
            internal System.Windows.Forms.ComboBox Parameter_Choice;
            internal System.Windows.Forms.GroupBox GroupBox4;
            internal System.Windows.Forms.Label Label3;
            internal System.Windows.Forms.Label AirTemp;
            internal System.Windows.Forms.NumericUpDown Rel_Humidity;
            internal System.Windows.Forms.NumericUpDown Air_Temp;
            internal System.Windows.Forms.Label Label19;
            internal System.Windows.Forms.NumericUpDown Air_Pressure;
            internal System.Windows.Forms.CheckBox Specular_Trace;
            internal System.Windows.Forms.NumericUpDown Spec_RayCount;
            internal System.Windows.Forms.Label Label21;
            internal System.Windows.Forms.ComboBox Atten_Method;
            internal System.Windows.Forms.ComboBox Receiver_Choice;
            internal System.Windows.Forms.Label label20;
            internal System.Windows.Forms.TrackBar ScatFlat;
            private System.Windows.Forms.NumericUpDown Abs63Out;
            private System.Windows.Forms.NumericUpDown Abs8kOut;
            private System.Windows.Forms.NumericUpDown Abs4kOut;
            private System.Windows.Forms.NumericUpDown Abs2kOut;
            private System.Windows.Forms.NumericUpDown Abs1kOut;
            private System.Windows.Forms.NumericUpDown Abs500Out;
            private System.Windows.Forms.NumericUpDown Abs250Out;
            private System.Windows.Forms.NumericUpDown Abs125Out;
            private System.Windows.Forms.NumericUpDown Scat8kOut;
            private System.Windows.Forms.NumericUpDown Scat4kOut;
            private System.Windows.Forms.NumericUpDown Scat2kOut;
            private System.Windows.Forms.NumericUpDown Scat1kOut;
            private System.Windows.Forms.NumericUpDown Scat500Out;
            private System.Windows.Forms.NumericUpDown Scat250Out;
            private System.Windows.Forms.NumericUpDown Scat125Out;
            private System.Windows.Forms.NumericUpDown Scat63Out;
            internal System.Windows.Forms.Label label24;
            private System.Windows.Forms.GroupBox groupBox5;
            private System.Windows.Forms.Button SaveAbs;
            private System.Windows.Forms.MaskedTextBox Material_Name;
            private System.Windows.Forms.CheckBox EdgeFreq;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
            private System.Windows.Forms.Label label18;
            private System.Windows.Forms.Label label22;
            internal System.Windows.Forms.CheckedListBox SourceList;
            internal System.Windows.Forms.Label label15;
            private System.Windows.Forms.Label ISOCOMP;
            private System.Windows.Forms.TrackBar AbsFlat;
            internal System.Windows.Forms.CheckBox BTM_ED;
            internal System.Windows.Forms.Label label27;
            internal System.Windows.Forms.Label label26;
            private System.Windows.Forms.NumericUpDown Alt_Choice;
            private System.Windows.Forms.NumericUpDown Azi_Choice;
            internal System.Windows.Forms.CheckedListBox IS_Path_Box;
            internal System.Windows.Forms.ComboBox Graph_Octave;
            private ZedGraph.ZedGraphControl Analysis_View;
            private System.Windows.Forms.CheckBox Normalize_Graph;
            private System.Windows.Forms.CheckBox LockUserScale;
            internal System.Windows.Forms.ComboBox Graph_Type;
            internal System.Windows.Forms.ComboBox Source_Aim;
            internal System.Windows.Forms.Label label25;
            private System.Windows.Forms.Button Abs_Designer;
            private System.Windows.Forms.DataVisualization.Charting.Chart SmartMat_Display;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
            internal System.Windows.Forms.Label label37;
            internal System.Windows.Forms.Label label38;
            internal System.Windows.Forms.Label label39;
            internal System.Windows.Forms.Label label40;
            internal System.Windows.Forms.Label label41;
            internal System.Windows.Forms.Label label42;
            internal System.Windows.Forms.Label label43;
            internal System.Windows.Forms.Label label44;
            internal System.Windows.Forms.Label label45;
            private System.Windows.Forms.TabControl tabControl1;
            private System.Windows.Forms.TabPage Absorption;
            private System.Windows.Forms.TabPage Scattering;
            private System.Windows.Forms.TabPage Transparency;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
            internal System.Windows.Forms.Label label23;
            internal System.Windows.Forms.TrackBar Trans_Flat;
            internal System.Windows.Forms.Label label28;
            private System.Windows.Forms.NumericUpDown Trans_8k_Out;
            internal System.Windows.Forms.Label label29;
            internal System.Windows.Forms.TrackBar Trans_8kv;
            internal System.Windows.Forms.Label label30;
            internal System.Windows.Forms.Label label31;
            internal System.Windows.Forms.Label label32;
            private System.Windows.Forms.NumericUpDown Trans_4k_Out;
            internal System.Windows.Forms.TrackBar Trans_4kv;
            internal System.Windows.Forms.Label label33;
            internal System.Windows.Forms.Label label34;
            private System.Windows.Forms.NumericUpDown Trans_2k_Out;
            internal System.Windows.Forms.Label label35;
            internal System.Windows.Forms.TrackBar Trans_2kv;
            private System.Windows.Forms.Label label36;
            internal System.Windows.Forms.TrackBar Trans_63v;
            private System.Windows.Forms.NumericUpDown Trans_63_Out;
            private System.Windows.Forms.NumericUpDown Trans_1k_Out;
            internal System.Windows.Forms.TrackBar Trans_1kv;
            private System.Windows.Forms.NumericUpDown Trans_125_Out;
            internal System.Windows.Forms.TrackBar Trans_125v;
            private System.Windows.Forms.NumericUpDown Trans_500_Out;
            internal System.Windows.Forms.TrackBar Trans_250v;
            internal System.Windows.Forms.TrackBar Trans_500v;
            private System.Windows.Forms.NumericUpDown Trans_250_Out;
            private System.Windows.Forms.CheckBox Trans_Check;
            private System.Windows.Forms.MenuStrip SP_menu;
            private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem openDataToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveDataToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveParameterResultsToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem savePTBFormatToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveVRSpectraToolStripMenuItem;
            private System.Windows.Forms.Button Auralisation;
        }
    }
}