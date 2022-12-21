string input = File.ReadAllText(args.FirstOrDefault() ?? "input.txt");
var movement = new Movement(input.Trim());
const int WorldWidth = 7;
const int WorldHeight = 1000;
var world = new char[WorldWidth,WorldHeight];
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
long topOffset = 0;
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
        if (movement.Index % movement.Input.Length == 0)
        {
            Console.WriteLine($"Exhausted input after {block} blocks - height is {topIndex + topOffset}");
        }
    }   
    var newTop = shape.Y + shape.Height;
    topIndex = newTop > topIndex ? newTop : topIndex;
    if (topIndex > WorldHeight - 20)
    {
        const int retainTop = 100;
        var newWorld = new char[WorldWidth, WorldHeight];
        var from = topIndex - retainTop;
        for(long y = 0 ; y < retainTop ; y++)
        {
            for(int x = 0 ; x < WorldWidth ; x++)
            {
                newWorld[x,y] = world[x, from + y];
            }
        }
        topOffset += topIndex - retainTop;
        topIndex = retainTop;
        world = newWorld;
    }
}

Console.WriteLine(topIndex + topOffset);
if (topIndex < 50)
{
    for(long y = topIndex ; y >= 0 ; y--)
    {
        Console.Write('|');
        for(int x = 0 ; x < WorldWidth ; x++)
        {
            char ch = world[x,y];
            ch = ch==0 ? '.' : ch;
            Console.Write(ch);
        }
        Console.WriteLine('|');
    }
    Console.WriteLine("+-------+");
}

// Part 2 - by observation - after exhausting the input after 1723 blocks,
// Then the same pattern recurs each 1725 blocks. THese 1725 blocks is 2709 pix high.
// Do some math 
const long firstIterationBlocks= 1723;
const long blocksToRecur = 1725;
const long heightPerRecur = 2709;

const long rocks = 1_000_000_000_000;
const long rocksAfterFirst = rocks - firstIterationBlocks;
const long iterations = rocksAfterFirst / blocksToRecur;
const long blocksRan = iterations * blocksToRecur + firstIterationBlocks;
const long blocksRemaining = rocks - blocksRan;
Console.WriteLine($"Part 2 blocks remaning {blocksRemaining} - run the program with input {blocksRemaining+ firstIterationBlocks}");
// 5247 is the output of the program run with blocksRemaining + firstIterationBlocks as input
const long remaining = 5247;
const long height = heightPerRecur * iterations + remaining;
Console.WriteLine($"Part 2 height: {height}");

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
    public int Index = 0;

    public char NextMove() => Input[Index++ % Input.Length];
}