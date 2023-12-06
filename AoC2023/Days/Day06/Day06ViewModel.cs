using Advent2023.Utils;
using AoC2023Lib.Days.Day06Lib;
using CommonWPF;

namespace AoC2023.Days.Day06;

public class Day06ViewModel : ViewModelBase
{
    public BoatRacer Racer { get; set; } = new();

    public int WinningRangeProduct
    {
        get => GetValue<int>();
        set => SetValue(value);
    }
    public long BigWinningRange
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    

    public Day06ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day06", "input.txt");

        Racer.Parse(fileData);
        WinningRangeProduct = Racer.GetWinningRangeProduct();

        BigWinningRange = Racer.GetBigWinningRange();
    }
}
