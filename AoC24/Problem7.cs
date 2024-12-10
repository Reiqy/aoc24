namespace AoC24;

public class Problem7
{
    public long SolveA()
    {
        var lines = File.ReadAllLines("input/aoc24_7.txt");

        var totalCalibrationResult = 0L;
        foreach (var line in lines)
        {
            totalCalibrationResult += this.GetIncrement(line, [Operator.Add, Operator.Multiply]);
        }

        return totalCalibrationResult;
    }

    public long SolveB()
    {
        var lines = File.ReadAllLines("input/aoc24_7.txt");

        var totalCalibrationResult = 0L;
        foreach (var line in lines)
        {
            totalCalibrationResult += this.GetIncrement(line, [Operator.Add, Operator.Multiply, Operator.Concat]);
        }

        return totalCalibrationResult;
    }

    private long GetIncrement(string line, Operator[] allowedOperators)
    {
        var lineParts = line.Split(':');
        var result = long.Parse(lineParts[0]);
        var numbers = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        return this.IsPossible(result, numbers, allowedOperators) ? result : 0;
    }

    private bool IsPossible(long result, long[] numbers, Operator[] allowedOperators)
    {
        if (numbers.Length == 1)
        {
            if (numbers[0] == result)
            {
                return true;
            }

            return false;
        }

        var operatorPossibilities = Enumerable.Repeat(allowedOperators, numbers.Length - 1);
        foreach (var possibility in this.Product(operatorPossibilities))
        {
            var partialResult = numbers[0];
            foreach (var (op, number) in possibility.Zip(numbers.Skip(1)))
            {
                partialResult = op switch
                {
                    Operator.Add => partialResult + number,
                    Operator.Multiply => partialResult * number,
                    Operator.Concat => long.Parse($"{partialResult}{number}"),
                    _ => throw new NotImplementedException(),
                };

                if (partialResult > result)
                {
                    break;
                }
            }

            if (partialResult == result)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerable<IEnumerable<T>> Product<T>(IEnumerable<IEnumerable<T>> sequences)
    {
        if (!sequences.Any())
        {
            yield return Enumerable.Empty<T>();
            yield break;
        }

        var firstSequence = sequences.First();
        var restSequences = sequences.Skip(1);

        foreach (var first in firstSequence)
        {
            foreach (var rest in this.Product(restSequences))
            {
                yield return new[] { first }.Concat(rest);
            }
        }
    }

    private enum Operator
    {
        Add,
        Multiply,
        Concat,
    }
}
