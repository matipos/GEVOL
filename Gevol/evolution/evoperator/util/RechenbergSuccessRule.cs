using Gevol.evolution.individual;
using Gevol.evolution.objective;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.util
{
    /// <summary>
    /// Adaptation of 1/5 success rule of Rechenberg as operator chooser (1973).
    /// If for last 1/5 executions the operator generated better individual then his probability of being choosen is increased.
    /// If for last 1/5 executions the operator didn't generated better individual then his probability of being choosen is decreased.
    /// 
    /// The ratio (1/5) can be modified to any value.
    /// 
    /// Implementation based on:
    /// 1. "The theory of evolution strategies", Hans-Georg Beyer, Springer, 2001 
    /// 2. "Parallel problem solving from nature--PPSN IV: International Conference on Evolutionary Computation, the 4th International Conference on Parallel Problem Solving from Nature, Berlin, Germany, September 22-26, 1996 : proceedings", Hans-Michael Voigt, Werner Ebeling, Ingo Rechenberg, Springer, 1996
    /// </summary>
    public class RechenbergSuccessRule : Operator
    {
        private double _ratio = 0.2;
        private double _modifyPower = 0.85;
        private int _executionLimit = 5;
        private IList<Operator> _operators;
        private ObjectiveFunction _objFun;
        
        /// <summary>
        /// How many times successed for i-th element.
        /// </summary>
        private IList<int> _succeeded;
        /// <summary>
        /// How many times the i-th element was executed.
        /// </summary>
        private IList<int> _executions;
        private IList<double> _probabilities;
        
        /// <summary>
        /// Default constructor. 
        /// Default values will be set:
        /// Ratio = 0.2
        /// ModifyPower = 0.85
        /// </summary>
        public RechenbergSuccessRule(ObjectiveFunction objectiveFunction) : this(objectiveFunction, 0.2, 0.85, 5)
        {        }

        public RechenbergSuccessRule() : this(null, 0.2, 0.85, 5)
        {        }

        public RechenbergSuccessRule(ObjectiveFunction objectiveFunction, double ratio, double modifyPower, int executionLimit)
        {
            /*if (objectiveFunction == null)
            {
                throw new ArgumentNullException("objectiveFunction", "ObjectiveFunction cannot be null");
            }*/
            _operators = new List<Operator>();
            _succeeded = new List<int>();
            _executions = new List<int>();
            _probabilities = new List<double>();
            Ratio = ratio;
            ModifyPower = modifyPower;
            if(objectiveFunction != null)
                ObjectiveFunction = objectiveFunction;
            ExecutionLimit = executionLimit;
        }

        /// <summary>
        /// Prepare method to being used.
        /// Init may be used also as reset - all parameters will be set to the starting values.
        /// </summary>
        public void Init()
        {
            _succeeded.Clear();
            _executions.Clear();
            _probabilities.Clear();

            for(int i = 0; i < _operators.Count; i++)
            {
                _succeeded.Add(0);
                _executions.Add(0);
                _probabilities.Add(0.5);
            }
        }

        /// <summary>
        /// Rand the operator to work on population based on the current probabilities.
        /// It requires to calculate the objective function for each individual, so this may have bad impact on performance.
        /// In the end the scores are not saved in the new population.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Population Apply(Population population)
        {
            if (population.Count == 0 || _operators.Count <= 0)
            {
                return population;
            }
            //find the best score before applying any operator
            double theBestScore = GetTheBestScore(population);
            //rand operator to be executed
            double probabilityValue = Randomizer.NextDouble(0, _probabilities.Sum());
            //find the operator
            double probabilitiesSum = 0;
            int operatorIndex = -1;
            for(int i = 0; i < _operators.Count; i++)
            {
                probabilitiesSum += _probabilities[i];
                if(probabilityValue <= probabilitiesSum)
                {
                    operatorIndex = i;
                    break;
                }
            }
            //execute the selected operator
            Population newPopulation = _operators[operatorIndex].Apply(population);
            //get new the best score
            double theBestNewScore = GetTheBestScore(newPopulation);
            UpdateProbability(theBestScore, theBestNewScore, operatorIndex);
            return newPopulation;
        }

        private double GetTheBestScore(Population population)
        {
            double theBestScore = _objFun.Evaluate(population[0]);
            //don't change the score for the individual, evaluation is only for internal use
            //probably evaluation will be done later anyway
            //it is also possible that other operators will be run on the population later on
            //population[0].Score[0] = theBestScore;    
            double score = theBestScore + 1;
            for (int i = 1; i < population.Count; i++)
            {
                score = _objFun.Evaluate(population[i]);
                if(theBestScore > score)
                {
                    theBestScore = score;
                }
                //population[i].Score[0] = score;
            }
            return theBestScore;
        }

        /// <summary>
        /// Calculate the ratio of success:
        /// success ratio = succeeded / executions
        /// 
        /// if the success ratio > ratio then increase the probability
        /// if the success ratio &lt; ratio then decrease the probability
        /// </summary>
        /// <param name="theBestScore"></param>
        /// <param name="theBestNewScore"></param>
        /// <param name="operatorIndex"></param>
        private void UpdateProbability(double theBestScore, double theBestNewScore, int operatorIndex)
        {
            _executions[operatorIndex]++;
            if(theBestNewScore < theBestScore)
            {
                _succeeded[operatorIndex]++;
            }
            double ratio = _succeeded[operatorIndex] / _executions[operatorIndex];
            if (ratio == _ratio || _executions[operatorIndex] < _executionLimit)
                return;
            if (ratio < _ratio)
            {
                //decrease probability
                _probabilities[operatorIndex] *= _modifyPower;
            }
            else
            {
                //increase probability
                _probabilities[operatorIndex] /= _modifyPower;
            }
        }

        //---------- getters & setters

            /// <summary>
            /// Ratio successed to failed - default: 1/5.
            /// </summary>
        public double Ratio
        {
            get { return _ratio; }
            set
            {
                if (value <= 0 || value >= 1)
                {
                    throw new ArgumentOutOfRangeException("ModifyPower", value, "Modification power must be between 0 and 1.");
                }
                else { _ratio = value; }
            }
        }

        /// <summary>
        /// How strong the probability is changing - defalut: 0.85.
        /// </summary>
        public double ModifyPower
        {
            get { return _modifyPower; }
            set
            {
                if (value <= 0 || value >= 1)
                {
                    throw new ArgumentOutOfRangeException("ModifyPower", value, "Modification power must be between 0 and 1.");
                }
                else { _modifyPower = value; }
            }
        }

        /// <summary>
        /// Probabilities of being selected for each element.
        /// </summary>
        public IList<double> Probabilities { get { return _probabilities; } }

        /// <summary>
        /// Operators for random selection.
        /// </summary>
        public IList<Operator> Operators { get { return _operators; } set { _operators = value; } }

        /// <summary>
        /// New individual must be evaluated to check if the operator improved the score or not.
        /// </summary>
        public ObjectiveFunction ObjectiveFunction
        {
            get { return _objFun; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("ObjectiveFunction", "ObjectiveFunction cannot be null");
                }
                else { _objFun = value; }
            }
        }

        /// <summary>
        /// The rule will be applied after that number of executions for each operator.
        /// Default value is 5.
        /// </summary>
        public int ExecutionLimit
        {
            get { return _executionLimit; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("ExecutionLimit", value, "Execution limit for the rule should be bigger than 1.");
                }
                else { _executionLimit = value; }
            }
        }
    }
}
