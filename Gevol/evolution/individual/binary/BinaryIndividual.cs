using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.individual.binary
{
    /// <summary>
    /// Individual with binary chromosome.
    /// </summary>
    public class BinaryIndividual : Individual
    {
        /// <summary>
        /// Represents chromosome.
        /// </summary>
        private IList<int> _chromosome;
        
        /// <summary>
        /// Constructor for this individual.
        /// </summary>
        /// <param name="chromosome">Chromosome of new individual.</param>
        public BinaryIndividual(IList<int> chromosome)
        {
            this.Chromosome = chromosome;
        }

        /// <summary>
        /// Default constructor - not recommended.
        /// </summary>
        public BinaryIndividual() { }

        /// <summary>
        /// Generate new individual.
        /// Require to set ChromosomeLenght before run this method.
        /// </summary>
        /// <returns>New individual.</returns>
        public override Individual GenerateIndividual(object individualProperties)
        {
            if (!(individualProperties is BinaryIndividualProperties))
            {
                throw new ArgumentException("Parameter should be type of BinaryIndividualProperties.", "individualProperties");
            }
            IList<int> chromosome = new List<int>(((BinaryIndividualProperties) individualProperties).chromosomeLength);
            for (int i = 0; i < ((BinaryIndividualProperties)individualProperties).chromosomeLength; i++)
            {
                chromosome.Add(Randomizer.NextInt(0, 1));
            }
            BinaryIndividual newIndividual = new BinaryIndividual();
            newIndividual.Chromosome = chromosome;
            return newIndividual;
        }

        /// <summary>
        /// Chromosome is generic class: IList contains elements int.
        /// </summary>
        public override object Chromosome
        {
            get
            {
                return _chromosome;
            }
            set
            {
                _chromosome = (IList<int>)value;
            }
        }

        
        public override string ToString()
        {
            StringBuilder visualization = new StringBuilder("");
            for (int i = 0; i < _chromosome.Count; i++)
            {
                visualization.Append(_chromosome[i]);
            }
            return visualization.ToString(); ;
        }
    }

    public struct BinaryIndividualProperties
    {
        public int chromosomeLength { get; set; }
    }
}
