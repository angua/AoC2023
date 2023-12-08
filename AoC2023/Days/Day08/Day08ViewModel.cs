using System.Collections.ObjectModel;
using System.Xml.XPath;
using Advent2023.Utils;
using AoC2023Lib.Days.Day08Lib;
using CommonWPF;

namespace AoC2023.Days.Day08;

public class Day08ViewModel : ViewModelBase
{
    public Navigator Navi { get; } = new();

    public ObservableCollection<Position> Positions { get; } = new();

    public ObservableCollection<ObservableCollection<Position>> StepPositions { get; set; } = new();

    public int CurrentInstructionNum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }
    public Instruction CurrentInstruction
    {
        get => GetValue<Instruction>();
        set => SetValue(value);
    }


    public long Steps
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public long MultiSteps
    {
        get => GetValue<long>();
        set => SetValue(value);
    }

    public Day08ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day08", "input.txt");
        Navi.Parse(fileData);

        foreach (var position in Navi.Positions) 
        {
            Positions.Add(position);
        }

        var startPositions = new ObservableCollection<Position>();
        foreach (var pos in Navi.CurrentPositions)
        {
            startPositions.Add(pos);
        }
        StepPositions.Add(startPositions);

        CurrentInstructionNum = Navi.CurrentInstructionNum;
        CurrentInstruction = Navi.Instructions[CurrentInstructionNum];


        Steps = Navi.GetStepsToDestination();
        // MultiSteps = Navi.GetMultiStepsToDestination();

        NextStep = new RelayCommand(CanDoNextStep, DoNextStep);

    }

    public RelayCommand NextStep { get; }
    public bool CanDoNextStep()
    {
        return true;
    }
    public void DoNextStep()
    {
        var nextPositions = Navi.GetNextPositions();

        var newPos = new ObservableCollection<Position>();

        foreach (var pos in nextPositions)
        {
            newPos.Add(pos);
        }

        StepPositions.Add(newPos);

        CurrentInstructionNum = Navi.CurrentInstructionNum;
        CurrentInstruction = Navi.Instructions[CurrentInstructionNum];
    }


}
