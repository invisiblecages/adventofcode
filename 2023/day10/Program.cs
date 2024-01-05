using System.Diagnostics;

namespace Day10;

internal class Program
{
  public static class Directions
  {
    public const string North = "North";
    public const string South = "South";
    public const string East = "East";
    public const string West = "West";
  }

  /*
  | is a vertical pipe connecting north and south
  - is a horizontal pipe connecting east and west
  L is a 90 - degree bend connecting north and east
  J is a 90 - degree bend connecting north and west
  7 is a 90 - degree bend connecting south and west
  F is a 90 - degree bend connecting south and east
  . is ground; there is no pipe in this tile
  S is the starting position
  */

  public static Dictionary<char, Dictionary<string, List<char>>> pipeConnections = new()
  {
    { '|', new Dictionary<string, List<char>> {
      { Directions.North, [ '|', '7', 'F' ] },
      { Directions.South, [ '|', 'L', 'J' ] },
      { Directions.East,  [ ] },
      { Directions.West,  [ ] }
    }},
    { '-', new Dictionary<string, List<char>> {
      { Directions.North, [ ] },
      { Directions.South, [ ] },
      { Directions.East,  [ '-', 'J', '7' ] },
      { Directions.West,  [ '-', 'L', 'F' ] }
    }},
    { 'L', new Dictionary<string, List<char>> {
      { Directions.North, [ '|', '7', 'F' ] },
      { Directions.South, [ ] },
      { Directions.East,  [ '-', 'J', '7' ] },
      { Directions.West,  [ ] }
    }},
    { 'J', new Dictionary<string, List<char>> {
      { Directions.North, [ '|', '7', 'F' ] },
      { Directions.South, [ ] },
      { Directions.East,  [ ] },
      { Directions.West,  [ '-', 'L', 'F' ] }
    }},
    { '7', new Dictionary<string, List<char>> {
      { Directions.North, [ ] },
      { Directions.South, [  '|', 'L', 'J' ] },
      { Directions.East,  [ ] },
      { Directions.West,  [  '-', 'L', 'F' ] }
    }},
    { 'F', new Dictionary<string, List<char>> {
      { Directions.North, [ ] },
      { Directions.South, [ '|', 'L', 'J' ] },
      { Directions.East,  [ '-', 'J', '7' ] },
      { Directions.West,  [ ] }
    }},
    { 'S', new Dictionary<string, List<char>> {
      { Directions.North, [ '|', '7', 'F' ] },
      { Directions.South, [ '|', 'L', 'J' ] },
      { Directions.East,  [ '-', 'J', '7' ] },
      { Directions.West,  [ '-', 'L', 'F' ] }
    }}
  };

  public static (char[,] grid, (int, int) startPositon) ParseInput(string[] input)
  {
    int rows = input.Length;
    int cols = input[0].Length;
    char[,] grid = new char[rows, cols];
    (int x, int y) startPositon = (0, 0);

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < cols; j++)
      {
        char c = input[i][j];

        if (c == 'S')
        {
          startPositon = (i, j);
        }

        grid[i, j] = c;
      }
    }

    return (grid, startPositon);
  }

  private static void TraverseGrid(char[,] grid, (int x, int y) pos, Dictionary<(int x, int y), int> distances)
  {
    Queue<(int x, int y, int steps)> queue = [];
    queue.Enqueue((pos.x, pos.y, 0));

    while (queue.Count > 0)
    {
      var (x, y, steps) = queue.Dequeue();
      // Console.WriteLine($"({x}, {y} - {steps})");

      if (distances.ContainsKey((x, y)))
      {
        continue;
      }

      distances.TryAdd((x, y), steps);

      if (x - 1 >= 0 && IsValidMove(grid[x, y], grid[x - 1, y], Directions.North))
      {
        queue.Enqueue((x - 1, y, steps + 1));
      }

      if (x + 1 < grid.GetLength(0) && IsValidMove(grid[x, y], grid[x + 1, y], Directions.South))
      {
        queue.Enqueue((x + 1, y, steps + 1));
      }

      if (y + 1 < grid.GetLength(1) && IsValidMove(grid[x, y], grid[x, y + 1], Directions.East))
      {
        queue.Enqueue((x, y + 1, steps + 1));
      }

      if (y - 1 >= 0 && IsValidMove(grid[x, y], grid[x, y - 1], Directions.West))
      {
        queue.Enqueue((x, y - 1, steps + 1));
      }
    }
  }

  private static bool IsValidMove(char fromPipe, char toPipe, string direction)
  {
    bool isValid = pipeConnections[fromPipe][direction].Contains(toPipe);
    // Console.WriteLine($"{fromPipe} -> {toPipe} ({direction}): {isValid}");
    return isValid;
  }

  private static void PrintGrid(char[,] grid, Dictionary<(int x, int y), int>? distances = null)
  {
    Dictionary<char, char> unicodeBoxDrawing = new()
    {
      { '|', '\u2551' },
      { '-', '\u2550' },
      { 'J', '\u255D' },
      { 'L', '\u255A' },
      { '7', '\u2557' },
      { 'F', '\u2554' },
      { '.', '.' },
      { 'S', 'S' },
    };

    for (int i = 0; i < grid.GetLength(0); i++)
    {
      for (int j = 0; j < grid.GetLength(1); j++)
      {
        if (distances != null && distances.ContainsKey((i, j)))
        {
          Console.Write(distances[(i, j)]);
        }
        else
        {
          Console.Write(unicodeBoxDrawing[grid[i, j]]);
        }
      }

      Console.WriteLine();
    }
  }

  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    var (grid, startingPosition) = ParseInput(input);
    Dictionary<(int x, int y), int> distances = [];

    TraverseGrid(grid, startingPosition, distances);
    PrintGrid(grid);

    Console.WriteLine($"1: {distances.Values.Max()}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string[] examples =
    [
      "example1.txt", "example2.txt", "example3.txt", "example4.txt", "example5.txt"
    ];
    string input = "input.txt";

    Console.WriteLine("Day 10 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/10");
    Console.WriteLine("********* Example ********");

    foreach (string example in examples)
    {
      stopwatch.Restart();
      Solve(example);
      Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
    }

    Console.WriteLine("********* Puzzle *********");

    stopwatch.Restart();
    Solve(input);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
  }
}