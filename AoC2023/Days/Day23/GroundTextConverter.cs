using System;
using System.Globalization;
using System.Windows.Data;

namespace AoC2023.Days.Day23;

internal class GroundTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (char)value switch
        {
            '.' => "",
            '#' => "",
            _ => value
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
