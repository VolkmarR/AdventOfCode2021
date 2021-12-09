namespace Day09;

public class Day09Tests
{
    private readonly ITestOutputHelper Output;
    public Day09Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day09Solver().ExecuteExample1("15");

    [Fact] public void Step2WithExample() => new Day09Solver().ExecuteExample2("1134");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day09Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day09Solver().ExecutePuzzle2());
}

public class Day09Solver : SolverBase
{
    int[,] Data;
    int MaxX;
    int MaxY;

    protected override void Parse(List<string> data)
    {
        MaxX = data[0].Length;
        MaxY = data.Count;

        Data = new int[MaxX, MaxY];
        for (int y = 0; y < data.Count; y++)
            for (int x = 0; x < data[y].Length; x++)
                Data[x, y] = int.Parse(data[y].Substring(x, 1));
    }

    bool ValidPos(int x, int y)
        => x >= 0 && x < MaxX && y >= 0 && y < MaxY;

    IEnumerable<(int x, int y)> AdjacentPos(int x, int y)
    {
        if (ValidPos(x - 1, y))
            yield return (x - 1, y);
        if (ValidPos(x + 1, y))
            yield return (x + 1, y);
        if (ValidPos(x, y - 1))
            yield return (x, y - 1);
        if (ValidPos(x, y + 1))
            yield return (x, y + 1);
    }


    IEnumerable<int> Adjacent(int x, int y)
    {
        foreach (var pos in AdjacentPos(x, y))
            yield return Data[pos.x, pos.y];
    }

    IEnumerable<(int x, int y)> GetLowPoints()
    {
        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
            {
                var value = Data[x, y];
                if (Adjacent(x, y).All(q => q > value))
                    yield return (x, y);
            }
    }

    int GetBasinSize((int x, int y) pos, int minValue, HashSet<(int x, int y)> visited)
    {
        var result = 0;
        foreach (var apos in AdjacentPos(pos.x, pos.y))
        {
            var value = Data[apos.x, apos.y];
            if (value > minValue && value < 9 && !visited.Contains(apos))
            {
                visited.Add(apos);
                result += 1 + GetBasinSize(apos, value, visited);
            }
        }

        return result;
    }

    int GetBasinSize(int x, int y)
        => 1 + GetBasinSize((x, y), Data[x, y], new HashSet<(int x, int y)>());

    protected override object Solve1()
    {
        var result = 0;
        foreach (var pos in GetLowPoints())
            result += Data[pos.x, pos.y] + 1;

        return result;
    }

    protected override object Solve2()
    {
        var size = new List<int>();
        foreach (var pos in GetLowPoints())
            size.Add(GetBasinSize(pos.x, pos.y));

        var result = 1;
        foreach (var item in size.OrderByDescending(q => q).Take(3))
            result *= item;

        return result;
    }
}
