//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2012, Arthur van der Harten 
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

namespace Pachyderm_Acoustic
{
    namespace UI
    {
        public sealed partial class Pach_Splash
        {
            /// <summary>
            /// Autopopulates the splash screen based on parameters set in the plugin base class.
            /// </summary>
            public Pach_Splash()
            {
                InitializeComponent();
                PachydermAc_PlugIn p = PachydermAc_PlugIn.Instance;
                //Application info 
                Version_box.Text = string.Format("Version: {0}", p.Version);
                Version_box.ForeColor = System.Drawing.Color.White;
                title_box.ForeColor = System.Drawing.Color.White;
                Copyright.ForeColor = System.Drawing.Color.White;
                Attribution.ForeColor = System.Drawing.Color.White;
            }

            private void title_box_Click(object sender, System.EventArgs e)
            {

            }
        }
    }
}