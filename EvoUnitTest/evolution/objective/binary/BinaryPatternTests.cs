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
    public class BinaryPatternTests
    {
        /// <summary>
        /// Check if the scoring is proper.
        /// </summary>
        [TestMethod()]
        public void EvaluateTest()
        {
            IList<int> chromosome = new List<int>() { 1, 0, 1, 1, 1, 0, 0, 0, 1 };
            BinaryIndividual individual = new BinaryIndividual(new List<int>(chromosome));
            BinaryPattern objFun = new BinaryPattern(new List<int>(chromosome));

            double scoreFull = objFun.Evaluate(individual);
            Assert.AreEqual<double>(-chromosome.Count, scoreFull, "Individual didn't get full score. Score: {0}, expected: {1}.", scoreFull, -chromosome.Count);

            objFun.Pattern = new List<int>() { 0, 1, 0, 0, 0, 1, 1, 1, 0 };
            double scoreNone = objFun.Evaluate(individual);
            Assert.AreEqual<double>(0, scoreNone, "Individual didn't get correct score. Score: {0}, expected: 0.", scoreNone);

            objFun.Pattern = new List<int>() { 1, 1, 0, 1, 0, 1, 0, 0, 0 };
            double scoreFour = objFun.Evaluate(individual);
            Assert.AreEqual<double>(-4, scoreFour, "Individual didn't get correct score. Score: {0}, expected: -4.", scoreFour);
        }
    }
}
