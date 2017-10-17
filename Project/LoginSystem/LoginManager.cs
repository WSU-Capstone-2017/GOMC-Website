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
                if (i.Email == email&&i.PasswordHash == password)
                {
                    return true;
                }
            }
            return false;
        }
        public int? GetLoginID(string email, string password)
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
                if (i.Email == email && i.PasswordHash == password)
                {
                    return i.ID;
                }
            }
            return null;
        }
        private bool IsValidEmail(string email)
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
        public static String GetHash(string value)
        {
            StringBuilder stringbuilder = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                byte[] hashmi = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (var i in hashmi)
                {
                    stringbuilder.Append(i.ToString("x2"));
                }

                return stringbuilder.ToString();

            }
        }
    }

}