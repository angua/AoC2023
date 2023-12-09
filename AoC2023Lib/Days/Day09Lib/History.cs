namespace AoC2023Lib.Days.Day09Lib;

public class History
{
    private string _line;

    private List<int> _values;

    private List<List<int>> _sequences = new();

    public History(string line)
    {
        _line = line;
        // 13 33 54 85 159 343 748 1539 2945 5269 8898 14313 22099 32955 47704 67303 92853 125609 166990 218589 282183
        var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        _values = parts.Select(p => int.Parse(p)).ToList();
    }

    internal void CreateDifferenceSequences()
    {
        var sequence = _values;
        _sequences.Add(sequence);

        while (sequence.Any(v => v != 0))
        {
            var result = new List<int>();
            for (int i = 1; i < sequence.Count; i++)
            {
                var diff = sequence[i] - sequence[i - 1];
                result.Add(diff);
            }
            _sequences.Add(result);
            sequence = result;
        }
    }

    

    internal int FindExtrapolatedValue()
    {
        var diff = 0;

        for (int i = _sequences.Count - 2; i >= 0; i--)
        {
            var sequence = _sequences[i];
            var last = sequence.Last();

            diff = last + diff;
        }

        return diff;

    }

    internal int FindExtrapolatedPreviousValue()
    {
        var diff = 0;

        for (int i = _sequences.Count - 2; i >= 0; i--)
        {
            var sequence = _sequences[i];
            var first = sequence.First();

            diff = first - diff;
        }

        return diff;
    }
}
