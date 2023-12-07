using System.Diagnostics;

namespace Day05;

internal class Program
{
  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    List<ulong> seeds = input[0].Split(": ")[1].Split(" ").Select(ulong.Parse).ToList();
    Dictionary<string, List<List<ulong>>> almanac = [];
    string source = "";

    foreach (string line in input[2..])
    {
      if (line == "")
      {
        continue;
      }

      if (line.Contains(':'))
      {
        source = line.Replace(" map:", "");
        almanac[source] = [];
      }
      else
      {
        almanac[source].Add(line.Split(" ").Select(ulong.Parse).ToList());
      }
    }

    Dictionary<ulong, List<ulong>> correspondingNumbers = [];

    foreach (ulong seed in seeds)
    {
      ulong number = seed;
      correspondingNumbers[seed] = [];

      foreach (var kvp in almanac)
      {
        foreach (List<ulong> numbers in kvp.Value)
        {
          ulong destRangeStart = numbers[0];
          ulong srcRangeStart = numbers[1];
          ulong rangeLength = numbers[2];

          if (srcRangeStart <= number && number < srcRangeStart + rangeLength)
          {
            number = destRangeStart + number - srcRangeStart;
            break;
          }
        }

        correspondingNumbers[seed].Add(number);
      }
    }

    // TODO: Part 2

    Console.WriteLine($"1: {correspondingNumbers.Min(kvp => kvp.Value.Last())}");
    Console.WriteLine($"2: {2}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string example = "example.txt";
    string input = "input.txt";

    Console.WriteLine("Day 5 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/5");
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
