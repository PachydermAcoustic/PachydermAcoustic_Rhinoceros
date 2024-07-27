using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Pach_Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;
using System.Runtime.CompilerServices;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public class SourceListBox : Scrollable
        {
            private ContextMenu Sources;
            private ButtonMenuItem DelayMod;
            private ButtonMenuItem PowerMod;
            private List<string> Srcs;
            private List<string> SrcTypes;
            Pachyderm_Acoustic.Direct_Sound[] DS = null;
            Pachyderm_Acoustic.ImageSourceData[] IS = null;
            Pachyderm_Acoustic.Environment.Receiver_Bank[] Rec;
            
            public double[] Delay;
            public event EventHandler Update;
            List<CheckBox> SrcBoxes = new List<CheckBox>();

            public SourceListBox()
            {
                Sources = new ContextMenu();
                DelayMod = new ButtonMenuItem();
                PowerMod = new ButtonMenuItem();
                DelayMod.Text = "Modify Selected Source Delay...";
                PowerMod.Text = "Modify Selected Source Power...";
                DelayMod.Click += DelayMod_Click;
                PowerMod.Click += ModifyPower_Click;
                Sources.Items.Add(PowerMod);
                Sources.Items.Add(DelayMod);
                this.ContextMenu = Sources;

                Clear();
            }

            //public void Populate(Pachyderm_Acoustic.Environment.Source[] Sourceobjs, Pachyderm_Acoustic.Environment.Receiver_Bank[] SimResults)
            //{
            //    DS = null;
            //    IS = null;
            //    Rec = SimResults;
            //    SrcBoxes.Clear();
            //    DynamicLayout SrcLayout = new DynamicLayout();
            //    Delay = new double[Sourceobjs.Length];

            //    foreach (Source s in Sourceobjs)
            //    {
            //        CheckBox src = new CheckBox();
            //        src.Text = String.Format("S{0}-", s.Source_ID()) + s.Type();
            //        src.MouseUp += update_proxy;
            //        SrcBoxes.Add(src);
            //        SrcLayout.AddRow(src);
            //    }
            //    this.Content = SrcLayout;
            //}

            private void update_proxy(object sender, MouseEventArgs e)
            {
                if (Update != null) Update(sender, e);
            }
            private void update_proxy(object sender, EventArgs e)
            {
                if (Update != null) Update(sender, e);
            }

            public void Populate(Direct_Sound[] Direct, ImageSourceData[] ISdata = null, Receiver_Bank[] Receiver = null)
            {
                if (Direct == null && Receiver == null) { throw new Exception("Misuse of Source list component - must have eithe direct sound or a raytracing receiver."); }
                DS = Direct;
                IS = ISdata;
                Rec = Receiver;
                SrcBoxes.Clear();
                DynamicLayout SrcLayout = new DynamicLayout();
                int ct = (Direct != null) ? Direct.Length : Receiver.Length;
                Delay = new double[ct];
                SrcTypes = new List<string>();
                for (int i = 0; i < ct; i++)
                {
                    string type = (Direct != null) ? Direct[i].type : Receiver[i].SrcType;
                    Delay[i] = (Direct != null) ? Direct[i].Delay_ms : Receiver[i].delay_ms;

                    CheckBox src = new CheckBox();
                    src.CheckedChanged += update_proxy;
                    src.Text = String.Format("S{0}-", i) + type;
                    SrcBoxes.Add(src);
                    SrcTypes.Add(type);
                    SrcLayout.AddRow(src);
                }

                SrcBoxes[0].Checked = true;

                this.Content = SrcLayout;
            }

            public void Clear()
            {
                SrcBoxes = new List<CheckBox>();
                DynamicLayout Empty = new DynamicLayout();
                Label Nothing = new Label();
                Nothing.Text = "No Source Objects Present...";
                Empty.AddRow(Nothing);
                this.Content = Empty;
            }

            public List<int> SelectedSources()
            {
                List<int> sources = new List<int>();
                if (SrcBoxes == null || SrcBoxes.Count == 0) return new List<int>();

                for (int i = 0; i < SrcBoxes.Count; i++)
                {
                    if (SrcBoxes[i].Checked.Value) sources.Add(i);
                }
                return sources;
            }

            private void DelayMod_Click(object sender, System.EventArgs e)
            {
                //Interface for time selection...
                List<int> SrcID = SelectedSources();

                double t = Rec[SrcID[0]].delay_ms;
                Rhino.Input.RhinoGet.GetNumber("Enter the delay to assign to selected source object(s)...", false, ref t, 0, 200);

                foreach (int id in SrcID)
                {
                    Delay[id] = t;
                    if (Rec != null) Rec[id].delay_ms = t;
                }

                Update(this, EventArgs.Empty);
            }

            private void ModifyPower_Click(object sender, System.EventArgs e)
            {
                List<int> SrcID = SelectedSources();

                if (SrcID.Count < 1) return;
                Pachyderm_Acoustic.SourcePowerMod mod = new SourcePowerMod(DS[SrcID[0]].SWL);
                mod.ShowModal();
                if (mod.accept)
                {
                    foreach (int i in SrcID)
                    {
                        double[] factor = null;
                        if (DS != null)
                        {
                            factor = DS[i].Set_Power(mod.Power);
                            DS[i].Create_Filter();
                        }
                        if (Rec != null)
                        {
                            if (factor == null) factor = Rec[i].PowerModFactor(mod.Power);
                            Rec[i].Set_Power(factor);
                            Pachyderm_Acoustic.ProgressBox VB = new ProgressBox("Creating Filter...");
                            VB.Show(Rhino.RhinoDoc.ActiveDoc);
                            Rec[i].Create_Filter(VB);
                            VB.Close();
                        }
                        if(IS != null)
                        {
                            Pachyderm_Acoustic.ProgressBox VB = new ProgressBox("Building Image Source Paths...");
                            VB.Show(Rhino.RhinoDoc.ActiveDoc);
                            IS[i].Set_Power(factor);
                            IS[i].Create_Filter(mod.Power, 4096, VB);
                            VB.Close();
                        }
                        
                    }
                }
                Update(this, EventArgs.Empty);
            }

            public void Set_Src_Checked(int index, bool Checked)
            {
                SrcBoxes[index].Checked = Checked;
                this.Invalidate();
            }

            public string[] SourceTypes
            {
                get
                {
                    List<int> srcs = SelectedSources();
                    string[] types = new string[srcs.Count];
                    for(int i = 0; i < srcs.Count; i++) 
                    {
                        types[i] = SrcTypes[i];
                    }
                    return types;
                }
            }
            
            public int Count
            { get { return SrcBoxes.Count; } }
        }

        public class PathListBox : Scrollable
        {
            private ContextMenu PathOperations;
            private ButtonMenuItem CheckAll;
            private ButtonMenuItem UncheckAll;
            private ButtonMenuItem SortArrival;
            private ButtonMenuItem SortOrder;
            List<Deterministic_Reflection> IS_Paths;
            public event EventHandler Update;
            List<CheckBox> PathBoxes = new List<CheckBox>();

            public PathListBox()
            {
                Height = 75;
                PathOperations = new ContextMenu();
                CheckAll = new ButtonMenuItem();
                UncheckAll = new ButtonMenuItem();
                SortArrival = new ButtonMenuItem();
                SortOrder = new ButtonMenuItem();
                CheckAll.Text = "Check all...";
                UncheckAll.Text = "Uncheck all...";
                SortArrival.Text = "Sort by arrival time...";
                SortOrder.Text = "Sort by order...";
                CheckAll.Click += ISCheckAll_Click;
                UncheckAll.Click += ISUncheckAll_Click;
                SortArrival.Click += SortPathsArrival;
                SortOrder.Click += SortPathsOrder;
                PathOperations.Items.Add(UncheckAll);
                PathOperations.Items.Add(CheckAll);
                PathOperations.Items.Add(SortArrival);
                PathOperations.Items.Add(SortOrder);
                this.ContextMenu = PathOperations;
                this.MouseUp += update_proxy;

                Clear();
            }

            public void Populate(ImageSourceData[] SimResults, List<int> source, int receiver)
            {
                if (IS_Paths == null) IS_Paths = new List<Deterministic_Reflection>(); 
                IS_Paths.Clear();

                foreach (int s in source)
                {
                    for (int i = 0; i < SimResults.Length; i++)
                    {
                        IS_Paths.AddRange(SimResults[i].Paths[receiver]);
                    }
                }

                Populate();
                SortPathsArrival(this, null);
            }

            private void Populate()
            {
                DynamicLayout SrcLayout = new DynamicLayout();
                PathBoxes.Clear();
                foreach (Deterministic_Reflection s in IS_Paths)
                {
                    CheckBox path = new CheckBox();
                    path.CheckedChanged += update_proxy;
                    path.Text = s.ToString();
                    PathBoxes.Add(path);
                    SrcLayout.AddRow(path);
                }
                this.Content = SrcLayout;
            }

            private void update_proxy(object sender, EventArgs e)
            {
                Update(sender, e);
            }

            public void Clear()
            {
                PathBoxes = new List<CheckBox>();
                DynamicLayout Empty = new DynamicLayout();
                Label Nothing = new Label();
                Nothing.Text = "No Reflections Present...";
                Empty.AddRow(Nothing);
                this.Content = Empty;
            }

            public List<Deterministic_Reflection> SelectedPaths()
            {
                List<Deterministic_Reflection> paths = new List<Deterministic_Reflection>();
                if (PathBoxes == null || PathBoxes.Count == 0) return new List<Deterministic_Reflection>();

                for (int i = 0; i < PathBoxes.Count; i++)
                {
                    if (PathBoxes[i].Checked.Value) paths.Add(IS_Paths[i]);
                }
                return paths;
            }

            private void SortPathsArrival(object sender, EventArgs e)
            {
                //Sort by time.
                Deterministic_Reflection[] DR = IS_Paths.Cast<Deterministic_Reflection>().ToArray();
                DR = DR.OrderBy<Deterministic_Reflection, double>(i => (i.TravelTime)).ToArray();
                IS_Paths.Clear();
                IS_Paths.AddRange(DR);
                Populate();
            }

            private void SortPathsOrder(object Sender, EventArgs e)
            {
                //Sort by order.
                Deterministic_Reflection[] DR = IS_Paths.Cast<Deterministic_Reflection>().ToArray();
                DR = DR.OrderBy<Deterministic_Reflection, int>(i => (i.Reflection_Sequence.Length)).ToArray();
                IS_Paths.Clear();
                IS_Paths.AddRange(DR);
                Populate();
            }

            private void ISCheckAll_Click(object sender, EventArgs e)
            {
                for (int i = 0; i < PathBoxes.Count; i++) SetItemChecked(i, true);
                Update(this, null);
                //IS_Path_Box_MouseUp(null, null);
            }

            private void ISUncheckAll_Click(object sender, EventArgs e)
            {
                for (int i = 0; i < PathBoxes.Count; i++) SetItemChecked(i, false);
                Update(this, null);
                //IS_Path_Box_MouseUp(null, null);
            }

            public void SetItemChecked(int index, bool Checked)
            {
                PathBoxes[index].Checked = Checked;
            }

            public int Count {  get { return PathBoxes.Count; } }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                PathOperations.Dispose();
                CheckAll.Dispose();
                UncheckAll.Dispose();
                SortArrival.Dispose();
                SortOrder.Dispose();
            }
        }

        public class Medium_Properties_Group : GroupBox
        {
            internal Label methodlbl;
            internal DropDown Atten_Method;
            internal Label PaLbl;
            internal NumericStepper Air_Pressure;
            internal Label RhLbl;
            internal NumericStepper Rel_Humidity;
            internal Label AirTemp;
            internal NumericStepper Air_Temp;
            private CheckBox EdgeFreq;

            public Medium_Properties_Group()
                : base()
            {
                this.methodlbl = new Label();
                this.methodlbl.Text = "Method:";
                this.EdgeFreq = new CheckBox();
                this.EdgeFreq.Text = "Edge Frequency Correction";
                DynamicLayout EDL = new DynamicLayout();
                EDL.DefaultSpacing = new Size(8, 8);
                EDL.Padding = 8;
                this.Text = "Environmental Factors";

                this.Atten_Method = new DropDown();
                this.Atten_Method.Items.Add("ISO 9613-1");
                this.Atten_Method.Items.Add("Evans & Bazley");
                this.Atten_Method.Items.Add("Vorlaender");
                this.Atten_Method.SelectedIndex = 0;

                this.PaLbl = new Label();
                this.PaLbl.Text = "Static Air Pressure (hPa)";
                this.Air_Pressure = new NumericStepper();
                this.Air_Pressure.MaxValue = 1500;
                this.Air_Pressure.MinValue = 50;
                this.Air_Pressure.Value = 1000;
                this.RhLbl = new Label();
                this.RhLbl.Text = "Relative Humidity(%)";
                this.Rel_Humidity = new NumericStepper();
                this.Rel_Humidity.MaxValue = 90;
                this.Rel_Humidity.MinValue = 10;
                this.Rel_Humidity.Value = 50;
                this.AirTemp = new Label();
                this.AirTemp.Text = "Air Temperature (C)";
                this.Air_Temp = new NumericStepper();
                this.Air_Temp.MaxValue = 80;
                this.Air_Temp.MinValue = -50;
                this.Air_Temp.Value = 20;

                EDL.AddRow(methodlbl, null, Atten_Method);
                EDL.AddRow(PaLbl, null, Air_Pressure);
                EDL.AddRow(RhLbl, null, Rel_Humidity);
                EDL.AddRow(AirTemp, null, Air_Temp);
                EDL.AddRow(EdgeFreq);
                this.Content = EDL;
            }

            public double StaticPressure_hPa
            {
                get
                {
                    return Air_Pressure.Value;
                }
            }

            public double StaticPressure_Pa
            {
                get
                {
                    return Air_Pressure.Value * 100;
                }
            }

            public double StaticPressure_kPa
            {
                get
                {
                    return Air_Pressure.Value / 10;
                }
            }

            public double RelHumidity
            {
                get { return Rel_Humidity.Value; }
            }

            public double Temp_Celsius
            {
                get { return Air_Temp.Value; }
            }
            public double Temp_Kelvin
            {
                get { return Air_Temp.Value + 273.15; }
            }

            public bool Edge_Frequency
            {
                get { return EdgeFreq.Checked.Value; }
            }

        }

        public class Color_Output_Control : DynamicLayout
        {
            private Label OctaveLabel;
            internal DropDown Octave;
            private Label ColorLabel;
            internal DropDown Color_Selection;
            private NumericStepper Param_Max;
            private Label Param1_4;
            private Label Param1_2;
            private Label Param3_4;
            private NumericStepper Param_Min;
            private CheckBox Discretize;
            ImageView Param_Scale;
            public Color_Output_Control(bool discretize_option = false, string label = "did not properly indicate what control was...", Control add_ctrl_1 = null)
            {
                Update += OnUpdate;
                this.Height = 300;
                this.Width = 200;
                this.Octave = new DropDown();
                this.OctaveLabel = new Label();
                this.ColorLabel = new Label();
                this.Color_Selection = new DropDown();
                this.Param1_4 = new Label();
                this.Param1_2 = new Label();
                this.Param_Min = new NumericStepper();
                this.Param_Max = new NumericStepper();
                this.Param3_4 = new Label();

                this.OctaveLabel.Text = "Octave Band Selection";
                this.Octave.Items.Add("62.5 Hz.");
                this.Octave.Items.Add("125 Hz.");
                this.Octave.Items.Add("250 Hz.");
                this.Octave.Items.Add("500 Hz.");
                this.Octave.Items.Add("1 kHz.");
                this.Octave.Items.Add("2 kHz.");
                this.Octave.Items.Add("4 kHz.");
                this.Octave.Items.Add("8 kHz.");
                this.Octave.Items.Add("Summation: All Octaves");
                this.Octave.SelectedIndex = 8;

                this.ColorLabel.Text = "Color Selection";
                this.Color_Selection.Items.Add("R-O-Y-G-B-I-V");
                this.Color_Selection.Items.Add("R-O-Y");
                this.Color_Selection.Items.Add("Y-G-B");
                this.Color_Selection.Items.Add("W-B");
                this.Color_Selection.Items.Add("R-B");
                this.Color_Selection.SelectedIndex = 0;
                this.Color_Selection.SelectedValueChanged += Color_Selection_SelectedIndexChanged;

                this.Discretize = new CheckBox();
                this.Discretize.Text = "Discretize";
                this.Discretize.Checked = false;

                this.Param1_4.Text = "00";
                this.Param1_2.Text = "00";
                this.Param3_4.Text = "00";
                this.Param_Min.DecimalPlaces = 1;
                this.Param_Max.DecimalPlaces = 1;
                this.Param_Max.MaxValue = 200;
                this.Param_Max.Value = 120;
                this.Param_Max.ValueChanged += Param_Max_ValueChanged;
                this.Param_Min.ValueChanged += Param_Max_ValueChanged;

                this.Param_Scale = new ImageView();
                Param_Scale.Height = 300;
                Param_Scale.Width = (Width / 5);

                scale = new Pach_Graphics.HSV_colorscale(300, Math.Max(Width / 5, 50), 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);

                //System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                //byte[] bb = (byte[])converter.ConvertTo(Scale.PIC, typeof(byte[]));
                Param_Scale.Image = Scale.PIC;

                StackLayout ControlCluster = new StackLayout();
                ControlCluster.Spacing = 12;
                if (add_ctrl_1 != null)
                {
                    Label addlbl = new Label();
                    addlbl.Text = label;
                    ControlCluster.Items.Add(addlbl);
                    ControlCluster.Items.Add(add_ctrl_1);
                }
                ControlCluster.Items.Add(OctaveLabel);
                ControlCluster.Items.Add(Octave);
                ControlCluster.Items.Add(ColorLabel);
                ControlCluster.Items.Add(Color_Selection);
                ControlCluster.Width = 250;

                if (discretize_option)
                {
                    Discretize.CheckedChanged += Color_Selection_SelectedIndexChanged;
                    ControlCluster.Items.Add(Discretize);
                }

                StackLayout Scale_Labels = new StackLayout();
                Scale_Labels.Items.Add(Param_Max);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param3_4);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param1_2);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param1_4);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param_Min);
                Scale_Labels.HorizontalContentAlignment = HorizontalAlignment.Right;
                Scale_Labels.AlignLabels = true;
                this.AddRow(ControlCluster, null, Scale_Labels, Param_Scale);

                On_Output_Changed += Param_Max_ValueChanged;
                On_Output_Changed(this, new System.EventArgs());
            }

            public Color_Output_Control(DynamicLayout add_ctrls)
            {
                Update += OnUpdate;
                this.Height = 300;
                this.Width = 200;
                this.Octave = new DropDown();
                this.OctaveLabel = new Label();
                this.ColorLabel = new Label();
                this.Color_Selection = new DropDown();
                this.Param1_4 = new Label();
                this.Param1_2 = new Label();
                this.Param_Min = new NumericStepper();
                this.Param_Max = new NumericStepper();
                this.Param3_4 = new Label();

                this.OctaveLabel.Text = "Octave Band Selection";
                this.Octave.Items.Add("62.5 Hz.");
                this.Octave.Items.Add("125 Hz.");
                this.Octave.Items.Add("250 Hz.");
                this.Octave.Items.Add("500 Hz.");
                this.Octave.Items.Add("1 kHz.");
                this.Octave.Items.Add("2 kHz.");
                this.Octave.Items.Add("4 kHz.");
                this.Octave.Items.Add("8 kHz.");
                this.Octave.Items.Add("Summation: All Octaves");
                this.Octave.SelectedIndex = 8;

                this.ColorLabel.Text = "Color Selection";
                this.Color_Selection.Items.Add("R-O-Y-G-B-I-V");
                this.Color_Selection.Items.Add("R-O-Y");
                this.Color_Selection.Items.Add("Y-G-B");
                this.Color_Selection.Items.Add("W-B");
                this.Color_Selection.Items.Add("R-B");
                this.Color_Selection.SelectedIndex = 0;
                this.Color_Selection.SelectedValueChanged += Color_Selection_SelectedIndexChanged;

                this.Discretize = new CheckBox();
                this.Discretize.Text = "Discretize";
                this.Discretize.Checked = false;

                this.Param1_4.Text = "00";
                this.Param1_2.Text = "00";
                this.Param3_4.Text = "00";
                this.Param_Min.DecimalPlaces = 1;
                this.Param_Max.DecimalPlaces = 1;
                this.Param_Max.MaxValue = 200;
                this.Param_Max.Value = 120;
                this.Param_Max.ValueChanged += Param_Max_ValueChanged;
                this.Param_Min.ValueChanged += Param_Max_ValueChanged;

                this.Param_Scale = new ImageView();
                Param_Scale.Height = 300;
                Param_Scale.Width = (Width / 5);

                scale = new Pach_Graphics.HSV_colorscale(300, Math.Max(Width / 5, 50), 0, 4.0 / 3.0, 1, 0, 1, 1, false, 12);

                //System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                //byte[] bb = (byte[])converter.ConvertTo(Scale.PIC, typeof(byte[]));
                Param_Scale.Image = Scale.PIC;

                StackLayout ControlCluster = new StackLayout();
                ControlCluster.Spacing = 12;

                ControlCluster.Items.Add(ColorLabel);
                ControlCluster.Items.Add(Color_Selection);
                ControlCluster.Items.Add(add_ctrls);
                ControlCluster.Width = 250;

                StackLayout Scale_Labels = new StackLayout();
                Scale_Labels.Items.Add(Param_Max);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param3_4);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param1_2);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param1_4);
                Scale_Labels.Items.Add(null);
                Scale_Labels.Items.Add(Param_Min);
                Scale_Labels.HorizontalContentAlignment = HorizontalAlignment.Right;
                Scale_Labels.AlignLabels = true;
                this.AddRow(ControlCluster, null, Scale_Labels, Param_Scale);

                On_Output_Changed += Param_Max_ValueChanged;
                On_Output_Changed(this, new System.EventArgs());
            }

            //protected override void OnSizeChanged(System.EventArgs e)
            //{
            //    base.OnSizeChanged(e);
            //    Width = this.Parent.Width;
            //    Height = 300;
            //    Invalidate();
            //}

            public EventHandler On_Output_Changed;
            public EventHandler Update;
            private void Param_Max_ValueChanged(object sender, System.EventArgs e)
            {
                this.Param3_4.Text = (Param_Max.Value - (Param_Max.Value - Param_Min.Value) / 4).ToString() + "---";
                this.Param1_2.Text = (Param_Max.Value - (Param_Max.Value - Param_Min.Value) / 2).ToString() + "---";
                this.Param1_4.Text = (Param_Min.Value + (Param_Max.Value - Param_Min.Value) / 4).ToString() + "---";
                Update(sender, EventArgs.Empty);
            }

            public Pach_Graphics.Colorscale scale;

            private void Color_Selection_SelectedIndexChanged(object sender, System.EventArgs e)
            {
                switch (this.Color_Selection.SelectedValue.ToString())
                {
                    case "R-O-Y-G-B-I-V":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "Y-G-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, Math.PI / 3.0, 2.0 / 3.0, 1, 0, 1, 0, Discretize.Checked.Value, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "R-O-Y":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 1.0 / 3.0, 1, 0, 1, 0, Discretize.Checked.Value, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "W-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 0, 0, 1, -1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    case "R-M-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 1, 0, 1, -1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                    default:
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, Math.PI / 2.0, 0, 0, 1, 1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = scale.PIC;
                        break;
                }
                On_Output_Changed(sender, EventArgs.Empty);
                Update(sender, e);
                //On_Output_Changed(this, EventArgs.Empty);
            }

            public void setscale(double min, double max)
            {
                Param_Max.Value = max;
                Param_Min.Value = min;

            }

            public double Max
            {
                get { return Param_Max.Value; }
            }

            public double Min
            {
                get { return Param_Min.Value; }
            }

            public int Freq_Band_ID
            {
                get { return Octave.SelectedIndex; }
            }

            public int Color_ID
            {
                get { return Color_Selection.SelectedIndex; }
            }

            private void OnUpdate(object sender, System.EventArgs e)
            {

            }

            public Colorscale Scale
            {
                get { return scale; }
            }

        }

        public partial class FreqSlider : Panel
        {
            bands bandwidth;
            System.Collections.Generic.List<Oct_Slider> sliders = new System.Collections.Generic.List<Oct_Slider>();
            public FreqSlider(bands precision, bool decibels = false)
            {
                buildSliders(precision, decibels);
            }

            private void buildSliders(bands precision, bool decibels = false)
            {
                sliders.Add(new Oct_Slider("    40.0 Hz: ", decibels));
                sliders.Add(new Oct_Slider("62.5 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    80 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    100 Hz: ", decibels));
                sliders.Add(new Oct_Slider("125 Hz: ",  decibels));
                sliders.Add(new Oct_Slider("    160 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    200 Hz: ", decibels));
                sliders.Add(new Oct_Slider("250 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    315 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    400 Hz: ", decibels));
                sliders.Add(new Oct_Slider("500 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    630 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    800 Hz: ", decibels));
                sliders.Add(new Oct_Slider("1 kHz: ", decibels));
                sliders.Add(new Oct_Slider("    1250 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    1600 Hz: ", decibels));
                sliders.Add(new Oct_Slider("2 kHz: ", decibels));
                sliders.Add(new Oct_Slider("    2500 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    3150 Hz: ", decibels));
                sliders.Add(new Oct_Slider("4 kHz: ", decibels));
                sliders.Add(new Oct_Slider("    5000 Hz: ", decibels));
                sliders.Add(new Oct_Slider("    6300 Hz: ", decibels));
                sliders.Add(new Oct_Slider("8 kHz: ", decibels));
                sliders.Add(new Oct_Slider("    10 kHz: ", decibels));
                sliders.Add(new Oct_Slider("Flatten", decibels));

                sliders[24].MouseUp += FlatAdjust;

                DynamicLayout sliderlayout = new DynamicLayout();
                foreach (Oct_Slider s in sliders) sliderlayout.AddRow(s);
                //sliderlayout.Size = new Size(8, 4);

                Content = sliderlayout;
                bandmode(precision);

                this.Invalidate();
            }

            public void bandmode(bands b)
            {
                bandwidth = b;
                bool mode = b == bands.Third_Octave;
                sliders[0].Visible = mode;
                sliders[2].Visible = mode;
                sliders[3].Visible = mode;
                sliders[5].Visible = mode;
                sliders[6].Visible = mode;
                sliders[8].Visible = mode;
                sliders[9].Visible = mode;
                sliders[11].Visible = mode;
                sliders[12].Visible = mode;
                sliders[14].Visible = mode;
                sliders[15].Visible = mode;
                sliders[17].Visible = mode;
                sliders[18].Visible = mode;
                sliders[20].Visible = mode;
                sliders[21].Visible = mode;
                sliders[23].Visible = mode;
                this.Invalidate();
            }

            public void populate(double[] values)
            {
                if (values.Length == 8 && sliders.Count != 8) buildSliders(bands.Octave);
                else if (values.Length == 24 && sliders.Count != 24) buildSliders(bands.Third_Octave);
                else if (values.Length != sliders.Count) throw new Exception("input to octave band sliders is not octave band or third-octave band, or is incomplete or in an unsupported format...");

                for (int i = 0; i < sliders.Count; i++)
                {
                    sliders[i].Value = values[i];
                }
            }

            public void Clear()
            {
                foreach (Oct_Slider slider in sliders)
                {
                    slider.Value = 1;
                }
            }

            public double[] Value
            {
                get
                {
                    double[] values;
                    if (bandwidth == bands.Third_Octave)
                    {
                        values = new double[sliders.Count];
                        for (int i = 0; i < sliders.Count; i++)
                        {
                            values[i] = sliders[i].Value;
                        }
                    }
                    else
                    {
                        values = new double[8];
                        values[0] = sliders[1].Value;
                        values[1] = sliders[4].Value;
                        values[2] = sliders[7].Value;
                        values[3] = sliders[10].Value;
                        values[4] = sliders[13].Value;
                        values[5] = sliders[16].Value;
                        values[6] = sliders[19].Value;
                        values[7] = sliders[22].Value;
                    }
                    return values;
                }
                set
                {
                    if (value.Length == 8)
                    {
                        bandmode(bands.Octave);
                        sliders[1].Value = value[0];
                        sliders[4].Value = value[1];
                        sliders[7].Value = value[2];
                        sliders[10].Value = value[3];
                        sliders[13].Value = value[4];
                        sliders[16].Value = value[5];
                        sliders[19].Value = value[6];
                        sliders[22].Value = value[7];
                    }
                    else if (value.Length == 24)
                    {
                        bandmode(bands.Third_Octave);
                        for (int i = 0; i < sliders.Count; i++) sliders[i].Value = value[i];
                    }
                    else
                    { 
                        throw new Exception("Incorrect number of bands...");
                    }
                }
            }

            public void FlatAdjust(object sender, EventArgs e)
            {
                foreach (Oct_Slider s in sliders) s.Value = sliders[24].Value;
            }

            public enum bands
            {
                Octave,
                Third_Octave
            }

            public void resize(int width)
            {
                foreach (Oct_Slider slider in sliders)
                {
                    slider.resize(width);
                }
            }
        }

        public class Oct_Slider : TableRow
        {
            Label l = new Label();
            Slider oct_Slider = new Slider();
            Label oct_out = new Label();
            Label units = new Label();
            public EventHandler MouseUp;
            public Oct_Slider(string text, bool decibels = false)
            : base()
            {
                this.oct_Slider.TickFrequency = 100;
                this.oct_Slider.Value = 1;
                this.oct_Slider.Size = new Size(-1, 20);
                this.oct_Slider.MaxValue = 1000;
                this.oct_Slider.ValueChanged += UpdateValue;
                if (!decibels) 
                {
                    units.Text = "%";
                    units.Width = 15;
                }
                else 
                {
                    units.Text = "decibels";
                    units.Width = 50;
                }

                l.Text = text;
                l.Width = 65;
                oct_out.Width = 35;
                this.Cells.Add(l);
                this.Cells.Add(oct_Slider);
                this.Cells.Add(oct_out);
                this.Cells.Add(units);
                this.Cells[1].ScaleWidth = true;
            }
            private void UpdateValue(object sender, System.EventArgs e)
            {
                oct_out.Text = ((double)oct_Slider.Value / 10).ToString();
                if (this.MouseUp != null) this.MouseUp(sender, e);
            }

            public bool Visible
            {
                get { return oct_Slider.Visible; }
                set 
                {
                    l.Visible = value;
                    oct_Slider.Visible = value;
                    oct_out.Visible = value;
                    units.Visible = value;
                }
            }

            public double Value
            {
                get { return (double)oct_Slider.Value / 10; }
                set { oct_Slider.Value = (int)(value * 10); }
            }

            public void resize(int width)
            {
                oct_Slider.Width = width - 75;
            }
        }
    }
}
