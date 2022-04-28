using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.evoperator.eda.model;
using System.Collections;

namespace Gevol.evolution.termination.eda.binary
{
    /// <summary>
    /// It returns true if ECGA model has convergenced.
    /// The model must be convergenced based on its blocks and probabilities.
    /// 
    /// If for blocksIterationLimit the block structure is not changed, it will satisfy the condition.
    /// If all of the probabilities are not changed for more than acceptedProbabilityDifference, it will satisfy the condition. 
    /// Probability is compared only with the previous model.
    /// If both conditions are true, it will return true.
    /// </summary>
    public class ECGAModelConvergence : TerminationCondition
    {
        private int _blocksIterationLimit = 0;
        private double _acceptedProbabilityDifference = 0.0;
        private int _actualIterationsWithTheSameBlocks = 0;
        ECGAModel _model;

        /// <summary>
        /// In how many iterations the block structure should remain the same.
        /// </summary>
        public int BlocksIterationLimit {
            get { return _blocksIterationLimit; }
            set {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("BlocksIterationLimit", value, "Blocks iteration limit must be greater than 0.");
                }
                _blocksIterationLimit = value;
            }
        }

        /// <summary>
        /// Maximum probability difference that is accepted in compare to previous model.
        /// </summary>
        public double AcceptedProbabilityDifference {
            get { return _acceptedProbabilityDifference; }
            set {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("AcceptedProbabilityDifference", value, "Accepted probability difference can have value between 0.0 and 1.0 only.");
                }
                _acceptedProbabilityDifference = value;
            }
        }

        /// <summary>
        /// It is satisfied when block structure is not changed for blocksIterationLimit iterations and its probabilities are not changed more than acceptedProbabilityDifference.
        /// </summary>
        /// <param name="blocksIterationLimit"></param>
        /// <param name="acceptedProbabilityDifference"></param>
        public ECGAModelConvergence(int blocksIterationLimit, double acceptedProbabilityDifference)
        {
            _blocksIterationLimit = blocksIterationLimit;
            _acceptedProbabilityDifference = acceptedProbabilityDifference;
        }

        public override bool isSatisfied(Population population)
        {
            throw new NotImplementedException("Method is not implemented for non EDA algorithms.");
        }

        public override bool isSatisfied(Population population, object model)
        {
            if (!(model is ECGAModel) && model != null)
            {
                throw new ArgumentException("Model must be type of ECGAModel.", "model");
            }
            if (model == null)
            {
                return false;
            }
            if (_model == null)
            {
                _model = (ECGAModel)model;
                return false;
            }
            if (!checkBlocksStructure((ECGAModel)model) || !checkProbabilities((ECGAModel)model))
            {
                _model = (ECGAModel)model;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compare the block structure with the previuos model. 
        /// It returns true if it's the same for _blocksIterationLimit times.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected bool checkBlocksStructure(ECGAModel model)
        {
            if(_model.Blocks.Count != model.Blocks.Count)   //probably most of the time the number of blocks will be different, so it's good to check count first
            {
                _actualIterationsWithTheSameBlocks = 0;
                return false;
            }
            for(int i = 0; i < _model.Blocks.Count; i++)
            {
                if(((List<int>)_model.Blocks[i].Genes).Except<int>(model.Blocks[i].Genes).Any<int>() && ((List<int>)model.Blocks[i].Genes).Except<int>(_model.Blocks[i].Genes).Any<int>())
                {
                    //differences found in some block
                    _actualIterationsWithTheSameBlocks = 0;
                    return false;
                }
            }
            if (++_actualIterationsWithTheSameBlocks < _blocksIterationLimit)
                return false;
            return true;
        }

        /// <summary>
        /// Compare all probabilities with the probabilities from previous model. 
        /// If difference between all of them is smaller than _acceptedProbabilityDifference it will return true.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected bool checkProbabilities(ECGAModel model)
        {
            if (_model.Blocks.Count != model.Blocks.Count)   //probably most of the time the number of blocks will be different, so it's good to check count first
            {
                return false;
            }
            for (int i = 0; i < _model.Blocks.Count; i++)
            {
                if (_model.Blocks[i].Probabilities.Count != model.Blocks[i].Probabilities.Count)
                {
                    return false;
                }
                for (int p = 0; p < _model.Blocks[i].Probabilities.Count; p++)
                {
                    if (Math.Abs(_model.Blocks[i].Probabilities[p] - model.Blocks[i].Probabilities[p]) > _acceptedProbabilityDifference)
                    {
                        return false;
                    }
                }
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
