namespace Project.Models
{
	public class RegistrationModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Affiliation { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
	}

	public enum RegistrationErrorType
	{
		Success,
		MissingName,
		MissingEmail,
		MissingAffiliation,
		MissingTitle,
		MissingText,
		EmailInvalidFormat,
	}

	public class RegistrationResult
	{
		public RegistrationModel Model { get; set; }
		public RegistrationErrorType? ErrorType { get; set; }

		public static RegistrationResult ErrorResult(RegistrationErrorType error)
		{
			return new RegistrationResult
			{
				ErrorType = error
			};
		}

	}
}