Instruction ParseInstruction(string line)
{
    var parts = line.Split(' ');
    return parts[0] switch
    {
        "noop" => new Instruction((_,__) => _, 0, 1),
        "addx" => new Instruction((x,arg) => x + arg, Int32.TryParse(parts[1], out int a) ? a : 0, 2),
        _ => throw new ApplicationException($"Unexpected input {line}")
    };
}

var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");
List<int> state = new();
int x = 1;
var instructions = input.Select(ParseInstruction);

foreach (Instruction what in instructions)
{
    for (int i = 0; i < what.Cycles; i++)
    {
        state.Add(x);
    }

    x = what.Operation(x, what.Arg);
}
state.Add(x);

var cycles = new[] {20, 60, 100, 140, 180, 220};
var strengths = cycles.Select(c => c * state[c-1]);
var sum = strengths.Sum();
Console.WriteLine($"Sum of signal strenghts: {sum}\n\n");

for (int i = 0; i < state.Count; i++)
{
    var spritePos = state[i];
    var rowPos = i % 40;
    if (rowPos == 0)
        Console.WriteLine();
    bool lit = rowPos >= spritePos - 1 && rowPos <= spritePos + 1;
    Console.Write(lit ? "#" : ".");
}
Console.WriteLine();

record Instruction(Func<int,int,int> Operation, int Arg, int Cycles);