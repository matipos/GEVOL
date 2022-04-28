using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator
{
    /// <summary>
    /// Top class for replacement operator.
    /// The operator is run every time at the end of iteration in algorithm.
    /// Using old population and the new population (old population changed by other operators) 
    /// generate new population that will be used in the next iteration.
    /// </summary>
    public abstract class ReplacementOperator : Operator
    {
        /// <summary>
        /// Merge two populations to one.
        /// </summary>
        /// <param name="childrenPopulation">Parents population changed by operators.</param>
        /// <param name="parentsPopulation">Parents population.</param>
        /// <returns>New population.</returns>
        public abstract Population Apply(Population childrenPopulation, Population parentsPopulation);
    }
}
