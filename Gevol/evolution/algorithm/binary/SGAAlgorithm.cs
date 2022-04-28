using Gevol.evolution.evoperator.binary;
using Gevol.evolution.evoperator.replacement;
using Gevol.evolution.evoperator.selection;
using Gevol.evolution.individual.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm.binary
{
    /// <summary>
    /// Implementation of SGA algorithm.
    /// 
    /// While not TerminationCondition
    ///   BlockSelection
    ///   UniformCrossover
    ///   NegationMutation
    ///   BestFromUnionReplacement
    /// </summary>
    public class SGAAlgorithm : Algorithm<BinaryIndividual>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="blockSelectionSize">Size for block selection operator.</param>
        public SGAAlgorithm(int blockSelectionSize)
            : base()
        {
            this.Operators.Add(new BlockSelection(blockSelectionSize));
            this.Operators.Add(new UniformCrossover());
            this.Operators.Add(new NegationMutation());
            this.ReplaceOperator = new BestFromUnionReplacement();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="blockSelectionSize">Size for block selection operator.</param>
        /// <param name="mutationProbability">Probability to appear mutation on the gene.</param>
        public SGAAlgorithm(int blockSelectionSize, double mutationProbability)
            : base()
        {
            this.Operators.Add(new BlockSelection(blockSelectionSize));
            this.Operators.Add(new UniformCrossover());
            this.Operators.Add(new NegationMutation(mutationProbability));
            this.ReplaceOperator = new BestFromUnionReplacement();
        }
    }
}
