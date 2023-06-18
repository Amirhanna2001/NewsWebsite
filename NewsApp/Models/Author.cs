using System.ComponentModel.DataAnnotations;

namespace NewsApp.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Range(3,20)]
        public string Name { get; set; }

    }
}
