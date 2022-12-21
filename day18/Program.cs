var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");
(int x, int y, int z) Parse(string line)
{
    var parts = line.Split(",");
    return (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
}

IEnumerable<(int x, int y, int z)> AdjacentSides(int x, int y, int z)
{
    yield return (x - 1, y, z);
    yield return (x + 1, y, z);
    yield return (x, y - 1, z);
    yield return (x, y + 1, z);
    yield return (x, y, z - 1);
    yield return (x, y, z + 1);
}

var positions = input.Select(Parse).ToArray();
var maxX = positions.Max(p => p.x);
var maxY = positions.Max(p => p.y);
var maxZ = positions.Max(p => p.z);

var grid = new bool[maxX + 2, maxY + 2, maxZ + 2];

const bool LAVA = true;
foreach(var pos in positions)
{
    grid[pos.x, pos.y, pos.z] = LAVA;
}

int uncovered = 0;
for(int x = 1 ; x <= maxX ; x++)
{
    for(int y = 1 ; y <= maxY ; y++)
    {
        for(int z = 1 ; z <= maxZ ; z++)
        {
            if (grid[x,y,z] == LAVA)
            {
                foreach(var side in AdjacentSides(x,y,z))
                {
                    if (grid[side.x,side.y, side.z] != LAVA)
                        uncovered++;
                }
            }
        }
    }
}

Console.WriteLine($"Calculated surface area {uncovered}");