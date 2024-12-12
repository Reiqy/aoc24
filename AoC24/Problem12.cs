namespace AoC24;

public class Problem12
{
    public int SolveA()
    {
        var input = File.ReadAllLines("input/aoc24_12.txt");
        var height = input.Length;
        var width = input[0].Length;

        var map = new char[width, height];
        foreach (var (line, y) in input.Select((l, y) => (l, y)))
        {
            foreach (var (symbol, x) in line.Select((s, x) => (s, x)))
            {
                map[x, y] = symbol;
            }
        }

        var totalPrice = 0;

        var exploredPoints = new bool[width, height];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (exploredPoints[x, y])
                {
                    continue;
                }

                var plant = map[x, y];

                var perimeter = 0;
                var area = 0;

                var pointsToExplore = new HashSet<(int X, int Y)>
                {
                    (x, y)
                };

                while (pointsToExplore.Count > 0)
                {
                    area += 1;
                    var current = pointsToExplore.First();
                    pointsToExplore.Remove(current);
                    var currentPerimeter = 0;

                    exploredPoints[current.X, current.Y] = true;

                    var leftX = current.X - 1;
                    var leftY = current.Y;
                    if (leftX < 0 || leftX >= width || map[leftX, leftY] != plant)
                    {
                        currentPerimeter++;
                    }
                    else if (!exploredPoints[leftX, leftY])
                    {
                        pointsToExplore.Add((leftX, leftY));
                    }

                    var rightX = current.X + 1;
                    var rightY = current.Y;
                    if (rightX < 0 || rightX >= width || map[rightX, rightY] != plant)
                    {
                        currentPerimeter++;
                    }
                    else if (!exploredPoints[rightX, rightY])
                    {
                        pointsToExplore.Add((rightX, rightY));
                    }

                    var upX = current.X;
                    var upY = current.Y - 1;
                    if (upY < 0 || upY >= height || map[upX, upY] != plant)
                    {
                        currentPerimeter++;
                    }
                    else if (!exploredPoints[upX, upY])
                    {
                        pointsToExplore.Add((upX, upY));
                    }

                    var downX = current.X;
                    var downY = current.Y + 1;
                    if (downY < 0 || downY >= height || map[downX, downY] != plant)
                    {
                        currentPerimeter++;
                    }
                    else if (!exploredPoints[downX, downY])
                    {
                        pointsToExplore.Add((downX, downY));
                    }

                    perimeter += currentPerimeter;
                }

                var price = area * perimeter;
                totalPrice += price;
            }
        }

        return totalPrice;
    }

    public int SolveB()
    {
        var input = File.ReadAllLines("input/aoc24_12.txt");
        var height = input.Length;
        var width = input[0].Length;

        var map = new char[width, height];
        foreach (var (line, y) in input.Select((l, y) => (l, y)))
        {
            foreach (var (symbol, x) in line.Select((s, x) => (s, x)))
            {
                map[x, y] = symbol;
            }
        }

        var pointsToExplore = new HashSet<(int X, int Y)>();
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                pointsToExplore.Add((x, y));
            }
        }

        var areas = new Dictionary<int, int>();

        var regionMap = new int[width, height];

        var currentRegionIndex = 0;
        while (pointsToExplore.Count > 0)
        {
            var current = pointsToExplore.Last();

            var currentPlant = map[current.X, current.Y];

            areas.Add(currentRegionIndex, 0);

            var regionPointsToExplore = new HashSet<(int X, int Y)>
            {
                current
            };

            while (regionPointsToExplore.Count > 0)
            {
                var currentRegionPoint = regionPointsToExplore.Last();
                regionPointsToExplore.Remove(currentRegionPoint);
                pointsToExplore.Remove(currentRegionPoint);
                areas[currentRegionIndex]++;

                regionMap[currentRegionPoint.X, currentRegionPoint.Y] = currentRegionIndex;

                var left = (X: currentRegionPoint.X - 1, Y: currentRegionPoint.Y);
                if (pointsToExplore.Contains(left))
                {
                    if (map[left.X, left.Y] == currentPlant)
                    {
                        regionPointsToExplore.Add(left);
                    }
                }

                var right = (X: currentRegionPoint.X + 1, Y: currentRegionPoint.Y);
                if (pointsToExplore.Contains(right))
                {
                    if (map[right.X, right.Y] == currentPlant)
                    {
                        regionPointsToExplore.Add(right);
                    }
                }

                var up = (X: currentRegionPoint.X, Y: currentRegionPoint.Y - 1);
                if (pointsToExplore.Contains(up))
                {
                    if (map[up.X, up.Y] == currentPlant)
                    {
                        regionPointsToExplore.Add(up);
                    }
                }

                var down = (X: currentRegionPoint.X, Y: currentRegionPoint.Y + 1);
                if (pointsToExplore.Contains(down))
                {
                    if (map[down.X, down.Y] == currentPlant)
                    {
                        regionPointsToExplore.Add(down);
                    }
                }
            }

            currentRegionIndex++;
        }

        var regions = Enumerable.Range(0, currentRegionIndex).ToArray();
        var sides = regions.ToDictionary(x => x, x => 0);
        for (var y = 0; y < height; y++)
        {
            bool newTop = true;
            bool newBottom = true;
            var previousRegion = -1;

            for (var x = 0; x < width; x++)
            {
                var pointRegion = regionMap[x, y];
                if (pointRegion != previousRegion)
                {
                    previousRegion = pointRegion;
                    newTop = true;
                    newBottom = true;
                }

                var upY = y - 1;
                if (upY < 0 || upY >= height || regionMap[x, upY] != pointRegion)
                {
                    if (newTop)
                    {
                        sides[pointRegion]++;
                        newTop = false;
                    }
                }
                else
                {
                    newTop = true;
                }

                var downY = y + 1;
                if (downY < 0 || downY >= height || regionMap[x, downY] != pointRegion)
                {
                    if (newBottom)
                    {
                        sides[pointRegion]++;
                        newBottom = false;
                    }
                }
                else
                {
                    newBottom = true;
                }
            }
        }

        for (var x = 0; x < width; x++)
        {
            bool newLeft = true;
            bool newRight = true;
            var previousRegion = -1;

            for (var y = 0; y < height; y++)
            {
                var pointRegion = regionMap[x, y];
                if (pointRegion != previousRegion)
                {
                    previousRegion = pointRegion;
                    newLeft = true;
                    newRight = true;
                }

                var leftX = x - 1;
                if (leftX < 0 || leftX >= width || regionMap[leftX, y] != pointRegion)
                {
                    if (newLeft)
                    {
                        sides[pointRegion]++;
                        newLeft = false;
                    }
                }
                else
                {
                    newLeft = true;
                }

                var rightX = x + 1;
                if (rightX < 0 || rightX >= width || regionMap[rightX, y] != pointRegion)
                {
                    if (newRight)
                    {
                        sides[pointRegion]++;
                        newRight = false;
                    }
                }
                else
                {
                    newRight = true;
                }
            }
        }

        return regions.Select(x => areas[x] * sides[x]).Sum();
    }
}
