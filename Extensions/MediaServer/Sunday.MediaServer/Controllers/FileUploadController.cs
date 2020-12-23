using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sunday.Core.Media.Application;
using Sunday.MediaServer.Attributes;

namespace Sunday.MediaServer.Controllers
{
    [ApiKeyAuthorized]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IApplicationMediaManager _applicationMediaManager;
        public FileUploadController(IApplicationMediaManager applicationMediaManager)
        {
            _applicationMediaManager = applicationMediaManager;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadInput input)
        {
            var result = await _applicationMediaManager.UploadBlob("images", input.File);
            return Ok(new { id = result.BlobIdentifier });
        }
    }

    public class FileUploadInput
    {
        public IFormFile File { get; set; }
    }
}
