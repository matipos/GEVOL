using Gevol.evolution.evoperator.eda.binary;
using Gevol.evolution.individual.binary;
using Gevol.evolution.termination.eda.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm.eda.binary
{
    public class CGAAlgorithm : EdaAlgorithm<BinaryIndividual>
    {
        /// <summary>
        /// Implementation of CGA algorithm.
        /// The algorithm is terminated when all probabilities are satisfied. 
        /// minimum convergence value means an acceptable probability to get 0. You can understand it as probability to get 0 = 1 - minConvergenceValue. 1 means 100%.
        /// maximum convergence value means an acceptable probability to get 1.
        /// Be careful, minimum convergence value will never get 0 and maximum value will never get 1.
        /// The reason is that the probability is changed when genes differ in both individuals. 
        /// If the gene has 0.9999999 probability to get 1 it's practically impossible to get 0 in the worse individual to update the probability to 1.
        /// 
        /// While not ConvergenceInProbability
        ///   CGAOperator
        /// </summary>
        /// <param name="simulatedPopulationSize">Simulated population size (speed of learning)</param>
        /// <param name="chromosomeLength">Chromosome length</param>
        /// <param name="mutationProbability">Probability that mutation occurs</param>
        /// <param name="minConvergenceValue">Acceptable probability to get 0 (lower value, more probable to get 0)</param>
        /// <param name="maxConvergenceValue">Acceptable probability to get 1 (higher value, more probable to get 1)</param>
        public CGAAlgorithm(int simulatedPopulationSize, int chromosomeLength, double minConvergenceValue, double maxConvergenceValue)
        {
            this.PopulationSize = 2;    //in this algorithm population must be 2 individuals
            CGAOperator cga = new CGAOperator(simulatedPopulationSize, chromosomeLength);
            Operators.Add(cga);
            this.Model = cga.Model;
            this.TermCondition = new ConvergenceInProbability(minConvergenceValue, maxConvergenceValue);
        }
    }
}
