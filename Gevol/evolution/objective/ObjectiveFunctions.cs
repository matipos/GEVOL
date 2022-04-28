using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.objective
{
    /// <summary>
    /// Top class for object contains objective functions used in algorithm.
    /// Algorithm is able to have more than one objective function. 
    /// Generally score for any individual from every objective function is computed here.
    /// </summary>
    public class ObjectiveFunctions 
    {
        /// <summary>
        /// Collection of objective functions.
        /// </summary>
        public IList<ObjectiveFunction> ObjFunctions { get; set; }

        /// <summary>
        /// Compute generally score of every objective function.
        /// </summary>
        /// <param name="individual">Individual to be scored.</param>
        /// <returns>Score for each objective function</returns>
        public IList<double> Evaluate(Individual individual)
        {
            IList<double> scores = new List<double>(ObjFunctions.Count);
            for (int i = 0; i < ObjFunctions.Count; i++)
            {
                scores.Add(ObjFunctions[i].Evaluate(individual));
            }
            return scores;
        }

        /// <summary>
        /// Compute score of every objective function for the whole population.
        /// Assign the score to each individual
        /// </summary>
        /// <param name="individual">Population to be avaluated.</param>
        public void Evaluate(Population population)
        {
            for (int i = 0; i < population.Count; i++)
            {
                IList<double> scores = Evaluate(population[i]);
                population[i].Score = scores;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ObjectiveFunctions()
        {
            this.ObjFunctions = new List<ObjectiveFunction>();
        }
    }
}
