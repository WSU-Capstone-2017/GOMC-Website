using Project.ConfigInput;

namespace Project.Models.Gomc
{
	/// <summary>
	///     The force field styles.
	/// </summary>
	public enum ForceFieldType
	{
		[InConfName("ParaTypeCHARMM")]
		Charmm,
		[InConfName("ParaTypeEXOTIC")]
		Exotic,
		[InConfName("ParaTypeMARTINI")]
		Martini
	}
}