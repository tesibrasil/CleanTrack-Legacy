using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("96C0868A-85A3-326C-9EB7-EF3674F26B4F")]
    public interface IRFIDEvent
	{
		void DeviceDetected(string desc, long id);
		void UserDetected(string nome, string cognome, long id);
		void BadgeDetected(string id);
		void Completed(long success);
	}
}
