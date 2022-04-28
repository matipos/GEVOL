using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.evoperator.eda.binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.evoperator.eda.model;
namespace Gevol.evolution.evoperator.eda.binary.Tests
{
    [TestClass()]
    public class UMDAOperatorTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UMDAOperatorTest()
        {
            UMDAOperator op = new UMDAOperator(0);
        }

        /// <summary>
        /// Population has individuals with genes equal to 1 only.
        /// New population should be the same.
        /// </summary>
        [TestMethod()]
        public void ApplyGenerateOnesTest()
        {
            UMDAOperator op = new UMDAOperator(5);
            Population pop = new Population();
            pop.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }));

            Population newPop = op.Apply(pop);

            //test model
            UMDAModel model = (UMDAModel)op.Model;
            Assert.AreEqual<int>(((IList<int>)pop[0].Chromosome).Count, model.probabilities.Count, "Model size is not correct. Expected: {0}, actual: {1}", ((IList<int>)pop[0].Chromosome).Count, model.probabilities.Count);
            for (int i = 0; i < model.probabilities.Count; i++)
            {
                Assert.AreEqual<double>(1, model.probabilities[i], "Probability for gene {0} is wrong. Expected: 1, actual {1}.", i, model.probabilities[i]);
            }

            //test population
            foreach (BinaryIndividual individual in newPop)
            {
                IList<int> chromosome = (IList<int>)individual.Chromosome;
                Assert.AreEqual<int>(((IList<int>)pop[0].Chromosome).Count, ((IList<int>)newPop[0].Chromosome).Count, "In new population, chromosome has wrong size. Expected: {0}, actual: {1}", ((IList<int>)pop[0].Chromosome).Count, ((IList<int>)newPop[0].Chromosome).Count);
                for(int i = 0; i < chromosome.Count; i++)
                {
                    Assert.AreEqual<int>(1, chromosome[i], "Gene at position {0} has wrong value. Expected: 1, actual: {1}.", i, chromosome[i]);
                }
            }
        }

        /// <summary>
        /// Population has individuals with genes equal to 0 only.
        /// New population should be the same.
        /// </summary>
        [TestMethod()]
        public void ApplyGenerateZerosTest()
        {
            UMDAOperator op = new UMDAOperator(5);
            Population pop = new Population();
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            Population newPop = op.Apply(pop);

            //test model
            UMDAModel model = (UMDAModel)op.Model;
            Assert.AreEqual<int>(((IList<int>)pop[0].Chromosome).Count, model.probabilities.Count, "Model size is not correct. Expected: {0}, actual: {1}", ((IList<int>)pop[0].Chromosome).Count, model.probabilities.Count);
            for (int i = 0; i < model.probabilities.Count; i++)
            {
                Assert.AreEqual<double>(0, model.probabilities[i], "Probability for gene {0} is wrong. Expected: 0, actual {1}.", i, model.probabilities[i]);
            }

            //test population
            foreach (BinaryIndividual individual in newPop)
            {
                IList<int> chromosome = (IList<int>)individual.Chromosome;
                Assert.AreEqual<int>(((IList<int>)pop[0].Chromosome).Count, ((IList<int>)newPop[0].Chromosome).Count, "In new population, chromosome has wrong size. Expected: {0}, actual: {1}", ((IList<int>)pop[0].Chromosome).Count, ((IList<int>)newPop[0].Chromosome).Count);
                for (int i = 0; i < chromosome.Count; i++)
                {
                    Assert.AreEqual<int>(0, chromosome[i], "Gene at position {0} has wrong value. Expected: 0, actual: {1}.", i, chromosome[i]);
                }
            }
        }

        [TestMethod()]
        public void ApplyGenerateDifferentTest()
        {
            Population pop = new Population();
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 0, 1, 0, 0, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 1, 0, 0, 1, 1, 0, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 1, 0, 0, 1, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 0, 0, 0, 0, 1, 0, 1 }));
            pop.Add(new BinaryIndividual(new List<int>() { 1, 0, 0, 0, 1, 0, 1 }));
            UMDAOperator op = new UMDAOperator(pop.Count);
            List<double> refProb = new List<double>(pop.Count);
            refProb.Add((double)2 / (double)pop.Count);
            refProb.Add((double)0 / (double)pop.Count);
            refProb.Add((double)1 / (double)pop.Count);
            refProb.Add((double)2 / (double)pop.Count);
            refProb.Add((double)3 / (double)pop.Count);
            refProb.Add((double)1 / (double)pop.Count);
            refProb.Add((double)5 / (double)pop.Count);

            Population newPop = op.Apply(pop);

            //test model
            UMDAModel model = (UMDAModel)op.Model;
            for (int i = 0; i < refProb.Count; i++)
            {
                Assert.AreEqual<double>(refProb[i], model.probabilities[i], "Calculated probability is wrong for gene {0}. Expected value: {1}, acutal: {2}.", i, refProb[i], model.probabilities[i]);
            }

            //just print new population
            Console.WriteLine("UMDAOperatorTest.ApplyGenerateDifferentTest: generated new population:");
            foreach (BinaryIndividual individual in newPop)
            {
                Console.WriteLine(individual.ToString());
            }
            Console.WriteLine("model:");
            Console.WriteLine(op.Model.ToString());
        }
    }
}
