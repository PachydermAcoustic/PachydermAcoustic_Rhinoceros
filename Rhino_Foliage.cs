// 'Pachyderm-Acoustic: Geometrical Acoustics for Rhinoceros (GPL)
// Copyright (c) 2008-2026, Open Research in Acoustical Science and Education, Inc.
// This file is part of Pachyderm-Acoustic, distributed under the GNU General
// Public License, version 3 or later, without any warranty. See General_Public_License.txt.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Eto.Forms;
using Rhino;
using Rhino.DocObjects;
using Rhino.Display;
using Rhino.Geometry;

namespace Pachyderm_Acoustic.UI
{
    // Botanical dimensions describe the stand; they are not attenuation coefficients.
    internal sealed class FoliageProperties : GroupBox
    {
        internal const string Key = "Pachyderm_Foliage";
        internal sealed class Tree
        {
            internal readonly string Name, Species, Source, WidthSource;
            internal readonly double HeightMin, HeightMax, LeafMin, LeafMax, WidthMin, WidthMax;
            internal readonly bool Evergreen, Leaflet;
            internal Tree(string name, string species, double minFeet, double maxFeet, double minInches, double maxInches, bool evergreen, string source, double widthMin, double widthMax, string widthSource, bool leaflet = false)
            {
                Name = name; Species = species; HeightMin = minFeet * .3048; HeightMax = maxFeet * .3048;
                LeafMin = minInches * 25.4; LeafMax = maxInches * 25.4; Evergreen = evergreen; Source = source;
                WidthMin = widthMin; WidthMax = widthMax; WidthSource = widthSource; Leaflet = leaflet;
            }
        }

