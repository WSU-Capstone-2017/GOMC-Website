namespace Project.Models.Gomc
{
	/// <summary>
	/// A class that represents two boolean values.
	/// </summary>
	public class OutBoolean
	{
		/// <summary>
		/// The first boolean value.
		/// </summary>
		public bool First { get; set; }

		/// <summary>
		/// The second boolean value.
		/// </summary>
		public bool Second { get; set; }

		public OutBoolean()
		{
			
		}

		public OutBoolean(bool first, bool second)
		{
			First = first;
			Second = second;
		}
	}
}