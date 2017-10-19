using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.LoginSystem
{
    public class AlreadyLoggedModel
    {
        public int ID { get; set; }
        public int LoginID { get; set; }
        public Guid Session { get; set; }
        public DateTime Expiration { get; set; }
    }
}