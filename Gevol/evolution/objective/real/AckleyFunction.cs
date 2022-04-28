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
    /// Ackley function. 
    /// It has many small minimums for each dimension and one big global.
    /// 
    /// Global minimum = 0.0 for X = 0.0
    /// Formula;
    /// f(X) = -a * exp( -b * sqrt(sum(x^2) / d) - exp(sum(cos(c * x)) / d) + a + exp(1)
    /// where
    /// x - chromosome with values
    /// d - length of the chromosome X
    /// a, b, c - parameters
    /// 
    /// Recommended and default values for parameters are:
    /// a = 20
    /// b = 0.2
    /// c = 2pi
    /// </summary>
    public class AckleyFunction : ObjectiveFunction
    {
        public double A;
        public double B;
        public double C;

        /// <summary>
        /// Default constructor. It sets default values for parameters
        /// </summary>
        public AckleyFunction()
        {
            A = 20;
            B = 0.2;
            C = 2 * Math.PI;
        }

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
            double result;
            double sumPow = 0;
            double sumCos = 0;
            int d = ((RealIndividualChromosome)individual.Chromosome).Values.Count;
            for (int i = 0; i < d; i++)
            {
                sumPow += Math.Pow(((RealIndividualChromosome)individual.Chromosome).Values[i], 2);
                sumCos += Math.Cos(C * ((RealIndividualChromosome)individual.Chromosome).Values[i]);
            }
            result = ((-1) * A * Math.Exp((-1) * B * Math.Sqrt(sumPow / d)))
                - Math.Exp(sumCos / d) + A + Math.Exp(1);
            return result;
        }
    }
}
