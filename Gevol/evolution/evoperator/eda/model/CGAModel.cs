﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.evoperator.eda.model
{
    /// <summary>
    /// Model used for CGA algorithm.
    /// This is a list with probability to get 1 for each gene.
    /// </summary>
    public class CGAModel
    {
        public IList<double> probabilities;

        public CGAModel()
        {
            probabilities = new List<double>();
        }

        public override string ToString()
        {
            String result = "";
            for (int i = 0; i < probabilities.Count; i++)
            {
                result = result + probabilities[i] + ";";
            }
            return result;
        }
    }
}
