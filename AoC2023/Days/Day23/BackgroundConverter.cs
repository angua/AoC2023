using System;
using System.Globalization;
using System.Windows.Data;

namespace AoC2023.Days.Day23;

internal class BackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (char)value switch
        {
            '.' => "#8dff28",
            '#' => "#146d2f",
            _ => "#62f2ff"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
