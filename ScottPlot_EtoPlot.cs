/*
ScottPlot Eto adapter.
Source: ScottPlot/src/ScottPlot5/ScottPlot5 Controls/ScottPlot.Eto/EtoPlot.cs
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
using SkiaSharp;
using Eto.Drawing;
using System.Runtime.InteropServices;

namespace ScottPlot.Eto;

public class EtoPlot : Drawable, IPlotControl
{
    public Plot Plot { get; internal set; }
    public IMultiplot Multiplot { get; set; }
    public GRContext? GRContext => null;
    public Interactivity.UserInputProcessor UserInputProcessor { get; }
    public IPlotMenu? Menu { get; set; }
    public float DisplayScale { get; set; }

    public EtoPlot()
    {
        Plot = new() { PlotControl = this };
        Multiplot = new Multiplot(Plot);
        DisplayScale = DetectDisplayScale();
        UserInputProcessor = new(this);
        Menu = new EtoPlotMenu(this);

        MouseDown += OnMouseDown;
        MouseUp += OnMouseUp;
        MouseMove += OnMouseMove;
        MouseWheel += OnMouseWheel;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        SizeChanged += (s, e) => Refresh();
    }

    public void Reset()
    {
        Plot plot = new() { PlotControl = this };
        Reset(plot);
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        oldPlot?.Dispose();
        Multiplot.Reset(Plot);
    }

    public void Refresh()
    {
        Invalidate();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    protected override void OnPaint(PaintEventArgs args)
    {
        base.OnPaint(args);

        SKImageInfo imageInfo = new((int)Bounds.Width, (int)Bounds.Height);

        using var surface = SKSurface.Create(imageInfo);
        if (surface is null)
            return;
        PixelRect rect = new(0, (float)Bounds.Width, (float)Bounds.Height, 0);
        Multiplot.Render(surface.Canvas, rect);

        SKImage img = surface.Snapshot();
        SKPixmap pixels = img.ToRasterImage().PeekPixels();
        byte[] bytes = pixels.GetPixelSpan().ToArray();

        var bmp = new Bitmap((int)Bounds.Width, (int)Bounds.Height, global::Eto.Drawing.PixelFormat.Format32bppRgba);

        using (var data = bmp.Lock())
        {
            Marshal.Copy(bytes, 0, data.Data, bytes.Length);
        }

        args.Graphics.DrawImage(bmp, 0, 0);
    }

    private void OnMouseDown(object? sender, MouseEventArgs e)
    {
        Focus();
        UserInputProcessor.ProcessMouseDown(e);
    }

    private void OnMouseUp(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseUp(e);
    }

    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseMove(e);
    }

    private void OnMouseWheel(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseWheel(e);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyDown(e);
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyUp(e);
    }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }

    public void SetCursor(Cursor cursor)
    {
        EtoMouse.SetCursor(null);
    }
}
