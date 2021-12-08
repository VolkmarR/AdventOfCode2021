namespace Day07;

public class Day07Tests
{
    private readonly ITestOutputHelper Output;
    public Day07Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day07Solver().ExecuteExample1("37");

    [Fact] public void Step2WithExample() => new Day07Solver().ExecuteExample2("168");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day07Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day07Solver().ExecutePuzzle2());
}

public class Day07Solver : SolverBase
{
    List<int> Data;

    protected override void Parse(List<string> data)
    {
        Data = data[0].Split(",").Select(q => int.Parse(q)).ToList();
    }

    protected override object Solve1()
    {
        var min = int.MaxValue;
        foreach (var pos in Data.Distinct())
        {
            var fuel = 0;
            foreach (var item in Data)
                fuel += Math.Abs(item - pos);

            if (min > fuel)
                min = fuel;
        }

        return min;
    }

    protected override object Solve2()
    {
        var Cache = new Dictionary<int, int>();
        int CalcFuel(int pos1, int pos2)
        {
            var key = Math.Abs(pos1 - pos2);
            if (!Cache.TryGetValue(key, out var result))
            {
                result = 0;
                for (int i = 1; i <= key; i++)
                    result += i;
                Cache[key] = result;
            }
            return result;
        }
    
        long min = long.MaxValue;
        for (int pos = Data.Min(); pos < Data.Max(); pos++)
        {
            long fuel = 0;
            foreach (var item in Data)
                fuel += CalcFuel(item, pos);

            if (min > fuel)
                min = fuel;
        }

        return min;
    }
}
