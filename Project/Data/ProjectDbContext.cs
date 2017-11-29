using System.Data.Entity;
using Project.Models;
using Project.Models.LoginSystem;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data
{
	public sealed class ProjectDbContext : DbContext
	{
		public class MockType
		{
			public UserLoginModel[] UserLogins { get; set; } = new UserLoginModel[0];
			public AlreadyLoggedModel[]  AlreadyLoggedIns { get; set; } = new AlreadyLoggedModel[0];
			public RegistrationModel[]  Registrations { get; set; } = new RegistrationModel[0];
			public LatexUploadModel[]  LatexUploads { get; set; } = new LatexUploadModel[0];
			public AnnouncementModel[]  Announcements { get; set; } = new AnnouncementModel[0];
			public FailedLoginModel[]  FailedLogins { get; set; } = new FailedLoginModel[0];
		}

        public DbSet<UserLoginModel> UserLogins { get; set; }
        public DbSet<AlreadyLoggedModel> AlreadyLoggedIns { get; set; }
        public DbSet<RegistrationModel> Registrations{ get; set; }
		public DbSet<LatexUploadModel> LatexUploads { get; set; }
		public DbSet<AnnouncementModel> Announcements { get; set; }
        public DbSet<FailedLoginModel> FailedLogins { get; set; }


		private ProjectDbContext(MockType mock) : base(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;")
		{
			UserLogins.RemoveRange(UserLogins);
			AlreadyLoggedIns.RemoveRange(AlreadyLoggedIns);
			Registrations.RemoveRange(Registrations);
			LatexUploads.RemoveRange(LatexUploads);
			Announcements.RemoveRange(Announcements);
			FailedLogins.RemoveRange(FailedLogins);

			UserLogins.AddRange(mock.UserLogins);
			AlreadyLoggedIns.AddRange(mock.AlreadyLoggedIns);
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

    [Table("FailedLogins")]
    public class FailedLoginModel
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public DateTime Date { get; set; }
    }
}