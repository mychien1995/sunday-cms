using Microsoft.AspNetCore.Http;

namespace Sunday.CMS.Core.Models.Media
{
    public class FileUploadInput
    {
        public string Directory { get; set; }
        public IFormFile FileUpload { get; set; }
    }
}
