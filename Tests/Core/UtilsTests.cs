using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Controllers;

namespace Project.Core
{
	[TestClass]
	public class UtilsTests
	{
		[TestMethod]
		public void AsUriEqualsTest()
		{
			const string tst = "http://google.com";

			Assert.AreEqual(new Uri(tst, UriKind.Absolute), Utils.AsUri(tst));
		}

		[TestMethod]
		public void AsUriNullTest()
		{
			Assert.IsNull(Utils.AsUri(null));
		}

		[TestMethod]
		[ExpectedException(typeof(UriFormatException))]
		public void AsUriInvalidThrows()
		{
			Utils.AsUri("_");
		}

	}
}