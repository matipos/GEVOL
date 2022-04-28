using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.util
{
    public class Sort : IComparer<double>
    {
        public int Compare(double in1, double in2)
        {
            if (in1 > in2) return 1;
            if (in1 < in2) return -1;
            //return 0; //if score1 == score2
            return 1;   //nothing can be equal becouse the list can't have 2 the same elements.
        }
    }
}
