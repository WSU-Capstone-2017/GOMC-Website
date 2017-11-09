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
        public class NewAnnouncementModel
        {
            public string Content { get; set; }
        }
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
            public int Length => Announcements.Length;
            public AnnouncementModel[] Announcements { get; set; }
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
                var skip = input.PageLength * input.PageIndex;
                var take = input.PageLength;

                var announcementResults = db.Announcements.SqlQuery(
                    "SELECT * FROM Announcements" +
                    "ORDER BY Id" +
                    $"OFFSET ({skip}) ROWS FETCH NEXT ({take}) ROWS ONLY").ToArray();
                
                return new FetchAnnouncementsOutput
                {
                    Result = AnnouncementResult.Success,
                    Announcements = announcementResults
                };
            }
        }

        public enum AnnouncementResult
        {
            Success,
            SessionExpired,
            InvalidSession
        }

    }
}