using System;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Project.Core;
using Project.Data;
using Project.Latex;
using Project.Models;
using Project.LoginSystem;

namespace Project.Controllers
{
	public class LatexController : ApiController
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


		[HttpPost]
		public async Task<LatexConvertResult> Convert()
		{
			log.Info("/api/Latex/Convert");

			var sessionCookie = Request.GetCookie("Admin_Session_Guid");

			if (sessionCookie == null)
			{
				return LatexConvertResult.BadSession;
			}
			Guid session;

			if (!Guid.TryParse(sessionCookie, out session))
			{
				return LatexConvertResult.BadSession;
			}

			var sessionValidateResult = LoginManager.ValidateSession(session);

			switch (sessionValidateResult)
			{
				case ValidateSessionResultType.SessionExpired:
					return LatexConvertResult.SessionExpired;

				case ValidateSessionResultType.SessionInvalid:
					return LatexConvertResult.BadSession;
			}

			var loginId = LoginManager.LoginIdFromSession(session);

			if (loginId == null)
			{
				return LatexConvertResult.BadSession;
			}

			var root = HttpContext.Current.Server.MapPath("~/temp/uploads");
			Directory.CreateDirectory(root);
			var provider = new MultipartFormDataStreamProvider(root);
			await Request.Content.ReadAsMultipartAsync(provider);

			var version = provider.FormData["version"];

			if (string.IsNullOrEmpty(version))
			{
				return LatexConvertResult.MissingVersion;
			}

			if (provider.FileData.Count == 0 || !File.Exists(provider.FileData[0].LocalFileName))
			{
				return LatexConvertResult.MissingFile;
			}

			var file = File.ReadAllText(provider.FileData[0].LocalFileName);
			File.Delete(provider.FileData[0].LocalFileName);

			using (var db = new ProjectDbContext())
			{
				var conv = new LatexConvertor();

				var convRes = conv.Convert(file);

				if (convRes != ConversionResult.Success)
				{
					return LatexConvertResult.InvalidFormat;
				}

				var model = new LatexUploadModel
				{
					AuthorId = loginId.Value,
					Version = version,
					HtmlZip = conv.HtmlZip,
					Pdf = conv.Pdf,
					Created = DateTime.Now,
					LatexFile = conv.LatexFile
				};

				db.LatexUploads.Add(model);
				db.SaveChanges();
				return LatexConvertResult.Success;
			}
		}

		public static int? GetSetLatexId()
		{
			var dir = HttpContext.Current.Server.MapPath("~/temp/set");
			var st = Path.Combine(dir, "latex_html.site_item");
			if (!File.Exists(st))
			{
				return null;
			}

			var latexId = File.ReadAllText(st).AsInt();

			if (!latexId.HasValue)
			{
				return null;
			}

			using (var db = new ProjectDbContext())
			{
				var lm = db.LatexUploads.SqlQuery("SELECT * FROM dbo.LatexUploads " +
												  "WHERE Id = @inputId",
					new SqlParameter("@inputId", latexId.Value)).SingleOrDefault();

				if (lm == null)
				{
					return null;
				}

				return latexId.Value;
			}
		}

		public static PublishLatexResult PublishLatex(int latexId, bool forceIfThere = false)
		{
			var dir = HttpContext.Current.Server.MapPath("~/temp/set");
			var st = Path.Combine(dir, "latex_html.site_item");

			var outManualHtmlPath = Path.Combine(
				dir, "latex_output_Manual.html");

			using (var db = new ProjectDbContext())
			{
				var lm = db.LatexUploads.SqlQuery("SELECT * FROM dbo.LatexUploads " +
												  "WHERE Id = @inputId",
					new SqlParameter("@inputId", latexId)).SingleOrDefault();

				if (lm == null)
				{
					return new PublishLatexResult { Kind = PublishLatexResultType.InputLatexIdInvalid };
				}

				if (lm.LatexFile == null)
				{
					return new PublishLatexResult { Kind = PublishLatexResultType.MissingLatexFile };
				}

				if (lm.HtmlZip != null && !forceIfThere)
				{
					var newDir = HttpContext.Current.Server.MapPath($"~/temp/auto_delete_{Guid.NewGuid()}");
					Directory.CreateDirectory(newDir);
					var zp = Path.Combine(newDir, "Manual.zip");
					File.WriteAllBytes(zp, lm.HtmlZip);
					PublishLatexResult ret = null;
					using (var zip = ZipFile.OpenRead(zp))
					{
						var entry = zip.GetEntry("Manual.html");
						if (entry != null)
						{
							using (var sr = new StreamReader(entry.Open()))
							{
								ret = new PublishLatexResult
								{
									Kind = PublishLatexResultType.Success,
									HtmlContent = sr.ReadToEnd()
								};
								File.WriteAllText(outManualHtmlPath, ret.HtmlContent);
							}
						}
					}
					Directory.Delete(newDir, true);
					if (ret != null)
					{
						File.WriteAllText(st, latexId.ToString());
						return ret;
					}
				}

				if (!forceIfThere && File.Exists(st) && File.Exists(outManualHtmlPath))
				{
					return new PublishLatexResult
					{
						Kind = PublishLatexResultType.Success,
						HtmlContent = File.ReadAllText(outManualHtmlPath, Encoding.UTF8)
					};
				}

				var conv = new LatexConvertor();

				string htmlContent;

				if (conv.Convert(lm.LatexFile, true, false) != ConversionResult.Success ||
				   !conv.HtmlMap.TryGetValue("Manual.html", out htmlContent))
				{
					return new PublishLatexResult { Kind = PublishLatexResultType.ConvertFail };
				}

				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				File.WriteAllText(outManualHtmlPath, htmlContent, Encoding.UTF8);

				lm.HtmlZip = conv.HtmlZip;
				db.LatexUploads.AddOrUpdate(lm);
				db.SaveChanges();

				File.WriteAllText(st, latexId.ToString());

				return new PublishLatexResult
				{
					Kind = PublishLatexResultType.Success,
					HtmlContent = htmlContent
				};
			}
		}

		public enum PublishLatexResultType
		{
			Success,
			InputLatexIdInvalid,
			SetLatexIdInvalid,
			MissingLatexFile,
			ConvertFail
		}
		public class PublishLatexResult
		{
			public PublishLatexResultType Kind { get; set; }
			public string HtmlContent { get; set; }
		}
	}
}