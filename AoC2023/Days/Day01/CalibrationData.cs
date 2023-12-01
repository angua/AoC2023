using CommonWPF;

namespace AoC2023.Days.Day01;

public class CalibrationData : ViewModelBase
{
    public string CalibrationInput
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public int CalibrationNumber
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public string ReplacedCalibrationInput
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    public int ReplacedCalibrationNumber
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

}
