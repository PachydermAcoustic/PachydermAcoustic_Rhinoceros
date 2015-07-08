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
using System;
using System.Collections.Generic;
using Hare.Geometry;
using Pachyderm_Acoustic.Environment;
using System.Linq;

namespace Pachyderm_Acoustic
{
    public class ImageSourceData:Simulation_Type
    {
        private List<Deterministic_Reflection>[]ValidPaths;
        private List<Deterministic_Reflection>[,] ThreadPaths;
        private double Speed_of_Sound;
        private int MaxOrder;
        private Source Src;
        private Hare.Geometry.Point[] Rec;
        private Polygon_Scene Room;
        private int[] CurrentSrf;
        private int[] CurrentEdge;
        private DateTime ST;
        private Random[] Rnd;
        private double[] Direct_Time;
        System.Threading.Thread[] T_List;
        int processorCT;
        int elementCt;
        private TimeSpan TS;
        private int SampleCT;
        private int SampleRate;
        public int SrcNo;
        public int[] Oct_choice;
        public bool Diffraction = false;
        public bool IncludeEdges = false;

        private ImageSourceData()
        {
        }

        public ImageSourceData(Source Source, Receiver_Bank Receiver, Direct_Sound Direct, Polygon_Scene Rm, int MaxOrder_in, int SourceID_in)
        :this(Source, Receiver, Direct, Rm, new int[2] { 0, 7 }, MaxOrder_in, false, SourceID_in)
        { }

        public ImageSourceData(Source Source, Receiver_Bank Receiver, Direct_Sound Direct, Polygon_Scene Rm, int MaxOrder_in, bool ED, int SourceID_in)
            : this(Source, Receiver, Direct, Rm, new int[2] { 0, 7 }, MaxOrder_in, ED, SourceID_in)
        { }

        /// <summary>
        /// Constructor prepares image source calculation to run.
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Receiver"></param>
        /// <param name="Direct"></param>
        /// <param name="Rm"></param>
        /// <param name="MaxOrder_in">The maximum order to be calculated for.</param>
        public ImageSourceData(Source Source, Receiver_Bank Receiver, Direct_Sound Direct, Polygon_Scene Rm, int[] Octaves, int MaxOrder_in, bool Edge_Diffraction, int SourceID_in)
        {
            IncludeEdges = Edge_Diffraction;
            Diffraction = Edge_Diffraction;
            Oct_choice = new int[Octaves[1] - Octaves[0] + 1];
            for (int i = 0; i < Octaves.Length; i++) Oct_choice[i] = i + Octaves[0];
            SrcNo = SourceID_in;
            ValidPaths = new List<Deterministic_Reflection>[Receiver.Count];
            Speed_of_Sound = Rm.Sound_speed(0);
            MaxOrder = MaxOrder_in;
            Src = Source;
            Rec = new Hare.Geometry.Point[Receiver.Count];
            SampleCT = Receiver.SampleCT;
            SampleRate = Receiver.SampleRate;
            for(int i = 0; i < Receiver.Count; i++)
            {
                Rec[i] = Receiver.H_Origin(i);
            }
            Room = Rm;
            Direct_Time = new double[Receiver.Count];
            for (int q = 0; q < Receiver.Count; q++)
            {
                Direct_Time[q] = Direct.Min_Time(q);
            }
        }

        /// <summary>
        /// Called by Pach_RunSim_Command. Indicates whether or not the simulation has completed.
        /// </summary>
        /// <returns>Returns running if any threads in this simulation are still running. Returns stopped if all have stopped.</returns>
        public override System.Threading.ThreadState ThreadState()
        {
            foreach (System.Threading.Thread T in T_List)
            {
                if (T.ThreadState == System.Threading.ThreadState.Running) return System.Threading.ThreadState.Running;
            }
            return System.Threading.ThreadState.Stopped;
        }

        /// <summary>
        /// Inherited member. Divides the simulation into threads, and begins.
        /// </summary>
        public override void Begin()
        {
            processorCT = UI.PachydermAc_PlugIn.Instance.ProcessorSpec();
            ThreadPaths = new List<Deterministic_Reflection>[Rec.Length, processorCT];
            int SrfCT = Room.PlaneCount;
            CurrentSrf = new int[processorCT];
            CurrentEdge = new int[processorCT];
            Rnd = new Random[processorCT];
            T_List = new System.Threading.Thread[processorCT];
            elementCt = Room.PlaneCount + ((this.Diffraction) ? Room.EdgeCount : 0);

            for (int P_I = 0; P_I < processorCT; P_I++)
            {
                for (int i = 0; i < Rec.Length; i++)
                {
                    ThreadPaths[i,P_I] = new List<Deterministic_Reflection>();
                    ValidPaths[i] = new List<Deterministic_Reflection>();
                }
                int start = (int)Math.Floor((double)P_I * SrfCT / processorCT);
                int end;
                if (P_I == processorCT - 1) end = SrfCT;
                else end = (P_I + 1) * SrfCT / processorCT;
                
                Calc_Params T = new Calc_Params(start, end, P_I, Room.R_Seed.Next());
                System.Threading.ParameterizedThreadStart TS = new System.Threading.ParameterizedThreadStart(delegate { Calculate(T); });
                T_List[P_I] = new System.Threading.Thread(TS);
                T_List[P_I].Start();
            }
        }

        /// <summary>
        /// Called by each thread from the begin method.
        /// </summary>
        /// <param name="i">the object is type "Calc_Params" which holds all the necessary information to run a portion of the simulation.</param>
        public void Calculate(object i)
        {
            Calc_Params Params = (Calc_Params) i;
            Rnd[Params.ThreadID] = new Random(Params.RandomSeed);
            ST = DateTime.Now;
            int[] Sequence = new int[1];
            for (CurrentSrf[Params.ThreadID] = 0; CurrentSrf[Params.ThreadID] < Params.EndIndex - Params.StartIndex; CurrentSrf[Params.ThreadID]++)
            {
                List<Hare.Geometry.Point[]> Images = new List<Hare.Geometry.Point[]> {new Hare.Geometry.Point[1]};
                Sequence[0] = CurrentSrf[Params.ThreadID] + Params.StartIndex;
                Images[0][0] = Room.Image(Src.H_Origin(), 0, Room.PlaneMembers[Sequence[0]][0]);
                //Process all valid first order reflections...
                ProcessImages(Images.ToArray(), Sequence, Params.ThreadID);
                //for (int j = 0; j < Rec.Length; j++) ProcessPath(Sequence, Params.ThreadID, j);
                //Process higher order reflections (if needed)
                if (Diffraction)
                {
                    LateOrdersED(1, Sequence, Params.ThreadID, Images);
                }
                else
                {   
                    LateOrders(1, Sequence, Params.ThreadID, Images[0]);
                }
            }
            
            if (!Diffraction) return;

            //User has chosen to process edge reflections as well.
            int EdgeStart = (Room.EdgeCount * Params.ThreadID / (processorCT));
            int EdgeEnd = (Room.EdgeCount * (Params.ThreadID + 1) / (processorCT));
            //int FS = 88200;

            for (CurrentEdge[Params.ThreadID] = EdgeStart; CurrentEdge[Params.ThreadID] < EdgeEnd; CurrentEdge[Params.ThreadID]++)
            {
                //Process all valid first order edge reflections...
                //Sequence[0] = CurrentEdge[Params.ThreadID] + EdgeStart;
                //List<double> P, T;
                //List<Vector> D;
                //List<Hare.Geometry.Point> Pt;
                //List<Hare.Geometry.Point> Src = new List<Hare.Geometry.Point>();
                Sequence[0] = CurrentEdge[Params.ThreadID] + Room.PlaneCount;
                List<Hare.Geometry.Point[]> Images = new List<Hare.Geometry.Point[]>();
                
                for (int j = 0; j < Room.Edge_Nodes[CurrentEdge[Params.ThreadID]].EdgeSources.Count; j++)
                {
                    Images.Add(new Hare.Geometry.Point[1]{Room.Edge_Nodes[CurrentEdge[Params.ThreadID]].EdgeSources[j].Z_mid});
                }

                //Process all valid first order reflections...
                ProcessImages(Images.ToArray(), Sequence, Params.ThreadID);
                //Process higher order reflections (if needed)
                LateOrdersED(1, Sequence, Params.ThreadID, Images);
            }
        }

        /// <summary>
        /// Used by specular raytracer.
        /// </summary>
        /// <param name="Sequences">List of surface index sequences to try.</param>
        public void Lookup_Sequences(List<int[]>[] Sequences)
        {
            processorCT = UI.PachydermAc_PlugIn.Instance.ProcessorSpec();
            ThreadPaths = new List<Deterministic_Reflection>[Rec.Length,processorCT];
            int SrfCT = Room.PlaneCount;
            CurrentSrf = new int[processorCT];
            Random R = new Random();
            T_List = new System.Threading.Thread[processorCT];
            for (int p = 0; p < Rec.Length; p++)
            {
                for (int P_I = 0; P_I < processorCT; P_I++)
                {
                    ThreadPaths[p, P_I] = new List<Deterministic_Reflection>();
                    Calc_Params T = new Calc_Params(P_I * Sequences[p].Count / processorCT, (P_I + 1) * Sequences[p].Count / processorCT, P_I, R.Next());
                    System.Threading.ParameterizedThreadStart TS = new System.Threading.ParameterizedThreadStart(delegate
                    {
                        for (int i = T.StartIndex; i < T.EndIndex; i++)
                        {
                            ProcessPath(Sequences[p][i], T.ThreadID, p);
                        }
                    });
                    T_List[P_I] = new System.Threading.Thread(TS);
                    T_List[P_I].Start();
                }
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    if (ThreadState() != System.Threading.ThreadState.Running) break;
                } while (true);

                for (int t = 0; t < processorCT; t++)
                {
                    ValidPaths[p].AddRange(ThreadPaths[p, t]);
                }
            }
        }

