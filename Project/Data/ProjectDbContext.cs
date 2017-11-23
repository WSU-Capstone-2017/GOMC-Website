using System.Data.Entity;
using Project.Models;
using Project.Models.LoginSystem;
using System;

namespace Project.Data
{
	public class ProjectDbContext : DbContext
	{
        public DbSet<UserLoginModel> UserLogins { get; set; }
        public DbSet<AlreadyLoggedModel> AlreadyLoggedIns { get; set; }
        public DbSet<RegistrationModel> Registrations{ get; set; }
		public DbSet<LatexUploadModel> LatexUploads { get; set; }
		public DbSet<AnnouncementModel> Announcements { get; set; }
        public DbSet<FailedLoginModel> FailedLogins { get; set; }

        public ProjectDbContext() : base("name=ProjectDbConnectionString")
		{
			
		}
	}

    public class FailedLoginModel
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public DateTime Date { get; set; }
    }
}