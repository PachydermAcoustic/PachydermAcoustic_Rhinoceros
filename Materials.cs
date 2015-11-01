//'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL) by Arthur van der Harten 
//' 
//'This file is part of Pachyderm-Acoustic. 
//' 
//'Copyright (c) 2008-2015, Arthur van der Harten 
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

using System.Collections.Generic;

namespace Pachyderm_Acoustic
{
    /// <summary>
    /// A class to contain the acoustic materials library.
    /// </summary>
    public class Acoustics_Library : List<Material>
    {
        public Acoustics_Library()
            : base()
        {
            //Read Absorption values in from library file designated on the Options Page...
            Load_Library();
        }

        /// <summary>
        /// Loads the user defined materials library.
        /// </summary>
        public void Load_Library()
        {
            this.Clear();
            System.IO.StreamReader Reader;
            try
            {
                string MLPath = UI.PachydermAc_PlugIn.Instance.ML_Path();
                MLPath += "\\Pach_Materials_Library.txt";
                Reader = new System.IO.StreamReader(MLPath);
            }
            catch (System.Exception)
            {
                return;
            }
            do
            {
                try
                {
                    string Material = Reader.ReadLine();
                    string[] D_Mat = Material.Split(new char[] { ':' });
                    string Name = D_Mat[0].Trim();
                    string Abs_Code = D_Mat[1].Trim();
                    Abs_Code += "0000000000000000";
                    double[] Abs = new double[8];
                    double[] Sct = new double[8];
                    double[] Trns = new double[8];
                    UI.PachydermAc_PlugIn.DecodeAcoustics(Abs_Code, ref Abs, ref Sct, ref Trns);
                    this.Add_Unique(Name, Abs);
                }
                catch (System.Exception)
                { continue; }
            } while (!Reader.EndOfStream);
            Reader.Close();
        }

        public void Add_Unique(string Name, double[] Abs)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!string.Equals(this[i].Name, Name, System.StringComparison.OrdinalIgnoreCase)) continue;
                Rhino.RhinoApp.WriteLine(string.Format("Replacing material '{0}'", this[i].Name));
                this.RemoveAt(i);
                break;
            }

            this.Add(new Material(Name, Abs));
        }

        public void Delete (string Name)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!string.Equals(this[i].Name, Name, System.StringComparison.OrdinalIgnoreCase)) continue;
                Rhino.RhinoApp.WriteLine(string.Format("Deleting material '{0}'", this[i].Name));
                this.RemoveAt(i);
                break;
            }
        }

        public Material byKey(string Selection)
        {
            foreach (Material Mat in this) if (Mat.Name == Selection) return Mat;
            throw new System.Exception();
        }

        /// <summary>
        /// Saves the user defined materials library.
        /// </summary>
        public void Save_Library()
        {
            //Enter an external file saver here... 
            string MLPath = UI.PachydermAc_PlugIn.Instance.ML_Path();
            MLPath += "\\Pach_Materials_Library.txt";

            System.IO.StreamWriter Writer;
            Writer = new System.IO.StreamWriter(MLPath);

            for (int i = 0; i < this.Count; i++)
            {
                string Entry = this[i].Name + ':';
                int[] sct = new int[8];
                int[] abs = new int[8];
                int[] trns = new int[1];
                for (int oct = 0; oct < 8; oct++)
                {
                    abs[oct] = (int)(this[i].Absorption[oct] * 100.0);
                }
                string Abs_Code = UI.PachydermAc_PlugIn.EncodeAcoustics(abs, sct, trns);
                Entry += Abs_Code.Substring(0, 16);
                Writer.WriteLine(Entry);
            }
            Writer.Close();
        }

        /// <summary>
        /// Returns the names of all user defined materials for display in an interface component.
        /// </summary>
        /// <returns></returns>
        public List<string> Names()
        {
            List<string> Catalog = new List<string>();
            foreach (Material obj in this)
            {
                Catalog.Add(obj.Name);
            }
            return Catalog;
        }
    }

    /// <summary>
    /// a structure defining a material.
    /// </summary>
    public struct Material
    {
        public double[] Absorption;
        public string Name;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="nym">the name of the material</param>
        /// <param name="Abs">the absorption coefficients, from 62.5 Hz.[0] to 8 khz.[7]</param>
        public Material(string nym, double[] Abs)
        {
            Name = nym;
            Absorption = Abs;
        }
    }
}