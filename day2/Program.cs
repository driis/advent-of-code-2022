Draw DrawFromInput(string input) => input switch
{
    "A" => Draw.Rock,
    "B" => Draw.Paper,
    "C" => Draw.Scissors,
    "X" => Draw.Rock,
    "Y" => Draw.Paper,
    "Z" => Draw.Scissors,
    _ => throw new ApplicationException($"Invalid input '{input}")
};

Result Win(Draw opponent, Draw me) => (opponent, me) switch
{
    (Draw.Rock, Draw.Paper) => Result.Win,
    (Draw.Rock, Draw.Scissors) => Result.Lose,
    (Draw.Paper, Draw.Rock) => Result.Lose,
    (Draw.Paper, Draw.Scissors) => Result.Win,
    (Draw.Scissors, Draw.Paper) => Result.Lose,
    (Draw.Scissors, Draw.Rock) => Result.Win,
    _ => Result.Draw
};


Result Expectation(string input) => input switch
{
    "X" => Result.Lose,
    "Y" => Result.Draw,
    "Z" => Result.Win,
    _ => throw new ApplicationException($"Invalid input '{input}")
};

Draw DrawForResult(Draw opponent, Result expectation) => (expectation, opponent) switch
{
    (Result.Lose, Draw.Paper) => Draw.Rock,
    (Result.Lose, Draw.Rock) => Draw.Scissors,
    (Result.Lose, Draw.Scissors) => Draw.Paper,
    (Result.Win, Draw.Paper) => Draw.Scissors,
    (Result.Win, Draw.Rock) => Draw.Paper,
    (Result.Win, Draw.Scissors) => Draw.Rock,
    _ => opponent
};

// Part 1
var lines = await File.ReadAllLinesAsync("input.txt");
var rounds = lines.Select(l => l.Split(' ')).Select(x => (DrawFromInput(x[0]), DrawFromInput(x[1])));
var results = rounds.Select(round => new {Win = Win(round.Item1, round.Item2), Me = round.Item2});
var score = results.Sum(r => (int) r.Win + (int) r.Me);
Console.WriteLine($"Part 1 - My score: {score}");

// Part 2
var roundExpectations = lines.Select(l => l.Split(' ')).Select(x => (DrawFromInput(x[0]), Expectation(x[1])));
var draws = roundExpectations.Select(x => new {Expectation = x.Item2, Draw = DrawForResult(x.Item1, x.Item2)});
var scorePart2 = draws.Sum(d => (int) (d.Expectation + (int) d.Draw));
Console.WriteLine($"Part 2 - My score: {scorePart2}");

enum Draw
{
    Rock = 1,Paper = 2,Scissors = 3
}

enum Result
{
    Draw = 3, Win = 6, Lose = 0
}