//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2025, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit 
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
using Pachyderm_Acoustic.UI;
using Rhino.UI;
using Eto.Drawing;
using Eto.Forms;

namespace Pachyderm_Acoustic
{
    namespace UI
    {

        public partial class Pach_SourceControl : Panel, IPanel
        {
            private List<Rhino.DocObjects.RhinoObject> Objects = new List<Rhino.DocObjects.RhinoObject>();
            private SourceConduit SC;

            private ComboBox SourceType;

            private Label H63;
            private Label H125;
            private Label H250;
            private Label H500;
            private Label H1k;
            private Label H2k;
            private Label H4k;
            private Label H8k;

            private DynamicLayout PowerTable;
            private NumericStepper SWL0;
            private NumericStepper SWL1;
            private NumericStepper SWL2;
            private NumericStepper SWL3;
            private NumericStepper SWL4;
            private NumericStepper SWL5;
            private NumericStepper SWL6;
            private NumericStepper SWL7;
            private Label SPL1;
            private Label SPL0;
            private Label SPL2;
            private Label SPL3;
            private Label SPL4;
            private Label SPL5;
            private Label SPL6;
            private Label SPL7;
            private NumericStepper Delay_ms;
            private Button Maximum;
            private Button Sensitivity;
            private GroupBox SrcDIR;
            private Label Altlbl;
            private Label Azilbl;
            private NumericStepper Azi;
            private NumericStepper Alt;
            private Label Rotlbl;
            private NumericStepper AxialRot;
            private Button SrcDetails;
            private Label OCT;
            private Label SPL;
            private Label SWL;
            private Label label7;
            private DynamicLayout Directional_Controls;

