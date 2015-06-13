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

//[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
namespace Pachyderm_Acoustic
{
    namespace UI
    {
        partial class Pach_Properties : System.Windows.Forms.UserControl
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
            this.PR_Single = new System.Windows.Forms.RadioButton();
            this.PR_MULTI_ALL = new System.Windows.Forms.RadioButton();
            this.PR_MULTI_EXP = new System.Windows.Forms.RadioButton();
            this.Processing_Select = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Thread_Spec = new System.Windows.Forms.NumericUpDown();
            this.Geometry_Select = new System.Windows.Forms.GroupBox();
            this.GEO_MR_MESH = new System.Windows.Forms.RadioButton();
            this.GEO_NURBS = new System.Windows.Forms.RadioButton();
            this.GEO_MESH = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.VGDOMAIN = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.OCT_DEPTH = new System.Windows.Forms.NumericUpDown();
            this.VGSP_CHECK = new System.Windows.Forms.RadioButton();
            this.OCT_CHECK = new System.Windows.Forms.RadioButton();
            this.Material_Path = new System.Windows.Forms.GroupBox();
            this.Browse_Material = new System.Windows.Forms.Button();
            this.Library_Path = new System.Windows.Forms.MaskedTextBox();
            this.Save_Results = new System.Windows.Forms.CheckBox();
            this.Processing_Select.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Thread_Spec)).BeginInit();
            this.Geometry_Select.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VGDOMAIN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OCT_DEPTH)).BeginInit();
            this.Material_Path.SuspendLayout();
            this.SuspendLayout();
            // 
            // PR_Single
            // 
            this.PR_Single.AutoSize = true;
            this.PR_Single.Location = new System.Drawing.Point(6, 19);
            this.PR_Single.Name = "PR_Single";
            this.PR_Single.Size = new System.Drawing.Size(104, 17);
            this.PR_Single.TabIndex = 0;
            this.PR_Single.TabStop = true;
            this.PR_Single.Text = "Single Processor";
            this.PR_Single.UseVisualStyleBackColor = true;
            this.PR_Single.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // PR_MULTI_ALL
            // 
            this.PR_MULTI_ALL.AutoSize = true;
            this.PR_MULTI_ALL.Checked = true;
            this.PR_MULTI_ALL.Location = new System.Drawing.Point(6, 42);
            this.PR_MULTI_ALL.Name = "PR_MULTI_ALL";
            this.PR_MULTI_ALL.Size = new System.Drawing.Size(147, 17);
            this.PR_MULTI_ALL.TabIndex = 2;
            this.PR_MULTI_ALL.TabStop = true;
            this.PR_MULTI_ALL.Text = "Multi-Processor (All Cores)";
            this.PR_MULTI_ALL.UseVisualStyleBackColor = true;
            this.PR_MULTI_ALL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // PR_MULTI_EXP
            // 
            this.PR_MULTI_EXP.AutoSize = true;
            this.PR_MULTI_EXP.Location = new System.Drawing.Point(6, 65);
            this.PR_MULTI_EXP.Name = "PR_MULTI_EXP";
            this.PR_MULTI_EXP.Size = new System.Drawing.Size(139, 17);
            this.PR_MULTI_EXP.TabIndex = 3;
            this.PR_MULTI_EXP.TabStop = true;
            this.PR_MULTI_EXP.Text = "Mutli-Processor (Explicit)";
            this.PR_MULTI_EXP.UseVisualStyleBackColor = true;
            this.PR_MULTI_EXP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // Processing_Select
            // 
            this.Processing_Select.Controls.Add(this.label1);
            this.Processing_Select.Controls.Add(this.Thread_Spec);
            this.Processing_Select.Controls.Add(this.PR_Single);
            this.Processing_Select.Controls.Add(this.PR_MULTI_EXP);
            this.Processing_Select.Controls.Add(this.PR_MULTI_ALL);
            this.Processing_Select.Location = new System.Drawing.Point(3, 111);
            this.Processing_Select.Name = "Processing_Select";
            this.Processing_Select.Size = new System.Drawing.Size(193, 118);
            this.Processing_Select.TabIndex = 4;
            this.Processing_Select.TabStop = false;
            this.Processing_Select.Text = "Processing";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "ThreadCount";
            // 
            // Thread_Spec
            // 
            this.Thread_Spec.Location = new System.Drawing.Point(99, 88);
            this.Thread_Spec.Name = "Thread_Spec";
            this.Thread_Spec.Size = new System.Drawing.Size(54, 20);
            this.Thread_Spec.TabIndex = 4;
            this.Thread_Spec.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SettingsChanged);
            this.Thread_Spec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // Geometry_Select
            // 
            this.Geometry_Select.Controls.Add(this.GEO_MR_MESH);
            this.Geometry_Select.Controls.Add(this.GEO_NURBS);
            this.Geometry_Select.Controls.Add(this.GEO_MESH);
            this.Geometry_Select.Location = new System.Drawing.Point(3, 3);
            this.Geometry_Select.Name = "Geometry_Select";
            this.Geometry_Select.Size = new System.Drawing.Size(193, 106);
            this.Geometry_Select.TabIndex = 5;
            this.Geometry_Select.TabStop = false;
            this.Geometry_Select.Text = "Geometry System";
            // 
            // GEO_MR_MESH
            // 
            this.GEO_MR_MESH.AutoSize = true;
            this.GEO_MR_MESH.Enabled = false;
            this.GEO_MR_MESH.Location = new System.Drawing.Point(6, 65);
            this.GEO_MR_MESH.Name = "GEO_MR_MESH";
            this.GEO_MR_MESH.Size = new System.Drawing.Size(162, 17);
            this.GEO_MR_MESH.TabIndex = 3;
            this.GEO_MR_MESH.Text = "Use Multi-Resolution Meshes";
            this.GEO_MR_MESH.UseVisualStyleBackColor = true;
            this.GEO_MR_MESH.CheckedChanged += new System.EventHandler(this.GEO_MR_MESH_CheckedChanged);
            this.GEO_MR_MESH.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // GEO_NURBS
            // 
            this.GEO_NURBS.AutoSize = true;
            this.GEO_NURBS.Location = new System.Drawing.Point(6, 19);
            this.GEO_NURBS.Name = "GEO_NURBS";
            this.GEO_NURBS.Size = new System.Drawing.Size(85, 17);
            this.GEO_NURBS.TabIndex = 0;
            this.GEO_NURBS.Text = "Use NURBS";
            this.GEO_NURBS.UseVisualStyleBackColor = true;
            this.GEO_NURBS.CheckedChanged += new System.EventHandler(this.GEO_NURBS_CheckedChanged);
            this.GEO_NURBS.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // GEO_MESH
            // 
            this.GEO_MESH.AutoSize = true;
            this.GEO_MESH.Checked = true;
            this.GEO_MESH.Location = new System.Drawing.Point(6, 42);
            this.GEO_MESH.Name = "GEO_MESH";
            this.GEO_MESH.Size = new System.Drawing.Size(84, 17);
            this.GEO_MESH.TabIndex = 2;
            this.GEO_MESH.TabStop = true;
            this.GEO_MESH.Text = "Use Meshes";
            this.GEO_MESH.UseVisualStyleBackColor = true;
            this.GEO_MESH.CheckedChanged += new System.EventHandler(this.GEO_MESH_CheckedChanged);
            this.GEO_MESH.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.VGDOMAIN);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.OCT_DEPTH);
            this.groupBox1.Controls.Add(this.VGSP_CHECK);
            this.groupBox1.Controls.Add(this.OCT_CHECK);
            this.groupBox1.Location = new System.Drawing.Point(202, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(193, 118);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spatial Partition";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Grid Domain";
            // 
            // VGDOMAIN
            // 
            this.VGDOMAIN.Location = new System.Drawing.Point(99, 37);
            this.VGDOMAIN.Name = "VGDOMAIN";
            this.VGDOMAIN.Size = new System.Drawing.Size(54, 20);
            this.VGDOMAIN.TabIndex = 6;
            this.VGDOMAIN.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.VGDOMAIN.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SettingsChanged);
            this.VGDOMAIN.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(24, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Depth";
            // 
            // OCT_DEPTH
            // 
            this.OCT_DEPTH.Enabled = false;
            this.OCT_DEPTH.Location = new System.Drawing.Point(99, 73);
            this.OCT_DEPTH.Name = "OCT_DEPTH";
            this.OCT_DEPTH.Size = new System.Drawing.Size(54, 20);
            this.OCT_DEPTH.TabIndex = 4;
            this.OCT_DEPTH.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.OCT_DEPTH.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SettingsChanged);
            this.OCT_DEPTH.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // VGSP_CHECK
            // 
            this.VGSP_CHECK.AutoSize = true;
            this.VGSP_CHECK.Checked = true;
            this.VGSP_CHECK.Location = new System.Drawing.Point(6, 19);
            this.VGSP_CHECK.Name = "VGSP_CHECK";
            this.VGSP_CHECK.Size = new System.Drawing.Size(73, 17);
            this.VGSP_CHECK.TabIndex = 0;
            this.VGSP_CHECK.TabStop = true;
            this.VGSP_CHECK.Text = "Voxel Grid";
            this.VGSP_CHECK.UseVisualStyleBackColor = true;
            this.VGSP_CHECK.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // OCT_CHECK
            // 
            this.OCT_CHECK.AutoSize = true;
            this.OCT_CHECK.Enabled = false;
            this.OCT_CHECK.Location = new System.Drawing.Point(6, 55);
            this.OCT_CHECK.Name = "OCT_CHECK";
            this.OCT_CHECK.Size = new System.Drawing.Size(57, 17);
            this.OCT_CHECK.TabIndex = 2;
            this.OCT_CHECK.Text = "Octree";
            this.OCT_CHECK.UseVisualStyleBackColor = true;
            this.OCT_CHECK.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsChanged);
            // 
            // Material_Path
            // 
            this.Material_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Material_Path.Controls.Add(this.Browse_Material);
            this.Material_Path.Controls.Add(this.Library_Path);
            this.Material_Path.Location = new System.Drawing.Point(3, 235);
            this.Material_Path.Name = "Material_Path";
            this.Material_Path.Size = new System.Drawing.Size(392, 82);
            this.Material_Path.TabIndex = 8;
            this.Material_Path.TabStop = false;
            this.Material_Path.Text = "Material Libary Path";
            // 
            // Browse_Material
            // 
            this.Browse_Material.Location = new System.Drawing.Point(6, 49);
            this.Browse_Material.Name = "Browse_Material";
            this.Browse_Material.Size = new System.Drawing.Size(126, 27);
            this.Browse_Material.TabIndex = 1;
            this.Browse_Material.Text = "Browse";
            this.Browse_Material.UseVisualStyleBackColor = true;
            this.Browse_Material.Click += new System.EventHandler(this.Browse_Material_Click);
            // 
            // Library_Path
            // 
            this.Library_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Library_Path.Location = new System.Drawing.Point(6, 23);
            this.Library_Path.Name = "Library_Path";
            this.Library_Path.Size = new System.Drawing.Size(380, 20);
            this.Library_Path.TabIndex = 0;
            // 
            // Save_Results
            // 
            this.Save_Results.AutoSize = true;
            this.Save_Results.Location = new System.Drawing.Point(208, 22);
            this.Save_Results.Name = "Save_Results";
            this.Save_Results.Size = new System.Drawing.Size(154, 17);
            this.Save_Results.TabIndex = 9;
            this.Save_Results.Text = "Automatically Save Results";
            this.Save_Results.UseVisualStyleBackColor = true;
            // 
            // Pach_Properties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Save_Results);
            this.Controls.Add(this.Material_Path);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Geometry_Select);
            this.Controls.Add(this.Processing_Select);
            this.Name = "Pach_Properties";
            this.Size = new System.Drawing.Size(398, 324);
            this.Processing_Select.ResumeLayout(false);
            this.Processing_Select.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Thread_Spec)).EndInit();
            this.Geometry_Select.ResumeLayout(false);
            this.Geometry_Select.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VGDOMAIN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OCT_DEPTH)).EndInit();
            this.Material_Path.ResumeLayout(false);
            this.Material_Path.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            private System.Windows.Forms.RadioButton PR_Single;
            private System.Windows.Forms.RadioButton PR_MULTI_ALL;
            private System.Windows.Forms.RadioButton PR_MULTI_EXP;
            private System.Windows.Forms.GroupBox Processing_Select;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.NumericUpDown Thread_Spec;
            private System.Windows.Forms.GroupBox Geometry_Select;
            private System.Windows.Forms.RadioButton GEO_NURBS;
            private System.Windows.Forms.RadioButton GEO_MESH;
            private System.Windows.Forms.RadioButton GEO_MR_MESH;
            private System.Windows.Forms.GroupBox groupBox1;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.NumericUpDown OCT_DEPTH;
            private System.Windows.Forms.RadioButton VGSP_CHECK;
            private System.Windows.Forms.RadioButton OCT_CHECK;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.NumericUpDown VGDOMAIN;
            private System.Windows.Forms.GroupBox Material_Path;
            private System.Windows.Forms.MaskedTextBox Library_Path;
            private System.Windows.Forms.Button Browse_Material;
            private System.Windows.Forms.CheckBox Save_Results;
        }
    }
}