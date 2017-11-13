using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
		public static string GetCookie(this HttpRequestMessage request, string cookieName)
		{
			var cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
			return cookie?[cookieName].Value;
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
		public static byte[] ReadToEnd(this System.IO.Stream stream)
		{
			long originalPosition = 0;

			if (stream.CanSeek)
			{
				originalPosition = stream.Position;
				stream.Position = 0;
			}

			try
			{
				byte[] readBuffer = new byte[4096];

				int totalBytesRead = 0;
				int bytesRead;

				while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
				{
					totalBytesRead += bytesRead;

					if (totalBytesRead == readBuffer.Length)
					{
						int nextByte = stream.ReadByte();
						if (nextByte != -1)
						{
							byte[] temp = new byte[readBuffer.Length * 2];
							Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
							Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
							readBuffer = temp;
							totalBytesRead++;
						}
					}
				}

				byte[] buffer = readBuffer;
				if (readBuffer.Length != totalBytesRead)
				{
					buffer = new byte[totalBytesRead];
					Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
				}
				return buffer;
			}
			finally
			{
				if (stream.CanSeek)
				{
					stream.Position = originalPosition;
				}
			}
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