﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Models.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Plant xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; Version=&quot;46&quot;&gt;
        ///  &lt;Name&gt;Chicory&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;GeneralDescription&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;MemoText&gt;&lt;![CDATA[
        ///# Presentation
        ///
        ///This model has been developed to simulate the growth of a forage chicory crop.  The chicory model focus, thus, on describing primarily the vegetative growth, with a simplified account of the reproductive phase, without expl [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Chicory {
            get {
                return ResourceManager.GetString("Chicory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Plant Version=&quot;45&quot;&gt;
        ///  &lt;Name&gt;Maize&lt;/Name&gt;
        ///  &lt;OrganArbitrator&gt;
        ///    &lt;Name&gt;Arbitrator&lt;/Name&gt;
        ///    &lt;RelativeAllocation&gt;
        ///      &lt;Name&gt;NArbitrator&lt;/Name&gt;
        ///      &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;/RelativeAllocation&gt;
        ///    &lt;RelativeAllocation&gt;
        ///      &lt;Name&gt;DMArbitrator&lt;/Name&gt;
        ///      &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;/RelativeAllocation&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///  &lt;/OrganArbitrator&gt;
        ///  &lt;Phenology&gt;
        ///    &lt;Name&gt;Phenology&lt;/Name&gt;
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Maize {
            get {
                return ResourceManager.GetString("Maize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Plant&gt;
        ///  &lt;Name&gt;Oats&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;Introduction&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;Enabled&gt;true&lt;/Enabled&gt;
        ///    &lt;ReadOnly&gt;false&lt;/ReadOnly&gt;
        ///    &lt;MemoText&gt;
        ///      &lt;![CDATA[
        ///# The APSIM Oats Model
        /// 
        ///_Allan Peake, Hamish Brown, Rob Zyskowski, Edmar I. Teixeira, Neil Huth_
        ///
        ///The APSIM oats model has been developed using the Plant Modelling Framework (PMF) of [brown_plant_2014]. This new framework provides a library of plant organ and process submodels that  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Oats {
            get {
                return ResourceManager.GetString("Oats", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;Windows-1252&quot;?&gt;
        ///&lt;Nutrient&gt;
        ///  &lt;Name&gt;Nutrient&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;TitlePage&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;MemoText&gt;
        ///      &lt;![CDATA[
        ///# The APSIM Soil Nutrient Model
        ///
        ///The APSIM soil nutrient model has been developed to simulate soil organic matter pools and the flows and losses of soil nutrients.  This model captures the functionality previously provided in the APSIM SoilN model ([probert_apsims_1998]).
        ///
        ///The model consists o [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Nutrient {
            get {
                return ResourceManager.GetString("Nutrient", resourceCulture);
            }
        }        

        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;Windows-1252&quot;?&gt;
        ///&lt;OilPalm Version=&quot;45&quot;&gt;
        ///  &lt;Name&gt;OilPalm&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;Memo&lt;/Name&gt;
        ///    &lt;MemoText&gt;&lt;![CDATA[The base configuration of the oil palm model has been configured to match commercial dura x pisifera palms developed in Dami, West New Britain in Papua New Guinea.  Other varieties are specified in terms of how they differ from this base variety.]]&gt;&lt;/MemoText&gt;
        ///  &lt;/Memo&gt;
        ///  &lt;UnderstoryCoverMax&gt;0.4&lt;/UnderstoryCoverMax&gt;
        ///  &lt;UnderstoryLegumeFraction&gt;1&lt;/Understor [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string OilPalm {
            get {
                return ResourceManager.GetString("OilPalm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Plant Version=&quot;46&quot;&gt;
        ///  &lt;Name&gt;Plantain&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;GeneralDescription&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;Enabled&gt;true&lt;/Enabled&gt;
        ///    &lt;MemoText&gt;&lt;![CDATA[
        ///## Presentation
        ///
        ///This model has been built using the Plant Modelling Framework (PMF) of [brown_plant_2014] to simulate the growth of a forage plantain crop ( _Plantago lanceolata_ ).  The model focus, thus, on describing primarily the vegetative growth, with a si [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Plantain {
            get {
                return ResourceManager.GetString("Plantain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Plant Version=&quot;45&quot;&gt;
        ///  &lt;Name&gt;Potato&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;TitlePage&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;false&lt;/IncludeInDocumentation&gt;
        ///    &lt;MemoText&gt;&lt;![CDATA[
        ///# The APSIM Potato Model
        ///
        ///_Brown, H.E., Huth, N.I. and Holzworth, D.P._
        ///
        ///#Building the model.
        ///The APSIM potato model has been described in part by [Brown_etal_2011] and developed using the Plant Modelling Framework (PMF) of [brown_plant_2014]. This new framework provides a library of plant organ and process submodels that can be coupled, [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Potato {
            get {
                return ResourceManager.GetString("Potato", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;parameters name=&quot;standard&quot; version=&quot;2.0&quot;&gt;
        ///  &lt;par name=&quot;editor&quot;&gt;Andrew Moore&lt;/par&gt;
        ///  &lt;par name=&quot;edited&quot;&gt;30 Jan 2013&lt;/par&gt;
        ///  &lt;par name=&quot;dairy&quot;&gt;false&lt;/par&gt;
        ///  &lt;par name=&quot;c-srs-&quot;&gt;1.2,1.4&lt;/par&gt;
        ///  &lt;par name=&quot;c-i-&quot;&gt;,1.7,,,,25.0,22.0,,,,,0.15,,0.002,0.5,1.0,0.01,20.0,3.0,1.5&lt;/par&gt;
        ///  &lt;par name=&quot;c-r-&quot;&gt;0.8,0.17,1.7,,0.6,,,,0.14,0.28,10.5,0.8,0.35,1.0,0.0,0.0,0.012,1.0,1.0,11.5&lt;/par&gt;
        ///  &lt;par name=&quot;c-k-&quot;&gt;0.5,0.02,0.85,0.7,0.4,0.02,0.6,0.133,0.95,0.84,0.8,0.7,0.035,0.33,0.12,0.043&lt;/par&gt;
        ///  &lt;par name=&quot;c-m-&quot;&gt;0.09,,0. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RUMINANT_PARAM_GLB {
            get {
                return ResourceManager.GetString("RUMINANT_PARAM_GLB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Plant xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; Version=&quot;46&quot;&gt;
        ///  &lt;Name&gt;SCRUM&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;TitlePage&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;MemoText&gt;&lt;![CDATA[
        ///# SCRUM: the Simple Crop Resource Uptake Model
        ///
        ///_Hamish Brown and Rob Zyskowski, Plant and Food Research, New Zealand_
        ///
        ///This model has been built using the Plant Modelling Framework (PMF) of [brown_plant_2014] to simulate a range of different crops [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SCRUM {
            get {
                return ResourceManager.GetString("SCRUM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Plant xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; Version=&quot;46&quot;&gt;
        ///  &lt;Name&gt;Slurp&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;TitlePage&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;MemoText&gt;&lt;![CDATA[
        ///# SLURP: the Sound of a crop using water
        ///
        ///This model has been built using the Plant Modelling Framework (PMF) of [brown_plant_2014] to provide a simple representation of crops.  It is usefull for water and nitrogen balance studies where the focus is  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Slurp {
            get {
                return ResourceManager.GetString("Slurp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name                              |R    DM    DMD    M/D     EE     CP     dg    ADIP     P        S       AA    MaxP Locales
        ///Alfalfa Hay Early-bloom           |Y  0.900  0.640   9.50  0.030  0.200  0.650  0.110  0.00250  0.00300  1.200  0.000 ca;us         es:&quot;Alfalfa Heno florac temprana&quot;
        ///Alfalfa Hay Full-bloom            |Y  0.900  0.610   9.00  0.030  0.170  0.650  0.160  0.00240  0.00300  1.200  0.000 ca;us         es:&quot;Alfalfa Heno plena floración&quot;
        ///Alfalfa Hay Mature                |Y  0.900  0.540  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Supplement {
            get {
                return ResourceManager.GetString("Supplement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;SurfaceOrganicMatter Version=&quot;36&quot;&gt;
        ///  &lt;ResidueTypes&gt;
        ///    &lt;Name&gt;ResidueTypes&lt;/Name&gt;
        ///    &lt;ResidueType&gt;
        ///      &lt;fom_type&gt;base_type&lt;/fom_type&gt;
        ///      &lt;fraction_C description=&quot;fraction of Carbon in FOM (0-1)&quot;&gt;0.4&lt;/fraction_C&gt;
        ///      &lt;po4ppm description=&quot;labile P concentration(ppm)&quot;&gt;0.0&lt;/po4ppm&gt;
        ///      &lt;nh4ppm description=&quot;ammonium N concentration (ppm)&quot;&gt;0.0&lt;/nh4ppm&gt;
        ///      &lt;no3ppm description=&quot;nitrate N concentration (ppm)&quot;&gt;0.0&lt;/no3ppm&gt;
        ///      &lt;specific_area descriptio [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SurfaceOrganicMatter {
            get {
                return ResourceManager.GetString("SurfaceOrganicMatter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Plant Version=&quot;45&quot;&gt;
        ///  &lt;Name&gt;Wheat&lt;/Name&gt;
        ///  &lt;Memo&gt;
        ///    &lt;Name&gt;TitlePage&lt;/Name&gt;
        ///    &lt;IncludeInDocumentation&gt;true&lt;/IncludeInDocumentation&gt;
        ///    &lt;MemoText&gt;&lt;![CDATA[
        ///# The APSIM Wheat Model
        ///
        ///_Brown, H.E., Huth, N.I. and Holzworth, D.P._
        ///
        ///The APSIM wheat model has been developed using the Plant Modelling Framework (PMF) of [brown_plant_2014]. This new framework provides a library of plant organ and process submodels that can be coupled, at runtime, to construct a model in much the same way that models ca [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Wheat {
            get {
                return ResourceManager.GetString("Wheat", resourceCulture);
            }
        }
    }
}
