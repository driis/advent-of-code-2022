int Mark(int[][] trees, bool[,] m, ref int cur, int x, int y)
{
    int v = trees[y][x];
    bool mark = v > cur;
    m[y, x] |= mark;
    return mark ? v : cur;
}

string [] lines = await File.ReadAllLinesAsync(args.Length > 0 ? "test.txt" : "input.txt");
var data = lines.Select(line => line.Select(ch => ch - '0').ToArray()).ToArray();
var markers = new bool[data.Length, data[0].Length];
for (int y = 0 ; y < data.Length ; y++)
{
    int z = -1;
    for (int x = 0; x < data[0].Length; x++)
    {
        z = Mark(data, markers, ref z, x, y);
    }

    z = -1;
    for (int x = data[0].Length - 1 ; x >=0; x--)
    {
        z = Mark(data, markers, ref z, x, y);
    }
}

for (int x = 0; x < data[0].Length; x++)
{
    int z = -1;
    for (int y = 0; y < data.Length; y++)
    {
        z = Mark(data, markers, ref z, x, y);
    }

    z = -1;
    for (int y = data.Length - 1 ; y >=0; y--)
    {
        z = Mark(data, markers, ref z, x, y);
    }
}

for (int y = 0; y < data.Length; y++)
{
    for (int x = 0; x < data[0].Length; x++)
    {
        Console.Write(markers[y,x] ? "X" : " ");
    }
    Console.WriteLine();
}

var count = markers.Cast<bool>().Count(_=>_);
Console.WriteLine($"\n{count} trees in matrix of {markers.GetLength(0)},{markers.GetLength(1)}.");

// Part 2 
int ScenicScore((int, int) tree)
{
    int Count((int,int) t, Func<(int,int), (int,int)> direction)
    {
        int z = data[t.Item1][t.Item2], c = 1;
        for (t = direction(t); t.Item1 > 0 && t.Item1 < data.Length - 1 && t.Item2 > 0 && t.Item2 < data[0].Length - 1 ; t = direction(t))
        {
            var v = data[t.Item1][t.Item2];
            if (v >= z)
                break;
            c++;

        }

        return c;
    }

    var down = Count(tree,  z => (z.Item1 + 1, z.Item2));
    var up = Count(tree, z => (z.Item1 - 1, z.Item2));
    var right = Count(tree, z => (z.Item1, z.Item2 + 1));
    var left = Count(tree, z => (z.Item1, z.Item2 - 1));
    return down * up * right * left;
}

int maxScenic = 0;
for (int y = 0; y < data.Length; y++)
{
    for (int x = 0; x < data[0].Length; x++)
    {
        int score = ScenicScore((y, x));
        if (score > maxScenic)
            maxScenic = score;
    }
}

Console.Write($"Maximum scenic score {maxScenic}");