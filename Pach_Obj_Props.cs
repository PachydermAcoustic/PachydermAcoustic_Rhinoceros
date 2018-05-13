//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2018, Arthur van der Harten 
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

using System.Collections.Generic;
using Rhino.Geometry;
using Rhino.DocObjects;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public partial class Pach_objProps
        {
            public Pach_objProps()
            {
                InitializeComponent();
            }

            private void User_Materials_CheckedChanged(object sender, System.EventArgs e)
            {
                Commit();
            }

            #region scrolls
            private void Abs63_Scroll(object sender, System.EventArgs e)
            {
                Abs63Out.Text = ((double)Abs63.Value / 100).ToString();
            }
            private void Abs125_Scroll(object sender, System.EventArgs e)
            {
                Abs125Out.Text = ((double)Abs125.Value / 100).ToString();
            }
            private void Abs250_Scroll(object sender, System.EventArgs e)
            {
                Abs250Out.Text = ((double)Abs250.Value / 100).ToString();
            }
            private void Abs500_Scroll(object sender, System.EventArgs e)
            {
                Abs500Out.Text = ((double)Abs500.Value / 100).ToString();
            }
            private void Abs1k_Scroll(object sender, System.EventArgs e)
            {
                Abs1kOut.Text = ((double)Abs1k.Value / 100).ToString();
            }
            private void Abs2k_Scroll(object sender, System.EventArgs e)
            {
                Abs2kOut.Text = ((double)Abs2k.Value / 100).ToString();
            }
            private void Abs4k_Scroll(object sender, System.EventArgs e)
            {
                Abs4kOut.Text = ((double)Abs4k.Value / 100).ToString();
            }
            private void Abs8k_Scroll(object sender, System.EventArgs e)
            {
                Abs8kOut.Text = ((double)Abs8k.Value / 100).ToString();
            }

            private void Scat63v_Scroll(object sender, System.EventArgs e)
            {
                Scat63Out.Text = ((double)Scat63v.Value / 100).ToString();
            }
            private void Scat125v_Scroll(object sender, System.EventArgs e)
            {
                Scat125Out.Text = ((double)Scat125v.Value / 100).ToString();
            }
            private void Scat250v_Scroll(object sender, System.EventArgs e)
            {
                Scat250Out.Text = ((double)Scat250v.Value / 100).ToString();
            }
            private void Scat500v_Scroll(object sender, System.EventArgs e)
            {
                Scat500Out.Text = ((double)Scat500v.Value / 100).ToString();
            }
            private void Scat1kv_Scroll(object sender, System.EventArgs e)
            {
                Scat1kOut.Text = ((double)Scat1kv.Value / 100).ToString();
            }
            private void Scat2kv_Scroll(object sender, System.EventArgs e)
            {
                Scat2kOut.Text = ((double)Scat2kv.Value / 100).ToString();
            }
            private void Scat4kv_Scroll(object sender, System.EventArgs e)
            {
                Scat4kOut.Text = ((double)Scat4kv.Value / 100).ToString();
            }
            private void Scat8kv_Scroll(object sender, System.EventArgs e)
            {
                Scat8kOut.Text = ((double)Scat8kv.Value / 100).ToString();
            }

            private void Trans63v_Scroll(object sender, System.EventArgs e)
            {
                Trans63Out.Text = ((double)Trans63v.Value / 100).ToString();
            }
            private void Trans125v_Scroll(object sender, System.EventArgs e)
            {
                Trans125Out.Text = ((double)Trans125v.Value / 100).ToString();
            }
            private void Trans250v_Scroll(object sender, System.EventArgs e)
            {
                Trans250Out.Text = ((double)Trans250v.Value / 100).ToString();
            }
            private void Trans500v_Scroll(object sender, System.EventArgs e)
            {
                Trans500Out.Text = ((double)Trans500v.Value / 100).ToString();
            }
            private void Trans1kv_Scroll(object sender, System.EventArgs e)
            {
                Trans1kOut.Text = ((double)Trans1kv.Value / 100).ToString();
            }
            private void Trans2kv_Scroll(object sender, System.EventArgs e)
            {
                Trans2kOut.Text = ((double)Trans2kv.Value / 100).ToString();
            }
            private void Trans4kv_Scroll(object sender, System.EventArgs e)
            {
                Trans4kOut.Text = ((double)Trans4kv.Value / 100).ToString();
            }
            private void Trans8kv_Scroll(object sender, System.EventArgs e)
            {
                Trans8kOut.Text = ((double)Trans8kv.Value / 100).ToString();
            }
            #endregion


            public string GetCode()
            {
                return MaterialCode;
            }

            private string MaterialCode;

            private void UpdateValues()
            {
                Abs63Out.Text = ((double)Abs63.Value / 100).ToString();
                Abs125Out.Text = ((double)Abs125.Value / 100).ToString();
                Abs250Out.Text = ((double)Abs250.Value / 100).ToString();
                Abs500Out.Text = ((double)Abs500.Value / 100).ToString();
                Abs1kOut.Text = ((double)Abs1k.Value / 100).ToString();
                Abs2kOut.Text = ((double)Abs2k.Value / 100).ToString();
                Abs4kOut.Text = ((double)Abs4k.Value / 100).ToString();
                Abs8kOut.Text = ((double)Abs8k.Value / 100).ToString();
                Scat63Out.Text = ((double)Scat63v.Value / 100).ToString();
                Scat125Out.Text = ((double)Scat125v.Value / 100).ToString();
                Scat250Out.Text = ((double)Scat250v.Value / 100).ToString();
                Scat500Out.Text = ((double)Scat500v.Value / 100).ToString();
                Scat1kOut.Text = ((double)Scat1kv.Value / 100).ToString();
                Scat2kOut.Text = ((double)Scat2kv.Value / 100).ToString();
                Scat4kOut.Text = ((double)Scat4kv.Value / 100).ToString();
                Scat8kOut.Text = ((double)Scat8kv.Value / 100).ToString();
                Trans63Out.Text = ((double)Trans63v.Value / 100).ToString();
                Trans125Out.Text = ((double)Trans125v.Value / 100).ToString();
                Trans250Out.Text = ((double)Trans250v.Value / 100).ToString();
                Trans500Out.Text = ((double)Trans500v.Value / 100).ToString();
                Trans1kOut.Text = ((double)Trans1kv.Value / 100).ToString();
                Trans2kOut.Text = ((double)Trans2kv.Value / 100).ToString();
                Trans4kOut.Text = ((double)Trans4kv.Value / 100).ToString();
                Trans8kOut.Text = ((double)Trans8kv.Value / 100).ToString();
            }

            public void UpdateForm()
            {
                if (Acoustics_Layer.Checked == true)
                {
                    SettingsTable.Enabled = false;
                    SettingsTable.Visible = false;
                }
                else if (User_Materials.Checked == true)
                {
                    SettingsTable.Enabled = true;
                    SettingsTable.Visible = true;
                }

                int[] Abs = new int[8];
                int[] Sct = new int[8];
                int[] Trn = new int[8];

                Abs[0] = (int)Abs63.Value;
                Abs[1] = (int)Abs125.Value;
                Abs[2] = (int)Abs250.Value;
                Abs[3] = (int)Abs500.Value;
                Abs[4] = (int)Abs1k.Value;
                Abs[5] = (int)Abs2k.Value;
                Abs[6] = (int)Abs4k.Value;
                Abs[7] = (int)Abs8k.Value;
                Sct[0] = (int)Scat63v.Value;
                Sct[1] = (int)Scat125v.Value;
                Sct[2] = (int)Scat250v.Value;
                Sct[3] = (int)Scat500v.Value;
                Sct[4] = (int)Scat1kv.Value;
                Sct[5] = (int)Scat2kv.Value;
                Sct[6] = (int)Scat4kv.Value;
                Sct[7] = (int)Scat8kv.Value;

                int TrnDet = 0;
                Trn[0] = (int)Trans63v.Value;
                TrnDet += Trn[0];
                Trn[1] = (int)Trans125v.Value;
                TrnDet += Trn[1];
                Trn[2] = (int)Trans250v.Value;
                TrnDet += Trn[2];
                Trn[3] = (int)Trans500v.Value;
                TrnDet += Trn[3];
                Trn[4] = (int)Trans1kv.Value;
                TrnDet += Trn[4];
                Trn[5] = (int)Trans2kv.Value;
                TrnDet += Trn[5];
                Trn[6] = (int)Trans4kv.Value;
                TrnDet += Trn[6];
                Trn[7] = (int)Trans8kv.Value;
                TrnDet += Trn[7];

                if (TrnDet < 1) Trn = new int[1];
                MaterialCode = Utilities.RC_PachTools.EncodeAcoustics(Abs, Sct, Trn);

                UpdateValues();
            }

            private void Commit()
            {
                if (Objects.Count != 0)
                {
                    if (Acoustics_Layer.Checked)
                    {
                        for (int i = 0; i < Objects.Count; i++)
                        {
                            Objects[i].Geometry.SetUserString("Acoustics_User", "no");
                            //Rhino.RhinoDoc.ActiveDoc.Objects.ModifyAttributes(obj, obj.Attributes, true);
                        }
                        UpdateForm();
                    }
                    else if (User_Materials.Checked)
                    {
                        //foreach (RhinoObject obj in Objects)
                        for(int i = 0; i < Objects.Count; i++)
                        {
                            Objects[i].Geometry.SetUserString("Acoustics_User", "yes");
                            UpdateForm();
                            Objects[i].Geometry.SetUserString("Acoustics", MaterialCode);
                        }
                        //UpdateForm();
                    }
                }

                Load_Doc(Objects);
            }

            private List<RhinoObject> Objects = new List<RhinoObject>();

            public void Load_Doc(List<RhinoObject> Obj)
            {
                Objects = Obj;

                //Check to see if all objects have the same key values... 
                if (Objects.Count != 0)
                {
                    string Mode = Objects[0].Geometry.GetUserString("Acoustics_User");
                    if (!string.IsNullOrEmpty(Mode))
                    {
                        //Check to see that all objects select materials the same way... 
                        foreach (RhinoObject obj in Objects)
                        {
                            string Mode2 = null;
                            Mode2 = obj.Geometry.GetUserString("Acoustics_User");
                            if (!Mode.Equals(Mode2))
                            {
                                this.User_Materials.Checked = false;
                                this.Acoustics_Layer.Checked = false;
                                //UpdateForm();
                                MaterialCode = null;
                                return;
                            }
                        }
                        //If the common method is user_materials 

                        if (Mode == "yes")
                        {
                            //Check to see if materials are all the same... 
                            string Code = Objects[0].Geometry.GetUserString("Acoustics");
                            this.User_Materials.Checked = true;
                            this.Acoustics_Layer.Checked = false;
                            string Code2 = null;
                            foreach (RhinoObject obj in Objects)
                            {
                                Code2 = obj.Geometry.GetUserString("Acoustics");
                                if (!Code.Equals(Code2))
                                {
                                    //If not the same... 
                                    this.User_Materials.Checked = true;
                                    this.Acoustics_Layer.Checked = false;
                                    //UpdateForm();
                                    Clear();
                                    return;
                                }
                            }
                            //If they are the same... 
                            double[] Absorption = new double[8];
                            double[] Scattering = new double[8];
                            double[] Transparency = new double[8]; //TODO: Finalize Transparency
                            this.User_Materials.Checked = true;
                            this.Acoustics_Layer.Checked = false;
                            //And if there is a predefined value... 
                            if (Code != null)
                            {
                                Utilities.RC_PachTools.DecodeAcoustics(Code, ref Absorption, ref Scattering, ref Transparency);
                                MaterialCode = Code;
                                Abs63.Value = (int)(Absorption[0] * 100);
                                Abs125.Value = (int)(Absorption[1] * 100);
                                Abs250.Value = (int)(Absorption[2] * 100);
                                Abs500.Value = (int)(Absorption[3] * 100);
                                Abs1k.Value = (int)(Absorption[4] * 100);
                                Abs2k.Value = (int)(Absorption[5] * 100);
                                Abs4k.Value = (int)(Absorption[6] * 100);
                                Abs8k.Value = (int)(Absorption[7] * 100);
                                Scat63v.Value = (int)(Scattering[0] * 100);
                                Scat125v.Value = (int)(Scattering[1] * 100);
                                Scat250v.Value = (int)(Scattering[2] * 100);
                                Scat500v.Value = (int)(Scattering[3] * 100);
                                Scat1kv.Value = (int)(Scattering[4] * 100);
                                Scat2kv.Value = (int)(Scattering[5] * 100);
                                Scat4kv.Value = (int)(Scattering[6] * 100);
                                Scat8kv.Value = (int)(Scattering[7] * 100);
                                Trans63v.Value = (int)(Transparency[0] * 100);
                                Trans125v.Value = (int)(Transparency[1] * 100);
                                Trans250v.Value = (int)(Transparency[2] * 100);
                                Trans500v.Value = (int)(Transparency[3] * 100);
                                Trans1kv.Value = (int)(Transparency[4] * 100);
                                Trans2kv.Value = (int)(Transparency[5] * 100);
                                Trans4kv.Value = (int)(Transparency[6] * 100);
                                Trans8kv.Value = (int)(Transparency[7] * 100);

                                //UpdateForm();
                            }
                            else
                            {
                                Clear();
                                //UpdateForm();
                            }
                        }
                        else
                        {
                            //If the common method is by Layer... (Default) 
                            this.User_Materials.Checked = false;
                            this.Acoustics_Layer.Checked = true;
                            Clear();
                            //UpdateForm();
                            return;
                        }
                    }
                    else
                    {
                        //By Default, Acoustics will be designated by layer 
                        this.User_Materials.Checked = false;
                        this.Acoustics_Layer.Checked = true;
                        Clear();
                        //UpdateForm();
                    }
                }
                UpdateForm();
            }

            public void Clear()
            {
                MaterialCode = null;
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
                Trans63v.Value = 0;
                Trans125v.Value = 0;
                Trans250v.Value = 0;
                Trans500v.Value = 0;
                Trans1kv.Value = 0;
                Trans2kv.Value = 0;
                Trans4kv.Value = 0;
                Trans8kv.Value = 0;

                UpdateValues();
            }
        }

        public class Pach_Materials_Page : Rhino.UI.ObjectPropertiesPage
        {
            public Pach_Materials_Page()
            {
            }

            public override string EnglishPageTitle
            {
                get { return "Object Acoustics Settings"; }
            }

            public override System.Drawing.Icon Icon
            {
                get { return Properties.Resources.PIcon1; }
            }

            public List<RhinoObject> GetSelected()
            {
                List<RhinoObject> Selected_Objects = new List<RhinoObject>();
                foreach (RhinoObject R in Rhino.RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(false, false))
                {
                    if (R.ObjectType == ObjectType.Brep ||
                        R.ObjectType == ObjectType.Surface ||
                        R.ObjectType == ObjectType.Extrusion)
                    {
                        Selected_Objects.Add(R);
                    }
                }
                return Selected_Objects;
            }

            public override bool ShouldDisplay(RhinoObject obj)
            {
                if (obj_Props == null) obj_Props = new Pach_objProps();
                List<RhinoObject> Selected = this.GetSelected();
                if (Selected.Count > 0)
                {
                    obj_Props.Load_Doc(Selected);
                    return true;
                }
                return false;
            }

            public override string LocalPageTitle
            {
                get
                {
                    return "Object Acoustics Settings";
                }
            }

            public override System.Windows.Forms.Control PageControl
            {
                get
                {
                    //System.Windows.Forms.MessageBox.Show("PageControl");
                    if (obj_Props == null) obj_Props = new Pach_objProps();
                    return obj_Props;
                }
            }

            Pach_objProps obj_Props = null;
        }
    }
}