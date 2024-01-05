using System;
using System.Collections.ObjectModel;
using System.Linq;
using Advent2023.Utils;
using AoC2023Lib.Days.Day25Lib;
using CommonWPF;

namespace AoC2023.Days.Day25;

public class Day25ViewModel : ViewModelBase
{
    public SnowMachine Machine { get; } = new();

    public ObservableCollection<string> TestedParts { get; set; } = new();

    public ObservableCollection<VisibleConnectionCount> ConnectionCounts { get; set; } = new();


    public long GroupSizeProduct
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public Day25ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day25", "input.txt");
        Machine.Parse(fileData);

        TestNextConnections = new RelayCommand(CanTestNextConnections, DoTestNextConnections);
        FindGroupSizeProduct = new RelayCommand(CanFindGroupSizeProduct, DoFindGroupSizeProduct);
    }

    public RelayCommand TestNextConnections { get; }
    public bool CanTestNextConnections()
    {
        return true;
    }
    public void DoTestNextConnections()
    {
        var part = Machine.CreateNextConnectionPaths();
        TestedParts.Add(part.Name);

        UpdateConnectionCounts();
    }

    public RelayCommand FindGroupSizeProduct { get; }
    public bool CanFindGroupSizeProduct()
    {
        return true;
    }
    public void DoFindGroupSizeProduct()
    {
        GroupSizeProduct = Machine.GetGroupSizeProduct();

        foreach (var part in Machine.TestedParts.Select(part => part.Name))
        {
            TestedParts.Add(part);
        }

        UpdateConnectionCounts();
    }


    private void UpdateConnectionCounts()
    {
        var counts = new ObservableCollection<VisibleConnectionCount>();
        foreach (var connectionCount in Machine.ConnectionCounts.OrderByDescending(c => c.Value)) 
        {
            counts.Add(new VisibleConnectionCount()
            {
                ConnectionString = connectionCount.Key.ConnectionString,
                Count = connectionCount.Value
            });
        }

        ConnectionCounts = counts;
        RaisePropertyChanged(nameof(ConnectionCounts));
    }

   
}
