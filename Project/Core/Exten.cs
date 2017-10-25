using System;
using System.Collections.Generic;
using System.Linq;
using Project.Models.Gomc;

namespace Project.Core
{
	public static class Exten
	{
		public static bool IsOneOf(this string d, params string[] args)
		{
			return args.Any(j => j == d);
		}
		public static TVal GetValue<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key)
		{
			return dict.TryGetValue(key, out TVal val) ? val : default(TVal);
		}

		public static bool? AsBool(this string str)
		{
			return bool.TryParse(str, out var result) ? result : (bool?)null;
		}

		public static int? AsInt(this string str)
		{
			return int.TryParse(str, out var result) ? result : (int?)null;
		}
		public static long? AsLong(this string str)
		{
			return long.TryParse(str, out var result) ? result : (long?)null;
		}
		public static uint? AsUint(this string str)
		{
			return uint.TryParse(str, out var result) ? result : (uint?)null;
		}
		public static ulong? AsUlong(this string str)
		{
			return ulong.TryParse(str, out var result) ? result : (ulong?)null;
		}
		public static double? AsDouble(this string str)
		{
			return double.TryParse(str, out var result) ? result : (double?)null;
		}
		public static float? AsFloat(this string str)
		{
			return float.TryParse(str, out var result) ? result : (float?)null;
		}
		public static ForceFieldType? AsForceFieldType(this string str)
		{
			return Enum.TryParse(str, out ForceFieldType val) ? val : (ForceFieldType?)null; 
		}
	}
}