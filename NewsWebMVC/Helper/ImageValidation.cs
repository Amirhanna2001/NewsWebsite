namespace NewsWebMVC.Helper
{
    public class ImageValidation
    {
        public static long maxAllowedPosterSize = 1_048_576;
        public static List<string> allowedPosterExtentions = new() { ".jpg", ".png" };

        public static bool IsValidSize(IFormFile file)
             => file.Length <= maxAllowedPosterSize;

        public static bool IsAllowedExtention(IFormFile file)
            => allowedPosterExtentions.Contains(Path.GetExtension(file.FileName.ToLower()));
    }
}
