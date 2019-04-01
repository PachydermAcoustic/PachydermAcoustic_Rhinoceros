//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2019, Arthur van der Harten 
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

namespace Pachyderm_Acoustic
{
    namespace Visualization
    {
        /// <summary>
        /// Base class for particle goemetry classes.
        /// </summary>
        public abstract class Phonon
        {
            public abstract Mesh Generate(Point3d P, Vector3d V);
        }

        /// <summary>
        /// Tetrahedron particle geometry.
        /// </summary>
        public class Tetrahedron : Phonon
        {
            public override Mesh Generate(Point3d P, Vector3d V)
            {
                Vector3d diffx;
                Vector3d diffy;
                Vector3d diffz = new Vector3d(V);
                double proj;
                diffx = new Vector3d(0, 0, 1);
                proj = System.Math.Abs(Vector3d.Multiply(diffz, diffx));

                if (0.99 < proj && 1.01 > proj) diffx = new Vector3d(1, 0, 0);
                diffy = diffz;
                diffy = Vector3d.CrossProduct(diffy, diffx);
                diffx = Vector3d.CrossProduct(diffy, diffz);
                diffx.Unitize();
                diffy.Unitize();
                diffz.Unitize();

                Mesh M = new Mesh();
                M.Vertices.SetVertex(0, P + diffz * 0.25);
                M.Vertices.SetVertex(1, P + (diffx * 0.7072 + diffy * -.5 + diffz * -.5) * 0.25);
                M.Vertices.SetVertex(2, P + (diffy + diffz * -.5) * 0.25);
                M.Vertices.SetVertex(3, P + (diffx * -0.7072 + diffy * -.5 + diffz * -.5) * 0.25);

                M.Faces.SetFace(0, 0, 1, 2);
                M.Faces.SetFace(1, 0, 2, 3);
                M.Faces.SetFace(2, 0, 3, 1);
                M.Faces.SetFace(3, 3, 2, 1);
                M.FaceNormals.ComputeFaceNormals();
                //M.ComputeVertexNormals();

                return M;
            }
        }

        /// <summary>
        /// Octahedron partical geometry.
        /// </summary>
        public class Octahedron : Phonon
        {
            public override Mesh Generate(Point3d P, Vector3d V)
            {
                Vector3d diffx;
                Vector3d diffy;
                Vector3d diffz = new Vector3d(V);
                double proj;
                diffx = new Vector3d(0, 0, 1);
                proj = System.Math.Abs(Vector3d.Multiply(diffz, diffx));

                if (0.99 < proj && 1.01 > proj) diffx = new Vector3d(1, 0, 0);
                diffy = diffz;
                diffy =  Vector3d.CrossProduct(diffy, diffx);
                diffx = Vector3d.CrossProduct(diffy, diffz);
                diffx.Unitize();
                diffy.Unitize();
                diffz.Unitize();

                Mesh M = new Mesh();
                M.Vertices.SetVertex(0, P + diffz * 0.25);
                M.Vertices.SetVertex(1, P + diffx * 0.25);
                M.Vertices.SetVertex(2, P + diffy * .25);
                M.Vertices.SetVertex(3, P + diffx * -0.25);
                M.Vertices.SetVertex(4, P + diffy * -0.25);
                M.Vertices.SetVertex(5, P + diffz * -.25);

                M.Faces.SetFace(0, 0, 1, 2);
                M.Faces.SetFace(1, 0, 2, 3);
                M.Faces.SetFace(2, 0, 3, 4);
                M.Faces.SetFace(3, 0, 4, 1);
                M.Faces.SetFace(4, 5, 2, 1);
                M.Faces.SetFace(5, 5, 3, 2);
                M.Faces.SetFace(6, 5, 4, 3);
                M.Faces.SetFace(7, 5, 1, 4);
                
                M.FaceNormals.ComputeFaceNormals();
                //M.ComputeVertexNormals();

                return M;
            }
        }

