using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2023Lib.Days.Day13Lib;

namespace AoC2023.Days.Day13
{
    public class VisualPattern
    {
        private Pattern _pattern;

        public ObservableCollection<object> VisualGrid { get; set; } = new();
        public ObservableCollection<object> SmudgedVisualGrid { get; set; } = new();

        

        public VisualPattern(Pattern pattern)
        {
            _pattern = pattern;

            foreach (var element in pattern.Grid)
            {
                VisualGrid.Add(new GridPosition(element));
            }
        }
    }
}
