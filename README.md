# GEVOL Evolutionary Algorithms library

This library is designed to focus on the problem, not on the algorithm. You can easily switch between algorithms or create your own using the same or custom operators. The minimum you have to develop is the objective function to evaluate solutions.  
Evolutionary algorithms are a wide family with different types. It's easy to extend this library with other algorithms or operators by implementing a proper class. Multi-objective evolutionary algorithms are supported by design.

## Implemented algorithms
* Binary
  * SGA
* Real
  * ESMI Lambda Kappa Ro Rechenberg
* EDA (Estimation of Distribution Algorithms)
  * Binary
    * CGA
	* ECGA
	* PBIL
	* UMDA
* Other
  * NSGA2
  * Roulette Selection
  * Rechenberg Success Rule
  * Block Selection
  * and many others...

## Examples
**Run the first algorithm**  
Here is and example how to use existing algorithm. More examples in the EvoTest project.
```csharp
//parameters for the algorithm
bool testRun = true;
double fhigh = 5;
double flow = 4;
int blockLength = 5;
int numberOfBlocks = 12;
int chromosomeLength = blockLength * numberOfBlocks;
int simulatedPopulationSize = 180;
double minConvergenceValue = 0.1;
double maxConvergenceValue = 0.9;

//setup the algorithm
CGAAlgorithm cga = new CGAAlgorithm(simulatedPopulationSize, chromosomeLength, minConvergenceValue, maxConvergenceValue);
//setup individual properties, here the chromosome length
cga.IndividualProperties = new BinaryIndividualProperties() { chromosomeLength = chromosomeLength };
//add objective function, for this example it is the Composed Trap One Max function
cga.ObjFunctions = new ObjectiveFunctions();
cga.ObjFunctions.ObjFunctions.Add(new ComposedTrapOneMax(numberOfBlocks, blockLength, fhigh, flow));

//run with statistics is good for debuggin, but the performance would be lower
if (testRun)
{
	cga.stats = new PrintStatistics(File.CreateText(printResult));
	cga.RunWithStats();
}
else
{
	cga.Run();
}
```  

**Build your own objective function**  
It's crucial to code well objective function to evaluate solution for the problem. This function will be executed thousands or millions times, so it should have very good performance. Depends on what kind of algorithm you are going to use, the code would be different - it will be different for the binary chromosome, different for real numbers chromosome, etc. Here is an example for a common One Max function.  
```csharp
/// <summary>
/// One Max objective function for binary individuals.
/// Chromosome with genes with value only 1 represents global minimum.
/// E.g.:
/// {0,0,0,0,0} = 0
/// {0,1,0,0,1} = -2
/// {1,1,1,1,1} = -5 - global minimum
/// </summary>
public class OneMax : ObjectiveFunction
{
    /// <summary>
    /// Evaluate individual.
    /// </summary>
    /// <param name="individual">Individual to be evaluated.</param>
    /// <returns>Score of the individual.</returns>
    public override double Evaluate(Individual individual)
    {
        if (!(individual is BinaryIndividual))
        {
            throw new ArgumentException("Parameter must be type of BinaryIndividual.", "individual");
        }
        double result = 0;
        for (int i = 0; i < ((IList<int>)individual.Chromosome).Count; i++)
        {
            result -= ((IList<int>)individual.Chromosome)[i];
        }
        return result;
    }
}
```

## License
License conditions in separate LICENSE file.