using Common;

namespace AoC2023Lib.Days.Day09Lib;

public class OasisHandler
{
    public List<History> Histories { get; set; } = new();

    

    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines) 
        {
            Histories.Add(new History(line));
        }

        foreach (var history in Histories)
        {
            history.CreateDifferenceSequences();
        }
    }

    public long GetExtrapolatedSum()
    {
        var sum = 0;
        foreach (var history in Histories)
        {
            sum += history.FindExtrapolatedValue();
        }
        return sum;
    }

    public long GetExtrapolatedPreviousSum()
    {
        var sum = 0;
        foreach (var history in Histories)
        {
            sum += history.FindExtrapolatedPreviousValue();
        }
        return sum;
    }
}
