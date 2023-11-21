//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2019, Arthur van der Harten 
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

using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Windows.Threading;

namespace Pachyderm_Acoustic
{
    namespace UI
    {

        [System.Runtime.InteropServices.Guid("E022769F-AAF6-45BA-9EB6-CF0391E0B239")]
        public class Pach_Splash: Dialog
        {
            int dialogWidth = 800;
            int dialogHeight = 600;
            public int pmode = 0;

            Label Attribution = new Eto.Forms.Label();
            Label Copyright = new Eto.Forms.Label();
            Label Version_box = new Eto.Forms.Label();
            Label title_box = new Eto.Forms.Label();
            Drawable drawable = new Drawable();

            /// <summary>
            /// Autopopulates the splash screen based on parameters set in the plugin base class.
            /// </summary>
            public Pach_Splash()
                : base()
            {

                this.ID = "Pach_splash";
                PachydermAc_PlugIn p = PachydermAc_PlugIn.Instance;
                this.WindowStyle = WindowStyle.None;
                this.Enabled = true;

                //Application info 
                TableLayout MainLayoutPanel = new Eto.Forms.TableLayout();
                Attribution.Font = new Font("Microsoft Sans Serif", 8F);
                Attribution.Size = new Size(430, 150);
                Attribution.Text = "A community project serving foundational \r\nalgorithms for practical application in \r\nthe development of the built environment \r\nand the global soundscape.";
                Attribution.TextAlignment = TextAlignment.Center;
                Copyright.Font = new Font("Microsoft Sans Serif", 8F);
                Copyright.Text = "copyright:\r\nOpen Research in Acoustical Science and Education, Inc.\r\n(a 501(c)3 non-profit organization. \r\n Licensed under the GNU General Public License 3.0.)";
                Copyright.TextAlignment = TextAlignment.Center;
                Version_box.Font = new Font("Microsoft Sans Serif", 8F);
                Version_box.TextAlignment = TextAlignment.Center;
                title_box.Font = new Font("Microsoft Sans Serif", 12F);
                title_box.Text = "Pachyderm Acoustic Simulation:\r\nFoundational Simulation in Acoustics";
                title_box.TextAlignment = TextAlignment.Center;

                drawable.Paint += (sender, e) =>
                {
                    System.Drawing.Bitmap bitmap;

                    if (pmode == 0)
                    {
                        this.BackgroundColor = Color.FromArgb(0, 0, 0);
                        Version_box.TextColor = Color.FromArgb(255, 255, 255);
                        title_box.TextColor = Color.FromArgb(255, 255, 255);
                        Copyright.TextColor = Color.FromArgb(255, 255, 255);
                        Attribution.TextColor = Color.FromArgb(255, 255, 255);
                        bitmap = global::Pachyderm_Acoustic.Properties.Resources.Logo1Splash2;
                    }
                    else
                    {
                        this.BackgroundColor = Color.FromArgb(255, 255, 255);
                        Version_box.TextColor = Color.FromArgb(0, 0, 0);
                        title_box.TextColor = Color.FromArgb(0, 0, 0);
                        Copyright.TextColor = Color.FromArgb(0, 0, 0);
                        Attribution.TextColor = Color.FromArgb(0, 0, 0);
                        bitmap = global::Pachyderm_Acoustic.Properties.Resources.LogoOSplash;
                    }

                    System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                    byte[] bb = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
                    Image image = new Bitmap(bb);
                    e.Graphics.DrawImage(image, 0, 0);
                };

                MainLayoutPanel = new TableLayout();
                MainLayoutPanel.BackgroundColor = Colors.Transparent;
                MainLayoutPanel.Padding = new Padding(450, 50, 0, 200);
                MainLayoutPanel.Spacing = new Size(0, 10);
                MainLayoutPanel.Rows.Add(new TableRow(null, new TableCell(title_box)));
                MainLayoutPanel.Rows.Add(new TableRow(null, new TableCell(Version_box)));
                MainLayoutPanel.Rows.Add(new TableRow(new TableCell(), new TableCell(Copyright)));
                MainLayoutPanel.Rows.Add(new TableRow(new TableCell(), new TableCell(Attribution)));

                drawable.Content = MainLayoutPanel;
                this.Content = drawable;
            }

            System.Threading.Thread t;
            Dispatcher closer;
            protected override async void OnShown(EventArgs e)
            {
                base.OnShown(e);
                this.BringToFront();
                System.Threading.Thread.Sleep(2000);
                pmode++;
                this.Content.Invalidate(true);
                //(this.Content as Drawable).Update(Bounds);

                closer = Dispatcher.CurrentDispatcher;

                t = new System.Threading.Thread(new System.Threading.ThreadStart(() => { System.Threading.Thread.Sleep(2000); closer.Invoke(() => { this.Close(); }); }));

                t.Start();
            }



        }
    }
}