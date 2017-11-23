using Project.Data;
using Project.LoginSystem;
using Project.Models.LoginSystem;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Project.Core;

namespace Project.Controllers
{
    public class LoginController : ApiController
    {
        private readonly LoginManager loginManager;

        public LoginController()                                        //Created this that way stuff from dbContext will not be null when called from loginmanager
        {
            loginManager = new LoginManager();
        }

        private int CheckFailedLogins(int loginId)
        {
            using (var db = new ProjectDbContext())
            {
                var lm =
                    db.FailedLogins.SqlQuery($"SELECT * FROM dbo.FailedLogins WHERE LoginId = '{loginId}'")
                    .Where(j => j.Date > (DateTime.Now - TimeSpan.FromHours(1)))
                    .ToArray();

                return lm.Length;
            }
        }

        public LoginResult ValidateLogin(FormDataCollection uiData)
        {
            var loginCredentials = uiData.ToDictionary(j => j.Key, j => j.Value);
            string email = loginCredentials.GetValue("uName");
            string password = loginCredentials.GetValue("pCode");
            string captchaResponse = loginCredentials.GetValue("g-recaptcha-response");

            var result = loginManager.GetLoginId(email, password);
            if (result.ResultType == LoginResultType.InvalidEmail)
            {
                return new LoginResult(LoginResultType.InvalidEmail);
            }
            if (result.ResultType == LoginResultType.InvalidPassword)
            {
                using (var db = new ProjectDbContext())
                {
                    var lm = db.UserLogins.SqlQuery($"SELECT * FROM dbo.UserLoginModels WHERE Email = '{email}'").SingleOrDefault();
                    if (lm != null)
                    {
                        var clm = CheckFailedLogins(lm.Id);
                        if (clm >= 3)
                        {
                            return new LoginResult(LoginResultType.NeedCaptcha);
                        }

                        db.FailedLogins.Add(new FailedLoginModel()
                        {
                            LoginId = lm.Id,
                            Date = DateTime.Now
                        });

                        db.SaveChanges();
                    }
                }
                return new LoginResult(LoginResultType.InvalidPassword);
            }
            if (result.ResultType == LoginResultType.Success)
            {
                Debug.Assert(result.LoginId.HasValue);

                using (var dbContext = new ProjectDbContext())
                {
                    var session = Guid.NewGuid(); //Create a GUID for id which has (email,password)
                    var expiredTime =
                        DateTime.Now +
                        TimeSpan.FromHours(3); //From exact time at that moment, the login will expire 3 hrs from then
                    var loggedIn =
                        new AlreadyLoggedModel(); //loggedin using the table created in AlreadyLoggedModel(4 options)
                    loggedIn.Expiration = expiredTime;
                    loggedIn.Session =
                        session; //Getting into loggedin and then session which is given to each user, session is a Guid(unique identifier)
                    loggedIn.LoginId = result.LoginId.Value; //We use .value to get the loginID since it is nullable
                    dbContext.AlreadyLoggedIns.Add(loggedIn); //Lets you add stuff in the AlreadyLoggedIns            

                    dbContext.SaveChanges(); //Saves changes automatically
                    return new LoginResult(LoginResultType.Success, session);
                }
            }
            return null;
        }
        public Boolean ValidateSession(Guid session)       //Create function for validating session
        {
	        using(var dbContext = new ProjectDbContext())
	        {
		        foreach(var i in dbContext.AlreadyLoggedIns) //Var i gets table from AlreadyLoggedIns in the database
		        {
			        if(i.Session == session && DateTime.Now < i.Expiration
			        ) //checks to see if sessions match and input expiration is less than expiration in database
			        {
				        return true;
			        }
		        }
	        }
	        return false;
        }           
    }
}