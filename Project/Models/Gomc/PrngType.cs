using Project.ConfigInput;

namespace Project.Models.Gomc
{
	public enum PrngType
	{
		/// <summary>
		///     Randomizes Mersenne Twister PRNG with random bits based on the system time.
		/// </summary>
		[InConfName("RANDOM")]
		Random,

		/// <summary>
		///     This option “seeds” the Mersenne Twister PRNG with a standard integer. When
		///     the same integer is used, the generated PRNG stream should be the same every time, which
		///     is helpful in tracking down bugs.
		/// </summary>
		[InConfName("INTSEED")]
		Intseed
	}
}