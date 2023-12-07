using System.Diagnostics;

namespace Day06;

internal class Program
{
  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    List<int> times = [];
    List<int> distances = [];
    ulong time = 0;
    ulong distance = 0;
    int part1 = 1;
    int part2 = 0;

    foreach (string line in input)
    {
      if (line.Contains("Time:"))
      {
        times = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
        time = ulong.Parse(string.Join("", times));
      }
      else if (line.Contains("Distance:"))
      {
        distances = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
        distance = ulong.Parse(string.Join("", distances));
      }
    }

    // Part 1
    for (int i = 0; i < times.Count; i++)
    {
      int wins = 0;

      for (int hold = 0; hold < times[i]; hold++)
      {
        // time: 7 distance: 9 hold: 2
        // (7 - 2) * 2 = 10
        int traveled = (times[i] - hold) * hold;

        if (traveled > distances[i])
          wins++;
      }

      part1 *= wins;
    }

    // Part 2
    for (ulong hold = 0; hold < time; hold++)
    {
      ulong traveled = (time - hold) * hold;

      if (traveled > distance)
        part2++;
    }

    Console.WriteLine($"1: {part1}");
    Console.WriteLine($"2: {part2}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string example = "example.txt";
    string input = "input.txt";

    Console.WriteLine("Day 6 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/6");
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
