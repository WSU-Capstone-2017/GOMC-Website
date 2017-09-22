using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Project.Core
{
	public class JsonConv
	{
		public static T ToObject<T>(string jsonString) => JsonConvert.DeserializeObject<T>(jsonString);

		public static string ToJson<T>(T obj) => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver()
		});
	}
}