        // Published landscape ranges, not maximum forest heights or measured site averages.
        // UF/IFAS Extension: Gilman, Watson et al., species fact sheets (accessed 2026-10-25).
        // Blade/needle widths (mm): Native Plant Trust Go Botany; NC State Extension; Scion.
        // Botanical range midpoints are editable defaults, not measured canopy means (accessed 2026-10-26).
        // Additional species: NC State Extension heights; Go Botany blade dimensions (accessed 2026-10-27).
        // Scots pine needles: Flora of Oregon; white fir needles: Oregon State University.
        // Equal width limits represent a single published dimension; missing lengths stay unknown.
        // Connecticut additions (2026-10-27): compound species use individual leaflet dimensions.
        // Boxelder includes larger terminal leaflets; widths remain editable range midpoints.
        internal static readonly Tree[] Trees = {
            new Tree("Red maple", "Acer rubrum", 60, 75, 2, 5, false, "https://ask.ifas.ufl.edu/publication/ST041", 60, 150, "https://gobotany.nativeplanttrust.org/species/acer/rubrum/"),
            new Tree("River birch", "Betula nigra", 40, 50, 1, 4, false, "https://ask.ifas.ufl.edu/publication/ST094", 30, 60, "https://gobotany.nativeplanttrust.org/species/betula/nigra/"),
            new Tree("White oak", "Quercus alba", 60, 100, 2, 8, false, "https://ask.ifas.ufl.edu/publication/ST541", 40, 165, "https://gobotany.nativeplanttrust.org/species/quercus/alba/"),
            new Tree("Southern live oak", "Quercus virginiana", 60, 80, 2, 5, true, "https://ask.ifas.ufl.edu/publication/ST564", 20.32, 38.1, "https://plants.ces.ncsu.edu/plants/quercus-virginiana/"),
            new Tree("Eastern white pine", "Pinus strobus", 50, 80, 2, 4, true, "https://hort.ifas.ufl.edu/database/documents/pdf/tree_fact_sheets/pinstra.pdf", 0.7, 1, "https://nzfri.scionresearch.com/Content/Projects/nzfri/keys/cultivated-pines/key/cultivated_pines/Media/Html/Pinus_strobus.htm"),
            new Tree("Sugar maple", "Acer saccharum", 40, 120, double.NaN, 5.90551181102362, false, "https://plants.ces.ncsu.edu/plants/acer-saccharum/", 80, 200, "https://gobotany.nativeplanttrust.org/species/acer/saccharum/"),
            new Tree("Silver maple", "Acer saccharinum", 50, 80, double.NaN, 5.90551181102362, false, "https://plants.ces.ncsu.edu/plants/acer-saccharinum/", 80, 140, "https://gobotany.nativeplanttrust.org/species/acer/saccharinum/"),
            new Tree("Norway maple", "Acer platanoides", 40, 50, 3.93700787401575, 5.90551181102362, false, "https://plants.ces.ncsu.edu/plants/acer-platanoides/", 100, 180, "https://gobotany.nativeplanttrust.org/species/acer/platanoides/"),
            new Tree("Northern red oak", "Quercus rubra", 50, 70, 4.7244094488189, 7.8740157480315, false, "https://plants.ces.ncsu.edu/plants/quercus-rubra/", 60, 120, "https://gobotany.nativeplanttrust.org/species/quercus/rubra/"),
            new Tree("Pin oak", "Quercus palustris", 50, 70, 1.96850393700787, 6.2992125984252, false, "https://plants.ces.ncsu.edu/plants/quercus-palustris/", 50, 120, "https://gobotany.nativeplanttrust.org/species/quercus/palustris/"),
            new Tree("Swamp white oak", "Quercus bicolor", 50, 60, 3.11023622047244, 8.46456692913386, false, "https://plants.ces.ncsu.edu/plants/quercus-bicolor/", 40, 160, "https://gobotany.nativeplanttrust.org/species/quercus/bicolor/"),
            new Tree("American beech", "Fagus grandifolia", 60, 80, 2.36220472440945, 4.7244094488189, false, "https://plants.ces.ncsu.edu/plants/fagus-grandifolia/", 30, 80, "https://gobotany.nativeplanttrust.org/species/fagus/grandifolia/"),
            new Tree("European beech", "Fagus sylvatica", 50, 60, 1.96850393700787, 3.93700787401575, false, "https://plants.ces.ncsu.edu/plants/fagus-sylvatica/", 38.1, 63.5, "https://gobotany.nativeplanttrust.org/species/fagus/sylvatica/"),
            new Tree("Paper birch", "Betula papyrifera", 50, 70, 0.984251968503937, 4.13385826771654, false, "https://plants.ces.ncsu.edu/plants/betula-papyrifera/", 40, 65, "https://gobotany.nativeplanttrust.org/species/betula/papyrifera/"),
            new Tree("Yellow birch", "Betula alleghaniensis", 70, 80, 2.36220472440945, 3.93700787401575, false, "https://plants.ces.ncsu.edu/plants/betula-alleghaniensis/", 30, 55, "https://gobotany.nativeplanttrust.org/species/betula/alleghaniensis/"),
            new Tree("Black / sweet birch", "Betula lenta", 60, 70, 1.96850393700787, 3.93700787401575, false, "https://plants.ces.ncsu.edu/plants/betula-lenta/", 30, 60, "https://gobotany.nativeplanttrust.org/species/betula/lenta/"),
            new Tree("American sycamore", "Platanus occidentalis", 75, 100, 1.96850393700787, 7.8740157480315, false, "https://plants.ces.ncsu.edu/plants/platanus-occidentalis/", 50, 250, "https://gobotany.nativeplanttrust.org/species/platanus/occidentalis/"),
            new Tree("Tulip tree", "Liriodendron tulipifera", 80, 120, 2.95275590551181, 5.90551181102362, false, "https://plants.ces.ncsu.edu/plants/liriodendron-tulipifera/", 80, 185, "https://gobotany.nativeplanttrust.org/species/liriodendron/tulipifera/"),
            new Tree("Sweetgum", "Liquidambar styraciflua", 60, 100, 2.75590551181102, 9.84251968503937, false, "https://plants.ces.ncsu.edu/plants/liquidambar-styraciflua/", 44, 160, "https://gobotany.nativeplanttrust.org/species/liquidambar/styraciflua/"),
            new Tree("Blackgum", "Nyssa sylvatica", 40, 70, 1.18110236220472, 5.90551181102362, false, "https://plants.ces.ncsu.edu/plants/nyssa-sylvatica/", 20, 60, "https://gobotany.nativeplanttrust.org/species/nyssa/sylvatica/"),
            new Tree("Eastern cottonwood", "Populus deltoides", 75, 100, 3.93700787401575, 7.08661417322835, false, "https://plants.ces.ncsu.edu/plants/populus-deltoides/", 60, 120, "https://gobotany.nativeplanttrust.org/species/populus/deltoides/"),
            new Tree("Quaking aspen", "Populus tremuloides", 20, 50, 0.78740157480315, 3.93700787401575, false, "https://plants.ces.ncsu.edu/plants/populus-tremuloides/", 18, 70, "https://gobotany.nativeplanttrust.org/species/populus/tremuloides/"),
            new Tree("American elm", "Ulmus americana", 60, 80, 2.75590551181102, 5.51181102362205, false, "https://plants.ces.ncsu.edu/plants/ulmus-americana/", 30, 70, "https://gobotany.nativeplanttrust.org/species/ulmus/americana/"),
            new Tree("Black cherry", "Prunus serotina", 60, 80, 1.37795275590551, 5.90551181102362, false, "https://plants.ces.ncsu.edu/plants/prunus-serotina/", 20, 55, "https://gobotany.nativeplanttrust.org/species/prunus/serotina/"),
            new Tree("White willow", "Salix alba", 50, 70, 1.5748031496063, 4.7244094488189, false, "https://plants.ces.ncsu.edu/plants/salix-alba/", 10, 30, "https://gobotany.nativeplanttrust.org/species/salix/alba/"),
            new Tree("Scarlet oak", "Quercus coccinea", 50, 80, 70 / 25.4, 160 / 25.4, false, "https://plants.ces.ncsu.edu/plants/quercus-coccinea/", 80, 130, "https://gobotany.nativeplanttrust.org/species/quercus/coccinea/"),
            new Tree("Black oak", "Quercus velutina", 50, 100, 80 / 25.4, 300 / 25.4, false, "https://plants.ces.ncsu.edu/plants/quercus-velutina/", 80, 150, "https://gobotany.nativeplanttrust.org/species/quercus/velutina/"),
            new Tree("Bur oak", "Quercus macrocarpa", 70, 80, 50 / 25.4, 310 / 25.4, false, "https://plants.ces.ncsu.edu/plants/quercus-macrocarpa/", 40, 160, "https://gobotany.nativeplanttrust.org/species/quercus/macrocarpa/"),
            new Tree("English oak", "Quercus robur", 40, 70, 50 / 25.4, 200 / 25.4, false, "https://plants.ces.ncsu.edu/plants/quercus-robur/", 20, 100, "https://gobotany.nativeplanttrust.org/species/quercus/robur/"),
            new Tree("Willow oak", "Quercus phellos", 40, 75, 50 / 25.4, 120 / 25.4, false, "https://plants.ces.ncsu.edu/plants/quercus-phellos/", 10, 25, "https://gobotany.nativeplanttrust.org/species/quercus/phellos/"),
            new Tree("Shingle oak", "Quercus imbricaria", 50, 70, 80 / 25.4, 200 / 25.4, false, "https://plants.ces.ncsu.edu/plants/quercus-imbricaria/", 15, 75, "https://gobotany.nativeplanttrust.org/species/quercus/imbricaria/"),
            new Tree("Chestnut oak", "Quercus montana", 50, 75, 100 / 25.4, 220 / 25.4, false, "https://plants.ces.ncsu.edu/plants/quercus-montana/", 60, 120, "https://gobotany.nativeplanttrust.org/species/quercus/montana/"),
            new Tree("Hedge maple", "Acer campestre", 25, 35, 40 / 25.4, 70 / 25.4, false, "https://plants.ces.ncsu.edu/plants/acer-campestre/", 50, 100, "https://gobotany.nativeplanttrust.org/species/acer/campestre/"),
            new Tree("Sycamore maple", "Acer pseudoplatanus", 40, 60, double.NaN, double.NaN, false, "https://plants.ces.ncsu.edu/plants/acer-pseudoplatanus/", 80, 160, "https://gobotany.nativeplanttrust.org/species/acer/pseudoplatanus/"),
            new Tree("American hornbeam", "Carpinus caroliniana", 20, 30, 25 / 25.4, 120 / 25.4, false, "https://plants.ces.ncsu.edu/plants/carpinus-caroliniana/", 10, 50, "https://gobotany.nativeplanttrust.org/species/carpinus/caroliniana/"),
            new Tree("Eastern hophornbeam", "Ostrya virginiana", 20, 40, 80 / 25.4, 100 / 25.4, false, "https://plants.ces.ncsu.edu/plants/ostrya-virginiana/", 40, 50, "https://gobotany.nativeplanttrust.org/species/ostrya/virginiana/"),
            new Tree("Cucumber magnolia", "Magnolia acuminata", 50, 80, 80 / 25.4, 250 / 25.4, false, "https://plants.ces.ncsu.edu/plants/magnolia-acuminata/", 40, 100, "https://gobotany.nativeplanttrust.org/species/magnolia/acuminata/"),
            new Tree("Sweetbay magnolia", "Magnolia virginiana", 10, 35, 60 / 25.4, 220 / 25.4, false, "https://plants.ces.ncsu.edu/plants/magnolia-virginiana/", 26, 70, "https://gobotany.nativeplanttrust.org/species/magnolia/virginiana/"),
            new Tree("Common hackberry", "Celtis occidentalis", 40, 100, 40 / 25.4, 120 / 25.4, false, "https://plants.ces.ncsu.edu/plants/celtis-occidentalis/", 12, 90, "https://gobotany.nativeplanttrust.org/species/celtis/occidentalis/"),
            new Tree("Sassafras", "Sassafras albidum", 30, 60, 70 / 25.4, 150 / 25.4, false, "https://plants.ces.ncsu.edu/plants/sassafras-albidum/", 50, 150, "https://gobotany.nativeplanttrust.org/species/sassafras/albidum/"),
            new Tree("Eastern redbud", "Cercis canadensis", 20, 30, 70 / 25.4, 120 / 25.4, false, "https://plants.ces.ncsu.edu/plants/cercis-canadensis/", 60, 150, "https://gobotany.nativeplanttrust.org/species/cercis/canadensis/"),
            new Tree("White mulberry", "Morus alba", 30, 60, 40 / 25.4, 120 / 25.4, false, "https://plants.ces.ncsu.edu/plants/morus-alba/", 30, 90, "https://gobotany.nativeplanttrust.org/species/morus/alba/"),
            new Tree("Red mulberry", "Morus rubra", 25, 60, 70 / 25.4, 360 / 25.4, false, "https://plants.ces.ncsu.edu/plants/morus-rubra/", 30, 155, "https://gobotany.nativeplanttrust.org/species/morus/rubra/"),
            new Tree("Japanese flowering cherry", "Prunus serrulata", 15, 25, 60 / 25.4, 120 / 25.4, false, "https://plants.ces.ncsu.edu/plants/prunus-serrulata/", 25, 50, "https://gobotany.nativeplanttrust.org/species/prunus/serrulata/"),
            new Tree("Downy serviceberry", "Amelanchier arborea", 15, 25, 40 / 25.4, 100 / 25.4, false, "https://plants.ces.ncsu.edu/plants/amelanchier-arborea/", 22, 50, "https://gobotany.nativeplanttrust.org/species/amelanchier/arborea/"),
            new Tree("White poplar", "Populus alba", 50, 75, 50 / 25.4, 85 / 25.4, false, "https://plants.ces.ncsu.edu/plants/populus-alba/", 40, 80, "https://gobotany.nativeplanttrust.org/species/populus/alba/"),
            new Tree("Black poplar", "Populus nigra", 40, 70, 50 / 25.4, 100 / 25.4, false, "https://plants.ces.ncsu.edu/plants/populus-nigra/", 40, 80, "https://gobotany.nativeplanttrust.org/species/populus/nigra/"),
            new Tree("Silver birch", "Betula pendula", 30, 50, 30 / 25.4, 70 / 25.4, false, "https://plants.ces.ncsu.edu/plants/betula-pendula/", 25, 50, "https://gobotany.nativeplanttrust.org/species/betula/pendula/"),
            new Tree("Scots pine", "Pinus sylvestris", 30, 60, 30 / 25.4, 60 / 25.4, true, "https://plants.ces.ncsu.edu/plants/pinus-sylvestris/", 1.5, 2, "https://oregonflora.org/taxa/index.php?tid=7313"),
            new Tree("White fir", "Abies concolor", 30, 70, 40 / 25.4, 60 / 25.4, true, "https://plants.ces.ncsu.edu/plants/abies-concolor/", 2, 2, "https://landscapeplants.oregonstate.edu/plants/abies-concolor"),
            new Tree("Amur maple", "Acer ginnala", 15, 20, 40 / 25.4, 80 / 25.4, false, "https://plants.ces.ncsu.edu/plants/acer-ginnala/", 30, 60, "https://gobotany.nativeplanttrust.org/species/acer/ginnala/"),
            new Tree("Princess tree", "Paulownia tomentosa", 30, 50, 60 / 25.4, 500 / 25.4, false, "https://plants.ces.ncsu.edu/plants/paulownia-tomentosa/", 40, 560, "https://gobotany.nativeplanttrust.org/species/paulownia/tomentosa/"),
            new Tree("Slippery elm", "Ulmus rubra", 40, 70, 80 / 25.4, 160 / 25.4, false, "https://plants.ces.ncsu.edu/plants/ulmus-rubra/", 50, 75, "https://gobotany.nativeplanttrust.org/species/ulmus/rubra/"),
            new Tree("Chokecherry", "Prunus virginiana", 20, 30, 50 / 25.4, 120 / 25.4, false, "https://plants.ces.ncsu.edu/plants/prunus-virginiana/", 25, 55, "https://gobotany.nativeplanttrust.org/species/prunus/virginiana/"),
            new Tree("Gray birch", "Betula populifolia", 20, 30, 30 / 25.4, 100 / 25.4, false, "https://hort.ifas.ufl.edu/database/documents/pdf/tree_fact_sheets/betpopa.pdf", 30, 80, "https://gobotany.nativeplanttrust.org/species/betula/populifolia/"),
            new Tree("Black locust", "Robinia pseudoacacia", 30, 50, 20 / 25.4, 50 / 25.4, false, "https://plants.ces.ncsu.edu/plants/robinia-pseudoacacia/", 10, 25, "https://plantnet.rbgsyd.nsw.gov.au/cgi-bin/NSWfl.pl?lvl=sp&name=Robinia~pseudoacacia&page=nswfl", true),
            new Tree("Tree of heaven", "Ailanthus altissima", 40, 60, 40 / 25.4, 130 / 25.4, false, "https://plants.ces.ncsu.edu/plants/ailanthus-altissima/", 25, 50, "https://plantnet.rbgsyd.nsw.gov.au/cgi-bin/NSWfl.pl?lvl=sp&name=Ailanthus~altissima&page=nswfl", true),
            new Tree("White ash", "Fraxinus americana", 60, 120, 76.2 / 25.4, 127 / 25.4, false, "https://plants.ces.ncsu.edu/plants/fraxinus-americana/", 38.1, 76.2, "https://mdc.mo.gov/discover-nature/field-guide/white-ash", true),
            new Tree("Green ash", "Fraxinus pennsylvanica", 50, 70, 63.5 / 25.4, 127 / 25.4, false, "https://plants.ces.ncsu.edu/plants/fraxinus-pennsylvanica/", 25.4, 44.45, "https://plants.ces.ncsu.edu/plants/fraxinus-pennsylvanica/", true),
            new Tree("Shagbark hickory", "Carya ovata", 70, 90, 76.2 / 25.4, 177.8 / 25.4, false, "https://plants.ces.ncsu.edu/plants/carya-ovata/", 25.4, 76.2, "https://plants.ces.ncsu.edu/plants/carya-ovata/", true),
            new Tree("Pignut hickory", "Carya glabra", 60, 80, 76.2 / 25.4, 152.4 / 25.4, false, "https://plants.ces.ncsu.edu/plants/carya-glabra/", 25.4, 50.8, "https://plants.ces.ncsu.edu/plants/carya-glabra/", true),
            new Tree("Mockernut hickory", "Carya tomentosa", 60, 80, 76.2 / 25.4, 152.4 / 25.4, false, "https://plants.ces.ncsu.edu/plants/carya-tomentosa/", 25.4, 50.8, "https://plants.ces.ncsu.edu/plants/carya-tomentosa/", true),
            new Tree("Bitternut hickory", "Carya cordiformis", 50, 100, 30 / 25.4, 190 / 25.4, false, "https://plants.ces.ncsu.edu/plants/carya-cordiformis/", 10, 70, "https://vplants.org/portal/taxa/index.php?clid=3503&taxauthid=1&taxon=76890", true),
            new Tree("Black walnut", "Juglans nigra", 50, 75, 50 / 25.4, 130 / 25.4, false, "https://plants.ces.ncsu.edu/plants/juglans-nigra/", 20, 50, "https://landscapeplants.oregonstate.edu/plants/juglans-nigra", true),
            new Tree("Butternut", "Juglans cinerea", 40, 60, 50.8 / 25.4, 127 / 25.4, false, "https://plants.ces.ncsu.edu/plants/juglans-cinerea/", 19.05, 57.15, "https://plants.ces.ncsu.edu/plants/juglans-cinerea/", true),
            new Tree("Honey locust", "Gleditsia triacanthos", 60, 80, 10 / 25.4, 35 / 25.4, false, "https://plants.ces.ncsu.edu/plants/gleditsia-triacanthos/", 5, 12, "https://plantnet.rbgsyd.nsw.gov.au/cgi-bin/NSWfl.pl?lvl=sp&name=Gleditsia~triacanthos&page=nswfl", true),
            new Tree("Boxelder", "Acer negundo", 30, 50, 30 / 25.4, 150 / 25.4, false, "https://plants.ces.ncsu.edu/plants/acer-negundo/", 20, 80, "https://plantnet.rbgsyd.nsw.gov.au/cgi-bin/NSWfl.pl?lvl=sp&name=Acer~negundo&page=nswfl", true),
            new Tree("American basswood", "Tilia americana", 60, 80, 127 / 25.4, 152.4 / 25.4, false, "https://plants.ces.ncsu.edu/plants/tilia-americana/", 76.2, 127, "https://mdc.mo.gov/discover-nature/field-guide/american-basswood-american-linden"),
            new Tree("Flowering dogwood", "Cornus florida", 15, 25, 60 / 25.4, 100 / 25.4, false, "https://plants.ces.ncsu.edu/plants/cornus-florida/", 30, 50, "https://gobotany.nativeplanttrust.org/species/benthamidia/florida/"),
            new Tree("Eastern hemlock", "Tsuga canadensis", 40, 70, 7 / 25.4, 13 / 25.4, true, "https://plants.ces.ncsu.edu/plants/tsuga-canadensis/", 1.5, 2, "https://www.dnr.state.mn.us/rsg/species-detail.html?id=10686")
        };

