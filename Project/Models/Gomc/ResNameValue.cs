namespace Project.Models.Gomc
{
	/// <summary>
	///     Specifies a resname with a double value.
	/// </summary>
	public class ResNameValue
	{
		/// <summary>
		///     The resname.
		/// </summary>
		public string ResName { get; set; }

		/// <summary>
		///     The value.
		/// </summary>
		public double Value { get; set; }

		public ResNameValue(string resName, double value)
		{
			ResName = resName;
			Value = value;
		}
	}
}