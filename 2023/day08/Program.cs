using System.Diagnostics;

namespace Day08;

internal class Program
{
  private static int Part1(string start, string end, char[] instructions, Dictionary<string, string[]> network)
  {
    int index = 0;
    int steps = 0;

    while (start != end)
    {
      if (instructions[index] == 'L')
      {
        start = network[start][0];
      }
      else
      {
        start = network[start][1];
      }

      index = (index + 1) % instructions.Length;
      steps++;
    }

    return steps;
  }

  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    char[] instructions = input[0].ToCharArray();
    Dictionary<string, string[]> network = [];

    foreach (string line in input[2..])
    {
      string[] parts = line.Split(" = ");
      string[] nodes = parts[0].Split(" ");
      string[] nextNodes = parts[1].Trim('(', ')').Split(", ");

      if (!network.ContainsKey(nodes[0]))
      {
        network[nodes[0]] = [nextNodes[0], nextNodes[1]];
      }
    }

    Console.WriteLine($"1: {Part1("AAA", "ZZZ", instructions, network)}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string example1 = "example1.txt";
    string example2 = "example2.txt";
    string input = "input.txt";

    Console.WriteLine("Day 8 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/8");
    Console.WriteLine("********* Example ********");

    stopwatch.Restart();
    Solve(example1);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");

    stopwatch.Restart();
    Solve(example2);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");

    Console.WriteLine("********* Puzzle *********");

    stopwatch.Restart();
    Solve(input);
    Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
  }
}
