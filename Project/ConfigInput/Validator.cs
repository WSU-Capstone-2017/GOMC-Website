using System;
using System.ComponentModel;
using Project.Models.Gomc;

namespace Project.ConfigInput
{
	public class Validator
	{
		public static bool IsValid(ConfigInputModel input)
		{
			if (input == null)
			{
				return false;
			}

			switch (input.Ensemble)
			{
				case Ensemble.Nvt: return NvtIsValid(input);
				case Ensemble.Npt: return NptIsValid(input);
				case Ensemble.GibbsNvt: return GibbsNvtIsValid(input);
				case Ensemble.GibbsNpt: return GibbsNptIsValid(input);
				case Ensemble.NvtGemc: return NvtGemcIsValid(input);
				default:
					throw new InvalidEnumArgumentException(nameof(input.Ensemble));
			}
		}

		private static bool NvtIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}

		private static bool NptIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}
		private static bool GibbsNvtIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}
		private static bool GibbsNptIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}
		private static bool NvtGemcIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}
	}
}