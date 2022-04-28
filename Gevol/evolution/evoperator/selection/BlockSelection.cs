using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.selection
{
    /// <summary>
    /// Select M the best individuals from population.
    /// M it's property called Size.
    /// </summary>
    public class BlockSelection : Operator
    {
        private int _size = -10000;

        /// <summary>
        /// Size of new population. 
        /// How many the best individuals is copied to the new population.
        /// </summary>
        public int Size
        {
            get { return _size; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Size", value, "Size must be greater than 0.");
                }
                else { _size = value; }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">How many individuals is choosen.</param>
        public BlockSelection(int size)
        {
            this.Size = size;
        }

        public override Population Apply(Population population)
        {
            Population temp = new Population(population);
            temp.Sort();
            while (temp.Count > Size)
            {
                temp.RemoveAt(temp.Count - 1);
            }
            return temp;
        }
    }
}
