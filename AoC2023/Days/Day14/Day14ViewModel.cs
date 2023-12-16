using Advent2023.Utils;
using AoC2023Lib.Days.Day07Lib;
using AoC2023Lib.Days.Day14Lib;

namespace AoC2023.Days.Day14;

public class Day14ViewModel
{
    public PlatformControl Control { get; } = new();

    public int Load {  get; set; }
    public int LoadAfter1BCycles {  get; set; }

    
    public Day14ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day14", "input.txt");
        Control.Parse(fileData);

        Load = Control.GetLoadAfterNorthTilt();
        LoadAfter1BCycles = Control.GetLoadAfter1BCycles();
    }
}
