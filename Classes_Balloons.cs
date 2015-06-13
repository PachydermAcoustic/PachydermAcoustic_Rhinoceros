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
using Hare.Geometry;
using Rhino.Geometry;

namespace Pachyderm_Acoustic
{
    public class Speaker_Balloon
    {
        /// <summary>
        /// A mesh edited to show position and aim.
        /// </summary>
        public Mesh m_DisplayMesh;
        /// <summary>
        /// Original mesh used to create display mesh.
        /// </summary>
        public Mesh m_RhinoMesh;
        /// <summary>
        /// Original mesh used for the final source object.
        /// </summary>
        public Topology[] m_HareMesh;
        public string[] code;
        string SWL;
        int Type;

        public Speaker_Balloon(string[] Ballooncode_in, string SWL_in, int Type_in, Point3d Center)//int sym_in,
        {
            code = Ballooncode_in;
            Type = Type_in;
            if (SWL_in != "" || SWL_in != null)
            {
                SWL = SWL_in;
            }
            else 
            {
                SWL_in = "120; 120; 120; 120; 120; 120; 120; 120";
            }
            string[] swl = SWL.Split(';');
            double[] swl_values = new double[8];

            Vector upper = new Vector(0, 1, 0);

            int umax, vmax;
            switch (Type)
            {
                case 0:
                    umax = 19;
                    vmax = 36;
                    break;
                case 1:
                    umax = 37;
                    vmax = 72;
                    break;
                default:
                    throw new Exception();
            }

            m_HareMesh = new Topology[8];
            for (int oct = 1; oct < 9; oct++)
            {
                string[] values;
                swl_values[oct - 1] = double.Parse(swl[oct - 1]);
                if (code[oct - 1] != "")
                {
                    values = code[oct - 1].Split(';');
                }
                else
                {
                    values = new string[umax * vmax];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = "0";
                    }
                }

                Vector[,] Magnitude = new Vector[umax, vmax];
                double Theta, Phi;
                int idx = 0;

                for (int v = 0; v < vmax; v++)
                {
                    for (int u = 0; u < umax; u++)
                    {
                        if (double.IsInfinity(swl_values[oct - 1]) || double.IsNaN(swl_values[oct - 1]))
                        {
                            Magnitude[u, v] = new Vector();
                        }
                        else
                        {
                            Theta = u * System.Math.PI / (umax - 1);
                            Phi = 2 * v * System.Math.PI / (vmax) + System.Math.PI / 2;
                            Magnitude[u, v] = new Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Cos(Theta), Math.Sin(Theta) * Math.Sin(Phi));
                            Magnitude[u, v].Normalize();
                            double swlmag = (double.Parse(values[idx]) + 60);
                            if (swlmag < 0) swlmag = 0;
                            Magnitude[u, v] *= swlmag;
                        }
                        idx++;
                    }
                }

                //Create a mesh of the points...
                List<Hare.Geometry.Point[]> list = new List<Hare.Geometry.Point[]>();
                double Minx = double.MaxValue, Miny = double.MaxValue, Minz = double.MaxValue, Maxx = double.MinValue, Maxy = double.MinValue, Maxz = double.MinValue;
                
                for (int u = 0; u < umax - 1; u++)
                {
                    for (int v = 0; v < vmax; v++)
                    {
                        Hare.Geometry.Point[] Poly = new Hare.Geometry.Point[4];
                        Poly[0] = Magnitude[u, v];
                        Poly[1] = Magnitude[u, (v + 1) % vmax];
                        Poly[2] = Magnitude[u + 1, (v + 1) % vmax];
                        Poly[3] = Magnitude[u + 1, v];
                        list.Add(Poly);

                        foreach (Hare.Geometry.Point p in Poly) 
                        {
                            if (p.x < Minx) Minx = p.x;
                            if (p.y < Miny) Miny = p.y;
                            if (p.z < Minz) Minz = p.z;
                            if (p.x > Maxx) Maxx = p.x;
                            if (p.y > Maxy) Maxy = p.y;
                            if (p.z > Maxz) Maxz = p.z;
                        }
                    }
                }

                m_HareMesh[oct - 1] = new Topology(new Hare.Geometry.Point(Minx, Miny, Minz), new Hare.Geometry.Point(Maxx, Maxy, Maxz));
                
