namespace Project.Models.Gomc
{
	/// <summary>
	///     This is a model for the control file. The control file is GOMC’s proprietary input file. It contains key settings
	/// </summary>
	public class ConfigModel
	{
		public Ensemble Ensemble { get; set; }

		/// <summary>
		///     In this section, input file names are listed.In addition, if you want to restart your simulation or use integer
		///     seed for running your simulation, you need to modify this section according to your purpose.
		/// </summary>
		public ConfigInputSection Input { get; set; }

		/// <summary>
		///     This section contains all the variables not involved in the output of data during the simulation, or in the
		///     reading of input files at the start of the simulation. In other words, it contains settings related to the moves,
		///     the thermodynamic constants (based on choice of ensemble), and the length of the simulation.
		///     Note that some tags, or entries for tags, are only used in certain ensembles (e.g.Gibbs ensemble). These
		///     cases are denoted with colored text.
		/// </summary>
		public ConfigSystemSection System { get; set; }

		/// <summary>
		///     This section contains all the values that control output in the control file. For example, certain variables
		///     control the naming of files dumped of the block-averaged thermodynamic variables of interest, the PDB files,
		///     etc.
		/// </summary>
		public ConfigOutputSection Output { get; set; }

		public ConfigModel()
		{
		}

		public ConfigModel(ConfigInputSection input, ConfigSystemSection system, ConfigOutputSection output)
		{
			Input = input;
			System = system;
			Output = output;
		}
	}
}