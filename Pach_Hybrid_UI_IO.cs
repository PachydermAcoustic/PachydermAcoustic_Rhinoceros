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

using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Audio;
using Pachyderm_Acoustic.Utilities;
using System.Windows.Documents;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public partial class Pach_Hybrid_Control
        {
            //File I/O Utilities
            private void Write_File()
            {
                if (Direct_Data == null && IS_Data == null && IS_Data == null && this.Receiver != null)
                {
                    System.Windows.Forms.MessageBox.Show("There is no simulated data to save.");
                    return;
                }

                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                sf.DefaultExt = ".pac1";
                sf.AddExtension = true;
                sf.Filter = "Pachyderm Ray Data file (*.pac1)|*.pac1|" + "All Files|";
                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.BinaryWriter sw = new System.IO.BinaryWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    //1. Date & Time
                    sw.Write(System.DateTime.Now.ToString());
                    //2. Plugin Version... if less than 1.1, assume only 1 source.
                    sw.Write(plugin.Version);
                    //3. Cut off Time (seconds) and SampleRate
                    sw.Write((double)CO_TIME.Value);
                    sw.Write(SampleRate);
                    //4.0 Source Count(int)
                    Hare.Geometry.Point[] SRC;
                    plugin.SourceOrigin(out SRC);
                    sw.Write(SRC.Length);
                    for(int i = 0; i < SRC.Length; i++)
                    {
                        //4.1 Source Location x (double)    
                        sw.Write(SRC[i].x);
                        //4.2 Source Location y (double)
                        sw.Write(SRC[i].y);
                        //4.3 Source Location z (double)
                        sw.Write(SRC[i].z);
                    }
                    //5. No of Receivers
                    sw.Write(Recs.Length);

                    //6. Write the coordinates of each receiver point
                    for (int q = 0; q < Recs.Length; q++)
                    {
                        sw.Write(Recs[q].x);
                        sw.Write(Recs[q].y);
                        sw.Write(Recs[q].z);
                        sw.Write(Receiver[0].Rec_List[q].Rho_C);
                    }

                    for (int s = 0; s < SRC.Length; s++)
                    {
                        if (Direct_Data != null)
                        {
                            //7. Write Direct Sound Data
                            Direct_Data[s].Write_Data(ref sw);
                        }

                        if (IS_Data[0] != null)
                        {
                            //8. Write Image Source Sound Data
                            IS_Data[s].Write_Data(ref sw);
                        }

                        if (Receiver != null)
                        {
                            //9. Write Ray Traced Sound Data
                            Receiver[s].Write_Data(ref sw);
                        }
                    }
                    sw.Write("End");
                    sw.Close();
                }
            }

            private bool Read_File()
            {
                if (FileIO.Read_Pac1(ref Direct_Data, ref IS_Data, ref Receiver))
                {
                    SourceList.Items.Clear();
                    LockUserScale.Checked = false;
                    Update_Graph(null, new System.EventArgs());
                    LockUserScale.Checked = true;
                    Receiver_Choice.Text = "0";
                    OpenAnalysis();
                    cleanup();

                    Source = new Source[Direct_Data.Length];
                    Recs = new Hare.Geometry.Point[Receiver[0].Count];

                    for (int DDCT = 0; DDCT < Direct_Data.Length; DDCT++)
                    {
                        Source[DDCT] = Direct_Data[DDCT].Src;
                        SourceList.Items.Add(string.Format("S{0}-", DDCT) + Direct_Data[DDCT].type);
                        Source_Aim.Items.Add(string.Format("S{0}-", DDCT) + Direct_Data[DDCT].type);
                        SrcTypeList.Add(Direct_Data[DDCT].type);
                    }

                    CutoffTime = Direct_Data[0].Cutoff_Time;
                    SampleRate = (int)Direct_Data[0].SampleRate;

                    for (int i = 0; i < Recs.Length; i++) Recs[i] = Receiver[0].Rec_List[i].Origin;
                    return true;
                }
                else 
                {
                    System.Windows.Forms.MessageBox.Show("File Read Failed...", "Results file was corrupt or incomplete. We apologize for this inconvenience. Please report this to the software author. It will be much appreciated.");
                    return false;
                }
            }

            public void Plot_Results_Intensity()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.Text != "Sabine RT" && Parameter_Choice.Text != "Eyring RT") { return; }

                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                sf.DefaultExt = ".txt";
                sf.AddExtension = true;
                sf.Filter = "Text File (*.txt)|*.txt|" + "All Files|";

                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamWriter SW;

                    fileread:
                    try
                    {
                        SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    }
                    catch
                    {
                        System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("File is in use. Close any programs using the file, and try again.", "File In Use", System.Windows.Forms.MessageBoxButtons.RetryCancel);
                        if (dr == System.Windows.Forms.DialogResult.Cancel) return;
                        goto fileread;
                    }
                    
                    double[] Schroeder;
                    double[, ,] EDT = new double[this.SourceList.Items.Count,Recs.Length,8];
                    double[, ,] T10 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] T15 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] T20 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] T30 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] G = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] C80 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] C50 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] D50 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] TS = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] LF = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[, ,] LE = new double[this.SourceList.Items.Count, Recs.Length, 8];

                    for (int s = 0; s < SourceList.Items.Count; s++)
                    {
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            for (int oct = 0; oct < 8; oct++)
                            {
                                double[] ETC = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, s, false);
                                Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                                EDT[s, r, oct] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                                T10[s, r, oct] = AcousticalMath.T_X(Schroeder, 10, SampleRate);
                                T15[s, r, oct] = AcousticalMath.T_X(Schroeder, 15, SampleRate);
                                T20[s, r, oct] = AcousticalMath.T_X(Schroeder, 20, SampleRate);
                                T30[s, r, oct] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                                G[s, r, oct] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                                C50[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                C80[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                                D50[s, r, oct] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                TS[s, r, oct] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                                double[] L_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int>() { s }, false, (double)this.Alt_Choice.Value, (double)this.Azi_Choice.Value, true)[1];
                                LF[s, r, oct] = AcousticalMath.Lateral_Fraction(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                                LE[s, r, oct] = AcousticalMath.Lateral_Efficiency(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                            }
                        }
                    }

                    SW.WriteLine("Pachyderm Acoustic Simulation Results");
                    SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                    SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);
                    for (int s = 0; s < SourceList.Items.Count; s++)
                    {
                        SW.WriteLine("Source {0};", s);
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            SW.WriteLine("Receiver {0};63 Hz.;125 Hz.;250 Hz.;500 Hz.;1000 Hz.; 2000 Hz.;4000 Hz.; 8000 Hz.", r);
                            SW.WriteLine("Early Decay Time (EDT);{0};{1};{2};{3};{4};{5};{6};{7}", EDT[s, r, 0], EDT[s, r, 1], EDT[s, r, 2], EDT[s, r, 3], EDT[s, r, 4], EDT[s, r, 5], EDT[s, r, 6], EDT[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-10);{0};{1};{2};{3};{4};{5};{6};{7}", T10[s, r, 0], T10[s, r, 1], T10[s, r, 2], T10[s, r, 3], T10[s, r, 4], T10[s, r, 5], T10[s, r, 6], T10[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-15);{0};{1};{2};{3};{4};{5};{6};{7}", T15[s, r, 0], T15[s, r, 1], T15[s, r, 2], T15[s, r, 3], T15[s, r, 4], T15[s, r, 5], T15[s, r, 6], T15[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-20);{0};{1};{2};{3};{4};{5};{6};{7}", T20[s, r, 0], T20[s, r, 1], T20[s, r, 2], T20[s, r, 3], T20[s, r, 4], T20[s, r, 5], T20[s, r, 6], T20[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-30);{0};{1};{2};{3};{4};{5};{6};{7}", T30[s, r, 0], T30[s, r, 1], T30[s, r, 2], T30[s, r, 3], T30[s, r, 4], T30[s, r, 5], T30[s, r, 6], T30[s, r, 7]);
                            SW.WriteLine("Clarity - 50 ms (C-50);{0};{1};{2};{3};{4};{5};{6};{7}", C50[s, r, 0], C50[s, r, 1], C50[s, r, 2], C50[s, r, 3], C50[s, r, 4], C50[s, r, 5], C50[s, r, 6], C50[s, r, 7]);
                            SW.WriteLine("Definition/Deutlichkeit (D);{0};{1};{2};{3};{4};{5};{6};{7}", D50[s, r, 0], D50[s, r, 1], D50[s, r, 2], D50[s, r, 3], D50[s, r, 4], D50[s, r, 5], D50[s, r, 6], D50[s, r, 7]);
                            SW.WriteLine("Clarity - 80 ms (C-80);{0};{1};{2};{3};{4};{5};{6};{7}", C80[s, r, 0], C80[s, r, 1], C80[s, r, 2], C80[s, r, 3], C80[s, r, 4], C80[s, r, 5], C80[s, r, 6], C80[s, r, 7]);
                            SW.WriteLine("Center Time (TS);{0};{1};{2};{3};{4};{5};{6};{7}", TS[s, r, 0], TS[s, r, 1], TS[s, r, 2], TS[s, r, 3], TS[s, r, 4], TS[s, r, 5], TS[s, r, 6], TS[s, r, 7]);
                            SW.WriteLine("Strength(G);{0};{1};{2};{3};{4};{5};{6};{7}", G[s, r, 0], G[s, r, 1], G[s, r, 2], G[s, r, 3], G[s, r, 4], G[s, r, 5], G[s, r, 6], G[s, r, 7]);
                            SW.WriteLine("Lateral Fraction(LF);{0};{1};{2};{3};{4};{5};{6};{7}", LF[s, r, 0], LF[s, r, 1], LF[s, r, 2], LF[s, r, 3], LF[s, r, 4], LF[s, r, 5], LF[s, r, 6], LF[s, r, 7]);
                            SW.WriteLine("Lateral Efficiency(LE);{0};{1};{2};{3};{4};{5};{6};{7}", LE[s, r, 0], LE[s, r, 1], LE[s, r, 2], LE[s, r, 3], LE[s, r, 4], LE[s, r, 5], LE[s, r, 6], LE[s, r, 7]);                        
                        }
                    }
                    SW.Close();
                }
            }

            public void Plot_Results_Pressure()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.Text != "Sabine RT" && Parameter_Choice.Text != "Eyring RT") { return; }

                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                sf.DefaultExt = ".txt";
                sf.AddExtension = true;
                sf.Filter = "Text File (*.txt)|*.txt|" + "All Files|";

                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamWriter SW;

                fileread:
                    try
                    {
                        SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    }
                    catch
                    {
                        System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("File is in use. Close any programs using the file, and try again.", "File In Use", System.Windows.Forms.MessageBoxButtons.RetryCancel);
                        if (dr == System.Windows.Forms.DialogResult.Cancel) return;
                        goto fileread;
                    }

                    double[] Schroeder;
                    double[,,] EDT = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] T10 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] T15 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] T20 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] T30 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] G = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] C80 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] C50 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] D50 = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] TS = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] LF = new double[this.SourceList.Items.Count, Recs.Length, 8];
                    double[,,] LE = new double[this.SourceList.Items.Count, Recs.Length, 8];

                    for (int s = 0; s < SourceList.Items.Count; s++)
                    {
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            double[] PTC = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new System.Collections.Generic.List<int>(1){ s }, false, true);
                            for (int oct = 0; oct < 8; oct++)
                            {
                                double[] ETC = Pachyderm_Acoustic.Audio.Pach_SP.FIR_Bandpass(PTC, oct, SampleRate, 0);
                                for (int i = 0; i < ETC.Length; i++) { ETC[i] = AcousticalMath.Intensity_Pressure(ETC[i]); }; Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                                EDT[s, r, oct] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                                T10[s, r, oct] = AcousticalMath.T_X(Schroeder, 10, SampleRate);
                                T15[s, r, oct] = AcousticalMath.T_X(Schroeder, 15, SampleRate);
                                T20[s, r, oct] = AcousticalMath.T_X(Schroeder, 20, SampleRate);
                                T30[s, r, oct] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                                G[s, r, oct] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                                C50[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                C80[s, r, oct] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                                D50[s, r, oct] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                                TS[s, r, oct] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                                double[] L_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int>() { s }, false, (double)this.Alt_Choice.Value, (double)this.Azi_Choice.Value, true)[1];
                                LF[s, r, oct] = AcousticalMath.Lateral_Fraction(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                                LE[s, r, oct] = AcousticalMath.Lateral_Efficiency(ETC, L_ETC, SampleRate, Direct_Data[s].Min_Time(r), false) * 1000;
                            }
                        }
                    }

                    SW.WriteLine("Pachyderm Acoustic Simulation Results");
                    SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                    SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);
                    for (int s = 0; s < SourceList.Items.Count; s++)
                    {
                        SW.WriteLine("Source {0};", s);
                        for (int r = 0; r < Recs.Length; r++)
                        {
                            SW.WriteLine("Receiver {0};63 Hz.;125 Hz.;250 Hz.;500 Hz.;1000 Hz.; 2000 Hz.;4000 Hz.; 8000 Hz.", r);
                            SW.WriteLine("Early Decay Time (EDT);{0};{1};{2};{3};{4};{5};{6};{7}", EDT[s, r, 0], EDT[s, r, 1], EDT[s, r, 2], EDT[s, r, 3], EDT[s, r, 4], EDT[s, r, 5], EDT[s, r, 6], EDT[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-10);{0};{1};{2};{3};{4};{5};{6};{7}", T10[s, r, 0], T10[s, r, 1], T10[s, r, 2], T10[s, r, 3], T10[s, r, 4], T10[s, r, 5], T10[s, r, 6], T10[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-15);{0};{1};{2};{3};{4};{5};{6};{7}", T15[s, r, 0], T15[s, r, 1], T15[s, r, 2], T15[s, r, 3], T15[s, r, 4], T15[s, r, 5], T15[s, r, 6], T15[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-20);{0};{1};{2};{3};{4};{5};{6};{7}", T20[s, r, 0], T20[s, r, 1], T20[s, r, 2], T20[s, r, 3], T20[s, r, 4], T20[s, r, 5], T20[s, r, 6], T20[s, r, 7]);
                            SW.WriteLine("Reverberation Time (T-30);{0};{1};{2};{3};{4};{5};{6};{7}", T30[s, r, 0], T30[s, r, 1], T30[s, r, 2], T30[s, r, 3], T30[s, r, 4], T30[s, r, 5], T30[s, r, 6], T30[s, r, 7]);
                            SW.WriteLine("Clarity - 50 ms (C-50);{0};{1};{2};{3};{4};{5};{6};{7}", C50[s, r, 0], C50[s, r, 1], C50[s, r, 2], C50[s, r, 3], C50[s, r, 4], C50[s, r, 5], C50[s, r, 6], C50[s, r, 7]);
                            SW.WriteLine("Definition/Deutlichkeit (D);{0};{1};{2};{3};{4};{5};{6};{7}", D50[s, r, 0], D50[s, r, 1], D50[s, r, 2], D50[s, r, 3], D50[s, r, 4], D50[s, r, 5], D50[s, r, 6], D50[s, r, 7]);
                            SW.WriteLine("Clarity - 80 ms (C-80);{0};{1};{2};{3};{4};{5};{6};{7}", C80[s, r, 0], C80[s, r, 1], C80[s, r, 2], C80[s, r, 3], C80[s, r, 4], C80[s, r, 5], C80[s, r, 6], C80[s, r, 7]);
                            SW.WriteLine("Center Time (TS);{0};{1};{2};{3};{4};{5};{6};{7}", TS[s, r, 0], TS[s, r, 1], TS[s, r, 2], TS[s, r, 3], TS[s, r, 4], TS[s, r, 5], TS[s, r, 6], TS[s, r, 7]);
                            SW.WriteLine("Strength(G);{0};{1};{2};{3};{4};{5};{6};{7}", G[s, r, 0], G[s, r, 1], G[s, r, 2], G[s, r, 3], G[s, r, 4], G[s, r, 5], G[s, r, 6], G[s, r, 7]);
                            SW.WriteLine("Lateral Fraction(LF);{0};{1};{2};{3};{4};{5};{6};{7}", LF[s, r, 0], LF[s, r, 1], LF[s, r, 2], LF[s, r, 3], LF[s, r, 4], LF[s, r, 5], LF[s, r, 6], LF[s, r, 7]);
                            SW.WriteLine("Lateral Efficiency(LE);{0};{1};{2};{3};{4};{5};{6};{7}", LE[s, r, 0], LE[s, r, 1], LE[s, r, 2], LE[s, r, 3], LE[s, r, 4], LE[s, r, 5], LE[s, r, 6], LE[s, r, 7]);
                        }
                    }
                    SW.Close();
                }
            }

            public void Plot_PTB_Results_Intensity()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.Text != "Sabine RT" && Parameter_Choice.Text != "Eyring RT") { return; }
                double[] Schroeder;
                string[] paramtype = new string [] {"T30/s","EDT/s", "D/%", "C/dB", "TS/ms", "G/dB", "LF%", "LFC%", "IACC"};//LF/% LFC/% IACC
                string ReceiverLine = "Receiver{0};";
                double[, , ,] ParamValues = new double[SourceList.Items.Count, Recs.Length, 8, paramtype.Length];

                for (int s = 0; s < Direct_Data.Length; s++)
                {
                    for (int r = 0; r < Recs.Length; r++)
                    {
                        ReceiverLine += r.ToString() + ";";
                        for (int oct = 0; oct < 8; oct++)
                        {
                            double[] ETC = IR_Construction.ETCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, s, false);
                            Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                            ParamValues[s, r, oct, 0] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                            ParamValues[s, r, oct, 1] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                            ParamValues[s, r, oct, 2] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 3] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 4] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                            ParamValues[s, r, oct, 5] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                            double azi, alt;
                            PachTools.World_Angles(Direct_Data[s].Src.Origin(), Recs[r], true, out alt, out azi);
                            double[][] Lateral_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int> { s }, false, alt, azi, true);
                            ParamValues[s, r, oct, 6] = AcousticalMath.Lateral_Fraction(ETC, Lateral_ETC, SampleRate, Direct_Data[s].Min_Time(r), false);
                        }
                    }
                }

                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                sf.DefaultExt = ".txt";
                sf.AddExtension = true;
                sf.Filter = "Text File (*.txt)|*.txt|" + "All Files|";

                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        System.IO.StreamWriter SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                        SW.WriteLine("Pachyderm Acoustic Simulation Results");
                        SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                        SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);

                        for (int oct = 0; oct < 8; oct++)
                        {
                            SW.WriteLine(string.Format(ReceiverLine, oct));
                            for (int param = 0; param < paramtype.Length; param++)
                            {
                                SW.Write(paramtype[param] + ";");
                                for (int i = 0; i < Direct_Data.Length; i++)
                                {
                                    for (int q = 0; q < Recs.Length; q++)
                                    {
                                        SW.Write(ParamValues[i, q, oct, param].ToString() + ";");
                                    }
                                }
                                SW.WriteLine("");
                            }
                        }
                        SW.Close();
                    }
                    catch (System.Exception)
                    {
                        Rhino.RhinoApp.WriteLine("File is open, and cannot be written over.");
                        return;
                    }
                }
            }

            public void Plot_PTB_Results_Pressure()
            {
                if (Direct_Data == null && IS_Data == null && Receiver == null && Parameter_Choice.Text != "Sabine RT" && Parameter_Choice.Text != "Eyring RT") { return; }
                double[] Schroeder;
                string[] paramtype = new string[] { "T30/s", "EDT/s", "D/%", "C/dB", "TS/ms", "G/dB", "LF%", "LFC%", "IACC" };//LF/% LFC/% IACC
                string ReceiverLine = "Receiver{0};";
                double[,,,] ParamValues = new double[SourceList.Items.Count, Recs.Length, 8, paramtype.Length];

                for (int s = 0; s < Direct_Data.Length; s++)
                {
                    for (int r = 0; r < Recs.Length; r++)
                    {
                        ReceiverLine += r.ToString() + ";";
                        double[] PTC = IR_Construction.PressureTimeCurve(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, r, new System.Collections.Generic.List<int>(1) { s }, false, true);
                        for (int oct = 0; oct < 8; oct++)
                        {
                            double[] ETC = Pachyderm_Acoustic.Audio.Pach_SP.FIR_Bandpass(PTC, oct, SampleRate, 0);
                            for(int i = 0; i < ETC.Length; i++) { ETC[i] = AcousticalMath.Intensity_Pressure(ETC[i]); };
                            Schroeder = AcousticalMath.Schroeder_Integral(ETC);
                            ParamValues[s, r, oct, 0] = AcousticalMath.T_X(Schroeder, 30, SampleRate);
                            ParamValues[s, r, oct, 1] = AcousticalMath.EarlyDecayTime(Schroeder, SampleRate);
                            ParamValues[s, r, oct, 2] = AcousticalMath.Definition(ETC, SampleRate, 0.05, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 3] = AcousticalMath.Clarity(ETC, SampleRate, 0.08, Direct_Data[s].Min_Time(r), false);
                            ParamValues[s, r, oct, 4] = AcousticalMath.Center_Time(ETC, SampleRate, Direct_Data[s].Min_Time(r)) * 1000;
                            ParamValues[s, r, oct, 5] = AcousticalMath.Strength(ETC, Direct_Data[s].SWL[oct], false);
                            double azi, alt;
                            PachTools.World_Angles(Direct_Data[s].Src.Origin(), Recs[r], true, out alt, out azi);
                            double[][] Lateral_ETC = IR_Construction.ETCurve_1d(Direct_Data, IS_Data, Receiver, CutoffTime, SampleRate, oct, r, new System.Collections.Generic.List<int> { s }, false, alt, azi, true);
                            ParamValues[s, r, oct, 6] = AcousticalMath.Lateral_Fraction(ETC, Lateral_ETC, SampleRate, Direct_Data[s].Min_Time(r), false);
                        }
                    }
                }

                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                sf.DefaultExt = ".txt";
                sf.AddExtension = true;
                sf.Filter = "Text File (*.txt)|*.txt|" + "All Files|";

                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        System.IO.StreamWriter SW = new System.IO.StreamWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                        SW.WriteLine("Pachyderm Acoustic Simulation Results");
                        SW.WriteLine("Saved {0}", System.DateTime.Now.ToString());
                        SW.WriteLine("Filename:{0}", Rhino.RhinoDoc.ActiveDoc.Name);

                        for (int oct = 0; oct < 8; oct++)
                        {
                            SW.WriteLine(string.Format(ReceiverLine, oct));
                            for (int param = 0; param < paramtype.Length; param++)
                            {
                                SW.Write(paramtype[param] + ";");
                                for (int i = 0; i < Direct_Data.Length; i++)
                                {
                                    for (int q = 0; q < Recs.Length; q++)
                                    {
                                        SW.Write(ParamValues[i, q, oct, param].ToString() + ";");
                                    }
                                }
                                SW.WriteLine("");
                            }
                        }
                        SW.Close();
                    }
                    catch (System.Exception)
                    {
                        Rhino.RhinoApp.WriteLine("File is open, and cannot be written over.");
                        return;
                    }
                }
            }
        }
    }
}