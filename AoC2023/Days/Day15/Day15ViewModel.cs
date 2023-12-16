using Advent2023.Utils;
using AoC2023Lib.Days.Day15Lib;

namespace AoC2023.Days.Day15;

public class Day15ViewModel
{
    public HashControl Control { get; } = new();

    public int HashSum { get; set; }
    public int FocusPower { get; set; }
    public Day15ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day15", "input.txt");
        Control.Parse(fileData);

        HashSum = Control.GetHashSum();
        FocusPower = Control.GetFocusPower();
    }
}
