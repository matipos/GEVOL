using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.eda.model
{
    /// <summary>
    /// Marginal Probability Model implemented for ECGAAlgorithm.
    /// It has list of blocks. In every block are two lists:
    /// - list of genes belonging to that block
    /// - list of probabilities for every possibly set of genes
    /// 
    /// For example
    /// Blocks[5].Genes = {2,3,11,21} means that these genes belong to group 5
    /// This group has 2^4 probabilities, e.g.:
    /// Blocks[5].Probabilities[3] = 0.02 means that gene 2=0, gene 3=0, gene 11=1, gene 21=1 with probability of 2%, index 3 -> 0011
    /// Blocks[5].Probabilities[14] = 0.12 means that gene 2=1, gene 3=1, gene 11=1, gene 21=0 with probability of 12%, index 14 -> 1110
    /// </summary>
    public class ECGAModel
    {
        private IList<ECGAStructure> _blocks;
        private double _cc = 0.0;  //score for the model
        private double _mc = 0.0;   //Model Complexity
        private double _cpc = 0.0;  //Compressed Populatin Complexity
        private int _populationSize = 0;
        

        public ECGAModel()
        {
            _blocks = new List<ECGAStructure>();
        }

        /// <summary>
        /// Clone this object.
        /// </summary>
        /// <returns></returns>
        public ECGAModel Clone()
        {
            ECGAModel clone = new ECGAModel();
            for(int i = 0; i < _blocks.Count; i++)
            {
                clone.Blocks.Add(new ECGAStructure() { Genes = new List<int>(_blocks[i].Genes), Probabilities = new List<double>(_blocks[i].Probabilities) });
            }
            clone.PopulationSize = _populationSize;
            clone._cc = _cc;
            clone._cpc = _cpc;
            clone._mc = _mc;
            return clone;
        }

        /// <summary>
        /// Convert binary value to int.
        /// For example 0110 converts to 6.
        /// </summary>
        /// <param name="genesValues"></param>
        /// <returns></returns>
        public int GenesSetToIndex(IList<int> genesValues)
        {
            return Convert.ToInt16(String.Join("", genesValues), 2);
        }

        /// <summary>
        /// Convert integer value into list of bits.
        /// For example 12 will be converted to IList<int>() {1100}
        /// </summary>
        /// <param name="index">Number to be converted</param>
        /// <param name="length">Length of the returned list. First bits will be filled with 0 if necessary</param>
        /// <returns></returns>
        public IList<int> IndexToGenesSet(int index, int length)
        {
            IList<int> genesSet = new List<int>();
            string bits = Convert.ToString(index, 2);
            while(bits.Count() + genesSet.Count < length)
            {
                genesSet.Add(0);
            }
            for(int i = 0; i < bits.Count(); i++)
            {
                genesSet.Add(Convert.ToInt32(bits.Substring(i, 1)));
            }
            return genesSet;
        }
        
        /// <summary>
        /// log2(PopulationSize) * sum(2^Blocks.Genes.Count)
        /// </summary>
        /// <returns></returns>
        protected double CalculateModelComplexity()
        {
            double blockPower = 0;
            foreach(ECGAStructure block in _blocks)
            {
                blockPower += Math.Pow(2.0, block.Genes.Count);
            }
            return _mc = Math.Log(_populationSize, 2) * blockPower;
        }

        /// <summary>
        /// PopulationSize * sum(abs(Blocks.Probabilities * log2(Blocks.Probabilities))
        /// </summary>
        /// <returns></returns>
        protected double CalculateCompressedPopulationComplexity()
        {
            double entropy = 0;
            foreach(ECGAStructure block in _blocks)
            {
                foreach(double probability in block.Probabilities)
                {
                    if(probability != 0) { 
                        entropy += Math.Abs(probability * Math.Log(probability, 2.0));
                    }
                }
            }
            return _cpc = _populationSize * entropy;
        }

        /// <summary>
        /// CalculateModelComplexity + CalculateCompressedPopulationComplexity
        /// </summary>
        /// <returns></returns>
        public double CalculateCC()
        {
            if (_populationSize <= 0)
            {
                throw new Exception("Population size has not been set or the value is incorrect. PopulationSize = " + _populationSize);
            }
            return _cc = CalculateModelComplexity() + CalculateCompressedPopulationComplexity();
        }

        public override string ToString()
        {
            String result = "CC=MC+CPC;" + _cc + "=" + _mc + "+" + _cpc + ";";
            for (int i = 0; i < _blocks.Count; i++)
            {
                result += "[";
                for(int k = 0; k < _blocks[i].Genes.Count - 1; k++)
                {
                    result += _blocks[i].Genes[k] + "+";
                }
                result += _blocks[i].Genes.Last();
                result += "]{";
                for(int k = 0; k < _blocks[i].Probabilities.Count - 1; k++)
                {
                    result += ListToString(IndexToGenesSet(k, _blocks[i].Genes.Count)) + "=" + _blocks[i].Probabilities[k] + ";";
                }
                result += ListToString(IndexToGenesSet(_blocks[i].Probabilities.Count - 1, _blocks[i].Genes.Count)) + "=" + _blocks[i].Probabilities.Last() + "}";
            }
            return result;
        }

        private string ListToString(IList<int> list)
        {
            string result = "";
            for (int i = 0; i < list.Count; i++)
                result += list[i];
            return result;
        }

        public double CC
        {
            get
            {
                if(_cc != 0)
                {
                    return _cc;
                }
                return CalculateCC();
            }
        }
        

        /// <summary>
        /// List of blocks. 
        /// </summary>
        public IList<ECGAStructure> Blocks
        {
            get { return _blocks; }
            set { _blocks = value; _cc = 0; _mc = 0; _cpc = 0; }
        }

        /// <summary>
        /// Size of new population. 
        /// How big population should be generated.
        /// </summary>
        public int PopulationSize
        {
            get { return _populationSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("PopulationSize", value, "Population size must be greater than 0.");
                }
                else { _populationSize = value; _cc = 0; _mc = 0; _cpc = 0; }
            }
        }
    }

    public class ECGAStructure
    {
        public IList<int> Genes;
        public IList<double> Probabilities;

        public ECGAStructure()
        {
            Genes = new List<int>();
            Probabilities = new List<double>();
        }
    }
}
