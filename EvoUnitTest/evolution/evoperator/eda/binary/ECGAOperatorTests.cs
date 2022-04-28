using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.eda.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.evoperator.eda.model;
using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using Gevol.evolution.util;

namespace Gevol.evolution.evoperator.eda.binary.Tests
{
    [TestClass()]
    public class ECGAOperatorTests : ECGAOperator
    {
        public ECGAOperatorTests() : base(1,1)
        {
        }

        [TestMethod()]
        public void ApplyTest()
        {
            this.NewPopulationSize = 5; // 10000;

            //Model 1, population 1:
            this.ChromosomeLength = 4;
            this.Model = PrepareModel(1);
            ECGAModel refModel = PrepareModel(1);       //all genes are independent
            refModel.PopulationSize = NewPopulationSize;
            refModel.CalculateCC();
            this.Apply(PreparePopulation(1));

            ECGAModel newModel = (ECGAModel)this.Model;
            Console.WriteLine("****************ref Model 1 (population 1): " + refModel.ToString());
            Console.WriteLine("****************new model 1: " + newModel.ToString() + "\r\n");
            Assert.IsTrue(refModel.CC > newModel.CC, "Model 1 (population 1): new model is not better, refModel.CC = {0}; newModel.CC = {1}.", refModel.CC, newModel.CC);
            Assert.AreEqual<double>(35.0398, Math.Round(newModel.CC, 4), "Model 1 (population 1): new model has wrong CC value.");
            Assert.AreNotEqual<int>(0, newModel.PopulationSize, "Model 1 (population 1): population size has not been set.");
            //check probabilities
            foreach (ECGAStructure block in newModel.Blocks)
            {
                Assert.AreEqual<int>((int)Math.Pow(2, block.Genes.Count), block.Probabilities.Count, "Model 1 population 1: probability size is incorrect.");
                double sumOfProbabilities = 0.0;
                foreach (double probability in block.Probabilities)
                {
                    sumOfProbabilities += probability;
                }
                Assert.AreEqual<double>(1.0, Math.Round(sumOfProbabilities, 1), "Model 1 population 1: probabilities have wrong values.");
            }

            //Model 1, population 2:
            //this.NewPopulationSize = 1000; // PreparePopulation(2).Count; 
            this.ChromosomeLength = 4;
            this.Model = PrepareModel(1);
            refModel = PrepareModel(1);       //all genes are independent
            Population testPopulation = PreparePopulation(2);
            testPopulation.AddRange(PreparePopulation(2));
            refModel.PopulationSize = testPopulation.Count;
            refModel.CalculateCC();
            this.Apply(testPopulation);

            newModel = (ECGAModel)this.Model;
            Console.WriteLine("****************ref Model 1 (population 2): " + refModel.ToString());
            Console.WriteLine("****************new model 1: " + newModel.ToString() + "\r\n");
            Assert.IsTrue(refModel.CC > newModel.CC, "Model 1 (population 2): new model is not better, refModel.CC = {0}; newModel.CC = {1}.", refModel.CC, newModel.CC);
            Assert.AreEqual<double>(100.5858, Math.Round(newModel.CC, 4), "Model 1 (population 2): new model has wrong CC value.");
            Assert.AreNotEqual<int>(0, newModel.PopulationSize, "Model 1 (population 2): population size has not been set.");
            //check probabilities
            foreach (ECGAStructure block in newModel.Blocks)
            {
                Assert.AreEqual<int>((int)Math.Pow(2, block.Genes.Count), block.Probabilities.Count, "Model 1 population 2: probability size is incorrect.");
                double sumOfProbabilities = 0.0;
                foreach (double probability in block.Probabilities)
                {
                    sumOfProbabilities += probability;
                }
                Assert.AreEqual<double>(1.0, Math.Round(sumOfProbabilities, 1), "Model 1 population 2: probabilities have wrong values.");
            }

            /*
             * To be sure, unmark Console.WriteLine lines in ECGAOperator.Apply.
             * Model is built to fit in the best way to the population. If we generate population randomly, the best model is without any concatenated blocks.
             * If we run based on the test models, the result will be the same or very close as the referencial model.
             * */
            /*
            //Model 1:
            this.NewPopulationSize = 10000;
            this.ChromosomeLength = 4;
            this.Model = PrepareModel(1);
            refModel.PopulationSize = NewPopulationSize;
            refModel.CalculateCC();
            this.Apply(PrepareRandomPopulation(ChromosomeLength, NewPopulationSize));   //random population, probability for every gene is equal

            newModel = (ECGAModel)this.Model;
            Console.WriteLine("****************ref Model 1: " + refModel.ToString());
            Console.WriteLine("****************new model 1: " + newModel.ToString() + "\r\n");
            Assert.IsTrue(refModel.CC > newModel.CC, "Model 1: new model is not better, refModel.CC = {0}; newModel.CC = {1}.", refModel.CC, newModel.CC);
            Assert.AreNotEqual<int>(0, newModel.PopulationSize, "Model 1: population size has not been set.");
            //check probabilities
            foreach (ECGAStructure block in newModel.Blocks)
            {
                Assert.AreEqual<int>((int)Math.Pow(2, block.Genes.Count), block.Probabilities.Count, "Model 1: probability size is incorrect.");
                double sumOfProbabilities = 0.0;
                foreach (double probability in block.Probabilities)
                {
                    sumOfProbabilities += probability;
                }
                Assert.AreEqual<double>(1.0, Math.Round(sumOfProbabilities, 1), "Model 1: probabilities have wrong values.");
            }

            //Model 0:
            this.ChromosomeLength = 19;
            this.Model = PrepareModel(0);
            refModel = PrepareModel(0);
            refModel.PopulationSize = NewPopulationSize;
            refModel.CalculateCC();
            this.Apply(PrepareRandomPopulation(ChromosomeLength, NewPopulationSize));   //random population, probability for every gene is equal

            newModel = (ECGAModel)this.Model;
            Console.WriteLine("****************ref Model 0: " + refModel.ToString());
            Console.WriteLine("****************new model 0: " + newModel.ToString());
            Assert.IsTrue(refModel.CC > newModel.CC, "Model 0: new model is not better, refModel.CC = {0}; newModel.CC = {1}.", refModel.CC, newModel.CC);
            Assert.AreNotEqual<int>(0, newModel.PopulationSize, "Model 0: population size has not been set.");
            //check probabilities
            foreach(ECGAStructure block in newModel.Blocks)
            {
                Assert.AreEqual<int>((int)Math.Pow(2, block.Genes.Count), block.Probabilities.Count, "Model 0: probability size is incorrect.");
                double sumOfProbabilities = 0.0;
                foreach(double probability in block.Probabilities)
                {
                    sumOfProbabilities += probability;
                }
                Assert.AreEqual<double>(1.0, Math.Round(sumOfProbabilities,1), "Model 0: probabilities have wrong values.");
            }
            */
        }

