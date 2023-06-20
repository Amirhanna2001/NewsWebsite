using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using NewsWebMVC.Models;

namespace NewsWebMVC.ViewModels
{
    public class NewsBaseViewModel
    {
        public string title { get; set; }
        [DataType(DataType.MultilineText), MaxLength(500)]
        public string TheNews { get; set; }
        [DisplayName("Publication Date")]
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
        public IEnumerable<Author>? Authors { get; set; }
    }
}
