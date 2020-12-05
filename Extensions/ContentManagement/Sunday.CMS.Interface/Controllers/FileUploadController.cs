using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Models.Media;
using Sunday.Core.Media.Application;
using System.Threading.Tasks;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class FileUploadController : BaseController
    {
        private readonly IApplicationMediaManager _applicationMediaManager;
        public FileUploadController(IApplicationMediaManager applicationMediaManager, ISundayContext context) : base(context)
        {
            _applicationMediaManager = applicationMediaManager;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadInput input)
        {
            var result = await _applicationMediaManager.UploadBlob(input.Directory, input.FileUpload);
            return Ok(result);
        }
    }
}
