using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Project.Latex
{
	[TestClass]
	public class LatetxContertorTests
	{
		private static string GetCallerFolderPath([CallerFilePath] string srcFilePath = "")
		{
			return Path.GetDirectoryName(srcFilePath);
		}

		private readonly string testDir;

		public LatetxContertorTests()
		{
			testDir = Path.Combine(GetCallerFolderPath(), "../bin/debug/", $"outs/test_{Guid.NewGuid()}");

			var srcPath = Path.Combine(GetCallerFolderPath(), "TestInput");


			foreach(var p in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
			{
				var p2 = p.Replace(srcPath, testDir);

				var p2Dir = Path.GetDirectoryName(p2);

				Debug.Assert(p2Dir != null);

				if(!Directory.Exists(p2Dir))
				{
					Directory.CreateDirectory(p2Dir);
				}

				File.Copy(p, p2, true);
			}
		}
		
		[TestCleanup]
		public void TearDown()
		{
			Directory.Delete(testDir, true);
		}

		[TestMethod]
		public void ConvertNotNull()
		{
			var conv = new LatexConvertor();
			var result = conv.ConvertAtDir(testDir);

			Assert.IsTrue(result == ConversionResult.Success);
			Assert.IsNotNull(conv.HtmlZip);
			Assert.IsNotNull(conv.Pdf);
		}

		
	}
}
