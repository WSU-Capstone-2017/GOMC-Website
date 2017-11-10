using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using Project.Core;
using Project.Data;
using Project.Latex;
using Project.LoginSystem;
using Project.Models;
using Project.Models.LoginSystem;

namespace Project.Controllers
{
    public class AdminController : ApiController
    {
        [HttpPost]
        public AnnouncementResult NewAnnouncement(NewAnnouncementModel model)
        {
            var authentication = Authenticate();

            if (authentication.Result != AnnouncementResult.Success || !authentication.Session.HasValue)
            {
                Debug.Assert(authentication.Result != AnnouncementResult.Success);
                return authentication.Result;
            }

            using (var db = new ProjectDbContext())
            {
                var sqlParameter = new SqlParameter("@SessionInput", authentication.Session);

                var l = db.Database
                    .SqlQuery<AlreadyLoggedModel>("dbo.GetLoginIdFromSession @SessionInput", sqlParameter)
                    .SingleOrDefault();

                if (l == null)
                {
                    return AnnouncementResult.InvalidSession;
                }

                if (l.Expiration < DateTime.Now)
                {
                    return AnnouncementResult.SessionExpired;
                }

	            if(string.IsNullOrEmpty(model?.Content))
	            {
		            return AnnouncementResult.MissingContent;
	            }

                var announcement = new AnnouncementModel
                {
                    AuthorId = l.LoginId,
                    Content = model.Content,
                    Created = DateTime.Now
                };

                db.Announcements.Add(announcement);
                db.SaveChanges();

                return AnnouncementResult.Success;

            }
        }

	    [HttpPost]
	    public FetchAnnouncementsOutput GetAnnouncementsCount()
	    {
			var authentication = Authenticate();

		    if (authentication.Result != AnnouncementResult.Success || !authentication.Session.HasValue)
		    {
			    Debug.Assert(authentication.Result != AnnouncementResult.Success);
			    return new FetchAnnouncementsOutput { Result = authentication.Result };
		    }
		    using (var db = new ProjectDbContext())
		    {
			    var totalLength = db.Database.SqlQuery<int>("SELECT COUNT(*) FROM dbo.Announcments").Single();

			    return new FetchAnnouncementsOutput
			    {
				    Result = AnnouncementResult.Success,
				    TotalLength = totalLength
			    };
		    }
		}

		[HttpPost]
        public FetchAnnouncementsOutput FetchAnnouncements(FetchAnnouncementsInput input)
        {
            var authentication = Authenticate();

            if (authentication.Result != AnnouncementResult.Success || !authentication.Session.HasValue)
            {
                Debug.Assert(authentication.Result != AnnouncementResult.Success);
                return new FetchAnnouncementsOutput {Result = authentication.Result};
            }

            using (var db = new ProjectDbContext())
            {
	            var totalLength = db.Database.SqlQuery<int>("SELECT COUNT(*) FROM dbo.Announcments").Single();
                var skip = input.PageLength * input.PageIndex;
                var take = input.PageLength;

                var sqlQuery = "SELECT * FROM Announcments " +
                          "ORDER BY Id " +
                          $"OFFSET ({skip}) ROWS FETCH NEXT ({take}) ROWS ONLY";
                var announcementResults = db.Announcements.SqlQuery(
                    sqlQuery).ToArray();

	            return new FetchAnnouncementsOutput
	            {
		            Result = AnnouncementResult.Success,
		            Announcements = announcementResults,
		            TotalLength = totalLength
	            };
            }
        }

	    [HttpPost]
	    public DeleteAnnouncementOutput DeleteAnnouncement(DeleteAnnouncementInput input)
		{
			var authentication = Authenticate();

			if (authentication.Result != AnnouncementResult.Success || !authentication.Session.HasValue)
			{
				Debug.Assert(authentication.Result != AnnouncementResult.Success);
				return new DeleteAnnouncementOutput {Result = authentication.Result};
			}

			using(var db = new ProjectDbContext())
			{
				const string query = "DELETE FROM dbo.Announcments " +
				            "WHERE Id = @inputAnnouncementId";

				var parm = new SqlParameter("@inputAnnouncementId", input.AnnouncementId);

				var affectedRows = db.Database.ExecuteSqlCommand(query, parm);

				return new DeleteAnnouncementOutput
				{
					Result = AnnouncementResult.Success,
					Deleted = affectedRows == 1
				};
			}
		}

	    public class DeleteAnnouncementOutput
	    {
		    public AnnouncementResult Result { get; set; }
			public bool Deleted { get; set; }
	    }
	    public class DeleteAnnouncementInput
	    {
		    public int AnnouncementId { get; set; }
	    }

        public class NewAnnouncementModel
        {
            public string Content { get; set; }
        }

        private class AuthenticateOutput
        {
            public AnnouncementResult Result { get; set; }
            public Guid? Session { get; set; }
        }
        private AuthenticateOutput Authenticate()
        {
            var sessionCookie = Request.GetCookie("Admin_Session_Guid");

            if (sessionCookie == null)
            {
                return new AuthenticateOutput{ Result = AnnouncementResult.InvalidSession, Session = null };
            }
            Guid session;

            if (!Guid.TryParse(sessionCookie, out session))
            {
                return new AuthenticateOutput{ Result = AnnouncementResult.InvalidSession, Session = session };
            }

            var validateSessionResultType = LoginManager.ValidateSession(session);

            switch (validateSessionResultType)
            {
                case ValidateSessionResultType.SessionExpired:
                    return new AuthenticateOutput{ Result = AnnouncementResult.SessionExpired, Session = session };

                case ValidateSessionResultType.SessionInvalid:
                    return new AuthenticateOutput{ Result = AnnouncementResult.InvalidSession, Session = session };
            }

            var loginId = LoginManager.LoginIdFromSession(session);

            if (loginId == null)
            {
                return new AuthenticateOutput{ Result = AnnouncementResult.InvalidSession, Session = session };
            }

            return new AuthenticateOutput{ Result = AnnouncementResult.Success, Session = session };
        }

        public class FetchAnnouncementsInput
        {
            public int PageIndex { get; set; }
            public int PageLength { get; set; }
        }

        public class FetchAnnouncementsOutput
        {
            public AnnouncementResult Result { get; set; }
            public int Length => Announcements?.Length ?? 0;
            public AnnouncementModel[] Announcements { get; set; }
			public int TotalLength { get; set; }
        }

        public enum AnnouncementResult
        {
            Success,
            SessionExpired,
            InvalidSession,
			MissingContent
        }

    }
}