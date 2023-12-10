using System.Diagnostics;

namespace Day09;

internal class Program
{
  private static Dictionary<string, List<List<int>>> BuildSequence(string key, List<int> history, Dictionary<string, List<List<int>>> sequences)
  {
    if (!sequences.ContainsKey(key))
    {
      sequences[key] = [];
    }

    sequences[key].Add(history);
    List<int> temp = [];

    for (int i = 0; i < history.Count - 1; i++)
    {
      temp.Add(history[i + 1] - history[i]);
    }

    if (temp.Sum() == 0)
    {
      sequences[key].Add(temp);
      return sequences;
    }

    return BuildSequence(key, temp, sequences);
  }

  private static void ExtrapolatePart1(string key, Dictionary<string, List<List<int>>> sequences)
  {
    for (int i = sequences[key].Count - 1; i >= 1; i--)
    {
      if (i == sequences[key].Count - 1)
      {
        sequences[key][i].Add(sequences[key][i][^1]);
      }

      if (sequences[key][i - 1].Sum() == 0)
      {
        sequences[key][i - 1].Add(0);
      }
      else
      {
        int num = sequences[key][i][^1] + sequences[key][i - 1][^1];
        sequences[key][i - 1].Add(num);
      }
    }
  }

  private static void ExtrapolatePart2(string key, Dictionary<string, List<List<int>>> sequences)
  {
    for (int i = sequences[key].Count - 1; i >= 1; i--)
    {
      if (i == sequences[key].Count - 1)
      {
        sequences[key][i].Insert(0, sequences[key][i][0]);
      }

      if (sequences[key][i - 1].Sum() == 0)
      {
        sequences[key][i - 1].Add(0);
      }
      else
      {
        int num = sequences[key][i - 1][0] - sequences[key][i][0];
        sequences[key][i - 1].Insert(0, num);
      }
    }
  }

  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    Dictionary<string, List<List<int>>> sequences = [];
    int part1 = 0;
    int part2 = 0;

    foreach (string line in input)
    {
      int[] history = line.Split(" ").Select(int.Parse).ToArray();
      BuildSequence(line, [.. history], sequences);
    }

    foreach (var kvp in sequences)
    {
      ExtrapolatePart1(kvp.Key, sequences);
      ExtrapolatePart2(kvp.Key, sequences);

      part1 += kvp.Value[0][^1];
      part2 += kvp.Value[0][0];
    }

    Console.WriteLine($"1: {part1}");
    Console.WriteLine($"2: {part2}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string example = "example.txt";
    string input = "input.txt";

    Console.WriteLine("Day 9 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/9");
    Console.WriteLine("********* Example ********");

    stopwatch.Restart();
    Solve(example);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");

    Console.WriteLine("********* Puzzle *********");

    stopwatch.Restart();
    Solve(input);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
  }
}
