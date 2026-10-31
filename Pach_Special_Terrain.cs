//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)
//'
//'This file is part of Pachyderm-Acoustic.
//'
//'Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit
//'Pachyderm-Acoustic is free software; you can redistribute it and/or modify
//'it under the terms of the GNU General Public License as published
//'by the Free Software Foundation; either version 3 of the License, or
//'(at your option) any later version. See General_Public_License.txt.

using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;

namespace Pachyderm_Acoustic.UI
{
    [System.Runtime.InteropServices.Guid("9d2b8f54-0f6e-4e1b-98a3-643ec96bda71")]
    public class Pach_Special_Terrain : Rhino.Commands.Command
    {
        public override string EnglishName => "Pach_Special_Terrain";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var objects = new List<RhinoObject>();
            using (var selection = new GetObject())
            {
                selection.SetCommandPrompt("Select terrain surfaces for foliage properties");
                selection.GeometryFilter = ObjectType.Surface | ObjectType.Brep | ObjectType.Extrusion;
                selection.SubObjectSelect = false;
                selection.EnablePreSelect(true, true);
                if (selection.GetMultiple(1, 0) != GetResult.Object) return selection.CommandResult();
                for (int i = 0; i < selection.ObjectCount; i++)
                    using (ObjRef reference = selection.Object(i)) objects.Add(reference.Object());
            }

            using (var dialog = new Dialog { Title = "Pachyderm Terrain / Foliage", ClientSize = new Size(480, 640), Resizable = true })
            {
                var foliage = new FoliageProperties();
                foliage.Load_Doc(objects);
                var close = new Button { Text = "Close" };
                close.Click += (sender, e) => dialog.Close();
                dialog.AbortButton = close;
                dialog.Closing += (sender, e) => { if (!foliage.Commit()) e.Cancel = true; else foliage.EndPreview(); };
                dialog.Content = new StackLayout { Padding = 10, Spacing = 8, Items = {
                    new Label { Text = objects.Count + " surface(s). Changes save on close. Orange outline: canopy base.", Wrap = WrapMode.Word },
                    new StackLayoutItem(new Scrollable { Content = foliage }, true), close
                } };
                dialog.ShowModal(Rhino.UI.RhinoEtoApp.MainWindowForDocument(doc));
            }
            return Result.Success;
        }
    }
}
