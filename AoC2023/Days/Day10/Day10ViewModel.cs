using Advent2023.Utils;
using AoC2023Lib.Days.Day10Lib;
using CommonWPF;

namespace AoC2023.Days.Day10;

public class Day10ViewModel : ViewModelBase
{
    public Pipinator Pipee { get; set; } = new();

    public int LongestDistanceFromStart
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public Day10ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day10", "input.txt");

        Pipee.Parse(fileData);

        LongestDistanceFromStart = Pipee.GetLongestDistanceFromStart();



    }
}
