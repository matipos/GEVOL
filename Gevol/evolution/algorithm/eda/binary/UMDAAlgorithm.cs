using Gevol.evolution.evoperator.eda.binary;
using Gevol.evolution.evoperator.selection;
using Gevol.evolution.individual.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm.eda.binary
{
    /// <summary>
    /// Implementation of UMDAc algorithm.
    /// 
    /// While not TerminationCondition
    ///   BlockSelection
    ///   UMDAOperator
    /// </summary>
    public class UMDAAlgorithm : EdaAlgorithm<BinaryIndividual>
    {
        private int _blockSelectionSize = 0;
        private int _newPopulationSize = 0;

        /// <summary>
        /// Size of new population. 
        /// How big population should be generated.
        /// </summary>
        public int NewPopulationSize
        {
            get { return _newPopulationSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("NewPopulationSize", value, "Size must be greater than 0.");
                }
                else { _newPopulationSize = value; }
            }
        }

        /// <summary>
        /// Size of block selection. 
        /// How big population should be generated.
        /// </summary>
        public int BlockSelectionSize
        {
            get { return _blockSelectionSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("BlockSelectionSize", value, "Size must be greater than 0.");
                }
                else { _blockSelectionSize = value; }
            }
        }

        public UMDAAlgorithm(int blockSelectionSize, int newPopulationSize)
        {
            BlockSelectionSize = blockSelectionSize;
            NewPopulationSize = newPopulationSize;
            Operators.Add(new BlockSelection(BlockSelectionSize));
            Operators.Add(new UMDAOperator(NewPopulationSize));
        }
    }
}
