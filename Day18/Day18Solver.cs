namespace Day18;

public class Day18Tests
{
    private readonly ITestOutputHelper Output;
    public Day18Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day18Solver().ExecuteExample1("4140");

    [Fact] public void Step2WithExample() => new Day18Solver().ExecuteExample2("3993");

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day18Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day18Solver().ExecutePuzzle2());
}

public class Day18Solver : SolverBase
{
    class Number
    {
        public Number Parent { get; set; }
        public Number Left { get; set; }
        public Number Right { get; set; }
        public int? Value { get; set; }
        public int Level
        {
            get
            {
                var level = 0;
                var item = Parent;
                while (item != null)
                {
                    level++;
                    item = item.Parent;
                }

                return level;
            }
        }
    }


    List<Number> Data;

    protected override void Parse(List<string> data)
    {
        (Number number, int endIndex) ParseNumber(string line, int startIndex, Number parent)
        {
            int endIndex;
            if (line[startIndex] == '[')
            {
                var result = new Number { Parent = parent };
                (result.Left, endIndex) = ParseNumber(line, startIndex + 1, result);
                (result.Right, endIndex) = ParseNumber(line, endIndex + 1, result);
                return (result, endIndex + 1);
            }

            endIndex = startIndex + 1;
            while (endIndex < line.Length && char.IsDigit(line[endIndex]))
                endIndex++;
            return (new Number { Parent = parent, Value = int.Parse(line.Substring(startIndex, endIndex - startIndex)) }, endIndex);

        }

        Number ParseLine(string line)
        {
            (var result, _) = ParseNumber(line, 0, null);
            return result;

        }
        Data = data.ConvertAll(ParseLine);
    }

    IEnumerable<Number> GetLeafs(Number root)
    {
        if (root.Left != null && root.Right != null)
        {
            foreach (var item in GetLeafs(root.Left))
                yield return item;
            foreach (var item in GetLeafs(root.Right))
                yield return item;
        }
        else
            yield return root;
    }

    Number Clone(Number number, Number parent = null)
    {
        var result = new Number { Parent = parent };
        if (number.Value != null)
            result.Value = number.Value;
        else
        {
            result.Left = Clone(number.Left, result);
            result.Right = Clone(number.Right, result);
        }

        return result;
    }


    Number Add(Number number1, Number number2)
    {
        var result = new Number { Left = number1, Right = number2 };
        result.Left.Parent = result;
        result.Right.Parent = result;
        return result;
    }

    bool Explode(Number number)
    {
        var leafs = GetLeafs(number).ToList();
        var explodeIndex = leafs.FindIndex(q => q.Parent.Level >= 4);
        if (explodeIndex > -1)
        {
            if (explodeIndex > 0)
                leafs[explodeIndex - 1].Value += leafs[explodeIndex].Value;
            if (explodeIndex + 1 < leafs.Count - 1)
                leafs[explodeIndex + 2].Value += leafs[explodeIndex + 1].Value;

            var leaf = leafs[explodeIndex].Parent;
            var newNumber = new Number() { Value = 0 };

            if (leaf.Parent.Left == leaf)
            {
                newNumber.Parent = leaf.Parent.Left.Parent;
                leaf.Parent.Left = newNumber;
            }
            else
            {
                newNumber.Parent = leaf.Parent.Right.Parent;
                leaf.Parent.Right = newNumber;
            }

            return true;
        }
        return false;
    }

    bool Split(Number number)
    {
        var leaf = GetLeafs(number).FirstOrDefault(q => q.Value >= 10);
        if (leaf != null)
        {
            var leftValue = (int)(leaf.Value / 2);
            leaf.Left = new Number() { Value = leftValue, Parent = leaf };
            leaf.Right = new Number() { Value = leaf.Value - leftValue, Parent = leaf };
            leaf.Value = null;
        }

        return leaf != null;
    }

    void Reduce(Number number)
    {
        if (Explode(number))
            Reduce(number);
        else if (Split(number))
            Reduce(number);
    }

    int Magnitude(Number number)
        => (number.Left.Value == null ? Magnitude(number.Left) : number.Left.Value.Value) * 3 +
            (number.Right.Value == null ? Magnitude(number.Right) : number.Right.Value.Value) * 2;
    protected override object Solve1()
    {
        var result = Clone(Data.First());
        foreach (var item in Data.Skip(1))
        {
            result = Add(result, Clone(item));
            Reduce(result);
        }

        return Magnitude(result);
    }

    protected override object Solve2()
    {
        var max = 0;
        foreach (var item1 in Data)
            foreach (var item2 in Data.Where(q => q != item1))
            {
                var result = Add(Clone(item1), Clone(item2));
                Reduce(result);

                var numbers = GetLeafs(result).ToList();
                var mag = Magnitude(result);
                if (mag > max)
                    max = mag;
            }

        return max;
    }
}
