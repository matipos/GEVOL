using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.util.Tests
{
    [TestClass()]
    public class RandomizerTests
    {

        [TestMethod()]
        public void NextIndexTest()
        {
            int numberOfIterations = 10000;
            double acceptedError = 0.1;
            int i = 0;
            IList<double> testList = new List<double>() { 0.5, 0, 0.5 };
            int numberOfFirstIndex = 0;
            int numberOfSecondIndex = 0;
            int numberOfThirdIndex = 0;
            while (i++ < numberOfIterations)
            {
                int randIndex = Randomizer.NextIndex(testList);
                switch (randIndex)
                {
                    case 0: numberOfFirstIndex++; break;
                    case 1: numberOfSecondIndex++; break;
                    case 2: numberOfThirdIndex++; break;
                }
            }
            Console.WriteLine("[test 1] number of first index = " + numberOfFirstIndex + "; number of second index = " + numberOfSecondIndex + "; number of third index = " + numberOfThirdIndex);
            Assert.AreEqual<int>(numberOfFirstIndex + numberOfSecondIndex + numberOfThirdIndex, numberOfIterations, "[test 1] Number of selected indexes differs from number of iterations.");
            Assert.AreEqual<int>(0, numberOfSecondIndex, "[test 1] Second index should never be selected.");
            Assert.IsTrue(Math.Abs(numberOfFirstIndex - numberOfThirdIndex) < (int)((double)numberOfIterations * acceptedError), "[test 1] Difference between first and third index is more than {0}, it is {1}.", acceptedError, (double)Math.Abs(numberOfFirstIndex - numberOfThirdIndex) / (double)numberOfIterations);

            //-----------------
            testList[0] = 0.2;
            testList[1] = 0.0;
            testList[2] = 0.0;
            testList.Add(0.8);
            numberOfFirstIndex = 0;
            numberOfSecondIndex = 0;
            numberOfThirdIndex = 0;
            int numberOfFourthIndex = 0;
            i = 0;
            while (i++ < numberOfIterations)
            {
                int randIndex = Randomizer.NextIndex(testList);
                switch (randIndex)
                {
                    case 0: numberOfFirstIndex++; break;
                    case 1: numberOfSecondIndex++; break;
                    case 2: numberOfThirdIndex++; break;
                    case 3: numberOfFourthIndex++; break;
                }
            }
            Console.WriteLine("[test 2] number of first index = " + numberOfFirstIndex + "; number of second index = " + numberOfSecondIndex + "; number of third index = " + numberOfThirdIndex + "; number of fourth index = " + numberOfFourthIndex);
            Assert.AreEqual<int>(numberOfFirstIndex + numberOfSecondIndex + numberOfThirdIndex + numberOfFourthIndex, numberOfIterations, "[test 2] Number of selected indexes differs from number of iterations.");
            Assert.AreEqual<int>(0, numberOfSecondIndex + numberOfThirdIndex, "[test 2] Second and third index should never be selected. 1st: {0}, 2nd: {1}", numberOfSecondIndex, numberOfThirdIndex);
            Assert.IsTrue(Math.Abs((int)(((double)numberOfIterations) * testList[0]) - numberOfFirstIndex) < (int)((((double)numberOfIterations) * testList[0]) * acceptedError), "[test 2] First index has been selected with different probability: {0}", (double)numberOfFirstIndex / (double)numberOfIterations);
            Assert.IsTrue(Math.Abs((int)(((double)numberOfIterations) * testList[3]) - numberOfFourthIndex) < (int)((((double)numberOfIterations) * testList[3]) * acceptedError), "[test 2] Fourth index has been selected with different probability: {0}", (double)numberOfFourthIndex / (double)numberOfIterations);

            //-----------------
            testList[0] = 0.2;
            testList[1] = 0.0;
            testList[2] = 0.1;
            testList[3] = 0.3;
            testList.Add(0.4);
            testList.Add(0.0);
            testList.Add(0.0);
            numberOfFirstIndex = 0;
            numberOfSecondIndex = 0;
            numberOfThirdIndex = 0;
            numberOfFourthIndex = 0;
            int numberOfFifthIndex = 0;
            int numberOfSixthIndex = 0;
            int numberOfSeventhIndex = 0;
            i = 0;
            while (i++ < numberOfIterations)
            {
                int randIndex = Randomizer.NextIndex(testList);
                switch (randIndex)
                {
                    case 0: numberOfFirstIndex++; break;
                    case 1: numberOfSecondIndex++; break;
                    case 2: numberOfThirdIndex++; break;
                    case 3: numberOfFourthIndex++; break;
                    case 4: numberOfFifthIndex++; break;
                    case 5: numberOfSixthIndex++; break;
                    case 6: numberOfSeventhIndex++; break;
                }
            }
            Console.WriteLine("[test 3] number of first index = " + numberOfFirstIndex + "; number of second index = " + numberOfSecondIndex + "; number of third index = " + numberOfThirdIndex + "; number of fourth index = " + numberOfFourthIndex + "; number of fifth index = " + numberOfFifthIndex + "; number of sixth index = " + numberOfSixthIndex + "; number of seventh index = " + numberOfSeventhIndex);
            Assert.AreEqual<int>(numberOfFirstIndex + numberOfSecondIndex + numberOfThirdIndex + numberOfFourthIndex + numberOfFifthIndex + numberOfSixthIndex + numberOfSeventhIndex, numberOfIterations, "[test 3] Number of selected indexes differs from number of iterations.");
            Assert.AreEqual<int>(0, numberOfSecondIndex + numberOfSixthIndex + numberOfSeventhIndex, "[test 3] Second and sixth and seventh index should never be selected. 2nd {0}, 6th {1}, 7th {2}", numberOfSecondIndex, numberOfSixthIndex, numberOfSeventhIndex);
            Assert.IsTrue(Math.Abs((int)(((double)numberOfIterations) * testList[0]) - numberOfFirstIndex) < (int)((((double)numberOfIterations) * testList[0]) * acceptedError), "[test 3] First index has been selected with different probability: {0}", (double)numberOfFirstIndex / (double)numberOfIterations);
            Assert.IsTrue(Math.Abs((int)(((double)numberOfIterations) * testList[2]) - numberOfThirdIndex) < (int)((((double)numberOfIterations) * testList[2]) * acceptedError), "[test 3] Fourth index has been selected with different probability: {0}", (double)numberOfThirdIndex / (double)numberOfIterations);
            Assert.IsTrue(Math.Abs((int)(((double)numberOfIterations) * testList[3]) - numberOfFourthIndex) < (int)((((double)numberOfIterations) * testList[3]) * acceptedError), "[test 3] Fourth index has been selected with different probability: {0}", (double)numberOfFourthIndex / (double)numberOfIterations);
            Assert.IsTrue(Math.Abs((int)(((double)numberOfIterations) * testList[4]) - numberOfFifthIndex) < (int)((((double)numberOfIterations) * testList[4]) * acceptedError), "[test 3] Fourth index has been selected with different probability: {0}", (double)numberOfFifthIndex / (double)numberOfIterations);
        }

        [TestMethod()]
        public void NextIndexZeroTest()
        {
            int numberOfIterations = 2000000000;
            int i = 0;
            IList<double> testList = new List<double>() { 0.9, 0, 0, 0.1 };
            while (i++ < numberOfIterations)
            {
                int randIndex = Randomizer.NextIndex(testList);
                Assert.IsTrue(randIndex >= 0, "Randomizer returned value below zero.");   //ths may occur if random value in Randomizer is equal 0.
                Assert.IsTrue(randIndex <= 3, "Randomizer returned value above 1.");
                Assert.AreNotEqual<int>(1, randIndex, "Randomizer returned 1.");
                Assert.AreNotEqual<int>(2, randIndex, "Randomizer returned 2.");
            }
        }
    }
}