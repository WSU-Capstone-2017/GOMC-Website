using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
	public class MenuModel
	{
		public MenuModelItem[] Items { get; set; }

		public MenuModel(IEnumerable<MenuModelItem> items = null)
		{
			Items = (items ?? new MenuModelItem[]{}).ToArray();
		}
	}
}