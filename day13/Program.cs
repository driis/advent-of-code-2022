var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");

(ListOfValues,int) ReadList(string line)
{
    if (line[0] != '[')
    {
        throw new ArgumentException("Not start of list", nameof(line));
    }

    List<ListOfValues> inner = new();
    int i = 1;
    while(i < line.Length)
    {
        if (line[i] == '[') 
        {
            var (list, read) = ReadList(line[i..]);
            i += read;
            inner.Add(list);
        }
        else if (Char.IsNumber(line[i]))
        {
            var numeric = line[i..].TakeWhile(Char.IsNumber).ToArray();
            var n = Int32.Parse(numeric);
            inner.Add(new ListOfValues(null,n));
            i += numeric.Length;
        }
        else if (line[i] == ']')
        {
            i++;
            break;
        }
        else
        {
            i++;
        }
    }

    return (new ListOfValues(inner, null),i);
}
IEnumerable<(ListOfValues, ListOfValues)> PairFromInput(string[] data)
{
    for (int i = 0; i < data.Length; i+=3)
    {
        var (first, _) = ReadList(data[i]);
        var (second, _) = ReadList(data[i + 1]);
        yield return (first, second);
    }
}


var pairs = PairFromInput(input);
var orderedIndices = pairs.Select((p, i) => ListComparer.IsOrdered(p) ? i+1 : 0).ToArray();
Console.WriteLine(String.Join(", ",orderedIndices));
var sum = orderedIndices.Sum();
Console.WriteLine($"Sum of ordered indices: {sum}");

// Part 2
var all = input.Where(x => !String.IsNullOrWhiteSpace(x)).Select(x => ReadList(x).Item1).Concat(new []{ReadList("[[2]]").Item1, ReadList("[[6]]").Item1}).ToList();
all.Sort(new ListComparer());
var indices = all.Select((item, index) => (item.ToString(), index + 1)).Where(x => x.Item1 == "[[2]]" || x.Item1 == "[[6]]")
    .ToArray();
var sortedString = String.Join("\n", all);
Console.WriteLine(sortedString);
int decoder = indices[0].Item2 * indices[1].Item2;

Console.WriteLine($"Decoder key: {decoder}");

class ListComparer : IComparer<ListOfValues>
{
    public int Compare(ListOfValues? x, ListOfValues? y)
    {
        bool? ordered = IsOrderedInner((x!, y!));
        return ordered switch
        {
            true => -1,
            false => 1,
            _ => 0
        };
    }

    public static bool IsOrdered((ListOfValues, ListOfValues) pair)
    {
        return IsOrderedInner(pair) ?? true;
    }

    static bool? IsOrderedInner((ListOfValues, ListOfValues) pair)
    {
        if (pair.Item1.Value != null && pair.Item2.Value != null)
        {
            if (pair.Item1.Value == pair.Item2.Value) return null;
            return pair.Item1.Value < pair.Item2.Value;
        }

        if (pair.Item1.Value != null && pair.Item2.Inner != null)
        {
            return IsOrdered((new ListOfValues(new[] {pair.Item1}, null), pair.Item2));
        }

        if (pair.Item1.Inner != null && pair.Item2.Value != null)
        {
            return IsOrdered((pair.Item1, new ListOfValues(new[] {pair.Item2}, null)));
        }

        for (int i = 0; i < pair.Item1.Inner!.Count; i++)
        {
            if (i >= pair.Item2.Inner!.Count)
                return false;

            var ordered = IsOrderedInner((pair.Item1.Inner.ElementAt(i), pair.Item2.Inner.ElementAt(i)));
            if (ordered != null)
                return ordered;
        }
    
        // List exhaused and left was first - right order
        return pair.Item1.Inner.Count < pair.Item2.Inner!.Count ? true : null;
    }

    
}
record ListOfValues(IReadOnlyCollection<ListOfValues>? Inner, int? Value)
{
    public override string ToString()
    {
        if (Value != null)
        {
            return Value.Value.ToString();
        }

        return $"[{String.Join(", ", Inner!.Select(x => x.ToString()))}]";
    }
};