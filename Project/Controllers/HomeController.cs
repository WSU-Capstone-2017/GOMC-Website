using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Project.Core;
using Project.Models;
using Project.Data;

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
           // dynamic releasesData = jsn1.zipball_url; // need to make a list of zipball_url and tarball_url to resolve the information I need in the list

            var items = new List<DownloadsModel.DownloadItem>();
            foreach (dynamic i in assets)
            {
                string name = i.name;
                name = name.Replace("_64","");
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
            return new DownloadsModel(tag, items,releaseName,releaseItems); // needs 4 params
		}
        public RegistrationModel RegistrationData()
        {
            using (var serverConn = new ProjectDbContext())
            {
                var AllRegister = serverConn.Registrations.ToList();
                var nameRoster = (
                    from row in AllRegister
                    select new { row.Name}
                    ).ToArray();

                var emailRoster = (
                    from row in AllRegister
                    select new { row.Email }
                    ).ToArray();

                // Run SELECT Name, Email FROM[projectdb].[dbo].[Registrations]
                // Iterate over the rows grabbing name and email
                // return it as a list
                return new RegistrationModel(nameRoster,emailRoster);
            }
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

        public ActionResult Admin()
        {
            return View(RegistrationData());
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
        {
            return View();
        }

        public ActionResult MoreExamples()
        {
            return View();
        }
	}
}