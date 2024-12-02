namespace AoC24;

public class Problem1
{
    public (List<int> Left, List<int> Right) ReadNumbers()
    {
        var left = new List<int>();
        var right = new List<int>();

        var lines = File.ReadAllLines("input/aoc24_1.txt");
        foreach (var line in lines)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(parts[0]));
            right.Add(int.Parse(parts[1]));
        }

        return (left, right);
    }

    public int SolveA()
    {
        var (left, right) = this.ReadNumbers();

        var result = left.Order()
            .Zip(right.Order())
            .Sum(x => Math.Abs(x.First - x.Second));

        return result;
    }

    public int SolveB()
    {
        var (left, right) = this.ReadNumbers();

        var counts = new Dictionary<int, int>();

        foreach (var number in left)
        {
            counts.TryAdd(number, 0);
        }

        foreach (var number in right)
        {
            if (counts.ContainsKey(number))
            {
                counts[number]++;
            }
        }

        var result = counts.Sum(x => x.Key * x.Value);
        return result;
    }
}
