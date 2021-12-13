namespace Day13;

public class Day13Tests
{
    private readonly ITestOutputHelper Output;
    public Day13Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day13Solver().ExecuteExample1("17");

    [Fact] public void Step2WithExample() => new Day13Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day13Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day13Solver().ExecutePuzzle2());
}

public class Day13Solver : SolverBase
{
    record Fold(bool IsX, int Value);
    record Pos(int X, int Y);

    HashSet<Pos> Data;
    List<Fold> Folds;

    protected override void Parse(List<string> data)
    {
        Pos ParseCoords(string line)
        {
            var parts = line.Split(",");
            return new Pos(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        Fold ParseFold(string line)
        {
            var parts = line.Substring(11).Split("=");
            return new Fold(parts[0] == "x", int.Parse(parts[1]));
        }

        Data = new();
        Folds = new();
        var isCoords = true;
        for (int i = 0; i < data.Count; i++)
        {
            if (string.IsNullOrEmpty(data[i]))
                isCoords = false;
            else if (isCoords)
                Data.Add(ParseCoords(data[i]));
            else
                Folds.Add(ParseFold(data[i]));
        }
    }

    void DoFold(Fold fold)
    {
        int FoldPos(int value)
            => value > fold.Value ? 2 * fold.Value - value : value;

        var newData = new HashSet<Pos>();
        foreach (var item in Data)
        {
            var x = item.X;
            var y = item.Y;
            if (fold.IsX)
                x = FoldPos(x);
            else
                y = FoldPos(y);

            newData.Add(new Pos(x, y));
        }

        Data = newData;
    }

    string Dump()
    {
        var minX = Data.Min(q => q.X);
        var minY = Data.Min(q => q.Y);
        var maxX = Data.Max(q => q.X);
        var maxY = Data.Max(q => q.Y);

        var sb = new StringBuilder();
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
                sb.Append(Data.Contains(new Pos(x, y)) ? "#" : " ");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    protected override object Solve1()
    {
        DoFold(Folds.First());
        return Data.Count();
    }

    protected override object Solve2()
    {
        foreach (var item in Folds)
            DoFold(item);

        return Dump();
    }
}
