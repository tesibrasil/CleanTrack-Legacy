using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9CA39C82-1C76-3B2F-9D9A-9EF437D7A973")]
	public interface IRFIDCycleEx
	{
		string Description { get; set; }

		string WasherDescription { get; set; }

		[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)]
		RFIDCycleStep[] Steps { get; set; }
	}
}
