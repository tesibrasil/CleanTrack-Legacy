using System;
using System.IO;
using System.Threading;
using KleanTrak.Core;
using KleanTrak.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons;

namespace EndoDryTest
{
	[TestClass]
	public class CycleReconstructionTest
	{
		[TestMethod]
		public void ReconstructCycle()
		{
			var tmpdir = "c:/temp/endodrytest";
			DbConnection.ConnectionString = "Driver={SQL Server};Server=10.171.1.24,1433;Database=CLEANTRACK;Uid=sa;Pwd=Nautilus2019;";
			if (!Directory.Exists(tmpdir))
				Directory.CreateDirectory(tmpdir);
			foreach (var file in Directory.GetFiles("../../TestFiles/", "*.xml", SearchOption.AllDirectories))
			{
				try
				{
					File.Copy(file, tmpdir.ComposePathToFile(file.GetFileNameWithoutPath()));
				}
				catch (Exception)
				{ }
			}
			var parser = WPBase.Get(new Washer
			{
				Type = WasherStorageTypes.Storage_Cantel_EndoDry,
				FolderOrFileName = tmpdir
			}) ;
			var cycles = parser.GetCycles(new Washer { SerialNumber = "CM00439" }, DateTime.MinValue);
		}
	}
}
