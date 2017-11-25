using Project.Data;
using Project.LoginSystem;
using Project.Models.LoginSystem;
using System;
using System.Diagnostics;
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

        public LoginController()                                        
        {
            loginManager = new LoginManager();
        }

        private int CheckFailedLogins(ProjectDbContext db, int loginId)
        {
            var lm =
                    db.FailedLogins.SqlQuery($"SELECT * FROM dbo.FailedLogins WHERE LoginId = '{loginId}'")
                    .Where(j => j.Date > (DateTime.Now - TimeSpan.FromHours(1)))
                    .ToArray();

                return lm.Length;            
        }

        public LoginResult ValidateLogin(FormDataCollection uiData)
        {
            var loginCredentials = uiData.ToDictionary(j => j.Key, j => j.Value);
            var email = loginCredentials.GetValue("uName");
            var password = loginCredentials.GetValue("pCode");
            var captchaResponse = loginCredentials.GetValue("g-recaptcha-response");

            var result = loginManager.GetLoginId(email, password);

            if (result.ResultType == LoginResultType.InvalidEmail || !result.LoginId.HasValue)
            {
                return new LoginResult(result.ResultType);
            }

            using (var db = new ProjectDbContext())
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

                    if (!RegistrationController.CaptchaCheck(captchaResponse))
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

                    var loggedIn = new AlreadyLoggedModel
                    {
                        Expiration = expiredTime,
                        Session = session,
                        LoginId = result.LoginId.Value
                    };

                    db.AlreadyLoggedIns.Add(loggedIn);

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