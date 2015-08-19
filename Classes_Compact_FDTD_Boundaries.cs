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

namespace Pachyderm_Acoustic
{
    namespace Numeric
    {
        namespace TimeDomain
        {
            public partial class Acoustic_Compact_FDTD : Simulation_Type
            {
                public abstract class Bound_Node : Node
                {
                    public Bound_Node(Point loc)
                    : base(loc)
                    { }

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
                    public static Boundary[] Face_Combos = new Boundary[10]
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

                    public static Boundary[] Wall = new Boundary[6]
                    {
                        Boundary.AZPos | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZPos | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZPos | Boundary.SDXNegZPos | Boundary.SDXPosZPos | Boundary.SDYNegZPos | Boundary.SDYPosZPos,
                        Boundary.AZNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYPosZNeg | Boundary.DXPosYNegZNeg | Boundary.DXPosYPosZNeg | Boundary.SDXNegZNeg | Boundary.SDXPosZNeg | Boundary.SDYNegZNeg | Boundary.SDYPosZNeg,
                        Boundary.AYPos | Boundary.DXNegYPosZNeg | Boundary.DXNegYPosZPos | Boundary.DXPosYPosZNeg | Boundary.DXPosYPosZPos | Boundary.SDXNegYPos | Boundary.SDXPosYPos | Boundary.SDYPosZNeg | Boundary.SDYPosZPos,
                        Boundary.AYNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYNegZPos | Boundary.DXPosYNegZNeg | Boundary.DXPosYNegZPos | Boundary.SDXNegYNeg | Boundary.SDXPosYNeg | Boundary.SDYNegZNeg | Boundary.SDYNegZPos,
                        Boundary.AXPos | Boundary.DXPosYNegZNeg | Boundary.DXPosYNegZPos | Boundary.DXPosYPosZNeg | Boundary.DXPosYPosZPos | Boundary.SDXPosYNeg | Boundary.SDXPosYPos | Boundary.SDXPosZNeg | Boundary.SDXPosZPos,
                        Boundary.AXNeg | Boundary.DXNegYNegZNeg | Boundary.DXNegYNegZPos | Boundary.DXNegYPosZNeg | Boundary.DXNegYPosZPos | Boundary.SDXNegYNeg | Boundary.SDXNegYPos | Boundary.SDXNegZNeg | Boundary.SDXNegZPos
                    };

                    public static Boundary[] Outer_Edges = new Boundary[12]
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

                    public static Boundary[] OuterCorner = new Boundary[8]
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

                    public static Boundary[] InnerCorner = new Boundary[12]
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

                    public static Boundary[] InnerEdge = new Boundary[18]
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

                    public static Boundary[] EdgeCombos = new Boundary[12]
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

                public class Bound_Node_IWB : Bound_Node
                {
                    double X, Y, Z;
                    Node Xpos_Link;
                    Node Xneg_Link;
                    Node Ypos_Link;
                    Node Yneg_Link;
                    Node Zpos_Link;
                    Node Zneg_Link;
                    Node[] Links2 = new Node[12];
                    Node[] Links3 = new Node[8];

                    //Kowalczyk Boundary Filter Node.
                    IIR_DIF[] filter;
                    List<double[]> acoef;
                    public List<Boundary> B_List;
                    Boundary Flags;
                    double ab1 = 0;
                    double abDenom = 0;
                    int[] id;

                    public Bound_Node_IWB(Point loc, double rho0, double dt, double dx, double C, int[] id_in, List<double[]> acoef_in, List<Boundary> B_in)
                        : base(loc)//, rho0, dt, dx, C, id_in)
                    {
                        id = id_in;
                        acoef = acoef_in;
                        B_List = B_in;
                        Flags = Boundary.None;
                        for (int i = 0; i < B_in.Count; i++) Flags |= B_in[i];
                    }

                    public override void UpdateP()
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
                        double P2 = 0;
                        foreach (P_Node P in this.Links2) P2 += P.P;
                        P2 *= 0;// 3.0 / 32.0;

                        double P3 = 0;
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

