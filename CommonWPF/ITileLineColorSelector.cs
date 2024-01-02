using System.Windows.Media;

namespace CommonWPF;

public interface ITileLineColorSelector
{
    Color GetColorForTileLine(TileLine tileLine);
}
