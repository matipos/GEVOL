using Gevol.evolution.individual;
using Gevol.evolution.individual.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.objective.binary
{
    /// <summary>
    /// Global optimum is when individual match the pattern.
    /// </summary>
    public class BinaryPattern : ObjectiveFunction
    {
        /// <summary>
        /// Pattern that algorithm is looking for.
        /// </summary>
        public IList<int> Pattern { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pattern">Pattern to be found.</param>
        public BinaryPattern(IList<int> pattern)
        {
            this.Pattern = pattern;
        }

        /// <summary>
        /// Compute how many times found matching.
        /// Global optimum is -Pattern.Length
        /// </summary>
        /// <param name="individual">Individual to be scored.</param>
        /// <returns>Score.</returns>
        public override double Evaluate(Individual individual)
        {
            if (!(individual is BinaryIndividual))
            {
                throw new ArgumentException("Parameter must be type of BinaryIndividual.", "individual");
            }
            double matching = 0;
            for (int i = 0; i < Pattern.Count; i++)
            {
                if (this.Pattern[i] == ((IList<int>)individual.Chromosome)[i])
                {
                    matching--;
                }
            }
            return matching;
        }
    }
}
