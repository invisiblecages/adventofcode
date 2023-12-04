namespace Day01;

internal class Program
{
  public static int CalibrationSum(string[] input)
  {
    int sum = 0;

    foreach (string line in input)
    {
      char firstValue = line.FirstOrDefault(char.IsDigit);
      char lastValue = line.LastOrDefault(char.IsDigit);

      sum += int.Parse($"{firstValue}{lastValue}");
    }

    return sum;
  }

  public static void Part1(string file)
  {
    string[] input = File.ReadAllLines(file);

    Console.WriteLine($"Part 1: {CalibrationSum(input)}");
  }

  public static void Part2(string file)
  {
    string[] input = File.ReadAllLines(file);

    // To handle cases like "eightwothree"
    // Correct: 8wo3 (8 - 3)
    // Incorrect: eigh23 (2 - 3)
    // Output: e8t2ot3e (8 - 3)
    Dictionary<string, string> digitDictionary = new()
    {
      { "one", "o1e" },
      { "two", "t2o" },
      { "three", "t3e" },
      { "four", "f4r" },
      { "five", "f5e" },
      { "six", "s6x" },
      { "seven", "s7n" },
      { "eight", "e8t" },
      { "nine", "n9e" }
    };

    for (int i = 0; i < input.Length; i++)
    {
      foreach (var kvp in digitDictionary)
      {
        input[i] = input[i].Replace(kvp.Key, kvp.Value);
      }
    }

    Console.WriteLine($"Part 2: {CalibrationSum(input)}");
  }

  public static void Main()
  {
    Part1("input.txt");
    Part2("input.txt");
  }
}