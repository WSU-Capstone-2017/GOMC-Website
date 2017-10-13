using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Project.ConfigInput
{
	[TestClass]
	public class ConfigInputModelTests
	{
		// used to return the folder path that this source file belongs to.
		private static string GetCallerFolderPath([CallerFilePath] string srcFilePath = "")
		{
			return Path.GetDirectoryName(srcFilePath);
		}

		[TestMethod]
		public void FromJsonTest()
		{
			var jsonString = File.ReadAllText(Path.Combine(GetCallerFolderPath(),
				"../../Documentation/Samples/gcmc_hexane_run2a_bridge.in.json"));

			var model = ConfigInputModel.FromJson(jsonString);

			Assert.IsNotNull(model);
		}
	}
}
