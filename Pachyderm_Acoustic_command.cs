//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)   
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2025, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit 
//'Pachyderm-Acoustic is free software; you can redistribute it and/or modify 
//'it under the terms of the GNU General Public License as published 
//'by the Free Software Foundation; either version 3 of the License, or 
//'(at your option) any later version. 
//'Pachyderm-Acoustic is distributed in the hope that it will be useful, 
//'but WITHOUT ANY WARRANTY; without even the implied warranty of 
//'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
//'GNU General Public License for more details. 
//' 
//'You should have received a copy of the GNU General Public 
//'License along with Pachyderm-Acoustic; if not, write to the Free Software 
//'Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA. 

using Pachyderm_Acoustic.Utilities;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        ///</summary> 
        [System.Runtime.InteropServices.Guid("9d8d728a-98ae-4b80-bf4a-89a160964c05")]
        public class PachyDerm_Acoustic : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Acoustic";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("8559be06-21d7-4535-803e-95a9dd3a2898"));
                return Result.Success;
            }
        }

        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        /// This command shows the controls for the particle animation tool. 
        ///</summary> 
        [System.Runtime.InteropServices.Guid("c55c2833-9b75-4ca5-b666-f0b8b52ad237")]
        public class Pach_Visual_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Animation";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("EA23F0D6-5462-4e42-9CFC-DC8E79723112"));
                return Result.Success;
            }
        }

        ///<summary> 
        /// A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants. 
        /// DO NOT create an instance of this class (this is the responsibility of Rhino.NET.) 
        /// This command shows the controls for the particle animation tool. 
        ///</summary>
        [System.Runtime.InteropServices.Guid("5CB43517-DB4A-4f5e-B7CB-54A79EED3727")]
        public class Pach_Mapping_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Mapping";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("55E14BEE-72F4-4d8c-B751-9BED20A7C5BC"));
                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("3668b612-1002-45ca-867f-94d0f98b741c")]
        public class Pach_CustomMap_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_CustomMapping";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("1c48c00e-abd8-40fd-8642-2ce7daa90ed5"));
                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("a0930289-af08-499e-af91-4d55c583f2b1")]
        public class Pach_TimeDomain_Control_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "PachyDerm_Numeric_TimeDomain";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Rhino.UI.Panels.OpenPanel(new System.Guid("7c62fae6-efa7-4c07-af12-cd440049c7fc"));
                return Result.Success;
            }
        }

        //public class Pach_SpeakerBuilder_Command : Command
        //{
        //    ///<returns>The command name as it appears on the Rhino command line</returns> 
        //    public override string EnglishName
        //    {
        //        get
        //        {
        //            return "PachyDerm_SpeakerBuilder";
        //        }
        //    }

        //    ///<summary> This gets called when when the user runs this command.</summary> 
        //    protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
        //    {
        //        Rhino.UI.Panels.OpenPanel(new System.Guid("5CD1A25E-1CC8-4BF2-A103-58CFDA8CF424"));
        //        return Result.Success;
        //    }
        //}

        [System.Runtime.InteropServices.Guid("89DE99D2-E07D-4730-8E98-ED1296B93808")]
        public class Pach_SetBackground_Noise : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Pachyderm_BackgroundNoise";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                double[] freq = new double[] { 62.5, 125, 250, 500, 1000, 2000, 4000, 8000 };
                double[] noise = new double[8];
                string n = Rhino.RhinoDoc.ActiveDoc.Strings.GetValue("Noise");

                if (n != "" && n != null)
                {
                    string[] ns = n.Split(","[0]);
                    double t = 0;
                    for (int i = 0; i < 8; i++) if (double.TryParse(ns[i], out t))
                        { noise[i] = t; }
                        else { noise[i] = 0; }
                }

                // Add option to choose noise criteria method
                Rhino.Input.Custom.GetOption getOption = new Rhino.Input.Custom.GetOption();
                getOption.SetCommandPrompt("Choose background noise input method");
                getOption.AddOption("Manual");
                getOption.AddOption("NoiseCriteria");
                getOption.AddOption("NoiseRating"); 
                getOption.AddOption("RoomCriteria");
                
                Rhino.Input.GetResult optionResult = getOption.Get();
                if (optionResult != Rhino.Input.GetResult.Option)
                    return Result.Cancel;

                string selectedOption = getOption.Option().EnglishName;

                if (selectedOption == "Manual")
                {
                    // Original manual input method
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            Rhino.Input.RhinoGet.GetNumber(string.Format("Specify background noise sound pressure level at {0} Hertz.", freq[i]), true, ref noise[i]);
                        }
                        catch
                        {
                            return Result.Nothing;
                        }
                    }
                }
                else if (selectedOption == "NoiseCriteria")
                {
                    double ncValue = 40.0; // Initial default
                    bool validInput = false;
                    
                    while (!validInput)
                    {
                        try
                        {
                            Result result = Rhino.Input.RhinoGet.GetNumber("Specify Noise Criteria (NC) value (15-80)", true, ref ncValue);
                            if (result == Result.Cancel)
                                return Result.Cancel;

                            if (ncValue >= 15 && ncValue <= 80)
                            {
                                validInput = true;
                                noise = Pachyderm_Acoustic.Utilities.AcousticalMath.Noise_Criteria(ncValue);
                            }
                            else
                            {
                                Rhino.RhinoApp.WriteLine("NC value must be between 15 and 80. Please try again.");
                            }
                        }
                        catch
                        {
                            return Result.Nothing;
                        }
                    }
                }
                else if (selectedOption == "NoiseRating")
                {
                    double nrValue = 40.0; // Initial default
                    bool validInput = false;
                    
                    while (!validInput)
                    {
                        try
                        {
                            Result result = Rhino.Input.RhinoGet.GetNumber("Specify Noise Rating (NR) value (0-130)", true, ref nrValue);
                            if (result == Result.Cancel)
                                return Result.Cancel;
                                
                            if (nrValue >= 0 && nrValue <= 130)
                            {
                                validInput = true;
                                noise = Pachyderm_Acoustic.Utilities.AcousticalMath.Noise_Rating(nrValue);
                            }
                            else
                            {
                                Rhino.RhinoApp.WriteLine("NR value must be between 0 and 130. Please try again.");
                            }
                        }
                        catch
                        {
                            return Result.Nothing;
                        }
                    }
                }
                else if (selectedOption == "RoomCriteria")
                {
                    double rcValue = 40.0; // Initial default
                    try
                    {
                        Result result = Rhino.Input.RhinoGet.GetNumber("Specify Room Criteria (RC) value", true, ref rcValue);
                        if (result == Result.Cancel)
                            return Result.Cancel;
                            
                        noise = Pachyderm_Acoustic.Utilities.AcousticalMath.Room_Criteria(rcValue);
                    }
                    catch
                    {
                        return Result.Nothing;
                    }
                }

                Rhino.RhinoDoc.ActiveDoc.Strings.SetString("Noise", string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", noise[0], noise[1], noise[2], noise[3], noise[4], noise[5], noise[6], noise[7]));

                return Result.Success;
            }
        }

        [System.Runtime.InteropServices.Guid("C05DC933-8366-4C81-992A-31A8178C38BD")]
        public class Pach_Absorption : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Procedural_Absorption";
                }
            }

            ///<summary> This gets called when when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                Pach_Absorption_Designer PAD = new Pach_Absorption_Designer();
                PAD.ShowModal();
                return Result.Success;
            }
        }


        [System.Runtime.InteropServices.Guid("5A7D94B2-D83C-4E8A-9C8F-E2F67384C01B")]
        public class Pach_EmbodiedCarbon_Command : Command
        {
            ///<returns>The command name as it appears on the Rhino command line</returns> 
            public override string EnglishName
            {
                get
                {
                    return "Pach_Calculate_EmbodiedCarbon";
                }
            }

            ///<summary> This gets called when the user runs this command.</summary> 
            protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
            {
                // Dictionary to store layer embodied carbon values (kg CO2e/m²)
                Dictionary<string, double> layerCarbonValues = new Dictionary<string, double>();

                // Get all layers and their embodied carbon values
                for (int i = 0; i < doc.Layers.Count; i++)
                {
                    Rhino.DocObjects.Layer layer = doc.Layers[i];
                    string carbonText = layer.GetUserString("Carbon");

                    if (!string.IsNullOrEmpty(carbonText))
                    {
                        // Try to parse the carbon value
                        if (double.TryParse(carbonText, out double carbonValue))
                        {
                            if (layer.HasName == false) continue;
                            layerCarbonValues.Add(layer.FullPath, carbonValue);
                            Rhino.RhinoApp.WriteLine($"Layer: {layer.FullPath}, Embodied Carbon: {carbonValue} kg CO2e/m²");
                        }
                        else
                        {
                            Rhino.RhinoApp.WriteLine($"Warning: Layer {layer.FullPath} has invalid embodied carbon value: {carbonText}");
                        }
                    }
                }

                if (layerCarbonValues.Count == 0)
                {
                    Rhino.RhinoApp.WriteLine("No layers found with embodied carbon values.");
                    Rhino.RhinoApp.WriteLine("Please add 'EmbodiedCarbon' user text to layers with numerical values (kg CO2e/m²).");
                    return Result.Nothing;
                }

                // Get all objects in the model
                Rhino.DocObjects.ObjectEnumeratorSettings settings = new Rhino.DocObjects.ObjectEnumeratorSettings();
                settings.DeletedObjects = false;
                settings.HiddenObjects = false;
                settings.LockedObjects = true;
                settings.NormalObjects = true;
                settings.VisibleFilter = true;

                // We'll look at Breps, Surfaces and Extrusions
                settings.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep |
                                           Rhino.DocObjects.ObjectType.Surface |
                                           Rhino.DocObjects.ObjectType.Extrusion;

                // Variables to track results
                double totalArea = 0;
                double totalEmbodiedCarbon = 0;
                Dictionary<string, double> layerAreas = new Dictionary<string, double>();
                Dictionary<string, double> layerCarbon = new Dictionary<string, double>();

                // Process each object
                foreach (Rhino.DocObjects.RhinoObject obj in doc.Objects.GetObjectList(settings))
                {
                    // Get the layer path
                    string layerPath = doc.Layers[obj.Attributes.LayerIndex].FullPath;

                    // Check if this layer has a carbon value
                    if (!layerCarbonValues.TryGetValue(layerPath, out double carbonPerSqM))
                        continue;

                    // Calculate the area based on object type
                    double objectArea = 0;

                    if (obj.ObjectType == Rhino.DocObjects.ObjectType.Brep)
                    {
                        Brep brep = obj.Geometry as Brep;
                        if (brep != null)
                        {
                            AreaMassProperties props = AreaMassProperties.Compute(brep);
                            if (props != null)
                                objectArea = props.Area;
                        }
                    }
                    else if (obj.ObjectType == Rhino.DocObjects.ObjectType.Surface)
                    {
                        Surface srf = obj.Geometry as Surface;
                        if (srf != null)
                        {
                            AreaMassProperties props = AreaMassProperties.Compute(srf);
                            if (props != null)
                                objectArea = props.Area;
                        }
                    }
                    else if (obj.ObjectType == Rhino.DocObjects.ObjectType.Extrusion)
                    {
                        Extrusion extr = obj.Geometry as Extrusion;
                        if (extr != null)
                        {
                            Brep brep = extr.ToBrep();
                            AreaMassProperties props = AreaMassProperties.Compute(brep);
                            if (props != null)
                                objectArea = props.Area;
                        }
                    }

                    // Calculate embodied carbon for this object
                    double objectCarbon = objectArea * carbonPerSqM;

                    // Update totals
                    totalArea += objectArea;
                    totalEmbodiedCarbon += objectCarbon;

                    // Update layer-specific totals
                    if (!layerAreas.ContainsKey(layerPath))
                    {
                        layerAreas[layerPath] = 0;
                        layerCarbon[layerPath] = 0;
                    }

                    layerAreas[layerPath] += objectArea;
                    layerCarbon[layerPath] += objectCarbon;
                }

                // Display results
                Rhino.RhinoApp.WriteLine("\n--- Embodied Carbon Analysis Results ---");
                Rhino.RhinoApp.WriteLine($"Total surface area: {totalArea:F2} m²");
                Rhino.RhinoApp.WriteLine($"Total embodied carbon: {totalEmbodiedCarbon:F2} kg CO2e");

                Rhino.RhinoApp.WriteLine("\n--- Breakdown by Layer ---");
                foreach (string layer in layerCarbon.Keys)
                {
                    Rhino.RhinoApp.WriteLine($"Layer: {layer}");
                    Rhino.RhinoApp.WriteLine($"  Area: {layerAreas[layer]:F2} m²");
                    Rhino.RhinoApp.WriteLine($"  Embodied Carbon: {layerCarbon[layer]:F2} kg CO2e");
                    Rhino.RhinoApp.WriteLine($"  Carbon Intensity: {layerCarbonValues[layer]:F2} kg CO2e/m²");
                    Rhino.RhinoApp.WriteLine("");
                }

                // Create a formatted summary text
                string summaryText = $"Total Embodied Carbon: {totalEmbodiedCarbon:F2} kg CO2e";

                // Add summary text to the document
                Rhino.RhinoDoc.ActiveDoc.Strings.SetString("EmbodiedCarbonSummary", summaryText);

                return Result.Success;
            }
        }

        public class ExportPachydermModelCommand : Command
        {
            public override string EnglishName => "Pach_ExportModel";

            protected override Result RunCommand(RhinoDoc doc, RunMode mode)
            {
                // Prompt for save location
                string filename = "";
                var fd = new Rhino.UI.SaveFileDialog { Filter = "Pachyderm Model (*.pmodel)|*.pmodel|All Files (*.*)|*.*" };
                if (fd.ShowSaveDialog())
                    filename = fd.FileName;
                else
                    return Result.Cancel;

                // Configure options
                double meshQuality = 1.0;
                bool includeCurvature = true;

                using (GetNumber getNumber = new GetNumber())
                {
                    getNumber.SetCommandPrompt("Mesh quality (0.1-10.0, higher is better quality)");
                    getNumber.SetDefaultNumber(1.0);
                    getNumber.SetLowerLimit(0.1, false);
                    getNumber.SetUpperLimit(10.0, false);

                    if (getNumber.Get() != GetResult.Number)
                        return Result.Cancel;

                    meshQuality = getNumber.Number();
                }

                using (GetOption getOption = new GetOption())
                {
                    getOption.SetCommandPrompt("Include curvature information?");
                    getOption.AddOption("Yes");
                    getOption.AddOption("No");
                    getOption.SetCommandPromptDefault("Yes");

                    if (getOption.Get() != GetResult.Option)
                        return Result.Cancel;

                    includeCurvature = getOption.Option().Index == 0;
                }

                // Export the model
                try
                {
                    ExportModel(doc, filename, meshQuality, includeCurvature);
                    RhinoApp.WriteLine($"Model exported to {filename}");
                    return Result.Success;
                }
                catch (Exception ex)
                {
                    RhinoApp.WriteLine($"Error: {ex.Message}");
                    return Result.Failure;
                }
            }

            private void ExportModel(RhinoDoc doc, string filename, double meshQuality, bool includeCurvature)
            {
                // Get all visible objects
                List<RhinoObject> objects = doc.Objects.GetObjectList(new ObjectEnumeratorSettings() { VisibleFilter = true }).ToList<RhinoObject>();

                if (objects.Count == 0)
                {
                    throw new Exception("No valid geometry found in the model.");
                }

                // Process model to extract vertices, materials, and polygons
                Dictionary<int, Point3d> vertices = new Dictionary<int, Point3d>();
                Dictionary<string, int> materialMap = new Dictionary<string, int>();
                Dictionary<int, MaterialDefinition> materials = new Dictionary<int, MaterialDefinition>();
                List<ObjectDefinition> modelObjects = new List<ObjectDefinition>();

                int vertexIndex = 1; // Start at 1 to match file format

                foreach (RhinoObject obj in objects)
                {
                    // Get material from layer
                    string layerName = doc.Layers[obj.Attributes.LayerIndex].Name;
                    int materialId;

                    if (!materialMap.TryGetValue(layerName, out materialId))
                    {
                        materialId = materialMap.Count + 1; // Start at 1
                        materialMap[layerName] = materialId;
                        materials[materialId] = CreateMaterialFromLayer(doc.Layers[obj.Attributes.LayerIndex]);
                    }

                    // Create meshes from geometry
                    Mesh[] meshes = CreateMeshes(obj, meshQuality);

                    foreach (Mesh mesh in meshes)
                    {
                        // Create a new object definition
                        ObjectDefinition objectDef = new ObjectDefinition
                        {
                            MaterialId = materialId,
                            Polygons = new List<PolygonDefinition>()
                        };

                        // Process mesh vertices
                        Dictionary<Point3d, int> meshVertexMap = new Dictionary<Point3d, int>();
                        for (int i = 0; i < mesh.Vertices.Count; i++)
                        {
                            Point3d vertex = mesh.Vertices[i];
                            if (!meshVertexMap.ContainsKey(vertex))
                            {
                                meshVertexMap[vertex] = vertexIndex;
                                vertices[vertexIndex] = vertex;
                                vertexIndex++;
                            }
                        }

                        // Process mesh faces
                        for (int i = 0; i < mesh.Faces.Count; i++)
                        {
                            MeshFace face = mesh.Faces[i];
                            PolygonDefinition polyDef = new PolygonDefinition
                            {
                                VertexIds = new List<int>(),
                                IsCurved = false,
                                Kurvature = new double[2] { 0, 0 },
                                FrameAxes = new Hare.Geometry.Vector[2] { new Hare.Geometry.Vector(), new Hare.Geometry.Vector() }
                            };

                            // Add vertex indices
                            polyDef.VertexIds.Add(meshVertexMap[mesh.Vertices[face.A]]);
                            polyDef.VertexIds.Add(meshVertexMap[mesh.Vertices[face.B]]);
                            polyDef.VertexIds.Add(meshVertexMap[mesh.Vertices[face.C]]);
                            if (face.IsQuad)
                                polyDef.VertexIds.Add(meshVertexMap[mesh.Vertices[face.D]]);

                            // Calculate curvature if needed
                            if (includeCurvature)
                            {
                                CalculateCurvature(mesh, i, ref polyDef);
                            }

                            objectDef.Polygons.Add(polyDef);
                        }

                        modelObjects.Add(objectDef);
                    }
                }

                // Write the file
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("# Pachyderm Acoustic Model - Exported from Rhinoceros");
                    writer.WriteLine("# Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteLine();

                    // Write vertices
                    writer.WriteLine("[VERTICES]");
                    foreach (var vertex in vertices)
                    {
                        writer.WriteLine($"{vertex.Key} {FormatPoint(vertex.Value)}");
                    }
                    writer.WriteLine();

                    // Write materials
                    writer.WriteLine("[MATERIALS]");
                    foreach (var material in materials)
                    {
                        writer.WriteLine($"{material.Key} {material.Value.Name}");
                        writer.WriteLine($"ABSORPTION={FormatArray(material.Value.AbsorptionCoefficients)}");
                        writer.WriteLine($"SCATTERING={FormatArray(material.Value.ScatteringCoefficients)}");
                        writer.WriteLine($"TRANSMISSION={FormatArray(material.Value.TransmissionCoefficients)}");
                        writer.WriteLine("[END]");
                    }
                    writer.WriteLine();

                    // Write objects
                    foreach (var obj in modelObjects)
                    {
                        writer.WriteLine($"[OBJECT {obj.MaterialId}]");
                        foreach (var poly in obj.Polygons)
                        {
                            string polyLine = $"POLY {string.Join(",", poly.VertexIds)}";

                            if (poly.IsCurved)
                            {
                                polyLine += $" CURVE={poly.Kurvature[0]},{poly.Kurvature[1]}";
                                polyLine += $" FRAME={FormatVector(poly.FrameAxes[0])},{FormatVector(poly.FrameAxes[1])}";
                            }

                            writer.WriteLine(polyLine);
                        }
                        writer.WriteLine();
                    }
                }
            }

            private bool IsValidGeometry(RhinoObject obj)
            {
                return obj.ObjectType == ObjectType.Brep ||
                       obj.ObjectType == ObjectType.Mesh ||
                       obj.ObjectType == ObjectType.Surface ||
                       obj.ObjectType == ObjectType.Extrusion;
            }

            private Mesh[] CreateMeshes(RhinoObject obj, double meshQuality)
            {
                MeshingParameters mp = new MeshingParameters();
                mp.MinimumEdgeLength = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 2;
                mp.MaximumEdgeLength = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 100 / meshQuality;
                mp.JaggedSeams = false;
                mp.RefineGrid = true;
                mp.SimplePlanes = false;
                mp.RelativeTolerance = 0.1 / meshQuality; // Adjust based on quality

                Rhino.Geometry.GeometryBase geo = obj.Geometry;

                if (geo is Mesh mesh)
                {
                    // Clone the mesh to avoid modifying the original
                    Mesh meshCopy = mesh.DuplicateMesh();
                    meshCopy.Faces.ConvertQuadsToTriangles();
                    return new Mesh[] { meshCopy };
                }
                else if (geo is Brep)
                {
                    return Mesh.CreateFromBrep(geo as Brep, mp);
                }
                else if (geo is Surface surface)
                {
                    Brep brep = surface.ToBrep();
                    return Mesh.CreateFromBrep(brep, mp);
                }
                else if (geo is Extrusion extrusion)
                {
                    Brep brep = extrusion.ToBrep();
                    return Mesh.CreateFromBrep(brep, mp);
                }

                return new Mesh[0];
            }

            private void CalculateCurvature(Mesh mesh, int faceIndex, ref PolygonDefinition polyDef)
            {
                MeshFace face = mesh.Faces[faceIndex];

                // Try to estimate curvature from normals
                Vector3d n0 = mesh.Normals[face.A];
                Vector3d n1 = mesh.Normals[face.B];
                Vector3d n2 = mesh.Normals[face.C];

                // Average normal
                Vector3d avgNormal = (n0 + n1 + n2) / 3.0;
                avgNormal.Unitize();

                // Check if this is a curved face (normals differ significantly)
                double angleVariation = Math.Max(
                    Math.Max(Vector3d.VectorAngle(n0, n1), Vector3d.VectorAngle(n1, n2)),
                    Vector3d.VectorAngle(n2, n0));

                if (angleVariation > 0.1) // About 5.7 degrees
                {
                    polyDef.IsCurved = true;

                    // Calculate principal curvature directions
                    // This is a simplification - we use the face edges as the frame axes
                    Point3d p0 = mesh.Vertices[face.A];
                    Point3d p1 = mesh.Vertices[face.B];
                    Point3d p2 = mesh.Vertices[face.C];

                    Vector3d edge1 = p1 - p0;
                    Vector3d edge2 = p2 - p0;

                    edge1.Unitize();
                    edge2.Unitize();

                    // Make sure the second axis is perpendicular to the first
                    Vector3d axis2 = Vector3d.CrossProduct(avgNormal, edge1);
                    axis2.Unitize();

                    // Estimate curvature values - this is approximate
                    // Higher values for more curved surfaces
                    polyDef.Kurvature[0] = angleVariation * 2; // Primary curvature
                    polyDef.Kurvature[1] = angleVariation; // Secondary curvature

                    // Set frame axes
                    polyDef.FrameAxes[0] = ToHareVector(edge1);
                    polyDef.FrameAxes[1] = ToHareVector(axis2);
                }
            }

            private MaterialDefinition CreateMaterialFromLayer(Layer layer)
            {
                // Default values for materials
                MaterialDefinition material = new MaterialDefinition
                {
                    Name = layer.Name,
                    AbsorptionCoefficients = new double[8] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 },
                    ScatteringCoefficients = new double[8] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 },
                    TransmissionCoefficients = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 }
                };

                // Try to get material properties from user data
                // In a real implementation, you would extract these from your custom material database or layer user data

                return material;
            }

            private string FormatPoint(Point3d pt)
            {
                return $"{pt.X} {pt.Y} {pt.Z}";
            }

            private string FormatVector(Hare.Geometry.Vector v)
            {
                return $"{v.dx},{v.dy},{v.dz}";
            }

            private string FormatArray(double[] values)
            {
                return string.Join(",", values);
            }

            private Hare.Geometry.Vector ToHareVector(Vector3d v)
            {
                return new Hare.Geometry.Vector(v.X, v.Y, v.Z);
            }

            private class MaterialDefinition
            {
                public string Name { get; set; }
                public double[] AbsorptionCoefficients { get; set; }
                public double[] ScatteringCoefficients { get; set; }
                public double[] TransmissionCoefficients { get; set; }
            }

            private class PolygonDefinition
            {
                public List<int> VertexIds { get; set; }
                public bool IsCurved { get; set; }
                public double[] Kurvature { get; set; }
                public Hare.Geometry.Vector[] FrameAxes { get; set; }
            }

            private class ObjectDefinition
            {
                public int MaterialId { get; set; }
                public List<PolygonDefinition> Polygons { get; set; }
            }
        }
    }
}