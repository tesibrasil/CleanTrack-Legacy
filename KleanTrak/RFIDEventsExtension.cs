using amrfidmgrex;
using System;

namespace KleanTrak
{
	public class RFIDEventsExtension : IRFIDEvent
	{
		public delegate void BadgeDetectedHandlerCTRK(string id);
		public event BadgeDetectedHandlerCTRK BadgeDetectedCTRK;

		#region IRFIDEvents Members

		void IRFIDEvent.BadgeDetected(string id)
		{
			BadgeDetectedCTRK(id);
		}

		void IRFIDEvent.Completed(long success)
		{
			throw new NotImplementedException();
		}

		void IRFIDEvent.DeviceDetected(string desc, long id)
		{
			throw new NotImplementedException();
		}

		void IRFIDEvent.UserDetected(string nome, string cognome, long id)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
