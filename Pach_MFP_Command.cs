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
//'GNU General Public License for more    details. 
//' 
//'You should have received a copy of the GNU General Public 
//'License along with Pachyderm-Acoustic; if not, write to the Free Software 
//'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA. 

using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Utilities;
using Rhino;
using Rhino.Commands;
using Rhino.Input;
using System;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [System.Runtime.InteropServices.Guid("787DEDD1-D090-4609-BA0D-5FFD5CBCBC69")]
        public class Pach_Mean_Free_Path_Command : Command
        {
            public override string EnglishName
            {
                get { return "Pach_MeanFreePath"; }
            }

            protected override Result RunCommand(RhinoDoc doc, RunMode mode)
            {
                int Samples = 100000;
                Result R = RhinoGet.GetInteger("Number of free paths to sample", true, ref Samples);
                if (R != Result.Success) return R;
                if (Samples < 1) return Result.Cancel;

                // Medium properties do not affect this geometry-only calculation.
                Polygon_Scene Room = RCPachTools.Get_Poly_Scene(50, false, 20, 1013.25, 0, false);

                if (Room == null || !Room.Complete)
                {
                    RhinoApp.WriteLine("A valid Pachyderm polygon scene could not be constructed.");
                    return Result.Failure;
                }

                try
                {
                    double[] PathLength;
                    double HitFraction;
                    AcousticalMath.Mean_Free_Path_Statistics MFP = AcousticalMath.Mean_Free_Path(Room, Samples);

                    RhinoApp.WriteLine("Arithmetic geometric MFP: {0:F3} m", MFP.Arithmetic_MFP);
                    RhinoApp.WriteLine("Reciprocal / collision MFP: {0:F3} m", MFP.Harmonic_MFP);
                    RhinoApp.WriteLine("Mean path time: {0:F2} ms", MFP.Mean_Path_Time * 1000);
                    RhinoApp.WriteLine("Mean collision frequency: {0:F2} collisions/s", MFP.Mean_Collision_Frequency);
                    RhinoApp.WriteLine("Mean collision time: {0:F2} ms", MFP.Mean_Collision_Time * 1000);
                    RhinoApp.WriteLine("Standard deviation: {0:F3} m", MFP.Standard_Deviation);
                    RhinoApp.WriteLine("Relative variance: {0:F3}", MFP.Relative_Variance);
                    RhinoApp.WriteLine("P10 / Median / P90: {0:F3} / {1:F3} / {2:F3} m", MFP.P10, MFP.Median, MFP.P90);

                    return Result.Success;
                }
                catch (Exception Ex)
                {
                    RhinoApp.WriteLine("Mean free path calculation failed: {0}", Ex.Message);
                    return Result.Failure;
                }
            }
        }
    }
}