using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CleantrackPi.Webapi;
using CleanTrackPi.Helpers;
using CleanTrackPi.ViewModels;
using CleanTrackPi.Views;
using KleanTrak.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CleanTrackPi
{
    public static class i18nExtension
    {
        public static string i18n(this string key) => App.Translate(key);
    }

    public class App : Application
    {
        public static int ListenPort = 8090;
        
        public static PiConfiguration Settings = null;

        public static DictionaryBase dict = null;

        public static string DumpSettings() => Settings.ServerHttpEndpoint;

        public static bool DesktopMode = false;

        public static Task WebTask = null;

        public static string ConfigPath
        {
            get => Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar + "config.json";
        }

        public static string DictionaryPath
        {
            get => Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar + "dictionary.json";
        }

        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            SetViewMode();
            SetLanguage();
            LoadConfiguration();
            LoadDictionary();
            WebTask = WebServer.Run(ListenPort, Callback);
            MainWindowViewModel.Instance.Init();
            Util.WriteLog("CleanTrack Started!");
            base.OnFrameworkInitializationCompleted();
        }

        private void SetLanguage()
        {
            string lang = Environment.GetEnvironmentVariable("CLEANTRACK_LANG");            
            if ((lang == null) || (lang == String.Empty))
            {
                Util.WriteLog("Environment Variable CLEANTRACK_LANG not set, using default language");
                DictionaryBase.Language = DictionaryBase.Languages.ENG;
            }
            else
                DictionaryBase.Language = (DictionaryBase.Languages)Enum.Parse(typeof(DictionaryBase.Languages), lang);
        }

        private void SetViewMode()
        {
            var vm = new MainWindowViewModel();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var sv = new StartUpView();
                desktop.MainWindow = sv;
                DesktopMode = true;
                sv.Init(vm);
            }
            else
                if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                singleView.MainView = new MainWindow() {
                    DataContext = vm
                };
                DesktopMode = false;
            }
        }

        public bool Callback(string data)
        {
            try
            {
                Settings = ((CmdSetPiConfiguration)PiConfiguration.ReadObjectFromXml(data)).Configuration;
                if (Settings != null)
                {
                    SaveConfiguration(Settings);
                    return true;
                }                    
            }
            catch (Exception exc)
            {
            }

            try
            {
                dict = ((CmdSetDictionary)DictionaryBase.ReadObjectFromXml(data)).Dictionary;
                if (Settings != null)
                {
                    SaveDictionary(dict);
                    MainWindowViewModel.Instance.Init();
                    return true;
                }
            }
            catch (Exception exc)
            {
            }

            return false;
        }

        public static string Translate(string key) {
            if (dict != null)
                return dict[key];

            return key;
        } 

        private static void LoadConfiguration()
        {
            try
            {
                Util.WriteLog("LoadConfiguration from " + ConfigPath);
                Settings = (PiConfiguration)PiConfiguration.ReadObjectFromXml(File.ReadAllText(ConfigPath));
            }
            catch (Exception exc)
            {
                Settings = new PiConfiguration() { ServerHttpEndpoint = "http://69.69.69.69:6969/AcceptMessage" };
                SaveConfiguration(Settings);
            }
        }

        private static void LoadDictionary()
        {
            try
            {
                Util.WriteLog("LoadDictionary from " + DictionaryPath);
                dict = (DictionaryBase)PiConfiguration.ReadObjectFromXml(File.ReadAllText(DictionaryPath));
            }
            catch (Exception exc)
            {
                dict = new DictionaryBase();
                SaveDictionary(dict);
            }
        }

        public static void SaveConfiguration(PiConfiguration conf) =>
            File.WriteAllText(ConfigPath, conf.SaveObjectToXml());

        public static void SaveDictionary(DictionaryBase dict) =>
            File.WriteAllText(DictionaryPath, dict.SaveObjectToXml());
    }
}
