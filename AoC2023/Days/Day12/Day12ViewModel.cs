using System.Collections.ObjectModel;
using Advent2023.Utils;
using AoC2023Lib.Days.Day12Lib;
using CommonWPF;

namespace AoC2023.Days.Day12;

public class Day12ViewModel : ViewModelBase
{
    public SpringHandler Springinator { get; } = new();

    public long ArrangementSum
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public long UnfoldedArrangementSum
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public ObservableCollection<VisualSpringLine> Rows { get; set; } = new();

    public Day12ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day12", "input.txt");
        Springinator.Parse(fileData);

        ArrangementSum = Springinator.GetArrangementSum();
        UnfoldedArrangementSum = Springinator.GetUnfoldedArrangementSum();
        
        foreach (var line in Springinator.SpringLines)
        {
            Rows.Add(new VisualSpringLine(line));
        }
    }
}
