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
using Hare.Geometry;
using Pachyderm_Acoustic.Environment;
using System.Collections.Generic;
using System.Linq;

namespace Pachyderm_Acoustic
{
    /// <summary>
    /// This simulation type calculates the direct sound, and whether or not it is manifest in the impulse response (I.E. audible or obstructed...).
    /// </summary>
    public class Direct_Sound:Simulation_Type
    {
        public Source Src;
        private List<Hare.Geometry.Point> Receiver;
        public double CO_Time;
        public int SampleFreq;
        private Scene Room;
        private double C_Sound;
        public double[] SWL;
        public double Delay_ms;
        public string type;
        public bool[] Validity;//[Rec]
        public double[][,] Io;//[Rec][Time_ms, Oct]
        public double[][] P;//[Rec][Time_ms]
        public double[][][] Pdir;//[Rec][direction][Time_ms] (0 for x+, 1 for x-, 2 for y+, 3 for y-, 4 for z+, 5 for z-)
        public double[][,] Phase;//[Rec][Time_ms, Oct] //Source starting phase delay...
        public float[][,,] Dir_Rec_Pos;//[receiver][oct, sample, axis]
        public float[][,,] Dir_Rec_Neg;//[receiver][oct, sample, axis]
        public double[] Time_Pt;//[Rec]
        public int[] Oct_choice;
        public double[] Rho_C;
        System.Threading.Thread T;

        /// <summary>
        /// This method copies the direct sound information to an open, writeable binary stream.
        /// </summary>
        /// <param name="BW">The Binary Writer to which the direct sound will be written.</param>
        public void Write_Data(ref System.IO.BinaryWriter BW)
        {
            //1. Write an indicator to signify that there is direct sound data
            BW.Write("Direct_Sound w sourcedata");

            //2. Write strength reference data
            //Deprecated at V 2.0.
            
            //2.1 Write source data - type and SWL
            BW.Write(type); //string (delimited by Colons.)
            for (int o = 0; o < SWL.Length; o++)
            {
                BW.Write(SWL[o]); //double
            }
            BW.Write(Delay_ms);

            for (int q = 0; q < Receiver.Count; q++)
            {
                //2.2 Write number of Source Pts (No_of_pts).
                //if (Duration_ms > 1) { BW.Write(Duration_ms); }
                BW.Write(Io[q].GetLength(0));

                //3. Write the validity of the direct sound
                BW.Write(Validity[q]);
                //4. Write the Time point
                BW.Write(Time(q));
                for (int time = 0; time < Io[q].GetLength(0); time++)
                {
                    //5. Write all Energy data
                    BW.Write(Io[q][time, 0]);
                    BW.Write(Io[q][time, 1]);
                    BW.Write(Io[q][time, 2]);
                    BW.Write(Io[q][time, 3]);
                    BW.Write(Io[q][time, 4]);
                    BW.Write(Io[q][time, 5]);
                    BW.Write(Io[q][time, 6]);
                    BW.Write(Io[q][time, 7]);

                    //5c. Write all directional data
                    for (int oct = 0; oct < 8; oct++) for (int dir = 0; dir < 3; dir++)
                        {
                            BW.Write(Dir_Rec_Pos[q][oct, time, dir]);
                            BW.Write(Dir_Rec_Neg[q][oct, time, dir]);
                        }
                }
            }
        }

        /// <summary>
        /// This method fills out a direct sound object with the minimum information from a .pac1 file. The Binary Reader must be set to a point where the method can read the direct sound data without any adjustment.
        /// </summary>
        /// <param name="BR">The open BinaryReader object</param>
        /// <param name="Rec_CT">The number of receivers the direct sound was calculated for.</param>
        /// <returns>The completed direct sound simulation type.</returns>
        public static Direct_Sound Read_Data(ref System.IO.BinaryReader BR, IEnumerable<Hare.Geometry.Point> RecPts, Hare.Geometry.Point SrcPt, double[] Rho_C, string Version)
        {
            Direct_Sound D = new Direct_Sound();

            D.Receiver = RecPts.ToList<Hare.Geometry.Point>(); //new Receiver_Bank(RecPts, SrcPt, 343, new double[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 0, 1000, 0, Receiver_Bank.Type.Stationary);
            int Rec_Ct = D.Receiver.Count;//RecPts.Count<Rhino.Geometry.Point3d>();
            D.Rho_C = Rho_C;
            //2. Write strength reference data
            //Pre 2.0, 8 doubles.
            if (double.Parse(Version.Substring(0, 3)) < 2.0)
            {
                for(int i = 0; i < 8; i++) BR.ReadDouble();
            }
            ////Duration_ms = 1;
            //TODO Add version 1.5 and greater Source direction. (oops)
            if (double.Parse(Version.Substring(0, 3)) >= 1.1)
            {
                //2.1 Write source data - type and SWL
                string[] typestring = BR.ReadString().Split(':'); //string
                D.type = typestring[0];
                //if (typestring.Length > 1 && typestring[1] == "Histogram") 
                //{
                    //2.2 Write number of samples
                    ////Duration_ms = BR.ReadInt32();
                //}
                D.SWL = new double[8];
                for (int o = 0; o < 8; o++)
                {
                    D.SWL[o] = BR.ReadDouble(); //double
                }
                if (double.Parse(Version.Substring(0, 3)) >= 2.0)
                {
                    D.Delay_ms = BR.ReadDouble();
                }
                else
                {
                    D.Delay_ms = 0;
                }
            }
            else 
            {
                D.type = "Geodesic Source";
                    D.SWL = new double[]{120, 120, 120, 120, 120, 120, 120, 120};
                    D.Delay_ms = 0;
            }

            D.Src = new GeodesicSource(D.SWL, new double[8]{0,0,0,0,0,0,0,0}, new Rhino.Geometry.Point3d(SrcPt.x, SrcPt.y, SrcPt.z), 0, 0);
            D.Validity = new Boolean[RecPts.Count<Hare.Geometry.Point>()];
            D.Time_Pt = new double[RecPts.Count<Hare.Geometry.Point>()];
            D.Io = new double[RecPts.Count<Hare.Geometry.Point>()][,];
            D.Dir_Rec_Pos = new float[RecPts.Count<Hare.Geometry.Point>()][, ,];
            D.Dir_Rec_Neg = new float[RecPts.Count<Hare.Geometry.Point>()][, ,];

            double v = double.Parse(Version.Substring(0, 3));

            for (int q = 0; q < RecPts.Count<Hare.Geometry.Point>(); q++)
            {
                //2.2 Write number of samples
                int no_of_samples = BR.ReadInt32();
                //3. Write the validity of the direct sound
                D.Validity[q] = BR.ReadBoolean();
                //4. Write the Time point
                D.Time_Pt[q] = BR.ReadDouble();
                D.Io[q] = new double[no_of_samples, 9];

                D.Dir_Rec_Pos[q] = new float[8, no_of_samples, 3];
                D.Dir_Rec_Neg[q] = new float[8, no_of_samples, 3];
                    
                for (int s = 0; s < no_of_samples; s++)
                {
                    //5a. Write all Energy data
                    D.Io[q][s, 0] = BR.ReadDouble();
                    D.Io[q][s, 1] = BR.ReadDouble();
                    D.Io[q][s, 2] = BR.ReadDouble();
                    D.Io[q][s, 3] = BR.ReadDouble();
                    D.Io[q][s, 4] = BR.ReadDouble();
                    D.Io[q][s, 5] = BR.ReadDouble();
                    D.Io[q][s, 6] = BR.ReadDouble();
                    D.Io[q][s, 7] = BR.ReadDouble();
                    D.Io[q][s, 8] = D.Io[q][s, 0] + D.Io[q][s, 1] + D.Io[q][s, 2] + D.Io[q][s, 3] + D.Io[q][s, 4] + D.Io[q][s, 5] + D.Io[q][s, 6] + D.Io[q][s, 7];

                    if (v == 1.7)
                    {
                        //5b. Write all Pressure Data
                        //Real
                        //Imag
                        for(int i = 0; i < 16; i++) BR.ReadSingle();    
                    }
                    //5c. Write all directional data
                    for (int oct = 0; oct < 8; oct++) for (int dir = 0; dir < 3; dir++)
                        {
                            D.Dir_Rec_Pos[q][oct, s, dir] = BR.ReadSingle();
                            D.Dir_Rec_Neg[q][oct, s, dir] = BR.ReadSingle();
                        }
                }
            }

            D.Create_Pressure();

            return D;
        }

