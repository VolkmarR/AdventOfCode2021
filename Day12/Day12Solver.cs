namespace Day12;

public class Day12Tests
{
    private readonly ITestOutputHelper Output;
    public Day12Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day12Solver().ExecuteExample1("10");

    [Fact] public void Step2WithExample() => new Day12Solver().ExecuteExample2("36");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day12Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day12Solver().ExecutePuzzle2());
}

public class Day12Solver : SolverBase
{
    const string Start = "start";
    const string End = "end";

    Dictionary<string, List<string>> Data;
    List<List<string>> Paths;

    protected override void Parse(List<string> data)
    {
        void Add(string from, string to)
        {
            if (from == End || to == Start)
                return;

            if (!Data.ContainsKey(from))
                Data[from] = new List<string>();
            Data[from].Add(to);
        }

        Paths = new();
        Data = new();
        foreach (var item in data)
        {
            var parts = item.Split("-", StringSplitOptions.RemoveEmptyEntries);
            Add(parts[0], parts[1]);
            Add(parts[1], parts[0]);
        }
    }

    void Follow1(string node, List<string> visitedNodes)
    {
        if (!Data.ContainsKey(node))
            return;

        visitedNodes.Add(node);

        foreach (var nextNode in Data[node])
        {
            if (nextNode == End)
                Paths.Add(visitedNodes);
            else if (nextNode.ToUpper() == nextNode || !visitedNodes.Contains(nextNode))
                Follow1(nextNode, new(visitedNodes));
        }
    }

    void Follow2(string node, List<string> visitedNodes, bool smallCaveTwice)
    {
        if (!Data.ContainsKey(node))
            return;

        visitedNodes.Add(node);

        foreach (var nextNode in Data[node])
        {
            if (nextNode == End)
                Paths.Add(visitedNodes);
            else
            {
                var useSmallCaveTwice = nextNode.ToLower() == nextNode && visitedNodes.Contains(nextNode);
                if (!(useSmallCaveTwice && smallCaveTwice))
                    Follow2(nextNode, new(visitedNodes), useSmallCaveTwice || smallCaveTwice);
            }
        }
    }


    protected override object Solve1()
    {
        Follow1(Start, new());
        return Paths.Count;
    }


    protected override object Solve2()
    {
        Follow2(Start, new(), false);
        return Paths.Count;
    }
}
