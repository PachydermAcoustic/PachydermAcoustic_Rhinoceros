//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)
//'
//'This file is part of Pachyderm-Acoustic.
//'
//'Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit
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

using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;
using System;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        /// <summary>
        /// Which glyph previews the conduits draw over source and receiver objects.
        /// shortcut: session-scoped, resets when Rhino restarts; persist to doc.Strings if it must survive a save.
        /// </summary>
        public static class Pach_Preview_State
        {
            public static bool Show_Sources = true;
            public static bool Show_Receivers = true;
        }

        [System.Runtime.InteropServices.Guid("954D9C04-3F86-4E31-8750-99CAAB255B90")]
        public class Pach_Preview_Command : Command
        {
            public override string EnglishName => "Pach_Preview";

            protected override Result RunCommand(RhinoDoc doc, RunMode mode)
            {
                while (true)
                {
                    string prompt = string.Format("Previews: sources {0}, receivers {1}. Toggle:",
                        Pach_Preview_State.Show_Sources ? "on" : "off",
                        Pach_Preview_State.Show_Receivers ? "on" : "off");

                    string choice;
                    using (GetOption go = new GetOption())
                    {
                        go.SetCommandPrompt(prompt);
                        go.AcceptNothing(true);
                        go.AddOption("Sources");
                        go.AddOption("Receivers");
                        go.AddOption("Both");
                        if (go.Get() != GetResult.Option) break;
                        choice = go.Option().EnglishName;
                    }

                    if (choice == "Sources" || choice == "Both") Pach_Preview_State.Show_Sources = !Pach_Preview_State.Show_Sources;
                    if (choice == "Receivers" || choice == "Both") Pach_Preview_State.Show_Receivers = !Pach_Preview_State.Show_Receivers;

                    doc.Views.Redraw();
                }

                return Result.Success;
            }
        }
    }
}
