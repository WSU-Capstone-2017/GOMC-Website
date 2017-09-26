using System.Web.Mvc;
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

		private HomeModel GetHomeModel()
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
	}
}