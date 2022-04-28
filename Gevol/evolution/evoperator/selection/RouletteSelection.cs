using Gevol.evolution.individual;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.selection
{
    /// <summary>
    /// It selects given number of individuals as a new population
    /// based on their fitness.
    /// </summary>
    public class RouletteSelection : Operator
    {
        private int _size = -10000;

        /// <summary>
        /// Size of new population. 
        /// How many the best individuals is copied to the new population.
        /// </summary>
        public int Size
        {
            get { return _size; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Size", value, "Size must be greater than 0.");
                }
                else { _size = value; }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">How many individuals is choosen.</param>
        public RouletteSelection(int size)
        {
            this.Size = size;
        }

        public override Population Apply(Population population)
        {
            IList<double> fitness = CalculateFitness(population);
            Population newPopulation = new Population();
            int counter = 0;

            while(counter < Size)
            {
                newPopulation.Add(population[Randomizer.NextIndex(fitness)]);
                counter++;
            }
            
            /*

            int currentPopSize = population.Count-1;
            int selectedIndv;
            while (counter < Size)
            {
                selectedIndv = Randomizer.NextInt(0, currentPopSize);
                //1 - fitness, because we are looking for global minimum
                //for low values fitness is also low
                if (fitness[selectedIndv] >= Randomizer.NextDouble(0, 1))
                {
                    newPopulation.Add(population[selectedIndv]);
                    counter++;
                }
            }*/
            return newPopulation;
        }

        /// <summary>
        /// Fitness is calculated for first objective function according to the formula
        /// fitness = (Fmax - F)/sum(Fmax - F)
        /// F - score for this individual
        /// Fmax - the worst score in population
        /// sum(Fmax - F) - sum of all individuals
        /// 
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        protected IList<double> CalculateFitness(Population population)
        {
            if (population == null || population.Count == 0)
            {
                throw new ArgumentException("Population should not be empty.", "population");
            }
            IList<double> fitness = new List<double>();
            Population temp = new Population(population);
            double theWorstScore;
            double sumOfAllScores = 0;

            //get the best individual score
            temp.Sort();
            theWorstScore = temp.Last().Score[0];

            //calculate F - Fmin for each individual and sum of that for all
            for (int i = 0; i < population.Count; i++)
            {
                fitness.Add(theWorstScore - population[i].Score[0]);
                sumOfAllScores += fitness[i];
            }

            //divide
            for (int i = 0; i < population.Count; i++)
            {
                fitness[i] = fitness[i] / sumOfAllScores;
            }
            return fitness;
        }
    }
}
