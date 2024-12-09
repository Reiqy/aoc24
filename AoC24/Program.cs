﻿namespace AoC24;

using System.Diagnostics;

internal class Program
{
    static void Main(string[] args)
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        var problem1 = new Problem1();
        Console.WriteLine($"Problem 1A: {problem1.SolveA()}");
        Console.WriteLine($"Problem 1B: {problem1.SolveB()}");

        var problem2 = new Problem2();
        Console.WriteLine($"Problem 2A: {problem2.SolveA()}");
        Console.WriteLine($"Problem 2B: {problem2.SolveB()}");

        var problem3 = new Problem3();
        Console.WriteLine($"Problem 3A: {problem3.SolveA()}");
        Console.WriteLine($"Problem 3B: {problem3.SolveB()}");

        var problem4 = new Problem4();
        Console.WriteLine($"Problem 4A: {problem4.SolveA()}");
        Console.WriteLine($"Problem 4B: {problem4.SolveB()}");

        var problem5 = new Problem5();
        Console.WriteLine($"Problem 5A: {problem5.SolveA()}");
        Console.WriteLine($"Problem 5B: {problem5.SolveB()}");

        var problem6 = new Problem6();
        Console.WriteLine($"Problem 6A: {problem6.SolveA()}");
        Console.WriteLine($"Problem 6B: {problem6.SolveB()}");

        var problem7 = new Problem7();
        Console.WriteLine($"Problem 7A: {problem7.SolveA()}");
        Console.WriteLine($"Problem 7B: {problem7.SolveB()}");

        var problem8 = new Problem8();
        Console.WriteLine($"Problem 8A: {problem8.SolveA()}");
        Console.WriteLine($"Problem 8B: {problem8.SolveB()}");

        var problem9 = new Problem9();
        Console.WriteLine($"Problem 9A: {problem9.SolveA()}");
        Console.WriteLine($"Problem 9B: {problem9.SolveB()}");

        var elapsed = Stopwatch.GetElapsedTime(startTimestamp);
        Console.WriteLine($"Total elapsed time: {elapsed}");
    }
}
