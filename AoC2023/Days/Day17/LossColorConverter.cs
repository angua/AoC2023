using System;
using System.Globalization;
using System.Windows.Data;
using CommonWPF;

namespace AoC2023.Days.Day17;

internal class LossColorConverter : IValueConverter
{
    private ColourGradient _colours = new ColourGradient();

    public LossColorConverter()
    {
        _colours.AddToColourMappings(1, "#FF0000");
        _colours.AddToColourMappings(5, "#FFFF00");
        _colours.AddToColourMappings(9, "#00FF00");
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
       return _colours.GetColour((int)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
