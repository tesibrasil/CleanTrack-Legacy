using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CleanTrackPi.Views
{
    public partial class BarcodeView : UserControl
    {
        public BarcodeView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
