using Advent2023.Utils;
using AoC2023Lib.Days.Day19Lib;

namespace AoC2023.Days.Day19;

public class Day19ViewModel
{
    public MachinePartSorter Sorter { get;} = new();

    public int RatingSum { get; set;}
    public ulong AllCombinations { get; set;}
    public Day19ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day19", "input.txt");
        Sorter.Parse(fileData);

        RatingSum = Sorter.GetRatingSum();
        AllCombinations = Sorter.GetAllCombinations();
    }
}
