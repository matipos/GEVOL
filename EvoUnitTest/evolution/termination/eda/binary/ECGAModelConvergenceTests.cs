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
    public class ECGAModelConvergenceTests 
    {
        int blocksIterationLimit = 3;
        double acceptedProbabilityDifference = 0.02;

        [TestMethod()]
        public void isSatisfiedTest()
        {   
            ECGAModelConvergence modelConvergenceTester = new ECGAModelConvergence(blocksIterationLimit, acceptedProbabilityDifference);

            ECGAModel modelA = PrepareModel(0);
            ECGAModel modelB = PrepareModel(1); //different model
            ECGAModel modelC = PrepareModel(2); //change in genes
            ECGAModel modelD = PrepareModel(3); //big change in probability
            ECGAModel modelE = PrepareModel(4); //small change in probability

            Assert.IsFalse(modelConvergenceTester.isSatisfied(null, modelA), "First execution should return false. BlocksIterationLimit = {0}, AcceptedProbabilityDifference = {1}.", blocksIterationLimit, acceptedProbabilityDifference);
            Assert.IsFalse(modelConvergenceTester.isSatisfied(null, modelB), "Second execution, model change should return be false. BlocksIterationLimit = {0}, AcceptedProbabilityDifference = {1}.", blocksIterationLimit, acceptedProbabilityDifference);
            Assert.IsFalse(modelConvergenceTester.isSatisfied(null, modelC), "Third execution, change in genes should return false. BlocksIterationLimit = {0}, AcceptedProbabilityDifference = {1}.", blocksIterationLimit, acceptedProbabilityDifference);
            Assert.IsFalse(modelConvergenceTester.isSatisfied(null, modelD), "Fourth execution, big change in probability should return false. BlocksIterationLimit = {0}, AcceptedProbabilityDifference = {1}.", blocksIterationLimit, acceptedProbabilityDifference);

            Assert.IsFalse(modelConvergenceTester.isSatisfied(null, modelE), "Fifth execution, small change in probability (iteration 2) should return false. BlocksIterationLimit = {0}, AcceptedProbabilityDifference = {1}.", blocksIterationLimit, acceptedProbabilityDifference);

            Assert.IsTrue(modelConvergenceTester.isSatisfied(null, modelD), "Sixth execution (iteration 3) should return false. BlocksIterationLimit = {0}, AcceptedProbabilityDifference = {1}.", blocksIterationLimit, acceptedProbabilityDifference);
        }
        
        [TestMethod()]
        public void resetTest()
        {
            ECGAModelConvergence modelConvergenceTester = new ECGAModelConvergence(blocksIterationLimit, acceptedProbabilityDifference);
            ECGAModel modelA = PrepareModel(1);
            modelA.PopulationSize = 100;
            modelA.CalculateCC();
            
            Assert.IsFalse(modelConvergenceTester.isSatisfied(null, modelA), "First test should return false.");
            Assert.AreEqual(417, Math.Round(modelConvergenceTester.Model.CC, 0), "Model is not present.");
            modelConvergenceTester.reset();
            Assert.AreEqual(null, modelConvergenceTester.Model, "Model should not be present.");
        }

        private ECGAModel PrepareModel(int modelNumber)
        {
            ECGAModel model = null;
            switch (modelNumber)
            {
                case 0:
                    model = new ECGAModel();
                    model.Blocks = new List<ECGAStructure>(new ECGAStructure[7]);
                    model.Blocks[0] = new ECGAStructure() { Genes = new List<int>() { 1, 5, 6 }, Probabilities = new List<double>() { 0.153846154, 0.153846154, 0.038461538, 0.153846154, 0.153846154, 0.076923077, 0.076923077, 0.192307692 } };
                    model.Blocks[1] = new ECGAStructure() { Genes = new List<int>() { 11, 12, 15, 18 }, Probabilities = new List<double>() { 0.038461538, 0, 0.038461538, 0.153846154, 0.038461538, 0, 0.153846154, 0.038461538, 0, 0.038461538, 0, 0.038461538, 0.076923077, 0.230769231, 0.115384615, 0.038461538 } };
                    model.Blocks[2] = new ECGAStructure() { Genes = new List<int>() { 3, 7 }, Probabilities = new List<double>() { 0.307692308, 0.192307692, 0.192307692, 0.307692308 } };
                    model.Blocks[3] = new ECGAStructure() { Genes = new List<int>() { 0 }, Probabilities = new List<double>() { 0.461538462, 0.538461538 } };
                    model.Blocks[4] = new ECGAStructure() { Genes = new List<int>() { 8 }, Probabilities = new List<double>() { 0.576923077, 0.423076923 } };
                    model.Blocks[5] = new ECGAStructure() { Genes = new List<int>() { 2, 4, 10, 16, 17 }, Probabilities = new List<double>() { 0, 0, 0.038461538, 0.076923077, 0, 0.076923077, 0, 0.038461538, 0, 0, 0, 0, 0.038461538, 0.038461538, 0, 0, 0.076923077, 0, 0, 0, 0.076923077, 0.038461538, 0.038461538, 0.038461538, 0.038461538, 0.153846154, 0.076923077, 0.038461538, 0.038461538, 0.038461538, 0, 0.038461538, } };
                    model.Blocks[6] = new ECGAStructure() { Genes = new List<int>() { 9, 13, 14 }, Probabilities = new List<double>() { 0.115384615, 0.269230769, 0.192307692, 0.192307692, 0.038461538, 0.115384615, 0.038461538, 0.038461538 } };
                    break;
                case 1:
                    model = new ECGAModel();
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 0 }, Probabilities = new List<double>() { 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 1 }, Probabilities = new List<double>() { 4.0 / 5.0, 1.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 2 }, Probabilities = new List<double>() { 2.0 / 5.0, 3.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 3 }, Probabilities = new List<double>() { 3.0 / 5.0, 2.0 / 5.0 } });
                    break;
                case 2: //two genes are combined
                    model = new ECGAModel();
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 0 }, Probabilities = new List<double>() { 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 1, 3 }, Probabilities = new List<double>() { 4.0 / 5.0, 1.0 / 5.0, 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 2 }, Probabilities = new List<double>() { 2.0 / 5.0, 3.0 / 5.0 } });
                    break;
                case 3: //big difference in probability for gene 1
                    model = new ECGAModel();
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 0 }, Probabilities = new List<double>() { 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 1, 3 }, Probabilities = new List<double>() { 4.0 / 5.0, (1.0 / 5.0) - (1.5 * acceptedProbabilityDifference), 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 2 }, Probabilities = new List<double>() { 2.0 / 5.0, 3.0 / 5.0 } });
                    break;
                case 4: //small difference in probability for gene 2
                    model = new ECGAModel();
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 0 }, Probabilities = new List<double>() { 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 1, 3 }, Probabilities = new List<double>() { 4.0 / 5.0, (1.0 / 5.0) - (1.5 * acceptedProbabilityDifference), 3.0 / 5.0, 2.0 / 5.0 } });
                    model.Blocks.Add(new ECGAStructure() { Genes = new List<int>() { 2 }, Probabilities = new List<double>() { (2.0 / 5.0) - (0.8 * acceptedProbabilityDifference), 3.0 / 5.0 } });
                    break;
            }
            return model;
        }
    }
}