using Advent2023.Utils;
using AoC2023Lib.Days.Day11Lib;
using Common;
using CommonWPF;

namespace AoC2023.Days.Day11;

public class Day11ViewModel : ViewModelBase
{
    public Astronomy Astronomer { get; } = new();

    public int DistanceSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public long WideExpandedDistanceSum
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public Day11ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day11", "input.txt");
        Astronomer.Parse(fileData);

        DistanceSum = Astronomer.GetDistanceSum();
        WideExpandedDistanceSum = Astronomer.GetWideExpandedDistanceSum();
    }
}
