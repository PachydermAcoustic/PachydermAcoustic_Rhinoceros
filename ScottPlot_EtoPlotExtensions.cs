/*
ScottPlot Eto adapter.
Source: ScottPlot/src/ScottPlot5/ScottPlot5 Controls/ScottPlot.Eto/EtoPlotExtensions.cs
Pachyderm packaging: relocated and renamed; executable code unchanged.

MIT License

Copyright (c) 2018 Scott Harden / Harden Technologies, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Eto.Forms;
using EtoMouse = Eto.Forms.Mouse;
using EtoCursor = Eto.Forms.Cursor;
using EtoCursors = Eto.Forms.Cursors;

namespace ScottPlot.Eto;

internal static class EtoPlotExtensions
{
    private static void SyncModifierKeys(Interactivity.UserInputProcessor processor, Keys mods)
    {
        processor.KeyState.Remove(Interactivity.StandardKeys.Alt);
        processor.KeyState.Remove(Interactivity.StandardKeys.Control);
        processor.KeyState.Remove(Interactivity.StandardKeys.Shift);

        if (mods.HasFlag(Keys.Alt)) processor.KeyState.Add(Interactivity.StandardKeys.Alt);
        if (mods.HasFlag(Keys.Control)) processor.KeyState.Add(Interactivity.StandardKeys.Control);
        if (mods.HasFlag(Keys.Shift)) processor.KeyState.Add(Interactivity.StandardKeys.Shift);
    }

    public static void ProcessMouseDown(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        SyncModifierKeys(processor, e.Modifiers);
        Pixel pixel = e.Pixel();

        Interactivity.IUserAction action = e.Buttons switch
        {
            MouseButtons.Primary => new Interactivity.UserActions.LeftMouseDown(pixel),
            MouseButtons.Middle => new Interactivity.UserActions.MiddleMouseDown(pixel),
            MouseButtons.Alternate => new Interactivity.UserActions.RightMouseDown(pixel),
            _ => new Interactivity.UserActions.Unknown(e.Buttons.ToString(), "pressed"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseUp(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        SyncModifierKeys(processor, e.Modifiers);
        Pixel pixel = e.Pixel();

        Interactivity.IUserAction action = e.Buttons switch
        {
            MouseButtons.Primary => new Interactivity.UserActions.LeftMouseUp(pixel),
            MouseButtons.Middle => new Interactivity.UserActions.MiddleMouseUp(pixel),
            MouseButtons.Alternate => new Interactivity.UserActions.RightMouseUp(pixel),
            _ => new Interactivity.UserActions.Unknown(e.Buttons.ToString(), "released"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseMove(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        SyncModifierKeys(processor, e.Modifiers);
        Pixel pixel = e.Pixel();
        Interactivity.IUserAction action = new Interactivity.UserActions.MouseMove(pixel);
        processor.Process(action);
    }

    public static void ProcessMouseWheel(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        SyncModifierKeys(processor, e.Modifiers);
        Pixel pixel = e.Pixel();

        Interactivity.IUserAction action = e.Delta.Height > 0
            ? new Interactivity.UserActions.MouseWheelUp(pixel)
            : new Interactivity.UserActions.MouseWheelDown(pixel);

        processor.Process(action);
    }

    public static void ProcessKeyDown(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyDown(key);
        processor.Process(action);
    }

    public static void ProcessKeyUp(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyUp(key);
        processor.Process(action);
    }

    public static Interactivity.Key ToKey(this KeyEventArgs e)
    {
        return e.Key switch
        {
            Keys.LeftControl => Interactivity.StandardKeys.Control,
            Keys.RightControl => Interactivity.StandardKeys.Control,
            Keys.LeftAlt => Interactivity.StandardKeys.Alt,
            Keys.RightAlt => Interactivity.StandardKeys.Alt,
            Keys.LeftShift => Interactivity.StandardKeys.Shift,
            Keys.RightShift => Interactivity.StandardKeys.Shift,
            _ => new Interactivity.Key(e.ToString() ?? string.Empty),
        };
    }

    internal static Pixel Pixel(this MouseEventArgs e)
    {
        double x = e.Location.X;
        double y = e.Location.Y;
        return new Pixel((float)x, (float)y);
    }

    public static EtoCursor GetCursor(this Cursor cursor)
    {
        // NOTE: not all cursors are available on Eto
        // https://pages.picoe.ca/docs/api/html/Properties_T_Eto_Forms_Cursors.htm

        return cursor switch
        {
            Cursor.Arrow => EtoCursors.Arrow,
            Cursor.No => EtoCursors.Arrow,
            Cursor.Wait => EtoCursors.Arrow,
            Cursor.Hand => EtoCursors.Pointer,
            Cursor.Cross => EtoCursors.Crosshair,
            Cursor.SizeAll => EtoCursors.Move,
            Cursor.SizeNorthSouth => EtoCursors.Move,
            Cursor.SizeWestEast => EtoCursors.Move,
            _ => throw new System.NotImplementedException(cursor.ToString()),
        };
    }
}
