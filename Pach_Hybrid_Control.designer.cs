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
            this.Abs_Table = new System.Windows.Forms.TableLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Delete_Material = new System.Windows.Forms.Button();
            this.Save_Material = new System.Windows.Forms.Button();
            this.Material_Name = new System.Windows.Forms.MaskedTextBox();
            this.Abs_Designer = new System.Windows.Forms.Button();
            this.Abs4kOut = new System.Windows.Forms.NumericUpDown();
            this.Material_Lib = new System.Windows.Forms.ListBox();
            this.Mat_Lbl = new System.Windows.Forms.Label();
            this.Abs8kOut = new System.Windows.Forms.NumericUpDown();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Abs2kOut = new System.Windows.Forms.NumericUpDown();
            this.Abs1kOut = new System.Windows.Forms.NumericUpDown();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Abs500Out = new System.Windows.Forms.NumericUpDown();
            this.Abs250Out = new System.Windows.Forms.NumericUpDown();
            this.Abs125Out = new System.Windows.Forms.NumericUpDown();
            this.Abs63Out = new System.Windows.Forms.NumericUpDown();
            this.Abs63 = new System.Windows.Forms.TrackBar();
            this.Abs125 = new System.Windows.Forms.TrackBar();
            this.Abs250 = new System.Windows.Forms.TrackBar();
            this.Abs500 = new System.Windows.Forms.TrackBar();
            this.Abs1k = new System.Windows.Forms.TrackBar();
            this.Abs2k = new System.Windows.Forms.TrackBar();
            this.Abs4k = new System.Windows.Forms.TrackBar();
            this.Abs8k = new System.Windows.Forms.TrackBar();
            this.AbsFlat = new System.Windows.Forms.TrackBar();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ThirdOctave_Abs = new System.Windows.Forms.RadioButton();
            this.Octave_Abs = new System.Windows.Forms.RadioButton();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.Abs50 = new System.Windows.Forms.TrackBar();
            this.Abs80 = new System.Windows.Forms.TrackBar();
            this.Abs100 = new System.Windows.Forms.TrackBar();
            this.Abs160 = new System.Windows.Forms.TrackBar();
            this.Abs200 = new System.Windows.Forms.TrackBar();
            this.Abs315 = new System.Windows.Forms.TrackBar();
            this.Abs400 = new System.Windows.Forms.TrackBar();
            this.Abs630 = new System.Windows.Forms.TrackBar();
            this.Abs800 = new System.Windows.Forms.TrackBar();
            this.Abs1250 = new System.Windows.Forms.TrackBar();
            this.Abs1600 = new System.Windows.Forms.TrackBar();
            this.Abs2500 = new System.Windows.Forms.TrackBar();
            this.Abs3150 = new System.Windows.Forms.TrackBar();
            this.Abs5k = new System.Windows.Forms.TrackBar();
            this.Abs6300 = new System.Windows.Forms.TrackBar();
            this.Abs10k = new System.Windows.Forms.TrackBar();
            this.Abs50Out = new System.Windows.Forms.NumericUpDown();
            this.Abs80Out = new System.Windows.Forms.NumericUpDown();
            this.Abs100Out = new System.Windows.Forms.NumericUpDown();
            this.Abs160Out = new System.Windows.Forms.NumericUpDown();
            this.Abs200Out = new System.Windows.Forms.NumericUpDown();
            this.Abs315Out = new System.Windows.Forms.NumericUpDown();
            this.Abs400Out = new System.Windows.Forms.NumericUpDown();
            this.Abs630Out = new System.Windows.Forms.NumericUpDown();
            this.Abs800Out = new System.Windows.Forms.NumericUpDown();
            this.Abs1250Out = new System.Windows.Forms.NumericUpDown();
            this.Abs1600Out = new System.Windows.Forms.NumericUpDown();
            this.Abs2500Out = new System.Windows.Forms.NumericUpDown();
            this.Abs3150Out = new System.Windows.Forms.NumericUpDown();
            this.Abs5kOut = new System.Windows.Forms.NumericUpDown();
            this.Abs6300Out = new System.Windows.Forms.NumericUpDown();
            this.Abs10kOut = new System.Windows.Forms.NumericUpDown();
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
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
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
            this.CO_TIME = new System.Windows.Forms.NumericUpDown();
            this.Spec_Rays = new System.Windows.Forms.RadioButton();
            this.ReceiverSelection = new System.Windows.Forms.ComboBox();
            this.RT_Count = new System.Windows.Forms.NumericUpDown();
            this.Label2 = new System.Windows.Forms.Label();
            this.Spec_RayCount = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.Image_Order = new System.Windows.Forms.NumericUpDown();
            this.DetailedConvergence = new System.Windows.Forms.RadioButton();
            this.Minimum_Convergence = new System.Windows.Forms.RadioButton();
            this.Label1 = new System.Windows.Forms.Label();
            this.BTM_ED = new System.Windows.Forms.CheckBox();
            this.Specular_Trace = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.COTime = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OctaveBand = new System.Windows.Forms.RadioButton();
            this.ThirdOctaveBand = new System.Windows.Forms.RadioButton();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.LayerLbl = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Absorption = new System.Windows.Forms.TabPage();
            this.Scattering = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.quart_lambda = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.user_quart_lambda = new System.Windows.Forms.TrackBar();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.PlasterScatter = new System.Windows.Forms.Button();
            this.GlassScatter = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.Transparency = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
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
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label64 = new System.Windows.Forms.Label();
            this.Isolation_Lib = new System.Windows.Forms.ListBox();
            this.label63 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DeleteAssembly = new System.Windows.Forms.Button();
            this.SaveAssembly = new System.Windows.Forms.Button();
            this.IsolationAssemblies = new System.Windows.Forms.MaskedTextBox();
            this.label53 = new System.Windows.Forms.Label();
            this.TL_Check = new System.Windows.Forms.CheckBox();
            this.TL8k = new System.Windows.Forms.NumericUpDown();
            this.label54 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.TL4k = new System.Windows.Forms.NumericUpDown();
            this.TL2k = new System.Windows.Forms.NumericUpDown();
            this.TL1k = new System.Windows.Forms.NumericUpDown();
            this.TL500 = new System.Windows.Forms.NumericUpDown();
            this.TL250 = new System.Windows.Forms.NumericUpDown();
            this.TL125 = new System.Windows.Forms.NumericUpDown();
            this.TL63 = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.LayerDisplay = new System.Windows.Forms.ComboBox();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.SourceList = new System.Windows.Forms.CheckedListBox();
            this.SourceContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PowerMod = new System.Windows.Forms.ToolStripMenuItem();
            this.DelayMod = new System.Windows.Forms.ToolStripMenuItem();
            this.Source_Aim = new System.Windows.Forms.ComboBox();
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
            this.label20 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.Receiver_Choice = new System.Windows.Forms.ComboBox();
            this.Analysis_View = new ZedGraph.ZedGraphControl();
            this.label26 = new System.Windows.Forms.Label();
            this.Normalize_Graph = new System.Windows.Forms.CheckBox();
            this.Alt_Choice = new System.Windows.Forms.NumericUpDown();
            this.LockUserScale = new System.Windows.Forms.CheckBox();
            this.Azi_Choice = new System.Windows.Forms.NumericUpDown();
            this.Graph_Type = new System.Windows.Forms.ComboBox();
            this.Auralisation = new System.Windows.Forms.Button();
            this.Label5 = new System.Windows.Forms.Label();
            this.Graph_Octave = new System.Windows.Forms.ComboBox();
            this.IS_Path_Box = new System.Windows.Forms.CheckedListBox();
            this.PathContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.PathCount = new System.Windows.Forms.Label();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromMeshSphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DirectionalSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectSourceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectASphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FromSphereObjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FromPointInputToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveParameterResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePTBFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveVRSpectraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SP_menu = new System.Windows.Forms.MenuStrip();
            this.Abs_Table.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AbsFlat)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abs50)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs80)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs100)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs160)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs200)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs315)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs400)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs630)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs800)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1250)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1600)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2500)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs3150)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs5k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs6300)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs10k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs50Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs80Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs100Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs160Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs200Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs315Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs400Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs630Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs800Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1250Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1600Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2500Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs3150Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs5kOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs6300Out)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs10kOut)).BeginInit();
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
            this.tableLayoutPanel7.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Spec_RayCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Order)).BeginInit();
            this.panel1.SuspendLayout();
            this.TabPage4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Absorption.SuspendLayout();
            this.Scattering.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.user_quart_lambda)).BeginInit();
            this.Transparency.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TL8k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL4k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL2k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL1k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL500)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL250)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL125)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL63)).BeginInit();
            this.TabPage3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SourceContext.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).BeginInit();
            this.PathContext.SuspendLayout();
            this.SP_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Abs_Table
            // 
            this.Abs_Table.AutoScroll = true;
            this.Abs_Table.ColumnCount = 4;
            this.Abs_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.Abs_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Abs_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Abs_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.Abs_Table.Controls.Add(this.label24, 0, 29);
            this.Abs_Table.Controls.Add(this.groupBox5, 2, 0);
            this.Abs_Table.Controls.Add(this.Abs_Designer, 0, 2);
            this.Abs_Table.Controls.Add(this.Abs4kOut, 3, 24);
            this.Abs_Table.Controls.Add(this.Material_Lib, 0, 1);
            this.Abs_Table.Controls.Add(this.Mat_Lbl, 0, 0);
            this.Abs_Table.Controls.Add(this.Abs8kOut, 3, 27);
            this.Abs_Table.Controls.Add(this.Label8, 0, 27);
            this.Abs_Table.Controls.Add(this.Label6, 0, 24);
            this.Abs_Table.Controls.Add(this.Label9, 0, 21);
            this.Abs_Table.Controls.Add(this.Abs2kOut, 3, 21);
            this.Abs_Table.Controls.Add(this.Abs1kOut, 3, 18);
            this.Abs_Table.Controls.Add(this.Label10, 0, 18);
            this.Abs_Table.Controls.Add(this.Label11, 0, 15);
            this.Abs_Table.Controls.Add(this.Label7, 0, 12);
            this.Abs_Table.Controls.Add(this.Label12, 0, 9);
            this.Abs_Table.Controls.Add(this.Label13, 0, 6);
            this.Abs_Table.Controls.Add(this.Abs500Out, 3, 15);
            this.Abs_Table.Controls.Add(this.Abs250Out, 3, 12);
            this.Abs_Table.Controls.Add(this.Abs125Out, 3, 9);
            this.Abs_Table.Controls.Add(this.Abs63Out, 3, 6);
            this.Abs_Table.Controls.Add(this.Abs63, 1, 6);
            this.Abs_Table.Controls.Add(this.Abs125, 1, 9);
            this.Abs_Table.Controls.Add(this.Abs250, 1, 12);
            this.Abs_Table.Controls.Add(this.Abs500, 1, 15);
            this.Abs_Table.Controls.Add(this.Abs1k, 1, 18);
            this.Abs_Table.Controls.Add(this.Abs2k, 1, 21);
            this.Abs_Table.Controls.Add(this.Abs4k, 1, 24);
            this.Abs_Table.Controls.Add(this.Abs8k, 1, 27);
            this.Abs_Table.Controls.Add(this.AbsFlat, 1, 29);
            this.Abs_Table.Controls.Add(this.label18, 0, 4);
            this.Abs_Table.Controls.Add(this.groupBox2, 0, 3);
            this.Abs_Table.Controls.Add(this.label66, 0, 5);
            this.Abs_Table.Controls.Add(this.label67, 0, 7);
            this.Abs_Table.Controls.Add(this.label68, 0, 8);
            this.Abs_Table.Controls.Add(this.label69, 0, 10);
            this.Abs_Table.Controls.Add(this.label70, 0, 11);
            this.Abs_Table.Controls.Add(this.label71, 0, 13);
            this.Abs_Table.Controls.Add(this.label72, 0, 14);
            this.Abs_Table.Controls.Add(this.label73, 0, 16);
            this.Abs_Table.Controls.Add(this.label74, 0, 17);
            this.Abs_Table.Controls.Add(this.label75, 0, 19);
            this.Abs_Table.Controls.Add(this.label76, 0, 20);
            this.Abs_Table.Controls.Add(this.label77, 0, 22);
            this.Abs_Table.Controls.Add(this.label78, 0, 23);
            this.Abs_Table.Controls.Add(this.label79, 0, 25);
            this.Abs_Table.Controls.Add(this.label80, 0, 26);
            this.Abs_Table.Controls.Add(this.label81, 0, 28);
            this.Abs_Table.Controls.Add(this.Abs50, 1, 5);
            this.Abs_Table.Controls.Add(this.Abs80, 1, 7);
            this.Abs_Table.Controls.Add(this.Abs100, 1, 8);
            this.Abs_Table.Controls.Add(this.Abs160, 1, 10);
            this.Abs_Table.Controls.Add(this.Abs200, 1, 11);
            this.Abs_Table.Controls.Add(this.Abs315, 1, 13);
            this.Abs_Table.Controls.Add(this.Abs400, 1, 14);
            this.Abs_Table.Controls.Add(this.Abs630, 1, 16);
            this.Abs_Table.Controls.Add(this.Abs800, 1, 17);
            this.Abs_Table.Controls.Add(this.Abs1250, 1, 19);
            this.Abs_Table.Controls.Add(this.Abs1600, 1, 20);
            this.Abs_Table.Controls.Add(this.Abs2500, 1, 22);
            this.Abs_Table.Controls.Add(this.Abs3150, 1, 23);
            this.Abs_Table.Controls.Add(this.Abs5k, 1, 25);
            this.Abs_Table.Controls.Add(this.Abs6300, 1, 26);
            this.Abs_Table.Controls.Add(this.Abs10k, 1, 28);
            this.Abs_Table.Controls.Add(this.Abs50Out, 3, 5);
            this.Abs_Table.Controls.Add(this.Abs80Out, 3, 7);
            this.Abs_Table.Controls.Add(this.Abs100Out, 3, 8);
            this.Abs_Table.Controls.Add(this.Abs160Out, 3, 10);
            this.Abs_Table.Controls.Add(this.Abs200Out, 3, 11);
            this.Abs_Table.Controls.Add(this.Abs315Out, 3, 13);
            this.Abs_Table.Controls.Add(this.Abs400Out, 3, 14);
            this.Abs_Table.Controls.Add(this.Abs630Out, 3, 16);
            this.Abs_Table.Controls.Add(this.Abs800Out, 3, 17);
            this.Abs_Table.Controls.Add(this.Abs1250Out, 3, 19);
            this.Abs_Table.Controls.Add(this.Abs1600Out, 3, 20);
            this.Abs_Table.Controls.Add(this.Abs2500Out, 3, 22);
            this.Abs_Table.Controls.Add(this.Abs3150Out, 3, 23);
            this.Abs_Table.Controls.Add(this.Abs5kOut, 3, 25);
            this.Abs_Table.Controls.Add(this.Abs6300Out, 3, 26);
            this.Abs_Table.Controls.Add(this.Abs10kOut, 3, 28);
            this.Abs_Table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Abs_Table.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.Abs_Table.Location = new System.Drawing.Point(6, 6);
            this.Abs_Table.Margin = new System.Windows.Forms.Padding(6);
            this.Abs_Table.Name = "Abs_Table";
            this.Abs_Table.RowCount = 31;
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Abs_Table.Size = new System.Drawing.Size(852, 1382);
            this.Abs_Table.TabIndex = 29;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 1316);
            this.label24.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(108, 40);
            this.label24.TabIndex = 38;
            this.label24.Text = "Flatten All";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox5
            // 
            this.Abs_Table.SetColumnSpan(this.groupBox5, 2);
            this.groupBox5.Controls.Add(this.Delete_Material);
            this.groupBox5.Controls.Add(this.Save_Material);
            this.groupBox5.Controls.Add(this.Material_Name);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(447, 6);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(6);
            this.Abs_Table.SetRowSpan(this.groupBox5, 2);
            this.groupBox5.Size = new System.Drawing.Size(399, 190);
            this.groupBox5.TabIndex = 28;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Save Material Absorption";
            // 
            // Delete_Material
            // 
            this.Delete_Material.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Delete_Material.Location = new System.Drawing.Point(12, 140);
            this.Delete_Material.Margin = new System.Windows.Forms.Padding(6);
            this.Delete_Material.Name = "Delete_Material";
            this.Delete_Material.Size = new System.Drawing.Size(435, 44);
            this.Delete_Material.TabIndex = 2;
            this.Delete_Material.Text = "Delete Materials";
            this.Delete_Material.UseVisualStyleBackColor = true;
            this.Delete_Material.Click += new System.EventHandler(this.Delete_Material_Click);
            // 
            // Save_Material
            // 
            this.Save_Material.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Save_Material.Location = new System.Drawing.Point(12, 85);
            this.Save_Material.Margin = new System.Windows.Forms.Padding(6);
            this.Save_Material.Name = "Save_Material";
            this.Save_Material.Size = new System.Drawing.Size(435, 44);
            this.Save_Material.TabIndex = 1;
            this.Save_Material.Text = "Save Material";
            this.Save_Material.UseVisualStyleBackColor = true;
            this.Save_Material.Click += new System.EventHandler(this.SaveAbs_Click);
            // 
            // Material_Name
            // 
            this.Material_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Material_Name.Location = new System.Drawing.Point(12, 35);
            this.Material_Name.Margin = new System.Windows.Forms.Padding(6);
            this.Material_Name.Name = "Material_Name";
            this.Material_Name.Size = new System.Drawing.Size(431, 31);
            this.Material_Name.TabIndex = 0;
            // 
            // Abs_Designer
            // 
            this.Abs_Table.SetColumnSpan(this.Abs_Designer, 4);
            this.Abs_Designer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Abs_Designer.Location = new System.Drawing.Point(6, 208);
            this.Abs_Designer.Margin = new System.Windows.Forms.Padding(6);
            this.Abs_Designer.Name = "Abs_Designer";
            this.Abs_Designer.Size = new System.Drawing.Size(840, 54);
            this.Abs_Designer.TabIndex = 2;
            this.Abs_Designer.Text = "Call Absorption Designer";
            this.Abs_Designer.UseVisualStyleBackColor = true;
            this.Abs_Designer.Click += new System.EventHandler(this.Abs_Designer_Click);
            // 
            // Abs4kOut
            // 
            this.Abs4kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs4kOut.AutoSize = true;
            this.Abs4kOut.Location = new System.Drawing.Point(738, 1122);
            this.Abs4kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Abs4kOut.Name = "Abs4kOut";
            this.Abs4kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs4kOut.Size = new System.Drawing.Size(108, 31);
            this.Abs4kOut.TabIndex = 32;
            this.Abs4kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs4kOut.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Material_Lib
            // 
            this.Abs_Table.SetColumnSpan(this.Material_Lib, 2);
            this.Material_Lib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Material_Lib.FormattingEnabled = true;
            this.Material_Lib.ItemHeight = 25;
            this.Material_Lib.Location = new System.Drawing.Point(6, 41);
            this.Material_Lib.Margin = new System.Windows.Forms.Padding(6);
            this.Material_Lib.Name = "Material_Lib";
            this.Material_Lib.ScrollAlwaysVisible = true;
            this.Material_Lib.Size = new System.Drawing.Size(429, 155);
            this.Material_Lib.TabIndex = 8;
            this.Material_Lib.SelectedIndexChanged += new System.EventHandler(this.Material_Lib_SelectedIndexChanged);
            // 
            // Mat_Lbl
            // 
            this.Mat_Lbl.AutoSize = true;
            this.Mat_Lbl.Location = new System.Drawing.Point(6, 0);
            this.Mat_Lbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Mat_Lbl.Name = "Mat_Lbl";
            this.Mat_Lbl.Size = new System.Drawing.Size(95, 35);
            this.Mat_Lbl.TabIndex = 7;
            this.Mat_Lbl.Text = "Material Library:";
            // 
            // Abs8kOut
            // 
            this.Abs8kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs8kOut.AutoSize = true;
            this.Abs8kOut.Location = new System.Drawing.Point(738, 1242);
            this.Abs8kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Abs8kOut.Name = "Abs8kOut";
            this.Abs8kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs8kOut.Size = new System.Drawing.Size(108, 31);
            this.Abs8kOut.TabIndex = 33;
            this.Abs8kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs8kOut.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Label8
            // 
            this.Label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(6, 1236);
            this.Label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(67, 40);
            this.Label8.TabIndex = 24;
            this.Label8.Text = "8 kHz";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label6
            // 
            this.Label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(6, 1116);
            this.Label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(67, 40);
            this.Label6.TabIndex = 22;
            this.Label6.Text = "4 kHz";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label9
            // 
            this.Label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(6, 996);
            this.Label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(67, 40);
            this.Label9.TabIndex = 21;
            this.Label9.Text = "2 kHz";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Abs2kOut
            // 
            this.Abs2kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs2kOut.AutoSize = true;
            this.Abs2kOut.Location = new System.Drawing.Point(738, 1002);
            this.Abs2kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Abs2kOut.Name = "Abs2kOut";
            this.Abs2kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs2kOut.Size = new System.Drawing.Size(108, 31);
            this.Abs2kOut.TabIndex = 31;
            this.Abs2kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs2kOut.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs1kOut
            // 
            this.Abs1kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1kOut.AutoSize = true;
            this.Abs1kOut.Location = new System.Drawing.Point(738, 882);
            this.Abs1kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Abs1kOut.Name = "Abs1kOut";
            this.Abs1kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs1kOut.Size = new System.Drawing.Size(108, 31);
            this.Abs1kOut.TabIndex = 30;
            this.Abs1kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs1kOut.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Label10
            // 
            this.Label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(6, 876);
            this.Label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(67, 40);
            this.Label10.TabIndex = 20;
            this.Label10.Text = "1 kHz";
            this.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label11
            // 
            this.Label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(6, 756);
            this.Label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(80, 40);
            this.Label11.TabIndex = 19;
            this.Label11.Text = "500 Hz";
            this.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label7
            // 
            this.Label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(6, 636);
            this.Label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(80, 40);
            this.Label7.TabIndex = 25;
            this.Label7.Text = "250 Hz";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label12
            // 
            this.Label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(6, 516);
            this.Label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(80, 40);
            this.Label12.TabIndex = 18;
            this.Label12.Text = "125 Hz";
            this.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label13
            // 
            this.Label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(6, 396);
            this.Label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(86, 40);
            this.Label13.TabIndex = 17;
            this.Label13.Text = "62.5 Hz";
            this.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Abs500Out
            // 
            this.Abs500Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs500Out.AutoSize = true;
            this.Abs500Out.Location = new System.Drawing.Point(738, 762);
            this.Abs500Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs500Out.Name = "Abs500Out";
            this.Abs500Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs500Out.Size = new System.Drawing.Size(108, 31);
            this.Abs500Out.TabIndex = 29;
            this.Abs500Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs500Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs250Out
            // 
            this.Abs250Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs250Out.AutoSize = true;
            this.Abs250Out.Location = new System.Drawing.Point(738, 642);
            this.Abs250Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs250Out.Name = "Abs250Out";
            this.Abs250Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs250Out.Size = new System.Drawing.Size(108, 31);
            this.Abs250Out.TabIndex = 28;
            this.Abs250Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs250Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs125Out
            // 
            this.Abs125Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs125Out.AutoSize = true;
            this.Abs125Out.Location = new System.Drawing.Point(738, 522);
            this.Abs125Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs125Out.Name = "Abs125Out";
            this.Abs125Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs125Out.Size = new System.Drawing.Size(108, 31);
            this.Abs125Out.TabIndex = 27;
            this.Abs125Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs125Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs63Out
            // 
            this.Abs63Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs63Out.AutoSize = true;
            this.Abs63Out.Location = new System.Drawing.Point(738, 402);
            this.Abs63Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs63Out.Name = "Abs63Out";
            this.Abs63Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs63Out.Size = new System.Drawing.Size(108, 31);
            this.Abs63Out.TabIndex = 26;
            this.Abs63Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs63Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs63
            // 
            this.Abs63.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs63.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs63, 2);
            this.Abs63.LargeChange = 10;
            this.Abs63.Location = new System.Drawing.Point(156, 402);
            this.Abs63.Margin = new System.Windows.Forms.Padding(6);
            this.Abs63.Maximum = 100;
            this.Abs63.Name = "Abs63";
            this.Abs63.Size = new System.Drawing.Size(570, 28);
            this.Abs63.TabIndex = 16;
            this.Abs63.TickFrequency = 10;
            this.Abs63.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs63.Value = 1;
            this.Abs63.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs125
            // 
            this.Abs125.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs125.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs125, 2);
            this.Abs125.LargeChange = 10;
            this.Abs125.Location = new System.Drawing.Point(156, 522);
            this.Abs125.Margin = new System.Windows.Forms.Padding(6);
            this.Abs125.Maximum = 100;
            this.Abs125.Name = "Abs125";
            this.Abs125.Size = new System.Drawing.Size(570, 28);
            this.Abs125.TabIndex = 15;
            this.Abs125.TickFrequency = 10;
            this.Abs125.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs125.Value = 1;
            this.Abs125.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs250
            // 
            this.Abs250.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs250.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs250, 2);
            this.Abs250.LargeChange = 10;
            this.Abs250.Location = new System.Drawing.Point(156, 642);
            this.Abs250.Margin = new System.Windows.Forms.Padding(6);
            this.Abs250.Maximum = 100;
            this.Abs250.Name = "Abs250";
            this.Abs250.Size = new System.Drawing.Size(570, 28);
            this.Abs250.TabIndex = 9;
            this.Abs250.TickFrequency = 10;
            this.Abs250.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs250.Value = 1;
            this.Abs250.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs500
            // 
            this.Abs500.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs500.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs500, 2);
            this.Abs500.LargeChange = 10;
            this.Abs500.Location = new System.Drawing.Point(156, 762);
            this.Abs500.Margin = new System.Windows.Forms.Padding(6);
            this.Abs500.Maximum = 100;
            this.Abs500.Name = "Abs500";
            this.Abs500.Size = new System.Drawing.Size(570, 28);
            this.Abs500.TabIndex = 13;
            this.Abs500.TickFrequency = 10;
            this.Abs500.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs500.Value = 1;
            this.Abs500.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs1k
            // 
            this.Abs1k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs1k, 2);
            this.Abs1k.LargeChange = 10;
            this.Abs1k.Location = new System.Drawing.Point(156, 882);
            this.Abs1k.Margin = new System.Windows.Forms.Padding(6);
            this.Abs1k.Maximum = 100;
            this.Abs1k.Name = "Abs1k";
            this.Abs1k.Size = new System.Drawing.Size(570, 28);
            this.Abs1k.TabIndex = 10;
            this.Abs1k.TickFrequency = 10;
            this.Abs1k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs1k.Value = 1;
            this.Abs1k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs2k
            // 
            this.Abs2k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs2k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs2k, 2);
            this.Abs2k.LargeChange = 10;
            this.Abs2k.Location = new System.Drawing.Point(156, 1002);
            this.Abs2k.Margin = new System.Windows.Forms.Padding(6);
            this.Abs2k.Maximum = 100;
            this.Abs2k.Name = "Abs2k";
            this.Abs2k.Size = new System.Drawing.Size(570, 28);
            this.Abs2k.TabIndex = 11;
            this.Abs2k.TickFrequency = 10;
            this.Abs2k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs2k.Value = 1;
            this.Abs2k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs4k
            // 
            this.Abs4k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs4k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs4k, 2);
            this.Abs4k.LargeChange = 10;
            this.Abs4k.Location = new System.Drawing.Point(156, 1122);
            this.Abs4k.Margin = new System.Windows.Forms.Padding(6);
            this.Abs4k.Maximum = 100;
            this.Abs4k.Name = "Abs4k";
            this.Abs4k.Size = new System.Drawing.Size(570, 28);
            this.Abs4k.TabIndex = 12;
            this.Abs4k.TickFrequency = 10;
            this.Abs4k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs4k.Value = 1;
            this.Abs4k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs8k
            // 
            this.Abs8k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs8k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs8k, 2);
            this.Abs8k.LargeChange = 10;
            this.Abs8k.Location = new System.Drawing.Point(156, 1242);
            this.Abs8k.Margin = new System.Windows.Forms.Padding(6);
            this.Abs8k.Maximum = 100;
            this.Abs8k.Name = "Abs8k";
            this.Abs8k.Size = new System.Drawing.Size(570, 28);
            this.Abs8k.TabIndex = 14;
            this.Abs8k.TickFrequency = 10;
            this.Abs8k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs8k.Value = 1;
            this.Abs8k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // AbsFlat
            // 
            this.AbsFlat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AbsFlat.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.AbsFlat, 2);
            this.AbsFlat.LargeChange = 10;
            this.AbsFlat.Location = new System.Drawing.Point(156, 1322);
            this.AbsFlat.Margin = new System.Windows.Forms.Padding(6);
            this.AbsFlat.Maximum = 100;
            this.AbsFlat.Name = "AbsFlat";
            this.AbsFlat.Size = new System.Drawing.Size(570, 28);
            this.AbsFlat.TabIndex = 46;
            this.AbsFlat.TickFrequency = 10;
            this.AbsFlat.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.AbsFlat.ValueChanged += new System.EventHandler(this.AbsFlat_ValueChanged);
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.AutoSize = true;
            this.Abs_Table.SetColumnSpan(this.label18, 3);
            this.label18.Location = new System.Drawing.Point(6, 331);
            this.label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(441, 25);
            this.label18.TabIndex = 40;
            this.label18.Text = "Absorption Coefficients (% energy absorbed)";
            // 
            // groupBox2
            // 
            this.Abs_Table.SetColumnSpan(this.groupBox2, 4);
            this.groupBox2.Controls.Add(this.ThirdOctave_Abs);
            this.groupBox2.Controls.Add(this.Octave_Abs);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(846, 54);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Assign by:";
            // 
            // ThirdOctave_Abs
            // 
            this.ThirdOctave_Abs.AutoSize = true;
            this.ThirdOctave_Abs.Location = new System.Drawing.Point(438, 15);
            this.ThirdOctave_Abs.Name = "ThirdOctave_Abs";
            this.ThirdOctave_Abs.Size = new System.Drawing.Size(203, 29);
            this.ThirdOctave_Abs.TabIndex = 1;
            this.ThirdOctave_Abs.Text = "1/3 Octave Band";
            this.ThirdOctave_Abs.UseVisualStyleBackColor = true;
            // 
            // Octave_Abs
            // 
            this.Octave_Abs.AutoSize = true;
            this.Octave_Abs.Checked = true;
            this.Octave_Abs.Location = new System.Drawing.Point(226, 15);
            this.Octave_Abs.Name = "Octave_Abs";
            this.Octave_Abs.Size = new System.Drawing.Size(203, 29);
            this.Octave_Abs.TabIndex = 0;
            this.Octave_Abs.TabStop = true;
            this.Octave_Abs.Text = "1/1 Octave Band";
            this.Octave_Abs.UseVisualStyleBackColor = true;
            this.Octave_Abs.CheckedChanged += new System.EventHandler(this.Octave_Abs_CheckedChanged);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label66.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.Location = new System.Drawing.Point(6, 356);
            this.label66.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(138, 40);
            this.label66.TabIndex = 48;
            this.label66.Text = "50 Hz";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label67.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.Location = new System.Drawing.Point(6, 436);
            this.label67.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(138, 40);
            this.label67.TabIndex = 49;
            this.label67.Text = "80 Hz";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label68.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.Location = new System.Drawing.Point(6, 476);
            this.label68.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(138, 40);
            this.label68.TabIndex = 50;
            this.label68.Text = "100 Hz";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label69.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label69.Location = new System.Drawing.Point(6, 556);
            this.label69.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(138, 40);
            this.label69.TabIndex = 51;
            this.label69.Text = "160 Hz";
            this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.Location = new System.Drawing.Point(6, 596);
            this.label70.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(138, 40);
            this.label70.TabIndex = 52;
            this.label70.Text = "200 Hz";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label71.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.Location = new System.Drawing.Point(6, 676);
            this.label71.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(138, 40);
            this.label71.TabIndex = 53;
            this.label71.Text = "315 Hz";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label72.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label72.Location = new System.Drawing.Point(6, 716);
            this.label72.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(138, 40);
            this.label72.TabIndex = 54;
            this.label72.Text = "400 Hz";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label73.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label73.Location = new System.Drawing.Point(6, 796);
            this.label73.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(138, 40);
            this.label73.TabIndex = 55;
            this.label73.Text = "630 Hz";
            this.label73.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label74.Location = new System.Drawing.Point(6, 836);
            this.label74.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(138, 40);
            this.label74.TabIndex = 56;
            this.label74.Text = "800 Hz";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label75.Location = new System.Drawing.Point(6, 916);
            this.label75.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(138, 40);
            this.label75.TabIndex = 57;
            this.label75.Text = "1.25 kHz";
            this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label76.Location = new System.Drawing.Point(6, 956);
            this.label76.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(138, 40);
            this.label76.TabIndex = 58;
            this.label76.Text = "1.6 kHz";
            this.label76.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label77.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label77.Location = new System.Drawing.Point(6, 1036);
            this.label77.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(138, 40);
            this.label77.TabIndex = 59;
            this.label77.Text = "2.5 kHz";
            this.label77.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label78.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label78.Location = new System.Drawing.Point(6, 1076);
            this.label78.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(138, 40);
            this.label78.TabIndex = 60;
            this.label78.Text = "3.15 kHz.";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label79.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label79.Location = new System.Drawing.Point(6, 1156);
            this.label79.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(138, 40);
            this.label79.TabIndex = 61;
            this.label79.Text = "5 kHz";
            this.label79.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label80.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label80.Location = new System.Drawing.Point(6, 1196);
            this.label80.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(138, 40);
            this.label80.TabIndex = 62;
            this.label80.Text = "6.3 kHz";
            this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label81.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label81.Location = new System.Drawing.Point(6, 1276);
            this.label81.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(138, 40);
            this.label81.TabIndex = 63;
            this.label81.Text = "10 kHz";
            this.label81.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Abs50
            // 
            this.Abs50.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs50.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs50, 2);
            this.Abs50.LargeChange = 10;
            this.Abs50.Location = new System.Drawing.Point(156, 362);
            this.Abs50.Margin = new System.Windows.Forms.Padding(6);
            this.Abs50.Maximum = 100;
            this.Abs50.Name = "Abs50";
            this.Abs50.Size = new System.Drawing.Size(570, 28);
            this.Abs50.TabIndex = 16;
            this.Abs50.TickFrequency = 10;
            this.Abs50.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs50.Value = 1;
            this.Abs50.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs80
            // 
            this.Abs80.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs80.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs80, 2);
            this.Abs80.LargeChange = 10;
            this.Abs80.Location = new System.Drawing.Point(156, 442);
            this.Abs80.Margin = new System.Windows.Forms.Padding(6);
            this.Abs80.Maximum = 100;
            this.Abs80.Name = "Abs80";
            this.Abs80.Size = new System.Drawing.Size(570, 28);
            this.Abs80.TabIndex = 16;
            this.Abs80.TickFrequency = 10;
            this.Abs80.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs80.Value = 1;
            this.Abs80.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs100
            // 
            this.Abs100.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs100.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs100, 2);
            this.Abs100.LargeChange = 10;
            this.Abs100.Location = new System.Drawing.Point(156, 482);
            this.Abs100.Margin = new System.Windows.Forms.Padding(6);
            this.Abs100.Maximum = 100;
            this.Abs100.Name = "Abs100";
            this.Abs100.Size = new System.Drawing.Size(570, 28);
            this.Abs100.TabIndex = 16;
            this.Abs100.TickFrequency = 10;
            this.Abs100.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs100.Value = 1;
            this.Abs100.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs160
            // 
            this.Abs160.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs160.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs160, 2);
            this.Abs160.LargeChange = 10;
            this.Abs160.Location = new System.Drawing.Point(156, 562);
            this.Abs160.Margin = new System.Windows.Forms.Padding(6);
            this.Abs160.Maximum = 100;
            this.Abs160.Name = "Abs160";
            this.Abs160.Size = new System.Drawing.Size(570, 28);
            this.Abs160.TabIndex = 16;
            this.Abs160.TickFrequency = 10;
            this.Abs160.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs160.Value = 1;
            this.Abs160.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs200
            // 
            this.Abs200.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs200.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs200, 2);
            this.Abs200.LargeChange = 10;
            this.Abs200.Location = new System.Drawing.Point(156, 602);
            this.Abs200.Margin = new System.Windows.Forms.Padding(6);
            this.Abs200.Maximum = 100;
            this.Abs200.Name = "Abs200";
            this.Abs200.Size = new System.Drawing.Size(570, 28);
            this.Abs200.TabIndex = 16;
            this.Abs200.TickFrequency = 10;
            this.Abs200.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs200.Value = 1;
            this.Abs200.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs315
            // 
            this.Abs315.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs315.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs315, 2);
            this.Abs315.LargeChange = 10;
            this.Abs315.Location = new System.Drawing.Point(156, 682);
            this.Abs315.Margin = new System.Windows.Forms.Padding(6);
            this.Abs315.Maximum = 100;
            this.Abs315.Name = "Abs315";
            this.Abs315.Size = new System.Drawing.Size(570, 28);
            this.Abs315.TabIndex = 16;
            this.Abs315.TickFrequency = 10;
            this.Abs315.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs315.Value = 1;
            this.Abs315.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs400
            // 
            this.Abs400.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs400.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs400, 2);
            this.Abs400.LargeChange = 10;
            this.Abs400.Location = new System.Drawing.Point(156, 722);
            this.Abs400.Margin = new System.Windows.Forms.Padding(6);
            this.Abs400.Maximum = 100;
            this.Abs400.Name = "Abs400";
            this.Abs400.Size = new System.Drawing.Size(570, 28);
            this.Abs400.TabIndex = 16;
            this.Abs400.TickFrequency = 10;
            this.Abs400.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs400.Value = 1;
            this.Abs400.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs630
            // 
            this.Abs630.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs630.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs630, 2);
            this.Abs630.LargeChange = 10;
            this.Abs630.Location = new System.Drawing.Point(156, 802);
            this.Abs630.Margin = new System.Windows.Forms.Padding(6);
            this.Abs630.Maximum = 100;
            this.Abs630.Name = "Abs630";
            this.Abs630.Size = new System.Drawing.Size(570, 28);
            this.Abs630.TabIndex = 16;
            this.Abs630.TickFrequency = 10;
            this.Abs630.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs630.Value = 1;
            this.Abs630.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs800
            // 
            this.Abs800.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs800.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs800, 2);
            this.Abs800.LargeChange = 10;
            this.Abs800.Location = new System.Drawing.Point(156, 842);
            this.Abs800.Margin = new System.Windows.Forms.Padding(6);
            this.Abs800.Maximum = 100;
            this.Abs800.Name = "Abs800";
            this.Abs800.Size = new System.Drawing.Size(570, 28);
            this.Abs800.TabIndex = 16;
            this.Abs800.TickFrequency = 10;
            this.Abs800.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs800.Value = 1;
            this.Abs800.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs1250
            // 
            this.Abs1250.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1250.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs1250, 2);
            this.Abs1250.LargeChange = 10;
            this.Abs1250.Location = new System.Drawing.Point(156, 922);
            this.Abs1250.Margin = new System.Windows.Forms.Padding(6);
            this.Abs1250.Maximum = 100;
            this.Abs1250.Name = "Abs1250";
            this.Abs1250.Size = new System.Drawing.Size(570, 28);
            this.Abs1250.TabIndex = 16;
            this.Abs1250.TickFrequency = 10;
            this.Abs1250.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs1250.Value = 1;
            this.Abs1250.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs1600
            // 
            this.Abs1600.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1600.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs1600, 2);
            this.Abs1600.LargeChange = 10;
            this.Abs1600.Location = new System.Drawing.Point(156, 962);
            this.Abs1600.Margin = new System.Windows.Forms.Padding(6);
            this.Abs1600.Maximum = 100;
            this.Abs1600.Name = "Abs1600";
            this.Abs1600.Size = new System.Drawing.Size(570, 28);
            this.Abs1600.TabIndex = 16;
            this.Abs1600.TickFrequency = 10;
            this.Abs1600.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs1600.Value = 1;
            this.Abs1600.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs2500
            // 
            this.Abs2500.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs2500.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs2500, 2);
            this.Abs2500.LargeChange = 10;
            this.Abs2500.Location = new System.Drawing.Point(156, 1042);
            this.Abs2500.Margin = new System.Windows.Forms.Padding(6);
            this.Abs2500.Maximum = 100;
            this.Abs2500.Name = "Abs2500";
            this.Abs2500.Size = new System.Drawing.Size(570, 28);
            this.Abs2500.TabIndex = 16;
            this.Abs2500.TickFrequency = 10;
            this.Abs2500.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs2500.Value = 1;
            this.Abs2500.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs3150
            // 
            this.Abs3150.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs3150.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs3150, 2);
            this.Abs3150.LargeChange = 10;
            this.Abs3150.Location = new System.Drawing.Point(156, 1082);
            this.Abs3150.Margin = new System.Windows.Forms.Padding(6);
            this.Abs3150.Maximum = 100;
            this.Abs3150.Name = "Abs3150";
            this.Abs3150.Size = new System.Drawing.Size(570, 28);
            this.Abs3150.TabIndex = 16;
            this.Abs3150.TickFrequency = 10;
            this.Abs3150.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs3150.Value = 1;
            this.Abs3150.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs5k
            // 
            this.Abs5k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs5k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs5k, 2);
            this.Abs5k.LargeChange = 10;
            this.Abs5k.Location = new System.Drawing.Point(156, 1162);
            this.Abs5k.Margin = new System.Windows.Forms.Padding(6);
            this.Abs5k.Maximum = 100;
            this.Abs5k.Name = "Abs5k";
            this.Abs5k.Size = new System.Drawing.Size(570, 28);
            this.Abs5k.TabIndex = 16;
            this.Abs5k.TickFrequency = 10;
            this.Abs5k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs5k.Value = 1;
            this.Abs5k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs6300
            // 
            this.Abs6300.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs6300.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs6300, 2);
            this.Abs6300.LargeChange = 10;
            this.Abs6300.Location = new System.Drawing.Point(156, 1202);
            this.Abs6300.Margin = new System.Windows.Forms.Padding(6);
            this.Abs6300.Maximum = 100;
            this.Abs6300.Name = "Abs6300";
            this.Abs6300.Size = new System.Drawing.Size(570, 28);
            this.Abs6300.TabIndex = 16;
            this.Abs6300.TickFrequency = 10;
            this.Abs6300.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs6300.Value = 1;
            this.Abs6300.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs10k
            // 
            this.Abs10k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs10k.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Abs_Table.SetColumnSpan(this.Abs10k, 2);
            this.Abs10k.LargeChange = 10;
            this.Abs10k.Location = new System.Drawing.Point(156, 1282);
            this.Abs10k.Margin = new System.Windows.Forms.Padding(6);
            this.Abs10k.Maximum = 100;
            this.Abs10k.Name = "Abs10k";
            this.Abs10k.Size = new System.Drawing.Size(570, 28);
            this.Abs10k.TabIndex = 16;
            this.Abs10k.TickFrequency = 10;
            this.Abs10k.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Abs10k.Value = 1;
            this.Abs10k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Abs50Out
            // 
            this.Abs50Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs50Out.AutoSize = true;
            this.Abs50Out.Location = new System.Drawing.Point(738, 362);
            this.Abs50Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs50Out.Name = "Abs50Out";
            this.Abs50Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs50Out.Size = new System.Drawing.Size(108, 31);
            this.Abs50Out.TabIndex = 26;
            this.Abs50Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs50Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs80Out
            // 
            this.Abs80Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs80Out.AutoSize = true;
            this.Abs80Out.Location = new System.Drawing.Point(738, 442);
            this.Abs80Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs80Out.Name = "Abs80Out";
            this.Abs80Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs80Out.Size = new System.Drawing.Size(108, 31);
            this.Abs80Out.TabIndex = 26;
            this.Abs80Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs80Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs100Out
            // 
            this.Abs100Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs100Out.AutoSize = true;
            this.Abs100Out.Location = new System.Drawing.Point(738, 482);
            this.Abs100Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs100Out.Name = "Abs100Out";
            this.Abs100Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs100Out.Size = new System.Drawing.Size(108, 31);
            this.Abs100Out.TabIndex = 26;
            this.Abs100Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs100Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs160Out
            // 
            this.Abs160Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs160Out.AutoSize = true;
            this.Abs160Out.Location = new System.Drawing.Point(738, 562);
            this.Abs160Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs160Out.Name = "Abs160Out";
            this.Abs160Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs160Out.Size = new System.Drawing.Size(108, 31);
            this.Abs160Out.TabIndex = 26;
            this.Abs160Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs160Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs200Out
            // 
            this.Abs200Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs200Out.AutoSize = true;
            this.Abs200Out.Location = new System.Drawing.Point(738, 602);
            this.Abs200Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs200Out.Name = "Abs200Out";
            this.Abs200Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs200Out.Size = new System.Drawing.Size(108, 31);
            this.Abs200Out.TabIndex = 26;
            this.Abs200Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs200Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs315Out
            // 
            this.Abs315Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs315Out.AutoSize = true;
            this.Abs315Out.Location = new System.Drawing.Point(738, 682);
            this.Abs315Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs315Out.Name = "Abs315Out";
            this.Abs315Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs315Out.Size = new System.Drawing.Size(108, 31);
            this.Abs315Out.TabIndex = 26;
            this.Abs315Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs315Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs400Out
            // 
            this.Abs400Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs400Out.AutoSize = true;
            this.Abs400Out.Location = new System.Drawing.Point(738, 722);
            this.Abs400Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs400Out.Name = "Abs400Out";
            this.Abs400Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs400Out.Size = new System.Drawing.Size(108, 31);
            this.Abs400Out.TabIndex = 26;
            this.Abs400Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs400Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs630Out
            // 
            this.Abs630Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs630Out.AutoSize = true;
            this.Abs630Out.Location = new System.Drawing.Point(738, 802);
            this.Abs630Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs630Out.Name = "Abs630Out";
            this.Abs630Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs630Out.Size = new System.Drawing.Size(108, 31);
            this.Abs630Out.TabIndex = 26;
            this.Abs630Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs630Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs800Out
            // 
            this.Abs800Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs800Out.AutoSize = true;
            this.Abs800Out.Location = new System.Drawing.Point(738, 842);
            this.Abs800Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs800Out.Name = "Abs800Out";
            this.Abs800Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs800Out.Size = new System.Drawing.Size(108, 31);
            this.Abs800Out.TabIndex = 26;
            this.Abs800Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs800Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs1250Out
            // 
            this.Abs1250Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1250Out.AutoSize = true;
            this.Abs1250Out.Location = new System.Drawing.Point(738, 922);
            this.Abs1250Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs1250Out.Name = "Abs1250Out";
            this.Abs1250Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs1250Out.Size = new System.Drawing.Size(108, 31);
            this.Abs1250Out.TabIndex = 26;
            this.Abs1250Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs1250Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs1600Out
            // 
            this.Abs1600Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs1600Out.AutoSize = true;
            this.Abs1600Out.Location = new System.Drawing.Point(738, 962);
            this.Abs1600Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs1600Out.Name = "Abs1600Out";
            this.Abs1600Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs1600Out.Size = new System.Drawing.Size(108, 31);
            this.Abs1600Out.TabIndex = 26;
            this.Abs1600Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs1600Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs2500Out
            // 
            this.Abs2500Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs2500Out.AutoSize = true;
            this.Abs2500Out.Location = new System.Drawing.Point(738, 1042);
            this.Abs2500Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs2500Out.Name = "Abs2500Out";
            this.Abs2500Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs2500Out.Size = new System.Drawing.Size(108, 31);
            this.Abs2500Out.TabIndex = 26;
            this.Abs2500Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs2500Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs3150Out
            // 
            this.Abs3150Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs3150Out.AutoSize = true;
            this.Abs3150Out.Location = new System.Drawing.Point(738, 1082);
            this.Abs3150Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs3150Out.Name = "Abs3150Out";
            this.Abs3150Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs3150Out.Size = new System.Drawing.Size(108, 31);
            this.Abs3150Out.TabIndex = 26;
            this.Abs3150Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs3150Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs5kOut
            // 
            this.Abs5kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs5kOut.AutoSize = true;
            this.Abs5kOut.Location = new System.Drawing.Point(738, 1162);
            this.Abs5kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Abs5kOut.Name = "Abs5kOut";
            this.Abs5kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs5kOut.Size = new System.Drawing.Size(108, 31);
            this.Abs5kOut.TabIndex = 26;
            this.Abs5kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs5kOut.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs6300Out
            // 
            this.Abs6300Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs6300Out.AutoSize = true;
            this.Abs6300Out.Location = new System.Drawing.Point(738, 1202);
            this.Abs6300Out.Margin = new System.Windows.Forms.Padding(6);
            this.Abs6300Out.Name = "Abs6300Out";
            this.Abs6300Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs6300Out.Size = new System.Drawing.Size(108, 31);
            this.Abs6300Out.TabIndex = 26;
            this.Abs6300Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs6300Out.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // Abs10kOut
            // 
            this.Abs10kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Abs10kOut.AutoSize = true;
            this.Abs10kOut.Location = new System.Drawing.Point(738, 1282);
            this.Abs10kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Abs10kOut.Name = "Abs10kOut";
            this.Abs10kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Abs10kOut.Size = new System.Drawing.Size(108, 31);
            this.Abs10kOut.TabIndex = 26;
            this.Abs10kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Abs10kOut.ValueChanged += new System.EventHandler(this.AbsOut_ValueChanged);
            // 
            // ScatFlat
            // 
            this.ScatFlat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScatFlat.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ScatFlat.LargeChange = 10;
            this.ScatFlat.Location = new System.Drawing.Point(206, 637);
            this.ScatFlat.Margin = new System.Windows.Forms.Padding(6);
            this.ScatFlat.Maximum = 100;
            this.ScatFlat.Name = "ScatFlat";
            this.ScatFlat.Size = new System.Drawing.Size(500, 36);
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
            this.Scat8kOut.Location = new System.Drawing.Point(718, 589);
            this.Scat8kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Scat8kOut.Name = "Scat8kOut";
            this.Scat8kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat8kOut.Size = new System.Drawing.Size(108, 31);
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
            this.Scat8kv.Location = new System.Drawing.Point(206, 589);
            this.Scat8kv.Margin = new System.Windows.Forms.Padding(6);
            this.Scat8kv.Maximum = 100;
            this.Scat8kv.Name = "Scat8kv";
            this.Scat8kv.Size = new System.Drawing.Size(500, 36);
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
            this.Scat4kv.Location = new System.Drawing.Point(206, 541);
            this.Scat4kv.Margin = new System.Windows.Forms.Padding(6);
            this.Scat4kv.Maximum = 100;
            this.Scat4kv.Name = "Scat4kv";
            this.Scat4kv.Size = new System.Drawing.Size(500, 36);
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
            this.Scat4kOut.Location = new System.Drawing.Point(718, 541);
            this.Scat4kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Scat4kOut.Name = "Scat4kOut";
            this.Scat4kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat4kOut.Size = new System.Drawing.Size(108, 31);
            this.Scat4kOut.TabIndex = 44;
            this.Scat4kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat4kOut.ValueChanged += new System.EventHandler(this.Scat4kOut_ValueChanged);
            // 
            // Scat2kOut
            // 
            this.Scat2kOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat2kOut.Location = new System.Drawing.Point(718, 493);
            this.Scat2kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Scat2kOut.Name = "Scat2kOut";
            this.Scat2kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat2kOut.Size = new System.Drawing.Size(108, 31);
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
            this.Scat2kv.Location = new System.Drawing.Point(206, 493);
            this.Scat2kv.Margin = new System.Windows.Forms.Padding(6);
            this.Scat2kv.Maximum = 100;
            this.Scat2kv.Name = "Scat2kv";
            this.Scat2kv.Size = new System.Drawing.Size(500, 36);
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
            this.Scat1kv.Location = new System.Drawing.Point(206, 445);
            this.Scat1kv.Margin = new System.Windows.Forms.Padding(6);
            this.Scat1kv.Maximum = 100;
            this.Scat1kv.Name = "Scat1kv";
            this.Scat1kv.Size = new System.Drawing.Size(500, 36);
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
            this.Scat1kOut.Location = new System.Drawing.Point(718, 445);
            this.Scat1kOut.Margin = new System.Windows.Forms.Padding(6);
            this.Scat1kOut.Name = "Scat1kOut";
            this.Scat1kOut.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat1kOut.Size = new System.Drawing.Size(108, 31);
            this.Scat1kOut.TabIndex = 42;
            this.Scat1kOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat1kOut.ValueChanged += new System.EventHandler(this.Scat1kOut_ValueChanged);
            // 
            // Scat500Out
            // 
            this.Scat500Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat500Out.Location = new System.Drawing.Point(718, 397);
            this.Scat500Out.Margin = new System.Windows.Forms.Padding(6);
            this.Scat500Out.Name = "Scat500Out";
            this.Scat500Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat500Out.Size = new System.Drawing.Size(108, 31);
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
            this.Scat500v.Location = new System.Drawing.Point(206, 397);
            this.Scat500v.Margin = new System.Windows.Forms.Padding(6);
            this.Scat500v.Maximum = 100;
            this.Scat500v.Name = "Scat500v";
            this.Scat500v.Size = new System.Drawing.Size(500, 36);
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
            this.Scat250v.Location = new System.Drawing.Point(206, 349);
            this.Scat250v.Margin = new System.Windows.Forms.Padding(6);
            this.Scat250v.Maximum = 100;
            this.Scat250v.Name = "Scat250v";
            this.Scat250v.Size = new System.Drawing.Size(500, 36);
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
            this.Scat250Out.Location = new System.Drawing.Point(718, 349);
            this.Scat250Out.Margin = new System.Windows.Forms.Padding(6);
            this.Scat250Out.Name = "Scat250Out";
            this.Scat250Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat250Out.Size = new System.Drawing.Size(108, 31);
            this.Scat250Out.TabIndex = 40;
            this.Scat250Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat250Out.ValueChanged += new System.EventHandler(this.Scat250Out_ValueChanged);
            // 
            // Scat125Out
            // 
            this.Scat125Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Scat125Out.Location = new System.Drawing.Point(718, 301);
            this.Scat125Out.Margin = new System.Windows.Forms.Padding(6);
            this.Scat125Out.Name = "Scat125Out";
            this.Scat125Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat125Out.Size = new System.Drawing.Size(108, 31);
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
            this.Scat125v.Location = new System.Drawing.Point(206, 301);
            this.Scat125v.Margin = new System.Windows.Forms.Padding(6);
            this.Scat125v.Maximum = 100;
            this.Scat125v.Name = "Scat125v";
            this.Scat125v.Size = new System.Drawing.Size(500, 36);
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
            this.Scat63v.Location = new System.Drawing.Point(206, 253);
            this.Scat63v.Margin = new System.Windows.Forms.Padding(6);
            this.Scat63v.Maximum = 100;
            this.Scat63v.Name = "Scat63v";
            this.Scat63v.Size = new System.Drawing.Size(500, 36);
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
            this.Scat63Out.Location = new System.Drawing.Point(718, 253);
            this.Scat63Out.Margin = new System.Windows.Forms.Padding(6);
            this.Scat63Out.Name = "Scat63Out";
            this.Scat63Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Scat63Out.Size = new System.Drawing.Size(108, 31);
            this.Scat63Out.TabIndex = 38;
            this.Scat63Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Scat63Out.ValueChanged += new System.EventHandler(this.Scat63Out_ValueChanged);
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.tableLayoutPanel5.SetColumnSpan(this.label22, 3);
            this.label22.Location = new System.Drawing.Point(6, 222);
            this.label22.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(559, 25);
            this.label22.TabIndex = 30;
            this.label22.Text = "Scattering Coefficients (% non-specular reflected energy)";
            // 
            // SmartMat_Display
            // 
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
            this.SmartMat_Display.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.SmartMat_Display.Location = new System.Drawing.Point(6, 6);
            this.SmartMat_Display.Margin = new System.Windows.Forms.Padding(6);
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
            this.SmartMat_Display.Size = new System.Drawing.Size(852, 1382);
            this.SmartMat_Display.TabIndex = 45;
            this.SmartMat_Display.TabStop = false;
            this.SmartMat_Display.Text = "Absorption By Angle";
            // 
            // Calculate
            // 
            this.tableLayoutPanel7.SetColumnSpan(this.Calculate, 5);
            this.Calculate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Calculate.Location = new System.Drawing.Point(4, 513);
            this.Calculate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Calculate.Name = "Calculate";
            this.Calculate.Size = new System.Drawing.Size(884, 57);
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
            this.tableLayoutPanel7.SetColumnSpan(this.RTBox, 2);
            this.RTBox.Location = new System.Drawing.Point(6, 311);
            this.RTBox.Margin = new System.Windows.Forms.Padding(6);
            this.RTBox.Name = "RTBox";
            this.RTBox.Size = new System.Drawing.Size(231, 29);
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
            this.tableLayoutPanel7.SetColumnSpan(this.ISBox, 2);
            this.ISBox.Location = new System.Drawing.Point(6, 100);
            this.ISBox.Margin = new System.Windows.Forms.Padding(6);
            this.ISBox.Name = "ISBox";
            this.ISBox.Size = new System.Drawing.Size(233, 29);
            this.ISBox.TabIndex = 4;
            this.ISBox.Text = "Image Source Solution";
            this.ISBox.UseVisualStyleBackColor = true;
            this.ISBox.CheckedChanged += new System.EventHandler(this.CalcType_CheckedChanged);
            // 
            // Tabs
            // 
            this.Tabs.AccessibleDescription = "";
            this.Tabs.AccessibleName = "";
            this.Tabs.Controls.Add(this.TabPage1);
            this.Tabs.Controls.Add(this.TabPage4);
            this.Tabs.Controls.Add(this.TabPage3);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 42);
            this.Tabs.Margin = new System.Windows.Forms.Padding(6);
            this.Tabs.MinimumSize = new System.Drawing.Size(800, 769);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(920, 1584);
            this.Tabs.TabIndex = 5;
            this.Tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.Tab_Selecting);
            // 
            // TabPage1
            // 
            this.TabPage1.AutoScroll = true;
            this.TabPage1.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.TabPage1.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.TabPage1.Controls.Add(this.tableLayoutPanel7);
            this.TabPage1.Location = new System.Drawing.Point(8, 39);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(6);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(6);
            this.TabPage1.Size = new System.Drawing.Size(904, 1537);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Impulse";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 5;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.Controls.Add(this.GroupBox4, 1, 12);
            this.tableLayoutPanel7.Controls.Add(this.Calculate, 1, 11);
            this.tableLayoutPanel7.Controls.Add(this.CO_TIME, 4, 10);
            this.tableLayoutPanel7.Controls.Add(this.Spec_Rays, 2, 8);
            this.tableLayoutPanel7.Controls.Add(this.ReceiverSelection, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.RT_Count, 4, 9);
            this.tableLayoutPanel7.Controls.Add(this.Label2, 1, 9);
            this.tableLayoutPanel7.Controls.Add(this.Spec_RayCount, 4, 5);
            this.tableLayoutPanel7.Controls.Add(this.label14, 4, 4);
            this.tableLayoutPanel7.Controls.Add(this.Image_Order, 4, 3);
            this.tableLayoutPanel7.Controls.Add(this.DetailedConvergence, 4, 8);
            this.tableLayoutPanel7.Controls.Add(this.Minimum_Convergence, 3, 8);
            this.tableLayoutPanel7.Controls.Add(this.ISBox, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.RTBox, 0, 7);
            this.tableLayoutPanel7.Controls.Add(this.Label1, 1, 3);
            this.tableLayoutPanel7.Controls.Add(this.BTM_ED, 1, 4);
            this.tableLayoutPanel7.Controls.Add(this.Specular_Trace, 1, 5);
            this.tableLayoutPanel7.Controls.Add(this.label4, 1, 8);
            this.tableLayoutPanel7.Controls.Add(this.COTime, 1, 10);
            this.tableLayoutPanel7.Controls.Add(this.Label17, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label65, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.panel1, 2, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 13;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(892, 1525);
            this.tableLayoutPanel7.TabIndex = 89;
            // 
            // GroupBox4
            // 
            this.tableLayoutPanel7.SetColumnSpan(this.GroupBox4, 5);
            this.GroupBox4.Controls.Add(this.EdgeFreq);
            this.GroupBox4.Controls.Add(this.Label21);
            this.GroupBox4.Controls.Add(this.Atten_Method);
            this.GroupBox4.Controls.Add(this.Label19);
            this.GroupBox4.Controls.Add(this.Air_Pressure);
            this.GroupBox4.Controls.Add(this.Label3);
            this.GroupBox4.Controls.Add(this.Rel_Humidity);
            this.GroupBox4.Controls.Add(this.AirTemp);
            this.GroupBox4.Controls.Add(this.Air_Temp);
            this.GroupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupBox4.Location = new System.Drawing.Point(6, 579);
            this.GroupBox4.Margin = new System.Windows.Forms.Padding(6);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Padding = new System.Windows.Forms.Padding(6);
            this.GroupBox4.Size = new System.Drawing.Size(880, 283);
            this.GroupBox4.TabIndex = 33;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Environmental Factors";
            // 
            // EdgeFreq
            // 
            this.EdgeFreq.AutoSize = true;
            this.EdgeFreq.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EdgeFreq.Location = new System.Drawing.Point(140, 236);
            this.EdgeFreq.Margin = new System.Windows.Forms.Padding(6);
            this.EdgeFreq.Name = "EdgeFreq";
            this.EdgeFreq.Size = new System.Drawing.Size(307, 29);
            this.EdgeFreq.TabIndex = 35;
            this.EdgeFreq.Text = "Edge Frequency Correction";
            this.EdgeFreq.UseVisualStyleBackColor = true;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(16, 190);
            this.Label21.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(90, 25);
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
            this.Atten_Method.Location = new System.Drawing.Point(140, 185);
            this.Atten_Method.Margin = new System.Windows.Forms.Padding(6);
            this.Atten_Method.Name = "Atten_Method";
            this.Atten_Method.Size = new System.Drawing.Size(717, 33);
            this.Atten_Method.TabIndex = 14;
            this.Atten_Method.Text = "ISO 9613-1 (Outdoor Attenuation)";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(16, 139);
            this.Label19.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(248, 25);
            this.Label19.TabIndex = 32;
            this.Label19.Text = "Static Air Pressure (hPa)";
            // 
            // Air_Pressure
            // 
            this.Air_Pressure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Pressure.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Pressure.Location = new System.Drawing.Point(732, 135);
            this.Air_Pressure.Margin = new System.Windows.Forms.Padding(6);
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
            this.Air_Pressure.Size = new System.Drawing.Size(128, 31);
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
            this.Label3.Location = new System.Drawing.Point(16, 89);
            this.Label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(212, 25);
            this.Label3.TabIndex = 30;
            this.Label3.Text = "Relative Humidity(%)";
            // 
            // Rel_Humidity
            // 
            this.Rel_Humidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Rel_Humidity.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Rel_Humidity.Location = new System.Drawing.Point(797, 85);
            this.Rel_Humidity.Margin = new System.Windows.Forms.Padding(6);
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
            this.Rel_Humidity.Size = new System.Drawing.Size(64, 31);
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
            this.AirTemp.Location = new System.Drawing.Point(16, 39);
            this.AirTemp.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.AirTemp.Name = "AirTemp";
            this.AirTemp.Size = new System.Drawing.Size(201, 25);
            this.AirTemp.TabIndex = 28;
            this.AirTemp.Text = "Air Temperature (C)";
            // 
            // Air_Temp
            // 
            this.Air_Temp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Air_Temp.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Air_Temp.Location = new System.Drawing.Point(797, 35);
            this.Air_Temp.Margin = new System.Windows.Forms.Padding(6);
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
            this.Air_Temp.Size = new System.Drawing.Size(64, 31);
            this.Air_Temp.TabIndex = 11;
            this.Air_Temp.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // CO_TIME
            // 
            this.CO_TIME.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CO_TIME.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.CO_TIME.Location = new System.Drawing.Point(758, 452);
            this.CO_TIME.Margin = new System.Windows.Forms.Padding(6);
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
            this.CO_TIME.Size = new System.Drawing.Size(128, 31);
            this.CO_TIME.TabIndex = 10;
            this.CO_TIME.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // Spec_Rays
            // 
            this.Spec_Rays.AutoSize = true;
            this.Spec_Rays.Location = new System.Drawing.Point(249, 357);
            this.Spec_Rays.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Spec_Rays.Name = "Spec_Rays";
            this.Spec_Rays.Size = new System.Drawing.Size(207, 29);
            this.Spec_Rays.TabIndex = 88;
            this.Spec_Rays.Text = "Specify Ray Count";
            this.Spec_Rays.UseVisualStyleBackColor = true;
            this.Spec_Rays.CheckedChanged += new System.EventHandler(this.Convergence_CheckedChanged);
            // 
            // ReceiverSelection
            // 
            this.ReceiverSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel7.SetColumnSpan(this.ReceiverSelection, 3);
            this.ReceiverSelection.FormattingEnabled = true;
            this.ReceiverSelection.Items.AddRange(new object[] {
            "1 m. Stationary Receiver",
            "Expanding Receiver (Expanding)"});
            this.ReceiverSelection.Location = new System.Drawing.Point(251, 6);
            this.ReceiverSelection.Margin = new System.Windows.Forms.Padding(6);
            this.ReceiverSelection.Name = "ReceiverSelection";
            this.ReceiverSelection.Size = new System.Drawing.Size(635, 33);
            this.ReceiverSelection.TabIndex = 3;
            this.ReceiverSelection.Text = "1 m. Stationary Receiver";
            // 
            // RT_Count
            // 
            this.RT_Count.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RT_Count.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.RT_Count.Enabled = false;
            this.RT_Count.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RT_Count.Location = new System.Drawing.Point(758, 405);
            this.RT_Count.Margin = new System.Windows.Forms.Padding(6);
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
            this.RT_Count.Size = new System.Drawing.Size(128, 31);
            this.RT_Count.TabIndex = 9;
            this.RT_Count.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(36, 399);
            this.Label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(166, 25);
            this.Label2.TabIndex = 13;
            this.Label2.Text = "Number of Rays";
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
            this.Spec_RayCount.Location = new System.Drawing.Point(758, 249);
            this.Spec_RayCount.Margin = new System.Windows.Forms.Padding(6);
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
            this.Spec_RayCount.Size = new System.Drawing.Size(128, 31);
            this.Spec_RayCount.TabIndex = 7;
            this.Spec_RayCount.Value = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Red;
            this.label14.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label14.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label14.Location = new System.Drawing.Point(751, 207);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(137, 25);
            this.label14.TabIndex = 85;
            this.label14.Text = "Experimental";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Image_Order
            // 
            this.Image_Order.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Image_Order.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Image_Order.Location = new System.Drawing.Point(796, 155);
            this.Image_Order.Margin = new System.Windows.Forms.Padding(6);
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
            this.Image_Order.Size = new System.Drawing.Size(90, 31);
            this.Image_Order.TabIndex = 5;
            this.Image_Order.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // DetailedConvergence
            // 
            this.DetailedConvergence.AutoSize = true;
            this.DetailedConvergence.Location = new System.Drawing.Point(679, 357);
            this.DetailedConvergence.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DetailedConvergence.Name = "DetailedConvergence";
            this.DetailedConvergence.Size = new System.Drawing.Size(209, 29);
            this.DetailedConvergence.TabIndex = 87;
            this.DetailedConvergence.Text = "Detailed Convergence";
            this.DetailedConvergence.UseVisualStyleBackColor = true;
            this.DetailedConvergence.CheckedChanged += new System.EventHandler(this.Convergence_CheckedChanged);
            // 
            // Minimum_Convergence
            // 
            this.Minimum_Convergence.AutoSize = true;
            this.Minimum_Convergence.Checked = true;
            this.Minimum_Convergence.Location = new System.Drawing.Point(464, 357);
            this.Minimum_Convergence.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Minimum_Convergence.Name = "Minimum_Convergence";
            this.Minimum_Convergence.Size = new System.Drawing.Size(207, 29);
            this.Minimum_Convergence.TabIndex = 86;
            this.Minimum_Convergence.TabStop = true;
            this.Minimum_Convergence.Text = "Minimum Convergence";
            this.Minimum_Convergence.UseVisualStyleBackColor = true;
            this.Minimum_Convergence.CheckedChanged += new System.EventHandler(this.Convergence_CheckedChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(36, 149);
            this.Label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(168, 25);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Reflection Order";
            // 
            // BTM_ED
            // 
            this.BTM_ED.AutoSize = true;
            this.BTM_ED.Location = new System.Drawing.Point(36, 202);
            this.BTM_ED.Margin = new System.Windows.Forms.Padding(6);
            this.BTM_ED.Name = "BTM_ED";
            this.BTM_ED.Size = new System.Drawing.Size(203, 29);
            this.BTM_ED.TabIndex = 34;
            this.BTM_ED.Text = "BTM Edge Diffraction";
            this.BTM_ED.UseVisualStyleBackColor = true;
            // 
            // Specular_Trace
            // 
            this.Specular_Trace.AutoSize = true;
            this.Specular_Trace.Enabled = false;
            this.Specular_Trace.Location = new System.Drawing.Point(36, 249);
            this.Specular_Trace.Margin = new System.Windows.Forms.Padding(6);
            this.Specular_Trace.Name = "Specular_Trace";
            this.Specular_Trace.Size = new System.Drawing.Size(203, 29);
            this.Specular_Trace.TabIndex = 6;
            this.Specular_Trace.Text = "Image Source Tracing";
            this.Specular_Trace.UseVisualStyleBackColor = true;
            this.Specular_Trace.CheckedChanged += new System.EventHandler(this.CalcType_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 352);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 25);
            this.label4.TabIndex = 90;
            this.label4.Text = "Convergence:";
            // 
            // COTime
            // 
            this.COTime.AutoSize = true;
            this.COTime.Location = new System.Drawing.Point(36, 446);
            this.COTime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.COTime.Name = "COTime";
            this.COTime.Size = new System.Drawing.Size(180, 25);
            this.COTime.TabIndex = 17;
            this.COTime.Text = "Cut Off Time (ms)";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.tableLayoutPanel7.SetColumnSpan(this.Label17, 2);
            this.Label17.Location = new System.Drawing.Point(6, 0);
            this.Label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(103, 25);
            this.Label17.TabIndex = 31;
            this.Label17.Text = "Receiver:";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.tableLayoutPanel7.SetColumnSpan(this.label65, 2);
            this.label65.Location = new System.Drawing.Point(6, 47);
            this.label65.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(187, 25);
            this.label65.TabIndex = 31;
            this.label65.Text = "Frequency Bands:";
            // 
            // panel1
            // 
            this.tableLayoutPanel7.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.OctaveBand);
            this.panel1.Controls.Add(this.ThirdOctaveBand);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(248, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(641, 41);
            this.panel1.TabIndex = 93;
            // 
            // OctaveBand
            // 
            this.OctaveBand.AutoSize = true;
            this.OctaveBand.Location = new System.Drawing.Point(3, 3);
            this.OctaveBand.Name = "OctaveBand";
            this.OctaveBand.Size = new System.Drawing.Size(147, 29);
            this.OctaveBand.TabIndex = 91;
            this.OctaveBand.Text = "1/1 Octave";
            this.OctaveBand.UseVisualStyleBackColor = true;
            // 
            // ThirdOctaveBand
            // 
            this.ThirdOctaveBand.AutoSize = true;
            this.ThirdOctaveBand.Checked = true;
            this.ThirdOctaveBand.Location = new System.Drawing.Point(186, 3);
            this.ThirdOctaveBand.Name = "ThirdOctaveBand";
            this.ThirdOctaveBand.Size = new System.Drawing.Size(147, 29);
            this.ThirdOctaveBand.TabIndex = 92;
            this.ThirdOctaveBand.TabStop = true;
            this.ThirdOctaveBand.Text = "1/3 Octave";
            this.ThirdOctaveBand.UseVisualStyleBackColor = true;
            // 
            // TabPage4
            // 
            this.TabPage4.Controls.Add(this.tableLayoutPanel2);
            this.TabPage4.Location = new System.Drawing.Point(8, 39);
            this.TabPage4.Margin = new System.Windows.Forms.Padding(6);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(6);
            this.TabPage4.Size = new System.Drawing.Size(904, 1537);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Materials";
            this.TabPage4.UseVisualStyleBackColor = true;
            this.TabPage4.MouseEnter += new System.EventHandler(this.Materials_MouseEnter);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.5F));
            this.tableLayoutPanel2.Controls.Add(this.LayerLbl, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.LayerDisplay, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.734577F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.26543F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(892, 1525);
            this.tableLayoutPanel2.TabIndex = 46;
            // 
            // LayerLbl
            // 
            this.LayerLbl.AutoSize = true;
            this.LayerLbl.Location = new System.Drawing.Point(6, 0);
            this.LayerLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LayerLbl.Name = "LayerLbl";
            this.LayerLbl.Size = new System.Drawing.Size(110, 25);
            this.LayerLbl.TabIndex = 6;
            this.LayerLbl.Text = "For Layer:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.tabControl1, 2);
            this.tabControl1.Controls.Add(this.Absorption);
            this.tabControl1.Controls.Add(this.Scattering);
            this.tabControl1.Controls.Add(this.Transparency);
            this.tabControl1.Location = new System.Drawing.Point(6, 78);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(880, 1441);
            this.tabControl1.TabIndex = 47;
            // 
            // Absorption
            // 
            this.Absorption.Controls.Add(this.Abs_Table);
            this.Absorption.Controls.Add(this.SmartMat_Display);
            this.Absorption.Location = new System.Drawing.Point(8, 39);
            this.Absorption.Margin = new System.Windows.Forms.Padding(6);
            this.Absorption.Name = "Absorption";
            this.Absorption.Padding = new System.Windows.Forms.Padding(6);
            this.Absorption.Size = new System.Drawing.Size(864, 1394);
            this.Absorption.TabIndex = 0;
            this.Absorption.Text = "Absorption";
            this.Absorption.UseVisualStyleBackColor = true;
            // 
            // Scattering
            // 
            this.Scattering.Controls.Add(this.tableLayoutPanel5);
            this.Scattering.Location = new System.Drawing.Point(8, 39);
            this.Scattering.Margin = new System.Windows.Forms.Padding(6);
            this.Scattering.Name = "Scattering";
            this.Scattering.Padding = new System.Windows.Forms.Padding(6);
            this.Scattering.Size = new System.Drawing.Size(864, 1394);
            this.Scattering.TabIndex = 1;
            this.Scattering.Text = "Scattering";
            this.Scattering.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.quart_lambda, 2, 3);
            this.tableLayoutPanel5.Controls.Add(this.label16, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.user_quart_lambda, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.label37, 0, 13);
            this.tableLayoutPanel5.Controls.Add(this.ScatFlat, 1, 13);
            this.tableLayoutPanel5.Controls.Add(this.label38, 0, 12);
            this.tableLayoutPanel5.Controls.Add(this.PlasterScatter, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.Scat8kOut, 2, 12);
            this.tableLayoutPanel5.Controls.Add(this.GlassScatter, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label39, 0, 11);
            this.tableLayoutPanel5.Controls.Add(this.Scat8kv, 1, 12);
            this.tableLayoutPanel5.Controls.Add(this.label40, 0, 10);
            this.tableLayoutPanel5.Controls.Add(this.label41, 0, 9);
            this.tableLayoutPanel5.Controls.Add(this.label42, 0, 8);
            this.tableLayoutPanel5.Controls.Add(this.Scat4kOut, 2, 11);
            this.tableLayoutPanel5.Controls.Add(this.Scat4kv, 1, 11);
            this.tableLayoutPanel5.Controls.Add(this.label43, 0, 7);
            this.tableLayoutPanel5.Controls.Add(this.label44, 0, 6);
            this.tableLayoutPanel5.Controls.Add(this.Scat2kOut, 2, 10);
            this.tableLayoutPanel5.Controls.Add(this.label45, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.Scat2kv, 1, 10);
            this.tableLayoutPanel5.Controls.Add(this.label22, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.Scat63v, 1, 5);
            this.tableLayoutPanel5.Controls.Add(this.Scat63Out, 2, 5);
            this.tableLayoutPanel5.Controls.Add(this.Scat1kOut, 2, 9);
            this.tableLayoutPanel5.Controls.Add(this.Scat1kv, 1, 9);
            this.tableLayoutPanel5.Controls.Add(this.Scat125Out, 2, 6);
            this.tableLayoutPanel5.Controls.Add(this.Scat125v, 1, 6);
            this.tableLayoutPanel5.Controls.Add(this.Scat500Out, 2, 8);
            this.tableLayoutPanel5.Controls.Add(this.Scat250v, 1, 7);
            this.tableLayoutPanel5.Controls.Add(this.Scat500v, 1, 8);
            this.tableLayoutPanel5.Controls.Add(this.Scat250Out, 2, 7);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 15;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(852, 1382);
            this.tableLayoutPanel5.TabIndex = 46;
            // 
            // quart_lambda
            // 
            this.quart_lambda.AutoSize = true;
            this.tableLayoutPanel5.SetColumnSpan(this.quart_lambda, 2);
            this.quart_lambda.Dock = System.Windows.Forms.DockStyle.Right;
            this.quart_lambda.Location = new System.Drawing.Point(766, 146);
            this.quart_lambda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.quart_lambda.Name = "quart_lambda";
            this.quart_lambda.Size = new System.Drawing.Size(82, 73);
            this.quart_lambda.TabIndex = 50;
            this.quart_lambda.Text = "25 mm.";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.tableLayoutPanel5.SetColumnSpan(this.label16, 4);
            this.label16.Location = new System.Drawing.Point(4, 110);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(373, 25);
            this.label16.TabIndex = 51;
            this.label16.Text = "Variegation (characteristic dimension)";
            // 
            // user_quart_lambda
            // 
            this.user_quart_lambda.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.user_quart_lambda.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tableLayoutPanel5.SetColumnSpan(this.user_quart_lambda, 2);
            this.user_quart_lambda.LargeChange = 10;
            this.user_quart_lambda.Location = new System.Drawing.Point(6, 152);
            this.user_quart_lambda.Margin = new System.Windows.Forms.Padding(6);
            this.user_quart_lambda.Maximum = 700;
            this.user_quart_lambda.Name = "user_quart_lambda";
            this.user_quart_lambda.Size = new System.Drawing.Size(700, 61);
            this.user_quart_lambda.TabIndex = 49;
            this.user_quart_lambda.TickFrequency = 10;
            this.user_quart_lambda.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.user_quart_lambda.Value = 25;
            this.user_quart_lambda.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 631);
            this.label37.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(108, 48);
            this.label37.TabIndex = 38;
            this.label37.Text = "Flatten All";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(6, 583);
            this.label38.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(67, 48);
            this.label38.TabIndex = 24;
            this.label38.Text = "8 kHz";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlasterScatter
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.PlasterScatter, 4);
            this.PlasterScatter.Location = new System.Drawing.Point(4, 60);
            this.PlasterScatter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PlasterScatter.Name = "PlasterScatter";
            this.PlasterScatter.Size = new System.Drawing.Size(712, 40);
            this.PlasterScatter.TabIndex = 48;
            this.PlasterScatter.Text = "Flat (plaster/gypsum)";
            this.PlasterScatter.UseVisualStyleBackColor = true;
            this.PlasterScatter.Click += new System.EventHandler(this.PlasterScatter_Click);
            // 
            // GlassScatter
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.GlassScatter, 4);
            this.GlassScatter.Location = new System.Drawing.Point(4, 5);
            this.GlassScatter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GlassScatter.Name = "GlassScatter";
            this.GlassScatter.Size = new System.Drawing.Size(712, 37);
            this.GlassScatter.TabIndex = 47;
            this.GlassScatter.Text = "Flat (Glass)";
            this.GlassScatter.UseVisualStyleBackColor = true;
            this.GlassScatter.Click += new System.EventHandler(this.GlassScatter_Click);
            // 
            // label39
            // 
            this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(6, 535);
            this.label39.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(67, 48);
            this.label39.TabIndex = 22;
            this.label39.Text = "4 kHz";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(6, 487);
            this.label40.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(67, 48);
            this.label40.TabIndex = 21;
            this.label40.Text = "2 kHz";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(6, 439);
            this.label41.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(67, 48);
            this.label41.TabIndex = 20;
            this.label41.Text = "1 kHz";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label42
            // 
            this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(6, 391);
            this.label42.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(80, 48);
            this.label42.TabIndex = 19;
            this.label42.Text = "500 Hz";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(6, 343);
            this.label43.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(80, 48);
            this.label43.TabIndex = 25;
            this.label43.Text = "250 Hz";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label44
            // 
            this.label44.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(6, 295);
            this.label44.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(80, 48);
            this.label44.TabIndex = 18;
            this.label44.Text = "125 Hz";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(6, 247);
            this.label45.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(86, 48);
            this.label45.TabIndex = 17;
            this.label45.Text = "62.5 Hz";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Transparency
            // 
            this.Transparency.Controls.Add(this.tabControl2);
            this.Transparency.Location = new System.Drawing.Point(8, 39);
            this.Transparency.Margin = new System.Windows.Forms.Padding(6);
            this.Transparency.Name = "Transparency";
            this.Transparency.Size = new System.Drawing.Size(864, 1394);
            this.Transparency.TabIndex = 2;
            this.Transparency.Text = "Transparency";
            this.Transparency.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(864, 1394);
            this.tabControl2.TabIndex = 48;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel6);
            this.tabPage2.Location = new System.Drawing.Point(8, 39);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(848, 1347);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Transmission Coefficient";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
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
            this.tableLayoutPanel6.Location = new System.Drawing.Point(10, 13);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 11;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(816, 2116);
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
            this.Trans_Flat.Location = new System.Drawing.Point(206, 418);
            this.Trans_Flat.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_Flat.Maximum = 100;
            this.Trans_Flat.Name = "Trans_Flat";
            this.Trans_Flat.Size = new System.Drawing.Size(464, 36);
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
            this.Trans_8k_Out.Location = new System.Drawing.Point(682, 370);
            this.Trans_8k_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_8k_Out.Name = "Trans_8k_Out";
            this.Trans_8k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_8k_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_8k_Out.TabIndex = 45;
            this.Trans_8k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_8k_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 316);
            this.label29.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(67, 48);
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
            this.Trans_8kv.Location = new System.Drawing.Point(206, 370);
            this.Trans_8kv.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_8kv.Maximum = 100;
            this.Trans_8kv.Name = "Trans_8kv";
            this.Trans_8kv.Size = new System.Drawing.Size(464, 36);
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
            this.label30.Location = new System.Drawing.Point(6, 268);
            this.label30.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(67, 48);
            this.label30.TabIndex = 21;
            this.label30.Text = "2 kHz";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 220);
            this.label31.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(67, 48);
            this.label31.TabIndex = 20;
            this.label31.Text = "1 kHz";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(6, 172);
            this.label32.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(80, 48);
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
            this.Trans_4k_Out.Location = new System.Drawing.Point(682, 322);
            this.Trans_4k_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_4k_Out.Name = "Trans_4k_Out";
            this.Trans_4k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_4k_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_4k_Out.TabIndex = 44;
            this.Trans_4k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_4k_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_4kv
            // 
            this.Trans_4kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_4kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_4kv.Enabled = false;
            this.Trans_4kv.LargeChange = 10;
            this.Trans_4kv.Location = new System.Drawing.Point(206, 322);
            this.Trans_4kv.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_4kv.Maximum = 100;
            this.Trans_4kv.Name = "Trans_4kv";
            this.Trans_4kv.Size = new System.Drawing.Size(464, 36);
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
            this.label33.Location = new System.Drawing.Point(6, 124);
            this.label33.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(80, 48);
            this.label33.TabIndex = 25;
            this.label33.Text = "250 Hz";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(6, 76);
            this.label34.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(80, 48);
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
            this.Trans_2k_Out.Location = new System.Drawing.Point(682, 274);
            this.Trans_2k_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_2k_Out.Name = "Trans_2k_Out";
            this.Trans_2k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_2k_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_2k_Out.TabIndex = 43;
            this.Trans_2k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_2k_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 28);
            this.label35.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(86, 48);
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
            this.Trans_2kv.Location = new System.Drawing.Point(206, 274);
            this.Trans_2kv.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_2kv.Maximum = 100;
            this.Trans_2kv.Name = "Trans_2kv";
            this.Trans_2kv.Size = new System.Drawing.Size(464, 36);
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
            this.label36.Location = new System.Drawing.Point(6, 3);
            this.label36.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(627, 25);
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
            this.Trans_63v.Location = new System.Drawing.Point(206, 34);
            this.Trans_63v.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_63v.Maximum = 100;
            this.Trans_63v.Name = "Trans_63v";
            this.Trans_63v.Size = new System.Drawing.Size(464, 36);
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
            this.Trans_63_Out.Location = new System.Drawing.Point(682, 34);
            this.Trans_63_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_63_Out.Name = "Trans_63_Out";
            this.Trans_63_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_63_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_63_Out.TabIndex = 38;
            this.Trans_63_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_63_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_1k_Out
            // 
            this.Trans_1k_Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_1k_Out.Enabled = false;
            this.Trans_1k_Out.Location = new System.Drawing.Point(682, 226);
            this.Trans_1k_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_1k_Out.Name = "Trans_1k_Out";
            this.Trans_1k_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_1k_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_1k_Out.TabIndex = 42;
            this.Trans_1k_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_1k_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_1kv
            // 
            this.Trans_1kv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_1kv.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_1kv.Enabled = false;
            this.Trans_1kv.LargeChange = 10;
            this.Trans_1kv.Location = new System.Drawing.Point(206, 226);
            this.Trans_1kv.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_1kv.Maximum = 100;
            this.Trans_1kv.Name = "Trans_1kv";
            this.Trans_1kv.Size = new System.Drawing.Size(464, 36);
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
            this.Trans_125_Out.Location = new System.Drawing.Point(682, 82);
            this.Trans_125_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_125_Out.Name = "Trans_125_Out";
            this.Trans_125_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_125_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_125_Out.TabIndex = 39;
            this.Trans_125_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_125_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_125v
            // 
            this.Trans_125v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_125v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_125v.Enabled = false;
            this.Trans_125v.LargeChange = 10;
            this.Trans_125v.Location = new System.Drawing.Point(206, 82);
            this.Trans_125v.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_125v.Maximum = 100;
            this.Trans_125v.Name = "Trans_125v";
            this.Trans_125v.Size = new System.Drawing.Size(464, 36);
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
            this.Trans_500_Out.Location = new System.Drawing.Point(682, 178);
            this.Trans_500_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_500_Out.Name = "Trans_500_Out";
            this.Trans_500_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_500_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_500_Out.TabIndex = 41;
            this.Trans_500_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_500_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_250v
            // 
            this.Trans_250v.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trans_250v.BackColor = System.Drawing.SystemColors.HighlightText;
            this.Trans_250v.Enabled = false;
            this.Trans_250v.LargeChange = 10;
            this.Trans_250v.Location = new System.Drawing.Point(206, 130);
            this.Trans_250v.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_250v.Maximum = 100;
            this.Trans_250v.Name = "Trans_250v";
            this.Trans_250v.Size = new System.Drawing.Size(464, 36);
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
            this.Trans_500v.Location = new System.Drawing.Point(206, 178);
            this.Trans_500v.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_500v.Maximum = 100;
            this.Trans_500v.Name = "Trans_500v";
            this.Trans_500v.Size = new System.Drawing.Size(464, 36);
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
            this.Trans_250_Out.Location = new System.Drawing.Point(682, 130);
            this.Trans_250_Out.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_250_Out.Name = "Trans_250_Out";
            this.Trans_250_Out.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Trans_250_Out.Size = new System.Drawing.Size(108, 31);
            this.Trans_250_Out.TabIndex = 40;
            this.Trans_250_Out.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Trans_250_Out.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // Trans_Check
            // 
            this.Trans_Check.AutoSize = true;
            this.Trans_Check.Dock = System.Windows.Forms.DockStyle.Top;
            this.Trans_Check.Location = new System.Drawing.Point(206, 466);
            this.Trans_Check.Margin = new System.Windows.Forms.Padding(6);
            this.Trans_Check.Name = "Trans_Check";
            this.Trans_Check.Size = new System.Drawing.Size(464, 29);
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
            this.label28.Location = new System.Drawing.Point(6, 364);
            this.label28.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(67, 48);
            this.label28.TabIndex = 24;
            this.label28.Text = "8 kHz";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 412);
            this.label23.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(108, 48);
            this.label23.TabIndex = 38;
            this.label23.Text = "Flatten All";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel8);
            this.tabPage5.Location = new System.Drawing.Point(8, 39);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage5.Size = new System.Drawing.Size(848, 1347);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Transmission Loss";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 4;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 258F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel8.Controls.Add(this.label64, 2, 2);
            this.tableLayoutPanel8.Controls.Add(this.Isolation_Lib, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label63, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label53, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.TL_Check, 1, 11);
            this.tableLayoutPanel8.Controls.Add(this.TL8k, 1, 10);
            this.tableLayoutPanel8.Controls.Add(this.label54, 0, 10);
            this.tableLayoutPanel8.Controls.Add(this.label52, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.label51, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.label48, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.label46, 0, 9);
            this.tableLayoutPanel8.Controls.Add(this.label47, 0, 8);
            this.tableLayoutPanel8.Controls.Add(this.label49, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.label50, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.TL4k, 1, 9);
            this.tableLayoutPanel8.Controls.Add(this.TL2k, 1, 8);
            this.tableLayoutPanel8.Controls.Add(this.TL1k, 1, 7);
            this.tableLayoutPanel8.Controls.Add(this.TL500, 1, 6);
            this.tableLayoutPanel8.Controls.Add(this.TL250, 1, 5);
            this.tableLayoutPanel8.Controls.Add(this.TL125, 1, 4);
            this.tableLayoutPanel8.Controls.Add(this.TL63, 1, 3);
            this.tableLayoutPanel8.Controls.Add(this.label55, 2, 4);
            this.tableLayoutPanel8.Controls.Add(this.label56, 2, 3);
            this.tableLayoutPanel8.Controls.Add(this.label57, 2, 5);
            this.tableLayoutPanel8.Controls.Add(this.label58, 2, 6);
            this.tableLayoutPanel8.Controls.Add(this.label59, 2, 7);
            this.tableLayoutPanel8.Controls.Add(this.label60, 2, 8);
            this.tableLayoutPanel8.Controls.Add(this.label61, 2, 9);
            this.tableLayoutPanel8.Controls.Add(this.label62, 2, 10);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(4, 5);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 12;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(840, 1337);
            this.tableLayoutPanel8.TabIndex = 48;
            // 
            // label64
            // 
            this.label64.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label64.AutoSize = true;
            this.label64.BackColor = System.Drawing.Color.Red;
            this.label64.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label64.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label64.Location = new System.Drawing.Point(675, 222);
            this.label64.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(137, 25);
            this.label64.TabIndex = 86;
            this.label64.Text = "Experimental";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Isolation_Lib
            // 
            this.Isolation_Lib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Isolation_Lib.FormattingEnabled = true;
            this.Isolation_Lib.ItemHeight = 25;
            this.Isolation_Lib.Location = new System.Drawing.Point(6, 41);
            this.Isolation_Lib.Margin = new System.Windows.Forms.Padding(6);
            this.Isolation_Lib.Name = "Isolation_Lib";
            this.Isolation_Lib.ScrollAlwaysVisible = true;
            this.Isolation_Lib.Size = new System.Drawing.Size(246, 174);
            this.Isolation_Lib.TabIndex = 50;
            this.Isolation_Lib.Click += new System.EventHandler(this.Isolation_Lib_SelectedIndexChanged);
            this.Isolation_Lib.SelectedIndexChanged += new System.EventHandler(this.Isolation_Lib_SelectedIndexChanged);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(6, 0);
            this.label63.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(218, 25);
            this.label63.TabIndex = 49;
            this.label63.Text = "Transmission Library:";
            // 
            // groupBox1
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.groupBox1, 3);
            this.groupBox1.Controls.Add(this.DeleteAssembly);
            this.groupBox1.Controls.Add(this.SaveAssembly);
            this.groupBox1.Controls.Add(this.IsolationAssemblies);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(264, 6);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel8.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(570, 209);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Save Assembly Transmission Loss";
            // 
            // DeleteAssembly
            // 
            this.DeleteAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteAssembly.Location = new System.Drawing.Point(12, 140);
            this.DeleteAssembly.Margin = new System.Windows.Forms.Padding(6);
            this.DeleteAssembly.Name = "DeleteAssembly";
            this.DeleteAssembly.Size = new System.Drawing.Size(546, 44);
            this.DeleteAssembly.TabIndex = 2;
            this.DeleteAssembly.Text = "Delete Assembly";
            this.DeleteAssembly.UseVisualStyleBackColor = true;
            this.DeleteAssembly.Click += new System.EventHandler(this.Delete_Isolation_Click);
            // 
            // SaveAssembly
            // 
            this.SaveAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveAssembly.Location = new System.Drawing.Point(12, 85);
            this.SaveAssembly.Margin = new System.Windows.Forms.Padding(6);
            this.SaveAssembly.Name = "SaveAssembly";
            this.SaveAssembly.Size = new System.Drawing.Size(546, 44);
            this.SaveAssembly.TabIndex = 1;
            this.SaveAssembly.Text = "Save Assembly";
            this.SaveAssembly.UseVisualStyleBackColor = true;
            this.SaveAssembly.Click += new System.EventHandler(this.SaveIso_Click);
            // 
            // IsolationAssemblies
            // 
            this.IsolationAssemblies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IsolationAssemblies.Location = new System.Drawing.Point(12, 35);
            this.IsolationAssemblies.Margin = new System.Windows.Forms.Padding(6);
            this.IsolationAssemblies.Name = "IsolationAssemblies";
            this.IsolationAssemblies.Size = new System.Drawing.Size(542, 31);
            this.IsolationAssemblies.TabIndex = 0;
            // 
            // label53
            // 
            this.label53.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label53.AutoSize = true;
            this.tableLayoutPanel8.SetColumnSpan(this.label53, 2);
            this.label53.Location = new System.Drawing.Point(6, 223);
            this.label53.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(332, 25);
            this.label53.TabIndex = 30;
            this.label53.Text = "Transmission Loss (decibels lost)";
            // 
            // TL_Check
            // 
            this.TL_Check.AutoSize = true;
            this.TL_Check.Dock = System.Windows.Forms.DockStyle.Top;
            this.TL_Check.Location = new System.Drawing.Point(264, 638);
            this.TL_Check.Margin = new System.Windows.Forms.Padding(6);
            this.TL_Check.Name = "TL_Check";
            this.TL_Check.Size = new System.Drawing.Size(375, 29);
            this.TL_Check.TabIndex = 46;
            this.TL_Check.Text = "Transmissive Assembly";
            this.TL_Check.UseVisualStyleBackColor = true;
            this.TL_Check.CheckedChanged += new System.EventHandler(this.Trans_Check_CheckedChanged);
            // 
            // TL8k
            // 
            this.TL8k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL8k.DecimalPlaces = 2;
            this.TL8k.Enabled = false;
            this.TL8k.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL8k.Location = new System.Drawing.Point(264, 590);
            this.TL8k.Margin = new System.Windows.Forms.Padding(6);
            this.TL8k.Name = "TL8k";
            this.TL8k.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL8k.Size = new System.Drawing.Size(375, 31);
            this.TL8k.TabIndex = 45;
            this.TL8k.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL8k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label54
            // 
            this.label54.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(6, 584);
            this.label54.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(67, 48);
            this.label54.TabIndex = 24;
            this.label54.Text = "8 kHz";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label52
            // 
            this.label52.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(6, 248);
            this.label52.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(86, 48);
            this.label52.TabIndex = 17;
            this.label52.Text = "62.5 Hz";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label51
            // 
            this.label51.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(6, 296);
            this.label51.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(80, 48);
            this.label51.TabIndex = 18;
            this.label51.Text = "125 Hz";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label48
            // 
            this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(6, 440);
            this.label48.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(67, 48);
            this.label48.TabIndex = 20;
            this.label48.Text = "1 kHz";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label46
            // 
            this.label46.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(6, 536);
            this.label46.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(67, 48);
            this.label46.TabIndex = 22;
            this.label46.Text = "4 kHz";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label47
            // 
            this.label47.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(6, 488);
            this.label47.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(67, 48);
            this.label47.TabIndex = 21;
            this.label47.Text = "2 kHz";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label49
            // 
            this.label49.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(6, 392);
            this.label49.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(80, 48);
            this.label49.TabIndex = 19;
            this.label49.Text = "500 Hz";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label50
            // 
            this.label50.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(6, 344);
            this.label50.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(80, 48);
            this.label50.TabIndex = 25;
            this.label50.Text = "250 Hz";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TL4k
            // 
            this.TL4k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL4k.DecimalPlaces = 2;
            this.TL4k.Enabled = false;
            this.TL4k.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL4k.Location = new System.Drawing.Point(264, 542);
            this.TL4k.Margin = new System.Windows.Forms.Padding(6);
            this.TL4k.Name = "TL4k";
            this.TL4k.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL4k.Size = new System.Drawing.Size(375, 31);
            this.TL4k.TabIndex = 44;
            this.TL4k.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL4k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // TL2k
            // 
            this.TL2k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL2k.DecimalPlaces = 2;
            this.TL2k.Enabled = false;
            this.TL2k.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL2k.Location = new System.Drawing.Point(264, 494);
            this.TL2k.Margin = new System.Windows.Forms.Padding(6);
            this.TL2k.Name = "TL2k";
            this.TL2k.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL2k.Size = new System.Drawing.Size(375, 31);
            this.TL2k.TabIndex = 43;
            this.TL2k.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL2k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // TL1k
            // 
            this.TL1k.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL1k.DecimalPlaces = 2;
            this.TL1k.Enabled = false;
            this.TL1k.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL1k.Location = new System.Drawing.Point(264, 446);
            this.TL1k.Margin = new System.Windows.Forms.Padding(6);
            this.TL1k.Name = "TL1k";
            this.TL1k.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL1k.Size = new System.Drawing.Size(375, 31);
            this.TL1k.TabIndex = 42;
            this.TL1k.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL1k.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // TL500
            // 
            this.TL500.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL500.DecimalPlaces = 2;
            this.TL500.Enabled = false;
            this.TL500.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL500.Location = new System.Drawing.Point(264, 398);
            this.TL500.Margin = new System.Windows.Forms.Padding(6);
            this.TL500.Name = "TL500";
            this.TL500.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL500.Size = new System.Drawing.Size(375, 31);
            this.TL500.TabIndex = 41;
            this.TL500.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL500.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // TL250
            // 
            this.TL250.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL250.DecimalPlaces = 2;
            this.TL250.Enabled = false;
            this.TL250.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL250.Location = new System.Drawing.Point(264, 350);
            this.TL250.Margin = new System.Windows.Forms.Padding(6);
            this.TL250.Name = "TL250";
            this.TL250.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL250.Size = new System.Drawing.Size(375, 31);
            this.TL250.TabIndex = 40;
            this.TL250.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL250.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // TL125
            // 
            this.TL125.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL125.DecimalPlaces = 2;
            this.TL125.Enabled = false;
            this.TL125.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL125.Location = new System.Drawing.Point(264, 302);
            this.TL125.Margin = new System.Windows.Forms.Padding(6);
            this.TL125.Name = "TL125";
            this.TL125.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL125.Size = new System.Drawing.Size(375, 31);
            this.TL125.TabIndex = 39;
            this.TL125.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL125.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // TL63
            // 
            this.TL63.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TL63.DecimalPlaces = 2;
            this.TL63.Enabled = false;
            this.TL63.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TL63.Location = new System.Drawing.Point(264, 254);
            this.TL63.Margin = new System.Windows.Forms.Padding(6);
            this.TL63.Name = "TL63";
            this.TL63.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TL63.Size = new System.Drawing.Size(375, 31);
            this.TL63.TabIndex = 38;
            this.TL63.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TL63.ValueChanged += new System.EventHandler(this.Acoustics_Coef_Update);
            // 
            // label55
            // 
            this.label55.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(649, 307);
            this.label55.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(92, 25);
            this.label55.TabIndex = 47;
            this.label55.Text = "decibels";
            // 
            // label56
            // 
            this.label56.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(649, 259);
            this.label56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(92, 25);
            this.label56.TabIndex = 47;
            this.label56.Text = "decibels";
            // 
            // label57
            // 
            this.label57.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(649, 355);
            this.label57.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(92, 25);
            this.label57.TabIndex = 47;
            this.label57.Text = "decibels";
            // 
            // label58
            // 
            this.label58.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(649, 403);
            this.label58.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(92, 25);
            this.label58.TabIndex = 47;
            this.label58.Text = "decibels";
            // 
            // label59
            // 
            this.label59.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(649, 451);
            this.label59.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(92, 25);
            this.label59.TabIndex = 47;
            this.label59.Text = "decibels";
            // 
            // label60
            // 
            this.label60.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(649, 499);
            this.label60.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(92, 25);
            this.label60.TabIndex = 47;
            this.label60.Text = "decibels";
            // 
            // label61
            // 
            this.label61.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(649, 547);
            this.label61.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(92, 25);
            this.label61.TabIndex = 47;
            this.label61.Text = "decibels";
            // 
            // label62
            // 
            this.label62.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(649, 595);
            this.label62.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(92, 25);
            this.label62.TabIndex = 47;
            this.label62.Text = "decibels";
            // 
            // LayerDisplay
            // 
            this.LayerDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LayerDisplay.FormattingEnabled = true;
            this.LayerDisplay.Location = new System.Drawing.Point(286, 6);
            this.LayerDisplay.Margin = new System.Windows.Forms.Padding(6);
            this.LayerDisplay.MaxDropDownItems = 100;
            this.LayerDisplay.Name = "LayerDisplay";
            this.LayerDisplay.Size = new System.Drawing.Size(600, 33);
            this.LayerDisplay.TabIndex = 27;
            this.LayerDisplay.SelectedValueChanged += new System.EventHandler(this.Retrieve_Layer_Acoustics);
            // 
            // TabPage3
            // 
            this.TabPage3.AutoScroll = true;
            this.TabPage3.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.TabPage3.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.TabPage3.Controls.Add(this.tableLayoutPanel3);
            this.TabPage3.Location = new System.Drawing.Point(8, 39);
            this.TabPage3.Margin = new System.Windows.Forms.Padding(6);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(6);
            this.TabPage3.Size = new System.Drawing.Size(904, 1537);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Analysis";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.ColumnCount = 7;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.72973F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.27027F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.tableLayoutPanel3.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label25, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.SourceList, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.Source_Aim, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.GroupBox3, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.label20, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label27, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.Receiver_Choice, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.Analysis_View, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.label26, 5, 3);
            this.tableLayoutPanel3.Controls.Add(this.Normalize_Graph, 5, 7);
            this.tableLayoutPanel3.Controls.Add(this.Alt_Choice, 4, 3);
            this.tableLayoutPanel3.Controls.Add(this.LockUserScale, 3, 7);
            this.tableLayoutPanel3.Controls.Add(this.Azi_Choice, 6, 3);
            this.tableLayoutPanel3.Controls.Add(this.Graph_Type, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.Auralisation, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.Label5, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.Graph_Octave, 4, 6);
            this.tableLayoutPanel3.Controls.Add(this.IS_Path_Box, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.PathCount, 6, 4);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 10;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 187F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(892, 1525);
            this.tableLayoutPanel3.TabIndex = 43;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Enabled = false;
            this.label15.Location = new System.Drawing.Point(6, 0);
            this.label15.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(117, 25);
            this.label15.TabIndex = 45;
            this.label15.Text = "Source";
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label25, 2);
            this.label25.Enabled = false;
            this.label25.Location = new System.Drawing.Point(6, 264);
            this.label25.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(147, 25);
            this.label25.TabIndex = 46;
            this.label25.Text = "Aim at Source";
            // 
            // SourceList
            // 
            this.SourceList.CheckOnClick = true;
            this.tableLayoutPanel3.SetColumnSpan(this.SourceList, 3);
            this.SourceList.ContextMenuStrip = this.SourceContext;
            this.SourceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceList.FormattingEnabled = true;
            this.SourceList.Location = new System.Drawing.Point(6, 36);
            this.SourceList.Margin = new System.Windows.Forms.Padding(6);
            this.SourceList.MinimumSize = new System.Drawing.Size(4, 119);
            this.SourceList.Name = "SourceList";
            this.SourceList.ScrollAlwaysVisible = true;
            this.SourceList.Size = new System.Drawing.Size(312, 175);
            this.SourceList.TabIndex = 44;
            this.SourceList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SourceList_MouseUp);
            // 
            // SourceContext
            // 
            this.SourceContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SourceContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PowerMod,
            this.DelayMod});
            this.SourceContext.Name = "contextMenuStrip1";
            this.SourceContext.Size = new System.Drawing.Size(316, 80);
            // 
            // PowerMod
            // 
            this.PowerMod.Name = "PowerMod";
            this.PowerMod.Size = new System.Drawing.Size(315, 38);
            this.PowerMod.Text = "Modify Source Power";
            this.PowerMod.Click += new System.EventHandler(this.Source_Power_Mod_Click);
            // 
            // DelayMod
            // 
            this.DelayMod.Name = "DelayMod";
            this.DelayMod.Size = new System.Drawing.Size(315, 38);
            this.DelayMod.Text = "Modify Source Delay";
            this.DelayMod.Click += new System.EventHandler(this.DelayMod_Click);
            // 
            // Source_Aim
            // 
            this.Source_Aim.FormattingEnabled = true;
            this.Source_Aim.Location = new System.Drawing.Point(165, 270);
            this.Source_Aim.Margin = new System.Windows.Forms.Padding(6);
            this.Source_Aim.Name = "Source_Aim";
            this.Source_Aim.Size = new System.Drawing.Size(102, 33);
            this.Source_Aim.TabIndex = 51;
            this.Source_Aim.Text = "None";
            this.Source_Aim.SelectedIndexChanged += new System.EventHandler(this.Source_Aim_SelectedIndexChanged);
            // 
            // GroupBox3
            // 
            this.GroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.GroupBox3, 4);
            this.GroupBox3.Controls.Add(this.tableLayoutPanel1);
            this.GroupBox3.Location = new System.Drawing.Point(330, 6);
            this.GroupBox3.Margin = new System.Windows.Forms.Padding(6);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Padding = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel3.SetRowSpan(this.GroupBox3, 3);
            this.GroupBox3.Size = new System.Drawing.Size(556, 251);
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
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 30);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(544, 215);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // SRT8
            // 
            this.SRT8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT8.AutoSize = true;
            this.SRT8.Location = new System.Drawing.Point(278, 176);
            this.SRT8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT8.Name = "SRT8";
            this.SRT8.Size = new System.Drawing.Size(260, 39);
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
            "T-10",
            "T-15",
            "T-20",
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
            this.Parameter_Choice.Location = new System.Drawing.Point(6, 6);
            this.Parameter_Choice.Margin = new System.Windows.Forms.Padding(6);
            this.Parameter_Choice.Name = "Parameter_Choice";
            this.Parameter_Choice.Size = new System.Drawing.Size(260, 33);
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
            this.SRT7.Location = new System.Drawing.Point(278, 137);
            this.SRT7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT7.Name = "SRT7";
            this.SRT7.Size = new System.Drawing.Size(260, 39);
            this.SRT7.TabIndex = 14;
            this.SRT7.Text = "4000 hz:";
            // 
            // SRT6
            // 
            this.SRT6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT6.AutoSize = true;
            this.SRT6.Location = new System.Drawing.Point(278, 98);
            this.SRT6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT6.Name = "SRT6";
            this.SRT6.Size = new System.Drawing.Size(260, 39);
            this.SRT6.TabIndex = 13;
            this.SRT6.Text = "2000 hz:";
            // 
            // SRT2
            // 
            this.SRT2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT2.AutoSize = true;
            this.SRT2.Location = new System.Drawing.Point(6, 98);
            this.SRT2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT2.Name = "SRT2";
            this.SRT2.Size = new System.Drawing.Size(260, 39);
            this.SRT2.TabIndex = 9;
            this.SRT2.Text = "125 hz:";
            // 
            // SRT5
            // 
            this.SRT5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT5.AutoSize = true;
            this.SRT5.Location = new System.Drawing.Point(278, 59);
            this.SRT5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT5.Name = "SRT5";
            this.SRT5.Size = new System.Drawing.Size(260, 39);
            this.SRT5.TabIndex = 12;
            this.SRT5.Text = "1000 hz:";
            // 
            // SRT3
            // 
            this.SRT3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT3.AutoSize = true;
            this.SRT3.Location = new System.Drawing.Point(6, 137);
            this.SRT3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT3.Name = "SRT3";
            this.SRT3.Size = new System.Drawing.Size(260, 39);
            this.SRT3.TabIndex = 10;
            this.SRT3.Text = "250 hz:";
            // 
            // SRT4
            // 
            this.SRT4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT4.AutoSize = true;
            this.SRT4.Location = new System.Drawing.Point(6, 176);
            this.SRT4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT4.Name = "SRT4";
            this.SRT4.Size = new System.Drawing.Size(260, 39);
            this.SRT4.TabIndex = 11;
            this.SRT4.Text = "500 hz:";
            // 
            // SRT1
            // 
            this.SRT1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SRT1.AutoSize = true;
            this.SRT1.Location = new System.Drawing.Point(6, 59);
            this.SRT1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SRT1.Name = "SRT1";
            this.SRT1.Size = new System.Drawing.Size(260, 39);
            this.SRT1.TabIndex = 8;
            this.SRT1.Text = "62.5 hz:";
            // 
            // ISOCOMP
            // 
            this.ISOCOMP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ISOCOMP.AutoSize = true;
            this.ISOCOMP.Location = new System.Drawing.Point(330, 17);
            this.ISOCOMP.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ISOCOMP.Name = "ISOCOMP";
            this.ISOCOMP.Size = new System.Drawing.Size(156, 25);
            this.ISOCOMP.TabIndex = 28;
            this.ISOCOMP.Text = "ISO-Compliant:";
            this.ISOCOMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Enabled = false;
            this.label20.Location = new System.Drawing.Point(6, 217);
            this.label20.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(117, 25);
            this.label20.TabIndex = 40;
            this.label20.Text = "Receiver";
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Enabled = false;
            this.label27.Location = new System.Drawing.Point(330, 264);
            this.label27.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(81, 47);
            this.label27.TabIndex = 50;
            this.label27.Text = "Altitude";
            // 
            // Receiver_Choice
            // 
            this.Receiver_Choice.FormattingEnabled = true;
            this.Receiver_Choice.Location = new System.Drawing.Point(165, 223);
            this.Receiver_Choice.Margin = new System.Windows.Forms.Padding(6);
            this.Receiver_Choice.Name = "Receiver_Choice";
            this.Receiver_Choice.Size = new System.Drawing.Size(151, 33);
            this.Receiver_Choice.TabIndex = 39;
            this.Receiver_Choice.Text = "No Results Calculated...";
            this.Receiver_Choice.SelectedIndexChanged += new System.EventHandler(this.Receiver_Choice_SelectedIndexChanged);
            // 
            // Analysis_View
            // 
            this.Analysis_View.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Analysis_View.AutoSize = true;
            this.Analysis_View.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.Analysis_View, 7);
            this.Analysis_View.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.Analysis_View.Location = new System.Drawing.Point(12, 576);
            this.Analysis_View.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.Analysis_View.Name = "Analysis_View";
            this.Analysis_View.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.Analysis_View.ScrollGrace = 0D;
            this.Analysis_View.ScrollMaxX = 0D;
            this.Analysis_View.ScrollMaxY = 0D;
            this.Analysis_View.ScrollMaxY2 = 0D;
            this.Analysis_View.ScrollMinX = 0D;
            this.Analysis_View.ScrollMinY = 0D;
            this.Analysis_View.ScrollMinY2 = 0D;
            this.Analysis_View.Size = new System.Drawing.Size(868, 888);
            this.Analysis_View.TabIndex = 42;
            this.Analysis_View.UseExtendedPrintDialog = true;
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Enabled = false;
            this.label26.Location = new System.Drawing.Point(571, 264);
            this.label26.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(83, 47);
            this.label26.TabIndex = 49;
            this.label26.Text = "Azimuth";
            // 
            // Normalize_Graph
            // 
            this.Normalize_Graph.AutoSize = true;
            this.Normalize_Graph.Checked = true;
            this.Normalize_Graph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel3.SetColumnSpan(this.Normalize_Graph, 2);
            this.Normalize_Graph.Location = new System.Drawing.Point(571, 527);
            this.Normalize_Graph.Margin = new System.Windows.Forms.Padding(6);
            this.Normalize_Graph.Name = "Normalize_Graph";
            this.Normalize_Graph.Size = new System.Drawing.Size(233, 29);
            this.Normalize_Graph.TabIndex = 43;
            this.Normalize_Graph.Text = "Normalize To Direct";
            this.Normalize_Graph.UseVisualStyleBackColor = true;
            this.Normalize_Graph.CheckedChanged += new System.EventHandler(this.Normalize_Graph_CheckedChanged);
            // 
            // Alt_Choice
            // 
            this.Alt_Choice.DecimalPlaces = 2;
            this.Alt_Choice.Location = new System.Drawing.Point(423, 270);
            this.Alt_Choice.Margin = new System.Windows.Forms.Padding(6);
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
            this.Alt_Choice.Size = new System.Drawing.Size(120, 31);
            this.Alt_Choice.TabIndex = 48;
            this.Alt_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Alt_Choice.ValueChanged += new System.EventHandler(this.Alt_Choice_ValueChanged);
            // 
            // LockUserScale
            // 
            this.LockUserScale.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.LockUserScale, 2);
            this.LockUserScale.Location = new System.Drawing.Point(330, 527);
            this.LockUserScale.Margin = new System.Windows.Forms.Padding(6);
            this.LockUserScale.Name = "LockUserScale";
            this.LockUserScale.Size = new System.Drawing.Size(201, 29);
            this.LockUserScale.TabIndex = 44;
            this.LockUserScale.Text = "Lock User Scale";
            this.LockUserScale.UseVisualStyleBackColor = true;
            this.LockUserScale.CheckedChanged += new System.EventHandler(this.Update_Graph);
            // 
            // Azi_Choice
            // 
            this.Azi_Choice.DecimalPlaces = 2;
            this.Azi_Choice.Location = new System.Drawing.Point(666, 270);
            this.Azi_Choice.Margin = new System.Windows.Forms.Padding(6);
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
            this.Azi_Choice.Size = new System.Drawing.Size(120, 31);
            this.Azi_Choice.TabIndex = 47;
            this.Azi_Choice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Azi_Choice.ValueChanged += new System.EventHandler(this.Azi_Choice_ValueChanged);
            // 
            // Graph_Type
            // 
            this.Graph_Type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Graph_Type, 4);
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
            this.Graph_Type.Location = new System.Drawing.Point(6, 483);
            this.Graph_Type.Margin = new System.Windows.Forms.Padding(6);
            this.Graph_Type.Name = "Graph_Type";
            this.Graph_Type.Size = new System.Drawing.Size(405, 33);
            this.Graph_Type.TabIndex = 33;
            this.Graph_Type.Text = "Energy Time Curve";
            this.Graph_Type.TextChanged += new System.EventHandler(this.Update_Graph);
            // 
            // Auralisation
            // 
            this.Auralisation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Auralisation, 7);
            this.Auralisation.Location = new System.Drawing.Point(6, 1481);
            this.Auralisation.Margin = new System.Windows.Forms.Padding(6);
            this.Auralisation.Name = "Auralisation";
            this.Auralisation.Size = new System.Drawing.Size(880, 38);
            this.Auralisation.TabIndex = 45;
            this.Auralisation.Text = "Go To Auralizations";
            this.Auralisation.UseVisualStyleBackColor = true;
            this.Auralisation.Click += new System.EventHandler(this.Auralisation_Click);
            // 
            // Label5
            // 
            this.Label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label5.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.Label5, 4);
            this.Label5.Enabled = false;
            this.Label5.Location = new System.Drawing.Point(6, 311);
            this.Label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(405, 25);
            this.Label5.TabIndex = 1;
            this.Label5.Text = "Image Source Paths";
            // 
            // Graph_Octave
            // 
            this.Graph_Octave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.Graph_Octave, 3);
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
            this.Graph_Octave.Location = new System.Drawing.Point(423, 483);
            this.Graph_Octave.Margin = new System.Windows.Forms.Padding(6);
            this.Graph_Octave.Name = "Graph_Octave";
            this.Graph_Octave.Size = new System.Drawing.Size(463, 33);
            this.Graph_Octave.TabIndex = 33;
            this.Graph_Octave.Text = "Summation: All Octaves";
            this.Graph_Octave.TextChanged += new System.EventHandler(this.Update_Graph);
            // 
            // IS_Path_Box
            // 
            this.IS_Path_Box.CheckOnClick = true;
            this.tableLayoutPanel3.SetColumnSpan(this.IS_Path_Box, 7);
            this.IS_Path_Box.ContextMenuStrip = this.PathContext;
            this.IS_Path_Box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IS_Path_Box.FormattingEnabled = true;
            this.IS_Path_Box.Location = new System.Drawing.Point(6, 350);
            this.IS_Path_Box.Margin = new System.Windows.Forms.Padding(6);
            this.IS_Path_Box.MinimumSize = new System.Drawing.Size(4, 119);
            this.IS_Path_Box.Name = "IS_Path_Box";
            this.IS_Path_Box.ScrollAlwaysVisible = true;
            this.IS_Path_Box.Size = new System.Drawing.Size(880, 121);
            this.IS_Path_Box.TabIndex = 7;
            this.IS_Path_Box.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IS_Path_Box_MouseUp);
            // 
            // PathContext
            // 
            this.PathContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.PathContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripSeparator1,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.PathContext.Name = "contextMenuStrip1";
            this.PathContext.Size = new System.Drawing.Size(314, 170);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(313, 40);
            this.toolStripMenuItem5.Text = "Check All...";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.ISCheckAll_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(313, 40);
            this.toolStripMenuItem6.Text = "Uncheck All...";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.ISUncheckAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(310, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Checked = true;
            this.toolStripMenuItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(313, 40);
            this.toolStripMenuItem3.Text = "Sort by Arrival Time...";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.SortPaths);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(313, 40);
            this.toolStripMenuItem4.Text = "Sort by Order...";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.SortPaths);
            // 
            // PathCount
            // 
            this.PathCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PathCount.AutoSize = true;
            this.PathCount.Enabled = false;
            this.PathCount.Location = new System.Drawing.Point(777, 311);
            this.PathCount.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PathCount.Name = "PathCount";
            this.PathCount.Size = new System.Drawing.Size(109, 25);
            this.PathCount.TabIndex = 8;
            this.PathCount.Text = "Pending...";
            this.PathCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FromMeshSphereToolStripMenuItem,
            this.FromPointInputToolStripMenuItem});
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(426, 44);
            this.ToolStripMenuItem1.Text = "Omni-Directional Source...";
            // 
            // FromMeshSphereToolStripMenuItem
            // 
            this.FromMeshSphereToolStripMenuItem.Name = "FromMeshSphereToolStripMenuItem";
            this.FromMeshSphereToolStripMenuItem.Size = new System.Drawing.Size(343, 44);
            this.FromMeshSphereToolStripMenuItem.Text = "From MeshSphere";
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
            this.DirectionalSourceToolStripMenuItem.Size = new System.Drawing.Size(426, 44);
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
            this.SelectASphereToolStripMenuItem.Size = new System.Drawing.Size(226, 44);
            this.SelectASphereToolStripMenuItem.Text = "Select...";
            // 
            // FromSphereObjectToolStripMenuItem1
            // 
            this.FromSphereObjectToolStripMenuItem1.Name = "FromSphereObjectToolStripMenuItem1";
            this.FromSphereObjectToolStripMenuItem1.Size = new System.Drawing.Size(361, 44);
            this.FromSphereObjectToolStripMenuItem1.Text = "From Sphere Object";
            // 
            // FromPointInputToolStripMenuItem2
            // 
            this.FromPointInputToolStripMenuItem2.Name = "FromPointInputToolStripMenuItem2";
            this.FromPointInputToolStripMenuItem2.Size = new System.Drawing.Size(361, 44);
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
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDataToolStripMenuItem,
            this.saveDataToolStripMenuItem,
            this.saveParameterResultsToolStripMenuItem,
            this.savePTBFormatToolStripMenuItem,
            this.saveVRSpectraToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openDataToolStripMenuItem
            // 
            this.openDataToolStripMenuItem.Name = "openDataToolStripMenuItem";
            this.openDataToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.openDataToolStripMenuItem.Text = "Open Data...";
            this.openDataToolStripMenuItem.Click += new System.EventHandler(this.OpenDataToolStripMenuItem_Click);
            // 
            // saveDataToolStripMenuItem
            // 
            this.saveDataToolStripMenuItem.Name = "saveDataToolStripMenuItem";
            this.saveDataToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.saveDataToolStripMenuItem.Text = "Save Data";
            this.saveDataToolStripMenuItem.Click += new System.EventHandler(this.SaveDataToolStripMenuItem_Click);
            // 
            // saveParameterResultsToolStripMenuItem
            // 
            this.saveParameterResultsToolStripMenuItem.Name = "saveParameterResultsToolStripMenuItem";
            this.saveParameterResultsToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.saveParameterResultsToolStripMenuItem.Text = "Save Results";
            this.saveParameterResultsToolStripMenuItem.Click += new System.EventHandler(this.SaveResultsToolStripMenuItem_Click);
            // 
            // savePTBFormatToolStripMenuItem
            // 
            this.savePTBFormatToolStripMenuItem.Name = "savePTBFormatToolStripMenuItem";
            this.savePTBFormatToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.savePTBFormatToolStripMenuItem.Text = "Save PTB Format";
            this.savePTBFormatToolStripMenuItem.Click += new System.EventHandler(this.savePTBFormatToolStripMenuItem_Click);
            // 
            // saveVRSpectraToolStripMenuItem
            // 
            this.saveVRSpectraToolStripMenuItem.Name = "saveVRSpectraToolStripMenuItem";
            this.saveVRSpectraToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            // 
            // SP_menu
            // 
            this.SP_menu.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.SP_menu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.SP_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.SP_menu.Location = new System.Drawing.Point(0, 0);
            this.SP_menu.Name = "SP_menu";
            this.SP_menu.Padding = new System.Windows.Forms.Padding(12, 3, 0, 3);
            this.SP_menu.Size = new System.Drawing.Size(920, 42);
            this.SP_menu.TabIndex = 14;
            this.SP_menu.Text = "menuStrip1";
            // 
            // Pach_Hybrid_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.SP_menu);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Pach_Hybrid_Control";
            this.Size = new System.Drawing.Size(920, 1626);
            this.Abs_Table.ResumeLayout(false);
            this.Abs_Table.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs63)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs125)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs250)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs500)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs4k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs8k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AbsFlat)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abs50)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs80)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs100)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs160)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs200)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs315)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs400)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs630)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs800)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1250)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1600)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2500)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs3150)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs5k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs6300)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs10k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs50Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs80Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs100Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs160Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs200Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs315Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs400Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs630Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs800Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1250Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs1600Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs2500Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs3150Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs5kOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs6300Out)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Abs10kOut)).EndInit();
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
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Pressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel_Humidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Air_Temp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CO_TIME)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT_Count)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Spec_RayCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Order)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.TabPage4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.Absorption.ResumeLayout(false);
            this.Scattering.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.user_quart_lambda)).EndInit();
            this.Transparency.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
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
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TL8k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL4k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL2k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL1k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL500)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL250)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL125)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TL63)).EndInit();
            this.TabPage3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.SourceContext.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Alt_Choice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azi_Choice)).EndInit();
            this.PathContext.ResumeLayout(false);
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
            private System.Windows.Forms.GroupBox groupBox5;
            private System.Windows.Forms.Button Save_Material;
            private System.Windows.Forms.MaskedTextBox Material_Name;
            private System.Windows.Forms.CheckBox EdgeFreq;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
            private System.Windows.Forms.TableLayoutPanel Abs_Table;
            private System.Windows.Forms.Label label18;
            private System.Windows.Forms.Label label22;
            internal System.Windows.Forms.CheckedListBox SourceList;
            internal System.Windows.Forms.Label label15;
            private System.Windows.Forms.Label ISOCOMP;
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
            private System.Windows.Forms.Button Auralisation;
            private System.Windows.Forms.Button Delete_Material;
            private System.Windows.Forms.ContextMenuStrip SourceContext;
            private System.Windows.Forms.ToolStripMenuItem PowerMod;
            private System.Windows.Forms.ToolStripMenuItem DelayMod;
            private System.Windows.Forms.Label label14;
            private System.Windows.Forms.RadioButton Spec_Rays;
            private System.Windows.Forms.RadioButton DetailedConvergence;
            private System.Windows.Forms.RadioButton Minimum_Convergence;
            private System.Windows.Forms.Label label16;
            private System.Windows.Forms.Label quart_lambda;
            internal System.Windows.Forms.TrackBar user_quart_lambda;
            private System.Windows.Forms.Button PlasterScatter;
            private System.Windows.Forms.Button GlassScatter;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
            internal System.Windows.Forms.Label label4;
            private System.Windows.Forms.ContextMenuStrip PathContext;
            private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
            private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
            private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
            private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
            private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            private System.Windows.Forms.TabControl tabControl2;
            private System.Windows.Forms.TabPage tabPage2;
            private System.Windows.Forms.CheckBox Trans_Check;
            private System.Windows.Forms.TabPage tabPage5;
            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
            internal System.Windows.Forms.Label label63;
            private System.Windows.Forms.GroupBox groupBox1;
            private System.Windows.Forms.Button DeleteAssembly;
            private System.Windows.Forms.Button SaveAssembly;
            private System.Windows.Forms.MaskedTextBox IsolationAssemblies;
            private System.Windows.Forms.Label label53;
            private System.Windows.Forms.CheckBox TL_Check;
            private System.Windows.Forms.NumericUpDown TL8k;
            internal System.Windows.Forms.Label label54;
            internal System.Windows.Forms.Label label52;
            internal System.Windows.Forms.Label label51;
            internal System.Windows.Forms.Label label48;
            internal System.Windows.Forms.Label label46;
            internal System.Windows.Forms.Label label47;
            internal System.Windows.Forms.Label label49;
            internal System.Windows.Forms.Label label50;
            private System.Windows.Forms.NumericUpDown TL4k;
            private System.Windows.Forms.NumericUpDown TL2k;
            private System.Windows.Forms.NumericUpDown TL1k;
            private System.Windows.Forms.NumericUpDown TL500;
            private System.Windows.Forms.NumericUpDown TL250;
            private System.Windows.Forms.NumericUpDown TL125;
            private System.Windows.Forms.NumericUpDown TL63;
            private System.Windows.Forms.Label label55;
            private System.Windows.Forms.Label label56;
            private System.Windows.Forms.Label label57;
            private System.Windows.Forms.Label label58;
            private System.Windows.Forms.Label label59;
            private System.Windows.Forms.Label label60;
            private System.Windows.Forms.Label label61;
            private System.Windows.Forms.Label label62;
            internal System.Windows.Forms.ListBox Isolation_Lib;
            private System.Windows.Forms.Label label64;
            internal System.Windows.Forms.Label label65;
            private System.Windows.Forms.Panel panel1;
            private System.Windows.Forms.RadioButton OctaveBand;
            private System.Windows.Forms.RadioButton ThirdOctaveBand;
            private System.Windows.Forms.GroupBox groupBox2;
            private System.Windows.Forms.RadioButton ThirdOctave_Abs;
            private System.Windows.Forms.RadioButton Octave_Abs;
            internal System.Windows.Forms.Label label24;
            private System.Windows.Forms.TrackBar AbsFlat;
            internal System.Windows.Forms.Label label66;
            internal System.Windows.Forms.Label label67;
            internal System.Windows.Forms.Label label68;
            internal System.Windows.Forms.Label label69;
            internal System.Windows.Forms.Label label70;
            internal System.Windows.Forms.Label label71;
            internal System.Windows.Forms.Label label72;
            internal System.Windows.Forms.Label label73;
            internal System.Windows.Forms.Label label74;
            internal System.Windows.Forms.Label label75;
            internal System.Windows.Forms.Label label76;
            internal System.Windows.Forms.Label label77;
            internal System.Windows.Forms.Label label78;
            internal System.Windows.Forms.Label label79;
            internal System.Windows.Forms.Label label80;
            internal System.Windows.Forms.Label label81;
            internal System.Windows.Forms.TrackBar Abs50;
            internal System.Windows.Forms.TrackBar Abs80;
            internal System.Windows.Forms.TrackBar Abs100;
            internal System.Windows.Forms.TrackBar Abs160;
            internal System.Windows.Forms.TrackBar Abs200;
            internal System.Windows.Forms.TrackBar Abs315;
            internal System.Windows.Forms.TrackBar Abs400;
            internal System.Windows.Forms.TrackBar Abs630;
            internal System.Windows.Forms.TrackBar Abs800;
            internal System.Windows.Forms.TrackBar Abs1250;
            internal System.Windows.Forms.TrackBar Abs1600;
            internal System.Windows.Forms.TrackBar Abs2500;
            internal System.Windows.Forms.TrackBar Abs3150;
            internal System.Windows.Forms.TrackBar Abs5k;
            internal System.Windows.Forms.TrackBar Abs6300;
            internal System.Windows.Forms.TrackBar Abs10k;
            private System.Windows.Forms.NumericUpDown Abs50Out;
            private System.Windows.Forms.NumericUpDown Abs80Out;
            private System.Windows.Forms.NumericUpDown Abs100Out;
            private System.Windows.Forms.NumericUpDown Abs160Out;
            private System.Windows.Forms.NumericUpDown Abs200Out;
            private System.Windows.Forms.NumericUpDown Abs315Out;
            private System.Windows.Forms.NumericUpDown Abs400Out;
            private System.Windows.Forms.NumericUpDown Abs630Out;
            private System.Windows.Forms.NumericUpDown Abs800Out;
            private System.Windows.Forms.NumericUpDown Abs1250Out;
            private System.Windows.Forms.NumericUpDown Abs1600Out;
            private System.Windows.Forms.NumericUpDown Abs2500Out;
            private System.Windows.Forms.NumericUpDown Abs3150Out;
            private System.Windows.Forms.NumericUpDown Abs5kOut;
            private System.Windows.Forms.NumericUpDown Abs6300Out;
            private System.Windows.Forms.NumericUpDown Abs10kOut;
            private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem openDataToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveDataToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveParameterResultsToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem savePTBFormatToolStripMenuItem;
            private System.Windows.Forms.ToolStripMenuItem saveVRSpectraToolStripMenuItem;
            private System.Windows.Forms.MenuStrip SP_menu;
        }
    }
}