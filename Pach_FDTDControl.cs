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
using System.Linq;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("7c62fae6-efa7-4c07-af12-cd440049c7fc")]
        public partial class Pach_TD_Numeric_Control : System.Windows.Forms.UserControl
        {
            // This call is required by the Windows Form Designer. 
            public Pach_TD_Numeric_Control()
            {
                InitializeComponent();
                Selected_Extent.SelectedIndex = 4;
                scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);
                Param_Scale.Image = scale.PIC;
            }

            #region Visualization

            private Pach_Graphics.colorscale scale;
            public delegate void Populator(double Dist);
            Numeric.TimeDomain.Acoustic_Compact_FDTD FDTD;
            WaveConduit P;
            Rhino.Geometry.Mesh[][] M;
            List<List<Rhino.Geometry.Point3d>> Pts;
            List<List<double>> Pressure;

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                FC = new ForCall(Forw_proc);

                Polygon_Scene Rm = PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                if (!Rm.Complete) return;

                if (P == null) P = new WaveConduit(scale, new double[2] { (double)this.Param_Min.Value, (double)this.Param_Max.Value });
                Rhino.Geometry.Point3d[] Src = PachTools.GetSource();
                Rhino.Geometry.Point3d[] Rec = new Rhino.Geometry.Point3d[0];//PachTools.GetReceivers().ToArray();
                if (Src.Length < 1 || Rm == null) Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");

                Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Dirac_Pulse;

                switch (SourceSelect.Text)
                {
                    case "Dirac Pulse":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Dirac_Pulse;
                        break;
                    case "Sine Wave":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Tone;
                        break;
                    case "Sine Pulse":
                        s_type = Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse;
                        break;
                }

                Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(s_type, (double)Frequency_Selection.Value, 1, PachTools.GetSource());
                Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact(Rec);

                FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, (double)Freq_Max.Value, (double)CO_TIME.Value);
                M = new Rhino.Geometry.Mesh[3][] { FDTD.m_templateX, FDTD.m_templateY, FDTD.m_templateZ };

                P.SetColorScale(new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12), new double[] { (double)Param_Min.Value, (double)Param_Max.Value });
                P.Enabled = true;

                if (AxisSelect.SelectedIndex == 0) Pos_Select.Maximum = FDTD.xDim - 1;
                else if (AxisSelect.SelectedIndex == 1) Pos_Select.Maximum = FDTD.yDim - 1;
                else if (AxisSelect.SelectedIndex == 2) Pos_Select.Maximum = FDTD.zDim - 1;

                if (Map_Planes.Items.Count == 0)
                {
                    Pos_Select.Value = Pos_Select.Maximum / 2;
                    AddPlane_Click(new object(), new EventArgs());
                }

                Loop_Click(new object(), new EventArgs());
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

                FDTD.Pressure_Points(ref Pts, ref Pressure, X.ToArray(), Y.ToArray(), Z.ToArray(), 0.00002 * Math.Pow(10, (double)Param_Min.Value / 20), false, false, true, Magnitude.Checked);
                P.Populate(X.ToArray(), Y.ToArray(), Z.ToArray(), FDTD.dx, Pressure, M, Magnitude.Checked);

                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Forw_proc()
            {
                double t = FDTD.Increment();

                Show_Field();

                this.Invoke((MethodInvoker)delegate
                {
                    Time_Preview.Text = Math.Round(t, 3).ToString();
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
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, Math.PI / 3.0, 2.0 / 3.0, 1, 0, 1, 0, false, 12);
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
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, Math.PI / 2.0, 0, 0, 1, 1, false, 12);
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
                        for (int j = 0; j < FDTD.xDim; j++)
                            for (int k = 0; k < FDTD.yDim; k++)
                            {
                                int V = (int)(255 * (20 * Math.Log10(FDTD.P(j, k, i) / 0.00002) / 200));
                                V = (V > 200) ? 200 : (V < 0) ? 0 : V;
                                BM.SetPixel(j, k, System.Drawing.Color.FromArgb(255, V, V, V));
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
                (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).axis = AxisSelect.SelectedIndex;

                if (FDTD != null)
                {
                    Pos_Select.Maximum = (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).axis == 0 ? (int)(FDTD.xDim - 1) : (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).axis == 1 ? (int)(FDTD.yDim - 1) : (int)(FDTD.zDim - 1);
                }

                Map_Planes.Refresh();
            }

            private void Pos_Select_ValueChanged(object sender, EventArgs e)
            {
                if (Map_Planes.SelectedIndex < 0) return;
                (Map_Planes.Items[Map_Planes.SelectedIndex] as CutPlane).pos = (int)Pos_Select.Value;
                Map_Planes.Update();
            }

            #endregion

            #region Simulation
            double samplefrequency;

            private void CalculateSim_Click(object sender, EventArgs e)
            {
                EigenFrequencies.Items.Clear();
                Chosenfreq = 0;
                FC = new ForCall(Forw_proc);

                Polygon_Scene Rm = PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                if (!Rm.Complete) return;

                Rhino.Geometry.Point3d[] Src = PachTools.GetSource();
                Rhino.Geometry.Point3d[] Rec = PachTools.GetReceivers().ToArray();
                if (Src.Length < 1 || Rm == null) Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");

                Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact(Rec);
                double fs = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Selected_Extent.SelectedIndex);
                //samplefrequency = fs;
                Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, 1000, 1, PachTools.GetSource());
                FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, fs, (double)CO_TIME.Value);
                FDTD.RuntoCompletion();

                samplefrequency = FDTD.SampleFrequency;

                Mic.reset();
                result_signals = Mic.Recordings[0];
                //System.Numerics.Complex[] source_response = SD.Frequency_Response(result_signals[0].Length);

                //double f_limit = 0.8 * fs / samplefrequency * result_signals[0].Length;
                //double f_top = 1.3 * f_limit;
                //double dpi = Utilities.Numerics.PiX2 / (f_top - f_limit);

                //for (int c = 0; c < result_signals.Length; c++)
                //{
                //    System.Numerics.Complex[] result_response = Audio.Pach_SP.FFT_General(result_signals[c],0);
                //    Array.Resize(ref result_response, result_response.Length / 2);
                //    for (int s = 0; s < result_response.Length; s++)
                //    {
                //        System.Numerics.Complex mod = source_response[s].Magnitude;
                //        if (s > f_limit) mod /= System.Numerics.Complex.Pow(source_response[s], (.5 * Math.Tanh((s-f_limit) * dpi) + 0.5));
                //        result_response[s] /= mod.Magnitude;
                //    }
                //    result_signals[c] = Audio.Pach_SP.IFFT_Real_General(Audio.Pach_SP.Mirror_Spectrum(result_response), 0);
                //}
                //Find Eigenfrequencies
                if (EigenFrequencies.Items.Count > 0) return;
                EigenFrequencies.Items.Clear();
                Find_EigenFrequencies();

                Receiver_Choice.Items.Clear();
                for (int i = 0; i < result_signals.Length; i++) Receiver_Choice.Items.Add(i);
                Time = new double[result_signals[0].Length];
                for (int i = 0; i < Time.Length; i++) Time[i] = (double)i / samplefrequency;
                Receiver_Choice.SelectedIndex = 0;
                Receiver_Choice.Update();

            }

            private void Find_EigenFrequencies()
            {
                for (int c = 0; c < result_signals.Length; c++)
                {
                    System.Numerics.Complex[] fdom = Audio.Pach_SP.FFT_General(result_signals[c], 0);
                    double[] mag = new double[fdom.Length];
                    double[] real = new double[fdom.Length];
                    double[] imag = new double[fdom.Length];
                    double[] freq = new double[fdom.Length];
                    for (int i = 0; i < fdom.Length / 2; i++)
                    {
                        mag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Magnitude);
                        real[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Real);
                        imag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Imaginary);
                        freq[i] = ((double)i / fdom.Length * samplefrequency);
                    }

                    Populate_EigenFrequencies(mag, freq, "Magnitude");
                    Populate_EigenFrequencies(real, freq, "Real-Part");
                    Populate_EigenFrequencies(imag, freq, "Imaginary-Part");
                }

                object[] l = new object[EigenFrequencies.Items.Count];
                EigenFrequencies.Items.CopyTo(l, 0);
                l.Distinct();
                List<object> lalpha = l.ToList();
                lalpha.Sort();
                EigenFrequencies.Items.Clear();
                EigenFrequencies.Items.AddRange(lalpha.ToArray());
            }

            double[] Time;
            double[][] result_signals;

            private void Update_Graph(object sender, EventArgs e)
            {
                double max = 0;
                double[] SPL = Utilities.AcousticalMath.SPL_Pressure_Signal(result_signals[Receiver_Choice.SelectedIndex]);
                for (int i = 0; i < SPL.Length; i++) max = Math.Max(max, SPL[i]);

                TransientView.GraphPane.CurveList.Clear();
                TransientView.GraphPane.Title.Text = "Time Domain Response";
                TransientView.GraphPane.XAxis.Title.Text = "Time (seconds)";
                TransientView.GraphPane.YAxis.Title.Text = "Sound Pressure (Pa)";
                TransientView.GraphPane.AddCurve("Time Domain Response", Time, Utilities.AcousticalMath.SPL_Pressure_Signal(result_signals[Receiver_Choice.SelectedIndex]), System.Drawing.Color.Blue, ZedGraph.SymbolType.None);
                TransientView.GraphPane.XAxis.Scale.Max = Time[Time.Length - 1];
                TransientView.GraphPane.XAxis.Scale.Min = Time[0];

                TransientView.GraphPane.YAxis.Scale.Max = max * 1.2;
                TransientView.GraphPane.YAxis.Scale.Min = 0;// -max;

                TransientView.AxisChange();
                TransientView.Invalidate();

                System.Numerics.Complex[] fdom = Audio.Pach_SP.FFT_General(result_signals[Receiver_Choice.SelectedIndex], 0);
                double[] mag = new double[fdom.Length];
                double[] real = new double[fdom.Length];
                double[] imag = new double[fdom.Length];
                double[] freq = new double[fdom.Length];
                for (int i = 0; i < fdom.Length / 2; i++)
                {
                    mag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Magnitude);
                    real[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Real);
                    imag[i] = Utilities.AcousticalMath.SPL_Pressure(fdom[i].Imaginary);
                    freq[i] = ((double)i / fdom.Length * samplefrequency);
                }

                for (int i = 0; i < mag.Length; i++) max = Math.Max(max, mag[i]);

                Frequency_View.GraphPane.CurveList.Clear();
                Frequency_View.GraphPane.Title.Text = "Frequency Domain Response";
                Frequency_View.GraphPane.XAxis.Type = ZedGraph.AxisType.Log;
                Frequency_View.GraphPane.XAxis.Title.Text = "Time (seconds)";
                Frequency_View.GraphPane.YAxis.Title.Text = "Sound Pressure (Pa)";
                Frequency_View.GraphPane.AddCurve("Magnitude", freq, mag, System.Drawing.Color.Red, ZedGraph.SymbolType.None);
                Frequency_View.GraphPane.AddCurve("Real-Part", freq, real, System.Drawing.Color.Blue, ZedGraph.SymbolType.None);
                Frequency_View.GraphPane.AddCurve("Imaginary-Part", freq, imag, System.Drawing.Color.Gray, ZedGraph.SymbolType.None);
                Frequency_View.GraphPane.XAxis.Scale.Max = freq[freq.Length - 1];
                Frequency_View.GraphPane.XAxis.Scale.Min = 0;

                Frequency_View.GraphPane.YAxis.Scale.Max = max * 1.2;
                Frequency_View.GraphPane.YAxis.Scale.Min = 0;

                Frequency_View.AxisChange();
                Frequency_View.Invalidate();

                Frequency_View.GraphPane.AddBar("Chosen_EigenFrequency", new double[1] { Chosenfreq }, new double[1] { 10000 }, System.Drawing.Color.Black);
            }

            public void Populate_EigenFrequencies(double[] mag, double[] freq, string functiontype)
            {
                MathNet.Numerics.Interpolation.CubicSpline CS = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(freq, mag);

                for (double f = freq[0]; f < freq[freq.Length / 10]; f += 10)
                {
                    double f_ = f + 10;
                    double v = CS.Differentiate(f);
                    double vf = CS.Differentiate(f_);

                    if (v > 0 && vf < 0)
                    {
                        //Look closer - find the exact frequency.
                        for (double f1 = f; f1 < f_; f1 += 1)
                        {
                            double v1 = CS.Differentiate(f1);
                            double v1f = CS.Differentiate(f1 + 1);
                            if (v1 > 0 && v1f < 0)
                            {
                                double eigen = v1 < -v1f ? Math.Ceiling(f1) : Math.Floor(f1 + 1);
                                string s = "";
                                if (eigen < 100) s = "00";
                                else if (eigen < 1000) s = "0";
                                s = s + string.Format("{0} hz. {1}", eigen, functiontype);
                                EigenFrequencies.Items.Add(s);
                            }
                        }
                    }
                }
            }

            private void Receiver_Choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graph(sender, e);
            }

            private void Export_Signal_Click(object sender, EventArgs e)
            {
                SaveFileDialog SaveWave = new SaveFileDialog();

                SaveWave.Filter = " Wave Audio (*.wav) |*.wav";

                if (SaveWave.ShowDialog() == DialogResult.OK)
                {
                    NAudio.Wave.WaveFileWriter Writer = new NAudio.Wave.WaveFileWriter(SaveWave.FileName, new NAudio.Wave.WaveFormat(44100, 24, result_signals.Length));
                    for (int j = 0; j < result_signals[0].Length; j++)
                    {
                        for (int c = 0; c < result_signals.Length; c++) if (j > result_signals[0].Length - 1) Writer.WriteSample(0); else Writer.WriteSample((float)result_signals[c][j]);
                    }
                    Writer.Close();
                    Writer.Dispose();
                }
            }
            #endregion
            double Chosenfreq;

            private void EigenFrequencies_SelectedIndexChanged(object sender, EventArgs e)
            {
                Chosenfreq = double.Parse((EigenFrequencies.Items[EigenFrequencies.SelectedIndex] as string).Split(" "[0])[0]);
                Update_Graph(sender, e);
                Frequency_Selection.Value = (decimal)Chosenfreq;
                Freq_Max.Value = (decimal)(62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Selected_Extent.SelectedIndex));
            }
        }
    }
}