                foreach (Hare.Geometry.Point[] Poly in list) m_HareMesh[oct - 1].Add_Polygon(Poly);
            }
            //RhinoMeshStuff
            m_RhinoMesh = new Mesh();
            int ct = 0;
            for (int i = 0; i < m_HareMesh[4].Polygon_Count; i++)
            {
                Hare.Geometry.Point[] Pt = m_HareMesh[4].Polygon_Vertices(i);
                int[] F = new int[m_HareMesh[4].Polys[i].VertextCT];
                //List<Point3d> F = new List<Point3d>();
                for (int j = 0; j < m_HareMesh[4].Polys[i].VertextCT; j++)
                {
                    m_RhinoMesh.Vertices.Add(new Point3d(Pt[j].x, Pt[j].y, Pt[j].z) / 90);
                    F[j] = ct;
                    //F.Add(new Point3d(Pt[j].x, Pt[j].y, Pt[j].z) / 200);
                    ct++;
                }
                m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0], F[3]);
            }
            m_RhinoMesh.FaceNormals.ComputeFaceNormals();
            m_RhinoMesh.Normals.ComputeNormals();
            m_DisplayMesh = m_RhinoMesh.DuplicateMesh();
            Update_Position(new Point3f((float)Center.X, (float)Center.Y, (float)Center.Z));
        }

        public Topology[] Balloons(double[] spl_values)
        {
            Topology[] Balloon = new Topology[8];

            int umax, vmax;
            switch (Type)
            {
                case 0:
                    umax = 19;
                    vmax = 36;
                    break;
                case 1:
                    umax = 37;
                    vmax = 72;
                    break;
                default:
                    throw new Exception("Balloon type not valid for this version of CLF.");
            }

            for (int oct = 1; oct < 9; oct++)
            {
                string[] values;
                if (code[oct - 1] != "")
                {
                    values = code[oct - 1].Split(';');
                }
                else
                {
                    values = new string[umax * vmax];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = "0";
                    }
                }

                Vector[,] Magnitude = new Vector[umax, vmax];
                double Theta, Phi;
                int idx = 0;
                for (int v = 0; v < vmax; v++)
                {
                    for (int u = 0; u < umax; u++)
                    {
                        if (double.IsInfinity(spl_values[oct - 1]) || double.IsNaN(spl_values[oct - 1]))
                        {
                            Magnitude[u, v] = new Vector();
                        }
                        else
                        {
                            Theta = u * System.Math.PI / (umax - 1);
                            Phi = 2 * v * System.Math.PI / (vmax) + System.Math.PI / 2;
                            Magnitude[u, v] = new Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Cos(Theta), Math.Sin(Theta) * Math.Sin(Phi));
                            Magnitude[u, v].Normalize();
                            Magnitude[u, v] *= (double.Parse(values[idx]) + spl_values[oct - 1]);
                        }
                        idx++;
                    }
                }

                for (int u = 0; u < umax; u++)
                {
                    for (int v = 0; v < vmax; v++)
                    {
                        double roll = CurrentAxi * Math.PI / 180;
                        double yaw = CurrentAlt * Math.PI / 180;
                        double pitch = CurrentAzi * Math.PI / 180;

                        double x = Magnitude[u, v].x;
                        Magnitude[u, v].x = x * Math.Cos(roll) - Magnitude[u, v].z * Math.Sin(roll);
                        Magnitude[u, v].z = x * Math.Sin(roll) + Magnitude[u, v].z * Math.Cos(roll);
                        double y = Magnitude[u, v].y;
                        Magnitude[u, v].y = y * Math.Cos(yaw) - Magnitude[u, v].z * Math.Sin(yaw);
                        Magnitude[u, v].z = y * Math.Sin(yaw) + Magnitude[u, v].z * Math.Cos(yaw);
                        x = Magnitude[u, v].x;
                        Magnitude[u, v].x = x * Math.Cos(pitch) - Magnitude[u, v].y * Math.Sin(pitch);
                        Magnitude[u, v].y = x * Math.Sin(pitch) + Magnitude[u, v].y * Math.Cos(pitch);
                    }
                }

                List<Hare.Geometry.Point[]> list = new List<Hare.Geometry.Point[]>();
                //Create a mesh of the points...
                for (int u = 0; u < umax - 1; u++)
                {
                    for (int v = 0; v < vmax; v++)
                    {
                        Hare.Geometry.Point[] Poly = new Hare.Geometry.Point[3];
                        Poly[0] = Magnitude[u, v];
                        Poly[1] = Magnitude[u, (v + 1) % vmax];
                        Poly[2] = Magnitude[u + 1, v];
                        list.Add(Poly);
                        //Balloon[oct - 1].Add_Polygon(Poly);

                        Poly = new Hare.Geometry.Point[3];
                        Poly[0] = Magnitude[u, (v + 1) % vmax];
                        Poly[1] = Magnitude[u + 1, (v + 1) % vmax];
                        Poly[2] = Magnitude[u + 1, v];
                        list.Add(Poly);
                        //Balloon[oct - 1].Add_Polygon(Poly);
                    }
                }

                double Minx = double.MaxValue, Miny = double.MaxValue, Minz = double.MaxValue, Maxx = double.MinValue, Maxy = double.MinValue, Maxz = double.MinValue;
                foreach(Hare.Geometry.Point[] p in list)
                {
                    foreach(Hare.Geometry.Point p0 in p)
                    {
                        if (p0.x < Minx) Minx = p0.x;
                        if (p0.y < Miny) Miny = p0.y;
                        if (p0.z < Minz) Minz = p0.z;
                        if (p0.x > Maxx) Maxx = p0.x;
                        if (p0.y > Maxy) Maxy = p0.y;
                        if (p0.z > Maxz) Maxz = p0.z;
                    }
                }

                Balloon[oct - 1] = new Topology(new Hare.Geometry.Point(Minx, Miny, Minz), new Hare.Geometry.Point(Maxx, Maxy, Maxz));
                foreach (Hare.Geometry.Point[] p in list) Balloon[oct - 1].Add_Polygon(p);
            }
            Balloon[0].Finish_Topology();
            Balloon[1].Finish_Topology();
            Balloon[2].Finish_Topology();
            Balloon[3].Finish_Topology();
            Balloon[4].Finish_Topology();
            Balloon[5].Finish_Topology();
            Balloon[6].Finish_Topology();
            Balloon[7].Finish_Topology();
            return Balloon;
        }

        public void Update_Position()
        {
            Update_Position(CurrentPos);
        }

        public void Update_Position(Point3f Center)
        {
            for (int i = 0; i < m_DisplayMesh.Vertices.Count; i++)
            {
                Point3f Po = m_RhinoMesh.Vertices[i];
                Point3f P =  new Point3f(m_RhinoMesh.Vertices[i].X + Center.X - CurrentPos.X, m_RhinoMesh.Vertices[i].Y + Center.Y - CurrentPos.Y, m_RhinoMesh.Vertices[i].Z + Center.Z - CurrentPos.Z);
                m_RhinoMesh.Vertices.SetVertex(i, P);
                Point3f Pn = m_RhinoMesh.Vertices[i];
                double x = Po.Y + Pn.Y;
            }
            CurrentPos = Center;
            Update_Aim();
        }

        Point3f CurrentPos = new Point3f(0,0,0);
        public float CurrentAlt, CurrentAzi, CurrentAxi;

        public void Update_Aim()
        {
            this.m_DisplayMesh = m_RhinoMesh.DuplicateMesh();
            for (int i = 0; i < m_DisplayMesh.Vertices.Count; i++)
            {
                m_DisplayMesh.Vertices[i] = new Point3f(m_DisplayMesh.Vertices[i].X - CurrentPos.X, m_DisplayMesh.Vertices[i].Y - CurrentPos.Y, m_DisplayMesh.Vertices[i].Z - CurrentPos.Z);
                double roll = CurrentAxi * Math.PI / 180;
                double yaw = CurrentAlt * Math.PI / 180;
                double pitch = CurrentAzi * Math.PI / 180;

                double x = m_DisplayMesh.Vertices[i].X;
                m_DisplayMesh.Vertices[i] = new Point3f((float)(x * Math.Cos(roll) - m_DisplayMesh.Vertices[i].Z * Math.Sin(roll)), m_DisplayMesh.Vertices[i].Y,(float)(x * Math.Sin(roll) + m_DisplayMesh.Vertices[i].Z * Math.Cos(roll)));
                double y = m_DisplayMesh.Vertices[i].Y;
                m_DisplayMesh.Vertices[i] = new Point3f(m_DisplayMesh.Vertices[i].X, (float)(y * Math.Cos(yaw) - m_DisplayMesh.Vertices[i].Z * Math.Sin(yaw)),(float)(y * Math.Sin(yaw) + m_DisplayMesh.Vertices[i].Z * Math.Cos(yaw)));
                x = m_DisplayMesh.Vertices[i].X;
                m_DisplayMesh.Vertices[i] = new Point3f((float)(x * Math.Cos(pitch) - m_DisplayMesh.Vertices[i].Y * Math.Sin(pitch)), (float)(x * Math.Sin(pitch) + m_DisplayMesh.Vertices[i].Y * Math.Cos(pitch)), m_DisplayMesh.Vertices[i].Z);
                
                m_DisplayMesh.Vertices[i] = new Point3f(m_DisplayMesh.Vertices[i].X + CurrentPos.X, m_DisplayMesh.Vertices[i].Y + CurrentPos.Y, m_DisplayMesh.Vertices[i].Z + CurrentPos.Z);
            }
        }
    }
}