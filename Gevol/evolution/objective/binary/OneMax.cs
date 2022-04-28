using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.objective.binary
{
    /// <summary>
    /// One Max objective function for binary individuals.
    /// Chromosome with genes with value only 1 represents global minimum.
    /// E.g.:
    /// {0,0,0,0,0} = 0
    /// {0,1,0,0,1} = -2
    /// {1,1,1,1,1} = -5 - global minimum
    /// </summary>
    public class OneMax : ObjectiveFunction
    {
        /// <summary>
        /// Evaluate individual.
        /// </summary>
        /// <param name="individual">Individual to be evaluated.</param>
        /// <returns>Score of the individual.</returns>
        public override double Evaluate(Individual individual)
        {
            if (!(individual is BinaryIndividual))
            {
                throw new ArgumentException("Parameter must be type of BinaryIndividual.", "individual");
            }
            double result = 0;
            for (int i = 0; i < ((IList<int>)individual.Chromosome).Count; i++)
            {
                result -= ((IList<int>)individual.Chromosome)[i];
            }
            return result;
        }
    }
}
