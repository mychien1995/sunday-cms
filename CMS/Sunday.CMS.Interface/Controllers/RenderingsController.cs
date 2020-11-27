using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Contents;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Models;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class RenderingsController : BaseController
    {
        private readonly IRenderingManager _renderingManager;
        public RenderingsController(ISundayContext context, IRenderingManager renderingManager) : base(context)
        {
            _renderingManager = renderingManager;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] RenderingQuery criteria)
        {
            var result = await _renderingManager.Search(criteria);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RenderingJsonResult data)
        {
            var result = await _renderingManager.Create(data);
            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] RenderingJsonResult data)
        {
            var result = await _renderingManager.Update(data);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await _renderingManager.GetById(id);
            return Ok(result);
        }

        [HttpGet("getRenderingTypes")]
        public IActionResult GetRenderingTypes()
        {
            return Ok(new
            {
                data = new[] { RenderingTypes.PageRendering, RenderingTypes.ViewComponent }
            });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var result = await _renderingManager.Delete(id);
            return Ok(result);
        }
    }
}
