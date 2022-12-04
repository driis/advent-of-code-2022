int Priority(char ch) => ch switch
{
    < 'a' => (ch - 'A') + 27 , 
    _ => ch - 'a' + 1
};

var lines = await File.ReadAllLinesAsync("input.txt");
var rucksacks = lines.Select(line =>
{
    var div = line.Length / 2;
    return (part1: line[..div], part2: line[div..]);
}).ToArray();

var bothCompartments = rucksacks.Select(r => r.part1.Intersect(r.part2));
var prioritySum = bothCompartments.SelectMany(x => x).Sum(Priority);
Console.WriteLine($"Sum of priorities: {prioritySum}");

// Part 2
var groups = lines.Select((line,idx) => new {line,g = idx /3 }).GroupBy(x => x.g).Select(g => g.ToArray());
var badges = groups.Select(group => group[0].line.Intersect(group[1].line).Intersect(group[2].line)).SelectMany(x => x);
var badgeSum = badges.Sum(Priority);
Console.WriteLine($"Sum of badges: {badgeSum}");

    