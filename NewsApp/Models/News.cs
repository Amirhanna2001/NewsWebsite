using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace NewsApp.Models
{
    public class News
    {
        public int Id { get; set; }
        public string title { get; set; }
        [DataType(DataType.MultilineText),MaxLength(500)]
        public string TheNews { get; set; }
        public string ImagePath { get; set; }
        [Range(typeof(DateTime),
           "{0:yyyy-MM-dd}",
           "{1:yyyy-MM-dd}",
           ErrorMessage = "The DateColumn must be within today and a week from today.")]
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; } =DateTime.Now;
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
