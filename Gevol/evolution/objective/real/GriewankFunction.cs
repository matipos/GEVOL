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
    /// Griewank function. 
    /// It has many regularly distrubuted widespread local minima. 
    /// 
    /// Global minimum = 0.0 for X = 0.0
    /// Formula: 
    /// f(X) = sum(x^2 / 4000) - product(cos(x / sqrt(i)) + 1
    /// where x - real values in chromosome
    /// i - number of gene
    /// 
    /// Usually executed for x between -600 and 600
    /// </summary>
    public class GriewankFunction : ObjectiveFunction
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
            double product = 1;
            double result;
            for (int i = 0; i < ((RealIndividualChromosome)individual.Chromosome).Values.Count; i++)
            {
                sum += Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[i], 2) / 4000;
                product *= Math.Cos(((RealIndividualChromosome)individual.Chromosome).Values[i] / Math.Sqrt(i+1));
            }
            result = sum - product + 1;
            return result;
        }
    }
}
