using Advent2023.Utils;
using AoC2023Lib.Days.Day03Lib;
using CommonWPF;

namespace AoC2023.Days.Day03;

public class Day03ViewModel : ViewModelBase
{
    public Engineering Engineer { get; set; } = new();

    public int EnginePartSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public Day03ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day03", "input.txt");

        Engineer.Parse(fileData);

        var enignePartNumbers = Engineer.FindPartNumbers();
        EnginePartSum = Engineer.GetEnginePartSum(enignePartNumbers);

    }

}
