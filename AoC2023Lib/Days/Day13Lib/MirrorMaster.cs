using Common;

namespace AoC2023Lib.Days.Day13Lib;

public class MirrorMaster
{
    public List<Pattern> Patterns { get; set; } = new();

    public void Parse(Filedata fileData)
    {
        var pattern = new Pattern();
        pattern.Id = Patterns.Count;
        Patterns.Add(pattern);

        for (int i = 0; i < fileData.Lines.Count; i++)
        {
            var line = fileData.Lines[i];

            if (string.IsNullOrEmpty(line))
            {
                pattern = new Pattern();
                pattern.Id = Patterns.Count;
                Patterns.Add(pattern);
            }
            else
            {
                pattern.AddLine(line);
            }
        }

        foreach (var currentPattern in Patterns)
        {
            currentPattern.CreateRowsAndColumns();
        }

    }

    public int GetOverallMirrorSum()
    {
        return Patterns.Select(p => p.GetMirrorSum()).Sum();
    }

    public int GetOverallSmudgedMirrorSum()
    {
        return Patterns.Select(p => p.GetSmudgedMirrorSum()).Sum();
    }
}
