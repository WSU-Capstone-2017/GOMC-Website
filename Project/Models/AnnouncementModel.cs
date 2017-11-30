using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
	[Table("Announcements")]
	public class AnnouncementModel
	{
		public int Id { get; set; }
		public int AuthorId { get; set; }
		public string Content { get; set; }
		public DateTime Created { get; set; }

        public AnnouncementModel() { }
        public AnnouncementModel(string content)
        {
            Content = content;
        }
    }
}