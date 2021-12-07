namespace Day06;

public class Day06Tests
{
    private readonly ITestOutputHelper Output;
    public Day06Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day06Solver().ExecuteExample1((Int64)5934);
        
    [Fact] public void Step2WithExample() => new Day06Solver().ExecuteExample2(26984457539);

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day06Solver().ExecutePuzzle1());
        
    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day06Solver().ExecutePuzzle2());
}

public class Day06Solver : SolverBase
{
    List<short> Data;

    protected override void Parse(List<string> data)
    {
        Data = data[0].Split(",").Select(q => short.Parse(q)).ToList();
    }

    long CalcFish(int days, short start)
    {
        var newFish = new List<int>();
        long result = 1;
        for (int i = days - start; i > 0; i -= 7)
            newFish.Add(i);

        foreach(var item in newFish)
            result += CalcFish(item, 9);

        return result;
    }

    long CountFishs(int days)
    {
        var cache = new Dictionary<int, long>();
        long result = 0;
        foreach (var item in Data)
        {
            if (!cache.ContainsKey(item))
                cache[item] = CalcFish(days, item);
            result += cache[item];
        }
        return result;
    }


    protected override object Solve1()
    {
        return CountFishs(80);
    }

    protected override object Solve2()
    {
        return CountFishs(256);
    }
}
