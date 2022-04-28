using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;

namespace Gevol.evolution.evoperator.replacement
{
    /// <summary>
    /// Replacement for multiobjective algorithms using NSGA2 Algorithm.
    /// Non-Dominated Sorting Genetic Algorithm uses Pareto-optimal method.
    /// 
    /// Implementation based on:
    /// A Fast Elitist Non-Dominated Sorting Genetic Algorithm for Multi-Objective Optimization: NSGA-II
    /// Kalyanmoy Deb, Samir Agrawal, Amrit Pratap, and T Meyarivan
    /// Kanpur Genetic Algorithms Laboratory (KanGAL)
    /// Indian Institute of Technology Kanpur
    /// Kanpur, PIN 208 016, India
    /// KanGAL Report No. 200001
    /// </summary>
    public class NSGA2Replacement : ReplacementOperator
    {
        protected List<NSGA2Structure> unionStructurePopulation;

        /// <summary>
        /// Execute NSGA-II algorith on parent and childreen population.
        /// Size of the new population is parentsPopulation.Count()
        /// </summary>
        /// <param name="childrenPopulation"></param>
        /// <param name="parentsPopulation"></param>
        /// <returns></returns>
        public override Population Apply(Population childrenPopulation, Population parentsPopulation)
        {
            unionStructurePopulation = new List<NSGA2Structure>();
            Population newPopulation;
            List<int> paretoFrontCounter;     //number of individuals in each Pareto front
            int idx = 0;
            foreach (Individual individual in childrenPopulation)
                unionStructurePopulation.Add(new NSGA2Structure() { Individual = individual, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = idx++ });
            foreach (Individual individual in parentsPopulation)
                unionStructurePopulation.Add(new NSGA2Structure() { Individual = individual, CrowdingDistance = 0, ParetoRank = 0, dominatingIndividuals = 0, dominatedIndividuals = new HashSet<int>(), index = idx++ });

            paretoFrontCounter = CalculateParetoRank();  //calculate Pareto front for each individual
            //calculate how many Pareto fronts we need to fulfill new population
            int sumOfIndividuals = 0;
            int index = 0;
            while (sumOfIndividuals < parentsPopulation.Count)
            {
                sumOfIndividuals += paretoFrontCounter[index++];                
            }
            //calculate the crowding distance
            for(int i = 0; i <= index; i++)
            {
                //for each objective function
                for (int s = 0; s < unionStructurePopulation[0].Individual.Score.Count; s++)
                {
                    CalculateCrowdingDistance(from individual in unionStructurePopulation where individual.ParetoRank == i orderby individual.Individual.Score[s] select individual.index, s);
                }
            }
            //select the best individuals to the new population
            IEnumerable<int> selectedIndividuals = (from individual in unionStructurePopulation orderby individual.ParetoRank, individual.CrowdingDistance descending select individual.index).Take<int>(parentsPopulation.Count);
            newPopulation = new Population();
            foreach(int selectedIndex in selectedIndividuals)
            {
                if (selectedIndex >= childrenPopulation.Count)
                    newPopulation.Add(parentsPopulation[selectedIndex - childrenPopulation.Count]); 
                else
                    newPopulation.Add(childrenPopulation[selectedIndex]);
            }
            return newPopulation;
        }

        /// <summary>
        /// Calculate Pareto Rank for each individual.
        /// First, the best Pareto rank is equal to 0, then 1, 2, 3, etc.
        /// </summary>
        /// <param name="unionPopulation">Population to compute Pareto rank.</param>
        /// <returns>Number of individuals in each Pareto front</returns>
        protected List<int> CalculateParetoRank()
        {
            int maxParetoRank = 0;
            List<int> paretoFrontCounter = new List<int>();
            HashSet<int> paretoFront = new HashSet<int>();
            for (int i = 0; i < unionStructurePopulation.Count; i++)
            {
                for (int j = 0; j < unionStructurePopulation.Count; j++)
                {
                    if (Compare(unionStructurePopulation[i].Individual.Score, unionStructurePopulation[j].Individual.Score) == -1)
                    {
                        //i dominate j
                        unionStructurePopulation[i].dominatedIndividuals.Add(j);
                    }
                    else
                    {
                        if (Compare(unionStructurePopulation[i].Individual.Score, unionStructurePopulation[j].Individual.Score) == 1)
                        {
                            //j dominate i
                            unionStructurePopulation[i].dominatingIndividuals++;
                        }
                    }
                }
                if (unionStructurePopulation[i].dominatingIndividuals == 0)
                {
                    //i is in the first Pareto front 
                    paretoFront.Add(i);
                }
            }
            //calculate next Pareto fronts
            while (paretoFront.Count != 0)
            {
                paretoFrontCounter.Add(paretoFront.Count);      //add number of individuals in this Pareto front to the list
                paretoFront = findNextParetoFront(++maxParetoRank, paretoFront);
            }
            return paretoFrontCounter;
        }

