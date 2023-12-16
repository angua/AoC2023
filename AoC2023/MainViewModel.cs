using AoC2023.Days;
using CommonWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace AoC2023;

internal class MainViewModel : ViewModelBase
{
    public List<Day> Days { get; } = new();

    public DataTemplateSelector DayTemplateSelector { get; set; }

    public Day SelectedDay
    {
        get => GetValue<Day>();
        set => SetValue(value);
    }

    public MainViewModel()
    {
        // find days in assembly using reflection
        var executingAssembly = Assembly.GetExecutingAssembly();
        // find all classes
        foreach (var type in executingAssembly.GetTypes().Where(t => t.IsClass))
        {
            // each day has a day provider interface 
            if (type.FindInterfaces((type, criteria) => type == typeof(IDayProvider), null).Length > 0)
            {
                var provider = (IDayProvider)Activator.CreateInstance(type);
                Days.Add(provider.GetDay());
            }
        }

        Days = Days.OrderBy(d => d.DayNumber).ToList();

        SelectedDay = Days.FirstOrDefault(d => d.DayNumber == 15);
    }
    

}
