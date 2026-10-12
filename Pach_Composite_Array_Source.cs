using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace Pachyderm_Acoustic.UI
{
    internal static class ArraySimulationSettings
    {
        internal const string ModeKey = "ArraySimulationMode";
        internal const string DistanceKey = "ArrayReferenceDistance";
        internal const string CenterModeKey = "ArrayCenterMode";
        internal const string PowerKey = "ArraySoundPowerBandsDb";
        internal const string PowerEnabledKey = "ArraySoundPowerEnabled";
        internal const string PowerModeKey = "ArraySoundPowerMode";

        internal static bool PerElementPower(RhinoObject obj)
        {
            return obj != null && obj.Geometry != null && obj.Geometry.GetUserString(PowerModeKey) == "Element";
        }

        internal static List<RhinoObject> SpeakerCluster(List<RhinoObject> selection)
        {
            if (selection == null || selection.Count == 0 || selection.Any(obj => obj == null || (obj.Geometry.GetUserString("SourceType") != "2" && obj.Geometry.GetUserString("SourceType") != "3"))) return new List<RhinoObject>();
            string group = selection[0].Geometry.GetUserString("ArrayGroup");
            string key = string.IsNullOrWhiteSpace(group) ? "Cluster" : "ArrayGroup";
            group = selection[0].Geometry.GetUserString(key);
            if (string.IsNullOrWhiteSpace(group) || selection.Any(obj => obj.Geometry.GetUserString(key) != group)) return new List<RhinoObject>();
            Rhino.RhinoDoc doc = selection[0].Document ?? Rhino.RhinoDoc.ActiveDoc;
            return doc == null ? new List<RhinoObject>() : doc.Objects.GetObjectList(ObjectType.Point)
                .Where(obj => obj.Geometry.GetUserString(key) == group && (obj.Geometry.GetUserString("SourceType") == "2" || obj.Geometry.GetUserString("SourceType") == "3")).ToList();
        }

        internal static double[] ElementPower(RhinoObject obj, bool thirdOctave)
        {
            double[] controlled = PerElementPower(obj) ? TargetPower(obj, thirdOctave) : null;
            if (controlled != null) return controlled;
            double[] levels = Utilities.PachTools.DecodeSourcePower(obj.Geometry.GetUserString("SWL"));
            return Utilities.AcousticalMath.ConvertBandEnergy(levels.Select(value => Math.Pow(10, value / 10)).ToArray(), thirdOctave).Select(value => 10 * Math.Log10(value)).ToArray();
        }

        internal static double[] TargetPower(RhinoObject obj, bool thirdOctave)
        {
            if (obj == null || obj.Geometry == null || obj.Geometry.GetUserString(PowerEnabledKey) == "False") return null;
            string spectrum = obj.Geometry.GetUserString(PowerKey);
            if (!string.IsNullOrWhiteSpace(spectrum))
            {
                double[] levels = Utilities.PachTools.DecodeSourcePower(spectrum);
                return Utilities.AcousticalMath.ConvertBandEnergy(levels.Select(value => Math.Pow(10, value / 10)).ToArray(), thirdOctave).Select(value => 10 * Math.Log10(value)).ToArray();
            }
            string mode = obj.Geometry.GetUserString("ArrayControlMode") ?? obj.Geometry.GetUserString("ArrayDeviceType");
            if (mode != "Column" && obj.Geometry.GetUserString(PowerEnabledKey) != "True" && string.IsNullOrWhiteSpace(obj.Geometry.GetUserString("ArraySoundPowerDba"))) return null;
            double defaultLevel;
            if (!double.TryParse(obj.Geometry.GetUserString("ArraySoundPowerDba"), NumberStyles.Float, CultureInfo.InvariantCulture, out defaultLevel)
                || double.IsNaN(defaultLevel) || double.IsInfinity(defaultLevel)) defaultLevel = 90;
            return Array.ConvertAll(Utilities.AcousticalMath.ConvertBandEnergy(Enumerable.Repeat(Math.Pow(10, defaultLevel / 10), 8).ToArray(), thirdOctave), value => 10 * Math.Log10(value));
        }

        // Legacy octave controls describe delays/gains, not energies: replicate rather than divide.
        internal static double[] BandValues(RhinoObject obj, string kind, bool thirdOctave)
        {
            string suffix = kind == "Delay" ? "Ms" : "Db";
            string native = obj.Geometry.GetUserString("Array" + kind + "ThirdOctave" + suffix);
            string legacy = obj.Geometry.GetUserString("Array" + kind + "Octave" + suffix);
            string[] parts = (thirdOctave && !string.IsNullOrWhiteSpace(native) ? native : legacy ?? "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            double[] result = new double[thirdOctave ? 24 : 8];
            for (int i = 0; i < result.Length; i++)
            {
                int index = parts.Length == 8 && thirdOctave ? i / 3 : i;
                if (index < parts.Length) double.TryParse(parts[index], NumberStyles.Float, CultureInfo.InvariantCulture, out result[i]);
            }
            return result;
        }

        internal static bool UseComposite(RhinoObject obj)
        {
            return obj != null && obj.Geometry.GetUserString(ModeKey) == "Composite";
        }

        internal static double ReferenceDistance(RhinoObject obj)
        {
            double value;
            if (obj != null && double.TryParse(obj.Geometry.GetUserString(DistanceKey),
                NumberStyles.Float, CultureInfo.InvariantCulture, out value) &&
                !double.IsNaN(value) && !double.IsInfinity(value) && value >= 1 && value <= 200)
                return value;
            return 10;
        }

        internal static bool UseAimingCenter(RhinoObject obj)
        {
            return obj != null && obj.Geometry != null && obj.Geometry.GetUserString(CenterModeKey) == "Aiming";
        }
    }
    // One source at the array center, using the same fixed-distance coherent
    // pattern as the aiming balloon. Propagation loss is applied by the solver.
    internal sealed class CompositeArraySource : Environment.GeodesicSource
    {
        private readonly SpeakerPatternConduit.ArrayPattern[] thirdPatterns;
        private double[] powerScale = Array.ConvertAll(new double[24], value => 1.0);

        public override double[] SoundPower
        {
            get { return base.SoundPower; }
            set
            {
                double[] updated = (double[])powerScale.Clone();
                for (int band = 0; band < 24; band++)
                {
                    int index = value.Length == 24 ? band : band / 3;
                    if (base.SoundPower[index] > 0) updated[band] *= value[index] / base.SoundPower[index];
                }
                powerScale = updated;
                base.SoundPower = value;
            }
        }

        internal CompositeArraySource(List<RhinoObject> elements, double distance, int id)
            : this(BuildPatterns(elements, distance, true), id) { }

        private CompositeArraySource(SpeakerPatternConduit.ArrayPattern[] thirdPatterns, int id)
            : base(PowerLevels(thirdPatterns), thirdPatterns[0].Center, id, false)
        {
            this.thirdPatterns = thirdPatterns;
            type = "Directional";
        }

        private static SpeakerPatternConduit.ArrayPattern[] BuildPatterns(List<RhinoObject> elements, double distance, bool thirdOctave)
        {
            var result = new SpeakerPatternConduit.ArrayPattern[thirdOctave ? 24 : 8];
            for (int oct = 0; oct < result.Length; oct++)
                result[oct] = new SpeakerPatternConduit.ArrayPattern(elements, oct, distance, thirdOctave);
            return result;
        }

        private static double[] PowerLevels(SpeakerPatternConduit.ArrayPattern[] patterns)
        {
            var result = new double[patterns.Length];
            for (int oct = 0; oct < result.Length; oct++) result[oct] = patterns[oct].PowerLevel;
            return result;
        }

        public override void SetBandResolution(bool thirdOctave)
        {
            double[] levels = PowerLevels(thirdPatterns);
            SourcePower = Utilities.AcousticalMath.ConvertBandEnergy(levels.Select((value, band) => powerScale[band] * 1E-12 * Math.Pow(10, value / 10)).ToArray(), thirdOctave);
            SPL = Array.ConvertAll(SourcePower, v => 10 * Math.Log10(v / 1E-12));
        }

        public override double[] DirPower(int threadid, int random, Hare.Geometry.Vector direction)
        {
            var result = new double[BandCount];
            var world = new Hare.Geometry.Vector(direction.dx, direction.dy, direction.dz);
            double[] thirdPower = new double[24];
            for (int band = 0; band < thirdPower.Length; band++)
                thirdPower[band] = powerScale[band] * 1E-12 * Math.Pow(10, thirdPatterns[band].PowerLevel / 10) * thirdPatterns[band].RelativePower(world);
            result = Utilities.AcousticalMath.ConvertBandEnergy(thirdPower, ThirdOctave);
            return result;
        }

        public override double[] DirPressure(int threadid, int random, Hare.Geometry.Vector direction)
        {
            double[] result = DirPower(threadid, random, direction);
            for (int oct = 0; oct < BandCount; oct++)
                result[oct] = Utilities.AcousticalMath.Pressure_Intensity(result[oct], Rho_C);
            return result;
        }
    }
}
