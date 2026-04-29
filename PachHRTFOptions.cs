using System;
using System.IO;
using Eto.Drawing;
using Eto.Forms;

namespace Pachyderm_Acoustic.UI
{
    public sealed class HrtfDialogResult
    {
        public string HrtfPath { get; set; }
        public Pachyderm_Acoustic.Audio.HRTF Hrtf { get; set; }

        public HrtfCompensationOptions Compensation { get; set; }
        public Pachyderm_Acoustic.Audio.SystemResponseCompensation.SystemCompensationSettings CompensationPrimitives { get; set; }
    }

    public sealed class HrtfCompensationOptions
    {
        public bool InputIsDTF { get; set; }
        public RhinoSystemCompSettings.EQType SelectedEQ { get; set; }

        public string MeasurementReferencePath { get; set; }
        public bool IsCalibrated { get; set; }

        public double? SmoothingOct { get; set; }
        public double? MaxBoostDb { get; set; }
        public double? LowFreqHz { get; set; }
        public bool MinPhase { get; set; }
        public int FreeFieldIncidence { get; set; }

        public Pachyderm_Acoustic.Audio.SystemResponseCompensation.SystemCompensationSettings ToPrimitiveSettings()
        {
            var primitive = new Pachyderm_Acoustic.Audio.SystemResponseCompensation.SystemCompensationSettings();

            if (InputIsDTF)
            {
                primitive.SelectedEQ = (int)RhinoSystemCompSettings.EQType.None;
                primitive.SmoothingOct = null;
                primitive.MaxBoostDb = null;
                primitive.LowFreqHz = null;
                primitive.MinPhase = false;
                primitive.IsCalibrated = false;
                primitive.TFReference = null;
                primitive.FreeFieldIncidence = 0;
                return primitive;
            }

            primitive.SelectedEQ = (int)SelectedEQ;
            primitive.SmoothingOct = SmoothingOct;
            primitive.MaxBoostDb = MaxBoostDb;
            primitive.LowFreqHz = LowFreqHz;
            primitive.MinPhase = MinPhase;
            primitive.IsCalibrated = IsCalibrated;
            primitive.FreeFieldIncidence = FreeFieldIncidence;

            if (SelectedEQ == RhinoSystemCompSettings.EQType.Measurement)
            {
                if (string.IsNullOrWhiteSpace(MeasurementReferencePath))
                {
                    throw new InvalidOperationException("Please select a transfer function WAV for measurement equalisation.");
                }

                if (!File.Exists(MeasurementReferencePath))
                {
                    throw new FileNotFoundException("Transfer function WAV not found.", MeasurementReferencePath);
                }

                int tfFs;
                double[][] tfReference = Pachyderm_Acoustic.Audio.Pach_SP.Wave.ReadtoDouble(MeasurementReferencePath, true, out tfFs);

                if (tfReference == null || tfReference.Length == 0 || tfReference[0] == null || tfReference[0].Length == 0)
                {
                    throw new InvalidOperationException("Unable to read the transfer function WAV.");
                }

                if (tfReference.Length > 1)
                {
                    Rhino.RhinoApp.WriteLine("Warning: transfer function WAV contains multiple channels. Channel 0 will be used.");
                }

                primitive.TFReference = tfReference[0];
            }
            else
            {
                primitive.TFReference = null;
            }

            return primitive;
        }
    }

    public sealed class PachHrtfDialog : Dialog<bool>
    {
        private TextBox _sofaPath;
        private Button _browseSofaButton;

        private DropDown _inputTypeDrop;
        private DropDown _eqTypeDrop;
        private Label _eqNote;

        private Panel _measurementPanel;
        private Panel _freeFieldPanel;
        private Panel _diffuseFieldPanel;

        private TextBox _measurementRefPath;
        private Button _browseMeasurementRefButton;
        private CheckBox _measurementIsCalibrated;
        private CheckBox _measurementUseSmoothing;
        private NumericStepper _measurementSmoothing;

        private NumericStepper _freeFieldIncidence;
        private CheckBox _freeFieldUseMaxBoost;
        private NumericStepper _freeFieldMaxBoost;
        private CheckBox _freeFieldUseLowFreq;
        private NumericStepper _freeFieldLowFreq;
        private CheckBox _freeFieldUseSmoothing;
        private NumericStepper _freeFieldSmoothing;

