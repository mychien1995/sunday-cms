using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.Media;
using Sunday.CMS.Core.Models.Media;
using Sunday.Core.Media.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class FileUploadController : BaseController
    {
        private readonly IApplicationMediaManager _applicationMediaManager;
        public FileUploadController(IApplicationMediaManager applicationMediaManager)
        {
            _applicationMediaManager = applicationMediaManager;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]FileUploadInput input)
        {
            var result = await _applicationMediaManager.UploadBlob(input.Directory, input.FileUpload);
            return Ok(result);
        }
    }
}
