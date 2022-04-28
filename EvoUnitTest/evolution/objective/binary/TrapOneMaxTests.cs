using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.objective.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual.real;

namespace Gevol.evolution.objective.binary.Tests
{
    [TestClass()]
    public class TrapOneMaxTests
    {
        [TestMethod()]
        public void EvaluateTest()
        {
            Individual indv1 = new BinaryIndividual(new List<int>() { 1, 1, 0, 0, 1, 0, 1, 0 });
            Individual indv2 = new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 1, 0 });
            Individual indv3 = new BinaryIndividual(new List<int>() { 1, 1, 0, 1, 1, 1, 0 });
            Individual indv4 = new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Individual indv5 = new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1, 1 });

            TrapOneMax trap = new TrapOneMax();

            Assert.AreEqual<double>(-4, trap.Evaluate(indv1), "Individual 1 has wrong trap score.");
            Assert.AreEqual<double>(-1, trap.Evaluate(indv2), "Individual 2 has wrong trap score.");
            Assert.AreEqual<double>(-5, trap.Evaluate(indv3), "Individual 3 has wrong trap score.");
            Assert.AreEqual<double>(-11, trap.Evaluate(indv4), "Individual 4 has wrong trap score.");
            Assert.AreEqual<double>(-6, trap.Evaluate(indv5), "Individual 5 has wrong trap score.");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EvaluateExceptionArgumentTest()
        {
            RealIndividual indv = new RealIndividual();
            TrapOneMax trap = new TrapOneMax();
            trap.Evaluate(indv);
        }
    }
}