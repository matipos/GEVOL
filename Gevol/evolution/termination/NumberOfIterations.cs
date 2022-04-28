using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.termination
{
    /// <summary>
    /// Termination condition is a number of iterations.
    /// The condition is satisfied when number of iteration is equal to Iterations.
    /// </summary>
    public class NumberOfIterations : TerminationCondition
    {
        private int _iterations = 0;        //number of iterations to be satisfied.
        private int finishedIterations = 0; //how many iterations have been done.

        /// <summary>
        /// How many times make evolution in evolutionary algorithm.
        /// </summary>
        public int Iterations { get { return _iterations; } set { _iterations = value; } }

        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="iterations">Number of iterations required to be done that termination condition could be satisfied.</param>
        public NumberOfIterations(int iterations)
        {
            this.Iterations = iterations;
        }

        /// <summary>
        /// It is satisified when number of iterations is equal to Iterations.
        /// </summary>
        /// <param name="population">Population, not required.</param>
        /// <returns>Satisfaction.</returns>
        public override bool isSatisfied(Population population)
        {
            if (finishedIterations >= _iterations)
            {
                return true;
            }
            else
            {
                finishedIterations++;
                return false;
            }
        }

        /// <summary>
        /// It is satisified when number of iterations is equal to Iterations.
        /// Version for EDA algorithms.
        /// </summary>
        /// <param name="population">Population, not required.</param>
        /// <param name="Model">It is not used.</param>
        /// <returns>Satisfaction.</returns>
        public override bool isSatisfied(Population population, object Model)
        {
            if (finishedIterations >= _iterations)
            {
                return true;
            }
            else
            {
                finishedIterations++;
                return false;
            }
        }

        /// <summary>
        /// Reset executed number of iterations to zero.
        /// </summary>
        public override void reset()
        {
            finishedIterations = 0;
        }
    }
}
