namespace Project.Models.Gomc
{
	/// <summary>
	/// Input for BoxDim field.
	/// </summary>
	public class BoxDimInput
	{
		/// <summary>
		///     Sets box number (first box is box ‘0’)
		/// </summary>
		public int BoxNumber { get; set; }

		/// <summary>
		///     X-axis length in Angstroms
		/// </summary>
		public double XAxis { get; set; }

		/// <summary>
		///     Y-axis length in Angstroms
		/// </summary>
		public double YAxis { get; set; }

		/// <summary>
		///     Z-axis length in Angstroms
		/// </summary>
		public double ZAxis { get; set; }
	}
}