using CommonWPF;

namespace AoC2023.Days.Day02;

public class VisualSet : ViewModelBase
{
    public int Red
    {
        get => GetValue<int>(); 
        set => SetValue(value);
    }
    public int Blue
    {
        get => GetValue<int>();
        set => SetValue(value);
    }
    public int Green
    {
        get => GetValue<int>();
        set => SetValue(value);
    }
}