        private CheckBox _diffuseUseMaxBoost;
        private NumericStepper _diffuseMaxBoost;
        private CheckBox _diffuseUseLowFreq;
        private NumericStepper _diffuseLowFreq;
        private CheckBox _diffuseMinPhase;
        private CheckBox _diffuseUseSmoothing;
        private NumericStepper _diffuseSmoothing;

        private Label _status;

        public HrtfDialogResult ResultData { get; private set; }

        public PachHrtfDialog(
            string initialSofaPath = null,
            HrtfCompensationOptions initialCompensation = null)
        {
            Title = "HRTF / Equalisation";
            ClientSize = new Size(720, 640);
            Padding = 12;
            Resizable = true;

            BuildControls(initialSofaPath, initialCompensation ?? CreateDefaultCompensation());

            DefaultButton = new Button { Text = "OK" };
            DefaultButton.Click += delegate { OnAccept(); };

            AbortButton = new Button { Text = "Cancel" };
            AbortButton.Click += delegate { Close(false); };

            var layout = new DynamicLayout();
            layout.Spacing = new Size(8, 8);

            layout.AddRow(BuildSofaGroup());
            layout.AddRow(BuildCompensationGroup());

            _status = new Label { Text = string.Empty };
            layout.AddRow(_status);
            layout.Add(null, true);

            var buttons = new DynamicLayout();
            buttons.Spacing = new Size(8, 8);
            buttons.AddRow(null, DefaultButton, AbortButton);
            layout.AddRow(buttons);

            Content = new Scrollable
            {
                Border = BorderType.None,
                Content = layout
            };

            UpdateUiState();
        }

        private static HrtfCompensationOptions CreateDefaultCompensation()
        {
            return new HrtfCompensationOptions
            {
                InputIsDTF = false,
                SelectedEQ = RhinoSystemCompSettings.EQType.None,
                FreeFieldIncidence = 30
            };
        }

        private void BuildControls(string initialSofaPath, HrtfCompensationOptions c)
        {
            _sofaPath = new TextBox { Text = initialSofaPath ?? string.Empty };
            _browseSofaButton = new Button { Text = "Browse..." };
            _browseSofaButton.Click += delegate { BrowseSofa(); };

            _inputTypeDrop = new DropDown();
            _inputTypeDrop.Items.Add("Full HRTF");
            _inputTypeDrop.Items.Add("Directional-only / DTF");
            _inputTypeDrop.SelectedIndex = c.InputIsDTF ? 1 : 0;
            _inputTypeDrop.SelectedIndexChanged += delegate { UpdateUiState(); };

            _eqTypeDrop = new DropDown();
            _eqTypeDrop.Items.Add("None");
            _eqTypeDrop.Items.Add("Measurement equalisation");
            _eqTypeDrop.Items.Add("Free-field equalisation");
            _eqTypeDrop.Items.Add("Diffuse-field equalisation");
            _eqTypeDrop.SelectedIndex = (int)c.SelectedEQ;
            _eqTypeDrop.SelectedIndexChanged += delegate { UpdateUiState(); };

            _eqNote = new Label { Text = string.Empty };

            BuildMeasurementControls(c);
            BuildFreeFieldControls(c);
            BuildDiffuseFieldControls(c);
        }

        private void BuildMeasurementControls(HrtfCompensationOptions c)
        {
            _measurementRefPath = new TextBox { Text = c.MeasurementReferencePath ?? string.Empty };
            _browseMeasurementRefButton = new Button { Text = "Browse..." };
            _browseMeasurementRefButton.Click += delegate { BrowseMeasurementReference(); };

            _measurementIsCalibrated = new CheckBox
            {
                Text = "Transfer function magnitude is calibrated",
                Checked = c.IsCalibrated
            };

            _measurementUseSmoothing = new CheckBox
            {
                Text = "Enable smoothing",
                Checked = c.SelectedEQ == RhinoSystemCompSettings.EQType.Measurement && c.SmoothingOct.HasValue
            };
            _measurementUseSmoothing.CheckedChanged += delegate { UpdateUiState(); };

            _measurementSmoothing = NewStepper(0.05, 2.0, 0.05, 2, c.SmoothingOct ?? 0.33);

            var layout = new DynamicLayout();
            layout.Padding = new Padding(8);
            layout.Spacing = new Size(6, 6);
            layout.AddRow(new Label { Text = "Reference TF WAV", Width = 180 }, _measurementRefPath, _browseMeasurementRefButton);
            layout.AddRow(_measurementIsCalibrated, null);
            layout.AddRow(_measurementUseSmoothing, null);
            layout.AddRow(new Label { Text = "Smoothing (octaves)", Width = 180 }, _measurementSmoothing);

            _measurementPanel = new Panel { Content = layout };
        }

