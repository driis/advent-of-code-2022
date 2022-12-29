var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");
var numbers = input.Select(x => Int32.Parse(x)).ToList();
Console.WriteLine($"Got {numbers.Count} numbers in input");

var numbersAndIndexes = numbers.Select((n,i)=>new N(n,i)).ToList();
var workingSet = numbersAndIndexes.ToList();
int size = workingSet.Count;
void DebugSmall(List<N> state) => Console.WriteLine(String.Join(", ", state.Select(x => x.Number)));
void DebugLarge(List<N> state) {};
Action<List<N>> visualize = size < 20 ? DebugSmall : DebugLarge;
visualize(workingSet);
foreach(var number in numbersAndIndexes)
{
    var indexNow = workingSet.IndexOf(number);
    workingSet.Remove(number);

    int offset = number.Number < 0 ? -1 : 0;
    var newIndex = ((size*10) + indexNow + number.Number + offset) % size;
    if (newIndex < indexNow && offset == 0) 
        newIndex++;
    
    workingSet.Insert(newIndex, number);

    visualize(workingSet);
}

var zeroOffset = workingSet.TakeWhile(x => x.Number != 0).Count();
var numbersOfInterest = new[]{workingSet[(1000+zeroOffset)%size], workingSet[(2000+zeroOffset)%size], workingSet[(3000+zeroOffset)%size]};

var sum = numbersOfInterest.Sum(x => x.Number);
Console.WriteLine($"Sum {sum}");
public record N(int Number, int Index) 
{ 
    // public int Index {get; set; } = Index;
};