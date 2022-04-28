using Gevol.evolution.evoperator.eda.binary;
using Gevol.evolution.individual.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm.eda.binary
{
    public class PBILAlgorithm : EdaAlgorithm<BinaryIndividual>
    {
        /// <summary>
        /// Implementation of PBIC algorithm
        /// 
        /// While not TerminationCondition
        ///   PBILOperator
        /// </summary>
        /// <param name="newPopulationSize">Population size to be generated</param>
        /// <param name="chromosomeLength">Chromosome length</param>
        /// <param name="learningRate">Learning rate</param>
        /// <param name="mutationSize">Mutation size</param>
        /// <param name="mutationProbability">Probability that mutation occurs</param>
        public PBILAlgorithm(int newPopulationSize, int chromosomeLength, double learningRate, double mutationSize, double mutationProbability)
        {
            this.PopulationSize = newPopulationSize;
            Operators.Add(new PBILOperator(newPopulationSize, chromosomeLength, learningRate, mutationSize, mutationProbability));
        }
        
    }
}
