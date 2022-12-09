var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");

Move ParseMove(string line)
{
    var p = line.Split(' ');
    if (p.Length != 2)
        throw new ApplicationException("Unexpected input");

    Direction dir = p[0] switch
    {
        "R" => Direction.Right,
        "L" => Direction.Left,
        "U" => Direction.Up,
        "D" => Direction.Down,
        _ => throw new ApplicationException($"Invalid move '{p[0]}'")
    };
    if (!int.TryParse(p[1], out int dist))
        throw new ApplicationException($"Invalid distance {dist}");
    return new Move(dir, dist);
}

var head = new Pos(0, 0);
var tail = new Pos(0, 0);
var moves = input.Select(ParseMove);
HashSet<Pos> positions = new() {tail};


foreach (Move m in moves)
{
    for (int i = 0; i < m.Distance; i++)
    {
        var prev = head;
        head = m.Direction switch
        {
            Direction.Right => head with {X = head.X + 1},
            Direction.Left => head with {X = head.X - 1},
            Direction.Down => head with {Y = head.Y + 1},
            Direction.Up => head with {Y = head.Y - 1},
            _ => head
        };
        if (Math.Abs(tail.X - head.X) > 1 || Math.Abs(tail.Y - head.Y) > 1)
        {
            tail = prev;
            if (!positions.Contains(tail))
                positions.Add(tail);
        }
    }
}

Console.WriteLine(positions.Count);


record Pos(int X, int Y);
record Move(Direction Direction, int Distance);
enum Direction {Left,Right,Up,Down};