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

using System;
using Rhino.Geometry;   
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Visualization;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;
using Rhino.UI;
using Eto.Drawing;
using Eto.Forms;
using System.Threading.Tasks;
using System.ComponentModel;
using MathNet.Numerics.Statistics.Mcmc;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("EA23F0D6-5462-4e42-9CFC-DC8E79723112")]
        public partial class PachVisualControl : Panel, IPanel
        {
            Source[] Source;

            internal Button Loop;
            internal Button Forw;
            internal Button StartOver;
            internal Button BackButton;
            internal Label FrRate_Label;
            internal DropDown VisualizationSelect;
            internal Label VisLabel;
            internal NumericStepper Frame_Rate;
            internal Label SecLabel;
            internal NumericStepper Seconds;
            internal Label COTimeLabel;
            internal NumericStepper CO_TIME;
            internal Label AirTemp;
            internal NumericStepper Air_Temp;
            internal Label RayctLabel;
            internal NumericStepper RT_Count;
            internal Button ForwButton;
            internal Label Time_Preview;
            internal Button Preview;
            internal Color_Output_Control colorcontrol;
            internal Label SelectOutput;
            internal TextBox Folder_Status;
            internal Button Clear_Folder;
            internal Button OpenFolder;

            internal GroupBox SimSetBox;
            internal GroupBox TimeBox;
            internal GroupBox OutputFolder;

            public PachVisualControl()
            {
                this.Forw = new Button();
                this.StartOver = new Button();
                this.TimeBox = new GroupBox();
                this.Time_Preview = new Label();
                this.Preview = new Button();
                this.Loop = new Button();
                this.BackButton = new Button();
                this.VisualizationSelect = new ComboBox();
                this.SimSetBox = new GroupBox();
                this.FrRate_Label = new Label();
                this.Frame_Rate = new NumericStepper();
                this.SecLabel = new Label();
                this.Seconds = new NumericStepper();
                this.COTimeLabel = new Label();
                this.CO_TIME = new NumericStepper();
                this.AirTemp = new Label();
                this.Air_Temp = new NumericStepper();
                this.RayctLabel = new Label();
                this.RT_Count = new NumericStepper();
                this.SelectOutput = new Label();
                this.Folder_Status = new TextBox();
                this.Clear_Folder = new Button();
                this.OpenFolder = new Button();
                this.ForwButton = new Button();
                this.VisLabel = new Label();
                this.colorcontrol = new Color_Output_Control();
                this.OutputFolder = new GroupBox();

                colorcontrol.Update += On_Output_Change;

                this.SimSetBox.Text = "Simulation Settings";

                this.VisLabel.Text = "Visualisation";
                this.VisualizationSelect.Items.Add("Smart Particle Wave");
                this.VisualizationSelect.Items.Add("Particle Wave");
                this.VisualizationSelect.Items.Add("Mesh Wave");
                this.VisualizationSelect.SelectedIndex = 0;
                this.VisualizationSelect.Width = 250;

                DynamicLayout TimeControls = new DynamicLayout();
                this.FrRate_Label.Text = "Frame Rate";
                this.Frame_Rate.MaxValue = 80;
                this.Frame_Rate.MinValue = 15;
                this.Frame_Rate.Value = 30;

                this.SecLabel.Text = "Duration (seconds)";
                this.Seconds.MaxValue = 8000;
                this.Seconds.MinValue = 1;
                Seconds.Value = 20;

                this.COTimeLabel.Text = "Cut Off Time (ms)";
                this.CO_TIME.MaxValue = 8000;
                this.CO_TIME.MinValue = 1;
                this.CO_TIME.Value = 300;

                this.AirTemp.Text = "Air Temperature (C)";
                this.Air_Temp.MaxValue = 80;
                this.Air_Temp.MinValue = 0;
                this.Air_Temp.Value = 20;

                this.RayctLabel.Text = "Number of Rays";
                this.RT_Count.Increment = 1000;
                this.RT_Count.MaxValue = 10000000;
                this.RT_Count.MinValue = 100;
                this.RT_Count.Value = 250;

                DynamicLayout TA = new DynamicLayout();
                TA.DefaultSpacing = new Size(20, 8);
                TA.Spacing = new Size(20, 8);
                TA.AddRow(VisLabel, null, VisualizationSelect);
                DynamicLayout TB = new DynamicLayout();
                TB.DefaultSpacing = new Size(8, 8);
                TB.AddRow(RayctLabel, null, RT_Count);
                TB.AddRow(COTimeLabel, null, CO_TIME);
                TB.AddRow(null);
                TB.AddRow(AirTemp, null, Air_Temp);
                TB.AddRow(null);
                TB.AddRow(FrRate_Label, null, Frame_Rate);
                TB.AddRow(SecLabel, null, Seconds);
                /////////////////////////////////////
                DynamicLayout TL = new DynamicLayout();
                TL.DefaultSpacing = new Size(0, 8);
                TL.AddRow(TA);
                TL.AddRow(TB);
                SimSetBox.Content = TL;

                // 
                // Playback Buttons
                //
                this.Preview.Text = "Preview";
                this.Loop.Text = "Loop";
                this.Preview.Click += this.onPreview_Click;
                this.Loop.Click += this.Loop_Click;
                
                this.StartOver.Text = "|<";
                this.StartOver.Click += this.ToStart_Click;
                this.StartOver.Width = this.Width / 8;
                this.BackButton.Text = "<<";
                this.BackButton.Width = this.Width / 8;
                this.BackButton.Click += this.Rev_Click;
                this.ForwButton.Text = ">>";
                this.ForwButton.Width = this.Width / 8;
                this.ForwButton.Click += this.Forw_Click;
                this.TimeBox.Content = Time_Preview;
                this.Time_Preview.Text = "0 ms.";
                //
                // Time
                //
                this.TimeBox.Text = "Current Time (ms)";
                DynamicLayout timecontrol = new DynamicLayout();
                timecontrol.DefaultSpacing = new Size(8, 8);
                timecontrol.SizeChanged += delegate {BackButton.Width = timecontrol.Width / 8; Loop.Width = timecontrol.Width * 5/8; ForwButton.Width = timecontrol.Width / 8; };
                timecontrol.AddRow(StartOver, Time_Preview, null);
                timecontrol.AddRow(BackButton, Loop, ForwButton);
                DynamicLayout DL = new DynamicLayout();
                DL.AddRow(timecontrol);
                DL.AddRow(Preview);
                DL.DefaultSpacing = new Size(0, 8);
                TimeBox.Content = DL;

                // 
                // Folder_Status
                // 
                OutputFolder.Text = "Select Output Folder";
                DynamicLayout output = new DynamicLayout();
                output.DefaultSpacing = new Size(8, 8);
                this.Folder_Status.ReadOnly = true;
                StackLayout Folder_Ctrl = new StackLayout();
                this.OpenFolder.Text = "Open...";
                this.OpenFolder.Click += this.OpenFolder_Click;
                this.Clear_Folder.Text = "Clear";
                this.Clear_Folder.Click += delegate { Folder_Status.Text = ""; };
                Folder_Ctrl.Items.Add(OpenFolder);
                Folder_Ctrl.Items.Add(Clear_Folder);
                output.AddRow(Folder_Ctrl, Folder_Status);
                OutputFolder.Content = output;

                //Final Control Assembly
                DynamicLayout layout = new DynamicLayout();
                layout.AddRow(SimSetBox);
                layout.AddRow(TimeBox);
                layout.AddRow(colorcontrol);
                layout.AddRow(OutputFolder);
                layout.DefaultSpacing = new Size(0, 20);

                Scrollable OverallScroll = new Scrollable();
                OverallScroll.Content = layout;
                this.Content = OverallScroll;
            }

            private double CutOffLength()
            {
                return ((double)CO_TIME.Value / 1000) * C_Sound();
            }

            private double C_Sound()
            {
                return AcousticalMath.SoundSpeed((double)Air_Temp.Value);
            }

            //Select folder for frames
            private Eto.Forms.SelectFolderDialog FileLocation = new Eto.Forms.SelectFolderDialog();
            private void OpenFolder_Click(object sender, System.EventArgs e)
            {
                if (FileLocation.ShowDialog(RhinoEtoApp.MainWindow) == DialogResult.Ok)
                {
                    Folder_Status.Text = FileLocation.Directory;
                }
            }

            private void On_Output_Change(object sender, EventArgs e)
            {
                if (PreviewDisplay != null) PreviewDisplay.SetColorScale(colorcontrol.Scale, new double[2] { (double)colorcontrol.Min, (double)colorcontrol.Max });
            }

            private async void onPreview_Click(object sender, EventArgs e)
            {
                if (T != null && T.ThreadState == System.Threading.ThreadState.Running)
                {
                    Time_Preview.Enabled = true;
                    Loop.Text = "Loop";
                    stop = true;
                }

                Loop.Enabled = false;
                ForwButton.Enabled = false;
                BackButton.Enabled = false;
                PachydermAc_PlugIn plugin = PachydermAc_PlugIn.Instance;

                if (PreviewDisplay != null) PreviewDisplay.Enabled = false;

                max = (int)(Frame_Rate.Value * Seconds.Value);

                Hare.Geometry.Point[] SPT;

                if (!plugin.SourceOrigin(out SPT))
                {
                    Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                    return;
                }

                if (T != null && T.ThreadState == System.Threading.ThreadState.Running) ;
                PreviewDisplay = null;

                Pach_GetModel_Command Model = Pach_GetModel_Command.Instance;

                Model.Rel_Humidity = 50;
                Model.Air_Temp = (double)Air_Temp.Value;
                Model.Atm_pressure = 1000;
                Model.Atten_Choice = 0;
                Rhino.RhinoApp.RunScript("GetModel", false);

                plugin.Source(out Source);

                ParticleRays[] RTParticles = new ParticleRays[Source.Length];
                List<Hare.Geometry.Point> L = new List<Hare.Geometry.Point>();
                for (int i = 0; i < Source.Length; i++)
                {
                    L.Add(Source[i].Origin);
                }
                for (int j = 0; j < Source.Length; j++)
                {
                    if (plugin.Geometry_Spec == 0)
                    {
                        Model.Ret_NURBS_Scene.partition(L);
                        RTParticles[j] = new ParticleRays(Source[j], Model.Ret_NURBS_Scene, (int)RT_Count.Value, CutOffLength());
                        PreviewDisplay = new WaveConduit(RTParticles, colorcontrol.Scale, new double[2] { colorcontrol.Min, colorcontrol.Max }, Model.Ret_NURBS_Scene);
                    }
                    else
                    {
                        Model.Ret_Mesh_Scene.partition(L);
                        RTParticles[j] = new ParticleRays(Source[j], Model.Ret_Mesh_Scene, (int)RT_Count.Value, CutOffLength());
                        PreviewDisplay = new WaveConduit(RTParticles, colorcontrol.Scale, new double[2] { colorcontrol.Min, colorcontrol.Max }, Model.Ret_Mesh_Scene);
                    }

                    RTParticles[j].Begin();
                }

                ForwButton.Enabled = true;
                BackButton.Enabled = true;
                Loop.Enabled = true;
                stop = false;
                Loop_Click(null, null);
            }

            WaveConduit PreviewDisplay = null;
            System.Threading.Thread T;
            int j = 0;
            int max = 0;
            bool stop = false;
            private async Task LoopStart()
            {
                do
                {
                    await Task.Run(() => Forw_proc());
                    if (stop) break;
                    int t = 0;
                    Eto.Forms.Application.Instance.Invoke(() => { t = (int)(1000 / this.Frame_Rate.Value); });
                    await Task.Delay(t);
                }
                while (true);
            }

            private void ToStart_Click(object sender, System.EventArgs e)
            {
                j = 0;
                Forw_proc();
            }

            private void Rev_Click(object sender, System.EventArgs e)
            {
                Rev_proc();
            }

            private void Rev_proc()
            {
                j -= 1;
                if (j <= 0) j = max;

                Time_Preview.Text = (CO_TIME.Value * j / max).ToString();
                if (this.VisualizationSelect.SelectedIndex == 1)
                {
                    PreviewDisplay.Populate(j * (double)CO_TIME.Value * C_Sound() / (max * 1000), RCPachTools.HaretoRhinoMesh(((GeodesicMeshSource)Source[0]).T, true));
                }
                else
                {
                    PreviewDisplay.Populate(j * (double)CO_TIME.Value * C_Sound() / (max * 1000), VisualizationSelect.SelectedIndex == 0);
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private async void Forw_Click(object sender, System.EventArgs e)
            {
                Forw_proc();
            }

            private async Task Forw_proc()
            {
                j++;
                if (j >= max)
                {
                    j = 0;
                    return;
                }

                await Eto.Forms.Application.Instance.InvokeAsync(() =>
                {
                    if (this.VisualizationSelect.SelectedIndex == 1)
                    {
                        PreviewDisplay.Populate(j * (double)CO_TIME.Value * C_Sound() / (max * 1000), RCPachTools.HaretoRhinoMesh(((GeodesicMeshSource)Source[0]).T, true));
                    }
                    else
                    {
                        PreviewDisplay.Populate(j * (double)CO_TIME.Value * C_Sound() / (max * 1000), VisualizationSelect.SelectedIndex == 0);
                    }

                    /////////////////////////
                    if (Folder_Status.Text.Length > 0)
                    {
                        string number;
                        if (j < 100)
                        {
                            if (j < 10) number = "00" + j.ToString();
                            else number = "0" + j.ToString();
                        }
                        else number = j.ToString();

                        Rhino.RhinoApp.RunScript("-ViewCaptureToFile " + Folder_Status.Text + "\\"[0] + "frame" + number + ".jpg Width=1280 Height=720 DrawGrid=No Enter", true);
                    }
                    /////////////////////////
                    Time_Preview.Text = (CO_TIME.Value * j / max).ToString();
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                });
                return;
            }


            private void Loop_Click(object sender, EventArgs e)
            {
                if (Loop.Text == "Loop")
                {
                    stop = false;
                    Time_Preview.Enabled = false;
                    Loop.Text = "Pause";
                    Task.Run(LoopStart);
                }
                else
                {
                    stop = true;
                    Time_Preview.Enabled = true;
                    Loop.Text = "Loop";
                }
            }

            public void update(object sender, System.EventArgs e)
            {
                this.Width = this.Parent.Width;

                SimSetBox.Width = this.Width;
                TimeBox.Width = this.Width;
                OutputFolder.Width = this.Width;
                colorcontrol.Size = new Size(this.Width, Height);
                colorcontrol.Invalidate();
                Loop.Width = (int)((double)this.Width * 0.75);
                Invalidate(true);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                FileLocation.Dispose();
                BackButton.Dispose();
                FrRate_Label.Dispose();
                VisLabel.Dispose();
                SecLabel.Dispose();
                COTimeLabel.Dispose();
                RayctLabel.Dispose();
                ForwButton.Dispose();
                Loop.Dispose();
                Forw.Dispose();
                StartOver.Dispose();
                VisualizationSelect.Dispose();
                Frame_Rate.Dispose();
                Seconds.Dispose();
                CO_TIME.Dispose();
                AirTemp.Dispose();
                Air_Temp.Dispose();
                RT_Count.Dispose();
                //Forw.Dispose();
                Time_Preview.Dispose();
                Preview.Dispose();
                colorcontrol.Dispose();
                SelectOutput.Dispose();
                Folder_Status.Dispose();
                OpenFolder.Dispose();
                SimSetBox.Dispose();
                TimeBox.Dispose();
                OutputFolder.Dispose();
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

        public class ParticleRays
        {
            //private List<List<int>> Poly_ID = new List<List<int>>();
            private List<List<double>> Power = new List<List<double>>();
            private List<Polyline> RayList = new List<Polyline>();
            private List<Rhino.DocObjects.CurveObject> RhinoRays = new List<Rhino.DocObjects.CurveObject>();
            private double CutoffLength;
            private int RayCount;
            private Scene Room;
            private Source Source;
            private static System.Random Rnd = new System.Random();

            public ParticleRays(Source S, Scene R, int No_of_Rays, double CO_Length)
            {
                CutoffLength = CO_Length;
                RayCount = No_of_Rays;
                Room = R;
                Source = S;
            }

            public void Begin()
            {
                System.Random Rnd = new System.Random();

                for (int q = 0; q < RayCount; q++)
                {
                    Rhino.RhinoApp.SetCommandPrompt(string.Format("Finding Ray {0} of {1}", q+1, RayCount));
                    double SumLength = 0;
                    Pachyderm_Acoustic.Environment.OctaveRay R = Source.Directions(0, ref Rnd).SplitRay(4);
                    R.Intensity /= RayCount;
                    double u = 0;
                    double v = 0;
                    int ChosenIndex = 0;
                    Polyline Ray = new Polyline();
                    List<double> leg = new List<double> { 0 };
                    List<int> code = new List<int> { 0 };
                    List<Hare.Geometry.Point> Start;
                    //List<int> IDs = new List<int>();
                    Ray.Add(new Rhino.Geometry.Point3d(R.x, R.y, R.z));
                    List<double> P = new List<double> { R.Intensity };
                    do
                    {
                        R.Ray_ID = Rnd.Next();
                        if (Room.shoot(R, out u, out v, out ChosenIndex, out Start, out leg, out code))
                        {
                            double cos_theta;

                            for (int i = 0; i < Start.Count; i++)
                            {
                                Ray.Add(RCPachTools.HPttoRPt(Start[i]));
                                R.Intensity *= System.Math.Pow(10,-.1 * Room.Attenuation(code[i])[5] * leg[i]);
                                SumLength += leg[i];
                            }

                            R.x = Start[Start.Count - 1].x;
                            R.y = Start[Start.Count - 1].y;
                            R.z = Start[Start.Count - 1].z;
                            R.Surf_ID = ChosenIndex;
                            bool trans = (Rnd.NextDouble() < Room.TransmissionValue[ChosenIndex][5]);

                            Room.Absorb(ref R, out cos_theta, u, v);
                            Room.Scatter_Simple(ref R, ref Rnd, cos_theta, u, v);

                            if (trans)
                            {
                                R.Reverse();
                                R.Intensity *= Room.TransmissionValue[ChosenIndex][5];
                            }
                            else
                            {
                                R.Intensity *= (1 - Room.TransmissionValue[ChosenIndex][5]);
                            }

                            P.Add((double)R.Intensity);        
                        }
                        else
                        {
                            break;
                            BroadRayPool.Instance.release();
                        }
                    }
                    while (SumLength < CutoffLength);
                    BroadRayPool.Instance.release();

                    if (SumLength > CutoffLength) Ray.Add(new Rhino.Geometry.Point3d(R.x, R.y, R.z));
                    RayList.Add(Ray);
                    Power.Add(P);
                }
            }

            public int Count()
            {
                return RayList.Count;
            }

            public bool RayPt(int Index, double u, int oct, out double energy, out Point3d Next, out Point3d Result)
            {
                double S_Length = 0;
                for (int q = 0; q < RayList[Index].Count - 1; q++)
                {
                    double Modifier = RayList[Index][q].DistanceTo(RayList[Index][q + 1]);
                    S_Length += Modifier;
                    //Locate which segment it is on...) 
                    if (S_Length > u)
                    {
                        //energy *= Math.Pow(10,-.1 * Room.Attenuation[oct] * u) / (4 * Math.PI * u * u);
                        Point3d Point = RayList[Index].PointAt(q + ((u - (S_Length - Modifier)) / Modifier));
                        energy = Power[Index][q] * Math.Pow(10, -.1 * Room.Attenuation(RCPachTools.RPttoHPt(Point))[oct] * (u - S_Length - Modifier) / Modifier);//  / (4*Math.PI * u * u);
                        Next = RayList[Index][q + 1];
                        Result = Point;
                        return true;
                    }
                    if (q >= RayList[Index].SegmentCount) break;
                    //energy *= Room.ReflectionValue[Poly_ID[Index][q]][oct];
                }
                energy = 0;
                Next = default(Point3d);
                Result = default(Point3d);
                return false;
            }

            public Rhino.DocObjects.CurveObject Ray(int Index)
            {
                return RhinoRays[Index];
            }            
        }
    }
}