﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.evoperator.eda.model;
using Gevol.evolution.util;
using Gevol.evolution.individual.binary;

namespace Gevol.evolution.evoperator.eda.binary
{
    public class ECGAOperator : EdaOperator
    {
        private int _chromosomeLength = 0;
        private int _newPopulationSize = 0;

        public ECGAOperator(int newPopulationSize, int chromosomeLength)
        {
            ChromosomeLength = chromosomeLength;
            NewPopulationSize = newPopulationSize;

            Model = new ECGAModel();
        }

        /// <summary>
        /// Perferom greedy search for better MPM model.
        /// If better model is found, it replaces the current model.
        /// New population is generated by the current model, doesn't matter if it is the same as in the previous operation or new one.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Population Apply(Population population)
        {
            ECGAModel newModel = new ECGAModel();
            newModel.PopulationSize = population.Count;
            //at the beginning every gene is in a separate block
            for(int i = 0; i < _chromosomeLength; i++)
            {
                newModel.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { i } });
            }
            CalculateModel(population, ref newModel);   //calculate probabilities for the new model
            ECGAModel concatenatedModel = newModel;
            while (true)
            {
                //concatenatedModel.CalculateCC(); Console.WriteLine("concatenatedModel = " + concatenatedModel.ToString());   //TEST
                //try to concatenate every group to every group and select the best concatenation
                for(int i = 0; i < newModel.Blocks.Count - 1; i++)
                {
                    for (int j = i + 1; j < newModel.Blocks.Count; j++)
                    {
                        ECGAModel newConcatenatedModel = ConcatenateGroups(newModel, i, j);
                        CalculateModel(population, ref newConcatenatedModel);   //calculate probabilities for the new model
                        newConcatenatedModel.CalculateCC();
                        if (newConcatenatedModel.CC < concatenatedModel.CC)
                        {
                            concatenatedModel = newConcatenatedModel;
                        }
                        //Console.WriteLine("newConcatenatedModel = " + newConcatenatedModel.ToString());     //TEST
                    }
                }
                if (concatenatedModel.CC < newModel.CC)
                {
                    newModel = concatenatedModel;
                    continue;
                }
                break;
            }
            this.Model = newModel;
            return GeneratePopulation();
        }

        protected ECGAModel ConcatenateGroups(ECGAModel model, int groupA, int groupB)
        {
            ECGAModel newModel = model.Clone();
            ((List<int>)newModel.Blocks[groupA].Genes).AddRange(newModel.Blocks[groupB].Genes);
            ((List<int>)newModel.Blocks[groupA].Genes).Sort(); 
            newModel.Blocks.RemoveAt(groupB);
            newModel.Blocks[groupA].Probabilities = new List<double>(new double[(int)Math.Pow(2, newModel.Blocks[groupA].Genes.Count)]);
            return newModel;
        }

        protected override void CalculateModel(Population population)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Calculate probabilities for the model.
        /// </summary>
        /// <param name="population"></param>
        protected void CalculateModel(Population population, ref ECGAModel model)
        {
            if (!(population[0] is BinaryIndividual))
            {
                throw new ArgumentException("Individuals in population must be type of BinaryIndividual.", "population");
            }
            //reset probabilities
            foreach (ECGAStructure block in model.Blocks)
            {
                block.Probabilities = new List<double>(new double[(int)Math.Pow(2, block.Genes.Count)]);
            }
            //loop on the population
            foreach (BinaryIndividual individual in population)
            {
                foreach(ECGAStructure block in model.Blocks)
                {
                    IList<int> genesValues = new List<int>();
                    for (int g = 0; g < block.Genes.Count; g++)
                    {
                        //block.Genes[g] has number of gene in the chromosome
                        //take the value of particular gene from chromosome and add it to the values in the block.
                        genesValues.Add(((IList<int>)individual.Chromosome)[block.Genes[g]]);
                    }
                    //we know value for each gene, translate it into index to increase number of occurences
                    block.Probabilities[model.GenesSetToIndex(genesValues)]++;
                }
            }
            //calculate probabilities
            foreach (ECGAStructure block in model.Blocks)
            {
                for(int i = 0; i < block.Probabilities.Count; i++)
                {
                    block.Probabilities[i] /= population.Count;
                }
            }
        }

        protected Population GeneratePopulation()
        {
            Population population = new Population();
            ECGAModel model = (ECGAModel)Model;
            int i = 0;
            while (i < NewPopulationSize)
            {
                int[] chromosome = new int[_chromosomeLength];
                foreach(ECGAStructure block in model.Blocks)
                {
                    IList<int> genesValues = model.IndexToGenesSet(Randomizer.NextIndex(block.Probabilities), block.Genes.Count);
                    for(int g = 0; g < genesValues.Count; g++)
                    {
                        chromosome[block.Genes[g]] = genesValues[g];
                    }
                }
                population.Add(new BinaryIndividual(new List<int>(chromosome)));
                i++;
            }
            return population;
        }

        /// <summary>
        /// Chromosome length
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
                    throw new ArgumentOutOfRangeException("NewPopulationSize", value, "New population size must be greater than 0.");
                }
                else { _newPopulationSize = value; }
            }
        }
    }
}