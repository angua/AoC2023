using System.Collections.Generic;
using AoC2023Lib.Days.Day02Lib;
using CommonWPF;

namespace AoC2023.Days.Day02;

public class ColorDraw : ViewModelBase
{
    public ColorDraw()
    {}

    public ColorDraw(CubeColor color, int count)
    {
        CubeColor = color;
        Count = count;
    }

    public CubeColor CubeColor
    {
        get => GetValue<CubeColor>();
        set => SetValue(value);
    }

    public int Count
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

}
