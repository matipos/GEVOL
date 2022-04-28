using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.replacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual;

namespace Gevol.evolution.evoperator.replacement.Tests
{
    [TestClass()]
    public class NSGA2ReplacementTests : NSGA2Replacement
    {
        [TestMethod()]
        public void ApplyTest()
        {
            Population children = createPopulation(false);
            Population parents = createPopulation(true);
            Population newPopulation = this.Apply(children, parents);

            for (int i = 0; i < newPopulation.Count; i++)
                Console.WriteLine(i + ": " + newPopulation[i].Score[0] + " , " + newPopulation[i].Score[1]);

            Assert.AreEqual<int>(parents.Count, newPopulation.Count, "New population has wrong size.");
            //first Pareto front
            Assert.AreEqual<double>(parents[0].Score[0] + parents[8].Score[0], newPopulation[0].Score[0] + newPopulation[1].Score[0], "First Pareto front error");
            Assert.AreEqual<double>(parents[0].Score[1] + parents[8].Score[1], newPopulation[0].Score[1] + newPopulation[1].Score[1], "First Pareto front error");
            //second Pareto front
            Assert.AreEqual<double>(parents[3].Score[0] + parents[5].Score[0], newPopulation[2].Score[0] + newPopulation[3].Score[0], "Second Pareto front error (border ones)");
            Assert.AreEqual<double>(parents[3].Score[1] + parents[5].Score[1], newPopulation[2].Score[1] + newPopulation[3].Score[1], "Second Pareto front error (border ones)");
            Assert.AreEqual<double>(children[0].Score[0], newPopulation[4].Score[0], "Second Pareto front error (middle)");
            Assert.AreEqual<double>(children[0].Score[1], newPopulation[4].Score[1], "Second Pareto front error (middle)");
            Assert.AreEqual<double>(parents[6].Score[0], newPopulation[5].Score[0], "Second Pareto front error (middle)");
            Assert.AreEqual<double>(parents[6].Score[1], newPopulation[5].Score[1], "Second Pareto front error (middle)");
            //third Pareto front
            Assert.AreEqual<double>(parents[2].Score[0] + children[3].Score[0], newPopulation[6].Score[0] + newPopulation[7].Score[0], "Third Pareto front error (border ones)");
            Assert.AreEqual<double>(parents[2].Score[1] + children[3].Score[1], newPopulation[6].Score[1] + newPopulation[7].Score[1], "Third Pareto front error (border ones)");
            Assert.AreEqual<double>(parents[7].Score[0], newPopulation[8].Score[0], "Third Pareto front error (middle)");
            Assert.AreEqual<double>(parents[7].Score[1], newPopulation[8].Score[1], "Third Pareto front error (middle)");
        }

        [TestMethod()]
        public void CalculateParetoRankTest()
        {
            this.unionStructurePopulation = createStructurePopulation();
            List<int> numberOfParetoFronts = CalculateParetoRank();

            for (int i = 0; i < unionStructurePopulation.Count; i++)
                Console.Out.WriteLine(i + " [" + unionStructurePopulation[i].Individual.Score[0] + "," + unionStructurePopulation[i].Individual.Score[1] + "]: " + unionStructurePopulation[i].ParetoRank);

            //check number of Pareto fronts
            Assert.AreEqual<int>(6, numberOfParetoFronts.Count, "Total number of Pareto fronts is incorrect.");
            Assert.AreEqual<int>(2, numberOfParetoFronts[0], "Number of individuals with Pareto rank = 0 is incorrect.");
            Assert.AreEqual<int>(4, numberOfParetoFronts[1], "Number of individuals with Pareto rank = 1 is incorrect.");
            Assert.AreEqual<int>(4, numberOfParetoFronts[2], "Number of individuals with Pareto rank = 2 is incorrect.");
            Assert.AreEqual<int>(1, numberOfParetoFronts[3], "Number of individuals with Pareto rank = 3 is incorrect.");
            Assert.AreEqual<int>(2, numberOfParetoFronts[4], "Number of individuals with Pareto rank = 4 is incorrect.");
            Assert.AreEqual<int>(1, numberOfParetoFronts[5], "Number of individuals with Pareto rank = 5 is incorrect.");

            //check each individual
            Assert.AreEqual<int>(0, unionStructurePopulation[0].ParetoRank, "Pareto rank for individual 0 is incorrect.");
            Assert.AreEqual<int>(1, unionStructurePopulation[1].ParetoRank, "Pareto rank for individual 1 is incorrect.");
            Assert.AreEqual<int>(3, unionStructurePopulation[2].ParetoRank, "Pareto rank for individual 2 is incorrect.");
            Assert.AreEqual<int>(4, unionStructurePopulation[3].ParetoRank, "Pareto rank for individual 3 is incorrect.");
            Assert.AreEqual<int>(2, unionStructurePopulation[4].ParetoRank, "Pareto rank for individual 4 is incorrect.");
            Assert.AreEqual<int>(2, unionStructurePopulation[5].ParetoRank, "Pareto rank for individual 5 is incorrect.");
            Assert.AreEqual<int>(1, unionStructurePopulation[6].ParetoRank, "Pareto rank for individual 6 is incorrect.");
            Assert.AreEqual<int>(2, unionStructurePopulation[7].ParetoRank, "Pareto rank for individual 7 is incorrect.");
            Assert.AreEqual<int>(5, unionStructurePopulation[8].ParetoRank, "Pareto rank for individual 8 is incorrect.");
            Assert.AreEqual<int>(1, unionStructurePopulation[9].ParetoRank, "Pareto rank for individual 9 is incorrect.");
            Assert.AreEqual<int>(4, unionStructurePopulation[10].ParetoRank, "Pareto rank for individual 10 is incorrect.");
            Assert.AreEqual<int>(1, unionStructurePopulation[11].ParetoRank, "Pareto rank for individual 11 is incorrect.");
            Assert.AreEqual<int>(2, unionStructurePopulation[12].ParetoRank, "Pareto rank for individual 12 is incorrect.");
            Assert.AreEqual<int>(0, unionStructurePopulation[13].ParetoRank, "Pareto rank for individual 13 is incorrect.");
        }

