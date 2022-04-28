using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;

namespace Gevol.evolution.evoperator.selection.Tests
{
    [TestClass()]
    public class RouletteSelectionTests : RouletteSelection
    {
        public RouletteSelectionTests() : base(1)
        {
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RouletteSelectionTest()
        {
            RouletteSelection rs = new RouletteSelection(0);
        }

        [TestMethod()]
        public void ApplyTest()
        {
            Population pop1 = GeneratePopulation(0);
            Console.WriteLine("Input population: ");
            PrintList(pop1);
            this.Size = 13;
            Population newPop = Apply(pop1);
            Console.WriteLine("Output population: ");
            PrintList(newPop);

            Console.WriteLine("-------------");
            Console.WriteLine("next population:");

            Population pop2 = GeneratePopulation(1);
            Console.WriteLine("Input population: ");
            PrintList(pop2);
            this.Size = 13;
            Population newPop2 = Apply(pop2);
            Console.WriteLine("Output population: ");
            PrintList(newPop2);
        }

        [TestMethod()]
        public void CalculateFitnessTest()
        {
            //Population 0:
            Population pop = GeneratePopulation(0);
            IList<double> fitness = this.CalculateFitness(pop);
            IList<double> expectedFitness = CalculateFitness(0);
            Assert.AreEqual<int>(pop.Count, fitness.Count, "Fitness is not calculated for all individuals (population 0). Population size: {0}, number of fitness values: {1}", pop.Count, fitness.Count);
            double fitnessSum = 0;
            for(int i = 0; i < fitness.Count; i++)
            {
                Assert.AreEqual<double>(expectedFitness[i], Math.Round(fitness[i], 4), "Fitness is wrong for population 0, individual {0}.", i);
                fitnessSum += fitness[i];
            }
            Assert.AreEqual<double>(1.0, fitnessSum, "Sum of all fitness values should be 1, but it is {0} (population 1).", fitnessSum);

            //Population 1:
            pop = GeneratePopulation(1);
            fitness = this.CalculateFitness(pop);
            expectedFitness = CalculateFitness(1);
            Assert.AreEqual<int>(pop.Count, fitness.Count, "Fitness is not calculated for all individuals (population 1). Population size: {0}, number of fitness values: {1}", pop.Count, fitness.Count);
            fitnessSum = 0;
            for (int i = 0; i < fitness.Count; i++)
            {
                Assert.AreEqual<double>(expectedFitness[i], Math.Round(fitness[i], 4), "Fitness is wrong for population 1, individual {0}.", i);
                fitnessSum += fitness[i];
            }
            Assert.AreEqual<double>(1.0, fitnessSum, "Sum of all fitness values should be 1, but it is {0} (population 1).", fitnessSum);

            //Population 2:
            pop = GeneratePopulation(2);
            fitness = this.CalculateFitness(pop);
            expectedFitness = CalculateFitness(2);
            Assert.AreEqual<int>(pop.Count, fitness.Count, "Fitness is not calculated for all individuals (population 2). Population size: {0}, number of fitness values: {1}", pop.Count, fitness.Count);
            fitnessSum = 0;
            for (int i = 0; i < fitness.Count; i++)
            {
                Assert.AreEqual<double>(expectedFitness[i], Math.Round(fitness[i], 4), "Fitness is wrong for population 2, individual {0}.", i);
                fitnessSum += fitness[i];
            }
            Assert.AreEqual<double>(1.0, fitnessSum, "Sum of all fitness values should be 1, but it is {0} (population 2).", fitnessSum);
        }

        private Population GeneratePopulation(int populationNumber)
        {
            switch (populationNumber)
            {
                case 0:
                    Population pop0 = new Population();
                    pop0.Add(new BinaryIndividual() { Score = new List<double>() { 5 } });
                    pop0.Add(new BinaryIndividual() { Score = new List<double>() { 34 } });
                    pop0.Add(new BinaryIndividual() { Score = new List<double>() { 1 } });
                    pop0.Add(new BinaryIndividual() { Score = new List<double>() { 0 } });
                    pop0.Add(new BinaryIndividual() { Score = new List<double>() { 77 } });
                    pop0.Add(new BinaryIndividual() { Score = new List<double>() { 4 } });
                    return pop0;
                case 1:
                    Population pop1 = new Population();
                    pop1.Add(new BinaryIndividual() { Score = new List<double>() { -5 } });
                    pop1.Add(new BinaryIndividual() { Score = new List<double>() { -2 } });
                    pop1.Add(new BinaryIndividual() { Score = new List<double>() { -3 } });
                    pop1.Add(new BinaryIndividual() { Score = new List<double>() { -1 } });
                    pop1.Add(new BinaryIndividual() { Score = new List<double>() { -6 } });
                    return pop1;
                case 2:
                    Population pop2 = new Population();
                    pop2.Add(new BinaryIndividual() { Score = new List<double>() { 5 } });
                    pop2.Add(new BinaryIndividual() { Score = new List<double>() { 2 } });
                    pop2.Add(new BinaryIndividual() { Score = new List<double>() { 3 } });
                    pop2.Add(new BinaryIndividual() { Score = new List<double>() { 1 } });
                    pop2.Add(new BinaryIndividual() { Score = new List<double>() { 6 } });
                    return pop2;
            }
            return null;
        }

        private IList<double> CalculateFitness(int populationNumber)
        {
            //formula: (F - Fmin)/sum(F - Fmin) -> if max is the best
            //(Fmax - F)/sum(Fmax - F) -> if min is the bet
            IList<double> fitness = new List<double>();
            switch (populationNumber)
            {
                case 0:
                    fitness.Add(0.2111);
                    fitness.Add(0.1261);
                    fitness.Add(0.2229);
                    fitness.Add(0.2258);
                    fitness.Add(0.0);
                    fitness.Add(0.2141);
                    break;
                case 1:
                    fitness.Add(0.3333);
                    fitness.Add(0.0833);
                    fitness.Add(0.1667);
                    fitness.Add(0.0);
                    fitness.Add(0.4167);
                    break;
                case 2:
                    fitness.Add(0.0769);
                    fitness.Add(0.3077);
                    fitness.Add(0.2308);
                    fitness.Add(0.3846);
                    fitness.Add(0.0);
                    break;
            }
            return fitness;
        }

        /// <summary>
        /// Print population to Console.out
        /// </summary>
        /// <param name="list"></param>
        private void PrintList(Population list)
        {
            IList<double> fitness = CalculateFitness(list);
            for (int i = 0; i < list.Count; i++)
                Console.Out.Write(list[i].Score[0] + " (" + fitness[i] + ") | ");
            Console.WriteLine();
        }
    }
}