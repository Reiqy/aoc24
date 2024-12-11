namespace AoC24;

public class Problem10
{
    public int SolveA()
    {
        var lines = File.ReadAllLines("input/aoc24_10.txt");
        var height = lines.Length;
        var width = lines[0].Length;

        var map = new byte[width, height];
        foreach (var (line, y) in lines.Select((l, y) => (l, y)))
        {
            foreach (var (symbol, x) in line.Select((s, x) => (s, x)))
            {
                var value = (byte)(symbol - '0');
                map[x, y] = value;
            }
        }

        var reachableTopsMap = new HashSet<(int X, int Y)>[width, height];

        for (int exploredHeight = 9; exploredHeight >= 0; exploredHeight--)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (map[x, y] != exploredHeight)
                    {
                        continue;
                    }

                    if (exploredHeight == 9)
                    {
                        reachableTopsMap[x, y] = [(x, y)];
                        continue;
                    }

                    var reachableTops = new HashSet<(int X, int Y)>();

                    // Left
                    var leftX = x - 1;
                    var leftY = y;
                    if (leftX >= 0 && leftX < width && map[leftX, leftY] == (exploredHeight + 1))
                    {
                        reachableTops.UnionWith(reachableTopsMap[leftX, leftY]);
                    }

                    // Right
                    var rightX = x + 1;
                    var rightY = y;
                    if (rightX >= 0 && rightX < width && map[rightX, rightY] == (exploredHeight + 1))
                    {
                        reachableTops.UnionWith(reachableTopsMap[rightX, rightY]);
                    }

                    // Up
                    var upX = x;
                    var upY = y - 1;
                    if (upY >= 0 && upY < height && map[upX, upY] == (exploredHeight + 1))
                    {
                        reachableTops.UnionWith(reachableTopsMap[upX, upY]);
                    }

                    // Down
                    var downX = x;
                    var downY = y + 1;
                    if (downY >= 0 && downY < height && map[downX, downY] == (exploredHeight + 1))
                    {
                        reachableTops.UnionWith(reachableTopsMap[downX, downY]);
                    }

                    reachableTopsMap[x, y] = reachableTops;
                }
            }
        }

        var count = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[x, y] != 0)
                {
                    continue;
                }

                count += reachableTopsMap[x, y].Count;
            }
        }

        return count;
    }

    public int SolveB()
    {
        var lines = File.ReadAllLines("input/aoc24_10.txt");
        var height = lines.Length;
        var width = lines[0].Length;

        var map = new byte[width, height];
        foreach (var (line, y) in lines.Select((l, y) => (l, y)))
        {
            foreach (var (symbol, x) in line.Select((s, x) => (s, x)))
            {
                var value = (byte)(symbol - '0');
                map[x, y] = value;
            }
        }

        var ratingMap = new int[width, height];

        for (int exploredHeight = 9; exploredHeight >= 0; exploredHeight--)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (map[x, y] != exploredHeight)
                    {
                        continue;
                    }

                    if (exploredHeight == 9)
                    {
                        ratingMap[x, y] = 1;
                        continue;
                    }

                    var rating = 0;

                    // Left
                    var leftX = x - 1;
                    var leftY = y;
                    if (leftX >= 0 && leftX < width && map[leftX, leftY] == (exploredHeight + 1))
                    {
                        rating += ratingMap[leftX, leftY];
                    }

                    // Right
                    var rightX = x + 1;
                    var rightY = y;
                    if (rightX >= 0 && rightX < width && map[rightX, rightY] == (exploredHeight + 1))
                    {
                        rating += ratingMap[rightX, rightY];
                    }

                    // Up
                    var upX = x;
                    var upY = y - 1;
                    if (upY >= 0 && upY < height && map[upX, upY] == (exploredHeight + 1))
                    {
                        rating += ratingMap[upX, upY];
                    }

                    // Down
                    var downX = x;
                    var downY = y + 1;
                    if (downY >= 0 && downY < height && map[downX, downY] == (exploredHeight + 1))
                    {
                        rating += ratingMap[downX, downY];
                    }

                    ratingMap[x, y] = rating;
                }
            }
        }

        var totalRating = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[x, y] != 0)
                {
                    continue;
                }

                totalRating += ratingMap[x, y];
            }
        }

        return totalRating;
    }
}
