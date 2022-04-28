using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.evoperator.eda.model;

namespace Gevol.evolution.termination.eda.binary
{
    /// <summary>
    /// It compares ECGA Models. It will return false as long as new models will be better than the current one.
    /// If new model is better (lower CC), it will overwrite the current ona saved in this class.
    /// </summary>
    public class ECGAModelImprovement : TerminationCondition
    {
        ECGAModel _model;

        public override bool isSatisfied(Population population)
        {
            throw new NotImplementedException("Method is not implemented for non EDA algorithms.");
        }

        /// <summary>
        /// Check if model is better than currently saved. It will save better one.
        /// If new is not better, it will return true.
        /// </summary>
        /// <param name="population">Parameter is not used</param>
        /// <param name="model">ECGA Model to compare</param>
        /// <returns></returns>
        public override bool isSatisfied(Population population, object model)
        {
            if (!(model is ECGAModel) && model != null)
            {
                throw new ArgumentException("Model must be type of ECGAModel.", "model");
            }
            if(model == null)
            {
                return false;
            }
            if(_model == null)
            {
                _model = (ECGAModel) model;
                return false;
            }
            if(((ECGAModel)model).CC < _model.CC)
            {
                _model = (ECGAModel)model;
                return false;
            }
            return true;
        }

        public override void reset()
        {
            _model = null;
        }

        /// <summary>
        /// The best ECGA model.
        /// </summary>
        public ECGAModel Model
        {
            get { return _model; }
        }
    }
}