        private Direct_Sound() 
        {
        }

        /// <summary>
        /// Direct sound constructor. This prepares the simulation to run. 
        /// </summary>
        /// <param name="Src_in">The sound source</param>
        /// <param name="Rec_in">The collection of receivers</param>
        /// <param name="Room_in">The acoustical scene to render</param>
        /// <param name="RayCount">The number of rays which will be used </param>
        public Direct_Sound(Source Src_in, Receiver_Bank Rec_in, Scene Room_in, int[] Octaves)
        {
            type = Src_in.Type();
            Validity = new bool[Rec_in.Count];//[Rec_in.Count];
            Io = new double[Rec_in.Count][,];//[Rec_in.Count][t,8];
            Time_Pt = new double[Rec_in.Count];//[Rec_in.Count];
            Dir_Rec_Pos = new float[Rec_in.Count][, ,];
            Dir_Rec_Neg = new float[Rec_in.Count][, ,];
            Room = Room_in;
            C_Sound = Room_in.Sound_speed(0);
            SampleFreq = Rec_in.SampleRate;
            Src = Src_in;
            this.CO_Time = Rec_in.CO_Time;
            Receiver = new List<Point>();
            Rho_C = new double[Rec_in.Count];
            for(int i = 0; i < Rec_in.Count; i++)
            {
                Rho_C[i] = Room.Rho_C(Rec_in.H_Origin(i));
                Receiver.Add(Rec_in.H_Origin(i));
            }
            SWL = new double[8] { Src_in.SWL(0), Src_in.SWL(1), Src_in.SWL(2), Src_in.SWL(3), Src_in.SWL(4), Src_in.SWL(5), Src_in.SWL(6), Src_in.SWL(7) };
            Delay_ms = Src.Delay;
            Oct_choice = Octaves;
        }

        /// <summary>
        /// A string to identify the type of simulation being run.
        /// </summary>
        /// <returns></returns>
        public override string Sim_Type()
        {
            return "Deterministic Direct Sound";
        }

        /// <summary>
        /// Called by Pach_RunSim_Command. Get a string describing the status of the simulation for display.
        /// </summary>
        /// <returns></returns>
        public override string ProgressMsg()
        {
            if (Special_Status.Instance.special) return string.Format("Calculating Special Direct Sound: {0}%", Math.Round(Special_Status.Instance.progress*100));
            return "Calculating Direct_Sound...";
        }

        /// <summary>
        /// Inherited member. Divides the simulation into threads, and begins.
        /// </summary>
        public override void Begin()
        {
            System.Threading.ThreadStart TS = new System.Threading.ThreadStart(Calculate);
            T = new System.Threading.Thread(TS);
            T.Start();
        }

        /// <summary>
        /// Aborts all threads, effectively ending the simulation.
        /// </summary>
        public override void Abort_Calculation()
        {
            T.Abort();
        }

        /// <summary>
        /// Called by Pach_RunSim_Command. Indicates whether or not the simulation has completed.
        /// </summary>
        /// <returns>Returns running if any threads in this simulation are still running. Returns stopped if all have stopped.</returns>
        public override System.Threading.ThreadState ThreadState()
        {
            if (T.ThreadState == System.Threading.ThreadState.Running || T.ThreadState == System.Threading.ThreadState.WaitSleepJoin) return System.Threading.ThreadState.Running;
            return System.Threading.ThreadState.Stopped;
        }

        /// <summary>
        /// Consolidates output from all threads into a single set of output. 
        /// </summary>
        public override void Combine_ThreadLocal_Results()
        {
        }

