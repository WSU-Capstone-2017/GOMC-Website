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

		private readonly IDictionary<string, string> formData;

		private readonly Dictionary<InputModelFieldType, string> fieldErrors = new Dictionary<InputModelFieldType, string>();

		private readonly List<string> generalErrors = new List<string>();

		private InputModelFieldType currentFieldType;

		public ConfigFormDataConvertor(IDictionary<string, string> inputFormData)
		{
			formData = inputFormData;
		}

		public string GetErrorMessage()
		{
			return JsonConv.ToJson(new { General = generalErrors, Fields = fieldErrors });
		}

		private void SetProp(object model, PropertyInfo prop, string value)
		{
			if (prop == null)
			{
				throw new ArgumentNullException();
			}

			if (model == null)
			{
				throw new ArgumentNullException();
			}

			void setPropHelper<T>(Func<T> valFn, string onValErr, string onSetErr)
			{
				T v;
				try
				{
					v = valFn();
				}
				catch
				{
					fieldErrors.Add(currentFieldType, onValErr);
					return;
				}
				try
				{
					prop.SetValue(model, v);
				}
				catch
				{
					fieldErrors.Add(currentFieldType, onSetErr);
				}
			}

			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			var t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
			if (t == typeof(string))
			{
				setPropHelper(() => value, null, "Field cannot bet set to a string value.");
			}
			else if (t == typeof(char))
			{
				setPropHelper(() => char.Parse(value), "Input is not a valid char value.", "Field cannot bet set to a char value.");
			}
			else if (t == typeof(bool) && value.AsBool().HasValue)
			{
				setPropHelper(() => value.AsBool().Value, "Input is not a valid bool value.", "Field cannot bet set to a bool value.");
			}
			else if (t == typeof(int) && value.AsInt().HasValue)
			{
				setPropHelper(() => value.AsInt().Value, "Input is not a valid int value.", "Field cannot bet set to a int value.");
			}
			else if (t == typeof(long) && value.AsLong().HasValue)
			{
				setPropHelper(() => value.AsLong().Value, "Input is not a valid long value.", "Field cannot bet set to a long value.");
			}
			else if (t == typeof(uint) && value.AsUint().HasValue)
			{
				setPropHelper(() => value.AsUint().Value, "Input is not a valid uint value.", "Field cannot bet set to a uint value.");
			}
			else if (t == typeof(ulong) && value.AsUlong().HasValue)
			{
				setPropHelper(() => value.AsUlong().Value, "Input is not a valid ulong value.", "Field cannot bet set to a ulong value.");
			}
			else if (t == typeof(float) && value.AsFloat().HasValue)
			{
				setPropHelper(() => value.AsFloat().Value, "Input is not a valid float value.", "Field cannot bet set to a float value.");
			}
			else if (t == typeof(double) && value.AsDouble().HasValue)
			{
				setPropHelper(() => value.AsDouble().Value, "Input is not a valid double value.", "Field cannot bet set to a double value.");
			}
			else if (t.IsEnum)
			{
				setPropHelper(() => Enum.Parse(t, value), $"Input is not a valid value of {prop.Name}", $"Field cannot bet set to '{value}'.");
			}
			else
			{
				throw new ArgumentException();
			}
		}

		public ConfigInputModel Convert()
		{
			var model = new ConfigInputModel();

			foreach (var i in formData)
			{
				if (!i.Key.StartsWith(NameStart))
				{
					generalErrors.Add($"The input key '{i.Key}' could not be recognized.");
					return null;
				}

				var key = i.Key.Remove(0, NameStart.Length);

				var sp = key.Split('_');

				var prop = propMap.GetValue(key);

				if (prop != null)
				{
					currentFieldType = Utils.EnumParse<InputModelFieldType>(prop.Name);

					SetProp(model, prop, i.Value);
				}
				else if (sp.Length == 2 && sp[0].IsOneOf("ChemPot", "Fugacity") && sp[1].IsOneOf("ResName", "Value"))
				{
					if (string.IsNullOrEmpty(i.Value))
					{
						continue;
					}
					prop = propMap[sp[0]];
					currentFieldType = Utils.EnumParse<InputModelFieldType>(prop.Name);

					if (prop.GetValue(model) == null)
					{
						prop.SetValue(model, new ResNameValue());
					}

					var prop2 = typeof(ResNameValue).GetProperty(sp[1]);

					SetProp(prop.GetValue(model), prop2, i.Value);
				}
				else if (key.IsOneOf("Structures_1", "Structures_2"))
				{
					currentFieldType = InputModelFieldType.Structures;
					model.Structures = model.Structures ?? new string[2];
					model.Structures[sp[1].AsInt().Value - 1] = i.Value;
				}
				else if (key.IsOneOf("Coordinates_1", "Coordinates_2"))
				{
					currentFieldType = InputModelFieldType.Coordinates;
					model.Coordinates = model.Coordinates ?? new string[2];
					model.Coordinates[sp[1].AsInt().Value - 1] = i.Value;
				}
				else if (key.IsOneOf("BoxDim_1_XAxis", "BoxDim_1_YAxis", "BoxDim_1_ZAxis", "BoxDim_2_XAxis", "BoxDim_2_YAxis", "BoxDim_2_ZAxis"))
				{
					currentFieldType = InputModelFieldType.BoxDim;
					model.BoxDim = model.BoxDim ?? new BoxDimInput[2];

					var prop2 = typeof(BoxDimInput).GetProperty(sp[2]);

					var boxI = sp[1].AsInt().Value - 1;

					model.BoxDim[boxI] = model.BoxDim[boxI] ?? new BoxDimInput();
					SetProp(model.BoxDim[boxI], prop2, i.Value);
				}
				else if (sp.Length == 2 && sp[0].EndsWith("Freq") && sp[1].IsOneOf("Enabled", "Value") &&
					(prop = propMap.GetValue(sp[0])) != null && prop.PropertyType == typeof(FreqInput))
				{
					currentFieldType = Utils.EnumParse<InputModelFieldType>(prop.Name);

					prop.SetValue(model, prop.GetValue(model) ?? new FreqInput());
                    // The second property of the freqInput is missing from here, it only gets the boolean value
					var prop2 = typeof(FreqInput).GetProperty(sp[1]);
                    // Maybe we need something like the below for a fix? I feel like this is something that Ahmed would know most on, so I'd like to talk to you before I start trying my own thing !Caleb
                    // var prop3 = typeof(FreqInput).GetProperty(sp[1]); 
                    // SetProp(prop.GetValue(model), prop2, i.Value);
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
						throw new ApplicationException();
					}

					prop.SetValue(model, prop.GetValue(model) ?? new OutBoolean());

					currentFieldType = Utils.EnumParse<InputModelFieldType>(prop.Name);

					var prop2 = typeof(OutBoolean).GetProperty(sp[1]);

					SetProp(prop.GetValue(model), prop2, i.Value);
				}
				else
				{
					generalErrors.Add($"The input field '{key} : {i.Value}' was not recognized.");
					return null;
				}
			}
			return model;
		}
	}
}