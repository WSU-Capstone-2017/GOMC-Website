using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Project.Latex
{
	public class LatexConvertor
	{
		public byte[] HtmlZip { get; set; }
		public string Pdf { get; set; }

		public ConversionResult Convert(string latexFileContent)
		{
			var fileDir = $"temp/upload_{DateTime.Now.Ticks}/";
			var fileName = Path.Combine(fileDir, "latex.tex");

			// the current version of the latex file has images referenced with out specifying the extension
			// htlatex is sensitive about that and so we must provide the extension. all the extensions are
			// .png except images/website which is .jpg

			var content = Regex.Replace(latexFileContent, "{images/(\\w+)}", "{images/$1.png}")
				.Replace("{images/website.png}", "{images/website.jpg}");

			File.WriteAllText(fileName, latexFileContent);

			var proc = new Process
			{
				StartInfo = new ProcessStartInfo("htlatex", fileName)
			};

			try
			{
				proc.Start();
			}
			catch
			{
				// ideally we would log the exception, but as of now, this is not part of the requirements
				return ConversionResult.Invalid;
			}

			// TODO: zip contents to file and extract bytes then set to HtmlZip
			File.Delete(fileName);
			return ConversionResult.Success;
		}
	}

	public enum ConversionResult
	{
		Success,
		Invalid
	}
}