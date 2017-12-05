using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Project.Core;
using Project.Models;
using Project.Data;
using Project.Latex;
using Project.LoginSystem;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
		public Func<ProjectDbContext> DbGetter { get; }

	    public HomeController() : this(null)
	    {
	    }

		public HomeController(Func<ProjectDbContext> dbGetter)
		{
			DbGetter = dbGetter ?? (() => new ProjectDbContext());
		}

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

        private DownloadsModel.ExampleItem[] DownloadExamplesArray()
        {
            var rsp = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC_Examples/releases");
            var jsn = Newtonsoft.Json.Linq.JArray.Parse(rsp);

            var listMoreExamples = new List<DownloadsModel.ExampleItem>();
            for (var i = 0; i < jsn.Count; i++)
            {
                dynamic item = jsn[i];

                var model = new DownloadsModel.ExampleItem
				{
					TagName = item.tag_name,
					TarBall = item.tarball_url,
					ZipBall = item.zipball_url,
				};

                listMoreExamples.Add(model);

            }
            return listMoreExamples.ToArray();
        }

        public MoreDownloadModelV2[] NewDownloadsModelArray()
        {
            var rsp = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC/releases");

            var jsn = Newtonsoft.Json.Linq.JArray.Parse(rsp);

            var listMoreDownloads = new List<MoreDownloadModelV2>();
            for (var i = 0; i < jsn.Count; i++)
            {
                dynamic jsn0 = jsn[i];
                string tag = jsn0.tag_name;
                dynamic assets = jsn0.assets;


                var items = new List<DownloadsModel.DownloadItem>();
                foreach (dynamic a in assets)
                {
                    string name = a.name;
                    name = name.Replace("_64", "");
                    string iurl = a.browser_download_url;
                    items.Add(new DownloadsModel.DownloadItem(name, iurl));
                }

                var model = new MoreDownloadModelV2
                {
                    TagName = tag,
                    Items = items.ToArray()
                };

                listMoreDownloads.Add(model);
            }
            return listMoreDownloads.ToArray();
        }

        public DownloadModelV2 NewDownloadsModel()
            {
            var rsp = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC/releases");
            var rspExamples = Utils.SimpleGet("https://api.github.com/repos/GOMC-WSU/GOMC_Examples/releases");

            var jsn = Newtonsoft.Json.Linq.JArray.Parse(rsp);

            dynamic jsn0 = jsn[0];
            dynamic jsn0Examples = jsn[0];
            string tag = jsn0.tag_name;
            string tagExamples = jsn0Examples.tag_name;
            dynamic assets = jsn0.assets;

            var releasesJSON = Newtonsoft.Json.Linq.JArray.Parse(rspExamples);
            var exampleItems = new List<DownloadsModel.DownloadItem>();

            foreach (dynamic set in releasesJSON)
            {
                string rName = set.name;
                string rLink = set.zipball_url;

                exampleItems.Add(new DownloadsModel.DownloadItem(rName, rLink));
            }

            var model = new DownloadModelV2()
            {
                ExamplesTagName = tagExamples,
                TagName = tag,
                Examples = exampleItems.ToArray(),
                Linux = new DownloadModelV2.DownloadSection(),
                Windows = new DownloadModelV2.DownloadSection(),
            };

            var items = new List<DownloadsModel.DownloadItem>();
            foreach (dynamic i in assets)
            {
                string name = i.name;
                name = name.Replace("_64", "");
                string iurl = i.browser_download_url;
                items.Add(new DownloadsModel.DownloadItem(name, iurl));
            }
            model.Linux.GPU = items.Where(j =>
                j.Name.Split('_')[3].Contains("Linux") &&
                j.Name.Split('_')[1].Contains("GPU")).ToArray();

            model.Linux.CPU = items.Where(j =>
                j.Name.Split('_')[3].Contains("Linux") &&
                j.Name.Split('_')[1].Contains("CPU")).ToArray();

            model.Windows.CPU = items.Where(j =>
                j.Name.Split('_')[3].Contains("Windows") &&
                j.Name.Split('_')[1].Contains("CPU")).ToArray();

            model.Windows.GPU = items.Where(j =>
                j.Name.Split('_')[3].Contains("Windows") &&
                j.Name.Split('_')[1].Contains("GPU")).ToArray();

            return model;
        }

        public class MoreDownloadModelV2
        {
            public string TagName { get; set; }
            public DownloadsModel.DownloadItem[] Items { get; set; }
        }

        public class MoreExamplesModel
        {
            public string ExamplesTagName { get; set; }
            public DownloadsModel.DownloadItem[] Examples { get; set; }
        }

        public class DownloadModelV2
        {
            public string TagName { get; set; }
            public string ExamplesTagName { get; set; }
            public DownloadsModel.DownloadItem[] Examples { get; set; }
            public DownloadSection Linux { get; set; }
            public DownloadSection Windows { get; set; }

            public class DownloadSection
            {
                public DownloadsModel.DownloadItem[] CPU { get; set; }
                public DownloadsModel.DownloadItem[] GPU { get; set; }
            }
        }

        public ActionResult Gomc()
        {
            using (var db = DbGetter())
            {
	            var lst = db.Announcements
					.ToArray()
		            .Select(j => new AnnouncementModel(j.Content))
		            .ToList();

	            ViewBag.AnnouncementList = lst;

				return View();
            }
        }
        public ActionResult Features()
        {
            return View();
        }
        public ActionResult Downloads()
        {
            return View(NewDownloadsModel());
        }
        public ActionResult Documentation()
        {
            var setPdfPath = "~/temp/set/Manual.pdf";
            if (System.IO.File.Exists(HttpContext.Server.MapPath(setPdfPath)))
            {
                ViewBag.UseSetPdf = true;
            }
            else
            {
                ViewBag.UseSetPdf = false;
            }
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
                using (var serverConn = DbGetter())
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

	    public ActionResult Latex2()
	    {
		    return View("Latex");
	    }
        public ActionResult Latex()
		{
	        var p = "~/temp/set/manual_view.cshtml";
            if (!System.IO.File.Exists(HttpContext.Server.MapPath(p)))
            {
                return View();
            }
            else
            {
                return View(p);
            }
        }

		public ActionResult MoreDownloads()
        {
            return View(NewDownloadsModelArray());
        }

        public ActionResult MoreExamples()
        { 
            return View(DownloadExamplesArray());
        }
    }
}