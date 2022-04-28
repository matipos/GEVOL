using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.individual
{
    /// <summary>
    /// Top class for individual.
    /// </summary>
    public abstract class Individual : IComparable
    {
        /// <summary>
        /// Represent chromosome. Can be any type.
        /// </summary>
        public abstract object Chromosome { get; set; }

        /// <summary>
        /// Generate new individual.
        /// </summary>
        /// <param name="individualProperties">Additiona information how the individual should be generated, e.g. chromosome length</param>
        /// <returns>New individual.</returns>
        public abstract Individual GenerateIndividual(object individualProperties);
        /*{
            throw new NotImplementedException("GenerateIndividual method should be implemented by derived class.");
        }*/
        
        /// <summary>
        /// Represent individual by string.
        /// </summary>
        /// <returns>Representation of the individual.</returns>
        public new abstract String ToString();

        /// <summary>
        /// Default method used in Population.Sort()
        /// By default compares only score for the firest objective function
        /// </summary>
        /// <param name="compareIndividual">Other individual from population</param>
        /// <returns></returns>
        public virtual int CompareTo(object compareIndividual)
        {
            if (!(compareIndividual is Individual))
            {
                throw new ArgumentException("Parameter should be type of Individual.", "compareIndividual");
            }
            if (this.Score.Count == 0 || ((Individual)compareIndividual).Score.Count == 0) throw new Exception("Individual has no score. Count of its score is equal to 0!");
            if (this.Score[0] > ((Individual)compareIndividual).Score[0]) return 1;
            if (this.Score[0] < ((Individual)compareIndividual).Score[0]) return -1;
            return 0; //if score1 == score2
        }
        
        /// <summary>
        /// Score of each objective function.
        /// </summary>
        public IList<double> Score;
    }
}
