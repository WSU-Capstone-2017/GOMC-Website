namespace Project.Models.Gomc
{
	/// <summary>
	///     Settings for the system during run.
	/// </summary>
	public class ConfigSystemSection
	{
		/// <summary>
		///     Defines what type of Gibbs Ensemble simulation you want to run (for Gibbs Ensemble runs only). The default is NVT.
		/// </summary>
		public GemcType Gemc { get; set; }

		/// <summary>
		///     The imposed pressure (in bar) for NPT simulations. The property <see cref="Gemc" /> must be set to 'Npt'.
		/// </summary>
		public double Pressure { get; set; }

		/// <summary>
		///     Sets the temperature at which the system will run in degrees Kelvin.
		/// </summary>
		public double Temperature { get; set; }

		/// <summary>
		///     Sets a specific radius that non-bounded interaction energy and force will be considered and calculated using
		///     defined potential function.
		/// </summary>
		/// <remarks>The distance to truncate the Lennard-Jones potential at.</remarks>
		public double Rcut { get; set; }

		/// <summary>
		///     Sets a specific minimum possible in angstrom that reject any move that place any atom closer than specified
		///     distance.
		/// </summary>
		/// <remarks>The minimum possible distance between any atoms.</remarks>
		public double RcutLow { get; set; }

		/// <summary>
		///     Defines whether or not long range corrections are used.
		/// </summary>
		/// <remarks>
		///     True to consider long range correction. In case of using "SHIFT" or "SWITCH" potential functions, this will be
		///     ignored.
		/// </remarks>
		public bool Lrc { get; set; }

		/// <summary>
		///     Defines which pairs of bonded atoms should be excluded from non-bonded interactions.
		/// </summary>
		/// <remarks>
		///     In CHARMM force field, the 1-4 interaction needs to be considered. Choosing “Exclude
		///     1-3” will modify 1-4 interaction based on 1-4 parameter in parameter file. If a kind force field is
		///     used, where 1-4 interaction needs to be ignored, such as TraPPE, either “exclude 1-4” needs to
		///     be chosen or 1-4 parameter needs to be assigned a value of zero in the parameter file.
		/// </remarks>
		public ExcludeType Exclude { get; set; }

		/// <summary>
		///     Defines the potential function type to calculate non-bonded interaction energy and force between
		///     atoms.
		/// </summary>
		public PotentialType Potential { get; set; }

		/// <summary>
		///     In the case of choosing “SWITCH” as potential function, a distance is set in which non-bonded
		///     interaction energy is truncated smoothly from to cutoff distance.
		/// </summary>
		/// <remarks>
		///     Define switch distance in angstrom. If the “SWITCH” function is
		///     chosen, Rswitch needs to be defined; otherwise, the program will be terminated.
		/// </remarks>
		public double Rswitch { get; set; }

		public bool ElectroStatic { get; set; }
		public double Ewald { get; set; }
		public bool CachedFourier { get; set; }
		public double Tolerance { get; set; }
		public double Dielectric { get; set; }
		public double PressureCalc { get; set; }
		public double OneFourScaling { get; set; }
		public ulong RunSteps { get; set; }
		public ulong EqSteps { get; set; }
		public ulong AdjSteps { get; set; }
		public double ChemPot { get; set; }
		public double Fugacity { get; set; }
		public double DisFreq { get; set; }
		public double RotFreq { get; set; }
		public double IntraSwapFreq { get; set; }
		public double VolFreq { get; set; }
		public double SwapFreq { get; set; }
		public bool UseConstantArea { get; set; }
		public bool FixVolBox0 { get; set; }
		public double BoxDim { get; set; }
		public int CbmcFirst { get; set; }
		public int CbmcNth { get; set; }
		public int CbmcAng { get; set; }
		public int CbmcDih { get; set; }
	}
}