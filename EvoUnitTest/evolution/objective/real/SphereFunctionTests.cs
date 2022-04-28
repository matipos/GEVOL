using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.objective.real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual.real;

namespace Gevol.evolution.objective.real.Tests
{
    [TestClass()]
    public class SphereFunctionTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EvaluateExceptionArgumentTest()
        {
            SphereFunction fun = new SphereFunction();
            BinaryIndividual indv = new BinaryIndividual();
            fun.Evaluate(indv);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            SphereFunction fun = new SphereFunction();
            RealIndividual indv = new RealIndividual();
            RealIndividualProperties indvprop = new RealIndividualProperties() { ChromosomeLength = 10 };
            RealIndividualChromosome chromosome = new RealIndividualChromosome(indvprop.ChromosomeLength, indvprop.AlphaLength);
            indv.Chromosome = chromosome;
            for (int k = 0; k < indvprop.ChromosomeLength; k++)
            {
                chromosome.Values.Add(0);
            }
            double result = fun.Evaluate(indv);
            Assert.AreEqual<double>(0, result, "Returned value is wrong.");

            chromosome.Values[3] = 1;
            result = fun.Evaluate(indv);
            Assert.AreEqual<double>(1, result, "Returned value is wrong.");

            chromosome.Values[3] = 0;
            chromosome.Values[7] = -2;
            result = fun.Evaluate(indv);
            Assert.AreEqual<double>(4, result, "Returned value is wrong.");

            chromosome.Values[0] = 2.3;
            chromosome.Values[1] = 0;
            chromosome.Values[2] = 4.856;
            chromosome.Values[3] = 0;
            chromosome.Values[4] = 1.22;
            chromosome.Values[5] = 0;
            chromosome.Values[6] = -98.2;
            chromosome.Values[7] = 0;
            chromosome.Values[8] = 1.22;
            chromosome.Values[9] = 9.834;
            result = fun.Evaluate(indv);
            Assert.AreEqual<double>(9771.795092, Math.Round(result,6), "Returned value is wrong.");
        }
    }
}