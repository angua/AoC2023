using Common;

namespace AoC2023Lib.Days.Day15Lib;

public class HashControl
{
    public List<string> Sequences { get; set; } = new();
    public List<int> HashSequences { get; set; } = new();

    public Dictionary<int, Box> Boxes { get; set; } = new();

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


    public int GetFocusPower()
    {
        // create boxes
        for (int i = 0; i < 256; i++)
        {
            Boxes.Add(i, new Box()
            {
                Number = i
            });
        }

        // go through sequences
        foreach (var sequence in Sequences)
        {
            // pc=4
            if (sequence.Contains("="))
            {
                var parts = sequence.Split('=', StringSplitOptions.RemoveEmptyEntries);
                var label = parts[0];
                var correctBoxNumber = CreateHashSequence(label);
                var focalLength = int.Parse(parts[1]);

                // add lens or replace lens with same label
                var box = Boxes[correctBoxNumber];
                var lens = box.Lenses.FirstOrDefault(l => l.Label == label);
                if (lens != null)
                {
                    lens.FocalLength = focalLength;
                }
                else
                {
                    box.Lenses.Add(new Lens()
                    { 
                        Label = label,
                        FocalLength = focalLength 
                    });
                }
            }

            // ztj-
            else
            {
                var parts = sequence.Split('-', StringSplitOptions.RemoveEmptyEntries);
                var label = parts[0];
                var correctBoxNumber = CreateHashSequence(label);

                // remove lens with label from box
                var box = Boxes[correctBoxNumber];
                var lens = box.Lenses.FirstOrDefault(l => l.Label == label);
                if (lens != null)
                {
                    box.Lenses.Remove(lens);
                }
            }
        }

        return Boxes.Select(b => b.Value.GetFocusingPower()).Sum();


    }
}
