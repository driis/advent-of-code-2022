var input = new []{
    new Monkey(0, new () {77, 69, 76, 77, 50, 58},          n => n * 11, n => n % 5 == 0,   1, 5),
    new Monkey(1, new () {75, 70, 82, 83, 96, 64, 62},      n => n + 08, n => n % 17 == 0,  5, 6),
    new Monkey(2, new () {53},                              n => n * 3,  n => n % 2 == 0,   0, 7),
    new Monkey(3, new () {85, 64, 93, 64, 99},              n => n + 4,  n => n % 7 == 0,   7, 2),
    new Monkey(4, new () {61, 92, 71},                      n => n * n,  n => n % 3 == 0,   2, 3),
    new Monkey(5, new () {79, 73, 50, 90},                  n => n + 2,  n => n % 11 == 0,  4, 6),
    new Monkey(6, new () {50, 89},                          n => n + 3,  n => n % 13 == 0,  4, 3),
    new Monkey(7, new () {83, 56, 64, 58, 93, 91, 56, 65},  n => n + 5,  n => n % 19 == 0,  1, 0)
};
var testInput = new []{
    new Monkey(0, new () {79, 98},                          n => n * 19, n => n % 23 == 0,  2, 3),
    new Monkey(1, new () {54, 65, 75, 74},                  n => n +  6, n => n % 19 == 0,  2, 0),
    new Monkey(2, new () {79, 60, 97},                      n => n *  n, n => n % 13 == 0,  1, 3),
    new Monkey(7, new () {74},                              n => n +  3, n => n % 17 == 0,  0, 1)
};

const long factor = 2 * 3 * 5 * 7* 11 * 13 * 17 * 19 * 23;
var monkeys = input;

long Solve(int divider, int rounds)
{
    for(int i = 0 ; i < rounds ; i++)
    {
        foreach(Monkey m in monkeys)
        {
            while(m.Items.Any())
            {
                var item = m.Items[0];
                m.Items.RemoveAt(0);
                m.Inspections++;
                item = (m.Operation(item)/divider) % factor;
                int throwTo = m.Test(item) ? m.ThrowIfTrue : m.ThrowIfFalse;
                monkeys[throwTo].Items.Add(item);
            }
        }
    }

    var activeMonkeys = monkeys.OrderByDescending(m => m.Inspections).Select(x => x.Inspections).Take(2).ToArray();
    var monkeyBusiness = activeMonkeys[0] * activeMonkeys[1];
    Console.WriteLine($"Monkey business for {rounds} with a divider of {divider} is: {monkeyBusiness}\n");
    return monkeyBusiness;
}

Console.WriteLine("Part 1:");
Solve(3,20);
Console.WriteLine("Part 2:");
Solve(1,10000);

record Monkey(int Index, List<long> Items, Func<long, long> Operation, Func<long, bool> Test, int ThrowIfTrue, int ThrowIfFalse)
{
    public long Inspections {get;set;} = 0;
};