        internal sealed class Settings
        {
            internal string Species;
            internal double Height, LeafLength, CanopyBase;
            internal double LeafWidth = 50, LeafAreaDensity;
            internal bool FromTreeCount;
            internal double TreesPerHectare, CrownDiameter, CrownLeafAreaIndex;
            internal bool LeafOn, Reverse, Preview, Estimated, LegacyEstimated;
            internal double[] Attenuation;
            internal string[] MixtureSpecies;
            internal double[] MixtureWidths, MixtureShares;
            // v6 retains tree-count assumptions; v1-v5 remain readable without reinterpretation.
            internal string Code
            {
                get
                {
                    bool extended = MixtureSpecies != null || Estimated || CanopyBase > 0;
                    string code = string.Join("|", FromTreeCount ? "6" : MixtureSpecies != null ? "5" : extended ? "4" : Attenuation == null ? "1" : "2", Species, Height.ToString("R", CultureInfo.InvariantCulture), LeafLength.ToString("R", CultureInfo.InvariantCulture), LeafOn ? "1" : "0", Reverse ? "1" : "0", Preview ? "1" : "0");
                    if (extended || Attenuation != null) code += "|" + (Attenuation == null ? "" : string.Join(";", Attenuation.Select(v => v.ToString("R", CultureInfo.InvariantCulture))));
                    if (extended) code += "|" + string.Join("|", CanopyBase.ToString("R", CultureInfo.InvariantCulture), LeafWidth.ToString("R", CultureInfo.InvariantCulture), LeafAreaDensity.ToString("R", CultureInfo.InvariantCulture), Estimated ? "1" : "0");
                    if (MixtureSpecies != null) code += "|" + string.Join(";", MixtureSpecies.Select((name, i) => name + "^" + MixtureShares[i].ToString("R", CultureInfo.InvariantCulture) + "^" + MixtureWidths[i].ToString("R", CultureInfo.InvariantCulture)));
                    if (FromTreeCount) code += "|" + string.Join("|", TreesPerHectare.ToString("R", CultureInfo.InvariantCulture), CrownDiameter.ToString("R", CultureInfo.InvariantCulture), CrownLeafAreaIndex.ToString("R", CultureInfo.InvariantCulture));
                    return code;
                }
            }
            internal static bool TryRead(string code, out Settings value)
            {
                value = null;
                if (string.IsNullOrEmpty(code)) return false;
                string[] fields = code.Split('|');
                if (!((fields.Length == 7 && fields[0] == "1") || (fields.Length == 8 && fields[0] == "2") || (fields.Length == 14 && fields[0] == "3") || (fields.Length == 12 && fields[0] == "4") || (fields.Length == 13 && fields[0] == "5") || (fields.Length == 16 && fields[0] == "6")) || string.IsNullOrWhiteSpace(fields[1])) return false;
                if (!double.TryParse(fields[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double height) || !(height > 0) || double.IsInfinity(height)) return false;
                if (!double.TryParse(fields[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double leaf) || !(leaf > 0) || double.IsInfinity(leaf)) return false;
                for (int i = 4; i < 7; i++) if (fields[i] != "0" && fields[i] != "1") return false;
                var settings = new Settings { Species = fields[1], Height = height, LeafLength = leaf, LeafOn = fields[4] == "1", Reverse = fields[5] == "1", Preview = fields[6] == "1" };
                if (fields[0] != "1" && !(fields[0] != "2" && fields[7] == ""))
                {
                    string[] bands = fields[7].Split(';');
                    if (bands.Length != 8) return false;
                    settings.Attenuation = new double[8];
                    for (int band = 0; band < 8; band++)
                        if (!double.TryParse(bands[band], NumberStyles.Float, CultureInfo.InvariantCulture, out settings.Attenuation[band]) || settings.Attenuation[band] < 0 || double.IsNaN(settings.Attenuation[band]) || double.IsInfinity(settings.Attenuation[band])) return false;
                }
                if (fields[0] == "3" || fields[0] == "4" || fields[0] == "5" || fields[0] == "6")
                {
                    bool legacy = fields[0] == "3";
                    double[] inputs = new double[legacy ? 5 : 3];
                    for (int i = 0; i < inputs.Length; i++)
                        if (!double.TryParse(fields[8 + i], NumberStyles.Float, CultureInfo.InvariantCulture, out inputs[i]) || inputs[i] < 0 || double.IsNaN(inputs[i]) || double.IsInfinity(inputs[i])) return false;
                    int mode = fields[0] == "5" || fields[0] == "6" ? 11 : fields.Length - 1;
                    if (inputs[0] >= height || inputs[1] == 0 || (legacy && (inputs[3] == 0 || inputs[4] == 0)) || (fields[mode] != "0" && fields[mode] != "1")) return false;
                    settings.CanopyBase = inputs[0]; settings.LeafWidth = inputs[1]; settings.LeafAreaDensity = inputs[2]; settings.Estimated = fields[mode] == "1";
                    if (settings.Estimated)
                    {
                        if (legacy)
                        {
                            if (settings.Attenuation == null) return false;
                            double[] estimate;
                            try { estimate = Environment.Foliage_Volume.Estimate_Aylor(settings.LeafWidth * .001, settings.LeafAreaDensity, inputs[3], inputs[4]); }
                            catch (ArgumentOutOfRangeException) { return false; }
                            for (int band = 0; band < 8; band++) if (Math.Abs(estimate[band] - settings.Attenuation[band]) > 1e-12 * Math.Max(1, estimate[band])) return false;
                            settings.LegacyEstimated = true;
                            settings.Attenuation = null; // Replace the legacy linearization with accumulated-distance attenuation.
                        }
                        else if (settings.Attenuation != null) return false;
                    }
                }
                if (fields[0] == "5" || fields[0] == "6")
                {
                    string[] rows = fields[12].Split(';');
                    settings.MixtureSpecies = new string[rows.Length]; settings.MixtureShares = new double[rows.Length]; settings.MixtureWidths = new double[rows.Length];
                    double total = 0;
                    var names = new HashSet<string>();
                    for (int i = 0; i < rows.Length; i++)
                    {
                        string[] row = rows[i].Split('^');
                        if (row.Length != 3 || (row[0] != "Custom" && !Trees.Any(tree => tree.Species == row[0])) || !names.Add(row[0])) return false;
                        if (!double.TryParse(row[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double share) || !(share > 0) || double.IsInfinity(share)) return false;
                        if (!double.TryParse(row[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double width) || !(width > 0) || double.IsInfinity(width)) return false;
                        settings.MixtureSpecies[i] = row[0]; settings.MixtureShares[i] = share; settings.MixtureWidths[i] = width; total += share;
                    }
                    if (Math.Abs(total - 1) > 1e-9) return false;
                }
                if (fields[0] == "6")
                {
                    if (!settings.Estimated) return false;
                    double[] inputs = new double[3];
                    for (int i = 0; i < inputs.Length; i++)
                        if (!double.TryParse(fields[13 + i], NumberStyles.Float, CultureInfo.InvariantCulture, out inputs[i])) return false;
                    double density;
                    try { density = Environment.Foliage_Volume.EstimateLeafAreaDensity(inputs[0], inputs[1], inputs[2], settings.Height - settings.CanopyBase); }
                    catch (ArgumentOutOfRangeException) { return false; }
                    if (Math.Abs(density - settings.LeafAreaDensity) > 1e-12 * Math.Max(density, settings.LeafAreaDensity)) return false;
                    settings.FromTreeCount = true; settings.TreesPerHectare = inputs[0]; settings.CrownDiameter = inputs[1]; settings.CrownLeafAreaIndex = inputs[2];
                }
                value = settings;
                return true;
            }
        }

        sealed class MixtureRow
        {
            internal string Species;
            public string Name { get; set; }
            public string Share { get; set; }
            public string Width { get; set; }
        }
        readonly List<MixtureRow> rows = new List<MixtureRow>();
        readonly GridView mixture = new GridView { AllowMultipleSelection = false, Height = 150 };
        readonly DropDown species = new DropDown { Width = 220 };
        readonly Label dimensions = new Label { Wrap = WrapMode.Word };
        readonly NumericStepper height = new NumericStepper { MinValue = .01, MaxValue = 1000, Value = 20.574, DecimalPlaces = 2, Increment = .5 };
        readonly NumericStepper canopyBase = new NumericStepper { MinValue = 0, MaxValue = 1000, DecimalPlaces = 2, Increment = .5 };
        readonly NumericStepper density = new NumericStepper { MinValue = 0, MaxValue = 1000, DecimalPlaces = 3, Increment = .1 };
        readonly DropDown densityFrom = new DropDown();
        readonly NumericStepper treeCount = new NumericStepper { MinValue = 0, MaxValue = 1000000, DecimalPlaces = 1, Increment = 10 };
        readonly NumericStepper crownDiameter = new NumericStepper { MinValue = 0, MaxValue = 1000, DecimalPlaces = 2, Increment = .5 };
        readonly NumericStepper crownLAI = new NumericStepper { MinValue = 0, MaxValue = 1000, Value = 5, DecimalPlaces = 2, Increment = .5 };
        readonly Label densitySummary = new Label { Wrap = WrapMode.Word };
        readonly NumericStepper distance = new NumericStepper { MinValue = 0, MaxValue = 1000000, Value = 10, DecimalPlaces = 1, Increment = 1 };
        readonly CheckBox leafOn = new CheckBox { Text = "Leaf-on", Checked = true };
        readonly CheckBox preview = new CheckBox { Text = "Preview canopy", Checked = true };
        readonly CheckBox remove = new CheckBox { Text = "Remove foliage on close" };
        readonly Label total = new Label(), status = new Label { Wrap = WrapMode.Word }, mediumInfo = new Label { Wrap = WrapMode.Word };
        readonly Label[] results = new Label[8];
        readonly DropDown medium = new DropDown();
        List<RhinoObject> objects = new List<RhinoObject>();
        RhinoDoc document;
        bool loading, saving, dirty;
        double leafLength = 88.9;

        internal FoliageProperties()
        {
            Text = "Foliage";
            ToolTip = "Aylor (1972), Fig. 3; doi:10.1121/1.1912852. Tree mixtures are a canopy-area weighted approximation of corn/reed data. Ground, trunks and branches are excluded.";
            var layout = new StackLayout { Spacing = 5 };
            mixture.Columns.Add(new GridColumn { HeaderText = "Tree", Width = 165, DataCell = new TextBoxCell { Binding = Binding.Property<MixtureRow, string>(row => row.Name) } });
            mixture.Columns.Add(new GridColumn { HeaderText = "Canopy %", Width = 85, Editable = true, DataCell = new TextBoxCell { Binding = Binding.Property<MixtureRow, string>(row => row.Share) } });
            mixture.Columns.Add(new GridColumn { HeaderText = "Width (mm)", Width = 115, Editable = true, DataCell = new TextBoxCell { Binding = Binding.Property<MixtureRow, string>(row => row.Width) } });
            mixture.ToolTip = "Edit canopy-area shares and individual leaf, leaflet or needle widths. Shares total 100%; they are not tree counts. Do not use whole compound-leaf widths.";
            int[] order = Enumerable.Range(0, Trees.Length).OrderBy(i => Trees[i].Name).ToArray();
            foreach (int i in order) species.Items.Add(new ListItem { Key = Trees[i].Species, Text = Trees[i].Name });
            species.Items.Add(new ListItem { Key = "Custom", Text = "Custom" });
            species.SelectedKey = Trees[0].Species;
            var add = new Button { Text = "Add" };
            var removeTree = new Button { Text = "Remove" };
            add.Click += (sender, e) =>
            {
                string key = species.SelectedKey;
                if (key == null) return;
                int existing = rows.FindIndex(row => row.Species == key);
                if (existing >= 0) { mixture.SelectedRow = existing; return; }
                Tree tree = Trees.FirstOrDefault(value => value.Species == key);
                rows.Add(new MixtureRow { Species = key, Name = tree?.Name ?? "Custom", Share = rows.Count == 0 ? "100" : "0", Width = (tree == null ? 50 : (tree.WidthMin + tree.WidthMax) / 2).ToString("0.##", CultureInfo.CurrentCulture) });
                mixture.DataStore = null; mixture.DataStore = rows; mixture.SelectedRow = rows.Count - 1;
                Changed(sender, e);
            };
            removeTree.Click += (sender, e) =>
            {
                int row = mixture.SelectedRow;
                if (row < 0 || row >= rows.Count) return;
                rows.RemoveAt(row); mixture.DataStore = null; mixture.DataStore = rows;
                Changed(sender, e);
            };
            species.SelectedIndexChanged += (sender, e) =>
            {
                Tree tree = Trees.FirstOrDefault(value => value.Species == species.SelectedKey);
                string width = tree == null ? "" : tree.WidthMin.ToString("0.##", CultureInfo.CurrentCulture) + (tree.WidthMin == tree.WidthMax ? "" : "–" + tree.WidthMax.ToString("0.##", CultureInfo.CurrentCulture));
                dimensions.Text = tree == null ? "Enter a representative leaf width." : string.Format(CultureInfo.CurrentCulture, "{0}: height {1:F1}–{2:F1} m; {3} width {4} mm.", tree.Species, tree.HeightMin, tree.HeightMax, tree.Leaflet ? "leaflet" : "leaf", width);
                dimensions.ToolTip = tree == null ? "" : tree.Source + (tree.Leaflet ? "\nLeaflet width: " : "\nLeaf width: ") + tree.WidthSource + "\nWidth defaults to the published range midpoint, not a measured stand average.";
            };
            species.SelectedIndex = -1; species.SelectedKey = Trees[0].Species;
            rows.Add(new MixtureRow { Species = Trees[0].Species, Name = Trees[0].Name, Share = "100", Width = "105" });
            mixture.DataStore = rows;
            mixture.CellEdited += (sender, e) => Changed(sender, e);
            layout.Items.Add(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 5, Items = { species, add, removeTree } });
            layout.Items.Add(dimensions); layout.Items.Add(mixture);
            layout.Items.Add(total);
            layout.Items.Add(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 8, Items = { new Label { Text = "Canopy base / top (m)", Width = 170 }, canopyBase, height } });
            layout.Items.Add(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 8, Items = { leafOn, preview } });
            densityFrom.Items.Add("Trees per hectare"); densityFrom.Items.Add("Leaf area density"); densityFrom.SelectedIndex = 0;
            var treeInputs = new StackLayout { Spacing = 5, Items = {
                new StackLayout { Orientation = Orientation.Horizontal, Spacing = 8, Items = { new Label { Text = "Trees / ha", Width = 170 }, treeCount } },
                new StackLayout { Orientation = Orientation.Horizontal, Spacing = 8, Items = { new Label { Text = "Crown diameter (m)", Width = 170, ToolTip = "Typical circular crown diameter for the mixture. Enter a site value; this is not set from tree height." }, crownDiameter } },
                new StackLayout { Orientation = Orientation.Horizontal, Spacing = 8, Items = { new Label { Text = "Assumed crown LAI", Width = 170, ToolTip = "One-sided leaf area per projected crown area. Initial 5 is an editable assumption, not a measured or species-specific value. Applies to every species in this mixture; overlapping crowns add leaf area." }, crownLAI } }, densitySummary } };
            var directInput = new StackLayout { Visible = false, Orientation = Orientation.Horizontal, Spacing = 8, Items = { new Label { Text = "Leaf area density (m²/m³)", Width = 170, ToolTip = "One-sided leaf area per cubic metre of canopy, including gaps. Mean LAD = stand LAI / canopy depth." }, density } };
            layout.Items.Add(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 8, Items = { new Label { Text = "Density from", Width = 170 }, densityFrom } });
            layout.Items.Add(treeInputs); layout.Items.Add(directInput);
            densityFrom.SelectedIndexChanged += (sender, e) => { mixture.CommitEdit(); treeInputs.Visible = densityFrom.SelectedIndex == 0; directInput.Visible = !treeInputs.Visible; Changed(sender, e); };
            layout.Items.Add(remove); layout.Items.Add(status);
            var output = new StackLayout { Orientation = Orientation.Horizontal, Spacing = 5 };
            string[] bands = { "63", "125", "250", "500", "1k", "2k", "4k", "8k" };
            for (int i = 0; i < 8; i++)
            {
                results[i] = new Label { Text = "—", TextAlignment = TextAlignment.Center, Width = 42 };
                output.Items.Add(new StackLayout { Items = { new Label { Text = bands[i], TextAlignment = TextAlignment.Center }, results[i] } });
            }
            medium.Items.Add("Hybrid"); medium.Items.Add("Mapping"); medium.SelectedIndex = PachHybridControl.Instance != null || Pach_Mapping_Control.Instance == null ? 0 : 1;
            var refresh = new Button { Text = "Refresh" };
            refresh.Click += (sender, e) => UpdateReadout();
            layout.Items.Add(new Label { Text = "Example loss (dB)", ToolTip = "Loss through the distance below. This display distance does not change the simulation. Octaves are interpolated on log frequency for third-octave simulations." });
            layout.Items.Add(new StackLayout { Orientation = Orientation.Horizontal, Spacing = 8, Items = { new Label { Text = "Example distance (m)", ToolTip = "Travel inside foliage used only for the loss readout. Simulations use actual accumulated path distance." }, distance, medium, refresh } });
            layout.Items.Add(mediumInfo); layout.Items.Add(output);
            Content = layout;
            height.ValueChanged += (sender, e) => { canopyBase.MaxValue = Math.Max(0, height.Value - .01); if (canopyBase.Value > canopyBase.MaxValue) canopyBase.Value = canopyBase.MaxValue; Changed(sender, e); };
            treeCount.ValueChanged += Changed; crownDiameter.ValueChanged += Changed; crownLAI.ValueChanged += Changed;
            canopyBase.ValueChanged += Changed; density.ValueChanged += Changed; leafOn.CheckedChanged += Changed; preview.CheckedChanged += Changed;
            remove.CheckedChanged += Changed;
            distance.ValueChanged += (sender, e) => UpdateReadout(); medium.SelectedIndexChanged += (sender, e) => UpdateReadout();
            UnLoad += (sender, e) => EndPreview();
            Changed(null, EventArgs.Empty);
        }

