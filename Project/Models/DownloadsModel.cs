using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
	public class DownloadsModel : HomeModel
	{

		public string Name { get; set; }

		public DownloadItem[] Items { get; set; }
		
		public DownloadsModel(MenuModel menu, string name, IEnumerable<DownloadItem> items) : base(menu)
		{
			Name = name;
			Items = items.ToArray();
		}

		public class DownloadItem
		{
			public string Name { get; set; }

			public string Link { get; set; }

			public DownloadItem(string name, string link)
			{
				Name = name;
				Link = link;
			}
		}
	}
}