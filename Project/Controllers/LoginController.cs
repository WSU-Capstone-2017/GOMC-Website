using Project.Data;
using Project.LoginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

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

        public Guid LoginValid(string email, string password)                       //Guid function for loginvalid 
        {
            var isloginvalid = loginManager.LoginIsValid(email, password);          //Gets the information from loginmanager.loginisvalid for email and password
            if (isloginvalid == false)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);   //Will throw an exception which says unathorized if the login is false
            }
            Guid id = Guid.NewGuid();               //Create a GUID for id which has (email,password)
            //TODO:Save this to the alreadyloggedin table, create new entry of type alreadyloggedin model with the 3 different options database
            dbContext.SaveChanges();                //Saves changes automatically
            return id;
        }
    }
}