        /// <summary>
        /// A string to identify the type of simulation being run.
        /// </summary>
        /// <returns></returns>
        public override string Sim_Type()
        {
            return "Image Source";
        }

        /// <summary>
        /// Called by Pach_RunSim_Command. Get a string describing the status of the simulation for display.
        /// </summary>
        /// <returns></returns>
        public override string ProgressMsg()
        {
            TS = DateTime.Now - ST;
            int Srf_CT = 0;

            for (int i = 0; i < processorCT; i++) Srf_CT += CurrentSrf[i];

            if (Diffraction)
            {
                int Edge_CT = 0;
                for (int i = 0; i < processorCT; i++) Edge_CT += CurrentEdge[i];
                
                TS = new TimeSpan((long)(TS.Ticks * (((double)(Room.PlaneCount - Srf_CT) + (Room.EdgeCount - Edge_CT)) / (Srf_CT + Edge_CT + 2))));
                return string.Format("Calculating Image/Edge Set {0} of {1}. ({2} hours,{3} minutes,{4} seconds Remaining.) Press 'Esc' to Cancel...", Srf_CT + Edge_CT, Room.PlaneCount + Room.EdgeCount, TS.Hours, TS.Minutes, TS.Seconds);
            }
            else
            {
                TS = new TimeSpan((long)(TS.Ticks * (((double)(Room.PlaneCount - Srf_CT)) / (Srf_CT + 1))));
                return string.Format("Calculating Image Set {0} of {1}. ({2} hours,{3} minutes,{4} seconds Remaining.) Press 'Esc' to Cancel...", Srf_CT, Room.PlaneCount, TS.Hours, TS.Minutes, TS.Seconds);
            }
        }

        /// <summary>
        /// Aborts all threads, effectively ending the simulation.
        /// </summary>
        public override void  Abort_Calculation()
        {
            foreach(System.Threading.Thread T in T_List) T.Abort();
        }

        /// <summary>
        /// Consolidates output from all threads into a single set of output.
        /// </summary>
        public override void Combine_ThreadLocal_Results()
        {
            for (int i = 0; i < Rec.Length; i++)
            {
                for (int p = 0; p < processorCT; p++)
                {
                    ValidPaths[i].AddRange(ThreadPaths[i, p]);
                }
            }
        }

        /// <summary>
        /// Recursive function which carries out image source method for orders greater than 1.
        /// </summary>
        /// <param name="LastOrder">The order of the calling iteration.</param>
        /// <param name="Sequence">The input sequence, to which the next surface will be appended.</param>
        /// <param name="ThreadId">The id of the thread using the function.</param>
        /// <param name="Images">the list of source images, to which the next image will be appended.</param>
        private void LateOrders(int LastOrder, int[] Sequence, int ThreadId, Hare.Geometry.Point[] Images)
        {
            int CurrentOrder = LastOrder + 1;
            Array.Resize(ref Sequence, Sequence.Length + 1);
            Array.Resize(ref Images, Images.Length + 1);
            bool GoNext = CurrentOrder < MaxOrder;
            for (int q = 0; q < elementCt; q++)
            {
                if (q != Sequence[Sequence.Length - 2] && CurrentOrder <= MaxOrder)
                {
                    Sequence[Sequence.Length - 1] = q;
                    Images[Images.Length - 1] = Room.Image(Images[Images.Length - 2], 0, Room.PlaneMembers[q][0]);
                    ProcessImages(new Hare.Geometry.Point[1][]{Images}, Sequence, ThreadId);
                    if (GoNext)
                    {
                        LateOrders(CurrentOrder, Sequence, ThreadId, Images);
                    }
                }
            }
        }

