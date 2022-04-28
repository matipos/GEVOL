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
    /// Rastrigin function. 
    /// It has many regularly distrubuted local minimuma. 
    /// 
    /// Global minimum = 0.0 for X = 0.0
    /// Formula: 
    /// f(X) = 10 * d + sum(x^2 - 10*cos(2*pi*x))
    /// where x - real values in chromosome
    /// d - chromosome's length
    /// 
    /// Usually executed for x between -5.12 and 5.12
    /// </summary>
    public class RastriginFunction : ObjectiveFunction
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
                sum += Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[i], 2) 
                    - 10 * Math.Cos(2 * Math.PI * ((RealIndividualChromosome)individual.Chromosome).Values[i]);
            }
            result = sum + 10 * length;
            return result;
        }
    }
}
