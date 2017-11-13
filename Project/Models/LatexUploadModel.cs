using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
	[Table("LatexUploads")]
	public class LatexUploadModel
	{
		public int Id { get; set; }
		public int AuthorId { get; set; }
		public string Version { get; set; }
		public byte[] HtmlZip{ get; set; }
		public byte[] Pdf { get; set; }
		public DateTime Created { get; set; }
		public string LatexFile { get; set; }
	}
}