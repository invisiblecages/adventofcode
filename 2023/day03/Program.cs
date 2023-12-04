using System.Diagnostics;

namespace Day03;

internal class Program
{
  private static string[,] BuildMatrix(string file)
  {
    string[] input = File.ReadAllLines(file);
    int rows = input.Length;
    int cols = input[0].Length;
    string[,] array = new string[rows + 1, cols + 1];

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < cols; j++)
      {
        array[i, j] = input[i][j].ToString();
      }
    }

    // Adding a row and a column to handle numbers at the edge of the matrix
    for (int j = 0; j < cols + 1; j++)
    {
      array[rows, j] = ".";
    }

    for (int i = 0; i < rows + 1; i++)
    {
      array[i, cols] = ".";
    }

    return array;
  }

  private static (bool, (int, int)) IsAdjacent(string[,] array, int row, int col, bool gearSearch = false)
  {
    int rows = array.GetLength(0);
    int cols = array.GetLength(1);

    // up, up-right, right, down-right, down, down-left, left, up-left
    int[] rowDirections = [-1, -1, 0, 1, 1, 1, 0, -1];
    int[] colDirections = [0, 1, 1, 1, 0, -1, -1, -1];

    for (int i = 0; i < rowDirections.Length; i++)
    {
      int searchRow = row + rowDirections[i];
      int searchCol = col + colDirections[i];

      if (searchRow >= 0 && searchRow < rows && searchCol >= 0 && searchCol < cols)
      {
        char adjacentSymbol = array[searchRow, searchCol][0];

        if (!gearSearch && !char.IsDigit(adjacentSymbol) && adjacentSymbol != '.')
        {
          return (true, (searchRow, searchCol));
        }

        if (gearSearch && adjacentSymbol == '*')
        {
          return (true, (searchRow, searchCol));
        }
      }
    }

    return (false, (-1, -1));
  }

  private static void AddGearAdjacency(Dictionary<(int, int), List<int>> gears, (int, int) gearPosition, int partNumber)
  {
    if (gears.ContainsKey(gearPosition))
    {
      gears[gearPosition].Add(partNumber);
    }
    else
    {
      gears.Add(gearPosition, [partNumber]);
    }
  }

  public static void Part1(string[,] schematic)
  {
    int sum = 0;

    for (int i = 0; i < schematic.GetLength(0); i++)
    {
      string partNumber = "";
      bool isAdjacent = false;

      for (int j = 0; j < schematic.GetLength(1); j++)
      {
        char part = schematic[i, j][0];

        if (char.IsDigit(part))
        {
          if (!isAdjacent)
          {
            isAdjacent = IsAdjacent(schematic, i, j).Item1;
          }

          partNumber += part.ToString();
          continue;
        }

        if (!string.IsNullOrEmpty(partNumber))
        {
          sum += isAdjacent ? int.Parse(partNumber) : 0;
        }

        partNumber = "";
        isAdjacent = false;
      }
    }

    Console.WriteLine($"1: {sum}");
  }

  public static void Part2(string[,] schematic)
  {
    Dictionary<(int, int), List<int>> gears = [];
    int sum = 0;

    for (int i = 0; i < schematic.GetLength(0); i++)
    {
      string partNumber = "";
      bool isAdjacent = false;
      (int, int) gearPosition = (-1, -1);

      for (int j = 0; j < schematic.GetLength(1); j++)
      {
        char part = schematic[i, j][0];

        if (char.IsDigit(part))
        {
          if (!isAdjacent)
          {
            (isAdjacent, gearPosition) = IsAdjacent(schematic, i, j, true);
          }

          partNumber += part.ToString();
          continue;
        }

        if (!string.IsNullOrEmpty(partNumber) && gearPosition != (-1, -1))
        {
          AddGearAdjacency(gears, gearPosition, int.Parse(partNumber));
        }

        partNumber = "";
        isAdjacent = false;
        gearPosition = (-1, -1);
      }
    }

    foreach (var gear in gears.Where(gear => gear.Value.Count > 1))
    {
      sum += gear.Value[0] * gear.Value[1];
    }

    Console.WriteLine($"2: {sum}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string[,] example = BuildMatrix("example.txt");
    string[,] input = BuildMatrix("input.txt");

    Console.WriteLine("Day 3 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/3");
    Console.WriteLine("********* Example ********");

    stopwatch.Restart();
    Part1(example);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");

    stopwatch.Restart();
    Part2(example);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");

    Console.WriteLine("********* Puzzle *********");

    stopwatch.Restart();
    Part1(input);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");

    stopwatch.Restart();
    Part2(input);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
  }
}
