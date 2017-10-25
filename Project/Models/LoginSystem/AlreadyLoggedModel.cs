using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.LoginSystem
{
    public class AlreadyLoggedModel
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public Guid Session { get; set; }
        public DateTime Expiration { get; set; }
    }
}