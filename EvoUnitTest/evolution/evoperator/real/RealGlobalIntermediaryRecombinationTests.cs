using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual;
using Gevol.evolution.individual.real;
using Gevol.evolution.util;

namespace Gevol.evolution.evoperator.real.Tests
{
    [TestClass()]
    public class RealGlobalIntermediaryRecombinationTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplyExceptionTest()
        {
            RealGlobalIntermediaryRecombination rs = new RealGlobalIntermediaryRecombination();
            BinaryIndividual indv = new BinaryIndividual();
            Population pop = new Population();
            pop.Add(indv);
            pop.Add(indv);
            rs.Apply(pop);
        }

        [TestMethod()]
        public void ApplyTest()
        {
            Population pop = new Population();
            int valLength = 4; int alphaLength = 6;
            for(int i = 0; i < 4; i++)
            {
                RealIndividualChromosome chr1 = new RealIndividualChromosome(valLength, alphaLength);
                for(int j = 0; j < valLength; j++)
                {
                    chr1.Sigma.Add(Randomizer.NextDouble(0, 10));
                    chr1.Values.Add(Randomizer.NextDouble(0, 10));
                }
                for (int j = 0; j < alphaLength; j++)
                {
                    chr1.Alpha.Add(Randomizer.NextDouble(0, 10));
                }
                RealIndividual indv = new RealIndividual(chr1);
                pop.Add(indv);
            }
            RealGlobalIntermediaryRecombination op = new RealGlobalIntermediaryRecombination();
            Population newPop = op.Apply(pop);

            Assert.AreEqual<int>(1, newPop.Count, "Returned population has wrong size. Expected: 1, actual {0}.", newPop.Count);
            RealIndividualChromosome newChromosome = (RealIndividualChromosome)(newPop[0].Chromosome);
            Assert.AreEqual<int>(0, newChromosome.Age, "Age for new individual is incorrect. Expected: 1, actual {0}.", newChromosome.Age);
            Assert.AreEqual<int>(valLength, newChromosome.Sigma.Count, "Sigma chromosome has wrong length. Expected: {0}, actual: {1}.", valLength, newChromosome.Sigma.Count);
            Assert.AreEqual<int>(valLength, newChromosome.Values.Count, "Values chromosome has wrong length. Expected: {0}, actual: {1}.", valLength, newChromosome.Values.Count);
            Assert.AreEqual<int>(alphaLength, newChromosome.Alpha.Count, "Alpha chromosome has wrong length. Expected: {0}, actual: {1}.", alphaLength, newChromosome.Alpha.Count);

            //check values
            for(int i = 0; i < valLength; i++)
            {
                IList<double> avgValues = new List<double>();
                IList<double> avgSigma = new List<double>();
                for (int k = 0; k < pop.Count; k++)
                {
                    avgValues.Add(((RealIndividualChromosome)pop[k].Chromosome).Values[i]);
                    avgSigma.Add(((RealIndividualChromosome)pop[k].Chromosome).Sigma[i]);
                }
                Assert.AreEqual<double>(avgValues.Average(), newChromosome.Values[i], "Value for values[{0}] is wrong. Expected: {1}, actual: {2}.", i, avgValues.Average(), newChromosome.Values[i]);
                Assert.AreEqual<double>(avgSigma.Average(), newChromosome.Sigma[i], "Value for sigma[{0}] is wrong. Expected: {1}, actual: {2}.", i, avgSigma.Average(), newChromosome.Sigma[i]);
            }
            for (int i = 0; i < alphaLength; i++)
            {
                IList<double> avgAlpha = new List<double>();
                for (int k = 0; k < pop.Count; k++)
                {
                    avgAlpha.Add(((RealIndividualChromosome)pop[k].Chromosome).Alpha[i]);
                }
                Assert.AreEqual<double>(avgAlpha.Average(), newChromosome.Alpha[i], "Value for alpha[{0}] is wrong. Expected: {1}, actual: {2}.", i, avgAlpha.Average(), newChromosome.Alpha[i]);
            }
        }

        /// <summary>
        /// Test average method.
        /// </summary>
        [TestMethod()]
        public void AverageTest()
        {
            IList<double> list = new List<double>();
            list.Add(5); list.Add(6); list.Add(9); list.Add(15);
            Assert.AreEqual<double>(8.75, list.Average(), "Average function returned {0} instead of 8.75.", list.Average());
        }
    }
}