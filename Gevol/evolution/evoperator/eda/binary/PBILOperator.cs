using Gevol.evolution.evoperator.eda.model;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.eda.binary
{
    /// <summary>
    /// Implements operator for Population-Based Incremental Learning PBIL.
    /// Model is a list of double what determines probability to get 1 for each gene.
    /// </summary>
    public class PBILOperator : EdaOperator
    {
        private int _newPopulationSize = 0;
        private double _learningRate = 0.0;
        private double _mutationSize = 0.0;
        private double _mutationProbability = 0.0;
        private int _chromosomeLength = 0;

        public PBILOperator(int newPopulationSize, int chromosomeLength, double learningRate, double mutationSize, double mutationProbability)
        {
            NewPopulationSize = newPopulationSize;
            LearningRate = learningRate;
            MutationSize = mutationSize;
            MutationProbability = mutationProbability;
            ChromosomeLength = chromosomeLength;

            PBILModel model = new PBILModel();
            model.probabilities = Enumerable.Repeat<double>(0.5, chromosomeLength).ToList<double>();
            Model = model;
        }

        public override Population Apply(Population population)
        {
            if (!(population[0] is BinaryIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of BinaryIndividual.", "population");
            }
            CalculateModel(population);

            return GeneratePopulation();
        }

        protected override void CalculateModel(Population population)
        {
            population.Sort();  //get the best individual
            PBILModel model = (PBILModel)Model;
            for (int i = 0; i < _chromosomeLength; i++)
            {
                model.probabilities[i] = ((1 - _learningRate) * model.probabilities[i]) + (((IList<int>)population[0].Chromosome)[i] * _learningRate);
                if(Randomizer.NextDouble(0.0, 1.0) <= _mutationProbability)
                {
                    model.probabilities[i] = Mutation(model.probabilities[i]);
                }
            }
            this.Model = model;
        }

        /// <summary>
        /// Mutate the probability.
        /// Formula:
        ///    P = P(1 - MUT) + rand(1,0)MUT
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        protected double Mutation(double probability)
        {
            return (probability * (1 - _mutationSize)) + (Randomizer.NextInt(0, 1) * _mutationSize);
        }

        protected Population GeneratePopulation()
        {
            Population population = new Population();
            PBILModel model = (PBILModel)Model;
            for (int i = 0; i < _newPopulationSize; i++)
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
        /// Size of the learning rate. 
        /// How strong each probability gene in the vector in the model is being modified based on the current the best value.
        /// </summary>
        public double LearningRate
        {
            get { return _learningRate; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("LearningRate", value, "Value must be greater than 0.");
                }
                else { _learningRate = value; }
            }
        }

        /// <summary>
        /// Size of the mutation. 
        /// How strong each probability gene in the vector in the model is being modified.
        /// </summary>
        public double MutationSize
        {
            get { return _mutationSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("MutationSize", value, "Value must be greater or equal 0.");
                }
                else { _mutationSize = value; }
            }
        }

        /// <summary>
        /// Probability that the mutation occurs. 
        /// </summary>
        public double MutationProbability
        {
            get { return _mutationProbability; }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("MutationProbability", value, "Value must be between 0 and 1.");
                }
                else { _mutationProbability = value; }
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
