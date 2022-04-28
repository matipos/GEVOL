using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gevol.evolution.evoperator.real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual.binary;
using Gevol.evolution.individual;
using Gevol.evolution.individual.real;
using MathNet.Numerics.LinearAlgebra;

namespace Gevol.evolution.evoperator.real.Tests
{
    [TestClass()]
    public class RealMutationTests : RealMutation
    {
        public RealMutationTests() : base(0.061, 0.12, 0.09)
        {
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplyExceptionTypeTest()
        {
            RealMutation mutate = new RealMutation(0.078, 0.11, 0.1);
            BinaryIndividual indv = new BinaryIndividual();
            Population pop = new Population();
            pop.Add(indv);
            mutate.Apply(pop);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplyExceptionSizeTest()
        {
            RealMutation mutate = new RealMutation(0.078, 0.11, 0.1);
            RealIndividual indv = new RealIndividual();
            Population pop = new Population();
            pop.Add(indv);
            pop.Add(indv);
            mutate.Apply(pop);
        }

        /// <summary>
        /// Run mutation in normal way and check if it works.
        /// </summary>
        [TestMethod()]
        public void ApplyTest()
        {
            RealIndividualProperties indvprop = new RealIndividualProperties() { ChromosomeLength = 8 };
            Population pop = new Population();
            RealIndividualChromosome chromosome = new RealIndividualChromosome(indvprop.ChromosomeLength, indvprop.AlphaLength);
            for (int k = 0; k < indvprop.ChromosomeLength; k++)
            {
                chromosome.Sigma.Add(k);
                chromosome.Values.Add(k);
            }
            for (int k = 0; k < indvprop.AlphaLength; k++)
            {
                chromosome.Alpha.Add(k);
            }
            RealIndividual indv1 = new RealIndividual(chromosome);
            pop.Add(indv1);
            RealMutation mutate = new RealMutation(0.078, 0.11, 0.1);
            Population newPop = mutate.Apply(pop);

            Assert.AreEqual<int>(1, newPop.Count, "Returned population has wrong size. Expected: 1, actual {0}.", newPop.Count);
            RealIndividualChromosome newChromosome = (RealIndividualChromosome)(newPop[0].Chromosome);
            Assert.AreEqual<int>(0, newChromosome.Age, "Age for new individual is incorrect. Expected: 1, actual {0}.", newChromosome.Age);
            Assert.AreEqual<int>(indvprop.ChromosomeLength, newChromosome.Sigma.Count, "Sigma chromosome has wrong length. Expected: {0}, actual: {1}.", indvprop.ChromosomeLength, newChromosome.Sigma.Count);
            Assert.AreEqual<int>(indvprop.ChromosomeLength, newChromosome.Values.Count, "Values chromosome has wrong length. Expected: {0}, actual: {1}.", indvprop.ChromosomeLength, newChromosome.Values.Count);
            Assert.AreEqual<int>(indvprop.AlphaLength, newChromosome.Alpha.Count, "Alpha chromosome has wrong length. Expected: {0}, actual: {1}.", indvprop.AlphaLength, newChromosome.Alpha.Count);

            Assert.AreNotEqual<double>(newChromosome.Sigma.Average(), chromosome.Sigma.Average(), "Sigma chromosome has not changed values.");
            Assert.AreNotEqual<double>(newChromosome.Values.Average(), chromosome.Values.Average(), "Values chromosome has not changed values.");
            Assert.AreNotEqual<double>(newChromosome.Alpha.Average(), chromosome.Alpha.Average(), "Alpha chromosome has not changed values.");
        }

        /// <summary>
        /// Check if rotation matrix is calculated in proper way.
        /// Test is performed based on referential data.
        /// </summary>
        [TestMethod()]
        public void calculateRotationMatrixTest()
        {
            IList<double> alpha = new List<double>();
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7); alpha.Add(0.15); alpha.Add(0.44); alpha.Add(0.86);
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7); alpha.Add(0.15); alpha.Add(0.44); alpha.Add(0.86);
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7); alpha.Add(0.15); alpha.Add(0.44); alpha.Add(0.86);
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7);
            int size = 7;

            Matrix<double> result = calculateRotationMatrix(alpha, size);

            Assert.AreEqual<int>(size, result.ColumnCount, "Number of columns in resulted matrix is wrong.");
            Assert.AreEqual<int>(size, result.RowCount, "Number of rows in resulted matrix is wrong.");

