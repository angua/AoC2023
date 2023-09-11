using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace AoC2023;

public class Day
{
    public int DayNumber { get; set; }

    public string Title { get; set; }

    public ImageSource Image { get; set; } 

    public Func<object> CreateViewModel { get; set; } 

    public Type ViewType { get; set; }

}
