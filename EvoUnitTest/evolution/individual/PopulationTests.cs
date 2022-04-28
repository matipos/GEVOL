using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.individual.binary;
namespace Gevol.evolution.individual.Tests
{
    [TestClass()]
    public class PopulationTests
    {
        /// <summary>
        /// Check if sorting is correct.
        /// Test is based on the BinaryIndividual.
        /// </summary>
        [TestMethod()]
        public void SortTest()
        {
            Population population = new Population();
            BinaryIndividual bin1 = new BinaryIndividual() { Score = new List<double>() { 5 } };
            BinaryIndividual bin2 = new BinaryIndividual() { Score = new List<double>() { 2 } };
            BinaryIndividual bin3 = new BinaryIndividual() { Score = new List<double>() { 8 } };
            BinaryIndividual bin4 = new BinaryIndividual() { Score = new List<double>() { 9 } };
            BinaryIndividual bin5 = new BinaryIndividual() { Score = new List<double>() { 4 } };

            population.Add(bin1);
            population.Add(bin2);
            population.Add(bin3);
            population.Add(bin4);
            population.Add(bin5);

            //check order before sorting
            Assert.AreEqual<double>(5, population[0].Score[0]);
            Assert.AreEqual<double>(2, population[1].Score[0]);
            Assert.AreEqual<double>(8, population[2].Score[0]);
            Assert.AreEqual<double>(9, population[3].Score[0]);
            Assert.AreEqual<double>(4, population[4].Score[0]);

            population.Sort();

            //check order after sorting
            Assert.AreEqual<double>(2, population[0].Score[0]);
            Assert.AreEqual<double>(4, population[1].Score[0]);
            Assert.AreEqual<double>(5, population[2].Score[0]);
            Assert.AreEqual<double>(8, population[3].Score[0]);
            Assert.AreEqual<double>(9, population[4].Score[0]);
        }
    }
}
