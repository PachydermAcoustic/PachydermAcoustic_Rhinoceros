//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)
//'
//'This file is part of Pachyderm-Acoustic.
//'
//'Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc. - a 501(c)3 nonprofit
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

using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Eto.Forms;
using FFMpegCore;
using Rhino;
using Rhino.Display;

namespace Pachyderm_Acoustic.UI
{
    internal sealed class MovieCapture : IDisposable
    {
        private string outputPath;
        private string binaryFolder;
        private bool replaceExistingMovie;
        private string frameDirectory;
        private int frameCount;
        private double frameRate;
        private bool disposed;

        public Button Button { get; } = new Button { Text = "Record Movie..." };
        public bool IsRecording => frameDirectory != null;

        public async Task ToggleAsync(double framesPerSecond)
        {
            if (disposed || !Button.Enabled) return;
            if (!IsRecording)
            {
                Button.Enabled = false;
                try
                {
                    using (var dialog = new SaveFileDialog { Title = "Record movie" })
                    {
                        dialog.Filters.Add(new FileFilter("H.264 MP4", ".mp4"));
                        dialog.Filters.Add(new FileFilter("VP9 WebM", ".webm"));
                        dialog.CurrentFilterIndex = 0;
                        if (dialog.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) != DialogResult.Ok) return;

                        string extension = dialog.CurrentFilterIndex == 1 ? ".webm" : ".mp4";
                        outputPath = Path.ChangeExtension(dialog.FileName, extension);
                        replaceExistingMovie = File.Exists(outputPath);
                        if (replaceExistingMovie && MessageBox.Show(
                            "Replace the existing movie when encoding succeeds?", "Record movie",
                            MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                        string executable = Path.DirectorySeparatorChar == '\\' ? "ffmpeg.exe" : "ffmpeg";
                        string pluginFolder = Path.GetDirectoryName(typeof(MovieCapture).Assembly.Location);
                        string[] folders = new[]
                        {
                            Path.Combine(pluginFolder, "ffmpeg", "win-x64"),
                            Path.Combine(pluginFolder, "ffmpeg"),
                            pluginFolder,
                            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Pachyderm", "ffmpeg")
                        }.Concat((System.Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
                            .Split(Path.PathSeparator)).ToArray();
                        binaryFolder = folders.FirstOrDefault(folder => File.Exists(Path.Combine(folder.Trim('"'), executable)));
                        if (binaryFolder == null)
                        {
                            using (var locate = new OpenFileDialog { Title = "Locate FFmpeg to enable movie recording" })
                            {
                                locate.Filters.Add(new FileFilter("FFmpeg executable", Path.GetExtension(executable)));
                                if (locate.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) != DialogResult.Ok) return;
                                binaryFolder = Path.GetDirectoryName(locate.FileName);
                                executable = Path.GetFileName(locate.FileName);
                                if (executable != "ffmpeg.exe" && executable != "ffmpeg")
                                    throw new InvalidOperationException("Select the ffmpeg executable.");
                            }
                        }
                        binaryFolder = binaryFolder.Trim('"');
                        string encoder = extension == ".webm" ? "libvpx-vp9" : "libx264";
                        await Task.Run(async () =>
                        {
                            using (var process = Process.Start(new ProcessStartInfo
                            {
                                FileName = Path.Combine(binaryFolder, executable),
                                Arguments = "-hide_banner -encoders",
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }))
                            {
                                Task<string> errors = process.StandardError.ReadToEndAsync();
                                string encoders = await process.StandardOutput.ReadToEndAsync();
                                string details = await errors;
                                process.WaitForExit();
                                if (process.ExitCode != 0 || !encoders.Contains(encoder))
                                    throw new InvalidOperationException("FFmpeg cannot provide " + encoder + ". " + details);
                            }
                        });
                        if (disposed) return;

                        string directory = Path.Combine(Path.GetTempPath(), "PachydermMovie-" + Guid.NewGuid().ToString("N"));
                        Directory.CreateDirectory(directory);
                        frameDirectory = directory;
                        frameRate = framesPerSecond;
                        frameCount = 0;
                        Button.Text = "Finish Movie";
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(Rhino.UI.RhinoEtoApp.MainWindow,
                        "Could not start movie recording.\n" + error.Message, "Record Movie");
                }
                finally
                {
                    if (!disposed) Button.Enabled = true;
                }
                return;
            }

            string directoryToEncode = frameDirectory;
            string destination = outputPath;
            bool replaceExisting = replaceExistingMovie;
            int count = frameCount;
            double fps = frameRate;
            frameDirectory = null;
            Button.Enabled = false;
            Button.Text = "Encoding Movie...";
            bool completed = false;
            try
            {
                if (count == 0)
                {
                    MessageBox.Show(Rhino.UI.RhinoEtoApp.MainWindow, "No movie frames were captured.", "Record Movie");
                    return;
                }

                await Task.Run(() => EncodeAsync(directoryToEncode, destination, fps, count, replaceExisting, binaryFolder));
                completed = true;
                RhinoApp.WriteLine("Movie saved to " + destination);
                if (!disposed) MessageBox.Show(Rhino.UI.RhinoEtoApp.MainWindow, "Movie saved to\n" + destination, "Record Movie");
            }
            catch (Exception error)
            {
                string message = "Movie export failed.\n" + error.Message + "\nCaptured frames were kept in\n" + directoryToEncode;
                RhinoApp.WriteLine(message);
                if (!disposed) MessageBox.Show(Rhino.UI.RhinoEtoApp.MainWindow, message, "Record Movie");
            }
            finally
            {
                if (completed || count == 0) RemoveFrames(directoryToEncode);
                if (!disposed)
                {
                    Button.Text = "Record Movie...";
                    Button.Enabled = true;
                }
            }
        }

        public void Capture(RhinoView view)
        {
            try
            {
                var capture = new ViewCapture { Width = 1280, Height = 720, DrawGrid = false };
                using (var bitmap = capture.CaptureToBitmap(view))
                {
                    if (bitmap == null) throw new InvalidOperationException("Rhino did not return a viewport image.");
                    string name = frameCount.ToString("D6", CultureInfo.InvariantCulture) + ".png";
                    bitmap.Save(Path.Combine(frameDirectory, name), ImageFormat.Png);
                    frameCount++;
                    Button.Text = "Finish Movie (" + frameCount.ToString(CultureInfo.InvariantCulture) + " frames)";
                }
            }
            catch (Exception error)
            {
                RhinoApp.WriteLine("Could not capture a movie frame: " + error.Message);
            }
        }

        private static async Task EncodeAsync(string directory, string destination, double fps, int count, bool replaceExisting, string binaryFolder)
        {
            string extension = Path.GetExtension(destination);
            string temporaryMovie = Path.Combine(Path.GetDirectoryName(destination),
                ".pachyderm-" + Guid.NewGuid().ToString("N") + extension);
            string encoder = extension == ".webm"
                ? "-c:v libvpx-vp9 -crf 30 -b:v 0"
                : "-c:v libx264 -crf 18 -movflags +faststart";

            try
            {
                bool encoded = await FFMpegArguments
                    .FromFileInput(Path.Combine(directory, "%06d.png"), false, options => options
                        .WithCustomArgument("-framerate " + fps.ToString(CultureInfo.InvariantCulture)))
                    .OutputToFile(temporaryMovie, false, options => options
                        .WithCustomArgument(encoder + " -pix_fmt yuv420p -frames:v " + count.ToString(CultureInfo.InvariantCulture)))
                    .ProcessAsynchronously(true, new FFOptions
                    {
                        BinaryFolder = binaryFolder
                    });

                if (!encoded || !File.Exists(temporaryMovie))
                    throw new IOException("FFmpeg did not create a movie.");

                if (replaceExisting && File.Exists(destination)) File.Replace(temporaryMovie, destination, null);
                else File.Move(temporaryMovie, destination);
            }
            finally
            {
                if (File.Exists(temporaryMovie)) File.Delete(temporaryMovie);
            }
        }

        private static void RemoveFrames(string directory)
        {
            if (directory == null) return;
            try
            {
                foreach (string path in Directory.EnumerateFiles(directory, "*.png"))
                    File.Delete(path);
                Directory.Delete(directory);
            }
            catch (IOException error) { RhinoApp.WriteLine("Movie frame cleanup failed: " + error.Message); }
            catch (UnauthorizedAccessException error) { RhinoApp.WriteLine("Movie frame cleanup failed: " + error.Message); }
        }

        public void Dispose()
        {
            disposed = true;
            RemoveFrames(frameDirectory);
            frameDirectory = null;
        }
    }
}
