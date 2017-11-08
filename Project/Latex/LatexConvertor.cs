using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Project.Latex
{
	public class LatexConvertor
	{
		public byte[] HtmlZip { get; set; }
		public byte[] Pdf { get; set; }

		public ConversionResult Convert(string latexFileContent)
		{
			var fileDir = HttpContext.Current.Server.MapPath($"~/temp/upload_{Guid.NewGuid()}/");
			var fileName = Path.Combine(fileDir, "Manual.tex");
			File.WriteAllText(fileName, latexFileContent);
			var result = ConvertAtDir(fileDir);

			Directory.Delete(fileDir, true);

			return result;
		}


		public ConversionResult ConvertAtDir(string fileDir)
		{
			Debug.Assert(fileDir != null);
			var fileName = Path.Combine(fileDir, "Manual.tex").Replace("\\", "/");
			var latexFileContent = File.ReadAllText(fileName);
			// the current version of the latex file has images referenced with out specifying the extension
			// htlatex is sensitive about that and so we must provide the extension. all the extensions are
			// .png except images/website which is .jpg

			var content = Regex.Replace(latexFileContent, "{images/(\\w+)}", "{images/$1.png}")
				.Replace("{images/website.png}", "{images/website.jpg}");

			File.WriteAllText(fileName, content, Encoding.UTF8);

			try
			{

				var htmlProc = new Process
				{
					StartInfo = new ProcessStartInfo("pandoc", "-o Manual.html Manual.tex")
					{
						WorkingDirectory = fileDir,
						WindowStyle = ProcessWindowStyle.Hidden
					}
				};

				htmlProc.Start();
				htmlProc.WaitForExit();

				using(var zip = ZipFile.Open(Path.Combine(fileDir, "html.zip"), ZipArchiveMode.Create))
				{
					zip.CreateEntryFromFile(Path.Combine(fileDir, "Manual.Html"), "Manual.html");
				}

				HtmlZip = File.ReadAllBytes(Path.Combine(fileDir, "html.zip"));

				var pdfProc = new Process
				{
					StartInfo = new ProcessStartInfo("pdflatex",
						$"--shell-escape --interaction=nonstopmode {fileName} > pdflatex.log.txt")
					{
						WorkingDirectory = fileDir,
						WindowStyle = ProcessWindowStyle.Hidden
					}
				};

				pdfProc.Start();
				pdfProc.WaitForExit();

				pdfProc.Start();
				pdfProc.WaitForExit();

				Pdf = File.ReadAllBytes(Path.Combine(fileDir, "Manual.pdf"));
			}
			catch(Exception ex)
			{
				// ideally we would log the exception, but as of now, this is not part of the requirements
				return ConversionResult.Invalid;
			}

			return ConversionResult.Success;
		}
	}

	public enum ConversionResult
	{
		Success,
		Invalid
	}
}