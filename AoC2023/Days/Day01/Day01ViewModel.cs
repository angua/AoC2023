using System.Collections.ObjectModel;
using Advent2023.Utils;
using AoC2023Lib.Days.Day01Lib;
using CommonWPF;

namespace AoC2023.Days.Day01;

public class Day01ViewModel : ViewModelBase
{
    public Calibrator Calibrinator { get; set; } = new();

    public ObservableCollection<CalibrationData> CalibrationDataList { get; set; } = new();

    public int CalibrationSum
    { 
        get => GetValue<int>();
        set => SetValue(value); 
    }

    public int SpelledCalibrationSum
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public Day01ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day01", "input.txt");
        Calibrinator.Parse(fileData);

        foreach ( var input in Calibrinator.CalibrationInput)
        {
            CalibrationDataList.Add(new CalibrationData()
            {
                CalibrationInput = input
            });
        }

        var list = Calibrinator.GetCalibrationNumbers();
        for (int i = 0; i < list.Count; i++)
        {
            CalibrationDataList[i].CalibrationNumber = list[i];
        }
        CalibrationSum = Calibrinator.GetCalibrationSum(list);

        var replaceList = Calibrinator.GetSpelledCalibrationNums();
        for (int i = 0; i < replaceList.Count; i++)
        {
            CalibrationDataList[i].ReplacedCalibrationInput = replaceList[i];
        }

        var replaceNums = Calibrinator.GetCalibrationNumbers(replaceList);
        for (int i = 0; i < replaceNums.Count; i++)
        {
            CalibrationDataList[i].ReplacedCalibrationNumber = replaceNums[i];
        }
        SpelledCalibrationSum = Calibrinator.GetCalibrationSum(replaceNums);
    }

}
