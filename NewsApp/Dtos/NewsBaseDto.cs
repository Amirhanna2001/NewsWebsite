using System.ComponentModel.DataAnnotations;

namespace NewsApp.Dtos
{
    public class NewsBaseDto
    { 
        public string title { get; set; }
        [DataType(DataType.MultilineText), MaxLength(500)]
        public string TheNews { get; set; }

        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
    }
}
