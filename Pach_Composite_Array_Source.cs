using System;
using System.Collections.Generic;
using System.Globalization;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace Pachyderm_Acoustic.UI
{
    internal static class ArraySimulationSettings
    {
        internal const string ModeKey = "ArraySimulationMode";
        internal const string DistanceKey = "ArrayReferenceDistance";
        internal const string CenterModeKey = "ArrayCenterMode";

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
        private readonly SpeakerPatternConduit.ArrayPattern[] patterns;
        private readonly SpeakerPatternConduit.ArrayPattern[] thirdPatterns;

        internal CompositeArraySource(List<RhinoObject> elements, double distance, int id)
            : this(BuildPatterns(elements, distance, true), id) { }

        private CompositeArraySource(SpeakerPatternConduit.ArrayPattern[] thirdPatterns, int id)
            : base(PowerLevels(thirdPatterns), thirdPatterns[0].Center, id, false)
        {
            this.thirdPatterns = thirdPatterns;
            patterns = new SpeakerPatternConduit.ArrayPattern[8];
            for (int oct = 0; oct < 8; oct++) patterns[oct] = new SpeakerPatternConduit.ArrayPattern(thirdPatterns, oct);
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
            double[] levels = PowerLevels(thirdOctave ? thirdPatterns : patterns);
            SourcePower = Array.ConvertAll(levels, v => 1E-12 * Math.Pow(10, v / 10));
            SPL = levels;
        }

        public override double[] DirPower(int threadid, int random, Hare.Geometry.Vector direction)
        {
            var result = new double[BandCount];
            var world = new Hare.Geometry.Vector(direction.dx, direction.dy, direction.dz);
            for (int oct = 0; oct < BandCount; oct++)
                result[oct] = SourcePower[oct] * (ThirdOctave ? thirdPatterns : patterns)[oct].RelativePower(world);
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
