using System.Text.RegularExpressions;

namespace Day05;

public class Day05Tests
{
    private readonly ITestOutputHelper Output;
    public Day05Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day05Solver().ExecuteExample1("5");

    [Fact] public void Step2WithExample() => new Day05Solver().ExecuteExample2("12");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day05Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day05Solver().ExecutePuzzle2());
}

public class Day05Solver : SolverBase
{
    record Pos(int X, int Y);
    record Line(Pos Start, Pos End);

    List<Line> Data;

    protected override void Parse(List<string> data)
    {
        var lineRegex = new Regex(@"(?<x1>\d*),(?<y1>\d*) -> (?<x2>\d*),(?<y2>\d*)");
        Line ParseLine(string line)
        {
            var match = lineRegex.Match(line).Groups;
            var start = new Pos(int.Parse(match["x1"].Value), int.Parse(match["y1"].Value));
            var end = new Pos(int.Parse(match["x2"].Value), int.Parse(match["y2"].Value));
            return new Line(start, end);
        }

        Data = data.ConvertAll(ParseLine);
    }

    IEnumerable<Pos> LinePosList(Line line, int deltaX, int deltaY)
    {
        var pos = line.Start;
        yield return pos;
        while (pos.X != line.End.X || pos.Y != line.End.Y)
        {
            pos = new Pos(pos.X + deltaX, pos.Y + deltaY);
            yield return pos;
        }
    }

    void IncrementMapPos(Dictionary<Pos, int> map, Pos pos)
    {
        if (!map.TryGetValue(pos, out var value))
            value = 0;

        map[pos] = ++value;
    }

    int GetDelta(int value1, int value2)
    {
        if (value1 > value2)
            return -1;
        if (value1 < value2)
            return 1;
        return 0;
    }

    protected override object Solve1()
    {
        var map = new Dictionary<Pos, int>();
        foreach (var item in Data)
        {
            var deltaX = GetDelta(item.Start.X, item.End.X);
            var deltaY = GetDelta(item.Start.Y, item.End.Y);
            if ((deltaX == 0 || deltaY == 0) && deltaX != deltaY)
                foreach (var pos in LinePosList(item, deltaX, deltaY))
                    IncrementMapPos(map, pos);
        }
        return map.Count(q => q.Value >= 2);
    }

    protected override object Solve2()
    {
        var map = new Dictionary<Pos, int>();
        foreach (var item in Data)
        {
            var deltaX = GetDelta(item.Start.X, item.End.X);
            var deltaY = GetDelta(item.Start.Y, item.End.Y);
            foreach (var pos in LinePosList(item, deltaX, deltaY))
                IncrementMapPos(map, pos);
        }
        return map.Count(q => q.Value >= 2);
    }
}
