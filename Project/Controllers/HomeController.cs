using System.Web.Mvc;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View(GetMenuModel());
		}

		private MenuModel GetMenuModel()
		{
			using(var db = new ProjectDbContext())
			{
				return new MenuModel(db.Menus);
			}
		}

		public ActionResult About()
		{
			return View(GetMenuModel());
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View(GetMenuModel());
		}
	}
}