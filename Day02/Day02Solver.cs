namespace Day02;

public class Day02Tests
{
    private readonly ITestOutputHelper Output;
    public Day02Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day02Solver().ExecuteExample1("??");

    [Fact] public void Step2WithExample() => new Day02Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day02Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day02Solver().ExecutePuzzle2());
}

public class Day02Solver : SolverBase
{
    enum Command { Forward, Up, Down }
    List<(Command Command, int Value)> Data;

    protected override void Parse(List<string> data)
    {
        (Command Command, int Value) ParseLine(string line)
        {
            var parts = line.Split(" ");
            var value = int.Parse(parts[1]);
            if (parts[0] == "up")
                return (Command.Up, value);
            if (parts[0] == "down")
                return (Command.Down, value);

            return (Command.Forward, value);
        }

        Data = data.ConvertAll(ParseLine);
    }

    protected override object Solve1()
    {
        var position = 0;
        var depht = 0;
        foreach (var item in Data)
        {
            if (item.Command == Command.Up)
                depht -= item.Value;
            else if (item.Command == Command.Down)
                depht += item.Value;
            else
                position += item.Value;
        }

        return position * depht;
    }

    protected override object Solve2()
    {
        var position = 0;
        var depht = 0;
        var aim = 0;
        foreach (var item in Data)
        {
            if (item.Command == Command.Up)
                aim -= item.Value;
            else if (item.Command == Command.Down)
                aim += item.Value;
            else
            {
                position += item.Value;
                depht += item.Value * aim;
            }
        }

        return position * depht;
    }
}
