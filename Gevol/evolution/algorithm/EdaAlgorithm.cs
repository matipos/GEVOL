using Gevol.evolution.evoperator;
using Gevol.evolution.evoperator.eda;
using Gevol.evolution.individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm
{
    /// <summary>
    /// Algorithm for Estimation of Distribution Algorihtms.
    /// 
    /// Standard EDA algorithm is defined according to the pseudocode:
    /// 
    /// generate population
    /// evaluate population 
    /// while satisfied condition is not achieved
    ///    work population = population
    ///    apply each operator on work population
    ///    evaluate work population
    ///    
    /// Replacement operator is not used in EDA algorithm.
    /// One of the operator must be EdaOperator. This operator will calculate the model 
    /// and it will generate new population.
    /// </summary>
    /// <typeparam name="IndividualType"></typeparam>
    public class EdaAlgorithm<IndividualType> : Algorithm<IndividualType> where IndividualType : Individual, new()
    {
        public object Model;   //calculated model as result of the algorithm
        public virtual new ReplacementOperator ReplaceOperator { 
            get { return null; }
            set { throw new MissingMemberException("In EDA algorihtms Replacement Operator is not used."); } 
        }

        /// <summary>
        /// Run the algorithm and start evolution.
        /// </summary>
        public override void Run()
        {
            Init();
            // Do evolution.
            while (!TermCondition.isSatisfied(Population, Model))
            {
                Iteration();
            }
        }

        /// <summary>
        /// Run algorithm with printing statistics after each iteration.
        /// </summary>
        public override void RunWithStats()
        {
            Init();
            // Do evolution.
            while (!TermCondition.isSatisfied(Population, Model))
            {
                IterationWithStats();
            }
            stats.Dispose();
        }

        /// <summary>
        /// Make one iteration in the evolutionary loop.
        /// </summary>
        protected override void Iteration()
        {
            WorkPopulation = new Population(this.Population);  //Create copy of population

            for (int i = 0; i < Operators.Count; i++)
            {
                WorkPopulation = Operators[i].Apply(WorkPopulation);
                if (Operators[i] is EdaOperator)
                {
                    Model = ((EdaOperator)Operators[i]).Model;
                }
            }
            Population = WorkPopulation;    //replacement for EDA
            this.ObjFunctions.Evaluate(WorkPopulation);
        }

        /// <summary>
        /// Make one iteration in the evolutionary loop and print statistics.
        /// </summary>
        protected override void IterationWithStats()
        {
            if (stats == null)
            {
                throw new Exception("PrintStatistics object is not set to perform run with statistics.");
            }
            Iteration();
            stats.PrintPopulation(Population);
            stats.PrintEdaModel(Model);
        }
    }
}
