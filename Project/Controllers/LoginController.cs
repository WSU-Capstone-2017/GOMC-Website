using Project.Data;
using Project.LoginSystem;
using Project.Models.LoginSystem;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Project.Core;

namespace Project.Controllers
{
    public class LoginController : ApiController
    {
        private ProjectDbContext dbContext;
        private LoginManager loginManager;

        public LoginController()                                        //Created this that way stuff from dbContext will not be null when called from loginmanager
        {
            dbContext = new ProjectDbContext();
            loginManager = new LoginManager(dbContext);
        }

        public LoginResult ValidateLogin(FormDataCollection uiData)                       
        {
            var loginCredentials = uiData.ToDictionary(j => j.Key, j => j.Value);
	        string email = loginCredentials.GetValue("uName");
	        string password = loginCredentials.GetValue("pCode");

            var result = loginManager.GetLoginId(email, password);          
            if (result.ResultType == LoginResultType.InvalidEmail )
            {
                return new LoginResult(LoginResultType.InvalidEmail);
            }
            if (result.ResultType == LoginResultType.InvalidPassword)
            {
                return new LoginResult(LoginResultType.InvalidPassword);
            }
            if (result.ResultType == LoginResultType.Success)
            {
                var session = Guid.NewGuid();               //Create a GUID for id which has (email,password)
                var expiredTime = DateTime.Now + TimeSpan.FromHours(3);     //From exact time at that moment, the login will expire 3 hrs from then
                var loggedIn = new AlreadyLoggedModel();                //loggedin using the table created in AlreadyLoggedModel(4 options)
                loggedIn.Expiration = expiredTime;
                loggedIn.Session = session;                         //Getting into loggedin and then session which is given to each user, session is a Guid(unique identifier)
                loggedIn.LoginId = result.LoginId.Value;                   //We use .value to get the loginID since it is nullable
                dbContext.AlreadyLoggedIns.Add(loggedIn);           //Lets you add stuff in the AlreadyLoggedIns            

                dbContext.SaveChanges();                //Saves changes automatically
                return new LoginResult(LoginResultType.Success, session);
            }
            return null;
        }
        public enum LoginResultType
        {
            Success,
            InvalidEmail,
            InvalidPassword
        }
        public class GetLoginIdResult
        { 
            public LoginResultType ResultType { get; set; }   
            public int? LoginId { get; set; }
            public GetLoginIdResult(LoginResultType type, int? loginId = null)
            {
                ResultType = type;
                LoginId = loginId;
            }
        }
        public class LoginResult
        {
            public LoginResultType ResultType { get; set; }   
            public Guid? Session { get; set; }
            public LoginResult(LoginResultType type, Guid? session = null)
            {
                ResultType = type;
                Session = session;
            }
        }
        public Boolean ValidateSession(Guid session)       //Create function for validating session
        {
            foreach (var i in dbContext.AlreadyLoggedIns)                      //Var i gets table from AlreadyLoggedIns in the database
            {
                if (i.Session == session && DateTime.Now < i.Expiration)      //checks to see if sessions match and input expiration is less than expiration in database
                {
                    return true;
                }
            }
            return false;
        }           
    }
}