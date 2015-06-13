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


///<summary> 
/// The following class is required for all Rhino.NET plug-ins. 
/// These are used to display plug-in information in the plug-in manager. 
/// Any string will work for these attributes, so if you don't have a fax 
/// number it is OK to enter something like "none" 
///</summary> 
public class PachydermAcousticAttributes : RMA.Rhino.MRhinoPlugInAttributes
{
    //MyPlugIn1Attributes 

    public override string Address()
    {
        return "NA";
    }

    public override string Country()
    {
        return "USA";
    }

    public override string Email()
    {
        return "arthurv@perspectivesketch.com";
    }

    public override string Fax()
    {
        return "NA";
    }

    public override string Organization()
    {
        return "NA";
    }

    public override string Phone()
    {
        return "";
    }

    public override string UpdateURL()
    {
        return "NA";
    }

    public override string Website()
    {
        return "http://www.perspectivesketch.com/pachyderm";
    }
}