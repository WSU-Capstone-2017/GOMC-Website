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

		[HttpPost]
		public IHttpActionResult Callback([FromBody] PingModel ping)
		{
			return Ok(ping);
		}

		[HttpPost]
		public IHttpActionResult Callback([FromBody] PushModel push)
		{
			foreach(var c in push.Commits)
			{
				if(c.Modified.Contains("Manual.tex"))
				{
					var rsp = Utils.SimpleGet("https://api.github.com/repos/ataher1992/GOMC_Manual/contents/Manual.tex");
					var jsn = JObject.Parse(rsp);
					var durl = jsn["download_url"];
					string file;
					using(var wc = new WebClient())
					{
						file = wc.DownloadString(durl.ToString());
					}
					if(UploadLatex(file, push.HeadCommit.Id, push.Pusher.Email))
					{
						return Ok("Manual updated.");
					}
					else
					{
						return Ok("Manual did not update.");
					}
				}
			}
			return Ok("Manual.tex was not changed");
		}

		private static bool UploadLatex(string file, string version, string email)
		{
			using (var db = new ProjectDbContext())
			{
				foreach (var s in db.UserLogins)
				{
					if (email != s.Email)
					{
						continue;
					}
					var conv = new LatexConvertor();

					var convRes = conv.Convert(file);

					if (convRes != ConversionResult.Success)
					{
						return false;
					}

					var model = new LatexUploadModel
					{
						AuthorId = s.Id,
						Version = version,
						Html = conv.Html,
						Pdf = conv.Pdf
					};

					db.LatexUploads.Add(model);
					return true;
				}
			}
			return false;
		}
	}
}
