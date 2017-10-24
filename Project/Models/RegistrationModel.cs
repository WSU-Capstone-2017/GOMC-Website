using System;
using System.Linq;

namespace Project.Models
{
	public class RegistrationModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Affiliation { get; set; }
		//public string Title { get; set; } // I removed this in the UI but I can add it back if it's a problem, it seems kind of redundant so I'm only commenting this out in case we need it later ~ CL
		public string Text { get; set; }

		public RegistrationModel()
		{
		}
	}

	public enum RegistrationErrorType
	{
		Success,
		MissingName,
		MissingEmail,
		MissingAffiliation,
		// MissingTitle,
		MissingText,
		EmailInvalidFormat,
	}

	public class RegistrationResult
	{
		public bool Success => Errors == null || Errors.Length == 0;
		public RegistrationModel Model { get; set; }
		public RegistrationErrorType[] Errors { get; set; }

		public RegistrationResult ErrorResult(RegistrationErrorType error)
		{
			Errors = Errors.Concat(new[] {error}).ToArray();
			return this;
		}

	}
}