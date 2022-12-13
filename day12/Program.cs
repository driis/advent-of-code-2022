using Microsoft.VisualBasic;

var data =await File.ReadAllLinesAsync("input.txt");
var w = data[0].Length;
var h = data.Length;

var map = new Edge[w, h];
Edge? begin = null, end = begin;

for (int x = 0; x < w; x++)
{
    for (int y = 0; y < h; y++)
    {
        var height = data[y][x];
        if (height == 'S' )
        {
            height = 'a';
            begin = map[x, y] = new Edge(new (x, y, height));
        } else if (height == 'E')
        {
            height = 'z';
            end = map[x,y] = new Edge(new (x, y, height));
        }
        else
        {
            map[x, y] = new Edge(new(x, y, height));
        }
    }
}

IEnumerable<Edge> ReachableEdges(Edge where)
{
    var p = where.Position;

    int yMin = Math.Max(p.Y - 1, 0);
    int yMax = Math.Min(p.Y + 2, h);
    int xMin = Math.Max(p.X - 1, 0);
    int xMax = Math.Min(p.X + 2, w);
    
    for (int y = yMin; y < yMax; y++)
    {
        for (int x = xMin; x < xMax; x++)
        {
            var edge = map[x, y];
            if (!edge.Visited && edge.Position.Height - where.Position.Height < 2)
                yield return edge;
        }
    }
}

if (begin == null || end == null)
    throw new ApplicationException("Begin or end not defined in input");

var cur = map[begin.Position.X, begin.Position.Y];
cur.Visited = true;
Queue<Edge> queue = new();
queue.Enqueue(cur);
while (queue.Count > 0)
{
    cur = queue.Dequeue();
    if (cur.Position == end.Position)
        break;
    foreach (var edge in ReachableEdges(cur))
    {
        edge.Visited = true;
        edge.Parent = cur;
        queue.Enqueue(edge);
    }
}

Console.WriteLine($"Found path to {cur.Position}");
int n = 0;
while (cur.Parent != null)
{
    cur = cur.Parent;
    n++;
}
Console.Write($"Path is {n} steps.");

public class Edge
{
    public Edge(Pos position)
    {
        Position = position;
    }
    public Pos Position { get; }
    public bool Visited { get; set; } = false;
    public Edge? Parent { get; set; } 
}

public record Pos(int X, int Y, char Height)
{
}