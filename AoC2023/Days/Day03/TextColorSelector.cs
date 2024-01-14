using System.Windows.Media;
using AoC2023Lib.Days.Day03Lib;
using CommonWPF;

namespace AoC2023.Days.Day03;

internal class TextColorSelector : ITextColorSelector
{
    public Brush GetColorForText(PositionedText text)
    {
        if (text is not VisualPosition visual)
        {
            return new SolidColorBrush()
            {
                Color = Colors.White
            };
        }

        if (visual.Type == SymbolType.Number)
        {
            return new SolidColorBrush()
            {
                Color = Color.FromRgb(255, 124, 102)
            };
        }
        if (visual.Type == SymbolType.Gear)
        {
            return new SolidColorBrush()
            {
                Color = Color.FromRgb(255, 246, 0)
            };
        }
        return new SolidColorBrush()
        {
            Color = Color.FromRgb(204, 204, 204)
        };

    }

}