        bool TryGetSettings(out Settings settings)
        {
            settings = null;
            double[] shares = new double[rows.Count], widths = new double[rows.Count];
            for (int i = 0; i < rows.Count; i++)
            {
                if (!double.TryParse(rows[i].Share, NumberStyles.Float, CultureInfo.CurrentCulture, out shares[i]) || shares[i] < 0 || shares[i] > 100 || double.IsNaN(shares[i]) ||
                    !double.TryParse(rows[i].Width, NumberStyles.Float, CultureInfo.CurrentCulture, out widths[i]) || !(widths[i] > 0) || double.IsInfinity(widths[i]))
                { total.Text = ""; status.Text = "Enter shares from 0 to 100 and positive leaf widths."; return false; }
            }
            double sum = shares.Sum();
            total.Text = "Canopy total: " + sum.ToString("0.##", CultureInfo.CurrentCulture) + "%";
            if (Math.Abs(sum - 100) > 1e-6) { status.Text = "Canopy shares must total 100%."; return false; }
            if (canopyBase.Value >= height.Value) { status.Text = "Canopy base must be below the top."; return false; }
            double leafAreaDensity = density.Value;
            if (densityFrom.SelectedIndex == 0)
            {
                try { leafAreaDensity = Environment.Foliage_Volume.EstimateLeafAreaDensity(treeCount.Value, crownDiameter.Value, crownLAI.Value, height.Value - canopyBase.Value); }
                catch (ArgumentOutOfRangeException) { densitySummary.Text = "—"; status.Text = "Enter a positive crown diameter for a nonzero tree count."; return false; }
                densitySummary.Text = "Calculated leaf area density: " + leafAreaDensity.ToString("0.###", CultureInfo.CurrentCulture) + " m²/m³";
            }
            int[] selected = Enumerable.Range(0, rows.Count).Where(i => shares[i] > 0).ToArray();
            settings = new Settings { Species = selected.Length == 1 ? rows[selected[0]].Species : "Mixed", Height = height.Value, CanopyBase = canopyBase.Value, LeafLength = leafLength, LeafWidth = selected.Sum(i => widths[i] * shares[i] / 100), LeafAreaDensity = leafAreaDensity, FromTreeCount = densityFrom.SelectedIndex == 0, TreesPerHectare = treeCount.Value, CrownDiameter = crownDiameter.Value, CrownLeafAreaIndex = crownLAI.Value, LeafOn = leafOn.Checked == true, Preview = preview.Checked == true, Estimated = true,
                MixtureSpecies = selected.Select(i => rows[i].Species).ToArray(), MixtureWidths = selected.Select(i => widths[i]).ToArray(), MixtureShares = selected.Select(i => shares[i] / 100).ToArray() };
            return true;
        }

