using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

namespace KleanTrak.Pi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PagePrincipal : IPage
    {
        public event OpenPopupDelegate OpenPopup;
        public event WaitForAbortOperationDelegate AbortOperation;
        public event Action<bool> OpenWaiter;

        public WorklistItem CurrentWorklistItem { set; get; }

        DispatcherTimer timerTimeout = new DispatcherTimer();

        string operationBarcode = "", deviceBarcode = "", accessoryBarcode = "", userBarcode = "";
        string currentBarcode = "";

        bool changingDeviceStatus = false;

        public PagePrincipal()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timerTimeout.Tick += TimerTimeout_Tick;
            timerTimeout.Interval = new TimeSpan(0, 0, ((App)App.Current).Configuration.TimeoutCompletingSteps.HasValue ? ((App)App.Current).Configuration.TimeoutCompletingSteps.Value : 10);

            labelOperation.Text = (((App)App.Current).Configuration.LabelOperationText != null) ? ((App)App.Current).Configuration.LabelOperationText : "";
            labelDevice.Text = (((App)App.Current).Configuration.LabelDeviceText != null) ? ((App)App.Current).Configuration.LabelDeviceText : "";
            labelUser.Text = (((App)App.Current).Configuration.LabelUserText != null) ? ((App)App.Current).Configuration.LabelUserText : "";

            if (((App)App.Current).Configuration.DefaultOperationBarcode != null && ((App)App.Current).Configuration.DefaultOperationBarcode.Length > 0)
            {
                operationBarcode = ((App)App.Current).Configuration.DefaultOperationBarcode;
                grid.RowDefinitions[0].Height = new GridLength(0);
            }
        }

        private async void BarcodeSelected(string barcode)
        {
            Response response = (Response)await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdGetInfoFromBarcode() { Barcode = barcode });
            if (response is CmdGetInfoFromBarcodeResponse && response.Successed)
            {
                switch (((CmdGetInfoFromBarcodeResponse)response).BarcodeType)
                {
                    case BarcodeTypes.Operation:
                        if ((operationBarcode == "" || operationBarcode == barcode) && 
                            (((App)App.Current).Configuration.DefaultOperationBarcode == null || ((App)App.Current).Configuration.DefaultOperationBarcode == ""))
                        {
                            textOperation.Text = ((CmdGetInfoFromBarcodeResponse)response).Description;
                            validOperation.Visibility = Visibility.Visible;
                            operationBarcode = barcode;
                        }
                        else
                            OpenPopup(((App)App.Current).Dictionary["piError"], ((App)App.Current).Dictionary["piOperationAlreadySelected"]);
                        break;

                    case BarcodeTypes.Device:
                        if (deviceBarcode == "" || deviceBarcode == barcode)
                        {
                            textDevice.Text = ((CmdGetInfoFromBarcodeResponse)response).Description;
                            textDeviceStatus.Text = await GetDeviceStatus(barcode);
                            validDevice.Visibility = Visibility.Visible;
                            deviceBarcode = barcode;
                        }
                        else
                            OpenPopup(((App)App.Current).Dictionary["piError"], ((App)App.Current).Dictionary["piDeviceAlreadySelected"]);
                        break;

                    case BarcodeTypes.Operator:
                        if (userBarcode == "" || userBarcode == barcode)
                        {
                            textUser.Text = ((CmdGetInfoFromBarcodeResponse)response).Description;
                            validUser.Visibility = Visibility.Visible;
                            userBarcode = barcode;
                        }
                        else
                            OpenPopup(((App)App.Current).Dictionary["piError"], ((App)App.Current).Dictionary["piOperatorAlreadySelected"]);
                        break;

                    case BarcodeTypes.Accessory:
                        if (accessoryBarcode == "" || accessoryBarcode == barcode)
                        {
                            textDevice.Text = ((CmdGetInfoFromBarcodeResponse)response).Description;
                            textDeviceStatus.Text = "";
                            validDevice.Visibility = Visibility.Visible;
                            accessoryBarcode = barcode;
                        }
                        else
                            OpenPopup(((App)App.Current).Dictionary["piError"], ((App)App.Current).Dictionary["piAccessoryAlreadySelected"]);
                        break;
                }

                if ((operationBarcode.Length == 0 && accessoryBarcode.Length == 0) || userBarcode.Length == 0 || (deviceBarcode.Length == 0 && accessoryBarcode.Length == 0))
                {
                    timerTimeout.Stop();
                    //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "timerTimeout.Stop() 1" });
                    timerTimeout.Start();
                    //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "timerTimeout.Start()" });
                }
                else
                {
                    timerTimeout.Stop();
                    //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "timerTimeout.Stop() 2" });

                    if (deviceBarcode.Length > 0)
                        ChangeDeviceStatus();
                    else if (accessoryBarcode.Length > 0)
                        ChangeAccessoryStatus();
                }
            }
            else
                OpenPopup(((App)App.Current).Dictionary["piError"], response.ErrorMessage);
        }

        private async void ChangeDeviceStatus()
        {
            changingDeviceStatus = true;
            //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "ChangeDeviceStatus()" });

            if (AbortOperation != null)
            {
                if (await AbortOperation(((App)App.Current).Configuration.DelayBeforeApplyChanges))
                {
                    //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "AbortOperation()" });

                    Reset();
                    changingDeviceStatus = false;
                    return;
                }
            }

            if (OpenWaiter != null)
                OpenWaiter(true);

            Response response = (Response)await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdSetDeviceStatus() { OperationBarcode = operationBarcode, DeviceBarcode = deviceBarcode, UserBarcode = userBarcode, WorklistItemID = CurrentWorklistItem != null ? CurrentWorklistItem.ID : 0 });

            if (OpenWaiter != null)
                OpenWaiter(false);

            if (!response.Successed)
                OpenPopup(((App)App.Current).Dictionary["piError"], response.ErrorMessage);

            Reset();
            changingDeviceStatus = false;
        }

        private async void ChangeAccessoryStatus()
        {
            changingDeviceStatus = true;

            if (AbortOperation != null)
            {
                if (await AbortOperation(((App)App.Current).Configuration.DelayBeforeApplyChanges))
                {
                    Reset();
                    changingDeviceStatus = false;
                    return;
                }
            }

            if (OpenWaiter != null)
                OpenWaiter(true);

            Response response = (Response)await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdSetAccessoryStatus() { AccessoryBarcode = accessoryBarcode, UserBarcode = userBarcode, WorklistItemID = CurrentWorklistItem != null ? CurrentWorklistItem.ID : 0 });

            if (OpenWaiter != null)
                OpenWaiter(false);

            if (!response.Successed)
                OpenPopup(((App)App.Current).Dictionary["piError"], response.ErrorMessage);

            Reset();
            changingDeviceStatus = false;
        }

        private void Reset()
        {
            //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "Reset!!!" });
            timerTimeout.Stop();

            operationBarcode = (((App)App.Current).Configuration.DefaultOperationBarcode != null) ? ((App)App.Current).Configuration.DefaultOperationBarcode : "";
            accessoryBarcode = "";
            deviceBarcode = "";
            userBarcode = "";

            textOperation.Text = (((App)App.Current).Configuration.DefaultOperationDescription != null) ? ((App)App.Current).Configuration.DefaultOperationDescription : "";
            textDevice.Text = "";
            textDeviceStatus.Text = "";
            textUser.Text = "";

            validOperation.Visibility = (((App)App.Current).Configuration.DefaultOperationBarcode != null && ((App)App.Current).Configuration.DefaultOperationBarcode.Length > 0) ? Visibility.Visible : Visibility.Collapsed;
            validDevice.Visibility = Visibility.Collapsed;
            validUser.Visibility = Visibility.Collapsed;
        }

        private void TimerTimeout_Tick(object sender, object e)
        {
            Reset();
        }

        public void KeyDownReceived(KeyboardEventArgs k)
        {
            if (changingDeviceStatus)
                return;

            currentBarcode += k.Character;

            if (k.VirtualKey == Windows.System.VirtualKey.Enter && currentBarcode.Length > 0)
            {
                BarcodeSelected(currentBarcode);
                currentBarcode = "";
            }
        }

        private async Task<string> GetDeviceStatus(string barcode)
        {
            string ret = "";
            Response response = (Response)await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdGetDeviceStatus() { DeviceBarcode = barcode });
            if (response is CmdGetDeviceStatusResponse && response.Successed)
                ret = ((CmdGetDeviceStatusResponse)response).Status;

            return ret;
        }
    }
}
