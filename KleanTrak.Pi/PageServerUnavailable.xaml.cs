using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Connectivity;
using Windows.Networking;
using System.Threading;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KleanTrak.Pi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageServerUnavailable : IPage
    {
        public event OpenPopupDelegate OpenPopup;

        public PageServerUnavailable()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            textError.Text = ((App)App.Current).Dictionary["piCantConnectToServer"];
            textDevice.Text = ((App)App.Current).Dictionary["piDeviceAddress"];
            textServer.Text = ((App)App.Current).Dictionary["piServerAddress"];

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            textIP.Text = GetCurrentIpv4Address();

            try
            {
                if (((App)App.Current).Configuration.ServerHttpEndpoint != null)
                {
                    Uri uri = new Uri(((App)App.Current).Configuration.ServerHttpEndpoint);
                    textServerIP.Text = uri.Host.ToUpper() + ":" + uri.Port;
                }
            }
            catch (Exception)
            {
                ((App)App.Current).Configuration.ServerHttpEndpoint = "";
            }

            if (((App)App.Current).Configuration.ServerHttpEndpoint == null || ((App)App.Current).Configuration.ServerHttpEndpoint == "")
            {
                textError.Text = ((App)App.Current).Dictionary["piDeviceConfigurationNeeded"];
                textServerIP.Text = ((App)App.Current).Dictionary["piNotSpecified"];
            }
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                textIP.Text = GetCurrentIpv4Address();
            });
        }

        public void KeyDownReceived(KeyboardEventArgs k)
        {
        }

        public string GetCurrentIpv4Address()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();
            if (icp != null
                  && icp.NetworkAdapter != null
                  && icp.NetworkAdapter.NetworkAdapterId != null)
            {
                var name = icp.ProfileName;

                var hostnames = NetworkInformation.GetHostNames();

                foreach (var hn in hostnames)
                {
                    if (hn.IPInformation != null
                        && hn.IPInformation.NetworkAdapter != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                                                   != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                    == icp.NetworkAdapter.NetworkAdapterId
                        && hn.Type == HostNameType.Ipv4)
                    {
                        return hn.CanonicalName;
                    }
                }
            }

            return "---";
        }
    }
}
