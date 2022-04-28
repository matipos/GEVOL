using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.util;
using Gevol.evolution.objective.binary;
using Gevol.evolution.individual.binary;

namespace Gevol.evolution.evoperator.util.Tests
{
    [TestClass()]
    public class RechenbergSuccessRuleTests
    {
        /// <summary>
        /// Test correctness of parameters ratio and modifyPower.
        /// </summary>
        [TestMethod()]
        public void RechenbergSuccessRuleParametersTest()
        {
            RechenbergSuccessRule rechenberg = new RechenbergSuccessRule(new OneMax());
            Assert.AreEqual<double>(0.2, rechenberg.Ratio, "Default ratio should be 0.2, but it is {0}.", rechenberg.Ratio);
            Assert.AreEqual<double>(0.85, rechenberg.ModifyPower, "Default modify power should be 0.85, but it is {0}.", rechenberg.ModifyPower);

            rechenberg = new RechenbergSuccessRule(new OneMax(), 0.11, 0.97, 22);
            Assert.AreEqual<double>(0.11, rechenberg.Ratio, "Ratio should be 0.11, but it is {0}.", rechenberg.Ratio);
            Assert.AreEqual<double>(0.97, rechenberg.ModifyPower, "Modify power should be 0.97, but it is {0}.", rechenberg.ModifyPower);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RechenbergSuccessRuleRatioException1Test()
        {
            RechenbergSuccessRule rechenber = new RechenbergSuccessRule(new OneMax(), 0, 0.97, 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RechenbergSuccessRuleRatioException2Test()
        {
            RechenbergSuccessRule rechenber = new RechenbergSuccessRule(new OneMax(), 1, 0.97, 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RechenbergSuccessRuleModifyPowerException1Test()
        {
            RechenbergSuccessRule rechenber = new RechenbergSuccessRule(new OneMax(), 0.22, 0, 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RechenbergSuccessRuleModifyPowerException2Test()
        {
            RechenbergSuccessRule rechenber = new RechenbergSuccessRule(new OneMax(), 0.12, 1, 5);
        }

        [TestMethod()]
        public void initTest()
        {
            BadOperator badOperator = new BadOperator();
            GoodOperator goodOperator = new GoodOperator();
            Population population = generatePopulation(1);

            RechenbergSuccessRule rechenberg = new RechenbergSuccessRule(new OneMax());
            rechenberg.Operators.Add(badOperator);
            rechenberg.Operators.Add(goodOperator);

            rechenberg.Init();
            for(int i = 0; i < 20; i++)
            {
                Console.Out.WriteLine(i + " score = " + rechenberg.ObjectiveFunction.Evaluate(population[0]));
                Console.Out.WriteLine("Probability for bad operator after 100 run: " + rechenberg.Probabilities[0]);
                Console.Out.WriteLine("Probability for good operator after 100 run: " + rechenberg.Probabilities[1]);
                rechenberg.Apply(population);
            }
            Console.Out.WriteLine("Probability for bad operator after 100 run: " + rechenberg.Probabilities[0]);
            Console.Out.WriteLine("Probability for good operator after 100 run: " + rechenberg.Probabilities[1]);
            //check the probability
            if (rechenberg.Probabilities[0] <=0  || rechenberg.Probabilities[0] >= 0.5)
            {
                Assert.Fail("Probability for bad operator should be always decreased. It should have value between 0 and 0.5, but it has {0}.", rechenberg.Probabilities[0]);
            }
            if (rechenberg.Probabilities[1] <= 0.5)
            {
                Assert.Fail("Probability for good operator should be always increased. It should have value bigger than 0.5, but it has {0}.", rechenberg.Probabilities[1]);
            }

            rechenberg.Init();
            Assert.AreEqual<double>(1, rechenberg.Probabilities[1] + rechenberg.Probabilities[0], "After the init both probabilities should be 0.5. First probability = {0}, second = {1}.", rechenberg.Probabilities[0], rechenberg.Probabilities[1]);
        }

        [TestMethod()]
        public void ExecutionLimitTest()
        {
            BadOperator badOperator = new BadOperator();
            GoodOperator goodOperator = new GoodOperator();
            Population population = generatePopulation(1);

            RechenbergSuccessRule rechenberg = new RechenbergSuccessRule(new OneMax());
            rechenberg.Operators.Add(badOperator);
            rechenberg.Operators.Add(goodOperator);

            rechenberg.Init();
            for (int i = 0; i < rechenberg.ExecutionLimit; i++)
            {
                rechenberg.Apply(population);
            }

            Assert.AreEqual<double>(1, rechenberg.Probabilities[1] + rechenberg.Probabilities[0], "Execution limit has not been exceeded, so the probabilities should have not been modified, both should be 0.5. First probability = {0}, second = {1}.", rechenberg.Probabilities[0], rechenberg.Probabilities[1]);
        }

        [TestMethod()]
        public void ApplyTest()
        {
            BadOperator badOperator = new BadOperator();
            GoodOperator goodOperator = new GoodOperator();
            Population population = generatePopulation(1);

            RechenbergSuccessRule rechenberg = new RechenbergSuccessRule(new OneMax());
            rechenberg.Operators.Add(badOperator);
            rechenberg.Operators.Add(goodOperator);
            rechenberg.ExecutionLimit = 1;

            rechenberg.Init();
            Assert.AreEqual<double>(0.5, rechenberg.Probabilities[0], "Default probability for first operator should be 0.5, but it is {0}.", rechenberg.Probabilities[0]);
            Assert.AreEqual<double>(0.5, rechenberg.Probabilities[1], "Default probability for second operator should be 0.5, but it is {0}.", rechenberg.Probabilities[1]);

            rechenberg.Apply(population);
            Assert.AreNotEqual<double>(1, rechenberg.Probabilities[1] + rechenberg.Probabilities[0], "One of the probability has not been changed. First probability = {0}, second = {1}.", rechenberg.Probabilities[0], rechenberg.Probabilities[1]);

            rechenberg.Operators.RemoveAt(1);   //only bad operator is present
            rechenberg.Init();
            Assert.AreEqual<double>(0.5, rechenberg.Probabilities[0], "Default probability for first operator should be 0.5, but it is {0}. The reset didn't work.", rechenberg.Probabilities[0]);
            rechenberg.Apply(population);
            Assert.AreEqual<double>(0.5 * rechenberg.ModifyPower, rechenberg.Probabilities[0], "Probability for bad operator is not calculated properly.");

            rechenberg.Operators[0] = goodOperator; //only good operator is present
            rechenberg.Init();
            rechenberg.Apply(population);
            Assert.AreEqual<double>(0.5 / rechenberg.ModifyPower, rechenberg.Probabilities[0], "Probability for good operator is not calculated properly.");

            rechenberg.Operators.Clear();
            rechenberg.Operators.Add(badOperator);
            rechenberg.Operators.Add(goodOperator);
            rechenberg.Init();
            for (int i = 0; i < 10; i++)
            {
                rechenberg.Apply(population);
            }
            if (rechenberg.Probabilities[0] <= 0 || rechenberg.Probabilities[0] >= 0.5)
            {
                Assert.Fail("Probability for bad operator should be always decreased. It should have value between 0 and 0.5, but it has {0}.", rechenberg.Probabilities[0]);
            }
            if (rechenberg.Probabilities[1] <= 0.5)
            {
                Assert.Fail("Probability for good operator should be always increased. It should have value bigger than 0.5, but it has {0}.", rechenberg.Probabilities[1]);
            }
            Console.Out.WriteLine("Probability for bad operator after 10 run: " + rechenberg.Probabilities[0]);
            Console.Out.WriteLine("Probability for good operator after 10 run: " + rechenberg.Probabilities[1]);
        }

        /// <summary>
        /// Generate new population
        /// </summary>
        /// <param name="populationSize"></param>
        /// <returns></returns>
        private Population generatePopulation(int populationSize)
        {
            Population population = new Population();
            Individual individual = new BinaryIndividual();
            BinaryIndividualProperties properties = new BinaryIndividualProperties();
            properties.chromosomeLength = 27;
            for (int i = 0; i < populationSize; i++)
            {
                population.Add(individual.GenerateIndividual(properties));
            }
            return population;
        }

        /// <summary>
        /// Operator will generate new worse individual for OneMax function.
        /// </summary>
        private class BadOperator : Operator
        {
            /// <summary>
            /// Randomly change one gene with 1 to 0.
            /// </summary>
            /// <param name="population">Only the first individual will be taken</param>
            /// <returns></returns>
            public override Population Apply(Population population)
            {
                IList<int> oneValues = new List<int>();
                for(int i = 0; i < ((IList<int>)population[0].Chromosome).Count; i++)
                {
                    if(((IList<int>)population[0].Chromosome)[i] == 1)
                    {
                        oneValues.Add(i);
                    }
                }
                //rand which gene will be changed to 0
                if (oneValues.Count > 0)
                    ((IList<int>)population[0].Chromosome)[oneValues[Randomizer.NextInt(0, oneValues.Count - 1)]] = 0;
                return population;
            }
        }

        /// <summary>
        /// Operator will generate new better individual for OneMax function.
        /// </summary>
        private class GoodOperator : Operator
        {
            /// <summary>
            /// Randomly change one gene with 1 to 0.
            /// </summary>
            /// <param name="population">Only the first individual will be taken</param>
            /// <returns></returns>
            public override Population Apply(Population population)
            {
                IList<int> zeroValues = new List<int>();
                for (int i = 0; i < ((IList<int>)population[0].Chromosome).Count; i++)
                {
                    if (((IList<int>)population[0].Chromosome)[i] == 0)
                    {
                        zeroValues.Add(i);
                    }
                }
                //rand which gene will be changed to 0
                if(zeroValues.Count > 0) 
                    ((IList<int>)population[0].Chromosome)[zeroValues[Randomizer.NextInt(0, zeroValues.Count - 1)]] = 1;
                return population;
            }
        }
    }

    
}