        /// <summary>
        /// Icosahedron partical geometry.
        /// </summary>
        public class Icosahedron : Phonon
        {
            public override Mesh Generate(Point3d P, Vector3d V)
            {

                double sqr5 = System.Math.Sqrt(5.0);
                double phi = (1.0 + sqr5) * 0.5; // golden ratio
                double ratio = System.Math.Sqrt(10.0 + (2.0 * sqr5)) / (4.0 * phi);
                double a = (.25 / ratio) * 0.5;
                double b = (.25 / ratio) / (2.0 * phi);

                // Define the icosahedron's 12 vertices
                Point3d P1 = P + new Point3d(0, b, -a);
                Point3d P2 = P + new Point3d(b, a, 0);
                Point3d P3 = P + new Point3d(-b, a, 0);
                Point3d P4 = P + new Point3d(0, b, a);
                Point3d P5 = P + new Point3d(0, -b, a);
                Point3d P6 = P + new Point3d(-a, 0, b);
                Point3d P7 = P + new Point3d(0, -b, -a);
                Point3d P8 = P + new Point3d(a, 0, -b);
                Point3d P9 = P + new Point3d(a, 0, b);
                Point3d P10 = P + new Point3d(-a, 0, -b);
                Point3d P11 = P + new Point3d(b, -a, 0);
                Point3d P12 = P + new Point3d(-b, -a, 0);

                Mesh M = new Mesh();
                M.Vertices.SetVertex(0,P1);
                M.Vertices.SetVertex(1, P2);
                M.Vertices.SetVertex(2, P3);
                M.Vertices.SetVertex(3, P4);
                M.Vertices.SetVertex(4, P5);
                M.Vertices.SetVertex(5, P6);
                M.Vertices.SetVertex(6, P7);
                M.Vertices.SetVertex(7, P8);
                M.Vertices.SetVertex(8, P9);
                M.Vertices.SetVertex(9, P10);
                M.Vertices.SetVertex(10, P11);
                M.Vertices.SetVertex(11, P12);

                //Create the icosahedron's 20 triangular faces
                M.Faces.SetFace(0, 0, 1, 2);
                M.Faces.SetFace(1, 3, 2, 1);
                M.Faces.SetFace(2, 3, 4, 5);
                M.Faces.SetFace(3, 3, 8, 4);
                M.Faces.SetFace(4, 0, 6, 7);
                M.Faces.SetFace(5, 0, 9, 6);
                M.Faces.SetFace(6, 4, 10, 11);
                M.Faces.SetFace(7, 6, 11, 10);
                M.Faces.SetFace(8, 2, 5, 9);
                M.Faces.SetFace(9, 11, 9, 5);
                M.Faces.SetFace(10, 1, 7, 8);
                M.Faces.SetFace(11, 10, 8, 7);
                M.Faces.SetFace(12, 3, 5, 2);
                M.Faces.SetFace(13, 3, 1, 8);
                M.Faces.SetFace(14, 0, 2, 9);
                M.Faces.SetFace(15, 0, 7, 1);
                M.Faces.SetFace(16, 6, 9, 11);
                M.Faces.SetFace(17, 6, 10, 7);
                M.Faces.SetFace(18, 4, 11, 5);
                M.Faces.SetFace(19, 4, 8, 10);

                return M;
            }
        }

        /// <summary>
        /// Geodesic sphere partical geometry.
        /// </summary>
        public class Geodesic_sphere : Phonon
        {
            private Mesh M;
            
            public override Mesh Generate(Point3d P, Vector3d V)
            {
                return Generate(P, V, 3);
            }

            public Mesh Generate(Point3d P, Vector3d V, int order)
            {
                Vector3d diffx;
                Vector3d diffy;
                Vector3d diffz = new Vector3d(V);
                double proj;
                diffx = new Vector3d(0, 0, 1);
                proj = System.Math.Abs(Vector3d.Multiply(diffz, diffx));

                if (0.99 < proj && 1.01 > proj) diffx = new Vector3d(1, 0, 0);
                diffy = diffz;
                diffy = Vector3d.CrossProduct(diffy, diffx);
                diffx = Vector3d.CrossProduct(diffy, diffz);
                diffx.Unitize();
                diffy.Unitize();
                diffz.Unitize();

                int PCT = 3;
                for (int i = 0; i < order; i++)
                {
                    PCT += (int)System.Math.Pow(3, i);
                }

                M = new Mesh();//(PCT, 4 * (int)System.Math.Pow(3, order), false, false);
                M.Vertices.SetVertex(0, P + diffz * 0.25);
                M.Vertices.SetVertex(1, P + (diffx * 0.7072 + diffy * -.5 + diffz * -.5) * 0.25);
                M.Vertices.SetVertex(2, P + (diffy + diffz * -.5) * 0.25);
                M.Vertices.SetVertex(3, P + (diffx * -0.7072 + diffy * -.5 + diffz * -.5) * 0.25);

                triangle(P, 0, 1, 2, .25, 0, order);
                triangle(P, 0, 2, 3, .25, 0, order);
                triangle(P, 0, 3, 1, .25, 0, order);
                triangle(P, 3, 2, 1, .25, 0, order);
                M.FaceNormals.ComputeFaceNormals();
                //M.ComputeVertexNormals();
                return M;
            }
            
