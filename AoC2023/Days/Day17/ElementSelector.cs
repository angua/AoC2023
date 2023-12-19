using System.Windows;
using System.Windows.Controls;

namespace AoC2023.Days.Day17;

public class ElementSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        return ((FrameworkElement)container).FindResource("PositionTemplate") as DataTemplate;
    }

}
