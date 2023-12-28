namespace AoC2023Lib.Days.Day12Lib;

internal class PossibilitiesFinder
{
    private Dictionary<int, SpringCondition> _row;
    private List<int> _groups;

    private List<int> _damagedPositions;

    private Dictionary<(int, int), long> _possibilities = new();

    public PossibilitiesFinder(Dictionary<int, SpringCondition> row, List<int> groups)
    {
        _row = row;
        _groups = groups;
    }

    internal long GetArrangementCount()
    {
        _damagedPositions = _row.Where(s => s.Value == SpringCondition.Damaged).Select(p => p.Key).ToList();

        var position = 0;
        var currentGroupIndex = 0;

        // possibilites to arrange groups starting with index at or after this position
        return GetPossibilities(position, currentGroupIndex);
    }

    private long GetPossibilities(int startPosition, int currentGroupIndex)
    {
        if (_possibilities.TryGetValue((startPosition, currentGroupIndex), out var possibleArrangements))
        {
            return possibleArrangements;
        }

        var groupLength = _groups[currentGroupIndex];

        // last possible start position for placing next group is the next marked damaged in row
        // or otherwise the end of the string
        var lastPossiblePosition = _damagedPositions.FirstOrDefault(p => p >= startPosition, -1);
        var lastPos = lastPossiblePosition == -1 ? _row.Count - 1 : lastPossiblePosition;

        var possibilitiesAtPosition = new Dictionary<int, long>();

        for (int pos = startPosition; pos <= lastPos; pos++)
        {
            // check if we can place group here
            var valid = true;

            for (int groupPos = 0; groupPos < groupLength; groupPos++)
            {
                var newPos = pos + groupPos;
                if (newPos >= _row.Count)
                {
                    // outside row, not valid
                    valid = false;
                    break;
                }

                if (_row[newPos] == SpringCondition.Operational)
                {
                    // no operational in group allowed
                    valid = false;
                    break;
                }
            }

            var posAfterGroup = pos + groupLength;
            if (posAfterGroup < _row.Count && _row[posAfterGroup] == SpringCondition.Damaged)
            {
                // no damaged right after group
                valid = false;
            }

            var markedDamagedAfterGroup = _damagedPositions.Where(p => p > posAfterGroup);
            var damagedAfterGroup = _groups.Skip(currentGroupIndex + 1).Sum();

            if (markedDamagedAfterGroup.Count() > damagedAfterGroup)
            {
                // more marked damaged in row than in groups left, not valid
                valid = false;
            }

            if (!valid)
            {
                // group can not be placed here
                // no new possibilies
                possibilitiesAtPosition[pos] = 0;
            }
            else
            {
                if (currentGroupIndex == _groups.Count - 1)
                {
                    // last group, one possibility here
                    possibilitiesAtPosition[pos] = 1;
                }
                else
                {
                    // place group here, possibilities for placing following groups after this
                    possibilitiesAtPosition[pos] = GetPossibilities(posAfterGroup + 1, currentGroupIndex + 1);
                }

            }
        }

        // sum of current position and allowed positions after this 
        // sum up possibilities starting with last and add to dict
        if (possibilitiesAtPosition.Count > 0)
        {

            var currentPos = possibilitiesAtPosition.Max(p => p.Key);
            long sum = 0;
            while (possibilitiesAtPosition.ContainsKey(currentPos))
            {
                sum += possibilitiesAtPosition[currentPos];
                _possibilities[(currentPos, currentGroupIndex)] = sum;
                currentPos--;
            }
        }
        else
        {
            _possibilities[(startPosition, currentGroupIndex)] = 0;
        }
        return _possibilities[(startPosition, currentGroupIndex)];
    }

}
