﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class SchwefelFunctionTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EvaluateExceptionArgumentTest()
        {
            SchwefelFunction fun = new SchwefelFunction();
            BinaryIndividual indv = new BinaryIndividual();
            fun.Evaluate(indv);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            SchwefelFunction fun = new SchwefelFunction();
            RealIndividual indv = new RealIndividual();
            RealIndividualProperties indvprop = new RealIndividualProperties() { ChromosomeLength = 10 };
            RealIndividualChromosome chromosome = new RealIndividualChromosome(indvprop.ChromosomeLength, indvprop.AlphaLength);
            indv.Chromosome = chromosome;
            for (int k = 0; k < indvprop.ChromosomeLength; k++)
            {
                chromosome.Values.Add(420.968746);
            }
            double result = fun.Evaluate(indv);
            Assert.AreEqual<double>(-0.000003, Math.Round(result, 6), "Returned value is wrong.");

            chromosome.Values[3] = -2;
            result = fun.Evaluate(indv);
            Assert.AreEqual<double>(420.958416, Math.Round(result, 6), "Returned value is wrong.");

            for (int k = 0; k < indvprop.ChromosomeLength; k++)
            {
                chromosome.Values[k] = 0;
            }
            result = fun.Evaluate(indv);
            Assert.AreEqual<double>(4189.82887, Math.Round(result, 6), "Returned value is wrong.");

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
            Assert.AreEqual<double>(4135.615836, Math.Round(result, 6), "Returned value is wrong.");
        }
    }
}