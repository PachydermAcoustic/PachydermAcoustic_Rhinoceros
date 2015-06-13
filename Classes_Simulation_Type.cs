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

namespace Pachyderm_Acoustic
{
    /// <summary>
    /// The simulation type base class. All simulations will inherit this class in order to be passed to the Pach_Runsim_Command.
    /// </summary>
    public abstract class Simulation_Type
    {
        public abstract string Sim_Type();
        public abstract void Begin();
        public abstract string ProgressMsg();
        public abstract System.Threading.ThreadState ThreadState();
        public abstract void Combine_ThreadLocal_Results();
        public abstract void Abort_Calculation();
    }

    /// <summary>
    /// a general purpose structure which holds threadlocal parameters, in order to efficiently divide the work among threads.
    /// </summary>
    public struct Calc_Params
    {
        public Environment.Scene Room;
        public int StartIndex;
        public int EndIndex;
        public int ThreadID;
        public int RandomSeed;

        public Calc_Params(int Start_In, int End_In, int Thread_In, int Random_In)
        {
            Room = null;
            StartIndex = Start_In;
            EndIndex = End_In;
            ThreadID = Thread_In;
            RandomSeed = Random_In;
        }

        public Calc_Params(Environment.Scene Rm, int Start_In, int End_In, int Thread_In, int Random_In)
        {
            Room = Rm;
            StartIndex = Start_In;
            EndIndex = End_In;
            ThreadID = Thread_In;
            RandomSeed = Random_In;
        }

    }
}
