using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.LoginSystem
{
    public class UserLoginModel
    {
        public int ID { get; set; } 
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}