using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AoC2023.Days;

internal class DayTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (!(item is Day day))
        {
            return null;
        }

        var factory = new FrameworkElementFactory(day.ViewType);

        // Get the view model and assign it to the DataContext property
        factory.SetValue(FrameworkElement.DataContextProperty, day.CreateViewModel());

        return new DataTemplate
        {
            VisualTree = factory,
        };
    }
}