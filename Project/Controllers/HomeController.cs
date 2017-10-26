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

        public ExamplesModel GetExamplesModel()
        {
            var repo = Utils.SimpleGet("https://github.com/GOMC-WSU/GOMC_Examples/releases");
            var jString = Newtonsoft.Json.Linq.JArray.Parse(repo);

            dynamic dString = jString[0];

            string name = dString.tag_name;
            dynamic list = dString.list;

            var items = new List<ExamplesModel.ExamplesItem>();
            foreach (dynamic node in list)
            {
                string exampleName = node.name;
                string exampleLink = node.browser_download_url;

                items.Add(new ExamplesModel.ExamplesItem(exampleName, exampleLink));
            }
            return new ExamplesModel(GetHomeModel().Menu, name, items);
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

		public ActionResult ConfigForm()
		{
			return View(GetHomeModel());
		}
	}
}