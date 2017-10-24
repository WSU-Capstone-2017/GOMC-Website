using System.Collections.Generic;
using System.Web.Mvc;
using Project.Core;
using Project.Models;

namespace Project.Controllers
{
	public class P2Controller : Controller
	{
		public DownloadsModel GetDownloadModel()
		{
			var rsp = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC/releases");
			var jsn = Newtonsoft.Json.Linq.JArray.Parse(rsp);

			dynamic jsn0 = jsn[0];
			string tag = jsn0.tag_name;

			dynamic assets = jsn0.assets;


			var items = new List<DownloadsModel.DownloadItem>();
			foreach (dynamic i in assets)
			{
				string name = i.name;
				string iurl = i.browser_download_url;

				items.Add(new DownloadsModel.DownloadItem(name, iurl));
			}
			return new DownloadsModel(null, tag, items);
		}
		public ActionResult Gomc()
		{
			return View();
		}
		public ActionResult Features()
		{
			return View();
		}
		public ActionResult Downloads()
		{
			return View(GetDownloadModel());
		}
		public ActionResult Documentation()
		{
			return View();
		}
		public ActionResult Publication()
		{
			return View();
		}
		public ActionResult About()
		{
			return View();
		}

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult SiteMap()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult admin()
        {
            return View();
        }
	}
}