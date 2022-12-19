var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");

IEnumerable<Line> LineFromText(string line)
{
    var parts = line.Split(" -> ");
    var coords = parts.Select(p =>
    {
        int[] data = p.Split(",").Select(z => Int32.Parse(z)).ToArray();
        return new Coord(data[0], data[1]);
    }).ToArray();

    for (int i = 0; i < coords.Length - 1; i++)
    {
        yield return new Line(coords[i], coords[i + 1]);
    }
}

void DrawLine(BlockType[,] w, Line line)
{
    // Skips bounds checking, assume input is good.
    bool horizontal = line.Begin.X != line.End.X;
    if (horizontal)
    {
        int xMax = Math.Max(line.Begin.X, line.End.X);
        int xMin = Math.Min(line.Begin.X, line.End.X);
        int y = line.Begin.Y;
        for (int x = xMin; x <= xMax; x++)
            w[x, y] = BlockType.Stone;
    }
    else
    {
        int yMax = Math.Max(line.Begin.Y, line.End.Y);
        int yMin = Math.Min(line.Begin.Y, line.End.Y);
        int x = line.Begin.X;
        for (int y = yMin; y <= yMax; y++)
            w[x, y] = BlockType.Stone;
    }
}

// Parse input
var allLines = input.SelectMany(LineFromText).ToArray();
var yMax = allLines.Max(x => x.YMax);
yMax += 2;
Console.WriteLine($"Parsed {allLines.Length} lines.");
Console.WriteLine($"Max Y is {yMax}");

// Draw lines on world
var world = new BlockType[1000, yMax+1];
foreach(var line in allLines) 
    DrawLine(world, line);
DrawLine(world, new Line(new(0, yMax), new(999, yMax)));

// Begin sim
var sand = new Coord(500, 0);
int sandCount = 0;
while (world[500,0] == BlockType.Void)
{
    int newX = sand.X;
    int newY = sand.Y + 1;
    // Fall down
    if (world[newX, newY] == BlockType.Void)
    {
        sand = sand with {Y = newY};
    }
    else if (world[newX-1, newY] == BlockType.Void)
    {
        sand = sand with {Y = newY, X = newX - 1};
    }
    else if (world[newX + 1, newY] == BlockType.Void)
    {
        sand = sand with {Y = newY, X = newX + 1};
    }
    else
    {
        // Blocked, rest here
        world[sand.X, sand.Y] = BlockType.Sand;
        sand = new Coord(500, 0);
        sandCount++;
    }
}

Console.WriteLine($"{sandCount} units rested");

enum BlockType {Void, Stone, Sand }
record Coord(int X, int Y);

record Line(Coord Begin, Coord End)
{
    public int YMax => Math.Max(Begin.Y, End.Y);
};