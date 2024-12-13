namespace AoC24;

using MathNet.Numerics.LinearAlgebra;
using System.Text.RegularExpressions;

public class Problem13
{
    public ulong SolveA()
    {
        var input = File.ReadAllLines("input/aoc24_13.txt");

        var totalPrice = 0UL;
        for (int i = 0; i < input.Length; i += 4)
        {
            totalPrice += this.ComputePriceWithLimits(input[i + 2], input[i], input[i + 1], (100, 100));
        }

        return totalPrice;
    }

    public ulong SolveB()
    {
        var input = File.ReadAllLines("input/aoc24_13.txt");

        const ulong prizeIncrease = 10_000_000_000_000;

        var totalPrice = 0UL;
        for (int i = 0; i < input.Length; i += 4)
        {
            totalPrice += this.ComputePriceWithLimits(input[i + 2], input[i], input[i + 1], limits: null, prizeIncrease: prizeIncrease);
        }

        return totalPrice;
    }

    private ulong ComputePriceWithLimits(string prizeInput, string buttonAInput, string buttonBInput, (double, double)? limits, ulong prizeIncrease = 0)
    {
        const ulong buttonAPrice = 3;
        const ulong buttonBPrice = 1;

        var buttonRegex = new Regex(@"X\+(?<x>\d+),\s*Y\+(?<y>\d+)");

        var prizeRegex = new Regex(@"X=(?<x>\d+),\s*Y=(?<y>\d+)");
        var prizeMatch = prizeRegex.Match(prizeInput);
        var px = ulong.Parse(prizeMatch.Groups["x"].Value) + prizeIncrease;
        var py = ulong.Parse(prizeMatch.Groups["y"].Value) + prizeIncrease;

        var prizeVector = Vector<double>.Build.Dense(2);
        prizeVector[0] = px;
        prizeVector[1] = py;

        var buttonAMatch = buttonRegex.Match(buttonAInput);
        var bax = ulong.Parse(buttonAMatch.Groups["x"].Value);
        var bay = ulong.Parse(buttonAMatch.Groups["y"].Value);

        var buttonBMatch = buttonRegex.Match(buttonBInput);
        var bbx = ulong.Parse(buttonBMatch.Groups["x"].Value);
        var bby = ulong.Parse(buttonBMatch.Groups["y"].Value);

        var baseMatrix = Matrix<double>.Build.Dense(2, 2);
        baseMatrix[0, 0] = bax;
        baseMatrix[1, 0] = bay;
        baseMatrix[0, 1] = bbx;
        baseMatrix[1, 1] = bby;

        var determinant = baseMatrix.Determinant();
        if (determinant == 0)
        {
            // The solution for this case is actually not very difficult, but isn't needed for the given input.
            // Determinant is equal to zero, so the base vector do not form a base and are a linearly dependent.
            // First we need to determine if one of the vectors is a multiple of the prize vector (this also implies the other one is as well).
            // Then we need to determine the coefficients for both of the vectors.
            // We only take the whole number coefficients and we take into account the limits.
            // Then we compare the prices for the coefficients.
            throw new NotImplementedException();
        }

        var transitionMatrix = baseMatrix.Inverse();

        var baseVector = transitionMatrix * prizeVector;
        var bx = baseVector[0];
        var by = baseVector[1];

        if (limits.HasValue && (bx > limits.Value.Item1 || by > limits.Value.Item2))
        {
            // Found solution doesn't fall within the limits.
            return 0;
        }

        var bxULong = (ulong)Math.Round(bx, MidpointRounding.AwayFromZero);
        var byULong = (ulong)Math.Round(by, MidpointRounding.AwayFromZero);
        var recomputedPx = bxULong * bax + byULong * bbx;
        var recomputedPy = bxULong * bay + byULong * bby;

        if (recomputedPx != px || recomputedPy != py)
        {
            return 0;
        }

        return buttonAPrice * bxULong + buttonBPrice * byULong;
    }
}