                    public override void Link_Nodes(ref Node[][][] Frame, int x, int y, int z)
                    {
                        double[] abs_poles = new double[] { -0.212854, -.212854 };
                        double[] abs_zeros = new double[] { 0.876, 0.876 };

                        double[] ref_poles = new double[] { 0, 0 };
                        double[] ref_zeros = new double[] { 0, 0 };

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
                                Link_Nodes(ref Frame, x, y, z);
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
                                Link_Nodes(ref Frame, x, y, z);
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
                        Link_Nodes(ref Frame, x, y, z);
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
                }

                public class Bound_Node_RDD : RDD_Node
                {
                    //IIR_DIF[] filter;
                    List<double> Rcoef;
                    double R = 0;
                    public List<Bound_Node.Boundary> B_List;
                    Bound_Node.Boundary Flags;
                    int[] id;

                    //Kowalczyk Boundary Filter Node.
                    IIR_DIF filter;
                    List<double[]> acoef;
                    double ab1 = 0;
                    double abDenom = 0;

                    public Bound_Node_RDD(Point loc, double rho0, double dt, double dx, double C, int[] id_in, List<double> Rcoef_in, List<Bound_Node.Boundary> B_in)
                    : base(loc)
                    {
                        id = id_in;
                        Rcoef = Rcoef_in;
                        B_List = B_in;
                        Flags = Bound_Node.Boundary.None;
                        for (int i = 0; i < B_in.Count; i++) Flags |= B_in[i];
                        for (int i = 0; i < Rcoef.Count; i++) R += Rcoef[i];
                        R /= Rcoef.Count;

                        //filter = new DIF_IWB_2p(abs_zeros, abs_poles, 0.81, IIR_DIF.IWB_Mask.Axial);

                        ab1 += filter.a_b;
                        this.abDenom = 1 / (ab1 + 1);
                        ab1 -= 1;
                    }

                    public override void UpdateP()
                    {
                        double p2 = 0;
                        foreach (Node node in Links2) p2 += node.P;
                        Pnf = p2 * 0.25 - Pn - Pn_1 * ab1 + filter.g_b_term();

                        Pnf *= abDenom;

                        filter.Update(Pnf, Pn_1);
                    }

                    public override void UpdateT()
                    {
                        Pn_1 = Pn;
                        Pn = Pnf * Attenuation;

                        base.UpdateT();
                    }

