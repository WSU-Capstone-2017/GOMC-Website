using Project.ConfigInput;

namespace Project.Models.Gomc
{
	/// <summary>
	///     The potential function type.
	/// </summary>
	public enum PotentialType
	{
		/// <summary>
		///     Non-bonded interaction energy and force calculated based on n-6 (Lennard-Johns) equation.
		///     This function will be discussed further in the Intermolecular energy and Virial calculation
		///     section.
		/// </summary>
		[InConfName("VDW")]
		Vdw,

		/// <summary>
		///     This option forces the potential energy to be zero at Rcut distance. This function will
		///     be discussed further in the Intermolecular energy and Virial calculation section.
		/// </summary>
		[InConfName("SHIFT")]
		Shift,

		/// <summary>
		///     This option smoothly forces the potential energy to be zero at Rcut distance and starts
		///     modifying the potential at Rswitch distance. Depending on force field type, specific potential
		///     function will be applied. These functions will be discussed further in the Intermolecular energy
		///     and Virial calculation section.
		/// </summary>
		[InConfName("SWITCH")]
		Switch
	}
}