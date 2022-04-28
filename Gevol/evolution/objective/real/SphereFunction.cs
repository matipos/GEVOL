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
    /// Sphere function. 
    /// 
    /// Global minimum = 0.0 for X = 0.0
    /// Formula: f(X) = sum of each x^2
    /// where x - real values in chromosome
    /// </summary>
    public class SphereFunction : ObjectiveFunction
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
            double result = 0;
            for (int i = 0; i < ((RealIndividualChromosome)individual.Chromosome).Values.Count; i++)
            {
                result += Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[i], 2);
            }
            return result;
        }
    }
}
