using System;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
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

		public Func<ProjectDbContext> DbGetter { get; }

		public LatexController() : this(null)
		{

		}

		public LatexController(Func<ProjectDbContext> dbGetter)
		{
			DbGetter = dbGetter ?? (() => new ProjectDbContext());
		}

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

			var file = File.ReadAllText(provider.FileData[0].LocalFileName, Encoding.Default);
			File.Delete(provider.FileData[0].LocalFileName);

			using (var db = DbGetter())
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

		public static PublishLatexResult PublishLatex(int latexId, Func<ProjectDbContext> dbGetter = null)
		{
			dbGetter = dbGetter ?? (() => new ProjectDbContext());

			var dir = HttpContext.Current.Server.MapPath("~/temp/set");

			if(!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			using(var db = dbGetter())
			{
				var lm = db.LatexUploads.SqlQuery("SELECT * FROM dbo.LatexUploads " +
				                                  "WHERE Id = @inputId",
					new SqlParameter("@inputId", latexId)).SingleOrDefault();

				if(lm == null)
				{
					return PublishLatexResult.InputLatexIdInvalid;
				}
				
				using(var htmlZipMs = new MemoryStream(lm.HtmlZip))
				using(var zip = new ZipArchive(htmlZipMs, ZipArchiveMode.Read))
				{
					var entry = zip.GetEntry("manual_view.cshtml");

					if(entry == null)
					{
						return PublishLatexResult.MissingLatexFile;
					}

					using(var s = entry.Open())
					{
						var manualViewPath = Path.Combine(dir, "manual_view.cshtml");

						var pdfPath = Path.Combine(dir, "Manual.pdf");

						var ms = new MemoryStream();
						s.CopyTo(ms);

						File.WriteAllBytes(manualViewPath, ms.ToArray());
						File.WriteAllBytes(pdfPath, lm.Pdf);

						File.Copy(HttpContext.Current.Server.MapPath("~/views/web.config"),
							HttpContext.Current.Server.MapPath("~/temp/set/web.config"), true);

						return PublishLatexResult.Success;
					}
				}
			}
		}

		public enum PublishLatexResult
		{
			Success,
			InputLatexIdInvalid,
			MissingLatexFile
		}
	}
}