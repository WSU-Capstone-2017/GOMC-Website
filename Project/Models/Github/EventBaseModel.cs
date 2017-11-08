namespace Project.Models.Github
{
	public class EventBaseModel
	{
		public dynamic Repository { get; set; }
		public dynamic Sender { get; set; }
	}
}