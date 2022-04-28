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
    /// Schwefel function. 
    /// It has many local minimuma. 
    /// 
    /// Global minimum = 0.0 for X = 420.968746
    /// Formula: 
    /// f(X) = 418.982887 * d - sum(x * sin(sqrt(abs(x))))
    /// where x - real values in chromosome
    /// d - chromosome's length
    /// 
    /// Usually executed for x between -500 and 500
    /// </summary>
    public class SchwefelFunction : ObjectiveFunction
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
            double length = ((RealIndividualChromosome)individual.Chromosome).Values.Count;
            double result;
            for (int i = 0; i < length; i++)
            {
                sum += ((RealIndividualChromosome)individual.Chromosome).Values[i]
                    * Math.Sin(Math.Sqrt(Math.Abs(((RealIndividualChromosome)individual.Chromosome).Values[i])));
            }
            result = 418.982887 * length - sum;
            return result;
        }
    }
}
