namespace AoC24;

public class Problem6
{
    public int SolveA()
    {
        var lines = File.ReadAllLines("input/aoc24_6.txt");
        var map = new Map(lines);
        var guard = map.SpawnGuard();

        var visitedTiles = new HashSet<(int, int)>();
        while (guard.IsInBounds(map.Width, map.Height))
        {
            visitedTiles.Add((guard.X, guard.Y));

            if (guard.IsPeekInBounds(map.Width, map.Height))
            {
                var (guardNextX, guardNextY) = guard.Peek();
                if (map.GetTileObject(guardNextX, guardNextY) != TileObject.Obstacle)
                {
                    guard.Move();
                    continue;
                }

                guard.Rotate();
                continue;
            }

            guard.Move();
        }

        return visitedTiles.Count;
    }

    // The following brute-force solution is definitely not very optimal.
    public int SolveB()
    {
        var lines = File.ReadAllLines("input/aoc24_6.txt");
        var map = new Map(lines);

        var possibleLoopsCount = 0;
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                bool isEligibleForLoopTest = map.GetTileObject(x, y) != TileObject.Obstacle;
                if (!isEligibleForLoopTest)
                {
                    continue;
                }

                map.SpawnObstacle(x, y);

                var guard = map.SpawnGuard();
                var visitedTiles = new HashSet<(int x, int y, DirectionVector fromDirection)>();
                while (guard.IsInBounds(map.Width, map.Height))
                {
                    var currentX = guard.X;
                    var currentY = guard.Y;
                    var currentDirection = guard.Direction;

                    if (visitedTiles.Contains((currentX, currentY, currentDirection)))
                    {
                        possibleLoopsCount++;
                        break;
                    }

                    visitedTiles.Add((guard.X, guard.Y, guard.Direction));
                    if (guard.IsPeekInBounds(map.Width, map.Height))
                    {
                        var (guardNextX, guardNextY) = guard.Peek();
                        if (map.GetTileObject(guardNextX, guardNextY) != TileObject.Obstacle)
                        {
                            guard.Move();
                            continue;
                        }

                        guard.Rotate();
                        continue;
                    }

                    guard.Move();
                }

                map.DespawnObstacle(x, y);
            }
        }

        return possibleLoopsCount;
    }

    private enum TileObject
    {
        Nothing,
        Obstacle,
    }

    private readonly record struct DirectionVector(int X, int Y)
    {
        public static DirectionVector Up
        {
            get => new(0, -1);
        }

        public static DirectionVector Down
        {
            get => new(0, 1);
        }

        public static DirectionVector Left
        {
            get => new(-1, 0);
        }

        public static DirectionVector Right
        {
            get => new(1, 0);
        }

        public DirectionVector Rotate()
        {
            return (this.X, this.Y) switch
            {
                (0, -1) => Right,
                (1, 0) => Down,
                (0, 1) => Left,
                (-1, 0) => Up,
                _ => throw new NotImplementedException(),
            };
        }
    }

    private class Map
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        private readonly TileObject[,] map;

        private int guardStartX, guardStartY;

        public Map(string[] lines)
        {
            this.Height = lines.Length;
            this.Width = lines[0].Length;
            this.map = new TileObject[this.Width, this.Height];
            foreach (var (y, line) in lines.Select((line, y) => (y, line)))
            {
                foreach (var (x, symbol) in line.Select((symbol, x) => (x, symbol)))
                {
                    if (symbol == '#')
                    {
                        this.map[x, y] = TileObject.Obstacle;
                        continue;
                    }

                    if (symbol == '^')
                    {
                        this.guardStartX = x;
                        this.guardStartY = y;
                    }

                    this.map[x, y] = TileObject.Nothing;
                }
            }
        }

        public Guard SpawnGuard()
        {
            return new Guard(this.guardStartX, this.guardStartY, DirectionVector.Up);
        }

        public TileObject GetTileObject(int x, int y)
        {
            return this.map[x, y];
        }

        public void SpawnObstacle(int x, int y)
        {
            this.map[x, y] = TileObject.Obstacle;
        }

        public void DespawnObstacle(int x, int y)
        {
            this.map[x, y] = TileObject.Nothing;
        }
    }

    private class Guard
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public DirectionVector Direction { get; private set; }

        public Guard(int startX, int startY, DirectionVector direction)
        {
            this.X = startX;
            this.Y = startY;
            this.Direction = direction;
        }

        public bool IsInBounds(int width, int height)
        {
            return IsInBounds(width, height, this.X, this.Y);
        }

        public bool IsPeekInBounds(int width, int height)
        {
            var (peekX, peekY) = this.Peek();
            return IsInBounds(width, height, peekX, peekY);
        }

        private static bool IsInBounds(int width, int height, int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public (int newX, int newY) Peek()
        {
            return (this.X + this.Direction.X, this.Y + this.Direction.Y);
        }

        public void Move()
        {
            this.X += this.Direction.X;
            this.Y += this.Direction.Y;
        }

        public void Rotate()
        {
            this.Direction = this.Direction.Rotate();
        }
    }
}
