using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
	public class DownloadsModel
	{

		public string DownloadName { get; set; }
		public string ExampleName { get; set; }

		public DownloadItem[] Downloads { get; set; }
		public ExampleList[] Examples { get; set; }

		public DownloadsModel(string dname, IEnumerable<DownloadItem> ditems, string rname, IEnumerable<ExampleList> ritems)
		{
			DownloadName = dname;
			Downloads = ditems.ToArray();
			ExampleName = rname;
			Examples = ritems.ToArray();
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

		public class ExampleItem
		{
			public string TagName { get; set; }
			public string TarBall { get; set; }
			public string ZipBall { get; set; }
		}

		public class ExampleList
		{
			public string Name { get; set; }
			public string Link { get; set; }

			public ExampleList(string name, string link)
			{
				Name = name;
				Link = link;
			}
		}
	}
}