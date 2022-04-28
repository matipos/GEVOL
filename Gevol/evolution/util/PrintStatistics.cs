using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.util
{
    /// <summary>
    /// Print statistic.
    /// It is possible to print statistics for each iteration. It save iteration number;
    /// Printed statistics:
    /// Generation (iteration):
    /// Population size: 
    /// The best individual: 
    /// The worst individual:
    /// Score of the best individual: 
    /// Score of the worst individual: 
    /// Average score of the population: 
    /// </summary>
    public class PrintStatistics
    {
        private TextWriter _writer = null;
        private int iteration = 0;  //number of iteration.

        /// <summary>
        /// Target to save statistics.
        /// </summary>
        public TextWriter Writer { get { return _writer; } set { _writer = value; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="replacement">Replacement operator.</param>
        public PrintStatistics(TextWriter writer)
        {
            this._writer = writer;
        }

        /// <summary>
        /// Print statistics for the population:
        /// *************************************
        /// Generation (iteration):
        /// Population size:
        /// The best individual:
        /// The worst inividual:
        /// Score for the best individual:
        /// Score for the worst individual:
        /// Average score of the population (first objective function):
        /// </summary>
        /// <param name="population"></param>
        public virtual void PrintPopulation(Population population)
        {
            iteration++;
            double sumOfValues = 0;
            Individual theBestIndividual = population[0];
            Individual theWorstIndividual = population[0];
            foreach (Individual individual in population)
            {
                sumOfValues += individual.Score[0];
                if (theBestIndividual.Score[0] > individual.Score[0]) { theBestIndividual = individual; }
                if (theWorstIndividual.Score[0] < individual.Score[0]) { theWorstIndividual = individual; }
            }
            //Print statstics:
            _writer.WriteLine("*************************************");
            _writer.WriteLine("Generation (iteration): " + iteration);
            _writer.WriteLine("Population size: " + population.Count);
            _writer.WriteLine("The best individual: " + theBestIndividual.ToString());
            _writer.WriteLine("The worst individual: " + theWorstIndividual.ToString());
            //_writer.WriteLine("Score of the best individual: " + newPopulation.TheBestScore);
            _writer.Write("Score of the best individual: ");
            for (int i = 0; i < theBestIndividual.Score.Count; i++) { _writer.Write(theBestIndividual.Score[i] + " "); }
            //Console.WriteLine("Score of the worst individual: " + newPopulation.TheWorstScore);
            _writer.Write("\nScore of the worst individual: ");
            for (int i = 0; i < theWorstIndividual.Score.Count; i++) { _writer.Write(theWorstIndividual.Score[i] + " "); }
            _writer.WriteLine("\nAverage score of the population (first objective function): " + (sumOfValues / population.Count));
            _writer.Flush();
        }

        /// <summary>
        /// Print model for the EDA algorightms.
        /// </summary>
        public void PrintEdaModel(object model)
        {
            _writer.WriteLine(model.ToString());
            _writer.Flush();
        }

        /// <summary>
        /// Reset statistics for the beginning.
        /// Set iteration number to 0.
        /// </summary>
        public void Reset()
        {
            iteration = 0;
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
