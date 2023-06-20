namespace NewsWebMVC.ViewModels
{
    public class EditNewsViewModel:NewsBaseViewModel
    {
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }

    }
}
 