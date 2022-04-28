using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gevol.evolution.algorithm;
using Gevol.evolution.individual.binary;
using Gevol.evolution.objective.binary;
using Gevol.evolution.objective;
using Gevol.evolution.evoperator.binary;
using Gevol.evolution.evoperator.replacement;
using Gevol.evolution.termination;
using System.IO;
using Gevol.evolution.algorithm.binary;
using Gevol.evolution.evoperator;
using Gevol.evolution.util;
using Gevol.util;
using Gevol.evolution.algorithm.eda.binary;
using Gevol.evolution.algorithm.real;
using Gevol.evolution.objective.real;
using Gevol.evolution.individual.real;

namespace EvoTest
{
    class EvoTester
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine(DateTime.Now + " testAlgorithm");
            testAlgorithm(@"EvoTester-testAlgorithm.txt");
            Console.WriteLine(DateTime.Now + " testSGA");
            testOneMaxSGA(@"EvoTester-testOneMaxSGA.txt");
            Console.WriteLine(DateTime.Now + " testBinPatternSGA");
            testBinPatternSGA(@"EvoTester-testBinPatternSGA.txt");
            Console.WriteLine(DateTime.Now + " testOneMaxUMDA");
            testOneMaxUMDA(@"EvoTester-testOneMaxUMDA.txt");

            Console.WriteLine(DateTime.Now + " testAckleyESMILambdaKappaRoRechenberg");
            testESMILambdaKappaRoRechenberg(@"EvoTester-testAckleyESMILambdaKappaRoRechenberg.txt", "Ackley");
            Console.WriteLine(DateTime.Now + " testGriewankESMILambdaKappaRoRechenberg");
            testESMILambdaKappaRoRechenberg(@"EvoTester-testGriewankESMILambdaKappaRoRechenberg.txt", "Griewank");
            Console.WriteLine(DateTime.Now + " testPowellESMILambdaKappaRoRechenberg");
            testESMILambdaKappaRoRechenberg(@"EvoTester-testPowellESMILambdaKappaRoRechenberg.txt", "Powell");
            Console.WriteLine(DateTime.Now + " testRastriginESMILambdaKappaRoRechenberg");
            testESMILambdaKappaRoRechenberg(@"EvoTester-testRastriginESMILambdaKappaRoRechenberg.txt", "Rastrigin");
            Console.WriteLine(DateTime.Now + " testSchwefelESMILambdaKappaRoRechenberg");
            testESMILambdaKappaRoRechenberg(@"EvoTester-testSchwefelESMILambdaKappaRoRechenberg.txt", "Schwefel");
            Console.WriteLine(DateTime.Now + " testSphereESMILambdaKappaRoRechenberg");
            testESMILambdaKappaRoRechenberg(@"EvoTester-testSphereESMILambdaKappaRoRechenberg.txt", "Sphere");

            Console.WriteLine(DateTime.Now + " testTrapOneMaxPBIL");
            testTrapOneMaxPBIL(@"EvoTester-testTrapOneMaxPBIL.txt");
            Console.WriteLine(DateTime.Now + " testTrapOneMaxCGA");
            testTrapOneMaxCGA(@"EvoTester-testTrapOneMaxCGA.txt");
            Console.WriteLine(DateTime.Now + " testComposedTrapOneMaxCGA");
            testComposedTrapOneMaxCGA(@"EvoTester-testComposedTrapOneMaxCGA.txt");*/
            Console.WriteLine(DateTime.Now + " testComposedTrapOneMaxECGA");
            testComposedTrapOneMaxECGA(@"EvoTester-testComposedTrapOneMaxECGA.txt");