            public Pach_SourceControl()
            {
                SC = SourceConduit.Instance;
                DynamicLayout Layout = new DynamicLayout();
                Layout.Padding = 8;
                Layout.DefaultPadding = 8;
                Layout.DefaultSpacing = new Size(4, 8);
                this.H63 = new Label();
                this.H125 = new Label();
                this.H250 = new Label();
                this.H500 = new Label();
                this.H1k = new Label();
                this.H2k = new Label();
                this.H4k = new Label();
                this.H8k = new Label();
                this.SPL = new Label();
                this.SWL = new Label();
                this.OCT = new Label();
                this.PowerTable = new DynamicLayout();
                this.PowerTable.Padding = 8;
                this.PowerTable.DefaultPadding = 8;
                this.PowerTable.DefaultSpacing = new Size(4,8);
                this.SWL0 = new NumericStepper();
                this.SWL1 = new NumericStepper();
                this.SWL2 = new NumericStepper();
                this.SWL3 = new NumericStepper();
                this.SWL4 = new NumericStepper();
                this.SWL5 = new NumericStepper();
                this.SWL6 = new NumericStepper();
                this.SWL7 = new NumericStepper();
                this.SPL0 = new Label();
                this.SPL1 = new Label();
                this.SPL2 = new Label();
                this.SPL3 = new Label();
                this.SPL4 = new Label();
                this.SPL5 = new Label();
                this.SPL6 = new Label();
                this.SPL7 = new Label();
                SPL0.TextAlignment = TextAlignment.Center;
                SPL1.TextAlignment = TextAlignment.Center;
                SPL2.TextAlignment = TextAlignment.Center;
                SPL3.TextAlignment = TextAlignment.Center;
                SPL4.TextAlignment = TextAlignment.Center;
                SPL5.TextAlignment = TextAlignment.Center;
                SPL6.TextAlignment = TextAlignment.Center;
                SPL7.TextAlignment = TextAlignment.Center;
                this.SrcDIR = new GroupBox();
                this.Rotlbl = new Label();
                this.SrcDetails = new Button();
                this.AxialRot = new NumericStepper();
                this.Altlbl = new Label();
                this.Azilbl = new Label();
                this.Azi = new NumericStepper();
                this.Alt = new NumericStepper();
                this.label7 = new Label();
                this.Delay_ms = new NumericStepper();
                this.Maximum = new Button();
                this.Sensitivity = new Button();
                this.SourceType = new ComboBox();

                // 
                // SourceType
                // 
                this.SourceType.Items.Add("Geodesic Directional Distribution");
                this.SourceType.Items.Add("Pseudo-Random Directional Distribution");
                this.SourceType.Items.Add("Common Loudspeaker Format");
                this.SourceType.Items.Add("Directional Source");

                this.SourceType.SelectedIndex = 0;
                this.SourceType.SelectedIndexChanged += this.SourceType_DropDownClosed;
                this.SourceType.Width = 250;

                Layout.AddRow(SourceType);

                this.OCT.Text = "Octave";
                this.SWL.Text = "SWL";
                this.SPL.Text = "SPL @ 1m.";
                this.OCT.Width = 150;
                PowerTable.AddRow(OCT, SWL, SPL);

                this.H63.Text = "63 Hz.";
                this.H125.Text = "125 Hz.";
                this.H250.Text = "250 Hz.";
                this.H500.Text = "500 Hz.";
                this.H1k.Text = "1 kHz.";
                this.H2k.Text = "2 kHz.";
                this.H4k.Text = "4 kHz.";
                this.H8k.Text = "8 kHz.";

                this.SWL0.DecimalPlaces = 2;
                this.SWL0.MaxValue = 200;
                this.SWL0.MinValue = 0;
                this.SWL0.Value = 120;
                this.SWL0.ValueChanged += SWL_ValueChanged;
                this.SWL1.DecimalPlaces = 2;
                this.SWL1.MaxValue = 200;
                this.SWL1.MinValue = 0;
                this.SWL1.Value = 120;
                this.SWL1.ValueChanged += SWL_ValueChanged;
                this.SWL2.DecimalPlaces = 2;
                this.SWL2.MaxValue = 200;
                this.SWL2.MinValue = 0;
                this.SWL2.Value = 120;
                this.SWL2.ValueChanged += SWL_ValueChanged;
                this.SWL3.DecimalPlaces = 2;
                this.SWL3.MaxValue = 200;
                this.SWL3.MinValue = 0;
                this.SWL3.Value = 120;
                this.SWL3.ValueChanged += SWL_ValueChanged;
                this.SWL4.DecimalPlaces = 2;
                this.SWL4.MaxValue = 200;
                this.SWL4.MinValue = 0;
                this.SWL4.Value = 120;
                this.SWL4.ValueChanged += SWL_ValueChanged;
                this.SWL5.DecimalPlaces = 2;
                this.SWL5.MaxValue = 200;
                this.SWL5.MinValue = 0;
                this.SWL5.Value = 120;
                this.SWL5.ValueChanged += SWL_ValueChanged;
                this.SWL6.DecimalPlaces = 2;
                this.SWL6.MaxValue = 200;
                this.SWL6.MinValue = 0;
                this.SWL6.Value = 120;
                this.SWL6.ValueChanged += SWL_ValueChanged;
                this.SWL7.DecimalPlaces = 2;
                this.SWL7.MaxValue = 200;
                this.SWL7.MinValue = 0;
                this.SWL7.Value = 120;
                this.SWL7.ValueChanged += SWL_ValueChanged;

                this.SPL0.Text = "109.00";
                this.SPL1.Text = "109.00";
                this.SPL2.Text = "109.00";
                this.SPL3.Text = "109.00";
                this.SPL4.Text = "109.00";
                this.SPL5.Text = "109.00";
                this.SPL6.Text = "109.00";
                this.SPL7.Text = "109.00";

                PowerTable.AddRow(OCT, SWL, SPL);
                PowerTable.AddRow(H63, SWL0, SPL0);
                PowerTable.AddRow(H125, SWL1, SPL1);
                PowerTable.AddRow(H250, SWL2, SPL2);
                PowerTable.AddRow(H500, SWL3, SPL3);
                PowerTable.AddRow(H1k, SWL4, SPL4);
                PowerTable.AddRow(H2k, SWL5, SPL5);
                PowerTable.AddRow(H4k, SWL6, SPL6);
                PowerTable.AddRow(H8k, SWL7, SPL7);

                Layout.AddRow(PowerTable);

                this.label7.Text = "Delay (ms.)";
                this.Delay_ms.DecimalPlaces = 4;
                this.Delay_ms.MaxValue = 360;
                this.Delay_ms.ValueChanged += this.Delay_ms_ValueChanged;

                PowerTable.AddRow(label7, Delay_ms);

                Directional_Controls = new DynamicLayout();
                Directional_Controls.Padding = 8;
                Directional_Controls.DefaultPadding = 8;
                Directional_Controls.DefaultSpacing = new Size(4,8);

                this.Maximum.Text = "Max";
                this.Maximum.Click += this.Maximum_Click;
                this.Sensitivity.Text = "Sensitivity";
                this.Sensitivity.Click += this.Sensitivity_Click;

                Directional_Controls.AddRow(new DynamicLayout(new DynamicRow(Sensitivity, Maximum)));

                this.SrcDIR.Text = "Direction";
                this.SrcDIR.Visible = false;

                this.Rotlbl.Text = "Rotation (Axial):";
                this.Altlbl.Text = "Altitude:";
                this.Azilbl.Text = "Azimuth:";

                this.SrcDetails.Text = "details...";
                this.SrcDetails.Click += this.SrcDetails_Click;
                this.AxialRot.MaxValue = 360;
                this.AxialRot.ValueChanged += this.Rotation_ValueChanged;
                this.Azi.MaxValue = 360;
                this.Azi.ValueChanged += this.Rotation_ValueChanged;
                this.Alt.MaxValue = 90;
                this.Alt.MinValue = -90;
                this.Alt.ValueChanged += this.Rotation_ValueChanged;

                DynamicLayout dir = new DynamicLayout();
                dir.AddRow(Altlbl, null, Alt);
                dir.AddRow(Azilbl, null, Azi);
                dir.AddRow(Rotlbl, null, AxialRot);
                Directional_Controls.AddRow(dir);
                Directional_Controls.AddRow(SrcDetails);

                Directional_Controls.Enabled = true;
                Directional_Controls.Visible = true;

                Layout.AddRow(Directional_Controls);

                this.SizeChanged += (sender, e) => {
                    Layout.Width = this.Width;
                    PowerTable.Width = this.Width;
                    SourceType.Width = this.Width;
                    SPL0.Width = (this.Width - 150) / 2;
                    SWL0.Width = (this.Width - 150) / 2;
                    SPL1.Width = (this.Width - 150) / 2;
                    SWL1.Width = (this.Width - 150) / 2;
                    SPL2.Width = (this.Width - 150) / 2;
                    SWL2.Width = (this.Width - 150) / 2;
                    SPL3.Width = (this.Width - 150) / 2;
                    SWL3.Width = (this.Width - 150) / 2;
                    SPL4.Width = (this.Width - 150) / 2;
                    SWL4.Width = (this.Width - 150) / 2;
                    SPL5.Width = (this.Width - 150) / 2;
                    SWL5.Width = (this.Width - 150) / 2;
                    SPL6.Width = (this.Width - 150) / 2;
                    SWL6.Width = (this.Width - 150) / 2;
                    SPL7.Width = (this.Width - 150) / 2;
                    SWL7.Width = (this.Width - 150) / 2;
                    Directional_Controls.Width = this.Width;
                    Sensitivity.Width = (Width - 40) / 2;
                    Maximum.Width = (Width - 40) / 2;

                    SrcDetails.Width = this.Width - 40;
                    Invalidate();
                };

                this.Content = Layout;
            }

