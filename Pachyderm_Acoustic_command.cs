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

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        ///</summary> 
        [System.Runtime.InteropServices.Guid("9d8d728a-98ae-4b80-bf4a-89a160964c05")]
        public class PachyDerm_Acoustic : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Acoustic";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("8559be06-21d7-4535-803e-95a9dd3a2898"));
                return Result.Success;
            }
        }

        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        /// This command shows the controls for the particle animation tool. 
        ///</summary> 
        [System.Runtime.InteropServices.Guid("c55c2833-9b75-4ca5-b666-f0b8b52ad237")]
        public class Pach_Visual_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Animation";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("EA23F0D6-5462-4e42-9CFC-DC8E79723112"));
                return Result.Success;
            }
        }

        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        /// This command shows the controls for the particle animation tool. 
        ///</summary>
        [System.Runtime.InteropServices.Guid("5CB43517-DB4A-4f5e-B7CB-54A79EED3727")]
        public class Pach_Mapping_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Mapping";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("55E14BEE-72F4-4d8c-B751-9BED20A7C5BC"));
                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("3668b612-1002-45ca-867f-94d0f98b741c")]
        public class Pach_CustomMap_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_CustomMapping";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("1c48c00e-abd8-40fd-8642-2ce7daa90ed5"));
                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("a0930289-af08-499e-af91-4d55c583f2b1")]
        public class Pach_TimeDomain_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Numeric_TimeDomain";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("7c62fae6-efa7-4c07-af12-cd440049c7fc"));
                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("aa55b183-cfd1-476e-9ef1-2ab96ea053db")]
        public class Pach_Auralisation_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Auralisation";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("12db68c3-c995-43c6-860a-6bd106b94a4c"));
                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("BA18CA6C-C32E-4532-8525-505C6E1148B4")]

        public class Pach_SpeakerBuilder_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_SpeakerBuilder";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("5CD1A25E-1CC8-4BF2-A103-58CFDA8CF424"));
                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("89DE99D2-E07D-4730-8E98-ED1296B93808")]
        public class Pach_SetBackground_Noise : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Pachyderm_BackgroundNoise";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                double[] freq = new double[]{62.5, 125, 250, 500, 1000, 2000, 4000, 8000};
                double[] noise = new double[8];
                string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");
                
                if (n != "" && n != null)
                {
                    string[] ns = n.Split(","[0]);
                    double t = 0;
                    for (int i = 0; i < 8; i++) if (double.TryParse(ns[i], out t))
                        { noise[i] = t; }
                        else { noise[i] = 0; }
                }

                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        
                        Rhino.Input.RhinoGet.GetNumber(string.Format("Specify background noise sound pressure level at {0} Hertz.", freq[i]), true, ref noise[i]);
                    }
                    catch 
                    {
                        return Result.Nothing;
                    }
                }

                Rhino.RhinoDoc.ActiveDoc.Strings.SetString("Noise", string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", noise[0], noise[1], noise[2], noise[3], noise[4], noise[5], noise[6], noise[7]));

                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("C05DC933-8366-4C81-992A-31A8178C38BD")]
        public class Pach_Absorption : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Procedural_Absorption";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Pach_Absorption_Designer PAD = new Pach_Absorption_Designer();
                PAD.Show();
                return Result.Success;
            }
        }
    }
}