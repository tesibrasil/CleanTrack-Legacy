using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CleanTrackPi.Helpers;
using CleanTrackPi.ViewModels;

namespace CleanTrackPi.Views
{
    public partial class StartUpView : Window
    {
        public StartUpView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var hostTb = this.FindControl<TextBox>("barcode");
            if (hostTb != null)
                hostTb.AttachedToVisualTree += (s, e) => hostTb.Focus();
        }

        private void InitializeComponent() =>
            AvaloniaXamlLoader.Load(this);

        public void Init(MainWindowViewModel vm) =>
            this.Find<MainWindow>("MainView").DataContext = vm;

        public void OnBarcodeCompose(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                var hostTb = this.FindControl<TextBox>("barcode");
                if (hostTb != null)
                {
                    Listener.SendToken(hostTb.Text + "\r");
                    hostTb.Text = "";
                }                    
            }
        }
    }
}
