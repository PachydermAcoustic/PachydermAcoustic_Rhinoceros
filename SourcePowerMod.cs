using Eto.Forms;
using System;

namespace Pachyderm_Acoustic
{
    public partial class SourcePowerMod : Eto.Forms.Dialog
    {
        public double[] Power;
        public bool accept = false;

        public SourcePowerMod(double[] initpower)
        {

            DynamicLayout layout = new DynamicLayout();
            Label OCT = new Eto.Forms.Label();
            Label SPL = new Eto.Forms.Label();
            Label SWL = new Eto.Forms.Label();
            SPL.Text = "SPL @ 1m.";
            SPL.TextAlignment = TextAlignment.Center;
            SWL.Text = "SWL";
            SWL.TextAlignment = TextAlignment.Center;
            OCT.Text = "Octave";
            OCT.TextAlignment = TextAlignment.Center;

            layout.AddRow(OCT, null, SWL, SPL);

            Label H8k = new Eto.Forms.Label();
            Label H4k = new Eto.Forms.Label();
            Label H2k = new Eto.Forms.Label();
            Label H1k = new Eto.Forms.Label();
            Label H500 = new Eto.Forms.Label();
            Label H250 = new Eto.Forms.Label();
            Label H125 = new Eto.Forms.Label();
            Label H63 = new Eto.Forms.Label();

            this.SWL0 = new Eto.Forms.NumericStepper();
            this.SWL1 = new Eto.Forms.NumericStepper();
            this.SWL2 = new Eto.Forms.NumericStepper();
            this.SWL3 = new Eto.Forms.NumericStepper();
            this.SWL4 = new Eto.Forms.NumericStepper();
            this.SWL5 = new Eto.Forms.NumericStepper();
            this.SWL6 = new Eto.Forms.NumericStepper();
            this.SWL7 = new Eto.Forms.NumericStepper();

            this.SPL1 = new Eto.Forms.Label();
            this.SPL0 = new Eto.Forms.Label();
            this.SPL2 = new Eto.Forms.Label();
            this.SPL3 = new Eto.Forms.Label();
            this.SPL4 = new Eto.Forms.Label();
            this.SPL5 = new Eto.Forms.Label();
            this.SPL6 = new Eto.Forms.Label();
            this.SPL7 = new Eto.Forms.Label();
            this.swl63 = new Eto.Forms.Slider();
            this.swl125 = new Eto.Forms.Slider();
            this.swl250 = new Eto.Forms.Slider();
            this.swl500 = new Eto.Forms.Slider();
            this.swl1k = new Eto.Forms.Slider();
            this.swl2k = new Eto.Forms.Slider();
            this.swl4k = new Eto.Forms.Slider();
            this.swl8k = new Eto.Forms.Slider();

            this.OK = new Eto.Forms.Button();
            this.Cancel = new Eto.Forms.Button();

            this.DefaultButton = OK;
            this.AbortButton = Cancel;

            H63.Text = "63 Hz.";
            H63.TextAlignment = Eto.Forms.TextAlignment.Center;
            this.SWL0.DecimalPlaces = 2;
            this.SWL0.MaxValue = 200;
            this.SWL0.MinValue = 0;
            this.SWL0.Value = 120;
            this.SWL0.ValueChanged += this.swl0_updown;
            H125.Text = "125 Hz.";
            H125.TextAlignment = Eto.Forms.TextAlignment.Center;
            this.SWL1.DecimalPlaces = 2;
            this.SWL1.MaxValue = 200;
            this.SWL1.MinValue = 0;
            this.SWL1.Value = 120;
            this.SWL1.ValueChanged += this.swl1_updown;
            H250.Text = "250 Hz.";
            H250.TextAlignment = Eto.Forms.TextAlignment.Center;
            this.SWL2.DecimalPlaces = 2;
            this.SWL2.MaxValue = 200;
            this.SWL2.MinValue = 0;
            this.SWL2.Value = 120;
            this.SWL2.ValueChanged += this.swl2_updown;
            H500.Text = "500 Hz.";
            H500.TextAlignment = Eto.Forms.TextAlignment.Center;
            this.SWL3.DecimalPlaces = 2;
            this.SWL3.MaxValue = 200;
            this.SWL3.MinValue = 0;
            this.SWL3.Value = 120;
            this.SWL3.ValueChanged += this.swl3_updown;
            H1k.Text = "1 kHz.";
            H1k.TextAlignment = TextAlignment.Center;
            this.SWL4.DecimalPlaces = 2;
            this.SWL4.MaxValue = 200;
            this.SWL4.MinValue = 0;
            this.SWL4.Value = 120;
            this.SWL4.ValueChanged += this.swl4_updown;
            H2k.Text = "2 kHz.";
            H2k.TextAlignment = TextAlignment.Center;
            this.SWL5.DecimalPlaces = 2;
            this.SWL5.MaxValue = 200;
            this.SWL5.MinValue = 0;
            this.SWL5.Value = 120;
            this.SWL5.ValueChanged += this.swl5_updown;
            H4k.Text = "4 kHz.";
            H4k.TextAlignment = TextAlignment.Center;
            this.SWL6.DecimalPlaces = 2;
            this.SWL6.MaxValue = 200;
            this.SWL6.MinValue = 0;
            this.SWL6.Value = 120;
            this.SWL6.ValueChanged += this.swl6_updown;
            H8k.Text = "8 kHz.";
            H8k.TextAlignment = TextAlignment.Center;
            this.SWL7.DecimalPlaces = 2;
            this.SWL7.MaxValue = 200;
            this.SWL7.MinValue = 0;
            this.SWL7.Value = 120;
            this.SWL7.ValueChanged += this.swl7_updown;


            this.SPL0.Text = "109.00";
            this.SPL0.TextAlignment = TextAlignment.Center;
            this.SPL1.Text = "109.00";
            this.SPL1.TextAlignment = TextAlignment.Center;
            this.SPL2.Text = "109.00";
            this.SPL2.TextAlignment = TextAlignment.Center;
            this.SPL3.Text = "109.00";
            this.SPL3.TextAlignment = TextAlignment.Center;
            this.SPL4.Text = "109.00";
            this.SPL4.TextAlignment = TextAlignment.Center;
            this.SPL5.Text = "109.00";
            this.SPL5.TextAlignment = TextAlignment.Center;
            this.SPL6.Text = "109.00";
            this.SPL6.TextAlignment = TextAlignment.Center;
            this.SPL7.Text = "109.00";
            this.SPL7.TextAlignment = TextAlignment.Center;

            layout.AddRow(H63, swl63, SWL0, SPL0);
            layout.AddRow(H125, swl125, SWL1, SPL1);
            layout.AddRow(H250, swl250, SWL2, SPL2);
            layout.AddRow(H500, swl500, SWL3, SPL3);
            layout.AddRow(H1k, swl1k, SWL4, SPL4);
            layout.AddRow(H2k, swl2k, SWL5, SPL5);
            layout.AddRow(H4k, swl4k, SWL6, SPL6);
            layout.AddRow(H8k, swl8k, SWL7, SPL7);

            this.Cancel.Text = "Cancel";
            this.Cancel.Click += this.Cancel_Click;
            this.OK.Text = "Ok";
            this.OK.Click += this.OK_Click;

            layout.AddRow(null, null, OK, Cancel);
            this.Content = layout;

            if (initpower == null || initpower.Length != 8) initpower = new double[8] { 120, 120, 120, 120, 120, 120, 120, 120 };
            SWL0.Value = initpower[0];
            SWL1.Value = initpower[1];
            SWL2.Value = initpower[2];
            SWL3.Value = initpower[3];
            SWL4.Value = initpower[4];
            SWL5.Value = initpower[5];
            SWL6.Value = initpower[6];
            SWL7.Value = initpower[7];
        }

