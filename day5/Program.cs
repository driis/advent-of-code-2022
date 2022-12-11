using static System.Console;

Stack<char>[] ParseStacks(string[] input)
{
    var indices = new[] {1, 5, 9, 13, 17, 21, 25, 29, 33};
    var stacks = indices.Select(i => new Stack<char>()).ToArray();
    foreach (string line in input.Reverse())
    {
        for (int i = 0; i < indices.Length; i++)
        {
            char x = line[indices[i]];
            if (x != ' ')
            {
                stacks[i].Push(x);
            }
        }
    }

    return stacks;
}

void MoveInBlocks<T>(int count, Stack<T> from, Stack<T> to)
{
    var temp = new T[count];
    for (int i = 0; i < count ; i++)
    {
        temp[i] = from.Pop();
    }

    foreach (var item in temp.Reverse())
    {
        to.Push(item);
    }
}

var lines = await File.ReadAllLinesAsync("input.txt");
var state = lines.TakeWhile(x => x.StartsWith("[")).ToArray();
var moves = lines.SkipWhile(x => x != "").Skip(1).ToArray();

var stacks = ParseStacks(state);
foreach (string move in moves)
{
    var parts = move.Split(' ');
    int amt = Int32.Parse(parts[1]);
    int from = Int32.Parse(parts[3]) - 1;
    int to = Int32.Parse(parts[5]) - 1;
    MoveInBlocks(amt, stacks[from], stacks[to]);
}
var msg = new string(stacks.Select(s => s.Pop()).ToArray());
WriteLine(msg);