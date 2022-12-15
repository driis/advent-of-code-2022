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

var allLines = input.SelectMany(LineFromText).ToArray();
Console.WriteLine($"Parsed {allLines.Length} lines.");

record Coord(int X, int Y);
record Line(Coord Begin, Coord End);