using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.individual.real
{
    /// <summary>
    /// Individual prepared for evolutionary strategies.
    /// It has three lists in its chromosome.
    /// Values - list of real numbers
    /// Alpha - list of additional parameter used by ES algorithm
    /// Sigma - list of additional parameter used by ES algorithm
    /// </summary>
    public class RealIndividual : Individual
    {
        private RealIndividualChromosome _chromosome;

        public override object Chromosome
        {
            get
            {
                return _chromosome;
            }

            set
            {
                _chromosome = (RealIndividualChromosome)value;
            }
        }

        public RealIndividual(RealIndividualChromosome chromosome)
        {
            this.Chromosome = new RealIndividualChromosome();
            _chromosome.Age = chromosome.Age;
            _chromosome.Alpha = new List<double>(chromosome.Alpha);
            _chromosome.Sigma = new List<double>(chromosome.Sigma);
            _chromosome.Values = new List<double>(chromosome.Values);
        }

        public RealIndividual() {  }

        /// <summary>
        /// Generate new individual.
        /// Values are random accordign to formulas:
        /// Values - ranodm double number between 0 and 1
        /// Sigma - random gaussian numbers with mean 0 and standard deviation 1
        /// Alpha - 2 * pi * random double between 0 and 1
        /// </summary>
        /// <param name="individualProperties"></param>
        /// <returns></returns>
        public override Individual GenerateIndividual(object individualProperties)
        {
            if (!(individualProperties is RealIndividualProperties))
            {
                throw new ArgumentException("Parameter should be type of RealIndividualProperties.", "individualProperties");
            }
            RealIndividualProperties inProp = (RealIndividualProperties)individualProperties;
            RealIndividualChromosome chromosome = new RealIndividualChromosome(inProp.ChromosomeLength, inProp.AlphaLength);
            //prawdopodobnie sie sypnie bo elementy w strukturze nie sa zainicjowane
            for(int i = 0; i < inProp.AlphaLength; i++)
            {
                chromosome.Alpha.Add(2 * Math.PI * Randomizer.NextDouble(0, 1));
            }
            for(int i = 0; i < inProp.ChromosomeLength; i++)
            {
                chromosome.Sigma.Add(Randomizer.RandomGaussian(0,1));
                chromosome.Values.Add(Randomizer.NextDouble(0, 1));
            }

            RealIndividual newIndividual = new RealIndividual(chromosome);
            return newIndividual;
        }

        public override string ToString()
        {
            StringBuilder visualization = new StringBuilder("Values: ");
            for (int i = 0; i < _chromosome.Values.Count; i++)
            {
                visualization.Append(_chromosome.Values[i] + " ");
            }
            visualization.AppendLine("Alpha: ");
            for (int i = 0; i < _chromosome.Alpha.Count; i++)
            {
                visualization.Append(_chromosome.Alpha[i] + " ");
            }
            visualization.AppendLine("Sigma: ");
            for (int i = 0; i < _chromosome.Sigma.Count; i++)
            {
                visualization.Append(_chromosome.Sigma[i] + " ");
            }
            return visualization.ToString(); ;
        }
    }

    public struct RealIndividualProperties
    {
        private int _chromosomeLenght;
        private int _alphaLength;

        /// <summary>
        /// When assigning value for that 
        /// also alpha length is set.
        /// AlphaLength = (value * (value - 1)) / 2
        /// </summary>
        public int ChromosomeLength
        {
            get
            {
                return _chromosomeLenght;
            }

            set
            {
                _chromosomeLenght = value;
                _alphaLength = (value * (value - 1)) / 2;  //or +1 instead of -1
            }
        }

        public int AlphaLength
        {
            get
            {
                return _alphaLength;
            }

            set
            {
                _alphaLength = value;
            }
        }
    }

    public struct RealIndividualChromosome
    {
        public IList<double> Values;
        public IList<double> Alpha;
        public IList<double> Sigma;
        public int Age;

        public RealIndividualChromosome(int length, int alphaLength)
        {
            Values = new List<double>(length);
            Alpha = new List<double>(alphaLength);
            Sigma = new List<double>(length);
            Age = 0;
        }
    }
}
