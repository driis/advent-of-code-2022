IEnumerable<int> Elves(string[] lines)
{
    int sum = 0;
    foreach (string line in lines)
    {
        if (line == "")
        {
            yield return sum;
            sum = 0;
        }

        if (int.TryParse(line, out int x))
        {
            sum += x;
        }
    }
}

// Part 1
var lines = (await File.ReadAllLinesAsync("input.txt")).ToArray();
var elves = Elves(lines).ToArray();
int largest = elves.Max();
Console.WriteLine($"Part 1 - Elf with largest calorie count: {largest}");

// Part 2
var threeLargest = elves.OrderByDescending(x => x).Take(3).Sum();
Console.WriteLine($"Part 2 - Three elves carrying a calorie total of {threeLargest}");