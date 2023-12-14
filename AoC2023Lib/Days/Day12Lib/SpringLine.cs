using Common;

namespace AoC2023Lib.Days.Day12Lib;

public class SpringLine
{
    public string Line {  get; private set; }

    public Dictionary<int, SpringCondition> Row { get; set; } = new();
    public Dictionary<int, SpringCondition> UnfoldedRow { get; set; }

    public List<int> Groups { get; set; } = new();
    public List<int> UnfoldedGroups { get; set; } = new();

    public long ArrangementCount { get; private set; }
    public long UnfoldedArrangementCount { get; private set; }


    internal SpringHelper Helper { get; set; }


    public SpringLine(string line)
    {
        Line = line;

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

    internal long GetArrangementCount()
    {
        var finder = new PossibilitiesFinder(Row, Groups);
        ArrangementCount = finder.GetArrangementCount();
        return ArrangementCount;
    }


    internal long GetUnfoldedArrangementCount()
    {
        var finder = new PossibilitiesFinder(UnfoldedRow, UnfoldedGroups);
        return finder.GetArrangementCount();
    }


    internal int GetArrangementCount(Dictionary<int, SpringCondition> row, List<int> validGroup)
    {
        // ..???#??.?????? 4,3

        var arrangementCount = 0;

        // there have to be this number of damaged springs in this row
        var damagedSum = validGroup.Sum();

        // marked damaged #
        var damagedPositions = row.Where(s => s.Value == SpringCondition.Damaged).Select(p => p.Key).ToList();
        var damagedCount = damagedPositions.Count();

        // count of springs that need to be marked damaged
        var additionalDamagedCount = damagedSum - damagedCount;

        var unknownPositions = row.Where(s => s.Value == SpringCondition.Unknown).Select(p => p.Key).ToList();
        var unknownCount = unknownPositions.Count();

        // find all possible ways to arrange damaged on unknown
        var variations = FindVariations(unknownCount, additionalDamagedCount);

        foreach (var variation in variations)
        {
            var possibledamagepositions = variation.Select(v => unknownPositions[v]).ToList();

            var damageSet = new List<int>(damagedPositions);
            damageSet.AddRange(possibledamagepositions);

            if (IsValid(damageSet, validGroup))
            {
                arrangementCount++;
            }
        }

        return arrangementCount;
    }

    private List<List<int>> FindVariations(int unknownCount, int additionalDamagedCount)
    {
        if (!Helper.Variations.TryGetValue((unknownCount, additionalDamagedCount), out var variations))
        {
            variations = MathUtils.GetAllCombinations(unknownCount, additionalDamagedCount);
            Helper.Variations.Add((unknownCount, additionalDamagedCount), variations);
        }
        return variations;
    }

    private bool IsValid(List<int> damageSet, List<int> validGroups)
    {
        damageSet.Sort();

        var groups = new Dictionary<int, int>();

        var previous = damageSet.First();
        var group = 0;
        var groupCount = 1;

        for (int i = 1; i < damageSet.Count; i++)
        {
            var current = damageSet[i];
            if (current - previous == 1)
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

        if (groups.Count != validGroups.Count)
        {
            return false;
        }

        for (int i = 0; i < groups.Count; i++)
        {
            if (groups[i] != validGroups[i])
            {
                return false;
            }
        }
        return true;
    }

    internal void Unfold()
    {
        UnfoldedRow = new Dictionary<int, SpringCondition>(Row);
        UnfoldedGroups = new List<int>(Groups);

        // add 4 times
        for (int i = 0; i < 4; i++)
        {
            // start with unknown as spacer
            var spacerPos = UnfoldedRow.Count;
            UnfoldedRow[spacerPos] = SpringCondition.Unknown;

            var offset = spacerPos + 1;

            // copy row dictionary
            for (int current = 0; current < Row.Count; current++)
            {
                UnfoldedRow[current + offset] = Row[current];
            }

            // add the same groups
            UnfoldedGroups.AddRange(Groups);
        }
    }



    public long PlaceNextGroup(Dictionary<int, SpringCondition> row, int startPos, List<int> groups, int currentGroupIndex, List<int> damagedPositions)
    {
        if (startPos >= row.Count)
        {
            // end of row, still groups left, not valid
            return 0;
        }

        long arrangementCount = 0;

        // remove leading operational
        while (row[startPos] == SpringCondition.Operational)
        {
            startPos++;
            if (startPos >= row.Count)
            {
                // end of row, still groups left, not valid
                return 0;
            }
        }

        var currentGroup = groups[currentGroupIndex];

        // first possible start position for placing group: startPos
        // last possible start position for placing next group is the next marked damaged in row
        // or otherwise the end of the string
        var lastPossiblePosition = damagedPositions.FirstOrDefault(p => p >= startPos, -1);
        var lastPos = lastPossiblePosition == -1 ? row.Count - 1 : lastPossiblePosition;

        for (int currentPos = startPos; currentPos <= lastPos; currentPos++)
        {
            var valid = true;

            // check if group fits in here
            for (int groupPos = 0; groupPos < currentGroup; groupPos++)
            {
                var newPos = currentPos + groupPos;

                if (newPos >= row.Count)
                {
                    // outside row, not valid
                    valid = false;
                    break;
                }

                if (row[newPos] == SpringCondition.Operational)
                {
                    // no operational in group allowed
                    valid = false;
                }
            }

            var posAfterGroup = currentPos + currentGroup;

            if (posAfterGroup < row.Count && row[posAfterGroup] == SpringCondition.Damaged)
            {
                // no damaged right after group
                valid = false;
            }

            var markedDamagedAfterGroup = damagedPositions.Where(p => p > posAfterGroup);
            var damagedAfterGroup = groups.Skip(currentGroupIndex + 1).Sum();

            if (markedDamagedAfterGroup.Count() > damagedAfterGroup)
            {
                // more marked damaged in row than in groups left, not valid
                valid = false;
            }

            if (valid)
            {
                // this is a valid placement
                if (currentGroupIndex == groups.Count - 1)
                {
                    // last group, add this possibility
                    arrangementCount++;
                }
                else
                {
                    // add arrangements for next group
                    arrangementCount += PlaceNextGroup(row, posAfterGroup + 1, groups, ++currentGroupIndex, damagedPositions);
                }
            }
        }

        return arrangementCount;
    }





}
