using System;
using amrfidmgrex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmRfidTest
{
	[TestClass]
	public class AmRfidTest
	{
		[TestMethod]
		public void InstanceCreation()
		{
			RFIDManager man = new RFIDManager();
			man.setConnectionString("Driver={SQL Server};Server=tesrv24,1433;Database=CLEANTRACK;Uid=sa;Pwd=Nautilus2019; ");
			var name = man.getUserName(1);
		}
	}
}
