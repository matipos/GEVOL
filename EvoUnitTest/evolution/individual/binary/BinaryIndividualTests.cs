using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual.binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Gevol.evolution.individual.binary.Tests
{
    [TestClass()]
    public class BinaryIndividualTests
    {
        /// <summary>
        /// Test if generated binary individual chromosome has correct values.
        /// </summary>
        [TestMethod()]
        public void GenerateIndividualChromosomeTest()
        {
            BinaryIndividualProperties properties = new BinaryIndividualProperties { chromosomeLength = 50 };
            BinaryIndividual individualGenerator = new BinaryIndividual();
            BinaryIndividual binaryIndividual = (BinaryIndividual)individualGenerator.GenerateIndividual(properties);
            //check if every chormosome is in set of {0,1}
            for (int i = 0; i < properties.chromosomeLength; i++)
            {
                if (((IList<int>)binaryIndividual.Chromosome)[i] != 0 && ((IList<int>)binaryIndividual.Chromosome)[i] != 1)
                    Assert.Fail("Generated chromosome for individual is has not a binary values.");
            }
        }

        /// <summary>
        /// Test if input parameters are checked by their type.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerateIndividualArgumentTest()
        {
            String wrongParam = "I'm a wrong type of object";
            BinaryIndividual individualGenerator = new BinaryIndividual();
            BinaryIndividual binaryIndividual = (BinaryIndividual)individualGenerator.GenerateIndividual(wrongParam);
            
        }

        /// <summary>
        /// Test if comparison for scoring is correct.
        /// </summary>
        [TestMethod()]
        public void CompareToTest()
        {
            BinaryIndividual binaryIndividual1 = new BinaryIndividual();
            BinaryIndividual binaryIndividual2 = new BinaryIndividual();
            
            binaryIndividual1.Score = new List<double>() { 2, 5 };
            binaryIndividual2.Score = new List<double>() { 1, 6 };
            Assert.AreEqual<int>(1, binaryIndividual1.CompareTo(binaryIndividual2));
            Assert.AreEqual<int>(-1, binaryIndividual2.CompareTo(binaryIndividual1));
            Assert.AreEqual<int>(0, binaryIndividual1.CompareTo(binaryIndividual1));
        }
    }
}
