using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual.real;
using Gevol.evolution.util;

namespace Gevol.evolution.evoperator.real.Tests
{
    [TestClass()]
    public class RealLocalIntermediaryRecombinationTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplyExceptionTest()
        {
            RealLocalIntermediaryRecombination rs = new RealLocalIntermediaryRecombination();
            BinaryIndividual indv = new BinaryIndividual();
            Population pop = new Population();
            pop.Add(indv);
            pop.Add(indv);
            rs.Apply(pop);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplyExceptionSizeTest()
        {
            RealLocalIntermediaryRecombination rs = new RealLocalIntermediaryRecombination();
            RealIndividual indv = new RealIndividual();
            Population pop = new Population();
            pop.Add(indv);
            rs.Apply(pop);
        }

        [TestMethod()]
        public void ApplyTest()
        {
            Population pop = new Population();
            int valLength = 4; int alphaLength = 6;
            RealIndividualChromosome chr1 = new RealIndividualChromosome(valLength, alphaLength);
            RealIndividualChromosome chr2 = new RealIndividualChromosome(valLength, alphaLength);
            for (int j = 0; j < valLength; j++)
            {
                chr1.Sigma.Add(Randomizer.NextDouble(0, 10));
                chr1.Values.Add(Randomizer.NextDouble(0, 10));
                chr2.Sigma.Add(chr1.Sigma[j]);
                chr2.Values.Add(chr1.Values[j]);
            }
            for (int j = 0; j < alphaLength; j++)
            {
                chr1.Alpha.Add(Randomizer.NextDouble(0, 10));
                chr2.Alpha.Add(chr1.Alpha[j]);
            }
            RealIndividual indv1 = new RealIndividual(chr1);
            RealIndividual indv2 = new RealIndividual(chr2);
            pop.Add(indv1);
            pop.Add(indv2);

            RealLocalIntermediaryRecombination rs = new RealLocalIntermediaryRecombination();
            Population newPop = rs.Apply(pop);

            Assert.AreEqual<int>(1, newPop.Count, "Returned population has wrong size. Expected: 1, actual {0}.", newPop.Count);
            RealIndividualChromosome newChromosome = (RealIndividualChromosome)(newPop[0].Chromosome);
            Assert.AreEqual<int>(0, newChromosome.Age, "Age for new individual is incorrect. Expected: 1, actual {0}.", newChromosome.Age);
            Assert.AreEqual<int>(valLength, newChromosome.Sigma.Count, "Sigma chromosome has wrong length. Expected: {0}, actual: {1}.", valLength, newChromosome.Sigma.Count);
            Assert.AreEqual<int>(valLength, newChromosome.Values.Count, "Values chromosome has wrong length. Expected: {0}, actual: {1}.", valLength, newChromosome.Values.Count);
            Assert.AreEqual<int>(alphaLength, newChromosome.Alpha.Count, "Alpha chromosome has wrong length. Expected: {0}, actual: {1}.", alphaLength, newChromosome.Alpha.Count);

            //if both individuals have the same values then the child will be also the same
            Assert.AreEqual<double>(Math.Round(chr1.Sigma[0], 5), Math.Round(newChromosome.Sigma[0], 5), "New chromosome has been modified.");
            Assert.AreEqual<double>(Math.Round(chr1.Values[0], 5), Math.Round(newChromosome.Values[0], 5), "New chromosome has been modified.");
            Assert.AreEqual<double>(Math.Round(chr1.Alpha[0], 5), Math.Round(newChromosome.Alpha[0], 5), "New chromosome has been modified.");
        }
    }
}