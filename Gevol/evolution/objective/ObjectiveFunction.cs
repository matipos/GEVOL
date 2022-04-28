using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.objective
{
    /// <summary>
    /// Top class for objective function in evolutionary algorithm.
    /// </summary>
    public abstract class ObjectiveFunction
    {
        /// <summary>
        /// Compute score for individual.
        /// </summary>
        /// <param name="individual">Individual to be scored.</param>
        /// <returns>Score.</returns>
        public abstract double Evaluate(Individual individual);
    }
}
