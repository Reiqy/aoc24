namespace AoC24;

public class Problem4
{
    public int SolveA()
    {
        var input = File.ReadAllLines("input/aoc24_4.txt").Select(x => x.ToCharArray()).ToArray();

        var count = 0;
        for (int y = 0; y < input.Length; y++)
        {
            var line = input[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'X')
                {
                    count += this.SearchInAllDirections(input, "MAS", x, y);
                }
            }
        }

        return count;
    }

    private int SearchInAllDirections(char[][] input, string query, int x, int y)
    {
        int count = 0;
        for (int directionX = -1; directionX <= 1; directionX++)
        {
            for (int directionY = -1; directionY <= 1; directionY++)
            {
                if (directionX == 0 && directionY == 0)
                {
                    continue;
                }

                if (this.SearchInDirection(input, query, x, y, directionX, directionY))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool SearchInDirection(char[][] input, string query, int x, int y, int directionX, int directionY)
    {
        if (query.Length == 0)
        {
            return true;
        }

        var newY = y + directionY;
        if (newY < 0 || newY >= input.Length)
        {
            return false;
        }

        var line = input[newY];

        var newX = x + directionX;
        if (newX < 0 || newX >= line.Length)
        {
            return false;
        }

        if (query[0] != line[newX])
        {
            return false;
        }

        return this.SearchInDirection(input, query.Substring(1, query.Length - 1), newX, newY, directionX, directionY);
    }

    public int SolveB()
    {
        var input = File.ReadAllLines("input/aoc24_4.txt").Select(x => x.ToCharArray()).ToArray();

        var count = 0;
        for (int y = 0; y < input.Length; y++)
        {
            var line = input[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'A' && this.IsXMas(input, x, y))
                {
                    count ++;
                }
            }
        }

        return count;
    }

    private bool IsXMas(char[][] input, int x, int y)
    {
        if ((this.SearchInDirection(input, "M", x, y, -1, -1) && this.SearchInDirection(input, "S", x, y, 1, 1))
            || (this.SearchInDirection(input, "S", x, y, -1, -1) && this.SearchInDirection(input, "M", x, y, 1, 1)))
        {
            if ((this.SearchInDirection(input, "M", x, y, 1, -1) && this.SearchInDirection(input, "S", x, y, -1, 1))
                || (this.SearchInDirection(input, "S", x, y, 1, -1) && this.SearchInDirection(input, "M", x, y, -1, 1)))
            {
                return true;
            }
        }

        return false;
    }
}
