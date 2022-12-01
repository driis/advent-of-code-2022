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
var lines = await File.ReadAllLinesAsync("input.txt");
int largest = Elves(lines).Max();
Console.WriteLine(largest);