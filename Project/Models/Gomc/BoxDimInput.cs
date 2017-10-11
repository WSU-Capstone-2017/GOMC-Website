namespace Project.Models.Gomc
{
	/// <summary>
	/// Input for BoxDim field.
	/// </summary>
	public class BoxDimInput
	{
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

		public BoxDimInput()
		{
			
		}
		public BoxDimInput(double xAxis, double yAxis, double zAxis)
		{
			XAxis = xAxis;
			YAxis = yAxis;
			ZAxis = zAxis;
		}
	}
}