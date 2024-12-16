namespace AoC24;

public class Problem15
{
    public ulong SolveA()
    {
        var input = File.ReadAllLines("input/aoc24_15.txt")
            .Select((l, i) => (l, i)).ToArray();
        var height = input.First(x => string.IsNullOrWhiteSpace(x.l)).i;
        var width = input[0].l.Length;
        var map = new char[width, height];

        var robot = new Vector2(-1, -1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = input[y].l[x];

                if (map[x, y] == '@')
                {
                    robot = new Vector2(x, y);
                }
            }
        }

        foreach (var line in input.Where(x => x.i > height).Select(x => x.l))
        {
            foreach (char instruction in line)
            {
                var direction = instruction switch
                {
                    '^' => new Vector2(0, -1),
                    '>' => new Vector2(1, 0),
                    'v' => new Vector2(0, 1),
                    '<' => new Vector2(-1, 0),
                    _ => throw new NotImplementedException(),
                };

                // Look in direction for blank or wall
                var lookVector = robot;
                int steps = 0;
                bool invalid = false;
                while (true)
                {
                    if (map[lookVector.X, lookVector.Y] == '#')
                    {
                        invalid = true;
                        break;
                    }

                    if (map[lookVector.X, lookVector.Y] == '.')
                    {
                        break;
                    }

                    lookVector = lookVector.Add(direction);
                    steps++;
                }

                if (invalid)
                {
                    continue;
                }

                //for (var y = 0; y < height; y++)
                //{
                //    for (var x = 0; x < width; x++)
                //    {
                //        Console.Write(map[x, y]);
                //    }

                //    Console.WriteLine();
                //}

                var current = lookVector;
                for (int i = 0; i < steps; i++)
                {
                    var previous = current.Subtract(direction);
                    map[current.X, current.Y] = map[previous.X, previous.Y];
                    current = previous;
                }

                map[robot.X, robot.Y] = '.';
                //Console.WriteLine(robot);
                robot = robot.Add(direction);
                //Console.WriteLine(robot);

                //for (var y = 0; y < height; y++)
                //{
                //    for (var x = 0; x < width; x++)
                //    {
                //        Console.Write(map[x, y]);
                //    }

                //    Console.WriteLine();
                //}
            }
        }

