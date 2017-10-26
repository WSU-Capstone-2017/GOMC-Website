using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
	public class DownloadsModel
	{

		public string Name { get; set; }

		public DownloadItem[] Items { get; set; }
		
		public DownloadsModel(string name, IEnumerable<DownloadItem> items)
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