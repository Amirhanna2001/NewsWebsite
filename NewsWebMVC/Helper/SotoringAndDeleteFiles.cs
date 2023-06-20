using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace NewsWebMVC.Helper
{
    public class SotoringAndDeleteFiles
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SotoringAndDeleteFiles(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string ProcessUploadedFile(IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "News");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public void DeleteImage(string image)
        {
            if (image != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "News", image);
                File.Delete(path);
            }
        }
    }
}
