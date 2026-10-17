//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)
//'Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc.
//'This file is distributed under the GNU General Public License, version 3 or later.

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
using System.Globalization;
using DrawingColor = System.Drawing.Color;

namespace Pachyderm_Acoustic.UI
{
    [System.Runtime.InteropServices.Guid("73639507-B99F-4F94-8873-B7380B22F532")]
    public class PachLineArraySourceCommand : Rhino.Commands.Command
    {
        private static Pach_LineArrayBuilder Builder;
        public override string EnglishName { get { return "Insert_Line_Array_Source"; } }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (Builder != null) { Builder.BringToFront(); return Result.Success; }
            ObjRef[] selected;
            Result result = RhinoGet.GetMultipleObjects("Select speaker sources to use as line-array module templates", false, ObjectType.Point, out selected);
            if (result != Result.Success) return result;
            List<Guid> ids = new List<Guid>();
            foreach (ObjRef reference in selected)
            {
                RhinoObject obj = reference.Object();
                if (obj != null && obj.Attributes.Name == "Acoustical Source" && !ids.Contains(obj.Id)) ids.Add(obj.Id);
            }
            if (ids.Count == 0) { RhinoApp.WriteLine("No acoustical source templates selected."); return Result.Cancel; }

            Point3d top;
            result = RhinoGet.GetPoint("Select top rigging point (source center if the first template has no cabinet)", false, out top);
            if (result != Result.Success) return result;
            Vector3d forward;
            using (GetPoint aim = new GetPoint())
            {
                aim.SetCommandPrompt("Select initial forward direction of line array");
                aim.SetBasePoint(top, true);
                aim.DrawLineFromPoint(top, true);
                if (aim.Get() != GetResult.Point) return Result.Cancel;
                forward = aim.Point() - top;
            }
            if (!forward.Unitize()) return Result.Cancel;
            Vector3d up = Vector3d.ZAxis - forward * (forward * Vector3d.ZAxis);
            if (!up.Unitize()) { RhinoApp.WriteLine("Choose a direction that is not vertical."); return Result.Cancel; }
            Builder = new Pach_LineArrayBuilder(doc, ids, top, forward, up);
            Builder.Owner = Rhino.UI.RhinoEtoApp.MainWindow;
            Builder.Closed += (s, e) => Builder = null;
            Builder.Show();
            return Result.Success;
        }
    }

    internal class Pach_LineArrayBuilder : Form
    {
        private class Template
        {
            public Guid Id;
            public string Name;
            public Mesh Mesh;
            public List<Line> Lines = new List<Line>();
            public BoundingBox Bounds = BoundingBox.Empty;
            private readonly List<Point3d> Profile = new List<Point3d>();
            private string GeometryCode;
            public void Refresh(RhinoObject obj)
            {
                string code = obj.Geometry.GetUserString("CLF_CabinetPoints") + "\n" + obj.Geometry.GetUserString("CLF_CabinetFaces") + "\n" + obj.Geometry.GetUserString("CLF_CabinetLines");
                if (code == GeometryCode) return;
                GeometryCode = code;
                Mesh?.Dispose(); Mesh = null; Lines.Clear(); Profile.Clear(); Bounds = BoundingBox.Empty;
                Cabinet_Geometry cabinet = Cabinet_Geometry_Parser.Parse(obj.Geometry.GetUserString("CLF_CabinetPoints"), obj.Geometry.GetUserString("CLF_CabinetFaces"), obj.Geometry.GetUserString("CLF_CabinetLines"));
                if (cabinet != null)
                {
                    // Keep template coordinates in meters; Rebuild scales them to document units.
                    if (cabinet.HasMesh) Mesh = SourceConduit.BuildRhinoCabinetMesh(cabinet);
                    foreach (Hare.Geometry.Point p in cabinet.Vertices)
                    {
                        Point3d point = Utilities.RCPachTools.HPttoRPt(p);
                        Bounds.Union(point); Profile.Add(point);
                    }
                    foreach (Cabinet_Line line in cabinet.Lines)
                    {
                        Line wire = new Line(Utilities.RCPachTools.HPttoRPt(line.A), Utilities.RCPachTools.HPttoRPt(line.B));
                        Lines.Add(wire);
                        Bounds.Union(wire.From); Bounds.Union(wire.To);
                        Profile.Add(wire.From); Profile.Add(wire.To);
                    }
                }
                // A convex side envelope includes bevels and slopes and conservatively closes recesses.
                // Splay is about local X, so projection onto Y/Z also covers geometry away from the center slice.
                Profile.Sort((a, b) => a.Y == b.Y ? a.Z.CompareTo(b.Z) : a.Y.CompareTo(b.Y));
                List<Point3d> points = new List<Point3d>();
                double tolerance = Bounds.IsValid ? Math.Max(1E-9, Bounds.Diagonal.Length * 1E-9) : 1E-9;
                foreach (Point3d point in Profile)
                    if (points.Count == 0 || Math.Abs(point.Y - points[points.Count - 1].Y) > tolerance || Math.Abs(point.Z - points[points.Count - 1].Z) > tolerance)
                        points.Add(point);
                Profile.Clear();
                for (int pass = 0; pass < 2; pass++)
                {
                    List<Point3d> half = new List<Point3d>();
                    for (int j = 0; j < points.Count; j++)
                    {
                        Point3d point = points[pass == 0 ? j : points.Count - 1 - j];
                        while (half.Count >= 2)
                        {
                            Point3d a = half[half.Count - 2], b = half[half.Count - 1];
                            double cross = (b.Y - a.Y) * (point.Z - a.Z) - (b.Z - a.Z) * (point.Y - a.Y);
                            if (cross > tolerance * tolerance) break;
                            half.RemoveAt(half.Count - 1);
                        }
                        half.Add(point);
                    }
                    for (int j = 0; j + 1 < half.Count; j++)
                        Profile.Add(new Point3d((Bounds.Min.X + Bounds.Max.X) * 0.5, half[j].Y, half[j].Z));
                }
            }
            public bool HasPivots { get { return Profile.Count >= 3 && Bounds.IsValid && Bounds.Max.Z - Bounds.Min.Z > 1E-6; } }

            // With a lower template, select clear contact points at the requested splay; the contact may change at a face transition.
            // Both outputs are in their respective cabinet's local meter coordinates; no world tilt enters this test.
            public bool Pivot(bool top, int reference, out Point3d pivot, out Point3d lowerPivot, Template below = null, double splay = 0, double? depthFraction = null)
            {
                pivot = lowerPivot = Point3d.Origin;
                if (!HasPivots) return false;
                double tolerance = Math.Max(1E-8, Bounds.Diagonal.Length * 1E-8);
                double fraction = depthFraction ?? (reference == 0 ? 1 : reference == 1 ? 0.5 : 0);
                double y = Bounds.Min.Y + fraction * (Bounds.Max.Y - Bounds.Min.Y);
                double z = top ? double.NegativeInfinity : double.PositiveInfinity;
                // Intersect the preferred Y with the actual envelope instead of inventing a bounding-box corner.
                for (int i = 0; i < Profile.Count; i++)
                {
                    Point3d a = Profile[i], b = Profile[(i + 1) % Profile.Count];
                    if (Math.Abs(b.Y - a.Y) <= tolerance)
                    {
                        if (Math.Abs(y - a.Y) <= tolerance) z = top ? Math.Max(z, Math.Max(a.Z, b.Z)) : Math.Min(z, Math.Min(a.Z, b.Z));
                    }
                    else if (y >= Math.Min(a.Y, b.Y) - tolerance && y <= Math.Max(a.Y, b.Y) + tolerance)
                    {
                        double value = a.Z + (b.Z - a.Z) * Math.Max(0, Math.Min(1, (y - a.Y) / (b.Y - a.Y)));
                        z = top ? Math.Max(z, value) : Math.Min(z, value);
                    }
                }
                if (double.IsInfinity(z) || double.IsNaN(z)) return false;
                pivot = new Point3d((Bounds.Min.X + Bounds.Max.X) * 0.5, y, z);
                if (below == null) return true;
                if (top || !below.HasPivots || double.IsNaN(splay) || double.IsInfinity(splay) || Math.Abs(splay) > 90) return false;
                Point3d unused;
                if (!below.Pivot(true, reference, out lowerPivot, out unused)) return false;
                Point3d preferred = pivot, lowerPreferred = lowerPivot;
                tolerance = Math.Max(tolerance, below.Bounds.Diagonal.Length * 1E-8);
                // Pair matching depths, so a blocked front joint cannot turn into a front-to-rear offset.
                List<double> candidates = new List<double> { fraction };
                for (int shape = 0; shape < 2; shape++)
                {
                    Template template = shape == 0 ? this : below;
                    double depth = template.Bounds.Max.Y - template.Bounds.Min.Y;
                    if (depth <= tolerance) return false;
                    foreach (Point3d point in template.Profile)
                    {
                        double candidate = (point.Y - template.Bounds.Min.Y) / depth;
                        if (!candidates.Exists(value => Math.Abs(value - candidate) <= 1E-9)) candidates.Add(candidate);
                    }
                }
                double angle = splay * Math.PI / 180.0, cosine = Math.Cos(angle), sine = Math.Sin(angle);
                List<Vector3d> normals = new List<Vector3d> { Vector3d.ZAxis };
                // Separating-line candidates come from both profiles, including the lower profile's rotated edges.
                for (int shape = 0; shape < 3; shape++)
                {
                    List<Point3d> profile = shape == 0 ? Profile : below.Profile;
                    for (int i = 0; i < profile.Count; i++)
                    {
                        Vector3d edge = profile[(i + 1) % profile.Count] - profile[i];
                        Vector3d normal = new Vector3d(0, -edge.Z, edge.Y);
                        if (shape == 2) normal = new Vector3d(0, normal.Y * cosine + normal.Z * sine, -normal.Y * sine + normal.Z * cosine);
                        if (normal.Z < 0) normal = -normal;
                        // A vertical separator would place cabinets side by side rather than one below the other.
                        if (normal.Unitize() && normal.Z > 1E-8) normals.Add(normal);
                    }
                }
                double best = double.PositiveInfinity;
                foreach (double candidate in candidates)
                {
                    Point3d upper, lower;
                    if (!Pivot(false, reference, out upper, out unused, null, 0, candidate) ||
                        !below.Pivot(true, reference, out lower, out unused, null, 0, candidate)) continue;
                    double score = upper.DistanceToSquared(preferred) + lower.DistanceToSquared(lowerPreferred);
                    if (score >= best) continue;
                    foreach (Vector3d normal in normals)
                    {
                        bool clear = true;
                        foreach (Point3d point in Profile)
                            if ((point - upper) * normal < -tolerance) { clear = false; break; }
                        if (!clear) continue;
                        foreach (Point3d point in below.Profile)
                        {
                            Vector3d delta = point - lower;
                            double a = normal.Y * delta.Y + normal.Z * delta.Z;
                            double b = normal.Y * delta.Z - normal.Z * delta.Y;
                            double maximum = a * cosine + b * sine;
                            if (maximum > tolerance) { clear = false; break; }
                        }
                        if (clear) { best = score; pivot = upper; lowerPivot = lower; break; }
                    }
                }
                return !double.IsInfinity(best);
            }
        }

        private class Module
        {
            public Template Speaker;
            public int Number { get; set; }
            public bool Auto = true;
            public double Spacing = 0.25;
            public double Splay;
            public string SpeakerName { get { return Speaker.Name; } }
            public string Placement { get { return Auto ? "Cabinet / Auto" : "Manual"; } }
            public string Pitch { get { return Auto ? "Auto" : Spacing.ToString("0.###", CultureInfo.InvariantCulture); } }
            public string Angle { get { return Splay.ToString("0.##", CultureInfo.InvariantCulture); } }
        }

        private readonly RhinoDoc Doc;
        private readonly List<Template> Templates = new List<Template>();
        private readonly List<Module> Modules = new List<Module>();
        private readonly Point3d Top;
        private readonly Vector3d Forward, Up;
        private readonly double Units;
        private readonly LineArrayPreviewConduit Preview;
        private readonly GridView Grid;
        private readonly DropDown Speaker, Placement, Rigging;
        private readonly NumericStepper Spacing, Splay, Copies;
        private readonly TextBox ArrayLabel;
        private readonly Label Status;
        private readonly Button Create;
        private bool Loading;
        private bool DocumentClosing;

        internal Pach_LineArrayBuilder(RhinoDoc doc, List<Guid> ids, Point3d top, Vector3d forward, Vector3d up)
        {
            Doc = doc; Top = top; Forward = forward; Up = up;
            Units = 1.0 / Utilities.RCPachTools.GetModelToMetersScale(doc);
            Title = "Pachyderm Line Array Builder";
            ClientSize = new Size(820, 700);
            MinimumSize = new Size(740, 600);
            foreach (Guid id in ids)
            {
                RhinoObject obj = doc.Objects.FindId(id);
                if (obj == null || obj.Geometry == null) continue;
                Template template = new Template { Id = id, Name = obj.Geometry.GetUserString("Model") };
                if (string.IsNullOrWhiteSpace(template.Name)) template.Name = "Speaker";
                template.Name += " (" + (Templates.Count + 1).ToString(CultureInfo.InvariantCulture) + ")";
                template.Refresh(obj);
                Templates.Add(template);
                Modules.Add(new Module { Speaker = template, Auto = template.HasPivots, Spacing = ArraySourceConstruction.TemplatePitch(obj) });
            }
            Preview = new LineArrayPreviewConduit(doc.RuntimeSerialNumber);
            ArrayLabel = new TextBox { Text = ArraySourceConstruction.NextArrayLabel(doc), Width = 100 };
            Rigging = new DropDown();
            Rigging.Items.Add("Front"); Rigging.Items.Add("Center"); Rigging.Items.Add("Rear"); Rigging.SelectedIndex = 0;
            Grid = new GridView { AllowMultipleSelection = false, Height = 200 };
            Grid.Columns.Add(new GridColumn { Editable = false, HeaderText = "#", DataCell = new TextBoxCell { Binding = Binding.Property<Module, string>(r => r.Number.ToString(CultureInfo.InvariantCulture)) }, Width = 40 });
            Grid.Columns.Add(new GridColumn { Editable = false, HeaderText = "Speaker", DataCell = new TextBoxCell { Binding = Binding.Property<Module, string>(r => r.SpeakerName) }, Width = 270 });
            Grid.Columns.Add(new GridColumn { Editable = false, HeaderText = "Placement below", DataCell = new TextBoxCell { Binding = Binding.Property<Module, string>(r => r.Placement) }, Width = 135 });
            Grid.Columns.Add(new GridColumn { Editable = false, HeaderText = "Spacing (m)", DataCell = new TextBoxCell { Binding = Binding.Property<Module, string>(r => r.Pitch) }, Width = 100 });
            Grid.Columns.Add(new GridColumn { Editable = false, HeaderText = "Splay below (deg)", DataCell = new TextBoxCell { Binding = Binding.Property<Module, string>(r => r.Angle) }, Width = 130 });
            Speaker = new DropDown { Width = 250 };
            foreach (Template template in Templates) Speaker.Items.Add(template.Name);
            Placement = new DropDown { Width = 135 };
            Placement.Items.Add("Cabinet / Auto"); Placement.Items.Add("Manual");
            Spacing = new NumericStepper { MinValue = 0.001, MaxValue = 100, DecimalPlaces = 4, Increment = 0.005, Width = 90 };
            Splay = new NumericStepper { MinValue = -90, MaxValue = 90, DecimalPlaces = 2, Increment = 0.5, Width = 90 };
            Copies = new NumericStepper { MinValue = 1, MaxValue = 256, DecimalPlaces = 0, Value = 4, Width = 60 };
            Status = new Label { Wrap = WrapMode.Word };
            Create = new Button { Text = "Create Array" };
            Button cancel = new Button { Text = "Cancel" };
            DynamicLayout layout = new DynamicLayout { Padding = 10, DefaultSpacing = new Size(6, 8) };
            layout.AddRow(new Label { Text = "Array label" }, ArrayLabel, new Label { Text = "Rigging reference" }, Rigging, null);
            layout.AddRow(new Label { Text = "Top to bottom. Select a cabinet row to edit its speaker and connection below." });
            layout.Add(Grid, yscale: true);
            DynamicLayout edit = new DynamicLayout { DefaultSpacing = new Size(6, 6) };
            edit.AddRow(new Label { Text = "Speaker" }, Speaker, new Label { Text = "Placement below" }, Placement, null);
            edit.AddRow(new Label { Text = "Manual spacing (m)" }, Spacing, new Label { Text = "Splay below (deg)" }, Splay, null);
            layout.AddRow(edit);
            DynamicLayout buttons = new DynamicLayout { DefaultSpacing = new Size(6, 6) };
            Action<string, Action> AddButton = (text, action) => { Button button = new Button { Text = text }; button.Click += (s, e) => action(); buttons.AddColumn(button); };
            buttons.BeginHorizontal();
            AddButton("Add", () => InsertCopies(1, false));
            AddButton("Duplicate", () => InsertCopies(1, true));
            AddButton("Remove", Remove);
            AddButton("Move Up", () => Move(-1));
            AddButton("Move Down", () => Move(1));
            buttons.EndHorizontal();
            layout.AddRow(buttons);
            Button addCopies = new Button { Text = "Add Copies" };
            addCopies.Click += (s, e) => InsertCopies((int)Copies.Value, true);
            layout.AddRow(addCopies, Copies, null);
            layout.AddRow(new Label { Text = "Auto uses cabinet profiles and moves blocked pivots to a clear corner. Manual pitch follows the current cabinet's downward axis.\nPositive splay aims the next cabinet farther down. The last row's connection is unused." });
            layout.AddRow(Status);
            layout.AddRow(Create, cancel);
            Content = layout;
            Grid.SelectionChanged += (s, e) => LoadSelection();
            Speaker.SelectedIndexChanged += (s, e) => EditSelection();
            Placement.SelectedIndexChanged += (s, e) => EditSelection();
            Spacing.ValueChanged += (s, e) => EditSelection();
            Splay.ValueChanged += (s, e) => EditSelection();
            Rigging.SelectedIndexChanged += (s, e) => Rebuild();
            ArrayLabel.TextChanged += (s, e) => Rebuild();
            Create.Click += (s, e) => CreateArray();
            cancel.Click += (s, e) => Close();
            Closed += (s, e) =>
            {
                RhinoDoc.CloseDocument -= DocumentClosed;
                Preview.Enabled = false; Preview.Clear();
                foreach (Template template in Templates) template.Mesh?.Dispose();
                if (!DocumentClosing) Doc.Views.Redraw();
            };
            RhinoDoc.CloseDocument += DocumentClosed;
            Preview.Enabled = true;
            RefreshRows(0);
        }

        private void DocumentClosed(object sender, DocumentEventArgs e)
        {
            if (e.Document.RuntimeSerialNumber == Doc.RuntimeSerialNumber) { DocumentClosing = true; Close(); }
        }

        private void RefreshRows(int selected)
        {
            Loading = true;
            for (int i = 0; i < Modules.Count; i++) Modules[i].Number = i + 1;
            Grid.DataStore = null; Grid.DataStore = Modules;
            Grid.SelectedRow = Modules.Count == 0 ? -1 : Math.Max(0, Math.Min(selected, Modules.Count - 1));
            Loading = false;
            LoadSelection();
        }

        private void LoadSelection()
        {
            if (Loading) return;
            int index = Grid.SelectedRow;
            bool selected = index >= 0 && index < Modules.Count;
            Speaker.Enabled = Placement.Enabled = Splay.Enabled = selected;
            Loading = true;
            if (selected)
            {
                Module row = Modules[index];
                Speaker.SelectedIndex = Templates.IndexOf(row.Speaker);
                Placement.SelectedIndex = row.Auto ? 0 : 1;
                Spacing.Value = row.Spacing; Splay.Value = row.Splay;
                Spacing.Enabled = !row.Auto;
            }
            else Spacing.Enabled = false;
            Loading = false;
            Rebuild();
        }

        private void EditSelection()
        {
            int index = Grid.SelectedRow;
            if (Loading || index < 0 || index >= Modules.Count || Speaker.SelectedIndex < 0) return;
            Module row = Modules[index];
            row.Speaker = Templates[Speaker.SelectedIndex];
            row.Auto = Placement.SelectedIndex == 0;
            row.Spacing = Spacing.Value; row.Splay = Splay.Value;
            RefreshRows(index);
        }

        private void InsertCopies(int count, bool duplicate)
        {
            if (Modules.Count + count > 256) { Status.Text = "An array can contain at most 256 cabinets."; return; }
            int selected = Grid.SelectedRow;
            Module source = selected >= 0 && selected < Modules.Count ? Modules[selected] : null;
            Template template = source == null ? Templates[0] : source.Speaker;
            int insert = source == null ? Modules.Count : selected + 1;
            for (int i = 0; i < count; i++) Modules.Insert(insert + i, new Module { Speaker = template, Auto = duplicate && source != null ? source.Auto : template.HasPivots, Spacing = source == null ? 0.25 : source.Spacing, Splay = duplicate && source != null ? source.Splay : 0 });
            RefreshRows(insert);
        }

        private void Remove()
        {
            int index = Grid.SelectedRow;
            if (index < 0 || index >= Modules.Count) return;
            Modules.RemoveAt(index); RefreshRows(index);
        }

        private void Move(int direction)
        {
            int index = Grid.SelectedRow, next = index + direction;
            if (index < 0 || next < 0 || next >= Modules.Count) return;
            Module row = Modules[index]; Modules.RemoveAt(index); Modules.Insert(next, row); RefreshRows(next);
        }

        private void Rebuild()
        {
            Preview.Clear();
            string error = null;
            if (Modules.Count == 0) error = "Add at least one cabinet.";
            else if (string.IsNullOrWhiteSpace(ArrayLabel.Text)) error = "Enter an array label.";
            Vector3d right = Vector3d.CrossProduct(Forward, Up);
            Transform frame = Transform.PlaneToPlane(Plane.WorldXY, new Plane(Point3d.Origin, right, Forward));
            Transform scale = Transform.Scale(Point3d.Origin, Units);
            double pitch = 0;
            for (int i = 0; i < Modules.Count; i++)
            {
                Module row = Modules[i];
                RhinoObject current = Doc.Objects.FindId(row.Speaker.Id);
                if (current == null || current.Geometry == null) error = "A speaker template was deleted. Cancel and select the available templates again.";
                else row.Speaker.Refresh(current);
                Transform rotation = frame * Transform.Rotation(-pitch * Math.PI / 180.0, Vector3d.XAxis, Point3d.Origin);
                Transform transform = rotation * scale;
                Point3d position;
                Point3d connection;
                if (i == 0)
                {
                    Point3d pivot, unused;
                    row.Speaker.Pivot(true, Rigging.SelectedIndex, out pivot, out unused);
                    pivot.Transform(transform);
                    position = Top - (Vector3d)pivot; connection = Top;
                }
                else
                {
                    Module above = Modules[i - 1];
                    LineArrayPreviewConduit.Item previous = Preview.Items[i - 1];
                    if (above.Auto && above.Speaker.HasPivots && row.Speaker.HasPivots)
                    {
                        Point3d pivot;
                        if (!above.Speaker.Pivot(false, Rigging.SelectedIndex, out connection, out pivot, row.Speaker, above.Splay))
                            error = "No clear cabinet pivot below row " + i + ". Reduce splay or choose Manual spacing.";
                        connection.Transform(previous.Transform);
                        pivot.Transform(transform);
                        position = connection - (Vector3d)pivot;
                    }
                    else
                    {
                        if (above.Auto) error = "Connection below cabinet " + i + " has no usable cabinet geometry. Choose Manual spacing for that row.";
                        connection = previous.Position;
                        position = previous.Position - previous.Up * (above.Spacing * Units);
                    }
                }
                transform = Transform.Translation((Vector3d)position) * transform;
                Vector3d forward = Vector3d.YAxis, up = Vector3d.ZAxis;
                forward.Transform(rotation); up.Transform(rotation);
                LineArrayPreviewConduit.Item item = new LineArrayPreviewConduit.Item { Transform = transform, Position = position, Forward = forward, Up = up, Connection = connection, Number = i + 1 };
                if (row.Speaker.Mesh != null) { item.Mesh = row.Speaker.Mesh.DuplicateMesh(); item.Mesh.Transform(transform); }
                foreach (Line wire in row.Speaker.Lines) { Line line = wire; line.Transform(transform); item.Lines.Add(line); }
                if (i > 0)
                {
                    Module above = Modules[i - 1];
                    double distance = position.DistanceTo(Preview.Items[i - 1].Position) / Units;
                    item.SpacingLabel = (above.Auto ? "Auto: " : "") + distance.ToString("0.###", CultureInfo.InvariantCulture) + " m";
                    item.Splay = above.Splay;
                }
                Preview.Items.Add(item);
                pitch += row.Splay;
            }
            Preview.Selected = Grid.SelectedRow;
            Create.Enabled = error == null;
            Status.Text = error ?? (Modules.Count + " cabinets. Profile pivots checked at each connection. Preview matches Create Array.");
            Preview.Valid = error == null;
            Doc.Views.Redraw();
        }

        private void CreateArray()
        {
            if (RhinoDoc.ActiveDoc == null || RhinoDoc.ActiveDoc.RuntimeSerialNumber != Doc.RuntimeSerialNumber)
            { Status.Text = "Activate the Rhino document where this builder was opened."; return; }
            Rebuild();
            if (!Create.Enabled) return;
            List<Guid> ids = new List<Guid>();
            List<RhinoObject> elements = new List<RhinoObject>();
            Guid group = Guid.NewGuid();
            string label = ArrayLabel.Text.Trim();
            uint undo = Doc.BeginUndoRecord("Create Pachyderm line array");
            bool success = false;
            try
            {
                for (int i = 0; i < Modules.Count; i++)
                {
                    Module row = Modules[i];
                    LineArrayPreviewConduit.Item item = Preview.Items[i];
                    RhinoObject template = Doc.Objects.FindId(row.Speaker.Id);
                    if (template == null || template.Geometry == null) throw new InvalidOperationException("A speaker template is unavailable.");
                    Guid id = Doc.Objects.AddPoint(item.Position);
                    if (id == Guid.Empty) throw new InvalidOperationException("Could not create a source point.");
                    ids.Add(id);
                    RhinoObject obj = Doc.Objects.FindId(id);
                    ArraySourceConstruction.CopyTemplateSource(template, obj);
                    ArraySourceConstruction.AssignArrayMetadata(obj, group, label, i, "Line");
                    obj.Geometry.SetUserString("Aiming", ArraySourceConstruction.AimingString(item.Forward, item.Up));
                    obj.Geometry.SetUserString("ArrayCabinetOwner", "True");
                    obj.Geometry.SetUserString("ArrayConstructionVersion", "2");
                    obj.Geometry.SetUserString("ArrayTemplateId", row.Speaker.Id.ToString());
                    obj.Geometry.SetUserString("ArraySpacingMode", row.Auto ? "Cabinet" : "Manual");
                    obj.Geometry.SetUserString("ArrayManualSpacing_m", row.Spacing.ToString(CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArraySplayBelowDeg", row.Splay.ToString(CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArrayRiggingReference", Rigging.Items[Rigging.SelectedIndex].Text);
                    obj.Geometry.SetUserString("ArrayElementSpacing_m", i + 1 < Modules.Count ? (item.Position.DistanceTo(Preview.Items[i + 1].Position) / Units).ToString(CultureInfo.InvariantCulture) : "0");
                    obj.Geometry.SetUserString("ArrayTopPoint", (Top.X / Units).ToString(CultureInfo.InvariantCulture) + ";" + (Top.Y / Units).ToString(CultureInfo.InvariantCulture) + ";" + (Top.Z / Units).ToString(CultureInfo.InvariantCulture));
                    obj.Geometry.SetUserString("ArrayInitialAiming", ArraySourceConstruction.AimingString(Forward, Up));
                    if (!Doc.Objects.ModifyAttributes(obj, obj.Attributes, true) || !obj.CommitChanges()) throw new InvalidOperationException("Could not save a source's metadata.");
                    obj = Doc.Objects.FindId(id);
                    if (obj == null) throw new InvalidOperationException("Could not retrieve the created source.");
                    elements.Add(obj);
                }
                ArraySourceConstruction.AddRhinoGroup(Doc, "Pachyderm Line Array " + label, ids);
                success = true;
            }
            catch (Exception ex)
            {
                foreach (Guid id in ids) Doc.Objects.Delete(id, true);
                Status.Text = "Array creation failed: " + ex.Message;
                RhinoApp.WriteLine(Status.Text);
            }
            finally { if (undo != 0) Doc.EndUndoRecord(undo); }
            if (!success) { Doc.Views.Redraw(); return; }
            foreach (RhinoObject obj in elements) ArraySourceConstruction.AddToSourceConduit(obj);
            Doc.Objects.UnselectAll();
            foreach (RhinoObject obj in elements) obj.Select(true);
            Close();
            Doc.Views.Redraw();
            RhinoApp.WriteLine("Created line array {0} with {1} cabinets.", label, elements.Count);
            new Pach_ArrayControl(elements).Show();
        }
    }

    internal class LineArrayPreviewConduit : Rhino.Display.DisplayConduit
    {
        internal class Item
        {
            internal Transform Transform;
            internal Point3d Position, Connection;
            internal Vector3d Forward, Up;
            internal Mesh Mesh;
            internal List<Line> Lines = new List<Line>();
            internal int Number;
            internal double Splay;
            internal string SpacingLabel;
        }
        internal readonly List<Item> Items = new List<Item>();
        internal int Selected;
        internal bool Valid;
        private readonly uint Document;
        internal LineArrayPreviewConduit(uint document) { Document = document; }
        internal void Clear() { foreach (Item item in Items) item.Mesh?.Dispose(); Items.Clear(); }

        protected override void CalculateBoundingBox(Rhino.Display.CalculateBoundingBoxEventArgs e)
        {
            if (e.RhinoDoc == null || e.RhinoDoc.RuntimeSerialNumber != Document) return;
            BoundingBox bounds = BoundingBox.Empty;
            foreach (Item item in Items)
            {
                bounds.Union(item.Position); bounds.Union(item.Connection);
                if (item.Mesh != null) bounds.Union(item.Mesh.GetBoundingBox(true));
                foreach (Line line in item.Lines) { bounds.Union(line.From); bounds.Union(line.To); }
            }
            if (bounds.IsValid) e.IncludeBoundingBox(bounds);
        }

        protected override void PostDrawObjects(Rhino.Display.DrawEventArgs e)
        {
            if (e.RhinoDoc == null || e.RhinoDoc.RuntimeSerialNumber != Document) return;
            double units = 1.0 / Utilities.RCPachTools.GetModelToMetersScale(e.RhinoDoc);
            for (int i = 0; i < Items.Count; i++)
            {
                Item item = Items[i];
                DrawingColor color = !Valid ? DrawingColor.Magenta : i == Selected ? DrawingColor.Lime : DrawingColor.White;
                if (item.Mesh != null)
                {
                    using (Rhino.Display.DisplayMaterial material = new Rhino.Display.DisplayMaterial(color, 0.25)) e.Display.DrawMeshShaded(item.Mesh, material);
                    e.Display.DrawMeshWires(item.Mesh, DrawingColor.Black, i == Selected ? 5 : 3);
                    e.Display.DrawMeshWires(item.Mesh, color, i == Selected ? 3 : 1);
                }
                foreach (Line wire in item.Lines)
                {
                    e.Display.DrawLine(wire, DrawingColor.Black, i == Selected ? 5 : 3);
                    e.Display.DrawLine(wire, color, i == Selected ? 3 : 1);
                }
                e.Display.DrawPoint(item.Position, Rhino.Display.PointStyle.RoundSimple, 4, color);
                e.Display.DrawArrow(new Line(item.Position, item.Position + item.Forward * (0.25 * units)), color);
                e.Display.DrawDot(item.Position, item.Number.ToString(CultureInfo.InvariantCulture), color, DrawingColor.Black);
                if (i == 0) continue;
                Item previous = Items[i - 1];
                Vector3d right = Vector3d.CrossProduct(previous.Forward, previous.Up);
                Point3d a = previous.Position + right * (0.45 * units), b = item.Position + right * (0.45 * units);
                foreach (Line dimension in new[] { new Line(previous.Position, a), new Line(item.Position, b), new Line(a, b) })
                {
                    e.Display.DrawLine(dimension, DrawingColor.Black, 3);
                    e.Display.DrawLine(dimension, DrawingColor.White, 1);
                }
                e.Display.DrawPoint(item.Connection, Rhino.Display.PointStyle.RoundSimple, 6, DrawingColor.Black);
                e.Display.DrawPoint(item.Connection, Rhino.Display.PointStyle.RoundSimple, 4, color);
                e.Display.DrawDot((a + b) * 0.5, item.SpacingLabel, DrawingColor.White, DrawingColor.Black);
                Point3d center = item.Connection - right * (0.25 * units);
                double radius = 0.15 * units;
                e.Display.DrawLine(center, center + previous.Forward * radius, color, 1);
                e.Display.DrawLine(center, center + item.Forward * radius, color, 1);
                Point3d last = center + previous.Forward * radius;
                for (int j = 1; j <= 16; j++)
                {
                    Vector3d direction = previous.Forward;
                    direction.Rotate(-item.Splay * Math.PI / 180.0 * j / 16, right);
                    Point3d next = center + direction * radius;
                    e.Display.DrawLine(last, next, color, 2); last = next;
                }
                e.Display.DrawDot(center, item.Splay.ToString("0.##", CultureInfo.InvariantCulture) + "°", color, DrawingColor.Black);
            }
        }
    }
}
