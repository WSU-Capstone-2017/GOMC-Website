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

			return true;
		}

		private static bool BasicIsValid(ConfigInputModel input)
		{
			if (input == null)
			{
				return false;
			}
			if(!GcOnly(input))
			{
				return false;
			}
			if (!ValidPressureCalcPressure(input))
			{
				return false;
			}
			if (!ValidToleranceEwald(input))
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

		private static bool PrngSeedValid(ConfigInputModel input)
		{
			if(input.Prng == PrngType.Random && input.RandomSeed != null)
			{
				return false;
			}
			if(input.Prng == PrngType.Intseed && input.RandomSeed == null)
			{
				return false;
			}
			return true;
		}

		private static bool CoordinatesLengthValid(ConfigInputModel input)
		{
			if(input.Ensemble == Ensemble.Nvt || input.Ensemble == Ensemble.Npt)
			{
				return input.Coordinates?.Length == 1;
			}
			return input.Coordinates?.Length == 2;
		}

		private static bool PressureNptValid(ConfigInputModel input)
		{
			if(input.Pressure.HasValue)
			{
				return input.Ensemble == Ensemble.Npt || input.Ensemble == Ensemble.GibbsNpt;
			}
			return input.Ensemble == Ensemble.Nvt || input.Ensemble == Ensemble.GibbsNvt || input.Ensemble == Ensemble.Gcmc;
		}

		private static bool LrcValid(ConfigInputModel input)
		{
			if(input.Potential == PotentialType.Vdw)
			{
				return input.Lrc.HasValue;
			}
			return !input.Lrc.HasValue;
		}

		private static bool SwitchValidi(ConfigInputModel input)
		{
			if(input.Potential == PotentialType.Shift)
			{
				return input.Rswitch.HasValue;
			}
			return !input.Rswitch.HasValue;
		}
		private static bool StructuresLengthValid(ConfigInputModel input)
		{
			if (input.Ensemble == Ensemble.Nvt || input.Ensemble == Ensemble.Npt)
			{
				return input.Structures?.Length == 1;
			}
			return input.Structures?.Length == 2;
		}
		private static bool GcOnly(ConfigInputModel input)
		{
			if(input.ChemPot != null || input.Fugacity != null)
			{
				return input.Ensemble == Ensemble.Gcmc;
			}
			return true;
		}

		private static bool ValidPressureCalcPressure(ConfigInputModel input)
		{
			if (input.PressureCalc.HasValue)
			{
				return input.Pressure.HasValue;
			}
			return true;
		}

		private static bool ValidToleranceEwald(ConfigInputModel input)
		{
			if (input.Ewald == true)
			{
				return input.Tolerance.HasValue;
			}
			return true;
		}

		private static bool ValidEwaldStatic(ConfigInputModel input)
		{
			if(input.Ewald.HasValue)
			{
				return input.ElectroStatic;
			}
			return !input.ElectroStatic;
		}

		private static bool ValidCachedFourierEwald(ConfigInputModel input)
		{
			if(input.CachedFourier.HasValue)
			{
				return input.Ewald == true;
			}
			return !input.Ewald.HasValue || input.Ewald == false;
		}

		private static bool ValidDielectricCachedFourier(ConfigInputModel input)
		{
			if(input.Ewald == false)
			{
				return input.Dielectric.HasValue;
			}
			return !input.Dielectric.HasValue;
		}
	}
}