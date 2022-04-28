using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.termination
{
    /// <summary>
    /// Represent termination condition for evolutionary loop.
    /// </summary>
    public abstract class TerminationCondition
    {
        /// <summary>
        /// Return if termination condition is satisfied.
        /// </summary>
        /// <param name="population">Population.</param>
        /// <returns>True if termination condtition is satisfied - finish the loop.</returns>
        public abstract bool isSatisfied(Population population);

        /// <summary>
        /// Return if termination condition is satisfied. This method is used by EDA algorithms.
        /// </summary>
        /// <param name="population">Population.</param>
        /// <returns>True if termination condtition is satisfied - finish the loop.</returns>
        public abstract bool isSatisfied(Population population, object model);

        /// <summary>
        /// Reset state of the object to the beginning. 
        /// </summary>
        public abstract void reset();
    }
}
