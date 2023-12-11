using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AoC2023Lib.Days.Day02Lib;

namespace AoC2023.Days.Day02
{
    internal class CubeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is KeyValuePair<CubeColor, int> pair)
            {
                return pair.Key switch
                {
                    CubeColor.red => Color.Red,
                    CubeColor.green => Color.Green,
                    CubeColor.blue => Color.Blue,
                    _ => Color.Black
                };
            }

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
