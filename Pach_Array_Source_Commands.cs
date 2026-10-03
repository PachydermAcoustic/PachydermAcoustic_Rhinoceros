//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)
//'
//'This file is part of Pachyderm-Acoustic.
//'
//'Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit
//'Pachyderm-Acoustic is free software; you can redistribute it and/or modify
//'it under the terms of the GNU General Public License as published
//'by the Free Software Foundation; either version 3 of the License, or
//'(at your option) any later version.

using Eto.Drawing;
using Eto.Forms;
using Pachyderm_Acoustic.Environment;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        internal static class ArraySourceConstruction
        {
            internal static void AssignArrayMetadata(RhinoObject obj, Guid arrayGroup, string groupLabel, int elementIndex, string deviceType)
            {
                if (obj == null || obj.Geometry == null) return;

                string suffix = AlphabeticSuffix(elementIndex);
                string label = groupLabel + suffix;

                obj.Geometry.SetUserString("ArrayGroup", arrayGroup.ToString());
                obj.Geometry.SetUserString("ArrayMode", "Steerable");
                obj.Geometry.SetUserString("ArrayDeviceType", deviceType);
                obj.Geometry.SetUserString("ArrayGroupLabel", groupLabel);
                obj.Geometry.SetUserString("ArrayElementIndex", elementIndex.ToString(CultureInfo.InvariantCulture));
                obj.Geometry.SetUserString("ArrayElementSuffix", suffix);
                obj.Geometry.SetUserString("SourceLabel", label);
                obj.Geometry.SetUserString("Cluster", null);

                if (string.IsNullOrWhiteSpace(obj.Geometry.GetUserString("ArrayPhaseOctaveDeg"))) obj.Geometry.SetUserString("ArrayPhaseOctaveDeg", "0;0;0;0;0;0;0;0");
                if (string.IsNullOrWhiteSpace(obj.Geometry.GetUserString("ArrayDelayOctaveMs"))) obj.Geometry.SetUserString("ArrayDelayOctaveMs", "0;0;0;0;0;0;0;0");
                if (string.IsNullOrWhiteSpace(obj.Geometry.GetUserString("ArrayGainOctaveDb"))) obj.Geometry.SetUserString("ArrayGainOctaveDb", "0;0;0;0;0;0;0;0");
                if (string.IsNullOrWhiteSpace(obj.Geometry.GetUserString("Delay"))) obj.Geometry.SetUserString("Delay", "0");
            }

            internal static string NextArrayLabel(RhinoDoc doc)
            {
                int max = 0;

                if (doc != null)
                {
                    foreach (RhinoObject obj in doc.Objects.GetObjectList(ObjectType.Point))
                    {
                        if (obj == null || obj.Geometry == null) continue;
                        int value;
                        if (int.TryParse(obj.Geometry.GetUserString("ArrayGroupLabel"), out value) && value > max) max = value;
                    }
                }

                return (max + 1).ToString("00", CultureInfo.InvariantCulture);
            }

            internal static string AlphabeticSuffix(int index)
            {
                string s = "";
                index++;

                while (index > 0)
                {
                    index--;
                    s = (char)('a' + (index % 26)) + s;
                    index /= 26;
                }

                return s;
            }

            internal static string AimingString(Vector3d forward, Vector3d up)
            {
                if (!forward.Unitize()) forward = Vector3d.YAxis;

                up -= forward * (up * forward);

                if (!up.Unitize())
                {
                    up = Math.Abs(forward * Vector3d.ZAxis) < 0.95 ? Vector3d.ZAxis : Vector3d.XAxis;
                    up -= forward * (up * forward);
                    up.Unitize();
                }

                double altitude = Math.Asin(Math.Max(-1.0, Math.Min(1.0, forward.Z)));
                double azimuth = Math.Atan2(-forward.X, forward.Y);

                Vector3d x0 = new Vector3d(Math.Cos(azimuth), Math.Sin(azimuth), 0);
                Vector3d z0 = new Vector3d(Math.Sin(azimuth) * Math.Sin(altitude), -Math.Cos(azimuth) * Math.Sin(altitude), Math.Cos(altitude));
                double axial = Math.Atan2(-(up * x0), up * z0);

                return (altitude * 180.0 / Math.PI).ToString("0.########", CultureInfo.InvariantCulture) + ";" +
                    (azimuth * 180.0 / Math.PI).ToString("0.########", CultureInfo.InvariantCulture) + ";" +
                    (axial * 180.0 / Math.PI).ToString("0.########", CultureInfo.InvariantCulture);
            }

            internal static string[] CircularPistonBalloon(double effectiveDiameter_m)
            {
                double[] frequency =
                    new double[] { 62.5, 125, 250, 500, 1000, 2000, 4000, 8000 };

                string[] result = new string[8];

                double radius = Math.Max(0.001, effectiveDiameter_m * 0.5);
                const double c = 343.0;

                for (int oct = 0; oct < 8; oct++)
                {
                    double k = 2.0 * Math.PI * frequency[oct] / c;
                    StringBuilder code = new StringBuilder();

                    for (int v = 0; v < 72; v++)
                    {
                        for (int u = 0; u < 37; u++)
                        {
                            double theta = u * Math.PI / 36.0;
                            double x = k * radius * Math.Sin(theta);

                            double magnitude;

                            if (Math.Abs(x) < 1E-10)
                            {
                                magnitude = 1.0;
                            }
                            else
                            {
                                magnitude = Math.Abs(2.0 * MathNet.Numerics.SpecialFunctions.BesselJ(1, x) / x);
                            }

                            double attenuation = -20.0 * Math.Log10(Math.Max(0.001, magnitude));
                            attenuation = Math.Max(0.0, Math.Min(60.0, attenuation));
                            if (u > 0) code.Append(" ");
                            code.Append( attenuation.ToString( "0.000", CultureInfo.InvariantCulture));
                        }

                        code.Append(";");
                    }

                    result[oct] = code.ToString();
                }

                return result;
            }

            internal static void SetColumnCabinetGeometry(RhinoObject owner, double width_m, double depth_m, double height_m, double centerOffsetZ_m)
            {
                if (owner == null || owner.Geometry == null) return;

                double x0 = -width_m * 0.5;
                double x1 = width_m * 0.5;
                double y0 = 0;
                double y1 = -depth_m;
                double z0 = centerOffsetZ_m - height_m * 0.5;
                double z1 = centerOffsetZ_m + height_m * 0.5;
                Func<double, string> F = value => value.ToString("0.########", CultureInfo.InvariantCulture);

                string points = "P|" +
                    "0:" + F(x0) + "," + F(y0) + "," + F(z0) + ";" +
                    "1:" + F(x1) + "," + F(y0) + "," + F(z0) + ";" +
                    "2:" + F(x1) + "," + F(y0) + "," + F(z1) + ";" +
                    "3:" + F(x0) + "," + F(y0) + "," + F(z1) + ";" +
                    "4:" + F(x0) + "," + F(y1) + "," + F(z0) + ";" +
                    "5:" + F(x1) + "," + F(y1) + "," + F(z0) + ";" +
                    "6:" + F(x1) + "," + F(y1) + "," + F(z1) + ";" +
                    "7:" + F(x0) + "," + F(y1) + "," + F(z1) + ";";

                string faces = "F|0:0,1,2,3;1:1,5,6,2;2:5,4,7,6;3:4,0,3,7;4:3,2,6,7;5:4,5,1,0;";

                owner.Geometry.SetUserString("CLF_CabinetPoints", points);
                owner.Geometry.SetUserString("CLF_CabinetFaces", faces);
                owner.Geometry.SetUserString("CLF_CabinetLines", "L|");
                owner.Geometry.SetUserString("ArrayCabinetOwner", "True");
            }

            internal static void CopyTemplateSource(RhinoObject template, RhinoObject target)
            {
                if (template == null || template.Geometry == null || target == null || target.Geometry == null) return;

                NameValueCollection values = template.Geometry.GetUserStrings();

                if (values != null)
                {
                    foreach (string key in values.AllKeys)
                    {
                        if (string.IsNullOrWhiteSpace(key)) continue;
                        if (key.StartsWith("Array", StringComparison.Ordinal)) continue;
                        if (key == "Cluster" || key == "SourceLabel" || key == "Aiming" || key == "Delay") continue;
                        target.Geometry.SetUserString(key, values[key]);
                    }
                }

                target.Attributes.Name = "Acoustical Source";
            }

            internal static double TemplatePitch(RhinoObject template)
            {
                if (template == null || template.Geometry == null) return 0.25;

                Cabinet_Geometry cabinet = Cabinet_Geometry_Parser.Parse(
                    template.Geometry.GetUserString("CLF_CabinetPoints"),
                    template.Geometry.GetUserString("CLF_CabinetFaces"),
                    template.Geometry.GetUserString("CLF_CabinetLines"));

                if (cabinet != null && cabinet.Vertices != null && cabinet.Vertices.Count > 0)
                {
                    double min = double.PositiveInfinity;
                    double max = double.NegativeInfinity;

                    for (int i = 0; i < cabinet.Vertices.Count; i++)
                    {
                        min = Math.Min(min, cabinet.Vertices[i].z);
                        max = Math.Max(max, cabinet.Vertices[i].z);
                    }

                    double height = max - min;
                    if (!double.IsNaN(height) && !double.IsInfinity(height) && height > 0.01) return height;
                }

                return 0.25;
            }

            internal static void AddRhinoGroup(RhinoDoc doc, string name, List<Guid> ids)
            {
                if (doc == null || ids == null || ids.Count == 0) return;
                if (doc.Groups.FindName(name) == null) doc.Groups.Add(name, ids);
                else doc.Groups.Add(ids);
            }

            internal static void AddToSourceConduit(RhinoObject obj)
            {
                if (obj == null || SourceConduit.Instance == null) return;

                foreach (Guid id in SourceConduit.Instance.UUID)
                {
                    if (id == obj.Id) return;
                }

                SourceConduit.Instance.SetSource(obj);
            }
        }

        [System.Runtime.InteropServices.Guid("394714A7-5C0D-4720-B824-EB01A6ED2979")]
        public class PachColumnArraySourceCommand : Rhino.Commands.Command
        {
            public override string EnglishName
            {
                get { return "Pach_Column_Array_Source"; }
            }

            protected override Result RunCommand(RhinoDoc doc, RunMode mode)
            {
                int elementCount = 8;
                double spacing_m = 0.075;
                double driverDiameter_m = 0.05;
                double cabinetWidth_m = 0.08;
                double cabinetDepth_m = 0.10;
                double elementSWL = 90.0;

                OptionInteger elementCountOption = new OptionInteger(elementCount, 2, 256);
                OptionDouble spacingOption = new OptionDouble(spacing_m, 0.005, 2.0);
                OptionDouble driverDiameterOption = new OptionDouble(driverDiameter_m, 0.005, 1.0);
                OptionDouble cabinetWidthOption = new OptionDouble(cabinetWidth_m, 0.01, 2.0);
                OptionDouble cabinetDepthOption = new OptionDouble(cabinetDepth_m, 0.01, 2.0);
                OptionDouble elementSWLOption = new OptionDouble(elementSWL, 20.0, 200.0);

                GetOption getOptions = new GetOption();
                getOptions.SetCommandPrompt("Column array parameters");
                getOptions.AddOptionInteger("Elements", ref elementCountOption);
                getOptions.AddOptionDouble("Spacing_m", ref spacingOption);
                getOptions.AddOptionDouble("DriverDiameter_m", ref driverDiameterOption);
                getOptions.AddOptionDouble("CabinetWidth_m", ref cabinetWidthOption);
                getOptions.AddOptionDouble("CabinetDepth_m", ref cabinetDepthOption);
                getOptions.AddOptionDouble("ElementSWL", ref elementSWLOption);
                getOptions.AcceptNothing(true);

                while (true)
                {
                    GetResult result = getOptions.Get();

                    if (result == GetResult.Cancel)
                    {
                        return Result.Cancel;
                    }

                    if (result == GetResult.Nothing)
                    {
                        break;
                    }
                }

                elementCount = elementCountOption.CurrentValue;
                spacing_m = spacingOption.CurrentValue;
                driverDiameter_m = driverDiameterOption.CurrentValue;
                cabinetWidth_m = cabinetWidthOption.CurrentValue;
                cabinetDepth_m = cabinetDepthOption.CurrentValue;
                elementSWL = elementSWLOption.CurrentValue;

                if (spacing_m < driverDiameter_m)
                {
                    RhinoApp.WriteLine("Warning: element spacing is smaller than the effective driver diameter.");
                }

                /*
                 * Cabinet height extends half a driver diameter beyond the center
                 * of the first and last driver.
                 */
                double cabinetHeight_m = (elementCount - 1) * spacing_m + driverDiameter_m;

                Rhino.Geometry.Point3d center;

                Result rc = RhinoGet.GetPoint("Select center of column array", false, out center);

                if (rc != Result.Success)
                {
                    return rc;
                }

                GetPoint getAim = new GetPoint();
                getAim.SetCommandPrompt("Select forward direction of column array");
                getAim.SetBasePoint(center, true);
                getAim.DrawLineFromPoint(center, true);

                if (getAim.Get() != GetResult.Point)
                {
                    return Result.Cancel;
                }

                Rhino.Geometry.Vector3d forward = getAim.Point() - center;

                forward.Z = 0;

                if (!forward.Unitize())
                {
                    RhinoApp.WriteLine("A valid horizontal aiming direction is required.");
                    return Result.Cancel;
                }

                double azimuth = Math.Atan2(-forward.X, forward.Y) * 180.0 / Math.PI;
                string aiming = "0;" + azimuth.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture) + ";0";
                string groupLabel = "01";

                rc = RhinoGet.GetString("Array label", true, ref groupLabel);

                if (rc != Result.Success)
                {
                    return rc;
                }

                double modelUnitsPerMeter = RhinoMath.UnitScale(UnitSystem.Meters, doc.ModelUnitSystem);
                double spacing = spacing_m * modelUnitsPerMeter;
                Source_Constructions.Cabinet_Diffraction cabinet = new Source_Constructions.Cabinet_Diffraction(cabinetWidth_m, cabinetHeight_m, cabinetDepth_m);

                double[] swl = new double[]{elementSWL, elementSWL, elementSWL, elementSWL, elementSWL, elementSWL, elementSWL, elementSWL};
                Guid arrayGroup = Guid.NewGuid();
                List<Guid> groupIds = new List<Guid>();

                List<RhinoObject> elements = new List<RhinoObject>();
                int cabinetOwner = elementCount / 2;
                RhinoApp.WriteLine("Generating BTMS cabinet directivity for {0} drivers...", elementCount);

                for (int i = 0; i < elementCount; i++)
                {
                    double offset_m = ((elementCount - 1) * 0.5 - i) * spacing_m;
                    double offset = ((elementCount - 1) * 0.5 - i) * spacing;
                    Point3d location = center + Vector3d.ZAxis * offset;
                    Hare.Geometry.Point localDriver = new Hare.Geometry.Point(0, 0, offset_m);

                    RhinoApp.WriteLine("Calculating driver {0} of {1}...", i + 1, elementCount);
                    string[] balloon = cabinet.Driver_Balloon(localDriver, driverDiameter_m);
                    Guid id = doc.Objects.AddPoint(location);

                    if (id == Guid.Empty)
                    {
                        continue;
                    }

                    RhinoObject obj = doc.Objects.FindId(id);

                    if (obj == null || obj.Geometry == null)
                    {
                        continue;
                    }

                    obj.Attributes.Name = "Acoustical Source";
                    obj.Geometry.SetUserString("SourceType", "3");
                    obj.Geometry.SetUserString("Model", "Generic BTMS Column Driver");
                    obj.Geometry.SetUserString("SWL", Utilities.PachTools.EncodeSourcePower(swl));
                    obj.Geometry.SetUserString("Phase", "0;0;0;0;0;0;0;0");
                    obj.Geometry.SetUserString("Aiming", aiming);
                    obj.Geometry.SetUserString("Delay", "0");
                    obj.Geometry.SetUserString("Balloon63", balloon[0]);
                    obj.Geometry.SetUserString("Balloon125", balloon[1]);
                    obj.Geometry.SetUserString("Balloon250", balloon[2]);
                    obj.Geometry.SetUserString("Balloon500", balloon[3]);
                    obj.Geometry.SetUserString("Balloon1000", balloon[4]);
                    obj.Geometry.SetUserString("Balloon2000", balloon[5]);
                    obj.Geometry.SetUserString("Balloon4000", balloon[6]);
                    obj.Geometry.SetUserString("Balloon8000", balloon[7]);

                    PachSteerableArraySourceCommand.AssignSteerableArrayMetadata(obj, arrayGroup, groupLabel, i);

                    obj.Geometry.SetUserString("ArrayDeviceType", "Column");
                    obj.Geometry.SetUserString("ArrayElementSpacing_m", spacing_m.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArrayDriverDiameter_m", driverDiameter_m.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArrayCabinetWidth_m", cabinetWidth_m.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArrayCabinetHeight_m", cabinetHeight_m.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArrayCabinetDepth_m", cabinetDepth_m.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArrayDriverDirectivityModel", "CircularPiston_BTMS_FrontBaffle");
                    obj.Geometry.SetUserString("ArrayCabinetDiffraction", "BTMS_FirstOrderFrontEdges");

                    if (i == cabinetOwner)
                    {
                        double cabinetCenterZ = -offset_m;
                        double x0 = -cabinetWidth_m * 0.5;
                        double x1 = cabinetWidth_m * 0.5;
                        double y0 = 0;
                        double y1 = -cabinetDepth_m;
                        double z0 = cabinetCenterZ - cabinetHeight_m * 0.5;
                        double z1 = cabinetCenterZ + cabinetHeight_m * 0.5;
                        string sx0 = x0.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture);
                        string sx1 = x1.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture);
                        string sy0 = y0.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture);
                        string sy1 = y1.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture);
                        string sz0 = z0.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture);
                        string sz1 = z1.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture);

                        string cabinetPoints = "P|" + "0:" + sx0 + "," + sy0 + "," + sz0 + ";" + "1:" + sx1 + "," + sy0 + "," + sz0 + ";" +
                            "2:" + sx1 + "," + sy0 + "," + sz1 + ";" + "3:" + sx0 + "," + sy0 + "," + sz1 + ";" + "4:" + sx0 + "," + sy1 + "," + sz0 + ";" +
                            "5:" + sx1 + "," + sy1 + "," + sz0 + ";" + "6:" + sx1 + "," + sy1 + "," + sz1 + ";" + "7:" + sx0 + "," + sy1 + "," + sz1 + ";";

                        string cabinetFaces = "F|" + "0:0,1,2,3;" + "1:1,5,6,2;" + "2:5,4,7,6;" + "3:4,0,3,7;" + "4:3,2,6,7;" + "5:4,5,1,0;";

                        obj.Geometry.SetUserString("CLF_CabinetPoints", cabinetPoints);
                        obj.Geometry.SetUserString("CLF_CabinetFaces", cabinetFaces);
                        obj.Geometry.SetUserString("CLF_CabinetLines", "L|");
                        obj.Geometry.SetUserString("ArrayCabinetOwner", "True");
                    }
                    else
                    {
                        obj.Geometry.SetUserString("CLF_CabinetPoints", "P|");
                        obj.Geometry.SetUserString("CLF_CabinetFaces", "F|");
                        obj.Geometry.SetUserString("CLF_CabinetLines", "L|");
                        obj.Geometry.SetUserString("ArrayCabinetOwner", "False");
                    }

                    doc.Objects.ModifyAttributes(obj, obj.Attributes, true);
                    obj.CommitChanges();
                    bool found = false;

                    foreach (Guid sourceId in SourceConduit.Instance.UUID)
                    {
                        if (sourceId == obj.Id)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        SourceConduit.Instance.SetSource(obj);
                    }

                    groupIds.Add(id);
                    elements.Add(obj);
                }

                if (elements.Count == 0)
                {
                    RhinoApp.WriteLine("No column-array sources were created.");
                    return Result.Failure;
                }

                string rhinoGroupName = "Pachyderm Column Array " + groupLabel;

                if (doc.Groups.FindName(rhinoGroupName) == null)
                {
                    doc.Groups.Add(rhinoGroupName, groupIds);
                }
                else
                {
                    doc.Groups.Add(groupIds);
                }

                doc.Objects.UnselectAll();

                for (int i = 0;i < elements.Count; i++)
                {
                    elements[i].Select(true);
                }

                doc.Views.Redraw();
                RhinoApp.WriteLine("Created column array {0} with {1} BTMS cabinet-mounted drivers.", groupLabel, elements.Count);
                Pach_ArrayControl form = new Pach_ArrayControl(elements);
                form.Show();

                return Result.Success;
            }
        }
    }
}