        private void BuildFreeFieldControls(HrtfCompensationOptions c)
        {
            _freeFieldIncidence = NewStepper(0, 360, 1, 0, c.FreeFieldIncidence == 0 ? 30 : c.FreeFieldIncidence);

            _freeFieldUseMaxBoost = new CheckBox
            {
                Text = "Limit max boost",
                Checked = c.SelectedEQ == RhinoSystemCompSettings.EQType.FreeField && c.MaxBoostDb.HasValue
            };
            _freeFieldUseMaxBoost.CheckedChanged += delegate { UpdateUiState(); };
            _freeFieldMaxBoost = NewStepper(0, 24, 0.5, 1, c.MaxBoostDb ?? 6);

            _freeFieldUseLowFreq = new CheckBox
            {
                Text = "Apply low-frequency cutoff",
                Checked = c.SelectedEQ == RhinoSystemCompSettings.EQType.FreeField && c.LowFreqHz.HasValue
            };
            _freeFieldUseLowFreq.CheckedChanged += delegate { UpdateUiState(); };
            _freeFieldLowFreq = NewStepper(20, 20000, 10, 0, c.LowFreqHz ?? 60);

            _freeFieldUseSmoothing = new CheckBox
            {
                Text = "Enable smoothing",
                Checked = c.SelectedEQ == RhinoSystemCompSettings.EQType.FreeField && c.SmoothingOct.HasValue
            };
            _freeFieldUseSmoothing.CheckedChanged += delegate { UpdateUiState(); };
            _freeFieldSmoothing = NewStepper(0.05, 2.0, 0.05, 2, c.SmoothingOct ?? 0.33);

            var layout = new DynamicLayout();
            layout.Padding = new Padding(8);
            layout.Spacing = new Size(6, 6);
            layout.AddRow(new Label { Text = "Free-field incidence (deg)", Width = 180 }, _freeFieldIncidence);
            layout.AddRow(_freeFieldUseMaxBoost, _freeFieldMaxBoost, null);
            layout.AddRow(_freeFieldUseLowFreq, _freeFieldLowFreq, null);
            layout.AddRow(_freeFieldUseSmoothing, null);
            layout.AddRow(new Label { Text = "Smoothing (octaves)", Width = 180 }, _freeFieldSmoothing);

            _freeFieldPanel = new Panel { Content = layout };
        }

        private void BuildDiffuseFieldControls(HrtfCompensationOptions c)
        {
            _diffuseUseMaxBoost = new CheckBox
            {
                Text = "Limit max boost",
                Checked = c.SelectedEQ == RhinoSystemCompSettings.EQType.DiffuseField && c.MaxBoostDb.HasValue
            };
            _diffuseUseMaxBoost.CheckedChanged += delegate { UpdateUiState(); };
            _diffuseMaxBoost = NewStepper(0, 24, 0.5, 1, c.MaxBoostDb ?? 6);

            _diffuseUseLowFreq = new CheckBox
            {
                Text = "Apply low-frequency cutoff",
                Checked = c.SelectedEQ == RhinoSystemCompSettings.EQType.DiffuseField && c.LowFreqHz.HasValue
            };
            _diffuseUseLowFreq.CheckedChanged += delegate { UpdateUiState(); };
            _diffuseLowFreq = NewStepper(20, 20000, 10, 0, c.LowFreqHz ?? 60);

            _diffuseMinPhase = new CheckBox
            {
                Text = "Use minimum-phase filter",
                Checked = c.MinPhase
            };

            _diffuseUseSmoothing = new CheckBox
            {
                Text = "Enable smoothing",
                Checked = c.SelectedEQ == RhinoSystemCompSettings.EQType.DiffuseField && c.SmoothingOct.HasValue
            };
            _diffuseUseSmoothing.CheckedChanged += delegate { UpdateUiState(); };
            _diffuseSmoothing = NewStepper(0.05, 2.0, 0.05, 2, c.SmoothingOct ?? 0.33);

            var layout = new DynamicLayout();
            layout.Padding = new Padding(8);
            layout.Spacing = new Size(6, 6);
            layout.AddRow(_diffuseUseMaxBoost, _diffuseMaxBoost, null);
            layout.AddRow(_diffuseUseLowFreq, _diffuseLowFreq, null);
            layout.AddRow(_diffuseMinPhase, null);
            layout.AddRow(_diffuseUseSmoothing, null);
            layout.AddRow(new Label { Text = "Smoothing (octaves)", Width = 180 }, _diffuseSmoothing);

            _diffuseFieldPanel = new Panel { Content = layout };
        }

