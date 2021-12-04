namespace Day04;

public class Day04Tests
{
    private readonly ITestOutputHelper Output;
    public Day04Tests(ITestOutputHelper output) => Output = output;

    [Fact] public void Step1WithExample() => new Day04Solver().ExecuteExample1(4512);

    [Fact] public void Step2WithExample() => new Day04Solver().ExecuteExample2(1924);

    [Fact] public void Step1WithPuzzleInput() => Output.WriteLine(new Day04Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => Output.WriteLine(new Day04Solver().ExecutePuzzle2());
}

public class Day04Solver : SolverBase
{
    List<int> Data;
    List<int[]> Boards;

    int Index(int x, int y) => y * 5 + x;

    IEnumerable<int> RowFromPos(int pos)
    {
        var y = (int)(pos / 5);
        for (int x = 0; x < 5; x++)
            yield return Index(x, y);
    }

    IEnumerable<int> ColumnFromPos(int pos)
    {
        var x = pos - (int)(pos / 5) * 5;
        for (int y = 0; y < 5; y++)
            yield return Index(x, y);
    }

    protected override void Parse(List<string> data)
    {
        Data = data.First().Split(",").Select(q => int.Parse(q)).ToList();
        Boards = new();
        var i = 2;
        while (i < data.Count)
        {
            var board = new int[5 * 5];
            Boards.Add(board);
            for (var y = 0; y < 5; y++)
            {
                var line = data[i + y].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(q => int.Parse(q)).ToArray();
                for (var x = 0; x < 5; x++)
                    board[Index(x, y)] = line[x];
            }
            i += 6;
        }
    }

    private int Play(bool stopOnFirst)
    {
        var result = 0;
        var games = Boards.Zip(Enumerable.Range(0, Boards.Count).Select(_ => new bool[5 * 5]).ToList(), (a, b) => (board: a, hits: b)).ToList();
        var wonGame = new HashSet<int[]>();
        foreach (var item in Data)
        {
            foreach (var game in games)
            {
                var pos = Array.IndexOf(game.board, item);
                if (pos > -1 && !wonGame.Contains(game.board))
                {
                    game.hits[pos] = true;

                    if (RowFromPos(pos).All(q => game.hits[q]) || ColumnFromPos(pos).All(q => game.hits[q]))
                    {
                        result = Enumerable.Range(0, game.hits.Length).Where(q => !game.hits[q]).Sum(q => game.board[q]) * item;
                        wonGame.Add(game.board);
                        if (stopOnFirst)
                            return result;
                    }
                }
            }
        }
        return result;
    }

    protected override object Solve1()
    {
        return Play(true);
    }

    protected override object Solve2()
    {
        return Play(false);
    }
}
