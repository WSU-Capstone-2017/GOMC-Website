using System;

namespace Project.Models.LoginSystem
{
    public class LoginSessions
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public Guid Session { get; set; }
        public DateTime Expiration { get; set; }
    }
}