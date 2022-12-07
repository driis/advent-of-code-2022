bool AllIsDifferent(char[] data)
{
    for (int i = 0; i < data.Length - 1; i++)
    {
        int offset = i + 1;
        int cur = data[i];
        if (data[offset..].Any(x => x == cur))
        {
            return false;
        }
    }
    return true;
}

const int msgSize = 14;
string data = File.ReadAllText("input.txt");
int i = 0;
var buf = data[..msgSize].ToArray();
for (i = msgSize - 1; i < data.Length; i++)
{
    buf[i % msgSize] = data[i];
    if (AllIsDifferent(buf))
        break;
}
Console.WriteLine(i+1);