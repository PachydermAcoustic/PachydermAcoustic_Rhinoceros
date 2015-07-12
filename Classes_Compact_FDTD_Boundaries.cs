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
using System.Linq;
using System.Text;
using Pachyderm_Acoustic.Environment;
using Hare.Geometry;
using System.Numerics;

namespace Pachyderm_Acoustic
{
    namespace Numeric
    {
        namespace TimeDomain
        {
            public partial class Acoustic_Compact_FDTD : Simulation_Type
            {
                public class Bound_Node : P_Node
                {
                    //Kowalczyk Boundary Filter Node.
                    IIR_DIF[] filter;
                    List<double[]> acoef;
                    public List<Boundary> B_List;
                    Boundary Flags;
                    System.Numerics.Complex ab1 = 0;
                    System.Numerics.Complex abDenom = 0;
                    int[] id;

                    public Bound_Node(Point loc, double rho0, double dt, double dx, double C, int[] id_in, List<double[]> acoef_in, List<Boundary> B_in)
                        : base(loc)//, rho0, dt, dx, C, id_in)
                    {
                        id = id_in;
                        acoef = acoef_in;
                        B_List = B_in;
                        Flags = Boundary.None;
                        for (int i = 0; i < B_in.Count; i++) Flags |= B_in[i];
                    }

                    public override void UpdateIWB()
                    {
                        X = Xpos_Link.P + Xneg_Link.P;
                        Y = Ypos_Link.P + Yneg_Link.P;
                        Z = Zpos_Link.P + Zneg_Link.P;
                        Pnf = 0;// ((14.0 / 48.0) * (X + Y + Z) - (9.0 / 8.0) * Pn + ab1 * Pn_1);

                        for (int i = 0; i < filter.Length; i++)
                        {
                            Pnf += filter[i].g_b_term(); //* Courrant2 (1 for IWB) 
                            filter[i].Update(Pnf, Pn_1);
                        }
                        System.Numerics.Complex P2 = 0;
                        foreach (P_Node P in this.Links2) P2 += P.P;
                        P2 *= 0;// 3.0 / 32.0;

                        System.Numerics.Complex P3 = 0;
                        foreach (P_Node P in this.Links3) P3 += P.P;
                        P3 *= 0;//1.0 / 64.0;

                        Pnf += (P2 + P3);
                        Pnf *= abDenom;
                    }

                    //public override void Link_Nodes(ref P_Node[, ,] Frame, int x, int y, int z)
                    //{
                    //    Complex[] abs_poles = new Complex[] { new Complex(-0.212854, -0.285357), new Complex(-.212854, 0.285357) };
                    //    Complex[] abs_zeros = new Complex[] { 0.876, 0.876 };

                    //    Complex[] ref_poles = new Complex[] { new Complex(0, 0), new Complex(0, 0) };
                    //    Complex[] ref_zeros = new Complex[] { new Complex(0, 0), new Complex(0, 0) };

                    //    Boundary BFlags = Flags;
                    //    base.Link_Nodes(ref Frame, x, y, z);

                    //    #region General Node Setup Algorithm
                    //    foreach (Boundary B in Face_Combos)
                    //    {
                    //        switch ((BFlags & B))
                    //        {
                    //            case Boundary.AXNeg | Boundary.AXPos | Boundary.AYNeg | Boundary.AYPos | Boundary.AZNeg | Boundary.AZPos:
                    //                Xpos_Link = Xneg_Link = Ypos_Link = Yneg_Link = Zpos_Link = Zneg_Link = new Null_Node();
                    //                BFlags ^= (Boundary.AXNeg | Boundary.AXPos | Boundary.AYNeg | Boundary.AYPos | Boundary.AZNeg | Boundary.AZPos);
                    //                break;
                    //            case Boundary.AXNeg | Boundary.AXPos:
                    //                Xpos_Link = Xneg_Link = new Null_Node();
                    //                BFlags ^= (Boundary.AXNeg | Boundary.AXPos);
                    //                break;
                    //            case Boundary.AYNeg | Boundary.AYPos:
                    //                Ypos_Link = Yneg_Link = new Null_Node();
                    //                BFlags ^= (Boundary.AYNeg | Boundary.AYPos);
                    //                break;
                    //            case Boundary.AZNeg | Boundary.AZPos:
                    //                Ypos_Link = Yneg_Link = new Null_Node();
                    //                BFlags ^= (Boundary.AZNeg | Boundary.AZPos);
                    //                break;
                    //            case Boundary.AXPos:
                    //                Xpos_Link = new Null_Node();
                    //                BFlags ^= Boundary.AXPos;
                    //                break;
                    //            case Boundary.AXNeg:
                    //                Xneg_Link = new Null_Node();
                    //                BFlags ^= Boundary.AXNeg;
                    //                break;
                    //            case Boundary.AYPos:
                    //                Ypos_Link = new Null_Node();
                    //                BFlags ^= Boundary.AYPos;
                    //                break;
                    //            case Boundary.AYNeg:
                    //                Yneg_Link = new Null_Node();
                    //                BFlags ^= Boundary.AYNeg;
                    //                break;
                    //            case Boundary.AZPos:
                    //                Zpos_Link = new Null_Node();
                    //                BFlags ^= Boundary.AZPos;
                    //                break;
                    //            case Boundary.AZNeg:
                    //                Zneg_Link = new Null_Node();
                    //                BFlags ^= Boundary.AZNeg;
                    //                break;
                    //        }
                    //        if (BFlags == Boundary.None) break;
                    //    }

                    //    Set the filters
                    //    List<DIF_IWB_2p> f = new List<DIF_IWB_2p>();
                    //    for (int i = 0; i < B_List.Count; i++)
                    //    {
                    //        if (B_List[i] < Boundary.SDXPosYPos)
                    //        {
                    //            if (acoef[i][4] < 0.2)
                    //            {
                    //                f.Add(new DIF_IWB_2p(ref_zeros, ref_poles, 0.01, 1));
                    //            }
                    //            else
                    //            {
                    //                f.Add(new DIF_IWB_2p(abs_zeros, abs_poles, 0.81, 1));
                    //            }
                    //        }
                    //    }
                    //    for (int i = 0; i < f.Count; i++) ab1 += f[i].a_b;
                    //    filter = f.ToArray();
                    //    this.abDenom = 1 / (ab1 + 1);
                    //    ab1 -= 1;

                    //    int xdim = Frame.GetUpperBound(0);
                    //    int ydim = Frame.GetUpperBound(1);
                    //    int zdim = Frame.GetUpperBound(2);

                    //    if ((Boundary.DXPosYPosZPos & Flags) == Boundary.DXPosYPosZPos)
                    //    {
                    //        Links3[0] = new Null_Node();
                    //    }
                    //    if ((Boundary.DXNegYNegZNeg & Flags) == Boundary.DXNegYNegZNeg)
                    //    {
                    //        Links3[1] = new Null_Node();
                    //    }
                    //    if ((Boundary.DXNegYNegZPos & Flags) == Boundary.DXNegYNegZPos)
                    //    {
                    //        Links3[2] = new Null_Node();
                    //    }
                    //    if ((Boundary.DXPosYPosZNeg & Flags) == Boundary.DXPosYPosZNeg)
                    //    {
                    //        Links3[3] = new Null_Node();
                    //    }
                    //    if ((Boundary.DXNegYPosZPos & Flags) == Boundary.DXNegYPosZPos)
                    //    {
                    //        Links3[4] = new Null_Node();
                    //    }
                    //    if ((Boundary.DXPosYNegZNeg & Flags) == Boundary.DXPosYNegZNeg)
                    //    {
                    //        Links3[5] = new Null_Node();
                    //    }
                    //    if ((Boundary.DXNegYPosZNeg & Flags) == Boundary.DXNegYPosZNeg)
                    //    {
                    //        Links3[6] = new Null_Node();
                    //    }
                    //    if ((Boundary.DXPosYNegZPos & Flags) == Boundary.DXPosYNegZPos)
                    //    {
                    //        Links3[7] = new Null_Node();
                    //    }

