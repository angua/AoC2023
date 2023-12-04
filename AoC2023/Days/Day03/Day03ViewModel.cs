using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Advent2023.Utils;
using AoC2023Lib.Days.Day03Lib;
using CommonWPF;

namespace AoC2023.Days.Day03;

public class Day03ViewModel : ViewModelBase
{
    private List<SchematicNumber> _enginePartNumbers;
    private Dictionary<Vector2, int> _gearPositions;

    public Engineering Engineer { get; set; } = new();

    public ObservableCollection<VisualPosition> VisualSchematic { get; set; } = new();

    public int MaxX
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public int MaxY
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public int EnginePartSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public int GearRatioSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public Day03ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day03", "input.txt");

        Engineer.Parse(fileData);
        MaxX = (int)Engineer.Schematic.Max(p => p.Key.X);
        MaxY = (int)Engineer.Schematic.Max(p => p.Key.Y);

        _enginePartNumbers = Engineer.FindPartNumbers();
        EnginePartSum = Engineer.GetEnginePartSum(_enginePartNumbers);


        _gearPositions = Engineer.FindGearPositions();
        GearRatioSum = Engineer.GetGearRatioSum(_gearPositions);


        Task.Run(ParseVisualSchematic);

    }

    private void SetVisualGearPositions(Dictionary<Vector2, int> gearPositions)
    {
        foreach (var gearPosition in gearPositions)
        {
            var visualPos = VisualSchematic.First(p => p.PositionX == gearPosition.Key.X && p.PositionY == gearPosition.Key.Y);
            visualPos.IsGear = true;
        }
    }

    private void SetVisualPartNumbers(List<SchematicNumber> enginePartNumbers)
    {
        foreach (var partNumber in enginePartNumbers)
        {
            foreach (var digit in partNumber.Positions)
            {
                var visualPos = VisualSchematic.First(p => p.PositionX == digit.Key.X && p.PositionY == digit.Key.Y);
                visualPos.IsSchematicNumber = true;
            }
        }
    }

    private void ParseVisualSchematic()
    {
        ParsePositions();
        App.Current.Dispatcher.Invoke(() =>
        {
            SetVisualGearPositions(_gearPositions);
            SetVisualPartNumbers(_enginePartNumbers);
        });
    }

    private void ParsePositions()
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var pos in Engineer.Schematic)
            {
                VisualSchematic.Add(new VisualPosition(pos));
            }
        });
    }
}
