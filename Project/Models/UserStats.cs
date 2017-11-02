namespace Project.Models
{
	public class UserStats
	{
		public int NumberOfDownloads { get; set; }
		public int NumberOfWindows { get; set; }
		public int NumberOfLinux{ get; set; }
		public int NumberOfCpu { get; set; }
		public int NumberOfGpu { get; set; }
		public int NumberOfNpt { get; set; }
		public int NumberOfNvt { get; set; }
		public int NumberOfGibbsNpt { get; set; }
		public int NumberOfGibbsNvt { get; set; }
		public int NumberOfGcmc { get; set; }
	}
}