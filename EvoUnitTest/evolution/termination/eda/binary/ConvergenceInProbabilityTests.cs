using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.termination.eda.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.evoperator.eda.model;

namespace Gevol.evolution.termination.eda.binary.Tests
{
    [TestClass()]
    public class ConvergenceInProbabilityTests
    {
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void isSatisfiedTest()
        {
            Population population = new Population();
            ConvergenceInProbability convergence = new ConvergenceInProbability(1, 2);
            convergence.isSatisfied(population);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void isSatisfiedWrongModelTest()
        {
            Population population = new Population();
            IList<double> list = new List<double>();
            ConvergenceInProbability convergence = new ConvergenceInProbability(1, 2);
            convergence.isSatisfied(population, list);
        }

        [TestMethod()]
        public void isSatisfiedCorrectModelTest()
        {
            Population population = new Population();
            UMDAModel umdaModel = new UMDAModel() { probabilities = new List<double>() { 0.5 } };
            PBILModel pbilModel = new PBILModel() { probabilities = new List<double>() { 0.5 } };

            ConvergenceInProbability convergence = new ConvergenceInProbability(1, 2);
            convergence.isSatisfied(population, umdaModel);
            convergence.isSatisfied(population, pbilModel);
        }

        [TestMethod()]
        public void isSatisfiedEdaTest()
        {
            Population population = new Population();
            UMDAModel umdaModel1 = new UMDAModel() { probabilities = new List<double>() { 0.5, 0.5, 0.2 } };
            UMDAModel umdaModel2 = new UMDAModel() { probabilities = new List<double>() { 0.1, 0.5, 1 } };
            UMDAModel umdaModel3 = new UMDAModel() { probabilities = new List<double>() { 0.1, 1, 0.1 } };
            UMDAModel umdaModel4 = new UMDAModel() { probabilities = new List<double>() { 1.5, -0.5, -0.2 } };
            UMDAModel umdaModel5 = new UMDAModel() { probabilities = new List<double>() { 0.12, -0.5, 1.2 } };

            ConvergenceInProbability convergence = new ConvergenceInProbability(0.1, 1);
            Assert.AreEqual<bool>(false, convergence.isSatisfied(population, umdaModel1), "Wrong result for test 1");
            Assert.AreEqual<bool>(false, convergence.isSatisfied(population, umdaModel2), "Wrong result for test 2");
            Assert.AreEqual<bool>(true, convergence.isSatisfied(population, umdaModel3), "Wrong result for test 3");
            Assert.AreEqual<bool>(true, convergence.isSatisfied(population, umdaModel4), "Wrong result for test 4");
            Assert.AreEqual<bool>(false, convergence.isSatisfied(population, umdaModel5), "Wrong result for test 4");
        }
    }
}