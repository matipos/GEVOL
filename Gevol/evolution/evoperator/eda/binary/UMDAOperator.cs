using Gevol.evolution.evoperator.eda.model;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.eda.binary
{
    /// <summary>
    /// Implements operator for discrete UMDA.
    /// Model is a list of double what determines probability to get 1 for each gene.
    /// </summary>
    public class UMDAOperator : EdaOperator
    {
        public int _newPopulationSize = -1;
        
        /// <summary>
        /// Size of new population. 
        /// How big population should be generated.
        /// </summary>
        public int NewPopulationSize
        {
            get { return _newPopulationSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("NewPopulationSize", value, "Size must be greater than 0.");
                }
                else { _newPopulationSize = value; }
            }
        }

        public UMDAOperator(int newPopulationSize)
        {
            NewPopulationSize = newPopulationSize;
        }

        public override Population Apply(Population population)
        {
            if (!(population[0] is BinaryIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of BinaryIndividual.", "population");
            }
            CalculateModel(population);

            return GeneratePopulation();
        }

        protected override void CalculateModel(individual.Population population)
        {
            int chromosomeLength = ((IList<int>)((BinaryIndividual)population[0]).Chromosome).Count;
            UMDAModel model = new UMDAModel();
            for (int i = 0; i < chromosomeLength; i++)
            {
                int sumOfOne = 0;
                foreach (BinaryIndividual individual in population)
                {
                    if (((IList<int>)individual.Chromosome)[i] == 1)
                    {
                        sumOfOne++;
                    }
                }
                model.probabilities.Add((double)sumOfOne / (double)population.Count);
            }
            this.Model = model;
        }

        protected Population GeneratePopulation()
        {
            Population population = new Population();
            UMDAModel model = (UMDAModel)Model;
            for (int i = 0; i < NewPopulationSize; i++)
            {
                IList<int> chromosome = new List<int>(model.probabilities.Count);
                for (int k = 0; k < model.probabilities.Count; k++)
                {
                    chromosome.Add(Randomizer.NextBoolIntProbability(model.probabilities[k]));
                }
                population.Add(new BinaryIndividual(chromosome));
            }
            return population;
        }
    }

}
