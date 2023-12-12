using Common;

namespace AoC2023Lib.Days.Day12Lib;

public class SpringHandler
{
    public List<SpringLine> SpringLines { get; set; } = new();


    public void Parse(Filedata fileData)
    {
        // ..???#??.?????? 4,3
        foreach (var line in fileData.Lines) 
        {
            SpringLines.Add(new SpringLine(line));
        }
    }

    public int GetArrangementSum()
    {
        var sum = 0;
        foreach (var row in SpringLines) 
        {
            var arrangementCount = row.GetArrangementCount();
            sum += arrangementCount;
        }
        return sum;
    }

}