                    //    Edges
                    //    todo - find the problem in this block of code. Links2 has a leak.
                    //    if (x < xdim && y < ydim) Links2[0] = Frame[x + 1, y + 1, z]; else Links2[0] = new Null_Node();
                    //    if ((Boundary.SDXPosYPos & Flags) == Boundary.SDXPosYPos)
                    //    {
                    //        Links2[0] = new Null_Node();
                    //    }
                    //    if (x > 0 && y > 0) Links2[1] = Frame[x - 1, y - 1, z]; else Links2[1] = new Null_Node();
                    //    if ((Boundary.SDXNegYNeg & Flags) == Boundary.SDXNegYNeg)
                    //    {
                    //        Links2[1] = new Null_Node();
                    //    }
                    //    if (x < xdim && y > 0) Links2[2] = Frame[x + 1, y - 1, z]; else Links2[2] = new Null_Node();
                    //    if ((Boundary.SDXPosYNeg & Flags) == Boundary.SDXPosYNeg)
                    //    {
                    //        Links2[2] = new Null_Node();
                    //    }
                    //    if (x > 0 && y < ydim) Links2[3] = Frame[x - 1, y + 1, z]; else Links2[3] = new Null_Node();
                    //    if ((Boundary.SDXNegYPos & Flags) == Boundary.SDXNegYPos) 
                    //    {
                    //        Links2[3] = new Null_Node();
                    //    }
                    //    if (x < xdim && z < zdim) Links2[4] = Frame[x + 1, y, z + 1]; else Links2[4] = new Null_Node();
                    //    if ((Boundary.SDXPosZPos & Flags) == Boundary.SDXPosZPos)
                    //    {
                    //        Links2[4] = new Null_Node();
                    //    }
                    //    if (x > 0 && z > 0) Links2[5] = Frame[x - 1, y, z - 1]; else Links2[5] = new Null_Node();
                    //    if ((Boundary.SDXNegZNeg & Flags) == Boundary.SDXNegZNeg)
                    //    {
                    //        Links2[5] = new Null_Node();
                    //    }
                    //    if (x > 0 && z < zdim) Links2[6] = Frame[x - 1, y, z + 1]; else Links2[6] = new Null_Node();
                    //    if ((Boundary.SDXNegZPos & Flags) == Boundary.SDXNegZPos)
                    //    {
                    //        Links2[6] = new Null_Node();
                    //    }
                    //    if (x < xdim && z > 0) Links2[7] = Frame[x + 1, y, z - 1]; else Links2[7] = new Null_Node();
                    //    if ((Boundary.SDXPosZNeg & Flags) == Boundary.SDXPosZNeg)
                    //    {
                    //        Links2[7] = new Null_Node();
                    //    }
                    //    if (y < ydim && z < zdim) Links2[8] = Frame[x, y + 1, z + 1]; else Links2[8] = new Null_Node();
                    //    if ((Boundary.SDYPosZPos & Flags) == Boundary.SDYPosZPos)
                    //    {
                    //        Links2[8] = new Null_Node();
                    //    }
                    //    if (y > 0 && z > 0) Links2[9] = Frame[x, y - 1, z - 1]; else Links2[9] = new Null_Node();
                    //    if ((Boundary.SDYNegZNeg & Flags) == Boundary.SDYNegZNeg)    
                    //    {
                    //        Links2[9] = new Null_Node();
                    //    }
                    //    if (y > 0 && z < zdim) Links2[10] = Frame[x, y - 1, z + 1]; else Links2[10] = new Null_Node();
                    //    if ((Boundary.SDYNegZPos & Flags) == Boundary.SDYNegZPos)
                    //    {
                    //        Links2[10] = new Null_Node();
                    //    }
                    //    if (y < ydim && z > 0) Links2[11] = Frame[x, y + 1, z - 1]; else Links2[11] = new Null_Node();
                    //    if ((Boundary.SDYPosZNeg & Flags) == Boundary.SDYPosZNeg)
                    //    {
                    //        Links2[11] = new Null_Node();
                    //    }

                    //    Rhino.RhinoApp.WriteLine("Meep");
                    //    if (BFlags == Boundary.None) return; else throw new Exception("Node does not fit the strict corner requirements. Raise the node density.");
                    //    #endregion
                    //}

                    public override void Link_Nodes(ref P_Node[][][] Frame, int x, int y, int z)
                    {
                        System.Numerics.Complex[] abs_poles = new System.Numerics.Complex[] { new System.Numerics.Complex(-0.212854, -0.285357), new System.Numerics.Complex(-.212854, 0.285357) };
                        System.Numerics.Complex[] abs_zeros = new System.Numerics.Complex[] { 0.876, 0.876 };

                        System.Numerics.Complex[] ref_poles = new System.Numerics.Complex[] { new System.Numerics.Complex(0, 0), new System.Numerics.Complex(0, 0) };
                        System.Numerics.Complex[] ref_zeros = new System.Numerics.Complex[] { new System.Numerics.Complex(0, 0), new System.Numerics.Complex(0, 0) };

                        //List<Complex[]> NodePoles = new List<Complex[]>();
                        //List<Complex[]> NodeZeros = new List<Complex[]>();
                        //List<IIR_DIF.IWB_Mask> Interp_Schemes = new List<IIR_DIF.IWB_Mask>();
                            Boundary BFlags = Flags;

                            Links2 = new P_Node[12];
                            Links3 = new P_Node[8];

                            #region Check for Outer Corner Node
                            foreach (Boundary B in OuterCorner) if (BFlags == B)//(BFlags & B) == B)
                                {
                                    //Set Corners here, by general algorithm.//
                                    int Sign = ((B & Boundary.AXPos) != 0) ? -1 : 1;
                                    Xneg_Link = Xpos_Link = Frame[x + Sign][y][z];
                                    Sign = ((B & Boundary.AYPos) != 0) ? -1 : 1;
                                    Yneg_Link = Ypos_Link = Frame[x][y + Sign][z];
                                    Sign = ((B & Boundary.AZPos) != 0) ? -1 : 1;
                                    Zneg_Link = Zpos_Link = Frame[x][y][z + Sign];

                                    #region EdgeNodes
                                    if ((B & Boundary.SDXNegYNeg) == 0)
                                    {
                                        Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x - 1][y - 1][z];
                                    }
                                    else if ((B & Boundary.SDXPosYPos) == 0)
                                    {
                                        Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x + 1][y + 1][z];
                                    }
                                    else if ((B & Boundary.SDXNegYPos) == 0)
                                    {
                                        Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x - 1][y + 1][z];
                                    }
                                    else if ((B & Boundary.SDXPosYNeg) == 0)
                                    {
                                        Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x + 1][y - 1][z];
                                    }

                                    if ((B & Boundary.SDXNegZNeg) == 0)
                                    {
                                        Links2[4] = Links2[5] = Links2[6] = Links2[7] = Frame[x - 1][y][z - 1];
                                    }
                                    else if ((B & Boundary.SDXPosZPos) == 0)
                                    {
                                        Links2[4] = Links2[5] = Links2[6] = Links2[7] = Frame[x + 1][y][z + 1];
                                    }
                                    else if ((B & Boundary.SDXNegZPos) == 0)
                                    {
                                        Links2[4] = Links2[5] = Links2[6] = Links2[7] = Frame[x - 1][y][z + 1];
                                    }
                                    else if ((B & Boundary.SDXPosZNeg) == 0)
                                    {
                                        Links2[4] = Links2[5] = Links2[6] = Links2[7] = Frame[x + 1][y][z - 1];
                                    }

                                    if ((B & Boundary.SDYNegZNeg) == 0)
                                    {
                                        Links2[8] = Links2[9] = Links2[10] = Links2[11] = Frame[x][y - 1][z - 1];
                                    }
                                    else if ((B & Boundary.SDYPosZPos) == 0)
                                    {
                                        Links2[8] = Links2[9] = Links2[10] = Links2[11] = Frame[x][y + 1][z + 1];
                                    }
                                    else if ((B & Boundary.SDYNegZPos) == 0)
                                    {
                                        Links2[8] = Links2[9] = Links2[10] = Links2[11] = Frame[x][y - 1][z + 1];
                                    }
                                    else if ((B & Boundary.SDYPosZNeg) == 0)
                                    {
                                        Links2[8] = Links2[9] = Links2[10] = Links2[11] = Frame[x][y + 1][z - 1];
                                    }
                                    #endregion
                                    #region CornerNodes
                                    if ((B & (Boundary.AXNeg | Boundary.AYNeg | Boundary.AZNeg)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y - 1][z - 1];
                                    }
                                    else if ((B & (Boundary.AXPos | Boundary.AYPos | Boundary.AZPos)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y + 1][z + 1];
                                    }
                                    else if ((B & (Boundary.AXNeg | Boundary.AYPos | Boundary.AZNeg)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y + 1][z - 1];
                                    }
                                    else if ((B & (Boundary.AXPos | Boundary.AYNeg | Boundary.AZPos)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y - 1][z + 1];
                                    }
                                    else if ((B & (Boundary.AXPos | Boundary.AYNeg | Boundary.AZNeg)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y - 1][z - 1];
                                    }
                                    else if ((B & (Boundary.AXNeg | Boundary.AYPos | Boundary.AZPos)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y + 1][z + 1];
                                    }
                                    else if ((B & (Boundary.AXPos | Boundary.AYPos | Boundary.AZNeg)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y + 1][z - 1];
                                    }
                                    else if ((B & (Boundary.AXNeg | Boundary.AYNeg | Boundary.AZPos)) == 0)
                                    {
                                        Links3[0] = Links3[1] = Links3[2] = Links3[3] = Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y - 1][z + 1];
                                    }
                                    #endregion

                                    List<DIF_IWB_2p> F = new List<DIF_IWB_2p>();

