using Microsoft.AspNetCore.Hosting;

namespace IntraVisionTestTask.Services
{
    public static class ImageUploadService
    {
        public static string SaveImage(IFormFile image, IWebHostEnvironment _webHostEnvironment)
        {
            if(image != null)
            {
                string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = GetUniqueFileName(image.FileName);
                string filePath = Path.Combine(uploads, uniqueFileName);
                image.CopyTo(new FileStream(filePath, FileMode.Create));
                return $"/images/{uniqueFileName}";
            }
            return "";
        }
        private static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
