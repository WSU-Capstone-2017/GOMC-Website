using Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Project.LoginSystem
{
    public class LoginManager
    {
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
            foreach (var i in dbContext.UserLogins)
            {
                if (i.Email == email && i.PasswordHash == GetHash(password)) 
                {
                    return true;
                }
            }
            return false;
        }
        public int? GetLoginID(string email, string password)                //We put int? that way we are able to return null
        {
            if (IsValidEmail(email) == false)
            {
                return null;
            }
            if (password.Length <= 7)
            {
                return null;
            }
            foreach (var i in dbContext.UserLogins)
            {
                if (i.Email == email && i.PasswordHash == GetHash(password))  //Password that is hashed needs to be the same as the hashed password
                {
                    return i.ID;                                          //If the email and password is correct, it will return the ID
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
                byte[] hashmi = hash.ComputeHash(Encoding.UTF8.GetBytes(value));   //Password is in string, gets computed to hash value and byte value

                foreach (var i in hashmi)                                        //Used for changing it back to string value
                {
                    stringbuilder.Append(i.ToString("x2"));                      //String builder for making the string into one whole string,append is adding var i to the string which will have hex value and 2 bytes each
                }

                return stringbuilder.ToString();                                  //Returning the string created

            }
        }
    }

}