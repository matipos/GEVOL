using Gevol.evolution.individual;
using Gevol.evolution.individual.real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.objective.real
{
    /// <summary>
    /// Powell function. 
    /// 
    /// Global minimum = 0.0 for X = 0.0
    /// Formula: 
    /// f(X) = sum for i = 0..d/4( (x4i-3 + 10*x4i-2)^2 + 5*(x4i-1 - x4i)^2 + (x4i-2 - 2*x4i-1)^4 + 10*(x4i-3 - x4i)^4 )
    /// where x - real values in chromosome
    /// i - number of gene
    /// d - chromosome's length
    /// 
    /// Usually executed for x between -4 and 5
    /// </summary>
    public class PowellFunction : ObjectiveFunction
    {
        /// <summary>
        /// Evaluate individual.
        /// </summary>
        /// <param name="individual">Individual to be evaluated.</param>
        /// <returns>Score of the individual.</returns>
        public override double Evaluate(Individual individual)
        {
            if (!(individual is RealIndividual))
            {
                throw new ArgumentException("Parameter must be type of RealIndividual.", "individual");
            }
            double sum = 0;
            for (int i = 1; i <= ((RealIndividualChromosome)individual.Chromosome).Values.Count / 4; i++)
            {
                sum += Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 3 - 1] + 10 * ((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 2 - 1], 2)
                    + 5 * Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 1 - 1] - ((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 1], 2)
                    + Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 2 - 1] - 2 * ((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 1 - 1], 4)
                    + 10 * Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 3 - 1] - ((RealIndividualChromosome)individual.Chromosome).Values[4 * i - 1], 4);
          ;
            }
            return sum;
        }
    }
}
