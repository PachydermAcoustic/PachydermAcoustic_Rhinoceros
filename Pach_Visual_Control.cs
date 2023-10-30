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
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Visualization;
using Pachyderm_Acoustic.Utilities;
using System.Runtime.InteropServices;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("EA23F0D6-5462-4e42-9CFC-DC8E79723112")]
        public partial class Pach_Visual_Control
        {
            Source[] Source;

            // This call is required by the Windows Form Designer. 
            public Pach_Visual_Control()
            {
                InitializeComponent();
                scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);
                Param_Scale.Image = scale.PIC;
            }

            private void Calculate_Click(object sender, System.EventArgs e)
            {
                Pach_GetModel_Command Model = new Pach_GetModel_Command();
                Source[] Source;

                if (PachydermAc_PlugIn.Instance.Source(out Source) && !object.ReferenceEquals(FileLocation.SelectedPath, "")) Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                ParticleRays[] RTParticles = new ParticleRays[Source.Length];

                Calculate.Enabled = false;
                PachydermAc_PlugIn plugin = PachydermAc_PlugIn.Instance;
                Scene Sc;
                if (PachydermAc_PlugIn.Instance.Geometry_Spec() == 0) Sc = RC_PachTools.Get_NURBS_Scene(0, (double)Air_Temp.Value, 0, 0, false);
                else Sc = RC_PachTools.Get_Poly_Scene(0, false, (double)Air_Temp.Value, 0, 0, false);
                
                for (int i = 0; i < Source.Length; i++)
                {
                    if (Source != null)
                    {
                        List<Hare.Geometry.Point> L = new List<Hare.Geometry.Point>();
                        for (int j = 0; j < Source.Length; j++)
                        {
                            L.Add(Source[j].Origin());
                        }

                        if (plugin.Geometry_Spec() == 0)
                        {
                            Sc.partition(L, 15);
                            RTParticles[i] = new ParticleRays(Source[i], Sc, (int)RT_Count.Value, CutOffLength());
                        }
                        else if (plugin.Geometry_Spec() == 1)
                        {
                            Sc.partition(L, 15);
                            RTParticles[i] = new ParticleRays(Source[i], Sc, (int)RT_Count.Value, CutOffLength());
                        }
                        RTParticles[i].Begin();
                    }
                    else
                    {
                        Rhino.RhinoApp.WriteLine("Model geometry not specified... Exiting calculation...");
                    }

                    Pachyderm_Acoustic.Visualization.Phonon P;
                    if (ParticleChoice.SelectedIndex == 0)
                    { P = new Tetrahedron(); }
                    else if (ParticleChoice.SelectedIndex == 1)
                    {
                        P = new Icosahedron();
                    }
                    else { P = new Geodesic_sphere(); }
                    RenderParticles(RTParticles, (double)(Frame_Rate.Value * Seconds.Value), P);
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
                }
                Calculate.Enabled = true;
            }

            private double CutOffLength()
            {
                return ((double)CO_TIME.Value / 1000) * C_Sound();
            }

            private double C_Sound()
            {
                return AcousticalMath.SoundSpeed((double)Air_Temp.Value);
            }

            private FolderBrowserDialog FileLocation = new FolderBrowserDialog();
            private void OpenFolder_Click(object sender, System.EventArgs e)
            {
                if (FileLocation.ShowDialog() == DialogResult.OK)
                {
                    Folder_Status.Text = FileLocation.SelectedPath;
                }
            }

            private void RenderParticles(ParticleRays[] Rays, double total_Frames, Pachyderm_Acoustic.Visualization.Phonon P)
            {
                List<Guid> Particle_IDS = new List<Guid>();
                double Increment = CutOffLength() / (double)(Frame_Rate.Value * Seconds.Value);
                Point3d Point = default(Point3d);
                double PMin = (double)this.Param_Min.Value;
                double PMax = (double)this.Param_Max.Value;
                double PRange = PMax - PMin;

                int h = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.Bounds.Height;
                int w = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.Bounds.Width;
                
                for (int q = 0; q < (int)total_Frames; q++)
                {
                    for (int i = 0; i < Rays.Length; i++)
                    {
                        for (int p = 0; p < Rays[i].Count(); p++)
                        {
                            Point3d N;
                            double energy;
                            if (Rays[i].RayPt(p, Increment * q, 4, out energy, out N, out Point))
                            {
                                Vector3d V = new Vector3d(N - Point);
                                V.Unitize();
                                double SPL = AcousticalMath.SPL_Intensity(energy);
                                System.Drawing.Color C = scale.GetValue(SPL, PMin, PMax);
                                Mesh M = P.Generate(Point, V);
                                M.Scale((SPL - PMin) / PRange);
                                System.Guid PG = Rhino.RhinoDoc.ActiveDoc.Objects.AddMesh(M);
                                Rhino.RhinoDoc.ActiveDoc.Objects.Find(PG).Attributes.ObjectColor = C;
                                Particle_IDS.Add(PG);
                            }
                        }
                    }

                    string number;
                    if (q < 100)
                    {
                        if (q < 10) number = "00" + q.ToString();
                        else number = "0" + q.ToString();
                    }
                    else number = q.ToString();
                    Rhino.RhinoApp.RunScript("-ViewCaptureToFile \"" + Folder_Status.Text + "\\"[0] + "frame" + number + string.Format(".jpg\" Width={0} Height={1} DrawGrid=No Enter", 2*w, 2*h), true);

                    //Clean Up Model
                    Rhino.RhinoDoc.ActiveDoc.Objects.Delete(Particle_IDS, true);
                    Particle_IDS.Clear();
                }
            }

            private void Preview_Click(object sender, EventArgs e)
            {
                if (T != null && T.ThreadState == System.Threading.ThreadState.Running)
                {
                    Time_Preview.Enabled = true;
                    Loop.Text = "Loop";
                    T.Abort();
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

                if (T != null && T.ThreadState == System.Threading.ThreadState.Running) T.Abort();
                PreviewDisplay = null;

                Pach_GetModel_Command Model = Pach_GetModel_Command.Instance;
                if (object.ReferenceEquals(RoomSelection.SelectedItem, "Use Entire Model"))
                {
                    Model.Rel_Humidity = 50;//(double)Rel_Humidity.Value;
                    Model.Air_Temp = (double)Air_Temp.Value;
                    Model.Atm_pressure = 1000;//(double)Air_Pressure.Value;
                    Model.Atten_Choice = 0;//Atten_Method.SelectedIndex;
                    Rhino.RhinoApp.RunScript("GetModel", false);
                }

                plugin.Source(out Source);

                if (MeshWave)
                {
                    for (int i = 0; i < Source.Length; i++) Source[i] = new GeodesicMeshSource(plugin.GetSourceSWL(i), Source[i].Origin(), (int)RT_Count.Value, i);
                }

                ParticleRays[] RTParticles = new ParticleRays[Source.Length];
                List<Hare.Geometry.Point> L = new List<Hare.Geometry.Point>();
                for (int i = 0; i < Source.Length; i++)
                {
                    L.Add(Source[i].Origin());
                }
                for (int j = 0; j < Source.Length; j++)
                {
                    if (plugin.Geometry_Spec() == 0)
                    {
                        Model.Ret_NURBS_Scene.partition(L);
                        RTParticles[j] = new ParticleRays(Source[j], Model.Ret_NURBS_Scene, (int)RT_Count.Value, CutOffLength());
                        PreviewDisplay = new WaveConduit(RTParticles, scale, new double[2] { (double)Param_Min.Value, (double)Param_Max.Value }, Model.Ret_NURBS_Scene);
                    }
                    else
                    {
                        Model.Ret_Mesh_Scene.partition(L);
                        RTParticles[j] = new ParticleRays(Source[j], Model.Ret_Mesh_Scene, (int)RT_Count.Value, CutOffLength());
                        PreviewDisplay = new WaveConduit(RTParticles, scale, new double[2] { (double)Param_Min.Value, (double)Param_Max.Value }, Model.Ret_Mesh_Scene);
                    }

                    RTParticles[j].Begin();
                }

                ForwButton.Enabled = true;
                BackButton.Enabled = true;
                Loop.Enabled = true;
                Loop.Text = "Stop";
                FC = new ForCall(Forw_proc);
                System.Threading.ParameterizedThreadStart St = new System.Threading.ParameterizedThreadStart(delegate { LoopStart((int)(this.Frame_Rate.Value * Seconds.Value)); });
                T = new System.Threading.Thread(St);
                T.Start();
            }

            WaveConduit PreviewDisplay = null;
            System.Threading.Thread T;
            int j = 0;
            int max = 0;
            private void LoopStart(object obj)
            {
                do
                {
                    FC();
                    System.Threading.Thread.Sleep(100);
                }
                while (true);
            }

            private delegate void ForCall();
            ForCall FC;

            private void Rev_Click(object sender, EventArgs e)
            {
                Rev_proc();
            }

            private void Rev_proc()
            {
                j -= 1;
                if (j <= 0) j = max;
                this.Invoke((MethodInvoker)delegate
                {
                    Time_Preview.Text = (CO_TIME.Value * j / max).ToString();
                });
                PreviewDisplay.Populate((double)CO_TIME.Value * C_Sound() / (max * 1000), SmartParticles);
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Forw_Click(object sender, EventArgs e)
            {
                Forw_proc();
            }

            private void Forw_proc()
            {
                j++;
                if (j >= max)
                {
                    j = 0;
                    return;
                }
                if (MeshWave)
                {
                    PreviewDisplay.Populate(j * (double)CO_TIME.Value * C_Sound() / (max * 1000), RC_PachTools.Hare_to_RhinoMesh(((GeodesicMeshSource)Source[0]).T, true));
                }
                else
                {
                    PreviewDisplay.Populate(j * (double)CO_TIME.Value * C_Sound() / (max * 1000), SmartParticles);
                }
                ////////////////////////
                if (Folder_Status.Text != "")
                {
                    string number;
                    if (j < 100)
                    {
                        if (j < 10) number = "00" + j.ToString();
                        else number = "0" + j.ToString();
                    }
                    else number = j.ToString();

                    this.Invoke((MethodInvoker)delegate { Rhino.RhinoApp.RunScript("-ViewCaptureToFile " + Folder_Status.Text + "\\"[0] + "frame" + number + ".jpg Width=1280 Height=720 DrawGrid=No Enter", true); });
                }
                /////////////////////////
                this.Invoke((MethodInvoker)delegate { Time_Preview.Text = (CO_TIME.Value * j / max).ToString(); });
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Loop_Click(object sender, EventArgs e)
            {
                if (Loop.Text == "Loop")
                {
                    Time_Preview.Enabled = false;
                    Loop.Text = "Pause";
                    System.Threading.ParameterizedThreadStart St = new System.Threading.ParameterizedThreadStart(delegate { LoopStart(this.Frame_Rate.Value * Seconds.Value); });
                    T = new System.Threading.Thread(St);
                    T.Start();
                }
                else
                {
                    Time_Preview.Enabled = true;
                    Loop.Text = "Loop";
                    T.Abort();
                }
            }

            Pach_Graphics.colorscale scale;

            private void Color_Selection_SelectedIndexChanged(object sender, EventArgs e)
            {
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
                if (PreviewDisplay != null) PreviewDisplay.SetColorScale(scale, new double[] { (double)Param_Min.Value, (double)Param_Max.Value });
            }

            private void Param_Max_ValueChanged(object sender, EventArgs e)
            {
                //if (T.ThreadState == System.Threading.ThreadState.Running) T.Suspend();
                this.Param3_4.Text = (Param_Max.Value - (Param_Max.Value - Param_Min.Value) / 4).ToString();
                this.Param1_2.Text = (Param_Max.Value - (Param_Max.Value - Param_Min.Value) / 2).ToString();
                this.Param1_4.Text = (Param_Min.Value + (Param_Max.Value - Param_Min.Value) / 4).ToString();

                if (PreviewDisplay != null) PreviewDisplay.SetColorScale(scale, new double[2] { (double)Param_Min.Value, (double)Param_Max.Value });
                //T.Resume();
            }

            bool MeshWave = false;
            bool SmartParticles = true;

            private void SourceSelect_SelectedIndexChanged(object sender, EventArgs e)
            {
                MeshWave = (SourceSelection.Text == "Mesh Wave");
                SmartParticles = (SourceSelection.Text == "Smart Particle Wave");
            }
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
            private static Random Rnd = new Random();

            public ParticleRays(Source S, Scene R, int No_of_Rays, double CO_Length)
            {
                CutoffLength = CO_Length;
                RayCount = No_of_Rays;
                Room = R;
                Source = S;
            }

            public void Begin()
            {
                Random Rnd = new Random();

                for (int q = 0; q < RayCount; q++)
                {
                    Rhino.RhinoApp.SetCommandPrompt(string.Format("Finding Ray {0} of {1}", q+1, RayCount));
                    double SumLength = 0;
                    Pachyderm_Acoustic.Environment.OctaveRay R = Source.Directions(0, ref Rnd).SplitRay(4);
                    double u = 0;
                    double v = 0;
                    int ChosenIndex = 0;
                    Polyline Ray = new Polyline();
                    List<double> leg = new List<double> { 0 };
                    List<int> code = new List<int> { 0 };
                    List<Hare.Geometry.Point> Start;
                    //List<int> IDs = new List<int>();
                    Ray.Add(RC_PachTools.HPttoRPt(R.origin));
                    List<double> P = new List<double> { R.Intensity };
                    do
                    {
                        R.Ray_ID = Rnd.Next();
                        if (Room.shoot(R, out u, out v, out ChosenIndex, out Start, out leg, out code))
                        {
                            double cos_theta;

                            for (int i = 0; i < Start.Count; i++)
                            {
                                Ray.Add(RC_PachTools.HPttoRPt(Start[i]));
                                //IDs.Add(-1);
                                R.Intensity *= Math.Pow(10,-.1 * Room.Attenuation(code[i])[5] * leg[i]);
                                SumLength += leg[i];
                            }

                            R.origin = Start[Start.Count - 1];
                            R.Surf_ID = ChosenIndex;
                            bool trans = (Rnd.NextDouble() < Room.TransmissionValue[ChosenIndex][5]);

                            Room.Absorb(ref R, out cos_theta, u, v);
                            Room.Scatter_Simple(ref R, ref Rnd, cos_theta, u, v);

                            if (trans)
                            {
                                R.direction *= -1;
                                R.Intensity *= Room.TransmissionValue[ChosenIndex][5];
                            }
                            else
                            {
                                R.Intensity *= (1 - Room.TransmissionValue[ChosenIndex][5]);
                            }

                            P.Add((double)R.Intensity);
                                
                            //if (Rnd.NextDouble() < Room.ScatteringValue[ChosenIndex].Coefficient(5))
                            //{
                            //    Room.Scatter_Simple(ref R, ref Rnd);
                            //    //Utilities.PachTools.Ray_Acoustics.LambertianReflection_Stoch(ref R.direction, ref Rnd, Room.Normal(ChosenIndex, u, v));

                            //    if (trans)
                            //    {
                            //        R.direction *= -1;
                            //        R.Intensity *= Room.TransmissionValue[ChosenIndex][5];
                            //    }
                            //    else
                            //    {
                            //        R.Intensity *= Room.AbsorptionValue[ChosenIndex].Coefficient_A_Broad()[5];
                            //    }

                            //}
                            //else
                            //{
                            //    if (!trans)
                            //    {
                            //        Utilities.PachTools.Ray_Acoustics.SpecularReflection(ref R.direction, ref Room, ref u, ref v, ref ChosenIndex);
                            //        R.Intensity *= Room.AbsorptionValue[ChosenIndex].Coefficient_A_Broad()[5];
                            //    }
                            //    else 
                            //    {
                            //        R.Intensity *= Room.TransmissionValue[ChosenIndex][5];
                            //    }
                            //}
                        }
                        else
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(PachTools.HPttoRPt(R.origin), PachTools.HPttoRPt(R.origin + R.direction));
                            break;
                        }
                    }
                    while (SumLength < CutoffLength);
                    //Poly_ID.Add(IDs);
                    if (SumLength > CutoffLength) Ray.Add(RC_PachTools.HPttoRPt(R.origin));
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
                ///TODO: Use new protocol here.

                //double energy = 1.0;
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
                        energy = Power[Index][q] * Math.Pow(10, -.1 * Room.Attenuation(RC_PachTools.RPttoHPt(Point))[oct] * (u - S_Length - Modifier) / Modifier);//  / (4*Math.PI * u * u);
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