        protected override void OnClosed(EventArgs e)
        {
            Power = new double[8];
            Power[0] = (double)SWL0.Value;
            Power[1] = (double)SWL1.Value;
            Power[2] = (double)SWL2.Value;
            Power[3] = (double)SWL3.Value;
            Power[4] = (double)SWL4.Value;
            Power[5] = (double)SWL5.Value;
            Power[6] = (double)SWL6.Value;
            Power[7] = (double)SWL7.Value;
            base.OnClosed(e);
        }

        private void swl63_Scroll(object sender, EventArgs e)
        {
            SWL0.Value = swl63.Value;
        }
        private void swl125_Scroll(object sender, EventArgs e)
        {
            SWL1.Value = swl125.Value;
        }
        private void swl250_Scroll(object sender, EventArgs e)
        {
            SWL2.Value = swl250.Value;
        }
        private void swl500_Scroll(object sender, EventArgs e)
        {
            SWL3.Value = swl500.Value;
        }
        private void swl1k_Scroll(object sender, EventArgs e)
        {
            SWL4.Value = swl1k.Value;
        }
        private void swl2k_Scroll(object sender, EventArgs e)
        {
            SWL5.Value = swl2k.Value;
        }
        private void swl4k_Scroll(object sender, EventArgs e)
        {
            SWL6.Value = swl4k.Value;
        }
        private void swl8k_Scroll(object sender, EventArgs e)
        {
            SWL7.Value = swl8k.Value;
        }

