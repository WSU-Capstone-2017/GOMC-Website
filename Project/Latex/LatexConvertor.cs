using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Project.Latex
{
	public class LatexConvertor
	{
		public byte[] HtmlZip { get; set; }
		public byte[] Pdf { get; set; }

		public ConversionResult Convert(string latexFileContent)
		{
			var fileDir = $"temp/upload_{Guid.NewGuid()}/";
			var fileName = Path.Combine(fileDir, "Manual.tex");

			// the current version of the latex file has images referenced with out specifying the extension
			// htlatex is sensitive about that and so we must provide the extension. all the extensions are
			// .png except images/website which is .jpg

			var content = Regex.Replace(latexFileContent, "{images/(\\w+)}", "{images/$1.png}")
				.Replace("{images/website.png}", "{images/website.jpg}");

			File.WriteAllText(fileName, content, Encoding.UTF8);


			try
			{
				var proc = new Process
				{
					StartInfo = new ProcessStartInfo("htlatex", fileName)
				};

				proc.Start();

				proc.WaitForExit();

				using(var zip = ZipFile.Open(Path.Combine(fileDir, "html.zip"), ZipArchiveMode.Create))
				{
					zip.CreateEntryFromFile(Path.Combine(fileDir, "Manual.Html"), "Manual.html");
					zip.CreateEntryFromFile(Path.Combine(fileDir, "Manual.css"), "Manual.css");
					zip.CreateEntryFromFile(Path.Combine(fileDir, "Manual.Html"), "Manual.html");
					foreach(var p in new DirectoryInfo(fileDir).GetFiles("Manual.*.png"))
					{
						zip.CreateEntryFromFile(p.FullName, p.Name);
					}
				}

				HtmlZip = File.ReadAllBytes(Path.Combine(fileDir, "html.zip"));

				var proc2 = new Process
				{
					StartInfo = new ProcessStartInfo("pdflatex", fileName)
				};

				proc2.Start();

				proc2.WaitForExit();

				Pdf = File.ReadAllBytes(Path.Combine(fileDir, "Manual.pdf"));
			}
			catch(Exception ex)
			{
				// ideally we would log the exception, but as of now, this is not part of the requirements
				return ConversionResult.Invalid;
			}
			finally
			{
				// in all cases delete that new dir we created
				Directory.Delete(fileDir, true);
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