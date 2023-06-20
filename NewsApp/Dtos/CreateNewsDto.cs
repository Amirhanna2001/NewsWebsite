using NewsApp.Models;
using System.ComponentModel.DataAnnotations;

namespace NewsApp.Dtos
{
    public class CreateNewsDto:NewsBaseDto
    {
        
        public string ImagePath { get; set; }
    }
}
