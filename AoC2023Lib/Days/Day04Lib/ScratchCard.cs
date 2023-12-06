namespace AoC2023Lib.Days.Day04Lib;

public class ScratchCard
{
    private string _line;

    public int Id { get; }

    public List<int> WinningNumbers { get; } = new();
    public List<int> MyNumbers { get; } = new();

    public List<int> MyWinningNumbers { get; private set; } = new();

    public int Score { get; private set; }

    public ScratchCard(string line)
    {
        _line = line;

        //Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

        //Card 1
        var cardIdParts = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Id = int.Parse(cardIdParts[1]);

        // 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        var numbersParts = parts[1].Split("|", StringSplitOptions.RemoveEmptyEntries);

        var winningNumberStrings = numbersParts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        WinningNumbers = winningNumberStrings.Select(x => int.Parse(x)).ToList();

        var myNumberStrings = numbersParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        MyNumbers = myNumberStrings.Select(x => int.Parse(x)).ToList();

        FindMyWinningNumbers();
        CalculateWinningScore();
    }

    private void CalculateWinningScore()
    {
        var count = MyWinningNumbers.Count;

        if (count > 0) 
        {
            Score = 1;
            count--;
        }

        while (count > 0) 
        {
            Score *= 2;
            count--;
        }
    }

    private void FindMyWinningNumbers()
    {
        MyWinningNumbers = WinningNumbers.Intersect(MyNumbers).ToList();
    }
}