using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KleanTrak.Core;
using System.Text;

namespace PreWasherMDGTest
{
	[TestClass]
	public class MethodTester
	{
		PWPCantelMDG mdgwasher = new PWPCantelMDG("1234");
		[TestMethod]
		public void TestGetParameterValue()
		{
			var response = "VER=MDG.0.42.0;MAC=1;NAME=1A000001;ID=27;DATE=13/07/2020;TIME=13:04;OP=---;STR=---;LEAK=1;MODE=0;CHM=52;WT=11;RNSC=1;RNST=30;ERR=;PHA=1,6,2,3,7,6,3,7,8,;CHK=244C";
			var name = PWPCantelMDG.GetParameterValue("NAME", response);
			Assert.AreEqual(name, "1A000001");
			var rnsc = PWPCantelMDG.GetParameterValue("RNSC", response);
			Assert.AreEqual(rnsc, "1");
		}
		[TestMethod]
		public void TestGetOldResponse()
		{
			var oldresponse = "VER=MDG.0.42.0;MAC=1;NAME=1A000001;ID=27;DATE=13/07/2020;TIME=13:04;OP=---;STR=---;LEAK=1;MODE=0;CHM=52;WT=11;RNSC=1;RNST=30;ERR=;PHA=1,6,2,3,7,6,3,7,8,;CHK=244C".PadRight(238, ' ');
			var newreponse = "#^02540088exp." + oldresponse + "0000";
			if (!mdgwasher.ExtractInnerResponse(Encoding.ASCII.GetBytes(newreponse), out byte[] extractedresponse))
				throw new Exception("GetOldResponse return false");
			var strresponse = Encoding.ASCII.GetString(extractedresponse);
			var name = PWPCantelMDG.GetParameterValue("NAME", strresponse);
			Assert.AreEqual(name, "1A000001");
			var rnsc = PWPCantelMDG.GetParameterValue("RNSC", strresponse);
			Assert.AreEqual(rnsc, "1");
		}
		[TestMethod]
		public void TestRequestCommand()
		{
			var command = mdgwasher.GetRequestCommand("GETMEM");
			Assert.IsTrue(command.Length == 32);
		}
	}
}
