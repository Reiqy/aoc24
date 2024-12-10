namespace AoC24;

public class Problem8
{
    public int SolveA()
    {
        var lines = File.ReadAllLines("input/aoc24_8.txt");

        var height = lines.Length;
        var width = lines[0].Length;

        var antennas = new Dictionary<char, List<Antenna>>();
        foreach (var (y, line) in lines.Select((line, y) => (y, line)))
        {
            foreach (var (x, symbol) in line.Select((symbol, x) => (x, symbol)))
            {
                if (symbol == '.')
                {
                    continue;
                }

                var antennaSymbol = symbol;
                if (!antennas.ContainsKey(antennaSymbol))
                {
                    antennas.Add(antennaSymbol, new List<Antenna>());
                }

                antennas[antennaSymbol].Add(new Antenna(x, y));
            }
        }

        var antinodes = new HashSet<(int, int)>();
        foreach (var antennaCategory in antennas)
        {
            var category = antennaCategory.Key;
            var categoryAntennas = antennaCategory.Value;
            foreach (var (i, firstAntenna) in categoryAntennas.Select((a, i) => (i, a)))
            {
                foreach (var (j, secondAntenna) in categoryAntennas.Select((a, j) => (j, a)))
                {
                    if (i == j)
                    {
                        continue;
                    }

                    // B - A = A -> B
                    // first - second = second -> first
                    var antennaVectorX = firstAntenna.X - secondAntenna.X;
                    var antennaVectorY = firstAntenna.Y - secondAntenna.Y;

                    var firstAntinodeX = firstAntenna.X + antennaVectorX;
                    var firstAntinodeY = firstAntenna.Y + antennaVectorY;

                    if (firstAntinodeX >= 0 && firstAntinodeX < width && firstAntinodeY >= 0 && firstAntinodeY < height)
                    {
                        antinodes.Add((firstAntinodeX, firstAntinodeY));
                    }

                    var secondAntinodeX = secondAntenna.X - antennaVectorX;
                    var secondAntinodeY = secondAntenna.Y - antennaVectorY;

                    if (secondAntinodeX >= 0 && secondAntinodeX < width && secondAntinodeY >= 0 && secondAntinodeY < height)
                    {
                        antinodes.Add((secondAntinodeX, secondAntinodeY));
                    }
                }
            }
        }

        return antinodes.Count;
    }

    private record Antenna(int X, int Y);

    public int SolveB()
    {
        var lines = File.ReadAllLines("input/aoc24_8.txt");

        var height = lines.Length;
        var width = lines[0].Length;

        var antennas = new Dictionary<char, List<Antenna>>();
        foreach (var (y, line) in lines.Select((line, y) => (y, line)))
        {
            foreach (var (x, symbol) in line.Select((symbol, x) => (x, symbol)))
            {
                if (symbol == '.')
                {
                    continue;
                }

                var antennaSymbol = symbol;
                if (!antennas.ContainsKey(antennaSymbol))
                {
                    antennas.Add(antennaSymbol, new List<Antenna>());
                }

                antennas[antennaSymbol].Add(new Antenna(x, y));
            }
        }

        var antinodes = new HashSet<(int, int)>();
        foreach (var antennaCategory in antennas)
        {
            var category = antennaCategory.Key;
            var categoryAntennas = antennaCategory.Value;
            foreach (var (i, firstAntenna) in categoryAntennas.Select((a, i) => (i, a)))
            {
                foreach (var (j, secondAntenna) in categoryAntennas.Select((a, j) => (j, a)))
                {
                    if (i == j)
                    {
                        continue;
                    }

                    // B - A = A -> B
                    // first - second = second -> first
                    var antennaVectorX = firstAntenna.X - secondAntenna.X;
                    var antennaVectorY = firstAntenna.Y - secondAntenna.Y;
                    var divisor = GCD(antennaVectorX, antennaVectorY); // Is this even needed?

                    var antennaStepVectorX = antennaVectorX / divisor;
                    var antennaStepVectorY = antennaVectorY / divisor;

                    var antinodeX = firstAntenna.X;
                    var antinodeY = firstAntenna.Y;
                    while (antinodeX >= 0 && antinodeX < width && antinodeY >= 0 && antinodeY < height)
                    {
                        antinodes.Add((antinodeX, antinodeY));

                        antinodeX += antennaStepVectorX;
                        antinodeY += antennaStepVectorY;
                    }

                    antinodeX = firstAntenna.X;
                    antinodeY = firstAntenna.Y;
                    while (antinodeX >= 0 && antinodeX < width && antinodeY >= 0 && antinodeY < height)
                    {
                        antinodes.Add((antinodeX, antinodeY));

                        antinodeX -= antennaStepVectorX;
                        antinodeY -= antennaStepVectorY;
                    }
                }
            }
        }

        return antinodes.Count;
    }

    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return Math.Abs(a);
    }
}
