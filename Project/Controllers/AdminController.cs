using System;
using System.Web.Http;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
	public class AdminController : ApiController
	{
		[HttpPost]
		public NewAnnouncementResult NewAnnouncement([FromBody]NewAnnouncementModel model)
		{
			using (var db = new ProjectDbContext())
			{
				foreach (var s in db.AlreadyLoggedIns)
				{
					if (s.Session.ToString() != model.Session)
					{
						continue;
					}

					if (s.Expiration < DateTime.Now)
					{
						db.AlreadyLoggedIns.Remove(s);

						return (NewAnnouncementResult.SessionExpired);
					}

					var announcement = new AnnouncementModel
					{
						AuthorId = s.LoginId,
						Title = model.Title,
						Content = model.Content,
						Created = DateTime.Now
					};

					db.Announcements.Add(announcement);

					return (NewAnnouncementResult.Success);
				}

				return (NewAnnouncementResult.InvalidSession);
			}
		}

		public class NewAnnouncementModel
		{
			public string Title { get; set; }
			public string Content { get; set; }
			public string Session { get; set; }
		}
		public enum NewAnnouncementResult
		{
			Success,
			SessionExpired,
			InvalidSession
		}

	}
}