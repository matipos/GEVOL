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
    /// Randomly select two parents from population: p1 and p2
    /// Random number u between 0 and 1
    /// Every new gene g is calculated according to the formula:
    /// g = u * p1 + (1 - u) * p2
    /// </summary>
    public class RealLocalIntermediaryRecombination : Operator
    {
        public override Population Apply(Population population)
        {
            if (population.Count <= 1)
            {
                throw new ArgumentException("Population is too small. Minimum expected size is two individuals in the population.", "population");
            }
            if (!(population[0] is RealIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of RealIndividual.", "population");
            }
            int indv1, indv2 = -1;
            //rand parents
            indv1 = Randomizer.NextInt(0, population.Count - 1);
            indv2 = indv1;
            while(indv1 == indv2)
            {
                indv2 = Randomizer.NextInt(0, population.Count - 1);
            }
            //rand modify parameter
            double u = Randomizer.NextDouble(0, 1);
            //generate new individual
            RealIndividualChromosome p1chromosome = (RealIndividualChromosome)population[indv1].Chromosome;
            RealIndividualChromosome p2chromosome = (RealIndividualChromosome)population[indv2].Chromosome;
            RealIndividualChromosome newChromosome = new RealIndividualChromosome(p1chromosome.Sigma.Count, p1chromosome.Alpha.Count);
            newChromosome.Age = 0;
            for (int i = 0; i < p1chromosome.Sigma.Count; i++)
            {
                newChromosome.Sigma.Add((u * p1chromosome.Sigma[i]) + ((1-u)*p2chromosome.Sigma[i]));
                newChromosome.Values.Add((u * p1chromosome.Values[i]) + ((1 - u) * p2chromosome.Values[i]));
            }
            for (int i = 0; i < p1chromosome.Alpha.Count; i++)
            {
                newChromosome.Alpha.Add((u * p1chromosome.Alpha[i]) + ((1 - u) * p2chromosome.Alpha[i]));
            }
            
            Population newPopulation = new Population();
            RealIndividual newIndividual = new RealIndividual(newChromosome);
            newPopulation.Add(newIndividual);
            return newPopulation;
        }
    }
}
