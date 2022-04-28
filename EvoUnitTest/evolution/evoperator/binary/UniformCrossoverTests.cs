using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.evoperator.binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual;
namespace Gevol.evolution.evoperator.binary.Tests
{
    [TestClass()]
    public class UniformCrossoverTests : UniformCrossover //to test protected method Combine
    {
        /// <summary>
        /// Test for wrong size argument (Size = 0).
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UniformCrossoverTest()
        {
            UniformCrossover uc = new UniformCrossover(0);
        }

        /// <summary>
        /// Test for wrong size argument (Size = -1).
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UniformCrossover2Test()
        {
            UniformCrossover uc = new UniformCrossover(-1);
        }

        [TestMethod()]
        public void CombineTest()
        {
            BinaryIndividual in1 = new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BinaryIndividual in2 = new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Population pop = new Population();
            pop.Add(in1);
            pop.Add(in2);

            Population r1 = Combine(pop);
            Assert.AreEqual<int>(2, r1.Count, "Returned population has wrong size. Expected: 2, actual: {0}.", r1.Count);
            for (int i = 0; i < ((IList<int>)in1.Chromosome).Count; i++)
            {
                Assert.AreEqual<int>(0, ((IList<int>)r1[0].Chromosome)[i], "Children 1 has wrong chromosome. Expected: 0, Actual: {0}.", ((IList<int>)r1[0].Chromosome)[i]);
                Assert.AreEqual<int>(0, ((IList<int>)r1[1].Chromosome)[i], "Children 2 has wrong chromosome. Expected: 0, Actual: {0}.", ((IList<int>)r1[1].Chromosome)[i]);
            }

            pop[0].Chromosome = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            pop[1].Chromosome = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            r1 = Combine(pop);
            for (int i = 0; i < ((IList<int>)in1.Chromosome).Count; i++)
            {
                Assert.AreEqual<int>(1, ((IList<int>)r1[0].Chromosome)[i], "Children 1 has wrong chromosome. Expected: 0, Actual: {0}.", ((IList<int>)r1[0].Chromosome)[i]);
                Assert.AreEqual<int>(1, ((IList<int>)r1[1].Chromosome)[i], "Children 2 has wrong chromosome. Expected: 0, Actual: {0}.", ((IList<int>)r1[1].Chromosome)[i]);
            }
        }
        
        /// <summary>
        /// It tests if genes are really mixed. 
        /// Population size = 2. One individual has only 0, second one only 1.
        /// In the result both childreen should get mix of 0 and 1 in their chromosomes.
        /// </summary>
        [TestMethod()]
        public void CombineRandomTest()
        {
            BinaryIndividual in1 = new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BinaryIndividual in2 = new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            Population pop = new Population();
            pop.Add(in1);
            pop.Add(in2);

            Population r1 = null;
            int genesSum1 = 0;
            int genesSum2 = 0;
            int backdoor = 1000; //number of attempts to get mixed individual (it is highly possible that the same individual will be taken to make crossover).
            int ii = 0;
            while (genesSum1 == 0 || genesSum1 == 1)
            {
                r1 = Combine(pop);
                genesSum1 = 0;
                genesSum2 = 0;
                for (int i = 0; i < ((IList<int>)in1.Chromosome).Count; i++)
                {
                    genesSum1 += ((IList<int>)r1[0].Chromosome)[i];
                    genesSum2 += ((IList<int>)r1[1].Chromosome)[i];
                }
                if (++ii >= backdoor) { break; }
            }
        
            Assert.AreNotEqual<int>(0, genesSum1, "Children 1 has chromosome with only 0. Genes values should be mixed.");
            Assert.AreNotEqual<int>(0, genesSum2, "Children 2 has chromosome with only 0. Genes values should be mixed.");
            Assert.AreNotEqual<int>(((IList<int>)r1[0].Chromosome).Count, genesSum1, "Children 1 has chromosome with only 1. Genes values should be mixed.");
            Assert.AreNotEqual<int>(((IList<int>)r1[1].Chromosome).Count, genesSum2, "Children 2 has chromosome with only 1. Genes values should be mixed.");
        }


        [TestMethod()]
        public void ApplySizeBelowPopulationTest()
        {
            Population pop = GeneratePopulationForApplyTest();
            int newPopulationSize = pop.Count - (pop.Count / 2);
            UniformCrossover uc = new UniformCrossover(newPopulationSize);
            Population newPop = uc.Apply(pop);
            Assert.AreEqual<int>(newPopulationSize, newPop.Count, "Size for the new population is wrong. Expected: {0}, actual: {1}.", newPopulationSize, newPop.Count);
        }

        [TestMethod()]
        public void ApplyZeroSizeEqualPopulationTest()
        {
            Population pop = GeneratePopulationForApplyTest();
            UniformCrossover uc = new UniformCrossover();
            Population newPop = uc.Apply(pop);
            Assert.AreEqual<int>(pop.Count, newPop.Count, "Size for the new population is wrong. Expected: {0}, actual: {1}.", pop.Count, newPop.Count);
        }

        [TestMethod()]
        public void ApplySizeEqualPopulationTest()
        {
            Population pop = GeneratePopulationForApplyTest();
            UniformCrossover uc = new UniformCrossover(pop.Count);
            Population newPop = uc.Apply(pop);
            Assert.AreEqual<int>(pop.Count, newPop.Count, "Size for the new population is wrong. Expected: {0}, actual: {1}.", pop.Count, newPop.Count);
        }

        [TestMethod()]
        public void ApplySizeGreaterPopulationTest()
        {
            Population pop = GeneratePopulationForApplyTest();
            int newPopulationSize = pop.Count + 1;
            UniformCrossover uc = new UniformCrossover(newPopulationSize);
            Population newPop = uc.Apply(pop);
            Assert.AreEqual<int>(newPopulationSize, newPop.Count, "Size for the new population is wrong. Expected: {0}, actual: {1}.", newPopulationSize, newPop.Count);
        }

        private Population GeneratePopulationForApplyTest()
        {
            BinaryIndividual in1 = new BinaryIndividual(new List<int>() { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0 });
            BinaryIndividual in2 = new BinaryIndividual(new List<int>() { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 });
            BinaryIndividual in3 = new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0 });
            BinaryIndividual in4 = new BinaryIndividual(new List<int>() { 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0 });
            BinaryIndividual in5 = new BinaryIndividual(new List<int>() { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0 });
            BinaryIndividual in6 = new BinaryIndividual(new List<int>() { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0 });
            BinaryIndividual in7 = new BinaryIndividual(new List<int>() { 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 1 });
            BinaryIndividual in8 = new BinaryIndividual(new List<int>() { 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 });
            BinaryIndividual in9 = new BinaryIndividual(new List<int>() { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0 });
            Population pop = new Population();
            pop.Add(in1);
            pop.Add(in2);
            pop.Add(in3);
            pop.Add(in4);
            pop.Add(in5);
            pop.Add(in6);
            pop.Add(in7);
            pop.Add(in8);
            pop.Add(in9);
            return pop;
        }
    }
}
