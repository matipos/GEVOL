using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.binary
{
    /// <summary>
    /// The operator combine two parent to generate two childreen.
    /// Exapmle:
    /// Parent 1: {1,1,1,1,1,1}
    /// Parent 2: {0,0,0,0,0,0}
    /// Result:
    /// Child 1: {1,0,1,0,1,1}
    /// Child 2: {0,1,0,1,0,0}
    /// 
    /// Points to crossover are random.
    /// </summary>
    public class UniformCrossover : Operator
    {
        private int _newPopulationSize = -1;

        /// <summary>
        /// New population size - how many childreen to create.
        /// If not set then childreen population is as big as parents population.
        /// </summary>
        public int NewPopulationSize
        {
            get { return _newPopulationSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("NewPopulationSize", value, "Size for the new population must be greater than 0.");
                }
                else { _newPopulationSize = value; }
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UniformCrossover() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="newPopulationSize">New population size.</param>
        public UniformCrossover(int newPopulationSize)
        {
            this.NewPopulationSize = newPopulationSize;
        }

        /// <summary>
        /// Crossover. Generate childreen population.
        /// </summary>
        /// <param name="population">Parents population.</param>
        /// <returns>Childreen population.</returns>
        public override Population Apply(Population population)
        {
            if (population.Count == 0)
            {
                return population;
            }
            if (!(population[0] is BinaryIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of BinaryIndividual.", "population");
            }
            //if NewPopulationSize is not set than set the input population size
            int childreenPopulationSize;
            if (NewPopulationSize > 0)
            {
                childreenPopulationSize = NewPopulationSize;
            }
            else
            {
                childreenPopulationSize = population.Count;
            }

            //make corssover
            Population NewPopulation = new Population();
            while (NewPopulation.Count < childreenPopulationSize)
            {
                Population kids = Combine(population);
                NewPopulation.Add(kids[0]);
                //Add 2 childreen to the new population if it is not under the limit (NewPopulationSize)
                if (NewPopulation.Count > childreenPopulationSize)
                {
                    NewPopulation.Add(kids[1]);
                }
            }
            return NewPopulation;
        }

        /// <summary>
        /// Combine two parents and generate two childreen.
        /// </summary>
        /// <param name="population">Parents population.</param>
        /// <returns>Two childreen.</returns>
        protected Population Combine(Population population)
        {
            BinaryIndividual parent1 = (BinaryIndividual)population[Randomizer.NextInt(0, population.Count - 1)];
            BinaryIndividual parent2 = (BinaryIndividual)population[Randomizer.NextInt(0, population.Count - 1)];

            List<int> chromosomeParent1 = (List<int>)parent1.Chromosome;
            List<int> chromosomeParent2 = (List<int>)parent2.Chromosome;

            List<int> chromosomeChild1 = new List<int>();
            List<int> chromosomeChild2 = new List<int>();

            for (int i = 0; i < chromosomeParent1.Count; i++)
            {
                double geneFromFirstParent = Randomizer.NextDouble(0, 1);
                if (geneFromFirstParent < 0.5)
                {
                    //Childreen 1 gets i-th gene from parent 1.
                    chromosomeChild1.Add(chromosomeParent1[i]);
                    chromosomeChild2.Add(chromosomeParent2[i]);
                }
                else
                {
                    //Childreen 1 gets i-th gene from parent 2.
                    chromosomeChild1.Add(chromosomeParent2[i]);
                    chromosomeChild2.Add(chromosomeParent1[i]);
                }
            }

            Population childreen = new Population();
            BinaryIndividual kid1 = new BinaryIndividual(chromosomeChild1);
            BinaryIndividual kid2 = new BinaryIndividual(chromosomeChild2);
            childreen.Add(kid1);
            childreen.Add(kid2);
            return childreen;
        }
    }
}
