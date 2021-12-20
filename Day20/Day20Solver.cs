namespace Day20;

public class Day20Tests
{
    private readonly ITestOutputHelper Output;
    public Day20Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day20Solver().ExecuteExample1("35");

    [Fact] public void Step2WithExample() => new Day20Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day20Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day20Solver().ExecutePuzzle2());
}

public class Day20Solver : SolverBase
{
    List<char> Algo;
    HashSet<(int X, int Y)> Map = new();
    List<(int X, int Y)> Deltas = new() { (-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1), };

    protected override void Parse(List<string> data)
    {
        var isAlgo = true;
        var algo = "";
        var y = 0;
        foreach (var line in data)
        {
            if (string.IsNullOrEmpty(line))
                isAlgo = false;
            else if (isAlgo)
                algo += line;
            else
            {
                for (int x = 0; x < line.Length; x++)
                    if (line[x] == '#')
                        Map.Add((x, y));
                y++;
            }
        }

        Algo = algo.ToCharArray().ToList();
    }

    void Dump()
    {
        var minX = Map.Min(q => q.X);
        var minY = Map.Min(q => q.Y);
        var maxX = Map.Max(q => q.X);
        var maxY = Map.Max(q => q.Y);

        var sb = new StringBuilder();

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (Map.Contains((x, y)))
                    sb.Append('#');
                else
                    sb.Append('.');

            }
            sb.AppendLine();
        }
        var xxx = sb.ToString();
    }

    void EnhanceImage(int step)
    {
        var swapLight = step % 2 == 1;

        bool GetLight(bool light)
        {
            if (swapLight)
                return !light;
            return light;
        }

        var minX = Map.Min(q => q.X) - 1;
        var minY = Map.Min(q => q.Y) - 1;
        var maxX = Map.Max(q => q.X) + 1;
        var maxY = Map.Max(q => q.Y) + 1;

        var newMap = new HashSet<(int X, int Y)>();

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var pos = 0;
                foreach (var delta in Deltas)
                {
                    pos = pos << 1;
                    if (GetLight(Map.Contains((x + delta.X, y + delta.Y))))
                        pos++;
                }


                if (GetLight(Algo[pos] == '#'))
                    newMap.Add((x, y));
            }
        }

        Map = newMap;
    }

    protected override object Solve1()
    {
        for (int i = 0; i < 2; i++)
        {
            Dump();
            EnhanceImage(i);
        }
        return Map.Count;
    }

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}
