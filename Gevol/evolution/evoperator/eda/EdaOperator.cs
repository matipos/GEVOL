using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.eda
{
    /// <summary>
    /// Operator for EDA algorithms.
    /// In the Apply method:
    /// 1. It must calculate model
    /// 2. Generate new population
    /// 3. Save the model.
    /// </summary>
    public abstract class EdaOperator : Operator
    {
        public object Model;

        /// <summary>
        /// Calculate model to generate individuals.
        /// It should be called by Apply method.
        /// </summary>
        /// <param name="population"></param>
        protected abstract void CalculateModel(Population population);
    }
}
