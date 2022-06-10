using System;
using System.IO;
using KleanTrak.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EndoDryTest
{
	[TestClass]
	public class XmLoaderTest
	{
		[TestMethod]
		public void LoadINFile()
		{
			FileStream fstream = null;
			try
			{
				var directoryinfo = new DirectoryInfo("../../TestFiles/IN/");
				var endodryparser = new SPCantelEndoDry();
				foreach (FileInfo f in directoryinfo.GetFiles("*.xml"))
				{
					fstream = File.OpenRead(f.FullName);
					var his = (SPCantelEndoDry.HIS)endodryparser.serializer.Deserialize(fstream);
					fstream.Close();
					Assert.IsTrue(his.IN != null);
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (fstream != null)
					fstream.Close();
			}
		}
		[TestMethod]
		public void LoadOUTFile()
		{
			FileStream fstream = null;
			try
			{
				var directoryinfo = new DirectoryInfo("../../TestFiles/OUT/");
				var endodryparser = new SPCantelEndoDry();
				foreach (FileInfo f in directoryinfo.GetFiles("*.xml"))
				{
					fstream = File.OpenRead(f.FullName);
					var his = (SPCantelEndoDry.HIS)endodryparser.serializer.Deserialize(fstream);
					fstream.Close();
					Assert.IsTrue(his.OUT != null);
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (fstream != null)
					fstream.Close();
			}
		}
	}
}
