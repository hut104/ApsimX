﻿using System;
using System.Collections.Generic;
using APSIM.Shared.Utilities;
using Models.Core;
using Models.Interfaces;
using Models.Utilities;
using Newtonsoft.Json;

namespace Models.Soils
{
    /// <summary>A model for holding layer structure information</summary>
    [Serializable]
    [ValidParent(ParentType = typeof(Soil))]
    [ViewName("ApsimNG.Resources.Glade.ProfileView.glade")]
    [PresenterName("UserInterface.Presenters.ProfilePresenter")]
    public class LayerStructure : Model, IGridTable
    {
        /// <summary>Depth strings. Wrapper around Thickness.</summary>
        [Summary]
        [Units("mm")]
        [JsonIgnore]
        public string[] Depth
        {
            get => SoilUtilities.ToDepthStrings(Thickness);
            set => Thickness = SoilUtilities.ToThickness(value);
        }

        /// <summary>Gets or sets the thickness.</summary>
        public double[] Thickness { get; set; }

        /// <summary>Tabular data. Called by GUI.</summary>
        [JsonIgnore]
        public List<GridTable> Tables
        {
            get
            {
                var columns = new List<GridTable.Column>();
                columns.Add(new GridTable.Column("Depth", new VariableProperty(this, GetType().GetProperty("Depth")), readOnly: false));

                List<GridTable> tables = new List<GridTable>();
                tables.Add(new GridTable(Name, columns, this));

                return tables;
            }
        }
    }
}
