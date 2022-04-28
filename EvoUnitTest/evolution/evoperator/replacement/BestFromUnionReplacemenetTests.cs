using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.evoperator.replacement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
namespace Gevol.evolution.evoperator.replacement.Tests
{
    [TestClass()]
    public class BestFromUnionReplacemenetTests
    {
        [TestMethod()]
        public void ApplyTest()
        {
            Individual in1p1 = new BinaryIndividual() { Score = new List<double>() { 5 } };
            Individual in2p1 = new BinaryIndividual() { Score = new List<double>() { 34 } };
            Individual in3p1 = new BinaryIndividual() { Score = new List<double>() { 1 } };
            Individual in4p1 = new BinaryIndividual() { Score = new List<double>() { 0 } };
            Individual in5p1 = new BinaryIndividual() { Score = new List<double>() { 77 } };
            Individual in6p1 = new BinaryIndividual() { Score = new List<double>() { 2 } };

            Individual in1p2 = new BinaryIndividual() { Score = new List<double>() { 9 } };
            Individual in2p2 = new BinaryIndividual() { Score = new List<double>() { 127 } };
            Individual in3p2 = new BinaryIndividual() { Score = new List<double>() { 15 } };
            Individual in4p2 = new BinaryIndividual() { Score = new List<double>() { 33 } };
            Individual in5p2 = new BinaryIndividual() { Score = new List<double>() { 3 } };
            Individual in6p2 = new BinaryIndividual() { Score = new List<double>() { 1 } };

            Population pop1 = new Population();
            Population pop2 = new Population();

            pop1.Add(in1p1);
            pop1.Add(in2p1);
            pop1.Add(in3p1);
            pop1.Add(in4p1);
            pop1.Add(in5p1);
            pop1.Add(in6p1);

            pop2.Add(in1p2);
            pop2.Add(in2p2);
            pop2.Add(in3p2);
            pop2.Add(in4p2);
            pop2.Add(in5p2);
            pop2.Add(in6p2);

            BestFromUnionReplacement replacement = new BestFromUnionReplacement();
            Population newPop = replacement.Apply(pop1, pop2);

            Assert.AreEqual<int>(6, newPop.Count, "Returned population has wrong size. Expected: 6, actual: {0}.", newPop.Count);

            Assert.AreEqual<double>(0, newPop[0].Score[0], "Wrong individual has been choosen at position 0. Expected individual score: 0, actual: {0}.", newPop[0].Score[0]);
            Assert.AreEqual<double>(1, newPop[1].Score[0], "Wrong individual has been choosen at position 1. Expected individual score: 1, actual: {0}.", newPop[1].Score[0]);
            Assert.AreEqual<double>(1, newPop[2].Score[0], "Wrong individual has been choosen at position 2. Expected individual score: 1, actual: {0}.", newPop[2].Score[0]);
            Assert.AreEqual<double>(2, newPop[3].Score[0], "Wrong individual has been choosen at position 3. Expected individual score: 2, actual: {0}.", newPop[3].Score[0]);
            Assert.AreEqual<double>(3, newPop[4].Score[0], "Wrong individual has been choosen at position 4. Expected individual score: 3, actual: {0}.", newPop[4].Score[0]);
            Assert.AreEqual<double>(5, newPop[5].Score[0], "Wrong individual has been choosen at position 5. Expected individual score: 5, actual: {0}.", newPop[5].Score[0]);
        }
    }
}
