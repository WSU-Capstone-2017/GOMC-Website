using Project.ConfigInput;

namespace Project.Models.Gomc
{
	public enum GemcType
	{
		/// <summary>
		///     NVT.
		/// </summary>
		[InConfName("NVT")]
		Nvt,

		/// <summary>
		///     NPT.
		/// </summary>
		[InConfName("NPT")]
		Npt
	}
}