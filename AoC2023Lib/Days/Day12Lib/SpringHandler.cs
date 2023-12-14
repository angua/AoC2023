using Common;

namespace AoC2023Lib.Days.Day12Lib;

public class SpringHandler
{
    public List<SpringLine> SpringLines { get; set; } = new();

    internal SpringHelper Helper { get; set; } = new();

    

    public void Parse(Filedata fileData)
    {
        // ..???#??.?????? 4,3
        foreach (var line in fileData.Lines) 
        {
            SpringLines.Add(new SpringLine(line)
            {
                Helper = Helper
            });
        }
    }

    public long GetArrangementSum()
    {
        long sum = 0;
        foreach (var line in SpringLines) 
        {
            var arrangementCount = line.GetArrangementCount();
            sum += arrangementCount;
        }
        return sum;
    }

    public long GetUnfoldedArrangementSum()
    {
        foreach (var row in SpringLines)
        {
            row.Unfold();
        }

        long sum = 0;
        foreach (var line in SpringLines)
        {
            var arrangementCount = line.GetUnfoldedArrangementCount();
            sum += arrangementCount;
        }
        return sum;

    }
}
