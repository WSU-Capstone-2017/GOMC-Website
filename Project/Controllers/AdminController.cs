using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Project.Data;
using Project.Models;
using Project.Models.LoginSystem;

namespace Project.Controllers
{
	public class AdminController : ApiController
	{

		[HttpPost]
		public NewAnnouncementResult NewAnnouncement([FromBody]NewAnnouncementModel model)
		{
			using (var db = new ProjectDbContext())
			{
				var sqlParameter = new SqlParameter("@SessionInput", model.Session);
				var l = db.Database.SqlQuery<AlreadyLoggedModel>("dbo.GetLoginIdFromSession @SessionInput", sqlParameter).SingleOrDefault();

				if (l == null)
				{
					return NewAnnouncementResult.InvalidSession;
				}

				if(l.Expiration< DateTime.Now)
				{
					return NewAnnouncementResult.SessionExpired;
				}

				var announcement = new AnnouncementModel
				{
					AuthorId = l.LoginId,
					Content = model.Content,
					Created = DateTime.Now
				};

				db.Announcements.Add(announcement);

				return (NewAnnouncementResult.Success);

			}
		}

		public class NewAnnouncementModel
		{
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