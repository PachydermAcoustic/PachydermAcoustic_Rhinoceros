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
        public double[][][] Io;//[Rec][Oct][Time_ms]
        public double[][] P;//[Rec][Time_ms]
        public double[][][] Pdir;//[Rec][direction][Time_ms] (0 for x+, 1 for x-, 2 for y+, 3 for y-, 4 for z+, 5 for z-)
        public float[][][][] Dir_Rec_Pos;//[receiver][oct, sample, axis]
        public float[][][][] Dir_Rec_Neg;//[receiver][oct, sample, axis]
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
                BW.Write(Io[q][0].Length);

                //3. Write the validity of the direct sound
                BW.Write(Validity[q]);
                //4. Write the Time point
                BW.Write(Time(q));
                for (int time = 0; time < Io[q][0].Length; time++)
                {
                    //5. Write all Energy data
                    BW.Write(Io[q][0][time]);
                    BW.Write(Io[q][1][time]);
                    BW.Write(Io[q][2][time]);
                    BW.Write(Io[q][3][time]);
                    BW.Write(Io[q][4][time]);
                    BW.Write(Io[q][5][time]);
                    BW.Write(Io[q][6][time]);
                    BW.Write(Io[q][7][time]);

                    //5c. Write all directional data
                    for (int oct = 0; oct < 8; oct++) for (int dir = 0; dir < 3; dir++)
                        {
                            BW.Write(Dir_Rec_Pos[q][oct][time][dir]);
                            BW.Write(Dir_Rec_Neg[q][oct][time][dir]);
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
            D.Io = new double[RecPts.Count<Hare.Geometry.Point>()][][];
            D.Dir_Rec_Pos = new float[RecPts.Count<Hare.Geometry.Point>()][][][];
            D.Dir_Rec_Neg = new float[RecPts.Count<Hare.Geometry.Point>()][][][];

            double v = double.Parse(Version.Substring(0, 3));

            for (int q = 0; q < RecPts.Count<Hare.Geometry.Point>(); q++)
            {
                //2.2 Write number of samples
                int no_of_samples = BR.ReadInt32();
                //3. Write the validity of the direct sound
                D.Validity[q] = BR.ReadBoolean();
                //4. Write the Time point
                D.Time_Pt[q] = BR.ReadDouble();
                D.Io[q] = new double[9][];
                D.Dir_Rec_Pos[q] = new float[8][][];
                D.Dir_Rec_Neg[q] = new float[8][][];

                for (int oct = 0; oct < 8; oct++)
                {
                    D.Io[q][oct] = new double[no_of_samples];
                    D.Dir_Rec_Pos[q][oct] = new float[no_of_samples][];
                    D.Dir_Rec_Neg[q][oct] = new float[no_of_samples][];
                    for (int t = 0; t < no_of_samples; t++)
                    {
                        D.Dir_Rec_Pos[q][oct][t] = new float[3];
                        D.Dir_Rec_Neg[q][oct][t] = new float[3];
                    }
                }

                D.Io[q][8] = new double[no_of_samples];

                for (int s = 0; s < no_of_samples; s++)
                {
                    //5a. Write all Energy data
                    D.Io[q][0][s] = BR.ReadDouble();
                    D.Io[q][1][s] = BR.ReadDouble();
                    D.Io[q][2][s] = BR.ReadDouble();
                    D.Io[q][3][s] = BR.ReadDouble();
                    D.Io[q][4][s] = BR.ReadDouble();
                    D.Io[q][5][s] = BR.ReadDouble();
                    D.Io[q][6][s] = BR.ReadDouble();
                    D.Io[q][7][s] = BR.ReadDouble();
                    D.Io[q][8][s] = D.Io[q][0][s] + D.Io[q][1][s] + D.Io[q][2][s] + D.Io[q][3][s] + D.Io[q][4][s] + D.Io[q][5][s] + D.Io[q][6][s] + D.Io[q][7][s];

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
                            D.Dir_Rec_Pos[q][oct][s][dir] = BR.ReadSingle();
                            D.Dir_Rec_Neg[q][oct][s][dir] = BR.ReadSingle();
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
            Io = new double[Rec_in.Count][][];//[Rec_in.Count][t,8];
            Time_Pt = new double[Rec_in.Count];//[Rec_in.Count];
            Dir_Rec_Pos = new float[Rec_in.Count][][][];
            Dir_Rec_Neg = new float[Rec_in.Count][][][];
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
                Io = new double[Receiver.Count][][];
                //Phase = new double[Receiver.Count][,];
                Time_Pt = new double[Receiver.Count];
                //P = new double[Receiver.Count][];

                for (int i = 0; i < Receiver.Count; i++)
                {
                    Io[i] = new double[8][];
                    Dir_Rec_Pos[i] = new float[8][][];
                    Dir_Rec_Neg[i] = new float[8][][];
                    for (int oct = 0; oct < 8; oct++)
                    {
                        Io[i][oct] = new double[1];
                        Dir_Rec_Pos[i][oct] = new float[1][];
                        Dir_Rec_Neg[i][oct] = new float[1][];
                        Dir_Rec_Pos[i][oct][0] = new float[3];
                        Dir_Rec_Neg[i][oct][0] = new float[3];
                    }
                    //P[i] = new double[1];
                    double[] transmod;
                    Check_Validity(i, rnd.Next(), out transmod);
                    Rho_C[i] = Room.Rho_C(Receiver[i]);

                    double Length = Src.Origin().DistanceTo(Utilities.PachTools.HPttoRPt(Receiver[i]));
                    Vector dir = Receiver[i] - Src.H_Origin();
                    dir.Normalize();

                    double[] Power = Src.DirPower(0, rnd.Next(), dir);
                    double[] phase_in = Src.Phase(dir, ref rnd);
                    //for (int o = 0; o < 8; o++) Phase[i][0, o] = phase_in[o];

                    Io[i][0][0] = Power[0] * Math.Pow(10, -.1 * Room.Attenuation(0)[0] * Length) * transmod[0] / (4 * Math.PI * Length * Length);
                    Io[i][1][0] = Power[1] * Math.Pow(10, -.1 * Room.Attenuation(0)[1] * Length) * transmod[1] / (4 * Math.PI * Length * Length);
                    Io[i][2][0] = Power[2] * Math.Pow(10, -.1 * Room.Attenuation(0)[2] * Length) * transmod[2] / (4 * Math.PI * Length * Length);
                    Io[i][3][0] = Power[3] * Math.Pow(10, -.1 * Room.Attenuation(0)[3] * Length) * transmod[3] / (4 * Math.PI * Length * Length);
                    Io[i][4][0] = Power[4] * Math.Pow(10, -.1 * Room.Attenuation(0)[4] * Length) * transmod[4] / (4 * Math.PI * Length * Length);
                    Io[i][5][0] = Power[5] * Math.Pow(10, -.1 * Room.Attenuation(0)[5] * Length) * transmod[5] / (4 * Math.PI * Length * Length);
                    Io[i][6][0] = Power[6] * Math.Pow(10, -.1 * Room.Attenuation(0)[6] * Length) * transmod[6] / (4 * Math.PI * Length * Length);
                    Io[i][7][0] = Power[7] * Math.Pow(10, -.1 * Room.Attenuation(0)[7] * Length) * transmod[7] / (4 * Math.PI * Length * Length);

                    float time = (float)(Length / C_Sound) + (float)Src.Delay;
                    //float real, imag;

                    for (int oct = 0; oct < 8; oct++)
                    {
                        //Utilities.Numerics.ExpComplex(0, (float)(Utilities.Numerics.angularFrequency[oct] * time + phase_in[oct]), out real, out imag);
                        //P[i][0, oct] = Math.Sqrt(Io[i][0, oct] * Room.Rho_C(Receiver[i]));
                        Vector V = dir * Io[i][oct][0];

                        if (V.x > 0) Dir_Rec_Pos[i][oct][0][0] += (float)V.x; else Dir_Rec_Neg[i][oct][0][0] += (float)V.x;
                        if (V.y > 0) Dir_Rec_Pos[i][oct][0][1] += (float)V.y; else Dir_Rec_Neg[i][oct][0][1] += (float)V.y;
                        if (V.z > 0) Dir_Rec_Pos[i][oct][0][2] += (float)V.z; else Dir_Rec_Neg[i][oct][0][2] += (float)V.z;
                    }

                    Time_Pt[i] = Length / C_Sound + Src.Delay;
                }
            }
        }

        public double[] Dir_Pressure(int Rec_ID, double alt, double azi, bool degrees)
        {
            double[] Pn = new double[P[Rec_ID].Length];
                if (Dir_Rec_Pos[Rec_ID][0].Length > 1)
                {
                    for (int i = 0; i < Pdir[Rec_ID].Length; i++)
                    {
                        double[] Eo = new double[8];
                        Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][2]));
                        Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(D, azi, 0, degrees), 0, alt, degrees);
                        Vn.Normalize();
                        if (Vn.x > 0)
                        {
                            for (int octave = 0; octave < 8; octave++)
                            {
                                Eo[octave] = Math.Sqrt(Io[Rec_ID][octave][i] * Rho_C[Rec_ID]) ;
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
                    Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][2]));
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
                if (Dir_Rec_Pos[Rec_ID][0].Length > 1)
                {
                    for (int i = 0; i < Pn.Length; i++) Pn[i] = new double[3];

                    for (int i = 0; i < Pdir[Rec_ID].Length; i++)
                    {
                        double[] Eo = new double[8];
                        Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][2]));
                        Vector Vn = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(D, azi, 0, degrees), 0, alt, degrees);
                        Vn.Normalize();
                        if (Vn.x > 0)
                        {
                            for (int octave = 0; octave < 8; octave++)
                            {
                                Eo[octave] = Math.Sqrt(Io[Rec_ID][octave][i] * Rho_C[Rec_ID]);
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
                    Vector D = new Vector(Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][0]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][0]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][1]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][1]), Math.Abs(Dir_Rec_Pos[Rec_ID][5][0][2]) - Math.Abs(Dir_Rec_Neg[Rec_ID][5][0][2]));
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
            if (t < 0 || t > Io[Rec_ID][octave].Length) return 0;

            if (octave > 7) return EnergySum(Rec_ID, t);

            if (t < Io[Rec_ID][octave].Length)
            {
                return Io[Rec_ID][octave][t];
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
            if (t < Io[Rec_ID][0].Length)
            {
                return Io[Rec_ID][0][t] + Io[Rec_ID][1][t] + Io[Rec_ID][2][t] + Io[Rec_ID][3][t] + Io[Rec_ID][4][t] + Io[Rec_ID][5][t] + Io[Rec_ID][6][t] + Io[Rec_ID][7][t];
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
            double[] E = new double[Io[Rec_ID][octave].Length]; 

            for (int i = 0; i < Io[Rec_ID][octave].Length; i++)
            {
                E[i] = Io[Rec_ID][octave][i];
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
            for(int i = 0; i < Io[Rec_ID][0].Length; i++)
            {
                E[i] = Io[Rec_ID][0][i] + Io[Rec_ID][1][i] + Io[Rec_ID][2][i] + Io[Rec_ID][3][i] + Io[Rec_ID][4][i] + Io[Rec_ID][5][i] + Io[Rec_ID][6][i] + Io[Rec_ID][7][i];
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
            int length = Dir_Rec_Pos[Rec_Index][Octave].Length;
            Vector[] V = new Vector[length];

            for (int i = 0; i < length; i++)
            {
                V[i] = new Vector(Dir_Rec_Pos[Rec_Index][Octave][i][0], Dir_Rec_Pos[Rec_Index][Octave][i][1], Dir_Rec_Pos[Rec_Index][Octave][i][2]);
                V[i] = Utilities.PachTools.Rotate_Vector(Utilities.PachTools.Rotate_Vector(V[i], azi, 0, degrees), 0, alt, degrees);
            }
            return V;
        }

        public virtual Vector[] Directions_Neg(int Octave, int Rec_Index, double alt, double azi, bool degrees)
        {
            int length = Dir_Rec_Neg[Rec_Index][Octave].Length;
            Vector[] V = new Vector[length];

            for (int i = 0; i < length; i++)
            {
                V[i] = new Vector(Dir_Rec_Neg[Rec_Index][Octave][i][0], Dir_Rec_Neg[Rec_Index][Octave][i][1], Dir_Rec_Neg[Rec_Index][Octave][i][2]);
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
                    return Dir.x * this.Io[Rec_ID][Octave][0];
                case 1:
                    return Dir.y * this.Io[Rec_ID][Octave][0];
                case 2:
                    return Dir.z * this.Io[Rec_ID][Octave][0];
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
                V[i] *= Io[Rec_ID][Octave][0];
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
                V[i] *= Io[Rec_ID][Octave][i];
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
            
            Vector[] D = new Vector[Dir_Rec_Pos[Rec_ID][0].Length];
            for (int o = 0; o < 8; o++)
            {
                Vector[] Vf = new Vector[Dir_Rec_Pos[Rec_ID][0].Length];
                Vector[] Vpos = Directions_Pos(o, Rec_ID, alt, azi, false);
                Vector[] Vneg = Directions_Neg(o, Rec_ID, alt, azi, false);
                for (int i = 0; i < D.Length; i++)
                {
                    Vf[i] = Vpos[i] - Vneg[i];
                    Vf[i].Normalize();
                    Vf[i] *= Io[Rec_ID][o][i];
                    D[i] += Vf[i];
                }
            } 
            return D;
        }

        public Vector[] Dir_Energy_Sum(int Rec_ID, double alt, double azi, bool degrees)
        {
            Vector[] D = new Vector[Dir_Rec_Pos[Rec_ID][0].Length];
            for(int i = 0; i < D.Length; i++) D[i] = new Vector();
            for (int o = 0; o < 8; o++)
            {
                Vector[] Vf = new Vector[Dir_Rec_Pos[Rec_ID][0].Length];
                Vector[] Vpos = Directions_Pos(o, Rec_ID, alt, azi, false);
                Vector[] Vneg = Directions_Neg(o, Rec_ID, alt, azi, false);
                for (int i = 0; i < D.Length; i++)
                {
                    Vf[i] = Vpos[i] - Vneg[i];
                    Vf[i].Normalize();
                    Vf[i] *= Io[Rec_ID][o][i];
                    D[i] += Vf[i];
                }
            }
            return D;
        }

        public void Create_Pressure()
        {
            P = new double[Receiver.Count][];
            Pdir = new double[Receiver.Count][][];
            double scale = Math.Sqrt(4096);

            for (int i = 0; i < Receiver.Count; i++)
            {
                double[][] ETC = new double[8][];

                for (int oct = 0; oct < 8; oct++) ETC[oct] = new double[Io[i][0].Length];

                for (int t = 0; t < ETC[0].Length; t++)
                {
                    for (int oct = 0; oct < 8; oct++) ETC[oct][t] = Math.Sqrt(Io[i][oct][t] * Rho_C[i]);
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
                        vpos += new Vector(Math.Abs(Dir_Rec_Pos[i][oct][t][0]), Math.Abs(Dir_Rec_Pos[i][oct][t][1]), Math.Abs(Dir_Rec_Pos[i][oct][t][2]));
                        vneg += new Vector(Math.Abs(Dir_Rec_Neg[i][oct][t][0]), Math.Abs(Dir_Rec_Neg[i][oct][t][1]), Math.Abs(Dir_Rec_Neg[i][oct][t][2]));
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

        private void Record_Line_Segment(ref List<double> t, ref List<double[]> I, ref List<Vector> d, int rec_id)
        {
            if (t.Count > 4)
            {
                double[] t_dump = new double[t.Count];
                double[][] I_dump = new double[8][];
                double[][] xp_dump = new double[8][];
                double[][] xn_dump = new double[8][];
                double[][] yp_dump = new double[8][];
                double[][] yn_dump = new double[8][];
                double[][] zp_dump = new double[8][];
                double[][] zn_dump = new double[8][];

                double dt = 1d / (double)SampleFreq;

                double tmin = double.PositiveInfinity;
                double tmax = double.NegativeInfinity;
                for (int oct = 0; oct < 8; oct++)
                {
                    I_dump[oct] = new double[t.Count];
                    xp_dump[oct] = new double[t.Count];
                    xn_dump[oct] = new double[t.Count];
                    yp_dump[oct] = new double[t.Count];
                    yn_dump[oct] = new double[t.Count];
                    zp_dump[oct] = new double[t.Count];
                    zn_dump[oct] = new double[t.Count];
                }

                for (int i = 0; i < t.Count; i++)
                {
                    Vector v = d[i];
                    t_dump[i] = t[i];
                    tmin = Math.Min(t[i], tmin);
                    tmax = Math.Max(t[i], tmax);
                    double log10Eps = Math.Log10(1E-12);

                    for (int oct = 0; oct < 8; oct++)
                    {
                        I_dump[oct][i] = Math.Log10(I[i][oct]);
                        if (v.x > 0)
                        {
                            xp_dump[oct][i] = Math.Log10(Math.Abs(I[i][oct] * v.x));
                            xn_dump[oct][i] = log10Eps;
                        }
                        else
                        {
                            xn_dump[oct][i] = Math.Log10(Math.Abs(I[i][oct] * v.x));
                            xp_dump[oct][i] = log10Eps;
                        }
                        if (v.y > 0)
                        {
                            yp_dump[oct][i] = Math.Log10(Math.Abs(I[i][oct] * v.y));
                            yn_dump[oct][i] = log10Eps;
                        }
                        else
                        {
                            yn_dump[oct][i] = Math.Log10(Math.Abs(I[i][oct] * v.y));
                            yp_dump[oct][i] = log10Eps;
                        }
                        if (v.z > 0)
                        {
                            zp_dump[oct][i] = Math.Log10(Math.Abs(I[i][oct] * v.z));
                            zn_dump[oct][i] = log10Eps;
                        }
                        else
                        {
                            zn_dump[oct][i] = Math.Log10(Math.Abs(I[i][oct] * v.z));
                            zp_dump[oct][i] = log10Eps;
                        }
                    }
                }

                MathNet.Numerics.Interpolation.CubicSpline[] I_Spline = new MathNet.Numerics.Interpolation.CubicSpline[8];
                MathNet.Numerics.Interpolation.CubicSpline[] xp_Spline = new MathNet.Numerics.Interpolation.CubicSpline[8];
                MathNet.Numerics.Interpolation.CubicSpline[] xn_Spline = new MathNet.Numerics.Interpolation.CubicSpline[8];
                MathNet.Numerics.Interpolation.CubicSpline[] yp_Spline = new MathNet.Numerics.Interpolation.CubicSpline[8];
                MathNet.Numerics.Interpolation.CubicSpline[] yn_Spline = new MathNet.Numerics.Interpolation.CubicSpline[8];
                MathNet.Numerics.Interpolation.CubicSpline[] zp_Spline = new MathNet.Numerics.Interpolation.CubicSpline[8];
                MathNet.Numerics.Interpolation.CubicSpline[] zn_Spline = new MathNet.Numerics.Interpolation.CubicSpline[8];

                for (int oct = 0; oct < 8; oct++)
                {
                    I_Spline[oct] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t_dump, I_dump[oct]);
                    xp_Spline[oct] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t_dump, xp_dump[oct]);
                    xn_Spline[oct] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t_dump, xn_dump[oct]);
                    yp_Spline[oct] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t_dump, yp_dump[oct]);
                    yn_Spline[oct] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t_dump, yn_dump[oct]);
                    zp_Spline[oct] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkima(t_dump, zp_dump[oct]);
                    zn_Spline[oct] = MathNet.Numerics.Interpolation.CubicSpline.InterpolateAkimaSorted(t_dump, zn_dump[oct]);
                }
                int taumin = (int)Math.Ceiling(tmin / dt);
                int taumax = (int)Math.Floor(tmax / dt);

                if (Io[rec_id][0].Length < taumax)
                {
                    int tau_present = Io[rec_id][0].Length;
                    //resize the intensity histograms...
                    for (int oct = 0; oct < 8; oct++)
                    {
                        Array.Resize(ref Io[rec_id][oct], taumax);
                        for (int j = tau_present; j < Io[rec_id][oct].Length; j++)
                        {
                            Dir_Rec_Pos[rec_id][oct][j] = new float[3];
                            Dir_Rec_Neg[rec_id][oct][j] = new float[3];
                        }
                    }
                    Array.Resize(ref Io[rec_id][8], taumax);
                }

                //Adjust for energy differential between what might be two different sample rates...
                double mod = (double)(taumax - taumin) / t.Count;

                for (int tau = taumin; tau < taumax; tau++)
                {
                    for (int oct = 0; oct < 8; oct++)
                    {
                        double tdbl = (double)tau * dt;
                        double spl = I_Spline[oct].Interpolate(tdbl);
                        Io[rec_id][oct][tau] += Math.Pow(10, spl);// * mod;

                        if (Io[rec_id][oct][tau] < 0 || double.IsInfinity(Io[rec_id][oct][tau]) || double.IsNaN(Io[rec_id][oct][tau]))
                        {
                            Rhino.RhinoApp.Write("MEEP");
                        }

                        this.Dir_Rec_Pos[rec_id][oct][tau][0] += (float)(Math.Pow(10, xp_Spline[oct].Interpolate(tdbl)) * mod);
                        this.Dir_Rec_Neg[rec_id][oct][tau][0] += (float)(Math.Pow(10, xn_Spline[oct].Interpolate(tdbl)) * mod);
                        this.Dir_Rec_Pos[rec_id][oct][tau][1] += (float)(Math.Pow(10, yp_Spline[oct].Interpolate(tdbl)) * mod);
                        this.Dir_Rec_Neg[rec_id][oct][tau][1] += (float)(Math.Pow(10, yn_Spline[oct].Interpolate(tdbl)) * mod);
                        this.Dir_Rec_Pos[rec_id][oct][tau][2] += (float)(Math.Pow(10, zp_Spline[oct].Interpolate(tdbl)) * mod);
                        this.Dir_Rec_Neg[rec_id][oct][tau][2] += (float)(Math.Pow(10, zn_Spline[oct].Interpolate(tdbl)) * mod);
                    }
                }
            }
            else
            {
                Rhino.RhinoApp.Write("MEEP");
            }

            t = new List<double>();
            I = new List<double[]>();
            d = new List<Vector>();
        }

        private bool Line_Calculation()
        {
            Random RndGen = new Random();
            LineSource LSrc = Src as LineSource;
            double dmod = C_Sound / SampleFreq;

            int[] rnd = new int[Receiver.Count];
            for (int i = 0; i < Receiver.Count; i++) rnd[i] = RndGen.Next();
            Rhino.Geometry.BoundingBox b = LSrc.Curve.GetBoundingBox(true);

            //for (int i = 0; i < Receiver.Count; i++)
            System.Threading.Tasks.Parallel.For(0, Receiver.Count, i =>
            {
                Random RAND = new Random(rnd[i]);
                double mintime = double.PositiveInfinity, maxtime = double.NegativeInfinity;

                Io[i] = new double[9][];
                Dir_Rec_Pos[i] = new float[8][][];
                Dir_Rec_Neg[i] = new float[8][][];
                int BufferLength = (int)Math.Ceiling((Receiver[i] - Utilities.PachTools.RPttoHPt(b.FurthestPoint(Utilities.PachTools.HPttoRPt(Receiver[i])))).Length() * SampleFreq / C_Sound);
                for (int oct = 0; oct < 8; oct++)
                {
                    Io[i][oct] = new double[BufferLength];
                    Dir_Rec_Pos[i][oct] = new float[BufferLength][];
                    Dir_Rec_Neg[i][oct] = new float[BufferLength][];
                    for (int t = 0; t < BufferLength; t++)
                    {
                        Dir_Rec_Pos[i][oct][t] = new float[3];
                        Dir_Rec_Neg[i][oct][t] = new float[3];
                    }
                }

                double pos;
                LSrc.Curve.ClosestPoint(Utilities.PachTools.HPttoRPt(Receiver[i]), out pos);
                Rhino.Geometry.Point3d pt = LSrc.Curve.PointAt(pos);
                Vector v = Receiver[0] - Utilities.PachTools.RPttoHPt(pt);
                double d = v.Length();
                double dx = 1;// 5 * Math.Acos(d / (d + dmod)); //1
                double trib = (dx / C_Sound) * SampleFreq;
                Rhino.Geometry.Point3d[] Samples = LSrc.Curve.DivideEquidistant(dx);
                Point[] SamplesH = new Point[Samples.Length];
                for (int j = 0; j < SamplesH.Length; j++) SamplesH[j] = Utilities.PachTools.RPttoHPt(Samples[j]);
                double tprev = 0;

                double d1 = (Receiver[i] - Utilities.PachTools.RPttoHPt(LSrc.Samples[0])).Length(), d2 = (Receiver[i] - Utilities.PachTools.RPttoHPt(LSrc.Samples[1])).Length();
                int t_incline = (d2 - d1) < 0 ? 1 : -1;

                List<double> time = new List<double>();
                List<double[]> I = new List<double[]>();
                List<Vector> I_d = new List<Vector>();

                foreach (Rhino.Geometry.Point3d p in Samples)
                {
                    Point p_H = Utilities.PachTools.RPttoHPt(p);

                    Vector dir = Receiver[i] - p_H;
                    double dist = dir.Length();
                    double tdbl = dist / C_Sound;

                    double tau = tdbl * SampleFreq;
                    mintime = Math.Min(tdbl, mintime);
                    maxtime = Math.Max(tdbl, maxtime);

                    dir.Normalize();
                    double[] W_temp = LSrc.DirPower(0, RAND.Next(), dir, p);

                    if (tprev != 0)
                    {
                        ///Check for inflection...
                        if (((tdbl - tprev) > 0) != (t_incline < 0))
                        {
                            if (I.Count > 0) Record_Line_Segment(ref time, ref I, ref I_d, i);
                            t_incline *= -1;
                        }
                    }

                    tprev = tdbl;

                    //Check for occlusions and transparencies
                    Ray D = new Ray(Utilities.PachTools.RPttoHPt(p), dir, 0, RAND.Next());
                    double x1 = 0, x2 = 0;
                    List<double> t_in;
                    List<int> code;
                    int x3 = 0;
                    List<Hare.Geometry.Point> x4;
                    do
                    {
                        //Point is behind receiver...
                        if (!Room.shoot(D, out x1, out x2, out x3, out x4, out t_in, out code) || t_in[0] >= dist)
                        {
                            //Clear connection.
                            for (int oct = 0; oct < 8; oct++) W_temp[oct] *= Math.Pow(10, -.1 * Room.Attenuation(0)[oct] * dist) / (4 * Math.PI * dist * dist * trib);
                            I_d.Add(dir);
                            I.Add(W_temp);
                            time.Add(tdbl);
                            break;
                        }
                        else if (Room.IsTransmissive[x3])
                        {
                            //Semi-transparent veil is in between source and receiver...
                            D.origin = x4[0];
                            for(int oct = 0; oct < 8; oct++) W_temp[oct] *= Room.TransmissionValue[x3][oct];
                            continue;
                        }
                        else
                        {
                            //obstructed connection.
                            if (time.Count > 0) Record_Line_Segment(ref time, ref I, ref I_d, i);
                            break;
                        }
                    }
                    while (true);
                }

                if (time.Count > 0) Record_Line_Segment(ref time, ref I, ref I_d, i);

                //Finalize Receiver point
                Time_Pt[i] = mintime;
                int start = (int)Math.Floor(mintime * SampleFreq);
                int newlength = (int)Math.Ceiling((maxtime) * SampleFreq) - start;
                double total = 0;

                Io[i][8] = new double[newlength];

                for (int oct = 0; oct < 8; oct++)
                {
                    double[] Iotemp = Io[i][oct];
                    float[][] DRP_temp = Dir_Rec_Pos[i][oct];
                    float[][] DRN_temp = Dir_Rec_Neg[i][oct];

                    Io[i][oct] = new double[newlength];
                    Dir_Rec_Pos[i][oct] = new float[newlength][];
                    Dir_Rec_Neg[i][oct] = new float[newlength][];

                    int l = Math.Min(newlength, Iotemp.Length - start);

                    for (int t = 0; t < l; t++)
                    {
                        total += Iotemp[(t + start)];
                        Io[i][oct][t] = Iotemp[(t + start)];
                        Io[i][8][t] += Io[i][oct][t];
                        Dir_Rec_Pos[i][oct][t] = DRP_temp[(t + start)];
                        Dir_Rec_Neg[i][oct][t] = DRN_temp[(t + start)];
                    }
                }
                Validity[i] = (total != 0);
            });

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
            Io = new double[R.Count][][];
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
                Io[rec_id] = new double[8][];
                for(int oct = 0; oct < 8; oct++)
                {
                    Io[rec_id][oct] = new double[RecT[rec_id] + 1];
                }
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


                        Io[rec_id][0][tau[rec_id][i][j]] += Io_t[0];
                        Io[rec_id][1][tau[rec_id][i][j]] += Io_t[1];
                        Io[rec_id][2][tau[rec_id][i][j]] += Io_t[2];
                        Io[rec_id][3][tau[rec_id][i][j]] += Io_t[3];
                        Io[rec_id][4][tau[rec_id][i][j]] += Io_t[4];
                        Io[rec_id][5][tau[rec_id][i][j]] += Io_t[5];
                        Io[rec_id][6][tau[rec_id][i][j]] += Io_t[6];
                        Io[rec_id][7][tau[rec_id][i][j]] += Io_t[7];
                    }
                }
            }
            return true;
        }


}
}