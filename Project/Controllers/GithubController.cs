using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Project.Core;
using Project.Data;
using Project.Latex;
using Project.Models;
using Project.Models.Github;

namespace Project.Controllers
{
	public class GithubController : ApiController
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		[HttpPost]
		public IHttpActionResult Callback(JObject input)
		{
			try
			{
				log.Info("Github Webhooks--");

				var githubEvents = Request.Headers.GetValues("X-GitHub-Event").ToList();
				if (githubEvents.Contains("ping"))
				{
					var ping = input.ToObject<PingModel>();

					log.Info("Ping Event");
					return Ok(ping);
				}

				if (!githubEvents.Contains("push"))
				{
					log.Error("Unrecognized input");
					return Ok(input);

				}
				var push = input.ToObject<PushModel>();

				log.Info("Push Event");
				log.Info(push.Ref);

				foreach (var c in push.Commits)
				{
					if (c.Modified.Contains("Manual.tex"))
					{
						log.Info("Manual.tex was changed");

						var rsp = Utils.SimpleGet(
							"https://api.github.com/repos/ataher1992/GOMC_Manual/contents/Manual.tex?ref=test");

						log.Info("Getting Manual.tex from repo");

						var jsn = JObject.Parse(rsp);
						var durl = jsn["download_url"];

						string file;
						using (var wc = new WebClient())
						{
							file = wc.DownloadString(durl.ToString());
						}

						log.Info("Uploading..");
						log.Info(push.HeadCommit.Id);
						log.Info(push.Pusher.Email);
						if (UploadLatex(file, push.HeadCommit.Id, push.Pusher.Email))
						{
							log.Info("Uploaded succeed");
							return Ok("Manual updated.");
						}
						else
						{
							log.Warn("Upload failed");
							return Ok("Manual did not update.");
						}
					}
				}
				return Ok("Manual.tex was not changed");
			}
			catch(Exception e)
			{
				log.Error(e.ToString());
				return InternalServerError(e);
			}
		}

		private static bool UploadLatex(string file, string version, string email)
		{
			using (var db = new ProjectDbContext())
			{
				log.Info($"fetching email '{email}'");

				var prm = new SqlParameter("@inputEmail", email);

				var s = db.UserLogins.SqlQuery(
					"SELECT * FROM dbo.UserLoginModels WHERE Email = @inputEmail", prm)
					.SingleOrDefault();

				if (s == null)
				{
					log.Warn("email not found");
					return false;
				}

				var conv = new LatexConvertor();

				var convRes = conv.Convert(file, true);

				if (convRes != ConversionResult.Success)
				{
					return false;
				}

				var model = new LatexUploadModel
				{
					AuthorId = s.Id,
					Version = version,
					HtmlZip = conv.HtmlZip,
					Pdf = conv.Pdf,
					Created = DateTime.Now
				};

				db.LatexUploads.Add(model);
				db.SaveChanges();

				return true;
			}
		}
	}
}
