using System.Windows.Media;

namespace CommonWPF;

public interface ITileColorSelector
{
    Color GetColorForTile(Tile tile);
}
