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
    /// Each gen is taken from random parent.
    /// </summary>
    public class RealUniformCrossover : Operator
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
            int svLength = ((RealIndividualChromosome)population[0].Chromosome).Sigma.Count;
            int alphaLength = ((RealIndividualChromosome)population[0].Chromosome).Alpha.Count;
            RealIndividualChromosome newChromosome = new RealIndividualChromosome(svLength, alphaLength);
            newChromosome.Age = 0;
            for(int i = 0; i < svLength; i++)
            {
                newChromosome.Sigma.Add(((RealIndividualChromosome)population[Randomizer.NextInt(0, population.Count - 1)].Chromosome).Sigma[i]);
                newChromosome.Values.Add(((RealIndividualChromosome)population[Randomizer.NextInt(0, population.Count - 1)].Chromosome).Values[i]);
            }
            for(int i = 0; i < alphaLength; i++)
            {
                newChromosome.Alpha.Add(((RealIndividualChromosome)population[Randomizer.NextInt(0, population.Count - 1)].Chromosome).Alpha[i]);
            }
            Population newPop = new Population();
            RealIndividual newIndv = new RealIndividual(newChromosome);
            newPop.Add(newIndv);
            return newPop;
        }
    }
}
