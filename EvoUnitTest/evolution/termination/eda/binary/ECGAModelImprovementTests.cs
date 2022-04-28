using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.termination.eda.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.evoperator.eda.model;

namespace Gevol.evolution.termination.eda.binary.Tests
{
    [TestClass()]
    public class ECGAModelImprovementTests
    {
        [TestMethod()]
        public void isSatisfiedTest()
        {
            ECGAModelImprovement ecgaModelTest = new ECGAModelImprovement();
            ECGAModel modelA = PrepareModel(0);
            ECGAModel modelB = PrepareModel(1);
            bool result;

            modelA.PopulationSize = 100;
            modelB.PopulationSize = 100;
            modelA.CalculateCC();
            modelB.CalculateCC();

            result = ecgaModelTest.isSatisfied(null, modelB);
            Assert.AreEqual(false, result, "First test should return false.");
            Assert.AreEqual(417, Math.Round(ecgaModelTest.Model.CC, 0), "Model is not present.");
            result = ecgaModelTest.isSatisfied(null, modelA);
            Assert.AreEqual(false, result, "Second test should return false.");
            Assert.AreEqual(382, Math.Round(ecgaModelTest.Model.CC, 0), "Better model should be present.");
            result = ecgaModelTest.isSatisfied(null, modelA);
            Assert.AreEqual(true, result, "Third test should return true.");
            Assert.AreEqual(382, Math.Round(ecgaModelTest.Model.CC, 0), "The best model should be present.");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void isSatisfiedExceptionTest()
        {
            UMDAModel model = new UMDAModel();
            ECGAModelImprovement ecgaModelTest = new ECGAModelImprovement();
            ecgaModelTest.isSatisfied(null, model);
        }

        [TestMethod()]
        public void resetTest()
        {
            ECGAModelImprovement ecgaModelTest = new ECGAModelImprovement();
            ECGAModel modelA = PrepareModel(0);
            bool result;

            modelA.PopulationSize = 100;
            modelA.CalculateCC();

            result = ecgaModelTest.isSatisfied(null, modelA);
            Assert.AreEqual(false, result, "First test should return false.");
            Assert.AreEqual(382, Math.Round(ecgaModelTest.Model.CC, 0), "Model is not present.");
            ecgaModelTest.reset();
            Assert.AreEqual(null, ecgaModelTest.Model, "Model should not be present.");
        }


        private ECGAModel PrepareModel(int modelNumber)
        {
            ECGAModel model = null;
            switch (modelNumber)
            {
                case 0:
                    model = new ECGAModel();
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 0, 1 }, Probabilities = new List<double>() { 3.0 / 5.0, 0.0, 1.0 / 5.0, 1.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 2, 3 }, Probabilities = new List<double>() { 1.0 / 5.0, 1.0 / 5.0, 2.0 / 5.0, 1.0 / 5.0 } });
                    break;
                case 1:
                    model = new ECGAModel();
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 0 }, Probabilities = new List<double>() { 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 1 }, Probabilities = new List<double>() { 4.0 / 5.0, 1.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 2 }, Probabilities = new List<double>() { 2.0 / 5.0, 3.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 3 }, Probabilities = new List<double>() { 3.0 / 5.0, 2.0 / 5.0 } });
                    break;
            }
            return model;
        }
    }
}