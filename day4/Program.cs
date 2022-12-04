Range[] RangesFromLine(string line)
{
    var parts = line.Split(',');
    var ranges = parts.Select(p =>
    {
        var n = p.Split('-');
        var begin = int.Parse(n[0]);
        var end = int.Parse(n[1]);
        return begin..end;
    }).ToArray();
    return ranges;
}

bool IsContained(Range first, Range second)
{
    bool IsIn(Range f, Range s) => f.Start.Value >= s.Start.Value && f.End.Value <= s.End.Value;
    return IsIn(first, second) || IsIn(second, first);
}

bool IsPartiallyContained(Range first, Range second)
{
    bool IsIn(Range f, Range s) => (f.Start.Value >= s.Start.Value && f.Start.Value <= s.End.Value);
    return IsIn(first, second) || IsIn(second, first);
}
var lines = await File.ReadAllLinesAsync("input.txt");
var ranges = lines.Select(RangesFromLine).ToArray();
int countContained = ranges.Count(x=>IsContained(x[0], x[1]));
Console.WriteLine($"Fully contained ranges:\t\t{countContained}");

int countPartiallyContained = ranges.Count(x => IsPartiallyContained(x[0], x[1]));
Console.WriteLine($"Partially contained ranges:\t{countPartiallyContained}");

