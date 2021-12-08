namespace Day03;

public class Day03Tests
{
    private readonly ITestOutputHelper Output;
    public Day03Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day03Solver().ExecuteExample1("??");
        
    [Fact] public void Step2WithExample() => new Day03Solver().ExecuteExample2("230");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day03Solver().ExecutePuzzle1());
        
    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day03Solver().ExecutePuzzle2());
}

public class Day03Solver : SolverBase
{
    List<List<bool>> Data;

    protected override void Parse(List<string> data)
    {
        List<bool> ParseLine(string line) => line.Select(q => q == '1').ToList();
        Data = data.ConvertAll(ParseLine);
    }

    protected override object Solve1()
    {
        int gamma = 0;
        int epsilon = 0;
        for (int i = 0; i < Data[0].Count; i++)
        {
            gamma = gamma << 1;
            epsilon = epsilon << 1;

            var countTrue = Data.Count(q => q[i]);
            var countFalse = Data.Count - countTrue;

            if (countTrue > countFalse)
                gamma++;
            else
                epsilon++;
        }

        return gamma * epsilon;
    }

    protected override object Solve2()
    {
        int GetValue(bool useMostCommon)
        {
            var filtered = Data;
            for (int i = 0; i < Data[0].Count && filtered.Count > 1; i++)
            {
                var countTrue = filtered.Count(q => q[i]);
                var countFalse = filtered.Count - countTrue;
                bool filter;
                if (useMostCommon)
                    filter = countTrue >= countFalse;
                else
                    filter = countTrue < countFalse;

                filtered = filtered.Where(q => q[i] == filter).ToList();
            }

            var result = 0;
            for (int i = 0; i < filtered[0].Count; i++)
            {
                result = result << 1;
                if (filtered[0][i])
                    result++;
            }
            return result;
        }

        var oxygen = GetValue(true);
        var co2 = GetValue(false);

        return oxygen * co2;
    }
}
