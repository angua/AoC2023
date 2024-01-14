using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CommonWPF;

/// <summary>
/// Interaction logic for PositionedTextControl.xaml
/// </summary>
public partial class PositionedTextControl : UserControl
{
    public ITextColorSelector TextColorSelector { get; set; }

    public PositionedTextControl()
    {
        InitializeComponent();
    }

    public ObservableCollection<PositionedText> TextPieces
    {
        get => (ObservableCollection<PositionedText>)GetValue(TextPiecesProperty);
        set => SetValue(TextPiecesProperty, value);
    }

    public static readonly DependencyProperty TextPiecesProperty =
        DependencyProperty.Register(nameof(TextPieces), typeof(ObservableCollection<PositionedText>), typeof(PositionedTextControl),
            new PropertyMetadata(OnTextPiecesPropertyChanged));

    private static void OnTextPiecesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is ObservableCollection<PositionedText> collection && collection.Count > 0)
        {
            var self = (PositionedTextControl)d;
            self.ParseText();
        }
    }

    private void ParseText()
    {
        var textBlock = new TextBlock();
        textBlock.TextWrapping = TextWrapping.NoWrap;
        textBlock.FontFamily = new FontFamily("Courier New");
        textBlock.FontSize = 18;

        var maxY = TextPieces.Max(p => p.PositionY);

        for (int y = 0; y <= maxY; y++)
        {
            var linePieces = TextPieces.Where(p => p.PositionY == y).ToList();
            var maxX = linePieces.Max(p => p.PositionX);

            for (int x = 0; x <= maxX; x++)
            {
                var currentText = linePieces.FirstOrDefault(p => p.PositionX == x);

                if (currentText != null)
                {
                    textBlock.Inlines.Add(new Run(currentText.Text) { Foreground = TextColorSelector.GetColorForText(currentText) });
                }

            }
            textBlock.Inlines.Add(new Run("\n"));
        }

        Schematic.Children.Add(textBlock);
    }
}
