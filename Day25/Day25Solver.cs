namespace Day25;

public class Day25Tests
{
    private readonly ITestOutputHelper Output;
    public Day25Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day25Solver().ExecuteExample1("58");

    [Fact] public void Step2WithExample() => new Day25Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day25Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day25Solver().ExecutePuzzle2());
}

public class Day25Solver : SolverBase
{
    char[,] Data;
    int MaxX;
    int MaxY;
    protected override void Parse(List<string> data)
    {
        MaxX = data[0].Length;
        MaxY = data.Count;
        Data = new char[MaxX, MaxY];
        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                Data[x, y] = data[y][x];
    }

    bool Move()
    {
        var moved = false;
        var swaps = new List<(int x1, int y1, int x2, int y2)>();

        void CheckForSwap(int x1, int y1, int x2, int y2, char value1)
        {
            if (Data[x1, y1] == value1 && Data[x2, y2] == '.')
                swaps.Add((x1, y1, x2, y2));
        }

        void Swap()
        {
            moved = moved || swaps.Count > 0;
            foreach (var (x1, y1, x2, y2) in swaps)
            {
                var buffer = Data[x1, y1];
                Data[x1, y1] = Data[x2, y2];
                Data[x2, y2] = buffer;
            }
            swaps.Clear();
        }

        // go east
        for (int y = 0; y < MaxY; y++)
        {
            for (int x = 0; x < MaxX - 1; x++)
                CheckForSwap(x, y, x + 1, y, '>');

            CheckForSwap(MaxX - 1, y, 0, y, '>');
            Swap();
        }

        // go south
        for (int x = 0; x < MaxX; x++)
        {
            for (int y = 0; y < MaxY - 1; y++)
                CheckForSwap(x, y, x, y + 1, 'v');

            CheckForSwap(x, MaxY - 1, x, 0, 'v');
            Swap();
        }

        Dump();

        return moved;
    }

    void Dump()
    {
        var sb = new StringBuilder();
        for (int y = 0; y < MaxY; y++)
        {
            for (int x = 0; x < MaxX; x++)
                sb.Append(Data[x, y]);
            sb.AppendLine();
        }
        var dummy = sb.ToString();
    }

    protected override object Solve1()
    {
        int count = 1;
        while (Move())
            count++;

        return count;
    }

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}
