using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CleanTrackPi.Helpers;
using CleanTrackPi.ViewModels;

namespace CleanTrackPi.Views
{
    public partial class MainWindow : UserControl
    {      
        public MainWindowViewModel ViewModel
        {
            get => (MainWindowViewModel)DataContext;
        }

        public MainWindow() =>
            InitializeComponent();

        private void InitializeComponent() =>
            AvaloniaXamlLoader.Load(this);

    }
}
