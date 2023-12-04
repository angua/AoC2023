using System;
using System.Globalization;
using System.Windows.Data;

namespace AoC2023.Days.Day03;

internal class PositionXConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return 15 * (int)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
