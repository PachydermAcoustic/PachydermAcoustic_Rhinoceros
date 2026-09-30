# Run with PowerShell 7: pwsh -File tests/ArrayPattern.Regression.ps1
# Compiles production numerical methods with managed geometry adapters.
# A running Rhino viewport is still needed for interactive verification.
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot
$source = Get-Content (Join-Path $root 'Pach_Directivity_Conduit.cs') -Raw
function Extract-Block([string]$text, [string]$signature) {
    $start = $text.IndexOf($signature)
    if ($start -lt 0) { throw "Missing production member: $signature" }
    $open = $text.IndexOf('{', $start)
    $depth = 1
    $end = $open + 1
    while ($depth -gt 0) {
        if ($text[$end] -eq '{') { $depth++ }
        if ($text[$end] -eq '}') { $depth-- }
        $end++
    }
    return $text.Substring($start, $end - $start)
}
$members = @(
    'public enum DiagnosticPlane',
    'private static Vector3d DiagnosticDirection',
    'public double ArrayDiagnosticLevel',
    'private static void GetAiming',
    'private static Hare.Geometry.Vector WorldToLocal',
    'private sealed class DirectivityLookup',
    'internal sealed class ArrayPattern',
    'private static Point3d SourcePoint',
    'private static double SafeParse'
) | ForEach-Object { Extract-Block $source $_ }
$composite = Get-Content (Join-Path $root 'Pach_Composite_Array_Source.cs') -Raw
$composite = $composite.Substring($composite.IndexOf('namespace '))
$adapters = @'
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Rhino.Geometry;
using Rhino.DocObjects;
namespace Rhino {
    public static class RhinoMath { public const double ZeroTolerance = 1e-12; }
}
namespace Rhino.Geometry {
    public struct Vector3d {
        public double X,Y,Z;
        public Vector3d(double x,double y,double z) { X=x; Y=y; Z=z; }
        public double Length => Math.Sqrt(X*X+Y*Y+Z*Z);
        public bool Unitize() { double n=Length; if(n==0)return false; X/=n;Y/=n;Z/=n; return true; }
        public static Vector3d operator *(Vector3d v,double a) => new Vector3d(v.X*a,v.Y*a,v.Z*a);
    }
    public struct Point3d {
        public double X,Y,Z;
        public Point3d(double x,double y,double z) { X=x;Y=y;Z=z; }
        public static Point3d operator +(Point3d p,Vector3d v) => new Point3d(p.X+v.X,p.Y+v.Y,p.Z+v.Z);
        public static Vector3d operator -(Point3d a,Point3d b) => new Vector3d(a.X-b.X,a.Y-b.Y,a.Z-b.Z);
    }
    public struct BoundingBox { public Point3d Center; }
    public class Point {
        public Point3d Location;
        private Dictionary<string,string> data = new Dictionary<string,string>();
        public string GetUserString(string key) => data.ContainsKey(key)?data[key]:null;
        public void SetUserString(string key,string value) { data[key]=value; }
        public BoundingBox GetBoundingBox(bool accurate) => new BoundingBox { Center=Location };
    }
}
namespace Rhino.DocObjects { public class RhinoObject { public Rhino.Geometry.Point Geometry=new Rhino.Geometry.Point(); } }
namespace Hare.Geometry {
    public class Vector {
        public double dx,dy,dz;
        public Vector(double x,double y,double z) { dx=x;dy=y;dz=z; }
        public void Normalize() { double n=Math.Sqrt(dx*dx+dy*dy+dz*dz);dx/=n;dy/=n;dz/=n; }
    }
}
namespace Pachyderm_Acoustic.Utilities {
    public static class PachTools {
        public static double[] DecodeEight(string s) => s.Split(';').Select(v=>double.Parse(v,CultureInfo.InvariantCulture)).ToArray();
        public static double[] DecodeSourcePower(string s) => DecodeEight(s);
    }
    public static class Numerics { public static double[] angularFrequency_Octave=Enumerable.Range(0,8).Select(i=>2*Math.PI*62.5*Math.Pow(2,i)).ToArray(); }
    public static class RCPachTools { public static Point3d RPttoHPt(Point3d p)=>p; }
    public static class AcousticalMath { public static double Pressure_Intensity(double p,double rho)=>Math.Sqrt(p*rho); }
}
namespace Pachyderm_Acoustic.Environment {
    public class GeodesicSource {
        protected double[] SourcePower; protected string type; public double Rho_C=411.6;
        public GeodesicSource(double[] db,Point3d p,int id,bool thirds) { SourcePower=db.Select(v=>1e-12*Math.Pow(10,v/10)).ToArray(); }
        public virtual double[] DirPower(int t,int r,Hare.Geometry.Vector d)=>SourcePower;
        public virtual double[] DirPressure(int t,int r,Hare.Geometry.Vector d)=>SourcePower;
    }
}
'@
$checks = @'
private ArrayPattern Array_Diagnostic_Pattern;
private double Array_Diagnostic_Max;
public DiagnosticPlane Array_Diagnostic_Plane;
public static int Run() {
    int count=0;
    Action<double,double,string> near=(actual,expected,label)=>{
        if(double.IsNaN(actual) || Math.Abs(actual-expected)>1e-9) throw new Exception(label+": "+actual+" != "+expected);
        count++;
    };
    // Forward rotations follow Balloon.Update_Aim. Check arbitrary off-axis
    // directions, elevation poles, both azimuth signs and wrap-around.
    var v=new Vector3d(.3,.4,.5); v.Unitize();
    foreach(double alt in new[]{-90d,-35d,0d,42d,90d})
    foreach(double azi in new[]{-360d,-90d,-15d,0d,30d,90d,360d})
    foreach(double axi in new[]{-72d,0d,51d}) {
        var w=v;
        double a=axi*Math.PI/180, x=w.X,z=w.Z;
        w.X=x*Math.Cos(a)-z*Math.Sin(a); w.Z=x*Math.Sin(a)+z*Math.Cos(a);
        a=alt*Math.PI/180; double y=w.Y; z=w.Z;
        w.Y=y*Math.Cos(a)-z*Math.Sin(a); w.Z=y*Math.Sin(a)+z*Math.Cos(a);
        a=azi*Math.PI/180; x=w.X;y=w.Y;
        w.X=x*Math.Cos(a)-y*Math.Sin(a); w.Y=x*Math.Sin(a)+y*Math.Cos(a);
        var recovered=WorldToLocal(w,alt,azi,axi);
        near(recovered.dx,v.X,"inverse x");near(recovered.dy,v.Y,"inverse y");near(recovered.dz,v.Z,"inverse z");
    }
    near(WorldToLocal(new Vector3d(-1,0,0),0,90,0).dy,1,"positive azimuth");
    near(WorldToLocal(new Vector3d(1,0,0),0,-90,0).dy,1,"negative azimuth");
    Func<double,double,RhinoObject> element=(x,delay)=>{
        var obj=new RhinoObject();
        obj.Geometry.Location=new Point3d(x,0,0);
        obj.Geometry.SetUserString("SWL",string.Join(";",Enumerable.Repeat("120",8)));
        obj.Geometry.SetUserString("Delay",delay.ToString(CultureInfo.InvariantCulture));
        return obj;
    };
    var first=element(0,0);
    var second=element(0,0);
    var front=new Vector3d(0,1,0);
    var pair=new List<RhinoObject>{first,second};
    var inPhase=new ArrayPattern(pair,4,10);
    near(inPhase.Magnitude(front),2,"coherent amplitude");
    near(inPhase.RelativePower(front),1,"relative peak");
    near(inPhase.PowerLevel,120+20*Math.Log10(2),"coherent power level");
    var composite=new CompositeArraySource(pair,10,0);
    near(composite.DirPower(0,0,new Hare.Geometry.Vector(0,1,0))[4],4,"simulation coherent power");
    second.Geometry.SetUserString("Delay","0.5"); // Half a cycle at 1 kHz.
    near(new ArrayPattern(pair,4,10).Magnitude(front),0,"half-cycle cancellation");
    near(inPhase.Magnitude(front),2,"immutable snapshot");
    second.Geometry.SetUserString("Delay","0");
    second.Geometry.SetUserString("ArrayGainOctaveDb",string.Join(";",Enumerable.Repeat("-6.020599913279624",8)));
    near(new ArrayPattern(pair,4,10).Magnitude(front),1.5,"gain shading");
    second.Geometry.SetUserString("ArrayGainOctaveDb",string.Join(";",Enumerable.Repeat("0",8)));
    second.Geometry.SetUserString("ArrayDelayOctaveMs",string.Join(";",Enumerable.Repeat("0.5",8)));
    near(new ArrayPattern(pair,4,10).Magnitude(front),0,"octave delay");
    // Half-wavelength spacing along X: equal paths along Y, cancellation along X.
    pair=new List<RhinoObject>{element(-.08575,0),element(.08575,0)};
    var spaced=new ArrayPattern(pair,4,10);
    near(spaced.Magnitude(front),2,"broadside lobe");
    near(spaced.Magnitude(new Vector3d(1,0,0)),0,"spatial null");
    near(ArraySimulationSettings.UseComposite(first)?1:0,0,"legacy defaults to elements");
    first.Geometry.SetUserString(ArraySimulationSettings.ModeKey,"Composite");
    near(ArraySimulationSettings.UseComposite(first)?1:0,1,"composite setting");
    near(ArraySimulationSettings.ReferenceDistance(first),10,"default distance");
    first.Geometry.SetUserString(ArraySimulationSettings.DistanceKey,"25.5");
    near(ArraySimulationSettings.ReferenceDistance(first),25.5,"saved distance");
    first.Geometry.SetUserString(ArraySimulationSettings.DistanceKey,"NaN");
    near(ArraySimulationSettings.ReferenceDistance(first),10,"invalid distance fallback");
    string row=string.Join(" ",Enumerable.Range(0,19).Select(u=>(u*2).ToString(CultureInfo.InvariantCulture)));
    first.Geometry.SetUserString("SourceType","2");
    first.Geometry.SetUserString("Balloon1000",string.Join(";",Enumerable.Repeat(row,36))+";");
    foreach(double azi in new[]{-90d,90d}) {
        first.Geometry.SetUserString("Aiming","0;"+azi.ToString(CultureInfo.InvariantCulture)+";0");
        var aimed=new ArrayPattern(new List<RhinoObject>{first},4,10);
        var direction=new Vector3d(-Math.Sin(azi*Math.PI/180),Math.Cos(azi*Math.PI/180),0);
        near(aimed.Magnitude(direction),1,"directional lobe follows azimuth");
        if(aimed.Magnitude(new Vector3d(-direction.X,-direction.Y,0))>.02) throw new Exception("back lobe too strong");
        count++;
    }
    // Diagnostic readout: exact angles, plane conventions, shared reference and deep nulls.
    var left = new RhinoObject();
    var right = new RhinoObject();
    left.Geometry.Location = new Point3d(-0.1, 0, 0);
    right.Geometry.Location = new Point3d(0.1, 0, 0);
    var diagnosticPattern = new ArrayPattern(new List<RhinoObject>{left,right},4,100);
    var diagnostic = new SpeakerPatternConduit();
    if (!double.IsNaN(diagnostic.ArrayDiagnosticLevel(0))) throw new Exception("empty diagnostic should be unavailable");
    count++;
    diagnostic.Array_Diagnostic_Pattern = diagnosticPattern;
    diagnostic.Array_Diagnostic_Max = 20*Math.Log10(2);
    foreach (DiagnosticPlane plane in Enum.GetValues(typeof(DiagnosticPlane))) {
        diagnostic.Array_Diagnostic_Plane = plane;
        foreach (double degrees in new[]{-180d,-73.66,0d,23.86,90d,180d,360d}) {
            double a=degrees*Math.PI/180;
            Vector3d direction = plane == DiagnosticPlane.XY ? new Vector3d(Math.Cos(a),Math.Sin(a),0) :
                plane == DiagnosticPlane.XZ ? new Vector3d(Math.Cos(a),0,Math.Sin(a)) : new Vector3d(0,Math.Cos(a),Math.Sin(a));
            double expected=20*Math.Log10(diagnosticPattern.Magnitude(direction)/2);
            near(diagnostic.ArrayDiagnosticLevel(degrees),expected,"diagnostic plane and exact angle");
            near(diagnostic.ArrayDiagnosticLevel(degrees+360),expected,"diagnostic angle wrap");
        }
    }
    diagnostic.Array_Diagnostic_Plane = DiagnosticPlane.XZ;
    near(diagnostic.ArrayDiagnosticLevel(90),0,"broadside reference");
    diagnostic.Array_Diagnostic_Max += 6;
    near(diagnostic.ArrayDiagnosticLevel(90),-6,"shared normalization is not slice-normalized");
    diagnostic.Array_Diagnostic_Max -= 6;
    // For the symmetric two-source array solve the finite-distance half-wavelength path difference.
    double halfWavelength = 343.0 / 1000.0 / 2;
    double cosine = halfWavelength * Math.Sqrt(100*100 + .1*.1 - halfWavelength*halfWavelength/4) / (2*100*.1);
    double nullDegrees = Math.Acos(cosine)*180/Math.PI;
    if (diagnostic.ArrayDiagnosticLevel(nullDegrees) > -90) throw new Exception("exact null lost to slice interpolation");
    count++;
    if (!double.IsNaN(diagnostic.ArrayDiagnosticLevel(double.NaN)) ||
        !double.IsNaN(diagnostic.ArrayDiagnosticLevel(double.PositiveInfinity))) throw new Exception("invalid diagnostic angle");
    count++;
    return count;
}
'@
$nl = [System.Environment]::NewLine
$code = $adapters + $nl + 'namespace Pachyderm_Acoustic.UI { public class SpeakerPatternConduit {' +
    $nl + ($members -join $nl) + $nl + $checks + $nl + '} }' + $nl + $composite
Add-Type -TypeDefinition $code
$count = [Pachyderm_Acoustic.UI.SpeakerPatternConduit]::Run()
Write-Output "PASS: $count numerical regression checks (production code; managed geometry adapters)."
