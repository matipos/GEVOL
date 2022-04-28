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

namespace Gevol.evolution.evoperator.real.Tests
{
    [TestClass()]
    public class RealUniformCrossoverTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplyExceptionTest()
        {
            RealUniformCrossover rs = new RealUniformCrossover();
            BinaryIndividual indv = new BinaryIndividual();
            Population pop = new Population();
            pop.Add(indv);
            pop.Add(indv);
            rs.Apply(pop);
        }

        [TestMethod()]
        public void ApplyTest()
        {
            RealIndividualProperties indvprop = new RealIndividualProperties() { ChromosomeLength = 8 };
            Population pop = new Population();
            double populationSize = 16;
            for (double i = 0; i < populationSize; i++)
            {
                RealIndividualChromosome chromosome = new RealIndividualChromosome(indvprop.ChromosomeLength, indvprop.AlphaLength);
                for (int k = 0; k < indvprop.ChromosomeLength; k++)
                {
                    chromosome.Sigma.Add(i);
                    chromosome.Values.Add(i);
                }
                for (int k = 0; k < indvprop.AlphaLength; k++)
                {
                    chromosome.Alpha.Add(i);
                }
                RealIndividual indv1 = new RealIndividual(chromosome);
                pop.Add(indv1);
            }

            RealUniformCrossover rs = new RealUniformCrossover();
            Population newPop = rs.Apply(pop);

            Assert.AreEqual<int>(1, newPop.Count, "Returned population has wrong size. Expected: 1, actual {0}.", newPop.Count);
            RealIndividualChromosome newChromosome = (RealIndividualChromosome)(newPop[0].Chromosome);
            Assert.AreEqual<int>(0, newChromosome.Age, "Age for new individual is incorrect. Expected: 1, actual {0}.", newChromosome.Age);
            Assert.AreEqual<int>(indvprop.ChromosomeLength, newChromosome.Sigma.Count, "Sigma chromosome has wrong length. Expected: {0}, actual: {1}.", indvprop.ChromosomeLength, newChromosome.Sigma.Count);
            Assert.AreEqual<int>(indvprop.ChromosomeLength, newChromosome.Values.Count, "Values chromosome has wrong length. Expected: {0}, actual: {1}.", indvprop.ChromosomeLength, newChromosome.Values.Count);
            Assert.AreEqual<int>(indvprop.AlphaLength, newChromosome.Alpha.Count, "Alpha chromosome has wrong length. Expected: {0}, actual: {1}.", indvprop.AlphaLength, newChromosome.Alpha.Count);

            Assert.AreNotEqual<double>(newChromosome.Sigma[0], newChromosome.Sigma.Average(), "Sigma chromosome has not random values.");
            Assert.AreNotEqual<double>(newChromosome.Values[0], newChromosome.Values.Average(), "Values chromosome has not random values.");
            Assert.AreNotEqual<double>(newChromosome.Alpha[0], newChromosome.Alpha.Average(), "Alpha chromosome has not random values.");
        }
    }
}