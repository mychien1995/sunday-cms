using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class LayoutController : BaseController
    {
        private readonly ILayoutManager _layoutManager;

        public LayoutController(ILayoutManager layoutManager, ISundayContext context) : base(context)
        {
            _layoutManager = layoutManager;
        }

        [HttpGet("getNavigation")]
        public async Task<IActionResult> GetNavigation()
        {
            var result = await _layoutManager.GetUserNavigation();
            return Ok(result);
        }

        [HttpGet("getLayout")]
        public IActionResult GetLayout()
        {
            var result = _layoutManager.GetLayout();
            return Ok(result);
        }
    }
}