            public void Load_Doc(List<Rhino.DocObjects.RhinoObject> Obj)
            {
                Objects = Obj;

                SWL0.MaxValue = 200;
                SWL1.MaxValue = 200;
                SWL2.MaxValue = 200;
                SWL3.MaxValue = 200;
                SWL4.MaxValue = 200;
                SWL5.MaxValue = 200;
                SWL6.MaxValue = 200;
                SWL7.MaxValue = 200;

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
                        if (Mode == "0" || Mode == "1")
                        {
                            Directional_Controls.Enabled = false;
                            Directional_Controls.Visible = false;
                        }
                        else
                        {
                            Directional_Controls.Enabled = true;
                            Directional_Controls.Visible = true;
                        }

                        SourceType.SelectedIndex = int.Parse(Mode);
                        string Power = Objects[0].Geometry.GetUserString("SWL");
                        double[] SWL = Utilities.PachTools.DecodeSourcePower(Power);
                        SWL0.Value = (double)SWL[0];
                        SWL1.Value = (double)SWL[1];
                        SWL2.Value = (double)SWL[2];
                        SWL3.Value = (double)SWL[3];
                        SWL4.Value = (double)SWL[4];
                        SWL5.Value = (double)SWL[5];
                        SWL6.Value = (double)SWL[6];
                        SWL7.Value = (double)SWL[7];

                        string Aim = Objects[0].Geometry.GetUserString("Aiming");
                        if (!string.IsNullOrEmpty(Aim))
                        {
                            string[] Aims = Aim.Split(';');
                            Alt.Value = double.Parse(Aims[0]);
                            Azi.Value = double.Parse(Aims[1]);
                            AxialRot.Value = double.Parse(Aims[2]);
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
                            Delay_ms.Value = double.Parse(delay);
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
                    //if (!SWL0.HasFocus && !SWL1.HasFocus && !SWL2.HasFocus && !SWL3.HasFocus && !SWL4.HasFocus && !SWL5.HasFocus && !SWL6.HasFocus && !SWL7.HasFocus && !SourceType.HasFocus) return;
                    foreach (RhinoObject OBJ in Objects)
                    {
                        OBJ.Geometry.SetUserString("SourceType", SourceType.SelectedIndex.ToString());
                        OBJ.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(new double[] { (double)SWL0.Value, (double)SWL1.Value, (double)SWL2.Value, (double)SWL3.Value, (double)SWL4.Value, (double)SWL5.Value, (double)SWL6.Value, (double)SWL7.Value }));
                    }
                }
                Load_Doc(Objects);
            }

