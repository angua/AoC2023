using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonWPF
{
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

        public static readonly DependencyProperty TilesProperty =
            DependencyProperty.Register(nameof(Tiles), typeof(ObservableCollection<Tile>), typeof(TilesControl),
                new PropertyMetadata(OnTilesPropertyChanged));

        private static void OnTilesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableCollection<Tile> collection && collection.Count > 0)
            {
                var self = (TilesControl)d;
                self._maxX = self.Tiles.Max(t => t.PositionX);
                self._maxY = self.Tiles.Max(t => t.PositionY);
                var mapWidth = (self._maxX + 1) * self.TileSize;
                var mapHeight = (self._maxY + 1) * self.TileSize;

                self._bitmap = new WriteableBitmap(
                    mapWidth,
                    mapHeight,
                    96,
                    96,
                    PixelFormats.Bgr32,
                    null);

                self.DrawTiles();

                self.TilesImage.Source = self._bitmap;
            }
        }

        private void DrawTiles()
        {
            _bitmap.Lock();

            try
            {
                foreach (var tile in Tiles)
                {
                    var color = TileColorSelector.GetColorForTile(tile);

                    DrawRectangle(tile.PositionX, tile.PositionY, TileSize, color);
                }
                _bitmap.AddDirtyRect(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
            }

            finally
            {
                // Release the back buffer and make it available for display.
                _bitmap.Unlock();
            }



        }

        private void DrawRectangle(int positionX, int positionY, int tileSize, Color color)
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

        private void SetPixel(int x, int y, Color color)
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



        public ObservableCollection<Tile> Tiles
        {
            get => (ObservableCollection<Tile>)GetValue(TilesProperty);
            set => SetValue(TilesProperty, value);
        }

        public TilesControl()
        {
            InitializeComponent();

        }

        private void TilesImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
