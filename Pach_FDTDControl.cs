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
using System.Windows.Forms;
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("7c62fae6-efa7-4c07-af12-cd440049c7fc")]
        public partial class Pach_TD_Numeric_Control: System.Windows.Forms.UserControl
        {
            // This call is required by the Windows Form Designer. 
            public Pach_TD_Numeric_Control()
            {
                InitializeComponent();
                scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);
                Param_Scale.Image = scale.PIC;
            }

            private Pach_Graphics.colorscale scale;
            public delegate void Populator(double Dist);
            Numeric.TimeDomain.Acoustic_Compact_FDTD FDTD;
            WaveConduit P;
            Rhino.Geometry.Mesh[] M;
            List<List<Rhino.Geometry.Point3d>> Pts;
            List<List<double>> Pressure;

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                FC = new ForCall(Forw_proc);
                
                Polygon_Scene Rm = PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, (double) Air_Temp.Value, (double) Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                if (!Rm.Complete) return;

                if (P == null) P = new WaveConduit(scale, new double[2] { (double)this.Param_Min.Value, (double)this.Param_Max.Value }, Rm);
                Rhino.Geometry.Point3d[] Src = PachTools.GetSource();
                Rhino.Geometry.Point3d[] Rec = PachTools.GetReceivers().ToArray();
                if (Src.Length < 1 || Rm == null) Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");

                Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Tone;
                        
                switch (SourceSelect.Text)
                {
                    case "Dirac Pulse":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Dirac_Pulse;
                        break;
                    case "Sine Wave":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Tone;
                        break;
                }
                
                Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(s_type, (double)Frequency_Selection.Value, 1, PachTools.GetSource());
                Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact(Rec);

                FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, (double)Freq_Max.Value, (double)CO_TIME.Value);
                M = new Rhino.Geometry.Mesh[3] { FDTD.m_templateX, FDTD.m_templateY, FDTD.m_templateZ };

                P.SetColorScale(new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12), new double[]{(double)Param_Min.Value, (double)Param_Max.Value});
                P.Enabled = true;

                if (AxisSelect.SelectedIndex == 0) Pos_Select.Maximum = FDTD.xDim-1;

                else if (AxisSelect.SelectedIndex == 1) Pos_Select.Maximum = FDTD.yDim-1;
                else if (AxisSelect.SelectedIndex == 2) Pos_Select.Maximum = FDTD.zDim-1;

                if (Map_Planes.Items.Count == 0) 
                {
                    Pos_Select.Value = Pos_Select.Maximum / 2; 
                    AddPlane_Click(new object(), new EventArgs()); 
                }
            }

            private double CutOffLength()
            {
                return ((double)CO_TIME.Value / 1000) * C_Sound();
            }

            private double C_Sound()
            {
                return AcousticalMath.SoundSpeed((double)Air_Temp.Value);
            }

            System.Threading.Thread T;
            private void LoopStart()
            {
                do
                {
                    if (Running)
                    {
                        FC();
                        System.Threading.Thread.Sleep(100);
                    }
                    else 
                    {
                        System.Threading.Thread.CurrentThread.Abort();
                    }
                }
                while (true);
            }
            
            private delegate void ForCall();
            ForCall FC;

            private void Forw_Click(object sender, EventArgs e)
            {
                Forw_proc();
            }

            private void Show_Field()
            {
                List<int> X = new List<int>(), Y = new List<int>(), Z = new List<int>();

                foreach (CutPlane p in Map_Planes.Items)
                {
                    if (p.axis == 0) X.Add(p.pos);
                    else if (p.axis == 1) Y.Add(p.pos);
                    else if (p.axis == 2) Z.Add(p.pos);
                }

                FDTD.Pressure_Points(ref Pts, ref Pressure, X.ToArray(), Y.ToArray(), Z.ToArray(), 0.00002 * Math.Pow(10,(double)Param_Min.Value/20), false, false, true, Magnitude.Checked);
                P.Populate(X.ToArray(), Y.ToArray(), Z.ToArray(), FDTD.dx, Pressure, M, Magnitude.Checked);
            
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Forw_proc()
            {
                double t = FDTD.Increment();

                Show_Field();
                
                this.Invoke((MethodInvoker)delegate
                {
                    Time_Preview.Text = Math.Round(t,3).ToString();
                });
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            bool Running = false;

            private void Loop_Click(object sender, EventArgs e)
            {
                if (Loop.Text == "Loop")
                {
                    Running = true;
                    Time_Preview.Enabled = false;
                    Loop.Text = "Pause";
                    System.Threading.ParameterizedThreadStart St = new System.Threading.ParameterizedThreadStart(delegate { LoopStart(); });
                    T = new System.Threading.Thread(St);
                    T.Start();
                }
                else
                {
                    Running = false;
                    Time_Preview.Enabled = true;
                    Loop.Text = "Loop";
                }
            }

            private void Param_Max_ValueChanged(object sender, EventArgs e)
            {
                    this.Param3_4.Text = (Param_Max.Value - (Param_Max.Value - Param_Min.Value) / 4).ToString();
                this.Param1_2.Text = (Param_Max.Value - (Param_Max.Value - Param_Min.Value) / 2).ToString();
                this.Param1_4.Text = (Param_Min.Value + (Param_Max.Value - Param_Min.Value) / 4).ToString();

                P.SetColorScale(scale, new double[2] { (double)Param_Min.Value, (double)Param_Max.Value });
            }

            private void Color_Selection_SelectedIndexChanged(object sender, EventArgs e)
            {
                Pach_Graphics.colorscale scale;
                switch (this.Color_Selection.Text)
                {
                    case "R-O-Y-G-B-I-V":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "Y-G-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, Math.PI/3.0, 2.0 / 3.0, 1, 0, 1, 0, false, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "R-O-Y":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 1.0 / 3.0, 1, 0, 1, 0, false, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "W-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 0, 0, 1, -1, false, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "R-M-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 1, 0, 1, -1, false, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    default:
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, Math.PI/2.0, 0, 0, 1, 1, false, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                }
                if (P != null)
                {
                    P.SetColorScale(scale, new double[] { (double)Param_Min.Value, (double)Param_Max.Value });
                }
            }

            private void Output_Click(object sender, EventArgs e)
            {
                System.Windows.Forms.FolderBrowserDialog sf = new System.Windows.Forms.FolderBrowserDialog();
                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    for (int i = 0; i < FDTD.zDim; i++)
                    {
                        System.Drawing.Bitmap BM = new System.Drawing.Bitmap(FDTD.xDim, FDTD.yDim);
                        for (int j = 0; j < FDTD.xDim; j++) for (int k = 0; k < FDTD.yDim; k++)
                        {
                            int V = (int) (255 * ( 20 * Math.Log10(FDTD.P(j,k,i) / 0.00002)/200));
                            V = (V > 200) ? 200 : (V < 0) ? 0 : V;
                            BM.SetPixel(j, k, System.Drawing.Color.FromArgb(255,V,V,V));
                        }
                        BM.Save(sf.SelectedPath + "\\" + i.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }

            private class CutPlane 
            {
                public int pos;
                public int axis;

                public CutPlane(int Axis, int Pos)
                {
                    pos = Pos;
                    axis = Axis;
                }

                public override string ToString()
                {
                    return string.Format("{0} Axis - Pos {1}", axis == 0 ? "X" : axis == 1 ? "Y" : "Z", pos);
                }
            }

            private void AddPlane_Click(object sender, EventArgs e)
            {
                Map_Planes.Items.Add(new CutPlane(AxisSelect.SelectedIndex, (int)Pos_Select.Value));
            }

            private void AxisSelect_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Map_Planes.SelectedIndex < 0) return;

                if (FDTD != null)
                {
                    Pos_Select.Maximum = (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).axis == 0 ? FDTD.xDim-1 : (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).axis == 1 ? FDTD.yDim-1 : FDTD.zDim-1;
                }
                (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).axis = AxisSelect.SelectedIndex;
                Map_Planes.Update();
            }

            private void Pos_Select_ValueChanged(object sender, EventArgs e)
            {
                if (Map_Planes.SelectedIndex < 0) return; 
                (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).pos = (int)Pos_Select.Value;
                Map_Planes.Update();
            }
        }
    }
}