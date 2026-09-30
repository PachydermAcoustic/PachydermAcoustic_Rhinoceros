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
    }

    // One source at the array center, using the same fixed-distance coherent
    // pattern as the aiming balloon. Propagation loss is applied by the solver.
    internal sealed class CompositeArraySource : Environment.GeodesicSource
    {
        private readonly SpeakerPatternConduit.ArrayPattern[] patterns;

        internal CompositeArraySource(List<RhinoObject> elements, double distance, int id)
            : this(BuildPatterns(elements, distance), id) { }

        private CompositeArraySource(SpeakerPatternConduit.ArrayPattern[] patterns, int id)
            : base(PowerLevels(patterns), Utilities.RCPachTools.RPttoHPt(patterns[0].Center), id, false)
        {
            this.patterns = patterns;
            type = "Directional";
        }

        private static SpeakerPatternConduit.ArrayPattern[] BuildPatterns(List<RhinoObject> elements, double distance)
        {
            var result = new SpeakerPatternConduit.ArrayPattern[8];
            for (int oct = 0; oct < 8; oct++)
                result[oct] = new SpeakerPatternConduit.ArrayPattern(elements, oct, distance);
            return result;
        }

        private static double[] PowerLevels(SpeakerPatternConduit.ArrayPattern[] patterns)
        {
            var result = new double[8];
            for (int oct = 0; oct < 8; oct++) result[oct] = patterns[oct].PowerLevel;
            return result;
        }

        public override double[] DirPower(int threadid, int random, Hare.Geometry.Vector direction)
        {
            var result = new double[8];
            var world = new Vector3d(direction.dx, direction.dy, direction.dz);
            for (int oct = 0; oct < 8; oct++)
                result[oct] = SourcePower[oct] * patterns[oct].RelativePower(world);
            return result;
        }

        public override double[] DirPressure(int threadid, int random, Hare.Geometry.Vector direction)
        {
            double[] result = DirPower(threadid, random, direction);
            for (int oct = 0; oct < 8; oct++)
                result[oct] = Utilities.AcousticalMath.Pressure_Intensity(result[oct], Rho_C);
            return result;
        }
    }
}
