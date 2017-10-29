using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace Project.Controllers
{
	public class GithubController : ApiController
	{
		private static readonly Dictionary<string,string> formMap = new Dictionary<string, string>();

		[HttpPost]
		public IHttpActionResult Callback(JObject jobj)
		{
			formMap.Clear();
			foreach(var j in jobj.Properties())
			{
				var	val = j.Value.ToString();
				formMap.Add(j.Name, val);
			}
			return Ok(formMap);
		}
		[HttpGet]
		public string LastCallback()
		{
			return formMap.Select(j => $"{j.Key}:{j.Value}\n").Aggregate("", (a, b) => a += b);
		}
	}

}