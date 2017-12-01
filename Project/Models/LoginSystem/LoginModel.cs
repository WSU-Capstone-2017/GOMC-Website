using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models.LoginSystem
{
	[Table("Logins")]
    public class LoginModel
    {
        public int Id { get; set; } 
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}