                                    //Set the filters
                                    for (int i = 0; i < B_List.Count; i++)
                                    {
                                        if (B_List[i] < Boundary.SDXPosYPos)
                                        {
                                            if (acoef[i][4] < 0.2)
                                            {
                                                F.Add(new DIF_IWB_2p(ref_zeros, ref_poles, 0.01, IIR_DIF.IWB_Mask.Axial));
                                            }
                                            else
                                            {
                                                F.Add(new DIF_IWB_2p(abs_zeros, abs_poles, 0.81, IIR_DIF.IWB_Mask.Axial));
                                            }
                                        }
                                    }

                                    for (int i = 0; i < F.Count; i++) ab1 += F[i].a_b;
                                    filter = F.ToArray();
                                    this.abDenom = 1 / (ab1 + 1);

                                    ////Remove the corner flags
                                    BFlags ^= B;
                                    ab1 -= 1;
                                    if (BFlags == Boundary.None) return; else throw new Exception("Node does not fit the strict corner requirements. Raise the node density.");
                                }
                            #endregion
                            #region Check for Edge Node
                            foreach (Boundary B in Outer_Edges) if (BFlags == B)//(BFlags & B) == B)
                                {
                                    if (((B & Boundary.AXNeg) != 0 || (B & Boundary.AXPos) != 0) && ((B & Boundary.AYNeg) != 0 || (B & Boundary.AYPos) != 0))
                                    {
                                        Zpos_Link = Frame[x][y][z + 1];
                                        Zneg_Link = Frame[x][y][z - 1];

                                        if ((B & Boundary.SDXNegYNeg) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x - 1][y][z];
                                            Ypos_Link = Yneg_Link = Frame[x][y - 1][z];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x - 1][y - 1][z];
                                            Links2[4] = Links2[5] = Frame[x - 1][y][z + 1];
                                            Links2[6] = Links2[7] = Frame[x - 1][y][z - 1];
                                            Links2[8] = Links2[9] = Frame[x][y - 1][z + 1];
                                            Links2[10] = Links2[11] = Frame[x][y - 1][z - 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x - 1][y - 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y - 1][z - 1];
                                        }

