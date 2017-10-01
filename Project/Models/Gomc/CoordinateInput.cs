namespace Project.Models.Gomc
{
	public class CoordinateInput
	{
		/// <summary>
		///     Sets box number (first box is box '0').
		/// </summary>
		public int BoxNumber { get; set; }

		/// <summary>
		///     Sets PDB file name.
		/// </summary>
		public string PdbFileName { get; set; }

		public CoordinateInput()
		{
			
		}
		public CoordinateInput(int boxNumber, string pdbFileName)
		{
			BoxNumber = boxNumber;
			PdbFileName = pdbFileName;
		}
	}
}