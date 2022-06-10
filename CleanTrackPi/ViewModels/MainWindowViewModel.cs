using Avalonia.Media;
using CleanTrackPi.ConnectionClass;
using CleanTrackPi.Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using KleanTrak.Model;
using CleanTrackPi.Helpers;
using Avalonia.Threading;

namespace CleanTrackPi.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Instance { get; private set; }

        public MainWindowViewModel() =>
            Instance = this;

        IBrush HeaderWaitingColor = SolidColorBrush.Parse("#111111");

        public void Init()
        {
            Util.WriteLog("Init...");
            var serverStatus = Util.CheckServerStatus(App.Settings?.ServerHttpEndpoint);

            PageNotFoundVisibility = false;
            BarcodeVisibility = false;
            HeaderColor = HeaderWaitingColor;
            if (serverStatus)
            {
                WriteLog("Starting UDP listener");
                StartUp();

                Listener.ReceiveBarcode((s) => BarcodeText = s);

                this.WhenAnyValue(x => x.BarcodeText)
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DoSearch!);

            }
            else
                ServerNotFound();
            Util.WriteLog("...Init");
        }

        public void StartUp() //68 MainPage.xaml.cs
        {
            PageNotFoundVisibility = false;
            BarcodeVisibility = true;

            OperationLabel = "piOperation".i18n();
            SondaLabel = "piDevice".i18n();
            StateLabel = "piActualState".i18n();
            OperatorLabel = "piOperator".i18n();
            OperationFlag = false;
            SondaFlag = false;
            OperatorFlag = false;
            OperationName = "";
            SondaName = "";
            StateName = "";
            OperatorName = "";
            BarcodeText = "";
        }

        public void ServerNotFound() //68 MainPage.xaml.cs
        {
            PageNotFoundVisibility = true;
            BarcodeVisibility = false;
            SecondLine = "piDeviceAddress".i18n();
            ThirdLine = Util.GetLocalIPAddress();
            FourthLine = "piServerAddress".i18n();
            FifthLine = App.Settings?.ServerHttpEndpoint;
            Textbox3Foreground = Brushes.DarkRed;
        }

        public void Reset()
        {
            BackgroundVisibility = false;
            BackgroundColor = Brushes.Transparent;
            HeaderColor = HeaderWaitingColor;
            MessageBox = "";
            Progress = false;
            OperationName = "";
            OperationFlag = false;
            SondaName = "";
            StateName = "";
            SondaFlag = false;
            OperatorName = "";
            OperatorFlag = false;
            BarcodeText = "";
            Progress = false;
            timerTimeout.Stop();
        }

        public async void DoSearch(string s)
        {
            timerTimeout.Interval = new TimeSpan(0, 0, 10);
            timerTimeout.Tick += (s, e) => Reset();
            NewBarcodeScanned = false;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            if (!string.IsNullOrWhiteSpace(s))
            {
                WriteLog($"BARCODE SCANNED: {s}");
                timerTimeout.Stop();

                Response response = (Response)await PiHttpClient.Send(App.Settings.ServerHttpEndpoint,
                    new CmdGetInfoFromBarcode() { Barcode = s });

                if (!response.Successed)
                {
                    WriteLog(response.ErrorMessage);
                    BarcodeText = "";
                    return;
                }

                if (response is CmdGetInfoFromBarcodeResponse)
                {
                    switch (((CmdGetInfoFromBarcodeResponse)response).BarcodeType)
                    {
                        case BarcodeTypes.Operation:
                            OnOperation(s, response);
                            break;

                        case BarcodeTypes.Device:
                            await OnDevice(s, response);
                            break;
                        case BarcodeTypes.Operator:
                            OnOperator(s, response);
                            break;

                        case BarcodeTypes.Accessory:
                            OnAccessory(s, response);
                            break;
                    }
                }

                BarcodeText = "";
                if ((!string.IsNullOrWhiteSpace(OperationName) ||
                    !string.IsNullOrWhiteSpace(BarcodeAcessory)) &&
                    !string.IsNullOrWhiteSpace(OperatorName) &&
                    !string.IsNullOrWhiteSpace(SondaName))
                {
                    WriteLog("Updating..");
                    if (!string.IsNullOrWhiteSpace(SondaName))
                        ChangeDeviceStatus();
                    else if (!string.IsNullOrWhiteSpace(BarcodeAcessory))
                        ChangeAccessoryStatus();
                }
                else
                    if (NewBarcodeScanned)
                {
                    timerTimeout.Stop();
                    BackgroundVisibility = true;
                    BackgroundColor = Brushes.Transparent;
                    MessageBox = "";
                    Progress = false;

                    timerTimeout.Start();
                    Progress = true;
                    MessageBox = "piWaiting".i18n();
                }
            }
        }

        #region _DOSEARCH_LOGIC

        private void OnAccessory(string s, Response response)
        {
            if (String.IsNullOrWhiteSpace(BarcodeAcessory) || BarcodeAcessory.Equals(s))
            {
                SondaName = ((CmdGetInfoFromBarcodeResponse)response).Description;
                StateName = "";
                BarcodeAcessory = s;
                WriteLog($"SCANNING SUCESSFULL: Selected Acessory - {SondaName}.");
            }
            else
            {
                timerTimeout.Stop();
                Progress = false;
                BackgroundVisibility = true;
                BackgroundColor = Brushes.Red;
                HeaderColor = Brushes.Red;
                MessageBox = "piBarcodeNotFound".i18n();
                timerTimeout.Start();
                WriteLog($"SCANNING ERROR: Acessory aready scanned.\n {response.ErrorMessage}");
            }
        }

        private void OnOperator(string s, Response response)
        {
            if (String.IsNullOrWhiteSpace(OperatorName) || Operator.Tag.Equals(s))
            {
                OperatorName = ((CmdGetInfoFromBarcodeResponse)response).Description;
                NewBarcodeScanned = true;
                OperatorFlag = true;
                Operator = new CleanTrackPi.Models.Operator() { Tag = s };
                WriteLog($"SCANNING SUCESSFULL: Selected Operator - {OperatorName}.");
            }
            else
            {
                timerTimeout.Stop();
                Progress = false;
                BackgroundVisibility = true;
                BackgroundColor = Brushes.Red;
                HeaderColor = Brushes.Red;
                MessageBox = "piBarcodeNotFound".i18n();
                timerTimeout.Start();
                WriteLog($"SCANNING ERROR: Operator aready scanned.\n {response.ErrorMessage}");
            }
        }

        private async Task OnDevice(string s, Response response)
        {                     
            if (String.IsNullOrWhiteSpace(SondaName) || Device == null || Device.Tag.Equals(s))
            {
                SondaName = ((CmdGetInfoFromBarcodeResponse)response).Description;
                StateName = await GetDeviceStatus(s);
                Device = new CleanTrackPi.Models.Device() { Tag = s };
                NewBarcodeScanned = true;
                SondaFlag = true;
                WriteLog($"SCANNING SUCESSFULL: Selected Device - {SondaName}.");
            }
            else
            {
                Progress = false;
                BackgroundVisibility = true;
                BackgroundColor = Brushes.Red;
                HeaderColor = Brushes.Red;
                MessageBox = "piError".i18n();
                WriteLog($"SCANNING ERROR: Device aready scanned.\n {response.ErrorMessage}");
            }
        }

        private void OnOperation(string s, Response response)
        {
            if (String.IsNullOrWhiteSpace(OperationName) || NextStatus.Barcode.Equals(s))
            {
                OperationName = ((CmdGetInfoFromBarcodeResponse)response).Description;
                NewBarcodeScanned = true;
                OperationFlag = true;
                NextStatus = new Status() { Barcode = s };
                WriteLog($"SCANNING SUCESSFULL: Selected Operation - {OperationName}.");
            }
            else
            {
                timerTimeout.Stop();
                Progress = false;
                BackgroundVisibility = true;
                BackgroundColor = Brushes.Red;
                HeaderColor = Brushes.Red;
                MessageBox = "piBarcodeNotFound".i18n();
                timerTimeout.Start();
                WriteLog($"SCANNING ERROR: Operation aready scanned.\n {response.ErrorMessage}");
            }
        }

        #endregion

        private async Task<string> GetDeviceStatus(string barcode)
        {
            string ret = "";
            Response response = (Response)await PiHttpClient.Send(App.Settings.ServerHttpEndpoint,
                new CmdGetDeviceStatus() { DeviceBarcode = barcode });

            if (response is CmdGetDeviceStatusResponse && response.Successed)
                ret = ((CmdGetDeviceStatusResponse)response).Status;

            return ret;
        }

        private async Task ChangeDeviceStatus()
        {
            Progress = true;
            BackgroundVisibility = true;
            HeaderColor = Brushes.GreenYellow;
            MessageBox = "piUpdating".i18n();
            await Util.WaitAsync(1);

            await PiHttpClient.Send(App.Settings.ServerHttpEndpoint, new CmdPing() { Attachment = "ChangeDeviceStatus()" });
            Response response = (Response)await PiHttpClient.Send(App.Settings.ServerHttpEndpoint,
                new CmdSetDeviceStatus() { OperationBarcode = NextStatus.Barcode, DeviceBarcode = Device.Tag, UserBarcode = Operator.Tag, WorklistItemID = 0 });

            if (!response.Successed)
            {
                Progress = false;
                BackgroundColor = Brushes.Red;
                HeaderColor = Brushes.Red;
                MessageBox = "piNextStatusError".i18n();
                WriteLog($"UPDATING ERROR:\n {response.ErrorMessage} ");
                await Util.WaitAsync(3);
            }
            else
            {
                Progress = true;
                BackgroundColor = Brushes.Green;
                HeaderColor = Brushes.Green;
                MessageBox = "piUpdated".i18n();
                WriteLog("SUCCESS - UPDATED!");
                await Util.WaitAsync(3);
            }

            Reset();
        }

        private async Task ChangeAccessoryStatus()
        {
            Progress = true;
            BackgroundVisibility = true;
            HeaderColor = Brushes.GreenYellow;
            MessageBox = "piUpdating".i18n();
            await Util.WaitAsync(1);

            Response response = (Response)await PiHttpClient.Send(App.Settings.ServerHttpEndpoint,
                new CmdSetAccessoryStatus() { AccessoryBarcode = BarcodeAcessory, UserBarcode = Operator.Tag, WorklistItemID = 0 });

            if (!response.Successed)
            {
                Progress = false;
                BackgroundVisibility = true;
                BackgroundColor = Brushes.Red;
                HeaderColor = Brushes.Red;
                MessageBox = "piNextStatusError".i18n();
                WriteLog($"UPDATING ERROR:\n {response.ErrorMessage} ");
                await Util.WaitAsync(3);
            }
            else
            {
                Progress = true;
                BackgroundVisibility = true;
                BackgroundColor = Brushes.Green;
                HeaderColor = Brushes.Green;
                MessageBox = "piUpdated".i18n();
                WriteLog("SUCCESS - UPDATED!");
                await Util.WaitAsync(3);
            }

            Reset();
        }
    }
}