                    public void Complete_Boundary()
                    {
                        /*
                        [0] = x+y+z+
                        [1] = x+y-z-
                        [2] = x+y-z+
                        [3] = x+y+z-
                        */
                        foreach (Bound_Node.Boundary b in B_List)
                        {
                            if (b == Bound_Node.Boundary.DXPosYPosZPos)
                            {
                                if (Links2[0] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[0].Pt));
                                //(Links2[0] as RDD_Node).Links2[5] = new Null_Node();
                                //Links2[0] = new Null_Node();
                                (Links2[0] as RDD_Node).Links2[5] = (Links2[0] as RDD_Node).Links2[0];
                                Links2[0] = Links2[5];
                            }
                            else if (b == Bound_Node.Boundary.DXPosYNegZNeg)
                            {
                                if (Links2[1] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[1].Pt));
                                //(Links2[1] as RDD_Node).Links2[4] = new Null_Node();
                                //Links2[1] = new Null_Node();
                                (Links2[1] as RDD_Node).Links2[4] = (Links2[1] as RDD_Node).Links2[1];
                                Links2[1] = Links2[4];
                            }
                            else if (b == Bound_Node.Boundary.DXPosYNegZPos)
                            {
                                if (Links2[2] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[2].Pt));
                                //(Links2[2] as RDD_Node).Links2[7] = new Null_Node();
                                //Links2[2] = new Null_Node();
                                (Links2[2] as RDD_Node).Links2[7] = (Links2[2] as RDD_Node).Links2[2];
                                Links2[2] = Links2[7];
                            }
                            else if (b == Bound_Node.Boundary.DXPosYPosZNeg)
                            {
                                if (Links2[3] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[3].Pt));
                                //(Links2[3] as RDD_Node).Links2[6] = new Null_Node();
                                //Links2[3] = new Null_Node();
                                (Links2[3] as RDD_Node).Links2[6] = (Links2[3] as RDD_Node).Links2[3];
                                Links2[3] = Links2[6];
                            }
                            /*
                            [4] = x-y+z+
                            [5] = x-y-z-
                            [6] = x-y-z+
                            [7] = x-y+z-
                            */
                            else if (b == Bound_Node.Boundary.DXNegYPosZPos)
                            {
                                if (Links2[4] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[4].Pt));
                                //(Links2[4] as RDD_Node).Links2[1] = new Null_Node();
                                //Links2[4] = new Null_Node();
                                (Links2[4] as RDD_Node).Links2[1] = (Links2[4] as RDD_Node).Links2[4];
                                Links2[4] = Links2[1];
                            }
                            else if (b == Bound_Node.Boundary.DXNegYNegZNeg)
                            {
                                if (Links2[5] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[5].Pt));
                                //(Links2[5] as RDD_Node).Links2[0] = new Null_Node();
                                //Links2[5] = new Null_Node();
                                (Links2[5] as RDD_Node).Links2[0] = (Links2[5] as RDD_Node).Links2[5];
                                Links2[5] = Links2[0];
                            }
                            else if (b == Bound_Node.Boundary.DXNegYNegZPos)
                            {
                                if (Links2[6] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[6].Pt));
                                //(Links2[6] as RDD_Node).Links2[3] = new Null_Node();
                                //Links2[6] = new Null_Node();
                                (Links2[6] as RDD_Node).Links2[3] = (Links2[6] as RDD_Node).Links2[6];
                                Links2[6] = Links2[3];
                            }
                            else if (b == Bound_Node.Boundary.DXNegYPosZNeg)
                            {
                                if (Links2[7] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[7].Pt));
                                //(Links2[7] as RDD_Node).Links2[2] = new Null_Node();
                                //Links2[7] = new Null_Node();
                                (Links2[7] as RDD_Node).Links2[2] = (Links2[7] as RDD_Node).Links2[7];
                                Links2[7] = Links2[2];
                            }
                            /*
                            [8] = y+
                            [9] = y-    
                            [10] = z+
                            [11] = z-
                            */
                            else if (b == Bound_Node.Boundary.AYPos)
                            {
                                if (Links2[8] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[8].Pt));
                                //(Links2[8] as RDD_Node).Links2[9] = new Null_Node();
                                //Links2[8] = new Null_Node();
                                (Links2[8] as RDD_Node).Links2[9] = (Links2[8] as RDD_Node).Links2[8];
                                Links2[8] = Links2[9];
                            }
                            else if (b == Bound_Node.Boundary.AYNeg)
                            {
                                if (Links2[9] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[9].Pt));
                                //(Links2[9] as RDD_Node).Links2[8] = new Null_Node();
                                //Links2[9] = new Null_Node();
                                (Links2[9] as RDD_Node).Links2[8] = (Links2[9] as RDD_Node).Links2[9];
                                Links2[9] = Links2[8];
                            }
                            else if (b == Bound_Node.Boundary.AZPos)
                            {
                                if (Links2[10] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[10].Pt));
                                //(Links2[10] as RDD_Node).Links2[11] = new Null_Node();
                                //Links2[10] = new Null_Node();
                                (Links2[10] as RDD_Node).Links2[11] = (Links2[10] as RDD_Node).Links2[10];
                                Links2[10] = Links2[11];
                            }
                            else if (b == Bound_Node.Boundary.AZNeg)
                            {
                                if (Links2[11] is Null_Node) continue;
                                //Rhino.RhinoDoc.ActiveDoc.Objects.AddLine(Utilities.PachTools.HPttoRPt(Pt), Utilities.PachTools.HPttoRPt(Links2[11].Pt));
                                //(Links2[11] as RDD_Node).Links2[10] = new Null_Node();
                                //Links2[11] = new Null_Node();
                                (Links2[11] as RDD_Node).Links2[10] = (Links2[11] as RDD_Node).Links2[11];
                                Links2[11] = Links2[10];
                            }
                        }
                        /*
                        [0] = x+y+z+
                        [1] = x+y-z-
                        [2] = x+y-z+
                        [3] = x+y+z-
                        [4] = x-y+z+
                        [5] = x-y-z-
                        [6] = x-y-z+
                        [7] = x-y+z-
                        [8] = y+
                        [9] = y-
                        [10] = z+
                        [11] = z-
                        */
                    }

                    //public override void UpdateP()
                    //{
                    //    base.UpdateP();
                    //    Pnf *= R;
                    //}

                    //public override void Link_Nodes(ref Node[][][] Frame, int x, int y, int z)
                    //{
                    //    double[] abs_poles = new double[] { -0.212854, -.212854 };
                    //    double[] abs_zeros = new double[] { 0.876, 0.876 };

                    //    double[] ref_poles = new double[] { 0, 0 };
                    //    double[] ref_zeros = new double[] { 0, 0 };

                    //    Bound_Node.Boundary BFlags = Flags;

                    //    int mod = x % 2;

                    //    #region General Node Setup Algorithm

                    //    //Set the filters
                    //    //Edges
                    //    //todo - find the problem in this block of code. Links2 has a leak.
                    //    if (x == Frame.Length - 1 || y == 0 || z == Frame[mod][y].Length - 1 - mod || (Bound_Node.Boundary.SDXPosYPos & Flags) == Bound_Node.Boundary.SDXPosYPos)
                    //    {
                    //        //Links2[0] = Links2[1];
                    //        Links2[0] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[0] = Frame[x + 1 - mod][y + 1 - mod][z];
                    //    }
                    //    if (x == 0 || y == 0 || (Bound_Node.Boundary.SDXNegYNeg & Flags) == Bound_Node.Boundary.SDXNegYNeg)
                    //    {
                    //        //Links2[1] = Links2[0];
                    //        Links2[1] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[1] = Frame[x - mod][y - mod][z];
                    //    }
                    //    if (x == Frame.Length - 1 || y == 0 || z == Frame[mod][y].Length - 1 + mod || (Bound_Node.Boundary.SDXPosYNeg & Flags) == Bound_Node.Boundary.SDXPosYNeg)
                    //    {
                    //        //Links2[2] = Links2[3];
                    //        Links2[2] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[2] = Frame[x + 1 - mod][y - mod][z];
                    //    }
                    //    if (x == 0 || y == Frame[mod].Length - 1 || z == Frame[mod][y].Length - 1 + mod ||(Bound_Node.Boundary.SDXNegYPos & Flags) == Bound_Node.Boundary.SDXNegYPos)
                    //    {
                    //        //Links2[3] = Links2[2];
                    //        Links2[3] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[3] = Frame[x - mod][y + 1 - mod][z];
                    //    }
                    //    if (x == Frame.Length - 1 || z == Frame[mod][y].Length - 1 || z == Frame[mod][y].Length - 1 + mod || (Bound_Node.Boundary.SDXPosZPos & Flags) == Boundary.SDXPosZPos)
                    //    {
                    //        //Links2[4] = Links2[5];
                    //        Links2[4] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[4] = Frame[x + 1 - mod][y][z + 1 - mod];
                    //    }
                    //    if (x == 0 || z == 0 || (Bound_Node.Boundary.SDXNegZNeg & Flags) == Bound_Node.Boundary.SDXNegZNeg)
                    //    {
                    //        //Links2[5] = Links2[4];
                    //        Links2[5] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[5] = Frame[x - mod][y][z - mod];
                    //    }
                    //    if (x == 0 || z == Frame[mod][y].Length - 1 || (Bound_Node.Boundary.SDXNegZPos & Flags) == Bound_Node.Boundary.SDXNegZPos)
                    //    {
                    //        //Links2[6] = Links2[7];
                    //        Links2[6] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[6] = Frame[x - mod][y][z + 1 - mod];
                    //    }
                    //    if (x == Frame.Length - 1 || z == 0 || (Bound_Node.Boundary.SDXPosZNeg & Flags) == Bound_Node.Boundary.SDXPosZNeg)
                    //    {
                    //        //Links2[7] = Links2[6];
                    //        Links2[7] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[7] = Frame[x + 1 - mod][y][z - mod];
                    //    }
                    //    if (y == Frame[mod].Length - 1 || z == Frame[mod][y].Length - 1 || (Bound_Node.Boundary.SDYPosZPos & Flags) == Bound_Node.Boundary.SDYPosZPos)
                    //    {
                    //        //Links2[8] = Links2[9];
                    //        Links2[8] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[8] = Frame[x][y + 1 - mod][z + 1 - mod];
                    //    }
                    //    if (y == 0 || z == 0 || (Bound_Node.Boundary.SDYNegZNeg & Flags) == Bound_Node.Boundary.SDYNegZNeg)
                    //    {
                    //        //Links2[9] = Links2[8];
                    //        Links2[9] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[9] = Frame[x][y - mod][z - mod];
                    //    }
                    //    if (y == 0 || z == Frame[mod][y].Length - 1 || (Bound_Node.Boundary.SDYNegZPos & Flags) == Bound_Node.Boundary.SDYNegZPos)
                    //    {
                    //        //Links2[10] = Links2[11];
                    //        Links2[10] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[10] = Frame[x][y - mod][z + 1 - mod];
                    //    }
                    //    if (y == Frame[mod].Length - 1 || z == 0 || (Bound_Node.Boundary.SDYPosZNeg & Flags) == Bound_Node.Boundary.SDYPosZNeg)
                    //    {
                    //        //Links2[11] = Links2[10];
                    //        Links2[11] = new Null_Node();
                    //    }
                    //    else
                    //    {
                    //        Links2[11] = Frame[x][y + 1 - mod][z - mod];
                    //    }
                    //    #endregion
                    //}
                }

                public class DIF_IWB_2p : IIR_DIF
                {
                    double[] ax;
                    double[] bx;
                    double[] xx;
                    double[] yx;
                    double rec_b0;
                    double gn;
                    double interp;

                    public DIF_IWB_2p(double[] Poles, double[] Zeros, double scale, double M)
                        : this(Poles, Zeros, scale)
                    {
                        interp = M;
                    }

                    public DIF_IWB_2p(double[] Poles, double[] Zeros, double scale, IWB_Mask M)
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

                    public DIF_IWB_2p(double[] Poles, double[] Zeros, double scale)
                    {
                        ax = new double[3];
                        ax[0] = 1;
                        ax[1] = -(Poles[0] + Poles[1]);
                        ax[2] = Poles[0] * Poles[1];

                        bx = new double[3];
                        bx[0] = 1 / scale;
                        bx[1] = -(Zeros[0] + Zeros[1]) / scale;
                        bx[2] = Zeros[0] * Zeros[1] / scale;

                        if (ax.Length != bx.Length) throw new Exception("Number of Poles and Number of Zeros must be equal.");
                        rec_b0 = 1 / bx[0];
                        a_b = ax[0] / bx[0];
                        xx = new double[2];
                        yx = new double[2];
                    }

                    public override double g_b_term()
                    {
                        gn = bx[1] * xx[0] - ax[1] * yx[0] + bx[2] * xx[1] - ax[2] * yx[1];
                        return gn * rec_b0 * interp;
                    }

                    /// <summary>
                    /// Do before updating boundary node pressure values, but after getting the g-term.
                    /// </summary>
                    /// <param name="Pn"></param>
                    public override void Update(double Pnf, double Pn_1)
                    {
                        xx[1] = xx[0];
                        yx[1] = yx[0];
                        xx[0] = a_b * (Pn_1 - Pnf) - gn / bx[0];
                        yx[0] = (bx[0] * xx[0] + gn) / ax[0];
                    }

                    public double A0
                    {
                        get { return ax[0]; }
                    }

                    public double B0
                    {
                        get { return bx[0]; }
                    }
                }

                public abstract class IIR_DIF
                {
                    public double a_b;
                    protected int[] DIR;

                    public enum IWB_Mask
                    {
                        Axial,
                        SideDiagonal,
                        Diagonal
                    }

                    //public Complex a_b_sub_1 { get { return ab1; } }
                    //public Complex a_b_recip { get { return abdenom; } }
                    public abstract double g_b_term();
                    //public Complex ab;
                    public abstract void Update(double Pnf, double Pn_1);
                }
            }
        }
    }
}