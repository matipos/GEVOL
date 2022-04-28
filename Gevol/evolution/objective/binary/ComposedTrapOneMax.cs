using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.objective.binary
{
    /// <summary>
    /// The chromosome is built from k blocks of the same size.
    /// Each block is calculated separately by the trap function.
    /// Final result is sum of the results from all blocks.
    /// The trap for each block is calculated according the formula:
    /// If all genes have value 1, then return fhigh.
    /// Otherwise flow - u * (flow / (k-1))
    /// u - number of ones in the block
    /// fmax and flow are parameters. Thanks to them it is possible to steer the difference between local minimum (flow) and global minimum (fhigh).
    /// </summary>
    public class ComposedTrapOneMax : ObjectiveFunction
    {
        private int _numberOfBlocks;
        private int _blockLength;
        private double _fhigh;
        private double _flow;

        public ComposedTrapOneMax(int numberOfBlocks, int blockLength, double fhigh, double flow)
        {
            NumberOfBlocks = numberOfBlocks;
            BlockLength = blockLength;
            Fhigh = fhigh;
            Flow = flow;
        }

        /// <summary>
        /// Evaluate individual.
        /// </summary>
        /// <param name="individual">Individual to be evaluated.</param>
        /// <returns>Score of the individual.</returns>
        public override double Evaluate(Individual individual)
        {
            if (!(individual is BinaryIndividual))
            {
                throw new ArgumentException("Parameter must be type of BinaryIndividual.", "individual");
            }
            List<int> chromosome = (List<int>)((BinaryIndividual)individual).Chromosome;
            if(_numberOfBlocks * _blockLength != chromosome.Count)
            {
                throw new ArgumentException("Chromosome length is not equal to given number of blocks multiplied by length of the block.", "individual.Chromosome");
            }

            double result = 0;
            //calculate trap function for each block
            for(int block = 0; block < _numberOfBlocks; block++)
            {
                result += evaluateBlock(chromosome.GetRange(block * _blockLength, _blockLength));
            }
            return result;
        }

        private double evaluateBlock(IList<int> block)
        {
            //calculate number of ones
            double ones = 0;
            for (int i = 0; i < block.Count; i++)
            {
                ones += (double)block[i];
            }
            //execute the formula
            if ((int)ones == _blockLength)
            {
                return 0.0 - _fhigh;
            }
            else
            {
                return 0.0 - (_flow - (ones * (_flow / (double)(_blockLength - 1))));
            }
        }


        /// <summary>
        /// Number of blocks in the chromosome. 
        /// </summary>
        public int NumberOfBlocks
        {
            get { return _numberOfBlocks; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("NumberOfBlocks", value, "Value must be bigger than 0.");
                }
                else { _numberOfBlocks = value; }
            }
        }

        /// <summary>
        /// Length of each block. 
        /// </summary>
        public int BlockLength
        {
            get { return _blockLength; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("BlockLength", value, "Value must be bigger than 0.");
                }
                else { _blockLength = value; }
            }
        }

        /// <summary>
        /// Value for the global minimum. 
        /// </summary>
        public double Fhigh
        {
            get { return _fhigh; }
            set { _fhigh = value; }
        }

        /// <summary>
        /// Value for the local minimum. 
        /// </summary>
        public double Flow
        {
            get { return _flow; }
            set { _flow = value; }
        }
    }
}
