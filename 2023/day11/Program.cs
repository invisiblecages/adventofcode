using System.Diagnostics;
using System.Text;

namespace Day11;

internal class Program
{
  private static (List<(int x, int y)> galaxies, List<int> emptyRows, List<int> emptyCols) ParseInput(string[] input)
  {
    List<(int x, int y)> galaxies = [];
    List<int> emptyRows = [];
    List<int> emptyCols = [];
    int rows = input.Length;
    int cols = input[0].Length;

    for (int i = 0; i < rows; i++)
    {
      bool hasGalaxy = false;

      for (int j = 0; j < cols; j++)
      {
        if (input[i][j] == '#')
        {
          galaxies.Add((i, j));
          hasGalaxy = true;
        }
      }

      if (!hasGalaxy)
      {
        emptyRows.Add(i);
      }
    }

    for (int i = 0; i < cols; i++)
    {
      bool hasGalaxy = false;

      for (int j = 0; j < rows; j++)
      {
        if (input[j][i] != '.')
        {
          hasGalaxy = true;
          break;
        }
      }

      if (!hasGalaxy)
      {
        emptyCols.Add(i);
      }
    }

    return (galaxies, emptyRows, emptyCols);
  }

  private static List<(int x, int y)> ExpandUniverse(List<(int x, int y)> galaxies, List<int> emptyRows, List<int> emptyCols, int cosmicExpansion)
  {
    List<(int x, int y)> newGalaxies = galaxies.Select(g => g).ToList();
    cosmicExpansion--;

    for (int i = 0; i < galaxies.Count; i++)
    {
      foreach (int row in emptyRows)
      {
        if (galaxies[i].x >= row)
        {
          newGalaxies[i] = (newGalaxies[i].x + cosmicExpansion, newGalaxies[i].y);
        }
      }

      foreach (int col in emptyCols)
      {
        if (galaxies[i].y >= col)
        {
          newGalaxies[i] = (newGalaxies[i].x, newGalaxies[i].y + cosmicExpansion);
        }
      }
    }

    return newGalaxies;
  }

  private static ulong SumShortestPath(List<(int x, int y)> galaxies)
  {
    ulong sum = 0;

    for (int i = 0; i < galaxies.Count; i++)
    {
      for (int j = i + 1; j < galaxies.Count; j++)
      {
        var (x1, y1) = galaxies[i];
        var (x2, y2) = galaxies[j];

        sum += (ulong)(Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
      }
    }

    return sum;
  }

  public static void PrintInfo(List<(int x, int y)> galaxies, List<int> emptyCols, List<int> emptyRows)
  {
    Console.WriteLine($"Galaxies: ({galaxies.Count}): {string.Join(", ", galaxies)}");
    Console.WriteLine($"Empty rows: {string.Join(", ", emptyRows)}");
    Console.WriteLine($"Empty cols: {string.Join(", ", emptyCols)}");
  }

  private static void PrintUniverse(List<(int x, int y)> galaxies, bool showNumbers = false)
  {
    int rows = galaxies.Select(g => g.x).Max() + 1;
    int cols = galaxies.Select(g => g.y).Max() + 1;

    Console.WriteLine($"Grid: {rows}x{cols}");

    for (int i = 0; i < rows; i++)
    {
      StringBuilder sb = new();

      for (int j = 0; j < cols; j++)
      {
        sb.Append('.');

        for (int k = 0; k < galaxies.Count; k++)
        {
          if (galaxies[k].x == i && galaxies[k].y == j)
          {
            sb.Remove(sb.Length - 1, 1);

            if (!showNumbers)
              sb.Append('#');
            else
              sb.Append(k + 1);
          }
        }
      }

      Console.WriteLine(sb.ToString());
    }
  }

  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    var (galaxies, emptyRows, emptyCols) = ParseInput(input);

    PrintInfo(galaxies, emptyCols, emptyRows);    
    PrintUniverse(galaxies);
    var part1 = ExpandUniverse(galaxies, emptyRows, emptyCols, 2);
    var part2 = ExpandUniverse(galaxies, emptyRows, emptyCols, 1000000);

    Console.WriteLine($"1: {SumShortestPath(part1)}");
    Console.WriteLine($"2: {SumShortestPath(part2)}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    Console.WriteLine("Day 11 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/11");
    Console.WriteLine("********* Example ********");

    stopwatch.Restart();
    Solve("example.txt");
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
    
    Console.WriteLine("********* Puzzle *********");

    stopwatch.Restart();
    Solve("input.txt");
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
  }
}