                                        if ((B & Boundary.SDXNegYPos) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x - 1][y][z];
                                            Ypos_Link = Yneg_Link = Frame[x][y + 1][z];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x - 1][y + 1][z];
                                            Links2[4] = Links2[5] = Frame[x - 1][y][z + 1];
                                            Links2[6] = Links2[7] = Frame[x - 1][y][z - 1];
                                            Links2[8] = Links2[9] = Frame[x][y + 1][z + 1];
                                            Links2[10] = Links2[11] = Frame[x][y + 1][z - 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x - 1][y + 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y + 1][z - 1];
                                        }

                                        if ((B & Boundary.SDXPosYPos) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x + 1][y][z];
                                            Ypos_Link = Yneg_Link = Frame[x][y + 1][z];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x + 1][y + 1][z];
                                            Links2[4] = Links2[5] = Frame[x + 1][y][z + 1];
                                            Links2[6] = Links2[7] = Frame[x + 1][y][z - 1];
                                            Links2[8] = Links2[9] = Frame[x][y + 1][z + 1];
                                            Links2[10] = Links2[11] = Frame[x][y + 1][z - 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y + 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y + 1][z - 1];
                                        }

                                        if ((B & Boundary.SDXPosYNeg) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x + 1][y][z];
                                            Ypos_Link = Yneg_Link = Frame[x][y - 1][z];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x + 1][y - 1][z];
                                            Links2[4] = Links2[5] = Frame[x + 1][y][z + 1];
                                            Links2[6] = Links2[7] = Frame[x + 1][y][z - 1];
                                            Links2[8] = Links2[9] = Frame[x][y - 1][z + 1];
                                            Links2[10] = Links2[11] = Frame[x][y - 1][z - 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y - 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y - 1][z - 1];
                                        }
                                    }
                                    else if (((B & Boundary.AXNeg) != 0 || (B & Boundary.AXPos) != 0) && ((B & Boundary.AZNeg) != 0 || (B & Boundary.AZPos) != 0))
                                    {
                                        Ypos_Link = Frame[x][y + 1][z];
                                        Yneg_Link = Frame[x][y - 1][z];

                                        if ((B & Boundary.SDXNegZNeg) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x - 1][y][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z - 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x - 1][y][z - 1];
                                            Links2[4] = Links2[5] = Frame[x - 1][y - 1][z];
                                            Links2[6] = Links2[7] = Frame[x][y - 1][z - 1];
                                            Links2[8] = Links2[9] = Frame[x - 1][y + 1][z];
                                            Links2[10] = Links2[11] = Frame[x][y + 1][z - 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x - 1][y + 1][z - 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y - 1][z - 1];
                                        }

                                        if ((B & Boundary.SDXNegZPos) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x - 1][y][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z + 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x - 1][y][z + 1];
                                            Links2[4] = Links2[5] = Frame[x - 1][y - 1][z];
                                            Links2[6] = Links2[7] = Frame[x][y - 1][z + 1];
                                            Links2[8] = Links2[9] = Frame[x - 1][y + 1][z];
                                            Links2[10] = Links2[11] = Frame[x][y + 1][z + 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x - 1][y + 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y - 1][z + 1];
                                        }

                                        if ((B & Boundary.SDXPosZPos) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x + 1][y][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z + 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x + 1][y][z + 1];
                                            Links2[4] = Links2[5] = Frame[x + 1][y - 1][z];
                                            Links2[6] = Links2[7] = Frame[x][y - 1][z + 1];
                                            Links2[8] = Links2[9] = Frame[x + 1][y + 1][z];
                                            Links2[10] = Links2[11] = Frame[x][y + 1][z + 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y + 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y - 1][z + 1];
                                        }

                                        if ((B & Boundary.SDXPosZNeg) == 0)
                                        {
                                            Xpos_Link = Xneg_Link = Frame[x + 1][y][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z - 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x + 1][y][z - 1];
                                            Links2[4] = Links2[5] = Frame[x + 1][y - 1][z];
                                            Links2[6] = Links2[7] = Frame[x][y - 1][z - 1];
                                            Links2[8] = Links2[9] = Frame[x + 1][y + 1][z];
                                            Links2[10] = Links2[11] = Frame[x][y + 1][z - 1];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y + 1][z - 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x + 1][y - 1][z - 1];
                                        }
                                    }
                                    else if (((B & Boundary.AYNeg) != 0 || (B & Boundary.AYPos) != 0) && ((B & Boundary.AZNeg) != 0 || (B & Boundary.AZPos) != 0))
                                    {
                                        Xpos_Link = Frame[x + 1][y][z];
                                        Xneg_Link = Frame[x - 1][y][z];

                                        if ((B & Boundary.SDYNegZNeg) == 0)
                                        {
                                            Ypos_Link = Yneg_Link = Frame[x][y - 1][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z - 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x][y - 1][z - 1];
                                            Links2[4] = Links2[5] = Frame[x - 1][y][z - 1];
                                            Links2[6] = Links2[7] = Frame[x - 1][y - 1][z];
                                            Links2[8] = Links2[9] = Frame[x + 1][y][z - 1];
                                            Links2[10] = Links2[11] = Frame[x + 1][y - 1][z];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y - 1][z - 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y - 1][z - 1];
                                        }

                                        if ((B & Boundary.SDYNegZPos) == 0)
                                        {
                                            Ypos_Link = Yneg_Link = Frame[x][y - 1][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z + 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x][y - 1][z + 1];
                                            Links2[4] = Links2[5] = Frame[x - 1][y][z + 1];
                                            Links2[6] = Links2[7] = Frame[x - 1][y - 1][z];
                                            Links2[8] = Links2[9] = Frame[x + 1][y][z + 1];
                                            Links2[10] = Links2[11] = Frame[x + 1][y - 1][z];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y - 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y - 1][z + 1];
                                        }

                                        if ((B & Boundary.SDYPosZPos) == 0)
                                        {
                                            Ypos_Link = Yneg_Link = Frame[x][y + 1][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z + 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x][y + 1][z + 1];
                                            Links2[4] = Links2[5] = Frame[x - 1][y][z + 1];
                                            Links2[6] = Links2[7] = Frame[x - 1][y + 1][z];
                                            Links2[8] = Links2[9] = Frame[x + 1][y][z + 1];
                                            Links2[10] = Links2[11] = Frame[x + 1][y + 1][z];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y + 1][z + 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y + 1][z + 1];

                                        }

                                        if ((B & Boundary.SDYPosZNeg) == 0)
                                        {
                                            Ypos_Link = Yneg_Link = Frame[x][y + 1][z];
                                            Zpos_Link = Zneg_Link = Frame[x][y][z - 1];
                                            Links2[0] = Links2[1] = Links2[2] = Links2[3] = Frame[x][y + 1][z - 1];
                                            Links2[4] = Links2[5] = Frame[x - 1][y][z - 1];
                                            Links2[6] = Links2[7] = Frame[x - 1][y + 1][z];
                                            Links2[8] = Links2[9] = Frame[x + 1][y][z - 1];
                                            Links2[10] = Links2[11] = Frame[x + 1][y + 1][z];
                                            Links3[0] = Links3[1] = Links3[2] = Links3[3] = Frame[x + 1][y + 1][z - 1];
                                            Links3[4] = Links3[5] = Links3[6] = Links3[7] = Frame[x - 1][y + 1][z - 1];
                                        }
                                    }

                                    //Set the filters
                                    List<DIF_IWB_2p> F = new List<DIF_IWB_2p>();

                                    for (int i = 0; i < B_List.Count; i++)
                                    {
                                        if (B_List[i] < Boundary.SDXPosYPos)
                                        {
                                            if (acoef[i][4] < 0.2)
                                            {
                                                F.Add(new DIF_IWB_2p(ref_zeros, ref_poles, 0.01, 0.75));
                                            }
                                            else
                                            {
                                                F.Add(new DIF_IWB_2p(abs_zeros, abs_poles, 0.81, 0.75));
                                            }
                                        }
                                    }

                                    for (int i = 0; i < F.Count; i++) ab1 += F[i].a_b;
                                    filter = F.ToArray();
                                    abDenom = 1 / (ab1 + 1);
                                    ////Remove the corner flags
                                    BFlags ^= B;
                                    ab1 -= 1;
                                    if (BFlags == Boundary.None) return; else throw new Exception("Node does not fit the strict Edge requirements. Raise the node density.");

                                }
                            #endregion
                            #region Check for Wall Node
                            foreach (Boundary B in Wall) if (BFlags == B)//(BFlags & B) == B)
                                {
                                    //Set Wall here//
                                    if ((B & Boundary.AXNeg) != 0)
                                    {
                                        Yneg_Link = Frame[x][y - 1][z];
                                        Ypos_Link = Frame[x][y + 1][z];
                                        Zneg_Link = Frame[x][y][z - 1];
                                        Zpos_Link = Frame[x][y][z + 1];
                                        Xpos_Link = Xneg_Link = Frame[x + 1][y][z];
                                        Links2[0] = Frame[x][y - 1][z - 1];
                                        Links2[1] = Frame[x][y + 1][z - 1];
                                        Links2[2] = Frame[x][y - 1][z + 1];
                                        Links2[3] = Frame[x][y + 1][z + 1];
                                        Links2[4] = Links2[5] = Frame[x + 1][y - 1][z];
                                        Links2[6] = Links2[7] = Frame[x + 1][y + 1][z];
                                        Links2[8] = Links2[9] = Frame[x + 1][y][z - 1];
                                        Links2[10] = Links2[11] = Frame[x + 1][y][z + 1];
                                        Links3[0] = Links3[1] = Frame[x + 1][y + 1][z + 1];
                                        Links3[2] = Links3[3] = Frame[x + 1][y - 1][z + 1];
                                        Links3[4] = Links3[5] = Frame[x + 1][y + 1][z - 1];
                                        Links3[6] = Links3[7] = Frame[x + 1][y - 1][z - 1];
                                    }
                                    else if ((B & Boundary.AXPos) != 0)
                                    {
                                        Yneg_Link = Frame[x][y - 1][z];
                                        Ypos_Link = Frame[x][y + 1][z];
                                        Zneg_Link = Frame[x][y][z - 1];
                                        Zpos_Link = Frame[x][y][z + 1];
                                        Xpos_Link = Xneg_Link = Frame[x - 1][y][z];
                                        Links2[0] = Frame[x][y - 1][z - 1];
                                        Links2[1] = Frame[x][y + 1][z - 1];
                                        Links2[2] = Frame[x][y - 1][z + 1];
                                        Links2[3] = Frame[x][y + 1][z + 1];
                                        Links2[4] = Links2[5] = Frame[x - 1][y - 1][z];
                                        Links2[6] = Links2[7] = Frame[x - 1][y + 1][z];
                                        Links2[8] = Links2[9] = Frame[x - 1][y][z - 1];
                                        Links2[10] = Links2[11] = Frame[x - 1][y][z + 1];
                                        Links3[0] = Links3[1] = Frame[x - 1][y + 1][z + 1];
                                        Links3[2] = Links3[3] = Frame[x - 1][y - 1][z + 1];
                                        Links3[4] = Links3[5] = Frame[x - 1][y + 1][z - 1];
                                        Links3[6] = Links3[7] = Frame[x - 1][y - 1][z - 1];
                                    }
                                    else if ((B & Boundary.AYNeg) != 0)
                                    {
                                        Xneg_Link = Frame[x - 1][y][z];
                                        Xpos_Link = Frame[x + 1][y][z];
                                        Zneg_Link = Frame[x][y][z - 1];
                                        Zpos_Link = Frame[x][y][z + 1];
                                        Ypos_Link = Yneg_Link = Frame[x][y + 1][z];
                                        Links2[0] = Frame[x - 1][y][z - 1];
                                        Links2[1] = Frame[x + 1][y][z - 1];
                                        Links2[2] = Frame[x - 1][y][z + 1];
                                        Links2[3] = Frame[x + 1][y][z + 1];
                                        Links2[4] = Links2[5] = Frame[x + 1][y + 1][z];
                                        Links2[6] = Links2[7] = Frame[x - 1][y + 1][z];
                                        Links2[8] = Links2[9] = Frame[x][y + 1][z + 1];
                                        Links2[10] = Links2[11] = Frame[x][y + 1][z - 1];
                                        Links3[0] = Links3[1] = Frame[x - 1][y + 1][z + 1];
                                        Links3[2] = Links3[3] = Frame[x + 1][y + 1][z + 1];
                                        Links3[4] = Links3[5] = Frame[x - 1][y + 1][z - 1];
                                        Links3[6] = Links3[7] = Frame[x + 1][y + 1][z - 1];
                                    }
                                    else if ((B & Boundary.AYPos) != 0)
                                    {
                                        Xneg_Link = Frame[x - 1][y][z];
                                        Xpos_Link = Frame[x + 1][y][z];
                                        Zneg_Link = Frame[x][y][z - 1];
                                        Zpos_Link = Frame[x][y][z + 1];
                                        Ypos_Link = Yneg_Link = Frame[x][y - 1][z];
                                        Links2[0] = Frame[x - 1][y][z - 1];
                                        Links2[1] = Frame[x + 1][y][z - 1];
                                        Links2[2] = Frame[x - 1][y][z + 1];
                                        Links2[3] = Frame[x + 1][y][z + 1];
                                        Links2[4] = Links2[5] = Frame[x + 1][y - 1][z];
                                        Links2[6] = Links2[7] = Frame[x - 1][y - 1][z];
                                        Links2[8] = Links2[9] = Frame[x][y - 1][z + 1];
                                        Links2[10] = Links2[11] = Frame[x][y - 1][z - 1];
                                        Links3[0] = Links3[1] = Frame[x + 1][y - 1][z + 1];
                                        Links3[2] = Links3[3] = Frame[x - 1][y - 1][z + 1];
                                        Links3[4] = Links3[5] = Frame[x + 1][y - 1][z - 1];
                                        Links3[6] = Links3[7] = Frame[x - 1][y - 1][z - 1];
                                    }
                                    else if ((B & Boundary.AZNeg) != 0)
                                    {
                                        Xneg_Link = Frame[x - 1][y][z];
                                        Xpos_Link = Frame[x + 1][y][z];
                                        Yneg_Link = Frame[x][y - 1][z];
                                        Ypos_Link = Frame[x][y + 1][z];
                                        Zpos_Link = Zneg_Link = Frame[x][y][z + 1];
                                        Links2[0] = Frame[x - 1][y - 1][z];
                                        Links2[1] = Frame[x + 1][y - 1][z];
                                        Links2[2] = Frame[x - 1][y + 1][z];
                                        Links2[3] = Frame[x + 1][y + 1][z];
                                        Links2[4] = Links2[5] = Frame[x + 1][y][z + 1];
                                        Links2[6] = Links2[7] = Frame[x - 1][y][z + 1];
                                        Links2[8] = Links2[9] = Frame[x][y + 1][z + 1];
                                        Links2[10] = Links2[11] = Frame[x][y - 1][z + 1];
                                        Links3[0] = Links3[1] = Frame[x - 1][y + 1][z + 1];
                                        Links3[2] = Links3[3] = Frame[x + 1][y + 1][z + 1];
                                        Links3[4] = Links3[5] = Frame[x - 1][y - 1][z + 1];
                                        Links3[6] = Links3[7] = Frame[x + 1][y - 1][z + 1];
                                    }
                                    else if ((B & Boundary.AZPos) != 0)
                                    {
                                        Xneg_Link = Frame[x - 1][y][z];
                                        Xpos_Link = Frame[x + 1][y][z];
                                        Yneg_Link = Frame[x][y - 1][z];
                                        Ypos_Link = Frame[x][y + 1][z];
                                        Zpos_Link = Zneg_Link = Frame[x][y][z - 1];
                                        Links2[0] = Frame[x - 1][y - 1][z];
                                        Links2[1] = Frame[x + 1][y - 1][z];
                                        Links2[2] = Frame[x - 1][y + 1][z];
                                        Links2[3] = Frame[x + 1][y + 1][z];
                                        Links2[4] = Links2[5] = Frame[x + 1][y][z - 1];
                                        Links2[6] = Links2[7] = Frame[x - 1][y][z - 1];
                                        Links2[8] = Links2[9] = Frame[x][y + 1][z - 1];
                                        Links2[10] = Links2[11] = Frame[x][y - 1][z - 1];
                                        Links3[0] = Links3[1] = Frame[x - 1][y + 1][z - 1];
                                        Links3[2] = Links3[3] = Frame[x + 1][y + 1][z - 1];
                                        Links3[4] = Links3[5] = Frame[x - 1][y - 1][z - 1];
                                        Links3[6] = Links3[7] = Frame[x + 1][y - 1][z - 1];
                                    }

                                    //Set the filters
                                    List<DIF_IWB_2p> F = new List<DIF_IWB_2p>();

                                    for (int i = 0; i < B_List.Count; i++)
                                    {
                                        if (B_List[i] < Boundary.SDXPosYPos)
                                        {
                                            if (acoef[i][4] < 0.2)
                                            {
                                                F.Add(new DIF_IWB_2p(ref_zeros, ref_poles, 0.01, 1));
                                            }
                                            else
                                            {
                                                F.Add(new DIF_IWB_2p(abs_zeros, abs_poles, 0.81, 1));
                                            }
                                        }
                                    }

                                    for (int i = 0; i < F.Count; i++) ab1 += F[i].a_b;
                                    filter = F.ToArray();
                                    this.abDenom = 1 / (ab1 + 1);
                                    ////Remove the corner flags
                                    BFlags ^= B;
                                    ab1 -= 1;
                                    if (BFlags == Boundary.None) return; else throw new Exception("Node does not fit the strict corner requirements. Raise the node density.");
                                }
                            #endregion
                            #region Check for Inner Corner
                            for (int i = 4; i < InnerCorner.Length; i++) if (BFlags == InnerCorner[i])//(BFlags & B) == B)
                                {
                                    base.Link_Nodes(ref Frame, x, y, z);
                                    if (((Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZNeg) & BFlags) != 0)
                                    {
                                        Links3[0] = Links3[1] = Frame[x + 1][y + 1][z + 1];
                                        Links3[2] = Links3[3] = Frame[x - 1][y + 1][z - 1];
                                        Links3[4] = Links3[5] = Frame[x + 1][y - 1][z - 1];
                                        Links3[6] = Links3[7] = Frame[x - 1][y - 1][z + 1];
                                    }
                                    else
                                    {
                                        Links3[0] = Links3[1] = Frame[x - 1][y - 1][z - 1];
                                        Links3[2] = Links3[3] = Frame[x + 1][y - 1][z + 1];
                                        Links3[4] = Links3[5] = Frame[x - 1][y + 1][z + 1];
                                        Links3[6] = Links3[7] = Frame[x + 1][y + 1][z - 1];
                                    }
                                    filter = new DIF_IWB_2p[0];
                                    this.abDenom = 1;
                                    ab1 = -1;
                                    return;
                                }
                            #endregion
                            #region Check for Inner Edge
                            for (int i = 0; i < EdgeCombos.Length; i++) if (BFlags == EdgeCombos[i])//(BFlags & B) == B)
                                {
                                    base.Link_Nodes(ref Frame, x, y, z);
                                    if ((EdgeCombos[0] == BFlags) || (EdgeCombos[1] == BFlags))
                                    {
                                        Links2[0] = Links2[1] = Frame[x + 1][y - 1][z];
                                        Links2[2] = Links2[3] = Frame[x - 1][y + 1][z];
                                        Links3[0] = Links3[4];
                                        Links3[1] = Links3[5];
                                        Links3[2] = Links3[6];
                                        Links3[3] = Links3[7];
                                        filter = new DIF_IWB_2p[0];
                                        this.abDenom = 1;
                                        ab1 = -1;
                                        return;
                                    }
                                    else if ((EdgeCombos[2] == BFlags) || (EdgeCombos[3] == BFlags))
                                    {
                                        Links2[0] = Links2[1] = Frame[x - 1][y - 1][z];
                                        Links2[2] = Links2[3] = Frame[x + 1][y + 1][z];
                                        Links3[4] = Links3[0];
                                        Links3[5] = Links3[1];
                                        Links3[6] = Links3[2];
                                        Links3[7] = Links3[3];
                                        filter = new DIF_IWB_2p[0];
                                        this.abDenom = 1;
                                        ab1 = -1;
                                        return;
                                    }
                                    else if ((EdgeCombos[4] == BFlags) || (EdgeCombos[5] == BFlags))
                                    {
                                        Links2[4] = Links2[5] = Frame[x + 1][y][z - 1];
                                        Links2[6] = Links2[7] = Frame[x - 1][y][z + 1];
                                        Links3[0] = Links3[2];
                                        Links3[1] = Links3[3];
                                        Links3[6] = Links3[4];
                                        Links3[7] = Links3[5];
                                        filter = new DIF_IWB_2p[0];
                                        this.abDenom = 1;
                                        ab1 = -1;
                                        return;
                                    }
                                    else if ((EdgeCombos[6] == BFlags) || (EdgeCombos[7] == BFlags))
                                    {
                                        Links2[4] = Links2[5] = Frame[x - 1][y][z - 1];
                                        Links2[6] = Links2[7] = Frame[x + 1][y][z + 1];
                                        Links3[2] = Links3[0];
                                        Links3[3] = Links3[1];
                                        Links3[4] = Links3[6];
                                        Links3[5] = Links3[7];
                                        filter = new DIF_IWB_2p[0];
                                        this.abDenom = 1;
                                        ab1 = -1;
                                        return;
                                    }
                                    else if ((EdgeCombos[8] == BFlags) || (EdgeCombos[9] == BFlags))
                                    {
                                        Links2[8] = Links2[9] = Frame[x][y + 1][z - 1];
                                        Links2[10] = Links2[11] = Frame[x][y - 1][z + 1];
                                        Links3[0] = Links3[2];
                                        Links3[1] = Links3[3];
                                        Links3[4] = Links3[6];
                                        Links3[5] = Links3[7];
                                        filter = new DIF_IWB_2p[0];
                                        this.abDenom = 1;
                                        ab1 = -1;
                                        return;
                                    }
                                    else if ((EdgeCombos[10] == BFlags) || (EdgeCombos[11] == BFlags))
                                    {
                                        Links2[8] = Links2[9] = Frame[x][y - 1][z - 1];
                                        Links2[10] = Links2[11] = Frame[x][y + 1][z + 1];
                                        Links3[2] = Links3[0];
                                        Links3[3] = Links3[1];
                                        Links3[6] = Links3[4];
                                        Links3[7] = Links3[5];
                                        filter = new DIF_IWB_2p[0];
                                        this.abDenom = 1;
                                        ab1 = -1;
                                        return;
                                    }
                                }
                            #endregion
                            #region General Node Setup Algorithm
                            base.Link_Nodes(ref Frame, x, y, z);
                            foreach (Boundary B in Face_Combos)
                            {
                                switch ((BFlags & B))
                                {
                                    case Boundary.AXNeg | Boundary.AXPos | Boundary.AYNeg | Boundary.AYPos | Boundary.AZNeg | Boundary.AZPos:
                                        Xpos_Link = Xneg_Link = Ypos_Link = Yneg_Link = Zpos_Link = Zneg_Link = new Null_Node();
                                        BFlags ^= (Boundary.AXNeg | Boundary.AXPos | Boundary.AYNeg | Boundary.AYPos | Boundary.AZNeg | Boundary.AZPos);
                                        break;
                                    case Boundary.AXNeg | Boundary.AXPos:
                                        Xpos_Link = Xneg_Link = new Null_Node();
                                        BFlags ^= (Boundary.AXNeg | Boundary.AXPos);
                                        break;
                                    case Boundary.AYNeg | Boundary.AYPos:
                                        Ypos_Link = Yneg_Link = new Null_Node();
                                        BFlags ^= (Boundary.AYNeg | Boundary.AYPos);
                                        break;
                                    case Boundary.AZNeg | Boundary.AZPos:
                                        Zpos_Link = Zneg_Link = new Null_Node();
                                        BFlags ^= (Boundary.AZNeg | Boundary.AZPos);
                                        break;
                                    case Boundary.AXPos:
                                        Xpos_Link = Xneg_Link;
                                        //Xpos_Link = new Null_Node();
                                        BFlags ^= Boundary.AXPos;
                                        break;
                                    case Boundary.AXNeg:
                                        Xneg_Link = Xpos_Link;
                                        //Xneg_Link = new Null_Node();
                                        BFlags ^= Boundary.AXNeg;
                                        break;
                                    case Boundary.AYPos:
                                        Ypos_Link = Yneg_Link;
                                        //Ypos_Link = new Null_Node();
                                        BFlags ^= Boundary.AYPos;
                                        break;
                                    case Boundary.AYNeg:
                                        Yneg_Link = Ypos_Link;
                                        //Yneg_Link = new Null_Node();
                                        BFlags ^= Boundary.AYNeg;
                                        break;
                                    case Boundary.AZPos:
                                        Zpos_Link = Zneg_Link;
                                        //Zpos_Link = new Null_Node();
                                        BFlags ^= Boundary.AZPos;
                                        break;
                                    case Boundary.AZNeg:
                                        Zneg_Link = Zpos_Link;
                                        //Zneg_Link = new Null_Node();
                                        BFlags ^= Boundary.AZNeg;
                                        break;
                                }
                                if (BFlags == Boundary.None) break;
                            }

                            //Set the filters
                            List<DIF_IWB_2p> f = new List<DIF_IWB_2p>();
                            for (int i = 0; i < B_List.Count; i++)
                            {
                                if (B_List[i] < Boundary.SDXPosYPos)
                                {
                                    if (acoef[i][4] < 0.2)
                                    {
                                        f.Add(new DIF_IWB_2p(ref_zeros, ref_poles, 0.01, 1));
                                    }
                                    else
                                    {
                                        f.Add(new DIF_IWB_2p(abs_zeros, abs_poles, 0.81, 1));
                                    }
                                }
                            }
                            for (int i = 0; i < f.Count; i++) ab1 += f[i].a_b;
                            filter = f.ToArray();
                            this.abDenom = 1 / (ab1 + 1);
                            ab1 -= 1;

                            int xdim = Frame.Length;
                            int ydim = Frame[0].Length;
                            int zdim = Frame[0][0].Length;

                            if ((Boundary.DXPosYPosZPos & Flags) == Boundary.DXPosYPosZPos)
                            {
                                Links3[0] = Links3[1];
                                //Links3[0] = new Null_Node();
                            }
                            if ((Boundary.DXNegYNegZNeg & Flags) == Boundary.DXNegYNegZNeg)
                            {
                                Links3[1] = Links3[0];
                                //Links3[1] = new Null_Node();
                            }
                            if ((Boundary.DXNegYNegZPos & Flags) == Boundary.DXNegYNegZPos)
                            {
                                Links3[2] = Links3[3];
                                //Links3[2] = new Null_Node();
                            }
                            if ((Boundary.DXPosYPosZNeg & Flags) == Boundary.DXPosYPosZNeg)
                            {
                                Links3[3] = Links3[2];
                                //Links3[3] = new Null_Node();
                            }
                            if ((Boundary.DXNegYPosZPos & Flags) == Boundary.DXNegYPosZPos)
                            {
                                Links3[4] = Links3[5];
                                //Links3[4] = new Null_Node();
                            }
                            if ((Boundary.DXPosYNegZNeg & Flags) == Boundary.DXPosYNegZNeg)
                            {
                                Links3[5] = Links3[4];
                                //Links3[5] = new Null_Node();
                            }
                            if ((Boundary.DXNegYPosZNeg & Flags) == Boundary.DXNegYPosZNeg)
                            {
                                Links3[6] = Links3[7];
                                //Links3[6] = new Null_Node();
                            }
                            if ((Boundary.DXPosYNegZPos & Flags) == Boundary.DXPosYNegZPos)
                            {
                                Links3[7] = Links3[6];
                                //Links3[7] = new Null_Node();
                            }

                            //Edges
                            //todo - find the problem in this block of code. Links2 has a leak.
                            //if (x < xdim && y < ydim) Links2[0] = Frame[x + 1, y + 1, z]; else Links2[0] = new Null_Node();
                            if ((Boundary.SDXPosYPos & Flags) == Boundary.SDXPosYPos)
                            {
                                Links2[0] = Links2[1];
                                //Links2[0] = new Null_Node();
                            }
                            //if (x > 0 && y > 0) Links2[1] = Frame[x - 1, y - 1, z]; else Links2[1] = new Null_Node();
                            if ((Boundary.SDXNegYNeg & Flags) == Boundary.SDXNegYNeg)
                            {
                                Links2[1] = Links2[0];
                                //Links2[1] = new Null_Node();
                            }
                            //if (x < xdim && y > 0) Links2[2] = Frame[x + 1, y - 1, z]; else Links2[2] = new Null_Node();
                            if ((Boundary.SDXPosYNeg & Flags) == Boundary.SDXPosYNeg)
                            {
                                Links2[2] = Links2[3];
                                //Links2[2] = new Null_Node();
                            }
                            //if (x > 0 && y < ydim) Links2[3] = Frame[x - 1, y + 1, z]; else Links2[3] = new Null_Node();
                            if ((Boundary.SDXNegYPos & Flags) == Boundary.SDXNegYPos)
                            {
                                Links2[3] = Links2[2];
                                //Links2[3] = new Null_Node();
                            }
                            //if (x < xdim && z < zdim) Links2[4] = Frame[x + 1, y, z + 1]; else Links2[4] = new Null_Node();
                            if ((Boundary.SDXPosZPos & Flags) == Boundary.SDXPosZPos)
                            {
                                Links2[4] = Links2[5];
                                //Links2[4] = new Null_Node();
                            }
                            //if (x > 0 && z > 0) Links2[5] = Frame[x - 1, y, z - 1]; else Links2[5] = new Null_Node();
                            if ((Boundary.SDXNegZNeg & Flags) == Boundary.SDXNegZNeg)
                            {
                                Links2[5] = Links2[4];
                                //Links2[5] = new Null_Node();
                            }
                            //if (x > 0 && z < zdim) Links2[6] = Frame[x - 1, y, z + 1]; else Links2[6] = new Null_Node();
                            if ((Boundary.SDXNegZPos & Flags) == Boundary.SDXNegZPos)
                            {
                                Links2[6] = Links2[7];
                                //Links2[6] = new Null_Node();
                            }
                            //if (x < xdim && z > 0) Links2[7] = Frame[x + 1, y, z - 1]; else Links2[7] = new Null_Node();
                            if ((Boundary.SDXPosZNeg & Flags) == Boundary.SDXPosZNeg)
                            {
                                Links2[7] = Links2[6];
                                //Links2[7] = new Null_Node();
                            }
                            //if (y < ydim && z < zdim) Links2[8] = Frame[x, y + 1, z + 1]; else Links2[8] = new Null_Node();
                            if ((Boundary.SDYPosZPos & Flags) == Boundary.SDYPosZPos)
                            {
                                Links2[8] = Links2[9];
                                //Links2[8] = new Null_Node();
                            }
                            //if (y > 0 && z > 0) Links2[9] = Frame[x, y - 1, z - 1]; else Links2[9] = new Null_Node();
                            if ((Boundary.SDYNegZNeg & Flags) == Boundary.SDYNegZNeg)
                            {
                                Links2[9] = Links2[8];
                                //Links2[9] = new Null_Node();
                            }
                            //if (y > 0 && z < zdim) Links2[10] = Frame[x, y - 1, z + 1]; else Links2[10] = new Null_Node();
                            if ((Boundary.SDYNegZPos & Flags) == Boundary.SDYNegZPos)
                            {
                                Links2[10] = Links2[11];
                                //Links2[10] = new Null_Node();
                            }
                            //if (y < ydim && z > 0) Links2[11] = Frame[x, y + 1, z - 1]; else Links2[11] = new Null_Node();
                            if ((Boundary.SDYPosZNeg & Flags) == Boundary.SDYPosZNeg)
                            {
                                Links2[11] = Links2[10];
                                //Links2[11] = new Null_Node();
                            }

                            Rhino.RhinoApp.WriteLine("Meep");
                            //if (BFlags == Boundary.None) return; else throw new Exception("Node does not fit the strict corner requirements. Raise the node density.");
                            #endregion
                    }

                    public class DIF_IWB_2p : IIR_DIF
                    {
                        double[] ax;
                        double[] bx;
                        System.Numerics.Complex[] xx;
                        System.Numerics.Complex[] yx;
                        System.Numerics.Complex rec_b0;
                        System.Numerics.Complex gn;
                        double interp;

                        public DIF_IWB_2p(System.Numerics.Complex[] Poles, System.Numerics.Complex[] Zeros, double scale, double M)
                            : this(Poles, Zeros, scale)
                        {
                            interp = M;
                        }

                        public DIF_IWB_2p(System.Numerics.Complex[] Poles, System.Numerics.Complex[] Zeros, double scale, IWB_Mask M)
                            : this(Poles, Zeros, scale)
                        {
                            switch (M)
                            {
                                case IWB_Mask.Axial:
                                    interp = .25;
                                    break;
                                case IWB_Mask.SideDiagonal:
                                    interp = .125;
                                    break;
                                case IWB_Mask.Diagonal:
                                    interp = .0625;
                                    break;
                            }
                        }

                        public DIF_IWB_2p(System.Numerics.Complex[] Poles, System.Numerics.Complex[] Zeros, double scale)
                        {
                            ax = new double[3];
                            ax[0] = 1;
                            ax[1] = -(Poles[0].Real + Poles[1].Real);
                            ax[2] = Poles[0].Real * Poles[1].Real;

                            bx = new double[3];
                            bx[0] = 1 / scale;
                            bx[1] = -(Zeros[0].Real + Zeros[1].Real) / scale;
                            bx[2] = Zeros[0].Real * Zeros[1].Real / scale;

                            //if (Poles[0].Imag != 0)
                            //{
                            //  assert (pole2 == std::conj (pole1));

                            //  a1 = -2 * pole1.real();
                            //  a2 = std::norm (pole1);
                            //}
                            //else
                            //{
                            //  assert (pole2.imag() == 0);

                            //  a1 = -(pole1.real() + pole2.real());
                            //  a2 =   pole1.real() * pole2.real();
                            //}

                            //const double b0 = 1;
                            //double b1;
                            //double b2;

                            //if (zero1.imag() != 0)
                            //{
                            //  assert (zero2 == std::conj (zero1));

                            //  b1 = -2 * zero1.real();
                            //  b2 = std::norm (zero1);
                            //}
                            //else
                            //{
                            //  assert (zero2.imag() == 0);

                            //  b1 = -(zero1.real() + zero2.real());
                            //  b2 =   zero1.real() * zero2.real();
                            //}

                            if (ax.Length != bx.Length) throw new Exception("Number of Poles and Number of Zeros must be equal.");
                            rec_b0 = 1 / bx[0];
                            a_b = ax[0] / bx[0];
                            xx = new System.Numerics.Complex[2];
                            yx = new System.Numerics.Complex[2];
                        }

                        public override System.Numerics.Complex g_b_term()
                        {
                            gn = bx[1] * xx[0] - ax[1] * yx[0] + bx[2] * xx[1] - ax[2] * yx[1];
                            return gn * rec_b0 * interp;
                        }

                        /// <summary>
                        /// Do before updating boundary node pressure values, but after getting the g-term.
                        /// </summary>
                        /// <param name="Pn"></param>
                        public override void Update(System.Numerics.Complex Pnf, System.Numerics.Complex Pn_1)
                        {
                            xx[1] = xx[0];
                            yx[1] = yx[0];
                            xx[0] = a_b * (Pnf - Pn_1) - gn / bx[0];
                            yx[0] = (bx[0] * xx[0] + gn) / ax[0];
                        }

                        public System.Numerics.Complex A0
                        {
                            get { return ax[0]; }
                        }

                        public System.Numerics.Complex B0
                        {
                            get { return bx[0]; }
                        }
                    }
                    public abstract class IIR_DIF
                    {
                        public System.Numerics.Complex a_b;
                        protected int[] DIR;

                        public enum IWB_Mask
                        {
                            Axial,
                            SideDiagonal,
                            Diagonal
                        }

                        //public Complex a_b_sub_1 { get { return ab1; } }
                        //public Complex a_b_recip { get { return abdenom; } }
                        public abstract System.Numerics.Complex g_b_term();
                        //public Complex ab;
                        public abstract void Update(System.Numerics.Complex Pnf, System.Numerics.Complex Pn_1);
                    }
                    
                    [Flags]
                    public enum Boundary
                    {
                        None = 0x0000000,
                        AXPos = 0x0000001,
                        AXNeg = 0x0000002,
                        AYPos = 0x0000004,
                        AYNeg = 0x0000008,
                        AZPos = 0x0000010,
                        AZNeg = 0x0000020,
                        SDXPosYPos = 0x0000040,
                        SDXPosYNeg = 0x0000080,
                        SDXNegYPos = 0x0000100,
                        SDXNegYNeg = 0x0000200,
                        SDXPosZPos = 0x0000400,
                        SDXPosZNeg = 0x0000800,
                        SDXNegZPos = 0x0001000,
                        SDXNegZNeg = 0x0002000,
                        SDYPosZPos = 0x0004000,
                        SDYPosZNeg = 0x0008000,
                        SDYNegZPos = 0x0010000,
                        SDYNegZNeg = 0x0020000,
                        DXPosYPosZPos = 0x0040000,
                        DXNegYPosZPos = 0x0080000,
                        DXPosYNegZPos = 0x0100000,
                        DXNegYNegZPos = 0x0200000,
                        DXPosYPosZNeg = 0x0400000,
                        DXNegYPosZNeg = 0x0800000,
                        DXPosYNegZNeg = 0x1000000,
                        DXNegYNegZNeg = 0x2000000
                    }
                    #region Quick Recognition Boundary Conditions
                    static Boundary[] Face_Combos = new Boundary[10]
                {
                    Boundary.AXNeg | Boundary.AXPos | Boundary.AYNeg | Boundary.AYPos | Boundary.AZNeg | Boundary.AZPos,
                    Boundary.AXNeg | Boundary.AXPos,
                    Boundary.AYNeg | Boundary.AYPos,
                    Boundary.AZNeg | Boundary.AZPos,
                    Boundary.AXNeg,
                    Boundary.AXPos,
                    Boundary.AYNeg,
                    Boundary.AYPos,
                    Boundary.AZNeg,
                    Boundary.AZPos
                };

                    static Boundary[] Wall = new Boundary[6] 
                    {
                        Boundary.AZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos | Boundary.SDXNegZPos | Boundary.SDXPosZPos | Boundary.SDYNegZPos | Boundary.SDYPosZPos,
                        Boundary.AZNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg | Boundary.SDXNegZNeg | Boundary.SDXPosZNeg | Boundary.SDYNegZNeg | Boundary.SDYPosZNeg,
                        Boundary.AYPos | Boundary.DXNegYPosZNeg | Boundary.DXNegYPosZPos | Boundary.DXPosYPosZNeg | Boundary.DXPosYPosZPos | Boundary.SDXNegYPos | Boundary.SDXPosYPos | Boundary.SDYPosZNeg | Boundary.SDYPosZPos,
                        Boundary.AYNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYNegZPos | Boundary.DXPosYNegZNeg | Boundary.DXPosYNegZPos | Boundary.SDXNegYNeg | Boundary.SDXPosYNeg | Boundary.SDYNegZNeg | Boundary.SDYNegZPos,
                        Boundary.AXPos | Boundary.DXPosYNegZNeg | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXPosYPosZPos | Boundary.SDXPosYNeg | Boundary.SDXPosYPos | Boundary.SDXPosZNeg | Boundary.SDXPosZPos,
                        Boundary.AXNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZNeg | Boundary.DXNegYPosZPos | Boundary.SDXNegYNeg | Boundary.SDXNegYPos | Boundary.SDXNegZNeg | Boundary.SDXNegZPos
                    };

                    static Boundary[] Outer_Edges = new Boundary[12] 
                    {
                        Boundary.AZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos | Boundary.SDXNegZPos | Boundary.SDXPosZPos | Boundary.SDYNegZPos | Boundary.SDYPosZPos |    Boundary.AYPos | Boundary.SDXNegYPos | Boundary.SDXPosYPos |    Boundary.SDYPosZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYPosZNeg,
                        Boundary.AZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos | Boundary.SDXNegZPos | Boundary.SDXPosZPos | Boundary.SDYNegZPos | Boundary.SDYPosZPos |    Boundary.AYNeg | Boundary.SDXNegYNeg | Boundary.SDXPosYNeg |    Boundary.SDYNegZNeg | Boundary.DXNegYNegZNeg | Boundary.DXPosYNegZNeg,
                        Boundary.AZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos | Boundary.SDXNegZPos | Boundary.SDXPosZPos | Boundary.SDYNegZPos | Boundary.SDYPosZPos |    Boundary.AXPos | Boundary.SDXPosYNeg | Boundary.SDXPosYPos |    Boundary.SDXPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg,
                        Boundary.AZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos | Boundary.SDXNegZPos | Boundary.SDXPosZPos | Boundary.SDYNegZPos | Boundary.SDYPosZPos |    Boundary.AXNeg | Boundary.SDXNegYNeg | Boundary.SDXNegYPos |    Boundary.SDXNegZNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg,
                        Boundary.AZNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg | Boundary.SDXNegZNeg | Boundary.SDXPosZNeg | Boundary.SDYNegZNeg | Boundary.SDYPosZNeg |    Boundary.AYPos | Boundary.SDXNegYPos | Boundary.SDXPosYPos |    Boundary.SDYPosZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYPosZPos,
                        Boundary.AZNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg | Boundary.SDXNegZNeg | Boundary.SDXPosZNeg | Boundary.SDYNegZNeg | Boundary.SDYPosZNeg |    Boundary.AYNeg | Boundary.SDXNegYNeg | Boundary.SDXPosYNeg |    Boundary.SDYNegZPos | Boundary.DXNegYNegZPos | Boundary.DXPosYNegZPos,
                        Boundary.AZNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg | Boundary.SDXNegZNeg | Boundary.SDXPosZNeg | Boundary.SDYNegZNeg | Boundary.SDYPosZNeg |    Boundary.AXPos | Boundary.SDXPosYNeg | Boundary.SDXPosYPos |    Boundary.SDXPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos,
                        Boundary.AZNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg | Boundary.SDXNegZNeg | Boundary.SDXPosZNeg | Boundary.SDYNegZNeg | Boundary.SDYPosZNeg |    Boundary.AXNeg | Boundary.SDXNegYNeg | Boundary.SDXNegYPos |    Boundary.SDXNegZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos,
                        Boundary.DXNegYNegZPos | Boundary.SDXNegZPos | Boundary.DXNegYPosZPos | Boundary.SDYPosZPos | Boundary.DXPosYPosZPos | Boundary.SDXNegYNeg | Boundary.AXNeg | Boundary.SDXNegYPos | Boundary.AYPos | Boundary.SDXPosYPos | Boundary.DXNegYNegZNeg | Boundary.SDXNegZNeg | Boundary.DXNegYPosZNeg | Boundary.SDYPosZNeg | Boundary.DXPosYPosZNeg,
                        Boundary.DXNegYPosZPos | Boundary.SDYPosZPos | Boundary.DXPosYPosZPos | Boundary.SDXPosZPos | Boundary.DXPosYNegZPos | Boundary.SDXNegYPos | Boundary.AYPos | Boundary.SDXPosYPos | Boundary.AXPos | Boundary.SDXPosYNeg | Boundary.DXNegYPosZNeg | Boundary.SDYPosZNeg | Boundary.DXPosYPosZNeg | Boundary.SDXPosZNeg | Boundary.DXPosYNegZNeg,
                        Boundary.DXPosYPosZPos | Boundary.SDXPosZPos | Boundary.DXPosYNegZPos | Boundary.SDYNegZPos | Boundary.DXNegYNegZPos | Boundary.SDXPosYPos | Boundary.AXPos | Boundary.SDXPosYNeg | Boundary.AYNeg | Boundary.SDXNegYNeg | Boundary.DXPosYPosZNeg | Boundary.SDXPosZNeg | Boundary.DXPosYNegZNeg | Boundary.SDYNegZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.DXPosYNegZPos | Boundary.SDYNegZPos | Boundary.DXNegYNegZPos | Boundary.SDXNegZPos | Boundary.DXNegYPosZPos | Boundary.SDXPosYNeg | Boundary.AYNeg | Boundary.SDXNegYNeg | Boundary.AXNeg | Boundary.SDXNegYPos | Boundary.DXPosYNegZNeg | Boundary.SDYNegZNeg | Boundary.DXNegYNegZNeg | Boundary.SDXNegZNeg | Boundary.DXNegYPosZNeg
                    };

                    static Boundary[] OuterCorner = new Boundary[8]
                    {
                        Boundary.AXNeg | Boundary.AYPos | Boundary.AZNeg | Boundary.SDXPosYPos | Boundary.SDXNegYPos | Boundary.SDXNegYNeg | Boundary.SDXPosZNeg | Boundary.SDXNegZPos | Boundary.SDXNegZNeg | Boundary.SDYPosZPos | Boundary.SDYPosZNeg | Boundary.SDYNegZNeg | Boundary.DXPosYPosZPos | Boundary.DXNegYPosZPos | Boundary.DXNegYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.AXPos | Boundary.AYPos | Boundary.AZNeg | Boundary.SDXPosYPos | Boundary.SDXPosYNeg | Boundary.SDXNegYPos | Boundary.SDXPosZPos | Boundary.SDXPosZNeg | Boundary.SDXNegZNeg | Boundary.SDYPosZPos | Boundary.SDYPosZNeg | Boundary.SDYNegZNeg | Boundary.DXPosYPosZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.AXPos | Boundary.AYNeg | Boundary.AZPos | Boundary.SDXPosYPos | Boundary.SDXPosYNeg | Boundary.SDXNegYNeg | Boundary.SDXPosZPos | Boundary.SDXPosZNeg | Boundary.SDXNegZPos | Boundary.SDYPosZPos | Boundary.SDYNegZPos | Boundary.SDYNegZNeg | Boundary.DXPosYPosZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXNegYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.AXNeg | Boundary.AYNeg | Boundary.AZPos | Boundary.SDXPosYNeg | Boundary.SDXNegYPos | Boundary.SDXNegYNeg | Boundary.SDXPosZPos | Boundary.SDXNegZPos | Boundary.SDXNegZNeg | Boundary.SDYPosZPos | Boundary.SDYNegZPos | Boundary.SDYNegZNeg | Boundary.DXPosYPosZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.AXPos | Boundary.AYNeg | Boundary.AZNeg | Boundary.SDXPosYPos | Boundary.SDXPosYNeg | Boundary.SDXNegYNeg | Boundary.SDXPosZPos | Boundary.SDXPosZNeg | Boundary.SDXNegZNeg | Boundary.SDYPosZNeg | Boundary.SDYNegZPos | Boundary.SDYNegZNeg | Boundary.DXPosYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXNegYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.AXNeg | Boundary.AYNeg | Boundary.AZNeg | Boundary.SDXPosYNeg | Boundary.SDXNegYPos | Boundary.SDXNegYNeg | Boundary.SDXPosZNeg | Boundary.SDXNegZPos | Boundary.SDXNegZNeg | Boundary.SDYPosZNeg | Boundary.SDYNegZPos | Boundary.SDYNegZNeg | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXNegYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.AXNeg | Boundary.AYPos | Boundary.AZPos | Boundary.SDXPosYPos | Boundary.SDXNegYPos | Boundary.SDXNegYNeg | Boundary.SDXPosZPos | Boundary.SDXNegZPos | Boundary.SDXNegZNeg | Boundary.SDYPosZPos | Boundary.SDYPosZNeg | Boundary.SDYNegZPos | Boundary.DXPosYPosZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXNegYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXNegYPosZNeg | Boundary.DXNegYNegZNeg,
                        Boundary.AXPos | Boundary.AYPos | Boundary.AZPos | Boundary.SDXPosYPos | Boundary.SDXPosYNeg | Boundary.SDXNegYPos | Boundary.SDXPosZPos | Boundary.SDXPosZNeg | Boundary.SDXNegZPos | Boundary.SDYPosZPos | Boundary.SDYPosZNeg | Boundary.SDYNegZPos | Boundary.DXPosYPosZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg
                    };

                    static Boundary[] InnerCorner = new Boundary[12] 
                    {
                        Boundary.DXNegYNegZNeg | Boundary.DXPosYPosZPos,
                        Boundary.DXNegYNegZPos | Boundary.DXPosYPosZNeg,
                        Boundary.DXNegYPosZPos | Boundary.DXPosYNegZNeg,
                        Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZPos,
                        Boundary.DXPosYPosZPos,
                        Boundary.DXNegYNegZNeg,
                        Boundary.DXNegYNegZPos,
                        Boundary.DXPosYPosZNeg,
                        Boundary.DXNegYPosZPos,
                        Boundary.DXPosYNegZNeg,
                        Boundary.DXPosYNegZPos,
                        Boundary.DXNegYPosZNeg
                    };

                    static Boundary[] InnerEdge = new Boundary[18] 
                    {
                        Boundary.SDXNegYNeg | Boundary.SDXPosYPos,
                        Boundary.SDXNegYPos | Boundary.SDXPosYNeg,
                        Boundary.SDXNegZNeg | Boundary.SDXPosZPos,
                        Boundary.SDXNegZPos | Boundary.SDXPosZNeg,
                        Boundary.SDYPosZPos | Boundary.SDYNegZNeg,
                        Boundary.SDYPosZNeg | Boundary.SDYNegZPos,
                        Boundary.SDXPosYPos,
                        Boundary.SDXNegYNeg,
                        Boundary.SDXPosYNeg,
                        Boundary.SDXNegYPos,
                        Boundary.SDXPosZPos,
                        Boundary.SDXNegZNeg,
                        Boundary.SDXNegZPos,
                        Boundary.SDXPosZNeg,
                        Boundary.SDYPosZPos,
                        Boundary.SDYNegZNeg,
                        Boundary.SDYNegZPos,
                        Boundary.SDYPosZNeg
                    };

                    static Boundary[] EdgeCombos = new Boundary[12]     
                    {
                        Boundary.DXPosYPosZNeg | Boundary.DXPosYPosZPos | Boundary.SDXPosYPos,
                        Boundary.DXNegYNegZNeg | Boundary.DXNegYNegZPos | Boundary.SDXNegYNeg,
                        Boundary.DXPosYNegZNeg | Boundary.DXPosYNegZPos | Boundary.SDXPosYNeg,
                        Boundary.DXNegYPosZNeg | Boundary.DXNegYPosZPos | Boundary.SDXNegYPos,

                        Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos | Boundary.SDXPosZPos,
                        Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg | Boundary.SDXNegZNeg,
                        Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos | Boundary.SDXNegZPos,
                        Boundary.DXPosYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.SDXPosZNeg,
                            
                        Boundary.DXNegYPosZPos | Boundary.DXPosYPosZPos | Boundary.SDYPosZPos,
                        Boundary.DXPosYNegZNeg | Boundary.DXNegYNegZNeg | Boundary.SDYNegZNeg,
                        Boundary.DXNegYPosZNeg | Boundary.DXPosYPosZNeg | Boundary.SDYPosZNeg,
                        Boundary.DXNegYNegZPos | Boundary.DXPosYNegZPos | Boundary.SDYNegZPos
                    };
                    #endregion
                }
            }
        }
    }
}