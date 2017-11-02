using System;

namespace Project.Latex
{
	public class LatexConvertor
	{
		public string Html { get; set; }
		public string Pdf { get; set; }

		public ConversionResult Convert(string latexFileContent)
		{
			throw new NotImplementedException();
		}
	}

	public enum ConversionResult
	{
		Success,
		Invalid
	}
}