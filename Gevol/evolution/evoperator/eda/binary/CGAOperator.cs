using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.evoperator.eda.model;
using Gevol.evolution.util;

namespace Gevol.evolution.evoperator.eda.binary
{
    /// <summary>
    /// It implements the Compact Genetic Algorithm. This operator calculate new probabilities for each gene.
    /// The population should have two individuals. The better one is taken and the probabilities are modified accordingly.
    /// The formula to modify probability for each gene:
    /// p = p + 1/n, if the gene is different for each individual and the value is 1 for the better solution.
    /// p = p - 1/n, if the gene is different for each individual and the value is 0 for the better solution.
    /// n is the simulated size of the population.
    /// </summary>
    public class CGAOperator : EdaOperator
    {
        private int _simulatedPopulationSize = 0;
        /// <summary>
        /// _stepSize = 1 / simulatedPopulationSize; -> how strong the probability is being changed in every iteration
        /// </summary>
        private double _stepSize;
        private int _chromosomeLength = 0;

        public CGAOperator(int simulatedPopulationSize, int chromosomeLength)
        {
            SimulatedPopulationSize = simulatedPopulationSize;
            ChromosomeLength = chromosomeLength;

            CGAModel model = new CGAModel();
            model.probabilities = Enumerable.Repeat<double>(0.5, chromosomeLength).ToList<double>();
            Model = model;
        }

        public override Population Apply(Population population)
        {
            if(population.Count != 2)
            {
                throw new ArgumentOutOfRangeException("population", "Population should have two individuals.");
            }
            if (!(population[0] is BinaryIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of BinaryIndividual.", "population");
            }
            population.Sort();
            CalculateModel(population);
            return GeneratePopulation();
        }

        protected override void CalculateModel(Population population)
        {
            population.Sort();  //get the best individual
            IList<int> betterIndividualChromosome = (IList<int>)((BinaryIndividual)population[0]).Chromosome;
            IList<int> worseIndividualChromosome = (IList<int>)((BinaryIndividual)population[1]).Chromosome;
            CGAModel model = (CGAModel)Model;
            for (int i = 0; i < _chromosomeLength; i++)
            {
                if(betterIndividualChromosome[i] != worseIndividualChromosome[i])
                {
                    if(betterIndividualChromosome[i] == 1)
                    {
                        model.probabilities[i] += _stepSize;
                    } else
                    {   //so, better value is 0
                        model.probabilities[i] -= _stepSize;
                    }
                }
            }
            this.Model = model;
        }

        protected Population GeneratePopulation()
        {
            Population population = new Population();
            CGAModel model = (CGAModel)Model;
            for (int i = 0; i < 2; i++) //new population has always two individuals
            {
                IList<int> chromosome = new List<int>(model.probabilities.Count);
                for (int k = 0; k < model.probabilities.Count; k++)
                {
                    chromosome.Add(Randomizer.NextBoolIntProbability(model.probabilities[k]));
                }
                population.Add(new BinaryIndividual(chromosome));
            }
            return population;
        }

        /// <summary>
        /// Size of the simulated population. 
        /// </summary>
        public int SimulatedPopulationSize
        {
            get { return _simulatedPopulationSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("SimulatedPopulationSize", value, "Value must be bigger than 0.");
                }
                else { _simulatedPopulationSize = value; _stepSize = 1.0 / (double)_simulatedPopulationSize; }
            }
        }

        /// <summary>
        /// Size of new population. 
        /// How big population should be generated.
        /// </summary>
        public int ChromosomeLength
        {
            get { return _chromosomeLength; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("ChromosomeLength", value, "Chromosome length must be greater than 0.");
                }
                else { _chromosomeLength = value; }
            }
        }
    }
}
