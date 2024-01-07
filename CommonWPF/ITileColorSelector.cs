using System.Drawing;

namespace CommonWPF;

public interface ITileColorSelector
{
    Color GetColorForTile(Tile tile);
}
