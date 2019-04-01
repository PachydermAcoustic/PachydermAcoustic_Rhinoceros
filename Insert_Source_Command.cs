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
using System;
using System.Drawing;
using System.Collections.Generic;
using Rhino;
using Rhino.Display;
using Rhino.Commands;
using Rhino.Geometry;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        ///</summary>
        [System.Runtime.InteropServices.Guid("9834c9e9-5f74-4dd2-9631-9cf23d0d9832")]
        public class Pach_Source_Object : Command
        {
            ///<summary> 
            /// Rhino tracks commands by their unique ID. Every command must have a unique id. 
            /// The Guid created by the project wizard is unique. You can create more Guids using 
            /// the "Create Guid" tool in the Tools menu. 
            ///</summary> 
            ///<returns>The id for this command</returns> 
            public Pach_Source_Object()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_Source_Object Instance
            {
                get;
                private set;
            }

            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Insert_Source";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
            {
                SourceConduit m_source_conduit = SourceConduit.Instance;

                Rhino.Geometry.Point3d location;
                if(Rhino.Input.RhinoGet.GetPoint("Select Source Position", false, out location) != Result.Success) return Result.Cancel;

                Rhino.DocObjects.RhinoObject rhObj = doc.Objects.Find(doc.Objects.AddPoint(location));
                rhObj.Attributes.Name = "Acoustical Source";
                rhObj.Geometry.SetUserString("SourceType", "0");
                rhObj.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(new double[] { 120, 120, 120, 120, 120, 120, 120, 120 }));
                rhObj.Geometry.SetUserString("Phase", "0;0;0;0;0;0;0;0");
                doc.Objects.ModifyAttributes(rhObj, rhObj.Attributes, true);

                m_source_conduit.SetSource(rhObj);
                doc.Views.Redraw();

                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("2AE5A2A3-91EF-4D87-B2FC-5FB21FA2DE04")]
        public class Pach_SurfaceSource_Object : Command
        {
            ///<summary> 
            /// Rhino tracks commands by their unique ID. Every command must have a unique id. 
            /// The Guid created by the project wizard is unique. You can create more Guids using 
            /// the "Create Guid" tool in the Tools menu. 
            ///</summary> 
            ///<returns>The id for this command</returns> 
            public Pach_SurfaceSource_Object()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_SurfaceSource_Object Instance
            {
                get;
                private set;
            }

            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Insert_SurfaceSource";
                }
            }

            //Sound Pressure Level at 1 m. of several kinds of people.
            //Average Speech Levels and Spectra in Various Speaking/Listening Conditions: A Summary of the Pearson, Bennett, & Fidell (1977) Report
            //Wayne O. Olsen, Mayo Clinic, Rochester, MN
            public double[][] Females = new double[][] { new double[] { 20, 36, 48, 49, 42, 39, 35, 35 }, new double[] { 20, 37, 51, 53, 49, 44, 42, 37 }, new double[] { 20, 35, 56, 59, 57, 53, 48, 43 }, new double[] { 20, 34, 58, 64, 66, 63, 56, 51 }, new double[] { 20, 30, 56, 69, 76, 75, 69, 58 } };
            public double[][] Males = new double[][] { new double[] { 20, 45, 49, 50, 42, 41, 38, 35 }, new double[] { 20, 51, 56, 57, 50, 47, 43, 36 }, new double[] { 20, 53, 59, 64, 58, 54, 49, 43 }, new double[] { 20, 56, 64, 72, 70, 66, 60, 50 }, new double[] { 20, 45, 70, 80, 84, 80, 72, 63 } };
            public double[][] Children = new double[][] { new double[] { 20, 27, 48, 52, 44, 41, 38, 38 }, new double[] { 20, 30, 53, 56, 50, 45, 43, 42 }, new double[] { 20, 31, 56, 60, 60, 55, 51, 46 }, new double[] { 20, 30, 56, 63, 66, 65, 57, 51 }, new double[] { 20, 45, 55, 69, 75, 72, 70, 58 } };

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
            {
                SourceConduit m_source_conduit = SourceConduit.Instance;

                //Rhino.DocObjects.ObjRef location;

                string type = "custom";

                Rhino.Input.Custom.GetObject GO = new Rhino.Input.Custom.GetObject();
                GO.SetCommandPrompt("Select source surface...");
                GO.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
                GO.AddOption("Crowd");
                GO.GroupSelect = false;
                GO.SubObjectSelect = false;
                GO.EnableClearObjectsOnEntry(false);
                GO.EnableUnselectObjectsOnExit(false);
                GO.DeselectAllBeforePostSelect = false;

                int Men = 100, Women = 100, Kids = 100, effort = 0;

                do
                {
                    Rhino.Input.GetResult GR = GO.Get();

                    if (GR == Rhino.Input.GetResult.Option)
                    {
                        type = GO.Option().EnglishName;
                        if (type == "Crowd")
                        {
                            Rhino.Input.RhinoGet.GetInteger("Number of Men in Crowd...", true, ref Men);
                            Rhino.Input.RhinoGet.GetInteger("Number of Women in Crowd...", true, ref Women);
                            Rhino.Input.RhinoGet.GetInteger("Number of Children in Crowd...", true, ref Kids);

                            Rhino.Input.Custom.GetOption GOpt = new Rhino.Input.Custom.GetOption();
                            GOpt.SetCommandPrompt("Speech Activity");
                            GOpt.AddOption("SoftOrWhispered");
                            GOpt.AddOption("Conversation");
                            GOpt.AddOption("CompetingConversation");
                            GOpt.AddOption("Singing");
                            GOpt.AddOption("AllShouting");
                            GOpt.AcceptNothing(false);

                            GOpt.Get();
                            effort = GOpt.OptionIndex() - 1;
                        }
                        continue;
                    }
                    else if (GR == Rhino.Input.GetResult.Object)
                    {
                        for (int i = 0; i < GO.ObjectCount; i++)
                        {
                            Rhino.DocObjects.ObjRef obj = GO.Object(i);

                            Rhino.DocObjects.RhinoObject rhObj = doc.Objects.Find(obj.ObjectId);

                            double Area = (rhObj.Geometry as Rhino.Geometry.Brep).GetArea();

                            rhObj.Attributes.Name = "Acoustical Source";
                            rhObj.Geometry.SetUserString("SourceType", "");

                            double[] SPL = new double[] { 120, 120, 120, 120, 120, 120, 120, 120 };

                            if (type == "Crowd")
                            {
                                //double mod = (effort < 3) ? .5 : 1;

                                //Correct for tightly packed spaces, and competing speech.
                                //if (Area / (Men + Women + Kids) < 3.0f)
                                //{
                                //    //In overcrowded scenarios, vocal effort escalates as such:
                                //    //whispering becomes conversation.
                                //    //conversation becomes competing conversation.
                                //    //competing conversation becomes shouting.
                                //    //Singing stays singing... I'm pretty sure experienced singers wouldn't start shouting...
                                //    if (effort < 2) effort++;
                                //    else effort = 4;
                                //}

                                for (int oct = 0; oct < 8; oct++)
                                {
                                    double Power = Men * Math.Pow(10, Males[effort][oct] / 10) + Women * Math.Pow(10, Females[effort][oct] / 10) + Kids * Math.Pow(10, Children[effort][oct] / 10);
                                    //Power /= (Area);
                                    SPL[oct] = 10 * Math.Log10(Power) + 11;
                                }
                            }
                            else
                            {
                                Rhino.Input.RhinoGet.GetNumber("62.5 Hz. Sound Power Level", true, ref SPL[0]);
                                Rhino.Input.RhinoGet.GetNumber("125 Hz. Sound Power Level", true, ref SPL[1]);
                                Rhino.Input.RhinoGet.GetNumber("250 Hz. Sound Power Level", true, ref SPL[2]);
                                Rhino.Input.RhinoGet.GetNumber("500 Hz. Sound Power Level", true, ref SPL[3]);
                                Rhino.Input.RhinoGet.GetNumber("1000 Hz. Sound Power Level", true, ref SPL[4]);
                                Rhino.Input.RhinoGet.GetNumber("2000 Hz. Sound Power Level", true, ref SPL[5]);
                                Rhino.Input.RhinoGet.GetNumber("4000 Hz. Sound Power Level", true, ref SPL[6]);
                                Rhino.Input.RhinoGet.GetNumber("8000 Hz. Sound Power Level", true, ref SPL[7]);
                            }

                            rhObj.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(SPL));
                            rhObj.Geometry.SetUserString("Phase", "0;0;0;0;0;0;0;0");
                            doc.Objects.ModifyAttributes(rhObj, rhObj.Attributes, true);

                            m_source_conduit.SetSource(rhObj);
                        }

                        doc.Views.Redraw();
                        return Result.Success;
                    }
                    else return Result.Cancel;
                } while (true);
            }
        }

        [System.Runtime.InteropServices.Guid("3F0882FB-2DD9-46BA-9A8D-079EFEFF2F54")]
        public class Pach_HumanSource_Object : Command
        {
            ///<summary> 
            /// Rhino tracks commands by their unique ID. Every command must have a unique id. 
            /// The Guid created by the project wizard is unique. You can create more Guids using 
            /// the "Create Guid" tool in the Tools menu. 
            ///</summary> 
            ///<returns>The id for this command</returns> 
            public Pach_HumanSource_Object()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_HumanSource_Object Instance
            {
                get;
                private set;
            }

            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Insert_SpeakingPerson";
                }
            }

            //Sound Pressure Level at 1 m. of several kinds of people.
            public double[][] Females = new double[][] { new double[] { 20, 36, 48, 49, 42, 39, 35, 35 }, new double[] { 20, 37, 51, 53, 49, 44, 42, 37 }, new double[] { 20, 35, 56, 59, 57, 53, 48, 43 }, new double[] { 20, 34, 58, 64, 66, 63, 56, 51 }, new double[] { 20, 30, 56, 69, 76, 75, 69, 58 } };
            public double[][] Males = new double[][] { new double[] { 20, 45, 49, 50, 42, 41, 38, 35 }, new double[] { 20, 51, 56, 57, 50, 47, 43, 36 }, new double[] { 20, 53, 59, 64, 58, 54, 49, 43 }, new double[] { 20, 56, 64, 72, 70, 66, 60, 50 }, new double[] { 20, 45, 70, 80, 84, 80, 72, 63 } };
            public double[][] Children = new double[][] { new double[] { 20, 27, 48, 52, 44, 41, 38, 38 }, new double[] { 20, 30, 53, 56, 50, 45, 43, 42 }, new double[] { 20, 31, 56, 60, 60, 55, 51, 46 }, new double[] { 20, 30, 56, 63, 66, 65, 57, 51 }, new double[] { 20, 45, 55, 69, 75, 72, 70, 58 } };

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
            {
                SourceConduit m_source_conduit = SourceConduit.Instance;

                Rhino.Geometry.Point3d Pt;

                Rhino.Input.RhinoGet.GetPoint("Select Location of speaker's mouth...", false, out Pt);

                Rhino.Input.Custom.GetOption GOpt_type = new Rhino.Input.Custom.GetOption();
                Rhino.Input.Custom.GetOption GOpt_act = new Rhino.Input.Custom.GetOption();

                GOpt_type.SetCommandPrompt("Pick what kind of speaker this is...");
                GOpt_type.AddOption("Man");
                GOpt_type.AddOption("Woman");
                GOpt_type.AddOption("Child");
                GOpt_type.AcceptNothing(false);
                GOpt_type.Get();

                Rhino.Input.Custom.GetOption GOpt = new Rhino.Input.Custom.GetOption();
                GOpt_act.SetCommandPrompt("Pick the best descriptor of vocal effort...");
                GOpt_act.AddOption("SoftOrWhispered");
                GOpt_act.AddOption("Conversation");
                GOpt_act.AddOption("CompetingConversation");
                GOpt_act.AddOption("Singing");
                GOpt_act.AddOption("Shouting");
                GOpt_act.AcceptNothing(false);
                GOpt_act.Get();

                double[] SPL = new double[0];

                switch (GOpt_type.OptionIndex())
                {
                    case 0:
                        SPL = Males[GOpt_act.OptionIndex()-1];
                        break;
                    case 1:
                        SPL = Females[GOpt_act.OptionIndex()-1];
                        break;
                    case 2:
                        SPL = Children[GOpt_act.OptionIndex()-1];
                        break;
                }

                Rhino.DocObjects.RhinoObject rhObj = doc.Objects.Find(doc.Objects.AddPoint(Pt));
                rhObj.Attributes.Name = "Acoustical Source";
                rhObj.Geometry.SetUserString("SourceType", "0");
                for (int i = 0; i < 8; i++) SPL[i] += 11;
                rhObj.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(SPL));
                rhObj.Geometry.SetUserString("Phase", "0;0;0;0;0;0;0;0");
                doc.Objects.ModifyAttributes(rhObj, rhObj.Attributes, true);

                m_source_conduit.SetSource(rhObj);
                doc.Views.Redraw();

                Rhino.RhinoApp.WriteLine("Apologies, but at this time, human sources are treated as omnidirectional, which is great if you are trying to ");
                Rhino.RhinoApp.WriteLine("characterize speech, but not so good if you are trying to simulate an exact individual in the act of speaking...");
                Rhino.RhinoApp.WriteLine("If you happen to know of a good database with human speech directivity, or would otherwise like to contribute, please let me know. --Arthur");

                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("7E8161BC-118B-47D2-915A-06A97599266B")]
        public class Pach_LineSource_Object : Command
        {
            ///<summary> 
            /// Rhino tracks commands by their unique ID. Every command must have a unique id. 
            /// The Guid created by the project wizard is unique. You can create more Guids using 
            /// the "Create Guid" tool in the Tools menu. 
            ///</summary> 
            ///<returns>The id for this command</returns> 
            public Pach_LineSource_Object()
            {
                // Rhino only creates one instance of each command class defined in a
                // plug-in, so it is safe to store a refence in a static property.
                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static Pach_LineSource_Object Instance
            {
                get;
                private set;
            }

            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Insert_LineSource";
                }
            }

            public double[] WelshTraffic = new double[] { -32, -16, -4, -3,-10, -13, -17, -17 };
            /// FHWA traffic coefficients. [Vehicle Type i][Pavement Type p][Full Throttle][A	B	C	D1	D2	E1	E2	F1	F2	G1	G2	H1	H2	I1	I2	J1	J2]
            public double[][][][] FHWATraffic = new double[][][][] {
                new double[][][]{new double[][]{new double[]{41.740807,1.148546, 67,-7516.580054,-9.7623,16460.1,11.65932,-14823.9,-1.233347,7009.474786,-4.327918,-1835.189815,2.579086,252.418543,-0.573822,-14.268316,0.045682},
                new double[]{41.740807,1.148546,50.128316,-7516.580054,-9.7623,16460.1,11.65932,-14823.9,-1.23334,7009.474786,-4.327918,-1835.189815,2.579086,252.418543,-0.573822,-14.268316,0.045682}},
                new double[][]{new double[]{41.740807,0.494698,67,-7313.985627,-19.697019,16009.5,34.363901,-14414.4,-22.462943,6814.317463,6.093141,-1783.723974,-0.252834,245.299562,-0.170266,-13.86487,0.022131},
                new double[]{41.740807,0.494698,50.128316,-7313.985627,-19.697019,16009.5,34.363901,-14414.4,-22.462943,6814.317463,6.093141,-1783.723974,-0.252834,245.299562,-0.170266,-13.86487,0.022131}},
                new double[][]{new double[]{41.740807,-1.065026,67,-9549.987851,-146.173482,21064,340.622686,-19060.8,-324.802942,9032.990872,161.886578,-2363.810485,-44.454426,324.077238,6.378783,-18.21167,-0.373971},
                new double[]{41.740807,-1.065026,50.128316,-9549.987851,-146.173482,21064,340.622686,-19060.8,-324.802942,9032.990872,161.886578,-2363.810485,-44.454426,324.077238,6.378783,-18.21167,-0.373971}},
                new double[][]{new double[]{41.740807,3.520004,67,-2027.8376,-70.674562,3728.329033,155.109567,-2768.001364,-138.780925,1030.541403,64.525774,-195.32456,-16.430316,16.418899,2.17435,-0.339616,-0.117021},
                new double[]{41.740807,3.520004,50.128316,-2027.8376,-70.674562,3728.329033,155.109567,-2768.001364,-138.780925,1030.541403,64.525774,-195.32456,-16.430316,16.418899,2.17435,-0.339616,-0.117021}}},
                new double[][][]{new double[][]{new double[]{33.918713,20.591046,74,-8997.974274,96.301703,19015.4,-196.241744,-16587,162.56952,7627.874332,-70.394575,-1950.412341,16.876826,263.093464,-2.132793,-14.645109,0.111404},
                new double[]{33.918713,20.591046,68.002978,-1238.353632,-68.218944,2532.436947,151.781493,-2124.165806,-140.388413,919.784302,68.545463,-215.745405,-18.551234,25.909788,2.634001,-1.244253,-0.153272}},
                new double[][]{new double[]{33.918713,19.903775,74,-8997.974274,96.301703,19015.4,-196.241744,-16587,162.56952,7627.874332,-70.394575,-1950.412341,16.876826,263.093464,-2.132793,-14.645109,0.111404},
                new double[]{33.918713,19.903775,68.002978,-230.440015,-82.783198,172.725033,186.80143,131.655819,-174.718246,-207.664798,86.12481,95.139145,-23.513441,-18.96669,3.366475,1.407549,-0.197472}},
                new double[][]{new double[]{33.918713,19.345214,74,-8997.974274,96.301703,19015.4,-196.241744,-16587,162.56952,7627.874332,-70.394575,-1950.412341,16.876826,263.093464,-2.132793,-14.645109,0.111404},
                new double[]{33.918713,19.345214,68.002978,-234.711357,-103.147894,162.036132,244.033651,133.970948,-237.867685,-196.613672,121.527971,87.517298,-34.222359,-17.12562,5.031804,1.253128,-0.301914}},
                new double[][]{new double[]{33.918713,22.141611,74,-8997.974274,96.301703,19015.4,-196.241744,-16587,162.56952,7627.874332,-70.394575,-1950.412341,16.876826,263.093464,-2.132793,-14.645109,0.111404},
                new double[]{33.918713,22.141611,68.002978,-139.27717,-132.207111,97.357937,296.574807,65.350117,-273.981431,-104.555273,132.85439,47.637332,-35.600554,-9.424641,4.997542,0.689877,-0.287335}}},
                new double[][][]{new double[][]{new double[]{35.87985,21.019665,80,-6864.586846,-94.379848,14368.7,226.701375,-12459.2,-220.015419,5710.525999,110.518825,-1458.340416,-30.365892,196.811136,4.33716,-10.977676,-0.252197},
                new double[]{35.87985,21.019665,74.298135,1468.440649,-235.319117,-3852.393214,537.981518,3886.430673,-502.160068,-1986.858782,244.714955,549.002247,-65.686556,-78.239429,9.217734,4.509121,-0.529106}},
                new double[][]{new double[]{35.87985,20.358498,80,-6864.586846,-94.379848,14368.7,226.701375,-12459.2,-220.015419,5710.525999,110.518825,-1458.340416,-30.365892,196.811136,4.337165,-10.977676,-0.252197},
                new double[]{35.87985,20.358498,74.298135,-290.277032,-196.828915,156.854882,450.144699,151.082001,-420.250062,-168.033708,204.806845,60.772941,-54.968455,-9.681901,7.711617,0.570105,-0.442469}},
                new double[][]{new double[]{35.87985,19.107151,80,-6864.586846,-94.379848,14368.7,226.701375,-12459.2,-220.015419,5710.525999,110.518825,-1458.340416,-30.365892,196.811136,4.337165,-10.977676,-0.252197},
                new double[]{35.87985,19.107151,74.298135,-258.941348,-255.205946,135.514216,587.489921,132.973712,-552.824216,-151.366531,272.102657,57.66924,-73.912732,-9.928293,10.514055,0.649271,-0.612569}},
                new double[][]{new double[]{35.87985,21.822818,80,-6864.586846,-94.379848,14368.7,226.701375,-12459.2,-220.015419,5710.525999,110.518825,-1458.340416,-30.365892,196.811136,4.337165,-10.977676,-0.252197},
                new double[]{35.87985,21.822818,74.298135,87.378338,-224.132311,-497.410428,509.705253,579.584033,-473.326603,-298.5689955,229.5809,78.021585,-61.374037,-10.058424,8.58403,0.498685,-0.49149}}},
                new double[][][]{new double[][]{new double[]{23.47953,38.006238,74,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557},
                new double[]{23.47953,38.006238,68.002978,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557}},
                new double[][]{new double[]{23.47953,37.318967,74,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557},
                new double[]{23.47953,37.318967,68.002978,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557}},
                new double[][]{new double[]{23.47953,36.760406,74,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557},
                new double[]{23.47953,36.760406,68.002978,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557}},
                new double[][]{new double[]{23.47953,39.556803,74,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557},
                new double[]{23.47953,39.556803,68.002978,4621.365424,-123.140566,-11601.5,284.796174,11535.3,-267.623062,-5896.461017,130.822488,1645.797051,-35.139019,-238.929963,4.927783,14.139828,-0.282557}}},
                new double[][][]{new double[][]{new double[]{41.022542,10.013879,67,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005},
                new double[]{41.022542,10.013879,56,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005}},
                new double[][]{new double[]{41.022542,10.013879,67,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005},
                new double[]{41.022542,10.013879,56,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005}},
                new double[][]{new double[]{41.022542,10.013879,67,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005},
                new double[]{41.022542,10.013879,56,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005}},
                new double[][]{new double[]{41.022542,10.013879,67,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005},
                new double[]{41.022542,10.013879,56,7546.65902,-8.870177,-17396,7.899209,16181.8,2.526152,-7828.632535,-5.314462,2085.468458,2.344913,-290.816544,-0.435913,16.614043,0.03005}}}};

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(RhinoDoc doc, Rhino.Commands.RunMode mode)
            {
                SourceConduit m_source_conduit = SourceConduit.Instance;

                Rhino.Input.Custom.GetObject GO = new Rhino.Input.Custom.GetObject();
                GO.SetCommandPrompt("Select source curve...");
                GO.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
                GO.AddOption("TrafficWelsh");
                GO.AddOption("TrafficFHWA");
                GO.AddOption("AircraftANCON");
                GO.AddOption("Custom");
                //GO.AddOptionList("SourceType", new List<string>() { "TrafficWelshStandard", "TrafficFHWA Standard", "Custom" }, 2);
                GO.GroupSelect = false;
                GO.SubObjectSelect = false;
                GO.EnableClearObjectsOnEntry(false);
                GO.EnableUnselectObjectsOnExit(false);
                GO.DeselectAllBeforePostSelect = false;

                int pavement = 0;
                double SPLW = 0;
                double[] SWL = new double[] { 120, 120, 120, 120, 120, 120, 120, 120 };
                double velocity = 83;
                double delta = 45;
                double choice = 0;
                for (; ; )
                {
                    Rhino.Input.GetResult GR = GO.GetMultiple(1, 1);
                    int type = GO.OptionIndex();
                    if (GR == Rhino.Input.GetResult.Option)
                    {
                        choice = (int)type;
                        //type = GO.Option().EnglishName;
                        if (type == 1)//"Traffic (Welsh Standard)")
                        {
                            Rhino.Input.RhinoGet.GetNumber("Input basis road sound pressure level at 1 m from street...", false, ref SPLW);
                            SPLW += 8;
                        }
                        else if (type == 2)//"Traffic (FHWA Standard)")
                        {
                            ///Described at:
                            ///http://www.fhwa.dot.gov/environment/noise/traffic_noise_model/old_versions/tnm_version_10/tech_manual/tnm03.cfm#tnma2

                            double s = 0;
                            //int i = 0;

                            Rhino.Input.RhinoGet.GetNumber("Enter the speed of traffic on this road (in kph)...", false, ref s);

                            ///Pavement
                            Rhino.Input.Custom.GetOption GOpt = new Rhino.Input.Custom.GetOption();
                            GOpt.SetCommandPrompt("Pavement type...");
                            GOpt.AddOption("Average_DGAC_PCC)");
                            GOpt.AddOption("DGAC_Asphalt");
                            GOpt.AddOption("PCC_Concrete");
                            GOpt.AddOption("OGAC_OpenGradedAsphalt");
                            GOpt.AcceptNothing(false);
                            GOpt.Get();
                            pavement = GOpt.OptionIndex();

                            ///Vehicle tallies
                            double[] Veh = new double[5] { 0, 0, 0, 0, 0 };
                            Rhino.Input.RhinoGet.GetNumber("Enter the number of automobiles per hour...", false, ref Veh[0]);
                            Rhino.Input.RhinoGet.GetNumber("Enter the number of medium trucks per hour...", false, ref Veh[1]);
                            Rhino.Input.RhinoGet.GetNumber("Enter the number of heavy trucks per hour...", false, ref Veh[2]);
                            Rhino.Input.RhinoGet.GetNumber("Enter the number of buses per hour...", false, ref Veh[3]);
                            Rhino.Input.RhinoGet.GetNumber("Enter the number of motorcycles per hour...", false, ref Veh[4]);

                            bool throttle = false;
                            int t = 0;
                            Rhino.Input.RhinoGet.GetBool("Full throttle?", false, "Yes", "No", ref throttle);
                            t = (throttle) ? 1 : 0;
                            double root2 = Math.Sqrt(2);
                            double vtot = 0;
                            double[] Es = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

                            for (int v = 0; v < 5; v++)
                            {
                                double A = FHWATraffic[v][pavement][t][0];
                                double B = FHWATraffic[v][pavement][t][1];
                                double C = FHWATraffic[v][pavement][t][2];
                                double D1 = FHWATraffic[v][pavement][t][3];
                                double D2 = FHWATraffic[v][pavement][t][4];
                                double E1 = FHWATraffic[v][pavement][t][5];
                                double E2 = FHWATraffic[v][pavement][t][6];
                                double F1 = FHWATraffic[v][pavement][t][7];
                                double F2 = FHWATraffic[v][pavement][t][8];
                                double G1 = FHWATraffic[v][pavement][t][9];
                                double G2 = FHWATraffic[v][pavement][t][10];
                                double H1 = FHWATraffic[v][pavement][t][11];
                                double H2 = FHWATraffic[v][pavement][t][12];
                                double I1 = FHWATraffic[v][pavement][t][13];
                                double I2 = FHWATraffic[v][pavement][t][14];
                                double J1 = FHWATraffic[v][pavement][t][15];
                                double J2 = FHWATraffic[v][pavement][t][16];

                                vtot += Veh[v];

                                for (int oct = 0; oct < 8; oct++)
                                {
                                    double f = 62.5 * Math.Pow(2, oct);
                                    double[] freq = new double[3] { f / root2, f, f * root2 };

                                    for (int oct3 = 0; oct3 < 3; oct3++)
                                    {
                                        double Ea = Math.Pow(0.6214 * s, A / 10) * Math.Pow(10, B / 10) + Math.Pow(10, C / 10);
                                        double logf = Math.Log10(freq[oct3]);
                                        double Ls = 10 * Math.Log10(Ea) + (D1 + 0.6214 * D2 * s) + (E1 + 0.6214 * E2 * s) * logf
                                            + (F1 + 0.6214 * F2 * s) * logf * logf + (G1 + 0.6214 * G2 * s) * logf * logf * logf
                                            + (H1 + 0.6214 * H2 * s) * logf * logf * logf * logf + (I1 + 0.6214 * I2 * s) * logf * logf * logf * logf * logf
                                            + (J1 + 0.6214 * J2 * s) * logf * logf * logf * logf * logf * logf;
                                        Es[oct] += 0.0476 * Math.Pow(10, Ls / 10) * Veh[v] / s;
                                    }
                                }
                            }

                            double[] Awt = new double[8] { -26, -16, -9, -3, 0, 1.2, 1, -1 };
                            double dmod = 10 * Math.Log10(1 / (Utilities.Numerics.PiX2 * 15));

                            for (int oct = 0; oct < 8; oct++)
                            {
                                SWL[oct] = 10 * Math.Log10(Es[oct]) - Awt[oct] - dmod;//
                            }
                        }
                        else if (type == 3)//"Aircraft (ANCON-derived)")
                        {
                            Rhino.Input.Custom.GetOption GOpt = new Rhino.Input.Custom.GetOption();
                            GOpt.SetCommandPrompt("Takeoff or Landing?");
                            GOpt.AddOption("Takeoff");
                            GOpt.AddOption("Landing");
                            GOpt.AddOption("Both");
                            GOpt.AcceptNothing(false);
                            GOpt.Get();
                            int TL_Choice = GOpt.OptionIndex();

                            double SWLA = 150;

                            Rhino.Input.RhinoGet.GetNumber("What is the broadband sound power of the aircraft (in dBA)?", false, ref SWLA);
                            Rhino.Input.RhinoGet.GetNumber("What is the maximum velocity of the aircraft in m/s?", false, ref velocity);
                            Rhino.Input.RhinoGet.GetNumber("What is the slant angle for this aircraft?", false, ref delta);

                            double[][] Aircraft_Normalization = new double[3][] {
                                //new double[8]{ -12, -10.5, -12, -15, -20, -27, -40, -44},
                                //new double[8]{-11, -13, -12, -13.5, -18, -21, -25, -35},
                                //new double[8]{-11, -10.5, -12, -13.5, -18, -21, -25, -35}
                            new double[8] { 6.191472203, 7.691472203, 6.191472203, 3.191472203, -1.808527797, -8.808527797,-21.8085278, -25.8085278},
                            new double[8] { 5.6783811710, 3.6783811710, 4.678381171, 3.178381171, -1.321618829, -4.321618829, -8.321618829, -18.32161883},
                            new double[8] { 5.678381171, 6.178381171, 4.678381171, 3.178381171, -1.321618829, -4.321618829, -8.321618829, -18.32161883}
                            };

                            for (int oct = 0; oct < 8; oct++)
                            {
                                SWL[oct] = SWLA + Aircraft_Normalization[TL_Choice-1][oct];//
                            }
                        }
                        //    continue;
                        //    //return Result.Success;
                        //}
                    }
                    else if (GR == Rhino.Input.GetResult.Object)
                    {
                        for (int i = 0; i < GO.ObjectCount; i++)
                        {
                            Rhino.DocObjects.ObjRef obj = GO.Object(i);

                            Rhino.DocObjects.RhinoObject rhObj = doc.Objects.Find(obj.ObjectId);

                            rhObj.Attributes.Name = "Acoustical Source";

                            if (choice == 1)//"Traffic (Welsh Standard)")
                            {
                                rhObj.Geometry.SetUserString("SourceType", "Traffic (Welsh)");
                                for (int oct = 0; oct < 8; oct++) SWL[oct] = SPLW + WelshTraffic[oct];
                            }
                            else if (choice == 2)//"Traffic (FWHA Standard)")
                            {
                                rhObj.Geometry.SetUserString("SourceType", "Traffic (FHWA)");
                            }
                            else if (choice == 3)//"Aircraft (ANCON-derived)")
                            {
                                rhObj.Geometry.SetUserString("SourceType", "Aircraft (ANCON derived)");
                                rhObj.Geometry.SetUserString("Velocity", velocity.ToString());
                                rhObj.Geometry.SetUserString("delta", delta.ToString());
                            }
                            else
                            {
                                Rhino.Input.RhinoGet.GetNumber("62.5 Hz. Sound Power Level", true, ref SWL[0]);
                                Rhino.Input.RhinoGet.GetNumber("125 Hz. Sound Power Level", true, ref SWL[1]);
                                Rhino.Input.RhinoGet.GetNumber("250 Hz. Sound Power Level", true, ref SWL[2]);
                                Rhino.Input.RhinoGet.GetNumber("500 Hz. Sound Power Level", true, ref SWL[3]);
                                Rhino.Input.RhinoGet.GetNumber("1000 Hz. Sound Power Level", true, ref SWL[4]);
                                Rhino.Input.RhinoGet.GetNumber("2000 Hz. Sound Power Level", true, ref SWL[5]);
                                Rhino.Input.RhinoGet.GetNumber("4000 Hz. Sound Power Level", true, ref SWL[6]);
                                Rhino.Input.RhinoGet.GetNumber("8000 Hz. Sound Power Level", true, ref SWL[7]);
                            }

                            rhObj.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(SWL));
                            rhObj.Geometry.SetUserString("Phase", "0;0;0;0;0;0;0;0");
                            doc.Objects.ModifyAttributes(rhObj, rhObj.Attributes, true);

                            m_source_conduit.SetSource(rhObj);
                        }

                        doc.Views.Redraw();
                        return Result.Success;
                    }
                    else return Result.Cancel;
                }
            }
        }

        /// <summary>
        /// Handles the source objects, and displays an icon instead of the point object it is based on.
        /// </summary>
        public class SourceConduit : Rhino.Display.DisplayConduit
        {
            private bool m_bHandlerAdded = false;
            private List<System.Guid> m_id_list = new List<System.Guid>();
            public List<Balloon> m_Balloons = new List<Balloon>();
            public Dodec d = new Dodec(0.18);
            DisplayBitmap SS;
            DisplayBitmap SU;
            DisplayBitmap LS;
            DisplayBitmap LU;

            public SourceConduit()
            :base()
            {
                System.Drawing.Bitmap SSbmp = Pachyderm_Acoustic.Properties.Resources.Source_Selected;
                SSbmp.MakeTransparent(System.Drawing.Color.Black);
                System.Drawing.Bitmap SUbmp = Pachyderm_Acoustic.Properties.Resources.Source;
                SUbmp.MakeTransparent(System.Drawing.Color.Black);
                System.Drawing.Bitmap LSbmp = Pachyderm_Acoustic.Properties.Resources.LoudSpeaker_Selected;
                SSbmp.MakeTransparent(System.Drawing.Color.Black);
                System.Drawing.Bitmap LUbmp = Pachyderm_Acoustic.Properties.Resources.LoudSpeaker;
                SSbmp.MakeTransparent(System.Drawing.Color.Black);

                SS = new DisplayBitmap(SSbmp);
                SU = new DisplayBitmap(SUbmp);
                LS = new DisplayBitmap(LSbmp);
                LU = new DisplayBitmap(LUbmp);

                Instance = this;
            }

            ///<summary>The only instance of this command.</summary>
            public static SourceConduit Instance
            {
                get;
                private set;
            }
            /// <summary>
            /// Sets up watchers, which trigger events in the handling of the source objects.
            /// </summary>
            /// <param name="bAdd"></param>
            private void SetupEventHandlers(bool bAdd)
            {
                if ((bAdd))
                {
                    if ((!m_bHandlerAdded))
                    {
                        Rhino.RhinoDoc.AddRhinoObject += OnCopyObject;
                        Rhino.RhinoDoc.DeleteRhinoObject += OnDeleteObject;
                        Rhino.RhinoDoc.ReplaceRhinoObject += OnReplaceObject;
                        Rhino.RhinoDoc.BeginOpenDocument += ClearConduit;
                        Rhino.RhinoDoc.NewDocument += ClearConduit;
                    }
                }
                else
                {
                    Rhino.RhinoDoc.AddRhinoObject -= OnCopyObject;
                    Rhino.RhinoDoc.DeleteRhinoObject -= OnDeleteObject;
                    Rhino.RhinoDoc.ReplaceRhinoObject -= OnReplaceObject;
                    Rhino.RhinoDoc.BeginOpenDocument -= ClearConduit;
                    Rhino.RhinoDoc.NewDocument -= ClearConduit;
                }

                m_bHandlerAdded = bAdd;
            }

            private bool m_bReplaceCalled = false;

            private void ClearConduit(object sender, EventArgs e)
            {
                m_id_list.Clear();
                this.Enabled = false;
                // we don't want to watch for events any more 
                SetupEventHandlers(false);
            }

            private void OnReplaceObject(object sender, Rhino.DocObjects.RhinoReplaceObjectEventArgs e)
            {
                m_bReplaceCalled = true;

                try
                {
                    string typ = "";
                    typ = e.OldRhinoObject.Geometry.GetUserString("SourceType");
                    if (typ != "2" && typ != "3") return;

                    for (int i = 0; i < m_id_list.Count; i++)
                    {
                        if ((m_id_list[i] != System.Guid.Empty && e.NewRhinoObject != null))
                        {
                            if ((e.NewRhinoObject.Id == m_id_list[i]))
                            {
                                Rhino.Geometry.BoundingBox bbox = e.NewRhinoObject.Geometry.GetBoundingBox(true);
                                Update_Position(i, new Rhino.Geometry.Point3d(bbox.Min.X, bbox.Min.Y, bbox.Min.Z));
                            }
                        }
                    }
                }
                catch (Exception x)
                {
                    Rhino.RhinoApp.WriteLine(x.Message);
                }
            }

            /// <summary>
            /// What to do if an object is copied...
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="rhObject"></param>
            private void OnCopyObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
            {
                if (e.TheObject.Attributes.Name == "Acoustical Source") SetSource(e.TheObject);
            }

            /// <summary>
            /// What to do if an object is deleted...
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="rhObject"></param>
            private void OnDeleteObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
            {
                // skip this if a replace was called beforehand 
                if ((m_bReplaceCalled))
                {
                    m_bReplaceCalled = false;
                    return;
                }

                foreach (System.Guid m_id in m_id_list)
                {
                    if ((m_id != System.Guid.Empty && e != null))
                    {
                        if ((e.TheObject.Attributes.ObjectId == m_id))
                        {
                            m_id_list.Remove(m_id);
                            if (m_id_list.Count < 1)
                            {
                                this.Enabled = false;
                                // we don't want to watch for events any more 
                                SetupEventHandlers(false);
                            }
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// Adds a source to the conduit.
            /// </summary>
            /// <param name="rhino_object"></param>
            public void SetSource(Rhino.DocObjects.RhinoObject rhino_object)
            {
                if ((rhino_object == null))
                {
                    if (m_id_list.Count == 0)
                    {
                        SetupEventHandlers(false);
                        Enabled = false;
                        return;
                    }
                }

                SetupEventHandlers(true);
                Enabled = true;
                
                if (!m_id_list.Contains(rhino_object.Attributes.ObjectId))
                {
                    m_id_list.Add(rhino_object.Attributes.ObjectId);
                    string typ = "";
                    typ = rhino_object.Geometry.GetUserString("SourceType");
                    if (rhino_object.ObjectType == Rhino.DocObjects.ObjectType.Point)
                    {
                        if (typ == "2")
                        {
                            string[] strballoon = new string[8];
                            string Aim = "";
                            string ft = "";
                            string SWLMax = "";
                            strballoon[0] = rhino_object.Geometry.GetUserString("Balloon63");
                            strballoon[1] = rhino_object.Geometry.GetUserString("Balloon125");
                            strballoon[2] = rhino_object.Geometry.GetUserString("Balloon250");
                            strballoon[3] = rhino_object.Geometry.GetUserString("Balloon500");
                            strballoon[4] = rhino_object.Geometry.GetUserString("Balloon1000");
                            strballoon[5] = rhino_object.Geometry.GetUserString("Balloon2000");
                            strballoon[6] = rhino_object.Geometry.GetUserString("Balloon4000");
                            strballoon[7] = rhino_object.Geometry.GetUserString("Balloon8000");
                            Aim = rhino_object.Geometry.GetUserString("Aiming");
                            SWLMax = rhino_object.Geometry.GetUserString("SWLMax");
                            string[] A = Aim.Split(';');
                            ft = rhino_object.Geometry.GetUserString("FileType");
                            Speaker_Balloon L = new Speaker_Balloon(strballoon, SWLMax, int.Parse(ft),Utilities.RC_PachTools.RPttoHPt(rhino_object.Geometry.GetBoundingBox(true).Min));
                            L.CurrentAlt = float.Parse(A[0]);
                            L.CurrentAzi = float.Parse(A[1]);
                            L.CurrentAxi = float.Parse(A[2]);
                            L.Update_Position();
                            this.m_Balloons.Add(L);
                        }
                        else if (typ == "3")
                        {
                            string[] strballoon = new string[8];
                            string Aim = "";
                            string SWLMax = "";
                            strballoon[0] = rhino_object.Geometry.GetUserString("Balloon63");
                            strballoon[1] = rhino_object.Geometry.GetUserString("Balloon125");
                            strballoon[2] = rhino_object.Geometry.GetUserString("Balloon250");
                            strballoon[3] = rhino_object.Geometry.GetUserString("Balloon500");
                            strballoon[4] = rhino_object.Geometry.GetUserString("Balloon1000");
                            strballoon[5] = rhino_object.Geometry.GetUserString("Balloon2000");
                            strballoon[6] = rhino_object.Geometry.GetUserString("Balloon4000");
                            strballoon[7] = rhino_object.Geometry.GetUserString("Balloon8000");
                            Aim = rhino_object.Geometry.GetUserString("Aiming");
                            SWLMax = rhino_object.Geometry.GetUserString("SWLMax");
                            string[] A = Aim.Split(';');
                            //ft = rhino_object.Geometry.GetUserString("FileType");
                            Balloon L = new Balloon(strballoon, Utilities.RC_PachTools.RPttoHPt(rhino_object.Geometry.GetBoundingBox(true).Min));
                            L.CurrentAlt = float.Parse(A[0]);
                            L.CurrentAzi = float.Parse(A[1]);
                            L.CurrentAxi = float.Parse(A[2]);
                            L.Update_Position();
                            this.m_Balloons.Add(L);
                        }
                        else
                        {
                            m_Balloons.Add(null);
                        }
                    }
                    else if (rhino_object.ObjectType == Rhino.DocObjects.ObjectType.Curve)
                    {
                        m_Balloons.Add(null);
                    }
                    else if (rhino_object.ObjectType == Rhino.DocObjects.ObjectType.Brep)
                    {
                        m_Balloons.Add(null);
                    }
                }
            }

            protected override void DrawForeground(DrawEventArgs e)
            {
                int index = 0;
                foreach (Guid G in m_id_list)
                {
                    Rhino.DocObjects.RhinoObject rhobj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(G);
                    if (rhobj == null)
                    {
                        m_id_list.Remove(G);
                        m_Balloons.RemoveAt(index);
                        continue;
                    }

                    if (rhobj.Attributes.ObjectId == G)
                    {
                        if (rhobj.ObjectType == Rhino.DocObjects.ObjectType.Point)
                        {
                            string mode = rhobj.Geometry.GetUserString("SourceType");
                            // Draw our own representation of the object 
                            //m_pChannelAttrs.m_bDrawObject = false;
                            Rhino.Geometry.Point3d pt = rhobj.Geometry.GetBoundingBox(true).Min;
                            // this is a point object so the bounding box will be wee sized 
                            Rhino.Geometry.Point2d screen_pt = e.Display.Viewport.WorldToClient(pt);
                            if ((mode == "2" || mode == "3") && m_Balloons[index] != null)
                            {
                                if ((rhobj.IsSelected(false) != 0))
                                {
                                    //Display the balloon for 1khz.
                                    e.Display.DrawSprite(LS, pt, 0.25f, true);// screen_pt, 32.0f);
                                    e.Display.DrawMeshWires(Utilities.RC_PachTools.Hare_to_RhinoMesh(this.m_Balloons[index].m_DisplayMesh, false), Color.Blue);
                                    e.Display.Draw2dText(index.ToString(), Color.Yellow, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                                    double Theta = (m_Balloons[index].CurrentAlt + 270) * System.Math.PI / 180;
                                    double Phi = (m_Balloons[index].CurrentAzi - 90) * System.Math.PI / 180;
                                    Hare.Geometry.Vector Direction = new Hare.Geometry.Vector(Math.Sin(Theta) * Math.Cos(Phi), Math.Sin(Theta) * Math.Sin(Phi), Math.Cos(Theta));
                                    e.Display.DrawLine(pt, new Rhino.Geometry.Point3d(Direction.x * 1000, Direction.y * 1000, Direction.z * 1000) + pt, Color.Red, 1);
                                }
                                else
                                {
                                    //Display the Icon for a loudspeaker.
                                    e.Display.DrawSprite(LU, pt, 0.5f, true);// screen_pt, 32.0f);
                                    //e.Display.DrawMeshVertices(this.m_Balloons[i].m_DisplayMesh, R);
                                    e.Display.Draw2dText(index.ToString(), Color.Black, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                                }
                            }
                            else
                            {
                                if ((rhobj.IsSelected(false) != 0))
                                {
                                    e.Display.DrawLines(d.GetLines(pt), Color.Yellow);
                                    List<Circle> circles = d.GetCircles(pt);
                                    foreach (Circle c in circles)
                                    {
                                        e.Display.DrawCircle(c, Color.Red);
                                        Circle c1 = c;
                                        c1.Radius += 0.02;
                                        e.Display.DrawCircle(c1, Color.Red);
                                        c1.Radius -= 0.04;
                                        e.Display.DrawCircle(c1, Color.Red);
                                    }
                                    //e.Display.DrawSprite(SS, pt, 0.5f, true);// screen_pt, 32.0f);
                                    e.Display.Draw2dText(index.ToString(), Color.Yellow, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                                }
                                else
                                {
                                    e.Display.DrawLines(d.GetLines(pt), Color.Black);
                                    List<Circle> circles = d.GetCircles(pt);
                                    foreach (Circle c in circles)
                                    {
                                        e.Display.DrawCircle(c, Color.Red);
                                        Circle c1 = c;
                                        c1.Radius += 0.02;
                                        e.Display.DrawCircle(c1, Color.Red);
                                        c1.Radius -= 0.04;
                                        e.Display.DrawCircle(c1, Color.Red);
                                    }
                                    //e.Display.DrawSprite(SU, pt, 0.5f, true);// screen_pt, 32.0f, Color.Black);
                                    e.Display.Draw2dText(index.ToString(), Color.Black, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                                }
                            }
                        }
                        else if (rhobj.ObjectType == Rhino.DocObjects.ObjectType.Brep)
                        {
                            Rhino.Geometry.Point3d pt = (rhobj.Geometry as Rhino.Geometry.Brep).GetBoundingBox(false).Center;
                            Rhino.Geometry.Point2d screen_pt = e.Display.Viewport.WorldToClient(pt);
                            if ((rhobj.IsSelected(false) != 0))
                            {
                                e.Display.DrawSprite(SS, pt, 0.5f, true);// screen_pt, 32.0f);
                                e.Display.Draw2dText(index.ToString(), Color.Yellow, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                            }
                            else
                            {
                                e.Display.DrawSprite(SU, pt, 0.5f, true);// screen_pt, 32.0f, Color.Black);
                                e.Display.Draw2dText(index.ToString(), Color.Black, new Rhino.Geometry.Point2d((int)screen_pt.X, (int)screen_pt.Y + 40), false, 12, "Arial");
                            }
                        }
                        else if (rhobj.ObjectType == Rhino.DocObjects.ObjectType.Curve)
                        {
                            Rhino.Geometry.Curve crv = (rhobj.Geometry as Rhino.Geometry.Curve);
                            Rhino.Geometry.Point3d[] pts;
                            double[] par = crv.DivideByLength(10, true, out pts);
                            if (par == null || par.Length == 0) par = crv.DivideByCount(2, true, out pts);

                            for(int i = 0; i < par.Length; i++)
                            {
                                Rhino.Geometry.Vector3d V = crv.TangentAt(par[i]);
                                e.Display.DrawCircle(new Rhino.Geometry.Circle(new Rhino.Geometry.Plane(pts[i], V), .5), System.Drawing.Color.Red);
                                e.Display.DrawCircle(new Rhino.Geometry.Circle(new Rhino.Geometry.Plane(pts[i], V), .7), System.Drawing.Color.Red);
                                e.Display.DrawCircle(new Rhino.Geometry.Circle(new Rhino.Geometry.Plane(pts[i], V), 1.2), System.Drawing.Color.Red);
                                e.Display.DrawCircle(new Rhino.Geometry.Circle(new Rhino.Geometry.Plane(pts[i], V), 1.4), System.Drawing.Color.Red);
                                e.Display.DrawCircle(new Rhino.Geometry.Circle(new Rhino.Geometry.Plane(pts[i], V), 1.9), System.Drawing.Color.Red);
                                e.Display.DrawCircle(new Rhino.Geometry.Circle(new Rhino.Geometry.Plane(pts[i], V), 2.1), System.Drawing.Color.Red); 
                            }
                            
                            Rhino.Geometry.Point2d screen_pt0 = e.Display.Viewport.WorldToClient(pts[0]);
                            Rhino.Geometry.Point2d screen_pt1 = e.Display.Viewport.WorldToClient(pts[pts.Length-1]);
                            if ((rhobj.IsSelected(false) != 0))
                            {
                                e.Display.Draw2dText(index.ToString(), Color.Yellow, new Rhino.Geometry.Point2d((int)screen_pt0.X, (int)screen_pt0.Y + 40), false, 12, "Arial");
                                e.Display.Draw2dText(index.ToString(), Color.Yellow, new Rhino.Geometry.Point2d((int)screen_pt1.X, (int)screen_pt1.Y + 40), false, 12, "Arial");
                            }
                            else
                            {
                                e.Display.Draw2dText(index.ToString(), Color.Black, new Rhino.Geometry.Point2d((int)screen_pt0.X, (int)screen_pt0.Y + 40), false, 12, "Arial");
                                e.Display.Draw2dText(index.ToString(), Color.Black, new Rhino.Geometry.Point2d((int)screen_pt1.X, (int)screen_pt1.Y + 40), false, 12, "Arial");
                            }
                        }
                    }
                    index++;
                }
            }

            public void AddBalloon(System.Guid ID, Balloon B)
            {
                for (int i = 0; i < m_id_list.Count; i++)
                {
                    if ((ID == m_id_list[i]))
                    {
                        if (m_Balloons == null) m_Balloons = new List<Balloon>();
                        if (m_Balloons.Count < m_id_list.Count)
                        {
                            while (m_Balloons.Count < m_id_list.Count) m_Balloons.Add(null);
                        }
                        m_Balloons[i] = B;
                        return;
                    }
                }
            }

            /// <summary>
            /// The ids of the receiver point objects. Rhino objects all have GUIDs.
            /// </summary>
            public List<System.Guid> UUID
            {
                get
                {
                    return m_id_list;
                }
            }

            /// <summary>
            /// Call when balloons or sources have key changes.
            /// </summary>
            public void Update_Position(int ID, Rhino.Geometry.Point3d P)
            {
                if (m_Balloons[ID] == null) return;
                m_Balloons[ID].Update_Position(Utilities.RC_PachTools.RPttoHPt(P));
            }

            public double[] SWL(int idx)
            {
                string Pow = Rhino.RhinoDoc.ActiveDoc.Objects.Find(UUID[idx]).Geometry.GetUserString("SWL");
                string[] power = Pow.Split(';');
                double[] powerLevels = new double[8];
                powerLevels[0] = double.Parse(power[0]);
                powerLevels[1] = double.Parse(power[1]);
                powerLevels[2] = double.Parse(power[2]);
                powerLevels[3] = double.Parse(power[3]);
                powerLevels[4] = double.Parse(power[4]);
                powerLevels[5] = double.Parse(power[5]);
                powerLevels[6] = double.Parse(power[6]);
                powerLevels[7] = double.Parse(power[7]);
                return powerLevels;
            }

            public void Update_Aiming(System.Guid id, float alt, float azi, float axi)
            {
                for (int i = 0; i < m_id_list.Count; i++)
                {
                    if (m_Balloons == null) return;
                    //IRhinoObject rhobj = m_pChannelAttrs.m_pObject;
                    if ((id == m_id_list[i]))
                    {
                        if (m_Balloons[i] == null) return;
                        m_Balloons[i].CurrentAlt = alt;
                        m_Balloons[i].CurrentAzi = azi;
                        m_Balloons[i].CurrentAxi = axi;
                        m_Balloons[i].Update_Position();
                    }
                }
            }

            public class Dodec
            {
                List<Rhino.Geometry.Line> L = new List<Rhino.Geometry.Line>();
                Vector3d[] C = new Vector3d[12];
                double r;

                public Dodec(double radius)
                {
                    r = radius;
                    double phi = (Math.Sqrt(5) - 1) / 2;
                    double a = r / Math.Sqrt(3);
                    double b =  a / phi;
                    double c =  a * phi;
                    int[] sign = new int[2] { -1, 1 };

                    L.Add(new Line(c, 0, b, -c, 0, b));//0
                    L.Add(new Line(c, 0, -b, -c, 0, -b));//1
                    L.Add(new Line(L[0].From, new Point3d(a, a, a)));//2
                    L.Add(new Line(L[0].From, new Point3d(a, -a, a)));//3
                    L.Add(new Line(L[0].To, new Point3d(-a, a, a)));//4
                    L.Add(new Line(L[0].To, new Point3d(-a, -a, a)));//5
                    L.Add(new Line(L[1].From, new Point3d(a, a, -a)));//6
                    L.Add(new Line(L[1].From, new Point3d(a, -a, -a)));//7
                    L.Add(new Line(L[1].To, new Point3d(-a, a, -a)));//8
                    L.Add(new Line(L[1].To, new Point3d(-a, -a, -a)));//9
                    L.Add(new Line(L[2].To, new Point3d(0, b, c)));//10
                    L.Add(new Line(L[4].To, new Point3d(0, b, c)));//11
                    L.Add(new Line(L[3].To, new Point3d(0, -b, c)));//12
                    L.Add(new Line(L[5].To, new Point3d(0, -b, c)));//13
                    L.Add(new Line(L[6].To, new Point3d(0, b, -c)));//14
                    L.Add(new Line(L[8].To, new Point3d(0, b, -c)));//15
                    L.Add(new Line(L[7].To, new Point3d(0, -b, -c)));//16
                    L.Add(new Line(L[9].To, new Point3d(0, -b, -c)));//17
                    L.Add(new Line(L[10].To, L[14].To));//18
                    L.Add(new Line(L[12].To, L[16].To));//19
                    Point3d Ppp = new Point3d(b, c, 0);
                    Point3d Ppm = new Point3d(b, -c, 0);
                    Point3d Pmp = new Point3d(-b, c, 0);
                    Point3d Pmm = new Point3d(-b, -c, 0);
                    L.Add(new Line(L[2].To, Ppp));//20
                    L.Add(new Line(L[6].To, Ppp));//21
                    L.Add(new Line(L[3].To, Ppm));//22
                    L.Add(new Line(L[7].To, Ppm));//23
                    L.Add(new Line(L[21].To, L[22].To));//24
                    L.Add(new Line(L[4].To, Pmp));//25
                    L.Add(new Line(L[8].To, Pmp));//26
                    L.Add(new Line(L[5].To, Pmm));//27
                    L.Add(new Line(L[9].To, Pmm));//28
                    L.Add(new Line(L[26].To, L[28].To));//29

                    r *= 0.7;
                    C[0] = new Vector3d(-r, 0, -r * phi);
                    C[1] = new Vector3d(-r, 0, r * phi);
                    C[2] = new Vector3d(r, 0, -r * phi);
                    C[3] = new Vector3d(r, 0, r * phi);
                    C[4] = new Vector3d(-r * phi, -r, 0);
                    C[5] = new Vector3d(-r * phi, r, 0);
                    C[6] = new Vector3d(r * phi, -r, 0);
                    C[7] = new Vector3d(r * phi, r, 0);
                    C[8] = new Vector3d(0, -r * phi, -r);
                    C[9] = new Vector3d(0, r * phi, -r);
                    C[10] = new Vector3d(0, -r * phi, r);
                    C[11] = new Vector3d(0, r * phi, r);
                }

                public List<Line> GetLines(Point3d p)
                {
                    List<Line> OUTPUT = new List<Line>();

                    foreach (Line l in L)
                    {
                        OUTPUT.Add(new Line(l.From + p, l.To + p));
                    }

                    return OUTPUT;
                }

                public List<Circle> GetCircles(Point3d p)
                {
                    List<Circle> OUTPUT = new List<Circle>();

                    foreach (Vector3d c in C)
                    {
                        OUTPUT.Add(new Circle(new Plane(c + p, c), .3 * r));
                    }

                    return OUTPUT;
                }
            }
        }
    }
}