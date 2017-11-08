using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Project.Latex
{
	[TestClass]
	public class LatetxContertorTests
	{
		[TestMethod]
		public void ConvertNotNull()
		{
			const string dir = @"\dev\gomc\gomc_manual\";

			var latex = File.ReadAllText(Path.Combine(dir, "Manual.tex"), Encoding.UTF8);

			var conv = new LatexConvertor();
			conv.Convert2(dir);

			Assert.IsNotNull(conv.HtmlZip);
			Assert.IsNotNull(conv.Pdf);
		}
	}
}