        ulong gps = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[x, y] == 'O')
                {
                    gps += (100UL * (ulong)y + (ulong)x);
                }
            }
        }

        return gps;
    }

    private readonly record struct Vector2(int X, int Y)
    {
        public Vector2 Add(Vector2 vector)
        {
            return new Vector2(this.X + vector.X, this.Y + vector.Y);
        }

        public Vector2 Subtract(Vector2 vector)
        {
            return new Vector2(this.X - vector.X, this.Y - vector.Y);
        }
    }

    public ulong SolveB()
    {
        var input = File.ReadAllLines("input/aoc24_15.txt")
            .Select((l, i) => (l, i)).ToArray();
        var inHeight = input.First(x => string.IsNullOrWhiteSpace(x.l)).i;
        var inWidth = input[0].l.Length;
        var height = inHeight;
        var width = inWidth * 2;

        var map = new char[width, height];

        var robot = new Vector2(-1, -1);
        for (int y = 0; y < inHeight; y++)
        {
            for (int x = 0; x < inWidth; x++)
            {
                var inputSymbol = input[y].l[x];
                if (inputSymbol == '@')
                {
                    robot = new Vector2(2 * x, y);
                    map[2 * x, y] = '@';
                    map[2 * x + 1, y] = '.';
                    continue;
                }

                if (inputSymbol == '#')
                {
                    map[2 * x, y] = '#';
                    map[2 * x + 1, y] = '#';
                    continue;
                }

                if (inputSymbol == '.')
                {
                    map[2 * x, y] = '.';
                    map[2 * x + 1, y] = '.';
                    continue;
                }

                if (inputSymbol == 'O')
                {
                    map[2 * x, y] = '[';
                    map[2 * x + 1, y] = ']';
                    continue;
                }
            }
        }

        foreach (var line in input.Where(x => x.i > height).Select(x => x.l))
        {
            foreach (char instruction in line)
            {
                var direction = instruction switch
                {
                    '^' => new Vector2(0, -1),
                    '>' => new Vector2(1, 0),
                    'v' => new Vector2(0, 1),
                    '<' => new Vector2(-1, 0),
                    _ => throw new NotImplementedException(),
                };

                if (!this.IsMoveAllowed(map, MapObject.Robot, robot, direction))
                {
                    continue;
                }

                this.MoveRobot(map, robot, direction);
                robot = robot.Add(direction);
            }
        }

        ulong gps = 0UL;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[x, y] == '[')
                {
                    gps += (100UL * (ulong)y + (ulong)x);
                }
            }
        }

        return gps;
    }

    // The following functions are total madness.
    private bool IsMoveAllowed(char[,] map, MapObject objectType, Vector2 position, Vector2 direction, bool boxCheck = true)
    {
        var newPosition = position.Add(direction);
        switch (objectType)
        {
            case MapObject.Robot:
                return map[newPosition.X, newPosition.Y] switch
                {
                    '.' => true,
                    '[' => this.IsMoveAllowed(map, MapObject.BoxLeft, newPosition, direction),
                    ']' => this.IsMoveAllowed(map, MapObject.BoxRight, newPosition, direction),
                    _ => false,
                };
            case MapObject.BoxLeft:
                if (direction.Y == 0)
                {
                    return map[newPosition.X, newPosition.Y] switch
                    {
                        '.' => true,
                        ']' => this.IsMoveAllowed(map, MapObject.BoxRight, newPosition, direction),
                        _ => false,
                    };
                }

                return map[newPosition.X, newPosition.Y] switch
                {
                    '.' => !boxCheck || this.IsMoveAllowed(map, MapObject.BoxRight, position.Add(new Vector2(1, 0)), direction, boxCheck: false),
                    '[' => this.IsMoveAllowed(map, MapObject.BoxLeft, newPosition, direction) && (!boxCheck || this.IsMoveAllowed(map, MapObject.BoxRight, position.Add(new Vector2(1, 0)), direction, boxCheck: false)),
                    ']' => this.IsMoveAllowed(map, MapObject.BoxRight, newPosition, direction) && (!boxCheck || this.IsMoveAllowed(map, MapObject.BoxRight, position.Add(new Vector2(1, 0)), direction, boxCheck: false)),
                    _ => false,
                };
            case MapObject.BoxRight:
                if (direction.Y == 0)
                {
                    return map[newPosition.X, newPosition.Y] switch
                    {
                        '.' => true,
                        '[' => this.IsMoveAllowed(map, MapObject.BoxLeft, newPosition, direction),
                        _ => false,
                    };
                }

                return map[newPosition.X, newPosition.Y] switch
                {
                    '.' => !boxCheck || this.IsMoveAllowed(map, MapObject.BoxLeft, position.Add(new Vector2(-1, 0)), direction, boxCheck: false),
                    '[' => this.IsMoveAllowed(map, MapObject.BoxLeft, newPosition, direction) && (!boxCheck || this.IsMoveAllowed(map, MapObject.BoxLeft, position.Add(new Vector2(-1, 0)), direction, boxCheck: false)),
                    ']' => this.IsMoveAllowed(map, MapObject.BoxRight, newPosition, direction) && (!boxCheck || this.IsMoveAllowed(map, MapObject.BoxLeft, position.Add(new Vector2(-1, 0)), direction, boxCheck: false)),
                    _ => false,
                };
            default:
                break;
        }

        throw new NotImplementedException();
    }

    private void MoveSomething(char[,] map, Vector2 position, Vector2 direction)
    {
        switch (map[position.X, position.Y])
        {
            case '.':
                return;
            case '[':
                this.MoveBoxLeft(map, position, direction);
                return;
            case ']':
                this.MoveBoxRight(map, position, direction);
                return;
            default:
                break;
        }

        throw new NotImplementedException();
    }

    private void MoveRobot(char[,] map, Vector2 position, Vector2 direction)
    {
        var newPosition = position.Add(direction);
        this.MoveSomething(map, newPosition, direction);
        map[newPosition.X, newPosition.Y] = '@';
        map[position.X, position.Y] = '.';
    }

    private void MoveBoxLeft(char[,] map, Vector2 position, Vector2 direction, bool boxCheck = true)
    {
        var newPosition = position.Add(direction);
        if (direction.Y == 0)
        {
            this.MoveSomething(map, newPosition, direction);
            map[newPosition.X, newPosition.Y] = '[';
            map[position.X, position.Y] = '.';
            return;
        }

        if (boxCheck)
        {
            this.MoveBoxRight(map, position.Add(new Vector2(1, 0)), direction, boxCheck: false);
        }

        this.MoveSomething(map, newPosition, direction);
        map[newPosition.X, newPosition.Y] = '[';
        map[position.X, position.Y] = '.';
    }

    private void MoveBoxRight(char[,] map, Vector2 position, Vector2 direction, bool boxCheck = true)
    {
        var newPosition = position.Add(direction);
        if (direction.Y == 0)
        {
            this.MoveSomething(map, newPosition, direction);
            map[newPosition.X, newPosition.Y] = ']';
            map[position.X, position.Y] = '.';
            return;
        }

        if (boxCheck)
        {
            this.MoveBoxLeft(map, position.Add(new Vector2(-1, 0)), direction, boxCheck: false);
        }

        this.MoveSomething(map, newPosition, direction);
        map[newPosition.X, newPosition.Y] = ']';
        map[position.X, position.Y] = '.';
    }

    private enum MapObject
    {
        Robot,
        BoxLeft,
        BoxRight,
    }
}
