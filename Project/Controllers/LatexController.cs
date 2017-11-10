using System;
using System.IO;
using System.Net.Http;
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
					Created = DateTime.Now
				};

				db.LatexUploads.Add(model);
				db.SaveChanges();
				return LatexConvertResult.Success;
			}
		}
	}
}