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
    Dictionary<(char Start, char End), char> Rules;

    protected override void Parse(List<string> data)
    {
        var regex = new Regex("(?<First>.)(?<Second>.) -> (?<New>.)");
        Start = data[0];

        Rules = data.Skip(2).Select(q => regex.Match(q)).ToDictionary(q => (q.Groups["First"].Value[0], q.Groups["Second"].Value[0]), q => q.Groups["New"].Value[0]);
    }

    List<char> BuildChain(int count)
    {
        var work = new LinkedList<char>(Start.ToCharArray());
        for (int i = 0; i < count; i++)
        {
            var item = work.First;
            while (item.Next != null)
            {
                work.AddAfter(item, Rules[(item.Value, item.Next.Value)]);
                item = item.Next.Next;
            }
        }
        
        return work.ToList();
    }

    protected override object Solve1()
    {
        var data = BuildChain(10).GroupBy(q => q).Select(q => q.Count()).OrderBy(q => q).ToList();
        return data.Last() - data.First();
    }

    protected override object Solve2()
    {
        var data = BuildChain(40).GroupBy(q => q).Select(q => q.LongCount()).OrderBy(q => q).ToList();
        return data.Last() - data.First();
    }
}
