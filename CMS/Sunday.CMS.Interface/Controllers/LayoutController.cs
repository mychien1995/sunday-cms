using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class LayoutController : BaseController
    {
        private readonly INavigationManager _navigationManager;

        public LayoutController(INavigationManager navigationManager, ISundayContext context) : base(context)
        {
            _navigationManager = navigationManager;
        }

        [HttpGet("getNavigation")]
        public async Task<IActionResult> GetNavigation()
        {
            var result = await _navigationManager.GetUserNavigation();
            return Ok(result);
        }
    }
}
