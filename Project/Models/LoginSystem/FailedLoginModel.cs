using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models.LoginSystem
{
	[Table("FailedLogins")]
	public class FailedLoginModel
	{
		public int Id { get; set; }
		public int LoginId { get; set; }
		public DateTime Date { get; set; }
	}
}