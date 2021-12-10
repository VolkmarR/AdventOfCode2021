namespace Day10;

public class Day10Tests
{
    private readonly ITestOutputHelper Output;
    public Day10Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day10Solver().ExecuteExample1("26397");

    [Fact] public void Step2WithExample() => new Day10Solver().ExecuteExample2("288957");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day10Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day10Solver().ExecutePuzzle2());
}

public class Day10Solver : SolverBase
{
    List<string> Data;
    Dictionary<char, (char Open, int SyntaxPoints)> ClosePairs;
    Dictionary<char, int> AutoCompletePoints;

    public Day10Solver()
    {
        ClosePairs = new()
        {
            { ')', ('(', 3) },
            { ']', ('[', 57) },
            { '}', ('{', 1197) },
            { '>', ('<', 25137) },
        };

        AutoCompletePoints = new()
        {
            { '(', 1 },
            { '[', 2 },
            { '{', 3 },
            { '<', 4 },
        };
    }

    protected override void Parse(List<string> data)
    {
        Data = data;
    }

    (int SyntaxErrorPoints, long AutoCompletePoints) SyntaxCheck(string line)
    {
        var stack = new Stack<char>();
        foreach (var item in line.ToCharArray())
        {
            if (ClosePairs.ContainsKey(item))
            {
                if (ClosePairs[item].Open != stack.Pop())
                    return (ClosePairs[item].SyntaxPoints, 0);
            }
            else
                stack.Push(item);
        }

        long autoComplete = 0;
        while (stack.Count > 0)
            autoComplete = autoComplete * 5 + AutoCompletePoints[stack.Pop()];

        return (0, autoComplete);
    }

    protected override object Solve1()
    {
        return Data.Sum(q => SyntaxCheck(q).SyntaxErrorPoints);
    }

    protected override object Solve2()
    {
        var points = Data.Select(SyntaxCheck).Where(q => q.SyntaxErrorPoints == 0).Select(q => q.AutoCompletePoints).OrderBy(q => q).ToList();
        return points[points.Count / 2];
    }
}
