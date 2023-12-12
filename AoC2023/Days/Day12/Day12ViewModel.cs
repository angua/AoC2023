using Advent2023.Utils;
using AoC2023Lib.Days.Day12Lib;
using CommonWPF;

namespace AoC2023.Days.Day12;

public class Day12ViewModel : ViewModelBase
{
    public SpringHandler Springinator { get; } = new();

    public int ArrangementSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

   

    public Day12ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day12", "input.txt");
        Springinator.Parse(fileData);

        ArrangementSum = Springinator.GetArrangementSum();
    }
}
