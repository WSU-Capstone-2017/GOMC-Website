using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
	[Table("LatexUploads")]
	public class LatexUploadModel
	{
		public int Id { get; set; }
		public int AuthorId { get; set; }
		public string Version { get; set; }
		public string HtmlFile { get; set; }
		public string PdfFile { get; set; }
	}
}