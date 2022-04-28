using Gevol.evolution.evoperator.eda.binary;
using Gevol.evolution.evoperator.selection;
using Gevol.evolution.individual.binary;
using Gevol.evolution.termination.eda.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm.eda.binary
{
    public class ECGAAlgorithm : EdaAlgorithm<BinaryIndividual>
    {
        /// <summary>
        /// It implements Extended Compact Genetic Algorithm invented by Georges Harik in 1999.
        /// The algorithm:
        /// While MPM model is not convergenced
        ///   Generate random population based on the model
        ///   Block selection
        ///   Calculate new model
        ///   
        /// Model convergence is based on two parameters: blocksIterationLimit and acceptedProbabilityDifference.
        /// If genes has not changed in last blocksIterationLimit iterations and all of its previous probabilites are not changed for more than acceptedProbabilityDifference the algorithm is stopped.
        /// </summary>
        /// <param name="populationSize">Population size</param>
        /// <param name="selectionSize">Population size returned by tournament selection</param>
        /// <param name="chromosomeLength">Chromosome length</param>
        /// <param name="blocksIterationLimit">Convergence limit for blocks structure</param>
        /// <param name="acceptedProbabilityDifference">Convergence limit for probability</param>
        public ECGAAlgorithm(int populationSize, int selectionSize, int chromosomeLength, int blocksIterationLimit, double acceptedProbabilityDifference)
        {
            this.PopulationSize = populationSize;
            //RouletteSelection rs = new RouletteSelection(selectionSize);  //block selection gives better results
            BlockSelection rs = new BlockSelection(selectionSize);
            ECGAOperator ecga = new ECGAOperator(populationSize, chromosomeLength);
            Operators.Add(rs);
            Operators.Add(ecga);
            //this.Model = ecga.Model;
            this.TermCondition = new ECGAModelConvergence(blocksIterationLimit, acceptedProbabilityDifference); 
        }
    }
}
