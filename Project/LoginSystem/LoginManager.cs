﻿using Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
        
    }

}