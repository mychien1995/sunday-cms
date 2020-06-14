using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.Layout;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class LayoutController : BaseController
    {
        private readonly INavigationManager _navigationManager;

        public LayoutController(INavigationManager navigationManager)
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
