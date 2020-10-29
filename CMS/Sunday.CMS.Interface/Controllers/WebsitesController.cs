using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.ApplicationLayouts;
using Sunday.CMS.Core.Models.ApplicationWebsites;
using Sunday.ContentManagement.Models;
using Sunday.Foundation.Context;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Interface.Controllers
{
    public class WebsitesController : BaseController
    {
        private readonly IApplicationWebsiteManager _applicationWebsiteManager;
        public WebsitesController(IApplicationWebsiteManager websiteManager, ISundayContext context) : base(context)
        {
            _applicationWebsiteManager = websiteManager;
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] WebsiteQuery criteria)
        {
            var result = await _applicationWebsiteManager.Search(criteria);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] WebsiteMutationModel data)
        {
            var result = await _applicationWebsiteManager.Create(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] WebsiteMutationModel data)
        {
            var result = await _applicationWebsiteManager.Update(data);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await _applicationWebsiteManager.GetById(id);
            return Ok(result);
        }

        [HttpPut("activate")]
        public async Task<IActionResult> Activate([FromQuery] Guid id)
        {
            var result = await _applicationWebsiteManager.Activate(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var result = await _applicationWebsiteManager.Delete(id);
            return Ok(result);
        }
    }
}