        /// <summary>
        /// Recursive function which carries out image source/edge source method for orders greater than 1.
        /// </summary>
        /// <param name="LastOrder">The order of the calling iteration.</param>
        /// <param name="Sequence">The input sequence, to which the next surface will be appended.</param>
        /// <param name="ThreadId">The id of the thread using the function.</param>
        /// <param name="Images">the list of source images, to which the next image will be appended.</param>
        private void LateOrdersED(int LastOrder, int[] Sequence, int ThreadId, List<Hare.Geometry.Point[]> Images0)
        {
            int CurrentOrder = LastOrder + 1;
            Array.Resize(ref Sequence, CurrentOrder);//Sequence.Length + 1);          
            bool GoNext = CurrentOrder < MaxOrder;
            for (int q = 0; q < elementCt; q++)
            {
                List<Hare.Geometry.Point[]> Images = new List<Hare.Geometry.Point[]>();
                if (q != Sequence[Sequence.Length - 2] && CurrentOrder <= MaxOrder)
                {
                    //int Current = Images0.Count;
                    Sequence[LastOrder] = q;    
                    for(int r = 0; r < Images0.Count; r++)
                    {
                        Hare.Geometry.Point[] im = Images0[r].Clone() as Hare.Geometry.Point[];
                        if (Sequence[q] > Room.PlaneCount - 1)
                        {
                            int edge_id = Sequence[q] - Room.PlaneCount;
                            //Its an edge...
                            //Hare.Geometry.Point[] im = Images0[r].Clone() as Hare.Geometry.Point[];
                            for(int i = 0; i < Room.Edge_Nodes[edge_id].EdgeSources.Count; i++)
                            {
                                im[LastOrder] = Room.Edge_Nodes[edge_id].EdgeSources[i].Z_mid;
                                Images.Add(im.Clone() as Hare.Geometry.Point[]);
                            }
                            //Room.Edge_Nodes[edge_id].EdgeSources
                        }
                        else
                        {
                            //Hare.Geometry.Point[] im = Images0[r].Clone() as Hare.Geometry.Point[];
                            im[LastOrder] = Room.Image(Images[r][Images[r].Length - 2], 0, Room.PlaneMembers[q][0]);
                            Images.Add(im);
                            //Images[r][Images[r].Length - 1] = Room.Image(Images[r][Images[r].Length - 2], 0, Room.PlaneMembers[q][0]);
                        }
                        ProcessImages(Images.ToArray(), Sequence, ThreadId);
                        if (GoNext)
                        {
                            LateOrdersED(CurrentOrder, Sequence, ThreadId, Images);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function calculates the actual path of the specular reflection.
        /// </summary>
        /// <param name="Images">The list of images.</param>
        /// <param name="Sequence">The list of surface indices for reflection.</param>
        /// <param name="Threadid">The id of the calling thread.</param>
        private void ProcessImages(Hare.Geometry.Point[][] Images, int[] Sequence, int Threadid)
        {
            for (int rec_id = 0; rec_id < Rec.Length; rec_id++)
            {
                double c_sound =  Room.Sound_speed(Rec[rec_id]);

                double[][] Trans_Mod = new double[Images.Length][];
                int[] Seq_Polys = new int[Sequence.Length];
                List<Hare.Geometry.Point[]> PathVertices = new List<Hare.Geometry.Point[]>();
                Hare.Geometry.Point S = Src.H_Origin();
                Hare.Geometry.Point E = Rec[rec_id];
                double df = SampleRate * .5 / 4096;

                //Find all Path Legs from Receiver to Source
                
                for (int r = 0; r < Images.Length; r++)
                {
                    Trans_Mod[r] = new double[8];
                    for (int t_oct = 0; t_oct < 8; t_oct++) Trans_Mod[r][t_oct] = 1;
                
                    Hare.Geometry.Point[] path = new Hare.Geometry.Point[Sequence.Length + 2];
                    path[0] = S;
                    path[path.Length - 1] = E;

                    for (int q = Sequence.Length - 1; q >= 0; q--)
                    {
                        if (Sequence[q] > Room.PlaneCount - 1)
                        {
                            //It's an edge!
                            int EdgeID = Sequence[q] - Room.PlaneCount;
                            //for (int i = 1; i < Room.Edge_Nodes[EdgeID].EdgeSources.Count; i++)
                            if (!OcclusionIntersectED(path[q + 2], Images[r][q], Sequence[q], ref Trans_Mod[r], ref path[q + 1], Threadid))// ref Trans_Mod, , ref Seq_Polys[q], 
                            {
                                path = null;
                                break;
                            }
                        }
                        else
                        {
                            if (!OcclusionIntersect(path[q + 2], Images[r][q], Sequence[q], ref Trans_Mod[r], ref path[q + 1], ref Seq_Polys[q], Threadid))
                            {
                                path = null;
                                break;
                            }
                        }
                    }
                    PathVertices.Add(path);
                }

                //Check that any path was unoccluded... if so, then record this entry. If not, move on...
                if (PathVertices.Count(item => item != null) == 0) continue; //goto Next;
                
                //Final Occlusion Check:
                for (int r = 0; r < PathVertices.Count; r++)
                {
                    if (PathVertices[r] == null) continue;
                    if (Sequence[0] < Room.PlaneCount)
                    {
                        if (FinalOcclusion(PathVertices[r][0], PathVertices[r][1], Sequence[0], ref Trans_Mod[r], Threadid))
                            PathVertices[r] = null;
                    }
                    else
                    {
                        int edge_id = Sequence[0] - Room.PlaneCount;
                        if (FinalOcclusion(PathVertices[r][0], PathVertices[r][1], 0.00001, Room.Edge_Nodes[edge_id].ParentBreps[0], Room.Edge_Nodes[edge_id].ParentBreps[1], ref Trans_Mod[r], Threadid))
                            PathVertices[r] = null;
                    }
                }

                //Check again for null(occluded) paths...
                if (PathVertices.Count(item => item != null) == 0) continue; //goto Next;

                ///Process all paths for pulse entry...
                if (PathVertices.Count == 0) continue;//goto Next;

                if (PathVertices.Count == 1)
                {
                    ThreadPaths[rec_id, Threadid].Add(new Specular_Path(PathVertices[0], Sequence, Seq_Polys, Room, Src, Speed_of_Sound, Trans_Mod[0], ref Direct_Time[rec_id], Threadid, Rnd[Threadid].Next()));
                    continue;
                }

                //Process Compound Path before storing it.
                double[] H = new double[0];
                Environment.Material[] M = new Environment.Material[Sequence.Length];
                for (int i = 0; i < M.Length; i++) M[i] = (Sequence[i] < Room.PlaneCount) ? Room.Surface_Material(Sequence[i]) : null;

                //Arrange all information to build filtered response...
                List<List<double>> Times = new List<List<double>>();
                List<List<double>> Pr = new List<List<double>>();
                List<double> Time = new List<double>();
                List<double> Bs = new List<double>();
                List<double> X = new List<double>();
                List<double> Y = new List<double>();
                List<double> Z = new List<double>();
                List<List<double>> Xe = new List<List<double>>();
                List<List<double>> Ye = new List<List<double>>();
                List<List<double>> Ze = new List<List<double>>();
                List<double> X_ = new List<double>();
                List<double> Y_ = new List<double>();
                List<double> Z_ = new List<double>();
                List<List<double>> Xs = new List<List<double>>();
                List<List<double>> Ys = new List<List<double>>();
                List<List<double>> Zs = new List<List<double>>();

                double deltaS = 0;
                double dt = 1.0f / SampleRate;
                List<double[]> t_limits = new List<double[]>();
                    
                    for (int i = 0; i < PathVertices.Count; i++)
                    {
                        if (PathVertices[i] == null)
                        {
                            if (Bs.Count > 0)
                            {
                                t_limits.Add(new double[2] { Time.Min(), Time.Max() });
                                Pr.Add(Bs);
                                Bs = new List<double>();
                                Times.Add(Time);
                                Time = new List<double>();
                                Xe.Add(X);
                                X = new List<double>();
                                Ye.Add(Y);
                                Y = new List<double>();
                                Ze.Add(Z);
                                Z = new List<double>();
                                Xs.Add(X_);
                                X_ = new List<double>();
                                Ys.Add(Y_);
                                Y_ = new List<double>();
                                Zs.Add(Z_);
                                Z_ = new List<double>();
                            }
                            continue;
                        }
                        double l = 0;
                        double[] dm = null, dl = null;
                        double length1 = 0, length2 = 0;
                        double Pres = 1;
                        int s, c, e;
                        for (s = 0, c = 1, e = 2; e < PathVertices[i].Length; s++, c++, e++)
                        {
                            if (Sequence[s] < Room.PlaneCount)
                            {
                                length1 += (PathVertices[i][1] - PathVertices[i][c]).Length();
                                length2 = length1;
                                dl = new double[2] { (PathVertices[i][c] - PathVertices[i][e]).Length(), (PathVertices[i][c] - PathVertices[i][e]).Length() };
                            }
                            else if (Sequence[s] >= Room.PlaneCount)
                            {
                                double m = 0;
                                double B = Room.Edge_Nodes[Sequence[s] - Room.PlaneCount].EdgeSources[i].Flex_Solve(PathVertices[i][s], PathVertices[i][e], ref m, ref l, ref dm, ref dl);
                                Pres *= B;
                                length1 += dm[0];
                                length2 += dm[1];
                            }
                            else { throw new NotImplementedException("...well isn't that novel..."); }
                        }
                        length1 += dl[0];
                        length2 += dl[1];

                        double duration_s = SampleRate * Math.Abs(length2 - length1) / c_sound;

                        Vector DIR;
                        DIR = PathVertices[i][c-1] - PathVertices[i][e-1];
                        DIR.Normalize();
                        Pres /= duration_s;
                        double Tn = 0.5 * (length1 + length2) / c_sound;
                        if (Time.Count > 2)
                        {
                            double dtnew = Time[Time.Count - 2] - Tn;
                            if (deltaS != 0 && (dtnew > 0) != (deltaS > 0))
                            {
                                //Break it...
                                t_limits.Add(new double[2] { Time.Min(), Time.Max() });
                                if (Bs.Last() < Pres) { t_limits[t_limits.Count - 1][0] += dt; }
                                else { t_limits[t_limits.Count - 1][1] -= dt; }
                                Pr.Add(Bs);
                                Bs = new List<double>();
                                Times.Add(Time);
                                Time = new List<double>();
                                Xe.Add(X);
                                X = new List<double>();
                                Ye.Add(Y);
                                Y = new List<double>();
                                Ze.Add(Z);
                                Z = new List<double>();
                                Xs.Add(X_);
                                X_ = new List<double>();
                                Ys.Add(Y_);
                                Y_ = new List<double>();
                                Zs.Add(Z_);
                                Z_ = new List<double>();
                                ///Ensure Continuity...
                                //Time.Add(Times[Times.Count - 1].Last());
                                //Bs.Add(Pr[Pr.Count - 1].Last());
                                //X.Add(Xe[Xe.Count - 1].Last());
                                //Y.Add(Ye[Ye.Count - 1].Last());
                                //Z.Add(Ze[Ze.Count - 1].Last());
                                //X_.Add(Xs[Xs.Count - 1].Last());
                                //Y_.Add(Ys[Ys.Count - 1].Last());
                                //Z_.Add(Zs[Zs.Count - 1].Last());
                                //dtnew *= -1;
                            }
                            deltaS = dtnew;
                        }
                        Vector DIRs = PathVertices[i][1] - PathVertices[i][0];
                        DIRs.Normalize();
                        X_.Add(DIRs.x);
                        Y_.Add(DIRs.y);
                        Z_.Add(DIRs.z);

                        Bs.Add(Pres);
                        Time.Add(Tn);
                        X.Add(DIR.x);
                        Y.Add(DIR.y);
                        Z.Add(DIR.z);
                    }

                    if (Bs.Count > 0)
                    {
                        t_limits.Add(new double[2] { Time.Min(), Time.Max() });
                        Pr.Add(Bs);
                        Times.Add(Time);
                        Xe.Add(X);
                        Ye.Add(Y);
                        Ze.Add(Z);
                        Xs.Add(X_);
                        Ys.Add(Y_);
                        Zs.Add(Z_);
                    }
                    MathNet.Numerics.Interpolation.CubicSpline[] Pr_Spline = new MathNet.Numerics.Interpolation.CubicSpline[Pr.Count];
                    MathNet.Numerics.Interpolation.CubicSpline[] X_Spline = new MathNet.Numerics.Interpolation.CubicSpline[Xe.Count];
                    MathNet.Numerics.Interpolation.CubicSpline[] Y_Spline = new MathNet.Numerics.Interpolation.CubicSpline[Ye.Count];
                    MathNet.Numerics.Interpolation.CubicSpline[] Z_Spline = new MathNet.Numerics.Interpolation.CubicSpline[Ze.Count];
                    MathNet.Numerics.Interpolation.CubicSpline[] Xs_Spline = new MathNet.Numerics.Interpolation.CubicSpline[Xs.Count];
                    MathNet.Numerics.Interpolation.CubicSpline[] Ys_Spline = new MathNet.Numerics.Interpolation.CubicSpline[Ys.Count];
                    MathNet.Numerics.Interpolation.CubicSpline[] Zs_Spline = new MathNet.Numerics.Interpolation.CubicSpline[Zs.Count];
                    double min = double.PositiveInfinity;
                    for (int i = 0; i < Times.Count; i++)
                    {
                        Pr_Spline[i] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(Times[i], Pr[i]);
                        X_Spline[i] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(Times[i], Xe[i]);
                        Y_Spline[i] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(Times[i], Ye[i]);
                        Z_Spline[i] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(Times[i], Ze[i]);
                        Xs_Spline[i] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(Times[i], Xs[i]);
                        Ys_Spline[i] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(Times[i], Ys[i]);
                        Zs_Spline[i] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(Times[i], Zs[i]);

                        min = Math.Min(min, t_limits[i][1]);
                    }

                    //TODO: Find a way to include absorption/transmission effects... which will not affect the entire multipath reflection...
                    Dictionary<int, double> H_d = new Dictionary<int, double>();
                    Dictionary<int, double>[] H_directional = new Dictionary<int, double>[6];
                    for (int i = 0; i < 6; i++) H_directional[i] = new Dictionary<int, double>(); 

                    for (int i = 0; i < t_limits.Count; i++)
                    {
                        for (double t = t_limits[i][0]; t < t_limits[i][1]; t += dt)
                        {
                            Vector dir = new Vector(Xs_Spline[i].Interpolate(t), Ys_Spline[i].Interpolate(t), Zs_Spline[i].Interpolate(t));

                            ////Compose Impulse Response...
                            //double T_current = (lengths[p_id] + dl[0]) / Room.Sound_speed(new Hare.Geometry.Point(0, 0, 0));
                            //double T_duration = (length2 + dl[1]) / Room.Sound_speed(new Hare.Geometry.Point(0, 0, 0)) - T_current;

                            ///Apply TransMod to TF...
                            ///Each sample will have it's own unique air attenuation and occlusion conditions, which means that it needs to be treated individually for input signal (for air attenuation and absorption).
                            //double[] TF = Audio.Pach_SP.Magnitude_Filter(new double[8] { Math.Sqrt(Trans_Mod[p_id][0] * SW[0]), Math.Sqrt(Trans_Mod[p_id][1] * SW[1]), Math.Sqrt(Trans_Mod[p_id][2] * SW[2]), Math.Sqrt(Trans_Mod[p_id][3] * SW[3]), Math.Sqrt(Trans_Mod[p_id][4] * SW[4]), Math.Sqrt(Trans_Mod[p_id][5] * SW[5]), Math.Sqrt(Trans_Mod[p_id][6] * SW[6]), Math.Sqrt(Trans_Mod[p_id][7] * SW[7]) }, 44100, 4096, Threadid);
                            double[] SW = Src.DirPower(Threadid, this.Rnd[Threadid].Next(), dir);
                            foreach (Environment.Material m in M)
                            {
                                if (m is Environment.Basic_Material) for (int oct = 0; oct < 8; oct++)
                                    {
                                        SW[oct] *= 1 - m.Coefficient_A_Broad(oct);
                                        //SW[oct] *= Trans_Mod[j][oct];
                                    }
                            }

                            System.Numerics.Complex[] TF = Audio.Pach_SP.Filter.Spectrum(new double[8] { Math.Sqrt(SW[0]), Math.Sqrt(SW[1]), Math.Sqrt(SW[2]), Math.Sqrt(SW[3]), Math.Sqrt(SW[4]), Math.Sqrt(SW[5]), Math.Sqrt(SW[6]), Math.Sqrt(SW[7]) }, 44100, 4096, Threadid);
                        //double[] ms = Audio.Pach_SP.Magnitude_Spectrum(new double[8] { Math.Sqrt(SW[0]), Math.Sqrt(SW[1]), Math.Sqrt(SW[2]), Math.Sqrt(SW[3]), Math.Sqrt(SW[4]), Math.Sqrt(SW[5]), Math.Sqrt(SW[6]), Math.Sqrt(SW[7]) }, 44100, 4096, Threadid);
                        //double[] ms = Audio.Pach_SP.Magnitude_Spectrum(new double[8] { Math.Sqrt(SW[0]), Math.Sqrt(SW[1]), Math.Sqrt(SW[2]), Math.Sqrt(SW[3]), Math.Sqrt(SW[4]), Math.Sqrt(SW[5]), Math.Sqrt(SW[6]), Math.Sqrt(SW[7]) }, 88200, 8096, Threadid);

                        //System.Numerics.Complex[] TF = new System.Numerics.Complex[ms.Length];
                        //    for (int j = 0; j < TF.Length; j++) TF[j] = ms[j];
                                //Array.Resize(ref TF, TF.Length / 2);

                            ///Apply Air attenuation to TF...
                            double[] atten = new double[0];
                            double[] freq = new double[0];
                            Room.AttenuationFilter(4096, 44100, t * c_sound, ref freq, ref atten, Rec[rec_id]);//sig is the magnitude response of the air attenuation filter...
                            //Room.AttenuationFilter(4096, 88200, t * c_sound, ref freq, ref atten, Rec[rec_id]);//sig is the magnitude response of the air attenuation filter...
                            for (int j = 0; j < TF.Length; j++) TF[j] *= atten[j];
                            
                            for (int j = 0; j < M.Length; j++)
                            {
                                if (!(M[j] is Environment.Basic_Material)) 
                                {
                                    System.Numerics.Complex[] spec = M[j].Reflection_Spectrum(44100, 4096/2, Room.Normal(Sequence[j]), PathVertices[i][j] - PathVertices[i][j - 1], Threadid);
                                    for (int k = 0; k < TF.Length; k++) TF[k] *= spec[k];
                                }
                            }

                            //Audio.Pach_SP.Filter.Response(TF, SampleRate, Threadid);

                            double[] pulse = Audio.Pach_SP.IFFT_Real4096(Audio.Pach_SP.Mirror_Spectrum(TF), Threadid);
                            //double[] pulse = new double[prepulse.Length];
                            Audio.Pach_SP.Scale(ref pulse);
                            //for (int j = 0; j < prepulse.Length; j++)
                            //{
                            //    pulse[j] = prepulse[(j + prepulse.Length/2) % prepulse.Length];
                            //}
                            ////////////////////////////////////////////////////////
                            //Pachyderm_Acoustic.Audio.Pach_SP.resample(ref pulse);
                            ////////////////////////////////////////////////////////
                            //Audio.Pach_SP.Raised_Cosine_Window(ref pulse);
                            //Manual convolution of each distinct contribution of edge...
                            int index = (int)Math.Floor(t * SampleRate);
                            double omni_pr = Pr_Spline[i].Interpolate(t);
                            dir = new Vector(X_Spline[i].Interpolate(t), Y_Spline[i].Interpolate(t), Z_Spline[i].Interpolate(t));
                            double[] dir_c = new double[6];
                            if (dir.x > 0) dir_c[0] = dir.x; else dir_c[1] = -dir.x;
                            if (dir.y > 0) dir_c[2] = dir.y; else dir_c[3] = -dir.y;
                            if (dir.z > 0) dir_c[4] = dir.z; else dir_c[5] = -dir.z;

                            for (int j = 0; j < pulse.Length; j++)
                            {
                                //Todo: confirm that pulse comes in at right times...
                                int t_c = index + j;
                                double p_t = omni_pr * pulse[j] / 4096;

                                if (!H_d.Keys.Contains<int>(index + j))
                                {
                                    H_d.Add(t_c, p_t);
                                    H_directional[0].Add(t_c, p_t * dir_c[0]);
                                    H_directional[1].Add(t_c, p_t * dir_c[1]);
                                    H_directional[2].Add(t_c, p_t * dir_c[2]);
                                    H_directional[3].Add(t_c, p_t * dir_c[3]);
                                    H_directional[4].Add(t_c, p_t * dir_c[4]);
                                    H_directional[5].Add(t_c, p_t * dir_c[5]);
                                }
                                else
                                {
                                    H_d[t_c] += (float)(p_t);
                                    H_directional[0][t_c] += p_t * dir_c[0];
                                    H_directional[1][t_c] += p_t * dir_c[1];
                                    H_directional[2][t_c] += p_t * dir_c[2];
                                    H_directional[3][t_c] += p_t * dir_c[3];
                                    H_directional[4][t_c] += p_t * dir_c[4];
                                    H_directional[5][t_c] += p_t * dir_c[5];
                                }
                            }
                        }
                    }
                    int minsample = H_d.Keys.Min();
                    int maxsample = H_d.Keys.Max();
                    double T0 = (double)minsample / SampleRate;
                    H = new double[maxsample - minsample];
                    double[][] Hdir = new double[6][];
                    for(int j = 0; j < 6; j++) Hdir[j] = new double[maxsample - minsample];
                    for (int j = minsample; j < maxsample; j++) if (H_d.Keys.Contains<int>(j))
                        {
                            H[j - minsample] = H_d[j];
                            Hdir[0][j - minsample] = H_directional[0][j];
                            Hdir[1][j - minsample] = H_directional[1][j];
                            Hdir[2][j - minsample] = H_directional[2][j];
                            Hdir[3][j - minsample] = H_directional[3][j];
                            Hdir[4][j - minsample] = H_directional[4][j];
                            Hdir[5][j - minsample] = H_directional[5][j];
                        }
                    ///Enter the reflection
                    PathVertices.RemoveAll(item => item == null);
                    ThreadPaths[rec_id, Threadid].Add(new Compound_Path(PathVertices.ToArray(), Sequence, Src.Source_ID(), H, Hdir, T0, Speed_of_Sound, ref Direct_Time[rec_id], Threadid));
            }
        }

        /// <summary>
        /// This function calculates the actual path of the specular reflection.
        /// </summary>
        /// <param name="Images">The list of images.</param>
        /// <param name="Sequence">The list of surface indices for reflection.</param>
        /// <param name="Threadid">The id of the calling thread.</param>
        private void ProcessImages(Hare.Geometry.Point[] Images, int[] Sequence, int rec_id, int Threadid)
        {
            double[] Trans_Mod = new double[8];
            int[] Seq_Polys = new int[Sequence.Length];
            for (int t_oct = 0; t_oct < 8; t_oct++) Trans_Mod[t_oct] = 1;
            Hare.Geometry.Point[] PathVertices = new Hare.Geometry.Point[Sequence.Length + 2];
            PathVertices[0] = Src.H_Origin();
            PathVertices[PathVertices.Length - 1] = Rec[rec_id];

            //Find all Path Legs from Receiver to Source
            for (int q = Sequence.Length; q > 0; q--) if (!OcclusionIntersect(PathVertices[q + 1], Images[q - 1], Sequence[q - 1], ref Trans_Mod, ref PathVertices[q], ref Seq_Polys[q - 1], Threadid)) return;

            //Final Occlusion Check:
            if (FinalOcclusion(PathVertices[0], PathVertices[1], Sequence[0], ref Trans_Mod, Threadid)) return;
            Specular_Path SP = new Specular_Path(PathVertices, Sequence, Seq_Polys, Room, Src, Speed_of_Sound, Trans_Mod, ref Direct_Time[rec_id], Threadid, Rnd[Threadid].Next());
            ThreadPaths[rec_id, Threadid].Add(SP);
        }

        /// <summary>
        /// Processes image source paths with input of only a sequence of indices.
        /// </summary>
        /// <param name="Sequence">The input sequence of surface indices.</param>
        /// <param name="Threadid">The id of the calling thread.</param>
        /// <param name="rec_id">The id of the receiver.</param>
        /// <returns>True if a valid path, false if not.</returns>
        private bool ProcessPath(int[] Sequence, int Threadid, int rec_id)
        {
            Hare.Geometry.Point RefPoint;
            Hare.Geometry.Point NextPoint = new Hare.Geometry.Point();
            Hare.Geometry.Point[] Images = new Hare.Geometry.Point[Sequence.Length];
            RefPoint = Src.H_Origin();
            int[] Seq_Polys = new int[Sequence.Length];
            double[] Trans_Mod = new double[8];
            for (int t_oct = 0; t_oct < 8; t_oct++) Trans_Mod[t_oct] = 1;

            //Find all Source Images
            for (int q = 0; q < Sequence.Length; q++)
            {
                //RefPM = Room.PlaneMembers[Sequence[q]];
                Images[q] = Room.Image(RefPoint, 0, Room.PlaneMembers[Sequence[q]][0]);
                RefPoint = Images[q];
            }

            ProcessImages(Images, Sequence, rec_id, Threadid);

            //Hare.Geometry.Point[] PathVertices = new Hare.Geometry.Point[Sequence.Length + 2];
            //PathVertices[0] = Src.H_Origin();
            //PathVertices[PathVertices.Length - 1] = Rec[rec_id];
            //RefPoint = Rec[rec_id];
            ////Find all Path Legs from End to Start
            //int Limit = Sequence.Length - 1;
            //for (int q = Limit; q >= 0; q--)
            //{
            //    if (!OcclusionIntersect(PathVertices[q + 2], Images[q], Sequence[q], ref Trans_Mod, ref RefPoint, ref Seq_Polys[q], Threadid)) 
            //        return false;
            //    PathVertices[q + 1] = RefPoint;
            //}

            ////Final Occlusion Check:
            //if (!FinalOcclusion(PathVertices[0], PathVertices[1], Sequence[0], ref Trans_Mod, ref RefPoint, Threadid)) 
            //    return false;
            //Specular_Path SP = new Specular_Path(PathVertices, Sequence, Seq_Polys, Oct_choice, Room, Src, Speed_of_Sound, Trans_Mod, ref Direct_Time[rec_id], Threadid, Rnd[Threadid].Next());

            //ThreadPaths[rec_id, Threadid].Add(SP);
            return true;
        }

        /// <summary>
        /// Checks a path for occlusions in the model.
        /// </summary>
        /// <param name="Src"></param>
        /// <param name="EndPt"></param>
        /// <param name="Poly_X"></param>
        /// <param name="X_Point"></param>
        /// <param name="Thread_Id">The id of the calling thread.</param>
        /// <returns></returns>
        private bool FinalOcclusion(Hare.Geometry.Point Src, Hare.Geometry.Point EndPt, int Poly_X, ref double[] Trans_Mod, int Thread_Id)
        {
            Hare.Geometry.Vector D = (Src - EndPt);
            double L = D.Length();
            double L2 = 0;
            D.Normalize();
            Ray R = new Ray(EndPt, D, Thread_Id, Rnd[Thread_Id].Next());

            Hare.Geometry.X_Event X;
            do
            {
                if (Room.shoot(R, 0, out X) && Room.IsTransmissive[X.Poly_id] && Poly_X != Room.PlaneID(X.Poly_id) && X.t < 0.001)
                {
                    ///The ray hit something transparent. Account for this...
                    double[] Absorption = Room.AbsorptionValue[X.Poly_id].Coefficient_A_Broad();
                    for (int oct = 0; oct < 8; oct++) Trans_Mod[oct] *= (1-Absorption[oct]) * Room.TransmissionValue[X.Poly_id][oct];
                    R.origin = X.X_Point;
                    R.Ray_ID = Rnd[Thread_Id].Next();
                    L2 += X.t;
                    continue;
                }
                break;
            } while (true);

            ///If it hits nothing, then there is nothing occluding... IsOccluded = false
            if (!X.Hit) return false;

            ///If the thing it hit is closer than the source, then it is occluded... IsOccluded = true
            L2 += X.t;
            if (L2 < L) return true;

            ///If we got this far, then there is nothing occluding... 
            return false;
        }

        //Edge Diffraction Version of the Final Occlusion Check.
        private bool FinalOcclusion(Hare.Geometry.Point Src, Hare.Geometry.Point EndPt, double tol, int Poly_1, int Poly_2, ref double[] Trans_Mod, int Thread_Id)
        {
            Hare.Geometry.Vector D = (Src - EndPt);
            double L = D.Length();
            double L2 = 0;
            D.Normalize();
            Ray R = new Ray(EndPt, D, Thread_Id, Rnd[Thread_Id].Next());

            Hare.Geometry.X_Event X;
            do
            {
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Change this so that it checks for the edge the BREPS the edge is on when x.t is less than tolerance.
                ////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (Room.shoot(R, 0, out X) && (Room.IsTransmissive[X.Poly_id] || (X.t < tol && (Room.BrepID(X.Poly_id) == Poly_1 || Room.BrepID(X.Poly_id) == Poly_2))))//(Poly_2 != Room.PlaneID(X.Poly_id) || Poly_1 != Room.PlaneID(X.Poly_id)))
                {
                    ///The ray hit something transparent. Account for this...
                    double[] Absorption = Room.AbsorptionValue[X.Poly_id].Coefficient_A_Broad();
                    for (int oct = 0; oct < 8; oct++) Trans_Mod[oct] *= (1 - Absorption[oct]) * Room.TransmissionValue[X.Poly_id][oct];
                    R.origin = X.X_Point;
                    R.Ray_ID = Rnd[Thread_Id].Next();
                    L2 += X.t;
                    continue;
                }
                break;
            } while (true);

            ///If it hits nothing, then there is nothing occluding... IsOccluded = false
            if (!X.Hit) return false;

            ///If the thing it hit is closer than the source, then it is occluded... IsOccluded = true
            L2 += X.t;
            if (L2 < L) return true;

            ///If we got this far, then there is nothing occluding... 
            return false;
        }

        /// <summary>
        /// Checks a path for occlusions in the model.
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="EndPt"></param>
        /// <param name="Poly_X"></param>
        /// <param name="X_Point"></param>
        /// <param name="Thread_Id">The id of the calling thread.</param>
        /// <returns></returns>
        private bool OcclusionIntersect(Hare.Geometry.Point Origin, Hare.Geometry.Point EndPt, int Poly_X, ref double[] Trans_Mod, ref Hare.Geometry.Point X_Point, ref int Poly_Seq, int Thread_Id)
        {   
            Hare.Geometry.Vector D = (EndPt - Origin);
            D.Normalize();
            Ray R = new Ray(Origin, D, Thread_Id, Rnd[Thread_Id].Next());

            Hare.Geometry.X_Event X = new Hare.Geometry.X_Event();
            do
            {
                if (Room.shoot(R, 0, out X) && Room.IsTransmissive[X.Poly_id] && Poly_X != Room.PlaneID(X.Poly_id))
                {
                    double[] Absorption = Room.AbsorptionValue[X.Poly_id].Coefficient_A_Broad();
                    for (int oct = 0; oct < 8; oct++) Trans_Mod[oct] *= (1 - Absorption[oct]) * Room.TransmissionValue[X.Poly_id][oct];
                    R.origin = X.X_Point;
                    R.Ray_ID = Rnd[Thread_Id].Next();
                    continue;
                }
                break;
            } while (true);

            if (X.t < 0.000001)
                return false;
            if (Poly_X != Room.PlaneID(X.Poly_id))
            {
                //Guid G1 = Rhino.RhinoDoc.ActiveDoc.Objects.Add(new Rhino.Geometry.LineCurve(Utilities.PachTools.HPttoRPt(X.X_Point), Utilities.PachTools.HPttoRPt(EndPt)));
                //Guid G2 = Rhino.RhinoDoc.ActiveDoc.Objects.Add(new TextDot(Room.PlaneID(X.Poly_id).ToString(), Utilities.PachTools.HPttoRPt(EndPt)));
                //Rhino.RhinoDoc.ActiveDoc.Groups.Add(new Guid[2] { G1, G2 });
                return false;
            }
            Poly_Seq = X.Poly_id;
            X_Point = X.X_Point;
            return true;
        }

        /// <summary>
        /// Checks a path for occlusions in the model.
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="EndPt"></param>
        /// <param name="Poly_X"></param>
        /// <param name="X_Point"></param>
        /// <param name="Thread_Id">The id of the calling thread.</param>
        /// <returns></returns>
        private bool OcclusionIntersectED(Hare.Geometry.Point Origin, Hare.Geometry.Point EndPt, int Poly_X, ref double[] Trans_Mod, ref Hare.Geometry.Point X_Point, int Thread_Id)//ref double[] Trans_Mod
        {
            Hare.Geometry.Vector D = (EndPt - Origin);
            double L = D.Length();
            D /= L;
            Ray R = new Ray(Origin, D, Thread_Id, Rnd[Thread_Id].Next());
            Hare.Geometry.X_Event X = new Hare.Geometry.X_Event();

            do
            {
                if (Room.shoot(R, 0, out X) && Room.IsTransmissive[X.Poly_id])// && Poly_X != Room.PlaneID(X.Poly_id))
                {
                    double[] Absorption = Room.AbsorptionValue[X.Poly_id].Coefficient_A_Broad();
                    for (int oct = 0; oct < 8; oct++) Trans_Mod[oct] *= (1 - Absorption[oct]) * Room.TransmissionValue[X.Poly_id][oct];
                    R.origin = X.X_Point;
                    R.Ray_ID = Rnd[Thread_Id].Next();
                    continue;
                }
                break;
            } while (true);

            if (X.t < L - 0.0001 && X.t != 0) return false;
            //if (Poly_X != Room.PlaneID(X.Poly_id))
            //{
            //    //Guid G1 = Rhino.RhinoDoc.ActiveDoc.Objects.Add(new Rhino.Geometry.LineCurve(Utilities.PachTools.HPttoRPt(X.X_Point), Utilities.PachTools.HPttoRPt(EndPt)));
            //    //Guid G2 = Rhino.RhinoDoc.ActiveDoc.Objects.Add(new TextDot(Room.PlaneID(X.Poly_id).ToString(), Utilities.PachTools.HPttoRPt(EndPt)));
            //    //Rhino.RhinoDoc.ActiveDoc.Groups.Add(new Guid[2] { G1, G2 });
            //    return false;
            //}
            //Poly_Seq = X.Poly_id;
            //X_Point = X.X_Point;
            X_Point = EndPt;
            return true;
        }

        /// <summary>
        /// Write the image source paths to a file for storage.
        /// </summary>
        /// <param name="BW">The binary writer to which data will be written.</param>
        public void Write_Data(ref System.IO.BinaryWriter BW)
        {
            //1. Write an indicator to signify that there is image source data
            BW.Write("Image-Source_Data");

            for (int q = 0; q < ValidPaths.Length; q++)
            {
                //2. Write the receiver number:int
                BW.Write(q);
                //3. Write number of paths:int
                BW.Write(ValidPaths[q].Count);
                //Write for all specular paths...
                //TODO: Sort for Specular Paths, filter out Compound Paths.
                for (int i = 0; i < ValidPaths[q].Count; i++)
                {
                    
                    //V2 procedure: Introducing pressure based simulation...
                    //3a.1 Write an enumeration for the kind of reflection it is(0 for specular, 1 for compound). (int)
                    if (ValidPaths[q][i] is Specular_Path) 
                    {
                        ///Specular Path:
                        BW.Write((short)0);

                        //4. Write the number of reflection path points
                        BW.Write(ValidPaths[q][i].Path[0].Length);
                        //5. Write the reflection path:double
                        for (int r = 0; r < ValidPaths[q][i].Path[0].Length; r++)
                        {
                            BW.Write(ValidPaths[q][i].Path[0][r].x);
                            BW.Write(ValidPaths[q][i].Path[0][r].y);
                            BW.Write(ValidPaths[q][i].Path[0][r].z);
                        }
                        
                        //6a.1 Write the energy levels
                        BW.Write(ValidPaths[q][i].Energy(0)[0]);
                        BW.Write(ValidPaths[q][i].Energy(1)[0]);
                        BW.Write(ValidPaths[q][i].Energy(2)[0]);
                        BW.Write(ValidPaths[q][i].Energy(3)[0]);
                        BW.Write(ValidPaths[q][i].Energy(4)[0]);
                        BW.Write(ValidPaths[q][i].Energy(5)[0]);
                        BW.Write(ValidPaths[q][i].Energy(6)[0]);
                        BW.Write(ValidPaths[q][i].Energy(7)[0]);

                        //6a.2 Write a bool for whether it has a special materials filter...
                        if ((ValidPaths[q][i] as Specular_Path).Special_Filter != null)
                        {
                            BW.Write(true);
                            BW.Write((ValidPaths[q][i] as Specular_Path).Special_Filter.Length);
                            foreach(System.Numerics.Complex val in (ValidPaths[q][i] as Specular_Path).Special_Filter)
                            {
                                BW.Write(val.Real);
                                BW.Write(val.Imaginary);
                            }
                        }
                        else
                        {
                            BW.Write(false);
                        }
                        double[] prms = (ValidPaths[q][i] as Specular_Path).prms;
                        for (int j = 0; j < prms.Length; j++) BW.Write(prms[j]);
                    }
                    else if (ValidPaths[q][i] is Compound_Path)
                    {
                        ////TODO: Find a robust format for compound reflection paths...
                        /////Compound Path:
                        //BW.Write((short)1);
                        ////Write the number of samples and the pressure signal down:
                        //BW.Write((ValidPaths[q][i] as Compound_Path).P.Length);
                        //foreach (double val in (ValidPaths[q][i] as Compound_Path).P)
                        //{
                        //    BW.Write(val);
                        //}
                        ////Write the number of samples and the pressure signal down:
                        //foreach(Hare.Geometry.Vector vct in (ValidPaths[q][i] as Compound_Path).Directions)
                        //{
                        //    BW.Write(vct.x);
                        //    BW.Write(vct.y);
                        //    BW.Write(vct.z);
                        //}
                    }

                    //7. Write the arrival time:double
                    BW.Write(ValidPaths[q][i].TravelTime);

                    //8. Write the Reflection Sequence:int
                    for (int r = 0; r < ValidPaths[q][i].Reflection_Sequence.Length; r++)
                    {
                        BW.Write(ValidPaths[q][i].Reflection_Sequence[r]);
                    }
                }
            }
        }

        /// <summary>
        /// Constructor which takes a Binary Reader at the appropriate point, from which calculated data will be extracted.
        /// </summary>
        /// <param name="BR"></param>
        /// <param name="Rec_CT"></param>
        /// <param name="Direct"></param>
        /// <returns></returns>
        public static ImageSourceData Read_Data(ref System.IO.BinaryReader BR, int Rec_CT, Direct_Sound Direct, bool Edges, int Src_ID, string version)
        {
            ImageSourceData IS = new ImageSourceData();
            IS.ValidPaths = new List<Deterministic_Reflection>[Rec_CT];
            IS.Direct_Time = new double[Rec_CT];
            double v = double.Parse(version.Substring(0, 3));

            for (int q = 0; q < Rec_CT; q++)
            {
                IS.ValidPaths[q] = new List<Deterministic_Reflection>();
                //2. Write the receiver number:int
                BR.ReadInt32();
                //3. Write number of paths:int
                int PathCt = BR.ReadInt32();
                for (int i = 0; i < PathCt; i++)
                {
                    int ReflectionType = BR.ReadInt16();
                    if (ReflectionType == 0)
                    {
                        //Speculare Reflection
                        //4. Write the number of reflection path points
                        Hare.Geometry.Point[] PTS = new Hare.Geometry.Point[BR.ReadInt32()];

                        //5. Write the reflection path:double
                        for (int r = 0; r < PTS.Length; r++)
                        {
                            PTS[r] = new Hare.Geometry.Point(BR.ReadDouble(), BR.ReadDouble(), BR.ReadDouble());
                        }

                        //Previously, Pachyderm performed the deterministic part in intensity only...                    
                        //6a. Write the energy values
                        double[] Energy = new double[8];
                        Energy[0] = BR.ReadDouble();
                        Energy[1] = BR.ReadDouble();
                        Energy[2] = BR.ReadDouble();
                        Energy[3] = BR.ReadDouble();
                        Energy[4] = BR.ReadDouble();
                        Energy[5] = BR.ReadDouble();
                        Energy[6] = BR.ReadDouble();
                        Energy[7] = BR.ReadDouble();

                        bool Special_Filter = BR.ReadBoolean();
                        System.Numerics.Complex[] Filter = null;
                   
                        if (Special_Filter)
                        {
                            //6aa1. Write length of filter...
                            int Filter_Length = BR.ReadInt32();
                            Filter = new System.Numerics.Complex[Filter_Length];
                            //6aa2. Write filter...
                            for(int j = 0; j < Filter.Length; j++)
                            {
                                Filter[j] = new System.Numerics.Complex(BR.ReadDouble(), BR.ReadDouble());
                            }
                            //6aa3. Write octave band root mean square pressure...
                        }

                        double[] prms = new double[8];
                        for (int j = 0; j < prms.Length; j++) prms[j] = BR.ReadDouble();

                        //7. Write the arrival time:double
                        double Time = BR.ReadDouble();

                        //8. Write the Reflection Sequence:int
                        int[] Sequence = new int[PTS.Length - 2];
                        for (int r = 0; r < Sequence.Length; r++)
                        {
                            Sequence[r] = BR.ReadInt32();
                        }

                        IS.ValidPaths[q].Add(new Specular_Path(PTS, Energy, prms, Filter, Time, Sequence, Direct.Min_Time(q), Src_ID));
                    }
                    else if (ReflectionType == 1)
                    {
                        //TODO: Find a robust format for compound reflection paths...
                        ///Specular Path:
                        //BW.Write((short)1);
                        //Write the number of samples and the pressure signal down:
                        //Write the number of samples and the pressure signal down:

                        //6a.2. Write the number of samples in the pressure signal.(int)
                        //6b. Write the pressure values
                    }

                }
            }
            return IS;
        }

        public void Create_Pressure(int samplingfrequency, int length)
        {
            for (int i = 0; i < Paths.Length; i++)
            {
                foreach (Deterministic_Reflection P in Paths[i])
                {
                    if (P is Specular_Path) (P as Specular_Path).Create_pressure(samplingfrequency, length, 0);
                }
            }
        }

        public Vector[] Dir_Energy(int rec_id, int index, int Octave, double alt, double azi, bool degrees)
        {
            return ValidPaths[rec_id][index].Dir_Energy(Octave, alt, azi, degrees);
        }

        public Vector[] Dir_Energy(int rec_id, int index, int Octave, Vector V)
        {
            return ValidPaths[rec_id][index].Dir_Energy(Octave, V);
        }

        /// <summary>
        /// returns the list of calculated paths.
        /// </summary>
        public List<Deterministic_Reflection>[] Paths
        {
            get { return ValidPaths; }
        }

        /// <summary>
        /// The number of image source paths for a given receiver.
        /// </summary>
        /// <param name="rec_id">The index of the chosen receiver.</param>
        /// <returns></returns>
        public int Count(int rec_id)
        {
            return ValidPaths[rec_id].Count;
        }

    }

    public abstract class Deterministic_Reflection
    {
        protected string Identifier;

        public override string ToString()
        {
            return Identifier;
        }

        public abstract Hare.Geometry.Point[][] Path{get;}

        public abstract double TravelTime { get;}

        public abstract int[] Reflection_Sequence { get; }

        public abstract double[] Energy(int Octave);
        public abstract Vector[] Dir_Energy(int Octave);
        public abstract Vector[] Dir_Energy(int Octave, Vector V);
        public abstract double[] Dir_Energy(int Octave, int dir);
        public abstract Vector[] Dir_Energy(int Octave, double alt, double azi, bool degrees);

        public abstract Vector[] Dir_EnergySum(Vector V);
        public abstract Vector[] Dir_EnergySum(double alt, double azi, bool degrees);

        public abstract double[] Pressure { get; }
        public abstract double[] Dir_Pressure(int Rec_ID, double alt, double azi, bool degrees, bool Figure8, int sampleFreq);
        public abstract double[][] Dir_Pressure(int Rec_ID, double alt, double azi, bool degrees, int sampleFreq);

        public abstract Polyline[] PolyLine { get; }
    }

    /// <summary>
    /// Class written to store the specular image source path.
    /// </summary>
    public class Specular_Path: Deterministic_Reflection
    {
        private Hare.Geometry.Point[] ValidPath;
        private double[] PathEnergy;
        private double Time;
        private double Length;
        private int[] Sequence;
        public double[] P;
        public double[] prms;//Octave band rms pressure.
        public System.Numerics.Complex[] Special_Filter;//Special circumstances filter (usually for detailed materials...)

        public Specular_Path(Hare.Geometry.Point[] Path, double[]Energy, double[] p, System.Numerics.Complex[] Filter, double T, int[] Seq, double Direct_Time, int SrcID)
        {
            ValidPath = Path;
            PathEnergy = Energy;
            prms = p;
            Special_Filter = Filter;
            Time = T;
            Sequence = Seq;
            Identify(SrcID, Direct_Time);

            Create_pressure(44100, 4096, 0);

            //System.Numerics.Complex[] Pspec = Audio.Pach_SP.Mirror_Spectrum(Audio.Pach_SP.Magnitude_Spectrum(prms, 44100, 4096, 0));
            ////System.Numerics.Complex[] Pspec = Audio.Pach_SP.Mirror_Spectrum(Audio.Pach_SP.Magnitude_Spectrum(prms, 88200, 4096, 0));

            //if (Special_Filter != null)
            //{
            //    for (int j = 0; j < Pspec.Length; j++) Pspec[j] *= Special_Filter[j];
            //}

            //double[] pre = Audio.Pach_SP.IFFT_Real4096(Pspec, 0);
            //P = new double[pre.Length];
            //double scale = Math.Sqrt(P.Length);
            //int hw = P.Length / 2;
            //for (int i = 0; i < pre.Length; i++) P[i] = pre[(i + hw) % pre.Length] / scale;

            //Audio.Pach_SP.resample(ref P);
            //Audio.Pach_SP.Raised_Cosine_Window(ref P);
        }

        public Specular_Path(Hare.Geometry.Point[] Path, int[] Seq_planes, int[] Seq_Polys, Scene Room, Source Src, double C_Sound, double[] Trans_Mod, ref double Direct_Time, int thread, int Rnd)
        {
            PathEnergy = new double[8];
            ValidPath = Path;
            //Build an Identifier
            Sequence = Seq_planes;

            Hare.Geometry.Point Pt;

            for (int q = 1; q < ValidPath.Length; q++)
            {
                Pt = ValidPath[q] - ValidPath[q - 1];
                Length += Math.Sqrt(Pt.x * Pt.x + Pt.y * Pt.y + Pt.z * Pt.z);
            }
            
            Time = Length / C_Sound + Src.Delay;
            Vector DIR = ValidPath[1] - ValidPath[0];
            DIR.Normalize();

            Random rnd = new Random(Rnd);
            float time = (float)(Length / C_Sound);
            double[] phase = Src.Phase(DIR, ref rnd);

            ///Energy based formulation
            double[] Power = Src.DirPower(thread, Rnd, DIR);
            Identify(Src.Source_ID(), Direct_Time);
            
            for (int oct = 0; oct < 8; oct++)
            {
                PathEnergy[oct] = (Power[oct] * Math.Pow(10,-.1 * Room.Attenuation(0)[oct] * Length) / (4 * Math.PI * Length * Length));
                PathEnergy[oct] *= Trans_Mod[oct];
            }

            foreach (int q in Seq_Polys)
            {
                if (!(Room.AbsorptionValue[q] is Basic_Material)) continue;
                double[] AbsorptionData = Room.AbsorptionValue[q].Coefficient_A_Broad();
                double[] ScatteringData = Room.ScatteringValue[q].Coefficient();
                for (int t = 0; t <= 7; t++)
                {
                    PathEnergy[t] *= (1 - AbsorptionData[t]) * (1 - ScatteringData[t]);
                }
            }

            prms = new double[8];

            for (int i = 0; i < 8; i++) prms[i] = Math.Sqrt(PathEnergy[i] * Room.Rho_C(Path[0]));

            //System.Numerics.Complex[] Pspec = Audio.Pach_SP.Mirror_Spectrum(Audio.Pach_SP.Magnitude_Spectrum(prms, 44100, 4096, thread));
            //System.Numerics.Complex[] Pspec = Audio.Pach_SP.Mirror_Spectrum(Audio.Pach_SP.Magnitude_Spectrum(prms, 88200, 4096, thread));

            Special_Filter = new System.Numerics.Complex[4096];
            for (int i = 0; i < Special_Filter.Length; i++) Special_Filter[i] = 1;

            foreach (int q in Seq_Polys)
            {
                if (Room.AbsorptionValue[q] is Basic_Material) continue; 
                //Pressure based formulation of materials
                for (int i = 0; i < Seq_Polys.Length; i++)
                {
                    Hare.Geometry.Vector d = Path[i + 1] - Path[i + 2]; d.Normalize();
                    if (!(Room.AbsorptionValue[Seq_Polys[i]] is Basic_Material))
                    {
                        System.Numerics.Complex[] Ref = Room.AbsorptionValue[Seq_Polys[i]].Reflection_Spectrum(44100, 4096, Room.Normal(Seq_Polys[i]), d, thread);
                        //System.Numerics.Complex[] Ref = Room.AbsorptionValue[Seq_Polys[i]].Reflection_Spectrum(88200, 4096, Room.Normal(Seq_Polys[i]), d, thread);
                        //for (int j = 0; j < Pspec.Length; j++) Pspec[j] *= Ref[j];
                        for (int j = 0; j < Special_Filter.Length; j++) Special_Filter[j] *= Ref[j];
                    }
                }
            }

            Create_pressure(44100, 4096, thread);

            //double[] tank = new double[Pspec.Length];
            //for (int i = 0; i < tank.Length; i++) tank[i] = Pspec[i].Real;
            //P = Audio.Pach_SP.Minimum_Phase_Response(tank, 44100, thread);
            //TODO: Investigate phase propoerties of this for special materials filters...
            //double[] pre = Audio.Pach_SP.IFFT_Real4096(Pspec, thread);
            //P = new double[pre.Length];
            //double scale = Math.Sqrt(P.Length);
            //int hw = P.Length / 2;
            //for (int i = 0; i < pre.Length; i++) P[i] = pre[(i + hw) % pre.Length] / scale;
            /////////////////////////////////
            //Audio.Pach_SP.resample(ref P);
            ///////////////////////////////
            //Audio.Pach_SP.Raised_Cosine_Window(ref P);
        }

        public void Create_pressure(int samplefrequency, int length, int threadid)
        {
            System.Numerics.Complex[] Pspec = Audio.Pach_SP.Filter.Spectrum(prms, samplefrequency, length, threadid);

            //System.Numerics.Complex[] Pspec = Audio.Pach_SP.Mirror_Spectrum(Audio.Pach_SP.Magnitude_Spectrum(prms, 44100, 4096, 0));
            //System.Numerics.Complex[] Pspec = Audio.Pach_SP.Mirror_Spectrum(Audio.Pach_SP.Magnitude_Spectrum(prms, 88200, 4096, 0));

            if (Special_Filter != null)
            {
                for (int j = 0; j < Pspec.Length; j++) Pspec[j] *= Special_Filter[j];
            }

            double[] pre = Audio.Pach_SP.IFFT_Real4096(Pspec, 0);
            P = new double[pre.Length];
            double scale = Math.Sqrt(P.Length);
            int hw = P.Length / 2;
            for (int i = 0; i < pre.Length; i++) P[i] = pre[(i + hw) % pre.Length] / scale;
        }

        private void Identify(int SrcID, double Direct_Time)
        {
            if (SrcID < 10)
            {
                Identifier = string.Format("S00{0}-", SrcID);
            }
            else if (SrcID < 100)
            {
                Identifier = string.Format("S0{0}-", SrcID);
            }
            else
            {
                Identifier = string.Format("S{0}-", SrcID);
            }

            if (Sequence.Length < 10)
            {
                Identifier = string.Concat(Identifier, string.Format("Order 00{0}: ", Sequence.Length));
            }
            else if (Sequence.Length < 100)
            {
                Identifier = string.Concat(Identifier, string.Format("Order 0{0}: ", Sequence.Length));
            }
            else
            {
                Identifier = string.Concat(Identifier, string.Format("Order {0}:", Sequence.Length));
            }

            Identifier = string.Concat(Identifier, string.Format("{0} ms. ", Math.Round((Time - Direct_Time) * 1000)));

            foreach (int Digit in Sequence)
            {
                Identifier = string.Concat(Identifier, Digit.ToString(), " ");
            }
        }

        public override Polyline[] PolyLine
        {
            get 
            {
                Polyline Path = new Polyline();
                //Enter the Data
                foreach (Hare.Geometry.Point Point in ValidPath)
                {
                    Path.Add(new Point3d(Point.x, Point.y, Point.z));
                }
                return new Polyline[] {Path};
            }
        }

        public override Hare.Geometry.Point[][] Path
        {
            get { return new Hare.Geometry.Point[][] { ValidPath }; }
        }

        public override double TravelTime
        {
            get { return Time; }
        }

        public override int[] Reflection_Sequence
        {
            get { return Sequence; }
        }

        public override double[] Energy(int Octave)
        {
            return new double[] {PathEnergy[Octave]};
        }

        public override double[] Dir_Energy(int Octave, int dir)
        {
            Vector Dir = Path[0][Path.Length - 1] - Path[0][Path.Length - 2];
            Dir.Normalize();
            switch (dir)
            {
                case 0:
                    return new double[] { Dir.x * PathEnergy[Octave] };
                case 1:
                    return new double[] { Dir.y * PathEnergy[Octave] };
                case 2:
                    return new double[] { Dir.z * PathEnergy[Octave] };
                default:
                    throw new Exception("indexed directions must conform to 0 = x, 1 = y and 2 = z") ;
            }
        }

        public override Vector[] Dir_Energy(int Octave)
        {
            Vector Dir = Path[0][Path[0].Length - 1] - Path[0][Path[0].Length - 2];
            Dir.Normalize();
            return new Vector[] { Dir * PathEnergy[Octave] };
        }

        public override Vector[] Dir_Energy(int Octave, double alt, double azi, bool degrees)
        {
            Vector[] V = Dir_Energy(Octave);
            return new Vector[] { Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V[0], azi, 0, degrees), 0, alt, degrees)};
        }

        public override double[][] Dir_Pressure(int Octave, double alt, double azi, bool degrees, int SampleFreq)
        {
            Vector V = Path[0][Path[0].Length - 1] - Path[0][Path[0].Length - 2];
            V.Normalize();
            Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V, azi, 0, degrees), 0, alt, degrees);
            double[][] Pn = new double[P.Length][];

                for (int i = 0; i < P.Length; i++)
                {
                    Pn[i] = new double[3] {Vn.x * P[i], Vn.y * P[i], Vn.z * P[i]};
                }
            return Pn;
        }

        public override double[] Dir_Pressure(int Octave, double alt, double azi, bool degrees, bool Figure8, int SampleFreq)
        {
            Vector V = Path[0][Path[0].Length - 1] - Path[0][Path[0].Length - 2];
            V.Normalize();
            Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V, azi, 0, degrees), 0, alt, degrees);
            double[] Pn = new double[P.Length];
            if (Figure8)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    Pn[i] = Vn.x * P[i];
                }
            }
            if (Vn.x > 0)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    Pn[i] = Vn.x * P[i];
                }
            }
            return Pn;
        }

        public override Vector[] Dir_Energy(int Octave, Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);
            return Dir_Energy(Octave, alt, azi, false);
        }

        public int ReflectionOrder
        {
            get { return Sequence.Length; }
        }

        public int SurfaceIndex(int x)
        {
            return Sequence[x];
        }

        public double EnergySum()
        {
            double sum = 0;
            foreach(double e in PathEnergy) sum += e;
            return sum;
        }

        public override Vector[] Dir_EnergySum(Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);

            Vector E = new Vector();
            for (int oct = 0; oct < 8; oct++) E += Dir_Energy(oct, alt, azi, false)[0];
            return new Vector[] { E };
        }

        public override Vector[] Dir_EnergySum(double alt, double azi, bool degrees)
        {
            Vector E = new Vector();
            for (int oct = 0; oct < 8; oct++) E += Dir_Energy(oct, alt, azi, degrees)[0];
            return new Vector[] { E };
        }

        public override double[] Pressure
        {
            get
            {
                return P;
            }
        }

        public Vector[] Dir_Pressure()
        {
            Vector[] Dp = new Vector[P.Length];
            for (int i = 0; i < P.Length; i++)
            {
                Vector Dir = Path[i][Path[i].Length - 1] - Path[i][Path[i].Length - 2];
                Dir.Normalize();
                Dp[i] = Dir * P[i];
            }
            return Dp;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }

    public class Compound_Path : Deterministic_Reflection
    {
        private Hare.Geometry.Point[][] ValidPath;
        private double[][] PathEnergy;
        private double Time;
        private int[] Sequence;
        public double[] P;
        public double[][] Pdir;
        
        int fs = 44100;

        public Compound_Path(Hare.Geometry.Point[][] PathVertices, int[] Seq_Planes, int Source_ID, double[] H, double[][] Hdir, double T0, double Speed_of_Sound, ref double Direct_Time, int Threadid)
        {
            Time = T0;
            ///Here, a compound reflection collector, and any interpolation that must be done.
            ValidPath = PathVertices;
            List<Hare.Geometry.Point[]> Paths = new List<Hare.Geometry.Point[]>();
            Sequence = Seq_Planes;
            P = H;
            Pdir = Hdir;

            double[] pd = new double[P.Length];
            double[] timeaxis = new double[P.Length];
            for (int t = 0; t < timeaxis.Length; t++) { timeaxis[t] = (double)t / 44100f; pd[t] = (double)P[t]; }

            PathEnergy = new double[P.Length][];
            for (int t = 0; t < PathEnergy.Length; t++) PathEnergy[t] = new double[8];
            double[][] OctavePressure = new double[8][];

            for (int oct = 0; oct < 8; oct++)
            {
                OctavePressure[oct] = Audio.Pach_SP.FIR_Bandpass(this.P, oct, fs, Threadid);
                for (int t = 0; t < P.Length; t++)
                {
                    PathEnergy[t][oct] = OctavePressure[oct][t] * OctavePressure[oct][t] / (Speed_of_Sound * 1.2);
                }
            }

            //Build an Identifier
            Identify(Source_ID, Direct_Time);
        }

        private void Identify(int SrcID, double Direct_Time)
        {
            if (SrcID < 10)
            {
                Identifier = string.Format("S00{0}-", SrcID);
            }
            else if (SrcID < 100)
            {
                Identifier = string.Format("S0{0}-", SrcID);
            }
            else
            {
                Identifier = string.Format("S{0}-", SrcID);
            }

            if (Sequence.Length < 10)
            {
                Identifier = string.Concat(Identifier, string.Format("Order 00{0}: ", Sequence.Length));
            }
            else if (Sequence.Length < 100)
            {
                Identifier = string.Concat(Identifier, string.Format("Order 0{0}: ", Sequence.Length));
            }
            else
            {
                Identifier = string.Concat(Identifier, string.Format("Order {0}:", Sequence.Length));
            }

            Identifier = string.Concat(Identifier, string.Format("{0} ms. ", Math.Round((Time - Direct_Time) * 1000)));

            foreach (int Digit in Sequence)
            {
                Identifier = string.Concat(Identifier, Digit.ToString(), " ");
            }
        }

        public override Polyline[] PolyLine
        {
            get 
            {
                Polyline[] paths = new Polyline[ValidPath.Length];
                for(int i = 0; i < this.ValidPath.Length; i++)
                {
                    Rhino.Geometry.Point3d[] pline = new Rhino.Geometry.Point3d[ValidPath[i].Length];
                    for (int j = 0; j < pline.Length; j++) pline[j] = Utilities.PachTools.HPttoRPt(ValidPath[i][j]);
                    paths[i] = new Rhino.Geometry.Polyline(pline);
                }
                return paths;
            }
        }

        public override double[] Energy(int Octave)
        {
            double[] energy = new double[PathEnergy.Length];
            for(int i = 0; i < PathEnergy.Length; i++) energy[i] = PathEnergy[i][Octave];
            return energy;
        }

        public override double[] Dir_Energy(int Octave, int dir)
        {
            double[] De = new double[PathEnergy.Length];
            for (int i = 0; i < PathEnergy.Length; i++)
            {
                Vector Dir = Path[i][Path.Length - 1] - Path[i][Path.Length - 2];
                Dir.Normalize();
                switch (dir)
                {
                    case 0:
                        De[i] = Dir.x * PathEnergy[i][Octave]; break;
                    case 1:
                        De[i] = Dir.y * PathEnergy[i][Octave]; break;
                    case 2:
                        De[i] = Dir.z * PathEnergy[i][Octave]; break;
                    default:
                        throw new Exception("indexed directions must conform to 0 = x, 1 = y and 2 = z");
                }
            }
            return De;
        }

        public override Vector[] Dir_Energy(int Octave)
        {
            Vector[] De = new Vector[PathEnergy.Length];
            for (int i = 0; i < PathEnergy.Length; i++)
            {
                Vector Dir = Path[i][Path[i].Length - 1] - Path[i][Path[i].Length - 2];
                Dir.Normalize();
                De[i] = Dir * PathEnergy[i][Octave];
            }
            return De;
        }

        public override Vector[] Dir_Energy(int Octave, double alt, double azi, bool degrees)
        {
            Vector[] V = Dir_Energy(Octave);
            for (int i = 0; i < V.Length; i++)
            {
                 Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V[i], azi, 0, degrees), 0, alt, degrees);
            }
            return V;
        }

        public override Vector[] Dir_Energy(int Octave, Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);
            return Dir_Energy(Octave, alt, azi, false);
        }

        public override Vector[] Dir_EnergySum(Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);

            return Dir_EnergySum(alt, azi, false);
        }

        public override Vector[] Dir_EnergySum(double alt, double azi, bool degrees)
        {
            Vector[] E = new Vector[PathEnergy.Length];
            for(int i = 0; i < E.Length; i++) E[i] = new Vector();
            
            for (int oct = 0; oct < 8; oct++)
            {
                Vector[] d = Dir_Energy(oct, alt, azi, degrees);
                for (int i = 0; i < d.Length; i++) E[i] += d[i];
            }

            return E;
        }

        public override Hare.Geometry.Point[][] Path
        {
            get { return ValidPath; }
        }

        public override int[] Reflection_Sequence
        {
            get{ return Sequence;}
        }

        public override double TravelTime
        {
            get { return Time; }
        }

        public override double[] Dir_Pressure(int Rec_ID, double alt, double azi, bool degrees, bool Figure8, int sampleFreq)
        {
            double[] Pn = new double[P.Length];
            if (Figure8)
            {
                for (int i = 0; i < Pdir[Rec_ID].Length; i++)
                {
                    Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Vector(Pdir[i][0] - Pdir[i][1], Pdir[i][2] - Pdir[i][3], Pdir[i][4] - Pdir[i][5]), azi, 0, degrees), 0, alt, degrees);
                    Pn[i] = Vn.x;
                }
            }
            else
            {
                int[] ids = new int[3];
                ids[0] = (azi > 90 && azi < 270) ? 1 : 0;
                ids[0] = (azi <= 180) ? 3 : 4;
                ids[0] = (alt > 0) ? 4 : 5;
                for (int i = 0; i < Pdir[0].Length; i++)
                {
                    Hare.Geometry.Vector V = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Hare.Geometry.Vector(Pdir[ids[0]][i], Pdir[ids[1]][i], Pdir[ids[2]][i]), azi, 0, true), 0, alt, true);
                    Pn[i] = V.x;
                }
            }
            return Pn;
        }

        public override double[][] Dir_Pressure(int Rec_ID, double alt, double azi, bool degrees, int sampleFreq)
        {
            double[][] Pn = new double[P.Length][];

            for (int i = 0; i < Pdir[Rec_ID].Length; i++)
            {
                Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Vector(Pdir[i][0] - Pdir[i][1], Pdir[i][2] - Pdir[i][3], Pdir[i][4] - Pdir[i][5]), azi, 0, degrees), 0, alt, degrees);
                Pn[i] = new double[3] {Vn.x, Vn.y, Vn.z};
            }
            return Pn;
        }

        public override double[] Pressure
        {
            get
            {
                return P;
            }
        }
    }
}