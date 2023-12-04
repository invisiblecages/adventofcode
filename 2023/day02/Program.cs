namespace Day02;

internal class Program
{
  public static void Solve(string file)
  {
    string[] input = File.ReadAllLines(file);
    int sum = 0;
    int powerSum = 0;

    foreach (string line in input)
    {
      string[] game = line.Split(": ");
      int gameId = int.Parse(game[0].Split(" ")[1]);
      int maxRedSet = 0, maxGreenSet = 0, maxBlueSet = 0;
      bool isValid = true;

      foreach (string set in game[1].Split("; "))
      {
        string[] cubes = set.Split(new string[] { " ", ", " }, StringSplitOptions.RemoveEmptyEntries);
        int red = 0, green = 0, blue = 0;

        for (int i = 0; i < cubes.Length; i++)
        {
          if (cubes[i] == "red")
          {
            red += int.Parse(cubes[i - 1]);
          }
          else if (cubes[i] == "green")
          {
            green += int.Parse(cubes[i - 1]);
          }
          else if (cubes[i] == "blue")
          {
            blue += int.Parse(cubes[i - 1]);
          }
        }

        maxRedSet = Math.Max(maxRedSet, red);
        maxGreenSet = Math.Max(maxGreenSet, green);
        maxBlueSet = Math.Max(maxBlueSet, blue);

        if (red > 12 || green > 13 || blue > 14)
        {
          isValid = false;
        }
      }

      sum += isValid ? gameId : 0;
      powerSum += maxRedSet * maxGreenSet * maxBlueSet;
    }

    Console.WriteLine($"Part 1: {sum}");
    Console.WriteLine($"Part 2: {powerSum}");
  }

  public static void Main()
  {
    Solve("input.txt");
  }
}