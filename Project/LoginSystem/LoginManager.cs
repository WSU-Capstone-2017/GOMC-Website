using Project.Data;
using Project.Models.LoginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using static Project.Controllers.LoginController;

namespace Project.LoginSystem
{
    public class LoginManager
    {
	    private static readonly byte[] salt =
		    Encoding.UTF8.GetBytes(
			    "IvgpV69JXsiEb3VdhXuijykfjvWWutgsthAiQs1bdfXf0kKRgdkBGC2MSdJ9Sp92YeWehTXF9tzCywbmJSdW2hTmoClpejFV");

		private ProjectDbContext dbContext;
        public LoginManager(ProjectDbContext databaseContext)
        {
            dbContext = databaseContext;                                
        }
        public Boolean LoginIsValid(string email, string password)
        {
            if (IsValidEmail(email) == false) 
            {
                return false;
            }
            if (password.Length <= 7)
            {
                return false;
            }
            using (var db = new ProjectDbContext())
            {
                var b = db.Database.SqlQuery<UserLoginModel>($"select * from UserLoginModels where email = '{email}'").ToArray();
                if (b.Length == 0)
                {
                    return false;
                }
                if (b.Length == 1)
                {
                    if (b[0].PasswordHash == GetHash(password))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }                   
                }             
            }
            return false;           
        }
        public GetLoginIdResult GetLoginId(string email, string password)                
        {
            if (LoginManager.IsValidEmail(email) == false)
            {
                return new GetLoginIdResult(LoginResultType.InvalidEmail);
            }
            if (password.Length <= 7)
            {
                return new GetLoginIdResult(LoginResultType.InvalidPassword);
            }
            using (var db = new ProjectDbContext())
            {
                var b = db.Database.SqlQuery<UserLoginModel>($"select * from UserLoginModels where email = '{email}'").ToArray();
                if (b.Length == 0)
                {
                    return new GetLoginIdResult(LoginResultType.InvalidEmail);
                }
                if (b.Length == 1)
                {
                    if (b[0].PasswordHash == LoginManager.GetHash(password))
                    {
                        return new GetLoginIdResult(LoginResultType.Success, b[0].Id);
                    }
                    else
                    {
                        return new GetLoginIdResult(LoginResultType.InvalidPassword);
                    }
                }
            }
            return null;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var adrs = new MailAddress(email);
                return adrs.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static String GetHash(string value)                                //Function for getting password to hash value
        {
            StringBuilder stringbuilder = new StringBuilder();                    //For building a string

            using (var hash = SHA256.Create())                                     //var confonts to w.e hash is and this is where we start creating using sha256 algorithm
            {
                byte[] hashmi = hash.ComputeHash(Encoding.UTF8.GetBytes(value).Concat(salt).ToArray());   //Password is in string, gets computed to hash value and byte value

                foreach (var i in hashmi)                                        //Used for changing it back to string value
                {
                    stringbuilder.Append(i.ToString("x2"));                      //String builder for making the string into one whole string,append is adding var i to the string which will have hex value and 2 bytes each
                }

                return stringbuilder.ToString();                                  //Returning the string created

            }
        }
    }

}