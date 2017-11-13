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

		public ConversionResult Convert(string latexFileContent, bool htlatex = true, bool pdflatex = true)
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

			File.WriteAllText(fileName, latexFileContent);

			Result = ConvertAtDir(fileDir, htlatex, pdflatex);

			Directory.Delete(fileDir, true);

			return Result;
		}


		public ConversionResult ConvertAtDir(string fileDir, bool htlatex = false, bool pdflatex = true)
		{
			HtmlMap.Clear();

			if(!htlatex && !pdflatex)
			{
				return Result = ConversionResult.Invalid;
			}

			Debug.Assert(fileDir != null);

			log.Info($"fileDir = {fileDir}");

			var fileName = Path.Combine(fileDir, "Manual.tex").Replace("\\", "/");
			var latexFileContent = File.ReadAllText(fileName);

			LatexFile = latexFileContent;

			// the current version of the latex file has images referenced with out specifying the extension
			// htlatex is sensitive about that and so we must provide the extension. all the extensions are
			// .png except images/website which is .jpg

			var content = Regex.Replace(latexFileContent, "{images/(\\w+)}", "{images/$1.png}")
				.Replace("{images/website.png}", "{images/website.jpg}");

			File.WriteAllText(fileName, content, Encoding.UTF8);

			try
			{
				if (htlatex)
				{
					log.Info("running htlatex...");

					var htmlProc = new Process
					{
						StartInfo = new ProcessStartInfo("htlatex", "Manual.tex")
						{
							WorkingDirectory = fileDir,
						}
					};


					htmlProc.Start();
					Thread.Sleep(1000);

					var hndl = htmlProc.MainWindowHandle;
					if (hndl == IntPtr.Zero)
					{
						throw new ApplicationException($"{nameof(htmlProc)}.{nameof(htmlProc.MainWindowHandle)} == 0");
					}

					while (!htmlProc.HasExited)
					{
						// htlatex will request user input when there is latex errors,
						// we will press enter each second to simulate user input
						// TODO: find an alternative way to run htlatex without using the PostMessage hack
						if (!PostMessage(htmlProc.MainWindowHandle, 0x100, 0x0D, 0))
						{
							throw new Win32Exception(Marshal.GetLastWin32Error());
						}
						Thread.Sleep(1000);
					}

					log.Info("htlatex done");
					using (var zip = ZipFile.Open(Path.Combine(fileDir, "html.zip"), ZipArchiveMode.Create))
					{
						HtmlMap.Add("Manual.html", File.ReadAllText(Path.Combine(fileDir, "Manual.Html"), Encoding.UTF8));
						HtmlMap.Add("Manual.css", File.ReadAllText(Path.Combine(fileDir, "Manual.css"), Encoding.UTF8));
						zip.CreateEntryFromFile(Path.Combine(fileDir, "Manual.Html"), "Manual.html");
						zip.CreateEntryFromFile(Path.Combine(fileDir, "Manual.css"), "Manual.html");
						foreach (var i in new DirectoryInfo(fileDir).GetFiles("Manual*x.png"))
						{
							HtmlMap.Add(i.Name, File.ReadAllText(i.FullName, Encoding.UTF8));
							zip.CreateEntryFromFile(i.FullName, i.Name);
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