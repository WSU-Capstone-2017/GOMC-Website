using System;
using System.Linq;
using System.Web.Http;
using Project.Data;

namespace Project.Controllers
{
	public class HomeApiController : ApiController
	{
		[HttpPost]
		public AnnouncementItem[] FetchAnnouncements()
		{
			using (var db = new ProjectDbContext())
			{
				var totalLength = db.Database.SqlQuery<int>("SELECT COUNT(*) FROM dbo.Announcments").Single();

				if(totalLength == 0)
				{
					return new AnnouncementItem[0];
				}

				const int skip = 0;
				var take = totalLength >= 5 ? 5 : totalLength;

				var sqlQuery = "SELECT * FROM Announcments " +
				               "ORDER BY Created DESC " +
				               $"OFFSET ({skip}) ROWS FETCH NEXT ({take}) ROWS ONLY";

				var announcementResults = db.Announcements
					.SqlQuery(sqlQuery)
					.Select(j => new AnnouncementItem {Content = j.Content, Created = j.Created})
					.ToArray();

				return announcementResults;
			}
		}
	}

	public class AnnouncementItem
	{
		public string Content { get; set; }
		public DateTime Created { get; set; }
	}
}