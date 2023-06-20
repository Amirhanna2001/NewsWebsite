using NewsWebMVC.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewsWebMVC.ViewModels
{
    public class CreateNewsViewModel:NewsBaseViewModel 
    {
        public IFormFile Image { get; set; }

    }
}