        private Control BuildSofaGroup()
        {
            var layout = new DynamicLayout();
            layout.Padding = new Padding(8);
            layout.Spacing = new Size(6, 6);
            layout.AddRow(new Label { Text = "SOFA file", Width = 180 }, _sofaPath, _browseSofaButton);

            return new GroupBox
            {
                Text = "HRTF Dataset",
                Content = layout
            };
        }

        private Control BuildCompensationGroup()
        {
            var layout = new DynamicLayout();
            layout.Padding = new Padding(8);
            layout.Spacing = new Size(8, 8);

            layout.AddRow(new Label { Text = "Input interpretation", Width = 180 }, _inputTypeDrop);
            layout.AddRow(new Label { Text = "Equalisation", Width = 180 }, _eqTypeDrop);
            layout.AddRow(_eqNote);
            layout.AddRow(_measurementPanel);
            layout.AddRow(_freeFieldPanel);
            layout.AddRow(_diffuseFieldPanel);

            return new GroupBox
            {
                Text = "Input / Equalisation Processing",
                Content = layout
            };
        }

        private void UpdateUiState()
        {
            bool inputIsDtf = _inputTypeDrop.SelectedIndex == 1;
            RhinoSystemCompSettings.EQType eq = GetSelectedEqType();

            _eqTypeDrop.Enabled = !inputIsDtf;

            if (inputIsDtf)
            {
                _measurementPanel.Visible = false;
                _freeFieldPanel.Visible = false;
                _diffuseFieldPanel.Visible = false;
                _eqNote.Text = "Directional-only / DTF input bypasses equalisation setup.";
            }
            else
            {
                _measurementPanel.Visible = eq == RhinoSystemCompSettings.EQType.Measurement;
                _freeFieldPanel.Visible = eq == RhinoSystemCompSettings.EQType.FreeField;
                _diffuseFieldPanel.Visible = eq == RhinoSystemCompSettings.EQType.DiffuseField;

                _eqNote.Text = string.Empty;
            }

            _measurementSmoothing.Enabled = _measurementUseSmoothing.Checked == true;

            _freeFieldMaxBoost.Enabled = _freeFieldUseMaxBoost.Checked == true;
            _freeFieldLowFreq.Enabled = _freeFieldUseLowFreq.Checked == true;
            _freeFieldSmoothing.Enabled = _freeFieldUseSmoothing.Checked == true;

            _diffuseMaxBoost.Enabled = _diffuseUseMaxBoost.Checked == true;
            _diffuseLowFreq.Enabled = _diffuseUseLowFreq.Checked == true;
            _diffuseSmoothing.Enabled = _diffuseUseSmoothing.Checked == true;
        }

        private RhinoSystemCompSettings.EQType GetSelectedEqType()
        {
            int idx = Math.Max(0, _eqTypeDrop.SelectedIndex);
            return (RhinoSystemCompSettings.EQType)idx;
        }