        void Changed(object sender, EventArgs e)
        {
            if (loading || saving) return;
            dirty |= objects.Count > 0;
            if (remove.Checked == true)
            {
                status.Text = "Foliage will be removed on close.";
                FoliageConduit.Active?.SetPreview(this, document, objects, null);
            }
            else if (TryGetSettings(out Settings settings))
            {
                status.Text = settings.LeafAreaDensity == 0 ? "Density 0: no estimated loss." : dirty ? "Changes save on close." : "";
                if (dirty) FoliageConduit.Active?.SetPreview(this, document, objects, settings);
            }
            else EndPreview();
            UpdateReadout();
        }

        void UpdateReadout()
        {
            if (loading || saving) return;
            if (remove.Checked == true) { mediumInfo.Text = "Foliage removed"; foreach (Label label in results) label.Text = 0.0.ToString("0.0", CultureInfo.CurrentCulture); return; }
            if (!TryGetSettings(out Settings settings)) { foreach (Label label in results) label.Text = "—"; return; }
            Medium_Properties_Group environment = medium.SelectedIndex == 0 ? PachHybridControl.Instance?.MediumProps : Pach_Mapping_Control.Instance?.Env_Control;
            if (environment == null) { mediumInfo.Text = "Open " + (medium.SelectedIndex == 0 ? "Hybrid" : "Mapping") + " to read its medium."; foreach (Label label in results) label.Text = "—"; return; }
            double speed = Utilities.AcousticalMath.SoundSpeed(environment.Temp_Celsius);
            double[] loss = Environment.Foliage_Volume.Estimate_Aylor(settings.MixtureWidths.Select(value => value * .001).ToArray(), settings.MixtureShares, settings.LeafAreaDensity, speed);
            for (int band = 0; band < 8; band++) loss[band] *= Math.Sqrt(distance.Value);
            mediumInfo.Text = (medium.SelectedIndex == 0 ? "Hybrid" : "Mapping") + " medium · " + speed.ToString("0.0", CultureInfo.CurrentCulture) + " m/s";
            for (int band = 0; band < 8; band++) results[band].Text = loss[band].ToString("0.0", CultureInfo.CurrentCulture);
        }

