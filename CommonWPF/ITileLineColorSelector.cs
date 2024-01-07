using System.Drawing;

namespace CommonWPF;

public interface ITileLineColorSelector
{
    Color GetColorForTileLine(TileLine tileLine);
}
