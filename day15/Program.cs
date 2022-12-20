using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args.FirstOrDefault() ?? "input.txt");
Regex parseEx = new Regex("x=(?<x>-?\\d+), y=(?<y>-?\\d+)", RegexOptions.Compiled | RegexOptions.Multiline);

Pos PositionFromMatch(Match m)
{
    int x = int.TryParse(m.Groups["x"].Value, out x) ? x : throw new ApplicationException("Bad input");
    int y = int.TryParse(m.Groups["y"].Value, out y) ? y : throw new ApplicationException("Bad input");
    return new (x,y);
}

Sensor SensorFromLine(string line)
{
    var matches = parseEx.Matches(line);
    if(matches.Count != 2) throw new ApplicationException("Bad input");
    return new Sensor(PositionFromMatch(matches[0]), PositionFromMatch(matches[1]));
}

var sensors = input.Select(SensorFromLine).ToArray();
int lineOfInterest = args.Length > 0 ? 10 : 2000000;
var minX = sensors.Min(s => Math.Min(s.Position.X, s.Beacon.X));
var maxX = sensors.Max(s => Math.Max(s.Position.X, s.Beacon.X));
Console.WriteLine($"X delta {maxX - minX} from {minX} to {maxX}");
HashSet<int> occupiedInLine = new();
foreach(var sensor in sensors)
{
    var distanceToY = Math.Abs(sensor.Position.Y - lineOfInterest);
    if (distanceToY > sensor.DistanceToBeacon)
        continue;
    var offSet = sensor.DistanceToBeacon - distanceToY;
    var x = sensor.Position.X;
    var data = Enumerable.Range(x - offSet, offSet * 2 );
    foreach(int i in data) 
        occupiedInLine.Add(i);
}

Console.WriteLine($"Line with index {lineOfInterest} has {occupiedInLine.Count} occupied places");

// Part 2
var map = new bool[4000000,4000000];
foreach(var sensor in sensors)
{
    var distance = sensor.DistanceToBeacon;
    var yDelta = distance - 1;
    var yDeltas = Enumerable.Range(-yDelta, yDelta * 2);
    
    foreach(var yd in yDeltas)
    {
        var xDelta = distance - yd;
        var xs = Enumerable.Range(sensor.Position.X - xDelta, xDelta * 2);
        var y = sensor.Position.Y + yd;
        foreach(var x in xs)
        {
            map[x,y] = true;
        }
    }
}
var option = map.Cast<bool>().Select((m,i)=> (m,i)).First(x => !x.m).i;
Console.WriteLine(option);

record Sensor(Pos Position, Pos Beacon)
{
    public int DistanceToBeacon => Math.Abs(Position.X - Beacon.X) + Math.Abs(Position.Y - Beacon.Y);
};
record Pos(int X, int Y);