using Gevol.evolution.evoperator.eda.model;
using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.termination.eda.binary
{
    /// <summary>
    /// This is specialized termination condition for EDA algorithms with a model of probability vector.
    /// It is satisfied when all probabilities are convergenced. So, all probabilities have at least max value or at most min value.
    /// For example, 1 or 0.
    /// </summary>
    public class ConvergenceInProbability : TerminationCondition
    {
        private double _minValue = 0.0;
        private double _maxValue = 1.0;

        /// <summary>
        /// Below that value when the probability is satisfied.
        /// </summary>
        public double MinValue { get { return _minValue; } set { _minValue = value; } }

        /// <summary>
        /// Above that value when the probability is satisfied.
        /// </summary>
        public double MaxValue { get { return _maxValue; } set { _maxValue = value; } }

        /// <summary>
        /// It is satisfied when all probabilities have value above maxValue or below minValue.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public ConvergenceInProbability(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// This method is not implemented as it works only for EDA algorithms..
        /// </summary>
        /// <param name="population">Not required.</param>
        /// <returns>Satisfaction.</returns>
        public override bool isSatisfied(Population population)
        {
            throw new NotImplementedException("Method is not implemented for non EDA algorithms.");
        }

        /// <summary>
        /// EDA model must have probability vector. The vector is tested if all values have 1 or 0.
        /// It works only for:
        /// UMDAModel
        /// PBILModel
        /// </summary>
        /// <param name="population">Population, not required.</param>
        /// <param name="Model">Model with probability vector.</param>
        /// <returns>Satisfaction.</returns>
        public override bool isSatisfied(Population population, object model)
        {
            if (!(model is UMDAModel || model is PBILModel || model is CGAModel))
            {
                throw new ArgumentException("Model must be type of UMDAModel or PBILModel or CGAModel.", "model");
            }
            IList<double> probabilities = getProbabilities(model);
            foreach (double probability in probabilities)
            {
                if (probability < _maxValue && probability > _minValue)
                    return false;
            }
            return true;
        }
        
        /// <summary>
        /// In this algorithm is nothing to reset. This method does nothing.
        /// </summary>
        public override void reset()
        {
            
        }

        private IList<double> getProbabilities(object model)
        {
            if (model is UMDAModel)
            {
                return ((UMDAModel)model).probabilities;
            }
            if (model is PBILModel)
            {
                return ((PBILModel)model).probabilities;
            }
            if (model is CGAModel)
            {
                return ((CGAModel)model).probabilities;
            }
            return null;
        }
    }
}
