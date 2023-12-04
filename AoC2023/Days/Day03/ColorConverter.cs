using System;
using System.Globalization;
using System.Windows.Data;

namespace AoC2023.Days.Day03;

internal class ColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is VisualPosition pos)
        {
            if (pos.IsSchematicNumber)
            {
                return "#ff7c66";
            }
            if (pos.IsGear)
            {
                return "#fff600";
            }
        }
        return "#cccccc";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
