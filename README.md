# AdventOfCode CSharp Starter Kit

This template can be used as starter kit for the [AdventOfCode](https://www.AdventOfCode.com) advent calendar. Just click the "Use Template" button, enter a name for your repository and you are ready to code. 

To get your started quickly, the starter kit assists you with a generator for initializing the 
source code to solve the puzzles.

## Initialize a new day

Just run the application and enter the number of the day, you want to initialize.

The generator creates the following:
- A new directory for the requested day
- The boilerplate code for the solver
- The Unit-Tests to execute the solver for the example and the puzzle input
- Two empty text files called input.txt and example.txt (which will be opened in the standard text editor)

## Implement the solver

After initializing a day, you have to copy the input data from the website and paste it to the input.txt file 
and implement the Solve1 and Solve2 methods. If you want to check your solution with the example data, you 
need to copy the example data from the website and paste it to the example.txt file and replace the "??" string 
in the Step1WithExample and Step1WithExample unit tests with the example results from the website.

```csharp

// Example for AoC2018, Day 1, part 1

public class Day01Tests
{
    // ...

    [Fact]
    public void Step1WithExample() => new Day01Solver().ExecuteExample1(3); 
       
    // ...
}

public class Day01Solver : SolverBase
{

    List<int> Data;

    protected override void Parse(List<string> data)
        => Data = data.Select(q => int.Parse(q)).ToList();

    // Solver for AoC2018, day 1
    protected override object Solve1()
        =>  data.Sum();

    // ...
}
```

## Executing the solvers

To execute the solvers, you must run the generated unit tests. You can run them using the Test Explorer 
in Visual Studio. 

The Step1WithExample and Step2WithExample unit tests load the example.txt file, call the Parse and Solve method and compare 
the result with the expected result. 

The Step1WithPuzzleInput and Step2WithPuzzleInput unit tests load the input.txt, call the Solve method and save the output to a text 
file called output1.txt or output2.txt.