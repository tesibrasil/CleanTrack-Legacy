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
using KleanTrak.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KleanTrak.Pi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageWorklist : IPage
    {
        public event OpenPopupDelegate OpenPopup;
        public event Action<WorklistItem> ConfirmPressed;

        public PageWorklist()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            btnConfirm.Content = ((App)App.Current).Dictionary["piConfirm"];
            Response response = (Response)await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdGetWorklist());
            if (response is CmdGetWorklistResponse)
            {
                listBox.ItemsSource = ((CmdGetWorklistResponse)response).Items;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
                return;

            if (ConfirmPressed != null)
                ConfirmPressed((WorklistItem)listBox.SelectedItem);
        }

        public void KeyDownReceived(KeyboardEventArgs k)
        {
        }
    }
}
