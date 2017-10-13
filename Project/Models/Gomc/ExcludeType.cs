using Project.ConfigInput;

namespace Project.Models.Gomc
{
	/// <summary>
	///     Types of excludes from non-bonded interactions.
	/// </summary>
	public enum ExcludeType
	{
		/// <summary>
		///     1-2 - All interactions pairs of bonded atoms, except the ones that separated with one bond, will
		///     be considered and modified using 1-4 parameters defined in parameter file.
		/// </summary>
		[InConfName("1-2")]
		OneTwo,

		/// <summary>
		///     1-3 - All interaction pairs of bonded atoms, except the ones that separated with one or two bonds,
		///     will be considered and modified using 1-4 parameters defined in parameter file.
		/// </summary>
		[InConfName("1-3")]
		OneThree,

		/// <summary>
		///     1-4 - All interaction pairs of bonded atoms, except the ones that separated with one, two or three
		///     bonds, will be considered using non-bonded parameters defined in parameter file.
		/// </summary>
		[InConfName("1-4")]
		OneFour
	}
}