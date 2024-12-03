namespace AoC24;

using System.Text.RegularExpressions;

public class Problem3
{
    public int SolveA()
    {
        var input = File.ReadAllText("input/aoc24_3.txt");
        var regex = new Regex(@"mul\((?<left>\d{1,3}),(?<right>\d{1,3})\)");
        var matches = regex.Matches(input);

        var sum = matches.OfType<Match>()
            .Select(x => (Left: int.Parse(x.Groups["left"].Value), Right: int.Parse(x.Groups["right"].Value)))
            .Sum(x => x.Left * x.Right);
        return sum;
    }

    public int SolveB()
    {
        var input = File.ReadAllText("input/aoc24_3.txt");
        var regex = new Regex(@"(?<mul>mul\((?<left>\d{1,3}),(?<right>\d{1,3})\))|(?<do>do\(\))|(?<dont>don't\(\))");
        var matches = regex.Matches(input);

        var sum = matches.OfType<Match>()
            .Aggregate((State: AdderState.Enabled, Sum: 0), (accumulator, instruction) =>
            {
                if (instruction.Groups["do"].Success)
                {
                    return (AdderState.Enabled, accumulator.Sum);
                }

                if (instruction.Groups["dont"].Success)
                {
                    return (AdderState.Disabled, accumulator.Sum);
                }

                var newSum = accumulator.Sum;
                if (accumulator.State == AdderState.Enabled)
                {
                    var left = int.Parse(instruction.Groups["left"].Value);
                    var right = int.Parse(instruction.Groups["right"].Value);
                    newSum += left * right;
                }

                return (accumulator.State, newSum);
            }).Sum;
        return sum;
    }

    private enum AdderState
    {
        Enabled,
        Disabled,
    }
}
