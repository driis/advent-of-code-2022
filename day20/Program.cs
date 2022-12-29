var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");
var numbers = input.Select(x => Int32.Parse(x)).ToArray();
Console.WriteLine($"Got {numbers.Length} numbers in input");