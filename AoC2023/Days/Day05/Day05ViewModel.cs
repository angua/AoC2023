using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Advent2023.Utils;
using AoC2023Lib.Days.Day05Lib;
using CommonWPF;

namespace AoC2023.Days.Day05;

public class Day05ViewModel : ViewModelBase
{
    public Gardening Gardener { get; set; } = new();

    // <Seed number, location number>
    public Dictionary<long, long> SeedLocations { get; set; } = new();

    public long ClosestLocation
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public Dictionary<long, long> SeedRangeLocations { get; set; } = new();

    public long ClosestRangeLocation
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public Day05ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day05", "input.txt");

        Gardener.Parse(fileData);

        SeedLocations = Gardener.GetSeedLocations();

        ClosestLocation = Gardener.GetClosestLocation(SeedLocations);

        // SeedRangeLocations= Gardener.GetSeedRangeLocations();

        ClosestRangeLocation = Gardener.GetClosestRangeLocation(SeedRangeLocations);


    }
}
