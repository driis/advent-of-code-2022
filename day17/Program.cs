string input = File.ReadAllText(args.FirstOrDefault() ?? "input.txt");
var movement = new Movement(input.Trim());

var world = new char[7,12000];
var shapes = new Shape[] {
    new (new []{"####"}),
    new (new []{".#.",
                "###" ,
                ".#."}),
    new (new []{"..#",
                "..#",
                "###"}),
    new (new []{"#",
                "#",
                "#",
                "#"}),
    new (new []{"##",
                "##"})
};

long count = Int64.TryParse(args.Skip(1).FirstOrDefault(), out long c) ? c : 2022;
long topIndex = 0;
for(long block = 0 ; block < count ; block++)
{
    var shape = shapes[block%5] with { Y = topIndex + 4 };
    while(shape.Fall(world))
    {
        var move = movement.NextMove();
        var _ = move switch {
            '<' => shape.Left(world),
            '>' => shape.Right(world),
            _ => throw new ApplicationException($"Don't know move '{move}'")
        };        
    }   
    var newTop = shape.Y + shape.Height;
    topIndex = newTop > topIndex ? newTop : topIndex;
}

Console.WriteLine(topIndex);
if (topIndex < 50)
{
    for(long y = topIndex ; y >= 0 ; y--)
    {
        Console.Write('|');
        for(int x = 0 ; x < 7 ; x++)
        {
            char ch = world[x,y];
            ch = ch==0 ? '.' : ch;
            Console.Write(ch);
        }
        Console.WriteLine('|');
    }
    Console.WriteLine("+-------+");
}

record Shape(string[] Lines)
{
    public long X { get;set;} = 2;
    public long Y { get;set;} = 0;

    public int Width => Lines[0].Length;
    public int Height => Lines.Length;

    public bool Left(char[,] w) => AttemptMove(w, -1, 0);
    public bool Right(char[,] w) => AttemptMove(w, 1, 0);
    
    public void DrawSolid(char[,] w)
    {
        long y = Y + Lines.Length - 1;
        foreach(var line in Lines)
        {
            long x = X;
            foreach(char c in line)
            {        
                if (c != '.')                                        
                    w[x, y] = c;
                x++;
            }
            y--;
        }        
    }

    public bool Fall(char[,] w) 
    {
        bool moved = AttemptMove(w, 0, -1);
        if (!moved)
            DrawSolid(w);
        return moved;
    } 

    public bool AttemptMove(char[,] w, int dx, int dy)
    {
        long newX = X + dx;
        long newY = Y + dy;
        bool didMove = !Collision(w, newX, newY);
    
        if (didMove)
        {
            X = newX;            
            Y = newY;
        }
        return didMove;
    }

    public bool Collision(char[,] w, long x, long y)
    {
        if (y < 0 || x < 0 || x + Width > 7) 
            return true;

        foreach(string line in Lines.Reverse())
        {
            for(int i = 0 ; i < line.Length ; i++)
            {
                if (line[i] == '.')
                    continue;
                
                char wouldOccupy = w[x + i,y];
                if (wouldOccupy == '#')
                    return true;
            }
            y++;
        }

        return false;
    }
};
record Movement(string Input)
{
    private int index = 0;

    public char NextMove() => Input[index++ % Input.Length];
}