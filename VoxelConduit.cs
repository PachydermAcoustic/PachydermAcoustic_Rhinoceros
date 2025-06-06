﻿//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
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

using Rhino.Geometry;
using Rhino.Display;
using System.Collections.Generic;
using Pachyderm_Acoustic.Numeric.TimeDomain;
using Rhino.Render.CustomRenderMeshes;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        /// <summary>
        /// A conduit used to display spatial data.
        /// </summary>
        public class CellConduit : DisplayConduit
        {
            List<Mesh> DisplayMesh;
            List<Mesh[]> DirMesh;

            public bool labguide = false;
            public bool hemianechoic = false;
            public double radius = 0;
            public double depth = 0;
            static CellConduit instance = null;

            private CellConduit()
            {
                DisplayMesh = new List<Mesh>();
                DirMesh = new List<Mesh[]>();
                instance = this;
            }

            public static CellConduit Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new CellConduit();
                    }
                    return instance;
                }
            }

            protected override void PostDrawObjects(DrawEventArgs e)
            {
                try
                {
                    lock (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView)
                    {
                        if (labguide)
                        {
                            e.Display.DrawDot(new Point3d(0, 0, 0), "Center");
                            e.Display.DrawSurface(RevSurface.Create(new ArcCurve(new Arc(new Plane(new Point3d(0, 0, depth), new Vector3d(1, 0, 0)), radius, System.Math.PI / 2)), new Line(new Point3d(0, 0, 0), new Point3d(0, 0, 1)), 0, 2 * System.Math.PI), System.Drawing.Color.Green, 4);
                            e.Display.DrawDot(new Point3d(0, 0, radius + depth), "Source");
                            e.Display.DrawArrow(new Line(new Point3d(0, 0, 1.2 * radius + depth), new Point3d(0, 0, 1 * radius + depth)), System.Drawing.Color.Red);
                            if (hemianechoic)
                            {
                                e.Display.DrawBox(new BoundingBox(new Point3d(-radius, -radius, 0), new Point3d(radius, radius, 1.2 * radius + depth)), System.Drawing.Color.Blue, 2);
                                e.Display.DrawBox(new BoundingBox(new Point3d(-radius - 1, -radius - 1, 0), new Point3d(radius + 1, radius + 1, 1.2 * radius + 1 + depth)), System.Drawing.Color.Black, 2);
                            } else
                            {
                                e.Display.DrawBox(new BoundingBox(new Point3d(-2 * radius, -2 * radius, 0), new Point3d(2 * radius, 2 * radius, 1.2 * radius + depth)), System.Drawing.Color.Blue, 2);
                                e.Display.DrawBox(new BoundingBox(new Point3d(-2 * radius - 1, -2 * radius - 1, -1), new Point3d(2 * radius + 1, 2 * radius + 1, 1.2 * radius + 1 + depth)), System.Drawing.Color.Black, 2);
                            }
                        }

                        for (int i = 0; i < DisplayMesh.Count; i++)
                        {
                            for (int j = 0; j < DirMesh[i].Length; j++) e.Display.DrawMeshWires(DirMesh[i][j], System.Drawing.Color.Blue);
                            e.Display.DrawMeshWires(DisplayMesh[i], System.Drawing.Color.Red);
                        }
                    }
                }
                catch { return; }
            }

            public void Add(Numeric.TimeDomain.Acoustic_Compact_FDTD.Node Node, int x, int y, int z, double dx, Rhino.Geometry.Point3d corner)
            {
                double dx3 = dx / 3;

                if (Node.GetType() == typeof(Numeric.TimeDomain.Acoustic_Compact_FDTD.Bound_Node))
                {
                    Point3d START = corner + new Rhino.Geometry.Point3d(dx * x, dx * y, dx * z);
                    DisplayMesh.Add(Mesh.CreateFromBox(new BoundingBox(START, START + new Point3d(dx, dx, dx)), 1, 1, 1));
                    List<Numeric.TimeDomain.Acoustic_Compact_FDTD.Bound_Node.Boundary> B = (Node as Numeric.TimeDomain.Acoustic_Compact_FDTD.Bound_Node_RDD).B_List;
                    Mesh[] corners = new Mesh[B.Count];

                    for (int i = 0; i < B.Count; i++)
                    {
                        switch (B[i])
                        {
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.AXNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, dx3, 0), START + new Point3d(2 * dx3, 2 * dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.AXPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, dx3, 2 * dx3), START + new Point3d(2 * dx3, 2 * dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.AYNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, 0, dx3), START + new Point3d(2 * dx3, dx3, 2 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.AYPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, 2 * dx3, dx3), START + new Point3d(2 * dx3, 3 * dx3, 2 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.AZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, dx3, dx3), START + new Point3d(dx3, 2 * dx3, 2 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.AZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, dx3, dx3), START + new Point3d(3 * dx3, 2 * dx3, 2 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXNegYNegZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START, START + new Point3d(dx3, dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXNegYNegZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, 0, 2 * dx3), START + new Point3d(dx3, dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXNegYPosZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, 2 * dx3, 0), START + new Point3d(dx3, 3 * dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXNegYPosZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, 2 * dx3, 2 * dx3), START + new Point3d(dx3, 3 * dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXPosYNegZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, 0, 0), START + new Point3d(3 * dx3, dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXPosYNegZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, 0, 2 * dx3), START + new Point3d(3 * dx3, dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXPosYPosZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, 2 * dx3, 0), START + new Point3d(3 * dx3, 3 * dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.DXPosYPosZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, 2 * dx3, 2 * dx3), START + new Point3d(3 * dx3, 3 * dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXNegYNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, 0, dx3), START + new Point3d(dx3, dx3, 2 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXNegYPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, 2 * dx3, 0), START + new Point3d(dx3, 3 * dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXNegZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, dx3, 0), START + new Point3d(dx3, 2 * dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXNegZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(0, dx3, 2 * dx3), START + new Point3d(dx3, 2 * dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXPosYNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, 0, dx3), START + new Point3d(3 * dx3, dx3, 2 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXPosYPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, 2 * dx3, dx3), START + new Point3d(3 * dx3, 3 * dx3, 2 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXPosZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, dx3, 0), START + new Point3d(3 * dx3, 2 * dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDXPosZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(2 * dx3, dx3, 2 * dx3), START + new Point3d(3 * dx3, 2 * dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDYNegZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, 0, 0), START + new Point3d(2 * dx3, dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDYNegZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, 0, 2 * dx3), START + new Point3d(2 * dx3, dx3, 3 * dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDYPosZNeg:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, 2 * dx3, 0), START + new Point3d(2 * dx3, 3 * dx3, dx3)), 1, 1, 1);
                                break;
                            case Acoustic_Compact_FDTD.Bound_Node.Boundary.SDYPosZPos:
                                corners[i] = Mesh.CreateFromBox(new BoundingBox(START + new Point3d(dx3, 2 * dx3, 2 * dx3), START + new Point3d(2 * dx3, 3 * dx3, 3 * dx3)), 1, 1, 1);
                                break;
                        }

                        DirMesh.Add(corners);
                    }
                }

                this.Enabled = true;

            }
        }
    }
}