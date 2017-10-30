namespace Project.Models.Github
{
	public class PingModel : EventBaseModel
	{
		public string Zen { get; set; }
		public int HookId { get; set; }
		public HookModel Hook { get; set; }
		public class HookModel
		{
			public string Type { get; set; }
			public int Id { get; set; }
			public string Name { get; set; }
			public bool Active { get; set; }
			public string[] Events { get; set; }
			public ConfigModel Config { get; set; }
			public string UpdatedAt { get; set; }
			public string CreatedAt { get; set; }
			public string Url { get; set; }
			public string TestUrl { get; set; }
			public string PingUrl { get; set; }
			public ResponseModel LastResponse { get; set; }
		}

		public class ResponseModel
		{
			public string Code { get; set; }
			public string Status { get; set; }
			public string Message { get; set; }

		}
		public class ConfigModel
		{
			public string ContentType { get; set; }
			public string InsecureSsl { get; set; }
			public string Secret { get; set; }
			public string Url { get; set; }
		}
	}
}