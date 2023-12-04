using Common;

namespace AoC2023Lib.Days.Day04Lib;

public class ScratchCardHandler
{
    public List<ScratchCard> ScratchCards { get; set; } = new();

    // Id, count
    public Dictionary<int, int> ScratchCardCounts { get; set; } = new();

    public int ScratchCardSum { get; set; }

    public int ScoreSum { get; set; }

    public long EvaluateMoreScratchCards()
    {
        var maxId = ScratchCards.Max(c => c.Id);
        for (int i = 1; i <= maxId; i++)
        {
            ScratchCardCounts[i] = 1;
        }

        for (int i = 1; i <= maxId; i++)
        {
            var currentCard = ScratchCards.First(c => c.Id == i);

            var winningCount = currentCard.MyWinningNumbers.Count;

            for (int s = 1; s <= winningCount; s++)
            {
                if (ScratchCardCounts.ContainsKey(i + s))
                {
                    ScratchCardCounts[i + s] += ScratchCardCounts[i];
                }
            }
        }
        return ScratchCardCounts.Sum(s => s.Value);
    }

    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            ScratchCards.Add(new ScratchCard(line));
        }
        ScoreSum = ScratchCards.Sum(c => c.Score);
    }


}
