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

using System;
using System.IO;
namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public partial class Pach_Properties
        {
            string SettingsPath;

            public Pach_Properties()
            {
                InitializeComponent();
                Thread_Spec.Maximum = System.Environment.ProcessorCount;
                Thread_Spec.Value = System.Environment.ProcessorCount;

                SettingsPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                SettingsPath = System.IO.Path.GetDirectoryName(SettingsPath);
                SettingsPath += "\\Pach_Settings.pset";

                FileStream S = new FileStream(SettingsPath, FileMode.OpenOrCreate);
                BinaryReader Reader = new BinaryReader(S);
                BinaryWriter Writer = new BinaryWriter(S);

                try
                {
                    string p_version = Reader.ReadString();
                    if (p_version != PachydermAc_PlugIn.Instance.Version) newSettings(ref Reader, ref Writer);

                    //2. Geometry System(int)
                    switch (Reader.ReadInt32())
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
                    switch (Reader.ReadInt32())
                    {
                        case 1:
                            PR_Single.Checked = true;
                            break;
                        case 2:
                            PR_MULTI_ALL.Checked = true;
                            break;
                        case 3:
                            PR_MULTI_EXP.Checked = true;
                            break;
                    }

                    //4. ThreadCount(int)
                    Thread_Spec.Value = Reader.ReadInt32();

                    //5. Spatial Partition Selection (int)
                    switch (Reader.ReadInt32())
                    {
                        case 1:
                            VGSP_CHECK.Checked = true;
                            break;
                        case 2:
                            OCT_CHECK.Checked = true;
                            break;
                    }
                    //6. Voxel Grid Domain(int)
                    VGDOMAIN.Value = Reader.ReadInt32();
                    //7. Octree Depth(int)
                    OCT_DEPTH.Value = Reader.ReadInt32();
                    //8. Material Library Path
                    Library_Path.Text = Reader.ReadString();
                    //9. Save Results after simulation?
                    Save_Results.Checked = Reader.ReadBoolean();
                    //10. Save Filter Method
                    switch (Reader.ReadInt32())
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
                    Reader.Close();
                    Writer.Close();
                }
                catch (Exception x)
                {
                    if (x is EndOfStreamException)
                    { newSettings(ref Reader, ref Writer); }
                    else if (x is AccessViolationException)
                    { System.Windows.Forms.MessageBox.Show("You do not have write access permissions on Pachyderm Files. Go to your installation and check the security properties of all Pachyderm files. We recommend setting permissions on these files to 'Full Control' for all users.", "Access Violation Exception"); }
                }
            }

            /// <summary>
            /// This method creates a new settings file for Pachyderm set to certain defaults.
            /// Called when previous methods find that there is no suitable settings file.
            /// </summary>
            /// <param name="Reader"></param>
            /// <param name="Writer"></param>
            private void newSettings(ref BinaryReader Reader, ref BinaryWriter Writer)
            {
                Reader.Close();
                Writer.Close();
                FileStream S = new FileStream(SettingsPath, FileMode.Create);
                Writer = new BinaryWriter(S);
                //1. Plugin Version(string)
                Writer.Write(PachydermAc_PlugIn.Instance.Version);
                //2. Geometry System(int)
                Writer.Write(2);
                //3. Processing(int)
                Writer.Write(2);
                //4. ThreadCount(int)
                Writer.Write((int)Thread_Spec.Value);
                //5. Spatial Partition Selection (int)
                Writer.Write(1);
                //6. Voxel Grid Domain(int)
                Writer.Write(15);
                //7. Octree Depth(int)
                Writer.Write(3);
                //8. Material Library Path
                string mlPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                mlPath = System.IO.Path.GetDirectoryName(mlPath);
                Library_Path.Text = mlPath;
                Writer.Write(mlPath);
                //9. Save Results after simulation?
                Writer.Write(false);
                //10. Save Filter Method
                { Writer.Write(1); }
                Reader.Close();
                Writer.Close();
            }

            /// <summary>
            /// Get the number of processors on the system.
            /// </summary>
            /// <returns></returns>
            public int ProcessorCount()
            {
                if (PR_Single.Checked) return 1;
                if (PR_MULTI_ALL.Checked) return System.Environment.ProcessorCount;
                if (PR_MULTI_EXP.Checked) return (int)Thread_Spec.Value;
                throw new Exception("Is there a new Processor Option that needs to be implemented?");
            }

            /// <summary>
            /// Returns user's choice for geometry system.
            /// </summary>
            /// <returns></returns>
            public int Geometry_Spec()
            {
                if (GEO_NURBS.Checked) return 0;
                if (GEO_MESH.Checked) return 1;
                if (GEO_MR_MESH.Checked) return 2;
                throw new Exception("Is there a new Geometrical Option that needs to be implemented?");
            }

            /// <summary>
            /// Returns the user selected spatial partition
            /// </summary>
            /// <returns></returns>
            public int SP_Spec()
            {
                if (VGSP_CHECK.Checked) return 0;
                if (OCT_CHECK.Checked) return 1;
                throw new Exception("Is there a new spatial partition to be implemented?");
            }

            /// <summary>
            /// If octree is chosen, returns the depth of the octree (how many branches deep)
            /// </summary>
            /// <returns></returns>
            public int Oct_Depth()
            {
                return (int)OCT_DEPTH.Value;
            }

            /// <summary>
            /// Gets the path to the material library
            /// </summary>
            /// <returns></returns>
            public string Lib_Path()
            {
                return Library_Path.Text;
            }

            public bool SaveResults()
            {
                return Save_Results.Checked;
            }

            /// <summary>
            /// Returns the user selected domain of the voxel grid (how many voxels in each dimension)
            /// </summary>
            /// <returns></returns>
            public int VG_Domain()
            {
                return (int)VGDOMAIN.Value;
            }

            private void GEO_NURBS_CheckedChanged(object sender, EventArgs e)
            {
                //PR_Single.Checked = true;
                //Processing_Select.Enabled = false;
            }

            private void GEO_MESH_CheckedChanged(object sender, EventArgs e)
            {
                //PR_MULTI_ALL.Checked = true;
                //Processing_Select.Enabled = true;
            }

            private void GEO_MR_MESH_CheckedChanged(object sender, EventArgs e)
            {
                //PR_MULTI_EXP.Checked = true;
                //Processing_Select.Enabled = true;
            }

            private void Browse_Material_Click(object sender, EventArgs e)
            {
                System.Windows.Forms.FolderBrowserDialog FBD = new System.Windows.Forms.FolderBrowserDialog();

                if (FBD.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
                Library_Path.Text = FBD.SelectedPath;
                Commit_Settings();
            }

            /// <summary>
            /// Records settings to the pachyderm settings file
            /// </summary>
            private void Commit_Settings()
            {
                File.Delete(SettingsPath);
                FileStream S = new FileStream(SettingsPath, FileMode.CreateNew);
                BinaryWriter Writer = new BinaryWriter(S);
                //1. Plugin Version(string)
                Writer.Write(PachydermAc_PlugIn.Instance.Version);
                //2. Geometry System(int)
                if (GEO_NURBS.Checked == true)
                { Writer.Write(1); }
                else if (GEO_MESH.Checked == true)
                { Writer.Write(2); }
                else
                { Writer.Write(3); }

                //3. Processing(int)
                if (PR_Single.Checked == true)
                { Writer.Write(1); }
                else if (PR_MULTI_ALL.Checked == true)
                { Writer.Write(2); }
                else
                { Writer.Write(3); }

                //4. ThreadCount(int)
                Writer.Write((int)Thread_Spec.Value);

                //5. Spatial Partition Selection (int)
                if (VGSP_CHECK.Checked == true)
                { Writer.Write(1); }
                else
                { Writer.Write(2); }

                //6. Voxel Grid Domain(int)
                Writer.Write((int)VGDOMAIN.Value);
                //7. Octree Depth(int)
                Writer.Write((int)OCT_DEPTH.Value);
                //8. Material Library Path
                Writer.Write(Library_Path.Text);
                //9. Save Results after simulation?
                Writer.Write(Save_Results.Checked);

                //10. Save Filter Method
                if (Filt_LinearPhase.Checked == true)
                {
                    Audio.Pach_SP.Filter = new Audio.Pach_SP.Linear_Phase_System();
                    Writer.Write(1);
                }
                else
                {
                    Audio.Pach_SP.Filter = new Audio.Pach_SP.Minimum_Phase_System();
                    Writer.Write(2);
                }
                Writer.Close();
            }

            private void SettingsChanged(object sender, EventArgs e)
            {
                Commit_Settings();
            }

            private void SettingsChanged(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                Commit_Settings();
            }

            private void SettingsChanged(object sender, System.Windows.Forms.KeyEventArgs e)
            {
                Commit_Settings();
            }
        }

        public class Pach_Props_Page : Rhino.UI.OptionsDialogPage
        {
            Pach_Properties pprops = null;

            public Pach_Props_Page()
                : base("Pachyderm Acoustic")
            {
            }

            public override System.Windows.Forms.Control PageControl
            {
                get
                {
                    if ((pprops == null)) pprops = new Pach_Properties();
                    return pprops;
                }
            }

            public override bool OnApply()
            {
                Pach_Hybrid_Control.Instance.Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                Pach_Mapping_Control.Instance.Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                return base.OnApply();
            }

            public int Get_Processor_Spec()
            {
                if ((pprops == null)) pprops = new Pach_Properties();
                return pprops.ProcessorCount();
            }

            public int Get_Geometry_Spec()
            {
                if ((pprops == null)) pprops = new Pach_Properties();
                return pprops.Geometry_Spec();
            }

            public int Get_SP_Spec()
            {
                if ((pprops == null)) pprops = new Pach_Properties();
                return pprops.SP_Spec();
            }

            public int Get_Oct_Depth()
            {
                if ((pprops == null)) pprops = new Pach_Properties();
                return pprops.Oct_Depth();
            }

            public string Get_MatLib_Path()
            {
                if ((pprops == null)) pprops = new Pach_Properties();
                return pprops.Lib_Path();
            }

            public int Get_VG_Domain()
            {
                if ((pprops == null)) pprops = new Pach_Properties();
                return pprops.VG_Domain();
            }

            public bool Save_Results()
            {
                if ((pprops == null)) pprops = new Pach_Properties();
                return pprops.SaveResults();
            }

            public override string LocalPageTitle
            {
                get
                {
                    return "Pachyderm_Acoustic";
                }
            }
        }
    }
}