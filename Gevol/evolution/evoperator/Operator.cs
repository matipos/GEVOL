using Gevol.evolution.individual;
using Gevol.evolution.objective;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator
{
    /// <summary>
    /// Top clas for every evolutionary operator used in evolution.
    /// </summary>
	public abstract class Operator
    {
        /// <summary>
        /// Operator can use objective functions e.g. tournament selection.
        /// @deprecated if evaluation is required, the operator can do it itself
        /// </summary>
        //public virtual ObjectiveFunctions ObjFunctions { get; set; }

        /// <summary>
        /// Run the operator.
        /// </summary>
        /// <param name="population">Parents population.</param>
        /// <returns>Child population.</returns>
        public abstract Population Apply(Population population);
    }
}
