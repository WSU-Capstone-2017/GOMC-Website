using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.WebPages;
using HtmlAgilityPack;

namespace Project.Latex
{
	public class LatexConvertor
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public string LatexFile { get; set; }
		public byte[] HtmlZip { get; set; }
		public byte[] Pdf { get; set; }
		public ConversionResult Result { get; set; }

		public ConversionResult Convert(string latexFileContent)
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

			Result = ConvertAtDir(fileDir);

			Directory.Delete(fileDir, true);

			return Result;
		}

		private static string csviewCache;

		private string GetHtmlView(string content)
		{
			if(csviewCache.IsEmpty())
			{
				csviewCache = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Home/LatexHtml.cshtml"), Encoding.UTF8);
			}

			var hdoc = new HtmlDocument();
			hdoc.LoadHtml(content);

			var body = hdoc.DocumentNode.SelectNodes("//body")[0];

			var tocNav = hdoc.DocumentNode.SelectNodes("//nav")[0];
			body.RemoveChild(tocNav);

			tocNav.SelectNodes("ul")[0].Attributes.Add("class", "navspy-menu");
			tocNav.Attributes["id"].Value = "site-nav";

			foreach(var i in tocNav.SelectNodes("ul/descendant::ul"))
			{
				i.Attributes.Add("style", "display: block;");
			}
			foreach(var i in tocNav.SelectNodes("descendant::li"))
			{
				i.Attributes.Add("class", "menu-item");
			}
			foreach(var i in tocNav.SelectNodes("descendant::a"))
			{
				if(i.ParentNode.ChildNodes["ul"] != null)
				{
					var spn = hdoc.CreateElement("span");
					spn.Attributes.Add("class", "caret");
					i.AppendChild(spn);
				}
			}
			foreach(var i in tocNav.SelectNodes("descendant::a"))
			{

				var a = hdoc.CreateElement("a");
				a.Attributes.Add("href", i.Attributes["href"].Value);

				a.InnerHtml = i.InnerHtml;

				i.Attributes.Remove();
				i.Attributes.Add("class", "section-link");

				i.InnerHtml = "";

				i.Name = "div";

				i.AppendChild(a);
			}

			var div1 = hdoc.CreateElement("div");
			div1.Attributes.Add("class", "col-md-10");

			var div2 = hdoc.CreateElement("div");
			div2.Attributes.Add("id", "contents-container");
			div2.InnerHtml = body.InnerHtml;

			div1.AppendChild(div2);

			var div3 = hdoc.CreateElement("div");
			div3.Attributes.Add("class", "col-md-2");

			var div4 = hdoc.CreateElement("div");
			div4.Attributes.Add("id", "site-content");
			div4.AppendChild(tocNav);

			div3.AppendChild(div4);

			hdoc.DocumentNode.ChildNodes["html"].RemoveChild(body);

			var body2 = hdoc.CreateElement("body");
			body2.AppendChild(div3);
			body2.AppendChild(div1);

			hdoc.DocumentNode.ChildNodes["html"].AppendChild(body2);

			var imgs = body2.SelectNodes("//img").ToArray();

			foreach(var i in imgs)
			{
				var attr = i.Attributes["src"];
				attr.Value = attr.Value.Replace("images/", "~/content/latex/images/");
			}

			return csviewCache
				.Replace("@Html.Raw(ViewBag.HtmlContent)", body2.InnerHtml)
				.Replace("Plugin homepage @", "Plugin homepage @@");
		}

		public ConversionResult ConvertAtDir(string fileDir)
		{
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

				using(var zip = ZipFile.Open(Path.Combine(fileDir, "html.zip"), ZipArchiveMode.Create, Encoding.UTF8))
				{
					var pandocOut = File.ReadAllText(Path.Combine(fileDir, "pandoc.out"), Encoding.UTF8);
					zip.CreateEntryFromFile(Path.Combine(fileDir, "pandoc.out"), "Manual.html");
					File.WriteAllText(Path.Combine(fileDir, "manual_view.cshtml"), GetHtmlView(pandocOut), Encoding.UTF8);
					zip.CreateEntryFromFile(Path.Combine(fileDir, "manual_view.cshtml"), "manual_view.cshtml");
					foreach(var i in new DirectoryInfo(Path.Combine(fileDir, "images")).GetFiles())
					{
						zip.CreateEntryFromFile(i.FullName, "images/" + i.Name);
					}
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