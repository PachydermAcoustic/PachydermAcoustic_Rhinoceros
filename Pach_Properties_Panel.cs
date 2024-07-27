//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2023, Arthur van der Harten 
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

using Eto.Forms;
using ScottPlot.AxisPanels;
using System;
using System.IO;
using System.Linq;

namespace Pachyderm_Acoustic
{
    public partial class Pach_Properties_Panel : Eto.Forms.Panel
    {
        string SettingsPath;

        private RadioButton TskHigh;
        private RadioButton TskAbove;
        private RadioButton TskNormal;
        private GroupBox Processing_Select;
        //private NumericStepper Thread_Spec;
        private GroupBox Geometry_Select;
        private RadioButton GEO_NURBS;
        private RadioButton GEO_MESH;
        private RadioButton GEO_MR_MESH;
        private GroupBox GeoSys;
        private NumericStepper OCT_DEPTH;
        private RadioButton VGSP_CHECK;
        private RadioButton OCT_CHECK;
        private NumericStepper VGDOMAIN;
        private GroupBox Material_Path;
        private MaskedTextBox Library_Path;
        private Button Browse_Material;
        private CheckBox Save_Results;
        private GroupBox FilterCtrls;
        private RadioButton Filt_MinPhase;
        private RadioButton Filt_LinearPhase;

        private static Pach_Properties_Panel instance;