            private void SWL_ValueChanged(object sender, System.EventArgs e)
            {
                double SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL0.Value), 12);
                SPL0.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL1.Value), 12);
                SPL1.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL2.Value), 12);
                SPL2.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL3.Value), 12);
                SPL3.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL4.Value), 12);
                SPL4.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL5.Value), 12);
                SPL5.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL6.Value), 12);
                SPL6.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                SourcePower = System.Math.Round(1E-12 * System.Math.Pow(10, .1 * (double)SWL7.Value), 12);
                SPL7.Text = Math.Round((10 * System.Math.Log10(SourcePower / (4 * System.Math.PI * 1E-12))), 2).ToString();
                Commit();
            }

            Eto.Forms.OpenFileDialog OF;
            Balloon B;
            String[] CODES;
            string[] ballooncodes = new string[8];
            double[] SWLmax = new double[8];
            double[] SWLnom = new double[8];

            private void Select_Type()
            {
                if (SourceType.SelectedIndex != 2 && SourceType.SelectedIndex != 3)
                {
                    SWL0.MaxValue = 200;
                    SWL1.MaxValue = 200;
                    SWL2.MaxValue = 200;
                    SWL3.MaxValue = 200;
                    SWL4.MaxValue = 200;
                    SWL5.MaxValue = 200;
                    SWL6.MaxValue = 200;
                    SWL7.MaxValue = 200;
                    SrcDetails.Enabled = false;
                    SrcDetails.Visible = false;
                    SrcDIR.Enabled = false;
                    SrcDIR.Visible = false;
                    Commit();
                    return;
                }

                if (SourceType.SelectedIndex == 2)
                {
                    string[] L;
                    try
                    {
                        L = CLF_Read.SecureAccess.Read(Rhino.UI.RhinoEtoApp.MainWindow);
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
                        string Sens = "";
                        string Max = "";

                        for (int i = 0; i < Objects.Count; i++)
                        {

                            Objects[i].Geometry.SetUserString("Model", L[0]);
                            Objects[i].Geometry.SetUserString("FileType", L[1]);

                            if (L.Length == 13)
                            {
                                Sens = L[2];
                                Max = L[3];

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

                                SC.AddBalloon(Objects[i].Attributes.ObjectId, new Speaker_Balloon(new string[] { L[4], L[5], L[6], L[7], L[8], L[9], L[10], L[11] }, L[2], int.Parse(L[1]), Utilities.RCPachTools.RPttoHPt(Objects[i].Geometry.GetBoundingBox(true).Min)));
                            }
                            else
                            {

                                string[] SP = L[2].Split(';');
                                string[] MP = L[3].Split(';');

                                string[] balloon = new string[8];

                                for (int oct = 0; oct < 8; oct++)
                                {
                                    balloon[oct] = "";
                                    int idx = (oct * 3) + 4;

                                    Sens += (10 * (Math.Log10(Math.Pow(10, double.Parse(SP[idx - 4]) / 10) + Math.Pow(10, double.Parse(SP[idx - 3]) / 10) + Math.Pow(10, double.Parse(SP[idx - 2]) / 10)))).ToString() + ";";
                                    Max += (10 * (Math.Log10(Math.Pow(10, double.Parse(MP[idx - 4]) / 10) + Math.Pow(10, double.Parse(MP[idx - 3]) / 10) + Math.Pow(10, double.Parse(MP[idx - 2]) / 10)))).ToString() + ";";

                                    string[] balloon0 = L[idx].Split(';') ;
                                    string[] balloon1 = L[idx + 1].Split(';');
                                    string[] balloon2 = L[idx + 2].Split(';');
                                    for (int r = 0; r < balloon0.Length - 1; r++)
                                    {
                                        balloon[oct] += (10 * (Math.Log10(Math.Pow(10, double.Parse(balloon0[r]) / 10) + Math.Pow(10, double.Parse(balloon1[r]) / 10) + Math.Pow(10, double.Parse(balloon2[r]) / 10)))).ToString() + ";";
                                    }
                                    Objects[i].Geometry.SetUserString("Balloon" + Math.Ceiling(62.5 * Math.Pow(2,oct)).ToString(), balloon[oct]);
                                }

                                Objects[i].Geometry.SetUserString("Sensitivity", Sens);
                                Objects[i].Geometry.SetUserString("SWLMax", Max);

                                //Objects[i].Geometry.SetUserString("Balloon63", L[4]);
                                //Objects[i].Geometry.SetUserString("Balloon125", L[5]);
                                //Objects[i].Geometry.SetUserString("Balloon250", L[6]);
                                //Objects[i].Geometry.SetUserString("Balloon500", L[7]);
                                //Objects[i].Geometry.SetUserString("Balloon1000", L[8]);
                                //Objects[i].Geometry.SetUserString("Balloon2000", L[9]);
                                //Objects[i].Geometry.SetUserString("Balloon4000", L[10]);
                                //Objects[i].Geometry.SetUserString("Balloon8000", L[11]);
                                Objects[i].Geometry.SetUserString("Bands", L[28]);

                                SC.AddBalloon(Objects[i].Attributes.ObjectId, new Speaker_Balloon(balloon, Sens, int.Parse(L[1]), Utilities.RCPachTools.RPttoHPt(Objects[i].Geometry.GetBoundingBox(true).Min)));
                            }

                            Objects[i].Geometry.SetUserString("Aiming", Alt.Value.ToString() + ";" + Azi.Value.ToString() + ";" + AxialRot.Value.ToString());
                            Objects[i].Geometry.SetUserString("Delay", Delay_ms.Value.ToString());
                            
                            SrcDetails.Enabled = true;
                            SrcDetails.Visible = true;
                            SrcDIR.Enabled = true;
                            SrcDIR.Visible = true;

                            string[] strSens = Sens.Split(';');
                            string[] strMSwl = Max.Split(';');
                            float[] SenSwl = new float[8];
                            float[] MSwl = new float[8];

                            for (int oct = 0; oct < 8; oct++)
                            {
                                SenSwl[oct] = float.Parse(strSens[oct]);
                                MSwl[oct] = float.Parse(strMSwl[oct]);
                            }

                            SWL0.MaxValue = (!float.IsInfinity(MSwl[0]) && !float.IsNaN(MSwl[0])) ? (double)MSwl[0] : (double)200;
                            SWL1.MaxValue = (!float.IsInfinity(MSwl[1]) && !float.IsNaN(MSwl[1])) ? (double)MSwl[1] : (double)200;
                            SWL2.MaxValue = (!float.IsInfinity(MSwl[2]) && !float.IsNaN(MSwl[2])) ? (double)MSwl[2] : (double)200;
                            SWL3.MaxValue = (!float.IsInfinity(MSwl[3]) && !float.IsNaN(MSwl[3])) ? (double)MSwl[3] : (double)200;
                            SWL4.MaxValue = (!float.IsInfinity(MSwl[4]) && !float.IsNaN(MSwl[4])) ? (double)MSwl[4] : (double)200;
                            SWL5.MaxValue = (!float.IsInfinity(MSwl[5]) && !float.IsNaN(MSwl[5])) ? (double)MSwl[5] : (double)200;
                            SWL6.MaxValue = (!float.IsInfinity(MSwl[6]) && !float.IsNaN(MSwl[6])) ? (double)MSwl[6] : (double)200;
                            SWL7.MaxValue = (!float.IsInfinity(MSwl[7]) && !float.IsNaN(MSwl[7])) ? (double)MSwl[7] : (double)200;

                            SWL0.Value = (double)SenSwl[0];
                            SWL1.Value = (double)SenSwl[1];
                            SWL2.Value = (double)SenSwl[2];
                            SWL3.Value = (double)SenSwl[3];
                            SWL4.Value = (double)SenSwl[4];
                            SWL5.Value = (double)SenSwl[5];
                            SWL6.Value = (double)SenSwl[6];
                            SWL7.Value = (double)SenSwl[7];

                            Commit();
                        }
                    }
                }
                else if (SourceType.SelectedIndex == 3)
                {
                    Eto.Forms.OpenFileDialog OF = new Eto.Forms.OpenFileDialog();

                    if (OF.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) != Eto.Forms.DialogResult.Cancel)
                    {
                        CODES = Balloon.Read_Generic(OF.FileName);

                        string[] nomcode = CODES[8].Split(';');
                        string[] maxcode = CODES[9].Split(';');
                        for (int oct = 0; oct < 8; oct++)
                        {
                            ballooncodes[oct] = CODES[oct];
                            SWLnom[oct] = double.Parse(nomcode[oct]);
                            SWLmax[oct] = double.Parse(maxcode[oct]);
                        }

                        B = new Balloon(ballooncodes, Utilities.RCPachTools.RPttoHPt(Objects[0].Geometry.GetBoundingBox(true).Min));
                    }
                }
                else if (SourceType.SelectedIndex == 4)
                {
                    //BRAS format CSV intake:
                    OF = new Eto.Forms.OpenFileDialog();
                    OF.Filters.Add("Source Directivity File (*.csv)|*.csv|");
                    OF.Filters.Add("All Files|");
                    OF.CurrentFilterIndex = 0;

                    if (OF.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) != Eto.Forms.DialogResult.Cancel)
                    {
                        CODES = Balloon.Read_Generic(OF.FileName);

                        string[] nomcode = CODES[8].Split(';');
                        string[] maxcode = CODES[9].Split(';');
                        for (int oct = 0; oct < 8; oct++)
                        {
                            ballooncodes[oct] = CODES[oct];
                            SWLnom[oct] = double.Parse(nomcode[oct]);
                            SWLmax[oct] = double.Parse(maxcode[oct]);
                        }

                        B = new Balloon(ballooncodes, Utilities.RCPachTools.RPttoHPt(Objects[0].Geometry.GetBoundingBox(true).Min));
                    }
                }
                else
                {
                    return;
                }

                    if (Objects.Count != 0 && SourceType.SelectedIndex != 2)
                    {
                        for (int i = 0; i < Objects.Count; i++)
                        {
                            Objects[i].Geometry.SetUserString("Sensitivity", CODES[8]);
                            Objects[i].Geometry.SetUserString("SWLMax", CODES[9]);

                            Objects[i].Geometry.SetUserString("Balloon63", ballooncodes[0]);
                            Objects[i].Geometry.SetUserString("Balloon125", ballooncodes[1]);
                            Objects[i].Geometry.SetUserString("Balloon250", ballooncodes[2]);
                            Objects[i].Geometry.SetUserString("Balloon500", ballooncodes[3]);
                            Objects[i].Geometry.SetUserString("Balloon1000", ballooncodes[4]);
                            Objects[i].Geometry.SetUserString("Balloon2000", ballooncodes[5]);
                            Objects[i].Geometry.SetUserString("Balloon4000", ballooncodes[6]);
                            Objects[i].Geometry.SetUserString("Balloon8000", ballooncodes[7]);

                            Objects[i].Geometry.SetUserString("Aiming", Alt.Value.ToString() + ";" + Azi.Value.ToString() + ";" + AxialRot.Value.ToString());
                            Objects[i].Geometry.SetUserString("Delay", Delay_ms.Value.ToString());
                            SC.AddBalloon(Objects[i].Attributes.ObjectId, B);

                            SrcDetails.Enabled = true;
                            SrcDetails.Visible = true;
                            SrcDIR.Enabled = true;
                            SrcDIR.Visible = true;

                            SWL0.MaxValue = (!double.IsInfinity(SWLmax[0]) && !double.IsNaN(SWLmax[0])) ? (double)SWLmax[0] : (double)200;
                            SWL1.MaxValue = (!double.IsInfinity(SWLmax[1]) && !double.IsNaN(SWLmax[1])) ? (double)SWLmax[1] : (double)200;
                            SWL2.MaxValue = (!double.IsInfinity(SWLmax[2]) && !double.IsNaN(SWLmax[2])) ? (double)SWLmax[2] : (double)200;
                            SWL3.MaxValue = (!double.IsInfinity(SWLmax[3]) && !double.IsNaN(SWLmax[3])) ? (double)SWLmax[3] : (double)200;
                            SWL4.MaxValue = (!double.IsInfinity(SWLmax[4]) && !double.IsNaN(SWLmax[4])) ? (double)SWLmax[4] : (double)200;
                            SWL5.MaxValue = (!double.IsInfinity(SWLmax[5]) && !double.IsNaN(SWLmax[5])) ? (double)SWLmax[5] : (double)200;
                            SWL6.MaxValue = (!double.IsInfinity(SWLmax[6]) && !double.IsNaN(SWLmax[6])) ? (double)SWLmax[6] : (double)200;
                            SWL7.MaxValue = (!double.IsInfinity(SWLmax[7]) && !double.IsNaN(SWLmax[7])) ? (double)SWLmax[7] : (double)200;

                            SWL0.Value = (double)SWLnom[0];
                            SWL1.Value = (double)SWLnom[1];
                            SWL2.Value = (double)SWLnom[2];
                            SWL3.Value = (double)SWLnom[3];
                            SWL4.Value = (double)SWLnom[4];
                            SWL5.Value = (double)SWLnom[5];
                            SWL6.Value = (double)SWLnom[6];
                            SWL7.Value = (double)SWLnom[7];

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
                if (!SourceType.HasFocus) return;
                Select_Type();
            }

            private void Rotation_ValueChanged(object sender, System.EventArgs e)
            {
                if (!AxialRot.HasFocus && !Azi.HasFocus && !Alt.HasFocus) return;
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
                SWL0.Value = (!double.IsInfinity(Ult_Levels[0]) && !double.IsNaN(Ult_Levels[0]) && (double)Ult_Levels[0] > SWL0.MinValue) ? (double)Ult_Levels[0] : 0;
                SWL1.Value = (!double.IsInfinity(Ult_Levels[1]) && !double.IsNaN(Ult_Levels[1]) && (double)Ult_Levels[1] > SWL1.MinValue) ? (double)Ult_Levels[1] : 0;
                SWL2.Value = (!double.IsInfinity(Ult_Levels[2]) && !double.IsNaN(Ult_Levels[2]) && (double)Ult_Levels[2] > SWL2.MinValue) ? (double)Ult_Levels[2] : 0;
                SWL3.Value = (!double.IsInfinity(Ult_Levels[3]) && !double.IsNaN(Ult_Levels[3]) && (double)Ult_Levels[3] > SWL3.MinValue) ? (double)Ult_Levels[3] : 0;
                SWL4.Value = (!double.IsInfinity(Ult_Levels[4]) && !double.IsNaN(Ult_Levels[4]) && (double)Ult_Levels[4] > SWL4.MinValue) ? (double)Ult_Levels[4] : 0;
                SWL5.Value = (!double.IsInfinity(Ult_Levels[5]) && !double.IsNaN(Ult_Levels[5]) && (double)Ult_Levels[5] > SWL5.MinValue) ? (double)Ult_Levels[5] : 0;
                SWL6.Value = (!double.IsInfinity(Ult_Levels[6]) && !double.IsNaN(Ult_Levels[6]) && (double)Ult_Levels[6] > SWL6.MinValue) ? (double)Ult_Levels[6] : 0;
                SWL7.Value = (!double.IsInfinity(Ult_Levels[7]) && !double.IsNaN(Ult_Levels[7]) && (double)Ult_Levels[7] > SWL7.MinValue) ? (double)Ult_Levels[7] : 0;
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
                SWL0.Value = (!double.IsInfinity(Ult_Levels[0]) && !double.IsNaN(Ult_Levels[0])) ? (double)Ult_Levels[0] : 0;
                SWL1.Value = (!double.IsInfinity(Ult_Levels[1]) && !double.IsNaN(Ult_Levels[1])) ? (double)Ult_Levels[1] : 0;
                SWL2.Value = (!double.IsInfinity(Ult_Levels[2]) && !double.IsNaN(Ult_Levels[2])) ? (double)Ult_Levels[2] : 0;
                SWL3.Value = (!double.IsInfinity(Ult_Levels[3]) && !double.IsNaN(Ult_Levels[3])) ? (double)Ult_Levels[3] : 0;
                SWL4.Value = (!double.IsInfinity(Ult_Levels[4]) && !double.IsNaN(Ult_Levels[4])) ? (double)Ult_Levels[4] : 0;
                SWL5.Value = (!double.IsInfinity(Ult_Levels[5]) && !double.IsNaN(Ult_Levels[5])) ? (double)Ult_Levels[5] : 0;
                SWL6.Value = (!double.IsInfinity(Ult_Levels[6]) && !double.IsNaN(Ult_Levels[6])) ? (double)Ult_Levels[6] : 0;
                SWL7.Value = (!double.IsInfinity(Ult_Levels[7]) && !double.IsNaN(Ult_Levels[7])) ? (double)Ult_Levels[7] : 0;
            }

            private void Delay_ms_ValueChanged(object sender, EventArgs e)
            {
                if (!Delay_ms.HasFocus) return;
                foreach (RhinoObject OBJ in Objects)
                {
                    OBJ.Geometry.SetUserString("Delay", Delay_ms.Value.ToString());
                    this.SC.Update_Aiming(OBJ.Attributes.ObjectId, (float)Alt.Value, (float)Azi.Value, (float)AxialRot.Value);
                }
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

        public class Pach_SourceControl_Page : ObjectPropertiesPage
        {
            Pach_SourceControl Source_Props = null;

            public Pach_SourceControl_Page()
            { }

            public override string EnglishPageTitle => "Sound Source Control";

            public override System.Drawing.Icon PageIcon(System.Drawing.Size sizeInPixels)
            {
                return Properties.Resources.PIcon1;
            }

             public override object PageControl
            {
                get
                {
                    if (Source_Props == null) 
                        Source_Props = new Pach_SourceControl();
                    return Source_Props;
                }
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

            //public override ObjectType SupportedTypes => Rhino.DocObjects.ObjectType.Point;

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