using System.Collections.ObjectModel;
using Advent2023.Utils;
using AoC2023Lib.Days.Day07Lib;
using CommonWPF;

namespace AoC2023.Days.Day07;

public class Day07ViewModel : ViewModelBase
{
    public GameMaster Master { get; } = new();

    public long TotalWinnings
    {
        get => GetValue<long>();
        set => SetValue(value);
    }
    public long TotalWinningsWithJoker
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public ObservableCollection<VisualSet> VisualSets { get; set; } = new();

    public Day07ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day07", "input.txt");

        Master.Parse(fileData);

        foreach (var set in Master.Sets) 
        {
            VisualSets.Add(new VisualSet(set));
        }

        TotalWinnings = Master.GetTotalWinnings();
        TotalWinningsWithJoker = Master.GetTotalWinnings(true);


    }
}
