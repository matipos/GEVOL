using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.individual.real;
using Gevol.evolution.util;

namespace Gevol.evolution.evoperator.real
{
    /// <summary>
    /// Each chromosom is fully copied from random parent.
    /// </summary>
    public class RealRandomSelection : Operator
    {
        public override Population Apply(Population population)
        {
            if (population.Count <= 1)
            {
                return population;
            }
            if (!(population[0] is RealIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of RealIndividual.", "population");
            }
            Population newPopulation = new Population();
            RealIndividual newIndividual = new RealIndividual();
            RealIndividualChromosome newChromosome = new RealIndividualChromosome();
            newChromosome.Age = 0;
            newChromosome.Alpha = ((RealIndividualChromosome)population[Randomizer.NextInt(0, population.Count - 1)].Chromosome).Alpha;
            newChromosome.Sigma = ((RealIndividualChromosome)population[Randomizer.NextInt(0, population.Count - 1)].Chromosome).Sigma;
            newChromosome.Values = ((RealIndividualChromosome)population[Randomizer.NextInt(0, population.Count - 1)].Chromosome).Values;
            newIndividual.Chromosome = newChromosome;
            newPopulation.Add(newIndividual);
            return newPopulation;
        }
    }
}
