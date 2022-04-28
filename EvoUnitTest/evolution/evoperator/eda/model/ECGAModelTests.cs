using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.eda.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.eda.model.Tests
{
    [TestClass()]
    public class ECGAModelTests : ECGAModel
    {
        [TestMethod()]
        public void GenesSetToIndexTest()
        {
            ECGAModel model = new ECGAModel();
            Assert.AreEqual<int>(6, model.GenesSetToIndex(new List<int>() { 0, 1, 1, 0 }));
            Assert.AreEqual<int>(12, model.GenesSetToIndex(new List<int>() { 1, 1, 0, 0 }));
            Assert.AreEqual<int>(98, model.GenesSetToIndex(new List<int>() { 0, 1, 1, 0, 0, 0, 1, 0 }));
            Assert.AreEqual<int>(101, model.GenesSetToIndex(new List<int>() { 1, 1, 0, 0, 1, 0, 1 }));
            Assert.AreEqual<int>(2, model.GenesSetToIndex(new List<int>() { 0, 0, 0, 1, 0 }));
            Assert.AreEqual<int>(0, model.GenesSetToIndex(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
        }

        [TestMethod()]
        public void IndexToGenesSetTest()
        {
            ECGAModel model = new ECGAModel();
            Assert.IsTrue(model.IndexToGenesSet(7, 3).SequenceEqual<int>(new List<int>() { 1, 1, 1 }), "Failed to convert value of 7 into binary code. Returned value is {0}", ListToString(model.IndexToGenesSet(7, 3)));
            Assert.IsTrue(model.IndexToGenesSet(71, 7).SequenceEqual<int>(new List<int>() { 1, 0, 0, 0, 1, 1, 1 }), "Failed to convert value of 71 into binary code. Returned value is {0}", ListToString(model.IndexToGenesSet(71, 7)));
            Assert.IsTrue(model.IndexToGenesSet(0, 2).SequenceEqual<int>(new List<int>() { 0, 0 }), "Failed to convert value of 0 into binary code. Returned value is {0}", ListToString(model.IndexToGenesSet(0, 2)));
            Assert.IsTrue(model.IndexToGenesSet(22, 7).SequenceEqual<int>(new List<int>() { 0, 0, 1, 0, 1, 1, 0 }), "Failed to convert value of 22 into binary code. Returned value is {0}", ListToString(model.IndexToGenesSet(22, 7)));
            Assert.IsTrue(model.IndexToGenesSet(11, 4).SequenceEqual<int>(new List<int>() { 1, 0, 1, 1 }), "Failed to convert value of 11 into binary code. Returned value is {0}", ListToString(model.IndexToGenesSet(11, 4)));
            Assert.IsTrue(model.IndexToGenesSet(3, 2).SequenceEqual<int>(new List<int>() { 1, 1 }), "Failed to convert value of 3 into binary code. Returned value is {0}", ListToString(model.IndexToGenesSet(3, 2)));
            Assert.IsTrue(model.IndexToGenesSet(1, 3).SequenceEqual<int>(new List<int>() { 0, 0, 1 }), "Failed to convert value of 1 into binary code. Returned value is {0}", ListToString(model.IndexToGenesSet(1, 3)));
        }

        private string ListToString(IList<int> list)
        {
            string result = "";
            for (int i = 0; i < list.Count; i++)
                result += list[i];
            return result;
        }

        [TestMethod()]
        public void CalculateModelComplexityTest()
        {
            this.PopulationSize = 5;

            this.Blocks = getIndependentModel();
            Assert.AreEqual<double>(18.58, Math.Round(this.CalculateModelComplexity(), 2), "MC for first model failed");
            
            this.Blocks = getModel2();
            Assert.AreEqual<double>(18.58, Math.Round(this.CalculateModelComplexity(), 2), "MC for second model failed");
        }

        [TestMethod()]
        public void CalculateCompressedPopulationComplexityTest()
        {
            this.PopulationSize = 5;

            this.Blocks = getIndependentModel();
            Assert.AreEqual<double>(18.17, Math.Round(this.CalculateCompressedPopulationComplexity(), 2), "CPC for first model failed");
            
            this.Blocks = getModel2();
            Assert.AreEqual<double>(16.56, Math.Round(this.CalculateCompressedPopulationComplexity(), 2), "CPC for second model failed");
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CalculateCCExceptionTest()
        {
            ECGAModel model = new ECGAModel();

            model.Blocks = getIndependentModel();
            double cc = model.CC;
        }

        [TestMethod()]
        public void CalculateCCTest()
        {
            ECGAModel model = new ECGAModel();
            model.PopulationSize = 5;

            model.Blocks = getIndependentModel();
            Assert.AreEqual<double>(36.75, Math.Round(model.CC,2), "CC for first model failed");

            model.Blocks = getModel2();
            Assert.AreEqual<double>(35.14, Math.Round(model.CC, 2), "CC should be updated after getting new model");
            Assert.AreEqual<double>(35.14, Math.Round(model.CalculateCC(), 2), "CC for second model failed");
            Assert.AreEqual<double>(35.14, Math.Round(model.CC, 2), "CC has not been updated");

            model.Blocks[0].Probabilities[2] -= 0.15;
            model.Blocks[0].Probabilities[3] += 0.15;
            Assert.AreEqual<double>(35.14, Math.Round(model.CC, 2), "CC shouldn't be updated");
            Assert.AreNotEqual<double>(35.14, Math.Round(model.CalculateCC(), 2), "CC should be updated");
        }

        [TestMethod()]
        public void CloneTest()
        {
            int populationSize = 33;
            int modifier = 10;
            ECGAModel model = new ECGAModel() { Blocks = getModel2(), PopulationSize = populationSize };
            ECGAModel clone = model.Clone();
            
            //modify every value in the clone
            foreach(ECGAStructure cloneBlock in clone.Blocks)
            {
                for(int i = 0; i < cloneBlock.Genes.Count; i++)
                {
                    cloneBlock.Genes[i] += modifier;
                }
                for (int i = 0; i < cloneBlock.Probabilities.Count; i++)
                {
                    cloneBlock.Probabilities[i] += modifier;
                }
            }

            for(int i = 0; i < model.Blocks.Count; i++)
            {
                for(int j = 0; j < model.Blocks[i].Genes.Count; j++)
                {
                    Assert.AreNotEqual<int>(model.Blocks[i].Genes[j], clone.Blocks[i].Genes[j], "Gene {0} has been changed.", j);
                    Assert.AreEqual<int>(model.Blocks[i].Genes[j], clone.Blocks[i].Genes[j] -  modifier, "Gene {0} has wrong value.", j);
                }
                for (int j = 0; j < model.Blocks[i].Probabilities.Count; j++)
                {
                    Assert.AreNotEqual<double>(Math.Round(model.Blocks[i].Probabilities[j], 2), Math.Round(clone.Blocks[i].Probabilities[j], 2), "Probability {0} has been changed.", j);
                    Assert.AreEqual<double>(Math.Round(model.Blocks[i].Probabilities[j], 2), Math.Round(clone.Blocks[i].Probabilities[j] - modifier, 2), "Probability {0} has wrong value.", j);
                }
            }
        }

        [TestMethod()]
        public void ToStringTest()
        {
            ECGAModel model = new ECGAModel();
            Console.WriteLine("Manual test for ToString method\r\nModel 1:");
            model.Blocks = getIndependentModel();
            model.PopulationSize = 1200;
            model.CalculateCC();
            Console.WriteLine(model.ToString());
            Assert.AreEqual<String>("CC=MC+CPC;4443,56640342561=81,8305495239671+4361,73585390164;[0]{0=0,6;1=0,4}[1]{0=0,8;1=0,2}[2]{0=0,4;1=0,6}[3]{0=0,6;1=0,4}", model.ToString());
            Console.WriteLine("\r\nModel2:");
            model.Blocks = getModel2();
            model.PopulationSize = 1200;
            model.CalculateCC();
            Console.WriteLine(model.ToString());
            Assert.AreEqual<String>("CC=MC+CPC;4057,25268956077=81,8305495239671+3975,42214003681;[0+1]{00=0,6;01=0;10=0,2;11=0,2}[2]{0=0,4;1=0,6}[3]{0=0,6;1=0,4}", model.ToString());
        }

        /// <summary>
        /// Test for all independent genes
        /// model: x1,x2,x3,x4
        /// The population:
        /// x1	x2	x3	x4
        /// 0	0	0	0
        /// 0	0	1	0
        /// 0	0	1	1
        /// 1	0	1	0
        /// 1	1	0	1
        /// </summary>
        /// <returns></returns>
        private IList<ECGAStructure> getIndependentModel()
        {
            IList<ECGAStructure> model = new List<ECGAStructure>();
            for (int i = 0; i < 4; i++)
            {
                ECGAStructure structure = new ECGAStructure();
                structure.Genes.Add(i);
                switch (i)
                {
                    case 0:
                        structure.Probabilities.Add(3.0 / 5.0);
                        structure.Probabilities.Add(2.0 / 5.0);
                        break;
                    case 1:
                        structure.Probabilities.Add(4.0 / 5.0);
                        structure.Probabilities.Add(1.0 / 5.0);
                        break;
                    case 2:
                        structure.Probabilities.Add(2.0 / 5.0);
                        structure.Probabilities.Add(3.0 / 5.0);
                        break;
                    case 3:
                        structure.Probabilities.Add(3.0 / 5.0);
                        structure.Probabilities.Add(2.0 / 5.0);
                        break;
                }
                model.Add(structure);
            }
            return model;
        }

        /// <summary>
        /// Test for all independent genes
        /// model: x1+x2,x3,x4
        /// The population:
        /// x1	x2	x3	x4
        /// 0	0	0	0
        /// 0	0	1	0
        /// 0	0	1	1
        /// 1	0	1	0
        /// 1	1	0	1
        /// </summary>
        /// <returns></returns>
        private IList<ECGAStructure> getModel2()
        {
            IList<ECGAStructure> model = new List<ECGAStructure>();
            for (int i = 0; i < 3; i++)
            {
                ECGAStructure structure = new ECGAStructure();
                switch (i)
                {
                    case 0:
                        structure.Genes.Add(0);
                        structure.Genes.Add(1);
                        structure.Probabilities.Add(3.0 / 5.0); //00
                        structure.Probabilities.Add(0.0 / 5.0); //01
                        structure.Probabilities.Add(1.0 / 5.0); //10
                        structure.Probabilities.Add(1.0 / 5.0); //11
                        break;
                    case 1:
                        structure.Genes.Add(2);
                        structure.Probabilities.Add(2.0 / 5.0);
                        structure.Probabilities.Add(3.0 / 5.0);
                        break;
                    case 2:
                        structure.Genes.Add(3);
                        structure.Probabilities.Add(3.0 / 5.0);
                        structure.Probabilities.Add(2.0 / 5.0);
                        break;
                }
                model.Add(structure);
            }
            return model;
        }
    }
}