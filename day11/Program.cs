var monkeys = new Monkey[]{
    new Monkey(0, new () {77, 69, 76, 77, 50, 58}, n => n * 11, n => n % 5 == 0, 1, 5),
    new Monkey(1, new () {75, 70, 82, 83, 96, 64, 62}, n => n + 11, n => n % 17 == 0, 5, 6),
    new Monkey(2, new () {53}, n => n * 3, n => n % 2 == 0, 0, 7),
    new Monkey(3, new () {85, 64, 93, 64, 99}, n => n + 4, n => n % 7 == 0, 7, 2),
    new Monkey(4, new () {61, 92, 71}, n => n * n, n => n % 3 == 0, 2, 3),
    new Monkey(5, new () {79, 73, 50, 90}, n => n + 2, n => n % 11 == 0, 4, 6),
    new Monkey(6, new () {50, 89}, n => n + 3, n => n % 13 == 0, 4, 3),
    new Monkey(7, new () {83, 56, 64, 58, 93, 91, 56, 65}, n => n + 5, n => n % 19 == 0, 1, 0)
};
record Monkey(int Index, Stack<int> Items, Func<int, int> Operation, Func<int, bool> Test, int ThrowIfTrue, int ThrowIfFalse);