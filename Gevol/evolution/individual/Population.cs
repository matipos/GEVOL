using Gevol.evolution.objective;
using Gevol.evolution.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.individual
{
    
    public class Population : List<Individual>
    {
        
        public Population()
            : base()
        { }

        public Population(Population population)
            : base(population)
        { }
                
    }
}
