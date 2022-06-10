using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Linq;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using CleanTrackPi.Helpers;

namespace CleanTrackPi
{
    class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            var builder = BuildAvaloniaApp();
            if (args.Contains("--drm"))
            {
                SilenceConsole();
                return TryStartDrmOn(builder, args);
            }

            return builder.StartWithClassicDesktopLifetime(args);
        }

        private static int TryStartDrmOn(AppBuilder builder, string[] args)
        {
            try
            {
                Console.WriteLine("Try run on /dev/dri/card1");
                return builder.StartLinuxDrm(args, card: "/dev/dri/card1");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                Console.WriteLine("Try run on /dev/dri/card0");
                return builder.StartLinuxDrm(args, card: "/dev/dri/card0");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return -1;
        }

        private static void SilenceConsole()
        {
            new Thread(() =>
            {
                Console.CursorVisible = false;
                while (true) {
                    Listener.SendToken(ReadToken());
                }
                    
            })
            { IsBackground = true }.Start();
        }

        private static string ReadToken()
        {
            var code = String.Empty;
            ConsoleKeyInfo k;

            try
            {
                do
                {
                    k = Console.ReadKey();
                    code += k.KeyChar;
                }
                while (k.Key != ConsoleKey.Enter);
            }
            catch (Exception exc) { }
            Util.WriteLog("ReadToken" + code);
            return code;
        }

        public static AppBuilder BuildAvaloniaApp()
                => AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .LogToTrace()
                    .UseReactiveUI();
    }
}
