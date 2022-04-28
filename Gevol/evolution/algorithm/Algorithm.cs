using Gevol.evolution.evoperator;
using Gevol.evolution.individual;
using Gevol.evolution.objective;
using Gevol.evolution.termination;
using Gevol.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm
{
    /// <summary>
    /// Simple evolutionary algorithm. 
    /// The class is able to run evolution and return results.
    /// Before run algorithm must be defined: operators, replacement operator, objective function(s), termination condition, size of population.
    ///
    /// Standard algorithm is defined as:
    /// 
    /// generate population
    /// evaluate population 
    /// while satisfied condition is not achieved
    ///    work population = population
    ///    apply each operator on work population
    ///    evaluate work population
    ///    apply replacement operator
    /// </summary>
    public class Algorithm<IndividualType> where IndividualType : Individual, new()
    {
        //Properties:
        public IList<Operator> Operators { get; set; }
        public ReplacementOperator ReplaceOperator { get; set; }
        public ObjectiveFunctions ObjFunctions { get; set; }
        public TerminationCondition TermCondition { get; set; }
        public Population Population { get; set; }
        protected Population WorkPopulation { get; set; }
        public int PopulationSize { get; set; }
        public object IndividualProperties { get; set; }    //Individual properties used to generate new individual.

        public PrintStatistics stats = null;

        public Algorithm()
        {
            this.Operators = new List<Operator>();
            this.Population = new Population();
            this.WorkPopulation = new Population();
        }

        /// <summary>
        /// Run the algorithm and start evolution.
        /// </summary>
        public virtual void Run()
        {
            Init();
            // Do evolution.
            while (!TermCondition.isSatisfied(Population))
            {
                Iteration();
            }
        }

        /// <summary>
        /// Run algorithm with printing statistics after each iteration.
        /// </summary>
        public virtual void RunWithStats()
        {
            Init();
            // Do evolution.
            while (!TermCondition.isSatisfied(Population))
            {
                IterationWithStats();
            }
            stats.Dispose();
        }

        /// <summary>
        /// Make one iteration in the evolutionary loop.
        /// </summary>
        protected virtual void Iteration()
        {
            WorkPopulation = new Population(this.Population);  //Create copy of population

            for (int i = 0; i < Operators.Count; i++)
            {
                WorkPopulation = Operators[i].Apply(WorkPopulation);
            }
            this.ObjFunctions.Evaluate(WorkPopulation);  
            Population = ReplaceOperator.Apply(WorkPopulation, Population);
        }

        /// <summary>
        /// Make one iteration in the evolutionary loop and print statistics.
        /// </summary>
        protected virtual void IterationWithStats()
        {
            if (stats == null)
            {
                throw new Exception("PrintStatistics object is not set to perform run with statistics.");
            }
            Iteration();
            stats.PrintPopulation(Population);
        }

        /// <summary>
        /// Initialize algorithm.
        /// </summary>
        protected virtual void Init()
        {
            // Create population.   
            Population.Clear();
            WorkPopulation.Clear();
            Individual individual = new IndividualType();
            for (int i = 0; i < this.PopulationSize; i++)
            {
                Population.Add(individual.GenerateIndividual(IndividualProperties));
            }
            //evaluate population
            this.ObjFunctions.Evaluate(Population);  
        }
    }
}
