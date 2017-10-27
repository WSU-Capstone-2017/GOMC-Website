using System.Data.Entity;
using Project.Models;
using Project.Models.LoginSystem;

namespace Project.Data
{
	public class ProjectDbContext : DbContext
	{
        public DbSet<UserLoginModel> UserLogins { get; set; }
        public DbSet<AlreadyLoggedModel> AlreadyLoggedIns { get; set; }
        public DbSet<RegistrationModel> Registrations{ get; set; }
		public DbSet<LatexUploadModel> LatexUploads { get; set; }

		public ProjectDbContext() : base("name=ProjectDbConnectionString")
		{
			
		}
	}
   
}