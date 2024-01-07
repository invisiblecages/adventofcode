using System.Diagnostics;

Console.WriteLine("Day 11 - Advent of Code 2023");
Console.WriteLine("https://adventofcode.com/2023/day/11");

string[] example = await File.ReadAllLinesAsync("example.txt");
string[] input = await File.ReadAllLinesAsync("input.txt");

Console.WriteLine("********* Example ********");

Stopwatch stopwatch = Stopwatch.StartNew();
Universe universe = new(example);
// universe.PrintInfo();
// universe.Print();
universe.Expand(2);
Console.WriteLine($"1: {universe.SumShortestPaths()}");

universe = new(example);
universe.Expand(100);
Console.WriteLine($"2: {universe.SumShortestPaths()}");
Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");

Console.WriteLine("********* Puzzle *********");

stopwatch.Restart();
universe = new(input);
universe.Expand(2);
Console.WriteLine($"1: {universe.SumShortestPaths()}");

universe = new(input);
universe.Expand(1000000);
Console.WriteLine($"2: {universe.SumShortestPaths()}");
Console.WriteLine($"({stopwatch.ElapsedMilliseconds} ms)");
