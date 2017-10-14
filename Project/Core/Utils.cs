using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Project.Core
{
	public static class Utils
	{
		public static Uri AsUri(string url) => url != null ? new Uri(url, UriKind.Absolute) : null;

		public static string SimpleGet(string getUrl)
		{
			using (var client = NewClient())
			{
				var response = client.GetAsync(getUrl).Result;
				response.EnsureSuccessStatusCode();
				return response.Content.ReadAsStringAsync().Result;
			}
		}

		public static HttpClient NewClient(string url = null)
		{
			var ret = new HttpClient(
				new HttpClientHandler
				{
					AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
				});

			ret.DefaultRequestHeaders.Add("User-Agent", "Anything");

			if (url != null)
			{
				ret.BaseAddress = AsUri(url);
			}

			return ret;
		}
		public static T ModelFromActionResult<T>(ActionResult actionResult) where T : class
		{
			object model;
			if (actionResult.GetType() == typeof(ViewResult))
			{
				var viewResult = (ViewResult)actionResult;
				model = viewResult.Model;
			}
			else if (actionResult.GetType() == typeof(PartialViewResult))
			{
				var partialViewResult = (PartialViewResult)actionResult;
				model = partialViewResult.Model;
			}
			else
			{
				return null;
			}
			return model as T;
		}

		public static T EnumParse<T>(string str)
		{
			return (T)Enum.Parse(typeof(T), str);
		}

		public static T? EnumTryParse<T>(string str) where T : struct
		{
			try
			{
				return (T?)Enum.Parse(typeof(T), str);
			}
			catch
			{
				return null;
			}
		}
		public static T[] EnumVals<T>()
		{
			return (T[])Enum.GetValues(typeof(T));
		}

		public static string[] EnumNames<T>()
		{
			return Enum.GetNames(typeof(T));
		}

		public static TAttrib GetAttributeOfEnumMember<TEnum, TAttrib>(string m) where TAttrib : Attribute
		{
			try
			{
				var p = typeof(TEnum).GetMember(m);
				return (TAttrib)p[0].GetCustomAttributes(typeof(TAttrib), false)[0];
			}
			catch
			{
				return default(TAttrib);
			}
		}
		public static TAttrib GetAttributeOfEnumMember<TEnum, TAttrib>(TEnum m) where TAttrib : Attribute
		{
			try
			{
				var p = typeof(TEnum).GetMember(m.ToString());
				return (TAttrib)p[0].GetCustomAttributes(typeof(TAttrib), false)[0];
			}
			catch
			{
				return default(TAttrib);
			}
		}
	}

}