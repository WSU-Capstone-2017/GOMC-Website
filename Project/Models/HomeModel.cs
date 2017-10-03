namespace Project.Models
{
	public class HomeModel
	{
		public MenuModel Menu { get; set; }

		public HomeModel(MenuModel menu)
		{
			Menu = menu;
		}
	}
}