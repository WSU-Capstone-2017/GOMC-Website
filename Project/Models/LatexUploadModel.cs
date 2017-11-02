using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
	[Table("LatexUploads")]
	public class LatexUploadModel
	{
		public int Id { get; set; }
		public int AuthorId { get; set; }
		public string Version { get; set; }
		public string Html { get; set; }
		public string Pdf { get; set; }
	}
}