            Assert.AreEqual<double>(0.14, Math.Round(result[0, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 0);
            Assert.AreEqual<double>(-0.44, Math.Round(result[0, 1], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 1);
            Assert.AreEqual<double>(0.07, Math.Round(result[0, 2], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 2);
            Assert.AreEqual<double>(0.38, Math.Round(result[0, 3], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 3);
            Assert.AreEqual<double>(-0.56, Math.Round(result[0, 4], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 4);
            Assert.AreEqual<double>(-0.38, Math.Round(result[0, 5], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 5);
            Assert.AreEqual<double>(0.43, Math.Round(result[0, 6], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 6);

            Assert.AreEqual<double>(0.08, Math.Round(result[1, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 0);
            Assert.AreEqual<double>(0.01, Math.Round(result[1, 1], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 1);
            Assert.AreEqual<double>(-0.36, Math.Round(result[1, 2], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 2);
            Assert.AreEqual<double>(0.52, Math.Round(result[1, 3], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 3);
            Assert.AreEqual<double>(-0.12, Math.Round(result[1, 4], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 4);
            Assert.AreEqual<double>(-0.17, Math.Round(result[1, 5], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 5);
            Assert.AreEqual<double>(-0.74, Math.Round(result[1, 6], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 6);

            Assert.AreEqual<double>(0.42, Math.Round(result[2, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 0);
            Assert.AreEqual<double>(-0.65, Math.Round(result[2, 1], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 1);
            Assert.AreEqual<double>(-0.3, Math.Round(result[2, 2], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 2);
            Assert.AreEqual<double>(-0.33, Math.Round(result[2, 3], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 3);
            Assert.AreEqual<double>(0.41, Math.Round(result[2, 4], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 4);
            Assert.AreEqual<double>(-0.18, Math.Round(result[2, 5], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 5);
            Assert.AreEqual<double>(-0.08, Math.Round(result[2, 6], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 6);

            Assert.AreEqual<double>(0.38, Math.Round(result[3, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 0);
            Assert.AreEqual<double>(0.21, Math.Round(result[3, 1], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 1);
            Assert.AreEqual<double>(-0.64, Math.Round(result[3, 2], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 2);
            Assert.AreEqual<double>(-0.13, Math.Round(result[3, 3], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 3);
            Assert.AreEqual<double>(-0.38, Math.Round(result[3, 4], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 4);
            Assert.AreEqual<double>(0.45, Math.Round(result[3, 5], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 5);
            Assert.AreEqual<double>(0.22, Math.Round(result[3, 6], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 6);

            Assert.AreEqual<double>(0.09, Math.Round(result[4, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 0);
            Assert.AreEqual<double>(0.52, Math.Round(result[4, 1], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 1);
            Assert.AreEqual<double>(-0.24, Math.Round(result[4, 2], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 2);
            Assert.AreEqual<double>(-0.25, Math.Round(result[4, 3], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 3);
            Assert.AreEqual<double>(0.04, Math.Round(result[4, 4], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 4);
            Assert.AreEqual<double>(-0.77, Math.Round(result[4, 5], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 5);
            Assert.AreEqual<double>(0.12, Math.Round(result[4, 6], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 6);

            Assert.AreEqual<double>(0.28, Math.Round(result[5, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 0);
            Assert.AreEqual<double>(-0.02, Math.Round(result[5, 1], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 1);
            Assert.AreEqual<double>(0.41, Math.Round(result[5, 2], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 2);
            Assert.AreEqual<double>(-0.52, Math.Round(result[5, 3], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 3);
            Assert.AreEqual<double>(-0.54, Math.Round(result[5, 4], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 4);
            Assert.AreEqual<double>(-0.03, Math.Round(result[5, 5], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 5);
            Assert.AreEqual<double>(-0.44, Math.Round(result[5, 6], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 6);

            Assert.AreEqual<double>(0.76, Math.Round(result[6, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 0);
            Assert.AreEqual<double>(0.28, Math.Round(result[6, 1], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 1);
            Assert.AreEqual<double>(0.38, Math.Round(result[6, 2], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 2);
            Assert.AreEqual<double>(0.34, Math.Round(result[6, 3], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 3);
            Assert.AreEqual<double>(0.27, Math.Round(result[6, 4], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 4);
            Assert.AreEqual<double>(0.07, Math.Round(result[6, 5], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 5);
            Assert.AreEqual<double>(0.08, Math.Round(result[6, 6], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 6);
        }

        /// <summary>
        /// Check if the resulting matrix which is used to modify values is calculated properly.
        /// Referential data is used in that test.
        /// It doesn't test any method, but part of the code from method Apply. (white box test)
        /// </summary>
        [TestMethod()]
        public void calculateEpsilonMatrixTest()
        {
            IList<double> alpha = new List<double>();
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7); alpha.Add(0.15); alpha.Add(0.44); alpha.Add(0.86);
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7); alpha.Add(0.15); alpha.Add(0.44); alpha.Add(0.86);
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7); alpha.Add(0.15); alpha.Add(0.44); alpha.Add(0.86);
            alpha.Add(0.5); alpha.Add(1.2); alpha.Add(0.7);
            int size = 7;
            Matrix<double> z = Matrix<double>.Build.Dense(size, 1);
            z[0, 0] = 0.2; z[1, 0] = -0.54; z[2, 0] = 1.2; z[3, 0] = 0.88; z[4, 0] = -0.3; z[5, 0] = 0.11; z[6, 0] = -0.27;

            Matrix<double> r = calculateRotationMatrix(alpha, size);
            Matrix<double> m_epsilon = r.Multiply(z);

            Assert.AreEqual<int>(1, m_epsilon.ColumnCount, "Number of columns in resulted matrix is wrong.");
            Assert.AreEqual<int>(size, m_epsilon.RowCount, "Number of rows in resulted matrix is wrong.");

            Assert.AreEqual<double>(0.7, Math.Round(m_epsilon[0, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 0, 0);
            Assert.AreEqual<double>(0.26, Math.Round(m_epsilon[1, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 1, 0);
            Assert.AreEqual<double>(-0.34, Math.Round(m_epsilon[2, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 2, 0);
            Assert.AreEqual<double>(-0.81, Math.Round(m_epsilon[3, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 3, 0);
            Assert.AreEqual<double>(-0.9, Math.Round(m_epsilon[4, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 4, 0);
            Assert.AreEqual<double>(0.38, Math.Round(m_epsilon[5, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 5, 0);
            Assert.AreEqual<double>(0.66, Math.Round(m_epsilon[6, 0], 2), "In resulted matrix value is wrong at position[?,?] (row, column)", 6, 0);
        }
    }
}