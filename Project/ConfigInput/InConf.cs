using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Project.Core;
using Project.Models.Gomc;

namespace Project.ConfigInput
{
	public class InConf
	{
		static string NameOfProp(PropertyInfo p)
		{
			var attrib = p.GetCustomAttribute<InConfNameAttribute>();

			return attrib == null ? p.Name : attrib.Name;
		}

		private static bool ParaTypeNameValid(string name)
		{
			return typeof(ForceFieldType)
				.GetFields()
				.Where(j => j.FieldType == typeof(ForceFieldType))
				.Select(j => j.GetCustomAttribute<InConfNameAttribute>().Name)
				.ToList()
				.Contains(name);
		}

		private static readonly Dictionary<string, PropertyInfo> props =
			typeof(ConfigInputModel).GetProperties().ToDictionary(NameOfProp, v => v);

		private static T? EnumTryParse<T>(string str) where T : struct
		{
			try
			{
				var dict = Utils.EnumNames<T>().ToDictionary(j => j, Utils.EnumParse<T>);
				foreach(var a in Utils.EnumNames<T>())
				{
					var atr = Utils.GetAttributeOfEnumMember<T, InConfNameAttribute>(a);
					if(atr != null)
					{
						dict.Add(atr.Name, dict[a]);
					}
				}
				return dict[str];
			}
			catch
			{
				return null;
			}
		}
		public static ConfigInputModel Parse(string inConfFile)
		{
			var lines = inConfFile.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var model = new ConfigInputModel();
			foreach (var line in lines)
			{
				if (line.StartsWith("#"))
				{
					continue;
				}

				var args = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

				if (args.Length <= 1)
				{
					return null;
				}

				if (args.Length == 3 && args[0] == "Structure" && args[1].AsInt().HasValue)
				{
					model.Structures = (model.Structures ?? new string[0]).Concat(new[] { args[2] }).ToArray();
					continue;
				}
				if (args.Length == 3 && args[0] == "Coordinates" && args[1].AsInt().HasValue)
				{
					model.Coordinates = (model.Coordinates ?? new string[0]).Concat(new[] { args[2] }).ToArray();
					continue;
				}
				if (args.Length == 3 && args[0] == "ChemPot" && args[2].AsDouble().HasValue)
				{
					model.ChemPot = new ResNameValue(args[1], args[2].AsDouble().Value);
					continue;
				}

				if (args.Length == 5 && args[0] == "BoxDim")
				{
					var val0 = args[1].AsInt();
					var val1 = args[2].AsDouble();
					var val2 = args[3].AsDouble();
					var val3 = args[4].AsDouble();

					if (val0 == null || val1 == null || val2 == null || val3 == null)
					{
						return null;
					}
					model.BoxDim = (model.BoxDim ?? new BoxDimInput[0])
						.Concat(new[] { new BoxDimInput(val1.Value, val2.Value, val3.Value) })
						.ToArray();
					continue;
				}

				var prop = props.GetValue(args[0]);

				if (args.Length == 3 && prop?.PropertyType == typeof(FreqInput) && args[1].AsBool().HasValue && args[2].AsUlong().HasValue)
				{
					var fqin = new FreqInput(args[1].AsBool().Value, args[2].AsUlong().Value);
					prop.SetValue(model, fqin);
					continue;
				}
				if (args.Length == 3 && prop?.PropertyType == typeof(OutBoolean) && args[1].AsBool().HasValue && args[2].AsBool().HasValue)
				{
					var fqin = new OutBoolean(args[1].AsBool().Value, args[2].AsBool().Value);
					prop.SetValue(model, fqin);
					continue;
				}

				if (args.Length == 2 && prop?.PropertyType == typeof(string))
				{
					prop.SetValue(model, args[1]);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(bool) && args[1].AsBool().HasValue)
				{
					prop.SetValue(model, args[1].AsBool().Value);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(int) && args[1].AsInt().HasValue)
				{
					prop.SetValue(model, args[1].AsInt().Value);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(uint) && args[1].AsUint().HasValue)
				{
					prop.SetValue(model, args[1].AsUint().Value);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(int?) && args[1].AsInt().HasValue)
				{
					prop.SetValue(model, args[1].AsInt().Value);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(ulong) && args[1].AsUlong().HasValue)
				{
					prop.SetValue(model, args[1].AsUlong().Value);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(ulong?) && args[1].AsUlong().HasValue)
				{
					prop.SetValue(model, args[1].AsUlong().Value);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(double) && args[1].AsDouble().HasValue)
				{
					prop.SetValue(model, args[1].AsDouble().Value);
					continue;
				}
				if (args.Length == 2 && prop?.PropertyType == typeof(char) && args[1].Length == 1)
				{
					prop.SetValue(model, args[1][0]);
					continue;
				}
				if (args.Length == 2 && EnumTryParse<PrngType>(args[1]).HasValue)
				{
					prop.SetValue(model, EnumTryParse<PrngType>(args[1]));
					continue;
				}
				if (args.Length == 2 && EnumTryParse<PotentialType>(args[1]).HasValue)
				{
					prop.SetValue(model, EnumTryParse<PotentialType>(args[1]));
					continue;
				}
				if (args.Length == 2 && EnumTryParse<ExcludeType>(args[1]).HasValue)
				{
					prop.SetValue(model, EnumTryParse<ExcludeType>(args[1]));
					continue;
				}
				if (args.Length == 2 && EnumTryParse<GemcType>(args[1]).HasValue)
				{
					prop.SetValue(model, EnumTryParse<GemcType>(args[1]));
					continue;
				}
				if(args.Length >= 2 && args[0] == "PressureCalc" && args[1].AsBool().HasValue)
				{
					var b = args[1].AsBool().Value;
					if(b && args.Length != 3)
					{
						return null;
					}
					if(!b && args.Length != 2)
					{
						return null;
					}
					if(b)
					{
						var val = args[2].AsUlong();
						if(!val.HasValue)
						{
							return null;
						}
						model.PressureCalc = val.Value;
					}
					continue;
				}
				if (prop == null)
				{
					if (args.Length == 2 && ParaTypeNameValid(args[0]))
					{
						var val = args[1].AsBool();

						if (val == null)
						{
							return null;
						}

						if (val == false)
						{
							continue;
						}

						if (val == true)
						{
							var ft = typeof(ForceFieldType)
									.GetFields()
									.Where(j => j.FieldType == typeof(ForceFieldType))
									.Select(j => new
									{
										Name = j.GetCustomAttribute<InConfNameAttribute>().Name,
										Val = Utils.EnumParse<ForceFieldType>(j.Name)
									})
									.FirstOrDefault(j => j.Name == args[0]);
							if (ft == null)
							{
								return null;
							}

							model.ParaType = ft.Val;
							continue;
						}
					}
				}
				return null;
			}
			return model;
		}
	}
}