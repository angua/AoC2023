using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using Advent2023.Utils;
using AoC2023Lib.Days.Day17Lib;
using CommonWPF;

namespace AoC2023.Days.Day17;

public class Day17ViewModel : ViewModelBase
{
    public HeatLossControl Control { get; } = new();

    public ObservableCollection<object> HeatLossMap { get; set; } = new();

    public ObservableCollection<CommonWPF.Tile> VisualGrid { get; set; } = new();
    public ObservableCollection<CommonWPF.TileLine> VisualPathLines { get; set; } = new();


    public int MapWidth => Control.CountX;
    public int MapHeight => Control.CountY;


    public Move BestMove
    {
        get => GetValue<Move>();
        set => SetValue(value);
    }
    public Move BestUltraMove
    {
        get => GetValue<Move>();
        set => SetValue(value);
    }

    public int LowestHeatLoss
    {
        get => GetValue<int>();
        set => SetValue(value);
    }
    public int LowestUltraHeatLoss
    {
        get => GetValue<int>();
        set => SetValue(value);
    }


    private Stopwatch _watch = new Stopwatch();
    public long Timer
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    private bool _calculating = false;

    public Day17ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day17", "input.txt");
        Control.Parse(fileData);

        Task.Run(ParseVisuals);

        GetLowestHeatLoss = new RelayCommand(CanGetLowestHeatLoss, DoGetLowestHeatLoss);
        GetLowestUltraHeatLoss = new RelayCommand(CanGetLowestUltraHeatLoss, DoGetLowestUltraHeatLoss);


    }

    public RelayCommand GetLowestHeatLoss { get; }
    public bool CanGetLowestHeatLoss()
    {
        return !_calculating;
    }
    public void DoGetLowestHeatLoss()
    {
       StartGetLowestHeatLoss();
    }

    public async Task StartGetLowestHeatLoss()
    {
        _watch.Reset();
        _watch.Start();
        _calculating = true;

        BestMove = await Task.Run(() => Control.GetLowestHeatLossMove());
        LowestHeatLoss = BestMove.Loss;

        var visualLines = ParseLines(BestMove.Route);

        App.Current.Dispatcher.Invoke(() =>
        {
            VisualPathLines = visualLines;
            RaisePropertyChanged(nameof(VisualPathLines));
        });

        _calculating = false;
        _watch.Stop();
        Timer = _watch.ElapsedMilliseconds;
    }


    public RelayCommand GetLowestUltraHeatLoss { get; }
    public bool CanGetLowestUltraHeatLoss()
    {
        return !_calculating;
    }
    public void DoGetLowestUltraHeatLoss()
    {
        StartGetLowestUltraHeatLoss();
    }
    public async Task StartGetLowestUltraHeatLoss()
    {
        _watch.Reset();
        _watch.Start();
        _calculating = true;
        
        BestUltraMove = await Task.Run(() => Control.GetLowestHeatLossMove(true));
        LowestUltraHeatLoss = BestUltraMove.Loss;

        var visualLines = ParseLines(BestUltraMove.Route);

        App.Current.Dispatcher.Invoke(() =>
        {
            VisualPathLines = visualLines;
            RaisePropertyChanged(nameof(VisualPathLines));
        });

        _calculating = false;
        _watch.Stop();
        Timer = _watch.ElapsedMilliseconds;
    }

    

    private void ParseVisuals()
    {
        var visuals = new ObservableCollection<Tile>();
        foreach (var pos in Control.Grid)
        {
            visuals.Add(new VisualTile()
            {
                PositionX = (int)pos.Key.X,
                PositionY = (int)pos.Key.Y,
                HeatLoss = pos.Value,
            });
        }
        App.Current.Dispatcher.Invoke(() =>
        {
            VisualGrid = visuals;
            RaisePropertyChanged(nameof(VisualGrid));
        });
    }

    private ObservableCollection<TileLine> ParseLines(List<Vector2> route)
    {
        
        var visualLines = new ObservableCollection<TileLine>();
        for (int i = 0; i < route.Count - 1; i++)
        {
            var first = route[i];
            var second = route[i + 1];
            visualLines.Add(new TileLine()
            {
                StartX = (int)first.X,
                StartY = (int)first.Y,
                EndX = (int)second.X,
                EndY = (int)second.Y,
            });
        }

        return visualLines;
    }
}
