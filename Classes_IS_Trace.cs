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

namespace Pachyderm_Acoustic
{
    class IS_Trace: Simulation_Type
    {
        protected double C_Sound;
        protected Polygon_Scene Room;
        protected Source Source;
        protected Receiver_Bank Receiver;
        protected int Raycount;
        protected double CutoffLength;
        protected int ImageOrder;
        protected double CO_Time;
        protected List<int[]>[] SequenceList;
        protected List<int[]>[,] Detections;
        private DateTime ST;
        private int[] Current_Ray;
        System.Threading.Thread[] T_List;
        int processorCT;
        private TimeSpan TS;

        public IS_Trace(Source Source_in, Receiver_Bank Receiver_in, Polygon_Scene Room_in, double CutOffLength_in, int RayCount_in, int ImageOrder_in, double Speed_of_sound, int Bincount_in)
        {
            C_Sound = Speed_of_sound;
            CO_Time = CutOffLength_in / Speed_of_sound;
            Room = Room_in;
            Receiver = Receiver_in;
            Raycount = RayCount_in;
            CutoffLength = CutOffLength_in;
            ImageOrder = ImageOrder_in;
            Source = Source_in;
        }

        /// <summary>
        /// Inherited member. Divides the simulation into threads, and begins.
        /// </summary>
        public override void Begin()
        {
            Random Rnd = new Random();
            processorCT = UI.PachydermAc_PlugIn.Instance.ProcessorSpec();
            Current_Ray = new int[processorCT];
            Detections = new List<int[]>[Receiver.Count, processorCT];
            SequenceList = new List<int[]>[Receiver.Count];

            T_List = new System.Threading.Thread[processorCT];

            for (int P_I = 0; P_I < processorCT; P_I++)
            {
                for (int i = 0; i < Receiver.Count; i++)
                {
                    Detections[i, P_I] = new List<int[]>();
                }
                 
                Calc_Params T = new Calc_Params(P_I * Raycount / processorCT, (P_I + 1) * Raycount / processorCT, P_I, Rnd.Next());
                System.Threading.ParameterizedThreadStart TS = new System.Threading.ParameterizedThreadStart(delegate { Calculate(T); });
                T_List[P_I] = new System.Threading.Thread(TS);
                T_List[P_I].Start();
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
        /// Aborts all threads, effectively ending the simulation.
        /// </summary>
        public override void Abort_Calculation()
        {
            foreach (System.Threading.Thread T in T_List)
            {
                T.Abort();
            }
        }

        /// <summary>
        /// Called by each thread from the begin method.
        /// </summary>
        /// <param name="i">the object is type "Calc_Params" which holds all the necessary information to run a portion of the simulation.</param>
        public void Calculate(object i)
        {
            Calc_Params Params = (Calc_Params)i;
            Random RND = new Random(Params.RandomSeed);
            ST = DateTime.Now;
            Hare.Geometry.Point Origin = Source.H_Origin();
            ///'''''''''''''Renewable Variables''''''''''''''''''
            double SumLength;
            double u = 0;
            double v = 0;
            Hare.Geometry.Point Point = new Hare.Geometry.Point();
            int ChosenIndex = 0;
            List<Hare.Geometry.Point> Start;
            int Reflections;
            List<int> Sequence = new List<int>();
            ///''''''''''''''''''''''''''''''''''''''''''''''''''

            for (Current_Ray[Params.ThreadID] = 0; Current_Ray[Params.ThreadID] < Params.EndIndex-Params.StartIndex; Current_Ray[Params.ThreadID]++)
            {
                BroadRay R = Source.Directions(Current_Ray[Params.ThreadID] + Params.StartIndex, Params.ThreadID, ref RND);
                SumLength = 0;
                Reflections = 0;
                Sequence.Clear();
                List<int> code = new List<int> { 0 };
                List<double> leg = new List<double> { 0 };
                do
                {
                    ///Only useable with homogeneous media///
                    SumLength += leg[0];
                    R.Ray_ID = RND.Next();
                    if (Room.shoot(R, out u, out v, out ChosenIndex, out Start, out leg, out code))
                    {
                        if (!Room.IsPlanar(ChosenIndex)) break;
                        Reflections ++;
                        ReflectRay(ref R, ref u, ref v, ref ChosenIndex);
                        if (Reflections > ImageOrder + 1)
                        {
                            bool[] B = Receiver.SimpleCheck(R, Start[0], SumLength);
                            for (int q = 0; q < B.Length; q++)
                            {
                                if (B[q]) Detections[q, Params.ThreadID].Add(Sequence.ToArray());
                            }
                        }
                    }
                    else
                    {
                        //Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(new LineCurve(Pachyderm_Acoustic.Utilities.PachTools.HPttoRPt(R.origin), Pachyderm_Acoustic.Utilities.PachTools.HPttoRPt(R.origin + R.direction) * 5));
                        break;
                    }
                    Sequence.Add(Room.PlaneID(ChosenIndex));
                    R.origin = Start[0];
                }
                while (SumLength < CutoffLength);
            }
        }

        /// <summary>
        /// A string to identify the type of simulation being run.
        /// </summary>
        /// <returns></returns>
        public override string Sim_Type()
        {
            return "Ray tracing - Image Source Specular Hybrid";
        }

        /// <summary>
        /// Called by Pach_RunSim_Command. Get a string describing the status of the simulation for display.
        /// </summary>
        /// <returns></returns>
        public override string ProgressMsg()
        {
            TS = DateTime.Now - ST;
            int Ray_CT = 0;
            for (int i = 0; i < processorCT; i++)
            {
                Ray_CT += Current_Ray[i];
            }
            TS = new TimeSpan((long)(TS.Ticks * (((double)(Raycount - Ray_CT)) / (Ray_CT + 1))));
            return string.Format("Calculating Ray {0} of {1}. ({2} hours,{3} minutes,{4} seconds Remaining.) Press 'Esc' to Cancel...", Ray_CT, Raycount, TS.Hours, TS.Minutes, TS.Seconds);
        }

        /// <summary>
        /// Calculates the direction of a specular reflection.
        /// </summary>
        /// <param name="RayDirect"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="x"></param>
        protected virtual void ReflectRay(ref BroadRay RayDirect, ref double u, ref double v, ref int x)
        {
            Vector local_N = Room.Normal(x, u, v);
            RayDirect.direction -= local_N * Hare_math.Dot(RayDirect.direction, local_N) * 2;
        }

        /// <summary>
        /// Consolidates output from all threads into a single set of output.
        /// </summary>
        public override void Combine_ThreadLocal_Results()
        {
            List<int[]>[] New_List = new List<int[]>[Receiver.Count];
            T_List = new System.Threading.Thread[processorCT];
            for (int R_ID = 0; R_ID < Receiver.Count; R_ID++)
            {
                New_List[R_ID] = new List<int[]>();
                for (int q = 0; q < processorCT; q++)
                {
                    foreach (int[] Sequence in Detections[R_ID,q])
                    {
                        for (int i = 0; i < New_List[R_ID].Count; i++) //(int[] value in List)
                        {
                            if (New_List[R_ID][i].Length != Sequence.Length) continue;
                            for (int t = 0; t < New_List[R_ID][i].Length; t++)
                            {
                                if (New_List[R_ID][i][t] != Sequence[t]) goto continue2;
                            }
                            goto NO_Add;
                        continue2:
                            continue;
                        }
                        New_List[R_ID].Add(Sequence);
                    NO_Add:
                        continue;
                    }
                }
            }
            SequenceList = New_List;
        }

        /// <summary>
        /// Get the list of specular reflection sequences.
        /// </summary>
        /// <returns></returns>
        public List<int[]>[] IS_Sequences()
        {
            return SequenceList;
        }
    }
}