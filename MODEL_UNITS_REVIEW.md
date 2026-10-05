# Model-space units review

The reviewed rule is **document geometry uses document units; simulation geometry uses meters**. Geometry type alone does not establish units. A Rhino Point3d can hold a private simulation point, and the cabinet parser can hold document coordinates in a Hare point.

## Conversion boundaries

| Path | Unit contract and reviewed change |
| --- | --- |
| RCPachTools legacy point/vector conversions | RPttoHPt and HPttoRPt retain coordinate-preserving behavior. They do not read the active document or scale directions. |
| Explicit document points | ModelPointToHare converts document points to meters; HarePointToModel performs the inverse. Both accept an optional owning document. |
| Mesh conversion | RhinotoHareMesh and HaretoRhinoMesh preserve coordinates. HareMeshToModel is the explicit display/export boundary. Mesh vertex reads and conversion output retain double precision where available. |
| Polygon and NURBS scenes | Private copies of constructor geometry are normalized before meshing, curvature, plane construction, and intersections. Original document geometry is not transformed. |
| NURBS scene methods | All partition overloads, bounds, intersection points, and mirror geometry use meters, including those represented with Rhino types. |
| Sources and receivers | Rhino object positions and Grasshopper point inputs become meters at construction. Acoustic coefficients, power, delays, aiming directions, and existing user-data strings retain their established units. |
| Line and track sources | Lengths become meters; physical sample spacing becomes document distance before curve subdivision. Curvature, an inverse length, is divided by meters per model unit. |
| Mapping | Physical receiver spacing becomes document distance for meshing; the newly created private mesh then becomes meters. Mesh simplification remains in meters before output conversion. |
| FDTD | Simulation centers and box dimensions use meters. Custom-section centroids enter meter space; display cell sizes and section offsets leave it. Laboratory guides use one display transform. |
| Particle paths | Private polylines and interpolation distances stay in meters. Returned display points alone become document coordinates. |
| Arrays and cabinets | Array interference uses meter positions. Unit spheres and normalized directions remain unscaled. Cabinet templates remain meters until placement; the cabinet display parser intentionally receives a Hare point in document coordinates. |
| Directivity and receiver displays | Array centers and displayed vertices use the same document space. Physical contour spacing and visibility tolerance convert at use. Receiver sphere geometry and its guides share one meter-to-document draw transform. |
| Grasshopper direct field | Input mesh points become meters before the parallel loop. Distance, phase, attenuation, and source direction calculations no longer consult the document inside that loop. Both GH versions were updated. |

## Scale and accuracy

Standard units use RhinoMath.UnitScale. Custom units use the document's meters-per-unit value, with invalid/nonfinite/nonpositive values rejected. Unitless and no-document inputs retain the existing meter convention.

Existing public conversion signatures and their raw coordinate semantics are preserved. Universal and Hare were not modified. Existing scene tolerances remain physical meter tolerances; this work does not change acoustic algorithms or promise arbitrary numerical precision for extreme coordinates.

The review deliberately leaves the unused Rhino absorption-analysis routines with their existing meter-input contract. No active caller establishes that their Rhino geometry comes from document space, so inserting active-document scaling there would be speculative.

## Validation performed

- Rhino project: net48 and net7.0 builds pass.
- Grasshopper 1 and 2 builds pass against the revised Rhino plugin.
- New tests/ModelUnits.Regression.ps1 passes: meter, millimeter, centimeter, inch, foot, and custom-unit boundaries; raw point/vector/mesh preservation; scalar and point round trips; physical distance/time/spreading invariance; owning-document override; invalid custom scales; unitless/no-document conventions; triangular and quadrilateral meshes in both welded and unwelded paths.
- These regression checks extract production conversion methods and use managed geometry adapters. They do not execute Rhino's native mesher, intersection engine, or the acoustic solver.
- Git whitespace checks pass. Rhino/GH1 builds retain 27 existing dependency warnings.
- Existing tests/ArrayPattern.Regression.ps1 cannot run: its extractor looks for "private static Vector3d DiagnosticDirection", while the current source qualifies the return type as Rhino.Geometry.Vector3d. This mismatch also predates these unit changes; that unrelated test file was left untouched.
- GH2 validation used a temporary reference override to the newly built plugin. Its bundled older plugin binary and project file were not replaced. GH1 required GenerateResourceUsePreserializedResources=true for the installed build environment.

## Native Rhino verification still required

Use geometrically equivalent copies of one room in meters, millimeters, feet, and a custom unit system, scaling geometry when changing units. Include a source, receiver, line source, mapping surface, and a curved surface.

Compare private scene bounds and areas, ray intersection distances, direct/reflected arrival times, direct-field levels, line-source sample count, and mapping receiver count. Check that balloons, mapping meshes, contours, particle paths, and FDTD slices align with the original geometry. Test a cabinet array away from the origin to expose mixed-coordinate translations.

Rhino/GH continue to rely on the existing active-document workflow in several setup and display paths. Switching documents or changing units while a simulation or cached display is active has not been validated or redesigned here; rebuild the scene and display after such a change.

All changes remain uncommitted in the Rhino and Grasshopper working trees.
