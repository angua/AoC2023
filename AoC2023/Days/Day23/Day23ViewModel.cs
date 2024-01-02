using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Documents;
using Advent2023.Utils;
using AoC2023Lib.Days.Day23Lib;
using CommonWPF;
using Microsoft.VisualBasic;

namespace AoC2023.Days.Day23;

public class Day23ViewModel : ViewModelBase
{
    private bool _calculationRunning = false;
    public PathFinding PathFinder { get; set; } = new();

    private List<AoC2023Lib.Days.Day23Lib.Tile> _longestPath = new();
    public long LongestPathStepsCount
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    private List<AoC2023Lib.Days.Day23Lib.Tile> _longestPathWithoutSlopes = new();
    public long LongestPathWithoutSlopesStepsCount
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public ObservableCollection<CommonWPF.Tile> VisualGrid { get; set; } = new();
    public ObservableCollection<CommonWPF.TileLine> VisualPathLines { get; set; } = new();

    public int Width => PathFinder.Width;
    public int Height => PathFinder.Height;

    public Day23ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day23", "input.txt");
        PathFinder.Parse(fileData);

        Task.Run(ParseVisuals);

        FindLongestPath = new RelayCommand(CanFindLongestPath, DoFindLongestPath);
        FindLongestPathWithoutSlopes = new RelayCommand(CanFindLongestPathWithoutSlopes, DoFindLongestPathWithoutSlopes);


        //LongestPath = PathFinder.FindLongestPath();
        // LongestPathWithoutSlopes = PathFinder.FindLongestPathWithoutSlopes();
    }

    public RelayCommand FindLongestPath { get; }
    public bool CanFindLongestPath()
    {
        return !_calculationRunning;
    }
    public void DoFindLongestPath()
    {
        StartFindingLongestPath();
    }

    private async Task StartFindingLongestPath()
    {
        _calculationRunning = true;
        _longestPath = PathFinder.FindLongestPath();
        var visualLines = ParseLines(_longestPath);

        App.Current.Dispatcher.Invoke(() =>
        {
            VisualPathLines = visualLines;
            LongestPathStepsCount = _longestPath.Count - 1;
            RaisePropertyChanged(nameof(VisualPathLines));
        });
        _calculationRunning = false;
    }

    private ObservableCollection<TileLine> ParseLines(List<AoC2023Lib.Days.Day23Lib.Tile> path)
    {
        var visualLines = new ObservableCollection<CommonWPF.TileLine>();
        for (int i = 0; i < path.Count - 1; i++)
        {
            var first = path[i];
            var second = path[i + 1];
            visualLines.Add(new TileLine()
            {
                StartX = (int)first.Position.X,
                StartY = (int)first.Position.Y,
                EndX = (int)second.Position.X,
                EndY = (int)second.Position.Y,
            });
        }

        return visualLines;
    }

    public RelayCommand FindLongestPathWithoutSlopes { get; }
    public bool CanFindLongestPathWithoutSlopes()
    {
        return !_calculationRunning;
    }
    public void DoFindLongestPathWithoutSlopes()
    {
        StartFindingLongestPathWithoutSlopes();

    }
    private async Task StartFindingLongestPathWithoutSlopes()
    {
        _calculationRunning = true;

        _longestPathWithoutSlopes = PathFinder.FindLongestPathWithoutSlopes();
        var visualLines = ParseLines(_longestPathWithoutSlopes);

        App.Current.Dispatcher.Invoke(() =>
        {
            VisualPathLines = visualLines;
            LongestPathStepsCount = _longestPathWithoutSlopes.Count - 1;
            RaisePropertyChanged(nameof(VisualPathLines));
        });
        _calculationRunning = false;

    }


    private void ParseVisuals()
    {
        var visuals = new ObservableCollection<CommonWPF.Tile>();
        foreach (var pos in PathFinder.Grid)
        {
            visuals.Add(new VisualTile()
            {
                PositionX = (int)pos.Value.Position.X,
                PositionY = (int)pos.Value.Position.Y,
                Ground = pos.Value.Ground,
            });
        }
        App.Current.Dispatcher.Invoke(() =>
        {
            VisualGrid = visuals;
            RaisePropertyChanged(nameof(VisualGrid));
        });
    }


}
