using System;
using System.Collections.Generic;
using System.Linq;
using Project.Models.Gomc;

namespace Project.Core
{
	public static class Exten
	{

		public static TVal GetValue<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key)
		{
			return dict.TryGetValue(key, out TVal val) ? val : default(TVal);
		}

		public static bool? AsBool(this string str)
		{
			return bool.TryParse(str, out bool result) ? result : (bool?)null;
		}

		public static ForceFieldType? AsForceFieldType(this string str)
		{
			return Enum.TryParse(str, out ForceFieldType val) ? val : (ForceFieldType?)null; 
		}
	}
}