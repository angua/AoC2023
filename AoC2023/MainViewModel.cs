using CommonWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AoC2023
{
    internal class MainViewModel : ViewModelBase
    {
        public List<Day> Days { get; } = new();

        public DataTemplateSelector DayTemplateSelector { get; set; }

        public Day SelectedDay
        {
            get => GetValue<Day>();
            set => SetValue(value);
        }


    }
}
