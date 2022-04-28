using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.evoperator.binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual;
namespace Gevol.evolution.evoperator.binary.Tests
{
    [TestClass()]
    public class NegationMutationTests
    {
        /// <summary>
        /// Test mutation
        /// </summary>
        [TestMethod()]
        public void ApplyTest()
        {
            IList<int> chromosome = new List<int>() { 1, 0, 1, 1, 1, 0, 0, 0, 1 };
            BinaryIndividual individual = new BinaryIndividual(new List<int>(chromosome));
            Population population = new Population();
            population.Add(individual);

            //set mutation to 0, so all genes should be the same
            NegationMutation mutationOperator = new NegationMutation(0);    
            population = mutationOperator.Apply(population);
        Console.WriteLine(population[0].ToString());
            for (int i = 0; i < chromosome.Count; i++)
            {
                Assert.AreEqual<int>(chromosome[i], ((IList<int>)population[0].Chromosome)[i], "Gene has been mutated, but it shouldn't");
            }

            //all genes will be mutated
            mutationOperator.MutationProbability = 1;
            population = mutationOperator.Apply(population);
        Console.WriteLine(population[0].ToString());
            //compare, all genes should have opposite values
            for (int i = 0; i < chromosome.Count; i++)
            {
                Assert.AreEqual<int>(chromosome[i] == 1 ? 0 : 1, ((IList<int>)population[0].Chromosome)[i],"Gene has not been mutated, but it should");
            }
            
            //set mutation to 50%, so not all genes should be mutated
            mutationOperator.MutationProbability = 0.5;
            population = mutationOperator.Apply(population);
        Console.WriteLine(population[0].ToString());
            int theSameGenes = 0;
            for (int i = 0; i < chromosome.Count; i++)
            {
                if (chromosome[i] == ((IList<int>)population[0].Chromosome)[i])
                {
                    theSameGenes++;
                }
            }
            Assert.AreNotEqual<int>(theSameGenes,0,"None of the genes have been mutated, but some should be");
            Assert.AreNotEqual<int>(theSameGenes, chromosome.Count, "All of the genes have been mutated, not all should be mutated");
        }
    }
}
