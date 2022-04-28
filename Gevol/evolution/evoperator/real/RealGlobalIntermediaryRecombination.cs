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
    /// Each chromosom is an average from all parents.
    /// </summary>
    public class RealGlobalIntermediaryRecombination : Operator
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
            Population newPopulation = new Population();
            RealIndividual newIndividual = new RealIndividual();
            RealIndividualChromosome newChromosome = new RealIndividualChromosome();
            newChromosome.Age = 0;
            //first individual as starting point
            newChromosome.Alpha = new List<double>(((RealIndividualChromosome)population[0].Chromosome).Alpha);
            newChromosome.Sigma = new List<double>(((RealIndividualChromosome)population[0].Chromosome).Sigma);
            newChromosome.Values = new List<double>(((RealIndividualChromosome)population[0].Chromosome).Values);
            //to get average first summarize all values
            for(int k = 1; k < population.Count; k++)
            { 
                for(int i = 0; i < svLength; i++)
                {
                    newChromosome.Sigma[i] += ((RealIndividualChromosome)population[k].Chromosome).Sigma[i];
                    newChromosome.Values[i] += ((RealIndividualChromosome)population[k].Chromosome).Values[i];
                }
                for (int i = 0; i < alphaLength; i++)
                {
                    newChromosome.Alpha[i] += ((RealIndividualChromosome)population[k].Chromosome).Alpha[i];
                }
            }
            //divide sum by number of individuals
            for (int i = 0; i < svLength; i++)
            {
                newChromosome.Sigma[i] = newChromosome.Sigma[i] / population.Count;
                newChromosome.Values[i] = newChromosome.Values[i] / population.Count;
            }
            for (int i = 0; i < alphaLength; i++)
            {
                newChromosome.Alpha[i] = newChromosome.Alpha[i] / population.Count;
            }
            newIndividual.Chromosome = newChromosome;
            newPopulation.Add(newIndividual);
            return newPopulation;
        }
    }
}
