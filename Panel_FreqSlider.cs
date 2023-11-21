using System;
using Eto.Forms;
using Eto.Drawing;
using Rhino.UI;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;

namespace Pachyderm_Acoustic
{
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

            for(int i = 0; i < sliders.Count; i++) 
            {
                sliders[i].Value = values[i];
            }
        }

        public void Clear()
        {
            foreach(Oct_Slider slider in sliders)
            {
                slider.Value = 1;
            }
        }

        public double[] Value
        {
            get 
            {
                double[] values = new double[sliders.Count];
                for(int i = 0; i < sliders.Count; i++)
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

    public class Oct_Slider: TableRow
    {
        Slider oct_Slider = new Slider();
        Label oct_out = new Label();

        public Oct_Slider(string text, int width)
        :base()
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
            get { return (double) oct_Slider.Value / 10; }
            set { oct_Slider.Value = (int)(value * 10); }
        }

        public void resize (int width)
        {
            oct_Slider.Width = width - 75;
        }
    }
}
