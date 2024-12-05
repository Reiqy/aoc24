namespace AoC24;

public class Problem5
{
    public int SolveA()
    {
        var lines = File.ReadAllLines("input/aoc24_5.txt");

        var rules = new HashSet<Rule>();

        int i = 0;
        for (i = 0; !string.IsNullOrEmpty(lines[i]); i++)
        {
            var lineSplit = lines[i].Split('|');
            var before = int.Parse(lineSplit[0]);
            var after = int.Parse(lineSplit[1]);
            rules.Add(new Rule(before, after));
        }

        i++;

        var sum = 0;
        for (; i < lines.Length; i++)
        {
            var update = lines[i]
                .Split(',')
                .Select(x => int.Parse(x))
                .ToArray();

            bool isCorrect = this.IsUpdateCorrect(rules, update);
            if (isCorrect)
            {
                var middleIndex = update.Length / 2;
                var middlePage = update[middleIndex];
                sum += middlePage;
            }
        }

        return sum;
    }

    private bool IsUpdateCorrect(HashSet<Rule> rules, int[] update)
    {
        for (int pageIndexBefore = 0; pageIndexBefore < update.Length; pageIndexBefore++)
        {
            var before = update[pageIndexBefore];
            for (int pageIndexAfter = pageIndexBefore; pageIndexAfter < update.Length; pageIndexAfter++)
            {
                var after = update[pageIndexAfter];
                if (rules.Contains(new Rule(after, before)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private readonly record struct Rule(int Before, int After);

    public int SolveB()
    {
        var lines = File.ReadAllLines("input/aoc24_5.txt");

        var rules = new HashSet<Rule>();

        int i = 0;
        for (i = 0; !string.IsNullOrEmpty(lines[i]); i++)
        {
            var lineSplit = lines[i].Split('|');
            var before = int.Parse(lineSplit[0]);
            var after = int.Parse(lineSplit[1]);
            rules.Add(new Rule(before, after));
        }

        i++;

        var comparer = new ElfPageComparer(rules);

        var sum = 0;
        for (; i < lines.Length; i++)
        {
            var update = lines[i]
                .Split(',')
                .Select(x => int.Parse(x))
                .ToArray();

            bool isCorrect = this.IsUpdateCorrect(rules, update);
            if (!isCorrect)
            {
                update = update
                    .OrderBy(x => x, comparer)
                    .ToArray();

                var middleIndex = update.Length / 2;
                var middlePage = update[middleIndex];
                sum += middlePage;
            }
        }

        return sum;
    }

    private sealed class ElfPageComparer : IComparer<int>
    {
        private readonly HashSet<Rule> rules;

        public ElfPageComparer(HashSet<Rule> rules)
        {
            this.rules = rules;
        }

        public int Compare(int x, int y)
        {
            if (this.rules.Contains(new Rule(x, y)))
            {
                return -1;
            }

            if (this.rules.Contains(new Rule(y, x)))
            {
                return 1;
            }

            return 0;
        }
    }
}
