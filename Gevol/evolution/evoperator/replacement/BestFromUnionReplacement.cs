using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.replacement
{
    /// <summary>
    /// Return population with the best individuals from union of parents and childreen population.
    /// </summary>
    public class BestFromUnionReplacement : ReplacementOperator
    {
        /// <summary>
        /// Merge two population and return new population with the best individuals.
        /// Size of the new population is parentsPopulation.Count.
        /// </summary>
        /// <param name="childrenPopulation">Children population.</param>
        /// <param name="parentsPopulation">Parents population.</param>
        /// <returns>New population.</returns>
        public override Population Apply(Population childrenPopulation, Population parentsPopulation)
        {
            Population unionPopulation = new Population(parentsPopulation);
            for (int i = 0; i < childrenPopulation.Count; i++) { unionPopulation.Add(childrenPopulation[i]); }

            unionPopulation.Sort();

            while (unionPopulation.Count > parentsPopulation.Count)
            {
                unionPopulation.RemoveAt(unionPopulation.Count - 1);
            }
            return unionPopulation;
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
