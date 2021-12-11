namespace Day11;

public class Day11Tests
{
    private readonly ITestOutputHelper Output;
    public Day11Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day11Solver().ExecuteExample1("1656");

    [Fact] public void Step2WithExample() => new Day11Solver().ExecuteExample2("195");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day11Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day11Solver().ExecutePuzzle2());
}

public class Day11Solver : SolverBase
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

        if (ValidPos(x - 1, y - 1))
            yield return (x - 1, y - 1);
        if (ValidPos(x - 1, y + 1))
            yield return (x - 1, y + 1);
        if (ValidPos(x + 1, y - 1))
            yield return (x + 1, y - 1);
        if (ValidPos(x + 1, y + 1))
            yield return (x + 1, y + 1);
    }

    IEnumerable<(int x, int y)> AllCoords()
    {
        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                yield return (x, y);
    }


    void Flash((int x, int y) pos)
    {
        Data[pos.x, pos.y] = 0;
        foreach (var item in AdjacentPos(pos.x, pos.y))
        {
            if (Data[item.x, item.y] > 0)
                Data[item.x, item.y]++;
        }
    }

    int SimulateRound()
    {
        var result = 0;

        foreach (var item in AllCoords())
            Data[item.x, item.y]++;

        List<(int x, int y)> flashCoords;
        do
        {
            flashCoords = AllCoords().Where(q => Data[q.x, q.y] > 9).ToList();
            result += flashCoords.Count;
            foreach (var item in flashCoords)
                Flash(item);
        } while (flashCoords.Count > 0);

        return result;
    }

    protected override object Solve1()
    {
        var result = 0;
        for (int i = 0; i < 100; i++)
            result += SimulateRound();
        return result;
    }

    protected override object Solve2()
    {
        var result = 0;
        do
        {
            SimulateRound();
            result++;
        } while (AllCoords().Any(q => Data[q.x, q.y] > 0));
        return result;
    }
}
