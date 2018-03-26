using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pachyderm_Acoustic
{
    namespace Numeric
    {
        namespace TimeDomain
        {
            public partial class Acoustic_Compact_FDTD_RC : Acoustic_Compact_FDTD
            {
                public Rhino.Geometry.Mesh[] m_templateX, m_templateY, m_templateZ;

                public Acoustic_Compact_FDTD_RC(Environment.Polygon_Scene Rm_in, ref Signal_Driver_Compact S_in, ref Microphone_Compact M_in, double fmax_in, double tmax_in)
                    : base(Rm_in, ref S_in, ref M_in, fmax_in, tmax_in, GridType.Freefield, new Hare.Geometry.Point(), 0, 0, 0)
                {
                    Build_Mesh_Sections();
                    Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(Utilities.RC_PachTools.HPttoRPt(RDD_Location(SD.X[0], SD.Y[0], SD.Z[0])));
                    if (Mic.X.Length > 0) Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(Utilities.RC_PachTools.HPttoRPt(RDD_Location(Mic.X[0], Mic.Y[0], Mic.Z[0])));
                }

                public Acoustic_Compact_FDTD_RC(Environment.Polygon_Scene Rm_in, ref Signal_Driver_Compact S_in, ref Microphone_Compact M_in, double fmax_in, double tmax_in, GridType GT, Hare.Geometry.Point SampleOrigin, double MindimX, double MindimY, double MindimZ)
                : base(Rm_in, ref S_in, ref M_in, fmax_in, tmax_in, GridType.Freefield, SampleOrigin, MindimX, MindimY, MindimZ)
                {
                    Build_Mesh_Sections();
                    Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(Utilities.RC_PachTools.HPttoRPt(RDD_Location(SD.X[0], SD.Y[0], SD.Z[0])));
                    if (Mic.X.Length > 0) Rhino.RhinoDoc.ActiveDoc.Objects.AddPoint(Utilities.RC_PachTools.HPttoRPt(RDD_Location(Mic.X[0], Mic.Y[0], Mic.Z[0])));
                }

                private void Build_Mesh_Sections()
                {
                    ///
                    ///Testing source and receiver positions
                    ///

                    int ct = -1;
                    m_templateX = new Rhino.Geometry.Mesh[2];
                    m_templateX[0] = new Rhino.Geometry.Mesh();
                    m_templateX[1] = new Rhino.Geometry.Mesh();
                    m_templateY = new Rhino.Geometry.Mesh[2];
                    m_templateY[0] = new Rhino.Geometry.Mesh();
                    m_templateY[1] = new Rhino.Geometry.Mesh();
                    m_templateZ = new Rhino.Geometry.Mesh[2];
                    m_templateZ[0] = new Rhino.Geometry.Mesh();
                    m_templateZ[1] = new Rhino.Geometry.Mesh();

                    double rt2 = Math.Sqrt(2);

                    Hare.Geometry.Point min = RDD_Location(Bounds.Min_PT, 0, 0, 0, dx, dy, dz);

                    for (int i = 0; i < 2; i++)
                    {
                        ct = -1;
                        for (int y = 0; y < PFrame[i].Length; y++)
                        {
                            for (int z = 0; z < PFrame[i][y].Length; z++)
                            {
                                ct++;
                                Rhino.Geometry.Point3d pt = Utilities.RC_PachTools.HPttoRPt(RDD_Location(Bounds.Min_PT, i, y, z, dx, dy, dz)); //new Rhino.Geometry.Point3d(PFrame[0][0][0].Pt.x, PFrame[i][y][z].Pt.y, PFrame[i][y][z].Pt.z);
                                pt.X = min.x;
                                m_templateX[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(0, dy, dz)));
                                m_templateX[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(0, dy, -dz)));
                                m_templateX[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(0, -dy, -dz)));
                                m_templateX[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(0, -dy, dz)));
                                int ct4 = ct * 4;
                                m_templateX[i].Faces.AddFace(ct4, ct4 + 1, ct4 + 2, ct4 + 3);
                            }
                        }

                        ct = -1;

                        for (int x = 0; x < PFrame.Length; x++)
                        {
                            for (int z = 0; z < PFrame[x][i].Length; z++)
                            {
                                ct++;
                                Rhino.Geometry.Point3d pt = Utilities.RC_PachTools.HPttoRPt(RDD_Location(Bounds.Min_PT, x, i, z, dx, dy, dz)); //new Rhino.Geometry.Point3d(PFrame[x][i][z].Pt.x, PFrame[0][0][0].Pt.y, PFrame[x][i][z].Pt.z);
                                pt.Y = min.y;
                                m_templateY[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(dx, 0, 0)));
                                m_templateY[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(0, 0, -dz)));
                                m_templateY[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(-dx, 0, 0)));
                                m_templateY[i].Vertices.Add((Rhino.Geometry.Point3d)(pt + new Rhino.Geometry.Point3d(0, 0, dz)));
                                int ct4 = ct * 4;
                                m_templateY[i].Faces.AddFace(ct4, ct4 + 1, ct4 + 2, ct4 + 3);
                            }
                        }

                        ct = -1;

                        for (int x = 0; x < PFrame.Length; x++)
                        {
                            for (int y = 0; y < PFrame[x].Length; y++)
                            {
                                ct++;

                                Rhino.Geometry.Point3d pt = Utilities.RC_PachTools.HPttoRPt(RDD_Location(Bounds.Min_PT, x, y, i, dx, dy, dz)); //new Rhino.Geometry.Point3d(PFrame[x][y][i].Pt.x, PFrame[x][y][i].Pt.y, PFrame[0][0][0].Pt.z);
                                pt.Z = min.z;
                                m_templateZ[i].Vertices.Add((Rhino.Geometry.Point3d)(pt - new Rhino.Geometry.Point3d(dx, 0, 0)));
                                m_templateZ[i].Vertices.Add((Rhino.Geometry.Point3d)(pt - new Rhino.Geometry.Point3d(0, -dy, 0)));
                                m_templateZ[i].Vertices.Add((Rhino.Geometry.Point3d)(pt - new Rhino.Geometry.Point3d(-dx, 0, 0)));
                                m_templateZ[i].Vertices.Add((Rhino.Geometry.Point3d)(pt - new Rhino.Geometry.Point3d(0, dy, 0)));
                                int ct4 = ct * 4;
                                m_templateZ[i].Faces.AddFace(ct4, ct4 + 1, ct4 + 2, ct4 + 3);
                            }
                        }

                        m_templateX[i].Normals.ComputeNormals();
                        m_templateX[i].FaceNormals.ComputeFaceNormals();
                        m_templateY[i].Normals.ComputeNormals();
                        m_templateY[i].FaceNormals.ComputeFaceNormals();
                        m_templateZ[i].Normals.ComputeNormals();
                        m_templateZ[i].FaceNormals.ComputeFaceNormals();
                    }
                }
            }
        }
    }
}