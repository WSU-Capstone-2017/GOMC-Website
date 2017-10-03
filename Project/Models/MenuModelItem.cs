namespace Project.Models
{
	public class MenuModelItem
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Action { get; set; }

		public string Controller { get; set; }

		public MenuModelItem()
		{
		}

		public MenuModelItem(string name, string action, string controller)
		{
			Name = name;
			Action = action;
			Controller = controller;
		}
	}
}