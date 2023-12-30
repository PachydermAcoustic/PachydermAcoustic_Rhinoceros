using Eto.Forms;
using Pachyderm_Acoustic.Environment;
using Pachyderm_Acoustic.Pach_Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;

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
            PachMapReceiver[] Map;
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

            public void Populate(Pachyderm_Acoustic.Environment.Source[] Sourceobjs, PachMapReceiver[] SimResults)
            {
                Map = SimResults;
                SrcBoxes.Clear();
                DynamicLayout SrcLayout = new DynamicLayout();

                foreach (Source s in Sourceobjs)
                {
                    CheckBox src = new CheckBox();
                    src.Text = String.Format("S{0}-", s.Source_ID()) + s.Type();
                    src.MouseUp += update_proxy;
                    SrcBoxes.Add(src);
                    SrcLayout.AddRow(src);
                }
                this.Content = SrcLayout;
            }

            private void update_proxy(object sender, EventArgs e)
            {
                Update(sender, e);
            }

            public void Populate(PachMapReceiver[] SimResults)
            {
                Map = SimResults;
                SrcBoxes.Clear();
                DynamicLayout SrcLayout = new DynamicLayout();

                for (int i = 0; i < SimResults.Length; i++)
                {
                    CheckBox src = new CheckBox();
                    src.MouseUp += update_proxy;
                    src.Text = String.Format("S{0}-", i) + Map[i].SrcType;
                    SrcLayout.AddRow(src);
                }
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

            //public void Populate(List<string> SourceNames, PachMapReceiver[] SimResults)
            //{
            //    Map = SimResults;
            //    Srcs = SourceNames;
            //    Sources.Menu.Items.Clear();
            //    foreach (string name in SourceNames)
            //    {
            //        CheckMenuItem src = new CheckMenuItem();
            //        src.Text = name;
            //        Sources.Menu.Items.Add(src);
            //    }

            //    Sources.Menu.Items.Add(PowerMod);
            //    Sources.Menu.Items.Add(DelayMod);
            //}

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

                double t = Map[SrcID[0]].delay_ms;
                Rhino.Input.RhinoGet.GetNumber("Enter the delay to assign to selected source object(s)...", false, ref t, 0, 200);

                foreach (int id in SrcID)
                {
                    Map[id].delay_ms = t;
                }

                Update(this, EventArgs.Empty);
            }

            private void ModifyPower_Click(object sender, System.EventArgs e)
            {
                List<int> SrcID = SelectedSources();

                if (SrcID.Count < 1) return;
                Pachyderm_Acoustic.SourcePowerMod mod = new SourcePowerMod(Map[SrcID[0]].SWL);
                mod.ShowDialog();
                if (mod.accept)
                {
                    foreach (int i in SrcID)
                    {
                        double[] factor = Map[i].PowerModFactor(mod.Power);
                        Map[i].Set_Power(factor);
                    }
                }
                Update(this, EventArgs.Empty);
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
                this.Atten_Method.Items.Add("ISO 9613-1 (Outdoor Attenuation)");
                this.Atten_Method.Items.Add("Evans & Bazley (Indoor Attenuation)");
                this.Atten_Method.Items.Add("Common values (Vorlaender)");
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

                System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                byte[] bb = (byte[])converter.ConvertTo(Scale.PIC, typeof(byte[]));
                Param_Scale.Image = new Eto.Drawing.Bitmap(bb);

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

                System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                byte[] bb = (byte[])converter.ConvertTo(Scale.PIC, typeof(byte[]));
                Param_Scale.Image = new Eto.Drawing.Bitmap(bb);

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
                Update(this, EventArgs.Empty);
            }

            public Pach_Graphics.colorscale scale;

            private void Color_Selection_SelectedIndexChanged(object sender, System.EventArgs e)
            {
                switch (this.Color_Selection.SelectedValue.ToString())
                {
                    case "R-O-Y-G-B-I-V":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 4.0 / 3.0, 1, 0, 1, 1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = Rhino.UI.EtoExtensions.ToEto(scale.PIC);
                        break;
                    case "Y-G-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, Math.PI / 3.0, 2.0 / 3.0, 1, 0, 1, 0, Discretize.Checked.Value, 12);
                        Param_Scale.Image = Rhino.UI.EtoExtensions.ToEto(scale.PIC);
                        break;
                    case "R-O-Y":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 1.0 / 3.0, 1, 0, 1, 0, Discretize.Checked.Value, 12);
                        Param_Scale.Image = Rhino.UI.EtoExtensions.ToEto(scale.PIC);
                        break;
                    case "W-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 0, 0, 1, -1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = Rhino.UI.EtoExtensions.ToEto(scale.PIC);
                        break;
                    case "R-M-B":
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, 0, 1, 0, 1, -1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = Rhino.UI.EtoExtensions.ToEto(scale.PIC);
                        break;
                    default:
                        scale = new Pach_Graphics.HSV_colorscale(Param_Scale.Height, Param_Scale.Width, 0, Math.PI / 2.0, 0, 0, 1, 1, Discretize.Checked.Value, 12);
                        Param_Scale.Image = Rhino.UI.EtoExtensions.ToEto(scale.PIC);
                        break;
                }
                On_Output_Changed(this, EventArgs.Empty);
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

            public colorscale Scale
            {
                get { return scale; }
            }

        }

        public partial class FreqSlider : Panel
        {
            System.Collections.Generic.List<Oct_Slider> sliders = new System.Collections.Generic.List<Oct_Slider>();
            int _width = 0;
            public FreqSlider(bands precision, int width)
            {
                _width = width;
                buildSliders(precision, width);
            }

            private void buildSliders(bands precision, int width)
            {
                if (precision == bands.Octave)
                {
                    sliders.Add(new Oct_Slider("62.5 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("125 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("250 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("500 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("1 kHz: ", this.Width));
                    sliders.Add(new Oct_Slider("2 kHz: ", this.Width));
                    sliders.Add(new Oct_Slider("4 kHz: ", this.Width));
                    sliders.Add(new Oct_Slider("8 kHz: ", this.Width));
                }
                else
                {
                    sliders.Add(new Oct_Slider("    40.0 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("62.5 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    80 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    100 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("125 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    160 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    200 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("250 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    315 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    400 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("500 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    630 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    800 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("1 kHz: ", this.Width));
                    sliders.Add(new Oct_Slider("    1250 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    1600 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("2 kHz: ", this.Width));
                    sliders.Add(new Oct_Slider("    2500 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    3150 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("4 kHz: ", this.Width));
                    sliders.Add(new Oct_Slider("    5000 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("    6300 Hz: ", this.Width));
                    sliders.Add(new Oct_Slider("8 kHz: ", this.Width));
                    sliders.Add(new Oct_Slider("    10 kHz: ", this.Width));
                }
                Content = new TableLayout(sliders);
                this.Invalidate();
            }

            public void populate(double[] values)
            {
                if (values.Length == 8 && sliders.Count != 8) buildSliders(bands.Octave, this.Width);
                else if (values.Length == 24 && sliders.Count != 24) buildSliders(bands.Third_Octave, this.Width);
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
                    double[] values = new double[sliders.Count];
                    for (int i = 0; i < sliders.Count; i++)
                    {
                        values[i] = sliders[i].Value;
                    }
                    return values;
                }
                set
                {
                    if (value.Length == 8) { if (sliders.Count != 8) buildSliders(bands.Octave, this.Width); }
                    else if (value.Length == 24) { if (sliders.Count != 24) buildSliders(bands.Octave, this.Width); }
                    else throw new Exception("Material properties format not recognized...");

                    for (int i = 0; i < sliders.Count; i++) sliders[i].Value = value[i];
                }
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
            Slider oct_Slider = new Slider();
            Label oct_out = new Label();

            public Oct_Slider(string text, int width)
            : base()
            {
                Label l = new Label();
                this.oct_Slider.TickFrequency = 100;
                this.oct_Slider.Value = 1;
                this.oct_Slider.Size = new Size(400, 20);
                this.oct_Slider.MaxValue = 1000;
                this.oct_Slider.ValueChanged += UpdateValue;

                l.Text = text;
                this.Cells.Add(l);
                this.Cells.Add(oct_Slider);
                this.Cells.Add(oct_out);
            }
            private void UpdateValue(object sender, System.EventArgs e)
            {
                oct_out.Text = ((double)oct_Slider.Value / 10).ToString();
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
