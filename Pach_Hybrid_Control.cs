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

using Rhino.Geometry;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using System.Linq;
using Pachyderm_Acoustic.AbsorptionModels;
using System.Drawing;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("8559be06-21d7-4535-803e-95a9dd3a2898")]
        public partial class Pach_Hybrid_Control
        {
            // This call is required by the Windows Form Designer. 
            public Pach_Hybrid_Control()
            {
                InitializeComponent();
                Load_Library();
                Instance = this;
                Rhino.RhinoDoc.LayerTableEvent += Load_Library_Event;
                Linear_Phase = Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System;
            }

            private void Load_Library_Event(object sender, Rhino.DocObjects.Tables.LayerTableEventArgs e)
            {
                Load_Library();
            }

            private void Load_Library()
            {
                string Selection = LayerDisplay.Text;
                LayerNames.Clear();

                for (int q = 0; q < Rhino.RhinoDoc.ActiveDoc.Layers.Count; q++)
                {
                    if (!Rhino.RhinoDoc.ActiveDoc.Layers[q].IsDeleted) LayerNames.Add(new layer(Rhino.RhinoDoc.ActiveDoc.Layers[q].Name, q));
                }
                LayerDisplay.Items.Clear();
                LayerDisplay.Items.AddRange(LayerNames.ToArray());
                LayerDisplay.Text = Selection;
                Material_Lib.Items.Clear();
                Material_Lib.Items.AddRange(Materials.Names_Abs().ToArray());
                Isolation_Lib.Items.Clear();
                Isolation_Lib.Items.AddRange(Materials.Names_TL().ToArray());
            }

            ///<summary>Gets the only instance of the PachydermAcoustic plug-in.</summary>
            public static Pach_Hybrid_Control Instance
            {
                get;
                private set;
            }

            #region Tab 1: Impulse Response... 
            //PachyDerm needs these common objects... 
            private Source[] Source = null;
            private Receiver_Bank[] Receiver = null;
            private Hare.Geometry.Point[] Recs;
            public int SampleRate = 44100;
            public double CutoffTime;
            List<System.Guid> ShownPaths = new List<Guid>();
            bool Linear_Phase = false;
            //bool Pressure_Ready;

            //To Begin Calculation... 
            PachydermAc_PlugIn plugin = PachydermAc_PlugIn.Instance;
            Direct_Sound[] Direct_Data = null;
            ImageSourceData[] IS_Data = null;
            List<string> SrcTypeList = new List<string>();

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                string SavePath = null;
                CutoffTime = (double)this.CO_TIME.Value;

                if (plugin.Save_Results())
                {
                    System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                    sf.DefaultExt = ".pac1";
                    sf.AddExtension = true;
                    sf.Filter = "Pachyderm Ray Data file (*.pac1)|*.pac1|" + "All Files|";
                    if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SavePath = sf.FileName;
                    }
                }
                for (int i = 0; i < SourceList.Items.Count; i++) SourceList.SetItemChecked(i, false);

                SourceList.Items.Clear();
                Source_Aim.Items.Clear();
                Receiver_Choice.Text = "0";

                IS_Data = null;
                Direct_Data = null;
                List<Hare.Geometry.Point> RPT;

                if (!(plugin.Receiver(out RPT) && plugin.Source(out Source)))
                {
                    Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                    return;
                }

                List<Hare.Geometry.Point> SPT = new List<Hare.Geometry.Point>();
                foreach (Source S in Source)
                {
                    S.AppendPts(ref SPT);
                }
                IS_Data = new ImageSourceData[Source.Length];
                Direct_Data = new Direct_Sound[Source.Length];

                Calculate.Enabled = false;
                IS_Path_Box.Items.Clear();

                List<Hare.Geometry.Point> P = new List<Hare.Geometry.Point>();
                P.AddRange(RPT);
                P.AddRange(SPT);

                Polygon_Scene PScene = RC_PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, this.BTM_ED.Checked, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                if (PScene == null || !PScene.Complete)
                {
                    CancelCalc();
                    return;
                }
                PScene.partition(P);

                ///Needed as long as there are issues with compound reflections...
                for (int i = 0; i < PScene.ObjectCount; i++)
                {
                    if (!PScene.IsPlanar(i))
                    {
                        if (ISBox.Checked)
                        {
                            if ((int)Image_Order.Value > 1)
                            {
                                System.Windows.Forms.MessageBox.Show("You have started a simulation with higher order image source in a model with curves. This version of Pachyderm uses an experimental method for deterministic curved reflections. At this time, it is not possible to perform the operation on more than one reflection. Even when it is possible, it will be prohibitively processor intensive. This simulation will be canceled, but you can proceed with an image source order of 1, or by turning image source off altogether. As always, scrutinize the results carefully, and email ORASE (info@orase.org) with any questions or concerns.", "Temporary Alert");
                                CancelCalc();
                                return;
                            }
                            else
                            {
                                DialogResult DR = System.Windows.Forms.MessageBox.Show("You have started a simulation with image source enabled in a model with curves. This version of Pachyderm uses an experimental method for deterministic curved reflections. Proceed with caution. If you would not like to participate in testing of this experimental feature, you can run your simulation with Image Source disabled for a more reliable result. Would you like to continue to run this simulation?", "Temporary Alert", MessageBoxButtons.YesNo);
                                if (DR == DialogResult.Yes)
                                {
                                    System.Windows.Forms.MessageBox.Show("As always, scrutinize the results carefully, and email ORASE (info@orase.org) with any questions or concerns.", "Temporary Alert");
                                    break;
                                }
                                else
                                {
                                    CancelCalc();
                                    return;
                                }
                            }
                        }
                    }
                }
                ///

                double[] s_total = new double[8];
                double[] a_total = new double[8];
                double total_area = 0;
                bool reviewedSCT = false;
                bool reviewedABS = false;
                ///User competence checks...
                for (int i = 0; i < PScene.AbsorptionValue.Count; i++)
                {
                    double area = PScene.SurfaceArea(i);
                    total_area += area;

                    for (int oct = 0; oct < 8; oct++)
                    {
                        double a = PScene.AbsorptionValue[i].Coefficient_A_Broad(oct);
                        if (!reviewedABS && a == 0)
                        {
                            reviewedABS = true;
                            System.Windows.Forms.MessageBox.Show("Your model has absorption coefficients of zero. Not only is this unrealistic, it will prevent your simulation from finishing in a timely manner. Please set more realistic absorption coefficients.", "Absorption Coefficient Alert");
                        }
                        a_total[oct] += area * a;
                    }
                    for (int oct = 0; oct < 8; oct++)
                    {
                        double s = PScene.ScatteringValue[i].Coefficient(oct);
                        s_total[oct] += area * s;
                        if (!reviewedSCT)
                        {
                            DialogResult DR;
                            if (s == 0)
                            {
                                reviewedSCT = true;
                                DR = System.Windows.Forms.MessageBox.Show("Your model has scattering coefficients of zero. There are cases where a simulation of this kind can be useful, but the user is warned that all surfaces scatter at least a little bit. Very low scattering only occurs with very flat, smooth, uniformly heavy surfaces, such as steel or polished, painted concrete. We recommend that you use the guides on the scattering pane of the materials control. Would you like to proceed with these coefficients?", "Zero Scattering Alert", MessageBoxButtons.YesNo);
                            }
                            else if (s < 0.1)
                            {
                                reviewedSCT = true;
                                DR = System.Windows.Forms.MessageBox.Show("Your model has very low scattering coefficients. The user is advised that rooms where these coefficients are valid are very unusual. Very low scattering only occurs with very flat, smooth, uniformly heavy surfaces, such as steel or polished, painted concrete, as might be found in a reverberation chamber. We recommend that you use the guides on the scattering pane of the materials control. Would you like to proceed with these coefficients?", "Low Scattering Alert", MessageBoxButtons.YesNo);
                            }
                            else continue;

                            if (DR != DialogResult.Yes)
                            {
                                CancelCalc();
                                return;
                            }
                        }
                    }
                }

                for(int oct = 0; oct < 8; oct++) 
                {
                    s_total[oct] /= total_area;
                    a_total[oct] /= total_area;
                    if (s_total[oct] < 0.15)
                    {
                        DialogResult DR;
                        if (!reviewedSCT)
                        {
                            reviewedSCT = true;
                            DR = System.Windows.Forms.MessageBox.Show("The total scattering of this model is very low. Are you sure?", "Low Total Scattering", MessageBoxButtons.YesNo);
                            if (DR != DialogResult.Yes)
                            {
                                CancelCalc();
                                return;
                            }
                        }
                    }
                    if (a_total[oct] < 0.1)
                    {
                        if (!reviewedABS)
                        {
                            reviewedABS = true;
                            DialogResult DR = System.Windows.Forms.MessageBox.Show("The total absorption of this model is very low. There are cases where this is appropriate, but please be aware that this simulation may take a very long time to run. Are you sure?", "Low Total Absorption", MessageBoxButtons.YesNo);
                            if (DR != DialogResult.Yes)
                            {
                                CancelCalc();
                                return;
                            }
                        }
                    }
                }
                ///

                ////////////////////////////////////////////////
                //in order to check coefficients for all polygons
                //for (int i = 0; i < PScene.Count(); i++)
                //{
                //    Hare.Geometry.Point p = PScene.polygon_centroid(i);

                //    double coef = PScene.AbsorptionValue[i].Coefficient_A_Broad(4);
                //    Rhino.RhinoDoc.ActiveDoc.Objects.AddTextDot(coef.ToString(), new Point3d(p.x, p.y, p.z));
                //}
                ////////////////////////////////////////////////
                ///////////////
                if (BTM_ED.Checked) PScene.Register_Edges(SPT, RPT);
                ///////////////

                Scene Flex_Scene;
                if (PachydermAc_PlugIn.Instance.Geometry_Spec() == 0)
                {
                    RhCommon_Scene NScene = RC_PachTools.Get_NURBS_Scene((double)Rel_Humidity.Value, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                    if (!NScene.Complete)
                    {
                        CancelCalc();
                        return;
                    }
                    NScene.partition(P, plugin.VG_Domain());
                    Flex_Scene = NScene;
                }
                else
                {
                    Flex_Scene = PScene;
                }

                Receiver_Bank.Type T;

                switch ((string)ReceiverSelection.SelectedItem)
                {
                    case "1 m. Stationary Receiver":
                        T = Receiver_Bank.Type.Stationary;
                        break;
                    case "Expanding Receiver (Expanding)":
                        T = Receiver_Bank.Type.Variable;
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Receiver Object Error");
                        Calculate.Enabled = true;
                        Source = null;
                        return;
                }

                Receiver = new Receiver_Bank[Source.Length];
                Pach_RunSim_Command command = Pach_RunSim_Command.Instance;

                if (command == null) { return; }

                for (int s = 0; s < Source.Length; s++)
                {
                    Receiver_Bank Rec = new Receiver_Bank(RPT.ToArray(), SPT[s], PScene, SampleRate, CutoffTime, T);

                    command.Sim = new Direct_Sound(Source[s], Rec, PScene, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
                    Rhino.RhinoApp.RunScript("Run_Simulation", false);
                    if (command.CommandResult != Rhino.Commands.Result.Cancel)
                    {
                        Direct_Data[s] = ((Direct_Sound)command.Sim);
                        Direct_Data[s].Create_Filter();
                    }
                    else
                    {
                        CancelCalc();
                        return;
                    }
                    command.Reset();

                    if (ISBox.CheckState == CheckState.Checked)
                    {
                        command.Sim = new ImageSourceData(Source[s], Rec, Direct_Data[s], PScene, new int[] { 0, 7 }, (int)Image_Order.Value, BTM_ED.Checked, s);

                        Rhino.RhinoApp.RunScript("Run_Simulation", false);
                        if (command.CommandResult != Rhino.Commands.Result.Cancel)
                        {
                            IS_Data[s] = ((ImageSourceData)command.Sim);
                            IS_Data[s].Create_Filter(Source[s].SWL(), 4096);
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                        command.Reset();
                    }

                    if (ISBox.CheckState == CheckState.Checked && Specular_Trace.CheckState == CheckState.Checked)
                    {
                        command.Sim = new IS_Trace(Source[s], Rec, PScene, ((double)(CO_TIME.Value / 1000) * PScene.Sound_speed(0)), (int)Spec_RayCount.Value, (int)Image_Order.Value, PScene.Sound_speed(0), SampleRate);
                        Rhino.RhinoApp.RunScript("Run_Simulation", false);
                        if (command.CommandResult != Rhino.Commands.Result.Cancel)
                        {
                            IS_Data[s].Lookup_Sequences(((IS_Trace)(command.Sim)).IS_Sequences());
                            IS_Data[s].Create_Filter(Source[s].SWL(), 4096);
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                        command.Reset();
                    }
                    if (RTBox.CheckState == CheckState.Checked)
                    {
                        command.Sim = new SplitRayTracer(Source[s], Rec, Flex_Scene, ((double)(CO_TIME.Value / 1000) * PScene.Sound_speed(0)), new int[2] { 0, 7 }, Specular_Trace.Checked ? int.MaxValue : ISBox.Checked ? (int)Image_Order.Value : 0, Minimum_Convergence.Checked ? -1 : DetailedConvergence.Checked ? 0 : (int)RT_Count.Value);

                        Rhino.RhinoApp.RunScript("Run_Simulation", false);
                        if (command.CommandResult != Rhino.Commands.Result.Cancel)
                        {
                            SplitRayTracer RT_Data = (SplitRayTracer)command.Sim;
                            Receiver[s] = RT_Data.GetReceiver;
                            Receiver[s].Create_Filter();
                            Rhino.RhinoApp.WriteLine(string.Format("{0} Rays ({1} sub-rays) cast in {2} hours, {3} minutes, {4} seconds.", RT_Data._currentRay.Sum(), RT_Data._rayTotal.Sum(), RT_Data._ts.Hours, RT_Data._ts.Minutes, RT_Data._ts.Seconds));
                            Rhino.RhinoApp.WriteLine("Percentage of energy lost: {0}%", RT_Data.PercentLost);
                        }
                        else
                        {
                            CancelCalc();
                            return;
                        }
                        command.Reset();
                    }
                }

                if (Source != null)
                {
                    List<Hare.Geometry.Point> R;
                    plugin.Receiver(out R);
                    Recs = new Hare.Geometry.Point[R.Count];
                    for (int q = 0; q < R.Count; q++)
                    {
                        Recs[q] = R[q];
                    }
                    if (SavePath != null) Utilities.FileIO.Write_Pac1(SavePath, Direct_Data, IS_Data, Receiver);

                    OpenAnalysis();
                    cleanup();
                }

                Populate_Sources();
            }

            private void CancelCalc()
            {
                if (Receiver == null || Receiver.Length == 0)
                {
                    Source = null;
                    Receiver = null;
                    Direct_Data = null;
                    IS_Data = null;
                }
                else
                {
                    int i;
                    for (i = 0; i < Receiver.Length; i++) if (Receiver[i] == null) break;
                    if (i == 0)
                    {
                        Source = null;
                        Receiver = null;
                        Direct_Data = null;
                        IS_Data = null;
                    }
                    else
                    {
                        Array.Resize(ref Source, i);
                        Array.Resize(ref Direct_Data, i);
                        Array.Resize(ref IS_Data, i);
                        Populate_Sources();
                    }
                }
                this.Calculate.Enabled = true;
            }

            private void Populate_Sources()
            {
                if (Source != null)
                {
                    SourceList.Items.Clear();
                    foreach (Source S in Source)
                    {
                        SourceList.Items.Add(String.Format("S{0}-", S.Source_ID()) + S.Type());
                        Source_Aim.Items.Add(String.Format("S{0}-", S.Source_ID()) + S.Type());
                        SrcTypeList.Add(S.Type());
                    }
                    SourceList.SetItemChecked(0, true);
                    Source_Aim.SelectedIndex = 0;
                }
                OpenAnalysis();
                Update_Graph(null, null);
            }

            private void cleanup()
            {
                //Cleanup Code 
                Calculate.Enabled = true;
                Rhino.RhinoApp.WriteLine("Calculation has been completed. Have a nice day.");
            }

            private void OpenAnalysis()
            {
                if (IS_Data != null)
                {
                    IS_Path_Box.Items.Clear();
                    if (IS_Data != null && IS_Data[0] != null)
                    {
                        if (SourceList.CheckedIndices.Count == 0) return;
                        foreach (int i in SourceList.CheckedIndices)
                        {
                            Deterministic_Reflection[] S = IS_Data[i].Paths[int.Parse(Receiver_Choice.Text)].ToArray();
                            foreach (Deterministic_Reflection s in S) IS_Path_Box.Items.Add(s);
                        }
                        PathCount.Text = string.Format("{0} Specular Reflections", IS_Path_Box.Items.Count);
                        SortPaths(null, null);
                    }
                }
            }
            #endregion

            #region Tab 2: Materials Tab 

            private List<layer> LayerNames = new List<layer>();
            private Acoustics_Library Materials = new Acoustics_Library();

            private void Materials_MouseEnter(object sender, System.EventArgs e)
            {
                if (Tabs.SelectedTab.Text == "Materials")
                {
                    string Selection = LayerDisplay.Text;
                    LayerNames.Clear();
                    for (int q = 0; q < Rhino.RhinoDoc.ActiveDoc.Layers.Count; q++)
                    {
                        if (Rhino.RhinoDoc.ActiveDoc.Layers[q].IsDeleted) continue;
                        LayerNames.Add(new layer(Rhino.RhinoDoc.ActiveDoc.Layers[q].Name, q));
                    }
                    LayerDisplay.Items.Clear();
                    for (int i = 0; i < LayerNames.Count; i++) if (LayerNames[i]!= null) LayerDisplay.Items.Add(LayerNames[i]);
                    LayerDisplay.Text = Selection;
                }
            }

            public class layer
            {
                public string name;
                public int id;

                public layer(string n, int index)
                {
                    name = n;
                    id = index;
                }

                public override string ToString()
                {
                    return name;
                }
            }

            private void UpdateForm()
            {
                Abs63Out.Value = (decimal)Abs63.Value / 10;
                Abs125Out.Value = (decimal)Abs125.Value / 10;
                Abs250Out.Value = (decimal)Abs250.Value / 10;
                Abs500Out.Value = (decimal)Abs500.Value / 10;
                Abs1kOut.Value = (decimal)Abs1k.Value / 10;
                Abs2kOut.Value = (decimal)Abs2k.Value / 10;
                Abs4kOut.Value = (decimal)Abs4k.Value / 10;
                Abs8kOut.Value = (decimal)Abs8k.Value / 10;
                Scat63Out.Value = Scat63v.Value;
                Scat125Out.Value = Scat125v.Value;
                Scat250Out.Value = Scat250v.Value;
                Scat500Out.Value = Scat500v.Value;
                Scat1kOut.Value = Scat1kv.Value;
                Scat2kOut.Value = Scat2kv.Value;
                Scat4kOut.Value = Scat4kv.Value;
                Scat8kOut.Value = Scat8kv.Value;
                Trans_63_Out.Value = Trans_63v.Value;
                Trans_125_Out.Value = Trans_125v.Value;
                Trans_250_Out.Value = Trans_250v.Value;
                Trans_500_Out.Value = Trans_500v.Value;
                Trans_1k_Out.Value = Trans_1kv.Value;
                Trans_2k_Out.Value = Trans_2kv.Value;
                Trans_4k_Out.Value = Trans_4kv.Value;
                Trans_8k_Out.Value = Trans_8kv.Value;
            }

            private void Acoustics_Coef_Update(object sender, System.EventArgs e)
            {
                UpdateForm();
                Commit_Layer_Acoustics();
            }

            private void SaveAbs_Click(object sender, EventArgs e)
            {
                foreach (Material MAT in Materials.Abs_List)
                {
                    if (!string.Equals(MAT.Name, Material_Name.Text, StringComparison.OrdinalIgnoreCase)) continue;

                    int si = Material_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_Abs(si);
                    Material_Lib.Items.RemoveAt(si);
                    Materials.Save_Abs_Library();

                    Load_Library();
                    break;
                }

                double[] Abs = new double[8];
                Abs[0] = (double)Abs63Out.Value / 100;
                Abs[1] = (double)Abs125Out.Value / 100;
                Abs[2] = (double)Abs250Out.Value / 100;
                Abs[3] = (double)Abs500Out.Value / 100;
                Abs[4] = (double)Abs1kOut.Value / 100;
                Abs[5] = (double)Abs2kOut.Value / 100;
                Abs[6] = (double)Abs4kOut.Value / 100;
                Abs[7] = (double)Abs8kOut.Value / 100;

                Materials.Add_Unique_Abs(Material_Name.Text, Abs);
                Material_Lib.Items.Add(Material_Name.Text);
                Materials.Save_Abs_Library();

                Load_Library();
            }

            private void SaveIso_Click(object sender, EventArgs e)
            {
                foreach (Material MAT in Materials.TL_List)
                {
                    if (!string.Equals(MAT.Name, IsolationAssemblies.Text, StringComparison.OrdinalIgnoreCase)) continue;

                    int si = Isolation_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_TL(si);
                    Isolation_Lib.Items.RemoveAt(si);
                    Materials.Save_TL_Library();

                    Load_Library();
                    break;
                }

                double[] TL = new double[8];
                TL[0] = (double)TL63.Value;
                TL[1] = (double)TL125.Value;
                TL[2] = (double)TL250.Value;
                TL[3] = (double)TL500.Value;
                TL[4] = (double)TL1k.Value;
                TL[5] = (double)TL2k.Value;
                TL[6] = (double)TL4k.Value;
                TL[7] = (double)TL8k.Value;

                Materials.Add_Unique_TL(IsolationAssemblies.Text, TL);
                Isolation_Lib.Items.Add(IsolationAssemblies.Text);
                Materials.Save_TL_Library();

                Load_Library();
            }

            private void Delete_Material_Click(object sender, EventArgs e)
            {
                if (Delete_Material.Text == "Delete Materials")
                {
                    Delete_Material.Text = "Cancel Deletion Mode";
                    Save_Material.Enabled = false;
                }
                else
                {
                    Delete_Material.Text = "Delete Materials";
                    Save_Material.Enabled = true;
                }
            }

            private void Delete_Isolation_Click(object sender, EventArgs e)
            {
                if (DeleteAssembly.Text == "Delete Assembly")
                {
                    DeleteAssembly.Text = "Cancel Deletion Mode";
                    SaveAssembly.Enabled = false;
                }
                else
                {
                    DeleteAssembly.Text = "Delete Assembly";
                    SaveAssembly.Enabled = true;
                }
            }

            private void Isolation_Lib_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (DeleteAssembly.Text == "Delete Assembly")
                {
                    if (Isolation_Lib.SelectedItem == null) return;
                    string Selection = Isolation_Lib.SelectedItem.ToString();
                    Material_Name.Text = Isolation_Lib.SelectedItem.ToString();
                    Material Mat = Materials.TL_byKey(Selection);

                    TL63.Value = (int)(Mat.Values[0]);
                    TL125.Value = (int)(Mat.Values[1]);
                    TL250.Value = (int)(Mat.Values[2]);
                    TL500.Value = (int)(Mat.Values[3]);
                    TL1k.Value = (int)(Mat.Values[4]);
                    TL2k.Value = (int)(Mat.Values[5]);
                    TL4k.Value = (int)(Mat.Values[6]);
                    TL8k.Value = (int)(Mat.Values[7]);
                    Commit_Layer_Acoustics();

                    Material_Mode(true);
                }
                else
                {
                    int si = Isolation_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_TL(si);
                    Isolation_Lib.Items.RemoveAt(si);
                    Materials.Save_TL_Library();

                    Load_Library();
                }
            }

            private void Material_Lib_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Delete_Material.Text == "Delete Materials")
                {
                    if (Material_Lib.SelectedItem == null) return;
                    string Selection = Material_Lib.SelectedItem.ToString();
                    Material_Name.Text = Material_Lib.SelectedItem.ToString();
                    Material Mat = Materials.Abs_byKey(Selection);
                    
                    Abs63.Value = (int)(Mat.Values[0] * 1000);
                    Abs125.Value = (int)(Mat.Values[1] * 1000);
                    Abs250.Value = (int)(Mat.Values[2] * 1000);
                    Abs500.Value = (int)(Mat.Values[3] * 1000);
                    Abs1k.Value = (int)(Mat.Values[4] * 1000);
                    Abs2k.Value = (int)(Mat.Values[5] * 1000);
                    Abs4k.Value = (int)(Mat.Values[6] * 1000);
                    Abs8k.Value = (int)(Mat.Values[7] * 1000);
                    Commit_Layer_Acoustics();

                    Material_Mode(true);
                }
                else
                {
                    int si = Material_Lib.SelectedIndex;
                    if (si < 0) return;
                    Materials.Delete_Abs(si);
                    Material_Lib.Items.RemoveAt(si);
                    Materials.Save_Abs_Library();

                    Load_Library();
                }
            }

            private void Commit_SmartMaterial(Pach_Absorption_Designer AD)
            {
                //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                int layer_index = (LayerDisplay.SelectedItem as layer).id;
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                if (AD.Is_Finite)
                {
                    layer.SetUserString("ABSType", "Buildup_Finite");
                }
                else
                {
                    layer.SetUserString("ABSType", "Buildup");
                }
                string Buildup = "";
                List<AbsorptionModels.ABS_Layer> Layers = AD.Material_Layers();
                foreach (AbsorptionModels.ABS_Layer Layer in Layers) Buildup += Layer.LayerCode();
                layer.SetUserString("BuildUp", Buildup);

                SmartMat_Display.Series.Clear();

                Material_Mode(false);

                for (int i = 0; i < AD.Polar_Plot().Series.Count; i++) this.SmartMat_Display.Series.Add(AD.Polar_Plot().Series[i]);
            }

            private void Commit_Layer_Acoustics()
            {
                if (LayerDisplay.Text == "") return;
                //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                int layer_index = (LayerDisplay.SelectedItem as layer).id;
                int[] Abs = new int[8];
                int[] Sct = new int[8];
                int[] Trn = null;
                Abs[0] = (int)(Abs63Out.Value * 10);
                Abs[1] = (int)(Abs125Out.Value * 10);
                Abs[2] = (int)(Abs250Out.Value * 10);
                Abs[3] = (int)(Abs500Out.Value * 10);
                Abs[4] = (int)(Abs1kOut.Value * 10);
                Abs[5] = (int)(Abs2kOut.Value * 10);
                Abs[6] = (int)(Abs4kOut.Value * 10);
                Abs[7] = (int)(Abs8kOut.Value * 10);
                Sct[0] = (int)Scat63Out.Value;
                Sct[1] = (int)Scat125Out.Value;
                Sct[2] = (int)Scat250Out.Value;
                Sct[3] = (int)Scat500Out.Value;
                Sct[4] = (int)Scat1kOut.Value;
                Sct[5] = (int)Scat2kOut.Value;
                Sct[6] = (int)Scat4kOut.Value;
                Sct[7] = (int)Scat8kOut.Value;

                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                double[] TL = null;
                if (Trans_Check.Checked)
                {
                    Trn = new int[8];
                    Trn[0] = (int)Trans_63_Out.Value;
                    Trn[1] = (int)Trans_125_Out.Value;
                    Trn[2] = (int)Trans_250_Out.Value;
                    Trn[3] = (int)Trans_500_Out.Value;
                    Trn[4] = (int)Trans_1k_Out.Value;
                    Trn[5] = (int)Trans_2k_Out.Value;
                    Trn[6] = (int)Trans_4k_Out.Value;
                    Trn[7] = (int)Trans_8k_Out.Value;
                    layer.SetUserString("Transmission", "");
                }
                else if (TL_Check.Checked)
                {
                    TL = new double[8];
                    TL[0] = (double)TL63.Value;
                    TL[1] = (double)TL125.Value;
                    TL[2] = (double)TL250.Value;
                    TL[3] = (double)TL500.Value;
                    TL[4] = (double)TL1k.Value;
                    TL[5] = (double)TL2k.Value;
                    TL[6] = (double)TL4k.Value;
                    TL[7] = (double)TL8k.Value;
                    layer.SetUserString("Transmission", PachTools.EncodeTransmissionLoss(TL));
                }

                layer.SetUserString("Acoustics", RC_PachTools.EncodeAcoustics(Abs, Sct, Trn));
                Rhino.RhinoDoc.ActiveDoc.Layers.Modify(layer, layer_index, false);
            }

            public void Set_Phase_Regime(bool Linear_Phase)
            {
                if (Direct_Data == null) return;
                if (this.Linear_Phase == Linear_Phase) return;
                this.Linear_Phase = Linear_Phase;
                if ((Linear_Phase == true && Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System) || (Linear_Phase == false && Audio.Pach_SP.Filter is Audio.Pach_SP.Minimum_Phase_System))
                {
                    for (int i = 0; i < Direct_Data.Length; i++) Direct_Data[i].Create_Filter();
                    for (int i = 0; i < IS_Data.Length; i++) if (IS_Data[i] != null) IS_Data[i].Create_Filter(Direct_Data[i].SWL, 4096);
                    //for (int i = 0; i < Receiver.Length; i++) Receiver[i].Create_Filter();
                }
            }

            private void Trans_Check_CheckedChanged(object sender, EventArgs e)
            {

                if ((sender as CheckBox).Name == "TL_Check")
                {
                    if (TL_Check.Checked) Trans_Check.Checked = false;
                    else
                    {
                        //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                        int layer_index = (LayerDisplay.SelectedItem as layer).id;
                        Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                        layer.SetUserString("Transmission", "");
                        TL63.Value = 0;
                        TL125.Value = 0;
                        TL250.Value = 0;
                        TL500.Value = 0;
                        TL1k.Value = 0;
                        TL2k.Value = 0;
                        TL4k.Value = 0;
                        TL8k.Value = 0;
                    }
                }
                else
                {
                    if (Trans_Check.Checked)
                    {
                        TL_Check.Checked = false;
                        //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                        int layer_index = (LayerDisplay.SelectedItem as layer).id;
                        Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                        layer.SetUserString("Transmission", "");
                        TL63.Value = 0;
                        TL125.Value = 0;
                        TL250.Value = 0;
                        TL500.Value = 0;
                        TL1k.Value = 0;
                        TL2k.Value = 0;
                        TL4k.Value = 0;
                        TL8k.Value = 0;
                    }
                }
                
                Trans_63_Out.Enabled = Trans_Check.Checked;
                Trans_63v.Enabled = Trans_Check.Checked;
                Trans_125_Out.Enabled = Trans_Check.Checked;
                Trans_125v.Enabled = Trans_Check.Checked;
                Trans_250_Out.Enabled = Trans_Check.Checked;
                Trans_250v.Enabled = Trans_Check.Checked;
                Trans_500_Out.Enabled = Trans_Check.Checked;
                Trans_500v.Enabled = Trans_Check.Checked;
                Trans_1k_Out.Enabled = Trans_Check.Checked;
                Trans_1kv.Enabled = Trans_Check.Checked;
                Trans_2k_Out.Enabled = Trans_Check.Checked;
                Trans_2kv.Enabled = Trans_Check.Checked;
                Trans_4k_Out.Enabled = Trans_Check.Checked;
                Trans_4kv.Enabled = Trans_Check.Checked;
                Trans_8k_Out.Enabled = Trans_Check.Checked;
                Trans_8kv.Enabled = Trans_Check.Checked;
                Trans_Flat.Enabled = Trans_Check.Checked;

                TL63.Enabled = TL_Check.Checked;
                TL125.Enabled = TL_Check.Checked;
                TL250.Enabled = TL_Check.Checked;
                TL500.Enabled = TL_Check.Checked;
                TL1k.Enabled = TL_Check.Checked;
                TL2k.Enabled = TL_Check.Checked;
                TL4k.Enabled = TL_Check.Checked;
                TL8k.Enabled = TL_Check.Checked;

                Commit_Layer_Acoustics();
            }

            private void Retrieve_Layer_Acoustics(object sender, EventArgs e)
            {
                //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                int layer_index = (LayerDisplay.SelectedItem as layer).id;
                Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                string AC = layer.GetUserString("Acoustics");
                string TL = layer.GetUserString("Transmission");

                string M = layer.GetUserString("ABSType");
                if (M == "Buildup")
                {
                    Material_Mode(false);
                    string[] BU = layer.GetUserString("Buildup").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                    for (int i = 0; i < BU.Length; i++) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(BU[i]));

                    Environment.Smart_Material sm = new Smart_Material(false, Layers, 44100, 1.2, 343, 2);
                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;
                    for (int i = 0; i < 8; i++) SmartMat_Display.Series[0].Points.DataBindXY(AnglesDeg, sm.Ang_Coef_Oct[i]);
                }
                else if (M == "Buildup_Finite")
                {
                    Material_Mode(false);
                    string[] BU = layer.GetUserString("Buildup").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<AbsorptionModels.ABS_Layer> Layers = new List<AbsorptionModels.ABS_Layer>();
                    for (int i = 0; i < BU.Length; i++) Layers.Add(AbsorptionModels.ABS_Layer.LayerFromCode(BU[i]));

                    Environment.Smart_Material sm = new Smart_Material(false, Layers, 44100, 1.2, 343, 2);
                    double[] AnglesDeg = new double[sm.Angles.Length];
                    for (int i = 0; i < sm.Angles.Length; i++) AnglesDeg[i] = sm.Angles[i].Real;
                    for (int i = 0; i < 8; i++) SmartMat_Display.Series[0].Points.DataBindXY(AnglesDeg, sm.Ang_Coef_Oct[i]);
                }
                else
                {
                    Material_Mode(true);
                }

                if (!string.IsNullOrEmpty(AC))
                {
                    double[] Abs = new double[8];
                    double[] Sct = new double[8];
                    double[] Trn = new double[1];
                    RC_PachTools.DecodeAcoustics(AC, ref Abs, ref Sct, ref Trn);
                    Abs63.Value = (int)(Abs[0] * 1000);
                    Abs125.Value = (int)(Abs[1] * 1000);
                    Abs250.Value = (int)(Abs[2] * 1000);
                    Abs500.Value = (int)(Abs[3] * 1000);
                    Abs1k.Value = (int)(Abs[4] * 1000);
                    Abs2k.Value = (int)(Abs[5] * 1000);
                    Abs4k.Value = (int)(Abs[6] * 1000);
                    Abs8k.Value = (int)(Abs[7] * 1000);
                    Scat63v.Value = (int)(Sct[0] * 100);
                    Scat125v.Value = (int)(Sct[1] * 100);
                    Scat250v.Value = (int)(Sct[2] * 100);
                    Scat500v.Value = (int)(Sct[3] * 100);
                    Scat1kv.Value = (int)(Sct[4] * 100);
                    Scat2kv.Value = (int)(Sct[5] * 100);
                    Scat4kv.Value = (int)(Sct[6] * 100);
                    Scat8kv.Value = (int)(Sct[7] * 100);
                    if (TL != "" && TL != null)
                    {
                        double[] T_Loss = PachTools.DecodeTransmissionLoss(TL);
                        TL63.Value = (decimal)T_Loss[0];
                        TL125.Value = (decimal)T_Loss[1];
                        TL250.Value = (decimal)T_Loss[2];
                        TL500.Value = (decimal)T_Loss[3];
                        TL1k.Value = (decimal)T_Loss[4];
                        TL2k.Value = (decimal)T_Loss[5];
                        TL4k.Value = (decimal)T_Loss[6];
                        TL8k.Value = (decimal)T_Loss[7];
                        TL_Check.Checked = true;
                    }
                    else if (Trn != null && Trn.Length == 8 && Trn.Sum() > 0)
                    {
                        Trans_63v.Value = (int)(Trn[0] * 100);
                        Trans_125v.Value = (int)(Trn[1] * 100);
                        Trans_250v.Value = (int)(Trn[2] * 100);
                        Trans_500v.Value = (int)(Trn[3] * 100);
                        Trans_1kv.Value = (int)(Trn[4] * 100);
                        Trans_2kv.Value = (int)(Trn[5] * 100);
                        Trans_4kv.Value = (int)(Trn[6] * 100);
                        Trans_8kv.Value = (int)(Trn[7] * 100);
                        Trans_Check.Checked = true;
                    }
                    else
                    {
                        Trans_Check.Checked = false; TL_Check.Checked = false;
                    }
                }
                else
                {
                    Abs63.Value = 1;
                    Abs125.Value = 1;
                    Abs250.Value = 1;
                    Abs500.Value = 1;
                    Abs1k.Value = 1;
                    Abs2k.Value = 1;
                    Abs4k.Value = 1;
                    Abs8k.Value = 1;
                    Scat63v.Value = 1;
                    Scat125v.Value = 1;
                    Scat250v.Value = 1;
                    Scat500v.Value = 1;
                    Scat1kv.Value = 1;
                    Scat2kv.Value = 1;
                    Scat4kv.Value = 1;
                    Scat8kv.Value = 1;
                    Trans_63v.Value = 0;
                    Trans_125v.Value = 0;
                    Trans_250v.Value = 0;
                    Trans_500v.Value = 0;
                    Trans_1kv.Value = 0;
                    Trans_2kv.Value = 0;
                    Trans_4kv.Value = 0;
                    Trans_8kv.Value = 0;
                    TL63.Value = 0;
                    TL125.Value = 0;
                    TL250.Value = 0;
                    TL500.Value = 0;
                    TL1k.Value = 0;
                    TL2k.Value = 0;
                    TL4k.Value = 0;
                    TL8k.Value = 0;
                }
            }

            private void ScatFlat_ValueChanged(object sender, EventArgs e)
            {
                Scat63v.Value = ScatFlat.Value;
                Scat125v.Value = ScatFlat.Value;
                Scat250v.Value = ScatFlat.Value;
                Scat500v.Value = ScatFlat.Value;
                Scat1kv.Value = ScatFlat.Value;
                Scat2kv.Value = ScatFlat.Value;
                Scat4kv.Value = ScatFlat.Value;
                Scat8kv.Value = ScatFlat.Value;
            }

            private void TransFlat_ValueChanged(object sender, EventArgs e)
            {
                Trans_63v.Value = Trans_Flat.Value;
                Trans_125v.Value = Trans_Flat.Value;
                Trans_250v.Value = Trans_Flat.Value;
                Trans_500v.Value = Trans_Flat.Value;
                Trans_1kv.Value = Trans_Flat.Value;
                Trans_2kv.Value = Trans_Flat.Value;
                Trans_4kv.Value = Trans_Flat.Value;
                Trans_8kv.Value = Trans_Flat.Value;
            }

            private void AbsFlat_ValueChanged(object sender, EventArgs e)
            {
                Abs63.Value = AbsFlat.Value;
                Abs125.Value = AbsFlat.Value;
                Abs250.Value = AbsFlat.Value;
                Abs500.Value = AbsFlat.Value;
                Abs1k.Value = AbsFlat.Value;
                Abs2k.Value = AbsFlat.Value;
                Abs4k.Value = AbsFlat.Value;
                Abs8k.Value = AbsFlat.Value;
            }

            private void Abs63Out_ValueChanged(object sender, EventArgs e)
            {
                Abs63.Value = (int)(Abs63Out.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Abs125Out_ValueChanged(object sender, EventArgs e)
            {
                Abs125.Value = (int)(Abs125Out.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Abs250Out_ValueChanged(object sender, EventArgs e)
            {
                Abs250.Value = (int)(Abs250Out.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Abs500Out_ValueChanged(object sender, EventArgs e)
            {
                Abs500.Value = (int)(Abs500Out.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Abs1kOut_ValueChanged(object sender, EventArgs e)
            {
                Abs1k.Value = (int)(Abs1kOut.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Abs2kOut_ValueChanged(object sender, EventArgs e)
            {
                Abs2k.Value = (int)(Abs2kOut.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Abs4kOut_ValueChanged(object sender, EventArgs e)
            {
                Abs4k.Value = (int)(Abs4kOut.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Abs8kOut_ValueChanged(object sender, EventArgs e)
            {
                Abs8k.Value = (int)(Abs8kOut.Value * 10);
                Commit_Layer_Acoustics();
            }

            private void Scat63Out_ValueChanged(object sender, EventArgs e)
            {
                Scat63v.Value = (int)Scat63Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Scat125Out_ValueChanged(object sender, EventArgs e)
            {
                Scat125v.Value = (int)Scat125Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Scat250Out_ValueChanged(object sender, EventArgs e)
            {
                Scat250v.Value = (int)Scat250Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Scat500Out_ValueChanged(object sender, EventArgs e)
            {
                Scat500v.Value = (int)Scat500Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Scat1kOut_ValueChanged(object sender, EventArgs e)
            {
                Scat1kv.Value = (int)Scat1kOut.Value;
                Commit_Layer_Acoustics();
            }

            private void Scat2kOut_ValueChanged(object sender, EventArgs e)
            {
                Scat2kv.Value = (int)Scat2kOut.Value;
                Commit_Layer_Acoustics();
            }

            private void Scat4kOut_ValueChanged(object sender, EventArgs e)
            {
                Scat4kv.Value = (int)Scat4kOut.Value;
                Commit_Layer_Acoustics();
            }

            private void Scat8kOut_ValueChanged(object sender, EventArgs e)
            {
                Scat8kv.Value = (int)Scat8kOut.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans63Out_ValueChanged(object sender, EventArgs e)
            {
                Trans_63v.Value = (int)Trans_63_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans125Out_ValueChanged(object sender, EventArgs e)
            {
                Trans_125v.Value = (int)Trans_125_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans250Out_ValueChanged(object sender, EventArgs e)
            {
                Trans_250v.Value = (int)Trans_250_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans500Out_ValueChanged(object sender, EventArgs e)
            {
                Trans_500v.Value = (int)Trans_500_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans1kOut_ValueChanged(object sender, EventArgs e)
            {
                Trans_1kv.Value = (int)Trans_1k_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans2kOut_ValueChanged(object sender, EventArgs e)
            {
                Trans_2kv.Value = (int)Trans_2k_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans4kOut_ValueChanged(object sender, EventArgs e)
            {
                Trans_4kv.Value = (int)Trans_4k_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Trans8kOut_ValueChanged(object sender, EventArgs e)
            {
                Trans_8kv.Value = (int)Trans_8k_Out.Value;
                Commit_Layer_Acoustics();
            }

            private void Abs_Designer_Click(object sender, EventArgs e)
            {
                Pach_Absorption_Designer AD = new Pach_Absorption_Designer();
                AD.ShowDialog();

                switch (AD.Result)
                {
                    case Pach_Absorption_Designer.AbsorptionModelResult.Random_Incidence:
                        //Assign Random Incidence Absorption Coefficients...
                        Material_Mode(true);
                        Abs63Out.Value = (decimal)(AD.RI_Absorption[0] * 100);
                        Abs125Out.Value = (decimal)(AD.RI_Absorption[1] * 100);
                        Abs250Out.Value = (decimal)(AD.RI_Absorption[2] * 100);
                        Abs500Out.Value = (decimal)(AD.RI_Absorption[3] * 100);
                        Abs1kOut.Value = (decimal)(AD.RI_Absorption[4] * 100);
                        Abs2kOut.Value = (decimal)(AD.RI_Absorption[5] * 100);
                        Abs4kOut.Value = (decimal)(AD.RI_Absorption[6] * 100);
                        Abs8kOut.Value = (decimal)(AD.RI_Absorption[7] * 100);
                        UpdateForm();

                        //int layer_index = Rhino.RhinoDoc.ActiveDoc.Layers.Find(LayerDisplay.Text, true);
                        int layer_index = (LayerDisplay.SelectedItem as layer).id;
                        Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[layer_index];
                        layer.SetUserString("ABSType", "Coefficients");

                        Commit_Layer_Acoustics();
                        Material_Mode(true);
                        return;
                    case Pach_Absorption_Designer.AbsorptionModelResult.Smart_Material:
                        //Store and Assign Smart Material.
                        Material_Mode(false);
                        Commit_SmartMaterial(AD);
                        //Commit_Layer_Acoustics();
                        return;
                    default:
                        return; //Do nothing
                }
            }

            private void Material_Mode(bool RI)
            {
                Label6.Visible = RI;
                Label7.Visible = RI;
                Label8.Visible = RI;
                Label9.Visible = RI;
                Label10.Visible = RI;
                Label11.Visible = RI;
                Label12.Visible = RI;
                Label13.Visible = RI;
                Abs63Out.Visible = RI;
                Abs63.Visible = RI;
                Abs125Out.Visible = RI;
                Abs125.Visible = RI;
                Abs250Out.Visible = RI;
                Abs250.Visible = RI;
                Abs500Out.Visible = RI;
                Abs500.Visible = RI;
                Abs1kOut.Visible = RI;
                Abs1k.Visible = RI;
                Abs2kOut.Visible = RI;
                Abs2k.Visible = RI;
                Abs4kOut.Visible = RI;
                Abs4k.Visible = RI;
                Abs8kOut.Visible = RI;
                Abs8k.Visible = RI;
                AbsFlat.Visible = RI;
                label24.Visible = RI;

                SmartMat_Display.Visible = true;

                if (RI)
                {
                    SmartMat_Display.SendToBack();
                }
                else
                {
                    SmartMat_Display.BringToFront();
                }
            }

            #endregion

            #region Tab 3: Data Analysis 

            private void Tab_Selecting(object sender, System.Windows.Forms.TabControlCancelEventArgs e)
            {
                if (Tabs.SelectedTab.Text == "Analysis")
                {
                    Receiver_Choice.Items.Clear();


                    if (Direct_Data != null && Direct_Data[0] != null)
                    {
                        for (int i = 0; i < Direct_Data[0].Rec_Origin.Count(); i++)
                        {
                            Receiver_Choice.Items.Add(i.ToString());
                        }
                    }
                    if (IS_Data != null && IS_Data.Length > 0)
                    {
                        IS_Path_Box_MouseUp(sender, null);
                    }
                    if (Direct_Data != null && Direct_Data.Length > 0 && Direct_Data[0] != null) Update_Graph(null, new System.EventArgs());
                }
                else if (Tabs.SelectedTab.Text == "Processing")
                {
                    Update_Graph(null, new System.EventArgs());
                }
            }

            private void Parameter_Choice_SelectedIndexChanged(object sender, System.EventArgs e)
            {
                Update_Parameters();
            }

            private void Update_Parameters()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.Text != "Sabine RT" && Parameter_Choice.Text != "Eyring RT") { return; }
                double[] Schroeder;

                List<int> SrcIDs = new List<int>();
                foreach (int i in SourceList.CheckedIndices) SrcIDs.Add(i);

                if (SrcIDs.Count == 0) return;

                if (SrcIDs.Count > 1 || SrcIDs.Count < 1)
                {
                    ISOCOMP.Text = "ISO Compliant: No";
                    if (Parameter_Choice.Text == "Strength/Loudness (G)")
                    {
                        SRT1.Text = "62.5 hz. : NA";
                        SRT2.Text = "125 hz. : NA";
                        SRT3.Text = "250 hz. : NA";
                        SRT4.Text = "500 hz. : NA";
                        SRT5.Text = "1000 hz. : NA";
                        SRT6.Text = "2000 hz. : NA";
                        SRT7.Text = "4000 hz. : NA";
                        SRT8.Text = "8000 hz. : NA";
                        return;
                    }
                }
                else if (SrcTypeList[SrcIDs[0]] == "Pseudo-Random" || SrcTypeList[SrcIDs[0]] != "Geodesic")
                {
                    ISOCOMP.Text = "ISO Compliant: No";
                }
                else
                {
                    ISOCOMP.Text = "ISO Compliant: Yes";
                }

                bool Echo10, Echo50;
                double[] EKG, PercEcho;
                double dtime = double.PositiveInfinity;

                foreach (int i in SelectedSources())
                {
                    double d = Direct_Data[i].Min_Time(int.Parse(Receiver_Choice.Text));
                    dtime = Math.Min(dtime, d);
                }

                bool pressure;
                double[][] ETC = new double[8][];
                if (Graph_Type.Text == "Pressure Time Curve")
                {
                    double RhoC = Direct_Data[0].Rho_C[int.Parse(Receiver_Choice.Text)];
                    double[] ETC_BB = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true);
                    ///Need a new PT_Curve method that will apply the correct power to each filter appropriately. This one forces only one power level to be used...)
                    for (int oct = 0; oct < 8; oct++)
                    {
                        ETC[oct] = Audio.Pach_SP.FIR_Bandpass(ETC_BB, oct, SampleRate, 0);
                        for (int i = 0; i < ETC[oct].Length; i++) ETC[oct][i] *= ETC[oct][i] / RhoC;
                    }
                    pressure = true;
                }
                else
                {
                    for (int oct = 0; oct < 8; oct++) ETC[oct] = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, int.Parse(Receiver_Choice.Text), SrcIDs, false);
                    pressure = false;
                }

                switch (Parameter_Choice.Text)
                {
                    case "Early Decay Time":

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(EDT, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        EDT = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(EDT, 2));
                        break;

                    case "T-10":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T10 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T10, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T10 = AcousticalMath.T_X(Schroeder, 10, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T10, 2));
                        break;

                    case "T-15":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T15, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T15 = AcousticalMath.T_X(Schroeder, 15, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T15, 2));
                        break;

                    case "T-20":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T20, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T20 = AcousticalMath.T_X(Schroeder, 20, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T20, 2));
                        break;

                    case "T-30":
                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[0]);
                        double T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT1.Text = string.Format("62.5 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[1]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT2.Text = string.Format("125 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[2]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT3.Text = string.Format("250 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[3]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT4.Text = string.Format("500 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[4]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT5.Text = string.Format("1000 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[5]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT6.Text = string.Format("2000 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[6]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT7.Text = string.Format("4000 hz. : {0} s.", Math.Round(T30, 2));

                        Schroeder = AcousticalMath.Schroeder_Integral(ETC[7]);
                        T30 = AcousticalMath.T_X(Schroeder, 30, SampleRate);

                        SRT8.Text = string.Format("8000 hz. : {0} s.", Math.Round(T30, 2));
                        break;

                    case "Strength/Loudness (G)":

                        double G = AcousticalMath.Strength(ETC[0], Direct_Data[SrcIDs[0]].SWL[0], false);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[1], Direct_Data[SrcIDs[0]].SWL[1], false);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[2], Direct_Data[SrcIDs[0]].SWL[2], false);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[3], Direct_Data[SrcIDs[0]].SWL[3], false);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[4], Direct_Data[SrcIDs[0]].SWL[4], false);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[5], Direct_Data[SrcIDs[0]].SWL[5], false);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[6], Direct_Data[SrcIDs[0]].SWL[6], false);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(G, 2));

                        G = AcousticalMath.Strength(ETC[7], Direct_Data[SrcIDs[0]].SWL[7], false);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(G, 2));
                        break;

                    case "Sound Pressure Level (SPL)":

                        double I = 0;
                        for (int i = 0; i < ETC[0].Length; i++) I += ETC[0][i];
                        double SPL = AcousticalMath.SPL_Intensity(I);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[1].Length; i++) I += ETC[1][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[2].Length; i++) I += ETC[2][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[3].Length; i++) I += ETC[3][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[4].Length; i++) I += ETC[4][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[5].Length; i++) I += ETC[5][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[6].Length; i++) I += ETC[6][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(SPL, 2));

                        I = 0;
                        for (int i = 0; i < ETC[7].Length; i++) I += ETC[7][i];
                        SPL = AcousticalMath.SPL_Intensity(I);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(SPL, 2));
                        break;

                    case "Clarity (C-50)":
                        double C50 = AcousticalMath.Clarity(ETC[0], SampleRate, 0.05, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[1], SampleRate, 0.05, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[2], SampleRate, 0.05, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[3], SampleRate, 0.05, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[4], SampleRate, 0.05, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[5], SampleRate, 0.05, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[6], SampleRate, 0.05, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(C50, 2));

                        C50 = AcousticalMath.Clarity(ETC[7], SampleRate, 0.05, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(C50, 2));
                        break;

                    case "Clarity (C-80)":

                        double C80 = AcousticalMath.Clarity(ETC[0], SampleRate, 0.08, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[1], SampleRate, 0.08, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[2], SampleRate, 0.08, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[3], SampleRate, 0.08, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[4], SampleRate, 0.08, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[5], SampleRate, 0.08, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[6], SampleRate, 0.08, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(C80, 2));

                        C80 = AcousticalMath.Clarity(ETC[7], SampleRate, 0.08, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(C80, 2));
                        break;

                    case "Definition (D-50)":
                        double D50 = AcousticalMath.Definition(ETC[0], SampleRate, 0.05, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[1], SampleRate, 0.05, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[2], SampleRate, 0.05, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[3], SampleRate, 0.05, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[4], SampleRate, 0.05, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[5], SampleRate, 0.05, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[6], SampleRate, 0.05, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0} %", Math.Round(D50, 2));

                        D50 = AcousticalMath.Definition(ETC[7], SampleRate, 0.05, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0} %", Math.Round(D50, 2));
                        break;
                    case "Center Time (TS)":
                        double TS = AcousticalMath.Center_Time(ETC[0], SampleRate, dtime);
                        SRT1.Text = string.Format("62.5 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[1], SampleRate, dtime);
                        SRT2.Text = string.Format("125 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[2], SampleRate, dtime);
                        SRT3.Text = string.Format("250 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[3], SampleRate, dtime);
                        SRT4.Text = string.Format("500 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[4], SampleRate, dtime);
                        SRT5.Text = string.Format("1000 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[5], SampleRate, dtime);
                        SRT6.Text = string.Format("2000 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[6], SampleRate, dtime);
                        SRT7.Text = string.Format("4000 hz. : {0} ms.", Math.Round(TS * 1000, 2));

                        TS = AcousticalMath.Center_Time(ETC[7], SampleRate, dtime);
                        SRT8.Text = string.Format("8000 hz. : {0} ms.", Math.Round(TS * 1000, 2));
                        break;
                    case "Initial Time Delay Gap (ITDG)":
                        double ITDG = AcousticalMath.InitialTimeDelayGap(ETC[0], SampleRate);
                        SRT1.Text = string.Format("62.5 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[1], SampleRate);
                        SRT2.Text = string.Format("125 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[2], SampleRate);
                        SRT3.Text = string.Format("250 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[3], SampleRate);
                        SRT4.Text = string.Format("500 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[4], SampleRate);
                        SRT5.Text = string.Format("1000 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[5], SampleRate);
                        SRT6.Text = string.Format("2000 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[6], SampleRate);
                        SRT7.Text = string.Format("4000 hz. : {0} ms", ITDG);

                        ITDG = AcousticalMath.InitialTimeDelayGap(ETC[7], SampleRate);
                        SRT8.Text = string.Format("8000 hz. : {0} ms", ITDG);
                        break;
                    case "Speech Transmission Index (Explicit)":
                        //Speech Intelligibility Index (Statistical)
                        double[] Noise = new double[8];
                        string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                        if (n != "" && n != null)
                        {
                            string[] ns = n.Split(","[0]);
                            for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                        }
                        else
                        {
                            this.Enabled = false;
                            Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                            n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                            string[] ns = n.Split(","[0]);
                            for (int oct = 0; oct < 8; oct++) Noise[oct] = double.Parse(ns[oct]);
                            this.Enabled = true;
                        }

                        double[] STI = AcousticalMath.Speech_Transmission_Index(ETC, 1.2 * 343, Noise, SampleRate);
                        SRT1.Text = string.Format("General(2003) : {0}", Math.Round(STI[0], 2));
                        SRT2.Text = string.Format("Male : {0}", Math.Round(STI[1], 2));
                        SRT3.Text = string.Format("Female : {0}", Math.Round(STI[2], 2));
                        SRT4.Text = "";
                        SRT5.Text = "";
                        SRT6.Text = "";
                        SRT7.Text = "";
                        SRT8.Text = "";
                        break;
                    case "Modulation Transfer Index (MTI - root STI)":
                        //Speech Intelligibility Index (Statistical)
                        double[] p_2m = new double[8];
                        //for (int oct = 0; oct < 8; oct++) p_2m[oct] = Direct_Data[SrcIDs[0]].EnergyValue(oct, int.Parse(Receiver_Choice.Text))[0] * 413;
                        double[] NoiseM = new double[8];
                        string N = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                        if (N != "")
                        {
                            string[] ns = N.Split(","[0]);
                            for (int oct = 0; oct < 8; oct++) NoiseM[oct] = double.Parse(ns[oct]);
                        }
                        else
                        {
                            this.Enabled = false;
                            Rhino.RhinoApp.RunScript("Pachyderm_BackgroundNoise", false);
                            N = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                            string[] ns = N.Split(","[0]);
                            for (int oct = 0; oct < 8; oct++) NoiseM[oct] = double.Parse(ns[oct]);
                            this.Enabled = true;
                        }

                        double[][] ETCm = new double[8][];
                        for (int oct = 0; oct < 8; oct++)
                        {
                            ETCm[oct] = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, int.Parse(Receiver_Choice.Text), SrcIDs, false);
                            //ETCm[oct] = new double[ptc.Length];
                            //for (int s = 0; s < ptc.Length; s++) ETCm[oct][s] = ptc[s] * ptc[s];
                        }
                        double[] MTI = AcousticalMath.Modulation_Transfer_Index(ETCm, 1.2 * 343, NoiseM, SampleRate);
                        SRT1.Text = "";
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(MTI[0], 2));
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(MTI[1], 2));
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(MTI[2], 2));
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(MTI[3], 2));
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(MTI[4], 2));
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(MTI[5], 2));
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(MTI[6], 2));
                        break;
                    case "Lateral Fraction (LF)":
                        double LF = AcousticalMath.Lateral_Fraction(ETC[0], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 0, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[1], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 1, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[2], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 2, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[3], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 3, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true), SampleRate, dtime, pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[4], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 4, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[5], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 5, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[6], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 6, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(LF, 2));

                        LF = AcousticalMath.Lateral_Fraction(ETC[7], IR_Construction.ETCurve_1d_Tight(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 7, int.Parse(Receiver_Choice.Text), SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, dtime, pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(LF, 2));
                        break;
                    case "Lateral Efficiency (LE)":
                        double LE = AcousticalMath.Lateral_Efficiency(ETC[0], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 0, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[1], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 1, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT2.Text = string.Format("125 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[2], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 2, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT3.Text = string.Format("250 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[3], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 3, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT4.Text = string.Format("500 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[4], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 4, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT5.Text = string.Format("1000 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[5], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 5, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT6.Text = string.Format("2000 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[6], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 6, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT7.Text = string.Format("4000 hz. : {0}", Math.Round(LE, 2));

                        LE = AcousticalMath.Lateral_Efficiency(ETC[7], IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, 7, int.Parse(Receiver_Choice.Text), SrcIDs, false, -(double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1], SampleRate, Direct_Data[SourceList.CheckedIndices[0]].Min_Time(int.Parse(Receiver_Choice.Text)), pressure);
                        SRT8.Text = string.Format("8000 hz. : {0}", Math.Round(LE, 2));
                        break;
                    case "Echo Criterion (Music, 10%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo10);
                        break;
                    case "Echo Criterion (Music, 50%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo50);
                        break;
                    case "Echo Criterion (Speech, 10%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo10);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo10);
                        break;
                    case "Echo Criterion (Speech, 50%)":
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 0, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT1.Text = string.Format("62.5 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 1, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT2.Text = string.Format("125 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 2, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT3.Text = string.Format("250 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 3, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT4.Text = string.Format("500 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 4, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT5.Text = string.Format("1000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 5, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT6.Text = string.Format("2000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 6, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT7.Text = string.Format("4000 hz. : {0}", Echo50);
                        AcousticalMath.EchoCriterion(Audio.Pach_SP.FIR_Bandpass(IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, int.Parse(Receiver_Choice.Text), SrcIDs, false, true), 7, SampleRate, 0), SampleRate, dtime, false, out EKG, out PercEcho, out Echo10, out Echo50);
                        SRT8.Text = string.Format("8000 hz. : {0}", Echo50);
                        break;
                }
            }

            private void IS_Path_Box_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                List<int> Srcs = SelectedSources();
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                foreach (Guid path in ShownPaths)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Delete(path, true);
                }
                ShownPaths.Clear();
                foreach (int s in Srcs)
                {
                    //foreach (string pathid in IS_Path_Box.CheckedItems)    
                    foreach (Deterministic_Reflection Path in IS_Path_Box.CheckedItems)
                    {
                        //Polyline P = IS_Data[s].GetPLINE(pathid, int.Parse(Receiver_Choice.Text));
                        //if (P == null) continue;
                        foreach (Hare.Geometry.Point[] P in Path.Path)
                        {
                            List<Point3d> pts = new List<Point3d>();
                            foreach (Hare.Geometry.Point p in P) pts.Add(Utilities.RC_PachTools.HPttoRPt(p));
                            ShownPaths.Add(Rhino.RhinoDoc.ActiveDoc.Objects.AddPolyline(new Polyline(pts)));
                        }
                    }
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                Update_Graph(null, null);
            }

            private void Update_Graph(object sender, EventArgs e)
            {
                Analysis_View.GraphPane.CurveList.Clear();

                int REC_ID = 0;
                try
                {
                    if (Receiver_Choice.Text == "No Results Calculated...") return;
                    REC_ID = int.Parse(Receiver_Choice.Text);

                    int OCT_ID = PachTools.OctaveStr2Int(Graph_Octave.Text);
                    Analysis_View.GraphPane.Title.Text = "Logarithmic Energy Time Curve";
                    Analysis_View.GraphPane.XAxis.Title.Text = "Time (seconds)";
                    Analysis_View.GraphPane.YAxis.Title.Text = "Sound Pressure Level (dB)";

                    List<int> SrcIDs = new List<int>();
                    foreach (int i in SourceList.CheckedIndices) SrcIDs.Add(i);

                    double[] Filter;
                    double[] Schroeder;
                    double[] Filter2;
                    int zero_sample = 0;
                    switch (Graph_Type.Text)
                    {
                        case "Energy Time Curve":
                            Filter = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false);
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Pressure Time Curve":
                            zero_sample = 4096 / 2;
                            Filter2 = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, true);
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
                            }
                            Filter = new double[Filter2.Length];
                            for (int i = 0; i < Filter.Length; i++)   Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Lateral ETC":
                            Filter = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[1];
                            for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Lateral PTC":
                            zero_sample = 4096 / 2;
                            Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[1];
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
                            }
                            Filter = new double[Filter2.Length];
                            for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Vertical ETC":
                            Filter = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[2];
                            for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Vertical PTC":
                            zero_sample = 4096 / 2;
                            Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[2];
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
                            }
                            Filter = new double[Filter2.Length];
                            for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Fore-Aft ETC":
                            Filter = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, OCT_ID, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true)[0];
                            for (int i = 0; i < Filter.Length; i++) Filter[i] = Math.Abs(Filter[i]);
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        case "Fore-Aft PTC":
                            zero_sample = 4096 / 2;
                            Filter2 = IR_Construction.PTC_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true)[0];
                            if (OCT_ID < 8)
                            {
                                Filter2 = Audio.Pach_SP.FIR_Bandpass(Filter2, OCT_ID, SampleRate, 0);
                            }
                            Filter = new double[Filter2.Length];
                            for (int i = 0; i < Filter.Length; i++) Filter[i] = Filter2[i] * Filter2[i] / Direct_Data[0].Rho_C[REC_ID];
                            Schroeder = AcousticalMath.Schroeder_Integral(Filter);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    //Get the maximum value of the Direct Sound
                    double DirectMagnitude = 0;
                    foreach (int i in SourceList.CheckedIndices)
                    {
                        double[] E = Direct_Data[i].EnergyValue(OCT_ID, REC_ID);
                        for (int j = 0; j < E.Length; j++)
                        {
                            double D = AcousticalMath.SPL_Intensity(E[j]);
                            if (D > DirectMagnitude) DirectMagnitude = D;
                        }
                    }

                    Filter = AcousticalMath.SPL_Intensity_Signal(Filter);
                    Schroeder = AcousticalMath.SPL_Intensity_Signal(Schroeder);

                    List<double> S_time = new List<double>();
                    List<double> S_Power = new List<double>();

                    if (IS_Path_Box.CheckedIndices.Count > 0)
                    {
                        SortedList<double, double> Selected = new SortedList<double, double>();

                        foreach (int i in IS_Path_Box.CheckedIndices)
                        {
                            //int ct = (IS_Path_Box.Items[i] as Deterministic_Reflection).Energy(0, SampleRate).Length;
                            double[] refl = (IS_Path_Box.Items[i] as Deterministic_Reflection).Energy(OCT_ID, SampleRate);
                            for (int j = 0; j < refl.Length; j++)
                            {
                                Selected.Add((IS_Path_Box.Items[i] as Deterministic_Reflection).TravelTime + (double)j / (double)SampleRate, Utilities.AcousticalMath.SPL_Intensity(refl[j]));
                            }
                        }

                        for(int i = 0; i < Selected.Count; i++)
                        {
                            S_time.Add(Selected.Keys[i]);
                            S_Power.Add(Selected.Values[i]);
                        }
                    }

                    if (Normalize_Graph.Checked)
                    {
                        Filter = Utilities.AcousticalMath.Normalize_Function(Filter, S_Power);
                        Schroeder = Utilities.AcousticalMath.Normalize_Function(Schroeder);
                    }

                    double[] time = new double[Filter.Length];
                    for (int i = 0; i < Filter.Length; i++)
                    {
                        time[i] = (double)(i - zero_sample) / SampleRate;
                    }

                    Analysis_View.GraphPane.AddCurve("Schroeder Integral", time, Schroeder, System.Drawing.Color.Red, ZedGraph.SymbolType.None);
                    Analysis_View.GraphPane.AddCurve("Logarithmic Energy Time Curve", time, Filter, System.Drawing.Color.Blue, ZedGraph.SymbolType.None);

                    if (IS_Path_Box.CheckedIndices.Count > 0)
                    {
                        ZedGraph.LineItem IScurve = Analysis_View.GraphPane.AddCurve("Selected IS Paths", S_time.ToArray(), S_Power.ToArray(), System.Drawing.Color.Red, ZedGraph.SymbolType.None);
                        IScurve.Line.IsVisible = false;
                        IScurve.Symbol = new ZedGraph.Symbol(ZedGraph.SymbolType.Diamond, System.Drawing.Color.Red);
                        IScurve.Symbol.Size = 10f;
                        IScurve.Symbol.Border.Color = System.Drawing.Color.Red;
                        IScurve.Symbol.Border.Width = 3f;
                    }

                    if (!LockUserScale.Checked)
                    {
                        Analysis_View.GraphPane.XAxis.Scale.Max = time[time.Length - 1];
                        Analysis_View.GraphPane.XAxis.Scale.Min = time[0];

                        if (Normalize_Graph.Checked)
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = 0;
                            Analysis_View.GraphPane.YAxis.Scale.Min = -100;
                        }
                        else
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = DirectMagnitude + 15;
                            Analysis_View.GraphPane.YAxis.Scale.Min = 0;
                        }
                    }
                    else
                    {
                        double max = Analysis_View.GraphPane.YAxis.Scale.Max;
                        double min = Analysis_View.GraphPane.YAxis.Scale.Min;

                        if (Normalize_Graph.Checked)
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = max;
                            Analysis_View.GraphPane.YAxis.Scale.Min = min;
                        }
                        else
                        {
                            Analysis_View.GraphPane.YAxis.Scale.Max = max;
                            Analysis_View.GraphPane.YAxis.Scale.Min = min;
                        } 
                    }

                    Hare.Geometry.Vector V = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(float)Alt_Choice.Value, true), -(float)Azi_Choice.Value, 0, true);

                    if (Receiver_Choice.SelectedIndex >= 0) ReceiverConduit.Instance.set_direction(Utilities.RC_PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), new Vector3d(V.x, V.y, V.z));
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                }
                catch (Exception x)
                {
                    System.Windows.Forms.MessageBox.Show(x.Message);
                    return;
                }

                Analysis_View.AxisChange();
                Analysis_View.Invalidate();
                Update_Parameters();
            }

            private void Normalize_Graph_CheckedChanged(object sender, EventArgs e)
            {
                LockUserScale.Checked = false;
                Update_Graph(null, new System.EventArgs());
            }

            private List<int> SelectedSources()
            {
                List<int> indices = new List<int>();
                foreach (int i in SourceList.CheckedIndices) indices.Add(i);
                return indices;
            }

            private void SourceList_MouseUp(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right) return;
                Update_Parameters();
                Update_Graph(null, System.EventArgs.Empty);
                OpenAnalysis();
            }

            private void Source_Aim_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Receiver_Choice.SelectedIndex < 0 || Source_Aim.SelectedIndex < 0) return;
                double azi, alt;

                PachTools.World_Angles(Direct_Data[Source_Aim.SelectedIndex].Src.Origin(), Recs[Receiver_Choice.SelectedIndex], true, out alt, out azi);

                Alt_Choice.Value = (decimal)alt;
                Azi_Choice.Value = (decimal)azi;
            }

            public void GetSims(ref Hare.Geometry.Point[] Src, ref Hare.Geometry.Point[] Rec, ref Direct_Sound[] D, ref ImageSourceData[] IS, ref Receiver_Bank[] RT)
            {
                if (Source != null)
                {
                    Src = new Hare.Geometry.Point[Source.Length];
                    for (int i = 0; i < Source.Length; i++) Src[i] = Source[i].H_Origin();
                    Rec = Recs;
                }
                else return;
                if (Direct_Data != null) D = Direct_Data;
                if (IS_Data != null) IS = IS_Data;
                if (Receiver != null) RT = Receiver;
            }

            public bool Auralisation_Ready()
            {
                if (Receiver != null) return true;
                return false;
            }

            private void CalcType_CheckedChanged(object sender, EventArgs e)
            {
                if (ISBox.Checked == false)
                {
                    Specular_Trace.Enabled = false;
                    Spec_RayCount.Enabled = false;
                    return;
                }
                if (RTBox.Checked == true)
                {
                    Specular_Trace.Enabled = false;
                    Spec_RayCount.Enabled = false;
                }
                else
                {
                    Specular_Trace.Enabled = true;
                    Spec_RayCount.Enabled = true;
                }
                if (Specular_Trace.Checked == true)
                {
                    RTBox.Enabled = false;
                    RT_Count.Enabled = false;
                }
                else
                {
                    RTBox.Enabled = true;
                    RT_Count.Enabled = true;
                }
            }

            private void Alt_Choice_ValueChanged(object sender, EventArgs e)
            {
                if (Alt_Choice.Value == 91) Alt_Choice.Value = -90;
                else if (Alt_Choice.Value == -91) Alt_Choice.Value = 90;

                Update_Graph(sender, e);
            }

            private void Azi_Choice_ValueChanged(object sender, EventArgs e)
            {
                if (Azi_Choice.Value == 360) Azi_Choice.Value = 0;
                else if (Azi_Choice.Value == -1) Azi_Choice.Value = 359;

                Update_Graph(sender, e);
            }
            #endregion

            #region ToolstripMenuItems
            private void SaveDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Write_File();
            }

            private void OpenDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                //Pach_Auralisation.Instance.Reset();
                Read_File();
                Update_Graph(null, System.EventArgs.Empty);
            }

            private void Receiver_Choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Parameters();
                Source_Aim_SelectedIndexChanged(null, EventArgs.Empty);
                Update_Graph(null, System.EventArgs.Empty);
                OpenAnalysis();
            }

            private void SaveIntensityResultsToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_Results_Intensity();
            }

            private void savePressureResultsToolStripMenuItem_Click_1(object sender, EventArgs e)
            {
                Plot_Results_Pressure();
            }

            private void saveIntensityPTBFormatToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_PTB_Results_Intensity();
            }

            private void savePressurePTBFormatToolStripMenuItem_Click_1(object sender, EventArgs e)
            {
                Plot_PTB_Results_Pressure();
            }

            private void SavePressureResultsToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_Results_Pressure();
            }

            private void savePressurePTBFormatToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Plot_PTB_Results_Pressure();
            }

            private void saveEDCToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.Text != "Sabine RT" && Parameter_Choice.Text != "Eyring RT") { return; }
                double[][][][] Schroeder = new double[Direct_Data.Length][][][];
                //string[] paramtype = new string[] { "T30/s", "EDT/s", "D/%", "C/dB", "TS/ms", "G/dB", "LF%", "LFC%", "IACC" };//LF/% LFC/% IACC
                string ReceiverLine = "Receiver{0};";
                //double[,,,] ParamValues = new double[SourceList.Items.Count, Recs.Length, 8, paramtype.Length];

                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                sf.DefaultExt = ".txt";
                sf.AddExtension = true;
                sf.Filter = "Text File (*.txt)|*.txt|" + "All Files|";

                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamWriter SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    try
                    {
                        SW.WriteLine("Pachyderm Acoustic Simulation Results");
                        SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                        SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);

                        for (int s = 0; s < Direct_Data.Length; s++)
                        {
                            Schroeder[s] = new double[Recs.Length][][];
                            for (int r = 0; r < Recs.Length; r++)
                            {
                                Schroeder[s][r] = new double[8][];
                                ReceiverLine = "S" + s.ToString() + "/" + "R" + r.ToString() + ";";
                                SW.WriteLine(ReceiverLine + "63;125;250;500;1k;2k;4k;8k");

                                double RhoC = Direct_Data[0].Rho_C[int.Parse(Receiver_Choice.Text)];
                                double[] ETC_BB = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new List<int>() { s }, false, true);

                                for (int oct = 0; oct < 8; oct++)
                                {
                                    ///Need a new PT_Curve method that will apply the correct power to each filter appropriately. This one forces only one power level to be used...)
                                    double[] ETC = Audio.Pach_SP.FIR_Bandpass(ETC_BB, oct, SampleRate, 0);
                                    for (int i = 0; i < ETC.Length; i++) ETC[i] *= ETC[i] / RhoC;
                                    //double[] ETC = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, (int)(CO_TIME.Value / 1000), SampleRate, oct, r, s, false);
                                    Schroeder[s][r][oct] = AcousticalMath.Log10Data(AcousticalMath.Schroeder_Integral(ETC),-100);
                                }

                                string line = ";";

                                for (int i = 0; i < Schroeder[s][r][0].Length; i++) 
                                {
                                    for(int oct = 0; oct < 8; oct++) 
                                    {
                                        line += Math.Round(Schroeder[s][r][oct][i],3).ToString() + ";";
                                    }
                                    SW.WriteLine(line);
                                    line = ";";
                                }

                                SW.WriteLine("");
                            }
                        }
                        SW.Close();
                    }
                    catch (System.Exception)
                    {
                        SW.Close();
                        Rhino.RhinoApp.WriteLine("File is open, and cannot be written over.");
                        return;
                    }
                }
            }

            #endregion

            private void Auralisation_Click(object sender, EventArgs e)
            {
                Rhino.RhinoApp.RunScript("PachyDerm_Auralisation", false);
            }

            private void DelayMod_Click(object sender, EventArgs e)
            {
                //Interface for time selection...
                double t = Direct_Data[SourceList.SelectedIndices[0]].Delay_ms;
                Rhino.Input.RhinoGet.GetNumber("Enter the delay to assign to selected source object(s)...", false, ref t, 0, 200);

                foreach (int id in SourceList.SelectedIndices)
                {
                    Direct_Data[id].Delay_ms = t;
                }
                Update_Graph(null, null);
            }

            private void Source_Power_Mod_Click(object sender, EventArgs e)
            {
                int[] srcs = new int[SourceList.SelectedIndices.Count];
                if (srcs.Length < 1) return;
                SourceList.SelectedIndices.CopyTo(srcs, 0);
                Pachyderm_Acoustic.SourcePowerMod mod = new SourcePowerMod(Direct_Data[srcs[0]].SWL);
                mod.ShowDialog();
                if (mod.accept)
                {
                    //Pressure_Ready = false;
                    foreach (int i in srcs)
                    {
                        double[] factor = Direct_Data[i].Set_Power(mod.Power);
                        IS_Data[i].Set_Power(factor);
                        Receiver[i].Set_Power(factor);
                        Direct_Data[i].Create_Filter();
                        IS_Data[i].Create_Filter(mod.Power, 4096);
                        Receiver[i].Create_Filter();
                    }
                }
                Update_Graph(null, null);
            }

            private void Convergence_CheckedChanged(object sender, EventArgs e)
            {
                if (Spec_Rays.Checked) RT_Count.Enabled = true;
                else RT_Count.Enabled = false;
            }

            private void trackBar1_Scroll(object sender, EventArgs e)
            {
                quart_lambda.Text = user_quart_lambda.Value.ToString() + " mm.";
                double f = 1000d * 343d / (user_quart_lambda.Value * 4);

                if (f > 88) Scat63Out.Value = 20;
                else if (f < 44) Scat63Out.Value = 90;
                else Scat63Out.Value = (decimal)( 20 + 70d * (1 - (f - 44d) / 44d));

                if (f > 196) Scat125Out.Value = 20;
                else if (f < 88) Scat125Out.Value = 90;
                else Scat125Out.Value = (decimal)(20 + 70d * (1 - (f - 88d) / 88d));

                if (f > 392) Scat250Out.Value = 20;
                else if (f < 196) Scat250Out.Value = 90;
                else Scat250Out.Value = (decimal)(20 + 70d * (1 - (f - 196d) / 196d));

                if (f > 784) Scat500Out.Value = 20;
                else if (f < 392) Scat500Out.Value = 90;
                else Scat500Out.Value = (decimal)(20 + 70f * (1 - (f - 392f) / 392f));

                if (f > 1568) Scat1kOut.Value = 20;
                else if (f < 784) Scat1kOut.Value = 90;
                else Scat1kOut.Value = (decimal)(20 + 70f * (1 - (f - 784f) / 784f));

                if (f > 3136) Scat2kOut.Value = 20;
                else if (f < 1568) Scat2kOut.Value = 90;
                else Scat2kOut.Value = (decimal)(20 + 70f * (1 - (f - 1568f) / 1568f));

                if (f > 6272) Scat4kOut.Value = 20;
                else if (f < 3136) Scat4kOut.Value = 90;
                else Scat4kOut.Value = (decimal)(20 + 70f * (1 - (f - 3136f) / 3136f));

                if (f > 12544) Scat8kOut.Value = 20;
                else if (f < 6272) Scat8kOut.Value = 90;
                else Scat8kOut.Value = (decimal)(20 + 70f * (1 - (f - 6272f) / 6272f));
            }

            private void PlasterScatter_Click(object sender, EventArgs e)
            {
                Scat63Out.Value = 25;
                Scat125Out.Value = 25;
                Scat250Out.Value = 25;
                Scat500Out.Value = 25;
                Scat1kOut.Value = 25;
                Scat2kOut.Value = 25;
                Scat4kOut.Value = 25;
                Scat8kOut.Value = 25;
            }

            private void GlassScatter_Click(object sender, EventArgs e)
            {
                Scat63Out.Value = 15;
                Scat125Out.Value = 15;
                Scat250Out.Value = 15;
                Scat500Out.Value = 15;
                Scat1kOut.Value = 15;
                Scat2kOut.Value = 15;
                Scat4kOut.Value = 15;
                Scat8kOut.Value = 15;
            }

            private void SortPaths(object sender, EventArgs e)
            {
                if (sender == toolStripMenuItem3)
                {
                    toolStripMenuItem3.Checked = true;
                    toolStripMenuItem4.Checked = false;
                }
                else if (sender == toolStripMenuItem4)
                {
                    toolStripMenuItem4.Checked = true;
                    toolStripMenuItem3.Checked = false;
                }

                if (toolStripMenuItem3.Checked)
                {
                    //Sort by time.
                    Deterministic_Reflection[] DR = IS_Path_Box.Items.Cast<Deterministic_Reflection>().ToArray();
                    DR = DR.OrderBy<Deterministic_Reflection,double>(i => (i.TravelTime)).ToArray();
                    IS_Path_Box.Items.Clear();
                    IS_Path_Box.Items.AddRange(DR);
                }
                else if (toolStripMenuItem4.Checked)
                {
                    //Sort by order.
                    Deterministic_Reflection[] DR = IS_Path_Box.Items.Cast<Deterministic_Reflection>().ToArray();
                    DR = DR.OrderBy<Deterministic_Reflection, int>(i => (i.Reflection_Sequence.Length)).ToArray();
                    IS_Path_Box.Items.Clear();
                    IS_Path_Box.Items.AddRange(DR);
                }
            }

            private void ISCheckAll_Click(object sender, EventArgs e)
            {
                for (int i = 0; i < IS_Path_Box.Items.Count; i++) IS_Path_Box.SetItemChecked(i, true);
                IS_Path_Box_MouseUp(null, null);
            }

            private void ISUncheckAll_Click(object sender, EventArgs e)
            {
                for (int i = 0; i < IS_Path_Box.Items.Count; i++) IS_Path_Box.SetItemChecked(i, false);
                IS_Path_Box_MouseUp(null, null);
            }
        }
    }
}