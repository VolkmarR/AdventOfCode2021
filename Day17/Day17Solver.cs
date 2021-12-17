using System.Text.RegularExpressions;

namespace Day17;

public class Day17Tests
{
    private readonly ITestOutputHelper Output;
    public Day17Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day17Solver().ExecuteExample1("45");

    [Fact] public void Step2WithExample() => new Day17Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day17Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day17Solver().ExecutePuzzle2());
}

public class Day17Solver : SolverBase
{
    int X1;
    int X2;
    int Y1;
    int Y2;
    int MaxY;

    protected override void Parse(List<string> data)
    {
        var regex = new Regex(@"x=(?<X1>[-]?\d*)\.\.(?<X2>[-]?\d*), y=(?<Y1>[-]?\d*)\.\.(?<Y2>[-]?\d*)");
        var matches = regex.Match(data[0]);

        X1 = int.Parse(matches.Groups["X1"].Value);
        X2 = int.Parse(matches.Groups["X2"].Value);
        Y1 = int.Parse(matches.Groups["Y1"].Value);
        Y2 = int.Parse(matches.Groups["Y2"].Value);
        MaxY = 0;
    }

    bool HitTargetArea(int xV, int yV)
    {
        int maxY = 0;
        int x = 0;
        int y = 0;
        while (x <= X2 && y >= Y2)
        {
            x += xV;
            y += yV;

            if (y > maxY)
                maxY = y;

            if (xV < 0)
                xV++;
            else if (xV > 0)
                xV--;

            yV--;

            if (X1 <= x && x <= X2 && Y1 <= y && y <= Y2)
            {
                if (MaxY < maxY)
                    MaxY = maxY;
                return true;
            }
        }
        return false;
    }

    protected override object Solve1()
    {
        for (int x = 1; x < X2; x++)
            for (int y = 1; y < 10000; y++)
                HitTargetArea(x, y);

        return MaxY;
    }

    protected override object Solve2()
    {
        long count = 0;
        for (int x = 1; x <= X2; x++)
            for (int y = -10000; y < 10000; y++)
                if (HitTargetArea(x, y))
                    count++;
        return count;
    }
}
