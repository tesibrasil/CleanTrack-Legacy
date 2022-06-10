using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CleanTrackPi.Helpers;
using System.Threading.Tasks;

namespace CleanTrackPi.Views
{
    public partial class HeaderView : UserControl
    {
        public TextBlock Title
        {
            get => this.Find<TextBlock>("Title");
        }

        public Border HeaderBorder
        {
            get => this.Find<Border>("HeaderBorder");
        }

        public HeaderView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public async void OnInfo(object sender, RoutedEventArgs e)
        {
            var tt = Title.Text;
            var ping = Util.CheckServerStatus(App.Settings.ServerHttpEndpoint);
            Title.Text = App.DumpSettings() + ", Ping: " + (ping ? "OK" : "FAIL");
            await Task.Delay(3000);
            Title.Text = tt;
        }
    }
}
