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
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Audio;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using MathNet.Numerics.RootFinding;
using MathNet.Numerics;
using Pachyderm_Acoustic.AbsorptionModels;
using System.Windows.Media.Media3D;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("12db68c3-c995-43c6-860a-6bd106b94a4c")]
        public partial class Pach_Auralisation
        {
            AuralisationConduit A;
            bool Linear_Phase;
            int[] SrcRendered;
            int RecRendered;
            double[][] Response;
            int SFreq_Rendered;
            // This call is required by the Windows Form Designer. 
            public Pach_Auralisation()
            {
                A = new AuralisationConduit();
                InitializeComponent();
                DistributionType.SelectedIndex = 1;
                Graph_Octave.SelectedIndex = 5;
                DistributionType_SelectedIndexChanged(this, new EventArgs());
                Sample_Freq_Selection.SelectedIndex = 1;
                Instance = this;
            }

            public void Reset()
            {
                Linear_Phase = false;
                Direct_Data = null;
                Receiver = null;
                IS_Data = null;
                Maps = null;
                Srcs = null;
                Recs = null;
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
            PachMapReceiver[] Maps;
            Hare.Geometry.Point[] Srcs;
            Hare.Geometry.Point[] Recs;
            int SampleRate;
            double CutoffTime;

            private void Draw_Feedback()
            {
                if (Hybrid_Select.Checked && Direct_Data != null)
                {
                    if (Pach_Hybrid_Control.Instance != null && Receiver_Choice.SelectedIndex < 0) return;
                    Hare.Geometry.Point[] rec = new Hare.Geometry.Point[Recs.Length];
                    for (int i = 0; i < Recs.Length; i++) rec[i] = Recs[i];
                    AuralisationConduit.Instance.Enabled = true;
                    AuralisationConduit.Instance.add_Receivers(rec);
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();
                    foreach (Direct_Sound D in Direct_Data) pts.Add(D.Src_Origin);
                    AuralisationConduit.Instance.add_Sources(pts);
                    List<Deterministic_Reflection> Paths = new List<Deterministic_Reflection>();
                    List<Polyline> Lines = new List<Polyline>();
                    foreach (ImageSourceData I in IS_Data) if (I != null) Paths.AddRange(I.Paths[Receiver_Choice.SelectedIndex]);
                    foreach (Deterministic_Reflection p in Paths) foreach (Hare.Geometry.Point[] P in p.Path)
                        {
                            List<Rhino.Geometry.Point3d> PTS = new List<Rhino.Geometry.Point3d>();
                            foreach (Hare.Geometry.Point hpt in P)
                            {
                                PTS.Add(Utilities.RC_PachTools.HPttoRPt(hpt));
                            }
                            Lines.Add(new Polyline(PTS));
                        }
                    AuralisationConduit.Instance.add_Reflections(Lines);
                    pts.Clear();
                    List<Vector3d> Dirs = new List<Vector3d>();
                    for (int i = 0; i < this.Channel_View.Items.Count; i++)
                    {
                        Hare.Geometry.Vector TempDir = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(((channel)Channel_View.Items[i]).V, 0, -(double)Alt_Choice.Value, true), -(double)Azi_Choice.Value, 0, true);//new Hare.Geometry.Vector(Speaker_Directions[i].X, Speaker_Directions[i].Y, Speaker_Directions[i].Z)
                        //Hare.Geometry.Vector TempDir = Utilities.PachTools.Rotate_Vector(((channel)Channel_View.Items[i]).V, -(double)Azi_Choice.Value, -(double)Alt_Choice.Value, true);
                        TempDir.Normalize();
                        pts.Add(Recs[0] + (TempDir) * -.343 * Math.Max(5, ((channel)Channel_View.Items[i]).delay));
                        Dirs.Add(new Vector3d(TempDir.x, TempDir.y, TempDir.z));
                    }
                    AuralisationConduit.Instance.add_Speakers(pts, Dirs);
                    AuralisationConduit.Instance.set_direction(Utilities.RC_PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), Utilities.RC_PachTools.HPttoRPt(Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), 0, -(double)Alt_Choice.Value, true), -(double)Azi_Choice.Value, 0, true)));
                    //AuralisationConduit.Instance.set_direction(Utilities.PachTools.HPttoRPt(Recs[Receiver_Choice.SelectedIndex]), Utilities.PachTools.HPttoRPt(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(1, 0, 0), -(double)Azi_Choice.Value, -(double)Alt_Choice.Value, true)));
                }
                else if (Mapping_Select.Checked)
                {
                    if (Pach_Mapping_Control.Instance != null && !Pach_Mapping_Control.Instance.Auralisation_Ready() && Receiver_Choice.SelectedIndex < 0) return;
                    if (Maps == null || Maps[0] == null) return;
                    AuralisationConduit.Instance.add_Receivers(Maps[0].Origins());
                    List<Hare.Geometry.Point> pts = new List<Hare.Geometry.Point>();
                    foreach (PachMapReceiver m in Maps) pts.Add(m.Src);
                    AuralisationConduit.Instance.add_Sources(pts);
                }
                else
                {
                    AuralisationConduit.Instance.Enabled = false;
                }
                Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.Redraw();
            }

            public void Set_Phase_Regime(bool Linear_Phase)
            {
                if (Direct_Data == null) return;

                if ((Linear_Phase != true && Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System) || (Linear_Phase != false && Audio.Pach_SP.Filter is Audio.Pach_SP.Minimum_Phase_System) || Direct_Data[0].F == null)
                {
                    for (int i = 0; i < Direct_Data.Length; i++) Direct_Data[i].Create_Filter();
                    for (int i = 0; i < IS_Data.Length; i++) IS_Data[i].Create_Filter(Direct_Data[i].SWL, 4096);
                    for (int i = 0; i < Receiver.Length; i++) Receiver[i].Create_Filter();
                    this.Linear_Phase = Linear_Phase;
                }
            }

            public double[][] Render_Filter(int Sample_Frequency)
            {
                double[][] Temp;
                double[][] Response;
                switch (DistributionType.Text)
                {
                    case "Monaural":
                        Response = new double[1][];
                        Response[0] = (IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true));
                        break;
                    case "First Order Ambisonics (ACN+SN3D)":
                        Response = new double[4][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        break;
                    case "First Order Ambisonics (FuMa+SN3D)":
                        Response = new double[4][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        break;
                    case "Second Order Ambisonics(ACN+SN3D)":
                        Response = new double[9][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        break;
                    case "Second Order Ambisonics(FuMa+SN3D)":
                        Response = new double[9][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        break;
                    case "Third Order Ambisonics(ACN+SN3D)":
                        Response = new double[16][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        Temp = IR_Construction.AurFilter_Ambisonics3(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.ACN);
                        Response[9] = Temp[0];
                        Response[10] = Temp[1];
                        Response[11] = Temp[2];
                        Response[12] = Temp[3];
                        Response[13] = Temp[4];
                        Response[14] = Temp[5];
                        Response[15] = Temp[6];
                        break;

                    case "Third Order Ambisonics(FuMa+SN3D)":
                        Response = new double[16][];
                        Temp = IR_Construction.AurFilter_Fig8_3Axis(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[0] = IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, true);
                        Response[1] = Temp[0];
                        Response[2] = Temp[1];
                        Response[3] = Temp[2];
                        Temp = IR_Construction.AurFilter_Ambisonics2(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
                        Response[4] = Temp[0];
                        Response[5] = Temp[1];
                        Response[6] = Temp[2];
                        Response[7] = Temp[3];
                        Response[8] = Temp[4];
                        Temp = IR_Construction.AurFilter_Ambisonics3(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, Receiver_Choice.SelectedIndex, SelectedSources(), false, (double)Alt_Choice.Value, (double)Azi_Choice.Value, true, true, IR_Construction.Ambisonics_Component_Order.FuMa);
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
                            Response[i] = IR_Construction.AurFilter_Directional(Direct_Data, IS_Data, Receiver, CutoffTime, Sample_Frequency, PachTools.OctaveStr2Int(Graph_Octave.Text), Receiver_Choice.SelectedIndex, SelectedSources(), false, alt, azi, true, true);
                        }
                        break;
                }
                return Response;
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

                    List<System.Drawing.Color> C = new List<System.Drawing.Color> {System.Drawing.Color.Red, System.Drawing.Color.OrangeRed, System.Drawing.Color.Orange, System.Drawing.Color.DarkGoldenrod, System.Drawing.Color.Olive, System.Drawing.Color.Green, System.Drawing.Color.Aquamarine, System.Drawing.Color.Azure, System.Drawing.Color.Blue, System.Drawing.Color.Indigo, System.Drawing.Color.Violet};

                    Response = Render_Filter(44100);

                    //Get the maximum value of the Direct Sound
                    double DirectMagnitude = 0;
                    foreach (int i in SourceList.CheckedIndices)
                    {
                        double[] E = Direct_Data[i].EnergyValue(OCT_ID, REC_ID);
                        for (int j = 0; j < E.Length; j++)
                        {
                            double D = AcousticalMath.SPL_Intensity(E[j])-150;
                            if (D > DirectMagnitude) DirectMagnitude = D;
                        }
                    }

                    double[] time = new double[Response[0].Length];
                    for (int i = 0; i < Response[0].Length; i++)
                    {
                        time[i] = (double)i / SampleRate - 2048f/SampleRate;
                    }

                    for (int i = 0; i < Response.Length; i++)
                    {
                        if (OCT_ID < 8)
                        {
                            double[] filter = Audio.Pach_SP.FIR_Bandpass(Response[i], OCT_ID, SampleRate, 0);
                            Array.Resize(ref filter, filter.Length - 12288);
                            Analysis_View.GraphPane.AddCurve(String.Format("Channel {0}", i), time, AcousticalMath.SPL_Pressure_Signal(filter), C[i % 10], ZedGraph.SymbolType.None);
                        }
                        else
                        {
                            Analysis_View.GraphPane.AddCurve(String.Format("Channel {0}", i), time, AcousticalMath.SPL_Pressure_Signal(Response[i]), C[i % 10], ZedGraph.SymbolType.None);
                        }
                    }
                    Analysis_View.GraphPane.XAxis.Scale.Max = time[time.Length - 1];
                    Analysis_View.GraphPane.XAxis.Scale.Min = time[0];
                    Analysis_View.GraphPane.YAxis.Scale.Max = DirectMagnitude;//(OCT_ID == 8)? 3E-6 * Math.Pow(10, DirectMagnitude/20) * 1.1 : 
                    Analysis_View.GraphPane.YAxis.Scale.Min = DirectMagnitude - 100;

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
                    int[][] signal = Audio.Pach_SP.Wave.ReadtoInt(Signal_Status.Text, false, out SampleRate);
                    DryChannel.Minimum = 1;
                    DryChannel.Maximum = signal.Length;
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
                SignalETC = Audio.Pach_SP.Wave.ReadtoDouble(Signal_Status.Text, true, out Sample_Freq)[0];
            }

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
                OpenWaveFile(out SamplesPerSec, out SignalBuffer);

                float maxvalue = 0;
                //Normalize input signal...
                for (int j = 0; j < SignalBuffer.Length; j++) maxvalue = (float)Math.Max(maxvalue, Math.Abs(SignalBuffer[j]));
                for (int j = 0; j < SignalBuffer.Length; j++) SignalBuffer[j] /= maxvalue;
                //Convert pressure response to a 24-bit dynamic range:

                double[][] Render_Response = Render_Filter(SamplesPerSec);

                float[][] NewSignal = new float[(int)Render_Response.Length][];
                for (int i = 0; i < Render_Response.Length; i++)
                {
                    NewSignal[i] = Pach_SP.FFT_Convolution(SignalBuffer, Render_Response[i], 0);
                    for (int j = 0; j < NewSignal[i].Length; j++) NewSignal[i][j] *= (float)(Math.Pow(10, 120 / 20) / Math.Pow(10, ((double)Normalization_Choice.Value + 60)/20));
                }

                SrcRendered = new int[SourceList.CheckedIndices.Count];
                for (int j = 0; j < SourceList.CheckedIndices.Count; j++)
                {
                    SrcRendered[j] = SourceList.CheckedIndices[j];
                }
                RecRendered = int.Parse(Receiver_Choice.Text);
                SFreq_Rendered = SamplesPerSec;

                SaveFileDialog SaveWave = new SaveFileDialog();
                if (NewSignal.Length < 4)
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

                    Audio.Pach_SP.Wave.Write(NewSignal, SamplesPerSec, SaveWave.FileName);

                    //TODO: Users find this annoying - make it optional, and provide a way to kill it.

                    if (PlayAuralization.Checked)
                    {
                        Player = new System.Media.SoundPlayer(SaveWave.FileName);
                        Player.Play();
                    }
                }
            }

            System.Media.SoundPlayer Player =  new System.Media.SoundPlayer();

            private void ExportFilter(object sender, EventArgs e)
            {
                SaveFileDialog SaveWave = new SaveFileDialog();

                if (Response.Length < 4)
                {
                    SaveWave.Filter = "Wave Audio (*.wav) |*.wav";
                }
                else 
                {
                    SaveWave.Filter = "Wave Audio (*.wavex) |*.wavex";
                }

                int SamplesPerSec = 44100;

                switch (Sample_Freq_Selection.SelectedIndex)
                {
                    case 0:
                        SamplesPerSec = 22050;
                        break;
                    case 1:
                        SamplesPerSec = 44100;
                        break;
                    case 2:
                        SamplesPerSec = 48000;
                        break;
                    case 3:
                        SamplesPerSec = 96000;
                        break;
                    case 4:
                        SamplesPerSec = 192000;
                        break;
                    case 5:
                        SamplesPerSec = 384000;
                        break;
                }

                double[][] Render_Response = Render_Filter(SamplesPerSec);
                float[][] RR = new float[Render_Response.Length][];
                int maxlength = 0;
                for (int j = 0; j < Render_Response.Length; j++) maxlength = Math.Max(Render_Response[j].Length, maxlength);
                //for (int j = 0; j < Render_Response.Length; j++) Rende[j] = new double[maxlength];

                float mod = (float)(Math.Pow(10, 120 / 20) / Math.Pow(10, ((double)Normalization_Choice.Value + 15)/ 20));
                for (int c = 0; c < Render_Response.Length; c++) RR[c] = new float[Render_Response[c].Length];

                for (int j = 0; j < Render_Response[0].Length; j++)
                {
                    for (int c = 0; c < Render_Response.Length; c++)  RR[c][j] = (j > Render_Response[c].Length - 1)? 0 : (float)Render_Response[c][j] * mod;
                }

                if (SaveWave.ShowDialog() == DialogResult.OK)
                {
                    Audio.Pach_SP.Wave.Write(RR, SamplesPerSec, SaveWave.FileName, 24);
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

                PachTools.World_Angles(Direct_Data[Source_Aim.SelectedIndex].Src.Origin(), Recs[Receiver_Choice.SelectedIndex], true, out alt, out azi);

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
                        UI.Pach_Hybrid_Control.Instance.GetSims(ref Srcs, ref Recs, ref Direct_Data, ref IS_Data, ref Receiver);
                        //Set_Phase_Regime(Audio.Pach_SP.Filter is Audio.Pach_SP.Linear_Phase_System);
                        if (Direct_Data != null)
                        {
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

                        double max = 0;
                        for (int i = 0; i < Direct_Data.Length; i++)
                        {
                            double[] Response = (IR_Construction.Auralization_Filter(Direct_Data, IS_Data, Receiver, CutoffTime, 44100, i, SelectedSources(), false, true));
                            double tmax = Math.Max(Response.Max(), Math.Abs(Response.Min()));
                            max = Math.Max(max, tmax);
                        }
                        max = AcousticalMath.SPL_Pressure(max);
                        Normalization_Choice.Value = (decimal)max;
                    }
                    else if (Mapping_Select.Checked)
                    {
                        if (UI.Pach_Mapping_Control.Instance != null && UI.Pach_Mapping_Control.Instance.Auralisation_Ready())
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

            private void PlayAuralization_CheckedChanged(object sender, EventArgs e)
            {
                if (PlayAuralization.Checked) Player.Play();
                else Player.Stop();
            }
        }
    }
}