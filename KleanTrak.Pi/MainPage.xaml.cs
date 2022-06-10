using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using KleanTrak.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace KleanTrak.Pi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        HttpServer httpServer = new HttpServer(8090);
        KeyboardHelper keyboardHelper = new KeyboardHelper();

        CancellationTokenSource tokenCountdown = null;
        DispatcherTimer timerCountdown = new DispatcherTimer();

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        Task taskStatus;

        bool serverConnected = false;
        bool popupOpened = false;

        public MainPage()
        {
            this.InitializeComponent();
#if X86
            this.Width = 800;
            this.Height = 480;
#endif
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfiguration();
            LoadDictionary();

            timerCountdown.Tick += TimerCountdown_Tick;
            timerCountdown.Interval = new TimeSpan(0, 0, 1);

            httpServer.RequestReceived += RequestReceived;
            keyboardHelper.KeyDown += KeyDownReceived;

            gridPrincipal.RowDefinitions[2].Height = new GridLength(0);
            gridBody.Visibility = Visibility.Collapsed;

            var token = tokenSource.Token;
            
            taskStatus = new Task(async () => 
                {
                    while (!token.IsCancellationRequested)
                    {
                        VerifyServerStatus();
                        await Task.Delay(10000);
                    }
                }, token);

            taskStatus.Start();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            taskStatus.Wait();
        }

        private void Startup()
        {
            button11.Visibility = Visibility.Collapsed;
            button22.Visibility = Visibility.Collapsed;
            button33.Visibility = Visibility.Collapsed;

            bool? bWorklist = ((App)App.Current).Configuration.WorklistSelection;
            if (bWorklist.HasValue && bWorklist.Value)
            {
                PageWorklist page = new PageWorklist();
                page.ConfirmPressed += PageWorklist_ConfirmPressed;
                page.OpenPopup += OpenPopup;
                gridBody.Child = page;

                txtTitle.Visibility = Visibility.Collapsed;
                txtTitleAlternative.Visibility = Visibility.Visible;
                txtTitleRow1.Text = ((App)App.Current).Dictionary["piWorklistTitle"];
                txtTitleRow2.Text = ((App)App.Current).Dictionary["piSelectWorklistItem"];
            }
            else
            {
                PagePrincipal page = new PagePrincipal();
                page.OpenPopup += OpenPopup;
                page.AbortOperation += AbortOperation;
                page.OpenWaiter += ShowWaiter;
                gridBody.Child = page;

                txtTitle.Visibility = Visibility.Visible;
                txtTitleAlternative.Visibility = Visibility.Collapsed;

                //button11.Visibility = Visibility.Visible;
            }

            GridLength length = new GridLength(bWorklist.HasValue && bWorklist.Value ? 0 : 0.22, GridUnitType.Star);
            gridPrincipal.RowDefinitions[2].Height = length;
        }

        private void PageWorklist_ConfirmPressed(WorklistItem obj)
        {
            PagePrincipal page = new PagePrincipal();
            page.CurrentWorklistItem = obj;
            page.OpenPopup += OpenPopup;
            page.AbortOperation += AbortOperation;
            page.OpenWaiter += ShowWaiter;
            gridBody.Child = page;

            txtTitleRow1.Text = obj.Patient;
            if (obj.BirthDate.HasValue)
                txtTitleRow1.Text += " " + obj.BirthDate.Value.ToString("dd/MM/yyyy");

            txtTitleRow2.Text = obj.Description;

            gridPrincipal.RowDefinitions[2].Height = new GridLength(0.22, GridUnitType.Star);

            button33.Visibility = Visibility.Visible;
        }

        public void KeyDownReceived(KeyboardEventArgs k)
        {
            if (!serverConnected)
                return;

            if (popupOpened || gridAbort.Visibility == Visibility.Visible || waiter.IsActive)
            {
                Buzzer.Instance.ErrorMessage();
                return;
            }
            
            if (gridBody.Child != null)
                ((IPage)gridBody.Child).KeyDownReceived(k);
        }

        private async Task<Response> RequestReceived(Model.Request req)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                switch (req.GetType().ToString())
                {
                    case "KleanTrak.Model.CmdSetPiConfiguration":
                        ((App)App.Current).Configuration = ((CmdSetPiConfiguration)req).Configuration;
                        SaveConfiguration();
                        VerifyServerStatus();
                        if (serverConnected)
                            Startup();
                        break;

                    case "KleanTrak.Model.CmdSetDictionary":
                        ((App)App.Current).Dictionary = ((CmdSetDictionary)req).Dictionary;
                        SaveDictionary();
                        break;
                }
            });

            return new Response() { Successed = true };
        }

        private async void LoadConfiguration()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile configurationFile = null;

            try
            {
                configurationFile = await storageFolder.GetFileAsync("KleanTrak.Pi.cfg");
                string text = await Windows.Storage.FileIO.ReadTextAsync(configurationFile);
                if (text != null && text != "")
                    ((App)App.Current).Configuration = (PiConfiguration)PiConfiguration.ReadObjectFromXml(text);
            }
            catch (Exception)
            {
            }

            VerifyServerStatus();
        }

        private async void SaveConfiguration()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile configurationFile = null;

            try
            {
                configurationFile = await storageFolder.GetFileAsync("KleanTrak.Pi.cfg");
            }
            catch (Exception)
            {
                configurationFile = await storageFolder.CreateFileAsync("KleanTrak.Pi.cfg");
            }

            string configSer = ((App)App.Current).Configuration.SaveObjectToXml();
            await Windows.Storage.FileIO.WriteTextAsync(configurationFile, configSer);
        }

        private async void LoadDictionary()
        {
            /*
            DictionaryBase dictionary = new DictionaryBase();
            dictionary.UpdateWithDefaultValues();
            ((App)App.Current).Dictionary = dictionary;
            return;*/
            
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile dictionaryFile = null;

            try
            {
                dictionaryFile = await storageFolder.GetFileAsync("Dictionary.xml");
                string text = await Windows.Storage.FileIO.ReadTextAsync(dictionaryFile);
                if (text != null && text != "")
                {
                    DictionaryBase dictionary = (DictionaryBase)DictionaryBase.ReadObjectFromXml(text);
                    dictionary.UpdateWithDefaultValues();
                    ((App)App.Current).Dictionary = dictionary;
                }
            }
            catch (Exception)
            {
            }

            SaveDictionary();
        }

        private async void SaveDictionary()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile dictionaryFile = null;

            try
            {
                dictionaryFile = await storageFolder.GetFileAsync("Dictionary.xml");
            }
            catch (Exception)
            {
                dictionaryFile = await storageFolder.CreateFileAsync("Dictionary.xml");
            }

            string text = ((App)App.Current).Dictionary.SaveObjectToXml();
            await Windows.Storage.FileIO.WriteTextAsync(dictionaryFile, text);
        }

        private async void VerifyServerStatus()
        {
            Response response = (Response)await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing());
            if (!response.Successed)
            {
                try
                {
                    Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    Windows.Storage.StorageFile logFile = null;

                    try
                    {
                        logFile = await storageFolder.GetFileAsync("VerifyServerStatus.log");
                    }
                    catch (Exception)
                    {
                        logFile = await storageFolder.CreateFileAsync("VerifyServerStatus.log");
                    }

                    await Windows.Storage.FileIO.WriteTextAsync(logFile, response.ErrorMessage);
                }
                catch
                {
                }
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (!response.Successed && (gridBody.Child == null || !(gridBody.Child is PageServerUnavailable)))
                {
                    gridBody.Child = new PageServerUnavailable();
                    txtTitle.Visibility = Visibility.Collapsed;
                    txtTitleAlternative.Visibility = Visibility.Collapsed;

                    button11.Visibility = Visibility.Collapsed;
                    button22.Visibility = Visibility.Collapsed;
                    button33.Visibility = Visibility.Collapsed;

                    gridPrincipal.RowDefinitions[2].Height = new GridLength(0);
                }
                else if (response.Successed && !serverConnected)
                    Startup();

                gridBody.Visibility = Visibility.Visible;
                serverConnected = response.Successed;
            });
        }

        private async void OpenPopup(string title, string message)
        {
            popupOpened = true;
            popupTitle.Text = title != null ? title : "";
            popupMessage.Text = message != null ? message : "";
            gridPopup.Visibility = Visibility.Visible;

            Buzzer.Instance.ErrorMessage();

            if (((App)App.Current).Configuration.DelayBeforeClosingPopup > 0)
            {
                //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "OpenPopup() 1" });
                await AbortOperation(((App)App.Current).Configuration.DelayBeforeClosingPopup);
                //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "OpenPopup() 2" });

                gridPopup.Visibility = Visibility.Collapsed;
                popupOpened = false;
            }
        }

        private void gridPopup_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "gridPopup_PointerPressed()" });
            if (tokenCountdown != null)
                tokenCountdown.Cancel();
            gridPopup.Visibility = Visibility.Collapsed;
            popupOpened = false;
        }

        private void button1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (button11.Visibility == Visibility.Collapsed)
                return;

            button11.Visibility = Visibility.Collapsed;
            //button22.Visibility = Visibility.Visible;
            button33.Visibility = Visibility.Collapsed;

            PageDeviceStatus page = new PageDeviceStatus();
            page.OpenPopup += OpenPopup;
            gridBody.Child = page;
        }

        private void button2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (button22.Visibility == Visibility.Collapsed)
                return;

            //button11.Visibility = Visibility.Visible;
            button22.Visibility = Visibility.Collapsed;
            button33.Visibility = Visibility.Collapsed;

            PagePrincipal page = new PagePrincipal();
            page.OpenPopup += OpenPopup;
            page.AbortOperation += AbortOperation;
            page.OpenWaiter += ShowWaiter;
            gridBody.Child = page;
        }

        private void button3_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (button33.Visibility == Visibility.Collapsed)
                return;

            button11.Visibility = Visibility.Collapsed;
            button22.Visibility = Visibility.Collapsed;
            button33.Visibility = Visibility.Collapsed;

            Startup();
        }

        private async Task<bool> AbortOperation(int? delay)
        {
            if (delay == null)
                delay = 5;

            tokenCountdown = new CancellationTokenSource();

            int countDownSec = delay.Value;

            Visibility a = button11.Visibility;
            Visibility b = button22.Visibility;
            Visibility c = button33.Visibility;
            button11.Visibility = Visibility.Collapsed;
            button22.Visibility = Visibility.Collapsed;
            button33.Visibility = Visibility.Collapsed;
            gridAbort.Visibility = Visibility.Visible;

            txtCountdown.Text = countDownSec.ToString();

            timerCountdown.Start();
            bool ret = false;

            try
            {
                //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "AbortOperation 1" });
                await Task.Delay(countDownSec * 1000, tokenCountdown.Token);
                //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "AbortOperation 2" });
            }
            catch (TaskCanceledException)
            {
                //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "AbortOperation 3" });
                ret = true;
            }

            timerCountdown.Stop();

            gridAbort.Visibility = Visibility.Collapsed;
            button11.Visibility = a;
            button22.Visibility = b;
            button33.Visibility = c;

            tokenCountdown.Dispose();
            tokenCountdown = null;
            return ret;
        }

        private void TimerCountdown_Tick(object sender, object e)
        {
            txtCountdown.Text = (Convert.ToInt32(txtCountdown.Text) - 1).ToString();
        }

        private void gridAbort_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (gridAbort.Visibility == Visibility.Collapsed)
                return;

            //await HttpClient.Send(((App)App.Current).Configuration.ServerHttpEndpoint, new CmdPing() { Attachment = "gridAbort_PointerPressed 1" });
            if (tokenCountdown != null)
                tokenCountdown.Cancel();
        }

        Visibility button1VisibilityBeforeShowingWaiter;
        Visibility button2VisibilityBeforeShowingWaiter;
        Visibility button3VisibilityBeforeShowingWaiter;

        private void ShowWaiter(bool show)
        {
            if (show)
            {
                button1VisibilityBeforeShowingWaiter = button11.Visibility;
                button2VisibilityBeforeShowingWaiter = button22.Visibility;
                button3VisibilityBeforeShowingWaiter = button33.Visibility;
                button11.Visibility = Visibility.Collapsed;
                button22.Visibility = Visibility.Collapsed;
                button33.Visibility = Visibility.Collapsed;
            }
            else
            {
                button11.Visibility = button1VisibilityBeforeShowingWaiter;
                button22.Visibility = button2VisibilityBeforeShowingWaiter;
                button33.Visibility = button3VisibilityBeforeShowingWaiter;
            }

            waiter.IsActive = show;
        }
    }
}
