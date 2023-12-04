using System.Diagnostics;

namespace Day04;

internal class Program
{
  public static (int points, int wins) CalculatePointsAndWins(string[] winningNumbers, string[] myNumbers)
  {
    int points = 0;
    int wins = 0;

    for (int i = 0; i < myNumbers.Length; i++)
    {
      if (winningNumbers.Contains(myNumbers[i]))
      {
        if (points == 0)
          points += 1;
        else
          points *= 2;

        wins++;
      }
    }

    return (points, wins);
  }

  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    int[] copies = new int[input.Length];
    Array.Fill(copies, 1);
    int scoreCard = 0;
    int scratchCards = 0;

    for (int i = 0; i < input.Length; i++)
    {
      string[] cardAndNumbers = input[i].Split(": ");
      string[] numbers = cardAndNumbers[1].Split(" | ");
      string[] winningNumbers = numbers[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
      string[] myNumbers = numbers[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

      (int points, int wins) = CalculatePointsAndWins(winningNumbers, myNumbers);

      for (int card = i + 1; card < wins + i + 1; card++)
      {
        copies[card] += copies[i];
      }

      scoreCard += points;
      scratchCards += copies[i];
    }

    Console.WriteLine($"1: {scoreCard}");
    Console.WriteLine($"2: {scratchCards}");
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string example = "example.txt";
    string input = "input.txt";

    Console.WriteLine("Day 4 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/4");
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