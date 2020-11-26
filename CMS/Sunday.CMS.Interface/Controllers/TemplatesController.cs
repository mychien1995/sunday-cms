using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Templates;
using Sunday.ContentManagement.Models;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class TemplatesController : BaseController
    {
        private readonly IApplicationTemplateManager _templateManager;
        public TemplatesController(ISundayContext context, IApplicationTemplateManager templateManager, IOptions<MvcViewOptions> optionsAccessor) : base(context)
        {
            _templateManager = templateManager;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] TemplateQuery criteria)
        {
            var result = await _templateManager.Search(criteria);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TemplateItem data)
        {
            var result = await _templateManager.Create(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TemplateItem data)
        {
            var result = await _templateManager.Update(data);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await _templateManager.GetById(id);
            return Ok(result);
        }

        [HttpGet("getFields")]
        public async Task<IActionResult> GetFields([FromQuery] Guid id)
        {
            var result = await _templateManager.LoadTemplateFields(id);
            return Ok(result);
        }

        [HttpGet("getFieldTypes")]
        public IActionResult GetFieldTypes()
        {
            var result = _templateManager.GetFieldTypes();
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var result = await _templateManager.Delete(id);
            return Ok(result);
        }
    }
}
