using System.Diagnostics;

namespace Day05;

internal class Program
{
  private static Dictionary<ulong, List<ulong>> Part1(List<ulong> seeds, Dictionary<string, List<List<ulong>>> almanac)
  {
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

    return correspondingNumbers;
  }

  // Brute force
  private static ulong Part2(List<ulong> seeds, Dictionary<string, List<List<ulong>>> almanac)
  {
    var parallelOptions = new ParallelOptions
    {
      MaxDegreeOfParallelism = Environment.ProcessorCount
    };

    ulong result = ulong.MaxValue;

    Parallel.For(0, seeds.Count, parallelOptions, i =>
    {
      if (i % 2 == 0)
      {
        ulong start = seeds[i];
        ulong end = seeds[i + 1];

        for (ulong seed = start; seed < start + end; seed++)
        {
          ulong number = seed;

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
          }

          result = Math.Min(result, Math.Min(seed, number));
        }
      }
    });

    return result;
  }

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

    Dictionary<ulong, List<ulong>> part1 = Part1(seeds, almanac);

    Console.WriteLine($"1: {part1.Min(kvp => kvp.Value.Last())}");
    Console.WriteLine($"2: {Part2(seeds, almanac)}");
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
