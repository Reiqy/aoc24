namespace AoC24;

public class Problem11
{
    private readonly Dictionary<(ulong, int), ulong> cache = new();

    public ulong SolveA()
    {
        var input = this.ParseInput();
        var computer = new CountComputer();
        return this.GetCountAfterBlinks(computer, input, 25);
    }

    public ulong SolveB()
    {
        var input = this.ParseInput();
        var computer = new CountComputer();
        return this.GetCountAfterBlinks(computer, input, 75);
    }

    private ulong[] ParseInput()
    {
        var input = File.ReadAllText("input/aoc24_11.txt")
            .Split(' ')
            .Select(ulong.Parse)
            .ToArray();
        return input;
    }

    private ulong GetCountAfterBlinks(CountComputer computer, ulong[] input, int blinkCount)
    {
        var perStoneResults = input.Select(x => computer.GetCount(x, blinkCount)).ToArray();

        var totalCount = 0UL;
        foreach (var result in perStoneResults)
        {
            totalCount += result;
        }

        return totalCount;
    }

    private class CountComputer
    {
        private readonly Dictionary<CacheKey, ulong> cache = new();

        public ulong GetCount(ulong input, int blinkCount)
        {
            var key = new CacheKey(input, blinkCount);
            if (!this.cache.TryGetValue(key, out var value))
            {
                value = this.ComputeCount(input, blinkCount);
                this.cache.Add(key, value);
            }

            return value;
        }

        private ulong ComputeCount(ulong input, int blinkCount)
        {
            if (blinkCount == 0)
            {
                return 1;
            }

            if (input == 0)
            {
                return this.GetCount(1, blinkCount - 1);
            }

            if (this.HasEvenNumbersOfDigits(input))
            {
                var split = this.SplitInHalf(input);
                return this.GetCount(split.Item1, blinkCount - 1) + this.GetCount(split.Item2, blinkCount - 1);
            }

            return this.GetCount(input * 2024UL, blinkCount - 1);
        }

        private bool HasEvenNumbersOfDigits(ulong number)
        {
            var digits = this.GetNumberOfDigits(number);
            return digits % 2 == 0;
        }

        private int GetNumberOfDigits(ulong number)
        {
            if (number == 0)
            {
                return 1;
            }

            var digits = 0;
            while (number > 0)
            {
                number /= 10;
                digits++;
            }

            return digits;
        }

        private (ulong, ulong) SplitInHalf(ulong number)
        {
            var digits = this.GetNumberOfDigits(number);
            var divisor = (ulong)Math.Pow(10, digits / 2);
            var first = number / divisor;
            var second = number % divisor;
            return (first, second);
        }

        private readonly record struct CacheKey(ulong Input, int BlinkCount);
    }
}
