using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.evoperator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.evoperator.selection;
namespace Gevol.evolution.evoperator.selection.Tests
{
    [TestClass()]
    public class BlockSelectionTests
    {
        /// <summary>
        /// Test block selection in normal use.
        /// BinaryIndividual is used for the test.
        /// </summary>
        [TestMethod()]
        public void ApplyTest()
        {
            Population pop1 = generatePopulation();

            BlockSelection blockSelection = new BlockSelection(3);
            Population newPop = blockSelection.Apply(pop1);

            Assert.AreEqual<int>(3, newPop.Count, "Returned population has wrong size. Expected: 3, actual: {0}.", newPop.Count);

            Assert.AreEqual<double>(0, newPop[0].Score[0], "Wrong individual has been choosen at position 0. Expected individual score: 0, actual: {0}.", newPop[0].Score[0]);
            Assert.AreEqual<double>(1, newPop[1].Score[0], "Wrong individual has been choosen at position 1. Expected individual score: 1, actual: {0}.", newPop[1].Score[0]);
            Assert.AreEqual<double>(4, newPop[2].Score[0], "Wrong individual has been choosen at position 2. Expected individual score: 4, actual: {0}.", newPop[2].Score[0]);
        }

        /// <summary>
        /// Test block selection when Size is equal to population size.
        /// BinaryIndividual is used for the test.
        /// </summary>
        [TestMethod()]
        public void ApplySizeEqualToPopulationTest()
        {
            Population pop1 = generatePopulation();

            BlockSelection blockSelection = new BlockSelection(pop1.Count);
            Population newPop = blockSelection.Apply(pop1);

            Assert.AreEqual<int>(pop1.Count, newPop.Count, "Returned population has wrong size. Expected: {1}, actual: {0}.", newPop.Count, pop1.Count);

            Assert.AreEqual<double>(0, newPop[0].Score[0], "Wrong individual has been choosen at position 0. Expected individual score: 0, actual: {0}.", newPop[0].Score[0]);
            Assert.AreEqual<double>(1, newPop[1].Score[0], "Wrong individual has been choosen at position 1. Expected individual score: 1, actual: {0}.", newPop[1].Score[0]);
            Assert.AreEqual<double>(4, newPop[2].Score[0], "Wrong individual has been choosen at position 2. Expected individual score: 4, actual: {0}.", newPop[2].Score[0]);
            Assert.AreEqual<double>(5, newPop[3].Score[0], "Wrong individual has been choosen at position 3. Expected individual score: 5, actual: {0}.", newPop[3].Score[0]);
            Assert.AreEqual<double>(34, newPop[4].Score[0], "Wrong individual has been choosen at position 4. Expected individual score: 34, actual: {0}.", newPop[4].Score[0]);
            Assert.AreEqual<double>(77, newPop[5].Score[0], "Wrong individual has been choosen at position 5. Expected individual score: 77, actual: {0}.", newPop[5].Score[0]);
        }

        /// <summary>
        /// Test block selection when Size is greater than population size.
        /// BinaryIndividual is used for the test.
        /// </summary>
        [TestMethod()]
        public void ApplySizeGreaterThanPopulationTest()
        {
            Population pop1 = generatePopulation();

            BlockSelection blockSelection = new BlockSelection(pop1.Count + 1);
            Population newPop = blockSelection.Apply(pop1);

            Assert.AreEqual<int>(pop1.Count, newPop.Count, "Returned population has wrong size. Expected: {1}, actual: {0}.", newPop.Count, pop1.Count);

            Assert.AreEqual<double>(0, newPop[0].Score[0], "Wrong individual has been choosen at position 0. Expected individual score: 0, actual: {0}.", newPop[0].Score[0]);
            Assert.AreEqual<double>(1, newPop[1].Score[0], "Wrong individual has been choosen at position 1. Expected individual score: 1, actual: {0}.", newPop[1].Score[0]);
            Assert.AreEqual<double>(4, newPop[2].Score[0], "Wrong individual has been choosen at position 2. Expected individual score: 4, actual: {0}.", newPop[2].Score[0]);
            Assert.AreEqual<double>(5, newPop[3].Score[0], "Wrong individual has been choosen at position 3. Expected individual score: 5, actual: {0}.", newPop[3].Score[0]);
            Assert.AreEqual<double>(34, newPop[4].Score[0], "Wrong individual has been choosen at position 4. Expected individual score: 34, actual: {0}.", newPop[4].Score[0]);
            Assert.AreEqual<double>(77, newPop[5].Score[0], "Wrong individual has been choosen at position 5. Expected individual score: 77, actual: {0}.", newPop[5].Score[0]);
        }
        
        /// <summary>
        /// Test for wrong size argument (Size = 0).
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ApplyIllegalArgumentTest()
        {
            BlockSelection bs = new BlockSelection(0);
        }

        /// <summary>
        /// Test for wrong size argument (Size = -1).
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ApplyIllegalArgument2Test()
        {
            BlockSelection bs = new BlockSelection(-1);
        }

        /// <summary>
        /// Generate population for many tests
        /// </summary>
        /// <returns></returns>
        private Population generatePopulation()
        {
            Individual in1p1 = new BinaryIndividual() { Score = new List<double>() { 5 } };
            Individual in2p1 = new BinaryIndividual() { Score = new List<double>() { 34 } };
            Individual in3p1 = new BinaryIndividual() { Score = new List<double>() { 1 } };
            Individual in4p1 = new BinaryIndividual() { Score = new List<double>() { 0 } };
            Individual in5p1 = new BinaryIndividual() { Score = new List<double>() { 77 } };
            Individual in6p1 = new BinaryIndividual() { Score = new List<double>() { 4 } };
            Population pop1 = new Population();
            pop1.Add(in1p1);
            pop1.Add(in2p1);
            pop1.Add(in3p1);
            pop1.Add(in4p1);
            pop1.Add(in5p1);
            pop1.Add(in6p1);
            return pop1;
        }
    }
}
