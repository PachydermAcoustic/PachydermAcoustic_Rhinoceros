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

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary>
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants.
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.)
        /// </summary>
        [System.Runtime.InteropServices.Guid("95f0c75b-1882-4493-bc3d-7dd4a92acea7")]
        [Rhino.Commands.CommandStyle(Rhino.Commands.Style.Hidden)]
        public class Pach_Special_Command : Command
        {
            ///<summary> 
            /// Rhino tracks commands by their unique ID. Every command must have a unique id. 
            /// The Guid created by the project wizard is unique. You can create more Guids using 
            /// the "Create Guid" tool in the Tools menu. 
            ///</summary> 
            ///<returns>The id for this command</returns> 
            public Pach_Special_Command()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_Special_Command Instance
            {
                get;
                private set;
            }

            ///<returns>The command name as it appears on the Rhino command line</returns>
            public override string EnglishName
            {
                get
                {
                    return "Pach_Special";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary>
            ///

            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                string special = "";
                Result CommandResult = Rhino.Input.RhinoGet.GetString("Enter a special condition code for Pachyderm simulations: (entering no code resets simulations to default)", true, ref special);
                PachydermAc_PlugIn.Instance.m_Obj_Page.OnActivate(true);

                PachydermAc_PlugIn P = PachydermAc_PlugIn.Instance;
                if (special == "" || special == null)
                {
                    P.SpecialCode = "";
                    Rhino.RhinoApp.WriteLine("Code set to default");
                }
                else
                {
                    P.SpecialCode = special;
                    Rhino.RhinoApp.WriteLine(string.Format("Code set to '{0}'", special));
                }

                return CommandResult;
            }
        }
    }
}