using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Project.Core;
using Project.Models.Gomc;

namespace Project.ConfigInput
{
	public class ConfigFormDataConvertor
	{
		private const string NameStart = "gomc_config_input_";

		private static readonly Dictionary<string, PropertyInfo> propMap =
			ConfigInputModel.PropertyInfos.ToDictionary(j => j.Name, j => j);

		private static void SetProp(object model, PropertyInfo prop, string value)
		{
			if (model == null || prop == null ||string.IsNullOrEmpty(value))
			{
				return;
			}
			var t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
			if (t == typeof(string))
			{
				prop.SetValue(model, value);
			}
			else if (t == typeof(char))
			{
				prop.SetValue(model, char.Parse(value));
			}
			else if (t == typeof(bool) && value.AsBool().HasValue)
			{
				prop.SetValue(model, value.AsBool() ?? false);
			}
			else if (t == typeof(int) && value.AsInt().HasValue)
			{
				prop.SetValue(model, value.AsInt() ?? 0);
			}
			else if (t == typeof(long) && value.AsLong().HasValue)
			{
				prop.SetValue(model, value.AsLong() ?? 0);
			}
			else if (t == typeof(uint) && value.AsUint().HasValue)
			{
				prop.SetValue(model, value.AsUint() ?? 0);
			}
			else if (t == typeof(ulong) && value.AsUlong().HasValue)
			{
				prop.SetValue(model, value.AsUlong() ?? 0);
			}
			else if (t == typeof(float) && value.AsFloat().HasValue)
			{
				prop.SetValue(model, value.AsFloat() ?? 0);
			}
			else if (t == typeof(double) && value.AsDouble().HasValue)
			{
				prop.SetValue(model, value.AsDouble() ?? 0);
			}
			else if (t.IsEnum)
			{
				prop.SetValue(model, Enum.Parse(t, value));
			}
			else
			{
				throw new ArgumentException();
			}
		}
		public static ConfigInputModel Convert(IDictionary<string, string> formData)
		{
			var model = new ConfigInputModel();
			foreach (var i in formData)
			{
				if (!i.Key.StartsWith(NameStart))
				{
					return null;
				}

				var key = i.Key.Remove(0, NameStart.Length);

				var sp = key.Split('_');

				var prop = propMap.GetValue(key);

				if (prop != null)
				{
					SetProp(model, prop, i.Value);
				}
				else if (sp.Length == 2 && sp[0].IsOneOf("ChemPot", "Fugacity") && sp[1].IsOneOf("ResName", "Value"))
				{
					prop = propMap[sp[0]];

					if (prop.GetValue(model) == null)
					{
						prop.SetValue(model, new ResNameValue());
					}

					var prop2 = typeof(ResNameValue).GetProperty(sp[1]);

					SetProp(prop.GetValue(model), prop2, i.Value);
				}
				else if (key.IsOneOf("Structures_1", "Structures_2"))
				{
					model.Structures = model.Structures ?? new string[2];
					model.Structures[sp[1].AsInt().Value - 1] = i.Value;
				}
				else if (key.IsOneOf("Coordinates_1", "Coordinates_2"))
				{
					model.Coordinates = model.Coordinates ?? new string[2];
					model.Coordinates[sp[1].AsInt().Value - 1] = i.Value;
				}
				else if (key.IsOneOf("BoxDim_1_XAxis", "BoxDim_1_YAxis", "BoxDim_1_ZAxis", "BoxDim_2_XAxis", "BoxDim_2_YAxis", "BoxDim_2_ZAxis"))
				{
					model.BoxDim = model.BoxDim ?? new BoxDimInput[2];

					var prop2 = typeof(BoxDimInput).GetProperty(sp[2]);

					var boxI = sp[1].AsInt().Value - 1;

					model.BoxDim[boxI] = model.BoxDim[boxI] ?? new BoxDimInput();
					SetProp(model.BoxDim[boxI], prop2, i.Value);
				}
				else if (sp.Length == 2 && sp[0].EndsWith("Freq") && sp[1].IsOneOf("Enabled", "Value") &&
					(prop = propMap.GetValue(sp[0])) != null && prop.PropertyType == typeof(FreqInput))
				{
					prop.SetValue(model, prop.GetValue(model) ?? new FreqInput());

					var prop2 = typeof(FreqInput).GetProperty(sp[1]);

					SetProp(prop.GetValue(model), prop2, i.Value);
				}
				else if (sp.Length == 2 && sp[0].StartsWith("Out") && sp[1].IsOneOf("1", "2") &&
					(prop = propMap.GetValue(sp[0])) != null && prop.PropertyType == typeof(OutBoolean))
				{
					if (sp[1] == "1")
					{
						sp[1] = "First";
					}
					else if (sp[1] == "2")
					{
						sp[1] = "Second";
					}
					else
					{
						return null;
					}
					prop.SetValue(model, prop.GetValue(model) ?? new OutBoolean());

					var prop2 = typeof(OutBoolean).GetProperty(sp[1]);

					SetProp(prop.GetValue(model), prop2, i.Value);
				}
				else
				{
					return null;
				}
			}
			return model;
		}
	}
}