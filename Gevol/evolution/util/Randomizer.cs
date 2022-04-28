using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.util
{
    /// <summary>
    /// Utility class that provides operations for random numbers.
    /// </summary>
    public class Randomizer
    {
        private static Random _rand = null;

        private static Random Rand
        {
            get
            {
                if (_rand == null)
                {
                    _rand = new Random(DateTime.Now.Millisecond);
                }
                return _rand;
            }
        }

        /// <summary>
        /// Generate natural number between minValue and maxValue.
        /// </summary>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>Random value between min and max.</returns>
        public static int NextInt(int minValue, int maxValue)
        {
            return Rand.Next(minValue, maxValue + 1);
        }

        /// <summary>
        /// Generate real number between minValue and maxValue.
        /// </summary>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>Random value between min and max.</returns>
        public static double NextDouble(double minValue, double maxValue)
        {
            return (Rand.NextDouble() * (maxValue - minValue)) + minValue;
        }

        /// <summary>
        /// Generate bool value [0,1] based on the probability.
        /// The probability determine the possibility to get 1.
        /// </summary>
        /// <param name="probability">Probability to get 1</param>
        /// <returns></returns>
        public static int NextBoolIntProbability(double probability)
        {
            if (NextDouble(0, 1) < probability)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Generate Gaussian pseudorandom numbers with mean and standard deviation.
        /// To generate the numbers used polar method of G. E. P. Box, M. E. Muller, G. Marsglia:
        /// "The Art of Computer" Donald E. Knuth
        /// </summary>
        /// <param name="mean">Mean.</param>
        /// <param name="standardDeviation">Standard deviation.</param>
        /// <returns>Next Gaussian random number.</returns>
        public static double RandomGaussian(double mean, double standardDeviation)
        {
            double v1, v2, s;
            do
            {
                v1 = (2 * NextDouble(-1, 1));   // between -1.0 and 1.0
                v2 = (2 * NextDouble(-1, 1));   // between -1.0 and 1.0
                s = v1 * v1 + v2 * v2;
            } while (s >= 1 || s == 0);
            double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);
            //double nextGaussian2 = v2 * multiplier;
            double nextGaussian = v1 * multiplier;
            //nextGaussian - rozklad normalny z mediana 0 i odchyleniem standardowym 1
            double result = mean + (nextGaussian * standardDeviation);

            //prawo Tuzina  - DZIALA!
            /*double result = 0;
            for (int i = 0; i < 12; i++)
            {
                result += NextDouble(0, 1);
            }
            result -= 6;*/

            //Console.WriteLine(nextGaussian);
            return result;// mean + (result * standardDeviation);
        }

        /// <summary>
        /// Rand index from the probability list. Sum of all values in the list should be 1.0.
        /// If the probability is 0 at some index, it will never be selected.
        /// </summary>
        /// <param name="probabilities">List of probabilities</param>
        /// <returns></returns>
        public static int NextIndex(IList<double> probabilities)
        {
            double value = Randomizer.NextDouble(0.0, 1.0);
            double sumOfProbabilities = 0.0;
            int index = 0;
            while(sumOfProbabilities < value)
            {
                sumOfProbabilities += probabilities[index++];
            }
            return (index == 0 ? 0 : --index);  //it's possible that value = 0.0, so it never gets into the loop, so the index will not be increased.
        }
    }
}
