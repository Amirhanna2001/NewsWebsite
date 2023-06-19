using System.ComponentModel.DataAnnotations;

namespace NewsApp.Dtos
{
    public class AuthorDto
    {
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
    } 
}