        public static Pach_Properties_Panel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Pach_Properties_Panel();
                }
                return instance;
            }
        }

        private Pach_Properties_Panel()
        {
            this.TskHigh = new RadioButton();
            this.TskAbove = new RadioButton();
            this.TskNormal = new RadioButton();
            this.Processing_Select = new GroupBox();
            //this.Thread_Spec = new NumericStepper();
            this.Geometry_Select = new GroupBox();
            this.GEO_MR_MESH = new RadioButton();
            this.GEO_NURBS = new RadioButton();
            this.GEO_MESH = new RadioButton();
            this.GeoSys = new GroupBox();
            this.VGDOMAIN = new NumericStepper();
            this.OCT_DEPTH = new NumericStepper();
            this.VGSP_CHECK = new RadioButton();
            this.OCT_CHECK = new RadioButton();
            this.Material_Path = new GroupBox();
            this.Browse_Material = new Button();
            this.Library_Path = new MaskedTextBox();
            this.Save_Results = new CheckBox();
            this.FilterCtrls = new GroupBox();
            this.Filt_MinPhase = new RadioButton();
            this.Filt_LinearPhase = new RadioButton();

            this.Processing_Select.Text = "Simulation Task Priority";
            this.TskHigh.Text = "High";
            this.TskHigh.MouseUp += this.PrioritySettingsChanged;
            this.TskAbove.Text = "Above Normal";
            this.TskAbove.MouseUp += this.PrioritySettingsChanged;
            this.TskNormal.Text = "Normal";
            this.TskNormal.MouseUp += this.PrioritySettingsChanged;

            //Label label1 = new Label();
            //label1.Text = "ThreadCount";
            //this.Thread_Spec.ValueChanged += this.ThreadSettingsChanged;
            DynamicLayout PL = new DynamicLayout();
            PL.Spacing = new Eto.Drawing.Size(8, 8);
            PL.AddRow(TskHigh);
            PL.AddRow(TskAbove);
            PL.AddRow(TskNormal);
            //PL.AddRow(label1, Thread_Spec);
            Processing_Select.Content = PL;

            this.Geometry_Select.Text = "Geometry System";
            this.GEO_MR_MESH.Text = "Use Multi-Resolution Meshes";
            this.GEO_MR_MESH.MouseUp += this.GeometrySettingsChanged;
            GEO_MR_MESH.Enabled = false;

            this.GEO_NURBS.Text = "Use NURBS";
            this.GEO_NURBS.MouseUp += this.GeometrySettingsChanged;

            this.GEO_MESH.Checked = true;
            this.GEO_MESH.Text = "Use Meshes";
            this.GEO_MESH.MouseUp += this.GeometrySettingsChanged;
            DynamicLayout GL = new DynamicLayout();
            GL.Spacing = new Eto.Drawing.Size(8, 8);
            GL.AddRow(GEO_NURBS);
            GL.AddRow(GEO_MESH);
            GL.AddRow(GEO_MR_MESH);
            Geometry_Select.Content = GL;

            DynamicLayout SPL = new DynamicLayout();
            SPL.Spacing = new Eto.Drawing.Size(8, 8);
            this.GeoSys.Text = "Spatial Partition";
            Label label3 = new Label();
            label3.Text = "Grid Domain";
            this.VGDOMAIN.Value = 20;
            this.VGDOMAIN.MouseLeave += this.VGSettingsChanged;

            Label label2 = new Label();
            label2.Enabled = false;
            label2.Text = "Depth";
            this.OCT_DEPTH.Enabled = false;
            this.OCT_DEPTH.Value = 3;
            this.OCT_DEPTH.MouseLeave += this.OctreeSettingsChanged;

            this.VGSP_CHECK.Checked = true;
            this.VGSP_CHECK.Text = "Voxel Grid";
            this.VGSP_CHECK.MouseLeave += this.SpatialSettingsChanged;

            this.OCT_CHECK.Enabled = false;
            this.OCT_CHECK.TabIndex = 2;
            this.OCT_CHECK.Text = "Octree";
            this.OCT_CHECK.MouseLeave += this.SpatialSettingsChanged;

            SPL.AddRow(VGSP_CHECK);
            SPL.AddRow(label3, VGDOMAIN);
            SPL.AddRow(OCT_CHECK);
            SPL.AddRow(label2, OCT_DEPTH);
            GeoSys.Content = SPL;

            this.Material_Path.Text = "Material Libary Path";
            this.Browse_Material.Text = "Browse";
            Browse_Material.Size = new Eto.Drawing.Size(20, 200);
            this.Browse_Material.Click += this.Browse_Material_Click;
            DynamicLayout ML = new DynamicLayout();
            ML.Spacing = new Eto.Drawing.Size(8, 8);
            ML.AddRow(Library_Path);
            ML.AddRow(Browse_Material);
            ML.AddSpace();
            Material_Path.Content = ML;

            this.Save_Results.Text = "Automatically Save Results";
            this.Save_Results.MouseLeave += SaveSettingsChanged;

            this.FilterCtrls.Controls.Append(this.Filt_MinPhase);
            this.FilterCtrls.Controls.Append(this.Filt_LinearPhase);
            this.FilterCtrls.Text = "Filtering Settings";
            DynamicLayout FL = new DynamicLayout();
            FL.Spacing = new Eto.Drawing.Size(8, 8);

            this.Filt_MinPhase.Text = "Minimum Phase";
            this.Filt_MinPhase.MouseUp += this.FilterSettingsChanged;

            this.Filt_LinearPhase.Checked = true;
            this.Filt_LinearPhase.Text = "Linear Phase";
            this.Filt_LinearPhase.MouseUp += this.FilterSettingsChanged;

            FL.AddRow(Filt_LinearPhase);
            FL.AddRow(Filt_MinPhase);
            FilterCtrls.Content = FL;

            DynamicLayout PropLayout = new DynamicLayout();
            PropLayout.AddRow(Save_Results);

            DynamicLayout Rows = new DynamicLayout();
            Rows.AddRow(Geometry_Select, FilterCtrls);
            Rows.AddRow(Processing_Select, GeoSys);
            PropLayout.AddRow(Rows);
            PropLayout.AddRow(Material_Path);
            PropLayout.AddSpace();
            this.Content = PropLayout;

            //Thread_Spec.MaxValue = System.Environment.ProcessorCount;
            //Thread_Spec.Value = System.Environment.ProcessorCount;

            SettingsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Pachyderm";
            if (!System.IO.Directory.Exists(SettingsPath)) System.IO.Directory.CreateDirectory(SettingsPath);

            SettingsPath += "\\Pach_Settings.pset";

            Pach_Properties pachset = Pach_Properties.Instance;


            //2. Geometry System(int)
            switch (pachset.Geometry_System)
            {
                case 1:
                    GEO_NURBS.Checked = true;
                    break;
                case 2:
                    GEO_MESH.Checked = true;
                    break;
                case 3:
                    GEO_MR_MESH.Checked = true;
                    break;
            }

            //3. Processing
            switch (pachset.TaskPriority())
            {
                case 0:
                    TskHigh.Checked = true;
                    break;
                case 1:
                    TskAbove.Checked = true;
                    break;
                case 2:
                    TskNormal.Checked = true;
                    break;
            }

            //4. ThreadCount(int)
            //Thread_Spec.Value = pachset.ThreadCount;

            //5. Spatial Partition Selection (int)
            switch (pachset.Spatial_Optimization)
            {
                case 1:
                    VGSP_CHECK.Checked = true;
                    break;
                case 2:
                    OCT_CHECK.Checked = true;
                    break;
            }
            //6. Voxel Grid Domain(int)
            VGDOMAIN.Value = pachset.VoxelGrid_Domain;
            //7. Octree Depth(int)
            OCT_DEPTH.Value = pachset.Oct_Depth;
            //8. Material Library Path
            Library_Path.Text = pachset.Lib_Path;
            //9. Save Results after simulation?
            Save_Results.Checked = pachset.SaveResults;
            //10. Save Filter Method
            switch (pachset.FilterPhase)
            {
                case 1:
                    Filt_LinearPhase.Checked = true;
                    Audio.Pach_SP.Filter = new Audio.Pach_SP.Linear_Phase_System();
                    break;
                case 2:
                    Filt_MinPhase.Checked = true;
                    Audio.Pach_SP.Filter = new Audio.Pach_SP.Minimum_Phase_System();
                    break;
            }
        }

        private void Browse_Material_Click(object sender, EventArgs e)
        {
            Eto.Forms.SelectFolderDialog FBD = new Eto.Forms.SelectFolderDialog();

            if (FBD.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) != Eto.Forms.DialogResult.Ok) return;
            Library_Path.Text = FBD.Directory;
            Pach_Properties.Instance.Lib_Path = Library_Path.Text;
        }

        private void PrioritySettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties pachset = Pach_Properties.Instance;

            if (sender == TskHigh)
            {
                TskAbove.Checked = false;
                TskNormal.Checked = false;
                pachset.Priority_Choice = 0;
            }
            else if (sender == TskAbove)
            {
                TskHigh.Checked = false;
                TskNormal.Checked = false;
                pachset.Priority_Choice = 1;
            }
            else if (sender == TskNormal)
            {
                TskAbove.Checked = false;
                TskHigh.Checked = false;
                pachset.Priority_Choice = 2;
            }
        }

        private void ThreadSettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties.Instance.ThreadCount = System.Environment.ProcessorCount;//(int)Thread_Spec.Value;
        }

        private void GeometrySettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties pachset = Pach_Properties.Instance;

            if (sender == GEO_NURBS) 
            {
                GEO_MESH.Checked = false;
                GEO_MR_MESH.Checked = false;
                pachset.Geometry_System = 1; 
            }
            else if (sender == GEO_MESH)
            {
                GEO_NURBS.Checked = false;
                GEO_MR_MESH.Checked = false;
                pachset.Geometry_System = 2; 
            }
            else if (sender == GEO_MR_MESH)
            {
                GEO_MESH.Checked = false;
                GEO_NURBS.Checked = false;
                pachset.Geometry_System = 3;
            }
        }

        private void SpatialSettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties pachset = Pach_Properties.Instance;

            if (sender == VGSP_CHECK) 
            {
                OCT_CHECK.Checked = false;
                pachset.Spatial_Optimization = 1; 
            }
            else if (sender == OCT_CHECK) 
            {
                VGSP_CHECK.Checked = false;
                pachset.Spatial_Optimization = 2;
            }
        }

        private void FilterSettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties pachset = Pach_Properties.Instance;

            if (sender == Filt_LinearPhase) 
            {
                Filt_MinPhase.Checked = false;
                pachset.FilterPhase = 1; 
            }
            else if (sender == Filt_MinPhase)
            {
                Filt_LinearPhase.Checked = false;
                pachset.FilterPhase = 2;
            }
        }

        private void VGSettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties.Instance.VoxelGrid_Domain = (int)VGDOMAIN.Value;
        }

        private void OctreeSettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties.Instance.ThreadCount = (int)OCT_DEPTH.Value;
        }

        private void SaveSettingsChanged(object sender, EventArgs e)
        {
            Pach_Properties.Instance.SaveResults = Save_Results.Checked.Value;
        }
    }
}