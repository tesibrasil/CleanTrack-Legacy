using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CleanTrackPi.Views
{
    public partial class NotFoundView : UserControl
    {
        public NotFoundView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
