using Advent2023.Utils;
using AoC2023Lib.Days.Day20Lib;

namespace AoC2023.Days.Day20;

public class Day20ViewModel
{
    public EngineDepartment Engineer { get; } = new();

    public long PulseProduct {  get; set; }
    public long CyclesUntilSwitchedOn {  get; set; }

    public Day20ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day20", "input.txt");
        Engineer.Parse(fileData);

        //PulseProduct = Engineer.GetPulseProduct();
        CyclesUntilSwitchedOn = Engineer.GetCyclesUntilSwitchedOn();
    }

}
