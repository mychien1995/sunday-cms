using Microsoft.AspNetCore.Http;

namespace Sunday.CMS.Core.Models.Media
{
    public class FileUploadInput
    {
        public FileUploadInput(string directory, IFormFile fileUpload)
        {
            Directory = directory;
            FileUpload = fileUpload;
        }

        public string Directory { get; set; }
        public IFormFile FileUpload { get; set; }
    }
}
