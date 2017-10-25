using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public void FromInConfTest()
		{
			var inConf = File.ReadAllText(Path.Combine(GetCallerFolderPath(),
				"../../Other/gcmc_hexane_run2a_bridge.in.conf"));

			var model = ConfigInputModel.FromInConfFile(inConf);

			Assert.IsNotNull(model);
		}

		[TestMethod]
		public void FromFormDataTest()
		{
			var formData = File.ReadAllLines(Path.Combine(GetCallerFolderPath(), "../../Other/tests_ConfigInputModelTests_FromFormDataTest.txt"))
				.Select(j => j.Split('=')).ToDictionary(j => $"gomc_config_input_{j[0]}", j => j[1]);

			var formConv = new ConfigFormDataConvertor(formData);
			var model = formConv.Convert();

			Assert.IsNotNull(model);
		}

		[TestMethod]
		public void FromJsonTest()
		{
			var jsonString = File.ReadAllText(Path.Combine(GetCallerFolderPath(),
				"../../Other/gcmc_hexane_run2a_bridge.in.json"));

			var model = ConfigInputModel.FromJson(jsonString);

			Assert.IsNotNull(model);
		}
	}
}
