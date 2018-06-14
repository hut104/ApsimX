﻿
namespace Models.Soils.Nutrients
{
    using Core;
    using Models.PMF.Functions;
    using System;
    using APSIM.Shared.Utilities;
    using System.Collections.Generic;

    /// <summary>
    /// # [Name]
    /// Encapsulates a carbon and nutrient flow between pools.  This flow is characterised in terms of the rate of flow (fraction of the pool per day).  Carbon loss as CO2 is expressed in terms of the efficiency of C retension within the soil.
    /// </summary>
    [Serializable]
    [ValidParent(ParentType = typeof(NutrientPool))]
    [PresenterName("UserInterface.Presenters.PropertyPresenter")]
    [ViewName("UserInterface.Views.GridView")]
    public class CarbonFlow : Model
    {
        private List<NutrientPool> destinations = new List<NutrientPool>();

        [ChildLinkByName]
        private IFunction Rate = null;

        [ChildLinkByName]
        private IFunction CO2Efficiency = null;

        [Link]
        private SoluteManager solutes = null;

        /// <summary>
        /// Net N Mineralisation
        /// </summary>
        public double[] MineralisedN { get; set; }

        /// <summary>
        /// CO2 lost to the atmosphere
        /// </summary>
        public double[] Catm { get; set; }

        /// <summary>
        /// Name of destination pool
        /// </summary>
        [Description("Names of destination pools (CSV)")]
        public string[] destinationNames { get; set; }
        /// <summary>
        /// Fractions for each destination pool
        /// </summary>
        [Description("Fractions of flow to each pool (CSV)")]
        public double[] destinationFraction { get; set; }

        /// <summary>Performs the initial checks and setup</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("Commencing")]
        private void OnSimulationCommencing(object sender, EventArgs e)
        {
            foreach (string destinationName in destinationNames)
            {
                NutrientPool destination = Apsim.Find(this, destinationName) as NutrientPool;
                if (destination == null)
                    throw new Exception("Cannot find destination pool with name: " + destinationName);
                destinations.Add(destination);
            }
            MineralisedN = new double[(Parent as NutrientPool).C.Length];
            Catm = new double[(Parent as NutrientPool).C.Length];
        }

        /// <summary>
        /// Get the information on potential residue decomposition - perform daily calculations as part of this.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EventSubscribe("DoSoilOrganicMatter")]
        private void OnDoSoilOrganicMatter(object sender, EventArgs e)
        {
            NutrientPool source = Parent as NutrientPool;
            double[] NH4 = solutes.GetSolute("NH4");
            double[] NO3 = solutes.GetSolute("NO3");

            for (int i = 0; i < source.C.Length; i++)
            {

                double carbonFlowFromSource = Rate.Value(i) * source.C[i];
                double nitrogenFlowFromSource = MathUtilities.Divide(carbonFlowFromSource, source.CNRatio[i], 0);

                double[] carbonFlowToDestination = new double[destinations.Count];
                double[] nitrogenFlowToDestination = new double[destinations.Count];

                for (int j = 0; j < destinations.Count; j++)
                {
                    carbonFlowToDestination[j] = carbonFlowFromSource * CO2Efficiency.Value(i) * destinationFraction[j];
                    nitrogenFlowToDestination[j] = MathUtilities.Divide(carbonFlowToDestination[j],destinations[j].CNRatio[i],0.0);
                }

                double TotalNitrogenFlowToDestinations = MathUtilities.Sum(nitrogenFlowToDestination);
                // some pools do not fully occupy a layer (e.g. residue decomposition) and so need to incorporate fraction of layer
                double MineralNSupply = (NO3[i] + NH4[i])* source.LayerFraction[i];
                double NSupply = nitrogenFlowFromSource + MineralNSupply;

                if (MathUtilities.Sum(nitrogenFlowToDestination) > NSupply)
                {
                    double NSupplyFactor = MathUtilities.Bound(MathUtilities.Divide(MineralNSupply, TotalNitrogenFlowToDestinations - nitrogenFlowFromSource, 1.0), 0.0, 1.0);

                    for (int j = 0; j < destinations.Count; j++)
                    {
                        carbonFlowToDestination[j] *= NSupplyFactor;
                        nitrogenFlowToDestination[j] *= NSupplyFactor;
                    }
                    TotalNitrogenFlowToDestinations *= NSupplyFactor;

                    carbonFlowFromSource *= NSupplyFactor;
                    nitrogenFlowFromSource *= NSupplyFactor;

                }

                source.C[i] -= carbonFlowFromSource;
                source.N[i] -= nitrogenFlowFromSource;
                Catm[i] = carbonFlowFromSource - MathUtilities.Sum(carbonFlowToDestination);
                for (int j = 0; j < destinations.Count; j++)
                {
                    destinations[j].C[i] += carbonFlowToDestination[j];
                    destinations[j].N[i] += nitrogenFlowToDestination[j];
                }

                if (TotalNitrogenFlowToDestinations <= nitrogenFlowFromSource)
                {
                    MineralisedN[i] = nitrogenFlowFromSource - TotalNitrogenFlowToDestinations;
                    NH4[i] += MineralisedN[i];
                }
                else
                {
                    double NDeficit = TotalNitrogenFlowToDestinations - nitrogenFlowFromSource;
                    double NH4Immobilisation = Math.Min(NH4[i], NDeficit);
                    NH4[i] -= NH4Immobilisation;
                    NDeficit -= NH4Immobilisation;

                    double NO3Immobilisation = Math.Min(NO3[i], NDeficit);
                    NO3[i] -= NO3Immobilisation;
                    NDeficit -= NO3Immobilisation;

                    MineralisedN[i] = -NH4Immobilisation - NO3Immobilisation;

                    if (MathUtilities.IsGreaterThan(NDeficit, 0.0))
                        throw new Exception("Insufficient mineral N for immobilisation demand for C flow " + Name);
                }

            }
            solutes.SetSolute("NH4", SoluteManager.SoluteSetterType.Soil, NH4);
            solutes.SetSolute("NO3", SoluteManager.SoluteSetterType.Soil, NO3);
        }


    }
}