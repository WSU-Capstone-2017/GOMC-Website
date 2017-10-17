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
        private LoginManager loginManager = new LoginManager();
        public Guid LoginValid(string email, string password)
        {
            var isloginvalid = loginManager.LoginIsValid(email, password);
            if (isloginvalid == false)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            }
            Guid id = Guid.NewGuid();
            //TODO:Save this to the database
            return id;
        }
    }
}