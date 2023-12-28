using Common;

namespace AoC2023Lib.Days.Day12Lib;

public class SpringLine
{
    public string Line {  get; private set; }

    public Dictionary<int, SpringCondition> Row { get; set; } = new();
    public Dictionary<int, SpringCondition> UnfoldedRow { get; set; } = new();

    public List<int> Groups { get; set; } = new();
    public List<int> UnfoldedGroups { get; set; } = new();

    public long ArrangementCount { get; private set; }
    public long UnfoldedArrangementCount { get; private set; }

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

        Unfold();
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
        UnfoldedArrangementCount = finder.GetArrangementCount();
        return UnfoldedArrangementCount;
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

    private SpringCondition GetCondition(char current)
    {
        return current switch
        {
            '?' => SpringCondition.Unknown,
            '.' => SpringCondition.Operational,
            '#' => SpringCondition.Damaged,
            _ => throw new InvalidOperationException()
        };
    }

}
