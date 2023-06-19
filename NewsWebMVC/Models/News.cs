using System.ComponentModel.DataAnnotations;

namespace NewsWebMVC.Models
{
    public class News
    {
        public int Id { get; set; }
        public string title { get; set; }
        [DataType(DataType.MultilineText), MaxLength(500)]
        public string TheNews { get; set; }
        public IFormFile Image { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; } 
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
