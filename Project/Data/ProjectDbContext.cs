using System.Data.Entity;
using Project.Models;

namespace Project.Data
{
	public class ProjectDbContext : DbContext
	{
		public DbSet<MenuModelItem> Menus { get; set; }

		public ProjectDbContext() : base("name=ProjectDbConnectionString")
		{
			
		}
	}
}