        private void swl0_updown(object sender, EventArgs e)
        {
            swl63.Value = (int)SWL0.Value;
            SPL0.Text = (Math.Round(SWL0.Value, 2) - 11).ToString();
        }
        private void swl1_updown(object sender, EventArgs e)
        {
            swl125.Value = (int)SWL1.Value;
            SPL1.Text = (Math.Round(SWL1.Value, 2) - 11).ToString();
        }
        private void swl2_updown(object sender, EventArgs e)
        {
            swl250.Value = (int)SWL2.Value;
            SPL2.Text = (Math.Round(SWL2.Value, 2) - 11).ToString();
        }
        private void swl3_updown(object sender, EventArgs e)
        {
            swl500.Value = (int)SWL3.Value;
            SPL3.Text = (Math.Round(SWL3.Value, 2) - 11).ToString();
        }
        private void swl4_updown(object sender, EventArgs e)
        {
            swl1k.Value = (int)SWL4.Value;
            SPL4.Text = (Math.Round(SWL4.Value, 2) - 11).ToString();
        }
        private void swl5_updown(object sender, EventArgs e)
        {
            swl2k.Value = (int)SWL5.Value;
            SPL5.Text = (Math.Round(SWL5.Value, 2) - 11).ToString();
        }
        private void swl6_updown(object sender, EventArgs e)
        {
            swl4k.Value = (int)SWL6.Value;
            SPL6.Text = (Math.Round(SWL6.Value, 2) - 11).ToString();
        }
        private void swl7_updown(object sender, EventArgs e)
        {
            swl8k.Value = (int)SWL7.Value;
            SPL7.Text = (Math.Round(SWL7.Value, 2) - 11).ToString();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            accept = true;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            accept = false;
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SWL0.Dispose();
            SWL1.Dispose();
            SWL2.Dispose();
            SWL3.Dispose();
            SWL4.Dispose();
            SWL5.Dispose();
            SWL6.Dispose();
            SWL7.Dispose();
            SPL1.Dispose();
            SPL0.Dispose();
            SPL2.Dispose();
            SPL3.Dispose();
            SPL4.Dispose();
            SPL5.Dispose();
            SPL6.Dispose();
            SPL7.Dispose();
            swl63.Dispose();
            swl125.Dispose();
            swl250.Dispose();
            swl500.Dispose();
            swl1k.Dispose();
            swl2k.Dispose();
            swl4k.Dispose();
            swl8k.Dispose();
            Cancel.Dispose();
            OK.Dispose();
        }

        private Eto.Forms.NumericStepper SWL0;
        private Eto.Forms.NumericStepper SWL1;
        private Eto.Forms.NumericStepper SWL2;
        private Eto.Forms.NumericStepper SWL3;
        private Eto.Forms.NumericStepper SWL4;
        private Eto.Forms.NumericStepper SWL5;
        private Eto.Forms.NumericStepper SWL6;
        private Eto.Forms.NumericStepper SWL7;
        private Eto.Forms.Label SPL1;
        private Eto.Forms.Label SPL0;
        private Eto.Forms.Label SPL2;
        private Eto.Forms.Label SPL3;
        private Eto.Forms.Label SPL4;
        private Eto.Forms.Label SPL5;
        private Eto.Forms.Label SPL6;
        private Eto.Forms.Label SPL7;
        private Eto.Forms.Slider swl63;
        private Eto.Forms.Slider swl125;
        private Eto.Forms.Slider swl250;
        private Eto.Forms.Slider swl500;
        private Eto.Forms.Slider swl1k;
        private Eto.Forms.Slider swl2k;
        private Eto.Forms.Slider swl4k;
        private Eto.Forms.Slider swl8k;
        private Eto.Forms.Button Cancel;
        private Eto.Forms.Button OK;
    }
}