        internal void Load_Doc(List<RhinoObject> selected)
        {
            if (saving) return;
            selected = selected ?? new List<RhinoObject>();
            bool same = objects.Count == selected.Count && objects.Select(obj => obj.Id).SequenceEqual(selected.Select(obj => obj.Id));
            if (same && dirty) { objects = selected.ToList(); if (objects.Count > 0) document = objects[0].Document ?? document; return; }
            if (!same && dirty && !Commit()) { status.Text += " Previous selection is still being edited."; return; }
            EndPreview(); loading = true;
            try
            {
                objects = selected.ToList(); document = objects.Count == 0 ? null : objects[0].Document; Enabled = objects.Count > 0; dirty = false;
                if (objects.Count == 0) return;
                string code = objects[0].Geometry.GetUserString(Key);
                bool mixed = objects.Any(obj => obj.Geometry.GetUserString(Key) != code);
                bool recognized = Settings.TryRead(code, out Settings settings);
                settings = settings ?? new Settings { Species = Trees[0].Species, Height = 20.574, LeafLength = 88.9, LeafWidth = 105, LeafOn = true, Preview = true, FromTreeCount = true, CrownLeafAreaIndex = 5 };
                rows.Clear();
                string[] names = settings.MixtureSpecies ?? new[] { Trees.Any(tree => tree.Species == settings.Species) ? settings.Species : "Custom" };
                for (int i = 0; i < names.Length; i++)
                {
                    Tree tree = Trees.FirstOrDefault(value => value.Species == names[i]);
                    rows.Add(new MixtureRow { Species = names[i], Name = tree?.Name ?? "Custom", Share = (settings.MixtureShares == null ? 100 : 100 * settings.MixtureShares[i]).ToString("R", CultureInfo.CurrentCulture), Width = (settings.MixtureWidths == null ? settings.LeafWidth : settings.MixtureWidths[i]).ToString("R", CultureInfo.CurrentCulture) });
                }
                mixture.DataStore = null; mixture.DataStore = rows;
                height.MaxValue = Math.Max(1000, settings.Height); height.Value = settings.Height; canopyBase.MaxValue = Math.Max(0, settings.Height - .01); canopyBase.Value = settings.CanopyBase; leafLength = settings.LeafLength;
                density.MaxValue = Math.Max(1000, settings.LeafAreaDensity); density.Value = settings.LeafAreaDensity; leafOn.Checked = settings.LeafOn; preview.Checked = settings.Preview;
                treeCount.MaxValue = Math.Max(1000000, settings.TreesPerHectare); treeCount.Value = settings.TreesPerHectare;
                crownDiameter.MaxValue = Math.Max(1000, settings.CrownDiameter); crownDiameter.Value = settings.CrownDiameter;
                crownLAI.MaxValue = Math.Max(1000, settings.CrownLeafAreaIndex); crownLAI.Value = settings.FromTreeCount ? settings.CrownLeafAreaIndex : 5;
                densityFrom.SelectedIndex = settings.FromTreeCount ? 0 : 1;
                remove.Checked = false;
                status.Text = mixed ? "Mixed settings. Editing replaces them on selection." : !recognized && !string.IsNullOrEmpty(code) ? "Unrecognized foliage. Editing replaces it." : settings.LegacyEstimated ? "Legacy estimate updated to cumulative distance." : recognized && !settings.Estimated ? "Legacy data is retained until edits are saved." : "";
            }
            finally { loading = false; }
            UpdateReadout();
        }