        [TestMethod()]
        public void CalculateCrowdingDistanceTest()
        {
            this.unionStructurePopulation = createStructurePopulation();
            CalculateParetoRank();
            //for each Pareto front
            for (int f = 0; f < 6; f++)
            {
                for (int s = 0; s < unionStructurePopulation[0].Individual.Score.Count; s++)
                {
                    CalculateCrowdingDistance(from individual in unionStructurePopulation where individual.ParetoRank == f orderby individual.Individual.Score[s] select individual.index, s);
                }
            }

            //check each individual
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[0].CrowdingDistance, "Crowding distance for individual 0 is incorrect.");
            Assert.AreEqual<double>(1.6333, Math.Round(unionStructurePopulation[1].CrowdingDistance,4), "Crowding distance for individual 1 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[2].CrowdingDistance, "Crowding distance for individual 2 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[3].CrowdingDistance, "Crowding distance for individual 3 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[4].CrowdingDistance, "Crowding distance for individual 4 is incorrect.");
            Assert.AreEqual<double>(1.125, unionStructurePopulation[5].CrowdingDistance, "Crowding distance for individual 5 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[6].CrowdingDistance, "Crowding distance for individual 6 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[7].CrowdingDistance, "Crowding distance for individual 7 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[8].CrowdingDistance, "Crowding distance for individual 8 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[9].CrowdingDistance, "Crowding distance for individual 9 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[10].CrowdingDistance, "Crowding distance for individual 10 is incorrect.");
            Assert.AreEqual<double>(0.9333, Math.Round(unionStructurePopulation[11].CrowdingDistance,4), "Crowding distance for individual 11 is incorrect.");
            Assert.AreEqual<double>(1.25, unionStructurePopulation[12].CrowdingDistance, "Crowding distance for individual 12 is incorrect.");
            Assert.AreEqual<double>(Double.MaxValue, unionStructurePopulation[13].CrowdingDistance, "Crowding distance for individual 13 is incorrect.");
        }

        private List<NSGA2Structure> createStructurePopulation()
        {
            List<NSGA2Structure> unionStructurePopulation = new List<NSGA2Structure>()
            {
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 2,1 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 0 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 2,2 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 1 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 3,2.5 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 2 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 3.5,3.5 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 3 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 1,5 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 4 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 2,3 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 5 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 4,1 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 6 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 5,1 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 7 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 4.5,3 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 8 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 1,3.5 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 9 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 4,3 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 10 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 1.5,3 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 11 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 2.5,2 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 12 },
                new NSGA2Structure() { Individual = new BinaryIndividual() { Score = new List<double>() { 1,1.5 } }, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = 13 }
            };
            return unionStructurePopulation;
        }

        /// <summary>
        /// Create childreen or parents population
        /// </summary>
        /// <param name="parents">true if create parents population, false for childreen one</param>
        /// <returns></returns>
        private Population createPopulation(bool parents)
        {
            List<NSGA2Structure> technicalPopulation = createStructurePopulation();
            Population newPopulation = new Population();
            if (parents)
            {
                /*0*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[0].Individual.Score[0], technicalPopulation[0].Individual.Score[1] } });
                /*1*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[3].Individual.Score[0], technicalPopulation[3].Individual.Score[1] } });
                /*2*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[4].Individual.Score[0], technicalPopulation[4].Individual.Score[1] } });
                /*3*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[6].Individual.Score[0], technicalPopulation[6].Individual.Score[1] } });
                /*4*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[8].Individual.Score[0], technicalPopulation[8].Individual.Score[1] } });
                /*5*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[9].Individual.Score[0], technicalPopulation[9].Individual.Score[1] } });
                /*6*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[11].Individual.Score[0], technicalPopulation[11].Individual.Score[1] } });
                /*7*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[12].Individual.Score[0], technicalPopulation[12].Individual.Score[1] } });
                /*8*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[13].Individual.Score[0], technicalPopulation[13].Individual.Score[1] } });
            } else
            {
                /*0*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[1].Individual.Score[0], technicalPopulation[1].Individual.Score[1] } });
                /*1*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[2].Individual.Score[0], technicalPopulation[2].Individual.Score[1] } });
                /*2*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[5].Individual.Score[0], technicalPopulation[5].Individual.Score[1] } });
                /*3*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[7].Individual.Score[0], technicalPopulation[7].Individual.Score[1] } });
                /*4*/newPopulation.Add(new BinaryIndividual() { Score = new List<double> { technicalPopulation[10].Individual.Score[0], technicalPopulation[10].Individual.Score[1] } });                
            }
            return newPopulation;
        }
    }
}