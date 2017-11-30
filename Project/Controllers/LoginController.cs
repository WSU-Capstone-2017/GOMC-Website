using Project.Data;
using Project.LoginSystem;
using Project.Models.LoginSystem;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.WebPages;
using Project.Core;

namespace Project.Controllers
{
    public class LoginController : ApiController
    {
        private readonly LoginManager loginManager;

	    public LoginController() : this(null)
	    {
		    
	    }
        public LoginController(Func<ProjectDbContext> dbGetter)
        {
	        DbGetter = dbGetter ?? (() => new ProjectDbContext());
            loginManager = new LoginManager(DbGetter);
        }

        private int CheckFailedLogins(ProjectDbContext db, int loginId)
        {
            var lm =
                    db.FailedLogins.SqlQuery($"SELECT * FROM dbo.FailedLogins WHERE LoginId = '{loginId}'")
                    .Where(j => j.Date > (DateTime.Now - TimeSpan.FromHours(1)))
                    .ToArray();

                return lm.Length;            
        }
	    public Func<ProjectDbContext> DbGetter { get; }

		public Func<string, bool> CaptchaCheckFn { get; set; }

		public LoginResult ValidateLogin(FormDataCollection uiData)
		{
			var captchaCheckFn = CaptchaCheckFn ?? RegistrationController.CaptchaCheck;

			var loginCredentials = uiData.ToDictionary(j => j.Key, j => j.Value);
            var email = loginCredentials.GetValue("uName");
            var password = loginCredentials.GetValue("pCode");
            var captchaResponse = loginCredentials.GetValue("g-recaptcha-response");

            var result = loginManager.GetLoginId(email, password);

            if (result.ResultType == LoginResultType.InvalidEmail || !result.LoginId.HasValue)
            {
                return new LoginResult(result.ResultType);
            }

            using (var db = DbGetter())
            {
                if (result.ResultType == LoginResultType.InvalidPassword)
                {
                    db.FailedLogins.Add(new FailedLoginModel()
                    {
                        LoginId = result.LoginId.Value,
                        Date = DateTime.Now
                    });

                    db.SaveChanges();
                }

                var failedLogins = CheckFailedLogins(db, result.LoginId.Value);

                if (failedLogins >= 3)
                {
                    if (captchaResponse.IsEmpty())
                    {
                        return new LoginResult(LoginResultType.NeedCaptcha);
                    }

                    if (!captchaCheckFn(captchaResponse))
                    {
                        return new LoginResult(LoginResultType.InvalidCaptcha);
                    }
                }

                if (result.ResultType == LoginResultType.InvalidPassword)
                {
                    return new LoginResult(LoginResultType.InvalidPassword);
                }

                if (result.ResultType == LoginResultType.Success)
                {
                    var session = Guid.NewGuid();

                    var expiredTime =
                        DateTime.Now +
                        TimeSpan.FromHours(3);

                    var loggedIn = new LoginSessions
                    {
                        Expiration = expiredTime,
                        Session = session,
                        LoginId = result.LoginId.Value
                    };

                    db.LoginSessions.Add(loggedIn);

                    db.SaveChanges();
                    return new LoginResult(LoginResultType.Success, session);
                }
            }
            return null;
        }
        public bool ValidateSession(Guid session)
        {
            return LoginManager.ValidateSession(session) == ValidateSessionResultType.SessionValid;
        }
    }
}