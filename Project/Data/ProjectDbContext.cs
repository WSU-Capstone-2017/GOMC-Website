using System.Data.Entity;
using Project.Models;
using Project.Models.LoginSystem;

namespace Project.Data
{
	public sealed class ProjectDbContext : DbContext
	{
		public class MockType
		{
			public LoginModel[] Logins { get; set; } = new LoginModel[0];
			public LoginSessions[]  LoginSessions { get; set; } = new LoginSessions[0];
			public RegistrationModel[]  Registrations { get; set; } = new RegistrationModel[0];
			public LatexUploadModel[]  LatexUploads { get; set; } = new LatexUploadModel[0];
			public AnnouncementModel[]  Announcements { get; set; } = new AnnouncementModel[0];
			public FailedLoginModel[]  FailedLogins { get; set; } = new FailedLoginModel[0];
		}

        public DbSet<LoginModel> UserLogins { get; set; }
        public DbSet<LoginSessions> LoginSessions { get; set; }
        public DbSet<RegistrationModel> Registrations{ get; set; }
		public DbSet<LatexUploadModel> LatexUploads { get; set; }
		public DbSet<AnnouncementModel> Announcements { get; set; }
        public DbSet<FailedLoginModel> FailedLogins { get; set; }


		private ProjectDbContext(MockType mock) : base(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;")
		{
			UserLogins.RemoveRange(UserLogins);
			LoginSessions.RemoveRange(LoginSessions);
			Registrations.RemoveRange(Registrations);
			LatexUploads.RemoveRange(LatexUploads);
			Announcements.RemoveRange(Announcements);
			FailedLogins.RemoveRange(FailedLogins);

			UserLogins.AddRange(mock.Logins);
			LoginSessions.AddRange(mock.LoginSessions);
			Registrations.AddRange(mock.Registrations);
			LatexUploads.AddRange(mock.LatexUploads);
			Announcements.AddRange(mock.Announcements);
			FailedLogins.AddRange(mock.FailedLogins);

			SaveChanges();
		}

		public ProjectDbContext() : base("name=ProjectDbConnectionString")
		{
		}

		public static ProjectDbContext Mock(MockType mock)
		{
			return new ProjectDbContext(mock);
		}
	}
}