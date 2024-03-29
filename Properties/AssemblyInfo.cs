﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rhino.PlugIns;


// Plug-in Description Attributes - all of these are optional
// These will show in Rhino's option dialog, in the tab Plug-ins
[assembly: PlugInDescription(DescriptionType.Address, "-")]
[assembly: PlugInDescription(DescriptionType.Country, "Earth")]
[assembly: PlugInDescription(DescriptionType.Email, "info@orase.com")]
[assembly: PlugInDescription(DescriptionType.Phone, "-")]
[assembly: PlugInDescription(DescriptionType.Fax, "-")]
[assembly: PlugInDescription(DescriptionType.Organization, "Open Research in Acoustical Science and Education, Inc.")]
[assembly: PlugInDescription(DescriptionType.UpdateUrl, "http://www.orase.org/resources/")]
[assembly: PlugInDescription(DescriptionType.WebSite, "http://www.orase.org/")]


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Pachyderm_Acoustic")]
[assembly: AssemblyDescription("Pachyderm_Acoustic")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("PerspectiveSketch.com")]
[assembly: AssemblyProduct("Pachyderm_Acoustic")]
[assembly: AssemblyCopyright("Copyright © Arthur van der Harten 2008-2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("25895777-97d3-4058-8753-503183d4bc01")] // This will also be the Guid of the Rhino plug-in

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.5.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]