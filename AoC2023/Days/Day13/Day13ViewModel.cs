using Advent2023.Utils;
using AoC2023Lib.Days.Day13Lib;
using CommonWPF;

namespace AoC2023.Days.Day13;

public class Day13ViewModel : ViewModelBase
{
    public MirrorMaster Master { get; } = new();

    public int MirrorSum { get; set; }

    public Day13ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day13", "input.txt");

        Master.Parse(fileData);


        MirrorSum = Master.GetOverallMirrorSum();
    }
}
