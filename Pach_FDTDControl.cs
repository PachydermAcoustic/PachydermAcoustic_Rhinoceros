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
                Analysis_Technique.SelectedIndex = 0;
                Eigen_Extent.SelectedIndex = 4;
                scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);
                Param_Scale.Image = scale.PIC;
                scatterscale = new Pach_Graphics.HSV_colorscale(Scatter_Scale.Height, Scatter_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);
                Scatter_Scale.Image = scatterscale.PIC;
            }

            #region Visualization

            private Pach_Graphics.colorscale scale;
            private Pach_Graphics.colorscale scatterscale;
            public delegate void Populator(double Dist);
            Numeric.TimeDomain.Acoustic_Compact_FDTD_RC FDTD;
            WaveConduit P;
            SphereConduit SP;
            Rhino.Geometry.Mesh[][] M;
            //List<List<Rhino.Geometry.Point3d>> Pts;
            List<List<double>> Pressure;
            CellConduit c = new CellConduit();

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                FC = new ForCall(Forw_proc);

                Polygon_Scene Rm = RC_PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, false, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                if (!Rm.Complete) return;

                if (P == null) P = new WaveConduit(scale, new double[2] { (double)this.Param_Min.Value, (double)this.Param_Max.Value });
                Hare.Geometry.Point[] Src = RC_PachTools.GetSource();
                Hare.Geometry.Point[] Rec = new Hare.Geometry.Point[0];//PachTools.GetReceivers().ToArray();
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

                Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(s_type, (double)Frequency_Selection.Value, 1, RC_PachTools.GetSource());
                Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact(Rec);

                FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD_RC(Rm, ref SD, ref Mic, (double)Freq_Max.Value, (double)CO_TIME.Value, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.Freefield, null, 0, 0, 0);
                //FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, (double)Freq_Max.Value, (double)CO_TIME.Value, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.ScatteringLab, new Hare.Geometry.Point(0,0,0), 8, 6, 5);
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

                List<List<Hare.Geometry.Point>> hpts = new List<List<Hare.Geometry.Point>>();

                FDTD.Pressure_Points(ref hpts, ref Pressure, X.ToArray(), Y.ToArray(), Z.ToArray(), 0.00002 * Math.Pow(10, (double)Param_Min.Value / 20), false, false, true, Magnitude.Checked);
                List<List<Rhino.Geometry.Point3d>> Pts = new List<List<Rhino.Geometry.Point3d>>();

                for (int i = 0; i < hpts.Count; i++)
                {
                    Pts.Add(new List<Rhino.Geometry.Point3d>());
                    for (int j = 0; j < hpts[i].Count; j++)
                    {
                        Pts[i].Add(RC_PachTools.HPttoRPt(hpts[i][j]));
                    }
                }

                P.Populate(X.ToArray(), Y.ToArray(), Z.ToArray(), FDTD.dx, Pressure, M, Magnitude.Checked);

                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Forw_proc()
            {
                double t = FDTD.Increment();

                Rhino.RhinoApp.CommandPrompt = string.Format("Running {0} Hz., {1} ms.", FDTD.SD.frequency, Math.Round(t * 1000));

                Show_Field();

                this.Invoke((MethodInvoker)delegate
                {
                    Time_Preview.Text = Math.Round(t, 3).ToString();
                });
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

                ////////////////////////
                if (Folder_Status.Text != "")
                {
                    int j = (int)Math.Round(t / FDTD.dt);
                    string number;
                    if (j < 100)
                    {
                        if (t < 10) number = "00" + j.ToString();
                        else number = "0" + j.ToString();
                    }
                    else number = j.ToString();

                    this.Invoke((MethodInvoker)delegate { Rhino.RhinoApp.RunScript("-ViewCaptureToFile " + Folder_Status.Text + "\\"[0] + "frame" + number + ".jpg Width=1280 Height=720 DrawGrid=No Enter", true); });
                }
                /////////////////////////
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

                Polygon_Scene Rm = RC_PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, false, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                if (!Rm.Complete) return;

                Hare.Geometry.Point[] Src = RC_PachTools.GetSource();
                List<Hare.Geometry.Point> Rec = RC_PachTools.GetReceivers();
                if (Src.Length < 1 || Rm == null) Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");

                Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, 1000, 1, RC_PachTools.GetSource());
                Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact(Rec.ToArray());
                double fs = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Eigen_Extent.SelectedIndex);
                FDTD = new Numeric.TimeDomain.Acoustic_Compact_FDTD_RC(Rm, ref SD, ref Mic, fs, (double)CO_TIME.Value, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.Freefield, null, 0, 0, 0);
                FDTD.RuntoCompletion();

                samplefrequency = FDTD.SampleFrequency;

                Mic.reset();
                result_signals = Mic.Recordings()[0];
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
                Freq_Max.Value = (decimal)(62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Eigen_Extent.SelectedIndex));
            }

            private FolderBrowserDialog FileLocation = new FolderBrowserDialog();
            private void SetFolder_Click(object sender, EventArgs e)
            {
                if (FileLocation.ShowDialog() == DialogResult.OK)
                {
                    Folder_Status.Text = FileLocation.SelectedPath;
                }
            }

            double[] Scattering;
            Numeric.TimeDomain.Microphone_Compact MicS;
            //List<System.Numerics.Complex[]> FFTs;
            double d_f = 2;
            int omit;
            //System.Numerics.Complex[][] FS2;
            //System.Numerics.Complex[][] FF2;
            //System.Numerics.Complex[][] FSFF;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FFTs;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FS2;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FF2;
            System.IO.MemoryMappedFiles.MemoryMappedFile[] FSFF;

            private void CalculateScattering_Click(object sender, EventArgs e)
            {
                Chosenfreq = 0;
                double radius = (double)ScatteringRadius.Value;
                double t = 5 * (radius + (double)Sample_Depth.Value) / C_Sound() * 1000;
                LabCenter = new Rhino.Geometry.Point3d(0,0, (double)Sample_Depth.Value);

                if (FS2 != null)
                {
                    for (int i = 0; i < FS2.Length; i++)
                    {
                        FS2[i].Dispose();
                        FF2[i].Dispose();
                        FSFF[i].Dispose();
                        if( i < FFTs.Length) FFTs[i].Dispose();
                    }
                }
                if (Analysis_Technique.SelectedIndex == 0)
                {
                    List<double> dir = new List<double>();
                    if (Scat_Dir_00.Checked) dir.Add(0);
                    if (Scat_Dir_15.Checked) dir.Add(15 * Math.PI / 180);
                    if (Scat_Dir_30.Checked) dir.Add(30 * Math.PI / 180);
                    if (Scat_Dir_45.Checked) dir.Add(45 * Math.PI / 180);
                    if (Scat_Dir_60.Checked) dir.Add(60 * Math.PI / 180);
                    if (Scat_Dir_75.Checked) dir.Add(75 * Math.PI / 180);

                    Hare.Geometry.Point[] Src = new Hare.Geometry.Point[dir.Count];
                    for(int i = 0; i < dir.Count; i++) Src[i] = new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z) + new Hare.Geometry.Vector(Math.Sin(dir[i]), 0, Math.Cos(dir[i])) * (radius) + new Hare.Geometry.Vector(0,0,(double)Sample_Depth.Value) ;
                    List<Hare.Geometry.Point> Rec = new List<Hare.Geometry.Point>();

                    double fs = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, comboBox2.SelectedIndex);

                    t += 60 / fs;

                    Polygon_Scene Rm = RC_PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, false, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                    Empty_Scene Rm_Ctrl = new Empty_Scene((double)Air_Temp.Value, (double)Rel_Humidity.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked, true, Rm.Min(), Rm.Max());
                    Rm_Ctrl.PointsInScene(new List<Hare.Geometry.Point> { Rm.Min(), Rm.Max() });
                    Rm_Ctrl.partition();

                    if (!Rm.Complete && Rm_Ctrl.Complete) return;

                    Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact();
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDS = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.ScatteringLab, Utilities.RC_PachTools.RPttoHPt(LabCenter), radius * 2.4, radius * 2.4, radius * 1.2 + (double)Sample_Depth.Value);
                    long size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of first model, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");
                    FDTDS.RuntoCompletion();                    
                    Mic.reset();
                    double[][] TimeS = Mic.Recordings()[0];
                    samplefrequency = FDTDS.SampleFrequency;
                    double dx = FDTDS.dx, dy = FDTDS.dy, dz = FDTDS.dz;

                    Numeric.TimeDomain.Signal_Driver_Compact SDf = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Micf = new Numeric.TimeDomain.Microphone_Compact();
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDF = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm_Ctrl, ref SDf, ref Micf, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.ScatteringLab, Utilities.RC_PachTools.RPttoHPt(LabCenter), radius * 2.4, radius * 2.4, radius * 1.2 + (double)Sample_Depth.Value);
                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of second model, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");
                    FDTDF.RuntoCompletion();
                    Micf.reset();
                    result_signals = Micf.Recordings()[0];

                    omit = SD.Z[0] + 60;

                    //Calculate Scattering Coefficients
                    
                    //Zero packing
                    for (int i = 0; i < TimeS.Length; i++)
                    {
                        Array.Resize(ref TimeS[i], (int)(samplefrequency / 2));
                        Array.Resize(ref result_signals[i], (int)(samplefrequency / 2));
                    }

                    Freq_Trackbar1.Maximum = (int)(samplefrequency / 2);
                    Freq_Trackbar2.Maximum = (int)(samplefrequency / 2);

                    System.Numerics.Complex[][] FS = new System.Numerics.Complex[TimeS.Length][];
                    System.Numerics.Complex[][] FF = new System.Numerics.Complex[TimeS.Length][];

                    for (int i = 0; i < TimeS.Length; i++)
                    {
                        FS[i] = Audio.Pach_SP.FFT_General(TimeS[i], 0);
                        FF[i] = Audio.Pach_SP.FFT_General(result_signals[i], 0);
                    }

                    Scattering = new double[FS[0].Length];
                    //FF2 = new System.Numerics.Complex[FS[0].Length][];
                    //FS2 = new System.Numerics.Complex[FS[0].Length][];
                    //FSFF = new System.Numerics.Complex[FS[0].Length][];
                    FF2 = new System.IO.MemoryMappedFiles.MemoryMappedFile[FS[0].Length];
                    FS2 = new System.IO.MemoryMappedFiles.MemoryMappedFile[FS[0].Length];
                    FSFF = new System.IO.MemoryMappedFiles.MemoryMappedFile[FS[0].Length];

                    for (int i = 0; i < FS[0].Length; i++)
                    {
                        FF2[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FF2"+i.ToString(), FS.Length * 16);
                        FS2[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FS2"+i.ToString(), FS.Length * 16);
                        FSFF[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FSFF"+i.ToString(), FS.Length * 16);
                        System.IO.BinaryWriter fs2writer = new System.IO.BinaryWriter(FS2[i].CreateViewStream());
                        System.IO.BinaryWriter ff2writer = new System.IO.BinaryWriter(FF2[i].CreateViewStream());
                        System.IO.BinaryWriter fsffwriter = new System.IO.BinaryWriter(FSFF[i].CreateViewStream());
                        //FF2[i] = new System.Numerics.Complex[FS.Length];
                        //FS2[i] = new System.Numerics.Complex[FS.Length];
                        //FSFF[i] = new System.Numerics.Complex[FS.Length];
                        System.Numerics.Complex sumFS2 = 0;
                        System.Numerics.Complex sumFF2 = 0;
                        System.Numerics.Complex sumFSFF = 0;

                        for (int j = 0; j < FS.Length; j++)
                        {
                            System.Numerics.Complex fs2 = System.Numerics.Complex.Pow(FS[j][i].Magnitude, 2);
                            System.Numerics.Complex ff2 = System.Numerics.Complex.Pow(FF[j][i].Magnitude, 2);
                            System.Numerics.Complex fsff = FS[j][i] * System.Numerics.Complex.Conjugate(FF[j][i]);

                            fs2writer.Write(fs2.Real);
                            fs2writer.Write(fs2.Imaginary);
                            ff2writer.Write(ff2.Real);
                            ff2writer.Write(ff2.Imaginary);
                            fsffwriter.Write(fsff.Real);
                            fsffwriter.Write(fsff.Imaginary);

                            sumFS2 += fs2;
                            sumFF2 += ff2;
                            sumFSFF += fsff;
                            //FS2[i][j] = System.Numerics.Complex.Pow(FS[j][i].Magnitude, 2);
                            //FF2[i][j] = System.Numerics.Complex.Pow(FF[j][i].Magnitude, 2);
                            //FSFF[i][j] = FS[j][i] * System.Numerics.Complex.Conjugate(FF[j][i]);
                            //sumFS2 += FS2[i][j];
                            //sumFF2 += FF2[i][j];
                            //sumFSFF += FSFF[i][j];
                        }
                        fs2writer.Close();
                        ff2writer.Close();
                        fsffwriter.Close();
                        fs2writer.Dispose();
                        ff2writer.Dispose();
                        fsffwriter.Dispose();

                        System.Numerics.Complex sumReflected = sumFSFF / sumFF2;
                        System.Numerics.Complex Ratio = sumFF2 / sumFS2;
                        Scattering[i] = 1 - System.Numerics.Complex.Abs(sumReflected * sumReflected * Ratio);
                    }

                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of coefficient calcs, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");

                    ///Add Balloon plot aparatus
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();

                    FFTs = new System.IO.MemoryMappedFiles.MemoryMappedFile[Mic.X.Length];
                    //System.Numerics.Complex.Pow(FS[j][i].Magnitude, 2);
                    //fftwriter.Write(fs2.Real);

                    for (int i = 0; i < Mic.X.Length; i++)
                    {
                        double[] Reflection = Mic.Recordings(i, omit);
                        if (Reflection.Length < 8192) Array.Resize(ref Reflection, omit + Reflection.Length);

                        System.Numerics.Complex[] fft = Audio.Pach_SP.FFT_General(Reflection, 0);
                        FFTs[i] = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("FFT" + i.ToString(), fft.Length * 16);
                        System.IO.BinaryWriter FFTwriter = new System.IO.BinaryWriter(FFTs[i].CreateViewStream());
                        d_f = samplefrequency / fft.Length;

                        for (int j = 0; j < fft.Length; j++)
                        {
                            FFTwriter.Write(fft[j].Real);
                            FFTwriter.Write(fft[j].Imaginary);
                        }
                        pts.Add(FDTDF.RDD_Location(Mic.X[i] , Mic.Y[i], Mic.Z[i]) - Utilities.RC_PachTools.RPttoHPt(LabCenter));
                        FFTwriter.Close(); FFTwriter.Dispose();
                    }

                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of balloon points, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");

                    Sphere_Plot SPS = new Sphere_Plot(pts, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), Math.Sqrt(dx * dx + dy * dy + dz * dz));
                    if (SP == null) SP = new SphereConduit(SPS, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), scatterscale, new double[2] { (double)this.ScatMin.Value, (double)this.ScatMax.Value });
                    else SP.plot = SPS;

                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of Delaunay balloon construction, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");

                    MicS = Mic;
                    size = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                    Rhino.RhinoApp.WriteLine("At end of simulation, using " + (double)size / (1024 * 1024 * 1024) + " gigabytes...");
                }
                else if (Analysis_Technique.SelectedIndex == 1)
                {
                    Polygon_Scene Rm = RC_PachTools.Get_Poly_Scene((double)Rel_Humidity.Value, false, (double)Air_Temp.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked);
                    Empty_Scene Rm_Ctrl = new Empty_Scene((double)Air_Temp.Value, (double)Rel_Humidity.Value, (double)Air_Pressure.Value, Atten_Method.SelectedIndex, EdgeFreq.Checked, true, Rm.Min(), Rm.Max());
                    Rm_Ctrl.PointsInScene(new List<Hare.Geometry.Point> { Rm.Min(), Rm.Max() });
                    Rm_Ctrl.partition();

                    if (!Rm.Complete && Rm_Ctrl.Complete) return;

                    Hare.Geometry.Point ArrayCenter = new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z + (double)Sample_Depth.Value);

                    Hare.Geometry.Point[] Src = new Hare.Geometry.Point[1] { new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z + radius + (double)Sample_Depth.Value) };
                    //List<Hare.Geometry.Point> Rec = new List<Hare.Geometry.Point>();

                    //for (int phi = 0; phi < 18; phi++) for (int theta = 0; theta < 36; theta++)
                    //    {
                    //        double anglePhi = phi * Math.PI / 18;
                    //        double angleTheta = theta * Utilities.Numerics.PiX2 / 36;
                    //        Rec.Add(ArrayCenter + radius * new Hare.Geometry.Point(Math.Cos(angleTheta) * Math.Cos(anglePhi), Math.Sin(angleTheta) * Math.Cos(anglePhi), Math.Sin(anglePhi)));
                    //    }

                    double fs = 62.5 * Utilities.Numerics.rt2 * Math.Pow(2, Analysis_Technique.SelectedIndex);

                    Numeric.TimeDomain.Signal_Driver_Compact SD = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Mic = new Numeric.TimeDomain.Microphone_Compact();// Rec.ToArray());
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDS = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm, ref SD, ref Mic, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.TransparencyLab, Utilities.RC_PachTools.RPttoHPt(LabCenter), radius * 4, radius * 4, radius * 1.2 + (double)Sample_Depth.Value);
                    FDTDS.RuntoCompletion();

                    Numeric.TimeDomain.Signal_Driver_Compact SDf = new Numeric.TimeDomain.Signal_Driver_Compact(Numeric.TimeDomain.Signal_Driver_Compact.Signal_Type.Sine_Pulse, fs, 1, Src);
                    Numeric.TimeDomain.Microphone_Compact Micf = new Numeric.TimeDomain.Microphone_Compact();// Rec.ToArray());
                    Numeric.TimeDomain.Acoustic_Compact_FDTD FDTDF = new Numeric.TimeDomain.Acoustic_Compact_FDTD(Rm_Ctrl, ref SDf, ref Micf, fs, t, Numeric.TimeDomain.Acoustic_Compact_FDTD.GridType.ScatteringLab, Utilities.RC_PachTools.RPttoHPt(LabCenter), radius * 4, radius * 4, radius * 1.2 + (double)Sample_Depth.Value);
                    FDTDF.RuntoCompletion();

                    int start = (int)Math.Round((2.25 * radius / Rm.Sound_speed(Src[0])) / FDTDS.dt);

                    samplefrequency = FDTDS.SampleFrequency;

                    Mic.reset();
                    result_signals = Mic.Recordings()[0];

                    //Calculate Scattering Coefficients
                    double[][] TimeS = Mic.Recordings()[0];
                    double[][] TimeF = Micf.Recordings()[0];

                    System.Numerics.Complex[][] FS = new System.Numerics.Complex[TimeS.Length][];
                    System.Numerics.Complex[][] FF = new System.Numerics.Complex[TimeS.Length][];

                    for (int i = 0; i < TimeS.Length; i++)
                    {
                        double[] ts = new double[TimeS[i].Length - start];
                        double[] tf = new double[TimeS[i].Length - start];
                        for (int ti = start; ti < TimeS[i].Length; ti++)
                        {
                            ts[ti - start] = TimeS[i][ti];
                            tf[ti - start] = TimeF[i][ti];
                        }
                        FS[i] = Audio.Pach_SP.FFT_General(ts, 0);
                        FF[i] = Audio.Pach_SP.FFT_General(TimeF[i], 0);
                    }

                    Scattering = new double[FS[0].Length];

                    for (int i = 0; i < FS[0].Length; i++)
                    {
                        System.Numerics.Complex sumFF2 = 0;

                        for (int j = 0; j < FS.Length; j++)
                        {
                            sumFF2 += System.Numerics.Complex.Pow(FS[j][i], 2) / System.Numerics.Complex.Pow(FF[j][i], 2);
                        }

                        Scattering[i] = Utilities.AcousticalMath.SPL_Pressure(System.Numerics.Complex.Abs(sumFF2));
                    }
                    ///Add Balloon plot aparatus
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();
                    for (int i = 0; i < Mic.X.Length; i++)
                    {
                        pts.Add(FDTDS.RDD_Location(Mic.X[i], Mic.Y[i], Mic.Z[i]) - Utilities.RC_PachTools.RPttoHPt(LabCenter));
                    }

                    Sphere_Plot SPS = new Sphere_Plot(pts, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), 5 * Math.Sqrt(FDTDS.dx * FDTDS.dx + FDTDS.dy * FDTDS.dy + FDTDS.dz * FDTDS.dz));
                    if (SP == null) SP = new SphereConduit(SPS, new Hare.Geometry.Point(LabCenter.X, LabCenter.Y, LabCenter.Z), scatterscale, new double[2] { (double)this.ScatMin.Value, (double)this.ScatMax.Value });
                    MicS = Mic;
                }
                Update_Scattering_Graph(null, null);
            }

            Rhino.Geometry.Point3d LabCenter;

            private void Set_Origin_Click(object sender, EventArgs e)
            {
                Rhino.Geometry.Point3d pt;
                Rhino.Commands.Result rr = Rhino.Input.RhinoGet.GetPoint("Select the center point of the sample that you would like to analyze...", true, out pt);
                if (rr == Rhino.Commands.Result.Success) LabCenter = pt;
            }

            private void Update_Scattering_Graph(object sender, EventArgs e)
            {
                double max = Scattering.Max();

                ScatteringGraph.GraphPane.CurveList.Clear();
                ScatteringGraph.GraphPane.Title.Text = "Scattering Performance";
                ScatteringGraph.GraphPane.XAxis.Title.Text = "Frequency (Hz.)";
                ScatteringGraph.GraphPane.YAxis.Title.Text = "Scattering Coefficient";

                ScatteringGraph.GraphPane.XAxis.Scale.Max = samplefrequency / 2;
                ScatteringGraph.GraphPane.XAxis.Scale.Min = 0;
 
                ScatteringGraph.GraphPane.YAxis.Scale.Max = 1.0;
                if (max > 1) ScatteringGraph.GraphPane.YAxis.Scale.Max = max;

                ScatteringGraph.GraphPane.YAxis.Scale.Min = 0;

                ScatteringGraph.AxisChange();
                ScatteringGraph.Invalidate();

                double[] freq = new double[Scattering.Length];
                for (int i = 0; i < Scattering.Length; i++)
                {
                    freq[i] = ((double)i / Scattering.Length * samplefrequency);
                }

                ScatteringGraph.GraphPane.AddCurve("Scattering Function", freq, Scattering, System.Drawing.Color.Red, ZedGraph.SymbolType.None);

                System.Numerics.Complex[] SWC = new System.Numerics.Complex[FFTs.Length];
                double[] SW = new double[FFTs.Length];

                int[] F = new int[2] { Math.Min(Freq_Trackbar1.Value, Freq_Trackbar2.Value), Math.Max(Freq_Trackbar1.Value, Freq_Trackbar2.Value) };

                if (Scat_Param_Select.SelectedIndex == 0)
                {
                    for (int i = 0; i < FFTs.Length; i++)
                    {
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream fftreader = FFTs[i].CreateViewStream();
                        fftreader.Position = 0;
                        for (int f = F[0]; f < F[1]; f++)
                        {
                            byte[] R = new byte[8], I = new byte[8];
                            //fftreader.Position = f * 16;
                            fftreader.Read(R, 0, 8);
                            fftreader.Read(I, 0, 8);
                            System.Numerics.Complex X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            SWC[i] += X * X;// FFTs[i][f] * FFTs[i][f];
                        }
                        fftreader.Dispose();
                        SW[i] = Pachyderm_Acoustic.Utilities.AcousticalMath.SPL_Intensity(SWC[i].Magnitude);
                    }
                }
                else if (Scat_Param_Select.SelectedIndex == 1)
                {
                    System.Numerics.Complex[] sumFS2 = new System.Numerics.Complex[FFTs.Length], sumFF2 = new System.Numerics.Complex[FFTs.Length], sumFSFF = new System.Numerics.Complex[FFTs.Length];
                    for (int i = 0; i < FFTs.Length; i++)
                    {
                        sumFS2[i] = System.Numerics.Complex.Zero;
                        sumFF2[i] = System.Numerics.Complex.Zero;
                        sumFSFF[i] = System.Numerics.Complex.Zero;
                    }

                    for (int f = F[0]; f < F[1]; f++)
                    {
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream fs2reader = FS2[f].CreateViewStream();
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream ff2reader = FF2[f].CreateViewStream();
                        System.IO.MemoryMappedFiles.MemoryMappedViewStream fsffreader = FSFF[f].CreateViewStream();
                        for (int i = 0; i < FFTs.Length; i++)
                        {
                            byte[] R = new byte[8], I = new byte[8];
                            fs2reader.Read(R, 0, 8);
                            fs2reader.Read(I, 0, 8);
                            System.Numerics.Complex X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            sumFS2[i] += X;
                            ff2reader.Read(R, 0, 8);
                            ff2reader.Read(I, 0, 8);
                            X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            sumFF2[i] += X;
                            fsffreader.Read(R, 0, 8);
                            fsffreader.Read(I, 0, 8);
                            X = new System.Numerics.Complex(System.BitConverter.ToDouble(R, 0), System.BitConverter.ToDouble(I, 0));
                            sumFSFF[i] += X;
                        }

                        fs2reader.Dispose();
                        ff2reader.Dispose();
                        fsffreader.Dispose();
                    }

                    for (int i = 0; i < FFTs.Length; i++)
                    {
                        System.Numerics.Complex sumReflected = sumFSFF[i] / sumFF2[i];
                        System.Numerics.Complex Ratio = sumFF2[i] / sumFS2[i];
                        SW[i] = 100 * (1 - System.Numerics.Complex.Abs(sumReflected * sumReflected * Ratio));
                    }
                }
                SP.Data_in(SW.ToArray(), new double[2] { (double)ScatMin.Value, (double)ScatMax.Value }, (double)ScatteringRadius.Value);
            }

            private void Update_LabGuides()
            {
                c.labguide = true;
                c.hemianechoic = (Analysis_Technique.SelectedIndex == 0);
                c.radius = (double)this.ScatteringRadius.Value;
                c.depth = (double)this.Sample_Depth.Value;
            }

            private void ScatteringLab_Focus(object sender, EventArgs e)
            {
                c.Enabled = true;
                Update_LabGuides();
            }

            private void ScatteringLab_FocusLost(object sender, EventArgs e)
            {
                c.Enabled = false;
                c.labguide = false;
            }

            private void LabGuideParametersChanged(object sender, EventArgs e)
            {
                Update_LabGuides();
            }

            private void Scat_ValueChanged(object sender, EventArgs e)
            {
                Scat_Mid.Text = ((ScatMax.Value - ScatMin.Value) / 2 + ScatMin.Value).ToString();
                Update_Scattering_Graph(null, null);
            }

            private void Scat_Color_SelectedIndexChanged(object sender, EventArgs e)
            {
                Pach_Graphics.colorscale scale;
                switch (this.Color_Selection.Text)
                {
                    case "R-O-Y-G-B-I-V":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);
                        Scatter_Scale.Image = scale.PIC;
                        break;
                    case "Y-G-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, Math.PI / 3.0, 2.0 / 3.0, 1, 0, 1, 0, false, 12);
                        Scatter_Scale.Image = scale.PIC;
                        break;
                    case "R-O-Y":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 1.0 / 3.0, 1, 0, 1, 0, false, 12);
                        Scatter_Scale.Image = scale.PIC;
                        break;
                    case "W-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 0, 0, 1, -1, false, 12);
                        Scatter_Scale.Image = scale.PIC;
                        break;
                    case "R-M-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 1, 0, 1, -1, false, 12);
                        Scatter_Scale.Image = scale.PIC;
                        break;
                    default:
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, Math.PI / 2.0, 0, 0, 1, 1, false, 12);
                        Scatter_Scale.Image = scale.PIC;
                        break;
                }
                if (SP != null)
                {
                    SP.SetColorScale(scale);
                }

                Update_Scattering_Graph(null, null);
            }

            private void Scat_Octave_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Scattering_Graph(null, null);
            }

            private void Freq_Trackbar_Scroll(object sender, EventArgs e)
            {
                Freq_Feedback.Text = "Frequency Selection: " + Math.Round(d_f * Math.Min(Freq_Trackbar1.Value, Freq_Trackbar2.Value)) + " to " + Math.Round(d_f * Math.Max(Freq_Trackbar1.Value, Freq_Trackbar2.Value)) + " Hz.";
                Update_Scattering_Graph(null, null);
            }
        }
    }
}