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

				var prop = props.GetValue(args[0]);

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
						}
					}
					return null;
				}
			}
			return model;
		}
	}
}