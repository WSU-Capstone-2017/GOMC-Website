using Newtonsoft.Json;

namespace Project.Models.Github
{
	public class PushModel : EventBaseModel
	{
		public string Ref { get; set; }
		public string Head { get; set; }
		public string Before { get; set; }
		public string After { get; set; }
		public bool Created { get; set; }
		public bool Deleted { get; set; }
		public bool Forced { get; set; }
		public string BaseRef { get; set; }
		public string Compare { get; set; }
		public int Size{ get; set; }
		public int DistinctSize{ get; set; }
		public CommitModel[] Commits { get; set; }

		[JsonProperty("head_commit")]
		public CommitModel HeadCommit { get; set; }
		public AuthorModel Pusher { get; set; }
		public class CommitModel
		{
			public string Sha { get; set; }
			public string Id { get; set; }
			public string TreeId { get; set; }
			public bool Distinct { get; set; }
			public string Message { get; set; }
			public string Timestamp { get; set; }
			public string Url{ get; set; }
			public AuthorModel Author{ get; set; }
			public AuthorModel Commitor{ get; set; }
			public string[] Added { get; set; }
			public string[] Removed { get; set; }
			public string[] Modified { get; set; }
		}

		public class AuthorModel
		{
			public string Name { get; set; }
			public string Email{ get; set; }
			public string Username{ get; set; }
		}
	}
}