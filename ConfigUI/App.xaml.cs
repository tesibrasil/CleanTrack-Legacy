using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ConfigUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Window wnd = null;
            if (e.Args.Length == 2 && e.Args[0] == "-s")
            {
                server = true;
                iniFilePath = e.Args[1];
                wnd = new MainWindow();
            }
            else if (e.Args.Length == 2 && e.Args[0] == "-c")
            {
                client = true;
                iniFilePath = e.Args[1];
                wnd = new MainWindow();
            }
            else
                wnd = new VoidWindow();
            MainWindow = wnd;
            MainWindow.Show();
        }

        internal static bool server = false;
        internal static bool client = false;
        internal static string iniFilePath = null;
    }
}
