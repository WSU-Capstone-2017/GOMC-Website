using System;
using System.ComponentModel;
using Project.Models.Gomc;

namespace Project.ConfigInput
{
	public class Validator
	{
		public static bool IsValid(ConfigInputModel input)
		{
			if (!BasicIsValid(input))
			{
				return false;
			}

			switch (input.Ensemble)
			{
				case Ensemble.Nvt: return NvtIsValid(input);
				case Ensemble.Npt: return NptIsValid(input);
				case Ensemble.GibbsNvt: return GibbsNvtIsValid(input);
				case Ensemble.GibbsNpt: return GibbsNptIsValid(input);
				case Ensemble.Gcmc: return GcmcIsValid(input);
				default:
					throw new InvalidEnumArgumentException(nameof(input.Ensemble));
			}
		}

		private static bool BasicIsValid(ConfigInputModel input)
		{
			if(input == null)
			{
				return false;
			}
			return true;
		}

		private static bool NvtIsValid(ConfigInputModel input)
		{
			if (input.Structures?.Length != 1)
			{
				return false;
			}
			if (input.Coordinates?.Length != 1)
			{
				return false;
			}
			return true;
		}

		private static bool NptIsValid(ConfigInputModel input)
		{
			if (input.Structures?.Length != 1)
			{
				return false;
			}
			if (input.Coordinates?.Length != 1)
			{
				return false;
			}
			return true;
		}
		private static bool GibbsNvtIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}
		private static bool GibbsNptIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}
		private static bool GcmcIsValid(ConfigInputModel input)
		{
			throw new NotImplementedException();
		}
	}
}