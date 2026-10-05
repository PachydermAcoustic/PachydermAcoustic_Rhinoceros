# Run with PowerShell 7: pwsh -File tests/ModelUnits.Regression.ps1
# Exercises production boundary and mesh conversion methods with managed adapters.
# Rhino scene meshing/intersections still require an interactive Rhino check.
$ErrorActionPreference = 'Stop'
$source = Get-Content (Join-Path (Split-Path $PSScriptRoot) 'Rhino_Pach_Tools.cs') -Raw
function Extract-Block([string]$signature) {
    $start = $source.IndexOf($signature)
    if ($start -lt 0) { throw "Missing production member: $signature" }
    $open = $source.IndexOf('{', $start)
    $depth = 1
    $end = $open + 1
    while ($depth -gt 0) {
        if ($source[$end] -eq '{') { $depth++ }
        if ($source[$end] -eq '}') { $depth-- }
        $end++
    }
    $source.Substring($start, $end - $start)
}
$members = @(
    'public static double GetModelToMetersScale(Rhino.RhinoDoc doc)',
    'public static Hare.Geometry.Point ModelPointToHare(',
    'public static Point3d HarePointToModel(',
    'public static Mesh HareMeshToModel(',
    'public static double ModelToMeters(double value)',
    'public static double MetersToModel(double value)',
    'public static Hare.Geometry.Point RPttoHPt(Point3d Point)',
    'public static Hare.Geometry.Vector RPttoHPt(Vector3d Point)',
    'public static Point3d HPttoRPt(Hare.Geometry.Point Point)',
    'public static Vector3d HPttoRPt(Hare.Geometry.Vector Point)',
    'public static Hare.Geometry.Topology RhinotoHareMesh(Mesh M)',
    'public static Mesh HaretoRhinoMesh(Hare.Geometry.Topology T, bool welded)'
) | ForEach-Object { Extract-Block $_ }
$code = @'
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Rhino.Geometry;
namespace Rhino {
    public enum UnitSystem { Meters, Millimeters, Centimeters, Inches, Feet, CustomUnits, None }
    public class RhinoDoc {
        public static RhinoDoc ActiveDoc;
        public UnitSystem ModelUnitSystem;
        public double CustomScale;
        public bool GetCustomUnitSystem(bool model, out string name, out double scale) { name="test"; scale=CustomScale; return true; }
    }
    public static class RhinoMath {
        public static double UnitScale(UnitSystem from, UnitSystem to) {
            double[] factors={1,.001,.01,.0254,.3048,1};
            return factors[(int)from]/factors[(int)to];
        }
    }
}
namespace Rhino.Geometry {
    public struct Point3d {
        public double X,Y,Z;
        public static Point3d Origin => new Point3d(0,0,0);
        public Point3d(double x,double y,double z) { X=x;Y=y;Z=z; }
    }
    public struct Vector3d {
        public double X,Y,Z;
        public Vector3d(double x,double y,double z) { X=x;Y=y;Z=z; }
    }
    public class MeshFace {
        public int A,B,C,D;
        public bool IsTriangle => C==D;
        public MeshFace(int a,int b,int c,int d) { A=a;B=b;C=c;D=d; }
    }
    public class Faces : List<MeshFace> {
        public void AddFace(int a,int b,int c) { Add(new MeshFace(a,b,c,c)); }
        public void AddFace(int a,int b,int c,int d) { Add(new MeshFace(a,b,c,d)); }
    }
    public class Normals { public void ComputeNormals() {} }
    public class Transform {
        public double Factor;
        public static Transform Scale(Point3d origin,double factor) { return new Transform { Factor=factor }; }
    }
    public class Vertices : List<Point3d> {
        public bool UseDoublePrecisionVertices;
        public Point3d Point3dAt(int i) { return this[i]; }
    }
    public class Mesh : IDisposable {
        public Vertices Vertices=new Vertices();
        public void Dispose() {}
        public Mesh DuplicateMesh() {
            var copy=new Mesh(); copy.Vertices.AddRange(Vertices); copy.Faces.AddRange(Faces); return copy;
        }
        public bool Transform(Transform t) {
            for(int i=0;i<Vertices.Count;i++) { var p=Vertices[i]; Vertices[i]=new Point3d(p.X*t.Factor,p.Y*t.Factor,p.Z*t.Factor); }
            return true;
        }
        public Faces Faces=new Faces();
        public Normals Normals=new Normals();
    }
}
namespace Hare.Geometry {
    public class Point {
        public double x,y,z;
        public int index;
        public Point(double a,double b,double c) { x=a;y=b;z=c; }
    }
    public class Vector {
        public double dx,dy,dz;
        public Vector(double a,double b,double c) { dx=a;dy=b;dz=c; }
    }
    public class Polygon {
        public Point[] Points;
        public int VertextCT => Points.Length;
    }
    public class Topology {
        public List<Point> vertices=new List<Point>();
        public Polygon[] Polys;
        public int Vertex_Count => vertices.Count;
        public int Polygon_Count => Polys.Length;
        public Point this[int i] => vertices[i];
        public Topology(Point[][] faces) {
            Polys=faces.Select(face=> {
                foreach(Point p in face) { p.index=vertices.Count; vertices.Add(p); }
                return new Polygon { Points=face };
            }).ToArray();
        }
        public void Finish_Topology(List<Point> points) {}
        public Point[] Polygon_Vertices(int i) { return Polys[i].Points; }
    }
}
namespace Utilities {
    public class RCPachTools {
        public static double ModelToMetersScale => GetModelToMetersScale(Rhino.RhinoDoc.ActiveDoc);
        public static double MetersToModelScale => 1.0/ModelToMetersScale;
__MEMBERS__
    }
}
public static class ModelUnitsRegression {
    static void Near(double expected,double actual) {
        if(Math.Abs(expected-actual)>1e-9*Math.Max(1,Math.Abs(expected)))
            throw new Exception("Expected "+expected+", got "+actual);
    }
    public static void Run() {
        var units=new[] { Rhino.UnitSystem.Meters,Rhino.UnitSystem.Millimeters,Rhino.UnitSystem.Centimeters,Rhino.UnitSystem.Inches,Rhino.UnitSystem.Feet,Rhino.UnitSystem.CustomUnits };
        double[] scale={1,.001,.01,.0254,.3048,2.5};
        for(int i=0;i<units.Length;i++) {
            Rhino.RhinoDoc.ActiveDoc=new Rhino.RhinoDoc { ModelUnitSystem=units[i],CustomScale=scale[i] };
            var model=new Point3d(12/scale[i],-3/scale[i],4/scale[i]);
            var raw=Utilities.RCPachTools.RPttoHPt(model);
            Near(model.X,raw.x); Near(model.X,Utilities.RCPachTools.HPttoRPt(raw).X);
            var meter=Utilities.RCPachTools.ModelPointToHare(model);
            Near(12,meter.x);Near(-3,meter.y);Near(4,meter.z);
            var roundtrip=Utilities.RCPachTools.HarePointToModel(meter);
            Near(model.X,roundtrip.X);Near(model.Y,roundtrip.Y);Near(model.Z,roundtrip.Z);
            Near(.5/scale[i],Utilities.RCPachTools.MetersToModel(.5));
            Near(12,Utilities.RCPachTools.ModelToMeters(model.X));
            // A physical 3-4-5 source/receiver separation must retain time and spreading in every document.
            var receiver=Utilities.RCPachTools.ModelPointToHare(new Point3d(15/scale[i],1/scale[i],4/scale[i]));
            double distance=Math.Sqrt(Math.Pow(receiver.x-meter.x,2)+Math.Pow(receiver.y-meter.y,2)+Math.Pow(receiver.z-meter.z,2));
            Near(5,distance); Near(5.0/343,distance/343); Near(1.0/25,1/(distance*distance));
            Near(2,Utilities.RCPachTools.ModelToMeters(2/scale[i])); // Track length.
            Near(.5/scale[i],Utilities.RCPachTools.MetersToModel(.5)); // Mapping/line element spacing.
            Near(.25,(.25*scale[i])/Utilities.RCPachTools.ModelToMetersScale); // Inverse-length curvature.
            var direction=Utilities.RCPachTools.RPttoHPt(new Vector3d(.3,.4,.5));
            Near(.3,direction.dx);Near(.4,direction.dy);Near(.5,direction.dz);
            var restoredDirection=Utilities.RCPachTools.HPttoRPt(direction);
            Near(.3,restoredDirection.X);
            var topology=new Hare.Geometry.Topology(new[] {
                new[] { new Hare.Geometry.Point(12,-3,4),new Hare.Geometry.Point(13,-3,4),new Hare.Geometry.Point(13,-2,4) },
                new[] { new Hare.Geometry.Point(12,-3,5),new Hare.Geometry.Point(13,-3,5),new Hare.Geometry.Point(13,-2,5),new Hare.Geometry.Point(12,-2,5) }
            });
            foreach(bool welded in new[] { true,false }) {
                var rawMesh=Utilities.RCPachTools.HaretoRhinoMesh(topology,welded);
                Near(12,rawMesh.Vertices[0].X);
                Near(12,Utilities.RCPachTools.RhinotoHareMesh(rawMesh).vertices.Min(p => p.x));
                var mesh=Utilities.RCPachTools.HareMeshToModel(topology,welded);
                Near(12/scale[i],mesh.Vertices[0].X);
                Near(-3/scale[i],mesh.Vertices[0].Y);
                var metersMesh=mesh.DuplicateMesh();
                metersMesh.Transform(Transform.Scale(Point3d.Origin,scale[i]));
                var back=Utilities.RCPachTools.RhinotoHareMesh(metersMesh);
                Near(12/scale[i],mesh.Vertices[0].X);
                foreach(var point in back.vertices) {
                    if(point.x<12-1e-9 || point.x>13+1e-9 || point.z<4-1e-9 || point.z>5+1e-9) throw new Exception("Mesh roundtrip changed physical coordinates");
                }
                Near(2,back.Polygon_Count);
            }
        }
        var metersDoc=new Rhino.RhinoDoc { ModelUnitSystem=Rhino.UnitSystem.Meters };
        Rhino.RhinoDoc.ActiveDoc=new Rhino.RhinoDoc { ModelUnitSystem=Rhino.UnitSystem.Millimeters };
        Near(12,Utilities.RCPachTools.ModelPointToHare(new Point3d(12,0,0),metersDoc).x);
        Near(12,Utilities.RCPachTools.HarePointToModel(new Hare.Geometry.Point(12,0,0),metersDoc).X);
        foreach(double bad in new[] { 0.0,-1.0,double.NaN,double.PositiveInfinity }) {
            var invalid=new Rhino.RhinoDoc { ModelUnitSystem=Rhino.UnitSystem.CustomUnits,CustomScale=bad };
            bool rejected=false;
            try { Utilities.RCPachTools.GetModelToMetersScale(invalid); } catch(InvalidOperationException) { rejected=true; }
            if(!rejected) throw new Exception("Invalid custom unit scale accepted");
        }
        Rhino.RhinoDoc.ActiveDoc=new Rhino.RhinoDoc { ModelUnitSystem=Rhino.UnitSystem.None };
        Near(1,Utilities.RCPachTools.ModelToMetersScale);
        Rhino.RhinoDoc.ActiveDoc=null;
        Near(1,Utilities.RCPachTools.ModelToMetersScale);
    }
}
'@
$code = $code.Replace('__MEMBERS__', ($members -join [Environment]::NewLine))
Add-Type -TypeDefinition $code
[ModelUnitsRegression]::Run()
Write-Output 'PASS: explicit meter/document boundaries across six unit systems; raw points, vectors and meshes preserve coordinates; owning-document override; invalid custom scales; unitless/no-document conventions; triangle/quad welded/unwelded round trips.'
