////'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
////' 
////'This file is part of Pachyderm-Acoustic. 
////' 
////'Copyright (c) 2008-2013, Arthur van der Harten 
////'Pachyderm-Acoustic is free software; you can redistribute it and/or modify 
////'it under the terms of the GNU General Public License as published 
////'by the Free Software Foundation; either version 3 of the License, or 
////'(at your option) any later version. 
////'Pachyderm-Acoustic is distributed in the hope that it will be useful, 
////'but WITHOUT ANY WARRANTY; without even the implied warranty of 
////'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
////'GNU General Public License for more details. 
////' 
////'You should have received a copy of the GNU General Public 
////'License along with Pachyderm-Acoustic; if not, write to the Free Software 
////'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA. 

//using System;
//using System.Collections.Generic;
//using Hare.Geometry;
//using Pachyderm_Acoustic.Environment;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Security.Permissions;
//using System.IO.Pipes;

//namespace Pachyderm_Acoustic
//{
//    /// <summary>
//    /// This simulation type calculates the energy time curve of a room by ray tracing.
//    /// </summary>
//    public class SplitRayTracer_Process : Simulation_Type
//    {
//        protected System.IO.MemoryMappedFiles.MemoryMappedFile[] Output;
//        protected System.IO.MemoryMappedFiles.MemoryMappedFile Rm;
//        protected System.IO.MemoryMappedFiles.MemoryMappedFile Src;
//        protected System.IO.MemoryMappedFiles.MemoryMappedFile Rec;
//        protected Process[] ProcessList;

//        //private Receiver_Bank RecMain;
//        private int _processorCt;

//        protected int Raycount;
//        protected double CutoffLength;
//        private int IS_Order;
//        private int[] _octaves;
//        private bool MappingCalc;
       
////      private int _rayTotal;
////      private double _lost;
////      private double _eInit;
//        private int _currentRay;

//        /// <summary>
//        /// Constructor for the general case ray tracer.
//        /// </summary>
//        /// <param name="sourcein"></param>
//        /// <param name="receiverin"></param>
//        /// <param name="roomin"></param>
//        /// <param name="cutofflengthin"></param>
//        /// <param name="raycountin"></param>
//        /// <param name="isorderin"></param>
//        /// <param name="partitionedreceiver"></param>
//        public SplitRayTracer_Process(Source sourcein, Receiver_Bank receiverin, Scene roomin, double cutofflengthin, int raycountin, int isorderin, bool partitionedreceiver)
//            : this(sourcein, receiverin, roomin, cutofflengthin, raycountin, new int[] { 0, 7 }, isorderin, partitionedreceiver)
//        {

//        }

//        /// <summary>
//        /// Constructor for the general case ray tracer.
//        /// </summary>
//        /// <param name="sourceIn"></param>
//        /// <param name="receiverIn"></param>
//        /// <param name="roomIn"></param>
//        /// <param name="cutOffLengthIn"></param>
//        /// <param name="rayCountIn"></param>
//        /// <param name="octaveRange">Two values - the lowest octave to be calculated and the highest octave to be calculated - 0 being 62.5, and 7 being 8k.</param>
//        /// <param name="isOrderIn">The highest order for which image source was calcualted. If no Image Source, then enter 0.</param>
//        /// <param name="partitionedReceiver">Is the receiver partitioned... i.e., did you use a mapping receiver bank?</param>
//        public SplitRayTracer_Process(Source sourceIn, Receiver_Bank receiverIn, Scene roomIn, double cutOffLengthIn, int rayCountIn, int[] octaveRange, int isOrderIn, bool partitionedReceiver)
//        {
//            Random R = new Random((int)DateTime.Now.Ticks);
//            Raycount = rayCountIn;
//            IS_Order = isOrderIn;            
//            Rm = Utilities.PachTools.SerializeToMMF("Room:"+Pachyderm_Acoustic.UI.PachydermAc_PlugIn.Instance.InstanceID, roomIn);
//            Src = Utilities.PachTools.SerializeToMMF("Src:" + Pachyderm_Acoustic.UI.PachydermAc_PlugIn.Instance.InstanceID, sourceIn);
//            Rec = Utilities.PachTools.SerializeToMMF("Rec:" + Pachyderm_Acoustic.UI.PachydermAc_PlugIn.Instance.InstanceID, receiverIn);
            
//            _octaves = new int[octaveRange[1] - octaveRange[0] + 1];
//            _octaves = octaveRange;
//            CutoffLength = cutOffLengthIn;
//            MappingCalc = partitionedReceiver;
//            IS_Order = isOrderIn;
//        }

//        private void increment(object o, DataReceivedEventArgs e)
//        {
//            _currentRay++;
//        }

//        private void Error(object o, DataReceivedEventArgs e)
//        {
//            Rhino.RhinoApp.WriteLine("Oops...");
//        }

//        public override void Abort_Calculation()
//        {
//            foreach (System.Diagnostics.Process P in ProcessList) P.Kill();
//        }

//        public override System.Threading.ThreadState ThreadState()
//        {
//            foreach (System.Diagnostics.Process P in ProcessList) if (!P.HasExited) return System.Threading.ThreadState.Running;
//            return System.Threading.ThreadState.Stopped;
//        }

//        public override void Combine_ThreadLocal_Results()
//        {
//            throw new NotImplementedException();
//        }

//        public override string ProgressMsg()
//        {
//            return _currentRay.ToString();
//            //foreach (System.Diagnostics.Process P in ProcessList) 
//        }

//        //public Receiver_Bank GetReceiver
//        //{
//        //    get
//        //    {
//        //        return RecMain;
//        //    }
//        //}

//        public override void Begin()
//        {
//            Random Rnd = new Random();
//            _processorCt = UI.PachydermAc_PlugIn.Instance.ProcessorSpec();
//            //PipeList = new NamedPipeServerStream[_processorCt];
//            ProcessList = new System.Diagnostics.Process[_processorCt];
//            //System.Threading.ThreadState t = System.Threading.Thread.CurrentThread.ThreadState;//.SetApartmentState(System.Threading.ApartmentState.MTA);
//            string id = Pachyderm_Acoustic.UI.PachydermAc_PlugIn.Instance.InstanceID.ToString();

//            for (int P_I = 0; P_I < _processorCt; P_I++)
//            {
//                int Start = P_I * Raycount / _processorCt, End = (P_I + 1) * Raycount / _processorCt;
//                ProcessList[P_I] = new System.Diagnostics.Process();
//                ProcessList[P_I].EnableRaisingEvents = true;
//                ProcessList[P_I].OutputDataReceived += increment;
//                ProcessList[P_I].ErrorDataReceived += Error;
//                ProcessList[P_I].StartInfo = new System.Diagnostics.ProcessStartInfo("Pach_RayTracer_Process.exe", P_I.ToString() + " " + true.ToString() + " " + id + " " + "Room:" + id + " " + "Src:" + id + " " + "Rec:" + id + " "+ ";" + Start.ToString() + ";" + End.ToString() + ";" + CutoffLength.ToString() + ";" + IS_Order.ToString() + ";" + _octaves[0].ToString() + ";" + _octaves[1].ToString());
//                ProcessList[P_I].StartInfo.UseShellExecute = false;
//                ProcessList[P_I].Start();
//                ProcessList[P_I].ProcessorAffinity = (IntPtr)(P_I + 1);
//            }
//        }

//        /// <summary>
//        /// A string to identify the type of simulation being run.
//        /// </summary>
//        /// <returns></returns>
//        public override string Sim_Type()
//        {
//            return "Stochastic Ray Tracing";
//        }
//    }
//}