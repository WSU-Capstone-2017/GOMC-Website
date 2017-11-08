using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Project.Models
{
    [Table("Registrations")]
	public class RegistrationModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Affiliation { get; set; }
		public string Text { get; set; }

		public RegistrationModel(){}
        public RegistrationModel(string name, string email) {
            Name = name;
            Email = email;
        }
    }

	public enum RegistrationErrorType
	{
		CaptchaInvalid,
		MissingName,
		MissingEmail,
		MissingAffiliation,
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