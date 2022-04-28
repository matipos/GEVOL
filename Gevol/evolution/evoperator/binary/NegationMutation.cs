using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.binary
{
    /// <summary>
    /// If mutation appear then the gene is negated.
    /// </summary>
    public class NegationMutation : Operator
    {
        private double _mutationProbability = 0.05; //Probability to appear mutation.

        /// <summary>
        /// Probability to appear mutation on the gene. Default value is 0.05 (5 percent).
        /// </summary>
        public double MutationProbability { get { return _mutationProbability; } set { _mutationProbability = value; } }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NegationMutation() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mutationProbability">Probability to apper mutation on the gene.</param>
        public NegationMutation(double mutationProbability)
        {
            this.MutationProbability = mutationProbability;
        }

        /// <summary>
        /// Run mutation for population on every gene of every individual.
        /// If mutation appear and the gene is 1 then change to 0 and if the gene is 0 then change to 1.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Population Apply(Population population)
        {
            if (population.Count == 0)
            {
                return population;
            }
            if (!(population[0] is BinaryIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of BinaryIndividual.", "population");
            }
            Population newPopulation = new Population();

            for (int k = 0; k < population.Count; k++)
            {
                for (int i = 0; i < ((IList<int>)population[k].Chromosome).Count; i++)
                {
                    if (Randomizer.NextDouble(0, 1) < this.MutationProbability)
                    {
                        if (((IList<int>)population[k].Chromosome)[i] == 0)
                        {
                            ((IList<int>)population[k].Chromosome)[i] = 1;
                        }
                        else
                        {
                            ((IList<int>)population[k].Chromosome)[i] = 0;
                        }
                    }
                }
                newPopulation.Add(population[k]);
            }

            return newPopulation;
        }
    }
}
