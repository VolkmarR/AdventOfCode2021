namespace Day08;

public class Day08Tests
{
    private readonly ITestOutputHelper Output;
    public Day08Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day08Solver().ExecuteExample1("26");

    [Fact] public void Step2WithExample() => new Day08Solver().ExecuteExample2("61229");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day08Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day08Solver().ExecutePuzzle2());
}

public class Day08Solver : SolverBase
{
    record Line(List<string> Pattern, List<string> Output);

    List<Line> Data;

    protected override void Parse(List<string> data)
    {
        Line ParseLine(string line)
        {
            var parts = line.Split(" | ");
            return new Line(parts[0].Split(" ").ToList(), parts[1].Split(" ").ToList());
        }

        Data = data.ConvertAll(ParseLine);
    }

    Dictionary<int, List<string>> GetDistOutput()
    {
        var result = new Dictionary<int, List<string>>();
        for (int i = 1; i <= 7; i++)
            result[i] = new List<string>();

        foreach (var item in Data)
            foreach (var part in item.Output)
                result[part.Length].Add(part);
        
        return result;
    }

    protected override object Solve1()
    {
        var dist = GetDistOutput();
        return dist[2].Count + dist[4].Count + dist[3].Count + dist[7].Count;
    }

    string ContainsAllChars(IEnumerable<string> patterns, string pattern)
    {
        var chars = pattern.ToCharArray();
        foreach (var item in patterns)
            if (chars.All(q => item.Contains(q)))
                return item;
        throw new InvalidOperationException();
    }

    string SubtractChars(string charsToSubtract, string fromChar)
        => string.Join("", fromChar.Where(q => !charsToSubtract.Contains(q)));

    string Sort(string digits)
        => string.Join("", digits.ToCharArray().OrderBy(q => q));

    protected override object Solve2()
    {
        long result = 0;

        foreach (var item in Data)
        {
            // Get map
            var patterns = item.Pattern.ToList();
            var map = new Dictionary<int, string>();
            map[8] = patterns.First(q => q.Length == 7);
            map[7] = patterns.First(q => q.Length == 3);
            map[4] = patterns.First(q => q.Length == 4);
            map[1] = patterns.First(q => q.Length == 2);
            patterns.RemoveAll(q => map.ContainsValue(q));

            map[9] = ContainsAllChars(patterns.Where(q => q.Length == 6), map[4] + map[7]);
            patterns.RemoveAll(q => map.ContainsValue(q));

            map[0] = ContainsAllChars(patterns.Where(q => q.Length == 6), map[7]);
            patterns.RemoveAll(q => map.ContainsValue(q));

            map[6] = patterns.First(q => q.Length == 6);
            patterns.RemoveAll(q => map.ContainsValue(q));

            map[3] = ContainsAllChars(patterns, map[1]);
            patterns.RemoveAll(q => map.ContainsValue(q));

            map[5] = ContainsAllChars(patterns, SubtractChars(map[1], map[4]));
            patterns.RemoveAll(q => map.ContainsValue(q));

            map[2] = patterns.First();

            var patternMap = map.ToDictionary(q => Sort(q.Value), q => q.Key);


            // Calc display
            var display = "";
            foreach (var outputItem in item.Output)
                display += patternMap[Sort(outputItem)];

            result += int.Parse(display);
        }

        return result;
    }
}
