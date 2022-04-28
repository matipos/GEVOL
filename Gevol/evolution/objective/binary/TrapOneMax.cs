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
    /// Trap One Max objective function for binary individuals.
    /// More 1 in the chromosome the better result. For example, for two genes with 1 it gets score -2, for three it will be -3.
    /// The trap is for the global minimum. The best value is for chromosome with only 0. The score is size of the chromosome minus one.
    /// E.g.:
    /// {0,0,0,0,0} = -6 - global minimum
    /// {0,1,0,0,0} = -1
    /// {0,1,0,0,1} = -2
    /// {1,1,1,1,1} = -5 
    /// </summary>
    public class TrapOneMax : ObjectiveFunction
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
            if(result == 0)
            {
                result = -1 - ((IList<int>)individual.Chromosome).Count;
            }
            return result;
        }
    }
}
