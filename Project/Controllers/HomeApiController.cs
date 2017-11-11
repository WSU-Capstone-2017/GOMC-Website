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
				const int skip = 0;
				const int take = 5;

				var sqlQuery = "SELECT * FROM Announcments " +
				               "ORDER BY Id " +
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