            Console.WriteLine(DateTime.Now + " Game over. Press any key to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Test ecga algorithm with ComposedTrapOneMax objective function
        /// </summary>
        /// <param name="printResult"></param>
        private static void testComposedTrapOneMaxECGA(string printResult)
        {
            double fhigh = 5;
            double flow = 4;
            int blockLength = 5;
            int numberOfBlocks = 9;
            int chromosomeLength = blockLength * numberOfBlocks;
            int populationSize = 10000;
            int selectionSize = 150;
            int blocksIterationLimit = 3;
            double acceptedProbabilityDifference = 0.02;

            ECGAAlgorithm ecga = new ECGAAlgorithm(populationSize, selectionSize, chromosomeLength, blocksIterationLimit, acceptedProbabilityDifference);
            ecga.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            ecga.ObjFunctions = new ObjectiveFunctions();
            ecga.ObjFunctions.ObjFunctions.Add(new ComposedTrapOneMax(numberOfBlocks, blockLength, fhigh, flow));

            if (printResult != null)
            {
                ecga.stats = new PrintStatistics(File.CreateText(printResult));
                ecga.RunWithStats();
            }
            else
            {
                ecga.Run();
            }
        }

        /// <summary>
        /// Test cga algorithm with ComposedTrapOneMax objective function
        /// </summary>
        /// <param name="printResult"></param>
        private static void testComposedTrapOneMaxCGA(string printResult)
        {
            double fhigh = 5;
            double flow = 4;
            int blockLength = 5;
            int numberOfBlocks = 12;
            int chromosomeLength = blockLength * numberOfBlocks;
            int simulatedPopulationSize = 180;
            double minConvergenceValue = 0.1;
            double maxConvergenceValue = 0.9;

            CGAAlgorithm cga = new CGAAlgorithm(simulatedPopulationSize, chromosomeLength, minConvergenceValue, maxConvergenceValue);
            cga.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            cga.ObjFunctions = new ObjectiveFunctions();
            cga.ObjFunctions.ObjFunctions.Add(new ComposedTrapOneMax(numberOfBlocks, blockLength, fhigh, flow));

            if (printResult != null)
            {
                cga.stats = new PrintStatistics(File.CreateText(printResult));
                cga.RunWithStats();
            }
            else
            {
                cga.Run();
            }
        }

        /// <summary>
        /// Test cga algorithm with TrapOneMax objective function
        /// </summary>
        /// <param name="printResult"></param>
        private static void testTrapOneMaxCGA(string printResult)
        {
            int chromosomeLength = 70;
            int simulatedPopulationSize = 180;
            double minConvergenceValue = 0.1;
            double maxConvergenceValue = 0.9;

            CGAAlgorithm cga = new CGAAlgorithm(simulatedPopulationSize, chromosomeLength, minConvergenceValue, maxConvergenceValue);
            cga.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            cga.ObjFunctions = new ObjectiveFunctions();
            cga.ObjFunctions.ObjFunctions.Add(new TrapOneMax());

            if (printResult != null)
            {
                cga.stats = new PrintStatistics(File.CreateText(printResult));
                cga.RunWithStats();
            }
            else
            {
                cga.Run();
            }
        }

        /// <summary>
        /// Test PBIL for TrapOneMax problem.
        /// </summary>
        /// <param name="printResult"></param>
        private static void testTrapOneMaxPBIL(string printResult)
        {
            PBILAlgorithm pbil = preparePBIL();
            ObjectiveFunctions objFun = new ObjectiveFunctions();
            objFun.ObjFunctions.Add(new TrapOneMax());
            pbil.ObjFunctions = objFun;

            if (printResult != null)
            {
                pbil.stats = new PrintStatistics(File.CreateText(printResult));
                pbil.RunWithStats();
            }
            else
            {
                pbil.Run();
            }
        }

        private static PBILAlgorithm preparePBIL()
        {
            int chromosomeLength = 70;
            int populationSize = 180;
            int numberOfIterations = 400;
            double learningRate = 0.15;
            double mutationSize = 0.08;
            double mutationProbability = 0.2;

            PBILAlgorithm pbil = new PBILAlgorithm(populationSize, chromosomeLength, learningRate, mutationSize, mutationProbability);
            pbil.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            pbil.TermCondition = new NumberOfIterations(numberOfIterations);
            return pbil;
        }

        /// <summary>
        /// Test ESMILambdaKappaRoRechenberg for real problem.
        /// </summary>
        /// <param name="printResult"></param>
        private static void testESMILambdaKappaRoRechenberg(string printResult, string function)
        {
            ESMILambdaKappaRoRechenberg esmi = prepareESMILambdaKappaRoRechenberg();
            ObjectiveFunctions objFun = new ObjectiveFunctions();
            switch (function)
            {
                case "Ackley": objFun.ObjFunctions.Add(new AckleyFunction()); break;
                case "Griewank": objFun.ObjFunctions.Add(new GriewankFunction()); break;
                case "Powell": objFun.ObjFunctions.Add(new PowellFunction()); break;
                case "Rastrigin": objFun.ObjFunctions.Add(new RastriginFunction()); break;
                case "Schwefel": objFun.ObjFunctions.Add(new SchwefelFunction()); break;
                case "Sphere": objFun.ObjFunctions.Add(new SphereFunction()); break;
            }
            esmi.ObjFunctions = objFun;

            if (printResult != null)
            {
                esmi.stats = new PrintStatistics(File.CreateText(printResult));
                esmi.RunWithStats();
            }
            else
            {
                esmi.Run();
            }
        }

        /// <summary>
        /// Prepare ESMILambdaKappaRoRechenberg algorithm.
        /// </summary>
        /// <returns></returns>
        private static ESMILambdaKappaRoRechenberg prepareESMILambdaKappaRoRechenberg()
        {
            int populationSize = 100;
            int childreenPopulationSize = 50;
            int maxAge = 25;
            int parentsPopulationSize = 65;
            int numberOfIterations = 230;
            int chromosomeLength = 17;
            ESMILambdaKappaRoRechenberg esmi = new ESMILambdaKappaRoRechenberg(populationSize, childreenPopulationSize, maxAge, parentsPopulationSize);
            esmi.TermCondition = new NumberOfIterations(numberOfIterations);
            esmi.IndividualProperties = new RealIndividualProperties() { ChromosomeLength = chromosomeLength };
            return esmi;
        }

        /// <summary>
        /// Test UMDA for OneMax problem.
        /// </summary>
        /// <param name="printResult"></param>
        private static void testOneMaxUMDA(string printResult)
        {
            UMDAAlgorithm umda = prepareUMDA();
            ObjectiveFunctions objFun = new ObjectiveFunctions();
            objFun.ObjFunctions.Add(new OneMax());
            umda.ObjFunctions = objFun;

            if (printResult != null)
            {
                umda.stats = new PrintStatistics(File.CreateText(printResult));
                umda.RunWithStats();
            }
            else
            {
                umda.Run();
            }
        }

        private static UMDAAlgorithm prepareUMDA()
        {
            int chromosomeLength = 40;
            int blockSelectionSize = 60;
            int populationSize = 100;
            int numberOfIterations = 400;

            UMDAAlgorithm umda = new UMDAAlgorithm(blockSelectionSize, populationSize);
            umda.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            umda.PopulationSize = populationSize;
            umda.TermCondition = new NumberOfIterations(numberOfIterations);
            return umda;
        }

        /// <summary>
        /// Test sga for binary pattern function. Pattern is generated automatically by Randomizer.
        /// </summary>
        /// <param name="printResult"></param>
        private static void testBinPatternSGA(string printResult)
        {
            int populationSize = 100;
            int chromosomeLength = 40;
            int blockSelectionSize = 60;
            int numberOfIterations = 400;
            double probabilityMutation = 0.2;

            SGAAlgorithm sga = new SGAAlgorithm(blockSelectionSize, probabilityMutation);
            sga.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            sga.PopulationSize = populationSize;
            sga.TermCondition = new NumberOfIterations(numberOfIterations);

            ObjectiveFunctions objFun = new ObjectiveFunctions();
            BinaryPattern pattern = new BinaryPattern(new List<int>());
            for (int i = 0; i < chromosomeLength; i++)
            {
                pattern.Pattern.Add(Randomizer.NextInt(0, 1));
            }
            objFun.ObjFunctions.Add(pattern);
            sga.ObjFunctions = objFun;

            if (printResult != null)
            {
                TextWriter resultFile = File.CreateText(printResult);
                //print pattern
                resultFile.Write("BinaryPattern = ");
                for (int i = 0; i < pattern.Pattern.Count; i++)
                {
                    resultFile.Write(pattern.Pattern[i]);
                }
                resultFile.WriteLine();

                sga.stats = new PrintStatistics(resultFile);
                sga.RunWithStats();
            }
            else
            {
                sga.Run();
            }
        }

        /// <summary>
        /// Test sga for OneMax function
        /// </summary>
        /// <param name="printResult"></param>
        private static void testOneMaxSGA(string printResult)
        {
            int populationSize = 100;
            int chromosomeLength = 40;
            int blockSelectionSize = 60;
            int numberOfIterations = 40;

            SGAAlgorithm sga = new SGAAlgorithm(blockSelectionSize);
            sga.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            sga.PopulationSize = populationSize;
            sga.TermCondition = new NumberOfIterations(numberOfIterations);

            ObjectiveFunctions objFun = new ObjectiveFunctions();
            objFun.ObjFunctions.Add(new OneMax());
            sga.ObjFunctions = objFun;

            if (printResult != null)
            {
                sga.stats = new PrintStatistics(File.CreateText(printResult));
                sga.RunWithStats();
            }
            else
            {
                sga.Run();
            }            
        }

        /// <summary>
        /// Test if algorithm works correctly.
        /// Individual: binary
        /// Operators: negation mutation
        /// Termination condition: number of iterations
        /// </summary>
        /// <param name="printResult">show results</param>
        /// <returns></returns>
        static void testAlgorithm(String printResult)
        {
            int chromosomeLength = 150;
            int numberOfIterations = 200;
            int populationSize = 300;

            Algorithm<BinaryIndividual> algorithm = new Algorithm<BinaryIndividual>();
            BinaryIndividualProperties individualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
            ObjectiveFunctions objFun = new ObjectiveFunctions();

            objFun.ObjFunctions.Add(new OneMax());

            algorithm.IndividualProperties = individualProperties;
            algorithm.ObjFunctions = objFun;
            algorithm.Operators.Add( new NegationMutation() );
            algorithm.PopulationSize = populationSize;
            algorithm.ReplaceOperator = new BestFromUnionReplacement();
            algorithm.TermCondition = new NumberOfIterations(numberOfIterations);

            if (printResult != null)
            {
                algorithm.stats = new PrintStatistics(File.CreateText(printResult));
                algorithm.RunWithStats();
            }
            else
            {
                algorithm.Run();
            }
        }
    }
}