        [TestMethod()]
        public void ConcatenateGroupsTest()
        {
            ECGAModel modelA = PrepareModel(0);
            modelA.PopulationSize = 340;
            int originalModelSize = modelA.Blocks.Count;
            ECGAModel modelB = ConcatenateGroups(modelA, 3, 4); //concatenate blocks
            modelB.PopulationSize = 340;

            Assert.AreEqual<int>(originalModelSize, modelA.Blocks.Count, "Original model has been changed.");
            Assert.AreEqual<int>(originalModelSize - 1, modelB.Blocks.Count, "Number of groups in model B is incorrect.");
            //check every group count
            ECGAModel refModel = PrepareModel(0);
            for(int i = 0; i < modelA.Blocks.Count; i++)
            {
                Assert.AreEqual<int>(refModel.Blocks[i].Genes.Count, modelA.Blocks[i].Genes.Count, "Number of genes in group {0} is incorrect in original model.", i);
                Assert.AreEqual<int>(refModel.Blocks[i].Probabilities.Count, modelA.Blocks[i].Probabilities.Count, "Number of probabilities in group {0} is incorrect in original model.", i);
            }
            //check every group in new model
            Assert.AreEqual<int>(refModel.Blocks[0].Genes.Count, modelB.Blocks[0].Genes.Count, "Number of genes in group 0 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[0].Probabilities.Count, modelB.Blocks[0].Probabilities.Count, "Number of probabilities in group 0 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[1].Genes.Count, modelB.Blocks[1].Genes.Count, "Number of genes in group 1 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[1].Probabilities.Count, modelB.Blocks[1].Probabilities.Count, "Number of probabilities in group 1 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[2].Genes.Count, modelB.Blocks[2].Genes.Count, "Number of genes in group 2 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[2].Probabilities.Count, modelB.Blocks[2].Probabilities.Count, "Number of probabilities in group 2 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[3].Genes.Count + refModel.Blocks[4].Genes.Count, modelB.Blocks[3].Genes.Count, "Number of genes in group 3 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[3].Probabilities.Count + refModel.Blocks[4].Probabilities.Count, modelB.Blocks[3].Probabilities.Count, "Number of probabilities in group 3 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[5].Genes.Count, modelB.Blocks[4].Genes.Count, "Number of genes in group 4 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[5].Probabilities.Count, modelB.Blocks[4].Probabilities.Count, "Number of probabilities in group 4 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[6].Genes.Count, modelB.Blocks[5].Genes.Count, "Number of genes in group 5 is incorrect in model B.");
            Assert.AreEqual<int>(refModel.Blocks[6].Probabilities.Count, modelB.Blocks[5].Probabilities.Count, "Number of probabilities in group 5 is incorrect in model B.");

            //check concatenated group
            Assert.AreEqual<int>(refModel.Blocks[3].Genes[0], modelB.Blocks[3].Genes[0], "Gene 0 in model B is incorrect.");
            Assert.AreEqual<int>(refModel.Blocks[4].Genes[0], modelB.Blocks[3].Genes[1], "Gene 1 in model B is incorrect.");

            ECGAModel modelC = ConcatenateGroups(modelB, 1, 5); //in model A it would be 1, 6, but after first concatenation group 6 is 5 now
            Assert.AreEqual<int>(refModel.Blocks[6].Genes[0], modelC.Blocks[1].Genes[0], "Gene 0 in model C is incorrect.");
            Assert.AreEqual<int>(refModel.Blocks[1].Genes[0], modelC.Blocks[1].Genes[1], "Gene 1 in model C is incorrect.");
            Assert.AreEqual<int>(refModel.Blocks[1].Genes[1], modelC.Blocks[1].Genes[2], "Gene 2 in model C is incorrect.");
            Assert.AreEqual<int>(refModel.Blocks[6].Genes[1], modelC.Blocks[1].Genes[3], "Gene 3 in model C is incorrect.");
            Assert.AreEqual<int>(refModel.Blocks[6].Genes[2], modelC.Blocks[1].Genes[4], "Gene 4 in model C is incorrect.");
            Assert.AreEqual<int>(refModel.Blocks[1].Genes[2], modelC.Blocks[1].Genes[5], "Gene 5 in model C is incorrect.");
            Assert.AreEqual<int>(refModel.Blocks[1].Genes[3], modelC.Blocks[1].Genes[6], "Gene 6 in model C is incorrect.");
        }

