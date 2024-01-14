using System.Diagnostics;

namespace Day10;

internal class Program
{
  private static char[,] grid = new char[0, 0];
  private static Dictionary<(int x, int y), int> loop = [];
  private static (int x, int y) startingPosition = (0, 0);

  private static class Directions
  {
    public const string North = "North";
    public const string South = "South";
    public const string East = "East";
    public const string West = "West";

    public static readonly List<(int dx, int dy, string dir)> All =
    [
      (-1, 0, North),
      (1, 0, South),
      (0, 1, East),
      (0, -1, West)
    ];
  }

  private static readonly Dictionary<char, Dictionary<string, List<char>>> PipeConnections = new()
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

  private static void ParseInput(string[] input)
  {
    int rows = input.Length;
    int cols = input[0].Length;
    grid = new char[rows, cols];

    for (int x = 0; x < rows; x++)
    {
      for (int y = 0; y < cols; y++)
      {
        char c = input[x][y];

        if (c == 'S')
        {
          startingPosition = (x, y);
        }

        grid[x, y] = c;
      }
    }
  }

  private static void TraverseLoop()
  {
    Queue<(int x, int y, int steps)> queue = [];
    loop = [];

    queue.Enqueue((startingPosition.x, startingPosition.y, 0));

    while (queue.Count > 0)
    {
      var (x, y, steps) = queue.Dequeue();

      if (loop.ContainsKey((x, y)))
      {
        continue;
      }

      loop.TryAdd((x, y), steps);

      foreach (var (dx, dy, direction) in Directions.All)
      {
        int nx = x + dx;
        int ny = y + dy;

        if (nx >= 0 && nx < grid.GetLength(0) && ny >= 0 && ny < grid.GetLength(1))
        {
          if (PipeConnections[grid[x, y]][direction].Contains(grid[nx, ny]))
          {
            queue.Enqueue((nx, ny, steps + 1));
          }
        }
      }
    }
  }

  private static void RemoveUnconnectedPipes()
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (!loop.ContainsKey((x, y)))
        {
          grid[x, y] = '.';
        }
      }
    }
  }

  private static int GetEnclosedTiles()
  {
    int count = 0;

    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (grid[x, y] == '.')
        {
          if (IsEnclosed(x, y))
          {
            count++;
          }
        }
      }
    }

    return count;
  }

  /* 
  raycast from tile to the right. if it intersects with an odd number of pipes, it's enclosed
  https://en.wikipedia.org/wiki/Point_in_polygon#Ray_casting_algorithm
  ray cast up or down: ['-', 'L', 'F']
  ray cast left or right: ['|', 'L', 'J']
  */
  private static bool IsEnclosed(int x, int startY)
  {
    int intersections = 0;

    for (int y = startY; y < grid.GetLength(1); y++)
    {
      // unsure why this works but not: ['|', '7', 'F']
      if (grid[x, y] == '|' || grid[x, y] == 'L' || grid[x, y] == 'J')
      {
        intersections++;
      }
    }

    return intersections % 2 == 1;
  }

  private static void PrintGrid(bool showSteps = false)
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

    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (showSteps && loop.ContainsKey((x, y)))
        {
          Console.Write(loop[(x, y)]);
        }
        else
        {
          Console.Write(unicodeBoxDrawing[grid[x, y]]);
        }
      }

      Console.WriteLine();
    }
  }

  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);

    ParseInput(input);
    TraverseLoop();
    RemoveUnconnectedPipes();
    // PrintGrid();

    Console.WriteLine($"1: {loop.Count / 2}");
    Console.WriteLine($"2: {GetEnclosedTiles()}");
  }

  public static void Main()
  {
    Console.WriteLine("Day 10 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/10");
    Console.WriteLine("********* Example ********");

    Stopwatch stopwatch = Stopwatch.StartNew();
    string[] examples =
    [
      "example1.txt", "example2.txt", "example3.txt", "example4.txt", "example5.txt"
    ];

    foreach (string example in examples)
    {
      stopwatch.Restart();
      Solve(example);
      Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
    }

    Console.WriteLine("********* Puzzle *********");

    stopwatch.Restart();
    Solve("input.txt");
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
  }
}