using System.Diagnostics;

namespace Day05;

internal class Program
{
  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);

    foreach (string line in input)
    {
      Console.WriteLine(line);
    }

    Console.WriteLine($"1: {1}");
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