        [TestMethod()]
        public void CalculateModelTest()
        {
            ECGAModel modelA = PrepareModel(1);
            ECGAModel refModel = PrepareModel(1);
            //reset values
            foreach(ECGAStructure block in modelA.Blocks)
            {
                block.Probabilities = new List<double>(new double[(int)Math.Pow(2, block.Genes.Count)]);
            }
            CalculateModel(PreparePopulation(1), ref modelA);
            
            for(int i = 0; i < refModel.Blocks.Count; i++)
            {
                Assert.AreEqual<int>(refModel.Blocks[i].Probabilities.Count, modelA.Blocks[i].Probabilities.Count, "Model A, probability count is incorrect in block {0}.", i);
                for (int j = 0; j < refModel.Blocks[i].Probabilities[j]; j++)
                {
                    Assert.AreEqual<double>(refModel.Blocks[i].Probabilities[j], modelA.Blocks[i].Probabilities[j], "Model A, block {0}, probability {1} is incorrect.", i, j);
                }
            }

            ECGAModel modelB = PrepareModel(0);
            refModel = PrepareModel(0);
            //reset values
            foreach (ECGAStructure block in modelB.Blocks)
            {
                block.Probabilities = new List<double>(new double[(int)Math.Pow(2, block.Genes.Count)]);
            }
            CalculateModel(PreparePopulation(0), ref modelB);

            for (int i = 0; i < refModel.Blocks.Count; i++)
            {
                Assert.AreEqual<int>(refModel.Blocks[i].Probabilities.Count, modelB.Blocks[i].Probabilities.Count, "Model B, probability count is incorrect in block {0}.", i);
                for (int j = 0; j < refModel.Blocks[i].Probabilities[j]; j++)
                {
                    Assert.AreEqual<double>(Math.Round(refModel.Blocks[i].Probabilities[j], 4), Math.Round(modelB.Blocks[i].Probabilities[j], 4), "Model B, block {0}, probability {1} is incorrect.", i, j);
                }
            }
        }

