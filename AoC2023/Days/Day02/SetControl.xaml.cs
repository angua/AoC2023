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

namespace AoC2023.Days.Day02
{
    /// <summary>
    /// Interaction logic for SetControl.xaml
    /// </summary>
    public partial class SetControl : UserControl
    {
        public SetControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            name: nameof(ItemsSource),
            propertyType: typeof(ObservableCollection<ColorDraw>),
            ownerType: typeof(SetControl),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: null, propertyChangedCallback: OnItemsSourceChanged));

        public ObservableCollection<ColorDraw> ItemsSource
        {
            get => (ObservableCollection<ColorDraw>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (SetControl)d;
            self.SetVisualizer.ItemsSource = self.ItemsSource;
        }
    }
}
