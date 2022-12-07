using System.ComponentModel;

IEnumerable<string> input = await File.ReadAllLinesAsync("input.txt");
Dictionary<string, Dir> AllDirs = new ();

Dir ParseInput(IEnumerator<string> lines, string path)
{
    string ls = lines.MoveNext() ? lines.Current : throw new ApplicationException("EOF");
    if (ls != "$ ls")
    {
        throw new ApplicationException("Expected ls as first command");
    }

    List<Dir> dirs = new List<Dir>();
    List<FileEntry> files = new List<FileEntry>();
    while(lines.MoveNext())
    {
        var data = lines.Current.Split(' ');
        if (data[0] == "dir")
            continue;
        if ((data[0], data[1]) == ("$", "cd"))
        {
            if (data[2] == "..")
            {
                break;
            }
            dirs.Add(ParseInput(lines, Path.Combine(path, data[2])));
        }

        if (int.TryParse(data[0], out int size))
        {
            files.Add(new FileEntry(data[1], size));
        }
    }
    var dir = new Dir(path, dirs, files);
    AllDirs.Add(path, dir);
    return dir;
}

var enumerator = input.GetEnumerator();
enumerator.MoveNext();
var root = ParseInput(enumerator ,"/");
var smallSizedDirs = AllDirs.Values.Where(x => x.Size < 100000).Sum(x => x.Size);
Console.WriteLine($"Size of small dirs: {smallSizedDirs} (Part 1)\n");
const int totalSize   = 70000000;
const int desiredSize = 30000000;
var freeSpace = totalSize - root.Size;
var neededSpace = desiredSize - freeSpace;
Console.WriteLine($"Current free space:\t{freeSpace,12}");
Console.WriteLine($"Space to be cleared:\t{neededSpace,12}");

var deletionCandidate = AllDirs.Values.OrderBy(x => x.Size).First(x => x.Size > neededSpace);
Console.WriteLine($"Deleting dir sized:\t{deletionCandidate.Size,12} ({deletionCandidate.Name})");

record FileEntry(string Name, int Size);

record Dir(string Name, List<Dir> Dirs, List<FileEntry> Files)
{
    public int Size => Dirs.Sum(d => d.Size) + Files.Sum(f => f.Size);
};