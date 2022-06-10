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
using System.Threading;
using System.Threading.Tasks;

namespace KleanTrak.Pi
{
    public delegate void OpenPopupDelegate(string title, string message);

    public delegate Task<bool> WaitForAbortOperationDelegate(int? delay);

    public interface IPage
    {
        void KeyDownReceived(KeyboardEventArgs k);

        event OpenPopupDelegate OpenPopup;
    }
}
