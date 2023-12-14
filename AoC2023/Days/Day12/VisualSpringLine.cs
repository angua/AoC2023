using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2023Lib.Days.Day12Lib;
using CommonWPF;

namespace AoC2023.Days.Day12
{
    public class VisualSpringLine : ViewModelBase
    {
        private SpringLine _row;

        public VisualSpringLine(SpringLine row)
        {
            _row = row;
        }

        public string Line => _row.Line;

        public long ArrangementCount => _row.ArrangementCount;
        public long UnfoldedArrangementCount => _row.UnfoldedArrangementCount;

    }
}
