using System.Collections.Generic;
using Project.Core;
using Project.Models.Gomc;

namespace Project.ConfigInput
{
	public class Validator
	{
		private const string OnlyOneXAllowedFmt = "Only one {0} input is allowed for '{1}' mode.";

		private readonly ConfigInputModel input;

		private readonly Dictionary<InputModelFieldType, string> fieldErrors = new Dictionary<InputModelFieldType, string>();

		public Validator(ConfigInputModel configInput)
		{
			input = configInput;
		}

		public string GetErrorMessage()
		{
			return JsonConv.ToJson(fieldErrors);
		}

		public bool IsValid()
		{
			if (!BasicIsValid())
			{
				return false;
			}

			return true;
		}

		private bool BasicIsValid()
		{
			if (input == null)
			{
				return false;
			}

			var b = GcOnly();
		
			if (!ValidPressureCalcPressure())
			{
				b = false;
			}
			if (!ValidToleranceEwald())
			{
				b = false;
			}

			if (!PrngSeedValid())
			{
				b = false;
			}
			if (!CoordinatesLengthValid())
			{
				b = false;
			}
			if (!PressureNptValid())
			{
				b = false;
			}
			if (!LrcValid())
			{
				b = false;
			}
			if (!SwitchValid())
			{
				b = false;
			}
			if (!StructuresLengthValid())
			{
				b = false;
			}
			if (!ValidEwaldStatic())
			{
				b = false;
			}
			if (!ValidCachedFourierEwald())
			{
				b = false;
			}
			if (!ValidDielectricCachedFourier())
			{
				return false;
			}
			return b;
		}

		private bool PrngSeedValid()
		{
			if (input.Prng == PrngType.Random && input.RandomSeed != null)
			{
				fieldErrors.Add(InputModelFieldType.RandomSeed, "Field must be null, since Prng is set to 'Random'.");
				return false;
			}
			if (input.Prng == PrngType.Intseed && input.RandomSeed == null)
			{
				fieldErrors.Add(InputModelFieldType.RandomSeed, "Field cannot be null, since Prng is set to 'IntSeed'.");
				return false;
			}
			return true;
		}

		private bool CoordinatesLengthValid()
		{
			if (input.Ensemble == Ensemble.Nvt || input.Ensemble == Ensemble.Npt)
			{
				fieldErrors.Add(InputModelFieldType.Coordinates, string.Format(OnlyOneXAllowedFmt, InputModelFieldType.Coordinates, input.Ensemble));
				return input.Coordinates?.Length == 1;
			}
			return input.Coordinates?.Length == 2;
		}

		private bool PressureNptValid()
		{
			if (input.Pressure.HasValue)
			{
				var b = input.Ensemble == Ensemble.Npt || input.Ensemble == Ensemble.GibbsNpt;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.Pressure,
						"Field cannot have a value unless the ensamble is 'Npt' of 'GibbsNpt'.");
				}
				return b;
			}
			return input.Ensemble == Ensemble.Nvt || input.Ensemble == Ensemble.GibbsNvt || input.Ensemble == Ensemble.Gcmc;
		}

		private bool LrcValid()
		{
			if (input.Potential == PotentialType.Vdw)
			{
				var b = input.Lrc.HasValue;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.Lrc, "Field cannot have a value if Potential is not set to 'Vdw'.");
				}
				return b;
			}
			return !input.Lrc.HasValue;
		}

		private bool SwitchValid()
		{
			bool b;
			if (input.Potential == PotentialType.Shift)
			{
				b = input.Rswitch.HasValue;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.Rswitch, "Field must have a value if Potential is set to 'Shift'.");
				}
				return b;
			}
			b = !input.Rswitch.HasValue || input.Rswitch == 0;
			if (!b)
			{
				fieldErrors.Add(InputModelFieldType.Rswitch, "Field cannot have a value if Potential is not set to 'Shift'.");
			}
			return b;
		}

		private bool StructuresLengthValid()
		{
			if (input.Ensemble == Ensemble.Nvt || input.Ensemble == Ensemble.Npt)
			{
				fieldErrors.Add(InputModelFieldType.Structures, string.Format(OnlyOneXAllowedFmt, InputModelFieldType.Structures, input.Ensemble));
				return input.Structures?.Length == 1;
			}
			return input.Structures?.Length == 2;
		}

		private bool GcOnly()
		{
			if (input.ChemPot != null)
			{
				var b = input.Ensemble == Ensemble.Gcmc;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.ChemPot, "Field cannot have a value unless the Ensemble is 'Gcmc'.");
				}
				return b;
			}
			if (input.Fugacity != null)
			{
				var b = input.Ensemble == Ensemble.Gcmc;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.Fugacity, "Field cannot have a value unless the Ensemble is 'Gcmc'.");
				}
				return b;
			}
			return true;
		}

		private bool ValidPressureCalcPressure()
		{
			if (input.PressureCalc.HasValue)
			{
				var b = input.Pressure.HasValue;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.PressureCalc, "Field cannot have a value if Pressure is not set.");
				}
				return b;
			}
			return true;
		}

		private bool ValidToleranceEwald()
		{
			if (input.Ewald == true)
			{
				var b = input.Tolerance.HasValue;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.Tolerance, "Field must have a value if Ewald is set to 'true'.");
				}
				return b;
			}
			return true;
		}

		private bool ValidEwaldStatic()
		{
			if (input.ElectroStatic == false)
			{
				var b = !input.Ewald.HasValue || input.Ewald == false;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.Ewald, "Field must be empty or 'false' if ElectroStatic is set to 'false'.");
				}
				return b;
			}
			return true;
		}

		private bool ValidCachedFourierEwald()
		{
			bool b;
			if (input.CachedFourier == true)
			{
				b = input.Ewald == true;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.CachedFourier, "Field cannot have a value if Ewald is not set to 'true'.");
				}
				return b;
			}
			b = !input.Ewald.HasValue || input.Ewald == false;
			if (!b)
			{
				fieldErrors.Add(InputModelFieldType.CachedFourier, "Field must have a value if Ewald is set to 'true'.");
			}
			return b;
		}

		private bool ValidDielectricCachedFourier()
		{
			bool b;
			if (input.Dielectric.HasValue)
			{
				b = !input.Ewald.HasValue || input.Ewald == false;
				if (!b)
				{
					fieldErrors.Add(InputModelFieldType.Dielectric, "Field cannot have a value if Ewald is not set to 'true'");
				}
				return b;
			}
			b = !input.Dielectric.HasValue;
			if (!b)
			{
				fieldErrors.Add(InputModelFieldType.Dielectric, "Field must have a value if Ewald is set to 'true'.");
			}
			return b;
		}
	}
}