using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.eda.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual;
using Gevol.evolution.evoperator.eda.model;
using Gevol.evolution.util;

namespace Gevol.evolution.evoperator.eda.binary.Tests
{
    [TestClass()]
    public class PBILOperatorTests 
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PBILOperatorWrongNewPopulationSizeTest()
        {
            PBILOperator pbil = new PBILOperator(0, 5, 0.2, 0.3, 0.5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PBILOperatorWrongChromosomeLengthTest()
        {
            PBILOperator pbil = new PBILOperator(10, -5, 0.2, 0.3, 0.5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PBILOperatorWrongLearningRateTest()
        {
            PBILOperator pbil = new PBILOperator(10, 5, -0.2, 0.3, 0.5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PBILOperatorWrongMutationSizeTest()
        {
            PBILOperator pbil = new PBILOperator(10, 5, 0.2, -0.3, 0.5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PBILOperatorWrongMutationProbabilityTest1()
        {
            PBILOperator pbil = new PBILOperator(10, 5, 0.2, 0.3, -0.5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PBILOperatorWrongMutationProbabilityTest2()
        {
            PBILOperator pbil = new PBILOperator(10, 5, 0.2, 0.3, 1.5);
        }

        [TestMethod()]
        public void modifyProbabilityInternalTest()
        {
            Assert.AreEqual<double>(0.32, Math.Round(modifyProbability(0.4, 0.2, 0),4), "Internal new probability is calculated incorrectly");
            Assert.AreEqual<double>(0.52, Math.Round(modifyProbability(0.4, 0.2, 1), 4), "Internal new probability is calculated incorrectly");
            Assert.AreEqual<double>(0.6952, Math.Round(modifyProbability(0.88, 0.21, 0), 4), "Internal new probability is calculated incorrectly");
            Assert.AreEqual<double>(0.9944, Math.Round(modifyProbability(0.96, 0.86, 1), 4), "Internal new probability is calculated incorrectly");
            Assert.AreEqual<double>(0.32, Math.Round(modifyProbability(0.2, 0.15, 1), 4), "Internal new probability is calculated incorrectly");
        }

        [TestMethod()]
        public void ApplyNoMutationTest()
        {
            PBILModel model = new PBILModel() { probabilities = new List<double>() { 0.8, 0.2, 0.3, 0, 1 } };
            PBILOperator pbil = new PBILOperator(1, 5, 0.27, 0.3, 0); 
            pbil.Model = new PBILModel() { probabilities = new List<double>() { 0.8, 0.2, 0.3, 0, 1 } }; //object has reference, so model always is equal to newModel
            BinaryIndividual theBestIndividual = new BinaryIndividual(new List<int>() { 1, 1, 0, 1, 1 }) { Score = new List<double>() { 3 } };
            Population population = new Population() {
                new BinaryIndividual(new List<int>() { 0, 0, 0, 1, 1 }) { Score = new List<double>() { 32 } },
                theBestIndividual,
                new BinaryIndividual(new List<int>() { 1, 0, 0, 1, 0 }) { Score = new List<double>() { 13 } }};
            
            pbil.Apply(population);
            PBILModel newModel = (PBILModel)pbil.Model;

            Assert.AreEqual<double>(modifyProbability(model.probabilities[0], 0.27, ((IList<int>)theBestIndividual.Chromosome)[0]), newModel.probabilities[0], "Probability 0 is wrong.");
            Assert.AreEqual<double>(modifyProbability(model.probabilities[1], 0.27, ((IList<int>)theBestIndividual.Chromosome)[1]), newModel.probabilities[1], "Probability 1 is wrong.");
            Assert.AreEqual<double>(modifyProbability(model.probabilities[2], 0.27, ((IList<int>)theBestIndividual.Chromosome)[2]), newModel.probabilities[2], "Probability 2 is wrong.");
            Assert.AreEqual<double>(modifyProbability(model.probabilities[3], 0.27, ((IList<int>)theBestIndividual.Chromosome)[3]), newModel.probabilities[3], "Probability 3 is wrong.");
            Assert.AreEqual<double>(modifyProbability(model.probabilities[4], 0.27, ((IList<int>)theBestIndividual.Chromosome)[4]), newModel.probabilities[4], "Probability 4 is wrong.");
        }

        [TestMethod()]
        public void ApplyMutationTest()
        {
            PBILModel model = new PBILModel() { probabilities = new List<double>() { 0.8, 0.2, 0.3, 0, 1, 0.01 } };
            PBILOperator pbil = new PBILOperator(1, 6, 0.44, 0.3, 1);
            pbil.Model = new PBILModel() { probabilities = new List<double>() { 0.8, 0.2, 0.3, 0, 1, 0.01 } }; //object has reference, so model always is equal to newModel
            BinaryIndividual theBestIndividual = new BinaryIndividual(new List<int>() { 0, 1, 0, 0, 1, 0 }) { Score = new List<double>() { 3 } };
            Population population = new Population() {
                new BinaryIndividual(new List<int>() { 0, 0, 0, 1, 1, 1 }) { Score = new List<double>() { 32 } },
                theBestIndividual,
                new BinaryIndividual(new List<int>() { 1, 0, 0, 1, 0, 0 }) { Score = new List<double>() { 13 } }};

            pbil.Apply(population);
            PBILModel newModel = (PBILModel)pbil.Model;

            Console.Out.WriteLine("Mutation probability = 1; mutation size = 0.3; learning rate = 0.44");
            for (int i = 0; i < model.probabilities.Count; i++)
                Console.Out.WriteLine("[{0}] {1} => {2}", i, model.probabilities[i], newModel.probabilities[i]);

            if (newModel.probabilities[0] != ((modifyProbability(model.probabilities[0], 0.44, ((IList<int>)theBestIndividual.Chromosome)[0]) * (1 - 0.3)) + 0.3) && newModel.probabilities[0] != (modifyProbability(model.probabilities[0], 0.44, ((IList<int>)theBestIndividual.Chromosome)[0]) * (1 - 0.3)))
                Assert.Fail("Probability 0 has wrong value = " + newModel.probabilities[0]);
            if (newModel.probabilities[1] != ((modifyProbability(model.probabilities[1], 0.44, ((IList<int>)theBestIndividual.Chromosome)[1]) * (1 - 0.3)) + 0.3) && newModel.probabilities[1] != (modifyProbability(model.probabilities[1], 0.44, ((IList<int>)theBestIndividual.Chromosome)[1]) * (1 - 0.3)))
                Assert.Fail("Probability 1 has wrong value = " + newModel.probabilities[1]);
            if (newModel.probabilities[2] != ((modifyProbability(model.probabilities[2], 0.44, ((IList<int>)theBestIndividual.Chromosome)[2]) * (1 - 0.3)) + 0.3) && newModel.probabilities[2] != (modifyProbability(model.probabilities[2], 0.44, ((IList<int>)theBestIndividual.Chromosome)[2]) * (1 - 0.3)))
                Assert.Fail("Probability 2 has wrong value = " + newModel.probabilities[2]);
            if (newModel.probabilities[3] != ((modifyProbability(model.probabilities[3], 0.44, ((IList<int>)theBestIndividual.Chromosome)[3]) * (1 - 0.3)) + 0.3) && newModel.probabilities[3] != (modifyProbability(model.probabilities[3], 0.44, ((IList<int>)theBestIndividual.Chromosome)[3]) * (1 - 0.3)))
                Assert.Fail("Probability 3 has wrong value = " + newModel.probabilities[3]);
            if (newModel.probabilities[4] != ((modifyProbability(model.probabilities[4], 0.44, ((IList<int>)theBestIndividual.Chromosome)[4]) * (1 - 0.3)) + 0.3) && newModel.probabilities[4] != (modifyProbability(model.probabilities[4], 0.44, ((IList<int>)theBestIndividual.Chromosome)[4]) * (1 - 0.3)))
                Assert.Fail("Probability 4 has wrong value = " + newModel.probabilities[4]);
            if (newModel.probabilities[5] != ((modifyProbability(model.probabilities[5], 0.44, ((IList<int>)theBestIndividual.Chromosome)[5]) * (1 - 0.3)) + 0.3) && newModel.probabilities[5] != (modifyProbability(model.probabilities[5], 0.44, ((IList<int>)theBestIndividual.Chromosome)[5]) * (1 - 0.3)))
                Assert.Fail("Probability 5 has wrong value = " + newModel.probabilities[5]);
        }

        [TestMethod()]
        public void ApplyGenerateOnesTest()
        {
            PBILModel model = new PBILModel() { probabilities = new List<double>() { 1, 1, 1, 1 } };
            PBILOperator pbil = new PBILOperator(100, 4, 0.44, 0.23, 0.14);
            pbil.Model = model;
            BinaryIndividual theBestIndividual = new BinaryIndividual(new List<int>() { 1, 1, 1, 1 }) { Score = new List<double>() { 3 } };
            Population population = new Population() {
                new BinaryIndividual(new List<int>() { 0, 0, 0, 1 }) { Score = new List<double>() { 32 } },
                new BinaryIndividual(new List<int>() { 1, 0, 0, 1 }) { Score = new List<double>() { 13 } },
                theBestIndividual};

            Population newPopulation = pbil.Apply(population);

            int countOnes = 0;
            int countAll = 0;
            foreach(Individual individual in newPopulation)
            {
                foreach(int gene in (IList<int>)individual.Chromosome)
                {
                    if (gene == 1)
                        countOnes++;
                    countAll++;
                }
            }
            Assert.AreEqual<int>(400, countAll, "Wrong number of individuals and / or genes were generated.");
            Assert.AreEqual<int>(400, countOnes, "Not only 1 was generated by the algorithm.");
        }

        [TestMethod()]
        public void ApplyGenerateZerosTest()
        {
            PBILModel model = new PBILModel() { probabilities = new List<double>() { 0, 0, 0, 0 } };
            PBILOperator pbil = new PBILOperator(100, 4, 0.44, 0.23, 0.14);
            pbil.Model = model;
            BinaryIndividual theBestIndividual = new BinaryIndividual(new List<int>() { 0, 0, 0, 0 }) { Score = new List<double>() { 3 } };
            Population population = new Population() {
                new BinaryIndividual(new List<int>() { 0, 0, 0, 1 }) { Score = new List<double>() { 32 } },
                new BinaryIndividual(new List<int>() { 1, 0, 0, 1 }) { Score = new List<double>() { 13 } },
                theBestIndividual};

            Population newPopulation = pbil.Apply(population);

            int countOnes = 0;
            int countAll = 0;
            foreach (Individual individual in newPopulation)
            {
                foreach (int gene in (IList<int>)individual.Chromosome)
                {
                    if (gene == 0)
                        countOnes++;
                    countAll++;
                }
            }
            Assert.AreEqual<int>(400, countAll, "Wrong number of individuals and / or genes were generated.");
            Assert.AreEqual<int>(400, countOnes, "Not only 0 was generated by the algorithm.");
        }

        [TestMethod()]
        public void ApplyGenerateMixedTest()
        {
            PBILModel model = new PBILModel() { probabilities = new List<double>() { 0, 1, 0.5, 0.2 } };
            PBILOperator pbil = new PBILOperator(100, 4, 0.44, 0.23, 0.14);
            pbil.Model = model;
            BinaryIndividual theBestIndividual = new BinaryIndividual(new List<int>() { 0, 1, 1, 0 }) { Score = new List<double>() { 3 } };
            Population population = new Population() {
                new BinaryIndividual(new List<int>() { 0, 0, 0, 1, 1, 1 }) { Score = new List<double>() { 32 } },
                new BinaryIndividual(new List<int>() { 1, 0, 0, 1, 0, 0 }) { Score = new List<double>() { 13 } },
                theBestIndividual};

            Population newPopulation = pbil.Apply(population);

            int countFirst = 0;
            int countSecond = 0;
            int countThird = 0;
            int countFourth = 0;
            int countAll = 0;
            foreach (Individual individual in newPopulation)
            {
                IList<int> chromosome = (IList<int>)individual.Chromosome;
                if (chromosome[0] == 1)
                    countFirst++;
                if (chromosome[1] == 1)
                    countSecond++;
                if (chromosome[2] == 1)
                    countThird++;
                if (chromosome[3] == 1)
                    countFourth++;
                countAll += chromosome.Count;
            }
            Console.Out.WriteLine("[0] expected: " + (100 * model.probabilities[0]) + " actual: " + countFirst);
            Console.Out.WriteLine("[1] expected: " + (100 * model.probabilities[1]) + " actual: " + countSecond);
            Console.Out.WriteLine("[2] expected: " + (100 * model.probabilities[2]) + " actual: " + countThird);
            Console.Out.WriteLine("[3] expected: " + (100 * model.probabilities[3]) + " actual: " + countFourth);

            Assert.AreEqual<int>(400, countAll, "Wrong number of individuals and / or genes were generated.");
            if (countFirst < (countAll * model.probabilities[0]) - 10 && countFirst > (countAll * model.probabilities[0]) + 10)
                Assert.Fail("First chromosome is calculated incorect.");
            if (countFirst < (countAll * model.probabilities[1]) - 10 && countFirst > (countAll * model.probabilities[1]) + 10)
                Assert.Fail("Second chromosome is calculated incorect.");
            if (countFirst < (countAll * model.probabilities[2]) - 10 && countFirst > (countAll * model.probabilities[2]) + 10)
                Assert.Fail("Third chromosome is calculated incorect.");
            if (countFirst < (countAll * model.probabilities[3]) - 10 && countFirst > (countAll * model.probabilities[3]) + 10)
                Assert.Fail("Fourth chromosome is calculated incorect.");
        }

        private double modifyProbability(double probability, double learningRate, int geneValue)
        {
            return ((1 - learningRate) * probability) + (((double)geneValue) * learningRate);
        }
    }
}