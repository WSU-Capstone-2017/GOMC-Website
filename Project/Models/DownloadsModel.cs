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
		
		public DownloadsModel(string Dname, IEnumerable<DownloadItem> Ditems, string Rname, IEnumerable<ExampleList>Ritems)
		{
			DownloadName = Dname;
			Downloads = Ditems.ToArray();
            ExampleName = Rname;
            Examples = Ritems.ToArray();
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