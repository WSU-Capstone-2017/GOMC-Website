namespace Project.Models.Gomc
{
	/// <summary>
	/// A class that represents two boolean values.
	/// </summary>
	public class DiBoolean
	{
		/// <summary>
		/// The first boolean value.
		/// </summary>
		public bool First { get; set; }

		/// <summary>
		/// The second boolean value.
		/// </summary>
		public bool Second { get; set; }

		public DiBoolean()
		{
			
		}

		public DiBoolean(bool first, bool second)
		{
			First = first;
			Second = second;
		}
	}
}