            private void triangle(Point3d Orig, int P0, int P1, int P2, double R, int Ord, int max)
            {
                if (Ord < max)
                {
                    Point3f Point3 = (new Point3f((M.Vertices[P0].X + M.Vertices[P1].X) / 2, (M.Vertices[P0].Y + M.Vertices[P1].Y) / 2, (M.Vertices[P0].Z + M.Vertices[P1].Z) / 2));
                    Vector3d PN = new Vector3d(Point3.X, Point3.Y, Point3.Z) - new Vector3d(Orig.X, Orig.Y, Orig.Z);
                    PN.Unitize();
                    Point3 = new Point3f((float)PN.X * (float)R, (float)PN.Y * (float)R, (float)PN.Z * (float)R);
                    Point3.X += (float)Orig.X;
                    Point3.Y += (float)Orig.Y;
                    Point3.Z += (float)Orig.Z;

                    Point3f Point4 = (new Point3f((M.Vertices[P2].X + M.Vertices[P1].X) / 2, (M.Vertices[P2].Y + M.Vertices[P1].Y) / 2, (M.Vertices[P2].Z + M.Vertices[P1].Z) / 2));
                    PN = new Vector3d(Point4.X, Point4.Y, Point4.Z) - new Vector3d(Orig.X, Orig.Y, Orig.Z);
                    PN.Unitize();
                    Point4 = new Point3f((float)PN.X * (float)R, (float)PN.Y * (float)R, (float)PN.Z * (float)R);
                    Point4.X += (float)Orig.X;
                    Point4.Y += (float)Orig.Y;
                    Point4.Z += (float)Orig.Z;

                    Point3f Point5 = (new Point3f((M.Vertices[P0].X + M.Vertices[P2].X) / 2, (M.Vertices[P0].Y + M.Vertices[P2].Y) / 2, (M.Vertices[P0].Z + M.Vertices[P2].Z) / 2));
                    PN = new Vector3d(Point5.X, Point5.Y, Point5.Z) - new Vector3d(Orig.X, Orig.Y, Orig.Z);
                    PN.Unitize();
                    Point5 = new Point3f((float)PN.X * (float)R, (float)PN.Y * (float)R, (float)PN.Z * (float)R);
                    Point5.X += (float)Orig.X;
                    Point5.Y += (float)Orig.Y;
                    Point5.Z += (float)Orig.Z;

                    //int P3 = M.Vertices.GetConnectedVertices();
                    //if (P3 < 0) P3 = M.VertexCount();
                    //M.SetVertex(P3, Point3);
                    
                    //int P4 = M.Vertices.Search(Point4);
                    //if (P4 < 0) P4 = M.VertexCount();
                    //M.SetVertex(P4, Point4);

                    //int P5 = M.Vertices.Search(Point5);
                    //if (P5 < 0) P5 = M.VertexCount();
                    //M.SetVertex(P5, Point5);

                    int P3 = M.Vertices.Count;
                    M.Vertices.SetVertex(P3, Point3);

                    int P4 = M.Vertices.Count;
                    M.Vertices.SetVertex(P4, Point4);

                    int P5 = M.Vertices.Count;
                    M.Vertices.SetVertex(P5, Point5);

                    this.triangle(Orig, P0, P3, P5, R, Ord + 1, max);
                    this.triangle(Orig, P1, P4, P3, R, Ord + 1, max);
                    this.triangle(Orig, P2, P5, P4, R, Ord + 1, max);
                    this.triangle(Orig, P3, P4, P5, R, Ord + 1, max);    
                }
                else 
                {
                    M.Faces.SetFace(M.Faces.Count, P0, P1, P2);
                }
            }
        }
    }
}