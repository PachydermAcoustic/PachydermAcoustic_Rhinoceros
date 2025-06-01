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
//'GNU General Public License for more details. 
//' 
//'You should have received a copy of the GNU General Public 
//'License along with Pachyderm-Acoustic; if not, write to the Free Software 
//'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA. 

using Rhino;
using System;
using System.Collections.Generic;
using Pachyderm_Acoustic.Environment;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [System.Runtime.InteropServices.Guid("FF42587B-ED11-4257-B012-673ED8364DB4")]
        [Rhino.Commands.CommandStyle(Rhino.Commands.Style.Hidden)]
        public class Pach_GetModel_Command : Rhino.Commands.Command
        {
            public Pach_GetModel_Command()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_GetModel_Command Instance
            {
                get;
                private set;
            }

            public override string EnglishName
            {
                get
                {
                    return "GetModel";
                }
            }

            public int Atten_Choice = 0;
            public double Rel_Humidity = 0;
            public double Atm_pressure = 0;
            public double Air_Temp = 0;
            public bool Edge_Freq = false;
            public bool Register_Edges = false;

            public RhCommon_Scene Ret_NURBS_Scene;
            public Polygon_Scene Ret_Mesh_Scene;

            private Rhino.Commands.Result C_Result;

            protected override Rhino.Commands.Result  RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
            {
                //List<Rhino.DocObjects.RhinoObject> ObjectList = new List<Rhino.DocObjects.RhinoObject>();
                Rhino.DocObjects.ObjectEnumeratorSettings settings = new Rhino.DocObjects.ObjectEnumeratorSettings();
                settings.DeletedObjects = false;
                settings.HiddenObjects = false;
                settings.LockedObjects = true;
                settings.NormalObjects = true;
                settings.VisibleFilter = true;
                settings.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep & Rhino.DocObjects.ObjectType.Surface & Rhino.DocObjects.ObjectType.Extrusion;
                List<Rhino.DocObjects.RhinoObject> RC_List = new List<Rhino.DocObjects.RhinoObject>();
                foreach (Rhino.DocObjects.RhinoObject RHobj in Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(settings))
                {
                    if (RHobj.ObjectType == Rhino.DocObjects.ObjectType.Brep || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Surface || RHobj.ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        RC_List.Add(RHobj);
                    }
                }
                if (RC_List.Count != 0)
                {
                    Ret_NURBS_Scene = new RhCommon_Scene(RC_List, Air_Temp, Rel_Humidity, Atm_pressure, Atten_Choice, Edge_Freq, false);
                    if (!Ret_NURBS_Scene.Valid) return Rhino.Commands.Result.Failure;
                    Ret_Mesh_Scene = new RhCommon_PolygonScene(RC_List, Register_Edges, Air_Temp, Rel_Humidity, Atm_pressure, Atten_Choice, Edge_Freq, false);
                    if (!Ret_Mesh_Scene.Valid) return Rhino.Commands.Result.Failure;
                }
                C_Result = Rhino.Commands.Result.Success;
                return Rhino.Commands.Result.Success;
            }

            public Rhino.Commands.Result CommandResult()
            {
                return C_Result;
            }
        }
    }
}