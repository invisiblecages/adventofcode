using System.Diagnostics;

namespace Day08;

internal class Program
{
  private static int Part1(string node, string end, char[] instructions, Dictionary<string, string[]> network)
  {
    if (!network.TryGetValue(node, out _))
    {
      return -1;
    }

    int steps = 0;

    while (!node.EndsWith(end))
    {
      node = instructions[steps % instructions.Length] == 'L' ? network[node][0] : network[node][1];
      steps++;
    }

    return steps;
  }

  /* 
  I could not solve part two by myself without brute force (trillions of operations), 
  but found out about the LCM approach on r/adventofcode
  */
  private static ulong Part2(string start, string end, char[] instructions, Dictionary<string, string[]> network)
  {
    List<string> startNodes = network.Keys.Where(node => node.EndsWith(start)).ToList();
    List<ulong> cycles = [];

    foreach (string startNode in startNodes)
    {
      string node = startNode;
      ulong steps = 0;

      while (!node.EndsWith(end))
      {
        node = instructions[steps % (ulong)instructions.Length] == 'L' ? network[node][0] : network[node][1];
        steps++;
      }

      cycles.Add(steps);
    }

    // Calculate the least common multiple (LCM) of all steps within the cycles
    return cycles.Aggregate((a, b) => CalculateGCDAndLCM(a, b).LCM);
  }

  private static (ulong GCD, ulong LCM) CalculateGCDAndLCM(ulong a, ulong b)
  {
    ulong num1 = a, num2 = b;

    while (num2 != 0)
    {
      ulong temp = num2;
      num2 = num1 % num2;
      num1 = temp;
    }

    ulong GCD = num1;
    ulong LCM = a * b / GCD;

    return (GCD, LCM);
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
    Console.WriteLine($"2: {Part2("A", "Z", instructions, network)}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string[] examples = ["example1.txt", "example2.txt", "example3.txt"];
    string input = "input.txt";

    Console.WriteLine("Day 8 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/8");
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
