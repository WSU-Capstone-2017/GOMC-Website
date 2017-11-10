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
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public byte[] HtmlZip { get; set; }
		public byte[] Pdf { get; set; }

		public ConversionResult Convert(string latexFileContent)
		{
			var fileDir = HttpContext.Current.Server.MapPath($"~/temp/upload_{Guid.NewGuid()}/");
			Directory.CreateDirectory(fileDir);

			var fileName = Path.Combine(fileDir, "Manual.tex");
			File.WriteAllText(fileName, latexFileContent);
			var result = ConvertAtDir(fileDir);

			Directory.Delete(fileDir, true);

			return result;
		}


		public ConversionResult ConvertAtDir(string fileDir)
		{
			Debug.Assert(fileDir != null);

			log.Info($"fileDir = {fileDir}");

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

				log.Info("running pandoc...");

				htmlProc.Start();
				htmlProc.WaitForExit();

				log.Info("pandoc done");

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

				log.Info("running pdflatex...");

				pdfProc.Start();
				pdfProc.WaitForExit();

				log.Info("pdflatex done");

				log.Info("running pdflatex again...");

				pdfProc.Start();
				pdfProc.WaitForExit();

				log.Info("pdflatex done");

				Pdf = File.ReadAllBytes(Path.Combine(fileDir, "Manual.pdf"));
			}
			catch(Exception ex)
			{
				log.Error(ex.ToString(), ex);
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