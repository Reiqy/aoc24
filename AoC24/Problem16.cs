namespace AoC24;

public class Problem16
{
    public ulong SolveA()
    {
        var input = File.ReadAllLines("input/aoc24_16.txt");
        var height = input.Length;
        var width = input[0].Length;
        var map = new char[width, height];
        var start = new Vector2();
        var end = new Vector2();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var currentInput = input[y][x];
                if (currentInput == 'S')
                {
                    start = new Vector2(x, y);
                    currentInput = '.';
                }
                else if (currentInput == 'E')
                {
                    end = new Vector2(x, y);
                    currentInput = '.';
                }

                map[x, y] = currentInput;
            }
        }

        var startNode = new Node(start, Rotation.Right);
        var endNode = new Node(end, Rotation.Right); // Final rotation is not important
        
        IEnumerable<Node> Explore(Node node)
        {
            var direction = node.Rotation switch
            {
                Rotation.Up => new Vector2(0, -1),
                Rotation.Down => new Vector2(0, 1),
                Rotation.Left => new Vector2(-1, 0),
                Rotation.Right => new Vector2(1, 0),
                _ => throw new NotImplementedException(),
            };

            var possibleNewPosition = node.Position.Add(direction);
            if (map[possibleNewPosition.X, possibleNewPosition.Y] == '.')
            {
                yield return new Node(possibleNewPosition, node.Rotation);
            }

            var rotationClockwise = node.Rotation switch
            {
                Rotation.Up => Rotation.Right,
                Rotation.Down => Rotation.Left,
                Rotation.Left => Rotation.Up,
                Rotation.Right => Rotation.Down,
                _ => throw new NotImplementedException(),
            };

            yield return new Node(node.Position, rotationClockwise);

            var rotationCounterClockwise = node.Rotation switch
            {
                Rotation.Up => Rotation.Left,
                Rotation.Down => Rotation.Right,
                Rotation.Left => Rotation.Down,
                Rotation.Right => Rotation.Up,
                _ => throw new NotImplementedException(),
            };

            yield return new Node(node.Position, rotationCounterClockwise);
        }

        ulong Cost(Node first, Node second)
        {
            return first.Position != second.Position ? 1UL : 1000UL;
        }

        Rotation[] rotations = [Rotation.Up, Rotation.Down, Rotation.Left, Rotation.Right];

        ulong CalculatePathPrice(Node start, Vector2 end, Rotation endRotation, Func<Node, IEnumerable<Node>> explore, Func<Node, Node, ulong> cost)
        {
            var endNode = new Node(end, endRotation);
            return this.CalculatePathPrice(start, endNode, explore, cost);
        }

        return rotations.Min(x => CalculatePathPrice(startNode, endNode.Position, x, Explore, Cost));
    }

    public int SolveB()
    {
        var input = File.ReadAllLines("input/aoc24_16.txt");
        var height = input.Length;
        var width = input[0].Length;
        var map = new char[width, height];
        var start = new Vector2();
        var end = new Vector2();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var currentInput = input[y][x];
                if (currentInput == 'S')
                {
                    start = new Vector2(x, y);
                    currentInput = '.';
                }
                else if (currentInput == 'E')
                {
                    end = new Vector2(x, y);
                    currentInput = '.';
                }

                map[x, y] = currentInput;
            }
        }

        var startNode = new Node(start, Rotation.Right);
        var endNode = new Node(end, Rotation.Right); // Final rotation is not important

        IEnumerable<Node> Explore(Node node)
        {
            var direction = node.Rotation switch
            {
                Rotation.Up => new Vector2(0, -1),
                Rotation.Down => new Vector2(0, 1),
                Rotation.Left => new Vector2(-1, 0),
                Rotation.Right => new Vector2(1, 0),
                _ => throw new NotImplementedException(),
            };

            var possibleNewPosition = node.Position.Add(direction);
            if (map[possibleNewPosition.X, possibleNewPosition.Y] == '.')
            {
                yield return new Node(possibleNewPosition, node.Rotation);
            }

            var rotationClockwise = node.Rotation switch
            {
                Rotation.Up => Rotation.Right,
                Rotation.Down => Rotation.Left,
                Rotation.Left => Rotation.Up,
                Rotation.Right => Rotation.Down,
                _ => throw new NotImplementedException(),
            };

            yield return new Node(node.Position, rotationClockwise);

            var rotationCounterClockwise = node.Rotation switch
            {
                Rotation.Up => Rotation.Left,
                Rotation.Down => Rotation.Right,
                Rotation.Left => Rotation.Down,
                Rotation.Right => Rotation.Up,
                _ => throw new NotImplementedException(),
            };

            yield return new Node(node.Position, rotationCounterClockwise);
        }

        ulong Cost(Node first, Node second)
        {
            return first.Position != second.Position ? 1UL : 1000UL;
        }

        ulong Heuristic(Node first, Node second)
        {
            return (ulong)Math.Abs(first.Position.X - second.Position.X) + (ulong)Math.Abs(first.Position.Y - second.Position.Y);
        }

        Rotation[] rotations = [ Rotation.Up, Rotation.Down, Rotation.Left, Rotation.Right ];

        ulong CalculatePathPrice(Node start, Vector2 end, Rotation endRotation, Func<Node, IEnumerable<Node>> explore, Func<Node, Node, ulong> cost)
        {
            var endNode = new Node(end, endRotation);
            return this.CalculatePathPrice(start, endNode, explore, cost);
        }

        var limitPrice = rotations.Min(x => CalculatePathPrice(startNode, endNode.Position, x, Explore, Cost));

        var visitedPositions = new HashSet<Vector2>();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[x, y] != '.')
                {
                    continue;
                }

                ulong PriceThroughThisNode(Rotation rotation)
                {
                    var current = new Node(new Vector2(x, y), rotation);
                    return this.CalculatePathPrice(startNode, current, Explore, Cost, Heuristic) + rotations.Min(r =>
                    {
                        var endNodeVariant = new Node(endNode.Position, r);
                        return this.CalculatePathPrice(current, endNodeVariant, Explore, Cost, Heuristic);
                    });
                }

                var minPrice = rotations.Min(x => PriceThroughThisNode(x));
                if (minPrice <= limitPrice)
                {
                    visitedPositions.Add(new Vector2(x, y));
                }
            }
        }

        return visitedPositions.Count;
    }

    private ulong CalculatePathPrice(Node start, Node end, Func<Node, IEnumerable<Node>> explore, Func<Node, Node, ulong> cost, Func<Node, Node, ulong>? heuristic = null)
    {
        heuristic ??= (x, y) => 0;

        var open = new PriorityQueue<Node, ulong>();
        open.Enqueue(start, heuristic(start, end));

        var score = new Dictionary<Node, ulong>()
        {
            [ start ] = 0,
        };

        var closed = new HashSet<Node>();

        while (open.TryDequeue(out var currentNode, out var currentPriority))
        {
            if (currentNode == end)
            {
                return score[currentNode];
            }

            if (!closed.Add(currentNode))
            {
                continue;
            }

            foreach (var neighbor in explore(currentNode))
            {
                var gScore = score[currentNode] + cost(currentNode, neighbor);

                var previousGScore = score.GetValueOrDefault(neighbor, ulong.MaxValue);
                if (closed.Contains(neighbor) && gScore >= previousGScore)
                {
                    continue;
                }

                if (gScore < previousGScore)
                {
                    score[neighbor] = gScore;
                    var fScore = gScore + heuristic(neighbor, end);
                    open.Enqueue(neighbor, fScore);
                }
            }
        }

        throw new InvalidOperationException("Path not found.");
    }

    private record Node(Vector2 Position, Rotation Rotation);

    private readonly record struct Vector2(int X, int Y)
    {
        public Vector2 Add(Vector2 other)
        {
            return new Vector2(this.X + other.X, this.Y + other.Y);
        }
    }

    private enum Rotation
    {
        Up, Down, Left, Right,
    }
}
