using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.ApplicationLayouts;
using Sunday.Foundation.Context;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Interface.Controllers
{
    [Route("cms/api/appLayout")]
    public class ApplicationLayoutController : BaseController
    {
        private readonly IApplicationLayoutManager _applicationLayoutManager;
        public ApplicationLayoutController(IApplicationLayoutManager layoutManager, ISundayContext context) : base(context)
        {
            _applicationLayoutManager = layoutManager;
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] LayoutQuery criteria)
        {
            var result = await _applicationLayoutManager.SearchLayout(criteria);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] LayoutMutationModel data)
        {
            var result = await _applicationLayoutManager.CreateLayout(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] LayoutMutationModel data)
        {
            var result = await _applicationLayoutManager.UpdateLayout(data);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await _applicationLayoutManager.GetLayoutById(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var result = await _applicationLayoutManager.DeleteLayout(id);
            return Ok(result);
        }
    }
}