        /// <summary>
        /// Checks whether the direct sound meets the receiver (true) or if it is obstructed (false).
        /// </summary>
        /// <param name="rec_id">Index of the receiver.</param>
        /// <param name="rnd">Random number, used to instantiate a ray.</param>
        private void Check_Validity(int rec_id, int rnd, out double[] TransMod)
        {
            Vector d = Receiver[rec_id] - Src.H_Origin();
            //double dist = Receiver.Origin(rec_id).DistanceTo(Src.Origin());
            double dist = d.Length();
            d.Normalize();
            Ray R = new Ray(Src.H_Origin(), d, 0, rnd);
            double x1 = 0, x2 = 0;
            double t;
            //List<int> code;
            int x3 = 0;
            Point x4;
            TransMod = new double[8] {1,1,1,1,1,1,1,1};
            while (true)
            {
                if (Room.shoot(R, out x1, out x2, out x3, out x4, out t))
                {
                    if (Room.IsTransmissive[x3]) 
                    {
                        for(int oct = 0; oct < 8; oct++) TransMod[oct] *= Room.TransmissionValue[x3][oct];
                        continue;
                    }
                    //if (t < dist) RMA.Rhino.RhUtil.RhinoApp().ActiveDoc().AddCurveObject(new RMA.OpenNURBS.OnLineCurve(new RMA.OpenNURBS.OnLineCurve(Receiver.Origin(rec_id), Src.Origin())));
                    Validity[rec_id] = (t >= dist);
                    break;
                }
                else
                {
                    Validity[rec_id] = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Called by each thread from the begin method.
        /// </summary>
        public void Calculate()
        {
            if (Src.Type() == "Line Source")
            { 
                Line_Calculation();
            }
            else if (Src.Type() == "Surface Sound")
            {
                Surface_Calculation();
            }
            else
            {
                Random rnd = new Random();
                Validity = new bool[Receiver.Count];
                Io = new double[Receiver.Count][,];
                Phase = new double[Receiver.Count][,];
                Time_Pt = new double[Receiver.Count];
                //P = new double[Receiver.Count][];

                for (int i = 0; i < Receiver.Count; i++)
                {
                    Io[i] = new double[1, 8];
                    Phase[i] = new double[1, 8];
                    //P[i] = new double[1];
                    Dir_Rec_Pos[i] = new float[8, 1, 3];
                    Dir_Rec_Neg[i] = new float[8, 1, 3];
                    double[] transmod;
                    Check_Validity(i, rnd.Next(), out transmod);
                    Rho_C[i] = Room.Rho_C(Receiver[i]);

                    double Length = Src.Origin().DistanceTo(Utilities.PachTools.HPttoRPt(Receiver[i]));
                    Vector dir = Receiver[i] - Src.H_Origin();
                    dir.Normalize();

                    double[] Power = Src.DirPower(0, rnd.Next(), dir);
                    double[] phase_in = Src.Phase(dir, ref rnd);
                    for (int o = 0; o < 8; o++) Phase[i][0, o] = phase_in[o];

                    Io[i][0, 0] = Power[0] * Math.Pow(10, -.1 * Room.Attenuation(0)[0] * Length) * transmod[0] / (4 * Math.PI * Length * Length);
                    Io[i][0, 1] = Power[1] * Math.Pow(10, -.1 * Room.Attenuation(0)[1] * Length) * transmod[1] / (4 * Math.PI * Length * Length);
                    Io[i][0, 2] = Power[2] * Math.Pow(10, -.1 * Room.Attenuation(0)[2] * Length) * transmod[2] / (4 * Math.PI * Length * Length);
                    Io[i][0, 3] = Power[3] * Math.Pow(10, -.1 * Room.Attenuation(0)[3] * Length) * transmod[3] / (4 * Math.PI * Length * Length);
                    Io[i][0, 4] = Power[4] * Math.Pow(10, -.1 * Room.Attenuation(0)[4] * Length) * transmod[4] / (4 * Math.PI * Length * Length);
                    Io[i][0, 5] = Power[5] * Math.Pow(10, -.1 * Room.Attenuation(0)[5] * Length) * transmod[5] / (4 * Math.PI * Length * Length);
                    Io[i][0, 6] = Power[6] * Math.Pow(10, -.1 * Room.Attenuation(0)[6] * Length) * transmod[6] / (4 * Math.PI * Length * Length);
                    Io[i][0, 7] = Power[7] * Math.Pow(10, -.1 * Room.Attenuation(0)[7] * Length) * transmod[7] / (4 * Math.PI * Length * Length);

                    float time = (float)(Length / C_Sound) + (float)Src.Delay;
                    //float real, imag;

                    for (int oct = 0; oct < 8; oct++)
                    {
                        //Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[oct] * time + phase_in[oct]), out real, out imag);
                        //P[i][0, oct] = Math.Sqrt(Io[i][0, oct] * Room.Rho_C(Receiver[i]));
                        Vector V = dir * Io[i][0, oct];

                        if (V.x > 0) Dir_Rec_Pos[i][oct, 0, 0] += (float)V.x; else Dir_Rec_Neg[i][oct, 0, 0] += (float)V.x;
                        if (V.y > 0) Dir_Rec_Pos[i][oct, 0, 1] += (float)V.y; else Dir_Rec_Neg[i][oct, 0, 1] += (float)V.y;
                        if (V.z > 0) Dir_Rec_Pos[i][oct, 0, 2] += (float)V.z; else Dir_Rec_Neg[i][oct, 0, 2] += (float)V.z;
                    }

                    Time_Pt[i] = Length / C_Sound + Src.Delay;
                }
            }
        }

        public double[] Dir_Pressure(int Rec_ID, double alt, double azi, bool degrees)
        {
            double[] Pn = new double[P[Rec_ID].Length];
                if (Dir_Rec_Pos[Rec_ID].GetLength(1) > 1)
                {
                    for (int i = 0; i < Pdir[Rec_ID].Length; i++)
                    {
                        double[] Eo = new double[8];
                        Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 2]));
                        Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(D, azi, 0, degrees), 0, alt, degrees);
                        Vn.Normalize();
                        if (Vn.x > 0)
                        {
                            for (int octave = 0; octave < 8; octave++)
                            {
                                Eo[octave] = Math.Sqrt(Io[Rec_ID][i, octave] * Rho_C[Rec_ID]) ;
                            }
                        }
                        double[] P_temp = Audio.Pach_SP.Filter.Signal(Eo, SampleFreq, 4096, 0);
                        for(int j = 0; j < 4096; j++)
                        {
                            Pn[i+j] += P_temp[j] * Vn.x;
                        }
                    }
                }
                else 
                {
                    Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 2]));
                    Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(D, azi, 0, degrees), 0, alt, degrees);
                    Vn.Normalize();
                    if (Vn.x > 0) for (int i = 0; i < P.Length; i++) Pn[i] = P[Rec_ID][i] * Vn.x;
                }
            return Pn;
        }

        public double[][] Dir_Pressure(int Rec_ID, double alt, double azi, bool degrees, bool Figure8)
        {
            double[][] Pn = new double[P[Rec_ID].Length][];
            if (Figure8)
            {
                for (int i = 0; i < Pdir[Rec_ID][0].Length; i++)
                {
                    Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(new Vector(Pdir[Rec_ID][0][i] - Pdir[Rec_ID][1][i], Pdir[Rec_ID][2][i] - Pdir[Rec_ID][3][i], Pdir[Rec_ID][4][i] - Pdir[Rec_ID][5][i]), azi, 0, degrees), 0, alt, degrees);
                    Pn[i] = new double[3]{ Vn.x, Vn.y, Vn.z };
                }
            }
            else
            {
                if (Dir_Rec_Pos[Rec_ID].GetLength(1) > 1)
                {
                    for (int i = 0; i < Pn.Length; i++) Pn[i] = new double[3];

                    for (int i = 0; i < Pdir[Rec_ID].Length; i++)
                    {
                        double[] Eo = new double[8];
                        Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 2]));
                        Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(D, azi, 0, degrees), 0, alt, degrees);
                        Vn.Normalize();
                        if (Vn.x > 0)
                        {
                            for (int octave = 0; octave < 8; octave++)
                            {
                                Eo[octave] = Math.Sqrt(Io[Rec_ID][i, octave] * Rho_C[Rec_ID]);
                            }
                        }
                        double[] P_temp = Audio.Pach_SP.Filter.Signal(Eo, SampleFreq, 4096, 0);
                        for (int j = 0; j < 4096; j++)
                        {
                            Pn[i + j][0] += P_temp[j] * Vn.x;
                            Pn[i + j][1] += P_temp[j] * Vn.y;
                            Pn[i + j][2] += P_temp[j] * Vn.z;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Pn.Length; i++) Pn[i] = new double[3];
                    Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5, 0, 2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5, 0, 2]));
                    Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(D, azi, 0, degrees), 0, alt, degrees);
                    Vn.Normalize();
                    if (Vn.x > 0) for (int i = 0; i < P[0].Length; i++) Pn[i][0] = P[Rec_ID][i] * Vn.x;
                    if (Vn.y > 0) for (int i = 0; i < P[0].Length; i++) Pn[i][1] = P[Rec_ID][i] * Vn.y;
                    if (Vn.z > 0) for (int i = 0; i < P[0].Length; i++) Pn[i][2] = P[Rec_ID][i] * Vn.z;
                }
            }
            return Pn;
        }

        /// <summary>
        /// Returns whether the direct sound is audible to the receiver.
        /// </summary>
        /// <param name="id">Index of the receiver</param>
        /// <returns>True if audible to the receiver or false if it is obstructed.</returns>
        public bool IsOccluded(int id)
        {
            return Validity[id];
        }

        /// <summary>
        /// Number of samples in direct sound. Divide by sample frequency to get time in seconds. Applicable to Energy. Will only yield >1 for non-point sources.
        /// </summary>
        /// <returns></returns>
        public int Duration()
        {
            int d = Io[0].GetLength(0);
            for (int i = 1; i < Io.Length; i++)
            {
                if (d < Io[i].GetLength(0)) d = Io[i].GetLength(0);
            }
            return d;
        }

        /// <summary>
        /// Earliest time index of the arrival of direct sound.
        /// </summary>
        /// <param name="Rec_ID"></param>
        /// <returns></returns>
        public double Min_Time(int Rec_ID)
        {
            return Time_Pt[Rec_ID];
        }

        /// <summary>
        /// Time index of the arrival of direct sound.
        /// </summary>
        /// <param name="Rec_ID">Index of the receiver.</param>
        /// <returns></returns>
        public double Time(int Rec_ID)
        {
             return Time_Pt[Rec_ID];
        }

        /// <summary>
        /// The energy which reaches the receiver from direct sound.
        /// </summary>
        /// <param name="octave">Chosen Octave Band</param>
        /// <param name="Rec_ID">Index of the receiver.</param>
        /// <returns></returns>
        public double EnergyValue(int octave, int t, int Rec_ID)
        {
            if (t < 0 || t > Io.Length) return 0;

            if (octave > 7) return EnergySum(Rec_ID, t);

            if (t < Io[Rec_ID].GetLength(0))
            {
                return Io[Rec_ID][t, octave];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// The combined energy of all octave bands.
        /// </summary>
        /// <param name="Rec_ID">The index of the receiver.</param>
        /// <returns></returns>
        public double EnergySum(int Rec_ID, int t)
        {
            if (t < Io[Rec_ID].GetLength(0))
            {
                return Io[Rec_ID][t, 0] + Io[Rec_ID][t, 1] + Io[Rec_ID][t, 2] + Io[Rec_ID][t, 3] + Io[Rec_ID][t, 4] + Io[Rec_ID][t, 5] + Io[Rec_ID][t, 6] + Io[Rec_ID][t, 7];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// The energy which reaches the receiver from direct sound.
        /// </summary>
        /// <param name="octave">Chosen Octave Band</param>
        /// <param name="Rec_ID">Index of the receiver.</param>
        /// <returns></returns>
        public double[] EnergyValue(int octave, int Rec_ID)
        {
            if (octave > 7) return EnergySum(Rec_ID);
            double[] E = new double[Io[Rec_ID].Length]; 

            for (int i = 0; i < Io[Rec_ID].GetLength(0); i++)
            {
                E[i] = Io[Rec_ID][i, octave];
            }

            return E;
        }

        /// <summary>
        /// The combined energy of all octave bands.
        /// </summary>
        /// <param name="Rec_ID">The index of the receiver.</param>
        /// <returns></returns>
        public double[] EnergySum(int Rec_ID)
        {
            double[] E = new double[Io[Rec_ID].Length];
            for(int i = 0; i < Io[Rec_ID].GetLength(0); i++)
            {
                E[i] = Io[Rec_ID][i, 0] + Io[Rec_ID][i, 1] + Io[Rec_ID][i, 2] + Io[Rec_ID][i, 3] + Io[Rec_ID][i, 4] + Io[Rec_ID][i, 5] + Io[Rec_ID][i, 6] + Io[Rec_ID][i, 7];
            }
            return E;
        }

        public Rhino.Geometry.Point3d Src_Origin
        {
            get 
            {
                return Src.Origin();
            }
        }
        public IEnumerable<Rhino.Geometry.Point3d> Rec_Origin
        {
            get
            {
                foreach (Hare.Geometry.Point p in this.Receiver)
                {
                    yield return Utilities.PachTools.HPttoRPt(p);
                }
            }
        }

        public double Cutoff_Time
        {
            get 
            {
                if (Receiver == null) return -1;
                return CO_Time;
            }
        }

        public double SampleRate
        {
            get
            {
                if (Receiver == null) return -1;
                return SampleFreq;
            }
        }

        public static Rhino.Geometry.Point3d[] Origins(Direct_Sound[] D)
        {
            System.Collections.Generic.List<Rhino.Geometry.Point3d> Orig = new System.Collections.Generic.List<Rhino.Geometry.Point3d>();
            foreach (Direct_Sound Dir in D)
            {
                Orig.Add(Dir.Src_Origin);
            }
            return Orig.ToArray();
        }

        //
        public virtual Vector[] Directions_Pos(int Octave, int Rec_Index, double alt, double azi, bool degrees)
        {
            int length = Dir_Rec_Pos[Rec_Index].GetLength(1);
            Vector[] V = new Vector[length];

            for (int i = 0; i < length; i++)
            {
                V[i] = new Vector(Dir_Rec_Pos[Rec_Index][Octave, i, 0], Dir_Rec_Pos[Rec_Index][Octave, i, 1], Dir_Rec_Pos[Rec_Index][Octave, i, 2]);
                V[i] = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V[i], azi, 0, degrees), 0, alt, degrees);
            }
            return V;
        }

        public virtual Vector[] Directions_Neg(int Octave, int Rec_Index, double alt, double azi, bool degrees)
        {
            int length = Dir_Rec_Neg[Rec_Index].GetLength(1);
            Vector[] V = new Vector[length];

            for (int i = 0; i < length; i++)
            {
                V[i] = new Vector(Dir_Rec_Neg[Rec_Index][Octave, i, 0], Dir_Rec_Neg[Rec_Index][Octave, i, 1], Dir_Rec_Neg[Rec_Index][Octave, i, 2]);
                V[i] = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V[i], azi, 0, degrees), 0, alt, degrees);
            }
            return V;
        }

        public virtual Vector[] Directions_Pos(int Octave, int Rec_Index, Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);
            return Directions_Pos(Octave, Rec_Index, alt, azi, false);
        }

        public virtual Vector[] Directions_Neg(int Octave, int Rec_Index, Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);
            return Directions_Neg(Octave, Rec_Index, alt, azi, false);
        }
        //

        public double Dir_Energy(int Octave, int Rec_ID, int dir)
        {
            Vector Dir = Receiver[Rec_ID] - Src.H_Origin();
            Dir.Normalize();
            switch (dir)
            {
                case 0:
                    return Dir.x * this.Io[Rec_ID][0, Octave];
                case 1:
                    return Dir.y * this.Io[Rec_ID][0, Octave];
                case 2:
                    return Dir.z * this.Io[Rec_ID][0, Octave];
                default:
                    throw new Exception("indexed directions must conform to 0 = x, 1 = y and 2 = z");
            }
        }

        public Vector[] Dir_Energy(int Octave, int Rec_ID)
        {
            Vector[] Vpos = Directions_Pos(Octave, Rec_ID, 0, 0, false);
            Vector[] Vneg = Directions_Neg(Octave, Rec_ID, 0, 0, false);
            Vector[] V = new Vector[Vpos.Length];

            for (int i = 0; i < V.Length; i++)
            {
                V[i] = Vpos[i] - Vneg[i];
                V[i].Normalize();
                V[i] *= Io[Rec_ID][i, Octave];
            }

            return V;
        }

        public Vector[] Dir_Energy(int Octave, int Rec_ID, double alt, double azi, bool degrees)
        {
            Vector[] Vpos = Directions_Pos(Octave, Rec_ID, alt, azi, degrees); 
            Vector[] Vneg = Directions_Neg(Octave, Rec_ID, alt, azi, degrees);
            Vector[] V = new Vector[Vpos.Length];

            for (int i = 0; i < V.Length; i++)
            {
                V[i] = Vpos[i] - Vneg[i];
                V[i].Normalize();
                V[i] *= Io[Rec_ID][i, Octave];
            }

            return V;
        }

        public Vector[] Dir_Energy(int Octave, int Rec_ID, Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);
            return Dir_Energy(Octave, Rec_ID, alt, azi, false);
        }

        public Vector[] Dir_Energy_Sum(int Rec_ID, Vector V)
        {
            double l = Math.Sqrt(V.z * V.z + V.x * V.x);
            double azi = Math.Asin(V.y / l);
            double alt = Math.Atan2(V.x, V.z);
            
            Vector[] D = new Vector[Dir_Rec_Pos[Rec_ID].GetLength(1)];
            for (int o = 0; o < 8; o++)
            {
                Vector[] Vf = new Vector[Dir_Rec_Pos[Rec_ID].GetLength(1)];
                Vector[] Vpos = Directions_Pos(o, Rec_ID, alt, azi, false);
                Vector[] Vneg = Directions_Neg(o, Rec_ID, alt, azi, false);
                for (int i = 0; i < D.Length; i++)
                {
                    Vf[i] = Vpos[i] - Vneg[i];
                    Vf[i].Normalize();
                    Vf[i] *= Io[Rec_ID][i, o];
                    D[i] += Vf[i];
                }
            } 
            return D;
        }

        public Vector[] Dir_Energy_Sum(int Rec_ID, double alt, double azi, bool degrees)
        {
            Vector[] D = new Vector[Dir_Rec_Pos[Rec_ID].GetLength(1)];
            for(int i = 0; i < D.Length; i++) D[i] = new Vector();
            for (int o = 0; o < 8; o++)
            {
                Vector[] Vf = new Vector[Dir_Rec_Pos[Rec_ID].GetLength(1)];
                Vector[] Vpos = Directions_Pos(o, Rec_ID, alt, azi, false);
                Vector[] Vneg = Directions_Neg(o, Rec_ID, alt, azi, false);
                for (int i = 0; i < D.Length; i++)
                {
                    Vf[i] = Vpos[i] - Vneg[i];
                    Vf[i].Normalize();
                    Vf[i] *= Io[Rec_ID][i, o];
                    D[i] += Vf[i];
                }
            }
            return D;
        }

        ///// <summary>
        ///// Combines two Direct Sound calculations with data in differing octave bands.
        ///// </summary>
        ///// <param name="A"></param>
        ///// <param name="B"></param>
        ///// <returns></returns>
        //public static Direct_Sound operator *(Direct_Sound A, Direct_Sound B)
        //{
        //    List<Rhino.Geometry.Point3d> ARec = A.Rec_Origin.ToList<Rhino.Geometry.Point3d>();
        //    List<Rhino.Geometry.Point3d> BRec = A.Rec_Origin.ToList<Rhino.Geometry.Point3d>();

        //    if (A.Src_Origin.GetHashCode() != B.Src_Origin.GetHashCode() || ARec.Count<Rhino.Geometry.Point3d>() != BRec.Count<Rhino.Geometry.Point3d>()) 
        //    {
        //        System.Windows.Forms.MessageBox.Show("Data is for two different calculations. Simulations not Combined.");
        //        return null;
        //    }

        //    for (int i = 0; i < ARec.Count; i++)
        //    {
        //        if (ARec[i].GetHashCode() != BRec[i].GetHashCode())
        //        {
        //            System.Windows.Forms.MessageBox.Show("Data is for two different calculations. Simulations not Combined.");
        //            return null;
        //        }
        //    }

        //    foreach (int a in A.Oct_choice)
        //    {
        //        foreach(int b in B.Oct_choice)
        //        {
        //            if (a==b)
        //            {
        //                System.Windows.Forms.MessageBox.Show("Data Conflicts. Simulations not Combined.");
        //                return null;
        //            }
        //        }
        //    }

        //    foreach (int oct in B.Oct_choice)
        //    {
        //        for(int rec = 0; rec < ARec.Count; rec++)
        //        {
        //            for (int t = 0; t <= A.Io[rec][oct].GetUpperBound(0); t++) A.Io[rec][t, oct] = B.Io[rec][t, oct];
        //        }
        //    }

        //    return A;
        //}

        public void Create_Pressure()
        {
            P = new double[Receiver.Count][];
            Pdir = new double[Receiver.Count][][];
            double scale = Math.Sqrt(4096);

            for (int i = 0; i < Receiver.Count; i++)
            {
                double[][] ETC = new double[8][];

                for (int oct = 0; oct < 8; oct++) ETC[oct] = new double[Io[i].GetLongLength(0)];

                for (int t = 0; t < ETC[0].Length; t++)
                {
                    for (int oct = 0; oct < 8; oct++) ETC[oct][t] = Math.Sqrt(Io[i][t, oct] * Rho_C[i]);
                }

                P[i] = new double[ETC[0].Length + 4096];
                Pdir[i] = new double[6][];
                for (int j = 0; j < 6; j++) Pdir[i][j] = new double[ETC[0].Length + 4096];
                for (int t = 0; t < ETC[0].Length; t++)
                {
                    Rhino.RhinoApp.CommandPrompt = string.Format("Creating direct sound pressure for receiver {0}. {1}% complete, ", i, Math.Round((double)t / ETC[0].Length * 100));
                    double[] Pmin = Audio.Pach_SP.Filter.Signal(new double[8] { ETC[0][0], ETC[1][0], ETC[2][0], ETC[3][0], ETC[4][0], ETC[5][0], ETC[6][0], ETC[7][0] }, 44100, 4096, 0);
                    //Audio.Pach_SP.Raised_Cosine_Window(ref Pmin);
                    for (int u = 0; u < Pmin.Length; u++)
                    {
                        P[i][t + u] += Pmin[u];// *scale;
                    }

                    double[][] dir_E = new double[6][];
                    for (int d = 0; d < 6; d++) dir_E[d] = new double[8];
                    //double[] totalprms = new double[6];
                    Vector vpos = new Vector(), vneg = new Vector();
                    for (int oct = 0; oct < 8; oct++)
                    {
                        vpos += new Vector(Math.Abs(Dir_Rec_Pos[i][oct, t, 0]), Math.Abs(Dir_Rec_Pos[i][oct, t, 1]), Math.Abs(Dir_Rec_Pos[i][oct, t, 2]));
                        vneg += new Vector(Math.Abs(Dir_Rec_Neg[i][oct, t, 0]), Math.Abs(Dir_Rec_Neg[i][oct, t, 1]), Math.Abs(Dir_Rec_Neg[i][oct, t, 2]));
                    }

                    //6th order normalization:
                    double length = Math.Sqrt(+vpos.x * vpos.x + vneg.x * vneg.x + vpos.y * vpos.y + vneg.y * vneg.y + vpos.z * vpos.z + vneg.z * vneg.z);
                    vpos /= length;
                    vneg /= length;

                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 4096; k++)
                        {
                            int j2 = 2 * j;
                            Pdir[i][j2][t + k] += Pmin[k] * vpos.byint(j);//((double.IsNaN(hist_temp[j2][k])) ? 0: hist_temp[j2][k]);
                            Pdir[i][j2 + 1][t + k] += Pmin[k] * vneg.byint(j);//((double.IsNaN(hist_temp[j2 + 1][k])) ? 0 : hist_temp[j2 + 1][k]);
                        }
                    }
                }
            }
        }

        private bool Line_Calculation()
        {
            LineSource LSrc = Src as LineSource;

            //Homogeneous media only...
            List<Hare.Geometry.Point> R = new List<Hare.Geometry.Point>();
            foreach (Hare.Geometry.Point p in Receiver) R.Add(p);
            bool[][][] Valid = new bool[R.Count][][];

            double[][][] tau = new double[R.Count][][]; //Rec;Curve;Sample
            double[][][] dist = new double[R.Count][][]; //Rec;Curve;Sample
            int[] RecT = new int[R.Count];
            Io = new double[R.Count][,];
            //P = new double[R.Count][,];
            Dir_Rec_Pos = new float[R.Count][, ,];
            Dir_Rec_Neg = new float[R.Count][, ,];
            Vector[][][] dir = new Vector[R.Count][][];
            double[][][][] tmod = new double[R.Count][][][];
            ///////////////
            double[][][] d_mod = new double[R.Count][][];
            ///////////////
            List<int>[][] Inflection = new List<int>[R.Count][];
            Validity = new Boolean[R.Count];

            double C_Sound = Room.Sound_speed(0);

            Random rnd = new Random();
            List<int> rand_list = new List<int>();
            for (int i = 0; i < R.Count; i++)
            {
                rand_list.Add(rnd.Next());
            }

            System.Threading.Semaphore S = new System.Threading.Semaphore(1, 1);

            System.Threading.Tasks.Parallel.For(0, R.Count, rec_id =>
            {
                Inflection[rec_id] = new List<int>[LSrc.Curves.Count];
                Valid[rec_id] = new bool[LSrc.Curves.Count][];
                Random RndGen = new Random(rand_list[rec_id]);
                tau[rec_id] = new double[LSrc.Curves.Count][];
                dist[rec_id] = new double[LSrc.Curves.Count][];
                tmod[rec_id] = new double[LSrc.Curves.Count][][];
                dir[rec_id] = new Vector[LSrc.Curves.Count][];
                ///////////////
                d_mod[rec_id] = new double[LSrc.Curves.Count][];
                ///////////////
                for (int i = 0; i < LSrc.Curves.Count; i++)
                {
                    Inflection[rec_id][i] = new List<int>();
                    Valid[rec_id][i] = new bool[LSrc.Samples[i].Length];
                    tau[rec_id][i] = new double[LSrc.Samples[i].Length];
                    dist[rec_id][i] = new double[LSrc.Samples[i].Length];
                    Time_Pt[rec_id] = double.MaxValue;
                    tmod[rec_id][i] = new double[LSrc.Samples[i].Length][];
                    dir[rec_id][i] = new Vector[LSrc.Samples[i].Length];
                    ///////////////
                    d_mod[rec_id][i] = new double[LSrc.Samples[i].Length];
                    ///////////////

                    double d1 = (R[rec_id] - Utilities.PachTools.RPttoHPt(LSrc.Samples[i][0])).Length(), d2 = (R[rec_id] - Utilities.PachTools.RPttoHPt(LSrc.Samples[i][1])).Length();
                    int t_peak = (d1 - d2) < 0 ? 1 : -1;

                    for (int j = 0; j < LSrc.Samples[i].Length; j++)
                    {
                        tmod[rec_id][i][j] = new double[8];
                        tmod[rec_id][i][j][0] = 1;
                        tmod[rec_id][i][j][1] = 1;
                        tmod[rec_id][i][j][2] = 1;
                        tmod[rec_id][i][j][3] = 1;
                        tmod[rec_id][i][j][4] = 1;
                        tmod[rec_id][i][j][5] = 1;
                        tmod[rec_id][i][j][6] = 1;
                        tmod[rec_id][i][j][7] = 1;

                        Rhino.Geometry.Point3d p = LSrc.Samples[i][j];
                        dir[rec_id][i][j] = R[rec_id] - Utilities.PachTools.RPttoHPt(LSrc.Samples[i][j]);
                        dist[rec_id][i][j] = dir[rec_id][i][j].Length();
                        double tdbl = dist[rec_id][i][j] / C_Sound;
                        tau[rec_id][i][j] = tdbl * SampleFreq;
                        if (RecT[rec_id] < tau[rec_id][i][j]) RecT[rec_id] = (int)tau[rec_id][i][j];
                        if (Time_Pt[rec_id] > tdbl) Time_Pt[rec_id] = tdbl;
                        dir[rec_id][i][j].Normalize();

                        ////////////////////
                        double tanpt;
                        LSrc.Curves[i].ClosestPoint(LSrc.Samples[i][j], out tanpt);
                        d_mod[rec_id][i][j] = Hare_math.Dot(dir[rec_id][i][j], Utilities.PachTools.RPttoHPt((Rhino.Geometry.Point3d)LSrc.Curves[i].TangentAt(tanpt)));
                        d_mod[rec_id][i][j] = Math.Sqrt(1 - d_mod[rec_id][i][j] * d_mod[rec_id][i][j]);
                        ////////////////////

                        if (j != 0)
                        {
                            ///Check for inflection...
                            if (((dist[rec_id][i][j] - dist[rec_id][i][j - 1]) < 0) != (t_peak < 0))
                            {
                                if (j != 1) Inflection[rec_id][i].Add(j);
                                t_peak *= -1;
                            }
                        }


                        Ray D = new Ray(Utilities.PachTools.RPttoHPt(p), dir[rec_id][i][j], 0, RndGen.Next());
                        double x1 = 0, x2 = 0;
                        List<double> t_in;
                        List<int> code;
                        int x3 = 0;
                        List<Hare.Geometry.Point> x4;
                        do
                        {
                            double t = 0;
                            if (Room.shoot(D, out x1, out x2, out x3, out x4, out t_in, out code))
                            {
                                //Point is behind receiver...
                                if (t_in[0] >= dist[rec_id][i][j])
                                {
                                    //Clear connection.
                                    Valid[rec_id][i][j] = true;
                                    break;
                                }
                                else if (Room.IsTransmissive[x3])
                                {
                                    //Semi-transparent veil is in between source and receiver...
                                    t += t_in[0];
                                    D.origin = x4[0];
                                    tmod[rec_id][i][j][0] *= Room.TransmissionValue[x3][0];
                                    tmod[rec_id][i][j][1] *= Room.TransmissionValue[x3][1];
                                    tmod[rec_id][i][j][2] *= Room.TransmissionValue[x3][2];
                                    tmod[rec_id][i][j][3] *= Room.TransmissionValue[x3][3];
                                    tmod[rec_id][i][j][4] *= Room.TransmissionValue[x3][4];
                                    tmod[rec_id][i][j][5] *= Room.TransmissionValue[x3][5];
                                    tmod[rec_id][i][j][6] *= Room.TransmissionValue[x3][6];
                                    tmod[rec_id][i][j][7] *= Room.TransmissionValue[x3][7];
                                    continue;
                                }
                                else
                                {
                                    //obstructed connection.
                                    Valid[rec_id][i][j] = false;
                                    break;
                                }
                            }
                            else
                            {
                                Valid[rec_id][i][j] = true;
                                break;
                            }
                        }
                        while (true);
                    }
                }

                S.WaitOne();
                Special_Status.Instance.progress += 1.0f / R.Count;
                S.Release();
            });

            for (int rec_id = 0; rec_id < R.Count; rec_id++)
            {
                Validity[rec_id] = false;
                Io[rec_id] = new double[RecT[rec_id] + 1, 8];
                //P[rec_id] = new double[RecT[rec_id] + 1, 8];
                Dir_Rec_Pos[rec_id] = new float[8, RecT[rec_id] + 1, 3];
                Dir_Rec_Neg[rec_id] = new float[8, RecT[rec_id] + 1, 3];

                List<MathNet.Numerics.Interpolation.IInterpolation>[][] I_Curve = new List<MathNet.Numerics.Interpolation.IInterpolation>[LSrc.Curves.Count][]; //[Curve][Octave]
                List<double[]>[] Intervals = new List<double[]>[LSrc.Curves.Count];
                double[] Intval;

                for (int i = 0; i < LSrc.Curves.Count; i++)
                {
                    Intval = new double[2] { tau[rec_id][i][0], 0 };
                    I_Curve[i] = new List<MathNet.Numerics.Interpolation.IInterpolation>[8];
                    List<double>[] Io_temp = new List<double>[8];
                    for (int oct = 0; oct < 8; oct++) I_Curve[i][oct] = new List<MathNet.Numerics.Interpolation.IInterpolation>();
                    for (int oct = 0; oct < 8; oct++) Io_temp[oct] = new List<double>();
                    List<double> t = new List<double>();
                    int start = (int)Math.Floor(Time_Pt[rec_id] * SampleFreq);
                    int C_id = 0;
                    Intervals[i] = new List<double[]>();

                    for (int j = 0; j < tau[rec_id][i].Length; j++)
                    {
                        Validity[rec_id] |= Valid[rec_id][i][j];
                        if (!Valid[rec_id][i][j])
                        {
                            if (t.Count == 0)
                            {
                                Intval = new double[2] { tau[rec_id][i][j], 0 };
                                continue;
                            }
                            //Build Spline
                            if (t.Count < 5)
                            {
                                if (t.Count == 1) { t = new List<double> { (tau[rec_id][i][j - 1] + tau[rec_id][i][j]) / 2, (tau[rec_id][i][j] + tau[rec_id][i][j + 1]) / 2 }; }
                                else { t.Add((tau[rec_id][i][j] + tau[rec_id][i][j + 1]) / 2); }

                                Io_temp[0].Add(Io_temp[0][0]);
                                Io_temp[1].Add(Io_temp[1][0]);
                                Io_temp[2].Add(Io_temp[2][0]);
                                Io_temp[3].Add(Io_temp[3][0]);
                                Io_temp[4].Add(Io_temp[4][0]);
                                Io_temp[5].Add(Io_temp[5][0]);
                                Io_temp[6].Add(Io_temp[6][0]);
                                Io_temp[7].Add(Io_temp[7][0]);
                                for (int oct = 0; oct < 8; oct++) I_Curve[i][oct].Add(MathNet.Numerics.Interpolation.LinearSpline.Interpolate(t, Io_temp[oct]));
                            }
                            else
                            {
                                for (int oct = 0; oct < 8; oct++) I_Curve[i][oct].Add(MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t, Io_temp[oct]));
                            }
                            //Reset Spline Template
                            for (int oct = 0; oct < 8; oct++) Io_temp[oct] = new List<double>();
                            t = new List<double>();
                            Intval[1] = tau[rec_id][i][j];
                            Intervals[i].Add(Intval);
                            Intval = new double[2] { tau[rec_id][i][j], 0 };
                            continue;
                        }
                        else if (C_id < Inflection[rec_id][i].Count && Inflection[rec_id][i][C_id] == j)
                        {
                            C_id++;
                            if (t.Count > 0 && t.Count > 4)
                            {
                                for (int oct = 0; oct < 8; oct++) I_Curve[i][oct].Add(MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t, Io_temp[oct]));
                                for (int oct = 0; oct < 8; oct++) Io_temp[oct] = new List<double>();
                                t = new List<double>();
                                Intval[1] = tau[rec_id][i][j];
                                Intervals[i].Add(Intval);
                                Intval = new double[2] { tau[rec_id][i][j], 0 };
                            }
                        }

                        ///Tributary length in samples - divide the energy of the line element up among the samples interpolation will produce...
                        double trib = 0;


                        if (j > 0)
                        {
                            trib += Math.Abs(tau[rec_id][i][j] - tau[rec_id][i][j - 1]) / 2;
                        }
                        else trib += Math.Abs(tau[rec_id][i][j] - tau[rec_id][i][j + 1]) / 2;
                        if (j < tau[rec_id][i].Length - 1)
                        {
                            trib += Math.Abs(tau[rec_id][i][j] - tau[rec_id][i][j + 1]) / 2;
                        }
                        else trib += Math.Abs(tau[rec_id][i][j] - tau[rec_id][i][j - 1]) / 2;

                        if (trib < 1) trib = 1;
                        //trib = C_Sound * tau[rec_id][i][j] * d_mod[rec_id][i][j] / 1000;

                        //TODO: Need to know start and end times for each of these...
                        double[] Io_t = new double[8];
                        Io_t[0] = LSrc.DomainPower[i][0] * Math.Pow(10,-.1 * Room.Attenuation(0)[0] * dist[rec_id][i][j]) * tmod[rec_id][i][j][0] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);
                        Io_t[1] = LSrc.DomainPower[i][1] * Math.Pow(10,-.1 * Room.Attenuation(0)[1] * dist[rec_id][i][j]) * tmod[rec_id][i][j][1] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);
                        Io_t[2] = LSrc.DomainPower[i][2] * Math.Pow(10,-.1 * Room.Attenuation(0)[2] * dist[rec_id][i][j]) * tmod[rec_id][i][j][2] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);
                        Io_t[3] = LSrc.DomainPower[i][3] * Math.Pow(10,-.1 * Room.Attenuation(0)[3] * dist[rec_id][i][j]) * tmod[rec_id][i][j][3] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);
                        Io_t[4] = LSrc.DomainPower[i][4] * Math.Pow(10,-.1 * Room.Attenuation(0)[4] * dist[rec_id][i][j]) * tmod[rec_id][i][j][4] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);
                        Io_t[5] = LSrc.DomainPower[i][5] * Math.Pow(10,-.1 * Room.Attenuation(0)[5] * dist[rec_id][i][j]) * tmod[rec_id][i][j][5] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);
                        Io_t[6] = LSrc.DomainPower[i][6] * Math.Pow(10,-.1 * Room.Attenuation(0)[6] * dist[rec_id][i][j]) * tmod[rec_id][i][j][6] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);
                        Io_t[7] = LSrc.DomainPower[i][7] * Math.Pow(10,-.1 * Room.Attenuation(0)[7] * dist[rec_id][i][j]) * tmod[rec_id][i][j][7] / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j] * trib);

                        bool isgood = true;
                        for (int oct = 0; oct < 8; oct++)
                        {
                            isgood &= !(double.IsInfinity(Io_t[oct]) || double.IsNaN(Io_t[oct]));
                        }

                        if (!isgood) continue;

                        for (int oct = 0; oct < 8; oct++) Io_temp[oct].Add(Io_t[oct]);
                        t.Add(tau[rec_id][i][j]);
                    }

                    if (t.Count < 5) continue;
                    for (int oct = 0; oct < 8; oct++) I_Curve[i][oct].Add(MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t, Io_temp[oct]));
                    Intval[1] = tau[rec_id][i][tau[rec_id][i].Length - 1];
                    Intervals[i].Add(Intval);
                }

                //TODO: Splines built... build interpolated curves.
                for (int i = 0; i < LSrc.Curves.Count; i++)
                {
                    int start = (int)Math.Floor(Time_Pt[rec_id] * SampleFreq);
                    //TODO: Need to know start and end times for each of these...
                    if (I_Curve[i][0].Count != Intervals[i].Count) throw new Exception("Curves and intervals mismatch...");

                    for (int j = 0; j < Intervals[i].Count; j++)
                    {
                        int intvallength = (int)Math.Floor(Intervals[i][j][1] - Intervals[i][j][0]);
                        int dt_mod = intvallength < 0 ? -1 : 1;
                        intvallength *= dt_mod;

                        for (int t_ = 0; t_ < intvallength; t_++)
                        {
                            int time = (int)Math.Floor(Intervals[i][j][0] + dt_mod * t_);

                            int t = time - start;

                            double[] Io_temp = new double[8];
                            for (int oct = 0; oct < 8; oct++)
                            {
                                Io_temp[oct] = I_Curve[i][oct][j].Interpolate((double)time);
                            }
                            for (int oct = 0; oct < 8; oct++) if (!(double.IsInfinity(Io_temp[oct]) || double.IsNaN(Io_temp[oct]))) Io[rec_id][t, oct] += Io_temp[oct];

                            for (int oct = 0; oct < 8; oct++)
                            {
                                if (Io_temp[oct] == 0) continue;
                                Vector V = dir[rec_id][i][j] * Io_temp[oct];

                                if (V.x > 0) Dir_Rec_Pos[rec_id][oct, t, 0] += (float)V.x; else Dir_Rec_Neg[rec_id][oct, t, 0] += (float)V.x;
                                if (V.y > 0) Dir_Rec_Pos[rec_id][oct, t, 1] += (float)V.y; else Dir_Rec_Neg[rec_id][oct, t, 1] += (float)V.y;
                                if (V.z > 0) Dir_Rec_Pos[rec_id][oct, t, 2] += (float)V.z; else Dir_Rec_Neg[rec_id][oct, t, 2] += (float)V.z;

                                //double real, imag;
                                //Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[oct] * dist[rec_id][i][j] / C_Sound + Utilities.Numerics.PiX2 * rnd_in.NextDouble()), out real, out imag);
                                //P[rec_id][t, oct] += (double)(Math.Sqrt(Io[oct] * Room.Rho_C(0)) * real);
                            }
                        }
                    }
                }//);
            }

            Special_Status.Instance.Reset();
            return true;
        }

        public bool Surface_Calculation()
        {
            //Homogeneous media only...
            SurfaceSource Ssrc = Src as SurfaceSource;
            List<Hare.Geometry.Point> R;
        
            R = new List<Hare.Geometry.Point>();
            foreach (Hare.Geometry.Point p in Receiver) R.Add(p);
            Random rnd = new Random();

            int[][][] tau = new int[R.Count][][]; //Rec;Curve;Sample
            double[][][] dist = new double[R.Count][][]; //Rec;Curve;Sample
            int[] RecT = new int[R.Count];
            Io = new double[R.Count][,];
            Validity = new Boolean[R.Count];

            double C_Sound = Room.Sound_speed(0);

            //int MT = MaxT;

            List<int> rndList = new List<int>();
            for (int i = 0; i < R.Count; i++)
            {
                rndList.Add(rnd.Next());
            }

            System.Threading.Semaphore S = new System.Threading.Semaphore(1, 1);

            //for (int k = 0; k < R.Count; k++)
            System.Threading.Tasks.Parallel.For(0, R.Count, k =>
            {
                Random RndGen = new Random(rndList[k]);
                tau[k] = new int[Ssrc.Srfs.Count][];
                dist[k] = new double[Ssrc.Srfs.Count][];
                for (int i = 0; i < Ssrc.Srfs.Count; i++)
                {
                    tau[k][i] = new int[Ssrc.Samples[i].Length];
                    dist[k][i] = new double[Ssrc.Samples[i].Length];
                    Time_Pt[k] = double.MaxValue;

                    for (int j = 0; j < Ssrc.Samples[i].Length; j++)
                    {
                        Rhino.Geometry.Point3d p = Ssrc.Samples[i][j];
                        Vector d = R[k] - Utilities.PachTools.RPttoHPt(Ssrc.Samples[i][j]);
                        dist[k][i][j] = d.Length();
                        double tdbl = dist[k][i][j] / C_Sound;
                        tau[k][i][j] = (int)Math.Ceiling(tdbl * SampleFreq);
                        if (RecT[k] < tau[k][i][j]) RecT[k] = tau[k][i][j];

                        if (Time_Pt[k] > tdbl) Time_Pt[k] = tdbl;

                        d.Normalize();

                        Ray D = new Ray(Utilities.PachTools.RPttoHPt(p), d, 0, RndGen.Next());
                        double x1 = 0, x2 = 0;
                        List<double> t_in;
                        List<int> code;
                        int x3 = 0;
                        List<Hare.Geometry.Point> x4;
                        if (Room.shoot(D, out x1, out x2, out x3, out x4, out t_in, out code))
                        {
                            if (t_in[0] >= dist[k][i][j]) Validity[k] = true; ;
                        }
                        else
                        {
                            Validity[k] = true;
                        }
                    }
                }

                S.WaitOne();
                //foreach (int T in RecT) if (MT < T) MT = T;
                Special_Status.Instance.progress += 1.0f / R.Count;
                S.Release();
            });

            //MaxT = MT;

            for (int rec_id = 0; rec_id < R.Count; rec_id++)
            {
                Io[rec_id] = new double[RecT[rec_id] + 1, 8];
                ////Parallel.For(0, SrcPts.Count, i =>
                for (int i = 0; i < Ssrc.Srfs.Count; i++)
                {
                    for (int j = 0; j < tau[rec_id][i].Length; j++)
                    {
                        double[] Io_t = new double[8];

                        //Io_t[0] = Ssrc.DomainPower[i][0] * Math.Pow(10,-.1 * Room.Attenuation(0)[0] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
                        //Io_t[1] = Ssrc.DomainPower[i][1] * Math.Pow(10,-.1 * Room.Attenuation(0)[1] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
                        //Io_t[2] = Ssrc.DomainPower[i][2] * Math.Pow(10,-.1 * Room.Attenuation(0)[2] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
                        //Io_t[3] = Ssrc.DomainPower[i][3] * Math.Pow(10,-.1 * Room.Attenuation(0)[3] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
                        //Io_t[4] = Ssrc.DomainPower[i][4] * Math.Pow(10,-.1 * Room.Attenuation(0)[4] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
                        //Io_t[5] = Ssrc.DomainPower[i][5] * Math.Pow(10,-.1 * Room.Attenuation(0)[5] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
                        //Io_t[6] = Ssrc.DomainPower[i][6] * Math.Pow(10,-.1 * Room.Attenuation(0)[6] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);
                        //Io_t[7] = Ssrc.DomainPower[i][7] * Math.Pow(10,-.1 * Room.Attenuation(0)[7] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]);

                        Io_t[0] = Math.Pow(10, Ssrc.DomainLevel[i][0] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[0] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;
                        Io_t[1] = Math.Pow(10, Ssrc.DomainLevel[i][1] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[1] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;
                        Io_t[2] = Math.Pow(10, Ssrc.DomainLevel[i][2] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[2] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;
                        Io_t[3] = Math.Pow(10, Ssrc.DomainLevel[i][3] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[3] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;
                        Io_t[4] = Math.Pow(10, Ssrc.DomainLevel[i][4] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[4] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;
                        Io_t[5] = Math.Pow(10, Ssrc.DomainLevel[i][5] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[5] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;
                        Io_t[6] = Math.Pow(10, Ssrc.DomainLevel[i][6] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[6] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;
                        Io_t[7] = Math.Pow(10, Ssrc.DomainLevel[i][7] / 10) * 1E-12 * Math.Exp(-0.2302 * Room.Attenuation(0)[7] * dist[rec_id][i][j]) / (4 * Math.PI * dist[rec_id][i][j] * dist[rec_id][i][j]) / Ssrc.Samples[i].Length;


                        Io[rec_id][tau[rec_id][i][j], 0] += Io_t[0];
                        Io[rec_id][tau[rec_id][i][j], 1] += Io_t[1];
                        Io[rec_id][tau[rec_id][i][j], 2] += Io_t[2];
                        Io[rec_id][tau[rec_id][i][j], 3] += Io_t[3];
                        Io[rec_id][tau[rec_id][i][j], 4] += Io_t[4];
                        Io[rec_id][tau[rec_id][i][j], 5] += Io_t[5];
                        Io[rec_id][tau[rec_id][i][j], 6] += Io_t[6];
                        Io[rec_id][tau[rec_id][i][j], 7] += Io_t[7];

                        //for (int oct = 0; oct < 8; oct++)
                        //{
                        //    double real, imag;
                        //    Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[oct] * dist[rec_id][i][j] / C_Sound + Utilities.Numerics.PiX2 * rnd_in.NextDouble()), out real, out imag);
                        //    P_real[rec_id][tau[rec_id][i][j], oct] += (float)(Math.Sqrt(Io[oct] * Room.Rho_C(0)) * real);
                        //    P_imag[rec_id][tau[rec_id][i][j], oct] += (float)(Math.Sqrt(Io[oct] * Room.Rho_C(0)) * imag);
                        //}
                    }
                }//);
            }
            return true;
        }


}
}