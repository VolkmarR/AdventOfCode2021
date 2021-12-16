namespace Day16;

public class Day16Tests
{
    private readonly ITestOutputHelper Output;
    public Day16Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day16Solver().ExecuteExample1("31");

    [Fact] public void Step2WithExample() => new Day16Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day16Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day16Solver().ExecutePuzzle2());
}

public class Day16Solver : SolverBase
{
    string Data;
    int Pos;
    int VersionSum = 0;

    protected override void Parse(List<string> data)
    {
        Pos = 0;
        var sb = new StringBuilder();

        foreach (var item in data[0])
        {
            var value = Convert.ToString(Convert.ToInt16(item.ToString(), 16), 2);
            sb.Append(value.PadLeft(4, '0'));
        }
        Data = sb.ToString();
    }

    string GetValue(int length)
    {
        var result = Data.Substring(Pos, length);
        Pos += length;
        return result;
    }

    int GetNumber(int length)
        => Convert.ToInt32(GetValue(length), 2);

    long GetLiteralValue()
    {
        var bitValue = "";
        string indicator;
        do
        {
            indicator = GetValue(1);
            bitValue += GetValue(4);
        } while (indicator == "1");

        return Convert.ToInt64(bitValue, 2);
    }

    long Product(List<long> values)
    {
        long result = 1;
        foreach (var item in values)
            result *= item;
        return result;
    }

    long ParseOperator(int typeID)
    {
        var values = new List<long>();
        if (GetValue(1) == "0")
        {
            var length = Convert.ToInt32(GetValue(15), 2);
            var end = Pos + length;
            while (Pos < end)
                values.Add(ParsePacket());
        }
        else
        {
            var count = Convert.ToInt32(GetValue(11), 2);
            for (int i = 0; i < count; i++)
                values.Add(ParsePacket());
        }

        return typeID switch
        {
            0 => values.Sum(),
            1 => Product(values),
            2 => values.Min(),
            3 => values.Max(),
            5 => values[0] > values[1] ? 1 : 0,
            6 => values[0] < values[1] ? 1 : 0,
            7 => values[0] == values[1] ? 1 : 0,
            _ => 0
        };
    }

    long ParsePacket()
    {
        VersionSum += GetNumber(3);
        var typeID = GetNumber(3);
        if (typeID == 4)
            return GetLiteralValue();
        else
            return ParseOperator(typeID);
    }

    protected override object Solve1()
    {
        ParsePacket();
        return VersionSum;
    }

    protected override object Solve2()
        => ParsePacket();
}
