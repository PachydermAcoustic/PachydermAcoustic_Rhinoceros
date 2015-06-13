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

using Rhino.DocObjects;
using System;
using System.Collections.Generic;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public partial class Pach_SourceControl : System.Windows.Forms.UserControl
        {
            private List<Rhino.DocObjects.RhinoObject> Objects = new List<Rhino.DocObjects.RhinoObject>();
            private SourceConduit SC;

            public Pach_SourceControl()
            {
                SC = SourceConduit.Instance;
                InitializeComponent();
            }

            public void Load_Doc(List<Rhino.DocObjects.RhinoObject> Obj)
            {
                Objects = Obj;

                SWL0.Maximum = 200;
                SWL1.Maximum = 200;
                SWL2.Maximum = 200;
                SWL3.Maximum = 200;
                SWL4.Maximum = 200;
                SWL5.Maximum = 200;
                SWL6.Maximum = 200;
                SWL7.Maximum = 200;

                //Check to see if all objects have the same key values... 
                string Mode = null;
                if (Objects.Count != 0)
                {
                    Mode = Objects[0].Geometry.GetUserString("SourceType");
                    if (!string.IsNullOrEmpty(Mode))
                    {
                        //Check to see that all sources are the same kind... 
                        foreach (RhinoObject obj in Objects)
                        {
                            string Mode2 = obj.Geometry.GetUserString("SourceType");
                            if (!Mode.Equals(Mode2))
                            {
                                SourceType.Text = "";
                                return;
                            }
                        }
                        if (Mode != "2")
                        {
                            Maximum.Enabled = false;
                            Maximum.Visible = false;
                            Sensitivity.Enabled = false;
                            Sensitivity.Visible = false;
                            SrcDIR.Enabled = false;
                            SrcDIR.Visible = false;
                            SrcDetails.Enabled = false;
                            SrcDetails.Visible = false;
                        }
                        else
                        {
                            Maximum.Enabled = true;
                            Maximum.Visible = true;
                            Sensitivity.Enabled = true;
                            Sensitivity.Visible = true;
                            SrcDIR.Enabled = true;
                            SrcDIR.Visible = true;
                            SrcDetails.Enabled = true;
                            SrcDetails.Visible = true;
                        }

                        SourceType.SelectedIndex = int.Parse(Mode);
                        string Power = Objects[0].Geometry.GetUserString("SWL");
                        double[] SWL = Utilities.PachTools.DecodeSourcePower(Power);
                        SWL0.Value = (decimal)SWL[0];
                        SWL1.Value = (decimal)SWL[1];
                        SWL2.Value = (decimal)SWL[2];
                        SWL3.Value = (decimal)SWL[3];
                        SWL4.Value = (decimal)SWL[4];
                        SWL5.Value = (decimal)SWL[5];
                        SWL6.Value = (decimal)SWL[6];
                        SWL7.Value = (decimal)SWL[7];

                        string phase = Objects[0].Geometry.GetUserString("Phase");
                        if (!string.IsNullOrEmpty(phase))
                        {
                            string[] ph = phase.Split(';');
                            this.Phase0.Value = decimal.Parse(ph[0]);
                            this.Phase1.Value = decimal.Parse(ph[1]);
                            this.Phase2.Value = decimal.Parse(ph[2]);
                            this.Phase3.Value = decimal.Parse(ph[3]);
                            this.Phase4.Value = decimal.Parse(ph[4]);
                            this.Phase5.Value = decimal.Parse(ph[5]);
                            this.Phase6.Value = decimal.Parse(ph[6]);
                            this.Phase7.Value = decimal.Parse(ph[7]);
                        }
                        else
                        {
                            this.Phase0.Value = 0;
                            this.Phase1.Value = 0;
                            this.Phase2.Value = 0;
                            this.Phase3.Value = 0;
                            this.Phase4.Value = 0;
                            this.Phase5.Value = 0;
                            this.Phase6.Value = 0;
                            this.Phase7.Value = 0;
                        }

                        string Aim = Objects[0].Geometry.GetUserString("Aiming");
                        if (!string.IsNullOrEmpty(Aim))
                        {
                            string[] Aims = Aim.Split(';');
                            Alt.Value = decimal.Parse(Aims[0]);
                            Azi.Value = decimal.Parse(Aims[1]);
                            AxialRot.Value = decimal.Parse(Aims[2]);
                        }
                        else
                        {
                            Alt.Value = 0;
                            Azi.Value = 0;
                            AxialRot.Value = 0;
                        }

                        string delay = Objects[0].Geometry.GetUserString("Delay");
                        if (!string.IsNullOrEmpty(delay))
                        {                            
                            Delay_ms.Value = decimal.Parse(delay);
                        }
                        else
                        {
                            Delay_ms.Value = 0;
                        }

                    }
                    else
                    {
                        //Default condition (unspecified source conditions)
                        foreach (RhinoObject OBJ in Objects)
                        {
                            OBJ.Geometry.SetUserString("SourceType", "0");
                            OBJ.Geometry.SetUserString("SWL", "120;120;120;120;120;120;120;120;");
                            OBJ.Geometry.SetUserString("Phase", "0;0;0;0;0;0;0;0");
                            //Rhino.RhinoDoc.ActiveDoc.Objects.ModifyAttributes(OBJ, OBJ.Attributes, true);
                        }
                    }
                }
            }

            private void Commit()
            {
                if (Objects.Count != 0)
                {
                    if (!SWL0.Focused && !SWL1.Focused && !SWL2.Focused && !SWL3.Focused && !SWL4.Focused && !SWL5.Focused && !SWL6.Focused && !SWL7.Focused 
                        && !Phase0.Focused && !Phase1.Focused && !Phase2.Focused && !Phase3.Focused && !Phase4.Focused && !Phase5.Focused && !Phase6.Focused && !Phase7.Focused && !SourceType.Focused) return;
                    foreach (RhinoObject OBJ in Objects)
                    {
                        OBJ.Geometry.SetUserString("SourceType", SourceType.SelectedIndex.ToString());
                        OBJ.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(new double[] { (double)SWL0.Value, (double)SWL1.Value, (double)SWL2.Value, (double)SWL3.Value, (double)SWL4.Value, (double)SWL5.Value, (double)SWL6.Value, (double)SWL7.Value }));
                        OBJ.Geometry.SetUserString("Phase", Utilities.PachTools.EncodeSourcePower(new double[] { (double)Phase0.Value, (double)Phase1.Value, (double)Phase2.Value, (double)Phase3.Value, (double)Phase4.Value, (double)Phase5.Value, (double)Phase6.Value, (double)Phase7.Value }));                        
                    }
                }
                Load_Doc(Objects);
            }

            private void SWL_ValueChanged(object sender, System.EventArgs e)
            {
                double SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL0.Value), 12);
                SPL0.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))),2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL1.Value), 12);
                SPL1.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))),2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL2.Value), 12);
                SPL2.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))),2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL3.Value), 12);
                SPL3.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))),2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL4.Value), 12);
                SPL4.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))),2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL5.Value), 12);
                SPL5.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))),2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL6.Value), 12);
                SPL6.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))),2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL7.Value), 12);
                SPL7.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                Commit();
            }

            private void Select_Type()
            {
                if (SourceType.SelectedIndex != 2)
                {
                    SWL0.Maximum = 200;
                    SWL1.Maximum = 200;
                    SWL2.Maximum = 200;
                    SWL3.Maximum = 200;
                    SWL4.Maximum = 200;
                    SWL5.Maximum = 200;
                    SWL6.Maximum = 200;
                    SWL7.Maximum = 200;
                    SrcDetails.Enabled = false;
                    SrcDetails.Visible = false;
                    SrcDIR.Enabled = false;
                    SrcDIR.Visible = false;
                    //Commit();
                    return;
                }

                string[] L;
                try
                {
                    L = CLF_Read.SecureAccess.Read();
                    if (L == null)
                    {
                        SourceType.SelectedIndex = 0;
                        return;
                    }
                }
                catch (System.Exception)
                {
                    return;
                }


                if (Objects.Count != 0)
                {
                    for (int i = 0; i < Objects.Count; i++)
                    {
                        
                        Objects[i].Geometry.SetUserString("Model", L[0]);
                        Objects[i].Geometry.SetUserString("FileType", L[1]);
                        Objects[i].Geometry.SetUserString("Sensitivity", L[2]);
                        Objects[i].Geometry.SetUserString("SWLMax", L[3]);
                        Objects[i].Geometry.SetUserString("Balloon63", L[4]);
                        Objects[i].Geometry.SetUserString("Balloon125", L[5]);
                        Objects[i].Geometry.SetUserString("Balloon250", L[6]);
                        Objects[i].Geometry.SetUserString("Balloon500", L[7]);
                        Objects[i].Geometry.SetUserString("Balloon1000", L[8]);
                        Objects[i].Geometry.SetUserString("Balloon2000", L[9]);
                        Objects[i].Geometry.SetUserString("Balloon4000", L[10]);
                        Objects[i].Geometry.SetUserString("Balloon8000", L[11]);
                        Objects[i].Geometry.SetUserString("Bands", L[12]);

                        Objects[i].Geometry.SetUserString("Aiming", Alt.Value.ToString() + ";" + Azi.Value.ToString() + ";" + AxialRot.Value.ToString());
                        Objects[i].Geometry.SetUserString("Delay", Delay_ms.Value.ToString());
                        SC.AddBalloon(Objects[i].Attributes.ObjectId, new Speaker_Balloon(new string[] { L[4], L[5], L[6], L[7], L[8], L[9], L[10], L[11] }, L[2], int.Parse(L[1]), Objects[i].Geometry.GetBoundingBox(true).Min));//(int)L.Symmetry(),

                        SrcDetails.Enabled = true;
                        SrcDetails.Visible = true;
                        SrcDIR.Enabled = true;
                        SrcDIR.Visible = true;

                        string[] strSens = L[2].Split(';');
                        string[] strMSwl = L[3].Split(';');
                        float[] SenSwl = new float[8];
                        float[] MSwl = new float[8];

                        for (int oct = 0; oct < 8; oct++)
                        {
                            SenSwl[oct] = float.Parse(strSens[oct]);
                            MSwl[oct] = float.Parse(strMSwl[oct]);
                        }

                        SWL0.Maximum = (!float.IsInfinity(MSwl[0]) && !float.IsNaN(MSwl[0])) ? (decimal)MSwl[0] : (decimal)200;
                        SWL1.Maximum = (!float.IsInfinity(MSwl[1]) && !float.IsNaN(MSwl[1])) ? (decimal)MSwl[1] : (decimal)200;
                        SWL2.Maximum = (!float.IsInfinity(MSwl[2]) && !float.IsNaN(MSwl[2])) ? (decimal)MSwl[2] : (decimal)200;
                        SWL3.Maximum = (!float.IsInfinity(MSwl[3]) && !float.IsNaN(MSwl[3])) ? (decimal)MSwl[3] : (decimal)200;
                        SWL4.Maximum = (!float.IsInfinity(MSwl[4]) && !float.IsNaN(MSwl[4])) ? (decimal)MSwl[4] : (decimal)200;
                        SWL5.Maximum = (!float.IsInfinity(MSwl[5]) && !float.IsNaN(MSwl[5])) ? (decimal)MSwl[5] : (decimal)200;
                        SWL6.Maximum = (!float.IsInfinity(MSwl[6]) && !float.IsNaN(MSwl[6])) ? (decimal)MSwl[6] : (decimal)200;
                        SWL7.Maximum = (!float.IsInfinity(MSwl[7]) && !float.IsNaN(MSwl[7])) ? (decimal)MSwl[7] : (decimal)200;

                        SWL0.Value = (decimal)SenSwl[0];
                        SWL1.Value = (decimal)SenSwl[1];
                        SWL2.Value = (decimal)SenSwl[2];
                        SWL3.Value = (decimal)SenSwl[3];
                        SWL4.Value = (decimal)SenSwl[4];
                        SWL5.Value = (decimal)SenSwl[5];
                        SWL6.Value = (decimal)SenSwl[6];
                        SWL7.Value = (decimal)SenSwl[7];

                        Commit();
                    }
                }
                Load_Doc(Objects);
            }

            private void SrcDetails_Click(object sender, System.EventArgs e)
            {
                if (Objects.Count != 0)
                {
                    foreach (RhinoObject OBJ in Objects)
                    {
                        string S = OBJ.Geometry.GetUserString("Model");
                        Rhino.RhinoApp.WriteLine("Model: " + S + "\n");
                    }
                }

                ///List of Source Object Keys
                ///SourceType - the type of the source by index.
                ///SWL - the user defined sound power levels of the source.
                ///List of optional loudspeaker keys.
                ///Model - speaker model.
                ///Colors - Available colors.
                ///Mounting - description of mounting method
                ///Manufacturer - Manufacturer name.
                ///Website - Manufacturer website
                ///Contact - Measurement Contact
                ///Email - Measurement Contact Email
                ///balloonx - encoded speaker directions, by octave band x.
                ///Aiming - the azimuth and angle values.
            }

            private void SourceType_DropDownClosed(object sender, System.EventArgs e)
            {
                if (!SourceType.Focused) return;
                Select_Type();
            }

            private void Rotation_ValueChanged(object sender, System.EventArgs e)
            {
                if (!AxialRot.Focused && !Azi.Focused && !Alt.Focused) return;
                foreach (RhinoObject OBJ in Objects)
                {
                    OBJ.Geometry.SetUserString("Aiming", Alt.Value.ToString() + ";" + Azi.Value.ToString() + ";" + AxialRot.Value.ToString());
                    this.SC.Update_Aiming(OBJ.Attributes.ObjectId, (float)Alt.Value, (float)Azi.Value, (float)AxialRot.Value);
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Sensitivity_Click(object sender, System.EventArgs e)
            {
                if (Objects.Count == 0) return;
                //Get Max levels of all sources... 
                double[][] levels = new double[Objects.Count][];

                for (int i = 0; i < Objects.Count; i++)
                {
                    string code = Objects[i].Geometry.GetUserString("Sensitivity");
                    levels[i] = Utilities.PachTools.DecodeSourcePower(code);
                }

                double[] Ult_Levels = new double[8];

                for (int oct = 0; oct < 8; oct++)
                {
                    Ult_Levels[oct] = levels[0][oct];
                    for (int v = 1; v < levels.Length; v++)
                    {
                        Ult_Levels[oct] = (Ult_Levels[oct] < levels[v][oct]) ? Ult_Levels[oct] : levels[v][oct];
                    }
                }
                SWL0.Value = (!double.IsInfinity(Ult_Levels[0]) && !double.IsNaN(Ult_Levels[0]) && (decimal)Ult_Levels[0] > SWL0.Minimum) ? (decimal)Ult_Levels[0] : 0;
                SWL1.Value = (!double.IsInfinity(Ult_Levels[1]) && !double.IsNaN(Ult_Levels[1]) && (decimal)Ult_Levels[1] > SWL1.Minimum) ? (decimal)Ult_Levels[1] : 0;
                SWL2.Value = (!double.IsInfinity(Ult_Levels[2]) && !double.IsNaN(Ult_Levels[2]) && (decimal)Ult_Levels[2] > SWL2.Minimum) ? (decimal)Ult_Levels[2] : 0;
                SWL3.Value = (!double.IsInfinity(Ult_Levels[3]) && !double.IsNaN(Ult_Levels[3]) && (decimal)Ult_Levels[3] > SWL3.Minimum) ? (decimal)Ult_Levels[3] : 0;
                SWL4.Value = (!double.IsInfinity(Ult_Levels[4]) && !double.IsNaN(Ult_Levels[4]) && (decimal)Ult_Levels[4] > SWL4.Minimum) ? (decimal)Ult_Levels[4] : 0;
                SWL5.Value = (!double.IsInfinity(Ult_Levels[5]) && !double.IsNaN(Ult_Levels[5]) && (decimal)Ult_Levels[5] > SWL5.Minimum) ? (decimal)Ult_Levels[5] : 0;
                SWL6.Value = (!double.IsInfinity(Ult_Levels[6]) && !double.IsNaN(Ult_Levels[6]) && (decimal)Ult_Levels[6] > SWL6.Minimum) ? (decimal)Ult_Levels[6] : 0;
                SWL7.Value = (!double.IsInfinity(Ult_Levels[7]) && !double.IsNaN(Ult_Levels[7]) && (decimal)Ult_Levels[7] > SWL7.Minimum) ? (decimal)Ult_Levels[7] : 0;
            }

            private void Maximum_Click(object sender, System.EventArgs e)
            {

                if (Objects.Count == 0) return;
                //Get Max levels of all sources... 
                double[][] levels = new double[Objects.Count][];

                for (int i = 0; i < Objects.Count; i++)
                {
                    string code = Objects[i].Geometry.GetUserString("SWLMax");
                    levels[i] = Utilities.PachTools.DecodeSourcePower(code);
                }

                double[] Ult_Levels = new double[8];

                for (int oct = 0; oct < 8; oct++)
                {
                    Ult_Levels[oct] = levels[0][oct];
                    for (int v = 1; v < levels.Length; v++)
                    {
                        Ult_Levels[oct] = (Ult_Levels[oct] < levels[v][oct]) ? Ult_Levels[oct] : levels[v][oct];
                    }
                }
                SWL0.Value = (!double.IsInfinity(Ult_Levels[0]) && !double.IsNaN(Ult_Levels[0])) ? (decimal)Ult_Levels[0] : 0;
                SWL1.Value = (!double.IsInfinity(Ult_Levels[1]) && !double.IsNaN(Ult_Levels[1])) ? (decimal)Ult_Levels[1] : 0;
                SWL2.Value = (!double.IsInfinity(Ult_Levels[2]) && !double.IsNaN(Ult_Levels[2])) ? (decimal)Ult_Levels[2] : 0;
                SWL3.Value = (!double.IsInfinity(Ult_Levels[3]) && !double.IsNaN(Ult_Levels[3])) ? (decimal)Ult_Levels[3] : 0;
                SWL4.Value = (!double.IsInfinity(Ult_Levels[4]) && !double.IsNaN(Ult_Levels[4])) ? (decimal)Ult_Levels[4] : 0;
                SWL5.Value = (!double.IsInfinity(Ult_Levels[5]) && !double.IsNaN(Ult_Levels[5])) ? (decimal)Ult_Levels[5] : 0;
                SWL6.Value = (!double.IsInfinity(Ult_Levels[6]) && !double.IsNaN(Ult_Levels[6])) ? (decimal)Ult_Levels[6] : 0;
                SWL7.Value = (!double.IsInfinity(Ult_Levels[7]) && !double.IsNaN(Ult_Levels[7])) ? (decimal)Ult_Levels[7] : 0;
            }

            private void Delay_ms_ValueChanged(object sender, EventArgs e)
            {
                if (!Delay_ms.Focused) return;
                foreach (RhinoObject OBJ in Objects)
                {
                    OBJ.Geometry.SetUserString("Delay", Delay_ms.Value.ToString());
                    this.SC.Update_Aiming(OBJ.Attributes.ObjectId, (float)Alt.Value, (float)Azi.Value, (float)AxialRot.Value);
                }
            }
        }

        public class Pach_SourceControl_Page : Rhino.UI.ObjectPropertiesPage
        {
            Pach_SourceControl Source_Props = null;

            public Pach_SourceControl_Page()
            { }

            public override string EnglishPageTitle
            {
                get { return "Sound Source Control"; }
            }

            public override System.Drawing.Icon Icon
            {
                get { return Properties.Resources.PIcon1; }
            }

            public override System.Windows.Forms.Control PageControl
            {
                get { return Source_Props; }
            }

            public List<RhinoObject> GetSelected()
            {
                List<RhinoObject> Selected_Objects = new List<RhinoObject>();
                foreach (RhinoObject R in Rhino.RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(false, false))
                {
                    if (R.ObjectType == Rhino.DocObjects.ObjectType.Point && R.Attributes.Name == "Acoustical Source")
                    {
                        Selected_Objects.Add(R);
                    }
                }
                return Selected_Objects;
            }

            public override bool ShouldDisplay(RhinoObject obj)
            {
                if (Source_Props == null) Source_Props = new Pach_SourceControl();

                List<RhinoObject> Selected = GetSelected();
                if (Selected.Count > 0)
                {
                    Source_Props.Load_Doc(Selected);
                    return true;
                }
                //default case is not to support the selected object type 
                return false;
            }

            public override string LocalPageTitle
            {
                get
                {
                    return "Sound Source Control";
                }
            }
        }
    }
}