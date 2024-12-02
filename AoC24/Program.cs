namespace AoC24;

internal class Program
{
    static void Main(string[] args)
    {
        var problem1 = new Problem1();
        Console.WriteLine($"Problem 1A: {problem1.SolveA()}");
        Console.WriteLine($"Problem 1B: {problem1.SolveB()}");

        var problem2 = new Problem2();
        Console.WriteLine($"Problem 2A: {problem2.SolveA()}");
        Console.WriteLine($"Problem 2B: {problem2.SolveB()}");
    }
}
