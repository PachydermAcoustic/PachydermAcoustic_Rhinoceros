//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2024, Arthur van der Harten 
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

using Rhino.Commands;
using System.Collections.Generic;
using System;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary>C:\Users\User\Desktop\DEV\PachydermAcoustic_Rhinoceros\Pach_RunSim_Command.cs
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants.
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.)
        /// </summary>
        /// 
        [System.Runtime.InteropServices.Guid("D3381146-DC13-44bb-8382-741669B9C66E")]
        [Rhino.Commands.CommandStyle(Rhino.Commands.Style.Hidden)]
        public class Pach_RunSim_Command : Command
        {
            ///<summary> 
            /// Rhino tracks commands by their unique ID. Every command must have a unique id. 
            /// The Guid created by the project wizard is unique. You can create more Guids using 
            /// the "Create Guid" tool in the Tools menu. 
            ///</summary> 
            ///<returns>The id for this command</returns> 
            public Pach_RunSim_Command()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_RunSim_Command Instance
            {
                get;
                private set;
            }


            ///<returns>The command name as it appears on the Rhino command line</returns>
            public override string EnglishName
            {
                get
                {
                    return "Run_Simulation";
                }
            }

            public Simulation_Type Sim = null;
            public Result CommandResult = Result.Nothing;
            private bool CancelCalc = false;
            public System.Threading.CountdownEvent CD = new System.Threading.CountdownEvent(0);

            ///<summary> This gets called when when the user runs this command.</summary>
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                System.Diagnostics.Process P = System.Diagnostics.Process.GetCurrentProcess();
                P.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

                if (Rhino.RhinoDoc.ActiveDoc.ModelUnitSystem != Rhino.UnitSystem.Meters)
                {
                    Eto.Forms.MessageBox.Show("At this point in development, Pachyderm supports documents in meters only. Please set the document units to meters, and run the calculation again.\n(A quick way to get to the document units control is to type 'units' into the command prompt.)");
                    CommandResult = Result.Cancel;
                    return CommandResult;
                }
                
                ////In order to deal with latency in memory, a thread sleep of 1 second is inserted...
                //System.Threading.Thread.Sleep(1000);
                //////////////////////////////////////////////////////////////////////////////////////
                Rhino.RhinoApp.EscapeKeyPressed += Escape;
                CancelCalc = false;
                CommandResult = Result.Success;
                Rhino.ApplicationSettings.FileSettings.AutoSaveEnabled = false;

                if (Sim != null)
                {
                    Sim.Begin();
                    Rhino.RhinoApp.SetCommandPrompt(string.Format("Initiating {0} Calculation...", Sim.Sim_Type()));
                    do
                    {
                        if (CancelCalc)
                        {
                            Sim.Abort_Calculation();
                            CommandResult = Result.Cancel;
                            Rhino.ApplicationSettings.FileSettings.AutoSaveEnabled = true;
                            return CommandResult;
                        }
                        if (Sim.ThreadState() != System.Threading.ThreadState.Running)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(3000);
                        Rhino.RhinoApp.SetCommandPrompt(Sim.ProgressMsg());
                    } while (true);

                    Sim.Combine_ThreadLocal_Results();
                    do
                    {
                        System.Threading.Thread.Sleep(3000);
                        if (Sim.ThreadState() != System.Threading.ThreadState.Running)
                        {
                            break;
                        }
                        Rhino.RhinoApp.SetCommandPrompt(Sim.ProgressMsg());
                    } while (true);
                }

                Rhino.ApplicationSettings.FileSettings.AutoSaveEnabled = true;
                //System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Interactive;
                Rhino.RhinoDoc.ActiveDoc.Views.RedrawEnabled = true;
                P.PriorityClass = System.Diagnostics.ProcessPriorityClass.Normal;
                return CommandResult;
            }

            private void Escape(object sender, System.EventArgs e)
            {
                CancelCalc = true;
            }

            public void Reset()
            {
                Sim = null;
                CommandResult = Result.Nothing;
            }
        }
    }
}