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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rhino.Geometry;
using System.Runtime.InteropServices;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        [GuidAttribute("1c48c00e-abd8-40fd-8642-2ce7daa90ed5")]
        public partial class Pach_MapCustom : UserControl
        {
            double Current_Min;
            double Current_Max;
            Pach_Graphics.colorscale c_scale;
            List<ParaMesh> Collection = new List<ParaMesh>();

            public Pach_MapCustom()
            {
                InitializeComponent();
                Update_Scale();
                Instance = this;
            }

            public void Create_Map()
            {
                Update_Scale();
                ParaMesh P = Collection[MeshList.SelectedIndex];
                Utilities.PachTools.CreateMap(P.M, Color_Selection.SelectedIndex, P.Params, (double)Param_Min.Value, (double)Param_Max.Value);
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

            /// <summary>
            /// Adjust scale boundaries
            /// </summary>
            private void Commit_Param_Bounds()
            {
                Current_Min = (double)this.Param_Min.Value;
                Current_Max = (double)this.Param_Max.Value;
                this.Param1_4.Text = (((Current_Max - Current_Min) * .25) + Current_Min).ToString();
                this.Param1_2.Text = (((Current_Max - Current_Min) * .5) + Current_Min).ToString();
                this.Param3_4.Text = (((Current_Max - Current_Min) * .75) + Current_Min).ToString();
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

            /// <summary>
            /// Updates color scale
            /// </summary>
            public void Update_Scale()
            {
                double H_OFFSET;
                double H_BREADTH;
                double S_OFFSET;
                double S_BREADTH;
                double V_OFFSET;
                double V_BREADTH;

                System.Drawing.Color[] Colors;
                switch (Color_Selection.Text)
                {
                    case "R-O-Y-G-B-I-V":
                        H_OFFSET = 0;
                        H_BREADTH = 4.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, this.Discretize.Checked, 24);
                        break;
                    case "R-O-Y":
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 0;
                        H_BREADTH = 1.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, this.Discretize.Checked, 12);
                        break;
                    case "Y-G":
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = Math.PI / 3.0;
                        H_BREADTH = 1.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, this.Discretize.Checked, 12);
                        break;
                    case "R-M-B":
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 0;
                        H_BREADTH = -2.0 / 3.0;
                        S_OFFSET = 1;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, this.Discretize.Checked, 12);
                        break;
                    case "W-B":
                        H_OFFSET = 0;
                        H_BREADTH = 0;
                        S_OFFSET = 0;
                        S_BREADTH = 0;
                        V_OFFSET = 1;
                        V_BREADTH = -1;
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, this.Discretize.Checked, 12);
                        break;
                    default:
                        Rhino.RhinoApp.WriteLine("Whoops... Color selection invalid... Bright green substituted!");
                        Colors = new System.Drawing.Color[2];
                        H_OFFSET = 2.0 * Math.PI / 3;
                        H_BREADTH = 0;
                        S_OFFSET = 1;
                        S_BREADTH = -1;
                        V_OFFSET = 1;
                        V_BREADTH = 0;
                        c_scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, H_OFFSET, H_BREADTH, S_OFFSET, S_BREADTH, V_OFFSET, V_BREADTH, this.Discretize.Checked, 12);
                        break;
                }
                this.Param_Scale.Image = c_scale.PIC;
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

            private void Color_Selection_SelectedIndexChanged(object sender, EventArgs e)
            {
                Update_Scale();
            }

            private void Discretize_CheckedChanged(object sender, EventArgs e)
            {
                Update_Scale();
            }

            private void SaveDataToolStripMenuItem_Click(object sender, EventArgs e)
            {
                PachydermAc_PlugIn plugin = PachydermAc_PlugIn.Instance;
                System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
                sf.DefaultExt = ".pccm";
                sf.AddExtension = true;
                sf.Filter = "Pachyderm Custom Map file (*.pccm)|*.pccm|" + "All Files|";
                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
        }
    }
}