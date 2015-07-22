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

using Rhino.Geometry;
using Rhino.Display;
using System.Collections.Generic;
using Pachyderm_Acoustic.Numeric.TimeDomain;

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

            public CellConduit()
            {
                DisplayMesh = new List<Mesh>();
                DirMesh = new List<Mesh[]>();
                Instance = this;
            }

            public static CellConduit Instance
            {
                get;
                private set;
            }

            protected override void PostDrawObjects(DrawEventArgs e)
            {
                try
                {
                    lock (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView)
                    {
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
                    List<Numeric.TimeDomain.Acoustic_Compact_FDTD.Bound_Node.Boundary> B = (Node as Numeric.TimeDomain.Acoustic_Compact_FDTD.Bound_Node).B_List;
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