using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.individual;
using Gevol.evolution.individual.real;
using Gevol.evolution.util;
using MathNet.Numerics.LinearAlgebra;

namespace Gevol.evolution.evoperator.real
{
    /// <summary>
    /// 
    /// </summary>
    public class RealMutation : Operator
    {
        //parameter for updating alpha
        //how strong alpha parameter will change, good value is usually below 0.1
        private double beta;
        //parameter for updating sigma
        //usually below 0.2
        private double tau;         //to generate epsilon for each sigma
        private double tau_prim;    //to generate epsilon0 (stronger influence)

        /// <summary>
        /// Mutation operator for real individuals require additional parameter
        /// to control how strong alpha and sigma will change
        /// </summary>
        /// <param name="beta">how strong alpha will be changed</param>
        /// <param name="tau">how strong sigma will be changed (epsilon)</param>
        /// <param name="tau_prim">how strong sigma will be changed (epsilon0)</param>
        public RealMutation(double beta, double tau, double tau_prim)
        {
            this.beta = beta;
            this.tau = tau;
            this.tau_prim = tau_prim;
        }

        public override Population Apply(Population population)
        {
            if (population.Count != 1)
            {
                throw new ArgumentException("In the population should be only one individual.", "population");
            }
            if (!(population[0] is RealIndividual))
            {
                throw new ArgumentException("Individual in population must be type of RealIndividual.", "population");
            }
            RealIndividual newIndividual = new RealIndividual((RealIndividualChromosome)population[0].Chromosome);

            updateAlpha(((RealIndividualChromosome)newIndividual.Chromosome).Alpha);
            updateSigma(((RealIndividualChromosome)newIndividual.Chromosome).Sigma);

            Matrix<double> z = generateZMatrix(((RealIndividualChromosome)newIndividual.Chromosome).Sigma);
            Matrix<double> r = calculateRotationMatrix(((RealIndividualChromosome)newIndividual.Chromosome).Alpha, z.RowCount);
            Matrix<double> m_epsilon = r.Multiply(z);

            //set new values
            for(int i = 0; i < m_epsilon.RowCount; i++)
            {
                ((RealIndividualChromosome)newIndividual.Chromosome).Values[i] += m_epsilon[i, 0];
            }
            RealIndividualChromosome newChromosome = (RealIndividualChromosome)newIndividual.Chromosome;
            newChromosome.Age = 0;

            Population pop = new Population();
            pop.Add(newIndividual);
            return pop;
        }

        /// <summary>
        /// Update alphas, add random number by Normal Distribution with mean 0 and standard deviation beta.
        /// alpha = alpha + gaussian(0, beta^2)
        /// </summary>
        /// <param name="alpha"></param>
        protected void updateAlpha(IList<double> alpha)
        {
            for(int i = 0; i < alpha.Count; i++)
            {
                alpha[i] = alpha[i] + Randomizer.RandomGaussian(0, beta);
            }
        }

        /// <summary>
        /// Update sigmas.
        /// epsilon0 = gaussian(0, tau_prim)
        /// sigma = sigma * exp(epsilon0 + gaussian(0, tau)
        /// </summary>
        /// <param name="sigma"></param>
        protected void updateSigma(IList<double> sigma)
        {
            //prepare epsilon0
            double epsilon0 = Randomizer.RandomGaussian(0, tau_prim);
            //update all sigmas
            for (int i = 0; i < sigma.Count; i++)
            {
                sigma[i] = sigma[i] * Math.Exp(epsilon0 + Randomizer.RandomGaussian(0, tau));
            }
        }

        /// <summary>
        /// Generate z matrix. Matrix is already transposed to size [chromosome length, 1].
        /// Matrix z[i,0] = gaussian(0, sigma[i])
        /// </summary>
        /// <param name="sigma"></param>
        /// <returns></returns>
        protected Matrix<double> generateZMatrix(IList<double> sigma)
        {
            Matrix<double> z = Matrix<double>.Build.Dense(sigma.Count, 1);
            for (int i = 0; i < sigma.Count; i++)
            {
                z[i,0] = Randomizer.RandomGaussian(0, sigma[i]);
            }
            //transpose?
            return z;
        }

        /// <summary>
        /// Calculate rotation matrix.
        /// Alpha length should be (size * (size - 1)) / 2
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected Matrix<double> calculateRotationMatrix(IList<double> alpha, int size)
        {
            MatrixBuilder<double> builder = Matrix<double>.Build;
            Matrix<double> r = builder.DenseIdentity(size, size);
            Matrix<double> temp;
            int j = -1;
            for (int p = 0; p < size - 1; p++)
            {
                for (int q = p + 1; q < size; q++)
                {
                    j++;
                    temp = builder.DenseIdentity(size, size);
                    temp[p, p] = Math.Cos(alpha[j]);
                    temp[p, q] = -Math.Sin(alpha[j]);
                    temp[q, p] = Math.Sin(alpha[j]);
                    temp[q, q] = Math.Cos(alpha[j]);
                    r = r.Multiply(temp);
                }
            }
            return r;
        }
    }
}
