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
using System.Collections.Generic;
using Rhino.Geometry;
using System.Runtime.InteropServices;
using Rhino.UI;
using Eto.Forms;
using Eto.Drawing;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("1c48c00e-abd8-40fd-8642-2ce7daa90ed5")]
        public class Pach_MapCustom : Panel, IPanel
        {
            double Current_Min;
            double Current_Max;
            Pach_Graphics.Colorscale c_scale;
            List<ParaMesh> Collection = new List<ParaMesh>();

            private Color_Output_Control color_control;
            internal GroupBox TimeBox;
            internal Label ETlabel;
            internal Label STlabel;
            private Label Par_Sel_Lbl;
            private Label ColorSel_Lbl;
            private CheckBox Discretize;
            private NumericStepper End_Time_Control;
            private NumericStepper Start_Time_Control;
            private Label Sel_Lbl;
            private SegmentedButton FileMenu;
            private MenuSegmentedItem fileToolStripMenuItem;
            private ButtonMenuItem saveDataToolStripMenuItem;
            internal Button Plot_Values;
            internal Button Calculate_Map;
            private ListBox MeshList;

            public Pach_MapCustom()
            {
                this.TimeBox = new GroupBox();
                this.ETlabel = new Label();
                this.STlabel = new Label();
                this.Par_Sel_Lbl = new Label();
                this.ColorSel_Lbl = new Label();
                this.End_Time_Control = new NumericStepper();
                this.Start_Time_Control = new NumericStepper();
                this.Sel_Lbl = new Label();
                this.FileMenu = new SegmentedButton();
                this.fileToolStripMenuItem = new MenuSegmentedItem();
                this.saveDataToolStripMenuItem = new ButtonMenuItem();
                this.MeshList = new ListBox();
                this.color_control = new Color_Output_Control(true);

                this.TimeBox.Text = "Time Interval";
                DynamicLayout TDL = new DynamicLayout();
                this.STlabel.Text = "Start Time (ms)";
                this.Start_Time_Control.MaxValue = 10000;
                this.ETlabel.Text = "End Time (ms)";
                this.End_Time_Control.MaxValue = 10000;
                this.End_Time_Control.Value = 50;
                TDL.AddRow(this.STlabel, null, Start_Time_Control);
                TDL.AddRow(this.ETlabel, null, End_Time_Control);
                this.TimeBox.Content = TDL;

                this.Sel_Lbl.Text = "Selection";

                this.saveDataToolStripMenuItem.Text = "Save Data...";
                this.saveDataToolStripMenuItem.Click += SaveDataToolStripMenuItem_Click;
                this.fileToolStripMenuItem.Menu = new ContextMenu()
                {
                    Items = { this.saveDataToolStripMenuItem }
                };
                this.fileToolStripMenuItem.Text = "File";
                FileMenu.Items.Add(fileToolStripMenuItem);

                this.Plot_Values = new Button();
                this.Calculate_Map = new Button();


                this.Plot_Values.Text = "Plot Numerical Values";
                this.Plot_Values.Click += Plot_Values_Click;

                this.Calculate_Map.Text = "Create Map";
                this.Calculate_Map.Click += Create_Map_Click;

                DynamicLayout Layout = new DynamicLayout();
                DynamicLayout FM = new DynamicLayout();
                FM.AddRow(FileMenu, null);
                Layout.AddRow(FM);
                Layout.AddRow(MeshList);
                Layout.AddRow(color_control);
                Layout.AddRow(TimeBox);
                Layout.AddRow(Calculate_Map);
                Layout.AddRow(Plot_Values);
                this.Content = Layout;

                Instance = this;
            }

            public void Create_Map()
            {
                //Update_Scale();
                ParaMesh P = Collection[MeshList.SelectedIndex];
                Utilities.RCPachTools.CreateMap(P.M, color_control.Color_ID, P.Params, color_control.Min, color_control.Max);
            }

            public static Pach_MapCustom Instance
            {
                get;
                private set;
            }

            public static void Clear()
            {
                Instance.Collection.Clear();
                Instance.MeshList.Items.Clear();
            }

            private void Create_Map_Click(object sender, EventArgs e)
            {
                Create_Map();
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }

            private void Plot_Values_Click(object sender, EventArgs e)
            {
                //Map.Plot_Values(new double[] { Current_SPLMin, Current_SPLMax }, new double[] { (double)End_Time_Control.Value, (double)Start_Time_Control.Value }, PachTools.OctaveStr2Int(Octave.Text));
            }

            public static void Add_Result(string Title, double[] Values, Mesh M)
            {
                Instance.MeshList.Items.Add(Title);
                Instance.MeshList.SelectedIndex = 0;
                Instance.Collection.Add(new ParaMesh(Title, Values, M));
            }


            private class ParaMesh
            {
                public bool Valid;
                public string Title;
                public Mesh M;
                public double[] Params;

                public ParaMesh(string Title_in, double[] p_in, Mesh m_in)
                {
                    Valid = (p_in.Length == m_in.Vertices.Count);
                    M = m_in;
                    Params = p_in;
                    Title = Title_in;
                }
            }

            private void SaveDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                PachydermAc_PlugIn plugin = PachydermAc_PlugIn.Instance;
                Eto.Forms.SaveFileDialog sf = new Eto.Forms.SaveFileDialog();
                sf.CurrentFilter = ".pccm";
                sf.Filters.Add("Pachyderm Custom Map file (*.pccm)|*.pccm|" + "All Files|");
                if (sf.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == Eto.Forms.DialogResult.Ok)
                {
                    System.IO.BinaryWriter sw = new System.IO.BinaryWriter(System.IO.File.Open(sf.FileName, System.IO.FileMode.Create));
                    //1. Date & Time (String)
                    sw.Write(System.DateTime.Now.ToString());
                    //2. Plugin Version (String)
                    sw.Write(plugin.Version);
                    //3. Number of Parameter Meshes (int)
                    sw.Write(Collection.Count);

                    for (int t = 0; t < Collection.Count; t++)
                    {
                        //4. Title (string)
                        sw.Write(Collection[t].Title);
                        //5. Number of parameters/Vertices (int)
                        int VertCt = Collection[t].Params.Length;
                        sw.Write(Collection[t].Params.Length);
                        //6. Write Parameter data (string) (double[VertCt])
                        sw.Write("Parameter Values");
                        for (int u = 0; u < VertCt; u++)
                        {
                            sw.Write(Collection[t].Params[u]);
                        }
                        //7. Write Vertex Locations (string) (double[VertCt][3])
                        sw.Write("Vertex Locations");
                        for (int u = 0; u < VertCt; u++)
                        {
                            //Write Vertex: (double) (double) (double)
                            sw.Write(Collection[t].M.Vertices[u].X);
                            sw.Write(Collection[t].M.Vertices[u].Y);
                            sw.Write(Collection[t].M.Vertices[u].Z);
                        }
                        //8. Announce Mesh Faces (string)
                        sw.Write("Mesh Faces");
                        for (int u = 0; u < Collection[t].M.Faces.Count; u++)
                        {
                            // Write mesh vertex indices: (int) (int) (int) (int)
                            sw.Write((UInt32)Collection[t].M.Faces[u][0]);
                            sw.Write((UInt32)Collection[t].M.Faces[u][1]);
                            sw.Write((UInt32)Collection[t].M.Faces[u][2]);
                            sw.Write((UInt32)Collection[t].M.Faces[u][3]);
                        }
                    }
                    sw.Write("End_of_File");
                    sw.Close();
                }
            }

            #region IPanel methods
            public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
            {
                // Called when the panel tab is made visible, in Mac Rhino this will happen
                // for a document panel when a new document becomes active, the previous
                // documents panel will get hidden and the new current panel will get shown.
            }

            public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
            {
                // Called when the panel tab is hidden, in Mac Rhino this will happen
                // for a document panel when a new document becomes active, the previous
                // documents panel will get hidden and the new current panel will get shown.
            }

            public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
            {
                // Called when the document or panel container is closed/destroyed
            }
            #endregion IPanel methods

        }
    }
}