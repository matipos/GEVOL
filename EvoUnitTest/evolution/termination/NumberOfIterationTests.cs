using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.termination;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Gevol.evolution.termination.Tests
{
    /// <summary>
    /// Check if condition is satisfied after particual number of iterations.
    /// </summary>
    [TestClass()]
    public class NumberOfIterationTests
    {
        [TestMethod()]
        public void isSatisfiedTest()
        {
            int condition = 0;
            int numberOfIteartions = 0;
            NumberOfIterations terminationCondition = new NumberOfIterations(condition);
            while (!terminationCondition.isSatisfied(null))
            {
                numberOfIteartions++;
            }
            Assert.AreEqual<int>(condition, numberOfIteartions, "Number of executed iterations is wrong. Expected: {0}, actual: {1}", condition, numberOfIteartions);

            condition = 86;
            numberOfIteartions = 0;
            terminationCondition.Iterations = condition;
            terminationCondition.reset();
            while (!terminationCondition.isSatisfied(null))
            {
                numberOfIteartions++;
            }
            Assert.AreEqual<int>(condition, numberOfIteartions, "Number of executed iterations is wrong. Expected: {0}, actual: {1}", condition, numberOfIteartions);

            condition = 1;
            numberOfIteartions = 0;
            terminationCondition.Iterations = condition;
            terminationCondition.reset();
            while (!terminationCondition.isSatisfied(null))
            {
                numberOfIteartions++;
            }
            Assert.AreEqual<int>(condition, numberOfIteartions, "Number of executed iterations is wrong. Expected: {0}, actual: {1}", condition, numberOfIteartions);
        }
    }
}
