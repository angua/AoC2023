using System.Collections.ObjectModel;
using System.Linq;
using Advent2023.Utils;
using AoC2023Lib.Days.Day02Lib;
using Common;
using CommonWPF;

namespace AoC2023.Days.Day02;

public class Day02ViewModel : ViewModelBase
{
    public CubeGameHandler Handler { get; set; } = new();

    private int[] _totalSet;
    public VisualSet TotalSet
    {
        get => GetValue<VisualSet>();
        set
        {
            SetValue(value);
            _totalSet = CreateSet(TotalSet);
        }
    }

    public ObservableCollection<VisualCubeGame> CubeGames { get; set; } = new();

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

        foreach (var game in Handler.CubeGames)
        {
            CubeGames.Add(new VisualCubeGame(game));
        }

        // 12 red cubes, 13 green cubes, and 14 blue cubes
        TotalSet = new VisualSet
        {
            Red = 12,
            Green = 14,
            Blue = 13
        };

        var possibleGames = Handler.GetPossibleGames(_totalSet);

        foreach (var game in possibleGames) 
        {
            var visualGame = CubeGames.First(g => g.Id == game.Id);
            visualGame.IsValid = true;
        }

        PossibleIdSum = possibleGames.Sum(d => d.Id);

        var minimumSets = Handler.CubeGames.Select(g => g.GetMinimumSet());

        var powers = minimumSets.Select(s => s[CubeColor.red] * s[CubeColor.green] * s[CubeColor.blue]);

        PowerSum = powers.Sum();


    }

    private int[] CreateSet(VisualSet totalSet)
    {
        var result = new int[3];
        result[0] = totalSet.Red;
        result[1] = totalSet.Blue;
        result[2] = totalSet.Green;

        return result;
    }
}
