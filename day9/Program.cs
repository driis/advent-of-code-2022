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

Pos DoMove(Direction dir, Pos pos) => dir switch
{
    Direction.Right => pos with {X = pos.X + 1},
    Direction.Left => pos with {X = pos.X - 1},
    Direction.Down => pos with {Y = pos.Y + 1},
    Direction.Up => pos with {Y = pos.Y - 1},
    _ => pos
};

const int SnakeLength = 10; // 2 for Part 1, 10 for part 2
var snake = Enumerable.Repeat(new Pos(0, 0), SnakeLength).ToArray();
var moves = input.Select(ParseMove);
HashSet<Pos> positions = new() {snake[SnakeLength - 1]};

foreach (Move m in moves)
{
    for (int i = 0; i < m.Distance; i++)
    {
        snake[0] = DoMove(m.Direction, snake[0]);
        for (int z = 1; z < snake.Length; z++)
        {            
            var head = snake[z-1];
            var tail = snake[z];
            var deltaX = head.X - tail.X;
            var deltaY = head.Y - tail.Y;
            if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
            {
                deltaX = deltaX < -1 ? -1 : deltaX > 1 ? 1 : deltaX;
                deltaY = deltaY < -1 ? -1 : deltaY > 1 ? 1 : deltaY;
                tail = tail with { X = tail.X + deltaX, Y = tail.Y +deltaY};
                snake[z] = tail;
            }                        
        }
        
        var last = snake[snake.Length-1];
        if (!positions.Contains(last))
            positions.Add(last);
    }
}

Console.WriteLine(positions.Count);


record Pos(int X, int Y);
record Move(Direction Direction, int Distance);
enum Direction {Left,Right,Up,Down};