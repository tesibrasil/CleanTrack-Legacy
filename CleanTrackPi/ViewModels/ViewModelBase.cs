using CleanTrackPi.Helpers;
using ReactiveUI;

namespace CleanTrackPi.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public void WriteLog(string content) => Util.WriteLog(content);
    }
}
