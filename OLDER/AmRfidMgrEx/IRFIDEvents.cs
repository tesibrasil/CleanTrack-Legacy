using System.Runtime.InteropServices;

namespace amrfidmgrex
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComVisible(true)]
  public interface IRFIDEvents
  {
    void DeviceDetected(string desc, long id);

    void UserDetected(string nome, string cognome, long id);

    void BadgeDetected(string id);

    void Completed(long success);
  }
}
