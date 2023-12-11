using System.Collections.Generic;
using System.Collections.ObjectModel;
using AoC2023Lib.Days.Day02Lib;
using CommonWPF;

namespace AoC2023.Days.Day02;

public class VisualCubeGame : ViewModelBase
{
    private CubeGame _game;

    public int Id => _game.Id;

    public ObservableCollection<VisualSet> Draws { get; set; } = new();

    public VisualSet MinimumSet { get; private set; }

    public bool IsValid
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public VisualCubeGame(CubeGame game)
    {
        _game = game;

        foreach (var draw in _game.Draws)
        {
            Draws.Add(GetVisualSet(draw));
        }
        MinimumSet = GetVisualSet(_game.MinimumSet);
    }

    private VisualSet GetVisualSet(Dictionary<CubeColor, int> set)
    {
        var visualSet = new VisualSet();
        visualSet.Red = set[CubeColor.red];
        visualSet.Green = set[CubeColor.green];
        visualSet.Blue = set[CubeColor.blue];
        return visualSet;
    }
}