        private HashSet<int> findNextParetoFront(int nextFrontNumber, HashSet<int> previousFront)
        {
            HashSet<int> nextFront = new HashSet<int>();
            foreach (int index in previousFront)
            {
                foreach (int dominatedIndex in unionStructurePopulation[index].dominatedIndividuals)
                {
                    unionStructurePopulation[dominatedIndex].dominatingIndividuals--;
                    if (unionStructurePopulation[dominatedIndex].dominatingIndividuals == 0)
                    {
                        unionStructurePopulation[dominatedIndex].ParetoRank = nextFrontNumber;
                        nextFront.Add(dominatedIndex);
                    }
                }
            }
            return nextFront;
        }


        /// <summary>
        /// Compute crowding distance for specified Pareto front.
        /// </summary>
        /// <param name="frontIndexes">Indexes of elements from the same Pareto front.</param>
        /// <param name="unionPopulation">The whole population.</param>
        protected void CalculateCrowdingDistance(IEnumerable<int> frontIndexes, int objIndex)
        //protected void CalculateCrowdingDistance(IEnumerable<NSGA2Structure> frontIndexes)
        {
            unionStructurePopulation[frontIndexes.First<int>()].CrowdingDistance = Double.MaxValue; //min value
            unionStructurePopulation[frontIndexes.Last<int>()].CrowdingDistance = Double.MaxValue;  //max value
            double normalization = unionStructurePopulation[frontIndexes.Last<int>()].Individual.Score[objIndex] - unionStructurePopulation[frontIndexes.First<int>()].Individual.Score[objIndex];
            for(int i = 1; i < frontIndexes.Count<int>(); i++)
            {
                if(unionStructurePopulation[frontIndexes.ElementAt<int>(i)].CrowdingDistance < Double.MaxValue)
                {
                    unionStructurePopulation[frontIndexes.ElementAt<int>(i)].CrowdingDistance += (unionStructurePopulation[frontIndexes.ElementAt<int>(i + 1)].Individual.Score[objIndex] - unionStructurePopulation[frontIndexes.ElementAt<int>(i - 1)].Individual.Score[objIndex]) / normalization;
                }
            }
        }

        /// <summary>
        /// Compare two lists.
        /// If each element of firstList is bigger than secondList then return 1
        /// If each element of firstList is less than secondList then return -1
        /// Other case return 0
        /// </summary>
        /// <param name="firstList">List to be compared.</param>
        /// <param name="secondList">List to compare.</param>
        /// <returns>Result of comparision.</returns>
        private int Compare(IList<double> firstList, IList<double> secondList)
        {
            if (firstList.Count != secondList.Count)
            {
                throw new Exception("Lists have different count!");
            }
            //firstList is higher 
            bool thisHigher = true;
            //firstList is smaller
            bool thisSmaller = true;
            for (int i = 0; i < firstList.Count; i++)
            {
                if (secondList[i] > firstList[i])
                    thisHigher = false;
                if (firstList[i] > secondList[i])
                    thisSmaller = false;
            }
            if (thisHigher && !thisSmaller)
            {
                return 1;
            }
            if (thisSmaller && !thisHigher)
            {
                return -1;
            }
            return 0;
        }

        protected class NSGA2Structure
        {
            public Individual Individual;
            public int ParetoRank;
            public double CrowdingDistance;
            public int dominatingIndividuals;           //number of individuals that dominate this individual
            public HashSet<int> dominatedIndividuals;   //set of individuals that are dominated by this individual
            public int index;
        }

        /// <summary>
        /// Not supported operation for this operator.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Population Apply(Population population)
        {
            throw new NotImplementedException();
        }
    }
    
}