        [TestMethod()]
        public void GeneratePopulationTest()
        {
            //while (true) { 
            double acceptedError = 0.1;
            this.NewPopulationSize = 1000000;
            this.ChromosomeLength = 4;
            this.Model = PrepareModel(1);
            ECGAModel model = (ECGAModel)Model;
            ECGAModel refModel = PrepareModel(1);

            Population population = this.GeneratePopulation();
            Assert.AreEqual<int>(NewPopulationSize, population.Count, "Model 1, Population size is incorrect.");
            //reset probabilities
            foreach (ECGAStructure block in model.Blocks)
            {
                block.Probabilities = new List<double>(new double[(int)Math.Pow(2, block.Genes.Count)]);
            }
            CalculateModel(population, ref model);  //Calculation of probabilities has been already tested

            //compare probabilities
            for(int i = 0; i < refModel.Blocks.Count; i++)
            {
                Assert.AreEqual<int>(refModel.Blocks[i].Probabilities.Count, model.Blocks[i].Probabilities.Count, "Model 1, probability size for block {0} is incorrect.", i);
                for(int p = 0; p < refModel.Blocks[i].Probabilities.Count; p++)
                {
                    //Assert.AreEqual<double>(Math.Round(refModel.Blocks[i].Probabilities[p], 2), Math.Round(model.Blocks[i].Probabilities[p], 2), "Model 1, probability for block {0}, index {1} is incorrect.", i, p);
                    Assert.IsTrue(Math.Abs(Math.Round(refModel.Blocks[i].Probabilities[p], 2) - Math.Round(model.Blocks[i].Probabilities[p], 2)) <= acceptedError, "Model 1, probability for block {0}, index {1} is incorrect, calculated probability: {2}, expected: {3}.", i, p, model.Blocks[i].Probabilities[p], refModel.Blocks[i].Probabilities[p]);
                }
            }

            //model 0
            this.ChromosomeLength = 19;
            this.Model = PrepareModel(0);
            model = (ECGAModel)Model;
            refModel = PrepareModel(0);

            population = this.GeneratePopulation();
            Assert.AreEqual<int>(NewPopulationSize, population.Count, "Model 0, Population size is incorrect.");
            //reset probabilities
            foreach (ECGAStructure block in model.Blocks)
            {
                block.Probabilities = new List<double>(new double[(int)Math.Pow(2, block.Genes.Count)]);
            }
            CalculateModel(population, ref model);  //Calculation of probabilities has been already tested

            //compare probabilities
            for (int i = 0; i < refModel.Blocks.Count; i++)
            {
                Assert.AreEqual<int>(refModel.Blocks[i].Probabilities.Count, model.Blocks[i].Probabilities.Count, "Model 0, probability size for block {0} is incorrect.", i);
                for (int p = 0; p < refModel.Blocks[i].Probabilities.Count; p++)
                {
                    //Assert.AreEqual<double>(Math.Round(refModel.Blocks[i].Probabilities[p], 2), Math.Round(model.Blocks[i].Probabilities[p], 2), "Model 0, probability for block {0}, index {1} is incorrect.", i, p);
                    Assert.IsTrue(Math.Abs(Math.Round(refModel.Blocks[i].Probabilities[p], 2) - Math.Round(model.Blocks[i].Probabilities[p], 2)) <= acceptedError, "Model 0, probability for block {0}, index {1} is incorrect, calculated probability: {2}, expected: {3}.", i, p, model.Blocks[i].Probabilities[p], refModel.Blocks[i].Probabilities[p]);
                }
            }
            //}
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
                    /*foreach (ECGAStructure block in model.Blocks)
                    {
                        block.Probabilities = new double[(int)Math.Pow(2, block.Genes.Count)];
                    }*/
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

        private Population PreparePopulation(int populationNumber)
        {
            Population population = new Population();
            switch (populationNumber)
            {
                case 0:
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 0, 0, 1 } });

                    break;
                case 1:
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 0, 1 } });
                    break;
                case 2:
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 0, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 0, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 0, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 1 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 0, 0, 1, 0 } });
                    population.Add(new BinaryIndividual() { Chromosome = new List<int>() { 1, 1, 0, 1 } });
                    break;
            }
            return population;
        }

        /// <summary>
        /// Generate random population
        /// </summary>
        /// <param name="chromosomeLength"></param>
        /// <param name="populationSize"></param>
        /// <returns></returns>
        private Population PrepareRandomPopulation(int chromosomeLength, int populationSize)
        {
            Population population = new Population();
            for(int i = 0; i < populationSize; i++)
            {
                BinaryIndividual individual = new BinaryIndividual();
                IList<int> chromosome = new List<int>();
                for (int g = 0; g < chromosomeLength; g++)
                {
                    chromosome.Add(Randomizer.NextInt(0, 1));
                }
                individual.Chromosome = chromosome;
                population.Add(individual);
            }
            return population;
        }
    }
}