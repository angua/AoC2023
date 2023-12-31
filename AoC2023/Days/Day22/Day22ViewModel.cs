using Advent2023.Utils;
using AoC2023Lib.Days.Day22Lib;

namespace AoC2023.Days.Day22;

public class Day22ViewModel
{
    public Brickinator Brickinator { get; set; } = new();

    public long DisintegrateSum { get; set; }
    public long FallingSum { get; set; }

    public Day22ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day22", "input.txt");
        Brickinator.Parse(fileData);

        DisintegrateSum = Brickinator.GetDisintegrateSum();
        FallingSum = Brickinator.GetFallingSum();
        
    }
}
