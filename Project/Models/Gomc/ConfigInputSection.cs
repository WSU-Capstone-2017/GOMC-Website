using System.Collections.Generic;
using System.Linq;

namespace Project.Models.Gomc
{
	/// <summary>
	///     Settings for simulation input setup.
	/// </summary>
	public class ConfigInputSection
	{
		/// <summary>
		///     Determines whether to restart.
		/// </summary>
		public bool Restart { get; set; }

		/// <summary>
		///     Dictates how to start the pseudo-random number generator (PRNG)
		/// </summary>
		public PrngType Prng { get; set; }

		/// <summary>
		///     Defines the seed number. This value should be null if <see cref="Prng" /> is not set to IntSeed.
		/// </summary>
		public int? RandomSeed { get; set; }

		/// <summary>
		///     Sets force field type.
		/// </summary>
		public ForceFieldType ParaType { get; set; }

		/// <summary>
		///     Provides the name and location of the parameter file to use for the simulation.
		/// </summary>
		public string ParametersFileName { get; set; }

		/// <summary>
		///     Defines the PDB filenames (coordinates) for each box in the system.
		/// </summary>
		/// <remarks>
		///     NVT and NPT ensembles requires only one PDB file and GEMC/GCMC requires two
		///     PDB files. If the number of PDB files is not compatible with the simulation type, the program
		///     will terminate.
		///     In case of <see cref="Restart" /> set to 'true', the restart PDB output file from GOMC (OutputName BOX 0
		///     restart.pdb) can be used for both boxes.
		/// </remarks>
		public CoordinateInput[] Coordinates { get; set; }

		/// <summary>
		///     Defines the PSF filenames (structures) for each box in the system.
		/// </summary>
		/// <remarks>
		///     NVT and NPT ensembles requires only one PSF file and GEMC/GCMC requires two
		///     PSF files. If the number of PSF files is not compatible with the simulation type, the program
		///     will terminate.
		///     In case of <see cref="Restart" /> set to 'true', the restart PSF output file from GOMC (OutputName BOX 0
		///     restart.pdb) can be used for both boxes.
		/// </remarks>
		public StructureInput[] Structures { get; set; }

		public ConfigInputSection()
		{
		}

		public ConfigInputSection(bool restart, PrngType prng, int? randomSeed, ForceFieldType paraType,
			string parametersFileName, IEnumerable<CoordinateInput> coordinates,
			IEnumerable<StructureInput> structures)
		{
			Restart = restart;
			Prng = prng;
			RandomSeed = randomSeed;
			ParaType = paraType;
			ParametersFileName = parametersFileName;
			Coordinates = coordinates.ToArray();
			Structures = structures.ToArray();
		}
	}
}