using System.Diagnostics;

namespace Day07;

public enum HandType
{
  HighCard = 1,
  OnePair,
  TwoPair,
  ThreeOfAKind,
  FullHouse,
  FourOfAKind,
  FiveOfAKind
}

// TODO: Refactor this mess
internal class Program
{
  private static void Solve(string file)
  {
    var hands = ParseInput(file);

    SetHandType(hands);
    SortHands(hands, "23456789TJQKA");
    Console.WriteLine($"1: {GetWinnings(hands)}");

    SetHandType(hands, true);
    SortHands(hands, "J23456789TQKA");
    Console.WriteLine($"2: {GetWinnings(hands)}");
  }

  private static List<(int handType, string hand, int bid)> ParseInput(string file)
  {
    string[] input = File.ReadAllLines(file);
    List<(int handType, string hand, int bid)> hands = [];

    foreach (string line in input)
    {
      string[] parts = line.Split(" ");
      hands.Add((0, parts[0], int.Parse(parts[1])));
    }

    return hands;
  }

  private static void SetHandType(List<(int handType, string hand, int bid)> hands, bool joker = false)
  {
    for (int i = 0; i < hands.Count; i++)
    {
      Dictionary<char, int> cardCount = GetCardCount(hands[i].hand);
      HandType handType = GetHandType(cardCount, joker);

      hands[i] = ((int)handType, hands[i].hand, hands[i].bid);
    }
  }

  private static Dictionary<char, int> GetCardCount(string hand)
  {
    Dictionary<char, int> cardCount = [];

    foreach (char c in hand)
    {
      cardCount[c] = cardCount.TryGetValue(c, out int count) ? count + 1 : 1;
    }

    return cardCount;
  }

  private static HandType GetHandType(Dictionary<char, int> cardCount, bool joker)
  {
    int jokers = 0;

    if (joker)
    {
      jokers = cardCount.TryGetValue('J', out int count) ? count : 0;
      cardCount.Remove('J');
    }

    int max = cardCount.Count > 0 ? cardCount.Values.Max() : 0;
    int countOfTwos = cardCount.Values.Count(x => x == 2);
    int countOfThrees = cardCount.Values.Count(x => x == 3);

    if (max + jokers == 5)
    {
      return HandType.FiveOfAKind;
    }

    if (max + jokers == 4)
    {
      return HandType.FourOfAKind;
    }

    if (countOfThrees == 1 && countOfTwos == 1 ||
        countOfTwos == 2 && jokers == 1 ||
        countOfTwos == 1 && jokers == 2)
    {
      return HandType.FullHouse;
    }

    if (max + jokers == 3)
    {
      return HandType.ThreeOfAKind;
    }

    if (countOfTwos + jokers == 2)
    {
      return HandType.TwoPair;
    }

    if (max + jokers == 2)
    {
      return HandType.OnePair;
    }

    return HandType.HighCard;
  }

  private static void SortHands(List<(int handType, string hand, int bid)> hands, string cardOrder)
  {
    hands.Sort(new HandComparer(cardOrder));
  }

  private class HandComparer : IComparer<(int handType, string hand, int bid)>
  {
    private readonly Dictionary<char, int> cardValues = [];

    public HandComparer(string cardOrder)
    {
      for (int i = 0; i < cardOrder.Length; i++)
      {
        cardValues[cardOrder[i]] = i;
      }
    }

    public int Compare((int handType, string hand, int bid) a, (int handType, string hand, int bid) b)
    {
      int typeComparison = a.handType.CompareTo(b.handType);

      if (typeComparison != 0)
      {
        return typeComparison;
      }

      for (int i = 0; i < a.hand.Length; i++)
      {
        int cardComparison = cardValues[a.hand[i]].CompareTo(cardValues[b.hand[i]]);

        if (cardComparison != 0)
        {
          return cardComparison;
        }
      }

      return 0;
    }
  }

  private static int GetWinnings(List<(int handType, string hand, int bid)> hands)
  {
    return hands.Select((hand, i) => hand.bid * (i + 1)).Sum();
  }

  public static void Main()
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    string example = "example.txt";
    string input = "input.txt";

    Console.WriteLine("Day 7 - Advent of Code 2023");
    Console.WriteLine("https://adventofcode.com/2023/day/7");
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