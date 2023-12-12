using Common;

namespace AoC2023Lib.Days.Day12Lib;

public class SpringLine
{
    private string _line;

    public Dictionary<int, SpringCondition> Row { get; set; } = new();
    public List<int> Groups { get; set; } = new();


    public SpringLine(string line)
    {
        _line = line;

        // ..???#??.?????? 4,3
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);


        for (int i = 0; i < parts[0].Length; i++)
        {
            var current = parts[0][i];
            Row[i] = GetCondition(current);
        }

        var groups = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var group in groups)
        {
            Groups.Add(int.Parse(group));
        }
    }

    private SpringCondition GetCondition(char current)
    {
        return current switch
        {
            '?' => SpringCondition.Unknown,
            '.' => SpringCondition.Operational,
            '#' => SpringCondition.Damaged
        };
    }

    internal int GetArrangementCount()
    {
        // ..???#??.?????? 4,3

        var allowedArrangements = new List<List<int>>();

        // there have to be this number of damaged springs in this row
        var damagedSum = Groups.Sum();

        // marked damaged #
        var damagedPositions = Row.Where(s => s.Value == SpringCondition.Damaged).Select(p => p.Key).ToList();
        var damagedCount = damagedPositions.Count();

        // count of springs that need to be marked damaged
        var additionalDamagedCount = damagedSum - damagedCount;

        var unknownPositions = Row.Where(s => s.Value == SpringCondition.Unknown).Select(p => p.Key).ToList();
        var unknownCount = unknownPositions.Count();



        var variations = MathUtils.GetAllCombinations(unknownCount, additionalDamagedCount);

        foreach (var variation in variations)
        {
            var possibledamagepositions = variation.Select(v => unknownPositions[v]).ToList();

            var damageSet = new List<int>(damagedPositions);
            damageSet.AddRange(possibledamagepositions);

            if (IsValid(damageSet))
            {
                allowedArrangements.Add(damageSet);
            }


        }

        return allowedArrangements.Count;

        


    }

    private bool IsValid(List<int> damageSet)
    {
        damageSet.Sort();

        var groups = new Dictionary<int, int>();

        var previous = damageSet.First();
        var group = 0;
        var groupCount = 1;

        for (int i = 1; i < damageSet.Count; i++)
        {
            var current = damageSet[i];
            if (current - previous  == 1)
            {
                groupCount++;
            }
            else
            {
                groups.Add(group, groupCount);
                group++;
                groupCount = 1;
            }
            previous = current;
        }
        // add last group
        groups.Add(group, groupCount);

        if (groups.Count != Groups.Count)
        {
            return false;
        }

        for (int i = 0; i < groups.Count; i++)
        {
            if (groups[i] != Groups[i])
            {
                return false;
            }
        }
        return true;
    }
}
