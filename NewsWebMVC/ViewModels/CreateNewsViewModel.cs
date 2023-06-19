using NewsWebMVC.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewsWebMVC.ViewModels
{
    public class CreateNewsViewModel
    {
        public string title { get; set; }
        [DataType(DataType.MultilineText), MaxLength(500)]
        public string TheNews { get; set; }
        public IFormFile Image { get; set; }
        [DisplayName("Publication Date")]
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
        public IEnumerable<Author> Authors { get; set; }
    }
}
