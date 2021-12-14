using System.Text.RegularExpressions;

namespace Day14;

public class Day14Tests
{
    private readonly ITestOutputHelper Output;
    public Day14Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day14Solver().ExecuteExample1("1588");

    [Fact] public void Step2WithExample() => new Day14Solver().ExecuteExample2("2188189693529");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day14Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day14Solver().ExecutePuzzle2());
}

public class Day14Solver : SolverBase
{
    string Start;
    Dictionary<string, char> Rules;

    protected override void Parse(List<string> data)
    {
        var regex = new Regex("(?<First>.)(?<Second>.) -> (?<New>.)");
        Start = data[0];

        Rules = data.Skip(2).Select(q => regex.Match(q)).ToDictionary(q => q.Groups["First"].Value + q.Groups["Second"].Value, q => q.Groups["New"].Value[0]);
    }

    public static void AddCount<T>(Dictionary<T, long> countDictionary, T key, long value)
    {
        countDictionary.TryGetValue(key, out var current);
        countDictionary[key] = current + value;
    }

    // Solution was found after the hint to count pairs from reddit 

    long BuildChain(int count)
    {
        var pairs = new Dictionary<string, long>();
        for (int i = 0; i < Start.Length - 1; i++)
            AddCount(pairs, Start.Substring(i, 2), 1);

        var nextPairs = new Dictionary<string, long>();

        for (int i = count; i > 0; i--)
        {
            nextPairs.Clear();
            foreach (var item in pairs)
            {
                var ruleChar = Rules[item.Key];
                var value1 = item.Key.Substring(0, 1) + ruleChar;
                var value2 = ruleChar + item.Key.Substring(1, 1);
                AddCount(nextPairs, value1, item.Value);
                AddCount(nextPairs, value2, item.Value);
            }

            var dummy = pairs;
            pairs = nextPairs;
            nextPairs = dummy;
        }

        var counter = new Dictionary<char, long>();
        foreach (var item in pairs)
            AddCount(counter, item.Key[1], item.Value);

        AddCount(counter, Start.First(), 1);
        return counter.Max(q => q.Value) - counter.Min(q => q.Value);
    }

    protected override object Solve1()
        => BuildChain(10);

    protected override object Solve2()
        => BuildChain(40);

}
