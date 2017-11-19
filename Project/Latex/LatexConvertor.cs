using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Project.Latex
{
	public class LatexConvertor
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		[DllImport("User32.Dll", EntryPoint = "PostMessageA")]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

		public Dictionary<string, string> HtmlMap { get; set; } = new Dictionary<string, string>();
		public string LatexFile { get; set; }
		public byte[] HtmlZip { get; set; }
		public byte[] Pdf { get; set; }
		public ConversionResult Result { get; set; }

		public ConversionResult Convert(string latexFileContent, bool pandoc = true, bool pdflatex = true)
		{
			var fileDir = HttpContext.Current.Server.MapPath($"~/temp/upload_{Guid.NewGuid()}/");
			Directory.CreateDirectory(fileDir);

			var imgDir = HttpContext.Current.Server.MapPath("~/Latex/images");

			foreach (var p in Directory.GetFiles(imgDir, "*.*", SearchOption.AllDirectories))
			{
				var p2 = p.Replace(imgDir, Path.Combine(fileDir, "images"));

				var p2Dir = Path.GetDirectoryName(p2);

				Debug.Assert(p2Dir != null);

				if (!Directory.Exists(p2Dir))
				{
					Directory.CreateDirectory(p2Dir);
				}

				File.Copy(p, p2, true);
			}

			var fileName = Path.Combine(fileDir, "Manual.tex");

			File.WriteAllText(fileName, latexFileContent, Encoding.Default);

			Result = ConvertAtDir(fileDir, pandoc, pdflatex);

			Directory.Delete(fileDir, true);

			return Result;
		}


		public ConversionResult ConvertAtDir(string fileDir, bool pandoc = false, bool pdflatex = true)
		{
			HtmlMap.Clear();

			if(!pandoc && !pdflatex)
			{
				return Result = ConversionResult.Invalid;
			}

			Debug.Assert(fileDir != null);

			log.Info($"fileDir = {fileDir}");

			var fileName = Path.Combine(fileDir, "Manual.tex").Replace("\\", "/");
			var latexFileContent = File.ReadAllText(fileName, Encoding.Default);

			LatexFile = latexFileContent;

			// the current version of the latex file has images referenced with out specifying the extension
			// all the extensions are
			// png except images/website which is jpg

			var content = Regex.Replace(latexFileContent, "{images/(\\w+)}", "{images/$1.png}")
				.Replace("{images/website.png}", "{images/website.jpg}")
				.Replace(" & ", " \\& ");

			File.WriteAllText(fileName, content, Encoding.UTF8);

			try
			{
				if (pandoc)
				{
					log.Info("running pandoc...");

					var pandocProc = new Process
					{
						StartInfo = new ProcessStartInfo("pandoc", "--mathjax -s --toc -o pandoc.out Manual.tex")
						{
							WorkingDirectory = fileDir,
						}
					};

					pandocProc.Start();
					pandocProc.WaitForExit();

					log.Info("pandoc done");

					using (var zip = ZipFile.Open(Path.Combine(fileDir, "html.zip"), ZipArchiveMode.Create, Encoding.UTF8))
					{
						HtmlMap.Add("Manual.html", File.ReadAllText(Path.Combine(fileDir, "pandoc.out")));
						zip.CreateEntryFromFile(Path.Combine(fileDir, "pandoc.out"), "Manual.html");
						foreach(var i in new DirectoryInfo(Path.Combine(fileDir, "images")).GetFiles())
						{
							zip.CreateEntryFromFile(i.FullName, "images/" + i.Name);
						}
					}

					HtmlZip = File.ReadAllBytes(Path.Combine(fileDir, "html.zip"));
				}

				if(pdflatex)
				{
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
			}
			catch (Exception ex)
			{
				log.Error(ex.ToString(), ex);
				return Result = ConversionResult.Invalid;
			}

			return Result = ConversionResult.Success;
		}
	}

	public enum ConversionResult
	{
		Success,
		Invalid
	}
}