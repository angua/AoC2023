using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using AoC2023Lib.Days.Day02Lib;

namespace AoC2023.Days.Day02;

internal class CubeCountConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is KeyValuePair<CubeColor, int> pair)
        {
            return pair.Value;
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
