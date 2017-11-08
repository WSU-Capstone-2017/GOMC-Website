using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Project.Core;
using Project.Models;
using Project.Data;
using Project.LoginSystem;

namespace Project.Controllers
{
	public class HomeController : Controller
	{
		public DownloadsModel GetDownloadModel()
		{
			var rsp = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC/releases");
			var releasesResponse = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC_Examples/releases");

			var jsn = Newtonsoft.Json.Linq.JArray.Parse(rsp);
			var releasesJSON = Newtonsoft.Json.Linq.JArray.Parse(releasesResponse);

			dynamic jsn0 = jsn[0];
			string tag = jsn0.tag_name;
			dynamic assets = jsn0.assets;

			dynamic jsn1 = releasesJSON[0];
			string releaseName = jsn1.tag_name;

			var items = new List<DownloadsModel.DownloadItem>();
			foreach (dynamic i in assets)
			{
				string name = i.name;
				name = name.Replace("_64", "");
				string iurl = i.browser_download_url;

				items.Add(new DownloadsModel.DownloadItem(name, iurl));
			}

			var releaseItems = new List<DownloadsModel.ExampleList>();
			foreach (dynamic set in releasesJSON)
			{
				string rName = set.name;
				string rLink = set.zipball_url;

				releaseItems.Add(new DownloadsModel.ExampleList(rName, rLink));
			}
			return new DownloadsModel(tag, items, releaseName, releaseItems);
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
		public ActionResult Publications()
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

		public ActionResult Admin()
		{
			var g = HttpContext.Request.Cookies.Get("Admin_Session_Guid");

			if (g == null)
			{
				return View("Login");
			}

			Guid session;

			if (!Guid.TryParse(g.Value, out session))
			{
				return View("Login");
			}

			var r = LoginManager.ValidateSession(session);

			if (r == ValidateSessionResultType.SessionValid)
			{
               using (var serverConn = new ProjectDbContext())
               {
                   var ResultRoster = new List<RegistrationModel>();
                   var Roster = (
                       from row in serverConn.Registrations
                       select new { row.Name, row.Email }
                       ).ToList();
                   foreach (var val in Roster)
                   {
                       ResultRoster.Add(new RegistrationModel(val.Name, val.Email));
                   }
                   ViewBag.Rez = ResultRoster;
               }
                return View();
			}

			return View("Login");
		}

		public ActionResult Login()
		{
			return View();
		}

		public ActionResult XMLConfigForm()
		{
			return View();
		}

		public ActionResult Latex()
		{
			return View();
		}

		public ActionResult MoreDownloads()
		{ // Needs more work, gotta debug ~Caleb
            var downloadsResponse = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC/releases");
            var jsn = Newtonsoft.Json.Linq.JArray.Parse(downloadsResponse);
            var oldDownloadsList = new List<DownloadsModel.DownloadItem>();
            foreach (dynamic i in jsn)
            {
                string name = i.name;
                name = name.Replace("_64", "");
                string iurl = i.browser_download_url;
                oldDownloadsList.Add(new DownloadsModel.DownloadItem(name, iurl));
            }
            ViewBag.Downloadslist = oldDownloadsList; 
            return View();
		}

		public ActionResult MoreExamples()
        { // Needs more work, gotta debug ~Caleb
            var examplesResponse = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC_Examples/releases");
            var jsn = Newtonsoft.Json.Linq.JArray.Parse(examplesResponse);
            var examplesList = new List<DownloadsModel.DownloadItem>();
            foreach (dynamic i in jsn)
            {
                string name = i.name;
                name = name.Replace("_64", "");
                string iurl = i.browser_download_url;
                examplesList.Add(new DownloadsModel.DownloadItem(name, iurl));
            }
            ViewBag.Downloadslist = examplesList;
            return View();
		}
	}
}