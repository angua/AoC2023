using Advent2023.Utils;
using AoC2023Lib.Days.Day04Lib;
using CommonWPF;

namespace AoC2023.Days.Day04;

public class Day04ViewModel : ViewModelBase
{
    public ScratchCardHandler CardHandler { get; set; } = new();

    public int ScoreSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public long WinMoreScratchCardsCount
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public Day04ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day04", "input.txt");

        CardHandler.Parse(fileData);

        // part 1
        ScoreSum = CardHandler.ScoreSum;

        // part 2
        WinMoreScratchCardsCount = CardHandler.EvaluateMoreScratchCards();


    }
}
