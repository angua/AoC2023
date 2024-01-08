using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Advent2023.Utils;
using AoC2023Lib.Days.Day17Lib;
using CommonWPF;

namespace AoC2023.Days.Day17;

public class Day17ViewModel : ViewModelBase
{
    public HeatLossMap HeatLoss { get; } = new();

    public ObservableCollection<Tile> VisualGrid { get; set; } = new();
    public ObservableCollection<TileLine> VisualPathLines { get; set; } = new();

    public int MapWidth => HeatLoss.CountX;
    public int MapHeight => HeatLoss.CountY;


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
        GetLowestHeatLoss = new RelayCommand(CanGetLowestHeatLoss, DoGetLowestHeatLoss);
        GetLowestUltraHeatLoss = new RelayCommand(CanGetLowestUltraHeatLoss, DoGetLowestUltraHeatLoss);

        var fileData = ResourceUtils.LoadDataFromResource("Day17", "input.txt");
        HeatLoss.Parse(fileData);

        Task.Run(ParseMap);
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
        _calculating = true;
        _watch.Reset();
        _watch.Start();

        BestMove = await Task.Run(() => HeatLoss.GetLowestHeatLossMove());
        LowestHeatLoss = BestMove.Loss;

        _watch.Stop();
        Timer = _watch.ElapsedMilliseconds;
        _calculating = false;

        ParseLines(BestMove);

        GetLowestHeatLoss.RaiseCanExecuteChanged();
        GetLowestUltraHeatLoss.RaiseCanExecuteChanged();
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
        _calculating = true;
        _watch.Reset();
        _watch.Start();
        
        BestUltraMove = await Task.Run(() => HeatLoss.GetLowestHeatLossMove(true));
        LowestUltraHeatLoss = BestUltraMove.Loss;

        _watch.Stop();
        Timer = _watch.ElapsedMilliseconds;
        _calculating = false;

        ParseLines(BestUltraMove);

        GetLowestHeatLoss.RaiseCanExecuteChanged();
        GetLowestUltraHeatLoss.RaiseCanExecuteChanged();
    }

    private void ParseMap()
    {
        var visuals = new ObservableCollection<Tile>();
        foreach (var pos in HeatLoss.Grid)
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

    private void ParseLines(Move move)
    {
        var route = HeatLoss.GetRoute(move);

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

        App.Current.Dispatcher.Invoke(() =>
        {
            VisualPathLines = visualLines;
            RaisePropertyChanged(nameof(VisualPathLines));
        });
    }
}
