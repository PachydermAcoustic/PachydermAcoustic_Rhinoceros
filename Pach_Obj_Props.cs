//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2025, Arthur van der Harten 
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
using Rhino.UI;
using Eto.Drawing;
using Eto.Forms;
using System.Linq;
using System;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public partial class Pach_objProps : Panel, IPanel
        {
            FreqSlider Absorption_Controls;
            FreqSlider Scattering_Controls;
            FreqSlider Transmission_Controls;
            internal GroupBox Material_Pref;
            internal RadioButton User_Materials;
            internal RadioButton Acoustics_Layer;
            private Label Abs_Label;
            private Label Scat_Label;
            private Label Trans_Label;

            public Pach_objProps()
            {
                this.Material_Pref = new GroupBox();
                this.Material_Pref.Text = "Materials by:";
                //this.Material_Pref.Width = this.Width / 2;
                //Material_Pref.Height = 100;
                this.User_Materials = new RadioButton();
                this.User_Materials.Text = "Manual Choice";
                this.User_Materials.CheckedChanged += this.User_Materials_CheckedChanged;
                this.Acoustics_Layer = new RadioButton();
                this.Acoustics_Layer.Checked = true;
                this.Acoustics_Layer.Text = "Layer";
                this.Acoustics_Layer.CheckedChanged += this.User_Materials_CheckedChanged;
                Material_Pref.Content = new StackLayout
                {
                    Items = { User_Materials, Acoustics_Layer }
                };

                this.Abs_Label = new Label();
                this.Abs_Label.Text = "Absorption Coefficient";
                this.Absorption_Controls = new FreqSlider(FreqSlider.bands.Octave);
                this.Absorption_Controls.MouseLeave += User_Materials_CheckedChanged;
                this.Scat_Label = new Label();
                this.Scat_Label.Text = "Scattering Coefficient";
                this.Scattering_Controls = new FreqSlider(FreqSlider.bands.Octave);
                this.Scattering_Controls.MouseLeave += User_Materials_CheckedChanged;
                this.Trans_Label = new Label();
                this.Trans_Label.Text = "Transparency Coefficient";
                this.Transmission_Controls = new FreqSlider(FreqSlider.bands.Octave);
                this.Transmission_Controls.MouseLeave += User_Materials_CheckedChanged;

                StackLayout cont = new StackLayout();
                cont.Items.Add(Material_Pref);
                cont.Items.Add(Abs_Label);
                cont.Items.Add(Absorption_Controls);
                cont.Items.Add(Scat_Label);
                cont.Items.Add(Scattering_Controls);
                cont.Items.Add(Trans_Label);
                cont.Items.Add(Transmission_Controls);

                this.SizeChanged += (sender, e) => { Absorption_Controls.resize(this.Width); Scattering_Controls.resize(this.Width); Transmission_Controls.resize(this.Width); };

                this.Content = cont;
            }

            private void User_Materials_CheckedChanged(object sender, System.EventArgs e)
            {
                if (sender is RadioButton)
                {
                    if ((sender as RadioButton).Text == "Manual Choice" && (sender as RadioButton).Checked) { User_Materials.Checked = true; this.Acoustics_Layer.Checked = false; }
                    else if ((sender as RadioButton).Checked) { this.User_Materials.Checked = false; Acoustics_Layer.Checked = true; }
                    this.Invalidate(true);
                }

                Commit();
            }

            public string GetCode()
            {
                return MaterialCode;
            }

            private string MaterialCode;

            public void UpdateForm()
            {
                if (Acoustics_Layer.Checked == true)
                {
                    User_Materials.Checked = false;
                    Abs_Label.Visible = false;
                    Scat_Label.Visible = false;
                    Trans_Label.Visible = false;
                    Absorption_Controls.Enabled = false;
                    Scattering_Controls.Enabled = false;
                    Transmission_Controls.Enabled = false;
                    Absorption_Controls.Visible = false;
                    Scattering_Controls.Visible = false;
                    Transmission_Controls.Visible = false;
                }
                else if (User_Materials.Checked == true)
                {
                    Acoustics_Layer.Checked = false;
                    Abs_Label.Visible = true;
                    Scat_Label.Visible = true;
                    Trans_Label.Visible = true;
                    Absorption_Controls.Enabled = true;
                    Scattering_Controls.Enabled = true;
                    Transmission_Controls.Enabled = true;
                    Absorption_Controls.Visible = true;
                    Scattering_Controls.Visible = true;
                    Transmission_Controls.Visible = true;
                }

                double[] Abs = Absorption_Controls.Value;
                double[] Sct = Scattering_Controls.Value;
                double[] Trn = Transmission_Controls.Value;

                double TrnDet = Trn.Sum();

                if (TrnDet < 1) Trn = new double[1];
                MaterialCode = Utilities.RCPachTools.EncodeAcoustics(Abs, Sct, Trn);
            }

            private void Commit()
            {
                if (Objects.Count != 0)
                {
                    if (Acoustics_Layer.Checked)
                    {
                        User_Materials.Checked = false;
                        for (int i = 0; i < Objects.Count; i++)
                        {
                            Objects[i].Geometry.SetUserString("Acoustics_User", "no");
                        }
                        UpdateForm();
                    }
                    else if (User_Materials.Checked)
                    {
                        Acoustics_Layer.Checked = false;
                        for(int i = 0; i < Objects.Count; i++)
                        {
                            Objects[i].Geometry.SetUserString("Acoustics_User", "yes");
                            UpdateForm();
                            Objects[i].Geometry.SetUserString("Acoustics", MaterialCode);
                        }
                    }
                }
                Invalidate(true);
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
                                MaterialCode = null;
                                return;
                            }
                        }

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
                                Utilities.RCPachTools.DecodeAcoustics(Code, ref Absorption, ref Scattering, ref Transparency);
                                MaterialCode = Code;

                                Absorption_Controls.populate(Absorption);
                                Scattering_Controls.populate(Scattering);
                                Transmission_Controls.populate(Transparency);
                            }
                            else
                            {
                                Clear();
                            }
                        }
                        else
                        {
                            //If the common method is by Layer... (Default) 
                            this.User_Materials.Checked = false;
                            this.Acoustics_Layer.Checked = true;
                            Clear();
                            return;
                        }
                    }
                    else
                    {
                        //By Default, Acoustics will be designated by layer 
                        this.User_Materials.Checked = false;
                        this.Acoustics_Layer.Checked = true;
                        Clear();
                    }
                }
                UpdateForm();
            }

            public void Clear()
            {
                MaterialCode = null;
                Absorption_Controls.Value = new double[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
                Scattering_Controls.Value = new double[8] { 15, 15, 15, 15, 15, 15, 15, 15 };
                Transmission_Controls.Value = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            #region IPanel methods
            public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
            {
                // Called when the panel tab is made visible, in Mac Rhino this will happen
                // for a document panel when a new document becomes active, the previous
                // documents panel will get hidden and the new current panel will get shown.
            }

            public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
            {
                // Called when the panel tab is hidden, in Mac Rhino this will happen
                // for a document panel when a new document becomes active, the previous
                // documents panel will get hidden and the new current panel will get shown.
            }

            public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
            {
                // Called when the document or panel container is closed/destroyed
            }
            #endregion IPanel methods
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

            [Obsolete]
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

            public override object PageControl
            {
                get
                {
                    if (obj_Props == null) obj_Props = new Pach_objProps();
                    return obj_Props;
                }
            }

            Pach_objProps obj_Props = null;
        }
    }
}