        internal bool Commit()
        {
            if (saving) return true;
            mixture.CommitEdit();
            if (!dirty || objects.Count == 0) return true;
            bool deleting = remove.Checked == true;
            Settings settings = null;
            if (!deleting && !TryGetSettings(out settings)) return false;
            RhinoDoc doc = document;
            if (doc == null) { status.Text = "Document unavailable."; return false; }
            Guid[] ids = objects.Select(obj => obj.Id).ToArray();
            // Validate the entire selection before writing any object.
            foreach (Guid id in ids)
            {
                RhinoObject obj = doc.Objects.FindId(id);
                if (obj == null || obj.IsLocked || obj.IsReference) { status.Text = "Selection contains an unavailable surface."; return false; }
                if (!deleting) using (Brep volume = FoliageConduit.CreateVolume(obj.Geometry, settings, doc)) if (volume == null) { status.Text = "A surface cannot form a vertical canopy."; return false; }
            }
            saving = true;
            uint undo = doc.BeginUndoRecord(deleting ? "Remove Pachyderm foliage" : "Assign Pachyderm foliage");
            try
            {
                foreach (Guid id in ids)
                {
                    RhinoObject obj = doc.Objects.FindId(id);
                    using (GeometryBase geometry = obj.DuplicateGeometry())
                    {
                        geometry.SetUserString(Key, deleting ? null : settings.Code);
                        if (!doc.Objects.Replace(id, geometry, false)) { status.Text = "Could not save foliage."; return false; }
                    }
                }
                dirty = false;
            }
            finally { saving = false; if (undo != 0) doc.EndUndoRecord(undo); }
            EndPreview(); doc.Views.Redraw(); return true;
        }

        internal void EndPreview() { FoliageConduit.Active?.ClearPreview(this); }
        protected override void Dispose(bool disposing) { if (disposing) { Commit(); EndPreview(); } base.Dispose(disposing); }
    }

    internal sealed class FoliageConduit : DisplayConduit, IDisposable
    {
        sealed class Volume : IDisposable
        {
            internal RhinoObject Object;
            internal Guid Id;
            internal uint Serial;
            internal Brep Shape, Bottom;
            internal bool LeafOn;
            public void Dispose() { Shape.Dispose(); Bottom?.Dispose(); }
        }
        internal static FoliageConduit Active { get; private set; }
        readonly Dictionary<FoliageProperties, (RhinoDoc Document, Guid[] Objects, FoliageProperties.Settings Settings)> drafts = new Dictionary<FoliageProperties, (RhinoDoc, Guid[], FoliageProperties.Settings)>();
        readonly Dictionary<uint, List<Volume>> cache = new Dictionary<uint, List<Volume>>();
        readonly DisplayMaterial summer = new DisplayMaterial(System.Drawing.Color.ForestGreen, .75);
        readonly DisplayMaterial winter = new DisplayMaterial(System.Drawing.Color.Sienna, .85);

        internal FoliageConduit()
        {
            Active = this;
            RhinoDoc.AddRhinoObject += ObjectChanged;
            RhinoDoc.DeleteRhinoObject += ObjectChanged;
            RhinoDoc.UndeleteRhinoObject += ObjectChanged;
            RhinoDoc.ReplaceRhinoObject += Replaced;
            RhinoDoc.ModifyObjectAttributes += AttributesChanged;
            RhinoDoc.DocumentPropertiesChanged += DocumentChanged;
            RhinoDoc.CloseDocument += DocumentChanged;
            Enabled = true;
        }

        internal static Brep CreateVolume(GeometryBase geometry, FoliageProperties.Settings settings, RhinoDoc doc)
        {
            // Translate the trimmed terrain in world Z, then extrude its face vertically with both caps.
            using (Brep brep = geometry is Brep source ? source.DuplicateBrep() : Brep.TryConvertBrep(geometry))
            {
                if (brep == null || !brep.IsValid || brep.IsSolid || brep.Faces.Count != 1) return null;
                if (settings.CanopyBase < 0 || settings.CanopyBase >= settings.Height) return null;
                double scale = Utilities.RCPachTools.GetModelToMetersScale(doc);
                brep.Transform(Transform.Translation(0, 0, settings.CanopyBase / scale));
                Point3d start = brep.Faces[0].PointAt(brep.Faces[0].Domain(0).Mid, brep.Faces[0].Domain(1).Mid);
                using (var path = new LineCurve(start, start + Vector3d.ZAxis * ((settings.Height - settings.CanopyBase) / scale)))
                {
                    Brep volume = brep.Faces[0].CreateExtrusion(path, true);
                    if (volume != null && volume.IsValid && volume.IsSolid) return volume;
                    volume?.Dispose();
                    return null;
                }
            }
        }

        internal static Environment.Foliage_Field BuildField(List<RhinoObject> objects)
        {
            var volumes = new List<Environment.Foliage_Volume>();
            foreach (RhinoObject obj in objects)
            {
                string code = obj.Geometry.GetUserString(FoliageProperties.Key);
                if (string.IsNullOrEmpty(code)) continue;
                if (!FoliageProperties.Settings.TryRead(code, out FoliageProperties.Settings settings)) throw new InvalidOperationException("Unrecognized foliage assignment on " + obj.Id + ". Reapply its terrain settings.");
                if (settings.Estimated ? settings.LeafAreaDensity == 0 : settings.Attenuation == null || settings.Attenuation.All(value => value == 0)) continue;
                using (Brep shape = CreateVolume(obj.Geometry, settings, obj.Document))
                using (var mesh = new Mesh())
                {
                    if (shape == null) throw new InvalidOperationException("Cannot create the foliage volume for " + obj.Id + ".");
                    double scale = Utilities.RCPachTools.GetModelToMetersScale(obj.Document);
                    shape.Transform(Transform.Scale(Point3d.Origin, scale));
                    // Keep shared Brep edges closed and preserve survey-coordinate precision in both mesh stages.
                    mesh.Vertices.UseDoublePrecisionVertices = true;
                    var parameters = new MeshingParameters { JaggedSeams = false, ClosedObjectPostProcess = true, DoublePrecision = true, SimplePlanes = true, Tolerance = obj.Document.ModelAbsoluteTolerance * scale };
                    foreach (Mesh part in Mesh.CreateFromBrep(shape, parameters) ?? Array.Empty<Mesh>())
                        using (part) mesh.Append(part);
                    mesh.Vertices.CombineIdentical(true, true);
                    mesh.Faces.ConvertQuadsToTriangles();
                    mesh.UnifyNormals();
                    if (mesh.SolidOrientation() < 0) mesh.Flip(true, true, true);
                    if (!mesh.IsValid || !mesh.IsClosed) throw new InvalidOperationException("The foliage canopy is a closed solid, but its simulation mesh failed for surface " + obj.Id + " (valid: " + mesh.IsValid + ", closed: " + mesh.IsClosed + ".");
                    Hare.Geometry.Topology topology = Utilities.RCPachTools.RhinotoHareMesh(mesh);
                    double tolerance = Math.Max(1e-9, obj.Document.ModelAbsoluteTolerance * scale);
                    volumes.Add(settings.Estimated ? new Environment.Foliage_Volume(topology, settings.MixtureWidths == null ? new[] { settings.LeafWidth * .001 } : settings.MixtureWidths.Select(value => value * .001).ToArray(), settings.MixtureShares ?? new[] { 1.0 }, settings.LeafAreaDensity, tolerance) : new Environment.Foliage_Volume(topology, settings.Attenuation, tolerance));
                }
            }
            return volumes.Count == 0 ? null : new Environment.Foliage_Field(volumes);
        }

