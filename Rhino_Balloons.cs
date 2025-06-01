////'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
////' 
////'This file is part of Pachyderm-Acoustic. 
////' 
////'Copyright (c) 2008-2025, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit 
////'Pachyderm-Acoustic is free software; you can redistribute it and/or modify 
////'it under the terms of the GNU General Public License as published 
////'by the Free Software Foundation; either version 3 of the License, or 
////'(at your option) any later version. 
////'Pachyderm-Acoustic is distributed in the hope that it will be useful, 
////'but WITHOUT ANY WARRANTY; without even the implied warranty of 
////'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
////'GNU General Public License for more details. 
////' 
////'You should have received a copy of the GNU General Public 
////'License along with Pachyderm-Acoustic; if not, write to the Free Software 
////'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA. 

//using System;
//using Rhino.Geometry;

//namespace Pachyderm_Acoustic
//{
//    public class Speaker_Balloon_RC:Speaker_Balloon
//    {
//        public Speaker_Balloon_RC(string[] Ballooncode_in, string SWL_in, int Type_in, Hare.Geometry.Point Center)
//            :base(Ballooncode_in, SWL_in, Type_in, Center)
//        {
//            //RhinoMeshStuff
//            m_RhinoMesh = new Mesh();
//            int ct = 0;
//            for (int i = 0; i < m_HareMesh[4].Polygon_Count; i++)
//            {
//                Hare.Geometry.Point[] Pt = m_HareMesh[4].Polygon_Vertices(i);
//                int[] F = new int[m_HareMesh[4].Polys[i].VertextCT];
//                for (int j = 0; j < m_HareMesh[4].Polys[i].VertextCT; j++)
//                {
//                    m_RhinoMesh.Vertices.Add(new Point3d(Pt[j].x, Pt[j].y, Pt[j].z) / 90);
//                    F[j] = ct;
//                    ct++;
//                }
//                m_RhinoMesh.Faces.AddFace(F[2], F[1], F[0], F[3]);
//            }
//            m_RhinoMesh.FaceNormals.ComputeFaceNormals();
//            m_RhinoMesh.Normals.ComputeNormals();
//            m_DisplayMesh = m_RhinoMesh.DuplicateMesh();
//            Update_Position(Center);
//        }
//    }
//}