        private HrtfCompensationOptions BuildCompensationOptions()
        {
            bool inputIsDtf = _inputTypeDrop.SelectedIndex == 1;
            RhinoSystemCompSettings.EQType eq = inputIsDtf ? RhinoSystemCompSettings.EQType.None : GetSelectedEqType();

            var c = new HrtfCompensationOptions
            {
                InputIsDTF = inputIsDtf,
                SelectedEQ = eq
            };

            switch (eq)
            {
                case RhinoSystemCompSettings.EQType.Measurement:
                    c.MeasurementReferencePath = _measurementRefPath.Text;
                    c.IsCalibrated = _measurementIsCalibrated.Checked == true;
                    c.SmoothingOct = _measurementUseSmoothing.Checked == true ? (double?)_measurementSmoothing.Value : null;
                    c.MaxBoostDb = null;
                    c.LowFreqHz = null;
                    c.MinPhase = false;
                    c.FreeFieldIncidence = 0;
                    break;

                case RhinoSystemCompSettings.EQType.FreeField:
                    c.MeasurementReferencePath = null;
                    c.IsCalibrated = false;
                    c.SmoothingOct = _freeFieldUseSmoothing.Checked == true ? (double?)_freeFieldSmoothing.Value : null;
                    c.MaxBoostDb = _freeFieldUseMaxBoost.Checked == true ? (double?)_freeFieldMaxBoost.Value : null;
                    c.LowFreqHz = _freeFieldUseLowFreq.Checked == true ? (double?)_freeFieldLowFreq.Value : null;
                    c.MinPhase = false;
                    c.FreeFieldIncidence = (int)(_freeFieldIncidence.Value == null ? 30: _freeFieldIncidence.Value);
                    break;

                case RhinoSystemCompSettings.EQType.DiffuseField:
                    c.MeasurementReferencePath = null;
                    c.IsCalibrated = false;
                    c.SmoothingOct = _diffuseUseSmoothing.Checked == true ? (double?)_diffuseSmoothing.Value : null;
                    c.MaxBoostDb = _diffuseUseMaxBoost.Checked == true ? (double?)_diffuseMaxBoost.Value : null;
                    c.LowFreqHz = _diffuseUseLowFreq.Checked == true ? (double?)_diffuseLowFreq.Value : null;
                    c.MinPhase = _diffuseMinPhase.Checked == true;
                    c.FreeFieldIncidence = 0;
                    break;

                default:
                    c.MeasurementReferencePath = null;
                    c.IsCalibrated = false;
                    c.SmoothingOct = null;
                    c.MaxBoostDb = null;
                    c.LowFreqHz = null;
                    c.MinPhase = false;
                    c.FreeFieldIncidence = 0;
                    break;
            }

            return c;
        }

        private Pachyderm_Acoustic.Audio.HRTF LoadHrtf()
        {
            string path = _sofaPath.Text;

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidOperationException("Please select an HRTF SOFA file.");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The selected HRTF SOFA file was not found.", path);
            }

            var hrtf = new Pachyderm_Acoustic.Audio.HRTF(path);
            if (!hrtf.ValidationPassed)
            {
                throw new InvalidOperationException(hrtf.ValidationMessage);
            }

            return hrtf;
        }

        private void OnAccept()
        {
            try
            {
                ClearStatus();

                HrtfCompensationOptions comp = BuildCompensationOptions();
                Pachyderm_Acoustic.Audio.HRTF hrtf = LoadHrtf();

                ResultData = new HrtfDialogResult
                {
                    HrtfPath = _sofaPath.Text,
                    Hrtf = hrtf,
                    Compensation = comp,
                    CompensationPrimitives = comp.ToPrimitiveSettings()
                };

                Close(true);
            }
            catch (Exception ex)
            {
                SetStatus(ex.Message);
            }
        }

        private void BrowseSofa()
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Select HRTF SOFA file";
            dlg.Filters.Add(new FileFilter("SOFA files", ".sofa"));

            if (dlg.ShowDialog(this) == DialogResult.Ok)
            {
                _sofaPath.Text = dlg.FileName;
            }
        }

        private void BrowseMeasurementReference()
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Select measurement reference transfer function";
            dlg.Filters.Add(new FileFilter("Wave files", ".wav"));

            if (dlg.ShowDialog(this) == DialogResult.Ok)
            {
                _measurementRefPath.Text = dlg.FileName;
            }
        }

        private static NumericStepper NewStepper(double min, double max, double increment, int decimals, double value)
        {
            return new NumericStepper
            {
                MinValue = min,
                MaxValue = max,
                Increment = increment,
                DecimalPlaces = decimals,
                Value = value
            };
        }

        private void SetStatus(string text)
        {
            _status.Text = text ?? string.Empty;
        }

        private void ClearStatus()
        {
            _status.Text = string.Empty;
        }
    }
}