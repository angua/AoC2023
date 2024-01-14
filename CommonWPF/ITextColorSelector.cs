
namespace CommonWPF;

public interface ITextColorSelector
{
    System.Windows.Media.Brush GetColorForText(PositionedText text);
}
