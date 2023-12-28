using Advent2023.Utils;
using AoC2023Lib.Days.Day09Lib;
using Common;
using CommonWPF;

namespace AoC2023.Days.Day09;

public class Day09ViewModel : ViewModelBase
{
    public OasisHandler Oasis { get; set; } = new();

    public long ExtrapolatedSum
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public long ExtrapolatedPreviousSum
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public Day09ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day09", "input.txt");
        Oasis.Parse(fileData);

        ExtrapolatedSum = Oasis.GetExtrapolatedSum();
        ExtrapolatedPreviousSum = Oasis.GetExtrapolatedPreviousSum();
    }
}
