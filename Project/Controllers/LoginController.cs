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

        public Guid ValidateLogin(FormDataCollection uiData)                       //Guid function for loginvalid 
        {
            var loginCredentials = uiData.ToDictionary(j => j.Key, j => j.Value);
	        string email = loginCredentials.GetValue("uName");
	        string password = loginCredentials.GetValue("pCode");

            var loginID = loginManager.GetLoginId(email, password);          //Gets the information from loginmanager.loginisvalid for email and password
            if (loginID == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);   //Will throw an exception which says unathorized if the login is false
            }
            Guid session = Guid.NewGuid();               //Create a GUID for id which has (email,password)
            var expiredTime = DateTime.Now + TimeSpan.FromHours(3);     //From exact time at that moment, the login will expire 3 hrs from then
            var loggedIn = new AlreadyLoggedModel();                //loggedin using the table created in AlreadyLoggedModel(4 options)
            loggedIn.Expiration = expiredTime;
            loggedIn.Session = session;                         //Getting into loggedin and then session which is given to each user, session is a Guid(unique identifier)
            loggedIn.LoginId = loginID.Value;                   //We use .value to get the loginID since it is nullable
            dbContext.AlreadyLoggedIns.Add(loggedIn);           //Lets you add stuff in the AlreadyLoggedIns 


            //TODO:Save this to the alreadyloggedin table, create new entry of type alreadyloggedin model with the 3 different options database
            dbContext.SaveChanges();                //Saves changes automatically
            return session;
        }
        public Boolean ValidateSession(Guid session)       //Create function for validating session
        {
            foreach (var i in dbContext.AlreadyLoggedIns)                      //Var i gets table from AlreadyLoggedIns in the database
            {
                if (i.Session == session && DateTime.Now < i.Expiration)      //checks to see if sessions math and input expiration is less than expiration in database
                {
                    return true;
                }
            }
            return false;
        }
            
    }
}