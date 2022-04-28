using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.objective.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual.real;
using Gevol.evolution.individual.binary;

namespace Gevol.evolution.objective.binary.Tests
{
    [TestClass()]
    public class ComposedTrapOneMaxTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EvaluateWrongIndividualTest()
        {
            RealIndividual individual = new RealIndividual();
            ComposedTrapOneMax trap = new ComposedTrapOneMax(2, 2, 2, 2);
            trap.Evaluate(individual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EvaluateWrongChromosomeLengthTest()
        {
            BinaryIndividual individual = new BinaryIndividual(new List<int>() { 1,2,3,4,5,6,7 });  //wrong length regarding block length and number of blocks
            ComposedTrapOneMax trap = new ComposedTrapOneMax(3, 4, 2, 1);   //chromosome length should be 3*4 = 12, not 8
            trap.Evaluate(individual);
        }

        [TestMethod()]
        public void EvaluateOneBlockTest()
        {
            BinaryIndividual individual = new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1 });
            ComposedTrapOneMax trap = new ComposedTrapOneMax(1, 5, 5, 4);
            double result = trap.Evaluate(individual);
            Assert.AreEqual<double>(-5, result, "Calculation for [11111] Fhigh = 5, Flow = 4 is wrong.");

            individual.Chromosome = new List<int>() { 1, 1, 0, 1, 1 };
            result = trap.Evaluate(individual);
            Assert.AreEqual<double>(0, result, "Calculation for [11011] Fhigh = 5, Flow = 4 is wrong.");
            
            individual.Chromosome = new List<int>() { 0, 0, 0, 0, 0 };
            result = trap.Evaluate(individual);
            Assert.AreEqual<double>(-4, result, "Calculation for [00000] Fhigh = 5, Flow = 4 is wrong.");
            
            individual.Chromosome = new List<int>() { 0, 1, 0, 0, 0 };
            trap.Flow = 2.5;
            result = trap.Evaluate(individual);
            Assert.AreEqual<double>(-1.875, result, "Calculation for [01000] Fhigh = 5, Flow = 2.5 is wrong.");
            
            individual.Chromosome = new List<int>() { 1, 1, 1, 1, 1 };
            trap.Fhigh = 3.5;
            result = trap.Evaluate(individual);
            Assert.AreEqual<double>(-3.5, result, "Calculation for [11111] Fhigh = 3.5, Flow = 2.5 is wrong.");
        }

        [TestMethod()]
        public void EvaluateMultipleBlocksTest()
        {
            BinaryIndividual individual = new BinaryIndividual(new List<int>() { 1, 1, 1,    1, 1, 0,    0, 0, 0 });
            ComposedTrapOneMax trap = new ComposedTrapOneMax(3, 3, 5, 4);
            double result = trap.Evaluate(individual);
            Assert.AreEqual<double>(-5 -0 -4, result, "Calculation for [111 110 000] Fhigh = 5, Flow = 4 is wrong.");

            individual.Chromosome = new List<int>() { 0, 1, 0,   0, 0, 0,  1, 1, 0 };
            trap.Fhigh = 3;
            trap.Flow = 2.5;
            result = trap.Evaluate(individual);
            Assert.AreEqual<double>(-1.25 -2.5 -0, result, "Calculation for [010 000 110] Fhigh = 3, Flow = 2.5 is wrong.");
        }
    }
}