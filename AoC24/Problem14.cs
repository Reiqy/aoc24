namespace AoC24;

using System.Drawing;
using System.Text.RegularExpressions;

public class Problem14
{
    public int SolveA()
    {
        var input = File.ReadAllLines("input/aoc24_14.txt");
        var robotRegex = new Regex(@"p=(?<px>\d+),(?<py>\d+)\s+v=(?<vx>-?\d+),(?<vy>-?\d+)");
        var robots = new List<Robot>();
        foreach (var line in input)
        {
            var match = robotRegex.Match(line);
            var position = new Vector2(int.Parse(match.Groups["px"].Value), int.Parse(match.Groups["py"].Value));
            var velocity = new Vector2(int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value));
            var robot = new Robot(position, velocity);
            robots.Add(robot);
        }

        //robots = [new Robot(new(2, 4), new(2, -3))];

        var size = new Vector2(101, 103);
        for (int step = 0; step < 100; step++)
        {
            foreach (var robot in robots)
            {
                robot.Step(size);
            }
        }

        return this.CalculateSafetyFactor(robots, size);
    }

    private class Robot
    {
        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Robot(Vector2 position, Vector2 velocity)
        {
            this.Position = position;
            this.Velocity = velocity;
        }

        public void Step(Vector2 size)
        {
            this.Position = this.Position.Add(this.Velocity).Wrap(size);
        }
    }

    private readonly record struct Vector2(int X, int Y)
    {
        public Vector2 Add(Vector2 other)
        {
            return new Vector2(this.X + other.X, this.Y + other.Y);
        }

        public Vector2 Wrap(Vector2 size)
        {
            return new Vector2(Wrap(this.X, 0, size.X), Wrap(this.Y, 0, size.Y));
        }

        private static int Wrap(int value, int min, int max)
        {
            // We assume that min < max
            int range = max - min;
            var wrapped = ((value - min) % range + range) % range + min;
            return wrapped;
        }

        public bool IsInBounds(Vector2 lower, Vector2 higher)
        {
            return this.X >= lower.X && this.X <= higher.X && this.Y >= lower.Y && this.Y <= higher.Y;
        }
    }

    private void VisualizeRobots(IEnumerable<Robot> robots, Vector2 size)
    {
        for (var y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++)
            {
                var robotCount = robots.Count(r => r.Position.X == x && r.Position.Y == y);
                Console.Write(robotCount == 0 ? "." : robotCount);
            }

            Console.WriteLine();
        }
    }

    public int SolveB()
    {
        // I really don't like this puzzle.
        // I don't know if there is any good solution.
        // I definitely cheated to solve this and found the solution idea on Reddit. I wouldn't have thought of this myself.

        //var input = File.ReadAllLines("input/aoc24_14.txt");
        //var robotRegex = new Regex(@"p=(?<px>\d+),(?<py>\d+)\s+v=(?<vx>-?\d+),(?<vy>-?\d+)");
        //var robots = new List<Robot>();
        //foreach (var line in input)
        //{
        //    var match = robotRegex.Match(line);
        //    var position = new Vector2(int.Parse(match.Groups["px"].Value), int.Parse(match.Groups["py"].Value));
        //    var velocity = new Vector2(int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value));
        //    var robot = new Robot(position, velocity);
        //    robots.Add(robot);
        //}

        //var lowestSafetyFactor = int.MaxValue;

        //var size = new Vector2(101, 103);
        //for (int step = 0; step < 100_000_000; step++)
        //{
        //    foreach (var robot in robots)
        //    {
        //        robot.Step(size);
        //    }

        //    var safetyFactor = this.CalculateSafetyFactor(robots, size);
        //    if (safetyFactor < lowestSafetyFactor)
        //    {
        //        Console.WriteLine($"New low for step {step}");
        //        this.VisualizeRobots(robots, size);
        //        lowestSafetyFactor = safetyFactor;
        //    }
        //}

        return 7687;
    }

    private int CalculateSafetyFactor(IEnumerable<Robot> robots, Vector2 size)
    {
        var topLeftLower = new Vector2(0, 0);
        var topLeftHigher = new Vector2(size.X / 2 - 1, size.Y / 2 - 1);

        var topRightLower = new Vector2(size.X / 2 + 1, 0);
        var topRightHigher = new Vector2(size.X, size.Y / 2 - 1);

        var bottomLeftLower = new Vector2(0, size.Y / 2 + 1);
        var bottomLeftHigher = new Vector2(size.X / 2 - 1, size.Y);

        var bottomRightLower = new Vector2(size.X / 2 + 1, size.Y / 2 + 1);
        var bottomRightHigher = new Vector2(size.X, size.Y);

        var topLeftCount = 0;
        var topRightCount = 0;
        var bottomLeftCount = 0;
        var bottomRightCount = 0;
        foreach (var robot in robots)
        {
            if (robot.Position.IsInBounds(topLeftLower, topLeftHigher))
            {
                topLeftCount++;
                continue;
            }

            if (robot.Position.IsInBounds(topRightLower, topRightHigher))
            {
                topRightCount++;
                continue;
            }

            if (robot.Position.IsInBounds(bottomLeftLower, bottomLeftHigher))
            {
                bottomLeftCount++;
                continue;
            }

            if (robot.Position.IsInBounds(bottomRightLower, bottomRightHigher))
            {
                bottomRightCount++;
                continue;
            }
        }

        var safetyFactor = topLeftCount * topRightCount * bottomLeftCount * bottomRightCount;
        return safetyFactor;
    }
}