        internal void SetPreview(FoliageProperties editor, RhinoDoc doc, List<RhinoObject> objects, FoliageProperties.Settings settings)
        {
            if (doc == null) { ClearPreview(editor); return; }
            Guid[] ids = objects.Select(obj => obj.Id).ToArray();
            bool unchanged = drafts.TryGetValue(editor, out var previous) && previous.Document == doc && previous.Objects.SequenceEqual(ids) &&
                (previous.Settings == null && settings == null || previous.Settings != null && settings != null && previous.Settings.Height == settings.Height && previous.Settings.CanopyBase == settings.CanopyBase && previous.Settings.Preview == settings.Preview && previous.Settings.LeafOn == settings.LeafOn);
            if (previous.Document != null && previous.Document != doc) { Invalidate(previous.Document); previous.Document.Views.Redraw(); }
            drafts[editor] = (doc, ids, settings);
            if (!unchanged) Invalidate(doc);
            doc.Views.Redraw();
        }

        internal void ClearPreview(FoliageProperties editor)
        {
            if (!drafts.TryGetValue(editor, out var draft)) return;
            drafts.Remove(editor);
            if (draft.Document != null) { Invalidate(draft.Document); draft.Document.Views.Redraw(); }
        }

        void Invalidate(RhinoDoc doc)
        {
            if (doc != null && cache.TryGetValue(doc.RuntimeSerialNumber, out List<Volume> volumes))
            {
                foreach (Volume volume in volumes) volume.Dispose();
                cache.Remove(doc.RuntimeSerialNumber);
            }
        }
        void ObjectChanged(object sender, RhinoObjectEventArgs e) { Invalidate(e.TheObject?.Document ?? sender as RhinoDoc); }
        void Replaced(object sender, RhinoReplaceObjectEventArgs e) { Invalidate(e.Document); }
        void AttributesChanged(object sender, RhinoModifyObjectAttributesEventArgs e) { Invalidate(e.Document); }
        void DocumentChanged(object sender, DocumentEventArgs e) { Invalidate(e.Document); }

        List<Volume> Volumes(RhinoDoc doc)
        {
            if (cache.TryGetValue(doc.RuntimeSerialNumber, out List<Volume> volumes))
            {
                // Headless documents do not always deliver the ordinary object events.
                // Check only cached object identities; unchanged redraws never repeat offsets.
                if (volumes.All(volume => doc.Objects.FindId(volume.Id)?.RuntimeSerialNumber == volume.Serial)) return volumes;
                Invalidate(doc);
            }
            volumes = new List<Volume>();
            cache.Add(doc.RuntimeSerialNumber, volumes);
            RhinoObject[] assigned = doc.Objects.FindByUserString(FoliageProperties.Key, "*", true, true, false, ObjectType.Surface | ObjectType.Brep | ObjectType.Extrusion);
            var candidates = (assigned ?? Array.Empty<RhinoObject>()).Concat(drafts.Values.Where(draft => draft.Document == doc).SelectMany(draft => draft.Objects.Select(id => doc.Objects.FindId(id))).Where(obj => obj != null)).GroupBy(obj => obj.Id).Select(group => group.First());
            foreach (RhinoObject obj in candidates)
            {
                FoliageProperties.Settings settings = null;
                bool drafted = false;
                foreach (var draft in drafts.Values)
                    if (draft.Document == doc && draft.Objects.Contains(obj.Id)) { settings = draft.Settings; drafted = true; }
                if (!drafted) FoliageProperties.Settings.TryRead(obj.Geometry.GetUserString(FoliageProperties.Key), out settings);
                if (settings == null || !settings.Preview) continue;
                Brep shape = CreateVolume(obj.Geometry, settings, doc);
                if (shape == null) continue;
                Brep bottom = obj.Geometry is Brep source ? source.DuplicateBrep() : Brep.TryConvertBrep(obj.Geometry);
                if (bottom != null) bottom.Transform(Transform.Translation(0, 0, settings.CanopyBase / Utilities.RCPachTools.GetModelToMetersScale(doc)));
                volumes.Add(new Volume { Object = obj, Id = obj.Id, Serial = obj.RuntimeSerialNumber, Shape = shape, Bottom = bottom, LeafOn = settings.LeafOn });
            }
            return volumes;
        }

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            foreach (Volume volume in Volumes(e.RhinoDoc)) if (volume.Object.Visible && volume.Object.IsActiveInViewport(e.Viewport)) e.IncludeBoundingBox(volume.Shape.GetBoundingBox(false));
        }

        protected override void PostDrawObjects(DrawEventArgs e)
        {
            foreach (Volume volume in Volumes(e.RhinoDoc))
            {
                if (!volume.Object.Visible || !volume.Object.IsActiveInViewport(e.Viewport)) continue;
                e.Display.DrawBrepShaded(volume.Shape, volume.LeafOn ? summer : winter);
                e.Display.DrawBrepWires(volume.Shape, volume.LeafOn ? System.Drawing.Color.ForestGreen : System.Drawing.Color.Sienna);
                if (volume.Bottom != null) e.Display.DrawBrepWires(volume.Bottom, System.Drawing.Color.DarkOrange, 2);
            }
        }

        public void Dispose()
        {
            Enabled = false;
            RhinoDoc.AddRhinoObject -= ObjectChanged;
            RhinoDoc.DeleteRhinoObject -= ObjectChanged;
            RhinoDoc.UndeleteRhinoObject -= ObjectChanged;
            RhinoDoc.ReplaceRhinoObject -= Replaced;
            RhinoDoc.ModifyObjectAttributes -= AttributesChanged;
            RhinoDoc.DocumentPropertiesChanged -= DocumentChanged;
            RhinoDoc.CloseDocument -= DocumentChanged;
            foreach (List<Volume> volumes in cache.Values) foreach (Volume volume in volumes) volume.Dispose();
            cache.Clear(); drafts.Clear(); summer.Dispose(); winter.Dispose();
            if (ReferenceEquals(Active, this)) Active = null;
        }
    }
}

namespace Pachyderm_Acoustic.Environment
{
    internal sealed class RhCommon_FoliagePolygonScene : RhCommon_PolygonScene
    {
        readonly Foliage_Field foliage;
        internal RhCommon_FoliagePolygonScene(List<RhinoObject> objects, bool edges, double temp, double humidity, double pressure, int air, bool edgeCorrection, bool failsafes, Foliage_Field foliage)
            : base(objects, edges, temp, humidity, pressure, air, edgeCorrection, failsafes) { this.foliage = foliage; foliage.SetSoundSpeed(Sound_speed(0)); }
        public override Foliage_Field Foliage => foliage;
        public override void AttenuatePath(double[] spectrum, Hare.Geometry.Point start, Hare.Geometry.Point end, bool pressure = false) { foliage.Attenuate(spectrum, start, end, pressure); }
        public override void AttenuatePath(double[] spectrum, Hare.Geometry.Point[] vertices, bool pressure = false) { foliage.Attenuate(spectrum, vertices, pressure); }
    }

    internal sealed class RhCommon_FoliageScene : RhCommon_Scene
    {
        readonly Foliage_Field foliage;
        internal RhCommon_FoliageScene(List<RhinoObject> objects, double temp, double humidity, double pressure, int air, bool edgeCorrection, bool failsafes, Foliage_Field foliage)
            : base(objects, temp, humidity, pressure, air, edgeCorrection, failsafes) { this.foliage = foliage; foliage.SetSoundSpeed(Sound_speed(0)); }
        public override Foliage_Field Foliage => foliage;
        public override void AttenuatePath(double[] spectrum, Hare.Geometry.Point start, Hare.Geometry.Point end, bool pressure = false) { foliage.Attenuate(spectrum, start, end, pressure); }
        public override void AttenuatePath(double[] spectrum, Hare.Geometry.Point[] vertices, bool pressure = false) { foliage.Attenuate(spectrum, vertices, pressure); }
    }
}
