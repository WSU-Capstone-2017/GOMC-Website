using System.Data.Entity;
using Project.Models;

namespace Project.Data
{
	public class ProjectDbContext3 : DbContext
	{
		public DbSet<MenuModelItem> Menus { get; set; }
	}
}