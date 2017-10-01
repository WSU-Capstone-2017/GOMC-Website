namespace Project.Models.Gomc
{
	public class StructureInput
	{
		/// <summary>
		///     Sets box number (first box is box '0').
		/// </summary>
		public int BoxNumber { get; set; }

		/// <summary>
		///     Sets PSF file name.
		/// </summary>
		public string PsfFileName { get; set; }

		public StructureInput()
		{
			
		}

		public StructureInput(int boxNumber, string psfFileName)
		{
			BoxNumber = boxNumber;
			PsfFileName = psfFileName;
		}
	}
}