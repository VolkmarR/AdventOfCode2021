namespace Day01;

public class Day01Tests
{
    private readonly ITestOutputHelper Output;
    public Day01Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day01Solver().ExecuteExample1("??");

    [Fact] public void Step2WithExample() => new Day01Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day01Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day01Solver().ExecutePuzzle2());
}

public class Day01Solver : SolverBase
{
    List<int> Data;

    protected override void Parse(List<string> data)
    {
        Data = data.Select(q => int.Parse(q)).ToList();
    }

    protected override object Solve1()
    {
        var count = 0;
        for (var i = 1; i < Data.Count; i++)
            if (Data[i - 1] < Data[i])
                count++;

        return count;
    }

    protected override object Solve2()
    {
        int WindowSum(int start)
            => Data[start] + Data[start + 1] + Data[start + 2];

        var count = 0;
        for (var i = 1; i < Data.Count - 2; i++)
            if (WindowSum(i - 1) < WindowSum(i))
                count++;

        return count;
    }
}
