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

using System;
using System.Configuration;
using System.Drawing;
using System.Collections.Generic;
using Rhino;
using Rhino.Display;
using Rhino.Commands;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        ///</summary> 
        [System.Runtime.InteropServices.Guid("49d3422c-aab5-4eb3-ac15-3796be13482c")]
        public class Pach_Receiver_Object : Command
        {
            public Pach_Receiver_Object()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_Receiver_Object Instance
            {
                get;
                private set;
            }

            public override string EnglishName
            {
                get { return "Insert_Receiver"; }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
            {
                ReceiverConduit m_Receiver_conduit = ReceiverConduit.Instance;
                Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
                ConfigurationSectionCollection S = config.Sections;


                Rhino.Geometry.Point3d location;
                if (Rhino.Input.RhinoGet.GetPoint("Select Source Position", false, out location) != Result.Success) return Result.Cancel;

                Rhino.DocObjects.RhinoObject rhObj = doc.Objects.Find(doc.Objects.AddPoint(location));
                rhObj.Attributes.Name = "Acoustical Receiver";
                rhObj.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(new double[] { 120, 120, 120, 120, 120, 120, 120, 120 }));

                Rhino.RhinoDoc.ActiveDoc.Objects.ModifyAttributes(rhObj, rhObj.Attributes, true);

                m_Receiver_conduit.SetReceiver(rhObj);
                doc.Views.Redraw();

                return Result.Success;
            }
        }

        /// <summary>
        /// Handles the receiver objects, and displays an icon instead of the point object it is based on.
        /// </summary>
        public class ReceiverConduit : Rhino.Display.DisplayConduit
        {
            private bool m_bHandlerAdded = false;
            private List<System.Guid> m_id_list = new List<System.Guid>();
            DisplayBitmap RS;
            DisplayBitmap RU;
            Rhino.Geometry.Line Dir;

            public ReceiverConduit()
            :base()
            {
                System.Drawing.Bitmap RSbmp = Properties.Resources.Receiver_Selected;
                System.Drawing.Bitmap RUbmp = Properties.Resources.Receiver;
                RSbmp.MakeTransparent(System.Drawing.Color.Black);
                RUbmp.MakeTransparent(System.Drawing.Color.Black);
                RS = new DisplayBitmap(RSbmp);
                RU = new DisplayBitmap(RUbmp);

                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static ReceiverConduit Instance
            {
                get;
                private set;
            }

            /// <summary>
            /// Sets up watchers, which trigger events in the handling of the receiver objects.
            /// </summary>
            /// <param name="bAdd"></param>
            private void SetupEventHandlers(bool bAdd)
            {
                if (bAdd)
                {
                    if ((!m_bHandlerAdded))
                    {
                        Rhino.RhinoDoc.AddRhinoObject += OnCopyObject;
                        Rhino.RhinoDoc.DeleteRhinoObject += OnDeleteObject;
                        Rhino.RhinoDoc.ReplaceRhinoObject += OnReplaceObject;
                        Rhino.RhinoDoc.BeginOpenDocument += ClearConduit;
                        Rhino.RhinoDoc.NewDocument += ClearConduit;
                    }
                }
                else
                {
                    Rhino.RhinoDoc.AddRhinoObject -= OnCopyObject;
                    Rhino.RhinoDoc.DeleteRhinoObject -= OnDeleteObject;
                    Rhino.RhinoDoc.ReplaceRhinoObject -= OnReplaceObject;
                    Rhino.RhinoDoc.BeginOpenDocument -= ClearConduit;
                    Rhino.RhinoDoc.NewDocument -= ClearConduit;
                }

                m_bHandlerAdded = bAdd;
            }

            private bool m_bReplaceCalled = false;

            private void ClearConduit(object sender, EventArgs e)
            {
                m_id_list.Clear();
                this.Enabled = false;
                // we don't want to watch for events any more 
                SetupEventHandlers(false);
            }

            private void OnReplaceObject(object sender, Rhino.DocObjects.RhinoReplaceObjectEventArgs e)
            {
                m_bReplaceCalled = true;
            }

            /// <summary>
            /// What to do if an object is copied...
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="rhObject"></param>
            private void OnCopyObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
            {
                if (e.TheObject.Attributes.Name == "Acoustical Receiver") SetReceiver(e.TheObject);
            }

            /// <summary>
            /// What to do if an object is deleted...
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="rhObject"></param>
            private void OnDeleteObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
            {
                // skip this if a replace was called beforehand 
                if ((m_bReplaceCalled))
                {
                    m_bReplaceCalled = false;
                    return;
                }

                foreach (System.Guid m_id in m_id_list)
                {
                    if ((m_id != System.Guid.Empty && e.TheObject != null))
                    {
                        if ((e.TheObject.Attributes.ObjectId == m_id))
                        {
                            m_id_list.Remove(m_id);
                            if (m_id_list.Count < 1)
                            {
                                this.Enabled = false;
                                // we don't want to watch for events any more 
                                SetupEventHandlers(false);
                            }
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// Adds a receiver to the conduit.
            /// </summary>
            /// <param name="rhino_object"></param>
            public void SetReceiver(Rhino.DocObjects.RhinoObject rhino_object)
            {
                if ((rhino_object == null))
                {
                    if (m_id_list.Count == 0)
                    {
                        SetupEventHandlers(false);
                        this.Enabled = false;
                    }
                }
                else
                {
                    SetupEventHandlers(true);
                    this.Enabled = true;
                    if (!m_id_list.Contains(rhino_object.Attributes.ObjectId)) m_id_list.Add(rhino_object.Attributes.ObjectId);
                }
            }

            protected override void DrawForeground(DrawEventArgs e)
            {
                int index = 0;

                if (Dir != null)
                    e.Display.DrawLineArrow(Dir, System.Drawing.Color.Red, 3, .1);

                foreach (Guid G in m_id_list)
                {
                    //Rhino.DocObjects.RhinoObject rhobj = m_pChannelAttrs.m_pObject;
                    Rhino.DocObjects.RhinoObject rhobj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(G);
                    if (rhobj == null)
                    {
                        m_id_list.Remove(G);
                        continue;
                    }
                    // Draw our own representation of the object 
                    Rhino.Geometry.Point2d screen_pt = e.Display.Viewport.WorldToClient(rhobj.Geometry.GetBoundingBox(true).Min);
                    if ((rhobj.IsSelected(false) != 0))
                    {
                        e.Display.DrawSprite(RS, rhobj.Geometry.GetBoundingBox(true).Min, 0.5f, true);// screen_pt, 32.0f);
                        e.Display.Draw2dText(index.ToString(), Color.Yellow, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                    }
                    else
                    {
                        e.Display.DrawSprite(RU, rhobj.Geometry.GetBoundingBox(true).Min, 0.5f, true);// screen_pt, 32.0f);
                        e.Display.Draw2dText(index.ToString(), Color.Black, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                    }
                    index++;
                }
            }

            public void set_direction(Rhino.Geometry.Point3d rec, Rhino.Geometry.Point3d dir)
            {
                Rhino.Geometry.Vector3d V = rec - dir;
                V.Unitize();
                Dir = new Rhino.Geometry.Line(rec, rec + dir);
            }

            public void set_direction(Rhino.Geometry.Point3d rec, Rhino.Geometry.Vector3d V)
            {
                V.Unitize();
                Dir = new Rhino.Geometry.Line(rec, rec + V);
            }

            /// <summary>
            /// The ids of the receiver point objects. Rhino objects all have GUIDs.
            /// </summary>
            public List<System.Guid> UUID
            {
                get
                {
                    return m_id_list;
                }
            }
        }
    }
}