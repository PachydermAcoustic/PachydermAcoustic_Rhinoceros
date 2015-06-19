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

using Rhino.Geometry;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Audio;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("12db68c3-c995-43c6-860a-6bd106b94a4c")]
        public partial class Pach_Auralisation
        {
            AuralisationConduit A;
            // This call is required by the Windows Form Designer. 
            public Pach_Auralisation()
            {
                A = new AuralisationConduit();
                InitializeComponent();
                DistributionType.SelectedIndex = 1;
                Graph_Octave.SelectedIndex = 5;
                DistributionType_SelectedIndexChanged(this, new EventArgs());
                Instance = this;
            }

            ///<summary>Gets the only instance of the PachydermAcoustic plug-in.</summary>
            public static Pach_Auralisation Instance
            {
                get;
                private set;
            }

            #region Tab 1
            Direct_Sound[] Direct_Data;
            ImageSourceData[] IS_Data;
            Receiver_Bank[] Receiver;
            Mapping.PachMapReceiver[] Maps;
            Hare.Geometry.Point[] Srcs;
            Hare.Geometry.Point[] Recs;
            int SampleRate;
            double CutoffTime;

            private void Tab_Selecting(object sender, System.Windows.Forms.TabControlCancelEventArgs e)
            {
                if (Tabs.SelectedTab.Text == "Render Settings")
                {
                    Receiver_Choice.Items.Clear();
                    if (Receiver != null && Receiver.Length > 0)
                    { 
                        for (int i = 0; i < Recs.Length; i++)
                        {
                            Receiver_Choice.Items.Add(i.ToString());
                        }
                    }
                    Update_Graph(null, new System.EventArgs());
                }
                else if (Tabs.SelectedTab.Text == "Data Source")
                {
                    if (Hybrid_Select.Checked)
                    {
                        if (UI.Pach_Hybrid_Control.Instance.Auralisation_Ready())
                        {
                            UI.Pach_Hybrid_Control.Instance.GetSims(ref Srcs, ref Recs, ref Direct_Data, ref IS_Data, ref Receiver);
                            SampleRate = UI.Pach_Hybrid_Control.Instance.SampleRate;
                            CutoffTime = UI.Pach_Hybrid_Control.Instance.CutoffTime;
                        }
                    }
                    else if (Mapping_Select.Checked)
                    {
                        if (UI.Pach_Mapping_Control.Instance.Auralisation_Ready())
                            UI.Pach_Mapping_Control.Instance.GetSims(ref Maps);
                    }

                }
            }

            private void Draw_Feedback()
            {
                if (Hybrid_Select.Checked)
                {
                    if (Pach_Hybrid_Control.Instance != null && !Pach_Hybrid_Control.Instance.Auralisation_Ready() || Receiver_Choice.SelectedIndex < 0) return;
                    Point3d[] rec = new Point3d[Recs.Length];
                    for (int i = 0; i < Recs.Length; i++) rec[i] = Utilities.PachTools.HPttoRPt(Recs[i]);
                    AuralisationConduit.Instance.add_Receivers(rec);
                    List<Point3d> pts = new List<Point3d>();
                    foreach(Direct_Sound D in Direct_Data) pts.Add(D.Src_Origin);
                    AuralisationConduit.Instance.add_Sources(pts);
                    List<Deterministic_Reflection> Paths = new List<Deterministic_Reflection>();
                    List<Polyline> Lines = new List<Polyline>();
                    foreach (ImageSourceData I in IS_Data) if (I != null) Paths.AddRange(I.Paths[Receiver_Choice.SelectedIndex]);
                    foreach (Deterministic_Reflection p in Paths) foreach (Polyline P in p.PolyLine) Lines.Add(P);
                    AuralisationConduit.Instance.add_Reflections(Lines);
                    pts.Clear();
                    List<Vector3d> Dirs = new List<Vector3d>();
                    for (int i = 0; i < this.Channel_View.Items.Count; i++)
                    {
                        Hare.Geometry.Vector TempDir = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(((channel)Channel_View.Items[i]).V, 0, -(double)Alt_Choice.Value, true), -(double)Azi_Choice.Value, 0, true);//new Hare.Geometry.Vector(Speaker_Directions[i].X, Speaker_Directions[i].Y, Speaker_Directions[i].Z)
                        //Hare.Geometry.Vector TempDir = Utilities.PachTools.Rotate_Vector(((channel)Channel_View.Items[i]).V, -(double)Azi_Choice.Value, -(double)Alt_Choice.Value, true);
                        TempDir.Normalize();
                        pts.Add(Utilities.PachTools.HPttoRPt(Recs[0]) + (Utilities.PachTools.HPttoRPt(TempDir) * -.343 * Math.Max(5,((channel)Channel_View.Items[i]).delay)));
                        Dirs.Add(new Vector3d(TempDir.x, TempDir.y, TempDir.z));
                    }
                    AuralisationConduit.Instance.add_Speakers(pts, Dirs);
                    AuralisationConduit.Instance.set_direction(Utilities.PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), Utilities.PachTools.HPttoRPt(Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(double)Alt_Choice.Value, true), -(double)Azi_Choice.Value, 0, true)));
                    //AuralisationConduit.Instance.set_direction(Utilities.PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), Utilities.PachTools.HPttoRPt(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), -(double)Azi_Choice.Value, -(double)Alt_Choice.Value, true)));
                }
                else if (Mapping_Select.Checked)
                {
                    if (Pach_Mapping_Control.Instance != null && !Pach_Mapping_Control.Instance.Auralisation_Ready() && Receiver_Choice.SelectedIndex < 0) return;
                    if (Maps == null || Maps[0] == null) return;
                    AuralisationConduit.Instance.add_Receivers(Maps[0].Origins());
                    List<Point3d> pts = new List<Point3d>();
                    foreach (Mapping.PachMapReceiver m in Maps) pts.Add(m.Src);
                    AuralisationConduit.Instance.add_Sources(pts);
                }
                else
                {
                    AuralisationConduit.Instance.Enabled = false;
                }
                Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.Redraw();
            }

            private void Update_Graph(object sender, EventArgs e)
            {
                Analysis_View.GraphPane.CurveList.Clear();
                
                int REC_ID = 0;
                try
                {
                    REC_ID = int.Parse(Receiver_Choice.Text);

                    int OCT_ID = PachTools.OctaveStr2Int(Graph_Octave.Text);
                    Analysis_View.GraphPane.Title.Text = "Logarithmic Energy Time Curve";
                    Analysis_View.GraphPane.XAxis.Title.Text = "Time (seconds)";
                    Analysis_View.GraphPane.YAxis.Title.Text = "Sound Pressure Level (dB)";

                    List<int> SrcIDs = new List<int>();
                    foreach (int i in SourceList.CheckedIndices) SrcIDs.Add(i);

                    //List<double[]> Filter = new List<double[]>();
                    List<System.Drawing.Color> C = new List<System.Drawing.Color> {System.Drawing.Color.Red, System.Drawing.Color.OrangeRed, System.Drawing.Color.Orange, System.Drawing.Color.DarkGoldenrod, System.Drawing.Color.Olive, System.Drawing.Color.Green, System.Drawing.Color.Aquamarine, System.Drawing.Color.Azure, System.Drawing.Color.Blue, System.Drawing.Color.Indigo, System.Drawing.Color.Violet};

                    double[][] Temp;

                    switch (DistributionType.Text)
                    {
                        case "Monaural":
                            Response = new double[1][];
                            Response[0] = (AcousticalMath.PTCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false));
                            break;
                        case "First Order Ambisonics":
                            Response = new double[4][];
                            Temp = AcousticalMath.PTCurve_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true);
                            Response[0] = AcousticalMath.PTCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false);
                            Response[1] = Temp[0];
                            Response[2] = Temp[1];
                            Response[3] = Temp[2];
                            break;
                        case "Second Order Ambisonics":
                            Response = new double[9][];
                            Temp = AcousticalMath.PTCurve_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true);
                            Response[0] = AcousticalMath.PTCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false);
                            Response[1] = Temp[0];
                            Response[2] = Temp[1];
                            Response[3] = Temp[2];
                            Temp = AcousticalMath.PTCurve_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true);
                            Response[4] = Temp[0];
                            Response[5] = Temp[1];
                            Response[6] = Temp[2];
                            Response[7] = Temp[3];
                            Response[8] = Temp[4];
                            break;
                        case "Third Order Ambisonics":
                            Response = new double[16][];
                            Temp = AcousticalMath.PTCurve_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true);
                            Response[0] = AcousticalMath.PTCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, REC_ID, SrcIDs, false);
                            Response[1] = Temp[0];
                            Response[2] = Temp[1];
                            Response[3] = Temp[2];
                            Temp = AcousticalMath.PTCurve_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true);
                            Response[4] = Temp[0];
                            Response[5] = Temp[1];
                            Response[6] = Temp[2];
                            Response[7] = Temp[3];
                            Response[8] = Temp[4];
                            Temp = AcousticalMath.PTCurve_Ambisonics3(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true);
                            Response[9] = Temp[0];
                            Response[10] = Temp[1];
                            Response[11] = Temp[2];
                            Response[12] = Temp[3];
                            Response[13] = Temp[4];
                            Response[14] = Temp[5];
                            Response[15] = Temp[6];
                            break;
                        default:
                            Response = new double[Channel_View.Items.Count][];
                            for (int i = 0; i < Channel_View.Items.Count; i++) 
                            {
                                double alt = -(double)Alt_Choice.Value + 180 * Math.Asin((Channel_View.Items[i] as channel).V.z) / Math.PI;
                                double azi = (double)Azi_Choice.Value + 180 * Math.Atan2((Channel_View.Items[i] as channel).V.y, (Channel_View.Items[i] as channel).V.x) / Math.PI;
                                if (alt > 90) alt -= 180;
                                if (alt < -90) alt += 180;
                                if (azi > 360) azi -= 360;
                                if (azi < 0) azi += 360;
                                Response[i] = AcousticalMath.PTCurve_Directional(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, PachTools.OctaveStr2Int(Graph_Octave.Text), REC_ID, SrcIDs, false, alt, azi, true);
                            }
                            break;
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

                    double[] time = new double[Response[0].Length];
                    for (int i = 0; i < Response[0].Length; i++)
                    {
                        time[i] = (double)i / SampleRate - 1024f/SampleRate;
                    }

                    for (int i = 0; i < Response.Length; i++)
                    {
                        double[] filter = Audio.Pach_SP.FIR_Bandpass(Response[i], OCT_ID, SampleRate, 0);
                        Analysis_View.GraphPane.AddCurve(String.Format("Channel {0}", i), time, AcousticalMath.SPL_Pressure_Signal(filter), C[i % 10], ZedGraph.SymbolType.None);
                    }
                    Analysis_View.GraphPane.XAxis.Scale.Max = time[time.Length - 1];
                    Analysis_View.GraphPane.XAxis.Scale.Min = time[0];
                    Analysis_View.GraphPane.YAxis.Scale.Max = DirectMagnitude + 15;
                    Analysis_View.GraphPane.YAxis.Scale.Min = 0;

                    //AuralisationConduit.Instance.set_direction(Receiver[0].Origin(Receiver_Choice.SelectedIndex), new Rhino.Geometry.Vector3d(Math.Cos((float)Azi_Choice.Value * Math.PI / 180.0f) * Math.Cos((float)Alt_Choice.Value * Math.PI / 180.0f), Math.Sin((float)Azi_Choice.Value * Math.PI / 180.0f) * Math.Cos((float)Alt_Choice.Value * Math.PI / 180.0f), Math.Sin((float)Alt_Choice.Value * Math.PI / 180.0f)));
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                }
                catch { return; }
                Analysis_View.AxisChange();
                Analysis_View.Invalidate();
                Draw_Feedback();
            }

            public void Read_Array()
            {
                GetWave.Filter = " Array Description Text File (*.txt) |*.txt";
                if (GetWave.ShowDialog() == DialogResult.OK)
                {
                    Channel_View.Items.Clear();
                    System.IO.StreamReader Reader;
                    Reader = new System.IO.StreamReader(GetWave.OpenFile());
                    do
                    {
                        try
                        {
                            string speaker = Reader.ReadLine();
                            string[] s = speaker.Split(new char[] { ':' });
                            Channel_View.Items.Add(new channel(int.Parse(s[0]), new Hare.Geometry.Vector(double.Parse(s[1]),double.Parse(s[2]),double.Parse(s[3])), (channel.channel_type)int.Parse(s[4]), double.Parse(s[5])));
                        }
                        catch (System.Exception)
                        {
                            Reader.Close();
                            break;
                        }
                    } while (!Reader.EndOfStream);
                    Reader.Close();
                }
            }
            #endregion

            #region Tab 2
            //Signal Processing 
            private OpenFileDialog GetWave = new OpenFileDialog();

            private void OpenSignal_Click(object sender, System.EventArgs e)
            {
                GetWave.Filter = " Wave Audio (*.wav) |*.wav";
                if (GetWave.ShowDialog() == DialogResult.OK)
                {
                    Signal_Status.Text = GetWave.FileName;
                    RenderBtn.Enabled = true;
                    NAudio.Wave.WaveFileReader WR = new NAudio.Wave.WaveFileReader(Signal_Status.Text);
                    DryChannel.Minimum = 1;
                    DryChannel.Maximum = WR.WaveFormat.Channels;
                }
            }

            private List<int> SelectedSources()
            {
                List<int> indices = new List<int>();
                foreach (int i in SourceList.CheckedIndices) indices.Add(i);
                return indices;
            }

            private void OpenWaveFile(out int Sample_Freq, out double[] SignalETC)
            {
                NAudio.Wave.WaveFileReader WP = new NAudio.Wave.WaveFileReader(Signal_Status.Text);

                int BytesPerSample = WP.WaveFormat.Channels * WP.WaveFormat.BitsPerSample / 8;
                int BytesPerChannel = WP.WaveFormat.BitsPerSample / 8;
                byte[] signalbuffer = new byte[BytesPerSample];
                int ChannelCt = WP.WaveFormat.Channels;
                Sample_Freq = WP.WaveFormat.SampleRate;
                double[] SignalInt = new double[WP.SampleCount];

                if (WP.WaveFormat.BitsPerSample == 8)
                {
                    System.Windows.Forms.MessageBox.Show("Selected File is an 8-Bit audio file. This program requires a minimum bit-depth of 16.");
                    SignalETC = new double[SignalInt.Length];
                    return;
                }
                byte[] temp = new byte[4];
                int c = (int)DryChannel.Value-1;
                double Max;

                switch (WP.WaveFormat.BitsPerSample)
                {
                    case 32:
                        Max = Int32.MaxValue;
                        break;
                    case 24:
                        Max = BitConverter.ToInt32(new byte[] { 0, byte.MaxValue, byte.MaxValue, byte.MaxValue }, 0);
                        break;
                    case 16:
                        Max = Int16.MaxValue;
                        break;
                    case 8:
                        Max = BitConverter.ToInt16(new byte[] { 0, byte.MaxValue }, 0);
                        break;
                    default:
                        throw new Exception("Invalid bit depth variable... Where did you get this audio file again?");
                }

                for (int i = 0; i < WP.SampleCount; i++)
                {
                    WP.Read(signalbuffer, 0, BytesPerSample);

                    if (WP.WaveFormat.BitsPerSample == 32)
                    {
                        SignalInt[i] = BitConverter.ToInt32(signalbuffer, c * 4);
                    }
                    else if (WP.WaveFormat.BitsPerSample == 24)
                    {
                        temp[1] = signalbuffer[c * BytesPerChannel];
                        temp[2] = signalbuffer[c * BytesPerChannel + 1];
                        temp[3] = signalbuffer[c * BytesPerChannel + 2];
                        SignalInt[i] = BitConverter.ToInt32(temp, 0);
                    }
                    else if (WP.WaveFormat.BitsPerSample == 16)
                    {
                        temp[2] = signalbuffer[c * BytesPerChannel];
                        temp[3] = signalbuffer[c * BytesPerChannel + 1];
                        SignalInt[i] = BitConverter.ToInt32(temp, 0);
                    }
                }

                SignalETC = SignalInt;
            }

            int[] SrcRendered;
            int RecRendered;
            double[][] Response;
            int SFreq_Rendered;

            private void Clear_Render()
            {
                SrcRendered = null;
                RecRendered = -1;
                Response = null;
                SFreq_Rendered = -1;
            }

            private bool IsRendered()
            {
                if (Receiver_Choice.SelectedIndex != RecRendered) return false;
                if ((Response == null) || (Response[0] == null)) return false;
                if (SrcRendered == null || SrcRendered.Length != SourceList.CheckedIndices.Count) return false;
                for (int i = 0; i < SourceList.CheckedIndices.Count; i++)
                {
                    if (SrcRendered[i] != SourceList.CheckedIndices[i]) return false;
                }
                return true;
            }

            private void RenderBtn_Click(object sender, System.EventArgs e)
            {
                if (Response == null || Response.Length == 0)
                {
                    Rhino.RhinoApp.WriteLine("No impulse response found to render...");
                    return;
                }

                int SamplesPerSec;
                double[] SignalBuffer;
                this.OpenWaveFile(out SamplesPerSec, out SignalBuffer);

                float maxvalue = 0;
                //Normalize input signal...
                for (int j = 0; j < SignalBuffer.Length; j++) maxvalue = (float)Math.Max(maxvalue, Math.Abs(SignalBuffer[j]));
                for (int j = 0; j < SignalBuffer.Length; j++) SignalBuffer[j] /= maxvalue;
                //Convert pressure response to a 24-bit dynamic range:
                double mod24 = Math.Pow(10, -50 / 10);
                for (int i = 0; i < Response.Length; i++) for(int j = 0; j < Response[i].Length; j++) Response[i][j] *= mod24;

                float[][] NewSignal = new float[(int)Response.Length][];
                for (int i = 0; i < Response.Length; i++)
                {
                    NewSignal[i] = Pach_SP.FFT_Convolution(SignalBuffer, Response[i], 0);
                }

                SrcRendered = new int[SourceList.CheckedIndices.Count];
                for (int j = 0; j < SourceList.CheckedIndices.Count; j++)
                {
                    SrcRendered[j] = SourceList.CheckedIndices[j];
                }
                RecRendered = int.Parse(Receiver_Choice.Text);
                SFreq_Rendered = SamplesPerSec;

                SaveFileDialog SaveWave = new SaveFileDialog();
                if (DryChannel.Maximum < 4)
                {
                    SaveWave.Filter = " Wave Audio (*.wav) |*.wav";
                }
                else 
                {
                    SaveWave.Filter = "Extended Wave Audio (*.wavex) |*.wavex";
                }

                if (SaveWave.ShowDialog() == DialogResult.OK)
                {
                    if (Response == null || Response.Length == 0)
                    {
                        Rhino.RhinoApp.WriteLine("No impulse response found to render...");
                        return;
                    }
                    NAudio.Wave.WaveFileWriter Writer = new NAudio.Wave.WaveFileWriter(SaveWave.FileName, new NAudio.Wave.WaveFormat(SamplesPerSec, 24, Channel_View.Items.Count));

                    for (int j = 0; j < NewSignal[0].Length-1; j++)
                    {
                        for (int i = 0; i < Channel_View.Items.Count; i++) Writer.WriteSample(NewSignal[i][j]);
                    }
                    Writer.Close();
                    Writer.Dispose();
                    System.Media.SoundPlayer Player = new System.Media.SoundPlayer(SaveWave.FileName);
                    Player.Play();
                }
            }

            private void ExportFilter(object sender, EventArgs e)
            {
                SaveFileDialog SaveWave = new SaveFileDialog();
                if (DryChannel.Maximum < 4)
                {
                    SaveWave.Filter = " Wave Audio (*.wav) |*.wav";
                }
                else 
                {
                    SaveWave.Filter = "Extended Wave Audio (*.wavex) |*.wavex";
                }

                if (SaveWave.ShowDialog() == DialogResult.OK)
                {
                    NAudio.Wave.WaveFileWriter Writer = new NAudio.Wave.WaveFileWriter(SaveWave.FileName, new NAudio.Wave.WaveFormat(44100, 24, Response.Length));
                    for (int j = 0; j < Response[0].Length; j++)
                    {
                        for (int c = 0; c < Response.Length; c++) if (j > Response[c].Length - 1) Writer.WriteSample(0); else Writer.WriteSample((float)Response[c][j]);
                    }
                    Writer.Close();
                    Writer.Dispose();
                }
            }

            private void OpenDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Update_Graph(null, System.EventArgs.Empty);
            }

            private void Receiver_Choice_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graph(null, System.EventArgs.Empty);
                Source_Aim_SelectedIndexChanged(null, EventArgs.Empty);
            }

            #endregion
            private void SourceList_MouseUp(object sender, MouseEventArgs e)
            {
                Update_Graph(null, System.EventArgs.Empty);
            }

            private void Source_Aim_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Receiver_Choice.SelectedIndex < 0 || Source_Aim.SelectedIndex < 0) return;
                double azi, alt;

                PachTools.World_Angles(Direct_Data[Source_Aim.SelectedIndex].Src.Origin(), Utilities.PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), true, out alt, out azi);

                Alt_Choice.Value = (decimal)alt;
                Azi_Choice.Value = (decimal)azi;
            }

            private void DistributionType_SelectedIndexChanged(object sender, EventArgs e)
            {
                Channel_View.Items.Clear();                
                disable_CEdit();
                switch (DistributionType.Text)
                {
                    case "Monaural":
                        Channel_View.Items.Add(channel.Monaural(0));
                        break;
                    case "Stereo":
                        Channel_View.Items.Add(channel.Left(0));
                        Channel_View.Items.Add(channel.Right(1));
                        break;
                    case "Binaural (select file...)":
                        System.Windows.Forms.MessageBox.Show("Not yet implemented... If you would like to see this area completed, please help. Get in touch with me to suggest an HRTF standard, and possibly help implement. --Arthur");
                        DistributionType.Text = "Stereo";
                        break;
                    case "A-Format (type I-A)":
                        Channel_View.Items.Add(channel.FLU(0));
                        Channel_View.Items.Add(channel.FRD(1));
                        Channel_View.Items.Add(channel.BLD(2));
                        Channel_View.Items.Add(channel.BRU(3));
                        break;
                    case "A-Format (type II-A)":
                        Channel_View.Items.Add(channel.FLD(0));
                        Channel_View.Items.Add(channel.FRU(1));
                        Channel_View.Items.Add(channel.BLU(2));
                        Channel_View.Items.Add(channel.BRD(3));
                        break;
                    case "B-Format":
                        
                        break;
                    case "Surround Array (select file...)":
                        enable_CEdit();
                        Read_Array();
                        break;
                }
                Draw_Feedback();
            }

            private void enable_CEdit()
            {
                Add_Channel.Enabled = true;
                Add_Channel.Visible = true;
                Remove_Channel.Enabled = true;
                Remove_Channel.Visible = true;
                Move_Up.Enabled = true;
                Move_Up.Visible = true;
                Move_Down.Enabled = true;
                Move_Down.Visible = true;
                Save_Channels.Enabled = true;
                Save_Channels.Visible = true;
            }

            private void disable_CEdit()
            {
                Add_Channel.Enabled = false;
                Add_Channel.Visible = false;
                Remove_Channel.Enabled = false;
                Remove_Channel.Visible = false;
                Move_Up.Enabled = false;
                Move_Up.Visible = false;
                Move_Down.Enabled = false;
                Move_Down.Visible = false;
                Save_Channels.Enabled = false;
                Save_Channels.Visible = false;
            }

            private void Tab_Selecting(object sender, EventArgs e)
            {
                if (Tabs.SelectedTab.Text == "Render Settings")
                {
                    Receiver_Choice.Items.Clear();
                    if (Receiver != null && Receiver.Length > 0)
                    {
                        for (int i = 0; i < Recs.Length; i++)
                        {
                            Receiver_Choice.Items.Add(i.ToString());
                        }
                        Receiver_Choice.SelectedIndex = 0;
                        Update_Graph(this, new System.EventArgs());
                    }
                }
                else if (Tabs.SelectedTab.Text == "Data Source")
                {
                    SourceList.Items.Clear();
                    Source_Aim.Items.Clear();

                    if (Hybrid_Select.Checked)
                    {
                        if (UI.Pach_Hybrid_Control.Instance.Auralisation_Ready())
                        {
                            UI.Pach_Hybrid_Control.Instance.GetSims(ref Srcs, ref Recs, ref Direct_Data, ref IS_Data, ref Receiver);
                            CutoffTime = Direct_Data[0].Cutoff_Time;
                            SampleRate = (int)Direct_Data[0].SampleRate;
                        }
                        if (Direct_Data != null && Direct_Data.Length > 0)
                        {
                            for (int i = 0; i < Direct_Data.Length; i++)
                            {
                                SourceList.Items.Add(String.Format("S{0}-", i) + Direct_Data[i].Src.Type());
                                Source_Aim.Items.Add(String.Format("S{0}-", i) + Direct_Data[i].Src.Type());
                            }
                            SourceList.SetItemChecked(0, true);
                            Source_Aim.SelectedIndex = 0;
                        }
                    }
                    else if (Mapping_Select.Checked)
                    {
                        if (UI.Pach_Mapping_Control.Instance.Auralisation_Ready())
                        {
                            UI.Pach_Mapping_Control.Instance.GetSims(ref Maps);
                            CutoffTime = Maps[0].CutOffTime;
                            SampleRate = Maps[0].SampleRate;
                        }
                        if (Maps != null)
                        {
                            for (int i = 0; i < Maps.Length; i++)
                            {
                                SourceList.Items.Add(String.Format("S{0}-", i) + Maps[i].SrcType);
                                Source_Aim.Items.Add(String.Format("S{0}-", i) + Maps[i].SrcType);
                            }
                            SourceList.SetItemChecked(0, true);
                            Source_Aim.SelectedIndex = 0;
                        }
                    }
                }
                Draw_Feedback();
            }

            private void Move_Up_Click(object sender, EventArgs e)
            {
                int s = Channel_View.SelectedIndex;
                channel t = (channel)Channel_View.Items[s];
                Channel_View.Items.Insert(s-1,t);
                Channel_View.Items.RemoveAt(s + 1);
                Update_Channel_IDS();
                Clear_Render();
            }

            private void Move_Down_Click(object sender, EventArgs e)
            {
                int s = Channel_View.SelectedIndex;
                channel t = (channel)Channel_View.Items[s];
                Channel_View.Items.Insert(s + 1, t);
                Channel_View.Items.RemoveAt(s - 1);
                Update_Channel_IDS();
                Clear_Render();
            }

            private void Add_Channel_Click(object sender, EventArgs e)
            {
                double azi = 0;
                double alt = 0;
                double d = 0;
                Rhino.Input.RhinoGet.GetNumber("Enter the azimuth of the Speaker in degrees (X = front/back, Y = Left/Right)...",false, ref azi,-360, 360);
                Rhino.Input.RhinoGet.GetNumber("Enter the altitude of the Speaker in degrees (Z = Up/Down)...",false, ref alt,-90, 90);
                Rhino.Input.RhinoGet.GetNumber("Enter the distance from the listener in meters...", false, ref d, 0, 100);
                azi *= Math.PI / 180;
                alt *= Math.PI / 180;
                Channel_View.Items.Add(new channel(Channel_View.Items.Count, new Hare.Geometry.Vector(-Math.Cos(azi) * Math.Cos(alt), -Math.Sin(azi) * Math.Cos(alt), -Math.Sin(alt)), channel.channel_type.Custom, d / 0.343));
                Clear_Render();
            }

            private void Remove_Channel_Click(object sender, EventArgs e)
            {
                Channel_View.Items.RemoveAt(Channel_View.SelectedIndex);
                Update_Channel_IDS();
                Clear_Render();
            }

            private void Update_Channel_IDS()
            {
                int i = 0;
                foreach (channel c in Channel_View.Items)
                {
                    c.set_index(i);
                    i++;
                }
                Clear_Render();
            }

            private void Save_Channels_Click(object sender, EventArgs e)
            {
                SaveFileDialog SaveArray = new SaveFileDialog();
                SaveArray.Filter = " Array Description Text File (*.txt) |*.txt";

                if (SaveArray.ShowDialog() == DialogResult.OK)
                {
                    System.IO.StreamWriter SW = new System.IO.StreamWriter(SaveArray.FileName);

                    for (int i = 0; i < Channel_View.Items.Count; i++)
                    {
                        channel c = (channel)Channel_View.Items[i];
                        string Entry = c._index.ToString() + ':' + c.V.x.ToString() + ':' + c.V.y.ToString() + ':' + c.V.z.ToString() + ":" + c.Type.GetHashCode() + ":" + c.delay.ToString();
                        SW.WriteLine(Entry);
                    }
                    SW.Close();
                }
            }

            private class channel
            {
                public double delay;
                public Hare.Geometry.Vector V;
                public channel_type Type;
                public int _index;

                public channel(int index, Hare.Geometry.Vector Dir, channel_type c, double delay_ms)
                {
                    _index = index;
                    V = Dir;
                    Type = c;
                    delay = delay_ms;
                }

                [Flags]
                public enum channel_type
                {
                    Omnidirectional = 0x01,
                    Left = 0x02,
                    Right = 0x04,
                    hrtf = 0x08,
                    Custom = 0x10
                }

                public void set_index(int index)
                {
                    _index = index;
                }

                public static channel Left(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0), channel_type.Left, 0);
                }

                public static channel Right(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(2), -1 / Math.Sqrt(2), 0), channel_type.Right, 0);
                }

                public static channel FLU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel FRU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }
 
                public static channel FLD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel FRD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(1 / Math.Sqrt(3), -1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BLU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BRU(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BLD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel BRD(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), -1 / Math.Sqrt(3)), channel_type.Custom, 0);
                }

                public static channel Monaural(int index)
                {
                    return new channel(index, new Hare.Geometry.Vector(0, 0, 0), channel_type.Omnidirectional, 0);
                }

                public override string ToString()
                {
                    switch (Type)
                    {
                        case channel_type.Omnidirectional:
                            return "0: Omnidirectional";
                        case channel_type.Left:
                            return string.Format("{0}: Stereo Left", _index);
                        case channel_type.Right:
                            return string.Format("{0}: Stereo Right", _index);
                        case (channel_type.Left | channel_type.hrtf):
                            return string.Format("{0}: Left Ear", _index);
                        case (channel_type.Right | channel_type.hrtf):
                            return string.Format("{0}: Right Ear", _index);
                        case channel_type.Custom:
                            return string.Format("{0}: Dir-({1},{2},{3}), Delay {4} ms.", _index, Math.Round(V.x,2), Math.Round(V.y,2), Math.Round(V.z,2), Math.Round(delay));
                        default:
                            return "Whoops... Doesn't conform";
                    }
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

            private void Graph_Octave_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Graph(sender, e);
            }
        }
    }
}