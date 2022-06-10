using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KleanTrak.Core;

namespace SteelcoTest
{
	[TestClass]
	public class SteelcoParserTest
	{
		[TestMethod]
		public void ParseFile()
		{
			DbConnection.ConnectionString = "Driver={SQL Server};Server=10.171.1.24,1433;Database=CLEANTRACK;Uid=sa;Pwd=Nautilus2019;";
			var parser = new WPSteelco(false);
			parser.FolderOrFileName = "./testfiles";
			foreach (var path in parser.CheckDirectory("16066027", DateTime.MinValue))
			{
				var cycle = parser.LoadInfos(path);
			}
		}
	}
}
