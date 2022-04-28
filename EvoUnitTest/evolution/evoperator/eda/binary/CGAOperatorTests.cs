using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.eda.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.evoperator.eda.model;

namespace Gevol.evolution.evoperator.eda.binary.Tests
{
    [TestClass()]
    public class CGAOperatorTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CGAOperatorWrongSizeTest()
        {
            CGAOperator cga = new CGAOperator(0, 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ApplyWrongPopulationTest1()
        {
            Population population = new Population();
            population.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 1 }));
            CGAOperator cga = new CGAOperator(250, 4);
            cga.Apply(population);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ApplyWrongPopulationTest2()
        {
            Population population = new Population();
            population.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 1 }));
            population.Add(new BinaryIndividual(new List<int>() { 1, 1, 0, 1 }));
            population.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 0 }));
            CGAOperator cga = new CGAOperator(250, 4);
            cga.Apply(population);
        }

        [TestMethod()]
        public void ApplyTest()
        {
            Population population = new Population();
            population.Add(new BinaryIndividual(new List<int>() { 0, 0, 1, 1 }) { Score = new List<double>() { 32 } });
            population.Add(new BinaryIndividual(new List<int>() { 1, 0, 0, 1 }) { Score = new List<double>() { 2 } });  //better individual
            CGAOperator cga = new CGAOperator(250, 4);
            double stepSize = 1.0 / 250.0;  //for veryfication

            Population newPopulation = cga.Apply(population);
            CGAModel model = (CGAModel) cga.Model;

            Assert.AreEqual<int>(2, newPopulation.Count(), "New population has wrong size.");

            for (int i = 0; i < model.probabilities.Count; i++)
                Console.WriteLine(i + ": " + model.probabilities[i]);

            Assert.AreEqual<double>(0.5 + stepSize, model.probabilities[0], "New probability for gene 0 is wrong");
            Assert.AreEqual<double>(0.5, model.probabilities[1], "New probability for gene 1 is wrong");
            Assert.AreEqual<double>(0.5 - stepSize, model.probabilities[2], "New probability for gene 2 is wrong");
            Assert.AreEqual<double>(0.5, model.probabilities[3], "New probability for gene 3 is wrong");
        }
    }
}