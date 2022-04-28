using Gevol.evolution.evoperator.real;
using Gevol.evolution.evoperator.replacement;
using Gevol.evolution.evoperator.selection;
using Gevol.evolution.evoperator.util;
using Gevol.evolution.individual;
using Gevol.evolution.individual.real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevol.evolution.algorithm.real
{
    /// <summary>
    /// Implementation of the arlgorithm ES(Mi, Lambda, Kappa, Ro).
    /// Mutation is based on rotation matrix, alhpa and sigma chromosome.
    /// Modifications:
    /// 1. Crossover operator is selected using Rechenberg Success rules
    /// 
    /// Crossover operators:
    /// 1. Global Intermediary Recombination
    /// 2. Local Intermediary Recombination
    /// 3. Uniform Crossover
    /// 4. Mutation based on rotation matrix
    /// 
    /// Algorithm:
    /// P = new population (mi)
    /// while not termination condition
    ///     P = kill old individuals and make older younger ones (P, kappa) 
    ///     P'' = new empty population
    ///     for lambda iterations
    ///         P'' = roulette selection (P, ro)
    ///         I = crossover by rechenberg rule (P'')
    ///         I = mutation (I)
    ///         P' = P' + I
    ///     P = best from P + P' 
    ///     
    /// Parameters:
    /// mi - population size
    /// lambda - size for childreen population to be generated
    /// kappa - maximum age, older individuals will not be taken to next generation
    /// ro - size for parents population
    /// beta - mutation parameter, strength of alpha modification, usually below 0.1
    /// tau_prim - mutation parameter, strength of sigma modification, usually below 0.2
    /// tau - mutation parameter, strength of sigma modification, usually below 0.2
    /// </summary>
    public class ESMILambdaKappaRoRechenberg : Algorithm<RealIndividual>
    {
        private const double BetaDefault = 0.05;
        private const double TauPrimDefault = 0.1;
        private const double TauDefault = 0.1;

        private int _mi = 0;
        private int _lambda = 0;
        private int _kappa = 0;
        private int _ro = 0;
        private double _beta = 0;
        private double _tauPrim = 0;
        private double _tau = 0;

        /// <summary>
        /// Prepare the algorithm.
        /// Default values for:
        /// beta = 0.05
        /// tauPrim = 0.1
        /// tau = 0.1
        /// </summary>
        /// <param name="mi">Population size</param>
        /// <param name="lambda">Childreen population size</param>
        /// <param name="kappa">Maximum age allowed for individuals</param>
        /// <param name="ro">Parents population size</param>
        public ESMILambdaKappaRoRechenberg(int mi, int lambda, int kappa, int ro)
            : this(mi, lambda, kappa, ro, BetaDefault, TauPrimDefault, TauDefault)
        {        }

        /// <summary>
        /// Prepare the algorithm.
        /// </summary>
        /// <param name="mi">Population size</param>
        /// <param name="lambda">Childreen population size</param>
        /// <param name="kappa">Maximum age allowed for individuals</param>
        /// <param name="ro">Parents population size</param>
        /// <param name="beta">How strong alpha should be modified in mutation</param>
        /// <param name="tauPrim">How strong sigma should be modified in mutation</param>
        /// <param name="tau">How strong sigma should be modified in mutation</param>
        public ESMILambdaKappaRoRechenberg(int mi, int lambda, int kappa, int ro, double beta, double tauPrim, double tau)
            : base()
        {
            Mi = mi;
            Lambda = lambda;
            Kappa = kappa;
            Ro = ro;
            Beta = beta;
            TauPrim = tauPrim;
            Tau = tau;

            //create operators
            Operators.Add(new RouletteSelection(_ro));
            RechenbergSuccessRule rechenberg = new RechenbergSuccessRule();
            rechenberg.Operators.Add(new RealGlobalIntermediaryRecombination());
            rechenberg.Operators.Add(new RealLocalIntermediaryRecombination());
            rechenberg.Operators.Add(new RealRandomSelection());
            rechenberg.Operators.Add(new RealUniformCrossover());
            Operators.Add(rechenberg);
            Operators.Add(new RealMutation(_beta, _tau, _tauPrim));
            this.ReplaceOperator = new BestFromUnionReplacement();
        }

        /// <summary>
        /// Create population and initiate Rechenberg 1/5 success rule.
        /// </summary>
        protected override void Init() 
        {
            base.Init();
            RechenbergSuccessRule rechenberg = (RechenbergSuccessRule) Operators[1];
            rechenberg.ObjectiveFunction = ObjFunctions.ObjFunctions[0];
            rechenberg.Init();
        }

        /// <summary>
        /// Make one iteration in the evolutionary loop.
        /// </summary>
        protected override void Iteration()
        {
            //kill old individuals
            //it must be done manually as operator has no access to main population, so good, very old individuals could stay
            this.Population = ManageIndividualsAge(this.Population);
            WorkPopulation.Clear();         //here new individuals will be stored

            //produce new individuals
            for(int i = 0; i < _lambda; i++)
            {
                WorkPopulation.Add(Reproduction(Population));
            }
            
            this.ObjFunctions.Evaluate(WorkPopulation);
            Population = ReplaceOperator.Apply(WorkPopulation, Population);
        }

        /// <summary>
        /// Remove from population individuals with age > kappa
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        protected Population ManageIndividualsAge(Population population)
        {
            foreach(RealIndividual individual in population)
            {
                RealIndividualChromosome chromosome = (RealIndividualChromosome)individual.Chromosome;
                if (chromosome.Age > _kappa)
                    population.Remove(individual);
                else
                    chromosome.Age++;
            }
            return population;
        }

        /// <summary>
        /// Select parents from input population.
        /// Apply crossover by Rechenber 1/5 success rule
        ///     the result of crossover should be only one individual
        /// Mutate the individual
        /// </summary>
        /// <param name="population"></param>
        /// <returns>New real individual</returns>
        protected RealIndividual Reproduction(Population population)
        {
            Population workPopulation = Operators[0].Apply(population); //roulette selection
            workPopulation = Operators[1].Apply(workPopulation);        //crossover by Rechenberg rule
            workPopulation = Operators[2].Apply(workPopulation);        //mutation
            if (workPopulation.Count != 1)
                throw new Exception("Produced population in reproduction has wrong size: " + workPopulation.Count + ", should be only one individual present.");
            return (RealIndividual) workPopulation[0];
        }

        //--------------------- getters & setters

        /// <summary>
        /// Population size.
        /// </summary>
        public int Mi
        {
            get { return _mi; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Mi", value, "Size must be greater than 0.");
                }
                else { _mi = value; this.PopulationSize = value; }
            }
        }

        /// <summary>
        /// Childreen population size
        /// </summary>
        public int Lambda
        {
            get { return _lambda; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Lambda", value, "Size must be greater than 0.");
                }
                else { _lambda = value; }
            }
        }

        /// <summary>
        /// Maximum age allowed to get by individuals.
        /// </summary>
        public int Kappa
        {
            get { return _kappa; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Kappa", value, "Size must be greater than 0.");
                }
                else { _kappa = value; }
            }
        }

        /// <summary>
        /// Parents population size.
        /// </summary>
        public int Ro
        {
            get { return _ro; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Ro", value, "Size must be greater than 0.");
                }
                else { _ro = value; }
            }
        }

        /// <summary>
        /// It determines how strong alpha values should be mutated.
        /// </summary>
        public double Beta
        {
            get { return _beta; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Beta", value, "Size must be greater than 0.");
                }
                else { _beta = value; }
            }
        }

        /// <summary>
        /// It determines how strong sigma values should be mutated. 
        /// </summary>
        public double TauPrim
        {
            get { return _tauPrim; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("TauPrim", value, "Size must be greater than 0.");
                }
                else { _tauPrim = value; }
            }
        }

        /// <summary>
        /// It determines how strong sigma values should be mutated. 
        /// </summary>
        public double Tau
        {
            get { return _tau; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Tau", value, "Size must be greater than 0.");
                }
                else { _tau = value; }
            }
        }
    }
}
