using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Common;

namespace CommonWPF;

/// <summary>
/// Interaction logic for TilesControl.xaml
/// </summary>
public partial class TilesControl : UserControl
{
    private WriteableBitmap _bitmap;
    private int _maxX;
    private int _maxY;

    public int TileSize { get; set; }

    public ITileColorSelector TileColorSelector { get; set; }
    public ITileLineColorSelector TileLineColorSelector { get; set; }

    public TilesControl()
    {
        InitializeComponent();
    }

    public ObservableCollection<Tile> Tiles
    {
        get => (ObservableCollection<Tile>)GetValue(TilesProperty);
        set => SetValue(TilesProperty, value);
    }

    public static readonly DependencyProperty TilesProperty =
        DependencyProperty.Register(nameof(Tiles), typeof(ObservableCollection<Tile>), typeof(TilesControl),
            new PropertyMetadata(OnTilesPropertyChanged));


    public ObservableCollection<TileLine> TileLines
    {
        get => (ObservableCollection<TileLine>)GetValue(TileLinesProperty);
        set => SetValue(TileLinesProperty, value);
    }

    public static readonly DependencyProperty TileLinesProperty =
        DependencyProperty.Register(nameof(TileLines), typeof(ObservableCollection<TileLine>), typeof(TilesControl),
            new PropertyMetadata(OnTileLinesPropertyChanged));

    private static void OnTileLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is ObservableCollection<TileLine> collection && collection.Count > 0)
        {
            var self = (TilesControl)d;
            self.DrawBitmap();
        }
    }

    private static void OnTilesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is ObservableCollection<Tile> collection && collection.Count > 0)
        {
            var self = (TilesControl)d;
            self.DrawBitmap();
        }
    }

    private void DrawBitmap()
    {
        _maxX = Tiles.Max(t => t.PositionX);
        _maxY = Tiles.Max(t => t.PositionY);
        var mapWidth = (_maxX + 1) * TileSize;
        var mapHeight = (_maxY + 1) * TileSize;

        _bitmap = new WriteableBitmap(
            mapWidth,
            mapHeight,
            96,
            96,
            PixelFormats.Bgr32,
            null);

        _bitmap.Lock();

        try
        {
            DrawTiles();

            if (TileLines is ObservableCollection<TileLine> collection && collection.Count > 0)
            {
                DrawLines();
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
        }

        finally
        {
            // Release the back buffer and make it available for display.
            _bitmap.Unlock();
        }
        TilesImage.Source = _bitmap;
    }

    private void DrawTiles()
    {

        foreach (var tile in Tiles)
        {
            var color = TileColorSelector.GetColorForTile(tile);

            DrawRectangle(tile.PositionX, tile.PositionY, TileSize, color);
        }

    }

    private void DrawRectangle(int positionX, int positionY, int tileSize, System.Drawing.Color color)
    {
        // start positions on bitmap
        var startX = positionX * tileSize;
        var startY = positionY * tileSize;

        // rectangle
        for (var deltaX = 0; deltaX < tileSize; deltaX++)
        {
            for (var deltaY = 0; deltaY < tileSize; deltaY++)
            {
                var posX = startX + deltaX;
                var posY = startY + deltaY;
                SetPixel(posX, posY, color);
            }
        }
    }

    private void DrawLines()
    {
        var half = TileSize / 2;

        foreach (var line in TileLines)
        {
            var color = TileLineColorSelector.GetColorForTileLine(line);
            var lineStart = new Vector2(line.StartX, line.StartY) * TileSize + new Vector2(half, half);
            var lineEnd = new Vector2(line.EndX, line.EndY) * TileSize + new Vector2(half, half);

            var linePoints = MathUtils.GetPointsOnLine(lineStart, lineEnd);

            foreach (var point in linePoints)
            {
                SetPixel((int)point.X, (int)point.Y, color);
            }
        }

    }


    private void SetPixel(int x, int y, System.Drawing.Color color)
    {
        var bytesPerPixel = (_bitmap.Format.BitsPerPixel + 7) / 8;
        var stride = _bitmap.PixelWidth * bytesPerPixel;

        int posX = x * bytesPerPixel;
        int posY = y * stride;
        unsafe
        {
            // Get a pointer to the back buffer.
            var backBuffer = _bitmap.BackBuffer;

            // Find the address of the pixel to draw.
            backBuffer += y * _bitmap.BackBufferStride;
            backBuffer += x * 4;

            // Compute the pixel's color.
            int color_data = color.R << 16; // R
            color_data |= color.G << 8;   // G
            color_data |= color.B << 0;   // B

            // Assign the color data to the pixel.
            *((int*)backBuffer) = color_data;
        }
    }





    private void TilesImage_SizeChanged(object sender, SizeChangedEventArgs e)
    {

    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {

    }
}
