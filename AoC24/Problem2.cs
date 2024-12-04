namespace AoC24;

public class Problem2
{
    public int SolveA()
    {
        var reports = File.ReadAllLines("input/aoc24_2.txt");

        int safeLevelsCount = 0;
        foreach (var report in reports)
        {
            var levels = report.Split(' ').Select(x => int.Parse(x)).ToArray();

            bool isSafe = true;
            int levelDifferenceSign = 0;
            for (int i = 1; i < levels.Length; i++)
            {
                var levelDifference = levels[i] - levels[i - 1];
                var absoluteLevelDifference = Math.Abs(levelDifference);
                if (absoluteLevelDifference < 1 || absoluteLevelDifference > 3)
                {
                    isSafe = false;
                    break;
                }

                var tmpLevelDifferenceSign = Math.Sign(levelDifference);
                if (levelDifferenceSign != 0 && tmpLevelDifferenceSign != levelDifferenceSign)
                {
                    isSafe = false;
                    break;
                }

                levelDifferenceSign = tmpLevelDifferenceSign;
            }

            if (isSafe)
            {
                safeLevelsCount++;
            }
        }

        return safeLevelsCount;
    }

    // The following solution is definitely suboptimal, but it works.
    public int SolveB()
    {
        var reports = File.ReadAllLines("input/aoc24_2.txt");

        int safeLevelsCount = 0;
        foreach (var report in reports)
        {
            var levels = report.Split(' ').Select(x => int.Parse(x)).ToArray();
            var reportLength = levels.Length;

            bool isSafe = false;
            for (int ignoreIndex = 0; ignoreIndex < reportLength; ignoreIndex++)
            {
                var levelsWithoutIgnore = levels
                    .Where((value, index) => index != ignoreIndex)
                    .ToArray();

                bool isCurrentSafe = true;
                int levelDifferenceSign = 0;
                for (int i = 1; i < reportLength - 1; i++)
                {
                    var levelDifference = levelsWithoutIgnore[i] - levelsWithoutIgnore[i - 1];
                    var absoluteLevelDifference = Math.Abs(levelDifference);
                    if (absoluteLevelDifference < 1 || absoluteLevelDifference > 3)
                    {
                        isCurrentSafe = false;
                        break;
                    }

                    var tmpLevelDifferenceSign = Math.Sign(levelDifference);
                    if (levelDifferenceSign != 0 && tmpLevelDifferenceSign != levelDifferenceSign)
                    {
                        isCurrentSafe = false;
                        break;
                    }

                    levelDifferenceSign = tmpLevelDifferenceSign;
                }

                if (isCurrentSafe)
                {
                    isSafe = true;
                    break;
                }
            }

            if (isSafe)
            {
                safeLevelsCount++;
            }
        }

        return safeLevelsCount;
    }
}
