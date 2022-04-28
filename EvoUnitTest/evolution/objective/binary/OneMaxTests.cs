using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.objective.binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.individual.binary;
namespace Gevol.evolution.objective.binary.Tests
{
    [TestClass()]
    public class OneMaxTests
    {
        [TestMethod()]
        public void EvaluateTest()
        {
            BinaryIndividual individual = new BinaryIndividual(new List<int>() { 1, 0, 1, 1, 1, 0, 0, 0, 1 });
            OneMax objFun = new OneMax();

            double scoreFive = objFun.Evaluate(individual);
            Assert.AreEqual<double>(-5, scoreFive, "Individual didn't get correct score. Score: {0}, expected: -5.", scoreFive);

            individual.Chromosome = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double scoreZero = objFun.Evaluate(individual);
            Assert.AreEqual<double>(0, scoreZero, "Individual didn't get correct score. Score: {0}, expected: 0.", scoreZero);

            individual.Chromosome = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            double scoreFull = objFun.Evaluate(individual);
            Assert.AreEqual<double>(-((IList<int>)individual.Chromosome).Count, scoreFull, "Individual didn't get full score. Score: {0}, expected: {1}.", scoreFull, -((IList<int>)individual.Chromosome).Count);
        }
    }
}
