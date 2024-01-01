using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Advent2023.Utils;
using AoC2023Lib.Days.Day23Lib;
using CommonWPF;
using Microsoft.VisualBasic;

namespace AoC2023.Days.Day23;

public class Day23ViewModel : ViewModelBase
{
    public PathFinding PathFinder { get; set; } = new();

    public long LongestPath { get; set; }
    public long LongestPathWithoutSlopes { get; set; }

    public ObservableCollection<CommonWPF.Tile> VisualGrid { get; set; } = new();

    public int Width => PathFinder.Width;
    public int Height => PathFinder.Height;

    public Day23ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day23", "input.txt");
        PathFinder.Parse(fileData);

        Task.Run(ParseVisuals);

        //LongestPath = PathFinder.FindLongestPath();
        // LongestPathWithoutSlopes = PathFinder.FindLongestPathWithoutSlopes();
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
