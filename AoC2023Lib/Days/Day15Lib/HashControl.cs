using Common;

namespace AoC2023Lib.Days.Day15Lib;

public class HashControl
{
    public List<string> Sequences { get; set; } = new();

    public List<int> HashSequences { get; set; } = new();
    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                Sequences.Add(part);
            }
        }
    }

    public int GetHashSum()
    {
        CreateHashSequences();
        return HashSequences.Sum();
    }

    public void CreateHashSequences()
    {
        foreach (var sequence in Sequences)
        {
            var hash = CreateHashSequence(sequence);
            HashSequences.Add(hash);
        }
    }

    private int CreateHashSequence(string sequence)
    {
        var value = 0;
        foreach (var character in sequence)
        {
            var ascii = (int)character;
            value += ascii;
            value *= 17;
            value = value % 256;
        }

        return value;
    }
}
