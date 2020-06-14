using Microsoft.AspNetCore.Mvc;
using Sunday.Core;
using Sunday.FeatureAccess.Application;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class ModulesController : BaseController
    {
        private readonly IApplicationModuleManager _moduleManager;
        public ModulesController(IApplicationModuleManager moduleManager, ISundayContext context) : base(context)
        {
            _moduleManager = moduleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _moduleManager.GetModules();
            return Ok(result);
        }

        [HttpGet("getByOrganization")]
        public async Task<IActionResult> GetModules()
        {
            var result = await _moduleManager.GetOrganizationModules(CurrentOrganizationId);
            return Ok(result);
        }
    }
}
