using System.Collections.Generic;
using System.Linq;
using Advent2023.Utils;
using AoC2023Lib.Days.Day02Lib;
using CommonWPF;

namespace AoC2023.Days.Day02;

public class Day02ViewModel : ViewModelBase
{
    public CubeGameHandler Handler { get; set; } = new();

    public Dictionary<Color, int> TotalSet { get; set; } = new();

    public int PossibleIdSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public int PowerSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public Day02ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day02", "input.txt");
        Handler.Parse(fileData);

        // 12 red cubes, 13 green cubes, and 14 blue cubes
        TotalSet.Add(Color.red, 12);
        TotalSet.Add(Color.green, 13);
        TotalSet.Add(Color.blue, 14);

        var possibleDraws = Handler.GetPossibleDraws(TotalSet);

        PossibleIdSum = possibleDraws.Sum(d => d.Id);

        var minimumSets = Handler.CubeGames.Select(g => g.GetMinimumSet());

        var powers = minimumSets.Select(s => s[Color.red] * s[Color.green] * s[Color.blue]);

        PowerSum = powers.Sum();


    }
}
