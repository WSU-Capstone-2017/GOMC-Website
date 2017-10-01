using System.Collections.Generic;
using System.Web.Mvc;
using Project.Core;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View(GetHomeModel());
		}

		public HomeModel GetHomeModel()
		{
			using(var db = new ProjectDbContext())
			{
				return new HomeModel(new MenuModel(db.Menus));
			}
		}

		public ActionResult About()
		{
			return View(GetHomeModel());
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View(GetHomeModel());
		}

		public DownloadsModel GetDownloadModel()
		{
			var rsp = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC/releases");
			var jsn = Newtonsoft.Json.Linq.JArray.Parse(rsp);

			dynamic jsn0 = jsn[0];
			string tag = jsn0.tag_name;

			dynamic assets = jsn0.assets;


			var items = new List<DownloadsModel.DownloadItem>();
			foreach(dynamic i in assets)
			{
				string name = i.name;
				string iurl = i.browser_download_url;

				items.Add(new DownloadsModel.DownloadItem(name, iurl));
			}
			return new DownloadsModel(GetHomeModel().Menu, tag, items);
		}

		public ActionResult Downloads()
		{
			return View(GetDownloadModel());
		}
	}
}